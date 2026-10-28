# Fase 2 · Il COT · 2026-10-27

**Stato**: fatto (manca la scala dei POC giornalieri: `sedute-NQZ6.json` non e' stato raccolto)
**Dati**: `dati/cot.json`, raccolto alle 12:18 ora italiana (`raccolta.json`), fonte CFTC (Legacy
futures only e TFF). Report di martedi' 20 ottobre, pubblicato venerdi' 23: **7 giorni**. Prezzo di
adesso NQZ6 29.579,75 alle 12:18 italiane (11:18 UTC, fuori dalla cassa). Errori: nessuno.

**In una riga**: gli speculatori sul Nasdaq restano lunghi quasi al massimo di tre anni, ma il prezzo
sta sotto il valore di tutte e quattro le date dei report; la data in cui hanno alleggerito di piu'
(13 ottobre) e' un prezzo circa 1.200 punti piu' in alto.

## In Parole Semplici

**Il punto**: chi specula sul Nasdaq e' ancora molto sbilanciato al rialzo, ma ha comprato e tenuto a
prezzi piu' alti di quelli di oggi. Sulla carta e' in perdita, e il dato ha gia' una settimana.

**Dal piu' ampio al piu' vicino**

1. **Tre anni**: gli speculatori hanno quasi la scommessa al rialzo piu' grande dal 2023, e quasi
   nessuna al ribasso.
   → Il peso e' tutto da una parte: se cambiano idea, c'e' molto da vendere.
2. **Le ultime quattro settimane**: il 13 ottobre hanno tolto la fetta piu' grande del mese (quasi
   10.000 contratti); il 20 ne hanno rimessi pochi.
   → Il mese mostra un alleggerimento, non una fuga.
3. **Gli altri indici**: S&P 500 e Dow si muovono al contrario del Nasdaq, il Russell nello stesso verso.
   → La conferma fra indici e' a meta'.
4. **La data sul prezzo**: il 13 ottobre il Nasdaq si scambiava intorno a 30.775; oggi sta circa
   1.200 punti sotto. Anche il valore del 20 ottobre (circa 29.600-29.760) e' appena sopra.
   → Chi e' posizionato al rialzo sta sopra il prezzo: e' scomodo, non in guadagno.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.775 | il prezzo piu' scambiato del 13 ottobre, quando gli speculatori hanno alleggerito di piu' | si torna dove hanno venduto: si guarda se li' il prezzo viene respinto |
| circa 29.600-29.760 | il valore del 20 ottobre, la data dell'ultimo report | e' il posto del report piu' recente; il prezzo ci sta appena sotto |

**Quanto e' provato**: i numeri sono misurati (CFTC e ATAS). Che il livello del 13 ottobre conti e' la
procedura del corso, non una regola provata: la verifica fatta qui fra indici non ha trovato
differenze nei movimenti successivi. Il COT non dice mai *quando*.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi tiene i futures; **speculatori** = la
categoria Non-Commercial (fondi e trader con posizioni grandi); **valore** = la fascia di prezzi dove
passa il 70% del volume della seduta.

## I Fatti

Legacy, solo Non-Commercial, report del 20/10 (`cot.json` > `indici`), percentili su 160 settimane.
Skew = (long - short)/(long + short); soglia 0,30 nostra, dal "delta del 30%" del live.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto / long / short | skew fondo / var. | z52 var. |
|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 | 86.099 | 35.373 | 50.726 | +2.574 | +371 | +2.203 | 96,9 / 71,8 / 3,2 | +0,42 / +0,75 | +0,20 |
| S&P 500 | 215.885 | 358.556 | -142.671 | -8.919 | -7.243 | -1.676 | 29,4 / 7,1 / 57,1 | -0,25 / -0,10 | -0,06 |
| Dow x5 | 20.612 | 13.887 | 6.725 | +1.236 | +2.647 | -1.411 | 55,6 / 60,3 / 59,0 | +0,20 / -0,36 | -0,50 |
| Russell 2000 | 115.794 | 157.703 | -41.909 | +3.649 | -15.445 | +19.094 | 30,0 / 100 / 97,4 | -0,15 / +1,00 | +1,67 |

NASDAQ-100, le ultime quattro (`ultime_4`; nessuna in rollover, ne' per open interest ne' per calendario):

| report | netto | var. netto | skew | z52 | pct var. 3a |
|---|---|---|---|---|---|
| 20/10 | 50.726 | +2.203 | +0,748 | +0,20 | 23,1 |
| 13/10 | 48.523 | -9.600 | -1,00 | -0,86 | 71,2 |
| 06/10 | 58.123 | +6.876 | +1,00 | +0,58 | 59,0 |
| 29/09 | 51.247 | -4.903 | -0,53 | -0,47 | 44,9 |

La data sul prezzo (`prezzo_alle_date`, `settimane_dei_report`): NQZ6, cassa 09:30-16:00 New York
(15:30-22:00 italiane in quelle date), fasce da 1 punto. "Adesso dal POC" = 29.579,75 meno il POC.

| report | giorno: POC, valore, chiusura, delta | chiude | adesso dal POC | settimana: POC, valore, delta | adesso dal POC sett. |
|---|---|---|---|---|---|
| 20/10 | 29.757, 29.599-29.842, 29.688, -982 | dentro | -177,25 | 29.770, 29.584-30.178, -524 | -190,25 |
| 13/10 | 30.775, 30.712-30.800, 30.788,75, -270 | dentro | -1.195,25 | 30.890, 30.846-31.315, +14.648 | -1.310,25 |
| 06/10 | 31.530, 31.483-31.556, 31.480,75, -3.557 | sotto | -1.950,25 | 30.824, 30.529-31.213, -4.000 | -1.244,25 |
| 29/09 | 30.609, 30.557-30.650, 30.617,5, +1.378 | dentro | -1.029,25 | 30.566, 30.520-30.773, +19.877 | -986,25 |

Settimana 14-20/10: da 30.551,5 a un minimo di 29.268,75, chiusura 29.688. Nessun giorno mancante.

TFF del Nasdaq, 20/10 (`tff_nasdaq`), percentili a tre anni:

| categoria | netto | var. netto | z52 | pct netto / long / short |
|---|---|---|---|---|
| Dealer | -94.122 | -16.190 | -1,12 | 4,5 / 30,1 / 92,3 |
| Asset Manager | 77.790 | +2.467 | +0,44 | 59,6 / 71,8 / 57,1 |
| Leveraged Funds | 950 | +12.543 (short -8.900) | +1,01 | 93,6 / 52,6 / 1,3 |

## Cosa Dicono Per NQ

- **Passi 1-3** `[1 · 27:46]`, `[1 · 34:51]`: Legacy, solo Non-Commercial. Netto al 96,9 percentile,
  vicino al massimo di tre anni (58.123, il 6/10). **Il campo estremo e' lo short** (3,2 percentile),
  non il long (71,8). Skew di fondo +0,42: sbilanciato al rialzo.
- **Passo 4** `[1 · 30:01]`, `[1 · 33:12]`: l'ultima variazione e' piccola. Skew +0,75 sopra soglia, ma
  z +0,20 e 23 percentile: il piede e' appena appoggiato. La piu' grande delle quattro e' il 13/10:
  -9.600, skew saturo a -1 (long giu', short su), quindi conta lo z, -0,86 (71 percentile).
- **Passo 5** `[1 · 32:31]`: 2 su 4 concordi col Nasdaq. L'S&P quasi fermo (3 percentile); il Russell
  e' il movimento piu' forte, fatto di short ricoperti. La metrica cross-index **non ha retto** alla
  verifica (`analisi-istituzionale.md`): la matrice si riporta, non si pesa.
- **Passo 6** `[1 · 31:53]`: il 13/10 gli speculatori alleggerivano mentre la cassa chiudeva **dentro il
  valore, nella parte alta** (30.788,75 contro VAH 30.800), delta quasi piatto. La settimana coperta
  scende da 31.233,75 a 30.788,75. **Il livello del passo e' il POC 30.775.**
- **Osservazione** (derivazione nostra, non passo del corso: `il-framing-e-il-cot-si-leggono-insieme.md`):
  tutti i POC dei giorni e delle settimane dei report stanno sopra il prezzo. Il lato posizionato,
  lungo, sta sopra il cuore del volume.
- **TFF, con riserva** (Legacy e TFF non sono sinonimi, `analisi-istituzionale.md`): nella settimana al
  20/10 i Leveraged Funds ricoprono short (short al 1,3 percentile), i Dealer allargano il corto
  (z -1,12). Il +2.203 Legacy contiene movimenti opposti piu' grandi.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.775 (30.712-30.800) | POC del 13/10, la variazione piu' grande (-9.600) | posizionamento: dove gli speculatori hanno alleggerito; dice se li' il prezzo viene respinto, come nell'esempio del live |
| 30.890 (30.846-31.315) | POC della settimana 07-13/10 | lo stesso, sul profilo della settimana coperta |
| 29.757 (29.599-29.842) | POC e valore del 20/10, ultimo report | posizionamento piu' recente; il prezzo di adesso sta appena sotto il VAL |
| 29.770 (29.584-30.178) | POC della settimana 14-20/10 | il prezzo di adesso e' a ridosso del VAL settimanale |
| ven 30/10, 20:30 italiane | prossimo COT (dati del 27/10) | nuova data, nuovo livello; 20:30 e non 21:30: l'Italia e' in ora solare, New York non ancora |

## Cosa Non Sappiamo

- **Eta'**: 7 giorni; il COT non dice *quando*. **Rollover**: nessuna delle quattro settimane.
- **Nessuna soglia COT e' validata** qui: la convergenza framing-COT e' osservazione.
- Il prezzo di adesso e' fuori dalla cassa: il confronto con i valori di cassa e' fra sessioni diverse.
- lacuna del collettore: `dati/sedute-NQZ6.json` non c'e'; manca la scala dei POC fra una data e l'altra.
- lacuna del collettore: percentili di long e short assoluti per ciascuna delle `ultime_4`.
- lacuna del collettore: il profilo del venerdi' di pubblicazione, quando il dato diventa pubblico.

## Per Il Quadro

1. Netto Non-Commercial Nasdaq al 96,9 percentile, short al 3,2: coerente con il regime della macro, o
   un posizionamento rimasto indietro rispetto alla settimana al 20/10 (minimo 29.268,75 da 30.660,75)?
2. Livello del passo 6: **30.775** (13/10), e 30.890 della settimana: confrontarli con muri e zero gamma.
3. **29.599-29.757** (valore del 20/10) e VAL settimanale 29.584: il prezzo ci sta appena sotto;
   confrontarli con muri, zero gamma e il valore delle ultime sedute dell'order flow.
4. Cross-confirm 2 su 4, Russell con ricoperture forti (z +1,67): coerente con la rotazione che legge la macro?
