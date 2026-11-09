---
chiave: kaeck-2017-informed-trading-in-the-index-option-market
sigla: KVS
titolo: "Informed Trading in the Index Option Market"
autori: [Andreas Kaeck, Vincent van Kervel, Norman J. Seeger]
versione_letta: "SSRN 2981332, versione rivista il 17 agosto 2019 secondo la pagina SSRN (scritta il 5 giugno 2017); il PDF non porta data; mai pubblicata in rivista per quanto dice la pagina SSRN"
fonte: "SSRN; TILEC Discussion Paper 2017-027 (dalla pagina SSRN, non dal PDF)"
identificativo: "10.2139/ssrn.2981332"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2981332
tipo: preprint
revisione_fra_pari: no
pagine: 51
pdf: kaeck-2017-informed-trading-in-the-index-option-market.pdf
pdf_sha256: e8593596f3c4a7286f847dce2b864e130c3516ac4ee9eaaf7614b5d5bee00113
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati Reuters Tick History (a pagamento), tassi OptionMetrics"
---

# Kaeck, van Kervel, Seeger 2017: L'Analisi

*Informed Trading in the Index Option Market*, Andreas Kaeck (University of Sussex Business School),
Vincent van Kervel (Pontificia Universidad Catolica de Chile secondo il PDF; la pagina SSRN, nella ricerca,
lo dava alla Universidad de los Andes), Norman J. Seeger (VU Amsterdam). **Preprint, non revisionato.**
Letto per intero il 9 novembre 2026: 51 pagine del PDF, cioe' frontespizio, abstract, testo (pagine 1-25),
appendice A sul modello di Heston, bibliografia, tabelle 1-7, figure 1-3 e la Web Appendix (figure A1-A5,
tabella 1 alla mezz'ora). **La versione**: SSRN la dice rivista il 17 agosto 2019; il PDF non porta data, ma
cita lavori del 2019 (Chordia e altri come working paper, Muravyev e Pearson, Bardgett e altri), coerente con
quella revisione. Le tabelle 1-6, le equazioni 2-4 e le figure 1 e 3 sono state guardate come immagine
(`pypdfium2`); il resto dal testo di `pdftotext -layout`. Citazioni `[KVS §n]`, `[KVS Tab. n]`, `[KVS eq. n]`.
Nessuna istruzione trovata nel PDF.

**In parole semplici.** Il paper somma tutti gli scambi di opzioni SPX di un'ora come fa il DEX di X-Ray:
contratti per delta per segno (comprato o venduto), in dollari. Trova che, **nella stessa ora**, quel delta
comprato va con un S&P 500 piu' alto (circa 5 punti base per una deviazione standard), e lo chiama
"informato". Ma **non lo anticipa**: il delta dell'ora prima non dice niente dell'ora dopo, e al minuto non
c'e' niente. Non ha gruppi (tutti gli scambi insieme), non ha 0DTE (solo scadenze da 7 a 180 giorni), un solo
anno, il 2014. Per il riquadro conferma il modo di sommare, non un segnale.

## I Termini

- **Order flow netto** (flusso netto): volume comprato da chi prende l'iniziativa (*buyer-originated*, chi
  colpisce l'offerta) meno quello venduto da chi colpisce la domanda. Il segno dello scambio si decide dalla
  posizione del prezzo rispetto a domanda e offerta (regola di Lee e Ready) [KVS §3.1, nota 9]. **Non e'**
  il segno di OptionsDepth, che dice se un gruppo ha comprato o venduto (codice d'origine del Cboe).
- **Delta order flow** (x con delta): la somma, su tutti gli scambi dell'ora, di contratti per delta per
  segno per prezzo del sottostante. E' il **delta in dollari** scambiato; si somma fra strike, scadenze, call
  e put [KVS eq. 2]. E' la stessa idea del DEX di X-Ray, con il prezzo che lo porta in dollari.
- **Vega order flow** (x con nu): lo stesso con il vega (quanto cambia il prezzo dell'opzione se la
  volatilita' sale di un punto) al posto del delta, moltiplicato per il livello della volatilita' [KVS eq. 3].
  Comprare uno straddle (call e put allo stesso strike) da' vega molto e delta quasi zero [KVS §1, nota 2].
- **VAR** (*vector autoregression*): un sistema di regressioni in cui ogni serie dipende dal passato di tutte.
  **VAR strutturale** con un **ordinamento**: si decide a priori chi muove chi **nella stessa ora**; e'
  equivalente a una scomposizione di Cholesky dei residui [KVS §2, nota 7]. L'ordinamento non si prova dai
  dati: e' un'ipotesi.
- **IRF** (*impulse response function*): di quanto si muove una serie, sommando le ore, dopo uno shock di una
  deviazione standard in un'altra. Qui "lungo periodo" vuol dire **quattro ore** [KVS Tab. 3].
- **Price impact** (impatto sul prezzo) **permanente**: nel quadro di Hasbrouck (1991), la parte del movimento
  del prezzo attribuita al flusso che non torna indietro. Gli autori chiamano "informato" il flusso che ha un
  impatto permanente [KVS §1, §7].
- **Effetto leva**: prezzo e volatilita' si muovono in versi opposti; qui la correlazione oraria e' -0,858
  [KVS nota 17].
- **Moneyness**: quanto un'opzione e' vicina al denaro (rapporto fra il forward e lo strike). **TTM**: tempo
  alla scadenza.
- **Newey-West**: errori standard che tengono conto della correlazione fra osservazioni vicine nel tempo (qui
  due ritardi, cioe' due ore).

## La Domanda

Chi scambia opzioni SPX lo fa con informazione privata sul **prezzo** dell'S&P 500, sulla sua **volatilita'**,
o su entrambi? E come si somma il flusso di centinaia di opzioni diverse in una misura sola, senza perdere la
distinzione fra prezzo e volatilita'? [KVS §1, abstract]. Il contributo dichiarato e' **il metodo**: due
variabili, il flusso in delta e il flusso in vega, dentro un VAR alla Hasbrouck con la volatilita' [KVS §1, §7].

## Il Metodo

**I dati** [KVS §3.1]: Reuters Tick History, scambi e quotazioni al millisecondo delle **opzioni SPX
europee** dal 2 gennaio al 31 dicembre 2014, **solo il ciclo del terzo venerdi'** (le mensili), e degli
scambi e quotazioni di **SPY**, l'ETF, usato per il prezzo del sottostante e per il suo flusso. Filtri: via
le opzioni con meno di 7 o piu' di 180 giorni alla scadenza; via gli scambi oltre 10 milioni di dollari
(probabilmente negoziati fuori borsa; meno dello 0,09% degli scambi, nota 10); **solo dalle 8:45 alle 14:45 di
Chicago** (9:45-15:45 di New York), sei ore piene, per evitare le aste d'apertura e di chiusura. **Nessun
gruppo di partecipanti**: dati pubblici, tutti gli scambi insieme [KVS §1: "publicly available trade and quote
data", in contrasto con i dati per tipo d'investitore di Pan e Poteshman].

**Il campione**: 1.804.397 scambi di opzioni [KVS Tab. 1]; 249 giornate per 6 ore = **1.494 osservazioni
orarie** [KVS Tab. 2, Tab. 4].

**Delta e vega** [KVS §3.2, App. A]: dal modello di Heston (volatilita' stocastica), calibrato ogni giorno
sulle quotazioni delle opzioni fuori dal denaro da 7 a 180 giorni e sul VIX insieme; i delta e i vega di un
giorno si calcolano con i **parametri del giorno prima** e la volatilita' ricavata dal VIX del giorno ("out-of-
sample", dicono: niente look-ahead nei greci). Il **VIX** lo ricostruiscono loro, al minuto, dalle opzioni
fuori dal denaro (eq. 5): una procedura che chiamano "standard", con le volatilita' implicite interpolate da un
polinomio cubico, estese piatte oltre gli strike scambiati e l'integrale calcolato con una quadratura di
Gauss-Kronrod (rimandano a Broadie e altri e a Carr e Wu, non al Cboe) [KVS §3.2].

**Le due somme** [KVS eq. 2-3, guardate sul PDF]:

```text
x_delta(t) = somma sugli scambi i fra t-1 e t di  Q_i * Delta_i * BuySell_i * P_u(t-1)
x_vega(t)  = somma sugli scambi i fra t-1 e t di  Q_i * nu_i    * BuySell_i * v(t-1)
```

con Q il numero di opzioni dello scambio, BuySell +1 se lo scambio e' iniziato da un compratore e -1 da un
venditore, P_u il prezzo del sottostante all'ora prima, v la volatilita' all'ora prima. Le unita' delle
tabelle: centinaia di milioni di dollari.

**Il VAR** [KVS eq. 4, §4.2]: cinque equazioni, nell'ordine variazione del prezzo (d_u), variazione della
volatilita' (d_v), flusso di SPY, flusso in delta, flusso in vega. **Il prezzo e' messo per primo**: nella
stessa ora e' mosso da tutti i flussi e non ne muove nessuno; il flusso in vega e' per ultimo: muove tutti e
non e' mosso da nessuno [KVS §2]. La ragione: nei modelli di scambio sequenziale il flusso causa il
rendimento, non il contrario (Hasbrouck 1991). **Sul flusso di SPY il testo e le tabelle non coincidono**
(verifica): il §2 dice che l'ordinamento da' a SPY il contenuto comune con le opzioni ("the most conservative
ordering"), ma nella Tab. 4 e' il flusso in delta che entra nell'equazione di SPY nella stessa ora (0,086),
non il contrario; cosi' nelle IRF la parte comune va al **flusso in delta** (circa 10 cents dei circa 106
dell'ora, conto sotto). "Conservativo" vale solo per il coefficiente dell'equazione del prezzo (2,735, al netto
di SPY). Attenzione alla notazione: la Tab. 4 e la Web Appendix scrivono l'ordinamento al rovescio, dal piu'
esogeno, {x vega, x delta, x SPY, d_v, d_u}. Due ritardi (criterio di Akaike);
ogni giornata riparte da zero (i ritardi della prima ora sono posti a zero) [KVS §4.2]. Stima OLS, errori
**Newey-West con due ritardi**, **per osservazione oraria** [KVS Tab. 4].

**Le prove della struttura** [KVS §4.3]: (a) un test di Wald se il flusso in delta e in vega ha lo stesso
impatto nei nove gruppi di opzioni (tre fasce di scadenza, 30 e 90 giorni; tre di moneyness, 0,95 e 1,0); (b)
quanto delta e vega insieme spiegano delle variazioni orarie del prezzo di ogni opzione (regressione alla
Bakshi, Cao e Chen, eq. 6, una osservazione per opzione-giorno-ora, 851.051 osservazioni).

**Le frequenze piu' alte** [KVS §5]: lo stesso VAR alla mezz'ora (quattro ritardi) e al minuto (dieci
ritardi, con i ritardi polinomiali di Hasbrouck 2018 per ridurre i parametri); la tabella al minuto usa solo i
primi due mesi del 2014 [KVS Tab. 7].

**La robustezza** [KVS §6, Web Appendix]: due ordinamenti diversi (prezzo e volatilita' scambiati, Fig. A1;
SPY come variabile piu' esogena, {x SPY, x vega, x delta, d_v, d_u}, Fig. A2, cioe' SPY che muove le opzioni
nella stessa ora), il forward dalle opzioni al posto di SPY (Fig. A3), rendimenti al posto
delle differenze (Fig. A4), il VAR a quattro equazioni senza SPY (Fig. A5). **Nessun campione fuori, nessuna
strategia, nessun costo, nessun modello nullo.**

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Gli scambi**: in media 93.300 dollari e 51,4 contratti (circa 20 volte uno scambio tipico di azioni o ETF); 41% call e 59% put; **66,2% iniziati da un compratore**; delta assoluto medio 0,248; vega medio 73,1; moneyness media 0,945; scadenza media 0,118 anni (circa 30 giorni di borsa) | §4.1, Tab. 1 |
| 2 | **I flussi orari** (centinaia di milioni di dollari): SPY media -0,43 (deviazione standard 4,32), cioe' vendite nette di 43 milioni l'ora; **flusso in delta delle opzioni media +1,79 (deviazione standard 8,64)**, cioe' **+179 milioni l'ora**, il doppio piu' variabile di SPY; flusso in vega 0,11 (0,16). Variazione oraria del prezzo: deviazione standard 392,91 "cents"; della volatilita' 0,58 punti | §4.1, Tab. 2 |
| 3 | **Il finding sul prezzo**: uno shock di una deviazione standard del flusso in delta (823 milioni di dollari, il residuo strutturale) muove il sottostante di **98,13 cents in quattro ore (t 4,41)**, cioe' da 1.930 a 1.930,98, **5,1 punti base**; il testo dice "il 50% della deviazione standard oraria" del prezzo, riferito al residuo (189,93) | §1, §4.2, Tab. 3 |
| 4 | **SPY pesa di piu'**: uno shock del flusso di SPY (420 milioni) muove il prezzo di 171,00 cents (t 8,06); per dollaro l'impatto di SPY e' **3,4 volte** quello delle opzioni (1,71/4,20 contro 0,98/8,23) | §4.2, Tab. 3, nota 15 |
| 5 | **Il finding sulla volatilita'**: uno shock di una deviazione standard del flusso in vega alza la volatilita' di **0,07 punti** (da 11,50% a 11,57%; t 2,36, solo al 5%), 0,14 deviazioni standard di d_v. Il flusso in delta la abbassa (-0,11, t 3,31), quello di SPY di piu' (-0,23, t 7,45): l'effetto leva | §1, §4.2, Tab. 3 |
| 6 | **R quadro**: d_v 0,25 ("un quarto" spiegato dai flussi dell'ora e dai ritardi); d_u 0,766, ma quasi tutto per la volatilita' nella stessa ora (coefficiente -525,69, t -32,2); flusso in delta 0,092, SPY 0,055, vega 0,051 | §4.2, Tab. 4 |
| 7 | **Il flusso dell'ora prima non predice il prezzo dell'ora dopo**: nell'equazione del prezzo il flusso in delta della stessa ora ha 2,735 (t 3,8), quello di un'ora prima **0,446 (t 0,6)**, di due ore prima **0,263 (t 0,4)**. Il flusso in vega: stessa ora 37,547 (t 1,0), ritardi -57,625 (t -1,5) e -41,673 (t -1,1). Il flusso di SPY: stessa ora 14,799 (t 8,5), due ore prima **-5,321 (t -3,6)**, un ritorno | Tab. 4 |
| 8 | **Il prezzo predice il flusso in delta**: nell'equazione del flusso in delta, la variazione del prezzo di un'ora prima ha 0,007 (t 5,7), di due ore prima 0,003 (t 2,2); quella della volatilita' di un'ora prima 1,665 (t 2,1). Nell'IRF uno shock del prezzo porta **+2,46 di flusso in delta (t 5,17)** in quattro ore. Il flusso in delta e' anche autocorrelato (0,178, t 4,3; poi -0,076, t -2,3) | Tab. 3, Tab. 4 |
| 9 | **SPY guida, le opzioni seguono**: lo shock di SPY porta flusso in delta (1,03, t 2,11), non il contrario (0,44, t 1,62); nella stessa ora il flusso in delta e quello di SPY vanno insieme (0,086, t 6,0), il che per gli autori vuol dire che si prende esposizione con entrambi, non che si copre l'una con l'altro | §1, §4.2, Tab. 3, Tab. 4 |
| 10 | **Il tempo dell'effetto**: "the majority of the effects occurs in the first hour" (Fig. 1). Sul grafico la risposta del prezzo al flusso in delta e' gia' vicina al valore finale all'ora zero, cioe' **nella stessa ora** dello shock (circa 105 cents all'ora 0, circa 115 all'ora 1, poi circa 98 dalla 2 alla 4; lettura del grafico in verifica) | §4.2, Fig. 1 |
| 11 | **Delta e vega insieme spiegano il 99,2%** delle variazioni orarie dei prezzi delle opzioni (somma dei coefficienti 0,997 sul prezzo e 0,899 sulla volatilita'; diverse da 1 con t 14,6 e 37,3 per la dimensione del campione); nel gruppo a scadenza breve e lontano dal denaro va peggio (0,612 e 1,35; R quadro 0,863) | §4.3, Tab. 5 |
| 12 | **Il flusso in vega si puo' sommare fra gruppi** (Wald: p 0,262 nell'equazione della volatilita', 0,694 in quella del prezzo); **il flusso in delta no**: il gruppo a scadenza breve e lontano dal denaro ha un impatto piu' alto sul prezzo e piu' negativo sulla volatilita' (p non dato) | §1, §4.3 |
| 13 | **Alla mezz'ora piu' debole, al minuto niente**: le risposte si riducono e al minuto non sono significative; al minuto delta e vega spiegano solo il 47,9% delle variazioni dei prezzi delle opzioni (42,1%, 28,8%, 19,1% nei gruppi lontani dal denaro). La ragione: spread e tick delle opzioni (spread mediano da 0,35 a 1,56 dollari per le opzioni sotto i 40 dollari, "da 7 a 15 tick" dice il testo, ma dalla Tab. 6 sono da 7 a 31 tick di 0,05; deviazione standard delle variazioni al minuto fra 0,05 e 0,17 dollari, "da 7 a 10 volte" meno dello spread dice il testo, dalla Tab. 6 da 7 a 23 volte). **L'ora e' "il miglior compromesso"** | §1, §5, Tab. 6, Tab. 7, Fig. 2, Fig. 3 |
| 14 | **Robustezza**: con prezzo e volatilita' scambiati la risposta di lungo periodo e' "nearly identical", con SPY come variabile piu' esogena "very similar"; con il forward "virtually identical"; con i rendimenti uguali dopo la scala; senza SPY i risultati sono **piu' significativi**, perche' SPY assorbe parte del potere predittivo. I numeri della robustezza non sono dati, solo figure. **Ma nella Fig. A2** (SPY piu' esogeno, l'unico ordinamento che da' davvero a SPY la parte comune) la risposta del prezzo al flusso in delta si legge intorno a 70-75 cents invece di 98, circa un quarto in meno, con la banda sempre sopra zero (lettura del grafico in verifica; il nostro conto con la Tab. 4 da' circa 73 nell'ora) | §6, Fig. A1-A5 |

**Il conto che il paper non fa** (nostro, dalla Tab. 4, da ricontrollare): con l'ordinamento del paper, la
risposta del prezzo **nell'ora stessa** a uno shock di 8,23 del flusso in delta viene circa 106 cents: circa 22
dal coefficiente diretto (2,735 x 8,23), circa 10 attraverso il flusso di SPY che va insieme, e circa 73
attraverso la volatilita' che nella stessa ora scende (-0,012 e -0,056 per -525,69). Dopo quattro ore e' 98,13
[Tab. 3]: **tutto l'effetto e' nella stessa ora, e poi cala un poco**; e circa due terzi passano dal canale
"il flusso va con un VIX che scende, il VIX che scende va con un prezzo che sale". Lo stesso per il vega:
circa 0,09 punti nell'ora, 0,07 dopo quattro.

**Rifatto in verifica** (Tab. 4 guardata come immagine, pagina 38 del PDF): 105,7 cents = 22,5 diretti + 10,5 da
SPY (0,086 x 8,23 = 0,71 di flusso di SPY, per 14,799) + 72,8 dalla volatilita' (d_v nell'ora -0,138, di cui
51,9 cents dal flusso in delta e 20,8 da SPY). L'arrotondamento a tre decimali di -0,012 sposta il totale fra
103,6 e 107,9. Il vega: 0,092 punti nell'ora. Coincide con la Fig. 1 (circa 105 cents e circa -0,13 punti
all'ora 0). In piu', **la previsione vera** (la forma ridotta, cioe' quanto il flusso di un'ora fa dice del
prezzo dopo senza conoscere le altre variabili di quell'ora; nostro conto, il paper non la stampa ne' ne da' la
t): il ritardo di un'ora vale circa 0,01 cents per 100 milioni (fra -0,25 e +0,28 con l'arrotondamento), quello
di due ore circa -1,9, cioe' un piccolo ritorno. Le t 0,6 e 0,4 della Tab. 4 sono coefficienti **a parita' delle
variabili della stessa ora**; la forma ridotta conferma la lettura "stessa ora, non previsione".

## Il Perche'

**Degli autori** [KVS §1, §4.2, §7]: il quadro di Kyle e di Glosten e Milgrom. Chi ha un'informazione privata
sul prezzo compra call e vende put (flusso in delta positivo, poco vega); chi l'ha sulla volatilita' compra
call e put insieme (flusso in vega). I market maker vedono il flusso, aggiornano il valore atteso e spostano i
prezzi di **tutte** le opzioni. La prova che e' informazione e non pressione temporanea: la volatilita' e'
estratta da tutta la sezione delle opzioni (difficile che una domanda temporanea su un'opzione le muova tutte),
e l'effetto non torna indietro entro i ritardi del VAR [KVS §4.2, §7].

**Perche' SPY pesa di piu' per dollaro**: gli scambi di opzioni sono un ordine di grandezza piu' grandi;
l'ETF attira scambi piccoli e informati, le opzioni chi vuole scambiare grosso; ed e' coerente con spread
effettivi delle opzioni piu' bassi di quanto si pensi (Muravyev e Pearson) [KVS §1, §4.2].

**Perche' al minuto non funziona**: le opzioni hanno tick e spread grandi rispetto ai movimenti di un minuto,
le quotazioni non si aggiornano e la volatilita' estratta e' lenta; in piu' l'effetto leva al minuto e' piu'
forte e il VAR non separa prezzo e volatilita' [KVS §5].

**Le alternative che scartano**: la pressione temporanea della domanda (vedi sopra). **Non discutono** che
nella stessa ora sia il prezzo a muovere il flusso: l'ordinamento lo esclude per ipotesi (Hasbrouck), e
in nessuna delle robustezze il prezzo muove i flussi nella stessa ora (in tutte d_u e d_v restano gli ultimi
della lista della Web Appendix, mossi da tutti) [KVS §2, §6, Fig. A1-A2]. Eppure le loro tabelle dicono che il
prezzo dell'ora prima muove il flusso in delta (risultato 8).

**Cosa lo ucciderebbe**, dedotto: (a) un ordinamento con il prezzo che muove i flussi nella stessa ora in cui l'impatto delle
opzioni sparisce (cioe' la correlazione della stessa ora e' il flusso che insegue il prezzo, come coperture o
momentum); (b) un ritorno del prezzo nelle ore dopo (pressione temporanea); (c) in un altro anno, un effetto
che non c'e'. Nessuna delle tre e' provata nel paper.

## Cosa Non Regge O Non Si Dice

1. **"Informato" non vuol dire "anticipa".** L'impatto viene dalla **stessa ora**, attribuito al flusso
   dall'ordinamento; i ritardi del flusso in delta non predicono il prezzo (t 0,6 e 0,4) [Tab. 4], e la
   risposta a quattro ore e' piu' bassa di quella nell'ora (nostro conto sopra, Fig. 1). L'abstract ("option
   trades are indeed informative") e il §5 ("delta and vega order flows predict changes in the underlying")
   dicono piu' di quanto le tabelle mostrano: nel senso di Hasbrouck e' impatto permanente, non previsione.
2. **L'identificazione e' un'ipotesi non provata.** Delle 120 permutazioni possibili ne provano tre, e
   in nessuna il prezzo muove i flussi nella stessa ora [§6]. E l'ordinamento principale **non e' il piu'
   prudente per le opzioni**, come dice il testo: attribuisce al flusso in delta la parte comune con SPY; con
   SPY piu' esogeno (Fig. A2) l'effetto si legge intorno a 70-75 cents, non 98 (risultato 14). Su un'ora intera l'ipotesi "il rendimento non causa il flusso
   nella stessa ora" e' forte: le loro stesse stime dicono che lo causa a un'ora (risultato 8), e la nostra
   misura sull'archivio di X-Ray e HHW lo vedono nello stesso minuto. Lo ammettono per l'effetto leva ("the VAR
   model cannot identify the causality", §6), non per il flusso.
3. **La significativita' e' per osservazione oraria**, 1.494 ore con Newey-West a due ritardi [Tab. 4], non
   per giornata e senza raggrupparle per giorno; le t delle IRF (Tab. 3) non dicono come sono calcolate; sono
   t in valore assoluto, non errori standard (confermato in verifica: tutte le stelle tornano con le soglie
   1,65, 1,96 e 2,58, e la banda della Fig. 1, circa da 55 a 140 cents a quattro ore, da' un errore di circa 22,
   cioe' t circa 4,4). Il risultato sulla volatilita' e' al 5% (t 2,36).
   Nessun modello nullo, nessun campione fuori.
4. **Un solo anno, calmo**: il 2014, volatilita' media 11,5% [nota 14]. Nessuna prova in un altro regime.
5. **Niente 0DTE e niente settimanali**: solo il ciclo del terzo venerdi', da 7 a 180 giorni [§3.1]. Il
   flusso che domina il DEX di X-Ray oggi (gli 0DTE delle SPXW) **non c'e'**.
6. **Nessun gruppo**: il paper non dice niente su chi sia informato; tutti gli scambi insieme, segnati con le
   quotazioni (errori ammessi, nota 9).
7. **Incoerenze nel testo**, da guardare in verifica:
   - "the standard deviation of delta order flow is much larger than ... vega order flow ($3.92 versus
     $0.16)" [§4.1]: nella Tab. 2 la deviazione standard del flusso in delta e' 8,64; 3,92 e' quella del
     prezzo (nota 14). Il conto del rischio "3,92 x 0,2 = 0,78%" usa quindi il numero sbagliato.
   - **L'unita' del prezzo**: d_u e' "in cents" dal prezzo di SPY [Fig. 1], ma il prezzo medio e' 1.930
     dollari [nota 14], che e' il livello dell'indice, non di SPY (circa un decimo). Non dicono se SPY e'
     moltiplicato per 10; i 5,1 punti base, che sono un rapporto, non cambiano.
   - **"il 50% della deviazione standard oraria (1,90 dollari)"** [§4.2] usa il residuo strutturale (189,93,
     Tab. 3), mentre la deviazione standard della Tab. 2 e della nota 14 e' 3,92: 98,13 e' il 25% di quella.
   - **La moneyness**: definita F/K per le put e K/F per le call [§4.1], cosi' sotto 1 vorrebbe dire **dentro**
     il denaro; ma il testo legge 0,945 come "fuori dal denaro", e il gruppo (2) con moneyness 0,878 ha delta
     0,0135 e prezzo 1,09 dollari, cioe' e' molto fuori dal denaro [Tab. 1]. La definizione scritta e'
     rovesciata rispetto ai dati. La Tab. 5 la chiama "S/K" con valori diversi (fino a 1,49).
   - La nota 12 dice "deep in the money puts (with a delta just below zero)": il delta di una put molto dentro
     il denaro e' vicino a -1.
   - **La quota di acquisti**: 0,662 nel campione intero, ma la media dei nove gruppi pesata per il numero di
     scambi viene 0,621 (nostro conto dalla Tab. 1; le call, 0,411, tornano; e i nove gruppi sommano
     esattamente 1.804.397 scambi, quindi non e' un problema di scambi esclusi dai gruppi).
   - **L'ordinamento descritto al rovescio** (trovato in verifica): il §2 dice che la parte comune di SPY e
     delta va a SPY, il §6 che nella Fig. A2 va al delta; la Tab. 4 e la Fig. A2 dicono il contrario (risultato
     14, "Il Metodo").
   - **I tick** (trovato in verifica): "spreads typically between 7 to 15 ticks" e "7 to 10 times smaller"
     [§5] non tornano con la Tab. 6 (da 7 a 31 tick, da 7 a 23 volte nelle fasce sotto i 40 dollari).
   - **"Il 25% spiegato dai flussi"** [§1]: l'R quadro 0,25 di d_v comprende anche il flusso di SPY e tutti i
     ritardi [§4.2], non solo delta e vega.
   - **La Tab. 5** (guardata come immagine, pagina 39 del PDF): il testo dice R quadro "exceeding 92%" in
     tutte le colonne da (3) a (10), ma la (8) ha 0,917; e cita una t di 5.551 per il prezzo, mentre la tabella
     stampa 5.323,3. Le somme dei coefficienti (0,997, 0,899, 0,612, 1,35) invece tornano.
8. **Su Chordia e altri e sulle put** il paper dice cose che le sue tabelle non provano: attribuisce a CKMS
   (versione 2019) che "high net put order flow of ISE index options predicts negative index returns ...
   informed market makers" (opzioni dell'ISE, non SPX), e dice di
   trovare "strong support for the opposite effect", in linea con Chen, Joslin e Ni [§1]. **Nessuna tabella
   separa le put**; e la nostra analisi di CKMS (versione accettata) dice put comprate dai piccoli, poi indice
   **piu' alto**, letto come assicurazione. Il confronto non si usa.
9. **Delta e vega dal modello di Heston** calibrato il giorno prima, non quelli di mercato; gli stessi autori
   dicono che le somme dei coefficienti diverse da 1 vogliono dire errori nei greci o fattori in piu', e quindi
   "some bias" nei flussi [§4.3]. Il flusso in delta **non e' omogeneo** fra gruppi di opzioni (Wald,
   risultato 12): sommarlo e' un'approssimazione che il test rifiuta.
10. **Preprint dal 2017**, rivisto nel 2019 e, per quanto dice la pagina SSRN, mai pubblicato. Nessun conflitto
    dichiarato; finanziamento Fondecyt per van Kervel.
11. **Look-ahead**: nei greci no (parametri del giorno prima). I nove gruppi di moneyness e scadenza hanno soglie
    scelte "per avere abbastanza scambi" [§4.1], cioe' guardando i dati, ma non entrano nel finding principale.

## Cosa Dice Al Riquadro "Chi Ha Comprato Delta Oggi"

Il riquadro mostra il DEX di SPX per gruppo di OptionsDepth, 0DTE e tutte le scadenze, sommato dalle 9:31, con
il percentile alla stessa ora e la sorpresa oltre il prezzo (sintesi della ricerca, punto 5). Dal paper:

- **Il modo di sommare e' giusto.** Contratti per delta per segno, sommati su strike, scadenze, call e put, e'
  la misura del paper [eq. 2]; il paper aggiunge il **prezzo** per portarla in dollari e confrontarla con il
  flusso del sottostante. Il riquadro puo' dirlo come riferimento: la sua unita' e' "delta", in dollari
  bisognerebbe moltiplicare per lo spot. Ma il test di Wald dice che **il delta delle opzioni a scadenza breve e
  lontane dal denaro pesa di piu'** sul prezzo [§4.3]: un DEX che somma 0DTE e lunghe scadenze mescola flussi con
  impatto diverso. Tenerli separati, come fa gia' il riquadro, e' coerente.
- **Il delta comprato va con il prezzo nella stessa ora, e non lo anticipa.** E' lo stesso quadro di HHW (stesso
  minuto si', minuto dopo no), di MAMU (ora dopo no) e della nostra misura (il flusso ripete il prezzo). Il
  paper lo chiama "informato", ma nessun numero del paper dice che il delta della mattina dica qualcosa del
  pomeriggio [Tab. 4]. **Il riquadro resta una descrizione.**
- **Il flusso segue il prezzo** (risultato 8): dopo un'ora di rialzo il flusso in delta delle opzioni (tutti
  insieme) e' piu' positivo. E' la **sorpresa oltre il prezzo** che il riquadro gia' calcola, vista da un
  altro paper: il flusso grezzo contiene il prezzo, e va tolto.
- **Il segno tipico non e' zero**: nel 2014 il flusso in delta medio e' **+179 milioni di dollari l'ora** e il
  66% degli scambi e' iniziato da un compratore [Tab. 1, Tab. 2]. Conferma il confronto con le sedute passate.
- **Al minuto il paper non vede niente** [§5]: il riquadro che si aggiorna ogni minuto disegna una grandezza
  che, nella misura del paper, al minuto e' rumore delle quotazioni delle opzioni. Non e' lo stesso dato (il
  DEX di OptionsDepth non dipende dal segno con le quotazioni), ma e' un motivo per leggere il riquadro **a
  blocchi di tempo lunghi** (l'ora, la mattina), non minuto per minuto.
- **Il vega**: il paper trova che il flusso in vega muove la volatilita' (0,07 punti per deviazione standard,
  t 2,36). Se OptionsDepth dia anche il vega scambiato per gruppo e' **da verificare**; senza, non si aggiunge.
- **Cosa non dice**: niente su **quale gruppo** e niente sugli **0DTE** (punti 5 e 6 di "Cosa Non Regge"). Non
  cambia la risposta della sintesi: nessun paper mostra un gruppo che anticipi il verso dell'indice.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

Lo strumento e' NQ, i dati da ATAS (CLAUDE.md, decisione 2); il DEX di SPX e' solo contesto, mai in un
backtest.

- **Il flusso in delta delle opzioni su NQ: da verificare.** Serve ogni scambio delle opzioni CME sul future
  NQ con il lato dell'aggressore e un delta: se la catena CME di ATAS li registri scambio per scambio, e da
  quando, e' da verificare; lo storico all'indietro non c'e'.
- **Con le sole barre di ATAS, il VAR di Hasbrouck sul future** (prezzo di NQ e delta delle barre, cioe' il
  flusso aggressivo del future, a un'ora, ogni giornata che riparte da zero) si puo' fare dal 2017 al 2024,
  dentro il campione. Non e' la domanda del riquadro (e' order flow del future, non delle opzioni per gruppo),
  e misura l'impatto della stessa ora, non una previsione. Ha senso solo come prova di metodo: **con il prezzo
  messo prima e dopo il flusso**, la prova che il paper non fa.
- **Sull'archivio di X-Ray (SPX, descrittivo)**: la regressione oraria del rendimento di ES sul DEX sommato dei
  quattro gruppi, nella stessa ora e un'ora dopo, una stima per giornata e il nullo per permutazione delle
  giornate. **Quante ore** (nostro calcolo, non del paper; stesso effetto, t che cresce con la radice delle
  osservazioni): il paper ha t 4,41 su 1.494 ore; con 35 sedute da circa 6 ore (circa 210 ore) la t attesa e'
  circa 1,7, **e per la sola stessa ora**, che non e' una previsione. Per il ritardo il paper e' gia' nullo.
- **Il fuori campione di NQ e' dal 2025**: ogni sguardo ai dati 2025-2026, anche esplorativo, lo consuma.

## Cosa Ne Prende Il Framework

Il ruolo e' **metodo**.

- **Come si somma un flusso di opzioni**: contratti per delta per segno (per il prezzo, se si vuole il
  dollaro), e lo stesso con il vega; i due flussi separano "verso" e "volatilita'" [eq. 2-3]. Il test di Wald
  per gruppi di scadenza e moneyness dice **se** sommarli e' lecito [§4.3].
- **Separare la stessa ora dal dopo**: un coefficiente della stessa ora, anche "permanente", non e' una
  previsione; si leggono i ritardi [Tab. 4]. E' un controllo da fare a ogni flusso che si dice "informativo".
- **L'ordinamento si dichiara e si rovescia**: ogni VAR strutturale va presentato con l'ordinamento opposto
  accanto; un risultato che vive di un solo ordinamento e' un'ipotesi.
- **La frequenza giusta per le opzioni**: sotto la mezz'ora i prezzi delle opzioni sono dominati da tick e
  spread [§5, Tab. 6]; una misura che usa i prezzi delle opzioni al minuto va dichiarata con questo limite.
- **Ogni giornata riparte da zero** nei ritardi [§4.2]: coerente con la nostra regola della significativita'
  per giornata, ma il paper poi conta le ore come osservazioni; noi no.
- **Da non prendere**: la parola "informato" per un impatto della stessa ora; la significativita' per ora
  senza nullo.

## Per La Verifica

I punti dove il lettore ha ricostruito o calcolato, da guardare sul PDF:

- **Tab. 3 e Tab. 4** sono state lette come immagine (pagine 37-38 del PDF): i valori di ritardo del flusso in
  delta nell'equazione del prezzo (0,446, t 0,6; 0,263, t 0,4) e quelli della variazione del prezzo
  nell'equazione del flusso in delta (0,007, t 5,7; 0,003, t 2,2) sono il cuore della lettura "stessa ora, non
  previsione": vanno ricontrollati cella per cella.
- **Il conto dei circa 106 cents nell'ora stessa** e della ripartizione (22 diretti, 10 da SPY, 73 dalla
  volatilita') e' nostro, dalla ricorsione di Cholesky con i coefficienti della stessa ora della Tab. 4 e la
  deviazione standard 8,23 della Tab. 3; va rifatto. Che la Fig. 1 parta vicino a 100 all'ora zero e' una
  lettura del grafico.
- **Le t della Tab. 3 sono in valore assoluto** (dedotto dalle stelle: 0,07** con 2,36; 0,01 senza stelle con
  0,62): da confermare.
- **Le incoerenze del punto 7** (3,92 contro 8,64; l'unita' del prezzo; 1,90 contro 3,92; la moneyness; la nota
  12; 0,662 contro 0,621): sono del paper o di lettura?
- **La Tab. 7** (al minuto) e' mescolata nel testo estratto e non e' stata resa come immagine: qui si usano solo
  i numeri che il testo ripete (47,9%, 42,1%, 28,8%, 19,1%). La Tab. 5 e' stata guardata come immagine
  (pagina 39, ruotata).
- **La Tab. 6**: il testo cita gli spread da 0,35 a 1,56 e le deviazioni orarie da 0,14 a 1,45 solo per le
  opzioni fino a 40 dollari; la tabella intera arriva a 2,94 e 7,75. Controllato sull'immagine (pagina 40).
- **L'affiliazione di van Kervel** e il numero TILEC vengono dalla pagina SSRN o dal PDF? Nel PDF c'e' la
  Pontificia Universidad Catolica de Chile e nessun numero TILEC.
- **Nessuna istruzione** trovata nel PDF (cercate parole come "ignore", "instruction", "assistant", "prompt":
  solo testo del paper).

## La Verifica

**9 novembre 2026**, fase 4, verifica avversaria sul PDF (sha256 ricontrollato, coincide). Testo da `pdftotext
-layout`; rese come immagine con `pypdfium2` (installato nella scratchpad) le pagine 36-44 e 46-47: Tab. 1-7,
Fig. 1 e Fig. A1-A2. Circa **190 affermazioni** controllate: circa **180 confermate**, **9 corrette o
precisate**, **nessuna tolta**. Nessuna istruzione nel PDF (cercate "ignore", "instruction", "assistant",
"prompt", "AI": niente).

**Confermati, cella per cella**: la Tab. 4 (pagina 38): flusso in delta nell'equazione del prezzo 2,735 (3,8),
ritardi 0,446 (0,6) e 0,263 (0,4); prezzo nell'equazione del flusso in delta 0,007 (5,7) e 0,003 (2,2),
volatilita' 1,665 (2,1); vega 37,547 (1,0), -57,625 (-1,5), -41,673 (-1,1); SPY 14,799 (8,5), -5,321 (-3,6);
0,086 (6,0); -525,69 (-32,2); autocorrelazioni 0,178 e -0,076; R quadro 0,051, 0,092, 0,055, 0,250, 0,766;
1.494 osservazioni. La Tab. 3 (pagina 37): 98,13 (4,41), 171,00 (8,06), 0,07 (2,36), -0,11 (3,31), -0,23
(7,45), 2,46 (5,17), 1,03 (2,11), 0,44 (1,62), sigma 189,93, 4,20, 8,23, 0,50, 0,16; le t sono in valore
assoluto. I dati: 2014, 249 giornate, sei ore 8:45-14:45 di Chicago, 1.804.397 scambi, solo il ciclo del terzo
venerdi' da 7 a 180 giorni, nessun gruppo. Il Wald del §4.3 (p 0,262 e 0,694 per il vega; il delta rifiutato in
entrambe le equazioni, p non dato, per il gruppo a scadenza breve e moneyness bassa). La Tab. 1, 2, 5, 6 e la
Tab. 7 (pagina 41, al minuto: R quadro 0,479, 0,421, 0,288, 0,191; primi due mesi del 2014; dieci ritardi).
Tutte le incoerenze elencate dal lettore sono **del paper**, non di lettura. Il conto dei circa 106 cents
(22 + 10 + 73) e dei circa 0,09 punti nel vega, rifatto: torna, e torna con la Fig. 1.

**Corretti o precisati**:

1. **(La piu' grave) L'ordinamento e SPY.** L'analisi ripeteva il §2: l'ordinamento darebbe a SPY la parte
   comune con le opzioni. La Tab. 4 dice il contrario (il delta entra nell'equazione di SPY, 0,086), e lo
   conferma il conto stesso dell'analisi (10 cents al delta attraverso SPY). Con SPY davvero piu' esogeno
   (Fig. A2) l'effetto del delta sul prezzo si legge intorno a 70-75 cents invece di 98. Corretti "Il Metodo",
   la robustezza, il risultato 14, il punto 2 e le incoerenze.
2. Il VIX ricostruito non e' "il metodo del Cboe": e' interpolazione cubica e quadratura, con i rimandi del paper.
3. "Da 7 a 15 tick" e "da 7 a 10 volte" sono del testo e non tornano con la Tab. 6: annotato.
4. "Il prezzo prima/dopo i flussi": la notazione della Tab. 4 e della Web Appendix e' al rovescio di quella
   dell'eq. 4; riscritto come "il prezzo che muove i flussi nella stessa ora" (tre punti).
5. Chordia e altri: la frase citata riguarda le opzioni dell'ISE, non SPX.
6. La Fig. 1: all'ora 1 la risposta sale a circa 115 prima di scendere a 98.
7. Le t della Tab. 3: da "dedotto" a confermato, con la banda della Fig. 1.
8. Aggiunta la forma ridotta (nostro conto): il ritardo di un'ora del flusso in delta non dice niente del
   prezzo (circa 0,01 cents per 100 milioni), quello di due ore un piccolo ritorno; la lettura "stessa ora, non
   previsione" regge anche cosi'.
9. Aggiunta un'incoerenza: il "25% spiegato da delta e vega" del §1 e' l'R quadro con SPY e i ritardi.

**Resta incerto**: le letture delle Fig. 1 e A2 sono a occhio (le cifre non sono stampate); l'unita' di d_u
(SPY in cents ma prezzo medio 1.930) resta non spiegata dal paper; la data della versione e il numero TILEC
vengono dalla pagina SSRN, non dal PDF. La forma ridotta e' calcolata con coefficienti arrotondati e senza t.
