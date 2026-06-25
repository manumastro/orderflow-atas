# FabioOrderFlow — Architecture

## Overview

FabioOrderFlow è un indicatore ATAS modulare per l'analisi di order flow sui futures (NQ/ES). L'architettura è basata su una separazione netta tra:

- **Core tracker**: gestione balance zones e sessioni
- **Moduli di strategia**: logiche di trading indipendenti e hot-pluggable

## Project Structure

```
FabioOrderFlow/
├── src/
│   ├── FabioOrderFlow.cs              # Entry point, orchestrazione moduli
│   ├── modules/
│   │   ├── shared/
│   │   │   ├── BalanceZoneTracker/    # Core: balance zones + sessioni
│   │   │   │   ├── BalanceZoneTracker.cs
│   │   │   │   ├── BalanceZone.cs
│   │   │   │   └── MarketContext.cs
│   │   │   └── SessionDetector/       # Rilevamento sessioni trading
│   │   │       ├── SessionDetector.cs
│   │   │       ├── TradingSession.cs
│   │   │       └── MarketTimeZones.cs
│   │   └── LondonMeanReversion/       # Strategy: mean reversion Londra
│   │       ├── LondonMeanReversionModule.cs
│   │       ├── MeanReversionTriggerLog.cs
│   │       ├── MeanReversionOutcome.cs
│   │       └── LiveSweepCandidate.cs
│   └── deploy.bat
└── ARCHITECTURE.md                     # Questo documento
```

## Component Roles

### 1. FabioOrderFlow.cs (Orchestrator)

**Responsabilità:**
- Inizializzazione moduli
- Routing eventi ATAS ai moduli appropriati
- Coordinamento CumulativeTrades data flow
- Rendering globale (chart overlay)

**Pattern:**
- Dependency Injection: passa riferimenti tra moduli
- Event routing: `OnCalculate(bar, type)` → `tracker.OnBarUpdate()`
- Module registration: `tracker.SetMeanReversionModule(module)`

**Codice chiave:**
```csharp
protected override void OnCalculate(int bar, decimal value)
{
    if (bar == 0)
    {
        _balanceTracker = new BalanceZoneTracker(...);
        
        if (EnableLondonMeanReversion)
        {
            _meanReversionModule = new LondonMeanReversionModule(...);
            _balanceTracker.SetMeanReversionModule(_meanReversionModule);
        }
    }
    
    _balanceTracker.OnBarUpdate(bar, candle, CurrentBar);
    
    if (ChartInfo.ChartType == "CumulativeTrades")
    {
        _balanceTracker.OnHistoricalCumulativeTrades(cumulativeTrades);
        _meanReversionModule?.OnLiveCumulativeTrade(trade);
    }
}
```

### 2. BalanceZoneTracker (Core)

**Responsabilità:**
- Rilevamento sessioni London/NY (via `SessionDetector`)
- Costruzione volume profile (POC, VAH, VAL, 70% value area)
- State machine balance zones (Building → Ready → Broken)
- Rendering zone rettangolari + POC lines
- Esposizione dati per moduli strategy

**NON gestisce:**
- Logiche di trading specifiche
- Trigger di entry/exit
- Aggression tracking
- Mean reversion logic

**Public API per moduli:**
```csharp
// State access
public BalanceZone? CurrentZone { get; }
public decimal? POC { get; }
public decimal LastPreviewPoc { get; }
public decimal LastPreviewVah { get; }
public decimal LastPreviewVal { get; }

// Module registration
public void SetMeanReversionModule(LondonMeanReversionModule module);

// Event delegation (pass-through to modules)
public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades);
public void OnLiveCumulativeTrade(CumulativeTrade trade);
```

**Architettura interna:**
```
MarketContext (state machine)
├── State: NoZone | BuildingSessionProfile | BalanceReady | Broken
├── CurrentZone: BalanceZone { POC, VAH, VAL, Profile, BreakoutDirection }
└── PendingDirection, ConsecutiveOutsideCloses

Session Detection (London/NY)
├── StartLondonSession() → State = BuildingSessionProfile
├── UpdateLondonProfile() → accumula volume, calcola preview
├── ProcessLondonClose() → State = BalanceReady
└── StartNewYorkSession() → monitora breakout

Profile Calculation
├── TryCalculateProfilePreview() → POC + 70% value area
└── CalculateVolumeProfile() → aggrega livelli prezzo

Visual Rendering
├── DrawBalanceZone() → Rectangle (rosso/blu per direction)
├── DrawPocLine() → POC horizontal line
└── UpdateBalanceZoneColors() → dynamic color on breakout
```

### 3. LondonMeanReversionModule (Strategy)

**Responsabilità:**
- Mean reversion logic su balance zones di Londra
- Rilevamento fakeout/rejection su sweep di high/low
- Trigger M5 quando prezzo ritorna verso POC
- Tracking aggression via CumulativeTrades
- Logging diagnostico trigger/outcome

**Dipendenze:**
- `BalanceZoneTracker`: legge `CurrentZone`, `LastPreviewPoc/Vah/Val`
- `CumulativeTrades`: analizza bid/ask aggression in tempo reale

**Public API:**
```csharp
// Data access (read-only)
public List<MeanReversionTriggerLog> MeanReversionTriggerLogs { get; }
public List<MeanReversionOutcome> MeanReversionOutcomes { get; }

// Event handlers (called by tracker or orchestrator)
public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades);
public void OnLiveCumulativeTrade(CumulativeTrade trade);
```

**Logica interna:**
```
Rejection Detection (historical)
├── LogPotentialRejection() → identifica sweep con rejection candle
├── RegisterMeanReversionTrigger() → salva in MeanReversionTriggerLog
└── LogHistoricalAggressionConfirmation() → verifica aggression su sweep

Live Aggression Tracking
├── TryLogLiveLongAggression() → controlla ask > bid sotto VAL
├── TryLogLiveShortAggression() → controlla bid > ask sopra VAH
└── GetCandleVolumeDiagnostics() → estrae bid/ask/delta da candle

Footprint-First (experimental)
├── Detect sweep → ProcessLiveHighSweep() / ProcessLiveLowSweep()
├── Detect rejection → ProcessLiveRejection()
└── Detect entry aggression → ProcessFootprintEntry()

Outcome Tracking
└── UpdateMeanReversionOutcomes() → verifica se POC è stato raggiunto
```

**Pattern di integrazione:**
```csharp
// In FabioOrderFlow.cs
_balanceTracker.SetMeanReversionModule(_meanReversionModule);

// In BalanceZoneTracker.cs
public void OnLiveCumulativeTrade(CumulativeTrade trade)
{
    _meanReversionModule?.OnLiveCumulativeTrade(trade); // Delegation
}
```

### 4. SessionDetector (Shared Utility)

**Responsabilità:**
- Rilevamento sessioni London (08:00-16:00 GMT)
- Rilevamento sessioni New York (14:30-21:00 GMT)
- Conversione timezone (UTC ↔ London ↔ New York)

**Design pattern:**
- Stateless utility (no instance state)
- Pure functions: `GetCurrentSession(DateTime utc) → TradingSession?`

## Data Flow

### Initialization (bar 0)

```
ATAS OnCalculate(bar=0)
  ↓
FabioOrderFlow.OnCalculate()
  ↓
Create BalanceZoneTracker
  ↓
Create LondonMeanReversionModule (if enabled)
  ↓
tracker.SetMeanReversionModule(module)
```

### Per-bar Updates

```
ATAS OnCalculate(bar)
  ↓
FabioOrderFlow.OnCalculate()
  ↓
tracker.OnBarUpdate(bar, candle, currentBar)
  ↓
  ├─→ SessionDetector: check session transitions
  ├─→ London session: UpdateLondonProfile() → calc preview POC/VAH/VAL
  ├─→ London close: ProcessLondonClose() → finalize zone
  ├─→ NY session: check breakout → update state
  └─→ Visual: DrawBalanceZone(), DrawPocLine()
```

### CumulativeTrades Flow (live + historical)

```
ATAS CumulativeTrades chart
  ↓
FabioOrderFlow.OnCalculate()
  ↓
Historical replay:
  tracker.OnHistoricalCumulativeTrades(allTrades)
    ↓
  module.OnHistoricalCumulativeTrades(allTrades)
    ↓
  LogHistoricalAggressionConfirmation() → verifica aggression su trigger

Live tick-by-tick:
  FabioOrderFlow gets CumulativeTrade from chart
    ↓
  module.OnLiveCumulativeTrade(trade)
    ↓
  TryLogLiveLongAggression() / TryLogLiveShortAggression()
```

## Configuration (Settings)

**FabioOrderFlow.cs Parameter Groups:**

```csharp
// Module enable/disable
[Display(Name = "Enable London Mean Reversion", GroupName = "Modules")]
public bool EnableLondonMeanReversion { get; set; } = true;

// Footprint-first experimental
[Display(Name = "Enable Live Footprint-First", GroupName = "Modules")]
public bool EnableLiveFootprintFirst { get; set; } = false;
```

**Moduli autonomi:**
- `BalanceZoneTracker`: nessun parametro esterno, configurazione hard-coded
- `LondonMeanReversionModule`: riceve `EnableLiveFootprintFirst` via constructor

## Extensibility

### Aggiungere un nuovo modulo strategy

**Step 1:** Creare modulo in `modules/<NomeModulo>/`

```csharp
public class PostLondonImpulseModule
{
    private readonly BalanceZoneTracker _tracker;
    private readonly Action<string> _log;
    
    public PostLondonImpulseModule(BalanceZoneTracker tracker, Action<string> log)
    {
        _tracker = tracker;
        _log = log;
    }
    
    public void OnBarUpdate(int bar, IndicatorCandle candle)
    {
        // Strategy logic: legge _tracker.CurrentZone, _tracker.POC
    }
}
```

**Step 2:** Registrare in `FabioOrderFlow.cs`

```csharp
if (bar == 0)
{
    _balanceTracker = new BalanceZoneTracker(...);
    
    if (EnablePostLondonImpulse)
    {
        _impulseModule = new PostLondonImpulseModule(_balanceTracker, Log);
    }
}

_balanceTracker.OnBarUpdate(bar, candle, CurrentBar);
_impulseModule?.OnBarUpdate(bar, candle);
```

**Step 3:** Esporre API necessarie in `BalanceZoneTracker` (se serve)

```csharp
public int? LondonCloseBar => _context.CurrentZone?.EndBar;
public decimal LondonRange => _context.CurrentZone?.High - _context.CurrentZone?.Low ?? 0;
```

### Principi di design

1. **Single Responsibility**: ogni modulo gestisce UNA strategia
2. **Dependency Injection**: moduli ricevono tracker via constructor
3. **Read-only access**: moduli leggono state del tracker, non lo modificano
4. **Event delegation**: tracker delega eventi ai moduli (es. `OnLiveCumulativeTrade`)
5. **Hot-pluggable**: moduli possono essere disabilitati via settings senza breaking changes

## Build & Deploy

```bash
cd src/
dotnet build -c Release
./deploy.bat  # Copia DLL in %APPDATA%\ATAS\Indicators\
```

**DLL size:** ~75KB (Release)

**Dependencies:**
- ATAS Platform SDK
- .NET 10.0-windows

## Testing Strategy

**Unit testing (future):**
- `BalanceZoneTracker`: test state machine transitions
- `SessionDetector`: test timezone conversions
- `LondonMeanReversionModule`: test trigger detection logic

**Integration testing:**
- Historical replay su CumulativeTrades chart
- Verifica log output (`[PROFILE_PREVIEW]`, `[MR_TRIGGER_M5]`)
- Visual inspection su ATAS chart

**Debugging:**
- `DetailedDebugLogs = true` in `BalanceZoneTracker.cs`
- Log parsing via `docs/atas/log-reading.md`

## Performance Considerations

**Profile calculation:**
- O(n) per bar: accumula volume in `Dictionary<decimal, decimal>`
- O(n log n) per preview: sort levels per value area calculation

**CumulativeTrades processing:**
- Historical: O(n) batch processing su tutti i trades
- Live: O(1) per singolo trade tick

**Optimizations:**
- Preview POC/VAH/VAL: calcolato solo a intervalli (ogni 4 bar o su eventi)
- State machine: early exit quando `State = NoZone`
- Visual rendering: update solo su `importantEvent = true`

## Known Limitations

1. **Single balance zone at a time**: tracker gestisce solo `CurrentZone`, vecchie zone vengono sovrascritte
2. **No multi-timeframe analysis**: tutto su timeframe del chart corrente
3. **Hard-coded session times**: London/NY orari fissi, no support per altri mercati
4. **Footprint-first experimental**: `EnableLiveFootprintFirst` non è production-ready

## Future Enhancements

- [ ] Multi-zone tracking: storico ultimi N balance zones
- [ ] Configurable session times via settings
- [ ] PostLondonImpulse module (Modello 1 trend-following)
- [ ] Refactor `LondonMeanReversionModule` → split rejection + aggression tracking
- [ ] Unit test suite
- [ ] Performance profiling su large historical datasets
