# Indice Dei Paper

I paper da cui vengono le idee (fase 1), e quelli da cui il framework prende un metodo. La fonte
del metodo, il transcript, sta in [`quant/metodo/fonti/`](../metodo/fonti/). Come si cercano, si scaricano e si
archiviano: [`quant/metodo/1-come-si-cercano-le-idee.md`](../metodo/1-come-si-cercano-le-idee.md).

## Come Sono Organizzati

```text
paper/
  indice-dei-paper.md                 questo file: tutti i paper, per filone, con lo stato
  <filone>/                           un filone di ricerca: prop-firm, momento-intraday, ...
    <chiave>/                         un paper: autore-anno-titolo-breve
      analisi.md                      la scheda bibliografica (in testa) e la nostra lettura
      <chiave>.pdf                    il PDF letto, fuori da git; il suo sha256 e' nella scheda
```

- **Il filone** raggruppa i paper che rispondono alla stessa domanda. Un paper ha un filone
  principale; se ne tocca altri, la scheda li elenca.
- **La chiave** e' il nome stabile del paper: cognome del primo autore, anno, titolo breve, in
  minuscolo e con i trattini. E' anche il nome della cartella e del PDF, e si cita cosi' nelle schede
  delle idee.
- **Il ruolo**: `idea` se il paper porta un finding da provare (fase 1), `metodo` se porta una
  tecnica che il framework usa (per esempio lo Sharpe deflazionato nella fase 5).
- **Lo stato**: trovato -> scaricato -> letto (per intero) -> analizzato. Un paper solo trovato sta
  in questo indice e non ha ancora una cartella.
- **Lo sha256 del PDF** dice quale versione e' stata letta: su SSRN i paper vengono rivisti, e
  un'analisi vale per la versione che ha letto.
- **Paper e studi sono separati**: uno studio puo' venire da piu' paper (i tre sulle prop firm fanno
  lo studio `contratto-prop`) e un paper puo' servire a piu' studi. La scheda dello studio elenca i
  paper; la scheda del paper elenca gli studi.

## prop-firm

Il valore del contratto di una prop firm per chi lo compra. Tre paper indicati dall'utente l'11
ottobre 2026, tutti preprint su SSRN, non revisionati. Studio: [`contratto-prop`](../studi/contratto-prop/1-idea.md).

| sigla | paper | autore, versione | ruolo | stato |
|---|---|---|---|---|
| TF | [*Valuing Proprietary Trading Firm Evaluation Contracts*](prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md) | Marti Tomas Fernandez, agosto 2026 | idea, e metodo (forma chiusa, sigillo dei criteri) | analizzato |
| L | [*The Price of a Funded Account*](prop-firm/lim-2026-price-of-a-funded-account/analisi.md) | Boon Chuan Lim, 25 luglio 2026 | idea | analizzato |
| Y | [*The Inherent Value of Retail Prop Firms for Fixed-Size Trading Strategies*](prop-firm/youngblood-2026-inherent-value-of-retail-prop-firms/analisi.md) | Zachary Youngblood, 21 agosto 2026 | idea | analizzato |
| | *Phantom Generosity: Contract Design and Value in the Market for Proprietary-trading Evaluations* ([SSRN 7184138](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7184138)) | Boon Chuan Lim | idea | trovato |
| | *Prop-Firm Challenges: A Barrier Model of Pass Rates and Expected Value* ([SSRN 7445798](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7445798)) | Ruben Villahermosa | idea | trovato |

Lo studio che li usa: [`tradeify-growth-50k`](../studi/contratto-prop/5-validazione/risultati/2026-10-11-tradeify-growth-50k.md).

## Bitcoin: I Punti Di Fabio

**Fuori dallo scope di oggi** (NQ, solo NQ): cercati il 14 ottobre 2026 su richiesta esplicita
dell'utente, per verificare cio' che Fabio dice di Bitcoin nell'intervista su YouTube
(`fabio_course/fabio_youtube_interview.txt`). E' solo la fase 1, **stato: trovato** per tutti:
esistenza verificata sulla pagina del paper, o su una seconda fonte (Crossref, IDEAS, arXiv) quando
SSRN o l'editore rispondono 403. **Cosa dicono viene dall'abstract** o, dove segnato, da un
riassunto: non e' una lettura. Nessun PDF scaricato.

Sigle: **R** rivista con revisione fra pari; **P** preprint o working paper. **PDF libero**: arXiv,
NBER, BIS o la pagina dell'autore; altrimenti SSRN (si scarica con Playwright, la verifica la fa
l'utente) o a pagamento.

### Cosa Dicono, Punto Per Punto

| Fabio dice | cosa dice la ricerca trovata (dagli abstract) |
|---|---|
| il **drift dopo l'halving** e' "matematicamente misurabile" | **debole e in calo.** Un solo disegno causale: effetto per il 2024, nessuno robusto per il 2020 (Virtonen). Un paper revisionato trova che l'halving deprime leggermente il prezzo (Lashkaripour). Gli halving sono quattro: nessun test ha potenza |
| l'**order flow di Bitcoin** e' molto piu' preciso che sul Nasdaq | **in parte.** Il flusso firmato spiega i rendimenti nello stesso intervallo (Makarov e Schoar, 80%), ma prevedere e' un'altra cosa: 10-37% a 500 millisecondi, e dipende dalle commissioni (Albers e altri); lo squilibrio ai quarti d'ora prevede circa 0,5 punti base (Kim e Hansen, da un riassunto). Il test piu' diretto conclude che le dinamiche sono **simili** a quelle degli altri mercati (Easley e altri). **Nessun paper confronta Bitcoin e NQ con lo stesso metodo** |
| il mercato cripto e' **inefficiente**, nuovo | **lo era.** Inefficiente fino al 2016-2017, poi sempre piu' efficiente; l'inefficienza resta alle frequenze piu' alte (Urquhart, Sensoy, Tran e Leirvik). Gli arbitraggi fra borse sono quasi inutilizzabili dal 2018 (Crepelliere e altri) |
| **arbitraggi** con "buchi incredibili" | il **carry** (future meno spot) arriva al 40-60% l'anno, nasce dalla domanda di leva dei piccoli e dal capitale di arbitraggio scarso, e **un carry alto predice i crolli** (Schmeling e altri); le deviazioni dei perpetual si restringono nel tempo (He e altri) |
| la **leva sul profitto** gia' fatto; il bull run che "si mangia tutto" | le liquidazioni forzate sono il 3,5% dei future aperti al giorno per i long, con una leva media di 60x (Cheng e altri); nessun precursore comune alle sette cascate del 2022-2025 (Garcia Seuma); le bolle si riconoscono, il momento del crollo no (Sornette e altri) |
| i **dati on-chain** prevedono il valore "con una precisione che fa paura" | **no, fuori campione.** I predittori noti non prevedono fuori campione, il migliore ha R² del 2,7% al giorno (Yae e Tian); nessuno studio revisionato batte la previsione ingenua a 1-6 mesi (Baquero, P). Il caso migliore: MVRV Z-score su tre cicli, Sharpe da 0,45 a 1,28, e la soglia non e' scattata nel 2022-2025 (Grobys e altri) |
| lo **swing sulle cripto** ha un profit factor "da pazzi" | il momentum di serie storica e' forte nelle cripto (Liu e Tsyvinski); le regole tecniche funzionano sulle altre monete ma **su Bitcoin non prevedono fuori campione** (Hudson e Urquhart); dopo costi e test multipli battono il buy-and-hold soprattutto corretto per il rischio (Deprez e Frommel). **Nessun paper riporta il profit factor** ne' confronta con lo scalping sul Nasdaq |
| le **opzioni comprimono il Nasdaq**, Bitcoin si muove | sull'indice S&P i risultati sono **opposti**: gli 0DTE abbassano la volatilita' (Adams e altri; Dim, Eraker e Vilkov) o la alzano (Brogaard, Han e Won). Il meccanismo: i dealer lunghi di gamma la smorzano, corti la amplificano (Ni e altri). **Nessun paper su NDX o NQ**, nessun confronto con Bitcoin. Vedi il filone `opzioni-gamma` |

### bitcoin-halving

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Estimating the Impact of the Bitcoin Halving on Its Price Using Synthetic Control* ([arXiv 2511.05512](https://arxiv.org/abs/2511.05512)) | Virtonen, 2025 | P | effetto causale positivo nel 2024 (circa un quinto del rialzo), nessuno robusto nel 2020 | libero |
| *Some stylized facts about bitcoin halving* ([doi](https://doi.org/10.1016/j.frl.2024.106198), SSRN 4840860) | Lashkaripour, 2024 | R, Finance Research Letters | gli halving deprimono leggermente il prezzo, abbassano la volatilita' | SSRN |
| *Is Bitcoin's Market Maturing? Cumulative Abnormal Returns and Volatility in the 2024 Halving and Past Cycles* ([doi](https://doi.org/10.3390/jrfm18050242)) | Veloso e altri, 2025 | R, JRFM (MDPI) | rendimenti e volatilita' intorno agli halving in calo, con l'arrivo degli istituzionali | libero |
| *Issuance Shocks in Mature Crypto Markets: An Event-Study Analysis of the Bitcoin Halving* ([SSRN 6589402](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=6589402)) | Lim, 2026 | P | nel quarto ciclo rendimenti anomali negativi; dopo gli ETF l'halving non conta. Lo stesso Lim dei paper sulle prop firm | SSRN |
| *Bitcoin Halvings and Institutional Investors: A Wavelet Analysis* ([doi](https://doi.org/10.13133/2037-3643/18744)) | Camacho, da Silva, 2026 | R, PSL Quarterly Review | il legame prezzo-transazioni e' piu' forte sugli orizzonti lunghi | libero |
| *Can wavelets produce a clearer picture of weak-form market efficiency in Bitcoin?* ([doi](https://doi.org/10.1007/s40822-022-00214-8)) | Phiri, 2022 | R, Eurasian Economic Review | prevedibilita' a cicli di 2-16 giorni, nelle bolle, fra halving consecutivi | a pagamento |
| *Fair market value of bitcoin: halving effect* ([doi](https://doi.org/10.21511/imfi.16(4).2019.07)) | Meynkhard, 2019 | R, rivista minore | l'halving alza il valore; metodo debole | libero |

### bitcoin-leva-e-liquidazioni, bitcoin-bolle

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Crypto Carry* ([BIS WP 1087](https://www.bis.org/publ/work1087.htm), SSRN 4268371) | Schmeling, Schrimpf, Todorov, 2023-2025 | R, Management Science (pubblicazione da confermare) | carry fino al 40-60% l'anno; un carry alto predice i crolli | libero (BIS) |
| *Liquidation, leverage and optimal margin in bitcoin futures markets* ([arXiv 2102.04591](https://arxiv.org/abs/2102.04591)) | Cheng, Deng, Wang, Yu, 2021 | R, Applied Economics | liquidazioni giornaliere 3,51% dei long, 1,89% degli short; leva media 60x | libero |
| *Towards Understanding Cryptocurrency Derivatives: A Case Study of BitMEX* ([doi](https://doi.org/10.1145/3442381.3450059)) | Soska e altri, 2021 | R, WWW '21 | i trader esperti battono i nuovi, liquidati in proporzione maggiore | libero (CMU) |
| *Where does the criticality live? Early-warning signals are event-heterogeneous across seven crypto-perpetual liquidation cascades* ([arXiv 2607.27070](https://arxiv.org/abs/2607.27070)) | Garcia Seuma, 2026 | P | nessun segnale anticipa tutte e sette le cascate; Binance BTCUSDT a 1 minuto | libero |
| *Two-Regime Liquidity Recovery After a Perpetual Futures Liquidation Cascade* ([doi](https://doi.org/10.34900/jfda.v1i2.1395)) | Lim, 2026 | rivista nuova, revisione incerta | dopo il 10 ottobre 2025 lo spread torna in 23 giorni, la profondita' no | libero |
| *Dissection of Bitcoin's multiscale bubble history from January 2012 to February 2018* ([arXiv 1804.06261](https://arxiv.org/abs/1804.06261)) | Gerlach, Demos, Sornette, 2019 | R, Royal Society Open Science | gli indicatori LPPLS avvertivano prima dei picchi principali | libero |
| *Are Bitcoin bubbles predictable? Combining a generalized Metcalfe's Law and the LPPLS model* ([arXiv 1803.05663](https://arxiv.org/abs/1803.05663)) | Wheatley e altri, 2019 | R, Royal Society Open Science | avvertimenti si', momento esatto del crollo no | libero |
| *Real-time prediction of Bitcoin bubble crashes* ([arXiv 1905.09647](https://arxiv.org/abs/1905.09647)) | Shu, Zhu, 2020 | R, Physica A | LPPLS multiscala su dati a 1 ora e 30 minuti meglio del giornaliero | libero |
| *Identifying price bubble periods in the Bitcoin market-based on GSADF model* ([doi](https://doi.org/10.1007/s11135-020-01077-4)) | Li e altri, 2021 | R, Quality & Quantity | quattro bolle lunghe e quattro corte, 2011-2020 | a pagamento |
| *Bubble detection in Bitcoin and Ethereum and its relationship with volatility regimes* ([doi](https://doi.org/10.1108/JES-09-2021-0452)) | Diniz e altri, 2023 | R, Journal of Economic Studies | le bolle coincidono con il regime di bassa volatilita' | a pagamento |

Escluso: Corbet, Lucey, Yarovaya (2018), *Datestamping the Bitcoin and Ethereum bubbles*, **ritirato**.

### bitcoin-order-flow, bitcoin-price-discovery, bitcoin-efficienza

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Microstructure and market dynamics in crypto markets* ([SSRN 4814346](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4814346)) | Easley, O'Hara, Yang, Zhang, 2026 | R, Journal of Financial Markets | VPIN e Roll predicono la dinamica dei prezzi; dinamiche "simili a quelle delle altre classi" | SSRN, copia Cornell |
| *Fragmentation, Price Formation and Cross-Impact in Bitcoin Markets* ([arXiv 2108.09750](https://arxiv.org/abs/2108.09750)) | Albers e altri, 2021 | R, Applied Mathematical Finance | book e trade spiegano il 10-37% dei rendimenti a 500 ms; il guadagno dipende dalle commissioni | libero |
| *The Quarter-Hour Effect: Periodic Algorithmic Trading and Return Predictability in Cryptocurrency Futures* ([arXiv 2607.09426](https://arxiv.org/abs/2607.09426)) | Kim, Hansen, 2026 | P | lo squilibrio ai quarti d'ora predice 4-12 ore, fuori campione; perpetual Binance | libero |
| *Trading and arbitrage in cryptocurrency markets* ([doi](https://doi.org/10.1016/j.jfineco.2019.07.001), SSRN 3171204) | Makarov, Schoar, 2020 | R, Journal of Financial Economics | la parte comune del volume firmato spiega l'80% dei rendimenti (nello stesso intervallo); arbitraggi grandi fra paesi | libero (LSE) |
| *Order flow analysis of cryptocurrency markets* ([doi](https://doi.org/10.1007/s42521-019-00007-w)) | Silantyev, 2019 | R, Digital Finance | lo squilibrio dei trade spiega il prezzo meglio di quello del book; BitMEX | a pagamento |
| *Bitcoin wild moves: Evidence from order flow toxicity and price jumps* ([doi](https://doi.org/10.1016/j.ribaf.2025.103163)) | Kitvanitphasu e altri, 2026 | R, RIBAF | il VPIN predice i salti di prezzo | libero |
| *Informed trading in the Bitcoin market* ([doi](https://doi.org/10.1016/j.frl.2017.11.009)) | Feng, Wang, Zhang, 2018 | R, Finance Research Letters | ordini grandi anomali 1-2 giorni prima degli eventi | a pagamento |
| *A Million Metaorder Analysis of Market Impact on the Bitcoin* ([arXiv 1412.4503](https://arxiv.org/abs/1412.4503)) | Donier, Bonart, 2015 | R, Market Microstructure and Liquidity | legge della radice quadrata dell'impatto | libero |
| *Price discovery in bitcoin spot or futures?* ([SSRN 3171464](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3171464)) | Baur, Dimpfl, 2019 | R, Journal of Futures Markets | lo spot guida i future CME e CBOE | SSRN |
| *Price discovery in Bitcoin: The impact of unregulated markets* ([SSRN 3583843](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3583843)) | Alexander, Heck, 2020 | R, Journal of Financial Stability | guidano i derivati non regolati; il CME reagisce | SSRN |
| *Bitcoin spot and futures market microstructure* ([SSRN 3459111](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3459111)) | Aleti, Mizrach, 2021 | R, Journal of Futures Markets | il CME guida; trade medio CME 18.000 $ contro 1.300 $ sullo spot | SSRN |
| *BitMEX bitcoin derivatives: price discovery, informational efficiency and hedging effectiveness* ([doi](https://doi.org/10.1002/fut.22050)) | Alexander, Choi, Park, Sohn, 2020 | R, Journal of Futures Markets | i derivati BitMEX guidano lo spot | libero (Sussex) |
| *The Role of Binance in Bitcoin Volatility Transmission* ([SSRN 3877949](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3877949)) | Alexander, Heck, Kaeck, 2022 | R, Applied Mathematical Finance | il perpetual Binance e' la fonte principale della volatilita' | libero |
| *An analysis of price discovery between Bitcoin futures and spot markets* ([doi](https://doi.org/10.1016/j.econlet.2018.10.031)) | Kapar, Olmo, 2019 | R, Economics Letters | guidano i future (contro Baur e Dimpfl) | a pagamento |
| *The determinants of price discovery on bitcoin markets* ([doi](https://doi.org/10.1002/fut.22101)) | Entrop, Frijns, Seruset, 2020 | R, Journal of Futures Markets | chi guida cambia nel tempo, con costi e volumi | a pagamento |
| *Trading activity and price discovery in Bitcoin futures markets* ([doi](https://doi.org/10.1016/j.jempfin.2021.03.001)) | Hung, Liu, Yang, 2021 | R, Journal of Empirical Finance | il CME meglio del CBOE; il retail peggiora la scoperta del prezzo | a pagamento |
| *The inefficiency of Bitcoin* ([SSRN 2828745](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2828745)) | Urquhart, 2016 | R, Economics Letters | inefficiente, ma si muove verso l'efficienza | SSRN |
| *The inefficiency of Bitcoin revisited: A high-frequency analysis with alternative currencies* ([doi](https://doi.org/10.1016/j.frl.2018.04.002)) | Sensoy, 2019 | R, Finance Research Letters | piu' efficiente dal 2016; meno efficiente alle frequenze piu' alte | a pagamento |
| *Efficiency in the markets of crypto-currencies* ([doi](https://doi.org/10.1016/j.frl.2019.101382)) | Tran, Leirvik, 2020 | R, Finance Research Letters | inefficienti prima del 2017, poi piu' efficienti | libero |
| *On the evolution of cryptocurrency market efficiency* ([arXiv 1904.09403](https://arxiv.org/abs/1904.09403)) | Noda, 2021 | R, Applied Economics Letters | efficienza variabile, BTC piu' di ETH | libero |
| *The adaptive market hypothesis in the high frequency cryptocurrency market* ([doi](https://doi.org/10.1016/j.irfa.2019.05.008)) | Chu, Zhang, Chan, 2019 | R, IRFA | efficienza variabile, non spiegata dalle notizie | a pagamento |
| *Bitcoin intraday time series momentum* ([doi](https://doi.org/10.1111/fire.12290)) | Shen, Urquhart, Wang, 2022 | R, Financial Review | la prima mezz'ora predice l'ultima, come il momento intraday degli indici | libero (Reading) |
| *Intraday return predictability in the cryptocurrency markets: Momentum, reversal, or both* ([SSRN 4080253](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4080253)) | Wen, Bouri, Xu, Zhao, 2022 | R, NAJEF | momento e inversione intraday, a seconda di salti e FOMC | SSRN |
| *Rise of the machines? Intraday high-frequency trading patterns of cryptocurrencies* ([arXiv 2009.04200](https://arxiv.org/abs/2009.04200)) | Petukhina, Reule, Hardle, 2021 | R, European Journal of Finance | schemi intraday legati al trading algoritmico | libero |

### bitcoin-arbitraggio, bitcoin-basis-e-funding, crypto-arbitraggio-statistico

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Arbitrage in the Market for Cryptocurrencies* ([doi](https://doi.org/10.1016/j.finmar.2023.100817)) | Crepelliere, Pelster, Zeisberger, 2023 | R, Journal of Financial Markets | dal 2018 gli arbitraggi fra borse sono quasi inutilizzabili | libero (Paderborn) |
| *Building Trust Takes Time: Limits to Arbitrage for Blockchain-Based Assets* ([arXiv 1812.00595](https://arxiv.org/abs/1812.00595)) | Hautsch, Scheuch, Voigt, 2024 | R, Review of Finance | i tempi di regolamento spiegano il 91% delle differenze di prezzo | libero |
| *Cryptomarket Discounts* ([doi](https://doi.org/10.1016/j.jimonfin.2023.102963)) | Borri, Shakhnov, 2023 | R, JIMF | sconti fra borse con emivita di un giorno, piu' grandi con i controlli sui capitali | libero |
| *Decentralized Efficiency? Arbitrage in Bitcoin Markets* ([doi](https://doi.org/10.1080/0015198X.2020.1733902)) | Kruckeberg, Scholz, 2020 | R, Financial Analysts Journal | almeno 380 milioni di profitti di arbitraggio lasciati nel 2017-2018 | a pagamento |
| *Fundamentals of Perpetual Futures* ([arXiv 2212.06888](https://arxiv.org/abs/2212.06888)) | He, Manela, Ross, von Wachter, 2022-2026 | P | deviazioni dei perpetual maggiori che nel FX, in calo; arbitraggio del funding con Sharpe alto anche con le commissioni piu' alte | libero |
| *Perpetual Futures Pricing* ([arXiv 2310.11771](https://arxiv.org/abs/2310.11771)) | Ackerer, Hugonnier, Jermann, 2025 | R, Mathematical Finance | teoria: prezzi di non arbitraggio dei perpetual | libero |
| *The Crypto Carry Trade* (pagina dell'autore, CMU) | Christin, Routledge, Soska, Zetlin-Jones, 2022 | P | short future e long spot, Sharpe 7-10 nel campione (da un riassunto) | libero |
| *Implied ETF Carry Rates and the Limits of Arbitrage in Segmented Bitcoin Markets* ([arXiv 2605.29309](https://arxiv.org/abs/2605.29309)) | Mallory, 2026 | P | il carry implicito nelle opzioni IBIT supera quello dei future CME | libero |
| *Statistical Arbitrage in Cryptocurrency Markets* ([doi](https://doi.org/10.3390/jrfm12010031)) | Fischer, Krauss, Deinert, 2019 | R, JRFM | random forest a minuti, 7,1 punti base al giorno dopo i costi; la capacita' e' il limite | libero |
| *Pairs Trading in Cryptocurrency Markets* ([doi](https://doi.org/10.1109/ACCESS.2020.3024619)) | Fil, Kristoufek, 2020 | R, IEEE Access | niente al giornaliero, 11,6% al mese a 5 minuti | libero |
| *Copula-based trading of cointegrated cryptocurrency pairs* ([arXiv 2305.06961](https://arxiv.org/abs/2305.06961)) | Tadi, Witzany, 2025 | R, Financial Innovation | coppie contro BTC sui perpetual Binance, Sharpe circa 0,97 | libero |
| *A Trend Factor for the Cross Section of Cryptocurrency Returns* ([doi](https://doi.org/10.1017/S0022109024000747)) | Fieberg e altri, 2025 | R, JFQA | un fattore di tendenza che sopravvive ai costi | libero |

### crypto-momentum, bitcoin-on-chain, bitcoin-volatilita, altcoin

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Risks and Returns of Cryptocurrency* ([NBER w24877](https://www.nber.org/papers/w24877)) | Liu, Tsyvinski, 2021 | R, Review of Financial Studies | momentum di serie storica forte, poca esposizione ai fattori azionari | libero |
| *Common Risk Factors in Cryptocurrency* ([NBER w25882](https://www.nber.org/papers/w25882)) | Liu, Tsyvinski, Wu, 2022 | R, Journal of Finance | mercato, dimensione e momentum spiegano la sezione trasversale | libero |
| *Technical trading and cryptocurrencies* ([doi](https://doi.org/10.1007/s10479-019-03357-1)) | Hudson, Urquhart, 2021 | R, Annals of Operations Research | 15.000 regole: redditizie, ma **su Bitcoin nessuna prevedibilita' fuori campione** | libero |
| *Are simple technical trading rules profitable in bitcoin markets?* ([doi](https://doi.org/10.1016/j.iref.2024.05.003)) | Deprez, Frommel, 2024 | R, IREF | 75.360 regole; dopo costi e test multipli battono il buy-and-hold, soprattutto corretto per il rischio | a pagamento |
| *Learning and predictability via technical analysis: Evidence from bitcoin and stocks with hard-to-value fundamentals* ([doi](https://doi.org/10.1111/fima.12310)) | Detzel e altri, 2021 | R, Financial Management | prezzo su media mobile predice BTC, come le azioni del Nasdaq delle dot-com | a pagamento |
| *Catching Crypto Trends; A Tactical Approach for Bitcoin and Altcoins* ([SSRN 5209907](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=5209907)) | Zarattini, Pagani, Barbon, 2025 | P, Swiss Finance Institute | canali di Donchian con dimensione per volatilita', Sharpe sopra 1,5 netto | SSRN |
| *Cryptocurrency momentum has (not) its moments* ([doi](https://doi.org/10.1007/s11408-025-00474-9)) | Grobys e altri, 2025 | R, FMPM | il momentum cripto ha crolli gravi; la gestione per volatilita' li attenua | libero |
| *Using on-chain data to predict Bitcoin cycles* ([doi](https://doi.org/10.1016/j.ribaf.2026.103486)) | Grobys, Nasman, Sandretto, 2026 | R, RIBAF | MVRV Z-score: Sharpe da 0,45 a 1,28 su tre cicli; soglia non toccata nel 2022-2025 (da riassunti) | libero |
| *Out-of-sample forecasting of cryptocurrency returns: A comprehensive comparison of predictors and algorithms* ([doi](https://doi.org/10.1016/j.physa.2022.127379)) | Yae, Tian, 2022 | R, Physica A | i predittori noti non prevedono fuori campione; il migliore R² 2,7% | a pagamento |
| *Bitcoin Price Prediction: Peer-Reviewed Evidence and Social Media Discourse* ([arXiv 2606.00071](https://arxiv.org/abs/2606.00071)) | Baquero, 2026 | P | nessuno studio revisionato batte la previsione ingenua a 1-6 mesi | libero |
| *Return and Volatility Forecasting Using On-Chain Flows in Cryptocurrency Markets* ([arXiv 2411.06327](https://arxiv.org/abs/2411.06327)) | Chi, Chu, Hao, 2024 | P | gli afflussi di USDT alle borse predicono BTC; quelli di BTC quasi no | libero |
| *Loaded for bear: Bitcoin private wallets, exchange reserves and prices* ([SSRN 3902504](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3902504)) | Hoang, Baur, 2022 | R, Journal of Banking & Finance | riserve delle borse in aumento, rendimenti futuri in calo | SSRN |
| *Blockchain characteristics and cryptocurrency returns* ([SSRN 4359884](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4359884)) | Bhambhwani, Delikouras, Korniotis, 2023 | R, JIFMIM | prezzi cointegrati con dimensione della rete e hash rate | libero (CEPR) |
| *Deep learning for Bitcoin price direction prediction: models and trading strategies empirically compared* ([doi](https://doi.org/10.1186/s40854-024-00643-1)) | Omole, Enke, 2024 | R, Financial Innovation | 82% di direzione e 6.654% l'anno: **segnale di sovra-adattamento**, costi non verificati | libero |
| *Regime changes in Bitcoin GARCH volatility dynamics* ([SSRN 3180830](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3180830)) | Ardia, Bluteau, Ruede, 2019 | R, Finance Research Letters | regimi di volatilita' evidenti | SSRN |
| *Exploring the predictability of cryptocurrencies via Bayesian hidden Markov models* ([arXiv 2011.03741](https://arxiv.org/abs/2011.03741)) | Koki, Leonardos, Piliouras, 2022 | R, RIBAF | quattro stati (rialzo, ribasso, calma) battono la passeggiata casuale | libero |
| *Safe haven or risky hazard? Bitcoin during the Covid-19 bear market* ([PMC](https://pmc.ncbi.nlm.nih.gov/articles/PMC7246008/)) | Conlon, McGee, 2020 | R, Finance Research Letters | nel marzo 2020 BTC e' sceso con l'S&P 500 | libero |
| *Return and volatility spillovers among cryptocurrencies* ([doi](https://doi.org/10.1016/j.econlet.2018.10.004)) | Koutmos, 2018 | R, Economics Letters | Bitcoin e' la fonte principale dei contagi fra 18 monete | a pagamento |
| *Virtual relationships: Short- and long-run evidence from BitCoin and altcoin markets* ([arXiv 1706.07216](https://arxiv.org/abs/1706.07216)) | Ciaian, Rajcaniova, Kancs, 2018 | R, JIFMIM | interdipendenza forte nel breve, debole nel lungo | libero |

Non verificati abbastanza, da non usare per ora: SSRN 6725492 (funding e open interest predicono
BTC), SSRN 6938742 (squilibrio degli ordini), SSRN 5660581 (141 predittori di BTC).

## opzioni-gamma

**Nello scope**: riguarda NQ ([`le-opzioni-in-atas.md`](../metodo/le-opzioni-in-atas.md)). Trovati il
14 ottobre 2026 nella stessa ricerca; **stato: trovato**.

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Hedging demand and market intraday momentum* (IDEAS) | Baltussen, Da, Lammers, Martens, 2021 | R, Journal of Financial Economics | in piu' di 60 future la giornata predice l'ultima mezz'ora; legato alla copertura dei dealer corti di gamma | libero (Notre Dame) |
| *Does Option Trading Have a Pervasive Impact on Underlying Stock Prices?* ([doi](https://doi.org/10.1093/rfs/hhaa082)) | Ni, Pearson, Poteshman, White, 2021 | R, Review of Financial Studies | dealer lunghi di gamma: volatilita' piu' bassa; corti: piu' alta | a pagamento |
| *0DTEs: Trading, Gamma Risk and Volatility Propagation* ([SSRN 4692190](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4692190)) | Dim, Eraker, Vilkov, 2024 | P | gamma netta dei dealer in media positiva, con volatilita' intraday piu' bassa | SSRN |
| *Does 0DTE Options Trading Increase Volatility?* ([SSRN 4426358](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4426358)) | Brogaard, Han, Won | P | piu' volume 0DTE, piu' volatilita' (+9,1% della media), guidato dal retail | SSRN |
| *Do S&P500 Options Increase Market Volatility? Evidence from 0DTEs* ([SSRN 5641974](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=5641974)) | Adams e altri, 2025 | P | volatilita' realizzata piu' bassa nei giorni con 0DTE; meno momento intraday | SSRN |
| *0DTE Index Options and Market Volatility: How Large is Their Impact?* (Cboe) | Vasquez, Amaya, Pearson, Garcia-Ares, 2025 | P | con gamma dei dealer negativa la volatilita' sale fino a 3,3 punti (da riassunti) | libero (Cboe) |
| *Net buying pressure and the information in bitcoin option trades* ([arXiv 2109.02776](https://arxiv.org/abs/2109.02776)) | Alexander, Deng, Feng, Wan, 2023 | R, Journal of Financial Markets | i vincoli dei market maker formano i prezzi delle opzioni Deribit | libero |
| *Bitcoin option expiration, gamma exposure, and intraday price reversals* ([doi](https://doi.org/10.1016/j.frl.2026.110340)) | Weiss e altri, 2026 | R, Finance Research Letters | calo un'ora prima della scadenza Deribit, poi ritorno; solo con gamma dei dealer corta (da riassunti) | a pagamento |

## Gli Esempi Del Transcript

Citati nel transcript, **non ancora cercati**: i titoli sono quelli pronunciati nel video e vanno
verificati. Il filone e' quello che avranno quando si scaricano.

| filone | paper | ruolo | stato |
|---|---|---|---|
| momento-intraday | *Market Intraday Momentum* (2018) | idea | da cercare |
| vwap | *VWAP: The Holy Grail for Day Trading Systems* | idea | da cercare |
| notte | *The Return Difference Between Trading and Non-Trading Hours: Like Night and Day* (2008) | idea | da cercare |
| pead | i tre paper sulla PEAD (1968, 1989, 2006) | idea, fuori scope (azioni) | da cercare |
| dimensione | *Volatility Managed Portfolios*; *The Impact of Volatility Targeting* | metodo (fase 2) | da cercare |

## Metodo

Paper da cui il framework prende una tecnica, citati nel codice ma **non ancora scaricati**:
Bailey e Lopez de Prado (2014), lo Sharpe deflazionato (fase 5); Broadie, Glasserman e Kou (1997),
la correzione per il monitoraggio discreto (fase 5, via TF).
