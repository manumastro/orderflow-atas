#!/usr/bin/env python3
"""Build the session-location descriptive report from ATAS observation logs.

The script only accepts fof-session-observation-v2 records. It snapshots those
records before deriving the event table, so the analysis remains reproducible
without committing a large ATAS application log to Git.
"""

from __future__ import annotations

import argparse
import bisect
import csv
import gzip
import hashlib
import heapq
import json
import math
import os
from collections import Counter, defaultdict
from dataclasses import dataclass
from datetime import datetime, timedelta
from decimal import Decimal
from pathlib import Path
from statistics import median
from typing import Any

SCHEMA = "fof-session-observation-v2"
MARKER = "FofSessionObservation "
RESPONSE_SECONDS = 300
SESSION_START = "09:30:00"


@dataclass(frozen=True)
class RawTrade:
    time: datetime
    session_time: datetime
    sequence: int
    price: Decimal
    volume: Decimal
    direction: str


@dataclass(frozen=True)
class FinalEvent:
    event_id: int
    payload: dict[str, Any]
    last_tick_time: datetime
    event_session_time: datetime
    total_volume: Decimal
    update_number: int


def decimal(value: Any) -> Decimal:
    return Decimal(str(value))


def parse_time(value: str) -> datetime:
    return datetime.fromisoformat(value)


def iso(value: datetime | None) -> str:
    return "" if value is None else value.isoformat(timespec="microseconds")


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def parse_observations(source_log: Path, snapshot_path: Path) -> tuple[list[RawTrade], dict[int, FinalEvent], dict[str, Any]]:
    raw_trades: list[RawTrade] = []
    final_events: dict[int, FinalEvent] = {}
    metadata: dict[str, Any] = {
        "sourceLog": str(source_log.resolve()),
        "sourceLogBytesAtReadStart": source_log.stat().st_size,
        "schema": SCHEMA,
        "recordCount": 0,
        "rawTradeCount": 0,
        "cumulativeNewCount": 0,
        "cumulativeUpdateCount": 0,
        "sessionNotices": [],
        "skippedMalformedLines": 0,
    }

    snapshot_path.parent.mkdir(parents=True, exist_ok=True)
    with source_log.open("rt", encoding="utf-8", errors="replace") as source, gzip.open(
        snapshot_path, "wt", encoding="utf-8", newline="\n"
    ) as snapshot:
        for line in source:
            marker_index = line.find(MARKER)
            if marker_index < 0:
                continue

            try:
                observation = json.loads(line[marker_index + len(MARKER) :])
            except json.JSONDecodeError:
                metadata["skippedMalformedLines"] += 1
                continue

            if observation.get("schema") != SCHEMA:
                continue

            snapshot.write(json.dumps(observation, separators=(",", ":"), ensure_ascii=True))
            snapshot.write("\n")
            metadata["recordCount"] += 1

            source_type = observation.get("source")
            if observation.get("type") == "session-first-trade-observed":
                metadata["sessionNotices"].append(observation)
                continue

            if source_type == "raw-trade":
                metadata["rawTradeCount"] += 1
                raw_trades.append(
                    RawTrade(
                        parse_time(observation["time"]),
                        parse_time(observation["sessionTime"]),
                        int(observation["sequence"]),
                        decimal(observation["price"]),
                        decimal(observation["volume"]),
                        str(observation["direction"]),
                    )
                )
                continue

            if source_type not in {"cumulative-new", "cumulative-update"}:
                continue

            if source_type == "cumulative-new":
                metadata["cumulativeNewCount"] += 1
            else:
                metadata["cumulativeUpdateCount"] += 1

            event_id = int(observation["eventId"])
            candidate = FinalEvent(
                event_id,
                observation,
                parse_time(observation["lastTickTime"]),
                parse_time(observation["eventSessionTime"]),
                decimal(observation["totalVolume"]),
                int(observation["updateNumber"]),
            )
            current = final_events.get(event_id)
            if current is None or event_sort_key(candidate) > event_sort_key(current):
                final_events[event_id] = candidate

    raw_trades.sort(key=lambda trade: (trade.time, trade.sequence))
    return raw_trades, final_events, metadata


def event_sort_key(event: FinalEvent) -> tuple[Decimal, datetime, int]:
    return (event.total_volume, event.last_tick_time, event.update_number)


def add_profile_trade(
    trade: RawTrade,
    profile: dict[Decimal, list[Decimal]],
    totals: dict[Decimal, set[Decimal]],
    heap: list[Decimal],
) -> None:
    values = profile.get(trade.price)
    if values is None:
        values = [Decimal(0), Decimal(0), Decimal(0)]  # bid, ask, total
        profile[trade.price] = values
    else:
        old_total = values[2]
        bucket = totals[old_total]
        bucket.remove(trade.price)
        if not bucket:
            del totals[old_total]

    if trade.direction == "Buy":
        values[1] += trade.volume
    elif trade.direction == "Sell":
        values[0] += trade.volume
    values[2] += trade.volume
    totals[values[2]].add(trade.price)
    heapq.heappush(heap, -values[2])


def current_pocs(totals: dict[Decimal, set[Decimal]], heap: list[Decimal]) -> tuple[Decimal, list[Decimal]]:
    while heap:
        total = -heap[0]
        prices = totals.get(total)
        if prices:
            return total, sorted(prices)
        heapq.heappop(heap)
    return Decimal(0), []


def classify_location(last_price: Decimal, pocs: list[Decimal]) -> str:
    if not pocs:
        return "missing-poc"
    if last_price in pocs:
        return "at-poc"
    if last_price > max(pocs):
        return "above-all-pocs"
    if last_price < min(pocs):
        return "below-all-pocs"
    return "between-tied-pocs"


def segment_tree(values: list[Decimal]) -> tuple[int, list[Decimal], list[Decimal]]:
    size = 1
    while size < len(values):
        size *= 2
    mins = [Decimal("Infinity")] * (size * 2)
    maxs = [Decimal("-Infinity")] * (size * 2)
    for index, value in enumerate(values):
        mins[size + index] = value
        maxs[size + index] = value
    for index in range(size - 1, 0, -1):
        mins[index] = min(mins[index * 2], mins[index * 2 + 1])
        maxs[index] = max(maxs[index * 2], maxs[index * 2 + 1])
    return size, mins, maxs


def range_min_max(tree: tuple[int, list[Decimal], list[Decimal]], left: int, right: int) -> tuple[Decimal, Decimal]:
    size, mins, maxs = tree
    minimum = Decimal("Infinity")
    maximum = Decimal("-Infinity")
    left += size
    right += size
    while left < right:
        if left & 1:
            minimum = min(minimum, mins[left])
            maximum = max(maximum, maxs[left])
            left += 1
        if right & 1:
            right -= 1
            minimum = min(minimum, mins[right])
            maximum = max(maximum, maxs[right])
        left //= 2
        right //= 2
    return minimum, maximum


def has_price_in_range(indices: dict[Decimal, list[int]], price: Decimal, left: int, right: int) -> bool:
    positions = indices.get(price)
    if not positions:
        return False
    index = bisect.bisect_left(positions, left)
    return index < len(positions) and positions[index] < right


def number(value: Decimal | None) -> str:
    if value is None:
        return ""
    return format(value, "f")


def percentile(values: list[Decimal], fraction: float) -> Decimal | None:
    if not values:
        return None
    ordered = sorted(values)
    return ordered[min(len(ordered) - 1, max(0, math.ceil(fraction * len(ordered)) - 1))]


def summarize(values: list[Decimal]) -> dict[str, str | int]:
    return {
        "count": len(values),
        "median": number(median(values)) if values else "",
        "p10": number(percentile(values, 0.10)),
        "p90": number(percentile(values, 0.90)),
    }


def derive_events(raw_trades: list[RawTrade], final_events: dict[int, FinalEvent], metadata: dict[str, Any]) -> tuple[list[dict[str, Any]], dict[str, Any]]:
    if not raw_trades:
        raise ValueError("No v2 raw trades were found.")

    raw_times = [trade.time for trade in raw_trades]
    raw_prices = [trade.price for trade in raw_trades]
    price_indices: dict[Decimal, list[int]] = defaultdict(list)
    for index, trade in enumerate(raw_trades):
        price_indices[trade.price].append(index)
    price_tree = segment_tree(raw_prices)

    events = sorted(final_events.values(), key=lambda event: (event.last_tick_time, event.event_id))
    profile: dict[Decimal, list[Decimal]] = {}
    totals: dict[Decimal, set[Decimal]] = defaultdict(set)
    heap: list[Decimal] = []
    raw_index = 0
    rows: list[dict[str, Any]] = []

    session_start = raw_trades[0].session_time.replace(hour=9, minute=30, second=0, microsecond=0)
    profile_started_on_time = raw_trades[0].session_time <= session_start + timedelta(seconds=1)
    tick_volume_mismatches = 0
    incomplete_reasons: Counter[str] = Counter()

    for event in events:
        while raw_index < len(raw_trades) and raw_trades[raw_index].time <= event.last_tick_time:
            add_profile_trade(raw_trades[raw_index], profile, totals, heap)
            raw_index += 1

        payload = event.payload
        poc_volume, pocs = current_pocs(totals, heap)
        ticks = payload.get("ticks", [])
        tick_volume = sum((decimal(tick["volume"]) for tick in ticks), Decimal(0))
        total_volume = decimal(payload["totalVolume"])
        volume_matches = tick_volume == total_volume
        if not volume_matches:
            tick_volume_mismatches += 1

        security = payload.get("security") or {}
        tick_size = decimal(security.get("tickSize") or "0.25")
        last_price = decimal(payload["lastPrice"])
        distances = [(last_price - price) / tick_size for price in pocs]

        future_start = bisect.bisect_right(raw_times, event.last_tick_time)
        response_end = event.last_tick_time + timedelta(seconds=RESPONSE_SECONDS)
        future_end = bisect.bisect_right(raw_times, response_end)
        future_exists = future_start < future_end
        complete = profile_started_on_time and raw_trades[-1].time >= response_end and future_exists
        reason = ""
        if not profile_started_on_time:
            reason = "profile-started-after-session-open"
        elif raw_trades[-1].time < response_end:
            reason = "response-window-not-yet-observed"
        elif not future_exists:
            reason = "no-future-trades-in-window"
        if reason:
            incomplete_reasons[reason] += 1

        if future_exists:
            future_min, future_max = range_min_max(price_tree, future_start, future_end)
            future_first = raw_prices[future_start]
            future_last = raw_prices[future_end - 1]
            returned_to_poc = any(has_price_in_range(price_indices, price, future_start, future_end) for price in pocs)
        else:
            future_min = future_max = future_first = future_last = None
            returned_to_poc = False

        rows.append(
            {
                "eventId": event.event_id,
                "source": payload.get("source", ""),
                "updateNumber": event.update_number,
                "eventTimeUtc": payload.get("eventTime", ""),
                "eventSessionTime": payload.get("eventSessionTime", ""),
                "firstTickTimeUtc": payload.get("firstTickTime", ""),
                "lastTickTimeUtc": payload.get("lastTickTime", ""),
                "lastTickSessionTime": payload.get("lastTickSessionTime", ""),
                "direction": payload.get("direction", ""),
                "totalVolume": number(total_volume),
                "tickCount": len(ticks),
                "tickVolume": number(tick_volume),
                "tickVolumeMatchesTotal": str(volume_matches).lower(),
                "securityId": security.get("securityId", ""),
                "connectorId": security.get("connectorId", ""),
                "code": security.get("code", ""),
                "exchange": security.get("exchange", ""),
                "tickSize": number(tick_size),
                "profilePocVolume": number(poc_volume),
                "profilePocPrices": ";".join(number(price) for price in pocs),
                "profilePocTie": str(len(pocs) > 1).lower(),
                "profilePocCount": len(pocs),
                "lastPrice": number(last_price),
                "pocDistanceTicks": ";".join(number(distance) for distance in distances),
                "minimumAbsolutePocDistanceTicks": number(min((abs(distance) for distance in distances), default=None)),
                "location": classify_location(last_price, pocs),
                "responseEndUtc": iso(response_end),
                "responseFirstPrice": number(future_first),
                "responseMaxPrice": number(future_max),
                "responseMinPrice": number(future_min),
                "responseLastPrice": number(future_last),
                "responseUpTicks": number((future_max - last_price) / tick_size if future_max is not None else None),
                "responseDownTicks": number((future_min - last_price) / tick_size if future_min is not None else None),
                "responseLastTicks": number((future_last - last_price) / tick_size if future_last is not None else None),
                "returnedToFrozenPoc": str(returned_to_poc).lower(),
                "complete": str(complete).lower(),
                "incompleteReason": reason,
            }
        )

    complete_rows = [row for row in rows if row["complete"] == "true" and row["tickVolumeMatchesTotal"] == "true"]
    location_counts = Counter(row["location"] for row in complete_rows)
    direction_counts = Counter(row["direction"] for row in complete_rows)
    return_counts = Counter(row["returnedToFrozenPoc"] for row in complete_rows)
    tied_poc_count = sum(row["profilePocTie"] == "true" for row in complete_rows)

    direction_summaries: dict[str, Any] = {}
    for direction in sorted(direction_counts):
        subset = [row for row in complete_rows if row["direction"] == direction]
        direction_summaries[direction] = {
            "count": len(subset),
            "medianMinimumAbsolutePocDistanceTicks": number(median([decimal(row["minimumAbsolutePocDistanceTicks"]) for row in subset])),
            "medianResponseUpTicks": number(median([decimal(row["responseUpTicks"]) for row in subset])),
            "medianResponseDownTicks": number(median([decimal(row["responseDownTicks"]) for row in subset])),
            "returnedToFrozenPocCount": sum(row["returnedToFrozenPoc"] == "true" for row in subset),
        }

    summary = {
        "schema": SCHEMA,
        "responseSeconds": RESPONSE_SECONDS,
        "rawTrades": len(raw_trades),
        "rawFirstUtc": iso(raw_trades[0].time),
        "rawLastUtc": iso(raw_trades[-1].time),
        "rawFirstSessionTime": iso(raw_trades[0].session_time),
        "rawLastSessionTime": iso(raw_trades[-1].session_time),
        "uniqueFinalEvents": len(events),
        "completeEvents": len(complete_rows),
        "incompleteEvents": len(rows) - len(complete_rows),
        "incompleteReasons": dict(sorted(incomplete_reasons.items())),
        "tickVolumeMismatches": tick_volume_mismatches,
        "profileStartedOnTime": profile_started_on_time,
        "locationCounts": dict(sorted(location_counts.items())),
        "returnToFrozenPocCounts": dict(sorted(return_counts.items())),
        "tiedPocEvents": tied_poc_count,
        "directionSummaries": direction_summaries,
        "absolutePocDistanceTicks": summarize([decimal(row["minimumAbsolutePocDistanceTicks"]) for row in complete_rows]),
        "responseUpTicks": summarize([decimal(row["responseUpTicks"]) for row in complete_rows]),
        "responseDownTicks": summarize([decimal(row["responseDownTicks"]) for row in complete_rows]),
        "responseLastTicks": summarize([decimal(row["responseLastTicks"]) for row in complete_rows]),
        "sessionNotice": metadata["sessionNotices"][0] if metadata["sessionNotices"] else None,
    }
    return rows, summary


def write_csv(rows: list[dict[str, Any]], path: Path) -> None:
    if not rows:
        raise ValueError("No final events were found.")
    with path.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=list(rows[0]))
        writer.writeheader()
        writer.writerows(rows)


def write_markdown(summary: dict[str, Any], metadata: dict[str, Any], snapshot_path: Path, csv_path: Path, output_path: Path) -> None:
    notice = summary.get("sessionNotice") or {}
    session = notice.get("session") or {}
    output_path.write_text(
        "\n".join(
            [
                "# Descrizione Di Sessione: Location POC E Risposta Prezzo",
                "",
                "## Stato",
                "",
                "```text",
                "Tipo:                 report descrittivo offline",
                "Sessione:             NQ US Cash 2026-08-04",
                f"Schema incluso:       {SCHEMA}",
                "Schema escluso:       fof-session-observation-v1",
                "Modello attivo:       nessuno",
                "Segnali / ordini:     nessuno",
                "```",
                "",
                "## Provenienza",
                "",
                f"- Snapshot locale: `{snapshot_path.as_posix()}`",
                f"- SHA-256 snapshot: `{sha256(snapshot_path)}`",
                f"- Tabella evento: `{csv_path.as_posix()}`",
                f"- SHA-256 tabella: `{sha256(csv_path)}`",
                f"- Log ATAS sorgente: `{metadata['sourceLog']}`",
                f"- Byte del log al primo accesso: `{metadata['sourceLogBytesAtReadStart']}`",
                f"- Record v2 nello snapshot: `{metadata['recordCount']}`",
                f"- Righe ATAS malformate escluse: `{metadata['skippedMalformedLines']}`",
                "",
                "Il log ATAS completo non e' incluso in Git. Lo snapshot compresso contiene soltanto i JSON v2 usati dal report; i file v1 restano esclusi.",
                "",
                "## Copertura E Integrita'",
                "",
                "```text",
                f"session id:                 {session.get('sessionId', '')}",
                f"primo raw UTC:              {summary['rawFirstUtc']}",
                f"primo raw America/New_York: {summary['rawFirstSessionTime']}",
                f"ultimo raw America/New_York:{summary['rawLastSessionTime']}",
                f"raw trade:                  {summary['rawTrades']}",
                f"eventi finali unici:        {summary['uniqueFinalEvents']}",
                f"eventi completi:            {summary['completeEvents']}",
                f"eventi incompleti:          {summary['incompleteEvents']}",
                f"mismatch volume tick:       {summary['tickVolumeMismatches']}",
                f"profilo avviato in orario:  {summary['profileStartedOnTime']}",
                "```",
                "",
                "Un evento e' completo soltanto quando la somma dei suoi tick coincide con il volume finale, il profilo parte entro le 09:30 e sono osservabili raw trade fino a 300 secondi dopo il tick finale. Gli eventi nell'ultima finestra di cinque minuti restano nel CSV ma non nelle statistiche sotto.",
                "",
                "## Descrizione Congiunta",
                "",
                "```text",
                f"location:                   {json.dumps(summary['locationCounts'], sort_keys=True)}",
                f"ritorno a POC congelato:    {json.dumps(summary['returnToFrozenPocCounts'], sort_keys=True)}",
                f"POC con parita':            {summary['tiedPocEvents']}",
                f"distanza assoluta POC tick: {json.dumps(summary['absolutePocDistanceTicks'], sort_keys=True)}",
                f"escursione up tick:         {json.dumps(summary['responseUpTicks'], sort_keys=True)}",
                f"escursione down tick:       {json.dumps(summary['responseDownTicks'], sort_keys=True)}",
                f"prezzo finale futuro tick:  {json.dumps(summary['responseLastTicks'], sort_keys=True)}",
                "```",
                "",
                "## Per Lato Dell'Evento",
                "",
                "```json",
                json.dumps(summary['directionSummaries'], indent=2, sort_keys=True),
                "```",
                "",
                "Queste sono distribuzioni descrittive della singola raccolta. Non definiscono assorbimento, accettazione, rifiuto, probabilita', filtro o modello; non sono base per segnali o ordini.",
                "",
                "## Metodo Riproducibile",
                "",
                "1. Il parser filtra esclusivamente `fof-session-observation-v2`.",
                "2. Per ogni EventId seleziona lo stato finale per volume totale massimo, poi ultimo tick, poi update maggiore.",
                "3. Ordina i raw trade UTC per tempo e sequenza; aggiorna il profilo solo fino all'ultimo tick dell'evento.",
                "4. Conserva tutti i POC a parita' e calcola il vettore delle distanze in tick.",
                "5. Misura il primo, massimo, minimo e ultimo raw trade nei 300 secondi strettamente successivi al tick finale.",
                "",
            ]
        ),
        encoding="utf-8",
    )


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--source-log", type=Path, required=True)
    parser.add_argument("--snapshot", type=Path, required=True)
    parser.add_argument("--events-csv", type=Path, required=True)
    parser.add_argument("--summary-json", type=Path, required=True)
    parser.add_argument("--report", type=Path, required=True)
    args = parser.parse_args()

    raw_trades, final_events, metadata = parse_observations(args.source_log, args.snapshot)
    rows, summary = derive_events(raw_trades, final_events, metadata)
    write_csv(rows, args.events_csv)
    args.summary_json.write_text(json.dumps({"metadata": metadata, "summary": summary}, indent=2, default=str) + "\n", encoding="utf-8")
    write_markdown(summary, metadata, args.snapshot, args.events_csv, args.report)


if __name__ == "__main__":
    main()
