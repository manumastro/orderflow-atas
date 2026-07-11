# Compression Study Evaluation 2026-07-11

Source: reload `2026-07-11 10:27`, `FabioCompressionStudy`, historical cumulative trades from `2026-07-03 22:59:59 Italy` through `2026-07-10 22:59:59 Italy`.

This is an observational evaluation of the retired `COMPRESSION_CASES_NO_TRADES` candidate pass. It does not change detector thresholds, create entries, or calculate PnL. Since the event-ledger refactor, use this only as a baseline comparison; new analysis comes from `MR_COMPRESSION_LEDGER_*`.

## Result

```text
READY / RESOLVED profiles: 7 / 7
Retired study cases:      7
Retired candidates:       8
Operational entries:      0
```

The detector is technically causal and finds contracted-bar phases. It does not yet select only the tight visual dealing ranges Fabio draws. Its start criterion is a contracted bar and its score rewards overlap/low directional progress; it has no hard maximum for the total profile width relative to the prior-bar baseline.

## Profiles

```text
Date/time Italy       Bars  Range  Range/baseline  Score  Assessment
02/07 12:40-13:24       9   54.50       1.82       0.68   Geometric only; no historical trade coverage before 03/07 22:59.
03/07 10:00-10:34       7   28.25       1.56       0.80   Mechanically valid but only two high tests and 22 buy contracts at boundary.
03/07 12:05-12:39       7   19.50       1.18       0.76   Best sample: compact, 15 high tests, 403 boundary buy volume, CVD +406.
06/07 11:00-11:34       7   36.00       1.67       0.67   Weak: score at minimum and boundary buy volume only 23.
08/07 11:30-12:24      11   88.25       1.70       0.69   Too wide for a tight dealing range despite contracted bars.
08/07 13:05-13:39       7   60.00       1.49       0.75   Broad balance; strong upside participation but not visually tight.
09/07 10:50-11:34       9   46.75       1.84       0.68   Broad/extended lower-bound auction, not a clean compact balance.
```

`Range/baseline` compares the full profile width with the median preceding M5 bar range. It is evidence for comparison, not an active threshold.

## Candidate Reading

```text
03/07 10:44 BREAKOUT_LONG
Boundary buy=22, acceptance CVD=+34. Minimum evidence only; observation, not quality confirmation.

03/07 13:19 REVERSION_SHORT
Boundary buy absorption=24; sell confirmation CVD=-3; target POC only 2.00 points below entry and stop 3.50 points above.
This is not a viable entry candidate without an explicit reward/risk gate.

03/07 13:55 BREAKOUT_LONG
Boundary buy=84, acceptance CVD=+55. Stronger confirmation on the best compact range.

06/07 12:16 BREAKOUT_LONG
Entry=29874.50 and Stop=29874.50. It demonstrates that the study must report risk distance before any promotion; it cannot be used as an entry.

08/07 12:34 / 13:45 BREAKOUT_LONG
Boundary buy=173 / 358 and CVD=+259 / +448. Order flow is strong, but the parent ranges are 88.25 and 60.00 points: these are expansion observations, not tight-range examples.

09/07 11:54 REVERSION_LONG then 12:53 BREAKOUT_SHORT
The same broad profile first records absorbed selling/reversion and later accepted downside continuation. This is chronological, not a simultaneous contradiction, but it shows the range is a multi-phase auction and needs an event sequence display rather than one undifferentiated box.
```

## Conclusion

The current boxes answer "where did M5 bars contract and overlap?", not yet "where is Fabio's clean dealing range?". The cleanest available case is 03/07 12:05-12:39. The 08/07 and 09/07 boxes explain the visual confusion: their score passes because bar contraction/overlap are present, even while their total width is too large for an intuitive compact range.

The next diagnostic-only decision should be made after visual comparison of these seven rows: either define a deterministic maximum total-width proxy, or retain broad profiles but classify them as `BALANCE_EXTENDED` rather than `COMPRESSION_TIGHT`. This must remain separate from entry, retest, target, or risk-rule work.
