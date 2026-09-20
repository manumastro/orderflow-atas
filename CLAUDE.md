# orderflow-atas — Istruzioni Per L'Agente

Questo file **non contiene contesto, e non contiene il dettaglio delle regole**. Contiene le
decisioni che governano tutto, l'obbligo di lettura, e **una riga per ogni obbligo con il
documento che lo spiega**. Il contesto e il dettaglio stanno nei documenti, ed e' li' che vanno
cercati: duplicarli qui li farebbe divergere — e' gia' successo.

**Una riga di questo file non basta mai per agire.** Dice *che* c'e' un obbligo e *dove* sta
scritto come si assolve. Se stai per applicare una regola di cui hai letto solo la riga qui, apri
prima il documento.

---

## Le Tre Decisioni Che Governano Tutto

**1. Il corso di Fabio comanda** (18 settembre 2026). Ogni operazione, ogni livello, ogni
condizione armata deve poter essere ricondotta a un minuto di una lezione del live Q1. Il
**modello IVB / Triple AAA** del dossier resta citabile — e' di Fabio, e va menzionato — ma **non
comanda**: descrive lo stesso setup dentro un'impalcatura con un gate orario che nel live non
esiste.

**2. Lo strumento e' NQ, e solo NQ.** *"Get good with one asset, one can pay your bills. One asset,
one. I choose NASDAQ"* (`fabio_q1/fabio_3.txt`, 1:06:37). Niente oro, niente crude, niente ES. I
file di quegli strumenti restano in `docs/research/giornate/` come evidenza e non si estendono.

**Conseguenza sulla sessione: si opera New York.** Nel live e' dichiarato due volte, col motivo
(`fabio_3.txt`, 22:12 e 23:00). La deroga "si opera anche a Londra" del 17 settembre e' anteriore a
questa decisione e non e' piu' attiva. La deroga "si opera prima del gate" decade da sola: **nel
live il gate non c'e'**.

**3. L'agente costruisce il contesto, non opera** (20 settembre 2026).

> *"non voglio l'agente che fa le operazioni ma che abbia e fornisca e aggiorna tutto il contesto
> necessario, questo e' lo scope e questo e' fondamentale"*

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

Prima di lavorare su questo repository, **leggi nell'ordine**. Nessuna e' facoltativa.

1. [`da-dove-si-comincia.md`](da-dove-si-comincia.md) — cos'e' il progetto e la mappa del repository.
2. [`i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md) — **il metodo
   che comanda**: i tre modelli, il vocabolario, il regime, la sessione, il COT, il framing, i
   timeframe. Ogni riga cita il minuto della lezione da cui viene.
3. [`i-pattern-di-esecuzione.md`](docs/research/metodo/i-pattern-di-esecuzione.md) — la libreria dei
   setup, e come si costruiscono ingresso, stop e bersaglio.
4. [`la-gestione-della-posizione.md`](docs/research/metodo/la-gestione-della-posizione.md) — break
   even, parziali, conto in R, size, budget di rischio. **Nel live e' l'edge, non un corollario.**
5. [`percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) — come si e' arrivati qui:
   le strade chiuse e perche', il COT, il Data Bridge.
6. [`come-lavora-l-utente.md`](docs/research/come-lavora-l-utente.md) — come va condotto il lavoro,
   e **il caso concreto da cui nascono** alcune regole di questo file. Una regola di cui si e'
   perso il motivo si erode.
7. [`indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) — **come si fa**
   una analisi, procedura per procedura, nell'ordine in cui si usano.
8. [`glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md) — **dove sta scritta**
   ogni definizione. Non definisce niente: dice a quale minuto o a quale riga tornare.

Se una risposta richiede contesto che non hai, il posto dove trovarlo e' li', non nella
conversazione.

---

## Gli Obblighi Prima Di Rispondere

**Otto, e ciascuno si assolve aprendo il suo documento.** L'ordine e' quello in cui si usano.

**1. Guarda tutto il contesto, per ogni risposta**, anche la piu' piccola, in qualunque fase di
mercato — globex, cash chiusa, weekend. Arriva da solo con un hook: **si legge, non si ignora**.
Se il bridge non risponde, **lo si dichiara**: una lettura senza dati va detta tale.
→ [`il-giro-d-orizzonte.md`](docs/research/metodo/il-giro-d-orizzonte.md)

**2. Fai il quadro — framing e COT insieme — anche senza che lo si chieda.** Sta nel file della
giornata fra `<!-- QUADRO -->`, e il giro lo stampa alla sezione 4bis. **Una lettura data con la
4bis vuota e' una lettura costruita sulle ultime barre.**
→ [`il-giro-d-orizzonte.md`](docs/research/metodo/il-giro-d-orizzonte.md),
[`il-framing-e-il-cot-si-leggono-insieme.md`](docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md)

**3. Leggi il file della giornata di oggi e quello di ieri prima di chiedere dati al bridge**, poi
chiedi solo i dati successivi all'ultimo aggiornamento. Il file si aggiorna **durante** la seduta,
non a posteriori: una giornata scritta alla fine perde le letture sbagliate, che sono la parte
verificabile.
→ [`come-si-scrive-una-giornata.md`](docs/research/giornate/come-si-scrive-una-giornata.md)

**4. Dichiara quale dei tre modelli sta girando e qual e' il regime**, prima di dire che un setup
esiste. Lo decide la posizione del prezzo, non l'umore.
→ [`come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md),
[`i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md)

**5. Non parafrasare un termine del metodo a memoria**: apri la fonte e leggi il passo. **Una
affermazione con conseguenza operativa deve essere tracciabile a un minuto del live**, o va
riscritta come osservazione.
→ [`glossario-del-metodo.md`](docs/research/metodo/glossario-del-metodo.md),
[`come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md)

**6. Ogni volta che nomini un prezzo, dichiara a cosa serve arrivarci** — permesso, bersaglio,
invalidazione o posizionamento. Se non lo sai dire, quel livello non va nominato, e probabilmente
non andava nemmeno disegnato.
→ [`come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md)

**7. Rispondi con una direzione e i suoi numeri** — `LONG`, `SHORT` o `NIENTE`, ingresso, stop,
**break even**, bersaglio, invalidazione. Tre parti, poche righe. Niente gestione su una posizione
senza sapere che l'utente e' dentro.
→ [`come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md)

**8. Prima di armare una condizione, le sette prove.** Una condizione non descrive un setup: **lo
separa dal suo sosia**. Nessuna e' facoltativa.
→ [`sorveglianza-del-tape.md`](docs/research/metodo/sorveglianza-del-tape.md)

---

## Il Contesto Sul Chart

**La sorveglianza a condizioni armate e' una fase chiusa.** `scenari.py`, `sveglia_tape.py` e
`sveglia_movimento.py` restano come evidenza e **non si riaprono senza richiesta esplicita**: una
condizione armata e' una previsione travestita da misura, e taceva proprio quando il mercato
faceva qualcosa di non previsto.
→ [`il-contesto-vivo.md`](docs/research/metodo/il-contesto-vivo.md)

**Non c'e' nessun processo da accendere: i livelli li calcola l'indicatore, a ogni barra**, dalle
regole che l'analisi deposita una volta in
`docs/research/giornate/regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json`, con
`bridge.py rules --file`.
→ [`i-livelli-li-calcola-l-indicatore.md`](docs/research/metodo/i-livelli-li-calcola-l-indicatore.md)

**La divisione del lavoro, ed e' la regola:**

| | chi |
|---|---|
| **il prezzo** di ogni livello — POC, bordi, estremi, nodi, mensole, aggressione, assorbimento | l'indicatore, a ogni barra |
| **quali** livelli contano, su quale finestra, e **a cosa serve arrivarci** | l'agente |
| la **lettura** — direzione, ingresso, stop, bersaglio | l'agente, **su richiesta** |

Il prezzo di una mensola e' una **misura**, e una misura non ha bisogno di qualcuno che la
rifaccia a mano. **Scegliere quali regole stanno nel file resta lavoro dell'analisi, a ogni
lettura che le usa**, e un `fisso` di ieri e' l'unico tipo che non si accorge del giorno.

**Tre marcatori sul chart, perche' le cose sono tre:**

```text
~   misurato, finestra ancora aperta    si muove a ogni barra
=   misurato, finestra chiusa           fermo, ma nato da un conteggio
*   dichiarato dall'analisi             e' un'affermazione, ed e' l'eccezione
```

**Il pannello e' l'indicatore, non un comando**, si ridisegna a ogni tick e **dichiara la propria
eta'**: un pannello fermo e indistinguibile da uno aggiornato e' il modo in cui uno strumento di
contesto danneggia invece di aiutare.
→ [`il-contesto-vivo.md`](docs/research/metodo/il-contesto-vivo.md)

**In replay il diario torna indietro da solo**, nello stesso hook del giro.
→ [`come-si-scrive-una-giornata.md`](docs/research/giornate/come-si-scrive-una-giornata.md)

---

## Dati

Il percorso e' il **Data Bridge**, client `FabioOrderFlow/tools/bridge.py`. Contratto, endpoint,
**i quattro vincoli che hanno gia' prodotto errori** (l'indice di barra, il rollover, la speed of
tape che e' solo un proxy, l'id del chart), la footprint prezzo per prezzo e la taratura dei big
trades stanno in
[`contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md).

**Il controllo del rollover precede ogni altra misura.**

---

## Regole Di Lavoro

- **Non esiste un modello attivo e nessuna regola operativa e' approvata.** Descrivere non e'
  validare.
- **La fase sistematica sul modello 40R e' chiusa.** Non riaprirla senza richiesta esplicita.
- **Scrivi in modo comprensibile a una persona e a un agente**: spiega un termine tecnico alla prima
  occorrenza, frasi brevi, nessuna decisione importante lasciata solo nella conversazione.
- **Per ogni fase sostanziale** aggiorna il documento canonico pertinente e aggiungi **una riga
  datata** a [`FabioOrderFlow/progress.txt`](FabioOrderFlow/progress.txt).
- **Quando una regola cambia, si cambia nel documento che la tiene**, e qui resta al massimo la
  riga che ci rimanda. Due copie della stessa regola divergono senza che nessuno se ne accorga.
- **Non accumulare output intermedi**: i file grezzi vanno nella scratchpad di sessione.
- **Ogni file di documentazione ha un nome descrittivo.** Niente `README.md`. L'ingresso del
  repository e' `da-dove-si-comincia.md`. L'unica cartella esclusa e' `docs/atas/`.
- **Registra i risultati negativi** con la stessa cura di quelli positivi, e dichiara sempre soglie,
  convenzioni e orizzonti accanto al numero che producono.
- **Un movimento si misura col vincolo di ritracciamento, non come massimo di una finestra.**
  → [`le-gambe-di-una-seduta.md`](docs/research/metodo/le-gambe-di-una-seduta.md)
- I recorder di agosto 2026 sono archiviati in
  [`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/) e non vanno estesi.

---

## Build E Deploy

```bash
cd FabioOrderFlow/src
./deploy.sh
```

**Su ATAS X non serve riavviare: basta il deploy**, e in replay la posizione non si perde.
**Un `git pull` non porta l'indicatore** — nel repo c'e' il sorgente C#, non la DLL — **e non porta
la memoria dell'agente.** La memoria di orientamento **si installa** con `semina_memoria.py`, non
si scrive a mano: in memoria va solo l'orientamento, le regole stanno qui e nei documenti.

Procedura completa, percorsi per sistema, e cosa rifare su una macchina nuova in
→ [`build-e-deploy.md`](docs/research/build-e-deploy.md)
