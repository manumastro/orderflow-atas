# Gli Obblighi Della Lettura Dal Vivo

Fino al 10 ottobre 2026 questi otto obblighi stavano in `CLAUDE.md` e valevano a ogni risposta.
Dall'11 ottobre lo scope e' il metodo quantitativo ([`quant/il-metodo-quantitativo.md`](../../../quant/il-metodo-quantitativo.md))
e la lettura discrezionale e' diventata un supporto: **gli obblighi valgono quando l'utente chiede
una lettura del mercato dal vivo**, e allora valgono tutti. Ciascuno si assolve aprendo il suo
documento, non fidandosi della riga.

| | | documento |
|---|---|---|
| **1** | **Guarda tutto il contesto, dal piu' lontano al piu' vicino**: settimane, sedute, eventi, poi notte e tape. Si lancia a mano (`CLAUDE_PROJECT_DIR="$PWD" bash .claude/hooks/giro-orizzonte.sh`) e si legge. Se il bridge non risponde, **lo si dichiara**. Il calendario degli eventi lo tiene l'agente. | [il-giro-d-orizzonte](il-giro-d-orizzonte.md) |
| **2** | **Fai il quadro — framing e COT insieme — anche senza che lo si chieda.** Una lettura con la sezione 4bis vuota e' una lettura sulle ultime barre. | [il-framing-e-il-cot](il-framing-e-il-cot-si-leggono-insieme.md) |
| **3** | **Leggi la giornata di oggi e di ieri prima di chiedere dati**, poi chiedi solo cio' che manca. | [come-si-scrive-una-giornata](../giornate/come-si-scrive-una-giornata.md) |
| **4** | **Dichiara quale dei tre modelli gira e qual e' il regime**, prima di dire che un setup esiste. Lo decide la posizione del prezzo, non l'umore; il regime lo **misura l'indicatore** e il giro lo stampa, non si deduce a mano. | [i-tre-modelli-del-live-q1](i-tre-modelli-del-live-q1.md) |
| **5** | **Non parafrasare un termine a memoria**: apri la fonte. Un'affermazione con conseguenza operativa deve essere tracciabile a un minuto del live, o e' un'osservazione. | [glossario-del-metodo](glossario-del-metodo.md) |
| **6** | **Ogni prezzo che nomini dice a cosa serve arrivarci** — permesso, bersaglio, invalidazione, posizionamento. Se non lo sai dire, non nominarlo. | [come-si-risponde-dal-vivo](come-si-risponde-dal-vivo.md) |
| **7** | **Rispondi con una direzione e i suoi numeri**: `LONG` / `SHORT` / `NIENTE`, ingresso, stop, **break even**, bersaglio, invalidazione. Poche righe. Niente gestione senza sapere che l'utente e' dentro. | [come-si-risponde-dal-vivo](come-si-risponde-dal-vivo.md) |
| **8** | **Prima di armare una condizione, le sette prove.** Una condizione non descrive un setup: **lo separa dal suo sosia**. | [sorveglianza-del-tape](sorveglianza-del-tape.md) |

## La Gerarchia Delle Fonti Della Lettura

Per la lettura discrezionale vale ancora quella del corso:

```text
1  fabio_course/fabio_q1/*.txt    le trascrizioni del live Q1   <- comanda la lettura
2  fabio_course/ivbaaa/*.webp     il dossier illustrato          <- riferimento
3  docs/research/metodo/*.md      i documenti di metodo          <- derivati, citano 1 e 2
```

Fra 1 e 2 vince 1; dentro 2, l'immagine batte la trascrizione del dossier. Il dossier IVB / Triple
AAA e' citabile ma non comanda: ha un gate orario che nel live non esiste. Lo strumento e' NQ —
*"One asset, one. I choose NASDAQ"* (`fabio_3.txt`, 1:06:37) — e la sessione e' New York
(`fabio_3.txt`, 22:12 e 23:00).

## Chi Fa Cosa Sul Chart

| | chi |
|---|---|
| **il prezzo** di ogni livello — POC, bordi, estremi, nodi, mensole, aggressione, assorbimento | l'indicatore, a ogni barra |
| **il regime**, il proxy della velocita' e i big trades sopra soglia | l'indicatore, a ogni barra |
| **quali** livelli contano, su quale finestra, e a cosa serve arrivarci | l'agente |
| la **lettura** — direzione, ingresso, stop, bersaglio | l'agente, **su richiesta** |

Non c'e' nessun processo da accendere, e i marcatori sono tre: `~` misurato su finestra aperta,
`=` misurato su finestra chiusa, `*` dichiarato a mano — l'eccezione, l'unico che puo' invecchiare
in silenzio. → [i-livelli-li-calcola-l-indicatore](i-livelli-li-calcola-l-indicatore.md)

## Il Rapporto Con Il Metodo Quantitativo

Un'osservazione del live che sembra funzionare non e' una regola: e' un'idea, e passa la pipeline
del metodo quantitativo come ogni finding di un paper. La lettura dal vivo resta descrizione:
**nessuna regola operativa e' approvata**.
