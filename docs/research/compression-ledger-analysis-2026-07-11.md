# Compression Ledger Analysis - 2026-07-11

## Scope

```text
Historical chart:    2026-06-12 00:00 -> 2026-07-10 22:59 Italy
ATAS requests:       5 sequential windows, maximum 7 days each
Raw trades received: 4.807.895
Ledger trades kept:  75.654, only READY -> RESOLVED
Profiles:            24
Events:              109
Outcomes:            436 = 109 events x 4 horizons
Orders / PnL:        0
```

Dataset and machine-readable aggregates:

```text
FabioOrderFlow/ledger-snapshots/compression-ledger-2026-07-11_18-02-29-*.csv
FabioOrderFlow/ledger-snapshots/compression-ledger-2026-07-11_18-02-29-summary.json
```

## Coverage

```text
TradeCoverage=AVAILABLE: 21 profiles, 99 events
TradeCoverage=MISSING:    3 profiles, 10 events, all 2026-06-26
```

Only the 99 available-coverage events are used for any flow comparison. The
three missing profiles remain valid for range geometry and price outcome only.

The 21 covered profiles average 7.10 study bars, 50.54 points range and
1.83x their prior median bar range. This confirms that the dynamic lifecycle
is a detector of local balance/contraction, not yet a verified label for a
visually tight Fabio-style dealing range.

## Descriptive Results

`CloseMoveRanges` is price-signed. The exported
`averageReversionCloseMoveRanges` is normalized by boundary: positive means a
move from HIGH/LOW back toward the range interior.

```text
Pooled flow-covered, profile-weighted reversion close:
H1   -0.083 ranges
H3   -0.127 ranges
H6   -0.157 ranges
H12  -0.197 ranges

By boundary at H12, profile-weighted reversion close:
HIGH  -0.381 ranges, 15 profiles / 59 events
LOW   +0.416 ranges, 12 profiles / 40 events
```

The opposite HIGH/LOW behavior and the difference between event-weighted and
profile-weighted pooled results show heterogeneous paths. A range with many
retests cannot be counted as independent evidence. This sample does not
support a direction-independent reversion rule.

```text
Interaction:
BREACH 97 events across 21 profiles
TOUCH   2 events across 2 profiles
```

TOUCH versus BREACH cannot be compared yet.

```text
Close state at H12:
INSIDE   POC touched 89.5% profile-weighted, 14 profiles / 35 events
OUTSIDE  POC touched 58.7% profile-weighted, 20 profiles / 62 events
```

This is a diagnostic candidate only. `INSIDE` is geometrically closer to the
range and POC, so it is not evidence for a tradable reversion trigger without
an out-of-sample comparison.

```text
Total-volume percentile prior 0.76-1.00:
19 profiles / 41 events
H6  profile-weighted reversion close: +0.138 ranges
H12 profile-weighted reversion close: +0.296 ranges
```

Lower volume bands have 7-8 profiles each and show mixed or negative
profile-weighted reversion in this sample. This is the only flow-relative
pattern worth retaining as a hypothesis, not as a threshold. Ten available
coverage events have percentile `NA` because no earlier comparable bar exists;
they must not be silently discarded in a future model.

POC touch rises mechanically with horizon in the pooled sample: 21.2% at H1,
37.4% at H3, 55.6% at H6 and 74.8% at H12. Without a matched baseline and
without a direction/invalidation contract, this is not an entry result.

## Decision

```text
Operational model: unchanged, COMPRESSION_EVENT_LEDGER_NO_TRADES.
Lifecycle:         unchanged.
Ledger filters:    none added.
Entries / PnL:     none.
```

Next collection must add independent London sessions and manual labels
`tight`, `extended`, or `unclear` per profile. Re-evaluate only after at least
20-30 sessions, with profile-weighted and out-of-sample comparisons. A future
shadow model needs a separate deterministic contract for direction, trigger,
invalidation, management and costs.
