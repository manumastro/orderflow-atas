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
- entry operativa value-reentry: big cumulative trade nella value area tra edge e POC entro 20 minuti dalla rejection;
- entry operativa delayed reclaim: dopo escursione fuori value e reclaim accettato, big cumulative trade coerente dopo cambio controllo;
- POC reclaim/loss: conferma e gestione, non prerequisito rigido per aprire la base;
- target operativo: lato opposto della value area;
- stop: high/low della rejection +/- offset in tick, con cap dinamico a 0.5 value-area width quando il rischio tecnico e' eccessivo;
- scale-in operativo: massimo 2 add-on `EXPAND25` dopo POC/risk-free della base;
- storico: usa la stessa logica live sui cumulative trades del chart.

Dettaglio completo: `models/LondonMeanReversionModel/LondonMeanReversionModel.md`.

Stato corrente:

```text
Modello operativo: London Mean Reversion live-first
Entry families: value re-entry, delayed reclaim accepted, scale-in EXPAND25
Management: POC come Target1/protezione, Target2 al lato opposto della value area
Historical debug: daily logs per giorno caricato
PostLondonImpulse: fuori scope
```

Focus aperto:

```text
- monitorare qualita' causale delle delayed reclaim operative dopo reload/live
- monitorare max 2 scale-in EXPAND25 quando il mercato espande dopo POC
- mantenere continuation oltre POC come study separato finche' non confermata su piu' giorni
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
→ [MR_ENTRY] con EntryModel storico/intrabar/delayed reclaim secondo la famiglia entry
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
%APPDATA%\ATAS\Logs\FabioOrderFlow-historical.log
%APPDATA%\ATAS\Logs\FabioOrderFlow-live.log
%APPDATA%\ATAS\Logs\FabioOrderFlow-replay.log
%APPDATA%\ATAS\Logs\FabioOrderFlow-days\FabioOrderFlow-day-YYYY-MM-DD.log
```

I log sono separati per sorgente dati osservata:

```text
FabioOrderFlow.log             log generale compatto
FabioOrderFlow-historical.log  backfill/study da RequestForCumulativeTrades
FabioOrderFlow-live.log        callback online in modalita' Live
FabioOrderFlow-replay.log      callback online quando OnlineMode=Replay
FabioOrderFlow-days/           debug storico giornaliero, un file per giorno Italy del chart
```

ATAS espone callback live/replay e request storiche, ma non un flag documentato affidabile per distinguere live reale da replay. La distinzione live/replay e' quindi una proprieta' manuale dell'indicatore (`OnlineMode`). Il file historical viene scritto solo durante il backfill/recalculation, non dagli aggiornamenti parziali del replay. Con `EnableDailyHistoricalDebugLogs=true`, lo study aggregato pesante resta disattivato e i dettagli per barra/trade/setup vengono scritti nei file giornalieri.

Per sapere se un reload storico ha finito, controllare:

```text
FabioOrderFlow.log                 [HISTORICAL_FLOW_FINISH]
FabioOrderFlow-day-YYYY-MM-DD.log  [DAY_DEBUG_FINISH]
```

Git Bash:

```bash
grep "HISTORICAL_FLOW_FINISH" "$APPDATA/ATAS/Logs/FabioOrderFlow.log"
grep "DAY_DEBUG_FINISH" "$APPDATA/ATAS/Logs/FabioOrderFlow-days"/FabioOrderFlow-day-*.log
```

PowerShell:

```powershell
Select-String "HISTORICAL_FLOW_FINISH" "$env:APPDATA\ATAS\Logs\FabioOrderFlow.log"
Select-String "DAY_DEBUG_FINISH" "$env:APPDATA\ATAS\Logs\FabioOrderFlow-days\FabioOrderFlow-day-*.log"
```

`[HISTORICAL_FLOW_FINISH]` chiude il flusso complessivo barre + cumulative trades. `[DAY_DEBUG_FINISH]` chiude ogni file giornaliero e riporta conteggi di barre, big trade, entry, exit e delayed reclaim accepted.

Parser:

```bash
python FabioOrderFlow/tools/parse_day_study.py %APPDATA%/ATAS/Logs/FabioOrderFlow-historical.log
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

Ogni riga dei nuovi log include prefissi ordinabili:

```text
[Source=Historical|Live|Replay|General]
[Seq=N]
[WriteItaly=...]
[WriteUtc=...]
[EventItaly=...] quando disponibile
```

`[MR_ENTRY]` include `EntryModel`, `ManagementMode`, `FinalTarget`, `StudyTarget2`, `StudyTrigger`, `TriggerAtEntry` e stop operativo dinamico:

```text
FootprintCumulativeTradeLive
FootprintCumulativeTradeHistorical
FootprintCumulativeTradeHistoricalIntrabar
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
