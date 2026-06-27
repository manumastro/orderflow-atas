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
- entry operativa: big cumulative trade nella value area tra edge e POC entro 20 minuti dalla rejection;
- POC reclaim/loss: conferma e gestione, non prerequisito rigido per aprire la base;
- target operativo: lato opposto della value area;
- stop: high/low della rejection +/- offset in tick, con cap dinamico a 0.5 value-area width quando il rischio tecnico e' eccessivo;
- scale-in operativo: massimo 1 add-on `EXPAND25` dopo POC/risk-free della base;
- storico: usa la stessa logica live sui cumulative trades del chart.

Dettaglio completo: `models/LondonMeanReversionModel/LondonMeanReversionModel.md`.

Stato ultimo reload analizzato:

```text
Entry operative: 13
Base: 11
Scale-in EXPAND25: 2
PnL storico caricato: +123.25 punti
Net R: +2.67R
Current commit: cd72f7a Allow causal pre-POC value re-entry entries
```

Focus aperto:

```text
- filtrare meglio NONE / LOW_REJECTION_FOLLOW_THROUGH / follow-through deboli
- studiare tempo massimo dalla rejection per evitare setup stanchi
- studiare stop dinamici per non penalizzare trade buoni con stop tecnico enorme
- mantenere PostLondonImpulse fuori scope
```

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
→ RequestForCumulativeTrades sugli ultimi 7 giorni del chart
→ OnCumulativeTradesResponse
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
%APPDATA%\ATAS\Logs\FabioOrderFlow-study-historical.log
```

Il file `FabioOrderFlow-study-historical.log` e' un dataset dedicato per studiare entry alternative, continuation e scale-in Fabio-style sullo storico caricato.

Parser:

```bash
python FabioOrderFlow/tools/parse_day_study.py %APPDATA%/ATAS/Logs/FabioOrderFlow-study-historical.log
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
[MR_MISSED_OPPORTUNITY]
[MR_STUDY_CONTINUATION_ENTRY]
[DAY_STUDY_DYNAMIC_STOP_CANDIDATE]
[DAY_STUDY_SCALE_IN_SUMMARY]
[DAY_STUDY_SCALE_IN_CANDIDATE]
[DAY_STUDY_SCALE_PLAN]
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
