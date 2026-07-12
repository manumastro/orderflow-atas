# orderflow-atas - Agent Guide
1. Runtime attivo: `FabioOrderFlow` / `FabioAuctionStudyModel` esclusivamente per New York impulse A->B; `LondonMeanReversionModel` e `BalanceZoneTracker` restano baseline compilate ma non inizializzate.
2. Prima di agire leggi: `FabioOrderFlow/FabioOrderFlow.md`, `FabioOrderFlow/CHANGELOG-AGENT.md` e i `.md` dei modelli coinvolti.
3. Codice studio: `FabioOrderFlow/models/FabioAuctionStudyModel/FabioAuctionStudyModel.cs` e `FabioOrderFlow/models/LondonMeanReversionModel/LondonMeanReversionModel.cs`.
4. Log: usa `docs/atas/log-reading.md`; PnL solo da `[MR_EXIT]` legacy, mai dai ledger.
5. Storico ATAS: richieste sequenziali da massimo 7 giorni ciascuna sull'intero chart; controlla `CUM_TRADES_LOOKBACK`.
6. Build: `cd FabioOrderFlow/src && dotnet build -c Release`.
7. Deploy DLL in `%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll`.
8. Non toccare PostLondonImpulse salvo richiesta.
9. Docs: scrivi per umano+agente; progetto=procedure, modello=strategia+contratto codice, changelog=decisioni/reload.
10. Lavora una cosa alla volta: mantieni sempre lo scopo scelto inizialmente, non mischiare local profile, retest, filtri o nuove entry nello stesso ciclo.
11. Ogni risposta deve essere chiarificatrice, umanamente comprensibile e concisa anche se il tema e' gia' stato discusso: riepiloga sempre cosa e' operativo, cosa e' solo diagnostico, cosa e' cambiato e cosa resta da verificare; non dare per scontato il contesto precedente.
12. Mantieni sempre la prospettiva sul fine: arrivare a un modello causale, validato e infine eseguibile. Una diagnostica e' utile solo se elimina un'ipotesi, promuove un candidato o definisce un test decisivo.
13. Prima di ogni ciclo dichiara domanda, candidato, criterio di promozione, criterio di scarto e dati necessari. Non scegliere questi criteri dopo aver letto gli outcome dello stesso campione.
14. Chiudi ogni ciclo con una sola conclusione esplicita: `PROMUOVI`, `SCARTA` oppure `INCONCLUDENTE`. Se inconcludente, specifica il test successivo, la numerosita' o scadenza, e la condizione che impedisce di continuare indefinitamente.
15. Ogni riepilogo finale deve contenere: `Fine perseguito`, `Conclusione`, `Impatto sul modello`, `Prossima azione`, `Criterio di arresto`. Non usare formule vaghe come "servono altri dati" senza un contratto di verifica.
16. Usa un linguaggio da conversazione tra persone. Spiega ogni termine tecnico o sigla alla prima occorrenza con parole comuni e chiarisci cosa significa concretamente per il modello; non presentare sequenze di marker, metriche o etichette senza tradurle in una conclusione comprensibile.
