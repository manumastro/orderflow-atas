#!/usr/bin/env python3
"""Build a reproducible historical cumulative context report from ATAS logs."""

from __future__ import annotations

import argparse
import csv
import gzip
import hashlib
import json
import math
from collections import Counter, defaultdict
from dataclasses import dataclass, field
from datetime import datetime
from decimal import Decimal
from pathlib import Path
from statistics import mean, median
from typing import Any

DEFAULT_SCHEMA = "fof-historical-cumulative-context-v4"
MARKER = "FofHistoricalContext "

CANDLE_COLUMNS = [
    "rangeId",
    "bar",
    "beginTime",
    "endTime",
    "open",
    "high",
    "low",
    "close",
    "volume",
    "bid",
    "ask",
    "delta",
    "ticks",
    "vwap",
    "valueAreaHigh",
    "valueAreaLow",
    "pocPrice",
    "pocVolume",
    "pocAsk",
    "pocBid",
    "priceLevelCount",
]

EVENT_COLUMNS = [
    "rangeId",
    "requestId",
    "requestSequence",
    "requestCount",
    "requestBeginTime",
    "requestEndTime",
    "eventId",
    "time",
    "timeRelationToRequest",
    "direction",
    "totalVolume",
    "firstPrice",
    "lastPrice",
    "priceChangeTicks",
    "tickCount",
    "tickVolume",
    "tickVolumeMatchesTotal",
    "firstTickTime",
    "lastTickTime",
    "firstTickPrice",
    "lastTickPrice",
    "intraTradeLow",
    "intraTradeHigh",
]

VOLUME_BUCKETS = [
    (Decimal("1"), "1"),
    (Decimal("2"), "2-4"),
    (Decimal("5"), "5-9"),
    (Decimal("10"), "10-24"),
    (Decimal("25"), "25-49"),
    (Decimal("50"), "50-99"),
    (Decimal("100"), "100-249"),
    (Decimal("250"), "250+"),
]


@dataclass
class RangeScan:
    range_id: str
    range_payload: dict[str, Any] | None = None
    started: dict[str, Any] | None = None
    started_line: int = 0
    requests: dict[int, dict[str, Any]] = field(default_factory=dict)
    responses: dict[int, dict[str, Any]] = field(default_factory=dict)
    latest_response_line: int = 0

    @property
    def expected_request_count(self) -> int | None:
        counts = [int(item.get("requestCount", 0)) for item in [*self.requests.values(), *self.responses.values()]]
        counts = [count for count in counts if count > 0]
        return max(counts) if counts else None

    @property
    def complete(self) -> bool:
        expected = self.expected_request_count
        return bool(self.started and expected and len(self.responses) >= expected)


class DecimalEncoder(json.JSONEncoder):
    def default(self, obj: Any) -> Any:
        if isinstance(obj, Decimal):
            return format(obj, "f")
        if isinstance(obj, datetime):
            return obj.isoformat()
        return super().default(obj)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--source-log", required=True, type=Path)
    parser.add_argument("--snapshot", required=True, type=Path)
    parser.add_argument("--candles-csv", required=True, type=Path)
    parser.add_argument("--events-csv", required=True, type=Path)
    parser.add_argument("--summary-json", required=True, type=Path)
    parser.add_argument("--report", required=True, type=Path)
    parser.add_argument("--schema", default=DEFAULT_SCHEMA)
    parser.add_argument("--range-id")
    return parser.parse_args()


def parse_time(value: str) -> datetime:
    return datetime.fromisoformat(value)


def decimal(value: Any) -> Decimal:
    if value is None or value == "":
        return Decimal(0)
    return Decimal(str(value))


def number(value: Decimal | float | int | None) -> str:
    if value is None:
        return "n/a"
    if isinstance(value, Decimal):
        return format(value, "f")
    if isinstance(value, float):
        if not math.isfinite(value):
            return "n/a"
        return f"{value:.2f}"
    return str(value)


def fmt_int(value: int) -> str:
    return f"{value:,}".replace(",", ".")


def fmt_decimal(value: Decimal | int | float | None) -> str:
    if value is None:
        return "n/a"
    if isinstance(value, Decimal):
        text = format(value, "f")
    elif isinstance(value, float):
        text = f"{value:.2f}"
    else:
        text = str(value)
    if "." in text:
        whole, fractional = text.split(".", 1)
        fractional = fractional.rstrip("0")
        text = whole if not fractional else f"{whole}.{fractional}"
    return text


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def observation_from_line(line: str, schema: str) -> dict[str, Any] | None:
    marker_index = line.find(MARKER)
    if marker_index < 0:
        return None

    if f'"schema":"{schema}"' not in line and f'"schema": "{schema}"' not in line:
        return None

    try:
        observation = json.loads(line[marker_index + len(MARKER) :])
    except json.JSONDecodeError:
        return None

    return observation if observation.get("schema") == schema else None


def scan_ranges(source_log: Path, schema: str) -> tuple[dict[str, RangeScan], int]:
    ranges: dict[str, RangeScan] = {}
    malformed = 0

    with source_log.open("rt", encoding="utf-8", errors="replace") as stream:
        for line_number, line in enumerate(stream, 1):
            if MARKER not in line or f'"schema":"{schema}"' not in line:
                continue
            if '"type":"' not in line and '"type": "' not in line:
                continue

            observation = observation_from_line(line, schema)
            if observation is None:
                malformed += 1
                continue

            range_payload = observation.get("range") or {}
            range_id = range_payload.get("rangeId")
            if not range_id:
                continue

            state = ranges.setdefault(str(range_id), RangeScan(str(range_id)))
            state.range_payload = range_payload or state.range_payload

            record_type = observation.get("type")
            if record_type == "historical-context-started":
                state.started = observation
                state.started_line = line_number
            elif record_type == "historical-cumulative-requested":
                state.requests[int(observation["requestSequence"])] = observation
            elif record_type == "historical-cumulative-response":
                state.responses[int(observation["requestSequence"])] = observation
                state.latest_response_line = line_number

    return ranges, malformed


def select_range(ranges: dict[str, RangeScan], requested_range_id: str | None) -> RangeScan:
    if requested_range_id:
        selected = ranges.get(requested_range_id)
        if selected is None:
            raise ValueError(f"Range id was not found: {requested_range_id}")
        if not selected.complete:
            raise ValueError(f"Range id is not complete: {requested_range_id}")
        return selected

    complete = [state for state in ranges.values() if state.complete]
    if not complete:
        raise ValueError("No complete historical cumulative context range was found.")
    return max(complete, key=lambda state: (state.latest_response_line, state.started_line))


def percentile(values: list[float], fraction: float) -> float | None:
    if not values:
        return None
    ordered = sorted(values)
    index = min(len(ordered) - 1, max(0, math.ceil(fraction * len(ordered)) - 1))
    return ordered[index]


def summarize_floats(values: list[float]) -> dict[str, Any]:
    return {
        "count": len(values),
        "min": number(min(values)) if values else "n/a",
        "p10": number(percentile(values, 0.10)),
        "median": number(median(values)) if values else "n/a",
        "p90": number(percentile(values, 0.90)),
        "max": number(max(values)) if values else "n/a",
        "mean": number(mean(values)) if values else "n/a",
    }


def volume_bucket(volume: Decimal) -> str:
    if volume == Decimal("1"):
        return "1"
    if volume < Decimal("5"):
        return "2-4"
    if volume < Decimal("10"):
        return "5-9"
    if volume < Decimal("25"):
        return "10-24"
    if volume < Decimal("50"):
        return "25-49"
    if volume < Decimal("100"):
        return "50-99"
    if volume < Decimal("250"):
        return "100-249"
    return "250+"


def relation_to_request(event_time: datetime, begin_time: datetime, end_time: datetime) -> str:
    if event_time < begin_time:
        return "before-request"
    if event_time > end_time:
        return "after-request"
    return "inside-request"


def empty_day_summary() -> dict[str, Any]:
    return {
        "events": 0,
        "volume": Decimal(0),
        "buyEvents": 0,
        "sellEvents": 0,
        "firstTime": None,
        "lastTime": None,
    }


def update_day_summary(summary: dict[str, Any], event_time: datetime, direction: str, volume: Decimal) -> None:
    summary["events"] += 1
    summary["volume"] += volume
    if direction == "Buy":
        summary["buyEvents"] += 1
    elif direction == "Sell":
        summary["sellEvents"] += 1
    if summary["firstTime"] is None or event_time < summary["firstTime"]:
        summary["firstTime"] = event_time
    if summary["lastTime"] is None or event_time > summary["lastTime"]:
        summary["lastTime"] = event_time


def compact_day_summary(values: dict[str, Any]) -> dict[str, Any]:
    return {
        "events": values["events"],
        "volume": values["volume"],
        "buyEvents": values["buyEvents"],
        "sellEvents": values["sellEvents"],
        "firstTime": values["firstTime"].isoformat() if values["firstTime"] else None,
        "lastTime": values["lastTime"].isoformat() if values["lastTime"] else None,
    }


def extract_selected_range(
    source_log: Path,
    schema: str,
    selected: RangeScan,
    snapshot_path: Path,
    candles_csv_path: Path,
    events_csv_path: Path,
) -> dict[str, Any]:
    for path in (snapshot_path, candles_csv_path, events_csv_path):
        path.parent.mkdir(parents=True, exist_ok=True)

    range_id = selected.range_id
    range_payload = selected.range_payload or {}
    security = range_payload.get("security") or {}
    tick_size = decimal(security.get("tickSize", 0))

    metadata: dict[str, Any] = {
        "schema": schema,
        "sourceLog": str(source_log.resolve()),
        "sourceLogBytesAtReadStart": source_log.stat().st_size,
        "selectedRangeId": range_id,
        "range": range_payload,
        "controlRecords": Counter(),
        "recordCount": 0,
        "snapshotRecordCount": 0,
        "candleCount": 0,
        "eventCount": 0,
        "eventCountInsideRequest": 0,
        "eventCountBeforeRequest": 0,
        "eventCountAfterRequest": 0,
        "responseRecords": sum(int(item.get("records", 0)) for item in selected.responses.values()),
        "tickVolumeMismatchCount": 0,
        "emptyTickEvents": 0,
        "malformedMatchingLines": 0,
        "firstEventTime": None,
        "lastEventTime": None,
        "firstInsideRequestEventTime": None,
        "lastInsideRequestEventTime": None,
        "directionSummary": {},
        "requestRelationCounts": Counter(),
        "volumeBuckets": Counter(),
        "eventsByDate": defaultdict(empty_day_summary),
        "candlesByDate": defaultdict(lambda: {"candles": 0, "volume": Decimal(0), "delta": Decimal(0)}),
        "candleVolume": Decimal(0),
        "candleDelta": Decimal(0),
        "candleMissingPocCount": 0,
        "candlePriceLevelCounts": [],
        "eventVolumeStats": {},
        "eventTickCountStats": {},
        "eventPriceChangeTicksStats": {},
    }

    direction_counts: Counter[str] = Counter()
    direction_volume: dict[str, Decimal] = defaultdict(Decimal)
    volumes: list[float] = []
    tick_counts: list[float] = []
    price_change_ticks: list[float] = []

    with source_log.open("rt", encoding="utf-8", errors="replace") as source, gzip.open(
        snapshot_path, "wt", encoding="utf-8", newline="\n"
    ) as snapshot, candles_csv_path.open("w", encoding="utf-8", newline="") as candles_stream, events_csv_path.open(
        "w", encoding="utf-8", newline=""
    ) as events_stream:
        candle_writer = csv.DictWriter(candles_stream, fieldnames=CANDLE_COLUMNS)
        event_writer = csv.DictWriter(events_stream, fieldnames=EVENT_COLUMNS)
        candle_writer.writeheader()
        event_writer.writeheader()

        for line in source:
            if MARKER not in line or f'"schema":"{schema}"' not in line:
                continue

            observation = observation_from_line(line, schema)
            if observation is None:
                metadata["malformedMatchingLines"] += 1
                continue

            observation_range = observation.get("range") or {}
            if observation_range.get("rangeId") != range_id:
                continue

            snapshot.write(json.dumps(observation, separators=(",", ":"), ensure_ascii=True))
            snapshot.write("\n")
            metadata["snapshotRecordCount"] += 1
            metadata["recordCount"] += 1

            record_type = observation.get("type")
            if record_type:
                metadata["controlRecords"][str(record_type)] += 1
                continue

            source_type = observation.get("source")
            if source_type == "chart-candle":
                metadata["candleCount"] += 1
                levels = observation.get("priceLevels") or []
                poc = observation.get("poc") or {}
                candle_volume = decimal(observation.get("volume"))
                candle_delta = decimal(observation.get("delta"))
                metadata["candleVolume"] += candle_volume
                metadata["candleDelta"] += candle_delta
                metadata["candlePriceLevelCounts"].append(len(levels))
                if not poc:
                    metadata["candleMissingPocCount"] += 1
                candle_day = str(observation.get("beginTime", ""))[:10]
                if candle_day:
                    metadata["candlesByDate"][candle_day]["candles"] += 1
                    metadata["candlesByDate"][candle_day]["volume"] += candle_volume
                    metadata["candlesByDate"][candle_day]["delta"] += candle_delta

                candle_writer.writerow(
                    {
                        "rangeId": range_id,
                        "bar": observation.get("bar", ""),
                        "beginTime": observation.get("beginTime", ""),
                        "endTime": observation.get("endTime", ""),
                        "open": observation.get("open", ""),
                        "high": observation.get("high", ""),
                        "low": observation.get("low", ""),
                        "close": observation.get("close", ""),
                        "volume": observation.get("volume", ""),
                        "bid": observation.get("bid", ""),
                        "ask": observation.get("ask", ""),
                        "delta": observation.get("delta", ""),
                        "ticks": observation.get("ticks", ""),
                        "vwap": observation.get("vwap", ""),
                        "valueAreaHigh": observation.get("valueAreaHigh", ""),
                        "valueAreaLow": observation.get("valueAreaLow", ""),
                        "pocPrice": poc.get("price", ""),
                        "pocVolume": poc.get("volume", ""),
                        "pocAsk": poc.get("ask", ""),
                        "pocBid": poc.get("bid", ""),
                        "priceLevelCount": len(levels),
                    }
                )
                continue

            if source_type != "historical-cumulative-trade":
                continue

            metadata["eventCount"] += 1
            event_time = parse_time(observation["time"])
            request_begin = parse_time(observation["requestBeginTime"])
            request_end = parse_time(observation["requestEndTime"])
            relation = relation_to_request(event_time, request_begin, request_end)
            metadata["requestRelationCounts"][relation] += 1
            if relation == "inside-request":
                metadata["eventCountInsideRequest"] += 1
                if metadata["firstInsideRequestEventTime"] is None or event_time < metadata["firstInsideRequestEventTime"]:
                    metadata["firstInsideRequestEventTime"] = event_time
                if metadata["lastInsideRequestEventTime"] is None or event_time > metadata["lastInsideRequestEventTime"]:
                    metadata["lastInsideRequestEventTime"] = event_time
            elif relation == "before-request":
                metadata["eventCountBeforeRequest"] += 1
            else:
                metadata["eventCountAfterRequest"] += 1

            if metadata["firstEventTime"] is None or event_time < metadata["firstEventTime"]:
                metadata["firstEventTime"] = event_time
            if metadata["lastEventTime"] is None or event_time > metadata["lastEventTime"]:
                metadata["lastEventTime"] = event_time

            direction = str(observation.get("direction", ""))
            total_volume = decimal(observation.get("totalVolume"))
            first_price = decimal(observation.get("firstPrice"))
            last_price = decimal(observation.get("lastPrice"))
            change_ticks = (last_price - first_price) / tick_size if tick_size else Decimal(0)
            ticks = observation.get("ticks") or []
            tick_count = len(ticks)
            if not ticks:
                metadata["emptyTickEvents"] += 1
            tick_volume = Decimal(0)
            first_tick = ticks[0] if ticks else {}
            last_tick = ticks[-1] if ticks else {}
            low_price: Decimal | None = None
            high_price: Decimal | None = None
            for tick in ticks:
                tick_volume += decimal(tick.get("volume"))
                tick_price = decimal(tick.get("price"))
                low_price = tick_price if low_price is None else min(low_price, tick_price)
                high_price = tick_price if high_price is None else max(high_price, tick_price)

            matches_volume = tick_volume == total_volume
            if not matches_volume:
                metadata["tickVolumeMismatchCount"] += 1

            direction_counts[direction] += 1
            direction_volume[direction] += total_volume
            metadata["volumeBuckets"][volume_bucket(total_volume)] += 1
            update_day_summary(metadata["eventsByDate"][event_time.date().isoformat()], event_time, direction, total_volume)
            volumes.append(float(total_volume))
            tick_counts.append(float(tick_count))
            price_change_ticks.append(float(change_ticks))

            event_writer.writerow(
                {
                    "rangeId": range_id,
                    "requestId": observation.get("requestId", ""),
                    "requestSequence": observation.get("requestSequence", ""),
                    "requestCount": observation.get("requestCount", ""),
                    "requestBeginTime": observation.get("requestBeginTime", ""),
                    "requestEndTime": observation.get("requestEndTime", ""),
                    "eventId": observation.get("eventId", ""),
                    "time": observation.get("time", ""),
                    "timeRelationToRequest": relation,
                    "direction": direction,
                    "totalVolume": observation.get("totalVolume", ""),
                    "firstPrice": observation.get("firstPrice", ""),
                    "lastPrice": observation.get("lastPrice", ""),
                    "priceChangeTicks": format(change_ticks, "f"),
                    "tickCount": tick_count,
                    "tickVolume": format(tick_volume, "f"),
                    "tickVolumeMatchesTotal": str(matches_volume).lower(),
                    "firstTickTime": first_tick.get("time", ""),
                    "lastTickTime": last_tick.get("time", ""),
                    "firstTickPrice": first_tick.get("price", ""),
                    "lastTickPrice": last_tick.get("price", ""),
                    "intraTradeLow": format(low_price, "f") if low_price is not None else "",
                    "intraTradeHigh": format(high_price, "f") if high_price is not None else "",
                }
            )

    metadata["directionSummary"] = {
        direction: {"events": direction_counts[direction], "volume": direction_volume[direction]}
        for direction in sorted(direction_counts)
    }
    metadata["eventVolumeStats"] = summarize_floats(volumes)
    metadata["eventTickCountStats"] = summarize_floats(tick_counts)
    metadata["eventPriceChangeTicksStats"] = summarize_floats(price_change_ticks)
    metadata["candlePriceLevelCountStats"] = summarize_floats([float(value) for value in metadata["candlePriceLevelCounts"]])
    metadata["eventsByDate"] = {
        date: compact_day_summary(values) for date, values in sorted(metadata["eventsByDate"].items())
    }
    metadata["candlesByDate"] = {
        date: {"candles": values["candles"], "volume": values["volume"], "delta": values["delta"]}
        for date, values in sorted(metadata["candlesByDate"].items())
    }
    metadata["controlRecords"] = dict(sorted(metadata["controlRecords"].items()))
    metadata["requestRelationCounts"] = dict(sorted(metadata["requestRelationCounts"].items()))
    metadata["volumeBuckets"] = {bucket: metadata["volumeBuckets"].get(bucket, 0) for _, bucket in VOLUME_BUCKETS}
    metadata["firstEventTime"] = metadata["firstEventTime"].isoformat() if metadata["firstEventTime"] else None
    metadata["lastEventTime"] = metadata["lastEventTime"].isoformat() if metadata["lastEventTime"] else None
    metadata["firstInsideRequestEventTime"] = (
        metadata["firstInsideRequestEventTime"].isoformat() if metadata["firstInsideRequestEventTime"] else None
    )
    metadata["lastInsideRequestEventTime"] = (
        metadata["lastInsideRequestEventTime"].isoformat() if metadata["lastInsideRequestEventTime"] else None
    )
    return metadata


def path_info(path: Path) -> dict[str, Any]:
    return {
        "path": path.as_posix(),
        "bytes": path.stat().st_size,
        "sha256": sha256(path),
    }


def write_summary(summary_path: Path, summary: dict[str, Any]) -> None:
    summary_path.parent.mkdir(parents=True, exist_ok=True)
    summary_path.write_text(json.dumps(summary, indent=2, cls=DecimalEncoder, ensure_ascii=True) + "\n", encoding="utf-8")


def markdown_table(headers: list[str], rows: list[list[str]]) -> str:
    lines = ["| " + " | ".join(headers) + " |", "| " + " | ".join("---" for _ in headers) + " |"]
    lines.extend("| " + " | ".join(row) + " |" for row in rows)
    return "\n".join(lines)


def build_report(summary: dict[str, Any], artifacts: dict[str, dict[str, Any]]) -> str:
    range_payload = summary["range"]
    security = range_payload.get("security") or {}
    direction_summary = summary["directionSummary"]
    response_records = int(summary.get("responseRecords", 0))
    event_count = int(summary.get("eventCount", 0))
    inside_count = int(summary.get("eventCountInsideRequest", 0))
    before_count = int(summary.get("eventCountBeforeRequest", 0))
    after_count = int(summary.get("eventCountAfterRequest", 0))

    artifact_rows = [
        [name, info["path"], fmt_int(int(info["bytes"])), info["sha256"]]
        for name, info in artifacts.items()
    ]

    direction_rows = [
        [direction, fmt_int(int(values["events"])), fmt_decimal(values["volume"])]
        for direction, values in sorted(direction_summary.items())
    ]

    relation_rows = [
        [relation, fmt_int(int(count))] for relation, count in summary["requestRelationCounts"].items()
    ]

    date_rows = [
        [
            date,
            fmt_int(int(values["events"])),
            fmt_decimal(values["volume"]),
            fmt_int(int(values["buyEvents"])),
            fmt_int(int(values["sellEvents"])),
            values["firstTime"] or "n/a",
            values["lastTime"] or "n/a",
        ]
        for date, values in summary["eventsByDate"].items()
    ]

    candle_rows = [
        [date, fmt_int(int(values["candles"])), fmt_decimal(values["volume"]), fmt_decimal(values["delta"])]
        for date, values in summary["candlesByDate"].items()
    ]

    stats_rows = [
        ["event total volume", *[str(summary["eventVolumeStats"][key]) for key in ("min", "p10", "median", "p90", "max", "mean")]],
        ["event tick count", *[str(summary["eventTickCountStats"][key]) for key in ("min", "p10", "median", "p90", "max", "mean")]],
        [
            "event price change ticks",
            *[str(summary["eventPriceChangeTicksStats"][key]) for key in ("min", "p10", "median", "p90", "max", "mean")],
        ],
        [
            "candle price levels",
            *[str(summary["candlePriceLevelCountStats"][key]) for key in ("min", "p10", "median", "p90", "max", "mean")],
        ],
    ]

    return f"""# Historical Cumulative Context Inventory - 2026-08-04

## Stato

```text
Schema valido:        {summary['schema']}
Range id:             {summary['selectedRangeId']}
Strumento:            {security.get('securityId', 'n/a')} / {security.get('instrument', 'n/a')}
Tipo:                 inventario storico osservativo da chart ATAS
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo report documenta la prima cattura storica valida del recorder **Fabio Historical Cumulative Context Recorder**. Lo scopo e' verificare cosa ATAS restituisce da `RequestForCumulativeTrades(...)` e conservare una base riproducibile per analisi offline successive. Non approva soglie, setup, filtri operativi o un modello.

## Artefatti

{markdown_table(['Artefatto', 'Path', 'Bytes', 'SHA-256'], artifact_rows)}

Il log sorgente ATAS non viene versionato. Al momento della lettura misurava `{fmt_int(int(summary['sourceLogBytesAtReadStart']))}` byte.

## Range

```text
Capture begin:        {range_payload.get('beginTime')}
Capture end:          {range_payload.get('endTime')}
Capture duration:     {range_payload.get('durationDays')} giorni
Captured bars:        {fmt_int(int(range_payload.get('barCount', 0)))}
Loaded begin:         {range_payload.get('loadedBeginTime')}
Loaded end:           {range_payload.get('loadedEndTime')}
Loaded duration:      {range_payload.get('loadedDurationDays')} giorni
Loaded bars:          {fmt_int(int(range_payload.get('loadedBarCount', 0)))}
Request count:        {max((int(item.get('requestCount', 0)) for item in summary.get('requests', [])), default=1)}
```

Il recorder `v4` ha correttamente limitato la cattura agli ultimi sette giorni disponibili dal fondo del chart. ATAS aveva precaricato piu' storico (`loadedDurationDays` > 7), ma il capture range resta di sette giorni.

## Conteggi

```text
Snapshot records:             {fmt_int(int(summary['snapshotRecordCount']))}
Chart candles:                {fmt_int(int(summary['candleCount']))}
Historical CumulativeTrade:   {fmt_int(event_count)}
ATAS response records:        {fmt_int(response_records)}
Inside requested window:      {fmt_int(inside_count)}
Before requested window:      {fmt_int(before_count)}
After requested window:       {fmt_int(after_count)}
Tick-volume mismatches:       {fmt_int(int(summary['tickVolumeMismatchCount']))}
Empty tick events:            {fmt_int(int(summary['emptyTickEvents']))}
```

`historical-cumulative-response` viene scritto dopo la serializzazione degli eventi ricevuti, quindi la presenza della risposta nel log conferma che la richiesta e' terminata. In questa cattura ATAS ha restituito anche record fuori dalla finestra richiesta: il parser li conserva nello snapshot come evidenza e li marca nel CSV con `timeRelationToRequest`.

## Relazione Con La Richiesta

{markdown_table(['Relazione', 'Eventi'], relation_rows)}

Evento piu' antico restituito: `{summary['firstEventTime']}`. Evento piu' recente restituito: `{summary['lastEventTime']}`. Dentro la finestra richiesta: `{summary['firstInsideRequestEventTime']}` -> `{summary['lastInsideRequestEventTime']}`.

## Lato E Volume

{markdown_table(['Direction', 'Eventi', 'Volume'], direction_rows)}

{markdown_table(['Metrica', 'Min', 'P10', 'Mediana', 'P90', 'Max', 'Media'], stats_rows)}

## Eventi Per Data

{markdown_table(['Data', 'Eventi', 'Volume', 'Buy', 'Sell', 'Primo evento', 'Ultimo evento'], date_rows)}

## Candle Per Data

{markdown_table(['Data', 'Candle', 'Volume', 'Delta'], candle_rows)}

## Limiti

- I record storici sono `CumulativeTrade` ATAS, non un backfill raw tick-by-tick equivalente a `OnNewTrade`.
- I record fuori finestra mostrano che ATAS puo' arrotondare o ampliare la risposta rispetto al begin richiesto, probabilmente per sessione ETH/caricamento interno. Qualsiasi analisi successiva deve scegliere esplicitamente se usare tutti i record restituiti o solo `inside-request`.
- Le righe evento sono fortemente non indipendenti nel tempo. Questo inventario non fornisce probabilita', edge o regole di esecuzione.
- Il contesto candle/footprint e' quello caricato dal chart, con granularita' di barra, non una ricostruzione tick-by-tick del POC di sessione live.

## Prossimo Uso

Il passo successivo corretto e' un report storico descrittivo che usi solo una popolazione dichiarata, per esempio `inside-request`, e confronti gli eventi con contesto candle/footprint e risposta futura da barre. Prima di qualunque ipotesi operativa servono piu' giorni e finestre non sovrapposte.
"""


def main() -> None:
    args = parse_args()
    ranges, malformed_control = scan_ranges(args.source_log, args.schema)
    selected = select_range(ranges, args.range_id)

    summary = extract_selected_range(
        args.source_log,
        args.schema,
        selected,
        args.snapshot,
        args.candles_csv,
        args.events_csv,
    )
    summary["scanMalformedControlLines"] = malformed_control
    summary["availableRanges"] = {
        range_id: {
            "complete": state.complete,
            "expectedRequestCount": state.expected_request_count,
            "requests": sorted(state.requests),
            "responses": sorted(state.responses),
        }
        for range_id, state in sorted(ranges.items())
    }
    summary["requests"] = [selected.requests[key] for key in sorted(selected.requests)]
    summary["responses"] = [selected.responses[key] for key in sorted(selected.responses)]

    write_summary(args.summary_json, summary)
    artifacts = {
        "snapshot": path_info(args.snapshot),
        "candlesCsv": path_info(args.candles_csv),
        "eventsCsv": path_info(args.events_csv),
    }
    summary["artifactHashes"] = artifacts
    write_summary(args.summary_json, summary)
    artifacts["summaryJson"] = path_info(args.summary_json)

    args.report.parent.mkdir(parents=True, exist_ok=True)
    args.report.write_text(build_report(summary, artifacts), encoding="utf-8")

    print(json.dumps(
        {
            "schema": args.schema,
            "selectedRangeId": selected.range_id,
            "snapshotRecords": summary["snapshotRecordCount"],
            "candles": summary["candleCount"],
            "events": summary["eventCount"],
            "insideRequest": summary["eventCountInsideRequest"],
            "beforeRequest": summary["eventCountBeforeRequest"],
            "afterRequest": summary["eventCountAfterRequest"],
            "report": args.report.as_posix(),
        },
        indent=2,
    ))


if __name__ == "__main__":
    main()
