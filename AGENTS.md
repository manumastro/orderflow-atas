# orderflow-atas — Agent Guide

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

**Quando lavori su questo progetto, consulta sempre i documenti specifici prima di agire.**

---

## 📂 Progetti

### FabioOrderFlow

**Path:** [`FabioOrderFlow/`](FabioOrderFlow/)  
**Status:** ✅ Live-first London Mean Reversion  
**Doc:** [`FabioOrderFlow/FabioOrderFlow.md`](FabioOrderFlow/FabioOrderFlow.md)

Indicatore modulare ATAS: multiple trading models, modular architecture.

**Models:**
- ✅ **LondonMeanReversionModel** (active, live-first) - [`LondonMeanReversionModel.md`](FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.md)
- 🅿️ **PostLondonImpulseModel** (parked/design) - [`PostLondonImpulseModel.md`](FabioOrderFlow/models/PostLondonImpulseModel/PostLondonImpulseModel.md)

---

## 🗂️ Struttura

```
orderflow-atas/
├── AGENTS.md                           # Questo file
├── docs/atas/                          # ATAS API docs (shared)
├── Modello-1-TrendFollowing/           # Spec originale (reference)
└── FabioOrderFlow/                     # Progetto principale
    ├── FabioOrderFlow.md               # Doc progetto
    ├── src/                            # Build & orchestrator
    └── models/
        ├── shared/                     # Shared modules
        ├── LondonMeanReversionModel/   # Modello 2
        │   ├── LondonMeanReversionModel.md
        │   └── LondonMeanReversionModel.cs
        └── PostLondonImpulseModel/     # Modello 1
            ├── PostLondonImpulseModel.md
            ├── PostLondonImpulseModel.cs
            └── modules/                # Support modules
```

---

## 🎯 Workflow per Agents

### Working on FabioOrderFlow

1. **Start:** [`FabioOrderFlow/FabioOrderFlow.md`](FabioOrderFlow/FabioOrderFlow.md)
2. **Select model:** `FabioOrderFlow/models/<ModelName>/<ModelName>.md`
3. **Core logic:** `FabioOrderFlow/models/<ModelName>/<ModelName>.cs`
4. **Shared:** `FabioOrderFlow/models/shared/`

### Working on a Specific Model

**London Mean Reversion (Modello 2):**
- Doc: [`FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.md`](FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.md)
- Core: `FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.cs`

**Post-London Impulse (Modello 1):**
- Doc: [`FabioOrderFlow/models/PostLondonImpulseModel/PostLondonImpulseModel.md`](FabioOrderFlow/models/PostLondonImpulseModel/PostLondonImpulseModel.md)
- Core: `FabioOrderFlow/models/PostLondonImpulseModel/PostLondonImpulseModel.cs` (to be implemented)
- Support modules: `FabioOrderFlow/models/PostLondonImpulseModel/modules/`

---

## 🔧 Build & Deploy

```bash
cd FabioOrderFlow/src/
dotnet build -c Release
./deploy.bat
```

**Output:**
- DLL: `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll`
- Logs: `%APPDATA%\ATAS\Logs\`

---

## 📊 Log Files

**Location:** `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`

**Key tags:**
- `[SESSION_START]` / `[SESSION_END]`
- `[PROFILE_PREVIEW]`
- `[MR_SETUP_LONG]` / `[MR_SETUP_SHORT]`
- `[MR_ENTRY]`
- `[MR_MFE_UPDATE]` / `[MR_EXIT]`

**Parsing guide:** [`docs/atas/log-reading.md`](docs/atas/log-reading.md)

---

## 📚 Documentazione ATAS

- **Log reading:** [`docs/atas/log-reading.md`](docs/atas/log-reading.md)
- **API reference:** [`docs/atas/api/`](docs/atas/api/)

---

## 🎯 Status

**FabioOrderFlow:**
- Build: ✅ 0 errors, 0 warnings
- Architecture: ✅ Model-based
- Active model: London Mean Reversion
- Parked model: Post-London Impulse

---

## 📝 Regole per Agents

1. **Entry point:** Questo file (`AGENTS.md`)
2. **Project doc:** `FabioOrderFlow/FabioOrderFlow.md`
3. **Model doc:** `models/<ModelName>/<ModelName>.md`
4. **Core logic:** `models/<ModelName>/<ModelName>.cs`
5. **No README.md:** Tutti i doc hanno nome del progetto/model
6. **Build sempre:** Dopo modifiche al codice
7. **Priorita' corrente:** London Mean Reversion live-first; Post-London resta fuori scope salvo richiesta esplicita

---

## 🔗 Riferimenti

- **Modello 1 spec:** [`Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`](Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md)
