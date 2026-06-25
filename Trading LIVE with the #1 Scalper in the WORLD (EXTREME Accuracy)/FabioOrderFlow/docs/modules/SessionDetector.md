# SessionDetector

Utility module per rilevamento sessioni trading e conversione timezone.

## Responsabilità

- Rilevamento sessione London (08:00-16:00 GMT)
- Rilevamento sessione New York (14:30-21:00 GMT)
- Conversione timezone (UTC ↔ London ↔ New York)

## Design

**Stateless utility:** no instance state, pure functions.

## Public API

```csharp
public static class SessionDetector
{
    public static TradingSession? GetCurrentSession(DateTime utc);
    public static bool IsLondonSession(DateTime utc);
    public static bool IsNewYorkSession(DateTime utc);
}

public static class MarketTimeZones
{
    public static DateTime ToLondon(DateTime utc);
    public static DateTime ToNewYork(DateTime utc);
}
```

## Session Definitions

### London Session
- **Start:** 08:00 GMT
- **End:** 16:00 GMT
- **Duration:** 8 hours
- **Purpose:** Balance zone construction

### New York Session
- **Start:** 14:30 GMT (09:30 EST)
- **End:** 21:00 GMT (16:00 EST)
- **Duration:** 6.5 hours
- **Purpose:** Breakout monitoring

### Overlap
- **London/NY overlap:** 14:30-16:00 GMT (1.5 hours)

## TradingSession Enum

```csharp
public enum TradingSession
{
    London,
    NewYork,
    PreMarket,   // Before London
    PostMarket   // After NY
}
```

## Usage

```csharp
// Check current session
var session = SessionDetector.GetCurrentSession(candle.Time);
if (session == TradingSession.London)
{
    // Build profile
}

// Convert timezone
var londonTime = MarketTimeZones.ToLondon(candle.Time);
if (londonTime.Hour == 8 && londonTime.Minute == 0)
{
    StartLondonSession();
}

// Boolean checks
if (SessionDetector.IsLondonSession(barTime))
{
    UpdateLondonProfile(bar, candle);
}
```

## Timezone Handling

**Assumption:** ATAS provides candle.Time in UTC.

**Conversions:**
- London: UTC + 0 (GMT, no DST handling)
- New York: UTC - 5 (EST, no DST handling)

**Note:** DST not implemented. Session times are fixed GMT.

## Known Limitations

- No DST support (Daylight Saving Time)
- Hard-coded session times (not configurable)
- No support for other markets (Asia, Frankfurt)
- Assumes UTC input (no validation)

## Future Enhancements

- [ ] DST support (BST, EDT)
- [ ] Configurable session times
- [ ] Additional markets (Asia, Frankfurt)
- [ ] Session overlap detection helper

## Source

`src/modules/shared/SessionDetector/SessionDetector.cs` (~100 lines)
