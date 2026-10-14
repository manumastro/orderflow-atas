---
chiave: garleanu-2009-demand-based-option-pricing
sigla: GPP
titolo: "Demand-Based Option Pricing"
autori: [Nicolae Garleanu, Lasse Heje Pedersen, Allen M. Poteshman]
versione_letta: "NBER Working Paper 11843, dicembre 2005"
fonte: NBER
identificativo: "w11843"
url: https://www.nber.org/papers/w11843.pdf
tipo: working paper
revisione_fra_pari: "no per la versione letta; si' per quella pubblicata (Review of Financial Studies 22(10):4259-4299, 2009), che non abbiamo letto"
pagine: 49
pdf: garleanu-2009-demand-based-option-pricing.pdf
pdf_sha256: 02e1b2107ba6e5c34555a1017e35c2f2a9da1e7eb5d44a528faa6dc0b6bef4ee
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; i dati (open interest CBOE per classe di investitore, OptionMetrics, la volatilita' filtrata di Bates) sono proprietari o privati"
---

# Garleanu, Pedersen E Poteshman 2009: L'Analisi

*Demand-Based Option Pricing*, Nicolae Garleanu, Lasse Heje Pedersen, Allen M. Poteshman. **Letta la
versione NBER Working Paper 11843 del dicembre 2005** (49 pagine), non quella pubblicata nella
*Review of Financial Studies* nel 2009: i numeri della rivista possono essere diversi, e non li
abbiamo controllati. Letto per intero il 14 ottobre 2026. Citazioni `[GPP §n]`.

Le formule delle sezioni 2 e 3 e dell'appendice escono storpiate da `pdftotext` (le lettere greche
spariscono): qui non se ne riporta nessuna, solo il loro significato come lo dice il testo. Le
tabelle 1-5 sono state rilette con `pdftotext -raw` e sono leggibili. **I valori delle barre delle
figure 1-3 non si leggono dal testo**: dove servirebbero, lo diciamo.

## I Termini

- **Opzione call / put**: il diritto di comprare / vendere il sottostante a un prezzo fissato
  (*strike*) entro una scadenza.
- **Market maker** (o *dealer*): chi sta in mezzo, quota prezzi in acquisto e vendita e prende
  l'altro lato degli ordini dei clienti; poi si copre sul sottostante.
- **End user** (utente finale): chi usa l'opzione per un bisogno suo (protezione, speculazione). Nel
  paper sono i clienti pubblici e i *firm proprietary traders* (i desk che operano in proprio).
- **Open interest**: i contratti aperti a fine giornata. Il totale dei lunghi e' uguale al totale
  dei corti; **per una sola classe di investitore no** [GPP nota 13].
- **Domanda netta** (*net demand*): per una classe, open interest lungo meno open interest corto.
  Positiva vuol dire che la classe e' compratrice netta di opzioni.
- **Gamma**: quanto cambia il delta (la sensibilita' al sottostante) quando il sottostante si muove.
  **Call e put con lo stesso strike e la stessa scadenza hanno la stessa gamma.**
- **Moneyness**: strike diviso il prezzo dell'indice. Sotto 1 le put sono fuori dal denaro (OTM),
  sopra 1 le call.
- **Volatilita' implicita in eccesso** (*excess implied volatility*, "expensiveness"): la
  volatilita' implicita meno una volatilita' attesa di riferimento (per SPX quella filtrata dal
  modello di Bates 2005; per le azioni una GARCH(1,1)). Misura quanto un'opzione e' cara.

## La Domanda

Se i market maker non possono coprirsi perfettamente, la domanda degli utenti finali sposta i
prezzi delle opzioni? E chi tiene davvero quali opzioni, sull'indice S&P 500 e sulle azioni?

Per noi conta la seconda parte: **gli indicatori GEX** (la gamma dei dealer sommata per strike)
**suppongono che i dealer siano lunghi le call e corti le put**. Questo paper e' il primo a guardare
le posizioni vere, per classe di investitore.

## Il Metodo

**Il modello** [GPP §2-3]. Un dealer rappresentativo, avverso al rischio (utilita' esponenziale),
commercia molte opzioni sullo stesso sottostante in tempi discreti e si copre con il sottostante e
con un titolo senza rischio. Gli utenti finali hanno una domanda esogena d; in equilibrio il dealer
tiene q = -d [GPP Def. 1]. I risultati:

- la domanda di un'opzione ne alza il prezzo in proporzione alla **varianza della parte non
  coperta** dell'opzione, e alza il prezzo delle altre opzioni in proporzione alla covarianza delle
  parti non coperte [GPP Teor. 2, Prop. 3];
- **call e put con lo stesso strike e scadenza si sommano**: una domanda di a call e b put vale come
  a + b call [GPP Prop. 2]. E' la parita' put-call;
- tre fonti di rischio non coperto: **la copertura discreta** pesa la domanda con la gamma (sposta il
  livello della volatilita' implicita, non la pendenza) [GPP Prop. 5]; **i salti** pesano con il
  danno del salto, che la gamma non misura (una put molto OTM ha poca gamma ma molto rischio di
  salto) e possono cambiare la pendenza [GPP §3.2, Prop. 6]; **la volatilita' stocastica** pesa con
  una vega corretta per la scadenza (livello, non pendenza) [GPP Prop. 7];
- la domanda sugli strike bassi pesa di piu' sulle put OTM che sulle call OTM [GPP Prop. 4].

**I dati** [GPP §4]:

- **CBOE**: per ogni giorno dal 1996 al 2001 e per ogni opzione SPX e su azioni, l'open interest
  lungo e corto di chiusura dei **clienti pubblici** e dei **firm proprietary traders**. I market
  maker sono il resto, perche' le tre classi fanno tutto il mercato. Dati non pubblici.
- Due misure: **non-market-maker** (clienti + firm proprietary come utenti finali) e **public
  customer** (solo i clienti; i firm proprietary come fornitori di liquidita'). Danno risultati
  simili; si riporta quasi sempre la prima.
- **OptionMetrics** (Ivy DB) per le volatilita' implicite di fine giornata; CRSP per i rendimenti.
- 303 azioni con volume di opzioni positivo in almeno l'80% dei giorni.

**Le prove** [GPP §5]: regressioni mensili della volatilita' in eccesso sulla domanda netta (eq. 38)
e della pendenza della volatilita' in eccesso (*skew*) sulla pendenza della domanda (eq. 39). La
domanda entra in quattro forme: contratti contati, pesati con la gamma, con il rischio di salto
(salti del +5% e del -20% equiprobabili), con la vega corretta. **Volatilita' e parametri tenuti
fissi** (20%, r = 5%, dividendo 1,5%) per pesare le opzioni, cosi' che la variabile cambi solo
quando cambia la domanda [GPP App., Empirical Implementation]. Errori standard Newey-West; per le
azioni, regressioni per titolo con test binomiale sui segni e una regressione trasversale
Fama-MacBeth.

## I Risultati

SPX e 303 azioni americane, 1996-2001, dati giornalieri di fine giornata.

| | risultato | dove |
|---|---|---|
| 1 | SPX: domanda netta media giornaliera degli utenti finali **+103.254 contratti** (non-market-maker), **+136.239** (clienti pubblici). Gli utenti finali sono **compratori netti** di opzioni sull'indice | §4 |
| 2 | SPX per tipo: **put +124.345, call -21.091** (non-market-maker); **put +182.205, call -45.966** (clienti). Gli utenti finali comprano put e **vendono circa 30.000 call**: la domanda netta viene quasi tutta dalle put | §4 |
| 3 | SPX per scadenza e moneyness (call e put insieme): il 39% della domanda netta sta sotto i 30 giorni; la colonna con moneyness sotto 0,85 fa 32.519 contratti su 103.260. **La domanda e' positiva in 51 celle su 56**, anche sopra il prezzo (1,00-1,05: 9.987; 1,05-1,10: 6.457; 1,10-1,15: 3.074) | Tab. 1A |
| 4 | Azioni: domanda netta per titolo **-2.717** (non-market-maker), -4.873 (clienti); put -1.103, call -1.614. Gli utenti finali sono **venditori netti** di opzioni su azioni, put e call in misura simile, in quasi tutte le celle (fanno eccezione le scadenze oltre l'anno) | §4, Tab. 1B |
| 5 | Volatilita' in eccesso media: **SPX +8,7%**, azioni -0,3%. Lungo la moneyness varia del 15% su SPX e del 5% sulle azioni | §4, Fig. 1, 3 |
| 6 | Posizione netta dei clienti su SPX da -1.065 a +385.750 contratti, "quasi sempre positiva"; i market maker ne sono quasi lo specchio. Correlazioni: clienti-market maker **-0,90**, clienti-firm proprietary -0,78, **firm proprietary-market maker +0,44** | §4, Fig. 4 |
| 7 | P&L dei market maker coperti una volta al giorno sul delta: **fra +100 e -100 milioni $ al giorno**, circa **+800 milioni in sei anni**, circa 1 milione l'anno per market maker (poco piu' di cento). Senza costi e senza i guadagni dello spread | §4, Fig. 5, nota 15 |
| 8 | Livello, SPX, 1997/10-2001/12, 50 mesi: domanda pesata con la gamma 2,8E-9 (t 3,85, R2 0,19), con i salti 3,2E-5 (t 3,68, R2 0,26), con la vega 1,1E-5 (t 2,74, R2 0,16); **contratti contati 3,8E-7, t 1,55, non significativo**. Nel 1996/01-1996/10 (10 mesi) **nessun coefficiente significativo** | Tab. 2 |
| 9 | Con il modello a salti: dal minimo al massimo della domanda, la volatilita' in eccesso cambia di circa 5,6 punti; una deviazione standard di domanda ne muove mezza di volatilita'. Della volatilita' in eccesso media del periodo (4,9%), **1,7 punti** corrispondono alla domanda media | §5.1 |
| 10 | Dopo 20 giorni di perdite dei market maker l'effetto della domanda **raddoppia**: 2,6E-5 (t 3,7, 330 giorni) contro 1,1E-5 (t 6,1, 646 giorni). E' il segno di un'offerta che dipende dal capitale del dealer | §5.1 |
| 11 | Pendenza, SPX, periodo tardo: skew della domanda 5,4E-7 (t 3,4, R2 0,22), modello a salti 1,8E-5 (t 3,0, R2 0,28); dal minimo al massimo circa 3 punti di skew. Periodo iniziale: nulla | Tab. 4 |
| 12 | Azioni, serie storiche per titolo: coefficiente medio 12E-6 e 8,5E-6 (prima e dopo il 1999), positivo in 227 su 303 e 213 su 299, test binomiale p = 0,00. Trasversale (Fama-MacBeth): 5,9E-8, t 6,44. Skew: 4,0E-6 e 4,2E-6, positivo in 200 su 270 e 163 su 224 | Tab. 3, 5, §5.1 |
| 13 | Solo call: la relazione fra domanda di call e volatilita' in eccesso delle call e' **negativa**. Gli autori: conta la domanda totale di call e put allo stesso strike (Prop. 2), e quella delle put domina | §4, Fig. 2 |

## La Convenzione "Dealer Lunghi Call, Corti Put" Su SPX, Secondo Questi Dati

E' la domanda per cui il paper e' nel filone. La risposta ha due parti.

**1. Per tipo di opzione, il segno della convenzione regge, in media, nel 1996-2001.** Gli utenti
finali comprano put (+124.345 contratti al giorno) e vendono call (-21.091); quindi i market maker,
insieme ai firm proprietary quando li si conta come fornitori, sono **corti di put e lunghi di
call** [GPP §4]. Ma non in modo simmetrico: **le put pesano circa sei volte le call** (con la misura
dei clienti, quattro volte). Sulle azioni la convenzione e' sbagliata: li' gli utenti finali vendono
put e call, e i dealer sono lunghi di entrambe [GPP Tab. 1B].

**2. Per la gamma, la domanda e' un'altra, e li' la convenzione sbaglia.** Una call e una put con lo
stesso strike e la stessa scadenza hanno la stessa gamma, e il paper mostra che le loro domande si
sommano [GPP Prop. 2]. Quindi **la gamma dei dealer su uno strike dipende dalla somma call + put
tenuta dagli utenti finali su quello strike**, non dalla divisione fra call e put. La tabella 1A da'
proprio quella somma: e' **positiva in 51 celle su 56, anche sopra il prezzo**. Nel 1996-2001, in
media, i dealer erano **corti di gamma quasi su ogni strike e scadenza**, non lunghi sopra e corti
sotto. La convenzione GEX invece mette gamma positiva dove l'open interest delle call supera quello
delle put. Questa e' una **deduzione nostra** dalla tabella 1A e dalla Prop. 2: il paper non calcola
la gamma dei dealer.

**Cosa la convenzione non puo' vedere**:

- conta **tutto** l'open interest come se fosse del dealer, mentre la posizione del dealer e' la
  **differenza** fra lunghi e corti degli utenti finali. Il paper non riporta l'open interest totale,
  quindi non si puo' dire che quota ne tenga il dealer;
- il segno cambia nel tempo: la posizione dei clienti va da -1.065 a +385.750 contratti, ed e' piu'
  alta nel primo anno [GPP §4, Fig. 4];
- i **firm proprietary** si comportano in parte da fornitori di liquidita' (correlazione +0,44 con i
  market maker): chi e' "dealer" e' una scelta di classificazione;
- **la gamma non misura il rischio di salto** delle put molto OTM, che e' quello che i dealer fanno
  pagare di piu' [GPP §3.2].

**Il periodo pesa.** 1996-2001: prima delle scadenze settimanali e degli 0DTE, con un cambio di
struttura nel 1997 (arrivano gli e-mini e le loro opzioni al CME) [GPP §5.1]. Vasquez e altri
(2025), con le posizioni dei market maker misurate trade per trade sul 2020-2023, trovano invece una
**gamma dei market maker per lo piu' positiva**, ma negativa almeno per un momento in un giorno su
quattro (vedi la loro analisi in questo filone). I due paper misurano cose diverse (contratti netti
contro gamma) in anni diversi; insieme dicono che **un segno fisso per tipo di opzione non e' un
dato, e' un'ipotesi che va misurata**.

## Cosa Non Regge, O Non Si Dice

1. **I dati sono proprietari.** L'open interest per classe di investitore viene direttamente dal
   CBOE; la volatilita' di riferimento l'ha data Bates, la skew del modello l'ha data Chernov. Non si
   puo' rifare.
2. **"Dealer" vuol dire market maker del CBOE.** I dealer fuori borsa (le banche, OTC) non si
   vedono. La frase "Wall Street e' corta di volatilita' sull'indice" e' mostrata solo per i market
   maker di borsa, e solo su SPX: le opzioni SPY e quelle sul future al CME non ci sono.
3. **La divisione del campione e' scelta guardando i dati.** Il cambio di struttura del 1997 si vede
   dalla figura 4; il primo sotto-campione ha 10 mesi e non mostra niente; sul campione intero "la
   robustezza e il potere esplicativo sono minori" [GPP §5.1]. Le azioni hanno la stessa divisione,
   al 1999.
4. **Varianti provate.** Quattro modi di pesare la domanda; il conteggio semplice non e'
   significativo, e lo diventa restringendo la banda di moneyness [GPP nota 17]. E' una ricerca di
   specifica, non dichiarata come tale.
5. **La significativita'** e' su 50 osservazioni mensili, con Newey-West. La versione giornaliera e'
   "simile" ma non riportata. Nessun modello nullo nel senso del nostro metodo.
6. **Causalita'.** Si regredisce un livello su un livello: domanda alta e opzioni care possono avere
   una causa comune (la paura). La prova dopo le perdite dei market maker (risultato 10) va nella
   direzione degli autori, ma non chiude la questione.
7. **La volatilita' in eccesso dipende dal modello di riferimento** (Bates per SPX, GARCH per le
   azioni): cambia il modello, cambia la misura.
8. **Nessun costo, nessuna strategia.** E' un paper sui prezzi delle opzioni. Il P&L dei market
   maker esclude i costi e i guadagni dello spread, che gli autori stessi dicono potrebbero essere la
   parte maggiore [GPP nota 15].
9. **Look-ahead**: non e' un problema qui, perche' non si prevede niente. La GARCH delle azioni usa i
   cinque anni prima del giorno; la volatilita' di Bates e' detta filtrata.
10. **La versione letta e' del 2005.** La pubblicata (2009) non e' stata letta.

## Cosa Si Puo' Rifare Su ES, Con ATAS

Quello che abbiamo: **barre a 1 minuto di ES** con volume e delta, dal Data Bridge; gli indicatori
**Options Chain** su Rithmic (OI Profile, Key Levels, Expected Move, GEX Profile con Zero Gamma)
**senza storia**, che stimano la GEX dall'open interest con un segno supposto; gli indicatori
**Options X-Ray**, dati OptionsDepth sulle SPX ogni minuto, con le posizioni divise per gruppo di
partecipanti, market maker compresi, con una storia che non conosciamo (vedi
[`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md)).

- **Rifare il paper: no.** Servono l'open interest per classe di investitore e la volatilita'
  implicita storica; ATAS non li ha nel passato.
- **Misurare la convenzione: si', registrando da oggi in avanti.** Ogni giorno, su ES, si salvano
  insieme (a) la GEX per strike della Chain, con il segno supposto, e (b) la gamma dei market maker
  per strike di X-Ray. Poi si conta **in quanti strike e in quanti giorni i due segni coincidono**, e
  quanto distano i due Zero Gamma. E' la prova diretta della convenzione, sui nostri dati. **Da
  verificare prima**: se il Data Bridge puo' leggere cio' che calcolano gli indicatori, e come
  OptionsDepth attribuisce le posizioni ai gruppi (e' una stima anche quella).
- **Sommare call e put per strike.** Qualunque misura di gamma dei dealer che costruiremo deve usare
  la posizione netta call + put per strike (Prop. 2), non la differenza fra open interest call e
  put.
- **Due cose non si sanno**: le opzioni che la Chain legge su ES via Rithmic sono quelle CME sul
  future, non le SPX di questo paper e di X-Ray; e la Chain non dice come calcola le greche.

## Cosa Ne Prende Il Framework

- **Il segno dei dealer e' un dato da misurare, non una convenzione.** Una variabile GEX che entra in
  uno studio dichiara da dove viene il segno (supposto o misurato) e lo prova.
- **Call e put allo stesso strike si sommano** per la gamma [Prop. 2].
- **Pesare con parametri fissi** (volatilita', tasso, dividendo costanti) per evitare che la variabile
  si muova per ragioni diverse da quella che vuole misurare [App., Empirical Implementation]. Vale per
  ogni GEX calcolata da noi: la gamma di un'opzione al denaro cresce quando la volatilita' implicita
  scende, e questo da solo crea una relazione fra gamma e volatilita'.
- **La gamma non e' tutto il rischio del dealer**: i salti contano a parte [§3.2].
