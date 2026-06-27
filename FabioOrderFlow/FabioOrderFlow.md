# FabioOrderFlow

Indicatore ATAS modulare per order flow su futures NQ/ES.

Obiettivo corrente: **London Mean Reversion live-first**. Il modello deve generare segnali in live durante London usando cumulative big trades, e usare i dati storici gia' caricati sul chart per mostrare come lo stesso motore si sarebbe comportato.

---

## Struttura

```text
FabioOrderFlow/
├── FabioOrderFlow.md
├── src/
│   ├── FabioOrderFlow.cs              # Orchestrator ATAS
│   ├── MarketTimeZones.cs             # Conversioni London/Italy/New York
│   ├── FabioOrderFlow.csproj
│   └── deploy.bat / deploy.sh
└── models/
    ├── LondonMeanReversionModel/      # Modello attivo
    │   ├── LondonMeanReversionModel.cs
    │   └── LondonMeanReversionModel.md
    ├── PostLondonImpulseModel/        # Non in scope ora
    │   ├── PostLondonImpulseModel.md
    │   └── modules/
    └── shared/
        └── BalanceZoneTracker/        # Profilo/value area condivisi
```

Regola: ogni modello ha una sola implementazione `.cs` attiva nella propria directory. Non tenere copie `.old` accanto al codice compilato.

---

## Architecture

```text
FabioOrderFlow.cs
├── inizializza BalanceZoneTracker
├── inizializza LondonMeanReversionModule
├── inoltra OnCalculate() al tracker
├── inoltra cumulative trades live al tracker
└── richiede cumulative trades storici dopo recalculation

BalanceZoneTracker
├── costruisce profilo London dinamico
├── espone LastPreviewPoc / LastPreviewVah / LastPreviewVal
├── evita duplicazione volume sulla candela corrente
└── inoltra eventi al modello London

LondonMeanReversionModule
├── crea setup su sweep/rejection di VAH/VAL preview
├── conferma entry con cumulative big trades live o storici
├── usa TickSize dello strumento per rejection/stop
└── traccia outcome da entry in avanti
```

---

## Models

| Model | Status | Scope |
|-------|--------|-------|
| LondonMeanReversionModel | Attivo | Mean reversion durante London |
| PostLondonImpulseModel | Design/parking | Non lavorare ora salvo richiesta esplicita |

---

## London Mean Reversion

Fonte logica: transcript Fabio Valentino.

Regole operative:

- sessione: London 08:00-16:00 London time, cioe' 09:00-17:00 italiane con ora legale;
- nuove entry: fino a 15:30 London, cioe' 16:30 italiane con ora legale;
- setup: sweep fuori `VAH/VAL` e close back inside;
- entry: big cumulative trade nella direzione del ritorno verso POC;
- target operativo: POC-only per trigger deboli, Target2 per POC reclaim/loss;
- stop: high/low della rejection +/- offset in tick, protetto dopo POC sui trade Target2;
- storico: usa la stessa logica live sui cumulative trades del chart.

Dettaglio completo: `models/LondonMeanReversionModel/LondonMeanReversionModel.md`.

---

## ATAS Data Flow

Live:

```text
OnCumulativeTrade / OnUpdateCumulativeTrade
→ FabioOrderFlow
→ BalanceZoneTracker.OnLiveCumulativeTrade
→ LondonMeanReversionModule.OnLiveCumulativeTrade
→ [MR_ENTRY] se setup + big trade sono validi
```

Storico:

```text
OnFinishRecalculate
→ RequestForCumulativeTrades in blocchi da massimo 7 giorni
→ OnCumulativeTradesResponse batch completo
→ BalanceZoneTracker.OnHistoricalCumulativeTrades
→ LondonMeanReversionModule.OnHistoricalCumulativeTrades
→ [MR_ENTRY] con EntryModel=FootprintCumulativeTradeHistorical
```

---

## Settings

| Setting | Default | Description |
|---------|---------|-------------|
| `EnableLondonMeanReversion` | `true` | Abilita il modello London live-first |
| `EnablePostLondonImpulse` | `false` | Placeholder, fuori scope ora |

---

## Logs

Location:

```text
%APPDATA%\ATAS\Logs\FabioOrderFlow.log
```

Key tags:

```text
[SESSION_START] / [SESSION_END]
[ZONE_READY]
[MR_SETUP_LONG] / [MR_SETUP_SHORT]
[MR_HISTORICAL_TRADES]
[MR_ENTRY]
[MR_MFE_UPDATE]
[MR_TARGET1_HIT]
[MR_EXIT]
[MR_STUDY_TRIGGER]
[MR_STUDY_ENTRY]
[MR_STUDY_TARGET1_HIT]
[MR_STUDY_CLOSE]
[MR_MISSED_OPPORTUNITY]
[MR_EXTENDED_CUTOFF_OPPORTUNITY]
[MR_STUDY_MIN_VOLUME_OPPORTUNITY]
[MR_STUDY_PRICE_TOUCH_OPPORTUNITY]
[MR_STUDY_SETUP_BAR_AGGRESSION]
```

`[MR_ENTRY]` include `EntryModel`, `ManagementMode`, `FinalTarget`, `StudyTarget2` e `StudyTrigger`:

```text
FootprintCumulativeTradeLive
FootprintCumulativeTradeHistorical
```

---

## Build & Deploy

```bash
cd FabioOrderFlow/src/
dotnet build -c Release
./deploy.bat
```

Output:

```text
%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll
```

---

## Development Rules

1. Prima di cambiare un modello, leggere il suo `.md`.
2. Dopo ogni modifica C#, eseguire `dotnet build -c Release`.
3. Il modello London e' la priorita' corrente.
4. Non aggiungere nuova documentazione se basta correggere i file esistenti.
5. Non duplicare implementazioni `.cs` nella directory del modello.
