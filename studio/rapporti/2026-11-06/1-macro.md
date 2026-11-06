# Fase 1 · La Macro · 6 novembre 2026

**Stato**: parziale (mancano i salari orari di ottobre, la struttura della volatilita' di oggi, il Michigan, non uscito alla raccolta)
**Dati**: `macro.json`, `ore-NQZ6.json`, `raccolta.json`; raccolta delle 15:05 italiane, nessun errore. Yahoo di oggi 14:49-14:55
(future, VIX, 10 anni, dollaro, petrolio, oro, fed funds); VXN, VIX9D, VIX3M ed ETF alla chiusura del 5/11; curva Treasury al
5/11; FRED al 4-5/11, CPI e PPI a settembre; notizie fino alle 15:02; ultima barra NQZ6 aperta (6 min). `2YY=F` fermo al 13/10, non usato.

**In una riga**: Fed attesa alzare a dicembre (9 su 10 dopo un lavoro forte), tassi lunghi scesi per la corsa ai titoli di Stato,
volatilita' del Nasdaq al massimo dell'anno con la paura vicina sopra la lontana, dollaro forte, credito debole: rischio su chip e AI.

## In Parole Semplici

**Il punto**: ieri il Nasdaq ha avuto una delle giornate peggiori dell'anno, trascinato da chip e intelligenza artificiale.
Stanotte ha recuperato una parte. Il rapporto sul lavoro delle 14:30 e' uscito molto piu' forte dell'atteso.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: dopo il lavoro forte, la Fed e' attesa alzare il 9 dicembre con circa 9 probabilita' su 10 (ieri 8).
   → il denaro piu' caro pesa sui titoli che valgono per gli utili futuri, come i tech; e' un modello, non provato su NQ.
2. **La paura**: l'indice della paura sul Nasdaq ha chiuso ieri a 38, il valore piu' alto dell'anno.
   → il mercato si aspetta movimenti molto ampi; la paura per i prossimi 9 giorni supera quella a 3 mesi.
3. **Chi e' sceso**: ieri chip -11%, Nasdaq -6,5%, il titolo medio dell'S&P -2,9%.
   → la vendita e' partita dai grandi dell'intelligenza artificiale e a fine giornata si e' allargata.
4. **La notte**: in Asia i chip sono crollati (SK Hynix -11,7%), ma il future del Nasdaq e' risalito di quasi 900 punti dal minimo.
   → e' un recupero parziale: resta circa 1.450 punti sotto la chiusura di martedi'.
5. **Oggi**: 165 mila nuovi posti contro 65 mila attesi, disoccupazione 4,1%. Nell'ora del dato NQ si e' mosso poco.
   → il dato rafforza l'attesa del rialzo dei tassi; alle 16:00 arriva la fiducia dei consumatori.

**Quando**: oggi 16:00 fiducia dei consumatori · 21:30 posizioni sui future (COT) · 17/11 sera conti di Nvidia · 9/12 la Fed
**Quanto e' provato**: i numeri sono misurati (fonti sotto); il legame tassi-tech e' un modello di manuale; le cause del crollo (elezioni, Qualcomm, chip Nvidia fermi) non sono nei nostri feed, le riporta l'orchestratore.

**Le parole**: **cassa** = la seduta di Wall Street, 15:30-22:00 italiane; **indice della paura sul Nasdaq** (VXN) = la volatilita'
attesa a 30 giorni dalle opzioni; **percentile** = quanti giorni dell'anno stavano sotto; **punto base** = un centesimo di punto %.

## I Fatti

**Tassi** (curva par, home.treasury.gov, chiusura del 5/11; oggi il 10 anni Yahoo `^TNX` alle 14:49)

| | 3M | 2A | 10A | 30A | 10A-2A | 10A-3M |
|---|---|---|---|---|---|---|
| 5/11 | 4,17 | 4,46 | 4,84 | 5,25 | +0,38 | +0,67 |
| 4/11 | 4,21 | 4,58 | 4,90 | 5,28 | +0,32 | +0,69 |
| 7/10 (20 sedute fa) | 4,22 | 4,77 | 5,28 | 5,67 | +0,51 | +1,06 |

- Il 5/11 2A -12 pb, 10A -6: curva giu' e piu' ripida. In 20 sedute 2A -31, 10A -44. Oggi 10A 4,874% (+3,7 pb, dopo il lavoro); `ZT=F` -0,086 (rendimento a 2 anni su). Fed funds effettivo 3,88% (FRED 4/11); inflazione attesa 10A 2,24% (FRED 5/11).
- Fed del 9/12: rialzo di 25 pb all'89,6% (future ZQ, Yahoo 14:53, metodo FedWatch in `fomc_future`; ieri 82,7% alle 16:05);
  Polymarket rialzo 78,5% (ieri 73,5%), tasso a fine anno 4,25% al 72,4%. Inflazione (FRED, settembre): CPI +3,72% annuo, PPI +6,35%.

**Volatilita'** (Yahoo; percentile a un anno su chiusure giornaliere)

| | ultimo | ora | chiusura prima | percentile 1a |
|---|---|---|---|---|
| VXN | 38,26 | 5/11 22:15 | 25,08 | 100 (massimo dell'anno) |
| VIX | 26,40 | 6/11 14:49 | 28,18 | 96,1 |
| VIX9D / VIX3M | 32,66 / 25,55 | 5/11 22:15 | 15,19 / 18,80 | 100 / 94,8 |

- Il 5/11 VIX da 16,61 a 28,18 (+70%; il +55% del contesto torna con la VXN, +52,6%). VIX9D sopra VIX3M di 7,1: **struttura invertita** (paura vicina sopra la lontana); il 4/11 contango di 3,6. Di oggi solo il VIX, -1,8.
- Realizzata NQZ6 (bridge, rendimenti log a 5 minuti annualizzati): 4 ore 28,3%, 24 ore 48,5%, 48 ore 42,0%.

**Dollaro, petrolio, oro, credito, sentiment** (Yahoo 14:54 di oggi; HYG e TLT alla chiusura del 5/11): DXY 102,20 +0,6%
(pct 98,0, massimo 1a 102,58); WTI 91,02 +0,6% (5 giorni -12,6%); oro 4.057 -1,4% (pct 8,7); HYG 77,28 -0,3% (pct 8,4); TLT
81,93 +0,2%. Spread high yield 3,32 punti % (FRED, al 4/11). Fear & Greed 20, paura estrema (CNN; 7 giorni fa 26,3, un mese fa 47).

**Chi guida** (Yahoo; ETF e titoli alla chiusura del 5/11, future alle 14:54 di oggi)

| | 1 giorno | 5 giorni | 20 giorni |
|---|---|---|---|
| NQ=F 27.168,5 (oggi) | +1,56% | -4,79% | -12,67% |
| ES=F 7.399 (oggi) | +0,22% | -2,82% | -5,86% |
| QQQ (5/11) | -6,49% | -9,55% | -13,45% |
| RSP (S&P equipesato, 5/11) | -2,89% | -1,44% | +2,36% |
| SMH (chip, 5/11) | -11,21% | -20,29% | -26,27% |

- Le grandi il 5/11: TSLA -10,8%, AVGO -10,4% (minimo dell'anno), NVDA -9,3%, XLK -8,0%, META -4,0%, MSFT -3,6%, GOOGL e AMZN -2,2%, AAPL -1,5%.
- Nasdaq-100 (api.nasdaq.com; etichetta "Nov 5" ma valori di stamattina): 48 su, 53 giu', pesata +1,32%, mediana -0,36%; NVDA +4,2%, AVGO +5,7%. NVDA riporta il 17/11 sera.

**Calendario** (Forex Factory, ora italiana; esiti da FRED e CNBC; NQZ6 dal bridge, barra oraria)

| quando | evento | attesa | uscito | NQZ6 nell'ora |
|---|---|---|---|---|
| 5/11 14:30 | sussidi di disoccupazione | 197 mila | 189 mila (FRED) | 28.327 -> 28.308 |
| 6/11 14:30 | occupati non agricoli | 65 mila (CNBC: 59) | 165 mila (CNBC 14:58) | 27.098 -> 27.154 |
| 6/11 14:30 | disoccupazione; salari m/m | 4,2%; 0,3% | 4,1% (CNBC); salari non trovati | idem |
| 6/11 16:00 | fiducia Michigan; attese d'inflazione | 48,1; prec. 4,7% | dopo la raccolta | |

**NQZ6 ora per ora** (bridge): cassa del 4/11 chiusa a 28.566,75. Il 5/11: ora 15 -775 (volume 196 mila), ora 16 minimo 26.732 (volume 286 mila, il massimo delle 48 ore, delta -3.279),
ore 17-20 recupero con delta positivo, ora 21 -369 (delta -4.856, il piu' negativo); cassa chiusa a 26.746,25 (-6,4%), giornata CME a 26.604 (-6,9%).
Notte: minimo 26.366,5 (ora 00), 27.159,25 (ora 03), massimo 27.251,5 (ora 10); alle 15:06 27.115,75.

**Le notizie** (ora del feed; Google News non sempre affidabile sull'ora)

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 5/11 21:58 | Reuters: Hammack e Barkin (Fed) per rialzi graduali | ora 21: -369 (titolo dopo l'ora) |
| 6/11 notte | 24/7 Wall St (12:45): chip asiatici giu', SK Hynix -11,7%, Taiwan oltre -8% | ora 00 min 26.366,5; ore 01-03 +476 |
| 6/11 10:49 | Reuters: future Nasdaq su, i chip si stabilizzano | ora 10: +102, massimo 27.251,5 |
| 6/11 13:30 | CNBC: la vittoria democratica fa ripensare il percorso dei tassi | ora 13: -73 |
| 6/11 14:53-58 | CNBC: 165 mila posti; Yahoo: il rialzo di dicembre resta in gioco | ora 14: +56, volume 31.577, delta -1.065 |

## Cosa Dicono Per NQ

- **La vendita si e' allargata**: alle 16:16 di ieri RSP era -0,7%, in chiusura -2,9%; QQQ -6,5%, SMH -11%. Resta concentrata
  (3,6 punti fra QQQ e RSP), ma non e' piu' solo dei grandi. **Osservazione**.
- **Tassi e azioni**: ieri il 2 anni scende di 12 pb (corsa ai titoli di Stato) mentre il future da' piu' peso al rialzo; oggi,
  dopo il lavoro, il 10 anni risale e il rialzo sale all'89,6%. Il canale "Fed piu' dura, tech giu'" e' un modello, non misurato su NQ.
- **La volatilita'**: al massimo dell'anno e invertita alla chiusura di ieri; la realizzata a 24 ore (48,5%) supera la VXN
  (38,3), quella a 4 ore scende al 28,3%: la notte si e' calmata. Se la struttura resti invertita oggi non lo sappiamo.
- **Il lavoro non ha mosso NQ nell'ora**: escursione di 148 punti contro ore da 700 ieri; la barra oraria non isola le 14:30. **Osservazione**.
- **Credito e dollaro**: HYG al percentile 8, dollaro al 98, oro giu': contesto di prudenza; lo spread high yield del 5/11 manca.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.566,75 | chiusura di cassa del 4/11 (bridge) | misura quanto del crollo e' recuperato |
| 26.746,25 | chiusura di cassa del 5/11 (bridge) | lo scarto di oggi dalla cassa di ieri |
| 26.366,5 | minimo della notte, ora 00 del 6/11 (bridge) | il punto piu' basso dopo il crollo; posizionamento |
| 6/11 16:00; 17/11 sera; 9/12 | Michigan; conti NVDA; Fed (rialzo 89,6%) | i tre appuntamenti noti |

## Cosa Non Sappiamo

- **lacuna del collettore: salari orari di ottobre, occupati e disoccupazione in `esiti`** (presi dal titolo CNBC, non da BLS; una ricerca web senza risultato).
- **lacuna del collettore: VIX9D, VIX3M e VXN di oggi** (fermi al 5/11 22:15) e lo spread high yield del 5/11.
- **lacuna del collettore: le cause del crollo nei feed** (Qualcomm, H200 a Hong Kong, data center: solo dall'orchestratore),
  una notizia per l'ora 21 del 5/11, l'esito delle trimestrali uscite.
- **lacuna del collettore: la data vera dell'ampiezza** ("Nov 5" su dati di stamattina) e il calendario della settimana prossima (404).

## Per Il Quadro

- VXN al massimo dell'anno e struttura invertita: coerente con la gamma (fase 3)? In gamma negativa, in teoria, il movimento si allunga.
- NQ -12,7% in 20 sedute, RSP +2,4%: le posizioni (fase 2) restano lunghe sul Nasdaq?
- Ora 21 del 5/11 delta -4.856, ora 14 di oggi delta -1.065 con prezzo su: cosa dice l'order flow (fase 4) sul recupero da 26.366,5?
- Rialzo Fed all'89,6% e 10 anni +3,7 pb dopo il lavoro: il recupero della notte regge un tasso piu' alto?
