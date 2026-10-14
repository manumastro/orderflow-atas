---
chiave: golez-2012-pinning-in-the-sp500-futures
sigla: GJ
titolo: "Pinning in the S&P 500 futures"
autori: [Benjamin Golez, Jens Carsten Jackwerth]
versione_letta: "versione pubblicata (impaginato JFE), copia depositata su KOPS, Universita' di Costanza"
fonte: Journal of Financial Economics 106(3), 566-585
identificativo: "10.1016/j.jfineco.2012.06.010"
url: https://d-nb.info/1112655492/34
tipo: rivista
revisione_fra_pari: si
pagine: 20
pdf: golez-2012-pinning-in-the-sp500-futures.pdf
pdf_sha256: 07fabf4aa169abe42c79f63c1780e691ca885be59a5326361d4583fff0fe0cdb
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; i dati (CME, Market Data Express) sono a pagamento"
---

# Golez E Jackwerth 2012: L'Analisi

*Pinning in the S&P 500 futures*, Benjamin Golez (Notre Dame) e Jens Carsten Jackwerth (Costanza),
Journal of Financial Economics 106(3), 2012, pp. 566-585. Rivista con revisione fra pari. Letto per
intero il 14 ottobre 2026. Citazioni `[GJ §n]`, `[GJ Tab. n]`, `[GJ Fig. n]`.

**Come e' stato letto.** Il PDF di KOPS ha **6 pagine su 20 senza testo** (sono scansioni: pp. 568,
569, 573, 575, 577, 584 della rivista). `pdftotext` le salta, e con loro le Fig. 1-6, il §2.1-2.2 e
la meta' del §5.2-5.3. Le abbiamo lette **renderizzando le pagine come immagini** (PyMuPDF, nella
scratchpad). Le tabelle 3 e 4 escono storpiate da `pdftotext`: i numeri qui sotto vengono
dall'immagine della pagina. I segni meno spariscono nel testo estratto (per esempio le posizioni
dei market maker, -1.840 e -12.315): ricontrollati sull'immagine.

## La Domanda

Il future sull'S&P 500 viene **attratto verso lo strike** delle opzioni nel giorno in cui scadono?
E se si', per quale meccanismo?

**Pinning** (da *pin*, spillo): la tendenza del prezzo del sottostante a chiudere vicino a uno
strike il giorno della scadenza delle opzioni. **Anti pinning**: il contrario, il prezzo viene
respinto dallo strike. **Cross pinning**: le opzioni su un mercato (l'indice SPX) attraggono o
respingono il prezzo di un altro mercato (il future), perche' i dealer si coprono su quello.

## Il Metodo

**Il meccanismo, prima** [GJ §2]. Il **delta hedging** e' la copertura con cui il market maker
(o *dealer*, chi tiene il libro delle opzioni) neutralizza la sensibilita' del suo portafoglio al
prezzo, comprando o vendendo il future in quantita' pari al delta delle opzioni. Gli autori
mettono in gara quattro spiegazioni:

1. **Avellaneda e Lipkin (AL)**: il decadimento nel tempo del delta (vicino alla scadenza il delta
   di un'opzione fuori dai soldi va a 0, quello di una dentro va a 1) costringe il dealer a
   ribilanciare. Se il dealer e' **lungo** opzioni produce pinning; se e' **corto**, come sugli
   indici, produce **anti pinning** [GJ §2.1, Fig. 2].
2. **Wall Street Journal 1982 (AN)**: le opzioni seriali sul future SP si consegnano fisicamente.
   L'investitore con un'opzione appena dentro i soldi non vuole ritrovarsi un future scoperto nel
   fine settimana e la **rivende** al dealer, che smonta la copertura spingendo il prezzo verso lo
   strike: pinning [GJ §2.2, Fig. 3].
3. **Esercizio anticipato (EARLY)**: stesso effetto, con l'esercizio delle opzioni americane dentro
   i soldi e la vendita del future ricevuto [GJ §2.3].
4. **Manipolazione (NI)**: chi e' corto opzioni spinge il prezzo verso lo strike. La misurano con il
   volume del future; si aspettano che non spieghi niente [GJ §2.4].

**Le posizioni dei dealer** non si osservano sulle opzioni SP. Le stimano sulle SPX con i dati
OpenClose (aperture e chiusure per tipo di investitore): il giorno prima della scadenza, novembre
1992 - novembre 2009, i market maker sono in media **corti di 1.840 call e di 12.315 put** alla
pari [GJ §2]. Assumono che sulle SP sia lo stesso, perche' SPX e SP hanno OI e volumi vicini alla
pari correlati 0,86 e 0,79 negli ultimi 5 giorni prima della scadenza [GJ §2, nota 3].

**Il test** [GJ §3, eq. 1-5]. Variabile binaria `Pinn_sym = 1` se il **prezzo di settlement** del
future del primo contratto sta entro **+/- 0,375 $** dallo strike piu' vicino (strike ogni 5 $).
Regressione logistica sui **10 giorni prima e dopo** ogni scadenza, con una variabile 1 nel giorno
di scadenza. Il nullo dichiarato e' anche la **distribuzione uniforme**: con strike ogni 5 $ ci si
aspetta il 15% (0,75 / 5). Il pinning si divide "dall'alto" e "dal basso" confrontando il
settlement con un **valore equo** ricavato dal secondo contratto (rapporto mediano fra i due, minuto
per minuto, 9:00-14:30) [GJ eq. 3-4]. Poi, solo sui giorni di scadenza, si spiega il pinning con OI
alla pari del giovedi', volume ed esercizio anticipato del venerdi', volume del future e volatilita'
del giovedi' (range di Parkinson, eq. 6).

**La misura economica** [GJ §5.1, eq. 7]: il limite inferiore di Ni, Pearson e Poteshman sullo
spostamento medio assoluto del rendimento, `|E(a^) - E(a)| ~ | somma_b [p^(b) - p(b)] a(b) |`, con
`p^` e `p` le probabilita' del rendimento assoluto nell'intervallo b nei venerdi' di scadenza e
negli altri venerdi', griglia 0,1 punti base.

**I dati** [GJ §4, App. B]: dalla CME, settlement giornalieri del future e delle opzioni SP
(1982/1983 - 2009) e **tutti gli scambi intraday del future**; da Market Data Express, opzioni SPX
giornaliere e OpenClose (1990-2009) e i valori di settlement AM (SOQ). Esclusi ottobre 1987 e
ottobre 2008.

**I campioni** [GJ §5.1, App. C]: **lungo**, 136 scadenze seriali, novembre 1992 - novembre 2009;
**corto**, 89 scadenze, ottobre 1998 - novembre 2009 (l'unico con l'esercizio anticipato non nullo).
Le scadenze seriali (gennaio, febbraio, aprile, maggio, luglio, agosto, ottobre, novembre) sono il
laboratorio: l'opzione scade al settlement PM del venerdi', il future continua a vivere. Prima del
novembre 1992 le SPX scadevano anch'esse PM e potevano disturbare: quegli anni sono esclusi.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | nei venerdi' di scadenza seriale il future chiude entro +/- 0,375 $ dallo strike il **21,32%** delle volte (29 su 136, calcolo nostro), contro il 15% atteso; **22,47%** nel campione corto. S&P 500 future, primo contratto, settlement giornaliero, 1992-2009 | §5.1, Tab. 1 |
| 2 | logistica scadenza contro +/- 10 giorni: coefficiente **0,54 (p = 0,01)**, 2.813 giorni; campione corto 0,59 (p = 0,03), 1.839 giorni. Probabilita' implicite (calcolo nostro): 13,6% negli altri giorni, 21,2% alla scadenza | Tab. 3, mod. 1-2 |
| 3 | **asimmetria**: dal basso 14,71%, dall'alto 6,62%. Dal basso coefficiente 0,83 (p = 0,00), dall'alto -0,01 (p = 0,99). Coerente con i dealer molto piu' corti di put che di call | §5.2, Tab. 1, Tab. 3 mod. 3-4 |
| 4 | **spostamento**: almeno **11,00 punti base** dal giovedi' al venerdi' PM, cioe' almeno **115 milioni $** di valore nozionale a scadenza (OI medio 410.890 contratti a 1.013,46); **59 milioni** nell'ultima mezz'ora. Campione corto: **240 milioni**, 105 nell'ultima mezz'ora | §5.1, eq. 7 |
| 5 | **intraday**: a intervalli di 5 minuti (primo scambio), il pinning compare **solo nell'ultima mezz'ora**, da circa le 14:45 (ora di Chicago, chiusura 15:15). Prima, la curva dei giorni di scadenza sta su quella degli altri giorni, intorno al 5-8%; alla chiusura sale verso il 21% contro circa il 13% | §5.2, Fig. 6 |
| 6 | **meccanismi**: OI alla pari **riduce** il pinning (-0,35, p = 0,09; corto -0,86, p = 0,02), il volume alla pari lo **aumenta** (0,69, p = 0,00; corto 1,13, p = 0,00). Cioe' AL (anti pinning) c'e', ma la rivendita lo sovrasta | Tab. 3 mod. 5-7 |
| 7 | **direzione**: il volume delle call spiega il pinning dall'alto (0,99, p = 0,03), quello delle put il pinning dal basso (1,01, p = 0,00) | Tab. 3 mod. 8-9 |
| 8 | esercizio anticipato: segno giusto, **non significativo** (p = 0,92; call 0,66, put 0,91). Volume del future (manipolazione): 0,00, p = 0,93. Volatilita' del giovedi': -13,03, p = 0,03 nel corto, p = 0,16 nel lungo | Tab. 3 mod. 7, 10-13 |
| 9 | **controlli negativi**: nessun pinning nel secondo contratto (-0,22, p = 0,42); nessun pinning nell'indice al SOQ delle SPX (15,44% contro 15%, p = 0,49); SPY "nessun pinning ai primi controlli", non tabulato | §5.4-5.5, Tab. 3 mod. 14, nota 13 |
| 10 | **anti cross pinning**: sui 245 giorni di scadenza delle opzioni regolate in contanti (SPX e SP trimestrali) il future chiude vicino allo strike corretto per il costo di finanziamento il **9,39%** delle volte contro il 14,71% degli altri giorni (-0,51, p = 0,02, 4.440 giorni) | §6.2, Tab. 4 mod. 15 |
| 11 | sul sottoinsieme SPX AM (122 giorni, misurato alla chiusura del giovedi'): anti pinning p = 0,06; l'OI delle put alla pari lo spiega (-0,11, p = 0,05), il volume e le chiusure dei clienti no | §6.3, Tab. 4 mod. 16-21 |
| 12 | **robustezza**: intervallo +/- 0,25 $: p = 0,02 e 0,00; +/- 0,50 $: p = 0,04 e 0,06. Cross pinning: 0,07 e 0,04. Modello lineare di probabilita', logaritmi, trattamento dei mancanti: risultati simili | §7 |

## Cosa Abbiamo Riprodotto

Niente sui dati: non abbiamo ne' la storia del future fino al 2009 ne' i dati delle opzioni. Solo
**controlli aritmetici** (nostri):

- 115 milioni $ tornano con **11 punti base x 410.890 contratti x 1.013,46 x 250 $**: il paper non
  dichiara il moltiplicatore, e il conto implica 250 $ per punto;
- 240 milioni $ nel campione corto equivalgono a circa **15,5 punti base**; i 59 e 105 milioni
  dell'ultima mezz'ora a circa 5,7 e 6,8 punti base;
- le probabilita' implicite dai coefficienti dei modelli 1 e 15 tornano con le frequenze del testo
  (21,2% contro 21,32%; 9,4% contro 9,39%);
- 29 scadenze su 136 dentro l'intervallo, contro un'attesa uniforme del 15%: binomiale a una coda
  p = 0,03.

## Cosa Non Regge, O Non Si Dice

1. **Le posizioni dei dealer sono stimate, e su un altro mercato.** Sulle opzioni SP non ci sono
   dati OpenClose: si assume che i dealer siano corti come sulle SPX. Tutta la spiegazione
   (asimmetria dal basso, AL che produce anti pinning) poggia su questa assunzione.
2. **L'effetto e' piccolo in numero di casi.** 21,3% contro 15% su 136 scadenze sono circa 8-9
   scadenze in piu' in 17 anni. Nella Fig. 5 (letta sul grafico, valori approssimati) il giorno 0
   e' l'unica barra sopra il 90-esimo percentile binomiale, ma di poco (circa 21 contro 19); i
   giorni +6 e +9 ci arrivano vicino.
3. **Il nullo dei giorni normali non e' il 15%.** La costante del modello 1 implica il **13,6%**
   negli altri giorni. Il paper dice che il confronto con l'uniforme da' risultati simili e non lo
   riporta. Una spiegazione possibile, **deduzione nostra da verificare**: la griglia dei tick. Se il
   settlement si muove a passi di 0,10, l'intervallo +/- 0,375 contiene 7 prezzi su 50, cioe' il 14%,
   non il 15%. Il nullo giusto si calcola sulla griglia dei prezzi possibili.
4. **Il campione e' scelto per evitare interferenze, e lo dicono**: gli anni 1987-1992 sono esclusi
   perche' "piccoli cambiamenti" nel trattare le forze di pinning e anti pinning "portano a
   risultati piuttosto diversi" [GJ §5.1]. Per il cross pinning si escludono anche 1998-2002 per dati
   mancanti; rimettendoli, i coefficienti diventano non significativi [GJ nota 17]. E' sensibilita'
   alla specificazione, dichiarata.
5. **Prove multiple senza correzione.** Tre intervalli, due campioni, simmetrico/alto/basso, 25
   modelli. Il risultato principale regge alle varianti dichiarate (p fra 0,00 e 0,06), ma molte
   spiegazioni stanno fra 0,05 e 0,12.
6. **Nella Fig. 6 lo strike e' scelto con il prezzo finale.** Lo strike "alla pari" e' quello piu'
   vicino al settlement, anche per i prezzi delle 9:00. Per questo la curva sale verso la chiusura
   anche nei giorni senza scadenza (da circa 5% a circa 13%): e' la costruzione, non un effetto.
   Conta solo la distanza fra le due curve. Per noi e' un **look-ahead** da non copiare: in una
   prova lo strike si sceglie con quello che si sa prima.
7. **La direzione usa dati dello stesso giorno.** Il valore equo che decide "dall'alto" o "dal
   basso" viene dal rapporto fra i due contratti fino alle 14:30 del venerdi'. Va bene per
   descrivere, non per decidere in anticipo. Lo stesso per volume ed esercizio anticipato, misurati
   il giorno di scadenza.
8. **Nessuna strategia, nessun costo.** Il paper misura una distorsione del prezzo, non un guadagno.
   L'unica cifra economica e' un **limite inferiore** di 11 punti base sul nozionale.
9. **Il moltiplicatore non e' dichiarato.** Il conto dei 115 milioni torna con 250 $ per punto; il
   contratto grande, se ricordiamo bene, valeva 500 $ per punto fino al 1997 (da verificare). Se
   e' cosi', per gli anni 1992-1997 il nozionale e' sottostimato, non gonfiato.
10. **Il mercato di oggi e' un altro.** I dati finiscono nel 2009. Le scadenze settimanali e
   giornaliere (0DTE) sono venute dopo: il contrasto "giorno di scadenza contro altri giorni" su cui
   si regge il test oggi non esiste piu' nella stessa forma.
11. **Il meccanismo dipende dal regolamento.** Il pinning c'e' dove le opzioni si consegnano in
    future e scadono PM (seriali SP); dove si regola in contanti c'e' anti pinning. Rifarlo su ES
    richiede di sapere, scadenza per scadenza, come si regolano le opzioni di oggi.

## Cosa Si Puo' Rifare Su ES, Con ATAS

**Cosa abbiamo.** Barre a 1 minuto di ES dal Data Bridge, con volume e delta; la profondita' della
storia la decide il tariffario ed e' **da misurare** (per NQ oggi: mesi, non anni;
[`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md)). Gli indicatori delle opzioni di ATAS
([`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md)): **Options Chain** dalla nostra
connessione Rithmic (OI Profile, Key Levels con Max Pain e muri, Expected Move, GEX Profile con
Zero Gamma), **senza storia**: registra da quando e' sul chart, l'OI e' giornaliero. **Options
X-Ray** (dati SPX di OptionsDepth, ogni minuto: Key Levels con Flip e Pin, GEX heatmap, charm
heatmap, hedge pressure), storia **sconosciuta**.

**Il conto della potenza** (nostro): per vedere 21% contro 15% con un test a una coda al 5% e
potenza 80% servono circa **220 scadenze**. Con una scadenza al mese sono 18 anni: **il test di GJ
cosi' com'e' non si rifa' con la storia di ATAS**.

**Cosa si puo' fare solo con i prezzi** (le barre di ES):

- la **frequenza del prezzo entro +/- 0,375 punti dallo strike multiplo di 5** al minuto delle
  15:00 di Chicago, per tipo di giorno. Con tick 0,25 e strike ogni 5 punti il nullo uniforme e'
  **3 prezzi su 20, il 15%** (calcolo nostro; la spaziatura degli strike vicini va verificata sulla
  catena di ATAS);
- il **profilo intraday** della Fig. 6: la frequenza minuto per minuto nell'ultima mezz'ora,
  confrontata con le ore precedenti della stessa giornata. Non richiede dati sulle opzioni, ma
  richiede il calendario delle scadenze e del loro regolamento (PM o AM, consegna o contanti),
  **da verificare sulle specifiche CME attuali**;
- la significativita' **per giornata**: un'osservazione per giornata, mai per minuto.

**Cosa richiede una registrazione in avanti** (da quando si accende):

- **OI per strike del giorno prima** (Chain, OI Profile): serve a separare i meccanismi di GJ
  (OI riduce, volume aumenta) e a scegliere lo strike giusto, quello con l'OI alla pari, invece del
  multiplo di 5 piu' vicino;
- **gli strike delle SPX** e il **Pin** di X-Ray: per il cross pinning, lo strike SPX va portato su
  ES con il rapporto ES/SPX del giorno, come fa GJ con il costo di finanziamento;
- dopo un anno di registrazione si hanno circa 250 giornate e circa 12 scadenze mensili: abbastanza
  per le prove giornaliere, non per quelle mensili.

**Ipotesi da scrivere in una scheda, se si apre lo studio** (non sono regole):

- **H1**: nei giorni di scadenza PM con consegna in future, ES alla chiusura delle 15:00 sta entro
  +/- 0,375 dallo strike con piu' OI piu' spesso che negli altri giorni e piu' del 15% del nullo.
  Serve l'OI registrato in avanti; con i soli prezzi si prova la versione debole (strike multiplo
  di 5).
- **H2**: l'attrazione, se c'e', si concentra negli ultimi 30 minuti prima della scadenza (Fig. 6):
  la distanza dallo strike si riduce nell'ultima mezz'ora piu' che nelle mezz'ore precedenti.
  Solo barre a 1 minuto.
- **H3**: con OI delle put alla pari molto superiore a quello delle call, il pinning arriva **dal
  basso** (Tab. 3 mod. 9). Serve l'OI per strike, da registrare.

## Cosa Ne Prende Il Framework

- **Il disegno del test del pinning**: variabile binaria entro un intervallo, contrasto con i
  giorni vicini **e** con il nullo uniforme, calcolato sulla griglia dei tick.
- **Il limite di Ni, Pearson e Poteshman** (eq. 7) per dare una misura economica a una distorsione
  della distribuzione, senza inventare una strategia.
- **I controlli negativi** come parte del test: il secondo contratto, l'indice. Un effetto di
  scadenza deve sparire dove il meccanismo non agisce.
- **La lezione di metodo**: GJ scarta un periodo perche' l'esito dipende da come si trattano forze
  opposte. Nel framework una scelta del genere si dichiara prima, nella scheda dell'idea, e conta
  nel registro come variante.
- **Un dato da confrontare con l'altro paper del filone**: GJ misura dealer **corti** di call alla
  pari sulle SPX (1992-2009); il GEX di SqueezeMetrics assume dealer **lunghi** di call. Vedi
  [`../squeezemetrics-2017-gamma-exposure/analisi.md`](../squeezemetrics-2017-gamma-exposure/analisi.md).
