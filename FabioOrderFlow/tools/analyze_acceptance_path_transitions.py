#!/usr/bin/env python3
"""Classify acceptance shadow paths and their early directional flow.

Stdout is JSON only. The report is diagnostic and never creates orders,
stops, targets, execution assumptions, or PnL.
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
    latest_snapshot_prefix,
    optional_float,
    profile_date,
    read_csv,
)

EARLY_REJECTION_MINUTES = 15.0
FLOW_PREFIX_BARS = (1, 2, 3)
HORIZONS = (6, 12)


def integer(value: str) -> int:
    return int(value)


def mean(values: list[float]) -> float | None:
    return round(statistics.fmean(values), 4) if values else None


def median(values: list[float]) -> float | None:
    return round(statistics.median(values), 4) if values else None


def rate(values: list[bool]) -> float | None:
    return round(sum(values) / len(values), 4) if values else None


def shadow_paths(prefix: Path) -> dict[str, Path]:
    return {
        "summary": prefix.with_name(prefix.name + "-summary.json"),
        "events": prefix.with_name(prefix.name + "-events.csv"),
        "entries": prefix.with_name(prefix.name + "-shadow-entries.csv"),
        "outcomes": prefix.with_name(prefix.name + "-shadow-outcomes.csv"),
        "pathBars": prefix.with_name(prefix.name + "-shadow-path-bars.csv"),
    }


def expected_outside_state(boundary: str) -> str:
    return "ABOVE_RANGE" if boundary == "HIGH" else "BELOW_RANGE"


def opposite_outside_state(boundary: str) -> str:
    return "BELOW_RANGE" if boundary == "HIGH" else "ABOVE_RANGE"


def directional_value(boundary: str, value: float) -> float:
    return value if boundary == "HIGH" else -value


def prefix_flow(path: list[dict[str, str]], boundary: str, bars: int) -> dict[str, object]:
    selected = path[:bars]
    total_volume = sum(float(row["TotalVolume"]) for row in selected)
    directional_delta = sum(directional_value(boundary, float(row["Delta"])) for row in selected)
    return {
        "bars": len(selected),
        "directionalDelta": round(directional_delta, 4),
        "totalVolume": round(total_volume, 4),
        "directionalFlowImbalance": round(directional_delta / total_volume, 4) if total_volume else None,
    }


def classify_path(
    entry: dict[str, str],
    path: list[dict[str, str]],
    outcomes: dict[int, dict[str, str]],
    event: dict[str, str],
) -> dict[str, object]:
    boundary = entry["Boundary"]
    expected = expected_outside_state(boundary)
    opposite = opposite_outside_state(boundary)
    inside_index = next((index for index, row in enumerate(path) if row["PriceState"] == "INSIDE_RANGE"), None)
    first_inside = path[inside_index] if inside_index is not None else None
    first_inside_minutes = float(first_inside["ElapsedMinutes"]) if first_inside else None
    if first_inside_minutes is None:
        initial_transition = "CONTINUOUS_ACCEPTANCE"
    elif first_inside_minutes <= EARLY_REJECTION_MINUTES:
        initial_transition = "EARLY_REJECTION"
    else:
        initial_transition = "DELAYED_REJECTION"

    after_inside = path[inside_index + 1:] if inside_index is not None else []
    reaccepted = any(row["PriceState"] == expected for row in after_inside)
    poc_row = next((row for row in path if row["PocTouchedThisBar"] == "True"), None)
    opposite_row = next((row for row in path if row["PriceState"] == opposite), None)
    terminal = path[-1]
    tags = [initial_transition]
    if poc_row:
        tags.append("REJECTION_TO_POC")
    if reaccepted:
        tags.append("REACCEPTANCE_AFTER_REENTRY")
    if opposite_row:
        tags.append("OPPOSITE_BREAKOUT")

    horizon_results: dict[str, object] = {}
    for horizon in HORIZONS:
        outcome = outcomes[horizon]
        horizon_results[f"h{horizon}"] = {
            "elapsedMinutes": float(outcome["ElapsedMinutes"]),
            "directionalMoveRanges": float(outcome["DirectionalMoveRanges"]),
            "favorableMfeRanges": float(outcome["FavorableMfeRanges"]),
            "adverseMfeRanges": float(outcome["AdverseMfeRanges"]),
            "endInsideRange": outcome["EndInsideRange"] == "True",
            "pocTouched": outcome["PocTouched"] == "True",
        }

    return {
        "shadowId": entry["ShadowId"],
        "profileLabel": entry["ProfileLabel"],
        "date": profile_date(entry["ProfileLabel"]),
        "entryBar": integer(entry["EntryBar"]),
        "boundary": boundary,
        "direction": entry["Direction"],
        "chartTimeFrame": entry["ChartTimeFrame"],
        "entryPrice": float(entry["EntryPrice"]),
        "initialTransition": initial_transition,
        "trajectoryTags": tags,
        "firstInsideMinutes": first_inside_minutes,
        "firstInsidePathBarOrdinal": integer(first_inside["PathBarOrdinal"]) if first_inside else None,
        "pocTouched": poc_row is not None,
        "firstPocTouchMinutes": float(poc_row["ElapsedMinutes"]) if poc_row else None,
        "reacceptedAfterReentry": reaccepted,
        "oppositeBreakout": opposite_row is not None,
        "terminalState": terminal["PriceState"],
        "terminalElapsedMinutes": float(terminal["ElapsedMinutes"]),
        "pathBars": len(path),
        "entryDirectionalDelta": directional_value(boundary, float(event["Delta"])),
        "entryTotalVolumePercentilePrior": optional_float(event["TotalVolumePercentilePrior"]),
        "entryAbsoluteDeltaPercentilePrior": optional_float(event["AbsoluteDeltaPercentilePrior"]),
        "flowPrefixes": {
            f"first{bars}Bars": prefix_flow(path, boundary, bars)
            for bars in FLOW_PREFIX_BARS
        },
        "horizons": horizon_results,
        "operationalEntry": False,
        "orderSubmitted": False,
    }


def summarize(rows: list[dict[str, object]]) -> dict[str, object]:
    if not rows:
        return {
            "profiles": 0,
            "dates": 0,
            "h6": {},
            "h12": {},
        }

    result: dict[str, object] = {
        "profiles": len(rows),
        "dates": len({str(row["date"]) for row in rows}),
        "firstInsideRate": rate([row["firstInsideMinutes"] is not None for row in rows]),
        "medianFirstInsideMinutes": median([
            float(row["firstInsideMinutes"])
            for row in rows if row["firstInsideMinutes"] is not None
        ]),
        "pocTouchRate": rate([bool(row["pocTouched"]) for row in rows]),
        "medianFirstPocTouchMinutes": median([
            float(row["firstPocTouchMinutes"])
            for row in rows if row["firstPocTouchMinutes"] is not None
        ]),
        "reacceptedAfterReentryRate": rate([bool(row["reacceptedAfterReentry"]) for row in rows]),
        "oppositeBreakoutRate": rate([bool(row["oppositeBreakout"]) for row in rows]),
    }
    for bars in FLOW_PREFIX_BARS:
        values = [
            row["flowPrefixes"][f"first{bars}Bars"]["directionalFlowImbalance"]
            for row in rows
        ]
        result[f"first{bars}BarsMedianDirectionalFlowImbalance"] = median([
            float(value) for value in values if value is not None
        ])
    for horizon in HORIZONS:
        horizon_rows = [row["horizons"][f"h{horizon}"] for row in rows]
        moves = [float(item["directionalMoveRanges"]) for item in horizon_rows]
        result[f"h{horizon}"] = {
            "averageDirectionalMoveRanges": mean(moves),
            "medianDirectionalMoveRanges": median(moves),
            "positiveDirectionalMoveRate": rate([move > 0 for move in moves]),
            "averageFavorableMfeRanges": mean([float(item["favorableMfeRanges"]) for item in horizon_rows]),
            "averageAdverseMfeRanges": mean([float(item["adverseMfeRanges"]) for item in horizon_rows]),
            "endInsideRate": rate([bool(item["endInsideRange"]) for item in horizon_rows]),
            "pocTouchedRate": rate([bool(item["pocTouched"]) for item in horizon_rows]),
        }
    return result


def grouped_summary(rows: list[dict[str, object]]) -> dict[str, object]:
    transitions = ("CONTINUOUS_ACCEPTANCE", "EARLY_REJECTION", "DELAYED_REJECTION")

    def flow_regime(row: dict[str, object]) -> str:
        imbalance = row["flowPrefixes"]["first3Bars"]["directionalFlowImbalance"]
        return "DIRECTIONAL_FLOW_POSITIVE" if imbalance is not None and float(imbalance) > 0 else "DIRECTIONAL_FLOW_NON_POSITIVE"

    return {
        "all": summarize(rows),
        "byBoundary": {
            boundary: summarize([row for row in rows if row["boundary"] == boundary])
            for boundary in ("HIGH", "LOW")
        },
        "byInitialTransition": {
            transition: summarize([row for row in rows if row["initialTransition"] == transition])
            for transition in transitions
        },
        "byBoundaryAndInitialTransition": {
            boundary: {
                transition: summarize([
                    row for row in rows
                    if row["boundary"] == boundary and row["initialTransition"] == transition
                ])
                for transition in transitions
            }
            for boundary in ("HIGH", "LOW")
        },
        "byFirst3BarsFlowRegime": {
            regime: summarize([row for row in rows if flow_regime(row) == regime])
            for regime in ("DIRECTIONAL_FLOW_POSITIVE", "DIRECTIONAL_FLOW_NON_POSITIVE")
        },
        "byBoundaryAndFirst3BarsFlowRegime": {
            boundary: {
                regime: summarize([
                    row for row in rows
                    if row["boundary"] == boundary and flow_regime(row) == regime
                ])
                for regime in ("DIRECTIONAL_FLOW_POSITIVE", "DIRECTIONAL_FLOW_NON_POSITIVE")
            }
            for boundary in ("HIGH", "LOW")
        },
        "overlappingTrajectoryTags": {
            tag: summarize([row for row in rows if tag in row["trajectoryTags"]])
            for tag in ("REJECTION_TO_POC", "REACCEPTANCE_AFTER_REENTRY", "OPPOSITE_BREAKOUT")
        },
    }


def main() -> int:
    parser = argparse.ArgumentParser(description="Analyze acceptance path transitions and early directional flow")
    parser.add_argument("--snapshot-prefix", type=Path)
    parser.add_argument("--snapshot-dir", type=Path, default=DEFAULT_SNAPSHOT_DIR)
    parser.add_argument("--train-fraction", type=float, default=0.70)
    parser.add_argument("--save", type=Path, help="Optional JSON path; stdout remains JSON only")
    args = parser.parse_args()

    try:
        if not 0.5 <= args.train_fraction <= 0.8:
            raise ValueError("train-fraction must be between 0.5 and 0.8")
        prefix = args.snapshot_prefix or latest_snapshot_prefix(args.snapshot_dir)
        paths = shadow_paths(prefix)
        missing = [str(path) for path in paths.values() if not path.exists()]
        if missing:
            raise ValueError(f"Missing snapshot files: {missing}")

        ledger_summary = json.loads(paths["summary"].read_text(encoding="utf-8"))
        entries = [row for row in read_csv(paths["entries"]) if row["TradeCoverage"] == "AVAILABLE"]
        events = {
            (row["ProfileLabel"], integer(row["Bar"]), row["Boundary"]): row
            for row in read_csv(paths["events"])
            if row["TradeCoverage"] == "AVAILABLE"
        }
        outcomes_by_shadow: dict[str, dict[int, dict[str, str]]] = defaultdict(dict)
        for row in read_csv(paths["outcomes"]):
            if row["TradeCoverage"] == "AVAILABLE":
                outcomes_by_shadow[row["ShadowId"]][integer(row["HorizonBars"])] = row
        path_by_shadow: dict[str, list[dict[str, str]]] = defaultdict(list)
        for row in read_csv(paths["pathBars"]):
            if row["TradeCoverage"] == "AVAILABLE":
                path_by_shadow[row["ShadowId"]].append(row)
        for path in path_by_shadow.values():
            path.sort(key=lambda row: integer(row["PathBarOrdinal"]))

        observations: list[dict[str, object]] = []
        for entry in entries:
            shadow_id = entry["ShadowId"]
            path = path_by_shadow.get(shadow_id, [])
            outcomes = outcomes_by_shadow.get(shadow_id, {})
            event = events.get((entry["ProfileLabel"], integer(entry["EntryBar"]), entry["Boundary"]))
            if not path or event is None or any(horizon not in outcomes for horizon in HORIZONS):
                raise ValueError(f"Incomplete acceptance path contract for {shadow_id}")
            observations.append(classify_path(entry, path, outcomes, event))

        dates = sorted({str(row["date"]) for row in observations})
        split_index = max(1, min(len(dates) - 1, math.floor(len(dates) * args.train_fraction)))
        train_dates = set(dates[:split_index])
        test_dates = set(dates[split_index:])
        train_rows = [row for row in observations if row["date"] in train_dates]
        test_rows = [row for row in observations if row["date"] in test_dates]

        report = {
            "report": "acceptance-path-transitions-exploratory",
            "generatedAtLocal": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "snapshotPrefix": str(prefix),
                "ledgerCounts": ledger_summary["counts"],
                "flowCoveredAcceptanceProfiles": len(observations),
                "chartTimeFrames": sorted({str(row["chartTimeFrame"]) for row in observations}),
            },
            "contract": {
                "maximumObservationsPerProfile": 1,
                "initialTransition": {
                    "CONTINUOUS_ACCEPTANCE": "no completed path bar closes inside the frozen range",
                    "EARLY_REJECTION": f"first completed close inside within {EARLY_REJECTION_MINUTES:g} elapsed minutes",
                    "DELAYED_REJECTION": f"first completed close inside after {EARLY_REJECTION_MINUTES:g} elapsed minutes",
                },
                "overlappingTags": {
                    "REJECTION_TO_POC": "POC touched during the observed path",
                    "REACCEPTANCE_AFTER_REENTRY": "expected outside close observed after the first inside close",
                    "OPPOSITE_BREAKOUT": "opposite outside close observed during the path",
                },
                "directionalFlowImbalance": "signed cumulative delta in the shadow direction divided by total volume over the first N path bars",
                "first3BarsFlowRegime": "causal shadow confirmation available after three completed path bars; positive versus non-positive, with no fitted threshold",
                "observationEnd": "first completed chart bar reaching 60 elapsed minutes, with at most 5 minutes completion tolerance",
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
                "selectionLeakage": True,
                "selectionLeakageReason": "Transition and early-flow hypotheses were formulated after inspecting this overlapping sample.",
            },
            "summaries": {
                "all": grouped_summary(observations),
                "train": grouped_summary(train_rows),
                "test": grouped_summary(test_rows),
            },
            "observations": observations,
            "assessment": {
                "validatedTransitions": [],
                "validatedFlowConfirmations": [],
                "minimumIndependentTestProfiles": 8,
                "conclusion": "Descriptive path classification only; all transition and flow hypotheses require unseen prospective sessions.",
            },
            "guardrail": "No executable ATAS entry, order, stop, target, PnL, or intrabar execution assumption is produced.",
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
