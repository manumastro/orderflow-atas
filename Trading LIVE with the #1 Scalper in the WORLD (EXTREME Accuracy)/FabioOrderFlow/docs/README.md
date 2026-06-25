# FabioOrderFlow - Documentation

**Unified order flow indicator combining two complementary trading models.**

---

## 📚 Models

### ✅ London Mean Reversion (Modello 2) - IMPLEMENTED

**File:** [`LondonMeanReversion.md`](LondonMeanReversion.md)

**Strategy:** Fade London fakeouts back to POC  
**Session:** London (08:00-16:00 London) with live profile preview  
**Entry:** Sweep → Rejection → Aggression confirmation  
**Exit:** Target2/Stop automatic management  
**Performance:** 15 entry, 57.1% win rate, +408.5 points net

**Parameter:** `EnableLondonMeanReversion = true` (default)

**Implementation:**
- Location: `src/modules/BalanceZoneTracker/BalanceZoneTracker.cs` (integrated)
- Lines: ~1835 (core + mean reversion mixed)
- Status: Fully operational, tested on live

**Optional Feature:**
- `EnableLiveFootprintFirst = true` - Real-time footprint detection (sweep→rejection→entry)

---

### ⏳ Post-London Impulse (Modello 1) - FUTURE

**File:** [`PostLondonImpulse.md`](PostLondonImpulse.md)

**Strategy:** Follow impulse after breakout to low volume nodes  
**Session:** Post-London breakout (NY overlap)  
**Entry:** Aggression clusters in low volume nodes  
**Exit:** POC of previous balance zone  
**Status:** Documented, not implemented

**Parameter:** `EnablePostLondonImpulse = false` (default)

**Planned Implementation:**
- Location: `src/modules/PostLondonImpulse/`
- Modules: ImpulseProfiler, LowVolumeNodeDetector, AggressionDetector, TradeManager
- Status: Specifications complete, awaiting implementation

---

## 🏗️ Architecture

### Current Implementation

```
FabioOrderFlow.cs (main indicator)
└── BalanceZoneTracker.cs (1835 lines, monolithic)
    ├── Core (~800 lines)
    │   ├── Session management (London/NY)
    │   ├── Profile calculation (POC/VAH/VAL)
    │   ├── Breakout detection
    │   ├── State machine
    │   └── Visual rendering
    │
    └── London Mean Reversion (~600 lines, integrated)
        ├── Trigger detection
        ├── Aggression confirmation
        ├── Exit management
        └── Footprint-first (optional)
```

**Analysis:** See [`src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md`](../src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md)

**Status:** Working perfectly as monolithic. Extraction to separate modules optional (recommended when implementing Post-London Impulse).

---

## 📊 Sessions & Timeline

**London Session (08:00-16:00 London):**
- Balance zone building
- Profile preview with live updates
- Mean reversion triggers active

**NY Overlap (14:30-16:00 London = 09:30-11:00 NY):**
- Breakout detection window (1.5h)
- State transition: Balance → OutOfBalance

**Post-London (16:00+ London):**
- Mean reversion continues if out-of-balance
- Future: Impulse following (Post-London Impulse strategy)

**Reference:** [`../../CHIAREZZA-DEFINITIVA.md`](../../CHIAREZZA-DEFINITIVA.md)

---

## 🎯 Quick Reference

| Strategy | Status | Session | Parameter | Implementation |
|----------|--------|---------|-----------|----------------|
| London Mean Reversion | ✅ Working | London 08:00-16:00 | `EnableLondonMeanReversion = true` | Integrated in BalanceZoneTracker |
| Post-London Impulse | ⏳ Documented | Post-breakout | `EnablePostLondonImpulse = false` | To be implemented |

**Configuration:**
```csharp
public bool EnableLondonMeanReversion { get; set; } = true;   // Always active (parameter not conditional yet)
public bool EnablePostLondonImpulse { get; set; } = false;    // Not implemented
public bool EnableLiveFootprintFirst { get; set; } = true;    // Optional real-time detection
```

---

## 📖 Related Documents

- **Project Overview:** [`../../AGENTS.md`](../../AGENTS.md)
- **Session Analysis:** [`../../CHIAREZZA-DEFINITIVA.md`](../../CHIAREZZA-DEFINITIVA.md)
- **Module Architecture:** [`../src/modules/README.md`](../src/modules/README.md)
- **Code Analysis:** [`../src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md`](../src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md)
- **Module Specs:** `../src/modules/<Module>/<Module>.md`

---

**Last Updated:** 2026-06-25
