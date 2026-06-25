# Modello 2 — Mean Reversion (London Balance Zones)

Strategy mean reversion su balance zones di Londra: fakeout detection + ritorno verso POC.

## Concept

**Idea:** Quando prezzo fa sweep di high/low della balance zone di Londra ma viene rigettato (rejection candle), si genera trigger M5 per trade verso POC.

**Edge:** Fakeout su liquidity grab, mean reversion verso value area.

## Implementation

**Module:** `LondonMeanReversionModule`  
**Status:** ✅ Production-ready  
**Historical Detection:** ✅ Fully operational (intrabar precision)  
**Live Detection:** ✅ Implemented (requires live trading to test)

## Strategy Flow

```
1. London Session (08:00-16:00 GMT)
   ↓
2. Build Volume Profile → POC, VAH, VAL
   ↓
3. Detect Sweep (new high/low)
   ↓
4. Check Rejection Candle (close 10+ ticks inside)
   ↓
5. Trigger M5 (direction verso POC)
   ↓
6. Track Aggression (CumulativeTrades)
   ↓
7. Monitor Outcome (POC reached?)
```

## Entry Conditions

### Long (dopo low sweep)
- ✅ New session low
- ✅ Rejection candle: close >= low + 10 ticks
- ✅ Delta positivo (ask > bid)
- ✅ Entry aggression: ask > bid sotto VAL

### Short (dopo high sweep)
- ✅ New session high
- ✅ Rejection candle: close <= high - 10 ticks
- ✅ Delta negativo (bid > ask)
- ✅ Entry aggression: bid > ask sopra VAH

## Trigger Types

| Type | Condition |
|------|-----------|
| `HIGH_REJECTION_CANDIDATE` | Sweep high + rejection → M5 short |
| `LOW_REJECTION_CANDIDATE` | Sweep low + rejection → M5 long |
| `POC_LOST_EARLY` | POC perso prima di M5 → early short |
| `POC_RECLAIMED_EARLY` | POC reclaimed prima di M5 → early long |

## Target & Exit

**Target:** POC della balance zone  
**Risk:** 10-15 ticks (distance rejection to extreme)  
**R:R:** Variabile (dipende da distanza POC-extreme)

## Data Logged

### Trigger Detection
```
[MR_TRIGGER] Direction, Trigger Type, Bar, CandidateBar
POC, VAH, VAL, Close, DistToPOC
StopReference, Target1, Bid, Ask, Delta
```

### Aggression Confirmation

**Historical Mode** (sempre attivo, per backtest/replay):
```
[MR_AGGRESSION_CONFIRM] Direction, EntryModel=FootprintCumulativeTradeHistorical
Bar, CandidateBar, EntryPrice, EntryAreaLow, EntryAreaHigh
Volume, TradeDirection, SweepTime (intrabar timestamp con millisecondi)
SecondsAfterSweep, StopReference, Target1POC, Target2
VAH, VAL, MinVolume
```

**Live Mode** (solo durante trading live, se `EnableLiveFootprintFirst=true`):
```
[FOOTPRINT_HIGH_SWEEP] Bar, Price, Volume, VAH
[FOOTPRINT_LOW_SWEEP] Bar, Price, Volume, VAL
[FOOTPRINT_REJECTION] Direction, Bar, RejectionPrice, Volume, SecondsAfterSweep
[FOOTPRINT_ENTRY] Direction, Trigger=FOOTPRINT_FIRST, EntryPrice, Volume
```

**Key Differences:**
- **Historical:** Batch processing dopo chiusura bar trigger, individua entry con timestamp intrabar precisi (es: 09:04:47.686)
- **Live:** Real-time tick-by-tick detection prima della chiusura bar, per alert anticipati
- **Entrambi:** Stessi criteri (volume >= minVolume, direction corretta, inside value area)
- **Precisione:** Entrambi individuano entry intrabar con millisecondi (non solo chiusura bar)

### Outcome Tracking
```
[MR_TARGET_HIT] Direction, Target (POC/Target2), Bar, EntryPrice, TargetPrice
RewardPoints, MFE, MAE

[MR_POSITION_CLOSED] Direction, Bar, EntryPrice, ExitPrice, ExitReason (STOP_HIT/TARGET_HIT)
PnL, MFE, MAE

[MR_MFE_UPDATE] Direction, Bar, MFE, MAE (tracking intrabar durante posizione attiva)
```

## Configuration

```csharp
EnableLondonMeanReversion = true      // Enable/disable module
EnableLiveFootprintFirst = true       // Live tick detection (default: true)
                                       // false = solo historical post-processing
                                       // true = real-time footprint detection
MinAggressionTradeVolume = 20         // Threshold contracts per entry
```

**EnableLiveFootprintFirst:**
- Controlla SOLO la detection live tick-by-tick
- NON influenza aggression confirmation storica (sempre attiva)
- `true`: Alert e detection anticipati durante trading live
- `false`: Aspetta chiusura bar per conferma (più conservativo)
- Su backtest/replay: Irrilevante (usa sempre historical mode)

## Log Examples

### Trigger Detection
```
[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION, 
  BarMode=HISTORICAL_CLOSED, Bar=5206, CurrentBar=5359, 
  Italy=2026-06-25 10:15:00, London=2026-06-25 09:15:00, UTC=2026-06-25 08:15:00, 
  CandidateBar=5203, Close=30170.50, POC=30180.00, VAH=30198.75, VAL=30148.00, 
  DistToPOC=-9.50, StopReference=30219.00, Target1=30148.00, Bid=296, Ask=296, Delta=0
```

### Aggression Confirmation (Historical)
```
[MR_AGGRESSION_CONFIRM] Direction=Short, EntryModel=FootprintCumulativeTradeHistorical, 
  Trigger=HIGH_REJECTION_FOLLOW_THROUGH, Bar=5205, CandidateBar=5203, 
  Italy=2026-06-25 10:04:47.686, London=2026-06-25 09:04:47.686, UTC=2026-06-25 08:04:47.686, 
  EntryPrice=30199.25, EntryAreaLow=30199.25, EntryAreaHigh=30202.50, 
  FirstPrice=30202.50, LastPrice=30199.25, Volume=25, TradeDirection=Sell, 
  SweepTimeItaly=2026-06-25 10:03:02.713, SweepTimeLondon=2026-06-25 09:03:02.713, 
  SweepTimeUtc=2026-06-25 08:03:02.713, SecondsAfterSweep=105.0, 
  StopReference=30219.00, RiskPoints=19.75, Target1POC=30180.00, Target2=30148.00, 
  RewardToPOC=19.25, RewardToTarget2=51.25, VAH=30200.00, VAL=30148.00, 
  MinVolume=20, VolumeRule=Hardcoded20
```

**Note:** Entry timestamp è INTRABAR (09:04:47.686) con millisecondi precisi, 105 secondi dopo sweep.

### Aggression Confirmation (Live - durante trading attivo)
```
[FOOTPRINT_HIGH_SWEEP] Bar=152, Italy=2026-06-25 14:30:11.888, 
  London=2026-06-25 13:30:11.888, UTC=2026-06-25 12:30:11.888, 
  Price=30202.50, Volume=45, VAH=30200.00

[FOOTPRINT_REJECTION] Direction=Short, Bar=152, 
  Italy=2026-06-25 14:30:35.123, London=2026-06-25 13:30:35.123, UTC=2026-06-25 12:30:35.123, 
  RejectionPrice=30195.00, Volume=32, SweepPrice=30202.50, SecondsAfterSweep=23.2

[FOOTPRINT_ENTRY] Direction=Short, Trigger=FOOTPRINT_FIRST, Bar=153, 
  SweepBar=152, Italy=2026-06-25 14:32:18.456, London=2026-06-25 13:32:18.456, 
  UTC=2026-06-25 12:32:18.456, EntryPrice=30190.00, Volume=28, 
  StopReference=30210.00, Target1POC=30180.00, Target2=30160.00, 
  SweepToEntrySeconds=127.3, RejectionToEntrySeconds=103.3
```

**Note:** Live tags appaiono solo durante trading attivo (non su dati storici).

### Outcome Tracking
```
[MR_TARGET_HIT] Direction=Short, EntryModel=FootprintCumulativeTradeHistorical, 
  Target=Target2, Bar=5212, CandidateBar=5203, Italy=2026-06-25 10:45:00, 
  London=2026-06-25 09:45:00, UTC=2026-06-25 08:45:00, 
  EntryPrice=30199.25, TargetPrice=30148.00, RewardPoints=51.25, MFE=60.00, MAE=18.50

[MR_POSITION_CLOSED] Direction=Short, EntryModel=FootprintCumulativeTradeHistorical, 
  Bar=5212, CandidateBar=5203, Italy=2026-06-25 10:45:00, London=2026-06-25 09:45:00, 
  UTC=2026-06-25 08:45:00, EntryPrice=30199.25, ExitPrice=30148.00, 
  ExitReason=TARGET_HIT, PnL=51.25, MFE=60.00, MAE=18.50
```

## Backtesting

**Log parsing:** `../../docs/atas/log-reading.md`

**Metrics to track:**
- Win rate (POC reached)
- Average bars to target
- Average R:R
- False breakout frequency

## Known Limitations

- Single zone tracking (no history)
- Historical aggression check: solo su sweep bars
- Footprint-first experimental (può generare falsi positivi)
- No dynamic target (sempre POC)

## Future Enhancements

- [ ] Dynamic target (partial profit at VAH/VAL)
- [ ] Multi-zone context (previous sessions)
- [ ] Volume profile quality filter
- [ ] Time-based filter (early vs late session)

## Related Docs

- Core tracker: [BalanceZoneTracker.md](BalanceZoneTracker.md)
- Session detection: [SessionDetector.md](SessionDetector.md)
- Implementation: `src/modules/LondonMeanReversion/`
