# FabioOrderFlow

Indicatore ATAS modulare per order flow su NQ/ES. Il runtime attivo studia esclusivamente il profilo impulso New York A->B tramite `FabioAuctionStudyModel`.

## Stato Corrente

```text
Modalita':          NEW_YORK_IMPULSE_STUDY_NO_TRADES
Sessione runtime:   New York 09:30-16:00 New York
Ordini / PnL:       DISABLED
Grafico:            nessun output London o trade
Output:             profilo A->B + pullback + cumulative flow + risoluzione
```

`LondonMeanReversionModel`, `BalanceZoneTracker`, il ledger auction-state dual-session e `PostLondonImpulseModel` non sono inizializzati dall'orchestrator. Restano sorgenti e baseline storiche riproducibili. `london-ny-close-hold` e il suo PnL `+634,25` appartengono soltanto al core legacy.

## Mappa Progetto

```text
src/FabioOrderFlow.cs                                  orchestrator ATAS, log, live/replay/storico
src/MarketTimeZones.cs                                 conversioni UTC/London/Italy/New York
models/shared/BalanceZoneTracker/                      baseline London compilata, runtime disattivato
models/LondonMeanReversionModel/                       baseline compression/shadow compilata, runtime disattivato
models/FabioAuctionStudyModel/                          studio runtime New York A->B, simmetrico e no-trade
models/PostLondonImpulseModel/                         piano legacy superseded, non operativo
tools/report_mr_performance.py                         report PnL legacy, solo MR_EXIT
tools/report_compression_ledger.py                     export/aggregati descrittivi ledger no-trade
tools/analyze_compression_shadow.py                    shadow primo evento offline, JSON only
tools/analyze_compression_state_entries.py             shadow state-confirmed offline, JSON only
tools/analyze_acceptance_path_transitions.py            transizioni path e flow iniziale, JSON only
tools/analyze_fabio_auction_playbooks.py                 balance rotation + NY pullback, JSON only
tools/report_auction_impulse_ledger.py                    profilo causale NY A->B, JSON only
tools/analyze_auction_impulse_confirmations.py             cumulative confirmation M1/M5, JSON only
performance-snapshots/                                 snapshot PnL legacy
ledger-snapshots/                                      CSV dataset e report JSON del compression ledger
archive/legacy-research/                               strumenti/snapshot pre-core, non operativi
CHANGELOG-AGENT.md                                     baseline, decisioni, reload verificati
```

## Contratto Tra Moduli

```text
ATAS OnCalculate -> FabioAuctionStudyModel
-> ignora tutte le barre fuori New York 09:30-16:00 New York
-> mantiene il profilo sessione NY necessario alla prior value area
-> riconosce l'impulso A->B e congela il footprint prima del pullback
-> registra READY, pullback e RESOLVED senza setup o trigger
-> non calcola rolling 6/12, developing value area, London o auction-state per barra

ATAS OnCumulativeTrade / OnUpdateCumulativeTrade
-> FabioAuctionStudyModel aggrega cumulative big trades nelle sole barre NY

ATAS OnFinishRecalculate
-> crea richieste CumulativeTrades sequenziali da massimo 7 giorni sull'intero chart
-> associa i trade alle sole barre NY trattenute in memoria
-> dopo l'ultima risposta emette soltanto marker impulse e summary
-> TradeCoverage=AVAILABLE/MISSING resta esplicito
```

`LondonMeanReversionModel` e `BalanceZoneTracker` restano nel progetto ma non sono costruiti, non ricevono barre/trade e non disegnano la zona London. `[AUCTION_STATE_BAR]` e' disabilitato nel runtime corrente; i vecchi snapshot dual-session restano baseline immutabili.

Reload M1 40 date verificato: output impulse identico byte-per-byte, tempo mode-to-summary ridotto del `72,5%` e log ridotto del `93,8%` rispetto al runtime dual-session/compression.

## Documenti Obbligatori

```text
FabioOrderFlow.md                                      panoramica progetto e procedure
CHANGELOG-AGENT.md                                     storico sintetico decisioni/reload
models/LondonMeanReversionModel/LondonMeanReversionModel.md  baseline compression e shadow
models/FabioAuctionStudyModel/FabioAuctionStudyModel.md      discovery dual-session dai transcript
models/shared/BalanceZoneTracker/BalanceZoneTracker.md       contratto del profilo/value area
```

## Log E Validazione

```text
%APPDATA%/ATAS/Logs/FabioOrderFlow.log
%APPDATA%/ATAS/Logs/FabioOrderFlow-historical.log
%APPDATA%/ATAS/Logs/FabioOrderFlow-live.log
%APPDATA%/ATAS/Logs/FabioOrderFlow-replay.log
```

Regole:

- reload storico completo solo dopo `[AUCTION_STATE_SUMMARY]`;
- controllare sempre `[CUM_TRADES_LOOKBACK]`, `[CUM_TRADES_RESPONSE]` e `[CUM_TRADES_COMPLETE]`: ogni singola request e' al massimo sette giorni, ma il batch copre l'intero chart;
- dopo un reload studio non devono apparire nuovi `[MR_ENTRY]` o `[MR_EXIT]`;
- `[MR_EXIT]` resta l'unica fonte PnL per confronti storici legacy, non per lo studio corrente;
- `PreviousDayProfile` e `PreviousLondonProfile` sono solo log;
- `[MR_LOCAL_PROFILE_READY]` e `[MR_LOCAL_PROFILE_RESOLVED]` delimitano causalmente ogni range;
- `[MR_COMPRESSION_LEDGER_PROFILE]`, `[MR_COMPRESSION_LEDGER_EVENT]` e `[MR_COMPRESSION_LEDGER_OUTCOME]` sono osservazioni, mai trade;
- usare `docs/research/fabio-transcript-synthesis.md` per il contratto derivato dai due transcript;
- usare `docs/research/compression-study-evaluation-2026-07-11.md` per la lettura comparativa dei sette range verificati;
- usare `docs/atas/log-reading.md` prima di interpretare nuovi log.

## Build E Deploy

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
```

Dopo deploy: ricaricare ATAS/indicatore, attendere `[AUCTION_STATE_SUMMARY]`, verificare `StudyMode=NEW_YORK_IMPULSE_STUDY_NO_TRADES`, `LondonBars=0`, conteggi impulse coerenti, assenza di `[HISTORICAL_FLOW_FINISH]`, `[AUCTION_STATE_BAR]`, box London e marker operativi; poi aggiornare `CHANGELOG-AGENT.md`.

Report PnL legacy:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report usa solo `[MR_EXIT]` per il PnL legacy. Il nuovo studio non produce PnL e non deve essere giudicato con il report performance.

Report ledger no-trade:

```bash
python FabioOrderFlow/tools/report_compression_ledger.py --save
```

Restituisce solo JSON su stdout e salva il report JSON con validation e aggregati flow-covered. Con `--save` salva anche i CSV con chiavi `ProfileLabel`, `EventBar`, `Boundary` e `HorizonBars`. Non introduce filtri, segnali o PnL.

Report auction-state storico/compatibilita':

```bash
python FabioOrderFlow/tools/report_auction_state_ledger.py --save
```

Restituisce JSON-only. Nei vecchi run salva le barre London/New York; nel runtime NY-only con `AuctionStateBars=DISABLED` valida il summary senza produrre il CSV per-barra utile alla discovery. Non genera segnali o PnL.

Report impulse profile New York:

```bash
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
```

M1 e' la granularita' primaria per questo report; M5 resta dataset baseline separato. Con dxFeed il candle footprint e' disponibile, ma se `CUM_TRADES_RESPONSE Count=0` cumulative bubble e relative conferme sono mancanti.

Conferma cumulative pre-risoluzione, M1/M5 separati:

```bash
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M1 --save
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M5 --save
```

Usa la soglia New York `30` dichiarata nei transcript, richiede directional dominance e non include la barra che ha gia' risolto l'impulso. Il report separa automaticamente `SessionDate < 2026-07-02` come historical holdout dal campione di formalizzazione successivo. Resta `selectionLeakage=true`, no-trade e non validato finche' non arrivano date prospettiche.

Analisi dei due playbook transcript:

```bash
python FabioOrderFlow/tools/analyze_fabio_auction_playbooks.py \
  --save FabioOrderFlow/ledger-snapshots/fabio-auction-playbooks-2026-07-11.json
```

Costruisce `BALANCE_ROTATION_V1` e `NY_IMBALANCE_PULLBACK_V1`, massimo una osservazione per modello/sessione/data/direzione, split cronologico e `selectionLeakage=true`. Non produce segnali o PnL.

Shadow offline esplorativo:

```bash
python FabioOrderFlow/tools/analyze_compression_shadow.py --save FabioOrderFlow/ledger-snapshots/compression-shadow-2026-07-11.json
```

Usa massimo una osservazione per profilo, split cronologico e exit fisse a 6/12 barre. Non ricostruisce stop, target, ordini o PnL.

Shadow state-confirmed offline:

```bash
python FabioOrderFlow/tools/analyze_compression_state_entries.py --save FabioOrderFlow/ledger-snapshots/compression-state-shadow-2026-07-11.json
```

Confronta causalmente failed breakout reversion e acceptance continuation, includendo nel JSON ogni entry individuale. I tempi di mercato mostrati nei log sono sempre i campi `Italy=`.

## Regole Di Documentazione

- Questa pagina descrive progetto, flussi e procedure, non la strategia.
- Il `.md` del modello contiene tesi, regole, parametri, log e limiti della strategia.
- Il changelog contiene solo decisioni operative, risultati reload e comandi essenziali.
- Ogni doc deve essere leggibile da umano e agente: frasi brevi, nomi file/tag esatti, nessuna narrativa superflua.
- Ogni risposta e aggiornamento deve chiarire in forma concisa, anche se gia' discusso: `Operativo`, `Diagnostico`, `Cambiato`, `Da verificare`. Non assumere che il lettore ricordi il contesto precedente.
