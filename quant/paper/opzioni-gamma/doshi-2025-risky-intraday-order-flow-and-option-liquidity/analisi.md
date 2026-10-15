---
chiave: doshi-2025-risky-intraday-order-flow-and-option-liquidity
sigla: DPS
titolo: "Risky Intraday Order Flow and Option Liquidity"
autori: [Hitesh Doshi, Paola Pederzoli, Saim Ayberk Sert]
versione_letta: working paper del 23 maggio 2025 (data in prima pagina), PDF anonimo dalla pagina di Doshi
fonte: "pagina dell'autore, Bauer College, University of Houston: elencato come 'Risky Intraday Order Flow and Equity Option Liquidity', con Paola Pederzoli e Saim Ayberk Sert, e con versioni precedenti DPS_Oct_2024 e DPS_Jan_2025; SSRN (versione 2024, stesso titolo con 'Equity')"
identificativo: "SSRN 5006194 (dalla fase 1; in fase 4 SSRN risponde 403, non riverificato)"
url: https://www.bauer.uh.edu/hdoshi/docs/DPS_May_2025.pdf
tipo: preprint
revisione_fra_pari: no
pagine: 70
pdf: doshi-2025-risky-intraday-order-flow-and-option-liquidity.pdf
pdf_sha256: 714182589aba9b2f3fe85d1672fbd6031d46476d31e9e5aba909e7589428a5a9
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-10-15
verificato_il: 2026-10-15
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; dati CBOE LiveVol (OPRA), CRSP, OptionMetrics, CBOE Open-Close, aste di SpiderRock: tutti a pagamento"
---

# Doshi, Pederzoli, Sert 2025: L'Analisi

*Risky Intraday Order Flow and Option Liquidity*, working paper datato 23 maggio 2025. **Il PDF non
porta i nomi**: gli autori (Hitesh Doshi, Paola Pederzoli, Saim Ayberk Sert) vengono dalla pagina di Doshi
alla University of Houston, da cui viene il file, che lo elenca col titolo *... and Equity Option
Liquidity*, e dal record SSRN 5006194. **Non revisionato.**
Letto per intero il 15 ottobre 2026: 56 pagine di articolo (testo, bibliografia, 4 figure, 10 tabelle)
e 14 di appendice online (11 tabelle, 1 figura). Il PDF non contiene frasi rivolte a chi lo legge con
un agente.

Il testo estratto con `pdftotext -layout` mescola le colonne di quasi tutte le tabelle; quello estratto
con `pdftotext -raw` le tiene in ordine, e **i numeri qui sotto vengono dall'estrazione `-raw`**,
pagina per pagina. Il PDF non si e' potuto guardare come immagine (manca `pdftoppm`). **Le stelle della
significativita' e le lettere greche escono con `pdftotext -enc UTF-8 -raw`** (sono caratteri fuori da
Latin-1, che la codifica predefinita butta): le stelle citate qui sotto vengono da quell'estrazione, e
le formule della §4.4 sono trascritte dal PDF. Gli autori segnano * al 10%, ** al 5%, *** all'1%, e
nel testo chiamano "significativo" anche cio' che ha una sola stella. Citazioni `[DPS §n]`, `[DPS Tab.
n]`, `[DPS IA.n]` per l'appendice online.

**Ruolo `metodo`**: il paper non da' una regola ne' parla del future. Da' una misura (la volatilita'
intraday del flusso degli ordini) e una prova indiretta sull'ipotesi su cui poggia ogni GEX.

## I Termini

- **Spread effettivo** (*effective spread*, ES): quanto costa davvero uno scambio rispetto al prezzo
  medio fra miglior denaro e miglior lettera. Qui in percentuale: `ES = 2 |ln P - ln M|`, con P il
  prezzo dello scambio e M il punto medio del **NBBO** (*national best bid and offer*, il miglior
  denaro e la miglior lettera fra tutte le borse) [DPS §4.2, eq. 4]. Giornaliero: media pesata per il
  volume.
- **Flusso degli ordini** (*order flow*) in un intervallo: contratti comprati da chi aggredisce meno
  contratti venduti da chi aggredisce, sommati su tutte le serie del gruppo [DPS §4.1, eq. 1]. E' il
  **delta** di ATAS, ma per le opzioni, e con il lato stimato invece che noto.
- **Sbilancio giornaliero** (*order imbalance*, OI): la somma dei flussi della giornata [eq. 2]. |OI| e'
  il suo valore assoluto: il **momento primo** del flusso.
- **Volatilita' intraday del flusso**, `log(SD)`: il logaritmo della deviazione standard dei 78 flussi a
  5 minuti della seduta (9:30-16:00 ET) [DPS §4.1-4.3]. E' il **momento secondo**: misura quanto il
  flusso va avanti e indietro durante il giorno, anche quando a fine giornata si pareggia.
- **Regola delle quotazioni e regola del tick** (*quote rule*, *tick rule*, Lee e Ready): uno scambio
  piu' vicino alla lettera e' un acquisto, piu' vicino al denaro una vendita; al punto medio si guarda
  la quotazione della borsa dove e' avvenuto, poi il prezzo precedente [DPS §4.1]. OPRA (*Options Price
  Reporting Authority*, il nastro consolidato delle opzioni USA) non dice il lato.
- **ATM** (*at the money*, alla pari): opzioni con delta assoluto fra 0,375 e 0,625 alla chiusura del
  giorno prima; **OTM** (fuori dal denaro) fra 0,125 e 0,375 [DPS §3, §6.3]. **DTE**: giorni alla
  scadenza; **0DTE**: scambiate il giorno in cui scadono [DPS nota 1].
- **Fornitore di liquidita'** (*liquidity provider*) e **market maker**: chi sta sul book e assorbe gli
  ordini degli altri. Due modi di gestire l'inventario [DPS §2]: **abbinare gli ordini** (*trade
  matching*, gestione attiva: rivendere presto a un altro cliente cio' che si e' comprato) oppure
  **coprirsi in delta** (*delta hedging*: compensare l'opzione scambiando il sottostante). La GEX assume
  il secondo.
- **GammaInv** e **DeltaOI**: le due misure del paper per il costo della copertura in delta [DPS §4.4].
  Definite piu' sotto, nel Metodo.
- **CBOE Open-Close**: la base dati che dice, ogni giorno, quante opzioni SPX hanno comprato e venduto
  i clienti che non sono market maker. Sommata dall'inizio, da' (col segno meno) un'approssimazione
  (*proxy*) dell'inventario dei market maker [DPS nota 20]. **Il segno dei market maker qui viene dai
  dati, non da un'ipotesi** come nella GEX di SqueezeMetrics (il confronto e' nostro), ma resta una
  stima.
- **Newey-West**: errori standard che tengono conto che i residui di giorni vicini si somigliano.
  **Effetti fissi** (*fixed effects*): una costante per giorno, per titolo, per borsa o per
  titolo-giorno, che toglie tutto cio' che e' comune a quel gruppo.
- **SLAN** (*single leg auction non intermarket sweep*): gli scambi passati da un'asta di
  miglioramento del prezzo, di solito ordini al dettaglio [DPS §6.3, IA.10].

## La Domanda

Cosa fa costare cari gli scambi sulle opzioni brevi e brevissime (fino a 48 giorni, compresi gli 0DTE)?
Il rischio di inventario che nasce da un flusso degli ordini che va e viene durante il giorno, o il
costo di coprirsi in delta sul sottostante? Dalla risposta gli autori deducono **come i fornitori di
liquidita' gestiscono l'inventario**: abbinando gli ordini o coprendosi [DPS §1, §2].

## Il Metodo

**I dati** [DPS §3]: tutti gli scambi di opzioni su indici e titoli USA da CBOE LiveVol (OPRA), al
millisecondo, con prezzo, quantita', NBBO e quotazione del sottostante, **dal 1 gennaio 2004 al 16 luglio
2021**; uniti a CRSP (titoli) e OptionMetrics (volatilita' implicita e greche giornaliere). Opzioni
SPX (solo CBOE) e opzioni sui titoli dell'S&P 500 (sedici borse). Solo la seduta 9:30-16:00 ET, anche
se le SPX scambiano fino alle 16:15 e di notte. Campione principale: ATM, fino a 48 giorni. Filtri:
scambi cancellati, prezzi, quantita' o spread non positivi, oltre 100.000 contratti, fuori da denaro meno spread
o lettera piu' spread, prezzi sotto 0,10 dollari.

**La classificazione** [DPS §4.1]: regola delle quotazioni, poi del tick. Verificata su circa un
milione di scambi all'asta del **2 febbraio 2024** (fuori dal campione), dati SpiderRock con il lato
vero: **giusta per circa l'85%** [DPS §4.1, nota 7].

**Le regressioni**:

```text
ES_t   = a + b1 log(SD_t) + b2 log(Volume_t) + b3 |OI_t| + controlli di calendario + altri controlli + e   (5)
ES_h,t = la stessa, con le variabili calcolate dentro l'ora h                                            (6)
```

Una regressione per call e una per put, per otto fasce di scadenza (0, 1-6, 7-13, ... 42-48 giorni),
**una osservazione per giorno** [DPS §4.3]. Controlli di calendario: giorno della settimana, mese,
anno. Altri controlli: rendimento dell'SPX, VIX, delta, vega e gamma medi, ES dei due giorni prima. Per
gli 0DTE le greche sono del giorno prima [DPS nota 16]. Errori Newey-West con il ritardo di Andrews e
Monahan.

**La copertura in delta** [DPS §4.4], formule trascritte dal PDF (verificate in fase 4 con
l'estrazione UTF-8, che restituisce Δ e Γ):

```text
DeltaOI_t  = somma sugli intervalli d di | somma sulle serie j di  OI_d,t,j x delta_d,t,j x S_d,t |
GammaInv_t = somma sugli intervalli d di | somma sulle serie j di Inv_d,t,j x gamma_d,t,j x S_d,t^2 |
```

DeltaOI e' il costo di coprire il **flusso nuovo** di ogni 5 minuti, in dollari di delta, su tutte le
serie fino a sette settimane, tutte le moneyness, call e put insieme, come in Dim, Eraker e Vilkov. GammaInv e' la gamma in dollari
dell'**inventario** dei market maker: quello della chiusura di ieri da CBOE Open-Close, aggiornato con
il flusso assorbito durante la giornata, serie per serie (il paper non dice su quali serie si somma:
"for each option series j"). **Entrambe prendono il valore assoluto**: misurano quanto
c'e' da ribilanciare, non il segno.

**Le prove contro le spiegazioni alternative**: la stessa regressione sui titoli singoli, panel con
effetti fissi per titolo, errori raggruppati per giorno e titolo [DPS §4.5]; per borsa, su 16 titoli del
Dow, con effetti fissi titolo-giorno [DPS §4.6]; `log(SD)` contro giorni di utili, terzo venerdi', inizio
e fine mese, volatilita' realizzata [DPS §5]; spread in dollari, volatilita' realizzata dell'opzione,
OTM, senza calendario, senza SLAN, senza la prima e l'ultima mezz'ora [DPS §6, IA].

**Non c'e'**: un campione fuori, una strategia, dati sul future o sugli scambi di copertura.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | Volumi SPX ATM in salita dal 2012-2013. Lo sbilancio giornaliero medio e' positivo sulle put e negativo sulle call; circa 2.000 contratti netti al giorno nel 2010; nel 2020 e 2021 negativo su entrambe, intorno a 2.000. **A fine giornata acquisti e vendite si pareggiano quasi** | §4.1, Fig. 1 |
| 2 | Dentro la giornata no. Flusso medio a 5 minuti su tutti gli anni: -6 contratti sulle call, 9 sulle put (fra -28,8 e 32,7); **deviazione standard fra 229,7 (call, 2004) e 1.764,7 (put, 2011)**; asimmetria bassa. Distribuzione piu' dispersa dalla crisi del 2007; picchi nel 2018 (call) e nel 2011 e 2018 (put) | §4.2, Tab. 1, Fig. 2 |
| 3 | Nei giorni a spread basso (10% piu' basso dell'anno) la deviazione standard e lo scarto interquartile del flusso sono **piu' bassi** che nei giorni a spread alto, nella maggior parte degli anni; media e asimmetria non cambiano. Con le stelle: la differenza di deviazione standard e' significativa in 13 anni su 17 sia sulle call (no 2013, 2016, 2018, 2019) sia sulle put (no 2009, 2011, 2012, 2014; 2015 e 2018 solo al 10%); i quartili Q25 e Q75 delle call in 16 su 17. Di segno opposto: put 2009 (+216,0) e 2011 (+621,6), entrambe non significative | §4.2, Tab. 1 A2/B2 |
| 4 | Spread effettivo medio: **8,7% sulle call 0DTE, 9,5% sulle put 0DTE**, intorno al 2% a 42-48 giorni. Le giornate con 0DTE nel campione sono circa 1.035-1.040 | Tab. 2 |
| 5 | **Il finding.** `log(SD)` -> ES, con tutti i controlli: 0DTE call **0,020 (t 4,10)**, put **0,023 (t 6,19)**; 1-6 giorni 0,020 (t 7,30) e 0,019 (t 7,00); scende con la scadenza fino a 0,001 (t 1,34, non significativo) sulle call a 42-48 giorni e 0,002 (t 4,29) sulle put. Coefficienti standardizzati: 0DTE 0,257 (call) e 0,327 (put), 1-6 giorni 0,306 e 0,400, 42-48 giorni 0,045 e 0,177 | §4.3, Tab. 3 A1/B1/A3/B3 |
| 6 | Nostro calcolo, non del paper: una deviazione standard di `log(SD)` (0,855 sulle call 0DTE) vale 0,020 x 0,855 = **1,7 punti di spread su una media di 8,7**; sulle put 0,023 x 0,843 = 1,9 su 9,5. Il rapporto con le deviazioni standard di ES (0,067 e 0,060) restituisce i coefficienti standardizzati della tabella (0,26 e 0,32): la lettura delle colonne regge | Tab. 2, Tab. 3 |
| 7 | Regressione unica con le fasce: `log(SD)` 0,025 (call) e 0,021 (put) sugli 0DTE; le interazioni con le altre fasce sono tutte negative (da -0,016 a -0,021 call, da -0,011 a -0,015 put), **tutte con tre stelle (1%)**, come dice il testo. La tabella non stampa le t di questi coefficienti, solo le stelle. N 19.579 (call) e 19.635 (put) | §4.3, Tab. 3 A2/B2 |
| 8 | **Il momento primo perde contro il secondo.** Con i soli controlli di calendario, |OI| da solo e' significativo in tutte le fasce (call 0DTE 0,057, t 6,62); messo con `log(SD)` crolla (call 0DTE -0,012, t -1,30). Nella specifica completa |OI| e' significativo solo in poche fasce, e sugli 0DTE call ha segno negativo (-0,016, t -2,07) | §4.3, Tab. 3, IA.3 |
| 9 | Ora per ora: il coefficiente di `log(SD)` di quell'ora sullo spread di quell'ora e' positivo in tutte le celle, senza stelle in una sola: call 35-48 alle 12:00-13:00 (t 1,48); sotto 1,96 ma al 10% le call 35-48 alle 11-12 e 14-15 e le put 0-6 alle 15-16 (vedi Cosa Non Regge, punto 1b). Fascia 0-6 giorni: call da 0,011 (t 4,67) alle 9:30-10:00 a **0,005 (t 2,03) alle 15:00-16:00**; put 0,006 (**t 1,77**) alle 15:00-16:00 | §4.3, Tab. 4 |
| 10 | **La copertura in delta.** GammaInv da sola: positiva sulle call 0DTE (0,522, t 2,24, al 5%) e sulle put 0DTE (0,525, **t 1,70, solo al 10%**); con `log(SD)` diventa -0,036 (t -0,16) e 0,368 (t 1,20); con tutti i controlli 0,040 (t 0,19) e 0,197 (t 0,60). `log(SD)` resta: call 0DTE 0,026 (t 12,39), con i controlli 0,019 (t 3,88). DeltaOI sugli 0DTE non e' mai positivo (call -0,009, t -0,26 da solo); sulle put 0DTE, con tutto, e' negativo al 10% (-0,052, t -1,93) | §4.4, Tab. 5(a), 5(b) |
| 11 | **Ma la copertura non sparisce del tutto.** DeltaOI a 1-6 giorni: call 0,123 (t 6,03) da sola, **0,049 (t 2,58) con `log(SD)` e tutti i controlli**; put 0,115 (t 6,62), **0,054 (t 3,32)** con tutto. GammaInv a 35-41 giorni, con tutto: call **0,130 (t 3,56)**, put 0,069 (t 2,10); put 42-48 0,099 (t 1,93, al 10%). DeltaOI call 35-41, con tutto: 0,013 (t 2,13); call 28-34 0,017 (t 1,75, al 10%). Il testo dice che con la specifica completa le misure di copertura "remain insignificant or negative" [§4.4]: per queste fasce la tabella dice altro | Tab. 5, IA.4, IA.5 |
| 12 | Titoli singoli (S&P 500, panel): `log(SD)` 0,015 (t 29,8) call 0-24 giorni, 0,007 (t 29,0) call 25-48; put 0,014 (t 24,0) e 0,006 (t 23,9). |OI| positivo solo sulle call 25-48 (0,003, t 2,01) | §4.5, Tab. 6 |
| 13 | Per borsa (16 titoli del Dow, CBOE 19,8% del volume, ISE 15%, PHLX 13,5%): lo scambio avviene sulla borsa con lo spread quotato piu' basso (coefficiente fra -3,27 e -11,50, x100); dopo lo scambio la borsa che l'ha preso allarga (+0,17 a +0,91) e le altre stringono. Con effetti fissi titolo-giorno, `log(SD)` della singola borsa resta (tutte le borse): call 0,0038 (t 8,73), put 0,0041 (t 8,44); circa la meta' di quanto con effetti fissi borsa e giorno (0,0079 e 0,0072). Simile sulle sei borse maggiori (0,0036 e 0,0044) | §4.6, Fig. 4, Tab. 7 |
| 14 | **Quando il flusso e' volatile** (titoli singoli): **meno** nei giorni di utili (call 0-24: -0,229, t -20,4) e il giorno prima (-0,141, t -13,9); meno quando la volatilita' realizzata del titolo e' alta (-8,818, t -3,09). Di piu' a fine mese sulle scadenze 25-48 (call 0,131, t 9,85). Il terzo venerdi', sulle 0-24: put 0,022 (t 3,31), **call 0,007 (t 1,07, non significativo)** | §5, Tab. 8 |
| 15 | Robustezza: spread in dollari (call 0DTE 0,035 x 1000, t 8,71); con la volatilita' realizzata dell'opzione, che conta anch'essa, `log(SD)` resta (call 0DTE 0,027, t 4,87; correlazione fra le due da -10% a 30%); OTM piu' forte (call 0DTE 0,038, t 6,10; put 0,047, t 7,31); senza SLAN quasi identico (0,020, t 4,10); senza la prima e l'ultima mezz'ora (0,022, t 5,40); senza giorno della settimana e mese (0,023, t 5,54); spread in differenze (0,021, t 4,10) | §6, Tab. 9, 10, IA.2, IA.8-IA.11 |

## Il Perche'

**Il meccanismo degli autori** [DPS §1, §2]: il modello di Bogousslavsky e Collin-Dufresne (2023). Chi
fornisce liquidita' tiene un inventario piccolo e cerca di pareggiare acquisti e vendite. Se durante
il giorno gli arrivi di compratori e venditori si scostano, resta con una posizione mentre aspetta
l'ordine opposto, e si fa pagare quel rischio nello spread. **A parita' di volume, lo spread sale con
la volatilita' intraday del flusso.** Sulle opzioni brevissime pesa di piu': non c'e' tempo di
guadagnare un premio sull'inventario (Fournier e Jacobs), e il premio va subito nello spread [DPS §1].

**Da qui la conclusione sulla copertura** [DPS §1, §2, §7]: se lo spread fosse il prezzo della
copertura in delta, a spiegarlo sarebbero la gamma dell'inventario e il delta del flusso nuovo
(Jameson e Wilhelm). Spiegano poco, e perdono quando entra `log(SD)`: quindi i fornitori di liquidita'
**gestiscono l'inventario soprattutto abbinando gli ordini, e la copertura in delta e' uno strumento
secondario**. La prova diretta non e' loro: e' di Hu, Kirilova, Muravyev e Ryu (2024), che con i dati
per conto dei market maker sul KOSPI 200 trovano che la maggior parte **non si copre in delta** e chiude
le posizioni indesiderate in pochi minuti [DPS §2].

**Le alternative scartate**:

- **Informazione privata** (Glosten e Milgrom): il flusso e' meno volatile nei giorni di utili, quando
  l'informazione privata dovrebbe essere massima [DPS §5, Tab. 8].
- **Incertezza del mercato**: il flusso e' meno volatile quando la volatilita' del titolo e' alta [DPS
  §5].
- **Volatilita' dell'opzione**: entra nella regressione e non assorbe `log(SD)` [DPS §6.2].
- **Il momento primo** (|OI|, la misura di Christoffersen e altri): perde contro il secondo [DPS §4.3].
- **Variabili omesse comuni**: le assorbono gli effetti fissi titolo-giorno nell'analisi per borsa
  [DPS §4.6].
- **Il dettaglio e l'apertura-chiusura**: tolti, non cambia niente [DPS §6.3].

**Cosa lo ucciderebbe**, dedotto dal perche' (gli autori non lo scrivono):

- una misura di gamma costruita come `log(SD)`, cioe' per la stessa fascia e dagli stessi scambi, che
  spiegasse lo spread e assorbisse `log(SD)`;
- dati sugli scambi di copertura dei market maker SPX (sul future ES o sullo SPY) che mostrassero
  coperture grandi e immediate dopo gli scambi di opzioni;
- un flusso piu' volatile proprio nei giorni di informazione.

## Cosa Dice Sull'Ipotesi Della GEX

La GEX (*gamma exposure*) dei dealer fa due ipotesi: **il segno** delle loro posizioni (corti di put,
lunghi di call, nella convenzione di SqueezeMetrics) e **la copertura**: i dealer tengono il delta a
zero scambiando il sottostante, quindi gamma per movimento di prezzo diventa ordini sul future. DPS
riguarda solo la seconda, e la riguarda **in modo indiretto**.

**Cosa sposta (lettura nostra: il paper non parla mai di GEX).** Gli autori concludono che i
fornitori di liquidita' gestiscono l'inventario soprattutto abbinando gli ordini, con la copertura in
delta come strumento secondario [DPS §1, §7]; la prova diretta che molti market maker non si coprono e'
di Hu e altri sul KOSPI 200, non di DPS [DPS §2]. Se questo vale anche per chi tiene la gamma che la GEX
conta, una parte del flusso di opzioni **non diventa flusso sul sottostante**. Attenzione: l'abbinamento
riguarda soprattutto il flusso nuovo della giornata, e l'open interest da cui si calcola la GEX e' gia'
il residuo dopo gli abbinamenti; su quanto di quel residuo venga coperto DPS non misura niente. La
nostra conseguenza pratica e' quindi un'ipotesi: la GEX si legge come **il flusso di copertura che ci
sarebbe se tutto il residuo si coprisse**, un tetto, non come il flusso che arriva. E' coerente con
VAPG, che stima il tetto dell'effetto degli 0DTE assumendo che tutto si copra.

**Cosa non sposta, e perche' non va letto come "i dealer non si coprono":**

1. **Nessuno scambio di copertura e' osservato.** La conclusione viene da quale variabile spiega meglio
   lo spread delle opzioni [DPS §4.4]. Non ci sono dati sul future, ne' sullo SPY se non come controllo
   [DPS Tab. 4, nota 18]. Un dealer puo' coprirsi molto e far pagare poco la copertura, se il future
   costa poco da scambiare: lo spread non lo vedrebbe.
2. **Il segno non e' provato.** GammaInv e DeltaOI prendono il valore assoluto [DPS §4.4]: misurano
   quanto c'e' da ribilanciare, non se il dealer e' lungo o corto di gamma. L'effetto che la GEX
   promette (smorzare con gamma positiva, amplificare con gamma negativa) qui non e' toccato.
3. **"Secondaria" non vuol dire assente.** Il delta del flusso nuovo resta significativo a 1-6 giorni
   (e sulle call a 35-41) con tutti i controlli (risultato 11), e la gamma dell'inventario a 35-41
   giorni.
4. **Il confronto non e' alla pari.** `log(SD)` si calcola dagli stessi scambi, della stessa fascia e
   dello stesso tipo (call o put), da cui si calcola lo spread; DeltaOI e' di tutto il mercato, tutte le
   scadenze fino a sette settimane e tutte le moneyness [DPS §4.4], e per GammaInv il paper non dice
   di restringere le serie alla fascia. La gara favorisce
   la misura piu' vicina alla variabile spiegata. Il paper non riporta la correlazione fra `log(SD)` e
   GammaInv.
5. **Il campione finisce il 16 luglio 2021** [DPS §3]. Dal paper: le giornate con 0DTE nel campione
   sono circa 1.035-1.040 [DPS Tab. 2, Tab. 3] su circa 17 anni e mezzo di sedute, quindi gli 0DTE SPX
   non c'erano ogni giorno. Che le scadenze giornaliere SPX siano arrivate solo dopo il campione e' un
   **fatto esterno al paper, non verificato qui**. Il mercato di cui parlano VAPG e la nostra GEX
   potrebbe quindi essere un altro.

**Per noi**: la GEX resta un'ipotesi sul flusso, non un fatto. Il passo "gamma dei dealer -> ordini sul
future" va **misurato**, non assunto: e' il test che manca sia a DPS sia a SqueezeMetrics.

## Cosa Abbiamo Riprodotto

Niente.

## Cosa Non Regge O Non Si Dice

1. **Il testo dice piu' delle tabelle, o le legge male**, in quattro punti. (a) Le misure di copertura
   "remain insignificant or negative" con tutti i controlli [DPS §4.4]: non a 1-6 giorni (DeltaOI, ***
   su call e put), ne' a 35-41 (GammaInv *** sulle call, ** sulle put; DeltaOI ** sulle call), ne' a
   42-48 sulle put (GammaInv *), risultato 11; e le fasce lunghe, rimandate all'appendice come
   "qualitatively similar", sono proprio quelle dove la copertura resta. Anche la frase "DeltaOI shows
   a positive relationship with the spread for all maturities except 0DTE" (da solo) non vale a 21-27
   giorni (call t 1,49, put t 0,58) [IA.4, IA.5]. (b) Il legame ora per ora "positive and statistically
   significant across all trading hours and maturity groups, with only one exception (put options with
   35-48 days to maturity during the 12 p.m.-1 p.m. interval)" [DPS §4.3]: le stelle della tabella 4
   dicono che l'unica cella senza stelle e' **la call** 35-48 alle 12-13 (0,001, t 1,48); la put 35-48
   alle 12-13 ha tre stelle (t 3,20). **Il testo scambia call e put**; le altre celle sotto 1,96 (call
   35-48 alle 11-12 e 14-15, put 0-6 alle 15-16) hanno una stella, cioe' sono significative al 10%, e
   gli autori le contano come significative. (c) Il terzo venerdi' alza il flusso delle 0-24 giorni
   [DPS §1, §5]: solo sulle put (***), sulle call no (t 1,07, nessuna stella), risultato 14. (d) GammaInv
   "positively associated" sulle put 0DTE: t 1,70, una stella, significativo **solo al 10%**; lo stesso
   per le put 7-13 (t 1,77).
2. **Le stelle si leggono, ma con soglia larga.** Con `pdftotext -enc UTF-8` le stelle escono: la
   tabella 1 conferma "significativo nella maggior parte degli anni" (13 su 17 per la deviazione
   standard, risultato 3) e la tabella 3 A2/B2 conferma interazioni tutte all'1% (risultato 7). Ma una
   stella e' il 10%, e nel testo "significativo" comprende il 10%: con la soglia del 5% alcune frasi
   (put 0DTE in Tab. 5, celle della Tab. 4) non reggono.
3. **Il legame meccanico fra spread e misura.** ES e `log(SD)` vengono dagli stessi scambi della stessa
   giornata. Un giorno con pochi scambi grandi ha insieme flusso piu' volatile e, plausibilmente, spread
   piu' larghi; il volume e' controllato, la composizione per taglia no. E' correlazione: il paper non
   ha uno strumento esterno.
4. **La classificazione del lato** e' giusta per l'85% su un solo giorno del 2024, fuori campione e solo
   su scambi all'asta [DPS §4.1]. Un errore di classificazione puo' gonfiare o smorzare la volatilita'
   del flusso, e non e' detto che lo faccia a caso (ipotesi nostra, il paper non ne parla).
5. **Le greche intraday.** DeltaOI usa il delta e il prezzo del sottostante "recorded at the end of
   each 5-minute interval" [DPS nota 19]; GammaInv usa la gamma "at each interval d" [DPS §4.4]. Ma le
   greche dichiarate nei dati sono quelle giornaliere di OptionMetrics [DPS §3], e per gli 0DTE il paper
   dice di usare le greche del giorno prima: nota 16 (§4.3, i controlli della regressione SPX) e nota 24
   (§4.5, le opzioni sui titoli). Da dove vengano il delta e la gamma intraday non e' detto, ne' se la
   regola del giorno prima valga anche per GammaInv. Se valesse (ipotesi nostra), la gamma di uno 0DTE
   presa il giorno prima e' molto piu' piccola di quella del giorno stesso, e questo indebolirebbe
   proprio GammaInv sugli 0DTE.
6. **L'inventario dei market maker** e' la somma del flusso dei clienti dall'inizio delle opzioni [DPS
   nota 20]: un'approssimazione con errori che si accumulano, e per le sole opzioni SPX.
7. **Numeri che non tornano**: le tabelle IA.9 (senza giorno della settimana e mese) e IA.11 (senza la
   prima e l'ultima mezz'ora) hanno esattamente lo stesso N in tutte le 16 celle (call 0DTE 1.030, put
   1.033; call 42-48 2.023, put 2.104), diverso da quello della tabella 3 (1.035; 2.165 e 2.216), con
   coefficienti e R2 diversi. Per IA.11 meno giornate sono plausibili (si tengono solo gli scambi delle
   10:00-15:30); per IA.9, che toglie solo variabili, no: il paper non lo spiega. Allo stesso modo la
   tabella IA.1 (B) da' alle call gli stessi N delle put (2.960, 2.903 ...). La tabella 1 si ferma al
   2020, il campione al 2021.
8. **Molte specifiche, nessuna correzione per test multipli**: 8 fasce x 2 tipi x varie specifiche
   (tabelle 3-10, 11 dell'appendice), piu' i risultati "available upon request" [DPS note 14, 21].
   L'effetto principale pero' ha t alte e ripetute, e non dipende da un'unica scelta.
9. **Significativita' per giornata**: una osservazione per giorno, Newey-West; e' quello che chiede il
   nostro metodo. Non c'e' un fuori campione, e non serve: il paper non predice niente, spiega.
10. **Scope**: solo la seduta 9:30-16:00, solo ATM (OTM in robustezza), solo fino a 48 giorni. Le SPX
    scambiano fino alle 16:15 e di notte [DPS nota 6]; quello che succede fuori seduta non c'e'.
11. **Conflitti**: nessuno dichiarato; SpiderRock ha dato i dati delle aste ed e' ringraziata [DPS nota
    7].

## Cosa Si Puo' Rifare Su NQ, Con ATAS

Lo strumento e' NQ, i dati solo da ATAS (CLAUDE.md, decisione 2); per le opzioni, solo ES.

**Il test del paper, no.** Servono gli scambi delle opzioni con il NBBO, per anni. ATAS da' la catena
delle opzioni CME su ES dalla connessione Rithmic, **senza storia**: si registra da quando c'e' (`quant
opzioni`, dal 14 ottobre 2026) [`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md). Su NQ nessuna
opzione. E lo spread di NQ e' quasi sempre di un tick: la variabile spiegata del paper sul future non
ha un analogo diretto.

**La misura, si'.** La volatilita' intraday del flusso si calcola su NQ meglio che sulle opzioni: le
barre a 1 minuto dell'archivio portano `VolumeAsk` e `VolumeBid` (`Barra.Delta`), con il lato di chi
aggredisce dato dalla borsa e non stimato. Da `NQ/m1` (dal 12 aprile 2017, 2.365 sedute di cassa
complete): i 78 delta a 5 minuti della seduta 9:30-16:00, la loro deviazione standard, il logaritmo.
Una variabile di stato giornaliera, accanto al |delta| della giornata (il momento primo). **Solo dentro
il campione** (fino al 31 dicembre 2024): ogni sguardo al 2025-2026 consuma il fuori campione.

**Cosa farne, e che non e' del paper.** Il paper non dice niente sul prezzo del future. Un'ipotesi
nostra, da scrivere in una scheda prima di provarla: le sedute di NQ con delta a 5 minuti piu' volatile
(a parita' di volume) hanno un impatto di prezzo per contratto diverso, o un comportamento diverso
dopo (inversioni, range del giorno dopo). La liquidita' di NQ si misura con l'impatto (escursione per
contratto scambiato, calcolabile dalle barre) o con la profondita' del book, che come storia oggi non
c'e'.

**Il passo della GEX, in avanti, su ES.** Con la catena di ES registrata ogni giorno (GEX e segno, nelle
due convenzioni della fase gamma) e il delta di ES per minuto: nei giorni di gamma negativa, il delta
aggressivo di ES dovrebbe seguire il prezzo (comprare i rialzi) piu' che nei giorni di gamma positiva.
Se non lo fa, la copertura che la GEX suppone non arriva sul future, come DPS lascia intendere. E' la
stessa registrazione proposta per BDLM; un anno basta a vedere la direzione, non a provarla.

## Cosa Ne Prende Il Framework

- **Una cautela, per la fase gamma dello studio globale e per lo studio
  [`opzioni-gamma-es`](../../../studi/opzioni-gamma-es/)**, che e' una lettura nostra e non un risultato
  del paper (DPS non parla di GEX): la GEX e' flusso di copertura **possibile**, un tetto se il residuo
  si coprisse tutto; che arrivi sul future e' un'ipotesi da misurare. Nessun livello di GEX va letto
  come ordini certi. DPS la sostiene solo in modo indiretto (la copertura spiega poco lo spread delle
  opzioni) e si appoggia, per la prova diretta, a un altro paper su un altro mercato.
- **Una misura**: la deviazione standard intraday del delta a 5 minuti, il momento secondo del flusso,
  da mettere accanto al momento primo. Il paper mostra che sulle opzioni il secondo batte il primo;
  su NQ e' un'ipotesi, da dichiarare come variante in una scheda prima di provarla.
- **Un disegno di prova**: momento primo e secondo nella stessa regressione; misure costruite con la
  stessa granularita' prima di metterle in gara (l'errore del punto 4 della sezione sulla GEX).
- **Nessuna regola operativa**: il paper non parla di rendimenti ne' del future.

## La Verifica

**15 ottobre 2026**, fase 4 (verifica avversaria), da un agente diverso da quello che ha letto. Fonte: il
PDF (sha256 ricontrollato, uguale alla scheda), estratto con `pdftotext -raw`, `-layout` e
`-enc UTF-8 -raw` (quest'ultima restituisce stelle e lettere greche); la pagina di Doshi per gli autori.
SSRN 5006194 risponde 403 e non e' stato riverificato.

**174 affermazioni controllate** (ogni numero conta per uno): **156 confermate**, **17 corrette**,
**1 tolta**.

Correzioni gravi:

1. **Il terzo autore e' Saim Ayberk Sert**, non "Mehmet Sert" (pagina di Doshi alla University of
   Houston, che elenca il lavoro con Paola Pederzoli e Saim Ayberk Sert). Il nome sbagliato ricorda
   Mehmet Saglam, citato nella bibliografia del paper.
2. **"Le stelle non escono dall'estrazione" era falso**: escono con `-enc UTF-8`. Con le stelle il
   dubbio della tabella 4 si chiude: l'unica cella senza stelle e' la **call** 35-48 alle 12-13 (t
   1,48), la put della stessa cella ha tre stelle; **il testo del paper scambia call e put**. Le
   interazioni della tabella 3 A2/B2 sono tutte all'1%; la tabella 1 conferma la maggior parte degli
   anni (13 su 17).
3. **"La GEX e' un tetto"** era presentato come conseguenza del paper: ora e' segnata come lettura
   nostra (DPS non parla mai di GEX), con il limite che l'open interest e' gia' il residuo dopo gli
   abbinamenti e che la prova diretta sulla copertura e' di Hu e altri sul KOSPI 200, non di DPS.

Correzioni minori:

4. Le formule di DeltaOI e GammaInv non sono "ricostruite": sono nel PDF (§4.4) e la trascrizione
   coincide. GammaInv: il paper non dice su quali serie somma (DeltaOI si', tutte fino a sette
   settimane).
5. Risultato 11 e Cosa Non Regge 1(a): alle eccezioni alla frase "insignificant or negative" si
   aggiungono DeltaOI call 35-41 (t 2,13), GammaInv put 42-48 (t 1,93, 10%) e DeltaOI call 28-34
   (t 1,75, 10%); anche "DeltaOI positivo in tutte le fasce tranne 0DTE" non vale a 21-27 giorni.
6. Put 0DTE di GammaInv (t 1,70) e put 0-6 alle 15-16 (t 1,77): una stella, significative al 10%;
   gli autori chiamano "significativo" anche il 10%. DeltaOI put 0DTE con tutto: -0,052, t -1,93.
7. CBOE Open-Close da' un'approssimazione dell'inventario, non una misura; il confronto con
   SqueezeMetrics e' nostro.
8. Greche intraday: le note 16 e 24 riguardano i controlli SPX e le opzioni sui titoli, non GammaInv;
   se la regola del giorno prima valga per GammaInv non e' detto (l'indebolimento degli 0DTE resta
   un'ipotesi nostra).
9. N di IA.9 e IA.11: per IA.11 meno giornate sono plausibili, l'anomalia e' IA.9; aggiunta la stessa
   anomalia in IA.1 (le call con gli N delle put).
10. Scadenze giornaliere SPX dopo il campione: resta segnato come fatto esterno non verificato; dal
    paper solo che le giornate 0DTE sono circa 1.035-1.040 su circa 17 anni e mezzo.
11. Minori: filtro sugli spread non positivi; 78 intervalli definiti in §4.1; risultato 13 con la
    specifica "tutte le borse" e le sei maggiori; errore di classificazione "non casuale" ridotto a
    ipotesi; titolo sulla pagina dell'autore con "Equity".

Tolta: "per gli 0DTE l'inventario della chiusura di ieri e' quasi tutto di altre scadenze" (il paper
non lo dice e non e' chiaro su quali serie GammaInv si calcoli).

Confermati sul PDF i numeri chiave: campione 1 gennaio 2004 - 16 luglio 2021 (§3); 78 flussi a 5
minuti (§4.1); Tab. 3 call 0DTE 0,020 (t 4,098), put 0,023 (t 6,192); Tab. 5(a) GammaInv call 0DTE
0,522 (t 2,240), poi -0,036 (t -0,157) con `log(SD)`; DeltaOI 1-6 con tutto call 0,049 (t 2,578), put
0,054 (t 3,322); GammaInv 35-41 con tutto call 0,130 (t 3,558) [IA.4]; Tab. 8 terzo venerdi' call
0,007 (t 1,067). Il PDF non contiene frasi rivolte a chi lo legge con un agente.
