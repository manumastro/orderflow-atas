#!/usr/bin/env python3
"""Exploratory fixed-horizon shadow analysis for compression ledger snapshots.

This tool never reconstructs orders, stops, targets, or PnL. It uses at most
one boundary event per profile and exits at an observed fixed bar horizon.
Stdout is always one JSON document on success.
"""

from __future__ import annotations

import argparse
import csv
import json
import math
import statistics
import sys
from collections import defaultdict
from datetime import datetime
from pathlib import Path
from typing import Callable

DEFAULT_SNAPSHOT_DIR = Path("FabioOrderFlow/ledger-snapshots")
HORIZONS = (6, 12)


def optional_float(value: str) -> float | None:
    if value in ("", "NA"):
        return None
    return float(value)


def integer(value: str) -> int:
    return int(value)


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8", newline="") as stream:
        return list(csv.DictReader(stream))


def latest_snapshot_prefix(directory: Path) -> Path:
    summaries = sorted(directory.glob("compression-ledger-*-summary.json"), key=lambda path: path.stat().st_mtime)
    if not summaries:
        raise ValueError(f"No compression ledger summary found in {directory}")
    suffix = "-summary.json"
    return summaries[-1].with_name(summaries[-1].name.removesuffix(suffix))


def snapshot_paths(prefix: Path) -> dict[str, Path]:
    return {
        "summary": prefix.with_name(prefix.name + "-summary.json"),
        "profiles": prefix.with_name(prefix.name + "-profiles.csv"),
        "events": prefix.with_name(prefix.name + "-events.csv"),
        "outcomes": prefix.with_name(prefix.name + "-outcomes.csv"),
    }


def profile_date(profile_label: str) -> str:
    return profile_label.split(":", 1)[0]


def first_event(events: list[dict[str, str]]) -> dict[str, str]:
    first_bar = min(integer(event["Bar"]) for event in events)
    same_bar = [event for event in events if integer(event["Bar"]) == first_bar]
    return sorted(
        same_bar,
        key=lambda event: (-(optional_float(event["BreachDistanceRanges"]) or 0.0), event["Boundary"]),
    )[0]


def reversion_close(outcome: dict[str, str]) -> float:
    move = float(outcome["CloseMoveRanges"])
    return -move if outcome["Boundary"] == "HIGH" else move


def reversion_mfe(outcome: dict[str, str]) -> float:
    return float(outcome["DownMaeRanges"] if outcome["Boundary"] == "HIGH" else outcome["UpMfeRanges"])


def continuation_mfe(outcome: dict[str, str]) -> float:
    return float(outcome["UpMfeRanges"] if outcome["Boundary"] == "HIGH" else outcome["DownMaeRanges"])


def mean(values: list[float]) -> float | None:
    return round(statistics.fmean(values), 4) if values else None


def median(values: list[float]) -> float | None:
    return round(statistics.median(values), 4) if values else None


def fixed_horizon_stats(rows: list[dict[str, object]]) -> dict[str, object]:
    if not rows:
        return {
            "observations": 0,
            "dates": 0,
            "averageReversionCloseRanges": None,
            "medianReversionCloseRanges": None,
            "positiveReversionRate": None,
            "dateWeightedAverageReversionCloseRanges": None,
            "averageReversionMfeRanges": None,
            "averageContinuationMfeRanges": None,
            "pocTouchedRate": None,
            "endInsideRate": None,
        }

    moves = [float(row["reversionCloseRanges"]) for row in rows]
    by_date: dict[str, list[float]] = defaultdict(list)
    for row in rows:
        by_date[str(row["date"])].append(float(row["reversionCloseRanges"]))
    date_moves = [statistics.fmean(date_values) for date_values in by_date.values()]
    count = len(rows)
    return {
        "observations": count,
        "dates": len(by_date),
        "averageReversionCloseRanges": mean(moves),
        "medianReversionCloseRanges": median(moves),
        "positiveReversionRate": round(sum(value > 0 for value in moves) / count, 4),
        "dateWeightedAverageReversionCloseRanges": mean(date_moves),
        "averageReversionMfeRanges": mean([float(row["reversionMfeRanges"]) for row in rows]),
        "averageContinuationMfeRanges": mean([float(row["continuationMfeRanges"]) for row in rows]),
        "pocTouchedRate": round(sum(bool(row["pocTouched"]) for row in rows) / count, 4),
        "endInsideRate": round(sum(bool(row["endInsideRange"]) for row in rows) / count, 4),
    }


def candidate_result(
    observations: list[dict[str, object]],
    train_dates: set[str],
    test_dates: set[str],
    predicate: Callable[[dict[str, object]], bool],
) -> dict[str, object]:
    eligible = [row for row in observations if predicate(row)]
    result: dict[str, object] = {"eligibleProfiles": len({str(row["profileLabel"]) for row in eligible})}
    for horizon in HORIZONS:
        horizon_rows = [row for row in eligible if row["horizonBars"] == horizon]
        result[f"horizon{horizon}"] = {
            "train": fixed_horizon_stats([row for row in horizon_rows if row["date"] in train_dates]),
            "test": fixed_horizon_stats([row for row in horizon_rows if row["date"] in test_dates]),
            "all": fixed_horizon_stats(horizon_rows),
        }
    return result


def main() -> int:
    parser = argparse.ArgumentParser(description="Exploratory fixed-horizon compression shadow analysis")
    parser.add_argument("--snapshot-prefix", type=Path, help="Snapshot path without -summary.json suffix")
    parser.add_argument("--snapshot-dir", type=Path, default=DEFAULT_SNAPSHOT_DIR)
    parser.add_argument("--train-fraction", type=float, default=0.70)
    parser.add_argument("--save", type=Path, help="Optional JSON output path; stdout remains JSON only")
    args = parser.parse_args()

    if not 0.5 <= args.train_fraction <= 0.8:
        print(json.dumps({"error": "train-fraction must be between 0.5 and 0.8"}))
        return 2

    try:
        prefix = args.snapshot_prefix or latest_snapshot_prefix(args.snapshot_dir)
        paths = snapshot_paths(prefix)
        missing = [str(path) for path in paths.values() if not path.exists()]
        if missing:
            raise ValueError(f"Missing snapshot files: {missing}")

        summary = json.loads(paths["summary"].read_text(encoding="utf-8"))
        profile_rows = read_csv(paths["profiles"])
        event_rows = read_csv(paths["events"])
        outcome_rows = read_csv(paths["outcomes"])

        profiles = {
            row["ProfileLabel"]: row
            for row in profile_rows
            if row["TradeCoverage"] == "AVAILABLE"
        }
        events_by_profile: dict[str, list[dict[str, str]]] = defaultdict(list)
        for row in event_rows:
            if row["ProfileLabel"] in profiles:
                events_by_profile[row["ProfileLabel"]].append(row)

        selected_events = {
            label: first_event(events)
            for label, events in events_by_profile.items()
            if events
        }
        outcomes_by_key = {
            (row["ProfileLabel"], integer(row["EventBar"]), row["Boundary"], integer(row["HorizonBars"])): row
            for row in outcome_rows
        }

        observations: list[dict[str, object]] = []
        for label, event in selected_events.items():
            profile = profiles[label]
            for horizon in HORIZONS:
                outcome = outcomes_by_key.get((label, integer(event["Bar"]), event["Boundary"], horizon))
                if outcome is None:
                    continue
                observations.append({
                    "profileLabel": label,
                    "date": profile_date(label),
                    "horizonBars": horizon,
                    "boundary": event["Boundary"],
                    "interaction": event["Interaction"],
                    "closeState": event["CloseState"],
                    "totalVolumePercentilePrior": optional_float(event["TotalVolumePercentilePrior"]),
                    "absoluteDeltaPercentilePrior": optional_float(event["AbsoluteDeltaPercentilePrior"]),
                    "rangeToBaselineMedian": float(profile["RangeToBaselineMedian"]),
                    "compressionScore": float(profile["CompressionScore"]),
                    "reversionCloseRanges": reversion_close(outcome),
                    "reversionMfeRanges": reversion_mfe(outcome),
                    "continuationMfeRanges": continuation_mfe(outcome),
                    "pocTouched": outcome["PocTouched"] == "True",
                    "endInsideRange": outcome["EndInsideRange"] == "True",
                })

        dates = sorted({str(row["date"]) for row in observations})
        split_index = max(1, min(len(dates) - 1, math.floor(len(dates) * args.train_fraction)))
        train_dates = set(dates[:split_index])
        test_dates = set(dates[split_index:])
        train_profiles = [profiles[label] for label in selected_events if profile_date(label) in train_dates]
        compact_threshold = statistics.median(float(profile["RangeToBaselineMedian"]) for profile in train_profiles)

        candidates: dict[str, Callable[[dict[str, object]], bool]] = {
            "BASELINE_FIRST_EVENT_REVERSION": lambda row: True,
            "HIGH_BOUNDARY_REVERSION": lambda row: row["boundary"] == "HIGH",
            "LOW_BOUNDARY_REVERSION": lambda row: row["boundary"] == "LOW",
            "CLOSE_INSIDE_REVERSION": lambda row: row["closeState"] == "INSIDE",
            "RELATIVE_VOLUME_TOP_QUARTILE_REVERSION": lambda row: isinstance(row["totalVolumePercentilePrior"], float) and row["totalVolumePercentilePrior"] >= 0.76,
            "ABS_DELTA_TOP_QUARTILE_REVERSION": lambda row: isinstance(row["absoluteDeltaPercentilePrior"], float) and row["absoluteDeltaPercentilePrior"] >= 0.76,
            "COMPACT_GEOMETRY_REVERSION": lambda row: float(row["rangeToBaselineMedian"]) <= compact_threshold,
            "EXTENDED_GEOMETRY_REVERSION": lambda row: float(row["rangeToBaselineMedian"]) > compact_threshold,
        }
        candidate_results = {
            name: candidate_result(observations, train_dates, test_dates, predicate)
            for name, predicate in candidates.items()
        }

        minimum_test_profiles = 8
        sufficiently_sampled = [
            name for name, result in candidate_results.items()
            if result["horizon6"]["test"]["observations"] >= minimum_test_profiles
        ]
        report = {
            "report": "compression-shadow-fixed-horizon-exploratory",
            "generatedAtLocal": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "snapshotPrefix": str(prefix),
                "ledgerCounts": summary["counts"],
                "flowCoveredProfiles": len(profiles),
                "selectedFirstEvents": len(selected_events),
            },
            "contract": {
                "entry": "close of the first recorded boundary event per flow-covered profile",
                "direction": "reversion toward the frozen compression range",
                "exit": "close after exactly 6 or 12 bars",
                "maximumEntriesPerProfile": 1,
                "orders": False,
                "stop": None,
                "target": None,
                "pnl": None,
                "costs": None,
            },
            "split": {
                "method": "chronological distinct profile dates",
                "trainFraction": args.train_fraction,
                "trainDates": sorted(train_dates),
                "testDates": sorted(test_dates),
                "trainProfiles": sum(profile_date(label) in train_dates for label in selected_events),
                "testProfiles": sum(profile_date(label) in test_dates for label in selected_events),
                "selectionLeakage": True,
                "selectionLeakageReason": "Candidate ideas were inspected on an earlier overlapping snapshot; this is not a pristine holdout.",
            },
            "geometry": {
                "compactThresholdRangeToBaselineMedian": round(compact_threshold, 4),
                "thresholdDerivedFrom": "training profiles only",
            },
            "candidates": candidate_results,
            "assessment": {
                "minimumTestProfilesPerCandidate": minimum_test_profiles,
                "candidatesMeetingMinimum": sufficiently_sampled,
                "validatedCandidates": [],
                "conclusion": "Exploratory only. No candidate can be promoted because the test block is small, costs are absent, and the holdout has prior-selection leakage.",
            },
            "guardrail": "This report contains no executable entry, order, stop, target, or PnL contract.",
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
