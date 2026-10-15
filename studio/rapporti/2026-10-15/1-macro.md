# Fase 1 · La Macro · 2026-10-15 (aggiornamento serale)

**Stato**: parziale (i dati delle 14:30 non sono verificati alla fonte: del PPI c'e' solo il titolo
CNBC, di vendite al dettaglio, Philly Fed e sussidi nessun valore; manca FRED)
**Dati**: `dati/macro.json` (raccolto alle 18:43 ora italiana, `raccolto_utc` 16:43:24),
`dati/ore-NQZ6.json` (barre orarie del bridge fino alle 18:44, l'ultima aperta da 44 minuti),
`dati/sedute-NQZ6.json` (sedute di cassa e `oggi_finora` dalle 00:00), `dati/raccolta.json` (e' ancora
quello della raccolta delle 11:10: verra' riscritto a fine raccolta, la GEX di ES e' in corso).
Freschezza: NQ, ES, VIX, VIX9D, DXY, WTI, oro, ^TNX fra le 18:28 e le 18:33; ETF e azioni alle 18:43;
**VXN fermo alla chiusura di ieri** (14/10 22:00); **VIX3M a 18,79 con variazione 0,0**, identico a
ieri: forse non aggiornato; curva Treasury al 14/10 (quella di oggi esce a fine giornata); Fear &
Greed di stasera. Errori in `macro.json`: nessuno. **FRED saltato** (manca `FRED_API_KEY`). Forex
Factory: settimana prossima ancora 404. Il rapporto delle 10:45 e' in `precedenti/1850/1-macro.md`.

**In una riga**: un PPI caldo (6,3% annuo secondo il titolo CNBC) non ha alzato i tassi lunghi, che
sono scesi di 10 pb per la fuga verso i Treasury; la Fed resta vista piu' vicina a un rialzo (27,5% il
28/10), la volatilita' si e' calmata in contango, il dollaro e' vicino al massimo e il credito vicino
al minimo; la vendita dell'AI continua mentre il resto del mercato sale: un contesto di rischio
concentrato sulla tecnologia, non generale.

## I Fatti

### Cosa e' cambiato dalle 10:45

| misura | 10:45 | 18:43 | fonte |
|---|---|---|---|
| 10 anni (^TNX) | 5,19% (14/10) | **5,087%** (-10 pb sul giorno) | Yahoo |
| rialzo di 25 pb il 28/10 | 24,5% | **27,5%** | Polymarket |
| VIX | 17,52 (pct 57,1) | **16,94** (pct 47,6) | Yahoo |
| NQ=F, 1 giorno | -1,23% | -0,86% | Yahoo |
| ES=F, 1 giorno | -0,34% | -0,05% | Yahoo |
| WTI | 94,67 (-0,44%) | 96,63 (+1,62%) | Yahoo |
| TLT | 78,89 (ieri) | 80,085 (+1,51%) | Yahoo |
| HYG | 76,81 (minimo a un anno) | 77,035 (+0,29%) | Yahoo |
| Fear & Greed | 29,8 | 29,3 | CNN |
| NQZ6 | 29.822,5 (10:46) | 29.946,25 (18:44); minimo 29.622,5 alle 15 | bridge |

### I tassi

- **Curva par** (home.treasury.gov, chiusura di New York, in %): invariata dal rapporto delle 10:45,
  ferma al 14/10: 3M 4,28, 2A 4,82, 5A 5,00, 10A 5,19, 30A 5,52; pendenze 10A-2A +0,37 e 10A-3M +0,91
  (la pendenza e' la differenza fra due rendimenti; positiva = curva "normale"). Su 20 sedute: 10A
  +19 pb, 2A +15 pb (pb = punto base, 0,01%).
- **10 anni di oggi** (^TNX, Yahoo, 18:28): **5,087%**, -1,98% sul giorno, cioe' circa **-10 pb** da
  5,19. Percentile a un anno 93,7 (era 94,8); massimo a un anno 5,311%. Su 5 giorni -2,75%.
- **Prossimo FOMC**: 27-28 ottobre, decisione il 28 alle 19:00 italiane (federalreserve.gov).
- **Polymarket** (gamma-api.polymarket.com, prezzo come probabilita', raccolta 18:43):

| mercato | esito | 10:45 | 18:43 |
|---|---|---|---|
| decisione di ottobre (volume 42,0 mln $) | nessun cambio | 75,5% | **71,5%** |
| | rialzo di 25 pb | 24,5% | **27,5%** |
| | rialzo di 50+ pb | | 0,4% |
| | taglio (25 o 50+) | 0,6% | 0,6% |
| tagli nel 2026 | zero | 96,9% | 96,9% |
| tasso a fine 2026 | 4,25% | 61,1% | 57,8% |
| | 4,5% o piu' | 19,6% | 22,6% |
| | 4,0% | 10,6% | 18,8% |
| primo taglio entro... | luglio 2027 | 51% | 51% |

- Il mercato "tasso a fine 2026" allarga entrambe le code (4,0% e 4,5%+ salgono insieme): piu'
  incertezza, non una direzione. Le regole del mercato (estremo alto o medio della forbice) non ci sono.

### La volatilita'

Fonte: Yahoo chart v8, giornaliero su un anno; percentile = posizione dell'ultimo valore nell'anno.

| indice | valore | ora | 1 giorno | 5 giorni | percentile 1 anno |
|---|---|---|---|---|---|
| VIX (implicita a 30 giorni dell'S&P 500) | 16,94 | 15/10 18:28 | +0,41% | +9,93% | 47,6 |
| VXN (la stessa misura sul Nasdaq-100) | 22,70 | **14/10 22:00** | +7,38% | +8,10% | 43,4 |
| VIX9D (orizzonte 9 giorni) | 14,87 | 15/10 18:28 | +1,92% | +21,79% | 46,4 |
| VIX3M (orizzonte 3 mesi) | 18,79 | 15/10 18:28 (?) | 0,0% | +3,93% | 22,6 |

- **Struttura a termine**: VIX9D 14,87 < VIX 16,94 < VIX3M 18,79, **contango** (la volatilita'
  vicina costa meno di quella lontana). VIX9D/VIX3M = 0,79; VIX/VIX3M = 0,90. La soglia della skill
  (VIX9D sopra VIX3M = struttura invertita) **non e' scattata**.
- Il VIX e' sceso da 17,52 (10:30) a 16,94 nella giornata in cui NQ ha toccato il minimo di un mese.
- Volatilita' realizzata di NQ=F (deviazione standard dei rendimenti giornalieri su 20 sedute,
  annualizzata): 18,7%; di ES=F 11,0%.

### Dollaro, petrolio, oro, credito

| strumento | ultimo | ora | 1 giorno | 5 giorni | 20 giorni | percentile 1 anno |
|---|---|---|---|---|---|---|
| DXY (indice del dollaro) | 102,108 | 18:33 | -0,43% | -0,03% | +1,88% | 96,8 (max 102,58) |
| WTI (petrolio, future) | 96,63 | 18:33 | +1,62% | +5,62% | -5,18% | 87,3 |
| oro (future) | 4.170,5 | 18:33 | +1,52% | +0,32% | -5,21% | 25,4 |
| TLT (ETF Treasury 20+ anni) | 80,085 | 18:43 | +1,51% | +2,84% | -2,07% | 6,3 |
| HYG (ETF obbligazioni high yield) | 77,035 | 18:43 | +0,29% | -0,14% | -2,14% | **1,6** (min 76,81) |

Fonte: Yahoo Finance. High yield = obbligazioni societarie sotto il grado d'investimento.

### Chi guida

| strumento | ultimo | ora | 1 giorno | 5 giorni | 20 giorni | percentile 1 anno |
|---|---|---|---|---|---|---|
| NQ=F (future Nasdaq-100, continuo) | 29.924,25 | 18:33 | -0,86% | -3,38% | +1,62% | 84,5 |
| ES=F (future S&P 500) | 7.766,75 | 18:33 | -0,05% | -0,63% | +1,66% | 92,1 |
| QQQ | 723,29 | 18:43 | -0,78% | -3,25% | +0,89% | 84,5 |
| RSP (S&P 500 equipesato: la partecipazione) | 214,16 | 18:43 | **+0,98%** | +1,08% | +0,40% | 83,7 |
| SMH (semiconduttori) | 550,965 | 18:43 | -2,44% | **-9,27%** | -1,72% | 61,5 |
| XLK (tecnologia) | 187,72 | 18:43 | -1,42% | -5,09% | -0,18% | 84,9 |

- QQQ meno RSP: **-1,76 punti percentuali** oggi (ieri -1,40), -4,33 su cinque giorni.
- NQ=F e' 4,95% sotto il massimo a un anno (31.483,25).
- Le otto grandi (18:43): **AAPL +0,41% (nuovo massimo a un anno, 344,35)**, MSFT +1,13% (pct 98,4),
  GOOGL +0,07%, AMZN -0,49%, META -0,76%, TSLA -2,08%, NVDA -2,11%, AVGO -2,42%. **Tre su otto in
  salita** (ieri una). Su cinque giorni: AVGO -6,18%, NVDA -6,00%, MSFT +3,27%.
- **Trimestrali** (api.nasdaq.com, EPS atteso): TSMC stamattina, "record quarter" (CNBC 17:51) e
  "earnings strong" (IBD 14:10), solo titoli; NFLX 20/10 dopo la chiusura (0,82 $); META (6,35 $,
  era 6,39) e TSLA (0,21 $) 21/10 dopo; GOOGL (2,94 $) e MSFT (4,71 $) 28/10 dopo, il giorno del
  FOMC; AMZN (2,02 $, ora non data) e INTC (0,27 $) 29/10.

### Il sentiment

CNN Fear & Greed (0-100 da sette misure; sotto 45 "fear"): **29,3, "fear"** alle 18:43. Una
settimana fa 39,8 (-10,5), un mese fa 28,0.

### Il calendario

Fonte: Forex Factory (attesa e precedente), ora italiana; NQZ6 da `ore-NQZ6.json` (etichetta = inizio
dell'ora; l'ora delle 14 contiene le 14:30).

| quando | evento | attesa | precedente | uscito | NQZ6 nell'ora |
|---|---|---|---|---|---|
| 14/10 14:30 | CPI a/a · core a/a | 3,6% · 2,5% | 3,4% · 2,4% | 3,7% · 2,6% (non verificato) | -205,75 (-0,67%) |
| 15/10 14:30 | PPI m/m · core m/m | 0,6% · 0,3% | 0,4% · 0,2% | **m/m non trovato; 6,3% a/a** (titolo CNBC 18:15) | ora delle 14: 29.816,5 -> 29.790 (-26,5); max 29.876,75, min 29.711 (escursione 165,75); volume 52.997, delta +1.223 |
| 15/10 14:30 | vendite al dettaglio m/m · core | 0,3% · 0,6% | 1,2% · 1,4% | non trovato | come sopra |
| 15/10 14:30 | Philly Fed manifatturiero | 28,2 | 37,8 | non trovato | come sopra |
| 15/10 14:30 | sussidi di disoccupazione | 199 mila | 197 mila | non trovato | come sopra |
| 16/10 05:30 | discorso del presidente della Fed Warsh | | | | |
| 16/10 21:30 | COT, posizioni di martedi' 13 | | | | |

- **Cinque ricerche WebSearch** (bls.gov, Census, stampa) non hanno restituito nessun comunicato
  del 15/10: le pagine trovate sono anteprime o anni passati. Il 6,3% del PPI viene solo dal titolo
  RSS di CNBC in `macro.json` ("Wholesale prices jumped 6.3% annually in September, biggest gain
  since 2022"); per confronto, il PPI di agosto era +5,4% a/a (BLS, citato da trade.gov.tr). I titoli
  confermano la direzione: "hot PPI print" (Seeking Alpha 14:57), "Hot PPI shows Fed's work is not
  done" (CNBC 17:37).
- **NQZ6 dopo le 14:30**: ora delle 15 (apertura della cassa alle 15:30) 29.790 -> 29.688 (-102),
  **minimo della giornata 29.622,5**, volume 191.688, delta +2.416; ora delle 16 29.688,25 ->
  29.971,25 (**+283**), volume 207.862, delta +4.594; ora delle 17 +42,25, massimo 30.061,75; ora
  delle 18 (44 minuti) -67, delta -222. Dal minimo al massimo: +439,25 punti.
- **La giornata dalle 00:00** (`oggi_finora`): apertura 30.154,75, massimo 30.158,5, minimo
  29.622,5, ultimo 29.946,25; volume 876.764, **delta +7.798**; valore 29.700-29.950, POC 29.800.
  Rispetto alla chiusura di cassa del 14/10 (30.185): **-238,75 (-0,79%)**.

### Le notizie e le ore di NQ

Fonte: RSS di CNBC e Google News in `macro.json` (titoli, senza testo), ore di NQZ6 dal bridge.

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 12:19-14:15 | Samsung giu', memorie "hammered" (Cina, eccesso d'offerta 2029), "AI sell-off continues on Anthropic news" (IBD 14:10) | ora delle 12: -50; ora delle 13: +15 |
| 14:30-15:16 | PPI caldo; "yields tumble despite a hot PPI print" (14:57); "Dow, Nasdaq futures diverge" (15:16); "neocloud bonds blow up" (IBD 14:42) | ora delle 14: -26,5 (escursione 165,75); ora delle 15: -102, minimo 29.622,5 |
| 16:49 | "Investors turn to safety of Treasurys as tech stocks extend selloff" (WSJ) | ora delle 16: **+283**, delta +4.594 |
| 18:02-18:19 | "yields fall as risk aversion beats inflation fears" (WSJ); PPI +6,3% a/a (CNBC 18:15); "Nvidia, AMD, Dell sell-off in global tech rout on AI funding worries" (CNBC 18:19) | ora delle 18: -67 in 44 minuti |
| 18:19 | "Fed may skip October but pull rate hike trigger in December" | Polymarket ottobre: rialzo 27,5% |

**Rumore** (notizia senza movimento orario coerente): Reuters 17:49 "Nasdaq slides to near
one-month low" mentre NQZ6 nell'ora delle 17 sale di 42 e sta 390 punti sopra il minimo (il titolo
descrive il livello rispetto a ieri, non l'ora); IEA 18:37, 20 milioni di barili di diesel (WTI
+1,62% sul giorno, nessun movimento di NQ legato); BNY sul 10 anni (15:25); oro in rimbalzo (15:47);
Southwest, JetBlue, banche ed Epstein, tassi dei CD e dei mutui.

## Cosa Dicono Per NQ

1. **Inflazione e tassi lunghi si sono separati.** Il PPI caldo (titolo) sposta Polymarket verso un
   rialzo il 28/10 (24,5% -> 27,5%), ma il 10 anni scende di 10 pb, il TLT sale dell'1,51% e l'oro
   dell'1,52%. I titoli lo chiamano fuga verso la qualita'. **Osservazione**: senza il 2 anni di oggi
   (curva ferma al 14/10) non si vede se la parte corta abbia seguito la Fed o il 10 anni.
2. **La vendita e' settoriale, e oggi ancora di piu'.** NQ=F -0,86% contro ES=F -0,05%; RSP +0,98%
   contro QQQ -0,78%; SMH -2,44% e -9,27% in cinque giorni. Dentro le otto grandi si dividono
   l'hardware dell'AI (NVDA, AVGO) e il resto (AAPL al massimo dell'anno, MSFT +1,13%).
   **Osservazione**: nessun documento del metodo misura la rotazione; e' una lettura dei rendimenti
   relativi.
3. **La volatilita' non ha seguito il prezzo.** NQ ha fatto il minimo di un mese e il VIX e' sceso
   (17,52 -> 16,94, percentile 47,6), in contango con VIX9D/VIX3M 0,79. La soglia d'inversione non e'
   scattata. **Osservazione**, con due limiti: VXN fermo a ieri e VIX3M forse non aggiornato.
4. **Il credito resta l'anello debole dell'AI.** HYG rimbalza appena dal minimo (percentile 1,6) e i
   titoli parlano di obbligazioni "neocloud" (i fornitori di calcolo per l'AI) in caduta e di "AI
   funding worries". **Osservazione**: manca lo spread high yield (FRED), e HYG e' un indice largo.
5. **Il prezzo ha assorbito il dato.** NQZ6 ha toccato 29.622,5 nell'ora dell'apertura di cassa e
   poi ha recuperato 439 punti con delta positivo in ogni ora fino alle 18 (giornata +7.798). La
   lettura di chi compra e di come: fase 4. **Osservazione** sulle barre orarie.
6. **Prossima prova: Warsh alle 05:30 di domani**, dopo un CPI e un PPI sopra l'attesa, con il
   mercato al 27,5% per un rialzo il 28/10.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 10A 5,311% | ^TNX, massimo a un anno | posizionamento dei tassi: oltre, il 10A esce dal massimo dell'anno (oggi 5,087%) |
| VIX9D 18,79 (= VIX3M) | Yahoo, 18:28 | soglia della struttura: sopra, la curva del VIX si inverte (oggi 14,87) |
| HYG 76,81 | Yahoo, minimo a un anno (14/10) | posizionamento del credito: sotto, nuovo minimo dell'anno (oggi 77,035) |
| DXY 102,58 | Yahoo, massimo a un anno | posizionamento del dollaro: sopra, nuovo massimo dell'anno (oggi 102,108) |
| NQZ6 30.185 | chiusura della cassa 14/10, bridge | riferimento del gap: alle 18:44 -238,75 |
| NQZ6 29.622,5 | minimo di oggi, ora delle 15, bridge | estensione della discesa iniziata con il CPI: per la fase 4 |
| NQZ6 30.061,75 | massimo del rimbalzo, ora delle 17, bridge | estremo del recupero dopo i dati: per la fase 4 |
| NQZ6 29.700-29.950, POC 29.800 | valore di oggi dalle 00:00 (70% dal POC), bridge | dove si e' scambiato di piu' oggi: per la fase 4 |
| 16/10 05:30 | discorso di Warsh (Forex Factory) | evento alto, in piena notte europea |
| 16/10 21:30 | COT, posizioni di martedi' 13 | contesto per la fase 2 |
| 20-21/10 | NFLX; META e TSLA, dopo la chiusura | trimestrali di due delle otto grandi |
| 28/10 19:00 | FOMC; poi GOOGL e MSFT dopo la chiusura | evento alto: decisione sui tassi e due grandi lo stesso giorno |

## Cosa Non Sappiamo

- **I dati delle 14:30 non sono verificati**: cinque ricerche senza un comunicato. Del PPI c'e' solo
  il 6,3% annuo da un titolo CNBC (non il m/m, non il core); vendite al dettaglio, Philly Fed e
  sussidi mancano del tutto. Il CPI di ieri resta non riverificato.
- **La Fed vista solo da Polymarket**: niente future sui fed funds; le regole del mercato "tasso a
  fine 2026" non sono nei dati.
- **VXN fermo al 14/10 22:00 e VIX3M identico a ieri** (18,79, variazione 0,0): la struttura e il
  confronto VXN-VIX potrebbero non essere di oggi.
- La variazione a un giorno di NQ=F (Yahoo) non dichiara la chiusura di riferimento; dal bridge,
  -0,79% sulla chiusura di cassa.
- L'ora delle 15 mescola mezz'ora prima e mezz'ora dopo l'apertura della cassa: il minimo 29.622,5
  non si attribuisce a una delle due.
- L'abbinamento notizia-ora e' una coincidenza temporale: non prova la causa.
- lacuna del collettore: il dato uscito degli eventi del calendario (Forex Factory da' solo attesa e
  precedente; WebSearch non trova i comunicati del giorno stesso).
- lacuna del collettore: FRED (fed funds effettivo, inflazione attesa, spread high yield, stress
  finanziario), saltato senza `FRED_API_KEY`.
- lacuna del collettore: i rendimenti intraday per scadenza (2A, 30A); c'e' solo ^TNX.
- lacuna del collettore: VXN e VIX3M intraday affidabili (VXN non aggiornato in giornata).
- lacuna del collettore: probabilita' FOMC dai future sui fed funds (ZQ) o da CME FedWatch.
- lacuna del collettore: l'esito delle trimestrali (TSMC: solo titoli).
- lacuna del collettore: l'ampiezza del Nasdaq-100 (titoli in salita contro titoli in calo).
- lacuna del collettore: il testo delle notizie (solo titoli RSS).
- lacuna del collettore: il calendario della settimana prossima (Forex Factory 404).
- lacuna del collettore: `raccolta.json` non ancora riscritto per questa raccolta (errori e ore
  della raccolta serale da confermare).

## Per Il Quadro

1. **Paura sulla tecnologia, non sul mercato** (RSP +0,98%, ES piatto, NQ -0,86%; Fear & Greed 29,3):
   il netto dei Non-Commercial su NQ della fase 2 va letto contro una rotazione, non contro una
   vendita generale.
2. **VIX in calo e contango con NQ al minimo di un mese** (VIX9D/VIX3M 0,79): coerente con il segno
   della gamma dei dealer della fase 3, o la gamma e' negativa e il contango e' fragile?
3. **Minimo 29.622,5 all'apertura e +439 con delta +4.594 nell'ora delle 16**: la fase 4 dica se il
   recupero e' aggressione in acquisto o chiusura di vendite, e se il valore 29.700-29.950 regge.
4. **Warsh alle 05:30 e Polymarket al 27,5% per un rialzo il 28/10**: i livelli di gamma e di order
   flow vanno letti sapendo che la notte ha un evento alto e che i tassi lunghi oggi hanno fatto da
   rifugio, non da minaccia.
