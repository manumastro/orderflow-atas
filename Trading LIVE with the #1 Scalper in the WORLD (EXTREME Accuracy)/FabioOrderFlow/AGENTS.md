# FabioOrderFlow — Agent Guide

Indicatore ATAS modulare per order flow analysis (NQ/ES futures).

**Quando lavori su questo progetto, consulta sempre i documenti specifici prima di agire.**

## 📂 Documentazione

| Documento | Path | Descrizione |
|-----------|------|-------------|
| **README** | [`README.md`](README.md) | Overview, quick start, build & deploy |
| **London MR Model** | [`models/LondonMeanReversionModel/README.md`](models/LondonMeanReversionModel/README.md) | Modello 2 - Mean Reversion (production) |
| **Post-London Impulse Model** | [`models/PostLondonImpulseModel/README.md`](models/PostLondonImpulseModel/README.md) | Modello 1 - Trend Following (planned) |

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
    ├── LondonMeanReversionModel/       # Modello 2 (production)
    │   ├── README.md                   # Doc del modello
    │   └── LondonMeanReversionModule.cs  # CORE del modello
    └── PostLondonImpulseModel/         # Modello 1 (planned)
        ├── README.md                   # Doc del modello
        ├── PostLondonImpulseModel.cs   # CORE (da implementare)
        └── modules/                    # Moduli di supporto
            ├── AggressionDetector/
            ├── ImpulseProfiler/
            ├── LowVolumeNodeDetector/
            └── ...
```

**Design:** 
- Ogni **model** ha la sua directory sotto `models/`
- Ogni model ha il suo **file core** (.cs) a livello model
- **models/<Model>/modules/** contiene moduli di supporto (opzionale)
- **models/shared/** contiene moduli condivisi tra tutti i models
- **Documentation:** README.md a livello model, .md files nei support modules

## 🔧 Build & Deploy

```bash
cd src/
dotnet build -c Release
./deploy.bat
```

**DLL:** `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll` (74KB)

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
2. **Core code:** `models/LondonMeanReversionModel/LondonMeanReversionModule.cs`
3. **Test:** CumulativeTrades chart + verify triggers in logs

### Working on Post-London Impulse Model

1. **Read:** [`models/PostLondonImpulseModel/README.md`](models/PostLondonImpulseModel/README.md)
2. **Module design docs:** `models/PostLondonImpulseModel/modules/*/*.md`
3. **Core code:** `models/PostLondonImpulseModel/PostLondonImpulseModel.cs` (to be created)
4. **Status:** Design phase

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

**Pattern:** Model-based organization, core files at model level, shared components

```
FabioOrderFlow (orchestrator)
├── LondonMeanReversionModel
│   └── LondonMeanReversionModule.cs (core)
├── PostLondonImpulseModel
│   ├── PostLondonImpulseModel.cs (core - to be implemented)
│   └── modules/ (support: ImpulseProfiler, AggressionDetector, etc.)
└── shared/
    └── BalanceZoneTracker (used by all models)
```

**Design principles:**
- **Model isolation:** Each trading model independent
- **Core at model level:** Main logic in model's .cs file
- **Support modules:** Optional modules/ subdirectory
- **Shared components:** Common logic in models/shared/
- **Documentation co-location:** README.md with model

## 📝 Regole per Agents

1. **Leggi prima di agire:** Consulta il README del model
2. **Core logic:** A livello model (ModelName.cs)
3. **Support modules:** In models/<Model>/modules/ (se necessari)
4. **Shared modules:** Modifiche a shared/ impattano tutti i models
5. **Test sempre:** Build + deploy + verifica log dopo ogni modifica

## 🎯 Status

- **Version:** 2.0.0
- **Architecture:** Model-based (refactored June 2026)
- **Build:** ✅ 0 errors, 8 warnings
- **DLL size:** 74KB
- **Models:**
  - ✅ LondonMeanReversionModel (production)
  - ⚠️ PostLondonImpulseModel (design phase)
