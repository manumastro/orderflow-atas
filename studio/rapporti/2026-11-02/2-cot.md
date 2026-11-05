# Fase 2 · Il COT · 2026-11-02

**Stato**: parziale (manca la scala dei POC fra il 27/10 e oggi: `sedute-NQZ6.json` non e' stato raccolto)
**Dati**: `dati/cot.json`, raccolto alle 10:57 ora italiana (09:57 UTC; `raccolta.json` chiusa alle 10:58), fonte CFTC
(Legacy futures only e TFF). Report di martedi' 27 ottobre, pubblicato venerdi' 30 alle 20:30 italiane: **6 giorni**.
Prezzo di adesso NQZ6 28.780,25 alle 10:57 italiane, fuori dalla cassa. Errori: nessuno.

**In una riga**: gli speculatori sul Nasdaq restano lunghi quasi al massimo di tre anni e al 27/10 hanno alleggerito
poco; i gestori di fondi hanno venduto come quasi mai; il report non vede il calo del 29-30/10.

## In Parole Semplici

**Il punto**: chi specula sul Nasdaq e' ancora molto sbilanciato al rialzo, e tutte le date dei report stanno da 1.000
a 2.700 punti sopra il prezzo di oggi. Il dato si ferma a martedi' 27: il calo di giovedi' e venerdi' si vedra' il 6 novembre.

**Dal piu' ampio al piu' vicino**

1. **Tre anni**: gli speculatori hanno ancora quasi la scommessa al rialzo piu' grande dal 2023, e quasi nessuna al ribasso.
   → Il peso resta tutto da una parte, come una settimana fa.
2. **L'ultima settimana**: hanno tolto poco, circa 1.900 contratti; una settimana fa ne aggiungevano poco.
   → Prima del calo nessuna fuga: un piccolo alleggerimento.
3. **I gestori di fondi** (un'altra classificazione dello stesso mercato): hanno venduto circa 12.400 contratti, uno dei movimenti piu' forti in tre anni.
   → Sotto la superficie qualcuno di grande vendeva gia' prima del 27; nessuno studio prova cosa ne segue.
4. **Gli altri indici**: S&P 500, Dow e Russell aggiungono un po' al rialzo; solo il Nasdaq toglie.
   → La conferma fra indici manca: il Nasdaq si muove da solo.
5. **La data sul prezzo**: il 27 ottobre il Nasdaq si scambiava intorno a 29.800; oggi sta circa 1.000 punti sotto, e sotto il minimo della settimana del report.
   → Chi e' posizionato al rialzo sta sopra il prezzo: e' scomodo, e piu' di una settimana fa.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.775 | il prezzo piu' scambiato del 13 ottobre, la settimana in cui gli speculatori hanno tolto di piu' | si torna dove hanno venduto: circa 2.000 punti sopra oggi |
| circa 29.640-29.880 | il valore del 27 ottobre, la data dell'ultimo report | il posto del report piu' recente, circa 1.000 punti sopra |
| circa 29.030 | il bordo basso del valore della settimana 21-27 ottobre | il bordo di valore dei report piu' vicino sopra il prezzo |

**Quanto e' provato**: i numeri sono misurati (CFTC e ATAS). Che queste date diventino livelli e' la procedura del
corso, non una regola provata. Il COT non dice mai *quando*, e non sappiamo ancora chi ha venduto il 29 e il 30.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi tiene i futures; **speculatori** = la categoria
Non-Commercial (fondi e trader grandi); **gestori di fondi** = la categoria Asset Manager del rapporto TFF;
**valore** = la fascia dove passa il 70% del volume della seduta.

## I Fatti

Legacy, solo Non-Commercial, report del 27/10 (`cot.json` > `indici`), percentili su 160 settimane.
Skew = (long - short)/(long + short); soglia 0,30 nostra, dal "delta del 30%" del live `[1 · 33:12]`.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto / long / short | skew fondo / var. | z52 var. |
|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 | 84.567 | 35.728 | 48.839 | -1.532 | +355 | -1.887 | 95,0 / 64,7 / 3,8 | +0,41 / -1,00 | -0,17 |
| S&P 500 | 215.924 | 350.292 | -134.368 | +39 | -8.264 | +8.303 | 34,4 / 7,7 / 51,9 | -0,24 / +1,00 | +0,23 |
| Dow x5 | 19.827 | 12.083 | 7.744 | -785 | -1.804 | +1.019 | 60,6 / 53,8 / 37,2 | +0,24 / +0,39 | +0,18 |
| Russell 2000 | 112.363 | 148.442 | -36.079 | -3.431 | -9.261 | +5.830 | 36,2 / 99,4 / 94,9 | -0,14 / +0,46 | +0,46 |

NASDAQ-100, le ultime quattro (`ultime_4`; nessuna in rollover, ne' per open interest ne' per calendario). Open interest 268.508 (-1,5%).

| report | netto | var. netto | skew | z52 | pct var. 3a |
|---|---|---|---|---|---|
| 27/10 | 48.839 | -1.887 | -1,00 | -0,17 | 21,2 |
| 20/10 | 50.726 | +2.203 | +0,75 | +0,20 | 23,1 |
| 13/10 | 48.523 | -9.600 | -1,00 | -0,86 | 71,2 |
| 06/10 | 58.123 | +6.876 | +1,00 | +0,58 | 59,0 |

La data sul prezzo (`prezzo_alle_date`, `settimane_dei_report`): NQZ6, cassa 09:30-16:00 New York (14:30-21:00
italiane il 27/10, 15:30-22:00 nelle altre date), fasce da 1 punto. "Adesso dal POC" = 28.780,25 meno il POC.

| report | giorno: POC, valore, chiusura, delta | chiude | adesso dal POC | settimana: POC, valore, delta | adesso dal POC sett. |
|---|---|---|---|---|---|
| 27/10 | 29.805, 29.637-29.880, 29.813,25, -565 | dentro | -1.024,75 | 29.500, 29.026-29.679, -5.567 | -719,75 |
| 20/10 | 29.757, 29.599-29.842, 29.688, -982 | dentro | -976,75 | 29.770, 29.584-30.178, -524 | -989,75 |
| 13/10 | 30.775, 30.712-30.800, 30.788,75, -270 | dentro | -1.994,75 | 30.890, 30.846-31.315, +14.648 | -2.109,75 |
| 06/10 | 31.530, 31.483-31.556, 31.480,75, -3.557 | sotto | -2.749,75 | 30.824, 30.529-31.213, -4.000 | -2.043,75 |

Settimana 21-27/10: da 29.874 a un minimo di 28.987, chiusura 29.813,25 sopra il VAH settimanale 29.679; nessun giorno mancante. Ritardo dalla chiusura del 27/10: -1.033 punti (-3,46%).

TFF del Nasdaq, 27/10 (`tff_nasdaq`), percentili a tre anni:

| categoria | netto | var. netto (long / short) | z52 | pct var. 3a | pct netto / long / short |
|---|---|---|---|---|---|
| Dealer | -85.514 | +8.608 (+2.893 / -5.715) | +0,58 | 53,8 | 10,3 / 33,3 / 85,9 |
| Asset Manager | 65.381 | -12.409 (-3.793 / +8.616) | **-2,06** | **97,4** | 24,4 / 50,0 / 89,1 |
| Leveraged Funds | 2.301 | +1.351 (-578 / -1.929) | +0,11 | 7,7 | 94,9 / 49,4 / 0,6 |

TFF dell'S&P 500 (`tff_sp500`): Asset Manager +22.072 (z +0,81); Leveraged Funds -29.359 (z -0,62), long al 0,6 percentile.

## Cosa Dicono Per NQ

- **Passi 1-3** `[1 · 27:46]`, `[1 · 34:51]`: Legacy, solo Non-Commercial. Netto al 95,0 percentile (massimo di tre
  anni 58.123, il 6/10). **Il campo estremo resta lo short** (3,8), non il long (64,7). Skew di fondo +0,41, come al 20/10.
- **Passo 4** `[1 · 30:01]`, `[1 · 33:12]`: skew della variazione saturo a -1 (long giu', short su), quindi conta lo
  z: -0,17, 21 percentile. Il piede passa ai venditori, appena appoggiato. La piu' grande delle quattro resta il 13/10.
- **Passo 5** `[1 · 32:31]`: **1 su 4**, il Nasdaq da solo (al 20/10 era 2 su 4); nessun indice supera |z| 0,5. La
  metrica cross-index **non ha retto** alla verifica (`analisi-istituzionale.md`): si riporta, non si pesa.
- **Passo 6** `[1 · 31:53]`: il 27/10 gli speculatori alleggerivano poco in una seduta che chiudeva **dentro il valore,
  sopra il POC** (29.813,25 contro 29.805), delta -565. **Il livello del passo resta il POC del 13/10, 30.775**: la
  variazione piu' grande delle quattro (-9.600, z -0,86).
- **Cosa e' cambiato dal 28/10**: il netto passa da +2.203 a -1.887, entrambi piccoli; la conferma fra indici da 2 a 1
  su 4; i gestori di fondi del TFF vendono (z -2,06) dopo settimane di acquisti piccoli; la distanza dal POC del 13/10
  passa da -1.195 a -1.995 punti, e il prezzo e' sotto il minimo della settimana 21-27/10 (28.987).
- **Osservazione** (derivazione nostra, non passo del corso: `il-framing-e-il-cot-si-leggono-insieme.md`): tutti i POC
  dei giorni e delle settimane dei report stanno sopra il prezzo, da 720 a 2.750 punti. Il lato lungo sta sopra il
  cuore del volume, e piu' di una settimana fa.
- **TFF, con riserva** (Legacy e TFF non sono sinonimi, `analisi-istituzionale.md`): il -1.887 Legacy contiene
  movimenti opposti piu' grandi, gestori di fondi -12.409 e Dealer +8.608. **Osservazione**: la vendita dei gestori
  di fondi precede il calo del 29-30/10; nessuna soglia COT e' validata.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.775 (30.712-30.800) | POC del 13/10, la variazione piu' grande (-9.600) | posizionamento: dove gli speculatori hanno alleggerito; dice se li' il prezzo viene respinto, come nell'esempio del live |
| 30.890 (30.846-31.315) | POC della settimana 07-13/10 | lo stesso, sul profilo della settimana coperta |
| 29.805 (29.637-29.880) | POC e valore del 27/10, ultimo report | posizionamento piu' recente; il prezzo di adesso sta circa 860 punti sotto il VAL |
| 29.500 (29.026-29.679) | POC della settimana 21-27/10 | il VAL 29.026 e' il bordo di valore dei report piu' vicino sopra il prezzo (+246) |
| ven 6/11, 21:30 italiane | prossimo COT (dati di martedi' 3/11) | il primo report che contiene il calo del 29-30/10; nuova data, nuovo livello. Di nuovo 21:30: New York e' all'ora solare dal 1/11 |

## Cosa Non Sappiamo

- **Eta'**: 6 giorni; il COT non dice *quando*, e **non vede il 28, 29 e 30 ottobre**. **Rollover**: nessuna delle quattro settimane.
- **Nessuna soglia COT e' validata** qui: la convergenza framing-COT e la lettura del TFF sono osservazioni.
- Il prezzo di adesso e' fuori dalla cassa (lunedi' mattina): il confronto con i valori di cassa e' fra sessioni diverse.
- lacuna del collettore: `dati/sedute-NQZ6.json` non c'e'; manca la scala dei POC fra il 27/10 e oggi (il profilo del calo del 29-30/10).
- lacuna del collettore: percentili di long e short assoluti per ciascuna delle `ultime_4`.
- lacuna del collettore: il profilo del venerdi' di pubblicazione (30/10), quando il dato diventa pubblico.

## Per Il Quadro

1. Netto Non-Commercial Nasdaq al 95,0 percentile, short al 3,8, gestori di fondi in vendita forte (z -2,06) **prima**
   del calo: coerente con il regime della macro, o un posizionamento che deve ancora reagire ai circa 1.030 punti persi dopo il 27/10?
2. Livello del passo 6: **30.775** (13/10) e 30.890 della settimana, circa 2.000 punti sopra: confrontarli con muri e zero gamma.
3. **29.637-29.880** (valore del 27/10) e **VAL settimanale 29.026**: i bordi di valore dei report piu' vicini sopra il
   prezzo; confrontarli con muri, zero gamma e il valore delle sedute del 29-30/10 dell'order flow.
4. Cross-confirm 1 su 4 (S&P, Dow e Russell aggiungono, il Nasdaq toglie; Leveraged Funds dell'S&P con il long al
   minimo di tre anni): coerente con la rotazione che legge la macro?
