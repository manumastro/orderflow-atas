# Modules - FabioOrderFlow Architecture

This folder contains the modular architecture of the FabioOrderFlow indicator.

---

## 📁 Structure

```
modules/
├── shared/                           # Core shared components
│   └── BalanceZoneTracker/          # Session, profile, breakout detection
│
├── LondonMeanReversion/             # Modello 2 (IMPLEMENTED)
│   └── [will contain extracted code]
│
└── PostLondonImpulse/               # Modello 1 (FUTURE)
    ├── ImpulseProfiler/
    ├── LowVolumeNodeDetector/
    ├── AggressionDetector/
    ├── TradeManager/
    ├── ConfirmationLayer/
    └── VisualRenderer/
```

---

## 🔧 Shared Components

### BalanceZoneTracker (Core)

**Path:** `shared/BalanceZoneTracker/`  
**Status:** ✅ Implemented (integrated with London Mean Reversion)  
**Lines:** ~1835 (will be split into core ~800 + MR module ~600)

**Responsibilities:**
- Session management (London/NY detection)
- Profile calculation (POC/VAH/VAL)
- Breakout detection (NY overlap window)
- Market state machine (Balance → OutOfBalance)
- Visual rendering (balance zones, levels)

**Currently also contains:**
- Mean reversion trigger detection (will be extracted)
- Aggression confirmation (will be extracted)
- Exit management (will be extracted)
- Footprint-first detection (will be extracted)

---

## ✅ London Mean Reversion Module (Modello 2)

**Path:** `LondonMeanReversion/`  
**Status:** Logic implemented in BalanceZoneTracker (pending extraction)  
**Lines:** ~600 (when extracted)

**Responsibilities:**
- Trigger detection (sweep → rejection → POC reclaim/loss)
- Aggression confirmation (volume ≥20 trades)
- Exit management (Target2/Stop automatic)
- Optional: Footprint-first real-time detection

**Implementation plan:**
1. Extract trigger detection methods from BalanceZoneTracker
2. Extract aggression confirmation methods
3. Extract exit management methods
4. Extract footprint-first methods
5. Create LondonMeanReversionModule.cs
6. Update FabioOrderFlow.cs to instantiate module
7. Clean BalanceZoneTracker to core only

---

## ⏳ Post-London Impulse Module (Modello 1)

**Path:** `PostLondonImpulse/`  
**Status:** Documented, not implemented  
**Lines:** TBD

### Submodules (Implementation Order)

#### 1. ImpulseProfiler
Profile impulse moves after breakout detection.

#### 2. LowVolumeNodeDetector
Detect low volume price levels (entry targets).

#### 3. AggressionDetector
Detect aggression clusters (entry confirmation).

#### 4. TradeManager
Entry/exit trade management.

#### 5. ConfirmationLayer
Multi-timeframe/additional confirmation filters.

#### 6. VisualRenderer
Visual indicators for impulse trades.

**Each submodule has:**
- `<Module>.md` - Operational design and specifications
- `<Module>.cs` - Implementation (when ready)

---

## 🔄 Refactoring Status

### Current State
```
FabioOrderFlow.cs
└── BalanceZoneTracker.cs (monolithic, 1835 lines)
    ├── Core functionality
    └── London Mean Reversion (integrated)
```

### Target State
```
FabioOrderFlow.cs
├── BalanceZoneTracker.cs (core only, ~800 lines)
├── LondonMeanReversionModule.cs (~600 lines)
└── PostLondonImpulseModule.cs (future)
```

---

## 📖 Documentation

- **Main docs:** `FabioOrderFlow/docs/`
  - `LondonMeanReversion.md` - Modello 2 spec
  - `PostLondonImpulse.md` - Modello 1 spec
  - `README.md` - Overview

- **Module docs:** `modules/<Module>/<Module>.md`
  - Technical specifications
  - Input/output contracts
  - State machines
  - Validation criteria

---

## 🎯 Rules

Before implementing or modifying a module:

1. Read `modules/<Module>/<Module>.md` for specifications
2. Update design docs if decisions change
3. Implement code in the same folder
4. Verify `dotnet build -c Release` passes
5. Test on historical data
6. Update central roadmap only for global decisions

---

**Last Updated:** 2026-06-25
