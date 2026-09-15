# orderflow-atas — Istruzioni Per L'Agente

Questo file **non contiene contesto**. Contiene gli obblighi di lettura e le regole operative. Il
contesto sta nei documenti linkati, ed e' li' che va cercato: duplicarlo qui lo farebbe divergere.

## Obbligo Di Lettura

Prima di lavorare su questo repository, **leggi nell'ordine**:

1. [`README.md`](README.md) — cos'e' il progetto.
2. [`docs/research/README.md`](docs/research/README.md) — **il percorso completo, dall'inizio**: il
   corso, le due strade chiuse e perche', il contesto istituzionale COT, il Data Bridge, e cosa si
   fa oggi. E' il documento che ricostruisce come si e' arrivati all'analisi attuale.
3. [`docs/research/metodo/README.md`](docs/research/metodo/README.md) — **come si fa** una analisi,
   procedura per procedura, nell'ordine in cui si usano. Ogni voce rimanda alle analisi passate che
   la applicano.

Nessuna di queste tre letture e' facoltativa. Se una risposta richiede contesto che non hai, il
posto dove trovarlo e' li', non nella conversazione.

## Prima Di Analizzare Il Mercato

Oltre alle tre letture sopra, **prima di chiedere dati al bridge**:

1. [`docs/research/giornate/`](docs/research/giornate/) — il file di **oggi**, se esiste.
   L'intestazione dice se e' `APERTO`; la sezione finale **"Dove eravamo"** da' lo stato
   all'ultimo aggiornamento; la sezione **"Le correzioni"** dice cosa e' gia' stato smentito oggi.
2. Il file del **giorno precedente**, per il framing: value area, POC e minimi di ieri stanno nella
   sua sezione 1.

Poi si chiedono al bridge solo i dati **successivi all'ultimo aggiornamento**, non tutta la seduta.

Il file del giorno si aggiorna **durante** la seduta, non a posteriori: una giornata scritta alla
fine perde proprio le letture sbagliate, che sono la parte verificabile. Formato e regole in
[`docs/research/giornate/README.md`](docs/research/giornate/README.md).

## Regole Di Lavoro

Le cinque regole di metodo stanno in [`docs/research/README.md`](docs/research/README.md) e valgono
sempre. Qui solo quelle che riguardano il modo di lavorare:

- **Non esiste un modello attivo** e nessuna regola operativa e' approvata. Descrivere non e'
  validare.
- **La fase sistematica sul modello 40R e' chiusa.** Non riaprirla — replay, sweep di parametri,
  ottimizzazione — senza una richiesta esplicita.
- **Scrivi in modo comprensibile a una persona e a un agente**: spiega un termine tecnico alla prima
  occorrenza, frasi brevi, e nessuna decisione importante lasciata solo nella conversazione.
- **Per ogni fase sostanziale** aggiorna il documento canonico pertinente e aggiungi **una riga
  datata** a [`FabioOrderFlow/progress.txt`](FabioOrderFlow/progress.txt).
- **Non accumulare output intermedi**: i file grezzi vanno nella scratchpad di sessione, nel
  repository entra il risultato.
- **Registra i risultati negativi** con la stessa cura di quelli positivi, e dichiara sempre
  soglie, convenzioni e orizzonti accanto al numero che producono.

## Dati

Il percorso e' il **Data Bridge**. Contratto ed endpoint in
[`docs/research/metodo/contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md),
client `FabioOrderFlow/tools/bridge.py`.

Due vincoli tecnici che hanno gia' prodotto errori, e che i documenti linkati spiegano per esteso:

- **`bar` e' un indice di posizione, non un identificatore**: riparte quando ATAS ricarica
  l'indicatore. Per riconoscere una barra si usa `time`.
- **Il contratto continuo di ATAS non e' back-adjusted**: usa i contratti singoli o
  `FabioOrderFlow/tools/build_continuous.py`. Il controllo del rollover precede ogni altra misura.

I livelli disegnati sul chart arrivano da `POST /levels`: l'indicatore disegna e basta, `bridge.py`
trasporta e basta, la derivazione resta nell'analisi. Procedura in
[`docs/research/metodo/livelli-sul-chart.md`](docs/research/metodo/livelli-sul-chart.md). Non
spostare quel calcolo dentro l'indicatore.

I recorder di agosto 2026 sono archiviati in
[`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/) e non vanno estesi senza motivo.

## Build E Deploy

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Target `net10.0` senza WPF, requisito di ATAS X. Gli assembly ATAS sono risolti dal bundle
dell'applicazione su macOS e da `Program Files` su Windows. Dopo il deploy ATAS va riavviato.

Quando l'API ATAS non e' chiara, **ispeziona gli assembly con reflection** invece di dedurla dalla
documentazione: `docs/atas/` non sempre coincide con la build ATAS X installata.

Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.
