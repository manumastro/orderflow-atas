#!/usr/bin/env python3
"""Analyze transcript-derived cumulative aggression before impulse resolution."""

from __future__ import annotations

import argparse
import csv
import glob
import json
import sys
from collections import Counter, defaultdict
from datetime import datetime
from pathlib import Path

TRANSCRIPT_NEW_YORK_BIG_TRADE = 30.0
FORMALIZATION_SAMPLE_START = "2026-07-02"


def load_csv(path: Path) -> list[dict[str, str]]:
    with path.open(encoding="utf-8", newline="") as handle:
        return list(csv.DictReader(handle))


def latest_dataset(timeframe: str) -> tuple[Path, Path, Path]:
    candidates: list[tuple[Path, Path, Path]] = []
    for profile_name in glob.glob("FabioOrderFlow/ledger-snapshots/auction-impulse-*-profiles.csv"):
        profile_path = Path(profile_name)
        prefix = profile_path.name.removesuffix("-profiles.csv")
        pullback_path = profile_path.with_name(f"{prefix}-pullbacks.csv")
        resolution_path = profile_path.with_name(f"{prefix}-resolutions.csv")
        if not pullback_path.exists() or not resolution_path.exists():
            continue
        profiles = load_csv(profile_path)
        if profiles and profiles[0].get("ChartTimeFrame") == timeframe:
            candidates.append((profile_path, pullback_path, resolution_path))
    if not candidates:
        raise FileNotFoundError(f"No complete auction impulse dataset found for {timeframe}")
    return sorted(candidates)[-1]


def directional_values(record: dict[str, str], direction: str) -> tuple[float, float]:
    buy = float(record["MaxCumulativeBuy"])
    sell = float(record["MaxCumulativeSell"])
    return (buy, sell) if direction == "LONG" else (sell, buy)


def find_confirmations(
    profiles: list[dict[str, str]],
    pullbacks: list[dict[str, str]],
    resolutions: list[dict[str, str]],
) -> list[dict[str, object]]:
    by_impulse: dict[str, list[dict[str, str]]] = defaultdict(list)
    for record in pullbacks:
        by_impulse[record["ImpulseId"]].append(record)
    resolution_by_impulse = {record["ImpulseId"]: record for record in resolutions}
    confirmations: list[dict[str, object]] = []

    for profile in profiles:
        impulse_id = profile["ImpulseId"]
        resolution = resolution_by_impulse.get(impulse_id)
        if resolution is None:
            continue
        direction = profile["Direction"]
        expected_result = "BUY_WITH_RESULT" if direction == "LONG" else "SELL_WITH_RESULT"
        resolved_bar = int(resolution["ResolvedBar"])
        ordered = sorted(by_impulse[impulse_id], key=lambda record: int(record["PullbackOrdinal"]))
        for record in ordered:
            directional_max, opposite_max = directional_values(record, direction)
            causal_confirmation = (
                int(record["Bar"]) < resolved_bar
                and record["CumulativeTradeCoverage"] == "AVAILABLE"
                and record["TouchedLvns"] not in {"", "NONE"}
                and record["EffortResult"] == expected_result
                and directional_max >= TRANSCRIPT_NEW_YORK_BIG_TRADE
                and directional_max > opposite_max
            )
            if not causal_confirmation:
                continue
            confirmations.append({
                "impulseId": impulse_id,
                "sessionDate": profile["SessionDate"],
                "direction": direction,
                "chartTimeFrame": profile["ChartTimeFrame"],
                "startBar": int(profile["StartBar"]),
                "impulseBars": int(profile["ImpulseBars"]),
                "confirmationBar": int(record["Bar"]),
                "pullbackOrdinal": int(record["PullbackOrdinal"]),
                "frozenProfileRelation": record["FrozenProfileRelation"],
                "touchedLvns": record["TouchedLvns"],
                "directionalCumulativeMax": directional_max,
                "oppositeCumulativeMax": opposite_max,
                "endReason": resolution["EndReason"],
                "resolvedBar": resolved_bar,
                "barsFromConfirmationToResolution": resolved_bar - int(record["Bar"]),
                "operationalEntry": False,
                "orderSubmitted": False,
            })
            break
    return confirmations


def primary_by_date_direction(confirmations: list[dict[str, object]]) -> list[dict[str, object]]:
    selected: list[dict[str, object]] = []
    seen: set[tuple[str, str]] = set()
    for record in sorted(confirmations, key=lambda item: int(item["startBar"])):
        key = (str(record["sessionDate"]), str(record["direction"]))
        if key in seen:
            continue
        seen.add(key)
        selected.append(record)
    return selected


def profile_summary(
    profiles: list[dict[str, str]],
    resolutions: list[dict[str, str]],
) -> dict[str, object]:
    resolution_by_impulse = {record["ImpulseId"]: record for record in resolutions}
    joined = [
        (profile, resolution_by_impulse[profile["ImpulseId"]])
        for profile in profiles
        if profile["ImpulseId"] in resolution_by_impulse
    ]
    continuation_count = sum(
        resolution["EndReason"] == "CONTINUATION_NEW_EXTREME"
        for _, resolution in joined
    )
    return {
        "profiles": len(joined),
        "dates": len({profile["SessionDate"] for profile, _ in joined}),
        "directions": dict(sorted(Counter(profile["Direction"] for profile, _ in joined).items())),
        "endReasons": dict(sorted(Counter(resolution["EndReason"] for _, resolution in joined).items())),
        "cleanContinuationRate": continuation_count / len(joined) if joined else None,
    }


def group_summary(records: list[dict[str, object]]) -> dict[str, object]:
    def rate(reason: str, selected: list[dict[str, object]]) -> float | None:
        return sum(record["endReason"] == reason for record in selected) / len(selected) if selected else None

    summary: dict[str, object] = {
        "observations": len(records),
        "dates": len({str(record["sessionDate"]) for record in records}),
        "directions": dict(sorted(Counter(str(record["direction"]) for record in records).items())),
        "endReasons": dict(sorted(Counter(str(record["endReason"]) for record in records).items())),
        "cleanContinuationRate": rate("CONTINUATION_NEW_EXTREME", records),
        "originReentryRate": rate("ORIGIN_REENTRY", records),
        "byDirection": {},
    }
    for direction in ("LONG", "SHORT"):
        selected = [record for record in records if record["direction"] == direction]
        summary["byDirection"][direction] = {
            "observations": len(selected),
            "cleanContinuationRate": rate("CONTINUATION_NEW_EXTREME", selected),
            "originReentryRate": rate("ORIGIN_REENTRY", selected),
        }
    return summary


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--timeframe", choices=("M1", "M5"), required=True)
    parser.add_argument("--profiles", type=Path)
    parser.add_argument("--pullbacks", type=Path)
    parser.add_argument("--resolutions", type=Path)
    parser.add_argument("--save", action="store_true")
    parser.add_argument("--output-dir", type=Path, default=Path("FabioOrderFlow") / "ledger-snapshots")
    args = parser.parse_args()

    try:
        supplied = (args.profiles, args.pullbacks, args.resolutions)
        if any(supplied) and not all(supplied):
            raise ValueError("Specify all three dataset paths or none")
        profile_path, pullback_path, resolution_path = supplied if all(supplied) else latest_dataset(args.timeframe)
        profiles = load_csv(profile_path)
        pullbacks = load_csv(pullback_path)
        resolutions = load_csv(resolution_path)
        if profiles and profiles[0].get("ChartTimeFrame") != args.timeframe:
            raise ValueError("Dataset timeframe does not match --timeframe")

        confirmations = find_confirmations(profiles, pullbacks, resolutions)
        primary = primary_by_date_direction(confirmations)
        historical_profiles = [
            record for record in profiles
            if record["SessionDate"] < FORMALIZATION_SAMPLE_START
        ]
        formalization_profiles = [
            record for record in profiles
            if record["SessionDate"] >= FORMALIZATION_SAMPLE_START
        ]
        historical_confirmations = [
            record for record in confirmations
            if str(record["sessionDate"]) < FORMALIZATION_SAMPLE_START
        ]
        formalization_confirmations = [
            record for record in confirmations
            if str(record["sessionDate"]) >= FORMALIZATION_SAMPLE_START
        ]
        report: dict[str, object] = {
            "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "profiles": str(profile_path),
                "pullbacks": str(pullback_path),
                "resolutions": str(resolution_path),
                "chartTimeFrame": args.timeframe,
            },
            "contract": {
                "model": "NY_IMPULSE_LVN_CUMULATIVE_CONFIRMATION_V1",
                "confirmationBeforeResolution": True,
                "requiresTouchedRawLvn": True,
                "requiresDirectionalEffortWithResult": True,
                "requiresDirectionalCumulativeDominance": True,
                "minimumDirectionalCumulativeTrade": TRANSCRIPT_NEW_YORK_BIG_TRADE,
                "thresholdSource": "Fabio transcript, not optimized on this dataset",
                "primaryIndependence": "first confirmation per session date and direction",
                "formalizationSampleStart": FORMALIZATION_SAMPLE_START,
                "selectionLeakage": True,
                "operationalEntry": False,
                "ordersSubmitted": False,
                "pnlComputed": False,
            },
            "dataset": {
                "profiles": len(profiles),
                "pullbackBars": len(pullbacks),
                "resolutions": len(resolutions),
            },
            "allConfirmations": group_summary(confirmations),
            "primaryConfirmations": group_summary(primary),
            "segments": {
                "historicalHoldout": {
                    "dateRule": f"SessionDate < {FORMALIZATION_SAMPLE_START}",
                    "isProspective": False,
                    "profileBaseline": profile_summary(historical_profiles, resolutions),
                    "allConfirmations": group_summary(historical_confirmations),
                    "primaryConfirmations": group_summary(
                        primary_by_date_direction(historical_confirmations)
                    ),
                },
                "formalizationSample": {
                    "dateRule": f"SessionDate >= {FORMALIZATION_SAMPLE_START}",
                    "isProspective": False,
                    "profileBaseline": profile_summary(formalization_profiles, resolutions),
                    "allConfirmations": group_summary(formalization_confirmations),
                    "primaryConfirmations": group_summary(
                        primary_by_date_direction(formalization_confirmations)
                    ),
                },
            },
            "confirmations": confirmations,
            "primaryObservationIds": [record["impulseId"] for record in primary],
            "decision": {
                "validated": False,
                "promotedToShadow": False,
                "reason": "The older historical holdout weakens the formalization result and is not prospective; independent future dates are required.",
            },
        }
        if args.save:
            args.output_dir.mkdir(parents=True, exist_ok=True)
            stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
            path = args.output_dir / f"auction-impulse-confirmations-{args.timeframe}-{stamp}.json"
            path.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
            report["saved"] = str(path)
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
