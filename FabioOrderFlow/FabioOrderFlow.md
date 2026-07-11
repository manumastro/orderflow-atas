# FabioOrderFlow

Indicatore ATAS modulare per order flow su NQ/ES. Il progetto contiene l'orchestrator, il tracker del volume profile e modelli indipendenti. Il modulo attivo e' `LondonMeanReversionModel` in modalita' `FabioCompressionStudy`.

## Stato Corrente

```text
Modalita':          COMPRESSION_CASES_NO_TRADES
Ordini / PnL:       DISABLED
Reference profile:  LOG_ONLY
Grafico:            DynamicCompression + candidati studio
```

`london-ny-close-hold` e il suo PnL `+634,25` restano baseline storica del precedente core MR, non un modello in esecuzione.

## Mappa Progetto

```text
src/FabioOrderFlow.cs                                  orchestrator ATAS, log, live/replay/storico
src/MarketTimeZones.cs                                 conversioni UTC/London/Italy/New York
models/shared/BalanceZoneTracker/                      tracker London corrente; profile legacy non consumato
models/LondonMeanReversionModel/                       studio live/replay Fabio compression cases
models/PostLondonImpulseModel/                         parked, non operativo ora
tools/report_mr_performance.py                         report canonico marker MR correnti
performance-snapshots/                                 snapshot correnti London MR
archive/legacy-research/                               strumenti/snapshot pre-core, non operativi
CHANGELOG-AGENT.md                                     baseline, decisioni, reload verificati
```

## Contratto Tra Moduli

```text
ATAS OnCalculate
-> BalanceZoneTracker riconosce la sessione London; per ora mantiene anche stato profile legacy e inoltra il flusso barre
-> LondonMeanReversionModel costruisce PreviousDayProfile/PreviousLondonProfile solo per log
-> LondonMeanReversionModel mantiene il lifecycle dinamico SEARCHING/BUILDING/READY/RESOLVED di DynamicCompression
-> LondonMeanReversionModel studia assorbimento/reversion e acceptance/breakout per ogni compression congelata
-> LondonMeanReversionModel non crea setup, posizioni, PnL o marker trade reali

ATAS OnCumulativeTrade / OnUpdateCumulativeTrade
-> BalanceZoneTracker.OnLiveCumulativeTrade
-> LondonMeanReversionModel.OnLiveCumulativeTrade

ATAS OnFinishRecalculate
-> RequestForCumulativeTrades ultimi 7 giorni effettivi
-> LondonMeanReversionModel.OnHistoricalCumulativeTrades
-> LondonMeanReversionModel.ProcessHistoricalPositions con le stesse regole live
```

`BalanceZoneTracker` funziona correttamente ed e' lasciato invariato: oggi riconosce London, mantiene il suo stato profile legacy e inoltra eventi. Il modello studio non consuma POC/VAH/VAL, high/low, state machine o visual del tracker; box/livelli sono disattivati. Il refactor futuro, separato dallo studio, lo ridurra' a `LondonTracker`, con la sola responsabilita' di identificare confini e appartenenza alla sessione London. La visualizzazione attiva resta `DynamicCompression` del modulo studio.

## Documenti Obbligatori

```text
FabioOrderFlow.md                                      panoramica progetto e procedure
CHANGELOG-AGENT.md                                     storico sintetico decisioni/reload
models/LondonMeanReversionModel/LondonMeanReversionModel.md  strategia e contratto operativo del modello
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

- reload storico completo solo dopo `[HISTORICAL_FLOW_FINISH]`;
- controllare sempre `[CUM_TRADES_LOOKBACK]`, perche' ATAS limita la request agli ultimi 7 giorni effettivi;
- dopo un reload studio non devono apparire nuovi `[MR_ENTRY]` o `[MR_EXIT]`;
- `[MR_EXIT]` resta l'unica fonte PnL per confronti storici legacy, non per lo studio corrente;
- `PreviousDayProfile` e `PreviousLondonProfile` sono solo log;
- `[MR_LOCAL_PROFILE_READY]` e `[MR_LOCAL_PROFILE_RESOLVED]` sono l'input causale dello studio;
- `[MR_COMPRESSION_STUDY_CASE]`, `[MR_COMPRESSION_STUDY_CANDIDATE]` e `[MR_COMPRESSION_STUDY_PROFILE]` sono candidati, mai trade;
- usare `docs/research/fabio-transcript-synthesis.md` per il contratto derivato dai due transcript;
- usare `docs/atas/log-reading.md` prima di interpretare nuovi log.

## Build E Deploy

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
```

Dopo deploy: ricaricare ATAS/indicatore, attendere `[HISTORICAL_FLOW_FINISH]`, validare `Entries=0`, i marker `MR_COMPRESSION_STUDY_*` e la resa chart, poi aggiornare `CHANGELOG-AGENT.md` con poche righe.

Report canonico:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report usa solo `[MR_EXIT]` per il PnL legacy. Il nuovo studio non produce PnL e non deve essere giudicato con il report performance. I tempi di mercato mostrati nei log sono sempre i campi `Italy=`.

## Regole Di Documentazione

- Questa pagina descrive progetto, flussi e procedure, non la strategia.
- Il `.md` del modello contiene tesi, regole, parametri, log e limiti della strategia.
- Il changelog contiene solo decisioni operative, risultati reload e comandi essenziali.
- Ogni doc deve essere leggibile da umano e agente: frasi brevi, nomi file/tag esatti, nessuna narrativa superflua.
- Ogni risposta e aggiornamento deve chiarire in forma concisa, anche se gia' discusso: `Operativo`, `Diagnostico`, `Cambiato`, `Da verificare`. Non assumere che il lettore ricordi il contesto precedente.
