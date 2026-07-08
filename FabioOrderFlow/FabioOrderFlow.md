# FabioOrderFlow

Indicatore ATAS modulare per order flow su NQ/ES. Il progetto contiene l'orchestrator, il tracker del volume profile e modelli indipendenti. Il modello operativo corrente e' `LondonMeanReversionModel`.

## Baseline Corrente

```text
Baseline:        2026-07-08 London MR reference + breakeven + NY close hold
Code commit:     f20ec7b
Validation docs: 26b17f5
Tag stabile:     london-ny-close-hold
Reload:          2026-07-08 10:40
Risultato:       20 trade chiusi, PnL [MR_EXIT] +634,25
```

Questa e' la baseline operativa fissa di partenza. Modifiche future devono essere confrontate contro questo risultato e devono avere un nuovo tag/checkpoint.

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
-> BalanceZoneTracker aggiorna profilo London e inoltra il flusso barre
-> LondonMeanReversionModel costruisce reference complete PreviousDayProfile/PreviousLondonProfile
-> LondonMeanReversionModel logga diagnostica CurrentLondonSessionProfile senza usarla per entry
-> LondonMeanReversionModel valuta setup e gestione posizioni

ATAS OnCumulativeTrade / OnUpdateCumulativeTrade
-> BalanceZoneTracker.OnLiveCumulativeTrade
-> LondonMeanReversionModel.OnLiveCumulativeTrade

ATAS OnFinishRecalculate
-> RequestForCumulativeTrades ultimi 7 giorni effettivi
-> LondonMeanReversionModel.OnHistoricalCumulativeTrades
-> LondonMeanReversionModel.ProcessHistoricalPositions con le stesse regole live
```

`BalanceZoneTracker` non decide entry, stop o target. Pubblica/visualizza i livelli London e inoltra barre/trade al modello. La strategia sta nel `.md` e nel `.cs` del modello specifico.

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
- PnL storico valido: sommare solo `[MR_EXIT]`;
- leggere il target operativo da `[MR_ENTRY] TargetPOC`, non dal POC visuale se l'indicatore volume profile e' impostato su `Current Day`;
- `[MR_ACTIVE_PROFILE_CONTEXT]` e' solo diagnostica sul profilo London corrente, non modifica entry/exit/PnL;
- le entry sono London, ma la massima durata trade e' fino a New York regular close 16:00 New York;
- usare `docs/atas/log-reading.md` prima di interpretare nuovi log.

## Build E Deploy

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
```

Dopo deploy: ricaricare ATAS/indicatore, attendere `[HISTORICAL_FLOW_FINISH]`, validare `[MR_ENTRY]` / `[MR_EXIT]` e aggiornare `CHANGELOG-AGENT.md` con poche righe.

## Regole Di Documentazione

- Questa pagina descrive progetto, flussi e procedure, non la strategia.
- Il `.md` del modello contiene tesi, regole, parametri, log e limiti della strategia.
- Il changelog contiene solo decisioni operative, risultati reload e comandi essenziali.
- Ogni doc deve essere leggibile da umano e agente: frasi brevi, nomi file/tag esatti, nessuna narrativa superflua.
