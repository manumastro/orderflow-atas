#!/usr/bin/env python3
"""Report legacy auction-state bars or validate an NY-only summary as JSON."""

from __future__ import annotations

import argparse
import csv
import json
import os
import re
import sys
from collections import Counter, defaultdict
from datetime import datetime
from decimal import Decimal, InvalidOperation
from pathlib import Path

BAR_MARKER = "AUCTION_STATE_BAR"
MODE_MARKER = "AUCTION_STATE_MODE"
SUMMARY_MARKER = "AUCTION_STATE_SUMMARY"

FIELDS = (
    "StudyMode", "Session", "SessionDate", "SessionBarOrdinal", "Bar",
    "ChartTimeFrame", "Italy", "London", "UTC", "NewYork", "Open", "High",
    "Low", "Close", "CandleVolume", "BidVolume", "AskVolume", "Delta",
    "DeltaImbalance", "MaxBidAtPrice", "MaxAskAtPrice", "CumulativeTradeCount",
    "CumulativeBuyVolume", "CumulativeSellVolume", "CumulativeDelta",
    "MaxCumulativeBuy", "MaxCumulativeSell", "CumulativeTradeCoverage", "PriceChange",
    "CloseLocation", "EffortResult", "PriorProfileBars", "PriorPOC",
    "PriorVAH", "PriorVAL", "PriorProfileRelation", "DevelopingPOC",
    "DevelopingVAH", "DevelopingVAL", "DevelopingProfileRelation",
    "Prior6Range", "Prior6Efficiency", "Prior12Range", "Prior12Efficiency",
    "RangeToPrior12Median", "OverlapWithPrevious", "RollingProfileBars",
    "RollingPOC", "RollingVAH", "RollingVAL", "PriorSessionLvnBelow",
    "PriorSessionLvnBelowVolumePercentile", "PriorSessionLvnAbove",
    "PriorSessionLvnAboveVolumePercentile", "PriorRollingLvnBelow",
    "PriorRollingLvnBelowVolumePercentile", "PriorRollingLvnAbove",
    "PriorRollingLvnAboveVolumePercentile", "FlowSource", "OperationalEntry",
    "OrderSubmitted",
)
INTEGER_FIELDS = {"SessionBarOrdinal", "Bar", "PriorProfileBars", "RollingProfileBars", "CumulativeTradeCount"}
DECIMAL_FIELDS = {
    "Open", "High", "Low", "Close", "CandleVolume", "BidVolume", "AskVolume",
    "Delta", "DeltaImbalance", "MaxBidAtPrice", "MaxAskAtPrice", "CumulativeBuyVolume",
    "CumulativeSellVolume", "CumulativeDelta", "MaxCumulativeBuy", "MaxCumulativeSell", "PriceChange",
    "CloseLocation", "PriorPOC", "PriorVAH", "PriorVAL", "DevelopingPOC",
    "DevelopingVAH", "DevelopingVAL", "Prior6Range", "Prior6Efficiency",
    "Prior12Range", "Prior12Efficiency", "RangeToPrior12Median",
    "OverlapWithPrevious", "RollingPOC", "RollingVAH", "RollingVAL",
    "PriorSessionLvnBelow", "PriorSessionLvnBelowVolumePercentile",
    "PriorSessionLvnAbove", "PriorSessionLvnAboveVolumePercentile",
    "PriorRollingLvnBelow", "PriorRollingLvnBelowVolumePercentile",
    "PriorRollingLvnAbove", "PriorRollingLvnAboveVolumePercentile",
}
BOOLEAN_FIELDS = {"OperationalEntry", "OrderSubmitted"}


def default_log() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs" / "FabioOrderFlow.log"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs" / "FabioOrderFlow.log"


def event_body(line: str, marker: str) -> str:
    token = f"[{marker}]"
    return line.split(token, 1)[1].lstrip(" ,") if token in line else ""


def raw_fields(body: str) -> dict[str, str]:
    return {
        match.group(1): match.group(2).strip()
        for match in re.finditer(
            r"(?:^|, )([A-Za-z][A-Za-z0-9_]*)=(.*?)(?=, [A-Za-z][A-Za-z0-9_]*=|$)",
            body,
        )
    }


def number(value: str) -> float | None:
    if not value or value == "NA":
        return None
    normalized = value.replace(".", "").replace(",", ".") if "," in value else value
    try:
        return float(Decimal(normalized))
    except InvalidOperation:
        return None


def convert(record: dict[str, str]) -> dict[str, object]:
    result: dict[str, object] = {}
    for key in FIELDS:
        value = record.get(key, "")
        if key in INTEGER_FIELDS:
            result[key] = int(value) if value else None
        elif key in DECIMAL_FIELDS:
            result[key] = number(value)
        elif key in BOOLEAN_FIELDS:
            result[key] = value.upper() == "TRUE"
        else:
            result[key] = value
    return result


def latest_run(lines: list[str]) -> tuple[dict[str, str], list[dict[str, object]], dict[str, str]]:
    mode_indexes = [index for index, line in enumerate(lines) if f"[{MODE_MARKER}]" in line]
    if not mode_indexes:
        raise ValueError(f"No [{MODE_MARKER}] marker found; reload the updated indicator")
    start = mode_indexes[-1]
    summary = next(
        (index for index in range(start + 1, len(lines)) if f"[{SUMMARY_MARKER}]" in lines[index]),
        -1,
    )
    if summary < 0:
        raise ValueError("The latest auction-state run has no completed summary")
    mode = raw_fields(event_body(lines[start], MODE_MARKER))
    records = [
        convert(raw_fields(event_body(line, BAR_MARKER)))
        for line in lines[start:summary]
        if f"[{BAR_MARKER}]" in line
    ]
    finish = raw_fields(event_body(lines[summary], SUMMARY_MARKER))
    return mode, records, finish


def nested_counts(records: list[dict[str, object]], field: str) -> dict[str, dict[str, int]]:
    grouped: dict[str, Counter[str]] = defaultdict(Counter)
    for record in records:
        grouped[str(record["Session"])][str(record[field])] += 1
    return {session: dict(sorted(counts.items())) for session, counts in sorted(grouped.items())}


def build_report(log_path: Path) -> tuple[dict[str, object], list[dict[str, object]]]:
    lines = log_path.read_text(encoding="utf-8", errors="replace").splitlines()
    mode, records, finish = latest_run(lines)
    sessions = Counter(str(record["Session"]) for record in records)
    session_dates: dict[str, set[str]] = defaultdict(set)
    for record in records:
        session_dates[str(record["Session"])].add(str(record["SessionDate"]))

    outside_counts: dict[str, Counter[str]] = defaultdict(Counter)
    absorption_at_location: dict[str, Counter[str]] = defaultdict(Counter)
    for record in records:
        session = str(record["Session"])
        relation = str(record["PriorProfileRelation"])
        effort = str(record["EffortResult"])
        if relation in {"ABOVE_VAH", "BELOW_VAL"}:
            outside_counts[session][relation] += 1
            if effort in {"BUY_ABSORBED", "SELL_ABSORBED"}:
                absorption_at_location[session][f"{relation}:{effort}"] += 1

    operational = [record for record in records if record["OperationalEntry"] or record["OrderSubmitted"]]
    configured_sessions = tuple(filter(None, mode.get("Sessions", "LONDON|NEW_YORK").split("|")))
    auction_state_bars_enabled = mode.get("AuctionStateBars", "ENABLED") != "DISABLED"
    summary_sessions = {
        "LONDON": int(finish.get("LondonBars", 0)),
        "NEW_YORK": int(finish.get("NewYorkBars", 0)),
    }
    configured_sessions_present = all(
        (sessions[session] if auction_state_bars_enabled else summary_sessions.get(session, 0)) > 0
        for session in configured_sessions
    )
    warnings: list[str] = []
    if auction_state_bars_enabled:
        for session in configured_sessions:
            if sessions[session] == 0:
                warnings.append(f"No completed {session} bars")

    report: dict[str, object] = {
        "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
        "source": {"log": str(log_path), "mode": mode, "summary": finish},
        "contract": {
            "studyMode": mode.get("StudyMode", "UNKNOWN"),
            "configuredSessions": configured_sessions,
            "auctionStateBarsEnabled": auction_state_bars_enabled,
            "flowSource": "CANDLE_FOOTPRINT|CUMULATIVE_TRADES",
            "cumulativeBigTradesIncluded": True,
            "signalsGenerated": False,
            "pnlComputed": False,
            "selectionThresholds": [],
        },
        "counts": {
            "records": len(records),
            "sessions": dict(sorted(sessions.items())),
            "summarySessions": summary_sessions,
            "sessionDates": {key: len(value) for key, value in sorted(session_dates.items())},
            "uniqueChartBars": len({record["Bar"] for record in records}),
        },
        "descriptive": {
            "effortResultBySession": nested_counts(records, "EffortResult"),
            "priorProfileRelationBySession": nested_counts(records, "PriorProfileRelation"),
            "outsidePriorValueBySession": {
                key: dict(sorted(value.items())) for key, value in sorted(outside_counts.items())
            },
            "absorptionOutsidePriorValueBySession": {
                key: dict(sorted(value.items())) for key, value in sorted(absorption_at_location.items())
            },
        },
        "validation": {
            "summaryPresent": True,
            "bothSessionsPresent": sessions["LONDON"] > 0 and sessions["NEW_YORK"] > 0,
            "configuredSessionsPresent": configured_sessions_present,
            "summaryOnlyMode": not auction_state_bars_enabled,
            "nonOperational": len(operational) == 0,
            "warnings": warnings,
        },
    }
    return report, records


def save_outputs(report: dict[str, object], records: list[dict[str, object]], directory: Path) -> dict[str, str]:
    directory.mkdir(parents=True, exist_ok=True)
    stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    base = directory / f"auction-state-{stamp}"
    json_path = base.with_name(f"{base.name}-summary.json")
    csv_path = base.with_name(f"{base.name}-bars.csv")
    json_path.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
    with csv_path.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELDS, extrasaction="ignore")
        writer.writeheader()
        writer.writerows(records)
    return {"summaryJson": str(json_path), "barsCsv": str(csv_path)}


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--log", type=Path, default=default_log())
    parser.add_argument("--save", action="store_true")
    parser.add_argument(
        "--output-dir",
        type=Path,
        default=Path("FabioOrderFlow") / "ledger-snapshots",
    )
    args = parser.parse_args()

    try:
        report, records = build_report(args.log)
        if args.save:
            report["saved"] = save_outputs(report, records, args.output_dir)
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc), "log": str(args.log)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
