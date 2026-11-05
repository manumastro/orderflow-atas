# Fase 2 · Il COT · 2026-11-05 · ESZ6

**Stato**: fatto
**Dati**: `dati/cot.json` (CFTC alla fonte, Legacy futures only e TFF, raccolto alle 16:16 ora italiana del 5 novembre), `dati/sedute-ESZ6.json` (stessa raccolta), `dati/raccolta.json` (nessun errore). Report COT del **martedi' 27 ottobre**, pubblicato venerdi' 30: **9 giorni**. Prossimo: **venerdi' 6 novembre, 21:30 ora italiana**. Prezzo di adesso: ESZ6 7.565,00 alle 16:16 ora italiana (`prezzo_adesso`), 46 minuti dopo l'apertura della cassa.

**In una riga**: gli speculatori dell'S&P 500 sono corti in modo ordinario ma hanno pochissime posizioni lunghe; nell'ultima settimana misurata hanno ricomprato un po' di corti; tutti i posizionamenti delle ultime quattro settimane sono avvenuti fra 190 e 310 punti sopra il prezzo di adesso.

## In Parole Semplici

**Il punto**: chi specula sull'S&P 500 e' piu' corto che lungo, e da quando si e' posizionato il prezzo e' sceso molto. Chi era corto sta guadagnando; chi ha comprato a meta' ottobre sta perdendo quasi 300 punti.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: gli speculatori hanno piu' scommesse al ribasso che al rialzo, e le scommesse al rialzo sono fra le piu' basse degli ultimi tre anni.
   → Il mercato non e' pieno di compratori a leva: e' un dato di contesto, non dice cosa fara' il prezzo.
2. **L'ultima settimana misurata (fino al 27 ottobre)**: hanno chiuso circa 8.000 contratti al ribasso, senza aggiungerne al rialzo.
   → Un movimento piccolo, di quelli normali: nessuno ha premuto forte sull'acceleratore.
3. **Gli altri indici**: Dow e Russell si sono mossi come l'S&P, il Nasdaq al contrario.
   → Concordano tre su quattro; questa verifica fra indici, pero', non ha mai retto ai nostri test.
4. **Dove stava il prezzo**: nelle quattro date dei report il prezzo stava fra 7.758 e 7.880, e ogni giorno ha chiuso dentro la sua zona di scambio principale.
   → Oggi siamo sotto tutte quelle zone: chi ha comprato il 13 ottobre e' sott'acqua di quasi 300 punti.
5. **Dopo il report**: dal 28 ottobre il prezzo e' sceso, risalito e oggi sceso di nuovo; questa discesa nessun report la copre ancora.
   → Chi ha venduto o comprato nella discesa lo dira' il report di venerdi'.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.851 | il prezzo piu' scambiato del 13 ottobre, la settimana in cui gli speculatori si sono mossi di piu' | chi ha comprato quel giorno torna in pari |
| circa 7.755 | il prezzo piu' scambiato del 27 ottobre, quasi uguale al cuore delle ultime venti sedute | si torna dove si e' preso l'ultimo posizionamento misurato |
| circa 7.690 | il prezzo piu' scambiato della settimana 21-27 ottobre | si rientra nella settimana coperta dall'ultimo report |

**Quando**: venerdi' 6 novembre 21:30, nuovo report (posizioni al martedi' 3 novembre)
**Quanto e' provato**: i numeri sono misurati (CFTC e profili di ATAS); il legame fra queste posizioni e il prezzo e' una lettura del corso, e nessuno studio lo prova; il COT non dice mai *quando*.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi ha quali contratti; **speculatori** = la categoria Non-Commercial, fondi e gestori che cercano profitto; **corti / lunghi** = posizioni che guadagnano se il prezzo scende / sale; **netto** = lunghi meno corti; **zona di scambio principale** = l'area di valore, dove passa il 70% del volume della seduta.

## I Fatti

**Passi 1-2: Legacy, solo Non-Commercial** (`cot.json`, `indici`, report del 27/10; percentili su 160 settimane, circa tre anni)

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| **S&P 500 e-mini** | 215.924 | 350.292 | -134.368 | +39 | -8.264 | +8.303 | 34,4 | **7,7** | 51,9 |
| NASDAQ-100 mini | 84.567 | 35.728 | 48.839 | -1.532 | +355 | -1.887 | 95,0 | 64,7 | 3,8 |
| Dow x5 | 19.827 | 12.083 | 7.744 | -785 | -1.804 | +1.019 | 60,6 | 53,8 | 37,2 |
| Russell 2000 mini | 112.363 | 148.442 | -36.079 | -3.431 | -9.261 | +5.830 | 36,2 | 99,4 | 94,9 |

Open interest S&P (contratti aperti): 1.955.711 (+1,0%). Netto S&P a tre anni fra -239.827 e +113.440.

**Passi 3-4: skew e ultime quattro, S&P 500** (skew = (long - short) / (long + short); skew della variazione sulla stessa formula con le variazioni; soglia 0,30, **convenzione nostra**, traduzione del "30%" del live `[1 · 33:12]`; z52 = scarto della variazione netta in deviazioni standard su 52 settimane; pct var. = percentile del valore assoluto della variazione su tre anni)

| report | netto | var. long | var. short | var. netto | skew var. | z52 | pct var. | rollover |
|---|---|---|---|---|---|---|---|---|
| 27/10 | -134.368 | +39 | -8.264 | +8.303 | +1,00 (saturo) | 0,23 | 21,8 | no / no |
| 20/10 | -142.671 | -8.919 | -7.243 | -1.676 | -0,10 | -0,06 | 3,2 | no / no |
| **13/10** | -140.995 | +12.795 | -280 | **+13.075** | +1,00 (saturo) | **0,36** | **32,1** | no / no |
| 06/10 | -154.070 | +2.422 | +13.993 | -11.571 | -0,71 | -0,31 | 26,9 | no / no |

Skew di fondo al 27/10: **-0,24**. Rollover: le due regole (open interest e calendario) danno "no" in tutte e quattro.

**Passo 5: cross-confirm** rispetto all'S&P (`cross_confirm`): segni della variazione netta S&P +, Dow +, Russell +, Nasdaq -: **3 su 4**.

**Passo 6: la data sul prezzo** (`prezzo_alle_date`; cassa 09:30-16:00 New York = 15:30-22:00 italiane il 6, 13 e 20 ottobre, 14:30-21:00 il 27 ottobre; fasce da 0,25 punti; distanza dal prezzo di adesso 7.565,00)

| giorno | chiusura | POC | VAL - VAH | delta | chiude | adesso dal POC |
|---|---|---|---|---|---|---|
| 27/10 | 7.758,25 | 7.758,00 | 7.740,75 - 7.772,25 | +18.413 | dentro il valore | -193,0 |
| 20/10 | 7.759,00 | 7.754,50 | 7.743,25 - 7.774,50 | +3.910 | dentro il valore | -189,5 |
| **13/10** | 7.856,00 | **7.851,25** | 7.843,50 - 7.856,75 | -502 | dentro il valore | **-286,25** |
| 06/10 | 7.873,00 | 7.879,75 | 7.871,50 - 7.887,50 | +6.487 | dentro il valore | -314,75 |

POC = prezzo piu' scambiato; VAL e VAH = bordi basso e alto dell'area di valore; delta = aggressori in acquisto meno in vendita.

**Le settimane dei report** (`settimane_dei_report`, cassa, nessun giorno mancante, nessuna seduta prima del rollover)

| settimana | min - max | POC | VAL - VAH | delta | adesso dal POC |
|---|---|---|---|---|---|
| 21-27/10 | 7.648,50 - 7.812,25 | 7.690,00 | 7.649,75 - 7.750,00 | +45.650 | -125,0 |
| 14-20/10 | 7.676,00 - 7.837,75 | 7.754,50 | 7.733,00 - 7.790,50 | +30.233 | -189,5 |
| 07-13/10 | 7.783,00 - 7.870,50 | 7.855,00 | 7.826,75 - 7.863,00 | -31.195 | -290,0 |
| 30/09-06/10 | 7.672,75 - 7.897,50 | 7.775,00 | 7.692,25 - 7.818,25 | -17.668 | -210,0 |

Dopo il report (`sedute-ESZ6.json`, cassa, POC): 28/10 7.772,50 · 29/10 7.753,75 · 30/10 7.610,00 · 02/11 7.657,00 · 03/11 7.597,00 · 04/11 7.729,25. Oggi (parziale): apertura 7.707,50, minimo 7.547,75, delta -23.684. Composito di 20 sedute: POC 7.754,00, valore 7.667,75 - 7.818,75.

**TFF dell'S&P 500** (`tff_sp500`, 27/10; percentili a tre anni)

| categoria | netto | var. long | var. short | var. netto | z52 | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|
| Dealer | -639.593 | +9.108 | +11.770 | -2.662 | -0,03 | 68,6 | 76,3 | 38,5 |
| Asset Manager | 923.449 | +9.413 | -12.659 | +22.072 | 0,81 | 44,2 | 23,1 | 29,5 |
| Leveraged Funds | -400.604 | -32.955 | -3.596 | -29.359 | -0,62 | 27,6 | **0,6** | 50,0 |

## Cosa Dicono Per ES

- **Lo skew di fondo** (passo 3, `[1 · 30:01]` per la lettura long contro short): -0,24, piu' corti che lunghi ma sotto la soglia di 0,30. **Il campo estremo non e' il netto** (34° percentile, ordinario) ma il **long al 7,7° percentile**: pochi speculatori lunghi rispetto agli ultimi tre anni (regola del campo giusto in `il-framing-e-il-cot-si-leggono-insieme.md`, "Il COT, con le tre dichiarazioni").
- **L'ultima variazione** (passo 4, `[1 · 30:01]`): +8.303 di netto fatto quasi solo di corti chiusi. Lo skew satura a +1 perche' le due variazioni hanno segno opposto, quindi conta lo z: 0,23, percentile 21,8. **Flusso ordinario**. Nelle quattro settimane i segni si alternano: nessuna direzione di flusso sostenuta.
- **Il cross-confirm** (passo 5, `[1 · 32:31]`): 3 su 4, il Nasdaq contro. Riserva: la metrica cross-index **non ha retto** alla verifica (`analisi-istituzionale.md`, "La Metrica Cross-Index E Il Suo Esito").
- **La data sul prezzo** (passo 6, `[1 · 31:53]`): la variazione piu' grande delle quattro e' quella del **13 ottobre** (+13.075, quasi tutta acquisti nuovi), in una seduta stretta (13 punti di valore) chiusa dentro il valore, a 7.851,25. Il 27 ottobre i corti si chiudono in una seduta chiusa dentro il valore a 7.758, con delta +18.413. Oggi il prezzo sta **sotto tutte e quattro le aree di valore e sotto il minimo della settimana 21-27 ottobre** (7.648,50).
- **La domanda finale** (nostra derivazione, non passo del corso): il lato posizionato, gli speculatori corti, ha il cuore del volume delle quattro settimane **sopra** il prezzo: e' in guadagno. Gli acquisti del 13 ottobre sono sotto di circa 286 punti. **Osservazione**, nessuna soglia COT e' validata qui.
- **TFF, complemento**: i Leveraged Funds hanno tagliato 33.000 lunghi e il loro long e' al minimo di tre anni (0,6° percentile), coerente con il long Legacy basso. Ma nella stessa settimana la Legacy segna corti chiusi e la TFF lunghi tagliati: **le categorie non coincidono** (Non-Commercial esiste solo in Legacy, `i-tre-modelli-del-live-q1.md` sezione 9). Gli Asset Manager aggiungono +22.072, z 0,81: ordinario.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| **7.851,25** (valore 7.843,50 - 7.856,75) | POC della cassa del 13/10, report con la variazione Legacy piu' grande delle quattro | posizionamento: il prezzo a cui gli acquisti del 13 ottobre tornano in pari; e' il livello del passo 6 |
| 7.758,00 (valore 7.740,75 - 7.772,25) | POC della cassa del 27/10, data dell'ultimo report; a 4 punti dal POC del composito di 20 sedute (7.754,00) | posizionamento: dove i corti si sono chiusi; rientrarvi rimette il prezzo nel cuore del mese |
| 7.690,00 (valore 7.649,75 - 7.750,00) | POC della settimana 21-27/10, coperta dall'ultimo report | il primo valore settimanale sopra il prezzo; rientrarvi riporta il prezzo nella settimana misurata |
| ven 6/11 21:30 | prossima pubblicazione CFTC, posizioni al 3/11 | dira' chi si e' mosso nella discesa dal 28/10 al 3/11 |

## Cosa Non Sappiamo

- **L'eta' del dato**: 9 giorni. Dopo il 27/10 il prezzo e' salito, sceso di circa 200 punti e oggi e' sceso di nuovo; il COT non copre niente di questo.
- **Il COT non dice quando**, ne' a che prezzo dentro la settimana: la data e' un martedi', il posizionamento puo' essere avvenuto in qualunque giorno dal mercoledi' prima.
- **Il prezzo di adesso si muove in fretta**: 7.565,00 alle 16:16 in `cot.json`, ultimo 7.556,00 in `sedute-ESZ6.json`, minimo 7.547,75. Le distanze valgono a quell'ora.
- **La soglia 0,30 e la domanda finale sono nostre**; nessuna soglia COT e' validata.
- Lacuna del collettore: il profilo di cassa aggregato della settimana **non ancora coperta** (28/10 - 3/11), quella del prossimo report; ci sono solo le sedute singole.
- Lacuna del collettore: lo skew di fondo e i percentili di long e short per ciascuna delle ultime quattro settimane (c'e' solo il valore al 27/10).

## Per Il Quadro

1. **Speculatori corti, long al minimo di tre anni (7,7° percentile; Leveraged Funds TFF 0,6°)**: coerente con il regime della macro? Un mercato con pochi lunghi a leva va confrontato con il rischio e i tassi della fase 1.
2. **7.851,25 (13/10) e 7.758 (27/10)**: da confrontare con muri, zero gamma e livelli X-Ray della fase 3, in prezzi di ES.
3. **Il prezzo sta sotto tutte le aree di valore delle date COT e sotto la settimana 21-27/10**: da mettere accanto alla scala dei POC e al valore di oggi della fase 4.
4. **Il report di venerdi' 21:30** arriva dentro la finestra della lettura: e' il fatto che puo' cambiare questa fase.
