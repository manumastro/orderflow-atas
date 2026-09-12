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

## Big Trades E Speed Of Tape

Implementati come descritti nel primo transcript, in tre letture diverse, e misurati sullo stesso
mese di 19 sessioni. Le opzioni sono `--big-trade`, `--big-trades`, `--big-dominance`, `--speed`,
`--speed-dominance`, `--flow-window`, `--big-levels`, `--big-level-age`.

**Definizioni.** Un big trade e' un singolo cumulative trade sopra una soglia di volume: sul tape
di una sessione il 99esimo percentile e' 10 contratti e il 99,9esimo e' 34, quindi 20 seleziona lo
0,3% superiore delle stampe. La speed of tape e' volume aggressivo al secondo, separata per lato e
normalizzata sulla mediana della sessione stessa, perche' una soglia assoluta non e' trasferibile
fra una giornata di CPI e una di agosto.

### Letture Come Filtro Di Barra

477 operazioni, cap giornaliero disattivato perche' non tagli il campione, base 49,7%.

| big trades a favore | op | WR | z |
|---|---|---|---|
| 0 | 215 | 45,6% | -1,20 |
| 1 | 171 | 50,9% | +0,31 |
| 2 | 62 | 64,5% | **+2,34** |
| >= 3 | 29 | 41,4% | -0,89 |

| speed a favore | op | WR | z |
|---|---|---|---|
| < 1x mediana | 96 | 54,2% | +0,88 |
| 1-2x | 174 | 45,4% | -1,13 |
| 2-4x | 124 | 52,4% | +0,61 |
| >= 4x | 83 | 49,4% | -0,05 |

Il rapporto fra velocita' a favore e velocita' contro e' piatto o leggermente negativo a ogni
soglia. Ripetendo tutto su finestre di 30, 60 e 120 secondi invece della barra, il massimo |z| su
circa trenta celle e' 1,9 e i segni cambiano da una finestra all'altra.

Il 64,5% a due big trades e' l'unica cella significativa, sta fra due celle che non lo sono e la
cella successiva inverte. Con trenta confronti, un |z| massimo intorno a 2,2 e' quello che produce
il caso: non e' un risultato.

### Lettura Come Costruzione Del Livello

E' quella che Fabio usa davvero: i big trades non filtrano la candela, marcano il prezzo, e il
prezzo resta un livello per il resto della sessione. `--big-levels N` tiene gli N prezzi con piu'
volume di big trades accumulato **prima** della barra di segnale e richiede che la barra li tocchi.

| | op | op/sessione | WR | R |
|---|---|---|---|---|
| top 3 | 207 | 10,9 | 49% | -5,00 |
| top 5 | 270 | 14,2 | 50% | -0,61 |
| top 10 | 339 | 17,8 | 51% | +4,39 |
| top 20 | 387 | 20,4 | 50% | +2,39 |

Combinandola con il gate sul profile framing del giorno prima la frequenza scende a 1,6-3,1
operazioni per sessione, dentro l'intervallo dichiarato, e la percentuale di vittorie scende a
35-46%.

### Cosa Se Ne Ricava

Nessuna delle tre letture produce un edge misurabile su questo modello. Vale la pena essere
precisi su cosa significa e cosa non significa.

Non significa che big trades e speed of tape non contengano informazione. Significa che
**l'informazione non e' nella forma in cui l'abbiamo codificata**: un conteggio sulla barra di
segnale e una velocita' normalizzata sono proxy grossolane di cio' che Fabio legge, che e' una
sequenza — chi carica per primo, chi risponde, chi cede il livello — e che legge insieme al
posizionamento nel profilo e alla narrativa del COT.

Resta che, dopo aver implementato tutto cio' che il dossier e il primo transcript dichiarano in
forma verificabile, il modello misurato sta al 49-51% con obiettivo 1:1, cioe' sulla moneta. Le
parti che mancano non sono dettagli di parametro: sono la parte discrezionale.

## La Scala Della Barra

Domanda: e se 40R fosse troppo rumore? Per rispondere senza dipendere da quale chart e' caricato
in ATAS, `FabioOrderFlow/tools/build_bars.py` ricostruisce le barre dal tape a qualunque scala.
Il tape e' completo, quindi footprint, delta e value area si ricompongono per intero; l'unica
approssimazione dichiarata e' che il volume di un cumulative trade viene assegnato tutto a
`lastPrice`. Verifica sulla sessione del 9 settembre, barra M1 delle 13:30: volume ricostruito
6.333 contro 6.334 di ATAS, delta -81 contro -80, value area 29.443,75-29.466,75 contro
29.444,50-29.467,50.

Stesso mese, stessa finestra, stessa esecuzione, solo la scala cambia.

| scala | barre/sess. | durata mediana | R mediano | modello | nullo | differenza |
|---|---|---|---|---|---|---|
| 20R | 1.897 | 2 s | 2,50 pt | 50% | 47% | +3 |
| 40R | 523 | 7 s | 5,00 pt | 47% | 47% | 0 |
| 80R | 130 | 28 s | 10,00 pt | 54% | 49% | +5 |
| 160R | 34 | 116 s | 19,75 pt | 62% | 53% | +9 |
| 320R | 12 | - | - | 75% (4 op) | - | - |
| M1 | 120 | 60 s | 11,00 pt | 51% | 51% | 0 |
| M5 | 24 | 300 s | 24,62 pt | 44% | 50% | -6 |

La colonna del modello sale davvero da 47% a 62% allargando la barra. Ma **sale anche quella del
modello nullo**, da 47% a 53%, e la differenza fra le due resta tra -6 e +9 senza una direzione.

Non e' il modello che migliora: e' il costo fisso della geometria d'ingresso che pesa meno. Lo
stop sta a una value area di distanza, e l'handicap di esecuzione — spread, il tick fra il prezzo
limite e il prezzo che lo attraversa, la posizione in coda — e' un numero di tick pressoche'
costante. Quando R vale 2,50 punti quell'handicap e' una frazione grande del rischio; quando R
vale 19,75 punti e' trascurabile. La percentuale di vittorie a 1:1 sale di conseguenza, che ci sia
un modello sopra oppure no.

**Quindi 40R non e' "troppo rumore" in senso stretto.** E' la scala in cui l'attrito e' piu'
caro, e quindi quella che punisce di piu' un modello che non ha edge. Allargare la barra non
aggiunge informazione: toglie costo. A 160R restano 26 operazioni sul mese, cioe' 1,3 per
sessione, ed e' li' che il campione smette di poter dire qualcosa.

Le barre M1 vere caricate da ATAS danno lo stesso risultato delle M1 ricostruite: 76 operazioni,
49%, contro 85 e 51% — la differenza sta nei confini di barra, non nel comportamento.

## Misurare La Predittivita' Separatamente Dall'Esecuzione

Tutto cio' che precede passa attraverso tre scelte d'esecuzione — ingresso al bordo della value
area, stop largo una value area, obiettivo 1:1 — che hanno un costo proprio e che possono
seppellire un segnale che c'e' o fabbricarne uno che non c'e'. `probe_predictability.py` le toglie
di mezzo: dalla chiusura di ogni barra si cammina sul tape e si guarda quale barriera simmetrica
viene toccata prima, +X punti o -X punti. Nessun limite, nessuno spread, nessuna asimmetria.

Sulle barre M1 caricate in ATAS, mese, finestra 13:30-15:30, barriera +/-20 punti: 2.358
osservazioni, 2.270 risolte, 50,5% sale per prima.

### Il Correttivo Che Cambia Tutto

Le osservazioni **non sono indipendenti**: 114 barre per sessione, e con barriere a 20 punti le
finestre in avanti di barre consecutive coprono quasi lo stesso movimento. Contarle come 2.270
prove separate gonfia ogni z. La misura onesta calcola la statistica **per sessione** e poi
testa sulle 20 sessioni, cosi' che ogni giornata pesi una volta sola.

| misura | z ingenuo | t per sessione |
|---|---|---|
| volume 0,6-1,4x la mediana | +4,13 | +1,43 |
| minuti 15-45 dall'apertura | -3,60 | **-3,00** |
| minuti > 90 | +2,89 | +1,33 |
| POC del giorno prima, +10..+50 punti | +2,76 | +1,16 |
| flip di delta verso il basso | +2,36 | +1,63 |
| flip di delta verso l'alto | +0,22 | +0,16 |
| tutte le osservazioni | +0,46 | +0,24 |

Il volume passa da 4,1 sigma a 1,4: era il conteggio, non il mercato. Lo stesso vale per tutto il
resto.

Da notare che il flip di delta **verso il basso** e' la cella piu' alta fra quelle del modello, e
va nella direzione opposta alla condizione 01: quando il delta gira in giu', il prezzo tende a
salire. E' la lettura per assorbimento, non per continuazione. A t=+1,63 non e' un risultato, ma
e' l'unico segno che la condizione 01 possa avere il verso sbagliato.

### Cosa Resta In Piedi

Una sola cella sopravvive al correttivo: fra i 15 e i 45 minuti dall'apertura il prezzo scende per
primo nel 57,4% dei casi, t=-3,00 su 20 sessioni. Due ragioni per non costruirci sopra:

1. e' una fra circa trenta celle testate; con la correzione per confronti multipli non passa;
2. e' una deriva oraria misurata su **un solo mese**, cioe' esattamente il tipo di regolarita' che
   appartiene al periodo e non al mercato.

Serve un secondo mese, fuori campione. Il chart M1 in ATAS ne tiene 28.738 barre, che partono dal
13 agosto: per andare piu' indietro va alzato il numero di barre caricate nel chart.
