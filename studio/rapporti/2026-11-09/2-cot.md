# Fase 2 · Il COT · 2026-11-09 (NQZ6 ed ESZ6, studio congiunto, pre-apertura)

**Stato**: fatto (aggiorna `precedenti/1452/2-cot.md` delle 12:28: stesso report CFTC, cambiano le distanze dal prezzo)
**Dati**: `dati/cot.json` (principale NASDAQ-100 mini, prezzi di NQZ6) e `dati-ES/cot.json` (principale S&P 500
e-mini, prezzi di ESZ6): CFTC alla fonte, Legacy futures only e TFF; poi `sedute-NQZ6.json` e `sedute-ESZ6.json`.
`raccolto_utc` 13:52Z in tutti e due, cioe' **14:52 ora italiana**. Report in uso: dati di **martedi' 3 novembre**,
pubblicato venerdi' 6/11 alle 21:30 ora italiana, eta' 6 giorni. Prezzi di adesso (`prezzo_adesso`, 14:52, prima
della cassa delle 15:30): **NQZ6 27.344,25**, **ESZ6 7.470,25** (alle 12:28: 27.204 e 7.449,5; NQ +140, ES +20,75).

**In una riga**: speculatori carichi al rialzo sul Nasdaq e al ribasso sull'S&P, corti che riaprono sul Nasdaq,
banche che vendono su tutti e due; il prezzo ha recuperato un poco ma resta ben sotto i prezzi del report.

## In Parole Semplici

**Il punto**: la fotografia (martedi' 3 novembre) mostra due indici con posizioni opposte. Sul Nasdaq gli speculatori
sono carichi al rialzo, sull'S&P al ribasso: lo stesso calo pesa sui primi e premia i secondi. Sul Nasdaq tornano le
scommesse al ribasso, e su entrambi le banche hanno venduto molto. Il rimbalzo di stamattina non cambia il quadro.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: sul Nasdaq gli speculatori hanno quasi il doppio di contratti al rialzo che al ribasso; sull'S&P circa 359 mila al ribasso contro 229 mila al rialzo.
   → Due lati carichi opposti: chi e' carico sul Nasdaq perde se il prezzo scende, chi e' carico sull'S&P guadagna.
2. **L'ultima settimana misurata** (fino al 3/11): sul Nasdaq gli speculatori aprono circa 10 mila contratti al ribasso. Sull'S&P aggiungono un po' da entrambi i lati.
   → Sul Nasdaq chi spinge e' chi vende, con forza normale; sull'S&P nessuno spinge.
3. **Le banche** (un'altra classificazione dello stesso rapporto): vendono su tutti e due; sul Nasdaq sono al ribasso come mai in tre anni.
   → Qui i due indici concordano. Dall'altra parte ci sono i fondi, che comprano.
4. **La data sul prezzo**: martedi' 3/11 il Nasdaq stava intorno a 28.300 e l'S&P intorno a 7.600, tutti e due in basso nella loro settimana.
   → Le posizioni nuove sono state prese dopo una settimana di discesa, non in cima.
5. **Adesso**: Nasdaq circa 27.340, circa 970 punti sotto (-3,4%); S&P circa 7.470, circa 127 punti sotto (-1,7%).
   → Dal 3/11 il Nasdaq ha perso circa il doppio dell'S&P anche dopo il rimbalzo; il crollo del 5/11 entra solo nel report di venerdi' 13.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.300 sul Nasdaq, circa 7.600 sull'S&P | dove si e' scambiato di piu' martedi' 3/11, la data del report e della vendita piu' grande delle banche | torna dove si sono prese le posizioni di quella settimana; oggi circa 970 e 127 punti sopra |
| circa 30.775 sul Nasdaq, circa 7.850 sull'S&P | il 13 ottobre, il movimento piu' grande degli speculatori in quattro settimane: sul Nasdaq riducevano, sull'S&P compravano | riferimento per le settimane: oggi circa 3.430 e 380 punti sopra |

**Quando**: venerdi' 13/11 alle 21:30 ora italiana, il report con i dati di martedi' 10/11 (vedra' il crollo del 5/11).
**Cambia idea se**: venerdi' gli speculatori sul Nasdaq risultano aver tolto molto rialzo, o le banche ricomprano su
tutti e due gli indici.
**Quanto e' provato**: i numeri sono misurati dalla CFTC. Prevedere la direzione con il COT non ha retto nelle nostre
verifiche; la data che diventa un prezzo e' la procedura del corso, vista su pochi casi e mai contata su una serie.

**Le parole**: **COT** = il rapporto settimanale della CFTC (l'autorita' dei futures americani) su chi tiene i
contratti; **speculatori** = i "Non-Commercial"; **banche** = i "Dealer", chi fa mercato; **fondi** = gestori ("Asset
Manager") e fondi speculativi ("Leveraged Funds"); **valore** = la fascia dove passa il 70% del volume di una seduta;
**POC** = il prezzo con piu' volume.

## I Fatti

**Passi 1-2: Legacy `[1 · 27:46]`, solo Non-Commercial `[1 · 34:51]`**, futures only, report del 3/11, variazioni sul
27/10 (`cot.json`, `indici`, identico nei due file, invariato dalle 12:28). Percentili su tre anni (160 settimane).

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short | open interest |
|---|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 mini | 88.899 | 46.026 | 42.873 | +4.332 | +10.298 | -5.966 | 90,0 | 78,8 | 14,7 | 292.587 (+9,0%) |
| S&P 500 e-mini | 229.322 | 358.736 | -129.414 | +13.398 | +8.444 | +4.954 | 36,2 | 22,4 | 57,1 | 1.986.532 (+1,6%) |
| Dow x5 | 20.024 | 9.670 | 10.354 | +197 | -2.413 | +2.610 | 69,4 | 56,4 | 16,0 | 87.410 (-4,6%) |
| Russell 2000 mini | 109.685 | 133.339 | -23.654 | -2.678 | -15.103 | +12.425 | 51,2 | 98,1 | 89,7 | 449.399 (-2,1%) |

Contro il report del 27/10 (rapporti del 6/11): Nasdaq pct netto 95,0 → 90,0, **pct short 3,8 → 14,7**; S&P pct netto
34,4 → 36,2, **pct long 7,7 → 22,4**. Commercial netto: Nasdaq -63.270, S&P +5.195.

**Passi 3-5.** Skew di fondo = (long - short) / (long + short). Skew della variazione = (var. long - var. short) /
(|var. long| + |var. short|), soglia **0,30, convenzione nostra** dal "delta del 30%" `[1 · 33:12]`; satura a +-1 con
variazioni di segno opposto, e allora conta lo z a 52 settimane `[1 · 30:01]`.

| indice | skew di fondo | skew var. | piede sull'acceleratore | z52 var. | pct var. 3a | segno var. netto |
|---|---|---|---|---|---|---|
| NASDAQ-100 mini | +0,318 | -0,408 | venditori | -0,53 | 53,2 | -1 |
| S&P 500 e-mini | -0,220 | +0,227 | nessuno (sotto 0,30) | +0,13 | 12,2 | +1 |
| Dow x5 | +0,349 | +1,000 (satura) | compratori | +0,62 | 58,3 | +1 |
| Russell 2000 mini | -0,097 | +0,699 | compratori | **+1,02** | 75,0 | +1 |

Cross-confirm `[1 · 32:31]`: rispetto al Nasdaq **1 su 4** concordi, rispetto all'S&P **3 su 4**; seconda settimana
con il Nasdaq unico in riduzione.

**Passo 6, le quattro date sul prezzo** `[1 · 31:53]` (`prezzo_alle_date`, `settimane_dei_report`). Cassa 09:30-16:00
New York convertita per data: 15:30-22:00 ora italiana il 13 e 20/10 e il 3/11, 14:30-21:00 il 27/10. Fasce da 1
punto (NQ) e 0,25 (ES). Settimana = dal mercoledi' dopo il report precedente al martedi'. Nessuna seduta mancante.
"Adesso" = `ritardo_in_prezzo.dal_poc`, prezzo delle 14:52 meno il POC della seduta.

| NQZ6 | var. netto NC | z52 | pct var. | seduta VAL-POC-VAH | chiude | delta seduta | settimana VAL-POC-VAH | delta sett. | adesso |
|---|---|---|---|---|---|---|---|---|---|
| 3/11 | -5.966 | -0,53 | 53,2 | 28.296-28.314-28.387 | dentro, 28.301 | -5.843 | 28.102-28.314-29.494 | -7.691 | **-969,75** |
| 27/10 | -1.887 | -0,17 | 21,2 | 29.637-29.805-29.880 | dentro, 29.813 | -565 | 29.026-29.500-29.679 | -5.567 | -2.460,75 |
| 20/10 | +2.203 | +0,20 | 23,1 | 29.599-29.757-29.842 | dentro, 29.688 | -982 | 29.584-29.770-30.178 | -524 | -2.412,75 |
| 13/10 | **-9.600** | -0,86 | **71,2** | 30.712-30.775-30.800 | dentro, 30.789 | -270 | 30.846-30.890-31.315 | +14.648 | -3.430,75 |

| ESZ6 | var. netto NC | z52 | pct var. | seduta VAL-POC-VAH | chiude | delta seduta | settimana VAL-POC-VAH | delta sett. | adesso |
|---|---|---|---|---|---|---|---|---|---|
| 3/11 | +4.954 | +0,13 | 12,2 | 7.589,75-7.597-7.609,25 | dentro, 7.596,25 | -3.521 | 7.610,25-7.753,75-7.794,25 | +36.602 | **-126,75** |
| 27/10 | +8.303 | +0,23 | 21,8 | 7.740,75-7.758-7.772,25 | dentro, 7.758,25 | +18.413 | 7.649,75-7.690-7.750 | +45.650 | -287,75 |
| 20/10 | -1.676 | -0,06 | 3,2 | 7.743,25-7.754,5-7.774,5 | dentro, 7.759 | +3.910 | 7.733-7.754,5-7.790,5 | +30.233 | -284,25 |
| 13/10 | **+13.075** | +0,36 | **32,1** | 7.843,5-7.851,25-7.856,75 | dentro, 7.856 | -502 | 7.826,75-7.855-7.863 | -31.195 | -381 |

Ritardo dalla chiusura del 3/11 (`ritardo_in_prezzo`, 14:52): **NQ -956,75 punti (-3,38%), ES -126 punti (-1,66%)**;
alle 12:28 erano -3,88% e -1,93%. Dal POC della settimana del 3/11 (`prezzo_adesso_dal_poc`): NQ -969,75, ES -283,5.
**Dopo il report** (`sedute-*.json`, POC di cassa): NQ 4/11 28.632, 5/11 26.900, 6/11 27.175; ES 4/11 7.729,25, 5/11
7.406, 6/11 7.426. Il prezzo delle 14:52 sta sopra i POC del 5 e del 6/11 in tutti e due. Composito di cassa a 20
sedute (rapporto delle 12:28): NQ valore 28.686-31.048, ES 7.631-7.810,75; tutti e due i prezzi restano sotto il VAL.

**TFF, complemento** (`tff_nasdaq`, `tff_sp500`, report del 3/11; pct su tre anni):

| indice | categoria | netto | var. long | var. short | var. netto | z52 | pct var. | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|---|
| Nasdaq | Dealer | -112.529 | -1.822 | +25.193 | **-27.015** | **-1,84** | **95,5** | **0,6** | 31,4 | **100,0** |
| Nasdaq | Asset Manager | 73.137 | +3.450 | -4.306 | +7.756 | **+1,29** | 82,7 | 44,2 | 69,2 | 76,3 |
| Nasdaq | Leveraged Funds | 10.073 | +11.527 | +3.755 | +7.772 | +0,63 | 54,5 | 98,1 | 87,2 | 3,2 |
| S&P | Dealer | -688.574 | -11.429 | +37.552 | **-48.981** | **-1,05** | 82,7 | 52,6 | 73,1 | 56,4 |
| S&P | Asset Manager | 928.584 | +666 | -4.469 | +5.135 | +0,17 | 12,8 | 47,4 | 23,7 | 22,4 |
| S&P | Leveraged Funds | -372.318 | +23.190 | -5.096 | +28.286 | +0,55 | 48,7 | 35,9 | 13,5 | 44,9 |

## Cosa Dicono Per NQ E ES

1. **Il livello** (`il-framing-e-il-cot-si-leggono-insieme.md`: si guarda il campo estremo). Nasdaq: netto al 90°; lo
   short al 3,8° del 27/10 **ne e' uscito** (14,7°). Il lato affollato resta il rialzo. S&P: nessun campo agli estremi.
2. **Il flusso** `[1 · 30:01]`: Nasdaq, piede dei **venditori** (skew -0,41, z -0,53: ordinario; open interest +9,0%,
   posizioni aperte). S&P: skew +0,23 sotto soglia, z +0,13: **nessuno** sull'acceleratore.
3. **Il cross-confirm** `[1 · 32:31]`: nella Legacy i due principali **divergono in segno** per la seconda settimana.
   La metrica cross-index **non ha retto** alla verifica (`analisi-istituzionale.md`): si scrive com'e'.
4. **Il livello del passo 6, Legacy**: il movimento piu' grande delle quattro date e' il **13/10** in tutti e due, di
   segno opposto (Nasdaq riduce -9.600, S&P aggiunge +13.075), e in tutti e due contro il delta della settimana.
   Livelli: **NQ 30.712-30.800, POC 30.775; ES 7.843,5-7.856,75, POC 7.851,25.**
5. **La data nuova, il 3/11**: tutti e due in basso nella settimana. NQ: POC della seduta = POC della settimana
   (28.314). ES: il valore della seduta sta sotto il VAL della settimana, con delta settimanale +36.602.
   **Osservazione**: sul Nasdaq gli speculatori aprono corti in basso dopo una discesa, non sul bordo alto.
6. **Comodo o scomodo** (la domanda finale del framing): i valori delle quattro date restano tutti sopra il prezzo, NQ
   da 970 a 3.431 punti, ES da 127 a 381. Il rimbalzo di oggi accorcia la distanza dal 3/11 (NQ da 1.110 a 970, ES
   da 147,5 a 126,75) ma non cambia il lato: il netto Nasdaq (rialzo) e' sotto, il netto S&P (ribasso) e' in
   guadagno. **Stesso calo, effetto opposto.** Modello di lettura, non provato.
7. **La TFF, che non e' un sinonimo** (`analisi-istituzionale.md`): sul Nasdaq Legacy "speculatori in riduzione",
   TFF gestori +7.756 e fondi speculativi +7.772, segni opposti nello stesso report. **Il movimento notevole e' delle
   banche, e i due indici concordano**: Dealer Nasdaq -27.015 (z -1,84, corti al massimo di tre anni), Dealer S&P
   -48.981 (z -1,05), il piu' grande delle quattro date in tutti e due, il 3/11. Il perche' il rapporto non lo dice.
8. **Il COT non dice quando** `[1 · 30:58]`: la seduta del 5/11 entra nel report di venerdi' 13/11.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 28.296-28.387, POC 28.314 | valore del 3/11: NC -5.966, Dealer TFF -27.015 (z -1,84); = POC della settimana | ritorno dove si sono aperti i corti speculativi e le vendite delle banche; alle 14:52 +969,75 |
| ES 7.589,75-7.609,25, POC 7.597 | valore del 3/11: Dealer TFF -48.981 (z -1,05), il piu' grande dei quattro; sotto il VAL della settimana | ritorno sul prezzo delle vendite delle banche; alle 14:52 +126,75 |
| NQ 30.712-30.800, POC 30.775 | valore del 13/10, Legacy, NC -9.600, il movimento piu' grande dei quattro | scala delle settimane: dove gli speculatori hanno ridotto; alle 14:52 +3.430,75 |
| ES 7.843,5-7.856,75, POC 7.851,25 | valore del 13/10, Legacy, NC +13.075, il movimento piu' grande dei quattro | ritorno sul prezzo dei long aggiunti quella settimana; alle 14:52 +381 |
| venerdi' 13/11, 21:30 ora italiana | report CFTC con i dati di martedi' 10/11 | aggiorna i passi 4-6 e vede il 5/11; da li' questo rapporto e' superato e la fase 2 si rifa |

## Cosa Non Sappiamo

- **L'eta'**: 6 giorni; il crollo del 5/11 non e' in nessun report. Distanze al prezzo delle 14:52, non all'apertura.
- **Nessuna soglia COT e' validata**: la data come livello e' osservata, non contata; 0,30 e |z| 1 sono convenzioni
  nostre; il netto e' un saldo. Legacy e TFF non si fondono. Percentili su tre anni. Futures only, senza opzioni.
- lacune del collettore: `prima_del_rollover` assente in `settimane_dei_report` (tutti e due i file); percentili a tre
  anni solo per l'ultimo report; manca lo z52 del livello del netto; manca l'ora effettiva di pubblicazione; la
  distanza dal composito a 20 sedute non e' nel file (si cita il valore del rapporto delle 12:28).

## Per Il Quadro

1. **Posizionamento contro il regime**: speculatori Nasdaq netti al rialzo (90°) con i corti in risalita (14,7°),
   speculatori S&P netti al ribasso (36,2°), banche al ribasso sul Nasdaq come mai in tre anni. Coerente con il
   regime della macro, o il calo sta scaricando il lato affollato del Nasdaq?
2. **I livelli del passo 6 contro muri e zero gamma**: NQ 28.314 (3/11) e 30.775 (13/10); ES 7.597 (3/11) e 7.851
   (13/10), in prezzi di ES come X-Ray.
3. **La forza relativa**: dal 3/11 NQ -3,38%, ES -1,66% alle 14:52; il rimbalzo non cambia il rapporto (circa il
   doppio). Da confrontare con la correlazione NQ-ES delle altre fasi.
