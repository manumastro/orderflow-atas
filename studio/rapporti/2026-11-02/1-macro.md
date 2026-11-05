# Fase 1 · La Macro · 2026-11-02

**Stato**: parziale (manca `ore-NQ.json`: niente movimento orario accanto alle notizie; `esiti` vuoto)
**Dati**: `dati/macro.json`, `dati/raccolta.json` (raccolta 10:58 ora italiana, nessun errore). NQ, ES, dollaro, petrolio, oro, VIX, future fed funds di oggi 10:42-10:47; VXN, VIX9D, VIX3M, 10 anni e titoli al 30/10 21:15; Treasury al 30/10; FRED al 29/10; mensili a settembre. Ora solare USA dal 1/11: cassa 15:30-22:00 italiane.

**In una riga**: tassi alti ma in discesa da un mese, con la Fed attesa al rialzo a dicembre; volatilita'
del Nasdaq alta e in salita, ma non ancora in panico; petrolio vicino ai 100 dollari per la guerra con
l'Iran; credito che si allarga; le vendite colpiscono i grandi della tecnologia e i chip, il titolo medio tiene.

## In Parole Semplici

**Il punto**: il Nasdaq e' sotto pressione per motivi suoi (i chip e i conti dei giganti dell'AI), non per
i tassi, che anzi scendono. Questa settimana ha tre prove: Apple stasera, le elezioni di meta' mandato
martedi', il dato sul lavoro venerdi'. Sullo sfondo, la guerra con l'Iran tiene il petrolio vicino a 100.

**Dal piu' ampio al piu' vicino**

1. **Il mondo dei tassi**: il 10 anni USA e' al 4,94%, era al 5,24% un mese fa; la Fed e' attesa al rialzo il 9 dicembre (fra il 70 e l'83%).
   → I soldi costano cari, ma i titoli di Stato si sono calmati: la pressione non arriva da li'.
2. **La paura**: l'indice della paura sul Nasdaq (VXN) e' a 28,5, piu' alto che nell'88% dei giorni dell'ultimo anno.
   → Ci si aspettano movimenti ampi, il doppio dell'S&P 500; ma la paura a breve non supera quella a tre mesi.
3. **Petrolio, oro, credito**: petrolio a 99,6 dollari (+11% in un mese), oro al minimo di un anno, prestiti rischiosi un po' piu' cari.
   → L'inflazione resta il problema; chi presta alle aziende fragili chiede un po' di piu'.
4. **Chi guida**: in un mese il Nasdaq perde l'8%, l'S&P il 2%, l'azione media dell'S&P guadagna il 4%.
   → Escono soldi dai giganti della tecnologia e dai chip, non dalla Borsa in generale.
5. **Venerdi' e stanotte**: venerdi' NQ ha perso circa 640 punti nel giorno, dopo i conti deludenti di Amazon e Intel; stamattina recupera quasi l'1%.
   → Il rimbalzo viene con notizie di vigilia (petrolio giu', elezioni), non con un fatto nuovo.

**Quando**: oggi 16:00 indice manifatturiero ISM · oggi dopo le 22:00 Apple · mar 3 elezioni USA (risultati nella notte italiana) e AMD · ven 6 alle 14:30 il rapporto sul lavoro
**Quanto e' provato**: i numeri sono misurati; il legame fra notizie e prezzo e' un'osservazione, e per stanotte manca il dettaglio ora per ora.

**Le parole**: **VXN** = quanto ci si aspetta che si muova il Nasdaq nel prossimo mese, dalle opzioni; **contango** = la paura a lungo termine piu' alta di quella vicina, lo stato normale; **spread high yield** = quanto in piu' pagano le aziende fragili per farsi prestare soldi; **FOMC** = la riunione della Fed che decide i tassi.

## I Fatti

**Tassi** (Treasury, curva par al 30/10, tra parentesi l'1/10, 20 sedute prima): 3 mesi 4,16% (4,17); 2 anni 4,56% (4,78); 10 anni 4,94% (5,24); 30 anni 5,31% (5,61). Pendenze 10A-2A +0,38 punti (+0,46), 10A-3M +0,78
(+1,07). FRED: fed funds effettivo 3,88% al 29/10 (forchetta 3,75-4,00); breakeven a 10 anni 2,34% al 30/10.

- **FOMC del 9 dicembre**: dai future sui fed funds (oggi 10:46) +20,7 pb attesi, **82,7%** di un rialzo
  di 25 pb (metodo CME FedWatch, `fomc_future.metodo`). Polymarket: rialzo 25 pb 70,5%, fermi 27,5%;
  nessun taglio nel 2026 al 97%; "surriscaldamento" dell'economia a fine 2026 al 95,4%.

**Volatilita'** (Yahoo; percentile a un anno)

| indice | ultimo | ora | var. 1g | percentile |
|---|---|---|---|---|
| VXN (Nasdaq-100) | 28,50 | 30/10 21:15 | +13,2% | 88 |
| VIX (S&P 500) | 18,87 | 02/11 10:42 | -2,8% | 72 |
| VIX9D (9 giorni) | 19,25 | 30/10 21:15 | +21,3% | 82 |
| VIX3M (3 mesi) | 20,78 | 30/10 21:15 | +7,3% | 69 |

Struttura VIX9D < VIX < VIX3M al 30/10: **contango**, non invertita. Realizzata a 20 giorni, annua: NQ 22,1%, ES 11,4%. VXN/VIX = 1,47 (30/10).

**Dollaro, petrolio, oro, credito** (Yahoo oggi 10:47, ETF al 30/10; 1g / 20g / percentile a un anno):
DXY 101,59 (-0,29% / -0,57% / 91); WTI 99,57 (-4,37% / +11,34% / 86); oro 3.907,8 (+0,23% / -5,99% / 0,4,
minimo di un anno); TLT 81,84 (+0,43% / +5,63% / 16); HYG 77,43 (-0,19% / +0,68% / 8). FRED: spread high
yield 3,26 punti al 29/10 (+0,12 in un giorno, percentile a due anni 90); stress St. Louis Fed -0,58 al 23/10.

**Chi guida** (Yahoo; NQ=F e ES=F oggi 10:47, il resto chiusura 30/10)

| | ultimo | 1g | 5g | 20g |
|---|---|---|---|---|
| NQ=F | 28.788,75 | +0,89% | -2,20% | -8,08% |
| ES=F | 7.653,25 | +0,52% | -0,79% | -2,21% |
| QQQ | 690,49 | -3,47% | -1,97% | -7,88% |
| RSP equipesato | 218,31 | -0,79% | +0,82% | +4,09% (percentile 90) |
| SMH chip | 522,49 | -6,98% | -0,89% | -17,14% |

Le grandi al 30/10, 1g / 20g: NVDA -4,2 / -12,5; AVGO -5,5 / -8,2; GOOGL -1,4 / -18,5 (percentile 2,8);
META -1,5 / -15,5; AMZN -1,4 / -4,4; MSFT -1,6 / +0,9; AAPL -1,2 / +3,3 (percentile 98); XLK -4,9 / -10,2.

**NQ venerdi' 30/10** (orchestratore, dal bridge): cassa 28.798 -> 28.527 (-271); giorno CME 29.180 -> 28.543 (-637). Prima, giovedi' 29/10 21:00-21:06: 29.593 -> 29.111 sui conti di Amazon e Intel (`eventi.json`).

**Macro USA** (FRED, settembre): CPI 3,72% a/a, core 2,57%; PPI 6,35% a/a; vendite +0,77% m/m; sussidi 194 mila al 24/10 (percentile a due anni 2,9). **Sentiment** (CNN Fear & Greed): 26,1, paura; una settimana fa 29,8, un mese fa 38,2.

**Notizie** (feed, ora italiana; senza movimento orario: manca `ore-NQ.json`)

| ora | fonte | fatto |
|---|---|---|
| 02/11 04:15 | StoneX | il Nasdaq-100 cede su Amazon e Intel deludenti |
| 02/11 03:39 | CNBC | Rubio: Trump vuole chiudere la guerra con l'Iran "pacificamente"; le parti preparano attacchi dopo le elezioni |
| 02/11 01:41-10:27 | Reuters, CNBC | petrolio giu' per la "prudenza pre elezioni"; rendimenti misti |
| 02/11 06:01 | FT | "la mania dell'AI non sopravvive a tassi piu' alti" |
| 01/11 15:41 | Bloomberg | la ripresa del lavoro puo' tenere la Fed sulla strada dei rialzi |

## Cosa Dicono Per NQ

- **La pressione e' settoriale, non di tassi.** In 20 sedute il 10 anni scende di 30 pb e TLT sale del 5,6%,
  eppure NQ perde l'8% e SMH il 17%, mentre RSP sale del 4%. **Osservazione**: rotazione fuori dai grandi della
  tecnologia e dai chip, coerente con i dubbi sulle spese per l'AI (capex e flusso di cassa di GOOGL il 28/10,
  di AMZN il 29/10, in `eventi.json`).
- **Venerdi'**: le notizie spiegano la gamba di giovedi' sera e della notte (Amazon e Intel). La discesa in
  cassa di venerdi', con i chip a -7%, **non ha una notizia propria** nei dati, e la ricerca web non l'ha
  trovata. Resta un'**osservazione**: vendite concentrate sui chip (SMH -6,98%, RSP -0,79%).
- **La volatilita'**: VXN all'88° percentile contro VIX al 72°: la paura e' sul Nasdaq, non sul mercato
  intero. Il contango dice che non e' paura di brevissimo. Il credito va nel verso sbagliato, di poco
  (spread +12 pb in un giorno, HYG all'8° percentile).
- **La Fed resta un vento contrario**: rialzo a dicembre all'82,7% (future) e al 70,5% (Polymarket). Il
  rapporto sul lavoro di venerdi' pesa su quella probabilita' (Bloomberg 01/11).
- **La geopolitica**: la guerra USA-Iran (dal 28/02, fonte web secondaria) tiene il petrolio fra 90 e 113
  nell'anno; le mine a Hormuz del 21/10 lo hanno riportato a 99. Rubio parla di pace, ma le parti preparano
  attacchi **dopo** le elezioni: il rischio si sposta a mercoledi'-giovedi'. Il petrolio -4,4% di stamattina
  coincide con queste notizie e con il rimbalzo dei future: **osservazione**, senza il dettaglio orario.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.527 | chiusura della cassa NQZ6 del 30/10 (bridge) | sotto, il rimbalzo di stanotte e' annullato |
| 29.180 | apertura del giorno CME del 30/10 (bridge) | sopra, il venerdi' e' recuperato per intero |
| 29.593 | NQZ6 prima dei conti AMZN/INTC, 29/10 21:00 | sopra, la reazione ai conti e' assorbita |
| 02/11 16:00 · dopo le 22:00 | ISM manifatturiero (att. 54,5) · trimestrale AAPL (EPS att. 1,98 $) | AAPL e' il peso piu' grande del Nasdaq-100 (11,6%) |
| 03/11 16:00 · notte 3-4 | JOLTS (att. 7,11 mln) · elezioni di meta' mandato; AMD (1,68 $) | lavoro, rischio politico, chip |
| 04/11 16:00 · dopo le 22:00 | ISM servizi (att. 54,9) · QCOM (1,46 $) | crescita, chip |
| 05/11 14:30 | sussidi (att. 197 mila) | lavoro |
| 06/11 14:30 · 16:00 | occupati non agricoli (att. 63 mila, prec. 29), disoccupazione 4,2%, salari +0,3% · Michigan (att. 47,8) | **l'evento della settimana per la Fed** |
| 09/12 | FOMC (rialzo 25 pb all'82,7% dai future) | la prossima decisione |

## Cosa Non Sappiamo

- **lacuna del collettore: `ore-NQ.json` assente**: nessun movimento orario di NQ accanto alle notizie della
  notte e del venerdi'. I numeri di venerdi' vengono dall'orchestratore.
- **lacuna del collettore: `ampiezza` incoerente**: dice "Oct 30", 68 titoli su 100 in salita, NVDA +1,62%;
  Yahoo per lo stesso giorno da' NVDA -4,22% e QQQ -3,47%. Non e' usata.
- **lacuna del collettore: `esiti` vuoto** (l'ECI del 30/10 resta senza dato); `2YY=F` fermo al 13/10; il
  10 anni di oggi manca (ultimo 30/10 19:59).
- **lacuna del collettore: il calendario non ha l'annuncio trimestrale delle aste del Tesoro** (Bloomberg
  01/11) ne' la chiusura dei seggi; trimestrali trovate 3 su 14 cercate; settimana prossima 404.

## Per Il Quadro

1. VXN all'88° percentile e realizzata di NQ al doppio di ES: coerente con l'ampiezza delle opzioni (fase 3)?
2. RSP +4,1% contro NQ -8,1% in 20 sedute: le posizioni degli speculatori su NQ (fase 2) seguono la rotazione?
3. Rialzo Fed a dicembre all'82,7% con il 10 anni giu' di 30 pb: il lavoro di venerdi' e' la verifica. Le tre date di rischio (AAPL stasera, elezioni nella notte 3-4, lavoro venerdi' 14:30) vanno messe contro gamma e order flow.
