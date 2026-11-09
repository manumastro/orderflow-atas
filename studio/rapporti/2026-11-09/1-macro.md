# Fase 1 · La Macro · lunedi' 9 novembre 2026 · NQZ6 ed ESZ6 insieme · aggiornamento pre-cassa

**Stato**: fatto, con lacune (vedi Cosa Non Sappiamo). Aggiorna il rapporto delle 12:28 (`precedenti/1452/1-macro.md`).
**Dati**: `dati/macro.json` e `dati-ES/macro.json` (raccolti alle 14:52 ora italiana, stessi numeri; ES ha in piu'
`ampiezza_sp500`), `ore-NQZ6.json` e `ore-ESZ6.json` (14:52, ultima ora aperta fino alle 14:53), i due `raccolta.json`
(NQ chiusa alle 15:01 per la GEX, ES alle 14:52; nessun errore). Freschezza: VIX, 10 anni, future fed funds, future
NQ ed ES, petrolio, oro, dollaro di oggi 14:35-14:42; VIX9D, VIX3M, VXN venerdi' 22:15; Treasury 6/11; FRED 5/11;
inflazione a settembre; ETF e titoli alla chiusura del 6/11. `esiti` vuoto (lunedi'): nessuna ricerca web.

**In una riga**: lo stesso mondo delle 12:28 (tassi corti tirati da un rialzo Fed atteso a dicembre, lunghi in
discesa, volatilita' alta, credito largo, paura estrema), ma il pre-mercato e' piu' forte: NQ ha raggiunto ES
(+0,84% tutti e due) nonostante Nvidia -5%, con compratori aggressivi su entrambi e la volatilita' di breve in calo.

## In Parole Semplici

**Il punto**: rispetto a mezzogiorno lo sfondo non cambia; cambia la mattina. NQ ed ES salgono ora della stessa
misura, e NQ lo fa senza Nvidia, che pesa un decimo dell'indice e perde il 5%. Domani alle 14:30 l'inflazione.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: il rialzo Fed di dicembre resta il caso centrale; il 10 anni oggi e' fermo a circa 4,85%.
   → Il denaro resta caro; la stima "96%" dai future e' salita per un solo scatto di prezzo, non per una notizia.
2. **La paura**: l'indice della paura dell'S&P scende ancora (23,1 da 25,6 venerdi'); quello di NQ oggi non si legge.
   → Meno tensione di venerdi', ma sempre fra i livelli piu' alti dell'anno.
3. **Credito, dollaro, petrolio**: credito largo (dato di giovedi'), dollaro un po' giu', petrolio -4% oggi, oro su.
   → Meno petrolio vuol dire meno timore d'inflazione: in teoria aiuta, e i titoli di Stato restano fermi.
4. **Chi guida**: nel Nasdaq-100 79 titoli su 96 salgono prima della cassa; solo Nvidia scende forte.
   → La vendita di Nvidia e' una notizia sua, non del settore: il resto dei grandi la compensa.
5. **Le ultime due ore**: NQ +0,5% e ES +0,3% fra le 13 e le 14, con compratori aggressivi su tutti e due.
   → Nessun dato USA oggi: muovono le posizioni e Nvidia; il rischio vero e' domani.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ circa 27.378, ES circa 7.476 | i massimi della mattina (ora 13) | il rimbalzo dopo la settimana di vendite allunga |
| NQ circa 26.750, ES circa 7.365 | i minimi di questa notte | la notizia Nvidia riprenderebbe il sopravvento |

**Quando**: domani 10:00 parla Waller (Fed) · domani 14:30 inflazione di ottobre · 17/11 sera conti di Nvidia
**Cambia idea se**: Nvidia trascina gli altri chip in cassa (il "solo Nvidia" cade); oppure l'inflazione di domani
esce sotto le attese e il rialzo di dicembre perde probabilita'.
**Quanto e' provato**: i numeri sono misurati; "il mercato assorbe Nvidia" e' un'osservazione di una mattina, con
un'ampiezza del Nasdaq che mescola probabilmente il pre-mercato; nessuno studio del repository prova un effetto
della macro sul prezzo di oggi.

**Le parole**: **Fed** = la banca centrale USA; **VIX, VXN** = la paura dai prezzi delle opzioni su S&P e Nasdaq;
**compratori aggressivi** = chi compra al prezzo di chi vende (delta positivo); **CPI** = prezzi al consumo.

## I Fatti

**Cosa e' cambiato dalle 12:28** (fonte e ora accanto)

| | 12:13-12:29 | 14:35-14:53 | fonte |
|---|---|---|---|
| NQZ6 (bridge) | 27.209,5 | 27.345,25, max 27.378 (ora 13) | bridge, M1 per ora |
| ESZ6 (bridge) | 7.451, max 7.459 | 7.470,25, max 7.476,5 (ora 13) | bridge |
| NQ=F / ES=F su venerdi' | +0,54% / +0,69% | +0,84% / +0,84% (14:42) | Yahoo |
| VIX | 23,56 | 23,10 (14:37; 88 pct a 1 anno) | Yahoo |
| 10 anni | 4,86 (venerdi') | 4,845 (14:37, -0,014) | Yahoo ^TNX |
| rialzo FOMC 9/12 dai future | 82,7% | 96,4% (14:39) | ZQ del CBOT, metodo FedWatch |
| WTI / oro / DXY | 87,83 / 4.114,5 / 101,80 | 87,28 (-3,9%) / 4.132,4 (+1,4%) / 101,86 (-0,43%) | Yahoo 14:42 |
| volatilita' realizzata 4 ore NQ / ES | 36,6 / 18,1 | 21,5 / 12,0 | bridge, rendimenti a 5 min annualizzati |

- Il salto del rialzo FOMC viene da **un tick**: ZQF27 da 95,92 a 95,91 (ZQZ26 fermo a 95,98). Il tasso dopo la
  riunione passa da 4,08 a 4,09, la variazione attesa da 20,7 a 24,1 punti base (un punto base = 0,01%).
  Polymarket invariato: rialzo 76,5%, fermo 22,5%; zero tagli nel 2026 al 98,1%.
- Fermi dalle 12:28: curva Treasury 6/11 (3M 4,19; 2A 4,51; 10A 4,86; 30A 5,26; 10A-2A 0,35; 10A-3M 0,67; 20 sedute
  fa 10A 5,22), fed funds 3,88%, breakeven 10 anni 2,23% (7 pct a 2 anni), spread high yield 3,48 (5/11, 95 pct),
  CPI settembre 3,72% a/a, core 2,57%, sussidi 189 mila. VIX9D 27,18, VIX3M 24,60, VXN 34,02 (99 pct): venerdi'.
- Fear & Greed (CNN) 24,5, paura estrema (19,4 una settimana fa). Volatilita' a 20 giorni: NQ 31,2, ES 19,7.

**Chi guida** (chiusura 6/11 salvo dove detto)

| | 1 g | 5 g | 20 g | pct 1a |
|---|---|---|---|---|
| NQ=F 27.312 (14:42) | +0,84% | -2,90% | -11,61% | 47 |
| ES=F 7.464 (14:42) | +0,84% | -1,46% | -5,01% | 59 |
| RSP (S&P a pesi uguali) | -0,38% | -1,04% | +1,41% | 84 |
| SMH (chip) / XLK (tech) | +3,62 / +1,45% | -11,2 / -8,2% | -23,1 / -17,1% | 45 / 49 |

- Nasdaq-100 (api.nasdaq.com, etichetta "Nov 6", ma i valori sono di oggi, vedi lacune): 79 salgono, 17 scendono;
  media +0,90%, mediana +0,95%, pesata +0,99%. NVDA (peso 10,7%) -5,23%; AMZN +3,23, AVGO +3,10, META +3,04, SPCX
  +2,55, GOOGL +2,34, MSFT +2,03, TSLA +0,64, AAPL +0,52. Alle 12:28 la stessa fonte dava 80 su 20, pesata +0,46%.
- S&P (`ampiezza_sp500`, invariata, chiusura 6/11): RSP meno SPY -0,69 punti in 1 g, +1,66 in 5 g, +6,97 in 20 g;
  settori in salita 3 su 11 in 1 g, 7 su 11 in 20 g (salute XLV il migliore, tech XLK il peggiore).
- Trimestrali: NVDA 17/11 dopo la chiusura (EPS atteso 2,49 $). CoreWeave e Nebius riportano questa settimana
  (CNBC 13:00: meta' del valore perso in un mese, timori sul loro debito).

**La mattina, ora per ora** (bridge, ora italiana; delta = acquisti aggressivi meno vendite aggressive, contratti)

| ora | NQZ6 | ESZ6 | notizia, con l'ora |
|---|---|---|---|
| 10 | 27.076 -> 27.131,5, delta +416 | 7.428,25 -> 7.428,25, +200 | 10:52 WSJ: rendimenti USA ed europei in salita |
| 11 | 27.135 -> 27.140, -57 | 7.428,5 -> 7.439, +215 | 11:02 Bloomberg: l'esito elettorale colpisce il trade AI |
| 12 | 27.142 -> 27.199, +29 | 7.439 -> 7.445, max 7.459, +779 | 12:09 Rubio: Iran senza svolta (WTI in calo) |
| 13 | 27.200 -> 27.349 (+0,55%), max 27.378, +807 | 7.445 -> 7.468 (+0,31%), max 7.476,5, +2.554 | 13:01 Reuters: rialzo per acquisti a sconto; 12:57 Oracle sale; 13:39 Barron's: spinta da AMD |
| 14 (53 min) | 27.348 -> 27.345, min 27.230, +1.080 | 7.468 -> 7.470, min 7.451, +2.529 | 14:13 e 14:46 CNBC: indagine del consiglio Nvidia sul CEO, titolo -5% |

Rumore, senza un movimento orario proprio: Congresso su fondi, debito e Hegseth (12:00); WSJ sul Senato democratico e la
Fed (14:31); Goldman su un cambio dei rendimenti (09:19).

**Calendario USD** (Forex Factory, ora italiana): oggi niente. 10/11 10:00 Waller; 14:30 CPI m/m (att. 0,3%, prec.
0,6%) e core (att. 0,2%, prec. 0,3%). 11/11 niente. 12/11 14:30 sussidi (att. 194 mila); 13/11 14:30 PPI (att. 0,4%,
prec. 1,3%) e core (att. 0,3%, prec. 0,7%). Calendario del giro gia' aggiornato stamattina: nessun esito nuovo.

## Cosa Dicono Per NQ Ed ES

- **Concordano sullo sfondo, e oggi anche sul passo.** Alle 12:28 NQ faceva meno di ES (+0,54% contro +0,69%);
  alle 14:42 sono pari (+0,84%). Dall'apertura di domenica NQ +1,7% (26.878,5 -> 27.345), ES +1,3% (7.371 ->
  7.470). **Osservazione**: nell'ora 13 NQ sale quasi il doppio di ES in percentuale, come nel mese, ma al rialzo.
- **Chi guida stamattina: i grandi tranne Nvidia.** Con NVDA -5,2% la media pesata del Nasdaq-100 e' +0,99%, piu'
  di quella delle 12:28 (+0,46%): AMZN, AVGO, META, GOOGL, MSFT fra +2 e +3,2%. **Osservazione**: la notizia e'
  letta come un rischio di una societa', non del settore chip (AVGO +3,1%, venerdi' chiuso a un passo dal minimo dell'anno).
  Se in cassa SMH segue Nvidia e non AVGO, la lettura cade.
- **ES: compratori piu' aggressivi in proporzione.** Delta ore 13-14 ES +5.083 contro NQ +1.887 (i contratti
  non sono comparabili fra i due: ES vale meno per punto ed e' piu' liquido). La rotazione del mese (RSP +7 punti
  su SPY) e' un dato di venerdi': oggi non si sa se il titolo medio dell'S&P segue.
- **La Fed non e' cambiata.** Il passaggio da 82,7% a 96,4% e' la sensibilita' del metodo: un tick sul future di
  gennaio sposta la stima di circa 14 punti. Polymarket, fermo al 76,5%, dice lo stesso di stamattina: rialzo
  probabile. Il 10 anni non si muove (-1,4 punti base): i tassi non spiegano il rialzo di oggi.
- **Volatilita' in calo, implicita sopra la realizzata.** VIX 23,1 contro 12,0 realizzata a 4 ore su ES; per NQ la
  realizzata a 4 ore scende a 21,5 contro un VXN di venerdi' a 34. Le opzioni costano molto piu' di quanto il
  prezzo si muova stamattina: in teoria, prezzano il CPI di domani e Nvidia del 17/11. La struttura VIX9D / VIX3M
  di oggi non si legge.
- **Petrolio -3,9% e oro +1,4%**: con il dollaro in calo (-0,43%), coerente col titolo delle 04:06 ("il petrolio
  basso tempera i timori d'inflazione"). Per il CPI di domani conta ottobre, non oggi.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 27.378 / ES 7.476,5 | massimi della mattina, ora 13 (bridge) | posizionamento: il punto piu' alto del rimbalzo pre-cassa |
| NQ 27.319 | massimo della notte, ora 03 (bridge) | posizionamento: superato alle 13, ora sotto il prezzo |
| NQ 26.751,25 / ES 7.363,75 | minimi della notte (bridge, ore 08 e 00) | posizionamento: dove la notizia Nvidia ha trovato compratori |
| NQ 26.366,5 | minimo della notte del 6/11 (calendario del giro) | posizionamento: il fondo della vendita dei chip |
| 10/11 10:00 e 14:30 | Waller; CPI e core di ottobre | l'evento della settimana per tutti e due |
| 17/11 sera; 9/12 20:00 | conti NVDA; decisione FOMC | NVDA pesa su NQ; il rialzo e' prezzato |

## Cosa Non Sappiamo

- **lacuna del collettore**: VIX9D, VIX3M e VXN fermi a venerdi' 22:15 anche alle 14:37 (Yahoo non li pubblica
  prima della cassa?); 2YY=F fermo al 13/10. La struttura a termine e la paura di NQ di oggi non si misurano.
- **lacuna del collettore**: l'ampiezza del Nasdaq-100 porta l'etichetta "Nov 6" ma i valori sono di oggi (NVDA
  -5,23% contro +2,78% di Yahoo venerdi'; AMZN +3,23% contro -0,35%): il campo `quando` va corretto.
- **lacuna del collettore**: la probabilita' FOMC dai future non porta l'incertezza di un tick (circa 14 punti):
  andrebbe scritta accanto alla stima.
- **lacuna del collettore**: `ore-*.json` non copre venerdi' 6 (48 ore di calendario col fine settimana); manca
  l'ampiezza per titolo dell'S&P e il peso dei grandi in SPY: l'effetto Nvidia su ES non si quantifica.
- Il delta di NQ ed ES non e' comparabile in contratti; nessun beta stimato fra i due.

## Per Il Quadro

- NQ ha raggiunto ES stamattina con Nvidia -5%: confrontare con la fase 4 (l'order flow conferma il passo uguale o
  NQ resta indietro in cassa?) e con la fase 3 (gamma sopra 27.378 e 7.476,5).
- Rialzo Fed ancora atteso (Polymarket 76,5%) e Fear & Greed a 24,5, mentre gli speculatori della fase 2 su NQ
  sono netti lunghi alti: una folla lunga che oggi compra il ribasso.
- Implicita molto sopra la realizzata (VIX 23,1 contro 12,0 su ES; VXN 34 di venerdi' contro 21,5 su NQ): coerente
  con le opzioni della fase 3 che pagano il CPI di domani? Livelli: massimi 27.378 / 7.476,5, minimi 26.751 / 7.364.
