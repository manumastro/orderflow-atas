# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

## Modelli

### Modello 1 — Trend Following (IN REFACTORING)
**Path:** `Modello-1-TrendFollowing/`  
**Spec:** `Modello-1-TrendFollowing/FabioTrendFollowing.md`  
**Analisi:** `Modello-1-TrendFollowing/ANALISI.md` (problemi rilevati e piano di refactoring)  
**Codice:** `src/FabioTrendFollowing.cs`

Approccio trend following di Fabio per mercati OUT OF BALANCE:
- Entry su aggression clusters in low volume nodes
- Target: POC della balance zone precedente
- Sessione: New York only
- **Stato:** Prototipo con segnali isolati — manca framework centrale (out-of-balance validation, balance zone tracking, entry/stop/target).

### Modello 2 — Mean Reversion (ABBANDONATO)
**Path:** `Modello-2-MeanReversion/`  
**Spec:** `Modello-2-MeanReversion/FabioMeanReversion.md`  
**Codice:** `src/FabioMeanReversion.cs` (placeholder vuoto)

Approccio mean reversion basato su balance zones — **non implementato programmaticamente**.  
Mantenuto solo come riferimento per trading discrezionale (balance zones, breakout/fakeout, trapped traders, big orders).

## Build & Deploy

```bash
cd Modello-*/src
dotnet build -c Release
deploy.bat  # copia DLL in %APPDATA%\ATAS\Indicators\
```

## Documentazione

- **Transcript Fabio:** `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt`
- **API ATAS:** `docs/atas/api/`

## Focus Corrente

**Refactoring completo Modello 1** per implementare il framework centrale:
1. Out-of-balance detection (state machine BALANCE → OUT_OF_BALANCE)
2. Balance zone tracking (volume profile, VAH/VAL/POC)
3. Entry/Stop/Target calculation
4. Context-aware signals (aggression, low volume nodes, absorption)

Vedere `Modello-1-TrendFollowing/ANALISI.md` per dettagli.
