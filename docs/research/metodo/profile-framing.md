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

---

## Cosa Non Fare, Sul Tuo Chart

**Il chart NQ continuous daily non e' back-adjusted.** Verificato sui dati: la barra del
2026-09-13 corrisponde a NQZ6 (H 29.604, L 29.107), quella del 2026-09-10 ancora a NQU6
(H 29.500, L 29.040). C'e' uno splice grezzo di circa 291 punti dentro la serie.

Conseguenza sul framing: fra venerdi' e lunedi' il chart mostra il valore che scende e poi risale.
Corretto per lo spread, l'area di venerdi' in termini Z6 era **29.674-29.746** e quella di lunedi'
**29.306-29.604**: il VAL scende di 368 punti e il VAH di 142. Il valore e' sceso e non e'
risalito. **Lo shift vero e' giu', il grafico ne mostra un altro.**

Finche' il continuous non e' back-adjusted, il framing va fatto sui contratti singoli con lo spread
dichiarato, come nella tabella sopra. Il continuous resta buono per la forma di lungo periodo, non
per i bordi.
