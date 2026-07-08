# ATAS Log Reading

Guida canonica per interpretare i log di `FabioOrderFlow`.

## Principi

- Il file corrente viene azzerato a ogni inizializzazione indicatore.
- Il prefisso `WriteItaly` e' l'ora di scrittura del log, non necessariamente l'ora di mercato.
- Per eventi di mercato usa i campi embedded `Italy=`, `London=` e `UTC=`.
- Per analisi operativa usa l'orario italiano come riferimento principale.
- PnL valido: sommare solo `[MR_EXIT]`.

## File corrente

```text
%APPDATA%/ATAS/Logs/FabioOrderFlow.log
```

## LondonMeanReversionModel pulito

`MR` significa `Mean Reversion`.

Il modello ha solo due modalita':

```text
LIVE        dati real-time ATAS
HISTORICAL  dati passati processati con le stesse regole live
```

Marker principali:

```text
[MR_MODE]                    configurazione del modello pulito
[MR_SETUP_LONG]              sweep sotto VAL e close back inside value
[MR_SETUP_SHORT]             sweep sopra VAH e close back inside value
[MR_SETUP_EXPIRED]           setup scaduto o POC toccato prima dell'entry
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[HISTORICAL_FLOW_FINISH]
[MR_ENTRY]                   entry su big trade nella direzione mean-reversion
[MR_EXIT]                    exit finale e PnL valido
[MR_LIVE_HEARTBEAT]          heartbeat live leggero
[MR_REPLAY_AUDIT]            riepilogo storico: reject reasons e scadenze setup
[MR_SETUP_NO_ENTRY]          diagnostica per setup senza entry: stato finale, primo big trade utile/opposto e primo touch POC
```

## Come leggere un trade

1. Trova `[MR_SETUP_LONG]` o `[MR_SETUP_SHORT]`.
2. Verifica `POC`, `VAH`, `VAL`, `Stop`, `TargetPOC`.
3. Trova `[MR_ENTRY]` con lo stesso `SetupId`.
4. Controlla `ExecutionMode`:
   - `LIVE` = evento real-time;
   - `HISTORICAL` = replay di dati passati con le stesse regole.
5. Trova `[MR_EXIT]` con lo stesso `SetupId`.
6. Usa solo il campo `PnL` di `[MR_EXIT]`.

## Barre

- Le barre M5 sono stampate sull'open time della barra.
- I timestamp precisi delle entry arrivano dai cumulative trades e sono intrabar.

## Regola pratica

Se devi capire il risultato del modello:

```text
grep "\[MR_EXIT\]" FabioOrderFlow.log
```

Poi somma solo `PnL=`.
