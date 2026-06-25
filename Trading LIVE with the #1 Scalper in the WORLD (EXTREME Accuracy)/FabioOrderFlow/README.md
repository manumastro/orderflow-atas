# FabioOrderFlow

Indicatore ATAS modulare per analisi order flow sui futures (NQ/ES).

## Quick Start

### Build & Deploy

```bash
cd src/
dotnet build -c Release
./deploy.bat
```

**Output:** `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll`

**ATAS Setup:**
1. Remove indicator from chart (if present)
2. Restart ATAS
3. Add `FabioOrderFlow` indicator to chart

### Requirements

- ATAS Platform
- .NET 10.0 SDK
- Chart type: **CumulativeTrades** (per live aggression tracking)

## Features

### Balance Zone Tracking
- Automatic London session detection (08:00-16:00 GMT)
- Volume profile calculation (POC, VAH, VAL, 70% value area)
- Balance zone state machine: Building → Ready → Broken
- Visual rendering: colored rectangles + POC lines

### London Mean Reversion Module
- Fakeout/rejection detection su sweep di high/low
- M5 trigger quando prezzo ritorna verso POC
- Live aggression tracking via CumulativeTrades
- Diagnostic logging per backtesting

### Settings

| Parameter | Default | Description |
|-----------|---------|-------------|
| `EnableLondonMeanReversion` | `true` | Enable/disable mean reversion module |
| `EnableLiveFootprintFirst` | `false` | Experimental footprint-first detection |

## Architecture

```
FabioOrderFlow (orchestrator)
├── BalanceZoneTracker (core)
│   ├── Session detection (London/NY)
│   ├── Volume profile calculation
│   └── Balance zone state machine
└── LondonMeanReversionModule (strategy)
    ├── Rejection/fakeout detection
    ├── M5 trigger logic
    └── Aggression tracking
```

**Modular design:**
- Core tracker: pure balance zone logic
- Strategy modules: hot-pluggable, independent
- Dependency injection: modules read tracker state

**See [ARCHITECTURE.md](ARCHITECTURE.md) for full details.**

## Log Output Examples

### Profile Preview
```
[PROFILE_PREVIEW] Bar=145, 2024-01-15 14:00:00 UTC, Reason=live, Bars=73, 
  High=16825.00, Low=16802.25, POC=16815.50, VAH=16820.25, VAL=16808.75, 
  Close=16812.50, Relation=INSIDE_PREVIEW_VA
```

### Mean Reversion Trigger
```
[MR_TRIGGER_M5] Direction=SHORT, Trigger=HIGH_REJECTION_CANDIDATE, 
  Bar=156, SweepBar=152, POC=16815.50, VAH=16820.25, VAL=16808.75, 
  High=16827.50, Close=16813.25, RejectionTicks=14.25, Delta=-245
```

### Session Events
```
[SESSION_START] London session started at bar 45 (2024-01-15 08:00:00 UTC)
[SESSION_END] London session ended at bar 205 (2024-01-15 16:00:00 UTC)
```

**Log parsing guide:** `docs/atas/log-reading.md`

## Development

### Project Structure
```
FabioOrderFlow/
├── src/
│   ├── FabioOrderFlow.cs              # Entry point
│   ├── modules/
│   │   ├── shared/
│   │   │   ├── BalanceZoneTracker/    # Core logic
│   │   │   └── SessionDetector/       # Session utils
│   │   └── LondonMeanReversion/       # MR strategy
│   └── deploy.bat
├── ARCHITECTURE.md                     # Full architecture docs
└── README.md                           # This file
```

### Adding a New Module

1. Create module class in `modules/<ModuleName>/`
2. Inject `BalanceZoneTracker` reference via constructor
3. Read tracker state (read-only): `_tracker.CurrentZone`, `_tracker.POC`
4. Register in `FabioOrderFlow.cs` orchestrator
5. Add enable/disable setting

**Example:**
```csharp
public class PostLondonImpulseModule
{
    private readonly BalanceZoneTracker _tracker;
    
    public PostLondonImpulseModule(BalanceZoneTracker tracker)
    {
        _tracker = tracker;
    }
    
    public void OnBarUpdate(int bar, IndicatorCandle candle)
    {
        if (_tracker.CurrentZone?.BreakoutDirection == BreakoutDirection.Bullish)
        {
            // Impulse logic
        }
    }
}
```

See [ARCHITECTURE.md § Extensibility](ARCHITECTURE.md#extensibility) for full guide.

## Related Docs

| Document | Description |
|----------|-------------|
| [ARCHITECTURE.md](ARCHITECTURE.md) | Full architecture, data flow, design patterns |
| [docs/atas/log-reading.md](../../docs/atas/log-reading.md) | ATAS log parsing guide |
| [docs/atas/api/](../../docs/atas/api/) | ATAS Platform API reference |

## Credits

- **Fabio trading system:** London mean reversion + post-London impulse
- **ATAS Platform:** Order flow analytics framework
