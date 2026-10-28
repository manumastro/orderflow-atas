# Fase 1 · Macro · 27/10/2026

**Stato**: parziale (manca `ore-NQ.json`: niente movimento orario di NQ accanto a eventi e notizie)
**Dati**: `dati/macro.json`, `dati/raccolta.json`, `dati/in-breve.json` (vuoto). Raccolta 27/10 12:18 ora italiana, nessun errore. Freschezza: quotazioni future di oggi 12:08; VIX di oggi 12:03; VIX9D, VIX3M, VXN e 10 anni Yahoo fermi a ieri 21:15; curva Treasury al 26/10; FRED giornalieri al 23/10; stress St. Louis al 16/10; 2YY=F fermo al 13/10 (inutilizzabile). `esiti` vuoto; il PMI flash del 23/10 cercato sul web (una ricerca).

**In una riga**: tassi alti e fermi in attesa della Fed, che domani non dovrebbe muoversi ma a dicembre e' attesa al rialzo; volatilita' del Nasdaq alta rispetto all'S&P con struttura normale; dollaro e petrolio forti, credito debole, sentiment di paura; il Nasdaq resta indietro rispetto al titolo medio.

## In Parole Semplici

**Il punto**: il Nasdaq si muove in un mondo di tassi alti e inflazione che riaccelera. Domani sera la Fed decide, e nei due giorni dopo parlano Microsoft, Google e Amazon: la settimana ha tre appuntamenti grossi in fila.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: il 10 anni americano e' al 5%, vicino ai massimi da quasi vent'anni; il mercato non vede tagli nel 2026 e prezza un rialzo a dicembre.
   → Il denaro costa caro: e' un vento contrario per i titoli tecnologici, che valgono per gli utili futuri.
2. **L'inflazione**: i prezzi al consumo crescono del 3,7% in un anno, quelli alla produzione del 6,3%.
   → La Fed non ha spazio per aiutare; un dato caldo puo' spingere i tassi su.
3. **La paura**: l'indice della volatilita' del Nasdaq e' a 26, piu' alto di tre quarti dell'ultimo anno; l'indice della paura della CNN e' a 29 su 100.
   → Il mercato paga per proteggersi, ma non e' in panico: la paura a breve e' sotto quella a tre mesi.
4. **Chi guida**: in un mese il Nasdaq ha perso circa il 3%, mentre il titolo medio dell'S&P ha guadagnato quasi il 4%.
   → I soldi escono dai grandi della tecnologia (Meta, Google, chip) e vanno sul resto del mercato.
5. **Oggi**: il future Nasdaq e' a circa 29.600 alle 12:08, +0,5% su ieri; alle 15:00 esce la fiducia dei consumatori.
   → E' un dato di peso medio; quello che conta arriva domani alle 19:00.

**Quando**: oggi 15:00 fiducia consumatori · domani 19:00 Fed, 19:30 conferenza · dopo le 21:00 Microsoft e Google · giovedi' 13:30 PIL e inflazione PCE · dopo le 21:00 Amazon
**Quanto e' provato**: i numeri sono misurati (fonti sotto); che tassi alti pesino sul Nasdaq e' un legame noto ma non provato da noi su NQ; non sappiamo come si e' mosso NQ ora per ora, perche' quel dato oggi manca.

**Le parole**: **Fed** = la banca centrale americana; **10 anni** = il rendimento del titolo di Stato a 10 anni; **volatilita' implicita** = quanto movimento il mercato delle opzioni si aspetta; **titolo medio** = l'S&P 500 con tutti i titoli a peso uguale (RSP); **credito debole** = le obbligazioni delle aziende piu' rischiose valgono poco.

## I Fatti

**Tassi** (home.treasury.gov, curva par del 26/10; 20 sedute fa = 25/09)

| scadenza | 26/10 | 25/09 | variazione |
|---|---|---|---|
| 3 mesi | 4,22% | 4,24% | -2 pb |
| 2 anni | 4,68% | 4,81% | -13 pb |
| 10 anni | 5,00% | 5,17% | -17 pb |
| 30 anni | 5,31% | 5,49% | -18 pb |
| 10A-2A / 10A-3M | +0,32 / +0,78 | +0,36 / +0,93 | curva meno ripida |

- Fed funds effettivo 3,88% (FRED DFF, 23/10); forchetta superiore 4,00% (Forex Factory). 10 anni al 95,6° percentile a due anni (FRED DGS10, 23/10). Breakeven a 10 anni 2,34% (FRED, 26/10). pb = punti base, centesimi di punto percentuale.
- FOMC 28/10 (federalreserve.gov). Future sui fed funds (ZQ, Yahoo, 11:52-11:58, metodo CME FedWatch): rialzo di 25 pb al 10,2%; implicito novembre 3,905%, dicembre 4,045%. Polymarket: invariato 92,4%, +25 pb 7,5%; dicembre +25 pb 75,5%; zero tagli nel 2026 96%; fine anno al 4,25% 71,2%.
- Inflazione (FRED, settembre): CPI 3,72% a/a e +0,65% m/m, core 2,57% a/a; PPI domanda finale 6,35% a/a, +1,34% m/m; vendite al dettaglio +0,77% m/m. Sussidi 194 mila (17/10). Philly Fed 20,9 (ottobre, prec. 37,8).
- PMI flash S&P Global 23/10 (web, newsquawk e investinglive): manifattura 56,0 (prec. 55,9, massimo da 53 mesi), servizi 57,9 (prec. 58,8), composito 57,6; prezzi praticati al ritmo piu' alto da agosto 2022.

**Volatilita'** (Yahoo; pct = percentile a un anno)

| indice | ultimo | ora | 20 sedute | pct |
|---|---|---|---|---|
| VIX | 17,86 | oggi 12:03 | +11,4% | 61 |
| VIX9D | 17,92 | 26/10 21:15 | +24,5% | 73 |
| VIX3M | 19,94 | 26/10 21:15 | +9,4% | 51 |
| VXN | 26,40 | 26/10 21:15 | +19,3% | 75 |

- Struttura VIX9D 17,92 < VIX3M 19,94: contango (la paura a breve sotto quella a tre mesi). Volatilita' realizzata a 20 giorni: NQ 18,4%, ES 9,7%.

**Dollaro, materie prime, credito** (Yahoo, oggi 12:08 i future; ETF chiusura 26/10)

| | ultimo | 1 g | 20 sedute | pct |
|---|---|---|---|---|
| DXY | 102,20 | +0,24% | +0,82% | 98 |
| WTI | 100,73 | -1,01% | +12,7% | 89 |
| oro | 4.024,6 | -0,34% | -3,71% | 6 |
| HYG | 77,20 | +0,19% | -0,44% | 6 |
| TLT | 81,68 | +0,42% | +3,89% | 12 |

- Spread high yield 3,38 punti (FRED, 23/10), 93° percentile a due anni. Stress St. Louis -0,63 (16/10, sotto zero = sotto la media). Bilancio Fed 6.754 mld $ (21/10, -2 mld).

**Chi guida** (Yahoo; NQ ed ES oggi 12:08, il resto chiusura 26/10)

| | 1 g | 5 g | 20 sedute |
|---|---|---|---|
| NQ=F (29.597,25) | +0,54% | -0,26% | -3,32% |
| ES=F (7.740,75) | +0,34% | -0,19% | +0,11% |
| RSP | +0,35% | +1,82% | +3,60% |
| SMH | +2,74% | -1,25% | -9,73% |

- Le grandi a 5 giorni: META -10,6%, GOOGL -8,0%, AMZN -5,1%, AVGO -1,6%, NVDA -0,5%, MSFT -0,2%, AAPL +0,3%, TSLA +6,4%.
- Ampiezza del Nasdaq-100 il 26/10 (api.nasdaq.com): 53 salgono, 36 scendono; media +0,46%, mediana +0,14%. Fear & Greed (CNN) 29,3 "fear", una settimana fa 29,6, un mese fa 36,9.
- Trimestrali (api.nasdaq.com, dopo la chiusura, ~21:00 italiane): 28/10 MSFT (EPS att. 4,71 $), GOOGL (2,94 $); 29/10 AMZN (2,01 $), INTC (0,28 $); 02/11 AAPL (1,98 $).
**Calendario USD** (Forex Factory, ora italiana; l'Italia e' gia' in ora solare, New York no fino al 1/11: la cassa apre alle 14:30 italiane)

| quando | evento | atteso | prec. |
|---|---|---|---|
| 27/10 15:00 | fiducia Conference Board | 81,2 | 81,9 |
| 28/10 19:00 / 19:30 | FOMC, decisione e conferenza | 4,00% | 4,00% |
| 29/10 13:30 | PIL anticipato t/t | 3,2% | 1,5% |
| 29/10 13:30 | PCE core m/m | 0,3% | 0,2% |
| 29/10 13:30 | sussidi | 198 mila | 194 mila |
| 30/10 13:30 | indice del costo del lavoro t/t | 0,8% | 0,9% |

Ieri (26/10) nessun evento USD in calendario.

**Notizie** (feed CNBC e Google News, ora italiana; il movimento orario di NQ non c'e', vedi lacune)

- 27/10 01:04 e 03:45 Stocktwits, Yahoo: future fra tassi in salita, vertice Trump-Xi e la quotazione di Anthropic.
- 27/10 10:07 CNBC: Nvidia investe 3 mld $ in Meta; Meta raccoglie 95 mld $ per l'infrastruttura AI.
- 27/10 11:04-11:50 WSJ, NYT, TheStreet: la Fed attesa ferma "nonostante argomenti per un rialzo"; future poco mossi.
- Rumore senza movimento misurabile: morbillo, GM che passa al Nasdaq dal 17/11, cessate il fuoco locale in Ucraina. Il titolo ABC delle 04:09 "Fed holds rates steady" parla di una decisione non ancora presa: non e' un dato.

## Cosa Dicono Per NQ

- **Tassi**: 10 anni al 5% e curva positiva ma meno ripida in un mese (10A-3M da 0,93 a 0,78). I tassi lunghi sono scesi di circa 17 pb, ma NQ ha perso il 3,3%: il ribasso del mese non viene dai tassi. **Osservazione**.
- **La Fed**: per domani i due mercati concordano su "fermi" (ZQ 10%, Polymarket 7,5% di rialzo). Il rischio sta nel tono: Polymarket prezza un rialzo a dicembre al 75%, e inflazione al 3,7% con PPI al 6,3% e PMI sui prezzi ai massimi dal 2022 lasciano spazio a un messaggio duro. Che la conferenza muova NQ piu' della decisione e' **osservazione** comune, non misurata qui.
- **Volatilita'**: VXN 26,4 contro VIX 17,9, e realizzata NQ 18,4% contro ES 9,7%: il rischio e' concentrato sulla tecnologia, non sul mercato intero. La struttura in contango dice che la paura non e' immediata, ma il dato e' di ieri sera.
- **Partecipazione**: RSP +3,6% contro NQ -3,3% in 20 sedute, SMH -9,7%: rotazione fuori dalle grandi. Le grandi che pesano di piu' sul Nasdaq-100 (MSFT 8,9%, GOOGL+GOOG 17,3%, AMZN 6,0%) riportano mercoledi' e giovedi'.
- **Dollaro al 98° percentile, oro al 6°, HYG al 6°, spread HY al 93°**: dollaro forte e credito debole sono un contesto di rischio; il petrolio sopra 100 $ alimenta i prezzi. **Osservazione**: il legame con NQ non e' misurato in questo studio.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 31.483,25 | massimo a un anno di NQ=F (Yahoo, continuo) | riferimento di posizionamento: NQ e' a circa il 6% sotto |
| 5,00% | 10 anni Treasury, 26/10 | contesto: il massimo a un anno dell'indice Yahoo ^TNX e' 5,31% |
| 28/10 19:00-19:30 | FOMC e conferenza | evento principale della settimana |
| 28/10 e 29/10 dopo le 21:00 | MSFT, GOOGL; AMZN, INTC | circa un terzo del Nasdaq-100 per peso |
| 29/10 13:30 | PIL e PCE core | inflazione dopo la Fed |

## Cosa Non Sappiamo

- lacuna del collettore: `ore-NQ.json` non c'e' in `dati/` (e non e' in `raccolta.json`): nessun movimento orario di NQ accanto a notizie ed eventi.
- lacuna del collettore: `esiti` vuoto; il PMI flash del 23/10 e il Michigan finale del 23/10 non sono letti da FRED. Il PMI viene da una ricerca web; il Michigan resta senza dato.
- lacuna del collettore: VIX9D, VIX3M, VXN e 10 anni Yahoo sono fermi a ieri 21:15; 2YY=F e' fermo al 13/10 e va tolto o sostituito.
- lacuna del collettore: la probabilita' di rialzo a dicembre dai future ZQ non e' calcolata (c'e' solo quella di ottobre). Il calendario della settimana prossima non e' pubblicato (Forex Factory 404).
- Fonti in conflitto sul PMI: il precedente della manifattura e' 55,9 per quasi tutte, 57,0 per una (e per il nostro calendario).

## Per Il Quadro

- VXN 26,4 al 75° percentile con contango: coerente con quanto prezzano le opzioni di NQ e con la GEX della fase 3?
- RSP +3,6% contro NQ -3,3% in un mese e META, GOOGL in calo: i grandi speculatori nel COT stanno riducendo il Nasdaq?
- Tre eventi in fila (FOMC 28/10 19:00, MSFT e GOOGL 28/10 sera, PIL e PCE 29/10 13:30): la fase 4 trova un mercato in attesa, in equilibrio, prima di domani sera?
- Tassi lunghi giu' di 17 pb in un mese e NQ giu' del 3%: il ribasso viene dalla tecnologia stessa (trimestrali, AI), non dai tassi.
