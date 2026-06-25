# Chiarezza Definitiva: Sessioni, Modelli e Architettura

**Data:** 2026-06-25
**Documento:** Verità completa dopo analisi transcript Fabio + docs + codice + log

---

## 🎯 Verità dai Transcript di Fabio

### Citazioni Chiave

**[16:39-17:25] — Sessioni e Modelli:**
> "The best session to use this model [Trend Following] it's **100% New York session**... during **London session** you see **out of balance back inside balance out of balance back inside balance**. This is called by traders **fake outs**... I **don't trade before New York session**. I only trade before New York session in the world cup but with a **mean reverting model**."

**[1:02:34-1:02:47] — Conferma:**
> "Even if I only use this in **New York session**, the model really works as also **reversion model in London** and New York."

### Interpretazione

**Fabio opera con DUE modelli distinti:**

1. **Trend Following (Modello 1):**
   - Sessione: **Solo NY** (09:30-16:00 NY)
   - Principio: Breakout accettato → follow impulse
   - Target: POC balance precedente
   
2. **Mean Reversion (Modello 2):**
   - Sessione: **London** (08:00-16:00 London) + eventualmente NY
   - Principio: Fakeout rifiutato → ritorno verso POC
   - Target: POC balance corrente
   - Fabio dice: "I only trade before New York session... with a mean reverting model"

**London = fakeouts:**
> "during London session you see out of balance back inside balance out of balance back inside balance. This is called by traders fake outs."

---

## 📊 Cosa Abbiamo Implementato Finora

### Sistema Attuale nel Codice

**File:** `FabioTrendFollowing.cs` + `BalanceZoneTracker.cs`

**Implementazione:**
1. ✅ London session profile building (08:00-16:00 London)
2. ✅ Profile preview live durante London (POC/VAH/VAL aggiornati)
3. ✅ NY breakout detection (09:30-16:00 NY, overlap window)
4. ✅ **Mean reversion triggers** (attivi DURANTE London con preview!)
5. ✅ Exit management (Target2/Stop)
6. ✅ Footprint-first (opzionale live)

**Stato mercato:**
- `BuildingSessionProfile` → durante London
- `LondonSession` → fine London
- `BalanceReady` → London finita, livelli congelati
- `BreakoutPending` → close sopra VAH/sotto VAL (NY session)
- `OutOfBalance` → breakout confermato (2 close consecutive)

### 🔍 Cosa Succede nei Log

**Durante London (08:00-16:00):**
```
[PROFILE_PREVIEW] Reason=live, POC=30600.00, VAH=30663.50, VAL=30547.25
[MR_TRIGGER] Direction=Long, Trigger=POC_RECLAIM_AFTER_LOW_REJECTION
↑ Usa preview profile CORRENTE, ancora in formazione!
```

**Fine London / Inizio Overlap NY (14:30-16:00 London = 09:30-11:00 NY):**
```
[SESSION_END] London session ended
[BALANCE_READY] VAH=30236.00, VAL=30209.00, POC=30233.25 ← Livelli congelati
[BREAKOUT_PENDING] Bullish | Close > VAH (solo se in NY session)
[BREAKOUT_CONFIRMED] → [OUT_OF_BALANCE]
```

**Post-London / NY Session (16:00+ London, 11:00-16:00 NY):**
```
[MR_TRIGGER] usa livelli balance PRECEDENTE (congelati)
[MR_AGGRESSION_CONFIRM] entry post-breakout
```

---

## 🎭 I Due Modelli - Architettura Corretta

### Modello 1: Trend Following (NON IMPLEMENTATO)

**Descrizione:** Follow impulse after breakout
**Sessione:** **Solo NY** (09:30-16:00 NY)
**Prerequisito:** OutOfBalance state (breakout confermato fine London)
**Pipeline:**
```
London balance finalized
→ NY breakout detection
→ OUT_OF_BALANCE confirmato
→ Impulse profiling
→ Low volume node detection
→ Aggression clusters
→ Entry on impulse direction
→ Target: POC balance precedente
```

**Timing:** Post-London, durante NY session
**Target:** Continuation impulse

---

### Modello 2: Mean Reversion (IMPLEMENTATO!)

**Descrizione:** Fade fakeouts back to POC
**Sessione:** **Durante London** (08:00-16:00) + post-breakout
**Prerequisito:** 
- Durante London: Profile preview live
- Post-breakout: OutOfBalance state
**Pipeline:**
```
Durante London:
→ Profile preview live (POC/VAH/VAL aggiornati)
→ Sweep high/low
→ Rejection detection
→ Entry su ritorno verso POC
→ Target: POC preview corrente

Post-Breakout:
→ Balance precedente congelata
→ Sweep high/low out-of-balance
→ Rejection
→ Entry su ritorno verso POC
→ Target: POC balance precedente
```

**Timing:** Durante London (fakeouts) + post-breakout
**Target:** Mean reversion verso POC

---

## 🔑 La Chiave: Profile Preview

### Problema Originale

**Modello 1:**
- Balance congelata alla fine London → semplice
- Breakout NY → livelli fissi

**Modello 2:**
- Balance in formazione durante London → complesso
- POC/VAH/VAL cambiano tick-by-tick → serve preview live

### Soluzione Implementata

```csharp
[PROFILE_PREVIEW] Reason=live
```

Durante la sessione London, il sistema calcola POC/VAH/VAL provvisori che si aggiornano ad ogni barra. I trigger mean reversion usano questi livelli preview per rilevare fakeouts/rejection DURANTE la costruzione della balance.

**Questo è il cuore del Modello 2!**

---

## 📋 Naming Definitivo Corretto

### Problema Naming Attuale

**File corrente:** `FabioTrendFollowing.cs`
- Nome: "Trend Following"
- Implementazione: Mean Reversion!
- Confusione totale ❌

### Soluzione Architetturale

```
FabioOrderFlow.cs (rinominato)
│
├── Input Parameters:
│   ├── EnableLondonMeanReversion (default: true) ← CORRENTE
│   ├── EnablePostLondonImpulse (default: false) ← FUTURO
│   └── EnableLiveFootprintFirst (default: false)
│
└── modules/
    │
    ├── BalanceZoneTracker/ (SHARED)
    │   ├── London session profile (08:00-16:00 London)
    │   ├── Profile preview live durante London
    │   ├── Balance finalized a fine London
    │   ├── NY breakout detection (09:30-16:00 NY)
    │   ├── OutOfBalance state management
    │   └── Public API per moduli
    │
    ├── LondonMeanReversionModule/ (MODELLO 2 - IMPLEMENTATO)
    │   ├── Descrizione: Fade London fakeouts back to POC
    │   ├── Sessione: Durante London (08:00-16:00)
    │   ├── Profile source: Preview live
    │   ├── Trigger: Sweep → rejection → POC reclaim
    │   ├── Entry: Aggression confirmation
    │   ├── Target: POC preview corrente
    │   ├── Exit: Target2/Stop management
    │   └── Optional: Footprint-first
    │
    └── PostLondonImpulseModule/ (MODELLO 1 - FUTURO)
        ├── Descrizione: Follow impulse after NY breakout
        ├── Sessione: Solo NY (09:30-16:00 NY)
        ├── Profile source: Balance finalized (congelata)
        ├── Prerequisito: OutOfBalance confirmed
        ├── Pipeline: Impulse → low volume nodes → aggression
        ├── Entry: On impulse direction
        └── Target: POC balance precedente
```

---

## 🕐 Timeline Completa Giornata

### 08:00-16:00 London — Balance Building + Mean Reversion

**Stato:** `BuildingSessionProfile` → `LondonSession`

**Cosa succede:**
- ✅ Profile accumulation (volume per price level)
- ✅ **Profile preview live** (POC/VAH/VAL aggiornati ogni barra)
- ✅ **LondonMeanReversionModule ATTIVO** ⚡
  - Rileva sweep sopra VAH preview / sotto VAL preview
  - Rileva rejection
  - Entry su ritorno verso POC preview
  - Target: POC preview corrente
- ❌ Nessun breakout detection (fuori NY session)

**Log:**
```
[PROFILE_PREVIEW] Reason=live, POC=30600.00, VAH=30663.50, VAL=30547.25
[LOW_REJECTION_CANDIDATE] Sweep low, delta turned positive
[MR_TRIGGER] POC_RECLAIM_AFTER_LOW_REJECTION
[MR_AGGRESSION_CONFIRM] Entry @ 30619.25
```

**Questo è il Modello 2 di Fabio!** Fade dei fakeouts London.

---

### 14:30-16:00 London (= 09:30-11:00 NY) — Overlap Window

**Stato:** Fine `LondonSession` → `BalanceReady`

**Cosa succede:**
- ✅ London session finisce (16:00 London)
- ✅ **Balance finalized** → POC/VAH/VAL congelati
- ✅ **NY breakout detection ATTIVO** (siamo in NY session 09:30-16:00)
  - 2 close sopra VAH → Bullish breakout
  - 2 close sotto VAL → Bearish breakout
- ✅ LondonMeanReversionModule ancora attivo (usa preview ultimi minuti)
- ❌ PostLondonImpulseModule NON ANCORA ATTIVO (nessun breakout confermato)

**Log:**
```
[SESSION_END] London session ended, Bars=90
[BALANCE_READY] VAH=30236.00, VAL=30209.00, POC=30233.25 ← CONGELATI!
[BREAKOUT_PENDING] Bullish | Close=30729.75 > VAH=30605.25
[BREAKOUT_CONFIRMED] Direction: Bullish
[OUT_OF_BALANCE] Bullish | TargetPOC=30506.00
```

**Window critica:** Solo 1.5h (09:30-11:00 NY) per rilevare breakout, perché London finisce alle 16:00 (=11:00 NY).

---

### 16:00+ London (= 11:00-16:00 NY) — Post-London / NY Trading

**Stato:** `OutOfBalance`

**Cosa succede:**
- ✅ Mercato out-of-balance (breakout confermato)
- ✅ **LondonMeanReversionModule ANCORA ATTIVO** ⚡
  - Usa livelli balance PRECEDENTE (congelati)
  - Rileva rejection su sweep out-of-balance
  - Entry su ritorno verso POC precedente
- ✅ **PostLondonImpulseModule DOVREBBE ATTIVARSI** (futuro)
  - Follow impulse direction
  - Low volume nodes
  - Aggression clusters
  - Entry on impulse continuation
- ✅ Entrambi i moduli possono coesistere!

**Log:**
```
[MR_TRIGGER] usa POC=30506.00 (balance precedente congelata)
[MR_AGGRESSION_CONFIRM] Entry @ 30150.75
[MR_POSITION_CLOSED] Target2 hit, +23.75 punti
```

**Nota importante:** Mean reversion funziona anche post-breakout! Non è limitato a London. Fabio dice che il modello "really works as also reversion model in London and New York."

---

### 16:00+ NY (= 21:00+ London) — Post-NY / Notte

**Stato:** `OutOfBalance` (permanente fino a nuova London session)

**Cosa succede:**
- ✅ **LondonMeanReversionModule ANCORA ATTIVO** 24/7 ⚡
- ✅ Entry possibili anche di notte (es. 07:45 NY oggi → +40.5 punti!)
- ❌ Nessun breakout detection (fuori NY session)

**Esempio reale oggi:**
```
[Italy=2026-06-25 13:45:50, London=12:45:50, NY=07:45:50]
[MR_AGGRESSION_CONFIRM] Direction=Long
→ Entry PRIMA dell'apertura NY (07:45 < 09:30)
→ Target2 hit, +40.5 punti ✅
```

---

## ✅ Decisioni Finali

### 1. Naming Moduli

**Raccomandato:**

```
- LondonMeanReversionModule (Modello 2, implementato)
- PostLondonImpulseModule (Modello 1, futuro)
```

**Perché:**
- ✅ "London" indica quando il modello è più attivo (fakeouts London)
- ✅ "PostLondon" indica prerequisito (breakout fine London)
- ✅ Nomi descrivono meccanica + timing
- ✅ Chiaro che sono modelli diversi per fasi diverse

**Alternative scartate:**
- ❌ "MeanReversionModule" - troppo generico, non indica sessione
- ❌ "PostLondonMeanReversionModule" - fuorviante, è attivo DURANTE London
- ❌ "BalanceFadeModule" - termine non universale

---

### 2. Session Filters

**LondonMeanReversionModule:**
- ❌ **NO session filter rigido**
- ✅ Attivo durante London (usa preview)
- ✅ Attivo post-breakout (usa balance congelata)
- ✅ Entry valide 24/7 se ci sono rejection

**PostLondonImpulseModule (futuro):**
- ✅ **Prerequisito:** OutOfBalance state
- ✅ **Session filter opzionale:** NY only (09:30-16:00 NY)
- 💡 Da decidere se Fabio vuole strict NY o 24/7 post-breakout

**BalanceZoneTracker:**
- ✅ London session filter (08:00-16:00) → profile building
- ✅ NY session filter (09:30-16:00) → breakout detection

---

### 3. Architettura Target

```
FabioOrderFlow.cs
│
├── LondonMeanReversionModule
│   ├── Durante London: usa profile preview live
│   ├── Post-breakout: usa balance precedente congelata
│   ├── Trigger: sweep → rejection → POC reclaim/loss
│   ├── Entry: aggression confirmation
│   ├── Target: POC
│   └── Session: 24/7 (più attivo durante London fakeouts)
│
└── PostLondonImpulseModule (futuro)
    ├── Prerequisito: OutOfBalance confirmed
    ├── Pipeline: impulse → low volume → aggression
    ├── Entry: follow impulse direction
    ├── Target: POC balance precedente
    └── Session: NY only (da confermare con Fabio)
```

---

## 🚦 Status

- [x] Analisi transcript Fabio
- [x] Analisi docs modelli
- [x] Analisi codice e log
- [x] Naming definitivo
- [ ] Approvazione finale
- [ ] Refactoring con naming corretto

---

## 📝 Note Finali

### Confusione Originale

Il file si chiamava `FabioTrendFollowing.cs` ma implementava **Mean Reversion** (Modello 2), non Trend Following (Modello 1).

### Chiarezza Acquisita

1. **Fabio opera con 2 modelli:**
   - London fakeouts (mean reversion)
   - NY breakout impulse (trend following)

2. **Abbiamo implementato Modello 2** (London mean reversion)

3. **Modello 1 è ancora da implementare** (NY impulse following)

4. **Mean reversion funziona DURANTE London** con profile preview, non solo post-breakout

5. **NY breakout detection** serve per attivare stato OutOfBalance, usato da entrambi i modelli

6. **LondonMeanReversionModule** è il nome corretto perché:
   - È più attivo durante London (fakeouts)
   - Usa profile preview live durante London
   - Continua a funzionare post-breakout (ma meno fakeouts)

---

**Prossimo step:** Conferma naming e procedi con refactoring usando `LondonMeanReversionModule` + `PostLondonImpulseModule`.
