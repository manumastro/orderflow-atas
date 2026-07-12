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
        return float(value.replace(",", "."))
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
                "pathMinutes": 30,
                "fixedHorizonDecision": "SUPERSEDED_BY_NY_IMPULSE_CONFIRMATION_BOUNDARY_RISK_V1",
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
            "decision": {
                "status": "SUPERSEDED_BY_BOUNDARY_RISK_V1",
                "plainMeaning": "Le letture a 5, 15 e 30 minuti restano descrittive. La decisione usa ora quale confine viene toccato per primo, non una durata fissa.",
                "validated": False,
                "promotedToExecutionSimulation": False,
            },
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
