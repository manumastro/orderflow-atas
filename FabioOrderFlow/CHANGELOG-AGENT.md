# CHANGELOG AGENT - FabioOrderFlow

## Reload 2026-07-07 19:19 - Core Clean Validated

```text
Reload dopo reset Fabio core:
- [MR_MODE] presente: Model=FabioLondonMeanReversionCore, Modes=LIVE|HISTORICAL, BigTradeVolume=20, Target=POC_FULL_EXIT.
- [CUM_TRADES_RESPONSE] Count=1.346.471.
- [MR_HISTORICAL_TRADES] Count=1.346.471, BeginItaly=2026-06-30 00:00:00, EndItaly=2026-07-07 19:19:34.
- [HISTORICAL_FLOW_FINISH] Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.
- Setup trovati: 10 long, 15 short; 21 scaduti per POC toccato prima di entry.
- Nessun log residuo di DAY_STUDY, delayed reclaim, secondary rejection, pressure gate, historical intrabar, follow-through, second-leg, target2, scale-in.
- PnL da [MR_EXIT]: 0,00 perche' non ci sono entry valide nel core pulito.
- Rimossi dal filesystem i vecchi file FabioOrderFlow-days, ormai obsoleti e confondenti.
```

## Baseline 2026-07-07 - Fabio London Mean Reversion Core Clean

Decisione operativa richiesta: ripulire completamente `LondonMeanReversionModel` e lasciare una sola tipologia di entry, coerente con il transcript Fabio.

Contratto attuale:

```text
MR = Mean Reversion
LIVE = dati real-time ATAS
HISTORICAL = dati passati processati con le stesse regole live
PnL valido = solo [MR_EXIT]
```

Entry unica:

```text
London value area attiva
sweep/fakeout fuori VAH/VAL
close back inside value
cumulative big trade >= 20 contratti nella direzione di rientro
entry dentro value prima del POC
target full position al POC
stop piccolo vicino all'estremo fallito
```

Pulizia effettuata:

```text
- LondonMeanReversionModel ridotto a un solo file: LondonMeanReversionModel.cs.
- Rimossi partial file e moduli ausiliari precedenti.
- Rimossi log e percorsi di ricerca/debug paralleli.
- Rimossi modelli alternativi e filtri aggiunti sui pochi giorni disponibili.
- Rimasta solo gestione full exit al POC oppure stop.
- Rimasto solo replay storico delle stesse regole live.
```

Log attuali:

```text
[MR_MODE]
[MR_SETUP_LONG]
[MR_SETUP_SHORT]
[MR_SETUP_EXPIRED]
[MR_HISTORICAL_TRADES]
[HISTORICAL_FLOW_PROCESS_START]
[HISTORICAL_FLOW_FINISH]
[MR_ENTRY]
[MR_EXIT]
[MR_LIVE_HEARTBEAT]
```

Build:

```text
cd FabioOrderFlow/src && dotnet build -c Release
Avvisi: 0
Errori: 0
```

Deploy:

```text
%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll
```

Rollback utili:

```text
checkpoint-london-before-core-only     stato prima della semplificazione core-only
london-core-clean                      core-only prima del refactor totale
london-pressure-secondary-live         stato con filtri/secondary poi rimosso
```
