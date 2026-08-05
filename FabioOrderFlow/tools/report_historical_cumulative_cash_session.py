#!/usr/bin/env python3
"""Describe historical CumulativeTrade records during NQ US Cash sessions."""

from __future__ import annotations

import argparse
import bisect
import csv
import hashlib
import json
from collections import Counter, defaultdict
from dataclasses import dataclass, field
from datetime import date, datetime, time, timedelta, timezone
from decimal import Decimal
from pathlib import Path
from typing import Any

SCHEMA = "fof-historical-cumulative-context-v5"
UTC = timezone.utc
CASH_START = time(9, 30)
CASH_END = time(16, 0)
HORIZONS = (60, 180, 300, 900)

EVENT_COLUMNS = [
    "sessionDateNewYork",
    "eventTimeUtc",
    "eventTimeNewYork",
    "eventId",
    "direction",
    "totalVolume",
    "firstPrice",
    "lastPrice",
    "priceChangeTicks",
    "tickCount",
    "candleBar",
    "candleBeginUtc",
    "candleBeginNewYork",
    "candleOpen",
    "candleHigh",
    "candleLow",
    "candleClose",
    "candleVolume",
    "candleDelta",
    "candleVwap",
    "candlePoc",
    "candleValueAreaHigh",
    "candleValueAreaLow",
    "locationVsCandlePoc",
    "locationVsValueArea",
    "locationVsVwap",
    "response60Ticks",
    "response180Ticks",
    "response300Ticks",
    "response900Ticks",
]

ANCHOR_COLUMNS = [
    "sessionDateNewYork",
    "anchorTimeUtc",
    "anchorTimeNewYork",
    "candleBar",
    "anchorPrice",
    "candleOpen",
    "candleHigh",
    "candleLow",
    "candleClose",
    "candleVolume",
    "candleDelta",
    "candleVwap",
    "candlePoc",
    "candleValueAreaHigh",
    "candleValueAreaLow",
    "response60Ticks",
    "response180Ticks",
    "response300Ticks",
    "response900Ticks",
]


@dataclass(frozen=True)
class Candle:
    bar: int
    begin_utc: datetime
    end_utc: datetime
    begin_ny: datetime
    end_ny: datetime
    open: Decimal
    high: Decimal
    low: Decimal
    close: Decimal
    volume: Decimal
    delta: Decimal
    vwap: Decimal | None
    poc: Decimal | None
    value_area_high: Decimal | None
    value_area_low: Decimal | None


@dataclass
class ResponseAccumulator:
    count: int = 0
    positive: int = 0
    negative: int = 0
    zero: int = 0
    total: Decimal = Decimal(0)
    minimum: Decimal | None = None
    maximum: Decimal | None = None

    def add(self, value: Decimal | None) -> None:
        if value is None:
            return
        self.count += 1
        self.total += value
        self.minimum = value if self.minimum is None else min(self.minimum, value)
        self.maximum = value if self.maximum is None else max(self.maximum, value)
        if value > 0:
            self.positive += 1
        elif value < 0:
            self.negative += 1
        else:
            self.zero += 1

    def to_dict(self) -> dict[str, Any]:
        return {
            "count": self.count,
            "positive": self.positive,
            "negative": self.negative,
            "zero": self.zero,
            "mean": None if not self.count else self.total / self.count,
            "min": self.minimum,
            "max": self.maximum,
        }


@dataclass
class CashDay:
    session_date: date
    candles: list[Candle] = field(default_factory=list)
    begins: list[datetime] = field(default_factory=list)
    ends: list[datetime] = field(default_factory=list)
    event_count: int = 0
    event_volume: Decimal = Decimal(0)
    buy_events: int = 0
    sell_events: int = 0
    selected_non_overlap: int = 0
    first_event: datetime | None = None
    last_event: datetime | None = None

    @property
    def full_coverage(self) -> bool:
        return bool(
            self.candles
            and self.candles[0].begin_ny.time() <= CASH_START
            and self.candles[-1].begin_ny.time() >= time(15, 59)
        )

    def add_candle(self, candle: Candle) -> None:
        self.candles.append(candle)

    def finalize(self) -> None:
        self.candles.sort(key=lambda candle: candle.begin_utc)
        self.begins = [candle.begin_utc for candle in self.candles]
        self.ends = [candle.end_utc for candle in self.candles]

    def candle_for(self, timestamp: datetime) -> Candle | None:
        index = bisect.bisect_right(self.begins, timestamp) - 1
        if index < 0:
            return None
        candle = self.candles[index]
        return candle if candle.begin_utc <= timestamp <= candle.end_utc else None

    def response(self, timestamp: datetime, reference_price: Decimal, horizon_seconds: int, tick_size: Decimal) -> Decimal | None:
        local_time = to_new_york(timestamp)
        target = timestamp + timedelta(seconds=horizon_seconds)
        if local_time.date() != self.session_date or to_new_york(target).date() != self.session_date:
            return None
        if to_new_york(target).time() > CASH_END:
            return None
        index = bisect.bisect_left(self.ends, target)
        if index >= len(self.candles):
            return None
        candle = self.candles[index]
        if candle.end_ny.time() > CASH_END:
            return None
        return (candle.close - reference_price) / tick_size


def to_new_york(timestamp: datetime) -> datetime:
    """Convert UTC to New York using the post-2007 United States DST rule."""
    if timestamp.tzinfo != UTC:
        raise ValueError(f"Expected a UTC timestamp, received {timestamp!r}")

    year = timestamp.year
    march_eighth = date(year, 3, 8)
    dst_start_date = march_eighth + timedelta(days=(6 - march_eighth.weekday()) % 7)
    november_first = date(year, 11, 1)
    dst_end_date = november_first + timedelta(days=(6 - november_first.weekday()) % 7)
    dst_start_utc = datetime.combine(dst_start_date, time(7, 0), UTC)
    dst_end_utc = datetime.combine(dst_end_date, time(6, 0), UTC)
    offset_hours = -4 if dst_start_utc <= timestamp < dst_end_utc else -5
    return timestamp.astimezone(timezone(timedelta(hours=offset_hours), "America/New_York"))


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--events-csv", required=True, type=Path)
    parser.add_argument("--candles-csv", required=True, type=Path)
    parser.add_argument("--summary-json", required=True, type=Path)
    parser.add_argument("--cash-events-csv", required=True, type=Path)
    parser.add_argument("--anchors-csv", required=True, type=Path)
    parser.add_argument("--non-overlap-events-csv", required=True, type=Path)
    parser.add_argument("--cash-summary-json", required=True, type=Path)
    parser.add_argument("--report", required=True, type=Path)
    return parser.parse_args()


def parse_utc(value: str) -> datetime:
    return datetime.fromisoformat(value).replace(tzinfo=UTC)


def decimal_or_none(value: str | None) -> Decimal | None:
    return None if value in {None, ""} else Decimal(value)


def number(value: Decimal | None) -> str:
    return "n/a" if value is None else format(value, "f")


def tick_number(value: Decimal | None) -> str:
    if value is None:
        return "n/a"
    text = f"{value:.2f}"
    return text.rstrip("0").rstrip(".")


def int_format(value: int) -> str:
    return f"{value:,}".replace(",", ".")


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def artifact(path: Path) -> dict[str, Any]:
    return {"path": path.as_posix(), "bytes": path.stat().st_size, "sha256": sha256(path)}


def is_cash_time(local_time: datetime) -> bool:
    return CASH_START <= local_time.time() < CASH_END


def load_cash_days(candles_csv: Path) -> dict[date, CashDay]:
    days: dict[date, CashDay] = {}
    with candles_csv.open("r", encoding="utf-8", newline="") as stream:
        for row in csv.DictReader(stream):
            begin_utc = parse_utc(row["beginTime"])
            end_utc = parse_utc(row["endTime"])
            begin_ny = to_new_york(begin_utc)
            end_ny = to_new_york(end_utc)
            if not is_cash_time(begin_ny):
                continue
            candle = Candle(
                int(row["bar"]),
                begin_utc,
                end_utc,
                begin_ny,
                end_ny,
                Decimal(row["open"]),
                Decimal(row["high"]),
                Decimal(row["low"]),
                Decimal(row["close"]),
                Decimal(row["volume"]),
                Decimal(row["delta"]),
                decimal_or_none(row["vwap"]),
                decimal_or_none(row["pocPrice"]),
                decimal_or_none(row["valueAreaHigh"]),
                decimal_or_none(row["valueAreaLow"]),
            )
            day = days.setdefault(begin_ny.date(), CashDay(begin_ny.date()))
            day.add_candle(candle)
    for day in days.values():
        day.finalize()
    return days


def relation_to_poc(price: Decimal, poc: Decimal | None) -> str:
    if poc is None:
        return "missing-poc"
    if price < poc:
        return "below-candle-poc"
    if price > poc:
        return "above-candle-poc"
    return "at-candle-poc"


def relation_to_value(price: Decimal, low: Decimal | None, high: Decimal | None) -> str:
    if low is None or high is None:
        return "missing-value-area"
    if price < low:
        return "below-value-area"
    if price > high:
        return "above-value-area"
    return "inside-value-area"


def relation_to_vwap(price: Decimal, vwap: Decimal | None) -> str:
    if vwap is None:
        return "missing-vwap"
    if price < vwap:
        return "below-vwap"
    if price > vwap:
        return "above-vwap"
    return "at-vwap"


def response_values(day: CashDay, timestamp: datetime, price: Decimal, tick_size: Decimal) -> dict[int, Decimal | None]:
    return {horizon: day.response(timestamp, price, horizon, tick_size) for horizon in HORIZONS}


def update_accumulators(
    accumulators: dict[str, dict[int, ResponseAccumulator]],
    key: str,
    responses: dict[int, Decimal | None],
) -> None:
    group = accumulators.setdefault(key, {horizon: ResponseAccumulator() for horizon in HORIZONS})
    for horizon, value in responses.items():
        group[horizon].add(value)


def response_summary(accumulators: dict[str, dict[int, ResponseAccumulator]]) -> dict[str, Any]:
    return {
        key: {str(horizon): accumulator.to_dict() for horizon, accumulator in horizons.items()}
        for key, horizons in sorted(accumulators.items())
    }


def csv_response_fields(responses: dict[int, Decimal | None]) -> dict[str, str]:
    return {f"response{horizon}Ticks": number(responses[horizon]) if responses[horizon] is not None else "" for horizon in HORIZONS}


def write_anchors(days: dict[date, CashDay], tick_size: Decimal, anchors_csv: Path) -> dict[str, Any]:
    summary: dict[str, Any] = {"count": 0, "byResponse": {}}
    accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    anchors_csv.parent.mkdir(parents=True, exist_ok=True)
    with anchors_csv.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=ANCHOR_COLUMNS)
        writer.writeheader()
        for session_date, day in sorted(days.items()):
            for candle in day.candles:
                responses = response_values(day, candle.begin_utc, candle.open, tick_size)
                update_accumulators(accumulators, "all", responses)
                summary["count"] += 1
                writer.writerow(
                    {
                        "sessionDateNewYork": session_date.isoformat(),
                        "anchorTimeUtc": candle.begin_utc.replace(tzinfo=None).isoformat(),
                        "anchorTimeNewYork": candle.begin_ny.isoformat(),
                        "candleBar": candle.bar,
                        "anchorPrice": number(candle.open),
                        "candleOpen": number(candle.open),
                        "candleHigh": number(candle.high),
                        "candleLow": number(candle.low),
                        "candleClose": number(candle.close),
                        "candleVolume": number(candle.volume),
                        "candleDelta": number(candle.delta),
                        "candleVwap": number(candle.vwap),
                        "candlePoc": number(candle.poc),
                        "candleValueAreaHigh": number(candle.value_area_high),
                        "candleValueAreaLow": number(candle.value_area_low),
                        **csv_response_fields(responses),
                    }
                )
    summary["byResponse"] = response_summary(accumulators)
    return summary


def write_cash_events(
    events_csv: Path,
    days: dict[date, CashDay],
    tick_size: Decimal,
    cash_events_csv: Path,
    non_overlap_events_csv: Path,
) -> dict[str, Any]:
    summary: dict[str, Any] = {
        "sourceEventRows": 0,
        "cashEventRows": 0,
        "missingContainingCandle": 0,
        "byDirection": {},
        "byCandlePoc": {},
        "byValueArea": {},
        "byVwap": {},
        "all": {},
        "bySessionDate": {},
        "nonOverlapping": {},
    }
    all_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    direction_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    poc_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    value_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    vwap_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    non_overlap_accumulators: dict[str, dict[int, ResponseAccumulator]] = {}
    last_selected: dict[date, datetime] = {}

    cash_events_csv.parent.mkdir(parents=True, exist_ok=True)
    with events_csv.open("r", encoding="utf-8", newline="") as source, cash_events_csv.open(
        "w", encoding="utf-8", newline=""
    ) as event_stream, non_overlap_events_csv.open("w", encoding="utf-8", newline="") as selected_stream:
        reader = csv.DictReader(source)
        event_writer = csv.DictWriter(event_stream, fieldnames=EVENT_COLUMNS)
        selected_writer = csv.DictWriter(selected_stream, fieldnames=EVENT_COLUMNS)
        event_writer.writeheader()
        selected_writer.writeheader()

        for row in reader:
            summary["sourceEventRows"] += 1
            if row["timeRelationToRequest"] != "inside-request":
                continue
            event_utc = parse_utc(row["time"])
            event_ny = to_new_york(event_utc)
            if not is_cash_time(event_ny):
                continue
            day = days.get(event_ny.date())
            if day is None:
                continue
            candle = day.candle_for(event_utc)
            if candle is None:
                summary["missingContainingCandle"] += 1
                continue

            last_price = Decimal(row["lastPrice"])
            responses = response_values(day, event_utc, last_price, tick_size)
            poc_relation = relation_to_poc(last_price, candle.poc)
            value_relation = relation_to_value(last_price, candle.value_area_low, candle.value_area_high)
            vwap_relation = relation_to_vwap(last_price, candle.vwap)
            direction = row["direction"]
            output = {
                "sessionDateNewYork": event_ny.date().isoformat(),
                "eventTimeUtc": event_utc.replace(tzinfo=None).isoformat(),
                "eventTimeNewYork": event_ny.isoformat(),
                "eventId": row["eventId"],
                "direction": direction,
                "totalVolume": row["totalVolume"],
                "firstPrice": row["firstPrice"],
                "lastPrice": row["lastPrice"],
                "priceChangeTicks": row["priceChangeTicks"],
                "tickCount": row["tickCount"],
                "candleBar": candle.bar,
                "candleBeginUtc": candle.begin_utc.replace(tzinfo=None).isoformat(),
                "candleBeginNewYork": candle.begin_ny.isoformat(),
                "candleOpen": number(candle.open),
                "candleHigh": number(candle.high),
                "candleLow": number(candle.low),
                "candleClose": number(candle.close),
                "candleVolume": number(candle.volume),
                "candleDelta": number(candle.delta),
                "candleVwap": number(candle.vwap),
                "candlePoc": number(candle.poc),
                "candleValueAreaHigh": number(candle.value_area_high),
                "candleValueAreaLow": number(candle.value_area_low),
                "locationVsCandlePoc": poc_relation,
                "locationVsValueArea": value_relation,
                "locationVsVwap": vwap_relation,
                **csv_response_fields(responses),
            }
            event_writer.writerow(output)
            summary["cashEventRows"] += 1
            day.event_count += 1
            day.event_volume += Decimal(row["totalVolume"])
            day.buy_events += direction == "Buy"
            day.sell_events += direction == "Sell"
            day.first_event = event_utc if day.first_event is None else min(day.first_event, event_utc)
            day.last_event = event_utc if day.last_event is None else max(day.last_event, event_utc)

            update_accumulators(all_accumulators, "all", responses)
            update_accumulators(direction_accumulators, direction, responses)
            update_accumulators(poc_accumulators, poc_relation, responses)
            update_accumulators(value_accumulators, value_relation, responses)
            update_accumulators(vwap_accumulators, vwap_relation, responses)

            selected_at = last_selected.get(day.session_date)
            response_900 = responses[900]
            if response_900 is not None and (selected_at is None or event_utc >= selected_at + timedelta(seconds=900)):
                selected_writer.writerow(output)
                last_selected[day.session_date] = event_utc
                day.selected_non_overlap += 1
                update_accumulators(non_overlap_accumulators, "all", responses)

    summary["all"] = response_summary(all_accumulators)
    summary["byDirection"] = response_summary(direction_accumulators)
    summary["byCandlePoc"] = response_summary(poc_accumulators)
    summary["byValueArea"] = response_summary(value_accumulators)
    summary["byVwap"] = response_summary(vwap_accumulators)
    summary["nonOverlapping"] = response_summary(non_overlap_accumulators)
    summary["bySessionDate"] = {
        session_date.isoformat(): {
            "candles": len(day.candles),
            "fullCashCoverage": day.full_coverage,
            "events": day.event_count,
            "volume": day.event_volume,
            "buyEvents": day.buy_events,
            "sellEvents": day.sell_events,
            "selectedNonOverlapping": day.selected_non_overlap,
            "firstEventUtc": None if day.first_event is None else day.first_event.replace(tzinfo=None).isoformat(),
            "lastEventUtc": None if day.last_event is None else day.last_event.replace(tzinfo=None).isoformat(),
        }
        for session_date, day in sorted(days.items())
    }
    return summary


def encode_default(value: Any) -> Any:
    if isinstance(value, Decimal):
        return format(value, "f")
    if isinstance(value, date):
        return value.isoformat()
    raise TypeError(f"Cannot encode {type(value)!r}")


def markdown_table(headers: list[str], rows: list[list[str]]) -> str:
    lines = ["| " + " | ".join(headers) + " |", "| " + " | ".join("---" for _ in headers) + " |"]
    lines.extend("| " + " | ".join(row) + " |" for row in rows)
    return "\n".join(lines)


def response_row(summary: dict[str, Any], label: str, horizon: int) -> list[str]:
    values = summary[str(horizon)]
    return [
        label,
        int_format(int(values["count"])),
        int_format(int(values["positive"])),
        int_format(int(values["negative"])),
        int_format(int(values["zero"])),
        tick_number(Decimal(values["mean"])) if values["mean"] is not None else "n/a",
        tick_number(Decimal(values["min"])) if values["min"] is not None else "n/a",
        tick_number(Decimal(values["max"])) if values["max"] is not None else "n/a",
    ]


def build_report(summary: dict[str, Any], artifacts: dict[str, dict[str, Any]]) -> str:
    event = summary["events"]
    anchors = summary["anchors"]
    day_rows = []
    for session_date, values in event["bySessionDate"].items():
        day_rows.append(
            [
                session_date,
                "full" if values["fullCashCoverage"] else "partial",
                int_format(int(values["candles"])),
                int_format(int(values["events"])),
                number(Decimal(values["volume"])),
                int_format(int(values["buyEvents"])),
                int_format(int(values["sellEvents"])),
                int_format(int(values["selectedNonOverlapping"])),
            ]
        )

    response_rows = []
    for horizon in HORIZONS:
        response_rows.append(response_row(event["all"]["all"], f"events {horizon}s", horizon))
        response_rows.append(response_row(anchors["byResponse"]["all"], f"anchors {horizon}s", horizon))

    direction_rows = [response_row(values, direction, 300) for direction, values in event["byDirection"].items()]
    poc_rows = [response_row(values, relation, 300) for relation, values in event["byCandlePoc"].items()]
    value_rows = [response_row(values, relation, 300) for relation, values in event["byValueArea"].items()]
    vwap_rows = [response_row(values, relation, 300) for relation, values in event["byVwap"].items()]
    non_overlap_rows = [response_row(event["nonOverlapping"]["all"], "selected events", 900)]

    artifact_rows = [
        [name, values["path"], int_format(int(values["bytes"])), values["sha256"]]
        for name, values in artifacts.items()
    ]

    return f"""# Historical Cumulative Cash Session Description - 2026-08-04

## Stato

```text
Schema sorgente:      {summary['sourceSchema']}
Strumento:            {summary['securityId']}
Sessione:             NQ US Cash, 09:30-16:00 America/New_York
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo report descrive lo storico `CumulativeTrade` ATAS nella sola sessione cash. E' un controllo di dati, contesto e risposta temporale. Non approva soglie, setup, filtri operativi o un modello.

## Artefatti

{markdown_table(['Artefatto', 'Path', 'Bytes', 'SHA-256'], artifact_rows)}

## Copertura Cash

{markdown_table(['Data NY', 'Copertura', 'Candle', 'Eventi', 'Volume', 'Buy', 'Sell', 'Non-overlap 900s'], day_rows)}

Le date `{', '.join(date for date, values in event['bySessionDate'].items() if not values['fullCashCoverage'])}` sono parziali. Non vengono trattate come sessioni complete nel testo interpretativo.

## Metodo

Ogni timestamp sorgente e' interpretato come UTC e convertito esplicitamente in `America/New_York`. Un evento entra nel dataset solo se `09:30 <= timeNewYork < 16:00`. L'evento viene associato alla candle a 1 minuto che lo contiene. La risposta e' il close della prima candle disponibile al o dopo 60, 180, 300 o 900 secondi, meno `lastPrice` dell'evento, espresso in tick. Non viene calcolata alcuna risposta che attraversi il close cash.

Le ancore usano lo stesso calcolo, ma partono dall'open di ogni candle cash. Sono una baseline temporale: controllano il movimento normalmente disponibile nel medesimo periodo, non la causalita' dell'evento.

## Eventi E Baseline

{markdown_table(['Popolazione', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], response_rows)}

Le righe evento sono molto sovrapposte. Le differenze tra eventi e ancore sono descrittive e non sono una stima di edge o probabilita'.

## Contesto Candle A 300 Secondi

### Direction

{markdown_table(['Gruppo', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], direction_rows)}

### Posizione Vs POC Candle

{markdown_table(['Gruppo', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], poc_rows)}

### Posizione Vs Value Area Candle

{markdown_table(['Gruppo', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], value_rows)}

### Posizione Vs VWAP Candle

{markdown_table(['Gruppo', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], vwap_rows)}

Queste classificazioni descrivono il contesto della candle storica ATAS. Non sono un POC di sessione ricostruito tick-by-tick e non definiscono trigger.

## Controllo Non Sovrapposto

Per ogni data cash il parser seleziona il primo evento con risposta completa a 900 secondi, poi il primo almeno 900 secondi dopo. Il risultato e':

{markdown_table(['Gruppo', 'N', 'Positivi', 'Negativi', 'Zero', 'Media tick', 'Min', 'Max'], non_overlap_rows)}

La forte riduzione rispetto alla popolazione completa mostra che i milioni di eventi non sono milioni di osservazioni indipendenti.

## Limiti

- Il dataset storico contiene `CumulativeTrade` ATAS, non raw trade storico equivalente a `OnNewTrade`.
- Il report copre sei date cash, di cui due parziali. Non convalida un modello o probabilita'.
- POC, value area e VWAP sono campi di candle storica; non sostituiscono il profilo di sessione in sviluppo del recorder live.
- Il lato Buy/Sell indica il campo ATAS dell'evento, non l'intenzione, il profilo o l'orizzonte operativo del partecipante.

## Esito

Il risultato utile di questa fase e' una popolazione cash dichiarata, con risposta da candle, baseline temporale e controllo non sovrapposto. Qualunque ipotesi futura deve essere definita dopo questa descrizione, testata su giorni addizionali e mantenuta distinta da una regola eseguibile.
"""


def main() -> None:
    args = parse_args()
    source_summary = json.loads(args.summary_json.read_text(encoding="utf-8"))
    if source_summary.get("schema") != SCHEMA:
        raise ValueError(f"Expected {SCHEMA}, found {source_summary.get('schema')}")
    tick_size = Decimal(str(source_summary["range"]["security"]["tickSize"]))
    days = load_cash_days(args.candles_csv)
    if not days:
        raise ValueError("No cash candles were found after UTC to New York conversion.")

    anchors = write_anchors(days, tick_size, args.anchors_csv)
    events = write_cash_events(args.events_csv, days, tick_size, args.cash_events_csv, args.non_overlap_events_csv)
    summary = {
        "sourceSchema": SCHEMA,
        "sourceInventorySummary": args.summary_json.as_posix(),
        "sourceInventorySha256": sha256(args.summary_json),
        "securityId": source_summary["range"]["security"]["securityId"],
        "tickSize": tick_size,
        "session": {"timeZone": "America/New_York", "start": "09:30", "end": "16:00"},
        "horizonsSeconds": list(HORIZONS),
        "events": events,
        "anchors": anchors,
    }
    args.cash_summary_json.parent.mkdir(parents=True, exist_ok=True)
    args.cash_summary_json.write_text(json.dumps(summary, default=encode_default, indent=2) + "\n", encoding="utf-8")
    artifacts = {
        "cashEventsCsv": artifact(args.cash_events_csv),
        "anchorsCsv": artifact(args.anchors_csv),
        "nonOverlapEventsCsv": artifact(args.non_overlap_events_csv),
    }
    summary["artifactHashes"] = artifacts
    args.cash_summary_json.write_text(json.dumps(summary, default=encode_default, indent=2) + "\n", encoding="utf-8")
    report_artifacts = {**artifacts, "cashSummaryJson": artifact(args.cash_summary_json)}
    args.report.parent.mkdir(parents=True, exist_ok=True)
    args.report.write_text(build_report(summary, report_artifacts), encoding="utf-8")

    print(json.dumps({
        "cashDays": len(days),
        "cashEvents": events["cashEventRows"],
        "anchors": anchors["count"],
        "missingContainingCandle": events["missingContainingCandle"],
        "report": args.report.as_posix(),
    }, indent=2))


if __name__ == "__main__":
    main()
