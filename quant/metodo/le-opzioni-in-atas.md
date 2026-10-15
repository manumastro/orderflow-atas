# Le Opzioni In ATAS: Cosa Sono Gli Indicatori, E Cosa Ci Si Puo' Fare

**14 ottobre 2026.** Un territorio nuovo per il progetto. Questa pagina raccoglie tre cose: cosa
dice Fabio delle opzioni, cosa offre ATAS (dalle pagine ufficiali), e cosa ne puo' fare il metodo.
**Nessuna regola**: e' la mappa prima dei paper.

## I Concetti, In Breve

- **Opzione**: il diritto di comprare (*call*) o vendere (*put*) a un prezzo fissato (*strike*)
  entro una scadenza. **0DTE**: le opzioni che scadono oggi.
- **Open interest (OI)**: i contratti aperti su uno strike. Si aggiorna **una volta al giorno**.
- **Gamma**: quanto cambia il delta di un'opzione quando il sottostante si muove. Chi vende opzioni
  (il *market maker*, o *dealer*) si copre comprando o vendendo il future; la gamma dice quanto deve
  ricoprirsi a ogni movimento.
- **GEX (gamma exposure)**: la gamma dei dealer sommata per strike, in $ per 1% di movimento. Con
  **GEX positiva** il dealer compra quando il prezzo scende e vende quando sale: **smorza** il
  movimento. Con **GEX negativa** fa il contrario: lo **amplifica**.
- **Zero Gamma** (o *gamma flip*): il prezzo dove la GEX totale cambia segno; separa i due regimi.
- **Call Wall / Put Wall**: gli strike con la GEX positiva / negativa piu' forte; letti spesso come
  resistenza e supporto.
- **Max Pain**: lo strike dove le opzioni in scadenza pagano meno ai loro possessori.
- **Expected Move**: il movimento atteso fino alla scadenza, a 1 e 2 deviazioni standard, dalla
  volatilita' implicita.

Queste letture (GEX positiva smorza, i muri tengono) sono **ipotesi diffuse, non risultati**: vanno
provate come ogni altra idea.

## Cosa Dice Fabio

Dall'intervista su YouTube in spagnolo (`fabio_course/fabio_youtube_interview.txt`, agosto 2026):

- "**Precio, volumen y option flow son tres caras de la misma moneda**": le opzioni creano il
  bisogno di copertura sui future, il volume mostra l'aggressione di quella copertura, il prezzo
  risponde. "El option flow es la razón."
- Il **piano pre-mercato** parte da "la presión de opciones está aquí", poi il livello di order
  flow, poi chi domina.
- Il Nasdaq del 2026 e' **compresso dalle opzioni** e dal volume enorme degli 0DTE: "falta
  volatilidad", la seduta "si inchioda 4 o 5 ore con 20 falsi breakout"; modelli che funzionavano
  da 10 anni (IB, breakout) ne soffrono.
- Guardare solo i future da' "una visione del 30-40% dei volumi".
- Usa anche **un modello in opzioni di *premium harvesting*** (incassare il decadimento del tempo,
  la theta), che rende quando la volatilita' manca e compensa il direzionale.
- Sta studiando "una metodologia di analisi che va dall'option flow all'order flow al prezzo"; non
  dice come.

Sono affermazioni di un trader, non misure. Il metodo le tratta come **spunti**: diventano regole
solo passando la pipeline.

## Cosa Offre ATAS

Dalle pagine ufficiali (help.atas.net, il changelog, il listino), lette il 14 ottobre 2026. Due
suite, uscite il 2 ottobre 2026.

### Options Chain: funziona su NQ

Opzioni dalla **propria connessione** (Rithmic o Interactive Brokers; CQG in sviluppo): bid, ask,
ultimo, open interest e volume del giorno per gli strike intorno al prezzo. **Volatilita' implicita
e greche le calcola ATAS** dai prezzi; il modello non e' documentato. Piano Pro o Ultra; serve un
abbonamento ai dati delle opzioni del proprio connettore.

| indicatore | cosa mostra |
|---|---|
| **Options OI Profile** | open interest o volume del giorno per strike, call e put ai due lati; modalita' Net OI |
| **Options Key Levels** | Max Pain, Call Wall, Put Wall, livelli di volume; **senza Zero Gamma** |
| **Options Expected Move** | cono o corridoio a 1σ e 2σ fino alla scadenza, dalla volatilita' implicita |
| **Options GEX Profile** | GEX per strike, con Zero Gamma, muri, Max Pain, Expected Move, curva cumulata; storia dei livelli solo da quando lo si aggiunge |

Il limite che conta: **"Some intraday visuals start collecting data after they are added to the
chart"**. L'OI e' giornaliero; il resto si registra solo da quando l'indicatore e' sul chart.

### Options X-Ray: non funziona su NQ

Dati di **OptionsDepth** sulle opzioni sull'indice S&P 500 (SPX) del CBOE, aggiornati **ogni
minuto**, con le posizioni divise per tipo di partecipante (retail, professionisti, market maker).
Dieci indicatori (dodici pagine di aiuto): Key Levels (con Flip e Pin), Expected Move, Strike
Profile (GEX, DEX, VEX, CEX: gamma, delta, vanna, charm), Surface Profile, Strike Heatmap, GEX
Heatmap, Charm Heatmap, Flow, Flow Delta, Big Trades, Market State, Hedge Pressure. Piano Ultra.

**Solo su ES, MES, SPY e US 500**: "On any other instrument (NQ, oil, gold, individual stocks,
crypto) the indicator draws nothing."

Le fonti, una per indicatore:
[panoramica](https://help.atas.net/en/support/solutions/articles/72000661129-how-to-use-option-analytics-indicators-in-atas),
[GEX Profile](https://help.atas.net/en/support/solutions/articles/72000661536-options-gex-profile),
[Key Levels](https://help.atas.net/en/support/solutions/articles/72000661534-options-key-levels),
[Expected Move](https://help.atas.net/en/support/solutions/articles/72000661535-options-expected-move),
[OI Profile](https://help.atas.net/en/support/solutions/articles/72000661533-options-oi-profile),
[Chain o X-Ray](https://help.atas.net/en/support/solutions/articles/72000662644-options-chain-vs-options-x-ray-which-to-use),
[changelog del 2 ottobre](https://feedback.atas.net/changelog/a-new-era-of-analysis-with-options-x-ray-suite),
[listino](https://atas.net/pricing/).

**Non confermato** dalle pagine: che le opzioni lette su NQ siano quelle CME sul future NQ (e non
su NDX o QQQ), come si calcolano Zero Gamma e le greche, con che frequenza.

### Sul Nostro Chart, Il 14 Ottobre

Sulla tab `2 SETTIMANA M30` (NQZ6, connessione Rithmic) ci sono **due** indicatori:

- **Options GEX Profile** (Chain), scadenza di oggi (0DTE), modalita' Split su Vol. Legge:
  - Zero Gamma 30.768,25;
  - Call Wall 30.840;
  - Put Wall 30.350;
  - Max Pain 30.750;
  - Expected Move ±231,54;
  - σ 28,1%, T 0,3 giorni.

  Il prezzo, 30.526,50, sta **sotto** lo Zero Gamma.
- **un indicatore Options X-Ray**, che su NQ scrive solo "SPX options data · for ES, MES, SPY and US
  500 charts only" e non disegna niente, come da documentazione.

## La GEX Calcolata Da Noi

**Dal 15 ottobre 2026** `FabioOrderFlow/tools/gex.py` calcola la GEX di NQZ6 dalla catena che il
bridge legge (`/opzioni`), e il giro d'orizzonte la stampa nella tab 2. Il modello, una riga per
scelta:

- prezzo dell'opzione: il medio fra bid e ask, se no l'ultimo;
- volatilita' implicita: **Black-76** (il modello delle opzioni su future), tasso zero, dalla sola
  opzione fuori dal denaro di ogni strike; vale per la call e per la put;
- scadenza: le 16:00 di New York; le trimestrali alle 9:30 (la SOQ del CME);
- **GEX di uno strike** = gamma x open interest x 20 $ x F² x 1%: i dollari di future che il dealer
  compra o vende per un movimento dell'1%;
- **convenzione A** (fornitori, SqueezeMetrics): dealer lunghi le call, corti le put; GEX = call - put;
- **convenzione B** [GPP]: dealer corti di tutto; la GEX e' negativa ovunque e dice solo dove pesa;
- **zero gamma**: dove la GEX totale A cambia segno, ricalcolando la gamma con la volatilita' di ogni
  strike ferma, su +-8% dal prezzo a passi dello 0,1%. Fino al 15 ottobre la griglia era di +-4%, e
  alle 11:04 la settimana non aveva uno zero dentro;
- **il future**: l'ultimo prezzo scambiato (chiusura della barra in corso del chart M30) quando il
  bridge legge la scadenza. Le scadenze si leggono una dopo l'altra in circa tre minuti, quindi ognuna
  ha il suo (`scadenze[].future`, `future_quando`). Il `future` del riassunto e' quello dell'ultima
  lettura, datato da `future_quando`; `prezzo_bridge` e' l'M1 subito dopo, con la differenza. Il 15
  ottobre il future (29.975,5) non tornava con il bridge delle 18:43 (29.946,25): non era sbagliato,
  era di tre minuti dopo, ma il JSON lo datava all'inizio del calcolo;
- **gli strike**: 120 per lato per scadenza (era 100). Il tetto del bridge e' 500 sottoscrizioni per
  richiesta (`TettoSottoscrizioni` in `LeOpzioni.cs`): 120 strike x 2 lati x 2 tipi = 480. Un giorno con
  piu' serie ne prende meno (`strike_per_lato` per scadenza);
- **le riletture**: la catena arriva a pezzi, e `gex.py` rilegge ogni scadenza ogni 3 secondi finche'
  l'open interest cresce meno dello 0,5%. Lo fa fino a sei volte per le scadenze entro sette giorni e
  per le trimestrali, due volte per le settimanali lontane, che sono quotate a pezzi e pesano circa il 7%
  della GEX A. Con sei riletture per tutte il calcolo passava da tre minuti e mezzo a cinque e mezzo.

**Il segnale dell'open interest.** L'OI "di ieri" cambia fra le letture, e dentro una lettura cresce
mentre le quotazioni arrivano. Per ogni scadenza il JSON ha `oi_prima_lettura`, `oi_ultima_lettura`,
`letture` e `oi_stabile`: falso se dopo le riletture l'OI cresceva ancora. In
`oi_cambiato_rispetto_al_calcolo_precedente` c'e' invece il confronto con l'ultimo calcolo archiviato
dello stesso giorno: OI totale (sulle scadenze comuni) e i cinque strike piu' pesanti. Il 15 ottobre,
fra le 18:43 e le 19:10, il totale e' salito del 24%; buona parte viene dai 120 strike al posto di 100 e
dalle settimanali lontane quotate meglio. Un OI che si muove molto e' una catena arrivata a pezzi, non
posizioni nuove.

**La finestra del volume.** `volume` e' il `CurrentDayTotalVolume` della quotazione di ATAS: il giorno di
contrattazione del CME. Per le opzioni sui future comincia alle 17:00 di Chicago del giorno prima, cioe'
alle 00:00 italiane in ora legale. Alle 08:32-09:51 UTC del 15 ottobre i +-6% avevano gia' 19-21 mila
contratti, quindi la notte e' dentro. L'azzeramento alle 17:00 di Chicago non e' ancora visto: una
lettura fra le 22:00 e le 23:00 UTC deve dare quasi zero. Il JSON lo dice in `volume_finestra`.

**I limiti**: l'open interest e' di ieri e non vede gli 0DTE aperti e chiusi in giornata; il segno
vero dei dealer non si conosce [GPP]; il GEX Profile di ATAS in modalita' Vol usa il volume del
giorno, non l'open interest, e non coincide con questo calcolo. **La misura della scala**: il 15
ottobre la GEX A totale era -1,5 miliardi di $ per 1%, cioe' circa 2.500 future NQ da vendere per un
ribasso di 300 punti, contro 500-700 mila contratti scambiati al giorno. Su NQ le opzioni sul future
sono piccole rispetto al future; su SPX ed ES il rapporto e' un altro.

## Cosa Ne Puo' Fare Il Metodo

1. **Non si puo' fare un backtest con questi indicatori.** Non hanno storia: Chain registra da
   quando e' sul chart, l'OI e' giornaliero. Nel metodo un'idea diventa regola
   solo dopo il backtest e la validazione.
2. **La storia si costruisce da qui in avanti, ed e' gia' possibile** (14 ottobre 2026): il Data
   Bridge legge la catena delle opzioni dalla connessione (`/opzioni`) e `quant opzioni` la
   registra in `quant/dati/ES/opzioni/`, strike per strike, con open interest, volume e prezzi.
   La GEX si calcola poi nel framework, con la convenzione dichiarata e la sua alternativa, invece
   di fidarsi del calcolo chiuso degli indicatori, i cui valori stanno in campi offuscati e non si
   leggono. Dopo qualche mese c'e' un campione; dopo un anno, uno serio. Resta dentro la
   decisione 2 del progetto (dati solo da ATAS).
3. **Una storia lunga di GEX su NQ da fuori** (fornitori come SpotGamma, Menthor Q, OptionsDepth, o
   i dati CME) **esce dalla decisione 2**: servirebbe una richiesta esplicita.
4. **Prima i paper.** La fase 1 vale anche qui: si cercano i paper sulla copertura dei dealer, la
   gamma e la volatilita' intraday degli indici, se ne estraggono i finding e il perche', e solo
   dopo si scrive una scheda in `quant/studi/`. E' il prossimo passo.
