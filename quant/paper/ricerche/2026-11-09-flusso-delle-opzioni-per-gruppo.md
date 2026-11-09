# Ricerca: il flusso delle opzioni per tipo di partecipante, e cosa dice del prezzo dell'indice · 9 novembre 2026

**Filone**: flusso-opzioni (nuovo; Adams e altri restano nel filone opzioni-gamma, dove erano gia') ·
**Chiesta da**: utente, attraverso lo studio dei paper (`/studio-paper`), fase 1 · **Quanti**: da 4 a 6

**Lo scopo**: ridisegnare il riquadro "Chi ha comprato delta oggi" della pagina X-Ray, perche' sia piu'
utile a una decisione. E' una **descrizione**, non un segnale operativo. Il dato dal vivo e' il **DEX per
gruppo** di OptionsDepth su SPX: il delta scambiato minuto per minuto da clienti non professionali
(retail), clienti professionali, firm (conti proprietari) e broker-dealer, separato fra 0DTE e tutte le
scadenze, sommato dall'inizio della giornata. E' un **flusso**, non una posizione, e viene da dati del
tipo Cboe Open-Close, che dicono per ogni contratto chi l'ha scambiato (il codice d'origine) e se apriva o
chiudeva una posizione.

**Le sotto-domande**: (1) quali gruppi sono informati e quali sono rumore o contrarian; (2) su che
orizzonte (intraday, il giorno, la settimana) e su indici, non solo su titoli; (3) con che significativita'
e con che misura; (4) il retail e gli 0DTE; (5) come si normalizza un flusso per leggerlo dal vivo.

**Le parole**:

- **0DTE** (*zero days to expiration*): opzioni che scadono il giorno stesso in cui si scambiano.
- **DEX** (*delta exposure*) scambiato: la somma, contratto per contratto, del delta comprato meno quello
  venduto. Il delta dice quante unita' di sottostante equivale un'opzione; un DEX positivo vuol dire che
  il gruppo, nel complesso, ha comprato esposizione al rialzo.
- **Open-buy** (acquisto in apertura): contratti comprati per aprire una posizione nuova. Pan e Poteshman
  lo indicano come la parte del volume che porta piu' informazione, perche' esclude chi chiude.
- **Order imbalance** (sbilancio degli ordini) e **net buying pressure** (pressione netta d'acquisto):
  acquisti meno vendite, spesso divisi per il volume totale, cosi' il numero sta fra -1 e +1 e si
  confronta fra giorni diversi.
- **Contrarian**: un flusso che anticipa il prezzo nel verso **opposto** (il gruppo compra call e
  l'indice poi scende).
- **Z-score**: quante deviazioni standard un valore dista dalla sua media storica; serve a dire se il
  flusso di oggi alle 11:00 e' grande rispetto agli altri giorni alla stessa ora.

**Ricerche fatte** (9 novembre 2026):

- ricerca web: `"The Information in Option Volume for Future Stock Prices" Pan Poteshman pdf`
- ricerca web: `retail investors 0DTE options SPX losses Beckmeyer Branger Gayda`
- ricerca web: `Bryzgalova Pavlova Sikorskaya retail trading in options and the rise of the big three wholesalers`
- ricerca web: `index option order flow by investor type public customers firm proprietary predicts S&P 500 returns open-close data`
- ricerca web: `Bogousslavsky Muravyev "An Anatomy of Retail Option Trading"` (e con i domini delle universita')
- ricerca web: `Chen Joslin Ni "Demand for Crash Insurance, Intermediary Constraints, and Risk Premia" public investors SPX puts`
- ricerca web: `0DTE SPX options order flow by trader type customer professional market maker intraday returns predictability paper`
- ricerca web: `Hu 2014 "Does option trading convey stock price information?" Journal of Financial Economics order imbalance delta`
- ricerca web (estesa): `0DTE retail order flow Cboe Open-Close SPX net delta intraday market returns predict paper 2024 2025`
- ricerca web (estesa): `index options informed trading public customers professional customers firm broker-dealer SPX open-close net buying S&P 500 returns study`
- ricerca web: `KOSPI 200 index options investor type foreign institutions individuals informed trading order imbalance predicts index returns`
- ricerca web: `Taiwan TAIEX index options trader type foreign institutional individual informed order flow predicts futures returns intraday`
- ricerca web: `Maceo Maurer 0DTE order flow S&P 500 downside risk next-hour returns opening put-call ratio` e il titolo esatto
- ricerca web: `Dim Eraker Vilkov 0DTEs trading gamma risk volatility propagation pdf Open-Close customer market maker net gamma`
- ricerca web: `Beckmeyer "Retail Traders Love 0DTE Options" pdf Cboe Open-Close trade size retail customers`
- ricerca web: `retail option order flow SPY QQQ index options predicts market returns contrarian retail investors index options study`
- ricerca web: `"Betting Against the Crowd" Cao Li Zhan Zhou option trading market risk premium journal pdf`
- OpenAlex (API): i titoli esatti di Beckmeyer e altri, Bogousslavsky e Muravyev, Dim, Eraker e Vilkov, Maurer e
  Muller, de Silva e altri, Muravyev 2016, Easley e altri 1998, Bollen e Whaley 2004, Ge e altri 2016, Hu 2014,
  Holowczak e altri 2014, Michael e altri 2022, Cao e altri
- Crossref (API): Chen, Joslin e Ni; Pan e Poteshman; Ge, Lin e Pearson; de Silva, Smith e So
- arXiv: la pagina di 2201.09319
- PDF scaricati **solo per verificare** che esistano e cosa coprono (frontespizio, sezione dei dati e, per
  Pan e Poteshman, la §4.7 sugli indici), nella scratchpad, non in `quant/paper`: NBER w10925, NBER w25573,
  arXiv 2201.09319, il PDF di Cao e altri su jfqa.org, il PDF di Adams e altri sulla pagina di Fontaine

**SSRN** non e' stato aperto (Cloudflare): i paper solo su SSRN sono verificati su OpenAlex. **Nessuna
istruzione** trovata nelle pagine e nei PDF aperti per la verifica.

**Supplemento su SSRN** (9 novembre 2026, sera, dopo che la persona ha superato Cloudflare nella finestra di
Playwright): ricerca del sito (`searchresults.cfm?term=`, la casella di ricerca) e pagine References e
Citations di beckmeyer, maurer, adams, chordia e holowczak. Query: `0DTE options retail`, `0DTE SPX order
flow`, `index option order flow market returns`, `option order imbalance investor type index`, `Cboe
open-close index options`, `public customer index options informed`, `retail option order flow market
return predictability`, `zero-day options trader type`, `net buying pressure index options returns`,
`intraday option flow S&P 500 returns`, `market maker net delta SPX`, `put-call ratio intraday index
returns`, `options sentiment index returns investor class`, `informed trading index option market`, `firm
proprietary options trading`, `broker-dealer options order flow`, `professional customers options`, `option
trading volume by investor type`, `retail 0DTE contrarian`. Titolo, autori, affiliazioni, date e abstract
letti sulla pagina SSRN di ogni candidato nuovo (righe sotto la tabella, da kaeck in giu'). Nessuna
istruzione nelle pagine aperte.

## La Risposta Breve

Scritta nella fase 1, prima di leggere; **corretta il 9 novembre 2026 nella fase 5** con le quattro
analisi verificate, **aggiornata lo stesso giorno** con altre quattro (BBG, MAMU, CKMS, HHW) e poi, con il
supplemento su SSRN, con altre tre (KVS, NSW, AGPV). Dove la prima versione sbagliava, lo si dice.

**In tre righe.** (1) Nessuno degli undici paper trova un gruppo che anticipi il **verso** di un indice: sulle
opzioni di SPX niente il giorno dopo, la settimana dopo (PP, CLZZ, CKMS), l'ora dopo (MAMU, KVS: il delta
comprato va con il prezzo **nella stessa ora**, e il delta dell'ora prima non dice niente dell'ora dopo).
(2) Dove qualcosa si vede e' **al contrario e lento** (i piccoli che comprano put su altri indici, poi l'indice
sale: CKMS, ed e' assicurazione; i clienti pubblici che comprano call **in the money** sulle SPX, poi il mercato
scende per uno-tre mesi: NSW, ed e' sentiment), oppure e' **insieme al prezzo** nello stesso minuto o nella
stessa ora (HHW, KVS, la nostra misura). (3) Il "retail" non ha una definizione sola: quello riconosciuto
dall'asta e' il 4-6% degli 0DTE e perde (BBG), quello riconosciuto dal comportamento manda **raffiche a ogni
mezz'ora** di spread venduti (AGPV); nessuno dei due e' per forza il gruppo che X-Ray chiama retail.

- **Sugli indici il flusso di un gruppo predice poco, e quando predice lo fa al contrario.** Pan e
  Poteshman (§4.7, Tab. 9, 1990-2001): su **SPX** nessuna classe predice il giorno dopo; contrarian e
  significativi su **NDX** (broker a sconto t 3,11, altri t 3,09) **e anche su OEX** (broker a sconto t 3,12,
  servizio completo t 3,60), che il testo del paper non commenta. *Corretto: la prima versione diceva
  "nessun gruppo su SPX e OEX"; per l'OEX era sbagliato.*
- **Cao e altri: il contrarian e' sulle opzioni dei titoli, non degli indici.** Lo sbilancio delle call
  comprate dai clienti, sommato sui **titoli**, predice il mercato al contrario: -0,070 (t -2,94) il giorno
  dopo per una deviazione standard (Tab. A3), -0,240 (t -2,69) la settimana dopo. Il **-0,329**
  settimanale che il paper mette nell'introduzione viene dalla regressione con **7 controlli** (Tab. 2), e
  con il solo sbilancio delle put accanto la settimana scende a -0,211 (t -1,67, non significativo).
  **Sulle opzioni di SPX e NDX** nessuna previsione significativa al giorno o alla settimana (Tab. 7A, A11);
  su SPX solo a due mesi, -0,583 (t -2,74), al contrario; su NDX a nessun orizzonte. E la variabile
  predetta e' sempre il mercato americano intero (MKTRF), mai SPX o NDX. *Corretto: la prima versione
  attribuiva il -0,070 all'indice e lo dava "da verificare".*
- **Intraday e 0DTE**: Adams e altri usano **gli stessi quattro gruppi** di OptionsDepth, a 10 minuti, su
  SPX 2018-2024, ma **non provano nessun gruppo contro il prezzo**: li sommano per ricostruire i market
  maker. Il delta scambiato sommato (diviso per il rendimento dei 10 minuti) non predice la volatilita'
  (t -0,61 senza gli effetti fissi della data; con la data t fra -1,20 e 0,95). I clienti del Cboe
  comprendono **anche gli istituzionali**, e sugli 0DTE sono corti di straddle al denaro e lunghi di ali
  lontane, con gamma molto negativa. *Confermato, con queste precisazioni.* **Maurer e Muller** (tesi di
  master, non revisionata), su 1.027 sedute 2022-2026 e **senza gruppi** (volume di tutti): il rapporto
  put-call 0DTE pesato per la gamma alle 9:35 anticipa un'ora un po' piu' debole (t -2,65) e piu' spesso in
  calo (logit t 3,22), solo nei primi 15 minuti; il verso sparisce nel 2024-2026 (p 0,205); fuori campione
  niente. **Il delta pesato e il flusso con il segno non predicono** l'ora dopo (t +0,07 e -0,11). *Nuovo:
  la prima versione lo dava "da verificare in lettura".*
- **Il retail sugli 0DTE perde, e non e' il "retail" di OptionsDepth** (Beckmeyer e altri): il retail
  riconosciuto dall'asta (SLAN/MLAT, fino a 10 contratti) e' il 2-4% del volume delle opzioni S&P 500 e oltre
  il 6% degli 0DTE, e perde oltre 125 milioni di dollari fra febbraio 2021 e settembre 2023, circa tre quarti
  in costi. Il paper **non prova il flusso contro il prezzo**: il delta con segno della Tab. 8 non dice niente
  sul verso, perche' con gli effetti fissi del giorno la posizione netta del giorno finisce nella costante.
  *Prima: non letto, fermo a Cloudflare.*
- **Chi e' informato sugli indici**: nessuno degli undici letti trova un gruppo informato sul **verso** di un
  indice. Michael e altri trovano i clienti "nel verso", ma sui titoli contro SPY e di notte, e a 10 minuti
  niente dopo la correzione per prove multiple. **Chordia e altri**: sulle opzioni su indici dell'ISE (Russell
  2000 e Nasdaq-100 sommati, solo Nasdaq-100 dal 22 aprile 2013), 2006-2017, predicono l'S&P 500 solo le
  **put dei clienti piccoli** (fino a 100 contratti; IRF a 10 settimane 0,56, errore 0,25), **al contrario**
  (piu' put comprate, indice piu' alto), a settimane e con il VIX alto; clienti grandi e firm insieme no
  (0,13, errore 0,25); le opzioni **SPX** no (0,02, errore 0,30). Gli autori lo leggono come premio per
  l'assicurazione, **non come informazione**. *Corretto: la prima versione diceva "i clienti nel complesso
  informativi sulle put a una settimana".* **Holowczak e altri** (QQQ 2006, tutto il flusso, senza gruppi): il
  flusso va con il prezzo **nello stesso minuto** (fino a 0,41), sul minuto dopo circa 0,01, significativo
  solo trattando i minuti come indipendenti.
- **Il supplemento su SSRN, letto e verificato.** **Kaeck, van Kervel, Seeger** (KVS, preprint, 2014, tutti
  gli scambi di opzioni SPX mensili da 7 a 180 giorni, **senza gruppi e senza 0DTE**): il flusso sommato come il
  DEX (contratti per delta per segno) va con l'S&P 500 **nella stessa ora** (98,13 cents in quattro ore per
  deviazione standard, t 4,41); i ritardi non predicono (t 0,6 e 0,4), e il prezzo dell'ora prima predice il
  flusso (t 5,7). "Informato" nel paper vuol dire impatto permanente, non previsione; e l'ordinamento del VAR
  da' al delta la parte comune con SPY (verifica): con SPY prima l'effetto scende a circa 70-75 cents. **Ni,
  Stouraitis, Wang** (NSW, preprint, Cboe Open-Close 1991-2018, mensile): lo sbilancio dei clienti pubblici
  sulle SPX **in the money** (call comprate meno put comprate, in apertura) predice il mercato **al contrario**
  a uno-tre mesi (mese dopo t -3,03; tre mesi t -3,85), solo sulle ITM, e le put **fuori dal denaro** vanno nel
  verso opposto. **Amaya, Garcia-Ares, Pearson, Vasquez** (AGPV, preprint, OPRA 2022-2026, **senza tipo di
  cliente**): gli 0DTE di SPX hanno **raffiche** entro pochi secondi da ogni mezz'ora, poi dai quarti d'ora e dai
  multipli di cinque minuti, crescenti ogni anno; **nessuna prova sul prezzo dell'indice**. *Nuovo.*
- **La nostra misura** su 35 sedute di X-Ray (sotto): dentro la seduta il flusso dei clienti non
  professionali va **contro** il prezzo e quello dei professionali **con** il prezzo; tolta la parte che il
  prezzo spiega, nessun gruppo predice il pomeriggio.

## I Candidati

**tipo**: R rivista con revisione fra pari, P preprint o working paper, T tesi. **Gia' nell'indice** e
quindi non ricercati: Chordia, Kurov, Muravyev, Subrahmanyam 2021 e Holowczak, Hu, Wu 2014 (ricerca
[`2026-11-09-opzioni-fra-indici`](2026-11-09-opzioni-fra-indici.md), entrambi `trovato` e bloccati da
Cloudflare: si riusano, non si duplicano); Vasquez e altri 2025 (VAPG, analizzato: usa i dati Cboe per
tipo di partecipante per la gamma dei market maker); Lakonishok e altri 2007 (trovato); Ni e altri 2021
(trovato); Adams e altri e Dim, Eraker, Vilkov (trovati, vedi sotto: il primo e' ora libero e assorbe il
secondo).

| chiave | titolo | autori, anno | id | tipo | pdf libero | perche' si', perche' no | stato |
|---|---|---|---|---|---|---|---|
| adams-2025-do-sp500-options-increase-market-volatility | *Do S&P 500 Options Increase Market Volatility? Evidence from 0DTEs* | Greg Adams (Wharton), Chukwuma Dim (George Washington), Bjorn Eraker (Wisconsin-Madison), Jean-Sebastien Fontaine (Bank of Canada), Chay Ornthanalai (Rotman), Grigory Vilkov (Frankfurt School); versione del 15 luglio 2026 | [SSRN 5641974](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=5641974) | P | **si'**: [jean-sebastienfontaine.com/papers/0dte-options-volatility.pdf](https://www.jean-sebastienfontaine.com/papers/0dte-options-volatility.pdf), raggiunto il 9 novembre 2026 (pagina di un coautore) | si': **lo stesso dato di OptionsDepth**. Dal PDF (§3.4): Cboe Open-Close Volume Summary di SPX a 10 minuti, cumulato dall'inizio del giorno, con i **quattro gruppi** Customers, Professional Customers, Firm Proprietary, Broker-Dealers e i quattro versi (open-buy, open-sell, close-buy, close-sell), gennaio 2018 - dicembre 2024; lo sbilancio per contratto e gruppo (eq. 7); la posizione 0DTE dei clienti dentro la giornata (§3.7); il flusso degli E-mini a 10 minuti (§4.3). Risponde a 1, 2 (intraday), 4 (0DTE) e 5 (flusso diviso per il volume). Assorbe *0DTEs: Trading, Gamma Risk and Volatility Propagation* (Dim, Eraker, Vilkov, SSRN 4692190), lo dice il frontespizio. No: il finding principale e' la gamma dei market maker, non quale gruppo e' informato; non revisionato | analizzato (verificato il 9 novembre 2026; versione del 15 luglio 2026, pagina di Fontaine), sha256 `6b256ade06bd64282dd8cfd4855521dfb1b0f404b687c0191c0edd61d73791ea` |
| pan-2006-information-in-option-volume-for-future-stock-prices | *The Information in Option Volume for Future Stock Prices* (NBER: *The Information of Option Volume for Future Stock Prices*) | Jun Pan (MIT Sloan, NBER), Allen M. Poteshman (Illinois Urbana-Champaign), 2006 | Review of Financial Studies 19(3), 871-908 ([RePEc](https://econpapers.repec.org/RePEc:oup:rfinst:v:19:y:2006:i:3:p:871-908)); NBER w10925, doi [10.3386/w10925](https://doi.org/10.3386/w10925) | R | **si'**, la versione NBER del novembre 2004: [nber.org/system/files/working_papers/w10925/w10925.pdf](https://nber.org/system/files/working_papers/w10925/w10925.pdf), raggiunto | si': **il riferimento per open-buy e gruppi**, su dati CBOE con il codice d'origine 1990-2001. Dal PDF (Tab. 8, §4.7, Tab. 9): sui titoli l'open-buy e' il volume che porta informazione; **sugli indici (SPX, OEX, NDX) nessun gruppo e' informato** il giorno dopo; sull'NDX i clienti dei broker a sconto e di altri broker sono contrarian e significativi (t 3,11 e 3,09); i conti proprietari valgono oltre il 20% del volume di SPX e NDX. Risponde a 1, 2, 3. No: dati vecchi, prima del retail di massa e degli 0DTE; orizzonte giornaliero | analizzato (verificato il 9 novembre 2026; NBER w10925, novembre 2004, non la pubblicata), sha256 `a58fe3a07cd58aed20aa63381ef225204f27a7bd49f7769ca1c3521bf554e5a0` |
| cao-2026-betting-against-the-crowd | *Betting Against the Crowd: Option Trading and Market Risk Premium* | Jie Cao (Hong Kong PolyU), Gang Li (CUHK), Xintong Zhan (Fudan), Guofu Zhou (Washington University in St. Louis); versione del gennaio 2026 | doi [10.1017/S0022109026102889](https://doi.org/10.1017/s0022109026102889), Journal of Financial and Quantitative Analysis (in uscita); SSRN 4301015 | R | **si'**: [jfqa.org/wp-content/uploads/2026/03/26295_Bet_Against_the_Crowd.pdf](https://jfqa.org/wp-content/uploads/2026/03/26295_Bet_Against_the_Crowd.pdf), raggiunto (versione accettata, sul sito della rivista) | si': **il retail come segnale contrarian sul mercato**, con i dati CBOE Open-Close per origine e per **taglia dell'ordine** (meno di 100, 100-199, oltre 199 contratti), 2005-2020 (dal PDF, §2). Lo sbilancio open-buy meno open-sell diviso per il volume (fra -1 e +1) dei clienti, sommato sui titoli, predice l'indice al contrario; lo guidano i piccoli; per market maker e professionisti il confronto e' nel paper; una sezione prova le opzioni su **SPX, NDX**, RUT e DJX (§5, Tab. A11). Risponde a 1, 2 (giorno, settimana, mese), 3, 4 (retail), 5. No: il segnale principale e' sulle opzioni dei titoli, non degli indici; nessun dato intraday | analizzato (verificato il 9 novembre 2026; versione accettata del gennaio 2026, jfqa.org), sha256 `f1a5055f3b7f4c0026ea5e3a9a5e164e6241106180744a200f0e9af467af55d5` |
| beckmeyer-2023-retail-traders-love-0dte-options | *Retail Traders Love 0DTE Options... But Should They?* | Heiner Beckmeyer, Nicole Branger, Leander Gayda (Universita' di Munster), 30 marzo 2023 | [SSRN 4404704](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4404704), doi 10.2139/ssrn.4404704 | P | no: solo SSRN (Cloudflare) | si': **il retail sugli 0DTE di SPX**, il gruppo e la scadenza che il riquadro separa. Da riassunti di terzi (da verificare): 0DTE circa tre quarti degli scambi retail su opzioni S&P 500, dati Cboe gennaio 2021 - febbraio 2023, il retail perde in media e la perdita cresce dopo le scadenze giornaliere (maggio 2022). Adams e altri lo citano per chi guida la domanda di 0DTE. Risponde a 4. No: non dice se il flusso retail anticipa l'indice; preprint | analizzato (verificato il 9 novembre 2026; SSRN 4404704, versione del 27 luglio 2024, prima del 30 marzo 2023, 50 pagine), sha256 `e97a8685aceef757121377a5e635b8c71a8638a37fb5d748c793c707f9695c50` |
| maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk | *Intraday 0DTE Option Order Flow and S&P 500 Downside Risk* | Maceo Maurer, Mirko Muller (Frankfurt School of Finance & Management), 2026 | [SSRN 7339718](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7339718), doi 10.2139/ssrn.7339718 | P | no: solo SSRN (Cloudflare) | si': **l'unico trovato che prova il flusso 0DTE contro il rendimento dell'ora dopo** su S&P 500. Dalla sola descrizione della ricerca web (l'abstract non e' su OpenAlex, **da verificare in lettura**): 1.027 giornate da maggio 2022 a giugno 2026; un rapporto put-call d'apertura pesato per la gamma sbilanciato sulle put e' seguito da rendimenti piu' deboli nell'ora dopo e da piu' probabilita' di un'ora negativa. Risponde a 2 (intraday) e 4. No: non si sa se separa i gruppi; preprint recentissimo; Maurer e' anche l'autore di SSRN 6650858, non revisionato | analizzato (verificato il 9 novembre 2026; SSRN 7339718, versione dell'agosto 2026, 83 pagine; dal frontespizio: viene dalla tesi di master degli autori alla Frankfurt School), sha256 `b140ae464b89c930769af21eefd8b41ce06588c3916daac65562a4fdb06cd71d` |
| michael-2022-option-volume-imbalance-as-a-predictor | *Option Volume Imbalance as a predictor for equity market returns* | Nikolas Michael, Mihai Cucuringu, Sam Howison (Oxford, Alan Turing Institute), gennaio 2022 | arXiv [2201.09319](https://arxiv.org/abs/2201.09319) v1; SSRN 4019647 | P | **si'**: [arxiv.org/pdf/2201.09319v1](https://arxiv.org/pdf/2201.09319v1), raggiunto | si': **i cinque gruppi e la normalizzazione**. Dal PDF (§2.2, §3): dati Nasdaq NOM e PHLX 2015-2019, volume **cumulato a intervalli di 10 minuti** dalle 9:40 alle 16:00, per Firm, Broker, Market Maker, Customer e Professional Customer, con open e close; lo sbilancio normalizzato (OVI, fra -1 e +1) per gruppo; i segnali piu' forti vengono dai market maker e dai clienti, firm e professionisti sono rumorosi (§3.1, §5); appendice sugli schemi intraday. Risponde a 1, 3 e 5. No: soprattutto titoli e 11 ETF, rendimento in eccesso sull'SPY (sezione trasversale), notturno; preprint | analizzato (verificato il 9 novembre 2026; arXiv v1, 23 gennaio 2022), sha256 `db09bc5cb1f187475736675bbedf261cf7e2187528c9ae94a313b9172dfbfb82` |
| chen-2019-demand-for-crash-insurance | *Demand for Crash Insurance, Intermediary Constraints, and Risk Premia in Financial Markets* | Hui Chen (MIT), Scott Joslin (USC), Sophie X. Ni (HKUST), 2019 | Review of Financial Studies 32(1), 228-265 ([RePEc](https://econpapers.repec.org/RePEc:oup:rfinst:v:32:y:2019:i:1:p:228-265.)); NBER w25573, doi [10.3386/w25573](https://doi.org/10.3386/w25573) | R | si': [nber.org/system/files/working_papers/w25573/w25573.pdf](https://www.nber.org/system/files/working_papers/w25573/w25573.pdf), raggiunto | dall'abstract e dall'introduzione: il netto comprato ogni mese dagli investitori pubblici in put SPX molto fuori dal denaro, che e' lo specchio di quanto vendono i broker-dealer, predice i rendimenti dei mesi dopo, soprattutto nei periodi in cui prezzo e quantita' delle put si muovono in versi opposti. E' su indice e per gruppo, ma l'orizzonte e' di mesi e la lettura passa dai vincoli dei dealer, non da un gruppo informato | scartato, da tenere |
| chordia-2021-index-option-trading-activity-and-market-returns | *Index Option Trading Activity and Market Returns* | Chordia, Kurov, Muravyev, Subrahmanyam, 2021 | Management Science 67(3); SSRN 2798390 | R | no (Cloudflare, due verifiche) | gia' nell'indice dalla ricerca sulle opzioni fra indici: il flusso settimanale delle put su indici dei clienti ISE predice l'S&P 500. **Risponde a 1 e 2 meglio di tutti** sugli indici; si riusa, non si duplica | analizzato (verificato il 9 novembre 2026; versione accettata del 17 ottobre 2019, non la pubblicata) in `opzioni-gamma/chordia-2021-index-option-trading-activity-and-market-returns/`, sha256 `fa1b5c768f3f` (completo nella ricerca [opzioni-fra-indici](2026-11-09-opzioni-fra-indici.md)) |
| holowczak-2014-aggregating-information-in-option-transactions | *Aggregating Information in Option Transactions* | Holowczak, Hu, Wu, 2014 | Journal of Derivatives 21(3); SSRN 1740046 | R | no (Cloudflare; copia SMU dietro anti-bot) | gia' nell'indice: il flusso delle opzioni su QQQ pesato per il delta, intraday. **Risponde a 3 e 5**; si riusa | analizzato (verificato il 9 novembre 2026; working paper del 13 gennaio 2011, non la pubblicata) in `opzioni-gamma/holowczak-2014-aggregating-information-in-option-transactions/`, sha256 `343b8e738893` (completo nella ricerca [opzioni-fra-indici](2026-11-09-opzioni-fra-indici.md)) |
| dim-2024-0dtes-trading-gamma-risk-and-volatility-propagation | *0DTEs: Trading, Gamma Risk and Volatility Propagation* | Dim, Eraker, Vilkov, 2024 | SSRN 4692190 | P | no | gia' nell'indice; **assorbito** dalla versione 2026 di Adams e altri (lo dice il frontespizio di quest'ultima): si legge quella | scartato (assorbito) |
| bogousslavsky-2024-an-anatomy-of-retail-option-trading | *An Anatomy of Retail Option Trading* | Vincent Bogousslavsky (Boston College), Dmitriy Muravyev (Michigan State), 2024 | SSRN 4682388 | P | no: solo SSRN | da riassunti: conti retail di un diario di trading 2020-2022, il trade piu' comune e' la call di un giorno sull'S&P 500, perdite modeste. Descrittivo, e il dato non e' quello di Cboe: Beckmeyer e altri e' piu' vicino al riquadro | scartato |
| bryzgalova-2023-retail-trading-in-options | *Retail Trading in Options and the Rise of the Big Three Wholesalers* | Svetlana Bryzgalova, Anna Pavlova, Taisiya Sikorskaya, 2023 | doi [10.1111/jofi.13285](https://doi.org/10.1111/jofi.13285), Journal of Finance 78(6) | R | si' (Wiley, accesso aperto; copia LBS) | il retail e' oltre il 60% del volume, preferisce le settimanali economiche, perde in media (dall'abstract). Soprattutto opzioni su titoli, e non misura se il flusso anticipa il prezzo | scartato |
| de-silva-2025-losing-is-optional | *Losing is optional: retail option trading and expected announcement volatility* | Tim de Silva, Kevin C. Smith, Eric C. So, 2025 | doi [10.1093/rof/rfaf052](https://doi.org/10.1093/rof/rfaf052), Review of Finance 30; SSRN 4050165 | R | no | il retail perde sulle opzioni dei titoli intorno alle trimestrali: titoli, non indici | scartato |
| easley-1998-option-volume-and-stock-prices | *Option Volume and Stock Prices: Evidence on Where Informed Traders Trade* | David Easley, Maureen O'Hara, P. S. Srinivas, 1998 | doi [10.1111/0022-1082.194060](https://doi.org/10.1111/0022-1082.194060), Journal of Finance | R | si' (Wiley) | il classico: il volume delle opzioni "positivo" e "negativo" anticipa il prezzo del titolo. Solo titoli, senza gruppi | scartato |
| hu-2014-does-option-trading-convey-stock-price-information | *Does option trading convey stock price information?* | Jianfeng Hu, 2014 | doi [10.1016/j.jfineco.2013.12.004](https://doi.org/10.1016/j.jfineco.2013.12.004), JFE 111(3), 625-645 | R | no (SSRN 1970702) | lo sbilancio indotto dalle opzioni (delta) predice i titoli, quello indipendente solo per poco: e' il meccanismo della copertura. Solo titoli; lo stesso autore e la stessa misura sono in Holowczak e altri, gia' nell'indice, che ha QQQ | scartato |
| bollen-2004-does-net-buying-pressure-affect-implied-volatility | *Does Net Buying Pressure Affect the Shape of Implied Volatility Functions?* | Nicolas Bollen, Robert Whaley, 2004 | doi [10.1111/j.1540-6261.2004.00647.x](https://doi.org/10.1111/j.1540-6261.2004.00647.x), Journal of Finance | R | si' (Wiley) | definisce la net buying pressure su SPX con i dati CBOE per origine, ma la prova sulla **volatilita' implicita**, non sul prezzo dell'indice | scartato |
| muravyev-2016-order-flow-and-expected-option-returns | *Order Flow and Expected Option Returns* | Dmitriy Muravyev, 2016 | doi [10.1111/jofi.12380](https://doi.org/10.1111/jofi.12380), Journal of Finance | R | no (SSRN 1963865) | lo sbilancio predice il rendimento **dell'opzione** (rischio d'inventario del market maker), non del sottostante | scartato |
| ge-2016-why-does-the-option-to-stock-volume-ratio-predict | *Why does the option to stock volume ratio predict stock returns?* | Li Ge, Tse-Chun Lin, Neil D. Pearson, 2016 | doi [10.1016/j.jfineco.2015.08.019](https://doi.org/10.1016/j.jfineco.2015.08.019), JFE 120, 601-622 | R | no (SSRN 2329714) | titoli; scompone il volume per tipo d'ordine e trova l'informazione negli acquisti in apertura. Pan e Poteshman copre lo stesso con gli indici | scartato |
| pan-w-2026-order-flow-toxicity-zero-day-spx | *Order Flow Toxicity and Cross-Market Price Impact in Zero-Day SPX Options* | William Pan, 2026 | [Princeton, tesi ORFE](https://theses-dissertations.princeton.edu/entities/publication/4a62bed4-0ad0-4c2d-87d4-72114ea4ff60) | T | forse (PDF elencato, accesso non dichiarato) | dalla pagina: il delta netto firmato degli 0DTE di SPX al minuto ha un impatto persistente su ES (0,48 punti per deviazione standard a un minuto, nessun ritorno in due ore), gennaio-giugno 2024. Vicinissimo al dato, ma **tesi triennale**, non revisionata, sei mesi, e senza gruppi | scartato |
| agarwal-2025-animal-spirits-on-steroids | *Animal Spirits on Steroids: Evidence from Retail Options Trading in India* | Vikas Agarwal, Pulak Ghosh, Nagpurnanand Prabhala, Haibei Zhao, 2025 | CFR WP 25-09; SSRN 5430635 | P | si' (CFR Cologne) | dal frontespizio: il retail indiano sulle opzioni su indici, day trading e 0DTE, perdite. Mercato indiano, nessun legame col prezzo dell'indice | scartato |
| eaton-2022-retail-option-traders-and-the-implied-volatility-surface | *Retail Option Traders and the Implied Volatility Surface* | Gregory W. Eaton, T. Clifton Green, Brian S. Roseman, Yanbin Wu, settembre 2022 | (copia NHH) | P | si' | dal frontespizio: il retail compra call brevi fuori dal denaro e muove la volatilita' implicita. Volatilita', non prezzo; titoli | scartato |
| kaeck-2017-informed-trading-in-the-index-option-market | *Informed Trading in the Index Option Market* | Andreas Kaeck (Sussex), Vincent van Kervel (Universidad de los Andes), Norman Seeger (VU Amsterdam); scritto il 5 giugno 2017, rivisto il 17 agosto 2019 | [SSRN 2981332](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2981332), doi 10.2139/ssrn.2981332; TILEC Discussion Paper 2017-027 | P (sulla pagina nessuna rivista) | no: PDF su SSRN (51 pagine), serve il browser | si': **il flusso delle opzioni S&P 500 scomposto in delta e vega**, cioe' lo stesso peso del DEX di X-Ray. Dall'abstract SSRN: un VAR strutturale (un modello in cui ogni serie dipende dal passato di tutte) del trading informato; il flusso di strike e scadenze diverse si somma attraverso il delta (esposizione al prezzo) e il vega (alla volatilita'); "S&P500 option trades are indeed informative" sui movimenti del sottostante e della volatilita'. Risponde a 1 (informato o no, sull'indice), 2 (dentro la giornata, da verificare) e 5 (come si somma). Trovato fra le citazioni di Chordia e altri. No: preprint mai pubblicato (per quanto dice la pagina); dati, periodo e gruppi non nell'abstract, **da verificare in lettura** | analizzato (verificato il 9 novembre 2026; SSRN 2981332, versione rivista del 17 agosto 2019 secondo SSRN, il PDF non porta data, 51 pagine; letto: tutti gli scambi senza gruppi, solo il 2014, niente 0DTE; stessa ora si', previsione no), sha256 `e8593596f3c4a7286f847dce2b864e130c3516ac4ee9eaaf7614b5d5bee00113` |
| wang-2021-index-options-trading-and-sentiment | *Index Options Trading and Sentiment* | Li Wang (Case Western Reserve), Sophie Xiaoyan Ni (Hong Kong Baptist), Aris Stouraitis (Hong Kong Baptist); 18 novembre 2021 | [SSRN 3981994](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3981994), doi 10.2139/ssrn.3981994 | P | no: PDF su SSRN (38 pagine), serve il browser | si': **lo sbilancio dei clienti pubblici sulle opzioni SPX, contrarian, sull'indice**. Dall'abstract SSRN: lo sbilancio direzionale pubblico delle opzioni SPX in the money ("DOI") predice **al contrario** il rendimento del mercato a un mese, fino a tre, al netto delle misure di sentiment note; piu' forte nelle recessioni (limiti all'arbitraggio) e sui titoli tecnologici. E' probabilmente la fonte del riassunto di Harbourfront Quant escluso sopra. Risponde a 1, 2 (mese) e 3; Ni e' coautrice di Chen, Joslin, Ni e di Ni, Pearson, Poteshman, White. No: orizzonte mensile, non intraday; preprint | analizzato (verificato il 9 novembre 2026; SSRN 3981994, versione dell'8 dicembre 2021, 38 pagine; sul frontespizio l'ordine e' Ni, Stouraitis, Wang, sigla NSW: **la chiave `wang-2021` non segue la regola del primo autore**, segnalato, non rinominato), sha256 `74e51c85f0881efa8809f9052518ca3db5d9678c2d8aaa97934c0b20ba003d39` |
| garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders | *The Rise of Algorithmic Retail Option Traders* | Pedro Angel Garcia-Ares (ITAM), Diego Amaya (Wilfrid Laurier), Neil D. Pearson (Illinois Urbana-Champaign), Aurelio Vasquez (ITAM); scritto il 14 febbraio 2026, rivisto il 13 agosto 2026 | [SSRN 6480379](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=6480379), doi 10.2139/ssrn.6480379 | P | no: PDF su SSRN (50 pagine), serve il browser | si': **il flusso retail 0DTE di SPX dentro la giornata, minuto per minuto**. Dall'abstract SSRN: con i record OPRA (il nastro consolidato delle opzioni USA) da maggio 2022 a gennaio 2026, gli scambi 0DTE di SPX hanno **raffiche entro pochi secondi da ogni mezz'ora**, finite in un minuto e crescenti ogni anno: poche strategie a rischio definito con strike, ampiezze e incassi da modelli delle piattaforme di automazione retail; alle raffiche lo spread quotato si stringe e quello effettivo si allarga. Risponde a 4 e soprattutto a 5: il confronto con lo storico alla stessa ora deve tener conto di raffiche a orari fissi. Pearson e Vasquez sono autori di VAPG. No: non prova il flusso contro il prezzo dell'indice; preprint | analizzato (verificato il 9 novembre 2026; SSRN 6480379, versione del 12 agosto 2026, 50 pagine), sha256 `8eda5c817adad7c8a4e63b57685747174f163d1f29d7afd85030a29774e2c1c7` |
| maleki-2026-cboe-open-close-labels-open-interest | *Do Cboe Open-Close Option Volume Labels Reconcile with Open Interest?* | Majid Maleki (Concordia, John Molson), 13 luglio 2026 | [SSRN 7365160](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7365160) | P | no: SSRN | dall'abstract: su SPX/SPXW da giugno 2012 a marzo 2022 l'11,48% delle osservazioni serie-giorno non si riconcilia con la variazione dell'open interest (OCC); con i dati scambio per scambio di aprile 2026 le violazioni salgono al 31,25-45,90%: le etichette apertura/chiusura si scrivono prima della compensazione. Conta per ogni misura che separa apertura e chiusura (l'open-buy di PP e CLZZ). Non dice chi e' informato | scartato, da tenere (qualita' del dato Open-Close) |
| bae-2017-options-and-futures-different-types-of-information | *Do Investors Use Options and Futures to Trade on Different Types of Information? Evidence from an Aggregate Stock Index* | Kyoung-hun Bae (UNIST), Peter Dixon (SEC), 22 maggio 2017 | [SSRN 2767207](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2767207), Journal of Futures Markets | R | no: SSRN | dall'abstract: sul KOSPI 200 lo sbilancio fra istituzionali e retail sui **future** predice il rendimento a breve, quello sulle **opzioni** predice la volatilita', non il rendimento. Ora verificato alla fonte (sopra era fra gli esclusi). Risponde a 1 per gruppo, ma su un mercato coreano | scartato, da tenere |
| lee-2016-information-content-investor-trading-taiwan-index-options | *Information Content of Investor Trading Behavior: Evidence from Taiwan Index Options Market* | Yen-Hsien Lee, David K. Wang, 2016 | [SSRN 2759299](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=2759299), Pacific-Basin Finance Journal | R | no: SSRN | dall'abstract: fra quattro classi di operatori sulle opzioni TAIEX solo gli istituzionali esteri predicono, soprattutto fuori dal denaro e a breve. Mercato taiwanese | scartato |
| han-2009-investor-trading-behavior-taiwan-index-options | *Investor Trading Behavior and Performances: Evidence from Taiwan Stock Index Options* | Bing Han, Yi-Tsung Lee, Yu-Jane Liu, 2009 | [SSRN 1343270](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=1343270), McCombs FIN-06-09 | P | no: SSRN | dall'abstract: sulle opzioni TAIEX i privati perdono e gli istituzionali esteri guadagnano; gli ordini aggressivi indovinano piu' spesso il verso dell'indice. Mercato taiwanese, rendimento degli operatori | scartato |
| lin-2026-when-does-intraday-options-trading-predict-stock-returns | *When Does Intraday Options Trading Predict Stock Returns?* | Tse-Chun Lin, Xingguo Luo, Shidong Shao, 2026 | [SSRN 7318198](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7318198) | P | no: SSRN | dall'abstract: lo sbilancio a 30 minuti (CBOE e ISE) predice il rendimento del titolo il giorno dopo, piu' forte vicino alla chiusura, guidato da open-buy e ordini piccoli. Titoli, non indici | scartato |
| li-2026-informed-trading-covid-sp500-index-options | *Informed Trading during the COVID-19 Pandemic: Evidence from S&P 500 Index Options* | Wei-Xuan Li, Joseph J. French, James Nguyen, 2026 | [SSRN 7309421](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7309421) | P | no: SSRN | dall'abstract: costi di selezione avversa sulle SPXW intorno al COVID; il rapporto volume opzioni/azioni contiene informazione sui rendimenti futuri dell'S&P 500. Nessun gruppo, periodo speciale | scartato |
| amaya-2023-price-discovery-in-option-markets-sp500 | *Price Discovery in Option Markets: Evidence from S&P 500 Index Options* | Diego Amaya, M. Fabricio Perez, 2023 | [SSRN 4615609](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4615609) | P | no: SSRN | dall'abstract: gli informati preferiscono put, brevi e al denaro, sempre piu' le settimanali. Per tipo di contratto, non per gruppo | scartato |
| odonovan-2023-retail-option-trading-and-market-quality | *Retail Option Trading and Market Quality: Evidence from High-Frequency Data* | James O'Donovan, Gloria Yang Yu, Jinyuan Zhang, 2023 | [SSRN 4613741](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4613741) | P | no: SSRN | dall'abstract: la copertura in delta del flusso retail non informato peggiora la liquidita'. Liquidita' dei titoli, non prezzo dell'indice | scartato |
| zeng-2026-the-retail-put | *The Retail Put* | Xuran Zeng (NYU Stern), 31 luglio 2026 | [SSRN 7213482](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7213482) | P | no: SSRN | dall'abstract: il flusso retail ad alta frequenza **compra dopo i ribassi** del mercato; un punto percentuale di acquisti netti alza il rendimento dei 30 minuti dopo di 40 punti base, senza ritorno. Ricorda la nostra misura (i clienti non professionali comprano delta quando SPX scende), ma e' flusso **azionario**, non di opzioni | scartato, da tenere |
| adams-2024-the-market-for-0dte | *The Market for 0DTE: The Role of Liquidity Providers in Volatility Attenuation* | Greg Adams, Jean-Sebastien Fontaine, Chayawat Ornthanalai, 2024 | [SSRN 4881008](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4881008) | P | no | **assorbito** da adams-2025 (lo dice l'analisi di ADEFOV, frontespizio) | scartato (assorbito) |
| pederzoli-2024-market-makers-and-the-dynamics-of-volatility-demand | *Market Makers and the Dynamics of Volatility Demand* | Paola Pederzoli, Kris Jacobs, Anh Thu Mai, 2025 | [SSRN 5052438](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=5052438) | P | no: SSRN | dall'abstract: domanda di market maker e utenti finali sulle opzioni VIX. Opzioni sul VIX, non flusso su SPX | scartato |

Esclusi perche' non verificabili alla fonte o senza sede: gli studi su KOSPI 200 e TAIEX per tipo
d'investitore (Bae e Dixon 2018, Journal of Futures Markets, e tesi taiwanesi; visti solo come riassunti, e
i mercati asiatici non sono la domanda); il riassunto di Harbourfront Quant sullo sbilancio delle opzioni SPX
in the money dei clienti pubblici (fonte non identificata); le note di Cboe, UBS e dei fornitori sul
retail negli 0DTE (affermazioni senza test pubblicati). *Aggiornato con il supplemento su SSRN*: Bae e Dixon
e due studi taiwanesi sono ora verificati sulla pagina SSRN (righe in tabella, scartati); il riassunto di
Harbourfront corrisponde all'abstract di Wang, Ni, Stouraitis (SSRN 3981994), ora in tabella. Singh 2026
(SSRN 7350465), riapparso nella ricerca, e' gia' scartato nella [ricerca del 15 ottobre](2026-10-15-gamma-0dte-volatilita-intraday.md)
e non si ripropone: l'abstract ora letto dice che misura una GEX di flusso stimata da un fornitore, contro la
varianza, non un gruppo contro il prezzo.

## La Scelta

Sei, piu' i due gia' nell'indice da riusare (Chordia e altri; Holowczak e altri).

- **adams-2025-do-sp500-options-increase-market-volatility**: e' l'unico con **lo stesso dato del
  riquadro** (Cboe Open-Close di SPX, quattro gruppi uguali a quelli di OptionsDepth, a 10 minuti, cumulato
  dall'inizio del giorno, 0DTE separati) e un periodo lungo e recente (2018-2024). Dice come si passa dal
  flusso dei gruppi alla posizione dei market maker, e cosa fanno i clienti sugli 0DTE dentro la giornata.
  PDF libero, versione 2026 che assorbe Dim, Eraker e Vilkov. Filone principale: opzioni-gamma.
- **pan-2006-information-in-option-volume-for-future-stock-prices**: il riferimento revisionato per
  **chi e' informato, gruppo per gruppo, anche su SPX e NDX**, con il perche' (informazione privata sui
  titoli, non sull'indice). Il suo risultato negativo sugli indici e' una risposta: e' quello che il riquadro
  non deve far credere. PDF libero (NBER).
- **cao-2026-betting-against-the-crowd**: **il retail come segnale contrarian** sul mercato, dal giorno al
  trimestre, con i dati Cboe Open-Close per origine e per taglia (cioe' il modo di isolare i piccoli), la
  misura normalizzata fra -1 e +1 e una prova su SPX e NDX. Revisionato (JFQA). PDF libero.
- **beckmeyer-2023-retail-traders-love-0dte-options**: il **retail sugli 0DTE di SPX**, i due tagli che
  il riquadro fa; da Cboe. Serve per dire cosa e' il flusso retail 0DTE (gioco o informazione). Solo SSRN.
- **maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk**: l'unico trovato che prova
  **il flusso 0DTE di apertura contro l'ora dopo**, cioe' l'orizzonte della lettura dal vivo, 2022-2026.
  Preprint, abstract non letto: entra come candidato da verificare in lettura. Solo SSRN.
- **michael-2022-option-volume-imbalance-as-a-predictor**: **i cinque gruppi a 10 minuti e la
  normalizzazione** dello sbilancio, con il confronto fra gruppi (market maker e clienti informativi, firm e
  professionisti rumorosi). Risponde alla sotto-domanda 5 meglio degli altri. Preprint, soprattutto titoli.
  PDF libero (arXiv).

**Perche' non gli altri.** Easley e altri, Hu, Ge e altri, Muravyev, de Silva e altri sono revisionati ma
sui titoli, o sul prezzo dell'opzione e non del sottostante. Bollen e Whaley ha la misura giusta su SPX ma
la prova sulla volatilita' implicita. Chen, Joslin e Ni e' su SPX e per gruppo, ma a mesi e con una lettura
sui vincoli dei dealer: da tenere se la sintesi chiede l'orizzonte lungo. Bryzgalova e altri e Bogousslavsky
e Muravyev descrivono il retail senza provare se anticipa l'indice. La tesi di Princeton e' il piu' vicino al
dato al minuto, ma e' una tesi triennale senza revisione.

**Il supplemento su SSRN (9 novembre 2026, sera): tre in piu'.** Tutti preprint, PDF solo su SSRN (serve il
browser, stessa sessione di Playwright).

- **kaeck-2017-informed-trading-in-the-index-option-market** (SSRN 2981332): l'unico trovato che prova
  **se il flusso delle opzioni S&P 500 e' informato sull'indice**, sommandolo per delta e vega come fa il DEX
  di X-Ray, con un modello dinamico (VAR) che dice anche l'orizzonte. Risponde a 1, 2 e 5. Gruppi e periodo
  da verificare in lettura.
- **wang-2021-index-options-trading-and-sentiment** (SSRN 3981994): **lo sbilancio dei clienti pubblici sulle
  SPX** che predice l'indice al contrario a uno-tre mesi, con il perche' (sentiment, limiti all'arbitraggio).
  E' la fonte del riassunto finora escluso, e completa PP e CLZZ sugli indici con dati piu' recenti.
- **garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders** (SSRN 6480379): **il retail 0DTE di SPX
  dentro la giornata**, con raffiche a ogni mezz'ora, 2022-2026, autori di VAPG. Serve alla sotto-domanda 5:
  la lettura "rispetto allo storico alla stessa ora" deve sapere che il flusso ha picchi a orari fissi.

Fuori, con il motivo in tabella: Maleki (qualita' delle etichette Open-Close, da tenere), Bae e Dixon (KOSPI,
revisionato, da tenere), Zeng (flusso retail azionario, da tenere), gli studi taiwanesi, Lin e altri (titoli),
Li e altri, Amaya e Perez, O'Donovan e altri, Pederzoli e altri, e Adams e altri 2024 (assorbito).

**Cosa resta scoperto, da dire all'utente.** Nessun paper trovato prova, **su SPX e al minuto, quale dei
quattro gruppi di OptionsDepth anticipa il prezzo**. Le prove sugli indici dicono: nessun gruppo informato
al giorno dopo (Pan e Poteshman, dati 1990-2001), i piccoli contrarian (Pan e Poteshman su NDX e, lo dice la
tabella ma non il testo, su OEX; Cao e altri sul mercato, ma dalle opzioni dei titoli), i **piccoli** clienti
(fino a 100 contratti) che comprano put su Russell 2000 e Nasdaq-100, seguiti da un S&P 500 piu' alto a settimane, che gli
autori leggono come assicurazione e non come informazione (Chordia e altri; *corretto nella fase 5: qui la
prima versione diceva "i clienti nel complesso informativi sulle put a una settimana"*). Per gli 0DTE
dentro la giornata ci sono Adams e altri (copertura, non informazione) e un preprint (Maurer e Muller). Il
riquadro puo' quindi **descrivere** chi ha comprato delta e quanto e' grande rispetto alla sua storia, non
dire chi ha ragione. *Dopo il supplemento letto*: KVS e' l'unico su SPX dentro la giornata con il delta, e
trova la stessa ora, non l'ora dopo; NSW aggiunge i clienti pubblici contrarian, ma a mesi e sulle sole ITM;
AGPV descrive il retail automatico agli orari tondi senza provare il prezzo. La frase sopra resta vera.

**Cosa e' rifacibile qui.** Lo storico del DEX per gruppo c'e' solo da quando `XrayLive` lo salva
(ottobre 2026): un confronto con lo storico alla stessa ora (z-score) e' possibile ma con poche settimane, e
su NQ il dato e' solo contesto (CLAUDE.md). Nessun backtest.

## La Sintesi

9 novembre 2026, fase 5, **aggiornata due volte lo stesso giorno**. Undici analisi verificate (per ognuna conta
la sezione "La Verifica" dell'analisi):

- le prime quattro: **ADEFOV** (Adams e altri, versione del 15 luglio 2026), **PP** (Pan e Poteshman, NBER
  2004), **CLZZ** (Cao e altri, versione accettata del gennaio 2026), **MCH** (Michael e altri, arXiv v1 2022);
- altre quattro, arrivate dopo Cloudflare o riusate dalla ricerca
  [opzioni-fra-indici](2026-11-09-opzioni-fra-indici.md): **BBG** (Beckmeyer e altri, versione del 27 luglio
  2024, non revisionato), **MAMU** (Maurer e Muller, agosto 2026, **tesi di master, non revisionata**),
  **CKMS** (Chordia e altri, versione accettata del 17 ottobre 2019, non la pubblicata), **HHW** (Holowczak e
  altri, working paper del 13 gennaio 2011, non la pubblicata);
- le ultime tre, dal supplemento su SSRN, tutte preprint non revisionati: **KVS** (Kaeck, van Kervel, Seeger,
  rivisto nel 2019), **NSW** (Ni, Stouraitis, Wang, 8 dicembre 2021; la cartella e' `wang-2021-...` e non segue
  la regola del primo autore: segnalato, non rinominato), **AGPV** (Amaya, Garcia-Ares, Pearson, Vasquez, 12
  agosto 2026; la cartella segue l'ordine della pagina SSRN, `garcia-ares-2026-...`).

**In parole semplici.** Nessuno degli undici paper trova un gruppo che, comprando o vendendo opzioni su un
indice, anticipi da che parte andra' l'indice: ne' il giorno dopo, ne' la settimana dopo, ne' nell'ora o nel
minuto dopo. Quelli che guardano dentro la giornata lo dicono in modo diretto: il flusso delle opzioni va
**insieme** al prezzo, nello stesso minuto (HHW) o nella stessa ora (KVS, sulle opzioni SPX, sommate come il
DEX), e quasi non dice niente del dopo; sugli 0DTE di SPX il delta comprato e il flusso con il segno non
anticipano l'ora dopo (MAMU). L'unica cosa che MAMU trova, **i primi cinque minuti** che pendono sulle put
vicino al prezzo, dice un po' di piu' **se** l'ora dopo scendera', e fuori campione non serve. Quando qualcosa
si vede su tempi lunghi, va al contrario: i **piccoli** che comprano put su altri indici, poi l'S&P 500 sale per
settimane (CKMS, paura che si paga); i clienti pubblici che comprano call in the money sulle SPX, poi il mercato
scende per mesi (NSW, ottimismo che si sgonfia). Il "retail" non e' una cosa sola: quello che si riconosce
dall'asta sugli 0DTE e' una fetta piccola e perde (BBG); quello che si riconosce dal comportamento sono **bot che
partono tutti allo scoccare della mezz'ora** (AGPV). Nessuno dei due coincide per forza con il gruppo che X-Ray
chiama retail. La nostra misura su 35 sedute dice lo stesso: il flusso dei clienti non professionali **ripete il
prezzo al rovescio**, quello dei professionali lo ripete dritto, e tolto il prezzo non resta niente che anticipi
il pomeriggio. Il riquadro puo' quindi dire **quanto** ha comprato un gruppo rispetto al solito e rispetto al
prezzo, non **chi ha ragione**.

### I Finding, Uno Per Paper

- **ADEFOV** [§3.4, §3.7-3.9, §4.1, Tab. 5, IA5]: stessi quattro gruppi del Cboe che usa OptionsDepth, SPX
  2018-2024 a 10 minuti. Nessuna prova di un gruppo contro il prezzo. Il delta scambiato dai quattro gruppi
  sommato (la Net Delta dei market maker, divisa per il rendimento) non predice la volatilita' dei 10 minuti
  dopo e non cambia nei giorni con 0DTE; predice la **gamma**, che e' uno stock ereditato dai giorni prima
  (circa meta' dal giorno prima, circa il 90% dalla settimana prima) e frena il mercato per circa un'ora. Il
  flusso netto dei clienti sugli 0DTE al denaro e' sotto i 5.000 contratti su circa un milione scambiati.
- **PP** [§4.1, §4.4, §4.6, §4.7, Tab. 6, 8, 9]: sui titoli il volume comprato in apertura (open-buy) da chi
  non e' market maker anticipa il prezzo per settimane (t -32,92); per classe i clienti si' (servizio
  completo t -37,00), i conti proprietari no (t -0,75). Sugli indici, il giorno
  dopo: SPX niente per nessuna classe; NDX e OEX contrarian in due classi ciascuno (t fra 3,09 e 3,60),
  dodici prove senza correzione. L'open-buy e' il tipo di volume che porta piu' informazione; il close-buy
  niente.
- **CLZZ** [§3.1, §4.1, §5.2, Tab. 2, 4, 7, A3, A11]: lo sbilancio delle call dei clienti sui titoli,
  sommato, predice il mercato al contrario (giorno -0,070, t -2,94; settimana -0,240, t -2,69; mese -1,024,
  t -2,91), guidato dagli ordini piccoli, e lo leggono come sentiment. Sulle opzioni di SPX e NDX niente al
  giorno e alla settimana; SPX al contrario solo a due mesi (t -2,74). Professionali e market maker
  (questi con firm e broker-dealer dentro) non predicono alla settimana.
- **MCH** [§5, §7.1, App. E, Fig. 3, 6]: sui titoli contro SPY, la notte dopo, i clienti predicono nel verso
  (Sharpe fino a 3,46), market maker e broker al contrario (fino a -4,42), firm e professionali niente. A 10
  minuti, dentro la giornata, **niente dopo la correzione di Bonferroni**. Nessuna prova su un indice.
- **BBG** [§3, §3.1, §4.1, §4.2, Fig. 2, Tab. 1, 4, 8]: il retail riconosciuto dall'asta del Cboe (codici
  SLAN/MLAT, ordini fino a 10 contratti, dal 22 febbraio 2021) e' il 2-4% del volume delle opzioni S&P 500 e
  oltre il 6% degli 0DTE nel 2022; circa tre quarti del suo volume e' negli 0DTE. Perde oltre 125 milioni di
  dollari netti fra febbraio 2021 e settembre 2023 (circa 30 lordi): -241.000 dollari al giorno netti, -57.000
  lordi, cioe' circa il 70-76% in costi. I market maker sono **corti di 0DTE** (sbilancio fra -0,5% e -1% del
  volume, sotto -1% da maggio 2022 a febbraio 2023): gli utenti finali comprano 0DTE e, dopo maggio 2022,
  vendono le altre scadenze. **Nessuna prova del flusso contro il prezzo.** Il delta con segno della Tab. 8
  (0,000, t 0,02) **non dice niente sul verso**: con gli effetti fissi del giorno la posizione netta del
  giorno finisce nella costante (correzione della verifica). Resta il lordo negativo, senza prova di
  significativita' e positivo per mesi (fine 2021 - maggio 2022). Il 61,7% delle operazioni retail 0DTE cade
  fra le 9:30 e le 13:00, ma sono 3,5 ore su 6,5: per ora il 17,6% contro il 12-13% del resto, una mattina
  **appena piu' densa**, contata in operazioni e non in delta.
- **MAMU** [§5.2-5.9, Tab. 7, 8, 9, 12, 14, 15, 23, §5.7]: volume 0DTE di SPX da ThetaData, di tutti, **senza
  gruppi**; 1.027 sedute dal 19 maggio 2022 al 24 giugno 2026. Il rapporto put-call 0DTE pesato per la gamma
  (cioe' i contratti vicini al prezzo) alle 9:35: l'ora dopo -3,5 punti base per deviazione standard (t -2,65,
  Holm 0,0163), una probabilita' di ora in calo di circa 5 punti in piu' su una base del 47% (logit t 3,22,
  Holm 0,0038), niente sul **quanto** (t 1,16). Solo nei primi 15 minuti (letture da 30 a 300 minuti: t fra
  -0,55 e +1,46). Il verso sparisce nel 2024-2026 (p 0,205), il "se" regge (p 0,037). Fuori campione (756
  giornate): Sharpe +0,34 lordo [-0,60, +1,31], +0,05 a un punto base; AUC 0,531 contro 0,520 dei soli
  controlli (DeLong p 0,233), e questa seconda prova e' stata decisa dopo il risultato del trading. **Il PCR
  pesato per il delta (t +0,07), il flusso firmato in dollari (t -0,11) e il punteggio firmato pesato per la
  gamma (t -0,23) non predicono.**
- **CKMS** [§2, §3.1-3.3, §4, §5.1-5.2, Tab. 2, 4, 6, 7, 9, IA Tab. A1-A2, Fig. 2-3]: opzioni su indici
  dell'ISE, **Russell 2000 e Nasdaq-100 sommati**, solo Nasdaq-100 dal 22 aprile 2013 (nota 11), 615
  settimane 2006-2017. Le put comprate in apertura predicono l'S&P 500 **al contrario**: +0,22% la settimana
  dopo per deviazione standard; +0,55% netto in 10 settimane, partendo da circa -0,22 nella settimana dello
  shock (Fig. 3). Solo con il VIX nel quartile alto (somma dei ritardi 0,2870 contro 0,0235, p 0,41), solo gli
  acquisti (Tab. 9), solo i clienti fino a 100 contratti (IRF 0,56, errore 0,25; grandi e firm insieme 0,13,
  errore 0,25). Le opzioni **SPX** non predicono (0,02, errore 0,30), ne' i flussi di E-mini, SPY e titoli
  NYSE la settimana dopo (Tab. 6). Market maker non informati (§5.1). Fuori campione R2OS 0,41-2,20%,
  significativo solo al 10% a una coda per gli inizi 2013 e 2014. Gli autori: **assicurazione**, non
  informazione, e non va usato per entrare o uscire.
- **HHW** [§3.1, §4.1, Tab. 3]: opzioni di QQQQ (oggi QQQ) nel 2006, ogni scambio OPRA segnato con Lee e
  Ready, **nessun gruppo**, a un minuto, 72 modi di sommare. Nello stesso minuto tutte le 36 pressioni sul
  prezzo vanno con il rendimento (fino a 0,41, *delta per share* con il logaritmo della taglia); sul minuto
  dopo fra -0,0035 e 0,0110, con le stelle che coincidono con 85.470 minuti trattati come indipendenti. Il DEX
  di X-Ray (delta per volume) nello stesso minuto vale 0,1052, il piu' basso fra le *delta per share*; sul
  minuto dopo e' fra i migliori (0,0103), ma a circa un centesimo. Nessuna normalizzazione.
- **KVS** [§3.1, §4.2, §4.3, §5, eq. 2-4, Tab. 1-4, 7, Fig. 1, A2]: opzioni SPX mensili da 7 a 180 giorni,
  **tutto il 2014 e solo il 2014**, 1.804.397 scambi segnati con le quotazioni, **nessun gruppo, niente 0DTE**,
  9:45-15:45 di New York, 1.494 ore. Il flusso in delta (contratti per delta per segno per prezzo, la misura
  del DEX in dollari) e' in media **+179 milioni di dollari l'ora** e il 66,2% degli scambi e' iniziato da un
  compratore. Uno shock di una deviazione standard muove l'S&P 500 di 98,13 cents in quattro ore (t 4,41, 5,1
  punti base), ma **tutto nella stessa ora**: il conto rifatto in verifica da' 105,7 cents nell'ora, di cui circa
  73 passano dalla volatilita' che scende. I ritardi del flusso in delta non predicono il prezzo (t 0,6 e 0,4;
  la forma ridotta, nostro conto, da' circa zero), mentre il **prezzo dell'ora prima predice il flusso** (t 5,7
  e 2,2). Per dollaro SPY pesa 3,4 volte le opzioni. Il delta delle opzioni a scadenza breve e lontane dal
  denaro pesa di piu' (Wald). Alla mezz'ora piu' debole, **al minuto niente** (tick e spread delle opzioni):
  l'ora e' "il miglior compromesso". **Correzione della verifica**: l'ordinamento del VAR da' al delta, non a
  SPY, la parte comune; con SPY messo prima (Fig. A2) l'effetto si legge intorno a 70-75 cents.
  Significativita' per ora, non per giornata; nessun nullo, nessun campione fuori.
- **NSW** [§2, §3.1-3.5, §4, Tab. 1-3, 5, 9, Fig. 1]: Cboe Open-Close di tutte le SPX 1991-2018, due sole
  classi (clienti pubblici e firm; niente professionali, niente retail: per gli autori i clienti delle SPX sono
  in maggioranza **istituzioni**), **mensile**, 336 mesi, Newey-West a 3 ritardi. Il DOI (call ITM comprate meno
  vendute, meno put ITM comprate meno vendute, in apertura) predice il mercato **al contrario**: mese dopo
  -2,02 (t -3,03, R quadro 3,06%), tre mesi -5,74 (t -3,85), in entrambe le meta' del campione; con i controlli t
  fra -2,27 e -2,81. Le put **OTM** vanno nel verso **opposto** (piu' put OTM comprate, mercato piu' basso: t
  -3,11), come Chen, Joslin e Ni. Le ITM in scadenza nel mese non predicono (t 1,21 e -0,30). Nello **stesso
  mese** il pubblico compra piu' call ITM del solito quando il mercato scende (t -2,45, Tab. 9), e il rendimento
  del mese non e' fra i controlli. Testo e Tab. 5 non coincidono, il fuori campione non e' descritto (R quadro
  fuori spesso piu' alti che dentro), le scadenze sono scelte sulla stessa tabella che poi si usa come prova.
- **AGPV** [§3-§7, Fig. 1-3, Tab. 1-3, 5, 6, 9, IA1]: OPRA via SpiderRock, 19 maggio 2022 - 31 gennaio 2026,
  tutte le SPX, **nessun tipo di cliente**, il verso stimato dal punto medio. Negli 0DTE, a ogni mezz'ora esatta
  una **raffica**: parte entro pochi secondi, finisce in 30-60 secondi, circa il doppio degli scambi dei minuti
  vicini, piu' del triplo quelli complessi; il secondo di picco vale 4-6 volte il minuto prima negli ultimi
  anni. **Crescono ogni anno e si allargano**: i quarti d'ora da 1,05 volte i minuti vicini (2022) a 1,3-1,5 (dal
  2023), i multipli di cinque minuti da 1,02 a 1,12 (2026), le 13:45 a circa il doppio nel 2026. Sono spread
  verticali venduti (put dal 3,6% al 6,7% degli scambi, call dal 3,1% al 5,5%) e iron condor, piccoli, da "menu"
  delle piattaforme; la quota delle put vendute scende dall'8,10% al 4,69% dal quintile di volatilita' piu'
  basso al piu' alto (t -16,47). **Per orario** le put vendute salgono piu' delle call quasi ovunque (13:30 +7,8
  contro +3,6; **14:00 +5,3 contro +4,3**, correzione della verifica); le call superano le put solo alle 10:30 e
  alle 15:00 (Tab. IA1). Il retail e' riconosciuto **dal comportamento**; la quota "fra meta' e tre quinti"
  degli 0DTE e' una stima Cboe citata, non misurata. **Nessuna regressione sul rendimento dell'indice** in tutto
  il paper.

### Dove Convergono

1. **Sul verso di un indice nessun gruppo e' informato.** Sulle opzioni di SPX: niente in PP (Tab. 9), in
   CLZZ al giorno e alla settimana (Tab. 7A, A11) e in CKMS a settimane (Tab. 7); dentro la giornata il delta e
   il flusso con il segno non anticipano l'ora dopo (MAMU Tab. 7, 23; KVS Tab. 4, su tutti gli scambi) e il
   minuto dopo vale circa 0,01 (HHW, su QQQ). ADEFOV non lo prova e il flusso sommato non predice nemmeno la
   volatilita'; MCH non tocca gli indici e a 10 minuti non trova niente; BBG e AGPV non lo provano.
2. **Quando un flusso per gruppo, o con il segno, predice su un indice, predice al contrario e lento**: PP su
   OEX e NDX al giorno dopo, CKMS su Nasdaq-100 e Russell 2000 a settimane, NSW sulle SPX in the money a uno-tre
   mesi, CLZZ su SPX a due mesi. L'unica previsione nel verso e' MAMU, senza segno e senza gruppi, e le put OTM di
   NSW (vedi le contraddizioni).
3. **I conti proprietari (firm) non dicono niente**: PP sui titoli e sugli indici, MCH sui titoli, CKMS
   insieme ai clienti grandi (0,13, errore 0,25); CLZZ e NSW non li usano come predittore.
4. **Il flusso si legge contro la sua storia, non da solo**, perche' il segno tipico non e' zero: i clienti
   sono di solito venditori netti (CLZZ Tab. 1A; CKMS: put -1,48%, call -2,78%, Tab. 1A; NSW: il pubblico vende
   call ITM e compra put ITM, cioe' e' corto di delta, Tab. 1), mentre sugli 0DTE gli utenti finali sono
   compratori netti e i market maker corti (BBG Fig. 2), e il flusso in delta di tutti gli scambi SPX del 2014 e'
   in media +179 milioni l'ora (KVS Tab. 2). Le misure: sbilancio fra -1 e +1 (CLZZ eq. 1, MCH eq. 1, CKMS §2,
   ADEFOV §4.3), normalizzato per il volume (NSW), standardizzato o in deviazioni standard (CLZZ, ADEFOV Tab. 3,
   MAMU), con le soglie su una finestra all'indietro (ADEFOV Tab. 7; MAMU nel fuori campione, §5.9.1).
5. **"Clienti" non e' "retail"**: per ADEFOV (§3.4) comprende gli istituzionali; per NSW i clienti pubblici
   delle SPX sono in maggioranza istituzioni; per CLZZ (nota 8) e MCH (App. A) e' chi manda meno di 390 ordini al
   giorno; CKMS separa i clienti per **taglia** (fino a 100 contratti), non per identita'; BBG riconosce il
   retail dal codice d'asta (4-6% degli 0DTE), AGPV dal comportamento a orario. Nessuno degli undici ha il retail
   puro nel gruppo "clienti".
6. **Il flusso va con il prezzo nello stesso tempo**, e questo non e' una previsione: HHW nello stesso minuto
   (fino a 0,41) contro circa 0,01 dopo; KVS nella stessa ora (e il prezzo dell'ora prima che **predice** il
   flusso, t 5,7); CKMS nella stessa settimana (residui delle put con il rendimento -0,09 e con il VIX +0,11,
   Tab. 3C; la risposta parte negativa, Fig. 3); NSW nello stesso mese (Tab. 9, t -2,45); MAMU (il rendimento dei
   primi cinque minuti spiega il 14,6% del rapporto, Tab. 16). La nostra misura lo ritrova per gruppo (82% sulla
   mattina dei clienti non professionali).
7. **Lo stock pesa piu' del flusso del giorno**: la gamma che frena e' ereditata (ADEFOV §3.6), e i market
   maker compensano il flusso del giorno (§3.8); in MAMU lo stock di open interest non predice il verso (Tab.
   19), e la concentrazione della gamma vicino al prezzo va con un'ora meno ampia (Tab. 25; indizio, e il paper
   da' due serie di p diverse, Tab. A.11).
8. **Cosa si somma si separa**: PP (l'open-buy porta l'informazione, Tab. 8), CKMS (solo le put, solo gli
   acquisti, solo le aperture: le chiusure "non predicono", nota 9), MAMU (put contro call vicino al prezzo), NSW
   (ITM e OTM con segni opposti, Tab. 2), KVS (il delta delle opzioni brevi e lontane dal denaro pesa di piu': il
   test di Wald rifiuta la somma, §4.3). Un netto che somma call e put, apertura e chiusura, moneyness e
   scadenze mescola cose che i paper tengono separate.
9. **Dentro la giornata la forma non e' solo la U** (AGPV): sugli 0DTE di SPX ci sono raffiche agli orari tondi
   di New York, e sono sempre di piu'. Ogni confronto con lo storico dentro la giornata deve tenerne conto.

### Dove Si Contraddicono

- **Il verso dei clienti.** Nel verso sui titoli, uno contro l'altro (PP Tab. 6, MCH Fig. 6); al contrario
  sul mercato sommato (CLZZ) e sugli indici dove c'e' qualcosa (PP Tab. 9, CKMS, NSW). CLZZ lo spiega: nella
  sezione trasversale resta l'informazione sul titolo, nella somma si compensa e resta il sentiment. **Un
  finding sui titoli non vale per un indice.**
- **Il verso delle put.** In MAMU piu' put (volume senza segno, vicino al prezzo) vanno con un'**ora dopo
  piu' debole**: nel verso, dentro la giornata, 2022-2026, e il verso sparisce nel 2024-2026. In NSW le put
  **OTM** comprate vanno con un mercato **piu' basso** dopo (nel verso, mensile), le put **ITM** comprate con un
  mercato **piu' alto** (al contrario). In CKMS piu' put **comprate** dai piccoli (circa due terzi al denaro)
  vanno con **settimane dopo piu' alte**: al contrario, 2006-2017, col VIX alto. In PP, su NDX e OEX, al
  contrario il giorno dopo. Orizzonti, misure (con e senza segno), moneyness e anni diversi: **il verso delle
  put cambia con la moneyness e con l'orizzonte**. Una lettura possibile, **nostra e non provata**: a brevissimo
  le put vanno con il calo (MAMU, e la settimana stessa dello shock in CKMS, Fig. 3), a settimane si paga il
  premio di chi si e' assicurato.
- **"Informato".** KVS chiama informato il flusso che muove il prezzo **nella stessa ora** in modo permanente
  (Hasbrouck), e lo attribuisce al flusso per ipotesi (l'ordinamento del VAR mette il prezzo per ultimo); gli
  altri chiamano informato il flusso che **anticipa**. Con la seconda definizione KVS non trova niente (t 0,6 e
  0,4). E le sue stesse tabelle dicono che il prezzo dell'ora prima muove il flusso (t 5,7): la stessa
  correlazione si puo' leggere al rovescio, come HHW e la nostra misura.
- **Il flusso insegue il prezzo o gli va contro?** In KVS il flusso di **tutti** gli scambi SPX segue il prezzo
  (dopo un'ora di rialzo piu' delta comprato). In NSW i **clienti pubblici** comprano piu' call ITM nei mesi in
  cui il mercato **scende** (contro), mentre gli autori leggono la loro Fig. 2 come "inseguire la tendenza"
  (trend-chasing): la loro tabella li smentisce. Nella nostra misura, dentro la seduta, i clienti non
  professionali vanno contro il prezzo e i professionali con il prezzo. Gruppi e scale diverse: **il verso
  dipende dal gruppo**, e il flusso di tutti insieme nasconde gruppi che si muovono in versi opposti.
- **Quanto e' il retail.** Sugli 0DTE di SPX: oltre il 6% del volume per BBG (dall'asta), "fra meta' e tre
  quinti" per la stima Cboe che cita AGPV. Dieci volte di differenza, per due definizioni diverse; nessuna
  delle due e' una misura del gruppo `nonprocust` di OptionsDepth.
- **I piccoli.** Per HHW gli ordini spezzati in piccoli pezzi sono il segno di **chi sa** e si nasconde (il
  logaritmo della taglia vince nello stesso minuto, §2.2); per CKMS e CLZZ i piccoli sono **chi ha paura** o
  segue il sentiment, e si leggono al contrario; per BBG il retail piccolo **perde**; per AGPV i piccoli
  pacchetti a orario sono **bot che seguono un menu**, senza informazione per costruzione. La stessa taglia,
  quattro letture.
- **I market maker.** In MCH sono il segnale piu' forte, al contrario (titoli, notte, 2015-2019, Nasdaq);
  in CLZZ non predicono alla settimana, salvo al giorno e al denaro, di segno positivo (+0,083, t 2,60,
  Tab. A8A); in ADEFOV il loro delta non predice la volatilita'; in CKMS non sono informati (§5.1). Dati,
  strumenti e periodi diversi.
- **Acquisti o vendite.** PP: l'open-buy e' il piu' informativo (Tab. 8). CKMS: contano solo gli acquisti di
  put (0,1342, p 0,0064; le vendite 0,0639, p 0,14, Tab. 9). CLZZ: "e' il comprare", ma a settimana nessuna delle tre misure e' significativa e al
  giorno anche la vendita fuori dal denaro ha t 2,00. MCH: per i clienti l'informazione sta **nelle vendite**
  (Fig. 17).
- **Il broker.** In MCH (Nasdaq "Broker") predice al contrario; in PP non esiste; in CLZZ e' dentro i market
  maker; ADEFOV non lo prova.
- **Stesso mercato, risultato diverso**: in PP l'OEX (S&P 100) ha due classi contrarian significative, lo SPX
  (S&P 500) nessuna, negli stessi anni. In CKMS le opzioni su indici dell'ISE predicono e le SPX no: per gli
  autori la differenza e' **chi scambia** (scambi un sesto delle SPX, 90% di clienti), non l'indice. In NSW
  invece le SPX predicono, ma solo una fetta (ITM, scadenze lunghe) e a mesi.
- **La significativita' non si confronta**: per giornata con Newey-West in MAMU, per settimana in CKMS, per
  mese in NSW, per ora in KVS (1.494 ore, non raggruppate per giorno), per minuto come se i minuti fossero
  indipendenti in HHW, per operazione con gli effetti fissi del giorno in BBG, nessun test sui picchi in AGPV
  (solo figure).
- **I periodi non si sovrappongono come servirebbe**: PP 1990-2001, NSW 1991-2018, HHW 2006, CKMS 2006-2017,
  CLZZ 2005-2020, KVS 2014, MCH 2015-2019, ADEFOV 2018-2024, BBG 2021-2023, MAMU e AGPV 2022-2026. Vedono gli
  0DTE quotidiani (da maggio 2022) ADEFOV, BBG, MAMU e AGPV; nessuno dei quattro prova un gruppo contro il
  prezzo.

### Il Perche' Comune

Sei ragioni, dette dagli autori. (a) L'informazione privata riguarda i **singoli titoli**, non il mercato, e
gli indici si usano per **coprirsi** (PP §4.7, §5; CLZZ §1, §5.2: su SPX e NDX scambiano soprattutto gli
istituzionali; CKMS §5.2: le SPX sono istituzionali, scambi sei volte piu' grandi). (b) Sugli indici, cosi'
visibili, gli arbitraggisti correggono subito gli errori di prezzo del rumore (CLZZ nota 2); e un indice si
scambia a costo quasi nullo con ES e SPY, quindi chi sa qualcosa non e' spinto sulle opzioni (MAMU §5.9.4; per
dollaro, nella stessa ora, SPY pesa 3,4 volte le opzioni in KVS). (c) Quello che resta e' **sentiment**, che si
legge al contrario e lentamente: CLZZ; NSW (scommettere sul verso con le SPX costa piu' del future, quindi chi lo
fa ha una parte irrazionale, e i limiti all'arbitraggio fanno durare l'errore mesi, piu' nelle recessioni); PP
lo collega all'uso al contrario del put-call ratio a Wall Street. (d) Oppure e' **assicurazione**: quando
l'incertezza sale chi ha paura compra protezione, e il premio chiesto da chi tiene il rischio sale con lei; per
questo piu' put, poi rendimenti piu' alti, e solo con il VIX alto (CKMS §5.2, con Grossman e Zhou, Bates). (e) I
market maker compensano il flusso del giorno, e conta lo stock di posizioni (ADEFOV). (f) Una parte del flusso
**segue l'orologio, non l'informazione**: bot retail che ereditano gli stessi orari e gli stessi parametri dalle
stesse piattaforme, "sincronizzazione senza coordinamento" (AGPV §2.2-2.4). Una settima e' **nostra**, ed e'
l'alternativa che HHW e KVS non discutono: nello stesso minuto o nella stessa ora puo' essere il prezzo a
muovere il flusso (scambi segnati su quote non ancora aggiornate, coperture che inseguono), e non il contrario;
KVS la esclude per ipotesi, e le sue tabelle la sostengono a un'ora.

### Cosa Si Rifa' Su NQ Con ATAS, E Cosa No

- **Il flusso per gruppo, su NQ, no.** La catena CME di ATAS non dice chi ha scambiato; il Cboe Open-Close
  di NDX e l'ISE Open/Close non li abbiamo. Il DEX per gruppo di X-Ray e' di SPX, e su NQ resta **contesto**
  (CLAUDE.md, decisione 2): mai in un backtest.
- **Con le sole barre di ATAS, su NQ**: l'inversione del flusso aggressivo dopo un movimento (ADEFOV §4.3,
  eq. 11 senza la gamma; su ES -5,824, t -18,93), con il delta di ATAS a 10 minuti. Il **controllo negativo
  di CKMS** (Tab. 6): lo sbilancio aggressivo settimanale del future muove la settimana stessa e non la dopo.
  Il **VAR di Hasbrouck di KVS** sul future (prezzo di NQ e delta delle barre a un'ora, ogni giornata che
  riparte da zero), **con il prezzo messo prima e dopo il flusso**, la prova che KVS non fa. L'analisi di CKMS
  contava circa 100 settimane dal solo tape (dal 2023); con il delta delle barre M1 di NQ, che c'e' da aprile
  2017 ([i-dati-da-atas](../../metodo/i-dati-da-atas.md)), dentro il campione sono circa 400 settimane (nostro
  conto). E' order flow, non flusso per gruppo; ogni sguardo al 2025-2026 consuma il fuori campione di NQ.
- **Registrando in avanti**: il rapporto put-call 0DTE pesato per la gamma alle 9:35 dalla catena CME di NQ
  (MAMU), se la catena da' il volume per strike al minuto (**da verificare**): circa 400 sedute per il "se",
  circa 585 per il verso (nostro calcolo nell'analisi), senza garanzia che valga per le opzioni CME. Se le opzioni
  CME su NQ abbiano raffiche a orario come le SPX (AGPV) non lo dice nessuno: da guardare, in avanti. Su SPX la
  domanda dei paper (un gruppo anticipa il prezzo?) richiede **anni**: circa 800 sedute per l'effetto di PP su
  NDX, circa 1.860 per quello di CLZZ al giorno, oltre tre anni di settimane per CKMS, circa 150 mesi per NSW.
  L'archivio di X-Ray ha oggi 35 sedute utili.
- **Descrittivo, sull'archivio di X-Ray (SPX)**: il segno medio del DEX 0DTE dei clienti non professionali
  (BBG: compratori netti di 0DTE?); lo **scalino del DEX netto e del lordo nel minuto delle :00 e delle :30**
  contro i minuti 25-29 della stessa mezz'ora, per gruppo, una stima per seduta e il nullo per permutazione
  degli orari (AGPV Tab. 6): dice **in quale gruppo cadono le raffiche** e se lo sbilancio va verso zero
  all'orario; la regressione oraria del rendimento di ES sul DEX sommato, nella stessa ora e un'ora dopo (KVS),
  e la correlazione al minuto (HHW), una stima per giornata e il nullo per permutazione delle giornate. Con 35
  sedute KVS fa attendere una t intorno a 1,7, e per la sola stessa ora (nostro calcolo nell'analisi). Su NQ,
  X-Ray cade nel fuori campione: solo contesto.
- **Dati che non abbiamo**: lo storico del Cboe Open-Close e dell'ISE Open/Close (a pagamento), i codici
  d'asta OPRA (BBG), gli scambi uno per uno con la taglia (HHW, AGPV), la divisione apertura/chiusura, call/put,
  moneyness e taglia nel dato di OptionsDepth (**da verificare** se le dia), il vega scambiato per gruppo (KVS,
  **da verificare**), il flusso per gruppo sulle opzioni di NDX e QQQ.

### Le Idee Candidate

Nessuna e' una strategia; la scheda non e' stata chiesta e non si scrive.

| | finding da provare | cosa la ucciderebbe |
|---|---|---|
| A | Il **residuo** del flusso di un gruppo (tolta la parte spiegata dal prezzo) anticipa il rendimento di SPX/ES dei 30-120 minuti dopo. Nessun paper lo prova; MCH a 10 minuti, MAMU dopo i primi 15 minuti e KVS all'ora dopo non trovano niente | Le 8 prove della nostra misura sono gia' nulle (p fra 0,06 e 0,82); con alcune centinaia di sedute, una correlazione non diversa dal nullo per permutazione delle giornate |
| B | Dopo un movimento di 10 minuti il flusso aggressivo di NQ va contro il movimento (ADEFOV §4.3, misurato su ES) | Su NQ 2017-2024, per giornata, un coefficiente non diverso dal nullo |
| C | Lo sbilancio dei clienti sulle opzioni di un indice anticipa l'indice al contrario a settimane o mesi (PP Tab. 9; CLZZ Tab. A11; CKMS, solo i piccoli sulle put, col VIX alto; NSW, solo le SPX ITM, a uno-tre mesi) | Gia' oggi CLZZ e CKMS: niente su SPX a giorni e settimane; NSW non controlla il rendimento del mese in cui il flusso si forma (Tab. 9 lo mostra legato). Non rifacibile: servono anni di dati per gruppo che non abbiamo |
| D | L'apertura che pende sulle put 0DTE vicino al prezzo (PCR pesato per la gamma, primi 5-15 minuti) alza la probabilita' di un'ora in calo (MAMU) | Gia' oggi MAMU: fuori campione AUC non migliore dei soli controlli (DeLong p 0,233) e verso assente nel 2024-2026. Su NQ, registrando in avanti dalla catena CME, un "se" non diverso dal nullo per permutazione delle giornate dopo circa 400 sedute |
| E | Controllo negativo con le sole barre: lo sbilancio aggressivo settimanale di NQ muove la settimana stessa e non la dopo (CKMS Tab. 6, su ES) | Un coefficiente sulla settimana dopo diverso dal nullo per permutazione delle settimane: allora il controllo di CKMS non si trasferisce a NQ, e un "flusso predittivo" va provato contro di esso |
| K | Sul future NQ, a un'ora, l'impatto del flusso aggressivo e' tutto nella stessa ora e il prezzo dell'ora prima predice il flusso (KVS, rifatto con l'ordinamento rovesciato accanto) | Un ritardo del flusso che predice il prezzo dell'ora dopo, per giornata, oltre il nullo: allora la lettura "stessa ora, non previsione" non vale per NQ |
| L | **Descrittiva, non una strategia**: le raffiche a orario di AGPV si vedono nel DEX di X-Ray come scalini a :00 e :30, in un gruppo preciso, con lo sbilancio che va verso zero | Nessuno scalino diverso dal nullo per permutazione degli orari: allora le raffiche non toccano il riquadro, e il punto 6 del riquadro si ridimensiona |

**Quale studio aprire.** Sul flusso per gruppo, **nessuno**: le prove degli undici paper e la nostra misura sono
negative, e il lavoro utile e' il riquadro come descrizione (sotto) e l'archivio che cresce. La misura piu'
utile subito e' la **L**, sull'archivio di X-Ray, perche' dice se il riquadro va letto diversamente agli orari
tondi; non e' uno studio e non entra nel registro. Rifacibili con i soli dati di ATAS sono la **B**, la **E** e
la **K**, che pero' sono order flow del future, non questa domanda; la **D** ha gia' un fuori campione negativo
nel paper. Non si apre niente senza la richiesta dell'utente.

## La Nostra Misura Sull'Archivio Di X-Ray

**Non e' un paper.** E' una misura **nostra, descrittiva**, fatta il 9 novembre 2026 nello studio dei paper
(i numeri sono quelli passati alla fase 5; il calcolo non e' stato rifatto in questa fase). Serve a vedere
se quello che i paper dicono vale anche sul dato del riquadro. Non e' un backtest, non e' su NQ, e non
entra nel registro.

**In parole semplici.** Il "retail" di OptionsDepth, dentro la giornata, compra delta quando SPX scende e lo
vende quando sale, quasi tutti i giorni; i clienti professionali fanno il contrario. Il flusso della
mattina sembra anticipare il pomeriggio, ma e' un'illusione: anche il prezzo della mattina lo anticipa un
po', e il flusso ripete il prezzo. Tolto il prezzo, il flusso non anticipa niente.

**Le convenzioni.**

- **Campione**: 35 sedute di cassa di SPX, dal 21 settembre al 6 novembre 2026.
- **Il dato**: il DEX per gruppo di OptionsDepth (delta scambiato, comprato meno venduto), sommato dalle
  **9:31 di New York**, letto da `/api/deriva` del server di X-Ray; quattro gruppi (`nonprocust`, cioe' i
  clienti non professionali che X-Ray chiama retail, `procust`, `firm`, `broker`), separati in 0DTE e
  tutte le scadenze. Il **prezzo** e' il riferimento della stessa API, lo spot di SPX in cassa.
- **Dentro la seduta**: ogni 10 minuti, correlazione fra la variazione del prezzo dalle 9:31 e il flusso
  accumulato; una correlazione per seduta, poi la mediana sulle sedute.
- **Mattina contro pomeriggio**: il flusso della mattina (9:31-12:00) contro il rendimento del pomeriggio
  (12:00-15:59), una coppia per seduta.
- **Il nullo**: permutazione delle giornate (si rimescola quale pomeriggio va con quale mattina), 5000 giri.
- **Quante prove**: 8 (4 gruppi per 0DTE e tutte). Bonferroni: 0,05 / 8 = 0,006.

**Dentro la seduta.**

| gruppo | 0DTE: correlazione mediana, sedute con lo stesso segno | tutte: correlazione mediana, sedute |
|---|---|---|
| clienti non professionali (`nonprocust`) | **-0,85**, negativa in 33 su 35 | **-0,86**, 34 su 35 |
| clienti professionali (`procust`) | **+0,87**, positiva in 32 su 35 | **+0,86**, 32 su 35 |
| firm | -0,04, nessun verso fisso | -0,07, nessun verso fisso |
| broker | -0,10, nessun verso fisso | -0,15, nessun verso fisso |

Fra le sedute, il movimento del prezzo della mattina **spiega l'82%** del flusso 0DTE della mattina dei
clienti non professionali (il 64% su tutte le scadenze).

Avvertenza nostra: due serie accumulate, confrontate dentro una sola seduta, hanno correlazioni gonfiate in
valore assoluto anche se sono indipendenti (la regressione spuria fra passeggiate casuali). Per questo il
numero da guardare non e' -0,85 ma **il segno ripetuto**: se il segno fosse a caso, 33 sedute su 35 dello
stesso segno capiterebbero meno di una volta su un milione (nostro conto binomiale, sedute indipendenti).
E l'82% fra sedute usa un punto per seduta e non soffre di quel problema.

**Mattina contro pomeriggio.**

| prova | a prima vista: r (p) | tolta la parte spiegata dal prezzo della mattina: r (p) |
|---|---|---|
| clienti non professionali, 0DTE | -0,41 (0,02) | -0,12 (0,51) |
| clienti professionali, 0DTE | +0,27 (0,11) | +0,04 (0,82) |
| firm, 0DTE | -0,44 (0,03) | -0,26 (0,12) |
| broker, 0DTE | -0,37 (0,04) | -0,14 (0,42) |
| clienti non professionali, tutte | -0,37 (0,03) | -0,08 (0,63) |
| clienti professionali, tutte | +0,31 (0,07) | +0,05 (0,76) |
| firm, tutte | +0,51 (0,01) | +0,33 (0,06) |
| broker, tutte | +0,47 (0,04) | +0,26 (0,13) |

- **A prima vista** sei prove su otto hanno p sotto 0,05, ma **nessuna regge Bonferroni** (0,006).
- **Il prezzo stesso** della mattina e il pomeriggio hanno r **+0,39** in queste 35 sedute. Il flusso dei
  clienti non professionali e' quasi il prezzo rovesciato: il suo -0,41 e' quel +0,39 visto allo specchio.
- **Tolta la parte del flusso spiegata dal prezzo della mattina** (regressione lineare del flusso sul
  movimento della mattina, poi il residuo), **nessun gruppo predice il pomeriggio**. Il piu' vicino e' firm
  su tutte le scadenze, +0,33 con p 0,06: sopra 0,05 anche senza correzione.
- **Lo stesso gruppo cambia segno fra 0DTE e tutte** a prima vista (firm -0,44 e +0,51; broker -0,37 e
  +0,47): non e' un effetto stabile.

**I limiti.** 35 sedute; un solo regime di mercato (autunno 2026); il DEX e' una **stima di un fornitore**
(OptionsDepth), con unita' non dichiarate; i minuti anomali non sono stati tolti; nessuna correzione per
l'autocorrelazione dentro la seduta; la regressione che toglie il prezzo e' stimata sulle stesse 35 sedute
che giudica. Non sappiamo se la correlazione negativa dei clienti non professionali sia **comportamento**
(vendere sui rialzi, comprare sui ribassi, prendere profitto) o un **effetto della misura** (il delta delle
opzioni scambiate cambia con il prezzo): ipotesi nostre, non provate.

**Accanto ai paper.** La misura **converge** con i paper sul punto centrale: niente predice il prezzo
dell'indice (PP su SPX, CLZZ su SPX e NDX, MCH a 10 minuti). Converge anche con una lettura nostra di
ADEFOV (la Net Delta dei market maker ha media positiva, quindi i gruppi insieme scambiano delta contro il
movimento): qui va contro il prezzo **un** gruppo, i clienti non professionali, mentre i professionali vanno
con il prezzo. **Mostra un rischio che i paper non vedono**, perche' lavorano al giorno: dentro la seduta un
flusso puo' sembrare "contrarian" solo perche' ripete il prezzo. Firm senza verso fisso, come in PP e MCH.
**Con le altre quattro analisi** (aggiornamento del 9 novembre 2026) quel rischio non e' piu' solo nostro: HHW
trova il flusso delle opzioni di QQQ legato al prezzo nello stesso minuto (fino a 0,41) e quasi niente nel
minuto dopo (circa 0,01); MAMU, sugli 0DTE di SPX 2022-2026, trova che il rendimento dei primi cinque minuti
spiega il 14,6% del suo rapporto e che il delta e il flusso con il segno non anticipano l'ora dopo; CKMS trova
le put comprate nella stessa settimana in cui il mercato scende e il VIX sale (residui -0,09 e +0,11). Sono
misure diverse dalla nostra (tutto il flusso o il volume senza segno, non il DEX per gruppo), ma vanno nello
stesso verso: il flusso descrive il movimento, non lo anticipa. **Con il supplemento** si aggiungono due
fonti: KVS, con la stessa somma del DEX su tutti gli scambi SPX del 2014, trova l'impatto nella stessa ora e il
prezzo dell'ora prima che predice il flusso (t 5,7); NSW trova i clienti pubblici delle SPX che comprano call
ITM nei mesi in cui il mercato scende (Tab. 9), cioe' la stessa cosa della nostra misura su un'altra scala:
contro il prezzo, nello stesso periodo. Nessuno dei due separa i gruppi di OptionsDepth.

## Per Il Riquadro "Chi Ha Comprato Delta Oggi"

**Solo descrizione, nessun segnale.** Cosa sostengono i paper e la misura, e cosa no. Come si disegna lo
decide chi lavora sulla pagina X-Ray; qui ci sono le ragioni.

**1. Il confronto con lo storico alla stessa ora (percentile).**

- *Lo sostengono*: CLZZ (lo sbilancio si standardizza sulla sua storia, e i clienti sono di solito venditori
  netti: il segno da solo non dice niente, Tab. 1A); ADEFOV (le grandezze in deviazioni standard dentro la
  giornata, Tab. 3; le soglie su una finestra mobile **all'indietro**, che non guarda avanti, Tab. 7); PP
  (il rapporto controlla le differenze di volume nel tempo, nota 10). Alla **stessa ora** perche' il volume
  di opzioni ha la forma a U nella giornata (MCH App. E, Fig. 31) e il flusso si accumula dalle 9:31.
- *Nostra scelta, non dei paper*: il **percentile** invece dello z-score, perche' con 35 sedute e una
  distribuzione sconosciuta un rango ("piu' grande di 31 sedute su 35") non presuppone una forma.
- *Come si dichiara*: solo sedute **passate**; il numero di sedute accanto al percentile (con 35 sedute il
  percentile si muove a scalini di circa 3 punti); 0DTE e tutte separate.

**2. La parte del flusso che il prezzo non spiega.**

- *La sostiene la nostra misura*: per i clienti non professionali il prezzo della mattina spiega l'82% del
  flusso 0DTE (64% su tutte), e dentro la seduta il segno e' opposto al prezzo in 33 sedute su 35; per i
  professionali il segno e' lo stesso del prezzo in 32 su 35. Il flusso grezzo di questi due gruppi
  **ripete il prezzo**: il riquadro che lo mostra da solo ridice il grafico. Per firm e broker il prezzo
  spiega poco, e il residuo e' quasi il flusso.
- *La logica viene da PP* (§4.3: il segnale si mette nella stessa regressione con quello pubblico, e conta
  la parte che resta), non la misura.
- *Come si dichiara*: il residuo e' "quanto il gruppo ha fatto di diverso dal solito, dato il movimento di
  oggi", con la regressione stimata solo sulle sedute passate alla stessa ora. E va scritto che il residuo
  **non ha anticipato il pomeriggio** in nessuna delle 8 prove (p fra 0,06 e 0,82).

**3. Lo sbilancio fra -1 e +1.**

- *Lo sostengono*: CLZZ (eq. 1, in contratti, solo aperture), MCH (eq. 1, rialzo meno ribasso diviso la
  somma), ADEFOV (§4.3, sugli ordini degli E-mini). Rende confrontabili giorni e gruppi di taglia diversa.
- *Il limite*: i paper contano **contratti**; il DEX pesa per il delta. Il DEX di OptionsDepth e' gia' un
  netto per minuto e per strike, quindi la versione possibile (proposta nell'analisi di MCH, nostra) e'
  DEX accumulato diviso la somma dei valori assoluti del DEX di ogni minuto e strike: **sottostima il
  lordo**, e va dichiarato. Anche lo sbilancio si legge contro la sua storia (punto 1), perche' il livello
  tipico dei clienti non e' zero (CLZZ Tab. 1A).

**4. Cosa non va scritto.**

- **Chi ha ragione e dove va il prezzo.** Nessun paper lo sostiene per nessun gruppo su SPX (PP Tab. 9;
  CLZZ Tab. 7A, A11; CKMS Tab. 7; ADEFOV, BBG e AGPV non lo provano; MCH a 10 minuti niente; MAMU, sugli 0DTE,
  delta e flusso con il segno nulli sull'ora dopo, Tab. 7, 23; KVS, ritardi del delta nulli, Tab. 4), e la nostra
  misura, tolto il prezzo, nemmeno. Nemmeno "informato" nel senso di KVS: e' impatto della stessa ora.
- **"Il retail e' contrarian"** dalle 35 sedute: il -0,41 e' il prezzo allo specchio. E nemmeno "retail"
  come etichetta piena: i clienti non professionali del Cboe comprendono gli istituzionali (ADEFOV §3.4;
  CLZZ nota 8).
- **Il flusso come posizione**: la gamma che frena il mercato viene da posizioni dei giorni prima (ADEFOV
  §3.6), e sta nella GEX dei market maker, non nel DEX di oggi.
- **Un segnale su NQ**: il DEX di SPX su NQ e' contesto (CLAUDE.md, decisione 2).
- Da dire, se il riquadro somma call e put e apertura e chiusura: che i paper le separano (le put dicono la
  volatilita', CLZZ Tab. A14; l'open-buy e' il piu' informativo, PP Tab. 8), e che se OptionsDepth separi
  apertura e chiusura e' da verificare.

**5. Il riquadro com'e' fatto, e cosa ne dicono BBG, MAMU, CKMS e HHW** (aggiornamento del 9 novembre
2026). Il riquadro e' gia' implementato (`Server/Flusso.cs`, `/api/flusso`; la lettura in
[x-ray-in-vivo](../../metodo/x-ray-in-vivo.md)): per gruppo, il **percentile alla stessa ora** fra le sedute
passate, la **sorpresa oltre il prezzo** (il flusso meno la retta sul movimento dall'apertura, fra le stesse
sedute), le **bande 10-90%** delle sedute passate, e "insolito" sotto il 20% o sopra l'80%, dichiarato come
**convenzione**. Qui nessun codice si tocca: si dice cosa confermano, cosa cambiano e cosa aggiungono i quattro
paper.

*Confermano.*

- **Il confronto con le sedute passate, e 0DTE separati da tutte**: il segno tipico non e' zero e cambia con
  la scadenza. Dopo maggio 2022 gli utenti finali sono compratori netti di 0DTE e venditori delle altre
  scadenze, con i market maker corti di 0DTE per oltre l'1% del volume (BBG Fig. 2); all'ISE i clienti sono
  venditori netti di put e call (CKMS Tab. 1A). Un acquisto netto di 0DTE "dei clienti" e' la normalita'.
- **Solo sedute passate**: CKMS calcola il quartile del VIX sull'intero campione nelle tabelle (lo deduce la
  verifica) e solo nel fuori campione "fino al giorno t"; MAMU usa la scala "per deviazione standard"
  sull'intero campione, ma non nel fuori campione. Il riquadro fa gia' la cosa giusta.
- **La sorpresa oltre il prezzo**: e' la parte che serve, perche' il flusso va con il prezzo nello stesso
  tempo (HHW, fino a 0,41 nello stesso minuto; CKMS nella stessa settimana; MAMU, il 14,6% del rapporto dai
  primi cinque minuti). MAMU fa la stessa operazione per il suo segnale (Tab. 16: tolto il movimento
  d'apertura il segnale resta, -3,16 punti base, t -2,61) e avverte che, con il movimento gia' fra i
  controlli, toglierlo prima non cambia il coefficiente (Frisch-Waugh-Lovell): la retta va stimata **solo
  sulle sedute passate**, come gia' fa il riquadro, altrimenti guarda avanti.
- **Mai chi ha ragione**: MAMU e' la prova piu' diretta sullo stesso tipo di dato (0DTE di SPX, 2022-2026):
  delta e flusso con il segno non anticipano l'ora dopo, e un flusso letto da 30 minuti a 5 ore dopo
  l'apertura non predice niente (Tab. 8); CKMS: le opzioni SPX non predicono (Tab. 7). Il dato del riquadro e'
  proprio quello che nei paper non anticipa l'indice.

*Cambiano.*

- **L'etichetta "Retail"**: il retail che si riesce a riconoscere sugli 0DTE di SPX e' il 4-6% del volume
  (BBG), e il gruppo che OptionsDepth chiama retail (i clienti non professionali del Cboe) e' piu' largo e
  comprende istituzioni (ADEFOV §3.4). La nota sotto il riquadro lo dice gia'; il nome della colonna no. Piu'
  fedele ai paper: "Clienti non prof." con la spiegazione, oppure "Retail" sempre accanto alla nota.
  Decisione di chi lavora sulla pagina.
- **La base "clienti informativi sulle put" non c'e' piu'**: la risposta breve la diceva da un riassunto di
  CKMS; letto, sono i piccoli, al contrario, ed e' assicurazione. Nessun gruppo del riquadro ha un fondamento
  per essere letto "nel verso".

*Aggiungono.*

- **Le soglie 20/80 coincidono con i quintili** che usano CKMS (Fig. 2) e la regola di MAMU (§4.4.1, il quinto
  di giornate piu' sbilanciate per parte). Non le provano come soglie: in CKMS l'effetto a una settimana sta
  quasi tutto nel quintile piu' alto (+0,79% contro circa -0,1% dei primi tre, dalla figura), in MAMU la regola
  sui quintili **non regge fuori campione** e la pendenza per decili e' fatta quasi tutta dal primo e dal
  decimo (Fig. 1, cioe' oltre le bande 10-90%). Le soglie restano una **convenzione**; i paper dicono solo che,
  quando c'e' qualcosa, sta nelle code.
- **Il VIX come seconda variabile** (proposta nostra, non provata e non da fare ora): in CKMS le put si
  comprano quando il VIX sale (residui +0,11) e l'effetto c'e' solo col VIX alto; la sorpresa potrebbe
  togliere anche la variazione del VIX. Con 35 sedute di un solo regime, aggiungere una variabile alla retta
  la rende piu' instabile: da riprendere quando l'archivio avra' piu' regimi.
- **Il DEX e' una variante dichiarata**: delta per volume, la piu' bassa fra le *delta per share* nello stesso
  minuto (0,1052 contro 0,4112 con il logaritmo della taglia, HHW Tab. 3A). La taglia per scambio il DEX per
  minuto non la da'; va detto, non si corregge.
- **Quello che il DEX netto mescola**: le strategie del retail (fino all'80% nel 2021, circa un terzo nel
  2023) sono vendute molto piu' spesso delle singole opzioni (BBG Tab. 3) e hanno delta piccolo; CKMS trova
  tutto nelle **put** comprate in **apertura**, MAMU nel rapporto **put/call**. Se OptionsDepth desse call e
  put, apertura e chiusura, o la taglia separate (**da verificare**), il riquadro sarebbe piu' vicino ai paper.
- **L'apertura**: quel poco che MAMU trova sta nei primi 15 minuti, e nei primi minuti l'indice e' in parte
  vecchio (dubbio nostro, limite 3 dell'analisi). Il riquadro somma dalle 9:30, e in Deriva la prima colonna
  della cassa, con la finestra piu' lunga di un minuto, e' gia' marcata come "finestra": e' coerente. Un rapporto put/call 0DTE pesato per la gamma dei primi 5-15 minuti,
  contro la sua storia, sarebbe una **descrizione** in piu', solo se OptionsDepth da' il volume per strike
  (**da verificare**), e senza valore fuori campione nel paper.
- **La mattina di BBG non giustifica la stessa ora**: il 61,7% delle operazioni retail 0DTE fra le 9:30 e le
  13:00 e' poco piu' della quota oraria (17,6% contro 12-13%), in operazioni e non in delta. La ragione della
  stessa ora resta l'accumulo dall'apertura e la forma a U del volume (MCH App. E).
- **La significativita' della nostra misura**: HHW mostra cosa succede trattando i minuti come indipendenti
  (stelle su correlazioni di 0,01); il riquadro e la nostra misura lavorano per seduta, ed e' giusto cosi'.

**In una riga**: il disegno regge, nessun paper lo smentisce; cambia il nome del gruppo, le soglie restano una
convenzione (coincidono con i quintili dei paper, che non le provano), e i dati che lo renderebbero piu' fedele
(call/put, apertura/chiusura, taglia) sono da verificare in OptionsDepth.

**6. Cosa dicono KVS, NSW e AGPV** (secondo aggiornamento del 9 novembre 2026). Il riquadro implementato
(`Server/Flusso.cs`, letto nell'analisi di AGPV, non toccato qui) somma il DEX per gruppo dal minuto k dopo le
9:30 di New York, la data convertita con `Cassa.Di`, e lo confronta con le sedute passate **allo stesso minuto
k**: percentile, sorpresa oltre il prezzo, sbilancio netto/lordo, variazione degli ultimi 30 minuti; **al
massimo 60 sedute passate, almeno 10**.

*Confermano.*

- **Il modo di sommare**: contratti per delta per segno, sommati su strike, scadenze, call e put, e' la misura
  di KVS (eq. 2), che aggiunge solo il prezzo per portarla in dollari. E il test di Wald di KVS (il delta delle
  opzioni brevi e lontane dal denaro pesa di piu' sul prezzo, §4.3) rende **coerente tenere 0DTE e tutte le
  scadenze separati**, come fa il riquadro.
- **Lo stesso minuto, in ora di New York, e' il confronto giusto per le raffiche** (AGPV §4, Fig. 1-3): le
  raffiche cadono agli stessi orari ogni giorno, quindi allo stesso k ogni seduta passata le ha gia' dentro e il
  confronto le neutralizza senza correzioni. Con un UTC fisso si sposterebbero di un'ora per meta' dell'anno: il
  riquadro usa gia' l'ora di New York. La variazione degli ultimi 30 minuti contiene sempre esattamente un orario
  di mezz'ora: e' confrontabile da un momento all'altro della giornata.
- **La sorpresa oltre il prezzo**: due fonti in piu' dicono che il flusso contiene il prezzo. In KVS il prezzo
  dell'ora prima predice il flusso in delta (t 5,7, Tab. 4); in NSW il flusso dei clienti pubblici va contro il
  mercato dello stesso mese (Tab. 9), e il paper, che non lo toglie, mostra il rischio che il riquadro evita:
  una "previsione al contrario" che puo' essere in parte il prezzo allo specchio.
- **Il segno tipico non e' zero**: +179 milioni di dollari di delta comprato l'ora, 66% di scambi iniziati da
  un compratore (KVS, 2014); il pubblico corto di delta sulle ITM (NSW Tab. 1). Il percentile contro le sedute
  passate resta la lettura giusta.
- **Mai chi ha ragione**: KVS, l'unico su SPX dentro la giornata con il delta, trova i ritardi nulli (t 0,6 e
  0,4; forma ridotta circa zero); NSW predice solo a mesi e solo con le ITM a scadenza lunga (quelle in scadenza
  nel mese, le piu' vicine agli 0DTE, non predicono: t 1,21 e -0,30); AGPV non prova il prezzo.

*Cambiano.*

- **Come si legge, non come si calcola: a blocchi lunghi, non minuto per minuto.** KVS al minuto non trova
  niente (tick e spread delle opzioni) e chiama l'ora "il miglior compromesso" (§5); il DEX di OptionsDepth non
  dipende dal segno stimato con le quotazioni, ma il motivo resta. E **il minuto subito dopo un orario tondo e'
  instabile**: la raffica si consuma in 30-60 secondi (AGPV §4.3), quindi nel minuto che contiene le :00 o le
  :30 la lettura di oggi puo' essere avanti o indietro rispetto allo storico, secondo come OptionsDepth data la
  colonna (13:29-13:30 o 13:30-13:31: **da verificare**). Una lettura a un minuto o due dall'orario e' piu'
  confrontabile.
- **L'etichetta "Retail" e' ancora meno sicura.** Le raffiche di AGPV sono retail riconosciuto dal
  comportamento, ma senza tipo di cliente: se cadano nei clienti non professionali (`nonprocust`, il "Retail" di
  X-Ray) o nei professionali (`procust`) non si sa. Ipotesi nostra, dall'analisi di AGPV: un bot che entra a molti
  orari con ordini a piu' gambe potrebbe superare la soglia dei 390 ordini al giorno che fa di un cliente un
  "professionale" (CLZZ nota 8, MCH App. A). La nota del riquadro potrebbe dirlo: **il retail automatico
  potrebbe stare nella colonna "Prof."**. La misura L della sintesi lo deciderebbe.

*Aggiungono, sulle raffiche all'orario* (da misurare sull'archivio prima di cambiare qualcosa).

- **Scalini nel DEX accumulato**: se le raffiche entrano nel DEX di un gruppo, la linea fa uno scalino alle :00 e
  alle :30, e dal 2023 sempre piu' anche ai quarti d'ora e ai multipli di cinque minuti (AGPV §4.2). Il
  percentile allo stesso minuto li assorbe; la **variazione degli ultimi 30 minuti** e la lettura "a occhio"
  della curva no.
- **Lo scalino ha un segno che dipende dall'ora, non dal mercato** (ipotesi nostra, dall'analisi di AGPV): le
  raffiche sono soprattutto spread venduti, e le put vendute (delta positivo per chi le vende) salgono piu' delle
  call vendute quasi a ogni orario, di piu' alle 13:30 (+7,8 contro +3,6) e ancora alle 14:00 (+5,3 contro +4,3,
  correzione della verifica); le call superano le put solo alle 10:30 e alle 15:00 (Tab. IA1). Sono quote di
  scambi, non delta pesati, e il delta netto di uno spread e' piccolo (-0,011, segno non dichiarato, Tab. 6).
- **Lo sbilancio all'orario puo' andare verso zero**: spread e iron condor alzano la somma dei |delta| (il lordo)
  piu' del netto (ipotesi nostra). Quanto pesano nel giorno: il primo minuto di ogni mezz'ora porta il 7,0% degli
  spread verticali nel 3,3% dell'orario (AGPV §6.1); dodici minuti al doppio dei vicini fanno intorno al 6% degli
  scambi del giorno (nostro conto nell'analisi). **Le raffiche non spiegano il flusso accumulato; possono
  spiegarne gli scalini.**
- **Il percentile all'orario dipende dalla volatilita' del giorno** (deduzione nostra): le raffiche pesano
  meno quando il mercato si agita (put vendute dall'8,10% al 4,69% del minuto di raffica, dal quintile di
  volatilita' piu' basso al piu' alto, Tab. 3). In una seduta agitata, allo stesso minuto, lo storico calmo ha
  raffiche piu' grandi: il confronto all'orario puo' dare un percentile spostato senza che sia cambiato niente
  di discrezionale. Un motivo in piu' per la sorpresa oltre il prezzo, non una correzione.
- **Le 14:00 dei giorni di FOMC**: gli autori dicono di trattarle a parte (§7.1), senza mostrarle; il riquadro le
  confronta con le 14:00 normali. Da dire nella lettura, non da correggere.

*Aggiungono, sullo storico che invecchia.*

- **Le raffiche crescono ogni anno e si aggiungono orari** (quarti d'ora da 1,05 a 1,3-1,5 dal 2023; cinque
  minuti da 1,02 a 1,12 nel 2026; le 13:45 al doppio nel 2026, AGPV §4.2, Fig. 2). Uno storico lungo confronta
  oggi con sedute che avevano raffiche piu' piccole o a orari diversi: il percentile a quegli orari si sposta in
  alto **senza che i gruppi abbiano fatto niente di insolito**. Il tetto di **60 sedute** (circa tre mesi) limita
  il problema e **non andrebbe allungato a molti mesi senza dirlo**; quando le piattaforme aggiungono un orario,
  per qualche settimana lo storico a quel minuto non ha lo scalino.
- **Il compromesso, da dichiarare accanto al numero**: una finestra corta segue la struttura recente delle
  raffiche ma copre un solo regime (le nostre 35 sedute sono un autunno); una lunga copre piu' regimi ma
  confronta con raffiche vecchie. Anche nei paper l'effetto cambia nel tempo: il verso di MAMU sparisce nel
  2024-2026, i picchi di AGPV crescono ogni anno; NSW invece regge nelle due meta' del campione, ma a mesi. Non
  c'e' una lunghezza giusta provata: la scelta di 60 e' una **convenzione** come le soglie 20/80.

*Aggiungono, d'altro.*

- **La moneyness cambia il segno** (NSW Tab. 2): put OTM comprate, mercato piu' basso dopo; put ITM comprate,
  piu' alto. Il DEX pesa per il delta, quindi le ITM pesano piu' delle OTM a parita' di contratti (deduzione
  dell'analisi di NSW): il netto mescola flussi che in NSW vanno in versi opposti. Se OptionsDepth desse il DEX per
  moneyness, **da verificare**.
- **Il vega**: KVS separa il flusso in vega, che muove la volatilita' (0,07 punti per deviazione standard, t
  2,36). Se OptionsDepth dia il vega scambiato per gruppo e' **da verificare**; senza, non si aggiunge.

**In una riga, dopo il supplemento**: il disegno regge ancora; il confronto allo stesso minuto in ora di New York
e' proprio quello che neutralizza le raffiche, ma il minuto subito dopo un orario tondo e la variazione degli
ultimi 30 minuti vanno letti con cautela; il tetto di 60 sedute e' una convenzione che protegge da uno storico
che invecchia e va dichiarato come tale; l'etichetta "Retail" ha un dubbio in piu' (il retail automatico potrebbe
stare fra i professionali); la misura L sull'archivio dice quanto di tutto questo tocca davvero il riquadro.

## Il Diario

- **9 novembre 2026**, fase 1: scelti sei candidati, piu' due gia' nell'indice da riusare (chordia,
  holowczak, entrambi bloccati da Cloudflare). PDF libero: adams (pagina di Fontaine, versione del 15 luglio
  2026), pan (NBER 2004), cao (jfqa.org, gennaio 2026), michael (arXiv v1). Col browser e la persona: beckmeyer
  e maurer (solo SSRN). Nuovo filone `flusso-opzioni`; adams resta nel filone `opzioni-gamma`, dove era gia'
  `trovato`. Nessuna istruzione nei PDF aperti per la verifica.
- **9 novembre 2026**, fase 2: scaricati con curl i quattro PDF liberi, tutti `%PDF` veri, frontespizio
  controllato: adams (versione del 15 luglio 2026, in `opzioni-gamma/`), pan (NBER w10925, novembre 2004),
  cao (jfqa.org, gennaio 2026), michael (arXiv v1); sha256 nella tabella. Nessuna istruzione trovata nei
  PDF. Beckmeyer: la pagina SSRN 4404704 mostra "Esecuzione della verifica di sicurezza" (Cloudflare, HTTP
  403) nella finestra di Playwright; ci si ferma, la verifica la supera la persona. Maurer (SSRN 7339718)
  non ancora aperto, dietro lo stesso controllo. Restano `trovato`.
- **9 novembre 2026**, fase 3: letto per intero adams (75 pagine, Internet Appendix compresa), scritta
  [`analisi.md`](../opzioni-gamma/adams-2025-do-sp500-options-increase-market-volatility/analisi.md),
  stato `letto`. Il paper **non prova nessun gruppo contro il prezzo**: usa i quattro gruppi solo per
  ricostruire i market maker. Il delta scambiato sommato (Net Delta dei MM) non predice la volatilita' a
  10 minuti (t -0,61); predice la gamma (t -13,16, circa un'ora), che viene per lo piu' da posizioni dei
  giorni prima. I Customers del Cboe comprendono anche gli istituzionali. Tabelle controllate con
  `pdftotext -raw`; dubbi per la verifica in fondo all'analisi. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero **cao** (72 pagine, articolo e appendice internet),
  [`analisi.md`](../flusso-opzioni/cao-2026-betting-against-the-crowd/analisi.md), stato `letto`. Corretta la
  risposta breve: il -0,070 (t -2,94) al giorno dopo c'e' (Tab. A3), ma e' lo sbilancio delle call **sui
  titoli**; sulle opzioni di SPX e NDX nessuna previsione significativa al giorno o alla settimana (Tab. 7,
  Tab. A11), e la variabile predetta e' sempre il mercato intero (MKTRF), non SPX o NDX. Tabelle rilette con
  `pdftotext -raw` (niente `pdftoppm`); dubbi per la verifica nella sezione "Cosa Non Regge". Nessuna
  istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero pan (NBER w10925, 37 pagine), scritta
  [`analisi.md`](../flusso-opzioni/pan-2006-information-in-option-volume-for-future-stock-prices/analisi.md),
  stato `letto`. Tab. 9 riletta con `pdftotext -table`: su SPX nessuna classe predice il giorno dopo; contrarian e
  significativi su NDX (discount t 3,11, other t 3,09) **e anche su OEX** (discount t 3,12, full service t 3,60),
  che il testo del paper non commenta: la "risposta breve" qui sopra va corretta per l'OEX. Prop mai informativi.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **pan** sul PDF, stato `analizzato`. Circa 230
  affermazioni controllate, tutte le cifre confermate (righe SPX, OEX e NDX della Tab. 9 rilette con
  `-table` e `-raw`: giuste); 9 correzioni, nessuna tolta. La piu' grave: "il segnale pubblico e' pressione
  sul prezzo, non informazione" diceva piu' del paper ("consistent with price pressure"); poi lo Sharpe 0,52
  che e' giornaliero, i prop "sotto il 10%" che e' una media, le classi precise del contrarian su OEX e NDX.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero michael (arXiv v1, 43 pagine, appendici comprese), scritta
  `flusso-opzioni/michael-2022-option-volume-imbalance-as-a-predictor/analisi.md`, stato `letto`. Il finding:
  sbilancio normalizzato (Up - Down)/(Up + Down) per gruppo; sulla notte, in eccesso su SPY, i clienti
  predicono nel verso (SR fino a 3,46), market maker e broker al contrario (fino a -4,42), firm e professionisti
  niente; a 10 minuti nulla di significativo dopo la correzione; nessuna prova sugli indici (e' trasversale e i
  dati Nasdaq non hanno SPX). Figure lette solo come testo (manca il programma per le immagini): la Fig. 18 e'
  ricostruita, e testo e figure si contraddicono sui segni di broker e professionisti. Nel PDF sono rimasti
  messaggi della console di R (pagina 28): non sono istruzioni. Nessuna istruzione trovata.
- **9 novembre 2026**, fase 4: verificata l'analisi di **michael** sul PDF, stato `analizzato`. Pagine rese
  come immagine (`pypdfium2` nella cartella temporanea) e figure guardate. 88 affermazioni controllate, 75
  confermate, 13 corrette, nessuna tolta. La piu' grave: la differenza 2,88 della Fig. 15 attribuita ai market
  maker, mentre la figura non dice il gruppo e i suoi segni seguono la descrizione dei broker. Poi: la Fig. 18
  ha davvero l'intestazione sbagliata (PHOTO e' la terna centrale di ogni blocco per tipo di rendimento, 30
  serie confermate), e le frasi del §7.3 e del §8 su NOTO, compreso il "SR 2,5-3" dei market maker, sono
  scritte su quell'intestazione: non verificabili; clienti contro broker -0,6 solo su PHOTO; la didascalia della
  Fig. 10 e' una terza definizione dei gruppi Q, non una conferma. Confermata la contraddizione dei segni nel
  §5. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **cao** sul PDF, stato `analizzato`. Circa 220
  affermazioni controllate, tutte le tabelle rilette con `pdftotext -table`; le righe SPX e NDX (Tab. 7A, 7B,
  A11) sono giuste. 17 correzioni, 1 tolta. La piu' grave: il -0,329 settimanale viene da 7 controlli, non
  da 17, e non e' nell'abstract; poi l'omissione di ACIB con il solo APIB accanto a una settimana (t -1,67,
  Tab. A3) e dell'ACIB pesato in modo uguale (Tab. A6). Confermati i dubbi del lettore sugli R quadro
  giornalieri (conto t^2/N) e sulle tabelle A2, A3, A4, A6 e A8A; la ITI resta ambigua anche sul PDF.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **adams** sul PDF, stato `analizzato`. 206
  affermazioni controllate, tabelle rilette con `pdftotext -table`: 196 confermate, 10 corrette o
  precisate, nessuna tolta. La piu' grave: la **colonna (1) della Tab. 5** (Net Gamma t -13,16, Net Delta
  t -0,61) **non ha gli effetti fissi della data** che il §4.1 dichiara; il finding regge con la data
  (Net Delta 0DTE e non-0DTE, t fra -1,20 e 0,95) e la Net Delta e' il delta diviso per il rendimento.
  Confermati i segni delle eq. 2-9, le Tab. 3, 4, IA5 e la formula del momento; la gamma riduce la
  volatilita' per circa un'ora (Tab. IA9); i Customers comprendono gli istituzionali (§3.4). Nessuna
  istruzione nel PDF.
- **9 novembre 2026**, fase 5: scritta [La Sintesi](#la-sintesi) sulle quattro analisi verificate (ADEFOV,
  PP, CLZZ, MCH); corretta la risposta breve (OEX contrarian in PP; in CLZZ il -0,070 e' dei titoli, non di
  SPX o NDX, e il -0,329 viene da 7 controlli). Aggiunte [la nostra misura sull'archivio di X-Ray](#la-nostra-misura-sullarchivio-di-x-ray)
  (35 sedute, descrittiva, numeri passati dall'orchestratore) e [cosa ne viene per il riquadro](#per-il-riquadro-chi-ha-comprato-delta-oggi).
  Beckmeyer e maurer restano `trovato` (Cloudflare): la sintesi si aggiorna quando arrivano. Nessuna scheda
  d'idea, nessuno studio aperto. Indice aggiornato.
- **9 novembre 2026**, fase 2, ripresa: la persona ha superato la verifica di Cloudflare nella finestra di
  Playwright; nello stesso browser, "Download This Paper" dalla pagina dell'abstract, senza altre verifiche.
  Scaricati beckmeyer (versione del 27 luglio 2024) e maurer (agosto 2026; il frontespizio dice che viene
  dalla tesi di master degli autori). Abstract letto dalla pagina: Maurer dichiara che **fuori campione** una
  regola costruita sul segnale non da' rendimenti significativi, ne' prima ne' dopo i costi, e che
  "what is left is a signal about risk, not a source of profit"; Beckmeyer identifica il retail con
  l'Automated Price Improvement Mechanism (non col codice d'origine) e trova oltre il 75% dei suoi scambi su
  opzioni S&P 500 negli 0DTE, con perdite sostanziali. Sha256 nella tabella; nessuna istruzione nei PDF.
- **9 novembre 2026**, fase 3: letto per intero **maurer** (83 pagine, appendice compresa), scritta
  [`analisi.md`](../flusso-opzioni/maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk/analisi.md),
  stato `letto`. **Tesi di master** (Frankfurt School, agosto 2026, relatori Vilkov e Cocoma), non revisionata.
  **Nessun gruppo di partecipanti**: dati ThetaData, volume di tutti. Il PCR 0DTE pesato per la gamma alle 9:35
  predice l'ora dopo (verso t -2,65, circa -3,5 punti base per deviazione standard; "se scende" logit t 3,22,
  circa +5 punti percentuali), solo nei primi 15 minuti; il verso sparisce nel 2024-2026 (p 0,205), il "se"
  regge; fuori campione (756 giornate) niente: Sharpe +0,34 lordo con intervallo sullo zero, AUC 0,531 contro
  0,520. **Il delta pesato e il flusso firmato non predicono** (t +0,07, -0,11, firmato alla Bergsma -0,23): per
  il riquadro, il "chi ha comprato" non anticipa l'ora dopo. Tabelle rilette con `pdftotext -raw`, formule
  ricostruite; dubbi per la verifica in fondo all'analisi. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero **beckmeyer** (versione del 27 luglio 2024, 50 pagine,
  appendici comprese), scritta
  [`analisi.md`](../flusso-opzioni/beckmeyer-2023-retail-traders-love-0dte-options/analisi.md), stato
  `letto`. Il paper **non prova il flusso retail contro il prezzo**: misura il profitto. Il retail d'asta
  (SLAN/MLAT, fino a 10 contratti, 2-4% del volume S&P 500, oltre il 6% degli 0DTE) perde oltre 125 milioni
  di dollari fra febbraio 2021 e settembre 2023, circa tre quarti in costi (-241.000 al giorno netti, -57.000
  lordi, Tab. 4); i market maker sono corti di 0DTE (sbilancio fra -0,5% e -1% del volume, poi sotto -1%,
  Fig. 2). Indizi indiretti di nessuna informazione sul verso: lordo negativo, delta firmato senza relazione
  col rendimento (Tab. 8, t 0,02). Tabelle rilette con `pdftotext -table`, pagine con segni e formule rese
  come immagine; incoerenze del paper (unita' della Tab. 7, segno delle ore a scadenza nella Tab. 8, "$500.000")
  elencate nell'analisi per la verifica. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero holowczak (working paper SSRN 1740046 del 13 gennaio 2011, non
  la versione pubblicata), [analisi](../opzioni-gamma/holowczak-2014-aggregating-information-in-option-transactions/analisi.md),
  stato `letto`. Per le sotto-domande 3 e 5: il flusso si somma con il segno e il peso dell'esposizione, e la
  taglia conta piu' del peso (log(volume) 0,41 contro volume 0,11 nello stesso minuto, Tab. 3A); il DEX di
  X-Ray e' la variante delta x volume, la taglia peggiore nello stesso minuto (0,11) e fra le migliori sul
  minuto dopo, ma a circa 0,01 (Tab. 3B). Il paper **non normalizza** (niente quota sul volume, niente z-score) e non separa i gruppi.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero chordia (versione accettata del 17 ottobre 2019, non la
  pubblicata), [analisi](../opzioni-gamma/chordia-2021-index-option-trading-activity-and-market-returns/analisi.md),
  stato `letto`. Per i gruppi: predicono solo le put dei **clienti piccoli** (fino a 100 contratti; IRF a 10
  settimane 0,56, errore 0,25), al contrario e a settimane; clienti grandi e firm insieme no (0,13, errore
  0,25); i market maker non risultano informati; le opzioni **SPX** non predicono (0,02, errore 0,30). Gli autori
  lo leggono come assicurazione, non informazione: la risposta breve ("i clienti nel complesso informativi sulle
  put a una settimana") va corretta in questo senso nella sintesi. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di holowczak, stato `analizzato` (dettagli nel diario di
  [opzioni-fra-indici](2026-11-09-opzioni-fra-indici.md)). Per questa ricerca: il DEX "delta x volume" e' la
  combinazione piu' bassa fra le *delta per share* nello stesso minuto (0,1052), non fra tutte le pesate per il
  delta (la *delta per dollar* con il volume e' 0,0619). Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **maurer** sul PDF, stato `analizzato`. Pagine rese
  come immagine (`pypdfium2`): formule, Tab. 5, 7, 13, 17-19 e Fig. 1 guardate. Circa 320 affermazioni
  controllate, circa 310 confermate, 10 corrette o precisate, nessuna tolta; nessun numero sbagliato. Confermati
  campione, t -2,65 e logit t 3,22 alle 9:35, Holm, sparizione dopo 15 minuti, 2024-2026 p 0,205, Sharpe +0,34
  lordo, AUC 0,531 contro 0,520, delta t +0,07 e firmato t -0,11 e -0,23. La piu' grave: l'insieme
  confermativo e' H1-H4 (H1-H3 e' solo la famiglia della correzione). Poi: la previsione del rischio fuori
  campione e' decisa dopo il risultato del trading (§5.7); la Fig. 1 e' fatta quasi tutta dai decili 1 e 10;
  incoerenze del paper (concentrazione della gamma in Tab. A.11 contro Tab. 25, finestra che si allarga
  assente dalla Tab. 26, t -1,18 stampata due volte nella Tab. 19, volumi della Tab. 5 "in milioni"). Nessuna
  istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di chordia, stato `analizzato` (dettagli nel diario di
  [opzioni-fra-indici](2026-11-09-opzioni-fra-indici.md)). Per questa ricerca: confermati i piccoli clienti
  sulle put, 0,56 (0,25), contro clienti grandi e firm 0,13 (0,25) (IA Tab. A2), e le SPX che non predicono,
  0,02 (0,30) (Tab. 7); dalla Fig. 2 l'effetto a una settimana sta nelle settimane di piu' put comprate. Chi
  stia nello sbilancio principale (clienti, o clienti e firm) il paper non lo dice in chiaro. Nessuna
  istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **beckmeyer** sul PDF, stato `analizzato`. Fig. 2, Tab.
  1-4, 7, 8, D2-D4 ed eq. 3 rese come immagine (`pypdfium2`). Circa 215 affermazioni controllate: 199
  confermate, 15 corrette o precisate, 1 tolta. Confermati la definizione del retail (SLAN/MLAT, fino a 10
  contratti), le quote (2-4%, oltre 6% degli 0DTE, tre quarti), lo sbilancio dei market maker (-0,5/-1%, poi
  sotto -1%), le perdite (125 milioni, -241.000 e -57.000 al giorno, circa 70-76% di costi), il 61,7% e il
  delta firmato 0,000 (t 0,02, col. 4 della Tab. 8). La piu' grave: **il delta firmato con gli effetti fissi del
  giorno non misura il verso** (la posizione netta del giorno finisce nella costante), quindi non e' un indizio
  di "nessuna informazione"; resta solo il lordo negativo, senza prova di significativita'. Poi: l'unita' della
  Tab. 7 e' il milione (gli effetti in dollari del testo sono dieci volte troppo piccoli); la Tab. 8 e'
  probabilmente in decimali (effetti cento volte quelli del testo, dubbio forte); il "75%" al denaro e il 74,2%
  di taglia 1 del testo non sono nella Tab. 1 (65,2% e 62,5%). Nessuna istruzione nel PDF. La sintesi, che
  aspettava beckmeyer, va aggiornata: per il riquadro conta il punto sul delta firmato.
- **9 novembre 2026**, fase 5, aggiornamento: [La Sintesi](#la-sintesi) riscritta con le otto analisi
  verificate (le quattro di prima piu' BBG, MAMU, CKMS, HHW), lette per intero con la loro sezione "La
  Verifica". Corretta la risposta breve: **Chordia e altri** non dicono "i clienti nel complesso informativi
  sulle put a una settimana" ma i **piccoli** (fino a 100 contratti), al contrario, a settimane, col VIX alto,
  letti come assicurazione; corretta anche la frase in "Cosa resta scoperto". Di **Beckmeyer e altri** non si
  usa l'indizio del delta con segno della Tab. 8 (effetti fissi del giorno, tolto in verifica), e il 61,7% della
  mattina e' poco piu' della quota oraria. **Maurer e Muller** e' una tesi di master non revisionata, senza
  gruppi: delta e flusso con il segno non anticipano l'ora dopo. Nuove contraddizioni (il verso delle put fra
  MAMU e CKMS; tre letture dei piccoli), due idee candidate in piu' (D, E), nessuno studio aperto. Aggiunto al
  riquadro il punto 5 (il disegno implementato regge; cambia l'etichetta "Retail"; le soglie 20/80 coincidono
  con i quintili di CKMS e MAMU, che non le provano). Nessun codice toccato. Indice aggiornato.
- **9 novembre 2026**, fase 1, supplemento: **SSRN aperto** nella finestra di Playwright, dopo la verifica
  superata dalla persona (un primo tentativo su `results.cfm` era fermo a Cloudflare; l'indirizzo giusto della
  ricerca e' `searchresults.cfm?term=`). 19 query sul sito e le pagine References e Citations di beckmeyer,
  maurer, adams, chordia e holowczak. Quattordici candidati nuovi verificati sulla pagina SSRN (titolo, autori,
  date, abstract, numero); scelti tre, tutti preprint con PDF solo su SSRN: kaeck (2981332), wang (3981994),
  garcia-ares (6480379); gli altri scartati con il motivo. Nessun PDF scaricato. Nessuna istruzione nelle pagine.
- **9 novembre 2026**, fase 2, terzo giro: dopo la verifica di Cloudflare superata dalla persona (riapparsa
  perche' il browser era stato chiuso), nella stessa finestra scaricati kaeck (SSRN, rivisto il 17 agosto 2019;
  il PDF non porta data), wang (8 dicembre 2021), garcia-ares (12 agosto 2026). Sha256 nella tabella; abstract
  letto dalla pagina; nessuna istruzione nei PDF. Lezione: il browser si chiude solo dopo l'ultimo paper del
  giro, perche' chiudendolo si perdono i cookie della verifica.
- **9 novembre 2026**, fase 3: letto per intero **garcia-ares** (versione del 12 agosto 2026, 50 pagine,
  Internet Appendix compresa), scritta
  [`analisi.md`](../flusso-opzioni/garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders/analisi.md),
  stato `letto`. Dati OPRA (SpiderRock), **senza tipo di cliente** e senza Open-Close. Raffiche negli 0DTE di SPX
  entro pochi secondi da ogni mezz'ora, finite in 30-60 secondi, circa il doppio degli scambi dei minuti vicini
  (piu' del triplo i complessi), crescenti ogni anno e allargate a quarti d'ora e cinque minuti; sono spread
  venduti e iron condor da menu delle piattaforme. Il retail e' riconosciuto **dal comportamento**; la quota
  "meta'-tre quinti" degli 0DTE e' una stima Cboe citata. **Nessuna prova del flusso contro il prezzo.** Per il
  riquadro: il confronto allo stesso minuto, in ora di New York, neutralizza le raffiche; restano il minuto
  dopo l'orario (instabile), lo storico che invecchia e lo sbilancio che all'orario puo' andare verso zero (da
  misurare sull'archivio). Figure e tabelle rese come immagine (`pypdfium2`); incoerenze del paper elencate per
  la verifica. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero **wang** (SSRN 3981994, versione dell'8 dicembre 2021, 38
  pagine; sul frontespizio gli autori sono nell'ordine Ni, Stouraitis, Wang, sigla NSW), scritta
  [`analisi.md`](../flusso-opzioni/wang-2021-index-options-trading-and-sentiment/analisi.md), stato `letto`.
  Confermato il riassunto, con i confini: due sole classi (clienti pubblici e firm, niente professionali, niente
  retail; per gli autori i clienti delle SPX sono in maggioranza istituzioni), dati Cboe Open-Close 1991-2018,
  **mensili** (336 mesi, Newey-West a 3 ritardi). Il DOI (call ITM da 1 a 12 mesi meno put ITM da 2 a 12, in
  apertura) predice il mercato al contrario: mese dopo -2,02 (t -3,03), tre mesi -5,74 (t -3,85), entrambe le
  meta' del campione (Tab. 3); con i controlli t fra -2,27 e -2,81 (Tab. 5). Le put **OTM** vanno nel verso
  opposto (Tab. 2). Nello stesso mese il pubblico compra call ITM quando il mercato scende (Tab. 9, t -2,45),
  come i clienti non professionali di X-Ray dentro la seduta; il rendimento del mese non e' fra i controlli.
  Fuori campione "half-period" non descritto, con R quadro spesso piu' alti che nel campione. Testo e Tab. 5 non
  coincidono (modelli, t -3,70 e -2,10 assenti, segno del VRP); "fino a nove mesi" senza tabella. Tutte le
  tabelle e le figure guardate come immagine (`pypdfium2`), perche' il testo estratto ne lascia vuote tre.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 3: letto per intero **kaeck** (SSRN 2981332, rivisto il 17 agosto 2019 secondo
  SSRN, il PDF non porta data; 51 pagine, Web Appendix compresa), scritta
  [`analisi.md`](../flusso-opzioni/kaeck-2017-informed-trading-in-the-index-option-market/analisi.md), stato
  `letto`. Somma il flusso delle opzioni SPX come il DEX (contratti x delta x segno, per il prezzo), ma su
  **tutti gli scambi, senza gruppi**, segnati con le quotazioni, solo mensili da 7 a 180 giorni (**niente
  0DTE**), solo il 2014, a un'ora. Il delta comprato va con il prezzo **nella stessa ora** (98,13 cents in
  quattro ore per deviazione standard, t 4,41, 5,1 punti base), attribuito al flusso dall'ordinamento del VAR;
  i ritardi **non predicono** l'ora dopo (t 0,6 e 0,4, Tab. 4), e il prezzo dell'ora prima predice il flusso
  (t 5,7). Al minuto niente. Significativita' per ora (Newey-West), non per giornata. Tab. 1-6, eq. 2-4 e
  figure rese come immagine (`pypdfium2`); incoerenze del testo elencate per la verifica. Nessuna istruzione
  nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **kaeck** sul PDF, stato `analizzato`. Tab. 1-7, Fig. 1,
  A1 e A2 rese come immagine (`pypdfium2` nella scratchpad). Circa 190 affermazioni controllate, circa 180
  confermate, 9 corrette o precisate, nessuna tolta. Confermate cella per cella la Tab. 4 (ritardi del delta sul
  prezzo t 0,6 e 0,4; prezzo sul delta t 5,7 e 2,2) e la Tab. 3 (98,13, t 4,41, t in valore assoluto), i dati
  (2014, 249 giornate, 1.494 ore, 1.804.397 scambi, mensili 7-180 giorni, nessun gruppo), il Wald, la Tab. 7 al
  minuto; rifatto il conto della stessa ora (105,7 cents = 22,5 + 10,5 + 72,8), che torna con la Fig. 1; la
  forma ridotta (nostro conto) da' circa zero al ritardo di un'ora. La piu' grave: **l'ordinamento non e'
  quello che dice il testo**: nella Tab. 4 il delta muove SPY nella stessa ora, quindi la parte comune va al
  delta; con SPY piu' esogeno (Fig. A2) l'effetto si legge intorno a 70-75 cents invece di 98. Le incoerenze
  elencate dal lettore sono tutte del paper; aggiunte quella dei tick (Tab. 6) e del "25%". Nessuna istruzione
  nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **wang** (NSW, SSRN 3981994), stato `analizzato`.
  Circa 230 affermazioni controllate sul PDF (sha256 uguale), tutte le tabelle e le figure rese come immagine
  (`pypdfium2` in un ambiente della cartella temporanea); 11 corrette, 0 tolte. Confermate cella per cella le Tab.
  2-9, compreso il verso della Tab. 9 (Panel B su -PNBP) e le incoerenze testo-tabella. La piu' grave: la Fig. 1
  non regge la scelta delle scadenze come diceva l'analisi (le put ITM hanno b positivo a ogni scadenza, e oltre
  i due mesi anche le call ATM e OTM sono vicine a zero). La chiave `wang-2021` non segue la regola del primo
  autore (Ni): segnalato, non rinominato; nell'indice l'ordine degli autori corretto in Ni, Stouraitis, Wang.
  Elenco nella sezione "La Verifica" dell'analisi. Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 4: verificata l'analisi di **garcia-ares** (AGPV, SSRN 6480379, versione del 12
  agosto 2026), stato `analizzato`. Circa 190 affermazioni controllate sul PDF (sha256 uguale), figure e
  tabelle rilette come immagine (`pypdfium2` nella scratchpad); 172 confermate, 17 corrette o completate, 1
  tolta. Confermati i dati (OPRA via SpiderRock, 19 maggio 2022 - 31 gennaio 2026, nessun tipo di cliente, nota
  2), le raffiche (circa il doppio, piu' del triplo i complessi; 1,3-1,5 e 1,12), Tab. 2 (3,6 -> 6,7), Tab. 3
  (8,10% e 4,69%), Tab. 7 (ROC-AUC 0,685), la quota retail "meta'-tre quinti" citata da Cboe (§2.1, nota 4), e
  che **non c'e' nessuna regressione sul rendimento dell'indice**. La piu' grave, nel riquadro: alle 14:00 non
  prevalgono le call vendute (+4,3) ma ancora le put (+5,3, Tab. IA1); le call superano le put solo alle 10:30 e
  alle 15:00. Tolta come incoerenza il "3,3% dell'orario" (12/360 col taglio dei primi 25 e degli ultimi 5
  minuti); aggiunte due incoerenze del paper (delta dello strike venduto nella Tab. 4, finestra della Fig. 3) e
  il segno dei greci della Tab. 6, tutti dal lato lungo. Elenco nella sezione "La Verifica" dell'analisi.
  Nessuna istruzione nel PDF.
- **9 novembre 2026**, fase 5, secondo aggiornamento: [La Sintesi](#la-sintesi) riscritta con undici analisi
  verificate, le otto di prima piu' **KVS** (kaeck), **NSW** (wang) e **AGPV** (garcia-ares), lette per intero
  con la loro sezione "La Verifica". KVS: il flusso SPX in delta va con il prezzo nella stessa ora e non lo
  anticipa (ritardi t 0,6 e 0,4), e la verifica ha corretto l'ordinamento con SPY (la parte comune va al delta;
  con SPY prima circa 70-75 cents invece di 98). NSW: al contrario, mensile, solo sulle ITM; le put OTM nel verso
  opposto; la chiave `wang-2021` non segue la regola del primo autore (Ni), segnalato e non rinominato. AGPV:
  raffiche a ogni mezz'ora, quarto d'ora e cinque minuti, nessun rendimento guardato; la verifica ha corretto put
  e call alle 14:00 (put +5,3, call +4,3). Aggiornate la risposta breve, gli stati di kaeck e wang, le
  convergenze (nuova la 9, la forma dentro la giornata), le contraddizioni (il verso delle put con la moneyness,
  "informato", chi insegue il prezzo, quanto e' il retail), le idee candidate (K e L nuove; L descrittiva
  sull'archivio). Aggiunto al riquadro il punto 6: confermato il confronto allo stesso minuto in ora di New York;
  cautela sul minuto dopo l'orario e sulla variazione di 30 minuti; il tetto di 60 sedute come convenzione contro
  lo storico che invecchia; il retail automatico che potrebbe stare fra i professionali. Nessun codice toccato,
  nessuno studio aperto. Indice aggiornato.
