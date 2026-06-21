# BalanceZoneTracker — Design Document

## 1. Obiettivi

**Scopo primario:** Fornire il contesto "balance vs out-of-balance" per il Modello 1 Trend Following.

**Non è:** Un indicatore standalone per tracciare balance zones perfette (quello era Modello 2).

**È:** Un modulo di supporto che fornisce:
1. Riferimento balance zone corrente (VAH/VAL/POC)
2. Stato del mercato: `BALANCE` vs `OUT_OF_BALANCE`
3. Target level (POC) per trade management
4. Visualizzazione background (rettangoli grigi/blu)

**Requisito chiave:** "Good enough" detection — non serve perfezione, ma robustezza e semplicità.

---

## 2. Approccio Scelto: Session-Based con Override

### 2.1 Logica Core

**Default (sempre attivo):**
- Balance zone = **sessione precedente completa** (London o NY)
- Calcola POC/VAH/VAL su tutta la sessione chiusa
- Pulito, deterministico, sempre disponibile

**Override (opzionale, se rilevato):**
- Se durante la sessione corrente si forma un **consolidation evidente** (range stretto per 15+ barre M5):
  - Aggiorna balance zone al nuovo range
  - Ricalcola POC/VAH/VAL sul consolidation
  - Usa quello come riferimento fino al breakout

**Transizione OUT_OF_BALANCE:**
- Breakout = 2 chiusure consecutive oltre VAH (bullish) o VAL (bearish)
- Conferma = terza barra mantiene direzione (no immediate reversal)

### 2.2 Perché Questo Approccio?

✅ **Sempre hai un riferimento** (sessione precedente) anche se non perfetto  
✅ **Robusto** — session boundaries sono chiare (orario fisso)  
✅ **Cattura consolidation evidenti** (intra-session balance) senza complessità  
✅ **Deterministico** — stesso input = stesso output (importante per backtest)  
✅ **Non richiede validazione complessa** come Modello 2 (POC stability, acceptance rate, ecc.)

---

## 3. Strutture Dati

### 3.1 Balance Zone State

```csharp
public class BalanceZone
{
    // Identity
    public int StartBar { get; set; }        // Prima barra della zone
    public int EndBar { get; set; }          // Ultima barra (CurrentBar se attiva)
    public BalanceType Type { get; set; }    // SESSION o CONSOLIDATION
    
    // Volume Profile Data
    public decimal POC { get; set; }         // Point of Control (max volume)
    public decimal VAH { get; set; }         // Value Area High (top 70% volume)
    public decimal VAL { get; set; }         // Value Area Low (bottom 70% volume)
    
    // Range
    public decimal High { get; set; }        // Session/consolidation high
    public decimal Low { get; set; }         // Session/consolidation low
    
    // Volume metrics
    public decimal TotalVolume { get; set; }
    public Dictionary<decimal, decimal> PriceVolumeProfile { get; set; }
    
    // State
    public bool IsActive { get; set; }       // true se è la balance zone corrente
    public bool IsBroken { get; set; }       // true se out of balance confermato
    public BreakoutDirection? Breakout { get; set; }  // BULLISH, BEARISH, null
    
    // Visual
    public DrawingRectangle Rectangle { get; set; }  // VAH/VAL box
    public TrendLine PocLine { get; set; }           // POC horizontal line
}

public enum BalanceType
{
    SESSION,        // Balance = intera sessione precedente
    CONSOLIDATION   // Balance = consolidation intra-session
}

public enum BreakoutDirection
{
    BULLISH,   // Breakout sopra VAH
    BEARISH    // Breakout sotto VAL
}
```

### 3.2 Market State

```csharp
public enum MarketState
{
    BALANCE,            // Prezzo dentro VAH/VAL
    OUT_OF_BALANCE,     // Breakout confermato
    BREAKOUT_PENDING    // 1 chiusura oltre VAH/VAL, attende conferma
}

public class MarketContext
{
    public MarketState State { get; set; }
    public BalanceZone CurrentBalanceZone { get; set; }
    public int BreakoutBar { get; set; }  // Bar del breakout (se OUT_OF_BALANCE)
    public BreakoutDirection? Direction { get; set; }
}
```

### 3.3 Tracker State

```csharp
private MarketContext _context;
private List<BalanceZone> _historicalZones;  // Tutte le zone tracciate (per visual + analisi)
private SessionInfo _currentSession;          // London o NY
private ConsolidationDetector _consolidationDetector;
```

---

## 4. Algoritmo — Step by Step

### 4.1 OnCalculate(int bar, int period)

```
1. Rileva sessione corrente (London/NY)
   ├─ Se nuova sessione AND sessione precedente completa:
   │  └─ CreateBalanceZoneFromSession(previous_session)
   │
2. Se non esiste balance zone corrente:
   └─ Usa sessione precedente come fallback
   
3. Aggiorna balance zone corrente:
   ├─ Se TYPE == SESSION:
   │  └─ EndBar = bar corrente (estendi fino ad ora)
   │
   ├─ Se TYPE == CONSOLIDATION:
   │  └─ EndBar = bar corrente (estendi fino ad ora)
   │
4. Rileva consolidation intra-session:
   ├─ Se ConsolidationDetector.IsConsolidating(last 15-20 bars):
   │  └─ CreateBalanceZoneFromConsolidation()
   │     └─ Override la balance zone corrente
   │
5. Verifica stato breakout:
   ├─ Se BALANCE:
   │  ├─ Close > VAH per 2 barre consecutive?
   │  │  └─ State = BREAKOUT_PENDING (bullish)
   │  ├─ Close < VAL per 2 barre consecutive?
   │  │  └─ State = BREAKOUT_PENDING (bearish)
   │
   ├─ Se BREAKOUT_PENDING:
   │  ├─ Terza barra conferma direzione?
   │  │  └─ State = OUT_OF_BALANCE
   │  │     └─ IsBroken = true
   │  ├─ Terza barra rientra in VAH/VAL?
   │  │  └─ State = BALANCE (false breakout)
   │
   ├─ Se OUT_OF_BALANCE:
   │  └─ Rimani OUT_OF_BALANCE fino a nuova balance zone
   │
6. Aggiorna visual:
   └─ UpdateDrawings(balance_zone)
```

### 4.2 CreateBalanceZoneFromSession

```
INPUT: SessionInfo (start_bar, end_bar, tipo_sessione)

1. Calcola profilo volumetrico su [start_bar, end_bar]:
   └─ PriceVolumeProfile = AggregateVolume(start_bar, end_bar)
   
2. Calcola POC:
   └─ POC = prezzo con volume massimo in PriceVolumeProfile
   
3. Calcola VAH/VAL:
   ├─ Sort PriceVolumeProfile per volume (desc)
   ├─ Accumula volume fino a 70% del totale
   └─ VAH = max(prices in 70% area), VAL = min(prices in 70% area)
   
4. Crea BalanceZone:
   └─ Type = SESSION
   └─ StartBar = start_bar
   └─ EndBar = end_bar
   └─ IsActive = true
   └─ IsBroken = false
   
5. Salva in _context.CurrentBalanceZone e _historicalZones

OUTPUT: BalanceZone
```

### 4.3 ConsolidationDetector.IsConsolidating

```
INPUT: Ultime N barre (es. 15 barre su M5 = 75 minuti)

1. Calcola range:
   └─ range = max(high) - min(low) nelle ultime N barre
   
2. Calcola ATR medio (Average True Range):
   └─ ATR_avg = media degli ATR delle ultime N barre
   
3. Check consolidation criteria:
   ├─ Range < 2.0 * ATR_avg (range stretto)
   ├─ StdDev(close) < threshold (prezzo poco volatile)
   ├─ N >= 15 barre (consolidation deve durare almeno 75 min)
   
4. Se tutti i criteri soddisfatti:
   └─ RETURN true
   ELSE:
   └─ RETURN false

OUTPUT: bool
```

### 4.4 CreateBalanceZoneFromConsolidation

```
INPUT: Ultime N barre consolidate

1. Trova start_bar della consolidation:
   └─ start_bar = bar - N
   
2. Calcola profilo volumetrico su [start_bar, current_bar]:
   └─ PriceVolumeProfile = AggregateVolume(start_bar, current_bar)
   
3. Calcola POC/VAH/VAL (stesso algoritmo di session)

4. Chiudi balance zone precedente:
   └─ previous_zone.IsActive = false
   └─ previous_zone.EndBar = start_bar - 1
   
5. Crea nuova BalanceZone:
   └─ Type = CONSOLIDATION
   └─ StartBar = start_bar
   └─ EndBar = current_bar
   └─ IsActive = true
   └─ IsBroken = false
   
6. Aggiorna _context.CurrentBalanceZone

OUTPUT: BalanceZone
```

---

## 5. Session Detection

### 5.1 Sessioni da Tracciare

**London:** 08:00 - 16:00 GMT (BST-aware via TimeZoneInfo)  
**New York:** 14:30 - 21:00 UTC (9:30 AM - 4:00 PM ET)

**Overlap:** 14:30-16:00 UTC (London + NY contemporanee)

### 5.2 Quale Sessione Usare Come Balance Reference?

**Opzione 1 (consigliata):** London come balance, NY come trading session

```
- London (8:00-16:00 GMT): calcola balance zone
- NY (14:30-21:00 UTC): cerca out-of-balance e trade
- Logica: London spesso consolida, NY spesso trend
```

**Opzione 2:** Sessione precedente generica

```
- Se ora è NY → balance = London appena chiusa
- Se ora è Asia/pre-London → balance = NY precedente
```

**Implementazione suggerita:** Opzione 1 (più semplice, allineato con transcript Fabio)

### 5.3 Codice Session Detection

```csharp
private SessionInfo DetectCurrentSession(DateTime utcTime)
{
    var gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
    var gmtTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, gmtZone);
    var timeOfDay = gmtTime.TimeOfDay;
    
    // London: 8:00 - 16:00 GMT
    if (timeOfDay >= new TimeSpan(8, 0, 0) && timeOfDay < new TimeSpan(16, 0, 0))
        return new SessionInfo { Type = SessionType.London, StartTime = ..., EndTime = ... };
    
    // NY: 14:30 - 21:00 UTC (già UTC, no conversione)
    var utcTimeOfDay = utcTime.TimeOfDay;
    if (utcTimeOfDay >= new TimeSpan(14, 30, 0) && utcTimeOfDay < new TimeSpan(21, 0, 0))
        return new SessionInfo { Type = SessionType.NewYork, StartTime = ..., EndTime = ... };
    
    return new SessionInfo { Type = SessionType.Other };
}
```

---

## 6. Calcolo Volume Profile — Dettagli

### 6.1 Aggregazione Volume per Livello di Prezzo

```csharp
private Dictionary<decimal, decimal> AggregateVolume(int startBar, int endBar)
{
    var profile = new Dictionary<decimal, decimal>();
    
    for (int i = startBar; i <= endBar; i++)
    {
        var candle = GetCandle(i);
        var levels = candle.GetAllPriceLevels();
        
        if (levels == null) continue;
        
        foreach (var level in levels)
        {
            var price = level.Price;
            var volume = level.Volume;
            
            if (!profile.ContainsKey(price))
                profile[price] = 0;
            
            profile[price] += volume;
        }
    }
    
    return profile;
}
```

### 6.2 Calcolo POC (Point of Control)

```csharp
private decimal CalculatePOC(Dictionary<decimal, decimal> profile)
{
    if (profile.Count == 0) return 0;
    
    var maxVolume = profile.Values.Max();
    var pocPrice = profile.First(kvp => kvp.Value == maxVolume).Key;
    
    return pocPrice;
}
```

### 6.3 Calcolo VAH/VAL (Value Area)

```csharp
private (decimal VAH, decimal VAL) CalculateValueArea(
    Dictionary<decimal, decimal> profile, 
    decimal poc, 
    decimal valueAreaPercent = 0.70m)
{
    if (profile.Count == 0) return (0, 0);
    
    var totalVolume = profile.Values.Sum();
    var targetVolume = totalVolume * valueAreaPercent;
    
    // Start dal POC e espandi su/giù
    var sortedByPrice = profile.OrderBy(kvp => kvp.Key).ToList();
    var pocIndex = sortedByPrice.FindIndex(kvp => kvp.Key == poc);
    
    var accumulatedVolume = profile[poc];
    var upperIndex = pocIndex;
    var lowerIndex = pocIndex;
    
    while (accumulatedVolume < targetVolume)
    {
        // Guarda sopra e sotto, prendi il lato con più volume
        var upperVolume = (upperIndex + 1 < sortedByPrice.Count) 
            ? sortedByPrice[upperIndex + 1].Value : 0;
        var lowerVolume = (lowerIndex - 1 >= 0) 
            ? sortedByPrice[lowerIndex - 1].Value : 0;
        
        if (upperVolume >= lowerVolume && upperIndex + 1 < sortedByPrice.Count)
        {
            upperIndex++;
            accumulatedVolume += sortedByPrice[upperIndex].Value;
        }
        else if (lowerIndex - 1 >= 0)
        {
            lowerIndex--;
            accumulatedVolume += sortedByProfile[lowerIndex].Value;
        }
        else
        {
            break;  // Raggiunti i limiti
        }
    }
    
    var vah = sortedByPrice[upperIndex].Key;
    var val = sortedByPrice[lowerIndex].Key;
    
    return (vah, val);
}
```

---

## 7. Rilevamento Breakout

### 7.1 Logica Breakout con Conferma

```csharp
private void CheckBreakout(int bar, BalanceZone zone)
{
    var candle = GetCandle(bar);
    var close = candle.Close;
    
    // State: BALANCE
    if (_context.State == MarketState.BALANCE)
    {
        // Bullish breakout?
        if (close > zone.VAH)
        {
            _context.State = MarketState.BREAKOUT_PENDING;
            _context.Direction = BreakoutDirection.BULLISH;
            _context.BreakoutBar = bar;
            return;
        }
        
        // Bearish breakout?
        if (close < zone.VAL)
        {
            _context.State = MarketState.BREAKOUT_PENDING;
            _context.Direction = BreakoutDirection.BEARISH;
            _context.BreakoutBar = bar;
            return;
        }
    }
    
    // State: BREAKOUT_PENDING
    else if (_context.State == MarketState.BREAKOUT_PENDING)
    {
        var barsFromBreakout = bar - _context.BreakoutBar;
        
        // Conferma dopo 2 barre (total 3 chiusure oltre VAH/VAL)
        if (barsFromBreakout >= 2)
        {
            var isStillOut = (_context.Direction == BreakoutDirection.BULLISH && close > zone.VAH)
                          || (_context.Direction == BreakoutDirection.BEARISH && close < zone.VAL);
            
            if (isStillOut)
            {
                // Conferma OUT_OF_BALANCE
                _context.State = MarketState.OUT_OF_BALANCE;
                zone.IsBroken = true;
                zone.Breakout = _context.Direction;
                
                if (EnableLogging)
                    LogBreakout(bar, zone);
            }
            else
            {
                // False breakout — torna in BALANCE
                _context.State = MarketState.BALANCE;
                _context.Direction = null;
                _context.BreakoutBar = 0;
            }
        }
    }
}
```

---

## 8. Visualizzazione

### 8.1 Drawing Balance Zone

```csharp
private void UpdateDrawings(BalanceZone zone)
{
    if (zone.Rectangle == null)
    {
        // Crea nuovo rettangolo VAH/VAL
        var pen = new Pen(Brushes.Gray, 1) { DashStyle = DashStyles.Dash };
        var brush = new SolidBrush(Color.FromArgb(30, 128, 128, 128));  // Grigio trasparente
        
        zone.Rectangle = new DrawingRectangle(
            zone.StartBar, zone.VAH,
            zone.EndBar, zone.VAL,
            pen, brush
        );
        
        Rectangles.Add(zone.Rectangle);
    }
    else
    {
        // Aggiorna rettangolo esistente
        zone.Rectangle.SecondBar = zone.EndBar;  // Estendi a destra
        
        // Cambia colore se broken
        if (zone.IsBroken)
        {
            zone.Rectangle.Brush = new SolidBrush(Color.FromArgb(30, 0, 100, 255));  // Blu
        }
    }
    
    // POC line
    if (zone.PocLine == null)
    {
        var pocPen = new Pen(Brushes.Orange, 2);
        zone.PocLine = new TrendLine(
            zone.StartBar, zone.POC,
            zone.EndBar, zone.POC,
            pocPen
        ) { IsRay = false };
        
        TrendLines.Add(zone.PocLine);
    }
    else
    {
        zone.PocLine.SecondBar = zone.EndBar;  // Estendi a destra
    }
}
```

---

## 9. Edge Cases & Robustness

### 9.1 Sessione Senza Dati Sufficienti

**Problema:** Se la sessione precedente ha < 10 barre (broker issues, weekend, ecc.)

**Soluzione:**
```csharp
if (endBar - startBar < 10)
{
    // Usa sessione ancora precedente come fallback
    // O skip balance zone creation per questa iterazione
    return null;
}
```

### 9.2 Profilo Volumetrico Piatto

**Problema:** Tutti i livelli hanno volume simile (no POC chiaro)

**Soluzione:**
```csharp
var maxVolume = profile.Values.Max();
var avgVolume = profile.Values.Average();

if (maxVolume < avgVolume * 1.2m)  // POC non chiaramente dominante
{
    // Usa semplicemente il mid del range come POC
    POC = (High + Low) / 2;
}
```

### 9.3 Multiple Breakout nella Stessa Sessione

**Problema:** Out of balance → ri-balance → nuovo out of balance

**Soluzione:**
- Mantieni la balance zone originale (sessione precedente) sempre visibile
- Crea nuova balance zone dal consolidation
- Non chiudere mai la zone di sessione, solo override temporaneo

### 9.4 Gap Opening

**Problema:** NY apre con gap oltre VAH/VAL della London session

**Soluzione:**
```csharp
// Prima barra della sessione ignora breakout check
if (IsFirstBarOfSession(bar))
{
    _context.State = MarketState.BALANCE;  // Reset state
    return;
}
```

---

## 10. Parametri Configurabili

```csharp
[Parameter]
[Display(Name = "Value Area %", GroupName = "Balance Detection", Order = 10)]
public decimal ValueAreaPercent { get; set; } = 0.70m;  // 70%

[Parameter]
[Display(Name = "Breakout Confirm Bars", GroupName = "Balance Detection", Order = 20)]
public int BreakoutConfirmBars { get; set; } = 2;

[Parameter]
[Display(Name = "Consolidation Lookback", GroupName = "Balance Detection", Order = 30)]
public int ConsolidationLookback { get; set; } = 15;  // barre

[Parameter]
[Display(Name = "Consolidation Range Factor", GroupName = "Balance Detection", Order = 40)]
public decimal ConsolidationRangeFactor { get; set; } = 2.0m;  // * ATR

[Parameter]
[Display(Name = "Draw Historical Zones", GroupName = "Visual", Order = 50)]
public bool DrawHistoricalZones { get; set; } = true;
```

---

## 11. Testing Strategy

### 11.1 Unit Tests (Pseudo-Code)

```csharp
[Test]
public void Test_CalculatePOC_ReturnsMaxVolumePrice()
{
    var profile = new Dictionary<decimal, decimal> {
        { 100m, 500 },
        { 101m, 1200 },  // <- POC
        { 102m, 800 }
    };
    
    var poc = CalculatePOC(profile);
    Assert.AreEqual(101m, poc);
}

[Test]
public void Test_BreakoutConfirmation_Requires2Bars()
{
    // Setup balance zone VAH=105, VAL=95
    // Bar 100: close = 106 (above VAH) → BREAKOUT_PENDING
    // Bar 101: close = 107 (still above) → BREAKOUT_PENDING
    // Bar 102: close = 108 (still above) → OUT_OF_BALANCE
    
    Assert.AreEqual(MarketState.OUT_OF_BALANCE, _context.State);
}

[Test]
public void Test_FalseBreakout_ReturnsToBalance()
{
    // Bar 100: close = 106 (above VAH) → BREAKOUT_PENDING
    // Bar 101: close = 104 (back inside) → BALANCE
    
    Assert.AreEqual(MarketState.BALANCE, _context.State);
}
```

### 11.2 Visual Validation

1. Caricare 5 giorni di dati NQ M5
2. Verificare che le balance zones appaiano su London sessions
3. Verificare breakout su NY sessions
4. Controllare che POC sia visibile (linea arancione)
5. Controllare che i colori cambino (grigio → blu su breakout)

### 11.3 Log Validation

```
Expected log output:
[BALANCE] Session=London | Start=08:00 | POC=15420.50 | VAH=15435.25 | VAL=15405.75
[BREAKOUT_PENDING] Bar=145 | Direction=BULLISH | Close=15437.00 > VAH=15435.25
[OUT_OF_BALANCE] Bar=147 | Confirmed BULLISH | Bars from breakout=2
```

---

## 12. Performance Considerations

### 12.1 Profilo Volumetrico

**Problema:** Ricalcolare il profilo ogni barra è costoso.

**Soluzione:**
- Calcola profilo solo a fine sessione (sessione chiusa)
- Per sessione corrente: calcola una volta, poi aggiorna incrementalmente
```csharp
// Invece di ricalcolare tutto:
for (int i = startBar; i <= endBar; i++) { ... }

// Aggiorna solo l'ultima barra:
if (bar == endBar) {
    UpdateProfile(bar);
}
```

### 12.2 Limiti Historical Zones

**Problema:** Tenere tutte le zone storiche in memoria può essere pesante.

**Soluzione:**
```csharp
private const int MAX_HISTORICAL_ZONES = 20;  // Ultimi 20 zone (circa 10 giorni)

if (_historicalZones.Count > MAX_HISTORICAL_ZONES)
{
    var oldest = _historicalZones.First();
    _historicalZones.Remove(oldest);
    
    // Rimuovi drawings
    Rectangles.Remove(oldest.Rectangle);
    TrendLines.Remove(oldest.PocLine);
}
```

---

## 13. Prossimi Step

### Phase 1: Implementazione Base (2 giorni)

- [ ] Strutture dati (BalanceZone, MarketContext, SessionInfo)
- [ ] Session detection (London/NY)
- [ ] Volume profile calculation (POC/VAH/VAL)
- [ ] Balance zone creation da sessione precedente

### Phase 2: Breakout Detection (1 giorno)

- [ ] State machine (BALANCE → BREAKOUT_PENDING → OUT_OF_BALANCE)
- [ ] Logica conferma breakout (2+ barre)
- [ ] Gestione false breakout

### Phase 3: Consolidation Override (1 giorno)

- [ ] ConsolidationDetector (range stretto + ATR check)
- [ ] CreateBalanceZoneFromConsolidation
- [ ] Override logic

### Phase 4: Visual + Testing (1 giorno)

- [ ] DrawingRectangle per VAH/VAL
- [ ] TrendLine per POC
- [ ] Logging dettagliato
- [ ] Visual validation su replay

---

## 14. Domande da Risolvere

1. **Timeframe:** M5 per balance detection è OK? O preferisci M15?
   - Pro M5: più dati, profilo più granulare
   - Pro M15: meno noise, consolidation più chiare

2. **Session reference:** London come balance reference + NY come trading session? O generico "sessione precedente"?
   - Raccomandato: London → balance, NY → trade (allineato transcript)

3. **Consolidation detection:** Abilitare da subito o implementare in Phase 3 dopo test iniziali?
   - Raccomandato: Implementare subito ma con flag on/off

4. **Visual:** Disegnare solo balance zone corrente o anche storiche (ultimi 5-10 giorni)?
   - Raccomandato: Corrente + 5 precedenti (context visivo)

---

**Pronto per iniziare l'implementazione? O vuoi chiarire/modificare qualcosa nel design?**
