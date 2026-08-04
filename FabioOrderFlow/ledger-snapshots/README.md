# Ledger Snapshots

This directory holds local, reproducible artifacts generated from ATAS logs:

- `*.jsonl.gz`: filtered observation source snapshots;
- `*.csv`: event-level report tables;
- `*.json`: report metadata and aggregate summaries.

They are deliberately ignored by Git because an ATAS session log can be hundreds of megabytes. The corresponding report under `docs/research/` records each artifact path and SHA-256 hash. Keep the snapshot together with its report until the research result is superseded by a documented, canonical replacement.

Generate the 2026-08-04 session report from the repository root with:

```bash
python FabioOrderFlow/tools/report_session_location_response.py \
  --source-log "$APPDATA/ATAS/Logs/app_20260804.log" \
  --snapshot FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2.jsonl.gz \
  --events-csv FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-events.csv \
  --summary-json FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-summary.json \
  --report docs/research/session-location-and-price-response-description-2026-08-04.md
```

Generate the 2026-08-04 forensic case study from the existing local artifacts with:

```bash
python FabioOrderFlow/tools/report_session_forensic_case_study.py \
  --snapshot FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2.jsonl.gz \
  --events-csv FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-events.csv \
  --anchors-csv FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-anchors.csv \
  --timeline-csv FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-timeline.csv \
  --summary-json FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-summary.json \
  --report docs/research/session-forensic-case-study-2026-08-04.md \
  --non-overlap-summary-json FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-non-overlap-summary.json
```

Generate the canonical 2026-08-04 historical cumulative context `v5` inventory from the ATAS application log with:

```bash
python FabioOrderFlow/tools/report_historical_cumulative_context.py \
  --source-log "$APPDATA/ATAS/Logs/app_20260804.log" \
  --snapshot FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5.jsonl.gz \
  --candles-csv FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-candles.csv \
  --events-csv FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-events.csv \
  --summary-json FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-summary.json \
  --report docs/research/historical-cumulative-context-inventory-2026-08-04-v5.md
```

Reproduce the earlier `v4` evidence inventory with:

```bash
python FabioOrderFlow/tools/report_historical_cumulative_context.py \
  --source-log "$APPDATA/ATAS/Logs/app_20260804.log" \
  --snapshot FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4.jsonl.gz \
  --candles-csv FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-candles.csv \
  --events-csv FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-events.csv \
  --summary-json FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-summary.json \
  --report docs/research/historical-cumulative-context-inventory-2026-08-04.md \
  --schema fof-historical-cumulative-context-v4
```
