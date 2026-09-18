# COT Gold — Rilevazione Del 18 Settembre 2026

Fonte: `tradingster.com/cot/legacy-futures/088691`, vista **Legacy**, variante **futures only**,
serie estratta dal `dataProvider` di amCharts con il server MCP Playwright.
Rilevazione: **martedi 8 settembre 2026** a chiusura. Lettura: **venerdi 18 settembre**.
Serie completa: [`../data/cot-legacy-gold-weekly.csv`](../data/cot-legacy-gold-weekly.csv),
**520 settimane dal 2016-09-20**.

Passo 3 dei nove per l'apertura di **GCZ6** ([`../metodo/come-si-apre-un-asset.md`](../metodo/come-si-apre-un-asset.md)).
Metodo in [`../metodo/analisi-istituzionale.md`](../metodo/analisi-istituzionale.md).

Stato: **descrizione**. Nessuna regola operativa, nessun segnale, nessuna soglia.

## Le Dichiarazioni Obbligatorie

```text
codice Tradingster   088691   GOLD - COMMODITY EXCHANGE INC.
vista                LEGACY   (Non-Commercial / Commercial / Non-Reportable)
variante             futures only
data del report      2026-09-08   (martedi)
data di lettura      2026-09-18
ritardo              10 giorni
```

**Il ritardo e' di dieci giorni, non dei tre strutturali.** Il report del 15 settembre esce **oggi**,
venerdi 18: questa rilevazione va rifatta stasera e confrontata.

**Il COT copre esattamente il contratto che si tradera'** — GC, non un derivato micro. Non serve
l'avvertenza che serviva sul crude micro.

## Convenzioni Dichiarate

Le stesse della rilevazione Nasdaq, per poterle confrontare:

- `net = long − short`, per ciascun gruppo.
- `z52` = z-score del net sulle **52 settimane precedenti**, deviazione di popolazione.
- `pct3a` = percentile del net dentro le ultime **156 settimane**.
- `pct10a` = percentile dentro tutte le **520 settimane** disponibili.
- Il percentile sulle variazioni confronta `|Δnet|` con le **519** variazioni del decennio.

## La Rilevazione

| Gruppo | Long | Δlong | Short | Δshort | Net | Δnet |
|---|---|---|---|---|---|---|
| **Non-Commercial** | 261.007 | +522 | **29.047** | **−3.314** | +231.960 | +3.836 |
| **Commercial** | 54.403 | −7.047 | 324.677 | −1.491 | −270.274 | −5.556 |
| **Non-Reportable** | 52.554 | +524 | 14.240 | −1.196 | +38.314 | +1.720 |

Prezzo della settimana di rilevazione: apertura 4.391,90, massimo 4.406,10, minimo 4.384,40,
chiusura 4.393,90.

### Dove stanno questi numeri nella loro distribuzione

```text
                          valore      z52     pct3a   pct10a
  Non-Commercial netto   231.960     1,02      64%      70%
  Non-Commercial long    261.007     0,40      38%      44%
  Non-Commercial SHORT    29.047    -1,76       1%       1%
  Commercial netto      -270.274    -1,09      31%      27%
  Non-Reportable netto    38.314     0,50      89%      91%
```

---

## Tre Osservazioni

### 1. L'estremo non e' il netto: e' lo short, e sta al 1° percentile del decennio

**Guardare il netto di questa rilevazione porta fuori strada.** +231.960 e' un valore alto ma
ordinario: 70° percentile su dieci anni, z52 poco sopra 1. Il long, 261.007, e' **sotto la
mediana** del decennio (44° percentile). Da soli, nessuno dei due dice niente.

**Lo short dei Non-Commercial e' 29.047, e questo e' il numero della rilevazione.** z52 **−1,76**,
**1° percentile** sia su tre anni sia su dieci. In 520 settimane solo **tre** hanno avuto meno
short speculativi sull'oro:

```text
  2020-04-28    24.653
  2020-05-05    25.567
  2020-03-31    28.680
  2026-09-08    29.047   <- oggi
  2026-08-04    29.379
  2020-03-24    29.562
  2026-06-02    30.076
  2026-06-16    30.907
```

**Le tre settimane sotto il valore attuale sono tutte marzo-maggio 2020**, cioe' il panico da
Covid. Le altre quattro nella lista sono **tutte del 2026**. Non e' un episodio: e' uno stato in
cui questo mercato e' entrato quest'anno e non e' piu' uscito.

**Cosa significa, in termini di posizionamento e non di previsione:** **non c'e' quasi piu' nessuno
short speculativo da far chiudere.** Il carburante di un rialzo violento — la copertura forzata di
chi e' corto — e' **esaurito**. Questo non dice che l'oro scendera'; dice che se sale, deve salire
perche' qualcuno compra, non perche' qualcuno e' costretto a ricomprare.

### 2. Il flusso e' ordinario, e il netto si muove per il lato sbagliato

**La variazione della settimana e' piccola:** netto +3.836, **piu' grande solo del 20%** delle 519
variazioni settimanali del decennio. Quattro settimane su cinque, storicamente, si muovono di piu'.

E' la distinzione flusso/livello della regola 2 del
[percorso del progetto](../percorso-del-progetto.md), qui **nella forma opposta** a quella vista sul
Nasdaq l'8 settembre: li' il flusso era notevole (89° percentile) e il livello ordinario; qui il
flusso e' ordinario (20° percentile) e il livello e' estremo.

**Come si e' mosso, che il netto da solo non direbbe:** +3.836 di netto e' fatto da **+522 long e
−3.314 short**. L'**86% del miglioramento viene da short chiusi**, non da long aperti.

Allargando alle otto settimane, il quadro cambia di nuovo:

```text
  data        NC long   NC short   NC netto   Comm netto   NonRept    close
  2026-07-21   224.785    40.875    183.910    -213.199    29.289   4.071,1
  2026-07-28   219.622    37.552    182.070    -212.309    30.239   4.036,3
  2026-08-04   227.013    29.379    197.634    -226.491    28.857   4.095,4
  2026-08-11   250.936    32.996    217.940    -252.640    34.700   4.383,0
  2026-08-18   256.902    34.713    222.189    -258.418    36.229   4.366,0
  2026-08-25   277.159    33.825    243.334    -279.585    36.251   4.638,1
  2026-09-01   260.485    32.361    228.124    -264.718    36.594   4.348,0
  2026-09-08   261.007    29.047    231.960    -270.274    38.314   4.393,9
```

Dal 21 luglio all'8 settembre il netto e' salito di **+48.050**: **+36.222 di long nuovi** e
**−11.828 di short chiusi**. Su sette settimane il motore e' stato l'**acquisto** (75%); nell'ultima
settimana e' stato la **copertura** (86%). Il motore ha cambiato lato mentre il prezzo si fermava
(4.638 il 25 agosto, 4.394 l'8 settembre).

### 3. I piccoli sono al 91° percentile, e ci arrivano tagliando gli short

I Non-Reportable stanno a **+38.314**, **89° percentile su tre anni, 91° su dieci**. Ci arrivano
con +524 long e **−1.196 short**: contrazione di posizione con gli short tagliati piu' del doppio
dei long aggiunti.

**E' lo stesso comportamento dei Non-Commercial, in scala piccola**, ed e' l'unico altro numero
della rilevazione fuori dalla propria distribuzione recente. Va letto col limite gia' registrato
per il Nasdaq: la [verifica della metrica cross-index](tradingster-cross-index-2026-09-11.md) non
ha trovato separazione nella risposta di prezzo, quindi **nessuna soglia di posizionamento e' stata
validata su questo repository**. Resta contesto.

---

## Il Buco Fra Il Report E Oggi, Quantificato In Prezzo

Come vuole la procedura: dieci giorni di ritardo si giudicano da quanto si e' mosso il prezzo, non
da quanti giorni sono.

```text
   8 settembre   4.381,00 - 4.488,80    giorno del report, chiusura 4.393,90
  16 settembre   4.273,30               minimo del periodo, -120,60 dalla chiusura del report
  18 settembre   4.427                  prezzo alla lettura, +33 dalla chiusura del report
```

**Il prezzo e' tornato dentro il range del giorno del report, e la fotografia regge.** E' il caso
favorevole: il posizionamento misurato allora e' plausibilmente ancora quello. Nel mezzo pero' c'e'
stata una discesa di 120 punti e un recupero di 154, e **chi si e' mosso in quel viaggio non e' in
questa tabella**.

## Quello Che Il COT E La Footprint Dicono Insieme — E Il Limite

Sono due misure su orologi diversi e **non vanno fuse in un punteggio** (regola di
`analisi-istituzionale.md`). Qui pero' dicono la stessa cosa per due strade indipendenti, e vale la
pena registrarlo come osservazione:

- **COT, settimanale, dieci giorni fa:** gli short speculativi sono al 1° percentile del decennio.
  Non c'e' piu' un lato corto da spremere.
- **Footprint, stamattina, misurata sul bridge** ([`../giornate/GCZ6-2026-09-18.md`](../giornate/GCZ6-2026-09-18.md)):
  dalle 04Z alle 06Z il prezzo e' salito **27 punti con delta netto −194**. Il rialzo non e' stato
  comprato, e' stato **smesso di vendere**.

**Il limite, ed e' serio:** una sola occorrenza non dimostra un legame. Questa e' la prima
rilevazione COT mai fatta su questo strumento in questo repository, non esiste una base storica per
dire se la coincidenza sia informativa, e **nessuna inferenza causale e' possibile**. La si
registra perche' sia verificabile in futuro, non perche' concluda qualcosa.

---

## Verifica Incrociata Con La Vista Disaggregated (18 settembre, 09:00Z)

L'utente ha portato una analisi esterna costruita sulla vista **Disaggregated** invece che sulla
Legacy usata qui. **Le due viste si possono controllare l'una con l'altra**, perche' nella vista
disaggregata il Non-Commercial della Legacy si spacca in **Managed Money + Other Reportables**.

Numeri dell'analisi esterna, rilevazione 2026-09-08:

```text
  Managed Money      long 145.804   short 10.832   netto +134.972   variazione netto  ~-1.800
                     (long -3.917, short -2.118)
  Other Reportables                                variazione netto  ~+5.600
```

**Il controllo:**

```text
  disaggregated:  Managed Money -1.800  +  Other Reportables +5.600  =  +3.800
  legacy (qui):   Non-Commercial netto  231.960 - 228.124            =  +3.836
  scarto: 36 contratti su 3.836, dentro l'arrotondamento dei "circa"
```

**Le due viste tornano.** E' la prima volta che in questo repository una rilevazione COT viene
verificata contro una tassonomia diversa, e il risultato dice che **i numeri dell'analisi esterna
sono buoni** — nonostante la fonte citata sia quella sbagliata (vedi sotto).

### Cosa aggiunge la vista disaggregata

**Managed Money short = 10.832.** E' il dettaglio che la Legacy nasconde: dei 29.047 short
Non-Commercial, solo **10.832 sono fondi**. Il resto sono Other Reportables. La misura del 1°
percentile fatta sulla Legacy regge, e la disaggregata la rende piu' netta: **i fondi sono
praticamente assenti dal lato corto.**

### I due errori dell'analisi esterna

1. **La fonte citata non contiene quei numeri.** Il link e' `cftc.gov/dea/futures/other_lf.htm`,
   che e' il report **"Other (Combined)"** — mercati non finanziari, formato lungo, **futures piu'
   opzioni**. L'analisi dichiara **"Disaggregated Futures Only"**: *Combined* e *Futures Only* sono
   due varianti con numeri diversi, e la pagina citata non e' quella dei metalli. **I numeri sono
   giusti, la citazione no**, e in questo repository la citazione fa parte del dato.

2. **Legge "bullish" dal netto, che e' il numero ordinario.** Dire *"fortemente net long, quindi
   strutturalmente bullish"* usa proprio la grandezza che sta al **70° percentile** su dieci anni.
   Il numero fuori scala e' lo **short al 1°**. E' la stessa distinzione flusso/livello della
   regola 2, applicata dentro la stessa rilevazione: **non tutti i campi di un COT sono ugualmente
   informativi, e il netto e' quasi sempre il meno informativo dei tre.**

### Dove invece ha ragione, e conta piu' del resto

> *"Oggi non userei il COT per anticipare la direzione: GC su ATAS decide il trade."*

**E' esattamente la posizione di questo repository**, ed e' in contraddizione con l'etichetta
*"Bias COT GC: bullish"* che la stessa analisi scrive due righe sopra. La conclusione operativa e'
giusta; l'etichetta e' il residuo di un modo di usare il COT che qui non e' mai stato validato —
la [verifica cross-index](tradingster-cross-index-2026-09-11.md) **non ha trovato separazione**
nella risposta di prezzo, quindi **nessun bias direzionale discende da un COT** in questo
repository.

Corretti anche: la scelta di GC invece di MGC (il COT copre il sottostante, e GC e' il contratto
istituzionale), e l'orario di pubblicazione (venerdi 15:30 ET, 21:30 italiane, dati del martedi).

## Limiti Della Fonte

- La rilevazione e' di **martedi** e viene pubblicata il **venerdi** successivo: tre giorni di
  ritardo strutturale, dieci in questo caso perche' il report del 15 non era ancora uscito.
- La vista **Legacy** (`Non-Commercial`) e la vista **TFF** (`Asset Manager`, `Leveraged Funds`)
  sono pagine diverse dello stesso strumento e **non sono sinonimi**. Su NQ si e' usata la TFF; qui
  la Legacy. **I due vocabolari non si confrontano** senza dirlo.
- Le OHLC settimanali di Tradingster sono sull'oro di riferimento e **non coincidono** con GCZ6 su
  ATAS: servono per ordine di grandezza. La chiusura del 2026-09-08 e' 4.393,90 contro un range
  misurato sul bridge di 4.381,00-4.488,80 quel giorno — coerente, non identico.
- **La vista TFF sull'oro non e' stata estratta.** E' il primo compito aperto.

## Cosa Resta Aperto

1. **Il report del 15 settembre esce oggi**: va letto stasera e confrontato con questo.
2. **La vista TFF / Disaggregated per intero** (`tradingster.com/cot/futures/fin/088691`): i
   numeri di Managed Money e Other Reportables dell'8 settembre sono stati verificati contro la
   Legacy (scarto 36 contratti su 3.836) ma **non estratti in serie**. Senza la serie non si puo'
   calcolare z52 e percentile su quella tassonomia.
3. **Il cross-index metalli** (oro / argento / rame), sul modello di
   [`tradingster-cross-index-2026-09-11.md`](tradingster-cross-index-2026-09-11.md) — che pero' su
   NQ **non ha trovato separazione**, quindi va rifatto sapendo che l'esito atteso e' negativo.
4. **Se lo short al 1° percentile abbia un seguito misurabile**: la serie ha 520 settimane, si puo'
   cercare cosa e' successo al prezzo nelle settimane successive alle altre occorrenze sotto il 5°
   percentile. **Non e' stato fatto**, ed e' il modo di trasformare questa osservazione in una
   misura invece che in una impressione.

---

## Il Cross Confirm — 18 Settembre 2026

Passo richiesto dal transcript, `fabio_live_charting_1_q1.txt` righe **1187-1197**: *"the reason
people lose money by using COT because they don't understand it is that ... you can cross confirm"*.
Per gli indici l'incrocio e' S&P / NASDAQ / Dow / Russell. Per l'oro sono **argento** (stesso
metallo, stesso flusso di rifugio) e **dollaro** (il lato opposto dello stesso scambio).

Tutti e tre: Tradingster, **legacy futures only, vista Non-Commercial**, report **8 settembre 2026**,
serie di **520 settimane** dal 2016-09-20. Percentile sulla serie intera, `z52` sulle ultime 52.

| | codice | netto | pct | z52 | long | pct | short | pct | z52 short |
|---|---|---|---|---|---|---|---|---|---|
| **oro** | 088691 | +231.960 | 70 | — | 261.007 | 44 | **29.047** | **1,0** | **−1,76** |
| **argento** | 084691 | +26.049 | 35 | −0,39 | 36.245 | **3,7** | 10.196 | **2,3** | −0,88 |
| **dollaro** | 098662 | +17.604 | 59 | +1,50 | 28.407 | 49 | 10.803 | 21,2 | **−1,57** |

### Cosa Dice L'Incrocio, E Non E' Quello Che Diceva L'Oro Da Solo

**1. L'argento non conferma: non e' compresso, e' piccolo.** Lo short e' al 2,3° percentile, quasi
come l'oro — ma il **long e' al 3,7°**. Le due gambe stanno entrambe sul fondo del decennio, cioe'
la partecipazione speculativa complessiva e' minima. Un estremo simmetrico non e' uno sbilanciamento:
e' un mercato vuoto. L'oro invece ha il **long al 44° percentile** — ordinario — contro lo short al
1°, ed e' quella **asimmetria** a essere l'informazione.

**2. Il dollaro toglie il resto.** Anche li' gli short Non-Commercial sono compressi (`z52 −1,57`),
e il netto e' lungo e in crescita (+579, `z52 +1,50`). Ma **oro e dollaro si muovono in verso
opposto**: "corti sull'oro" e "corti sul dollaro" sono due scambi contrari, e non possono essere
entrambi *il lato esaurito* della stessa cosa.

### La Conseguenza, Che E' Una Correzione

**Nessuno e' corto di niente.** Su tre strumenti — due dei quali si muovono all'inverso — lo short
speculativo e' compresso nello stesso momento. L'estremo dell'oro quindi **non e' un fatto sull'oro**:
e' l'assenza di esposizione corta nel complesso, e un livello estremo dappertutto insieme non
distingue nessuno dei tre.

Nel quadro del 18 settembre avevo scritto *"chi ha venduto l'oro nelle ultime tre settimane e'
sotto"* come se fosse una misura del posizionamento sull'oro. **Il cross confirm la declassa**: la
parte specifica dell'oro resta l'asimmetria long-ordinario / short-estremo, che argento e dollaro
non hanno. Il resto e' contesto di mercato, non dell'oro.

**Cosa resta aperto.** Il conto non e' stato fatto sulla serie: quante volte in 520 settimane i tre
short sono stati compressi insieme, e cosa e' successo dopo. Finche' non lo si conta, anche questa
resta una osservazione singola.
