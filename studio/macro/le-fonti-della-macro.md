# Le Fonti Della Macro

**15 ottobre 2026.** Le fonti di `macro.py`, la fase 1 dello studio globale. Vengono dal catalogo di
**World Monitor** ([github.com/koala73/worldmonitor](https://github.com/koala73/worldmonitor),
AGPL-3.0), una dashboard di intelligence globale che ha anche un pannello per NQ ("NQ Pulse",
`src/config/nq-context.ts`). Una copia di sola consultazione sta fuori dal repository, in
`C:\Users\emanu\worldmonitor` (clonata il 15 ottobre 2026, commit `ffa3162`).

## Perche' Le Fonti E Non Il Codice

- **L'API di World Monitor e' a pagamento**: `worldmonitor.app/mcp` e le rotte REST chiedono una
  chiave `X-WorldMonitor-Key` (piano Pro).
- **Il codice e' AGPL, e questo repository e' pubblico** e non e' AGPL: copiarne il codice ci
  porterebbe la licenza. Gli indirizzi pubblici delle fonti sono fatti, non codice.
- Quindi `macro.py` e' **nostro**, in Python con la sola libreria standard, e interroga direttamente
  le stesse fonti pubbliche. Dove ne aggiunge una che World Monitor non ha, lo dice.

Se un giorno servisse tutta la dashboard, si puo' far girare World Monitor in locale dalla copia
(`npm run dev:finance`); molte sue fonti chiedono chiavi proprie (FRED, Finnhub, EIA, ACLED).

## Le Fonti

| blocco | fonte | chiave | dove sta in World Monitor |
|---|---|---|---|
| quotazioni: NQ=F, ES=F, QQQ, RSP, ^VXN, ^VIX, ^VIX9D, ^VIX3M, ^TNX, DXY, WTI, oro, TLT, HYG, SMH, XLK, le otto grandi | Yahoo Finance chart v8, un anno giornaliero | no (user agent di Chrome) | `scripts/seed-market-quotes.mjs`, `src/config/nq-context.ts` |
| curva dei Treasury | home.treasury.gov, curva par giornaliera (CSV) | no | `scripts/seed-us-treasury-par-yield.mjs` (XML) |
| fed funds, 2 e 10 anni, inflazione attesa, spread high yield, stress finanziario, bilancio Fed; PPI, vendite al dettaglio, CPI, sussidi, Philly Fed | FRED, CSV pubblico dei grafici (`fredgraph.csv?id=<serie>&cosd=<data>`) | **no** | `scripts/seed-us-interest-rates.mjs`, `scripts/_fred-seeder.mjs` (con la chiave); il CSV senza chiave e i dati del calendario sono **aggiunte nostre** |
| il dato uscito degli eventi della settimana (`esiti`) | FRED, la stessa raccolta, agganciata al titolo di Forex Factory | no | **aggiunta nostra** |
| VIX, VIX9D, VIX3M, VXN, 10 anni e 2 anni di adesso (`tempo_reale`) | Yahoo chart v8 a 5 minuti, con l'ora del dato | no | **aggiunta nostra** |
| probabilita' FOMC dai future sui fed funds (`fomc_future`) | Yahoo, future ZQ del CBOT (`ZQ<mese><aa>.CBT`), metodo CME FedWatch | no | **aggiunta nostra**: World Monitor usa solo Polymarket |
| ampiezza del Nasdaq-100 (`ampiezza`) | api.nasdaq.com, elenco del Nasdaq-100 con la variazione del giorno | no | **aggiunta nostra** |
| calendario USD con attesa e precedente | Forex Factory (`nfs.faireconomy.media`) | no | **aggiunta nostra**: World Monitor non ha le attese |
| date FOMC 2026 | federalreserve.gov | no | `scripts/seed-economic-calendar.mjs` |
| trimestrali delle grandi, 14 giorni | api.nasdaq.com | no | **aggiunta nostra**: World Monitor usa Finnhub, a chiave |
| mercati di previsione (fed, inflazione, recessione, dazi) | Polymarket gamma API | no | `scripts/_prediction-upstream.mjs` |
| sentiment | CNN Fear & Greed | no (user agent Mac e Referer, se no 418) | `scripts/seed-fear-greed.mjs` |
| notizie delle ultime 30 ore, titolo e testo (la descrizione del feed, al massimo 300 caratteri) | RSS: CNBC, Federal Reserve, Google News (Nasdaq futures, Treasury, Fed) | no | `src/config/variants/finance.ts` |

**Il COT** sta in un collettore a parte, `studio/cot/cot.py`, dall'API pubblica della CFTC (la stessa
che usa World Monitor in `scripts/seed-cot.mjs`).

## I Limiti

- **Contesto, non backtest.** La decisione 2 di CLAUDE.md (i dati solo da ATAS) vale per il metodo
  quantitativo. Queste fonti, su richiesta dell'utente del 15 ottobre 2026, servono a descrivere.
- **Yahoo** puo' rispondere 429 da reti di datacenter; da casa funziona. Le quotazioni sono in
  ritardo di qualche minuto e non sono di qualita' da esecuzione.
- **Forex Factory** pubblica la settimana prossima solo verso la fine della settimana, e non da' il
  dato uscito. Il dato uscito lo prende `macro.py` da FRED (`esiti`), quando FRED ha gia'
  l'osservazione del periodo giusto; altrimenti `esiti` lo dice, e l'agente lo cerca alla fonte
  primaria.

### FRED Senza Chiave

- **Il CSV dei grafici non chiede la chiave**: `https://fred.stlouisfed.org/graph/fredgraph.csv?id=<serie>&cosd=<AAAA-MM-GG>`.
- **Con lo user agent di Chrome FRED non risponde**: tiene la connessione e la chiude dopo circa 19
  secondi. Con uno user agent che dice chi siamo (`orderflow-atas-studio/1.0`) risponde in un decimo di
  secondo. Lo user agent e' onesto: non e' un modo di aggirare un blocco.
- **Le richieste vanno una alla volta.** Il 15 ottobre 2026 otto richieste in parallelo hanno fatto
  scadere per qualche minuto tutte le richieste da Python, anche quelle in serie. Per questo `macro.py`
  le fa in serie, con 0,2 secondi fra l'una e l'altra; dopo due errori di fila salta le serie rimaste
  e lo scrive in `fred.errori`.
- **I giornalieri** (DFF, DGS2, DGS10, T10YIE, spread HY) arrivano con un giorno di ritardo. Il 10 anni
  di oggi sta in `tempo_reale` (^TNX).
- **Le variazioni** mensili e annue di PPI, CPI e vendite al dettaglio sono calcolate dall'indice
  destagionalizzato. Possono differire di 0,1 dal comunicato per gli arrotondamenti.
- **Le serie "core" sono corrispondenze approssimate**: Forex Factory "Core PPI m/m" -> PPICOR (domanda
  finale esclusi alimentari, energia e commercio); "Core Retail Sales" -> RSFSXMV (esclusi auto e
  ricambi); "Core CPI" -> CPILFESL. I titoli delle serie vanno verificati sulla pagina di FRED
  (`fred.stlouisfed.org/series/<serie>`), che il 15 ottobre 2026 non si leggeva da script.
- **Il periodo atteso**: per un mensile, il mese prima del giorno dell'uscita (il Philly Fed e' dello
  stesso mese); per i sussidi, la settimana che finisce il sabato prima. Se FRED non ha ancora quel
  periodo, `esiti` scrive `uscito: null` e il perche'.

### Il Resto Delle Aggiunte

- **`tempo_reale`** dice per ogni simbolo se l'ultimo dato e' **di oggi** (ora di New York) o no. Il
  15 ottobre 2026 Yahoo dava il **VXN solo alla chiusura di ieri**, e `2YY=F` (il future Cboe sul
  rendimento a 2 anni) fermo al 13/10. Per il 2 anni di oggi resta **ZT=F**, il future sul Treasury
  a 2 anni: e' un **prezzo**, che sale quando il rendimento scende.
- **`fomc_future`** usa il metodo di CME FedWatch per un passo da 25 pb:
  - il tasso implicito di un mese e' 100 meno il prezzo del future, ed e' la media dei tassi di ogni
    giorno del mese;
  - la decisione vale dal giorno dopo la riunione;
  - il mese dopo la riunione non ne ha un'altra (vero per tutte le date del 2026), quindi il suo
    implicito e' il tasso dopo la decisione;
  - il tasso prima si ricava dal mese della riunione: (implicito x giorni del mese - dopo x giorni dopo
    la riunione) / giorni prima;
  - la probabilita' e' |dopo - prima| / 0,25.

  Il fed funds effettivo di FRED (DFF) sta accanto, come controllo del tasso prima. Il 15 ottobre 2026
  il metodo dava il 27,9% a un rialzo il 28/10, contro il 27,5% di Polymarket.
- **`ampiezza`** conta quanti dei titoli del Nasdaq-100 salgono e quanti scendono. La variazione
  "pesata" usa la capitalizzazione, non i pesi modificati dell'indice: e' un'approssimazione. GOOG e
  GOOGL sono due righe.
- **Il testo delle notizie** e' la descrizione del feed. CNBC vi mette il sommario dell'articolo;
  Google News solo il titolo con la testata, e allora il testo resta vuoto.
- **Bloccate e lasciate**: l'API delle quotazioni di CNBC (`quote.cnbc.com`) risponde "Access Denied".
  Non si aggira.
