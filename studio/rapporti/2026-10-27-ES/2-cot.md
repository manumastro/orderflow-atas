# Fase 2 · Il COT · 2026-10-27 (ES, contratto ESZ6)

**Stato**: parziale (manca il 29 settembre sul prezzo di ES, la TFF di ES e il profilo settimanale di ES; vedi Cosa Non Sappiamo)
**Dati**: `dati/cot.json` (raccolto 06:23 UTC = 07:23 italiane, CFTC Legacy e TFF, nessun errore), `dati/sedute-ESZ6.json` (sedute di cassa ESZ6 dal 2 al 26 ottobre, ultimo prezzo alle 07:23 italiane). `raccolta.json` non ancora scritto. COT al martedi' 20 ottobre: **7 giorni**. Strumento principale: **S&P 500 e-mini**, codice 13874A, al posto del NASDAQ-100 mini (richiesta dell'utente del 27 ottobre). **Sostituzione dichiarata**: in `cot.json` i campi `prezzo_alle_date`, `prezzo_adesso` e `settimane_dei_report` sono di NQZ6; qui per il prezzo di ES si usano le sedute di `sedute-ESZ6.json`.

**In una riga**: gli speculatori sono corti sull'S&P 500 con pochissimi acquisti in portafoglio, l'ultima settimana non hanno spinto da nessuna parte, e il prezzo sta circa 140 punti sotto il valore del giorno in cui avevano comprato di piu'.

## In Parole Semplici

**Il punto**: chi specula sui future dell'S&P 500 e' scommesso al ribasso, in misura normale, e ha pochissime scommesse al rialzo. Nelle ultime quattro settimane non c'e' stato un movimento forte. Quelli che hanno comprato la settimana del 13 ottobre lo hanno fatto circa 140 punti sopra il prezzo di adesso.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: gli speculatori hanno piu' scommesse al ribasso che al rialzo, e quelle al rialzo sono fra le piu' basse degli ultimi tre anni.
   → Pochi sono posizionati per una salita; il lato al ribasso invece e' nella norma, non estremo.
2. **Le ultime quattro settimane**: il 6 ottobre hanno aggiunto ribassi, il 13 hanno aggiunto rialzi, il 20 hanno tolto un po' di entrambi.
   → Nessuno ha "messo il piede sull'acceleratore" nell'ultima settimana: e' la variazione piu' piccola da tre anni.
3. **Gli altri indici**: sul Russell 2000 gli speculatori comprano forte da due settimane; Nasdaq, S&P 500 e Dow si muovono poco e in versi diversi.
   → Non c'e' una conferma comune sui quattro indici.
4. **La data sul prezzo**: il 13 ottobre l'S&P 500 si scambiava soprattutto intorno a 7.851; adesso e' a circa 7.710.
   → Chi ha comprato quella settimana e' sotto di circa 140 punti; chi ha venduto il 6 ottobre, intorno a 7.880, e' in guadagno.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.850 | il prezzo piu' scambiato il 13 ottobre, la settimana in cui gli speculatori hanno comprato di piu' | chi ha comprato allora torna in pari: e' il posto dove, nel metodo di Fabio, una reazione "ha senso" |
| circa 7.755 | il prezzo piu' scambiato il 20 ottobre, la data dell'ultimo report | il prezzo torna dove stava quando e' stata fatta l'ultima fotografia delle posizioni |
| circa 7.880 | il prezzo piu' scambiato il 6 ottobre, quando hanno aggiunto scommesse al ribasso | chi ha venduto allora torna in pari |

**Quanto e' provato**: i numeri delle posizioni sono misurati (fonte ufficiale americana, la CFTC), ma hanno 7 giorni e dicono chi e' posizionato, non quando il prezzo si muove. Nessuna soglia su questi dati e' mai stata validata in questo repository, e la conferma fra i quattro indici non ha retto alla verifica.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi tiene i future; **speculatori** = la categoria "Non-Commercial", fondi e gestori che cercano un guadagno sul prezzo; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume della seduta; **valore** = la fascia dove si e' scambiato il 70% del volume; **percentile** = su 100 settimane degli ultimi tre anni, quante avevano un numero piu' basso.

## I Fatti

**Passi 1 e 2: Legacy, solo Non-Commercial** `[1 · 27:46]`, `[1 · 34:51]`. Report del 20 ottobre, contratti; percentili sui tre anni (160 settimane), fonte `cot.json`.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| **S&P 500 e-mini** | 215.885 | 358.556 | -142.671 | -8.919 | -7.243 | -1.676 | 29,4 | **7,1** | 57,1 |
| NASDAQ-100 mini | 86.099 | 35.373 | 50.726 | +2.574 | +371 | +2.203 | 96,9 | 71,8 | 3,2 |
| Dow x5 | 20.612 | 13.887 | 6.725 | +1.236 | +2.647 | -1.411 | 55,6 | 60,3 | 59,0 |
| Russell 2000 mini | 115.794 | 157.703 | -41.909 | +3.649 | -15.445 | +19.094 | 30,0 | 100,0 | 97,4 |

S&P 500: open interest (contratti aperti in totale) 1.935.470 (+0,2%); Commercial netto +33.922; minimo e massimo del netto in tre anni -239.827 e +113.440.

**Passo 3, skew di fondo** (netto diviso long piu' short): S&P 500 **-0,248**; Nasdaq +0,418; Dow +0,195; Russell -0,153.

**Passo 4, l'ultima variazione** `[1 · 30:01]`. Skew della variazione = (var. long - var. short) / (|var. long| + |var. short|), soglia **0,30** per "piede sull'acceleratore": la formula e la soglia sono una **convenzione nostra** che traduce il "delta del 30% fra i due lati" del live `[1 · 33:12]`. Satura a +-1 quando le due variazioni hanno segno opposto: allora conta lo z a 52 settimane (z52, quante deviazioni standard la variazione del netto dista dalla media dell'ultimo anno).

| S&P 500, data | netto | var. long | var. short | var. netto | skew | z52 | pct della variazione |
|---|---|---|---|---|---|---|---|
| 20 ott | -142.671 | -8.919 | -7.243 | -1.676 | -0,10 | -0,06 | 3,2 |
| 13 ott | -140.995 | +12.795 | -280 | **+13.075** | +1,00 | +0,36 | 32,1 |
| 6 ott | -154.070 | +2.422 | +13.993 | -11.571 | -0,71 | -0,31 | 26,9 |
| 29 set | -142.499 | -8.456 | +815 | -9.271 | -1,00 | -0,27 | 22,4 |

Nessuna delle quattro e' rollover (`settimana_di_rollover` e `rollover_calendario` falsi). Russell, per confronto: z52 +1,67 il 20 ottobre e +2,50 il 13 (pct 87,2 e 96,8).

**Passo 5, cross-confirm** `[1 · 32:31]`, segni della variazione del netto al 20 ottobre: S&P 500 **-**, Dow **-**, Nasdaq **+**, Russell **+**. Concordi con l'S&P 500: 2 su 4 (il collettore lo calcola rispetto al Nasdaq, `concordi_col_nasdaq: 2 su 4`; rispetto a ES si legge dai segni).

**Passo 6, la data sul prezzo** `[1 · 31:53]`. ESZ6, cassa 13:30-20:00 UTC (15:30-22:00 italiane in ora legale), fasce da 0,25, valore al 70% dal POC; fonte `sedute-ESZ6.json`. Prezzo di adesso **7.709,75** alle 07:23 italiane (sessione notturna). La distanza e' la differenza fra due numeri del file, non una misura del collettore.

| giorno | apertura | massimo | minimo | chiusura | POC | VAL - VAH | delta | chiude | adesso dalla chiusura | adesso dal POC |
|---|---|---|---|---|---|---|---|---|---|---|
| 20 ott | 7.727 | 7.791,5 | 7.710,25 | 7.759 | 7.754,5 | 7.743,25 - 7.774,5 | +3.910 | dentro | -49,25 | -44,75 |
| 13 ott | 7.849,25 | 7.863,25 | 7.834,5 | 7.856 | **7.851,25** | 7.843,5 - 7.856,75 | -502 | dentro, al VAH | -146,25 | **-141,5** |
| 6 ott | 7.862,5 | 7.897,5 | 7.860,25 | 7.873 | 7.879,75 | 7.871,5 - 7.887,5 | +6.487 | dentro | -163,25 | -170 |
| 29 set | — | — | — | — | — | — | — | — | — | — |

Il 29 settembre non e' in `sedute-ESZ6.json` (le sedute partono dal 2 ottobre). Composito di cassa di 17 sedute dal 2 ottobre: POC 7.775, valore 7.681,75 - 7.823; il prezzo di adesso e' dentro, nella parte bassa ("fair value").

**TFF del Nasdaq**, complemento (non c'e' quella dell'S&P 500), 20 ottobre: Dealer netto -94.122 (pct 4,5; short pct 92,3; var. -16.190, z52 -1,12); Asset Manager +77.790 (pct 59,6; var. +2.467); Leveraged Funds +950 (pct 93,6; short pct **1,3**; var. +12.543, z52 +1,01).

## Cosa Dicono Per ES

- **Il campo estremo e' il long, non il netto.** Il netto Non-Commercial e' al 29° percentile, ordinario; il long e' al **7°**: gli speculatori hanno pochi acquisti come raramente in tre anni. E' la distinzione di `il-framing-e-il-cot-si-leggono-insieme.md` (sezione 4): il netto e' quasi sempre il meno informativo.
- **Livello e flusso dicono cose diverse.** Il livello e' corto e stabile (fra -154.070 e -140.995 nelle quattro settimane); il flusso dell'ultima settimana e' il piu' piccolo dei tre anni (3° percentile), con entrambi i lati ridotti. Nessuno spinge: **osservazione**.
- **Il passo 6** `[1 · 31:53]`. La settimana di maggior movimento e' quella del 13 ottobre: +12.795 long, short quasi fermi. Quel giorno ES chiudeva al bordo alto del proprio valore (7.856 contro VAH 7.856,75), in una seduta stretta (29 punti). Da allora il prezzo e' sceso di circa 140 punti dal POC. Nel live la coincidenza fra data e valore e' cio' che "da' senso" alla reazione successiva; qui **il lato che ha comprato sta sotto**, e la settimana dopo i long sono scesi di 8.919: **osservazione**, il dato aggregato non dice che siano gli stessi operatori.
- **Il rovescio, il 6 ottobre**: +13.993 short sul massimo del periodo (seduta al massimo 7.897,5). Il prezzo ora e' 170 punti sotto quel POC: chi ha aggiunto ribassi allora sta comodo. **Osservazione.**
- **Il cross-confirm non conferma**: 2 su 4. L'unico flusso notevole e' sul Russell (due settimane di acquisti, z +2,50 e +1,67), non sull'S&P 500. La metrica cross-index nella forma letterale **non ha retto** (`analisi-istituzionale.md`): si riporta, non si pesa.
- **La TFF e' del Nasdaq e non di ES**, e Legacy e TFF non sono sinonimi (`analisi-istituzionale.md`): i Leveraged Funds del Nasdaq che chiudono corti (short al 1° percentile) non dicono nulla sui Non-Commercial di ES.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| **7.851,25** (valore 7.843,5 - 7.856,75) | POC del 13 ottobre, ESZ6 cassa | **il livello del passo 6**: posizionamento; dove tornano in pari i long aggiunti nella settimana di maggior movimento |
| 7.754,5 (valore 7.743,25 - 7.774,5) | POC del 20 ottobre, data dell'ultimo report | posizionamento: il prezzo della fotografia piu' recente; adesso circa 45 punti sotto |
| 7.879,75 (valore 7.871,5 - 7.887,5) | POC del 6 ottobre | posizionamento: dove tornano in pari gli short aggiunti il 6 ottobre |
| 7.775 (valore 7.681,75 - 7.823) | POC del composito di 17 sedute | contesto: il cuore del volume, sopra il prezzo di adesso |
| venerdi' 30 ottobre, 20:30 italiane | prossimo COT (15:30 ET; l'Italia e' gia' in ora solare, gli Stati Uniti no fino al 1° novembre) | dati del martedi' 27 ottobre: sostituiscono quelli qui sopra |

## Cosa Non Sappiamo

- **Il COT non dice quando.** Ha 7 giorni, e fra il 20 ottobre e oggi ES ha fatto 7.812 il 21 e 7.648,5 il 23: nessuno sa come si sono mosse le posizioni nel frattempo.
- **Nessuna soglia COT e' validata** su questo repository; la soglia 0,30 dello skew e la sua formula sono una convenzione nostra.
- **Ipotesi non verificata qui**: il corto strutturale degli speculatori su ES potrebbe venire in parte da operazioni di arbitraggio fra future e titoli (corti sul future, lunghi sul titolo), che non sono una scommessa sul prezzo. Senza la TFF di ES non si puo' controllare.
- **lacuna del collettore**: `prezzo_alle_date`, `prezzo_adesso` e il ritardo in prezzo sono calcolati solo per NQZ6; per ES le distanze qui sopra sono differenze lette dal file, non misure del collettore.
- **lacuna del collettore**: la seduta del 29 settembre di ESZ6 non e' in `sedute-ESZ6.json` (la cache parte dal 2 ottobre): la quarta data non ha un prezzo.
- **lacuna del collettore**: `settimane_dei_report` (profilo di cassa della settimana coperta da ogni report) esiste solo per NQZ6.
- **lacuna del collettore**: la TFF e' estratta solo per il Nasdaq; manca quella dell'S&P 500 (codice 13874A).
- **lacuna del collettore**: il cross-confirm e' calcolato rispetto al Nasdaq, non rispetto allo strumento principale.

## Per Il Quadro

1. **Corti, con pochissimi acquisti (long al 7° percentile)**: e' coerente con il regime della macro? Se la macro e' di rischio, il posizionamento e' gia' dalla stessa parte; se e' distesa, chi e' corto non ha ancora cambiato idea.
2. **7.851,25** (valore 7.843,5 - 7.856,75), il livello del passo 6: confrontalo con i muri delle opzioni e con lo zero gamma di ES. Vale lo stesso per 7.754,5 (ultimo report) e 7.879,75 (short del 6 ottobre).
3. **Il cuore del volume (POC del composito 7.775) sta sopra il prezzo**: il lato che ha comprato il 13 ottobre e' scomodo, quello che ha venduto il 6 e' comodo. Da tenere accanto alla lettura dell'order flow.
4. **L'unico flusso notevole e' sul Russell**, non su ES: da segnalare se la gamma o l'order flow dicono qualcosa di diverso sui piccoli.
