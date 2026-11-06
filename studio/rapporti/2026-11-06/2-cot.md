# Fase 2 · Il COT · 2026-11-06

**Stato**: fatto
**Dati**: `dati/cot.json` (CFTC, Legacy futures only e TFF), `dati/sedute-NQZ6.json` (cassa 9/10-5/11 e
composito di 20 sedute), `dati/raccolta.json` (chiusa alle 15:05 ora italiana, nessun errore). Report in uso:
dati di **martedi' 27 ottobre**, 10 giorni fa; il nuovo (dati di martedi' 3/11) esce **oggi alle 21:30 ora
italiana**, e da li' questo rapporto e' superato. Prezzo: NQZ6 27.086,25 alle 15:05, prima della cassa delle 15:30.

**In una riga**: il 27/10 gli speculatori sul Nasdaq erano lunghi quasi come mai in tre anni e i gestori
tagliavano forte; poi il prezzo e' sceso di circa 2.700 punti, e il report di stasera non vedra' ieri.

## In Parole Semplici

**Il punto**: l'ultima fotografia delle posizioni e' del 27 ottobre: speculatori molto sbilanciati al rialzo,
grandi gestori che alleggeriscono come raramente fanno. Da allora il prezzo e' sceso di quasi il 9%: chi era
carico al rialzo oggi e' in perdita sulla carta. La fotografia di stasera si ferma a martedi' 3, senza il crollo di ieri.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: gli speculatori hanno piu' del doppio di contratti al rialzo che al ribasso, e quelli al ribasso sono quasi ai minimi di tre anni.
   → Il lato affollato e' quello che compra.
2. **L'ultima settimana misurata**: gli speculatori hanno tolto poco, circa 1.900 contratti; i gestori di fondi circa 12.400, uno dei tagli piu' grandi in tre anni.
   → L'unico movimento fuori dall'ordinario e' quello dei gestori.
3. **Dove stava il prezzo**: il 27 ottobre il valore della seduta era fra circa 29.640 e 29.880.
   → I gestori hanno alleggerito con il prezzo in alto, prima della discesa.
4. **Adesso**: circa 27.090, sotto tutte e quattro le date del rapporto, da circa 2.700 a 4.400 punti.
   → La fotografia e' vecchia piu' in prezzo che in giorni; stasera si sapra' se i gestori hanno continuato.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 29.640-29.880 (centro 29.805) | il valore del 27 ottobre, il giorno del taglio grande dei gestori | torna dove i gestori hanno alleggerito; oggi circa 2.700 punti sopra |
| circa 30.710-30.800 (centro 30.775) | il valore del 13 ottobre, la riduzione piu' grande degli speculatori in quattro settimane | circa 3.700 punti sopra: riferimento per le settimane, non per oggi |

**Quando**: oggi 21:30 ora italiana, il rapporto nuovo (dati di martedi' 3 novembre).
**Quanto e' provato**: i numeri sono misurati (CFTC). Prevedere la direzione con il COT non ha retto nelle
nostre verifiche; la data che diventa un prezzo e' la procedura del corso, vista una volta, mai contata.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi tiene i contratti; **speculatori** = i
"Non-Commercial", chi opera sul prezzo; **gestori di fondi** = gli "Asset Manager" della classificazione TFF;
**valore della seduta** = la fascia del 70% del volume; **centro** = il POC, il prezzo piu' scambiato.

## I Fatti

**Passi 1-2: Legacy `[1 · 27:46]`, solo Non-Commercial `[1 · 34:51]`, futures only, report del 27/10**
(`cot.json`, `indici`). Percentili a tre anni (160 settimane in serie); variazioni sul 20/10.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 mini | 84.567 | 35.728 | 48.839 | -1.532 | +355 | -1.887 | 95,0 | 64,7 | **3,8** |
| S&P 500 e-mini | 215.924 | 350.292 | -134.368 | +39 | -8.264 | +8.303 | 34,4 | 7,7 | 51,9 |
| Dow x5 | 19.827 | 12.083 | 7.744 | -785 | -1.804 | +1.019 | 60,6 | 53,8 | 37,2 |
| Russell 2000 mini | 112.363 | 148.442 | -36.079 | -3.431 | -9.261 | +5.830 | 36,2 | 99,4 | 94,9 |

**Passo 3, skew di fondo** = (long - short) / (long + short): NASDAQ +0,406; netto contro il massimo a tre
anni 58.123 (6/10). **Passo 4** `[1 · 30:01]`: skew della variazione **-1,0**, saturo (segni opposti), quindi
conta lo **z a 52 settimane, -0,17**; |var. netto| al 21,2 percentile. Soglia 0,30: convenzione nostra, da
"delta del 30%" `[1 · 33:12]`. **Passo 5** `[1 · 32:31]`: segni della var. netto NASDAQ -1, S&P +1, Dow +1,
Russell +1, concordi col principale **1 su 4**; nessuno z oltre 1.

**Passo 6, le quattro date sul prezzo** `[1 · 31:53]` (`prezzo_alle_date`, `settimane_dei_report`; cassa
09:30-16:00 New York = 15:30-22:00 ora italiana il 6, 13, 20/10, 14:30-21:00 il 27/10; fasce da 1 punto).

| report | var. netto NC | z52 | pct var. | seduta: VAL-POC-VAH | chiude | settimana: VAL-POC-VAH | adesso dal POC del giorno |
|---|---|---|---|---|---|---|---|
| 27/10 | -1.887 | -0,17 | 21,2 | 29.637-29.805-29.880 | dentro, 29.813 | 29.026-29.500-29.679 | -2.719 |
| 20/10 | +2.203 | +0,20 | 23,1 | 29.599-29.757-29.842 | dentro, 29.688 | 29.584-29.770-30.178 | -2.671 |
| 13/10 | **-9.600** | **-0,86** | **71,2** | 30.712-30.775-30.800 | dentro, 30.789 | 30.846-30.890-31.315 | **-3.689** |
| 06/10 | +6.876 | +0,58 | 59,0 | 31.483-31.530-31.556 | sotto, 31.481 | 30.529-30.824-31.213 | -4.444 |

Ritardo dal 27/10: -2.727 punti dalla chiusura (-9,15%). Nessun rollover in nessuna regola; nessuna seduta mancante.

**Dopo il report** (`sedute-NQZ6.json`, POC di cassa): 28/10 29.844, 29/10 29.560, 30/10 28.778, 2/11 28.695,
**3/11 28.314** (valore 28.296-28.387; la data del report di stasera), 4/11 28.632, **5/11 26.900** (valore
26.829-27.361, chiude 26.746 **sotto il valore**, delta -4.781). Composito di 20 sedute: POC 29.840, VAL 28.788.

**TFF del Nasdaq, complemento** (`tff_nasdaq`, report 27/10):

| categoria | netto | var. long | var. short | var. netto | z52 | pct var. 3a | pct netto | pct short |
|---|---|---|---|---|---|---|---|---|
| Asset Manager | 65.381 | -3.793 | +8.616 | **-12.409** | **-2,06** | **97,4** | 24,4 | 89,1 |
| Leveraged Funds | 2.301 | -578 | -1.929 | +1.351 | 0,11 | 7,7 | 94,9 | **0,6** |
| Dealer | -85.514 | +2.893 | -5.715 | +8.608 | 0,58 | 53,8 | 10,3 | 85,9 |

TFF S&P 500 (`tff_sp500`): Asset Manager +22.072 (z 0,81), Leveraged Funds -29.359 (z -0,62).

## Cosa Dicono Per NQ

- **Livello**: netto al 95° percentile, ma il campo estremo e' lo **short al 3,8°**: il netto e' alto perche'
  i corti sono quasi spariti, non per eccesso di lunghi (64,7°) (`il-framing-e-il-cot-si-leggono-insieme.md`).
- **Flusso**: ordinario sulla Legacy (z -0,17); piede sull'acceleratore dei venditori, appena appoggiato.
- **Cross-confirm**: il Nasdaq e' l'unico dei quattro in riduzione; la metrica cross-index **non ha retto**
  alla verifica (`analisi-istituzionale.md`), si scrive com'e' e non si usa come conferma.
- **Il livello del passo 6, Legacy**: il movimento piu' grande e' il **13/10** (-9.600, z -0,86). La seduta
  chiude in alto nel proprio valore ma tutta sotto il valore della sua settimana (massimo 30.814,75 contro
  VAL settimanale 30.846). Livello: **30.712-30.800, POC 30.775**.
- **La TFF, che non e' un sinonimo**: gli Asset Manager il 27/10 tagliano -12.409 (z -2,06), quasi tutto
  aprendo corti, con il prezzo che chiude sopra il valore della sua settimana (29.813 contro VAH 29.679).
  **Osservazione**: e' la forma dell'esempio del live, distribuzione in alto; dopo, massimo il 28/10 (30.009)
  e discesa fino a 26.718 (5/11). Vista una volta; nessuna categoria si lega al tape.
- **Comodo o scomodo**: il POC del composito (29.840) e le quattro date stanno **sopra** il prezzo, da 2.671 a
  4.444 punti: i lunghi di ottobre sono sotto. **Osservazione**: POC del composito e del 27/10 distano 35 punti.
  Leveraged Funds con lo short a 0,6°: in teoria pochi corti da ricoprire, **modello** non provato su NQ.
- **Il COT non dice quando**: il report di stasera finisce al 3/11; il 5/11 entra in quello del 13/11.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.712-30.800, POC 30.775 | valore del 13/10, Legacy, il movimento NC piu' grande | posizionamento: ritorno dove gli speculatori hanno ridotto; oggi +3.689 punti |
| 29.637-29.880, POC 29.805 | valore del 27/10, TFF, Asset Manager z -2,06 | posizionamento: ritorno dove i gestori hanno aperto corti; oggi +2.719 punti |
| 28.296-28.387, POC 28.314 | valore del 3/11, data del report di stasera | da confermare alle 21:30: livello solo se il report mostra un movimento grande |
| oggi 21:30 ora italiana | COT nuovo (dati di martedi' 3/11) | dira' se il taglio dei gestori e' continuato nella prima parte del calo |

## Cosa Non Sappiamo

- Il dato ha 10 giorni e circa 2.700 punti di calo dopo; il prezzo di adesso e' prima della cassa. **Nessuna
  soglia COT e' validata**: la data che diventa livello e' osservata, non contata su una serie.
- Legacy e TFF sono tassonomie diverse (13/10 contro 27/10): non si fondono. Percentili a tre anni, non sul
  decennio di `analisi-istituzionale.md`. Futures only: con le opzioni i numeri sarebbero diversi.
- **lacuna del collettore**: `prezzo_alle_date` e `settimane_dei_report` si fermano al report in uso; manca il
  profilo della settimana 28/10-3/11, quella del report di stasera (qui solo i POC delle sedute).
- **lacuna del collettore**: l'ora effettiva di pubblicazione del report in uso non e' in `cot.json`.
- **lacuna del collettore**: la TFF non riporta Other Reportable e Non Reportable, ne' lo skew di fondo.

## Per Il Quadro

1. Speculatori lunghi al 95° con i corti al 3,8°, gestori in taglio forte (z -2,06), prezzo -9% in dieci
   giorni: coerente con il regime della macro, o il calo sta scaricando il lato affollato?
2. I livelli del passo 6, **30.775** (13/10) e **29.805** (27/10), contro muri delle opzioni e zero gamma.
3. Il prezzo (27.086) e' dentro il valore del 5/11 (26.829-27.361) e sotto il VAL del composito (28.788): il
   primo prezzo di posizionamento sopra e' il 3/11 (28.314), solo se il report di stasera lo qualifica.
4. Il quadro scritto prima delle 21:30 usa il COT del 27/10: la fase 5 lo dichiara; dopo, la fase 2 si rifa.
