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
[MR_REFERENCE_READY]         reference completa disponibile: PreviousDayProfile o PreviousLondonProfile
[MR_SETUP_LONG]              sweep sotto reference VAL e close back inside value
[MR_SETUP_SHORT]             sweep sopra reference VAH e close back inside value
[MR_SETUP_EXPIRED]           setup scaduto o POC toccato prima dell'entry
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[HISTORICAL_FLOW_FINISH]
[MR_ENTRY]                   entry su big trade nella direzione mean-reversion
[MR_BREAKEVEN]               stop portato a breakeven dopo MFE >= 1R
[MR_REPLAY_OPEN]             posizione storica aperta a fine dati; non sommare nel PnL
[MR_EXIT]                    exit finale e PnL valido
[MR_LIVE_HEARTBEAT]          heartbeat live leggero
[MR_REPLAY_AUDIT]            riepilogo storico: reject reasons e scadenze setup
[MR_SETUP_NO_ENTRY]          diagnostica per setup senza entry: stato finale, primo big trade utile/opposto e primo touch POC
```

## Come leggere un trade

1. Trova `[MR_REFERENCE_READY]` per capire quale reference e' disponibile.
2. Trova `[MR_SETUP_LONG]` o `[MR_SETUP_SHORT]`.
3. Verifica `Source`, `ReferenceLabel`, `POC`, `VAH`, `VAL`, `Stop`, `TargetPOC`.
4. Trova `[MR_ENTRY]` con lo stesso `SetupId`.
5. Controlla `ExecutionMode`:
   - `LIVE` = evento real-time;
   - `HISTORICAL` = replay di dati passati con le stesse regole.
6. Se presente, leggi `[MR_BREAKEVEN]` con lo stesso `SetupId`.
7. Trova `[MR_EXIT]` con lo stesso `SetupId`.
8. Usa solo il campo `PnL` di `[MR_EXIT]`.

## POC visuale vs TargetPOC

Il POC disegnato da un indicatore volume profile ATAS dipende dalla sua impostazione.

Se il profile visuale e' impostato su `Current Day`, puo' mostrare il developing POC corrente. Il modello London MR invece usa reference complete:

```text
PreviousDayProfile
PreviousLondonProfile
```

Quindi il POC visuale puo' differire da `[MR_ENTRY] TargetPOC`.

Esempio:

```text
Visual current-day POC circa 29500
[MR_ENTRY] TargetPOC=29540, Source=PreviousDayProfile, ReferenceLabel=2026-07-07
```

In caso di dubbio, il target operativo e' sempre quello loggato in `[MR_ENTRY] TargetPOC` e confermato da `[MR_REFERENCE_READY] POC`.

## Durata massima trade

Le entry sono London, ma non vanno chiuse automaticamente a fine London. Un setup valido puo' nascere vicino alla chiusura London e avere bisogno della sessione US per completare il rientro al POC.

Regola operativa:

```text
Entry window: London 08:00-16:00 London
Max hold:     New York regular close 16:00 New York
```

Nel periodo estivo normale questo corrisponde circa a:

```text
16:00 New York = 22:00 Italia = 20:00 UTC
```

Il codice usa `MarketTimeZones.NewYork`, quindi gestisce i cambi DST tramite timezone.

## Barre

- Le barre M5 sono stampate sull'open time della barra.
- I timestamp precisi delle entry arrivano dai cumulative trades e sono intrabar.

## Regola pratica

Se devi capire il risultato del modello:

```text
grep "\[MR_EXIT\]" FabioOrderFlow.log
```

Poi somma solo `PnL=`.
