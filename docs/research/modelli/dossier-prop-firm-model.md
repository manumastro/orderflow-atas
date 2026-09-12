# The Prop Firm Model: Trascrizione Del Dossier

Trascrizione pagina per pagina di [`prop/prop_firm_model/`](../../../prop/prop_firm_model/), *The Prop Firm Model — a method by Fabio 'Fabervaale' Valentini*, sottotitolo *"A high winrate hyperscalping system for prop firm trading. Powered by auction market theory and real-time order flow."*

Il dossier e' numerato 09 pagine. La cartella ne contiene otto, dalla copertina alla 08. **La pagina 09 manca.**

Questa pagina riporta il dossier. Non lo interpreta e non lo corregge: dove la trascrizione e una qualunque altra pagina del repository divergono, vale il dossier. Le annotazioni di implementazione sono marcate e separate.

---

## Copertina — `1.webp`

Intestazione: `STRATEGY DOSSIER` · `ORDER FLOW · AMT`.

Tre parametri dichiarati in copertina:

| | |
|---|---|
| **1:1** | risk / reward |
| **40R** | candle frame |
| **High WR%** | win rate atteso |

Il win rate atteso non e' quantificato in nessuna pagina.

---

## 01 — Hyperscalping on 40R — `2.webp`

> *"Limit orders on value area edges. In and out fast."*

> *"This is a hyperscalping technique executed on 40-range candles. When favorable conditions align, a limit order is placed at the VAH of the just-closed candle. Stop at VAL. The 1:1 take profit is placed automatically via OCO. Fast entries, defined risk, high repetition."*

| | |
|---|---|
| **Entry** | Limit at VAH of the closed 40R candle |
| **Stop loss** | VAL of the same candle **or candle low** |
| **Take profit** | 1:1 placed automatically via OCO |

> *"Best in clearly trending markets or at key levels for mean reversion. If not filled on your order, do not chase. It is part of the game."*

**Nota di implementazione.** "When favorable conditions align" rimanda alla pagina 04. "Best in..." descrive due contesti, non una condizione di ingresso: il dossier non dice di astenersi fuori da essi.

---

## 02 — Chart setup — `3.webp`

> *"OrderTrack template. Value Area enabled."*

> *"Load the OrderTrack template in DeepCharts. Enable Value Area lines in the Order Flow Analyzer. Ensure Show Line is toggled on so VAH/VAL are visible on every 40R candle."*

| | |
|---|---|
| Template | OrderTrack |
| Candle range | **40 Range** · range bars, not time-based |
| Volume POC | Enabled |
| Delta POC | Enabled |
| Delta candles | Enabled |
| VA lines | Show Line ON · Highlight ON |

Nel pannello Order Flow Analyzer mostrato: `Chart area: Horizontal`, sezione `VALUE AREA` con `Enable`, `% Value Area` (valore non leggibile nell'immagine), `Highlight`, `Outside Color`, `Show Line`, `Line Color`.

**Nota di implementazione.** La percentuale di value area **non e' leggibile** nell'immagine e non e' dichiarata altrove. E' il parametro che determina l'ampiezza di VAH-VAL, cioe' la distanza di stop e target di ogni operazione. Nel replay si usa il valore di ATAS, non necessariamente lo stesso.

---

## 03 — OCO setup · DeepCharts — `4.webp`

> *"Define your risk. Let the software handle the rest."*

> *"The OCO strategy determines order quantity and TP location automatically once you set your SL and TP in dollar terms. Adjust to your own trading software accordingly."*

| | |
|---|---|
| OCO strategy | Toggle ON |
| Risk | OCO Qty panel · set your risk per trade |
| SL · money | Your risk per trade. Example: **$250** |
| TP · money | 1:1 mirror of SL. Example: **$250** |
| Quantity | Auto-calculated by software |
| Entry type | **LMT** at VAH (longs) · VAL (shorts) |

> **Prop firm sizing reference.** *"On a 50K prop account, $250 risk per trade x 6R daily cap = $1,500. After the 50% consistency rule, this is typically the highest allowed target win per day. Size accordingly."*

> **Mindset note.** *"If you do not get filled on your order, do not chase. It is part of the game."*

**Nota di implementazione.** Il rischio si fissa **in valuta** e la quantita' segue dallo stop; il cap giornaliero di 6R e' l'unico limite di frequenza dichiarato in tutto il dossier.

---

## 04 — Everything must align — `5.webp`

> *"Three conditions. All three. Every time."*

> *"Do not place the limit order unless all three conditions are confirmed on the closing 40R candle."*

### Condition 01 · Auction flip

> *"Watch for the flip of the auction. Delta flips: buyers take control from sellers (longs) or sellers from buyers (shorts). The auction direction changes. This is your first tell."*

### Condition 02 · Value area shift

> *"VA of new candle must be higher than the previous. For longs: the value area of the next 40R candle is positioned higher than the preceding candle. The auction is shifting. Mirror for shorts."*

### Condition 03 · Side control · real time

> *"Monitor who is in control as the candle forms. Buyers must remain in control for longs, sellers for shorts. If control flips mid-candle, cancel the pending limit order immediately."*

### All aligned

> *"Limit at VAH · SL at VAL · TP auto via OCO. Monitor side control on the active position."*

### Market context

| Trending markets | Key level mean reversion |
|---|---|
| *"Stacked VA shifts in one direction. Auction firmly controlled."* | *"Delta flips auction and VA shifts direction against prior trend."* |

**Note di implementazione.** Tre punti restano indeterminati:

- **Quanto grande** debba essere il flip del delta non e' detto. Nel replay e' una soglia dichiarata.
- **"Positioned higher"** ammette piu' letture: entrambi i bordi piu' alti, il solo VAL piu' alto, il punto medio piu' alto. Nel replay si usa "entrambi i bordi".
- **"Who is in control"** non e' definito operativamente. Nel replay e' il delta cumulato della candela in formazione.

L'ultima riga di "All aligned" aggiunge una quarta istruzione: *monitorare il controllo del lato anche sulla posizione aperta*, cioe' una possibile uscita discrezionale prima di stop o target. **Nel replay non e' implementata**: ogni operazione va a stop o a target.

---

## 05 — Live examples — `6.webp`

> *"Know the setup before it prints. Two live executions showing the pattern in action."*

**Example 01 · continuation after auction flip.** Posizione corta da 8 contratti, mostrata a `-8 QTY | -49,50 $`. Ordini: `+8 STP | 920,00 $` a 25.221,75 sopra, `+8 LMT | 1.040,00 $` a 25.209,50 sotto. Prezzo di carico attorno a 25.215,50: circa **6,25 punti** di stop e **6,00** di target.

**Example 02 · delta flip after pullback.** Posizione lunga da 5 contratti a `+5 QTY | 200,00 $` con ingresso a 25.720,25 e `-5 LMT | 1.000,00 $` a 25.730,25: **10 punti** di target. Sul grafico sono marcate due aree rettangolari e una linea tratteggiata a 25.722,75.

**Note di implementazione.**

- Gli importi in dollari implicano contratti **NQ full**, non MNQ: 8 x 20 x 6,25 = 1.000, coerente con i 920/1.040 mostrati.
- La distanza di rischio non e' la stessa nei due esempi: 6,25 punti contro 10. Con stop al VAL della candela chiusa la distanza e' determinata dalla candela, quindi la differenza e' compatibile, ma nell'esempio 02 le aree marcate a mano suggeriscono che il livello sia stato scelto e non letto da una singola candela.
- **Le aree rettangolari non sono descritte in nessuna pagina.** Sono l'unico elemento grafico discrezionale visibile e non hanno regola associata.

---

## 06 — Neural pattern library — `7.webp`

Sei configurazioni, presentate come immagini senza testo descrittivo:

| | Nome |
|---|---|
| 01 | Auction flip |
| 02 | Previous high breakout |
| 03 | Sellers protecting, buyers reload |
| 04 | Sellers absorbed at candle bottom |
| 05 | Buyers protecting · next up candle |
| 06 | Auction flip short · break + retest |

**Nota di implementazione.** La pagina non contiene regole: sono esempi da riconoscere a vista. Quattro dei sei nomi (02, 03, 04, 05) descrivono comportamenti che le tre condizioni della pagina 04 **non catturano**: rottura di un massimo precedente, difesa di un lato con ricarico dell'altro, assorbimento al minimo di candela, protezione dei compratori. Se il riconoscimento di queste configurazioni fa parte del filtro d'ingresso, la pagina 04 non e' la specifica completa del modello.

---

## 07 — Session timing — `8.webp`

> *"Trade in the right window. Protect your edge."*

> *"Chose to trade only when the conditions are aligning, when the market is moving just right, not too fast, not too slow."*

**ON — Pre-market and first two hours of RTH NY Session**
> *"Market is liquid. Auctions are decisive. Institutions are active. Conditions are favorable for clean setups."*

**OFF — Lunch hours and evening sessions**
> *"Thin liquidity. Choppy price action. Conditions are not ideal for this model. Avoid."*

> **Mindset note.** *"If you do not get filled on your order, do not chase. It is part of the game."*

**Nota di implementazione.** *"Not too fast, not too slow"* e' un filtro sul ritmo del mercato, dichiarato ma non quantificato. E' l'unico criterio di selezione oltre alle tre condizioni, e nel replay **non e' implementato**.

---

## 08 — manca

La cartella non contiene la pagina 09 di 09.

---

## Cosa Il Dossier Non Determina

Riepilogo di quanto sopra, perche' e' la lista che conta per chiunque provi a misurare il modello.

| # | Lasciato aperto | Dove |
|---|---|---|
| 1 | percentuale della value area | 02, non leggibile |
| 2 | grandezza minima del delta flip | 04, condizione 01 |
| 3 | cosa significa "VA positioned higher" | 04, condizione 02 |
| 4 | come si misura "who is in control" | 04, condizione 03 |
| 5 | uscita discrezionale sulla posizione aperta | 04, "All aligned" |
| 6 | ruolo delle sei configurazioni della library | 06 |
| 7 | le aree marcate a mano negli esempi | 05 |
| 8 | *"not too fast, not too slow"* | 07 |
| 9 | quanto dura la validita' di un ordine pendente | mai detto |
| 10 | contenuto della pagina 09 | assente |

Le prime quattro sono parametri: si dichiarano e si misura la sensibilita'. Le voci 5, 6, 7 e 8 sono **discrezione**, e nessuna quantita' le sostituisce. Un replay che le ignora non misura il modello del dossier ma un suo sottoinsieme meccanico, e va letto come tale.

## Rapporto Con Le Altre Pagine

- [Modello 40R di riferimento](modello-40r-riferimento.md): le convenzioni scelte per rendere eseguibile il modello.
- [Replay sulla settimana 2026-09-04 / 09-11](../sessioni/replay-modello-settimana-2026-09-11.md): cosa misura il sottoinsieme meccanico.

## Confronto Con Il Primo Transcript Del Corso

Il dossier e la prima live del corso (`fabio_course/fabio_q1/`) descrivono due cose diverse, e la
differenza spiega perche' il replay produce molte piu' operazioni di quante Fabio ne prenda.

| | dossier | primo transcript |
|---|---|---|
| innesco | ogni candela 40R che soddisfa le tre condizioni | un livello marcato **prima** della sessione, poi order flow come conferma |
| frequenza | non dichiarata | "one or two opportunities per day" con il session profile, "5, 10" con i fixed profile sugli swing point |
| obiettivo | 1:1 con OCO | 1:2, 1:2.5, fino a 1:12 e 1:20; "take out one, two, three, it's enough, and reload the risk" |
| gestione | non descritta | stop a pareggio appena possibile: "why leaving floating profit on the market?" |
| chiusura giornata | cap in R | "if we lose this trade, we stop for the day"; la live chiude a 4,5R con zero stop loss, solo pareggi e target |
| filtro di rumore | assente | big trades e speed of tape: "it's keeping you out of useless moment in the market" |

La sequenza del transcript e' esplicita: **"the first step is profile framing"**. La location viene
prima del segnale, non dopo. Il dossier la da' per scontata perche' descrive solo il momento
dell'esecuzione.

### Cosa Ha Prodotto La Misura

Mese di 19 sessioni, finestra 13:30-15:30 UTC, entrata limite vera:

| | op/sessione | WR | R |
|---|---|---|---|
| modello senza gate di location | 21,8 | 49% | -6,61 |
| modello nullo (nessuna condizione) | 221 | 47% | -273 |
| gate sul profile framing del giorno prima, ±10 punti | 4,6 | 48% | -4,00 |
| stesso gate, ±0 punti | 1,4 | 42% | -4,00 |
| stesso gate, ±25 punti | 8,1 | 46% | -13,00 |

Il gate di location porta la frequenza esattamente nell'intervallo che Fabio dichiara, ma **non
sposta la percentuale di vittorie**. Il disallineamento di frequenza era reale; l'edge non sta li'.

Allargare l'obiettivo peggiora in modo monotono (1:2 -> 32%, 1:3 -> 24%, sotto il breakeven di
entrambi), e lo stop a pareggio a 0,5R peggiora il totale invece di migliorarlo: troppe posizioni
che avrebbero raggiunto il target vengono azzerate prima. Sono i numeri di una serie senza
direzione. Cio' che il transcript aggiunge e che il replay non misura e' il filtro big trades piu'
speed of tape, cioe' esattamente la parte che Fabio descrive come "informational edge".
