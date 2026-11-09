---
chiave: chordia-2021-index-option-trading-activity-and-market-returns
sigla: CKMS
titolo: "Index Option Trading Activity and Market Returns"
autori: [Tarun Chordia, Alexander Kurov, Dmitriy Muravyev, Avanidhar Subrahmanyam]
versione_letta: versione accettata del 17 ottobre 2019 ("Management Science, forthcoming"), da SSRN 2798390, con l'Internet Appendix della stessa data; la versione pubblicata (Management Science 67(3), 2021, 1758-1778) non e' stata letta
fonte: SSRN; pubblicato in Management Science 67(3), 1758-1778 (dati della rivista da Crossref, fase 1)
identificativo: "doi 10.1287/mnsc.2019.3529; SSRN 2798390"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2798390
tipo: rivista (letta la versione accettata dell'autore, non quella impaginata)
revisione_fra_pari: si
pagine: 59
pdf: chordia-2021-index-option-trading-activity-and-market-returns.pdf
pdf_sha256: fa1b5c768f3f4bc34a66af5844b572ad362e635a78e919791e3271ec0a8f9998
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati a pagamento: ISE Open/Close Trade Profile (Nasdaq), Cboe Open-Close da Market Data Express, Tick Data, TAQ, OPRA, OptionMetrics; FRED pubblico"
---

# Chordia, Kurov, Muravyev, Subrahmanyam 2021: L'Analisi

*Index Option Trading Activity and Market Returns*, Tarun Chordia (Emory), Alexander Kurov (West
Virginia), Dmitriy Muravyev (Michigan State), Avanidhar Subrahmanyam (UCLA e Nanjing). **Letta la
versione accettata del 17 ottobre 2019**, scaricata da SSRN, che porta la scritta "Management Science,
forthcoming": **non e' la versione pubblicata** (Management Science 67(3), 2021), che non abbiamo visto.
I numeri qui sotto valgono per questa versione; nella pubblicata possono essere cambiati. Revisionato
(accettato dopo due referee, lo dicono i ringraziamenti). Letto per intero il 9 novembre 2026: 51 pagine
di articolo (frontespizio e 50 pagine numerate: testo, tabelle 1-10, figure 1-5, Appendice A) e 8 di
Internet Appendix (Tab. A1, A2, Fig. A1), 59 in tutto. Le tabelle sono state rilette con `pdftotext -table`
pagina per pagina, che le rende in colonne pulite; le formule escono senza lettere greche e sono
ricostruite dal testo (lo diciamo dove succede). Le figure 2-5 e A1 nella lettura non erano state viste;
**nella verifica le abbiamo rese come immagini** (con pypdfium2), insieme alle pagine delle formule e delle
tabelle 2, 4-7 e A2, e i valori letti dai grafici li segnaliamo come "dalla figura", approssimati. Citazioni `[CKMS §n]`, `[CKMS Tab. n]`, `[CKMS IA]`
per l'Internet Appendix. Nessuna istruzione trovata nel PDF.

## In Parole Semplici

Quando i piccoli investitori comprano **put** (opzioni che proteggono dai ribassi) sugli indici
**Nasdaq-100 e Russell 2000** all'ISE, una borsa di opzioni, l'S&P 500 nelle **settimane dopo** tende a
salire, non a scendere. Il legame c'e' soprattutto quando il VIX e' alto. Gli autori non lo leggono come
informazione ("sanno qualcosa") ma come **assicurazione**: chi ha paura compra protezione quando
l'incertezza e' alta, e in quei momenti il mercato paga un premio piu' alto a chi tiene il rischio. Il
flusso delle opzioni **sull'S&P 500** (SPX, istituzionali) invece **non predice niente**, e nemmeno il
flusso dei future E-mini, di SPY e dei titoli. L'orizzonte e' la **settimana**, non la giornata.

## I Termini

- **ISE** (*International Securities Exchange*): borsa di opzioni americana, oggi di Nasdaq. Il suo
  archivio *Open/Close Trade Profile* dice, per ogni giorno, quanti contratti sono stati comprati e
  venduti **per aprire** o **per chiudere** una posizione, e da quale lato stava il market maker [§2].
- **Market maker**: chi quota sempre un prezzo in acquisto e in vendita e prende l'altro lato degli
  ordini. **Cliente** (*customer*): chi non e' market maker e non scambia per conto proprio di un
  membro della borsa; dentro ci sono i piccoli investitori. **Firm**: un membro (per esempio una banca)
  che scambia per il proprio conto [§5.2].
- **OIB** (*order imbalance*, sbilancio degli ordini): in una settimana, contratti comprati per aprire
  meno contratti venduti per aprire, diviso il totale dei contratti scambiati per aprire. Sta fra -100%
  e +100% [§2, punto 1]. **OIB Puts** e' lo sbilancio sulle put, **OIB Calls** sulle call. Un OIB Puts
  positivo vuol dire che, nella settimana, chi non e' market maker ha comprato put piu' di quante ne ha
  vendute.
- **Regola del tick** (*tick rule*): un modo di dire se uno scambio l'ha voluto il compratore o il
  venditore guardando se il prezzo e' salito o sceso rispetto all'ultimo prezzo diverso. Il paper la usa
  sugli E-mini [§2, punto 2]. ATAS invece da' il lato dell'aggressore gia' classificato.
- **VAR** (*vector autoregression*): un sistema di regressioni in cui ogni variabile (rendimento,
  sbilanci, tassi, VIX) si spiega con i propri valori passati e con quelli di tutte le altre. Le
  **funzioni di risposta all'impulso** (IRF) dicono di quanto si muove il rendimento cumulato nelle 10
  settimane dopo uno shock di una deviazione standard di una variabile; l'**ordinamento di Cholesky**
  e' la scelta, arbitraria, di chi muove chi nella stessa settimana [§3.2].
- **Causalita' di Granger**: "X causa Y nel senso di Granger" vuol dire solo che i valori passati di X
  aiutano a prevedere Y, oltre ai valori passati di Y. Non e' una causa vera.
- **Newey-West**: errori standard che tengono conto dell'autocorrelazione dei residui.
- **R quadro fuori campione** (R2OS): di quanto la previsione riduce l'errore quadratico rispetto alla
  media storica calcolata solo sul passato; positivo vuol dire meglio della media. Il test di **Clark e
  West** dice se la differenza e' significativa [IA §A].
- **Modello a cambio di regime di Markov**: il rendimento segue due regimi (qui, varianza bassa e alta)
  che si alternano a caso; il modello stima da solo in che regime si e' [§4, App. A].
- **Spread di credito, spread a termine, tasso a breve**: i tre indicatori macro di contorno (BAA meno
  AAA di Moody's; Treasury a 10 anni meno 3 mesi; Treasury a 3 mesi), in differenze prime [§2, punto 7].

## La Domanda

Il flusso degli ordini sulle **opzioni su indici** porta informazione sul rendimento futuro del mercato,
come quello sulle opzioni dei singoli titoli lo porta sui titoli? Pan e Poteshman (2006) non avevano
trovato niente su OEX, SPX e NDX; qui si prova con un dato diverso, l'ISE, e a orizzonte settimanale
[§1].

## Il Metodo

**I dati** [§2]: dal 2 gennaio 2006 al 29 dicembre 2017, **615 settimane** [Tab. 1].

- **Il flusso delle opzioni**: OIB settimanale delle opzioni su indici all'ISE, **calcolato mettendo
  insieme le opzioni sul Russell 2000 (RUT) e sul Nasdaq-100 (NDX)**, separato fra call e put. RUT e NDX
  fanno circa il 94% del volume delle opzioni su indici all'ISE. **Le opzioni RUT sono state tolte dal
  listino ISE il 22 aprile 2013: da li' alla fine del campione il flusso e' solo NDX** [§2, nota 11]. La
  Tab. 1B lo conferma: le colonne RUT hanno 382 settimane, quelle NDX 615. Solo i contratti **di
  apertura**: quelli di chiusura "non predicono", non tabulato [nota 9]. Due buchi nei dati ISE (8-26
  febbraio 2016, 5 giugno - 28 luglio 2017). Sbilanci **winsorizzati** all'1% e al 99% [§2]. Circa due
  terzi del volume e' al denaro (0,9 < S/K < 1,1), un terzo fuori, l'1% dentro; scadenza media pesata
  per il volume circa 30 giorni per le call e 32 per le put [§2].
- **Chi e' "il compratore"**: l'ISE identifica il lato del market maker, quindi l'OIB e' **misurato e
  non stimato** con l'algoritmo di Lee e Ready [§2]. Il testo dice "net buying of ISE put options by
  customers" [§1]; nella scomposizione per taglia il denominatore e' "customer and firm trades" [§5.2,
  Tab. A2]. **Lettura nostra**: l'OIB principale e' il flusso di tutti quelli che non sono market maker
  (clienti e firm); i clienti ne fanno circa il 90% [§5.2].
- **I controlli**: lo sbilancio settimanale degli **E-mini S&P 500** (regola del tick, dati Tick Data);
  dei **titoli dell'S&P 500 al NYSE** (Lee e Ready, TAQ, in dollari); di **SPY** (TAQ, in dollari); delle
  **opzioni SPX** al Cboe (Open-Close da Market Data Express, costruito "esattamente" come quello ISE)
  [§2, §4]; in Tab. 7 compare anche uno sbilancio delle **opzioni su ETF all'ISE** ("OIB ... ISE ETF")
  che il testo non descrive.
- **La variabile da prevedere**: il rendimento logaritmico settimanale dell'**indice S&P 500**,
  dall'apertura del primo giorno della settimana alla chiusura dell'ultimo (salta il fine settimana)
  [§2, punto 5]. Con Russell 2000, Nasdaq-100, SPY o E-mini come bersaglio i risultati sono "simili",
  **non tabulato** [§2, punto 5; §1].
- **Il contorno**: VIX; tasso a 3 mesi, spread a termine, spread di credito (FRED), medie settimanali
  in differenze prime [§2].

**Le prove**:

1. **Regressione predittiva** [§3.1, eq. 1, Tab. 2], minimi quadrati con Newey-West. Ricostruita dal
   testo e dalle righe della tabella (le lettere greche mancano):
   ```text
   y_t = a + somma_{i=1..3} b_i R_{t-i} + somma c1_i OIBCalls_{t-i} + somma c2_i OIBPuts_{t-i} + somma d_i VIX_{t-i} + e_t
   ```
   con y, in turno, il rendimento dell'S&P 500, il tasso, i due spread e il VIX; tre ritardi.
2. **VAR a 7 variabili**, tre ritardi scelti con il criterio di Akaike; somma dei coefficienti dei
   ritardi con test di Wald; Granger; IRF a 10 settimane, ordine OIB Calls, OIB Puts, S&P 500, tasso,
   spread a termine, spread di credito, VIX [§3.2, eq. 2, Tab. 3, Fig. 3].
3. **VAR con interazione VIX alto**: variabile indicatrice uguale a 1 nelle settimane con il VIX nel
   quartile piu' alto (156 su 615), interagita con gli OIB [§3.3, eq. 3, Tab. 4, Fig. 4].
4. **Robustezza** [§4]: interazione con lo spread TED e con le settimane di forte ribasso; lo spread di
   volatilita' implicita call-put; il premio per il rischio di varianza; lo SKEW del Cboe (tutti non
   tabulati); il modello a cambio di regime di Markov [eq. 4, Tab. 5, Fig. 5]; i flussi non-opzioni
   [Tab. 6, Fig. A1]; il flusso delle opzioni SPX [Tab. 7].
5. **Le spiegazioni** [§5]: annunci macro (CPI, PIL, disoccupazione, FOMC; 424 settimane su 615 ne
   contengono almeno uno) [eq. 5, Tab. 8]; OIB positivo e negativo separati [Tab. 9]; spread e volume di
   call e put [Tab. 10]; ordini piccoli dei clienti (fino a 100 contratti) contro gli altri [Tab. A2].
6. **Fuori campione** [IA §A, Tab. A1]: regressione ricorsiva (finestra che si allarga), inizio della
   previsione nel 2010, 2011, 2012, 2013 o 2014, R2OS di Campbell e Thompson, statistica di Clark e West
   a una coda. Il quartile alto del VIX qui si calcola con i dati dal 1990 **fino al giorno t** [IA nota 1].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Gli sbilanci medi sono negativi**: OIB Calls -2,78% (mediana -2,65%, deviazione standard 7,16), OIB Puts -1,48% (-1,43%, 5,93). Sulle opzioni ISE i clienti sono **venditori netti** di call e di put, il contrario delle opzioni SPX, dove lo sbilancio medio e' positivo (Garleanu, Pedersen, Poteshman 2009). OIB E-mini 0,01%, NYSE -0,21%, SPY 0,53% | §2, Tab. 1A |
| 2 | Volume medio di apertura per settimana: 35.440 call e 43.270 put; NDX 10.350 call e 14.810 put; RUT 40.400 call e 45.820 put (su 382 settimane) | Tab. 1B |
| 3 | **Nella stessa settimana** OIB Puts e rendimento S&P 500 non sono legati (-0,05, p 0,19); l'OIB E-mini si' (0,60). Correlazioni fra gli indici: S&P 500 - Nasdaq-100 0,87, S&P 500 - Russell 2000 0,92, Nasdaq-100 - Russell 2000 0,85 | Tab. 1C |
| 4 | **Regressione predittiva, rendimento S&P 500**: OIB Puts al ritardo 1 **0,0366 (t 2,19)**, al ritardo 2 **0,0527 (t 3,14)**, al ritardo 3 0,0010 (t 0,07); OIB Calls nessun ritardo significativo (t fra -0,82 e 0,33). R quadro corretto 2,38%. Una deviazione standard in piu' di OIB Puts vale **+0,22%** sull'S&P 500 della settimana dopo (nostro controllo: 0,0366 x 5,93 = 0,217). Lo stesso OIB Puts predice un VIX **piu' basso** due settimane dopo (-0,0528, t -2,10) | §3.1, Tab. 2 |
| 5 | **Il segno e' "al contrario"**: piu' put comprate (un flusso ribassista) predicono rendimenti **piu' alti** | §3.1 |
| 6 | Divisi in quintili di OIB Puts, i rendimenti delle una e due settimane dopo crescono dal quintile basso a quello alto; differenza fra gli estremi significativa con p sotto l'1%. **Dalla figura** (valori approssimati): a una settimana -0,13%, -0,10%, -0,13%, +0,03%, **+0,79%** dal quintile 1 al 5, cioe' piatto nei primi tre e quasi tutto nel quintile piu' alto (le settimane di piu' put comprate); a due settimane -0,61%, -0,18%, +0,19%, +0,40%, +1,18%, crescente. Il testo dice che primo e ultimo quintile hanno rendimenti a due settimane "simili": **la figura no** (-0,6% contro +1,2%); torna solo come distanza dalla media, circa -0,8 e +1,0 punti (lettura nostra) | §3.1, Fig. 2 |
| 7 | **VAR**: somma dei tre ritardi di OIB Puts sul rendimento **0,0956, Wald 14,26, p 0,0002**; sul VIX -0,1050 (p 0,0029). OIB Calls -0,0145 (p 0,49). Granger: OIB Puts -> S&P 500 chi quadro 18,00 (p 0,00); OIB Puts -> VIX 10,28 (p 0,02); S&P 500 -> OIB Calls 14,82 (p 0,00) | §3.2, Tab. 3A, 3B |
| 8 | **La grandezza**: uno shock di una deviazione standard dell'OIB Puts da' **circa +0,55% cumulato in 10 settimane**, che gli autori chiamano "circa 2,9% annuo". Regge con l'S&P 500 messo primo nell'ordine di Cholesky; le IRF "generalizzate" sono simili. **Dalla figura**: la risposta cumulata **parte negativa**, circa -0,22% nella settimana dello shock (banda di due errori standard tutta sotto zero, da circa -0,41 a -0,03), torna a zero alla seconda, sale a circa +0,31% alla terza e a +0,55% alla decima; la banda inferiore sta appena sopra lo zero dalla quarta settimana. Lo 0,55% e' quindi un netto: dalla seconda alla decima settimana il rendimento cumulato sale di circa 0,77 punti | §1, §3.2, Fig. 3, nota 16 |
| 9 | Residui del VAR: OIB Puts con il VIX **+0,11** (p 0,00), con il rendimento **-0,09** (p 0,03): le put si comprano quando il VIX sale e il mercato scende, nella stessa settimana | Tab. 3C, §5.2 |
| 10 | Sentiment (AAII, Investors Intelligence, Baker-Wurgler mensile) e indice d'incertezza politica di Baker, Bloom, Davis aggiunti al VAR: non spiegano il rendimento e non cambiano il resto. **Non tabulato** | §3.2 |
| 11 | **VIX alto**: la somma dei ritardi di OIB Puts sul rendimento e' **0,0235 (p 0,41) fuori dal quartile alto** del VIX, e **0,2870 (Wald 34,68, p 0,0000)** dentro (0,0235 + 0,2635). "Predicono **solo** nei periodi di VIX alto"; l'effetto cresce di oltre 10 volte. IRF a 10 settimane: OIB Puts 0,55 (errore standard 0,27), OIB Puts x VIX alto 0,83 (0,26); OIB Calls x VIX alto 0,65 (0,29), che pero' "non regge" cambiando l'ordine. **Dalle figure 4 e A1**: la risposta a OIB Calls x VIX alto e' gia' circa 0,6 nella settimana dello shock e poi resta piatta (contemporanea, non predittiva); quella a OIB Puts x VIX alto parte da circa -0,1 e sale fino a 0,8-0,9 (predittiva) | §3.3, Tab. 4, Fig. 4, Fig. A1 |
| 12 | **Cambio di regime di Markov**: OIB Puts conta solo nel regime ad alta varianza (ritardo 1: 0,1348, z 2,23; ritardo 2: 0,2081, z 2,85); nel regime a bassa varianza z fra 0,18 e 0,68. Deviazione standard degli errori 3,74 contro 1,38 (circa 2,7 volte); durate attese circa 52 e 19 settimane (probabilita' di restare 0,9807 e 0,9469). La probabilita' del regime alto correla 0,78 con il VIX (0,82 con l'indicatrice del VIX alto). **Dalla figura**: il regime alto si concentra fra il 2007 e il 2011, con episodi brevi nel 2014-2016; nel 2012-2013 e nel 2017 il modello sta quasi sempre nel regime basso. L'effetto viene quindi soprattutto dagli anni della crisi | §4, Tab. 5, Fig. 5 |
| 13 | **I flussi degli altri strumenti non predicono**: somme dei ritardi sul rendimento E-mini -0,0822 (p 0,55), titoli NYSE 0,0465 (p 0,29), SPY -0,0030 (p 0,96). OIB Puts con VIX alto resta 0,2863 (p 0,0000). L'IRF dell'E-mini (1,28, errore 0,25) e' grande ma **contemporanea** (dalla Fig. A1: circa 1,3 gia' nella settimana dello shock, poi piatta): correlazione dei residui E-mini - rendimento circa 0,60. L'IRF dei titoli NYSE e' 0,54 (0,29), non in grassetto; dalla Fig. A1 cresce piano, da circa 0,25 a 0,54, con la banda inferiore intorno a zero | §4, Tab. 6, Fig. A1 |
| 14 | **Le opzioni SPX non predicono**: IRF del rendimento a 10 settimane per OIB Puts SPX **0,02 (0,30)**, OIB Calls SPX 0,03 (0,26). Con quei controlli l'IRF di OIB Puts ISE e' **0,89 (0,26)**, e 0,89 (0,26) anche l'interazione con il VIX alto. Nella stessa tabella l'IRF dello sbilancio dei **titoli NYSE e' 0,70 (0,27), in grassetto** (significativa per gli autori), e quella dell'E-mini 1,20 (0,25): il testo non commenta la prima | §4, Tab. 7 |
| 15 | **Annunci macro**: somma di OIB Puts **0,1067 (p 0,0007) nelle settimane con annuncio** e 0,0636 (p 0,25) nelle altre. Con l'indicatrice sulla **settimana prima** dell'annuncio: 0,1154 (p 0,0085) prima, 0,0837 (p 0,0434) nelle altre. Gli autori: nessuna differenza prima dell'annuncio, quindi niente informazione anticipata | §5.1, Tab. 8 |
| 16 | **I market maker non sembrano informati**: i rendimenti settimanali delle put al prezzo medio hanno autocorrelazione **-10%** (corretta per il delta) e non predicono il flusso della settimana dopo | §5.1 |
| 17 | **Conta comprare put, non venderle**: OIB Puts positivo 0,1342 (p 0,0064), negativo 0,0639 (p 0,14); OIB Calls positivo -0,0852 (p 0,08), negativo 0,0213 (p 0,49) | §5.2, Tab. 9 |
| 18 | **Chi scambia all'ISE**: taglia media in dollari degli scambi su indici **un sesto** di quella delle SPX (OPRA 2006-2015); i clienti fanno il **90%** del volume di apertura delle opzioni su indici ISE, contro il **69%** al Cboe. Entrambi **non tabulati** | §5.2 |
| 19 | **Solo gli ordini piccoli**: IRF a 10 settimane del rendimento S&P 500 per le put dei clienti fino a 100 contratti **0,56 (0,25)**; put dei clienti piu' grandi e delle firm 0,13 (0,25); call piccole 0,31 (0,26), altre -0,12 (0,27) | §5.2, IA Tab. A2 |
| 20 | Put piu' liquide delle call: spread medio 11,15% contro 12,32%; nelle settimane di VIX alto 9,44% contro 11,86%; volume settimanale (OptionMetrics) 287.787 put contro 204.085 call. Nelle settimane con annunci lo spread delle put scende (11,57% -> 10,96%, t 2,31), quello delle call no (t -0,18). Taglia media degli scambi: put 22.419 dollari, call 27.720 (non tabulato) | §5.3, Tab. 10 |
| 21 | **Fuori campione**: R2OS fra **0,41% e 2,20%** a seconda dell'anno d'inizio (2010-2014), Clark e West fra 1,48 e 1,94: significativo al 5% a una coda per gli inizi 2010-2012, **solo al 10% per il 2013 e il 2014**. Con previsioni non negative R2OS fra 1,54% e 2,65%, significativo al 5% tranne il 2014 (10%) | IA §A, Tab. A1 |
| 22 | Gli autori, nell'Internet Appendix: "non sosteniamo che lo sbilancio delle put si possa usare per decidere quando entrare o uscire dall'indice" | IA §A |

**La risposta alla domanda di partenza** [§6]: si', il flusso delle put su indici dell'ISE predice
l'S&P 500 a una settimana, nel verso opposto a quello che ci si aspetterebbe da un flusso informato, e
il flusso degli strumenti sull'S&P 500 stesso no.

## Il Perche'

**Il perche' degli autori: la domanda di assicurazione** [§5.2]. I market maker vendono put agli
investitori esterni. Quando l'incertezza sale, chi e' piu' avverso al rischio compra protezione, e il
premio per il rischio richiesto dal mercato sale con lei; quindi piu' put comprate oggi vogliono dire
rendimenti attesi piu' alti dopo. Nei periodi tranquilli si vendono put per incassare il premio, e
questo non predice niente. Lo sostengono, per gli autori: il legame solo con il VIX alto (risultati 11 e
12), il solo lato degli acquisti di put (17), il legame con il VIX dei residui (9), il dopo-annunci (15),
le put piu' liquide nei periodi incerti (20), e il fatto che lo guidino gli ordini piccoli (19). I
modelli di riferimento sono Grossman e Zhou (1996) e Bates (2008): chi teme meno il crollo assicura chi
lo teme di piu'.

**Perche' l'ISE e non l'SPX** [§1, §5.2]: le SPX sono soprattutto istituzionali (scambi grandi,
strategie a piu' gambe, costi bassi); le opzioni su indici dell'ISE sono soprattutto dei piccoli, che
sono piu' avversi al rischio e quindi comprano protezione (Haddad e Muir).

**Le alternative scartate**:

- **Sentiment** [§1, §3.2]: le misure di sentiment nel VAR non cambiano niente (non tabulato).
- **Market maker informati** [§5.1]: se lo fossero, l'effetto sarebbe piu' forte **prima** degli annunci,
  e i loro prezzi anticiperebbero il flusso; nessuna delle due cose.
- **Copertura di posizioni in altri strumenti** [§4]: il risultato regge con i flussi di E-mini, titoli,
  SPY e opzioni SPX.
- **Vincoli degli intermediari** (Chen, Joslin, Ni 2018): li' l'acquisto di put molto fuori dal denaro
  sull'S&P 500 predice rendimenti **piu' bassi** a un mese; qui il verso e' opposto e l'orizzonte e' la
  settimana: "fenomeni diversi, complementari" [§1].

**Cosa lo ucciderebbe**, dedotto dal perche' (gli autori non lo dicono): un effetto uguale nei periodi
di VIX basso e alto; un effetto portato dalle vendite di put invece che dagli acquisti; un effetto
portato dagli ordini grandi o dalle firm; un effetto piu' forte prima degli annunci che dopo.

## Cosa Non Regge O Non Si Dice

1. **Non e' "il Nasdaq-100 predice l'S&P 500".** Il flusso e' **RUT e NDX insieme** fino all'aprile 2013,
   poi solo NDX [§2, nota 11]. Nessuna tabella stima NDX da solo, ne' un salto nel 2013. I volumi di RUT
   sono piu' grandi di quelli NDX (risultato 2), quindi nei primi sette anni il segnale e', con ogni
   probabilita', soprattutto Russell (deduzione nostra: lo sbilancio somma i contratti, e le medie di NDX
   sono su tutte le 615 settimane, non solo sulle 382 con RUT). Che i risultati restino "simili" con il Nasdaq-100 come bersaglio e' **non tabulato** [§2,
   punto 5].
2. **Il riassunto di terzi usato nella fase 1 non corrisponde a questa versione.** La ricerca
   [`2026-11-09-opzioni-fra-indici`](../../ricerche/2026-11-09-opzioni-fra-indici.md) diceva, da CXO e
   da una versione del 2016, "ISE 250, Russell 2000, Nasdaq 100 e S&P 400". Qui gli indici sono **RUT e
   NDX**; altre opzioni su indici (Russell 1000 e quello che il paper chiama "S&P Mid Cap 600", nome che
   non esiste: l'S&P MidCap e' il 400, lo SmallCap il 600) "non cambiano quasi niente", non tabulato
   [nota 11]. Il controllo del flusso di **ES e di SPY c'e'** (Tab. 6, Tab. 7).
3. **"Robusto" ma solo con il VIX alto.** L'abstract dice "positively and robustly"; la Tab. 4 dice che
   fuori dal quartile alto del VIX la somma dei ritardi non e' significativa (0,0235, p 0,41; 0,0138, p
   0,64 in Tab. 6), e il §3.3 scrive "only in periods of high perceived uncertainty". Ma lo stesso §3.3
   dice che l'IRF mostra un effetto "both during and outside of the high VIX periods": l'IRF di OIB Puts
   da solo e' 0,55 (0,27) in Tab. 4 (t circa 2,0, nostro conto; in grassetto, cioe' significativa per gli
   autori) e 0,50 (0,26) in Tab. 6 (t circa 1,9, **non** in grassetto). L'introduzione dice "piu' forte",
   il §3.3 "solo": **le due letture non coincidono**.
4. **Il quartile alto del VIX e' calcolato sull'intero campione** nelle prove dentro il campione
   (Tab. 1, 4, 6, 7, 10): sa gia' quali sono le settimane piu' agitate dei 12 anni. Il paper non lo scrive
   in chiaro; lo deduciamo dal conteggio, 156 settimane su 615, cioe' il 25%, e dal contrasto con la nota 1
   dell'IA, che solo per il fuori campione precisa "dal 1990 al giorno t". Il fuori campione regge, ma piu'
   debole (risultato 21). Anche la winsorizzazione all'1-99% e' con ogni probabilita' sul campione intero:
   il paper non dice su quale finestra.
5. **Il fuori campione e' di specifica, non di scelta.** Le regole (put, tre ritardi, interazione con il
   VIX alto) sono state scelte guardando tutti i 12 anni; si ristima solo i coefficienti. Il risultato e'
   significativo **al 10% a una coda** per gli inizi 2013 e 2014. Gli autori lo confrontano, con ragione,
   con predittori del mercato che di solito falliscono fuori campione (Goyal e Welch).
6. **Molte prove non tabulate o "disponibili su richiesta"**: chiusure, altri indici come bersaglio,
   altre opzioni ISE, sentiment, incertezza politica, TED, ribassi estremi, spread di volatilita',
   premio di varianza, SKEW, periodo dopo la crisi 2010-2017 [nota 17], taglia degli scambi, quota dei
   clienti. Contando le tabelle e queste, sono **alcune decine di specifiche**, senza correzione per prove
   multiple. E le specifiche non sono tutte uguali: nella Tab. 9 tasso e spread sono **depurati da un trend
   quadratico** invece che in differenze prime come in tutte le altre tabelle, senza che il testo lo dica.
7. **Annunci macro: la differenza non e' provata.** Il testo dice che il flusso delle put predice "solo
   nelle settimane con annuncio" (0,1067 contro 0,0636), ma nessuna tabella prova che le due somme siano
   **diverse fra loro**: le 191 settimane senza annuncio sono meno di meta' delle 424 con, e la potenza
   e' minore. Nel pannello B la stima **prima** dell'annuncio (0,1154) e' piu' alta di quella delle altre
   settimane (0,0837), mentre il testo le dice "molto simili". La conclusione [§6] scrive poi che la
   predicibilita' e' "piu' forte intorno" agli annunci: e' un'affermazione piu' forte della tabella.
8. **Quintili, testo e Tab. 9.** Il §3.1 dice che primo e ultimo quintile hanno rendimenti a due settimane
   "simili", quindi l'effetto non verrebbe "solo" dagli acquisti o dalle vendite; il §5.2 (Tab. 9) dice che
   conta solo l'OIB positivo. **La Fig. 2 non dice "simili"**: -0,6% contro +1,2% a due settimane
   (risultato 6). A una settimana, l'orizzonte della Tab. 2, i primi tre quintili sono tutti intorno a
   -0,1% e quasi tutto l'effetto sta nel quintile piu' alto, che va nel verso della Tab. 9; a due settimane
   contribuiscono entrambe le code. La frase del §3.1 regge solo come distanza simmetrica dalla media.
9. **Due errori nella Tab. 5**: l'ultimo coefficiente comune (-0,0299) ha z -0,00 e p 0,5485, che non
   stanno insieme (con p 0,55 la z sarebbe circa -0,60); e il primo ritardo del rendimento (-0,0713) ha z
   -0,97 e p 0,4238, che non stanno insieme nemmeno loro (z 0,97 da' p 0,33; p 0,42 vuole z 0,80). Refusi
   di questa versione; non toccano il risultato, che sta nelle righe di OIB Puts.
10. **La Tab. 7 ha una variabile non descritta** (OIB delle opzioni su ETF all'ISE) e una data doppia
    ("December 27, 29, 2017"). Il testo dice che con l'SPX "la grandezza resta invariata", ma l'IRF di OIB
    Puts ISE passa da 0,50-0,55 a 0,89: cresce. Nella stessa tabella lo sbilancio dei titoli NYSE ha IRF
    0,70 (0,27) in grassetto, che il testo non commenta e che contrasta con "i flussi non-opzioni non
    predicono" (nella Tab. 6 era 0,54, non significativa); e il grassetto su OIB Calls SPX, colonna spread
    a termine, -0,49 (1,30), e' evidentemente un errore di impaginazione (t 0,4).
11. **"Annualizzato 2,9%"** torna con 0,55% in 10 settimane moltiplicato per 5,2 (nostro conto: il paper
    non dice come lo calcola); cosi' supporrebbe uno shock di una deviazione standard ogni 10 settimane, e
    lo 0,55% e' gia' al netto della risposta negativa della prima settimana (risultato 8). Non e' il rendimento di una strategia, e **nessuna strategia,
    nessun costo** sono provati; gli autori lo escludono loro stessi (risultato 22).
12. **I clienti come "piccoli investitori"** e' dedotto dalla taglia (un sesto delle SPX, il 90% di
    clienti) con prove non tabulate; l'ISE divide i clienti per taglia, non per identita'.
13. **Settimana come unita'**: una osservazione per settimana, 615 in tutto, Newey-West. Va nel senso del
   nostro metodo (significativita' per periodo, non per operazione), ma il confronto e' con zero e con la
   media storica, **non con un modello nullo** nel nostro senso (la stessa regola sul mercato rimescolato).
14. **Il campione finisce nel 2017**: prima degli 0DTE quotidiani (2022) e del retail di massa sulle
   opzioni del 2020-2021. Le opzioni RUT non sono piu' all'ISE dal 2013. Se il flusso dei piccoli su NDX
   si comporti ancora cosi' non si sa.
15. **Conflitti**: nessuno dichiarato. Muravyev e' anche autore di lavori sul flusso delle opzioni citati
    qui (2013, 2016).
16. **"Nessun effetto sul rendimento contemporaneo"** [§4] vale per la risposta all'interazione con il VIX
    alto nella Fig. A1 (parte da circa -0,1), ma nel VAR di base (Fig. 3) e in quello della Fig. 4 la
    risposta a OIB Puts nella settimana dello shock e' **negativa**, circa -0,2, con la banda della Fig. 3
    tutta sotto zero: con OIB Puts messo prima del rendimento nell'ordine di Cholesky, la correlazione
    negativa dei residui (-0,09, risultato 9) finisce nella risposta. Il testo non lo dice.

## Cosa Vale Per Le Due Ricerche

**Per [`2026-11-09-opzioni-fra-indici`](../../ricerche/2026-11-09-opzioni-fra-indici.md)** (il flusso
delle opzioni di indici diversi dall'S&P 500 predice l'S&P 500?):

- **Si', ma con quattro limiti.** (a) Il flusso e' RUT e NDX insieme fino al 2013, NDX da solo dopo,
  senza una stima separata (punto 1); (b) e' il flusso delle **put dei piccoli**, non la gamma, non i
  muri, non lo zero gamma; (c) a **settimane**, non dentro la giornata; (d) **soprattutto con il VIX
  alto**. Il controllo del flusso di ES e di SPY c'e' ed e' verificato (Tab. 6), e il flusso di ES non
  predice la settimana dopo.
- Il paper **non** prova che le opzioni di SPX informino il Nasdaq-100: le SPX non predicono nemmeno
  l'S&P 500 (risultato 14). La direzione che prova e' **NDX e RUT -> S&P 500**, e la legge come premio
  di rischio comune al mercato, non come legame fra due indici (le correlazioni settimanali fra gli
  indici sono 0,85-0,92, risultato 3).
- Quindi per lo studio globale unico di NQ ed ES vale come **contesto**: un indice largo e uno tecnologico
  rispondono allo stesso premio di rischio. Non e' una prova che i livelli di un indice servano a leggere
  l'altro.

**Per [`2026-11-09-flusso-delle-opzioni-per-gruppo`](../../ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md)**
(quali gruppi sono informati, a che orizzonte):

- **Nessun gruppo e' "informato" nel senso di sapere dove va l'indice.** Predicono solo i **clienti
  piccoli** (fino a 100 contratti) sulle **put**, **al contrario** (comprano protezione, l'indice poi
  sale), a **una-due settimane** e cumulato a dieci, e gli autori lo leggono come domanda d'assicurazione,
  non come informazione (risultati 4, 11, 16, 19). Clienti grandi e firm insieme: niente (0,13, errore
  0,25). Market maker: non informati (risultato 16).
- **Sulle opzioni SPX niente** (risultato 14), come in Pan e Poteshman (SPX al giorno dopo) e in Cao e
  altri (SPX alla settimana). Il DEX per gruppo di X-Ray e' di SPX: **il dato del riquadro e' proprio
  quello che qui non predice**. Il verso "piu' put comprate, indice piu' alto" e' lo stesso dei
  coefficienti significativi di Pan e Poteshman su NDX e OEX (lettura nostra, dalla loro analisi; il
  testo di questo paper riassume Pan e Poteshman come "nessuna prova" su OEX, SPX e NDX [§1]).
- **Come si misura** (lo conferma un terzo paper): sbilancio fra aperture in acquisto e in vendita diviso
  il volume di apertura, contro la sua storia; i clienti delle opzioni su indici ISE sono di solito
  venditori netti (risultato 1), quindi un segno da solo non dice niente.
- Corregge la risposta breve della ricerca, che diceva "i clienti nel complesso informativi sulle put a
  una settimana": sono **i piccoli**, ed e' **assicurazione**, non informazione.

## Cosa Si Puo' Rifare Su NQ Con ATAS, E Cosa No

**Non si puo' rifare il finding.** Servono gli sbilanci di apertura per lato del market maker sulle
opzioni NDX (ISE Open/Close o Cboe Open-Close, a pagamento), e anni di settimane. ATAS da' la catena CME
delle opzioni su NQ senza dire chi ha comprato, ne' se apriva o chiudeva, e senza storia; X-Ray da' i
gruppi, ma su SPX e solo da ottobre 2026. Su NQ il flusso delle opzioni resta **contesto** (CLAUDE.md,
decisione 2).

**Quanto servirebbe registrare in avanti** (nostro calcolo, non del paper): la somma dei ritardi di OIB
Puts ha Wald 14,26 su 615 settimane, cioe' una z di circa 3,8. Se la z cresce con la radice del numero
di settimane, per arrivare a 2 ne servono circa **170 (piu' di tre anni)**, e un anno (52 settimane)
darebbe z intorno a 1,1. Sul dato che abbiamo (SPX) il paper dice gia' che non c'e' niente.

**Si puo' rifare il controllo negativo, con le sole barre.** Il flusso aggressivo dei future (sbilancio
settimanale degli E-mini con la regola del tick) **non predice** la settimana dopo e muove solo la
settimana stessa (risultato 13). Su NQ, con il lato dell'aggressore di ATAS: sbilancio settimanale
(acquisti meno vendite aggressive, diviso il volume) contro il rendimento della settimana stessa e di
quella dopo. Il tape di NQ in archivio parte dal 2023 ([i-dati-da-atas](../../../metodo/i-dati-da-atas.md));
dentro il campione (fino al 2024) sono circa **100 settimane**, poche per dire "niente" con forza.
Ogni sguardo al 2025-2026 consuma il fuori campione di NQ e si fa solo con `valida --fuori`.

**Si puo' usare come contesto settimanale nello studio globale**: con il VIX nel suo quartile alto, una
settimana di acquisti di protezione e' stata seguita in media da rendimenti piu' alti, non piu' bassi
(2006-2017). E' una lettura del regime, non un segnale; il paper usa solo il VIX (estenderlo al VXN
sarebbe un'ipotesi nostra); e l'acquisto di put dei piccoli su NDX noi non lo vediamo.

## Cosa Ne Prende Il Framework

- **Nessuna regola operativa**: orizzonte settimanale, dato che non abbiamo, nessuna strategia e nessun
  costo nel paper, e gli autori stessi escludono l'uso per entrare e uscire.
- **Il disegno di prova**: una osservazione per periodo; sbilancio normalizzato e winsorizzato;
  condizione di regime (VIX alto) **calcolata solo sul passato**, come nel loro fuori campione e non come
  nelle loro tabelle; fuori campione ricorsivo con Clark e West come seconda misura accanto al nostro
  fuori campione unico.
- **Il controllo che separa flusso da prezzo**: mettere nello stesso sistema il flusso dei future, perche'
  quello muove la settimana stessa e non dopo; un flusso "predittivo" va provato oltre a quello.
- **Per il riquadro di X-Ray**: un terzo paper che dice che il flusso delle opzioni **SPX** non anticipa
  l'indice; dove qualcosa si vede, sono le **put** dei **piccoli** su altri indici, **al contrario**, a
  settimane, con il VIX alto.

## Dubbi Di Lettura Per Il Verificatore

Risolti nella verifica (9 novembre 2026), sulle pagine rese come immagini:

- **Le figure 2-5 e A1**: viste. Il +0,55% a 10 settimane torna con la Fig. 3; i quintili della Fig. 2
  **non** confermano il "simili" del testo (risultato 6, punto 8); le figure 4, 5 e A1 aggiungono quanto
  scritto nei risultati 8, 11, 12, 13 e nel punto 16.
- **Le formule**: eq. 1, 4 e 5 confermate sull'immagine. Nella eq. 5 l'interazione e' davvero allo stesso
  ritardo: OIB Calls_{t-j}(1 - D_{t-j}) e OIB Calls_{t-j} D_{t-j}, e lo stesso per le put, piu' tre
  ritardi di D. La regressione fuori campione (Tab. A1) ha ritardi da 0 a 2 di rendimento, OIB Puts,
  OIB Puts x D e D stessa (non il VIX).
- **Tab. 2**: le righe sono R_{t-1..3}, OIB Calls_{t-1..3}, OIB Puts_{t-1..3}, VIX_{t-1..3}, come
  attribuite. Confermato.
- **Tab. 5**: le righe dei due regimi sono OIB Puts_{t-1..3} e sigma_1, sigma_2; i comuni sono R, OIB
  Calls e VIX, come attribuiti. Confermato; in piu' un secondo refuso (punto 9).
- **Chi c'e' nell'OIB principale**: resta una deduzione. Il §2 e l'introduzione parlano di "customer";
  la Tab. A2 divide le due parti per il volume "of customer and firm trades", e le due parti insieme
  danno lo sbilancio di clienti e firm. Il paper non dice in chiaro che lo sbilancio principale sia
  quello.
- **Il grassetto**: visto. Significative per gli autori l'IRF di OIB Puts in Tab. 4 (0,55) e di OIB Puts
  Small in Tab. A2 (0,56); non quella di OIB Puts in Tab. 6 (0,50).

## La Verifica

**9 novembre 2026**, fase 4, da un agente diverso da quello che ha scritto l'analisi. Fonte: il PDF,
sha256 confermato (`fa1b5c76...8f9998`), testo con `pdftotext -layout` e `-table`, e le pagine delle
formule, delle tabelle 2, 4-7, A1, A2 e delle figure 2-5 e A1 rese come immagini con pypdfium2 nella
scratchpad. Nessuna istruzione trovata nel PDF.

**Circa 160 affermazioni controllate**: ogni numero delle tabelle 1-10, A1, A2 citato qui, le note 9, 11,
16, 17 e 1 dell'IA, le citazioni di sezione, i "gli autori dicono", i conti nostri (0,0366 x 5,93 =
0,217; Wald 14,26 -> z 3,78 -> circa 172 settimane per z 2, z 1,1 in 52; t delle IRF; durate 1/(1-p)
51,8 e 18,8 settimane; rapporto 3,7434/1,3825 = 2,71) e i fatti del repository usati qui (il tape di NQ dal
2023, la ricerca e il riassunto di CXO). **Confermate circa 145**, fra cui tutte quelle segnalate
dall'orchestratore: RUT e NDX sommati e solo NDX dal 22 aprile 2013 (§2, nota 11); 615 settimane
2006-2017; +0,22% per deviazione standard (§3.1) e +0,55% in 10 settimane (§3.2, Fig. 3); 0,0235 p 0,41
contro 0,2870 p 0,0000 (Tab. 4); solo l'OIB positivo delle put (Tab. 9); clienti fino a 100 contratti,
0,56 (0,25) (IA Tab. A2); SPX 0,02 (0,30) (Tab. 7); E-mini, NYSE e SPY (Tab. 6); fuori campione al 10% per
2013 e 2014 (Tab. A1). **Corrette o precisate 15, tolte 0, aggiunte 2** (punto 16 e i valori delle figure).

Le correzioni:

1. **Fig. 2, quintili** (risultato 6, punto 8): il "simili" del testo e' smentito dalla figura (-0,6%
   contro +1,2% a due settimane); a una settimana l'effetto sta quasi tutto nel quintile piu' alto, nel
   verso della Tab. 9, non contro. E' la correzione piu' grave: l'analisi riportava come finding una
   frase del testo che il grafico contraddice, e ci costruiva sopra un conflitto con la Tab. 9.
2. **Fig. 3** (risultato 8): la risposta parte da circa -0,22 nella settimana dello shock, con la banda
   sotto zero; lo 0,55% e' un netto. Da qui anche il punto 16, nuovo (il "nessun effetto contemporaneo"
   del §4 non vale per il VAR di base).
3. **Fig. 4 e A1** (risultato 11): la risposta a OIB Calls x VIX alto e' contemporanea.
4. **Fig. 5** (risultato 12): il regime alto e' soprattutto 2007-2011.
5. **Tab. 7** (risultato 14, punto 10): l'IRF dei titoli NYSE, 0,70 (0,27), e' in grassetto e il testo
   tace; grassetto sbagliato su OIB Calls SPX, spread a termine.
6. **Fig. A1** (risultato 13): la risposta all'E-mini e' contemporanea, confermato; quella ai titoli
   NYSE cresce piano, non significativa.
7. **Tab. 5** (punto 9): un secondo refuso, R_{t-1} con z -0,97 e p 0,4238.
8. **Tab. 4 e 6, grassetto** (punto 3): 0,55 e' significativa per gli autori, 0,50 no.
9. **Il quartile del VIX sull'intero campione** e la finestra della winsorizzazione (punto 4): deduzioni,
   non scritte nel paper; dette come tali.
10. **Il 2,9% annuo** (punto 11): il moltiplicatore 5,2 e' nostro, non del paper.
11. **"Soprattutto Russell"** (punto 1): deduzione dai volumi medi, detta come tale.
12. **Tab. 9** (punto 6): tasso e spread depurati da un trend quadratico, non in differenze prime.
13. **Pagine** (intestazione): 51 di articolo e 8 di Internet Appendix, non 50 e 9.
14. **Pan e Poteshman** (Cosa vale): contrarian su NDX e anche su OEX, e il riassunto che ne da' questo
    paper ("nessuna prova").
15. **I dubbi di lettura**: formule (eq. 1, 4, 5), righe delle Tab. 2 e 5, grassetto, risolti
    sull'immagine; resta una deduzione chi sta nello sbilancio principale (clienti e firm).

**Resta incerto**: chi c'e' nello sbilancio principale (il paper dice "customer", la Tab. A2 fa pensare a
clienti e firm); se la versione pubblicata (Management Science 2021) cambi i numeri, perche' non l'abbiamo
letta; i valori presi dai grafici sono approssimati a qualche centesimo.
