# orderflow-atas - Guida Per L'Agente

Leggi prima [README.md](README.md): contiene il percorso del progetto e il metodo. Questo file aggiunge solo le regole operative.

Il repository serve a capire il corso in `fabio_course/` e a verificarne le affermazioni sui dati reali di ATAS. **Non esiste un modello attivo** e nessuna regola operativa e' approvata. Descrivere non e' validare.

## Cosa C'e' Nel README, In Una Riga Ciascuno

Sezioni del [README](README.md), per sapere subito dove guardare:

| Sezione | In una riga |
|---|---|
| 1. Le trascrizioni | `fabio_course/` e la mappa del corso; **il corso principale e' il live Q1** in `fabio_course/fabio_q1/`, gli altri live si aggiungeranno; il dossier del modello e' in `fabio_course/ivbaaa/`, trascritto in `metodo/triple-aaa-dossier.md` |
| 2. Dal concetto al dato osservabile | i concetti del corso tradotti in quantita' che ATAS espone davvero; fase dei recorder, archiviata |
| 3. Il contesto istituzionale | COT e Tradingster: due viste diverse, raccolta riproducibile, metrica cross-index verificata e negativa |
| 4. Il bridge | `Fabio Data Bridge` espone i dati ATAS su HTTP locale: si chiedono le finestre che servono invece di deciderle prima |
| 5. La prima analisi servita dal bridge | la settimana 04-11 settembre, con il primo caso misurato di sforzo senza risultato |
| 6. Il modello sistematico | misurato su un mese e **chiuso**: 49-51% a 1:1, come il modello nullo; archiviato in `docs/research/archivio-modello-40r/` |
| 7. Lo scope attuale | la lettura discrezionale del live Q1: `metodo/profile-framing.md`, `metodo/analisi-istituzionale.md`, `metodo/livelli-sul-chart.md`, `metodo/triple-aaa-dossier.md` |
| Struttura | la mappa delle cartelle di `docs/research/` |
| Come si lavora qui | le cinque regole: dichiarare prima, distinguere livello e flusso, registrare i negativi, dichiarare le convenzioni, una fonte canonica per decisione |
| Avvio rapido | `./deploy.sh` e `bridge.py health` |

## Lo Scope Attuale

La lettura **discrezionale** del primo live del corso (`fabio_course/fabio_q1/`), insieme al
contesto istituzionale. La fase sistematica sul modello 40R e' chiusa: non riaprirla senza una
richiesta esplicita. Le fonti canoniche sono `docs/research/metodo/profile-framing.md`,
`docs/research/metodo/analisi-istituzionale.md` e `docs/research/metodo/triple-aaa-dossier.md`.

Le immagini in `fabio_course/ivbaaa/` sono **fonte primaria**: il dossier del modello. Il testo e'
trascritto alla lettera in `metodo/triple-aaa-dossier.md`, che e' il documento da citare; le
immagini restano il riferimento per le figure. Trascrivere non e' validare: cosa e' stato
effettivamente misurato, e cosa no, e' dichiarato in fondo a quel documento e non va confuso con
l'archivio del modello 40R, che ha testato una meccanizzazione parziale di tre soli trigger.

## Documentare E Comunicare

Scrivi in modo comprensibile a una persona e a un agente: spiega un termine tecnico alla prima occorrenza, usa frasi brevi e non lasciare decisioni importanti solo nella conversazione.

Per ogni fase sostanziale aggiorna il documento canonico pertinente e aggiungi **una sola riga datata** a `FabioOrderFlow/progress.txt`. Conserva una fonte canonica per ogni decisione e non accumulare output intermedi: i file grezzi vanno nella scratchpad di sessione, nel repository entra il risultato.

Registra anche i risultati negativi, con la stessa cura di quelli positivi. Dichiara sempre soglie, convenzioni e orizzonti accanto al numero che producono.

## Dati

Il percorso attuale e' il **Data Bridge**: un indicatore caricato su un chart ATAS che espone i dati su `http://127.0.0.1:8787`. Client: `FabioOrderFlow/tools/bridge.py`. Contratto: `docs/research/metodo/contratto-data-bridge.md`.

I recorder di agosto 2026 restano per il flusso live e per l'overlay sul chart; il loro materiale e' in `docs/research/archivio-2026-08/` e non va esteso senza motivo.

I livelli disegnati sul chart arrivano da `POST /levels`: l'indicatore li disegna e basta, `bridge.py` li trasporta e basta, la derivazione resta nell'analisi. Procedura in `docs/research/metodo/livelli-sul-chart.md`. Non spostare quel calcolo dentro l'indicatore: le soglie devono restare convenzioni dichiarate in un documento.

**Il contratto continuo di ATAS non e' back-adjusted**: incolla i contratti lasciando il salto di prezzo del roll. Per il profile framing usa `FabioOrderFlow/tools/build_continuous.py`, che misura lo spread sui minuti in cui entrambi i contratti stampano e sposta anche il footprint. Il controllo del rollover, confrontando il volume dei due contratti, precede ogni altra misura.

## Build E Deploy

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Target `net10.0` senza WPF, requisito di ATAS X. Gli assembly ATAS sono risolti dal bundle dell'applicazione su macOS e da `Program Files` su Windows. Dopo il deploy ATAS va riavviato.

Quando l'API ATAS non e' chiara, ispeziona gli assembly con reflection invece di dedurla dalla documentazione: `docs/atas/` non sempre coincide con la build ATAS X installata.

Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.
