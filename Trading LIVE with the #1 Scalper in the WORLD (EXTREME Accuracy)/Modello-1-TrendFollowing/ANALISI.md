# Analisi Completa: Modello 1 - Trend Following

## Stato Attuale del Codice

**File:** `Modello-1-TrendFollowing/src/FabioTrendFollowing.cs`

### Struttura Implementata

Il codice attuale implementa 4 segnali:
1. **AGGRESSION** — Big trades ≥ 30 contratti (trigger core)
2. **LOW_VOLUME_NODE** — Zone a basso volume nel profilo (60 barre lookback)
3. **ABSORPTION** — Grandi ordini assorbiti senza follow-through
4. **CVD_DIVERGENCE** — Divergenza prezzo vs cumulative delta

**Filtro sessione:** New York (14:30-21:00 UTC, cioè 9:30-16:00 ET)

**Output:** Solo log e frecce sul grafico — nessun ordine automatico.

---

## Problemi Rilevati dall'Analisi del Transcript

### 1. **Sessione NY: orario sbagliato**

**Codice attuale:**
```csharp
// NY: 9:30-16:00 ET = 14:30-21:00 UTC
return time >= new TimeSpan(14, 30, 0) && time <= new TimeSpan(21, 0, 0);
```

**Dal transcript (1:02:47):**
> "I want to show you the entity of the momentum to explain you why I do New York"

**Problema:** Non viene mai specificato l'orario esatto, ma Fabio menziona ripetutamente che NY è l'unica sessione per il trend model. L'orario attuale (9:30-16:00 ET) è corretto per il cash market, ma futures NQ/ES hanno sessioni più ampie.

**Azione:** Verificare se l'orario è corretto per futures, o se Fabio intende solo il core NY session (9:30-16:00 ET).

---

### 2. **Manca il concetto fondamentale: OUT OF BALANCE**

**Dal transcript (6:23, 8:26):**
> "if you wait for the market to get to a condition of out of balance your win rate will jump up by at least 20 to 30%"
> "What the market is telling us look I'm out of balance. I'm searching for a new level of balance"

**Problema critico:** Il codice NON valida se il mercato è "out of balance" prima di cercare segnali. Cerca aggression indiscriminatamente su qualsiasi barra NY.

**Cosa significa "out of balance" (dal transcript):**
- Il profilo volumetrico mostra distribuzione asimmetrica (non bell curve)
- Il mercato ha rotto una zona di balance precedente
- C'è momentum direzionale chiaro (inefficiency/fair value gap)

**Azione:** Implementare pre-filtro "out of balance" che verifica:
1. Rottura di una zona di balance (value area) precedente
2. Profilo asimmetrico (non bell curve) — volume sbilanciato verso un lato
3. Presenza di inefficiency (gap tra candle che indica aggressione unilaterale)

---

### 3. **Low Volume Node: implementazione incompleta**

**Dal transcript (1:05:21-1:05:34):**
> "plot the profile from the beginning to the end of the impulse and say look let's zoom out in here what we have we have a lack of volume here. So there is a huge area that we can use as a low volume node"

**Codice attuale:**
- Calcola profilo su lookback fisso di 60 barre
- Confronta il volume del prezzo corrente vs media

**Problema:** I low volume nodes vanno identificati **dentro l'impulso out-of-balance**, non su finestra generica. Fabio traccia il profilo "from the beginning to the end of the impulse" — cioè dal breakout della balance zone fino al prezzo attuale.

**Azione:** Refactor per:
1. Identificare l'inizio dell'impulso (breakout della balance zone)
2. Costruire profilo solo da quel punto in avanti
3. Identificare le zone con < 30% del volume medio dell'impulso

---

### 4. **Aggression: manca validazione del contesto**

**Dal transcript (1:04:23-1:04:36):**
> "if you know how this stuff works, can you understand that something is happening here? You are going from small orders balance to someone buying aggressively and continuation aggressively"

**Problema:** Il codice cerca big trades in modo isolato. Fabio cerca un **cambio di regime**: da "small orders balance" a "aggressive continuation".

**Azione:** Validare che:
1. Le barre precedenti avevano ordini piccoli (balance)
2. La barra corrente ha un cluster di big orders (≥30 contratti)
3. Ci sia continuation (barre successive mantengono aggressione nella stessa direzione)

---

### 5. **Target e Stop Loss: non implementati**

**Dal transcript (1:04:54, 11:58, 12:34):**
> "framing the level with a huge presence of buy aggression"
> "you can get protected exactly above the big sell aggression"
> "you get also the target point"

**Target:** POC della balance zone precedente (punto di equilibrio dove il mercato vuole tornare).

**Stop:** Appena oltre il cluster di aggression che ha triggerato l'entry — stop stretto, non "above the high".

**Problema:** Il codice logga solo i segnali, non calcola entry/stop/target.

**Azione:** Aggiungere:
1. Tracking della balance zone precedente (POC come target)
2. Calcolo dello stop loss (2-3 tick oltre l'aggression cluster)
3. Visualizzazione grafica dei livelli (linee orizzontali per entry/stop/target)

---

### 6. **Absorption: logica troppo semplicistica**

**Codice attuale:**
```csharp
// Big sells but price holds = buyers absorbing
if (candle.Delta < -AbsorptionMinDelta && candle.Close >= prev.Close)
```

**Dal transcript (1:07:51-1:07:58):**
> "if I see like you were saying here sellers, okay, trying to push the market down but they are punching a wall. They are not going through"

**Problema:** Absorption non è solo "delta negativo + prezzo che tiene". È un **pattern multi-barra** dove:
1. Venditori aggressivi (big red balls) per 2-3 barre
2. Prezzo che non scende (wick lunghi, close sopra i low)
3. Poi breakout verso l'alto (buyers vincono)

**Azione:** Implementare pattern multi-barra per absorption:
- Tracciare 3-5 barre con big sell pressure
- Verificare che il prezzo non rompa il supporto
- Triggerare solo quando c'è breakout confermato verso l'alto

---

### 7. **CVD Divergence: troppo generico**

**Codice attuale:** Confronta CVD e prezzo su 20 barre lookback fisso.

**Dal transcript (37:54):**
> "you see that at this level there is a lot of a aggression and the cumulative volume delta is doing here this"

**Problema:** CVD divergence va cercato in zone specifiche (low volume nodes, balance boundaries), non su qualsiasi barra.

**Azione:** Usare CVD divergence solo come **conferma** quando:
1. Siamo in un low volume node
2. C'è già aggression rilevata
3. CVD diverge dal prezzo (conferma che il trend sta perdendo forza)

---

## Piano di Refactoring

### Priority 1 — Fondamentali Mancanti

1. **Implementare rilevamento "Out of Balance"**
   - State machine: BALANCE → OUT_OF_BALANCE → RE_BALANCE
   - Profilo volumetrico per identificare balance zones (bell curve vs asimmetrico)
   - Tracking della last balance zone (VAH/VAL/POC)

2. **Refactor Low Volume Node**
   - Calcolare profilo dall'inizio dell'impulso (breakout)
   - Identificare gaps volumetrici dentro l'impulso

3. **Aggiungere Entry/Stop/Target**
   - Entry: sul cluster di aggression in low volume node
   - Stop: 2-3 tick oltre aggression
   - Target: POC della balance zone precedente

### Priority 2 — Raffinare i Segnali

4. **Migliorare Aggression detection**
   - Validare cambio di regime (small orders → big orders)
   - Richiedere continuation (3+ barre con aggression nella stessa direzione)

5. **Refactor Absorption**
   - Pattern multi-barra (3-5 barre)
   - Conferma breakout dopo absorption

6. **CVD come conferma, non trigger**
   - Usare solo in combinazione con aggression + low volume node

### Priority 3 — Visualizzazione

7. **Grafica migliorata**
   - Disegnare balance zones (rettangoli VAH/VAL)
   - Linee per entry/stop/target
   - Etichette con risk/reward ratio

---

## Domande da Chiarire

1. **Timeframe:** Fabio usa 1 minuto per l'execution, ma quale TF per identificare le balance zones? (5M? 15M?)

2. **Orario NY:** 9:30-16:00 ET è corretto o serve tutta la sessione futures (18:00-17:00 ET)?

3. **Parametri Big Trade:** 30 contratti è per NY. E per London (se mai implementassimo mean reversion)?

4. **Profile lookback:** Quanto indietro guardare per identificare la last balance zone? (60 barre? 100?)

---

## Conclusioni

**Il codice attuale è un prototipo che implementa i segnali isolati**, ma **manca completamente il framework centrale del modello di Fabio**:

✗ **Out of Balance validation** — non c'è  
✗ **Balance zone tracking** — non c'è  
✗ **Entry/Stop/Target calculation** — non c'è  
✗ **Context-aware signals** — segnali cercati indiscriminatamente  

**Per rendere il codice fedele al metodo di Fabio, serve un rewrite architetturale** che implementi prima il framework (balance zones, out-of-balance state), e poi i segnali come layer sopra quel framework.

**Stima complessità:** Rewrite completo ~300-400 righe, 3-4 giorni di implementazione + testing.
