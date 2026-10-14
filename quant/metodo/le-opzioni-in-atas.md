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

## Cosa Ne Puo' Fare Il Metodo

1. **Non si puo' fare un backtest con questi indicatori.** Non hanno storia: Chain registra da
   quando e' sul chart, l'OI e' giornaliero. Nel metodo un'idea diventa regola
   solo dopo il backtest e la validazione.
2. **La storia si puo' costruire da qui in avanti**, registrando ogni giorno i livelli (Zero Gamma,
   muri, Max Pain, Expected Move, OI per strike) dall'indicatore o, meglio, i prezzi delle opzioni
   dalla connessione. Dopo qualche mese c'e' un campione; dopo un anno, uno serio. Resta dentro la
   decisione 2 del progetto (dati solo da ATAS). **Da verificare**: se il Data Bridge puo' leggere
   cio' che calcola un altro indicatore, o le quote delle opzioni.
3. **Una storia lunga di GEX su NQ da fuori** (fornitori come SpotGamma, Menthor Q, OptionsDepth, o
   i dati CME) **esce dalla decisione 2**: servirebbe una richiesta esplicita.
4. **Prima i paper.** La fase 1 vale anche qui: si cercano i paper sulla copertura dei dealer, la
   gamma e la volatilita' intraday degli indici, se ne estraggono i finding e il perche', e solo
   dopo si scrive una scheda in `quant/studi/`. E' il prossimo passo.
