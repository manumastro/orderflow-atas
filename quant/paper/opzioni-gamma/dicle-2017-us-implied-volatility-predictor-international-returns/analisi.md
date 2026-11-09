---
chiave: dicle-2017-us-implied-volatility-predictor-international-returns
sigla: DIC
titolo: "US implied volatility as a predictor of international returns"
autori: [Mehmet F. Dicle]
versione_letta: working paper del 3 novembre 2017 ("Preprint submitted to Working Paper Series"), da SSRN 3057745; la versione pubblicata (Quantitative Finance and Economics 1(4), 388-402) non e' stata letta
fonte: SSRN; pubblicato in Quantitative Finance and Economics 1(4), 388-402 (dati della rivista dalla fase 1, non verificati qui)
identificativo: "SSRN 3057745; doi 10.2139/ssrn.3057745"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3057745
tipo: working paper (la versione in rivista esiste ma non e' quella letta)
revisione_fra_pari: "no per la versione letta; la versione in Quantitative Finance and Economics (sede minore, ad accesso aperto) e' revisionata, non vista"
pagine: 20
pdf: dicle-2017-us-implied-volatility-predictor-international-returns.pdf
pdf_sha256: 41b9313fc246a8524b30cf0ed1477d413488fb63814adee2292bb63357541476
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati pubblici: chiusure giornaliere da Yahoo! Finance confermate con Google Finance, VIX dal Cboe"
---

# Dicle 2017: L'Analisi

*US implied volatility as a predictor of international returns*, Mehmet F. Dicle (Loyola University New
Orleans). **Letto il working paper del 3 novembre 2017** scaricato da SSRN (20 pagine, "Preprint submitted
to Working Paper Series"): **non e' la versione pubblicata** in Quantitative Finance and Economics 1(4),
che non abbiamo visto. I numeri qui sotto valgono per questa versione; nella pubblicata possono essere
cambiati. Letto per intero il 9 novembre 2026, riferimenti compresi; le quattro tabelle e la figura sono
state lette sulle pagine rese come immagini (pypdfium2), perche' `pdftotext` sposta di una riga i nomi
degli indici nella Tab. 2. **Le sezioni del paper non sono numerate**: si citano per nome e pagina
(`[DIC Intr. p. 3]`, `[DIC Dati p. 7]`, `[DIC Econometria p. 9]`, `[DIC Sign. econ. p. 10]`,
`[DIC Concl. p. 11]`) e per tabella (`[DIC Tab. 2]`). Nessuna istruzione rivolta a chi legge nel PDF: la
nota 9 e' un'avvertenza legale sulla regola di trading ("solo a scopo educativo"), non un'istruzione.

## In Parole Semplici

Il paper prende il VIX (la paura implicita nelle opzioni sull'S&P 500) e 20 borse estere, con le
chiusure di ogni giorno dal 2000 al 2017. Trova che quando il VIX sale oggi, domani le borse estere
scendono, anche togliendo l'effetto dell'S&P 500 dello stesso giorno: il VIX "anticipa" 18 indici su 20.

**Pero', secondo la nostra lettura (il paper non ne parla mai), il "domani" estero e' in gran parte gia'
l'oggi americano.** Tokyo, Hong Kong, Sydney chiudono prima che New York apra; Londra e Parigi chiudono a
meta' della giornata americana. Quello che il VIX fa dopo la loro chiusura, quelle borse lo vedono solo la
seduta successiva. I numeri sono coerenti con questa lettura, senza provarla: l'effetto e' enorme in Asia ed
Europa, piccolo o assente nelle Americhe, che chiudono insieme a New York e per cui il "giorno dopo" e'
davvero dopo. Per la stessa ragione (deduzione nostra, se le date sono allineate per giorno di calendario,
cosa che il paper non dice) la regola di trading che "guadagna" in Asia compra un prezzo di chiusura che
c'era gia' prima del segnale.

Per NQ ed ES vuol dire: **niente su Nasdaq o VXN**, e fra due mercati americani aperti nelle stesse ore
il caso piu' vicino sono le Americhe del paper, dove l'anticipo giornaliero del VIX e' debole. E' un
esempio di come **non** fare la prova, piu' che una prova.

## I Termini

- **VIX**: l'indice di volatilita' implicita del Cboe, calcolato dai prezzi delle opzioni sull'S&P 500;
  dice quanta oscillazione il mercato delle opzioni si aspetta nei 30 giorni dopo. Sale quando si compra
  protezione. Qui entra come **variazione logaritmica giornaliera** (ΔVIX), non come livello [DIC Dati p. 7].
- **Volatilita' implicita**: la volatilita' che rende il prezzo teorico di un'opzione uguale a quello di
  mercato. Non e' gamma ne' flusso: e' il prezzo della protezione.
- **Anticipare** (*lead*): una serie anticipa un'altra se il suo valore di ieri ha un coefficiente
  significativo nella regressione dell'altra di oggi. Nel paper e' il test sui coefficienti della Tab. 2.
- **Causalita' di Granger**: X "causa nel senso di Granger" Y se i valori passati di X migliorano la
  previsione di Y oltre ai valori passati di Y. Non e' causalita' vera, e' solo un miglioramento della
  previsione. Si prova con un **test di Wald** (se un gruppo di coefficienti e' tutto zero) [DIC Econometria p. 9].
- **GARCH(1,1)**: un modello in cui la varianza dell'errore di oggi dipende dall'errore di ieri al
  quadrato e dalla varianza di ieri. Serve a non sottostimare gli errori nei giorni agitati [DIC Econometria p. 8].
- **SUR** (*seemingly unrelated regressions*): due regressioni stimate insieme, lasciando correlati i loro
  errori [DIC Econometria p. 9].
- **Rendimento da utili** (*earnings yield*): utile per azione diviso prezzo, il rovescio del P/E. E' il
  "perche'" dell'autore: piu' rischio chiede piu' rendimento, quindi prezzo piu' basso [DIC Intr. p. 3].
- **Orario di chiusura e fuso**: ogni borsa chiude nel suo fuso. Due chiusure con la stessa data di
  calendario non sono contemporanee: e' il punto che il paper non tratta (vedi "Cosa Non Regge").

## La Domanda

La volatilita' implicita americana (il VIX) predice i rendimenti delle borse estere, al netto del
rendimento dell'S&P 500 e della crisi 2007-2008? Se si', la diversificazione internazionale protegge meno
dal rischio americano di quanto si creda, e i rendimenti esteri si possono prevedere [DIC Abstract;
DIC Intr. p. 3, p. 6].

## Il Metodo

**I dati** [DIC Dati p. 7, Tab. 1]:

- 20 indici azionari esteri, il VIX e l'S&P 500, **chiusure giornaliere**. Indici da Yahoo! Finance
  "confermati" con Google Finance, VIX dal Cboe. Tutti come differenza di logaritmi delle chiusure.
- Periodo: da gennaio 2000 (Cile dal 2002, Nuova Zelanda dal 2003) a **luglio 2017**. Le prime date in
  Tab. 1 vanno dal 01/03/2000 al 01/06/2003; per S&P 500 e VIX il 01/05/2000.
- I paesi: Argentina (MERVAL), Australia (All Ordinaries), Belgio (BEL 20), Brasile (IBOVESPA), Canada
  (S&P/TSX), Cile (IPSA), Francia (CAC 40), Germania (DAX), Hong Kong (Hang Seng), India (SENSEX),
  Indonesia (Jakarta), Giappone (Nikkei 225), Malesia (FTSE Bursa), Messico (IPC), Nuova Zelanda (NZX 50
  gross), Singapore (STI), Corea del Sud (KOSPI), "Svizzera" (ESTX50 EUR), Taiwan (TSEC), Regno Unito
  (FTSE 100) [DIC Tab. 1].
- La variabile di crisi vale 1 dal 1 gennaio 2007 al 9 marzo 2009 [DIC Dati p. 7].
- VIX: media giornaliera -0,0006, minimo -0,3506, massimo 0,4960, deviazione standard 0,0666; S&P 500
  deviazione standard 0,0123 [DIC Tab. 1]. Test di Dickey-Fuller aumentato: tutte le serie stazionarie in
  differenza.
- Osservazioni per indice da 2.801 (Nuova Zelanda) a 4.348 (Francia) [DIC Tab. 2, colonna N].
- **Non detto**: come si allineano le date fra borse con calendari e fusi diversi.

**Le regressioni** [DIC Econometria p. 8-9], una per indice:

```text
(1) ΔIndex_d = b0 + b1 ΔIndex_d-1 + b2 ΔIndex_d-2 + b3 ΔVIX_d-1 + b4 ΔVIX_d-2
               + b5 ΔSP500_d + b6 Crisis_d + e_d,     varianza GARCH(1,1)
(2) ΔVIX_d   = g0 + g1 ΔVIX_d-1 + g2 ΔVIX_d-2 + g3 ΔIndex_d-1 + g4 ΔIndex_d-2
               + g5 ΔSP500_d + g6 Crisis_d + e1_d
(3) ΔIndex_d = w0 + w1 ΔIndex_d-1 + w2 ΔIndex_d-2 + w3 ΔVIX_d-1 + w4 ΔVIX_d-2
               + w5 ΔSP500_d + w6 Crisis_d + e2_d
(4) Index -> VIX:  g3 = g4 = 0        (5) VIX -> Index:  w3 = w4 = 0     test di Wald
```

(2) e (3) stimate insieme con il SUR. Il controllo dell'S&P 500 e' **solo contemporaneo** (ΔSP500_d),
mai ritardato [DIC Econometria p. 9]. Formule ricontrollate sulle pagine 8 e 9 rese come immagini.

**La prova economica** [DIC Sign. econ. p. 10, Tab. 4]: se il VIX scende piu' del 2,5% (variazione
logaritmica) in un giorno, si compra l'indice estero "per il giorno seguente", una posizione di un giorno,
rendimento chiusura su chiusura. I rendimenti si sommano per anno e si confrontano con il compra e tieni
dello stesso anno con un t-test su 18 anni (16 il Cile, 15 la Nuova Zelanda).

**Cosa non c'e'**: nessun campione fuori, nessun costo, nessun conto dei giorni di segnale, nessun
controllo per i test multipli (20 indici), nessuna indicazione sul tipo di errore standard del SUR.

## I Risultati

1. **Il VIX di ieri: 18 su 20 significativi al 5% o meglio, tutti negativi** [DIC Econometria p. 9, Tab. 2].
   Non significativi: Argentina (-0,0046) e Nuova Zelanda (-0,0013). Il Messico e' significativo solo al
   5% (-0,0048 **); gli altri 17 all'1%, con coefficienti da -0,0068 (Cile) a -0,0739 (Giappone): Hong Kong
   -0,0605, Corea -0,0453, Taiwan -0,0434, Francia -0,0414, Germania -0,0392, FTSE -0,0357.
2. **Il VIX di due giorni fa: 11 su 20 significativi all'1%, negativi** [DIC Econometria p. 9, Tab. 2]:
   Australia, Francia, Germania, Hong Kong, India, Giappone, Singapore, Corea, ESTX50, Taiwan, Regno Unito;
   altri due negativi solo al 10% (Belgio -0,0035, Malesia -0,0021). Nel testo la variabile e' scritta "ΔVIX_d-1" per un refuso; la tabella dice d-2. **L'Indonesia ha un
   coefficiente a d-2 positivo** (+0,0045, significativo al 10%), contro "tutti negativi" dell'introduzione
   [DIC Intr. p. 3].
3. **L'S&P 500 dello stesso giorno: positivo e significativo all'1% per tutti e 20** [DIC Econometria p. 8,
   Tab. 2]: piu' alto Brasile 0,9166, Argentina 0,8775, Germania 0,7413; piu' basso Malesia 0,0483,
   Indonesia 0,0967, poi Giappone 0,1624 e Taiwan 0,1639 (il testo nomina Taiwan fra i tre piu' bassi; in
   tabella il Giappone e' piu' basso di Taiwan).
4. **La crisi non conta** [DIC Econometria p. 8-9, Tab. 2]: significativa al 5% solo per Belgio (-0,0009)
   e Nuova Zelanda (-0,0006), al 10% per il Brasile (+0,0010). L'autore lo attribuisce all'S&P 500 nel
   modello.
5. **Granger, dal VIX agli indici: 17 su 20 al 5% o meglio** [DIC Econometria p. 9, Tab. 3]. Non
   significativi: Argentina (3,1867), Messico (4,1649), Nuova Zelanda (1,6962). I piu' grandi: Giappone
   667,4963, Hong Kong 580,2403, Australia 433,9916, Regno Unito 411,4937, Singapore 407,6022, Corea
   386,3308. Le Americhe sono basse: Brasile 7,0556 (**), Cile 17,4938, Canada 34,9830.
6. **Granger, dagli indici al VIX**: significativo al 5% o meglio per 6 (Brasile 6,1516, Canada 27,0668,
   India 6,0031, Messico 8,3291, ESTX50 10,4257, Regno Unito 9,1058), al 10% per Francia e Giappone
   [DIC Tab. 3]. Il testo non lo commenta.
7. **La regola di trading batte il compra e tieni per 15 indici su 20** [DIC Sign. econ. p. 10, Tab. 4]:
   non per Argentina, Brasile, Cile, Messico, Nuova Zelanda; Canada, India e Indonesia solo al 10% nel test
   a due code (5% a una coda). Medie annue della regola: Giappone 0,4337 (compra e tieni -0,0033), Hong Kong
   0,4084 (0,0244), Corea 0,3682; t da 5,5651 (Giappone) a -1,2657 (Nuova Zelanda). Rendimenti come somma
   di log-rendimenti giornalieri, senza costi.
8. **Nasdaq e VXN: assenti.** Nessuna parola su Nasdaq, Nasdaq-100, VXN o NDX nel testo, nelle tabelle o
   nei riferimenti (cercato su tutto il testo estratto). L'unico indice americano oltre al VIX e' l'S&P 500.

## Il Perche'

**Quello dell'autore** [DIC Intr. p. 3; DIC Concl. p. 11]: il rendimento da utili. Se le azioni si
comportano come obbligazioni, piu' rischio atteso (VIX alto) chiede un rendimento piu' alto, che si ottiene
subito con un prezzo piu' basso: da qui il segno negativo. Un segno positivo avrebbe voluto dire "fuga dal
rischio americano verso l'estero", e non si vede [DIC Econometria p. 8]. Il canale piu' semplice, la
**velocita' di aggiustamento** dei mercati esteri al rischio americano, e' dichiarato come implicazione
(mercati integrati, diversificazione che protegge poco) [DIC Econometria p. 9-10].

**Alternative scartate**: nessuna messa alla prova. L'autore cita Rapach e altri 2013 (il rendimento
americano ritardato predice gli esteri) e dice di "controllare" l'S&P 500 [DIC Intr. p. 4-6], ma lo fa
con l'S&P **dello stesso giorno**, non con quello di ieri.

## Cosa Non Regge O Non Si Dice

1. **I fusi orari (il punto centrale; deduzione nostra, il paper non ne parla).** Le chiusure hanno la
   stessa data ma non lo stesso istante. Per un indice asiatico, la "seduta d" chiude prima che New York
   apra la sua seduta d: la variazione del VIX del giorno d-1 (alla chiusura americana di d-1) e' **la
   notizia piu' recente** che l'Asia incorpora alla sua apertura di d. Per l'Europa, la parte del VIX d-1
   dopo la chiusura europea arriva nella seduta d. Quindi il "VIX di ieri" per Asia ed Europa e' in buona
   parte **informazione contemporanea**, non un anticipo. Tutto questo presuppone che le serie siano
   allineate per data di calendario, cosa che il paper non dice. I numeri sono coerenti con questa lettura
   (coerenti, non una prova: la lettura non e' stata messa alla prova):
   - Wald VIX -> indice (Tab. 3): Asia ed Europa (senza la Nuova Zelanda) da 165,69 (India) a 667,50
     (Giappone); le Americhe, che chiudono insieme a New York, 3,19 e 4,17 (Argentina, Messico, non
     significativi), 7,06 (Brasile, al 5%), 17,49 (Cile) e 34,98 (Canada), questi due all'1%.
   - coefficiente del VIX di ieri (Tab. 2): Americhe da -0,0046 a -0,0097, ma Brasile, Canada e Cile
     restano significativi all'1%; Asia ed Europa (senza la Nuova Zelanda) da -0,0238 a -0,0739.
   - regola di trading (Tab. 4): le Americhe non battono il compra e tieni (Canada solo al 10% a due code),
     Asia ed Europa si' (India e Indonesia anche loro solo al 10% a due code).
   - **Eccezione**: la Nuova Zelanda, che chiude fra le prime, non mostra niente (Tab. 2, 3, 4). La lettura
     dei fusi non la spiega; ha meno osservazioni (2.801, dal 2003).
2. **Il controllo dell'S&P 500 e' allineato male (deduzione nostra).** Il modello mette ΔSP500_d (stesso
   giorno) e non ΔSP500_d-1: questo e' un fatto del paper [DIC Econometria p. 9]. Il resto e' nostro: per
   Asia ed Europa la seduta americana d viene dopo (in tutto o in parte) la loro chiusura d; quella che
   conta e' la d-1, cioe' il rendimento americano ritardato, l'effetto che il paper attribuisce a Rapach e
   altri [DIC Intr. p. 4-5] e dice di controllare [DIC Intr. p. 6].
   Il paper stesso riporta che VIX e S&P 500 sono correlati negativamente nello stesso giorno (Sarwar,
   [DIC Intr. p. 5]): il "VIX di ieri" puo' essere in gran parte **l'S&P di ieri** sotto altro nome. Il
   test che lo direbbe (aggiungere ΔSP500_d-1) non c'e'.
3. **La regola di trading ha il look-ahead per Asia ed Europa (deduzione nostra).** Il segnale e' il VIX
   alla chiusura americana di d; il rendimento "del giorno seguente" e' chiusura d -> chiusura d+1
   dell'indice estero (la didascalia della Tab. 4 dice "bought for the following day", rendimento come
   differenza logaritmica giornaliera; che d sia la stessa data di calendario e' un nostro presupposto,
   il paper non lo dice). Per l'Asia la chiusura d viene **prima** del segnale: comprarla e' impossibile, e il
   rendimento incassato contiene la reazione al giorno americano che ha generato il segnale. Le medie
   annue del 40% di Giappone e Hong Kong [DIC Sign. econ. p. 10] vanno lette cosi'. Dove il segnale arriva
   davvero prima (le Americhe), la regola non batte il compra e tieni.
4. **Nessun campione fuori**, nessuna stima a finestra mobile: tutto il 2000-2017 dentro.
5. **Nessun costo, nessun conto delle operazioni.** Quanti giorni fanno scattare "VIX giu' piu' del 2,5%"
   non e' detto; con una deviazione standard giornaliera del VIX di 0,0666 [DIC Tab. 1] la soglia e' meno
   di mezza deviazione standard, quindi i giorni sono molti (nostro conto qualitativo, non nel paper).
6. **La prova economica confronta somme non comparabili**: la regola e' investita solo nei giorni di
   segnale, il compra e tieni sempre; nessuna misura per rischio; t-test su 15-18 anni.
7. **Errori standard e dipendenza**: il tipo di errore del SUR non e' dichiarato. Le equazioni (2) e (3)
   hanno gli stessi regressori (costante, due ritardi del VIX e due dell'indice, S&P del giorno, crisi),
   e con regressori uguali le stime del SUR coincidono con quelle di due regressioni ordinarie separate
   (proprieta' nota, deduzione nostra, non detta nel paper): il SUR non aggiunge niente. Se gli errori
   standard sono quelli classici (il paper non lo dice), il Wald non tiene conto dell'eteroschedasticita'
   che lo stesso autore modella con il GARCH nella (1).
8. **Venti test, nessuna correzione.** Con statistiche nell'ordine delle centinaia non cambia il segno del
   risultato in Asia ed Europa, ma pesa sulle Americhe al margine (Messico ** in Tab. 2, Brasile ** in
   Tab. 3).
9. **Incoerenze di testo**: l'abstract dice "anticipa 18", l'introduzione "anticipa 17 ... con due giorni
   di ritardo" [DIC Intr. p. 3]; il refuso d-1 per d-2 [DIC Econometria p. 9]; "tutti negativi" smentito
   dall'Indonesia a d-2 (al 10%); l'indice ESTX50 EUR (Euro Stoxx 50, area euro) attribuito alla
   "Svizzera" [DIC Tab. 1].
10. **Dati**: Yahoo e Google Finance, sopravvivenza degli indici non trattata (gli indici scelti sono
    quelli che esistono nel 2017), allineamento delle festivita' non detto.
11. **Sede**: working paper; la versione revisionata e' in una rivista minore (dato della fase 1, non
    verificato qui). Un solo autore.
12. **La crisi entra solo come costante, e il criterio dell'autore non regge per intero.** L'autore stesso
    riporta che in Sarwar (2014) il VIX predice le borse europee **solo nei periodi di crisi**, e che "ogni
    prova" della sua tesi "richiederebbe coefficienti tutti negativi per tutti i paesi" [DIC Intr. p. 6].
    Ma la variabile Crisis sposta solo la costante: nessuna interazione Crisis x ΔVIX, nessun
    sottoperiodo, quindi non si sa se l'effetto viene dal 2007-2009. E l'Indonesia a d-2 (+0,0045, al
    10%) manca il suo stesso criterio.

**Cosa lo ucciderebbe** (dal perche' e dai limiti): che il coefficiente del VIX di ieri sparisca
aggiungendo l'S&P 500 di ieri; oppure che sparisca allineando gli istanti (VIX misurato all'ora di
chiusura di ciascuna borsa, o rendimenti estero dall'apertura). Le Americhe del paper sono gia' un indizio
in quel senso.

## Cosa Vale Per La Ricerca Fra Indici

- **Non risponde alla domanda su NQ ed ES.** Nessun Nasdaq, nessun VXN, nessuna gamma, nessun flusso: solo
  la variazione giornaliera della paura implicita dell'S&P 500 contro borse estere.
- **Il caso piu' vicino a VIX -> NQ sono le Americhe del paper**: mercati aperti nelle stesse ore del VIX.
  Li' l'anticipo giornaliero e' debole (Argentina e Messico non significativi in Granger; Brasile, Cile e
  Canada si', ma con Wald da 7 a 35 contro 166-667 in Asia ed Europa; coefficienti piccoli, regola senza
  vantaggio) [DIC Tab. 2-4]. La nostra lettura, non una conclusione del paper: per due indici americani
  scambiati insieme, **il vantaggio di un giorno si riduce di molto quando gli orari coincidono** (non
  sparisce del tutto: Brasile, Canada e Cile restano significativi).
- **La lezione di metodo, che e' il motivo per tenerlo**: in ogni prova "le opzioni di un indice anticipano
  un altro indice" **gli istanti si allineano prima** (gli orari nel fuso di ciascun mercato, come vuole il
  repository) e il controllo del mercato guida si mette **anche ritardato**. E' lo schema di Granger da
  rifare, ma con questi due accorgimenti, non come lo fa il paper.
- Conferma, per altra via, quanto detto nella sintesi della ricerca: il legame fra indici che si vede e'
  **comune e contemporaneo**, non una guida.

## Cosa Si Puo' Rifare Su NQ Con ATAS, E Cosa No

- **VIX -> NQ giornaliero, come nel paper: no, per ora.** Il VIX ufficiale da Yahoo entra nel repository
  solo come contesto nella pagina X-Ray, mai in un backtest (CLAUDE.md, decisione 2); il VIX e il VXN
  ricostruiti dalla catena CME di ATAS esistono solo da poco e cadono nel fuori campione di NQ (dal 2025).
  Servirebbe la richiesta esplicita dell'utente.
- **Se richiesto**, il disegno corretto: variazione del VIX (o del VXN) **misurata alla stessa ora** del
  rendimento di NQ che si vuole spiegare; regressione con i ritardi di NQ **e** di ES (o dell'S&P) di ieri;
  una osservazione per giornata; nullo con giornate rimescolate; solo dentro il campione (fino al 2024);
  le varianti (soglia, ritardi, VIX o VXN) dichiarate prima e registrate. La regola di trading del paper
  si scarta: con orari allineati e' da rifare da capo.
- **Cosa non si rifa'**: le borse estere (fuori dallo strumento e dalle fonti).

## Cosa Ne Prende Il Framework

- **Nessuna regola operativa**: il finding e' giornaliero, su borse estere, senza fuori campione e senza
  costi, e la parte forte si puo' spiegare con i fusi orari (lettura nostra, non provata).
- **Un controllo di metodo da tenere** per ogni prova fra indici o fra mercati: allineare gli istanti, non
  le date; mettere il mercato guida anche ritardato; diffidare di un "anticipo" che e' forte dove gli orari
  non coincidono e si riduce di molto dove coincidono.
- **Per la pagina X-Ray**: nessun uso del VIX come anticipatore di NQ trova appoggio qui.

## Dubbi Di Lettura Per Il Verificatore

- **Tab. 2**: `pdftotext` sfasa di una riga la colonna dei nomi degli indici rispetto ai numeri. Le righe
  sono state attribuite sull'immagine (pagina 18 ruotata): Argentina 0,0733 / -0,0046 / 0,8775, Giappone
  -0,0739, ecc. Da ricontrollare almeno le righe di Messico e Nuova Zelanda e le stelle della colonna
  ΔVIX_d-1, che nell'immagine stanno a cavallo del taglio.
- **I conteggi** (18, 11, 17, 6, 15) sono rifatti sulle stelle delle tabelle: tornano con il testo per
  18, 11 e 17; i 6 della direzione inversa e i 15 della Tab. 4 sono nostri.
- **Tab. 4**: le colonne delle stelle sono "x - y ≠ 0" e "x - y > 0"; la colonna "x - y < 0" e' vuota per
  tutti. La didascalia di `pdftotext` scrive "x - y = 0": sull'immagine e' "≠".
- **La lettura dei fusi orari, del controllo ritardato mancante e del look-ahead della regola** e' nostra,
  non del paper: va giudicata come deduzione. Gli orari delle borse non sono nel paper.
- **Il SUR equivalente a due regressioni ordinarie** con regressori uguali e' una proprieta' generale, non
  scritta nel paper.
- La versione pubblicata non e' stata vista: se il verificatore la trova, i numeri vanno confrontati.

## La Verifica

**9 novembre 2026**, fase 4, da un agente diverso da quello che ha scritto l'analisi. Fonte: il PDF, sha256
confermato (`41b9313f...57541476`), testo con `pdftotext -layout`, e le pagine 8, 9, 16, 18, 19 e 20 (formule,
Tab. 1-4) rese come immagini con pypdfium2 nella scratchpad, la Tab. 2 anche ingrandita a 5x. Nessuna
istruzione rivolta a chi legge nel PDF: la nota 9 e' un'avvertenza legale, come scritto sopra.

**Circa 115 affermazioni controllate** (ogni numero delle Tab. 1-4 citato qui, i conteggi, le citazioni di
sezione e pagina, i "l'autore dice", le formule (1)-(5), la scheda bibliografica). **Confermate circa 103**,
**corrette o riformulate 12**, **nessuna tolta**, **un limite aggiunto** (il 12).

**I dubbi del lettore, sciolti sull'immagine:**

- **Tab. 2**: l'attribuzione delle righe e' giusta. `pdftotext` sposta di una riga la colonna Index e anche
  la colonna χ² (l'India risulta vuota e il 3.834,85 del Regno Unito finisce su una riga a parte); i numeri
  delle altre colonne stanno sulla riga del paese giusto. Messico: -0,0048 ** a d-1, +0,0008 a d-2, S&P
  0,6753 ***, N 4.265. Nuova Zelanda: -0,0013 senza stelle a d-1, 0,0000 a d-2, Crisis -0,0006 **, N 2.801.
  Le stelle di ΔVIX_d-1: nessuna per Argentina e Nuova Zelanda, ** per il Messico, *** per gli altri 17.
- **I conteggi**: 18 (d-1), 11 (d-2 all'1%), 17 (Granger VIX -> indice), 6 al 5% o meglio nella direzione
  inversa (Brasile, Canada, India, Messico, ESTX50, Regno Unito; Francia e Giappone al 10%), 15 in Tab. 4:
  tutti confermati. Anche i 7 coefficienti autoregressivi positivi e i 10 negativi del testo [p. 8] tornano.
- **Tab. 4**: la didascalia e l'intestazione dicono "x - y ≠ 0"; la colonna "x - y < 0" e' vuota. Medie
  della regola: Giappone 0,4337, Hong Kong 0,4084 (sopra il 40%, come dice il testo a p. 10).
- **Le deduzioni** (fusi orari, controllo dell'S&P di ieri mancante, look-ahead della regola, SUR uguale a
  due regressioni ordinarie): nessuna e' nel paper, che non nomina mai orari o fusi. Sono deduzioni
  ragionevoli e ora sono scritte come tali ovunque; tutte dipendono da un presupposto non detto nel paper
  (le serie allineate per data di calendario), ora dichiarato. Sul SUR: le equazioni (2) e (3) hanno
  davvero gli stessi regressori (verificato sull'immagine di p. 9), quindi la proprieta' si applica.

**Le correzioni:**

1. **La piu' grave**: "il vantaggio di un giorno **sparisce** quando gli orari coincidono" (Cosa Vale, e
   nel Framework) e' smentito dalle tabelle: nelle Americhe Brasile, Canada e Cile hanno il VIX di ieri
   significativo all'1% (Tab. 2) e Granger significativo (7,06 **, 34,98 ***, 17,49 ***, Tab. 3). Ora: "si
   riduce di molto", come lettura nostra.
2. "In Parole Semplici" presentava fusi e look-ahead come fatti ("i numeri lo tradiscono", "compra un
   prezzo che c'era gia'"): ora sono dichiarati come lettura nostra, coerente con i numeri ma non provata.
3. Cosa Non Regge 1: "i numeri tornano" diventa "coerenti, non una prova"; dichiarato il presupposto delle
   date di calendario; le forbici di Asia ed Europa dicono che escludono la Nuova Zelanda; aggiunti i tre
   paesi americani significativi e India e Indonesia solo al 10% in Tab. 4.
4. Cosa Non Regge 2: non era marcato come deduzione; ora separa il fatto (solo ΔSP500_d, p. 9) dalla
   deduzione; l'effetto "di Rapach" e' quello che il paper gli attribuisce, non verificato su Rapach.
5. Cosa Non Regge 3: aggiunta la didascalia della Tab. 4 e il presupposto delle date.
6. Cosa Non Regge 7: "quindi senza correzione per l'eteroschedasticita'" era affermato; il paper non dice
   che errori usa. Ora condizionale.
7. Risultato 2: aggiunti Belgio e Malesia negativi al 10% a d-2.
8. Risultato 1: chiarito che la forbice -0,0068 / -0,0739 e' dei 17 all'1% (il Messico, -0,0048, e' fuori).
9. Cosa Non Regge 11: la sede in rivista viene dalla fase 1 e non e' verificata qui.
10. Cosa Vale: "il livello giornaliero" della paura implicita era sbagliato; il paper usa la variazione
    logaritmica giornaliera.
11. Framework: "la parte forte e' spiegata dai fusi" diventa "si puo' spiegare (lettura nostra)".
12. **Limite aggiunto** (12): la crisi entra solo come costante, mentre lo stesso autore riporta che in
    Sarwar (2014) il VIX predice solo nei periodi di crisi, e fissa il criterio "coefficienti tutti negativi
    per tutti i paesi" [p. 6], che l'Indonesia a d-2 manca.

**Resta incerto**: la versione pubblicata in Quantitative Finance and Economics non e' stata vista; gli
orari di chiusura delle borse usati nelle deduzioni non sono nel paper (sono conoscenza generale, da
controllare se la lettura dei fusi diventa una prova); come il paper allinei le date resta non detto.
