#!/usr/bin/env python3
"""Report the causal New York A-to-B impulse-profile ledger as JSON and optional CSV."""

from __future__ import annotations

import argparse
import csv
import json
import os
import re
import sys
from collections import Counter
from datetime import datetime
from decimal import Decimal, InvalidOperation
from pathlib import Path

MODE_MARKER = "AUCTION_STATE_MODE"
READY_MARKER = "AUCTION_IMPULSE_READY"
PULLBACK_MARKER = "AUCTION_IMPULSE_PULLBACK_BAR"
RESOLVED_MARKER = "AUCTION_IMPULSE_RESOLVED"
SUMMARY_MARKER = "AUCTION_STATE_SUMMARY"

COMMON_FIELDS = (
    "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame", "StartBar",
    "EndBar", "ImpulseBars", "OriginBoundary", "ImpulseHigh", "ImpulseLow",
    "ImpulsePOC", "ImpulseVAH", "ImpulseVAL", "ImpulseLvns", "ImpulseLvnMetrics",
    "OperationalEntry", "OrderSubmitted",
)
PULLBACK_FIELDS = COMMON_FIELDS[:-2] + (
    "PullbackOrdinal", "Bar", "Close", "FrozenProfileRelation", "TouchedLvns", "TouchedLvnMetrics",
    "EffortResult", "CumulativeTradeCoverage", "MaxCumulativeBuy",
    "MaxCumulativeSell", "OperationalEntry", "OrderSubmitted",
)
RESOLVED_FIELDS = COMMON_FIELDS[:-2] + (
    "ResolvedBar", "EndReason", "OperationalEntry", "OrderSubmitted",
)
IDENTITY_FIELDS = ("ImpulseId", "SessionDate", "Direction", "ChartTimeFrame")
PULLBACK_EXPORT_FIELDS = IDENTITY_FIELDS + (
    "PullbackOrdinal", "Bar", "Close", "FrozenProfileRelation", "TouchedLvns",
    "TouchedLvnMetrics", "EffortResult", "CumulativeTradeCoverage",
    "MaxCumulativeBuy", "MaxCumulativeSell", "OperationalEntry", "OrderSubmitted",
)
RESOLVED_EXPORT_FIELDS = IDENTITY_FIELDS + (
    "ResolvedBar", "EndReason", "OperationalEntry", "OrderSubmitted",
)
INTEGER_FIELDS = {"StartBar", "EndBar", "ImpulseBars", "PullbackOrdinal", "Bar", "ResolvedBar"}
DECIMAL_FIELDS = {
    "OriginBoundary", "ImpulseHigh", "ImpulseLow", "ImpulsePOC", "ImpulseVAH",
    "ImpulseVAL", "Close", "MaxCumulativeBuy", "MaxCumulativeSell",
}
BOOLEAN_FIELDS = {"OperationalEntry", "OrderSubmitted"}
LVN_METRIC_FIELDS = (
    "Price", "VolumePercentile", "AdjacentDepth", "ShoulderDepth", "Prominence",
    "ProminenceRank", "ProminenceRankScore", "PositionInRange", "DirectionalProgress",
    "DistanceToPocRanges", "DistanceToOriginRanges", "DistanceToEdgeRanges",
)
LVN_FIELDS = (
    "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame", "StartBar",
) + LVN_METRIC_FIELDS
TOUCHED_LVN_FIELDS = (
    "ImpulseId", "SessionDate", "Direction", "ChartTimeFrame", "StartBar",
    "PullbackOrdinal", "Bar", "EffortResult", "CumulativeTradeCoverage",
    "MaxCumulativeBuy", "MaxCumulativeSell",
) + LVN_METRIC_FIELDS


def default_log() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs" / "FabioOrderFlow.log"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs" / "FabioOrderFlow.log"


def event_body(line: str, marker: str) -> str:
    token = f"[{marker}]"
    return line.split(token, 1)[1].lstrip(" ,") if token in line else ""


def raw_fields(body: str) -> dict[str, str]:
    return {
        match.group(1): match.group(2).strip()
        for match in re.finditer(
            r"(?:^|, )([A-Za-z][A-Za-z0-9_]*)=(.*?)(?=, [A-Za-z][A-Za-z0-9_]*=|$)",
            body,
        )
    }


def number(value: str) -> float | None:
    if not value or value == "NA":
        return None
    try:
        return float(Decimal(value))
    except InvalidOperation:
        return None


def convert(record: dict[str, str], fields: tuple[str, ...]) -> dict[str, object]:
    converted: dict[str, object] = {}
    for field in fields:
        value = record.get(field, "")
        if field in INTEGER_FIELDS:
            converted[field] = int(value) if value else None
        elif field in DECIMAL_FIELDS:
            converted[field] = number(value)
        elif field in BOOLEAN_FIELDS:
            converted[field] = value.upper() == "TRUE"
        else:
            converted[field] = value
    return converted


def parse_lvn_metrics(value: object) -> list[dict[str, object]]:
    if not value or value == "NONE":
        return []
    records: list[dict[str, object]] = []
    for encoded in str(value).split("|"):
        values = encoded.split(":")
        if len(values) != len(LVN_METRIC_FIELDS):
            raise ValueError(f"Invalid LVN metric record with {len(values)} fields: {encoded}")
        record: dict[str, object] = {}
        for field, raw in zip(LVN_METRIC_FIELDS, values, strict=True):
            record[field] = int(raw) if field == "ProminenceRank" else number(raw)
        records.append(record)
    return records


def expand_lvn_rows(
    profiles: list[dict[str, object]],
    pullbacks: list[dict[str, object]],
) -> tuple[list[dict[str, object]], list[dict[str, object]]]:
    identity_fields = ("ImpulseId", "SessionDate", "Direction", "ChartTimeFrame", "StartBar")
    lvns: list[dict[str, object]] = []
    for profile in profiles:
        identity = {field: profile[field] for field in identity_fields}
        lvns.extend({**identity, **metrics} for metrics in parse_lvn_metrics(profile["ImpulseLvnMetrics"]))

    touched: list[dict[str, object]] = []
    pullback_fields = identity_fields + (
        "PullbackOrdinal", "Bar", "EffortResult", "CumulativeTradeCoverage",
        "MaxCumulativeBuy", "MaxCumulativeSell",
    )
    for pullback in pullbacks:
        context = {field: pullback[field] for field in pullback_fields}
        touched.extend({**context, **metrics} for metrics in parse_lvn_metrics(pullback["TouchedLvnMetrics"]))
    return lvns, touched


def latest_run(lines: list[str]) -> tuple[dict[str, str], list[dict[str, object]], list[dict[str, object]], list[dict[str, object]], dict[str, str]]:
    starts = [index for index, line in enumerate(lines) if f"[{MODE_MARKER}]" in line]
    if not starts:
        raise ValueError(f"No [{MODE_MARKER}] marker found")
    start = starts[-1]
    finish_index = next(
        (index for index in range(start + 1, len(lines)) if f"[{SUMMARY_MARKER}]" in lines[index]),
        -1,
    )
    if finish_index < 0:
        raise ValueError("The latest auction-state run has no completed summary")
    run = lines[start:finish_index + 1]
    mode = raw_fields(event_body(lines[start], MODE_MARKER))
    if mode.get("ImpulseProfile") != "NEW_YORK_A_TO_B_CAUSAL":
        raise ValueError("The latest run does not include the causal impulse profiler; reload the updated DLL")
    ready_raw = [
        raw_fields(event_body(line, READY_MARKER))
        for line in run if f"[{READY_MARKER}]" in line
    ]
    common_by_impulse = {record["ImpulseId"]: record for record in ready_raw}

    def hydrate(line: str, marker: str) -> dict[str, str]:
        marker_record = raw_fields(event_body(line, marker))
        common = common_by_impulse.get(marker_record.get("ImpulseId", ""), {})
        return {**common, **marker_record}

    ready = [convert(record, COMMON_FIELDS) for record in ready_raw]
    pullbacks = [
        convert(hydrate(line, PULLBACK_MARKER), PULLBACK_FIELDS)
        for line in run if f"[{PULLBACK_MARKER}]" in line
    ]
    resolved = [
        convert(hydrate(line, RESOLVED_MARKER), RESOLVED_FIELDS)
        for line in run if f"[{RESOLVED_MARKER}]" in line
    ]
    summary = raw_fields(event_body(lines[finish_index], SUMMARY_MARKER))
    return mode, ready, pullbacks, resolved, summary


def build_report(log_path: Path) -> tuple[dict[str, object], dict[str, list[dict[str, object]]]]:
    lines = log_path.read_text(encoding="utf-8", errors="replace").splitlines()
    mode, ready, pullbacks, resolved, summary = latest_run(lines)
    all_records = ready + pullbacks + resolved
    operational = [
        record for record in all_records
        if record.get("OperationalEntry") or record.get("OrderSubmitted")
    ]
    coverage = Counter(str(record["CumulativeTradeCoverage"]) for record in pullbacks)
    lvn_rows, touched_lvn_rows = expand_lvn_rows(ready, pullbacks)
    report: dict[str, object] = {
        "generatedAt": datetime.now().astimezone().isoformat(timespec="seconds"),
        "source": {"log": str(log_path), "mode": mode, "summary": summary},
        "contract": {
            "model": "NEW_YORK_A_TO_B_CAUSAL",
            "profileFrozenBeforePullback": True,
            "lvnRanking": mode.get("LvnRanking", "LEGACY_RAW"),
            "lvnMetricSchema": [
                *LVN_METRIC_FIELDS,
            ],
            "allRawLvnsRetained": True,
            "selectionThresholds": [],
            "signalsGenerated": False,
            "pnlComputed": False,
            "operationalEntries": False,
            "ordersSubmitted": False,
        },
        "counts": {
            "profilesReady": len(ready),
            "pullbackBars": len(pullbacks),
            "profilesResolved": len(resolved),
            "directions": dict(sorted(Counter(str(record["Direction"]) for record in ready).items())),
            "endReasons": dict(sorted(Counter(str(record["EndReason"]) for record in resolved).items())),
            "pullbackCoverage": dict(sorted(coverage.items())),
            "sessionDates": len({str(record["SessionDate"]) for record in ready}),
            "rankedRawLvns": len(lvn_rows),
            "rankedTouchedLvnOccurrences": len(touched_lvn_rows),
        },
        "validation": {
            "summaryPresent": True,
            "nonOperational": not operational,
            "summaryCountsMatch": (
                int(summary.get("ImpulseProfilesReady", -1)) == len(ready)
                and int(summary.get("ImpulsePullbackBars", -1)) == len(pullbacks)
                and int(summary.get("ImpulseProfilesResolved", -1)) == len(resolved)
            ),
            "warnings": (["Cumulative trade coverage is missing for all pullback bars"] if pullbacks and coverage.get("MISSING", 0) == len(pullbacks) else []),
        },
        "profiles": ready,
        "pullbackBars": pullbacks,
        "resolutions": resolved,
    }
    return report, {
        "profiles": ready,
        "pullbacks": pullbacks,
        "resolutions": resolved,
        "lvns": lvn_rows,
        "touchedLvns": touched_lvn_rows,
    }


def save_outputs(report: dict[str, object], datasets: dict[str, list[dict[str, object]]], directory: Path) -> dict[str, str]:
    directory.mkdir(parents=True, exist_ok=True)
    stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    base = directory / f"auction-impulse-{stamp}"
    paths = {
        "summaryJson": base.with_name(f"{base.name}-summary.json"),
        "profilesCsv": base.with_name(f"{base.name}-profiles.csv"),
        "pullbacksCsv": base.with_name(f"{base.name}-pullbacks.csv"),
        "resolutionsCsv": base.with_name(f"{base.name}-resolutions.csv"),
        "lvnsCsv": base.with_name(f"{base.name}-lvns.csv"),
        "touchedLvnsCsv": base.with_name(f"{base.name}-touched-lvns.csv"),
    }
    paths["summaryJson"].write_text(json.dumps(report, indent=2, ensure_ascii=True) + "\n", encoding="utf-8")
    for key, fields, path_key in (
        ("profiles", COMMON_FIELDS, "profilesCsv"),
        ("pullbacks", PULLBACK_EXPORT_FIELDS, "pullbacksCsv"),
        ("resolutions", RESOLVED_EXPORT_FIELDS, "resolutionsCsv"),
        ("lvns", LVN_FIELDS, "lvnsCsv"),
        ("touchedLvns", TOUCHED_LVN_FIELDS, "touchedLvnsCsv"),
    ):
        with paths[path_key].open("w", encoding="utf-8", newline="") as handle:
            writer = csv.DictWriter(handle, fieldnames=fields, extrasaction="ignore")
            writer.writeheader()
            writer.writerows(datasets[key])
    return {key: str(path) for key, path in paths.items()}


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--log", type=Path, default=default_log())
    parser.add_argument("--save", action="store_true")
    parser.add_argument("--output-dir", type=Path, default=Path("FabioOrderFlow") / "ledger-snapshots")
    args = parser.parse_args()
    try:
        report, datasets = build_report(args.log)
        if args.save:
            report["saved"] = save_outputs(report, datasets, args.output_dir)
        json.dump(report, sys.stdout, indent=2, ensure_ascii=True)
        sys.stdout.write("\n")
        return 0
    except Exception as exc:
        json.dump({"error": str(exc), "log": str(args.log)}, sys.stdout, ensure_ascii=True)
        sys.stdout.write("\n")
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
