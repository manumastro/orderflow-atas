# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

## Modelli

### Modello 1 — Trend Following (IN REFACTORING)
**Path:** `Modello-1-TrendFollowing/`  
**📘 Documento Centrale:** `Modello-1-TrendFollowing/README.md` ⭐  
**Codice:** `src/FabioTrendFollowing.cs`

Approccio trend following di Fabio per mercati OUT OF BALANCE:
- Entry su aggression clusters in low volume nodes
- Target: POC della balance zone precedente
- Sessione: New York only
- **Stato:** Prototipo con segnali isolati — richiede rewrite architetturale completo

**Leggi README.md per:**
- Framework completo dal transcript (balance zones, out-of-balance validation)
- Analisi dei 7 problemi architetturali critici
- Piano di implementazione in 3 fasi
- Architettura target (state machine, moduli, dataseries)

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

**Refactoring completo Modello 1** per implementare il framework centrale.

**📖 Tutto è documentato in:** `Modello-1-TrendFollowing/README.md`

Piano di implementazione in 3 fasi:
1. **Core Framework** (3-4 giorni): Balance zone tracker, impulse profiler, trade manager
2. **Signals Refactor** (2-3 giorni): Aggression con context, absorption pattern, CVD as confirm
3. **Polish & Testing** (1-2 giorni): Visual, parametri, backtest
