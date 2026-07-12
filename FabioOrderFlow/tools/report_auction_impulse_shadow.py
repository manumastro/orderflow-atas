#!/usr/bin/env python3
"""Report the no-order New York cumulative-confirmation shadow study."""

from __future__ import annotations

import argparse
import csv
import json
import re
import statistics
import sys
from collections import Counter, defaultdict
from datetime import datetime
from pathlib import Path

ENTRY_MARKER = "AUCTION_IMPULSE_SHADOW_ENTRY"
PATH_MARKER = "AUCTION_IMPULSE_SHADOW_PATH"
SHADOW_RESOLVED_MARKER = "AUCTION_IMPULSE_SHADOW_RESOLVED"
IMPULSE_READY_MARKER = "AUCTION_IMPULSE_READY"
IMPULSE_RESOLVED_MARKER = "AUCTION_IMPULSE_RESOLVED"
SUMMARY_MARKER = "AUCTION_STATE_SUMMARY"
PROSPECTIVE_START = "2026-07-13"
MINIMUM_OBSERVATIONS = 20
MINIMUM_PER_DIRECTION = 8
MAXIMUM_SESSION_DATES = 40
MINIMUM_COMPLETE_15_MINUTE_PATHS = 18

ENTRY_FIELDS = (
    "ShadowId", "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame",
    "EvaluationCohort", "ConfirmationBar", "ConfirmationTimeItaly", "ShadowEntryPrice",
    "PullbackOrdinal", "TouchedLvns", "DirectionalCumulativeMax", "OppositeCumulativeMax",
    "MinimumDirectionalCumulativeTrade", "Meaning", "OperationalEntry", "OrderSubmitted",
    "PnLComputed",
)
PATH_FIELDS = (
    "ShadowId", "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame",
    "EvaluationCohort", "PathBarOrdinal", "Bar", "BarEndItaly", "ElapsedMinutes",
    "Open", "High", "Low", "Close", "DirectionalCloseMovePoints", "FavorableMfePoints",
    "AdverseMaePoints", "DirectionalCloseMoveRanges", "FavorableMfeRanges",
    "AdverseMaeRanges", "CumulativeTradeCoverage", "MaxCumulativeBuy",
    "MaxCumulativeSell", "Completes30MinutePath", "OperationalEntry", "OrderSubmitted",
    "PnLComputed",
)
SHADOW_RESOLVED_FIELDS = (
    "ShadowId", "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame",
    "EvaluationCohort", "ResolvedBar", "EndReason", "BarsFromConfirmationToResolution",
    "OperationalEntry", "OrderSubmitted", "PnLComputed",
)
IMPULSE_READY_FIELDS = ("ImpulseId", "SessionDate", "Direction", "ChartTimeFrame")
IMPULSE_RESOLVED_FIELDS = (
    "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame", "ResolvedBar", "EndReason",
)
INTEGER_FIELDS = {
    "ConfirmationBar", "PullbackOrdinal", "PathBarOrdinal", "Bar", "ResolvedBar",
    "BarsFromConfirmationToResolution",
}
FLOAT_FIELDS = {
    "ShadowEntryPrice", "DirectionalCumulativeMax", "OppositeCumulativeMax",
    "MinimumDirectionalCumulativeTrade", "ElapsedMinutes", "Open", "High", "Low", "Close",
    "DirectionalCloseMovePoints", "FavorableMfePoints", "AdverseMaePoints",
    "DirectionalCloseMoveRanges", "FavorableMfeRanges", "AdverseMaeRanges",
    "MaxCumulativeBuy", "MaxCumulativeSell",
}
BOOLEAN_FIELDS = {"Completes30MinutePath", "OperationalEntry", "OrderSubmitted", "PnLComputed"}


def parse_value(key: str, value: str) -> object:
    if key in INTEGER_FIELDS:
        return int(value)
    if key in FLOAT_FIELDS:
        return float(value)
    if key in BOOLEAN_FIELDS:
        return value.upper() == "TRUE"
    return value


def parse_fields(text: str, fields: tuple[str, ...]) -> dict[str, object]:
    positions: list[tuple[int, str]] = []
    for field in fields:
        match = re.search(rf"(?:^|, ){re.escape(field)}=", text)
        if match:
            positions.append((match.start() + (2 if match.group(0).startswith(", ") else 0), field))
    positions.sort()
    record: dict[str, object] = {}
    for index, (start, field) in enumerate(positions):
        value_start = start + len(field) + 1
        value_end = positions[index + 1][0] - 2 if index + 1 < len(positions) else len(text)
        record[field] = parse_value(field, text[value_start:value_end])
    return record


def parse_marker(line: str, marker: str, fields: tuple[str, ...]) -> dict[str, object] | None:
    token = f"[{marker}] "
    if token not in line:
        return None
    return parse_fields(line.split(token, 1)[1].strip(), fields)


def parse_summary(line: str) -> dict[str, object] | None:
    token = f"[{SUMMARY_MARKER}] "
    if token not in line:
        return None
    record: dict[str, object] = {}
    for part in line.split(token, 1)[1].strip().split(", "):
        if "=" not in part:
            continue
        key, value = part.split("=", 1)
        if value.isdigit():
            record[key] = int(value)
        else:
            record[key] = value
    return record


def read_log(path: Path) -> dict[str, object]:
    entries: list[dict[str, object]] = []
    paths: list[dict[str, object]] = []
    shadow_resolutions: list[dict[str, object]] = []
    impulse_ready: list[dict[str, object]] = []
    impulse_resolutions: list[dict[str, object]] = []
    summary: dict[str, object] | None = None
    with path.open(encoding="utf-8", errors="replace") as handle:
        for line in handle:
            for marker, fields, target in (
                (ENTRY_MARKER, ENTRY_FIELDS, entries),
                (PATH_MARKER, PATH_FIELDS, paths),
                (SHADOW_RESOLVED_MARKER, SHADOW_RESOLVED_FIELDS, shadow_resolutions),
                (IMPULSE_READY_MARKER, IMPULSE_READY_FIELDS, impulse_ready),
                (IMPULSE_RESOLVED_MARKER, IMPULSE_RESOLVED_FIELDS, impulse_resolutions),
            ):
                record = parse_marker(line, marker, fields)
                if record is not None:
                    target.append(record)
                    break
            parsed_summary = parse_summary(line)
            if parsed_summary is not None:
                summary = parsed_summary
    return {
        "entries": entries,
        "paths": paths,
        "shadowResolutions": shadow_resolutions,
        "impulseReady": impulse_ready,
        "impulseResolutions": impulse_resolutions,
        "summary": summary,
    }


def hydrate(data: dict[str, object]) -> None:
    entries = data["entries"]
    by_shadow = {str(record["ShadowId"]): record for record in entries}
    for collection_name in ("paths", "shadowResolutions"):
        for record in data[collection_name]:
            entry = by_shadow.get(str(record.get("ShadowId")))
            if entry is None:
                continue
            for field in ENTRY_FIELDS:
                if field in entry and field not in record:
                    record[field] = entry[field]


def first_at_horizon(records: list[dict[str, object]], minutes: float) -> dict[str, object] | None:
    eligible = [record for record in records if float(record["ElapsedMinutes"]) >= minutes]
    return min(eligible, key=lambda record: float(record["ElapsedMinutes"])) if eligible else None


def observation_rows(data: dict[str, object]) -> list[dict[str, object]]:
    paths_by_shadow: dict[str, list[dict[str, object]]] = defaultdict(list)
    for record in data["paths"]:
        paths_by_shadow[str(record["ShadowId"])].append(record)
    resolution_by_shadow = {
        str(record["ShadowId"]): record for record in data["shadowResolutions"]
    }
    rows: list[dict[str, object]] = []
    for entry in sorted(data["entries"], key=lambda record: int(record["ConfirmationBar"])):
        shadow_id = str(entry["ShadowId"])
        paths = sorted(paths_by_shadow[shadow_id], key=lambda record: int(record["PathBarOrdinal"]))
        resolution = resolution_by_shadow.get(shadow_id, {})
        row = dict(entry)
        row.update({
            "EndReason": resolution.get("EndReason"),
            "ResolvedBar": resolution.get("ResolvedBar"),
            "PathBars": len(paths),
            "Completes30MinutePath": any(bool(record["Completes30MinutePath"]) for record in paths),
        })
        for minutes in (5, 15, 30):
            point = first_at_horizon(paths, minutes)
            prefix = f"H{minutes}"
            row[f"{prefix}Available"] = point is not None
            row[f"{prefix}ElapsedMinutes"] = point.get("ElapsedMinutes") if point else None
            row[f"{prefix}DirectionalCloseMovePoints"] = point.get("DirectionalCloseMovePoints") if point else None
            row[f"{prefix}FavorableMfePoints"] = point.get("FavorableMfePoints") if point else None
            row[f"{prefix}AdverseMaePoints"] = point.get("AdverseMaePoints") if point else None
        rows.append(row)
    return rows


def rate(records: list[dict[str, object]], reason: str) -> float | None:
    return sum(record.get("EndReason") == reason for record in records) / len(records) if records else None


def median(records: list[dict[str, object]], field: str) -> float | None:
    values = [float(record[field]) for record in records if record.get(field) is not None]
    return statistics.median(values) if values else None


def summarize(records: list[dict[str, object]]) -> dict[str, object]:
    return {
        "observations": len(records),
        "dates": len({str(record["SessionDate"]) for record in records}),
        "directions": dict(sorted(Counter(str(record["Direction"]) for record in records).items())),
        "endReasons": dict(sorted(Counter(str(record.get("EndReason")) for record in records).items())),
        "continuationRate": rate(records, "CONTINUATION_NEW_EXTREME"),
        "complete15MinutePaths": sum(bool(record["H15Available"]) for record in records),
        "complete30MinutePaths": sum(bool(record["Completes30MinutePath"]) for record in records),
        "medianH15DirectionalCloseMovePoints": median(records, "H15DirectionalCloseMovePoints"),
        "medianH15FavorableMfePoints": median(records, "H15FavorableMfePoints"),
        "medianH15AdverseMaePoints": median(records, "H15AdverseMaePoints"),
    }


def first_decision_sample(records: list[dict[str, object]]) -> list[dict[str, object]]:
    selected: list[dict[str, object]] = []
    for record in sorted(records, key=lambda item: int(item["ConfirmationBar"])):
        selected.append(record)
        directions = Counter(str(item["Direction"]) for item in selected)
        if len(selected) >= MINIMUM_OBSERVATIONS and all(
            directions.get(direction, 0) >= MINIMUM_PER_DIRECTION for direction in ("LONG", "SHORT")
        ):
            return selected
    return []


def baseline_rate(data: dict[str, object], through_date: str | None) -> float | None:
    resolutions = {
        str(record["ImpulseId"]): record for record in data["impulseResolutions"]
    }
    eligible = [
        record for record in data["impulseReady"]
        if str(record["SessionDate"]) >= PROSPECTIVE_START
        and (through_date is None or str(record["SessionDate"]) <= through_date)
        and str(record["ImpulseId"]) in resolutions
    ]
    return (
        sum(resolutions[str(record["ImpulseId"])].get("EndReason") == "CONTINUATION_NEW_EXTREME" for record in eligible)
        / len(eligible)
        if eligible else None
    )


def decision(data: dict[str, object], prospective: list[dict[str, object]]) -> dict[str, object]:
    prospective_dates = {
        str(record["SessionDate"]) for record in data["impulseReady"]
        if str(record["SessionDate"]) >= PROSPECTIVE_START
    }
    sample = first_decision_sample(prospective)
    if not sample:
        status = "REJECTED_RARITY" if len(prospective_dates) >= MAXIMUM_SESSION_DATES else "COLLECTING"
        return {
            "status": status,
            "plainMeaning": (
                "La conferma e' comparsa troppo raramente per raggiungere il campione previsto entro 40 sessioni New York."
                if status == "REJECTED_RARITY"
                else "La prova prospettica sta ancora raccogliendo osservazioni; non e' ancora permessa una conclusione sul modello."
            ),
            "validated": False,
            "promotedToExecutionSimulation": False,
            "prospectiveSessionDates": len(prospective_dates),
            "decisionSampleObservations": 0,
            "checks": {},
        }

    through_date = max(str(record["SessionDate"]) for record in sample)
    baseline = baseline_rate(data, through_date)
    overall_rate = rate(sample, "CONTINUATION_NEW_EXTREME")
    by_direction = {
        direction: [record for record in sample if record["Direction"] == direction]
        for direction in ("LONG", "SHORT")
    }
    favorable = median(sample, "H15FavorableMfePoints")
    adverse = median(sample, "H15AdverseMaePoints")
    ratio = favorable / adverse if favorable is not None and adverse not in (None, 0.0) else None
    checks = {
        "overallContinuationAtLeast60Percent": overall_rate is not None and overall_rate >= 0.60,
        "longContinuationAtLeast50Percent": (rate(by_direction["LONG"], "CONTINUATION_NEW_EXTREME") or 0) >= 0.50,
        "shortContinuationAtLeast50Percent": (rate(by_direction["SHORT"], "CONTINUATION_NEW_EXTREME") or 0) >= 0.50,
        "upliftAtLeast15PercentagePoints": baseline is not None and overall_rate is not None and overall_rate - baseline >= 0.15,
        "atLeast18Complete15MinutePaths": sum(bool(record["H15Available"]) for record in sample) >= MINIMUM_COMPLETE_15_MINUTE_PATHS,
        "longMedian15MinuteMovePositive": (median(by_direction["LONG"], "H15DirectionalCloseMovePoints") or 0) > 0,
        "shortMedian15MinuteMovePositive": (median(by_direction["SHORT"], "H15DirectionalCloseMovePoints") or 0) > 0,
        "medianFavorableToAdverseRatioAtLeast1Point5": ratio is not None and ratio >= 1.5,
    }
    passed = all(checks.values())
    return {
        "status": "PROMOTED_TO_EXECUTION_SIMULATION" if passed else "REJECTED",
        "plainMeaning": (
            "Il campione prospettico congelato ha superato tutti i controlli e puo' avanzare a una simulazione che includa i costi; non e' ancora autorizzato alcun ordine reale."
            if passed
            else "Il primo campione prospettico completo ha fallito almeno un controllo; il candidato shadow viene quindi scartato senza modificarne le regole."
        ),
        "validated": False,
        "promotedToExecutionSimulation": passed,
        "prospectiveSessionDates": len(prospective_dates),
        "decisionSampleObservations": len(sample),
        "decisionSampleThroughDate": through_date,
        "candidateContinuationRate": overall_rate,
        "contemporaneousImpulseBaselineRate": baseline,
        "continuationUplift": overall_rate - baseline if overall_rate is not None and baseline is not None else None,
        "medianH15FavorableToAdverseRatio": ratio,
        "checks": checks,
        "decisionSampleShadowIds": [record["ShadowId"] for record in sample],
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
    parser.add_argument("--log", type=Path, default=Path.home() / "AppData/Roaming/ATAS/Logs/FabioOrderFlow.log")
    parser.add_argument("--save", action="store_true")
    parser.add_argument("--output-dir", type=Path, default=Path("FabioOrderFlow") / "ledger-snapshots")
    args = parser.parse_args()
    try:
        data = read_log(args.log)
        hydrate(data)
        observations = observation_rows(data)
        prospective = [record for record in observations if record["EvaluationCohort"] == "PROSPECTIVE"]
        historical = [record for record in observations if record["EvaluationCohort"] == "HISTORICAL_REFERENCE"]
        duplicate_primary = [
            key for key, count in Counter(
                (str(record["SessionDate"]), str(record["Direction"])) for record in observations
            ).items() if count > 1
        ]
        path_by_shadow: dict[str, list[dict[str, object]]] = defaultdict(list)
        for record in data["paths"]:
            path_by_shadow[str(record["ShadowId"])].append(record)
        path_errors: list[str] = []
        for shadow_id, records in path_by_shadow.items():
            ordered = sorted(records, key=lambda record: int(record["PathBarOrdinal"]))
            if [int(record["PathBarOrdinal"]) for record in ordered] != list(range(1, len(ordered) + 1)):
                path_errors.append(f"{shadow_id}:ordinal")
            if any(float(current["FavorableMfePoints"]) < float(previous["FavorableMfePoints"])
                   or float(current["AdverseMaePoints"]) < float(previous["AdverseMaePoints"])
                   for previous, current in zip(ordered, ordered[1:])):
                path_errors.append(f"{shadow_id}:excursion")
        non_operational = all(
            not record.get("OperationalEntry") and not record.get("OrderSubmitted") and not record.get("PnLComputed")
            for collection in (data["entries"], data["paths"], data["shadowResolutions"])
            for record in collection
        )
        summary = data["summary"] or {}
        report: dict[str, object] = {
            "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
            "source": str(args.log),
            "contract": {
                "model": "NY_IMPULSE_CUMULATIVE_CONFIRMATION_SHADOW_V1",
                "plainMeaning": "Osserva il primo pullback con aggressione cumulative valida per data New York e direzione, senza inviare ordini.",
                "prospectiveStartSessionDate": PROSPECTIVE_START,
                "minimumObservations": MINIMUM_OBSERVATIONS,
                "minimumPerDirection": MINIMUM_PER_DIRECTION,
                "maximumSessionDates": MAXIMUM_SESSION_DATES,
                "minimumComplete15MinutePaths": MINIMUM_COMPLETE_15_MINUTE_PATHS,
                "pathMinutes": 30,
                "technicalTerms": {
                    "MFE": "Massima escursione favorevole: il punto piu' lontano raggiunto dal prezzo nella direzione attesa.",
                    "MAE": "Massima escursione contraria: il punto piu' lontano raggiunto dal prezzo contro la direzione attesa.",
                    "shadow": "Osservazione simulata registrata senza inviare un ordine o calcolare profitto di trading.",
                },
                "operationalEntry": False,
                "ordersSubmitted": False,
                "pnlComputed": False,
            },
            "counts": {
                "entries": len(data["entries"]),
                "pathBars": len(data["paths"]),
                "resolutions": len(data["shadowResolutions"]),
                "historicalReferenceEntries": len(historical),
                "prospectiveEntries": len(prospective),
            },
            "historicalReference": summarize(historical),
            "prospective": summarize(prospective),
            "validation": {
                "summaryPresent": bool(data["summary"]),
                "summaryCountsMatch": bool(data["summary"]) and int(summary.get("ShadowEntries", -1)) == len(data["entries"])
                    and int(summary.get("ShadowPathBars", -1)) == len(data["paths"])
                    and int(summary.get("ShadowResolutions", -1)) == len(data["shadowResolutions"]),
                "oneObservationPerDateDirection": not duplicate_primary,
                "duplicateDateDirections": duplicate_primary,
                "pathErrors": path_errors,
                "nonOperational": non_operational,
            },
            "decision": decision(data, prospective),
            "observations": observations,
        }
        if args.save:
            args.output_dir.mkdir(parents=True, exist_ok=True)
            stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
            prefix = args.output_dir / f"auction-impulse-shadow-{stamp}"
            summary_path = prefix.with_name(prefix.name + "-summary.json")
            entries_path = prefix.with_name(prefix.name + "-observations.csv")
            paths_path = prefix.with_name(prefix.name + "-paths.csv")
            write_csv(entries_path, observations)
            write_csv(paths_path, data["paths"])
            report["saved"] = {
                "summaryJson": str(summary_path),
                "observationsCsv": str(entries_path) if observations else None,
                "pathsCsv": str(paths_path) if data["paths"] else None,
            }
            summary_path.write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
