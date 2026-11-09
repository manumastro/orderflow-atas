---
chiave: garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders
sigla: AGPV
titolo: "The Rise of Algorithmic Retail Option Traders"
autori: [Diego Amaya, Pedro A. Garcia-Ares, Neil D. Pearson, Aurelio Vasquez]
versione_letta: "SSRN 6480379, versione del 12 agosto 2026 (la pagina SSRN: scritto il 14 febbraio 2026, rivisto il 13 agosto 2026)"
fonte: SSRN (working paper)
identificativo: "10.2139/ssrn.6480379"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=6480379
tipo: preprint
revisione_fra_pari: no
pagine: 50 (frontespizio 1, abstract 2, testo 3-27, bibliografia 28-32, figure 33-38, tabelle 39-47, Internet Appendix 48-50)
pdf: garcia-ares-2026-the-rise-of-algorithmic-retail-option-traders.pdf
pdf_sha256: 8eda5c817adad7c8a4e63b57685747174f163d1f29d7afd85030a29774e2c1c7
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; record OPRA di scambi e quote delle opzioni SPX forniti da SpiderRock, non pubblici"
---

# Amaya, Garcia-Ares, Pearson, Vasquez 2026: L'Analisi

*The Rise of Algorithmic Retail Option Traders*, Diego Amaya (Wilfrid Laurier), Pedro A. Garcia-Ares (ITAM),
Neil D. Pearson (Illinois Urbana-Champaign, Canadian Derivatives Institute), Aurelio Vasquez (ITAM). Working
paper su SSRN, **versione del 12 agosto 2026**, **non revisionato**. Nel PDF il primo autore e' Amaya (la
chiave della cartella segue l'ordine della pagina SSRN); il titolo del frontespizio dice "Options Traders",
quello di pagina 2 "Option Traders". Letto per intero il 9 novembre 2026, Internet Appendix compresa. Il testo
estratto con `pdftotext -layout` mescola le colonne delle tabelle 1, 4, 5, 7, 8, IA1 e IA2: **tutte le figure
e le tabelle (pagine 33-47, 49, 50) sono state rese come immagine** con `pypdfium2` e lette sull'immagine.
Citazioni `[AGPV §n]`, `[AGPV Tab. n]`, `[AGPV Fig. n]`. Nessuna istruzione trovata nel PDF.

**In parole semplici.** Sulle opzioni SPX che scadono in giornata (0DTE), a ogni mezz'ora esatta arriva una
raffica di scambi: parte entro pochi secondi dalle 10:00, 10:30, 11:00 e cosi' via, e finisce entro un
minuto. Sono soprattutto **spread venduti** (si incassa un premio e si limita la perdita con un'opzione
comprata piu' lontano) e iron condor, piccoli, con parametri da "menu" delle piattaforme che automatizzano il
trading dei privati. Gli autori le leggono come **bot retail che partono tutti alla stessa ora** perche'
usano le stesse piattaforme. Non provano mai se questo flusso anticipa o segue il prezzo dell'indice, e non
sanno chi lo manda: i loro dati non hanno l'identita' ne' il tipo di cliente.

## I Termini

- **0DTE** (*zero days to expiration*): opzioni che scadono il giorno stesso in cui si scambiano.
- **OPRA** (*Options Price Reporting Authority*): il nastro consolidato di tutti gli scambi e le quote delle
  opzioni americane. Dice prezzo, taglia, ora, se lo scambio e' una gamba di un ordine complesso; **non dice
  chi ha scambiato ne' in che verso** [AGPV nota 2].
- **Cboe Open-Close**: i dati della borsa che dividono il volume per tipo di cliente (clienti non
  professionali, professionali, firm, broker-dealer), apertura o chiusura, acquisto o vendita. E' il tipo di
  dato da cui viene il DEX per gruppo di OptionsDepth. **Questo paper non lo usa** [AGPV nota 2].
- **Ordine complesso** (*multi-leg*): piu' opzioni scambiate insieme a un prezzo netto unico, per esempio uno
  spread verticale (due gambe) o un iron condor (quattro). L'opposto e' lo scambio **semplice** (una gamba).
- **Spread verticale venduto** (*short vertical*): si vende un'opzione piu' vicina al prezzo e se ne compra
  una piu' lontana dello stesso tipo e scadenza; si incassa un premio (il credito) e la perdita massima e'
  l'ampiezza fra i due strike meno il credito. Su put e' una scommessa che l'indice **non scenda** sotto lo
  strike venduto; su call che **non salga**.
- **Iron condor venduto**: uno spread verticale venduto di put piu' uno di call, insieme: si guadagna se
  l'indice resta fra i due strike venduti [AGPV nota 8].
- **Delta**: quanto cambia il valore dell'opzione per un punto dell'indice; il delta dello strike venduto e'
  il parametro con cui le piattaforme scelgono gli strike [AGPV §2.2].
- **Raffica** (*burst*, *spike*): il picco di scambi nel minuto che comincia a un orario tondo.
- **Spread quotato, effettivo, realizzato** [AGPV §3.3]: il quotato e' meta' della distanza fra la migliore
  offerta in vendita e in acquisto; l'effettivo e' la distanza fra il prezzo dello scambio e il punto medio,
  con il segno del verso, cioe' quanto paga chi chiede di essere eseguito subito; il realizzato e' la stessa
  distanza misurata sul punto medio 1 o 10 minuti dopo, cioe' quanto resta al market maker. Per gli spread e
  gli iron condor il quotato e' **sintetico**: nel libro degli ordini complessi non ci sono quote in attesa, e
  gli autori sommano i mezzi spread delle singole gambe nel libro semplice [AGPV §7.1].
- **ROC-AUC**: la probabilita' che il modello dia un punteggio piu' alto a uno scambio della raffica preso a
  caso che a uno scambio "tranquillo" preso a caso; 0,5 e' il caso, 1 e' perfetto.
- **Newey-West**: errori standard che tengono conto della correlazione fra giorni vicini.

## La Domanda

Degli algoritmi indipendenti possono produrre un flusso d'ordini sincronizzato **senza** imparare, comunicare
o reagire l'uno all'altro, solo perche' ereditano le stesse regole dalle stesse piattaforme? Gli autori lo
cercano negli 0DTE di SPX, con quattro previsioni [AGPV §2.4]: (1) scambi concentrati a orari fissi; (2) piu'
forte sugli ordini complessi; (3) caratteristiche comuni (delta, ampiezze); (4) entrate e uscite a orari da
regole. E poi: cosa fa questa prevedibilita' al costo di scambiare [AGPV §7].

La domanda non e' "chi e' informato" ne' "il flusso anticipa il prezzo": il paper non la pone.

## Il Metodo

**I dati** [AGPV §3.1]: record OPRA di scambi e quote di **tutte** le opzioni SPX (sono quotate solo su Cboe
C1, quindi e' tutto il mercato), forniti da SpiderRock, dal **19 maggio 2022 al 31 gennaio 2026**. Solo
orario regolare "9:30 to 16:00 EST"; via scambi cancellati, taglie o prezzi non positivi, quote incrociate,
prezzi fuori dall'intervallo [bid - spread, ask + spread]. Le stampe semplici uguali (stesso contratto, ora
della quota, borsa, prezzo, condizione) si fondono in un'osservazione.

**Il verso** [AGPV §3.1, §3.2.2]: per le gambe singole, sopra il punto medio e' un acquisto, sotto una
vendita (Muravyev 2016); per gli spread e gli iron condor, il premio netto eseguito contro quello implicito
nei punti medi: se e' piu' basso, e' una vendita (corta), se e' piu' alto un acquisto; uguale, neutro.

**Gli ordini complessi ricostruiti** [AGPV §3.2.1, nota 12]: OPRA segna che una gamba fa parte di un ordine
complesso, non quale. Gli autori raggruppano le gambe con **lo stesso millisecondo e lo stesso prezzo del
sottostante**; due gambe = verticale se stesso tipo, stessa scadenza, strike diversi, taglie uguali; quattro
= iron condor se si dividono in un verticale di call sopra uno di put. Le gambe dello stesso ordine stampate
a microsecondi di distanza si separano, quindi **le strategie con piu' gambe sono sottocontate** (lo dicono).

**Le prove.**

- Profilo per minuto e per secondo del numero di scambi e del volume, per anno, semplici contro complessi, per
  scadenza (0DTE...7DTE) [AGPV §4, Fig. 1-5]: **solo figure**, senza test statistico sul picco.
- **Il minuto della raffica contro i cinque minuti prima**, per le mezz'ore dalle 10:00 alle 15:30 (12
  finestre; esclusi apertura e chiusura), gennaio 2024 - gennaio 2026 [AGPV §5, Tab. 1-5, IA1]: quote per
  strategia, struttura degli spread, profitti a 5 e 30 minuti e a scadenza. Newey-West giornaliero, 8 ritardi
  (nella Tab. 1 "the optimal number of lags, on average eight days").
- **Il classificatore** [AGPV §6, Tab. 6-8, Fig. 6]: 11.130.060 scambi di spread verticali in 516 giornate
  (gennaio 2024 - gennaio 2026, tolti i primi 25 e gli ultimi 5 minuti di ogni giorno); la raffica sono i primi
  60 secondi di ogni mezz'ora (778.694 scambi, il 7,0% del totale, in finestre che fanno il 3,3% dell'orario),
  il riferimento i minuti 25-29 della stessa mezz'ora (1.192.268, il 10,7%). LightGBM con 24 caratteristiche
  dello scambio e del mercato prima, **senza l'ora**; addestrato sul primo 80%, cinque finestre che si
  allargano per la taratura, provato **una volta** sull'ultimo 20% [AGPV §6.3, Tab. 7].
- Spread quotato, effettivo e realizzato, raffica contro i cinque minuti prima [AGPV §7, Tab. 9, IA2].

**Cosa manca nel metodo**: nessuna variabile di prezzo dell'indice come risultato; nessun dato per tipo di
cliente; nessun confronto con un modello nullo per i picchi (i picchi si vedono nelle figure, non si
provano).

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Raffiche a ogni mezz'ora**: picchi netti alle 10:00, 10:30, 11:00, 11:30 e, piu' grandi rispetto al fondo, alle 13:00, 13:30, 14:00; nel minuto del picco gli scambi sono **circa il doppio** dei minuti intorno, quelli complessi **piu' del triplo** (il §1 lo dice della raffica delle 13:30, il §4.1 in generale) | §1, §4.1, Fig. 1 |
| 2 | **Al secondo**: nessuna salita prima; il picco parte entro pochi secondi dall'orario e torna verso il normale in 30-60 secondi; negli ultimi anni il secondo di picco vale **4-6 volte** il minuto prima. Nella Fig. 3 (2026) il picco e' poco sotto 100 scambi al secondo contro circa 20-25 prima | §4.3, Fig. 3 |
| 3 | **Crescono ogni anno e si allargano** ad altri orari: i quarti d'ora valgono 1,05 volte i minuti intorno nel 2022, **fra 1,3 e 1,5 dal 2023**; i multipli di cinque minuti da 1,02 (2022) a **1,12 (2026)**; le 13:45 arrivano a circa **il doppio** dei vicini nel 2026 | §4.2, Fig. 2 |
| 4 | **Ordini complessi e 0DTE**: le raffiche sono soprattutto negli ordini complessi (Fig. 4B) e negli 0DTE; sulle scadenze piu' lunghe quasi spariscono | §4.4, §4.5, Fig. 4, 5 |
| 5 | **La composizione cambia** (gennaio 2024 - gennaio 2026, minuto della raffica contro i 5 minuti prima): scambi complessi dal 37,1% al **49,4%** (+12,2 punti); premio sotto 25 dollari dal 6,6% all'11,9%; taglie fino a 5 contratti l'83,5% prima e l'85,0% durante; piu' strike fuori dal denaro (moneyness sotto -0,01 dall'11,7% al 17,2%) | §5.1, Tab. 1 |
| 6 | **Per orario**: la quota dei complessi sale di 6,1 punti alle 10:00, **21,3 alle 13:30** (t 48,7), sopra 18 fino alle 14:30, 3,3 alle 15:30 | §5.1, Tab. IA1 |
| 7 | **Le strategie** (Tab. 2): put verticali vendute dal 3,6% al **6,7%**, call verticali vendute dal 3,1% al **5,5%**, iron condor venduti dallo 0,6% all'1,2%; le call e le put semplici **scendono** (call comprate dal 16,6% al 14,6%). Alle 13:30 le put verticali vendute salgono di 7,8 punti, alle 14:00 le call verticali vendute di 4,3; alle 10:00 solo 0,7 e 0,6; alle 15:30 circa zero | §5.2, Tab. 2, IA1 |
| 8 | **Meno quando il mercato si muove**: la quota delle put verticali vendute nel minuto della raffica scende dall'8,10% nel quintile di volatilita' realizzata (30 minuti prima) piu' basso al **4,69%** nel piu' alto (differenza -3,41 punti, t -16,47); meno marcato sulle call verticali vendute (da 5,75% a 4,91%, t -5,65) e sugli iron condor (da 1,36% a 1,10%, t -4,58) | §5.2, Tab. 3 |
| 9 | **La struttura** (Tab. 4): strike venduti piu' vicini all'indice (call da 23,2 a 22,6 punti, put da 28,7 a 27,2; sugli iron condor invece piu' lontani, da 20,6 a 21,5), spread piu' larghi (call da 28,8 a 42,4 punti, put da 41,9 a 62,3), credito per unita' d'ampiezza piu' basso (call dal 17,2% all'11,3%, put dal 13,3% al 9,2%), meno contratti (call da 4,7 a 3,4). Si muovono insieme come un obiettivo di delta e un tetto di perdita | §5.3, Tab. 4 |
| 10 | **Profitti** (in dollari al giorno, sommati sulle 12 raffiche): a 5 minuti le call verticali vendute perdono 6.954 (t -2,54), le put 2.541 (t -0,73), gli iron condor 1.762 (t -4,85); a scadenza guadagnano 2.011 (t 0,06), 35.536 (t 0,82), 3.090 (t 0,44): **positivi ma non diversi da zero**. Anche gli spread aperti nei 5 minuti prima perdono a 5 minuti (put -7.357, t -3,36; iron condor -2.515, t -3,04); la differenza raffica meno prima, a 5 minuti, e' -5.964 sulle call (t -3,18) e +4.816 sulle put (t 2,09) | §5.4, Tab. 5 |
| 11 | **Il classificatore senza l'ora**: fuori campione ROC-AUC **0,685**, PR-AUC 0,531 contro una base del 36,1%, Brier 0,218, precisione 0,52, richiamo 0,61; nelle cinque finestre di taratura ROC-AUC 0,702 +/- 0,022. Le variabili piu' usate: l'ampiezza dello spread (22,6% del guadagno), la volatilita' realizzata dei 30 minuti prima (9,9%), la volatilita' implicita al denaro dei 5 minuti prima (5,6%), il vega netto (5,6%) | §6.3, Tab. 7, 8, Fig. 6 |
| 12 | **Raffica contro riferimento** (spread verticali): 125,8 scambi al minuto contro 48,1 (2,6 volte); ampiezza 56,5 punti contro 36,3; credito 8,8% contro 14,5%; **delta netto -0,011 contro -0,001**; volatilita' realizzata prima **9,21% contro 11,69%** (t -12,57), implicita uguale (12,01% e 11,99%) | §6.2, Tab. 6 |
| 13 | **Liquidita'**: lo spread quotato **si stringe** (call verticali -0,009 dollari, put -0,005, iron condor -0,004), quello effettivo **si allarga** (+0,004, +0,004, +0,006). Alle **10:00** anche il quotato si allarga (+0,0187, +0,0283, +0,0308; t 6,1, 4,6, 5,6); il piu' stretto alle 13:30 (-0,0226 sulle call verticali). Il realizzato a 10 minuti si muove al contrario fra call (+0,031) e put (-0,034), niente sugli iron condor | §7.1, Tab. 9, IA2 |
| 14 | **Quanto pesa lo 0DTE e il retail, da fonti Cboe citate** (non misurato dagli autori): gli 0DTE il 59% del volume SPX nel 2025, 63% a febbraio e 66% a luglio 2026; il retail **fra meta' e tre quinti** del volume 0DTE di SPX; oltre il 95% degli scambi 0DTE di SPX a rischio limitato | §1, nota 1, §2.1, nota 4, §2.3 |

## Il Perche'

**Il meccanismo degli autori** [AGPV §2.2-2.4]: le piattaforme (API dei broker, bot senza codice come Option
Alpha) offrono strategie a modello (verticali, iron condor) e un menu di pochi valori tondi: delta dello
strike venduto, ampiezza, orario d'entrata, obiettivo di profitto, uscita. Chi sceglie gli stessi valori, o
adotta lo stesso bot pubblicato, manda lo stesso ordine alla stessa ora ogni giorno, senza sapere degli
altri: **sincronizzazione senza coordinamento**. Il rischio limitato viene dai margini e dai controlli dei
broker [AGPV §2.3]. Cboe, che vede i conti, attribuisce i picchi a strumenti d'automazione retail (Trade
Automation Toolbox di IBKR, Option Alpha, Trade Steward, Option Omega, Brokerbotics) [AGPV §1, nota 3].

**Perche' retail e non istituzionale** [AGPV §1, §5.3, §7.2]: scambi piccoli, a orari fissi e a pacchetti
riconoscibili, mentre gli algoritmi istituzionali spezzano l'esecuzione nel tempo per nascondersi; nessuna
uniformita' simile fra i clienti dei broker non retail (Ni, Pan, Poteshman 2008). Gli autori **ammettono**
che il modello riconosce "trading automatico a orario", **non il retail**: un algoritmo istituzionale potrebbe
fare lo stesso [AGPV §7.2].

**Le alternative scartate**: l'arrivo di notizie (i picchi piu' grandi sono nel primo pomeriggio, quando le
notizie programmate sono poche, e non si concentrano sugli annunci macro: **affermato senza una tabella o una
figura che lo mostri**) [AGPV §1, §4.5]; la forma a U del
volume (qui il picco appare e sparisce all'orario) [AGPV §4.1]; un protocollo d'esecuzione istituzionale fisso
(ci sarebbe dall'inizio, invece cresce) [AGPV §4.2]; il market making (non concentrerebbe posizioni corte di
volatilita' a orari fissi) [AGPV §5.2].

**Sul prezzo** [AGPV §2.4, §7.2]: l'orario e' fissato prima, quindi il flusso "non puo' rispondere"
all'informazione del momento; il costo piu' alto non e' selezione avversa ("We find no evidence that these
traders know where prices are going") ma la simultaneita'. **E' un'affermazione dedotta dal meccanismo, non
una prova sui rendimenti dell'indice.**

**Cosa lo ucciderebbe**, dedotto dal perche': picchi che si spostano con le notizie o con il prezzo invece
di restare agli orari; picchi uguali fra semplici e complessi; e, per l'etichetta "retail", i dati Open-Close
che mettono le raffiche fuori dai clienti (gli autori dicono di volerli usare nella prossima versione, nota 2).

## Cosa Non Regge O Non Si Dice

1. **Nessuna prova sul prezzo dell'indice.** Nessuna regressione del rendimento di SPX sul flusso, ne' prima
   ne' dopo. Le uniche relazioni col mercato sono **prima** dello scambio (la raffica pesa meno quando la
   volatilita' realizzata dei 30 minuti prima e' alta, Tab. 3 e 6; il rendimento dell'indice dei 5 minuti prima
   e' l'ottava variabile del classificatore, Tab. 8) e **dopo** solo attraverso i profitti degli spread e lo
   spread realizzato (Tab. 5, 9). La frase "nessuna prova che sappiano dove va il prezzo" [§7.2] e' un'assenza
   di prova, non una prova di assenza.
2. **Lo spread realizzato a 10 minuti non va letto come verso del prezzo.** Call verticali vendute +0,031,
   put -0,034 (Tab. 9). Una lettura possibile, **nostra e non provata**: per un market maker che compra lo
   spread, un valore che dopo sale sulle call e scende sulle put e' quello che succede se l'indice sale nei 10
   minuti dopo. Un movimento comune (volatilita', decadimento del tempo) sposterebbe gli spread venduti di call e
   di put nello stesso verso, quindi spiega poco la **differenza** fra i due; resta la lettura degli autori,
   "domande di copertura diverse" [§1, §7.1], che non e' misurata. Vanno nello stesso verso, sempre nostra
   lettura, le differenze a 5 minuti della Tab. 5 (call vendute -5.964, t -3,18; put vendute +4.816, t 2,09).
   Per finestra pochi valori arrivano a |t| 2: sulle call le 13:00 (2,0), 13:30 (2,8), 14:30 (2,2) e 15:00
   (2,1), sulle put le 10:30 (-2,1) e le 15:00 (-3,9), sugli iron condor le 13:30 (2,0) (Tab. IA2).
3. **Chi sono, nei gruppi di Cboe, non si sa.** OPRA non ha il tipo di cliente [nota 2]. Se le raffiche
   cadano nei **clienti non professionali** (il "retail" di OptionsDepth) o nei **professionali** non e'
   provato. Un'ipotesi nostra da verificare: le analisi di CLZZ (nota 8) e MCH (App. A) dicono che il cliente
   professionale del Cboe e' chi manda piu' di 390 ordini al giorno; un bot che entra a molti orari con ordini a
   piu' gambe potrebbe passare la soglia.
4. **Tre "retail" diversi.** Qui il retail e' **riconosciuto dal comportamento** (orario, modello, taglia); la
   quota "fra meta' e tre quinti" degli 0DTE e' una stima di Cboe citata, non misurata [§2.1, nota 4]; in BBG il
   retail riconosciuto dall'asta (SLAN/MLAT, fino a 10 contratti) e' oltre il 6% degli 0DTE; in ADEFOV (§3.4) i
   clienti del Cboe comprendono gli istituzionali. Nessuno dei tre coincide con il gruppo di OptionsDepth.
5. **Il verso e' stimato.** Punto medio per le gambe singole, premio netto contro i punti medi per i pacchetti
   [§3]; gli scambi al punto medio sono "neutri". Non validato: gli autori aspettano i dati Open-Close [nota 2].
6. **Pacchetti ricostruiti**: stesso millisecondo e stesso sottostante; le strategie a piu' gambe sono
   sottocontate (lo dicono, nota 12). Le gambe di due ordini diversi nello stesso millisecondo, nella raffica,
   potrebbero anche fondersi: non discusso.
7. **I picchi non hanno un test**: le Fig. 1-5 sono medie, senza intervallo ne' modello nullo; i rapporti
   "1,05", "1,3-1,5", "1,02-1,12" [§4.2] non hanno errore. I test (Newey-West giornaliero) ci sono solo nelle
   tabelle 2024-2026. Ne' l'assenza di concentrazione sugli annunci macro [§1, §4.5] ne' la finestra delle
   14:00 dei giorni di FOMC, che gli autori dicono di trattare "separately" [§7.1], sono mostrate in una
   tabella.
8. **Incoerenze del paper**, da guardare:
   - la quota dei complessi e' **37,1% -> 49,4%** nella Tab. 1 (e nel testo) ma **19,7% -> 28,4%** nella Tab. 2;
     forse una conta per gambe e una per pacchetti, non detto;
   - il §1 dice che lo spread quotato si stringe "solo nel pomeriggio", ma la Tab. IA2 lo mostra stretto anche
     alle 10:30, 11:00 e 11:30 (su tutte e tre le strategie alle 10:30); si allarga alle 10:00 su tutte e tre, e
     alle 14:00 sugli iron condor (+0,0201, t 4,3; il §7.1 lo dice);
   - la didascalia della Tab. IA2 rimanda alla "Table 8": e' la Tab. 9;
   - i profitti "per scambio complesso" del §5.4 (75,50, 55,75, 167,16, 4,19, 237,03, 85,69, 392, 71,46, 345,71
     dollari) **non sono in nessuna tabella** e non tornano con i totali giornalieri: per gli iron condor
     1.762/167,16 fa circa 10,5 scambi al giorno a 5 minuti e 1.742/237,03 circa 7,3 a 30 minuti, sugli stessi
     scambi; per le put vendute 2.541/55,75 fa circa 46 scambi a 5 minuti e 191/85,69 circa 2,2 a 30 minuti
     (nostro conto);
   - la Tab. 6 dice che il riferimento occupa **48** minuti al giorno ma lo definisce "minuti 25-29" di 12
     mezz'ore (sarebbero 60; e con i primi 25 e gli ultimi 5 minuti tolti, i minuti 15:55-15:59 della mezz'ora
     delle 15:30 sono fuori dal campione); 1.192.268 / 516 / 48 fa 48,1, quindi il conto usato e' 48 (4 minuti
     per finestra), non spiegato;
   - **non e' un'incoerenza** il "3,3% of the regular trading hours" [§6.1]: tolti i primi 25 e gli ultimi 5
     minuti, l'orario del campione e' di 360 minuti, e 12/360 fa il 3,3% (nostro conto; il testo dice "regular
     trading hours", che sarebbero 390);
   - la Tab. 4 da' il delta dello strike venduto che **scende** in valore assoluto alla raffica (call da 0,3 a
     0,2, put da -0,3 a -0,2, differenza stampata "0.0*"), mentre il §5.3 dice che gli strike piu' vicini
     "raise the delta of the position"; con una sola cifra decimale non si sa quanto;
   - la didascalia della Fig. 3 dice "a window of 60 seconds centered on the spike time", ma l'asse va da -1 a
     +1 minuto;
   - il volume della Fig. 1B e 2B (fra 200.000 e oltre 1.000.000 di contratti **al minuto**) per 390 minuti fa
     oltre 100 milioni di contratti al giorno (nostro conto): l'unita' o la media non tornano;
   - "never falls below 0.668" [§6.3] non e' nella Tab. 7 (che da' solo media e deviazione);
   - il segno dei greci della Tab. 6 non e' dichiarato: con piu' put verticali vendute (delta positivo per chi
     vende) che call, ci si aspetterebbe un delta netto positivo dal lato di chi inizia, e la tabella da'
     -0,011. Tutti i greci della tabella hanno il segno di **chi compra** lo spread: gamma positiva (0,0118),
     vega positivo (0,380), theta negativo (-1,978), e il §6.2 dice "more vega and more negative theta" di
     spread in maggioranza venduti. Lettura nostra: i greci sono dal lato lungo del pacchetto (la controparte, o
     una convenzione "spread comprato"), e allora il -0,011 e' coerente con le put vendute; il paper non lo dice.
9. **"Senza l'orologio" e' vero a meta'**: la seconda variabile e' la volatilita' realizzata **prima**
   (Tab. 8), che distingue i giorni calmi (dove la raffica pesa di piu') piu' che gli scambi; e raffica e
   riferimento stanno nella stessa mezz'ora, quindi il confronto e' fra due momenti vicini, non fra retail e
   altri. Il modello riconosce gli spread "da raffica", non chi li manda.
10. **Gli orari**: "9:30 to 16:00 EST" [§3.1]. Se fosse l'ora solare fissa tutto l'anno, d'estate ogni orario
    sarebbe spostato di un'ora; i picchi resterebbero sulle mezz'ore, ma il profilo per orario (Tab. IA1)
    mescolerebbe le 10:00 con le 11:00. Il forte picco d'apertura alle 9:30 delle Fig. 1 e 5 fa pensare all'ora
    di New York; non e' detto.
11. **La forma a U c'e' ancora**: le Fig. 1 e 5 hanno l'apertura come massimo assoluto e la risalita verso la
    chiusura. "I picchi piu' grandi nel pomeriggio" [§1] vale **rispetto al fondo**, non in valore assoluto (la
    raffica delle 10:00 e' la piu' alta in numero di scambi, Fig. 1A).
12. **Il campione e' nella crescita degli 0DTE** (2022-2026) e le raffiche crescono ogni anno: ogni numero e'
    una media su un fenomeno che cambia. Le analisi di dettaglio sono solo gennaio 2024 - gennaio 2026.
13. **Conflitti**: dati forniti da SpiderRock, ringraziata; nessun conflitto dichiarato. Pearson e Vasquez
    sono autori di VAPG; la nota 10 rimanda a un loro paper sulle liquidazioni forzate di Robinhood.

## Cosa Dice Al Riquadro "Chi Ha Comprato Delta Oggi"

Il riquadro (`quant/strumenti/xray/XrayLive/Server/Flusso.cs`, letto, non toccato) somma il DEX per gruppo dal
minuto k dopo le 9:30 di New York (`Cassa.Di` per la data) e lo confronta con le sedute passate **allo stesso
minuto k**: percentile, sorpresa oltre il prezzo, sbilancio netto/lordo, variazione degli ultimi 30 minuti;
fino a 60 sedute passate, almeno 10.

**Cosa conferma.**

- **Lo stesso minuto e' il confronto giusto per le raffiche.** Le raffiche cadono agli stessi orari ogni
  giorno [§4, Fig. 1-3]: allo stesso k ogni seduta passata ha gia' "dentro" le stesse raffiche, e il confronto
  le neutralizza senza correzioni, **se l'orario e' quello di New York**, come gia' fa il riquadro. Un UTC
  fisso le sposterebbe di un'ora per meta' dell'anno.
- **La finestra di 30 minuti** (`trentaMinuti`) contiene sempre esattamente un orario di mezz'ora: e'
  confrontabile fra un momento e l'altro della giornata quanto alle raffiche.
- **Nessuna lettura come previsione**: il paper non prova il flusso contro il prezzo, e il suo meccanismo dice
  che le raffiche sono a orario, non a informazione [§2.4, §7.2]. Conferma la nota del riquadro.

**Cosa aggiunge, da verificare sull'archivio prima di cambiare qualcosa.**

- **Il minuto subito dopo l'orario e' instabile.** La raffica si consuma in 30-60 secondi [§4.3]: nel minuto
  che contiene le :00 o le :30 la lettura di oggi puo' essere avanti o indietro rispetto allo storico, secondo
  come OptionsDepth data la colonna (se la colonna delle 13:30 copre 13:29-13:30 o 13:30-13:31: **da
  verificare**). Una lettura a un minuto o due dall'orario e' piu' confrontabile.
- **Lo storico invecchia.** Le raffiche crescono ogni anno e si aggiungono orari (quarti d'ora, cinque minuti)
  [§4.2, Fig. 2]: con una finestra lunga, le sedute vecchie hanno raffiche piu' piccole. Il tetto di 60 sedute
  limita il problema; non andrebbe allungato a molti mesi senza dirlo.
- **Le raffiche pesano piu' sul lordo che sul netto.** Sono soprattutto spread venduti di put **e** di call e
  iron condor [Tab. 2], a delta netto piccolo (Tab. 6: -0,011 per spread, segno non dichiarato). Ipotesi nostra:
  nel DEX la raffica alza la somma dei |delta| (il lordo) piu' del netto, e allo scoccare della mezz'ora lo
  **sbilancio** si avvicina a zero. Quanto pesano nel giorno: nel paper il primo minuto di ogni mezz'ora porta il
  7,0% degli spread verticali nel 3,3% dell'orario [§6.1]; sugli scambi 0DTE il minuto di picco vale circa il
  doppio dei vicini [§4.1]. Nostro conto: dodici minuti al doppio sono intorno al 6% degli scambi del giorno.
  **Le raffiche non spiegano il flusso accumulato del riquadro**; possono spiegarne gli scalini.
- **Il peso fra put e call cambia con l'orario** [Tab. IA1, aumento della quota in punti]: le put verticali
  vendute (delta positivo per chi vende) salgono piu' delle call (delta negativo) in quasi tutte le finestre, di
  piu' alle 13:30 (+7,8 contro +3,6) e ancora alle 14:00 (+5,3 contro +4,3), dove le call hanno il loro massimo;
  le call salgono piu' delle put solo alle 10:30 (+3,2 contro +2,0) e alle 15:00 (+2,9 contro +1,5). Ipotesi
  nostra: se la raffica entra nel DEX dei clienti, i suoi scalini hanno un segno che dipende dall'ora, non dal
  mercato, per lo piu' positivo per chi vende. Sono quote di scambi, non delta pesati.
- **Le 14:00 dei giorni di FOMC** vanno lette a parte: gli autori dicono di trattare quella finestra
  separatamente [§7.1], ma il paper non lo mostra.
- **Le raffiche scendono quando il mercato si agita** [Tab. 3, Tab. 6]: in una seduta mossa la parte "a
  orario" del flusso dei clienti e' piu' piccola e quella discrezionale piu' grande. E' un motivo in piu' per
  la sorpresa oltre il prezzo, non una correzione.

**Cosa cambia nell'etichetta "Retail".** Il paper rafforza la sintesi: il retail degli 0DTE che si riconosce
(qui dal comportamento, in BBG dall'asta) **non e' per definizione** il gruppo dei clienti non professionali di
OptionsDepth. Dove cadano le raffiche fra `nonprocust` e `procust` si puo' misurare (sotto), e dice quale delle
due colonne porta il retail automatico.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

- **Il flusso per gruppo su NQ, no**: la catena CME di ATAS non dice chi ha scambiato, e il paper e' su SPX.
  Se le opzioni CME su NQ abbiano raffiche a orario non lo dice nessuno: il paper guarda solo SPX [§3.1].
- **Con le barre M1 di NQ** (volume e delta di ATAS, dentro il campione fino al 2024): il profilo del volume e
  del delta aggressivo nel minuto delle :00 e delle :30 contro i minuti vicini, per anno. E' una **descrizione
  della stagionalita'**, non questa domanda: le raffiche sono su opzioni SPX, e chi le prende si copre semmai su
  ES o SPX. Su ES la stessa misura e' in tema (lo studio delle opzioni, CLAUDE.md decisione 2), su richiesta.
  Ogni sguardo al 2025-2026 di NQ consuma il fuori campione.
- **Sull'archivio di X-Ray (SPX, solo contesto)**, descrittivo, con le 35 sedute: per ogni gruppo e ambito, lo
  scalino del DEX netto e del lordo nel minuto delle :00 e :30 contro i minuti 25-29 della stessa mezz'ora
  (lo stesso confronto della Tab. 6), una stima per seduta e il nullo per permutazione degli orari. Risponde a
  due domande che il paper non puo' fare: **in quale gruppo cadono le raffiche** (clienti non professionali o
  professionali) e **se lo sbilancio va verso zero** all'orario. Non e' un backtest e non entra nel registro.
- **Il verso delle raffiche sul prezzo**: con l'archivio, il rendimento di SPX o ES nei 10 minuti dopo le
  mezz'ore contro i minuti di riferimento, una stima per seduta e il nullo per permutazione; con 35 sedute dice
  poco e non prova niente.

## Cosa Ne Prende Il Framework

- **Una convenzione di lettura**, non una regola: nel flusso delle opzioni SPX 0DTE ci sono raffiche a orari
  tondi di New York, crescenti negli anni; ogni confronto con lo storico si fa **allo stesso minuto, nel fuso
  del mercato** e con una finestra di sedute non troppo lunga.
- **Un'avvertenza per l'etichetta "Retail"**: tre definizioni (comportamento, asta, tipo di cliente) e nessuna
  coincide con il gruppo di OptionsDepth.
- **Una misura descrittiva da fare sull'archivio di X-Ray** (sopra): dove cadono le raffiche fra i gruppi.
- **Niente sul prezzo**: il paper non prova che il flusso anticipi o segua l'indice.
- **Il metodo del classificatore** (fuori campione cronologico, una sola prova sull'ultimo 20%, taratura con
  finestre che si allargano) e' nello spirito dell'obbligo 2 del metodo; non serve qui.

## Dubbi Di Lettura Per Il Verificatore

- Tutte le tabelle sono lette dalle immagini delle pagine 39-47, 49, 50: rileggere in particolare Tab. 4 (tre
  blocchi), Tab. 5 (segni e t), Tab. IA1 e IA2 (per orario).
- Le incoerenze del punto 8 di "Cosa Non Regge": confermare Tab. 1 contro Tab. 2 sui complessi, i profitti per
  scambio del §5.4 assenti dalle tabelle, i 48 minuti della Tab. 6, l'unita' del volume delle Fig. 1B e 2B.
- La lettura dello spread realizzato (punto 2) e quella dello sbilancio alle raffiche sono **nostre**.
- La soglia dei 390 ordini al giorno viene dalle analisi di CLZZ e MCH, non da questo paper.

## La Verifica

**9 novembre 2026**, fase 4, verificatore diverso dal lettore. Testo estratto con `pdftotext -layout` (50
pagine) e **tutte le figure e le tabelle rilette sull'immagine** (pagine 33-37, 39, 40, 42, 43, 45, 46, 49, 50;
Tab. 3, 6 e 9 anche dal testo, leggibile). Sha256 del PDF ricalcolato: coincide con la scheda.

**Circa 190 affermazioni controllate**: 172 confermate, 17 corrette o completate, 1 tolta. Confermati sul PDF:
dati OPRA di SpiderRock dal 19 maggio 2022 al 31 gennaio 2026, nessun tipo di cliente e Open-Close solo in
programma (nota 2); "roughly twice", "more than triple" (§1, §4.1); 1,05, 1,3-1,5, 1,02-1,12 e il doppio alle
13:45 (§4.2); Fig. 3 col picco del 2026 poco sotto 100 scambi al secondo; Tab. 1 (37,1 -> 49,4; 6,6 -> 11,9;
83,5 e 85,0 sommando 1 e 2-5 contratti; 11,7 -> 17,2); Tab. 2 (3,6 -> 6,7, 3,1 -> 5,5, 0,6 -> 1,2, complessi
19,7 -> 28,4); Tab. 3 (8,10%, 4,69%, t -16,47); Tab. 4, 5, 6, 7 (0,6848, 0,5309, 0,2183, 0,52, 0,61, 0,7022 +/-
0,0215, base 36,1%), 8, 9, IA1, IA2 cifra per cifra; la quota retail "roughly one-half to three-fifths" (§1,
§2.1, nota 4, Mandy Xu di Cboe); **nessuna regressione sul rendimento dell'indice** in tutto il paper. Le
incoerenze del lettore reggono tutte tranne una (il 3,3%). Le letture nostre (spread realizzato come verso,
sbilancio verso zero, soglia dei 390 ordini) sono rimaste scritte come nostre. Nessuna istruzione nel PDF.

**Le correzioni.**

1. **La piu' grave**, nel riquadro: "alle 14:00 [prevalgono] le call" era sbagliato. Alle 14:00 le put
   vendute salgono di 5,3 punti e le call di 4,3 (Tab. IA1); le call superano le put solo alle 10:30 e alle
   15:00. Riscritto il punto sul verso per orario.
2. Tolta come incoerenza il "3,3% dell'orario": torna con i 360 minuti del campione (12/360).
3. Tab. IA2: non "solo le 10:00 si allargano"; alle 14:00 si allarga il quotato degli iron condor (t 4,3).
4. Spread realizzato per finestra: le 13:00 delle call hanno t 2,0 (non "superano"); aggiunte le 13:30 degli
   iron condor (t 2,0).
5. Punto 2: "lo stesso segno lo danno la volatilita'" non regge, perche' un movimento comune sposta call e put
   nello stesso verso; aggiunte, come lettura nostra, le differenze a 5 minuti della Tab. 5.
6. Profitti del §5.4: mancava il 4,19 dollari delle call a 30 minuti; aggiunto il conto delle put (46 scambi a
   5 minuti, 2,2 a 30).
7. Greci della Tab. 6: gamma, vega e theta hanno tutti il segno del lato lungo; aggiunto.
8. Nuova incoerenza: il delta dello strike venduto nella Tab. 4 scende mentre il testo dice che sale.
9. Nuova incoerenza: la Fig. 3 dice 60 secondi, l'asse ne mostra 120.
10. I 48 minuti della Tab. 6: aggiunto che il taglio degli ultimi 5 minuti toglie anche il riferimento delle
    15:30.
11. FOMC alle 14:00 e annunci macro: il paper li afferma senza mostrarli; aggiunto.
12. Newey-West della Tab. 1: numero di ritardi "ottimale, in media otto", non otto fissi.
13. Tab. 3: "-3,41%" e' -3,41 punti; aggiunte le call e gli iron condor.
14. Tab. 4: sugli iron condor gli strike venduti si allontanano (20,6 -> 21,5); aggiunto.
15. Tab. 5: aggiunte le perdite a 5 minuti anche prima della raffica e le differenze.
16. Tab. 8: la volatilita' implicita al denaro e' quella dei 5 minuti prima.
17. I Termini: lo spread quotato degli spread e degli iron condor e' sintetico, dalle gambe singole (§7.1).

**Cosa resta incerto**: perche' i complessi sono 37,1% nella Tab. 1 e 19,7% nella Tab. 2 (gambe contro
pacchetti e' un'ipotesi); l'unita' del volume delle Fig. 1B e 2B; il fuso "EST" (ora solare fissa o di New
York); la convenzione dei greci della Tab. 6; da dove vengano i profitti per scambio del §5.4.
