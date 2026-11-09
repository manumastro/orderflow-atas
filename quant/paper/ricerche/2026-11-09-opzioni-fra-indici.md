# Ricerca: L'informazione delle opzioni di un indice aiuta a leggere un altro indice correlato (SPX/ES e NDX/NQ)? · 9 novembre 2026

**Filone**: opzioni-gamma · **Chiesta da**: utente, attraverso lo studio dei paper (`/studio-paper`),
fase 1 · **Quanti**: 4, **non gia' presenti** nell'[indice](../indice-dei-paper.md)

**Lo scopo**: l'utente vuole che lo studio globale di NQ ed ES sia uno solo, perche' le informazioni
delle opzioni dei due indici convergono e si aiutano. I paper devono dire **quanto e' provato** e
**come si fa**.

**Le parole**:

- **Flusso delle opzioni** (*option order flow*) e **sbilancio degli ordini** (*order imbalance*):
  quanti contratti si comprano meno quanti se ne vendono, dal lato di chi aggredisce.
- **Pressione d'acquisto pesata per il delta** (*delta buy pressure*): lo sbilancio dove ogni
  contratto conta per il suo delta, cioe' per quanto sottostante equivale. E' la stessa idea del
  "delta scambiato" che X-Ray mostra per gruppo.
- **Scoperta del prezzo** (*price discovery*) e **quota d'informazione** (*information share*): quale
  mercato si muove per primo quando arriva una notizia, e quanta parte del movimento comune nasce li'.
- **Impatto incrociato** (*cross-impact*): gli ordini su un titolo che muovono il prezzo di un altro
  titolo correlato. E' il canale per cui la copertura dei dealer su ES potrebbe arrivare a NQ.
- **Correlazione implicita**: la correlazione fra i titoli dell'indice che i prezzi delle opzioni
  sull'indice e sui titoli lasciano dedurre. Spiega perche' VXN e VIX si separano.

**Ricerche fatte** (9 novembre 2026):

- ricerca web: `cross-asset gamma hedging spillover index options dealers correlated underlyings paper`
- ricerca web: `price discovery SPX SPY options QQQ NDX lead-lag index options ETF options paper`
- ricerca web: `aggregate market gamma exposure single stock options hedging index returns predictability paper SSRN`
- ricerca web: `Nasdaq-100 options SPX options information spillover implied volatility VXN VIX lead lag`
- ricerca web: `option order flow net buying pressure predicts market index returns S&P 500 index options informed trading`
- ricerca web: `"cross-gamma" OR "cross-hedging" dealer hedging pressure index futures empirical correlation feedback options`
- ricerca web: `QQQ options SPY options gamma exposure Nasdaq S&P 500 dealer hedging comparison empirical paper arXiv`
- ricerca web: `option hedging demand stock return comovement correlation dealers index options paper`
- ricerca web: `E-mini Nasdaq-100 E-mini S&P 500 futures lead-lag intraday price discovery cross-market information transmission paper`
- ricerca web: `0DTE options spillover Nasdaq QQQ SPX hedging flows cross-index volatility transmission paper 2025`
- ricerca web: `arxiv dealer gamma exposure SPX NDX RUT multiple indices intraday volatility regime empirical`
- ricerca web: `"market maker" gamma imbalance index options "Nasdaq" "S&P 500" hedging intraday returns "both indices" OR "cross-index" study`
- ricerca web: `SSRN gamma hedging pressure spillover across assets correlated underlying "hedging spillover" options paper 2024 2025 2026`
- ricerca web: `options market makers hedging induces comovement correlation between stocks "comovement" "delta hedging" empirical paper journal`
- ricerca web: `VXN minus VIX implied volatility spread predicts Nasdaq-100 relative performance S&P 500 academic paper journal`
- ricerca web: `"implied volatility" spillover "VXN" "VIX" Granger causality transmission US volatility indices journal paper`
- ricerca web: `"Nasdaq-100" "S&P 500" futures lead-lag "information share" cross-index which index leads intraday high frequency study`
- ricerca web: titoli e autori dei candidati (Chordia e altri; Holowczak, Hu, Wu; Siriopoulos, Fassas; pagina di Muravyev)
- OpenAlex (API): `implied correlation index options future market returns`, `Nasdaq-100 options gamma hedging`,
  `VXN VIX spillover`, `gamma exposure index options hedging spillover`, `cross-impact index futures order flow`,
  `option order flow information index returns`, `index option ETF option price discovery SPY SPX`,
  `net buying pressure index options QQQ`, `Nasdaq 100 index options information`,
  `dealer gamma market-wide aggregate hedging`, `volatility spillover Nasdaq S&P 500 implied volatility indices`,
  `option hedging comovement stock returns`, `implied correlation Nasdaq-100`,
  `VXN information content Nasdaq volatility forecast`, `index option order flow market returns`,
  `price discovery index options ETF options QQQ`, `leveraged ETF rebalancing gamma Nasdaq`,
  `option market makers hedging index futures intraday`, e i titoli dei candidati
- arXiv (API): `"dynamic hedging" AND correlation AND options`, `"cross-impact" AND futures`,
  `gamma AND dealer AND index`, `"implied correlation" AND index`, `"lead-lag" AND "S&P 500" AND Nasdaq`,
  `option AND "order flow" AND index AND predict`
- Crossref (API): i quattro DOI scelti (titolo, rivista, volume, pagine, affiliazioni)
- le pagine degli autori: Liuren Wu (Baruch, nessun link al paper), Muravyev (profilo Illinois, solo la
  citazione); il repository SMU (INK) di Holowczak, Hu, Wu e' dietro un controllo anti-bot (Incapsula),
  non aggirato

**SSRN** non e' stato aperto (Cloudflare): l'esistenza dei paper SSRN e' verificata su Crossref e OpenAlex.
**Nessuna istruzione** trovata nelle pagine e nei PDF letti per la verifica.

**Supplemento su SSRN** (9 novembre 2026, sera, dopo che la persona ha superato Cloudflare nella finestra di
Playwright): ricerca del sito (`searchresults.cfm?term=`) e pagine References e Citations di chordia e
holowczak. Query: `QQQ options`, `Nasdaq 100 options S&P 500`, `NDX options`, `gamma exposure spillover`,
`cross-index option`, `VXN VIX`, `SPY QQQ options price discovery`, `dealer gamma Nasdaq`, `option hedging
spillover correlated`, `volatility leadership index options`, `implied volatility spillover equity indices`,
`Nasdaq S&P 500 lead-lag`, `cross-asset gamma`, `index options predict other index`, `option implied
information spillover across markets`, `ETF options index options information`, `technology sector options
market returns`, `implied correlation predicts returns`. **Nessun paper su SSRN prova che la gamma o il flusso
delle opzioni di NDX/QQQ predica l'S&P 500, o il contrario.** Candidati nuovi in tabella, da dicle in giu',
con titolo, autori, date e abstract letti sulla pagina SSRN. Nessuna istruzione nelle pagine aperte.

## La Risposta Breve, Prima Di Leggere

**Nessun paper trovato prova che la gamma, i muri o lo zero gamma delle opzioni di un indice
spieghino un altro indice** (SPX su NQ o NDX su ES). La letteratura sulla gamma dei dealer, compresa
quella gia' letta, misura ogni indice **con le sue** opzioni. Ci sono invece prove su due pezzi della
catena: **il flusso delle opzioni** di indici diversi dall'S&P 500 che predice l'S&P 500 (Chordia e
altri), e **la pressione degli ordini** che passa dal future piu' liquido ai titoli correlati (Le Coz e
altri; Hasbrouck). Il legame "gamma di SPX -> copertura su ES -> NQ" e' un'ipotesi da costruire con
questi pezzi, non un finding.

*Dopo la lettura* (fase 5, 9 novembre 2026): confermata nella sostanza; corretti i dettagli di Chordia e
altri (gli indici sono Russell 2000 e Nasdaq-100 sommati, solo Nasdaq-100 dal 22 aprile 2013, e le opzioni
SPX non predicono) e di Holowczak e altri (il flusso va con il prezzo nello stesso minuto, non nei successivi;
la misura proposta non pesa per il delta). Dicle, letto dopo, non la cambia: il VIX "anticipa" soprattutto le
borse che chiudono prima di New York. La risposta aggiornata e' in [La Sintesi](#la-sintesi).

## I Candidati

**tipo**: R rivista con revisione fra pari, P preprint o working paper. **Gia' nell'indice** e quindi
non ricercati: Barbon, Buraschi 2021 (BB); Baltussen e altri 2021 (BDLM); Baltussen, Terstegge, Whelan
2024 (BTW); Ni, Pearson, Poteshman, White 2021; Barbon, Beckmeyer, Buraschi, Moerke (SSRN 3925725);
Adams e altri 2025 (SSRN 5641974); Dim, Eraker, Vilkov 2024 (SSRN 4692190); gli altri del filone.

| chiave | titolo | autori, anno | id | tipo | pdf libero | perche' si', perche' no | stato |
|---|---|---|---|---|---|---|---|
| chordia-2021-index-option-trading-activity-and-market-returns | *Index Option Trading Activity and Market Returns* | Tarun Chordia (Emory), Alexander Kurov (West Virginia), Dmitriy Muravyev (Michigan State), Avanidhar Subrahmanyam (UCLA, Nanjing), 2021 | doi [10.1287/mnsc.2019.3529](https://doi.org/10.1287/mnsc.2019.3529), Management Science 67(3), 1758-1778; [SSRN 2798390](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2798390) | R | no: solo SSRN (Cloudflare). Una copia su Scribd non e' degli autori: non si usa | si': **il test piu' diretto della domanda sul flusso**. Dall'abstract: lo sbilancio settimanale degli acquisti di put su indici all'ISE predice il rendimento settimanale dell'S&P 500, piu' forte con VIX alto e dopo gli annunci macro. Da un riassunto di terzi (CXO, versione 2016, **da verificare in lettura**): gli indici sono ISE 250, Russell 2000, **Nasdaq 100** e S&P 400, e il risultato regge controllando il flusso di ES e di SPY. Cioe' il flusso delle opzioni di altri indici, Nasdaq compreso, informa l'S&P 500. No: orizzonte settimanale, non intraday; dati fino a circa il 2013-2017 | analizzato (verificato il 9 novembre 2026; SSRN 2798390, versione del 17 ottobre 2019 accettata da Management Science, non la pubblicata, 59 pagine; il "da verificare" e' risolto: RUT e NDX sommati, solo NDX dal 22 aprile 2013), sha256 `fa1b5c768f3f4bc34a66af5844b572ad362e635a78e919791e3271ec0a8f9998` |
| holowczak-2014-aggregating-information-in-option-transactions | *Aggregating Information in Option Transactions* (working paper: *Consolidating Information in Option Transactions*, 15 marzo 2011) | Richard Holowczak, Jianfeng Hu, Liuren Wu (Baruch College, CUNY), 2014 | doi [10.3905/jod.2014.21.3.009](https://doi.org/10.3905/jod.2014.21.3.009), Journal of Derivatives 21(3), 9-23; SSRN 1740046 e 1787407 | R | no in automatico: copia SMU su [core.ac.uk](https://core.ac.uk/download/111759292.pdf) (403 a curl) e su INK SMU (controllo anti-bot); SSRN (Cloudflare). Serve il browser e la persona | si': **il come** per il flusso delle opzioni. Dall'abstract (OpenAlex): classificano gli scambi di opzioni come iniziati dal compratore o dal venditore e li sommano pesati per il delta (domanda di esposizione al prezzo) e per il vega (domanda di volatilita'); su **QQQ, cioe' il Nasdaq-100**, a intervalli intraday, entrambe le misure predicono rendimento e volatilita' nello stesso intervallo e nei successivi. E' la base per leggere il "delta scambiato" di X-Ray e della catena di NQ. No: un solo indice, non incrociato; dati vecchi (QQQQ, prima del 2011) | analizzato (verificato il 9 novembre 2026; SSRN 1740046, working paper *Consolidating Information in Option Transactions*, prima stesura del 13 gennaio 2011, non la pubblicata, 28 pagine; corretto in lettura: va con il prezzo nello stesso minuto, non nei successivi), sha256 `343b8e738893d66d36df4b4902e7fb3eadba819afd30b498f25f8e6496b6ebc2` |
| le-coz-2024-when-is-cross-impact-relevant | *When is cross impact relevant?* | Victor Le Coz (Ecole polytechnique), Iacopo Mastromatteo (Capital Fund Management), Damien Challet (Universite' Paris-Saclay), Michael Benzaquen (Ecole polytechnique, CFM), 2024 | doi [10.1080/14697688.2024.2302827](https://doi.org/10.1080/14697688.2024.2302827), Quantitative Finance 24(2), 265-279; arXiv [2305.16915](https://arxiv.org/abs/2305.16915) v2; SSRN 4460293 | R | si': [arxiv.org/pdf/2305.16915v2](https://arxiv.org/pdf/2305.16915v2), raggiunto il 9 novembre 2026 | si': **il canale di trasmissione**. Dall'abstract: dati tick per tick su 500 strumenti USA, 2017-2022 (azioni, obbligazioni, future su obbligazioni e **su indici**, dal PDF §IV.A), stimati un anno dentro e il successivo fuori; la formazione del prezzo avviene negli strumenti piu' liquidi, e gli scambi li' muovono i prezzi dei correlati meno liquidi. Il future E-mini S&P e' nominato fra gli strumenti per cui il modello regge su scale di tempo piu' lunghe (arXiv v2, §V.B). Se ES e' il serbatoio di liquidita', la copertura dei dealer di SPX su ES passa a NQ per questa via. Rifacibile con ATAS: il flusso di ES e di NQ c'e'. No: non riguarda le opzioni | analizzato (verificato il 9 novembre 2026; arXiv v2, 26 marzo 2024), sha256 `63d78f1c18f2c4374d2622bdc7476a3da945cf56c01b9976aaaebf02be6a52d9` |
| hasbrouck-2003-intraday-price-formation-in-us-equity-index-markets | *Intraday Price Formation in U.S. Equity Index Markets* | Joel Hasbrouck (NYU Stern), 2003 | doi [10.1046/j.1540-6261.2003.00609.x](https://doi.org/10.1046/j.1540-6261.2003.00609.x), Journal of Finance 58(6), 2375-2400 | R | si', la versione di lavoro (16 novembre 2000): [archive.nyu.edu](https://archive.nyu.edu/jspui/bitstream/2451/27374/2/FIN-00-046.pdf), raggiunto il 9 novembre 2026. La versione pubblicata (Wiley) risponde 403 alla richiesta automatica | si': **il metodo per dire chi guida**, il riferimento per la scoperta del prezzo sugli indici USA. Dall'abstract: per S&P 500 e Nasdaq-100 la scoperta del prezzo avviene soprattutto sugli E-mini, che anticipano i future regolari e gli ETF; l'ETF dell'S&P 500 contribuisce molto alla scoperta del prezzo degli ETF di settore e poco il contrario; il settore tecnologico (il piu' scambiato) contribuisce un poco all'indice (versione 2000). Le quote d'informazione si calcolano su ES e NQ con le barre di ATAS. No: non riguarda le opzioni; dati del 2000; **da verificare in lettura** se stima S&P 500 e Nasdaq-100 insieme | analizzato (verificato il 9 novembre 2026; versione di lavoro del 16 novembre 2000, non la pubblicata), sha256 `68f7d314210c1e6215064015fc38f5a24e300c9ed05188661511a41dedcaad3a` |
| cont-2023-cross-impact-of-order-flow-imbalance | *Cross-impact of order flow imbalance in equity markets* | Rama Cont, Mihai Cucuringu, Chao Zhang (Oxford, Alan Turing Institute), 2023 | doi [10.1080/14697688.2023.2236159](https://doi.org/10.1080/14697688.2023.2236159), Quantitative Finance; arXiv [2112.13213](https://arxiv.org/abs/2112.13213) | R | si': arXiv, raggiunto | dall'abstract: nello stesso intervallo l'impatto incrociato non aggiunge niente una volta che lo sbilancio del libro e' ben misurato; **lo sbilancio ritardato degli altri titoli migliora la previsione**, a orizzonti brevi, e decade presto. Utile, ma solo azioni (100 titoli dell'S&P 500, Nasdaq ITCH); Le Coz e altri hanno i future su indici. Da tenere come seconda fonte se la sintesi lo chiede | scartato, da tenere |
| tomas-2022-cross-impact-in-derivative-markets | *Cross impact in derivative markets* | Mehdi Tomas, Iacopo Mastromatteo, Michael Benzaquen, 2021-2022 | arXiv [2102.02834](https://arxiv.org/abs/2102.02834) v2 | P (sede non dichiarata su arXiv) | si' | stima l'impatto incrociato fra future E-mini, opzioni europee e future sul VIX: e' un metodo di stima, non dice se un indice informa l'altro | scartato |
| bangsgaard-2023-lead-lag-vix-futures-spx-futures | *The lead-lag relation between VIX futures and SPX futures* | Christine Bangsgaard, Thomas Kokholm (Aarhus), 2023 | doi [10.1016/j.finmar.2023.100851](https://doi.org/10.1016/j.finmar.2023.100851), Journal of Financial Markets | R | si' (accesso aperto, da OpenAlex) | dall'abstract: con volatilita' alta i future sul VIX anticipano quelli sull'S&P; la copertura dei market maker cambia il legame. Tocca la pagina X-Ray (VIX), ma non e' fra due indici azionari | scartato |
| mu-2025-liquidity-price-informativeness-options-extended-hours | *Liquidity and Price Informativeness of Options: Evidence From Extended Trading Hours* | Liangyi Mu (Queen's Belfast), Arie Eskenazi Gozluklu (Warwick), 2025 | doi [10.1002/fut.70026](https://doi.org/10.1002/fut.70026), Journal of Futures Markets | R | si' (Wiley, accesso aperto) | dall'abstract: i prezzi delle opzioni sull'indice nella sessione estesa informano l'indice e la volatilita' della seduta dopo. Stesso indice, non incrociato | scartato |
| lee-2026-cross-market-convergence-trading-options-order-flow | *Cross-Market Convergence Trading in Options Order Flow* | Jaeram Lee, Doojin Ryu, Heejin Yang, Jinyoung Yu, 2026 | doi [10.1002/fut.70142](https://doi.org/10.1002/fut.70142), Journal of Futures Markets | R | si' (Wiley, accesso aperto) | flusso delle opzioni contro il loro future e il pronto, sul mercato coreano: "fra mercati" vuol dire opzioni-future dello stesso indice | scartato |
| chiang-2012-implied-volatility-indexes-spot-indexes | *The Relationships between Implied Volatility Indexes and Spot Indexes* | Shu-Mei Chiang, 2012 | doi [10.1016/j.sbspro.2012.09.1179](https://doi.org/10.1016/j.sbspro.2012.09.1179), Procedia | atti di convegno | si' | dall'abstract: VIX con S&P 500 e VXN con Nasdaq-100, ciascuno col suo; "nessun legame anticipatore evidente" fra Nasdaq-100 e VXN. Sede debole, nessuna prova incrociata VIX -> NDX | scartato |
| siriopoulos-2013-dynamic-relations-uncertainty-expectations | *Dynamic relations of uncertainty expectations: a conditional assessment of implied volatility indices* | Costas Siriopoulos, Athanasios Fassas, 2013 | doi [10.1007/s11147-012-9085-x](https://doi.org/10.1007/s11147-012-9085-x), Review of Derivatives Research 16(3), 233-266 | R | no (a pagamento) | correlazioni dinamiche fra tutti gli indici di volatilita' implicita pubblici: dice che si muovono insieme, non chi anticipa chi, e non se VXN serve a leggere l'S&P | scartato |
| konstantinidi-2008-can-evolution-implied-volatility-be-forecasted | *Can the evolution of implied volatility be forecasted? Evidence from European and US implied volatility indices* | Eirini Konstantinidi, George Skiadopoulos, Emilia Tzagkaraki, 2008 | doi [10.1016/j.jbankfin.2008.02.003](https://doi.org/10.1016/j.jbankfin.2008.02.003), Journal of Banking & Finance; SSRN 1114325 | R | no: SSRN | prevede gli indici di volatilita' (abstract non leggibile da qui); non e' la domanda | scartato |
| driessen-2013-option-implied-correlations-price-of-correlation-risk | *Option-Implied Correlations and the Price of Correlation Risk* | Joost Driessen, Pascal Maenhout, Grigory Vilkov, 2012-2013 | SSRN 2166829 e 2359380 (OpenAlex) | P | no: SSRN | la correlazione implicita e il suo premio: spiega perche' VXN e VIX si separano, non se un indice aiuta a leggere l'altro. Da riprendere solo se lo studio usera' lo scarto VXN-VIX | scartato |
| kurov-2004-price-dynamics-regular-emini-futures | *Price Dynamics in the Regular and E-Mini Futures Markets* | Alexander Kurov, Dennis Lasser, 2004 | Journal of Financial and Quantitative Analysis 39(2), 365-384 (IDEAS) | R | no | dall'abstract: su S&P 500 e Nasdaq-100 la scoperta del prezzo parte dagli E-mini. Hasbrouck copre la stessa domanda con gli ETF | scartato |
| goel-2026-dealer-inventory-frictions-spx-letf | *Dealer-inventory frictions in SPX options: Evidence from LETF rebalancing* | Aditya Goel, 2026 | doi [10.1016/j.frl.2026.110474](https://doi.org/10.1016/j.frl.2026.110474), Finance Research Letters | R | forse: una copia su eprints.gla.ac.uk, non aperta | gli shock del ribilanciamento degli ETF a leva allargano lo spread delle opzioni SPX: inventario dei dealer, non trasmissione fra indici | scartato |
| dicle-2017-us-implied-volatility-predictor-international-returns | *US Implied Volatility as a Predictor of International Returns* | Mehmet F. Dicle (Loyola University New Orleans); scritto il 3 novembre 2017 | [SSRN 3057745](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3057745), doi 10.2139/ssrn.3057745; Quantitative Finance and Economics 1(4), 388-402 | R (sede minore, ad accesso aperto) | PDF su SSRN (20 pagine), serve il browser; la copia della rivista non e' stata cercata | si': **l'unico trovato in cui l'informazione delle opzioni di un indice predice altri indici**, al netto del rendimento dell'S&P 500. Dall'abstract SSRN: il VIX, gennaio 2000 - luglio 2017, **anticipa 18** dei 20 indici azionari esteri e ne **causa nel senso di Granger 17** (cioe' il suo passato migliora la previsione), controllando il rendimento dell'S&P 500 e la crisi 2007-2008. E' lo schema da rifare fra VIX e NQ, o fra VXN ed ES. No: dati giornalieri, indici esteri e non NDX; volatilita' implicita, non gamma ne' flusso; sede minore | analizzato (verificato il 9 novembre 2026; SSRN 3057745, working paper del 3 novembre 2017, non la versione pubblicata, 20 pagine), sha256 `41b9313fc246a8524b30cf0ed1477d413488fb63814adee2292bb63357541476` |
| figlewski-2017-volatility-leadership-among-index-options | *Volatility Leadership Among Index Options* | Stephen Figlewski (NYU Stern), Anja Frommherz (Basilea); scritto il 30 giugno 2016 | [SSRN 2837536](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2837536), doi [10.3905/jod.2017.25.2.043](https://doi.org/10.3905/jod.2017.25.2.043), Journal of Derivatives 25(2) | R | **no**: SSRN ha solo l'abstract, senza PDF; la rivista e' a pagamento | dall'abstract: sul complesso S&P 500 l'**ETF** e' il mercato che trasmette prima prezzo e volatilita', e le put poco fuori dal denaro portano piu' informazione sulla volatilita'. E' il metodo per dire quale mercato di opzioni guida, ma **dentro un solo indice** (SPX, SPY), non fra due | scartato, da tenere (nessun PDF libero) |
| cenobar-2026-gamma-exposure-walls-ndx | *Do Gamma-Exposure Walls Show Strike-Local Effects on NDX? A Pre-Registered, Placebo-Controlled Test* | Reza Cenobar, Katie Fazeli (indipendenti, senza affiliazione); 9 ottobre 2026 | [SSRN 7592005](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7592005), doi 10.2139/ssrn.7592005 | P | PDF su SSRN (13 pagine) | dall'abstract: muri di GEX sulle opzioni NDX/NDXP, 124 sedute (12 gennaio - 10 luglio 2026), disegno preregistrato con livelli placebo: i muri si rompono un po' meno dei placebo (call 16,9% contro 23,8%, put 15,0% contro 22,5%) ma **non significativo** (p 0,115 e 0,135); il "fade" al muro perde -4,0 e -2,3 punti di NQ per operazione. **Il solo test su NDX trovato**, ma dentro un solo indice: risponde alla gamma di NQ, non a questa domanda. Dichiara di essere scritto con l'aiuto di un'intelligenza artificiale; stesso disegno di Popovici (SSRN 7082418, scartato il 15 ottobre) | scartato, da tenere (per una ricerca sulla gamma di NQ) |
| krause-2014-implied-volatility-dynamics-etfs-component-stocks | *Implied Volatility Dynamics Among Exchange-Traded Funds and Their Largest Component Stocks* | Timothy A. Krause, Donald D. Lien, 2014 | [SSRN 2778295](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2778295), Journal of Derivatives 22(1), 7-26 | R | no: solo abstract su SSRN | dall'abstract: le volatilita' implicite degli ETF di settore e dei loro titoli hanno un fattore di mercato e uno di settore; il contagio dagli ETF ai titoli dipende soprattutto dagli scambi di opzioni su SPY e sugli ETF di settore. Volatilita' fra ETF e titoli, non fra due indici | scartato |
| arak-2006-vix-and-vxn-fear-gauges-or-forecasts | *The VIX and VXN Volatility Measures: Fear Gauges or Forecasts?* | Marcelle Arak, Naranchimeg Mijid, 2006 | [SSRN 2344384](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2344384), Derivatives Use, Trading and Regulation 12(1) | R | no: SSRN | dall'abstract: VXO e VXN dalla meta' degli anni '90 al 2002; dopo il default russo entrambi anticipano la volatilita' del proprio indice. Ciascuno col suo indice, dati vecchi | scartato |
| ivanov-2011-implied-volatility-of-etf-and-index-options | *The Implied Volatility of ETF and Index Options* | Stoyu I. Ivanov, Jeff Whitworth, Yi Zhang, 2011 | [SSRN 1879583](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=1879583), International Journal of Business and Finance Research 5(4) | R | no: SSRN | dall'abstract: il sorriso di volatilita' di DIA, SPY e QQQ e' piu' marcato di quello dei loro indici. Confronto ETF-indice, nessuna previsione | scartato |
| bernales-2016-implied-correlation-and-market-returns | *Implied Correlation and Market Returns* | Alejandro Bernales, Marcela Valenzuela, 2016 | [SSRN 2739757](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2739757) | P | no: SSRN | dall'abstract: la correlazione implicita predice il rendimento del mercato a trimestri e semestri. Un indice solo, orizzonte lungo; come Driessen e altri, spiega lo scarto VXN-VIX, non la domanda | scartato |
| buss-2019-expected-correlation-and-future-market-returns | *Expected Correlation and Future Market Returns* | Adrian Buss, Lorenzo Schonleber, Grigory Vilkov, 2019 | [SSRN 3114063](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3114063) | P | no: SSRN | dall'abstract: la correlazione implicita da opzioni su indice e titoli predice il mercato fino a 18 mesi, anche fuori campione. Stesso motivo di Bernales e Valenzuela | scartato |

Esclusi perche' non verificabili alla fonte o senza sede: il materiale dei fornitori (SpotGamma,
FlashAlpha, MenthorQ, Convextrade, thetrading.tools, Sentimentrader) su VXN-VIX, "cross gamma" e GEX di
QQQ, che fa affermazioni senza test pubblicati; un paper MDPI sul riconoscimento della gamma con i
modelli linguistici (non e' la domanda); il preprint arXiv 2209.04685 (portafogli con opzioni e
CoVaR: teoria di portafoglio, non trasmissione fra indici).

## La Scelta

- **chordia-2021-index-option-trading-activity-and-market-returns**: e' l'unico candidato
  revisionato che misura **il flusso delle opzioni di indici diversi dall'S&P 500 che predice
  l'S&P 500**, con il controllo del flusso di ES e SPY (questo dal riassunto di terzi, da verificare).
  Dichiara il perche' (la protezione dei piccoli con le put, preferita a sentiment e informazione dei
  market maker). Rifacibile su NQ solo in parte: ATAS non da' il flusso delle opzioni per indice nel
  passato; il flusso di X-Ray e della catena c'e' solo da ottobre 2026.
- **holowczak-2014-aggregating-information-in-option-transactions**: **come si somma** il flusso
  delle opzioni (pesato per delta e per vega), provato sul **Nasdaq-100** a intervalli intraday. Serve
  a decidere come si legge il delta scambiato di SPX (X-Ray) e quello di NQ (catena CME) prima di
  confrontarli. Revisionato, autori con lavori revisionati sulle opzioni (Wu).
- **le-coz-2024-when-is-cross-impact-relevant**: **il canale**: la pressione degli ordini nasce sullo
  strumento piu' liquido e si trasmette ai correlati, con future su indici nel campione, 2017-2022, e
  stima fuori campione anno per anno. Rifacibile con le barre e il delta di ES e NQ da ATAS. PDF libero.
- **hasbrouck-2003-intraday-price-formation-in-us-equity-index-markets**: **il metodo** classico per
  dire quale mercato guida (quote d'informazione) proprio su S&P 500 e Nasdaq-100, con l'indice largo
  che guida i settori e non il contrario. Rifacibile su ES e NQ con ATAS. PDF libero (versione di
  lavoro).

**Il supplemento su SSRN (9 novembre 2026, sera): uno in piu', non tre.** Su SSRN non c'e' nessun paper sulla
gamma o sul flusso delle opzioni di un indice contro un altro indice; gli altri candidati restano dentro un
indice solo, quindi se ne sceglie uno.

- **dicle-2017-us-implied-volatility-predictor-international-returns** (SSRN 3057745): e' l'unico che prova
  **l'informazione delle opzioni di un indice (il VIX) contro altri indici**, al netto del rendimento
  dell'S&P 500, con una prova di Granger. E' lo schema da rifare fra VIX e NQ, o fra VXN ed ES. Rivista minore,
  dati giornalieri, volatilita' implicita e non gamma: e' il piu' vicino, non una risposta. PDF su SSRN.

Fuori, con il motivo in tabella: Figlewski e Frommherz (il metodo per dire quale mercato di opzioni guida, ma
dentro l'S&P 500, e senza PDF libero: da tenere), Cenobar e Fazeli (muri di GEX su NDX, dentro un indice: da
tenere per una ricerca sulla gamma di NQ), Krause e Lien, Arak e Mijid, Ivanov e altri, Bernales e Valenzuela,
Buss e altri.

**Perche' non gli altri.** Cont e altri dice cose vicine a Le Coz ma solo sulle azioni. Gli studi su
VIX e VXN sono di correlazione o in sedi deboli, e nessuno prova che uno serva a leggere l'altro
indice. La correlazione implicita (Driessen e altri) spiega lo scarto VXN-VIX ma non risponde alla
domanda. Bangsgaard e Kokholm, Mu e Gozluklu, Lee e altri sono revisionati ma restano dentro un solo
indice o fra volatilita' e indice.

**Cosa copre gia' la biblioteca.** Nessuno dei paper letti prova un effetto **incrociato**: ognuno
misura un indice con le sue opzioni. **BB** ha quattro indici (S&P 500, Dow, Nasdaq 100, Russell
2000) ma la gamma di ciascuno spiega la sua volatilita', con il segno assunto. **BDLM** ha il momento
dell'ultima mezz'ora su NQ (t 7,97) e su ES, ma la gamma la misura solo sull'S&P: la prova "gamma
dell'S&P -> momento di NQ" non c'e' ed e' la piu' vicina da costruire. **BTW** trova la stessa tenda
di scadenza su ES e su NQ, e avverte che sugli stessi eventi i due non sono campioni indipendenti. Fra
i trovati, **Ni e altri 2021** (copertura delle opzioni che muove il sottostante) e **Barbon,
Beckmeyer, Buraschi, Moerke** (gamma sommata su titoli ed ETF a leva) sono i piu' vicini al contagio
della gamma, ma sui titoli.

**Cosa resta scoperto, da dire all'utente.** "Le opzioni di ES e NQ convergono e si aiutano" **non e'
un finding pubblicato**: per la gamma e i livelli non c'e' un paper; per il flusso c'e' Chordia e
altri, a orizzonte settimanale; per la trasmissione fra future ci sono Le Coz e altri e Hasbrouck. Uno
studio globale unico si puo' giustificare come **lettura** (contesto comune), non come regola provata.

## La Sintesi

9 novembre 2026, fase 5, **aggiornata lo stesso giorno** con il supplemento su SSRN. Cinque analisi verificate
(per ognuna conta la sezione "La Verifica"):
**CKMS** (Chordia, Kurov, Muravyev, Subrahmanyam, versione accettata del 17 ottobre 2019, non la pubblicata
in Management Science 2021), **HHW** (Holowczak, Hu, Wu, working paper del 13 gennaio 2011, non la pubblicata
nel Journal of Derivatives 2014), **LMCB** (Le Coz, Mastromatteo, Challet, Benzaquen, arXiv v2 2024),
**HAS** (Hasbrouck, versione di lavoro del 16 novembre 2000, non la pubblicata), e dal supplemento **DIC**
(Dicle, working paper SSRN del 3 novembre 2017, non la versione pubblicata in Quantitative Finance and Economics,
sede minore). CKMS e HHW servono anche alla ricerca sul
[flusso per gruppo](2026-11-09-flusso-delle-opzioni-per-gruppo.md#la-sintesi).

**La risposta breve, dopo la lettura.** (1) **Nessun paper prova che la gamma, i muri o lo zero gamma di un
indice spieghino un altro indice**: nessuno dei quattro tocca la gamma, e il legame che si vede fra indici e'
**comune e contemporaneo**, non una guida. (2) CKMS: le put comprate dai piccoli sulle opzioni ISE di
**Russell 2000 e Nasdaq-100 sommati** (solo Nasdaq-100 dal 22 aprile 2013, senza una stima del solo NDX) sono
seguite da un S&P 500 **piu' alto** a settimane, col VIX alto, ed e' assicurazione; le opzioni SPX non
predicono. (3) HHW: sul flusso delle opzioni di QQQ dice **come si somma** (segno, delta, taglia), e che va con
il prezzo nello stesso minuto e quasi non dopo; LMCB e HAS: il prezzo si forma sui future liquidi e la guida
si misura in secondi. DIC, l'unico che prova le opzioni di un indice (il VIX) contro altri indici, trova un
"anticipo" di un giorno forte dove le borse **chiudono prima di New York** (Asia, Europa) e molto ridotto, ma
non sparito, nelle Americhe, che chiudono insieme (Brasile, Canada e Cile restano significativi): in gran
parte, secondo la nostra lettura, e' l'oggi americano visto il giorno dopo.

**In parole semplici.** L'idea "le opzioni di ES e di NQ si aiutano a vicenda" non ha un paper che la provi.
Quello che c'e': quando la gente ha paura compra protezione su piu' indici insieme, e quella paura, nei
periodi agitati, e' stata seguita da settimane migliori per tutto il mercato (CKMS); e il prezzo nasce sui
future piu' scambiati, poi gli altri seguono nel giro di secondi (HAS, LMCB). Quando la paura americana (il
VIX) sembra annunciare le borse estere il giorno dopo, e' soprattutto perche' Tokyo o Londra hanno gia' chiuso
quando New York si muove (DIC, lettura nostra); fra due mercati aperti nelle stesse ore il vantaggio si riduce
molto. Sono ragioni per leggere NQ ed ES **insieme come contesto**, non regole per leggere l'uno con i livelli
dell'altro.

### I Finding, Uno Per Paper

- **CKMS** [§2, nota 11, §3.1-3.3, §4, §5.2, Tab. 1-4, 6, 7, 9, IA Tab. A1-A2, Fig. 2-3]: 615 settimane
  2006-2017. Lo sbilancio settimanale delle put comprate in apertura sulle opzioni su indici dell'ISE (**RUT e
  NDX sommati**, circa il 94% del volume su indici dell'ISE; **solo NDX dal 22 aprile 2013**, quando le RUT
  escono dall'ISE) predice l'S&P 500 **al contrario**: +0,22% la settimana dopo per deviazione standard
  (Tab. 2), +0,55% netto in 10 settimane partendo da circa -0,22 nella settimana dello shock (Fig. 3). Solo
  col VIX nel quartile alto (0,2870 contro 0,0235, p 0,41), solo gli acquisti, solo i clienti fino a 100
  contratti. Le opzioni **SPX** non predicono (IRF 0,02, errore 0,30); i flussi di E-mini, SPY e titoli NYSE
  non predicono la settimana dopo (l'E-mini muove la settimana stessa, correlazione 0,60). **Nessuna stima di
  NDX da solo**, nessun salto nel 2013; il bersaglio e' l'S&P 500, e con il Nasdaq-100 come bersaglio i
  risultati sono "simili" ma **non tabulati**. Correlazioni settimanali S&P 500 - Nasdaq-100 0,87, S&P 500 -
  Russell 2000 0,92. Letto come premio per l'assicurazione.
- **HHW** [§2, §3, §4.1-4.2, Tab. 3-4]: opzioni di QQQQ (oggi QQQ) nel 2006, ogni scambio OPRA segnato con
  Lee e Ready, a un minuto, 72 modi di sommare. Nello stesso minuto la pressione sul prezzo va con il
  rendimento (fino a 0,41, *delta per share* con il logaritmo della taglia); il minuto dopo fra -0,0035 e
  0,0110, con stelle da 85.470 minuti trattati come indipendenti. La misura proposta (HHW) pesa per n(d1)/M,
  cioe' per vicinanza al denaro e scadenza breve, **non per il delta**. **Un solo sottostante, nessun
  confronto fra indici.** *Corregge la fase 1, che dal riassunto scriveva "predicono nello stesso intervallo e
  nei successivi" e "pesato per il delta".*
- **LMCB** [§III, §IV.A, §V.B-V.E, App. B, Fig. 2, 10, 16]: 500 strumenti USA 2017-2022, stima sull'anno
  prima e misura sull'anno stesso. Il flusso di uno strumento spiega **in piu'** il prezzo di un altro
  (fino a oltre il 5% di R2 aggiunto, celle fino a 8,1 e un 11,4) a tre condizioni: 10-20 scambi per finestra
  in tutti e due, correlazione sopra il 50%, strumento spiegato **meno liquido** di quello che spiega. **E' una
  spiegazione nella stessa finestra, non una previsione.** L'E-mini S&P e' fra i piu' liquidi; **il
  Nasdaq-100 non e' nominato**. Il modello lineare e' "invalidato" dall'autocorrelazione del flusso, dicono
  gli autori, ma "resta significativo".
- **HAS** [§6, §7, Tab. 4, 5, 7]: marzo-maggio 2000, 64 giornate, a un secondo, una stima per giornata.
  Quota d'informazione dell'E-mini: ES 0,854-0,882 sull'S&P 500, NQ 0,836-0,853 sul Nasdaq-100, **stimati
  separatamente**. L'unico numero fra indici e' una frase: QQQ contribuisce alla scoperta del prezzo di SPY
  "al massimo circa il 10%", senza tabella e senza la direzione opposta. La guida sta in secondi: con 10
  secondi di ritardo sull'S&P il future regolare e' gia' davanti all'E-mini (Tab. 7).
- **DIC** [Dati p. 7, Econometria p. 8-9, Sign. econ. p. 10, Tab. 1-4]: chiusure giornaliere di 20 indici
  esteri, del VIX e dell'S&P 500, 2000 - luglio 2017, da Yahoo e Google Finance. Regressione con GARCH(1,1) e
  prova di Granger in un SUR. La variazione del VIX di ieri ha un coefficiente negativo e significativo per **18
  indici su 20** (17 all'1%, da -0,0068 del Cile a -0,0739 del Giappone; il Messico al 5%; Argentina e Nuova
  Zelanda niente), e il VIX "causa nel senso di Granger" 17 indici, con statistiche da 166 a 667 in Asia ed
  Europa e da 7 a 35 nelle Americhe (Brasile 7,06 al 5%, Cile 17,49 e Canada 34,98 all'1%; Argentina e Messico
  niente). L'S&P 500 entra **solo dello stesso giorno**, mai di ieri. Una regola "compra l'estero se il VIX
  scende piu' del 2,5%" batte il compra e tieni in 15 indici su 20, nessuno nelle Americhe con un vantaggio
  pieno, senza costi e senza campione fuori. **Nasdaq, NDX e VXN non ci sono.** Il paper non dice come allinea
  le date di borse con fusi diversi; la lettura dei fusi (il "VIX di ieri" e' in gran parte contemporaneo per
  Asia ed Europa, e la regola compra per l'Asia una chiusura che c'era gia' prima del segnale) e' **nostra**,
  coerente con i numeri ma non provata, e presuppone le date allineate per calendario. **Correzione della
  verifica**: nelle Americhe l'anticipo si riduce di molto ma **non sparisce**.

### Dove Convergono

1. **Nessuno prova un effetto incrociato della gamma o dei livelli.** Nessuno dei quattro usa la gamma; la
   biblioteca gia' letta misura ogni indice con le sue opzioni (BB, BDLM, BTW; vedi "Cosa copre gia' la
   biblioteca").
2. **Il legame fra indici che si vede e' comune, non di guida**: CKMS lo legge come premio di rischio comune
   al mercato (correlazioni settimanali 0,85-0,92); HAS stima i due indici separatamente e trova QQQ verso
   SPY al massimo al 10%; HAS stesso avverte che correlazione non e' guida (XLI molto correlato, quasi senza
   quota). DIC, letto con gli orari, va nello stesso verso: dove gli orari coincidono (le Americhe) il
   vantaggio di un giorno del VIX si riduce di molto.
3. **Contemporaneo, non anticipatore**: LMCB spiega il prezzo con il flusso della stessa finestra; HHW trova
   0,41 nello stesso minuto e circa 0,01 dopo; HAS misura la guida in secondi, che su barre di un minuto
   diventa movimento insieme; in CKMS il flusso degli E-mini muove la settimana stessa e non la dopo; in DIC
   l'"anticipo" e' forte proprio dove il giorno dopo estero contiene l'oggi americano (lettura nostra).
4. **Il prezzo si forma sui future liquidi**: HAS (gli E-mini guidano su S&P 500 e Nasdaq-100), LMCB
   (l'E-mini S&P fra i serbatoi di liquidita', con la durata ottima lunga).
5. **Quando un flusso di opzioni su indici anticipa qualcosa, e' lento e al contrario**: CKMS a settimane,
   come PP e CLZZ nell'altra ricerca; HHW sul minuto dopo non da' niente di utile.

### Dove Si Contraddicono

- **Il flusso delle opzioni predice o no?** CKMS si' (settimane, put dei piccoli in apertura, contratti),
  HHW quasi no (minuto dopo, tutti gli scambi, pesati per il delta). Non e' una vera contraddizione:
  orizzonti, misure e chi scambia sono diversi.
- **SPX contro NDX**: in CKMS le opzioni su indici dell'ISE predicono e le SPX no, e gli autori lo spiegano con
  **chi scambia** (piccoli contro istituzionali), non con l'indice. Nell'altra ricerca PP trova contrarian NDX
  e OEX, non SPX. La differenza fra indici, nei paper, e' una differenza di **partecipanti**.
- **Chi guida fra ES e NQ**: nessuno lo misura. HAS ha solo QQQ verso SPY (2000, ETF); LMCB dice "dal piu'
  liquido al meno liquido", ma quale fra ES e NQ sia il piu' liquido nel suo senso si misura, non si assume.
- **Previsione o spiegazione fra future**: HAS usa un VAR con i ritardi (dinamico, a secondi); LMCB solo la
  stessa finestra. Cont e altri (scartato, da tenere) trova che il flusso ritardato degli altri titoli aiuta
  a orizzonti brevi, sulle azioni.
- **Un anticipo di un giorno, o niente?** DIC trova che il VIX anticipa altri indici di un giorno (18 su 20);
  gli altri quattro trovano legami contemporanei o di secondi. La contraddizione si scioglie in gran parte con
  gli orari (lettura nostra), **ma non del tutto**: nelle Americhe, che chiudono insieme a New York, Brasile,
  Canada e Cile restano significativi (Wald da 7 a 35, coefficienti da -0,0046 a -0,0097). Un piccolo anticipo
  giornaliero fra mercati aperti nelle stesse ore non e' escluso; il paper non lo separa dall'S&P 500 di ieri,
  che non mette mai nel modello.
- **I periodi**: HAS 2000, HHW 2006, CKMS 2006-2017, DIC 2000-2017, LMCB 2017-2022. Nessuno vede gli 0DTE
  quotidiani.

### Il Perche' Comune

Tre ragioni, dette dagli autori, e una nostra. (a) Gli indici azionari americani condividono un **fattore comune** e un
premio di rischio comune: la paura comprata su un indice si paga su tutto il mercato (CKMS §5.2). (b) Il prezzo
si forma **dove c'e' la liquidita'** (LMCB §V.E, §VII; HAS §6.d, congettura sui costi non visibili), e gli altri
strumenti seguono a una velocita' limitata dal piu' lento. (c) Sulle opzioni di un ETF l'informazione e'
soprattutto di **liquidita'** (chi deve comprare o vendere molto a breve), non sui fondamentali (HHW §3.1,
§3.3), e il flusso delle opzioni su indici e' **copertura o assicurazione** piu' che informazione privata
(CKMS; PP e CLZZ nell'altra ricerca). Nessuna delle tre dice che i livelli delle opzioni di un indice muovano
l'altro. Per DIC il perche' e' il rendimento da utili (piu' rischio atteso, prezzo piu' basso subito); la ragione
**nostra**, che il paper non nomina, e' (d) **gli orari**: due chiusure con la stessa data non sono lo stesso
istante, e un "anticipo" fra mercati con fusi diversi puo' essere informazione contemporanea registrata il
giorno dopo.

### Cosa Si Rifa' Su NQ Con ATAS, E Cosa No

- **Chi guida fra ES e NQ** (HAS): VAR a un secondo sui prezzi medi del tape di ATAS, una stima per giornata,
  quote d'informazione con minimo e massimo, prova degli orologi (ritardi apposta di 1-5 secondi), nullo con le
  passeggiate casuali e con giornate scambiate. **Solo 2023-2024** per non consumare il fuori campione di NQ;
  per ES la divisione va fissata prima.
- **Quanto il flusso di ES spiega NQ in piu'** (LMCB): dR2 stimato sul 2023 e misurato sul 2024, nei due
  versi, per giornata, con il nullo delle giornate scambiate; e, dichiarata prima, la variante con il flusso
  di ES della finestra prima.
- **Il controllo negativo di CKMS** con le sole barre: lo sbilancio aggressivo settimanale di NQ contro la
  settimana stessa e la dopo, con il delta delle barre M1 dal 2017 (circa 400 settimane dentro il campione,
  nostro conto).
- **Il flusso delle opzioni di QQQ o NDX, no**: ne' per scambio (HHW, OPRA) ne' per gruppo (CKMS, ISE o Cboe
  Open-Close). La catena CME di ATAS non dice chi ha comprato; se porti gli scambi delle opzioni di NQ con il
  lato e' **da verificare**.
- **VIX (o VXN) contro NQ, alla DIC: no, per ora.** Il VIX ufficiale da Yahoo e' solo contesto nella pagina X-Ray
  (CLAUDE.md, decisione 2), e VIX e VXN ricostruiti dalla catena CME ci sono da poco e cadono nel fuori campione
  di NQ. Se richiesto, con i due accorgimenti che DIC non usa: la variazione del VIX **misurata alla stessa ora**
  del rendimento di NQ che si vuole spiegare (gli orari nel fuso del mercato, come vuole il repository), e il
  mercato guida **anche ritardato** (ES o S&P di ieri) nel modello; una osservazione per giornata, nullo con
  giornate rimescolate, solo dentro il campione. La regola di trading di DIC si scarta.
- **Due vincoli**: usare il future ES per leggere NQ esce dal perimetro di ES (solo lo studio delle opzioni,
  CLAUDE.md, decisione 2) e **serve la richiesta esplicita dell'utente**; la gamma di SPX c'e' solo da
  ottobre 2026, quindi ogni prova "gamma di SPX -> NQ" cade nel fuori campione di NQ e si registra in avanti.

### Le Idee Candidate

Nessuna e' una strategia; la scheda non e' stata chiesta e non si scrive.

| | finding da provare | cosa la ucciderebbe |
|---|---|---|
| F | Il flusso aggressivo di ES spiega il prezzo di NQ nella stessa finestra **piu'** di quanto NQ spieghi ES (LMCB, condizione del piu' liquido) | dR2 fuori campione (2024) non diverso dal nullo delle giornate scambiate, oppure nessuna asimmetria fra i due versi |
| G | La variante, dichiarata prima: il flusso di ES della finestra prima spiega NQ della finestra dopo | Niente sopra il nullo a un minuto (HAS fa attendere che la guida si spenga in pochi secondi) |
| H | La quota d'informazione fra ES e NQ a un secondo ha un verso stabile (HAS) | Quote che cambiano verso con 1-2 secondi di ritardo apposta, o simmetriche come nel nullo |
| I | La trasmissione da ES a NQ (dR2) e' piu' grande nei giorni di gamma negativa dei dealer di SPX | Non rifacibile ora: la gamma c'e' da ottobre 2026 e su NQ e' fuori campione; registro in avanti, a regole congelate |
| J | Le put dei piccoli su indici diversi dall'S&P 500 anticipano l'S&P 500 al contrario, col VIX alto (CKMS) | Non rifacibile: servono gli sbilanci di apertura per lato del market maker sulle opzioni NDX (a pagamento) e anni di settimane; su SPX il paper dice gia' che non c'e' niente |
| M | La variazione del VIX (o del VXN), misurata alla stessa ora, anticipa il rendimento di NQ del giorno dopo oltre il rendimento di ES di ieri (DIC, rifatto con gli orari allineati) | Il coefficiente sparisce aggiungendo ES di ieri, o allineando gli istanti; gia' oggi le Americhe di DIC (Wald da 7 a 35 contro 166-667) dicono che il vantaggio si riduce molto quando gli orari coincidono. Fuori perimetro: il VIX e' solo contesto, serve la richiesta dell'utente |

**Per lo studio globale unico di NQ ed ES**: si giustifica come **lettura del contesto comune** (il premio di
rischio comune, CKMS; il prezzo che nasce sui future, HAS e LMCB), **non come regola**: nessun paper prova che i
livelli o la gamma di un indice servano a leggere l'altro, e la guida fra future, se c'e', sta in secondi. E una
lezione di metodo da DIC, per ogni prova fra indici: **si allineano gli istanti, non le date**, e il mercato
guida si mette anche ritardato; un "anticipo" forte dove gli orari non coincidono e debole dove coincidono e'
un indizio di orologio, non di informazione.

**Quale studio aprire.** Se uno, la misura di **chi guida fra ES e NQ** (H, poi F): e' rifacibile con il tape
di ATAS dentro il campione, e risponde alla domanda di questa ricerca come lettura. Richiede pero' la
richiesta esplicita dell'utente (ES fuori dallo studio delle opzioni) e la divisione di ES fissata prima. Non
si apre senza.

## Il Diario

- **9 novembre 2026**, fase 1: scelti quattro candidati. PDF libero: le-coz (arXiv) e hasbrouck
  (versione di lavoro NYU). Col browser e la persona: chordia (SSRN, Cloudflare) e holowczak (SSRN,
  oppure la copia SMU dietro un controllo anti-bot).
- **9 novembre 2026**, fase 2: scaricati con curl le-coz (arXiv v2, 2,4 MB, il PDF dichiara 26 marzo
  2024) e hasbrouck (versione di lavoro NYU del 16 novembre 2000, 190 KB), entrambi PDF veri, con lo
  sha256 nella tabella; nessuna istruzione nella prima pagina. Chordia (SSRN 2798390): Playwright apre
  la pagina dell'abstract e Cloudflare mostra "Esecuzione della verifica di sicurezza" (HTTP 403): fermo,
  non aggirato, serve la persona nella finestra di Chrome aperta. Holowczak (SSRN) non ancora tentato,
  stessa barriera attesa. Restano `trovato`.
- **9 novembre 2026**, fase 3: letto per intero le-coz (arXiv v2, 18 pagine, appendici comprese),
  [analisi](../opzioni-gamma/le-coz-2024-when-is-cross-impact-relevant/analisi.md), stato `letto`.
  Due correzioni a quanto scritto in fase 1: il modello **spiega il prezzo con il flusso della stessa
  finestra**, non prevede; e il Nasdaq-100 **non e' nominato** nel paper (dei future su indici c'e'
  solo l'E-mini S&P). Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero hasbrouck (versione di lavoro NYU del 16 novembre 2000,
  59 pagine, non la pubblicata), `analisi.md` nella sua cartella, stato `letto`. **Il "da verificare"
  e' risolto in negativo**: S&P 500 e Nasdaq-100 sono stimati separatamente (E-mini in testa, ES
  0,854-0,882 e NQ 0,836-0,853 di quota d'informazione, 64 giornate del 2000); l'unico numero
  incrociato e' una frase, QQQ verso SPY "al massimo circa il 10%", senza tabella e senza la direzione
  opposta. Serve come metodo: nell'analisi, come si misurerebbe chi guida fra ES e NQ con il tape di
  ATAS (VAR a un secondo, una stima per giornata, prova degli orologi, nullo), solo sul 2023-2024 per
  non consumare il fuori campione di NQ. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificato hasbrouck contro il PDF, stato `analizzato`. Circa 140
  affermazioni controllate (83 numeri delle tabelle, tutti giusti), 11 correzioni, nessuna tolta, un
  limite aggiunto. La piu' grave: "il primato degli E-mini regge a errori di 5-10 secondi" e' il
  riassunto dell'autore, ma la sua Tab. 7 dice che sull'S&P a 10 secondi il future regolare e' gia'
  davanti (SP 0,449-0,684 contro ES 0,214-0,469); regge a 10 solo su NQ. Aggiunto: settori e QQQ sono
  misurati contro SPY, non contro l'E-mini che guida. Le eq. 10 e 12 restano ricostruite (pagine non
  visibili come immagine). Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificato le-coz contro il PDF, stato `analizzato`. Circa 105
  affermazioni controllate, 93 confermate, 11 correzioni, una tolta come affermazione del paper (resta
  come lettura nostra), un limite aggiunto. La piu' grave: l'analisi riportava solo il condizionale
  dell'App. B.2 ("sarebbe invalidato"), mentre il §IV.B e il §VII dicono al presente che
  l'autocorrelazione del flusso "invalida" il modello lineare. Confermati i dubbi del lettore: la
  radice di matrice dell'eq. 9 (verificata sul glifo del font del PDF), le celle 27,0 e 45,2 della
  Fig. 18 e 18,9 e 4,2 della Fig. 17, le unita' delle pendenze non dichiarate, la coppia "E-mini S&P
  future (3M) - E-mini S&P future" della Fig. 9, il Nasdaq-100 assente. Aggiunto: i massimi sulle
  durate sono presi fuori campione, quindi probabilmente ottimisti. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 2, ripresa: la persona ha superato la verifica sulla pagina dell'abstract
  di chordia (SSRN 2798390), che si e' aperta. Il clic su "Download This Paper" porta a `Delivery.cfm`,
  dove Cloudflare mostra una seconda "Esecuzione della verifica di sicurezza" (HTTP 403): fermo di nuovo,
  non aggirato. Nessun PDF salvato; chordia e holowczak restano `trovato`.
- **9 novembre 2026**, fase 2, seconda ripresa (dalla ricerca sul flusso per gruppo): la persona ha superato
  la verifica nella finestra di Playwright; nello stesso browser il clic su "Download This Paper" ha scaricato
  senza una seconda verifica. Chordia: versione del 17 ottobre 2019, "Management Science, forthcoming", 59
  pagine. Holowczak: il PDF di SSRN e' il working paper *Consolidating Information in Option Transactions*,
  "First draft: January 13, 2011" (SSRN: Date Written 13 gennaio 2011), 28 pagine, non la versione
  pubblicata nel Journal of Derivatives; l'abstract parla di **QQQQ** (il vecchio ticker di QQQ) e di delta e
  vega come pesi. Sha256 nella tabella; nessuna istruzione nei PDF.
- **9 novembre 2026**, fase 3: letto per intero holowczak (working paper SSRN 1740046, prima stesura del 13
  gennaio 2011, 28 pagine, **non la versione pubblicata** nel Journal of Derivatives 2014, i cui numeri possono
  differire), [analisi](../opzioni-gamma/holowczak-2014-aggregating-information-in-option-transactions/analisi.md),
  stato `letto`. Due correzioni alla fase 1: il flusso di QQQQ (2006, OPRA, un minuto) va con il rendimento
  **nello stesso minuto** (fino a 0,41, *delta per share* con log(volume)) ma sul minuto dopo le correlazioni
  sono intorno a 0,01 (Tab. 3B), significative solo trattando gli 85.000 minuti come indipendenti; e la misura
  proposta (HHW) pesa per n(d1)/M, non per il delta. Un solo indice: niente fra indici. Figure non leggibili
  (manca `pdftoppm`). Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero chordia (versione accettata del 17 ottobre 2019, "Management
  Science, forthcoming", non la pubblicata; 59 pagine con l'Internet Appendix),
  [analisi](../opzioni-gamma/chordia-2021-index-option-trading-activity-and-market-returns/analisi.md), stato
  `letto`. **Il "da verificare" della fase 1 e' corretto**: gli indici non sono "ISE 250, Russell 2000, Nasdaq
  100 e S&P 400" (riassunto di terzi) ma **RUT e NDX sommati** in un solo sbilancio, e dal 22 aprile 2013 (RUT
  tolto dall'ISE) **solo NDX**; nessuna stima di NDX da solo. Il controllo del flusso di ES e di SPY c'e' (Tab.
  6, 7): non predicono; le opzioni SPX nemmeno (IRF 0,02, errore 0,30). Predicono le **put**, al contrario
  (piu' put comprate, S&P 500 piu' alto), a una-due settimane, soprattutto con il VIX nel quartile alto, e gli
  autori lo leggono come premio per l'assicurazione, non come informazione. Tabelle con `pdftotext -table`;
  figure non viste. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di holowczak sul PDF (sha256 confermato), stato
  `analizzato`. Circa 140 affermazioni controllate, 7 corrette, nessuna tolta, 4 aggiunte. Confermati i numeri
  delle tabelle 3 e 4 (0,4112; minuto dopo fra -0,0035 e 0,0110; VBP 0,0205 e 0,0203) e le stelle, che
  coincidono con 85.470 minuti trattati come indipendenti. La correzione piu' importante: "per giornata l'errore
  sarebbe ben piu' grande" non segue dall'indipendenza (riscritto come dipendenza non controllata); poi il
  MA(1) "for each trading day" (look-ahead solo dal coefficiente), la 3B fra -0,0035 e 0,0110, la radice della
  formula 10 ricavata dal testo. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di chordia sul PDF (sha256 confermato), stato
  `analizzato`. Circa 160 affermazioni controllate, circa 145 confermate, 15 corrette o precisate, nessuna
  tolta, 2 aggiunte. Confermati gli indici (RUT e NDX sommati, solo NDX dal 22 aprile 2013, nota 11), 615
  settimane, +0,22% (Tab. 2), +0,55% in 10 settimane (Fig. 3), 0,0235 p 0,41 contro 0,2870 (Tab. 4), Tab. 6,
  7, 9, A1, A2. Figure rese come immagini con pypdfium2. La correzione piu' grave: la Fig. 2 smentisce il
  "primo e ultimo quintile simili" del testo (-0,6% contro +1,2% a due settimane), e a una settimana l'effetto
  sta nel quintile delle put piu' comprate; poi la risposta della Fig. 3 che parte negativa (-0,22), l'IRF
  NYSE 0,70 in grassetto in Tab. 7 non commentata, un secondo refuso in Tab. 5. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 5: scritta [La Sintesi](#la-sintesi) sulle quattro analisi verificate (CKMS, HHW,
  LMCB, HAS), lette per intero con la loro sezione "La Verifica". Nessun paper prova un effetto incrociato
  della gamma o dei livelli; il legame fra indici e' comune e contemporaneo. Chordia e altri con gli indici
  veri (RUT e NDX sommati, solo NDX dal 22 aprile 2013, nessuna stima del solo NDX; SPX non predice);
  Holowczak e altri come metodo (stesso minuto, non dopo). Cinque idee candidate (F-J), nessuno studio aperto:
  il candidato e' "chi guida fra ES e NQ", che richiede la richiesta esplicita dell'utente. Indice aggiornato.
- **9 novembre 2026**, fase 1, supplemento: **SSRN aperto** nella finestra di Playwright, dopo la verifica
  superata dalla persona. 18 query con la ricerca del sito (`searchresults.cfm?term=`) e le pagine References e
  Citations di chordia e holowczak. Otto candidati nuovi verificati sulla pagina SSRN (titolo, autori, date,
  abstract, numero); **nessuno prova un effetto incrociato della gamma o del flusso fra NDX/QQQ e S&P 500**.
  Scelto uno solo, dicle (SSRN 3057745, VIX contro 20 indici esteri, PDF su SSRN); gli altri scartati con il
  motivo, due da tenere (figlewski, senza PDF libero; cenobar, gamma di NDX). Nessun PDF scaricato. Nessuna
  istruzione nelle pagine.
- **9 novembre 2026**, fase 2, terzo giro: scaricato dicle da SSRN nella stessa finestra di Playwright, dopo
  la verifica superata dalla persona: working paper del 3 novembre 2017 ("Preprint submitted to Working Paper
  Series"), non la versione di Quantitative Finance and Economics; 20 pagine; sha256 nella tabella; nessuna
  istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero dicle (working paper SSRN 3057745 del 3 novembre 2017, 20
  pagine, **non la versione pubblicata** in Quantitative Finance and Economics),
  [analisi](../opzioni-gamma/dicle-2017-us-implied-volatility-predictor-international-returns/analisi.md),
  stato `letto`. Confermati i numeri dell'abstract (VIX di ieri significativo per 18 indici su 20, Granger
  per 17, chiusure giornaliere 2000 - luglio 2017, Yahoo e Cboe), ma **il paper non tratta i fusi orari**:
  l'effetto e' grande in Asia ed Europa (Wald fino a 667) e piccolo o nullo nelle Americhe, che chiudono con
  New York; il controllo dell'S&P 500 e' solo dello stesso giorno, non di ieri; la regola di trading compra
  in Asia una chiusura gia' passata quando arriva il segnale (deduzioni nostre). Nessun campione fuori,
  nessun costo, **niente su Nasdaq o VXN**. Serve come metodo: allineare gli istanti e mettere il mercato
  guida ritardato. Tabelle lette sulle pagine rese come immagini. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificato dicle contro il PDF, stato `analizzato`. Circa 115
  affermazioni controllate, circa 103 confermate, 12 corrette o riformulate, nessuna tolta, un limite
  aggiunto. Le tabelle 1-4 rilette sulle immagini: righe della Tab. 2 attribuite giuste (Messico -0,0048 **,
  Nuova Zelanda -0,0013 non significativa), 17 su 20 in Granger, 6 nella direzione inversa, 15 in Tab. 4 con
  le colonne "≠" e ">". La piu' grave: "il vantaggio di un giorno sparisce quando gli orari coincidono" e'
  smentito dal paper (Brasile, Canada e Cile restano significativi): ora "si riduce di molto", come lettura
  nostra. Fusi orari, S&P di ieri mancante, look-ahead e SUR restano deduzioni, ora marcate come tali
  ovunque, con il presupposto non detto delle date di calendario. Aggiunto: la crisi entra solo come
  costante. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 5, aggiornamento: [La Sintesi](#la-sintesi) aggiornata con **DIC** (dicle), letto per
  intero con la sua sezione "La Verifica". Il VIX "anticipa" di un giorno le borse estere che chiudono prima di New
  York (Asia, Europa); nelle Americhe l'anticipo si riduce molto ma non sparisce (Brasile, Canada e Cile restano
  significativi: correzione della verifica). Nasdaq e VXN assenti; la lettura dei fusi orari e' nostra. Aggiunti il
  finding, una convergenza (comune e contemporaneo, anche con gli orari), una contraddizione (un giorno o niente),
  il perche' degli orari, la riga su VIX contro NQ e l'idea candidata M, e la lezione di metodo (allineare gli
  istanti, mettere il mercato guida ritardato). Stati di le-coz e hasbrouck portati ad `analizzato`. Nessuno
  studio aperto. Indice aggiornato.
