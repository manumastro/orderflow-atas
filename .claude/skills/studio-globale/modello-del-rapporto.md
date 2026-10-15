# Il Modello Del Rapporto Di Una Fase

Ogni fase dello studio globale scrive **un file**, `studio/rapporti/<data>/<n>-<fase>.md`, con queste
sezioni e in quest'ordine. Italiano, apostrofi ASCII (`e'`, `puo'`), orari in **ora italiana**, numeri
con il punto delle migliaia e la virgola dei decimali. **Al massimo 120 righe**, il quadro 150: e' un rapporto, non
un'enciclopedia; il dettaglio sta nei JSON di `dati/`, e si cita.

```markdown
# Fase <n> · <Nome> · <data>

**Stato**: fatto | parziale (cosa manca) | senza dati (perche')
**Dati**: i file di `dati/` letti, l'ora della raccolta (da `raccolta.json`), la freschezza di ogni
fonte (es. COT al martedi' 6 ottobre, 9 giorni), gli errori della raccolta, o "nessuno".

**In una riga**: <la fase in una frase, descrittiva>

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
