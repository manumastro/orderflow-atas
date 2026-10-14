---
chiave: baltussen-2021-hedging-demand-and-market-intraday-momentum
sigla: BDLM
titolo: "Hedging demand and market intraday momentum"
autori: [Guido Baltussen, Zhi Da, Sten Lammers, Martin Martens]
versione_letta: versione pubblicata, Journal of Financial Economics 142 (2021) 377-403
fonte: Journal of Financial Economics
identificativo: "10.1016/j.jfineco.2021.04.029"
url: https://www3.nd.edu/~zda/intramom.pdf
tipo: rivista
revisione_fra_pari: si
pagine: 27
pdf: baltussen-2021-hedging-demand-and-market-intraday-momentum.pdf
pdf_sha256: 623ba82490acd01546f568cf582de5a9b45dfbd97731cd548cb119d7a0acec57
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; dati Tick Data LLC, OptionMetrics, SqueezeMetrics e Bloomberg, a pagamento"
---

# Baltussen, Da, Lammers, Martens 2021: L'Analisi

*Hedging demand and market intraday momentum*, Guido Baltussen (Erasmus e Robeco), Zhi Da (Notre
Dame), Sten Lammers (Erasmus), Martin Martens (Robeco). Journal of Financial Economics 142 (2021),
pagine 377-403; ricevuto a gennaio 2020, accettato a settembre 2020. **Revisionato.** Letto per intero
il 14 ottobre 2026 nella versione impaginata della rivista, dalla pagina di Zhi Da. Il testo estratto
con `pdftotext` mescola le colonne di quasi tutte le tabelle: i numeri qui sotto sono quelli che si
leggono senza ambiguita', o che il testo ripete; dove abbiamo dovuto ricostruire una tabella lo
diciamo. Citazioni `[BDLM §n]`, `[BDLM Tab. n]`.

## I Termini

- **Momento intraday di mercato**: il rendimento dell'ultima mezz'ora della seduta si muove nella
  direzione del resto della giornata. E' un momento **nel tempo** di un indice, non fra titoli.
- **Gamma, delta hedging**: la gamma dice quanto cambia il delta (la sensibilita' al sottostante)
  di un'opzione quando il prezzo si muove. Chi si copre in delta ed e' **corto di gamma** deve
  comprare quando il prezzo sale e vendere quando scende: compra e vende **con** il movimento.
- **NGE** (*net gamma exposure*; nell'introduzione anche *negative gamma exposure*): la gamma netta
  dei market maker sulle opzioni dell'S&P 500, calcolata con un'ipotesi sul segno [§4.1].
- **Le cinque parti della giornata** [§1]: ON (dalla chiusura di ieri all'apertura), FH (la prima
  mezz'ora), M (dalla fine di FH a un'ora dalla chiusura), SLH (la penultima mezz'ora), LH (l'ultima
  mezz'ora). **ONFH** = ON + FH, il segnale di Gao e altri (2018); **ROD** (*rest of day*) = ON + FH +
  M + SLH, cioe' dalla chiusura di ieri a 30 minuti dalla chiusura di oggi.
- **R quadro fuori campione** (R2OOS): quanto la previsione batte la media storica calcolata solo
  sul passato; positivo vuol dire meglio della media. Il test di **Clark e West** dice se la
  differenza e' significativa.

## La Domanda

Il rendimento della giornata predice quello dell'ultima mezz'ora su tutti i mercati di future, non
solo sulle azioni? E il motore e' la copertura di chi e' corto di gamma (market maker di opzioni, ETF
a leva), piuttosto che il ribilanciamento o l'informazione che arriva tardi?

## Il Metodo

**I dati** [§2, Tab. 1, App. A]: barre di 1 minuto da Tick Data LLC su **62 future**: 17 indici
azionari, 16 titoli di Stato, 21 materie prime, 8 valute; da dicembre 1974 a maggio 2020; il volume
solo dal 2003. Contratto piu' scambiato, rollover quando il volume del successivo lo supera. Filtri:
via prezzi non positivi, giorni non lavorativi, **giorni a chiusura anticipata**, giorni sotto i 100
contratti. Per gli indici l'orario e' quello del mercato a pronti: **ES e NQ 9:30-16:00 ET**. Per gli
altri, apertura e chiusura scelte dai picchi di volume ("disponibili su richiesta").

**Le regressioni** [§3]: una osservazione al giorno per mercato.

```text
r_ONFH = P(apertura + 30) / P(chiusura di ieri) - 1            (1)
r_ROD  = P(chiusura - 30) / P(chiusura di ieri) - 1            (2)
r_LH   = a + b r_ONFH + e                                      (5)  come Gao e altri
r_LH   = a + b1 r_ONFH + b2 r_M + b3 r_SLH + e                 (6)
r_LH   = a + b r_ROD + e                                       (7)  la specifica del paper
r_LH   = a + b1 r_ONFH + b2 r_ROD + e                          (11) la gara fra i due
```

Errori standard: nelle regressioni raggruppate per classe, **raggruppati per tempo e per mercato**
(Cameron, Gelbach, Miller); per il singolo contratto, **Newey-West**. I coefficienti e gli R quadro
nelle tabelle sono moltiplicati per 100. Il fuori campione e' **ricorsivo**: finestra che si allarga,
almeno 500 osservazioni, confronto con la media storica fino al giorno prima.

**La strategia** [§3.5, eq. 12]: alle 15:30 si va lunghi per l'ultima mezz'ora se il segnale e'
positivo, corti se e' negativo, e si chiude alla chiusura. Portafoglio 1/N per classe. **Senza costi.**

**La gamma delle opzioni** [§4.1, eq. 13-15]: dalle open interest (OI) di tutte le opzioni
sull'S&P 500, OptionMetrics 1996-2017, SqueezeMetrics fino a maggio 2020 (verificato uguale su
2002-2017). Quattro ipotesi: tutte le opzioni passano dai coperturisti; le call le vendono gli
investitori e le comprano i market maker; le put le comprano gli investitori e le vendono i market
maker; i market maker si coprono esattamente.

```text
NGE call = gamma x OI x 100 x P        NGE put = gamma x OI x (-100) x P
NGE      = (somma su tutti gli strike e le scadenze) / valore di mercato dell'S&P 500
```

Il simbolo della gamma sparisce nel testo estratto; la definizione a parole e' chiara. Se P compare
una o due volte **va ricontrollato sul PDF**.

**Gli ETF a leva** [§4.2, eq. 16-17]: il ribilanciamento di fine giornata e' `NAV x (x^2 - x) x r`,
con x la leva; la quota degli ETF a leva sul mercato (*IndexingLETF*) entra in interazione con r_ROD.
Dati Bloomberg, giugno 2006 - maggio 2020.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | Volume a U in tutte e quattro le classi: alto dopo l'apertura e prima della chiusura (2003-2020) | Fig. 1 |
| 2 | **Azioni, tutti i 17 future insieme**: r_ONFH -> r_LH coefficiente 4,86 (t 6,52), R quadro 1,49%, **fuori campione -1,71%**; r_ROD -> r_LH 4,18 (t 7,29), R quadro 2,45%, **fuori campione 2,88%**. Nella (6) contano tutte e tre le parti: t 6,46 (ONFH), 4,43 (M), 4,82 (SLH) | §3.1, Tab. 2 |
| 3 | **ES** da solo (aprile 1982 - maggio 2020, 9.535 giornate, Newey-West): r_ONFH 5,24 (t 3,38), R quadro 0,98%, **fuori campione -1,68%**; r_ROD 6,18 (t 4,97), R quadro 3,41%, fuori campione 2,29% | Tab. B1 |
| 4 | **NQ** da solo (aprile 1996 - maggio 2020, 6.017 giornate, Newey-West): r_ONFH 7,45 (t 5,33), R quadro 2,01%, fuori campione 1,83%; r_ROD 6,36 (t **7,97**), R quadro **4,10%**, fuori campione **3,76%** | Tab. B1 |
| 5 | Altre classi, r_ROD: titoli di Stato 1,90 (t 5,97), materie prime 1,21 (t 3,63), valute 0,72 (t 4,57). Fuori campione positivo e significativo su 14 future azionari su 17, 11 su 16 di Stato, 40-50% di materie prime e valute | Tab. 2, App. B |
| 6 | **La gara** (azioni): insieme, r_ONFH 1,14 (t 1,39, non significativo), r_ROD 3,65 (t 5,28). Quando i due hanno segno opposto, r_ONFH da solo ha il **segno sbagliato** (-3,71, t -2,76) e r_ROD resta positivo (3,59, t 3,54). Le valute sono l'eccezione | §3.2, Tab. 3 |
| 7 | Non e' la stagionalita' di Heston e altri: l'ultima mezz'ora di ieri, aggiunta, ha segno **negativo** sulle azioni (circa -6,7, t -2,7) e non cambia il coefficiente di r_ROD | §3.3, Tab. 4 |
| 8 | Stabile nel tempo: azioni, r_ROD 5,96 (t 4,80) nel 1974-1999 e 3,98 (t 6,65) nel 2000-2020; il coefficiente si e' ridotto | §3.4, Tab. 5 |
| 9 | **Strategia, azioni (1/N su 17 future, lordo)**: segnale r_ROD rendimento 6,86% annuo, volatilita' 3,96%, **Sharpe 1,73**, giorni giusti 55%; segnale r_ONFH Sharpe 1,07; i due d'accordo (altrimenti fermi) Sharpe 1,60 con il 61% di giorni giusti; sempre lunghi nell'ultima mezz'ora Sharpe 0,11. Sharpe del segnale r_ROD per classe: 1,73 azioni, 1,62 Stato, 1,42 materie prime, 0,87 valute | §3.5, Tab. 6 |
| 10 | **Gamma delle opzioni** (future S&P 500, gennaio 1996 - maggio 2020, Newey-West): con NGE del giorno prima >= 0, r_ROD 0,82 (t 1,03), R quadro 0,05%: **niente momento**; con NGE < 0, r_ROD **6,63 (t 4,78)**, R quadro 3,58%. Giornate: 2.930 negative, 3.158 positive | §4.1, Tab. 7 |
| 11 | In una regressione con l'interazione, piu' la NGE e' negativa piu' il momento e' forte: interazione NGE x r_ROD -123,04 (t -3,42); regge in differenze (-119,79, t -4,06). Tabella storpiata: l'attribuzione delle colonne e' nostra | §4.1, Tab. 8 |
| 12 | **ETF a leva**: azioni, IndexingLETF x r_ROD 1,79 (t 2,20), in differenze 2,94 (t 3,10); titoli di Stato 0,67 (t 1,96), in differenze 0,45 (t 1,31, non significativo). Nella sezione trasversale, piu' ETF a leva vuol dire t di r_ROD piu' alta | §4.2, Tab. 9, Fig. 4 |
| 13 | **Salti intraday** (mezz'ora oltre il 10/90 percentile): il rendimento fino al salto predice il resto della giornata (azioni 6,74, t 5,03), poco il tratto fino alla penultima mezz'ora (1,50, t 1,40), molto l'ultima (5,16, t 7,51). La copertura dopo un salto e' parziale, il grosso si fa alla fine | §4.3, Tab. 10 |
| 14 | **Inversione nei giorni dopo** (azioni): r_LH di oggi su 1, 2, 3 giorni: -14,51 (t -1,70), -29,05 (t -3,16), -27,98 (t -2,61). Circa un terzo dell'ultima mezz'ora torna indietro in due giorni; sulle valute non e' significativo | §4.4, Tab. 11, Fig. 5 |
| 15 | **Dopo le 16:00 non c'e' niente** (S&P 500, aprile 1982 - maggio 2020, Newey-West): sull'indice a pronti r_ROD predice l'ultima mezz'ora (5,98, t 4,78), gli ultimi 15 minuti (2,83, t 3,99) e gli ultimi 5 (1,94, t 5,03); sul future, il tratto 16:00-16:15 no (-0,37, t -0,97). Chi sa qualcosa potrebbe scambiare fino alle 16:15; chi si copre ha finito alle 16:00 | §4.4, Tab. 12 |

**Il perche'** degli autori [§1, §4.3]: chi e' corto di gamma (market maker di opzioni, fornitori di
ETF a leva, vecchie assicurazioni di portafoglio, swap di varianza) deve comprare dopo i rialzi e
vendere dopo i ribassi. Lo fa **soprattutto alla fine**, per cinque ragioni: con costi fissi conviene
coprirsi solo in parte durante la giornata (Clewlow e Hodges); la liquidita' e' migliore alla
chiusura; la notte si vuole essere coperti del tutto; il capitale e i margini si calcolano sul delta
di chiusura; gli ETF a leva promettono un multiplo del rendimento da chiusura a chiusura.

**Le alternative scartate**: l'informazione che arriva tardi (Gao e altri) non spiega l'inversione
dei giorni dopo (risultato 14) ne' il silenzio dopo le 16:00 (risultato 15); il ribilanciamento
infrequente non spiega perche' conti tutta la giornata e non solo la prima mezz'ora (risultato 6).

**Cosa lo ucciderebbe**, dedotto dal perche': un momento uguale nei giorni di gamma positiva e
negativa; nessuna inversione nei giorni dopo; predicibilita' che continua dopo le 16:00 sul future.

## Cosa Abbiamo Riprodotto

Niente, ancora. La parte sulle barre si puo' rifare con i dati di ATAS (vedi sotto).

## Cosa Non Regge, O Non Si Dice

1. **I costi non ci sono.** La strategia gira ogni giorno e gli autori lo ammettono: "potrebbe non
   essere sfruttabile per molti investitori". Dicono che sull'S&P 500 resta uno Sharpe netto positivo
   con un tick di costo, **senza dare il numero** [§3.5]. Lo Sharpe di 1,73 e' lordo e su un
   portafoglio di 17 future, non su ES da solo.
2. **Il segno della gamma e' un'ipotesi.** NGE assume che i market maker siano lunghi di tutte le
   call e corti di tutte le put. Gli autori ammettono [nota 8] che mancano le opzioni fuori borsa
   (OTC) e le posizioni vere dei clienti; e la NGE risulta **positiva nella maggior parte dei giorni**,
   il contrario di "i market maker sono di solito corti di gamma" con cui si apre il paper. E' la
   stessa convenzione che Barbon e Buraschi usano sugli indici, e che sui titoli singoli, dove
   misurano il segno, risulta rovesciata.
3. **Il tempo della NGE.** La tabella 7 usa la NGE del giorno prima; il testo della tabella 8 dice
   "NGE_t" e poi "r_ROD,t-1". Se la NGE del giorno t usa l'OI di fine giornata t, nella tabella 8 c'e'
   look-ahead. Non si capisce dal testo.
4. **Scelte prese guardando i dati.** Gli orari dei mercati non azionari si scelgono dai picchi di
   volume, "disponibili su richiesta". La soglia dei salti (10/90 percentile) e' sul campione intero.
   Il segnale r_ROD nasce dopo aver visto i risultati di Gao e altri. Le specifiche provate sono molte
   (5 segnali, 4 classi, 62 contratti, sottocampioni) e nessuna correzione per test multipli. Il fuori
   campione ricorsivo, almeno, non guarda avanti.
5. **Una tabella duplicata**: il pannello C della tabella D2 (materie prime 2000-2020) e' identico
   cifra per cifra al pannello C della D1 (1974-1999). Un errore di impaginazione, non segnalato.
6. **Le tabelle sono storpiate nel testo estratto**: le colonne di materie prime e valute della
   tabella 6 le abbiamo ricostruite e verificate con il rapporto rendimento/volatilita' = Sharpe; le
   tabelle 8, 9 e 12 hanno righe mescolate. Prima di usare un numero di quelle tabelle in un
   documento del framework, si guarda il PDF.
7. **La significativita' e' per giornata**, ed e' quello che chiede il nostro metodo: un'osservazione
   per giorno e per mercato, errori raggruppati per tempo o Newey-West. Ma il confronto e' con zero e
   con "sempre lunghi", **non con un modello nullo** nel nostro senso (la stessa regola sul mercato
   rimescolato).
8. **Il campione finisce a maggio 2020**: prima della crescita delle opzioni a scadenza giornaliera
   (0DTE), che cambiano dove e quando i market maker si coprono. Negli ultimi quattro mesi (febbraio -
   maggio 2020, il Covid) la strategia va "forte", dicono gli autori [§3.5]: il campione finisce su
   un periodo eccezionale.
9. **Il dopo-16:00 dipende dagli orari di allora**: il future S&P 500 si fermava alle 16:15. Gli orari
   di oggi e il momento in cui il future fa il prezzo di regolamento vanno verificati sui nostri dati,
   non presi dal paper.
10. **Conflitti**: due autori lavorano per Robeco, un gestore. Non e' dichiarato un conflitto;
    SqueezeMetrics ha fornito dati ed e' ringraziata.

## Cosa Si Puo' Rifare Su ES, Con ATAS

Lo strumento del filone opzioni e' ES (CLAUDE.md, decisione 2). Cosa abbiamo: barre di 1 minuto di
ES con volume e delta (volume aggressivo in acquisto meno in vendita) dal Data Bridge, con una
profondita' di storia **da misurare**; gli indicatori Options Chain sul nostro feed Rithmic (OI
Profile, Key Levels, Expected Move, GEX Profile con Zero Gamma), **senza storia**: registrano solo da
quando sono sul chart; gli indicatori Options X-Ray (dati OptionsDepth sulle opzioni SPX, aggiornati
ogni minuto, posizioni dei market maker per gruppo di partecipanti, GEX heatmap, Hedge Pressure),
solo su ES, con una storia che **non conosciamo**. Vedi
[`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md).

**Con le sole barre (niente opzioni):**

- **Il test semplice del momento intraday** (r_ROD dalle 16:00 di ieri alle 15:30 -> r_LH dalle 15:30
  alle 16:00), e la sua versione con la prima mezz'ora (r_ONFH), richiedono **solo le barre**.
  Lo stesso vale per la gara fra i due, l'inversione nei giorni dopo e il tratto 16:00-16:15.
- **Quante giornate servono.** Nostro calcolo, non del paper: se l'effetto fosse quello medio del
  campione e la t crescesse con la radice del numero di giornate, su NQ (t 7,97 su 6.017 giornate)
  servirebbero circa **380 giornate** per arrivare a t = 2; su ES (t 4,97 su 9.535) circa **1.550**.
  Un anno di storia (circa 250 giornate) darebbe t intorno a 1,6 su NQ e 0,8 su ES. Con 22 sedute
  complete di NQ in archivio non si prova niente. Prima di tutto si misura quanta storia a 1 minuto
  ATAS da' per ES e NQ.

**Cosa serve storia di opzioni che non abbiamo:**

- La prova centrale del paper (momento solo nei giorni di gamma negativa, risultato 10) vuole anni di
  NGE giornaliera. ATAS Chain non la ricostruisce all'indietro: l'OI per strike del passato non c'e'.
  X-Ray la avrebbe solo se la storia di OptionsDepth arriva attraverso ATAS: **da verificare**.

**Cosa permetterebbe la registrazione in avanti:**

- Ogni giorno, alle 15:30, il segno della GEX totale e la posizione del prezzo rispetto allo Zero Gamma
  (Chain), e da X-Ray la posizione dei market maker e la Hedge Pressure. Poi la regressione della
  tabella 7, divisa per segno. Nostro calcolo: nel paper la t con NGE negativa e' 4,78 su circa 2.930
  giornate; con 125 giornate negative (un anno, se fossero meta') la t attesa sarebbe intorno a 1.
  **Un anno di registrazione basta a vedere la direzione, non a provarla.**
- **X-Ray misura il segno invece di assumerlo**: e' la prova che il paper non poteva fare. Se i giorni
  in cui X-Ray dice "market maker corti" non coincidono con quelli in cui la convenzione del paper dice
  NGE negativa, la convenzione e' sbagliata.
- **Il delta di ATAS** aggiunge una verifica del meccanismo che il paper non ha: se chi si copre
  compra e vende con la giornata, nell'ultima mezz'ora il delta cumulato dovrebbe avere lo stesso segno
  di r_ROD, e piu' nei giorni di gamma negativa.

**Da verificare prima**: se il Data Bridge puo' leggere i valori calcolati da un altro indicatore;
se le opzioni lette da Chain su ES sono quelle CME sul future; gli orari di ES dopo le 16:00 oggi.

## Il Legame Con Il Momento Intraday Su NQ

Lo studio [`momento-intraday`](../../../studi/momento-intraday/1-idea.md) parte da Gao e altri (2018),
attraverso il transcript, e prova un'uscita dal range 9:30-10:00 con chiusura alle 15:30. BDLM e'
l'estensione dello stesso finding, e dice tre cose che lo riguardano:

1. **Il segnale migliore non e' la prima mezz'ora ma tutta la giornata fino alle 15:30.** Su NQ
   r_ONFH funziona (fuori campione 1,83%), r_ROD di piu' (3,76%). Su ES r_ONFH e' **negativo** fuori
   campione (-1,68%).
2. **Il finding riguarda l'ultima mezz'ora (15:30-16:00)**, e la regola dello studio chiude proprio
   alle 15:30: opera sul tratto M, che il paper non prova a predire dalla prima mezz'ora. Sono due
   ipotesi diverse, e lo studio dovrebbe dirlo nella sua scheda.
3. **Se il motore e' la copertura**, la condizione naturale e' la gamma dei dealer, e l'effetto
   dovrebbe sparire nei giorni di gamma positiva.

Queste sono osservazioni per la scheda dello studio, che si aggiorna a parte.

## Cosa Ne Prende Il Framework

- **Il finding da provare**: r_ROD -> r_LH, una osservazione per giornata, su NQ e su ES, con il
  nullo e la significativita' per giornata. Varianti naturali, da dichiarare prima: r_ONFH, r_ROD, i
  due d'accordo.
- **Il disegno delle prove di meccanismo**: inversione nei giorni dopo; tratto dopo la chiusura del
  mercato a pronti; condizione sul segno della gamma, misurata prima.
- **Il fuori campione ricorsivo** con il test di Clark e West come seconda misura accanto al nostro
  fuori campione unico.
- **Nessuna regola operativa**: il paper non prova i costi, e i numeri sono lordi.
