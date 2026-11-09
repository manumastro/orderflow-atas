---
chiave: le-coz-2024-when-is-cross-impact-relevant
sigla: LMCB
titolo: "When is cross impact relevant?"
autori: [Victor Le Coz, Iacopo Mastromatteo, Damien Challet, Michael Benzaquen]
versione_letta: arXiv 2305.16915 v2 (timbro arXiv 26 marzo 2024, il PDF porta "Dated: March 27, 2024"); la versione impaginata della rivista non e' stata letta
fonte: arXiv (q-fin.TR); pubblicato in Quantitative Finance 24(2), 265-279 (dati della rivista da Crossref, fase 1)
identificativo: "arXiv:2305.16915v2; doi 10.1080/14697688.2024.2302827; SSRN 4460293"
url: https://arxiv.org/pdf/2305.16915v2
tipo: rivista (letto il preprint arXiv della versione accettata)
revisione_fra_pari: si
pagine: 18
pdf: le-coz-2024-when-is-cross-impact-relevant.pdf
pdf_sha256: 63d78f1c18f2c4374d2622bdc7476a3da945cf56c01b9976aaaebf02be6a52d9
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; i dati (tick e quote di 500 strumenti USA) non hanno fornitore dichiarato"
---

# Le Coz, Mastromatteo, Challet, Benzaquen 2024: L'Analisi

*When is cross impact relevant?*, Victor Le Coz (Ecole polytechnique, Quant AI Lab), Iacopo
Mastromatteo (Capital Fund Management), Damien Challet (CentraleSupelec, Universite' Paris-Saclay),
Michael Benzaquen (Ecole polytechnique, CFM). Letto per intero il 9 novembre 2026 nella versione
arXiv v2, 18 pagine, appendici comprese. **Revisionato** (Quantitative Finance, 2024): la versione
della rivista non l'abbiamo letta.

Su come e' stato letto: `pdftotext -layout` mescola le figure con il testo e perde le lettere
greche, e il lettore di PDF a pagine non funziona su questo PC (manca `pdftoppm`). Le formule le
abbiamo ricontrollate su una seconda estrazione (`pdftotext -raw -enc UTF-8`), che le lettere greche
le conserva. **Quasi tutti i risultati sono figure** (mappe di densita', mappe di calore), non
tabelle: i numeri qui sotto sono quelli **scritti nel testo**, o i valori delle celle che si leggono
nelle mappe di calore. Dove abbiamo ricostruito noi a quale cella va un numero, lo diciamo.
Citazioni `[LMCB §n]`, `[LMCB Fig. n]`, `[LMCB App. B]`.

**Nessuna istruzione** nel PDF: niente testo che chieda di fare qualcosa.

## I Termini

- **Impatto sul prezzo** (*price impact*): gli ordini a mercato spostano il prezzo. Qui si misura
  sommando in una finestra di durata tau i volumi degli scambi, **positivi se l'aggressore compra e
  negativi se vende**. E' quello che ATAS chiama **delta** della barra [§I, §III].
- **Impatto incrociato** (*cross-impact*): il flusso degli ordini su un titolo che muove il prezzo
  di **un altro** titolo [§I].
- **Il modello lineare** [§III, eq. 1]: `dp_t = Lambda_t q_t + eta_t`. Qui `dp_t` e' il vettore
  delle variazioni di prezzo di n titoli nella finestra `[t, t+tau]`, `q_t` il vettore dei loro
  flussi netti nella **stessa** finestra, `Lambda_t` la matrice n x n dell'impatto incrociato ed
  `eta_t` un rumore a media zero. **E' una spiegazione nello stesso intervallo, non una previsione**:
  il flusso della finestra spiega il prezzo della stessa finestra.
- **Tre modelli** [§III.A, eq. 7-9]. Sigma e' la covarianza delle variazioni di prezzo, Omega quella
  dei flussi, R la covarianza incrociata prezzi-flussi (la "matrice di risposta"), Y un fattore di
  scala comune (*Y-ratio*):
  - **diagonale**: `Y diag(R) diag(Omega^-1)`. Ogni titolo risponde solo al proprio flusso
    (eq. 11: `dp_i = lambda_i q_i + eta_i`). E' il caso **senza** impatto incrociato;
  - **ML** (massima verosimiglianza): `Y R Omega^-1`, cioe' una regressione multipla dei prezzi su
    tutti i flussi;
  - **Kyle** (multidimensionale, del Molino e altri 2018): `Y (Omega^-1/2)' sqrt((Omega^1/2)' Sigma
    Omega^1/2) Omega^-1/2`. Impone l'assenza di arbitraggio (non si guadagna comprando e rivendendo).
    **Il simbolo della radice esce come un carattere storpiato** (`»`). **Verificato (fase 4)**: il
    carattere viene dal font matematico esteso `Yhcmex` del PDF, e il suo disegno decodificato e' un
    segno di radice alto (il tratto lungo che sale e il piccolo uncino); il §II definisce `sqrt(A)` come
    la radice quadrata di matrice (l'unica matrice simmetrica semidefinita positiva con quadrato A),
    distinta da `A^1/2` (una matrice qualunque con `A^1/2 (A^1/2)' = A`). La lettura e' giusta.
- **R quadro generalizzato** `R2(M)` [§IV.B, eq. 16]: `1 - somma (dp - dp^)' M (dp - dp^) / somma
  dp' M dp`. M sceglie quali errori contano; quasi sempre gli autori usano `M = I_sigma_i`, cioe'
  **solo l'errore sul titolo che si vuole spiegare**, diviso per la sua varianza. Eccezione: le Fig.
  10 e 11 usano `M = I_sigma`, l'errore **su tutti e due** i titoli (didascalia della Fig. 10:
  "accounting for errors relative to both assets").
- **L'accuratezza aggiunta** `dR2(M) = R2(M) - R2_diag(M)` [eq. 17]: quanto il modello con impatto
  incrociato spiega **in piu'** del modello in cui ogni titolo risponde solo al proprio flusso. E' il
  numero che dice se l'impatto incrociato serve.
- **Il massimo e la scala ottima**: `R2*` e `dR2*` sono i massimi su tutte le durate di finestra
  provate; `tau*` e' la durata a cui si ottengono [App. A, Tab. I].
- **Titolo spiegato i, titolo che spiega j**: nelle coppie, si misura l'errore su i usando anche il
  flusso di j [§IV.B].
- **Liquidita'** [§IV.B.2, §V.D]: `sigma_i x omega_i`, la volatilita' del prezzo per la volatilita'
  del flusso in una finestra di 5 minuti, in dollari: **quanto si guadagna o si perde di solito in 5
  minuti** su quel titolo. Piu' e' grande, piu' il titolo e' liquido.
- **Frequenza degli scambi** f: il numero medio di scambi al secondo durante l'orario di mercato
  [§V.B.1].
- **Effetto Epps**: le correlazioni misurate su finestre troppo brevi escono piu' basse del vero
  [§V.A, §V.C].
- **Modello propagatore**: il modello in cui ogni scambio passato continua a pesare sul prezzo, con
  un effetto che si spegne nel tempo [§VII, App. B.2, eq. B4]. Gli autori non lo usano perche' costa
  troppo calcolarlo su 500 titoli.

## La Domanda

**Quando il flusso degli ordini di un titolo aiuta a spiegare il prezzo di un altro**, oltre al flusso
del titolo stesso? Gli autori la scompongono in quattro caratteristiche: la durata della finestra, la
frequenza degli scambi, la correlazione fra i due titoli e la loro liquidita' [§I]. Poi applicano le
risposte ai titoli di Stato USA, a pronti e future [§VI].

## Il Metodo

**I dati** [§IV.A]: scambi e quote tick per tick di **500 strumenti** quotati negli Stati Uniti:
azioni, titoli di Stato, future sui titoli di Stato e **future su indici azionari**, dal **2017 al
2022**. **Quali future su indici non si dice**: nel testo e nelle figure compare solo il future
E-mini S&P. **Il Nasdaq-100 non e' nominato in nessun punto del paper.** Non si dichiarano il
fornitore dei dati, gli orari usati oltre a "durante l'orario di mercato" [§V.B.1], ne' come si
assegna il lato dell'aggressore. Il prezzo e' il **punto medio** fra denaro e lettera (*mid-price*),
preso all'apertura di ogni finestra [§III].

**Dentro e fuori campione** [§IV.A]: per ogni anno si stima sull'anno precedente (dentro) e si misura
sull'anno stesso (fuori); i risultati si aggregano su tutti gli anni.

**Le coppie** [§IV.A]: ogni anno 20.000 coppie su "120.000" possibili, scelte per coprire in modo
uniforme tutte le correlazioni (secchi della correlazione larghi 0,002%, la prima coppia di ogni
secchio); in tutto 100.000 coppie-anno. Solo il **7% delle coppie** ha una correlazione sopra il
50%, e molte figure si limitano a quelle [didascalie delle Fig. 3-8, 12-14; nelle Fig. 7, 8, 12, 13 e
14 solo il pannello b, quello delle coppie].

**La stima** [§IV.A, eq. 12-14]: le correlazioni (dei prezzi, dei flussi, e prezzi-flussi) si
assumono stabili e si stimano come medie di un anno. **Le volatilita' invece sono del giorno**: per
ogni giorno `t_k`, la volatilita' dei prezzi e dei flussi si calcola **con i dati di quello stesso
giorno**, e la covarianza del giorno e' "volatilita' del giorno x correlazione dell'anno x
volatilita' del giorno". Lo scopo dichiarato e' togliere l'eteroschedasticita' (la volatilita' che
cambia nel tempo).

**Le prove** [§V]: per ogni titolo e coppia si cerca la durata di finestra che massimizza R2 o dR2,
fuori campione, e si guarda come quel massimo e quella durata cambiano con la frequenza, la
correlazione e la liquidita'. **Le correlazioni** si misurano su finestre di **5 minuti**, un
compromesso fra effetto Epps e rumore [§V.C, Fig. 9]. Le linee nelle mappe di densita' sono stime di
Theil-Sen (una pendenza robusta, la mediana delle pendenze fra coppie di punti) con l'intervallo di
confidenza al 95% [§V.C.1].

**La significativita'** [App. B.1]: per un titolo solo, R2 e' l'R quadro di una regressione, e si
prova con un test F robusto all'eteroschedasticita' (MacKinnon e White, nella versione di Long ed
Ervin), su "m circa 10^3 (500 titoli per 5 anni)" test, con la correzione di Bonferroni e con quella
di Benjamini e Hochberg per i test multipli. **Le coppie e l'accuratezza aggiunta dR2 non hanno un
test**: le mappe di calore mostrano la media per secchio, con l'errore a due deviazioni standard, e
nascondono le celle con errore oltre il 50% della media [§V.D.3].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Un titolo solo** (Trimble, TRMB; dentro 2021, fuori 2022): il suo flusso spiega il prezzo sempre meglio allungando la finestra fino a un massimo **fra 10 e 100 secondi**, poi l'R2 scende fino a quasi niente a 1 ora (circa 1.400 scambi). Per un titolo solo i tre modelli coincidono | §V.A, Fig. 1 |
| 2 | Il calo sulle finestre lunghe **non** viene dall'impatto che si spegne: viene dal legame flusso-prezzo che e' **a forma di S** (lineare per flussi piccoli, piatto per quelli grandi), e le finestre lunghe hanno flussi grandi | §V.A |
| 3 | **Servono da 10 a 20 scambi per finestra**, per arrivare alla durata ottima: su un titolo solo, e **su entrambi** i titoli di una coppia | §V.B.1, Fig. 2, Fig. 5-6 |
| 4 | Un gruppo piu' piccolo ha la durata ottima molto piu' lunga, dove il modello regge fino a **500-5.000 scambi**: grandi azioni (AMZN, AAPL, TSLA) e **i future piu' scambiati, fra cui il future E-mini S&P** e il future sul decennale | §V.B.1, Fig. 2 |
| 5 | **La durata ottima dell'impatto incrociato dipende dalla frequenza del titolo che spiega**, non da quella del titolo spiegato (forma a triangolo di Fig. 3b, assente in Fig. 3a); nelle coppie conta **la frequenza minore** delle due, non la maggiore | §V.B.1, Fig. 3, Fig. 5-6 |
| 6 | Lo scarto fra la durata ottima del modello incrociato e quella del modello diagonale **cresce quando le due frequenze sono lontane** | §V.B.1, Fig. 4 |
| 7 | La frequenza degli scambi alza l'R2 massimo. La maggior parte dei titoli ha circa **0,5 scambi al secondo e R2 intorno al 25%** | §V.B.2, Fig. 7-8 |
| 8 | **Il modello ML fa meglio del modello di Kyle fuori campione**, anche se e' piu' flessibile e non vieta l'arbitraggio. Gli autori: costi e attriti (spread, commissioni) rompono in parte l'ipotesi di assenza di arbitraggio | §V.B.2, Fig. 7b |
| 9 | **La correlazione decide**: l'accuratezza aggiunta dR2* cresce in modo monotono con la correlazione della coppia, **da 0 a oltre il 5%**, per Kyle e per ML (pendenza di Theil-Sen 0,03 [0,03; 0,03] nella Fig. 10a). La durata ottima invece **non** dipende dalla correlazione (pendenza 0,0, Fig. 11a) | §V.C, Fig. 10-11 |
| 10 | Le correlazioni crescono con la durata della finestra (effetto Epps); fra le coppie della figura c'e' il future E-mini S&P con un altro future E-mini S&P (la legenda dice "E-mini S&P future (3M) - E-mini S&P future"; "(3M)", forse la scadenza successiva, a tre mesi, non e' spiegato da nessuna parte) e con il future sul decennale ("E-mini S&P future - bond future 10Y") | §V.C, Fig. 9 |
| 11 | **La liquidita'**: il modello diagonale fuori campione va da **sotto il 20% a circa il 30%** di R2 dai titoli meno liquidi ai piu' liquidi; la maggior parte dei titoli ha una liquidita' di **500 dollari per 5 minuti** e un R2 intorno al **25%**. Pendenze di Theil-Sen 4,44 [3,81; 5,07] per i titoli soli e 1,34 [1,14; 1,54] per le coppie (Fig. 13; **le unita' delle pendenze non sono dichiarate**) | §V.D.1, Fig. 12-13 |
| 12 | La durata ottima e la liquidita' non hanno un legame chiaro: due gruppi, uno grande con durate **intorno ai 90 secondi** e uno piu' piccolo, piu' liquido, **intorno ai 10 minuti** | §V.D.2, Fig. 14 |
| 13 | **Il risultato centrale**: a correlazione bassa (0-20%) il titolo meno liquido resta spiegato male dal piu' liquido; **sopra il 50% di correlazione l'impatto incrociato diventa importante**. "L'impatto incrociato conta se il titolo spiegato e' meno liquido del titolo che spiega" | §V.D.3, Fig. 15-16 |
| 14 | Nella Fig. 16, i valori delle celle (dR2 medio fuori campione, in punti percentuali): con correlazione 0-20% quasi tutti **fra -0,2 e 1,4**; con correlazione 20-50% fra -0,3 e 2,8; con correlazione 50-100% **fra 0,2 e 8,1**, piu' una cella di 11,4 (riga del titolo spiegato piu' liquido, 22,0-442,4 K$, colonna del titolo che spiega piu' liquido; e' l'unica riga senza celle vuote, quindi l'assegnazione e' sicura) con un errore di 4,5 a due deviazioni standard (lettura nostra del pannello degli errori, la cui ultima riga e' anch'essa piena), cioe' circa il 40% della media, sotto la soglia del 50% che nasconde le celle. **A quale coppia di secchi di liquidita' vadano gli altri valori lo abbiamo ricostruito noi**: il testo estratto perde le celle vuote. Gli intervalli dei valori per secchio di correlazione sono verificati (fase 4) | Fig. 16 |
| 15 | **Le tre condizioni** degli autori: (1) almeno **10-20 scambi in entrambi** i titoli per finestra; (2) correlazione **sopra il 50%**, a qualunque durata; (3) **titolo spiegato meno liquido** di quello che spiega. Fra titoli poco correlati e di liquidita' simile, l'impatto incrociato a coppie non domina (come Cont e altri 2023 sulle azioni USA, la cui correlazione media nel campione e' il 25%); sommato su molti titoli puo' contare | §V.E |
| 16 | **La sintesi**: la formazione del prezzo avviene **dentro i titoli molto liquidi**; poi gli scambi su questi muovono i prezzi dei correlati meno liquidi, a una velocita' limitata dalla **frequenza minore** delle due | §V.E, §VII |
| 17 | **Titoli di Stato USA** (2021-2022, a pronti e future, finestra di 30 minuti per Fig. 18-19): gli scambi dei titoli piu' liquidi (**il future sul decennale** e il decennale a pronti) aumentano molto la spiegazione dei meno liquidi. Esempio del testo: il 5 anni a pronti si spiega all'**11,8%** con i propri scambi, al **14,1%** aggiungendo il 2 anni a pronti, al **45,2%** con tutti | §VI.A-B, Fig. 17-18 |
| 18 | Nella Fig. 17, letta da noi riga per riga: il future sul decennale aggiunge circa **18,9 punti** al 5 anni a pronti, e il 5 anni a pronti ne aggiunge circa **4,2** al future sul decennale. E' l'asimmetria del risultato 13. **Cella ricostruita** dall'ordine delle colonne; **verificata (fase 4)**: nelle due righe il vuoto del testo estratto cade proprio sulla diagonale (l'8a colonna per il 5 anni a pronti, la 3a per il future sul decennale), quindi le colonne sono allineate. Nella stessa riga il decennale a pronti aggiunge 6,8 al future sul decennale | Fig. 17 |
| 19 | Anche il titolo piu' liquido migliora con molti titoli meno liquidi: nella Fig. 18 la riga del future sul decennale va da **27,0%** (i suoi scambi) a **45,2%** (con gli altri). **Valori letti dalla figura**, non scritti nel testo; **verificati (fase 4)**: la riga ha tutte le 11 celle, il 27,0 sta nell'ultima colonna, che e' la diagonale (il modello diagonale, come l'11,8 del 5 anni a pronti), e il 45,2 nella penultima, cioe' con tutti gli altri titoli. La riga non sale in modo monotono (43,7 poi 43,3) | §VI.B, Fig. 18 |
| 20 | **La matrice di Kyle del 9 novembre 2021** (finestra di 30 minuti), in due normalizzazioni: impatti dello stesso segno e della stessa grandezza su tutta la curva (lo spostamento parallelo dei tassi); i volumi di una scadenza muovono di piu' le scadenze vicine; future e pronti si comportano allo stesso modo; scambiare un titolo poco liquido resta caro anche se e' esposto a un fattore liquido | §VI.C, Fig. 19 |
| 21 | **Significativita'** sui titoli soli: solo il **2% circa** dei valori p del test F supera la soglia di Bonferroni (alfa 10^-2, soglia alfa/m = 10^-5); con Benjamini e Hochberg l'**1,8%** | App. B.1, Fig. 20 |
| 22 | **Il limite del modello lineare** (E-mini S&P, finestre di 1 minuto, 2021): **il flusso e i residui sono autocorrelati**, i prezzi no. Con piu' dati l'autocorrelazione dei residui crescerebbe e il modello "sarebbe invalidato" [App. B.2]. Altrove gli autori lo dicono al presente: l'autocorrelazione del flusso "invalida" il quadro lineare usato nello studio [§IV.B] e "invalida questi modelli", che pero' "restano significativi" [§VII]. L'autocorrelazione a un passo dei residui e' "intorno al 10%", che gli autori ritengono abbastanza bassa per il test F | App. B.1-B.2, Fig. 21 |

## Il Perche'

Il meccanismo degli autori [§V.B.2, §V.D.1, §V.E, §VII]: il prezzo si forma **dove c'e' la
liquidita'**, perche' li' molti partecipanti tengono coerenti flusso e prezzo. Uno strumento liquido
e molto correlato fa da **serbatoio di liquidita'** (l'espressione degli autori e' per il future sul
decennale, abstract e §VII), e gli strumenti meno liquidi seguono. Perche' gli scambi si concentrino
li' (per esempio, chi vuole esposizione al fattore comune scambia lo strumento liquido) **e' una nostra
spiegazione**: il paper non la scrive. La velocita' con cui il flusso si trasmette e' limitata
dallo strumento piu' lento: servono abbastanza scambi in tutti e due.

**Le alternative che scartano**:

- **la sola comunanza dei flussi**: se i flussi dei due titoli sono correlati, il modello diagonale
  la cattura gia'. Per questo la misura e' dR2, il guadagno **oltre** il modello diagonale [§III.A];
- **l'impatto che si spegne nel tempo** come causa del calo dell'R2 sulle finestre lunghe: non regge,
  perche' l'impatto "di solito resta significativo dopo un migliaio di scambi" (citando Bouchaud e
  altri 2018), e in un'ora su TRMB ce ne sono circa 1.400; la causa e' la forma a S del legame flusso-prezzo [§V.A];
- **lo sfasamento degli orologi e i ritardi** come causa dell'effetto Epps su un titolo solo: per un
  titolo, flusso e prezzo si aggiornano insieme, quindi resta solo il tempo minimo di reazione [§V.A];
- **la teoria delle aspettative sui tassi** (i tassi lunghi come attese dei tassi brevi): gli autori
  dicono che i loro dati la "mettono in discussione", perche' l'informazione va dal decennale verso
  le altre scadenze [§VI.A, §VII].

**Cosa lo ucciderebbe**, dedotto dal perche' (gli autori non lo scrivono): un'accuratezza aggiunta
fuori campione nulla o negativa sulle coppie molto correlate; nessuna asimmetria fra il verso "dal
piu' liquido al meno liquido" e il verso opposto; un guadagno che non dipende dal numero di scambi
per finestra.

## Cosa Abbiamo Riprodotto

Niente, ancora. Si puo' rifare tutto con il tape di ATAS (vedi sotto).

## Cosa Non Regge, O Non Si Dice

1. **E' una spiegazione, non una previsione.** Il flusso della finestra spiega il prezzo della
   **stessa** finestra [§III, eq. 1, 15]. "Fuori campione" vuol dire solo che i coefficienti sono
   stimati l'anno prima. Non dice che il flusso di ES **anticipa** NQ: dice che, a parita' di flusso
   di NQ, il flusso di ES nello stesso minuto spiega una parte in piu' del movimento di NQ. Per una
   previsione servono i flussi ritardati, che il paper non prova (lo fa Cont e altri 2023, scartato
   in fase 1).
2. **Le volatilita' del giorno guardano avanti.** Le covarianze del giorno `t_k` usano la volatilita'
   dei prezzi e dei flussi **calcolata sullo stesso giorno** [eq. 12-14], anche fuori campione. Per
   misurare un legame va bene; per una lettura in tempo reale no: chi legge alle 10:00 non conosce la
   volatilita' di tutta la giornata.
3. **Il Nasdaq-100 non c'e'**, o almeno non e' nominato. Dei future su indici si nomina solo
   l'E-mini S&P. La frase della fase 1, "rifacibile con ES e NQ", e' una nostra estensione, non un
   risultato del paper.
4. **I dati non sono descritti**: fornitore, orari, regola per il lato dell'aggressore, filtri,
   giorni esclusi. Non c'e' codice.
5. **Le coppie e dR2 non hanno un test di significativita'.** Il test F dell'Appendice B riguarda i
   titoli soli; per le coppie ci sono medie per secchio con un errore a due deviazioni standard. Le
   osservazioni sono finestre, non giornate.
6. **Il modello e' sbagliato e gli autori lo dicono**: il flusso e' autocorrelato, e l'autocorrelazione
   "invalida" il quadro lineare dello studio, scritto al presente [§IV.B, §VII]; con piu' dati, in
   App. B.2, il modello "sarebbe invalidato" anche nella misura dei residui. Lo tengono perche' "resta
   significativo" per spiegare i prezzi [§VII]. Lo difendono perche' il propagatore
   migliorerebbe di poco l'adattamento (lo citano da Tomas e altri 2022, non lo misurano qui).
7. **Numeri che non tornano**: "120.000" coppie possibili fra 500 titoli (sono 124.750); 50.000
   secchi di correlazione per scegliere 20.000 coppie (molti secchi saranno vuoti, non si dice); "m
   circa 10^3 (500 titoli per 5 anni)" test, che sarebbero 2.500; il campione e' "2017-2022" ma
   l'abstract dice 5 anni. Sono con ogni probabilita' i 5 anni fuori campione, 2018-2022, con il 2017
   solo da dentro: lo implica il §IV.A ("aggregated over a five-year period", 5 x 20.000 = 100.000
   coppie-anno), ma non lo scrive in chiaro.
8. **Le unita' delle pendenze** di Theil-Sen (Fig. 10, 11, 13) non sono dichiarate, e gli assi delle
   liquidita' sono in migliaia di dollari in scala logaritmica: le pendenze non si possono tradurre con
   certezza in "quanto R2 in piu' per quanta liquidita' in piu'". Una lettura compatibile con gli assi,
   **nostra**: in Fig. 10a punti percentuali di dR2 per punto di correlazione (0,03 x 100 = circa 3
   punti su tutta la scala); in Fig. 13 punti percentuali di R2 per decade di liquidita' (4,44 su circa
   due decadi, coerente con il "da sotto il 20% a circa il 30%" della Fig. 12a).
9. **La conclusione sui tassi va oltre i dati.** Chi muove il prezzo di minuto in minuto e cosa
   determina il livello dei tassi su mesi sono domande diverse: il paper mostra la prima e ne trae
   una conclusione sulla seconda.
10. **Le figure sono le sole fonti di molti numeri**, e le mappe di calore estratte perdono le celle
    vuote: i risultati 14, 18 e 19 sono ricostruiti. La fase 4 ha verificato le celle citate in 18 e 19
    e gli intervalli di 14 (vedi La Verifica); le altre celle della Fig. 16 restano da guardare sul PDF
    prima di usarle in un documento del framework.
11. **Nessun costo, nessuna strategia**: il paper non ne propone, quindi non c'e' niente da
    sopravvalutare, ma neanche niente da usare direttamente.
12. **Conflitti**: due autori sono di Capital Fund Management, un gestore, e la ricerca e' condotta
    nella cattedra di Econofisica, "sotto l'egida" anche di CFM [§VIII] (il paper non usa la parola
    "finanzia"). Non e' dichiarato un conflitto.
13. **Il massimo sulle durate e' preso fuori campione** (aggiunto in fase 4). R2*, dR2* e tau* sono i
    massimi "sulle durate provate" [App. A, Tab. I], e le didascalie parlano di "optimal out-of-sample"
    (Fig. 2, 3, 5, 6, 8, 10a, 13): il paper non dice che la durata si scelga dentro campione. Se il
    massimo e' preso sui risultati fuori campione, R2* e dR2* sono **ottimisti** (la scelta e' fatta
    sui dati di prova). **La griglia delle durate provate non e' dichiarata**: non si sa quante
    varianti per titolo e per coppia.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

**Cosa abbiamo** ([`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md)): il **tape** di NQ e di
ES dal 2023 (ogni scambio con tempo, lato dell'aggressore, volume, prezzo, **denaro e lettera prima e
dopo**), e le barre a 1 minuto con volume e delta dal 2017 per entrambi. Il tape da' esattamente
quello che usa il paper: il flusso con il segno, dal lato dell'aggressore, e il punto medio fra denaro
e lettera. Le barre danno il delta e il prezzo dello scambio, che a 1 minuto basta.

**Due vincoli prima di cominciare**:

- **Il fuori campione di NQ e' dal 2025 in poi** e ogni sguardo lo consuma (CLAUDE.md, obbligo 2;
  `.claude/rules/il-framework-quantitativo.md`). Lo schema del paper, "stima sull'anno prima, misura
  sull'anno dopo", si fa quindi **stimando sul 2023 e misurando sul 2024**, tutto dentro il campione
  di NQ. Il 2025-2026 si guarda solo a regole congelate, quando l'utente lo decide.
- **ES e' nel progetto solo per lo studio delle opzioni** (CLAUDE.md, decisione 2). Usare il flusso
  del future ES per leggere NQ esce da quel perimetro: **serve la richiesta esplicita dell'utente**.

**Come si misura l'impatto incrociato fra ES e NQ**, passo per passo:

1. **Le finestre.** Seduta di cassa 09:30-16:00 di New York, convertita in UTC per data con
   `LeSedute.cs` o `sedute.py`. Durate tau da 10 secondi a 30 minuti (10 s, 30 s, 1, 2, 5, 15, 30
   min): il paper dice che l'E-mini S&P sta fra gli strumenti con la durata ottima lunga (risultato
   4), quindi la durata giusta per ES-NQ va cercata, non assunta.
2. **Le due serie per finestra e per strumento.** `q` = somma dei volumi con il segno
   dell'aggressore (il delta); `dp` = punto medio alla fine meno punto medio all'inizio della
   finestra, in punti, poi in dollari (20 dollari a punto per NQ, 50 per ES). Con le barre a 1
   minuto, `dp` = differenza fra le aperture di due barre e `q` = delta della barra. **Il contratto e'
   il continuo** con il rollover gia' trattato dai dati.
3. **Le volatilita' del giorno.** Il paper usa quelle dello stesso giorno (punto 2 dei limiti). Si
   fanno due versioni dichiarate: quella del paper (per confrontare i numeri) e quella **con la
   volatilita' del giorno prima**, che non guarda avanti.
4. **Le correlazioni dell'anno** (prezzi, flussi, prezzi-flussi), stimate sul 2023, e la verifica
   della condizione 2 del paper: **la correlazione dei prezzi di ES e NQ sui 5 minuti deve stare
   sopra il 50%** (da misurare: il paper non la da').
5. **La liquidita'** di ciascuno, `sigma x omega` in dollari per 5 minuti, e il numero di scambi per
   finestra: la condizione 3 dice che l'impatto va **dal piu' liquido al meno liquido**, e la 1 che
   servono almeno 10-20 scambi in tutti e due. Quale dei due sia "il piu' liquido" nel senso del
   paper **si misura**, non si assume.
6. **I modelli, misurati sul 2024.** Modello diagonale di NQ (`dp_NQ = lambda q_NQ`), poi ML (la
   regressione di `dp_NQ` su `q_NQ` e `q_ES`) e Kyle sulla coppia; l'R2 sull'errore di NQ e
   **l'accuratezza aggiunta dR2 = R2 - R2_diag**, per ogni durata. E **lo stesso al contrario**,
   NQ che spiega ES. L'ipotesi del paper, tradotta: se ES e' il piu' liquido, dR2 da ES verso NQ e'
   **maggiore** di dR2 da NQ verso ES.
7. **La significativita' per giornata** (obbligo 3): si calcola dR2 giorno per giorno (o la
   differenza fra le somme degli errori quadrati dei due modelli), e la prova e' sulla serie delle
   circa 250 giornate del 2024, con un bootstrap a blocchi di giornate. Mai sulle finestre: sono
   autocorrelate (risultato 22).
8. **Il modello nullo.** Si accoppia il flusso di ES di un giorno con NQ di **un altro** giorno
   (oppure si sposta il flusso di ES dentro la giornata di un ritardo casuale oltre un'ora). Con il
   nullo, dR2 fuori campione deve stare intorno a zero; il risultato vero si confronta con la sua
   distribuzione.
9. **La variante che servirebbe davvero, da dichiarare prima** perche' il paper non la prova: **il
   flusso di ES della finestra prima** che spiega NQ della finestra dopo. Solo questa direbbe che ES
   aiuta a leggere NQ **in anticipo**; quella del paper dice che lo aiuta a leggerlo **mentre succede**.

**Il legame con le opzioni** (la domanda della ricerca), che il paper non tocca: se la copertura dei
dealer di SPX passa da ES, ci si aspetterebbe un dR2 da ES verso NQ **piu' grande nei giorni di gamma
negativa** dei dealer, quando la copertura compra e vende con il movimento. La gamma dei dealer su
SPX si ha da X-Ray solo da ottobre 2026: questa prova cade **nel fuori campione di NQ** e si
registra in avanti, solo a regole congelate e su decisione dell'utente.

**Cosa non si puo' rifare**: le 500 coppie e la curva dei tassi (fuori dallo scopo); la scala
temporale sotto il tick di ATAS, se il tape aggrega scambi dello stesso istante (da verificare sulle
finestre di 10 secondi).

## Cosa Ne Prende Il Framework

- **Il metodo, non un'idea di trading**: dR2 fuori campione, cioe' quanto il flusso di uno strumento
  spiega **in piu'** del flusso dello strumento stesso, come misura di "un mercato aiuta a leggere
  l'altro". E' la misura che manca alla domanda della ricerca.
- **Le tre condizioni** come cose da verificare prima: scambi sufficienti per finestra, correlazione
  sopra il 50%, verso dal piu' liquido al meno liquido.
- **La durata della finestra si cerca**, non si fissa: per i future molto scambiati il paper la trova
  piu' lunga del solito.
- **I due avvertimenti**: e' una spiegazione nello stesso intervallo, non una previsione; le
  volatilita' del giorno vanno prese dal passato.
- **Nessuna regola operativa**: il paper non ne propone e non ne prova.
- **Per lo studio globale unico di NQ ed ES**: questo paper sostiene che leggere il flusso di ES
  **insieme** a quello di NQ ha senso, se ES e' il serbatoio di liquidita'; non sostiene niente sulle
  opzioni, sui muri o sullo zero gamma.

## La Verifica

**9 novembre 2026**, verifica avversaria contro il PDF (arXiv v2, 18 pagine, sha256 confermato:
`63d78f1c...6a52d9`). Testo estratto con `pdftotext -layout` e `-raw -enc UTF-8`; le pagine non si
vedono come immagine (manca `pdftoppm`), quindi le formule e le figure si sono controllate sul testo e,
per la radice dell'eq. 9, **sul font incorporato nel PDF**: il carattere `»` e' il glifo 200 del font
`Yhcmex` (estensione matematica di yhmath), e il suo disegno, decodificato dal programma Type 1, e' una
radice alta. Nessuna istruzione nel PDF.

**Controllate circa 105 affermazioni** (scheda, termini, metodo, i 22 risultati con i loro numeri, il
perche', i limiti, i rimandi al paper nella parte su NQ): **93 confermate, 11 corrette, 1 tolta come
affermazione del paper** (spostata a lettura nostra), **1 limite aggiunto**.

I dubbi del lettore, uno per uno:

- **Eq. 9, Kyle**: confermata la radice quadrata di matrice (font e §II), tolto il "da ricontrollare".
- **Celle delle Fig. 16-18**: confermate. Fig. 18, riga del future sul decennale: 11 celle piene, il
  27,0 e' la diagonale (i suoi scambi), il 45,2 la penultima colonna (tutti gli altri). Fig. 17: il
  vuoto della diagonale cade nella posizione giusta in tutte e due le righe, quindi 18,9 e 4,2 sono
  nelle colonne giuste. Fig. 16: gli intervalli per secchio di correlazione tornano; l'11,4 sta
  nell'unica riga piena, quindi la sua cella e' certa.
- **Pendenze delle Fig. 10 e 13**: confermate (0,03 [0,03; 0,03]; 4,44 [3,81; 5,07]; 1,34 [1,14; 1,54];
  Fig. 11a 0,0 [0,0; 0,0]) e confermato che le unita' non sono dichiarate; aggiunta una lettura
  compatibile con gli assi, dichiarata nostra.
- **Fig. 9**: la legenda grezza dice "E-mini S&P future (3M) - E-mini S&P future" e "E-mini S&P future -
  bond future 10Y"; "(3M)" non e' spiegato da nessuna parte.
- **Nasdaq-100**: confermato che non e' nominato (cercati Nasdaq, NDX, NQ: nessuna occorrenza; degli
  indici c'e' solo l'E-mini S&P, nel §V.B.1, nella Fig. 9 e in App. B.2 con la Fig. 21).

**Le correzioni**:

1. **Il modello lineare "invalidato"** (la piu' grave): l'analisi riportava solo il condizionale di
   App. B.2 ("sarebbe invalidato" con piu' dati); il §IV.B e il §VII lo dicono al presente,
   l'autocorrelazione del flusso "invalida" il quadro lineare. Corretti il risultato 22 e il limite 6.
2. **Dentro e fuori campione**: "si misura sull'anno dopo" era sbagliato; si stima sull'anno
   precedente e si misura sull'anno stesso [§IV.A].
3. **Le figure filtrate al 7%** sono le Fig. 3-8 (mancava la Fig. 8b), non 3-7.
4. **La matrice M**: le Fig. 10-11 usano l'errore su tutti e due i titoli (`I_sigma`), non solo sul
   titolo spiegato.
5. **L'impatto che dura**: il paper dice "resta significativo dopo un migliaio di scambi", non
   "dura migliaia di scambi".
6. **I 5 anni**: il §IV.A li implica (5 x 20.000 = 100.000 coppie-anno), non era "non scritto" del
   tutto.
7. **Le unita' delle pendenze**: "non si possono tradurre" era troppo forte; aggiunta la lettura
   compatibile con gli assi, dichiarata nostra.
8. **Conflitti**: il paper non dice che CFM "finanzia" la cattedra; dice "sotto l'egida" di quattro
   enti, fra cui CFM.
9. **Fig. 9**: riportata la legenda esatta delle due coppie con l'E-mini S&P.
10. **Fig. 16**: precisata la cella dell'11,4 e il suo errore (4,5 a due deviazioni standard, circa il
    40% della media), lettura nostra del pannello degli errori.
11. **Eq. 9**: da "da ricontrollare" a confermata.

**Tolta come affermazione del paper**: "chi vuole esposizione al fattore comune scambia li'" (Il
Perche'). Il paper non lo scrive: resta come spiegazione nostra, dichiarata. "Serbatoio di liquidita'"
e' l'espressione degli autori per il future sul decennale.

**Aggiunto, cosa mancava**: il limite 13. R2*, dR2* e tau* sono massimi sulle durate provate, e le
didascalie li dicono "out-of-sample": il paper non dice di scegliere la durata dentro campione, quindi
i massimi sono probabilmente ottimisti, e la griglia delle durate (il numero di varianti) non e'
dichiarata.

**Confermati senza modifiche**, fra gli altri: TRMB 2021/2022 e il massimo fra 10 e 100 secondi, 1.400
scambi in un'ora; 10-20 scambi per finestra in tutti e due i titoli; 500-5.000 scambi per AMZN, AAPL,
TSLA, E-mini S&P e future sul decennale; 0,5 scambi al secondo e R2 25%; ML meglio di Kyle fuori
campione e la spiegazione degli attriti; dR2 da 0 a oltre il 5%; 500 dollari per 5 minuti; i due
gruppi a 90 secondi e 10 minuti; le tre condizioni e il 25% di correlazione media delle azioni;
11,8 / 14,1 / 45,2 del 5 anni a pronti; il 9 novembre 2021 e le quattro osservazioni della Fig. 19;
il 2% di Bonferroni (alfa 10^-2, alfa/m 10^-5) e l'1,8% di Benjamini e Hochberg; l'autocorrelazione dei
residui intorno al 10% al primo ritardo; i "numeri che non tornano" del limite 7 (124.750 coppie, 2.500
test). La scheda (titolo, autori, affiliazioni, timbro arXiv 26 marzo 2024, "Dated: March 27, 2024",
18 pagine, sha256) e' giusta; la rivista e il doi vengono da Crossref (fase 1) e il PDF non li riporta.
