#!/usr/bin/env python3
"""Study Fabio-style POC management from complete FabioOrderFlow day logs.

This is a research tool, not production logic. It reads full daily logs and
compares:
- current70_30: current logged 70% POC + 30% runner management
- poc100: all out at POC when Target1 was hit
- conditional runner rules: all out at POC unless causal post-POC evidence appears

The causal evidence is computed only from DAY_STUDY_BIG_TRADE and DAY_STUDY_BAR
records after Target1/POC time and before the final exit or the selected window.
"""

from __future__ import annotations

import argparse
import json
import os
import re
import shutil
from collections import defaultdict
from dataclasses import asdict, dataclass
from datetime import datetime, timedelta
from pathlib import Path
from typing import Callable

DAY_RE = re.compile(r"day-(\d{4}-\d{2}-\d{2})\.log")
DECIMAL_KEYS = {
    "Entry",
    "Exit",
    "Target1POC",
    "Target2",
    "Current70_30PnL",
    "PocAllOutPnL",
    "FullRunnerProtectedPnL",
    "DeltaPocAllOutVsCurrent",
    "DeltaFullRunnerVsCurrent",
    "Volume",
    "FirstPrice",
    "LastPrice",
    "Open",
    "High",
    "Low",
    "Close",
    "Delta",
}


def default_logs_dir() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs"


def strip_prefix(line: str, tag: str) -> str:
    marker = f"[{tag}]"
    if marker not in line:
        return line
    return line.split(marker, 1)[1].strip()


def field(body: str, key: str) -> str:
    match = re.search(rf"\b{re.escape(key)}=([^,]*(?:,[0-9]+)?)(?=, [A-Za-z0-9_]+=|$)", body)
    return match.group(1).strip() if match else ""


def dec(value: str) -> float:
    if not value:
        return 0.0
    return float(value.replace("Italy=", "").replace(",", ".").rstrip("R"))


def parse_dt(value: str) -> datetime | None:
    if not value or value == "NA":
        return None
    value = value.replace("Italy=", "").strip()
    try:
        return datetime.strptime(value, "%Y-%m-%d %H:%M:%S")
    except ValueError:
        return None


@dataclass
class BigTrade:
    time: datetime
    direction: str
    volume: float
    first_price: float
    last_price: float

    @property
    def high(self) -> float:
        return max(self.first_price, self.last_price)

    @property
    def low(self) -> float:
        return min(self.first_price, self.last_price)


@dataclass
class Bar:
    time: datetime
    open: float
    high: float
    low: float
    close: float
    delta: float


@dataclass
class PocTrade:
    day: str
    setup_id: str
    direction: str
    exit_reason: str
    event_time: datetime
    target1_hit: bool
    target1_time: datetime | None
    entry: float
    target1_poc: float
    target2: float
    current70_30: float
    poc100: float
    full_runner_be: float


@dataclass
class Features:
    window_seconds: int
    same_count: int = 0
    same_volume: float = 0.0
    max_same_volume: float = 0.0
    opposite_count: int = 0
    opposite_volume: float = 0.0
    max_opposite_volume: float = 0.0
    close_beyond_poc_count: int = 0
    close_against_poc_count: int = 0
    signed_bar_delta: float = 0.0
    extension_pct: float = 0.0
    seconds_available: float = 0.0

    @property
    def pressure_ratio(self) -> float:
        return self.same_volume / self.opposite_volume if self.opposite_volume > 0 else (999.0 if self.same_volume > 0 else 0.0)


@dataclass
class RuleResult:
    name: str
    window_seconds: int
    trades: int = 0
    runner_kept: int = 0
    pnl: float = 0.0
    delta_vs_current: float = 0.0
    delta_vs_poc100: float = 0.0


def parse_day_log(path: Path) -> tuple[str, list[PocTrade], list[BigTrade], list[Bar]]:
    day_match = DAY_RE.search(path.name)
    day = day_match.group(1) if day_match else path.stem
    trades: list[PocTrade] = []
    big_trades: list[BigTrade] = []
    bars: list[Bar] = []

    for line in path.read_text(encoding="utf-8", errors="replace").splitlines():
        if "[DAY_STUDY_POC_MANAGEMENT]" in line:
            body = strip_prefix(line, "DAY_STUDY_POC_MANAGEMENT")
            event_time = parse_dt(field(body, "Italy"))
            if event_time is None:
                continue
            trades.append(
                PocTrade(
                    day=day,
                    setup_id=field(body, "SetupId"),
                    direction=field(body, "Direction"),
                    exit_reason=field(body, "ExitReason"),
                    event_time=event_time,
                    target1_hit=field(body, "Target1Hit") == "True",
                    target1_time=parse_dt(field(body, "Target1Time")),
                    entry=dec(field(body, "Entry")),
                    target1_poc=dec(field(body, "Target1POC")),
                    target2=dec(field(body, "Target2")),
                    current70_30=dec(field(body, "Current70_30PnL")),
                    poc100=dec(field(body, "PocAllOutPnL")),
                    full_runner_be=dec(field(body, "FullRunnerProtectedPnL")),
                )
            )
        elif "[DAY_STUDY_BIG_TRADE]" in line:
            body = strip_prefix(line, "DAY_STUDY_BIG_TRADE")
            event_time = parse_dt(field(body, "Italy"))
            if event_time is None:
                continue
            big_trades.append(
                BigTrade(
                    time=event_time,
                    direction=field(body, "Direction"),
                    volume=dec(field(body, "Volume")),
                    first_price=dec(field(body, "FirstPrice")),
                    last_price=dec(field(body, "LastPrice")),
                )
            )
        elif "[DAY_STUDY_BAR]" in line:
            body = strip_prefix(line, "DAY_STUDY_BAR")
            event_time = parse_dt(field(body, "Italy"))
            if event_time is None:
                continue
            bars.append(
                Bar(
                    time=event_time,
                    open=dec(field(body, "Open")),
                    high=dec(field(body, "High")),
                    low=dec(field(body, "Low")),
                    close=dec(field(body, "Close")),
                    delta=dec(field(body, "Delta")),
                )
            )

    return day, trades, big_trades, bars


def read_reload_bounds(log_file: Path) -> tuple[datetime | None, datetime | None]:
    begin = None
    end = None
    if not log_file.exists():
        return begin, end
    for line in log_file.read_text(encoding="utf-8", errors="replace").splitlines():
        if "[CUM_TRADES_LOOKBACK]" not in line:
            continue
        body = strip_prefix(line, "CUM_TRADES_LOOKBACK")
        begin = parse_dt(field(body, "EffectiveBeginItaly"))
        end = parse_dt(field(body, "EndItaly"))
    return begin, end


def is_complete_day(day: str, begin: datetime | None, end: datetime | None) -> bool:
    date = datetime.strptime(day, "%Y-%m-%d").date()
    if begin and date <= begin.date():
        return False
    if end and date >= end.date():
        return False
    return True


def features_for(trade: PocTrade, big_trades: list[BigTrade], bars: list[Bar], window_seconds: int) -> Features:
    features = Features(window_seconds=window_seconds)
    if not trade.target1_hit or trade.target1_time is None:
        return features

    cutoff = min(trade.event_time, trade.target1_time + timedelta(seconds=window_seconds))
    features.seconds_available = max(0.0, (cutoff - trade.target1_time).total_seconds())
    if features.seconds_available <= 0:
        return features

    same_direction = "Buy" if trade.direction == "Long" else "Sell"
    opposite_direction = "Sell" if trade.direction == "Long" else "Buy"
    selected_trades = [t for t in big_trades if trade.target1_time < t.time <= cutoff]
    same = [t for t in selected_trades if t.direction == same_direction]
    opposite = [t for t in selected_trades if t.direction == opposite_direction]
    features.same_count = len(same)
    features.same_volume = sum(t.volume for t in same)
    features.max_same_volume = max((t.volume for t in same), default=0.0)
    features.opposite_count = len(opposite)
    features.opposite_volume = sum(t.volume for t in opposite)
    features.max_opposite_volume = max((t.volume for t in opposite), default=0.0)

    selected_bars = [b for b in bars if trade.target1_time < b.time <= cutoff]
    target_span = abs(trade.target2 - trade.target1_poc)
    max_extension = 0.0
    for bar in selected_bars:
        if trade.direction == "Long":
            if bar.close > trade.target1_poc:
                features.close_beyond_poc_count += 1
            if bar.close < trade.target1_poc:
                features.close_against_poc_count += 1
            features.signed_bar_delta += bar.delta
            max_extension = max(max_extension, bar.high - trade.target1_poc)
        else:
            if bar.close < trade.target1_poc:
                features.close_beyond_poc_count += 1
            if bar.close > trade.target1_poc:
                features.close_against_poc_count += 1
            features.signed_bar_delta += -bar.delta
            max_extension = max(max_extension, trade.target1_poc - bar.low)

    for big in selected_trades:
        if trade.direction == "Long":
            max_extension = max(max_extension, big.high - trade.target1_poc)
        else:
            max_extension = max(max_extension, trade.target1_poc - big.low)

    features.extension_pct = max_extension / target_span if target_span > 0 else 0.0
    return features


def rule_pressure(features: Features) -> bool:
    return features.same_count > 0 and features.same_volume > features.opposite_volume and features.max_same_volume >= features.max_opposite_volume


def rule_dominant_pressure(features: Features) -> bool:
    return features.same_count > 0 and features.pressure_ratio >= 1.25 and features.max_same_volume >= 1.25 * features.max_opposite_volume


def rule_acceptance(features: Features) -> bool:
    return features.close_beyond_poc_count >= 1 and features.signed_bar_delta > 0


def rule_extension25(features: Features) -> bool:
    return features.extension_pct >= 0.25


def rule_pressure_acceptance(features: Features) -> bool:
    return rule_pressure(features) and rule_acceptance(features)


def rule_pressure_extension25(features: Features) -> bool:
    return rule_pressure(features) and rule_extension25(features)


def rule_fabio_candidate(features: Features) -> bool:
    return rule_pressure(features) and features.extension_pct >= 0.15 and features.close_against_poc_count == 0


RULES: list[tuple[str, Callable[[Features], bool]]] = [
    ("PRESSURE", rule_pressure),
    ("DOMINANT_PRESSURE", rule_dominant_pressure),
    ("ACCEPTANCE", rule_acceptance),
    ("EXTENSION25", rule_extension25),
    ("PRESSURE_ACCEPTANCE", rule_pressure_acceptance),
    ("PRESSURE_EXTENSION25", rule_pressure_extension25),
    ("FABIO_CANDIDATE", rule_fabio_candidate),
]


def summarize_baselines(trades: list[PocTrade]) -> dict[str, float | int]:
    return {
        "trades": len(trades),
        "current70_30": sum(t.current70_30 for t in trades),
        "poc100": sum(t.poc100 for t in trades),
        "fullRunnerBE": sum(t.full_runner_be for t in trades),
        "target1Hits": sum(1 for t in trades if t.target1_hit),
    }


def evaluate_rules(
    trades: list[PocTrade],
    day_big_trades: dict[str, list[BigTrade]],
    day_bars: dict[str, list[Bar]],
    windows: list[int],
) -> list[RuleResult]:
    results: list[RuleResult] = []
    current_total = sum(t.current70_30 for t in trades)
    poc_total = sum(t.poc100 for t in trades)

    for window in windows:
        feature_cache = {
            t.setup_id: features_for(t, day_big_trades[t.day], day_bars[t.day], window)
            for t in trades
        }
        for rule_name, rule in RULES:
            result = RuleResult(name=rule_name, window_seconds=window, trades=len(trades))
            for trade in trades:
                keep_runner = trade.target1_hit and rule(feature_cache[trade.setup_id])
                result.runner_kept += int(keep_runner)
                result.pnl += trade.current70_30 if keep_runner else trade.poc100
            result.delta_vs_current = result.pnl - current_total
            result.delta_vs_poc100 = result.pnl - poc_total
            results.append(result)

    return sorted(results, key=lambda r: r.pnl, reverse=True)


def section_lines(title: str, trades: list[PocTrade], results: list[RuleResult]) -> list[str]:
    base = summarize_baselines(trades)
    lines = [
        title,
        "=" * len(title),
        (
            f"baselines trades={base['trades']} target1Hits={base['target1Hits']} "
            f"current70_30={base['current70_30']:.2f} poc100={base['poc100']:.2f} "
            f"fullRunnerBE={base['fullRunnerBE']:.2f}"
        ),
        "top conditional runner rules",
    ]
    for result in results[:10]:
        lines.append(
            f"{result.name:22s} window={result.window_seconds:4d}s "
            f"kept={result.runner_kept:2d}/{result.trades:<2d} pnl={result.pnl:8.2f} "
            f"vsCurrent={result.delta_vs_current:7.2f} vsPoc={result.delta_vs_poc100:7.2f}"
        )
    lines.append("")
    return lines


def print_section(title: str, trades: list[PocTrade], results: list[RuleResult]) -> None:
    print("\n".join(section_lines(title, trades, results)))


def main() -> int:
    parser = argparse.ArgumentParser(description="Study post-POC runner rules from FabioOrderFlow day logs")
    parser.add_argument("--logs-dir", type=Path, default=default_logs_dir())
    parser.add_argument("--pattern", default="FabioOrderFlow-day-*.log")
    parser.add_argument("--out-dir", type=Path, default=Path("FabioOrderFlow/performance-snapshots"))
    parser.add_argument("--windows", default="60,120,300,600,900,1800")
    parser.add_argument("--archive-logs", action="store_true", help="Copy source day logs and FabioOrderFlow.log into the snapshot directory")
    args = parser.parse_args()

    day_dir = args.logs_dir / "FabioOrderFlow-days"
    begin, end = read_reload_bounds(args.logs_dir / "FabioOrderFlow.log")
    windows = [int(value) for value in args.windows.split(",") if value.strip()]

    all_trades: list[PocTrade] = []
    complete_trades: list[PocTrade] = []
    day_big_trades: dict[str, list[BigTrade]] = defaultdict(list)
    day_bars: dict[str, list[Bar]] = defaultdict(list)
    source_logs: list[Path] = []

    for path in sorted(day_dir.glob(args.pattern)):
        day, trades, big_trades, bars = parse_day_log(path)
        if not trades:
            continue
        source_logs.append(path)
        day_big_trades[day] = big_trades
        day_bars[day] = bars
        all_trades.extend(trades)
        if is_complete_day(day, begin, end):
            complete_trades.extend(trades)

    all_results = evaluate_rules(all_trades, day_big_trades, day_bars, windows)
    complete_results = evaluate_rules(complete_trades, day_big_trades, day_bars, windows)

    created = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    args.out_dir.mkdir(parents=True, exist_ok=True)
    json_path = args.out_dir / f"poc-management-study-{created}.json"
    txt_path = args.out_dir / f"poc-management-study-{created}.txt"
    payload = {
        "createdLocal": created,
        "reloadBeginItaly": begin.strftime("%Y-%m-%d %H:%M:%S") if begin else None,
        "reloadEndItaly": end.strftime("%Y-%m-%d %H:%M:%S") if end else None,
        "windows": windows,
        "sourceLogs": [str(path) for path in source_logs],
        "allReload": {
            "baseline": summarize_baselines(all_trades),
            "rules": [asdict(r) for r in all_results],
        },
        "completeDaysOnly": {
            "baseline": summarize_baselines(complete_trades),
            "rules": [asdict(r) for r in complete_results],
        },
        "notes": [
            "A conditional rule keeps the current 30% runner only when the rule is true after POC; otherwise it exits 100% at POC.",
            "Rules are causal within the selected post-POC window, but still require more samples before production use.",
        ],
    }
    json_path.write_text(json.dumps(payload, indent=2, ensure_ascii=False), encoding="utf-8")

    lines: list[str] = []
    lines.extend(section_lines("All reload trades", all_trades, all_results))
    lines.extend(section_lines("Complete days only", complete_trades, complete_results))
    lines.append(f"JSON: {json_path}")

    if args.archive_logs:
        archive_dir = args.out_dir / f"poc-management-study-{created}-logs"
        archive_dir.mkdir(parents=True, exist_ok=True)
        for path in source_logs:
            shutil.copy2(path, archive_dir / path.name)
        general_log = args.logs_dir / "FabioOrderFlow.log"
        if general_log.exists():
            shutil.copy2(general_log, archive_dir / general_log.name)
        lines.append(f"Archived logs: {archive_dir}")

    txt_path.write_text("\n".join(lines) + "\n", encoding="utf-8")

    print("\n".join(lines))
    print(txt_path)
    print(json_path)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
