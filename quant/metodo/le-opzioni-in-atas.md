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

**Dal 30 ottobre 2026 i suoi valori si leggono dai file** (`quant/dati/xray/`), senza guardare il chart:
[`x-ray-in-vivo.md`](x-ray-in-vivo.md). Qui sotto, cosa mostra sul chart.

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

### Come Si Potrebbe Conoscere Il Segno (ricerca del 30 ottobre 2026)

Il segno dei dealer si **misura** solo con dati che dicono, per ogni trade, **chi** compra e vende
(cliente o market maker) e se **apre o chiude** una posizione. Il prezzo e l'open interest non bastano:
100 contratti in piu' di OI possono essere clienti che comprano o clienti che vendono.

| strumento | c'e' il dato? | dove | per noi |
|---|---|---|---|
| **SPX** (Cboe) | si', il migliore | Cboe Open-Close (a pagamento); i trade con la capacita' usati da [VAPG] | non lo compriamo; lo vediamo **filtrato da OptionsDepth** |
| **ES** in ATAS | si', ma **di SPX** | Options X-Ray (OptionsDepth, piano Ultra): posizioni per gruppo (retail, pro, market maker), ogni minuto | e' la **stima di un fornitore** su SPX, non le opzioni CME su ES; metodo non pubblicato. **Dal 30 ottobre 2026 la leggiamo** dai file: [`x-ray-in-vivo.md`](x-ray-in-vivo.md) |
| **NDX / QQQ** | in parte | Nasdaq NOTO (ISE, PHLX, NOM: apertura e chiusura per tipo di partecipante, a pagamento); Cboe Open-Close sulle sedi Cboe | QQQ e' quotato su piu' borse: ogni fonte ne vede un pezzo |
| **NQ** (opzioni CME) | **no** | il CME pubblica solo OI e volume totali; il COT da' i future piu' le opzioni in delta per categoria, settimanale, senza gamma ne' strike | nessuna fonte pubblica |

Cosa possiamo fare su NQ, dal piu' semplice:

1. **Il trade firmato**: nel feed del CME (MDP 3.0, tag 5797) ogni trade ha il lato
   dell'aggressore, anche sulle opzioni. Se il bridge registrasse i trade delle opzioni NQ, si
   saprebbe per strike chi ha comprato e chi venduto aggredendo (**da verificare** che ATAS e Rithmic
   lo passino sulle opzioni). Non dice chi e' il dealer ne' se il trade apre o chiude: e' la stessa
   ipotesi dei fornitori ("chi aggredisce e' il cliente"), ma su un dato vero.
2. **Il confronto con SPX** (si puo' fare dal 30 ottobre 2026, [`x-ray-in-vivo.md`](x-ray-in-vivo.md)): X-Ray su ES da' la posizione stimata dei market maker su SPX. Se su SPX
   la gamma misurata e la convenzione A dicono la stessa cosa nei giorni in cui la nostra GEX di ES
   la dice, la convenzione su NQ ha un indizio in piu'; non una prova (gli utenti di NQ non sono quelli
   di SPX).
3. **Comprare i dati di QQQ o NDX** (NOTO, Open-Close): misurano il segno su un sottostante vicino,
   non sulle opzioni CME su NQ.

I paper: su SPX con i dati Cboe la gamma dei market maker e' **di solito positiva**, negativa almeno per
un momento in un giorno su quattro [VAPG]; sommando call e put dello stesso strike i dealer risultavano
corti di gamma quasi ovunque nel 1996-2001 [GPP]. Su NQ nessuno l'ha misurata.

Le fonti: [Cboe, 0DTE e volatilita'](https://cdn.cboe.com/resources/education/research_publications/gammasqueezes.pdf),
[Cboe Insights](https://www.cboe.com/insights/posts/volatility-insights-evaluating-the-market-impact-of-spx-0-dte-options),
[OptionsDepth](https://optionsdepth.com/), [Nasdaq NOTO](https://m.nasdaqtrader.com/Micro.aspx?id=noto),
[CME MDP 3.0 Trade Summary](https://cmegroupclientsite.atlassian.net/wiki/spaces/EPICSANDBOX/pages/457418925/MDP+3.0+-+Trade+Summary),
[OCC per tipo di conto](https://dev.to/firmtape/how-much-of-the-spx-options-tape-is-dealer-to-dealer-a-free-daily-floor-from-occ-data-1ai3).

## Il VIX E Il VXN Rifatti Dalla Catena

**2 novembre 2026.** Provato nella scratchpad, poi portato nel server della pagina X-Ray
(`quant/strumenti/xray/XrayLive/Volatilita/`, che lo calcola ogni 5 minuti e lo archivia:
[lo strumento](../strumenti/xray/x-ray-lettore-e-dashboard.md#le-fonti-a-confronto)). Dalla catena
che il bridge legge (`/opzioni`: bid, ask e **settlement** di ogni strike, anche lontano) si applica
**la formula del Cboe**: la varianza a 30 giorni da tutte le opzioni fuori dal denaro, interpolata fra
due scadenze. Sulle opzioni ES e' il VIX; sulle opzioni NQ e' il VXN.

Le scelte, una per riga:
- **le scadenze**: i venerdi' prima e dopo i 30 giorni (27/11 e 4/12). Le giornaliere vicine, come la
  fine mese del 30/11, hanno strike ogni 5 punti, e i 120 strike per lato del bridge tagliano l'ala
  delle put a −18%;
- **il prezzo**: il settlement per la chiusura di ieri; il medio bid/ask (bid > 0) per adesso;
- gli strike senza prezzo si saltano; ci si ferma dopo due prezzi consecutivi ≤ 0,05;
- F per parita' put-call allo strike con |C−P| minimo; tasso 3,88%; scadenze alle 16:00 di New York.

| | ricostruito | ufficiale (Yahoo) |
|---|---|---|
| VIX, chiusura del 30/10 (settlement ES) | **19,29** | 19,42 |
| VXN, chiusura del 30/10 (settlement NQ) | **27,86** | 28,50 |
| VIX, 2/11 alle 05:45 di New York (medio bid/ask ES) | **18,58** | 18,91 alle 05:32 |

**La catena arriva a pezzi**: la prima lettura di NQ aveva 54 settlement su 226, e il risultato era
sbagliato (K0 di 300 punti lontano da F). Si rilegge finche' i settlement smettono di crescere.

A differenza della stima da X-Ray ([`x-ray-in-vivo.md`](x-ray-in-vivo.md), "La Volatilita' Implicita"),
questa e' la formula vera, con le ali. Ha due limiti: la storia comincia il 2 novembre 2026 (ATAS da' solo
il settlement di ieri e il vivo; da qui in avanti il server la archivia), e richiede ATAS aperto con la
connessione.

## Cosa Ne Puo' Fare Il Metodo

1. **Non si puo' fare un backtest con questi indicatori.** Non hanno storia: Chain registra da
   quando e' sul chart, l'OI e' giornaliero. Nel metodo un'idea diventa regola
   solo dopo il backtest e la validazione.
2. **La storia si costruisce da qui in avanti, ed e' gia' possibile** (14 ottobre 2026): il Data
   Bridge legge la catena delle opzioni dalla connessione (`/opzioni`) e `quant opzioni` la
   registra in `quant/dati/ES/opzioni/`, strike per strike, con open interest, volume e prezzi.
   La GEX si calcola poi nel framework, con la convenzione dichiarata e la sua alternativa,invece di fidarsi del calcolo chiuso degli indicatori, i cui valori **dal 30 ottobre 2026 si leggono** (sono una cache cifrata decifrata dal vivo: [`x-ray-in-vivo.md`](x-ray-in-vivo.md)). Dopo qualche mese c'e' un campione; dopo un anno, uno serio. Resta dentro la
   decisione 2 del progetto (dati solo da ATAS).
3. **Una storia lunga di GEX su NQ da fuori** (fornitori come SpotGamma, Menthor Q, OptionsDepth, o
   i dati CME) **esce dalla decisione 2**: servirebbe una richiesta esplicita.
4. **Prima i paper.** La fase 1 vale anche qui: si cercano i paper sulla copertura dei dealer, la
   gamma e la volatilita' intraday degli indici, se ne estraggono i finding e il perche', e solo
   dopo si scrive una scheda in `quant/studi/`. E' il prossimo passo.
