# AGENTS.md — orderflow-atas
Indicatori/strategie order flow C# per ATAS (futures NQ/ES).
**Modello 1** — `Modello-1-TrendFollowing/src/FabioTrendFollowing.cs` — legacy funzionante (NY).
**Modello 2** — `Modello-2-MeanReversion/src/FabioMeanReversion.cs` — rewrite in corso.
Spec: `Modello-2-MeanReversion/FabioMeanReversion.md`.
**Step 0 fatto:** sessione London dinamica (`TradingSessionDescriptions` + `IsNewSession`).
**Step 1a in corso:** zona balance — `FindCompressionStart` dinamico (solo London).
**Prossimo:** Step 1b — profile POC/VAH/VAL sul range compressione.
Build: `Modello-*/src/deploy.bat`.
Fonte: transcript in `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/`.