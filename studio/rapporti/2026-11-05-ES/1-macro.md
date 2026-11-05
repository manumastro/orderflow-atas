# Fase 1 · La Macro · ESZ6 · 5 novembre 2026

**Stato**: parziale (manca il dato uscito di JOLTS del 03/11 e ISM servizi del 04/11; il 2 anni in tempo reale c'e' solo come prezzo del future)
**Dati**: `macro.json`, `ore-ESZ6.json`, `raccolta.json`; raccolta del 05/11 alle 16:16 ora italiana, nessun errore. Freschezza: quotazioni Yahoo di oggi 16:01-16:16; curva Treasury del 04/11 (1 giorno); FRED giornalieri al 03-04/11, mensili a settembre, stress al 30/10, bilancio Fed al 28/10; ampiezza Nasdaq-100 alle 16:15; barre ES dal bridge fino alle 16:17 (l'ultima aperta, 17 minuti). `2YY=F` fermo al 13/10. Calendario della settimana prossima non pubblicato (404).

**In una riga**: tassi alti con la Fed attesa in rialzo a dicembre ma rendimenti lunghi in calo da un mese, paura in risveglio con la struttura quasi piatta, dollaro forte e fermo, credito debole ma calmo; il ribasso di oggi e' una vendita di tecnologia e chip, e l'S&P scende per il peso dei grandi mentre il titolo medio regge.

## In Parole Semplici

**Il punto**: oggi ES scende di circa il 2% perche' crollano tecnologia e chip, non perche' salgono i tassi: i rendimenti scendono, l'oro sale, i settori difensivi salgono. Il titolo medio dell'S&P perde meno dell'1%. Domani alle 14:30 escono i dati sul lavoro, con una Fed che parla di rialzi.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: la Fed e' attesa alzare il costo del denaro il 9 dicembre (circa 8 probabilita' su 10), ma il rendimento a 10 anni e' sceso da 5,27% a 4,90% in un mese.
   → Il denaro resta caro, ma il mercato dei titoli di Stato non vede un'accelerazione dei tassi.
2. **La paura**: l'indice della paura sull'S&P (VIX) e' salito da 16,6 a 20,4 in un giorno; quello sul Nasdaq (VXN) e' a 30.
   → La paura e' tornata ed e' concentrata sulla tecnologia; quella a 9 giorni ha raggiunto quella a 3 mesi.
3. **Chi scende**: chip -7,8%, tecnologia -5,4%; salute, consumi di base, energia e utility salgono.
   → E' una rotazione dalla tecnologia ai settori difensivi; ES la subisce in parte (meta' del calo di NQ), NQ per intero.
4. **Oggi**: alle 14:30 i sussidi di disoccupazione sono usciti migliori dell'atteso e ES non si e' mosso. Il crollo e' arrivato con l'apertura di Wall Street alle 15:30.
   → Nessun dato economico spiega il ribasso: lo spiegano la vendita sull'intelligenza artificiale e, forse, le parole di due membri della Fed sui rialzi.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.740 | i massimi di ieri e della notte, prima del crollo | il ribasso di oggi sarebbe riassorbito |
| circa 7.490 | il minimo della notte delle elezioni (04/11, 04:00) | il rialzo dopo le elezioni sarebbe cancellato per intero |

**Quando**: 06/11 14:30 occupati, disoccupazione e salari · 06/11 16:00 fiducia dei consumatori · 09/12 20:00 decisione della Fed
**Quanto e' provato**: i numeri sono misurati alle fonti; che la causa sia la tecnologia e non i tassi e' un'osservazione sui prezzi di oggi, non uno studio.

**Le parole**: **VIX / VXN** = quanto le opzioni scontano che l'S&P / il Nasdaq si muovano nei prossimi 30 giorni, la "paura"; **rendimento** = l'interesse che paga un titolo di Stato; **FOMC** = la riunione della Fed sui tassi; **percentile a un anno** = 90 vuol dire piu' alto del 90% dei giorni dell'ultimo anno.

## I Fatti

**Tassi** (pb = punti base, centesimi di punto percentuale)

| misura | valore | fonte, ora |
|---|---|---|
| Treasury 3M / 2A / 10A / 30A | 4,21 / 4,58 / 4,90 / 5,28% | home.treasury.gov, curva par del 04/11 |
| stessi, 20 sedute fa (06/10) | 4,21 / 4,79 / 5,27 / 5,64% | idem: 2A -21 pb, 10A -37 pb, 30A -36 pb |
| pendenze 10A-2A, 10A-3M | +0,32 e +0,69 punti (06/10: +0,48 e +1,06) | idem |
| 10 anni adesso | 4,868%, -2,6 pb da 4,894 | Yahoo `^TNX`, 16:01 |
| future Treasury 2 anni | 102,32, +0,05 (prezzo su = rendimento giu') | Yahoo `ZT=F`, 16:06 |
| fed funds effettivo | 3,88% al 03/11, percentile a 2 anni 48,8 | FRED `DFF` |
| rialzo di 25 pb al FOMC del 9/12 | 82,7% (variazione attesa +20,7 pb) | future ZQ del CBOT, 16:05, metodo CME FedWatch |
| Polymarket, dicembre | rialzo 25 pb 73,5%, fermo 24,5%; zero tagli nel 2026 al 97,9% | gamma-api.polymarket.com |
| inflazione attesa a 10 anni (breakeven) | 2,26% al 04/11, -4 pb, percentile a 2 anni 18,2 | FRED `T10YIE` |

**Inflazione e crescita** (FRED; m/m = sul mese prima, a/a = sull'anno prima)

| serie | ultimo | nota |
|---|---|---|
| CPI settembre | +0,65% m/m, 3,72% a/a | core +0,33% m/m, 2,57% a/a |
| PPI domanda finale settembre | +1,34% m/m, 6,35% a/a | core (senza alimentari, energia, commercio) +0,58%, 4,97% |
| vendite al dettaglio settembre | +0,77% m/m (agosto +1,29%), 6,29% a/a | senza auto +0,90% |
| sussidi, settimana al 31/10 | 189.000 (prima 195.000) | percentile a 2 anni 1,9: fra i piu' bassi |
| Philly Fed ottobre | 20,9 (settembre 37,8) | |

**Volatilita'** (Yahoo, 16:01; percentile a un anno)

| misura | oggi | ieri | percentile |
|---|---|---|---|
| VIX / VIX9D / VIX3M | 20,39 / 20,90 / 21,12 | 16,61 / 15,19 / 18,80 | 83,5 / 85,3 / 73,0 |
| VXN | 30,32 (massimo a un anno 33,54) | 25,08 | 94,4 |
| ES volatilita' realizzata | 31,0% ultime 4 ore; 19,4% 24 ore; 18,7% 48 ore | | bridge, log a 5 min, x276x252 |
| ES volatilita' 20 sedute | 14,2% annua | | Yahoo giornaliero |

**Dollaro, petrolio, oro, credito, liquidita'** (Yahoo 16:06-16:16; FRED): DXY 101,61, +0,1% (20g -0,52%, percentile 89,7). WTI 90,12, +2,27% (5g -11,65%). Oro 4.116, +1,06% (percentile 17,9). HYG 77,27, -0,33% (percentile 7,9); spread high yield 3,32 punti al 04/11, -8 pb (percentile a 2 anni 92,2). TLT 81,84, +0,1% (20g +5,1%). Stress St. Louis Fed -0,52 al 30/10 (sotto zero = sotto la media). Bilancio Fed 6.745,6 miliardi $ al 28/10, -8,0 in settimana.

**Chi guida** (Yahoo, variazioni %)

| | 1g | 5g | 20g | percentile 1a |
|---|---|---|---|---|
| ES=F (7.569, 16:06) | -1,96 | -2,39 | -3,16 | 69,0 |
| NQ=F | -4,03 | -7,27 | -11,48 | 47,6 |
| SPY / RSP (S&P equipesato) | -2,14 / -0,68 | -2,46 / +0,81 | -3,04 / +4,70 | 67,5 / 97,2 |
| SMH (chip) / XLK (tecnologia) | -7,75 / -5,44 | -17,18 / -11,57 | -23,40 / -15,61 | 45,6 / 49,6 |
| IWM (piccole) | -1,74 | -1,74 | +0,54 | 56,7 |

- **RSP meno SPY**: +1,46 / +3,27 / +7,74 punti a 1, 5, 20 giorni (il titolo medio batte i grandi).
- **Settori oggi**: 5 su 11 salgono, mediana -0,28%. Su: XLP +1,30, XLV +1,18, XLE +0,97, KRE +0,93, XLU +0,64. Giu': XLK -5,44, XLY -2,79, XLI -1,63. In 20 giorni 7 su 11 salgono (mediana +6,87%); XLV +14,11, XLP +11,65; XLF al percentile 99,6.
- **Grandi**: NVDA -6,71, TSLA -8,82, AVGO -6,33, META -4,88, AMZN -2,54, MSFT -2,00, GOOGL -1,61, AAPL +0,09 (al massimo dell'anno). Nasdaq-100 alle 16:15: 38 salgono, 63 scendono, mediana -1,88%, media pesata -3,7%.
- **Trimestrali** (api.nasdaq.com): BRK.B 06/11 (ora non data); NVDA 17/11 dopo la chiusura; HD 17/11 e WMT 19/11 prima dell'apertura.
- **Sentiment**: Fear & Greed 27,2 (paura); 30,8 una settimana fa, 43,1 un mese fa.

**Calendario** (Forex Factory; esiti da FRED; ES nell'ora dal bridge, ora italiana)

| ora | evento | attesa / precedente | uscito | ES nell'ora |
|---|---|---|---|---|
| 03/11 16:00 | JOLTS, posti vacanti | 7,15M / 7,08M | non nei dati | 7.591 -> 7.615, delta -1.406 |
| notte 03-04/11 | elezioni di medio termine | | vittoria democratica (notizie) | 01:00-05:00 da 7.602 a 7.490, poi fino a 7.744 il 04/11 17:00 |
| 04/11 16:00 | ISM servizi | 54,8 / 54,9 | non nei dati | 7.726 -> 7.714, range 7.698-7.740, delta -4.004 |
| 05/11 14:30 | sussidi | 197K / 194K | **189K** | 7.728 -> 7.724, range 15 punti, delta +540: nessuna reazione |
| 06/11 14:30 | occupati non agricoli; disoccupazione; salari orari m/m | 65K / 29K; 4,2% / 4,2%; 0,3% / 0,1% | | |
| 06/11 16:00 | fiducia Michigan; attese d'inflazione | 48,1 / 46,3; - / 4,7% | | |

**Notizie e prezzo** (titoli dei feed; ES nell'ora dal bridge)

| ora | notizia | ES nell'ora |
|---|---|---|
| 05/11 04:46 | Reuters: oro al massimo di 2 settimane, dollaro, rendimenti e petrolio giu' dopo il voto | 7.716 -> 7.706: rumore |
| 05/11 10:47 | WSJ: la vendita sull'intelligenza artificiale continua, i Treasury salgono | 7.715 -> 7.727: ES fermo, solo NQ cede |
| 05/11 12:01-14:00 | CNBC: il voto puo' frenare i data center; elettori contro i data center | 7.731 -> 7.725: rumore per ES |
| 05/11 14:55 / 15:29 | Hammack e Barkin (Fed): rialzi graduali in vista | 15:00-16:00: 7.724 -> 7.594, delta -17.408, volume 475.778 |
| 05/11 16:00-16:17 | (nessun titolo nuovo nei feed) | 7.594 -> 7.556, minimo 7.547,75, delta -8.955 |

## Cosa Dicono Per ES

1. **Il ribasso e' settoriale, non di tassi** (osservazione). Nell'ora del crollo il 10 anni scende (-2,6 pb), il future sul 2 anni sale di prezzo, l'oro sale, XLP, XLV, XLU salgono. Uno shock di tassi farebbe il contrario. La vendita sta su chip e grandi tecnologici; ES scende la meta' di NQ e RSP perde un terzo di SPY.
2. **I discorsi di Hammack e Barkin cadono nella stessa ora dell'apertura di New York** (osservazione). A risoluzione oraria non si separa l'effetto dell'apertura da quello dei discorsi; i rendimenti in calo dicono che il mercato dei titoli non li ha letti come una sorpresa.
3. **La Fed e' in una fase di rialzi**: fed funds effettivo 3,88%, rialzo a dicembre all'83% dai future e al 74% su Polymarket, nessun taglio nel 2026. Ma la curva e' scesa di quasi 40 pb sulle scadenze lunghe in un mese, e l'inflazione attesa e' al percentile 18. **Osservazione**: il mercato sconta tassi corti piu' alti e un'inflazione futura piu' bassa.
4. **La paura e' vicina**: VIX9D (20,90) ha superato il VIX (20,39) ed e' a un passo dal VIX3M (21,12); ieri la struttura era in contango largo (15,19 contro 18,80). La volatilita' realizzata di ES nelle ultime 4 ore (31%) e' sopra il VIX: oggi ES si muove piu' di quanto le opzioni a 30 giorni scontano.
5. **Credito debole ma calmo**: HYG al percentile 7,9 dell'anno e lo spread high yield al 92 di due anni, ma oggi -0,33% e lo stress St. Louis sotto zero. Non c'e' un segno di crisi di liquidita' nei dati.
6. **Domani**: con la Fed che parla di rialzi, gli occupati (attesi 65K) e i salari (+0,3%) toccano la probabilita' di dicembre. **In teoria** un dato forte la alza; nessuno studio qui lo prova su ES.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.735-7.746 | massimi del 04/11 (17:00-19:00) e della notte del 05/11 | il ribasso di oggi riassorbito: la rotazione non pesa piu' su ES |
| 7.490 | minimo della notte elettorale (04/11, barra delle 04:00) | il rialzo post-elettorale cancellato per intero |
| 7.874 / 6.388 | massimo / minimo a un anno di ES=F (Yahoo) | posizionamento: ES e' al percentile 69 dell'anno |
| 06/11 14:30 | occupati, disoccupazione, salari | il dato che tocca la probabilita' del rialzo |
| 06/11 16:00 | fiducia e attese d'inflazione Michigan (prelim) | inflazione attesa dalle famiglie, prima 4,7% |
| 06/11 | trimestrale BRK.B (ora non data) | il peso massimo non tech della settimana |
| 17/11 dopo le 22:00 | trimestrale NVDA | il titolo che guida la vendita di oggi |
| 09/12 20:00 | decisione FOMC (14:00 New York) | il rialzo scontato all'83% |

## Cosa Non Sappiamo

- **lacuna del collettore**: il dato uscito di JOLTS (03/11) e ISM servizi (04/11) non e' in `esiti`; due ricerche web non l'hanno trovato.
- **lacuna del collettore**: la fascia ufficiale dei fed funds e la data dell'ultimo rialzo non ci sono. I titoli si contraddicono: ABC (04/11 20:23) "la Fed alza i tassi per la prima volta dal 2023", WSJ (05/11 12:17) "la Fed ferma i tassi". Il future di dicembre (4,015%) dice che il 4 novembre non c'e' stato un rialzo.
- **lacuna del collettore**: il rendimento a 2 anni di oggi (`2YY=F` fermo al 13/10); la curva Treasury di oggi (c'e' solo quella di ieri).
- **lacuna del collettore**: le barre di ES sono orarie; il minuto d'inizio del crollo, per separare apertura e discorsi Fed, non c'e' in questa fase.
- **lacuna del collettore**: la decisione della Bank of England di oggi (titolo WTVB 15:31) non sta nel calendario, che e' solo USD; l'ampiezza dell'S&P per titolo non c'e' fra le fonti gratuite.
- I titoli dei feed sono sommari: "vittoria democratica" e "rialzo Fed" sono notizie, non dati verificati.

## Per Il Quadro

- La vendita e' su chip e grandi tecnologici, con rendimenti in calo: le opzioni (fase 3) e l'order flow (fase 4) vedono una caduta di ES guidata da NQ o un flusso proprio su ES?
- VIX9D sopra il VIX, a un passo dal VIX3M, e volatilita' realizzata a 4 ore al 31%: coerente con il segno dei market maker di X-Ray (gamma negativa amplifica i movimenti)?
- Fed attesa in rialzo all'83% mentre la curva lunga scende di 37 pb in un mese: coerente con le posizioni sui Treasury e sull'S&P del COT (fase 2)?
- RSP meno SPY +7,7 punti in 20 giorni e difensivi ai massimi dell'anno: le settimane di ES mostrano distribuzione sui massimi o una rotazione che lascia l'indice in range?
