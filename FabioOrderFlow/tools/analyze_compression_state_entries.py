#!/usr/bin/env python3
"""Generate causal state-confirmed shadow entries from a ledger snapshot.

Stdout is JSON only. No ATAS order, stop, target, or PnL is created.
"""

from __future__ import annotations

import argparse
import json
import math
import statistics
from collections import defaultdict
from datetime import datetime
from pathlib import Path

from analyze_compression_shadow import (
    DEFAULT_SNAPSHOT_DIR,
    continuation_mfe,
    latest_snapshot_prefix,
    optional_float,
    profile_date,
    read_csv,
    reversion_close,
    reversion_mfe,
    snapshot_paths,
)

HORIZONS = (6, 12)


def integer(value: str) -> int:
    return int(value)


def event_order(event: dict[str, str]) -> tuple[int, str]:
    return (integer(event["Bar"]), event["Boundary"])


def failed_breakout_entry(events: list[dict[str, str]]) -> dict[str, str] | None:
    prior_outside: set[str] = set()
    by_bar: dict[int, list[dict[str, str]]] = defaultdict(list)
    for event in events:
        by_bar[integer(event["Bar"])].append(event)

    for bar in sorted(by_bar):
        current = sorted(by_bar[bar], key=event_order)
        candidates = [
            event for event in current
            if event["CloseState"] == "INSIDE" and event["Boundary"] in prior_outside
        ]
        if candidates:
            return candidates[0]
        for event in current:
            if event["CloseState"] == "OUTSIDE":
                prior_outside.add(event["Boundary"])
    return None


def acceptance_entry(events: list[dict[str, str]]) -> dict[str, str] | None:
    candidates = [
        event for event in events
        if event["CloseState"] == "OUTSIDE" and integer(event["OutsideCloseStreak"]) >= 2
    ]
    return min(candidates, key=event_order) if candidates else None


def model_direction(model: str, boundary: str) -> str:
    if model == "FAILED_BREAKOUT_REVERSION":
        return "SHORT" if boundary == "HIGH" else "LONG"
    return "LONG" if boundary == "HIGH" else "SHORT"


def model_move(model: str, outcome: dict[str, str]) -> float:
    reversion = reversion_close(outcome)
    return reversion if model == "FAILED_BREAKOUT_REVERSION" else -reversion


def model_favorable_mfe(model: str, outcome: dict[str, str]) -> float:
    return reversion_mfe(outcome) if model == "FAILED_BREAKOUT_REVERSION" else continuation_mfe(outcome)


def model_adverse_mfe(model: str, outcome: dict[str, str]) -> float:
    return continuation_mfe(outcome) if model == "FAILED_BREAKOUT_REVERSION" else reversion_mfe(outcome)


def mean(values: list[float]) -> float | None:
    return round(statistics.fmean(values), 4) if values else None


def median(values: list[float]) -> float | None:
    return round(statistics.median(values), 4) if values else None


def summarize(entries: list[dict[str, object]], dates: set[str], horizon: int) -> dict[str, object]:
    rows = [entry for entry in entries if entry["date"] in dates and entry["horizonBars"] == horizon]
    if not rows:
        return {
            "entries": 0,
            "dates": 0,
            "averageMoveRanges": None,
            "medianMoveRanges": None,
            "positiveMoveRate": None,
            "dateWeightedAverageMoveRanges": None,
            "averageFavorableMfeRanges": None,
            "averageAdverseMfeRanges": None,
            "pocTouchedRate": None,
            "endInsideRate": None,
        }

    moves = [float(row["moveRanges"]) for row in rows]
    by_date: dict[str, list[float]] = defaultdict(list)
    for row in rows:
        by_date[str(row["date"])].append(float(row["moveRanges"]))
    count = len(rows)
    return {
        "entries": count,
        "dates": len(by_date),
        "averageMoveRanges": mean(moves),
        "medianMoveRanges": median(moves),
        "positiveMoveRate": round(sum(move > 0 for move in moves) / count, 4),
        "dateWeightedAverageMoveRanges": mean([statistics.fmean(values) for values in by_date.values()]),
        "averageFavorableMfeRanges": mean([float(row["favorableMfeRanges"]) for row in rows]),
        "averageAdverseMfeRanges": mean([float(row["adverseMfeRanges"]) for row in rows]),
        "pocTouchedRate": round(sum(bool(row["pocTouched"]) for row in rows) / count, 4),
        "endInsideRate": round(sum(bool(row["endInsideRange"]) for row in rows) / count, 4),
    }


def model_summary(entries: list[dict[str, object]], train_dates: set[str], test_dates: set[str]) -> dict[str, object]:
    profiles = {str(entry["profileLabel"]) for entry in entries}
    result: dict[str, object] = {"profiles": len(profiles)}
    for horizon in HORIZONS:
        result[f"horizon{horizon}"] = {
            "train": summarize(entries, train_dates, horizon),
            "test": summarize(entries, test_dates, horizon),
            "all": summarize(entries, train_dates | test_dates, horizon),
        }
    return result


def main() -> int:
    parser = argparse.ArgumentParser(description="Generate causal compression state shadow entries")
    parser.add_argument("--snapshot-prefix", type=Path)
    parser.add_argument("--snapshot-dir", type=Path, default=DEFAULT_SNAPSHOT_DIR)
    parser.add_argument("--train-fraction", type=float, default=0.70)
    parser.add_argument("--save", type=Path, help="Optional JSON path; stdout remains JSON only")
    args = parser.parse_args()

    try:
        if not 0.5 <= args.train_fraction <= 0.8:
            raise ValueError("train-fraction must be between 0.5 and 0.8")
        prefix = args.snapshot_prefix or latest_snapshot_prefix(args.snapshot_dir)
        paths = snapshot_paths(prefix)
        missing = [str(path) for path in paths.values() if not path.exists()]
        if missing:
            raise ValueError(f"Missing snapshot files: {missing}")

        ledger_summary = json.loads(paths["summary"].read_text(encoding="utf-8"))
        profiles = {
            row["ProfileLabel"]: row
            for row in read_csv(paths["profiles"])
            if row["TradeCoverage"] == "AVAILABLE"
        }
        events_by_profile: dict[str, list[dict[str, str]]] = defaultdict(list)
        for event in read_csv(paths["events"]):
            if event["ProfileLabel"] in profiles:
                events_by_profile[event["ProfileLabel"]].append(event)
        outcomes = {
            (row["ProfileLabel"], integer(row["EventBar"]), row["Boundary"], integer(row["HorizonBars"])): row
            for row in read_csv(paths["outcomes"])
        }

        selected: dict[str, dict[str, dict[str, str]]] = defaultdict(dict)
        for label, events in events_by_profile.items():
            ordered = sorted(events, key=event_order)
            failed = failed_breakout_entry(ordered)
            acceptance = acceptance_entry(ordered)
            if failed:
                selected["FAILED_BREAKOUT_REVERSION"][label] = failed
            if acceptance:
                selected["ACCEPTANCE_CONTINUATION"][label] = acceptance
            confirmed = min(
                (("FAILED_BREAKOUT_REVERSION", failed), ("ACCEPTANCE_CONTINUATION", acceptance)),
                key=lambda pair: event_order(pair[1]) if pair[1] else (10**9, ""),
            )
            if confirmed[1]:
                selected["FIRST_CONFIRMED_STATE"][label] = {**confirmed[1], "SelectedModel": confirmed[0]}

        entries: list[dict[str, object]] = []
        for selection_name, by_profile in selected.items():
            for label, event in by_profile.items():
                model = event.get("SelectedModel", selection_name)
                for horizon in HORIZONS:
                    outcome = outcomes.get((label, integer(event["Bar"]), event["Boundary"], horizon))
                    if not outcome:
                        continue
                    entries.append({
                        "selection": selection_name,
                        "model": model,
                        "profileLabel": label,
                        "date": profile_date(label),
                        "eventBar": integer(event["Bar"]),
                        "boundary": event["Boundary"],
                        "direction": model_direction(model, event["Boundary"]),
                        "entryPrice": float(event["EventClose"]),
                        "closeState": event["CloseState"],
                        "outsideCloseStreak": integer(event["OutsideCloseStreak"]),
                        "totalVolumePercentilePrior": optional_float(event["TotalVolumePercentilePrior"]),
                        "absoluteDeltaPercentilePrior": optional_float(event["AbsoluteDeltaPercentilePrior"]),
                        "horizonBars": horizon,
                        "exitBar": integer(outcome["EndBar"]),
                        "exitPrice": float(outcome["EndClose"]),
                        "moveRanges": model_move(model, outcome),
                        "favorableMfeRanges": model_favorable_mfe(model, outcome),
                        "adverseMfeRanges": model_adverse_mfe(model, outcome),
                        "pocTouched": outcome["PocTouched"] == "True",
                        "endInsideRange": outcome["EndInsideRange"] == "True",
                    })

        dates = sorted({profile_date(label) for label in profiles})
        split_index = max(1, min(len(dates) - 1, math.floor(len(dates) * args.train_fraction)))
        train_dates = set(dates[:split_index])
        test_dates = set(dates[split_index:])
        summaries: dict[str, object] = {}
        for selection_name in selected:
            selection_entries = [entry for entry in entries if entry["selection"] == selection_name]
            summary = model_summary(selection_entries, train_dates, test_dates)
            summary["byBoundary"] = {
                boundary: model_summary(
                    [entry for entry in selection_entries if entry["boundary"] == boundary],
                    train_dates,
                    test_dates,
                )
                for boundary in ("HIGH", "LOW")
            }
            summaries[selection_name] = summary

        minimum_test_entries = 8
        candidates_meeting_minimum = [
            name for name, result in summaries.items()
            if result["horizon6"]["test"]["entries"] >= minimum_test_entries
        ]
        report = {
            "report": "compression-state-shadow-entries-exploratory",
            "generatedAtLocal": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "snapshotPrefix": str(prefix),
                "ledgerCounts": ledger_summary["counts"],
                "flowCoveredProfiles": len(profiles),
            },
            "contracts": {
                "FAILED_BREAKOUT_REVERSION": {
                    "trigger": "a prior OUTSIDE close on the same boundary, followed by a boundary event closing INSIDE",
                    "direction": "toward the frozen range and POC",
                },
                "ACCEPTANCE_CONTINUATION": {
                    "trigger": "OUTSIDE close with OutsideCloseStreak >= 2",
                    "direction": "away from the frozen range",
                },
                "FIRST_CONFIRMED_STATE": {
                    "trigger": "the first causal trigger among failed breakout and acceptance",
                    "direction": "selected by the trigger type",
                },
                "common": {
                    "entry": "trigger event close",
                    "exit": "close after exactly 6 or 12 bars",
                    "maximumEntriesPerProfilePerContract": 1,
                    "usesProfileEndReason": False,
                    "orders": False,
                    "stop": None,
                    "target": None,
                    "pnl": None,
                    "costs": None,
                },
            },
            "split": {
                "method": "chronological distinct profile dates",
                "trainDates": sorted(train_dates),
                "testDates": sorted(test_dates),
                "selectionLeakage": True,
                "selectionLeakageReason": "State contracts were formulated after inspecting the overlapping ledger sample.",
            },
            "summaries": summaries,
            "entries": entries,
            "assessment": {
                "minimumTestEntriesPerContract": minimum_test_entries,
                "contractsMeetingMinimum": candidates_meeting_minimum,
                "validatedContracts": [],
                "conclusion": "Exploratory causal shadow entries only; promotion requires stable H6/H12 behavior and independent unseen sessions.",
            },
            "guardrail": "No executable ATAS order, stop, target, or PnL is produced.",
        }
    except (ValueError, KeyError, OSError, json.JSONDecodeError) as error:
        print(json.dumps({"error": str(error)}, ensure_ascii=True))
        return 2

    rendered = json.dumps(report, indent=2, ensure_ascii=True) + "\n"
    if args.save:
        args.save.parent.mkdir(parents=True, exist_ok=True)
        args.save.write_text(rendered, encoding="utf-8")
    print(rendered, end="")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
