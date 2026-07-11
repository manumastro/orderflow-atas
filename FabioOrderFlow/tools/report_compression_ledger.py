#!/usr/bin/env python3
"""Export and describe the active compression event ledger.

The report is observational: it never computes PnL, emits a trade signal, or
promotes a threshold. It selects the latest completed historical replay from
an ATAS log, exports stable-profile/event/outcome records, and labels missing
cumulative-trade coverage explicitly.
"""

from __future__ import annotations

import argparse
import csv
import json
import os
import re
import sys
from collections import Counter, defaultdict
from dataclasses import dataclass
from datetime import datetime
from decimal import Decimal, InvalidOperation
from pathlib import Path
from statistics import median
from typing import Callable, Iterable

PROFILE_MARKER = "MR_COMPRESSION_LEDGER_PROFILE"
EVENT_MARKER = "MR_COMPRESSION_LEDGER_EVENT"
OUTCOME_MARKER = "MR_COMPRESSION_LEDGER_OUTCOME"
PROCESS_START_MARKER = "HISTORICAL_FLOW_PROCESS_START"
PROCESS_FINISH_MARKER = "HISTORICAL_FLOW_FINISH"
LOOKBACK_MARKER = "CUM_TRADES_LOOKBACK"
COMPLETE_MARKER = "CUM_TRADES_COMPLETE"

PROFILE_FIELDS = (
    "ProfileLabel", "ReadyBar", "ResolvedBar", "EndReason", "StudyBars",
    "HighTests", "LowTests", "BoundaryEvents", "BuyVolume", "SellVolume",
    "ProfileCVD", "TradeCoverage", "High", "Low", "POC", "Range",
    "RangeToBaselineMedian", "CompressionScore",
)
EVENT_FIELDS = (
    "ProfileLabel", "Bar", "Italy", "London", "UTC", "Boundary",
    "Interaction", "TestOrdinal", "OutsideCloseStreak", "BoundaryPrice",
    "EventClose", "CloseState", "BreachDistanceRanges", "CloseDistanceRanges",
    "BarRangeToBaselineMedian", "TradeCount", "TotalVolume", "BuyVolume",
    "SellVolume", "Delta", "ProfileCVD", "MaxBuyVolume", "MaxSellVolume",
    "TotalVolumePercentilePrior", "AbsoluteDeltaPercentilePrior",
    "MaxBuyPercentilePrior", "MaxSellPercentilePrior",
)
OUTCOME_FIELDS = (
    "ProfileLabel", "EventBar", "Boundary", "Interaction", "HorizonBars",
    "EndBar", "Italy", "London", "UTC", "EventClose", "EndClose",
    "CloseMoveRanges", "UpMfeRanges", "DownMaeRanges", "EndInsideRange",
    "PocTouched",
)

INTEGER_FIELDS = {"ReadyBar", "ResolvedBar", "StudyBars", "HighTests", "LowTests", "BoundaryEvents", "Bar", "TestOrdinal", "OutsideCloseStreak", "TradeCount", "EventBar", "HorizonBars", "EndBar"}
DECIMAL_FIELDS = {
    "BuyVolume", "SellVolume", "ProfileCVD", "High", "Low", "POC", "Range",
    "RangeToBaselineMedian", "CompressionScore", "BoundaryPrice", "EventClose",
    "BreachDistanceRanges", "CloseDistanceRanges", "BarRangeToBaselineMedian",
    "TotalVolume", "Delta", "MaxBuyVolume", "MaxSellVolume",
    "TotalVolumePercentilePrior", "AbsoluteDeltaPercentilePrior",
    "MaxBuyPercentilePrior", "MaxSellPercentilePrior", "EndClose",
    "CloseMoveRanges", "UpMfeRanges", "DownMaeRanges",
}
BOOLEAN_FIELDS = {"EndInsideRange", "PocTouched"}


@dataclass(frozen=True)
class ReplayRun:
    lines: list[str]
    start_line: int
    finish_line: int
    lookback: dict[str, str]
    complete: dict[str, str]
    finish: dict[str, str]


def default_log() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs" / "FabioOrderFlow.log"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs" / "FabioOrderFlow.log"


def event_body(line: str, marker: str) -> str:
    token = f"[{marker}]"
    return line.split(token, 1)[1].strip() if token in line else ""


def fields(body: str) -> dict[str, str]:
    return {
        match.group(1): match.group(2).strip()
        for match in re.finditer(r"(?:^|, )([A-Za-z][A-Za-z0-9_]*)=(.*?)(?=, [A-Za-z][A-Za-z0-9_]*=|$)", body)
    }


def number(value: str) -> Decimal | None:
    value = value.strip()
    if not value or value == "NA":
        return None
    normalized = value.replace(".", "").replace(",", ".") if "," in value else value
    try:
        return Decimal(normalized)
    except InvalidOperation:
        return None


def as_float(value: Decimal | None) -> float | None:
    return float(value) if value is not None else None


def converted(record: dict[str, str], keys: Iterable[str]) -> dict[str, object]:
    result: dict[str, object] = {}
    for key in keys:
        value = record.get(key, "")
        if key in INTEGER_FIELDS:
            result[key] = int(value) if value else None
        elif key in DECIMAL_FIELDS:
            result[key] = as_float(number(value))
        elif key in BOOLEAN_FIELDS:
            result[key] = value == "True"
        else:
            result[key] = value
    return result


def latest_completed_historical_run(lines: list[str]) -> ReplayRun:
    starts = [index for index, line in enumerate(lines) if f"[{PROCESS_START_MARKER}]" in line]
    if not starts:
        raise ValueError(f"No [{PROCESS_START_MARKER}] marker found")

    start_line = starts[-1]
    finish_line = next(
        (index for index in range(start_line + 1, len(lines)) if f"[{PROCESS_FINISH_MARKER}]" in lines[index]),
        -1,
    )
    if finish_line < 0:
        raise ValueError("The latest historical replay has not reached HISTORICAL_FLOW_FINISH")

    lookback_line = next(
        (index for index in range(start_line, -1, -1) if f"[{LOOKBACK_MARKER}]" in lines[index]),
        -1,
    )
    run_begin_line = lookback_line if lookback_line >= 0 else start_line
    complete_line = next(
        (index for index in range(run_begin_line, finish_line + 1) if f"[{COMPLETE_MARKER}]" in lines[index]),
        -1,
    )
    return ReplayRun(
        lines=lines[lookback_line if lookback_line >= 0 else start_line:finish_line + 1],
        start_line=start_line + 1,
        finish_line=finish_line + 1,
        lookback=fields(event_body(lines[lookback_line], LOOKBACK_MARKER)) if lookback_line >= 0 else {},
        complete=fields(event_body(lines[complete_line], COMPLETE_MARKER)) if complete_line >= 0 else {},
        finish=fields(event_body(lines[finish_line], PROCESS_FINISH_MARKER)),
    )


def records_for_marker(lines: Iterable[str], marker: str) -> list[dict[str, str]]:
    return [fields(event_body(line, marker)) for line in lines if f"[{marker}]" in line]


def event_key(event: dict[str, object]) -> tuple[str, int | None, str]:
    return (str(event["ProfileLabel"]), event["Bar"] if isinstance(event["Bar"], int) else None, str(event["Boundary"]))


def outcome_key(outcome: dict[str, object]) -> tuple[str, int | None, str]:
    return (str(outcome["ProfileLabel"]), outcome["EventBar"] if isinstance(outcome["EventBar"], int) else None, str(outcome["Boundary"]))


def percentile_band(value: object) -> str:
    if not isinstance(value, float):
        return "NA"
    if value <= 0.25:
        return "0.00-0.25"
    if value <= 0.50:
        return "0.26-0.50"
    if value <= 0.75:
        return "0.51-0.75"
    return "0.76-1.00"


def flow_coverage(event: dict[str, object]) -> str:
    return str(event.get("TradeCoverage") or "UNKNOWN")


def outcome_summary(outcomes: list[dict[str, object]]) -> dict[str, object]:
    close_moves = [value for outcome in outcomes if isinstance(value := outcome["CloseMoveRanges"], float)]
    up_mfes = [value for outcome in outcomes if isinstance(value := outcome["UpMfeRanges"], float)]
    down_maes = [value for outcome in outcomes if isinstance(value := outcome["DownMaeRanges"], float)]
    reversion_close_moves = [
        -value if outcome["Boundary"] == "HIGH" else value
        for outcome in outcomes
        if isinstance(value := outcome["CloseMoveRanges"], float)
    ]
    reversion_mfes = [
        outcome["DownMaeRanges"] if outcome["Boundary"] == "HIGH" else outcome["UpMfeRanges"]
        for outcome in outcomes
        if isinstance(outcome["DownMaeRanges"], float) and isinstance(outcome["UpMfeRanges"], float)
    ]
    continuation_mfes = [
        outcome["UpMfeRanges"] if outcome["Boundary"] == "HIGH" else outcome["DownMaeRanges"]
        for outcome in outcomes
        if isinstance(outcome["DownMaeRanges"], float) and isinstance(outcome["UpMfeRanges"], float)
    ]
    end_inside = sum(outcome["EndInsideRange"] is True for outcome in outcomes)
    poc_touched = sum(outcome["PocTouched"] is True for outcome in outcomes)
    count = len(outcomes)
    return {
        "outcomes": count,
        "averageCloseMoveRanges": round(sum(close_moves) / len(close_moves), 4) if close_moves else None,
        "medianCloseMoveRanges": round(median(close_moves), 4) if close_moves else None,
        "averageUpMfeRanges": round(sum(up_mfes) / len(up_mfes), 4) if up_mfes else None,
        "averageDownMaeRanges": round(sum(down_maes) / len(down_maes), 4) if down_maes else None,
        "averageReversionCloseMoveRanges": round(sum(reversion_close_moves) / len(reversion_close_moves), 4) if reversion_close_moves else None,
        "medianReversionCloseMoveRanges": round(median(reversion_close_moves), 4) if reversion_close_moves else None,
        "averageReversionMfeRanges": round(sum(reversion_mfes) / len(reversion_mfes), 4) if reversion_mfes else None,
        "averageContinuationMfeRanges": round(sum(continuation_mfes) / len(continuation_mfes), 4) if continuation_mfes else None,
        "endInsideCount": end_inside,
        "endInsideRate": round(end_inside / count, 4) if count else None,
        "pocTouchedCount": poc_touched,
        "pocTouchedRate": round(poc_touched / count, 4) if count else None,
    }


def grouped_outcomes(
    events: list[dict[str, object]],
    outcomes_by_event: dict[tuple[str, int | None, str], list[dict[str, object]]],
    group_name: str,
    categorizer: Callable[[dict[str, object]], str],
) -> list[dict[str, object]]:
    groups: dict[tuple[str, int | None], list[dict[str, object]]] = defaultdict(list)
    by_profile: dict[tuple[str, int | None, str], list[dict[str, object]]] = defaultdict(list)
    event_counts: Counter[tuple[str, int | None]] = Counter()
    for event in events:
        category = categorizer(event)
        key = event_key(event)
        profile_label = str(event["ProfileLabel"])
        for outcome in outcomes_by_event.get(key, []):
            group_key = (category, outcome["HorizonBars"] if isinstance(outcome["HorizonBars"], int) else None)
            groups[group_key].append(outcome)
            by_profile[(group_key[0], group_key[1], profile_label)].append(outcome)
            event_counts[group_key] += 1

    rows: list[dict[str, object]] = []
    for (category, horizon), group_outcomes in sorted(groups.items(), key=lambda item: (str(item[0][0]), item[0][1] or 0)):
        group_key = (category, horizon)
        per_profile = [
            outcome_summary(profile_outcomes)
            for (profile_category, profile_horizon, _), profile_outcomes in by_profile.items()
            if (profile_category, profile_horizon) == group_key
        ]

        def profile_weighted(metric: str) -> float | None:
            values = [float(value) for profile in per_profile if isinstance(value := profile[metric], (int, float))]
            return round(sum(values) / len(values), 4) if values else None

        row = {
            "aggregation": group_name,
            "group": category,
            "horizonBars": horizon,
            "events": event_counts[group_key],
            "profiles": len(per_profile),
            "profileWeightedAverageCloseMoveRanges": profile_weighted("averageCloseMoveRanges"),
            "profileWeightedAverageReversionCloseMoveRanges": profile_weighted("averageReversionCloseMoveRanges"),
            "profileWeightedEndInsideRate": profile_weighted("endInsideRate"),
            "profileWeightedPocTouchedRate": profile_weighted("pocTouchedRate"),
        }
        row.update(outcome_summary(group_outcomes))
        rows.append(row)
    return rows


def profile_summary(profiles: list[dict[str, object]]) -> list[dict[str, object]]:
    grouped: dict[str, list[dict[str, object]]] = defaultdict(list)
    for profile in profiles:
        grouped[str(profile["TradeCoverage"] or "UNKNOWN")].append(profile)

    summary: list[dict[str, object]] = []
    for coverage, rows in sorted(grouped.items()):
        def average(key: str) -> float | None:
            values = [float(value) for row in rows if isinstance(value := row[key], (int, float))]
            return round(sum(values) / len(values), 4) if values else None

        summary.append({
            "tradeCoverage": coverage,
            "profiles": len(rows),
            "boundaryEvents": sum(row["BoundaryEvents"] or 0 for row in rows),
            "averageRange": average("Range"),
            "averageRangeToBaselineMedian": average("RangeToBaselineMedian"),
            "averageCompressionScore": average("CompressionScore"),
            "averageStudyBars": average("StudyBars"),
        })
    return summary


def csv_rows(rows: Iterable[dict[str, object]], path: Path) -> None:
    rows = list(rows)
    columns = sorted({key for row in rows for key in row})
    with path.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.DictWriter(stream, fieldnames=columns)
        writer.writeheader()
        writer.writerows(rows)


def main() -> int:
    parser = argparse.ArgumentParser(description="Export and describe the active compression event ledger")
    parser.add_argument("--log", type=Path, default=default_log(), help="ATAS FabioOrderFlow.log path")
    parser.add_argument("--save", action="store_true", help="Write CSV datasets and the JSON report snapshot")
    parser.add_argument("--out-dir", type=Path, default=Path("FabioOrderFlow/ledger-snapshots"), help="Snapshot output directory")
    parser.add_argument("--prefix", default="compression-ledger", help="Snapshot filename prefix")
    args = parser.parse_args()

    if not args.log.exists():
        print(f"Missing log: {args.log}", file=sys.stderr)
        return 2

    try:
        run = latest_completed_historical_run(args.log.read_text(encoding="utf-8", errors="replace").splitlines())
    except ValueError as error:
        print(str(error), file=sys.stderr)
        return 2

    profiles = [converted(record, PROFILE_FIELDS) for record in records_for_marker(run.lines, PROFILE_MARKER)]
    events = [converted(record, EVENT_FIELDS) for record in records_for_marker(run.lines, EVENT_MARKER)]
    outcomes = [converted(record, OUTCOME_FIELDS) for record in records_for_marker(run.lines, OUTCOME_MARKER)]

    profiles_by_label = {str(profile["ProfileLabel"]): profile for profile in profiles}
    warnings: list[str] = []
    for event in events:
        profile = profiles_by_label.get(str(event["ProfileLabel"]))
        if profile is None:
            warnings.append(f"Event without profile: {event_key(event)}")
            event["TradeCoverage"] = "UNKNOWN"
        else:
            event["TradeCoverage"] = profile["TradeCoverage"]

    outcomes_by_event: dict[tuple[str, int | None, str], list[dict[str, object]]] = defaultdict(list)
    event_keys = {event_key(event) for event in events}
    for outcome in outcomes:
        key = outcome_key(outcome)
        if key not in event_keys:
            warnings.append(f"Outcome without event: {key}, horizon={outcome['HorizonBars']}")
        outcomes_by_event[key].append(outcome)
        profile = profiles_by_label.get(str(outcome["ProfileLabel"]))
        outcome["TradeCoverage"] = profile["TradeCoverage"] if profile else "UNKNOWN"

    events_all = events
    events_flow_covered = [event for event in events if flow_coverage(event) == "AVAILABLE"]
    flow_groups = {
        "boundary": grouped_outcomes(events_flow_covered, outcomes_by_event, "boundary", lambda event: str(event["Boundary"])),
        "allEvents": grouped_outcomes(events_flow_covered, outcomes_by_event, "allEvents", lambda event: "ALL"),
        "interaction": grouped_outcomes(events_flow_covered, outcomes_by_event, "interaction", lambda event: str(event["Interaction"])),
        "testOrdinal": grouped_outcomes(events_flow_covered, outcomes_by_event, "testOrdinal", lambda event: "FIRST_TEST" if event["TestOrdinal"] == 1 else "REPEATED_TEST"),
        "closeState": grouped_outcomes(events_flow_covered, outcomes_by_event, "closeState", lambda event: str(event["CloseState"])),
        "totalVolumePercentilePrior": grouped_outcomes(events_flow_covered, outcomes_by_event, "totalVolumePercentilePrior", lambda event: percentile_band(event["TotalVolumePercentilePrior"])),
        "absoluteDeltaPercentilePrior": grouped_outcomes(events_flow_covered, outcomes_by_event, "absoluteDeltaPercentilePrior", lambda event: percentile_band(event["AbsoluteDeltaPercentilePrior"])),
    }
    all_groups = {
        "boundary": grouped_outcomes(events_all, outcomes_by_event, "boundary", lambda event: str(event["Boundary"])),
        "allEvents": grouped_outcomes(events_all, outcomes_by_event, "allEvents", lambda event: "ALL"),
        "interaction": grouped_outcomes(events_all, outcomes_by_event, "interaction", lambda event: str(event["Interaction"])),
        "testOrdinal": grouped_outcomes(events_all, outcomes_by_event, "testOrdinal", lambda event: "FIRST_TEST" if event["TestOrdinal"] == 1 else "REPEATED_TEST"),
        "closeState": grouped_outcomes(events_all, outcomes_by_event, "closeState", lambda event: str(event["CloseState"])),
    }

    expected_outcomes = len(events) * 4
    summary: dict[str, object] = {
        "report": "compression-event-ledger-descriptive",
        "generatedAtLocal": datetime.now().astimezone().isoformat(timespec="seconds"),
        "run": {
            "startLine": run.start_line,
            "finishLine": run.finish_line,
            "lookback": run.lookback,
            "complete": run.complete,
            "finish": run.finish,
        },
        "counts": {
            "profiles": len(profiles),
            "events": len(events),
            "outcomes": len(outcomes),
            "expectedOutcomesAtFourHorizons": expected_outcomes,
            "flowCoveredEvents": len(events_flow_covered),
            "missingCoverageEvents": len(events) - len(events_flow_covered),
        },
        "validation": {
            "outcomesComplete": len(outcomes) == expected_outcomes,
            "entries": run.finish.get("Entries"),
            "closedPositions": run.finish.get("ClosedPositions"),
            "openPositions": run.finish.get("OpenPositions"),
            "completedTrades": run.finish.get("CompletedTrades"),
            "warnings": warnings,
        },
        "profileCoverage": profile_summary(profiles),
        "allOutcomeGroups": all_groups,
        "flowCoveredOutcomeGroups": flow_groups,
        "guardrail": "Descriptive ledger analysis only. It does not create an entry, a threshold, a stop, a target, or PnL.",
    }
    if args.save:
        args.out_dir.mkdir(parents=True, exist_ok=True)
        timestamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        base = args.out_dir / f"{args.prefix}-{timestamp}"
        csv_rows(profiles, base.with_name(base.name + "-profiles.csv"))
        csv_rows(events, base.with_name(base.name + "-events.csv"))
        csv_rows(outcomes, base.with_name(base.name + "-outcomes.csv"))
        aggregation_rows = [row for groups in flow_groups.values() for row in groups]
        csv_rows(aggregation_rows, base.with_name(base.name + "-flow-covered-aggregates.csv"))
        base.with_name(base.name + "-summary.json").write_text(json.dumps(summary, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")

    print(json.dumps(summary, indent=2, ensure_ascii=True))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
