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

Per `FabioTrendFollowing` i log sono divisi:

- `%APPDATA%/ATAS/Logs/FabioTrendFollowing_YYYY-MM-DD.log` — eventi sintetici e decisionali (`[MR_EARLY_TRIGGER]`, `[MR_TRIGGER]`, `[ZONE_READY]`, `[OUT_OF_BALANCE]`).
- `%APPDATA%/ATAS/Logs/FabioTrendFollowing_verbose_YYYY-MM-DD.log` — diagnostica rumorosa/intrabar (`[PROFILE_PREVIEW]`, rejection candidate, new high/low, state, drawing/debug).

Quando analizzi cosa è successo sul mercato, leggi prima il main log e usa il verbose solo per ricostruire il contesto del trigger.

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
