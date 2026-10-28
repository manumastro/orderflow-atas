# Fase 1 · La Macro · 28/10/2026 · ES (ESZ6)

**Stato**: parziale (`esiti` vuoto; `raccolta.json` non ancora scritto; Fed, tassi e calendario ripresi dalla macro di NQ di oggi, `studio/rapporti/2026-10-28/1-macro.md`, senza nuove ricerche web)
**Dati**: `dati/macro.json` (raccolto 28/10 10:17 ora italiana, nessun errore), `dati/ore-ESZ6.json` (bridge, ultima ora aperta 10:00-10:18). Freschezza: curva Treasury al 27/10; FRED giornalieri al 26-27/10; VIX e future ZQ di oggi (10:02, 09:39-10:06); VIX9D, VIX3M, VXN e 10 anni Yahoo fermi al 27/10 sera; ETF settoriali, RSP, SPY alla chiusura del 27/10; future ES, NQ, DXY, WTI, oro alle 10:07.

**In una riga**: tassi alti ma in calo da un mese, Fed ferma oggi e attesa al rialzo a dicembre, volatilita' media con struttura normale, dollaro e petrolio forti, credito calmo; dentro l'S&P comandano da venti sedute energia e difensivi, mentre le comunicazioni sono al minimo dell'anno; ES vicino ai massimi, fermo nella notte, in attesa della Fed alle 19:00 e dei conti di MSFT e GOOGL dopo le 21:00.

## In Parole Semplici

**Il punto**: l'S&P sta a circa l'1,4% dal massimo dell'anno e si muove poco. Il mondo intorno e' teso (tassi alti, petrolio sopra 100 dollari), ma dentro l'indice i soldi sono andati verso energia e settori difensivi, non fuori dal mercato. Oggi la sorpresa possibile sta nelle parole della Fed su dicembre e nei conti di Microsoft e Alphabet.

**Dal piu' ampio al piu' vicino**

1. **I tassi e la Fed**: il titolo di Stato a 10 anni rende quasi il 5%, ma e' sceso di un quarto di punto in un mese; oggi tassi fermi quasi certi, a dicembre le scommesse danno un rialzo al 76%.
   → La partita e' su dicembre: le parole del presidente alle 19:30 possono muovere i tassi, e l'indice con loro.
2. **La paura**: l'indice della paura sull'S&P (VIX) e' a 17,6, a meta' del suo anno; la paura a nove giorni costa meno di quella a tre mesi.
   → Il mercato paga una protezione normale. Ma il VIX e' quasi il doppio di quanto ES si e' davvero mosso in un mese (circa 10% annuo): le opzioni prezzano gli eventi in arrivo.
3. **Dentro l'S&P**: in venti sedute energia +11%, utility +8%, beni di base +6,5%; comunicazioni -7%, al minimo dell'anno; tecnologia -2,6%. L'S&P equipesato (ogni azienda conta uguale) ha fatto 3 punti meglio di quello pesato.
   → Il mercato si e' allargato e si e' fatto piu' prudente: tengono i settori che reggono con petrolio e inflazione alti, soffrono i grandi delle comunicazioni (Meta, Alphabet).
4. **Ieri e la notte**: ieri ES ha chiuso circa 45 punti sopra il minimo delle 15, con la tecnologia il settore migliore (+1,7%); nella notte si e' mosso in meno di 20 punti, intorno a 7.765.
   → Una notte ferma: ES aspetta gli eventi. I chip asiatici, che hanno spinto il Nasdaq, sull'S&P quasi non si vedono.

**Quando**: 19:00 decisione della Fed · 19:30 conferenza del presidente · dopo le 21:00 Microsoft e Alphabet · domani 13:30 PIL e inflazione PCE, prima delle 14:30 Eli Lilly, Mastercard, Merck, Caterpillar, Linde
**Quanto e' provato**: i numeri sono misurati alle fonti; il legame "petrolio e inflazione alti, difensivi ed energia su" e' un'osservazione generale; nessuno studio del repository misura come ES reagisce alla Fed o alle trimestrali, e i pesi dei titoli nell'S&P non sono fra i dati.

**Le parole**: **Fed / FOMC** = la banca centrale USA e la riunione che decide i tassi; **rendimento** = l'interesse che paga un titolo di Stato; **VIX** = la volatilita' attesa (la "paura") sull'S&P per i prossimi 30 giorni; **realizzata** = quanto il prezzo si e' mosso davvero; **equipesato** = ogni azienda conta uguale, a prescindere dalla grandezza; **difensivi** = settori che vendono beni di cui non si fa a meno (utility, alimentari, salute).

## I Fatti

**Tassi e Fed** (stessi dati della macro di NQ; curva par home.treasury.gov al 27/10 · 26/10 · 28/09)

- 3M 4,22 · 4,22 · 4,28; 2A 4,70 · 4,68 · 4,92; 10A 4,99 · 5,00 · 5,24; 30A 5,30 · 5,31 · 5,56. Pendenze 10A-2A 0,29 · 0,32 · 0,32; 10A-3M 0,77 · 0,78 · 0,96.
- FRED: fed funds effettivo 3,88% (26/10); 2 e 10 anni ai percentili 95,0 e 95,6 su due anni; breakeven 10 anni 2,35% (27/10). Settembre: CPI 3,72% a/a (core 2,57%), PPI domanda finale 6,35% a/a, vendite al dettaglio +0,77% m/m; sussidi 194 mila (17/10); Philly Fed 20,9.
- Future ZQ (Yahoo, CBOT, 09:39-10:06, metodo FedWatch in `fomc_future.metodo`): rialzo da 25 pb oggi all'8%; dicembre implicito 4,05%. Polymarket (gamma-api, alla raccolta): oggi fermo 95%; dicembre +25 pb 76,5%; "surriscaldamento" a fine 2026 al 95,1%.

**Volatilita'** (Yahoo; percentile a un anno; realizzata ES dal bridge, log a 5 minuti, 276 x 252)

| | ultimo | ora | var 20 sedute | percentile |
|---|---|---|---|---|
| VIX | 17,57 | 28/10 10:02 | +7,5% | 57,9 |
| VIX9D | 16,65 | 27/10 21:15 | +17,2% | 63,7 |
| VIX3M | 19,57 | 27/10 21:15 | +8,2% | 43,8 |
| VXN | 24,91 | 27/10 21:15 | +12,9% | 65,3 |

- Struttura VIX9D < VIX < VIX3M: **contango** (la paura vicina costa meno di quella lontana). ES realizzata: 9,8% annua su 20 sedute (Yahoo, giornaliera); bridge 6,6% (4 ore), 9,6% (24 ore), 10,9% (48 ore). VIX / realizzata 20 sedute circa 1,8.

**Dollaro, petrolio, oro, credito** (Yahoo; future alle 10:07, ETF al 27/10): DXY 102,37 (percentile 98,8), WTI 101,69 (+12,5% in 20 sedute, percentile 91,3), oro 4.065,9 (percentile 10,3), TLT 81,63 (12,0), HYG 77,60 (8,8). Spread high yield 3,30 (26/10, -8 pb); stress St. Louis -0,63 (16/10).

**Chi guida l'S&P** (Yahoo, variazioni %; ETF alla chiusura del 27/10, future alle 10:07)

| | 1 g | 5 g | 20 g | percentile 1 anno |
|---|---|---|---|---|
| ES (future) | +0,07 | -0,47 | +0,63 | 90,9 (massimo 7.874,0) |
| SPY / RSP | +0,56 / +0,11 | +0,08 / +0,77 | +0,71 / +3,83 | 90,4 / 89,2 |
| NQ (future) | +0,35 | -0,31 | -2,55 | 83,7 |
| IWM / KRE | +1,15 / -0,21 | +1,60 / +2,78 | +0,42 / +3,94 | 60,6 / 72,9 |

- `ampiezza_sp500`: RSP meno SPY -0,45 (1 g), +0,69 (5 g), +3,12 (20 g). Settori in salita: 5 su 11 ieri (mediana -0,06%), 7 su 11 a 5 giorni, 8 su 11 a 20 giorni (mediana +4,95%).

| settore (ETF) | 1 g | 20 g | percentile 1 anno |
|---|---|---|---|
| energia XLE | -2,05 | +10,84 | 98,8 |
| utility XLU | +0,71 | +7,91 | 23,1 |
| beni di base XLP | -0,92 | +6,48 | 89,2 |
| consumi discrezionali XLY | +1,04 | +5,77 | 37,1 |
| materiali XLB | +0,78 | +5,46 | 74,9 |
| salute XLV | -0,06 | +4,95 | 98,4 |
| finanziari XLF | -0,25 | +3,46 | 76,9 |
| immobiliare XLRE | -0,84 | +2,66 | 44,2 |
| industriali XLI | +0,57 | -0,72 | 37,8 |
| tecnologia XLK | +1,70 | -2,58 | 87,3 |
| comunicazioni XLC | -0,26 | -7,08 | **0,0** (minimo dell'anno) |

- Pesi massimi (Yahoo, 20 g): META -16,05, GOOGL -5,97, NVDA -4,08, AVGO -3,60, MSFT +4,40, AAPL +5,37, TSLA +15,48. Sentiment: CNN Fear & Greed 34,3 ("paura"), 30,3 una settimana fa.

**Calendario** (Forex Factory e api.nasdaq.com, ora italiana; cassa di New York 09:30-16:00 = 14:30-21:00 italiane in questa settimana)

| quando | evento | attesa | precedente |
|---|---|---|---|
| 28/10 19:00 · 19:30 | decisione Fed (tasso superiore) · conferenza | 4,00% | 4,00% |
| 28/10 dopo le 21:00 | MSFT (EPS 4,71 $), GOOGL (2,94 $) | | |
| 29/10 13:30 | PIL anticipato t/t; PCE core m/m; sussidi | 3,2%; 0,3%; 198 mila | 1,5%; 0,2%; 194 mila |
| 29/10 prima delle 14:30 | LLY (5,38 $), MA (5,14 $), MRK (2,12 $), CAT (7,12 $), LIN (4,52 $) | | |
| 29/10 dopo le 21:00 | AMZN (2,01 $), INTC (0,28 $) | | |
| 30/10 13:30 · prima delle 14:30 | ECI t/t · XOM (3,90 $), ABBV (3,74 $), CVX (4,78 $) | 0,8% | 0,9% |

- **Esito di ieri**: Conference Board (27/10 15:00) non in `esiti`; la macro di NQ riporta dal web 91,7, **non verificato** (incoerente con il precedente 81,9). ESZ6 nell'ora delle 15:00: 7.723,25 -> 7.748,50 (+25,25), minimo 7.712,50, delta +4.415; ora delle 16:00 +19,25 a 7.768,00, delta +10.336.

**Le notizie e il movimento orario di ESZ6** (bridge, ora italiana)

| ora | notizia | ESZ6 nell'ora |
|---|---|---|
| 27/10 22:12 | CNBC: trimestrali (Coca-Cola, UPS) mostrano un consumatore "resiliente" ma sotto pressione | mercato chiuso, ora 23 -1,75: rumore |
| 28/10 00:25 | WSJ: Dow e Treasury su, rotazione fuori dal tech | ieri pero' XLK +1,70 il settore migliore, RSP-SPY -0,45: sul giorno il titolo non si vede |
| 28/10 02:11 | WSJ: rendimenti francesi ai massimi di piu' anni, Treasury in leggero rialzo | ora 02 -7,75 (7.762,50 -> 7.754,75), delta -1.236: la discesa piu' ampia della notte, legame debole |
| 28/10 04:02 | Trump: la guerra con l'Iran finira' "quando voglio"; costi a 47 mld $ | ora 04 +4: nessuna reazione |
| 28/10 06:51-07:56 | SK Hynix +11%, future Nasdaq su | ora 07 +2, ora 08 +8,75: ES quasi fermo, il movimento e' del Nasdaq |

## Cosa Dicono Per ES

- **Il regime dei tassi** e' quello della macro di NQ: 10 anni al percentile 95,6 su due anni ma -25 pb in venti sedute; Fed ferma oggi (ZQ 92%, Polymarket 95%), rialzo a dicembre al 76,5%. Per l'S&P i tassi alti pesano meno che sul Nasdaq perche' l'indice ha piu' finanziari, energia e difensivi: **osservazione** generale, non misurata su ES in questo repository.
- **La rotazione e' difensiva e da petrolio, non una fuga**: in venti sedute guidano energia (con WTI +12,5%), utility, beni di base e salute (XLV al percentile 98,4); perdono comunicazioni (minimo dell'anno, META -16%, GOOGL -6%) e tecnologia. ES +0,63% resta al percentile 90,9: l'indice tiene perche' i settori si compensano. **Osservazione**.
- **La partecipazione**: RSP fa 3,1 punti meglio di SPY in venti sedute, IWM e banche regionali (KRE +3,9%) positive: il titolo medio sta meglio dei grandi. Ieri il contrario (RSP-SPY -0,45, XLK il migliore): un giorno solo, non un'inversione.
- **La volatilita'**: VIX 17,6 contro realizzata ES 9,8% su 20 sedute e 6,6% nelle ultime 4 ore: le opzioni costano circa il doppio del movimento reale, coerente con Fed e trimestrali in arrivo. VXN/VIX circa 1,42: la paura sta piu' sul Nasdaq che sull'S&P. Il VIX9D sotto il VIX e' di ieri sera.
- **I pesi massimi di stasera**: MSFT e GOOGL sono fra i primi titoli dell'S&P, e XLC (dove stanno GOOGL e META) e' gia' al minimo dell'anno: i conti dopo le 21:00 arrivano a mercato a contanti chiuso e si leggono sul future. Il loro peso esatto nell'S&P non e' nei dati (lacuna). Domani mattina le trimestrali non tech (LLY, MA, MRK, CAT, LIN) toccano salute, finanziari e industriali.
- **Credito e sentiment**: HYG al percentile 8,8 ma spread high yield in calo e stress negativo: nessuna tensione acuta. Fear & Greed 34 con ES al percentile 91: il sentiment e' piu' cauto dei prezzi.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28/10 19:00 · 19:30 | FOMC, comunicato e conferenza | posizionamento: conferma o smentita del rialzo di dicembre |
| 28/10 dopo le 21:00 | MSFT e GOOGL | posizionamento: due pesi massimi, a cassa chiusa, con XLC al minimo dell'anno |
| 29/10 13:30 e prima delle 14:30 | PIL, PCE core, sussidi; LLY, MA, MRK, CAT, LIN | posizionamento: l'inflazione che la Fed guarda, poi i settori non tech |
| 7.874,0 | massimo a un anno di ES=F (Yahoo, continuo) | contesto: ES e' circa 110 punti (1,4%) sotto |
| 7.752,25 - 7.770,25 | minimo e massimo di ESZ6 dalle 23:00 del 27/10 alle 10:18 (bridge) | contesto: il range della notte, meno di 20 punti |

## Cosa Non Sappiamo

- Il dato del Conference Board di ieri: non in `esiti`, il valore web (91,7) non e' verificato.
- Come ES reagisce nell'ora del FOMC e alle trimestrali delle grandi: non misurato in questo repository.
- lacuna del collettore: `esiti` vuoto; il Conference Board non arriva da FRED. VIX9D, VIX3M, VXN e ^TNX senza barre di oggi; 2YY=F fermo al 13/10.
- lacuna del collettore: i pesi dei titoli e dei settori nell'S&P 500 (la fonte gratuita non li ha), quindi l'ampiezza per titolo dell'S&P e il peso di MSFT e GOOGL nell'indice.
- lacuna del collettore: `fomc_future` da' la probabilita' solo per la prossima riunione; manca la reazione oraria di ES ai FOMC passati e il movimento implicito delle opzioni di MSFT e GOOGL. Calendario della settimana prossima non pubblicato (HTTP 404); manca l'ora della lettura del Fear & Greed; `raccolta.json` non ancora scritto alla stesura.

## Per Il Quadro

- VIX 17,6 contro realizzata ES 9,8% (20 sedute) e 6,6% (4 ore), con tre eventi in tre ore: coerente con cio' che la gamma di ES (fase 3) dice sulle scadenze di oggi e domani, e con un mercato "compresso" dalle opzioni?
- Rotazione verso energia e difensivi, RSP +3,1 punti su SPY in 20 sedute: coerente con il posizionamento degli speculatori su ES e sui Treasury nel COT (fase 2)?
- Polymarket al 76,5% per un rialzo a dicembre e 10 anni al percentile 95,6, mentre ES sta al percentile 91 dell'anno: chi sta pagando la tesi dei tassi alti, i settori o l'indice?
- Notte ferma (range sotto 20 punti, delta orario misto): l'order flow (fase 4) vede attesa o accumulo prima delle 19:00?
