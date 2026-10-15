---
name: studio-globale-4-orderflow
description: Fase 4 dello studio globale di NQ - l'order flow delle quattro tab di ATAS in ordine (1 composito Daily, 2 settimana M30, 3 seduta M1, 4 esecuzione Range 40), con il profile framing del live, il pannello e il regime misurato dall'indicatore. Legge i dati gia' raccolti e scrive 4-orderflow.md.
argument-hint: "[cartella del rapporto]"
---

# Fase 4: L'Order Flow Delle Quattro Tab

Dove si e' costruito il valore e chi sta spingendo adesso, tab per tab, dalla piu' lontana alla piu'
vicina. Il rapporto segue [`../studio-globale/modello-del-rapporto.md`](../studio-globale/modello-del-rapporto.md).

## Leggi Prima

- [`docs/research/metodo/profile-framing.md`](../../../docs/research/metodo/profile-framing.md): i sei
  passi del framing; e la **sezione 10** di [`i-tre-modelli-del-live-q1.md`](../../../docs/research/metodo/i-tre-modelli-del-live-q1.md)
  (solo cash, composito di 90 giorni, una gaussiana per seduta, due POC, premium e discount);
- il [`glossario-del-metodo.md`](../../../docs/research/metodo/glossario-del-metodo.md) solo per i termini
  che usi.

## I Tuoi Dati

Tutti in `dati/`, raccolti da `studio/raccogli.py` dal bridge di ATAS:

| file | cosa c'e' |
|---|---|
| `tab-ruoli.json` | il ruolo di ogni tab con i suoi numeri: composito di 90 giornate (grezzo e aggiustato al rollover), valore di cassa di ogni giorno (M30), sedute di riferimento (M1, Range) |
| `quattro-tab.txt` | le quattro tab come le stampa il giro |
| `sedute-NQZ6.json` | **le ultime 20 sedute di cassa**: apertura, chiusura, estremi, volume, delta, POC, VAL, VAH, dove chiude, valore su o giu', staccato, **shift** alla lettera del live; la piu' recente con `profilo_da_10` (le fasce da 10 punti sopra il 2% del volume). **`oggi_notte`** (22:00Z-13:30Z) e **`oggi_cassa`** (in corso: stesse misure delle sedute chiuse, piu' `in_corso`, shift, stacco, dove sta l'ultimo prezzo), ciascuna con il suo profilo. I due compositi di sola cassa: `composito_di_cassa` (20 sedute) e **`composito_di_cassa_90`** (la regola del live: si riempie con l'archivio, `sedute` dice quante ne ha finche' non arriva a 90), con i nodi in percento del volume; `zona_nel_composito_di_cassa` (premium sopra il VAH, discount sotto il VAL); `rendimento_dalla_chiusura_di_ieri` (la variabile di BDLM) |
| `ore-NQZ6.json` | le ultime 48 ore a barre orarie, con il delta, e la `volatilita_realizzata` (rendimenti a 5 minuti, annua) a 4, 24 e 48 ore |
| `range-NQZ6.json` | la durata delle barre range: percentili su 600 barre e mediana delle ultime 30, il metro per dire "veloce" o "lenta" |
| `pannello.json` | il pannello dell'indicatore: il regime **con la sua finestra** (le ultime N barre e l'ora), la velocita' della barra in formazione **e della barra chiusa**, il livello in gioco, i veti, misurati |
| `giro.txt` | il giro d'orizzonte intero, raccolto **dopo** la GEX: il tape, il profilo della notte, i livelli sul chart, la struttura oraria letta sugli ultimi due swing |
| `confluenze.json` | i prezzi che order flow, gamma e COT indicano entro 15 punti, con `stesso_dato` quando due fonti leggono lo stesso numero del bridge: e' il punto di partenza del quadro, non una conclusione |

**Quando lo stesso numero arriva da due fonti** (per esempio il VAL della notte), vale il pannello per i
livelli dal vivo, `sedute-NQZ6.json` per le sedute chiuse; lo scarto di rollover e' quello di
`tab-ruoli.json` (l'indicatore). La differenza si scrive, non si sceglie a occhio.

**Non hai Bash e non ricalcoli niente.** Il valore, il regime e i livelli li misurano l'indicatore e il
collettore: **si leggono, non si ricostruiscono** (`dati-e-bridge`, "il pannello si legge"). Se manca
una misura, e' una lacuna del collettore e la scrivi.

## Le Quattro Tab, In Ordine

1. **Composito**: valore e POC grezzi e aggiustati; il prezzo in premium, fair value o discount.
2. **Settimana**: il valore di ogni giorno; se sale, scende o si sposta (shift = VAL **e** VAH piu'
   bassi), le aree staccate; **la scala dei POC** sopra e sotto il prezzo, e il POC della seduta che ha
   creato la rottura.
3. **Seduta**: le ultime sedute (chiude dentro, sopra, sotto il proprio valore), la notte e Londra, la
   struttura oraria, il CVD, **il regime e la velocita' come li misura l'indicatore**, i veti.
4. **Esecuzione**: le ultime barre range, verso, durata, delta; se il ritmo accelera o si spegne.

Il **vuoto** si cerca fra il VAH di una seduta e il VAL di un'altra, non in una fascia sottile.

## Scrivi `4-orderflow.md`

Nei **Livelli**: la scala dei POC, i bordi, i vuoti, ciascuno con a cosa serve arrivarci. Negli agganci
**Per il quadro**: i livelli da confrontare con quelli del COT (le date del report) e della gamma. Il
regime si dichiara, il setup no.
