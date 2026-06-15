# AGENTS.md — orderflow-atas

Progetto: indicatori/strategie order flow C# per ATAS (futures NQ/ES).

---

## Stato attuale (2026-06-15)

### Documentazione

| Modello | Spec completa | README |
|---------|---------------|--------|
| Modello 1 — Trend Following | — | `Trading LIVE.../Modello-1-TrendFollowing/README.md` |
| Modello 2 — Mean Reversion | `Trading LIVE.../Modello-2-MeanReversion/FabioMeanReversion.md` | `Trading LIVE.../Modello-2-MeanReversion/README.md` |

`FabioMeanReversion.md` contiene la **specifica completa** del Modello 2 (teoria, input, output, state machine, trigger, parametri target). Lo stato del codice vive solo qui in `AGENTS.md`.

### Implementazione codice

| Indicatore | Stato | Note |
|------------|-------|------|
| `FabioTrendFollowing.cs` | ✅ funzionante (legacy) | Aggression, LVN, Absorption, CVD — sessione NY |
| `FabioMeanReversion.cs` | ⚠️ legacy, da riscrivere | Wick/Squeeze/Absorption/CVD standalone — **non** allineato alla spec |

### Modello 2 — prossimo step

Riscrittura **da zero**, iniziando dalla **zona di balance**:

1. Profile engine (session profile via `IsNewSession(bar)` → barra corrente)
2. Calcolo POC / VAH / VAL
3. Rendering profile sul chart
4. Stato `BALANCE_READY` / `OUT_OF_BALANCE`

Poi in sequenza: breakout watch → fakeout → big trades → trigger → target POC.

### Cosa NON c'è ancora in `FabioMeanReversion.cs`

- Session profile engine
- State machine (`BALANCE_READY`, `FIRST_BREAKOUT_WAIT`, ecc.)
- Trigger profile-based
- Big trade engine (`OnCumulativeTrade`)
- Output box con STATO / DOVE / TARGET / INVALIDA

### Build

```cmd
cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)\Modello-1-TrendFollowing\src"
deploy.bat

cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)\Modello-2-MeanReversion\src"
deploy.bat
```