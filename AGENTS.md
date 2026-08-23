# orderflow-atas - Guida Per L'Agente

Questo repository serve prima a capire il corso in `fabio_course/` e poi, solo se i dati lo giustificano, a progettare un indicatore ATAS. Al momento non esiste un modello attivo.

## Documentare E Comunicare

Scrivi in modo comprensibile a una persona e a un agente: spiega un termine tecnico alla prima occorrenza, usa frasi brevi e non lasciare decisioni importanti solo nella conversazione. Per ogni fase sostanziale aggiorna il documento canonico pertinente e aggiungi una sola riga datata a `FabioOrderFlow/progress.txt`.

Conserva una fonte canonica per ogni decisione e non accumulare output intermedi inutili. Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.

In `docs/atas/api/` trovi la documentazione dell'API di ATAS.

## Build E Deploy

```bash
cd FabioOrderFlow/src
dotnet build -c Release
```

La DLL viene prodotta in `FabioOrderFlow/src/bin/Release/net10.0-windows/FabioOrderFlow.dll` e, con il deploy, copiata in `%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll`.
