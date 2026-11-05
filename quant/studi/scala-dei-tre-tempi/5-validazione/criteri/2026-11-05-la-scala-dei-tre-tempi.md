# I Criteri Della Scala Dei Tre Tempi

Scritti il 5 novembre 2026, **prima di guardare NQ**: le uniche prove fatte prima di questo file sono sul
mercato sintetico (`SINT`, registro nella scratchpad), per vedere che le operazioni stanno dove devono.
Questo file non si modifica. Un criterio sbagliato si corregge con un file nuovo che cita questo.

**Campione**: NQ M1 dell'archivio, solo dentro campione (dal 12 aprile 2017 al 31 dicembre 2024, la
divisione fissata il 15 ottobre 2026). Il fuori campione (dal 2025) non si guarda.

**Esecuzione**: quella standard del motore (1 contratto, commissione 2,50 $ per lato, 1 tick di
slittamento, ambiguita' al peggio). Ingresso a mercato alle 9:30 ET, uscita alle 16:00 ET.

**Le varianti, tutte e cinque, e nessun'altra** (entrano tutte nel registro):

| | tempi | memoria | perche' |
|---|---|---|---|
| V1 | mese, settimana, giorno | candela | la regola letterale: "I always judge the last completed candle" `[V1 · 3:14]`; **la principale** |
| V2 | mese, settimana, giorno | scalino | come il video legge il mese: "the current is still up, it's just resting" `[V1 · 4:03]` |
| V3 | settimana, giorno | candela | senza il mese, che cambia di rado |
| V4 | settimana, giorno | scalino | |
| V5 | giorno | candela | il controllo: il solo tempo piu' vicino |

## K1. Il Verso Dei Tre Tempi Allineati (V1)

- **Quantita'**: i p del nullo del quando (D1) e del nullo del verso (D2), 1.000 repliche, netto totale.
- **Campione minimo**: 30 operazioni. Sotto, l'esito e' "non decidibile", non "smentita".
- **Smentita se** D1 o D2 ha p > 0,05.

## K2. Lo Sharpe Deflazionato (V1)

- **Quantita'**: lo Sharpe deflazionato di V1 con N = le varianti nel registro al momento della validazione.
- **Smentita se** < 0,95 (il cancello D3).

## K3. Il Tempo Misto E' Equilibrio (descrizione, non strategia)

Il video: con i tempi misti "small size, walls only" `[V1 · 3:30 - 3:35]`, cioe' una giornata che ruota.
- **Quantita'**: il range della cassa (massimo meno minimo dalle 9:30 alle 16:00 ET, in punti) di ogni
  giornata, divisa in due gruppi con la classificazione di V1: **allineati** (verso +1 o -1) e **misti**
  (verso 0). Test di Welch sul logaritmo del range, **una giornata = un'osservazione**, a una coda
  (misti piu' stretti).
- **Campione minimo**: 30 giornate per gruppo.
- **Smentita se** p > 0,05, oppure se la mediana dei misti non e' piu' bassa di quella degli allineati.
- **Riserva scritta prima**: i tempi allineati arrivano dopo movimenti forti, quando la volatilita' e' piu'
  alta. Una differenza puo' venire da li', non dalla scala. Si scrive anche il range diviso per il range
  medio delle 20 giornate di cassa precedenti, come seconda lettura, senza soglia.
