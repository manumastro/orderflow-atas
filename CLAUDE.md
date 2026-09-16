# orderflow-atas — Istruzioni Per L'Agente

Questo file **non contiene contesto**. Contiene gli obblighi di lettura e le regole operative. Il
contesto sta nei documenti linkati, ed e' li' che va cercato: duplicarlo qui lo farebbe divergere.

## Obbligo Di Lettura

Prima di lavorare su questo repository, **leggi nell'ordine**:

1. [`da-dove-si-comincia.md`](da-dove-si-comincia.md) — cos'e' il progetto e la mappa del repository.
2. [`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) — **il percorso completo, dall'inizio**: il
   corso, le due strade chiuse e perche', il contesto istituzionale COT, il Data Bridge, e cosa si
   fa oggi. E' il documento che ricostruisce come si e' arrivati all'analisi attuale.
3. [`docs/research/metodo/indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) — **come si fa** una analisi,
   procedura per procedura, nell'ordine in cui si usano. Ogni voce rimanda alle analisi passate che
   la applicano.
4. [`docs/research/metodo/glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md) — **dove sta scritta** ogni
   definizione del metodo. Non definisce niente: dice a quali righe della fonte tornare, ed e' lo
   strumento con cui si rispetta l'obbligo della sezione seguente.

Nessuna di queste quattro letture e' facoltativa. Se una risposta richiede contesto che non hai, il
posto dove trovarlo e' li', non nella conversazione.

## Prima Di Analizzare Il Mercato

Oltre alle quattro letture sopra, **prima di chiedere dati al bridge**:

1. [`docs/research/giornate/`](docs/research/giornate/) — il file di **oggi**, se esiste.
   L'intestazione dice se e' `APERTO`; la sezione finale **"Dove eravamo"** da' lo stato
   all'ultimo aggiornamento; la sezione **"Le correzioni"** dice cosa e' gia' stato smentito oggi.
2. Il file del **giorno precedente**, per il framing: value area, POC e minimi di ieri stanno nella
   sua sezione 1.

Poi si chiedono al bridge solo i dati **successivi all'ultimo aggiornamento**, non tutta la seduta.

Il file del giorno si aggiorna **durante** la seduta, non a posteriori: una giornata scritta alla
fine perde proprio le letture sbagliate, che sono la parte verificabile. Formato e regole in
[`docs/research/giornate/come-si-scrive-una-giornata.md`](docs/research/giornate/come-si-scrive-una-giornata.md).

## Prima Di Usare Un Termine Del Metodo

**Un termine del metodo non si parafrasa a memoria. Si apre la fonte e si legge il passo.**

Vale per: IVB, Tier 01/02/03, mean reverting, Triple AAA, Triple A+, deep effort, 40 range,
candle framing, block-and-reload, risk envelope, e ogni altra parola che viene dal corso.
[`docs/research/metodo/glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md) dice
in quali righe di [`triple-aaa-dossier.md`](docs/research/metodo/triple-aaa-dossier.md) sta ciascuna
definizione, cosi' la verifica costa un `sed -n`.

L'obbligo scatta **ogni volta**, non solo a inizio sessione, e in particolare quando la risposta
sembra ovvia: e' li' che si salta il controllo. Il 16 settembre ho definito "mean reverting a
rischio stretto" un setup che non era mean reverting, con un vincolo di rischio che non era quello
scritto. Entrambi gli errori stavano in un passo di sessanta righe che non avevo aperto.

**Una affermazione con conseguenza operativa deve essere tracciabile a una riga della fonte.**
Il `verso` di uno scenario, un permesso, una regola di rischio, un bersaglio: se non si riesce a
dire da quale riga del dossier discende, il verso corretto e' `NESSUN PERMESSO` e la frase va
riscritta come osservazione.

**Gerarchia delle fonti**, quando divergono: le immagini in
[`fabio_course/ivbaaa/`](fabio_course/ivbaaa/) battono il dossier, il dossier batte i documenti di
metodo, i documenti di metodo battono la conversazione. La memoria non e' una fonte.

## Come Si Scrive Una Lettura Dal Vivo

Chi legge **ha gli occhi sul grafico**, a mercato aperto, e deve capire in dieci secondi. La
lettura serve a dire quello che **non si vede guardando** — non a descrivere quello che si vede.

**Tre parti, in quest'ordine, e nient'altro:**

1. **Lo stato**, una frase. Cosa sta facendo il prezzo, e se e' deciso o no.
2. **La misura**, due o tre numeri. Solo quelli che sostengono la frase sopra.
3. **Il discriminante**, una riga per ramo. Quale livello risolve, e cosa significa ciascun esito.

**Regole:**

- **Un numero entra solo se cambia la conclusione.** Se togliendolo la frase resta vera, era
  decoro.
- **Niente tabella sotto le quattro righe.** Due confronti sono due righe di prosa.
- **Non si rielenca cio' che e' gia' sul chart.** I livelli sono disegnati: si nominano, non si
  ridescrivono.
- **Un termine tecnico si spiega alla prima occorrenza della seduta**, non tutte le volte.
- **Se non e' cambiato niente, si scrive quella riga sola.** Una lettura che ripete la precedente
  con parole diverse fa perdere il segnale nelle prossime.
- **Prima la conclusione, poi il perche'.** Mai il contrario: chi guarda il grafico si ferma dopo
  la prima riga se quella riga gli basta.

**L'eccezione, e non e' negoziabile:** il file della giornata in
[`docs/research/giornate/`](docs/research/giornate/) e i documenti di metodo **restano completi**.
La brevita' vale per la lettura dal vivo, che si legge con un occhio solo; il diario si legge a
mercato chiuso e deve contenere tutto, comprese le misure che si riveleranno sbagliate — sono la
parte verificabile. Accorciare il diario per lo stesso motivo per cui si accorcia una lettura
significa perdere l'unica cosa che il progetto produce.

## Regole Di Lavoro

Le cinque regole di metodo stanno in [`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) e valgono
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
- **Ogni file di documentazione ha un nome descrittivo.** Niente `README.md`: il nome deve dire
  cosa c'e' dentro, cosi' che si riconosca dal percorso e nei risultati di ricerca senza aprirlo.
  `percorso-del-progetto.md`, non `README.md`; `come-si-scrive-una-giornata.md`, non `README.md`.
  Il titolo H1 dentro il file deve corrispondere al nome. La regola non fa eccezione per la radice:
  il repository non ha un `README.md`, l'ingresso e' `da-dove-si-comincia.md`. L'unica cartella
  esclusa e' `docs/atas/`, documentazione ATAS generata che va lasciata com'e'.
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

Durante una seduta gli scenari attesi si scrivono **prima**, in
`docs/research/giornate/scenari-AAAA-MM-GG.json`, guardando il contesto di quel giorno:
`FabioOrderFlow/tools/scenari.py` e' solo il motore che li valuta, e quando uno scatta annota da
solo sul chart. **Le condizioni non vanno messe nel programma**: sono un file che si riscrive ogni
volta, perche' dipendono dai livelli e dalla struttura di quella giornata.
`FabioOrderFlow/tools/sveglia_tape.py` copre in parallelo cio' che non era previsto, senza
concludere niente, e `FabioOrderFlow/tools/annota.py` porta sul chart e nel diario la lettura
ragionata.
Procedura in
[`docs/research/metodo/sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md).

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
