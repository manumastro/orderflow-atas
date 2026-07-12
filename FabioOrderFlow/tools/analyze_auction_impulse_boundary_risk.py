#!/usr/bin/env python3
"""Evaluate confirmation-close entry against impulse extreme B and origin A."""

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

MINIMUM_DIRECTIONAL_CUMULATIVE = 30.0
COST_SCENARIOS_POINTS = (0.5, 1.0, 1.5)
FEASIBILITY_MINIMUM_PER_DIRECTION = 8
EXTENDED_MINIMUM_OBSERVATIONS = 30
EXTENDED_MINIMUM_PER_DIRECTION = 10
MINIMUM_PROFIT_FACTOR = 1.25
MAXIMUM_AMBIGUOUS_RATE = 0.10


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


def directional_maxima(record: dict[str, str], direction: str) -> tuple[float, float]:
    buy = float(record["MaxCumulativeBuy"])
    sell = float(record["MaxCumulativeSell"])
    return (buy, sell) if direction == "LONG" else (sell, buy)


def confirmation_for_impulse(
    profile: dict[str, str],
    pullbacks: list[dict[str, str]],
    resolution: dict[str, str],
) -> dict[str, str] | None:
    direction = profile["Direction"]
    expected_result = "BUY_WITH_RESULT" if direction == "LONG" else "SELL_WITH_RESULT"
    resolved_bar = int(resolution["ResolvedBar"])
    for record in sorted(pullbacks, key=lambda item: int(item["PullbackOrdinal"])):
        directional, opposite = directional_maxima(record, direction)
        if (
            int(record["Bar"]) < resolved_bar
            and record["CumulativeTradeCoverage"] == "AVAILABLE"
            and record["TouchedLvns"] not in {"", "NONE"}
            and record["EffortResult"] == expected_result
            and directional >= MINIMUM_DIRECTIONAL_CUMULATIVE
            and directional > opposite
        ):
            return record
    return None


def primary_confirmations(
    profiles: list[dict[str, str]],
    pullbacks: list[dict[str, str]],
    resolutions: list[dict[str, str]],
) -> list[tuple[dict[str, str], dict[str, str], dict[str, str]]]:
    by_impulse: dict[str, list[dict[str, str]]] = defaultdict(list)
    for record in pullbacks:
        by_impulse[record["ImpulseId"]].append(record)
    resolution_by_impulse = {record["ImpulseId"]: record for record in resolutions}
    candidates: list[tuple[dict[str, str], dict[str, str], dict[str, str]]] = []
    for profile in profiles:
        resolution = resolution_by_impulse.get(profile["ImpulseId"])
        if resolution is None:
            continue
        confirmation = confirmation_for_impulse(profile, by_impulse[profile["ImpulseId"]], resolution)
        if confirmation is not None:
            candidates.append((profile, confirmation, resolution))

    selected: list[tuple[dict[str, str], dict[str, str], dict[str, str]]] = []
    seen: set[tuple[str, str]] = set()
    for item in sorted(candidates, key=lambda value: int(value[0]["StartBar"])):
        profile = item[0]
        key = (profile["SessionDate"], profile["Direction"])
        if key in seen:
            continue
        seen.add(key)
        selected.append(item)
    return selected


def level_touches(record: dict[str, str], direction: str, target: float, invalidation: float) -> tuple[bool, bool]:
    high = float(record["High"])
    low = float(record["Low"])
    if direction == "LONG":
        return high >= target, low <= invalidation
    return low <= target, high >= invalidation


def evaluate_observations(
    profiles: list[dict[str, str]],
    pullbacks: list[dict[str, str]],
    resolutions: list[dict[str, str]],
) -> list[dict[str, object]]:
    required_ohlc = {"Open", "High", "Low", "Close"}
    if pullbacks and not required_ohlc.issubset(pullbacks[0]):
        missing = sorted(required_ohlc - set(pullbacks[0]))
        raise ValueError(f"Pullback dataset requires a reload with OHLC fields; missing: {','.join(missing)}")

    by_impulse: dict[str, list[dict[str, str]]] = defaultdict(list)
    for record in pullbacks:
        by_impulse[record["ImpulseId"]].append(record)

    observations: list[dict[str, object]] = []
    for profile, confirmation, resolution in primary_confirmations(profiles, pullbacks, resolutions):
        direction = profile["Direction"]
        entry = float(confirmation["Close"])
        target = float(profile["ImpulseHigh"] if direction == "LONG" else profile["ImpulseLow"])
        invalidation = float(profile["OriginBoundary"])
        reward_points = target - entry if direction == "LONG" else entry - target
        risk_points = entry - invalidation if direction == "LONG" else invalidation - entry
        path = [
            record for record in sorted(by_impulse[profile["ImpulseId"]], key=lambda item: int(item["Bar"]))
            if int(record["Bar"]) > int(confirmation["Bar"])
            and int(record["Bar"]) <= int(resolution["ResolvedBar"])
        ]

        outcome = "INVALID_GEOMETRY"
        outcome_bar: int | None = None
        exit_price: float | None = None
        gross_r: float | None = None
        both_touched = False
        if reward_points > 0 and risk_points > 0:
            for record in path:
                target_touched, invalidation_touched = level_touches(record, direction, target, invalidation)
                if target_touched and invalidation_touched:
                    outcome = "AMBIGUOUS_SAME_BAR_AS_LOSS"
                    outcome_bar = int(record["Bar"])
                    exit_price = invalidation
                    gross_r = -1.0
                    both_touched = True
                    break
                if target_touched:
                    outcome = "TARGET_B_FIRST"
                    outcome_bar = int(record["Bar"])
                    exit_price = target
                    gross_r = reward_points / risk_points
                    break
                if invalidation_touched:
                    outcome = "ORIGIN_A_FIRST"
                    outcome_bar = int(record["Bar"])
                    exit_price = invalidation
                    gross_r = -1.0
                    break
            else:
                final_close = float(path[-1]["Close"] if path else confirmation["Close"])
                outcome = "SESSION_CLOSE_NO_BOUNDARY"
                outcome_bar = int(path[-1]["Bar"] if path else confirmation["Bar"])
                exit_price = final_close
                directional_move = final_close - entry if direction == "LONG" else entry - final_close
                gross_r = directional_move / risk_points

        observation: dict[str, object] = {
            "impulseId": profile["ImpulseId"],
            "sessionDate": profile["SessionDate"],
            "direction": direction,
            "chartTimeFrame": profile["ChartTimeFrame"],
            "confirmationBar": int(confirmation["Bar"]),
            "resolvedBar": int(resolution["ResolvedBar"]),
            "entryPrice": entry,
            "targetB": target,
            "invalidationA": invalidation,
            "rewardPoints": reward_points,
            "riskPoints": risk_points,
            "rewardRiskRatio": reward_points / risk_points if reward_points > 0 and risk_points > 0 else None,
            "outcome": outcome,
            "outcomeBar": outcome_bar,
            "barsToOutcome": outcome_bar - int(confirmation["Bar"]) if outcome_bar is not None else None,
            "exitPrice": exit_price,
            "grossR": gross_r,
            "sameBarAmbiguous": both_touched,
            "lifecycleEndReason": resolution["EndReason"],
            "operationalEntry": False,
            "orderSubmitted": False,
            "pnlComputed": False,
        }
        for cost in COST_SCENARIOS_POINTS:
            key = str(cost).replace(".", "p")
            observation[f"netRAt{key}PointsCost"] = (
                gross_r - cost / risk_points if gross_r is not None and risk_points > 0 else None
            )
        observations.append(observation)
    return observations


def profit_factor(values: list[float]) -> float | None:
    gains = sum(value for value in values if value > 0)
    losses = -sum(value for value in values if value < 0)
    if losses == 0:
        return None
    return gains / losses


def summarize(records: list[dict[str, object]]) -> dict[str, object]:
    summary: dict[str, object] = {
        "observations": len(records),
        "dates": len({str(record["sessionDate"]) for record in records}),
        "directions": dict(sorted(Counter(str(record["direction"]) for record in records).items())),
        "outcomes": dict(sorted(Counter(str(record["outcome"]) for record in records).items())),
        "ambiguousRate": sum(bool(record["sameBarAmbiguous"]) for record in records) / len(records) if records else None,
        "medianRewardPoints": statistics.median(float(record["rewardPoints"]) for record in records) if records else None,
        "medianRiskPoints": statistics.median(float(record["riskPoints"]) for record in records) if records else None,
        "medianRewardRiskRatio": statistics.median(
            float(record["rewardRiskRatio"]) for record in records if record["rewardRiskRatio"] is not None
        ) if records else None,
        "costScenarios": {},
    }
    for cost in COST_SCENARIOS_POINTS:
        key = str(cost).replace(".", "p")
        values = [float(record[f"netRAt{key}PointsCost"]) for record in records if record[f"netRAt{key}PointsCost"] is not None]
        summary["costScenarios"][str(cost)] = {
            "observations": len(values),
            "meanNetR": statistics.mean(values) if values else None,
            "medianNetR": statistics.median(values) if values else None,
            "profitFactor": profit_factor(values),
            "positiveRate": sum(value > 0 for value in values) / len(values) if values else None,
        }
    return summary


def feasibility_decision(observations: list[dict[str, object]]) -> dict[str, object]:
    grouped = {
        "ALL": observations,
        "LONG": [record for record in observations if record["direction"] == "LONG"],
        "SHORT": [record for record in observations if record["direction"] == "SHORT"],
    }
    summaries = {name: summarize(records) for name, records in grouped.items()}
    sample_ready = all(len(grouped[direction]) >= FEASIBILITY_MINIMUM_PER_DIRECTION for direction in ("LONG", "SHORT"))
    checks: dict[str, bool] = {
        "minimumEightPerDirection": sample_ready,
        "ambiguousRateAtMost10Percent": (summaries["ALL"]["ambiguousRate"] or 0) <= MAXIMUM_AMBIGUOUS_RATE,
        "noInvalidGeometry": not any(record["outcome"] == "INVALID_GEOMETRY" for record in observations),
    }
    for name in ("ALL", "LONG", "SHORT"):
        scenario = summaries[name]["costScenarios"]["1.0"]
        checks[f"{name.lower()}MeanNetRAt1PointCostPositive"] = scenario["meanNetR"] is not None and scenario["meanNetR"] > 0
        checks[f"{name.lower()}ProfitFactorAtLeast1Point25"] = (
            (scenario["profitFactor"] is not None and scenario["profitFactor"] >= MINIMUM_PROFIT_FACTOR)
            or (scenario["profitFactor"] is None and scenario["meanNetR"] is not None and scenario["meanNetR"] > 0)
        )

    if not sample_ready:
        status = "INSUFFICIENT_FEASIBILITY_SAMPLE"
        meaning = "Il campione non contiene ancora almeno otto casi per entrambe le direzioni."
    elif all(checks.values()):
        status = "EXTEND_HISTORICAL_HOLDOUT"
        meaning = "Il controllo iniziale e' positivo; congelare il contratto e ampliare lo storico fino ad almeno 30 casi, con almeno 10 per direzione."
    else:
        status = "REJECTED_HISTORICAL_FEASIBILITY"
        meaning = "Il candidato fallisce almeno un controllo storico minimo e non deve passare alla raccolta prospettica."
    return {
        "status": status,
        "plainMeaning": meaning,
        "checks": checks,
        "validated": False,
        "promotedToProspective": False,
        "nextHistoricalMinimumObservations": EXTENDED_MINIMUM_OBSERVATIONS,
        "nextHistoricalMinimumPerDirection": EXTENDED_MINIMUM_PER_DIRECTION,
    }


def write_csv(path: Path, rows: list[dict[str, object]]) -> None:
    if not rows:
        return
    fields = list(dict.fromkeys(key for row in rows for key in row))
    with path.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=fields)
        writer.writeheader()
        writer.writerows(rows)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--timeframe", choices=("M1",), default="M1")
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
        observations = evaluate_observations(profiles, pullbacks, resolutions)
        report: dict[str, object] = {
            "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": {
                "profiles": str(profile_path),
                "pullbacks": str(pullback_path),
                "resolutions": str(resolution_path),
                "chartTimeFrame": args.timeframe,
            },
            "contract": {
                "model": "NY_IMPULSE_CONFIRMATION_BOUNDARY_RISK_V1",
                "plainMeaning": "Dalla chiusura della conferma verifica se il prezzo tocca prima il vecchio estremo B oppure il confine di origine A.",
                "entry": "confirmation bar close",
                "target": "frozen impulse extreme B",
                "invalidation": "impulse origin boundary A",
                "sameBarPolicy": "target and invalidation in the same M1 bar count as a loss",
                "noBoundaryPolicy": "directional close at New York session end",
                "costScenariosPoints": list(COST_SCENARIOS_POINTS),
                "selectionLeakage": True,
                "operationalEntry": False,
                "ordersSubmitted": False,
                "pnlComputed": False,
                "technicalTerms": {
                    "R": "Unita' di rischio: +1R equivale al rischio iniziale guadagnato; -1R equivale all'intero rischio perso.",
                    "profitFactor": "Somma dei risultati positivi divisa per la somma assoluta dei risultati negativi.",
                    "ambiguous": "Nella stessa candela M1 sono stati toccati sia obiettivo sia invalidazione e non conosciamo l'ordine esatto.",
                },
            },
            "dataset": {
                "profiles": len(profiles),
                "pullbackBars": len(pullbacks),
                "resolutions": len(resolutions),
                "observations": len(observations),
            },
            "total": summarize(observations),
            "byDirection": {
                direction: summarize([record for record in observations if record["direction"] == direction])
                for direction in ("LONG", "SHORT")
            },
            "decision": feasibility_decision(observations),
            "observations": observations,
        }
        if args.save:
            args.output_dir.mkdir(parents=True, exist_ok=True)
            stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
            prefix = args.output_dir / f"auction-impulse-boundary-risk-M1-{stamp}"
            summary_path = prefix.with_name(prefix.name + "-summary.json")
            observations_path = prefix.with_name(prefix.name + "-observations.csv")
            report["saved"] = {
                "summaryJson": str(summary_path),
                "observationsCsv": str(observations_path),
            }
            write_csv(observations_path, observations)
            summary_path.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True, allow_nan=False)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
