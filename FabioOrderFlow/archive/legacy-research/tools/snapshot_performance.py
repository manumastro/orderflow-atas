#!/usr/bin/env python3
"""Create a persistent FabioOrderFlow performance snapshot from ATAS logs.

The snapshot intentionally parses only [MR_EXIT] for actual PnL. It also
computes management alternatives from the current 70/30 POC runner log fields:
- current70_30: current logged blended PnL
- poc100: all out at POC when Target1 was hit
- fullRunnerBE: full position held to final exit with current protected stop logic
"""

from __future__ import annotations

import argparse
import json
import os
import re
from collections import defaultdict
from dataclasses import dataclass, asdict
from datetime import datetime
from pathlib import Path

FIELD_RE = re.compile(r"([A-Za-z0-9_]+)=((?:Italy=)?[^,]+(?:,[0-9]+)?)")
DAY_RE = re.compile(r"day-(\d{4}-\d{2}-\d{2})\.log")


def dec(value: str | None) -> float:
    if value is None or value == "":
        return 0.0
    return float(value.strip().replace(",", ".").rstrip("R"))


def parse_fields(line: str) -> dict[str, str]:
    body = re.sub(r"^(?:\[[^\]]+\]\s*)+", "", line)
    return {match.group(1): match.group(2).strip() for match in FIELD_RE.finditer(body)}


@dataclass
class Bucket:
    exits: int = 0
    current70_30: float = 0.0
    poc100: float = 0.0
    full_runner_be: float = 0.0
    stop_hit: int = 0
    protected_stop_hit: int = 0
    target2_hit: int = 0
    london_close: int = 0
    target1_hits: int = 0

    def add(self, row: dict[str, str]) -> None:
        pnl = dec(row.get("PnL"))
        full_runner = dec(row.get("FullRunnerPnL"))
        realized_poc = dec(row.get("RealizedPocPnL"))
        target1_pct = dec(row.get("Target1ExitPct"))
        target1_hit = row.get("Target1Hit") == "True"
        reason = row.get("ExitReason", "UNKNOWN")

        poc100 = realized_poc / target1_pct if target1_hit and target1_pct > 0 else pnl
        full_runner_be = full_runner if target1_hit else pnl

        self.exits += 1
        self.current70_30 += pnl
        self.poc100 += poc100
        self.full_runner_be += full_runner_be
        self.target1_hits += int(target1_hit)
        self.stop_hit += int(reason == "STOP_HIT")
        self.protected_stop_hit += int(reason == "PROTECTED_STOP_HIT")
        self.target2_hit += int(reason == "TARGET2_HIT")
        self.london_close += int(reason == "LONDON_CLOSE")


def default_logs_dir() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs"


def read_reload_markers(log_file: Path) -> dict[str, str]:
    markers: dict[str, str] = {}
    if not log_file.exists():
        return markers

    for line in log_file.read_text(encoding="utf-8", errors="replace").splitlines():
        if "[CUM_TRADES_LOOKBACK]" in line:
            fields = parse_fields(line)
            markers["lookback"] = line.strip()
            markers["effectiveBeginItaly"] = fields.get("EffectiveBeginItaly", "")
            markers["endItaly"] = fields.get("EndItaly", "")
        elif "[HISTORICAL_FLOW_TRADES_READY]" in line:
            fields = parse_fields(line)
            markers["tradesReady"] = line.strip()
            markers["allTrades"] = fields.get("AllTrades", "")
            markers["aggressionTrades"] = fields.get("AggressionTrades", "")
        elif "[HISTORICAL_FLOW_FINISH]" in line:
            fields = parse_fields(line)
            markers["finish"] = line.strip()
            markers["openPositions"] = fields.get("OpenPositions", "")
            markers["closedPositions"] = fields.get("ClosedPositions", "")
            markers["positionRecords"] = fields.get("PositionRecords", "")
            markers["processDurationMs"] = fields.get("ProcessDurationMs", "")
            markers["totalFlowDurationMs"] = fields.get("TotalFlowDurationMs", "")
    return markers


def collect(day_logs: list[Path]) -> tuple[Bucket, dict[str, Bucket], dict[str, Bucket]]:
    total = Bucket()
    by_day: defaultdict[str, Bucket] = defaultdict(Bucket)
    by_reason: defaultdict[str, Bucket] = defaultdict(Bucket)

    for path in sorted(day_logs):
        day_match = DAY_RE.search(path.name)
        day = day_match.group(1) if day_match else path.stem
        for line in path.read_text(encoding="utf-8", errors="replace").splitlines():
            if "[MR_EXIT]" not in line:
                continue
            fields = parse_fields(line)
            total.add(fields)
            by_day[day].add(fields)
            by_reason[fields.get("ExitReason", "UNKNOWN")].add(fields)

    return total, dict(by_day), dict(by_reason)


def bucket_to_dict(bucket: Bucket) -> dict[str, float | int]:
    data = asdict(bucket)
    data["delta_poc100_vs_current70_30"] = bucket.poc100 - bucket.current70_30
    data["delta_full_runner_be_vs_current70_30"] = bucket.full_runner_be - bucket.current70_30
    data["target1_hit_rate"] = bucket.target1_hits / bucket.exits if bucket.exits else 0.0
    return data


def format_bucket(bucket: Bucket) -> str:
    return (
        f"exits={bucket.exits} current70_30={bucket.current70_30:.2f} "
        f"poc100={bucket.poc100:.2f} fullRunnerBE={bucket.full_runner_be:.2f} "
        f"deltaPoc={bucket.poc100 - bucket.current70_30:.2f}"
    )


def main() -> int:
    parser = argparse.ArgumentParser(description="Create FabioOrderFlow performance snapshot")
    parser.add_argument("--logs-dir", type=Path, default=default_logs_dir())
    parser.add_argument("--out-dir", type=Path, default=Path("FabioOrderFlow/performance-snapshots"))
    parser.add_argument("--pattern", default="FabioOrderFlow-day-*.log")
    args = parser.parse_args()

    day_dir = args.logs_dir / "FabioOrderFlow-days"
    day_logs = sorted(day_dir.glob(args.pattern))
    if not day_logs:
        raise SystemExit(f"No day logs found in {day_dir}")

    markers = read_reload_markers(args.logs_dir / "FabioOrderFlow.log")
    total, by_day, by_reason = collect(day_logs)

    created = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    snapshot = {
        "createdLocal": created,
        "logsDir": str(args.logs_dir),
        "reload": markers,
        "total": bucket_to_dict(total),
        "byDay": {key: bucket_to_dict(value) for key, value in sorted(by_day.items())},
        "byExitReason": {key: bucket_to_dict(value) for key, value in sorted(by_reason.items())},
        "notes": [
            "Actual PnL uses only [MR_EXIT].",
            "poc100 is an alternative management simulation from current log fields, not a separate replay.",
            "fullRunnerBE uses the current final exit with protected stop semantics when Target1Hit=True.",
        ],
    }

    args.out_dir.mkdir(parents=True, exist_ok=True)
    json_path = args.out_dir / f"performance-{created}.json"
    txt_path = args.out_dir / f"performance-{created}.txt"

    json_path.write_text(json.dumps(snapshot, indent=2, ensure_ascii=False), encoding="utf-8")

    lines = [
        f"FabioOrderFlow performance snapshot {created}",
        f"Logs: {args.logs_dir}",
        "",
        "Reload:",
        f"  EffectiveBeginItaly={markers.get('effectiveBeginItaly', 'NA')}",
        f"  EndItaly={markers.get('endItaly', 'NA')}",
        f"  AllTrades={markers.get('allTrades', 'NA')}",
        f"  AggressionTrades={markers.get('aggressionTrades', 'NA')}",
        f"  OpenPositions={markers.get('openPositions', 'NA')}",
        f"  ClosedPositions={markers.get('closedPositions', 'NA')}",
        f"  PositionRecords={markers.get('positionRecords', 'NA')}",
        "",
        "Total:",
        f"  {format_bucket(total)}",
        "",
        "By day:",
    ]
    lines.extend(f"  {day}: {format_bucket(bucket)}" for day, bucket in sorted(by_day.items()))
    lines.append("")
    lines.append("By exit reason:")
    lines.extend(f"  {reason}: {format_bucket(bucket)}" for reason, bucket in sorted(by_reason.items()))
    lines.append("")
    lines.append("Interpretation:")
    lines.append("  Positive deltaPoc means full exit at POC beat current 70/30 on this log set.")
    lines.append("  Use this as Fabio-style management evidence, not as final production proof.")

    txt_path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    print(txt_path)
    print(json_path)
    print(format_bucket(total))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
