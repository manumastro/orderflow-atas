# FabioOrderFlow

Indicatore ATAS modulare per order flow su NQ/ES. Il progetto contiene l'orchestrator, il tracker del volume profile e modelli indipendenti. Il modello operativo corrente e' `LondonMeanReversionModel`.

## Mappa Progetto

```text
src/FabioOrderFlow.cs                                  orchestrator ATAS, log, live/replay/storico
src/MarketTimeZones.cs                                 conversioni UTC/London/Italy/New York
models/shared/BalanceZoneTracker/                      costruzione POC/VAH/VAL e inoltro eventi
models/LondonMeanReversionModel/                       modello attivo London mean reversion
models/PostLondonImpulseModel/                         parked, non operativo ora
CHANGELOG-AGENT.md                                     baseline, decisioni, reload verificati
```

## Contratto Tra Moduli

```text
ATAS OnCalculate
-> BalanceZoneTracker aggiorna profilo London e preview POC/VAH/VAL
-> LondonMeanReversionModel valuta setup, gestione posizioni e study storico

ATAS OnCumulativeTrade / OnUpdateCumulativeTrade
-> BalanceZoneTracker.OnLiveCumulativeTrade
-> LondonMeanReversionModel.OnLiveCumulativeTrade

ATAS OnFinishRecalculate
-> RequestForCumulativeTrades ultimi 7 giorni effettivi
-> LondonMeanReversionModel.OnHistoricalCumulativeTrades
-> LondonMeanReversionModel.ProcessHistoricalPositions
```

`BalanceZoneTracker` non decide entry, stop o target. Pubblica i livelli e notifica nuovi high/low London. La strategia sta nel `.md` e nel `.cs` del modello specifico.

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
%APPDATA%/ATAS/Logs/FabioOrderFlow-days/FabioOrderFlow-day-YYYY-MM-DD.log
```

Regole:

- reload storico completo solo dopo `[HISTORICAL_FLOW_FINISH]`;
- controllare sempre `[CUM_TRADES_LOOKBACK]`, perche' ATAS limita la request agli ultimi 7 giorni effettivi;
- PnL storico valido: sommare solo `[MR_EXIT]`, non `DAY_STUDY_ACTUAL_EXIT`;
- snapshot performance persistente: `python FabioOrderFlow/tools/snapshot_performance.py`;
- studio POC runner con archiviazione log: `python FabioOrderFlow/tools/study_poc_management.py --archive-logs`;
- usare `docs/atas/log-reading.md` prima di interpretare nuovi log.

## Build E Deploy

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
```

Dopo deploy: ricaricare ATAS/indicatore, attendere `[HISTORICAL_FLOW_FINISH]`, validare i day log e aggiornare `CHANGELOG-AGENT.md` con poche righe.

## Regole Di Documentazione

- Questa pagina descrive progetto, flussi e procedure, non la strategia.
- Il `.md` del modello contiene tesi, regole, parametri, log e limiti della strategia.
- Il changelog contiene solo decisioni operative, risultati reload e comandi essenziali.
- Ogni doc deve essere leggibile da umano e agente: frasi brevi, nomi file/tag esatti, nessuna narrativa superflua.
