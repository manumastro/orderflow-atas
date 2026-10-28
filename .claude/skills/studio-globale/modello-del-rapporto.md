# Il Modello Del Rapporto Di Una Fase

Ogni fase dello studio globale scrive **un file**, `studio/rapporti/<data>/<n>-<fase>.md`, con queste
sezioni e in quest'ordine. Italiano, apostrofi ASCII (`e'`, `puo'`), orari in **ora italiana**, numeri
con il punto delle migliaia e la virgola dei decimali. **Al massimo 135 righe**, il quadro 180 (lo schema per la persona ne prende circa 30): e' un rapporto, non
un'enciclopedia; il dettaglio sta nei JSON di `dati/`, e si cita.

**Ogni rapporto ha due lettori, e la persona viene prima** (richiesta dell'utente del 21 ottobre 2026:
"vorrei che sia piu' comprensibile tutto all'umano"). Le sezioni tecniche servono all'agente del
quadro e restano com'erano; in cima c'e' **In parole semplici**, scritta per una persona che sta
imparando il mercato e le opzioni. E' la parte che finisce in chat e in cima all'artifact.

**In parole semplici e' uno schema fisso, non un discorso** (21 ottobre 2026, seconda richiesta: "non
ha un ordine chiaro, sembra non arrivare mai al dunque, deve essere tutto piu' chiaro e schematico,
con una visione dall'ampio al piu' vicino"; l'utente ha scelto lo schema a gradini). Sempre le stesse
parti, nello stesso ordine:

```markdown
## In Parole Semplici

**Il punto**: <una o due frasi: dove siamo e cosa vuol dire. E' la conclusione, e viene prima.>

**Dal piu' ampio al piu' vicino**

1. **<Gradino>**: <il fatto, una frase>.
   → <cosa vuol dire, una frase>.
2. ...

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa <prezzo> | <in parole comuni> | <cosa cambia per il quadro> |

**Quando**: <ora italiana> <evento> · <ora> <evento>
**Cambia idea se**: <i fatti che renderebbero sbagliato il punto>
**Quanto e' provato**: <una frase: cosa e' misurato, cosa e' modello, cosa nessuno studio prova>

**Le parole**: **<termine>** = <significato>; **<termine>** = <significato>; ...
```

- **I gradini** vanno dal piu' lontano al piu' vicino. Nel quadro sono sei: **Il mondo** (macro),
  **Le posizioni** (COT), **Le opzioni** (gamma), **Le settimane**, **I giorni**, **Adesso** (order
  flow). In una fase sono da tre a cinque, dal lontano al vicino dentro la fase (per esempio, per
  l'order flow: le settimane, i giorni, la notte, adesso).
- **Ogni gradino sta in due righe**: il fatto, poi la freccia con cosa vuol dire. Se servono tre
  righe, il fatto ha troppi numeri.
- **I prezzi da guardare** sono al massimo tre o quattro, nel quadro; in una fase, solo se la fase ne
  ha. **Quando** e **Cambia idea se** stanno nel quadro, e in una fase solo se servono.
- **Le parole**: solo i termini usati nello schema, su una o due righe. Il glossario lungo, se serve,
  va nelle sezioni tecniche.

**Le regole di scrittura**:

- **Frasi brevi**, una idea per frase. Prima la conclusione, poi il perche'.
- **Nessuna sigla senza spiegazione**: POC, VAH, VAL, CVD, GEX, 0DTE, COT, NC, OI, VXN, pb, pct si
  scrivono per esteso o si spiegano tra parentesi in parole comuni ("il prezzo piu' scambiato del
  giorno", "chi vende le opzioni"). Niente codici interni: "convenzione A", "z52", "stesso_dato",
  "passo 5", i minuti del live `[1 · 24:02]`, i nomi dei file restano nelle sezioni tecniche.
- **Ogni numero dice cosa significa**: non "netto al 95,6 pct" ma "gli speculatori sono lunghi come
  quasi mai negli ultimi tre anni". Si arrotonda (circa 30 punti, quasi 1.000 punti) e si danno pochi
  numeri: i prezzi che contano, non tutti.
- **Cosa significa per il prezzo**, e **quanto e' provato**: un'osservazione si dice osservazione, un
  modello si dice modello ("in teoria", "nessuno studio lo prova su NQ").
- **Cosa non sappiamo** sta in "Quanto e' provato", in una frase, se cambia la lettura.

Le regole di sempre valgono anche qui: nessuna direzione, nessun ingresso, stop o bersaglio.

```markdown
# Fase <n> · <Nome> · <data>

**Stato**: fatto | parziale (cosa manca) | senza dati (perche')
**Dati**: i file di `dati/` letti, l'ora della raccolta (da `raccolta.json`), la freschezza di ogni
fonte (es. COT al martedi' 6 ottobre, 9 giorni), gli errori della raccolta, o "nessuno".

**In una riga**: <la fase in una frase, descrittiva, senza sigle>

## In Parole Semplici
Lo schema fisso qui sopra: il punto, i gradini dal piu' ampio al piu' vicino, quanto e' provato, le parole.

## I Fatti
I numeri, in tabelle corte, ciascuno con la sua fonte. Niente interpretazione qui.

## Cosa Dicono Per NQ
La lettura: la catena causale, con il riferimento al documento del metodo, al minuto del live o al
paper da cui viene. Un'affermazione con una conseguenza lo porta, o e' marcata **osservazione**.

## I Livelli E Le Date
| prezzo o data | da dove viene | a cosa serve arrivarci |
Ogni prezzo dice a cosa serve arrivarci (permesso, bersaglio, invalidazione, posizionamento): se non
lo sai dire, non nominarlo. Le date: eventi con l'ora italiana.

## Cosa Non Sappiamo
I limiti dei dati e delle convenzioni, le ipotesi non provate, e **le lacune del collettore**: ogni
misura che sarebbe servita e non c'e' in `dati/`, scritta come `lacuna del collettore: <cosa>`.

## Per Il Quadro
Tre o quattro agganci per la fase 5: i numeri e i livelli di questa fase che vanno confrontati con le
altre (per esempio "il netto Non-Commercial al massimo di tre anni: coerente con il regime dei tassi
della macro?"). La fase 5 fa il confronto; tu dai gli agganci.
```

**Vietato in ogni rapporto**: LONG, SHORT, ingresso, stop, bersaglio operativo, size. Il quadro dice
dove si sta, non cosa fare. Le soglie si dichiarano accanto al numero che producono.
