# FabioOrderFlow — Agent Guide

Indicatore ATAS modulare per order flow analysis (NQ/ES futures).

**Quando lavori su questo progetto, consulta sempre i documenti specifici prima di agire.**

## 📂 Documentazione

| Documento | Path | Descrizione |
|-----------|------|-------------|
| **README** | [`README.md`](README.md) | Overview, quick start, build & deploy |
| **London MR Model** | [`models/LondonMeanReversionModel/README.md`](models/LondonMeanReversionModel/README.md) | Modello 2 - Mean Reversion (production) |
| **Post-London Impulse Model** | [`models/PostLondonImpulseModel/README.md`](models/PostLondonImpulseModel/README.md) | Modello 1 - Trend Following (planned) |

**Module docs:** Ogni modulo ha il proprio README.md nella sua directory.

## 🗂️ Struttura Progetto

```
FabioOrderFlow/
├── AGENTS.md                           # Questo file
├── README.md                           # Quick start
├── src/
│   ├── FabioOrderFlow.cs              # Orchestrator
│   ├── FabioOrderFlow.csproj
│   └── deploy.bat
└── models/
    ├── shared/                         # Shared modules
    │   └── BalanceZoneTracker/
    │       ├── BalanceZoneTracker.cs
    │       └── BalanceZoneTracker.md
    ├── LondonMeanReversionModel/      # Modello 2 (production)
    │   ├── README.md                  # Doc del modello
    │   └── modules/
    │       └── LondonMeanReversion/
    │           ├── LondonMeanReversionModule.cs
    │           └── README.md
    └── PostLondonImpulseModel/        # Modello 1 (planned)
        ├── README.md                  # Doc del modello
        └── modules/
            └── PostLondonImpulse/
                ├── README.md
                └── */                 # Submodules
```

**Design:** 
- Ogni **model** (trading strategy) ha la sua directory sotto `models/`
- Ogni model contiene i suoi **modules** (componenti tecnici)
- **models/shared/** contiene moduli condivisi tra tutti i models
- Le **doc stanano nelle directory dei moduli** (README.md o .md files)

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
- `[PROFILE_PREVIEW]` — Profile calculation
- `[MR_TRIGGER_M5]` — Mean reversion trigger
- `[MR_LIVE_AGGRESSION_*]` — Live aggression
- `[MR_OUTCOME]` — Outcome tracking

**Parsing guide:** `../../docs/atas/log-reading.md`

## 🎯 Workflow per Agents

### Working on London Mean Reversion Model

1. **Read:** [`models/LondonMeanReversionModel/README.md`](models/LondonMeanReversionModel/README.md)
2. **Module docs:** `models/LondonMeanReversionModel/modules/*/README.md`
3. **Code:** `models/LondonMeanReversionModel/modules/`
4. **Test:** CumulativeTrades chart + verify triggers in logs

### Working on Post-London Impulse Model

1. **Read:** [`models/PostLondonImpulseModel/README.md`](models/PostLondonImpulseModel/README.md)
2. **Module docs:** `models/PostLondonImpulseModel/modules/*/README.md` or `*.md`
3. **Code:** `models/PostLondonImpulseModel/modules/`
4. **Status:** Currently in design phase

### Working on Shared Modules (BalanceZoneTracker)

1. **Read:** `models/shared/BalanceZoneTracker/BalanceZoneTracker.md`
2. **Code:** `models/shared/BalanceZoneTracker/BalanceZoneTracker.cs`
3. **Note:** Shared between all models, changes affect all

### Debugging

1. **Enable logs:** `DetailedDebugLogs = true` in `BalanceZoneTracker.cs`
2. **Check ATAS logs:** `%APPDATA%\ATAS\Logs\`
3. **Parse with:** `../../docs/atas/log-reading.md`

## 🔗 Related Documentation

| Resource | Path |
|----------|------|
| ATAS log parsing | `../../docs/atas/log-reading.md` |
| ATAS API reference | `../../docs/atas/api/` |
| Modello 1 original spec | `../../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md` |

## ⚙️ Architecture

**Pattern:** Model-based organization, modular components, dependency injection

```
FabioOrderFlow (orchestrator)
├── LondonMeanReversionModel (Modello 2 - production)
│   └── LondonMeanReversionModule
├── PostLondonImpulseModel (Modello 1 - planned)
│   ├── ImpulseProfiler
│   ├── LowVolumeNodeDetector
│   └── AggressionDetector
└── shared/
    └── BalanceZoneTracker (used by all models)
```

**Design principles:**
- **Model isolation:** Each trading model is independent
- **Module composition:** Models use technical modules
- **Shared components:** Common logic in shared/
- **Documentation co-location:** Docs live with code

## 📝 Regole per Agents

1. **Leggi prima di agire:** Consulta il README del model, poi dei modules
2. **Documentazione co-located:** Ogni modulo ha il suo README.md nella sua directory
3. **Non improvvisare:** Segui l'architettura model → modules
4. **Shared modules:** Modifiche a shared/ impattano tutti i models
5. **Test sempre:** Build + deploy + verifica log dopo ogni modifica

## 🎯 Status

- **Version:** 2.0.0
- **Architecture:** Model-based (refactored June 2026)
- **Build:** ✅ 0 errors, 8 warnings (unused fields)
- **DLL size:** 74KB
- **Models:**
  - ✅ LondonMeanReversionModel (production)
  - ⚠️ PostLondonImpulseModel (design phase)
