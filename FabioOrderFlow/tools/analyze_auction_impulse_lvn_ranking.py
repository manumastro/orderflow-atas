#!/usr/bin/env python3
"""Analyze first-pullback LVN metrics without selecting thresholds or trades."""

from __future__ import annotations

import argparse
import csv
import glob
import json
import statistics
import sys
from collections import Counter, defaultdict
from datetime import datetime
from pathlib import Path

FORMALIZATION_SAMPLE_START = "2026-07-02"
CONTINUATION = "CONTINUATION_NEW_EXTREME"
REENTRY = "ORIGIN_REENTRY"
METRICS = (
    "BestRankScore",
    "BestProminence",
    "BestAdjacentDepth",
    "BestShoulderDepth",
    "MinimumVolumePercentile",
    "MinimumDirectionalProgress",
    "MaximumDirectionalProgress",
    "MinimumDistanceToPocRanges",
    "MinimumDistanceToOriginRanges",
    "TouchedLvnCount",
)


def load_csv(path: Path) -> list[dict[str, str]]:
    with path.open(encoding="utf-8", newline="") as handle:
        return list(csv.DictReader(handle))


def latest_for_timeframe(pattern: str, timeframe: str, excluded_suffix: str | None = None) -> Path:
    candidates: list[Path] = []
    for name in glob.glob(pattern):
        path = Path(name)
        if excluded_suffix and path.name.endswith(excluded_suffix):
            continue
        rows = load_csv(path)
        if rows and rows[0].get("ChartTimeFrame") == timeframe:
            candidates.append(path)
    if not candidates:
        raise FileNotFoundError(f"No {timeframe} dataset matches {pattern}")
    return sorted(candidates)[-1]


def percentile(values: list[float], fraction: float) -> float | None:
    if not values:
        return None
    ordered = sorted(values)
    position = (len(ordered) - 1) * fraction
    lower = int(position)
    upper = min(lower + 1, len(ordered) - 1)
    weight = position - lower
    return ordered[lower] * (1 - weight) + ordered[upper] * weight


def metric_summary(values: list[float]) -> dict[str, float | int | None]:
    return {
        "n": len(values),
        "q25": percentile(values, 0.25),
        "median": statistics.median(values) if values else None,
        "q75": percentile(values, 0.75),
    }


def auc(higher_group: list[float], lower_group: list[float]) -> float | None:
    if not higher_group or not lower_group:
        return None
    wins = 0.0
    for first in higher_group:
        for second in lower_group:
            wins += 1.0 if first > second else 0.5 if first == second else 0.0
    return wins / (len(higher_group) * len(lower_group))


def summarize(records: list[dict[str, object]]) -> dict[str, object]:
    by_outcome: dict[str, object] = {}
    for outcome in (CONTINUATION, REENTRY, "TWO_SIDED_RANGE", "SESSION_END"):
        selected = [record for record in records if record["endReason"] == outcome]
        if not selected:
            continue
        by_outcome[outcome] = {
            "observations": len(selected),
            "metrics": {
                metric: metric_summary([float(record[metric]) for record in selected])
                for metric in METRICS
            },
        }

    continuation = [record for record in records if record["endReason"] == CONTINUATION]
    reentry = [record for record in records if record["endReason"] == REENTRY]
    return {
        "observations": len(records),
        "dates": len({str(record["sessionDate"]) for record in records}),
        "directions": dict(sorted(Counter(str(record["direction"]) for record in records).items())),
        "endReasons": dict(sorted(Counter(str(record["endReason"]) for record in records).items())),
        "byOutcome": by_outcome,
        "continuationVsReentryAuc": {
            metric: auc(
                [float(record[metric]) for record in continuation],
                [float(record[metric]) for record in reentry],
            )
            for metric in METRICS
        },
    }


def primary_by_date_direction(records: list[dict[str, object]]) -> list[dict[str, object]]:
    selected: list[dict[str, object]] = []
    seen: set[tuple[str, str]] = set()
    for record in sorted(records, key=lambda item: int(item["firstPullbackBar"])):
        key = (str(record["sessionDate"]), str(record["direction"]))
        if key in seen:
            continue
        seen.add(key)
        selected.append(record)
    return selected


def segment(records: list[dict[str, object]]) -> dict[str, object]:
    return {
        "allImpulses": summarize(records),
        "primaryFirstPerDateDirection": summarize(primary_by_date_direction(records)),
    }


def build_observations(
    pullbacks: list[dict[str, str]],
    resolutions: list[dict[str, str]],
    lvns: list[dict[str, str]],
    touched: list[dict[str, str]],
) -> tuple[list[dict[str, object]], dict[str, int]]:
    resolution_by_impulse = {record["ImpulseId"]: record for record in resolutions}
    impulses_with_lvns = {record["ImpulseId"] for record in lvns}
    first_pullbacks = {
        record["ImpulseId"]: record
        for record in pullbacks
        if int(record["PullbackOrdinal"]) == 1
    }
    first_touches: dict[str, list[dict[str, str]]] = defaultdict(list)
    for record in touched:
        if int(record["PullbackOrdinal"]) == 1:
            first_touches[record["ImpulseId"]].append(record)

    exclusions = Counter()
    observations: list[dict[str, object]] = []
    for impulse_id, pullback in first_pullbacks.items():
        resolution = resolution_by_impulse.get(impulse_id)
        if resolution is None:
            exclusions["missingResolution"] += 1
            continue
        first_bar = int(pullback["Bar"])
        resolved_bar = int(resolution["ResolvedBar"])
        if first_bar >= resolved_bar:
            exclusions["resolvedOnFirstPullbackBar"] += 1
            continue
        touches = first_touches.get(impulse_id, [])
        if not touches:
            key = "noRawLvn" if impulse_id not in impulses_with_lvns else "noFirstPullbackLvnTouch"
            exclusions[key] += 1
            continue

        observations.append({
            "impulseId": impulse_id,
            "sessionDate": resolution["SessionDate"],
            "direction": resolution["Direction"],
            "chartTimeFrame": resolution["ChartTimeFrame"],
            "firstPullbackBar": first_bar,
            "resolvedBar": resolved_bar,
            "barsToResolution": resolved_bar - first_bar,
            "endReason": resolution["EndReason"],
            "BestRankScore": max(float(record["ProminenceRankScore"]) for record in touches),
            "BestProminence": max(float(record["Prominence"]) for record in touches),
            "BestAdjacentDepth": max(float(record["AdjacentDepth"]) for record in touches),
            "BestShoulderDepth": max(float(record["ShoulderDepth"]) for record in touches),
            "MinimumVolumePercentile": min(float(record["VolumePercentile"]) for record in touches),
            "MinimumDirectionalProgress": min(float(record["DirectionalProgress"]) for record in touches),
            "MaximumDirectionalProgress": max(float(record["DirectionalProgress"]) for record in touches),
            "MinimumDistanceToPocRanges": min(float(record["DistanceToPocRanges"]) for record in touches),
            "MinimumDistanceToOriginRanges": min(float(record["DistanceToOriginRanges"]) for record in touches),
            "TouchedLvnCount": len(touches),
            "operationalEntry": False,
            "orderSubmitted": False,
        })
    return observations, dict(sorted(exclusions.items()))


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--timeframe", choices=("M1", "M5"), required=True)
    parser.add_argument("--pullbacks", type=Path)
    parser.add_argument("--resolutions", type=Path)
    parser.add_argument("--lvns", type=Path)
    parser.add_argument("--touched-lvns", type=Path)
    parser.add_argument("--save", action="store_true")
    parser.add_argument("--output-dir", type=Path, default=Path("FabioOrderFlow") / "ledger-snapshots")
    args = parser.parse_args()

    try:
        pullback_path = args.pullbacks or latest_for_timeframe(
            "FabioOrderFlow/ledger-snapshots/auction-impulse-*-pullbacks.csv", args.timeframe
        )
        resolution_path = args.resolutions or latest_for_timeframe(
            "FabioOrderFlow/ledger-snapshots/auction-impulse-*-resolutions.csv", args.timeframe
        )
        lvn_path = args.lvns or latest_for_timeframe(
            "FabioOrderFlow/ledger-snapshots/auction-impulse-*-lvns.csv",
            args.timeframe,
            excluded_suffix="-touched-lvns.csv",
        )
        touched_path = args.touched_lvns or latest_for_timeframe(
            "FabioOrderFlow/ledger-snapshots/auction-impulse-*-touched-lvns.csv", args.timeframe
        )
        observations, exclusions = build_observations(
            load_csv(pullback_path), load_csv(resolution_path), load_csv(lvn_path), load_csv(touched_path)
        )
        historical = [record for record in observations if record["sessionDate"] < FORMALIZATION_SAMPLE_START]
        formalization = [record for record in observations if record["sessionDate"] >= FORMALIZATION_SAMPLE_START]
        report: dict[str, object] = {
            "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "pullbacks": str(pullback_path),
                "resolutions": str(resolution_path),
                "lvns": str(lvn_path),
                "touchedLvns": str(touched_path),
                "chartTimeFrame": args.timeframe,
            },
            "contract": {
                "model": "NY_IMPULSE_FIRST_PULLBACK_LVN_RANKING_V1",
                "firstPullbackOnly": True,
                "confirmationStrictlyBeforeResolution": True,
                "allRawLvnsRetained": True,
                "selectionThresholds": [],
                "aucMeaning": "Probability that a continuation observation has a higher metric than an origin-reentry observation; ties count 0.5.",
                "formalizationSampleStart": FORMALIZATION_SAMPLE_START,
                "selectionLeakage": True,
                "operationalEntry": False,
                "ordersSubmitted": False,
                "pnlComputed": False,
            },
            "dataset": {
                "observations": len(observations),
                "exclusions": exclusions,
            },
            "total": segment(observations),
            "byDirection": {
                direction: segment([record for record in observations if record["direction"] == direction])
                for direction in ("LONG", "SHORT")
            },
            "segments": {
                "historicalHoldout": {
                    "dateRule": f"SessionDate < {FORMALIZATION_SAMPLE_START}",
                    "isProspective": False,
                    **segment(historical),
                },
                "formalizationSample": {
                    "dateRule": f"SessionDate >= {FORMALIZATION_SAMPLE_START}",
                    "isProspective": False,
                    **segment(formalization),
                },
            },
            "observations": observations,
            "decision": {
                "validated": False,
                "promotedToShadow": False,
                "reason": "Metrics and analysis were formalized after inspecting this historical sample; prospective dates are required.",
            },
        }
        if args.save:
            args.output_dir.mkdir(parents=True, exist_ok=True)
            stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
            output = args.output_dir / f"auction-impulse-lvn-ranking-{args.timeframe}-{stamp}.json"
            output.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
            report["saved"] = str(output)
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
