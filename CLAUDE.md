# orderflow-atas — Istruzioni Per L'Agente

Questo file **non contiene contesto**. Contiene gli obblighi di lettura e le regole operative. Il
contesto sta nei documenti linkati, ed e' li' che va cercato: duplicarlo qui lo farebbe divergere.

---

## Le Due Decisioni Che Governano Tutto (18 Settembre 2026)

**1. Il corso di Fabio comanda.** Ogni operazione, ogni livello, ogni condizione armata deve poter
essere ricondotta a un minuto di una lezione del live Q1. Il **modello IVB / Triple AAA** del
dossier resta citabile — e' di Fabio, e va menzionato — ma **non comanda**: descrive lo stesso setup
dentro un'impalcatura con un gate orario che nel live non esiste.

**2. Lo strumento e' NQ, e solo NQ.** *"Get good with one asset, one can pay your bills. One asset,
one. I choose NASDAQ"* (`fabio_q1/fabio_3.txt`, 1:06:37). Niente oro, niente crude, niente ES. I
file di quegli strumenti restano in `docs/research/giornate/` come evidenza e non si estendono.

**Conseguenza sulla sessione: si opera New York.** Nel live e' dichiarato due volte, col motivo
(`fabio_3.txt`, 22:12 e 23:00). La deroga "si opera anche a Londra" del 17 settembre e' anteriore a
questa decisione e non e' piu' attiva. La deroga "si opera prima del gate" decade da sola: **nel
live il gate non c'e'**.

### Gerarchia Delle Fonti

```text
1  fabio_course/fabio_q1/*.txt      le trascrizioni del live Q1   <- COMANDA
2  fabio_course/ivbaaa/*.webp       il dossier illustrato          <- riferimento
3  docs/research/metodo/*.md        i documenti di metodo          <- derivati, citano 1 e 2
4  la conversazione                                                 <- non e' una fonte
```

Fra 1 e 2 vince 1. Dentro 2, l'immagine batte la trascrizione del dossier. **La memoria non e' una
fonte.**

---

## Obbligo Di Lettura

Prima di lavorare su questo repository, **leggi nell'ordine**:

1. [`da-dove-si-comincia.md`](da-dove-si-comincia.md) — cos'e' il progetto e la mappa del repository.
2. [`docs/research/metodo/i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md)
   — **il metodo che comanda**: i tre modelli, il vocabolario, il regime, la sessione, il COT, il
   framing, i timeframe. Ogni riga cita il minuto della lezione da cui viene.
3. [`docs/research/metodo/i-pattern-di-esecuzione.md`](docs/research/metodo/i-pattern-di-esecuzione.md)
   — la libreria dei setup, e come si costruiscono ingresso, stop e bersaglio.
4. [`docs/research/metodo/la-gestione-della-posizione.md`](docs/research/metodo/la-gestione-della-posizione.md)
   — break even, parziali, conto in R, size, budget di rischio. **Nel live e' l'edge, non un
   corollario.**
5. [`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) — come si e'
   arrivati qui: le strade chiuse e perche', il COT, il Data Bridge.
5bis. [`docs/research/come-lavora-l-utente.md`](docs/research/come-lavora-l-utente.md) — come va
   condotto il lavoro, e **il caso concreto da cui nascono** alcune regole di questo file. Una
   regola di cui si e' perso il motivo si erode.
6. [`docs/research/metodo/indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md)
   — **come si fa** una analisi, procedura per procedura, nell'ordine in cui si usano.
7. [`docs/research/metodo/glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md) —
   **dove sta scritta** ogni definizione. Non definisce niente: dice a quale minuto o a quale riga
   tornare.

Nessuna di queste letture e' facoltativa. Se una risposta richiede contesto che non hai, il posto
dove trovarlo e' li', non nella conversazione.

## Prima Di Analizzare Il Mercato

Oltre alle letture sopra, **prima di chiedere dati al bridge**:

1. [`docs/research/giornate/`](docs/research/giornate/) — il file di **oggi**, se esiste.
   L'intestazione dice se e' `APERTO`; la sezione finale **"Dove eravamo"** da' lo stato all'ultimo
   aggiornamento; la sezione **"Le correzioni"** dice cosa e' gia' stato smentito oggi.
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
domanda breve non autorizza un contesto breve: le domande piu' corte — *"ha senso che scenda?"*,
*"cosa ne pensi?"* — sono quelle a cui si risponde piu' facilmente guardando lo schermo invece dei
dati, ed e' li' che si sbaglia.

**Non costa niente: arriva da solo.** Un hook `UserPromptSubmit` in
[`.claude/settings.json`](.claude/settings.json) esegue
[`.claude/hooks/giro-orizzonte.sh`](.claude/hooks/giro-orizzonte.sh) a ogni messaggio e ne mette lo
stdout nel contesto del prompt.

**Il giro d'orizzonte automatico si legge, non si ignora.** Se il blocco manca — hook disattivato,
sessione diversa — si lancia il comando a mano:

```bash
python3 FabioOrderFlow/tools/giro_orizzonte.py
```

Se dice che il bridge non risponde, **lo si dichiara nella risposta**: una lettura senza dati va
detta tale, non presentata come una lettura.

**Tre cose che il giro d'orizzonte impedisce**, tutte gia' successe:

- **dedurre invece di leggere.** Il 16 settembre ho detto che uno scenario non era scattato: era
  scattato otto minuti prima, e stava scritto nel diario. La sezione 5 lo mette davanti agli occhi.
- **rispondere sulle ultime barre.** Una lettura costruita su dieci minuti ignora che il POC della
  seduta sta 100 punti sotto e che il delta a 60 minuti dice il contrario di quello a 15.
- **ripetere un errore gia' corretto oggi.** La sezione 3 elenca le correzioni della giornata.

**Questo non contraddice la brevita' della lettura dal vivo.** Il giro riguarda l'**input**: si
guarda tutto. La lettura riguarda l'**output**: si scrive poco, e si sceglie quel poco proprio
perche' si e' visto tutto.

## Il Quadro: Framing E COT, Sempre, Anche Senza Che Lo Si Chieda

**Prima di qualunque lettura si fa il quadro**: dove si e' costruito il valore (profile framing) e
chi e' posizionato (COT). Le due meta' si leggono **insieme**, perche' e' la coppia a dire la cosa
che nessuna delle due dice da sola — **se il lato posizionato sta sopra o sotto il cuore del
volume**. Procedura in
[`docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md`](docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md);
le fonti restano [`profile-framing.md`](docs/research/metodo/profile-framing.md) e
[`analisi-istituzionale.md`](docs/research/metodo/analisi-istituzionale.md), e la procedura COT
esatta del live sta alla **sezione 9** di
[`i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md).

**Non e' su richiesta, ed e' questo il punto.** Il quadro si scrive nel file della giornata fra
`<!-- QUADRO -->` e `<!-- /QUADRO -->`, e `giro_orizzonte.py` lo stampa alla **sezione 4bis** a ogni
messaggio. Se il blocco manca, il giro lo dichiara: **una lettura data con la 4bis vuota e' una
lettura costruita sulle ultime barre**.

**Tre cose che il quadro contiene e che si dimenticano:**

- **la scala dei POC**, non "il bersaglio". Ogni area di valore riattraversata regala il proprio POC
  come magnete. **Un massimo di seduta non e' un POC.** E il live ne vuole due: quello della singola
  seduta e **quello della seduta che ha creato la rottura**.
- **il vuoto e da dove nasce**: un vuoto vero e' lo spazio fra il VAH di una seduta e il VAL di
  un'altra, non una fascia sottile del composito.
- **quale campo del COT e' l'estremo.** Nel live si guarda **solo non-commercial, vista Legacy**, e
  **solo l'ultima variazione** decide chi ha il piede sull'acceleratore. Poi si prende la **data** e
  si torna sul **prezzo** di quella data: e' li' che il COT diventa un livello.

**Il quadro non contiene direzioni, ingressi, stop o bersagli operativi.** Dice dove si sta, non
cosa fare.

## Prima Di Dichiarare Che Un Setup Esiste

**Si dichiara quale dei tre modelli sta girando, e lo decide la posizione del prezzo, non l'umore.**

```text
balance / mean reverting   il prezzo e' chiuso dentro la cash precedente  -> fade dei bordi verso il POC
momentum                   una balance viene rotta                        -> con speed of tape, o non si entra
trend following            giornata direzionale                           -> si fade il ritracciamento nel verso del giorno
```

Prendere un setup momentum dentro la balance, o un mean reverting fuori dal valore, e' lo stesso
errore con due nomi. Dettaglio e citazioni in
[`i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md).

**E si dichiara il regime**, perche' decide la size prima di decidere il setup: direzionale (una
candela copre piu' di cinque candele del range precedente), balance, oppure choppy. Su una giornata
choppy si riduce la size, si prendono profitti piccoli sui livelli, si accettano i break even e si
chiude al secondo stop.

## Prima Di Usare Un Termine Del Metodo

**Un termine del metodo non si parafrasa a memoria. Si apre la fonte e si legge il passo.**

Vale per: assorbimento, aggressione, risultato, reload, squeeze, failed auction, muro di liquidita',
speed of tape, big trades, flip dell'asta, gap, point of no return, station, premium/discount — e
per ogni termine che viene dal dossier: IVB, Tier 01/02/03, mean reverting, Triple AAA, Triple A+,
deep effort, 40 range, candle framing, block-and-reload, risk envelope.

[`glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md) dice, per ciascuno, **a
quale minuto di quale lezione** (o a quali righe del dossier) tornare, cosi' la verifica costa un
`sed -n` o un `grep`.

L'obbligo scatta **ogni volta**, non solo a inizio sessione, e in particolare quando la risposta
sembra ovvia: e' li' che si salta il controllo. Il 16 settembre ho definito "mean reverting a
rischio stretto" un setup che non era mean reverting, con un vincolo di rischio che non era quello
scritto.

**Una affermazione con conseguenza operativa deve essere tracciabile a un minuto del live.** Un
permesso, una regola di rischio, un bersaglio: se non si riesce a dire da quale minuto discende, la
frase va riscritta come osservazione.

## Un Livello Non E' Mai Il Fine, E' Sempre Una Porta

**Ogni volta che si nomina un prezzo, una zona o un livello, si dice a cosa serve arrivarci.**
Vale ovunque: letture dal vivo, file della giornata, campi `attesa` e `implica` degli scenari,
etichette sul chart, risposte a una domanda.

Un livello puo' servire a **quattro cose**, e va detto quale:

| | cosa cambia arrivandoci |
|---|---|
| **permesso** | cambia cosa e' lecito fare: i bordi del valore per il mean reverting, la rottura della balance per il momentum |
| **bersaglio** | apre il tratto successivo — e allora si dice **quale** e **quanti punti** |
| **invalidazione** | smonta una lettura in corso: un rimbalzo smette di esserlo |
| **posizionamento** | dice chi resta intrappolato o liberato, e quindi chi dovra' agire |

**Sbagliato:** *"serve riprendere 29.372,50"* — riprenderlo per cosa?

**Giusto:** *"serve riprendere 29.372,50, dove ieri si e' aperto lo short: sopra, il primo
bersaglio e' la mensola a 29.453,50, ottanta punti, e chi ha comprato stamattina smette di essere
sott'acqua."*

**Corollario:** se non si sa dire a cosa serve un livello, quel livello non va nominato — e
probabilmente non andava nemmeno disegnato sul chart.

**E si marcano solo due tipi di livello**, come nel live: la **massima aggressione** e il **massimo
assorbimento impilato**, entrambi da ordini eseguiti. *"It's not necessary to mark intermediate
level that are useless for us."*

## Prima Di Armare Una Condizione

**Una condizione non descrive un setup: lo separa dal suo sosia.** Ogni setup ne ha uno — un
movimento che produce gli stessi numeri e significa il contrario. Un rifiuto del bordo alto del
valore e una rottura dello stesso bordo dall'alto hanno massimo sopra, chiusura sotto, corpo in
basso, volume e delta negativo: identici. Li separa **solo** da che parte arriva il prezzo.

Le sette prove obbligatorie — lato di arrivo, sosia dichiarato, verso tracciabile alla fonte,
`--controlla`, `--prova` sulla storia, tempi del setup dichiarati, livello coperto nei due sensi —
stanno in [`sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md), sezione *"Le
Prove Che Una Condizione Deve Passare Prima Di Essere Armata"*. **Nessuna e' facoltativa, e valgono
a ogni riscrittura degli scenari.**

**Una condizione che usa la derivata di una fascia deve nominare la fascia in cui il prezzo SARA'
quando la condizione viene valutata**, non quella che sta lasciando: il delta di una fascia si
congela appena il prezzo ne esce.

Le difese automatiche verificano che una condizione sia **eseguibile**. Non possono accorgersi che
sia **sbagliata**: quello lo fanno solo le sette prove, a mano, prima di armare.

## Una Lettura Dal Vivo Dice La Direzione, Non La Lascia Intendere

**Quando l'utente chiede cosa fare, si risponde con una direzione e i suoi numeri.** Non con una
descrizione da cui dedurla, non con un elenco di rami equiprobabili, non con un cappello di
cautele. Richiesta esplicita dell'utente, 17 settembre 2026.

```text
DIREZIONE      LONG, SHORT oppure NIENTE. Una parola, per prima.
INGRESSO       a che prezzo, e su quale evento (chiusura sopra X, ritorno su Y)
STOP           un prezzo, dietro una STRUTTURA di ordini eseguiti - mai un numero tondo
BREAK EVEN     il LIVELLO al quale l'analisi si smonta, e quale struttura lo giustifica
BERSAGLIO      un prezzo e i punti che dista
INVALIDA       il prezzo che smonta la lettura
```

**Il campo BREAK EVEN non e' decorativo**: nel live e' l'edge. Uno stop senza il suo punto di break
even e' meta' istruzione. Sta in
[`la-gestione-della-posizione.md`](docs/research/metodo/la-gestione-della-posizione.md).

**`NIENTE` e' una direzione e si dice senza scusarsi.** "Non c'e' un setup, questi sono i due prezzi
che lo farebbero nascere" e' una risposta completa. Cio' che non e' una risposta e' descrivere il
tape e lasciare che sia chi legge a concludere.

**Le cautele di provenienza non entrano nella risposta dal vivo.** Restano nei file, che e'
tracciabilita'. **Quello che invece si dice sempre, perche' cambia cosa si fa:** se il volume e'
troppo sottile perche' il segnale valga, se una misura viene da una **barra non chiusa**, se una
lettura precedente e' stata smentita, e **qual e' il regime**.

**Non si da' gestione su una posizione senza sapere che l'utente e' dentro.**

## Come Si Scrive Una Lettura Dal Vivo

Chi legge **ha gli occhi sul grafico**, a mercato aperto, e deve capire in dieci secondi. La lettura
serve a dire quello che **non si vede guardando**.

**Tre parti, in quest'ordine, e nient'altro:**

1. **Lo stato**, una frase. Cosa sta facendo il prezzo, e se e' deciso o no.
2. **La misura**, due o tre numeri. Solo quelli che sostengono la frase sopra.
3. **Il discriminante**, una riga per ramo. Quale livello risolve, e cosa significa ciascun esito.

**Regole:**

- **Un numero entra solo se cambia la conclusione.**
- **Niente tabella sotto le quattro righe.**
- **Non si rielenca cio' che e' gia' sul chart.**
- **Un termine tecnico si spiega alla prima occorrenza della seduta**, non tutte le volte.
- **Se non e' cambiato niente, si scrive quella riga sola.**
- **Prima la conclusione, poi il perche'.**

**L'eccezione, e non e' negoziabile:** il file della giornata in
[`docs/research/giornate/`](docs/research/giornate/) e i documenti di metodo **restano completi**. Il
diario si legge a mercato chiuso e deve contenere tutto, comprese le misure che si riveleranno
sbagliate — sono la parte verificabile.

## Regole Di Lavoro

- **Non esiste un modello attivo e nessuna regola operativa e' approvata.** Descrivere non e'
  validare.
- **La fase sistematica sul modello 40R e' chiusa.** Non riaprirla senza richiesta esplicita.
- **Scrivi in modo comprensibile a una persona e a un agente**: spiega un termine tecnico alla prima
  occorrenza, frasi brevi, nessuna decisione importante lasciata solo nella conversazione.
- **Per ogni fase sostanziale** aggiorna il documento canonico pertinente e aggiungi **una riga
  datata** a [`FabioOrderFlow/progress.txt`](FabioOrderFlow/progress.txt).
- **Non accumulare output intermedi**: i file grezzi vanno nella scratchpad di sessione.
- **Ogni file di documentazione ha un nome descrittivo.** Niente `README.md`. L'ingresso del
  repository e' `da-dove-si-comincia.md`. L'unica cartella esclusa e' `docs/atas/`.
- **Registra i risultati negativi** con la stessa cura di quelli positivi, e dichiara sempre soglie,
  convenzioni e orizzonti accanto al numero che producono.

## Dati

Il percorso e' il **Data Bridge**. Contratto ed endpoint in
[`docs/research/metodo/contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md),
client `FabioOrderFlow/tools/bridge.py`.

Due vincoli tecnici che hanno gia' prodotto errori:

- **`bar` e' un indice di posizione, non un identificatore**: riparte quando ATAS ricarica
  l'indicatore. Per riconoscere una barra si usa `time`.
- **Il contratto continuo di ATAS non e' back-adjusted**: usa i contratti singoli o
  `FabioOrderFlow/tools/build_continuous.py`. Il controllo del rollover precede ogni altra misura.

**Il delta esiste anche prezzo per prezzo.** `bridge.py candles --levels` restituisce la footprint di
ogni barra — `ask` meno `bid` a ogni prezzo scambiato, piu' `maxPositiveDelta` e il POC di quella
barra. **E' lo strumento con cui si misura l'assorbimento del live**: sforzo alto, risultato nullo.
Procedura in
[`la-footprint-e-il-delta-per-prezzo.md`](docs/research/metodo/la-footprint-e-il-delta-per-prezzo.md).

**I big trades del live sono il filtro di volume nativo**: `bridge.py trades`, **60 su NQ in cash**,
20-30 in premarket. Non e' una nostra soglia, e' la taratura dichiarata da Fabio.

**La speed of tape non esiste nel bridge.** Il proxy e' il volume per barra M1 contro la
distribuzione recente. **Va dichiarato come proxy ogni volta che si usa.**

Durante una seduta gli scenari attesi si scrivono **prima**, in
`docs/research/giornate/scenari-AAAA-MM-GG.json`. `FabioOrderFlow/tools/scenari.py` e' solo il motore
che li valuta. **Le condizioni non vanno messe nel programma**: sono un file che si riscrive ogni
volta. `sveglia_tape.py` copre in parallelo cio' che non era previsto, e `annota.py` porta sul chart
e nel diario la lettura ragionata.

**I sorveglianti sono tre, e il terzo non guarda i livelli.** `scenari.py` risponde a *e' successo
quello che avevamo previsto?*, `sveglia_tape.py` a *e' successo qualcosa su un livello che conta?*,
`sveglia_movimento.py` a ***si e' mosso, dovunque fosse?*** — legge la barra **in formazione** e
grida ogni `--strappo` punti di escursione dall'estremo. Serve perche' i primi due **fra un livello
e l'altro non parlano**: il 17 settembre quel silenzio e' durato quattordici minuti con uno short
aperto dentro.

**Per riaccenderli c'e' `/accendi`**, che chiede le righe a `comandi_sorveglianti.py` invece di
scriverle a memoria. **Le soglie non si inventano e `--from` non si lascia indietro.** **Per
spegnerli c'e' `/spegni`**, che fa `TaskStop`, uccide gli orfani e **verifica**. I tre si accendono
come `Monitor`, non come comando in background. **Si riarmano al cambio di sessione.** Riavviando a
seduta in corso serve anche `--storia`, o lo strumento riparte cieco.

**I livelli, i bordi del valore e la finestra su cui si misurano si rifanno a ogni lettura che li
usa**, in qualunque fase della giornata. Procedura obbligatoria in
[`livelli-sul-chart.md`](docs/research/metodo/livelli-sul-chart.md): il ridisegno si **esegue**
invece di chiederlo, la finestra di misura si **dichiara** accanto al numero, e nessuno dei due
dipende dalla fase di sessione. I livelli arrivano da `POST /levels`: **l'indicatore disegna,
`bridge.py` trasporta, la derivazione resta nell'analisi.**

**Si cancella sempre tutto prima di ridepositare, e non e' facoltativo.** Un livello che l'analisi
non ha appena riconfermato non resta sul grafico: un residuo di un altro giorno si disegna esattamente
come una misura di adesso, e chi guarda non puo' distinguerli. Il 19 settembre, aprendo un replay
del 14, sul chart c'erano ancora otto livelli del 18 — quattrocento punti sopra il mercato.

**I livelli che si muovono si dichiarano come regola, non come prezzo**, in
`docs/research/giornate/livelli-vivi-STRUMENTO-AAAA-MM-GG.json`, e si ridepositano con
[`FabioOrderFlow/tools/livelli_vivi.py`](FabioOrderFlow/tools/livelli_vivi.py). POC, VAH, VAL,
massimo e minimo della finestra in sviluppo **cambiano a ogni barra**: vanno ricalcolati a ogni
lettura che li usa, insieme al resto dell'elenco. Il programma e' il motore, le regole stanno nel
file — la stessa separazione di `scenari.py`.

**Un movimento si misura col vincolo di ritracciamento, non come massimo di una finestra.** La
seconda misura e' l'inviluppo dell'opportunita'. Procedura e parametro R per strumento in
[`le-gambe-di-una-seduta.md`](docs/research/metodo/le-gambe-di-una-seduta.md), insieme alla regola
sui pattern letti all'indietro.

I recorder di agosto 2026 sono archiviati in
[`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/) e non vanno estesi.

## Build E Deploy

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Target `net10.0` senza WPF, requisito di ATAS X. Gli assembly ATAS sono risolti dal bundle
dell'applicazione su macOS e da `Program Files` su Windows. Dopo il deploy ATAS va riavviato.

**Un `git pull` non porta l'indicatore.** Nel repo c'e' il sorgente C#, non la DLL: `bin/` e
`obj/` sono in `.gitignore`, e ATAS carica il binario da `%APPDATA%\ATAS\Indicators` (Windows) o
da `~/Library/Application Support/ATAS/Indicators` (macOS). Finche' non si esegue `deploy.sh` e
non si riavvia ATAS, sul chart resta la versione precedente — **e non e' distinguibile a occhio da
un pull che non ha funzionato.** Non porta nemmeno la **memoria dell'agente**, che vive in
`~/.claude/projects/<percorso>/memory/`, fuori dal repo.

**La memoria di orientamento si installa, non si scrive a mano.** Il contenuto e' versionato in
`docs/research/memoria-di-orientamento/` e si deposita con
`python3 FabioOrderFlow/tools/semina_memoria.py`, che trova da solo la cartella di memoria della
macchina in uso. In memoria va **solo l'orientamento** — dove siamo, come ci siamo arrivati: le
regole stanno qui e nei documenti, e una memoria che le duplica prima o poi diverge senza
accorgersene. E' gia' successo: due voci su undici dicevano il contrario di questo file.

Su una macchina nuova, la sequenza completa e cosa va rifatto a mano stanno in
[`portare-il-repo-su-un-altra-macchina.md`](docs/research/portare-il-repo-su-un-altra-macchina.md).

Quando l'API ATAS non e' chiara, **ispeziona gli assembly con reflection** invece di dedurla dalla
documentazione: `docs/atas/` non sempre coincide con la build ATAS X installata.

Non modificare `docs/atas/api/` salvo necessita' tecnica concreta.
