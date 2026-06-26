# London Mean Reversion Model

**Strategy:** Mean reversion durante London session su balance zones  
**Market State:** CONSOLIDATION (balanced market)  
**Session:** London (08:00-16:00 London time)  
**Timeframe:** M5 per setup, M1 per execution  
**Implementation:** Historical aggression detection only

---

## 📚 Core Concept (Da Fabio Valentino)

### Market Behavior: Balance vs Imbalance

**Auction Market Theory (AMT):**
- Market spende il 70% del tempo in BALANCE (consolidation)
- Solo il 30% in IMBALANCE (trending)
- Durante London session, il comportamento è prevalentemente **mean reverting**

**Il problema del trend following tradizionale:**
- Traders cercano breakout e continuazioni
- Ma se il breakout avviene DENTRO la value area del profilo → 70% probability di ritorno
- Solo breakout FUORI dalla value area hanno chance di continuazione

### Il Mean Reversion Edge

**Quando il mercato è in balance:**
1. Definisci la **balance zone** (consolidation range) usando Volume Profile
2. Identifica **POC** (Point of Control) = dove transaziona la maggior parte del volume
3. Aspetta che il prezzo esca dalla balance zone (**out of balance**)
4. **NON entrare subito!** Aspetta il **secondo movimento**
5. Quando il prezzo rientra verso POC, cerca **aggression** dai big players
6. Entry con loro, target = POC

**Perché funziona:**
- POC è dove i market participants sono disposti a transazionare
- Quando il prezzo va "deep discount" o "premium" rispetto al POC, tende a tornare
- È più facile che il mercato torni al POC che continui oltre la value area

---

## 🎯 Strategy Flow

```
STEP 1: BALANCE ZONE IDENTIFICATION
├─ Identifica consolidation range (compressed candles)
├─ Plot Volume Profile sulla balance zone
├─ Identifica: POC, VAH (Value Area High), VAL (Value Area Low)
└─ Balance zone = dove il 70% del volume è stato scambiato

STEP 2: WAIT FOR BREAKOUT (Out of Balance)
├─ Prezzo rompe VAH (breakout high) o VAL (breakout low)
├─ Market va "out of balance"
├─ Sta cercando nuovo equilibrio
└─ ❌ NON ENTRARE QUI! È il primo movimento (risky)

STEP 3: WAIT FOR REJECTION (Fake Breakout)
├─ Il breakout fallisce (rejection candle)
├─ Prezzo inizia a rientrare nella balance zone
├─ Conferma che il breakout era un "fakeout"
└─ Setup pronto per mean reversion

STEP 4: WAIT FOR RETRACEMENT (Second Movement)
├─ Prezzo rientra DENTRO la balance zone
├─ Questo è il SECONDO movimento → più sicuro
├─ "I'm back inside the balance - this probability is really high"
└─ Ora cerca entry point con aggression

STEP 5: ENTRY WITH AGGRESSION (Big Orders)
├─ Aspetta "big trades" (volume > threshold, es: 20-30 contracts)
├─ Entry quando vedi "bubble" di buy/sell orders
├─ "I jump in when the best ones are jumping in with big volume"
├─ Direction: VERSO IL POC (mean reversion)
└─ ✅ ENTRY CONFIRMED

STEP 6: TARGET & EXIT
├─ Target = POC della balance zone
├─ "70% probability market will reverse from POC"
├─ Take FULL position al POC (non partial)
├─ Stop loss = 1-2 ticks sotto/sopra high/low del rejection point
└─ Se sbagliato: "I want to be wrong IMMEDIATELY"
```

---

## 📋 Entry Rules (Historical Aggression Only)

### Prerequisites (Balance Zone Setup)

1. **Identify Balance Zone:**
   - Uso del profilo del giorno precedente (semplice)
   - OPPURE: Identifica manualmente consolidation range (compressed candles)
   - Plot Volume Profile → POC, VAH, VAL

2. **Confirm Consolidation:**
   - Market deve oscillare tra VAH e VAL senza breakout significativi
   - "It's protecting from breaking here and breaking here"

### Long Setup (After Low Breakout)

**Sequenza:**
1. ✅ Prezzo rompe VAL (breakout low) → Out of balance
2. ✅ Rejection: prezzo rientra sopra VAL → Fake breakout confirmed
3. ✅ Wait for retracement: prezzo torna DENTRO balance zone
4. ✅ Cerca aggression: big BUY orders (volume > 20-30 contracts)
5. ✅ Entry: quando vedi "bubble" di buy aggression
6. ✅ Direction: VERSO IL POC (upward mean reversion)

**Target:** POC  
**Stop:** 1-2 ticks sotto il low del rejection point  

### Short Setup (After High Breakout)

**Sequenza:**
1. ✅ Prezzo rompe VAH (breakout high) → Out of balance
2. ✅ Rejection: prezzo rientra sotto VAH → Fake breakout confirmed
3. ✅ Wait for retracement: prezzo torna DENTRO balance zone
4. ✅ Cerca aggression: big SELL orders (volume > 20-30 contracts)
5. ✅ Entry: quando vedi "bubble" di sell aggression
6. ✅ Direction: VERSO IL POC (downward mean reversion)

**Target:** POC  
**Stop:** 1-2 ticks sopra l'high del rejection point

---

## ⚠️ Critical Rules

### 1. NON Prendere il Primo Movimento

> "I don't take the first movement. I wait, I wait for the first breakout... then I get the retracement."

**Perché?**
- Il primo breakout può essere un vero trend change
- Rischi di entrare contro un nuovo trend
- Il secondo movimento (retracement) conferma che è fake breakout

### 2. Aspetta la Conferma dell'Aggression

> "I wait for the hand of the big market participants. I wait for big trades."

**Cosa cercare:**
- Volume > threshold (20-30 contracts per NASDAQ)
- "Bubble" on footprint chart
- Direzione verso POC

### 3. Se Sbagliato, Esci IMMEDIATAMENTE

> "If I'm wrong, I want to be wrong immediately."

**Risk Management:**
- Stop loss stretto (10-20 ticks tipicamente)
- No "let it breathe" - se va contro, esci
- "Move stop to break even" dopo small movimento favorevole

### 4. Stop Loss Placement

> "Put the stop loss 1-2 ticks below the high [per short], not above the high."

**Perché?**
- Sopra/sotto high/low ci sono molti ordini → slippage
- Market accelera quando rompe questi livelli
- 1-2 ticks inside = eviti slippage, salvaguardi capitale

### 5. Target = POC (FULL Position)

> "We take out the FULL position because probability is 70% market will reverse from POC."

**No partial exit:**
- POC ha 70% probability di reversal
- Non conviene tenere position per solo 30% chance
- Better: prendi tutto al POC, cerca nuovo setup

---

## 🚫 When NOT to Trade

### 1. Durante Compression Days

> "Consolidation is killing your win rate... maybe you take 5 small stop-loss."

**Quando skipare:**
- Market in tight range per ore
- Nessun clear breakout e retracement
- Molti false signal

### 2. Durante Strong Trends

**Se vedi:**
- Breakout oltre VAH/VAL con volume enorme
- Continuous momentum (no retracement)
- Market che continua out of balance
→ **NON mean revert!** Aspetta che torni in balance

### 3. Prima Barra della Sessione

**Evita:**
- Entry sulla prima M5 candle di London open
- Manca contesto e setup precedente
- Troppo volatile e imprevedibile

### 4. Late Session (Dopo 15:30 London)

**Cutoff:**
- Stop entries 30 minuti prima della chiusura
- Insufficient time per sviluppare trade verso POC
- Risk di essere stopped out on close

---

## 📊 Implementation Details

### Timeframe Strategy

**M5 (5 minuti):**
- Identifica balance zone
- Identifica breakout e rejection
- Frame setup generale

**M1 (1 minuto):**
- Execution level
- Identifica aggression entries
- Monitor order flow in dettaglio

### Volume Profile Settings

**Balance Zone:**
- Plot profile sulla consolidation range
- Timeframe: Dipende (può essere daily profile o intraday range)
- Key levels: POC, VAH (top 70%), VAL (bottom 70%)

### Aggression Filter

**Threshold:**
- NASDAQ: 20-30 contracts minimum
- Filtra "noise" (small orders)
- Focus su "big players"

**Come identificare:**
- Footprint chart: "bubbles" (large orders)
- Delta spike (bid/ask imbalance)
- Cluster di orders in pochi ticks

---

## 📈 Win Rate & Risk:Reward

### Expected Performance

**Win Rate:**
- Mean reversion in balance: ~60-70%
- Durante compression: può scendere a 40-50%
- Trade quality > quantity

**Risk:Reward:**
- Minimum: 1:1 (se POC vicino)
- Common: 1:2 to 1:5
- Depends su distanza entry-POC

**Position Sizing:**
- Risk 0.25% - 0.5% per trade
- Stop loss stretto permette size maggiore
- "Aggressive risk management"

---

## 🔍 Example Scenario

### Setup Identification

```
1. Consolidation Range Identified:
   - Range: 29,800 - 29,900 (100 pts)
   - VAH: 29,880
   - POC: 29,850
   - VAL: 29,820

2. Breakout Occurs:
   - Prezzo rompe VAL (29,820) → low: 29,790
   - Out of balance condition

3. Rejection:
   - Candle chiude a 29,835 (15 pts dentro VAL)
   - Fake breakout confirmed

4. Retracement:
   - Prezzo rientra a 29,825 (inside balance zone)
   - Questo è il SECONDO movimento

5. Aggression Entry:
   - Vedi big BUY bubble a 29,825 (30 contracts)
   - Entry LONG @ 29,825
   - Stop: 29,788 (2 ticks sotto low)
   - Target: POC @ 29,850

6. Outcome:
   - Risk: 37 pts (29,825 - 29,788)
   - Reward: 25 pts (29,850 - 29,825)
   - R:R: 1:0.67 (non ideale ma POC target è objective)
```

---

## 🛠️ Technical Implementation

### Module: LondonMeanReversionModel

**Status:** ✅ Production (Historical Detection Only)  
**Version:** 3.0 (Refactored for clarity)

### Detection Logic

**Phase 1: Balance Zone Tracking**
```
- Build Volume Profile per session
- Track POC, VAH, VAL dinamicamente
- Identify quando prezzo esce dalla value area
```

**Phase 2: Breakout & Rejection Detection**
```
- Detect new session high/low (breakout)
- Check rejection candle (close back inside VAL/VAH)
- Log rejection as potential setup
```

**Phase 3: Aggression Confirmation**
```
- Scan historical cumulative trades dopo rejection
- Filter by volume (> MinAggressionVolume)
- Filter by direction (towards POC)
- Filter by timing (within timeout window)
- Confirm entry quando aggression matches criteria
```

**Phase 4: Outcome Tracking**
```
- Track position verso POC target
- Monitor MFE (Max Favorable Excursion)
- Monitor MAE (Max Adverse Excursion)
- Log exit: TARGET_HIT, STOP_HIT, or SESSION_CLOSE
```

### Configuration

```csharp
EnableLondonMeanReversion = true
MinAggressionTradeVolume = 20          // Minimum contracts per entry
AggressionTimeoutSeconds = 3600        // Max delay sweep→entry (1 hour)
SessionStartHour = 8                   // London session start (UTC+1)
SessionEndHour = 16                    // London session end
LateCutoffHour = 15                    // Stop new entries after 15:30
LateCutoffMinute = 30
```

### Logging Tags

```
[MR_BALANCE_ZONE] - Balance zone identified
[MR_SWEEP] - VAH/VAL sweep detected
[MR_REJECTION] - Rejection candle confirmed
[MR_TRIGGER] - Setup ready, waiting for aggression
[MR_AGGRESSION_CONFIRM] - Entry confirmed with big orders
[MR_TARGET_HIT] - POC reached
[MR_POSITION_CLOSED] - Trade closed (target/stop/session end)
```

---

## 📝 Key Takeaways

1. **Market State:** Funziona solo in BALANCE (consolidation), NON in strong trends
2. **Session Timing:** London session (08:00-16:00), NOT New York
3. **Entry:** SECONDO movimento (retracement), NON primo breakout
4. **Confirmation:** Big orders (aggression), NON price action alone
5. **Target:** POC, take FULL position
6. **Stop:** Stretto (1-2 ticks inside high/low), exit FAST se wrong
7. **Risk Management:** Aggressive (small stops, quick exits)

---

## 🎓 Philosophy

> "Why do I call it a MODEL and not a STRATEGY? Because market is a dynamic entity. You cannot cage it with strict rules. You need to understand the NARRATIVE."

**Key Principles:**
- Non predire, LEGGI il mercato
- Aspetta conferme oggettive (volume, aggression)
- "Swim in the direction of the flow"
- Risk management aggressivo: "wrong immediately"
- Target oggettivo (POC) non soggettivo (R:R arbitrario)

---

## 📚 References

- Transcript: Fabio Valentino @ Chart Fanatics
- Concept: Auction Market Theory (AMT)
- Tools: Volume Profile, Footprint Charts, Order Flow
- Implementation: ATAS Platform, C# Indicator
