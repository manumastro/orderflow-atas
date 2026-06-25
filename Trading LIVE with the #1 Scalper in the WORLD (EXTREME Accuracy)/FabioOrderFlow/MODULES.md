# Strategy Modules Guide

## Overview

Strategy modules sono componenti hot-pluggable che implementano logiche di trading specifiche, leggendo state dal `BalanceZoneTracker` core.

## Existing Modules

### LondonMeanReversionModule

**Location:** `modules/LondonMeanReversion/`

**Purpose:** Mean reversion trading su balance zones di Londra.

**Strategy Logic:**
1. Detect fakeout/rejection su sweep di high/low
2. Trigger M5 quando prezzo ritorna verso POC
3. Track aggression via CumulativeTrades (bid/ask imbalance)
4. Log outcomes quando POC viene raggiunto

**Dependencies:**
- `BalanceZoneTracker`: legge `CurrentZone`, `LastPreviewPoc/Vah/Val`
- `CumulativeTrades` chart: necessario per live aggression tracking

**Enable/Disable:**
```csharp
[Display(Name = "Enable London Mean Reversion", GroupName = "Modules")]
public bool EnableLondonMeanReversion { get; set; } = true;
```

**Data Structures:**
- `MeanReversionTriggerLog`: trigger M5 registrati (direction, bar, POC, rejection ticks)
- `MeanReversionOutcome`: outcome tracking (POC reached, bars elapsed, profit/loss)
- `LiveSweepCandidate`: sweep attivo in attesa di rejection/entry (footprint-first)

**Key Methods:**
```csharp
// Event handlers
public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades)
public void OnLiveCumulativeTrade(CumulativeTrade trade)

// Internal logic
private bool LogPotentialRejection(...)  // Detect rejection candle
private void RegisterMeanReversionTrigger(...)  // Save M5 trigger
private void TryLogLiveLongAggression(...)  // Check ask > bid sotto VAL
private void TryLogLiveShortAggression(...)  // Check bid > ask sopra VAH
private void UpdateMeanReversionOutcomes(...)  // Verify POC reached
```

**Configuration:**
- `MinAggressionTradeVolume`: 20 contracts (threshold per aggression trade)
- `EnableLiveFootprintFirst`: false (experimental sweep detection)

**Log Output:**
```
[MR_TRIGGER_M5] Direction=SHORT, Trigger=HIGH_REJECTION_CANDIDATE, Bar=156, 
  SweepBar=152, POC=16815.50, VAH=16820.25, VAL=16808.75, High=16827.50, 
  Close=16813.25, RejectionTicks=14.25, Delta=-245

[MR_LIVE_AGGRESSION_LONG] Bar=178, Price=16809.25, Bid=45, Ask=123, 
  Delta=78, TotalVolume=168, BelowVAL (16808.75), DistToPOC=-6.25

[MR_OUTCOME] TriggerBar=156, CurrentBar=189, Elapsed=33, POC_REACHED=YES, 
  EntryPrice=16813.25, POCPrice=16815.50, ProfitTicks=2.25
```

**Status:** Production-ready (experimental footprint-first feature disabled by default)

---

## Module Development Guide

### Step-by-Step: Create New Module

#### 1. Create Module Directory

```
modules/
└── <ModuleName>/
    ├── <ModuleName>Module.cs       # Main module class
    ├── <DataClass1>.cs             # Data structures (if needed)
    └── <DataClass2>.cs
```

Example: `PostLondonImpulseModule`

```
modules/
└── PostLondonImpulse/
    ├── PostLondonImpulseModule.cs
    ├── ImpulseTrigger.cs
    └── ImpulseOutcome.cs
```

#### 2. Implement Module Class

**Template:**
```csharp
namespace ATAS.Indicators.Technical.Modules.PostLondonImpulse
{
    public class PostLondonImpulseModule
    {
        private readonly BalanceZoneTracker _tracker;
        private readonly Action<string> _log;
        private readonly Func<int, IndicatorCandle> _getCandle;
        
        // Module state
        private List<ImpulseTrigger> _triggers = new();
        
        public PostLondonImpulseModule(
            BalanceZoneTracker tracker,
            Action<string> log,
            Func<int, IndicatorCandle> getCandle)
        {
            _tracker = tracker;
            _log = log;
            _getCandle = getCandle;
        }
        
        // Public API: data access
        public List<ImpulseTrigger> Triggers => _triggers;
        
        // Event handler: chiamato da FabioOrderFlow per ogni bar
        public void OnBarUpdate(int bar, IndicatorCandle candle)
        {
            // Read tracker state (read-only)
            var zone = _tracker.CurrentZone;
            if (zone == null || zone.BreakoutDirection == null)
                return;
            
            // Strategy logic
            if (IsImpulseEntry(bar, candle, zone))
            {
                var trigger = new ImpulseTrigger
                {
                    Bar = bar,
                    Direction = zone.BreakoutDirection.Value,
                    EntryPrice = candle.Close,
                    TargetPOC = zone.POC
                };
                
                _triggers.Add(trigger);
                _log($"[IMPULSE_TRIGGER] Bar={bar}, Direction={trigger.Direction}, Entry={trigger.EntryPrice:F2}");
            }
        }
        
        private bool IsImpulseEntry(int bar, IndicatorCandle candle, BalanceZone zone)
        {
            // Implement strategy conditions
            // Example: breakout + aggression cluster + low volume node
            return false;
        }
    }
}
```

#### 3. Register in FabioOrderFlow.cs

**Add setting:**
```csharp
[Display(Name = "Enable Post-London Impulse", GroupName = "Modules", Order = 1)]
public bool EnablePostLondonImpulse { get; set; } = false;
```

**Initialize module:**
```csharp
protected override void OnCalculate(int bar, decimal value)
{
    if (bar == 0)
    {
        _balanceTracker = new BalanceZoneTracker(this, Log, Rectangles, HorizontalLinesTillTouch, GetCandle, EnableLiveFootprintFirst);
        
        if (EnablePostLondonImpulse)
        {
            _impulseModule = new PostLondonImpulseModule(_balanceTracker, Log, GetCandle);
            Log("[MODULE] Post-London Impulse module initialized");
        }
    }
    
    _balanceTracker.OnBarUpdate(bar, candle, CurrentBar);
    _impulseModule?.OnBarUpdate(bar, candle);
}
```

#### 4. Add CumulativeTrades Support (Optional)

If module needs tick-by-tick aggression data:

```csharp
// In module class
public void OnLiveCumulativeTrade(CumulativeTrade trade)
{
    // Process live trade
    if (trade.Direction == TradeDirection.Buy && trade.Volume > 50)
    {
        _log($"[IMPULSE_AGGRESSION] Price={trade.Price:F2}, Volume={trade.Volume}");
    }
}

// In FabioOrderFlow.cs
if (ChartInfo.ChartType == "CumulativeTrades" && bar == CurrentBar - 1)
{
    var trade = GetCumulativeTrade(bar);
    if (trade != null)
    {
        _impulseModule?.OnLiveCumulativeTrade(trade);
    }
}
```

#### 5. Expose Data via Tracker (If Needed)

If module needs new data from tracker, add public properties:

```csharp
// In BalanceZoneTracker.cs
public int? LondonCloseBar => _context.CurrentZone?.EndBar;
public decimal LondonRange => (_context.CurrentZone?.High - _context.CurrentZone?.Low) ?? 0;
```

---

## Design Patterns

### Dependency Injection

**Good:**
```csharp
public MyModule(BalanceZoneTracker tracker, Action<string> log)
{
    _tracker = tracker;  // Inject dependency
    _log = log;
}
```

**Bad:**
```csharp
public MyModule()
{
    _tracker = new BalanceZoneTracker(...);  // ❌ Hard-coded dependency
}
```

### Read-Only Access

**Good:**
```csharp
var poc = _tracker.CurrentZone?.POC;  // ✅ Read state
var vah = _tracker.LastPreviewVah;
```

**Bad:**
```csharp
_tracker.CurrentZone.POC = 16800;  // ❌ Modifying tracker state
_tracker._context.State = MarketState.Broken;  // ❌ Accessing private fields
```

### Event Delegation

**Good:**
```csharp
// In FabioOrderFlow.cs
_tracker.OnBarUpdate(bar, candle, CurrentBar);
_myModule?.OnBarUpdate(bar, candle);  // ✅ Explicit call
```

**Bad:**
```csharp
// In BalanceZoneTracker.cs
_myModule.OnBarUpdate(bar, candle);  // ❌ Tracker shouldn't know about modules
```

### Single Responsibility

**Good:**
```csharp
// PostLondonImpulseModule: only impulse logic
public void OnBarUpdate(...)
{
    if (IsImpulseEntry(...))
        RegisterTrigger(...);
}
```

**Bad:**
```csharp
// PostLondonImpulseModule doing balance zone calculation
public void OnBarUpdate(...)
{
    var poc = CalculatePOC(_tracker.CurrentZone.Profile);  // ❌ Tracker's job
}
```

---

## Testing Modules

### Unit Test Template

```csharp
[Test]
public void ImpulseModule_DetectsEntry_OnBullishBreakout()
{
    // Arrange
    var mockTracker = new Mock<IBalanceZoneTracker>();
    mockTracker.Setup(t => t.CurrentZone).Returns(new BalanceZone
    {
        BreakoutDirection = BreakoutDirection.Bullish,
        POC = 16815.50m
    });
    
    var module = new PostLondonImpulseModule(mockTracker.Object, Console.WriteLine, bar => null);
    
    // Act
    module.OnBarUpdate(100, new IndicatorCandle { Close = 16825.00m });
    
    // Assert
    Assert.AreEqual(1, module.Triggers.Count);
    Assert.AreEqual(BreakoutDirection.Bullish, module.Triggers[0].Direction);
}
```

### Integration Testing (ATAS)

1. Deploy indicator to ATAS
2. Load historical data su CumulativeTrades chart
3. Enable module via settings
4. Check log output: `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`
5. Parse logs with `docs/atas/log-reading.md`

### Debugging Tips

**Enable verbose logging:**
```csharp
_log($"[DEBUG_MODULE] Bar={bar}, Zone={_tracker.CurrentZone?.Type}, POC={_tracker.POC:F2}");
```

**Check state transitions:**
```csharp
if (_tracker.CurrentZone?.BreakoutDirection != _lastBreakoutDirection)
{
    _log($"[STATE_CHANGE] Direction changed: {_lastBreakoutDirection} → {_tracker.CurrentZone?.BreakoutDirection}");
    _lastBreakoutDirection = _tracker.CurrentZone?.BreakoutDirection;
}
```

**Validate assumptions:**
```csharp
if (_tracker.CurrentZone == null)
{
    _log("[WARNING] CurrentZone is null, skipping module logic");
    return;
}
```

---

## Best Practices

### ✅ Do

- Keep modules small and focused (single strategy)
- Use dependency injection for all external dependencies
- Read tracker state via public properties only
- Log important events with structured tags (`[MODULE_TAG]`)
- Handle null cases gracefully (`_tracker.CurrentZone?.POC ?? 0`)
- Make modules hot-pluggable (enable/disable via settings)

### ❌ Don't

- Modify tracker state from modules
- Access private fields of tracker (use public API)
- Mix multiple strategies in one module
- Hard-code dependencies
- Ignore null checks (can cause crashes)
- Add heavy computation in event handlers (keep O(1) or O(log n))

---

## Performance Considerations

### Per-Bar Processing

**Good (O(1)):**
```csharp
public void OnBarUpdate(int bar, IndicatorCandle candle)
{
    var poc = _tracker.POC;  // O(1) property access
    if (candle.Close > poc + 10)
        RegisterTrigger(...);
}
```

**Bad (O(n)):**
```csharp
public void OnBarUpdate(int bar, IndicatorCandle candle)
{
    // O(n) loop every bar
    for (int i = 0; i < bar; i++)
    {
        var pastCandle = _getCandle(i);
        // ...
    }
}
```

### CumulativeTrades Processing

**Good (incremental):**
```csharp
public void OnLiveCumulativeTrade(CumulativeTrade trade)
{
    _totalVolume += trade.Volume;  // O(1) increment
}
```

**Bad (full recalculation):**
```csharp
public void OnLiveCumulativeTrade(CumulativeTrade trade)
{
    _totalVolume = _trades.Sum(t => t.Volume);  // O(n) every tick
}
```

---

## Module Lifecycle

```
ATAS Start
  ↓
FabioOrderFlow.OnCalculate(bar=0)
  ↓
Module Constructor
  ├─→ Inject dependencies
  ├─→ Initialize state
  └─→ Log "[MODULE] Initialized"
  ↓
OnCalculate(bar=1..N)
  ├─→ tracker.OnBarUpdate()
  └─→ module.OnBarUpdate()
      ├─→ Read tracker state
      ├─→ Execute strategy logic
      └─→ Log events
  ↓
(Optional) CumulativeTrades events
  └─→ module.OnLiveCumulativeTrade()
  ↓
ATAS Stop
```

---

## FAQ

**Q: Can a module modify balance zones?**  
A: No. Modules have read-only access to tracker state. Only `BalanceZoneTracker` manages zones.

**Q: How do I share data between modules?**  
A: Use tracker as shared context. Add public properties to expose data:
```csharp
// In BalanceZoneTracker.cs
public decimal LondonRange => (_context.CurrentZone?.High - _context.CurrentZone?.Low) ?? 0;

// In Module1
var range = _tracker.LondonRange;

// In Module2
var range = _tracker.LondonRange;  // Same data
```

**Q: Can I disable a module at runtime?**  
A: No. Modules are initialized at bar 0 based on settings. To disable, change setting and restart ATAS.

**Q: How do I access data from another module?**  
A: Modules shouldn't depend on each other directly. If needed, expose data via tracker or orchestrator.

**Q: What happens if module crashes?**  
A: Indicator crashes. Always validate inputs and handle null cases gracefully.

**Q: Can I use async/await in modules?**  
A: No. ATAS event handlers are synchronous. Keep logic synchronous and fast.

---

## Examples

### Example 1: Simple Filter Module

```csharp
public class SessionVolatilityFilter
{
    private readonly BalanceZoneTracker _tracker;
    private readonly Action<string> _log;
    
    public bool IsHighVolatilitySession { get; private set; }
    
    public SessionVolatilityFilter(BalanceZoneTracker tracker, Action<string> log)
    {
        _tracker = tracker;
        _log = log;
    }
    
    public void OnBarUpdate(int bar, IndicatorCandle candle)
    {
        var zone = _tracker.CurrentZone;
        if (zone == null) return;
        
        var range = zone.High - zone.Low;
        IsHighVolatilitySession = range > 25;  // 25 ticks threshold
        
        if (bar == zone.EndBar)
        {
            _log($"[VOLATILITY_FILTER] Session range: {range:F2}, High volatility: {IsHighVolatilitySession}");
        }
    }
}
```

### Example 2: Multi-Condition Entry Module

```csharp
public class CompositeEntryModule
{
    private readonly BalanceZoneTracker _tracker;
    private readonly SessionVolatilityFilter _volatilityFilter;
    
    public CompositeEntryModule(
        BalanceZoneTracker tracker,
        SessionVolatilityFilter volatilityFilter)
    {
        _tracker = tracker;
        _volatilityFilter = volatilityFilter;
    }
    
    public void OnBarUpdate(int bar, IndicatorCandle candle)
    {
        // Combine conditions from multiple sources
        if (_tracker.CurrentZone?.BreakoutDirection == BreakoutDirection.Bullish
            && _volatilityFilter.IsHighVolatilitySession
            && candle.Close > _tracker.POC + 5)
        {
            RegisterEntry(bar, candle);
        }
    }
}
```

---

## Resources

- [ARCHITECTURE.md](../ARCHITECTURE.md) - Full system architecture
- [BalanceZoneTracker API](../src/modules/shared/BalanceZoneTracker/BalanceZoneTracker.cs) - Core API reference
- [LondonMeanReversionModule](../src/modules/LondonMeanReversion/LondonMeanReversionModule.cs) - Reference implementation
