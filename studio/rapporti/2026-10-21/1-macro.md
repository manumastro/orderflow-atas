# Fase 1 · La Macro · 2026-10-21

**Stato**: parziale (VIX9D, VIX3M, VXN e ^TNX fermi alla chiusura del 20/10; `raccolta.json` non ancora scritto)
**Dati**: `macro.json` (raccolto alle 11:12 italiane, nessun errore), `ore-NQZ6.json` (bridge, fino alle
11:13). Di oggi: NQ=F, ES=F, DXY, WTI, oro (11:02), VIX (10:57), ZT=F (11:00), future fed funds. **Al 20/10
sera**: VIX9D, VIX3M, VXN, ^TNX, ETF, azioni, curva Treasury. FRED al 19-20/10, stress St. Louis al 09/10.
`esiti` vuoto: nessuna ricerca web. `raccolta.json` assente (GEX in corso): errori della raccolta non letti.

**In una riga**: tassi alti e in salita, banca centrale ferma a ottobre e attesa in rialzo a dicembre,
paura in crescita sul Nasdaq, dollaro forte, petrolio vicino a 100 dollari, credito debole: rischio concentrato nella tecnologia.

## In Parole Semplici

**Il punto**: NQ si muove fra tassi alti e una paura che cresce sulla tecnologia, mentre il resto della borsa regge: il rischio e' concentrato proprio su NQ.

**Dal piu' ampio al piu' vicino**
1. **I tassi e la banca centrale**: il titolo di Stato a 10 anni rende circa il 5,1%, vicino al massimo dell'anno; a ottobre tassi fermi, a dicembre quasi 8 su 10 per un rialzo.
   → il denaro resta caro, e le sorprese possono venire piu' dai dati economici che dalla banca centrale.
2. **La paura**: le opzioni sul Nasdaq prezzano circa 470 punti al giorno, un quinto in piu' di una settimana fa; il credito alle aziende piu' indebitate e' al punto piu' debole dell'anno.
   → ci si aspetta piu' movimento di quello visto finora, con i conti di Meta e Tesla stasera dopo le 22; la tensione sta nella tecnologia e nel debito.
3. **Chi guida e chi frena**: in un mese il Nasdaq ha perso circa il 3% e i chip il 10%, mentre la media delle azioni americane e' salita.
   → NQ dipende da poche grandi aziende: quando i chip vendono, perde anche con il resto della borsa in salita.
4. **La notte**: dopo la chiusura di ieri NQ ha guadagnato circa 320 punti, poi ne ha restituiti circa 265; alle 11 e' poco sopra la chiusura.
   → il rialzo della notte non ha tenuto; chi spinge oggi lo dice lo studio del flusso degli ordini (fase 4).

**Quanto e' provato**: i numeri sono misurati; il loro legame con NQ e' un'osservazione, nessuno studio lo prova. Parte della paura di oggi non e' misurata: alcuni dati sono fermi a ieri sera.

**Le parole**: **rendimento** = l'interesse annuo pagato da un titolo di Stato; **opzioni** = contratti che assicurano contro un movimento, e il loro prezzo misura la paura; **credito** = i prestiti alle aziende; **chiusura** = l'ultimo prezzo della seduta della borsa americana, alle 22 italiane.

## I Fatti

**I tassi** (home.treasury.gov, curva par, %; pb = punto base, 0,01%):

| | 3M | 2A | 10A | 30A | 10A-2A | 10A-3M |
|---|---|---|---|---|---|---|
| 20/10 | 4,26 | 4,79 | 5,12 | 5,42 | +0,33 | +0,86 |
| 19/10 · 21/09 (20 sedute fa) | 4,27 · 4,17 | 4,77 · 4,76 | 5,15 · 4,96 | 5,48 · 5,29 | +0,38 · +0,20 | +0,88 · +0,79 |

- Ieri la curva si appiattisce (2A +2 pb, 10A -3, 30A -6); sul mese si irripidisce (10A +16, 2A +3).
  ^TNX 5,117% (20/10 20:59), pct 1 anno 93,3. ZT=F (prezzo del future sul 2 anni) fermo alle 11:00.
- FRED: fed funds 3,88% (19/10); breakeven 10 anni (inflazione attesa dal mercato) 2,35% (20/10);
  tasso reale approssimato, 10A meno breakeven: **2,77%**.
- **FOMC del 28/10**: rialzo di 25 pb al **13,3%** dai future ZQ (metodo CME FedWatch; 16/10: 15,5%);
  Polymarket fermo 88,0%, +25 pb 11,5%. **Dicembre**: +25 pb 77,5% (Polymarket); ZQZ6 4,075% contro
  3,885% di ottobre, circa +19 pb. Reuters (20/10 15:27): quasi tutti gli economisti dicono lo stesso.

**La volatilita'** (VIX: movimento a 30 giorni prezzato dalle opzioni S&P 500, % annua, /16 = al giorno; VXN: idem sul Nasdaq-100).

| indice | valore | ora | 5 giorni | pct 1 anno | al giorno |
|---|---|---|---|---|---|
| VIX | 17,47 | 21/10 10:57 | +3,6% | 57,5 | ~1,1% |
| VXN | 25,22 | 20/10 22:15 | +19,3% | 67,3 | ~1,6% (~470 punti NQ) |
| VIX9D · VIX3M | 16,27 · 18,99 | 20/10 22:15 | +35,7% · +5,7% | 59,0 · 28,3 | |

Struttura al 20/10 sera: **contango** (16,27 < 17,31 < 18,99), la paura vicina costa meno della lontana.
Realizzata di NQ: 14,6% a 20 giorni (Yahoo, chiusure); 19,3% a 48 ore, 16,5% a 4 ore (bridge, rendimenti
a 5 minuti x 276 x 252). Il VXN sta sopra tutte e tre.

**Dollaro, petrolio, oro, credito** (Yahoo): DXY 101,785 (11:02, 20 g +0,68%, pct 94,4, max 102,58);
WTI 99,44 (11:02, 1 g -2,31%, 20 g +7,9%, pct 88,1); oro 4.137,9 (20 g -4,18%, pct 19,8); TLT 80,13 (20/10,
pct 7,6); **HYG 76,83** (20/10, 20 g -2,34%, **pct 0,4**, minimo 76,81). Spread high yield 3,29 punti
(FRED 19/10, pct 2 anni 92,5); stress St. Louis -0,72 (09/10, sotto zero = sotto la media).

**Chi guida** (Yahoo, 20/10 · 5 g · 20 g): NQ=F +0,24% (oggi 11:02, 29.747,75) · -1,44 · -3,31; ES=F
+0,05 · -0,14 · -0,17; **RSP +1,15 · +1,20 · +1,44**; QQQ -0,10 · -3,49 · -3,99; **SMH -0,38 · -7,10 ·
-10,06**. Grandi a 5 g: META -8,95, AVGO -6,42, NVDA -5,27, GOOGL -3,82, MSFT -2,13, AAPL -0,52, TSLA
-0,41, AMZN +1,57. Ampiezza Nasdaq-100 (api.nasdaq.com, "Oct 20"): 46 su, 55 giu', mediana -0,09%,
SPCX +9,72% (vedi Cosa Non Sappiamo). **Trimestrali**: META (EPS att. 6,54 $) e TSLA (0,22 $) stasera
dopo la chiusura; 28/10 GOOGL, MSFT; 29/10 AMZN; 02/11 AAPL (api.nasdaq.com). **Il sentiment**: Fear
& Greed (CNN, 0-100) **28,1 "fear"**; una settimana fa 26,7, un mese fa 34,2.

**Il calendario** (Forex Factory, USD medi e alti): oggi nessuno; ieri nessuno riportato, `esiti` vuoto.
22/10 14:30 sussidi (att. 200 mila, prec. 199); 23/10 15:45 PMI flash manifattura (att. 55,4, prec. 57,0)
e servizi (57,8, prec. 58,7); 23/10 16:00 Michigan finale (46,3) e attese d'inflazione (prec. 4,7%).

**Le notizie e le ore di NQZ6** (RSS CNBC e Google News; abbinamento per ora, non per causa; delta =
acquisti aggressivi meno vendite aggressive):

| ora | notizia | NQZ6 nell'ora (bridge) |
|---|---|---|
| 20/10 15:27 | Reuters: Fed ferma a ottobre, rialzo a dicembre | ora 15 min 29.391,75, delta -811; ora 16 **+243,75**, delta +2.052 |
| 20/10 sera | "Chip rally in after hours" (Moomoo); SpaceX sopra le stime (CNBC) | ora 22 **+206,25** (29.687,5 -> 29.893,75), delta +1.260 |
| 21/10 02:38 | Trump vuole "un paio di giorni" per la tregua con l'Iran (Stocktwits) | ora 02 max 30.010,25, poi 29.863; ora 03 min 29.845,75 |
| 21/10 09:50-09:54 | Inflazione UK 3,5%, sopra l'atteso; mine a Hormuz fermano tre petroliere (CNBC) | ora 09 +89,75; ora 10 **-160,25**, delta +195 |
| 21/10 10:53 | Trump: i data center paghino un "dividendo", fra i timori sul finanziamento dell'AI | stessa ora 10 |

**Rumore**: Starlink, OpenAI, proteste in Europa, Rubio in India, mutui. Dalle 04 alle 08 NQZ6 scivola senza notizie (-164,25).

## Cosa Dicono Per NQ

Tutte **osservazioni**: nessun documento del metodo misura la reazione di NQ a tassi, rotazione o notizie.
1. **Il peso e' nella tecnologia, non nel mercato**: in 20 giorni NQ -3,31% contro ES -0,17%, RSP +1,44%,
   SMH -10%. NQ e' concentrato: quando l'AI vende, perde anche con un mercato largo in salita.
2. **I tassi sono il fondo della scena**: 10A al pct 93, reale circa 2,8%; a ottobre la Fed non dovrebbe
   sorprendere (88% fermo), la scommessa e' dicembre: il rischio di tasso viene dai dati, non dalla Fed.
3. **La paura e' sul Nasdaq, non sull'S&P**: VXN 25,22 contro VIX 17,47 (7,75 punti), in contango; le
   opzioni prezzano piu' movimento del realizzato, con META e TSLA stasera.
4. **Il credito resta il lato debole**: HYG al minimo dell'anno, spread HY al pct 92,5, stress di sistema
   basso. Coerente con i timori sul debito dell'AI gia' visti il 14-16/10.
5. **La notte restituisce il rally del dopo-chiusura**: +322 dalla chiusura al massimo delle 02, poi -265;
   alle 11:13 NQZ6 a 29.762,25, 74 punti sopra la chiusura delle 22:00.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 10A 5,311% · DXY 102,58 · WTI 112,95 | Yahoo, massimi a un anno | posizionamento (ora 5,117%, 101,785, 99,44) |
| HYG 76,81 | Yahoo, minimo a un anno | credito: sotto, nuovo minimo (ultimo 76,83) |
| NQZ6 29.688 | fine dell'ora 21 del 20/10 (chiusura cassa), bridge | riferimento del gap d'apertura: fase 4 |
| NQZ6 30.010,25 / 29.736,25 | estremi della notte del 21/10, bridge | escursione della notte: fase 4 |
| NQZ6 29.391,75 / 29.903,5 | estremi delle ore 15-16 del 20/10, bridge | escursione di ieri: fase 4 |
| 21/10 dopo le 22:00 · 22/10 14:30 · 23/10 15:45 e 16:00 | META e TSLA; sussidi; PMI flash e Michigan | trimestrali (circa 7% del Nasdaq-100); eventi medi |
| 26-31/10 | ora legale: l'Europa la lascia il 25/10, gli USA l'1/11 | la cassa apre alle 14:30 italiane, i dati alle 13:30 |
| 28/10 19:00 | FOMC; dopo la chiusura GOOGL, MSFT | evento alto |

## Cosa Non Sappiamo

- **L'ampiezza**: dice "Oct 20", ma SPCX +9,72% coincide con "SpaceX +10% in premarket" di oggi e NVDA
  +1,32, META -0,36 non tornano con le chiusure Yahoo del 20/10: probabile pre-mercato di oggi, come il 16/10.
- **WTI -2,31%**: il future di novembre scadeva il 20/10; il continuo CL=F puo' contenere il salto del
  rollover. Non verificato. ZQ e Polymarket divergono di 1,8 punti; notizie abbinate all'ora, non al minuto.
- lacuna del collettore: VIX9D, VIX3M, VXN e ^TNX di oggi (ultimo dato 20/10); 2YY=F fermo al 13/10;
  il mese di contratto dei continui Yahoo (CL=F, NQ=F); l'ora vera dell'`ampiezza`.
- lacuna del collettore: gli eventi passati della settimana nel calendario; NQZ6 a 5 minuti attorno a ogni notizia.

## Per Il Quadro

1. **10A 5,12% (pct 93), rialzo a dicembre al 77,5%**: coerente con il netto del COT (fase 2)?
2. **VXN 25,22 sopra la realizzata (14,6% a 20 g, 19,3% a 48 ore), contango, META e TSLA stasera**: le
   opzioni pagano movimento; coerente con il segno della gamma dei dealer (fase 3)?
3. **NQZ6 29.762 dopo -265 dal massimo della notte, ora 10 -160 con delta +195**: chi vende, e il gap
   sulla chiusura 29.688 regge? Fase 4.
4. **NQ -3,31% contro RSP +1,44% in 20 giorni, HYG al minimo dell'anno**: rotazione dall'AI e stress del
   credito sono lo stesso tema, o due?
