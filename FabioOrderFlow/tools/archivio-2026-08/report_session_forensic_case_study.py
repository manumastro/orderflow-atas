#!/usr/bin/env python3
"""Build a forensic single-session case study from v2 session observations."""

from __future__ import annotations

import argparse
import bisect
import csv
import gzip
import hashlib
import json
import math
from collections import Counter, defaultdict
from dataclasses import dataclass
from datetime import datetime, timedelta
from decimal import Decimal
from pathlib import Path
from statistics import median
from typing import Any

SCHEMA = "fof-session-observation-v2"
HORIZONS = (60, 120, 300)
ANCHOR_INTERVALS = (60, 300)
BUCKET_SECONDS = 300
SESSION_START_HOUR = 9
SESSION_START_MINUTE = 30

REQUIRED_EVENT_COLUMNS = {
    "eventId",
    "lastTickTimeUtc",
    "lastTickSessionTime",
    "direction",
    "tickVolumeMatchesTotal",
    "tickSize",
    "lastPrice",
    "location",
    "responseLastTicks",
    "returnedToFrozenPoc",
    "complete",
}


@dataclass(frozen=True)
class RawTrade:
    time: datetime
    session_time: datetime
    sequence: int
    price: Decimal
    volume: Decimal
    direction: str


def parse_time(value: str) -> datetime:
    return datetime.fromisoformat(value)


def decimal(value: Any) -> Decimal:
    return Decimal(str(value))


def number(value: Decimal | None) -> str:
    if value is None:
        return "n/a"
    return format(value, "f")


def one_decimal(value: Decimal | None) -> str:
    if value is None:
        return "n/a"
    return format(value.quantize(Decimal("0.1")), "f")


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def percentile(values: list[Decimal], fraction: float) -> Decimal | None:
    if not values:
        return None
    ordered = sorted(values)
    index = min(len(ordered) - 1, max(0, math.ceil(fraction * len(ordered)) - 1))
    return ordered[index]


def summarize(values: list[Decimal]) -> dict[str, str | int]:
    return {
        "count": len(values),
        "p10": number(percentile(values, 0.10)),
        "median": number(median(values)) if values else "n/a",
        "p90": number(percentile(values, 0.90)),
        "positive": sum(value > 0 for value in values),
        "negative": sum(value < 0 for value in values),
        "zero": sum(value == 0 for value in values),
    }


def read_snapshot(path: Path) -> tuple[list[RawTrade], dict[str, Any]]:
    raw_trades: list[RawTrade] = []
    notices: list[dict[str, Any]] = []
    metadata: dict[str, Any] = {
        "schema": SCHEMA,
        "snapshot": path.as_posix(),
        "recordCount": 0,
        "rawTradeCount": 0,
        "sessionNotices": notices,
    }

    with gzip.open(path, "rt", encoding="utf-8") as stream:
        for line in stream:
            observation = json.loads(line)
            if observation.get("schema") != SCHEMA:
                raise ValueError(f"Unexpected schema in snapshot: {observation.get('schema')}")
            metadata["recordCount"] += 1

            if observation.get("type") == "session-first-trade-observed":
                notices.append(observation)
                continue

            if observation.get("source") != "raw-trade":
                continue

            metadata["rawTradeCount"] += 1
            raw_trades.append(
                RawTrade(
                    parse_time(observation["time"]),
                    parse_time(observation["sessionTime"]),
                    int(observation["sequence"]),
                    decimal(observation["price"]),
                    decimal(observation["volume"]),
                    str(observation.get("direction", "")),
                )
            )

    if not raw_trades:
        raise ValueError("No raw-trade records were found in the v2 snapshot.")

    raw_trades.sort(key=lambda trade: (trade.time, trade.sequence))
    session_start = session_start_for(raw_trades[0].session_time)
    if not session_start <= raw_trades[0].session_time <= session_start + timedelta(seconds=1):
        raise ValueError(f"First raw trade is not aligned with session open: {raw_trades[0].session_time.isoformat()}")
    return raw_trades, metadata


def session_start_for(value: datetime) -> datetime:
    return value.replace(hour=SESSION_START_HOUR, minute=SESSION_START_MINUTE, second=0, microsecond=0)


def read_events(path: Path) -> tuple[list[dict[str, str]], list[dict[str, str]]]:
    with path.open("r", encoding="utf-8", newline="") as stream:
        reader = csv.DictReader(stream)
        missing = REQUIRED_EVENT_COLUMNS - set(reader.fieldnames or [])
        if missing:
            raise ValueError(f"Missing event CSV columns: {', '.join(sorted(missing))}")
        rows = list(reader)

    if not rows:
        raise ValueError("The event CSV is empty.")

    complete = [row for row in rows if row["complete"] == "true" and row["tickVolumeMatchesTotal"] == "true"]
    if not complete:
        raise ValueError("No complete tick-volume-matched events were found.")
    return rows, complete


def response_last_ticks(
    raw_times: list[datetime],
    raw_prices: list[Decimal],
    tick_size: Decimal,
    start_time: datetime,
    start_price: Decimal,
    seconds: int,
) -> Decimal | None:
    response_end = start_time + timedelta(seconds=seconds)
    if raw_times[-1] < response_end:
        return None

    future_start = bisect.bisect_right(raw_times, start_time)
    future_end = bisect.bisect_right(raw_times, response_end)
    if future_start >= future_end:
        return None
    return (raw_prices[future_end - 1] - start_price) / tick_size


def event_horizon_values(
    events: list[dict[str, str]],
    raw_times: list[datetime],
    raw_prices: list[Decimal],
    tick_size: Decimal,
    horizon: int,
) -> list[Decimal]:
    values: list[Decimal] = []
    for row in events:
        value = response_last_ticks(
            raw_times,
            raw_prices,
            tick_size,
            parse_time(row["lastTickTimeUtc"]),
            decimal(row["lastPrice"]),
            horizon,
        )
        if value is not None:
            values.append(value)
    return values


def grouped_event_summaries(
    events: list[dict[str, str]],
    field: str,
    raw_times: list[datetime],
    raw_prices: list[Decimal],
    tick_size: Decimal,
    session_start: datetime,
) -> dict[str, Any]:
    groups: dict[str, list[dict[str, str]]] = defaultdict(list)
    for row in events:
        groups[row[field]].append(row)

    result: dict[str, Any] = {}
    for key in sorted(groups):
        subset = groups[key]
        horizon_summaries = {
            str(horizon): summarize(event_horizon_values(subset, raw_times, raw_prices, tick_size, horizon))
            for horizon in HORIZONS
        }
        value: dict[str, Any] = {
            "count": len(subset),
            "responseLastTicks": horizon_summaries,
        }
        if field == "location":
            minutes = [
                Decimal(str((parse_time(row["lastTickSessionTime"]) - session_start).total_seconds())) / Decimal(60)
                for row in subset
            ]
            value["minutesFromOpen"] = {
                "p10": one_decimal(percentile(minutes, 0.10)),
                "median": one_decimal(median(minutes)) if minutes else "n/a",
                "p90": one_decimal(percentile(minutes, 0.90)),
            }
        result[key] = value
    return result


def build_anchors(
    raw_trades: list[RawTrade],
    raw_times: list[datetime],
    raw_session_times: list[datetime],
    raw_prices: list[Decimal],
    tick_size: Decimal,
    session_start: datetime,
) -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    last_session_time = raw_session_times[-1]

    for interval in ANCHOR_INTERVALS:
        target = session_start
        while target <= last_session_time:
            raw_index = bisect.bisect_left(raw_session_times, target)
            if raw_index >= len(raw_trades):
                break

            trade = raw_trades[raw_index]
            responses = {
                horizon: response_last_ticks(raw_times, raw_prices, tick_size, trade.time, trade.price, horizon)
                for horizon in HORIZONS
            }
            if all(value is None for value in responses.values()):
                break

            row = {
                "intervalSeconds": str(interval),
                "targetSessionTime": target.isoformat(timespec="seconds"),
                "actualSessionTime": trade.session_time.isoformat(timespec="microseconds"),
                "rawTimeUtc": trade.time.isoformat(timespec="microseconds"),
                "anchorPrice": number(trade.price),
            }
            for horizon in HORIZONS:
                row[f"responseLastTicks{horizon}"] = number(responses[horizon])
            rows.append(row)
            target += timedelta(seconds=interval)
    return rows


def anchor_summaries(anchor_rows: list[dict[str, str]]) -> dict[str, Any]:
    result: dict[str, Any] = {}
    for interval in ANCHOR_INTERVALS:
        interval_rows = [row for row in anchor_rows if row["intervalSeconds"] == str(interval)]
        result[str(interval)] = {
            str(horizon): summarize(
                [decimal(row[f"responseLastTicks{horizon}"]) for row in interval_rows if row[f"responseLastTicks{horizon}"] != "n/a"]
            )
            for horizon in HORIZONS
        }
    return result


def dominant(counter: Counter[str]) -> str:
    if not counter:
        return "n/a"
    key, count = counter.most_common(1)[0]
    return f"{key}:{count}"


def build_timeline(
    raw_trades: list[RawTrade],
    all_events: list[dict[str, str]],
    complete_events: list[dict[str, str]],
    raw_times: list[datetime],
    raw_session_times: list[datetime],
    raw_prices: list[Decimal],
    tick_size: Decimal,
    session_start: datetime,
) -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    bucket_start = session_start
    last_session_time = raw_session_times[-1]

    while bucket_start <= last_session_time:
        bucket_end = bucket_start + timedelta(seconds=BUCKET_SECONDS)
        raw_start = bisect.bisect_left(raw_session_times, bucket_start)
        raw_end = bisect.bisect_left(raw_session_times, bucket_end)
        if raw_start >= raw_end:
            bucket_start = bucket_end
            continue

        bucket_prices = raw_prices[raw_start:raw_end]
        bucket_volume = sum((trade.volume for trade in raw_trades[raw_start:raw_end]), Decimal(0))
        bucket_all_events = [row for row in all_events if bucket_start <= parse_time(row["lastTickSessionTime"]) < bucket_end]
        bucket_complete_events = [row for row in complete_events if bucket_start <= parse_time(row["lastTickSessionTime"]) < bucket_end]
        event_response_300 = [decimal(row["responseLastTicks"]) for row in bucket_complete_events]
        location_counts = Counter(row["location"] for row in bucket_complete_events)
        direction_counts = Counter(row["direction"] for row in bucket_complete_events)
        returned_to_poc = sum(row["returnedToFrozenPoc"] == "true" for row in bucket_complete_events)

        anchor_index = bisect.bisect_left(raw_session_times, bucket_start)
        anchor_300 = None
        if anchor_index < len(raw_trades):
            anchor = raw_trades[anchor_index]
            anchor_300 = response_last_ticks(raw_times, raw_prices, tick_size, anchor.time, anchor.price, 300)

        rows.append(
            {
                "bucketStartSessionTime": bucket_start.isoformat(timespec="seconds"),
                "bucketEndSessionTime": bucket_end.isoformat(timespec="seconds"),
                "rawTradeCount": str(raw_end - raw_start),
                "rawVolume": number(bucket_volume),
                "rawStartPrice": number(bucket_prices[0]),
                "rawLastPrice": number(bucket_prices[-1]),
                "rawNetTicks": number((bucket_prices[-1] - bucket_prices[0]) / tick_size),
                "rawRangeTicks": number((max(bucket_prices) - min(bucket_prices)) / tick_size),
                "baselineResponseLastTicks300": number(anchor_300),
                "allEventCount": str(len(bucket_all_events)),
                "completeEventCount": str(len(bucket_complete_events)),
                "eventMedianResponseLastTicks300": number(median(event_response_300)) if event_response_300 else "n/a",
                "eventP10ResponseLastTicks300": number(percentile(event_response_300, 0.10)),
                "eventP90ResponseLastTicks300": number(percentile(event_response_300, 0.90)),
                "eventPositive300": str(sum(value > 0 for value in event_response_300)),
                "eventNegative300": str(sum(value < 0 for value in event_response_300)),
                "eventZero300": str(sum(value == 0 for value in event_response_300)),
                "returnedToFrozenPocCount": str(returned_to_poc),
                "dominantLocation": dominant(location_counts),
                "dominantDirection": dominant(direction_counts),
            }
        )
        bucket_start = bucket_end
    return rows


def write_csv(rows: list[dict[str, str]], path: Path) -> None:
    if not rows:
        raise ValueError(f"No rows to write: {path}")
    with path.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=list(rows[0]))
        writer.writeheader()
        writer.writerows(rows)


def markdown_table(headers: list[str], rows: list[list[str]]) -> str:
    lines = ["| " + " | ".join(headers) + " |", "| " + " | ".join("---" for _ in headers) + " |"]
    lines.extend("| " + " | ".join(row) + " |" for row in rows)
    return "\n".join(lines)


def time_range(row: dict[str, str]) -> str:
    start = parse_time(row["bucketStartSessionTime"]).strftime("%H:%M")
    end = parse_time(row["bucketEndSessionTime"]).strftime("%H:%M")
    return f"{start}-{end}"


def compact_summary(summary: dict[str, Any], interval: int, horizon: int) -> dict[str, Any]:
    return summary["anchorSummaries"][str(interval)][str(horizon)]


def load_optional_json(path: Path | None) -> dict[str, Any] | None:
    if path is None:
        return None
    if not path.exists():
        raise ValueError(f"Optional JSON does not exist: {path}")
    return json.loads(path.read_text(encoding="utf-8"))


def write_report(
    summary: dict[str, Any],
    snapshot: Path,
    events_csv: Path,
    anchors_csv: Path,
    timeline_csv: Path,
    summary_json: Path,
    non_overlap_summary: Path | None,
    output_path: Path,
) -> None:
    event_rows = []
    for horizon in HORIZONS:
        values = summary["eventResponseSummaries"][str(horizon)]
        event_rows.append(
            [
                f"{horizon}s",
                str(values["count"]),
                str(values["p10"]),
                str(values["median"]),
                str(values["p90"]),
                f"{values['positive']} / {values['negative']} / {values['zero']}",
            ]
        )

    anchor_rows = []
    for interval in ANCHOR_INTERVALS:
        for horizon in HORIZONS:
            values = compact_summary(summary, interval, horizon)
            anchor_rows.append(
                [
                    f"ogni {interval}s",
                    f"{horizon}s",
                    str(values["count"]),
                    str(values["p10"]),
                    str(values["median"]),
                    str(values["p90"]),
                    f"{values['positive']} / {values['negative']} / {values['zero']}",
                ]
            )

    direction_rows = []
    for direction, values in summary["directionSummaries"].items():
        horizons = values["responseLastTicks"]
        direction_rows.append(
            [
                direction,
                str(values["count"]),
                str(horizons["60"]["median"]),
                str(horizons["120"]["median"]),
                str(horizons["300"]["median"]),
                f"{horizons['300']['positive']} / {horizons['300']['negative']} / {horizons['300']['zero']}",
            ]
        )

    location_rows = []
    for location, values in summary["locationSummaries"].items():
        horizons = values["responseLastTicks"]
        minutes = values.get("minutesFromOpen", {})
        location_rows.append(
            [
                location,
                str(values["count"]),
                str(minutes.get("median", "n/a")),
                str(horizons["60"]["median"]),
                str(horizons["120"]["median"]),
                str(horizons["300"]["median"]),
                f"{horizons['300']['positive']} / {horizons['300']['negative']} / {horizons['300']['zero']}",
            ]
        )

    timeline_rows = []
    for row in summary["timelineBuckets"]:
        timeline_rows.append(
            [
                time_range(row),
                row["rawNetTicks"],
                row["baselineResponseLastTicks300"],
                f"{row['allEventCount']} / {row['completeEventCount']}",
                row["eventMedianResponseLastTicks300"],
                f"{row['eventPositive300']} / {row['eventNegative300']} / {row['eventZero300']}",
                row["dominantLocation"],
                row["returnedToFrozenPocCount"],
            ]
        )

    non_overlap = summary.get("nonOverlapSummary") or {}
    non_overlap_text = "Non fornito."
    if non_overlap:
        categories = non_overlap.get("categories", {})
        non_overlap_text = (
            f"La selezione non sovrapposta ha lasciato `{non_overlap.get('selectedRows')}` finestre: "
            f"`{categories.get('above-all-pocs', {}).get('count', 0)}` sopra tutti i POC, "
            f"`{categories.get('at-poc', {}).get('count', 0)}` sul POC, "
            f"`{categories.get('below-all-pocs', {}).get('count', 0)}` sotto tutti i POC e "
            f"`{categories.get('between-tied-pocs', {}).get('count', 0)}` tra POC in parita'."
        )

    artifact_lines = [
        f"- Snapshot v2: `{snapshot.as_posix()}`",
        f"- SHA-256 snapshot: `{sha256(snapshot)}`",
        f"- Eventi: `{events_csv.as_posix()}`",
        f"- SHA-256 eventi: `{sha256(events_csv)}`",
        f"- Anchor temporali locali: `{anchors_csv.as_posix()}`",
        f"- SHA-256 anchor: `{sha256(anchors_csv)}`",
        f"- Timeline locale: `{timeline_csv.as_posix()}`",
        f"- SHA-256 timeline: `{sha256(timeline_csv)}`",
        f"- Sintesi locale: `{summary_json.as_posix()}`",
        f"- SHA-256 sintesi: `{sha256(summary_json)}`",
    ]
    if non_overlap_summary is not None:
        artifact_lines.append(f"- Sintesi non sovrapposta locale: `{non_overlap_summary.as_posix()}`")
        artifact_lines.append(f"- SHA-256 non sovrapposta: `{sha256(non_overlap_summary)}`")

    market = summary["marketPath"]
    anchors_60_300 = compact_summary(summary, 60, 300)
    anchors_300_300 = compact_summary(summary, 300, 300)
    event_300 = summary["eventResponseSummaries"]["300"]
    buy_300 = summary["directionSummaries"].get("Buy", {}).get("responseLastTicks", {}).get("300", {})
    sell_300 = summary["directionSummaries"].get("Sell", {}).get("responseLastTicks", {}).get("300", {})

    report = [
        "# Case Study Forense: Apertura NQ 2026-08-04",
        "",
        "## Stato",
        "",
        "```text",
        "Tipo:                 analisi offline su una sola apertura",
        "Sessione:             NQ US Cash 2026-08-04",
        f"Schema incluso:       {SCHEMA}",
        "Schema escluso:       fof-session-observation-v1",
        "Modello attivo:       nessuno",
        "Segnali / ordini:     nessuno",
        "```",
        "",
        "## Provenienza",
        "",
        *artifact_lines,
        "",
        "I CSV e JSON locali non sono inclusi in Git. Gli hash sopra identificano gli artefatti usati per questo report.",
        "",
        "## Metodo",
        "",
        "Il report confronta tre viste della stessa apertura: eventi `CumulativeTrade` completi, anchor temporali neutri e bucket fissi di cinque minuti. La risposta viene misurata a 60, 120 e 300 secondi. Gli anchor temporali non richiedono un evento: partono dal primo raw trade osservato a o dopo un orario fisso.",
        "",
        "Questa analisi non stima probabilita', non testa significativita' e non approva una regola. Serve a capire quanto delle risposte osservate sia spiegabile dal regime temporale della singola apertura.",
        "",
        "## Copertura",
        "",
        "```text",
        f"raw trade:                       {summary['rawTradeCount']}",
        f"record v2 snapshot:              {summary['snapshotRecordCount']}",
        f"primo raw America/New_York:      {summary['rawFirstSessionTime']}",
        f"ultimo raw America/New_York:     {summary['rawLastSessionTime']}",
        f"eventi finali nel CSV:           {summary['allEventCount']}",
        f"eventi completi e coerenti:      {summary['completeEventCount']}",
        f"primo evento completo:           {summary['firstCompleteEventSessionTime']}",
        f"ultimo evento completo:          {summary['lastCompleteEventSessionTime']}",
        f"prezzo apertura osservato:       {market['firstPrice']}",
        f"prezzo ultimo raw osservato:     {market['lastPrice']}",
        f"movimento netto osservato tick:  {market['netTicks']}",
        f"massimo da apertura tick:        {market['highFromOpenTicks']}",
        f"minimo da apertura tick:         {market['lowFromOpenTicks']}",
        "```",
        "",
        "La raccolta non copre la sessione cash intera: l'ultimo raw trade osservato e' alle `10:17:00` New York. Gli eventi dopo circa `10:12` non hanno un percorso futuro completo di 300 secondi e restano fuori dalle statistiche evento complete.",
        "",
        "## Baseline Temporale",
        "",
        markdown_table(
            ["anchor", "orizzonte", "n", "p10", "mediana", "p90", "+ / - / 0"],
            anchor_rows,
        ),
        "",
        "Gli anchor a cinque minuti sono il controllo piu' severo contro la sovrapposizione temporale. Nella finestra osservata hanno risposta a 300 secondi positiva in "
        f"`{anchors_300_300['positive']}` casi su `{anchors_300_300['count']}`. Gli anchor a un minuto hanno risposta a 300 secondi positiva in "
        f"`{anchors_60_300['positive']}` casi su `{anchors_60_300['count']}`. Questo conferma che la baseline dell'apertura era gia' inclinata al rialzo.",
        "",
        "## Eventi CumulativeTrade",
        "",
        markdown_table(
            ["orizzonte", "n", "p10", "mediana", "p90", "+ / - / 0"],
            event_rows,
        ),
        "",
        "A 300 secondi gli eventi completi sono positivi in "
        f"`{event_300['positive']}` casi su `{event_300['count']}`. Questo dato non e' indipendente: gli eventi sono molto densi e molte finestre future si sovrappongono.",
        "",
        "### Per Lato",
        "",
        markdown_table(
            ["lato", "n", "mediana 60s", "mediana 120s", "mediana 300s", "+ / - / 0 a 300s"],
            direction_rows,
        ),
        "",
        "Buy e Sell restano quasi sovrapposti: mediana a 300 secondi "
        f"`{buy_300.get('median', 'n/a')}` per Buy e `{sell_300.get('median', 'n/a')}` per Sell. In questa apertura il lato dell'evento non separa una lettura direzionale.",
        "",
        "### Per Location POC",
        "",
        markdown_table(
            ["location", "n", "minuti da open mediana", "mediana 60s", "mediana 120s", "mediana 300s", "+ / - / 0 a 300s"],
            location_rows,
        ),
        "",
        "Le location sono intrecciate col tempo. Gli eventi sotto tutti i POC hanno mediana temporale piu' vicina all'apertura rispetto agli eventi sopra tutti i POC, quindi la differenza di risposta non puo' essere letta come proprieta' autonoma della location.",
        "",
        "## Timeline A Cinque Minuti",
        "",
        markdown_table(
            ["fascia NY", "raw netto", "baseline +300", "eventi tutti/completi", "CT mediana +300", "CT + / - / 0", "location dominante", "ritorni POC"],
            timeline_rows,
        ),
        "",
        "La timeline mostra perche' il confronto grezzo puo' ingannare. Alcune fasce hanno baseline e risposta evento allineate, altre no, ma restano tutte parti dello stesso movimento di apertura. La fascia `10:15-10:20` contiene raw trade osservati ma non eventi completi, perche' manca la finestra futura di 300 secondi.",
        "",
        "## Controllo Non Sovrapposto",
        "",
        non_overlap_text,
        "",
        "Questo controllo riduce `57.534` eventi completi a poche finestre temporalmente distinte. E' la ragione principale per cui la singola apertura non puo' essere trasformata in probabilita' operative.",
        "",
        "## Cosa Si Puo' Dire",
        "",
        "1. L'apertura osservata e' stata fortemente direzionale: dal primo raw trade all'ultimo raw trade il prezzo sale di "
        f"`{market['netTicks']}` tick, con massimo a `{market['highFromOpenTicks']}` tick dall'apertura.",
        "2. La risposta positiva dopo molti `CumulativeTrade` e' compatibile con la baseline temporale rialzista. Gli eventi non bastano a isolare una causa.",
        "3. Il lato Buy/Sell non separa il comportamento futuro nella sessione osservata.",
        "4. La location rispetto al POC di sessione in sviluppo descrive dove si trova il prezzo nella distribuzione corrente, ma in questa apertura e' confusa con il momento della giornata e col trend gia' in corso.",
        "5. L'unico uso corretto di questo dataset e' come case study: timeline, anatomia degli eventi, limiti del recorder e ipotesi candidate. Non e' un set di validazione.",
        "",
        "## Ipotesi Candidate, Non Regole",
        "",
        "- In una apertura direzionale, `CumulativeTrade` puo' descrivere partecipazione nel movimento piu' che segnalare ritorno al POC.",
        "- Gli eventi sotto o sul POC durante questa apertura sembrano spesso coincidere con fasi precoci o pullback dentro una spinta rialzista; questa e' una lettura di contesto, non una regola di acquisto.",
        "- Per studiare assorbimento o rifiuto servirebbe un recorder del contesto precedente: POC/VAH/VAL della sessione passata, overnight range, apertura rispetto al valore e sviluppo dell'initial balance.",
        "",
        "## Conclusione",
        "",
        "Con i dati gia' registrati possiamo documentare bene una apertura e impedire letture premature. Non possiamo validare un modello. Il risultato pratico e' un frame di lavoro: prima baseline temporale e contesto d'asta, poi eventi di partecipazione; mai il contrario.",
        "",
    ]
    output_path.write_text("\n".join(report), encoding="utf-8")


def build_summary(
    raw_trades: list[RawTrade],
    metadata: dict[str, Any],
    all_events: list[dict[str, str]],
    complete_events: list[dict[str, str]],
    anchors: list[dict[str, str]],
    timeline: list[dict[str, str]],
    non_overlap_summary: dict[str, Any] | None,
) -> dict[str, Any]:
    raw_times = [trade.time for trade in raw_trades]
    raw_session_times = [trade.session_time for trade in raw_trades]
    raw_prices = [trade.price for trade in raw_trades]
    session_start = session_start_for(raw_trades[0].session_time)
    tick_size = decimal(complete_events[0]["tickSize"])

    event_response_summaries = {
        str(horizon): summarize(event_horizon_values(complete_events, raw_times, raw_prices, tick_size, horizon))
        for horizon in HORIZONS
    }
    market_path = {
        "firstPrice": number(raw_prices[0]),
        "lastPrice": number(raw_prices[-1]),
        "netTicks": number((raw_prices[-1] - raw_prices[0]) / tick_size),
        "highFromOpenTicks": number((max(raw_prices) - raw_prices[0]) / tick_size),
        "lowFromOpenTicks": number((min(raw_prices) - raw_prices[0]) / tick_size),
    }

    return {
        "schema": SCHEMA,
        "horizonsSeconds": list(HORIZONS),
        "anchorIntervalsSeconds": list(ANCHOR_INTERVALS),
        "bucketSeconds": BUCKET_SECONDS,
        "snapshotRecordCount": metadata["recordCount"],
        "rawTradeCount": len(raw_trades),
        "rawFirstUtc": raw_trades[0].time.isoformat(timespec="microseconds"),
        "rawLastUtc": raw_trades[-1].time.isoformat(timespec="microseconds"),
        "rawFirstSessionTime": raw_trades[0].session_time.isoformat(timespec="microseconds"),
        "rawLastSessionTime": raw_trades[-1].session_time.isoformat(timespec="microseconds"),
        "allEventCount": len(all_events),
        "completeEventCount": len(complete_events),
        "firstCompleteEventSessionTime": complete_events[0]["lastTickSessionTime"],
        "lastCompleteEventSessionTime": complete_events[-1]["lastTickSessionTime"],
        "marketPath": market_path,
        "eventResponseSummaries": event_response_summaries,
        "directionSummaries": grouped_event_summaries(complete_events, "direction", raw_times, raw_prices, tick_size, session_start),
        "locationSummaries": grouped_event_summaries(complete_events, "location", raw_times, raw_prices, tick_size, session_start),
        "anchorSummaries": anchor_summaries(anchors),
        "timelineBuckets": timeline,
        "nonOverlapSummary": non_overlap_summary,
        "sessionNotice": metadata["sessionNotices"][0] if metadata["sessionNotices"] else None,
    }


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--snapshot", type=Path, required=True)
    parser.add_argument("--events-csv", type=Path, required=True)
    parser.add_argument("--anchors-csv", type=Path, required=True)
    parser.add_argument("--timeline-csv", type=Path, required=True)
    parser.add_argument("--summary-json", type=Path, required=True)
    parser.add_argument("--report", type=Path, required=True)
    parser.add_argument("--non-overlap-summary-json", type=Path)
    args = parser.parse_args()

    raw_trades, metadata = read_snapshot(args.snapshot)
    all_events, complete_events = read_events(args.events_csv)
    raw_times = [trade.time for trade in raw_trades]
    raw_session_times = [trade.session_time for trade in raw_trades]
    raw_prices = [trade.price for trade in raw_trades]
    session_start = session_start_for(raw_trades[0].session_time)
    tick_size = decimal(complete_events[0]["tickSize"])

    anchors = build_anchors(raw_trades, raw_times, raw_session_times, raw_prices, tick_size, session_start)
    timeline = build_timeline(
        raw_trades,
        all_events,
        complete_events,
        raw_times,
        raw_session_times,
        raw_prices,
        tick_size,
        session_start,
    )
    write_csv(anchors, args.anchors_csv)
    write_csv(timeline, args.timeline_csv)

    non_overlap_summary = load_optional_json(args.non_overlap_summary_json)
    summary = build_summary(raw_trades, metadata, all_events, complete_events, anchors, timeline, non_overlap_summary)
    args.summary_json.write_text(json.dumps(summary, indent=2, sort_keys=True) + "\n", encoding="utf-8")
    write_report(
        summary,
        args.snapshot,
        args.events_csv,
        args.anchors_csv,
        args.timeline_csv,
        args.summary_json,
        args.non_overlap_summary_json,
        args.report,
    )


if __name__ == "__main__":
    main()
