# La Metodologia Istituzionale

Questo documento riporta **il metodo esattamente come lo espone il transcript**, fase per fase e
nell'ordine del transcript, senza aggiunte. Ogni affermazione porta il minuto da cui viene,
`[MC · 12:11]`. Dove il framework aggiunge o si discosta, lo dice un altro documento,
[`il-metodo-quantitativo.md`](../il-metodo-quantitativo.md), che a ogni fase rimanda qui.

**La fonte**: [`quant/metodo/fonti/matteo-conte-dalla-ricerca-alla-strategia.txt`](fonti/matteo-conte-dalla-ricerca-alla-strategia.txt),
episodio di Chart Fanatics con Matteo Conte. Secondo la presentazione ha mosso oltre 15 miliardi di
volume in una grande banca europea, fatto piu' di 30 milioni di euro per la banca e fatto girare
oltre 400 strategie insieme `[MC · 0:00 - 0:48]`.

**Il transcript e' una trascrizione automatica** di un video: termini e nomi a volte sono storpiati
("VWOP" per VWAP, "postix" per post-earnings, "Chpt" per ChatGPT). Qui sono scritti corretti; il
minuto permette di tornare al testo. I titoli dei paper sono quelli pronunciati nel video e vanno
verificati sul paper prima di citarli altrove.

## Le Fasi In Breve

| | fase | domanda a cui risponde | minuti |
|---|---|---|---|
| 0 | la differenza | inseguire operazioni o strategie validate? | 3:01 - 6:26 |
| 1 | l'idea | da dove viene, e perche' dovrebbe funzionare? | 4:16, 25:16 - 29:27 |
| 2 | le regole | ingresso, uscita, dimensione | 4:30 - 4:54, 29:35 - 31:16 |
| 3 | il codice | le regole dentro una macchina, un pezzo alla volta | 4:39 - 7:19, 23:27 - 25:16, 1:29:01 - 1:32:43 |
| 4 | il backtest | cosa avrebbero fatto le regole nella storia? | 7:19 - 9:11 |
| 5 | la validazione | era overfitting? quanto e' forte il vantaggio? | 9:11 - 18:31 |
| 6 | il vivo | automazione, sorveglianza di rendimento e rischio | 18:31 - 20:31 |
| 7 | il ciclo e il portafoglio | strategie poco correlate, revisione, ricerca continua | 1:32:43 - 1:37:39 |

Poi quattro esempi completi, dal paper alla strategia testabile `[MC · 33:41 - 1:28:50]`.

## 0. La Differenza Fra Retail E Istituzionale

- **Il retail insegue operazioni; l'istituzionale insegue strategie validate**: "in un guscio di
  noce" la differenza, e il motivo per cui il retail e' incostante mentre l'istituzionale guadagna
  quasi ogni mese `[MC · 3:01 - 3:57]`.
- **Inseguire un'operazione** e' puntare su un setup che "funziona il 70% delle volte". **Una
  strategia validata** e' una strategia che, **applicata sistematicamente ogni volta**, vince il
  70% delle operazioni: validata con i dati, la cosa piu' vicina alla certezza che si possa avere,
  perche' dice le probabilita' di successo `[MC · 5:00 - 5:58]`.
- **Una singola operazione non e' garantita** neanche con una buona strategia; **su cento
  operazioni**, statisticamente, se ne vince il 70% `[MC · 5:58 - 6:26]`.
- Il retail classico ha l'idea e la porta subito dal vivo con soldi veri; l'istituzionale segue la
  pipeline, e va dal vivo solo con una strategia validata, che ha dimostrato di poter fare soldi
  `[MC · 22:28 - 22:59]`.

## La Pipeline

Ogni trader istituzionale segue la stessa pipeline `[MC · 4:05 - 4:16]`:

```text
idea  ->  regole  ->  codice  ->  backtest  ->  validazione  ->  vivo
  ^                                                              |
  +-------------------------- si ricomincia ---------------------+
```

`[MC · 4:16 - 4:54]`, `[MC · 7:19]`, `[MC · 9:11]`, `[MC · 18:39 - 18:53]`, `[MC · 20:17 - 20:31]`.

## 1. L'Idea

**Da dove vengono.** Il 90%, poi detto 90-95%, delle strategie usate dai trader istituzionali su
qualunque trading floor **viene da paper di ricerca** `[MC · 25:42 - 26:06]`, `[MC · 27:43 - 28:00]`.
Il posto dove la maggior parte degli istituzionali andava a prenderle e' la **Social Science
Research Network (SSRN)** `[MC · 25:42 - 25:49]`: una raccolta enorme di ricerca, gratuita al 100%,
dove chiunque puo' andare a scavare per trovare qualcosa di interessante `[MC · 28:29 - 28:57]`.

**Due tipi di ricerca** `[MC · 28:00 - 28:42]`:

- **accademica**: professori universitari e ricercatori;
- **di industria**: gestori di hedge fund o trader, che studiano strategie specifiche (momentum,
  mean reversion, strategie sull'oro) e le pubblicano.

**Le due obiezioni, e la risposta.** "I paper sono difficili da leggere" e "il vantaggio e' gia'
decaduto" `[MC · 26:20 - 26:35]`. La risposta e' una sola:

> **Il paper non si copia.** Se ne guardano i *findings* e si cerca **la ragione sottostante, il
> perche'**: perche' quella strategia dovrebbe fare soldi in futuro. Il paper serve solo a far
> scattare l'idea che permette di inquadrare le regole della strategia `[MC · 26:27 - 27:22]`.

**Come si legge un paper.** Non si tratta di leggere 40 pagine: si usa la macchina, un modello
linguistico, per "craccarlo": di cosa parla la ricerca, quali sono i findings, perche' dovrebbe
fare soldi. Non si copia uno a uno, ma e' un punto di partenza estremamente importante per definire
le regole `[MC · 28:57 - 29:27]`. I paper sono di 30-60 pagine: si scaricano, si danno alla
macchina, se ne estrae il finding e il perche' `[MC · 1:04:46 - 1:05:09]`. Il finding e' affidabile
perche' molte persone ci hanno passato molte ore, e spesso i paper sono pubblicati su riviste
finanziarie con revisione fra pari `[MC · 1:05:21 - 1:06:18]`; sta a chi li legge prendere gli
elementi e combinarli in una strategia `[MC · 1:06:18 - 1:06:32]`.

**L'obiettivo** e' l'indipendenza: prendere l'idea, inquadrarla in ingresso, uscita e dimensione,
usare i modelli linguistici per averne versioni diverse e testarle da soli. Si puo' passare una
giornata su SSRN a trovare strategie da testare, "e se continui a scavare trovi l'oro", come su ogni
trading floor `[MC · 49:26 - 50:05]`.

## 2. Le Regole

Dopo l'idea si definisce **un insieme di regole cosi' specifico da poterlo dare a una macchina**, e
per darlo a una macchina va codificato `[MC · 4:30 - 4:54]`.

**Ogni strategia deve avere tre cose**, "tre caselle da riempire" `[MC · 29:35 - 30:15]`:

1. **l'ingresso**: le condizioni per cui si apre una posizione;
2. **l'uscita**: perche' si chiude la posizione dopo averla aperta. In genere **take profit e
   stop loss**; molto spesso anche **un'uscita legata al tempo**: chiudere dopo un'ora, o a un'ora
   precisa come le 15:00, se non si e' toccato ne' il take profit ne' lo stop
   `[MC · 30:15 - 30:48]`;
3. **la dimensione della posizione**: poco discussa fra i retail, ma fa una differenza enorme sia
   nel **ridurre il rischio di far saltare il conto** sia nel **migliorare il rendimento corretto
   per il rischio** `[MC · 30:48 - 31:16]`.

E serve **una ragione per cui la strategia dovrebbe farci soldi** `[MC · 33:28 - 33:36]`.

**Le regole restano semplici**, perche' bisogna avere il controllo di cio' che si fa
`[MC · 39:39 - 39:49]`. **Complessita' non vuol dire piu' profitto**: vuol dire piu' spesso
**fragilita'**, perche' ci sono piu' punti in cui la strategia si puo' rompere. L'80% dei piu' di
30 milioni fatti per la banca veniva da strategie estremamente semplici `[MC · 1:21:01 - 1:21:31]`.

**La dimensione in sviluppo e dal vivo** `[MC · 40:39 - 42:59]`:

- **in sviluppo, un numero fisso di contratti: uno.** Rende semplice sviluppare la strategia;
- **non e' cio' che si usa dal vivo**: con un contratto fisso, nei giorni a range largo si rischiano
  per esempio 30.000 $, in quelli stretti 1.000 $;
- su un trading floor la dimensione si fa **condizionata alla volatilita' dello strumento**. I due
  paper citati: *Volatility Managed Portfolios* e *The Impact of Volatility Targeting*;
- **volatility targeting**: si mette a rischio **sempre la stessa somma**, per esempio 10.000 $ a
  operazione, e il numero di contratti dipende dalla volatilita'. Giorno largo, meno contratti;
  giorno stretto, piu' contratti, sempre 10.000 $ a rischio;
- cosi' i giorni molto volatili non dettano il risultato della strategia: lo levigano, e
  migliorano **per definizione** il rendimento corretto per il rischio. Un cambiamento piccolo e
  semplice che puo' cambiare molto.

## 3. Il Codice

**La barriera.** Passare da regole ben definite a un pezzo di codice e' stata a lungo la barriera
d'ingresso piu' grande per il retail; nel 2023 i modelli linguistici (ChatGPT e gli altri) l'hanno
tolta `[MC · 6:26 - 7:19]`.

**Il modello linguistico e' uno strumento**, non uno sviluppatore senior `[MC · 1:29:30 - 1:29:50]`.
Si procede **per iterazioni** `[MC · 1:29:50 - 1:31:02]`. L'esempio del transcript:

1. si sviluppa solo la prima parte: "una strategia che va long alle 16:00 ET", e si controlla che
   funzioni;
2. se funziona, si aggiunge sopra la condizione successiva: "codifica un'uscita alle 9:30";
3. la si applica ai dati e ci si assicura che la logica funzioni come deve;
4. poi la dimensione: legare la posizione alla volatilita', piu' grande con volatilita' bassa, piu'
   piccola con volatilita' alta;
5. e cosi' via, finche' tutte le regole sono compilate in un pezzo di codice di cui si sa che
   funziona come deve.

**Gli strumenti** `[MC · 23:37 - 25:16]`, `[MC · 1:31:02 - 1:31:43]`:

- la codifica si fa con l'aiuto dei modelli linguistici; backtest e validazione sono automatizzati:
  si danno gli input e il processo produce i dati da rivedere;
- oggi anche il retail ha strumenti ben fatti per backtest e simulazioni. Il preferito di Matteo,
  senza affiliazioni, e' **MultiCharts**: il linguaggio (EasyLanguage) e' semplicissimo, **e' facile
  capire cosa fa il codice, cosa chiave per capire cosa si sta facendo**, e l'infrastruttura di
  backtest e' solida, con le simulazioni Monte Carlo;
- va bene anche **Pine Script** su TradingView; alla macchina si possono dare i manuali del
  linguaggio, per costruirle una base di conoscenza;
- **il punto non sono gli strumenti** ma l'accesso al processo: sapere quali passi e quali
  caselle vanno spuntate prima di portare una strategia dal vivo.

**Non serve automatizzare tutto** `[MC · 1:31:43 - 1:32:43]`: anche imparando a codificare una
versione base della strategia, per testarla come si deve e capire se ha senso, si puo' poi
automatizzarne solo una parte, o usarla **come conferma e continuare a operare a mano**. E' un
attrezzo in piu' nella cassetta.

## 4. Il Backtest

`[MC · 7:19 - 9:11]`:

- **cos'e'**: prendere l'insieme delle regole e **rigiocare la storia**. Ogni volta che le
  condizioni d'ingresso erano soddisfatte, si ha un'operazione simulata nel passato;
- **non si fa a mano**: si sfrutta il fatto che le regole sono codice, e si rigioca la storia;
- **produce una curva dei profitti**, che puo' anche scendere e poi salire se la strategia non e'
  abbastanza buona;
- **produce le statistiche** che dicono se si e' sulla strada giusta:
  - il **profitto netto**, se e' positivo;
  - la **percentuale vincente** sugli ultimi, per esempio, cinque anni;
  - **quanto vince in media** la strategia a ogni operazione;
  - il **drawdown**: la distanza dal picco alla valle del P&L cumulato in una fase negativa;
- **non e' l'ultimo passo**: poi si valida.

## 5. La Validazione

Un passo estremamente importante, **diviso in due fasi** `[MC · 9:11 - 9:26]`.

### 5a. Il test di divisione: dentro e fuori campione

`[MC · 9:26 - 12:01]`:

- si dividono i dati in **dentro campione** e **fuori campione**;
- **si lavora sulle regole sempre e solo con i dati dentro campione**. Con dieci anni di dati, per
  esempio, 80% dentro e 20% fuori;
- dentro campione si aggiungono e tolgono regole e parametri e si rifiniscono i parametri, finche'
  la strategia sembra comportarsi correttamente;
- poi **le stesse regole, congelate nel tempo, si danno ai dati mai visti**. Se la strategia rende
  ancora bene fuori campione, **non c'e' stato overfitting**;
- **l'overfitting** e' aggiungere troppe regole o parametri solo per far sembrare bella la curva
  dentro campione: sui dati mai visti la curva viene distrutta;
- buoni risultati dentro e fuori: **alta probabilita' che la strategia funzioni anche dal vivo**;
- la versione piu' semplice del controllo e' **confrontare i risultati dentro con quelli fuori**
  `[MC · 18:26 - 18:31]`.

Nell'esempio della PEAD il transcript mostra **a cosa serve la divisione nel lavoro vero**
`[MC · 1:14:21 - 1:15:18]`: si parte dalla versione semplificata; dentro campione si provano regole
in piu' (cosa succede aggiungendo uno stop loss? migliora il rendimento corretto per il rischio?);
se uno stop dell'1% e' il miglior risultato possibile dentro, lo si applica fuori per capire se era
overfitting o davvero la versione migliore. "E' un processo da seguire per assicurarsi di non
prendersi in giro."

### 5b. Il Monte Carlo

Sembra difficile, ma e' semplice `[MC · 12:11 - 12:24]`. Due versioni.

**Il rimescolamento** (*reshuffle*) `[MC · 12:24 - 15:38]`:

- si prendono **tutte le operazioni del backtest** e se ne **rimescola l'ordine**: si creano piu'
  versioni del passato, piu' percorsi della curva;
- si parte sempre dallo stesso punto e si arriva sempre allo stesso finale, perche' le operazioni
  sono le stesse; **cambia il percorso**;
- si ripete **migliaia di volte**;
- **piu' e' larga la dispersione** fra la curva piu' alta e la piu' bassa, **piu' e' debole il
  vantaggio**; una distribuzione stretta intorno alla media vuol dire che, in qualunque ordine
  siano arrivate le operazioni, l'esito era quello: **il vantaggio e' forte**;
- l'esempio del conduttore: nel test originale la serie massima di perdite consecutive era 15;
  rimescolando, potrebbe diventare 45, e allora le curve si allontanano e il vantaggio e' piu'
  debole; se resta 14 o 16, i dati sono coerenti;
- richiede poca potenza di calcolo, fa vedere **quanto si dipendeva dall'ordine delle operazioni**,
  e da' una **prima idea della distribuzione dei drawdown possibili**.

**Il ricampionamento** (*bootstrapping e resampling*) `[MC · 15:38 - 16:33]`:

- i classici "grafici a spaghetti", con tante curve;
- **cambia anche il punto d'arrivo**: le operazioni non si rimescolano soltanto, **alcune compaiono
  piu' volte e altre per niente**;
- si producono 10.000-20.000 simulazioni del passato.

**L'uscita del Monte Carlo** `[MC · 16:44 - 18:17]`:

- una **distribuzione degli esiti**: il profitto atteso della strategia; il rendimento atteso
  facendola girare 20.000 volte; **la probabilita' di un drawdown maggiore di 10.000 $**;
- **si usa anche dal vivo**: se dopo 10 operazioni ci si deve aspettare al massimo 10.000 $ di
  drawdown, e c'era solo il 5% di probabilita' di superarli, vederli superati e' **un segnale
  d'allarme**: forse va fatto un passo indietro e **fermata la strategia**;
- sono **metri** (*yardsticks*) che la validazione fornisce, per avere consapevolezza mentre si
  opera dal vivo.

**Solo quando tutti i passi sono completi** si arriva al vivo:

- verificato che non c'e' overfitting, confrontando dentro e fuori campione;
- fatto il Monte Carlo per vedere quanto e' forte il vantaggio, con 10.000-20.000 simulazioni.

`[MC · 18:17 - 18:53]`

## 6. Il Vivo

`[MC · 18:53 - 20:31]`:

- **non e' il trader seduto davanti ai monitor ad aspettare** che le condizioni d'ingresso si
  verifichino: come la strategia e' stata codificata, **il vivo e' automazione**. La macchina
  sorveglia i dati e, ogni volta che le condizioni sono soddisfatte, apre o chiude la posizione;
- **il lavoro del trader istituzionale** e' la ricerca: assicurarsi che le strategie dal vivo
  abbiano un **valore atteso positivo**, che piu' probabilmente che no chiuderanno con un P&L
  positivo;
- dal vivo il lavoro e' **uno: sorvegliare il rendimento; due: sorvegliare il rischio**;
- essendo automatiche, si puo' tornare indietro e **ricominciare il processo**: idea, regole e
  codice, backtest, validazione, vivo.

## 7. Il Ciclo E Il Portafoglio

`[MC · 1:32:43 - 1:36:01]`:

- **il punto d'arrivo non e' una strategia sola**: e' far girare **strategie poco correlate**, ed e'
  questo che permette agli istituzionali di guadagnare ogni mese. Una strategia che va bene in
  tendenza va male in un mercato che torna alla media, e un'altra che sfrutta il ritorno alla media
  la compensa;
- **il lavoro** e' imparare nel tempo a gestirle: limitare i drawdown di una, massimizzare i profitti
  dell'altra, e viceversa;
- **una volta al mese** si rivedono le strategie che girano dal vivo;
- **per un retail**: almeno **tre o quattro strategie insieme**, e per questo l'automazione: seguirne
  quattro a mano e' difficilissimo;
- in banca Matteo ne aveva **circa 400** insieme, con circa 15 miliardi di euro di volume l'anno; oggi
  fra **25 e 100 sistemi**, a seconda delle condizioni di mercato;
- il lavoro e' **sviluppare e ritoccare** di continuo: dal trading manuale all'analisi della storia,
  del vantaggio, alla rifinitura del rendimento;
- **le strategie smettono di funzionare**: serve un organismo che va dalla ricerca alla rifinitura,
  alla validazione, al dispiegamento, e poi di nuovo indietro.

Sul futuro `[MC · 1:36:11 - 1:37:39]`: il trading discrezionale resta, ma la distribuzione cambia.
Chi capisce che il computer puo' operare per lui, con tre o quattro sistemi, va in quella direzione:
tempo libero, piu' fiducia, **nessuna emozione che interferisce**.

## Gli Esempi: Dal Paper Alla Strategia Testabile

Quattro esempi "per andare da un paper di ricerca a una strategia testabile" `[MC · 27:22 - 27:36]`.
Per ognuno lo stesso schema: **i findings, il perche', le regole (ingresso, uscita, dimensione)**.

### Esempio 1. Market Intraday Momentum

`[MC · 33:41 - 50:05]`. Paper accademico del 2018, gratuito su SSRN `[MC · 34:03 - 34:25]`. Spiega
perche' l'**opening range breakout** ha funzionato cosi' bene negli ultimi sei anni
`[MC · 34:25 - 34:48]`.

- **Findings**: **il gap della notte piu' i primi 30 minuti** (9:30-10:00) rivelano lo squilibrio
  fra compratori e venditori e **predicono l'ultima mezz'ora** della seduta regolare. Rendimento
  positivo dalle 16:00 alle 10:00, rendimento positivo anche nell'ultima mezz'ora
  `[MC · 35:06 - 35:56]`.
- **Perche'**: dopo la chiusura escono informazioni nuove (**notizie, utili, posizionamento
  globale**); gli ordini che ne nascono si scontrano nei primi 30 minuti e rivelano lo squilibrio.
  Se i compratori sono molti piu' dei venditori, il mercato non lo digerisce in 30 minuti e lo porta
  nel resto della giornata: **momento intraday** `[MC · 35:56 - 37:11]`.
- **Non si copia**: replicando ingresso, uscita e dimensione del paper si ha una bella curva fino al
  2018 e poi una pessima: **alpha decay**. Si prende invece il finding, lo squilibrio che si rivela
  nei primi 30 minuti, per impostare un classico opening range breakout, sapendo perche' dovrebbe
  funzionare: non tutti i partecipanti operano prima dell'apertura `[MC · 37:18 - 38:32]`.
- **Ingresso**: **long se il prezzo chiude sopra il massimo del range 9:30-10:00** `[MC · 38:32 - 39:05]`.
- **Uscita**: **stop al minimo del range**; **take profit con rapporto rischio/rendimento fra 1 e 2**
  (a 1:1, tanto sopra quanto lo stop e' sotto); **chiusura alle 15:30 ET** se non si tocca ne' lo
  stop ne' il take profit. "Deve restare semplice, perche' dobbiamo avere il controllo"
  `[MC · 39:05 - 40:31]`.
- **Dimensione**: un contratto in sviluppo, poi volatility targeting, come nella fase 2
  `[MC · 40:31 - 42:59]`.
- **La versione short** (rottura del minimo) si puo' provare, ma per esperienza **le strategie sullo
  squilibrio dei primi 30 minuti funzionano meglio solo long**: rotto il livello al ribasso entrano
  molti compratori e lo squilibrio si riassorbe; al rialzo e' difficile che entrino altri
  compratori, e c'e' un effetto di deriva `[MC · 43:04 - 43:55]`.
- **Il perche' strutturale**: molti operatori **possono operare solo dopo le 9:30 ET**, e per questo
  non mettono ordini prima. Se potessero, l'anomalia sparirebbe; se le cose cambiassero (fondi
  pensione che operano prima delle 9:30), l'effetto potrebbe sparire. Sapere perche' funziona
  cambia la prospettiva su cio' che si fa `[MC · 46:23 - 47:23]`.
- **Indicazioni dell'accademia da verificare da soli** `[MC · 48:25 - 49:26]`:
  - i **primi 30 minuti** sono il range ottimale per un opening range breakout;
  - lo **solo long** funziona meglio, e **non e' solo il beta**: ha reso bene anche nel 2022, con il
    mercato in calo;
  - il **rapporto 1:1 o 1:2** e' l'ottimo.

  L'obiettivo e' testarlo da soli.

### Esempio 2. VWAP, The Holy Grail Of Day Trading Systems

`[MC · 50:05 - 59:46]`. Paper di industria, molto pratico `[MC · 50:13 - 51:11]`.

- **La VWAP**: il prezzo medio di tutto cio' che e' stato scambiato fino a un dato momento, pesato
  per il volume `[MC · 51:11 - 51:50]`.
- **Findings**: **long quando il prezzo e' sopra la VWAP, short quando e' sotto, chiusura quando il
  prezzo riattraversa la VWAP**: rendimenti sistematicamente buoni. Su **QQQ** (l'ETF del NASDAQ 100)
  **a un minuto**: ogni barra M1 che chiude sopra la VWAP apre un long, chiuso quando una barra
  richiude sotto; uguale e contrario per lo short. Circa **671% in 5 anni** `[MC · 52:02 - 54:10]`.
- **Oltre i titoli**: il numero non interessa, interessa capire cosa hanno fatto e perche'
  `[MC · 54:10 - 54:33]`.
- **Perche': gli algoritmi VWAP.** Ogni trader istituzionale manda almeno una volta al giorno un
  ordine di esecuzione, spesso con un **algoritmo VWAP**, che cerca un'esecuzione il piu' vicina
  possibile alla VWAP partecipando al volume: piu' volume, piu' ordini. Molti compratori o venditori
  che inseguono la VWAP producono movimenti direzionali forti, e il fatto che la grande maggioranza
  degli istituzionali usi questi algoritmi **amplifica i movimenti agli attraversamenti**
  `[MC · 54:33 - 55:57]`.
- **Cosa cambiare**: non serve usarla su QQQ; si puo' testare su **NQ**, sullo stesso sottostante,
  oppure su ES, crude, singole azioni. **Il codice si scrive una volta e si applica a qualunque
  strumento**, purche' li' gli istituzionali usino algoritmi VWAP: "questa logica dovrebbe
  funzionare su tutti gli strumenti", primo insegnamento importante `[MC · 55:57 - 57:04]`.
- **Ingresso**: long quando il prezzo **chiude** sopra la VWAP, short quando chiude sotto
  `[MC · 57:04 - 57:33]`.
- **Uscita**: una sola condizione, il prezzo che torna alla VWAP `[MC · 57:33 - 57:42]`.
- **La VWAP da quando**: dall'apertura, oppure incorporando la VWAP del giorno prima; sono parametri.
  **Il consiglio forte e' dall'apertura**: dalle 9:30 per gli USA, dalle 9:00 per alcuni mercati
  europei. Nel backtest si cerca **quanti minuti o ore** servono per sfruttare al meglio il volume
  `[MC · 57:42 - 58:52]`.
- **Dimensione**: in sviluppo **sempre un contratto**; sulle azioni una percentuale del portafoglio
  (per esempio il 10% di 100.000 $), oppure condizionata alla volatilita'. Le versioni diverse si
  provano dopo, quando la strategia punta nella direzione giusta `[MC · 58:52 - 59:46]`.

### Esempio 3. Post-Earnings Announcement Drift (PEAD)

`[MC · 59:54 - 1:19:33]`. Non un paper ma **una raccolta di ricerche**: una delle prime
inefficienze documentate, 60 anni di ricerca, **ancora viva**. L'alpha decay esiste, ma c'e' sempre
un angolo diverso da cui affrontarla, sempre partendo dalla ricerca: non funziona su un tipo di
azione, forse funziona su un altro `[MC · 1:00:02 - 1:01:42]`.

- **La logica**: con una **sorpresa positiva negli utili** e una **reazione positiva del prezzo**,
  nei giorni e nelle settimane dopo l'azione continua a derivare verso l'alto; con utili mancati o
  reazione negativa, deriva verso il basso `[MC · 1:01:42 - 1:02:40]`.
- **I tre paper** `[MC · 1:02:40 - 1:08:07]`:
  - **1968**, *An Empirical Evaluation of Accounting Income Numbers*: la prima osservazione, la
    forma pura del drift;
  - **1989**, *Post-Earnings-Announcement Drift: Delayed Price Response or Risk Premium?*: il drift
    dura **circa 60 giorni di borsa**, circa tre mesi;
  - **2006**, *Comparing the Post-Earnings Announcement Drift for Surprises Calculated from Analyst
    and Time Series Forecasts*: introduce **il consenso degli analisti**. Il drift e' piu' forte
    se l'utile per azione effettivo supera la stima degli analisti, e la reazione negativa e' piu'
    forte se la manca. Oggi e' lo standard.
- **Come si combinano gli elementi** `[MC · 1:06:32 - 1:07:13]`: la reazione del prezzo (un gap al
  rialzo il giorno dopo, dall'apertura alla chiusura) e' il primo strato; poi i 60 giorni; poi gli
  analisti.
- **Perche' l'informazione non entra subito nel prezzo**, contro l'ipotesi dei mercati efficienti
  che si insegna all'universita' `[MC · 1:08:07 - 1:11:05]`:
  1. **bassa copertura**: non tutte le azioni sono seguite bene dagli analisti, l'informazione
     viaggia lenta. Un paper del 2021 mostra che **sulle mega cap non c'e' vantaggio**, sulle small
     e mid cap si';
  2. **vincoli di liquidita'**: un gestore che vuole costruire una posizione grande non riesce ad
     allocare tutto il capitale in un giorno, e spalma l'esecuzione su 10 giorni o un paio di
     settimane. E' questo che genera il drift.
- **Ingresso**: reazione positiva del prezzo **e** utile per azione sopra la stima: long
  **all'apertura del giorno dopo** `[MC · 1:11:05 - 1:11:45]`.
- **Uscita**: **60 giorni**, come punto di partenza suggerito dalla ricerca `[MC · 1:11:45 - 1:12:08]`.
  Sullo stop: va testato. Con una strategia cross-section su 20-30 azioni, l'uscita piu' semplice e'
  il tempo; poi si puo' aggiungere uno strato di gestione del rischio, per esempio uno stop del 5%
  sotto l'apertura del giorno o, in caso di gap al rialzo, uno stop alla chiusura prima della
  pubblicazione degli utili `[MC · 1:13:14 - 1:14:07]`.
- **Dimensione**: una percentuale del portafoglio (1-2% per azione), una cifra fissa, oppure
  condizionata al movimento del prezzo o allo scarto fra utile effettivo e stimato. Si testano le
  varianti per trovare quella col miglior rendimento corretto per il rischio
  `[MC · 1:12:08 - 1:12:53]`.
- **Il backtest** parte dalla versione semplificata, con la sola uscita a 60 giorni, e dentro
  campione si aggiungono le regole (vedi 5a) `[MC · 1:14:16 - 1:15:18]`.
- **Altri angoli**: oggi funziona meglio sulle small e mid cap, ma non solo USA: anche sul mercato
  europeo, dove la copertura e l'interesse sono minori e la liquidita' e' diversa. "Questo e' il
  nuovo modo di pensare di un istituzionale: collegare i punti" `[MC · 1:15:28 - 1:16:33]`.
- **Lo short funziona sempre meno**: le societa' preannunciano i risultati negativi, l'amministratore
  delegato gestisce le aspettative, e la reazione arriva prima e piu' attenuata. Capita, piu'
  raramente, anche per i positivi. La base della strategia e' **la sorpresa** per il mercato
  `[MC · 1:16:41 - 1:19:33]`.

### Esempio 4. L'Anomalia Della Notte

`[MC · 1:21:39 - 1:28:50]`. Il mercato chiude alle 16:00 e apre alle 9:30 `[MC · 1:22:14 - 1:22:41]`.

- **Il paper**: 2008, *The Return Difference Between Trading and Non-Trading Hours: Like Night and
  Day* `[MC · 1:22:41 - 1:22:50]`.
- **Findings**: su piu' indici azionari e piu' azioni, il **90% circa dei rendimenti** (per esempio
  dell'S&P 500) **veniva dal possesso nella notte**: la seduta regolare 9:30-16:00 sostanzialmente
  piatta, poi il gap della notte `[MC · 1:22:50 - 1:23:43]`.
- **La prova da fare da soli**: **ingresso long alle 16:00, uscita alle 9:30** all'apertura. Su NQ
  dal 2015 a oggi (giugno 2026), di nuovo **il 90% del rendimento dalla notte e il 10% dalla seduta
  regolare**. Da decenni, e su piu' indici `[MC · 1:23:43 - 1:25:08]`.
- **Perche'**: piu' teorie, nessuna definitiva `[MC · 1:25:08 - 1:27:40]`:
  1. **premio per il rischio della notte**: chi tiene la posizione a mercato chiuso, con meno
     liquidita', va ricompensato. Ma il 90% sembra troppo, ad alcuni accademici e professionisti,
     per spiegarlo cosi';
  2. **liquidita' della notte**: di notte escono informazioni (notizie, utili, come nel primo
     esempio); con poca liquidita' la reazione del prezzo e' piu' aggressiva di quanto sarebbe
     di giorno, e nella seduta regolare c'e' un'inversione verso il valore equo. Per Matteo, un
     perche' "molto forte".
- **Il punto di partenza per un'idea nuova**: se il 90% dei rendimenti arriva di notte, con poca
  liquidita', **perche' non un opening range breakout che si fa solo di notte**, dove stanno i
  movimenti direzionali piu' grandi? Questo e' tutto il processo: un'idea fatta scattare dalla
  ricerca, inquadrata in una strategia, poi codificata e testata `[MC · 1:27:40 - 1:28:50]`.

## Cosa Il Transcript Non Dice

Per non attribuirgli cio' che non c'e':

- non nomina un **modello nullo** ne' un confronto con l'ingresso a caso;
- non distingue fra osservazioni **per operazione e per giornata**;
- non tratta il **numero di varianti provate** (multiple testing) ne' lo Sharpe deflazionato;
- non fissa **soglie** per dire validata una strategia: dice "buoni risultati dentro e fuori" e
  "dispersione stretta";
- non tratta **costi, commissioni e slittamento**, ne' l'ordine degli eventi dentro una barra;
- non dice come trattare i **rollover** dei future;
- non dice cosa fare se **il fuori campione e' gia' stato guardato**.

Il framework li aggiunge, e li dichiara come aggiunte, in [`il-metodo-quantitativo.md`](../il-metodo-quantitativo.md).
