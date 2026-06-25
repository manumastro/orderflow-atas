# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

---

## 📁 Progetto: FabioOrderFlow

**Path:** `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/FabioOrderFlow/`  
**Status:** ✅ v2.0.0 Production-ready (modular architecture)

**Quando lavori su questo progetto, leggi sempre AGENTS.md del progetto prima di agire.**

---

## 📚 Documentazione Progetto

**Central guide:** [`FabioOrderFlow/AGENTS.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/AGENTS.md)  
→ Path completi a tutti i documenti, workflow, regole per agents

**Quick start:** [`FabioOrderFlow/README.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/README.md)  
→ Overview, build, deploy, settings

---

## 📖 Documentazione per Modello

**Modello 2 — Mean Reversion (London):**  
[`FabioOrderFlow/docs/Modello-2-MeanReversion.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/Modello-2-MeanReversion.md)  
Strategy: fakeout detection, sweep → rejection → POC, aggression tracking

**Modello 1 — Trend Following (Post-London):**  
[`Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`](Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md)  
Strategy: impulse on balance zone breakout, low volume nodes (pianificato)

---

## 📖 Documentazione per Modulo Tecnico

**BalanceZoneTracker (core):**  
[`FabioOrderFlow/docs/modules/BalanceZoneTracker.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/modules/BalanceZoneTracker.md)  
Core tracker: balance zones, sessioni, volume profile, state machine

**SessionDetector (utility):**  
[`FabioOrderFlow/docs/modules/SessionDetector.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/modules/SessionDetector.md)  
Session detection, timezone conversions

---

## 🔧 Build & Deploy

```bash
cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/FabioOrderFlow/src"
dotnet build -c Release
./deploy.bat  # Copia DLL in %APPDATA%\ATAS\Indicators\
```

**Output:**
- DLL: `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll` (74KB)
- Logs: `%APPDATA%\ATAS\Logs\`

---

## 📊 Log Files

**Location:** `C:\Users\<User>\AppData\Roaming\ATAS\Logs\`

**Key tags:**
- `[SESSION_START]` / `[SESSION_END]` — Session transitions
- `[PROFILE_PREVIEW]` — Profile calculation
- `[MR_TRIGGER_M5]` — Mean reversion trigger
- `[MR_LIVE_AGGRESSION_*]` — Live aggression
- `[MR_OUTCOME]` — Outcome tracking

**Parsing guide:** [`docs/atas/log-reading.md`](docs/atas/log-reading.md)

---

## 🎯 Stato Progetto

**Architettura:** Modular (refactored June 2026)  
**Build:** ✅ 0 errors, 8 warnings (unused fields)  
**DLL size:** 74KB (-17% from v1.x monolith)  

**Modelli implementati:**
- ✅ Modello 2: London Mean Reversion (production)

**Modelli pianificati:**
- ⏳ Modello 1: Post-London Impulse (design phase)

---

## 📖 Documentazione ATAS

- **Log reading:** [`docs/atas/log-reading.md`](docs/atas/log-reading.md)
- **API reference:** [`docs/atas/api/`](docs/atas/api/)

---

## 🔗 Riferimenti Storici

- **Transcript Fabio:** [`Trading LIVE... .txt`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy).txt)
- **Sessioni clarification:** [`CHIAREZZA-DEFINITIVA.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/CHIAREZZA-DEFINITIVA.md)
- **Modello 2 original spec:** [`Modello-2-MeanReversion/FabioMeanReversion.md`](Modello-2-MeanReversion/FabioMeanReversion.md)
