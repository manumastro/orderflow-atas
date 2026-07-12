# Compression Ledger Snapshots

Output ripetibili di `../tools/report_compression_ledger.py`.

Snapshot canonici mantenuti:

```text
18-02-29   baseline ledger usata dall'analisi descrittiva iniziale
18-23-06   chart esteso usato dagli studi shadow offline
19-03-07   acceptance path completo verificato e fonte dello studio transizioni
```

I replay intermedi superseded non vengono conservati. Ogni snapshot completo contiene:

```text
*-profiles.csv                 un record per ProfileLabel
*-events.csv                   un record per ProfileLabel + Bar + Boundary
*-outcomes.csv                 un record per ProfileLabel + EventBar + Boundary + HorizonBars
*-shadow-entries.csv           acceptance continuation, massimo una per profilo
*-shadow-outcomes.csv          outcome direzionali H6/H12 delle shadow entry
*-shadow-path-bars.csv         ogni barra chart fino a 60 minuti dall'entry
*-shadow-low-flow-entries.csv  conferme LOW flow dopo tre barre
*-shadow-low-flow-outcomes.csv outcome H6/H12 dalla conferma
*-shadow-low-flow-path-bars.csv path 60 minuti dalla conferma
*-flow-covered-aggregates.csv  aggregati descrittivi; solo TradeCoverage=AVAILABLE
*-summary.json                 unico report: controlli replay, aggregati completi e metadati
```

Il comando restituisce soltanto JSON su stdout. Gli aggregati sono osservazioni senza PnL e senza segnali. `profiles` negli aggregati indica il numero di range distinti; i campi `profileWeighted*` riducono il peso di profili con molti retest.

Comando compression baseline:

```bash
python FabioOrderFlow/tools/report_compression_ledger.py --save
```

Comando auction-state storico/compatibilita':

```bash
python FabioOrderFlow/tools/report_auction_state_ledger.py --save
```

Per i run precedenti salva `auction-state-*-summary.json` e `auction-state-*-bars.csv` dual-session. Nel runtime NY-only `AuctionStateBars=DISABLED`: valida il summary e non ricostruisce un dataset per-barra. Non produce segnali o PnL.

Analisi playbook dai transcript:

```bash
python FabioOrderFlow/tools/analyze_fabio_auction_playbooks.py \
  --save FabioOrderFlow/ledger-snapshots/fabio-auction-playbooks-2026-07-11.json
```

Il report conserva tutte le osservazioni `BALANCE_ROTATION_V1` e `NY_IMBALANCE_PULLBACK_V1`, split train/test e `selectionLeakage=true`.

Profilo causale impulso New York:

```bash
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
```

Salva `auction-impulse-*-summary.json`, profili, barre pullback e risoluzioni in CSV. Con `LvnRanking=RAW_CAUSAL_V1` aggiunge `auction-impulse-*-lvns.csv` e `auction-impulse-*-touched-lvns.csv`; tutti i raw LVN restano inclusi. Gli snapshot M1 e M5 sono dataset distinti; la coverage cumulative mancante su dxFeed resta esplicita.

Conferma cumulative pre-risoluzione:

```bash
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M1 --save
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M5 --save
```

I report `auction-impulse-confirmations-M1|M5-*.json` conservano tutte le conferme e il sottoinsieme primario, massimo una per data/direzione. Separano inoltre lo storico precedente al `2026-07-02` dal campione usato per formalizzare il contratto. Non producono PnL.

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

Entry shadow confermate da stato, sempre offline e JSON-only:

```bash
python FabioOrderFlow/tools/analyze_compression_state_entries.py \
  --save FabioOrderFlow/ledger-snapshots/compression-state-shadow-2026-07-11.json
```

Confronta failed breakout reversion e acceptance continuation usando soltanto
stato disponibile alla barra di trigger. Il JSON include ogni entry e outcome.

Transizioni del path acceptance e directional flow iniziale, JSON-only:

```bash
python FabioOrderFlow/tools/analyze_acceptance_path_transitions.py \
  --save FabioOrderFlow/ledger-snapshots/acceptance-path-transitions-2026-07-11.json
```

Classifica continuous acceptance, rejection entro/dopo 15 minuti, POC touch,
reacceptance e opposite breakout. Misura il flow nelle prime 1/2/3 barre.
Le classi path sono descrittive e non producono entry, ordini o PnL.
