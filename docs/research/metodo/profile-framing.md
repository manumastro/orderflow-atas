# Profile Framing: Cosa Guardare, E In Che Ordine

Fonte: primo live del corso, `fabio_course/fabio_q1/fabio_live_charting_1_q1.txt`.
Stato: **procedura di lettura**. Descrive cosa osservare, non cosa eseguire.

Nel live la sequenza e' dichiarata senza ambiguita': **"the first step is profile framing"**. La
location viene prima del segnale. L'order flow conferma un livello gia' marcato; non lo crea.

## Le Sei Cose Da Guardare

### 1. Solo La Cash

> *"I don't use opening and closing price. I use only the cash session."*

Ogni profilo si costruisce sulla sola sessione cash, **13:30-20:00 UTC** (09:30-16:00 New York).
La notte si osserva a parte, per il retest dei bordi, ma non entra nel profilo.

Conseguenza pratica: un chart daily a barre di 24 ore **non e' il profilo che serve**. La barra
daily impasta cash e notte in una distribuzione sola.

### 2. Dove Si Sta Costruendo Il Valore

Tre distribuzioni di giorni consecutivi, e la domanda e' una: il valore sta salendo, scendendo, o
sta fermo?

> *"What the market is telling me? The value is building up."*

La definizione di shift e' letterale e va applicata cosi':

```text
shift su     VAL piu' alto  E  VAH piu' alto  del giorno prima
shift giu'   VAL piu' basso E  VAH piu' basso
dentro       tutto il resto: il valore non si e' spostato, si e' allargato o ristretto
```

### 3. I Profili Che Si Sovrappongono Si Uniscono

> *"You can merge. These two profiles you can merge, remember."*

Due sedute con aree di valore sovrapposte descrivono la stessa asta. Vanno lette come un blocco
unico, e il bordo che conta e' quello del blocco, non quello del singolo giorno.

### 4. Le Due Aree Sensibili

> *"In this case, I have two sensitive area, one is the value area as we saw before for the
> session, and another one is the POC."*

Per ogni blocco si marcano **area di valore** (VAL e VAH) e **POC**. E un terzo livello che il live
distingue esplicitamente:

> *"It's important also the POC of the session that created the breakout."*

Il POC della seduta che ha prodotto la rottura resta un livello anche dopo, ed e' spesso il primo
punto che il mercato ritrova sulla via del ritorno.

### 5. Dove Si Sta Nella Curva

> *"Here you are high in the curve. Here you are in the bottom of the curve. How can you check the
> curve using the value area? You see that here you are on the value area low."*

E' la domanda che decide quale dei due modelli si applica:

```text
dentro il valore, mercato in bilancia   ->  mean reverting: dal bordo al POC
sul bordo, con il valore che si sposta  ->  momentum: rottura e continuazione
```

### 6. Il Vuoto

> *"If we manage to break this level, we have a gap. And this gap, it's the first level that the
> market will aggressively want to rebalance."*
> *"Market maker don't like inefficient transaction."*

Dove il volume sparisce, il prezzo non trova attrito. I colli fra due nodi e i gap non riempiti
sono zone di transito, non di sosta: si attraversano in fretta o si respingono al bordo.

---

## Il Quadro Di Adesso — 15 Settembre 2026, Pre-Market

Costruito sulla sola cash, undici sedute, con il pre-roll riportato in termini NQZ6 (+291 punti).

| Data | Contratto | Volume | Delta | POC | VAL | VAH | Shift |
|---|---|---|---|---|---|---|---|
| 08-31 | U6* | 323.990 | +728 | 29.711 | 29.675 | 29.740 | |
| 09-01 | U6* | 390.649 | +6.107 | 29.416 | 29.373 | 29.504 | giu' |
| 09-02 | U6* | 304.665 | +2.759 | 29.451 | 29.418 | 29.496 | dentro |
| 09-03 | U6* | 388.005 | +2.851 | 29.819 | 29.618 | 29.875 | su |
| 09-04 | U6* | 344.165 | **-15.967** | 29.811 | 29.772 | 29.881 | su |
| 09-08 | U6* | 353.985 | -8.477 | 29.901 | 29.804 | 29.922 | dentro |
| 09-09 | U6* | 342.343 | +2.699 | 29.734 | 29.718 | 29.810 | giu' |
| 09-10 | U6* | 385.727 | +987 | 29.451 | 29.398 | 29.514 | giu' |
| 09-11 | U6* | 348.466 | -6.220 | 29.706 | 29.674 | 29.746 | su |
| **09-14** | **Z6** | 249.978 | **+3.282** | **29.450** | **29.306** | **29.604** | giu' |

### Il Composito: Due Nodi E Un Collo

3.456.825 contratti, blocchi da 25 punti, termini NQZ6:

```text
 29.900-29.924    97.469   -1.781   ##############
 29.875-29.899   110.597   -4.301   ################
 29.850-29.874   161.014   -3.932   #######################
 29.800-29.824   271.077   -3.077   ########################################   <- vertice
 29.775-29.799   179.371   -4.665   ##########################
 29.750-29.774   187.398   -1.114   ###########################
 29.725-29.749   243.228     +422   ###################################
 29.700-29.724   246.493   -2.435   ####################################
 29.675-29.699   190.113   -2.001   ############################
 ---------------------------------  collo
 29.650-29.674    85.323     -591   ############
 29.625-29.649    44.843     -479   ######
 29.600-29.624    37.113     +601   #####
 ---------------------------------
 29.550-29.574    81.976     -248   ############
 29.525-29.549   100.232   +2.296   ##############
 29.500-29.524   122.043     +319   ##################
 29.475-29.499   139.904   +2.270   ####################
 29.450-29.474   214.311   +2.543   ###############################            <- vertice
 29.425-29.449   218.284   +1.540   ################################
 29.400-29.424   156.890   +1.630   #######################
 29.375-29.399    97.368     +104   ##############   <- prezzo ora, 29.386
 29.350-29.374    68.969     +225   ##########
 29.325-29.349    54.408     -592   ########
 29.300-29.324    34.663     -109   #####
```

- **Nodo superiore 29.675-29.924**, vertice a 29.800-29.824. **Delta negativo in tutto il cuore.**
  Li' i venditori sono stati aggressivi, e il prezzo se ne e' andato: hanno ottenuto.
- **Collo 29.600-29.674**: 167.279 contratti su 75 punti, contro 271.077 sul solo blocco di
  vertice superiore. E' la zona di transito fra i due nodi.
- **Nodo inferiore 29.400-29.549**, vertice a 29.425-29.474. **Delta positivo ovunque.** Li' i
  compratori sono stati aggressivi e hanno ottenuto la tenuta.

Quattro sedute diverse hanno messo il POC fra 29.416 e 29.451 in termini Z6: 09-01, 09-02, 09-10 e
09-14. **Non e' un livello di una seduta, e' una mensola.**

### Dove Sta Il Prezzo

29.386 alle 06:14 UTC. Cioe':

- **appena sotto il bordo inferiore del nodo inferiore** (29.400);
- **dentro** l'area di valore di lunedi' (29.306-29.604), nella meta' bassa;
- **sotto** il POC di lunedi' (29.450), che coincide con il vertice del nodo;
- **sopra** il VAL di lunedi' (29.306), testato stanotte a 29.308 e tenuto.

Il minimo della notte cade **due punti** sopra il VAL. Non e' una coincidenza da interpretare, e'
un bordo che ha funzionato una volta.

### Sotto Il VAL C'e' Uno Scaffale, Non Un Vuoto

Questa e' una **correzione** a una lettura sbagliata fatta in giornata, e vale come esempio del
modo in cui un composito puo' ingannare.

Nel composito qui sopra il blocco 29.300-29.324 ha 34.663 contratti su 3.456.825: sembra sottile,
e da li' e' facile concludere che sotto il VAL non ci sia attrito. E' un errore di lettura. Quel
blocco e' sottile **perche' una sola delle undici sedute c'e' andata** — e quella seduta e'
lunedi', cioe' ieri. Il composito misura quante sedute hanno visitato un prezzo, non quanto quel
prezzo pesa nella struttura recente.

Il profilo della sola seduta di lunedi' (globex intera, 333.382 contratti, blocchi da 25 punti):

```text
 29.450-29.474    27.942   +440   ####################################   <- POC, la mensola
 29.425-29.449     9.121   +141   ############
 29.400-29.424     3.671    +33   #####
 29.375-29.399     3.871   +327   #####                                  <- la fascia sottile
 29.350-29.374    19.097   +693   #########################
 29.325-29.349    20.574   +804   ###########################
 29.300-29.324    33.628   +830   ############################################
 29.275-29.299    28.551   +243   #####################################
 29.250-29.274    13.970   +670   ##################
 29.225-29.249    10.179   +563   #############
 29.200-29.224    18.540   +628   ########################
 29.175-29.199    28.449   +697   #####################################
 29.150-29.174    13.332   +578   #################
 29.125-29.149     7.182   +252   #########
 29.100-29.124     1.213    -83   #
```

Due fatti che ribaltano la lettura:

1. **La fascia sottile e' sopra, non sotto.** 29.375-29.424 vale l'1,1-1,2% per blocco: e' li' che
   il prezzo non trova attrito. Fra 29.175 e 29.324, invece, lunedi' ha scambiato **il 27% della
   seduta**. Un rientro verso la mensola e' veloce; una discesa sotto il VAL non lo e'.
2. **Il delta e' positivo in ogni blocco della discesa**, fino a +830 sul blocco piu' pesante. I
   compratori hanno assorbito mentre il prezzo scendeva, e poi lunedi' ha chiuso a 29.449,50. Non
   e' una zona attraversata, e' una zona difesa.

I due minimi di lunedi' non coincidono e vanno tenuti distinti: **29.167,75 in cash** (15:37) e
**29.107,25 in globex** (11:25). Per il framing, che usa solo la cash, il minimo e' 29.167,75.

**La regola che ne esce.** Un blocco sottile nel composito significa una sola cosa: poche sedute
ci sono passate. Prima di chiamarlo vuoto va guardato il profilo della seduta piu' recente che
quel prezzo l'ha visitato. Se quella seduta e' ieri, non e' un vuoto: e' l'ultima cosa che il
mercato ha costruito.

---

## Cosa Non Fare, Sul Tuo Chart

**Il chart NQ continuous daily non e' back-adjusted.** Verificato sui dati: la barra del
2026-09-13 corrisponde a NQZ6 (H 29.604, L 29.107), quella del 2026-09-10 ancora a NQU6
(H 29.500, L 29.040). C'e' uno splice grezzo di circa 291 punti dentro la serie.

Conseguenza sul framing: fra venerdi' e lunedi' il chart mostra il valore che scende e poi risale.
Corretto per lo spread, l'area di venerdi' in termini Z6 era **29.674-29.746** e quella di lunedi'
**29.306-29.604**: il VAL scende di 368 punti e il VAH di 142. Il valore e' sceso e non e'
risalito. **Lo shift vero e' giu', il grafico ne mostra un altro.**

### Come Si Back-Adjusta

L'API ATAS espone soltanto le **date** di rollover (`ContractRolloverType`: `ExpirationDate`,
`VolumeBasedCurrentEnd`, `VolumeBasedNextStart`), non una correzione di prezzo: il continuo di
ATAS incolla e basta. Se la piattaforma offrisse un'opzione di back-adjust nelle impostazioni
dello strumento andrebbe attivata li'; sui dati serviti dal bridge non risulta attiva, perche' la
barra del 09-10 corrisponde a NQU6 al tick.

`FabioOrderFlow/tools/build_continuous.py` lo fa fuori dalla piattaforma, e per il profile framing
e' meglio, perche' sposta **anche il footprint**: POC e value area storici diventano confrontabili
invece che solo le OHLC.

```bash
python3 FabioOrderFlow/tools/bridge.py candles --chart NQU6 --from 2026-08-31 --to 2026-09-15 --levels --out u6.json
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 --from 2026-08-31 --to 2026-09-15 --levels --out z6.json
python3 FabioOrderFlow/tools/build_continuous.py u6.json z6.json --cash 13:30-20:00 --out continuo.json
```

Come misura lo spread, e perche' cosi':

- la data del roll e' la **prima giornata in cui il contratto nuovo supera il vecchio per volume**,
  non la scadenza;
- lo spread e' la **mediana della differenza fra le chiusure al minuto**, presa solo sui minuti in
  cui **entrambi** i contratti hanno scambiato almeno cinque lotti. Il filtro serve a scartare i
  minuti in cui il contratto lontano ha una sola stampa ferma, che userebbe un prezzo vecchio;
- misurato cosi' sul roll di settembre 2026: **+293,75 punti, mediana su 3.505 minuti appaiati,
  deviazione standard 4,15**. Non e' una stima su due chiusure, e' una mediana su migliaia di
  osservazioni;
- l'aggiustamento e' **per differenza** e si applica ai contratti **piu' vecchi**, cosi' la serie
  finisce nei prezzi del contratto corrente: i livelli che leggi sul grafico sono prezzi operabili
  oggi, non prezzi storici da riconvertire.

Nota su cosa l'aggiustamento **non** conserva: dopo il back-adjust i prezzi storici non sono piu'
quelli a cui si e' scambiato in quei giorni. Va bene per i livelli e per la forma del profilo, che
e' l'uso qui; non va bene per citare un prezzo storico.

Con il continuous grezzo il framing va comunque fatto sui contratti singoli con lo spread
dichiarato. Il continuous di ATAS resta buono per la forma di lungo periodo, non per i bordi.
