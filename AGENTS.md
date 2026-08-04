# orderflow-atas - Guida Per L'Agente

Questo repository serve prima a capire il corso in `fabio_course/` e poi, solo se i dati lo giustificano, a progettare un indicatore ATAS. Al momento non esiste un modello attivo: non assumere in anticipo ne' mean reversion ne' continuation.

## Prima Di Lavorare

1. Leggi `FabioOrderFlow/FabioOrderFlow.md`, che contiene lo stato operativo corrente.
2. Quando devi formulare un'ipotesi o toccare il runtime, leggi per intero `fabio_course/fabio1.txt`, `fabio_course/fabio2.txt` e `fabio_course/fabio3.txt`. La mappa del corso aiuta a orientarsi, ma non sostituisce le lezioni.
3. Considera il corso come un insieme: contesto, asta, valore, profilo, volume, partecipanti, timing, esecuzione e gestione vanno tenuti collegati.

## Come Ragionare

- Distingui sempre il fatto osservato, la sua possibile lettura, l'ipotesi testabile e la regola implementabile.
- Prima di ogni test scrivi domanda, dati necessari, selezione del campione, criterio di promozione e criterio di scarto. Non scegliere soglie dopo aver visto il risultato.
- Un singolo giorno puo' suggerire una domanda, non validare un modello. Tieni conto della sovrapposizione tra eventi e della dipendenza tra osservazioni.
- Mantieni il runtime neutro finche' non esiste un contratto di modello approvato. Niente ordini reali, PnL o automazione operativa senza una richiesta e una validazione separate.

## Documentare E Comunicare

Scrivi in modo comprensibile a una persona e a un agente: spiega un termine tecnico alla prima occorrenza, usa frasi brevi e non lasciare decisioni importanti solo nella conversazione. Per ogni fase sostanziale aggiorna il documento canonico pertinente e aggiungi una sola riga datata a `FabioOrderFlow/progress.txt`.

Conserva una fonte canonica per ogni decisione e non accumulare output intermedi inutili. Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.

## Build E Deploy

```bash
cd FabioOrderFlow/src
dotnet build -c Release
```

La DLL viene prodotta in `FabioOrderFlow/src/bin/Release/net10.0-windows/FabioOrderFlow.dll` e, con il deploy, copiata in `%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll`.
