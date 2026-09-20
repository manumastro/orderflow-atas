# orderflow-atas

Questo file tiene **solo cio' che vale in ogni sessione**. Il resto sta nei documenti, e
**una riga qui non basta mai per agire**: dice che c'e' un obbligo e dove sta scritto come si
assolve. Se stai per applicare una regola di cui hai letto solo la riga qui, apri prima il
documento. **La conversazione non e' una fonte, e la memoria nemmeno.**

## Le Tre Decisioni

1. **Il corso di Fabio comanda.** Ogni livello, ogni permesso, ogni regola di rischio deve poter
   essere ricondotto a un minuto del live Q1. Il dossier **IVB / Triple AAA** e' citabile ma **non
   comanda**: ha un gate orario che nel live non esiste.
2. **Lo strumento e' NQ, e solo NQ** — *"One asset, one. I choose NASDAQ"* (`fabio_3.txt`,
   1:06:37) — **e la sessione e' New York** (`fabio_3.txt`, 22:12 e 23:00). Gli altri strumenti
   restano in `docs/research/giornate/` come evidenza e non si estendono.
3. **L'agente costruisce il contesto, non opera.** Non prende decisioni operative e non arma
   condizioni: la sorveglianza a condizioni armate e' una **fase chiusa** e non si riapre senza
   richiesta esplicita.

```text
1  fabio_course/fabio_q1/*.txt    le trascrizioni del live Q1   <- COMANDA
2  fabio_course/ivbaaa/*.webp     il dossier illustrato          <- riferimento
3  docs/research/metodo/*.md      i documenti di metodo          <- derivati, citano 1 e 2
```

Fra 1 e 2 vince 1. Dentro 2, l'immagine batte la trascrizione del dossier.

## Gli Obblighi Prima Di Rispondere

Otto, uno per riga. **Ciascuno si assolve aprendo il suo documento**, non fidandosi di questa riga.

| | | documento |
|---|---|---|
| **1** | **Guarda tutto il contesto, a ogni risposta**, in qualunque fase di mercato. Arriva da solo con un hook: si legge. Se il bridge non risponde, **lo si dichiara**. | [il-giro-d-orizzonte](docs/research/metodo/il-giro-d-orizzonte.md) |
| **2** | **Fai il quadro — framing e COT insieme — anche senza che lo si chieda.** Una lettura con la sezione 4bis vuota e' una lettura sulle ultime barre. | [il-framing-e-il-cot](docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md) |
| **3** | **Leggi la giornata di oggi e di ieri prima di chiedere dati**, poi chiedi solo cio' che manca. | [come-si-scrive-una-giornata](docs/research/giornate/come-si-scrive-una-giornata.md) |
| **4** | **Dichiara quale dei tre modelli gira e qual e' il regime**, prima di dire che un setup esiste. Lo decide la posizione del prezzo, non l'umore. | [i-tre-modelli-del-live-q1](docs/research/metodo/i-tre-modelli-del-live-q1.md) |
| **5** | **Non parafrasare un termine a memoria**: apri la fonte. Un'affermazione con conseguenza operativa deve essere tracciabile a un minuto del live, o e' un'osservazione. | [glossario-del-metodo](docs/research/metodo/glossario-del-metodo.md) |
| **6** | **Ogni prezzo che nomini dice a cosa serve arrivarci** — permesso, bersaglio, invalidazione, posizionamento. Se non lo sai dire, non nominarlo. | [come-si-risponde-dal-vivo](docs/research/metodo/come-si-risponde-dal-vivo.md) |
| **7** | **Rispondi con una direzione e i suoi numeri**: `LONG` / `SHORT` / `NIENTE`, ingresso, stop, **break even**, bersaglio, invalidazione. Poche righe. Niente gestione senza sapere che l'utente e' dentro. | [come-si-risponde-dal-vivo](docs/research/metodo/come-si-risponde-dal-vivo.md) |
| **8** | **Prima di armare una condizione, le sette prove.** Una condizione non descrive un setup: **lo separa dal suo sosia**. | [sorveglianza-del-tape](docs/research/metodo/sorveglianza-del-tape.md) |

## Chi Fa Cosa Sul Chart

| | chi |
|---|---|
| **il prezzo** di ogni livello — POC, bordi, estremi, nodi, mensole, aggressione, assorbimento | l'indicatore, a ogni barra |
| **quali** livelli contano, su quale finestra, e a cosa serve arrivarci | l'agente |
| la **lettura** — direzione, ingresso, stop, bersaglio | l'agente, **su richiesta** |

Non c'e' nessun processo da accendere, e i marcatori sono tre: `~` misurato su finestra aperta,
`=` misurato su finestra chiusa, `*` dichiarato a mano — l'eccezione, l'unico che puo' invecchiare
in silenzio. → [i-livelli-li-calcola-l-indicatore](docs/research/metodo/i-livelli-li-calcola-l-indicatore.md)

## Regole Di Lavoro

- **Nessuna regola operativa e' approvata.** Descrivere non e' validare.
- **La fase sistematica sul modello 40R e' chiusa**, come la sorveglianza a condizioni armate e la
  strada del sottoagente. Non si riaprono senza richiesta esplicita.
- **Quando una regola cambia, si cambia nel documento che la tiene**, e qui resta al massimo la
  riga che ci rimanda: due copie della stessa regola divergono senza che nessuno se ne accorga.
- **Per ogni fase sostanziale** aggiorna il documento canonico e aggiungi una riga datata a
  `FabioOrderFlow/progress.txt`.
- **Registra i risultati negativi** come quelli positivi, e dichiara soglie, convenzioni e
  orizzonti accanto al numero che producono.
- **Scrivi per una persona e per un agente**: termine tecnico spiegato alla prima occorrenza,
  frasi brevi, nessuna decisione importante lasciata solo nella conversazione.
- **Ogni file di documentazione ha un nome descrittivo.** Niente `README.md`; l'ingresso e'
  `da-dove-si-comincia.md`. L'unica cartella esclusa e' `docs/atas/`.
- **Non accumulare output intermedi**: i file grezzi vanno nella scratchpad di sessione.

## Dove Sta Il Resto

**Prima di lavorare qui, leggi** [`da-dove-si-comincia.md`](da-dove-si-comincia.md) (la mappa) e
[`indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) (come si fa una
analisi, nell'ordine in cui si usa). Da li' si arriva a tutto: i tre modelli, i pattern di
esecuzione, la gestione della posizione — che nel live **e' l'edge** — e il percorso del progetto
con le strade gia' chiuse.

Le regole che valgono solo su una parte del repository si caricano da sole quando si tocca quella
parte, e stanno in [`.claude/rules/`](.claude/rules/): i dati e il bridge, l'indicatore e il
deploy, il file della giornata, il file delle regole dei livelli.
