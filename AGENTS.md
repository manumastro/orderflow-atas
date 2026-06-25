# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

## Modelli

### FabioOrderFlow — Indicatore Unificato
**Path:** `Modello-1-TrendFollowing/`  
**File:** `src/FabioOrderFlow.cs`
**DLL:** `FabioOrderFlow.dll`

#### Modulo 1: London Mean Reversion (IMPLEMENTATO) ✅
**Parametro:** `EnableLondonMeanReversion` (default: `true`)

Approccio mean reversion per London fakeouts:
- Attivo durante London session (08:00-16:00) con profile preview live
- Entry su sweep → rejection → ritorno verso POC
- Target: POC/Target2 della balance zone
- Exit automatico al Target2/Stop
- **Performance:** 15 entry, win rate 57.1%, +408.5 punti net

**Documentazione:**
- Spec completa: `Modello-2-MeanReversion/FabioMeanReversion.md`
- Logica: `src/modules/BalanceZoneTracker/BalanceZoneTracker.cs` (attualmente integrato)
- Sessioni: `CHIAREZZA-DEFINITIVA.md`

**Entry opzionale footprint-first:**
- Parametro: `EnableLiveFootprintFirst` (default: `false`)
- Real-time sweep→rejection→entry
- Solo su live/replay

#### Modulo 2: Post-London Impulse (NON IMPLEMENTATO) ⏳
**Parametro:** `EnablePostLondonImpulse` (default: `false`)

Approccio impulse following post-breakout (futuro):
- Entry su aggression clusters in low volume nodes
- Target: POC della balance zone precedente
- Sessione: Post-London breakout

**Documentazione:**
- Spec: `Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`
- Moduli: `src/modules/<Modulo>/<Modulo>.md`

## Build & Deploy

```bash
cd Modello-*/src
dotnet build -c Release
deploy.bat  # copia DLL in %APPDATA%\ATAS\Indicators\
```

## Documentazione

- **Log reading:** `docs/atas/log-reading.md`
- **Modello 1:** `Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`
- **Modello 2:** `Modello-2-MeanReversion/FabioMeanReversion.md`
- **Transcript Fabio:** `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt`
- **API ATAS:** `docs/atas/api/`

## Focus Corrente

**Refactoring completo Modello 1** per implementare il framework centrale.

**📖 Mappa centrale:** `Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`

**📦 Moduli:** `Modello-1-TrendFollowing/src/modules/<Modulo>/<Modulo>.md`

Ordine di implementazione:
1. `BalanceZoneTracker`
2. `ImpulseProfiler`
3. `LowVolumeNodeDetector`
4. `AggressionDetector`
5. `TradeManager`
6. `ConfirmationLayer`
7. `VisualRenderer`
