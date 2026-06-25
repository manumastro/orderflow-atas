# Chiarezza Sessioni e Timeline Trading

**Data:** 2026-06-25
**Documento:** Analisi timeline operativa e naming moduli

---

## 🕐 Timeline Completa Giornata Trading

### 1️⃣ London Session (08:00-16:00 London)
**Conversione timezone:**
- London: 08:00-16:00
- UTC: 07:00-15:00
- Italy: 09:00-17:00 (estate)
- NY: 03:00-11:00

**Cosa succede:**
- ✅ Balance zone **viene costruita**
- ✅ Profile accumulation (volume per price level)
- ✅ Alla fine (16:00 London): calcolo VAH/VAL/POC finali
- ❌ **Nessun breakout detection** (fuori sessione NY)
- ❌ **Nessun trigger mean reversion** (ancora in balance)

**Stato:** `BuildingSessionProfile` → `LondonSession` → `BalanceReady`

**Log esempio:**
```
[SESSION_START] London session started at bar 0 (09:00 Italy, 08:00 London, 07:00 UTC)
[SESSION_END] London session ended at bar 95 (16:55 Italy, 15:55 London, 14:55 UTC)
[BALANCE_READY] VAH=30236.00, VAL=30209.00, POC=30233.25
```

---

### 2️⃣ Overlap London-NY (09:30-11:00 NY = 14:30-16:00 London)
**Conversione timezone:**
- London: 14:30-16:00 (ultime 1.5h sessione London)
- NY: 09:30-11:00 (prime 1.5h sessione NY)
- Italy: 15:30-17:00
- UTC: 13:30-15:00

**Cosa succede:**
- ✅ Balance zone London già finalizzata
- ✅ **Breakout detection attivo** (siamo in NY session)
- ✅ Se breakout → stato `BreakoutPending` → `OutOfBalance`
- ❌ **Trigger mean reversion ancora NO** (balance non ancora rotta)

**Nota importante:** Breakout detection richiede sessione NY (09:30-16:00 NY), ma London finisce alle 16:00 London (11:00 NY), quindi la window per breakout è **09:30-11:00 NY (solo 1.5h)**.

**Log esempio:**
```
[Italy=2026-06-04 16:05:00, London=16:05:00, NY=11:05:00]
[BREAKOUT_PENDING] Bullish | Close=30729.75 > VAH=30605.25

[Italy=2026-06-04 16:10:00, London=16:10:00, NY=11:10:00]
[BREAKOUT_CONFIRMED] Direction: Bullish
[OUT_OF_BALANCE] Bullish | BreakoutBar=1248 | TargetPOC=30506.00
```

---

### 3️⃣ Post-London / NY Session (11:00-16:00 NY = 16:00-21:00 London)
**Conversione timezone:**
- London: 16:00-21:00 (post-London, NY ancora aperta)
- NY: 11:00-16:00 (resto sessione NY)
- Italy: 17:00-22:00
- UTC: 15:00-20:00

**Cosa succede:**
- ✅ Mercato **out-of-balance** (breakout già confermato)
- ✅ **Mean reversion triggers ATTIVI** ⚡
- ✅ Detection rejection candidates
- ✅ Entry su follow-through o POC reclaim
- ✅ Exit management (Target2/Stop)

**Stato:** `OutOfBalance` (permanente fino a nuova London session)

**Log esempio:**
```
[Italy=2026-05-28 09:45:00, London=08:45:00, UTC=07:45:00]
[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION

[Italy=2026-05-28 15:35:00, London=14:35:00, UTC=13:35:00]
[MR_TRIGGER] Direction=Short, Trigger=POC_LOSS_AFTER_HIGH_REJECTION

[Italy=2026-06-25 13:45:50, London=12:45:50, UTC=11:45:50, NY=07:45:50] ← FUORI NY!
[MR_AGGRESSION_CONFIRM] Direction=Long ← Entry valida!
```

---

### 4️⃣ Post-NY / Notte (16:00+ NY = 21:00+ London)
**Conversione timezone:**
- London: 21:00-08:00 (notte, no trading attivo)
- NY: 16:00-03:00 (chiuso)
- Italy: 22:00-09:00
- UTC: 20:00-07:00

**Cosa succede:**
- ✅ Mercato **ancora out-of-balance**
- ✅ **Mean reversion triggers ANCORA ATTIVI** ⚡
- ✅ Entry possibili anche di notte/mattina presto
- ✅ Sistema non ha filtro sessione per mean reversion

**Esempio reale oggi:**
```
[Italy=2026-06-25 13:45:50, London=12:45:50, UTC=11:45:50, NY=07:45:50]
[MR_AGGRESSION_CONFIRM] Direction=Long

NY time: 07:45 (prima apertura NY alle 09:30!)
Entry validissima, ha fatto +40.5 punti Target2! ✅
```

---

## 🎯 Conclusione Chiarezza Sessioni

### Breakout Detection
- **Quando:** Solo durante NY session (09:30-16:00 NY)
- **Dove:** Overlap London-NY (14:30-16:00 London = 09:30-11:00 NY)
- **Risultato:** 1.5h window per rilevare breakout

### Mean Reversion Triggers
- **Quando:** **SEMPRE** dopo breakout confermato
- **Dove:** Out-of-balance state (non ha filtro sessione)
- **Risultato:** Entry 24/7 finché mercato è out-of-balance

---

## 📋 Naming Moduli - Decisione Finale

### Problema Iniziale
- "Trend Following" = nome fuorviante
- "NY Breakout" = parzialmente corretto (breakout detection NY only)
- Ma mean reversion funziona **sempre dopo breakout**, non solo NY

### Soluzione Naming

#### Opzione A: Focus su Timing
```
FabioOrderFlow.cs
├── BalanceZoneTracker (shared)
├── PostLondonMeanReversionModule (Modello 2)
│   └── Trigger attivi post-London, durante/dopo overlap NY
└── PostLondonTrendFollowingModule (Modello 1, futuro)
    └── Impulse profiling post-breakout
```

**Pro:** Chiarezza timeline
**Contro:** Lungo, "PostLondon" confonde (funziona anche prima London se out-of-balance)

---

#### Opzione B: Focus su Meccanica ⭐ RACCOMANDATO
```
FabioOrderFlow.cs
├── BalanceZoneTracker (shared)
│   ├── London session profile building
│   ├── NY breakout detection (overlap window)
│   └── Out-of-balance state management
│
├── MeanReversionModule (Modello 2)
│   ├── Descrizione: "Fade sweeps back to POC after breakout"
│   ├── Prerequisito: OutOfBalance state
│   ├── Trigger detection: rejection, follow-through, POC reclaim
│   └── Session filter: NONE (funziona sempre se out-of-balance)
│
└── ImpulseFollowingModule (Modello 1, futuro)
    ├── Descrizione: "Follow impulse after breakout to low volume nodes"
    ├── Prerequisito: OutOfBalance state
    ├── Impulse profiling, aggression clusters
    └── Session filter: NONE (funziona sempre se out-of-balance)
```

**Pro:** 
- ✅ Nome descrive cosa fa il modulo (meccanica trading)
- ✅ Sessione gestita da `BalanceZoneTracker` (breakout NY only)
- ✅ Moduli operano su stato `OutOfBalance`, non su sessione specifica
- ✅ Più corto e leggibile

**Contro:** Nessuno

---

#### Opzione C: Focus su Stato Mercato
```
FabioOrderFlow.cs
├── BalanceZoneTracker (shared)
├── BalanceFadeModule (Modello 2)
└── BreakoutImpulseModule (Modello 1, futuro)
```

**Pro:** Descrittivo
**Contro:** "Fade" non è termine universale, "BalanceFade" ridondante

---

## ✅ Decisione Finale

**Opzione B: Focus su Meccanica**

### Naming Definitivo

```
FabioOrderFlow.cs (rinominato da FabioTrendFollowing.cs)
│
├── Input Parameters:
│   ├── EnableMeanReversion (default: true)
│   ├── EnableImpulseFollowing (default: false)
│   └── EnableLiveFootprintFirst (default: false)
│
└── modules/
    │
    ├── BalanceZoneTracker/
    │   ├── BalanceZoneTracker.cs
    │   │   ├── London session: 08:00-16:00 London
    │   │   ├── NY breakout detection: 09:30-16:00 NY (overlap window)
    │   │   ├── State management: Balance → OutOfBalance
    │   │   └── Public API per moduli
    │   └── BalanceZoneTracker.md
    │
    ├── MeanReversionModule/
    │   ├── MeanReversionModule.cs
    │   │   ├── Descrizione: Fade sweeps back to POC
    │   │   ├── Prerequisito: OutOfBalance state
    │   │   ├── Session filter: NONE ⚡
    │   │   ├── Trigger: rejection, follow-through, POC reclaim
    │   │   ├── Entry: aggression confirmation
    │   │   ├── Exit: Target2/Stop management
    │   │   └── Optional: Footprint-first detection
    │   └── MeanReversionModule.md
    │
    └── ImpulseFollowingModule/ (FUTURO)
        ├── ImpulseFollowingModule.cs
        │   ├── Descrizione: Follow impulse to low volume nodes
        │   ├── Prerequisito: OutOfBalance state
        │   ├── Session filter: NONE
        │   ├── Impulse profiling
        │   ├── Low volume node detection
        │   └── Aggression clusters
        └── ImpulseFollowingModule.md
```

---

## 📝 Note Importanti

### Session Filters

**BalanceZoneTracker:**
- ✅ London session filter (08:00-16:00 London) → profile building
- ✅ NY session filter (09:30-16:00 NY) → breakout detection

**MeanReversionModule:**
- ❌ **NO session filter** → funziona sempre se OutOfBalance
- ✅ Entry valida anche fuori NY (es. 07:45 NY oggi → +40.5 punti!)

**ImpulseFollowingModule (futuro):**
- ❌ **NO session filter** → funziona sempre se OutOfBalance
- 💡 Possibile aggiungere filtro NY opzionale se Fabio dice "solo NY"

### Timeline Riassuntiva

```
08:00 London → 16:00 London: Balance zone building
14:30 London → 16:00 London: NY overlap, breakout detection window
16:00 London+: Out-of-balance, mean reversion triggers attivi 24/7
```

---

## 🚦 Status

- [x] Analisi sessioni completa
- [x] Naming definitivo scelto
- [ ] Approvazione finale
- [ ] Refactoring con nuovo naming

**Prossimo step:** Conferma naming e procedi con refactoring Phase 2.

