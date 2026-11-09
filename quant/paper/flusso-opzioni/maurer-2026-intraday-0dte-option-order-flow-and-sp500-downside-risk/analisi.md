---
chiave: maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk
sigla: MAMU
titolo: "Intraday 0DTE Option Order Flow and S&P 500 Downside Risk"
autori: [Maceo Maurer, Mirko Muller]
versione_letta: "versione dell'agosto 2026 (SSRN 7339718); dal frontespizio, viene dalla tesi di master degli autori alla Frankfurt School of Finance & Management, consegnata nell'agosto 2026, relatori Grigory Vilkov e Paula Cocoma"
fonte: SSRN (working paper tratto da una tesi di master)
identificativo: "10.2139/ssrn.7339718"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7339718
tipo: preprint (tesi di master)
revisione_fra_pari: no
pagine: 83 (frontespizio con l'abstract, poi 82 pagine numerate; appendice alle pagine 76-82)
pdf: maurer-2026-intraday-0dte-option-order-flow-and-sp500-downside-risk.pdf
pdf_sha256: b140ae464b89c930769af21eefd8b41ce06588c3916daac65562a4fdb06cd71d
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno pubblicato; la specifica, dicono, e' 'scritta nel codice dell'analisi' (§4.4.1). Dati ThetaData Options Standard (a pagamento), SOFR, VIX; la fonte dei livelli dell'S&P 500 al minuto e del VIX al minuto non e' dichiarata"
---

# Maurer, Muller 2026: L'Analisi

*Intraday 0DTE Option Order Flow and S&P 500 Downside Risk*, Maceo Maurer e Mirko Muller (Frankfurt School
of Finance & Management), versione dell'agosto 2026, SSRN 7339718. **Non e' revisionato, e viene da una tesi
di master**: lo dice la nota del frontespizio ("This paper is based on the authors' master's thesis at
Frankfurt School of Finance & Management, submitted in August 2026"), con i ringraziamenti ai relatori
Grigory Vilkov e Paula Cocoma. Vilkov e' coautore di Adams e altri (ADEFOV, gia' analizzato) e autore di un
paper sulle regole di trading 0DTE che MAMU cita spesso (Vilkov 2026). Letto per intero il 9 novembre 2026,
dalla prima pagina all'ultima dell'appendice.

**Come e' stato letto.** Testo estratto con `pdftotext -layout`; le tabelle che nel layout si mescolano
(Tab. 5, 7, 13, 17, 18, 19, 23, A.1, A.3, A.4) sono state rilette pagina per pagina con `pdftotext -raw`.
Le pagine **non** si sono potute vedere come immagine (`pdftoppm` manca): le figure si conoscono solo dalle
didascalie, e le formule, che escono storpiate, sono **ricostruite** dalle definizioni a parole che le
accompagnano (lo diciamo accanto a ciascuna). **In verifica** le pagine sono state rese come immagine con
`pypdfium2`: le eq. 1-4, 6 e 14, le Tab. 5, 7, 13, 17, 18, 19 e la Fig. 1 sono state guardate sul PDF
(vedi La Verifica). Nella numerazione, la pagina stampata n e' la pagina n+1 del
PDF. Citazioni `[MAMU §n]`, `[MAMU Tab. n]`, `[MAMU Tab. A.n]` (appendice).

**In parole semplici.** Gli autori guardano le opzioni su SPX che scadono in giornata (0DTE) nei **primi
cinque minuti** dopo l'apertura delle 9:30 di New York, e contano quante put sono state scambiate rispetto
alle call, dando piu' peso ai contratti vicini al prezzo (pesati per la gamma). **Non sanno chi ha comprato e
chi ha venduto**, ne' per conto di chi: e' volume, non il flusso di un gruppo. Quando la mattina pende verso
le put, **l'ora dopo** l'indice va un po' peggio (circa 3,5 punti base per una deviazione standard del
rapporto) e, soprattutto, e' un po' piu' probabile che l'ora chiuda in calo (circa 5 punti percentuali in
piu', su una base di circa 47%). Non dice **quanto** scendera', e sulle cadute grandi non dice niente. Dopo il
primo quarto d'ora non c'e' piu' niente. E **fuori campione non serve**: la regola costruita sul segnale non
guadagna in modo distinguibile da zero neanche senza costi, e una previsione della probabilita' di calo che lo
usa non e' migliore di una che lo ignora. Un'altra cosa conta per il nostro riquadro: **le misure con il
verso (chi ha comprato) e quelle pesate per il delta non predicono niente**.

## I Termini

- **0DTE** (*zero days to expiration*): opzioni che scadono il giorno stesso. Qui le SPXW, le settimanali di
  SPX, europee e regolate in contanti alla chiusura delle 16:00 [§4.1].
- **PCR** (*put-call ratio*, rapporto put-call): volume delle put diviso volume delle call. Qui e' in
  logaritmo, cosi' zero vuol dire bilanciato, positivo piu' put, negativo piu' call [§4.2].
- **PCR pesato per la gamma**: ogni contratto conta in proporzione alla sua gamma. La gamma dice quanto cambia
  il delta (quante unita' di sottostante equivale un'opzione) quando il prezzo si muove; e' grande vicino al
  prezzo e quasi nulla lontano. Una put e una call allo stesso strike hanno la **stessa gamma**: il peso
  **non** sposta il rapporto fra put e call allo stesso strike, sposta il peso **lungo gli strike**, verso il
  denaro. Gli autori lo chiamano un filtro di moneyness (di vicinanza al prezzo) [§3.2, §5.1].
- **Volume non firmato** (*unsigned*): il volume senza sapere se ha comprato chi ha preso l'iniziativa o chi
  ha venduto. Il PCR principale e' non firmato [§2.1, §4.2].
- **Volume firmato** (*signed*): ogni scambio classificato come acquisto o vendita con la **regola della
  quotazione** di Lee e Ready (sopra il punto medio fra denaro e lettera e' un acquisto, sotto una vendita;
  al punto medio decide il prezzo precedente, il *tick test*) [§4.2, nota 2].
- **Flusso e stock**: il flusso e' cio' che si scambia oggi; lo stock e' l'open interest (OI), le posizioni
  aperte alla chiusura di ieri [§4.2].
- **Margine estensivo e intensivo**: il primo e' **se** l'ora scende (si' o no); il secondo e' **quanto**
  scende, dato che scende. Il modello a due parti (*hurdle*, di Cragg) li stima separatamente [§4.4.3].
- **HAC, Newey-West**: errori standard corretti per l'eteroschedasticita' e per la dipendenza fra giornate
  vicine [§4.4.2].
- **Holm-Bonferroni, Benjamini-Hochberg**: correzioni per prove multiple; la prima controlla la probabilita'
  di anche un solo falso positivo nella famiglia, la seconda la quota attesa di falsi positivi [§4.4.4].
- **Walk-forward**: si stima su una finestra di un anno e si prova sul mese dopo, poi si scorre; ogni
  rendimento di prova e' fuori campione [§4.4.5].
- **AUC** (*area under the curve*): quanto una previsione ordina bene i giorni di calo prima di quelli
  senza; 0,5 e' il caso. I test di **DeLong** e di **Clark e West** dicono se un AUC o un errore di previsione
  e' migliore di un altro [§4.4.5].
- **PBO e Sharpe deflazionato**: la probabilita' di sovra-adattamento del backtest (Bailey e altri) e lo
  Sharpe corretto per il numero di configurazioni provate (Bailey e Lopez de Prado) [§4.4.5].

## La Domanda

Il flusso delle opzioni 0DTE su SPX nei primi minuti della seduta, pesato dove si concentra la pressione di
copertura dei market maker, avvisa di un calo **nell'ora che segue**? E se si', avvisa che il calo
**avverra'** o anche di **quanto** sara'? E l'informazione e' **usabile**, per fare trading o per gestire il
rischio? [§2.4, §3.3]

Sei ipotesi [§3.3]: H1 un'apertura sbilanciata sulle put predice un rendimento dell'ora piu' basso; H2 una
probabilita' piu' alta di un'ora in calo; H3 **non** predice quanto e' grande il calo (la nulla e' dichiarata
come attesa); H4 l'effetto sta nei primi minuti e sparisce; H5 la volatilita' alta lo rafforza
(esplorativa); H6 il segnale non diventa sfruttabile (fuori campione). **L'insieme confermativo e' H1-H4**,
fissato prima delle stime; la correzione per prove multiple tratta **H1-H3 come una famiglia**, mentre H4 resta
confermativa ma fuori dalla correzione e si giudica dalla forma; H5 e' esplorativa e H6 una prova fuori
campione, fuori dall'insieme confermativo [§3.3, §4.4.1, §4.4.4].

## Il Metodo

**I dati** [§4.1, Tab. 1]: tutte le SPXW che scadono in giornata, a **1 minuto**, da ThetaData (Options
Standard): barre OHLC, stampe degli scambi, volatilita' implicita, OI giornaliero. **1.027 sedute, dal 19
maggio 2022 al 24 giugno 2026**, cioe' da quando ci sono scadenze ogni giorno della settimana; tolta una sola
seduta (il 25 giugno 2026, senza file di opzioni). 103.171.215 righe di opzioni al minuto. Solo la seduta
regolare 9:30-16:00 di New York. Nessun filtro di volume o di moneyness.

**La gamma** [§4.2, eq. 2]: ricalcolata con Black-Scholes dalla volatilita' implicita del fornitore, con il
SOFR come tasso, senza dividendi, e con il **tempo alla scadenza in tempo di contrattazione**: (390 - m) minuti
diviso 252 x 6,5 x 60. Ricostruita dalle definizioni, e confermata in verifica sul PDF reso come immagine:

```text
gamma(i,m) = phi(d1) / ( S(m) * sigma(i,m) * sqrt(tau(m)) )
d1         = ( ln(S(m)/K(i)) + (r + sigma(i,m)^2 / 2) * tau(m) ) / ( sigma(i,m) * sqrt(tau(m)) )
```

Filtri sulla volatilita' implicita: via le letture con errore del fornitore sopra 0,5 (11.452, lo 0,006%) e
quelle fuori dall'intervallo 0-5 (52,0 milioni, il 29,1%, quasi tutte ali lontane); restano 126,9 milioni di
letture su 179,0. Sugli scambi: via le opzioni con denaro zero e con lettera/denaro oltre 10. Un contratto
filtrato resta nel volume e conta zero solo nel peso della gamma [§4.2].

**Le misure del flusso** [§4.2, eq. 1, 3, 4, Tab. 2], tutte cumulate dall'apertura (m = 0) al minuto della
lettura h. Ricostruite dalle definizioni, e confermate in verifica sul PDF (anche il meno davanti all'OTS e
il segno del tipo d'opzione dentro SDV, che nel paper si chiama epsilon; la Tab. 2 lo omette nel peso di
SDV, l'eq. 3 lo ha):

```text
PCR_w(h) = ln( somma_{put, m<=h} w(i,m) v(i,m) / somma_{call, m<=h} w(i,m) v(i,m) )      (1)
           w = 1 (volume), |delta| del fornitore (delta), gamma (gamma: la misura principale)
SDV(h)   = somma_{scambi j fino a h} eta(j) * s(j) * p(j) * q(j) * 100                   (3)
           s = +1 sopra il punto medio, -1 sotto; eta = +1 call, -1 put
OTS(h)   = - somma_j s(j) eta(j) w(j) q(j) / somma_j w(j) q(j)                           (4)
           il punteggio firmato di Bergsma e altri, fra -1 e +1, positivo = ribassista
```

Il PCR pesato per il delta ha un livello spostato per costruzione (i due delta assoluti a uno strike sommano a
uno): positivo in 1.025 giornate su 1.027 [§5.1]. Il flusso firmato in dollari (SDV) e' al contrario dei
rapporti: sale quando dominano gli acquisti di call [§4.2].

**Le misure dello stock** [§4.2, eq. 5, 6, Tab. 3], fuori dall'insieme dichiarato prima: il PCR dell'OI
pesato per la gamma, il PCR dell'OI non pesato, e la **concentrazione**: la quota della gamma dell'OI entro lo
0,5% dal prezzo dell'indice.

**Le letture**: 5, 10, 15, 30, 60, 120, 180, 240, 300 minuti dopo l'apertura; **la principale e' 5 minuti**,
scelta prima delle stime come il primo momento in cui l'asta d'apertura e' passata [§3.3, §4.1]. 9.243
osservazioni giorno per lettura; le prove confermative usano le 1.027 a 5 minuti [Tab. 1].

**Gli esiti** [§4.3, eq. 7-10, Tab. 4], tutti sui **60 minuti dopo la lettura** (accorciati se superano la
fine della seduta): il rendimento logaritmico dell'S&P 500 r = ln(S(h+60)/S(h)); l'ampiezza |r|; il calo
(|r| se r < 0, altrimenti 0); l'indicatore di calo 1{r < 0}.

**Le stime** [§4.4]: una regressione per esito, **un'osservazione per giornata**. Controlli: il VIX alla
lettura, il rendimento dall'apertura alla lettura, i giorni della settimana. Errori Newey-West con 6 ritardi
(regola di Newey e West 1994 per T = 1.027) e correzione per piccoli campioni. Il calo con il modello a due
parti: logit con errori HAC per il **se** (tutte le giornate), minimi quadrati con errori HC3 per il
**quanto** (solo le giornate in calo). Poi la scomposizione della risposta del calo atteso in un canale di
occorrenza e uno di profondita' (eq. 14), con intervalli da bootstrap a blocchi circolari.

**La pre-specificazione** [§4.4.1]: misura (PCR pesato per la gamma), lettura (5 minuti), finestra (60
minuti), controlli, ipotesi e regola di trading con il calendario walk-forward "scritti nel codice
dell'analisi e lasciati invariati". Le prove aggiunte dopo aver visto i risultati sono dichiarate come tali
e corrette in una famiglia a parte (§5.9, 11 prove).

**Il fuori campione** [§4.4.5]: walk-forward con finestra mobile di un anno e prova sul mese dopo, **756
giornate** fuori campione. La regola: lunghi l'ora dopo la lettura nel quinto di giornate piu' sbilanciate
sulle call, corti nel quinto piu' sbilanciato sulle put [§4.4.1]. Costi andata e ritorno da 0 a 50 punti
base; **1 punto base** e' il costo dichiarato su ES o SPY, 10-50 quello delle opzioni 0DTE [§4.4.5] (la
didascalia della Tab. 12 dice invece "da cinque in su"). Accanto, la previsione della probabilita' di calo
(logit su un anno mobile, applicato al mese dopo) contro quella con i soli controlli, con AUC, DeLong, Clark e
West, e la scomposizione di Murphy [§4.4.5, §5.7]. **Questa seconda prova non e' pre-specificata**: "was
specified after the trading result was in", con lo stesso calendario walk-forward [§5.7].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | Il PCR pesato per la gamma ha media -0,196 e deviazione standard 0,270; il PCR non pesato media +0,028. Correlazione fra i due 0,837; il peso della gamma sposta il livello di -0,223 punti logaritmici (-0,83 deviazioni standard) e porta le mattine sbilanciate sulle put dal 54,5% al 21,4%. Lettura degli autori: le put scambiate stanno piu' lontane dal denaro delle call | §5.1, Tab. 5, Tab. 6 |
| 2 | L'ora dopo scende nel **46,9%** delle sedute; il calo medio e' 0,14% su tutte e circa 0,30% sulle giornate in calo. Il rendimento dell'ora ha deviazione standard 0,393% e curtosi 5,52 | §5.1, Tab. 5 |
| 3 | **H1, il verso**: coefficiente -0,0013 (errore HAC 0,0005), **t -2,65, p 0,008**, R quadro 2,64% contro 2,0% dei soli controlli. Una deviazione standard del rapporto sposta il rendimento dell'ora di **circa -3,5 punti base** (-3,46 in Tab. 16), intervallo al 95% circa da 1 a 6, contro una deviazione standard dell'ora di circa 40 | §5.2, Tab. 7 |
| 4 | Le altre misure, a 5 minuti, sullo stesso rendimento: PCR non pesato -0,0012 (t -1,90, p 0,058); **PCR pesato per il delta +0,0000 (t +0,07, p 0,947)**; **flusso firmato in dollari -0,0000 (t -0,11, p 0,909)**. Mostrate "per trasparenza", non per inferenza | §5.2, Tab. 7 |
| 5 | Per decili del rapporto, il rendimento medio scende dalle call alle put; lo guidano gli estremi, i decili centrali sono piatti (cosi' il testo). **Sulla figura** (vista in verifica): il decile 1, il piu' sbilanciato sulle call, sta a circa +11 punti base, il decile 10 a circa -7; i decili 2-9 oscillano senza ordine fra circa -3 e +4. La pendenza e' fatta quasi tutta dai due decili estremi, il primo piu' del decimo | §5.2, Fig. 1 |
| 6 | **H4, il decadimento**: t -2,65 a 5 minuti, -2,71 a 10, -2,23 a 15; poi -0,55 a 30, +1,08 a 60, +1,46 a 120, -0,51 a 180, -0,16 a 240, -0,20 a 300. Il contrasto primi 15 minuti contro dopo: -0,0014, t -2,42, p 0,015 (specificato dopo aver visto i risultati); i sei coefficienti dopo sono insieme indistinguibili da zero. Misurato **fino alla chiusura** invece che a un'ora, niente: t -0,41 | §5.3, Tab. 8; §5.6 |
| 7 | **H2, se l'ora scende**: logit **+0,8032, t 3,22, p 0,001**. Una deviazione standard del rapporto alza le probabilita' relative (odds) di circa un quarto, **circa 5 punti percentuali** su una base poco sotto la meta' | §5.4, Tab. 9, Tab. A.6 |
| 8 | **H3, quanto scende**: +0,0006, t 1,16, p 0,247 (HC3; HAC t 1,10). L'ampiezza |r|: +0,0005, t 1,40, p 0,161: il rapporto **non** dice quanto si muovera' l'ora | §5.4, Tab. 9, Tab. A.6 |
| 9 | **La scomposizione**: canale di occorrenza +1,59 punti base per deviazione standard [+0,59, +2,58]; canale di profondita' +0,70 [-0,46, +1,85]; totale +2,29; controllo indipendente con la regressione censurata +2,34 [+0,87, +3,82]. Circa sette decimi (69,5%) passano dall'occorrenza | §5.4, Tab. 10 |
| 10 | **La potenza sul quanto**: 482 giornate in calo; effetto stimato +1,49 punti base per deviazione standard (errore 1,29); calo medio 30,08 punti base; effetto minimo rilevabile all'80% di potenza **+3,60** (12,0% del calo medio); potenza contro l'effetto stimato 0,212; servirebbero 2.822 giornate in calo, cioe' **6.012 sedute**. Il test di equivalenza (TOST) da' p 0,543: l'effetto ne' c'e' ne' e' escluso | §5.4, Tab. A.4 |
| 11 | **Il quanto per quantili** (dopo aver visto i risultati): sulle cadute leggere positivo (quantile 0,25: +2,16, p 0,013, Holm p 0,051), al quantile 0,75 +0,16 (p 0,929), al 0,90 -1,30 (p 0,714). Correlazione fra effetto e quantile -0,92: **sulle cadute gravi niente** | §5.4, Tab. A.5 |
| 12 | **Le quattro misure sul se**: solo il PCR pesato per la gamma (p 0,0013) e quello non pesato (+0,8942, p 0,0059) predicono se l'ora scende; delta (p 0,217) e flusso firmato (p 0,320) no; nessuna predice quanto. A 15 minuti la gamma e' al confine (p 0,050), il volume oltre (p 0,130) | §5.4, Tab. A.8, Tab. A.1 |
| 13 | **H5, la volatilita'** (esplorativa, fuori correzione): con il VIX d'apertura sopra la mediana (17,56) l'interazione e' -0,0017, p 0,034; per il PCR non pesato p 0,236 | §5.5, Tab. 11 |
| 14 | **La correzione per prove multiple** (H1-H3): H1 p 0,0081, Holm 0,0163, BH 0,0122, regge; H2 p 0,0013, Holm 0,0038, regge; H3 p 0,2475, non regge | §5.8, Tab. 14 |
| 15 | **Le due meta'**: 2022-2023 (406 sedute) verso -0,0027 p 0,008, logit p 0,029; 2024-2026 (621 sedute) **verso -0,0006 p 0,205**, logit p 0,037. Il verso sta nella prima meta', piu' volatile; il se regge in entrambe | §5.8, Tab. 15 |
| 16 | **Robustezza**: Huber t -3,09 sul verso e 3,71 sul calo censurato; permutazione a blocchi alla Freedman-Lane sul calo censurato p 0,001 (blocco scelto di lunghezza uno); ritardi HAC da 6 a 20 stabili (verso t fra -2,65 e -2,73) | §5.8, Tab. A.10 |
| 17 | **Non e' la mossa d'apertura**: il rendimento dei primi cinque minuti spiega il 14,6% del rapporto; tolta quella parte (e tolto il controllo), -3,16 punti base per deviazione standard, t -2,61, p 0,009 | §5.8, Tab. 16 |
| 18 | **Non sono i giorni di dati macro** (FOMC, CPI, payroll, 129 giornate): senza quelle, -3,67 (898 sedute, p 0,008); solo quelle, -0,89 (p 0,838, troppo poche); interazione p 0,856 | §5.8, Tab. 17 |
| 19 | **Non e' la notte**: con gap notturno, variazione del VIX e volatilita' realizzata di 5 giorni, -3,17 (p 0,027), logit p 0,0065; nessuna delle tre variabili significativa. **Placebo**: il rapporto di ieri alla stessa ora +0,50, p 0,719 (logit p 0,676) | §5.8, Tab. 18 |
| 20 | **Flusso, non stock**: il PCR dell'OI pesato per la gamma da solo non predice (p 0,238 verso, 0,498 se, 0,159 quanto); insieme al flusso, il flusso resta -0,0012 (t -2,11, p 0,035) e nel logit sale a +0,9566 (p 0,001), lo stock -0,0001 (p 0,860). Correlazione fra flusso e stock 0,538 | §5.8, Tab. 19 |
| 21 | **Lo stock dice l'ampiezza, non il verso** (fuori dall'insieme dichiarato): la concentrazione della gamma dell'OI vicino al denaro va con un'ora **meno ampia** (p 0,016 sull'ampiezza; 0,256 sul verso, 0,210 sul se); la differenza fra gamma nozionale di call e di put, p 0,004 sull'ampiezza, l'unica delle 11 prove successive che regge Holm (0,046). Insieme nessuna delle due regge; il VIX vale +12,32 punti base per deviazione standard (t 9,26). **Il paper non e' coerente con se stesso qui**: la Tab. A.11 da' per la stessa concentrazione p 0,005 sull'ampiezza, 0,325 sul verso e 0,327 sul se, non 0,016, 0,256 e 0,210 come il §5.8 e la Tab. 25 | §5.8, §5.9.5, Tab. 25, Tab. 26, Tab. A.11 |
| 22 | **Il flusso firmato non predice niente**: punteggio firmato pesato per la gamma -0,32 punti base, t -0,23, p 0,815 (sul se p 0,173); non pesato t -0,55, pesato per il delta t -0,08, per il premio t +0,34. Insieme al PCR non firmato, questo sale a t -2,70 e il firmato resta a -0,53; correlazione fra i due -0,16; le quattro versioni firmate correlano a 0,94 | §5.9.4, Tab. 23, Tab. 24 |
| 23 | **H6, il trading fuori campione** (756 giornate): Sharpe annuo **+0,05 a 1 punto base** [-0,88, +1,02]; **+0,34 lordo** [-0,60, +1,31]; -0,24 a 2; -1,09 a 5 (intervallo tutto sotto zero da qui in su); -2,49 a 10; -10,08 a 50. La versione adattiva, che riseleziona lettura e soglie, -0,61. Per uno Sharpe di 0,5 servirebbe un effetto circa 2,3 volte quello misurato | §5.6, Tab. 12, Tab. A.9, Tab. 13 |
| 24 | **La ricerca dentro il campione**: 40 configurazioni, circa 4,5 indipendenti; PBO 0,142; Sharpe deflazionato 0,852 (sotto 0,95); 24 celle su 40 positive; la configurazione scelta +0,88 dentro, +0,05 fuori | §5.6, Tab. 13, Fig. A.4 |
| 25 | **La previsione del rischio fuori campione**: AUC 0,531 con il flusso contro 0,520 con i soli controlli; DeLong p 0,233, Clark e West p 0,118. Giusta nel **51,6%** dei giorni contro il **52,5%** di "nessun calo, sempre": peggio di non fare niente | §5.7 |
| 26 | **Soglie di calo piu' severe** (dopo aver visto i risultati): sotto il 25° percentile, dentro il campione p 0,002, fuori guadagno di AUC +0,009 (p 0,278); sotto il 10° p 0,028, +0,004; sotto il 5° **p 0,460**, +0,004. Varianti del protocollo: solo la finestra che si allarga ha Clark e West p 0,028; il testo dice che non regge la correzione della Tab. 26, ma **nella Tab. 26 quella prova non c'e'** (delle varianti del protocollo c'e' solo la regola senza parametri, p 0,695) | §5.9.1, §5.9.2, Tab. 20, Tab. 21, Tab. 26 |
| 27 | **Concentrazione della gamma come condizione** (dopo): il segnale e' piu' debole dove la gamma e' concentrata al denaro (logit interazione -0,451, p 0,041; rendimento +0,0009, p 0,017); nessuna regge la correzione | §5.9.3, Tab. 22 |

**Il finding in due righe.** Il PCR 0DTE pesato per la gamma letto alle 9:35 di New York anticipa un'ora
un po' piu' debole (-3,5 punti base per deviazione standard, t -2,65) e soprattutto una probabilita' di
un'ora in calo piu' alta di circa 5 punti (t 3,22), stabile nelle due meta' del campione; niente sul quanto,
niente dopo il primo quarto d'ora, e niente di usabile fuori campione [§5.2, §5.4, §5.6, §5.7].

## Il Perche'

**Il meccanismo degli autori** [§3.1, §3.2, §6.2]: due canali. Il primo e' l'**informazione**: chi sa
qualcosa usa le opzioni, e le put comprate annunciano un calo (Pan e Poteshman, Easley e altri). Il secondo e'
la **copertura**: i market maker coprono le opzioni scambiate e, dato che la loro gamma netta su SPX e'
positiva quasi sempre (l'88% degli intervalli di 10 minuti in ADEFOV, citato da MAMU), comprano quando scende e
vendono quando sale, **frenando** la mossa. Il primo canale da' il **verso** (un calo diventa piu' probabile),
il secondo **ne limita la profondita'**: per questo il segnale dice "se" e non "quanto". Il peso della gamma
serve a guardare dove la copertura si concentra (vicino al denaro).

**Cosa ammettono loro stessi** [§3.2, §4.2, §6.3]: il peso della gamma **non** identifica quale dei due canali
lavora, perche' la copertura e la scommessa di giornata di chi sa qualcosa si concentrano **tutte e due** vicino
al denaro, dove il peso porta la misura ("both concentrate precisely there", §3.2); che la gamma sia uguale
per put e call allo stesso strike spiega invece perche' il peso non ricava il segno della posizione dei market
maker [§4.2, §6.3]. Il segno della gamma dei market maker non si
ricava dal volume pubblico e lo **prendono dalla letteratura**; il meccanismo vero si proverebbe solo con i
dati per tipo di partecipante (Cboe Open-Close, come ADEFOV). E: "the two findings are one mechanism read two
ways", perche' un rendimento atteso piu' basso alza da solo la probabilita' di un calo; con una distribuzione
normale, 3,5 punti base darebbero circa 3,5 punti percentuali, con code grasse quasi 4, e il logit stima circa
5, entro un errore standard [§1.2, §5.4]. Il "se" e' quindi **lo stesso spostamento** del verso, letto in
probabilita', piu' robusto perche' non dipende dalle code.

**Le alternative scartate**: la sola mossa d'apertura (risultato 17), i giorni macro (18), le notizie della
notte (19), lo stock di posizioni (20), il segno del flusso (22). Perche' il flusso firmato non funziona su un
indice, la loro lettura [§5.9.4]: in Bergsma e altri il punteggio firmato predice dove il sottostante e' caro
da scambiare e chi sa qualcosa e' spinto sulle opzioni; un indice si scambia a costo quasi nullo con ES e SPY,
e quella spinta non c'e'. L'errore di classificazione non basta: un errore di uno su cinque riduce il segnale
di meno della meta', e nessun errore sotto il lancio di moneta porta t da -0,23 a -2,65.

**Cosa lo ucciderebbe** (dedotto dal meccanismo, e in parte detto da loro [§6.3]): (a) un effetto uguale o piu'
forte nei giorni di gamma dei market maker negativa, o piu' debole dove e' piu' positiva (loro prevedono il
contrario: occorrenza piu' forte dove la gamma netta e' piu' positiva); (b) lo stesso rapporto costruito sulle
scadenze lunghe che predice allo stesso modo (allora non e' un fatto degli 0DTE); (c) un controllo per il
movimento dei **future** nei primi cinque minuti che lo assorbe (vedi sotto, punto 3); (d) dati nuovi dopo
giugno 2026 senza effetto sul "se".

## Cosa Non Regge O Non Si Dice

1. **E' una tesi di master, non revisionata.** I relatori sono Vilkov e Cocoma; Vilkov e' coautore di ADEFOV e
   autore di Vilkov 2026, i due lavori su cui il paper appoggia meccanismo e confronti. Non e' un conflitto
   dichiarato, ma la stessa scuola fornisce ipotesi e conferme.
2. **Nessun gruppo di partecipanti, e la parola "customer" e' fuorviante.** Il §2.4 chiede del "same-day
   customer order flow", ma i dati (ThetaData) **non** dicono chi ha scambiato: e' volume di tutti, firmato al
   piu' con la regola della quotazione [§4.1, §4.2]. Gli autori lo ammettono in §3.2 e §6.3.
3. **I prezzi dell'indice nei primi minuti (nostra osservazione, non del paper).** L'S&P 500 alle 9:35 e'
   calcolato su titoli che non hanno ancora aperto tutti: nei primi minuti il valore dell'indice e' in parte
   **vecchio**. Se nei primi cinque minuti i future scendono, si comprano put (PCR alto) e l'indice "recupera"
   il ritardo nei minuti dopo, il rapporto sembra predire l'ora senza informazione nuova. Il controllo del
   rendimento dall'apertura alla lettura e' misurato sullo stesso indice, quindi non lo esclude; un
   controllo sul rendimento di ES dei primi cinque minuti si'. Il paper non ne parla, e la fonte dei livelli
   al minuto dell'indice non e' dichiarata [§4.1, §4.3]; non lo e' neanche il calendario dei giorni macro, che
   la Tab. 17 dice "described in Section 4.1" mentre il §4.1 non lo descrive. **E' un'ipotesi nostra, da provare.** Va pero'
   detto che il decadimento in 15-30 minuti e il placebo nullo sono compatibili sia con la loro lettura sia
   con questa.
4. **Il verso e' fragile, e lo dicono.** Nel 2024-2026 il verso non c'e' (p 0,205); resta il "se" (p 0,037),
   che e' lo stesso spostamento letto in un altro modo [Tab. 15, §5.4]. Su 621 sedute recenti il coefficiente
   del verso e' meno di un quarto di quello del 2022-2023 (-0,0006 contro -0,0027).
5. **La dimensione e' minuscola.** R quadro aggiunto circa 0,6 punti percentuali; 3,5 punti base contro una
   deviazione standard dell'ora di circa 40 [§5.2]. Dentro il campione il tasso di successo del logit (54,4%)
   e' quello di misure che non predicono niente, contro il 53,1% di "sempre nessun calo" [Tab. A.1, Tab.
   A.11].
6. **La significativita' e' per giornata**, un'osservazione per seduta, con Newey-West: e' quella che chiede il
   nostro metodo. Un nullo per permutazione c'e', ma **solo** sul calo censurato (Freedman-Lane, p 0,001)
   [Tab. A.10]; il verso e' giudicato contro zero.
7. **Varianti provate.** Quattro misure di flusso, nove letture, quattro esiti, tre misure di stock, piu' 11
   prove dopo i risultati, piu' varianti "secondarie" "riportate senza correzione" [Tab. 26]. La
   pre-specificazione e' dichiarata ("scritta nel codice dell'analisi"), ma il codice non e' pubblico e non
   c'e' una registrazione datata: si prende sulla parola. Il PCR non pesato era il controllo di robustezza
   dichiarato, e **sul verso non passa** (p 0,058) [Tab. 7].
8. **Il fuori campione e' corto e lo dicono**: 756 giornate, potenza bassa; per provare un guadagno della
   dimensione stimata servirebbe circa il doppio [§5.7, §6.3]. Il "niente" fuori campione e' quindi un
   **limite superiore**, non una prova di assenza. Pre-specificata e' solo la regola di trading; la
   previsione del rischio (AUC, DeLong, Clark e West) e' stata decisa dopo aver visto il risultato del
   trading [§5.7], anche se con lo stesso calendario.
9. **Una contraddizione interna**: §5.6 dice che la configurazione scelta e' "positive in sample and negative
   out of sample", ma Tab. 13 da' **+0,05** fuori campione (a 1 punto base), e la didascalia della Fig. A.4
   dice "roughly flat". Negativa e' solo da 2 punti base in su (-0,24) [Tab. 12].
10. **Il rendimento del backtest e' quello dell'indice**, che non si compra: il costo di 1 punto base e' quello
    di ES o SPY [§4.4.5], ma il rendimento di ES o SPY nell'ora non e' quello dell'indice, e la differenza non
    e' misurata.
11. **Un'unita' sospetta**: Tab. 5 da' il volume cumulato delle call e delle put "in millions of contracts"
    (medie 31,09 e 32,26) alla lettura dei cinque minuti. Trentuno milioni di contratti 0DTE di SPX in cinque
    minuti non sono plausibili; e non e' detto se quelle due righe siano alla lettura o sull'intera seduta.
    **Verificato sul PDF reso come immagine**: la didascalia dice proprio "millions of contracts", e lo stesso
    paper stima per una seduta media "on the order of 100,000 contract-minutes" [§4.1], cioe' righe, non
    contratti. L'unita' resta incerta (forse migliaia); non tocca i risultati, ma tocca chi volesse rifare la
    misura.
12. **I volumi cumulati in Tab. 5 stanno sotto "Market-Condition Controls"**, ma i controlli dichiarati sono
    solo VIX, rendimento dall'apertura e giorno della settimana [§4.4.1]: etichetta imprecisa, non un errore di
    stima.
13. **Il periodo**: maggio 2022 - giugno 2026, "anni insolitamente movimentati", come dicono loro [§6.3];
    nessuna prova che duri in un mercato calmo, e il verso infatti si spegne nella meta' piu' calma.
14. **Look-ahead**: non ne vediamo. Delta e gamma sono quelli del minuto dello scambio [§4.2]; l'OI e' di ieri;
    il VIX e' alla lettura; le soglie del fuori campione sono sulla finestra di stima [§5.9.1]. La sola
    stima sull'intero campione e' la scala "per deviazione standard", che non entra nel fuori campione.

## Cosa Dice Al Riquadro "Chi Ha Comprato Delta Oggi"

Il riquadro mostra il **DEX di SPX per gruppo** di OptionsDepth (clienti non professionali, professionali,
firm, broker-dealer), 0DTE e tutte le scadenze, sommato dalle 9:31. Dal paper:

- **Il paper non ha gruppi.** Non dice niente su quale gruppo sia informato: e' volume di tutti. Non conferma
  e non smentisce la sintesi della ricerca su questo punto.
- **Le misure piu' vicine al riquadro non predicono niente sull'ora dopo.** Il DEX e' delta, con il verso
  (comprato meno venduto). Nel paper: il PCR pesato per il **delta** t +0,07 sul verso e p 0,217 sul se; il
  flusso firmato in dollari t -0,11; il punteggio firmato alla Bergsma, in quattro pesi, t fra -0,55 e +0,34
  [Tab. 7, Tab. A.8, Tab. 23]. Non e' lo stesso dato (loro firmano con la regola della quotazione, OptionsDepth
  con il codice d'origine del Cboe e un suo modello), ma la direzione e' quella: **sugli 0DTE di un indice,
  sapere chi ha comprato non ha anticipato l'ora dopo**; ha anticipato qualcosa solo il "dove" (put contro call
  vicino al denaro), non il "chi" ne' il "verso".
- **L'orizzonte utile e' corto e all'apertura.** Quello che c'e' sta nei primi 15 minuti e anticipa la sola
  ora dopo; un flusso cumulato e letto a meta' giornata, come il riquadro lo mostra, nel paper non predice
  niente (letture da 30 a 300 minuti, t fra -0,55 e +1,46) [Tab. 8]. Il placebo dice che il valore di ieri non
  dice niente di oggi [Tab. 18].
- **Anche l'unico segnale non serve a decidere**: fuori campione non distingue i giorni di calo meglio del VIX
  e degli altri controlli [§5.7]. Il riquadro resta una **descrizione**, come la sintesi gia' dice.
- **Lo stock conta per l'ampiezza, non per il verso** (indizio, non finding): dove la gamma dell'OI e'
  concentrata vicino al prezzo l'ora e' meno ampia [Tab. 25]. E' la GEX dei market maker, non il DEX di oggi:
  sta in un altro riquadro della pagina X-Ray.
- **Cosa si potrebbe aggiungere, come descrizione**: un rapporto put/call 0DTE delle prime 5-15 minuti pesato
  per la gamma, confrontato con la sua storia. Se OptionsDepth dia il volume per strike, e non solo il DEX, e'
  **da verificare**; senza, non si costruisce.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

Lo strumento e' NQ, i dati da ATAS (CLAUDE.md, decisione 2); le opzioni QQQ/NDX del Cboe e il DEX di SPX sono
solo **contesto**, mai in un backtest.

- **Il test del paper su NQ non si rifa' con quello che abbiamo.** Servono anni di volume al minuto per strike
  delle opzioni 0DTE, con la volatilita' implicita per la gamma. La catena CME di ATAS (opzioni sul future NQ)
  registra **solo in avanti**, da quando e' sul chart; se dia il volume per strike al minuto e' **da
  verificare**.
- **Registrando in avanti**: ogni seduta, alle 9:35 di New York (convertite in UTC per la data, come vuole
  `sedute.py`), il PCR 0DTE pesato per la gamma dalla catena CME di NQ, poi il rendimento di NQ dalle 9:35 alle
  10:35. **Quante sedute servono** (nostro calcolo, non del paper; ipotesi: lo stesso effetto e la t che cresce
  con la radice del numero di sedute): per il verso (t -2,65 su 1.027) circa **585 sedute** per t = 2; per il
  "se" (t 3,22) circa **400**. Cioe' fra uno e mezzo e due anni e mezzo, e senza garanzia che l'effetto di SPX
  valga per le opzioni CME su NQ.
- **Con le sole barre di ATAS, una prova del dubbio 3** (nostro, su ES e SPX, non su NQ): se il rendimento dei
  primi cinque minuti del future anticipi il rendimento della cassa nell'ora dopo. Su NQ l'equivalente
  (future contro l'indice NDX) richiede l'indice al minuto, che da ATAS non abbiamo: **da verificare**.
- **Il fuori campione di NQ e' dal 2025** (CLAUDE.md, regola del framework): qualunque sguardo ai dati
  2025-2026, anche esplorativo, lo consuma.
- **Nessuna strategia**: il paper stesso mostra che la regola non e' usabile, e lo Sharpe lordo fuori campione
  ha un intervallo che contiene lo zero [Tab. A.9].

## Cosa Ne Prende Il Framework

Il ruolo e' **metodo**: come idea il paper e' negativo sull'uso, ma il disegno e' un buon modello.

- **Una sola specifica principale dichiarata prima**, il resto etichettato come robustezza o esplorazione, e
  le prove dopo i risultati corrette in una famiglia a parte [§4.4.1, Tab. 26]: e' la nostra regola delle
  varianti dichiarate e del registro.
- **Distinguere informazione e utilita'**: un coefficiente significativo dentro il campione e un guadagno
  fuori campione sono due prove diverse; la seconda si fa contro **il modello con i soli controlli**, non
  contro il caso (AUC con DeLong, Clark e West) [§5.7].
- **Trasformare un nulla in un limite**: effetto minimo rilevabile, potenza, sedute necessarie [Tab. A.4,
  §5.7]. Da aggiungere ai nostri rapporti quando una prova non passa.
- **Separare il se dal quanto** con un modello a due parti e la scomposizione in punti base (eq. 14), con il
  controllo indipendente della regressione censurata [§5.4, Tab. 10].
- **Il placebo con il segnale di ieri** alla stessa ora, e il segnale **ortogonalizzato** alla mossa
  d'apertura togliendo il controllo, perche' con il controllo dentro il risultato e' identico per costruzione
  (Frisch-Waugh-Lovell) [§5.8, Tab. 16, Tab. 18].
- **La griglia dei costi** e il confronto con la versione adattiva: una regola libera di cercare che fa peggio
  di quella fissa dice che non c'e' struttura [§5.6].
- **Da non prendere**: il giudizio sul verso contro zero senza un nullo per permutazione; la sola permutazione
  sul calo censurato non basta per il nostro obbligo 3.

## Per La Verifica

I punti dove il lettore ha dovuto ricostruire, da guardare sul PDF:

- **Le formule** (eq. 1-6, 14) sono ricostruite dalle definizioni a parole: il simbolo della gamma e dei pesi
  sparisce nel testo estratto. Il segno meno davanti all'OTS (eq. 4) e la presenza di eta in SDV (eq. 3)
  vanno guardati.
- **Tab. 17 e Tab. 18**: nel layout le colonne si mescolano; i valori qui sono dall'estrazione `-raw`
  (senza i giorni macro: 898 sedute, -3,67, p 0,008, logit p 0,0026; interazione -0,85, p 0,856, logit vuoto;
  gap notturno -2,99, p 0,033, logit vuoto). Che le celle vuote siano davvero vuote va controllato.
- **Tab. 19**: nell'estrazione `-raw` anche il PCR dell'OI non pesato ha t -1,18 (p 0,240), come quello
  pesato (p 0,238); va controllato che non sia una riga duplicata.
- **Tab. A.4**: nel layout i valori sono sfalsati di una riga; quelli qui sono dall'estrazione `-raw`.
- **Tab. 5**: l'unita' dei volumi cumulati (punto 11 dei limiti).
- **Il 52,5% contro il 53,1%**: il primo e' "sempre nessun calo" fuori campione [§5.7], il secondo dentro il
  campione [Tab. A.1]: sono due campioni diversi, non una contraddizione.
- **La contraddizione del punto 9** (§5.6 contro Tab. 13 e Fig. A.4).
- **Nessuna istruzione** trovata nel PDF (cercate parole come "ignore", "instruction", "assistant": solo testo
  del paper).

**Come si sono chiusi questi punti in verifica**: formule confermate (meno dell'OTS e segno del tipo d'opzione
in SDV compresi); Tab. 17 e 18 confermate cella per cella, e le celle vuote sono vuote anche sul PDF; la t
-1,18 della Tab. 19 e' stampata due volte nel PDF, su due righe con coefficienti e p diversi (-0,0005, p
0,238; -0,0006, p 0,240, gli stessi della Tab. A.11) e lo stesso R quadro 2,21%: non e' un errore di
estrazione, puo' esserlo del paper e non si decide; Tab. A.4 confermata; Tab. 5 come stampata (l'unita'
resta incerta, punto 11); la contraddizione del punto 9 confermata (la Tab. 13 e' calcolata "at the headline
cost", cioe' 1 punto base).

## La Verifica

**9 novembre 2026**, fase 4, verifica avversaria sul PDF (sha256 `b140ae46...cd71d` ricalcolato: coincide; 83
pagine; il numero SSRN 7339718 non e' stampato nel PDF, viene dal download). Testo con `pdftotext -layout`,
pagine rese come immagine con `pypdfium2` (17, 18, 20, 22, 24, 33, 37, 40, 52, 58, 59, 60 del PDF).

**Circa 320 affermazioni controllate** (ogni numero dei 27 risultati, del metodo, dei limiti, del riquadro e
della scheda): **circa 310 confermate**, **10 corrette o precisate**, nessuna tolta. I punti chiesti in
particolare sono tutti confermati: campione (1.027 sedute dal 19/05/2022 al 24/06/2026, ThetaData, una sola
seduta tolta, 756 giornate fuori campione); alle 9:35 -0,0013, t -2,65, p 0,008, -3,46 punti base per
deviazione standard (Tab. 7, 16), logit +0,8032, t 3,22, p 0,0013, circa 5 punti (Tab. 9, A.6); Holm 0,0163 e
0,0038 (Tab. 14); dopo 15 minuti niente (Tab. 8); 2024-2026 p 0,2047 sul verso, logit 0,0373 (Tab. 15);
Sharpe +0,34 lordo [-0,60, +1,31], +0,05 a 1 punto base, AUC 0,531 contro 0,520, DeLong 0,233, Clark e West
0,118 (Tab. 12, A.9, §5.7); delta t +0,07 (p 0,947), flusso firmato t -0,11 (p 0,909), firmato pesato per la
gamma t -0,23 (Tab. 7, 23, 24).

Le correzioni:

1. **La famiglia confermativa** (la Domanda): il paper dice che l'insieme confermativo e' H1-H4; H1-H3 e' la
   famiglia della correzione, H4 confermativa ma fuori dalla correzione [§3.3, §4.4.1]. L'analisi diceva "la
   famiglia confermativa e' H1-H3".
2. **Perche' il peso della gamma non identifica il canale** (il Perche'): non perche' la gamma e' uguale per
   put e call, ma perche' copertura e informazione si concentrano entrambe vicino al denaro [§3.2]. La
   simmetria della gamma spiega un'altra cosa, che il segno dei market maker non si ricava.
3. **La previsione del rischio fuori campione e' decisa dopo** il risultato del trading [§5.7]: aggiunto nel
   Metodo e nel limite 8 (la regola di trading resta pre-specificata).
4. **La Fig. 1 vista**: la pendenza e' fatta dal decile 1 (circa +11 punti base) e dal decile 10 (circa -7);
   i decili 2-9 oscillano fra circa -3 e +4.
5. **Incoerenza del paper sulla concentrazione della gamma**: Tab. A.11 (p 0,005, 0,325, 0,327) contro §5.8 e
   Tab. 25 (0,016, 0,256, 0,210). Aggiunta al risultato 21.
6. **La finestra che si allarga** (Clark e West p 0,028) che il testo dice "non regge la correzione della Tab.
   26" nella Tab. 26 non c'e'. Aggiunto al risultato 26.
7. **I costi delle opzioni**: §4.4.5 dice 10-50 punti base, la didascalia della Tab. 12 "da cinque in su".
8. **Il calendario macro** della Tab. 17 rimanda a un §4.1 che non lo descrive: aggiunto al limite 3.
9. **L'unita' dei volumi della Tab. 5**: stampata davvero "millions of contracts"; aggiunto il "100,000
   contract-minutes" per seduta del §4.1. Resta incerta.
10. **Come e' stato letto** e i dubbi del lettore: aggiornati con cio' che la verifica ha visto sul PDF
    (formule, Tab. 17-19, Tab. 13).

**La piu' grave**: la 1, perche' cambia cosa il paper conta come confermativo (H4 lo e'). Nessun numero
dell'analisi era sbagliato. **Resta incerto**: l'unita' dei volumi della Tab. 5; la t -1,18 doppia della
Tab. 19; quale delle due serie di p sulla concentrazione (Tab. 25 o A.11) sia quella giusta; la fonte dei
prezzi dell'indice al minuto e del calendario macro. Nessuna istruzione nel PDF.
