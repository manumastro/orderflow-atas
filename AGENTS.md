# AGENTS.md — orderflow-atas
Progetto: indicatori/strategie order flow C# per ATAS (futures NQ/ES).
1. Prima di scrivere codice leggi `docs/atas/programming/README.md` e `ricerca-order-flow-completa.md`.
2. Stack: C# (.NET 8) → class library DLL in `Indicators/` o `Strategies/`.
3. API: `Indicator.OnCalculate(bar, value)` · `GetCandle(bar)` → footprint via `GetPriceVolumeInfo()` / `GetAllPriceLevels()`.
4. Bar index valido: `0` → `CurrentBar - 1`. Mai chiamare fuori range.
5. Order flow: `IndicatorCandle` (Delta, POC) + `PriceVolumeInfo` (Ask, Bid) + `CumulativeTrade` (big trades).
6. Esempi: [github.com/AtasPlatform/Indicators](https://github.com/AtasPlatform/Indicators) · Docs full: `docs/atas/`.
7. Refresh docs: `node scripts/scrape-atas-docs.mjs`.
