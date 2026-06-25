# BalanceZoneTracker

Core module per tracking balance zones e sessioni di trading.

## Responsabilità

- Rilevamento sessioni London (08:00-16:00 GMT) e New York (14:30-21:00 GMT)
- Costruzione volume profile (POC, VAH, VAL, 70% value area)
- State machine balance zones: Building → Ready → Broken
- Rendering visuale (rectangles + POC lines)
- Esposizione dati per moduli strategy

## Public API

```csharp
// State access
public BalanceZone? CurrentZone { get; }
public decimal? POC { get; }
public decimal LastPreviewPoc { get; }
public decimal LastPreviewVah { get; }
public decimal LastPreviewVal { get; }

// Module registration
public void SetMeanReversionModule(LondonMeanReversionModule module);

// Event delegation
public void OnBarUpdate(int bar, IndicatorCandle candle, int currentBar);
public void OnHistoricalCumulativeTrades(IEnumerable<CumulativeTrade> trades);
public void OnLiveCumulativeTrade(CumulativeTrade trade);
```

## State Machine

```
NoZone
  ↓ (London 08:00)
BuildingSessionProfile
  ↓ (accumula volume, calcola preview POC/VAH/VAL)
BalanceReady
  ↓ (London 16:00, finalize zone)
BalanceReady (waiting for breakout)
  ↓ (NY session, 3+ closes outside)
Broken (direction: Bullish/Bearish)
  ↓ (new session)
NoZone
```

## Data Structures

### BalanceZone
```csharp
public class BalanceZone
{
    public BalanceType Type { get; set; }  // LondonSession, NewYorkSession
    public int StartBar { get; set; }
    public int EndBar { get; set; }
    public bool IsReady { get; set; }
    
    // Price levels
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal POC { get; set; }
    public decimal VAH { get; set; }
    public decimal VAL { get; set; }
    
    // Volume profile
    public Dictionary<decimal, decimal> Profile { get; }
    public decimal TotalVolume { get; set; }
    
    // Breakout
    public BreakoutDirection? BreakoutDirection { get; set; }
    public int? BreakoutBar { get; set; }
}
```

### MarketContext
```csharp
public class MarketContext
{
    public MarketState State { get; set; }
    public BalanceZone? CurrentZone { get; set; }
    public BreakoutDirection? PendingDirection { get; set; }
    public int PendingBreakoutBar { get; set; }
    public int ConsecutiveOutsideCloses { get; set; }
}
```

## Core Methods

### Session Management
```csharp
private void StartLondonSession(int bar, IndicatorCandle candle)
// Inizia nuova sessione London, crea BalanceZone, reset state

private void UpdateLondonProfile(int bar, IndicatorCandle candle)
// Accumula volume per bar, aggiorna high/low, calcola preview POC/VAH/VAL

private void ProcessLondonClose(int bar)
// Finalizza profile, calcola POC/VAH/VAL definitivo, State = BalanceReady

private void StartNewYorkSession(int bar)
// Monitora breakout durante NY session
```

### Profile Calculation
```csharp
private bool TryCalculateProfilePreview(
    IReadOnlyDictionary<decimal, decimal> profile,
    decimal totalVolume,
    out decimal poc, out decimal vah, out decimal val,
    out decimal valueAreaVolume, out decimal maxVolume)
// Calcola POC (max volume level) e 70% value area (VAH/VAL)

private Dictionary<decimal, decimal> CalculateVolumeProfile(int startBar, int endBar)
// Aggrega volume per price level da startBar a endBar
```

### Breakout Detection
```csharp
private void CheckForPendingBreakout(int bar, IndicatorCandle candle)
// Se chiusura fuori balance zone: increment counter, set pending direction

private void ConfirmBreakout(int bar, BreakoutDirection direction)
// Se 3+ consecutive closes: conferma breakout, update zone colors
```

## Visual Rendering

```csharp
private void DrawBalanceZone()
// Rectangle: rosso (bearish) / blu (bullish), alpha 30

private void DrawPocLine()
// Horizontal line POC, colore match zone

private void UpdateBalanceZoneColors()
// Dynamic color update on breakout confirmation
```

## Usage (from Orchestrator)

```csharp
// Initialization (bar 0)
_balanceTracker = new BalanceZoneTracker(
    this, Log, Rectangles, HorizontalLinesTillTouch, GetCandle, enableFootprint);

// Per-bar update
_balanceTracker.OnBarUpdate(bar, candle, CurrentBar);

// CumulativeTrades (if chart type)
_balanceTracker.OnHistoricalCumulativeTrades(allTrades);
_balanceTracker.OnLiveCumulativeTrade(trade);

// Read state (from modules)
var zone = _balanceTracker.CurrentZone;
var poc = _balanceTracker.LastPreviewPoc;
```

## Configuration

Hard-coded constants:
```csharp
private const int LondonStartHour = 8;   // GMT
private const int LondonEndHour = 16;
private const int LondonPreviewStartHour = 14;  // Start preview at 14:00
private const int NYStartHour = 14;      // 14:30 GMT (NY open)
private const int NYEndHour = 21;        // GMT

private const int RequiredConsecutiveCloses = 3;  // For breakout
```

## Log Output

```
[SESSION_START] London session started at bar 45 (2024-01-15 08:00:00 UTC)
[PROFILE_PREVIEW] Bar=145, 2024-01-15 14:00:00 UTC, Reason=live, Bars=73, 
  High=16825.00, Low=16802.25, POC=16815.50, VAH=16820.25, VAL=16808.75
[SESSION_END] London session ended at bar 205 (2024-01-15 16:00:00 UTC)
[BREAKOUT_CONFIRMED] Bar=245, Direction=BULLISH, Zone=LONDON_SESSION
```

## Dependencies

- `SessionDetector`: session detection utility
- `IndicatorCandle`: ATAS candle data
- `CumulativeTrade`: ATAS trade data (optional, per aggression tracking)

## Known Limitations

- Single zone at time (CurrentZone overwrites previous)
- Hard-coded session times (no configurability)
- No multi-timeframe support
- Preview calculation every 4 bars or on important events (performance optimization)

## Source

`src/modules/shared/BalanceZoneTracker/BalanceZoneTracker.cs` (~1000 lines)
