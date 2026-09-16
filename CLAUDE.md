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

## Prima Di Aprire Un Asset Nuovo

**Nessuna lettura, nessun livello sul chart e nessuno scenario armato su uno strumento che non ha
completato i nove passi di
[`docs/research/metodo/come-si-apre-un-asset.md`](docs/research/metodo/come-si-apre-un-asset.md).**
Rollover, anagrafica del tick, i due orologi, il contesto istituzionale COT, il framing delle
sedute precedenti, i livelli, le soglie riscalate, gli scenari e i file col prefisso dello
strumento. L'obbligo vale **per ogni asset** e i primi otto passi si rifanno **a ogni rollover**.

Il metodo di questo repository e' nato sul Nasdaq, e quasi tutto il resto presuppone NQ senza
dirlo: l'ora della sessione regolamentata, le soglie in punti, la finestra dell'IVB, il codice
COT. Aprire un secondo strumento significa **scoprire quali costanti erano in realta' parametri**,
e un asset aperto a meta' produce numeri che sembrano una analisi e non lo sono.

Primo caso applicato, con gli errori che ha prodotto:
[`docs/research/giornate/MCLV6-2026-09-16.md`](docs/research/giornate/MCLV6-2026-09-16.md).

## Prima Di Analizzare Il Mercato

Oltre alle quattro letture sopra, **prima di chiedere dati al bridge**:

1. [`docs/research/giornate/`](docs/research/giornate/) — il file di **oggi per quello
   strumento**, se esiste (con il prefisso, dove c'e': `MCLV6-AAAA-MM-GG.md`).
   L'intestazione dice se e' `APERTO`; la sezione finale **"Dove eravamo"** da' lo stato
   all'ultimo aggiornamento; la sezione **"Le correzioni"** dice cosa e' gia' stato smentito oggi.
2. Il file del **giorno precedente**, per il framing: value area, POC e minimi di ieri stanno nella
   sua sezione 1.

Poi si chiedono al bridge solo i dati **successivi all'ultimo aggiornamento**, non tutta la seduta.

Il file del giorno si aggiorna **durante** la seduta, non a posteriori: una giornata scritta alla
fine perde proprio le letture sbagliate, che sono la parte verificabile. Formato e regole in
[`docs/research/giornate/come-si-scrive-una-giornata.md`](docs/research/giornate/come-si-scrive-una-giornata.md).

## Il Giro D'Orizzonte, Prima Di Ogni Risposta

**Prima di qualunque analisi, anche la piu' piccola, si guarda tutto il contesto disponibile.**
Non le ultime dieci barre, non solo il livello di cui si sta parlando: **tutto** — lo stato del
bridge, dove eravamo, le correzioni gia' fatte oggi, il framing di ieri, cosa e' gia' scattato, gli
scenari armati, il tape con le finestre mobili, il profilo della seduta.

L'obbligo vale **per ogni risposta**, non a inizio sessione, e **non dipende dalla fase di
mercato**: in globex, a mercato quasi fermo, a cash chiusa e nel weekend si riconsidera tutto
esattamente come durante la cash. Una fase tranquilla non e' un contesto piu' piccolo, e' lo stesso
contesto con meno barre nuove — ed e' quando si e' piu' tentati di rispondere a memoria. Una
domanda breve non autorizza un contesto breve: le domande piu' corte — *"ha senso che scenda?"*, *"cosa ne pensi?"* — sono quelle
a cui si risponde piu' facilmente guardando lo schermo invece dei dati, ed e' li' che si sbaglia.

**Non costa niente: arriva da solo.** Un hook `UserPromptSubmit` in
[`.claude/settings.json`](.claude/settings.json) esegue
[`.claude/hooks/giro-orizzonte.sh`](.claude/hooks/giro-orizzonte.sh) a ogni messaggio e ne mette lo
stdout nel contesto del prompt. Mezzo secondo, otto sezioni, prima che la risposta cominci.

```bash
python3 FabioOrderFlow/tools/giro_orizzonte.py     # a mano, se serve altro o l'hook e' spento
```

**Il giro d'orizzonte automatico si legge, non si ignora.** E' in cima al prompt proprio perche' la
risposta si costruisca su quello. Se il blocco manca — hook disattivato, sessione diversa — si
lancia il comando a mano. Se dice che il bridge non risponde, **lo si dichiara nella risposta**:
una lettura senza dati va detta tale, non presentata come una lettura.

**Tre cose che il giro d'orizzonte impedisce**, tutte gia' successe:

- **dedurre invece di leggere.** Il 16 settembre ho detto che uno scenario non era scattato: era
  scattato otto minuti prima, e stava scritto nel diario. La sezione 5 lo mette davanti agli occhi.
- **rispondere sulle ultime barre.** Una lettura costruita su dieci minuti ignora che il POC della
  seduta sta 100 punti sotto e che il delta a 60 minuti dice il contrario di quello a 15.
- **ripetere un errore gia' corretto oggi.** La sezione 3 elenca le correzioni della giornata: se
  una risposta sta per ricadere in una di quelle, si vede prima di scriverla.

**Questo non contraddice la brevita' della lettura dal vivo.** Il giro d'orizzonte riguarda
l'**input**: si guarda tutto. La lettura riguarda l'**output**: si scrive poco, e si sceglie quel
poco proprio perche' si e' visto tutto. Una risposta corta costruita su un contesto corto e'
un'altra cosa, ed e' il modo in cui si sbaglia in fretta.


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

## Un Livello Non E' Mai Il Fine, E' Sempre Una Porta

**Ogni volta che si nomina un prezzo, una zona o un livello, si dice a cosa serve arrivarci.**
Vale ovunque: letture dal vivo, file della giornata, campi `attesa` e `implica` degli scenari,
etichette sul chart, risposte a una domanda. Senza il "per cosa", il livello e' un numero e chi
legge deve indovinare perche' glielo si sta dicendo.

Un livello puo' servire a **quattro cose**, e va detto quale:

| | cosa cambia arrivandoci |
|---|---|
| **permesso** | cambia cosa e' lecito fare: il gate Tier 01, i bordi del valore per il mean reverting |
| **bersaglio** | apre il tratto successivo — e allora si dice **quale** e **quanti punti** |
| **invalidazione** | smonta una lettura in corso: uno scenario decade, un rimbalzo smette di esserlo |
| **posizionamento** | dice chi resta intrappolato o liberato, e quindi chi dovra' agire |

**Sbagliato:** *"serve riprendere 29.372,50"* — riprenderlo per cosa?

**Giusto:** *"serve riprendere 29.372,50, dove ieri si e' aperto lo short: sopra, il primo
bersaglio e' la mensola a 29.453,50, ottanta punti, e chi ha comprato stamattina smette di essere
sott'acqua."*

La stessa frase dice il livello, la sua storia, **cosa apre**, **quanto dista** e **chi libera**.
Costa una riga in piu' e toglie una domanda.

**Corollario:** se non si sa dire a cosa serve un livello, quel livello non va nominato — e
probabilmente non andava nemmeno disegnato sul chart.

## Prima Di Armare Una Condizione

**Una condizione non descrive un setup: lo separa dal suo sosia.** Ogni setup ne ha uno — un
movimento che produce gli stessi numeri e significa il contrario. Un rifiuto del bordo alto del
valore e una rottura dello stesso bordo dall'alto hanno massimo sopra, chiusura sotto, corpo in
basso, volume e delta negativo: identici. Li separa **solo** da che parte arriva il prezzo.

Le sei prove obbligatorie — lato di arrivo, sosia dichiarato, verso tracciabile a una riga della
fonte, `--controlla`, `--prova` sulla storia, tempi del setup dichiarati — stanno in
[`docs/research/metodo/sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md),
sezione *"Le Prove Che Una Condizione Deve Passare Prima Di Essere Armata"*, con il caso del
16 settembre che le ha prodotte. **Nessuna e' facoltativa, e valgono a ogni riscrittura degli
scenari, non solo alla prima.**

Le difese automatiche (`--controlla`, scenario rotto che grida) verificano che una condizione sia
**eseguibile**. Non possono accorgersi che sia **sbagliata**: quello lo fanno solo le sei prove,
e le fa l'analisi, a mano, prima di armare.


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

**I due sorveglianti si accendono come `Monitor`, non come comando in background**: un comando in
background scrive su un file e non sveglia l'agente. Procedura completa — filtri, riarmo alla
scadenza, come si sospende e si riprende una pausa, e perche' il log entra nel giro d'orizzonte —
in [`sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md), sezioni *"Come Si
Tengono Accesi"*, *"Sospendere E Riprendere La Sorveglianza"* e *"La Notifica Non E' L'Unica Rete"*.

**Riavviando la sveglia a seduta in corso, `--from` non basta**: dice da quando sorvegliare, non su
quante barre calcolare le soglie di "fuori scala". Serve `--storia` (default 480 minuti), o lo
strumento riparte cieco. Sezione *"`--from` Dice Da Quando, `--storia` Dice Su Cosa Si Misura"*.
Procedura in
[`docs/research/metodo/sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md).

**Il delta esiste anche prezzo per prezzo.** `bridge.py candles --levels` restituisce la footprint
di ogni barra — `ask` meno `bid` a ogni prezzo scambiato, piu' `maxPositiveDelta` e il POC di
quella barra. Prima di dichiarare che un dato non c'e', si apre l'elenco degli endpoint in
[`contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md): il 16 settembre ho
detto che la footprint non era disponibile mentre era documentata. Procedura in
[`la-footprint-e-il-delta-per-prezzo.md`](docs/research/metodo/la-footprint-e-il-delta-per-prezzo.md).

**I livelli, i bordi del valore e la finestra su cui si misurano si rifanno a ogni lettura che li
usa** — in qualunque fase della giornata, non solo a sessione cash aperta. La procedura completa e'
**obbligatoria** e sta in
[`livelli-sul-chart.md`](docs/research/metodo/livelli-sul-chart.md), sezioni *"I Livelli Si Rifanno
Durante La Seduta"*, *"Il Ridisegno Non Si Chiede, Si Fa"* e *"La Finestra Di Misura Si Sceglie A
Ogni Lettura"*. Tre obblighi che quelle sezioni impongono e che non vanno dedotti: il ridisegno si
**esegue** invece di chiederlo, la finestra di misura si **dichiara** accanto al numero, e nessuno
dei due dipende dalla fase di sessione.

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
