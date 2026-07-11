#!/usr/bin/env python3
"""Build transcript-derived, non-operational auction playbook observations."""

from __future__ import annotations

import argparse
import csv
import glob
import json
import math
import sys
from collections import Counter, defaultdict
from datetime import datetime
from pathlib import Path
from statistics import median

HORIZONS = (3, 6, 12)
NUMERIC_FIELDS = {
    "Open", "High", "Low", "Close", "PriorPOC", "PriorVAH", "PriorVAL",
    "Prior12Range", "PriorRollingLvnBelow", "PriorRollingLvnAbove",
    "MaxCumulativeBuy", "MaxCumulativeSell", "CumulativeDelta",
}


def latest(pattern: str) -> Path:
    paths = sorted(Path(path) for path in glob.glob(pattern))
    if not paths:
        raise FileNotFoundError(f"No input matches {pattern}")
    return paths[-1]


def optional_float(value: str) -> float | None:
    if value in {"", "NA", None}:
        return None
    return float(value)


def load_bars(path: Path) -> list[dict[str, object]]:
    with path.open(encoding="utf-8", newline="") as handle:
        records = list(csv.DictReader(handle))
    for record in records:
        for field in NUMERIC_FIELDS:
            record[field] = optional_float(str(record.get(field, "")))
        for field in ("Bar", "SessionBarOrdinal", "PriorProfileBars"):
            record[field] = int(str(record[field]))
    return records


def grouped_bars(records: list[dict[str, object]]) -> dict[tuple[str, str], list[dict[str, object]]]:
    grouped: dict[tuple[str, str], list[dict[str, object]]] = defaultdict(list)
    for record in records:
        grouped[(str(record["Session"]), str(record["SessionDate"]))].append(record)
    for sequence in grouped.values():
        sequence.sort(key=lambda item: int(item["Bar"]))
    return grouped


def directional_max(record: dict[str, object], direction: str) -> tuple[float, float]:
    buy = float(record["MaxCumulativeBuy"] or 0.0)
    sell = float(record["MaxCumulativeSell"] or 0.0)
    return (buy, sell) if direction == "LONG" else (sell, buy)


def outcome(
    sequence: list[dict[str, object]],
    entry_index: int,
    direction: str,
    scale: float,
    horizon: int,
    target: float | None,
) -> dict[str, object] | None:
    end_index = entry_index + horizon
    if end_index >= len(sequence):
        return None
    factor = 1.0 if direction == "LONG" else -1.0
    entry = float(sequence[entry_index]["Close"])
    future = sequence[entry_index + 1:end_index + 1]
    move = factor * (float(sequence[end_index]["Close"]) - entry) / scale
    favorable = max(
        [0.0] + [
            (float(bar["High"]) - entry) / scale
            if direction == "LONG"
            else (entry - float(bar["Low"])) / scale
            for bar in future
        ]
    )
    adverse = max(
        [0.0] + [
            (entry - float(bar["Low"])) / scale
            if direction == "LONG"
            else (float(bar["High"]) - entry) / scale
            for bar in future
        ]
    )
    return {
        "horizonBars": horizon,
        "endBar": sequence[end_index]["Bar"],
        "directionalMoveRanges": move,
        "favorableMfeRanges": favorable,
        "adverseMfeRanges": adverse,
        "targetTouched": bool(target is not None and any(
            float(bar["Low"]) <= target <= float(bar["High"]) for bar in future
        )),
    }


def build_balance_rotations(
    grouped: dict[tuple[str, str], list[dict[str, object]]],
) -> list[dict[str, object]]:
    observations: list[dict[str, object]] = []
    for (session, session_date), sequence in sorted(grouped.items()):
        transcript_big_trade = 20.0 if session == "LONDON" else 30.0
        for direction in ("LONG", "SHORT"):
            selected: dict[str, object] | None = None
            for event_index, event in enumerate(sequence):
                if int(event["PriorProfileBars"]) < 6:
                    continue
                scale = event["Prior12Range"]
                vah = event["PriorVAH"]
                val = event["PriorVAL"]
                if not isinstance(scale, float) or scale <= 0 or not isinstance(vah, float) or not isinstance(val, float):
                    continue

                absorbed = (
                    direction == "SHORT"
                    and event["PriorProfileRelation"] == "ABOVE_VAH"
                    and event["EffortResult"] == "BUY_ABSORBED"
                ) or (
                    direction == "LONG"
                    and event["PriorProfileRelation"] == "BELOW_VAL"
                    and event["EffortResult"] == "SELL_ABSORBED"
                )
                if not absorbed:
                    continue

                for entry_index in range(event_index, len(sequence)):
                    confirmation = sequence[entry_index]
                    close = float(confirmation["Close"])
                    if direction == "SHORT" and close < val:
                        break
                    if direction == "LONG" and close > vah:
                        break
                    if not val <= close <= vah:
                        continue

                    expected_result = "SELL_WITH_RESULT" if direction == "SHORT" else "BUY_WITH_RESULT"
                    directional_volume, opposite_volume = directional_max(confirmation, direction)
                    confirmed = (
                        confirmation["EffortResult"] == expected_result
                        and confirmation["CumulativeTradeCoverage"] == "AVAILABLE"
                        and directional_volume >= transcript_big_trade
                        and directional_volume > opposite_volume
                    )
                    if not confirmed:
                        continue

                    target = event["PriorPOC"] if isinstance(event["PriorPOC"], float) else None
                    selected = {
                        "model": "BALANCE_ROTATION_V1",
                        "session": session,
                        "sessionDate": session_date,
                        "direction": direction,
                        "eventBar": event["Bar"],
                        "entryBar": confirmation["Bar"],
                        "latencyBars": entry_index - event_index,
                        "entryPrice": confirmation["Close"],
                        "frozenPOC": target,
                        "frozenVAH": vah,
                        "frozenVAL": val,
                        "normalizationRange": scale,
                        "directionalBigTrade": directional_volume,
                        "oppositeBigTrade": opposite_volume,
                        "transcriptBigTradeMinimum": transcript_big_trade,
                        "outcomes": [
                            result for horizon in HORIZONS
                            if (result := outcome(sequence, entry_index, direction, scale, horizon, target))
                        ],
                        "operationalEntry": False,
                        "orderSubmitted": False,
                    }
                    break
                if selected:
                    break
            if selected:
                observations.append(selected)
    return observations


def build_ny_continuations(
    grouped: dict[tuple[str, str], list[dict[str, object]]],
) -> list[dict[str, object]]:
    observations: list[dict[str, object]] = []
    for (session, session_date), sequence in sorted(grouped.items()):
        if session != "NEW_YORK":
            continue
        for direction in ("LONG", "SHORT"):
            selected: dict[str, object] | None = None
            for event_index, event in enumerate(sequence):
                if event_index == 0 or int(event["PriorProfileBars"]) < 6:
                    continue
                scale = event["Prior12Range"]
                vah = event["PriorVAH"]
                val = event["PriorVAL"]
                if not isinstance(scale, float) or scale <= 0 or not isinstance(vah, float) or not isinstance(val, float):
                    continue
                previous_close = float(sequence[event_index - 1]["Close"])
                if not val <= previous_close <= vah:
                    continue

                expansion = (
                    direction == "LONG"
                    and float(event["Close"]) > vah
                    and event["EffortResult"] == "BUY_WITH_RESULT"
                ) or (
                    direction == "SHORT"
                    and float(event["Close"]) < val
                    and event["EffortResult"] == "SELL_WITH_RESULT"
                )
                if not expansion:
                    continue

                boundary = vah if direction == "LONG" else val
                opposite_boundary = val if direction == "LONG" else vah
                lvn = event["PriorRollingLvnBelow"] if direction == "LONG" else event["PriorRollingLvnAbove"]
                pullback_index: int | None = None
                pullback_location = ""
                for index in range(event_index + 1, len(sequence)):
                    bar = sequence[index]
                    close = float(bar["Close"])
                    if (direction == "LONG" and close < opposite_boundary) or (direction == "SHORT" and close > opposite_boundary):
                        break
                    boundary_touched = float(bar["Low"]) <= boundary if direction == "LONG" else float(bar["High"]) >= boundary
                    lvn_touched = isinstance(lvn, float) and float(bar["Low"]) <= lvn <= float(bar["High"])
                    if boundary_touched or lvn_touched:
                        pullback_index = index
                        pullback_location = "BOTH" if boundary_touched and lvn_touched else "LVN" if lvn_touched else "VALUE_BOUNDARY"
                        break
                if pullback_index is None:
                    continue

                pullback = sequence[pullback_index]
                for entry_index in range(pullback_index + 1, len(sequence)):
                    confirmation = sequence[entry_index]
                    close = float(confirmation["Close"])
                    if (direction == "LONG" and close < opposite_boundary) or (direction == "SHORT" and close > opposite_boundary):
                        break
                    directional_volume, opposite_volume = directional_max(confirmation, direction)
                    confirmed = (
                        direction == "LONG"
                        and confirmation["EffortResult"] == "BUY_WITH_RESULT"
                        and close > float(pullback["High"])
                    ) or (
                        direction == "SHORT"
                        and confirmation["EffortResult"] == "SELL_WITH_RESULT"
                        and close < float(pullback["Low"])
                    )
                    if not (
                        confirmed
                        and confirmation["CumulativeTradeCoverage"] == "AVAILABLE"
                        and directional_volume > opposite_volume
                    ):
                        continue

                    selected = {
                        "model": "NY_IMBALANCE_PULLBACK_V1",
                        "session": session,
                        "sessionDate": session_date,
                        "direction": direction,
                        "expansionBar": event["Bar"],
                        "pullbackBar": pullback["Bar"],
                        "entryBar": confirmation["Bar"],
                        "expansionToPullbackBars": pullback_index - event_index,
                        "pullbackToEntryBars": entry_index - pullback_index,
                        "totalLatencyBars": entry_index - event_index,
                        "pullbackLocation": pullback_location,
                        "entryPrice": confirmation["Close"],
                        "frozenVAH": vah,
                        "frozenVAL": val,
                        "rollingLvn": lvn,
                        "normalizationRange": scale,
                        "directionalBigTrade": directional_volume,
                        "oppositeBigTrade": opposite_volume,
                        "outcomes": [
                            result for horizon in HORIZONS
                            if (result := outcome(sequence, entry_index, direction, scale, horizon, None))
                        ],
                        "operationalEntry": False,
                        "orderSubmitted": False,
                    }
                    break
                if selected:
                    break
            if selected:
                observations.append(selected)
    return observations


def summarize(observations: list[dict[str, object]], dates: list[str]) -> dict[str, object]:
    cutoff_index = max(0, math.ceil(len(dates) * 2 / 3) - 1)
    cutoff = dates[cutoff_index]
    result: dict[str, object] = {"dateCutoff": cutoff, "groups": {}}
    for split, predicate in (
        ("all", lambda date: True),
        ("train", lambda date: date <= cutoff),
        ("test", lambda date: date > cutoff),
    ):
        for model in sorted({str(item["model"]) for item in observations}):
            for session in ("LONDON", "NEW_YORK"):
                for direction in ("LONG", "SHORT"):
                    selected = [
                        item for item in observations
                        if item["model"] == model
                        and item["session"] == session
                        and item["direction"] == direction
                        and predicate(str(item["sessionDate"]))
                    ]
                    if not selected:
                        continue
                    key = f"{model}:{session}:{direction}:{split}"
                    horizon_summary: dict[str, object] = {}
                    for horizon in HORIZONS:
                        outcomes = [
                            outcome_record
                            for item in selected
                            for outcome_record in item["outcomes"]
                            if outcome_record["horizonBars"] == horizon
                        ]
                        moves = [float(item["directionalMoveRanges"]) for item in outcomes]
                        horizon_summary[f"H{horizon}"] = {
                            "observations": len(moves),
                            "medianDirectionalMoveRanges": median(moves) if moves else None,
                            "positiveRate": sum(value > 0 for value in moves) / len(moves) if moves else None,
                            "medianFavorableMfeRanges": median(float(item["favorableMfeRanges"]) for item in outcomes) if outcomes else None,
                            "medianAdverseMfeRanges": median(float(item["adverseMfeRanges"]) for item in outcomes) if outcomes else None,
                            "targetTouchRate": sum(bool(item["targetTouched"]) for item in outcomes) / len(outcomes) if outcomes else None,
                        }
                    result["groups"][key] = {
                        "observations": len(selected),
                        "dates": len({str(item["sessionDate"]) for item in selected}),
                        "horizons": horizon_summary,
                    }
    return result


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--bars",
        type=Path,
        default=None,
        help="Auction-state bars CSV; defaults to latest snapshot",
    )
    parser.add_argument("--save", type=Path)
    args = parser.parse_args()
    try:
        bars_path = args.bars or latest("FabioOrderFlow/ledger-snapshots/auction-state-*-bars.csv")
        records = load_bars(bars_path)
        grouped = grouped_bars(records)
        observations = build_balance_rotations(grouped) + build_ny_continuations(grouped)
        dates = sorted({str(record["SessionDate"]) for record in records})
        report = {
            "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": str(bars_path),
            "contract": {
                "operationalEntries": False,
                "ordersSubmitted": False,
                "pnlComputed": False,
                "selectionLeakage": True,
                "independence": "maximum one observation per model/session/date/direction",
                "transcriptThresholds": {"LONDON": 20, "NEW_YORK": 30},
                "optimizedThresholds": [],
            },
            "counts": {
                "observations": len(observations),
                "models": dict(Counter(str(item["model"]) for item in observations)),
                "sessions": dict(Counter(str(item["session"]) for item in observations)),
                "directions": dict(Counter(str(item["direction"]) for item in observations)),
            },
            "summary": summarize(observations, dates),
            "observations": observations,
            "decision": {
                "validatedModels": [],
                "reason": "Post-hoc transcript reconstruction on an already inspected sample; prospective independent dates are required.",
            },
        }
        if args.save:
            args.save.parent.mkdir(parents=True, exist_ok=True)
            args.save.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
