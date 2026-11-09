# Fase 1 · La Macro · lunedi' 9 novembre 2026 · NQZ6 ed ESZ6 insieme

**Stato**: fatto, con lacune (venerdi' 6 non c'e' nelle barre orarie; vedi Cosa Non Sappiamo).
**Dati**: `dati/macro.json` e `dati-ES/macro.json` (stessi numeri; ES ha in piu' `ampiezza_sp500`), `ore-NQZ6.json`,
`ore-ESZ6.json`, i due `raccolta.json` (12:28 e 12:29 ora italiana, nessun errore). Freschezza: Treasury 6/11; FRED
5/11, inflazione a settembre; VIX e future fed funds oggi 12:13-12:18; VIX9D, VIX3M, VXN, 10 anni venerdi' 22:15.

**In una riga**: tassi corti tirati da un rialzo della Fed atteso a dicembre mentre i lunghi scendono, volatilita'
alta con la paura vicina (struttura invertita venerdi'), credito che si allarga, dollaro forte, paura estrema; la
vendita dei chip colpisce NQ circa il doppio di ES, e dentro l'S&P il titolo medio regge meglio dei grandi.

## In Parole Semplici

**Il punto**: il mondo e' lo stesso per i due future, ma pesa diverso. NQ (le cento grandi del Nasdaq, piene di
chip e intelligenza artificiale) ha perso quasi il 12% in un mese; ES (le cinquecento dell'S&P) circa il 5%, perche'
dentro l'S&P salute, consumi di base e banche salgono. Domani alle 14:30 l'inflazione, con un rialzo Fed atteso.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: la Fed e' attesa alzare di 0,25 a dicembre (circa 8 su 10); i rendimenti a 10 anni pero' sono scesi di 0,36 in un mese.
   → Il denaro costa caro e si teme l'inflazione; ma i tassi lunghi non spiegano la discesa di NQ.
2. **La paura**: l'indice della paura di NQ e' vicino al massimo dell'anno; venerdi' la paura a 9 giorni superava quella a 3 mesi.
   → Chi compra protezione la vuole per i prossimi giorni: un mercato teso, non calmo.
3. **Credito e dollaro**: le obbligazioni delle aziende rischiose chiedono sempre piu' interessi; il dollaro e' forte.
   → Il denaro vuole piu' compenso per il rischio: uno sfondo da prudenza.
4. **Chi guida**: i chip (meno 23% in un mese) trascinano NQ; nell'S&P il titolo medio ha battuto i grandi di 7 punti in un mese.
   → NQ soffre il doppio di ES; ES e' sostenuto dai settori lontani dalla tecnologia.
5. **La notte**: Nvidia (un decimo di NQ) perde il 5% prima della cassa per un'indagine sul suo capo, ma i future salgono un po'.
   → Oggi nessun dato USA: muovono notizie e posizioni; domani conta l'inflazione.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ circa 26.370 | il minimo della notte di venerdi', dopo la vendita dei chip | si torna sul fondo della settimana scorsa |
| NQ circa 26.750, ES circa 7.365 | i minimi di questa notte | la notizia Nvidia riprenderebbe il sopravvento sul rimbalzo |

**Quando**: domani 10:00 parla Waller (Fed) · domani 14:30 inflazione di ottobre · 17/11 sera conti di Nvidia
**Cambia idea se**: l'inflazione esce sotto le attese e il rialzo di dicembre perde probabilita'; oppure il credito
torna a stringersi e i chip smettono di guidare la discesa.
**Quanto e' provato**: i numeri sono misurati; che i chip e non i tassi guidino NQ e' una lettura di un mese di
dati, non uno studio; nessuno studio del repository prova un effetto della macro sul prezzo di domani.

**Le parole**: **Fed** = la banca centrale USA; **rendimento** = quanto paga un titolo di Stato; **VXN, VIX** = la paura
dai prezzi delle opzioni su Nasdaq e S&P; **struttura invertita** = protezione a breve piu' cara che a 3 mesi; **CPI** = prezzi al consumo.

## I Fatti

**Tassi** (curva par del Tesoro; FRED; future ZQ del CBOT di oggi 12:17, metodo FedWatch in `fomc_future`)

| | 3M | 2A | 10A | 30A | 10A-2A | 10A-3M |
|---|---|---|---|---|---|---|
| 6/11 (5/11: 2A 4,46, 10A-2A 0,38) | 4,19 | 4,51 | 4,86 | 5,26 | 0,35 | 0,67 |
| 8/10 (20 sedute fa) | 4,23 | 4,75 | 5,22 | 5,60 | 0,47 | 0,99 |

- Fed funds effettivo 3,88% (5/11). FOMC (la riunione della Fed) il 9/12, decisione alle 20:00. Rialzo di 25 punti
  base all'82,7% dai future; Polymarket: rialzo 76,5%, fermo 22,5%, zero tagli nel 2026 al 98%.
- Inflazione attesa a 10 anni (breakeven, 6/11) 2,23%, al 7 percentile a 2 anni. CPI settembre +0,65% m/m, 3,72%
  a/a; core +0,33% m/m, 2,57% a/a; PPI 6,35% a/a. Sussidi 189 mila (31/10, 2 pct a 2 anni).
- Michigan prelim. novembre 51,0 (att. 48), attese d'inflazione a 1 anno 4,3% da 4,7% (6/11, web, fonte secondaria).

**Volatilita'** (Yahoo, percentile a un anno): VIX 23,56 oggi 12:13 (venerdi' 25,64; 89 pct; +55% in 20 sedute);
VIX9D 27,18 (96 pct), VIX3M 24,60 (90 pct), VXN 34,02 (99 pct; giovedi' 38,26, massimo dell'anno), tutti venerdi'
22:15. Venerdi' VIX9D 27,18 > VIX 25,64 > VIX3M 24,60: struttura invertita. Realizzata (bridge, rendimenti a 5
minuti annualizzati): NQ 30,3 su 24 ore e 36,6 su 4; ES 15,6 e 18,1. A 20 giorni (Yahoo): NQ 31,0, ES 19,6.

**Dollaro, petrolio, oro, credito** (Yahoo; oggi 12:18 salvo gli ETF, 6/11): DXY 101,80 (-0,49% oggi, 92 pct); WTI
87,83 (-3,3% oggi, -9,0% in 5 g); oro 4.114,5 (+0,97% oggi, 18 pct); TLT 81,92 (+5,1% in 20 g); HYG 77,23 (7 pct).
Spread high yield ICE BofA 3,48 punti (5/11, +0,16 in un giorno, 95 pct a 2 anni); stress St. Louis -0,52 (30/10).

**Chi guida** (Yahoo; future oggi 12:18, il resto chiusura 6/11)

| | 1 g | 5 g | 20 g | pct 1a |
|---|---|---|---|---|
| NQ=F 27.230,5 | +0,54% | -3,19% | -11,87% | 47 |
| ES=F 7.453,25 | +0,69% | -1,60% | -5,15% | 59 |
| RSP (S&P a pesi uguali) | -0,38% | -1,04% | +1,41% | 84 |
| SMH (chip) / XLK (tech) | +3,62 / +1,45% | -11,2 / -8,2% | -23,1 / -17,1% | 45 / 49 |

- Grandi a 20 g: AAPL +1,7 e MSFT -0,8 (95 pct a 1 anno); NVDA -18,6; GOOGL -19,0 (5 pct); AVGO -18,8 (1 pct,
  sul minimo dell'anno); META -15,1; TSLA -6,4; AMZN -1,2.
- Nasdaq-100 "6/11" (api.nasdaq.com): 80 salgono, 20 scendono; mediana +0,76%, pesata +0,46%; NVDA pesa il 10,7%.
- S&P (`ampiezza_sp500`): RSP meno SPY -0,69 punti in 1 g, +1,66 in 5 g, +6,97 in 20 g. Settori in salita: 3 su
  11 venerdi' (XLK il migliore, XLP il peggiore), 5 su 11 in 5 g (XLE -12%), 7 su 11 in 20 g (mediana +4,19%).
- Settori a 20 g: salute XLV +8,2 e consumi di base XLP +8,0 (98 pct), immobiliare +5,7, materiali +5,1, banche
  regionali KRE +4,6, finanziari +4,4; giu' tech -17,1, energia -5,3, comunicazioni -5,1, industriali -3,9; IWM -1,9.
- Trimestrali: NVDA 17/11 dopo la chiusura (EPS att. 2,49 $); HD 17/11 e WMT 19/11 prima dell'apertura (S&P non tech).

**Sentiment**: Fear & Greed (CNN) 24,5, paura estrema; 19,4 una settimana fa, 43,5 un mese fa.

**La notte, ora per ora** (bridge, ora italiana; notizie dal feed di `macro.json`)

| ora | NQZ6 | ESZ6 | notizia, con l'ora |
|---|---|---|---|
| 00 | apre 26.878,5, max 27.199 | apre 7.371, min 7.363,75 | 00:42 future volatili per l'indagine sul CEO di Nvidia |
| 03 | massimo della notte 27.319 | 7.444,5 | nessuna |
| 08 | 27.058,75 -> 26.817 (-0,9%), min 26.751,25 | 7.422,5 -> 7.391,25 (-0,4%) | nessuna: movimento senza notizia |
| 09 | 26.818 -> 27.075 (+1,0%) | 7.391,5 -> 7.428 (+0,5%) | 09:05 Commerzbank: rialzo a dicembre; 09:43 "Nasdaq recupera" |
| 12 (29 min) | 27.209,5 | 7.451, max 7.459 | 12:26 CNBC: Nvidia -5% prima della cassa |

Rumore per stanotte, senza un movimento orario proprio: midterm vinte dai democratici e rendimenti giu' (WSJ, 08/11
21:20); Daly (Fed) pronta ad alzare (00:14); Iran senza svolta (Rubio, 12:09) col WTI in calo.

**Calendario USD** (Forex Factory, ora italiana): oggi niente. 10/11 10:00 Waller; 14:30 CPI m/m (att. 0,3%, prec.
0,6%) e core (att. 0,2%, prec. 0,3%). 11/11 niente. 12/11 14:30 sussidi (att. 194 mila); 13/11 14:30 PPI (att. 0,4%).

## Cosa Dicono Per NQ Ed ES

- **I tassi non sono il motore della discesa, i chip si'.** **Osservazione**: in 20 sedute il 10 anni scende di
  0,36 e TLT sale del 5%, cosa che in teoria aiuta la tech; NQ invece perde l'11,9% e SMH il 23%. Per NQ la
  variabile del mese e' il settore chip e AI. Per ES lo stesso calo dei tassi e' coerente con cio' che sale:
  banche, immobiliare, utility.
- **La Fed e' il rischio comune.** Il 2 anni a 4,51 sta 0,63 sopra il fed funds: il mercato prezza un rialzo.
  Venerdi', dopo gli occupati forti (165 mila contro 65, calendario del giro), il 2 anni sale di 5 punti base e
  la curva si appiattisce (10A-2A da 0,38 a 0,35). Il CPI di domani tocca entrambi; per NQ, con volatilita' quasi
  doppia, l'effetto in percentuale e' in teoria maggiore. L'inflazione attesa a lungo e' bassa (7 pct): il
  mercato crede che la Fed la contenga.
- **Volatilita'.** NQ e' in un regime di paura quasi massima (VXN 99 pct), ES alto ma meno estremo (VIX 89 pct).
  Per entrambi l'implicita sta sopra la realizzata (VXN 34 contro 30; VIX 23,6 contro 15,6 su 24 ore): le opzioni
  costano piu' di quanto il prezzo si muova. L'inversione di venerdi' concentra la paura sui prossimi giorni,
  cioe' su CPI e Nvidia. La struttura di oggi non e' misurabile.
- **NQ si muove circa il doppio di ES.** **Osservazione**: -11,9% contro -5,2% in un mese; -0,9% contro -0,4%
  nell'ora 08, +1,0% contro +0,5% nell'ora 09. Dall'apertura di domenica sera pero' salgono quasi uguale
  (circa +1,2% e +1,1%), nonostante il -5% di Nvidia, che pesa il 10,7% del Nasdaq-100.
- **L'ampiezza divide i due.** Nel Nasdaq-100 la mediana sta sopra la media pesata: i grandi frenano. Nell'S&P il
  titolo medio batte i grandi di quasi 7 punti in un mese, salute e consumi di base sono ai massimi dell'anno: una
  rotazione difensiva che sostiene ES e non NQ. Venerdi' e' stato il contrario (tech prima, RSP sotto SPY).
  Sfondo comune: credito al 95 pct e in salita, ma non in crisi in assoluto (3,48 punti); dollaro al 92 pct.
- **Le notizie.** Solo Nvidia muove: all'apertura di domenica NQ parte circa 200 punti sotto la chiusura implicita
  da Yahoo (var_1g), ES circa 30. Le 08 non hanno notizia nel feed; il rimbalzo delle 09 coincide con l'Europa.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 26.366,5 | minimo della notte del 6/11 (calendario del giro) | posizionamento: il fondo della vendita dei chip |
| NQ 26.751,25 / ES 7.363,75 | minimi di questa notte (bridge, ore 08 e 00) | posizionamento: dove la notizia Nvidia ha trovato compratori |
| 10/11 10:00 e 14:30 | Waller; CPI e core di ottobre | l'evento della settimana per tutti e due |
| 17/11 e 19/11 | NVDA dopo la chiusura; HD, WMT prima dell'apertura | NVDA pesa su NQ; HD e WMT sull'S&P non tech |
| 9/12 20:00 | decisione FOMC | il rialzo prezzato all'82,7% |

## Cosa Non Sappiamo

- **lacuna del collettore**: `ore-*.json` copre 48 ore di calendario e col fine settimana parte da lunedi' 00:00;
  mancano le ore di venerdi' 6, quindi nessun movimento orario per Michigan (16:00) e la cassa di venerdi'.
- **lacuna del collettore**: `esiti` e' vuoto il lunedi', la settimana prima resta senza dato. Michigan viene da una
  ricerca web su fonti secondarie (investinglive, advisorperspectives); i salari orari restano non trovati.
- **lacuna del collettore**: l'ampiezza del Nasdaq-100 da' NVDA -5,13% "al 6/11", Yahoo +2,78% venerdi': l'api
  mescola probabilmente il pre-mercato di oggi. Media e mediana di "venerdi'" sono da prendere con cautela.
- **lacuna del collettore**: VIX9D, VIX3M, VXN e 10 anni fermi a venerdi', 2YY=F al 13/10; manca il peso delle
  grandi nell'S&P e l'ampiezza per titolo dell'S&P: l'effetto Nvidia su ES non si quantifica.
- Calendario della settimana prossima non pubblicato (404). "Circa il doppio" fra NQ ed ES non e' una stima del beta.

## Per Il Quadro

- Rialzo Fed all'82,7% e Fear & Greed a 24,5: coerente con la fase 2 (speculatori su NQ mini netti lunghi al 90
  pct a 3 anni, in calo di 5.966)? Una folla lunga in un mondo di paura.
- VXN al 99 pct e inversione di venerdi': confrontare con la gamma della fase 3 (X-Ray su ES, catena di NQ). Una
  gamma negativa sotto il prezzo amplificherebbe un CPI fuori attesa.
- Rotazione dell'S&P (RSP +7 punti su SPY in un mese): se l'order flow di ES diverge da NQ, la macro lo spiega cosi'.
  Minimi della notte NQ 26.751 / ES 7.364 e del 6/11 NQ 26.366,5: da confrontare con i livelli delle fasi 3 e 4.
