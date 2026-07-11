# orderflow-atas - Agent Guide
1. Attivi: `FabioOrderFlow` / `FabioAuctionStudyModel` per discovery dual-session; `LondonMeanReversionModel` resta baseline compression/shadow.
2. Prima di agire leggi: `FabioOrderFlow/FabioOrderFlow.md`, `FabioOrderFlow/CHANGELOG-AGENT.md` e i `.md` dei modelli coinvolti.
3. Codice studio: `FabioOrderFlow/models/FabioAuctionStudyModel/FabioAuctionStudyModel.cs` e `FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.cs`.
4. Log: usa `docs/atas/log-reading.md`; PnL solo da `[MR_EXIT]` legacy, mai dai ledger.
5. Storico ATAS: richieste sequenziali da massimo 7 giorni ciascuna sull'intero chart; controlla `CUM_TRADES_LOOKBACK`.
6. Build: `cd FabioOrderFlow/src && dotnet build -c Release`.
7. Deploy DLL in `%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll`.
8. Non toccare PostLondonImpulse salvo richiesta.
9. Docs: scrivi per umano+agente; progetto=procedure, modello=strategia+contratto codice, changelog=decisioni/reload.
10. Lavora una cosa alla volta: mantieni sempre lo scopo scelto inizialmente, non mischiare local profile, retest, filtri o nuove entry nello stesso ciclo.
11. Ogni risposta deve essere chiarificatrice e concisa anche se il tema e' gia' stato discusso: riepiloga sempre cosa e' operativo, cosa e' solo diagnostico, cosa e' cambiato e cosa resta da verificare; non dare per scontato il contesto precedente.
