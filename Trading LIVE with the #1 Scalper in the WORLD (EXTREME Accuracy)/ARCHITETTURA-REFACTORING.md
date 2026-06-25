# Architettura e Piano Refactoring

**Data:** 2026-06-25
**Status:** Sistema funzionante, necessita riorganizzazione

---

## 📊 Situazione Attuale

### File Principale
- **Indicatore:** `FabioTrendFollowing.cs` (nome fuorviante)
- **Modulo core:** `BalanceZoneTracker.cs` (1835 righe, 42 metodi)
- **Funzionalità:** Mean Reversion (Modello 2) implementato
- **Performance:** ✅ 15 entry, win rate 57%, +408.5 punti net, tutto funzionante

### Problema
Il nome "TrendFollowing" non riflette ciò che fa l'indicatore (fa Mean Reversion).

---

## 🗂️ Analisi `BalanceZoneTracker.cs` (1835 righe)

### Responsabilità Attuali

#### 1️⃣ **Core Balance Zone (CONDIVISO)** ✅
Logica fondamentale per entrambi i modelli:

**Classi:**
- `MarketContext` (linea 10-30) - State machine mercato
- `BalanceZone` (linea 32-60) - Dati balance zone
- `MarketTimeZones` (MarketTimeZones.cs) - Conversioni timezone

**Metodi session management:**
- `OnBarUpdate` (linea ~349) - Update per barra
- `IsInLondonSession` (linea ~905) - Check sessione London
- `IsInNewYorkSession` (linea ~915) - Check sessione NY (09:30-16:00)
- `StartLondonSession` (linea ~455) - Inizializza profilo London
- `LogLondonSession` / `LogNewYorkSession` - Accumula volume

**Metodi profile calculation:**
- `FinalizeLondonSession` (linea ~570) - Calcola VAH/VAL/POC finali
- `TryCalculateProfilePreview` (linea ~1066) - Preview profile live
- `CheckForBreakout` (linea ~815) - Rileva breakout VAH/VAL (NY only)
- `ConfirmBreakout` (linea ~885) - Conferma breakout (NY only)

**Metodi rendering:**
- `DrawBalanceZone` - Rettangolo balance zone
- `DrawPOCLine`, `DrawVAHLine`, `DrawVALLine` - Livelli

**Entry point:**
- `OnHistoricalCumulativeTrades` (linea ~219) - Batch processing storico
- `OnLiveCumulativeTrade` (linea ~234) - Tick-by-tick live

---

#### 2️⃣ **Mean Reversion Module (MODELLO 2)** ✅
Logica specifica mean reversion, attualmente mescolata:

**Classi:**
- `MeanReversionTriggerLog` (linea ~65) - Log trigger
- `MeanReversionOutcome` (linea ~77) - Tracking outcome + exit management

**Variabili stato:**
```csharp
// Rejection candidates
private int _lastLowRejectionCandidateBar = -1;
private int _lastHighRejectionCandidateBar = -1;
private bool _lowRejectionPocReclaimed = false;
private bool _highRejectionPocLost = false;
private DateTime? _liveLowSweepTimeUtc;
private DateTime? _liveHighSweepTimeUtc;

// Outcome tracking
private readonly List<MeanReversionTriggerLog> _meanReversionTriggerLogs;
private readonly List<MeanReversionOutcome> _meanReversionOutcomes;
```

**Metodi trigger detection:**
- `LogPotentialRejection` (linea ~930) - Rileva HIGH/LOW_REJECTION_CANDIDATE
- `LogMeanReversionTriggerIfNeeded` (linea ~1044)
  - `LOW_REJECTION_FOLLOW_THROUGH` - Entry candela dopo rejection
  - `HIGH_REJECTION_FOLLOW_THROUGH`
  - `POC_RECLAIM_AFTER_LOW_REJECTION` - Entry su reclaim POC
  - `POC_LOSS_AFTER_HIGH_REJECTION`

**Metodi aggression confirmation:**
- `LogHistoricalAggressionConfirmation` (linea ~1353) - Storico batch
- `TryLogLiveLongAggression` (linea ~293) - Live tick-by-tick
- `TryLogLiveShortAggression` (linea ~321)
- `IsHistoricalAggressionInsideValue` (linea ~1434) - Check entry area
- `LogAggressionConfirmation` (linea ~1163) - Log finale

**Metodi exit management:**
- `EvaluateLongOutcome` (linea ~1145) - Check Target2/Stop long
- `EvaluateShortOutcome` (linea ~1213) - Check Target2/Stop short
- `UpdateOutcome` - Aggiorna MFE/MAE
- `LogPositionClosed` - Log chiusura

**Metodi helper:**
- `RegisterMeanReversionTrigger` (linea ~1125) - Salva trigger
- `GetMinAggressionTradeVolume` - Volume minimo (20)
- `GetCandidateSweepTime` - Timestamp sweep

---

#### 3️⃣ **Footprint-First Module (OPZIONALE)** ✅
Sistema entry alternativo real-time:

**Classi:**
- `LiveSweepCandidate` (linea ~103) - Sweep con VAH/VAL/POC salvati

**Variabili stato:**
```csharp
private LiveSweepCandidate? _activeLongSweep;
private LiveSweepCandidate? _activeShortSweep;
private readonly bool _enableLiveFootprintFirst;
```

**Metodi detection (#region linea 1678-1832):**
- `IsHighSweepTrade` - Big sell >VAH
- `IsLowSweepTrade` - Big buy <VAL
- `IsRejectionTrade` - Big trade direzione opposta
- `IsAggressionEntryTrade` - Entry confirmation
- `ProcessLiveHighSweep` / `ProcessLiveLowSweep` - Salva sweep
- `ProcessLiveRejection` - Marca rejection
- `ProcessFootprintEntry` - Log entry finale

**Gestione timeout:**
- Reset sweep senza rejection in `OnBarUpdate`
- Timeout 300s per sweep con rejection

---

## 🎯 Piano Refactoring: Opzione A

### Obiettivo
Un indicatore con moduli chiari e separati, mantenendo tutto funzionante.

### Struttura Target

```
FabioOrderFlow.cs (rinominato da FabioTrendFollowing.cs)
├── Input parameters:
│   ├── EnableMeanReversion (default: true)
│   ├── EnableTrendFollowing (default: false, futuro)
│   └── EnableLiveFootprintFirst (default: false)
│
└── modules/
    ├── BalanceZoneTracker/
    │   ├── BalanceZoneTracker.cs (CORE, ~800 righe)
    │   │   ├── Session management (London/NY)
    │   │   ├── Profile calculation (POC/VAH/VAL)
    │   │   ├── Breakout detection (NY only)
    │   │   ├── Visual rendering
    │   │   └── Public API per moduli
    │   │
    │   └── BalanceZoneTracker.md
    │
    ├── MeanReversionModule/
    │   ├── MeanReversionModule.cs (~600 righe)
    │   │   ├── Trigger detection (rejection, follow-through, POC reclaim)
    │   │   ├── Aggression confirmation (storico/live)
    │   │   ├── Exit management (Target2/Stop)
    │   │   ├── Outcome tracking
    │   │   └── Footprint-first (opzionale)
    │   │
    │   └── MeanReversionModule.md
    │
    └── TrendFollowingModule/ (FUTURO)
        ├── TrendFollowingModule.cs
        │   ├── Impulse profiling
        │   ├── Low volume node detection
        │   ├── Aggression clusters
        │   └── Entry management
        │
        └── TrendFollowingModule.md
```

---

## 📋 Refactoring Step-by-Step

### Phase 1: Preparazione (No Code Changes)
1. ✅ Documento analisi (questo file)
2. ⏳ Piano dettagliato spostamenti
3. ⏳ Test plan per validare nessuna regressione

### Phase 2: Extract MeanReversionModule
1. Creare `MeanReversionModule.cs`
2. Spostare classi:
   - `MeanReversionTriggerLog`
   - `MeanReversionOutcome`
   - `LiveSweepCandidate`
3. Spostare metodi trigger detection
4. Spostare metodi aggression confirmation
5. Spostare metodi exit management
6. Spostare metodi footprint-first
7. Constructor che riceve `BalanceZoneTracker` (dependency)

### Phase 3: Clean BalanceZoneTracker
1. Rimuovere logica mean reversion
2. Mantenere solo:
   - Session management
   - Profile calculation
   - Breakout detection
   - Visual rendering
3. Esporre public API:
   - `CurrentZone` (VAH/VAL/POC)
   - `State` (MarketState)
   - `IsInNewYorkSession()`
   - `OnBarUpdate()` notification

### Phase 4: Update FabioTrendFollowing.cs
1. Rinominare → `FabioOrderFlow.cs`
2. Aggiungere parametri:
   - `EnableMeanReversion` (default: true)
   - `EnableTrendFollowing` (default: false)
3. Istanziare moduli condizionalmente:
```csharp
_balanceTracker = new BalanceZoneTracker(...);

if (EnableMeanReversion)
    _meanReversionModule = new MeanReversionModule(_balanceTracker, ...);

if (EnableTrendFollowing)
    _trendFollowingModule = new TrendFollowingModule(_balanceTracker, ...);
```
4. Delegare chiamate ai moduli:
```csharp
protected override void OnCalculate(int bar, decimal value)
{
    _balanceTracker.OnBarUpdate(bar, candle, CurrentBar);
    _meanReversionModule?.OnBarUpdate(bar, candle);
    _trendFollowingModule?.OnBarUpdate(bar, candle);
}

protected override void OnCumulativeTrade(CumulativeTrade trade)
{
    _meanReversionModule?.OnLiveCumulativeTrade(trade);
    _trendFollowingModule?.OnLiveCumulativeTrade(trade);
}
```

### Phase 5: Testing
1. Build & deploy
2. Validare su storico: 15 entry identiche
3. Validare performance: +408.5 punti
4. Test live: entry funzionanti
5. Test parametri: EnableMeanReversion on/off

### Phase 6: Documentation
1. Aggiornare `AGENTS.md`
2. Aggiornare `FabioMeanReversion.md`
3. Creare `FabioOrderFlow.md` (overview indicatore)
4. Aggiornare `MODELLO-1-DOCUMENTAZIONE.md`

---

## 🔑 Principi Refactoring

1. ✅ **Non rompere nulla** - Sistema attuale funziona perfettamente
2. ✅ **Incrementale** - Una fase alla volta, test dopo ogni fase
3. ✅ **Backward compatible** - Stesso comportamento con parametri default
4. ✅ **Separazione responsabilità** - Ogni modulo fa una cosa
5. ✅ **Dependency injection** - Moduli ricevono BalanceZoneTracker, non lo creano
6. ✅ **Testabilità** - Ogni modulo testabile indipendentemente

---

## 🎯 Benefici Attesi

### Chiarezza
- ✅ Nome indicatore riflette cosa fa (`FabioOrderFlow`)
- ✅ Moduli separati concettualmente
- ✅ File < 800 righe ciascuno (vs 1835)

### Manutenibilità
- ✅ Modifiche mean reversion → solo `MeanReversionModule.cs`
- ✅ Implementazione trend following → nuovo file, zero impatto
- ✅ Bug fix isolati per modulo

### Flessibilità
- ✅ User sceglie quali moduli abilitare
- ✅ Possibilità di aggiungere altri moduli (es. Scalping)
- ✅ Test A/B tra modelli (entrambi attivi, log separati)

### Performance
- ✅ Balance zone calcolato una volta
- ✅ Moduli disabilitati → zero overhead
- ✅ Nessun impatto negativo (stesso codice, riorganizzato)

---

## ⚠️ Rischi

### Rischio 1: Regressioni
**Mitigazione:** Test completo dopo ogni phase, validazione entry identiche

### Rischio 2: Timing issues live
**Mitigazione:** Test su replay prima di live, log dettagliati

### Rischio 3: Complessità deployment
**Mitigazione:** Tutto in un DLL, stesso processo deploy attuale

---

## ❓ Domande Aperte

1. **Session filters mean reversion:**
   - Attualmente: funziona sempre dopo breakout
   - Fabio originale: probabilmente solo NY
   - Decisione: lasciare come ora o filtrare NY only?

2. **Nome moduli:**
   - `MeanReversionModule` vs `BalanceZoneFadeModule`?
   - `TrendFollowingModule` vs `BreakoutImpulseModule`?

3. **Footprint-first:**
   - Parte di `MeanReversionModule` (attuale) ✅
   - Oppure modulo separato `FootprintFirstModule`?
   - Proposta: lasciare dentro MeanReversionModule (è entry alternativo)

4. **Namespace:**
   - Attuale: `namespace FabioTrendFollowing;`
   - Target: `namespace FabioOrderFlow;`?

---

## 🚦 Status

- [x] Analisi completa
- [ ] Approvazione piano
- [ ] Phase 1: Preparazione
- [ ] Phase 2: Extract MeanReversionModule
- [ ] Phase 3: Clean BalanceZoneTracker
- [ ] Phase 4: Update FabioOrderFlow.cs
- [ ] Phase 5: Testing
- [ ] Phase 6: Documentation

---

**Prossimo step:** Review questo documento e decidere se procedere con refactoring o mantenere status quo.
