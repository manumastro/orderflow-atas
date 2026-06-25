# FabioOrderFlow — Agent Guide

Indicatore ATAS modulare per order flow analysis (NQ/ES futures).

**Quando lavori su questo progetto, consulta sempre i documenti specifici prima di agire.**

## 📂 Documentazione

| Documento | Path | Descrizione |
|-----------|------|-------------|
| **README** | [`README.md`](README.md) | Overview, quick start, settings |
| **Core Tracker** | [`docs/modules/BalanceZoneTracker.md`](docs/modules/BalanceZoneTracker.md) | Balance zones, sessioni, volume profile |
| **Session Detector** | [`docs/modules/SessionDetector.md`](docs/modules/SessionDetector.md) | Rilevamento sessioni, timezone |
| **Modello 2 MR** | [`docs/Modello-2-MeanReversion.md`](docs/Modello-2-MeanReversion.md) | Mean reversion strategy (London) |

## 🗂️ Struttura Progetto

```
FabioOrderFlow/
├── README.md                           # Quick start
├── AGENTS.md                           # Questo file
├── docs/
│   ├── Modello-2-MeanReversion.md     # Strategy mean reversion
│   └── modules/
│       ├── BalanceZoneTracker.md      # Core tracker
│       └── SessionDetector.md         # Session utility
└── src/
    ├── FabioOrderFlow.cs              # Orchestrator
    └── modules/
        ├── shared/
        │   ├── BalanceZoneTracker/    # Core
        │   └── SessionDetector/       # Utility
        └── LondonMeanReversion/       # Strategy MR
```

## 🔧 Build & Deploy

```bash
cd src/
dotnet build -c Release
./deploy.bat
```

**DLL:** `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll`

## 📊 Log Files

**Location:** `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`

**Key tags:**
- `[SESSION_START]` / `[SESSION_END]` — Session transitions
- `[PROFILE_PREVIEW]` — Profile calculation preview
- `[MR_TRIGGER_M5]` — Mean reversion trigger
- `[MR_LIVE_AGGRESSION_*]` — Live aggression tracking
- `[MR_OUTCOME]` — Outcome tracking

**Parsing guide:** `../../docs/atas/log-reading.md`

## 🎯 Workflow per Agents

### Working on Core Tracker

1. **Read:** [`docs/modules/BalanceZoneTracker.md`](docs/modules/BalanceZoneTracker.md)
2. **Code:** `src/modules/shared/BalanceZoneTracker/`
3. **Test:** Build + check logs per session detection

### Working on Mean Reversion Strategy

1. **Read:** [`docs/Modello-2-MeanReversion.md`](docs/Modello-2-MeanReversion.md)
2. **Code:** `src/modules/LondonMeanReversion/`
3. **Test:** CumulativeTrades chart + verify triggers in logs

### Adding New Strategy Module

1. **Create:** `src/modules/<ModuleName>/`
2. **Document:** `docs/Modello-X-<Name>.md`
3. **Register:** In `FabioOrderFlow.cs` orchestrator
4. **Update:** This `AGENTS.md` with new paths

### Debugging

1. **Enable logs:** `DetailedDebugLogs = true` in `BalanceZoneTracker.cs`
2. **Check ATAS logs:** `%APPDATA%\ATAS\Logs\`
3. **Parse with:** `../../docs/atas/log-reading.md`

## 🔗 Related Documentation

| Resource | Path |
|----------|------|
| ATAS log parsing | `../../docs/atas/log-reading.md` |
| ATAS API reference | `../../docs/atas/api/` |
| Modello 1 spec (Trend Following) | `../../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md` |
| Modello 2 original spec | `../../Modello-2-MeanReversion/FabioMeanReversion.md` |

## ⚙️ Architecture

**Pattern:** Modular, dependency injection, event delegation

```
FabioOrderFlow (orchestrator)
├── BalanceZoneTracker (core)
│   ├── Session detection
│   ├── Volume profile (POC/VAH/VAL)
│   └── Balance zone state machine
└── LondonMeanReversionModule (strategy)
    ├── Rejection detection
    ├── M5 trigger logic
    └── Aggression tracking
```

**Design principles:**
- Core tracker: pure balance zone logic
- Strategy modules: hot-pluggable, independent
- Read-only access: modules read state, don't mutate
- Event delegation: orchestrator → tracker → modules

## 📝 Regole per Agents

1. **Leggi prima di agire:** Consulta sempre il documento specifico del modulo/modello
2. **Non improvvisare:** Segui l'architettura esistente (DI, delegation, read-only)
3. **Mantieni separazione:** Core tracker non deve contenere strategy logic
4. **Documenta modifiche:** Aggiorna il .md relativo quando modifichi codice
5. **Test sempre:** Build + deploy + verifica log dopo ogni modifica

## 🎯 Status

- **Version:** 2.0.0
- **Architecture:** Modular (refactored June 2026)
- **Build:** ✅ 0 errors, 8 warnings (unused fields)
- **DLL size:** 74KB
- **Modelli implementati:** Modello 2 (Mean Reversion)
- **Modelli pianificati:** Modello 1 (Trend Following - Post-London Impulse)
