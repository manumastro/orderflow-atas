# Fase 2 · COT · 2026-10-21

**Stato**: fatto (con le lacune del collettore in fondo)
**Dati**: `dati/cot.json` (raccolto alle 11:12 ora italiana, 09:12 UTC; CFTC Legacy futures only e TFF; rilevazione di martedi' 13 ottobre, **8 giorni**; errori: nessuno) e `dati/sedute-NQZ6.json` (sedute di cassa NQZ6 15:30-22:00 italiane, fasce da 1 punto, valore al 70%). `dati/raccolta.json` non c'era alla lettura (raccolta in corso): l'ora viene da `cot.json`. Prezzo di adesso: **NQZ6 29.760,75 alle 11:12 italiane**, fuori cassa (`prezzo_adesso`).

**In una riga**: gli speculatori sul Nasdaq restano scommessi al rialzo come quasi mai negli ultimi tre anni; nell'ultima settimana misurata hanno alleggerito un poco, gli altri indici non vanno nello stesso verso, e oggi il prezzo sta sotto tutti i prezzi a cui si erano mossi nelle ultime quattro settimane.

## In Parole Semplici

**Il punto**: gli speculatori sul Nasdaq sono scommessi al rialzo come quasi mai in tre anni, ma oggi il prezzo sta circa 1.000 punti sotto i prezzi a cui si sono mossi nelle ultime settimane. Chi di loro ha comprato in quelle settimane e' in perdita, cioe' "scomodo".

**Dal piu' ampio al piu' vicino**

1. **Cos'e' il rapporto**: ogni venerdi' l'ente americano che sorveglia i futures dice quanti contratti avevano in mano le varie categorie di operatori il martedi' prima.
   → dice chi e' posizionato e da che parte; non dice quando si muovera' il prezzo.
2. **Gli speculatori**: fondi e gestori che operano per guadagnare sul prezzo sono lunghi sul Nasdaq come quasi mai in tre anni, e quasi nessuno di loro scommette sul ribasso.
   → il lato del rialzo e' affollato, quello del ribasso quasi vuoto.
3. **L'ultima settimana**: fino al 13 ottobre hanno alleggerito un po', chiudendo scommesse al rialzo e aprendone al ribasso.
   → e' una mossa di dimensione normale, non un'uscita in massa.
4. **Gli altri indici**: S&P 500, Dow Jones e Russell non si sono mossi tutti nello stesso verso; il movimento piu' forte e' stato sul Russell, le aziende piccole, nel verso opposto.
   → la mossa sul Nasdaq non e' confermata dagli altri indici.
5. **Il prezzo di allora e di oggi**: il 13 ottobre il Nasdaq si e' scambiato soprattutto intorno a 30.775, chiudendo vicino al bordo alto della fascia del giorno; oggi sta circa 1.000 punti piu' sotto.
   → 30.775 e' il prezzo dove gli speculatori hanno cambiato posizione, e il prezzo di oggi sta sotto tutti i loro prezzi delle ultime quattro settimane.

**Quanto e' provato**: i numeri sono misurati alla fonte. L'idea che il prezzo del giorno del rapporto diventi un riferimento viene dal corso, ed e' un'osservazione su pochi casi. Nessuna soglia di questo rapporto e' provata sui nostri dati: e' contesto, non dice ne' dove andra' il prezzo ne' quando. Il dato ha 8 giorni (martedi' 13 ottobre); il prossimo esce venerdi' 23 ottobre alle 21:30 ora italiana.

**Le parole**: **futures** = contratti su un indice, con una scadenza; **speculatori** = chi opera sui futures per guadagnare sul prezzo; **lungo** = avere contratti che guadagnano se il prezzo sale; **fascia del giorno** = i prezzi dove si e' fatto il 70% degli scambi della seduta; **scomodo** = con il prezzo sotto quello a cui si e' comprato.

## I Fatti

Termini. **Legacy**: la vista storica del report CFTC. **Non-Commercial (NC)**: gli speculatori, fondi e asset manager `[1 · 34:51]`. **Netto**: long meno short. **Percentile 3a**: dove sta il numero nelle ultime 156 settimane (100 = il piu' alto). **z52**: la variazione del netto divisa per la deviazione standard delle variazioni delle 52 settimane prima. **Skew**: (var. long - var. short) / (|var. long| + |var. short|); vale +-1 quando le due variazioni hanno segno opposto. Fonte di tutto: `cot.json`, CFTC, report del 13/10.

**Passi 1-2.** Legacy, solo Non-Commercial `[1 · 27:46]`, `[1 · 34:51]`.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| Nasdaq mini | 83.525 | 35.002 | 48.523 | -3.220 | +6.380 | -9.600 | 95,6 | 62,2 | **2,6** |
| S&P e-mini | 224.804 | 365.799 | -140.995 | +12.795 | -280 | +13.075 | 30,0 | 15,4 | 60,9 |
| Dow x5 | 19.376 | 11.240 | 8.136 | -4.249 | -2.894 | -1.355 | 61,9 | 51,9 | 27,6 |
| Russell mini | 112.145 | 173.148 | -61.003 | +24.695 | -2.147 | +26.842 | 16,9 | **100,0** | 99,4 |

**Passi 3-5.** Skew di fondo = (long - short) / (long + short). Soglia dello skew della variazione **0,30**, convenzione nostra dal "30%" del live `[1 · 33:12]`.

| indice | skew di fondo | skew var. | z52 var. | pct \|var.\| 3a | piede sull'acceleratore | rollover |
|---|---|---|---|---|---|---|
| Nasdaq | +0,409 | -1,0 | -0,86 | 71,2 | venditori | no |
| S&P | -0,239 | +1,0 | +0,36 | 32,1 | compratori | no |
| Dow | +0,266 | -0,19 | -0,48 | 34,6 | nessuno (sotto 0,30) | no |
| Russell | -0,214 | +1,0 | **+2,50** | 96,8 | compratori | no |

Cross-confirm `[1 · 32:31]`: segni della variazione del netto Nasdaq -, S&P +, Dow -, Russell +: **2 su 4** concordi col Nasdaq.

**Nasdaq, le ultime quattro** (OI = open interest, i contratti aperti; rollover = OI -10% nel mese di scadenza):

| report | netto | var. long | var. short | var. netto | skew | z52 | pct \|var.\| | OI % | rollover |
|---|---|---|---|---|---|---|---|---|---|
| 13 ott | 48.523 | -3.220 | +6.380 | -9.600 | -1,0 | -0,86 | 71,2 | -1,4 | no |
| 6 ott | 58.123 | +5.315 | -1.561 | +6.876 | +1,0 | +0,58 | 59,0 | +4,8 | no |
| 29 set | 51.247 | -7.077 | -2.174 | -4.903 | -0,53 | -0,47 | 44,9 | -5,5 | no |
| 22 set | 56.150 | +3.445 | -18.987 | +22.432 | +1,0 | +1,96 | 98,1 | -12,1 | **si'** (OI) |

**Passo 6, il prezzo alle date** (`prezzo_alle_date`). Seduta di cassa NQZ6; **POC** = il prezzo piu' scambiato; **VAL-VAH** = l'area di valore, il 70% del volume attorno al POC. Ritardo = prezzo di adesso meno il valore di quel giorno.

| report | apertura / massimo / minimo / chiusura | POC | VAL-VAH | delta | chiude | ritardo da chiusura | da POC |
|---|---|---|---|---|---|---|---|
| 13 ott | 30.619 / 30.814,75 / 30.591,25 / 30.788,75 | 30.775 | 30.712-30.800 | -270 | dentro | -1.028 (-3,34%) | -1.014,25 |
| 6 ott | 31.513,25 / 31.616,5 / 31.457,75 / 31.480,75 | 31.530 | 31.483-31.556 | -3.557 | sotto | -1.720 (-5,46%) | -1.769,25 |
| 29 set | 30.708,75 / 30.725,5 / 30.504,5 / 30.617,5 | 30.609 | 30.557-30.650 | +1.378 | dentro | -856,75 (-2,80%) | -848,25 |
| 22 set | 30.764,25 / 31.065,5 / 30.763,5 / 31.029,25 | 30.922 | 30.888-30.980 | +2.427 | sopra | -1.268,5 (-4,09%) | -1.161,25 |

**Le settimane coperte** (`settimane_dei_report`, cassa, nessuna seduta mancante):

| settimana | minimo-massimo | POC | VAL-VAH | delta | adesso dal POC |
|---|---|---|---|---|---|
| 7-13 ott | 30.591,25-31.422,25 | 30.890 | 30.846-31.315 | +14.648 | -1.129,25 |
| 30 set-6 ott | 30.529,25-31.616,5 | 30.824 | 30.529-31.213 | -4.000 | -1.063,25 |
| 23-29 set | 30.356,75-31.013 | 30.566 | 30.520-30.773 | +19.877 | -805,25 |
| 16-22 set | 29.053-31.065,5 | 29.735 | 29.606-30.951 | -5.363 | +25,75 |

Composito di cassa (`sedute-NQZ6.json`): 20 sedute dal 23/9, POC **30.770**, valore 30.235-31.414, primo nodo 30.770-30.775; 90 sedute (34 effettive dal 3/9, 6 prima del rollover), POC **29.735**.

**TFF del Nasdaq** (Traders in Financial Futures: Dealer = le banche, Asset Manager = i fondi a lungo termine, Leveraged Funds = hedge fund e CTA):

| categoria | netto | var. long | var. short | var. netto | z52 | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|
| Dealer | -77.932 | -256 | +5.114 | -5.370 | -0,39 | 19,2 | 57,7 | 85,9 |
| Asset Manager | 75.323 | +877 | -648 | +1.525 | +0,29 | 50,0 | 83,3 | 80,1 |
| Leveraged Funds | -11.593 | -3.243 | -10.862 | +7.619 | +0,63 | 84,0 | 42,9 | 8,3 |

## Cosa Dicono Per NQ

- **Livello estremo, flusso ordinario** (`analisi-istituzionale.md`, "Livello contro flusso"). Il netto NC e' al 95,6° percentile, ma il **campo estremo e' lo short: 2,6° percentile**, quasi nessuno speculatore e' corto. Il campo giusto non e' per forza il netto (`il-framing-e-il-cot-si-leggono-insieme.md`, passo 4).
- **Il piede sull'acceleratore e' dei venditori, ma leggero** `[1 · 30:01]`. Il netto scende di 9.600 da due lati: long chiusi (-3.220) e short aperti (+6.380). Lo skew satura a -1 perche' i segni sono opposti, quindi conta lo z: -0,86, sotto 1 in valore assoluto; la variazione supera il 71% di quelle a tre anni. E' un flusso ordinario.
- **Il cross-confirm non c'e'**: 2 su 4, e il flusso piu' forte della settimana e' il Russell (z +2,50, long al 100° percentile), di verso opposto. In ogni caso la metrica cross-index **non ha retto** alla verifica (criterio A senza separazione, criterio B su 12 e 3 casi): resta contesto, non e' un trigger (`analisi-istituzionale.md`, "La metrica cross-index").
- **Si torna sul prezzo** `[1 · 31:53]`. Il 13 ottobre la seduta apre vicino al minimo (30.619) e chiude a 30.788,75, 11 punti sotto il VAH 30.800, con delta quasi nullo (-270). I NC riducono in una settimana che finisce **al bordo alto del valore del giorno**. Il 14 ottobre il valore scende staccato (30.082-30.478) e il prezzo non e' piu' tornato a 30.712. **Osservazione**: ricorda il 13 gennaio del live (distribuzione al VAH, poi il rifiuto). E' una sola occorrenza, non una verifica.
- **Osservazione**: il POC del 13 ottobre (30.775) coincide entro 5 punti con il POC del composito a 20 sedute (30.770, primo nodo). E' la data del posizionamento che cade sul vertice del volume.
- **Il 6 ottobre e' il caso rovescio**: i NC aggiungono +6.876, quasi tutto long (+5.315). Lo fanno nella seduta del massimo del periodo (31.616,5), che chiude **sotto il proprio valore** con delta -3.557. Comprano in una seduta che rompe, come sull'oro l'8 settembre (`il-framing-e-il-cot...`). Quel valore sta 1.769 punti sopra il prezzo.
- **Il 22 settembre** ha la variazione piu' grande (+22.432, z +1,96, 98° percentile), quasi tutta chiusura di short (-18.987), in una seduta chiusa sopra il valore, in una settimana salita da 29.053 a 31.065,5. Ma l'OI scende del 12,1%: la variazione attraversa la scadenza di settembre, e una parte degli short chiusi puo' essere scadenza non rinnovata.
- **La domanda finale** (nostra derivazione, non e' il passo del corso): il lato posizionato, il netto long, sta sopra o sotto il cuore del volume? Vertice del composito a 20 sedute **30.770**, prezzo **29.760,75**: il prezzo sta sotto, e sotto il valore di tutte e quattro le date (da -848 a -1.769 punti dal POC). Chi si e' allungato in queste settimane e' **scomodo**. Il prezzo siede invece sul POC a 90 sedute e della settimana 16-22 settembre (29.735, +25,75).
- **La TFF non coincide con la Legacy**. I Leveraged Funds coprono short (+7.619 di netto) mentre il netto NC Legacy scende di 9.600. A scendere sono i Dealer (short +5.114). Le due tassonomie non sono sinonimi e non si sommano (`i-tre-modelli-del-live-q1.md`, sezione 9).

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| **30.712-30.800, POC 30.775** | valore del 13 ott, la variazione piu' grande fuori dal rollover e l'ultima | **il livello del passo 6**: posizionamento, il prezzo dove i NC hanno ridotto; un ritorno li' e' un ritorno sul loro prezzo |
| 30.770 | POC del composito a 20 sedute | vertice del volume: il termine della domanda finale; coincide col livello sopra |
| 30.888-30.980, POC 30.922 | valore del 22 set, la variazione piu' grande in assoluto | posizionamento, **con la riserva del rollover**: dove si sono chiusi 18.987 short |
| 31.483-31.556, POC 31.530 | valore del 6 ott, NC +6.876 | posizionamento: dove si sono aggiunti i long; sta al massimo del periodo |
| 29.735 | POC della settimana 16-22 set e del composito a 90 sedute | il cuore del volume dove sta il prezzo adesso |
| ven 23 ott, 21:30 | prossimo COT (dati di martedi' 20 ott) | si rifa' la fase: dira' come si sono mossi i NC durante la discesa sotto 30.712 |
| ven 30 ott, 20:30 | COT dopo il cambio d'ora europeo (25 ott; USA il 1 nov, da calendario) | l'ora italiana scende di un'ora per una settimana |

## Cosa Non Sappiamo

- **L'eta' del dato**: la rilevazione e' di 8 giorni fa. Dal 14 ottobre il prezzo ha perso piu' di 1.000 punti, e come i NC hanno reagito si sapra' venerdi'.
- **Il COT non dice quando**: la variazione e' sparsa su cinque sedute (7-13 ott, 30.591-31.422, POC 30.890). Il martedi' e' la data del report, non per forza il giorno in cui ci si e' posizionati.
- **Nessuna soglia COT e' validata.** La soglia 0,30 e' nostra; le coincidenze del passo 6 sono una occorrenza ciascuna; il COT resta contesto, mai un segnale.
- **Il 22 settembre** e' settimana di rollover per la regola dell'OI: la variazione grande non si legge come una scelta direzionale pulita.
- lacuna del collettore: il campo `prima_del_rollover` manca in `settimane_dei_report` (la skill lo prevede). Non si sa quali sedute del 16-18 set avevano NQZ6 non ancora principale, cioe' se il POC 29.735 di quella settimana poggia su volume sottostimato.
- lacuna del collettore: le date in cui il netto NC e' salito verso il 95° percentile, prima delle ultime quattro, con il valore di quelle sedute. Senza, non si sa a che prezzo e' stato costruito il grosso del netto long.
- lacuna del collettore: percentili e z sono su 160 settimane. Il documento del metodo usa 521 variazioni (dieci anni): i numeri delle due serie non si confrontano.

## Per Il Quadro

- **Posizionamento contro il regime della macro (fase 1)**: speculatori al 95,6° percentile di netto, con lo short al 2,6°, e il prezzo sotto tutti i loro valori recenti. Il regime della macro e' compatibile con un lato lungo affollato e scomodo, o lo mette sotto pressione?
- **I livelli del passo 6 contro i muri e lo zero gamma (fase 3)**: 30.712-30.800 (POC 30.775, uguale al vertice 30.770), 30.888-30.980 con riserva, 31.483-31.556. Un muro o uno zero gamma che cade su uno di questi valori e' una convergenza da segnalare, non un segnale.
- **Il prezzo sul nodo 29.735 (fase 4)**: e' il POC a 90 sedute e della settimana 16-22 set. Cosa fa l'order flow su quel nodo.
- **Il Russell**: flusso NC al 96,8° percentile (z +2,50), con long e short entrambi ai massimi a tre anni, di verso opposto al Nasdaq. Da confrontare con la lettura della macro sulle small cap.
