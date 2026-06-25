# Post-London Impulse Module

**Status:** Documented, not implemented  
**Target Location:** `PostLondonImpulse/PostLondonImpulseModule.cs`  
**Lines:** TBD

---

## Overview

Follow impulse moves after London breakout, entering at aggression clusters in low volume nodes, targeting POC of previous balance zone.

**Active during:**
- Post-London breakout (16:00+ London)
- NY session overlap preferred

---

## Strategy

**From Fabio's transcript:**
> "The best session to use this model it's 100% New York session... I don't trade before New York session."

**Pattern:**
1. Balance zone established during London
2. Breakout confirmed at end of London (~16:10)
3. Impulse move detected post-breakout
4. Low volume nodes identified as entry zones
5. Aggression cluster confirms entry
6. Target: POC of previous balance

---

## Submodules

### 1. ImpulseProfiler
**Path:** `PostLondonImpulse/ImpulseProfiler/`  
**Status:** Documented  
**Doc:** `ImpulseProfiler/ImpulseProfiler.md`

**Responsibilities:**
- Profile impulse move after breakout
- Identify impulse direction (up/down)
- Measure impulse strength and velocity
- Detect impulse exhaustion

---

### 2. LowVolumeNodeDetector
**Path:** `PostLondonImpulse/LowVolumeNodeDetector/`  
**Status:** Documented  
**Doc:** `LowVolumeNodeDetector/LowVolumeNodeDetector.md`

**Responsibilities:**
- Detect low volume price levels during impulse
- Identify entry zones (low resistance)
- Avoid high volume rejection zones
- Monitor volume profile in real-time

---

### 3. AggressionDetector
**Path:** `PostLondonImpulse/AggressionDetector/`  
**Status:** Documented  
**Doc:** `AggressionDetector/AggressionDetector.md`

**Responsibilities:**
- Detect aggression clusters (big trades)
- Confirm entry at low volume nodes
- Monitor continuation vs reversal patterns
- Real-time trade flow analysis

---

### 4. TradeManager
**Path:** `PostLondonImpulse/TradeManager/`  
**Status:** Documented  
**Doc:** `TradeManager/TradeManager.md`

**Responsibilities:**
- Entry execution management
- Position sizing
- Stop placement (balance zone extremes)
- Target management (POC of previous balance)
- Exit on invalidation

---

### 5. ConfirmationLayer
**Path:** `PostLondonImpulse/ConfirmationLayer/`  
**Status:** Documented  
**Doc:** `ConfirmationLayer/ConfirmationLayer.md`

**Responsibilities:**
- Multi-timeframe confirmation
- Delta divergence detection
- Volume confirmation
- Risk/reward filtering

---

### 6. VisualRenderer
**Path:** `PostLondonImpulse/VisualRenderer/`  
**Status:** Documented  
**Doc:** `VisualRenderer/VisualRenderer.md`

**Responsibilities:**
- Visual impulse indicators
- Low volume node markers
- Aggression cluster visualization
- Entry/exit signals on chart

---

## Implementation Order

From `../../docs/PostLondonImpulse.md`:

1. **BalanceZoneTracker** (shared, already implemented)
2. **ImpulseProfiler** - Profile post-breakout moves
3. **LowVolumeNodeDetector** - Find entry zones
4. **AggressionDetector** - Confirm entries
5. **TradeManager** - Execute and manage trades
6. **ConfirmationLayer** - Additional filters
7. **VisualRenderer** - Chart visualization

---

## Target Architecture

```csharp
public class PostLondonImpulseModule
{
    private readonly BalanceZoneTracker _balanceTracker;
    private readonly ImpulseProfiler _impulseProfiler;
    private readonly LowVolumeNodeDetector _volumeDetector;
    private readonly AggressionDetector _aggressionDetector;
    private readonly TradeManager _tradeManager;
    private readonly ConfirmationLayer _confirmationLayer;
    private readonly VisualRenderer _visualRenderer;
    
    public PostLondonImpulseModule(
        BalanceZoneTracker balanceTracker,
        Action<string> log,
        Func<int, IndicatorCandle> getCandle)
    {
        _balanceTracker = balanceTracker;
        _impulseProfiler = new ImpulseProfiler(...);
        _volumeDetector = new LowVolumeNodeDetector(...);
        _aggressionDetector = new AggressionDetector(...);
        _tradeManager = new TradeManager(...);
        _confirmationLayer = new ConfirmationLayer(...);
        _visualRenderer = new VisualRenderer(...);
    }
    
    public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar)
    {
        if (_balanceTracker.State != MarketState.OutOfBalance)
            return;
        
        // 1. Profile impulse
        _impulseProfiler.Update(bar, candle);
        
        // 2. Detect low volume nodes
        _volumeDetector.Update(bar, candle);
        
        // 3. Detect aggression at nodes
        _aggressionDetector.Update(bar, candle);
        
        // 4. Manage trades
        _tradeManager.Update(bar, candle);
        
        // 5. Render visuals
        _visualRenderer.Render(bar);
    }
    
    public void OnCumulativeTrade(CumulativeTrade trade)
    {
        _aggressionDetector.OnTrade(trade);
    }
}
```

---

## Configuration

```csharp
// In FabioOrderFlow.cs
public bool EnablePostLondonImpulse { get; set; } = false;

// Module instantiation
if (EnablePostLondonImpulse)
{
    _impulseModule = new PostLondonImpulseModule(
        _balanceTracker,
        Log,
        GetCandle);
}
```

---

## Dependencies

**From BalanceZoneTracker:**
- `CurrentZone` - Previous balance zone (for target POC)
- `State` - Must be OutOfBalance to activate
- `BreakoutBar` - Breakout timing reference
- `IsInNewYorkSession()` - Session filtering

**From FabioOrderFlow:**
- `Log()` - Logging function
- `GetCandle()` - Bar data accessor
- `RequestCumulativeTrades()` - Historical trades
- `OnCumulativeTrade()` - Live trade events

---

## Testing Plan

1. **Backtest on historical breakouts**
   - Identify clean breakouts from historical data
   - Verify impulse profiling accuracy
   - Test low volume node detection
   - Validate aggression detection

2. **Forward test on paper trading**
   - Real-time impulse tracking
   - Entry signal generation
   - Exit management validation
   - Performance metrics collection

3. **Live validation**
   - Compare with Fabio's live trades
   - Validate timing and entry quality
   - Monitor win rate and risk/reward
   - Adjust parameters based on results

---

## Full Documentation

**Strategy specification:** `../../docs/PostLondonImpulse.md`  
**Module specifications:** See each submodule's `.md` file  
**Code location (target):** `PostLondonImpulse/*.cs`

---

**Status:** Specifications complete, awaiting implementation.
