# BalanceZoneTracker — Documento Centrale

## 0. Scopo

Questo documento è la fonte centrale per progettare e implementare il `BalanceZoneTracker` del Modello 1 — Trend Following.

Il modulo non deve replicare il vecchio Modello 2 Mean Reversion. Nel Modello 1 la balance zone non è il segnale finale, ma il contesto necessario per capire quando il mercato è uscito dal valore e può generare un trade trend-following.

Obiettivi del modulo:

1. Identificare una zona di balance affidabile.
2. Calcolare `POC`, `VAH`, `VAL` tramite volume profile.
3. Stabilire se il mercato è in `BALANCE` o `OUT_OF_BALANCE`.
4. Fornire un riferimento per target e invalidazione.
5. Disegnare sul grafico zone e POC in modo leggibile.
6. Funzionare retroattivamente su storico ATAS.

Non-obiettivi:

1. Non deve trovare ogni micro-compressione.
2. Non deve produrre segnali di mean reversion.
3. Non deve stimare trigger live-only come big trades storici non disponibili.
4. Non deve diventare un sistema discrezionale complesso con troppi parametri.

---

## 1. Sintesi della Ricerca

### 1.1 Fonti consultate

Ricerca online:

- TradingView — concetti base Volume Profile: `https://www.tradingview.com/support/solutions/43000502040-volume-profile-indicators-basic-concepts/`
- Edgeful — sessioni futures NY, London, Asia: `https://www.edgeful.com/blog/posts/trading-sessions-explained`
- NexusFi — multi-timeframe analysis per futures: `https://nexusfi.com/a/platforms/multi-timeframe-analysis-workflow`
- Materiale generale su Market Profile / Volume Profile: POC, Value Area, VAH, VAL, Initial Balance, RTH vs ETH.

Documentazione locale ATAS:

- `docs/atas/api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1Indicator.md`

Codice locale analizzato:

- `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Modello-1-TrendFollowing/src/FabioTrendFollowing.cs`
- Vecchi esperimenti del Modello 2 su balance zones e disegno grafico.

### 1.2 Conclusione principale

La zona di balance per il Modello 1 deve essere robusta, non perfetta.

La soluzione più sicura è:

1. Usare una balance zone session-based come riferimento principale.
2. Calcolare `POC`, `VAH`, `VAL` tramite volume profile standard.
3. Considerare `OUT_OF_BALANCE` solo dopo breakout confermato.
4. Aggiungere consolidation intra-session solo come override opzionale, non come base obbligatoria.

Approccio raccomandato:

```text
London session = crea balance reference
NY session     = cerca breakout / out-of-balance / trend-following
```

Motivo:

- La NY session è la sessione con maggiore volume e range per ES/NQ.
- Il transcript di Fabio indica NY come sessione principale del trend model.
- London spesso costruisce range/valore che NY può rompere.
- Session boundaries sono più robuste di una detection libera di compressioni.

---

## 2. Concetti Chiave

### 2.1 Balance

Una market balance è una zona in cui il mercato sta accettando prezzo e volume.

Caratteristiche:

- Volume concentrato attorno a un `POC`.
- Prezzo che ruota dentro `VAH` e `VAL`.
- Auction relativamente efficiente.
- Nessun impulso direzionale dominante.

Nel Volume Profile, la balance viene rappresentata da:

- `POC` — prezzo con volume massimo.
- `VAH` — limite superiore della value area.
- `VAL` — limite inferiore della value area.
- `Value Area` — area che contiene circa il 70% del volume.

### 2.2 Out of Balance

Il mercato è out of balance quando rifiuta la value area e cerca nuovo valore.

Condizioni operative:

- Chiusure oltre `VAH` per scenario bullish.
- Chiusure sotto `VAL` per scenario bearish.
- Conferma su più barre, non semplice wick.
- Momentum direzionale coerente dopo la rottura.

Nel Modello 1, questo è il filtro più importante: senza `OUT_OF_BALANCE` non si cercano entry trend-following.

### 2.3 Low Volume Node

Un low volume node è una zona del profilo con poco volume transato.

Nel Modello 1 non va calcolato su una finestra generica arbitraria, ma sull'impulso successivo al breakout. Questo appartiene alla fase successiva (`ImpulseProfiler`), non al `BalanceZoneTracker` di base.

Il `BalanceZoneTracker` deve solo fornire:

- Quale zona è stata rotta.
- Da quale barra parte l'impulso.
- Dove si trova il POC della balance precedente.

---

## 3. Decisioni Finali

### 3.1 Timeframe

Decisione: `M5` come timeframe principale per balance detection.

Motivi:

- ES/NQ hanno volume sufficiente per rendere M5 affidabile.
- M5 offre più granularità di M15 per il profilo volumetrico.
- M15 è più pulito ma può arrivare tardi nella detection.
- Multi-timeframe best practice: usare HTF/MTF per contesto e LTF per execution. Per questo modello M5 è un buon layer strutturale, mentre l'execution può essere più bassa.

Nota:

- Il modulo non deve forzare il timeframe nel codice.
- Deve funzionare sul timeframe del chart.
- Se il chart non è M5/M15, il log deve segnalarlo ma non bloccare l'indicatore.

### 3.2 Sessioni

Decisione: London crea la balance, NY la rompe.

Sessioni raccomandate:

```text
London:   08:00–16:00 London local time
New York: 09:30–16:00 New York local time
```

Uso tecnico:

- `TimeZoneInfo` obbligatorio.
- Nessun offset hardcoded.
- Gestione automatica BST/GMT e EST/EDT.

Time zone IDs Windows:

```csharp
GMT Standard Time      // London, gestisce BST/GMT
Eastern Standard Time  // New York, gestisce EST/EDT
```

### 3.3 Value Area

Decisione: 70% del volume totale.

Motivi:

- Standard Market Profile / Volume Profile.
- TradingView e strumenti professionali usano value area attorno al 70%.
- 68% sarebbe più statistico, ma 70% è più comune operativamente.

### 3.4 POC

Decisione: prezzo con volume massimo.

Non usare:

- VWAP come POC.
- Centro geometrico del range.
- Media ponderata dei prezzi.

POC corretto:

```text
POC = price level con massimo volume aggregato nel profilo
```

### 3.5 VAH/VAL

Decisione: espansione dal POC fino al 70% del volume.

Algoritmo:

1. Ordina i price levels per prezzo.
2. Parti dal POC.
3. Guarda il livello immediatamente sopra e sotto.
4. Aggiungi il lato con volume maggiore.
5. Continua finché il volume cumulato raggiunge il 70%.
6. `VAH` è il prezzo più alto incluso.
7. `VAL` è il prezzo più basso incluso.

Questo è più coerente con Market Profile rispetto a prendere semplicemente i livelli con volume più alto non contigui.

### 3.6 Breakout

Decisione: breakout confermato da 2 chiusure consecutive oltre VAH/VAL.

Regole:

- Bullish pending: close > VAH.
- Bearish pending: close < VAL.
- Conferma: seconda chiusura consecutiva fuori dalla value area.
- False breakout: se rientra nella value area prima della conferma, torna `BALANCE`.

Non usare wick/high/low come breakout primario.

Motivo:

- Il close conferma acceptance fuori valore.
- Le wick generano troppi falsi segnali.

### 3.7 Consolidation Override

Decisione: implementare dopo la versione session-based, con flag disattivabile.

Motivo:

- Il problema storico del Modello 2 era proprio individuare balance intra-session in modo stabile.
- Non bisogna reintrodurre subito quella complessità.
- Prima va validato il modello semplice: London balance → NY breakout.

Regola:

```text
Phase 1: solo session-based
Phase 2: consolidation override opzionale
```

---

## 4. Architettura Target

### 4.1 State Machine

```text
NO_ZONE
  ↓
BUILDING_SESSION_PROFILE
  ↓
BALANCE_READY
  ↓
BREAKOUT_PENDING
  ↓
OUT_OF_BALANCE
  ↓
NEW_BALANCE_READY / SESSION_RESET
```

Descrizione stati:

- `NO_ZONE`: nessuna zona affidabile disponibile.
- `BUILDING_SESSION_PROFILE`: London in corso, accumula volume profile.
- `BALANCE_READY`: London chiusa, POC/VAH/VAL congelati.
- `BREAKOUT_PENDING`: NY ha chiuso fuori VAH/VAL, attende conferma.
- `OUT_OF_BALANCE`: breakout confermato, il trend model può cercare setup.

### 4.2 Oggetti principali

```csharp
private enum MarketState
{
    NoZone,
    BuildingSessionProfile,
    BalanceReady,
    BreakoutPending,
    OutOfBalance
}

private enum BalanceType
{
    LondonSession,
    Consolidation
}

private enum BreakoutDirection
{
    Bullish,
    Bearish
}

private sealed class BalanceZone
{
    public BalanceType Type { get; set; }
    public int StartBar { get; set; }
    public int EndBar { get; set; }

    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal POC { get; set; }
    public decimal VAH { get; set; }
    public decimal VAL { get; set; }
    public decimal TotalVolume { get; set; }

    public bool IsReady { get; set; }
    public bool IsBroken { get; set; }
    public BreakoutDirection? BreakoutDirection { get; set; }
    public int? BreakoutBar { get; set; }

    public Dictionary<decimal, decimal> Profile { get; } = new();
}

private sealed class MarketContext
{
    public MarketState State { get; set; } = MarketState.NoZone;
    public BalanceZone? CurrentZone { get; set; }
    public BreakoutDirection? PendingDirection { get; set; }
    public int PendingBreakoutBar { get; set; } = -1;
    public int ConsecutiveOutsideCloses { get; set; }
}
```

### 4.3 Responsabilità del Tracker

Il `BalanceZoneTracker` deve esporre dati semplici al resto dell'indicatore:

```csharp
public bool HasBalanceZone => _context.CurrentZone?.IsReady == true;
public bool IsOutOfBalance => _context.State == MarketState.OutOfBalance;
public BreakoutDirection? Direction => _context.CurrentZone?.BreakoutDirection;
public decimal? TargetPOC => _context.CurrentZone?.POC;
public int? BreakoutBar => _context.CurrentZone?.BreakoutBar;
```

---

## 5. Algoritmo Operativo

### 5.1 Ogni barra

Pseudo-flow:

```text
OnCalculate(bar):

1. Leggi candle corrente.
2. Determina se siamo in London o NY usando TimeZoneInfo.
3. Se è prima barra London:
   - reset profilo London
   - crea nuova zona in BUILDING_SESSION_PROFILE
4. Se siamo in London:
   - aggiorna profilo con i price levels della barra
   - aggiorna high/low della zona
5. Se London è appena finita:
   - calcola POC/VAH/VAL
   - congela la zona
   - state = BALANCE_READY
6. Se siamo in NY e c'è una zona ready:
   - controlla breakout su close
   - aggiorna state machine
7. Aggiorna disegno.
8. Logga solo transizioni importanti.
```

### 5.2 Costruzione profilo incrementale

Non ricalcolare tutto ogni barra.

```csharp
private void AddCandleToProfile(BalanceZone zone, IndicatorCandle candle)
{
    var levels = candle.GetAllPriceLevels();
    if (levels == null)
        return;

    foreach (var level in levels)
    {
        if (level.Volume <= 0)
            continue;

        if (!zone.Profile.ContainsKey(level.Price))
            zone.Profile[level.Price] = 0;

        zone.Profile[level.Price] += level.Volume;
        zone.TotalVolume += level.Volume;
    }

    zone.High = Math.Max(zone.High, candle.High);
    zone.Low = Math.Min(zone.Low, candle.Low);
}
```

### 5.3 Calcolo POC

```csharp
private static decimal CalculatePoc(Dictionary<decimal, decimal> profile)
{
    return profile
        .OrderByDescending(level => level.Value)
        .ThenBy(level => level.Key)
        .First()
        .Key;
}
```

Tie-break:

- Se due prezzi hanno stesso volume, scegliere quello più vicino al centro del range sarebbe più raffinato.
- Per semplicità iniziale, `ThenBy(price)` è deterministico.

### 5.4 Calcolo VAH/VAL

```csharp
private static (decimal vah, decimal val) CalculateValueArea(
    Dictionary<decimal, decimal> profile,
    decimal poc,
    decimal valueAreaPercent)
{
    var levels = profile.OrderBy(x => x.Key).ToList();
    var pocIndex = levels.FindIndex(x => x.Key == poc);

    var totalVolume = profile.Values.Sum();
    var targetVolume = totalVolume * valueAreaPercent;

    var lower = pocIndex;
    var upper = pocIndex;
    var accumulated = levels[pocIndex].Value;

    while (accumulated < targetVolume)
    {
        var canGoLower = lower > 0;
        var canGoUpper = upper < levels.Count - 1;

        if (!canGoLower && !canGoUpper)
            break;

        var lowerVolume = canGoLower ? levels[lower - 1].Value : -1;
        var upperVolume = canGoUpper ? levels[upper + 1].Value : -1;

        if (upperVolume >= lowerVolume && canGoUpper)
        {
            upper++;
            accumulated += levels[upper].Value;
        }
        else if (canGoLower)
        {
            lower--;
            accumulated += levels[lower].Value;
        }
    }

    return (levels[upper].Key, levels[lower].Key);
}
```

### 5.5 Breakout detection

```csharp
private void UpdateBreakoutState(int bar, IndicatorCandle candle, BalanceZone zone)
{
    if (_context.State != MarketState.BalanceReady &&
        _context.State != MarketState.BreakoutPending)
        return;

    var close = candle.Close;

    var above = close > zone.VAH;
    var below = close < zone.VAL;
    var inside = close <= zone.VAH && close >= zone.VAL;

    if (_context.State == MarketState.BalanceReady)
    {
        if (above)
            StartBreakoutPending(bar, BreakoutDirection.Bullish);
        else if (below)
            StartBreakoutPending(bar, BreakoutDirection.Bearish);

        return;
    }

    if (_context.State == MarketState.BreakoutPending)
    {
        if (inside)
        {
            ResetBreakoutPending();
            return;
        }

        var sameDirection =
            (_context.PendingDirection == BreakoutDirection.Bullish && above) ||
            (_context.PendingDirection == BreakoutDirection.Bearish && below);

        if (!sameDirection)
        {
            ResetBreakoutPending();
            return;
        }

        _context.ConsecutiveOutsideCloses++;

        if (_context.ConsecutiveOutsideCloses >= BreakoutConfirmBars)
            ConfirmOutOfBalance(bar, zone);
    }
}
```

Con `BreakoutConfirmBars = 2`, la prima chiusura fuori avvia pending e la seconda conferma.

---

## 6. Session Detection

### 6.1 Principio

Non usare UTC hardcoded per London/NY perché DST cambia gli orari reali.

Usare sempre:

```csharp
TimeZoneInfo.ConvertTimeFromUtc(utcTime, timezone)
```

### 6.2 Codice raccomandato

```csharp
private readonly TimeZoneInfo _londonTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

private readonly TimeZoneInfo _newYorkTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

private bool IsLondonSession(DateTime utcTime)
{
    var londonTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _londonTimeZone);
    var tod = londonTime.TimeOfDay;

    return tod >= new TimeSpan(8, 0, 0)
        && tod < new TimeSpan(16, 0, 0);
}

private bool IsNewYorkSession(DateTime utcTime)
{
    var nyTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _newYorkTimeZone);
    var tod = nyTime.TimeOfDay;

    return tod >= new TimeSpan(9, 30, 0)
        && tod < new TimeSpan(16, 0, 0);
}
```

### 6.3 Prima barra sessione

```csharp
private bool IsFirstLondonBar(int bar)
{
    if (bar <= 0)
        return false;

    return IsLondonSession(GetCandle(bar).Time)
        && !IsLondonSession(GetCandle(bar - 1).Time);
}

private bool IsLastLondonBar(int bar)
{
    if (bar <= 0)
        return false;

    return !IsLondonSession(GetCandle(bar).Time)
        && IsLondonSession(GetCandle(bar - 1).Time);
}
```

Nota ATAS:

- Verificare se usare `Time` o `LastTime` in base al comportamento reale sul chart.
- Per session boundaries, `Time` è preferibile perché rappresenta l'apertura della candle.
- Per log leggibile, `LastTime` può essere usato come timestamp operativo.

---

## 7. Visualizzazione

### 7.1 Cosa disegnare

Disegnare solo elementi utili:

1. Rettangolo `VAH`/`VAL` della balance zone.
2. Linea orizzontale `POC`.
3. Colore diverso quando la zona è rotta.
4. Al massimo alcune zone storiche per contesto.

### 7.2 Colori raccomandati

```text
Balance ready:      grigio trasparente
Out of balance up:  blu trasparente
Out of balance down: rosso trasparente
POC:                arancione
```

### 7.3 Regola importante

Non ricreare gli oggetti grafici ogni barra.

Corretto:

- Crea una volta.
- Aggiorna `SecondBar` per estendere a destra.
- Cambia colore solo su transizione stato.

Sbagliato:

- `Clear()` e recreate ogni barra.
- Creare nuovi rettangoli per ogni update.

---

## 8. Performance

### 8.1 Problema principale

Il calcolo del profile può diventare costoso se si ricalcola tutto lo storico ogni barra.

Da evitare:

```text
Per ogni bar:
  per ogni barra della sessione:
    per ogni price level:
      aggrega volume
```

Questo diventa `O(N²)`.

### 8.2 Soluzione

Costruire il profilo incrementalmente durante London:

```text
Per ogni bar London:
  leggi solo i price levels della barra corrente
  aggiungi al dictionary profile
```

Questo è `O(N)`.

### 8.3 Recalculation ATAS

ATAS può ricalcolare tutto lo storico quando l'indicatore viene caricato. L'algoritmo deve essere deterministico:

- Reset su prima barra London.
- Accumulo progressivo.
- Freeze a fine London.
- Breakout detection in NY.

Non usare stato live-only per il tracker.

### 8.4 Profiling

ATAS espone strumenti di profiling in `BaseIndicator`:

```csharp
using (MeasurePerformance("BalanceZoneTracker"))
{
    // codice critico
}
```

Da usare solo se serve debug performance.

---

## 9. Edge Cases

### 9.1 Sessione London senza dati sufficienti

Se London ha meno di un numero minimo di barre, non creare zona.

Default raccomandato:

```text
MinSessionBars = 12 su M5
```

A M5 sono 60 minuti. È un minimo assoluto; normalmente London avrà molte più barre.

### 9.2 Profile vuoto

Se `GetAllPriceLevels()` ritorna null o zero livelli per troppe barre:

- Non calcolare POC/VAH/VAL.
- Loggare warning.
- Restare in `NO_ZONE`.

### 9.3 POC poco significativo

Se il profilo è piatto, il POC può essere meno utile.

Per la prima versione non aggiungere filtri. Loggare solo metriche:

```text
MaxVolume / AvgVolume
ValueAreaHeight
SessionRange
```

Questi dati serviranno per calibrare dopo.

### 9.4 Gap di apertura NY

Se la prima barra NY apre già fuori VAH/VAL:

- Non confermare immediatamente out-of-balance sulla prima barra.
- Avvia `BREAKOUT_PENDING`.
- Richiedi comunque una seconda chiusura fuori.

### 9.5 Overlap London/NY

London e NY possono sovrapporsi.

Regola semplice:

- London profile si costruisce fino alle 16:00 London local.
- NY breakout detection parte dalle 09:30 New York local.
- Se entrambe sono vere, NY può monitorare breakout usando l'ultima balance pronta disponibile.
- Se London non è ancora chiusa, usare la balance del giorno precedente oppure restare `NO_ZONE`.

Per la prima versione, più sicuro:

```text
Trade NY solo se esiste una London balance chiusa.
```

Questo evita usare una zona ancora in formazione.

---

## 10. Parametri Raccomandati

Per restare semplice, non esporre subito tutto nella UI. Usare costanti interne.

```csharp
private const decimal ValueAreaPercent = 0.70m;
private const int BreakoutConfirmBars = 2;
private const int MinSessionBars = 12;
private const int MaxHistoricalZones = 10;
private const int HistoricalZonesToDraw = 5;
```

Parametri da esporre eventualmente dopo validazione:

```text
EnableLogging
DrawHistoricalZones
EnableConsolidationOverride
```

Parametri da NON esporre nella prima versione:

```text
ValueAreaPercent
BreakoutConfirmBars
Session times
POC algorithm
```

Motivo: evitare overfitting e mantenere il modello fedele e stabile.

---

## 11. Roadmap Implementativa

### Phase 1 — Session-Based Balance Tracker

Obiettivo: London balance → NY out-of-balance.

Da implementare:

1. `MarketState`, `BalanceZone`, `MarketContext`.
2. TimeZoneInfo per London e NY.
3. Rilevamento prima/ultima barra London.
4. Profilo incrementale su London.
5. Calcolo POC/VAH/VAL a chiusura London.
6. State `BALANCE_READY`.
7. Drawing rettangolo VAH/VAL e POC.
8. Log su creazione zona.

Output atteso:

```text
[BALANCE_READY] London | POC=... | VAH=... | VAL=... | Bars=... | Volume=...
```

### Phase 2 — Breakout / Out of Balance

Da implementare:

1. Breakout pending su close fuori VAH/VAL.
2. Conferma con 2 chiusure consecutive.
3. Reset su false breakout.
4. Stato `OUT_OF_BALANCE`.
5. Colore zona cambiato dopo breakout.
6. Esposizione `IsOutOfBalance`, `Direction`, `BreakoutBar`, `TargetPOC`.

Output atteso:

```text
[BREAKOUT_PENDING] Bullish | Close=... > VAH=...
[OUT_OF_BALANCE] Bullish | BreakoutBar=... | TargetPOC=...
[FALSE_BREAKOUT] Returned inside value area
```

### Phase 3 — Integrazione Modello 1

Da implementare dopo tracker stabile:

1. Aggression detection solo se `IsOutOfBalance`.
2. Impulse profile dal `BreakoutBar`.
3. Low volume node sull'impulso, non su lookback generico.
4. Entry/stop/target usando `TargetPOC`.

### Phase 4 — Consolidation Override

Solo dopo validazione visiva della Phase 1/2.

Da implementare:

1. ATR-based consolidation detector.
2. Lookback default 15 barre M5.
3. Range compression: range ultime N barre < 2x ATR medio.
4. Creazione zone consolidation solo se chiarissima.
5. Flag `EnableConsolidationOverride`.

---

## 12. Criteri di Validazione Visiva

Il tracker funziona se, su replay/storico:

1. La zona London viene disegnata una volta e non si ridisegna continuamente.
2. `POC` è dentro la zona e su un livello volume plausibile.
3. `VAH/VAL` contengono la parte principale del volume, non tutto il range.
4. La zona non attraversa più giorni in modo errato.
5. Il breakout non viene confermato su singola wick.
6. Il breakout viene confermato solo dopo acceptance fuori value.
7. I log sono leggibili e non spammano ogni barra.

Esempio buono:

```text
London costruisce range relativamente chiaro.
NY rompe sopra VAH con due close.
Tracker passa OutOfBalance Bullish.
POC London resta target/reference.
```

Esempio cattivo:

```text
Zona cambia dimensione ogni barra durante NY.
POC si sposta dopo breakout.
Rettangolo attraversa la notte.
OutOfBalance viene confermato da una wick.
```

---

## 13. Logica da Evitare

Non reintrodurre gli errori del Modello 2:

1. Finestra mobile generica per trovare balance.
2. Balance pronta dopo N barre senza vera session/reference.
3. VAH/VAL che cambiano dopo conferma.
4. Breakout contro confini che si spostano.
5. Troppi criteri fragili prima di avere validazione visiva.
6. Parametri esposti prematuramente.

Regola guida:

```text
Prima rendere stabile il riferimento.
Poi aggiungere intelligenza.
```

---

## 14. Raccomandazione Finale

Implementare prima una versione minimale ma solida:

```text
London volume profile chiuso
→ POC/VAH/VAL congelati
→ NY close fuori VAH/VAL
→ 2 close confermano out-of-balance
→ modulo espone stato e target POC
```

Questa versione è:

- Retroattiva.
- Deterministica.
- Semplice da validare visivamente.
- Coerente con Fabio.
- Basata su standard Volume Profile.
- Molto meno fragile della detection libera di balance zones.

Solo dopo aver verificato che questa base funziona bene su replay si aggiunge:

1. Consolidation override.
2. Impulse profiler.
3. Low volume node.
4. Aggression trigger.
5. Trade manager.

---

## 15. Checklist Prima di Codare

Prima dell'implementazione confermare:

- `BalanceZoneTracker` va implementato dentro `FabioTrendFollowing.cs` per ora, senza file separati.
- Nessun parametro UI nella prima versione tranne `EnableLogging` e forse `DrawZones`.
- Timeframe atteso: M5, ma non bloccante.
- Sessione balance: London chiusa.
- Sessione trading: NY RTH.
- Value Area: 70%.
- Breakout: 2 close consecutive fuori VAH/VAL.
- POC/VAH/VAL congelati dopo chiusura London.

Se questi punti sono accettati, la Phase 1 può partire.
