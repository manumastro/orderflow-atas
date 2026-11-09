# Fase 2 · Il COT · 2026-11-09 (NQZ6 ed ESZ6, studio congiunto)

**Stato**: fatto
**Dati**: `dati/cot.json` (principale NASDAQ-100 mini, prezzi di NQZ6) e `dati-ES/cot.json` (principale S&P 500
e-mini, prezzi di ESZ6): CFTC alla fonte, Legacy futures only e TFF, gli stessi quattro indici nei due file. Poi
`dati/sedute-NQZ6.json` e `dati-ES/sedute-ESZ6.json`. Raccolta alle 12:28 ora italiana (`raccolta.json`), nessun
errore. Report in uso: dati di **martedi' 3 novembre**, pubblicato venerdi' 6/11 alle 21:30 ora italiana, eta' 6
giorni. Prezzi di adesso (12:28, notte, prima della cassa delle 15:30): **NQZ6 27.204**, **ESZ6 7.449,5**.

**In una riga**: nel report nuovo gli speculatori restano carichi al rialzo sul Nasdaq e al ribasso sull'S&P; sul
Nasdaq riaprono scommesse al ribasso, e su tutti e due gli indici le banche vendono come raramente.

## In Parole Semplici

**Il punto**: la fotografia nuova (martedi' 3 novembre) mostra due indici con posizioni opposte. Sul Nasdaq gli
speculatori sono carichi al rialzo, sull'S&P al ribasso: lo stesso calo pesa sui primi e premia i secondi. La
novita': sul Nasdaq tornano le scommesse al ribasso, e su entrambi le banche hanno venduto molto.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: sul Nasdaq gli speculatori hanno quasi il doppio di contratti al rialzo che al ribasso; sull'S&P circa 359 mila al ribasso contro 229 mila al rialzo.
   → Due lati carichi opposti: chi e' carico sul Nasdaq perde se il prezzo scende, chi e' carico sull'S&P guadagna.
2. **L'ultima settimana misurata** (fino al 3/11): sul Nasdaq gli speculatori aprono circa 10 mila contratti al ribasso, e quelli al ribasso non sono piu' ai minimi di tre anni. Sull'S&P aggiungono un po' da entrambi i lati.
   → Sul Nasdaq chi spinge e' chi vende, con forza normale; sull'S&P nessuno spinge.
3. **Le banche** (un'altra classificazione dello stesso rapporto): vendono su tutti e due gli indici; sul Nasdaq sono al ribasso come mai in tre anni.
   → Qui i due indici concordano. Dall'altra parte ci sono i fondi, che comprano.
4. **La data sul prezzo**: martedi' 3/11 il Nasdaq stava intorno a 28.300 e l'S&P intorno a 7.600, tutti e due in basso nella loro settimana.
   → Le posizioni nuove sono state prese dopo una settimana di discesa, non in cima.
5. **Adesso**: Nasdaq circa 27.200, circa 1.100 punti sotto (quasi -4%); S&P circa 7.450, circa 150 punti sotto (circa -2%).
   → Dal 3/11 il Nasdaq e' sceso il doppio dell'S&P; il crollo del 5/11 entra solo nel report di venerdi' 13.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.300 sul Nasdaq, circa 7.600 sull'S&P | dove si e' scambiato di piu' martedi' 3/11, la data del report nuovo e della vendita piu' grande delle banche | torna dove si sono prese le posizioni di questa settimana; oggi circa 1.100 e 150 punti sopra |
| circa 30.775 sul Nasdaq, circa 7.850 sull'S&P | il 13 ottobre, il movimento piu' grande degli speculatori in quattro settimane: sul Nasdaq riducevano, sull'S&P compravano | riferimento per le settimane: oggi circa 3.570 e 400 punti sopra |

**Quando**: venerdi' 13/11 alle 21:30 ora italiana, il report con i dati di martedi' 10/11 (vedra' il crollo del 5/11).
**Cambia idea se**: venerdi' gli speculatori sul Nasdaq risultano aver tolto molto rialzo (il lato carico si svuota),
o le banche ricomprano su tutti e due gli indici.
**Quanto e' provato**: i numeri sono misurati dalla CFTC. Prevedere la direzione con il COT non ha retto nelle nostre
verifiche; la data che diventa un prezzo e' la procedura del corso, vista su pochi casi e mai contata su una serie.

**Le parole**: **COT** = il rapporto settimanale della CFTC (l'autorita' dei futures americani) su chi tiene i
contratti; **speculatori** = i "Non-Commercial", chi opera sul prezzo; **banche** = i "Dealer", chi fa mercato;
**fondi** = gestori ("Asset Manager") e fondi speculativi ("Leveraged Funds"); **valore** = la fascia dove passa il
70% del volume di una seduta.

## I Fatti

**Passi 1-2: Legacy `[1 · 27:46]`, solo Non-Commercial `[1 · 34:51]`**, futures only, report del 3/11, variazioni sul
27/10 (`cot.json`, `indici`, identico nei due file). Percentili su tre anni (160 settimane in serie).

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short | open interest |
|---|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 mini | 88.899 | 46.026 | 42.873 | +4.332 | +10.298 | -5.966 | 90,0 | 78,8 | 14,7 | 292.587 (+9,0%) |
| S&P 500 e-mini | 229.322 | 358.736 | -129.414 | +13.398 | +8.444 | +4.954 | 36,2 | 22,4 | 57,1 | 1.986.532 (+1,6%) |
| Dow x5 | 20.024 | 9.670 | 10.354 | +197 | -2.413 | +2.610 | 69,4 | 56,4 | 16,0 | 87.410 (-4,6%) |
| Russell 2000 mini | 109.685 | 133.339 | -23.654 | -2.678 | -15.103 | +12.425 | 51,2 | 98,1 | 89,7 | 449.399 (-2,1%) |

Commercial netto: Nasdaq -63.270, S&P +5.195. **Contro il report del 27/10** (rapporti del 6/11, `2-cot.md` NQ ed ES):
Nasdaq netto 48.839 → 42.873, pct netto 95,0 → 90,0, **pct short 3,8 → 14,7**; S&P netto -134.368 → -129.414, pct
netto 34,4 → 36,2, **pct long 7,7 → 22,4**.

**Passi 3-5.** Skew di fondo = (long - short) / (long + short). Skew della variazione = (var. long - var. short) /
(|var. long| + |var. short|), soglia di "skew forte" **0,30, convenzione nostra** dal "delta del 30%" `[1 · 33:12]`;
satura a +-1 quando le due variazioni hanno segno opposto, e allora conta lo z a 52 settimane `[1 · 30:01]`.

| indice | skew di fondo | skew var. | piede sull'acceleratore | z52 var. | pct var. 3a | segno var. netto |
|---|---|---|---|---|---|---|
| NASDAQ-100 mini | +0,318 | -0,408 | venditori | -0,53 | 53,2 | -1 |
| S&P 500 e-mini | -0,220 | +0,227 | nessuno (sotto 0,30) | +0,13 | 12,2 | +1 |
| Dow x5 | +0,349 | +1,000 (satura) | compratori | +0,62 | 58,3 | +1 |
| Russell 2000 mini | -0,097 | +0,699 | compratori | **+1,02** | 75,0 | +1 |

Cross-confirm `[1 · 32:31]` (`cross_confirm`): rispetto al Nasdaq **1 su 4** concordi, rispetto all'S&P **3 su 4**.
Stessi segni del report del 27/10: seconda settimana di fila con il Nasdaq unico in riduzione.

**Passo 6, le quattro date sul prezzo** `[1 · 31:53]` (`prezzo_alle_date`, `settimane_dei_report`). Cassa 09:30-16:00
New York convertita per data: 15:30-22:00 ora italiana il 13 e 20/10 e il 3/11, 14:30-21:00 il 27/10. Fasce da 1
punto (NQ) e 0,25 (ES). Settimana = dal mercoledi' dopo il report precedente al martedi'. Nessun rollover in nessuna
delle due regole, nessuna seduta mancante. "Adesso" = 27.204 e 7.449,5 meno il POC della seduta.

| NQZ6 | var. netto NC | z52 | pct var. | seduta VAL-POC-VAH | chiude | delta seduta | settimana VAL-POC-VAH | delta sett. | adesso |
|---|---|---|---|---|---|---|---|---|---|
| 3/11 | -5.966 | -0,53 | 53,2 | 28.296-28.314-28.387 | dentro, 28.301 | -5.843 | 28.102-28.314-29.494 | -7.691 | -1.110 |
| 27/10 | -1.887 | -0,17 | 21,2 | 29.637-29.805-29.880 | dentro, 29.813 | -565 | 29.026-29.500-29.679 | -5.567 | -2.601 |
| 20/10 | +2.203 | +0,20 | 23,1 | 29.599-29.757-29.842 | dentro, 29.688 | -982 | 29.584-29.770-30.178 | -524 | -2.553 |
| 13/10 | **-9.600** | -0,86 | **71,2** | 30.712-30.775-30.800 | dentro, 30.789 | -270 | 30.846-30.890-31.315 | +14.648 | -3.571 |

| ESZ6 | var. netto NC | z52 | pct var. | seduta VAL-POC-VAH | chiude | delta seduta | settimana VAL-POC-VAH | delta sett. | adesso |
|---|---|---|---|---|---|---|---|---|---|
| 3/11 | +4.954 | +0,13 | 12,2 | 7.589,75-7.597-7.609,25 | dentro, 7.596,25 | -3.521 | 7.610,25-7.753,75-7.794,25 | +36.602 | -147,5 |
| 27/10 | +8.303 | +0,23 | 21,8 | 7.740,75-7.758-7.772,25 | dentro, 7.758,25 | +18.413 | 7.649,75-7.690-7.750 | +45.650 | -308,5 |
| 20/10 | -1.676 | -0,06 | 3,2 | 7.743,25-7.754,5-7.774,5 | dentro, 7.759 | +3.910 | 7.733-7.754,5-7.790,5 | +30.233 | -305 |
| 13/10 | **+13.075** | +0,36 | **32,1** | 7.843,5-7.851,25-7.856,75 | dentro, 7.856 | -502 | 7.826,75-7.855-7.863 | -31.195 | -401,75 |

Ritardo dalla chiusura del 3/11 (`ritardo_in_prezzo`): NQ -1.097 punti (-3,88%), ES -146,75 punti (-1,93%).
**Dopo il report** (`sedute-*.json`, POC di cassa): NQ 4/11 28.632, 5/11 26.900, 6/11 27.175; ES 4/11 7.729,25, 5/11
7.406, 6/11 7.426. Composito di cassa a 20 sedute: NQ POC 29.840 (valore 28.686-31.048), ES POC 7.754 (valore
7.631-7.810,75); tutti e due i prezzi in discount (sotto il valore del composito).

**TFF, complemento** (`tff_nasdaq`, `tff_sp500`, report del 3/11; pct su tre anni):

| indice | categoria | netto | var. long | var. short | var. netto | z52 | pct var. | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|---|
| Nasdaq | Dealer | -112.529 | -1.822 | +25.193 | **-27.015** | **-1,84** | **95,5** | **0,6** | 31,4 | **100,0** |
| Nasdaq | Asset Manager | 73.137 | +3.450 | -4.306 | +7.756 | **+1,29** | 82,7 | 44,2 | 69,2 | 76,3 |
| Nasdaq | Leveraged Funds | 10.073 | +11.527 | +3.755 | +7.772 | +0,63 | 54,5 | 98,1 | 87,2 | 3,2 |
| S&P | Dealer | -688.574 | -11.429 | +37.552 | **-48.981** | **-1,05** | 82,7 | 52,6 | 73,1 | 56,4 |
| S&P | Asset Manager | 928.584 | +666 | -4.469 | +5.135 | +0,17 | 12,8 | 47,4 | 23,7 | 22,4 |
| S&P | Leveraged Funds | -372.318 | +23.190 | -5.096 | +28.286 | +0,55 | 48,7 | 35,9 | 13,5 | 44,9 |

Il 27/10, per confronto: Asset Manager Nasdaq -12.409 (z -2,06), Asset Manager S&P +22.072, Leveraged Funds S&P -29.359.

## Cosa Dicono Per NQ E ES

1. **Il livello** (`il-framing-e-il-cot-si-leggono-insieme.md`: si guarda il campo estremo, non per forza il netto).
   Nasdaq: netto al 90°, long al 78,8°; il campo estremo del 27/10, lo short al 3,8°, **ne e' uscito** (14,7°) con
   +10.298 corti in una settimana. Il lato affollato resta il rialzo. S&P: netto ordinario (36,2°); il long al 7,7°
   del 27/10 risale al 22,4°: nella Legacy dell'S&P oggi nessun campo e' agli estremi.
2. **Il flusso** `[1 · 30:01]`: Nasdaq, piede dei **venditori**, skew -0,41 non saturo (crescono tutti e due i lati,
   i corti di piu'), z -0,53: ordinario. L'open interest +9,0% dice posizioni aperte, non chiuse. S&P: skew +0,23
   sotto soglia, z +0,13, pct 12,2: **nessuno** sull'acceleratore.
3. **Il cross-confirm** `[1 · 32:31]`, l'asse dello studio congiunto: **nella Legacy i due principali divergono in
   segno** (Nasdaq -1, S&P +1), per la seconda settimana. Dow e Russell stanno con l'S&P; solo il Russell supera
   |z| 1 (convenzione nostra per "fuori dall'ordinario"). La metrica cross-index **non ha retto** alla verifica
   (`analisi-istituzionale.md`): si scrive com'e', non conferma niente.
4. **Il livello del passo 6, Legacy**: in tutti e due il movimento piu' grande delle quattro date resta il **13/10**,
   ma di segno opposto: sul Nasdaq gli speculatori riducono (-9.600, corti nuovi +6.380), sull'S&P aggiungono (+13.075,
   long nuovi +12.795). NQ: la seduta sta tutta sotto il valore della sua settimana (massimo 30.814,75 contro VAL
   30.846). ES: dentro il valore della settimana, vicino al bordo alto. **Osservazione**: in tutti e due gli
   speculatori si muovono contro il delta della settimana (NQ +14.648 mentre riducono, ES -31.195 mentre comprano).
   Livelli: **NQ 30.712-30.800, POC 30.775; ES 7.843,5-7.856,75, POC 7.851,25.**
5. **La data nuova, il 3/11**: tutti e due in basso nella settimana. NQ chiude dentro il valore della seduta, con il
   POC della seduta uguale al POC della settimana (28.314) e a un nodo del composito (28.315-28.320); la settimana
   scende da 30.009 a 28.101,75. ES: il valore della seduta sta sotto il VAL della settimana (7.609,25 contro
   7.610,25), mentre il delta della settimana e' +36.602. Per la Legacy il 3/11 e' il secondo movimento dei quattro
   sul Nasdaq (pct 53,2) e il piu' piccolo sull'S&P (12,2). **Osservazione**: sul Nasdaq gli speculatori aprono corti
   dopo una settimana di discesa, in basso, non sul bordo alto come nell'esempio del 13 gennaio del live.
6. **Comodo o scomodo** (la domanda finale del framing): i valori delle quattro date stanno tutti sopra il prezzo, NQ
   da 1.110 a 3.571 punti, ES da 147,5 a 401,75; i POC dei compositi pure (29.840 e 7.754). Nella lettura del
   documento, il lato netto Nasdaq (rialzo) e' sotto, il lato netto S&P (ribasso) e' in guadagno. **Stesso calo,
   effetto opposto**: e' la divergenza che conta di piu' fra i due posizionamenti. E' un modello di lettura, non provato.
7. **La TFF, che non e' un sinonimo** (`analisi-istituzionale.md`). Sul Nasdaq la Legacy dice "speculatori in
   riduzione" (-5.966), la TFF dice gestori +7.756 (z +1,29) e fondi speculativi +7.772: segni opposti nello stesso
   report, la prova in piccolo che le tassonomie non coincidono. **Il movimento notevole della settimana e' delle
   banche**, e qui **i due indici concordano**: Dealer Nasdaq -27.015 (z -1,84, corti al massimo di tre anni, netto
   allo 0,6°), Dealer S&P -48.981 (z -1,05). In tutti e due e' il movimento Dealer piu' grande delle quattro date, e
   cade il 3/11. Venerdi' i gestori dei due indici divergevano (Nasdaq -12.409, S&P +22.072); ora quelli del Nasdaq
   ricomprano. Perche' le banche vendono (copertura o altro) il rapporto non lo dice.
8. **Il COT non dice quando**, e si guarda solo quando esce un report nuovo `[1 · 30:58]`: la seduta del 5/11 (NQ POC
   26.900, ES 7.406) entra nel report di venerdi' 13/11.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 28.296-28.387, POC 28.314 | valore del 3/11: NC -5.966 (secondo dei quattro), Dealer TFF -27.015 (z -1,84); = POC della settimana, nodo del composito | posizionamento: ritorno dove si sono aperti i corti speculativi e le vendite delle banche; oggi +1.110 |
| ES 7.589,75-7.609,25, POC 7.597 | valore del 3/11: Dealer TFF -48.981 (z -1,05), il piu' grande dei quattro; NC ordinario; sotto il VAL della settimana | posizionamento: ritorno sul prezzo delle vendite delle banche; oggi +147,5 |
| NQ 30.712-30.800, POC 30.775 | valore del 13/10, Legacy, NC -9.600, il movimento piu' grande dei quattro | posizionamento, scala delle settimane: dove gli speculatori hanno ridotto; oggi +3.571 |
| ES 7.843,5-7.856,75, POC 7.851,25 | valore del 13/10, Legacy, NC +13.075, il movimento piu' grande dei quattro | posizionamento: ritorno sul prezzo dei long aggiunti quella settimana; oggi +401,75 |
| venerdi' 13/11, 21:30 ora italiana | report CFTC con i dati di martedi' 10/11 (15:30 New York) | aggiorna i passi 4-6 e vede il 5/11; da li' questo rapporto e' superato |

## Cosa Non Sappiamo

- **L'eta'**: 6 giorni, e dopo il 3/11 NQ -1.097 punti, ES -146,75. Il crollo del 5/11 non e' in nessun report uscito.
- **Nessuna soglia COT e' validata**: la data come livello e' osservata, non contata su una serie. Lo 0,30 dello
  skew e l'|z| 1 sono convenzioni nostre. Il netto settimanale e' un saldo: non dice se i long del 13/10 sono aperti.
- Legacy e TFF sono tassonomie diverse: non si fondono, e questa settimana sul Nasdaq danno segni opposti.
  Percentili su tre anni, non sul decennio di `analisi-istituzionale.md`. Futures only, senza opzioni.
- **Osservazione sui profili settimanali**: il valore tocca un estremo (NQ VAL 28.102 contro minimo 28.101,75; ES
  VAH 7.794,25 = massimo): con il POC su un bordo il 70% si allarga da un lato solo, e il bordo e' meno informativo.
- lacuna del collettore: `prima_del_rollover` non c'e' in `settimane_dei_report` (in nessuno dei due file), anche se
  la skill lo prevede; dalle date le settimane stanno dopo il rollover di settembre, ma il campo non lo conferma.
- lacuna del collettore: i percentili a tre anni di netto, long e short ci sono solo per l'ultimo report, non per le
  ultime quattro; il confronto con il 27/10 viene dai rapporti del 6/11.
- lacuna del collettore: manca lo z52 del **livello** del netto (c'e' solo quello della variazione).
- lacuna del collettore: l'ora effettiva di pubblicazione del report in uso non e' in `cot.json`.

## Per Il Quadro

1. **Posizionamento contro il regime**: speculatori Nasdaq netti al rialzo (90°) con i corti in risalita (14,7°),
   speculatori S&P netti al ribasso (36,2°), banche al ribasso sul Nasdaq come mai in tre anni. Coerente con il
   regime della macro, o il calo sta scaricando il lato affollato del Nasdaq?
2. **I livelli del passo 6 contro muri e zero gamma**: NQ 28.314 (3/11) e 30.775 (13/10); ES 7.597 (3/11) e 7.851
   (13/10), in prezzi di ES come X-Ray.
3. **La forza relativa**: dal 3/11 NQ -3,88%, ES -1,93%. Con speculatori Nasdaq al rialzo e S&P al ribasso, lo
   stesso calo fa male al lato carico solo sul Nasdaq: da confrontare con la correlazione NQ-ES delle altre fasi.
4. Il rapporto vale fino a venerdi' 13/11 alle 21:30 ora italiana; dopo, la fase 2 si rifa.
