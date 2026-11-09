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

**Nello scope**: le opzioni sull'S&P 500 e la loro copertura sui future, per lo studio su **ES**
(CLAUDE.md, decisione 2, dal 14 ottobre 2026). Gli indicatori di ATAS: [`le-opzioni-in-atas.md`](../metodo/le-opzioni-in-atas.md).
Cercati il 14 ottobre 2026; i sette con il PDF libero sono stati letti per intero lo stesso giorno.
Il 15 ottobre una seconda ricerca (gamma, 0DTE e volatilita' intraday) ne ha aggiunti due, BPVZ e DPS,
letti e verificati. Studio: [`opzioni-gamma-es`](../studi/opzioni-gamma-es/1-idea.md).

**Cosa manca in letteratura**: nessun paper revisionato, e nessuna ricerca seria di industria,
**prova i livelli che ATAS disegna** (Zero Gamma o Flip, call wall, put wall). Esistono solo nel
materiale dei fornitori; l'unico test trovato e' un preprint SSRN non letto (Popovici 2026, SSRN
7082418, vedi la [ricerca del 15 ottobre](ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
Nessun paper prova il pinning sugli strike degli 0DTE.

### Letti E Analizzati

**R** rivista con revisione fra pari, **P** preprint o working paper. **Stato**: tutti `analizzato`;
solo BPVZ, DPS e ADEFOV hanno anche la verifica avversaria (fase 4), le altre sette no.

| sigla | paper | autori, versione letta | sede | ruolo | stato | cosa ne viene |
|---|---|---|---|---|---|---|
| GJ | [*Pinning in the S&P 500 futures*](opzioni-gamma/golez-2012-pinning-in-the-sp500-futures/analisi.md) | Golez, Jackwerth, 2012, versione pubblicata | R, Journal of Financial Economics | idea | analizzato | il future S&P chiude vicino allo strike il 21,3% dei venerdi' di scadenza seriale contro il 15% atteso, quasi tutto dal basso, nell'ultima mezz'ora |
| BB | [*Gamma Fragility*](opzioni-gamma/barbon-2021-gamma-fragility/analisi.md) | Barbon, Buraschi, 18 marzo 2021 | P, SSRN 3725454 | idea | analizzato | gamma dei dealer del giorno prima alta: volatilita' piu' bassa; sugli indici il segno dei dealer e' assunto, non misurato |
| BDLM | [*Hedging demand and market intraday momentum*](opzioni-gamma/baltussen-2021-hedging-demand-and-market-intraday-momentum/analisi.md) | Baltussen, Da, Lammers, Martens, 2021, versione pubblicata | R, Journal of Financial Economics | idea | analizzato | la giornata fino alle 15:30 predice l'ultima mezz'ora in 62 future (NQ t 7,97, ES t 4,97); sui future S&P **solo con gamma netta negativa** |
| BTW | [*The Derivative Payoff Bias*](opzioni-gamma/baltussen-2024-the-derivative-payoff-bias/analisi.md) | Baltussen, Terstegge, Whelan, gennaio 2024 | P, SSRN 4562800 | idea | analizzato | su ES la notte prima della scadenza del terzo venerdi' sale di 14,3 punti base (t 3,54), il giorno dopo scende |
| GPP | [*Demand-Based Option Pricing*](opzioni-gamma/garleanu-2009-demand-based-option-pricing/analisi.md) | Garleanu, Pedersen, Poteshman, NBER WP 11843, dicembre 2005 (pubblicato 2009) | R, Review of Financial Studies | metodo | analizzato | su SPX 1996-2001 i dealer sono corti di put e lunghi di call, ma sommando gli strike **corti di gamma quasi ovunque** |
| VAPG | [*0DTE Index Options and Market Volatility*](opzioni-gamma/vasquez-2025-0dte-index-options-and-market-volatility/analisi.md) | Vasquez, Amaya, Pearson, Garcia-Ares, 25 gennaio 2025 | P, Cboe | idea | analizzato | gamma dei market maker di solito positiva, negativa in parte di almeno un giorno su quattro; effetto sulla volatilita' mediano piccolo |
| SM | [*Gamma Exposure*](opzioni-gamma/squeezemetrics-2017-gamma-exposure/analisi.md) | SqueezeMetrics, dicembre 2017 | white paper | idea | analizzato | la definizione della GEX e la convenzione del segno; nessun test, nessun campione fuori |
| BPVZ | [*Gamma positioning and market quality*](opzioni-gamma/buis-2024-gamma-positioning-and-market-quality/analisi.md) | Buis, Pieterse-Bloem, Verschoor, Zwinkels, 2024, versione dell'editore | R, Journal of Economic Dynamics and Control | idea | analizzato, verificato il 15 ottobre 2026 | **simulazione**: gamma positiva, volatilita' piu' bassa e autocorrelazione dei rendimenti brevi piu' negativa; negativa, il contrario, e mercato piu' fragile. Con il dealer al 18-19% del flusso la volatilita' e' nel rumore, l'autocorrelazione no; nessuno scenario sotto il 18% |
| DPS | [*Risky Intraday Order Flow and Option Liquidity*](opzioni-gamma/doshi-2025-risky-intraday-order-flow-and-option-liquidity/analisi.md) | Hitesh Doshi, Paola Pederzoli, Saim Ayberk Sert, 23 maggio 2025 | P, SSRN 5006194 | metodo | analizzato, verificato il 15 ottobre 2026 | opzioni SPX 2004-2021: lo spread delle brevissime lo spiega la volatilita' intraday del flusso, non la gamma dell'inventario; la copertura in delta e' secondaria (prova indiretta, nessun dato sul future, nessun segno) |
| ADEFOV | [*Do S&P 500 Options Increase Market Volatility? Evidence from 0DTEs*](opzioni-gamma/adams-2025-do-sp500-options-increase-market-volatility/analisi.md) | Adams, Dim, Eraker, Fontaine, Ornthanalai, Vilkov, versione del 15 luglio 2026 (assorbe Dim, Eraker, Vilkov) | P, SSRN 5641974 | metodo | analizzato, verificato il 9 novembre 2026 | SPX 2018-2024: nei giorni con 0DTE la volatilita' a 10 minuti e' circa il 7% piu' bassa; la gamma dei market maker, ereditata dai giorni prima, la frena per circa un'ora e fa tornare indietro il flusso degli E-mini; il delta scambiato non predice. Letto nella ricerca sul [flusso per gruppo](#flusso-opzioni) |

**La sintesi del filone** (15 ottobre 2026, dettagli e numeri nella
[ricerca](ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md#la-sintesi)). Tutti e nove danno lo
stesso verso: gamma dei dealer positiva smorza e fa tornare indietro il prezzo, negativa lo spinge;
nessuno trova il contrario. Sul **livello** della volatilita' l'effetto e' piccolo (VAPG, BB, BPVZ
sotto il 30% del flusso); sulla **forma** del percorso (il segno dell'autocorrelazione dei rendimenti
brevi, il range e le code della giornata) lo e' meno: e' la variabile da provare per prima. Restano
aperti: **il segno** dei dealer sugli indici (GPP corti, VAPG di solito lunghi, gli altri lo
assumono); **gli 0DTE** (i paper su SSRN si contraddicono, i letti non li toccano o li toccano poco);
**il passo gamma -> ordini sul future**, che nessuno misura e che DPS mette in dubbio in modo
indiretto. Nessuno usa NDX o NQ. Su NQ, con le sole barre e dentro il campione, si misurano le
variabili (autocorrelazione intraday, volatilita' realizzata, momento secondo del delta); la gamma
c'e' solo da ottobre 2026, e su NQ cade nel fuori campione. Su NQ la copertura vale circa lo 0,4-0,5%
del flusso (stima nostra), molto sotto gli scenari di BPVZ. **Adams e altri** (ADEFOV), che era il prossimo
da leggere, e' stato letto e verificato il 9 novembre 2026 dalla versione libera: conferma il verso (gamma
positiva, volatilita' piu' bassa per circa un'ora, piu' inversione) con lo stesso dato del Cboe per gruppo.

### Trovati

| paper | autori, anno | sede | dall'abstract | PDF |
|---|---|---|---|---|
| *Does Option Trading Have a Pervasive Impact on Underlying Stock Prices?* ([doi](https://doi.org/10.1093/rfs/hhaa082)) | Ni, Pearson, Poteshman, White, 2021 | R, Review of Financial Studies | dealer lunghi di gamma: volatilita' piu' bassa; corti: piu' alta | bozza 2006 |
| *Stock price clustering on option expiration dates* ([doi](https://doi.org/10.1016/j.jfineco.2004.08.005)) | Ni, Pearson, Poteshman, 2005 | R, Journal of Financial Economics | le azioni chiudono sugli strike alla scadenza | a pagamento |
| *A market-induced mechanism for stock pinning* ([pagina](https://math.nyu.edu/~avellane/stock_pinning.html)) | Avellaneda, Lipkin, 2003 | R, Quantitative Finance | modello: la copertura dei market maker attira il prezzo allo strike | libero |
| *Liquidity Provision to Leveraged ETFs and Equity Options Rebalancing Flows* ([SSRN 3925725](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3925725)) | Barbon, Beckmeyer, Buraschi, Moerke | P | gamma negativa: momento nell'ultima mezz'ora; positiva: inversione | libero |
| *End-of-Day Reversal* ([SSRN 5039009](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=5039009)) | Baltussen, Da, Soebhag, 2025 | P | le azioni invertono nell'ultima mezz'ora, non per la gamma | libero |
| *Dealer Gamma Exposure and Overnight Gap Risk* ([SSRN 6650858](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=6650858)) | Maurer, 2026 | P | **l'unico test fuori campione di una GEX**: aiuta a prevedere i gap della notte solo con VIX basso | SSRN |
| *0DTEs: Trading, Gamma Risk and Volatility Propagation* ([SSRN 4692190](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4692190)) | Dim, Eraker, Vilkov, 2024 | P | gamma dei dealer in media positiva, volatilita' intraday piu' bassa | SSRN; assorbito da ADEFOV, letto quello |
| *Does 0DTE Options Trading Increase Volatility?* ([SSRN 4426358](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4426358)) | Brogaard, Han, Won | P | piu' volume 0DTE, piu' volatilita' | SSRN |
| *The impact of option hedging on the spot market volatility* ([doi](https://doi.org/10.1016/j.jimonfin.2022.102627)) | Anderegg, Ulmann, Sornette, 2022 | R, JIMF | sul cambio: gamma dei market maker ricostruita, negativa, alza la volatilita' | libero (SNB) |
| *Option Market Activity* ([doi](https://doi.org/10.1093/rfs/hhl025)) | Lakonishok, Lee, Pearson, Poteshman, 2007 | R, Review of Financial Studies | sulle azioni chi non e' market maker vende piu' opzioni di quante ne compri | libero (bozza) |
| *Intraday Share Price Volatility and Leveraged ETF Rebalancing* ([doi](https://doi.org/10.1093/rof/rfv061)) | Shum e altri, 2016 | R, Review of Finance | volatilita' di fine giornata legata al ribilanciamento degli ETF a leva | libero |
| *Do leveraged ETFs really amplify late-day returns and volatility?* ([doi](https://doi.org/10.1016/j.finmar.2018.09.001)) | Ivanov, Lenkey, 2018 | R, Journal of Financial Markets | l'effetto e' economicamente trascurabile | libero (Fed) |
| *Net buying pressure and the information in bitcoin option trades* ([arXiv 2109.02776](https://arxiv.org/abs/2109.02776)) | Alexander, Deng, Feng, Wan, 2023 | R, Journal of Financial Markets | i vincoli dei market maker formano i prezzi delle opzioni Deribit | libero |
| *Bitcoin option expiration, gamma exposure, and intraday price reversals* ([doi](https://doi.org/10.1016/j.frl.2026.110340)) | Weiss e altri, 2026 | R, Finance Research Letters | calo prima della scadenza Deribit, poi ritorno, con gamma dei dealer corta (da riassunti) | a pagamento |

Due note di ricerca di Cboe (Mandy Xu, 2023 e 2025) dicono che la copertura netta degli 0DTE dei
market maker e' al massimo lo 0,2% della liquidita' giornaliera dell'SPX: un riferimento per quanto
una GEX stimata dall'open interest puo' distare dalle posizioni vere.

### La Ricerca Del 15 Ottobre 2026: Gamma, 0DTE E Volatilita' Intraday

Domanda: che effetto hanno la gamma dei dealer e le opzioni 0DTE sulla volatilita' intraday dei
future sugli indici? I 19 candidati, perche' BPVZ e DPS (ora fra i letti, sopra) e la sintesi:
[`ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`](ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md).
Gli scartati con un titolo sul tema (Singh, Elms, Ladia, Wilkens, Popovici, 2026) sono solo su SSRN,
con l'abstract non letto: restano nel file della ricerca, non in questo indice.

### La Ricerca Del 9 Novembre 2026: Le Opzioni Fra Indici

Domanda: l'informazione delle opzioni di un indice (gamma, flusso, livelli di SPX/ES) aiuta a leggere
un altro indice correlato (NQ/NDX/QQQ), e viceversa? Serve a decidere se lo studio globale di NQ ed ES
puo' essere uno solo. **Nessun paper trovato prova un effetto incrociato della gamma o dei livelli**;
i quattro scelti coprono il flusso delle opzioni fra indici e la trasmissione fra future, e il supplemento
su SSRN ne ha aggiunto uno (DIC, il VIX contro le borse estere). Candidati, scarti e motivi: [`ricerche/2026-11-09-opzioni-fra-indici.md`](ricerche/2026-11-09-opzioni-fra-indici.md).

**R** rivista con revisione fra pari, **P** preprint o working paper; tutti e cinque con la verifica
avversaria (fase 4) del 9 novembre 2026.

| sigla | paper | autori, versione letta | sede | ruolo | stato | cosa ne viene |
|---|---|---|---|---|---|---|
| CKMS | [*Index Option Trading Activity and Market Returns*](opzioni-gamma/chordia-2021-index-option-trading-activity-and-market-returns/analisi.md) ([doi](https://doi.org/10.1287/mnsc.2019.3529), [SSRN 2798390](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2798390)) | Chordia, Kurov, Muravyev, Subrahmanyam, versione accettata del 17 ottobre 2019 (non la pubblicata, 2021) | R, Management Science | idea | analizzato, verificato il 9 novembre 2026 (sha256 `fa1b5c768f3f`) | opzioni su indici dell'ISE, **Russell 2000 e Nasdaq-100 sommati**, solo Nasdaq-100 dal 22 aprile 2013 (nota 11), nessuna stima del solo NDX; 2006-2017: le put comprate dai **piccoli** (fino a 100 contratti) predicono l'S&P 500 **al contrario** a settimane (+0,22% per deviazione standard), solo col VIX alto; letto come assicurazione. Le opzioni **SPX** e i flussi di E-mini e SPY non predicono. Letto anche nella ricerca sul [flusso per gruppo](#flusso-opzioni) |
| HHW | [*Aggregating Information in Option Transactions*](opzioni-gamma/holowczak-2014-aggregating-information-in-option-transactions/analisi.md) ([doi](https://doi.org/10.3905/jod.2014.21.3.009), SSRN 1740046) | Holowczak, Hu, Wu, working paper del 13 gennaio 2011 (*Consolidating Information in Option Transactions*, non la pubblicata, 2014) | P (la pubblicata: R, Journal of Derivatives) | metodo | analizzato, verificato il 9 novembre 2026 (sha256 `343b8e738893`) | come si somma il flusso delle opzioni (segno, delta o vega, taglia): su QQQ 2006 va con il prezzo **nello stesso minuto** (fino a 0,41), il minuto dopo circa 0,01 (stelle da minuti trattati come indipendenti); la misura HHW pesa per n(d1)/M, non per il delta; un solo indice. Letto anche nella ricerca sul [flusso per gruppo](#flusso-opzioni) |
| LMCB | [*When is cross impact relevant?*](opzioni-gamma/le-coz-2024-when-is-cross-impact-relevant/analisi.md) ([arXiv 2305.16915](https://arxiv.org/abs/2305.16915), [doi](https://doi.org/10.1080/14697688.2024.2302827)) | Le Coz, Mastromatteo, Challet, Benzaquen, arXiv v2 del 26 marzo 2024 | R, Quantitative Finance | metodo | analizzato, verificato il 9 novembre 2026 | il flusso dello strumento piu' liquido spiega in piu' il correlato meno liquido (correlazione sopra il 50%, 10-20 scambi per finestra), **nella stessa finestra, non in anticipo**; E-mini S&P fra i liquidi, Nasdaq-100 non nominato |
| HAS | [*Intraday Price Formation in U.S. Equity Index Markets*](opzioni-gamma/hasbrouck-2003-intraday-price-formation-in-us-equity-index-markets/analisi.md) ([doi](https://doi.org/10.1046/j.1540-6261.2003.00609.x), [versione NYU 2000](https://archive.nyu.edu/jspui/bitstream/2451/27374/2/FIN-00-046.pdf)) | Hasbrouck, versione di lavoro del 16 novembre 2000 (non la pubblicata, 2003) | R, Journal of Finance | metodo | analizzato, verificato il 9 novembre 2026 | 2000: gli E-mini guidano, ES 0,854-0,882 e NQ 0,836-0,853 di quota d'informazione, **stimati separatamente**; fra indici solo QQQ verso SPY "al massimo circa il 10%", senza tabella; la guida sta in secondi |
| DIC | [*US Implied Volatility as a Predictor of International Returns*](opzioni-gamma/dicle-2017-us-implied-volatility-predictor-international-returns/analisi.md) ([SSRN 3057745](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3057745)) | Dicle, working paper SSRN del 3 novembre 2017 (non la pubblicata) | P (la pubblicata: R, Quantitative Finance and Economics, sede minore) | metodo | analizzato, verificato il 9 novembre 2026 (sha256 `41b9313fc246`) | 2000-2017, chiusure giornaliere: il VIX di ieri ha un coefficiente negativo per 18 indici esteri su 20 e li "causa" nel senso di Granger in 17, fortissimo in Asia ed Europa, che chiudono prima di New York, e molto ridotto nelle Americhe, dove pero' Brasile, Canada e Cile restano significativi; l'S&P 500 solo dello stesso giorno; Nasdaq e VXN assenti. La lettura dei fusi orari e' nostra. Lezione di metodo: allineare gli istanti, non le date, e mettere il mercato guida ritardato |

**La sintesi della ricerca** (9 novembre 2026, aggiornata lo stesso giorno con DIC; dettagli nella
[ricerca](ricerche/2026-11-09-opzioni-fra-indici.md#la-sintesi)). Nessuno dei cinque tocca la gamma: **nessun
paper prova che gamma, muri o zero gamma di un indice spieghino l'altro**. Il legame fra indici che si vede e'
**comune e contemporaneo**: un premio di rischio comune (CKMS), il prezzo che nasce sui future liquidi e passa
agli altri nella stessa finestra o in pochi secondi (LMCB, HAS), il flusso delle opzioni che va con il prezzo
nello stesso minuto (HHW). L'unico che prova le opzioni di un indice contro altri indici (DIC, il VIX) trova un
anticipo di un giorno forte dove le borse chiudono prima di New York e molto ridotto, non sparito, nelle Americhe:
in gran parte orologio, non informazione (lettura nostra). Dove un flusso di opzioni su indici anticipa, e' lento
e al contrario (CKMS), e le SPX non anticipano. Per lo studio globale unico di NQ ed ES: **lettura del contesto
comune, non regola**. Rifacibile con il tape di ATAS, dentro il campione: chi guida fra ES e NQ (HAS) e quanto il
flusso di ES spiega NQ in piu' (LMCB), ma il future ES per leggere NQ esce dal perimetro e richiede la richiesta
dell'utente; VIX o VXN contro NQ (DIC) solo con gli istanti allineati e su richiesta, perche' il VIX e' contesto.
Nessuno studio aperto. Il supplemento su SSRN non ha trovato nessun paper sulla gamma o sul flusso fra NDX/QQQ e
S&P 500; gli scartati (fra cui Figlewski e Frommherz e Cenobar e Fazeli, da tenere) sono nella
[ricerca](ricerche/2026-11-09-opzioni-fra-indici.md).

## flusso-opzioni

Il flusso delle opzioni per tipo di partecipante (clienti, professionisti, conti proprietari,
broker-dealer, market maker) e cosa dice del prezzo dell'indice: chi e' informato, su che orizzonte, il
retail e gli 0DTE, come si normalizza. Serve al riquadro "Chi ha comprato delta oggi" della pagina X-Ray
(il DEX per gruppo di OptionsDepth su SPX), come descrizione. Ricerca del 9 novembre 2026, candidati,
scarti e motivi: [`ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md`](ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md).
Riusa due paper gia' trovati nel filone `opzioni-gamma` (Chordia e altri 2021; Holowczak, Hu, Wu 2014).

### Letti E Analizzati

**R** rivista con revisione fra pari, **P** preprint o working paper. Tutti con la verifica avversaria
(fase 4) del 9 novembre 2026.

| sigla | paper | autori, versione letta | sede | ruolo | stato | cosa ne viene |
|---|---|---|---|---|---|---|
| ADEFOV | [*Do S&P 500 Options Increase Market Volatility? Evidence from 0DTEs*](opzioni-gamma/adams-2025-do-sp500-options-increase-market-volatility/analisi.md) | Adams, Dim, Eraker, Fontaine, Ornthanalai, Vilkov, versione del 15 luglio 2026 ([libera](https://www.jean-sebastienfontaine.com/papers/0dte-options-volatility.pdf)) | P, SSRN 5641974 | metodo | analizzato, verificato il 9 novembre 2026 | stessi quattro gruppi Cboe di OptionsDepth, SPX 2018-2024 a 10 minuti; **nessun gruppo provato contro il prezzo**; il delta scambiato sommato non predice la volatilita', la gamma ereditata si'. Filone principale `opzioni-gamma` |
| PP | [*The Information in Option Volume for Future Stock Prices*](flusso-opzioni/pan-2006-information-in-option-volume-for-future-stock-prices/analisi.md) | Pan, Poteshman, NBER w10925 del novembre 2004 (pubblicato 2006) | R, Review of Financial Studies | idea | analizzato, verificato il 9 novembre 2026 | open-buy per classe, CBOE 1990-2001: informato sui titoli; sugli indici il giorno dopo SPX niente, NDX e OEX contrarian in due classi ciascuno; i conti proprietari mai |
| CLZZ | [*Betting Against the Crowd: Option Trading and Market Risk Premium*](flusso-opzioni/cao-2026-betting-against-the-crowd/analisi.md) | Cao, Li, Zhan, Zhou, versione accettata del gennaio 2026 | R, JFQA (in uscita) | metodo | analizzato, verificato il 9 novembre 2026 | lo sbilancio delle call dei clienti **sui titoli**, sommato, predice il mercato al contrario (giorno -0,070, t -2,94); sulle opzioni di SPX e NDX niente al giorno e alla settimana |
| MCH | [*Option Volume Imbalance as a predictor for equity market returns*](flusso-opzioni/michael-2022-option-volume-imbalance-as-a-predictor/analisi.md) | Michael, Cucuringu, Howison, arXiv v1 del 23 gennaio 2022 | P, arXiv 2201.09319 | metodo | analizzato, verificato il 9 novembre 2026 | sbilancio fra -1 e +1 per cinque gruppi, Nasdaq 2015-2019, titoli contro SPY: clienti nel verso, market maker e broker al contrario, la notte; a 10 minuti niente dopo Bonferroni; nessun indice |
| MAMU | [*Intraday 0DTE Option Order Flow and S&P 500 Downside Risk*](flusso-opzioni/maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk/analisi.md) | Maurer, Muller, versione dell'agosto 2026 (tesi di master, Frankfurt School) | P, SSRN 7339718 | metodo | analizzato, verificato il 9 novembre 2026 | PCR 0DTE di SPX pesato per la gamma alle 9:35, 1.027 sedute 2022-2026, **senza gruppi**: l'ora dopo un po' piu' debole (t -2,65) e piu' spesso in calo (logit t 3,22), solo nei primi 15 minuti; fuori campione niente; delta pesato e flusso firmato non predicono |
| BBG | [*Retail Traders Love 0DTE Options... But Should They?*](flusso-opzioni/beckmeyer-2023-retail-traders-love-0dte-options/analisi.md) | Beckmeyer, Branger, Gayda, versione del 27 luglio 2024 | P, SSRN 4404704 | metodo | analizzato, verificato il 9 novembre 2026 | il retail d'asta (SLAN/MLAT, fino a 10 contratti; 2-4% del volume S&P 500, oltre il 6% degli 0DTE) perde oltre 125 milioni di dollari nel 2021-2023, circa tre quarti in costi; market maker corti di 0DTE; **nessuna prova del flusso contro il prezzo** (il delta con segno della Tab. 8, con gli effetti fissi del giorno, non dice niente sul verso); unita' della Tab. 7 il milione (dedotto), della Tab. 8 dubbio forte |
| CKMS | [*Index Option Trading Activity and Market Returns*](opzioni-gamma/chordia-2021-index-option-trading-activity-and-market-returns/analisi.md) | Chordia, Kurov, Muravyev, Subrahmanyam, versione accettata del 17 ottobre 2019 | R, Management Science | idea | analizzato, verificato il 9 novembre 2026 | ISE, Russell 2000 e Nasdaq-100 sommati (solo Nasdaq-100 dal 22/04/2013): predicono l'S&P 500 solo le put dei **clienti fino a 100 contratti**, **al contrario**, a settimane, col VIX alto, come assicurazione; clienti grandi e firm no; opzioni SPX no. Filone principale `opzioni-gamma` |
| HHW | [*Aggregating Information in Option Transactions*](opzioni-gamma/holowczak-2014-aggregating-information-in-option-transactions/analisi.md) | Holowczak, Hu, Wu, working paper del 13 gennaio 2011 | P (pubblicato: R, Journal of Derivatives) | metodo | analizzato, verificato il 9 novembre 2026 | QQQ 2006, senza gruppi: il flusso va con il prezzo nello stesso minuto (fino a 0,41), il minuto dopo circa 0,01; il DEX (delta per volume) e' la piu' bassa delle *delta per share* nello stesso minuto (0,1052); nessuna normalizzazione. Filone principale `opzioni-gamma` |
| KVS | [*Informed Trading in the Index Option Market*](flusso-opzioni/kaeck-2017-informed-trading-in-the-index-option-market/analisi.md) ([SSRN 2981332](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2981332)) | Kaeck, van Kervel, Seeger, versione rivista del 17 agosto 2019 secondo SSRN (il PDF non porta data) | P, TILEC DP 2017-027 | metodo | analizzato, verificato il 9 novembre 2026 (sha256 `e8593596f3c4`) | opzioni SPX 2014, tutti gli scambi, **senza gruppi e senza 0DTE**, sommati come il DEX (contratti per delta per segno): il delta comprato va con il prezzo **nella stessa ora** (98,13 cents in quattro ore, t 4,41), i ritardi non predicono (t 0,6 e 0,4), il prezzo dell'ora prima predice il flusso (t 5,7); al minuto niente. La verifica: l'ordinamento da' al delta la parte comune con SPY (con SPY prima circa 70-75 cents) |
| NSW | [*Index Options Trading and Sentiment*](flusso-opzioni/wang-2021-index-options-trading-and-sentiment/analisi.md) ([SSRN 3981994](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3981994)) | Ni, Stouraitis, Wang, 8 dicembre 2021 (ordine del frontespizio; **la cartella `wang-2021-...` non segue la regola del primo autore**: segnalato, non rinominata) | P | idea | analizzato, verificato il 9 novembre 2026 (sha256 `74e51c85f088`) | Cboe Open-Close 1991-2018, mensile: lo sbilancio dei clienti pubblici sulle SPX **in the money** predice il mercato **al contrario** a uno-tre mesi (mese dopo t -3,03, tre mesi t -3,85), letto come sentiment; le put OTM vanno nel verso opposto; nello stesso mese il pubblico compra call ITM quando il mercato scende; testo e Tab. 5 non coincidono, fuori campione non descritto |
| AGPV | [*The Rise of Algorithmic Retail Option Traders*](flusso-opzioni/garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders/analisi.md) ([SSRN 6480379](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=6480379)) | Amaya, Garcia-Ares, Pearson, Vasquez, 12 agosto 2026 (ordine del PDF; la cartella segue la pagina SSRN) | P | metodo | analizzato, verificato il 9 novembre 2026 (sha256 `8eda5c817ada`) | OPRA 2022-2026, **senza tipo di cliente**: negli 0DTE di SPX raffiche entro pochi secondi da ogni mezz'ora, poi dai quarti d'ora e dai cinque minuti, crescenti ogni anno, di spread venduti e iron condor da bot retail; piu' piccole quando il mercato si agita; **nessun rendimento dell'indice guardato**. Per il riquadro: confronto allo stesso minuto in ora di New York, storico non troppo lungo |

**La sintesi del filone** (9 novembre 2026, aggiornata due volte lo stesso giorno: con BBG, MAMU, CKMS e HHW, poi
con KVS, NSW e AGPV dal supplemento su SSRN; dettagli e numeri nella
[ricerca](ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md#la-sintesi)). Nessuno degli undici trova un
gruppo che anticipi il **verso di un indice**: sulle opzioni di SPX niente al giorno e alla settimana (PP, CLZZ,
CKMS), e dentro la giornata il delta e il flusso con il segno non anticipano l'ora dopo (MAMU; KVS, con la stessa
somma del DEX) ne' il minuto dopo (HHW, su QQQ). Dove un flusso per gruppo su indici predice, predice **al
contrario e lento** (PP su OEX e NDX; CLZZ su SPX a due mesi; CKMS con le put dei piccoli su Russell 2000 e
Nasdaq-100, assicurazione; NSW con le SPX in the money a uno-tre mesi, sentiment); l'unica previsione nel verso e'
MAMU, volume senza segno e senza gruppi, nei primi 15 minuti, nulla fuori campione, e il verso delle put cambia
con la moneyness (NSW). Il flusso va con il prezzo nello stesso tempo (HHW, KVS, CKMS, NSW, MAMU). I conti
proprietari non dicono mai niente; i "clienti" del Cboe comprendono gli istituzionali, e il retail ha tre
definizioni che non coincidono con il gruppo di OptionsDepth (l'asta in BBG, 4-6% degli 0DTE; il comportamento a
orario in AGPV; la stima Cboe di meta'-tre quinti). Dentro la giornata gli 0DTE di SPX hanno **raffiche agli orari
tondi** di New York, crescenti ogni anno (AGPV). La **nostra misura** su 35 sedute di X-Ray (descrittiva, non un
paper): dentro la seduta i clienti non professionali vanno contro il prezzo (mediana -0,85 su 0DTE), i
professionali con il prezzo (+0,87); tolta la parte spiegata dal prezzo della mattina, nessun gruppo predice il
pomeriggio (8 prove, p fra 0,06 e 0,82). **Per il riquadro** gia' implementato (percentile alla stessa ora,
sorpresa oltre il prezzo, bande 10-90%, soglie 20/80, al massimo 60 sedute): i paper lo confermano, e il confronto
allo stesso minuto in ora di New York e' quello che neutralizza le raffiche; da leggere con cautela il minuto
subito dopo un orario tondo e la variazione degli ultimi 30 minuti; il tetto di 60 sedute protegge da uno storico
che invecchia e, come le soglie 20/80, e' una convenzione da dichiarare; l'etichetta "Retail" va qualificata (il
retail automatico potrebbe stare fra i professionali). Nessuno studio aperto.

### Trovati

Nessuno: i tre del supplemento su SSRN (KVS, NSW, AGPV) sono letti, verificati e saliti fra i letti; gli scartati
e i motivi sono nella [ricerca](ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md).

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
