#!/usr/bin/env python3
"""Parse FabioOrderFlow day-study logs.

Usage:
  python FabioOrderFlow/tools/parse_day_study.py \
    %APPDATA%/ATAS/Logs/FabioOrderFlow-study-historical.log
"""

from __future__ import annotations

import argparse
import re
from collections import defaultdict
from dataclasses import dataclass
from pathlib import Path


FIELD_RE = re.compile(r"([A-Za-z0-9_]+)=((?:Italy=)?[^,]+(?:,[0-9]+)?)")


def dec(value: str) -> float:
    return float(value.strip().replace(",", "."))


def parse_fields(line: str) -> dict[str, str]:
    return {match.group(1): match.group(2).strip() for match in FIELD_RE.finditer(line)}


@dataclass
class Stats:
    count: int = 0
    wins_poc: int = 0
    wins_t2: int = 0
    pnl_poc: float = 0.0
    pnl_t2: float = 0.0
    low_volume: int = 0
    rr05_count: int = 0
    rr05_wins_t2: int = 0
    rr05_pnl_t2: float = 0.0
    rr1_count: int = 0
    rr1_wins_t2: int = 0
    rr1_pnl_t2: float = 0.0

    def add(self, row: dict[str, str]) -> None:
        pnl_poc = dec(row.get("PnLPOC", "0"))
        pnl_t2 = dec(row.get("PnLT2", "0"))
        rr_t2 = dec(row.get("RR_T2", "0"))
        volume = dec(row.get("Volume", "0"))

        self.count += 1
        self.pnl_poc += pnl_poc
        self.pnl_t2 += pnl_t2
        self.wins_poc += pnl_poc > 0
        self.wins_t2 += pnl_t2 > 0
        self.low_volume += volume < 20

        if rr_t2 >= 0.5:
            self.rr05_count += 1
            self.rr05_pnl_t2 += pnl_t2
            self.rr05_wins_t2 += pnl_t2 > 0

        if rr_t2 >= 1.0:
            self.rr1_count += 1
            self.rr1_pnl_t2 += pnl_t2
            self.rr1_wins_t2 += pnl_t2 > 0


def get_italy_time(line: str) -> str:
    match = re.search(r"EntryTime=Italy=([0-9-]+ [0-9:]+)", line)
    return match.group(1) if match else ""


def time_bucket(italy_time: str, minutes: int = 15) -> str:
    if not italy_time:
        return "UNKNOWN"
    date, clock = italy_time.split(" ")
    hour, minute, _second = (int(part) for part in clock.split(":"))
    return f"{date} {hour:02d}:{(minute // minutes) * minutes:02d}"


def print_stats(title: str, stats_by_key: dict[str, Stats]) -> None:
    print(f"\n{title}")
    print("key\tn\tlowVol\tpocW\tpocPnL\tt2W\tt2PnL\tnRR05\tt2WRR05\tt2PnLRR05\tnRR1\tt2WRR1\tt2PnLRR1")
    for key in sorted(stats_by_key):
        stats = stats_by_key[key]
        print(
            f"{key}\t{stats.count}\t{stats.low_volume}\t{stats.wins_poc}\t{stats.pnl_poc:.2f}"
            f"\t{stats.wins_t2}\t{stats.pnl_t2:.2f}"
            f"\t{stats.rr05_count}\t{stats.rr05_wins_t2}\t{stats.rr05_pnl_t2:.2f}"
            f"\t{stats.rr1_count}\t{stats.rr1_wins_t2}\t{stats.rr1_pnl_t2:.2f}"
        )


def main() -> int:
    parser = argparse.ArgumentParser(description="Parse FabioOrderFlow day-study logs")
    parser.add_argument("log", type=Path, help="Path to FabioOrderFlow-study-YYYY-MM-DD.log")
    parser.add_argument("--top", type=int, default=15, help="Top candidates by PnLT2")
    args = parser.parse_args()

    if not args.log.exists():
        raise SystemExit(f"Log not found: {args.log}")

    counts: defaultdict[str, int] = defaultdict(int)
    by_type: defaultdict[str, Stats] = defaultdict(Stats)
    by_trigger: defaultdict[str, Stats] = defaultdict(Stats)
    by_bucket: defaultdict[str, Stats] = defaultdict(Stats)
    candidates: list[tuple[float, dict[str, str], str]] = []

    with args.log.open("r", encoding="utf-8", errors="replace") as handle:
        for line in handle:
            tag_match = re.search(r"\[([^\]]+)\]", line)
            if not tag_match:
                continue

            tag = tag_match.group(1)
            counts[tag] += 1

            if tag != "DAY_STUDY_CANDIDATE_ENTRY":
                continue

            fields = parse_fields(line)
            candidate_type = fields.get("CandidateType", "UNKNOWN")
            trigger = fields.get("Trigger", "UNKNOWN")
            bucket = time_bucket(get_italy_time(line))

            by_type[candidate_type].add(fields)
            by_trigger[trigger].add(fields)
            by_bucket[f"{bucket} {candidate_type}"].add(fields)
            candidates.append((dec(fields.get("PnLT2", "0")), fields, line.strip()))

    print("Counts")
    for tag in sorted(counts):
        print(f"{tag}\t{counts[tag]}")

    print_stats("By Candidate Type", by_type)
    print_stats("By Trigger", by_trigger)
    print_stats("By 15m Bucket", by_bucket)

    print(f"\nTop {args.top} Candidates By PnLT2")
    print("PnLT2\tRR_T2\tType\tTrigger\tEntryTime\tEntryPrice\tVolume\tRisk\tOutcomeT2")
    for pnl_t2, fields, _line in sorted(candidates, key=lambda item: item[0], reverse=True)[: args.top]:
        print(
            f"{pnl_t2:.2f}\t{dec(fields.get('RR_T2', '0')):.2f}\t{fields.get('CandidateType', '')}"
            f"\t{fields.get('Trigger', '')}\t{fields.get('EntryTime', '')}\t{fields.get('EntryPrice', '')}"
            f"\t{fields.get('Volume', '')}\t{fields.get('Risk', '')}\t{fields.get('OutcomeT2', '')}"
        )

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
