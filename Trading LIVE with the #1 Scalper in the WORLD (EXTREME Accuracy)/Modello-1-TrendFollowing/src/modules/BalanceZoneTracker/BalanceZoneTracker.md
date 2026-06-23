# BalanceZoneTracker

## Status

✅ **Phase 1 Completata** - Hybrid approach implementato (commit 877df9b)

---

## 1. Scopo

Il `BalanceZoneTracker` è il primo modulo da implementare.

Responsabilità:

1. Costruire il volume profile della sessione London.
2. Calcolare `POC`, `VAH`, `VAL`.
3. Congelare la balance reference a fine London.
4. Monitorare la sessione New York per breakout confermato.
5. Esporre al resto del modello lo stato `BALANCE_READY`, `BREAKOUT_PENDING`, `OUT_OF_BALANCE`.
6. Fornire `POC` come target/reference per il `TradeManager`.

Non responsabilità:

- non cerca aggression;
- non cerca low volume node;
- non gestisce entry/stop/target;
- non usa CVD o absorption;
- non deve trovare ogni micro-consolidation.

---

## 2. Decisioni

| Area | Decisione |
|------|-----------|
| Balance source | London session chiusa |
| Trading window | New York RTH |
| Value Area | 70% del volume |
| POC | price level con volume massimo |
| VAH/VAL | espansione contigua dal POC |
| Breakout | 2 close consecutive fuori VAH/VAL |
| **Rendering** | **Box: High/Low, Breakout logic: VAH/VAL** |
| Timezone | `TimeZoneInfo`, no offset hardcoded |
| Timeframe | M5 raccomandato, non bloccante |
| Confini | congelati dopo fine London |
| Consolidation override | non in Phase 1 |

---

## 3. State Machine

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
```

### Stati

`NO_ZONE`

- Nessuna balance reference valida.
- Nessun altro modulo può produrre setup.

`BUILDING_SESSION_PROFILE`

- London session in corso.
- Il profilo viene aggiornato incrementalmente barra per barra.

`BALANCE_READY`

- London chiusa.
- `POC`, `VAH`, `VAL` calcolati e congelati.
- NY può monitorare breakout.

`BREAKOUT_PENDING`

- Prima close fuori `VAH` o `VAL`.
- Serve una seconda close nella stessa direzione.

`OUT_OF_BALANCE`

- Breakout confermato.
- Gli altri moduli possono iniziare la pipeline operativa.

---

## 4. Strutture Dati Target

```csharp
internal enum MarketState
{
    NoZone,
    BuildingSessionProfile,
    BalanceReady,
    BreakoutPending,
    OutOfBalance
}

internal enum BalanceType
{
    LondonSession,
    Consolidation
}

internal enum BreakoutDirection
{
    Bullish,
    Bearish
}
```

```csharp
internal sealed class BalanceZone
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

```csharp
internal sealed class MarketContext
{
    public MarketState State { get; set; } = MarketState.NoZone;
    public BalanceZone? CurrentZone { get; set; }
    public BreakoutDirection? PendingDirection { get; set; }
    public int PendingBreakoutBar { get; set; } = -1;
    public int ConsecutiveOutsideCloses { get; set; }
}
```

---

## 5. API da Esporre

Il modulo deve esporre dati semplici agli altri moduli:

```csharp
public bool HasBalanceZone { get; }
public bool IsOutOfBalance { get; }
public BreakoutDirection? Direction { get; }
public decimal? BalancePocTarget { get; }
public int? BreakoutBar { get; }
public decimal? VAH { get; }
public decimal? VAL { get; }
public decimal? POC { get; }
```

---

## 6. Session Detection

Usare sempre `TimeZoneInfo`.

```csharp
private readonly TimeZoneInfo _londonTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

private readonly TimeZoneInfo _newYorkTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
```

London:

```text
08:00–16:00 London local time
```

New York:

```text
09:30–16:00 New York local time
```

Note:

- `GMT Standard Time` gestisce GMT/BST.
- `Eastern Standard Time` gestisce EST/EDT.
- Non usare offset UTC fissi.

---

## 7. Volume Profile

### 7.1 Aggregazione Incrementale

Durante London, ogni barra aggiunge i propri price levels al profilo.

```csharp
var levels = candle.GetAllPriceLevels();
foreach (var level in levels)
{
    profile[level.Price] += level.Volume;
}
```

Regola:

```text
mai ricalcolare tutta la sessione a ogni barra
```

### 7.2 POC

```text
POC = price level con volume massimo aggregato
```

Tie-break iniziale:

```text
se due livelli hanno stesso volume, scegliere il prezzo più basso per determinismo
```

### 7.3 VAH/VAL

Algoritmo:

1. Ordina i livelli per prezzo.
2. Parti dal POC.
3. Accumula volume partendo dal POC.
4. A ogni step confronta livello sopra e livello sotto.
5. Aggiungi il lato con volume maggiore.
6. Continua fino al 70% del volume totale.
7. `VAH` = prezzo più alto incluso.
8. `VAL` = prezzo più basso incluso.

Non usare i top level non contigui: la value area deve essere contigua.

---

## 8. Breakout Detection

Bullish pending:

```text
Close > VAH
```

Bearish pending:

```text
Close < VAL
```

Conferma:

```text
2 close consecutive fuori dalla value area nella stessa direzione
```

False breakout:

```text
se il prezzo rientra tra VAL e VAH prima della conferma → reset a BALANCE_READY
```

Non usare wick/high/low come conferma primaria.

---

## 9. Edge Cases

### London senza dati

Se il profilo è vuoto o la sessione ha meno di `MinSessionBars`:

```text
non creare balance zone
state = NO_ZONE
log warning
```

### NY apre già fuori value

Non confermare immediatamente. Avvia `BREAKOUT_PENDING` e richiedi la seconda close fuori.

### Overlap London/NY

Phase 1 usa solo London chiusa. Se London non è chiusa, non usare livelli in formazione.

### POC poco significativo

Non bloccare nella prima versione. Loggare metriche per analisi futura.

---

## 10. Hybrid Approach: Rendering

**Problema identificato:**  
Quando il mercato si muove in trend durante London, la Value Area (VAH/VAL) diventa asimmetrica e non copre tutte le candele della sessione.

**Esempio:** Sessione scende da 30820 a 30450 → POC finisce a 30506 → VAH espande poco verso l'alto (30605) → prime candele (30820) fuori dalla Value Area.

**Soluzione implementata:**
- **Box visivo**: usa `High/Low` della sessione (copre 100% candele)
- **Breakout logic**: usa `VAH/VAL` (mantiene coerenza modello Fabio)
- **POC line**: invariato (prezzo con max volume)

```csharp
// Rendering: High/Low per box visivo
new DrawingRectangle(
    zone.StartBar,
    zone.High,    // ← range completo sessione
    zone.EndBar,
    zone.Low,
    outlinePen,
    fillBrush
);

// Business logic: VAH/VAL per breakout
var isBullishBreak = close > zone.VAH;
var isBearishBreak = close < zone.VAL;
```

**Vantaggi:**
1. Visual accuracy: il trader vede il range completo London
2. Coerenza modello: breakout detection invariato
3. Stabilità: non dipende da asimmetrie volume profile

**Impatto su moduli successivi:** Nessuno - la business logic usa VAH/VAL come previsto.

---

## 11. Visual

Elementi minimi:

- rettangolo `High/Low` (box visivo);
- linea `POC`;
- colore differente se breakout bullish/bearish.

Regola performance:

```text
creare drawing object una volta e aggiornare SecondBar
```

---

## 12. Log Attesi

File unico:

```text
FabioTrendFollowing_YYYY-MM-DD.log
```

Contiene un set pulito di eventi decisionali e diagnostica utile:

```text
[SESSION_START]
[SESSION_END]
[SESSION_EXTREMES]
[ZONE_READY]
[BREAKOUT_PENDING]
[BREAKOUT_CONFIRMED]
[OUT_OF_BALANCE]
[FALSE_BREAKOUT]
[PROFILE_PREVIEW]
[HIGH_REJECTION_CANDIDATE]
[LOW_REJECTION_CANDIDATE]
[NEW_SESSION_HIGH]
[NEW_SESSION_LOW]
[LONDON_PRE_CLOSE]
[MR_AGGRESSION_CONFIRM]
[MR_EARLY_TRIGGER]
[MR_TRIGGER]
[CUM_TRADES_REQUEST]
[CUM_TRADES_RESPONSE]
```

Log disattivati di default e riattivabili solo per debug tecnico:

```text
[BAR_CHECK]
[BAR_DETAIL]
[STATE]
[PROFILE_RANGE]
[PROFILE_DETAIL]
[POC_CALC]
[VALUE_AREA_CALC]
[DRAW_ZONE]
[VERIFY_COVERAGE]
```

Regola di analisi:

```text
Filtrare prima il file per fascia oraria.
Per entry footprint cercare MR_AGGRESSION_CONFIRM.
Per conferma di barra cercare MR_EARLY_TRIGGER e MR_TRIGGER.
Se non ci sono trigger, seguire: NEW_SESSION_LOW/HIGH -> LOW/HIGH_REJECTION_CANDIDATE -> PROFILE_PREVIEW.
```

Nota operativa:

```text
[PROFILE_PREVIEW] è volutamente rumoroso e aggiornato live/intrabar durante London.
Non va rallentato per ridurre spam mentre il Modello 2 è in fase diagnostica,
perché i trigger MR usano POC/VAH/VAL preview aggiornati.
```

I trigger mean reversion diagnostici includono `BarMode`:

```text
BarMode=HISTORICAL_CLOSED   // barra storica/chiusa, tipica di reload o replay
BarMode=LIVE_OR_LAST_BAR    // barra più recente o ancora in formazione
```

`[MR_EARLY_TRIGGER]` e `[MR_TRIGGER]` sono conferme di barra: descrivono quando la candela successiva conferma il fakeout o quando il prezzo reclama/perde il `POC preview`.

`[MR_AGGRESSION_CONFIRM]` è invece il log operativo dello studio footprint. Può nascere live da `OnCumulativeTrade`/`OnUpdateCumulativeTrade` oppure, dopo reload, da `RequestForCumulativeTrades`:

```text
EntryModel=FootprintCumulativeTradeLive
EntryModel=FootprintCumulativeTradeHistorical
EntryPrice=...
EntryAreaLow=...
EntryAreaHigh=...
Volume=...
MinVolume=...
VolumeRule=Hardcoded20
SweepTimeUtc=...
SecondsAfterSweep=...
StopReference=...
RiskPoints=...
Target1POC=...
RewardToPOC=...
Target2=...
RewardToTarget2=...
```

Per i long cerca prima il momento dello sweep del low della candidate e poi solo buy aggression successive; per gli short fa l'equivalente sullo sweep high. La soglia minima è provvisoriamente hardcoded a `20` per evitare overfitting; il transcript conferma il concetto di big trades/bubbles e cita `30 contracts on NASDAQ on the one minute`, ma non una regola completa per sessione. Questo è il log più vicino all'idea Fabio di ingresso sui big trades.

### Barra vs Footprint

```text
Barra = contesto e candidate.
Footprint = entry.
Barra successiva = conferma.
POC reclaim/loss = conferma conservativa e management.
```

La barra serve ancora per definire lo sweep/fakeout perché il fakeout è una condizione strutturale: nuovo high/low, rejection, close position, rapporto con `VAH/VAL preview`. Il footprint non sostituisce questa parte: la rende operativa, mostrando quando entrano i big trades dopo lo sweep.

---

## 13. Criteri di Validazione

Il modulo è valido se:

1. Disegna una sola London balance per sessione.
2. Congela `POC/VAH/VAL` a fine London.
3. Non sposta i livelli durante NY.
4. Conferma breakout solo con close, non wick.
5. Funziona su storico/replay.
6. Non genera setup se non c'è balance reference.
7. Build Release passa senza warning/errori.

---

## 14. Prima Implementazione

Ordine consigliato:

1. Creare classi/enum del modulo.
2. Integrare il modulo nel placeholder `FabioTrendFollowing`.
3. Implementare session detection.
4. Implementare profile incrementale London.
5. Calcolare POC/VAH/VAL a fine London.
6. Implementare state machine breakout.
7. Aggiungere log.
8. Aggiungere visual minimale.
