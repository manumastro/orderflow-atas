# Fase 1 · La Macro · 2026-11-03

**Stato**: parziale (`esiti` vuoto: JOLTS dal feed CNBC, non verificato al BLS; i conti di Apple da una ricerca web)
**Dati**: `dati/macro.json`, `dati/ore-NQZ6.json`, `dati/raccolta.json` (raccolta 16:47 ora italiana, nessun errore). NQ, ES, dollaro, petrolio, oro e titoli Yahoo di oggi 16:36-16:47; VIX, VIX9D, VIX3M, VXN, 10 anni di oggi 16:31; future fed funds di oggi 16:30-16:35; Treasury al 2/11; FRED giornalieri al 30/10; mensili a settembre. Barre orarie NQZ6 dal bridge fino alle 16:47 (l'ultima aperta, 47 minuti). Ora solare in Italia e negli USA: cassa 15:30-22:00 italiane.

**In una riga**: tassi alti con la Fed attesa al rialzo il 9 dicembre; volatilita' del Nasdaq alta, paura a breve appena sopra quella a tre mesi;
dollaro forte, credito che si allarga, paura estrema; i soldi escono dai grandi della tecnologia: un contesto di rischio per NQ.

## In Parole Semplici

**Il punto**: il Nasdaq ha perso il 10% in un mese, per motivi suoi (chip e giganti dell'AI), non per i tassi.
Oggi rimbalza poco, con i chip su e Apple giu'. Stanotte arrivano i risultati delle elezioni, a mercato dei
future aperto, e stasera i conti di AMD.

**Dal piu' ampio al piu' vicino**

1. **Il mondo dei tassi**: il 10 anni USA e' al 4,95%, era al 5,28% un mese fa; un rialzo della Fed il 9 dicembre e' dato fra il 74 e il 90%.
   → I soldi costano cari e la Fed stringe, ma i titoli di Stato non sono la causa del calo del Nasdaq.
2. **La paura**: l'indice della paura sul Nasdaq e' a 30, piu' alto che nel 95% dei giorni dell'anno; l'umore del mercato e' "paura estrema".
   → Ci si aspettano movimenti ampi, e la paura a nove giorni ha superato di poco quella a tre mesi: le elezioni pesano.
3. **Petrolio, oro, credito**: petrolio a 96 dollari, giu' due giorni sulle speranze di un accordo con l'Iran; oro al minimo dell'anno; prestiti rischiosi piu' cari.
   → L'inflazione resta il problema; chi presta alle aziende fragili chiede di piu' da una settimana.
4. **Chi guida**: in un mese il Nasdaq perde il 10%, l'S&P il 3,5%, l'azione media dell'S&P guadagna quasi il 4%.
   → Escono soldi dai giganti della tecnologia, non dalla Borsa intera.
5. **Ieri e oggi**: ieri NQ e' sceso di quasi 800 punti in cassa, senza una notizia che lo spieghi; Apple ha battuto le stime ma ha deluso sul margine futuro, e oggi perde quasi il 3%. Le offerte di lavoro (JOLTS) escono molto sopra l'atteso.
   → Il lavoro forte sostiene la Fed che alza; il rimbalzo di oggi e' dei chip, non del Nasdaq intero.

**Quando**: oggi dopo le 22:00 AMD · notte 3-4 risultati delle elezioni · mer 4 alle 16:00 ISM servizi, dopo le 22:00 Qualcomm · ven 6 alle 14:30 il rapporto sul lavoro
**Quanto e' provato**: i numeri sono misurati; il legame fra notizie e prezzo e' un'osservazione; nessun dato dice come il Nasdaq reagira' alle elezioni.

**Le parole**: **indice della paura (VXN)** = quanto le opzioni si aspettano che si muova il Nasdaq nel prossimo mese; **JOLTS** = l'indagine sui posti di lavoro offerti dalle aziende; **margine lordo** = la parte dei ricavi che resta dopo il costo di produzione; **cassa** = la seduta di Wall Street, 15:30-22:00 italiane.

## I Fatti

**Tassi** (Treasury, curva par al 2/11, tra parentesi il 2/10, 20 sedute prima): 3 mesi 4,21% (4,19); 2 anni 4,59% (4,83);
10 anni 4,91% (5,28); 30 anni 5,28% (5,63). Pendenze 10A-2A +0,32 punti (+0,45), 10A-3M +0,70 (+1,09): la curva si
appiattisce scendendo dal lungo. Oggi 16:31 (Yahoo `^TNX`) 10 anni 4,952%, +5,8 pb sulla chiusura; future sul 2 anni
`ZT=F` -0,07 (rendimento in salita). FRED: fed funds effettivo 3,88% al 30/10 (forchetta 3,75-4,00); breakeven 10 anni 2,29% al 2/11.

- **FOMC del 9 dicembre**: future sui fed funds (oggi 16:30-16:35) +22,4 pb attesi, **89,6%** di un rialzo di 25 pb
  (metodo CME FedWatch, `fomc_future.metodo`; ieri nel rapporto del 2/11: 82,7%). Polymarket: +25 pb 73,5%, fermi 24,5%;
  tasso a fine 2026 al 4,25% per il 71,9%; nessun taglio nel 2026 al 97,6%; "surriscaldamento" a fine 2026 al 95,3%.

**Volatilita'** (Yahoo, oggi 16:31; variazione sulla chiusura di ieri; percentile a un anno)

| indice | ultimo | var. 1g | 20g | percentile |
|---|---|---|---|---|
| VXN (Nasdaq-100) | 30,30 | -1,9% | +43% | 94,8 |
| VIX (S&P 500) | 20,63 | -2,5% | +37% | 84,6 |
| VIX9D (9 giorni) | 21,53 | -4,1% | +79% | 86,5 |
| VIX3M (3 mesi) | 21,27 | -1,5% | +21% | 75,8 |

Struttura: VIX9D 21,53 sopra VIX3M 21,27, **invertita di poco** (ieri 22,46 contro 21,60); il VIX sta sotto entrambi. VXN/VIX = 1,47.
Realizzata di NQ (bridge, rendimenti a 5 minuti annualizzati): 4 ore 33,7%, 24 ore 23,5%, 48 ore 20,2%; 20 giorni Yahoo 21,8% (ES 11,0%).

**Dollaro, petrolio, oro, credito** (Yahoo oggi 16:36-16:46; 1g / 20g / percentile a un anno): DXY 101,91 (+0,16% / +0,08% / 94);
WTI 96,25 (-0,22% / +7,61% / 80; 5g -3,65%); oro 3.938,4 (-0,79% / -5,94% / 0,8, minimo di un anno); TLT 81,18 (-0,67% / +5,05% / 11);
HYG 76,97 (+0,10% / -0,39% / 3,2). FRED: spread high yield 3,44 punti al 30/10 (+0,18 in un giorno, percentile a due anni 94); stress St. Louis -0,58 al 23/10.

**Chi guida** (Yahoo oggi 16:36-16:47)

| | ultimo | 1g | 5g | 20g |
|---|---|---|---|---|
| NQ=F | 28.298,75 | +0,61% | -5,08% | -10,11% |
| ES=F | 7.594,25 | +0,26% | -2,11% | -3,55% |
| RSP equipesato | 220,35 | +0,17% | +1,30% | +3,77% (percentile 95) |
| SMH chip | 521,18 | +2,53% | -6,60% | -17,60% |

Le grandi, 1g / 20g: NVDA +2,7 / -13,3; AAPL -2,8 / -1,4; MSFT +1,4 / -2,8; GOOGL -0,6 / -20,2 (percentile 1,2); AMZN +0,2 / -3,9;
META +1,8 / -16,4; AVGO +3,6 / -11,2; TSLA -0,7 / +5,3. Ampiezza Nasdaq-100 (api.nasdaq, 16:46): 55 salgono, 46 scendono;
media +0,76%, mediana +0,26%, pesata per capitalizzazione +0,69%.

**Apple** (trimestre a settembre, comunicato SEC 8-K via ricerca web): ricavi 114,5 mld $ (+12%), EPS 2,10 $ contro 1,98 atteso (0,05 dai
rimborsi sui dazi), margine lordo 47,9%. CNBC 03/11 15:38: la previsione sul margine delude, per la crisi delle memorie.

**Macro USA**: JOLTS settembre **7,52 mln**, +333 mila su agosto, contro 7,15 atteso (CNBC 16:37, citando il BLS; non verificato alla fonte).
FRED, settembre: CPI 3,72% a/a, core 2,57%; PPI 6,35% a/a; vendite +0,77% m/m; sussidi 194 mila al 24/10 (percentile a due anni 2,9).
**Sentiment** (CNN Fear & Greed): 20,1, paura estrema; una settimana fa 32,9, un mese fa 38,2.

**Le ore di NQZ6** (bridge, barre orarie, ora italiana)

| ora | fatto | NQZ6 nell'ora |
|---|---|---|
| 02/11 15:00-22:00 | cassa di ieri, nessuna notizia propria nei feed | massimo 28.891,25 (ora 16), minimo 28.101,75 (ora 21): circa -790; delta orario negativo in tutte le ore |
| 02/11 22:00 circa | conti di Apple dopo la chiusura | ora 22: 28.137,5 -> 28.223,75 (+86), massimo 28.270, delta -716 |
| 03/11 02:04 | Moomoo: "il rimbalzo dei chip alza i future del Nasdaq" | ora 02: +54; poi la notte scende a 28.038,25 (ora 08) |
| 03/11 14:00-15:47 | apertura con il petrolio giu' e le elezioni in vista | ora 14: massimo 28.426,5, +89; ora 15 (apertura della cassa): -31, delta -2.272 |
| 03/11 16:00 | JOLTS 7,52 mln contro 7,15 | ora 16 (47 minuti): 28.286 -> 28.341,75, minimo 28.248, massimo 28.389, delta -1.475 |

## Cosa Dicono Per NQ

- **La pressione resta settoriale.** In 20 sedute il 10 anni scende di 37 pb e TLT sale del 5%, eppure NQ perde il 10% e
  SMH il 17,6%, mentre RSP sale del 3,8%. **Osservazione**: rotazione fuori dai grandi della tecnologia, coerente con i
  dubbi sulle spese per l'AI (GOOGL il 28/10, AMZN e INTC il 29/10, in `eventi.json`).
- **Ieri sera**: la discesa di circa 790 punti fra le 16 e le 22 **non ha una notizia nei feed**. Un titolo (EnergyNow 02/11
  23:15) parla del 30 anni "al massimo di vent'anni", ma il Tesoro lo da' a 5,28%, sotto il 5,31% del 30/10. Causa non trovata.
- **Apple**: la reazione nei future e' stata piccola (+86 nell'ora dei conti); il colpo arriva oggi in cassa (AAPL -2,8%,
  11,1% del Nasdaq-100), compensato dai chip (NVDA +2,7%, AVGO +3,6%). Mediana +0,26% contro media +0,76%: il rimbalzo
  di oggi e' di pochi grandi titoli. **Osservazione**.
- **Il lavoro e la Fed**: JOLTS forte, sussidi bassi, "surriscaldamento" al 95% su Polymarket; il rialzo a dicembre
  sale dall'82,7% di ieri all'89,6% dei future. Logan (Dallas Fed, WSJ 03/11 09:27) chiede almeno altri due rialzi.
  Il 10 anni sale di 6 pb oggi. Il rapporto sul lavoro di venerdi' resta la verifica.
- **La volatilita'**: VXN al 95° percentile, VIX all'85°: la paura e' sul Nasdaq. La paura a nove giorni sopra quella a
  tre mesi e' coerente con un evento vicino (le elezioni); si e' ridotta da ieri. **Osservazione**.
- **Elezioni e Iran**: i Democratici sono favoriti (CNBC 16:34), e una loro vittoria piena al Congresso "potrebbe scuotere
  le azioni" (CNBC 14:28). USA e Iran aperti a una soluzione diplomatica (CNBC 16:46), petrolio giu' da due giorni. "Attacchi
  in Iran che fanno volare il petrolio" (24/7 Wall St 15:17): WTI oggi -0,22%, il prezzo non lo conferma. **Rumore**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.038,25 | minimo della notte del 3/11, ora 08 (bridge) | sotto, il rimbalzo di oggi e' annullato |
| 28.891,25 | massimo di ieri, ora 16 (bridge) | sopra, la discesa di ieri in cassa e' recuperata |
| 03/11 dopo le 22:00 | trimestrale AMD (EPS att. 1,68 $) | chip, il motore del rimbalzo di oggi |
| notte 3-4 | risultati delle elezioni di meta' mandato, future aperti | rischio politico su tassi e tecnologia |
| 04/11 16:00 · dopo le 22:00 | ISM servizi (att. 54,8, prec. 54,9) · QCOM (1,46 $) | crescita, chip |
| 05/11 14:30 | sussidi (att. 197 mila, prec. 194) | lavoro |
| 06/11 14:30 · 16:00 | occupati non agricoli (att. 65 mila, prec. 29), disoccupazione 4,2%, salari +0,3% · Michigan (att. 48,1) | **l'evento della settimana per la Fed** |
| 09/12 | FOMC (rialzo 25 pb all'89,6% dai future) | la prossima decisione |

## Cosa Non Sappiamo

- **lacuna del collettore: `esiti` vuoto senza il perche'**: il JOLTS delle 16:00 viene da una notizia CNBC; la ricerca web non lo conferma al BLS.
- **lacuna del collettore: l'esito delle trimestrali passate**: solo le prossime (3 su 14 cercate); Apple da ricerca web, senza la guida di dicembre in cifre.
- **lacuna del collettore: niente mercati di previsione sulle elezioni** (Polymarket: solo Fed, inflazione, recessione, dazi) ne' orari dei seggi.
- **lacuna del collettore**: `2YY=F` fermo al 13/10; calendario della settimana prossima 404; nessun feed sull'Iran (due notizie sul petrolio si contraddicono).

## Per Il Quadro

1. VXN al 95° percentile e paura a nove giorni sopra quella a tre mesi: coerente con l'ampiezza attesa dalle opzioni (fase 3) per la notte delle elezioni?
2. RSP +3,8% contro NQ -10,1% in 20 sedute, paura estrema: le posizioni degli speculatori su NQ (fase 2) seguono la rotazione o la contraddicono?
3. Rialzo Fed a dicembre dall'82,7% all'89,6% in un giorno, con JOLTS forte: il 10 anni +6 pb oggi pesa sull'order flow della cassa (fase 4)?
4. Le date di rischio: AMD stasera, elezioni nella notte 3-4 a future aperti, ISM servizi mercoledi', lavoro venerdi' 14:30.
