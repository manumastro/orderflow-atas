# FabioOrderFlow

Indicatore ATAS modulare per analisi order flow (NQ/ES futures).

## Architecture

```
FabioOrderFlow (orchestrator)
├── BalanceZoneTracker (core)
│   ├── Session detection (London/NY)
│   ├── Volume profile (POC/VAH/VAL)
│   └── Balance zone state machine
└── LondonMeanReversionModule (strategy)
    ├── Rejection/fakeout detection
    ├── M5 trigger logic
    └── Aggression tracking
```

**Design:** Modular, hot-pluggable strategies, dependency injection.

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
| `EnableLondonMeanReversion` | `true` | Enable MR module |
| `EnableLiveFootprintFirst` | `false` | Experimental footprint detection |

## Documentation

| Document | Description |
|----------|-------------|
| [docs/modules/BalanceZoneTracker.md](docs/modules/BalanceZoneTracker.md) | Core balance zone tracking |
| [docs/modules/LondonMeanReversion.md](docs/modules/LondonMeanReversion.md) | Mean reversion strategy |
| [docs/modules/SessionDetector.md](docs/modules/SessionDetector.md) | Session detection utility |

## Development

**Project structure:**
```
src/
├── FabioOrderFlow.cs              # Orchestrator
├── modules/
│   ├── shared/
│   │   ├── BalanceZoneTracker/    # Core
│   │   └── SessionDetector/       # Utility
│   └── LondonMeanReversion/       # Strategy
└── deploy.bat
```

**Add new module:**
1. Create `modules/<ModuleName>/<ModuleName>Module.cs`
2. Inject `BalanceZoneTracker` via constructor
3. Register in `FabioOrderFlow.cs`
4. Add setting for enable/disable
5. Document in `docs/modules/<ModuleName>.md`

## Logs

**Location:** `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`

**Key tags:**
- `[PROFILE_PREVIEW]` - Profile calculation
- `[MR_TRIGGER_M5]` - Mean reversion trigger
- `[SESSION_START]` / `[SESSION_END]` - Session transitions

**Parsing guide:** `../../docs/atas/log-reading.md`

## Status

- **Version:** 2.0.0
- **Build:** ✅ 0 errors, 8 warnings
- **Architecture:** Modular (refactored from monolith)
- **DLL size:** 74KB (-17% from v1.x)
- **Code reduction:** -45% in core tracker

## Related

- ATAS API docs: `../../docs/atas/api/`
- Modello 1 spec: `../../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`
- Modello 2 spec: `../../Modello-2-MeanReversion/FabioMeanReversion.md`
