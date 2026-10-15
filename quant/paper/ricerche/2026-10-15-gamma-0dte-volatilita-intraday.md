# Ricerca: Effetto della gamma dei dealer e delle opzioni 0DTE sulla volatilita' intraday dei future sugli indici azionari · 15 ottobre 2026

**Filone**: opzioni-gamma · **Chiesta da**: utente, attraverso lo studio dei paper (`/studio-paper`),
fase 1 · **Quanti**: 2, con PDF libero e **non gia' presenti** nell'[indice](../indice-dei-paper.md)

**Le parole**: la **gamma dei dealer** e' quanto cambia la copertura (il delta) dei market maker
delle opzioni quando il prezzo si muove: se e' positiva si coprono comprando i ribassi e vendendo
i rialzi (smorzano), se e' negativa fanno il contrario (amplificano). Gli **0DTE** (*zero days to
expiration*) sono le opzioni che scadono il giorno stesso. Un **dealer dinamico** (*dynamic
hedger*) e' chi tiene costante il delta del proprio portafoglio.

**Ricerche fatte** (15 ottobre 2026):

- ricerca web: `0DTE options dealer gamma intraday volatility S&P 500 futures paper`
- ricerca web: `arXiv dealer gamma exposure intraday volatility index futures`
- ricerca web: `"zero-days-to-expiry" options market maker hedging intraday volatility working paper pdf`
- ricerca web: `SSRN "gamma" "0DTE" "intraday volatility" hedging flows market maker inventory`
- ricerca web: `option market maker gamma imbalance intraday return reversal index futures paper "net gamma" Journal`
- ricerca web: `"gamma" hedging "E-mini" futures intraday volatility options market makers empirical paper SSRN`
- ricerca web: `"0DTE" paper "realized volatility" S&P 500 "market makers" 2025 SSRN abstract new evidence intraday`
- ricerca web: NBER, BIS, Federal Reserve, Bank of Canada, KOSPI 200, Eurex sullo stesso tema (nessun paper nuovo)
- arXiv (API): `all:0DTE`, `"gamma exposure"`, `"zero-day" AND options`, `"gamma squeeze"`, `"hedging flows"`,
  `"market maker" AND gamma AND volatility`, `"option expiration" AND intraday`
- OpenAlex (API): `0DTE options volatility`, `zero-day-to-expiry options`, `dealer gamma exposure volatility`,
  `option market makers gamma hedging intraday volatility`, `gamma imbalance intraday momentum`,
  `intraday momentum gamma hedging options market makers`, `gamma exposure intraday reversal futures options`,
  `weekly options expiration volatility underlying`, `expiry day volatility Nifty weekly options`
- le referenze di VAPG (gia' letto), per i paper citati sul tema
- le pagine degli autori: Vilkov (rimanda solo a SSRN), Doshi (Bauer, Houston: PDF libero)

**SSRN** risponde 403 alle richieste automatiche anche oggi (provati 5 `abstract_id`): l'esistenza dei
paper SSRN qui sotto e' verificata su Crossref e OpenAlex, non sulla pagina SSRN.

## I Candidati

**tipo**: R rivista con revisione fra pari, P preprint o working paper. **Gia' nell'indice** e quindi
non ricercati: Dim, Eraker, Vilkov (SSRN 4692190); Brogaard, Han, Won (SSRN 4426358); Adams e altri
2025 (SSRN 5641974); Ni, Pearson, Poteshman, White 2021; Anderegg, Ulmann, Sornette 2022; Barbon,
Beckmeyer, Buraschi, Moerke (SSRN 3925725); Maurer 2026; e i sette letti (VAPG, BDLM, BB, GJ, BTW,
GPP, SM).

| chiave | titolo | autori, anno | id | tipo | pdf libero | perche' si', perche' no | stato |
|---|---|---|---|---|---|---|---|
| buis-2024-gamma-positioning-and-market-quality | *Gamma positioning and market quality* | Buis (NatWest Markets, VU Amsterdam), Pieterse-Bloem (Rabobank, Erasmus), Verschoor, Zwinkels (VU Amsterdam, Tinbergen), 2024 | doi [10.1016/j.jedc.2024.104880](https://doi.org/10.1016/j.jedc.2024.104880); Journal of Economic Dynamics and Control 164, 104880 | R (ricevuto 23 dicembre 2022, rivisto 2 febbraio 2024, accettato 14 maggio 2024), licenza CC BY | si': [pure.eur.nl](https://pure.eur.nl/ws/files/149733755/1-s2.0-S0165188924000721-main.pdf), versione dell'editore, 21 pagine, raggiunto il 15 ottobre 2026 | si': revisionato, PDF libero, e **il meccanismo e' l'oggetto del paper**: dall'abstract, gamma netta positiva dei dealer dinamici abbassa la volatilita', negativa la alza e rende il mercato piu' fragile, e l'effetto pesa di piu' dove la liquidita' e' bassa. No: e' una **simulazione** (modello a *zero intelligence*, agenti che inviano ordini a caso), non dati di mercato | scaricato, sha256 `aeb80faa4e7343213d423aa6133d06446be951dbcbf425278e0f2457a33f5eea` |
| doshi-2025-risky-intraday-order-flow-and-option-liquidity | *Risky Intraday Order Flow and Option Liquidity* (su SSRN: *... and Equity Option Liquidity*) | Hitesh Doshi (Bauer, Univ. of Houston), Paola Pederzoli, Saim Ayberk Sert, 2025 | SSRN 5006194 (versione 2024); versione letta in prima pagina: 23 maggio 2025 | P | si': [bauer.uh.edu](https://www.bauer.uh.edu/hdoshi/docs/DPS_May_2025.pdf), 70 pagine, raggiunto il 15 ottobre 2026. Il PDF e' anonimo: gli autori vengono dalla pagina di Doshi e dal record SSRN | si': **dati reali** (tutti i trade OPRA, opzioni SPX e sui titoli dell'S&P 500, 2004-2021, compresi gli 0DTE) e mette alla prova **l'ipotesi su cui poggia ogni GEX**: dall'abstract, chi fornisce liquidita' sulle opzioni brevissime gestisce l'inventario **piu' abbinando gli ordini che coprendosi col delta**. Se regge, la copertura che arriva sul future e' minore di quella che la gamma fa supporre. No: misura i costi delle opzioni, **non la volatilita' del future**; preprint | scaricato, sha256 `714182589aba9b2f3fe85d1672fbd6031d46476d31e9e5aba909e7589428a5a9` |
| adams-2024-market-for-0-days-to-expiration | *The Market for 0-Days-to-Expiration: The Role of Liquidity Providers in Volatility Attenuation* | Adams (Univ. of Pennsylvania), Fontaine (Bank of Canada), Ornthanalai (Univ. of Toronto), 2024 | SSRN 4881008 (Crossref) | P | no: solo SSRN; non trovato sul sito della Bank of Canada ne' sulle pagine degli autori | il piu' vicino alla domanda (giorni con e senza 0DTE prima di maggio 2022, citato da VAPG come "well-identified"), ma e' **la versione precedente di Adams e altri 2025**, gia' nell'indice come trovato. Da scaricare col browser se si vuole il test diretto | scartato |
| yuan-2022-delta-hedging-demand-intraday-momentum-china | *Delta-Hedging Demand and Intraday Momentum: Evidence from China* | Yuan, Li (Xi'an Jiaotong), 2022 | doi [10.1016/j.physa.2022.127508](https://doi.org/10.1016/j.physa.2022.127508); SSRN 4025948 | R, Physica A 600 | no: Physica A a pagamento, SSRN dietro Cloudflare | sull'indice e intraday, ma misura il momento, non la volatilita', in un mercato diverso; BDLM (gia' letto) copre la stessa domanda sui future USA | scartato |
| zheng-2024-intraday-reversal-commodity-futures-options-china | *Is there an intraday reversal effect in commodity futures and options? Evidence from the Chinese market* | Zheng, Luo, 2024 | doi [10.1016/j.pacfin.2024.102534](https://doi.org/10.1016/j.pacfin.2024.102534); SSRN 4688712 | R, Pacific-Basin Finance Journal 88 | no | future sulle materie prime, non sugli indici; il legame gamma negativa e inversione e' noto solo da un blog | scartato |
| wilkens-2026-european-zero-day-options | *Here Today, Gone Today: First Evidence on European Zero-Day Options* | Wilkens, 2026 | SSRN 7066558 e 7094758 | P | no: solo SSRN | 0DTE sull'Euro Stoxx 50 (Eurex); l'abstract non e' leggibile, e la domanda sulla volatilita' non e' confermata | scartato |
| elms-2026-from-pinning-to-amplification | *From Pinning to Amplification: Evidence of a Regime Shift in S&P 500 Options Expiration Dynamics, 2016-2025* | Elms, 2026 | SSRN 6564078 | P | no: solo SSRN | tocca la domanda (range piu' ampi con molto open interest al denaro, da un blog), ma autore senza affiliazione nota e risultato letto solo in un riassunto | scartato |
| singh-2026-inherited-state-new-flow-0dte | *Inherited State, New Flow, and Intraday Volatility in SPX 0DTE Options* | Singh, 2026 | SSRN 7350465 | P | no: solo SSRN | titolo esattamente sul tema, ma autore senza affiliazione e nessun abstract leggibile | scartato |
| ladia-2026-opening-range-reversal-dow-futures | *Opening-Range Reversal in Dow Jones Futures: Evidence Consistent with 0DTE Hedging Pressure* | Ladia, 2026 | SSRN 7124578 | P | no: solo SSRN | sui future (Dow), ma "consistent with": la gamma non e' misurata, a giudicare dal titolo; autore senza affiliazione | scartato |
| popovici-2026-gamma-exposure-walls | *Do Gamma-Exposure Walls Exhibit Strike-Local Effects? A Pre-Registered, Control-Matched Test of Strike-Local Dealer-Gamma Effects in SPY* | Popovici, 2026 | SSRN 7082418 | P | no: solo SSRN | non e' la domanda (i muri, non la volatilita'), ma **e' l'unico test trovato dei muri che ATAS disegna**, che l'indice dice mancare: da cercare in una ricerca sui livelli | scartato, da tenere |
| lipson-2023-real-cost-of-free-trades | *A Real Cost of Free Trades: Retail Option Trading Increases the Volatility of Underlying Securities* | Lipson, Tomio, Zhang, 2023 | SSRN 4383463 | P | no: solo SSRN | singoli titoli, non indici | scartato |
| odonovan-2023-option-market-maker-hedging-stock-liquidity | *Option Market Maker Hedging and Stock Market Liquidity* | O'Donovan, Yu, Zhang, 2023 | SSRN 4567604 | P | no: la copia SMU e' dietro un controllo anti-bot (Incapsula) | liquidita' dei singoli titoli, non volatilita' degli indici | scartato |
| dai-2025-beta-dependent-gamma-feedback | *Beta-Dependent Gamma Feedback and Endogenous Volatility Amplification in Option Markets* | Dai, 2025 | arXiv [2511.22766](https://arxiv.org/abs/2511.22766) | P | si' | solo teoria, sui singoli titoli e sulle *gamma squeeze*; Buis e altri danno lo stesso meccanismo con una rivista | scartato |
| huang-2023-derivatives-and-market-illiquidity | *Derivatives and Market (Il)liquidity* | Huang, Yueshen, Zhang, 2023 | doi [10.1017/S0022109023000224](https://doi.org/10.1017/S0022109023000224) | R, JFQA | si' | teoria della liquidita' del sottostante, non della volatilita' intraday | scartato |
| egebjerg-2026-deep-hedging-0dte | *Deep hedging 0DTE options* | Egebjerg, 2026 | doi [10.1016/j.jfs.2026.101535](https://doi.org/10.1016/j.jfs.2026.101535) | R, Journal of Financial Stability | si' (accesso aperto) | come coprire uno 0DTE, non che effetto ha la copertura sul mercato | scartato |
| todorov-2024-intraday-volatility-patterns-short-dated-options | *Intraday volatility patterns from short-dated options* | Todorov, Zhang, 2024 | doi [10.1016/j.jeconom.2024.105732](https://doi.org/10.1016/j.jeconom.2024.105732) | R, Journal of Econometrics | no | misura la volatilita' intraday dalle opzioni, non l'effetto della gamma | scartato |
| chhimwal-2025-multiple-index-derivative-expiry-india | *Effect of multiple index derivative expiry on volatility, volume, and connectedness: a tale of two stock indices in India* | Chhimwal, Pandey, Pandey, 2025 | doi [10.1007/s11147-025-09221-8](https://doi.org/10.1007/s11147-025-09221-8) | R, Review of Derivatives Research | no | scadenze multiple sugli indici indiani, senza la gamma | scartato |
| beckmeyer-2021-end-of-day-momentum-option-hedging | *End-of-Day Momentum in the Cross-Section and Option Hedging* | Beckmeyer, Moerke, 2021 | EFMA 2021 | P | si' (EFMA) | singoli titoli; e' il precursore di Barbon e altri (SSRN 3925725), gia' nell'indice | scartato |
| anonimo-2024-where-does-gamma-hedge-drive | *Where does gamma hedge drive the intraday market move?* | anonimo (sottomissione AFA), 2024 | afajof.org | P | si' | autori non dichiarati, singoli titoli | scartato |

Esclusi perche' non verificabili alla fonte: una tesi triennale di Princeton (*0DTE Options and
Intraday Volatility*, aprile 2026, consultabile solo in biblioteca) e un saggio di Fabio Iannucci
(maggio 2026) ospitato su uno spazio di archiviazione di terzi, senza sede ne' revisione.

## La Scelta

- **buis-2024-gamma-positioning-and-market-quality**: e' l'unico candidato nuovo, revisionato e con
  PDF libero che risponde al **perche'**. Dichiara il meccanismo (la copertura dei dealer dinamici
  con gamma positiva o negativa) e le condizioni in cui conta (quanta parte del mercato fanno i
  dealer, quanta liquidita' c'e'). Serve a scrivere "cosa lo ucciderebbe" nella scheda: su un
  mercato liquido come ES o NQ l'effetto potrebbe essere piccolo. Il limite va scritto: e' una
  simulazione, e un numero simulato non e' un finding di mercato.
- **doshi-2025-risky-intraday-order-flow-and-option-liquidity**: dati reali su SPX e 0DTE, periodo
  lungo, PDF libero. Mette alla prova **il passo che ogni GEX da' per scontato**: che la gamma dei
  market maker diventi davvero ordini sul sottostante. Se si coprono soprattutto abbinando gli
  ordini, la copertura che arriva sul future e' minore di quella che la gamma fa supporre. Con VAPG
  (il limite massimo dell'effetto) e BB/BDLM (gli effetti misurati) chiude il quadro.

**Perche' non gli altri.** I paper che misurano proprio la volatilita' intraday contro la gamma o gli
0DTE sull'S&P (Dim, Eraker e Vilkov; Brogaard, Han e Won; Adams e altri) **sono gia' nell'indice** e
stanno solo su SSRN. Adams, Fontaine e Ornthanalai 2024 e' la loro versione precedente. I nuovi
paper del 2026 che toccano la domanda (Singh, Elms, Ladia, Wilkens) sono preprint SSRN di autori
senza affiliazione nota o con l'abstract illeggibile. Tutti richiedono il browser e la persona per
la verifica di Cloudflare. Se si vuole il test diretto, il prossimo da scaricare col browser e'
**Adams e altri 2025 (SSRN 5641974)**, la versione piu' recente.

**Cosa nessuno dei due da'.** Nessuno misura la volatilita' intraday **del future** contro la gamma,
e nessuno usa NDX o NQ. Si puo' rifare con i dati di ATAS solo da quando ATAS registra la catena
(ES, dal 14 ottobre 2026): la storia della gamma non c'e'.

## La Sintesi

**15 ottobre 2026**, fase 5. Fonti: le due analisi verificate,
[BPVZ](../opzioni-gamma/buis-2024-gamma-positioning-and-market-quality/analisi.md) e
[DPS](../opzioni-gamma/doshi-2025-risky-intraday-order-flow-and-option-liquidity/analisi.md) (stato
`analizzato`, verificate il 15 ottobre), e le sette del filone lette il 14 ottobre (GJ, BB, BDLM, BTW,
GPP, VAPG, SM; vedi l'[indice](../indice-dei-paper.md#opzioni-gamma)). **Le sette sono `analizzato`
ma senza una verifica avversaria registrata** (nessuna sezione "La Verifica" nelle loro analisi): i
loro numeri valgono come li riporta l'analisi. Ogni numero qui sotto viene da un'analisi, con la
sezione del paper; dove la lettura e' nostra, e' scritto.

**Le correzioni delle verifiche, tenute qui.** (1) Il terzo autore di DPS e' **Saim Ayberk Sert**
(allineata la tabella dei candidati). (2) In BPVZ la frase "effetto grande solo con il dealer al
30-52% del flusso" della fase 3 era sbagliata: la volatilita' resta dentro il rumore fino al 18-29%
del flusso, ma l'autocorrelazione dei rendimenti brevi, le code giornaliere e lo spread si muovono di
molti errori standard **gia' al 18-19%**, e il paper non ha scenari sotto il 18%. L'estrapolazione a
NQ (copertura circa lo 0,4-0,5% del flusso) e' **nostra**, non del paper. (3) In DPS "la GEX e' un
tetto del flusso di copertura" e' **lettura nostra**: il paper non parla mai di GEX.

### I Finding

- **BPVZ** [§4.1-4.3, Tab. 2, 3, 4, A1, A2]: **simulazione**, nessun dato di mercato, rivista. Un dealer
  dinamico (chi tiene il delta a zero scambiando il sottostante) lungo di gamma (+80 straddle) porta
  la volatilita' delle variazioni a 0,579 di quella senza dealer senza notizie, 0,852 con notizie;
  corto (-80), a 1,789 e 1,243. L'**autocorrelazione** al primo ritardo delle variazioni del quarto
  d'ora (se una variazione tende a essere seguita da una dello stesso segno, positiva, o di segno
  opposto, negativa) passa da -0,252 senza dealer a **+0,127** a -80 e -0,345 a +80. Code giornaliere
  2,596 volte quelle senza dealer a -80, 0,617 a +80. Con il dealer al 18-19% del flusso la
  volatilita' e' dentro un errore standard, l'autocorrelazione no (0,298 e 1,409 del valore senza
  dealer). Gamma negativa: dopo un salto del valore il prezzo arriva presto e sfora; positiva: arriva
  tardi e resta incollato (Tab. 4, senza errori standard).
- **DPS** [§4.3-4.4, Tab. 3, 5, IA.4]: **dati reali**, opzioni SPX dal 2004 al 16 luglio 2021,
  preprint. Lo spread effettivo delle opzioni SPX brevissime lo spiega la volatilita' intraday del
  flusso degli ordini, `log(SD)`: il logaritmo della deviazione standard dei 78 flussi a 5 minuti della
  seduta, cioe' il **momento secondo** del flusso (quanto va avanti e indietro), non lo sbilancio
  della giornata (il **momento primo**). 0DTE call 0,020 (t 4,10), put 0,023 (t 6,19). La gamma
  dell'inventario dei market maker (GammaInv) perde quando entra `log(SD)` (call 0DTE da 0,522, t
  2,24, a -0,036, t -0,16). Gli autori ne deducono che chi da' liquidita' gestisce l'inventario
  **soprattutto abbinando gli ordini**, con la copertura in delta secondaria [§1, §7]. La prova e'
  **indiretta**: nessuno scambio di copertura osservato, misure in valore assoluto (nessun segno), e
  la copertura resta significativa a 1-6 giorni (DeltaOI call 0,049, t 2,58) e a 35-41 (GammaInv call
  0,130, t 3,56).

### Dove Convergono

1. **Il verso del meccanismo.** Gamma dei dealer positiva: meno volatilita', piu' inversione;
   negativa: piu' volatilita', piu' continuazione. BPVZ per costruzione (sopra). BB su dati reali:
   gamma del giorno prima piu' alta, rendimento assoluto piu' basso (indici: oltre 20 punti base per
   una deviazione standard, segno assunto) e autocorrelazione intraday piu' bassa da 5 a 60 minuti
   (-1,81, t -4,83, a 5 minuti) [BB Tab. III, VI]. BDLM: il momento dell'ultima mezz'ora sui future
   S&P c'e' solo con gamma netta negativa (6,63, t 4,78, contro 0,82, t 1,03) [BDLM Tab. 7]. VAPG:
   coefficiente della gamma negativo nella varianza a 1 minuto di ES (circa -0,48 / -0,50) [VAPG Tab. 3].
   SM: un grafico senza test. **Nessuno dei nove trova il verso opposto.**
2. **L'effetto sul livello della volatilita' e' piccolo; quello sulla forma del percorso, meno.**
   VAPG: effetto mediano della gamma sulla volatilita' del giorno -0,08 punti, massimo +3,32, contro
   variazioni da un giorno all'altro con deviazione 4,5 punti [VAPG Tab. 5A, 6]. BB: R quadro fino a
   0,003 sull'autocorrelazione [Tab. VI]. BPVZ: al 18-29% del flusso la volatilita' e' nel rumore,
   l'autocorrelazione e le code giornaliere no [Tab. 3]. **Lettura nostra**: la variabile piu'
   sensibile alla gamma non e' il livello della volatilita' ma **il segno dell'autocorrelazione dei
   rendimenti brevi**, e la scala della giornata (range, code) piu' di quella del quarto d'ora. BPVZ
   dice perche' [nota 23]: il dealer corto ammorbidisce il singolo quarto d'ora ma ne somma molti nello
   stesso verso (code del quarto d'ora 0,637 a -80, giornaliere 2,596). Un test sulla sola volatilita'
   a 15 minuti puo' non vedere un effetto che c'e' sulla giornata.
3. **La copertura che arriva sul sottostante e' meno di quella che la gamma fa supporre, o non si
   sa.** VAPG assume che i market maker si coprano tutti, subito, in ES, e trova comunque un effetto
   mediano piccolo: e' un tetto [VAPG limite 5]. DPS dice, in modo indiretto, che sulle scadenze
   brevissime la copertura e' secondaria all'abbinamento. Le note Cboe citate nell'indice (non lette)
   danno la copertura netta degli 0DTE al massimo lo 0,2% della liquidita' giornaliera SPX. BPVZ mette
   il dealer al 18-52% del flusso, molto sopra. **Lettura nostra** (DPS non parla di GEX): la GEX e' il
   flusso che ci sarebbe se tutto il residuo si coprisse, un tetto; l'open interest e' gia' il residuo
   dopo gli abbinamenti, e quanto di quel residuo si copra non lo misura nessuno dei nove.
4. **La liquidita' conta.** BPVZ: il mercato e' piu' fragile con il libro meno profondo e con piu'
   ordini a mercato [Tab. 5]; BB: l'effetto sta nei titoli meno liquidi (interazione -8,88, t -7,61)
   [Tab. V]. Dove il mercato e' profondo l'effetto e' minore; ES e NQ sono fra i future piu' liquidi.
5. **La significativita' per giornata**: BB, BDLM e DPS usano una osservazione per giorno
   (Newey-West o errori raggruppati), come chiede il metodo; VAPG fa la media di 36 statistiche
   mensili; BPVZ non fa test. **Nessuno ha un modello nullo** nel senso del metodo (la stessa prova su
   un mercato rimescolato).

### Dove Si Contraddicono

1. **Il segno dei dealer sugli indici.** I due nuovi non lo toccano: BPVZ lo fissa per scenario, DPS
   usa valori assoluti. Resta com'era:
   - GPP, SPX 1996-2001: sommando call e put per strike, dealer **corti** di gamma quasi ovunque
     (deduzione nostra dalla Tab. 1A);
   - VAPG, SPX 2020-2023, posizioni ricostruite trade per trade: gamma dei market maker **di solito
     positiva** (media 341,2), negativa almeno per un momento in un giorno su quattro, uno su due dopo
     maggio 2022 [VAPG Tab. 2];
   - BB: sulle azioni misurata, in media positiva; sugli indici assunta;
   - BDLM: per convenzione, NGE positiva nella maggior parte dei giorni;
   - GJ: dealer corti di call alla pari (1992-2009); SM li assume lunghi.

   Periodi e misure diversi (contratti netti, gamma, convenzione). **Un segno fisso non e' un dato.**
2. **Gli 0DTE abbassano o alzano la volatilita'?** Dagli abstract nell'indice, non letti: Adams e altri
   2025 e Dim, Eraker, Vilkov 2024 la trovano piu' bassa; Brogaard, Han, Won piu' alta. Dei letti, solo
   VAPG ha gli 0DTE: in media la gamma la abbassa (-0,19 punti al giorno), nei massimi la alza (10%
   dei giorni). BPVZ non ha scadenze dentro la seduta [limite 10]; DPS finisce il 16 luglio 2021, con
   circa 1.035-1.040 giornate 0DTE su circa 17 anni e mezzo. **La contraddizione resta; i due nuovi non
   la sciolgono.**
3. **Copertura alla chiusura (BDLM) contro copertura secondaria (DPS).** BDLM spiega il momento
   dell'ultima mezz'ora con chi e' corto di gamma e si copre soprattutto alla chiusura [BDLM §1]; DPS
   dice che sulle opzioni brevissime la copertura in delta pesa poco sullo spread. Non e' una
   contraddizione diretta: BDLM mette nel meccanismo anche gli ETF a leva [Tab. 9] e tutte le
   scadenze, 1996-2020; DPS misura lo spread delle opzioni, non il future, e trova la copertura ancora
   significativa a 1-6 giorni. Ma se l'abbinamento domina, la parte 0DTE di una GEX pesa meno di quanto
   conta.
4. **La liquidita' del sottostante con il dealer.** In BPVZ il dealer di qualunque segno stringe lo
   spread del sottostante (0,406 a -80); gli autori stessi dicono che e' un effetto del modello, perche'
   i market maker veri davanti alla volatilita' allargano o si ritirano [nota 5]. DPS, sulle opzioni
   vere, trova che il rischio di inventario si paga nello spread. Mercati diversi: non si confrontano,
   ma il risultato di BPVZ sullo spread non e' un finding.
5. **La scoperta del prezzo in BPVZ**, fra i suoi due esperimenti: senza notizie la gamma positiva la
   migliora (errore quadratico 0,153), con notizie la peggiora (3,514) [Tab. 2, 3; limite 7].

### Il Perche' Comune

Uno, meccanico: chi si copre in delta con gamma positiva scambia contro il movimento, con gamma
negativa con il movimento [BPVZ §2.1, BB, BDLM §1]. Tutti e nove poggiano su due passi che nessuno
misura direttamente: **(a) il segno** delle posizioni dei dealer sugli indici, e **(b) che la gamma
diventi davvero ordini sul future**. GPP e VAPG mettono in dubbio (a); DPS e' il primo letto che
mette in dubbio (b), in modo indiretto. BPVZ aggiunge la condizione: l'effetto richiede che la
copertura sia una parte non piccola del flusso.

### Cosa Si Rifa' Su NQ Con I Dati Di ATAS

**Con le sole barre**: `NQ/m1` dal 12 aprile 2017, 2.365 sedute di cassa complete, **solo dentro il
campione** (fino al 31 dicembre 2024; dal 2025 e' fuori campione e si guarda solo con `valida
--fuori`) [`i-dati-da-atas.md`](../../metodo/i-dati-da-atas.md), [`5-come-si-valida.md`](../../metodo/5-come-si-valida.md):

- l'autocorrelazione intraday dei rendimenti a 5, 15, 30, 60 minuti, seduta per seduta (BB Fig. 1,
  BPVZ D.1): quanti giorni di inversione e quanti di continuazione ha NQ;
- volatilita' realizzata a 5 e 30 minuti, range della seduta, code giornaliere, e come si muovono da
  un giorno all'altro (VAPG Tab. 6): il termine di paragone di qualunque effetto della gamma. Barre a
  5 minuti accanto a quelle a 1, perche' i prezzi scambiati rimbalzano fra denaro e lettera [VAPG];
- il momento secondo del flusso di DPS, **meglio che sulle opzioni**: i 78 delta a 5 minuti della
  seduta, con il lato dell'aggressore dato dalla borsa, non stimato (DPS lo stima, giusto per circa
  l'85%) [DPS §4.1];
- BDLM (dalla chiusura di ieri alle 15:30 -> l'ultima mezz'ora), gia' nella scheda
  [`opzioni-gamma-es`](../../studi/opzioni-gamma-es/1-idea.md), ipotesi A.

Tutto descrittivo: senza la gamma non prova il meccanismo.

**Con la gamma.** Su NQ la catena si legge dal Data Bridge (Options Chain funziona su NQ, X-Ray no) e
`FabioOrderFlow/tools/gex.py` ne calcola la GEX **dal 15 ottobre 2026**, nelle due convenzioni di
segno, in una cache che il giro d'orizzonte stampa: una serie giornaliera va conservata a parte. Su ES
`quant opzioni` archivia la catena grezza dal 14 ottobre 2026
[`le-opzioni-in-atas.md`](../../metodo/le-opzioni-in-atas.md). Nessuna storia all'indietro, e **su NQ
ogni giornata con la GEX sta nel fuori campione**: come trattarle lo decide l'utente, prima. (L'analisi
di DPS scrive "su NQ nessuna opzione": vale per l'archivio di `quant opzioni`, che oggi registra solo
ES, non per la lettura della catena.)

**Cosa richiederebbe dati che non abbiamo**: le posizioni dei market maker per tipo di partecipante
(BB, VAPG, GPP; X-Ray le ha solo su ES, in campi che non si leggono), gli scambi delle opzioni con il
NBBO (DPS), la storia della catena, gli scambi di copertura veri, la profondita' del book nel passato.
Una storia lunga di GEX da fornitori esterni esce dalla decisione 2.

**La scala, su NQ** (nostro calcolo, da `le-opzioni-in-atas.md`): circa 2.500 future per un ribasso di
300 punti contro 500-700 mila contratti al giorno, circa lo 0,4-0,5% del flusso, contro il 18-52%
del dealer di BPVZ. Le due quote non sono la stessa misura e BPVZ non ha scenari sotto il 18%: "su NQ
l'effetto e' piccolo" e' una **nostra estrapolazione**, non la previsione di un paper. Se DPS ha
ragione, la quota vera e' ancora piu' bassa.

### Cosa Resta Senza Risposta

- **Nessuno dei due nuovi misura la volatilita' intraday del future contro la gamma.** Dei nove lo fa
  solo VAPG (ES a 1 minuto, 2020-2023, supponendo che tutto si copra); BB e SM guardano l'indice al
  giorno, BDLM solo l'ultima mezz'ora. **Nessuno usa NDX o NQ.**
- **I test diretti degli 0DTE stanno su SSRN**: Adams e altri 2025 (SSRN 5641974), Dim, Eraker, Vilkov
  (SSRN 4692190), Brogaard, Han, Won (SSRN 4426358), e i preprint del 2026 scartati in fase 1 (Singh,
  Elms, Ladia). SSRN risponde 403 alle richieste automatiche: si scaricano col browser, e la verifica
  di Cloudflare la supera una persona. Il primo da prendere: **Adams e altri 2025**.
- **Il passo gamma -> ordini sul future** non lo misura nessuno; per la prova diretta DPS rimanda a Hu
  e altri, sul KOSPI 200.
- **Il segno dei dealer sulle opzioni CME sui future** (quelle che la Chain legge su ES e NQ): tutte le
  misure del segno sono sulle SPX.
- **Sotto il 18% del flusso** BPVZ non dice niente.
- **I livelli di ATAS** (Zero Gamma, muri): nessun paper letto li prova; l'unico test trovato e' un
  preprint SSRN, Popovici 2026 (SSRN 7082418), non letto.

### Le Idee Candidate

| | finding da provare | da dove | cosa la ucciderebbe |
|---|---|---|---|
| 1 | Nelle giornate di gamma negativa, misurata prima dell'apertura, l'autocorrelazione dei rendimenti a 15 minuti della seduta e' piu' alta, e il range della seduta piu' largo, che nelle giornate di gamma positiva; una osservazione per giornata, contro il nullo che rimescola la gamma fra le giornate | BPVZ ris. 3, 6, 7; BB Tab. VI; BDLM Tab. 7 | nessuna differenza con abbastanza giornate negative; la differenza sparisce controllando la volatilita' implicita (la gamma al denaro cresce quando la volatilita' implicita scende: legame meccanico, VAPG limite 8, GPP); c'e' con una convenzione di segno e non con l'altra |
| 2 | Nelle giornate di gamma negativa il delta aggressivo di ES segue il movimento appena fatto (compra dopo i rialzi) piu' che in quelle di gamma positiva: la copertura arriva sul future | DPS (il dubbio), BDLM §1, BB | nessuna differenza: la copertura che la GEX suppone non arriva sul future, o passa altrove. Un anno basta a vedere la direzione, non a provarla (analisi di DPS e BDLM) |
| 3 | Su NQ, a parita' di volume e di delta assoluto della giornata, le sedute con il delta a 5 minuti piu' volatile hanno un impatto di prezzo per contratto diverso, o un range diverso il giorno dopo | DPS per la misura; **il finding sul future e' nostro**, il paper parla solo dello spread delle opzioni | nessun legame oltre il volume e il momento primo; la misura e' un doppione del volume |

Le idee 1 e 2 vogliono la gamma: registrazione in avanti (ES dal 14 ottobre; NQ dal 15, da conservare
ogni giorno) e, su NQ, giornate fuori campione. La 3 si fa oggi, con le barre e dentro il campione, ma
nessun paper ne da' il finding sul future: la sua scheda lo deve dire.

**Lo studio da aprire, proposta (non aperto).** Non uno nuovo: **aggiornare
[`opzioni-gamma-es`](../../studi/opzioni-gamma-es/1-idea.md)** con BPVZ e DPS fra i paper, l'idea 1
come variante dichiarata accanto all'ipotesi B (l'autocorrelazione della seduta per segno della gamma,
non solo l'ultima mezz'ora), l'idea 2 come prova del meccanismo, e in "Cosa la ucciderebbe" il dubbio
di DPS sulla copertura e la quota del flusso di BPVZ. Prima ancora: conservare ogni giorno la GEX di
NQ, e decidere come trattare le giornate di NQ con la GEX, tutte fuori campione. Lo decide l'utente.

## Il Diario

- 2026-10-15 · fase 1 (cerca): 19 candidati verificati, 2 scelti, entrambi con PDF libero
  raggiunto; nessun PDF salvato nel repository.
- 2026-10-15 · fase 2 (scarica): i due PDF scaricati con curl, senza verifiche di sicurezza ne'
  login (HTTP 200, `application/pdf`, header `%PDF-1.5`), fuori da git (`.gitignore`). Buis:
  21 pagine, la prima e' la copertina del portale EUR (l'articolo va da pagina 2 a 21), versione
  dell'editore. Doshi: 70 pagine, versione del 23 maggio 2025, anonima in prima pagina. Nel testo
  nessuna frase rivolta a chi lo legge con un agente.
- 2026-10-15 · fase 3 (leggi): Doshi letto per intero (70 pagine, appendice online compresa),
  [analisi](../opzioni-gamma/doshi-2025-risky-intraday-order-flow-and-option-liquidity/analisi.md),
  stato `letto`. Finding: lo spread delle opzioni SPX brevissime lo spiega la volatilita' intraday del
  flusso, non la gamma dell'inventario dei market maker; per la GEX vuol dire flusso di copertura
  possibile, non certo, e la prova e' indiretta (nessun dato sul future). Numeri presi
  dall'estrazione `-raw`; quattro punti in cui il testo dice piu' delle tabelle, da verificare.
- 2026-10-15 · fase 3 (leggi): Buis e altri letto per intero, scritto
  [`analisi.md`](../opzioni-gamma/buis-2024-gamma-positioning-and-market-quality/analisi.md)
  (sigla BPVZ). E' una simulazione: gamma positiva abbassa la volatilita' e rende negativa
  l'autocorrelazione dei rendimenti brevi, gamma negativa la alza, la rende positiva e fa fallire il
  mercato piu' spesso; l'effetto e' grande solo quando il dealer fa il 30-52% del flusso. Le tabelle
  lette in modo `pdftotext -raw`; simboli delle formule e figure non leggibili.
- 2026-10-15 · fase 4 (verifica): Buis e altri verificato sul PDF, circa 130 affermazioni, 112
  confermate, 15 corrette, 3 tolte; stato `analizzato`. **La riga della fase 3 qui sopra va letta
  corretta**: l'effetto sulla volatilita' svanisce con il dealer al 18-29% del flusso, ma quello
  sull'autocorrelazione dei rendimenti brevi, sulle code giornaliere e sullo spread resta grande
  anche al 18-19%; il paper non ha scenari sotto il 18%, quindi un effetto piccolo su NQ e' una
  nostra estrapolazione, non una sua previsione. I simboli delle formule e la tabella 1 si leggono
  con `pdftotext -enc UTF-8` decodificando le coppie surrogate.
- 2026-10-15 · fase 4 (verifica): Doshi e altri verificato sul PDF, 174 affermazioni, 156
  confermate, 17 corrette, 1 tolta; stato `analizzato`. **Le righe qui sopra vanno lette corrette**:
  il terzo autore e' **Saim Ayberk Sert** (pagina di Doshi), non "Sert" generico ne' "Mehmet Sert";
  la pagina elenca il titolo con "Equity" e due versioni precedenti (ottobre 2024, gennaio 2025);
  SSRN 5006194 risponde 403, non riverificato. Le stelle della significativita' escono con
  `pdftotext -enc UTF-8`: dei quattro punti della fase 3, nella tabella 4 il testo del paper scambia
  call e put (la cella senza stelle e' la call 35-48 alle 12-13), la gamma sulle put 0DTE e' al 10%,
  il terzo venerdi' vale solo sulle put, e la copertura resta significativa a 1-6 e 35-41 giorni (e
  sulle put a 42-48 al 10%). "La GEX e' un tetto" e' ora segnata come lettura nostra: il paper non
  parla di GEX.
- 2026-10-15 · fase 5 (sintesi): scritta "La Sintesi" sopra, con le due analisi verificate e le sette
  del filone (queste ultime senza verifica avversaria registrata). Convergono sul verso del
  meccanismo e su un effetto piccolo sul livello della volatilita'; restano aperti il segno dei dealer,
  l'effetto degli 0DTE e il passo gamma -> ordini sul future. Tre idee candidate, nessuna scheda
  scritta (non chiesta). Allineato il terzo autore di DPS nella tabella dei candidati; la soglia del
  flusso di BPVZ e "la GEX e' un tetto" usate come corrette in fase 4. Aggiornato l'indice.
