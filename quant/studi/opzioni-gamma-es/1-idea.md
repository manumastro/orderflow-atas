# La Gamma Dei Dealer Su ES

**Stato**: idea.
- Paper letti per intero il 14 ottobre 2026; nessuna strategia codificata.
- ES M1 in archivio dal 2017 (dal 25 ottobre 2026).
- **Dal 5 novembre 2026 ci sono anche le ipotesi D ed E**, da un video, non da un paper. D e' stata
  **descritta** su 42 sedute di X-Ray: con il regime noto alle 9:35, i giorni con lo 0DTE negativo non
  sono stati piu' larghi (rapporto mediano 0,55 contro 0,74, p 0,24), e un allargamento di un terzo come
  nel video e' poco compatibile con questi dati. La prova vera e' in avanti, con il criterio scritto.
  E: un caso solo, aneddotico.
- Lo strumento e' ES su richiesta dell'utente (CLAUDE.md, decisione 2): gli indicatori X-Ray di ATAS
  disegnano solo su ES.

**Strategia nel catalogo**: nessuna.

## Da Dove Viene

Fabio: "precio, volumen y option flow son tres caras de la misma moneda", le opzioni "creano il
bisogno di copertura sui future", e quest'anno "comprimono" il Nasdaq
([`le-opzioni-in-atas.md`](../../metodo/le-opzioni-in-atas.md)). Sono affermazioni di un trader: qui
diventano domande, e le risposte si cercano nei paper.

## Il Paper

Il filone `opzioni-gamma` dell'[indice](../../paper/indice-dei-paper.md#opzioni-gamma). Letti per
intero:

| sigla | paper | analisi |
|---|---|---|
| BDLM | Baltussen, Da, Lammers, Martens (2021), *Hedging demand and market intraday momentum*, JFE | [analisi](../../paper/opzioni-gamma/baltussen-2021-hedging-demand-and-market-intraday-momentum/analisi.md) |
| BB | Barbon, Buraschi (2021), *Gamma Fragility*, working paper | [analisi](../../paper/opzioni-gamma/barbon-2021-gamma-fragility/analisi.md) |
| BTW | Baltussen, Terstegge, Whelan (2024), *The Derivative Payoff Bias*, working paper | [analisi](../../paper/opzioni-gamma/baltussen-2024-the-derivative-payoff-bias/analisi.md) |
| GJ | Golez, Jackwerth (2012), *Pinning in the S&P 500 futures*, JFE | [analisi](../../paper/opzioni-gamma/golez-2012-pinning-in-the-sp500-futures/analisi.md) |
| GPP | Garleanu, Pedersen, Poteshman (2009), *Demand-Based Option Pricing*, RFS | [analisi](../../paper/opzioni-gamma/garleanu-2009-demand-based-option-pricing/analisi.md) |
| VAPG | Vasquez, Amaya, Pearson, Garcia-Ares (2025), *0DTE Index Options and Market Volatility*, Cboe | [analisi](../../paper/opzioni-gamma/vasquez-2025-0dte-index-options-and-market-volatility/analisi.md) |
| SM | SqueezeMetrics (2017), *Gamma Exposure*, white paper | [analisi](../../paper/opzioni-gamma/squeezemetrics-2017-gamma-exposure/analisi.md) |

Tutti i numeri qui sotto vengono dalle analisi, che citano la sezione del paper.

## Il Finding

**La gamma**: quanto cambia il delta di un'opzione quando il sottostante si muove. Chi ha venduto
opzioni (il dealer, o market maker) si copre sul future; se e' **lungo di gamma** compra quando il
prezzo scende e vende quando sale, e smorza il movimento; se e' **corto di gamma** fa il contrario, e
lo amplifica.

1. **Il momento dell'ultima mezz'ora** [BDLM §3-4]. In 62 future, 1974-2020, il rendimento dalla
   chiusura di ieri alle 15:30 predice quello dalle 15:30 alle 16:00. Su NQ t 7,97 (R quadro fuori
   campione 3,76%), su ES t 4,97 (2,29%), Newey-West. **Sui future S&P c'e' solo nei giorni di gamma
   netta negativa dei dealer**: coefficiente 6,63 (t 4,78) contro 0,82 (t 1,03) [BDLM §4.1, Tab. 7].
2. **La gamma e la volatilita'** [BB]. Con gamma dei dealer alta il giorno prima, il rendimento
   assoluto del giorno dopo e' piu' basso (sugli indici oltre 20 punti base per una deviazione
   standard, su una media di 180) e l'autocorrelazione intraday piu' bassa. Sugli indici il segno dei
   dealer e' **assunto**, non misurato. [VAPG]: gamma dei market maker di solito positiva, negativa in
   parte di almeno un giorno su quattro; l'effetto sulla volatilita' mediano e' piccolo.
3. **La scadenza del terzo venerdi'** [BTW]. Su ES, 2003-2021, dalle 16:00 del giovedi' alle 9:30 del
   venerdi' di scadenza +14,3 punti base (t 3,54), poi dall'apertura alla chiusura -12,9 (t -2,40);
   comprare alla chiusura del giovedi' e vendere all'apertura rende 24,2 punti base dopo lo spread,
   senza commissioni. Piu' forte sulle scadenze trimestrali.
4. **Il pinning** [GJ]. Il future S&P chiude vicino allo strike il 21,3% dei venerdi' di scadenza
   seriale contro il 15% atteso (29 su 136), quasi tutto dal basso e nell'ultima mezz'ora; sulle
   scadenze regolate in contanti il contrario.
5. **Il segno della GEX e' fragile** [GPP, SM]. La GEX dei fornitori assume dealer lunghi di call e
   corti di put. Su SPX 1996-2001 e' vero per tipo di opzione, ma sommando call e put dello stesso
   strike i dealer risultano **corti di gamma quasi ovunque**. SqueezeMetrics non da' ne' test ne'
   campione fuori.
6. **I livelli che ATAS disegna** (Zero Gamma o Flip, call wall, put wall) **non sono provati da
   nessun paper**.

## Il Perche'

La copertura dei dealer e' un flusso meccanico e prevedibile: dipende dalle posizioni in opzioni e da
quanto si e' mosso il prezzo, non da un'opinione. Corti di gamma, devono comprare dopo un rialzo e
vendere dopo un ribasso, e lo fanno soprattutto verso la chiusura, quando ribilanciano il delta per la
notte [BDLM §2]. Alla scadenza il delta delle opzioni vicine allo strike cambia di colpo, e la
copertura attira o spinge il prezzo rispetto allo strike [GJ §2, BTW §2].

## Cosa La Ucciderebbe

- **Il segno sbagliato.** Se la stima di chi e' lungo e chi e' corto di gamma e' sbagliata [GPP], la
  condizione "gamma negativa" non seleziona niente. Le posizioni vere dei dealer non sono pubbliche;
  ATAS le stima (Chain, dall'open interest e con la convenzione) o le prende da OptionsDepth (X-Ray).
- **Gli 0DTE.** L'open interest si aggiorna una volta al giorno e non vede le posizioni aperte e
  chiuse in giornata; sull'effetto degli 0DTE sulla volatilita' i paper si contraddicono.
- **Il decadimento.** Effetti pubblicati dal 2021 in poi, su un mercato pieno di algoritmi.
- **I costi.** Un'operazione al giorno su mezz'ora, o una a mese sulla scadenza: i costi pesano.
- **Il campione.** Le scadenze sono 12 l'anno: per [BTW] servono circa 72 scadenze per un t di 2.

## Le Regole Da Provare

Tutte su ES, 1 contratto, convenzioni di esecuzione del motore ([`4-il-backtest.md`](../../metodo/4-il-backtest.md)).
Le stesse ipotesi A e C si provano anche su NQ, che e' lo strumento del progetto e su cui [BDLM] trova
l'effetto piu' forte. **Varianti dichiarate prima di provarle:**

**Ipotesi A, il momento dell'ultima mezz'ora** [BDLM]. Solo prezzi: si prova appena c'e' la storia.
- ingresso alle 15:30 ET nel verso del rendimento dalla chiusura del giorno prima (16:00) alle 15:30;
- uscita alle 16:00 ET, a tempo; nessuno stop;
- varianti: A1 il rendimento dalla chiusura di ieri; A2 il rendimento della prima mezz'ora (9:30-10:00),
  la versione di *Market Intraday Momentum*; A3 A1 solo se il rendimento supera in valore assoluto la
  sua mediana degli ultimi 60 giorni.

**Ipotesi B, la stessa, solo con gamma negativa** [BDLM Tab. 7, BB]. Serve la gamma di ogni giorno:
nessuno storico, si registra da qui in avanti.
- B1 il prezzo alle 15:30 e' sotto lo Zero Gamma del GEX Profile di ATAS (Chain);
- B2 la gamma netta dei market maker di X-Ray e' negativa alle 15:30;
- confronto: B contro A nello stesso periodo.

**Ipotesi C, la scadenza** [BTW]. Solo prezzi e calendario.
- long alla chiusura delle 16:00 ET del giovedi' prima del terzo venerdi', uscita all'apertura delle
  9:30 ET del venerdi';
- varianti: C1 tutte le scadenze mensili; C2 solo le trimestrali.

**Ipotesi D, il regime dello 0DTE all'apertura e l'ampiezza del resto della giornata** (5 novembre
2026). **Fonte: un video**, non un paper: la trascrizione e' in
[`docs/research/fonti-video/2026-11-05-il-bias-dalle-opzioni.txt`](../../../docs/research/fonti-video/2026-11-05-il-bias-dalle-opzioni.txt),
citata [V2 · m:ss]. Si collega a [BB] (gamma alta, giorno dopo piu' calmo) e a [VAPG] (effetto mediano
piccolo).

- *Cosa dice il video, in parole semplici.*
  - I market maker fanno da ammortizzatori quando sono lunghi di gamma, e li tolgono quando sono corti
    [V2 · 0:52-1:20].
  - Con lo 0DTE (le opzioni che scadono oggi) negativo, la giornata e' piu' larga: 70 punti di SPX
    contro 51, in 101 sedute da aprile a settembre.
  - Il regime non dice la direzione: "la direzione non cambia" [V2 · 1:39-2:16].
  - Il regime va riletto durante il giorno, perche' cambia [V2 · 2:42-3:06].
- *Il perche'*: e' lo stesso meccanismo della sezione "Il Perche'". Corti di gamma, i market maker si
  coprono nel verso del movimento e lo allungano; lunghi, lo frenano.
- **La trappola della misura del video.** Il video classifica la seduta col regime che "e' rimasto
  negativo per gran parte della giornata" [V2 · 1:42-1:52], cioe' guardando la giornata finita.
  - La GEX dello 0DTE **cambia col prezzo**: se il prezzo si allontana dagli strike pesanti, la gamma
    vicino al prezzo cambia segno anche con le stesse posizioni
    ([come-si-leggono-le-opzioni-dal-vivo](../../metodo/come-si-leggono-le-opzioni-dal-vivo.md), punto 4).
  - Una giornata larga diventa "negativa" perche' e' larga: il risultato si conferma da se'.
  - **La misura giusta** usa il regime **noto all'apertura**, e confronta il resto della seduta con il
    **movimento atteso**: nei giorni di paura il movimento atteso e' gia' piu' grande, e il range grezzo
    lo confonde col regime.
- **Le regole della misura** (dichiarate prima di guardare, scritte nello script prima della prima
  esecuzione):
  - regime = segno della GEX netta dei market maker sullo 0DTE di SPX (X-Ray, `gex__mm_0dte`, call + put,
    tutti gli strike), alla colonna piu' recente fra le **09:30 e le 09:35** di New York. Senza colonna
    in quella finestra la seduta si esclude;
  - ampiezza = massimo meno minimo di ES dalle 09:35 alle 16:00 di New York;
  - movimento atteso = **VIX ufficiale di chiusura del giorno prima** × ES alle 9:35 / √252 (una
    deviazione standard di un giorno). Controllo: la deviazione standard dei rendimenti di cassa di ES
    delle 20 sedute prima × ES;
  - misura = ampiezza / movimento atteso; direzione = rendimento dalle 9:35 alle 16:00;
  - nullo = permutazione delle etichette del regime, 10.000 volte, p a due code;
  - varianti: **D1** la GEX netta su tutti gli strike (la principale); **D2** solo gli strike entro
    l'1% dallo spot; **la trappola**, il regime della giornata come nel video (negativo se lo e' in
    almeno meta' dei minuti di cassa), solo per mostrare quanto cambia.
- **Cosa la ucciderebbe**:
  - con il regime noto all'apertura, i giorni negativi non piu' larghi dei positivi, sul movimento
    atteso;
  - il segno di OptionsDepth sbagliato [GPP];
  - lo 0DTE che pesa poco sull'ampiezza [VAPG].
- **La descrizione del 5 novembre 2026** (42 sedute, dal 08/09 al 04/11):
  [`5-validazione/risultati/d-regime-0dte-e-ampiezza-2026-11-05.md`](5-validazione/risultati/d-regime-0dte-e-ampiezza-2026-11-05.md).
  - Rapporto mediano: **0,55 nei giorni negativi** (16 sedute) contro **0,74 nei positivi** (26). La
    differenza va nel verso opposto al video, con p 0,24.
  - La direzione non cambia col regime (p 0,37).
  - Contando il regime sulla giornata intera, come nel video, la media va nel suo verso: 0,85 contro
    0,69, p 0,14. Ma lo fa per la trappola.
  - Il criterio della prova in avanti (le sedute dal 6 novembre) e' scritto nel rapporto.

**Ipotesi E, il muro vicino al bordo del valore tiene o si rompe secondo il regime** (5 novembre 2026,
stessa fonte).

- *Cosa dice il video*:
  - un muro put vicino al VAL, con gamma positiva, attira l'assorbimento e la rotazione verso il POC;
  - con gamma negativa si rompe; un muro call vicino al VAH, al contrario, esaurisce o viene sfondato
    [V2 · 8:08-8:38];
  - i due esempi dello stesso giorno: alle 10:20 un muro put rotto in gamma negativa [V2 · 6:35-7:09], e
    nel pomeriggio un muro che tiene 12 volte in gamma positiva [V2 · 7:11-7:54].
- *Il perche'*: lunghi di gamma, i market maker comprano mentre il prezzo scende verso lo strike, e il
  livello fa da freno; corti, vendono anche loro, e il livello cede.
- **I livelli di X-Ray ci sono solo dal 23 ottobre 2026** (`levels__gamma`), quindi per ora **si
  registra**. Cosa si registra, per ogni seduta:
  - **muro** = lo strike con la GEX call (o put) dei market maker piu' grande in valore assoluto, sullo
    0DTE, alle 9:35, portato su ES con la base ES-SPX mediana della seduta;
  - **vicino** = entro 4 punti di ES dal VAL o dal VAH della cassa del giorno prima (il valore lo
    calcola l'indicatore, ruolo della tab M30);
  - **tocco** = il primo minuto dopo le 9:35 in cui ES arriva entro 1 punto dal muro; da li' si sa da
    che parte arriva il prezzo;
  - **regime al tocco** = il segno della GEX netta 0DTE all'ultima colonna prima del tocco;
  - **rompe** = una chiusura a 5 minuti oltre il muro di almeno 3 punti, nei 60 minuti dopo il tocco;
    altrimenti **tiene**.
- **Cosa la ucciderebbe**: muri che tengono o cedono allo stesso modo nei due regimi; oppure muri
  che non tengono piu' di un prezzo qualunque alla stessa distanza (il nullo, quando ci saranno casi).
- **Dal 23/10 al 4/11, un caso solo**. Il 3/11 il muro put era a 3 punti dal VAL, la gamma era
  negativa, e il muro ha tenuto: il contrario di cio' che il video farebbe aspettare. E' un aneddoto
  (rapporto qui sopra). Servono decine di casi: con questa soglia, uno ogni due settimane circa.

**Non provate, e perche'.** Il pinning [GJ] chiede circa 220 scadenze: non le avremo. I livelli di
ATAS (muri, Zero Gamma) non hanno ne' paper ne' storia: si **descrivono** con la registrazione, e
diventano un'ipotesi solo con una scheda nuova.

## Cosa Serve Prima

1. **La storia di ES da ATAS**: quanti anni di barre a 1 minuto da `quant scarica --simbolo ES`. ES e'
   ora nel framework (`Strumento.ES`, 50 $ a punto).
2. **La registrazione delle opzioni**: dal 14 ottobre 2026 `quant opzioni` registra la catena delle
   opzioni CME su ESZ6 dalla connessione di ATAS (strike, tipo, open interest, volume, prezzi), il
   dato grezzo. Va lanciata ogni giorno a orari fissi (proposta: 9:35, 12:00, 15:30, 16:00 ET), con
   ATAS aperto. I valori calcolati dagli indicatori di ATAS (GEX Profile, X-Ray) **non si leggono**:
   stanno in campi offuscati. La gamma la calcoleremo noi, con le due convenzioni di segno.
   **Aggiornamento del 30 ottobre 2026**: quelli di X-Ray ora si leggono (la stima di OptionsDepth su
   SPX, con il segno dei market maker per strike), [`x-ray-in-vivo.md`](../../metodo/x-ray-in-vivo.md);
   quelli del GEX Profile (Chain) no. Le varianti dichiarate sopra non cambiano.
4. **La storia di ES, misurata**: `CandlesDataProvider` da' solo 30 giorni di borsa; il resto
   dipende da quanti giorni carica un chart, da misurare.
3. **Il calendario delle scadenze**: i terzi venerdi', con le festivita'.

## Le Prove

Nessuna prova di strategia.

| data | cosa | esito | rapporto |
|---|---|---|---|
| 2026-11-05 | ipotesi D ed E, **descrizione** su 42 sedute di X-Ray (08/09-04/11), non una prova: niente registro | D1: giorni negativi alle 9:35 non piu' larghi (0,55 contro 0,74 del movimento atteso, p 0,24); un allargamento di un terzo e' poco compatibile. E: un caso | [d-regime-0dte-e-ampiezza-2026-11-05](5-validazione/risultati/d-regime-0dte-e-ampiezza-2026-11-05.md) |
