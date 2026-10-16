# Fase 1 · La Macro · 2026-10-16

**Stato**: parziale (manca `raccolta.json`; mancano fed funds effettivo e indice di stress da FRED)
**Dati**: `macro.json` (09:34 italiane), `ore-NQZ6.json` (bridge, fino alle 09:35); `raccolta.json`
assente (GEX in corso). Di stamattina: NQ, ES, DXY, WTI, oro, VIX, ZT=F, future fed funds; **alla
chiusura del 15/10**: VIX9D, VIX3M, VXN, ^TNX, ETF, azioni, curva Treasury. Errori: FRED `DFF` e
`STLFSI4` in timeout, `2YY=F` fermo al 13/10, Forex Factory settimana prossima 404. Nessuna ricerca
web: l'esito di Warsh e' nelle notizie.

**In una riga**: inflazione di settembre sopra l'attesa su tutta la linea, ma Warsh non apre a un
rialzo il 28/10 e il mercato lo toglie (27,5% -> 12,5%); tassi in discesa da due giorni, VIX basso in
contango, dollaro forte, credito al minimo dell'anno; la vendita resta sui grandi della tecnologia
mentre il Nasdaq-100 per mediana sale: un contesto di rischio concentrato, non generale.

## I Fatti

**Gli esiti** (`esiti`: FRED, variazione calcolata dall'indice; attesa da Forex Factory; NQZ6 dal bridge):

| quando | dato | attesa | uscito | NQZ6 nell'ora |
|---|---|---|---|---|
| 14/10 14:30 | CPI m/m · a/a; core m/m · a/a | 0,6 · 3,6; 0,2 · 2,5 | **0,65 · 3,72; 0,33 · 2,57** | ora 14: -205,75 |
| 15/10 14:30 | PPI m/m (a/a); core m/m | 0,6; 0,3 | **1,34 (6,35); 0,58** | ora 14: -26,5, escursione 165,75 |
| 15/10 14:30 | vendite al dettaglio m/m · core | 0,3 · 0,6 | **0,77 · 0,90** | come sopra |
| 15/10 14:30 | Philly Fed · sussidi | 28,2 · 199 mila | **20,9** · 199 mila | come sopra |
| 16/10 05:30 | discorso di Warsh | | nessun segnale su ottobre (CNBC 09:11) | ora 05: **+105**, delta +400 |

Da ieri 18:43: esiti ora presenti; rialzo 28/10 (Polymarket) 27,5% -> **12,5%**; Fear & Greed 29,3 -> 32,1.

**I tassi** (home.treasury.gov, curva par, %; pb = punto base, 0,01%; pendenza = differenza):

| | 3M | 2A | 10A | 30A | 10A-2A | 10A-3M |
|---|---|---|---|---|---|---|
| 15/10 | 4,28 | 4,77 | 5,10 | 5,43 | +0,33 | +0,82 |
| 14/10 | 4,28 | 4,82 | 5,19 | 5,52 | +0,37 | +0,91 |
| 16/09 (20 sedute) | 4,14 | 4,74 | 5,01 | 5,35 | +0,27 | +0,87 |

- Sul giorno 2A -5 pb, 10A e 30A -9: appiattimento in discesa. ^TNX 5,089% (pct 1 anno 93,3); ZT=F
  fermo stamattina. FRED: breakeven 10 anni (inflazione attesa) **2,32%**, -3 pb; spread HY **3,25**, +9 pb.
- **FOMC 27-28/10**, decisione il 28 alle 19:00. Future fed funds (ZQ, metodo FedWatch, 09:23): +3 pb,
  **rialzo di 25 pb al 12,2%**; dicembre implicito 4,065% contro 3,88% di ottobre (circa +18 pb).
  Polymarket (42,7 mln $): fermo 86,5%, +25 pb 12,5%; fine 2026: 4,25% 63,3%, >= 4,5% 20,3%.

**La volatilita'** (Yahoo; percentile = posizione nell'anno):

| indice | valore | ora | 1 g | 5 g | pct 1 anno |
|---|---|---|---|---|---|
| VIX (implicita S&P 500 a 30 giorni) | 16,02 | 16/10 09:19 | -2,67% | +7,95% | 29,5 |
| VXN (la stessa sul Nasdaq-100) | 22,89 | 15/10 22:00 | +0,84% | +4,14% | 45,4 |
| VIX9D (9 giorni) · VIX3M (3 mesi) | 13,97 · 18,49 | 15/10 22:15 | -4,25% · -1,60% | | 31,5 · 17,1 |

- **Contango** (VIX9D 13,97 < VIX 16,46 in chiusura < VIX3M 18,49), VIX9D/VIX3M 0,76 (ieri sera 0,79):
  la soglia d'inversione (VIX9D sopra VIX3M) e' lontana. Realizzata di NQZ6 (log a 5 minuti,
  annualizzata): 4 ore 14,0%, 24 ore 19,9%, 48 ore 21,3%.

**Dollaro, petrolio, oro, credito** (Yahoo): DXY 101,806 (-0,19%, pct 95,6); WTI 95,71 (-2,31%); oro
4.208,6 (+0,92%); TLT 79,97 (+1,37%, pct 6,0); HYG 77,13 (+0,42%, **pct 2,4**, min 76,81).

**Chi guida** (Yahoo, ETF alla chiusura del 15/10; RSP = S&P 500 equipesato, la partecipazione):

| | NQ=F | ES=F | QQQ | RSP | SMH | XLK |
|---|---|---|---|---|---|---|
| 1 g | +0,15% (09:24) | +0,13% | -0,13% | **+1,37%** | -1,04% | -0,65% |
| 5 g | -2,96% | -0,74% | -2,62% | +1,48% | **-7,98%** | -4,34% |

- Nasdaq-100 il 15/10 (api.nasdaq.com): **63 salgono, 38 scendono**, mediana +1,19%, **pesata per
  capitalizzazione -0,09%**. Giu' NVDA (11,2% del peso) -1,84%, SPCX -2,33%, TSLA -1,29%, AVGO, AAPL,
  META; su MSFT +1,08%, GOOGL +0,45%, AMZN +0,27%. NQ=F 4,1% sotto il massimo a un anno (31.483,25).
- Trimestrali: NFLX 20/10; META, TSLA 21/10; GOOGL, MSFT 28/10; AMZN, INTC 29/10. **Sentiment**:
  CNN Fear & Greed **32,1 "fear"**; una settimana fa 46,5, un mese fa 27,3.
- **NQZ6**: cassa 15/10 chiusa a 30.140,25 (-44,75), minimo 29.622,5; notte 29.959,25-30.225.

**Le notizie e le ore** (RSS CNBC e Google News; abbinamento per ora, non per causa):

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 15/10 20:17-16/10 01:24 | "rotate out of tech" (WSJ); "shift from AI to healthcare" (Reuters) | coerente con l'ampiezza sopra |
| 15/10 21:59 | Warsh "focused on taming rising prices", nessun segnale (NBC) | ora 21: +104,75, **delta -2.067** |
| 15/10 23:33-00:32 | CDS di CoreWeave in dissesto (CNBC); Hammack sulle aspettative (Reuters) | ora 01: **-81,75**, delta -471 |
| 16/10 05:30-07:25 | Warsh: niente su ottobre, "stretta sincrona" globale; "bets on hold strengthen" (FT) | ora 05: **+105**, volume 9.520 (6.340 prima); ore 06-08 piatte |

**Rumore**: Hormuz (07:56; NQ ora 07 -10); StoneX 04:38 "oil jumps 5%" (smentito: WTI -2,31%); altro.

## Cosa Dicono Per NQ

Tutte **osservazioni**: nessun documento del metodo misura la reazione ai dati o la rotazione.

1. **Dati caldi, Fed meno aggressiva nel breve**: rialzo il 28/10 al 12% (ZQ e Polymarket), 10A -9 pb,
   breakeven in calo; il rialzo si sposta a dicembre (+18 pb impliciti).
2. **Warsh ha mosso NQ nell'ora del discorso** (+105, delta positivo, volume +50%), poi tre ore piatte.
3. **La rotazione e' il tema**: NQ e' tirato da NVDA e dai semiconduttori, non da un ribasso largo.
4. **Volatilita' calma, credito no**: VIX pct 29,5, HYG pct 2,4, CoreWeave in dissesto: lo stress sta
   nel debito dell'AI, non nelle opzioni sull'indice.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 10A 5,311% · DXY 102,58 | Yahoo, massimi a un anno | posizionamento di tassi e dollaro (ora 5,089% e 101,806) |
| HYG 76,81 | Yahoo, minimo a un anno | posizionamento del credito: sotto, nuovo minimo (ora 77,13) |
| VIX9D = VIX3M (18,49) | Yahoo, 15/10 | soglia della struttura invertita (ora 13,97) |
| NQZ6 30.140,25 | chiusura cassa 15/10, bridge | riferimento del gap d'apertura: per la fase 4 |
| NQZ6 29.959,25 / 30.225 | estremi della notte, bridge | la notte di Warsh: per la fase 4 |
| 16/10 21:30 · 20-21/10 · 28/10 19:00 | COT; NFLX, META, TSLA; FOMC e GOOGL, MSFT | contesto fase 2; trimestrali; evento alto |

## Cosa Non Sappiamo

- PPI "core" di Forex Factory e serie PPICOR (esclude anche il commercio) possono non coincidere; gli
  esiti sono dall'indice, non il numero arrotondato del comunicato. Oggi nessun altro evento USD.
- NBC riporta Warsh alle 21:59 del 15/10: forse il testo e' uscito prima delle 05:30.
- Le barre di ieri cambiano dal rapporto delle 18:43 (ora 14 del 15/10: volume 33.217 e delta +1.027
  oggi, 52.997 e +1.223 ieri). Polymarket "fine 2026": livello forse l'estremo alto della forbice.
- lacuna del collettore: `raccolta.json` assente; DFF e STLFSI4 (timeout FRED); VIX9D, VIX3M, VXN e
  ^TNX di stamattina; 2YY=F fermo al 13/10; calendario della settimana prossima; testo di Google News.

## Per Il Quadro

1. **Mediana NDX +1,19%, pesata -0,09%, SMH -7,98% in 5 g**: il COT (fase 2) va letto contro una rotazione.
2. **VIX 16,02 in contango, VIX9D/VIX3M 0,76**: coerente con il segno della gamma dei dealer (fase 3)?
3. **Warsh +105, cassa a 30.140,25**: chi ha comprato il recupero da 29.622,5 (ora 21 delta -2.067)? Fase 4.
4. **Rialzo il 28/10 al 12%, HYG al minimo dell'anno**: cala il rischio di tasso, non quello del credito AI.
