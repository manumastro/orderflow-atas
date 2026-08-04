# ATAS Cumulative Trade Capture Validation - 2026-08-04

## Decision

```text
Decision:       promote the ATAS recorder to descriptive research for one observed session
H1 Aggregate:   pending; no DeepCharts or equivalent reference was available
Model:          none
Signals:        none
Orders / PnL:   none
```

This report evaluates the recorder and its data contract only. It does not infer absorption, acceptance, rejection, trade direction, or any model rule.

## Question

Can `Fabio Cumulative Trade Recorder` collect a reconstructible ATAS `CumulativeTrade` event stream, with correct incremental-volume accounting, security provenance, footprint context, and a historical response for a chart range of at most seven days?

## Sources And Boundaries

```text
ATAS application log:  %APPDATA%/ATAS/Logs/app_20260804.log
Instrument:            NQU6@CME
Instrument name:       E-Mini Nasdaq-100
Exchange:              CME
Connector ID:          NQU6
Live capture log time: 2026-08-04 12:02:24 to before 2026-08-04 13:12:34
Live event time:       2026-08-04 10:01:33.9753093 to 2026-08-04 11:09:13.6600612
Historical request:    2026-08-03 22:00:00 to 2026-08-04 11:12:45.4168907
Historical response:   requestId=635742406; records=50688
```

The application-log timestamp and the event payload timestamp are separate clocks. The payload does not provide an explicit time-zone identifier, so the time zone is recorded as absent rather than inferred.

The live capture begins after a previous wide-chart historical request was skipped. It ends before the reload used to request the one-day history. The separate historical response is not merged into the live-event totals.

## Pre-Registered Criteria

The observation contract requires 2,500 unique `live-new` events in one cash session, a historical response for a range of no more than seven days, metadata either present or explicitly absent, and no duplicated incremental volume.

## Live Capture Results

```text
Unique final events:           5,050
live-new records:              5,049
live-update records:           7,304
Total audit records:          12,353
Final event volume:            9,849 contracts
Incremental volume sum:        9,849 contracts
Buy final events:              2,584
Sell final events:             2,466
Events with footprint:         5,050 / 5,050
```

The first event in the capture arrived as `live-update`, which accounts for the one-event difference between unique final events and `live-new` records. It does not create a volume discrepancy.

Final per-event volume distribution:

```text
minimum:     1
median:      1
p90:         3
p99:        13
maximum:   108
```

Final constituent-tick-count distribution:

```text
minimum:     1
median:      1
p90:         3
p99:        10
maximum:    48
```

## Live Integrity Checks

```text
Records checked:                         12,353
Incremental volume mismatches:                0
Negative incremental volumes:                 0
Decreasing cumulative total volumes:          0
Security metadata missing:                    0 fields
Footprint records with no POC:                0
Footprint records with no first-price level:  7
Footprint records with no last-price level: 828
```

A missing first-price or last-price footprint level is retained as unavailable data. It is not converted to zero and it is not removed from the sample.

## Historical Response Checks

```text
Historical snapshots:                     50,688
Event time range:     2026-08-03 22:00:00 to 2026-08-04 11:12:45.4168907
Snapshots with footprint:                  50,688 / 50,688
Security metadata missing:                       0 fields
Footprint records with no POC:                   0
Footprint records with no first-price level:     0
Footprint records with no last-price level:      0
```

The earlier request on a 28.55-day chart was intentionally skipped. The successful request used a range shorter than seven days, as required by ATAS.

## Outcome

The recorder meets the technical conditions for descriptive research on the recorded ATAS session:

- the live sample exceeds the 2,500-event target;
- final event volume equals the sum of reported incremental volume;
- security and connector provenance are present;
- footprint context is captured and its unavailable levels are preserved;
- the constrained historical request returned a complete, logged response.

This promotion is limited to the recorder and the observed ATAS data. `CumulativeTrade` remains a declared functional proxy, not a verified replication of DeepCharts `Aggregate`. No model contract, threshold, marker, or operational behavior is approved by this result.

## Next Research Question

Before any interpretation, define a separate descriptive question for the recorded session: how do final `CumulativeTrade` size, tick composition, and footprint location co-occur with the contemporaneous auction structure? The question requires a frozen feature definition and a separate report; it must not be converted into a signal or a directional rule.
