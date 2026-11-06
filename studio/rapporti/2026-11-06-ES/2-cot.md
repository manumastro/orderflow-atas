# Fase 2 · Il COT · 2026-11-06 (ESZ6)

**Stato**: fatto, sul report con i dati di martedi' 27 ottobre. Il nuovo (dati di martedi' 3 novembre) esce stasera alle 21:30.
**Dati**: `dati/cot.json` (CFTC alla fonte, Legacy futures only e TFF) e `dati/sedute-ESZ6.json`, raccolti alle 15:05 ora italiana (`raccolta.json`). COT al 27 ottobre: 10 giorni. Prezzo di adesso ESZ6 7.388 alle 15:05. Errori della raccolta: nessuno.

**In una riga**: gli speculatori sull'S&P 500 sono netti al ribasso con pochissimi contratti al rialzo, l'ultima settimana misurata e' ordinaria, e il prezzo sta 370-460 punti sotto i valori delle date dei report.

## In Parole Semplici

**Il punto**: gli speculatori sull'S&P 500 tengono piu' scommesse al ribasso che al rialzo, e quelle al rialzo sono poche come quasi mai in tre anni. L'ultimo report mostra solo un piccolo movimento. Da allora il prezzo e' sceso di circa 370 punti, e nessun report l'ha ancora visto.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: circa 350 mila contratti al ribasso contro 216 mila al rialzo.
   → il lato carico e' quello che guadagna se il prezzo scende; il rialzo e' quasi vuoto.
2. **L'ultima settimana misurata** (fino al 27 ottobre): hanno chiuso circa 8 mila scommesse al ribasso, e non ne hanno aperte al rialzo.
   → un movimento piccolo e normale: non dice che stanno cambiando idea.
3. **Gli altri indici**: Dow e Russell si muovono nello stesso verso, il Nasdaq al contrario.
   → nessun accordo pieno; e misurato sul passato, questo incrocio non ha mai anticipato il prezzo.
4. **La data sul prezzo**: la settimana in cui si sono mossi di piu' (al 13 ottobre, circa 13 mila contratti in piu' al rialzo), l'S&P 500 stava intorno a 7.850.
   → oggi e' circa 460 punti sotto: chi ha comprato li', se tiene ancora, perde.
5. **Adesso**: 7.388, circa 370 punti sotto la zona piu' scambiata delle ultime quattro settimane (circa 7.755).
   → chi e' netto al ribasso sta in guadagno; il crollo del 5 novembre non entra nemmeno nel report di stasera.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.850 | dove si e' scambiato di piu' il 13 ottobre, la settimana in cui gli speculatori hanno aggiunto di piu' al rialzo | e' un ritorno sul loro prezzo d'acquisto |
| circa 7.755 | dove si e' scambiato di piu' il 27 ottobre (data dell'ultimo report) e nelle ultime 20 sedute | e' il prezzo dove chi era al ribasso ha ridotto; li' quel guadagno si azzera |
| circa 7.600 | dove si e' scambiato di piu' martedi' 3 novembre, la data del report di stasera | diventa il prezzo da leggere se stasera il report mostra un movimento grande |

**Quando**: oggi 21:30 il report della CFTC con i dati di martedi' 3 novembre.
**Cambia idea se**: stasera gli speculatori risultano aver aggiunto molte scommesse al rialzo, molto piu' del solito.
**Quanto e' provato**: i numeri sono misurati dalla CFTC. Che i prezzi delle date contino e' una lettura del corso, vista su pochi casi: nessuno studio la prova su ES, e il report non dice quando il prezzo reagisce.

**Le parole**: **COT** = il report settimanale della CFTC (l'autorita' dei futures americani) su chi tiene quali contratti; **speculatori** = fondi e gestori che non usano i futures per coprire un'attivita' (nel report, "Non-Commercial"); **netto** = contratti al rialzo meno al ribasso; **valore** = la fascia di prezzi dove passa il 70% del volume di una seduta.

## I Fatti

**Legacy, solo Non-Commercial**, report del 27/10 (fonte `cot.json`; pct = percentile su 156 settimane; var. = rispetto al 20/10).

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| S&P 500 e-mini | 215.924 | 350.292 | -134.368 | +39 | -8.264 | +8.303 | 34,4 | **7,7** | 51,9 |
| NASDAQ-100 mini | 84.567 | 35.728 | 48.839 | -1.532 | +355 | -1.887 | 95,0 | 64,7 | 3,8 |
| Dow x5 | 19.827 | 12.083 | 7.744 | -785 | -1.804 | +1.019 | 60,6 | 53,8 | 37,2 |
| Russell 2000 mini | 112.363 | 148.442 | -36.079 | -3.431 | -9.261 | +5.830 | 36,2 | 99,4 | 94,9 |

| indice | skew di fondo | skew variazione | z variazione (52 sett.) | pct della variazione | open interest |
|---|---|---|---|---|---|
| S&P 500 e-mini | -0,24 | +1,00 (satura) | +0,23 | 21,8 | +1,0% |
| NASDAQ-100 mini | +0,41 | -1,00 (satura) | -0,17 | 21,2 | -1,5% |
| Dow x5 | +0,24 | +0,39 | +0,18 | 25,0 | +1,8% |
| Russell 2000 mini | -0,14 | +0,46 | +0,46 | 41,0 | -1,7% |

Convenzioni: skew = (var. long - var. short) / (|var. long| + |var. short|), soglia di "skew forte" 0,30 (convenzione nostra, dal "delta del 30%" del live); satura a +-1 quando le due variazioni hanno segno opposto, e allora conta lo z. Commercial netto S&P: +17.301.

**S&P 500, le ultime quattro date e il prezzo** (ESZ6, cassa 09:30-16:00 di New York: 15:30-22:00 ora italiana fino al 20/10, 14:30-21:00 il 27/10; fasce da 0,25; ritardo = adesso 7.388 meno il POC della seduta). Nessuna delle quattro e' settimana di rollover, in nessuna delle due regole.

| report | var. long | var. short | var. netto | skew | z | pct | valore (VAL-VAH) | POC | delta seduta | ritardo |
|---|---|---|---|---|---|---|---|---|---|---|
| 27/10 | +39 | -8.264 | +8.303 | +1,00 | +0,23 | 21,8 | 7.740,75-7.772,25 | 7.758 | +18.413 | -370 |
| 20/10 | -8.919 | -7.243 | -1.676 | -0,10 | -0,06 | 3,2 | 7.743,25-7.774,5 | 7.754,5 | +3.910 | -366,5 |
| 13/10 | +12.795 | -280 | **+13.075** | +1,00 | +0,36 | 32,1 | 7.843,5-7.856,75 | 7.851,25 | -502 | -463,25 |
| 06/10 | +2.422 | +13.993 | -11.571 | -0,71 | -0,31 | 26,9 | 7.871,5-7.887,5 | 7.879,75 | +6.487 | -491,75 |

Tutte e quattro le sedute chiudono dentro il proprio valore. **Le settimane coperte** (cassa, dal mercoledi' al martedi'; nessun giorno mancante):

| settimana | POC | valore | delta | chiusura | adesso dal POC |
|---|---|---|---|---|---|
| 21-27/10 | 7.690 | 7.649,75-7.750 | +45.650 | 7.758,25 | -302 |
| 14-20/10 | 7.754,5 | 7.733-7.790,5 | +30.233 | 7.759 | -366,5 |
| 07-13/10 | 7.855 | 7.826,75-7.863 | **-31.195** | 7.856 | -467 |
| 30/09-06/10 | 7.775 | 7.692,25-7.818,25 | -17.668 | 7.873 | -387 |

Composito di cassa a 20 sedute (dal 9/10, `sedute-ESZ6.json`): POC 7.754, valore 7.640,25-7.810,75; il prezzo e' in discount. La seduta del 5/11: da 7.707,5 a 7.381,25. Martedi' 3/11: POC 7.597, valore 7.589,75-7.609,25.

**TFF S&P 500** (stesso report; pct del netto e dei lati su 156 settimane):

| categoria | netto | var. long | var. short | var. netto | z | pct var. | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| Dealer | -639.593 | +9.108 | +11.770 | -2.662 | -0,03 | 5,8 | 68,6 | 76,3 | 38,5 |
| Asset Manager | +923.449 | +9.413 | -12.659 | +22.072 | +0,81 | 61,5 | 44,2 | 23,1 | 29,5 |
| Leveraged Funds | -400.604 | -32.955 | -3.596 | -29.359 | -0,62 | 48,7 | 27,6 | **0,6** | 50,0 |

TFF Nasdaq, per il confronto: Asset Manager var. netto -12.409, **z -2,06, pct 97,4**; Leveraged Funds short al percentile 0,6.

## Cosa Dicono Per ES

1. **Legacy, solo Non-Commercial** `[1 · 27:46]`, `[1 · 34:51]`: gli speculatori. **Lo skew di fondo** e' -0,24: piu' short che long. Il netto (pct 34,4) e' ordinario; **il campo estremo e' il long, al percentile 7,7** (il documento del framing: il netto e' quasi sempre il meno informativo).
2. **L'ultima variazione** `[1 · 30:01]`: +8.303, quasi tutta chiusura di short (-8.264) con il long fermo (+39). Lo skew satura a +1, quindi conta lo z: +0,23, variazione al percentile 21,8. **Flusso ordinario, livello ordinario**: nessuno ha "messo il piede sull'acceleratore" in modo notevole.
3. **Il cross-confirm** `[1 · 32:31]`: per segno 3 su 4 concordi con l'S&P (Nasdaq contrario). Criterio A (skew >= 0,30, nessuno contrario): no, il Nasdaq e' a -1,00. Criterio B (|z| >= 1): nessun indice. E il criterio A, misurato, non separa nulla (`analisi-istituzionale.md`, la metrica cross-index non ha retto): e' contesto, non conferma.
4. **La data sul prezzo** `[1 · 31:53]`: la variazione piu' grande delle quattro e' il 13/10, +13.075, quasi tutta long nuovi (+12.795). **Il livello del passo e' il valore del 13/10, 7.843,5-7.856,75, POC 7.851,25**. Quella settimana il delta di cassa e' -31.195: **osservazione**, gli speculatori aggiungevano long in una settimana dominata dai venditori aggressivi, come l'oro dell'8 settembre (`il-framing-e-il-cot-si-leggono-insieme.md`). Anche il massimo e' ordinario (pct 32,1, z +0,36): e' il piu' grande di quattro movimenti piccoli.
5. **Il 6/10**, seconda variazione (-11.571, quasi tutta short nuovi, +13.993), cade sul valore piu' alto del periodo, 7.871,5-7.887,5, sotto il massimo di 7.897,5. **Osservazione**: e' la forma del 13 gennaio del live (distribuzione sul bordo alto del valore), e il prezzo poi l'ha lasciato. Una sola occorrenza.
6. **La domanda del framing**: il lato netto (short, Legacy) sta sopra o sotto il cuore del volume? Il POC del composito a 20 sedute e' 7.754, il prezzo 7.388: **il cuore del volume sta 366 punti sopra il prezzo**. Nella lettura del documento, il lato netto short e' in guadagno; i long del 13/10 sono 463 punti sotto.
7. **La TFF**, complemento, con la riserva che Legacy e TFF non sono sinonimi (`analisi-istituzionale.md`). Il 27/10 gli Asset Manager aggiungono +22.072 e i Leveraged Funds tolgono -29.359, quasi tutto chiudendo long (-32.955). Il long dei Leveraged Funds e' al percentile 0,6: coerente con il long Non-Commercial al 7,7. Sul Nasdaq, nello stesso report, gli Asset Manager tagliano con un flusso notevole (z -2,06): **i due indici divergono**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.843,5-7.856,75, POC 7.851,25 | valore del 13/10, la variazione Non-Commercial piu' grande delle ultime quattro (+13.075) | posizionamento: un ritorno li' e' un ritorno sul prezzo dei long aggiunti quella settimana |
| 7.871,5-7.887,5, POC 7.879,75 | valore del 6/10, short nuovi +13.993 sul valore piu' alto del periodo | posizionamento: il prezzo dove si sono aggiunti gli short; sopra, quel lato va sotto |
| 7.740,75-7.772,25, POC 7.758 | valore del 27/10, data dell'ultimo report; coincide con il POC del composito, 7.754 | posizionamento: dove gli short hanno ridotto; e' anche il cuore del volume |
| 7.589,75-7.609,25, POC 7.597 | valore di martedi' 3/11, la data del report di stasera | da rileggere alle 21:30: diventa il livello se il nuovo report mostra un movimento grande |
| oggi 21:30 | report CFTC con i dati del 3/11 (15:30 New York) | aggiorna i passi 4-6; il 5/11 entra nel report del 13/11 (dati del 10/11) |

## Cosa Non Sappiamo

- **L'eta'**: il dato e' di 10 giorni e non vede la seduta del 5/11 (circa -330 punti). Nemmeno il report di stasera la vede.
- **Il COT non dice quando** `[1 · 30:58]`, e non dice se i long del 13/10 sono ancora aperti: il netto settimanale e' un saldo, non un registro dei singoli contratti.
- **Nessuna soglia COT e' validata** su questo repository; la data come livello e' stata osservata una volta (oro, 8/9) e nel live. La soglia di skew 0,30 e' convenzione nostra.
- **Perche' il Non-Commercial S&P e' netto short** mentre gli Asset Manager sono netti long di 923 mila non e' misurato qui; le due viste non si sommano.
- **Il cuore del volume** e' il composito a 20 sedute: lo storico di ESZ6 nei dati comincia il 2/10, e il composito "a 90 giorni" del framing ha solo 25 sedute.
- lacuna del collettore: `prima_del_rollover` non c'e' in `settimane_dei_report` (la skill lo prevede); dalle date, le settimane stanno tutte dopo il rollover di settembre, ma il campo non lo conferma.
- lacuna del collettore: lo z52 del **livello** del netto (convenzione di `analisi-istituzionale.md`) non c'e'; c'e' solo lo z della variazione. Il livello si legge solo dal percentile.
- lacuna del collettore: il percentile della variazione e' su tre anni (160 settimane in serie), non sulle 521 del documento: non e' confrontabile con i numeri storici di quel documento.

## Per Il Quadro

1. **Posizionamento contro il regime**: Non-Commercial S&P netto short (pct 34,4) con il long al 7,7 e i Leveraged Funds long allo 0,6. E' coerente con il regime della macro, o il calo del 5/11 ha trovato un rialzo gia' quasi vuoto?
2. **I livelli del passo 6 contro le opzioni**: 7.851 (13/10), 7.880 (6/10), 7.755 (27/10 e composito). Coincidono con muri o con lo zero gamma di X-Ray in prezzi di ES?
3. **Il prezzo e' sotto tutte le date**: 7.388 sta 370-490 punti sotto i valori dei quattro report, e vicino al valore del 5/11 (VAL 7.388,25, POC 7.406). La fase 4 dice se quel valore regge; il COT non ci arriva.
4. **Divergenza fra indici**: sul Nasdaq gli Asset Manager tagliano forte (z -2,06), sull'S&P aggiungono (z +0,81). Da confrontare con la correlazione NQ-ES e con il report di stasera.
