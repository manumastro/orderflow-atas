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

### Current State (Integrated)

```
FabioOrderFlow.cs
└── BalanceZoneTracker.cs (1835 lines)
    ├── Core (session, profile, breakout)
    └── London Mean Reversion (integrated)
```

### Target State (Modular)

```
FabioOrderFlow.cs
├── BalanceZoneTracker (core only, ~800 lines)
│   ├── Session management
│   ├── Profile calculation
│   ├── Breakout detection
│   └── Visual rendering
│
├── LondonMeanReversionModule (~600 lines)
│   ├── Trigger detection
│   ├── Aggression confirmation
│   ├── Exit management
│   └── Footprint-first (optional)
│
└── PostLondonImpulseModule (future)
    ├── Impulse profiling
    ├── Low volume nodes
    ├── Aggression clusters
    └── Entry management
```

---

## 📊 Sessions & Timeline

**London Session (08:00-16:00 London):**
- Balance zone building
- Profile preview live updates
- Mean reversion triggers active

**NY Overlap (09:30-11:00 NY = 14:30-16:00 London):**
- Breakout detection window (1.5h)
- State transition: Balance → OutOfBalance

**Post-London (16:00+ London):**
- Mean reversion continues (if out-of-balance)
- Future: Impulse following active

---

## 🎯 Quick Reference

| Model | Status | Session | Parameter | Performance |
|-------|--------|---------|-----------|-------------|
| London Mean Reversion | ✅ Implemented | London 08:00-16:00 | `EnableLondonMeanReversion = true` | 15 entry, +408.5 pts |
| Post-London Impulse | ⏳ Future | Post-breakout | `EnablePostLondonImpulse = false` | TBD |

---

## 📖 Related Documents

- **Project Overview:** `../../AGENTS.md`
- **Session Analysis:** `../../CHIAREZZA-DEFINITIVA.md`
- **Refactoring Summary:** `../../REFACTORING-SUMMARY.md`
- **Module Specs:** `../src/modules/<Module>/<Module>.md`

---

**Last Updated:** 2026-06-25
