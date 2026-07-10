#!/usr/bin/env python3
"""Canonical performance report for the active LondonMeanReversionModel.

Only current MR markers are parsed. Realized PnL comes exclusively from
[MR_EXIT]. The report stays non-promotional until the sample and cost inputs
are sufficient.
"""

from __future__ import annotations

import argparse
import json
import os
import re
from collections import defaultdict
from dataclasses import dataclass
from datetime import datetime
from decimal import Decimal
from pathlib import Path
from typing import Iterable

MARKERS = (
    "CUM_TRADES_LOOKBACK",
    "HISTORICAL_FLOW_FINISH",
    "MR_ENTRY",
    "MR_EXIT",
    "MR_REPLAY_OPEN",
)


def default_log() -> Path:
    appdata = os.environ.get("APPDATA")
    if appdata:
        return Path(appdata) / "ATAS" / "Logs" / "FabioOrderFlow.log"
    return Path.home() / "AppData" / "Roaming" / "ATAS" / "Logs" / "FabioOrderFlow.log"


def event_body(line: str, marker: str) -> str:
    token = f"[{marker}]"
    return line.split(token, 1)[1].strip() if token in line else ""


def field(body: str, key: str) -> str:
    match = re.search(rf"(?:^|, ){re.escape(key)}=(.*?)(?=, [A-Za-z][A-Za-z0-9_]*=|$)", body)
    return match.group(1).strip() if match else ""


def number(value: str) -> Decimal:
    cleaned = value.strip().rstrip("R")
    if not cleaned:
        return Decimal("0")
    if "," in cleaned:
        cleaned = cleaned.replace(".", "").replace(",", ".")
    return Decimal(cleaned)


def parse_time(value: str) -> datetime:
    value = value.replace("Italy=", "").strip()
    for fmt in ("%Y-%m-%d %H:%M:%S.%f", "%Y-%m-%d %H:%M:%S"):
        try:
            return datetime.strptime(value, fmt)
        except ValueError:
            pass
    return datetime.min


def marker_for(line: str) -> str:
    return next((marker for marker in MARKERS if f"[{marker}]" in line), "")


@dataclass(frozen=True)
class Entry:
    setup_id: str
    time: str
    execution_mode: str
    source: str
    reference_label: str
    direction: str
    entry_price: Decimal
    risk: Decimal


@dataclass(frozen=True)
class Exit:
    setup_id: str
    time: str
    execution_mode: str
    direction: str
    reason: str
    pnl: Decimal
    r_multiple: Decimal


@dataclass
class Bucket:
    trades: int = 0
    wins: int = 0
    breakeven: int = 0
    losses: int = 0
    gross_profit_points: Decimal = Decimal("0")
    gross_loss_points: Decimal = Decimal("0")
    pnl_points: Decimal = Decimal("0")
    r_total: Decimal = Decimal("0")

    def add(self, exit_record: Exit) -> None:
        self.trades += 1
        self.pnl_points += exit_record.pnl
        self.r_total += exit_record.r_multiple
        if exit_record.pnl > 0:
            self.wins += 1
            self.gross_profit_points += exit_record.pnl
        elif exit_record.pnl < 0:
            self.losses += 1
            self.gross_loss_points += abs(exit_record.pnl)
        else:
            self.breakeven += 1

    def result(self) -> dict[str, object]:
        profit_factor = self.gross_profit_points / self.gross_loss_points if self.gross_loss_points else None
        return {
            "trades": self.trades,
            "wins": self.wins,
            "breakeven": self.breakeven,
            "losses": self.losses,
            "winRate": decimal_or_none(Decimal(self.wins) / self.trades if self.trades else None),
            "grossProfitPoints": decimal_or_none(self.gross_profit_points),
            "grossLossPoints": decimal_or_none(self.gross_loss_points),
            "pnlPoints": decimal_or_none(self.pnl_points),
            "profitFactor": decimal_or_none(profit_factor),
            "averagePoints": decimal_or_none(self.pnl_points / self.trades if self.trades else None),
            "totalR": decimal_or_none(self.r_total),
            "averageR": decimal_or_none(self.r_total / self.trades if self.trades else None),
        }


def decimal_or_none(value: Decimal | None) -> float | None:
    return float(value) if value is not None else None


def read_records(paths: Iterable[Path]) -> tuple[dict[str, Entry], dict[str, Exit], list[str], list[str], dict[str, str]]:
    entries: dict[str, Entry] = {}
    exits: dict[str, Exit] = {}
    replay_open: list[str] = []
    warnings: list[str] = []
    coverage: dict[str, str] = {}

    for path in paths:
        if not path.exists():
            warnings.append(f"Missing log: {path}")
            continue
        for line in path.read_text(encoding="utf-8", errors="replace").splitlines():
            marker = marker_for(line)
            if not marker:
                continue
            body = event_body(line, marker)
            if marker == "CUM_TRADES_LOOKBACK":
                coverage["effectiveBeginItaly"] = field(body, "EffectiveBeginItaly")
                coverage["endItaly"] = field(body, "EndItaly")
            elif marker == "HISTORICAL_FLOW_FINISH":
                coverage["historicalEntries"] = field(body, "Entries")
                coverage["historicalClosedPositions"] = field(body, "ClosedPositions")
                coverage["historicalOpenPositions"] = field(body, "OpenPositions")
                coverage["historicalCompletedTrades"] = field(body, "CompletedTrades")
            elif marker == "MR_ENTRY":
                setup_id = field(body, "SetupId")
                if setup_id:
                    entries[setup_id] = Entry(
                        setup_id=setup_id,
                        time=field(body, "Italy"),
                        execution_mode=field(body, "ExecutionMode"),
                        source=field(body, "Source") or "UNKNOWN",
                        reference_label=field(body, "ReferenceLabel"),
                        direction=field(body, "Direction"),
                        entry_price=number(field(body, "EntryPrice")),
                        risk=number(field(body, "Risk")),
                    )
            elif marker == "MR_EXIT":
                setup_id = field(body, "SetupId")
                if setup_id:
                    exits[setup_id] = Exit(
                        setup_id=setup_id,
                        time=field(body, "Italy"),
                        execution_mode=field(body, "ExecutionMode"),
                        direction=field(body, "Direction"),
                        reason=field(body, "ExitReason") or "UNKNOWN",
                        pnl=number(field(body, "PnL")),
                        r_multiple=number(field(body, "RMultiple")),
                    )
            elif marker == "MR_REPLAY_OPEN":
                replay_open.append(field(body, "SetupId") or line.strip())

    return entries, exits, replay_open, warnings, coverage


def max_drawdown(records: list[Exit], selector) -> Decimal:
    equity = Decimal("0")
    peak = Decimal("0")
    drawdown = Decimal("0")
    for record in records:
        equity += selector(record)
        peak = max(peak, equity)
        drawdown = max(drawdown, peak - equity)
    return drawdown


def max_consecutive_losses(records: list[Exit]) -> int:
    maximum = 0
    current = 0
    for record in records:
        current = current + 1 if record.pnl < 0 else 0
        maximum = max(maximum, current)
    return maximum


def grouped(records: list[Exit], entries: dict[str, Entry], key_selector) -> dict[str, dict[str, object]]:
    buckets: defaultdict[str, Bucket] = defaultdict(Bucket)
    for record in records:
        buckets[key_selector(record, entries.get(record.setup_id))].add(record)
    return {key: bucket.result() for key, bucket in sorted(buckets.items())}


def main() -> int:
    parser = argparse.ArgumentParser(description="Report active London MR performance from current MR markers")
    parser.add_argument("--log", type=Path, action="append", help="Log file; repeat for multiple non-overlapping files")
    parser.add_argument("--save", action="store_true", help="Write JSON and text snapshots")
    parser.add_argument("--execution-mode", choices=("HISTORICAL", "LIVE", "ALL"), default="HISTORICAL", help="Avoid mixing overlapping replay and live trades")
    parser.add_argument("--out-dir", type=Path, default=Path("FabioOrderFlow/performance-snapshots"))
    parser.add_argument("--commission-round-turn", type=Decimal, help="Commission per round turn in account currency")
    parser.add_argument("--slippage-points", type=Decimal, help="Total round-turn slippage in instrument points")
    parser.add_argument("--point-value", type=Decimal, help="Account currency value of one instrument point")
    parser.add_argument("--minimum-trades", type=int, default=30)
    parser.add_argument("--minimum-sessions", type=int, default=10)
    args = parser.parse_args()

    paths = args.log or [default_log()]
    entries, exits_by_id, replay_open, warnings, coverage = read_records(paths)
    all_records = sorted(exits_by_id.values(), key=lambda item: parse_time(item.time))
    records = [record for record in all_records if args.execution_mode == "ALL" or record.execution_mode == args.execution_mode]
    selected_entries = {
        setup_id: entry
        for setup_id, entry in entries.items()
        if args.execution_mode == "ALL" or entry.execution_mode == args.execution_mode
    }
    selected_replay_open = replay_open if args.execution_mode in ("HISTORICAL", "ALL") else []

    total = Bucket()
    for record in records:
        total.add(record)
    total_result = total.result()

    days = {record.time[:10] for record in records if record.time}
    missing_entry_ids = sorted({record.setup_id for record in records} - set(selected_entries))
    if missing_entry_ids:
        warnings.append(f"{len(missing_entry_ids)} exits have no matching MR_ENTRY; source breakdown may be UNKNOWN")

    costs_complete = args.commission_round_turn is not None and args.slippage_points is not None and args.point_value is not None and args.point_value > 0
    cost_points_per_trade: Decimal | None = None
    net_after_costs: Decimal | None = None
    if costs_complete:
        cost_points_per_trade = args.slippage_points + args.commission_round_turn / args.point_value
        net_after_costs = total.pnl_points - cost_points_per_trade * total.trades

    profit_factor = total.gross_profit_points / total.gross_loss_points if total.gross_loss_points else None
    average_r = total.r_total / total.trades if total.trades else None
    sample_complete = total.trades >= args.minimum_trades and len(days) >= args.minimum_sessions
    if not sample_complete:
        gate_status = "INSUFFICIENT_SAMPLE"
    elif not costs_complete:
        gate_status = "MISSING_COST_ASSUMPTIONS"
    elif net_after_costs is not None and net_after_costs > 0 and average_r is not None and average_r > 0 and profit_factor is not None and profit_factor >= Decimal("1.25"):
        gate_status = "CANDIDATE_ACCEPTABLE_REQUIRES_OUT_OF_SAMPLE_REVIEW"
    else:
        gate_status = "NOT_ACCEPTABLE"

    report = {
        "createdLocal": datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
        "executionMode": args.execution_mode,
        "logs": [str(path) for path in paths],
        "coverage": coverage,
        "totals": total_result,
        "risk": {
            "maxDrawdownPoints": decimal_or_none(max_drawdown(records, lambda row: row.pnl)),
            "maxDrawdownR": decimal_or_none(max_drawdown(records, lambda row: row.r_multiple)),
            "maxConsecutiveLosses": max_consecutive_losses(records),
        },
        "costs": {
            "complete": costs_complete,
            "commissionRoundTurn": decimal_or_none(args.commission_round_turn),
            "slippagePoints": decimal_or_none(args.slippage_points),
            "pointValue": decimal_or_none(args.point_value),
            "costPointsPerTrade": decimal_or_none(cost_points_per_trade),
            "netPointsAfterCosts": decimal_or_none(net_after_costs),
        },
        "sample": {
            "sessions": len(days),
            "minimumTrades": args.minimum_trades,
            "minimumSessions": args.minimum_sessions,
            "complete": sample_complete,
        },
        "gate": {
            "status": gate_status,
            "requirements": "positive net points after costs, positive average R, profit factor >= 1.25, sample threshold met",
        },
        "integrity": {
            "entries": len(selected_entries),
            "exits": len(records),
            "replayOpen": len(selected_replay_open),
            "missingEntryForExit": len(missing_entry_ids),
            "warnings": warnings,
        },
        "byDay": grouped(records, selected_entries, lambda row, _entry: row.time[:10] or "UNKNOWN"),
        "byReferenceSource": grouped(records, selected_entries, lambda _row, entry: entry.source if entry else "UNKNOWN"),
        "byDirection": grouped(records, selected_entries, lambda row, _entry: row.direction or "UNKNOWN"),
        "byExitReason": grouped(records, selected_entries, lambda row, _entry: row.reason),
        "byExecutionMode": grouped(records, selected_entries, lambda row, _entry: row.execution_mode or "UNKNOWN"),
    }

    print(f"MR performance: mode={args.execution_mode} trades={total.trades} sessions={len(days)} pnl={total.pnl_points:.2f} points")
    print(f"profitFactor={profit_factor:.2f}" if profit_factor is not None else "profitFactor=NA")
    average_r_text = f"{average_r:.2f}" if average_r is not None else "NA"
    print(f"averageR={average_r_text} maxDrawdown={report['risk']['maxDrawdownPoints']:.2f} points")
    print(f"gate={gate_status} costsComplete={costs_complete} replayOpen={len(selected_replay_open)}")
    for warning in warnings:
        print(f"warning={warning}")

    if args.save:
        args.out_dir.mkdir(parents=True, exist_ok=True)
        stamp = datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
        mode_name = args.execution_mode.lower()
        json_path = args.out_dir / f"mr-performance-{mode_name}-{stamp}.json"
        text_path = args.out_dir / f"mr-performance-{mode_name}-{stamp}.txt"
        json_path.write_text(json.dumps(report, indent=2, ensure_ascii=True), encoding="utf-8")
        text_path.write_text(
            "\n".join(
                [
                    f"London MR performance {report['createdLocal']}",
                    f"Execution mode: {args.execution_mode}",
                    f"Coverage: {coverage.get('effectiveBeginItaly', 'NA')} -> {coverage.get('endItaly', 'NA')}",
                    f"Trades: {total.trades}; sessions: {len(days)}; replay open: {len(selected_replay_open)}",
                    f"PnL: {total.pnl_points:.2f} points; profit factor: {profit_factor if profit_factor is not None else 'NA'}; average R: {average_r if average_r is not None else 'NA'}",
                    f"Max drawdown: {report['risk']['maxDrawdownPoints']} points / {report['risk']['maxDrawdownR']}R",
                    f"Costs complete: {costs_complete}; net after costs: {net_after_costs if net_after_costs is not None else 'NA'}",
                    f"Gate: {gate_status}",
                    "PnL source: [MR_EXIT] only.",
                ]
            )
            + "\n",
            encoding="utf-8",
        )
        print(text_path)
        print(json_path)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
