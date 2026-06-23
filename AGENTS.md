# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

## Modelli

### Modello 1 — Trend Following (IN REFACTORING)
**Path:** `Modello-1-TrendFollowing/`  
**📘 Documento Centrale:** `Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md` ⭐  
**Codice:** `src/FabioTrendFollowing.cs`

Approccio trend following di Fabio per mercati OUT OF BALANCE:
- Entry su aggression clusters in low volume nodes
- Target: POC della balance zone precedente
- Sessione: New York only
- **Stato:** Prototipo con segnali isolati — richiede rewrite architetturale completo

**Leggi `MODELLO-1-DOCUMENTAZIONE.md` per:**
- Mappa centrale del modello
- Principi globali e pipeline target
- Roadmap e ordine di implementazione
- Link ai documenti modulari

**Leggi anche `src/modules/<Modulo>/<Modulo>.md` quando lavori su un modulo:**
- Design operativo del modulo
- Input/output e state machine locali
- Criteri di validazione specifici

### Modello 2 — Mean Reversion (STUDIO DIAGNOSTICO)
**Path:** `Modello-2-MeanReversion/`  
**Spec:** `Modello-2-MeanReversion/FabioMeanReversion.md`  
**Codice:** `src/FabioMeanReversion.cs` (placeholder vuoto)

Approccio mean reversion basato su balance zones, fakeout e ritorno verso POC.  
Non è ancora un modello operativo separato: il codice diagnostico vive temporaneamente nel `BalanceZoneTracker` del Modello 1 senza contaminare la state machine trend-following.

## Build & Deploy

```bash
cd Modello-*/src
dotnet build -c Release
deploy.bat  # copia DLL in %APPDATA%\ATAS\Indicators\
```

## Log ATAS

Per `FabioTrendFollowing` c'è un solo log giornaliero:

- `%APPDATA%/ATAS/Logs/FabioTrendFollowing_YYYY-MM-DD.log` — contiene tutto: trigger, transizioni, `[PROFILE_PREVIEW]`, rejection candidate, new high/low, state e drawing/debug.

Quando analizzi cosa è successo sul mercato:
1. filtra prima il log per la fascia oraria richiesta;
2. per entry footprint cerca `[MR_AGGRESSION_CONFIRM]`;
3. per push/outcome cerca `[MR_MFE_UPDATE]`, `[MR_TARGET_HIT]`, `[MR_INVALIDATED]`;
4. per conferma di barra cerca `[MR_EARLY_TRIGGER]` e `[MR_TRIGGER]`;
5. per contesto risali a `[LOW/HIGH_REJECTION_CANDIDATE]`, `[NEW_SESSION_LOW/HIGH]`, `[PROFILE_PREVIEW]`;
6. considera la barra come contesto/candidate e il footprint come entry Fabio-style;
7. per `[MR_AGGRESSION_CONFIRM]` la soglia volume è hardcoded a `20`;
8. i log tecnici `[BAR_CHECK]`, `[STATE]`, `[DRAW_ZONE]`, `[VERIFY_COVERAGE]`, `[POC_CALC]`, `[VALUE_AREA_CALC]` sono disattivati di default.

## Documentazione

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
