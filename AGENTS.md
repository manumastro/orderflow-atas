# orderflow-atas - Agent Guide
1. Attivo: `FabioOrderFlow` / `LondonMeanReversionModel`.
2. Prima di agire leggi: `FabioOrderFlow/FabioOrderFlow.md`, `FabioOrderFlow/CHANGELOG-AGENT.md`, modello `.md`.
3. Codice strategia: `FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.cs`.
4. Log: usa `docs/atas/log-reading.md`; PnL solo da `[MR_EXIT]`.
5. Storico ATAS: ultimi 7 giorni; controlla `CUM_TRADES_LOOKBACK`.
6. Build: `cd FabioOrderFlow/src && dotnet build -c Release`.
7. Deploy DLL in `%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll`.
8. Non toccare PostLondonImpulse salvo richiesta.
9. Docs: scrivi per umano+agente; progetto=procedure, modello=strategia+contratto codice, changelog=decisioni/reload.
