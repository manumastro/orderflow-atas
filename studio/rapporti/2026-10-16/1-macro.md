# Fase 1 · La Macro · 2026-10-16 (aggiornamento del pomeriggio)

**Stato**: parziale (VIX9D, VIX3M, VXN e 2YY=F non di oggi; `raccolta.json` ancora quello delle 09:36)
**Dati**: `macro.json` (raccolto 15:29 italiane, nessun errore), `ore-NQZ6.json` (bridge, fino alle
15:30; l'ora 15 ha 30 minuti). Di oggi: NQ, ES, DXY, WTI, oro, VIX (15:14), ^TNX (15:14), ZT=F,
future fed funds (14:41-15:17). **Alla chiusura del 15/10**: VIX9D, VIX3M, VXN, ETF, azioni, curva
Treasury. FRED ora completo (DFF e STLFSI4 tornati). Nessuna ricerca web: tutti gli esiti presenti.
Il rapporto delle 09:35 e' in `precedenti/0936/1-macro.md`: qui si dice cosa e' cambiato.

**In una riga**: mattina senza dati e senza strappi; i tassi risalgono un poco (10A +3 pb) e il
mercato rimette un filo di rialzo a ottobre (ZQ 12,2% -> 15,5%), il VIX scende ancora in un contango
calmo, dollaro forte e fermo, credito al minimo dell'anno; NQ scivola piano (-117 punti dalle 10)
mentre l'ampiezza del Nasdaq-100 si restringe: contesto di rischio concentrato, invariato nel segno.

## I Fatti

**Cosa e' cambiato dalle 09:35** (fonte e ora nella colonna; pb = punto base, 0,01%):

| misura | 09:35 | 15:29 | fonte, ora del dato |
|---|---|---|---|
| 10 anni (^TNX) | 5,089% | **5,121%** (+3,2 pb, pct 1 anno 94,0) | Yahoo, 15:14 |
| ZT=F (prezzo del future sul 2 anni) | fermo | 101,914 contro 101,941: prezzo giu', rendimento su di poco | Yahoo, 15:19 |
| rialzo 25 pb il 28/10, future ZQ | 12,2% | **15,5%** (+3,9 pb attesi) | Yahoo/CBOT, metodo FedWatch |
| rialzo 25 pb il 28/10, Polymarket | 12,5% | **13,5%**; fermo 86,5% invariato | gamma-api.polymarket.com |
| dicembre implicito (ZQZ6) | 4,065% | **4,085%** (ottobre 3,885%: circa +20 pb) | Yahoo/CBOT, 14:55 |
| fine 2026 a 4,25% · >= 4,5% | 63,3% · 20,3% | **66,3% · 13,4%** (4,0%: 14,5%) | Polymarket |
| VIX | 16,02 | **15,81** (-3,95% sul giorno, pct 25,2) | Yahoo, 15:14 |
| DXY · WTI · oro | 101,806 · 95,71 · 4.208,6 | **101,788 · 95,90 · 4.179,9** | Yahoo, 15:19 |
| NQ=F (Yahoo) | +0,15% | **30.051, -0,31%** | Yahoo, 15:19 |
| Fear & Greed (CNN, 0-100) | 32,1 "fear" | 32,1 "fear" (una settimana fa 46,5) | CNN |

**FRED, tornati** (giornalieri con un giorno di ritardo): fed funds effettivo **3,88%** (14/10, fermo);
indice di stress St. Louis **-0,72** al 09/10 (prima -0,47; sotto zero = stress sotto la media).
Invariati: breakeven 10 anni (inflazione attesa dal mercato) 2,32%; spread high yield 3,25 (+9 pb).

**I tassi** (home.treasury.gov, curva par del 15/10, %; nessuna curva nuova da stamattina):
3M 4,28 · 2A 4,77 · 10A 5,10 · 30A 5,43; pendenze 10A-2A +0,33, 10A-3M +0,82 (14/10: +0,37 e +0,91;
16/09, 20 sedute fa: +0,27 e +0,87). Il +3 pb di ^TNX di oggi annulla un terzo del -9 di ieri.

**La volatilita'**. Il VIX e' il prezzo delle opzioni sull'S&P 500 tradotto in "quanto il mercato
si aspetta che l'indice si muova nei prossimi 30 giorni", in percentuale annua; diviso per 16 (la
radice dei 252 giorni di borsa, circa) da' il movimento giornaliero atteso: 15,81 vuol dire circa
**1% al giorno**. Il VXN e' lo stesso sul Nasdaq-100: 22,89 (15/10) vuol dire circa 1,4% al giorno.

| indice | valore | ora | pct 1 anno |
|---|---|---|---|
| VIX (30 giorni) | 15,81 | 16/10 15:14 | 25,2 |
| VXN (Nasdaq-100, 30 giorni) | 22,89 | 15/10 22:00 | 45,4 |
| VIX9D (9 giorni) · VIX3M (3 mesi) | 13,97 · 18,49 | 15/10 22:15 | 31,5 · 17,1 |

- Struttura (ieri sera): **contango**, cioe' la scadenza vicina costa meno della lontana (13,97 <
  16,46 < 18,49). Oggi VIX9D e VIX3M non sono aggiornati: la struttura di oggi non e' misurata.
- NQ=F volatilita' realizzata a 20 giorni 18,4% annua (Yahoo, chiusure): sotto il VXN 22,89, cioe'
  le opzioni prezzano piu' movimento di quello avvenuto nell'ultimo mese.

**Chi guida**: ETF e azioni sono ancora alla chiusura del 15/10 (QQQ -0,13%, RSP +1,37%, SMH -1,04%
in 1 g, -7,98% in 5 g). **Ampiezza Nasdaq-100** (api.nasdaq.com, etichetta "Oct 15"): ora **61 su,
39 giu'**, mediana **+0,34%** (stamattina +1,19%), media -0,18%, pesata per capitalizzazione -0,06%;
TSLA +2,29%, AAPL +1,35%, NVDA -0,26%, AVGO -1,32%. Numeri diversi da stamattina con la stessa data:
vedi Cosa Non Sappiamo.

**NQZ6 oggi** (bridge, UTC convertito): massimo notte 30.225 (ora 07), minimo 29.959,25 (ora 02).
Dalle 10:00 (30.157,5) alle 15:30 (30.040,5): **-117**, ore 11-15 con delta (acquisti meno vendite
aggressive) fra -223 e +239, nessuna ora sopra 50 punti di corpo. Chiusura cassa 15/10: 30.140,25.

**Le notizie della giornata e le ore** (RSS CNBC e Google News; abbinamento per ora, non per causa):

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 11:46-11:49 | Trump: niente nucleare ne' truppe di terra in Iran (CNBC); "yields edge higher as stocks recover" (WSJ) | ora 11: **-49**, delta -223, volume 6.399 |
| 13:57-14:03 | Tesla presenta la Roadster (CNBC); "market likes what Warsh didn't say" (Barron's) | ora 13: +4,5, volume 10.008 |
| 14:30-14:38 | "Futures mixed; Tesla flies as Anthropic value questioned" (IBD); "Warsh touting AI" (Boston Herald) | ora 14: -26,75, volume 15.693, delta +239 |
| 15:19 | "mixed open, as CoreWeave shares tumble again" (CNBC) | ora 15 (30 min): -24,5, volume 14.911 |

**Rumore**: Roadster, KFC, Eutelsat, fondo norvegese, Turchia, super PAC: nessun movimento di NQ
riconoscibile. Il titolo Trump-Iran coincide con l'ora piu' debole, ma -49 punti sono meno di 1/4
del movimento giornaliero atteso dal VXN: non e' un movimento da notizia.

## Cosa Dicono Per NQ

Tutte **osservazioni**: nessun documento del metodo misura la reazione alle notizie o alla rotazione.

1. **Il sollievo di Warsh si e' ridotto, non annullato**: rialzo il 28/10 da 12% a 15,5% (ZQ) e 10A
   +3 pb dopo -9 ieri; la scommessa resta "fermi a ottobre, rialzo a dicembre" (+20 pb impliciti).
2. **La giornata europea di NQ e' una deriva lenta** (-117 in cinque ore, volumi bassi fino alle 13,
   poi in aumento verso l'apertura): il movimento delle 05 (+105, Warsh) e' stato restituito.
3. **La rotazione si attenua nei dati di ampiezza** (mediana da +1,19 a +0,34), se i numeri sono di
   oggi; TSLA e AAPL su, AVGO giu': i semiconduttori restano il lato debole.
4. **Volatilita' calma, credito no**: VIX pct 25 e in calo, HYG al pct 2,4, CoreWeave "tumble again":
   lo stress resta nel debito dell'AI, non nelle opzioni sull'indice.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 10A 5,311% · DXY 102,58 | Yahoo, massimi a un anno | posizionamento di tassi e dollaro (ora 5,121% e 101,788) |
| HYG 76,81 | Yahoo, minimo a un anno | posizionamento del credito: sotto, nuovo minimo (ultimo 77,13) |
| VIX 13,47 | Yahoo, minimo a un anno | posizionamento della volatilita' (ora 15,81) |
| NQZ6 30.140,25 | chiusura cassa 15/10, bridge | riferimento del gap d'apertura: per la fase 4 |
| NQZ6 29.959,25 / 30.225 | estremi della notte, bridge | escursione della notte di Warsh: per la fase 4 |
| 16/10 21:30 · 20/10 · 21/10 · 28/10 19:00 | COT; NFLX; META e TSLA; FOMC, poi GOOGL e MSFT | contesto fase 2; trimestrali dopo la chiusura; evento alto |

## Cosa Non Sappiamo

- **L'ampiezza**: l'API dice "Oct 15", ma i numeri cambiano da stamattina e TSLA +2,29% coincide con
  "Tesla flies" di oggi. Forse sono le variazioni del pre-mercato di oggi con l'etichetta di ieri.
- ZQ e Polymarket divergono di 2 punti (15,5% contro 13,5%): il metodo ZQ vale per un passo da 25 pb.
- Il calendario della settimana prossima non c'e' (Forex Factory 404 stamattina, nessuna nota nuova).
- Google News da' solo il titolo: il testo non c'e'.
- lacuna del collettore: VIX9D, VIX3M e VXN di oggi (ultimo dato 15/10); 2YY=F fermo al 13/10;
  curva Treasury di oggi (esce la sera); `raccolta.json` del pomeriggio non ancora scritto.

## Per Il Quadro

1. **Rialzo il 28/10 al 15,5% (ZQ), 10A 5,121%**: il rischio di tasso torna un poco; coerente con il
   COT (fase 2) e con il tono di NQ in apertura?
2. **VIX 15,81 in calo, contango ieri sera, VXN 22,89 sopra la realizzata 18,4%**: le opzioni pagano
   movimento; coerente con il segno della gamma dei dealer (fase 3)?
3. **NQZ6 -117 dalle 10, sotto la chiusura 30.140,25**: la deriva europea ha chi la spinge? Fase 4.
4. **Mediana NDX +0,34%, pesata -0,06%, HYG al minimo dell'anno**: la rotazione dall'AI e' ancora il
   tema, o si spegne?
