# London Mean Reversion Module

**Status:** Logic implemented in `shared/BalanceZoneTracker/BalanceZoneTracker.cs` (pending extraction)  
**Target Location:** `LondonMeanReversion/LondonMeanReversionModule.cs`  
**Lines:** ~600 (estimated after extraction)

---

## Overview

Fade London fakeouts back to POC using sweep → rejection → aggression confirmation pattern.

**Active during:**
- London session (08:00-16:00 London) with live profile preview
- Post-breakout (until new balance established)

---

## Architecture

### Current State (Integrated)
All logic is embedded in `BalanceZoneTracker.cs` (~1835 lines):
- Lines 65-120: Classes (MeanReversionTriggerLog, MeanReversionOutcome, LiveSweepCandidate)
- Lines 293-350: Live aggression detection methods
- Lines 930-1050: Trigger detection methods
- Lines 1125-1350: Exit management methods
- Lines 1678-1832: Footprint-first detection (optional)

### Target State (Extracted)
Separate module `LondonMeanReversionModule.cs` with clean interfaces:

```csharp
public class LondonMeanReversionModule
{
    public LondonMeanReversionModule(
        BalanceZoneTracker balanceTracker,
        Action<string> log,
        Func<int, IndicatorCandle> getCandle,
        bool enableLiveFootprintFirst = true)
    
    public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar)
    public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades)
    public void OnLiveCumulativeTrade(CumulativeTrade trade)
}
```

---

## Components to Extract

### 1. Data Structures
- `MeanReversionTriggerLog` - Trigger metadata
- `MeanReversionOutcome` - Entry tracking and exit management
- `LiveSweepCandidate` - Footprint-first sweep/rejection state

### 2. Trigger Detection
- `LogPotentialRejection()` - Detect rejection candles
- `LogMeanReversionTriggerIfNeeded()` - Register triggers
- `RegisterMeanReversionTrigger()` - Create trigger logs

### 3. Aggression Confirmation
- `TryLogLiveLongAggression()` - Live long entry confirmation
- `TryLogLiveShortAggression()` - Live short entry confirmation
- `LogHistoricalAggressionConfirmation()` - Historical scan
- `IsHistoricalAggressionInsideValue()` - Validation helper
- `LogAggressionConfirmation()` - Entry registration

### 4. Exit Management
- `EvaluateLongOutcome()` - Long position exit logic
- `EvaluateShortOutcome()` - Short position exit logic

### 5. Footprint-First (Optional)
- `IsHighSweepTrade()` - Detect high sweep
- `IsLowSweepTrade()` - Detect low sweep
- `IsRejectionTrade()` - Detect rejection after sweep
- `IsAggressionEntryTrade()` - Detect entry aggression
- `ProcessLiveHighSweep()` - Register high sweep
- `ProcessLiveLowSweep()` - Register low sweep
- `ProcessLiveRejection()` - Register rejection
- `ProcessFootprintEntry()` - Register entry
- `CheckFootprintTimeouts()` - Cleanup stale sweeps

### 6. State Variables
```csharp
private int _lastLowRejectionCandidateBar;
private int _lastHighRejectionCandidateBar;
private bool _lowRejectionPocReclaimed;
private bool _highRejectionPocLost;
private DateTime? _liveLowSweepTimeUtc;
private DateTime? _liveHighSweepTimeUtc;
private List<MeanReversionTriggerLog> _meanReversionTriggerLogs;
private List<MeanReversionOutcome> _meanReversionOutcomes;
private LiveSweepCandidate? _activeLongSweep;
private LiveSweepCandidate? _activeShortSweep;
```

---

## Extraction Plan

**When:** After implementing Post-London Impulse module, or when clean separation is needed.

**Steps:**
1. Create `LondonMeanReversionModule.cs` with class skeleton
2. Copy data structures (classes)
3. Copy and adapt methods (update dependencies)
4. Copy state variables
5. Update `FabioOrderFlow.cs` to instantiate module
6. Pass dependencies (BalanceZoneTracker, log, getCandle)
7. Delegate calls from main indicator
8. Remove extracted code from BalanceZoneTracker
9. Test: verify identical behavior
10. Deploy and validate on live

**Testing:** Must produce identical results to current integrated version.

---

## Dependencies

**From BalanceZoneTracker:**
- `CurrentZone` - Current balance zone
- `LastPreviewPoc/Vah/Val` - Live profile levels
- `State` - Market state (Balance/OutOfBalance)
- `IsInNewYorkSession()` - Session check

**From FabioOrderFlow:**
- `Log()` - Logging function
- `GetCandle()` - Bar data accessor
- `RequestCumulativeTrades()` - Historical trades
- `OnCumulativeTrade()` - Live trade events

---

## Performance

**Current (integrated):**
- 15 entry, win rate 57.1%, +408.5 points net
- Tested on historical and live data

**After extraction:**
- Must maintain identical performance
- No regression in trigger detection
- Same entry/exit behavior

---

## Configuration

```csharp
// In FabioOrderFlow.cs
public bool EnableLondonMeanReversion { get; set; } = true;
public bool EnableLiveFootprintFirst { get; set; } = true;

// Module instantiation
if (EnableLondonMeanReversion)
{
    _meanReversionModule = new LondonMeanReversionModule(
        _balanceTracker,
        Log,
        GetCandle,
        EnableLiveFootprintFirst);
}
```

---

## Full Documentation

**Strategy specification:** `../../docs/LondonMeanReversion.md`  
**Code location (current):** `shared/BalanceZoneTracker/BalanceZoneTracker.cs`  
**Code location (target):** `LondonMeanReversion/LondonMeanReversionModule.cs`

---

**Status:** Pending extraction - works perfectly in integrated form.
