# Fabio Trend Following (Modello 1) — Specifica

**Stato:** In fase di refactoring — vedere `ANALISI.md` per problemi rilevati e piano di intervento.

## Overview

Modello di trend following di Fabio Valentino per mercati **out of balance** (trending). Usato per raggiungere 500%+ return nel Robins World Cup.

**Principio fondamentale:** Trade solo quando il mercato è OUT OF BALANCE, non in balance/ranging.

**Sessione:** New York (9:30-16:00 ET) — unica sessione con momentum sufficiente per trend puro.

**Strumenti:** NQ, ES (futures)

**Timeframe:** 1 minuto per execution, 5-15 minuti per identificare balance zones.

---

## Framework Centrale: Out of Balance

### 1. Identificazione Balance Zone

**Balance = Bell Curve nel profilo volumetrico:**
- Volume concentrato intorno al POC (Point of Control)
- Value Area (VAH/VAL) contiene 70% del volume
- Prezzo che oscilla dentro VAH/VAL (acceptance)

**Strumenti:**
- Volume Profile su sessione (London o NY precedente)
- POC, VAH, VAL come riferimenti

### 2. Out of Balance Trigger

**Breakout della balance zone:**
- Prezzo rompe VAH (bullish) o VAL (bearish)
- Inefficiency (fair value gap) — candle che non transazionano efficientemente
- Profilo asimmetrico — volume sbilanciato verso un lato (no bell curve)

**Conferma:**
- 2-3 barre consecutive oltre VAH/VAL
- Aggression clusters (big orders) nella direzione del breakout

### 3. Target

**POC della balance zone precedente** — punto di equilibrio dove il mercato tende a tornare (mean reversion verso il centro dopo l'espansione).

---

## Pipeline di Entry (Step by Step)

### Step 1: Validazione Out of Balance

✓ Mercato ha rotto una balance zone (VAH/VAL)  
✓ Profilo volumetrico asimmetrico (no bell curve)  
✓ Sessione NY attiva

**Se NON out of balance → SKIP tutto il resto.**

### Step 2: Identificazione Low Volume Node

**Low Volume Node = zona con < 30% del volume medio dell'impulso.**

**Come identificare:**
1. Tracciare profilo dall'inizio dell'impulso (breakout) a ora
2. Calcolare volume medio per livello di prezzo
3. Identificare gaps dove volume < 30% della media

**Perché importante:** Zone a basso volume = reazione veloce (pochi contratti da assorbire prima che il prezzo acceleri).

### Step 3: Aggression Cluster (TRIGGER)

**Big orders ≥ 30 contratti** (NY) nella direzione del trend.

**Validazione:**
- Barre precedenti avevano ordini piccoli (balance)
- Barra corrente: cluster di big orders
- Continuation: 2-3 barre successive mantengono aggression

**Entry:** Sul cluster di aggression dentro un low volume node.

### Step 4: Entry/Stop/Target

**Entry:** Al prezzo del cluster di aggression (o limite poco oltre per fill garantito).

**Stop Loss:** 2-3 tick oltre il cluster di aggression — stop STRETTO, non "above the high".

**Target:** POC della balance zone precedente.

**Risk/Reward tipico:** 1:3 a 1:5.

---

## Segnali Secondari (Conferme)

### Absorption

**Pattern multi-barra:**
1. 3-5 barre con big sell pressure (o buy pressure)
2. Prezzo che non rompe supporto/resistenza (wick lunghi)
3. Breakout confermato nella direzione opposta

**Interpretazione:** I trapped traders vengono assorbiti, il mercato è pronto per accelerare.

### CVD Divergence

**Divergenza Prezzo vs Cumulative Delta:**
- Prezzo fa higher high, ma CVD fa lower high → debolezza trend
- Prezzo fa lower low, ma CVD fa higher low → forza nascosta

**Uso:** Solo come **conferma** in combinazione con aggression + low volume node, non come trigger isolato.

---

## Gestione della Posizione

### Scaling Out

**Primo target (50%):** A metà strada verso il POC.

**Secondo target (50%):** Al POC della balance zone.

### Trailing Stop

Quando il prezzo supera il primo target:
- Spostare stop a breakeven
- Monitorare aggression opposta (big sellers in long, big buyers in short)

### Exit Anticipato

**Segnali di inversione:**
1. Big orders nella direzione opposta (sellers in long, buyers in short)
2. Compression (range stretto dopo momentum)
3. CVD divergence confermata

---

## Implementazione ATAS

### State Machine

```
SEARCHING_BALANCE → BALANCE_FOUND → OUT_OF_BALANCE → IN_TRADE → CLOSED
```

### Dataseries

1. **Balance Zones** — rettangoli VAH/VAL delle balance precedenti
2. **POC Line** — linea orizzontale al POC (target)
3. **Entry Signals** — frecce sul grafico (buy/sell)
4. **Stop/Target Lines** — linee orizzontali per visualizzare R:R

### Log Output

Ogni segnale logga:
- Tipo (AGGRESSION, LOW_VOL_NODE, ABSORPTION, CVD_DIV)
- Bar number e timestamp
- Metriche (volume, delta, prezzo)
- Entry/Stop/Target calcolati

---

## Parametri Configurabili

| Parametro | Default | Descrizione |
|-----------|---------|-------------|
| MinBigTradeSize | 30 | Contratti minimi per aggression (NY) |
| ProfileLookback | 60 | Barre per calcolare profilo balance zone |
| LowVolumeNodePct | 30% | Threshold per low volume node |
| BreakoutConfirmBars | 2 | Barre consecutive per confermare out-of-balance |
| StopLossTicks | 3 | Tick oltre aggression per stop loss |
| EnableLogging | true | Log dettagliato su file |

---

## Roadmap Refactoring

**Vedere `ANALISI.md` per dettagli completi.**

### Phase 1 — Core Framework (Priority 1)

- [ ] State machine: BALANCE → OUT_OF_BALANCE
- [ ] Volume profile per balance zone detection
- [ ] Tracking last balance zone (VAH/VAL/POC)
- [ ] Out of balance validation (pre-filtro per tutti i segnali)

### Phase 2 — Signals Refactor (Priority 1)

- [ ] Low volume node: calcolo dall'inizio impulso
- [ ] Aggression: validazione cambio regime + continuation
- [ ] Entry/Stop/Target calculation

### Phase 3 — Visual + Polish (Priority 2)

- [ ] Disegno balance zones (rettangoli)
- [ ] Linee entry/stop/target
- [ ] Etichette con R:R ratio
- [ ] Absorption pattern multi-barra
- [ ] CVD divergence come conferma only

---

## Riferimenti

- **Transcript:** `../Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt`
- **Video:** https://www.youtube.com/watch?v=tvERE-Beu2U
- **Codice attuale:** `src/FabioTrendFollowing.cs`
- **Analisi problemi:** `ANALISI.md`

---

**Ultima revisione:** 2026-06-21  
**Prossimo step:** Refactor del framework centrale (out-of-balance detection).
