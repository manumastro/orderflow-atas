# FabioOrderFlow

Indicatore ATAS modulare per analisi order flow (NQ/ES futures).

## Struttura

```
FabioOrderFlow/
├── FabioOrderFlow.md                  # Questo file
├── src/
│   ├── FabioOrderFlow.cs             # Orchestrator
│   ├── FabioOrderFlow.csproj
│   └── deploy.bat
└── models/
    ├── shared/                        # Shared modules
    │   └── BalanceZoneTracker/
    ├── LondonMeanReversionModel/      # Modello 2 (production)
    │   ├── LondonMeanReversionModel.md
    │   └── LondonMeanReversionModel.cs
    └── PostLondonImpulseModel/        # Modello 1 (planned)
        ├── PostLondonImpulseModel.md
        ├── PostLondonImpulseModel.cs  # To be implemented
        └── modules/                   # Support modules
```

**Design:** Core file = model file (.cs con nome del model)

## Architecture

```
FabioOrderFlow (orchestrator)
├── LondonMeanReversionModel.cs (✅ production)
├── PostLondonImpulseModel.cs (⚠️ planned)
└── shared/BalanceZoneTracker (used by all)
```

## Models

| Model | Status | Doc |
|-------|--------|-----|
| LondonMeanReversionModel | ✅ Production | [LondonMeanReversionModel.md](models/LondonMeanReversionModel/LondonMeanReversionModel.md) |
| PostLondonImpulseModel | ⚠️ Planned | [PostLondonImpulseModel.md](models/PostLondonImpulseModel/PostLondonImpulseModel.md) |

## Quick Start

```bash
cd src/
dotnet build -c Release
./deploy.bat
```

**Deploy:** `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll` (74KB)

**ATAS:**
1. Restart ATAS
2. Add `FabioOrderFlow` to chart (CumulativeTrades type recommended)
3. Configure settings

## Settings

| Setting | Default | Description |
|---------|---------|-------------|
| `EnableLondonMeanReversion` | `true` | Enable MR model |
| `EnableLiveFootprintFirst` | `false` | Experimental footprint |

## Logs

**Location:** `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`

**Key tags:**
- `[SESSION_START]` / `[SESSION_END]` - Session transitions
- `[PROFILE_PREVIEW]` - Profile calculation
- `[MR_TRIGGER_M5]` - Mean reversion trigger
- `[MR_LIVE_AGGRESSION_*]` - Live aggression
- `[MR_OUTCOME]` - Outcome tracking

**Parsing guide:** `../docs/atas/log-reading.md`

## Development

**Project structure:**
```
src/FabioOrderFlow.cs              # Orchestrator
models/<ModelName>/
  ├── <ModelName>.md              # Doc
  ├── <ModelName>.cs              # Core
  └── modules/                    # Support (optional)
```

**Add new model:**
1. Create `models/<ModelName>/`
2. Add `<ModelName>.md` (doc)
3. Add `<ModelName>.cs` (core logic)
4. Register in `FabioOrderFlow.cs`
5. Add enable setting

## Status

- **Version:** 2.0.0
- **Build:** ✅ 0 errors, 8 warnings
- **Architecture:** Model-based
- **DLL size:** 74KB (-17% from v1.x)

## Related

- ATAS API docs: `../docs/atas/api/`
- ATAS log reading: `../docs/atas/log-reading.md`
- Modello 1 spec: `../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`
