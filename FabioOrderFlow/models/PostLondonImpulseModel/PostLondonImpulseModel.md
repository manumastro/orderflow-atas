# PostLondonImpulseModel (Modello 1)

Trend-following strategy su breakout della London balance zone durante NY session.

## Status

**SUPERSEDED LEGACY PLAN - NOT IMPLEMENTED**

Il piano `post-London + 3 close` non corrisponde fedelmente ai transcript: il modello continuation principale opera dall'apertura New York `09:30`, che si sovrappone a London, e richiede stato di imbalance, location LVN/pullback e aggressione con risultato. La discovery corrente e' nel ledger no-trade `../FabioAuctionStudyModel/`.

## Concept

**Idea:** Quando London balance zone viene rotta durante NY session, seguire l'impulso verso low volume nodes identificati nel profile.

**Edge:** Momentum continuation su breakout confermato con volume profile context.

## Strategy Flow (Planned)

```
1. London Balance Zone Ready
   ↓
2. NY Session imbalance/expansion (nessun requisito transcript di 3 close)
   ↓
3. Identify Low Volume Nodes in direction
   ↓
4. Wait for aggression confirmation
   ↓
5. Entry on pullback to breakout level
   ↓
6. Target: Next low volume node
```

## Modules (Planned)

### Core
- **BalanceZoneTracker** (shared) - Balance zone detection
- **ImpulseProfiler** - Impulse strength measurement
- **LowVolumeNodeDetector** - Target identification

### Entry/Exit
- **AggressionDetector** - Entry confirmation
- **TradeManager** - Position management
- **ConfirmationLayer** - Multi-timeframe validation

### Visual
- **VisualRenderer** - Chart annotations

## Documentation

Each module has its own README.md:
- `modules/PostLondonImpulse/README.md` - Module overview
- `modules/*/README.md` or `*.md` - Individual module docs
- `modules/shared/BalanceZoneTracker/` - Core tracker (shared)

## Implementation Status

- [x] Piano ritirato e sostituito dal dual-session auction-state ledger
- [ ] Module interfaces defined
- [ ] BalanceZoneTracker integrated (shared)
- [ ] ImpulseProfiler
- [ ] LowVolumeNodeDetector
- [ ] AggressionDetector
- [ ] TradeManager
- [ ] ConfirmationLayer
- [ ] VisualRenderer

## Related

- **Spec:** `../../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`
- **London MR Model:** `../LondonMeanReversionModel/`
