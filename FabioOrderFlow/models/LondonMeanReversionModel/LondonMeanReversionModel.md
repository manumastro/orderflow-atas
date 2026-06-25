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

```
[MR_AGGRESSION_CONFIRM] Direction, EntryModel=FootprintCumulativeTradeHistorical
Bar, CandidateBar, EntryPrice, Volume, TradeDirection
SweepTime, SecondsAfterSweep
StopReference, Target1POC, Target2, VAH, VAL
```

**Timestamp intrabar:**  
Gli entry timestamp sono sempre intrabar con precisione al millisecondo (es: `London=09:04:47.686`), non timestamp di chiusura bar. Questo permette di tracciare il timing preciso dell'aggression all'interno della barra.

**Live footprint detection:**  
Durante trading live (se `EnableLiveFootprintFirst=true`), vengono generati anche tag `[FOOTPRINT_*]` per sweep/rejection/entry real-time.

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
EnableLiveFootprintFirst = true       // Live footprint detection (default: true)
MinAggressionTradeVolume = 20         // Threshold contracts per entry
```

**EnableLiveFootprintFirst:**  
Controlla la detection live tick-by-tick durante trading attivo. Se `true`, genera tag `[FOOTPRINT_*]` real-time. Non influenza l'aggression confirmation su dati storici.

## Log Examples

### Trigger Detection
```
[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION, 
  BarMode=HISTORICAL_CLOSED, Bar=5206, CandidateBar=5203, 
  Italy=2026-06-25 10:15:00, London=2026-06-25 09:15:00, UTC=2026-06-25 08:15:00, 
  Close=30170.50, POC=30180.00, VAH=30198.75, VAL=30148.00, 
  DistToPOC=-9.50, StopReference=30219.00, Target1=30148.00
```

### Aggression Confirmation
```
[MR_AGGRESSION_CONFIRM] Direction=Short, EntryModel=FootprintCumulativeTradeHistorical, 
  Trigger=HIGH_REJECTION_FOLLOW_THROUGH, Bar=5205, CandidateBar=5203, 
  Italy=2026-06-25 10:04:47.686, London=2026-06-25 09:04:47.686, UTC=2026-06-25 08:04:47.686, 
  EntryPrice=30199.25, Volume=25, TradeDirection=Sell, 
  SweepTimeItaly=2026-06-25 10:03:02.713, SweepTimeLondon=2026-06-25 09:03:02.713, 
  SecondsAfterSweep=105.0, StopReference=30219.00, Target1POC=30180.00, Target2=30148.00
```

**Nota timestamp intrabar:**  
L'entry time `09:04:47.686` è dentro la barra (presumibilmente 09:00-09:05), con precisione al millisecondo. Il campo `SecondsAfterSweep=105.0` indica il ritardo preciso dal sweep.

### Outcome Tracking
```
[MR_TARGET_HIT] Direction=Short, Target=Target2, Bar=5212, 
  Italy=2026-06-25 10:45:00, EntryPrice=30199.25, TargetPrice=30148.00, 
  RewardPoints=51.25, MFE=60.00, MAE=18.50

[MR_POSITION_CLOSED] Direction=Short, Bar=5212, 
  Italy=2026-06-25 10:45:00, EntryPrice=30199.25, ExitPrice=30148.00, 
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
