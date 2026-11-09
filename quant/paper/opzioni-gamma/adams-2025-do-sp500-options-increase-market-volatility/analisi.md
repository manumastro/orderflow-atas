---
chiave: adams-2025-do-sp500-options-increase-market-volatility
sigla: ADEFOV
titolo: "Do S&P 500 Options Increase Market Volatility? Evidence from 0DTEs"
autori: [Greg Adams, Chukwuma Dim, Bjorn Eraker, Jean-Sebastien Fontaine, Chay Ornthanalai, Grigory Vilkov]
versione_letta: "15 luglio 2026 (assorbe Dim, Eraker, Vilkov, SSRN 4692190, e un secondo working paper)"
fonte: pagina personale di Jean-Sebastien Fontaine (coautore)
identificativo: "SSRN 5641974"
url: https://www.jean-sebastienfontaine.com/papers/0dte-options-volatility.pdf
tipo: working paper
revisione_fra_pari: no
pagine: 75
pdf: adams-2025-do-sp500-options-increase-market-volatility.pdf
pdf_sha256: 6b256ade06bd64282dd8cfd4855521dfb1b0f404b687c0191c0edd61d73791ea
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; dati Cboe Open-Close Volume Summary a 10 minuti e Cboe Option Quotes al minuto (a pagamento), CME (ordini e quote degli E-mini), DTN IQFeed, OptionMetrics, OPRA"
---

# Adams, Dim, Eraker, Fontaine, Ornthanalai, Vilkov 2026: L'Analisi

*Do S&P 500 Options Increase Market Volatility? Evidence from 0DTEs*, versione del 15 luglio 2026, 75
pagine (frontespizio, 52 di testo con figure, tabelle e l'appendice dei dati A1, la copertina e 21 pagine
di Internet Appendix). **Working paper, non revisionato**; presentato a molte conferenze, con i nomi dei discussant
nel frontespizio. Assorbe *0DTEs: Trading, Gamma Risk and Volatility Propagation* (Dim, Eraker, Vilkov) e
*The Market for 0DTE options: The Role of Liquidity Providers in Volatility Attenuation*. Fontaine lavora
alla Bank of Canada; le opinioni sono degli autori. Letto per intero il 9 novembre 2026, ricerca
[`2026-11-09-flusso-delle-opzioni-per-gruppo`](../../ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md).

**Come e' stato letto.** Il testo e' stato estratto con `pdftotext -layout` e poi, per tabelle e formule,
pagina per pagina con `pdftotext -raw -enc UTF-8`, che tiene l'ordine delle celle. Il lettore di PDF
dell'agente qui non funziona (manca `pdftoppm`). Nelle formule i simboli escono sostituiti: la delta
diventa `#`, la gamma `$`, il segno meno una freccia; il segno delle formule (2)-(9) e' stato ricostruito
da questa sostituzione, ed e' coerente con il testo (vedi "I dubbi di lettura" in fondo). Citazioni
`[ADEFOV §n]`, `[ADEFOV Tab. n]`, `[ADEFOV Tab. IAn]` per l'Internet Appendix, `[ADEFOV Fig. n]`.

**Perche' interessa a noi.** E' l'unico paper trovato con **lo stesso dato** del riquadro "Chi ha
comprato delta oggi" della pagina X-Ray: il Cboe Open-Close di SPX diviso nei quattro gruppi di
OptionsDepth. **Ma non chiede quale gruppo anticipa il prezzo.** Usa i gruppi solo per ricostruire, per
differenza, la posizione dei market maker, e studia la loro gamma.

## In Parole Semplici

- Nei giorni in cui scadono opzioni 0DTE (quelle che scadono il giorno stesso), la volatilita' a 10
  minuti dell'S&P 500 e' **piu' bassa di circa il 7%**. Il motivo che gli autori propongono: i market
  maker sono **lunghi di gamma** su quelle opzioni, e coprendosi comprano quando l'indice scende e
  vendono quando sale, cioe' fanno da **freno**.
- Quella gamma **non viene dagli scambi della giornata**: viene da posizioni aperte nei giorni prima,
  per meta' il giorno prima e per circa il 90% nella settimana prima. Gli scambi 0DTE del giorno sono
  enormi, ma i market maker li compensano fra loro: a fine giornata lo sbilancio netto dei clienti sugli
  0DTE al denaro e' sotto i 5.000 contratti su circa un milione scambiati.
- **Il delta scambiato** dai gruppi (la cosa che il riquadro somma) **non predice la volatilita'** dei
  10 minuti dopo e non cambia nei giorni con gli 0DTE. Conta la gamma, cioe' lo stock di posizioni,
  non il flusso di delta. Attenzione: nel paper il delta scambiato e' sempre **diviso per il rendimento
  dei 10 minuti** (la Net Delta), una misura molto rumorosa; il delta scambiato cosi' com'e' non e' provato.
- La gamma dei market maker alta predice, nell'ora dopo: meno volatilita', ordini sugli E-mini che
  vanno contro il movimento appena fatto (piu' forti per circa 30 minuti), e un mercato che tende a
  tornare indietro invece di proseguire.
- Il gruppo che il riquadro chiama **retail** (i clienti non professionali di Cboe) per il paper
  comprende **anche gli istituzionali**. Sugli 0DTE questi clienti sono, nel complesso, **corti di
  straddle al denaro e lunghi di ali lontane** (una farfalla o un condor di ferro), con gamma molto
  negativa.
- Nessun test sul verso del prezzo per gruppo, nessun fuori campione, nessun costo, significativita'
  per intervallo di 10 minuti e non per giornata.

## I Termini

- **0DTE** (*zero days to expiration*): opzione SPX che scade alla chiusura del giorno stesso. Sono le
  SPXW, regolate sul prezzo di chiusura dell'indice (PM); le SPX regolate all'apertura (AM) non diventano
  mai 0DTE [§2.1, nota 1].
- **Delta**: di quante unita' di sottostante si muove un'opzione. **Gamma**: quanto cambia il delta quando
  il prezzo si muove. Chi e' **lungo di gamma** e si copre vende sui rialzi e compra sui ribassi (freno);
  chi e' **corto** fa il contrario (acceleratore).
- **Market maker** (MM): chi fornisce liquidita' sulle opzioni. Nel paper non e' osservato: e' **il
  negativo della somma degli altri quattro gruppi** [§3.4, eq. 9].
- **I quattro gruppi del Cboe** [§3.4]: **Customers**, gli investitori pubblici, "sia retail sia
  istituzionali"; **Professional Customers**, conti pubblici con molta attivita' in giornata; **Firm
  Proprietary**, conti propri di banche o societa' di trading membri della OCC (la camera di
  compensazione delle opzioni quotate); **Broker-Dealers**, ordini delle case di brokeraggio.
- **I quattro versi**: open-buy (comprato per aprire), open-sell (venduto per aprire), close-buy (comprato
  per chiudere), close-sell (venduto per chiudere) [§3.4].
- **OIB** (*order imbalance*, sbilancio): acquisti meno vendite di un contratto e di un gruppo in un
  intervallo. **NOI** (*net open interest*): la posizione netta di un gruppo, somma degli OIB dalla
  nascita del contratto [§3.4, eq. 7-8].
- **Net Gamma** (Net Gamma_t): la gamma dei MM come **percentuale delle azioni dell'S&P 500 che dovrebbero
  vendere per coprirsi da un rialzo dell'1%** (e comprare per un ribasso). Positiva vuol dire freno
  [§3.2, eq. 5].
- **Net Delta** (Net Delta_t): la copertura richiesta dalle **posizioni nuove** dell'intervallo, nella
  stessa unita', divisa per il rendimento dell'intervallo per renderla confrontabile con la Net Gamma
  [§3.3, eq. 6]. E' la versione del paper di "chi ha comprato delta", vista dal lato dei MM.
- **Stale e New 0DTE**: posizioni 0DTE dei MM aperte **prima** delle 16:00 del giorno precedente
  (stale, "vecchie") o **dopo** (new) [§3.6].
- **Volatilita' realizzata** a 10 minuti: radice della somma dei quadrati dei rendimenti logaritmici di 1
  minuto, annualizzata con 252 giorni e 39 intervalli al giorno [§2.3].
- **Effetti fissi** (*fixed effects*, FE): variabili indicatrici che tolgono la media di un gruppo
  (il giorno, la settimana, l'ora): il coefficiente resta identificato solo dalle variazioni dentro il
  gruppo. **Newey-West**: errori standard corretti per l'autocorrelazione dei residui. **2SLS**: regressione
  a due stadi, il primo isola la parte della variabile dovuta a uno strumento esogeno.
- **Differenza nelle differenze** (DiD): si confronta il cambiamento di un gruppo trattato con quello di un
  gruppo di controllo, prima e dopo un evento.

## La Domanda

Gli 0DTE aumentano la volatilita' dell'S&P 500 attraverso la copertura dei market maker? Se no, qual e'
il meccanismo, e si vede dentro la giornata nella volatilita', nel flusso degli E-mini e nel momento
[§1]?

## Il Metodo

**I dati** [§2.3, §3.4, App. A1]:

- **Cboe Open-Close Volume Summary** di SPX, solo la borsa C1 di Cboe, **istantanee a 10 minuti** del
  volume cumulato dall'inizio del giorno, per contratto, per gruppo e per verso; gennaio 2018 - dicembre
  2024. Il volume di un intervallo e' la differenza fra due istantanee. Prima del 2018 solo giornaliero
  (2012-2017).
- **Cboe Option Quotes** al minuto: prezzi, volatilita' implicita, **delta e gamma** di ogni contratto.
- **E-mini S&P 500** (CME): trade e migliore denaro-lettera, per il flusso degli ordini a 10 minuti;
  contratto piu' vicino, cambiato a 8 giorni dalla scadenza. Rendimenti di 1 minuto dal prezzo medio
  (DTN IQFeed). SPY e indice a pronti come prove di robustezza.
- Solo la **cassa, 9:30-16:00 di New York**; 39 intervalli di 10 minuti al giorno [App. A1, §2.3].
- OPRA per i volumi su tutte le borse (Fig. 1); OptionMetrics per dividendi e tassi.

**La posizione dei MM, passo per passo** [§3.4, eq. 7-9]. Per ogni contratto i, gruppo j e intervallo t:

```text
OIB(i,j,t) = OpenBuy + CloseBuy - OpenSell - CloseSell            (7)
NOI(i,j,t) = somma degli OIB(i,j) dalla nascita del contratto a t   (8)
OIB_MM(i,t) = - somma_j OIB(i,j,t)     NOI_MM(i,t) = - somma_j NOI(i,j,t)   (9)
```

Ogni contratto e' "in offerta netta zero": quello che i quattro gruppi comprano in netto, qualcuno lo
vende, e il paper assume che sia il market maker.

**La copertura dei MM** [§3.1-3.3, eq. 2-6]. Con w la posizione in opzioni, la copertura in azioni e'
`-w x delta`. Il suo cambiamento in un intervallo si divide in due pezzi:

```text
q(i,t) = -w(i,t-1) x [delta(i,t) - delta(i,t-1)]  circa  -w(i,t-1) x gamma(i,t) x dS   (4) posizione vecchia
n(i,t) = -[w(i,t) - w(i,t-1)] x delta(i,t)                                       (3) posizione nuova

Net Gamma_t = -100 x somma_i q / M / (dS/S)  circa  (100 x S / M) x somma_i w(i,t-1) x gamma(i,t)   (5)
Net Delta_t = -100 x somma_i n / M / (dS/S)  circa  100 / (M x R_t) x somma_i [w(i,t)-w(i,t-1)] x delta(i,t)   (6)
```

con M = **numero di azioni dell'S&P 500** (capitalizzazione divisa per il livello dell'indice, alla
chiusura del giorno prima), 100 azioni per contratto, R_t il rendimento logaritmico dell'intervallo. Per
la Net Delta, se |R_t| < 0,001% (meno dell'1% delle osservazioni) il rendimento si somma all'indietro fino
a cinque intervalli; se non basta, il dato manca [nota 11]. Delta e gamma vengono dai dati Cboe; in una prova
di robustezza da Black-Scholes con il VIX come volatilita' [Tab. IA7].

**Quindi**: `somma_i OIB(i,j,t) x delta(i,t)` e' il **delta scambiato dal gruppo j** nell'intervallo, la
quantita' che il DEX di OptionsDepth da' al minuto. Il paper non la guarda gruppo per gruppo: la somma sui
quattro, cambia il segno, la divide per M e per R_t, e la chiama Net Delta dei MM.

**Le regressioni.**

- **Effetto degli 0DTE sulla volatilita'** [§2.4, eq. 1]: panel di intervalli di 10 minuti, volatilita'
  su una indicatrice "oggi scade uno 0DTE", gennaio 2018 - 19 maggio 2022. Identificazione: fra il 2016 e
  aprile 2022 gli 0DTE scadevano lunedi', mercoledi' e venerdi', e **solo a volte** martedi' o giovedi'
  (fine mese, fine trimestre, festivi spostati) [§2.2]; nel 2021, per esempio, 11 giorni [nota 7]. FE
  Anno x Settimana x Ora e Anno x Giorno della settimana; Newey-West a 5 ritardi **dentro la giornata**
  [nota 9].
- **DiD sull'introduzione** degli 0DTE di martedi' e giovedi' (aprile e maggio 2022), 2018-2024 [§2.5].
- **Effetto degli 0DTE sulla gamma dei MM**, e 2SLS dalla gamma alla volatilita' [§3.9].
- **Predittive dentro la giornata** [§4]: volatilita' di t+1 (fino a t+12) sulla Net Gamma e Net Delta di
  t, con due ritardi della volatilita', FE Data e Anno x Settimana x Ora [eq. 10] (ma la colonna (1)
  della Tab. 5 ha solo Anno x Settimana x Ora, vedi il risultato 12); flusso degli E-mini di
  t+1 sul rendimento di t, la Net Gamma e il loro prodotto [eq. 11]; "momento" per quartili di Net Gamma
  [Tab. 7].

**Il flusso degli E-mini** [§4.3, App. A1]: ogni trade e' un acquisto o una vendita secondo il lato del
libro; poi due misure a 10 minuti:

```text
Order Flow      = (Buy - Sell) x 50 / M          (rispetto alla dimensione dell'S&P 500)
Order Imbalance = (Buy - Sell) / (Buy + Sell)    (quota del volume lordo, fra -1 e +1)
```

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Volume**: nel 2015 circa 1 milione di contratti SPX al giorno, 0DTE meno di un quinto; nel 2024 oltre 3 milioni, 0DTE quasi meta' | §2.1, Fig. 1 |
| 2 | **Effetto degli 0DTE sulla volatilita' a 10 minuti** (ES, 2018 - 19 maggio 2022, 42.831 intervalli): -0,00456 (t -4,91) con FE Anno x Settimana x Ora; -0,00627 (t -3,02) con il giorno della settimana; **-0,00613 (t -2,99)** con Anno x Giorno della settimana, la specifica di base: **-61 punti base annualizzati, circa il 7% della media di 9,10%** | §2.4, Tab. 1 |
| 3 | Robustezza: 2012-2022, da -0,00326 (t -3,29) a -0,00427 (t -5,97); su SPY -0,00712 (t -3,39), sull'indice a pronti -0,00570 (t -2,91); con Newey-West a 10 ritardi o errori raggruppati per Anno x Ora, t fra -2,74 e -9,40 | Tab. IA1, IA2, IA3 |
| 4 | **DiD** (2018-2024, 66.656 intervalli): martedi'/giovedi' +0,0045 (t 4,87); introduzione -0,0097 (t -1,12, non significativo); **martedi'/giovedi' x introduzione -0,0054 (t -4,27)**: la volatilita' in piu' di martedi' e giovedi' sparisce quando gli 0DTE scadono ogni giorno | §2.5, Tab. 2 |
| 5 | **La gamma dei MM e' positiva** (freno) nell'**88%** degli intervalli di 10 minuti; la media mensile, sommando 0DTE e non, e' sempre positiva | §3.5, Fig. IA2-IA4 |
| 6 | **Statistiche** (percentuale delle azioni dell'S&P 500 per un movimento dell'1%), prima e dopo il 19 maggio 2022: Net Gamma media 0,884 e **1,424**; di cui 0DTE 0,233 e 0,542, non-0DTE 0,644 e 0,867. Deviazione standard media dentro la giornata della Net Gamma dopo maggio 2022: 0,547 (0DTE 0,512, non-0DTE 0,129). **Net Delta** media 0,084 e 0,281, mediana 0,080 e 0,220, **deviazione standard 3,971 e 5,559** | §3.5, Tab. 3 |
| 7 | **Stale e New**: nei giorni con 0DTE prima di maggio 2022, stale 0,293 e new 0,083; dopo, 0,392 e 0,159 (mediana new 0,027). Delle posizioni stale, circa **meta' aperta il giorno prima**, circa il **90% nella settimana prima**, pochissime oltre due settimane. Durante la giornata la mediana della new resta vicina a zero, la stale parte intorno a 0,3 e resta li' fino alla chiusura (Fig. 3 e 4: solo 19 maggio 2022 - dicembre 2024) | §3.6, Tab. 3, Fig. 3-4 |
| 8 | **I clienti sugli 0DTE** (19 maggio 2022 - dicembre 2024): partono **corti** di opzioni al denaro o appena fuori, **lunghi** di quelle molto fuori (farfalla o condor di ferro); durante il giorno aumentano le lunghe fuori dal denaro, le corte restano; la loro **gamma netta e' molto negativa** tutto il giorno, ed e' lo specchio della gamma positiva dei MM | §3.7, Fig. 5 |
| 9 | **I MM compensano il flusso del giorno**: lo scambio 0DTE si concentra al denaro e cresce durante il giorno, circa 1 milione di contratti al giorno, ma lo **sbilancio cumulato dei clienti sugli 0DTE al denaro e' sotto i 5.000 contratti** verso la fine della giornata; piu' grande lontano dal denaro, dove la gamma conta poco | §3.8, Fig. 6 |
| 10 | **Effetto degli 0DTE sulla gamma dei MM** (2018 - maggio 2022): 0DTE +0,00275 (t 9,20), cioe' +27,5 punti base; **stale +0,00247 (t 11,70)**; new +0,00029 (t 1,18, non significativo); non-0DTE -0,00137 (t -7,02), sostituzione; **totale +0,00153 (t 4,66)**. 2SLS: **-4,04 (t -2,90)**, cioe' 1 punto base di gamma in piu', 4 punti base di volatilita' in meno | §3.9, Tab. 4 |
| 11 | **Il delta dei MM non cambia nei giorni con 0DTE**: 0,00062 (t 0,87) sugli 0DTE, 0,0000109 (t 0,01) sui non-0DTE, 0,00062 (t 0,59) in totale. Gli autori: e' coerente con i MM che bilanciano acquisti e vendite degli utenti finali, e **contro** l'idea che il trading direzionale degli 0DTE guidi gamma e volatilita' | §3.9, Tab. IA5 |
| 12 | **Gamma e volatilita' dei 10 minuti dopo** (2018-2024, circa 64.800 intervalli): Net Gamma **-0,4042 (t -13,16)**; una deviazione standard dentro la giornata (0,54) vuol dire circa **-22 punti base** annualizzati. **Questa colonna (1) ha solo gli effetti fissi Anno x Settimana x Ora, senza quelli della data** (riga "Date" vuota; R quadro interno 0,330 contro circa 0,117 delle altre), benche' il testo dichiari la data in ogni specifica. Con la data: 0DTE -0,4392 (t -8,82), non-0DTE -0,9609 (t -3,31), insieme -0,4211 (t -8,51) e -0,9173 (t -3,28): per unita' di gamma non distinguibili; per deviazione standard circa -23 e -12 punti base | §4.1, Tab. 5 |
| 13 | **Il delta scambiato non predice la volatilita'**: Net Delta **-0,0015 (t -0,61)**, nella colonna (1) senza effetti fissi della data; con la data, 0DTE -0,0040 (t -1,20) e non-0DTE +0,0027 (t 0,95), insieme -0,0032 (t -0,93) e +0,0012 (t 0,44); con le greche dal VIX t fra -1,09 e 0,80 (Tab. IA7). Correlazione media nella giornata fra volatilita' e Net Delta: -0,10% (non-0DTE) e 0,81% (0DTE); fra volatilita' e Net Gamma: -19,10% (non-0DTE) e -13,40% (0DTE) | Tab. 5, Tab. IA4, IA7 |
| 14 | Per scadenza: 1-5 giorni -0,7503 (t -4,67); oltre 5 giorni -1,096 (t -1,74, debole). Con delta e gamma dal VIX: 0DTE da sola -0,4536 (t -8,05), non-0DTE da sola -1,184 (t -5,60); nella colonna per scadenze 0DTE -0,4443 (t -7,87), 1-5 giorni -0,4822 (t -2,30), **oltre 5 giorni -4,406 (t -7,00)** | Tab. 5, Tab. IA7 |
| 15 | **Per ora**: l'effetto della gamma 0DTE e' piu' forte all'apertura (9-10: -0,900, t -9,32) e piu' debole alle 15 (-0,350, t -6,64); sulla non-0DTE fra -0,70 e -1,28, tutti significativi | Tab. IA6 |
| 16 | Con speed (gamma della gamma) e charm (decadimento del delta nel tempo) fra i controlli, la gamma 0DTE resta -0,4213 (t -8,53), la non-0DTE -0,9287 (t -3,29); charm non significativa; **speed significativa**, anch'essa negativa (0DTE -0,2736, t -2,19; non-0DTE -0,8295, t -3,64), non commentata nel testo | Tab. IA8 |
| 17 | **Orizzonte**: l'effetto della gamma 0DTE dura **circa un'ora**: t+1 -0,4394 (t -8,82), t+3 -0,3264 (t -4,84), t+6 -0,2261 (t -2,74), t+7 -0,1695 (t -1,90), poi zero; non-0DTE significativa fino a t+6 (-0,4776, t -2,14). Somma dei coefficienti 0DTE nella prima ora circa -2,0, per +27,5 punti base di gamma: circa -55 punti base, vicino ai -61 del risultato 2 | §4.2, Fig. 8, Tab. IA9, IA10 |
| 18 | **Flusso degli E-mini dopo un movimento** (56.148 intervalli): Order Imbalance sul rendimento -5,824 (t -18,93), sul prodotto rendimento x Net Gamma **-452,9 (t -10,84)**; Order Flow -0,0026 (t -13,27) e -0,1696 (t -6,39). **La Net Gamma da sola non predice il verso del flusso** (Order Imbalance -0,2605, t -1,52). Dopo un ribasso di 14 punti base (una deviazione standard) gli acquisti superano le vendite dello 0,8% del volume lordo nei 10 minuti dopo; con la gamma a 55 punti base, circa 1,2% | §4.3, Tab. 6 |
| 19 | L'inversione del flusso in piu' con gamma alta e' significativa per **circa 30 minuti** e svanisce entro l'ora | §4.3, Fig. 9 |
| 20 | **"Momento" per quartili di Net Gamma** (punti base, quartili su una finestra mobile di 180 giorni all'indietro): Q4 meno Q1 a 10 minuti -0,119 (t -0,71), a 30 **-1,055 (t -2,99)**, a 60 **-1,777 (t -3,93)**, a 120 **-2,888 (t -4,18)**; Q4 da solo -1,144 (t -1,97) a 60 e -2,158 (t -2,46) a 120; Q1 positivo ma debole (0,633, t 1,73 a 60) | §4.4, Tab. 7 |
| 21 | Separando 0DTE e non-0DTE: Q4 meno Q1 a 120 minuti -2,249 (t -4,01) per la gamma 0DTE, -4,676 (t -5,12) per la non-0DTE | Tab. IA11 |
| 22 | Gli autori: il risultato del momento **non e' una strategia** perche' la gamma dei MM non e' osservabile dagli altri investitori. Dicono pero' che coprire ad alta frequenza una gamma positiva (gamma scalping) puo' essere redditizio in media **se i costi sono bassi**, senza misurarlo | §4.4, §4 e nota 15 |

## Il Perche'

**Il meccanismo degli autori** [§1, §3.2, §3.11, §4]: i clienti, nel complesso, vendono straddle 0DTE al
denaro; i market maker li comprano e sono **lunghi di gamma**. Per restare coperti vendono quando l'indice
sale e comprano quando scende, nell'intervallo dopo. Se il mercato degli E-mini non e' perfettamente
elastico, questo flusso contrario smorza la volatilita', rende piu' forte l'inversione del flusso e
indebolisce il momento. La gamma che conta e' quella **ereditata** (stale): le posizioni si costruiscono
nei giorni prima, e i market maker gestiscono l'inventario in piu' giorni [§5]. Il flusso del giorno
conta poco perche' i MM lo compensano: comprano e vendono al denaro da clienti diversi e restano quasi
piatti [§3.8].

**Le alternative che scartano.**

- **Un fattore di calendario** (fine mese, festivi) che muove la volatilita' per conto suo: dovrebbe
  sparire esattamente quando gli 0DTE diventano quotidiani, nel 2022; la DiD dice che e' quello che
  succede [§2.5].
- **Il trading direzionale degli 0DTE**: la Net Delta non cambia nei giorni con 0DTE e non predice la
  volatilita' [§3.9, Tab. IA5, Tab. 5].
- **Termini di ordine superiore** (speed, charm): non cambiano il risultato [Tab. IA8].
- **La misura delle greche**: con il VIX al posto della volatilita' implicita di ogni contratto il
  risultato regge [Tab. IA7].
- **Brogaard, Han e Won** trovano piu' volatilita' con piu' volume 0DTE: per gli autori quell'identificazione
  dipende dal calendario d'introduzione delle SPXW, e conta lo stock di posizioni, non la quota di volume
  [§1].

**Cosa lo ucciderebbe**, dedotto dal perche': una gamma dei MM negativa nei giorni con 0DTE (i clienti
lunghi di straddle); un effetto sulla volatilita' che non dipende dalla gamma; un'inversione del flusso
degli E-mini uguale con gamma alta e bassa; un effetto della DiD che non sparisce nel 2022. Per la
nostra domanda: un **delta scambiato da un gruppo** che predice la volatilita' o il prezzo, cosa che il
paper non prova.

## Cosa Dice Per La Nostra Domanda

La domanda: il flusso di SPX per gruppo cosa dice del prezzo e della volatilita' intraday, su che
orizzonte, con che significativita', e come si misura e normalizza.

1. **Sul prezzo per gruppo il paper non dice niente.** Non c'e' nessuna regressione del rendimento
   sul flusso di un gruppo. I gruppi entrano solo sommati, nella posizione dei MM [§3.4, eq. 9].
2. **Sul delta scambiato sommato** (la Net Delta dei MM, cioe' il delta scambiato dai quattro gruppi con
   il segno cambiato) la risposta e' **negativa per la volatilita'**: non predice quella dei 10 minuti
   dopo (t -0,61 nella colonna senza effetti fissi della data; con la data, separata in 0DTE e non-0DTE,
   t fra -1,20 e 0,95), non e' correlata con quella dello stesso intervallo, non cambia nei giorni con 0DTE
   [Tab. 5, IA4, IA5]. Il paper la chiama un flusso, opposto allo stock della gamma [Tab. IA5, didascalia].
   Due avvertenze: la misura provata e' il delta **diviso per il rendimento dell'intervallo**, non il delta
   scambiato in se'; e il test e' sulla somma dei quattro gruppi. **Sul verso del prezzo non e' provata.**
3. **Quello che predice e' la gamma**, cioe' la posizione accumulata, non il flusso: volatilita' piu'
   bassa per circa un'ora, flusso degli E-mini contro il movimento per circa 30 minuti, inversione del
   prezzo da 30 minuti a 2 ore [risultati 12, 17-20]. Significativita' con t fra 2 e 13, ma **per
   intervallo di 10 minuti**, non per giornata.
4. **Il gruppo "retail" del riquadro non e' solo retail.** I Customers del Cboe, secondo il paper,
   comprendono retail e istituzionali [§3.4]. Il loro comportamento tipico sugli 0DTE e' vendere
   volatilita' al denaro e comprare ali lontane [§3.7].
5. **Lo stock pesa piu' del flusso**: la gamma 0DTE dei MM viene per la maggior parte da posizioni aperte
   prima del giorno [§3.6], e il flusso netto dei clienti al denaro nel giorno e' piccolo [§3.8]. Un
   riquadro che somma il delta scambiato **da oggi** vede la parte minore.
6. **La normalizzazione** che usano, utile al riquadro:
   - il delta scambiato in **azioni equivalenti** (100 x OIB x delta), diviso per **M**, il numero di
     azioni dell'S&P 500 alla chiusura di ieri: diventa una **percentuale dell'indice**, confrontabile fra
     giorni e anni [eq. 5-6];
   - per un flusso, la quota del **volume lordo**: (acquisti - vendite) / (acquisti + vendite), fra -1 e
     +1 [§4.3];
   - le dimensioni si leggono in **deviazioni standard dentro la giornata** (la media, giorno per giorno,
     della deviazione standard nella giornata) [Tab. 3];
   - le soglie (quartili) si calcolano su una **finestra mobile di 180 giorni all'indietro**, senza
     guardare avanti [Tab. 7];
   - la Net Delta divisa per il rendimento dell'intervallo e' **molto rumorosa** (deviazione standard 5,6
     contro una media di 0,28, dopo maggio 2022) [Tab. 3]: per un riquadro non serve dividere per il
     rendimento.

**Una nostra lettura, non del paper**: la Net Delta dei MM ha media e mediana **positive** in entrambi i
periodi [Tab. 3]. Con la definizione dell'eq. 6, positiva vuol dire che nell'intervallo i MM hanno preso
delta nello stesso verso del rendimento, quindi che i quattro gruppi insieme hanno scambiato delta **contro**
il movimento dei 10 minuti (vendono delta sui rialzi, lo comprano sui ribassi). Se fosse cosi', un delta
cumulato dei gruppi che va contro il prezzo sarebbe la norma e non un segnale. Va verificato sui dati, non
preso dal paper.

## Cosa Non Regge O Non Si Dice

1. **Non revisionato**, versione del luglio 2026 che fonde due lavori; i numeri possono cambiare.
2. **Nessun test del flusso per gruppo**, ne' sul prezzo ne' sulla volatilita'. Il paper e' sulla gamma.
3. **Il market maker e' un residuo.** E' il negativo dei quattro gruppi: firm proprietari e broker-dealer
   sono trattati come utenti finali, e ogni contratto che i quattro gruppi comprano in netto lo vende un
   market maker per ipotesi. L'appendice A1 elenca "market makers" fra le capacita' presenti nei dati, ma
   la §3.4 dice che i tipi sono quattro: non si capisce se il dato del MM esisteva e non e' stato usato.
4. **Le posizioni partono dal 2018.** Il NOI si somma "dalla nascita del contratto", ma i dati a 10
   minuti iniziano a gennaio 2018: le posizioni in opzioni lunghe aperte prima mancano all'inizio del
   campione, a meno che servano i dati giornalieri 2012-2017 (App. A1), cosa che il paper non dice. Non e'
   discusso.
5. **Solo C1 e solo la cassa** (9:30-16:00): niente flusso notturno, che OptionsDepth invece copre.
6. **Istantanee a 10 minuti**: gli autori dicono che e' la frequenza piu' alta possibile con i loro dati
   [§2.3]. Il DEX di OptionsDepth e' al minuto: la sua fonte esatta non e' nel paper.
7. **La significativita' e' per intervallo**, Newey-West a 5 ritardi dentro la giornata, con FE della data.
   Gli errori **non sono raggruppati per giornata**: le prove alternative raggruppano per Anno x Ora
   [Tab. IA3]. Il nostro metodo chiede la significativita' per giornata. **Nessun modello nullo**, nessun
   fuori campione, nessun costo.
8. **Quanti giorni identificano l'effetto 0DTE**: i martedi' e giovedi' d'eccezione; il numero totale non
   e' dato (11 nel 2021, nota 7). Con cosi' pochi giorni trattati, una t di -2,99 calcolata su 42.831
   intervalli va presa con cautela.
9. **La causalita'** del 2SLS regge solo se gli 0DTE agiscono sulla volatilita' **solo** attraverso la
   gamma dei MM: e' un'ipotesi, che gli autori dichiarano [§3.9].
10. **Il collegamento con gli E-mini e' indiretto**: non si sa chi manda gli ordini sugli E-mini [§3.11,
    §4.3]; altri operatori possono produrre la stessa inversione.
11. **La misura del "momento"** [Tab. 7]: e' la differenza fra il rendimento futuro (da t a t+n) e quello
    passato (da t-n-1 a t-1), non il rendimento di una strategia che segue il verso passato. Con gli effetti
    fissi della data, cosa voglia dire "momento positivo" non e' ovvio dal testo; i numeri si leggono come
    "con gamma alta il rendimento dopo e' piu' basso di quello prima" (gli autori: "rendimenti di inversione
    positivi"), non come il guadagno di una strategia.
12. **Le scadenze lunghe dipendono dalle greche**: oltre 5 giorni -1,096 (t -1,74) con le greche Cboe,
    -4,406 (t -7,00) con il VIX [Tab. 5, IA7]. Non commentato.
13. **Piccole incoerenze di testo**: il testo alternativo (non la didascalia) di Fig. IA2 dice la
    distribuzione della gamma "centrata vicino a zero", il testo dice positiva nell'88% dei casi; la Fig. 1 usa OPRA ma dice "investitori
    diversi dai market maker", che OPRA non distingue; le unita' cambiano fra tabelle (decimali in Tab. 1, 4
    e 6, percentuali in Tab. 3 e 5); la Tab. 1 dice la volatilita' "in percentuale annualizzata" ma il suo
    -0,00613 vale 61 punti base solo se e' in decimali; la colonna (1) della Tab. 5 non ha gli effetti fissi
    della data che il §4.1 dichiara "in ogni specifica".
14. **Molte specifiche** (effetti fissi, sottocampioni, scadenze, ore, orizzonti) senza correzione per test
    multipli; la t centrale della gamma (-13) e' pero' lontana da qualsiasi soglia.
15. **La parte dentro la giornata non e' causale**, lo dicono gli autori [§1]: le relazioni del §4 sono
    predittive. E il risultato **non si estende direttamente ad altri mercati** [§1]: per NQ e NDX il paper
    non dice niente.
16. **Nessuna istruzione** trovata nel PDF; le "Alternative text" sotto le figure sono descrizioni per
    l'accessibilita'.

## Cosa Si Puo' Rifare Qui

Su **NQ** (CLAUDE.md, decisione 2) il paper non si rifa': non abbiamo il Cboe Open-Close di NDX ne' la
storia della gamma dei MM di NQ (la catena CME di ATAS registra solo da quando e' sul chart). Su **ES**,
ammesso per lo studio delle opzioni, X-Ray porta da OptionsDepth il DEX per gruppo al minuto e la GEX dei
MM per strike dal 31 agosto 2026 (vedi [`x-ray-in-vivo`](../../../metodo/x-ray-in-vivo.md)).

**Con le sole barre di ATAS, anche su NQ** (nessun dato di opzioni):

- **L'inversione del flusso dopo un movimento** [eq. 11 senza la gamma]: Order Imbalance a 10 minuti dal
  delta di ATAS (volume aggressivo in acquisto meno in vendita, diviso per il volume) sul rendimento dei
  10 minuti prima. E' il coefficiente b1, che nel paper vale -5,824 (t -18,93) su ES. Attenzione: su NQ
  il fuori campione e' dal 2025, e ogni sguardo ai dati 2025-2026 lo consuma (regola del framework).

**Con X-Ray su ES, registrando in avanti:**

- **Il controllo che il riquadro e' coerente**: il delta scambiato dai quattro gruppi, sommato e con il
  segno cambiato, e' il delta preso dai MM (eq. 9). Si puo' confrontare con il segno della GEX dei MM di
  X-Ray: OptionsDepth probabilmente costruisce i MM nello stesso modo (da verificare).
- **La gamma dei MM e la volatilita' dei 10 minuti dopo** [eq. 10] su ES, con la GEX 0DTE di X-Ray al
  posto della Net Gamma. Nostro calcolo, non del paper: se la t scalasse con la radice del numero di
  intervalli, da 64.800 intervalli a circa 2.000 (50 sedute) la t di -13 diventerebbe circa -2,3; ma per
  giornata, come chiede il nostro metodo, servono molte piu' sedute.
- **Il delta cumulato di ogni gruppo contro il rendimento dei 10-60 minuti dopo**: e' la prova che il
  paper non fa. Con poche settimane di storia e' una descrizione, non una prova.

**Per il riquadro "Chi ha comprato delta oggi"**, cosa ne viene (descrizione, non segnale):

1. Chiamare il gruppo `nonprocust` **clienti (non professionali)**, non "retail": per il paper la
   categoria comprende anche gli istituzionali [§3.4].
2. Mostrare accanto ai quattro gruppi **la loro somma con il segno cambiato**, cioe' il delta preso dai
   market maker, e dire che nel paper quel flusso **non predice la volatilita'** [Tab. 5, IA5].
3. Normalizzare in modo confrontabile fra giorni: in **percentuale dell'S&P 500** (azioni equivalenti
   divise per le azioni dell'indice) o come **quota del volume lordo** del gruppo, fra -1 e +1; e dire
   quanto e' grande in **deviazioni standard alla stessa ora** su una finestra all'indietro.
4. Dire che **il flusso di oggi e' la parte minore**: la gamma che frena il mercato viene da posizioni
   aperte nei giorni prima [§3.6], e quella e' nella GEX dei MM, non nel DEX.
5. Nessuna lettura "chi ha ragione": il paper non la sostiene per nessun gruppo.

## Cosa Ne Prende Il Framework

- **Il metodo per costruire i MM** dal flusso per gruppo (eq. 7-9) e le due misure di copertura, la
  gamma dallo stock e il delta dal flusso (eq. 3-6), con la normalizzazione su M.
- **La prova di meccanismo** su ES: gamma dei MM alta, volatilita' piu' bassa e flusso piu' contrario
  nell'ora dopo; e le soglie a quartili su una finestra all'indietro di 180 giorni, che non guarda avanti.
- **Un risultato negativo da registrare**: il delta scambiato dai gruppi, sommato e diviso per il
  rendimento dell'intervallo, non predice la volatilita' a 10 minuti [Tab. 5, con e senza effetti fissi
  della data].
- **Nessuna regola operativa**: niente costi, niente fuori campione, significativita' per intervallo e
  non per giornata; e gli autori stessi dicono che non e' una strategia [§4.4].

## I Dubbi Di Lettura

Per il verificatore, perche' il PDF non e' stato visto come immagine. Le pagine sono quelle stampate
sul foglio; nel file PDF sono una in piu' (il frontespizio non e' numerato), e l'Internet Appendix
riparte da 1 alla pagina 55 del file.

- **Il segno delle formule (2)-(9)**: ricostruito dalla sostituzione dei simboli (il meno esce come una
  freccia, e la stessa freccia compare fra i termini dell'eq. 7 e nell'eq. 9). E' coerente con il testo
  ("se dominano le posizioni corte, la Net Gamma e' negativa"), ma va guardato sulle pagine 13-17.
- **Tab. 5, colonna (1)**: la riga "Date" ha quattro "Yes" su cinque colonne e la colonna (1) ha un R
  quadro interno di 0,330 contro circa 0,117 delle altre: sembra **senza** gli effetti fissi della data,
  che il testo dichiara. Da guardare sulla pagina 49.
- **Tab. 3**: le righe si sono rimesse in ordine con `-raw`; i valori del sottocampione "solo giorni con
  0DTE prima di maggio 2022" (stale 0,293, new 0,083) e quelli dopo maggio 2022 sono da ricontrollare.
- **Tab. 4**: con `-layout` le colonne si mescolano; con `-raw` l'ordine e' 0DTE, Stale, New, non-0DTE,
  totale, 2SLS (+0,00275, +0,00247, +0,00029, -0,00137, +0,00153, -4,0370).
- **Tab. IA5**: il coefficiente non-0DTE e' "1,09 x 10^-5" (t 0,0101), letto da una notazione esponenziale
  storpiata.
- **Tab. 7 e IA11**: la formula del "momento" (R da t a t+n meno R da t-n-1 a t-1) e' letta da una
  didascalia con i simboli sostituiti.

**Come sono stati sciolti** (verifica del 9 novembre 2026, con `pdftotext -table`, che tiene le colonne
al loro posto, e `-layout -enc UTF-8`; il PDF come immagine non si e' potuto aprire nemmeno qui):

- **Segni delle eq. (2)-(9)**: confermati. Nel font delle formule il meno della riga esce come `↑`, il
  meno dei pedici come `→` (t→1 = t-1), e `→` fra due membri e' il "circa uguale". Letti cosi': (2)
  -w_t D_t - (-w_{t-1} D_{t-1}) = -w_{t-1}(D_t - D_{t-1}) - (w_t - w_{t-1}) D_t; (3) n = -(w_t - w_{t-1}) D_t;
  (4) q = -w_{t-1}(D_t - D_{t-1}) circa -w_{t-1} G dS; (5) Net Gamma = -100 somma q / M / (dS/S) circa
  (100 S / M) somma w_{t-1} G; (6) Net Delta = -100 somma n / M / (dS/S) circa 100/(M R_t) somma (w_t - w_{t-1}) D;
  (7) OB + CB - OS - CS; (9) OIB e NOI dei MM = meno la somma sui gruppi. Il testo li conferma ("we multiply
  q by -100", "the negative values of the sums"). Le formule dell'analisi sono giuste.
- **Tab. 5, colonna (1)**: confermato, **senza effetti fissi della data** (in `-table` la riga "Date" ha
  "Yes" solo nelle colonne 2-5). Corretti i risultati 12 e 13 e i punti che li citano.
- **Tab. 3**: confermata riga per riga, compreso il sottocampione "solo giorni con 0DTE" (stale 0,293,
  new 0,083) e il dopo maggio 2022.
- **Tab. 4**: confermato l'ordine 0DTE, Stale, New, non-0DTE, totale, 2SLS, coerente con il testo §3.9.
- **Tab. IA5**: confermato 1,09 x 10^-5 (t 0,0101): `↓` e' il segno per, `→` il meno dell'esponente.
- **Tab. 7 e IA11**: confermata la formula R(t, t+n) - R(t-n-1, t-1), anche dal testo del §4.4, che dice
  di omettere R(t-1, t).

## La Verifica

**9 novembre 2026**, fase 4, verificatore diverso dal lettore. PDF riaperto (sha256
`6b256ade06bd64282dd8cfd4855521dfb1b0f404b687c0191c0edd61d73791ea`, uguale alla scheda; 75 pagine).
**Controllate 206 affermazioni** (ogni numero delle 22 righe dei risultati contato uno per uno, piu'
definizioni, citazioni di sezione, metodo e limiti): **196 confermate**, **10 corrette o precisate**,
**0 tolte**. Nessuna istruzione nel PDF.

I tre punti chiesti dall'orchestratore:

- **Il delta scambiato non predice la volatilita' a 10 minuti**: confermato il numero (Net Delta -0,0015,
  t -0,6116, Tab. 5), ma viene dalla colonna (1), che **non ha gli effetti fissi della data**. Il finding
  regge lo stesso: con la data, separato in 0DTE e non-0DTE, t fra -1,20 e 0,95 (colonne 2-5), e con le
  greche dal VIX t fra -1,09 e 0,80 (Tab. IA7). La misura e' pero' il delta **diviso per il rendimento**.
- **La gamma, uno stock, la riduce per circa un'ora**: confermato. 0DTE significativa all'1% fino a t+6
  (-0,2261, t -2,74), al 10% a t+7 (t -1,90), poi zero (Tab. IA9, con la data); somma della prima ora -2,0
  (§4.2). La didascalia di Tab. IA5 dice in parole sue che la Net Delta "e' un flusso" (stock contro flusso).
- **I Customers (§3.4, p. 16)**: confermato alla lettera, "public investors who represent the primary
  end-users of options, including both retail and institutional investors".

Le correzioni:

1. **Tab. 5, colonna (1)** senza effetti fissi della data, contro quanto dice il §4.1: aggiunto nei
   risultati 12 e 13, nel metodo, in "Cosa dice" punto 2, nei limiti (13) e in "Cosa ne prende il
   framework". E' la piu' grave: la t -13,16 della gamma e la t -0,61 del delta citate come centrali
   vengono dalla specifica meno controllata.
2. **Nota 11**: "meno dell'1%" sono i casi con |R_t| sotto la soglia, non i dati mancanti.
3. **Risultato 14**: i coefficienti dal VIX venivano da due colonne diverse della Tab. IA7; separati, e
   aggiunta la non-0DTE (-1,184, t -5,60).
4. **Risultato 16**: aggiunta la speed, significativa e negativa (0DTE t -2,19; non-0DTE t -3,64), che
   l'analisi taceva.
5. **Risultato 22**: aggiunto il gamma scalping "redditizio se i costi sono bassi" (§4, nota 15).
6. **Risultato 7**: le Fig. 3 e 4 coprono solo 19 maggio 2022 - dicembre 2024.
7. **Limite 13**: "centrata vicino a zero" sta nel testo alternativo di Fig. IA2, non nella didascalia;
   aggiunta l'incoerenza delle unita' della Tab. 1.
8. **Limite 11**: frase finale storpiata ("non di piu'") riscritta con le parole degli autori.
9. **Limiti mancanti** aggiunti (15): il §4 non e' causale e il risultato non si estende ad altri
   mercati, dichiarati dagli autori nel §1; e nel limite 4, i dati giornalieri 2012-2017 che il paper non
   dice se usa per le posizioni iniziali.
10. **"In parole semplici" e "Cosa dice" punto 2**: il delta che non predice e' il delta diviso per il
    rendimento; aggiunto. Scheda: 75 pagine contano anche la copertina dell'Internet Appendix.

Non verificabile dal PDF: l'identificativo SSRN 5641974 e l'URL (vengono dalla ricerca, fase 1); le
affermazioni su OptionsDepth e X-Ray, che sono del repository e non del paper. Resta incerto, senza
l'immagine delle pagine: la lettura delle figure (3-9, IA2-IA6), descritte solo dal testo e dalle
didascalie.
