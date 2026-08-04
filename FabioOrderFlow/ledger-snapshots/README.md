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
