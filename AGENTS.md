# orderflow-atas - Guida Per L'Agente

Leggi prima [README.md](README.md): contiene il percorso del progetto e il metodo. Questo file aggiunge solo le regole operative.

Il repository serve a capire il corso in `fabio_course/` e a verificarne le affermazioni sui dati reali di ATAS. **Non esiste un modello attivo** e nessuna regola operativa e' approvata. Descrivere non e' validare.

## Documentare E Comunicare

Scrivi in modo comprensibile a una persona e a un agente: spiega un termine tecnico alla prima occorrenza, usa frasi brevi e non lasciare decisioni importanti solo nella conversazione.

Per ogni fase sostanziale aggiorna il documento canonico pertinente e aggiungi **una sola riga datata** a `FabioOrderFlow/progress.txt`. Conserva una fonte canonica per ogni decisione e non accumulare output intermedi: i file grezzi vanno nella scratchpad di sessione, nel repository entra il risultato.

Registra anche i risultati negativi, con la stessa cura di quelli positivi. Dichiara sempre soglie, convenzioni e orizzonti accanto al numero che producono.

## Dati

Il percorso attuale e' il **Data Bridge**: un indicatore caricato su un chart ATAS che espone i dati su `http://127.0.0.1:8787`. Client: `FabioOrderFlow/tools/bridge.py`. Contratto: `docs/research/metodo/contratto-data-bridge.md`.

I recorder di agosto 2026 restano per il flusso live e per l'overlay sul chart; il loro materiale e' in `docs/research/archivio-2026-08/` e non va esteso senza motivo.

## Build E Deploy

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Target `net10.0` senza WPF, requisito di ATAS X. Gli assembly ATAS sono risolti dal bundle dell'applicazione su macOS e da `Program Files` su Windows. Dopo il deploy ATAS va riavviato.

Quando l'API ATAS non e' chiara, ispeziona gli assembly con reflection invece di dedurla dalla documentazione: `docs/atas/` non sempre coincide con la build ATAS X installata.

Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.
