# Modello 2 — Mean Reversion (London Balance Zones)

Strategy mean reversion su balance zones di Londra: fakeout detection + ritorno verso POC.

## Concept

**Idea:** Quando prezzo fa sweep di high/low della balance zone di Londra ma viene rigettato (rejection candle), si genera trigger M5 per trade verso POC.

**Edge:** Fakeout su liquidity grab, mean reversion verso value area.

## Implementation

**Module:** `LondonMeanReversionModule`  
**Status:** ✅ Production (experimental footprint-first disabled)

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

### Trigger (M5)
```
Direction, Trigger Type, Bar, SweepBar
POC, VAH, VAL
Sweep High/Low, Rejection Close
Rejection Ticks, Delta
```

### Aggression (Live)
```
Price, Bid, Ask, Delta, Volume
Distance to POC, Distance to VAH/VAL
```

### Outcome
```
TriggerBar, CurrentBar, BarsElapsed
POC Reached (YES/NO)
EntryPrice, POCPrice, ProfitTicks
```

## Configuration

```csharp
EnableLondonMeanReversion = true      // Enable/disable module
EnableLiveFootprintFirst = false      // Experimental (disabled)
MinAggressionTradeVolume = 20         // Threshold contracts
```

## Log Examples

```
[MR_TRIGGER_M5] Direction=SHORT, Trigger=HIGH_REJECTION_CANDIDATE, 
  Bar=156, SweepBar=152, POC=16815.50, VAH=16820.25, VAL=16808.75, 
  High=16827.50, Close=16813.25, RejectionTicks=14.25, Delta=-245

[MR_LIVE_AGGRESSION_LONG] Bar=178, Price=16809.25, Bid=45, Ask=123, 
  Delta=78, BelowVAL, DistToPOC=-6.25

[MR_OUTCOME] TriggerBar=156, CurrentBar=189, Elapsed=33, POC_REACHED=YES, 
  ProfitTicks=2.25
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
