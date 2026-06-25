# BalanceZoneTracker - Code Analysis

**File:** `FabioOrderFlow/src/modules/shared/BalanceZoneTracker/BalanceZoneTracker.cs`  
**Total Lines:** 1835  
**Status:** Monolithic (Core + Mean Reversion integrated)

---

## 📊 Component Breakdown

### Core Components (Shared, ~800 lines estimated)

#### Data Structures
```csharp
// Lines 9-63
internal enum MarketState                  // State machine
internal enum BalanceType                  // London vs Consolidation
internal enum BreakoutDirection            // Bullish/Bearish
internal sealed class BalanceZone          // Balance zone data
internal sealed class MarketContext        // Current market state
```

#### Core Logic
- Session detection (London/NY)
- Profile calculation (POC/VAH/VAL)
- Breakout detection
- State machine management
- Visual rendering (rectangles, lines)

#### Member Variables (Core)
```csharp
private readonly Indicator _indicator;
private readonly MarketContext _context;
private readonly Action<string> _log;
private readonly Func<int, IndicatorCandle> _getCandle;
private readonly List<DrawingRectangle> _rectangles;
private readonly List<LineTillTouch> _lines;
```

#### Key Methods (Core)
- `OnBarUpdate()` - Main bar processing
- `StartLondonSession()` - Begin London tracking
- `UpdateLondonProfile()` - Update profile live
- `FinalizeLondonSession()` - Complete session
- `StartNewYorkSession()` - NY overlap tracking
- `CalculatePOC()` - Volume profile calculation
- `IsInLondonSession()` - Session check
- `IsInNewYorkSession()` - Session check

---

### Mean Reversion Components (Specific, ~600 lines estimated)

#### Data Structures
```csharp
// Lines 65-120
internal sealed class MeanReversionTriggerLog    // Trigger metadata
internal sealed class MeanReversionOutcome       // Entry/exit tracking
internal sealed class LiveSweepCandidate         // Footprint-first state
```

#### Member Variables (Mean Reversion)
```csharp
private readonly List<MeanReversionTriggerLog> _meanReversionTriggerLogs;
private readonly List<MeanReversionOutcome> _meanReversionOutcomes;
private readonly HashSet<string> _loggedAggressionCandidateKeys;
private readonly bool _enableLiveFootprintFirst;

// Rejection tracking
private int _lastLowRejectionCandidateBar;
private int _lastHighRejectionCandidateBar;
private decimal _lastLowRejectionHigh/Close/Low/Delta;
private decimal _lastHighRejectionHigh/Close/Low/Delta;
private bool _lowRejectionPocReclaimed;
private bool _highRejectionPocLost;

// Footprint-first state
private DateTime? _liveLowSweepTimeUtc;
private DateTime? _liveHighSweepTimeUtc;
private LiveSweepCandidate? _activeLongSweep;
private LiveSweepCandidate? _activeShortSweep;
```

#### Key Methods (Mean Reversion)
**Trigger Detection:**
- `LogPotentialRejection()` - Detect rejection candles
- `LogMeanReversionTriggerIfNeeded()` - Check POC reclaim/loss
- `RegisterMeanReversionTrigger()` - Create trigger log

**Aggression Confirmation:**
- `TryLogLiveLongAggression()` - Live long entry
- `TryLogLiveShortAggression()` - Live short entry
- `LogHistoricalAggressionConfirmation()` - Historical scan
- `IsHistoricalAggressionInsideValue()` - Validation

**Exit Management:**
- `EvaluateLongOutcome()` - Long position tracking
- `EvaluateShortOutcome()` - Short position tracking

**Footprint-First (Optional):**
- `IsHighSweepTrade()` - Detect high sweep
- `IsLowSweepTrade()` - Detect low sweep
- `IsRejectionTrade()` - Detect rejection
- `IsAggressionEntryTrade()` - Detect entry aggression
- `ProcessLiveHighSweep()` - Register high sweep
- `ProcessLiveLowSweep()` - Register low sweep
- `ProcessLiveRejection()` - Register rejection
- `ProcessFootprintEntry()` - Register entry
- `CheckFootprintTimeouts()` - Cleanup stale state

**Public Properties (Mean Reversion):**
```csharp
public List<MeanReversionTriggerLog> MeanReversionTriggerLogs { get; }
public List<MeanReversionOutcome> MeanReversionOutcomes { get; }
```

---

## 🔍 Integration Points

### Mean Reversion Dependencies on Core

**From BalanceZone:**
- `CurrentZone` - Current balance zone
- `High/Low/POC/VAH/VAL` - Price levels
- `IsReady` - Zone validity

**From State Machine:**
- `State` - Market state (Balance/OutOfBalance)
- Session checks (`IsInLondonSession()`, `IsInNewYorkSession()`)

**From Profile Preview:**
- `LastPreviewPoc/Vah/Val` - Live profile levels during London

**Shared:**
- `_log` - Logging function
- `_getCandle()` - Bar data access

### Core Dependencies on Mean Reversion

**NONE!** Core doesn't depend on Mean Reversion logic.

---

## 📈 Code Statistics

**Total:** 1835 lines

**Estimated Breakdown:**
- Core shared: ~800 lines (44%)
- Mean Reversion: ~600 lines (33%)
- Data structures: ~120 lines (7%)
- Imports/whitespace: ~315 lines (17%)

**Mean Reversion References:** 63 occurrences of MeanReversion/LiveSweep/Footprint keywords

---

## 🎯 Extraction Feasibility

### Easy to Extract ✅
- Data structures (classes) - Clean copy
- Methods - Self-contained logic
- State variables - Clear ownership

### Requires Careful Handling ⚠️
- `_log` function - Pass as dependency
- `_getCandle()` - Pass as dependency
- Profile preview levels - Expose as properties
- Session checks - Keep in core, call from module

### Benefits of Extraction
1. **Clarity** - Separate concerns clearly
2. **Testability** - Test modules independently
3. **Maintainability** - Edit without affecting core
4. **Reusability** - Core can be used by other modules
5. **Conditional loading** - Enable/disable modules easily

### When to Extract
- **Now:** If working on another model (Post-London Impulse)
- **Later:** If current integrated version works perfectly (it does!)
- **Never:** If code stays stable and unchanging

**Current Status:** Working perfectly integrated. No urgent need to extract unless implementing second model.

---

## 🔄 Proposed Extraction (Future)

### Step 1: Create LondonMeanReversionModule
```csharp
internal sealed class LondonMeanReversionModule
{
    private readonly BalanceZoneTracker _balanceTracker;
    private readonly Action<string> _log;
    private readonly Func<int, IndicatorCandle> _getCandle;
    private readonly bool _enableLiveFootprintFirst;
    
    // Move all MR data structures and methods here
}
```

### Step 2: Clean BalanceZoneTracker
- Remove MR classes (lines 65-120)
- Remove MR member variables
- Remove MR methods
- Keep only core functionality
- Result: ~800 lines

### Step 3: Update FabioOrderFlow
```csharp
private BalanceZoneTracker _balanceTracker;
private LondonMeanReversionModule? _meanReversionModule;

if (EnableLondonMeanReversion)
{
    _meanReversionModule = new LondonMeanReversionModule(
        _balanceTracker, Log, GetCandle, EnableLiveFootprintFirst);
}
```

### Step 4: Test
- Verify identical behavior
- Compare log outputs
- Validate performance metrics

---

## 📝 Current Configuration

```csharp
// In FabioOrderFlow.cs
public bool EnableLondonMeanReversion { get; set; } = true;
public bool EnablePostLondonImpulse { get; set; } = false;
public bool EnableLiveFootprintFirst { get; set; } = true;
```

**Note:** `EnableLondonMeanReversion` currently does nothing - logic is always active when BalanceZoneTracker exists.

---

**Conclusion:** BalanceZoneTracker is a well-functioning monolith. Mean Reversion logic is integrated but clearly separable. Extraction recommended only when implementing second model or when module independence becomes valuable.
