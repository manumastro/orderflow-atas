# Compression Ledger Snapshots

Output ripetibili di `../tools/report_compression_ledger.py`.

Ogni snapshot completo contiene:

```text
*-profiles.csv                 un record per ProfileLabel
*-events.csv                   un record per ProfileLabel + Bar + Boundary
*-outcomes.csv                 un record per ProfileLabel + EventBar + Boundary + HorizonBars
*-flow-covered-aggregates.csv  aggregati descrittivi; solo TradeCoverage=AVAILABLE
*-summary.json                 unico report: controlli replay, aggregati completi e metadati
```

Il comando restituisce soltanto JSON su stdout. Gli aggregati sono osservazioni senza PnL e senza segnali. `profiles` negli aggregati indica il numero di range distinti; i campi `profileWeighted*` riducono il peso di profili con molti retest.

Comando:

```bash
python FabioOrderFlow/tools/report_compression_ledger.py --save
```

Non confrontare direttamente `CloseMoveRanges` di HIGH e LOW: il report
fornisce `averageReversionCloseMoveRanges`, positivo soltanto quando il prezzo
si muove dal bordo verso l'interno del range.

Shadow fixed-horizon esplorativo, sempre JSON e senza ordini/PnL:

```bash
python FabioOrderFlow/tools/analyze_compression_shadow.py \
  --save FabioOrderFlow/ledger-snapshots/compression-shadow-2026-07-11.json
```

Usa al massimo il primo evento di ogni profilo, separa train/test per data e
non ricostruisce stop o target dall'ordine intrabar non disponibile.
