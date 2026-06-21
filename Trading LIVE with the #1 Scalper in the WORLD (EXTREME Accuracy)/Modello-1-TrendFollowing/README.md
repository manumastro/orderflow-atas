# Fabio Trend Following — Documento Centrale

## Executive Summary

Implementazione ATAS del modello di trend following di Fabio Valentino (Robins World Cup 500%+ return).

**Stato attuale:** Prototipo con segnali isolati — richiede rewrite architetturale completo.

**Principio fondamentale:** Trade solo quando il mercato è **OUT OF BALANCE** (trending), mai in balance/ranging.

**Documento operativo BalanceZoneTracker:** `BalanceZoneTracker.md` — fonte unica per ricerca, best practice, design e roadmap implementativa del modulo balance/out-of-balance.

---

## 1. Il Metodo di Fabio (dal Transcript)

### 1.1 Framework Centrale: Balance vs Out of Balance

**Il 90% dei trader perde perché:**
- Cerca trend following quando il mercato è in **balance** (bell curve volumetrica)
- Ignora il contesto volumetrico (auction market theory)
- Usa solo price action senza validare lo stato del mercato

**La soluzione di Fabio:**
> "If you wait for the market to get to a condition of out of balance your win rate will jump up by at least 20 to 30%" (6:23)

**Balance = Bell Curve:**
- Volume concentrato intorno al POC (Point of Control)
- 70% del volume dentro Value Area (VAH/VAL)
- Prezzo che oscilla dentro VAH/VAL
- Market participants transazionano "efficiently"

**Out of Balance = Asimmetria:**
- Rottura di VAH (bullish) o VAL (bearish)
- Profilo volumetrico sbilanciato (no bell curve)
- Inefficiency (fair value gaps — candle non transazionano efficiently)
- Momentum direzionale chiaro

### 1.2 La Pipeline di Entry (Step by Step)

**Step 1: Location (Context)**
> "The location is when you can efficiently trade when you are out of balance" (7:24-7:30)

✓ Identificare balance zone precedente (volume profile)  
✓ Confermare breakout (out of balance)  
✓ Sessione NY attiva (unica con momentum sufficiente)

**Step 2: Level Validation**
> "Now it comes handy to use supply and demand zones" (7:47)

✓ Identificare livelli strutturali (swing points)  
✓ Tracciare profilo volumetrico dell'impulso  
✓ Identificare low volume nodes (< 30% volume medio)

**Step 3: Aggression (Trigger)**
> "The trigger of the model is aggression and that target point" (16:14-16:21)

✓ Big orders ≥ 30 contratti (NY)  
✓ Cluster di aggression in low volume node  
✓ Continuation (2-3 barre successive con aggression)

**Step 4: Entry/Stop/Target**

- **Entry:** Sul cluster di aggression dentro low volume node
- **Stop:** 2-3 tick oltre aggression cluster (stop stretto)
- **Target:** POC della balance zone precedente

> "You can get protected exactly above the big sell aggression" (11:58)  
> "You get also the target point" (12:34)

### 1.3 Sessione: Solo New York

> "The best session to use this model it's 100% New York session" (16:39)

**Perché NY e non London:**
- London: alternanza continua balance ↔ out of balance
- NY: momentum unidirezionale, volatility esplosiva
- "This is New York. This is you see how much is compressed more retracement. This when you take it, it just explode" (1:02:54)

**Orario:** 9:30-16:00 ET (14:30-21:00 UTC) — core cash market session.

---

## 2. Stato del Codice Attuale

### 2.1 Struttura Implementata

**File:** `src/FabioTrendFollowing.cs` (~330 righe)

**Segnali implementati:**
1. AGGRESSION — Big trades ≥ 30 contratti
2. LOW_VOLUME_NODE — Zone a basso volume (60 barre lookback)
3. ABSORPTION — Ordini assorbiti senza follow-through
4. CVD_DIVERGENCE — Divergenza prezzo vs cumulative delta

**Filtro sessione:** NY 14:30-21:00 UTC

**Output:** Log + frecce sul grafico (no ordini automatici)

### 2.2 Problemi Architetturali Critici

#### ❌ Problema 1: Manca "Out of Balance" validation

**Cosa fa il codice:**
```csharp
if (!IsInNySession(candle)) return;
DetectAggression(bar, candle);  // cerca aggression su OGNI barra NY
```

**Cosa dovrebbe fare:**
```csharp
if (!IsInNySession(candle)) return;
if (!IsOutOfBalance()) return;  // PRE-FILTRO OBBLIGATORIO
DetectAggression(bar, candle);
```

Il codice cerca segnali indiscriminatamente su qualsiasi barra NY, ignorando lo stato del mercato (balance vs out of balance).

#### ❌ Problema 2: Manca Balance Zone Tracking

Non esiste tracking delle balance zones precedenti:
- Nessun calcolo di VAH/VAL/POC
- Nessuna identificazione di breakout
- Nessun target (POC) calcolato

#### ❌ Problema 3: Low Volume Node — Lookback Generico

**Codice attuale:**
```csharp
for (int i = bar - ProfileLookback; i <= bar; i++)  // lookback fisso 60 barre
```

**Dal transcript (1:05:21):**
> "plot the profile from the beginning to the end of the impulse"

Il profilo va calcolato **dall'inizio dell'impulso** (breakout della balance), non su finestra generica.

#### ❌ Problema 4: Aggression — Nessun Contesto

**Codice attuale:** Cerca big trades in modo isolato.

**Dal transcript (1:04:23):**
> "You are going from small orders balance to someone buying aggressively and continuation aggressively"

Serve validare:
1. Barre precedenti con ordini piccoli (balance)
2. Cambio di regime → big orders improvvisi
3. Continuation (2-3 barre successive)

#### ❌ Problema 5: Entry/Stop/Target Non Implementati

Il codice logga solo i segnali. Non calcola:
- Prezzo di entry
- Stop loss (2-3 tick oltre aggression)
- Target (POC della balance precedente)

#### ❌ Problema 6: Absorption Troppo Semplicistico

**Codice attuale:**
```csharp
if (candle.Delta < -AbsorptionMinDelta && candle.Close >= prev.Close)
```

**Dal transcript (1:07:51):**
> "sellers, okay, trying to push the market down but they are punching a wall"

Absorption è un **pattern multi-barra** (3-5 barre):
1. Big sell pressure ripetuta
2. Prezzo che non scende (wick lunghi)
3. Breakout confermato verso l'alto

#### ❌ Problema 7: CVD Usato Come Trigger

CVD divergence va usato solo come **conferma**, non trigger isolato. Serve in combinazione con aggression + low volume node.

---

## 3. Architettura Target

### 3.1 State Machine

```
SEARCHING_BALANCE
    ↓
BALANCE_FOUND (tracking VAH/VAL/POC)
    ↓
OUT_OF_BALANCE (breakout confermato)
    ↓
SEARCHING_ENTRY (aggression in low volume node)
    ↓
IN_TRADE (entry/stop/target attivi)
    ↓
CLOSED (target raggiunto o stop hit)
    ↓
SEARCHING_BALANCE (ricomincia)
```

### 3.2 Moduli Core

**Module 1: BalanceZoneTracker**
- Calcola volume profile su sessione (London/NY precedente)
- Identifica POC, VAH, VAL
- Traccia lo stato: BALANCE vs OUT_OF_BALANCE
- Rileva breakout (2+ barre oltre VAH/VAL)

**Module 2: ImpulseProfiler**
- Profilo volumetrico dall'inizio dell'impulso (breakout) a ora
- Identifica low volume nodes (< 30% volume medio)
- Traccia swing points strutturali

**Module 3: AggressionDetector**
- Filtra big orders ≥ threshold
- Valida cambio di regime (small → big orders)
- Richiede continuation (2-3 barre)

**Module 4: TradeManager**
- Calcola entry: prezzo dell'aggression cluster
- Calcola stop: 2-3 tick oltre aggression
- Calcola target: POC della balance zone precedente
- Risk/reward: 1:3 a 1:5

**Module 5: VisualRenderer**
- Disegna balance zones (rettangoli VAH/VAL)
- Linee entry/stop/target
- Etichette con R:R ratio

### 3.3 Dataseries ATAS

```csharp
// Zones
private List<DrawingRectangle> _balanceZones;  // VAH/VAL boundaries
private List<TrendLine> _pocLines;             // POC levels

// Trade levels
private ValueDataSeries _entrySignals;         // Buy/Sell arrows
private List<TrendLine> _stopLines;            // Stop loss levels
private List<TrendLine> _targetLines;          // Target levels

// Volume profile
private ValueDataSeries _pocIndicator;         // POC line
private ValueDataSeries _vahIndicator;         // VAH line
private ValueDataSeries _valIndicator;         // VAL line

// Secondary
private ValueDataSeries _cvdLine;              // Cumulative delta
private PaintbarsDataSeries _signalBars;       // Colora barre con segnali
```

---

## 4. Piano di Implementazione

### Phase 1: Core Framework (3-4 giorni)

**Priority 1A: BalanceZoneTracker**
- [ ] Calcolo volume profile sessione (London/NY)
- [ ] Identificazione POC (max volume)
- [ ] Calcolo VAH/VAL (70% volume area)
- [ ] State machine: BALANCE ↔ OUT_OF_BALANCE
- [ ] Rilevamento breakout (2+ barre oltre VAH/VAL)

**Priority 1B: ImpulseProfiler**
- [ ] Tracking inizio impulso (bar del breakout)
- [ ] Profilo volumetrico dall'impulso a ora
- [ ] Identificazione low volume nodes (< 30% avg)
- [ ] Visualizzazione rettangoli balance zones

**Priority 1C: TradeManager**
- [ ] Calcolo entry/stop/target
- [ ] Visualizzazione linee entry/stop/target
- [ ] Logging dettagliato con metriche R:R

### Phase 2: Signals Refactor (2-3 giorni)

**Priority 2A: AggressionDetector**
- [ ] Validazione cambio regime (small → big orders)
- [ ] Richiesta continuation (2-3 barre)
- [ ] Filtro: solo in low volume nodes + out of balance

**Priority 2B: Absorption Pattern**
- [ ] Pattern multi-barra (3-5 barre)
- [ ] Validazione "punching a wall" (wick analysis)
- [ ] Conferma breakout successivo

**Priority 2C: CVD Divergence**
- [ ] Refactor: solo come conferma (non trigger)
- [ ] Combinare con aggression + low volume node

### Phase 3: Polish & Testing (1-2 giorni)

**Priority 3A: Visual**
- [ ] Colori e stili professionali
- [ ] Etichette informative (R:R, entry price, etc)
- [ ] Toggle per attivare/disattivare layers

**Priority 3B: Parametri**
- [ ] Esporre parametri critici (big trade size, lookback, thresholds)
- [ ] Validazione parametri (range checking)
- [ ] Preset per diversi strumenti (NQ vs ES)

**Priority 3C: Testing**
- [ ] Backtest su 20+ giorni NY session
- [ ] Verifica visual su replay
- [ ] Log analysis (win rate, avg R:R)

---

## 5. Parametri di Default

| Parametro | Default | Range | Descrizione |
|-----------|---------|-------|-------------|
| **Balance Detection** |
| ProfileSessionBars | 60 | 30-120 | Barre per calcolare profilo balance zone |
| ValueAreaPercent | 70% | 60-80% | Volume % per calcolare VAH/VAL |
| BreakoutConfirmBars | 2 | 1-3 | Barre consecutive per confermare out-of-balance |
| **Aggression** |
| MinBigTradeSize | 30 | 20-50 | Contratti minimi per big order (NY) |
| AggressionContinuationBars | 2 | 2-4 | Barre di continuation richieste |
| **Low Volume Node** |
| LowVolumeThresholdPct | 30% | 20-40% | Threshold per low volume node |
| **Trade Management** |
| StopLossTicks | 3 | 2-5 | Tick oltre aggression per stop |
| TargetType | POC | POC/VAH/VAL | Target level |
| **Session** |
| SessionStart | 14:30 UTC | - | Inizio NY session |
| SessionEnd | 21:00 UTC | - | Fine NY session |

---

## 6. Metriche di Successo

**Target del modello (da transcript):**
- Win rate: 60-70% (vs 40-50% senza out-of-balance filter)
- Risk/Reward medio: 1:3 a 1:5
- Max drawdown: < 3R per giornata
- Setup frequency: 2-5 trade per sessione NY

**Metriche da loggare:**
- Numero balance zones identificate
- Breakout true vs false
- Entry in low volume node (%) 
- R:R effettivo per trade
- Win rate per tipo di segnale (aggression, absorption, CVD)

---

## 7. Note Implementative

### 7.1 Volume Profile ATAS

**API disponibile:**
```csharp
var levels = candle.GetAllPriceLevels();
foreach (var lvl in levels) {
    decimal price = lvl.Price;
    decimal volume = lvl.Volume;
    decimal ask = lvl.Ask;
    decimal bid = lvl.Bid;
}
```

**Calcolo POC:** `price` con `volume` massimo nel periodo.

**Calcolo VAH/VAL:** Espandere dal POC fino a coprire 70% del volume totale.

### 7.2 Session Detection

**Timezone management:**
```csharp
// ATAS candle.Time è sempre UTC
var utcTime = candle.Time;
var nyTime = TimeZoneInfo.ConvertTimeFromUtc(
    utcTime, 
    TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
);
```

**NY session:** 9:30-16:00 ET = 14:30-21:00 UTC (no DST adjustments — ATAS gestisce automaticamente).

### 7.3 Drawing Objects

**Balance zones (rettangoli):**
```csharp
var rect = new DrawingRectangle(
    startBar, vah,  // top-left
    endBar, val,    // bottom-right
    pen, brush
);
Rectangles.Add(rect);
```

**Entry/Stop/Target (linee):**
```csharp
var line = new TrendLine(
    startBar, price,
    endBar, price,  // horizontal line
    pen
) { IsRay = false };
TrendLines.Add(line);
```

---

## 8. Riferimenti

**Transcript completo:**  
`../Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt` (2035 righe)

**Video originale:**  
https://www.youtube.com/watch?v=tvERE-Beu2U

**Codice attuale:**  
`src/FabioTrendFollowing.cs`

**Documentazione ATAS:**  
`../../docs/atas/api/`

---

## 9. Decision Log

**2026-06-21:** Abbandonato Modello 2 (mean reversion) — troppo discrezionale per implementazione retroattiva. Focus esclusivo su Modello 1.

**2026-06-21:** Analisi completa transcript + codice esistente → identificati 7 problemi architetturali critici. Necessario rewrite completo con framework centrale (balance zones, out-of-balance validation, trade management).

---

**Documento vivo — aggiornare ad ogni milestone del refactoring.**
