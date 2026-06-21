# Modello 1 — Trend Following Fabio

## 0. Scopo del Documento

Questo è l'unico documento di riferimento per `Modello-1-TrendFollowing`.

Accorpa:

- metodo di Fabio dal transcript;
- analisi del codice attuale;
- problemi architetturali rilevati;
- design del `BalanceZoneTracker`;
- ricerca online su Volume Profile, sessioni e timeframe;
- note dalla documentazione ATAS;
- roadmap implementativa.

Regola operativa: se una decisione non è in questo documento, non è ancora una decisione del modello.

---

## 1. Executive Summary

Il Modello 1 è un indicatore ATAS per trend following su futures ES/NQ basato sull'approccio di Fabio Valentino.

Principio fondamentale:

```text
Trade solo quando il mercato è OUT OF BALANCE.
Non cercare trend-following dentro una balance/range.
```

L'indicatore attuale è un prototipo: implementa segnali isolati come aggression, low volume node, absorption e CVD divergence, ma manca il framework centrale del metodo.

Il rewrite deve partire dal `BalanceZoneTracker`, perché senza una balance zone di riferimento non è possibile sapere se il mercato è davvero out-of-balance.

Obiettivo finale:

```text
London balance reference
→ NY breakout / out-of-balance
→ impulse profile
→ low volume node
→ aggression trigger
→ entry / stop / target
```

---

## 2. Metodo di Fabio dal Transcript

### 2.1 Balance vs Out of Balance

Fabio distingue chiaramente due condizioni:

**Balance:**

- il mercato accetta prezzo;
- il volume crea una distribuzione relativamente equilibrata;
- il prezzo ruota attorno al POC;
- i trade trend-following hanno bassa qualità.

**Out of Balance:**

- il mercato rifiuta il valore precedente;
- rompe una value area;
- cerca un nuovo livello di equilibrio;
- il momentum può accelerare.

Dal transcript:

> "If you wait for the market to get to a condition of out of balance your win rate will jump up by at least 20 to 30%."

Questa frase è il filtro principale del modello.

### 2.2 Pipeline del Modello

La pipeline corretta non è cercare aggression ovunque. È:

1. Identificare una balance zone precedente.
2. Calcolare `POC`, `VAH`, `VAL`.
3. Verificare se il mercato rompe la value area.
4. Confermare lo stato `OUT_OF_BALANCE`.
5. Profilare l'impulso dal breakout.
6. Trovare low volume node dentro l'impulso.
7. Cercare aggression cluster nel punto giusto.
8. Calcolare entry, stop e target.

### 2.3 Sessione

Fabio indica New York come sessione migliore per il trend model.

Per il nostro design:

```text
London = costruzione balance reference
New York = ricerca breakout / trend-following
```

Motivi:

- NY è la sessione con maggior volume e range per ES/NQ.
- London spesso crea livelli e range utili.
- Il breakout NY da value precedente è un contesto robusto e verificabile.

---

## 3. Stato del Codice Attuale

File principale:

```text
Modello-1-TrendFollowing/src/FabioTrendFollowing.cs
```

Segnali presenti:

1. `AGGRESSION` — big trades sopra soglia.
2. `LOW_VOLUME_NODE` — volume basso su lookback fisso.
3. `ABSORPTION` — delta forte ma prezzo che non segue.
4. `CVD_DIVERGENCE` — divergenza prezzo/CVD.

Problema: questi segnali sono cercati senza contesto strutturale.

Il codice attuale fa concettualmente questo:

```text
Se siamo in NY:
  cerca aggression
  cerca low volume node
  cerca absorption
  cerca CVD divergence
```

Il codice target deve fare questo:

```text
Se siamo in NY:
  se NON siamo out-of-balance: skip
  se siamo out-of-balance:
    profila impulso
    cerca low volume node
    cerca aggression contestuale
    calcola trade plan
```

---

## 4. Problemi Architetturali Critici

### 4.1 Manca Out-of-Balance Validation

Il codice cerca segnali su ogni barra NY. Questo è contrario al framework di Fabio.

Serve un pre-filtro obbligatorio:

```csharp
if (!IsOutOfBalance)
    return;
```

### 4.2 Manca Balance Zone Tracking

Non esiste tracking robusto di:

- POC;
- VAH;
- VAL;
- balance precedente;
- breakout confermato;
- target POC.

### 4.3 Low Volume Node su Lookback Generico

Il codice usa un lookback fisso. Fabio invece profila l'impulso:

```text
from the beginning to the end of the impulse
```

Quindi il low volume node deve essere calcolato dal breakout in poi, non sulle ultime N barre casuali.

### 4.4 Aggression Senza Contesto

L'aggression isolata non basta. Serve cambio di regime:

```text
small orders / balance
→ big orders / aggression
→ continuation
```

### 4.5 Entry / Stop / Target Mancanti

Il modello deve calcolare:

- entry sul cluster di aggression;
- stop 2-3 tick oltre il cluster;
- target sul POC della balance precedente.

### 4.6 Absorption Troppo Semplice

L'absorption non è solo:

```text
delta forte + close che tiene
```

È un pattern multi-barra:

1. pressione aggressiva ripetuta;
2. prezzo che non passa;
3. breakout opposto/conferma.

### 4.7 CVD Usato nel Posto Sbagliato

Il CVD deve essere conferma, non trigger primario.

---

## 5. Ricerca Online e Best Practice

### 5.1 Fonti Consultate

Fonti online:

- TradingView — Volume Profile concepts: `https://www.tradingview.com/support/solutions/43000502040-volume-profile-indicators-basic-concepts/`
- Edgeful — futures trading sessions: `https://www.edgeful.com/blog/posts/trading-sessions-explained`
- NexusFi — multi-timeframe futures workflow: `https://nexusfi.com/a/platforms/multi-timeframe-analysis-workflow`

Documentazione locale ATAS:

- `docs/atas/api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1Indicator.md`

### 5.2 Volume Profile

Best practice standard:

- `POC` = price level con volume massimo.
- `Value Area` = area che contiene circa il 70% del volume.
- `VAH` = limite superiore della value area.
- `VAL` = limite inferiore della value area.

Decisione:

```text
Value Area = 70%
POC = max volume price
VAH/VAL = espansione contigua dal POC
```

Non usare:

- VWAP come POC;
- centro del range come POC;
- top 70% dei livelli non contigui.

### 5.3 Timeframe

Decisione: M5 come riferimento principale.

Motivi:

- ES/NQ hanno volume sufficiente per un profilo affidabile su M5.
- M5 offre più granularità di M15.
- M15 può essere utile come validazione visiva, ma arriva più tardi.
- Il modello deve comunque funzionare sul timeframe del chart senza bloccare.

### 5.4 Sessioni Futures

Per ES/NQ:

- NY RTH 09:30–16:00 ET è la sessione con maggior volume e range.
- London è utile come pre-market / reference session.
- Asia è generalmente più lenta e meno rilevante per questo modello.

Decisione:

```text
Balance reference: London session chiusa
Trading window: New York RTH
```

### 5.5 Breakout

Best practice:

- usare close, non wick;
- richiedere conferma;
- evitare conferma immediata su gap/open.

Decisione:

```text
Out-of-balance = 2 close consecutive fuori VAH/VAL
```

---

## 6. Documentazione ATAS Rilevante

### 6.1 IndicatorCandle

`IndicatorCandle` espone dati OHLC e order-flow.

API rilevanti:

```csharp
var candle = GetCandle(bar);
var levels = candle.GetAllPriceLevels();
```

Proprietà utili:

```text
Open
High
Low
Close
Volume
Delta
Time
LastTime
MaxVolumePriceInfo
VWAP
```

### 6.2 PriceVolumeInfo

Ogni livello prezzo/volume contiene:

```text
Price
Volume
Ask
Bid
Ticks
Between
Time
```

Per il volume profile del tracker si usa principalmente:

```text
Price
Volume
```

`Ask` e `Bid` serviranno più avanti per aggression / imbalance, non per la balance base.

### 6.3 Performance

ATAS supporta profiling tramite `MeasurePerformance` da `BaseIndicator`.

Da usare solo se il tracker diventa lento.

Principio più importante:

```text
Non ricalcolare tutto il profilo ogni barra.
Aggiornare il profile incrementalmente.
```

---

## 7. BalanceZoneTracker — Scopo

Il `BalanceZoneTracker` è il primo modulo da implementare.

Responsabilità:

1. Costruire il profilo volumetrico London.
2. Calcolare `POC`, `VAH`, `VAL`.
3. Congelare la zona a fine London.
4. Monitorare NY per breakout.
5. Esporre lo stato `BALANCE` / `OUT_OF_BALANCE`.
6. Disegnare rettangolo value area e linea POC.

Non responsabilità:

- non cerca aggression;
- non calcola low volume node dell'impulso;
- non entra a mercato;
- non fa mean reversion;
- non cerca ogni micro-consolidation.

---

## 8. Decisioni Finali del BalanceZoneTracker

### 8.1 Approccio

Decisione: session-based prima, consolidation override dopo.

Versione 1:

```text
London profile chiuso
→ POC/VAH/VAL congelati
→ NY breakout monitorato
```

Versione futura:

```text
Se si forma una consolidation intra-session chiarissima
→ crea override opzionale
```

La consolidation override non va implementata nella prima fase perché reintroduce il problema che aveva reso fragile il Modello 2.

### 8.2 Confini Congelati

Una volta chiusa London:

```text
POC, VAH, VAL non cambiano più.
```

Questo è fondamentale. Il breakout deve avvenire contro confini fissi, non contro una value area che si sposta.

### 8.3 Stato del Mercato

Stati target:

```text
NO_ZONE
BUILDING_SESSION_PROFILE
BALANCE_READY
BREAKOUT_PENDING
OUT_OF_BALANCE
```

Significato:

- `NO_ZONE`: nessuna zona disponibile.
- `BUILDING_SESSION_PROFILE`: London in corso, profilo in costruzione.
- `BALANCE_READY`: London finita, zona congelata.
- `BREAKOUT_PENDING`: prima chiusura fuori VAH/VAL.
- `OUT_OF_BALANCE`: breakout confermato.

---

## 9. Strutture Dati Target

### 9.1 Enum

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
```

### 9.2 BalanceZone

```csharp
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
```

### 9.3 MarketContext

```csharp
private sealed class MarketContext
{
    public MarketState State { get; set; } = MarketState.NoZone;
    public BalanceZone? CurrentZone { get; set; }
    public BreakoutDirection? PendingDirection { get; set; }
    public int PendingBreakoutBar { get; set; } = -1;
    public int ConsecutiveOutsideCloses { get; set; }
}
```

### 9.4 API Interna Esposta agli Altri Moduli

```csharp
private bool HasBalanceZone => _context.CurrentZone?.IsReady == true;
private bool IsOutOfBalance => _context.State == MarketState.OutOfBalance;
private BreakoutDirection? OutOfBalanceDirection => _context.CurrentZone?.BreakoutDirection;
private decimal? BalancePocTarget => _context.CurrentZone?.POC;
private int? BreakoutBar => _context.CurrentZone?.BreakoutBar;
```

---

## 10. Session Detection

### 10.1 Regola Generale

Non usare offset hardcoded.

Usare sempre `TimeZoneInfo`:

```csharp
private readonly TimeZoneInfo _londonTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

private readonly TimeZoneInfo _newYorkTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
```

Questo gestisce automaticamente:

- GMT/BST per London;
- EST/EDT per New York.

### 10.2 London Session

```csharp
private bool IsLondonSession(DateTime utcTime)
{
    var londonTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _londonTimeZone);
    var time = londonTime.TimeOfDay;

    return time >= new TimeSpan(8, 0, 0)
        && time < new TimeSpan(16, 0, 0);
}
```

### 10.3 New York Session

```csharp
private bool IsNewYorkSession(DateTime utcTime)
{
    var nyTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, _newYorkTimeZone);
    var time = nyTime.TimeOfDay;

    return time >= new TimeSpan(9, 30, 0)
        && time < new TimeSpan(16, 0, 0);
}
```

### 10.4 Prima e Ultima Barra London

```csharp
private bool IsFirstLondonBar(int bar)
{
    if (bar <= 0)
        return false;

    return IsLondonSession(GetCandle(bar).Time)
        && !IsLondonSession(GetCandle(bar - 1).Time);
}

private bool IsLondonJustClosed(int bar)
{
    if (bar <= 0)
        return false;

    return !IsLondonSession(GetCandle(bar).Time)
        && IsLondonSession(GetCandle(bar - 1).Time);
}
```

Nota:

- Per session boundary usare preferibilmente `candle.Time`.
- Per log leggibili usare anche `candle.LastTime` se più utile.

---

## 11. Volume Profile

### 11.1 Aggregazione Incrementale

Il profilo va aggiornato barra per barra durante London.

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
    zone.EndBar = CurrentBar;
}
```

Non fare:

```text
ogni barra → ricalcola tutta London da zero
```

Perché diventa costoso e inutile.

### 11.2 Calcolo POC

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

Tie-break deterministico:

- in caso di stesso volume, prezzo più basso;
- eventualmente in futuro: prezzo più vicino al centro del range.

### 11.3 Calcolo VAH/VAL

Algoritmo corretto: espansione contigua dal POC.

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

### 11.4 Freeze della Zona

A fine London:

```text
calcola POC/VAH/VAL
set IsReady = true
state = BALANCE_READY
non aggiornare più i livelli
```

---

## 12. Breakout Detection

### 12.1 Regola Base

Bullish breakout:

```text
close > VAH
```

Bearish breakout:

```text
close < VAL
```

Conferma:

```text
2 close consecutive fuori dalla value area nella stessa direzione
```

### 12.2 Pseudo-Codice

```csharp
private void UpdateBreakoutState(int bar, IndicatorCandle candle, BalanceZone zone)
{
    if (_context.State != MarketState.BalanceReady &&
        _context.State != MarketState.BreakoutPending)
        return;

---

## 14. Edge Cases

### 14.1 London senza dati sufficienti

Se la sessione London ha poche barre o profilo vuoto:

```text
non creare zona
state = NO_ZONE
log warning
```

Default:

```csharp
private const int MinSessionBars = 12;
```

### 14.2 GetAllPriceLevels nullo

Può succedere su barre senza footprint disponibile.

Regola:

- saltare la barra;
- non crashare;
- se tutto il profilo resta vuoto, non creare zona.

### 14.3 Gap di apertura NY

Se NY apre già fuori `VAH/VAL`:

- non confermare subito;
- avvia `BREAKOUT_PENDING`;
- richiedi comunque seconda close fuori.

### 14.4 Overlap London / New York

London e NY possono sovrapporsi.

Per la prima versione:

```text
Usare solo London balance chiusa.
Se London non è ancora chiusa, non usare una zona in formazione per NY.
```

Questo evita breakout contro livelli mobili.

### 14.5 Profilo piatto

Se il profilo ha POC poco dominante, non bloccare la zona nella prima versione.

Loggare metriche utili:

```text
maxVolume
avgVolume
max/avg ratio
VA range
session range
```

Eventuali filtri si aggiungono solo dopo validazione visiva.

---

## 15. Parametri

Prima versione: pochi parametri, preferibilmente costanti.

```csharp
private const decimal ValueAreaPercent = 0.70m;
private const int BreakoutConfirmBars = 2;
private const int MinSessionBars = 12;
private const int MaxHistoricalZones = 10;
private const int HistoricalZonesToDraw = 5;
```

Parametri UI accettabili subito:

```text
EnableLogging
DrawBalanceZones
```

Parametri da NON esporre subito:

```text
ValueAreaPercent
BreakoutConfirmBars
Session times
POC algorithm
Consolidation thresholds
```

Motivo: evitare overfitting e mantenere il modello semplice.

---

## 16. Roadmap Implementativa

### Phase 1 — BalanceZoneTracker Session-Based

Obiettivo:

```text
London volume profile → POC/VAH/VAL → zona congelata
```

Implementare:

1. Strutture dati `BalanceZone`, `MarketContext`, enum.
2. TimeZoneInfo London/NY.
3. Rilevamento first/last London bar.
4. Profilo incrementale London.
5. Calcolo POC/VAH/VAL.
6. Freeze zona a fine London.
7. Drawing rectangle + POC line.
8. Logging transizioni.

Log atteso:

```text
[BALANCE_READY] London | POC=... | VAH=... | VAL=... | Bars=... | Volume=...
```

### Phase 2 — Out-of-Balance Detection

Obiettivo:

```text
NY close fuori value → pending → conferma → out-of-balance
```

Implementare:

1. `BALANCE_READY → BREAKOUT_PENDING`.
2. Conferma 2 close consecutive.
3. Reset false breakout.
4. `OUT_OF_BALANCE` con direzione.
5. Colore zona dopo breakout.
6. API interna per altri moduli.

Log atteso:

```text
[BREAKOUT_PENDING] Bullish | Close=... > VAH=...
[OUT_OF_BALANCE] Bullish | BreakoutBar=... | TargetPOC=...
[FALSE_BREAKOUT] Returned inside value area
```

### Phase 3 — ImpulseProfiler

Obiettivo:

```text
Profilo dall'inizio impulso al prezzo corrente
```

Implementare:

1. Start impulse = breakout bar confermata.
2. Profilo volume dal breakout in poi.
3. Identificazione low volume node sull'impulso.
4. Non usare più lookback fisso generico.

### Phase 4 — Aggression Contextuale

Obiettivo:

```text
Cercare aggression solo nel posto giusto
```

Implementare:

1. Big orders ≥ 30 contratti in NY.
2. Validazione cambio regime small orders → big orders.
3. Continuation 2-3 barre.
4. Filtro obbligatorio: out-of-balance + low volume node.

### Phase 5 — TradeManager

Obiettivo:

```text
entry / stop / target
```

Implementare:

1. Entry sul cluster aggression.
2. Stop 2-3 tick oltre cluster.
3. Target POC della balance precedente.
4. Disegno linee entry/stop/target.
5. Risk/reward loggato.

### Phase 6 — Absorption e CVD

Solo dopo core stabile.

Absorption:

- pattern multi-barra;
- pressione aggressiva ripetuta;
- prezzo che non passa;
- conferma opposta.

CVD:

- solo conferma;
- mai trigger primario;
- usare solo con contesto già valido.

### Phase 7 — Consolidation Override

Solo dopo validazione della versione session-based.

Possibile logica:

```text
lookback 15 barre M5
range ultime N barre < 2x ATR medio
crea balance consolidation solo se evidente
flag EnableConsolidationOverride
```

---

## 17. Criteri di Validazione Visiva

Il tracker funziona se:

1. La London balance viene disegnata una volta.
2. `POC` appare su un prezzo plausibile ad alto volume.
3. `VAH/VAL` contengono la value area, non tutto il range.
4. La zona non attraversa notti/sessioni in modo errato.
5. I livelli restano congelati dopo fine London.
6. Il breakout non viene confermato da una wick.
7. Due close fuori value confermano correttamente out-of-balance.
8. Il log non spamma ogni barra.

Esempio buono:

```text
London costruisce value area.
NY rompe sopra VAH con due close.
State = OUT_OF_BALANCE Bullish.
POC London resta target/reference.
```

Esempio cattivo:

```text
VAH/VAL cambiano durante NY.
Rettangolo attraversa più giorni.
Out-of-balance confermato su singolo spike.
POC fuori dalla value area.
```

---

## 18. Cose da Evitare

Non ripetere gli errori del Modello 2:

1. Balance detection su finestra mobile casuale.
2. Balance pronta solo perché sono passate N barre.
3. Confini che si spostano dopo conferma.
4. Breakout contro livelli mobili.
5. Troppi parametri prima della validazione.
6. Delta/CVD come gate della balance.
7. Trigger cercati prima del contesto.

Regola guida:

```text
Prima stabilità del riferimento.
Poi intelligenza del segnale.
```

---

## 19. Build & Deploy

Comandi:

```bash
cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Modello-1-TrendFollowing/src"
dotnet build -c Release
```

Deploy manuale DLL:

```bash
cp bin/Release/net10.0-windows/FabioTrendFollowing.dll "$APPDATA/ATAS/Indicators/"
```

Oppure:

```bash
deploy.bat
```

Nota: dopo ogni modifica codice, build Release obbligatoria.

---

## 20. Checklist Prima di Implementare

Conferme operative:

- Implementare prima dentro `FabioTrendFollowing.cs`, senza file separati.
- Nessun parametro UI nuovo tranne logging/drawing se necessario.
- Timeframe atteso M5, ma non bloccante.
- London chiusa = balance reference.
- NY RTH = sessione trading.
- Value Area = 70%.
- POC = max volume price.
- VAH/VAL = espansione contigua dal POC.
- Breakout = 2 close consecutive fuori VAH/VAL.
- POC/VAH/VAL congelati dopo London.

---

## 21. Decision Log

### 2026-06-21 — Pivot Modello 1

Modello 2 Mean Reversion abbandonato come implementazione programmatica. Mantenuto solo come riferimento discrezionale.

Focus esclusivo su Modello 1 Trend Following.

### 2026-06-21 — Analisi Codice Attuale

Rilevati sette problemi critici:

1. manca out-of-balance validation;
2. manca balance zone tracking;
3. low volume node su lookback generico;
4. aggression senza contesto;
5. entry/stop/target mancanti;
6. absorption troppo semplice;
7. CVD usato come trigger.

### 2026-06-21 — Ricerca BalanceZoneTracker

Decisioni finali:

- M5 come timeframe operativo principale;
- London balance reference;
- NY trading session;
- Value Area 70%;
- POC max volume;
- breakout con 2 close consecutive;
- consolidation override rimandata.

---

## 22. Fonte Unica

Questo file è l'unico documento del Modello 1.

Da ora in poi, ogni decisione, analisi, design o roadmap del Modello 1 va aggiornata solo qui:

```text
MODELLO-1-DOCUMENTAZIONE.md
```
