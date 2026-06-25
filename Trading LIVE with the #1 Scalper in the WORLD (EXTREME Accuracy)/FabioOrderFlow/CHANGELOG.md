# Changelog

## [2.0.0] - 2026-06-25

### ♻️ Major Refactoring - Modular Architecture

**Breaking Changes:**
- Codebase completamente ristrutturato da monolite a architettura modulare
- `BalanceZoneTracker` ora è un componente core puro (balance zones + sessioni)
- Logica mean reversion estratta in `LondonMeanReversionModule`

**Architecture:**
- ✅ Modular design: core tracker + strategy modules hot-pluggable
- ✅ Dependency injection: moduli ricevono tracker reference
- ✅ Event delegation: tracker delega eventi ai moduli
- ✅ Single responsibility: ogni componente ha un ruolo ben definito

**Components:**
```
FabioOrderFlow (orchestrator)
├── BalanceZoneTracker (core)
│   ├── Session detection
│   ├── Volume profile calculation
│   └── Balance zone state machine
└── LondonMeanReversionModule (strategy)
    ├── Rejection/fakeout detection
    ├── M5 trigger logic
    └── Aggression tracking
```

**Code Metrics:**
- `BalanceZoneTracker.cs`: 1819 → ~1000 lines (-45% cleanup)
- `LondonMeanReversionModule.cs`: 861 lines (nuovo modulo estratto)
- DLL size: 89.6KB → 74.7KB (-17%)

**Files:**
- Added: `modules/LondonMeanReversion/LondonMeanReversionModule.cs`
- Added: `modules/LondonMeanReversion/MeanReversionTriggerLog.cs`
- Added: `modules/LondonMeanReversion/MeanReversionOutcome.cs`
- Added: `modules/LondonMeanReversion/LiveSweepCandidate.cs`
- Added: `ARCHITECTURE.md` (full architectural documentation)
- Updated: `README.md` (quick start + module guide)
- Cleaned: `BalanceZoneTracker.cs` (removed all MR logic)

**Documentation:**
- `ARCHITECTURE.md`: complete design patterns, data flow, extensibility guide
- `README.md`: quick start, feature overview, dev guide
- `CHANGELOG.md`: this file

### 🎯 Functional Equivalence

**No behavior changes:**
- Balance zone detection: identical logic
- Mean reversion triggers: same detection algorithm
- Aggression tracking: identical CumulativeTrades processing
- Visual rendering: same rectangles + POC lines

**Build status:**
- ✅ 0 errors
- ⚠️ 8 warnings (unused fields in MR module - to be cleaned)

---

## [1.x] - Pre-refactoring

### Previous Architecture (Monolith)

**Single file approach:**
- `BalanceZoneTracker.cs`: 1819 lines
  - Balance zone logic
  - Mean reversion logic (embedded)
  - NY session logic (embedded)
  - All state management
  - Mixed responsibilities

**Issues:**
- ❌ Low cohesion: MR logic mixed with core tracker
- ❌ No modularity: impossible to disable/swap strategies
- ❌ Hard to test: tightly coupled components
- ❌ Poor extensibility: adding new strategies required editing core

---

## Migration Guide

### For Users

**No action required:**
- Same indicator name: `FabioOrderFlow`
- Same settings: `EnableLondonMeanReversion`, `EnableLiveFootprintFirst`
- Same visual output: balance zones + POC lines
- Same log output: `[PROFILE_PREVIEW]`, `[MR_TRIGGER_M5]`, etc.

**Deployment:**
```bash
cd src/
dotnet build -c Release
./deploy.bat
```

### For Developers

**Reading tracker state (modules):**
```csharp
// OLD (direct field access - now private)
_lastPreviewPoc  // ❌ not accessible

// NEW (public API)
_tracker.LastPreviewPoc  // ✅ exposed via property
_tracker.CurrentZone     // ✅ read-only access
```

**Adding event handlers:**
```csharp
// OLD (embedded in tracker)
private void UpdateMeanReversionOutcomes(...)  // ❌ in BalanceZoneTracker

// NEW (module method)
public void OnBarUpdate(int bar, IndicatorCandle candle)  // ✅ in Module
{
    // Strategy logic here
}
```

**Module registration pattern:**
```csharp
// In FabioOrderFlow.cs
_balanceTracker = new BalanceZoneTracker(...);

if (EnableLondonMeanReversion)
{
    _meanReversionModule = new LondonMeanReversionModule(_balanceTracker, ...);
    _balanceTracker.SetMeanReversionModule(_meanReversionModule);
}
```

---

## Development Stats

### Refactoring Phases

**Phase 1: Planning**
- ✅ Identify module boundaries
- ✅ Design public APIs
- ✅ Plan data flow

**Phase 2: Extraction**
- ✅ Extract MR classes (TriggerLog, Outcome, Sweep)
- ✅ Extract MR methods (~30 methods)
- ✅ Create `LondonMeanReversionModule.cs`

**Phase 3: Integration**
- ✅ Implement module registration pattern
- ✅ Delegate events (CumulativeTrades)
- ✅ Clean core tracker from MR code
- ✅ Test build + deploy

**Phase 4: Documentation**
- ✅ Write `ARCHITECTURE.md`
- ✅ Update `README.md`
- ✅ Create `CHANGELOG.md`

### Commits

- `feat: extract MR module - Phase 2a DONE` (extraction complete)
- `refactor: complete MR extraction - Phase 2b DONE` (integration complete)
- `refactor: Phase 3a cleanup - remove MR code from core tracker` (cleanup complete)
- `docs: add comprehensive architecture documentation` (this commit)

---

## Future Roadmap

### Planned Modules

1. **PostLondonImpulseModule** (Modello 1 trend-following)
   - Entry su aggression clusters in low volume nodes
   - Target: POC della balance zone precedente
   - Status: design phase

2. **SessionVolatilityFilter**
   - ATR-based volatility filtering
   - Session-specific volume thresholds

3. **MultiTimeframeContext**
   - Higher timeframe trend alignment
   - Key level identification

### Technical Debt

- [ ] Remove unused fields in `LondonMeanReversionModule` (CS0649 warnings)
- [ ] Add unit tests for `BalanceZoneTracker` state machine
- [ ] Add unit tests for `SessionDetector` timezone conversions
- [ ] Refactor `LondonMeanReversionModule` → split rejection + aggression logic
- [ ] Performance profiling su large historical datasets
- [ ] Multi-zone tracking (storico ultimi N balance zones)

### Documentation

- [ ] Add sequence diagrams for data flow
- [ ] Add state machine diagrams for balance zones
- [ ] Create tutorial video (module development)
- [ ] Add troubleshooting guide
