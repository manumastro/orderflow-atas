---
chiave: barbon-2021-gamma-fragility
sigla: BB
titolo: "Gamma Fragility"
autori: [Andrea Barbon, Andrea Buraschi]
versione_letta: 18 marzo 2021 (prima versione gennaio 2019)
fonte: SSRN; St. Gallen School of Finance Research Paper 2020/05
identificativo: "3725454"
url: http://www.abarbon.com/assets/Barbon_Buraschi_2021_Gamma_Fragility.pdf
tipo: preprint
revisione_fra_pari: no
pagine: 48
pdf: barbon-2021-gamma-fragility.pdf
pdf_sha256: f8152092d72c450314da78daa97414ba6c5e10c7bb7e8642d6f636e1db6e3c4a
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; i dati per tipo di operatore (ISE, GEMX, PHLX), OptionMetrics e TAQ sono a pagamento"
---

# Barbon e Buraschi 2021: L'Analisi

*Gamma Fragility*, Andrea Barbon (Universita' di San Gallo) e Andrea Buraschi (Imperial College),
versione del 18 marzo 2021, 48 pagine, SSRN 3725454, scaricata dalla pagina dell'autore. Working
paper, **non revisionato**. Letto per intero il 14 ottobre 2026, dal testo estratto con `pdftotext`:
le formule e diverse tabelle escono storpiate, e dove succede lo diciamo. Citazioni `[BB §n]`.

## I Termini

- **Delta**: quanto cambia il prezzo di un'opzione quando il sottostante si muove di un punto.
  **Delta hedging** (copertura in delta): chi ha l'opzione compra o vende il sottostante per
  annullare quel delta.
- **Gamma**: quanto cambia il delta quando il sottostante si muove. Chi e' **lungo di gamma**, per
  restare coperto, vende quando il prezzo sale e compra quando scende: **contro** il movimento. Chi e'
  **corto di gamma** compra quando sale e vende quando scende: **con** il movimento.
- **Gamma imbalance** (squilibrio di gamma): la gamma netta dei dealer (i market maker e i
  broker-dealer) su tutte le opzioni di un titolo, espressa come volume di copertura che un
  movimento dell'1% costringe a fare, in percentuale del volume medio giornaliero [BB §II.B].
- **Momento intraday** in questo paper: l'**autocorrelazione** positiva dei rendimenti a h minuti
  dentro la stessa giornata (un intervallo che sale e' seguito da uno che sale). **Inversione**:
  autocorrelazione negativa. E' un concetto diverso da quello di Gao e altri e di Baltussen e altri,
  dove il momento intraday e' "la giornata predice l'ultima mezz'ora".

## La Domanda

La copertura in delta dei dealer spiega la volatilita' intraday, il segno dell'autocorrelazione
intraday e i flash crash? Con gamma imbalance negativa ci si aspetta piu' volatilita' e momento; con
gamma imbalance positiva meno volatilita' e inversione. L'effetto dovrebbe essere piu' forte sui
titoli meno liquidi [BB introduzione, §III].

## Il Metodo

**I dati** [BB §II.A]: volumi di apertura e chiusura delle opzioni **divisi per tipo di
operatore** (broker-dealer, market maker, desk proprietari, clienti) da tre borse Nasdaq (ISE,
GEMX, PHLX), gennaio 2010 - maggio 2020. Le tre borse fanno fra il 10 e il 20% del volume in dollari
delle opzioni su azioni USA. Prezzi, open interest e greche da OptionMetrics; rendimenti e volumi
intraday da TAQ; notizie da RavenPack. Campione finale: **2.700 azioni USA e 4 indici** (S&P 500, Dow
Jones, Nasdaq 100, Russell 2000).

**La misura** [BB §II.B]: l'inventario dei dealer per ogni opzione si ricostruisce con la procedura
di Ni, Pearson, Poteshman e White (2018), cumulando i volumi di apertura e chiusura. Solo
broker-dealer e market maker si coprono:

```text
(1)  inventario dei coperturisti_j = inventario broker-dealer_j - inventario clienti_j
     gamma dei coperturisti_i = somma su j scritte su i di  gamma_j x inventario_j
(2)  I_B(i,t) = gamma dei coperturisti_i x S_i(t) / ADSV_i(t)     <- storpiata nel testo estratto
```

ADSV e' il volume medio giornaliero in azioni sui 21 giorni che finiscono il giorno prima. La
formula (2) esce storpiata: la definizione a parole e' "il volume in dollari dei coperturisti
indotto da un movimento dell'1%, in percentuale del volume medio giornaliero". **Va ricontrollata sul
PDF** prima di usarla.

**Per gli indici** le tre borse Nasdaq non hanno dati, e la misura cambia [BB §III.A, eq. 4]: si
**assume** che gli investitori siano corti di call e lunghi di put (il *collar*), quindi i dealer
lunghi di call e corti di put. La gamma imbalance diventa `(gamma $ call - gamma $ put) / ADV x 100`
da OptionMetrics. E' un'ipotesi, non una misura.

**Le prove**, tutte regressioni panel con il regressore al giorno prima (t - 1), effetti fissi per
titolo e per mese, errori standard **raggruppati per titolo e per mese** (per titolo e per giorno
nelle tabelle VIII e IX), regressori standardizzati e winsorizzati all'1%:

- volatilita': `|R(j,t)|` su `I_B(j,t-1)` e la volatilita' implicita del giorno prima [eq. 3];
- liquidita': interazione con un indicatore dei titoli meno liquidi (rapporto di Amihud) [§III.B];
- autocorrelazione: per ogni titolo e giorno, l'autocorrelazione dei rendimenti a h = 5, 10, 20, 30,
  60 minuti, regredita su `I_B(j,t-1)` [eq. 5];
- range giornaliero `(H - L) / (H + L)` [eq. 6];
- flash crash: test del *drift burst* di Christensen, Oomen e Reno' (2018), banda di 30 minuti per la
  media e 60 per la volatilita', statistica calcolata ogni secondo, inizio quando passa -2,576, piu'
  una caduta di almeno l'1% in 30 minuti [§IV.A]; poi una regressione sulla probabilita' e una
  sull'entita' della caduta.

Per ogni titolo anche una regressione nel tempo con errori standard di **Newey-West** (corretti per
eteroschedasticita' e autocorrelazione) [BB Fig. 6].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | Senza condizionare, l'autocorrelazione a 5 minuti non e' diversa da zero in media; ma nel 13% delle giornate-titolo supera +0,20 e nell'8% sta sotto -0,20. Su TAQ 1996-2014, 2.417.788 osservazioni | §II.A, Fig. 1, Fig. 7 |
| 2 | La gamma imbalance media dei dealer sulle azioni e' **positiva** (13,5, deviazione standard 124,5, mediana 0,054, in %): sui titoli singoli i dealer sono in media lunghi di gamma | Tab. I |
| 3 | Gamma imbalance del giorno prima e volatilita' di oggi: coefficiente negativo, t da -4,0 a -16,6 secondo gli effetti fissi. Una deviazione standard di gamma imbalance in piu' vale **da 5 a 25 punti base** in meno di rendimento assoluto giornaliero, su una media di 180 | §III.A, Tab. II |
| 4 | Regge aggiungendo il rendimento assoluto dei tre giorni prima (t da -3,2 a -15,4) | Tab. IV |
| 5 | Nelle regressioni titolo per titolo (Newey-West) il 70% delle statistiche t e' negativo | Fig. 6 |
| 6 | **Indici** (S&P 500, Dow, Nasdaq 100, Russell 2000; misura per ipotesi, 21.434 osservazioni): coefficiente da -14,8 a -5,5, t da -4,4 a -3,3; una deviazione standard vale **piu' di 20 punti base** di rendimento assoluto, circa il 20% di una deviazione standard | §III.A, Tab. III |
| 7 | Liquidita': l'interazione con i titoli meno liquidi vale -8,88 (t -7,61); da sola, la gamma imbalance in dollari perde significativita' (-1,30, t -1,34) | §III.B, Tab. V |
| 8 | **Autocorrelazione**: coefficiente negativo a tutte le frequenze: -1,81 (t -4,83) a 5 minuti, -1,47 (t -2,78) a 10, -1,62 (t -2,79) a 20, -1,55 (t -2,64) a 30, -1,98 (t -2,35) a 60. Una deviazione standard vale **da 1,5 a 2 punti** di autocorrelazione in meno. R quadro da 0,000 a 0,003. 1.541.400 osservazioni | §III.C, Tab. VI |
| 9 | Range giornaliero: una deviazione standard di gamma imbalance in meno vale circa **16 punti base** di range in piu' (coefficiente -15,98, t -10,2, con entrambi gli effetti fissi) | §IV, Tab. VII |
| 10 | Flash crash: 10.572 eventi su 1.849 titoli, caduta media -2,06% (mediana -1,60%) in 30 minuti. Una deviazione standard di gamma imbalance in meno alza la probabilita' di circa il **5% relativo**: significativo, economicamente piccolo (t -1,38, -2,17, -2,97, -1,78: una specifica non significativa, una solo al 10%). Sull'entita': da 20 a 200 punti base di caduta in piu' | §IV.A, Tab. VIII, IX |
| 11 | 6 maggio 2010: la gamma imbalance media del mercato a -1,7 deviazioni standard (circa -431 milioni di $), una settimana prima +383 milioni | §IV.B, Fig. 8 |
| 12 | Gli ETF a leva **non** spiegano la variazione di momento e inversione: dichiarato, senza tabella | §V |

**Il perche'** che gli autori propongono: la copertura dei dealer e' un flusso automatico che, con
liquidita' limitata, muove il prezzo. Corti di gamma lo spingono nella direzione del movimento, lunghi
di gamma lo frenano. Il canale e' distinto dall'informazione privata e dai vincoli di margine. Gli
autori elencano anche **perche' poteva non vedersi** [§III.A]: dealer poco avversi al rischio, mercato
abbastanza liquido, coperture senza impatto, canale troppo debole, misura rumorosa.

**Cosa lo ucciderebbe**, dedotto dal perche': nessuna differenza fra titoli liquidi e illiquidi;
nessuna differenza di autocorrelazione fra giorni di gamma positiva e negativa con una misura
migliore; un effetto che resta uguale quando i dealer non si coprono.

## Cosa Abbiamo Riprodotto

Niente. Servono i dati di posizione per tipo di operatore, che non abbiamo (vedi sotto).

## Cosa Non Regge, O Non Si Dice

1. **I dati non si possono avere.** La misura sui titoli usa volumi di apertura e chiusura **per
   tipo di operatore**, a pagamento e coperti solo al 10-20% del volume. L'inventario cumulato parte
   da un punto iniziale che il paper non dichiara.
2. **Il segno, chi e' lungo e chi e' corto.** Sui titoli il segno si **misura**, e in media i dealer
   risultano **lunghi** di gamma (risultato 2). Sugli indici il segno si **assume** (dealer lunghi di
   call, corti di put): l'opposto di cio' che misurano sui titoli, e proprio sugli indici, l'unico
   caso che ci interessa. La formula (1), broker-dealer **meno** clienti, non e' spiegata e non torna
   con l'idea che le posizioni nette sommino a zero.
3. **Il campione non torna.** I dati delle opzioni coprono 2010-2020, ma l'esempio di General Motors
   del 9 gennaio 2009 sta fuori [§I]. I flash crash si cercano su TAQ 1997-2015 [§IV.A] e si
   regrediscono su una gamma imbalance che parte nel 2010. La tabella IX ha 11.405 osservazioni per
   10.572 eventi. Il campione degli indici (21.434 osservazioni su 4 indici) e' piu' lungo di
   2010-2020: il periodo non e' dichiarato. La Figura 1 dice 1996-2014 nel testo e 1996-2017 nella
   didascalia.
4. **Testo e tabelle non coincidono.** L'introduzione dice che l'effetto sull'autocorrelazione
   "cresce con la frequenza e raggiunge il massimo a 60 minuti"; la tabella VI ha la t piu' alta a 5
   minuti (-4,83) e la piu' bassa a 60 (-2,35), e le differenze fra frequenze non sono provate. Per gli
   indici il testo attribuisce -14,8 alla colonna con entrambi gli effetti fissi, la tabella alla
   colonna senza. Per la volatilita' il testo cita come statistiche t (-5,07 e -12,08) quelli che in
   tabella II sono coefficienti; le t vere vanno da -4,0 a -16,6. La didascalia della tabella V chiama "meno liquidi" i titoli **sotto** la mediana
   di Amihud, che sono i piu' liquidi.
5. **Il potere esplicativo e' minimo**: R quadro fino a 0,003 sull'autocorrelazione. La significativita'
   viene da 1,5 milioni di osservazioni, non da un effetto grande.
6. **Look-ahead nelle prove, non nel segnale.** La gamma imbalance e' al giorno prima, bene. Ma la
   divisione liquidi/illiquidi usa la mediana di tutto il campione; la tabella VI controlla con il
   volume **dello stesso giorno**; la winsorizzazione e' sul campione intero. Per una regressione
   descrittiva passa; per una regola, no.
7. **Nessuna strategia, nessun costo, nessun fuori campione.** Il paper e' tutto in regressioni
   panel nel campione. Il numero di specifiche provate non e' dichiarato.
8. **La "nulla" e' chiamata al contrario**: gli autori chiamano "ipotesi nulla" la loro tesi
   (`beta < 0`) [§III.C]. Le statistiche sono comunque test contro zero.
9. **Una sezione senza numeri**: gli ETF a leva sono "comodamente" respinti senza tabella [§V].
10. **Il campione finisce a maggio 2020**, prima della crescita delle opzioni a scadenza giornaliera
    (0DTE). Il paesaggio della gamma sugli indici di oggi puo' essere diverso.

## Cosa Si Puo' Rifare Su ES, Con ATAS

Lo strumento del filone opzioni e' ES (CLAUDE.md, decisione 2), con NQ come confronto. Cosa abbiamo:
barre di 1 minuto di ES con volume e delta (volume aggressivo in acquisto meno in vendita) dal Data
Bridge, con una profondita' di storia ancora da misurare; gli indicatori Options Chain sul nostro
feed Rithmic (OI Profile, Key Levels, Expected Move, GEX Profile con Zero Gamma), **senza storia**:
registrano solo da quando sono sul chart, e l'OI e' giornaliero; gli indicatori Options X-Ray
(dati OptionsDepth sulle opzioni SPX, aggiornati ogni minuto, posizioni dei market maker per gruppo
di partecipanti, GEX heatmap, Hedge Pressure), solo su ES, **con una storia che non conosciamo**.
Vedi [`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md).

**Senza storia di opzioni, solo con le barre:**

- La distribuzione dell'autocorrelazione intraday di ES a 5, 10, 20, 30, 60 minuti, giorno per giorno
  (l'analogo della Figura 1 e 7). Ci dice se ES ha giorni di momento e giorni di inversione, e
  quanti. E' descrittivo: senza la gamma non verifica il meccanismo.

**Cosa serve storia che non abbiamo:**

- la regressione centrale (autocorrelazione di oggi sulla gamma di ieri) richiede anni di gamma
  giornaliera dei dealer su ES/SPX. ATAS Chain non la ricostruisce all'indietro: l'OI per strike del
  passato non c'e'. X-Ray la avrebbe solo se OptionsDepth espone la storia attraverso ATAS: **da
  verificare**.

**Cosa permetterebbe la registrazione in avanti:**

- ogni giorno, prima dell'apertura e a orari fissi, il segno della GEX totale e la distanza del prezzo
  dallo Zero Gamma (Chain), e da X-Ray la posizione dei market maker e la Hedge Pressure. Dopo qualche
  centinaio di giornate si puo' provare se **l'autocorrelazione intraday di ES e' piu' alta nei giorni
  di GEX negativa**, con la significativita' per giornata. X-Ray, che divide le posizioni per tipo di
  partecipante, e' la cosa piu' vicina ai dati di BB, e misura il segno invece di assumerlo.
- il delta di ATAS aggiunge una verifica che il paper non ha: nei giorni di GEX negativa il flusso
  aggressivo dovrebbe andare **nella direzione** del movimento appena fatto.

**Da verificare prima**: se il Data Bridge puo' leggere i valori calcolati da un altro indicatore
(GEX, Zero Gamma, Hedge Pressure); se le opzioni lette da Chain su ES sono quelle CME sul future.

## Cosa Ne Prende Il Framework

- **Il meccanismo**, come ipotesi da provare: gamma dei dealer negativa -> copertura con il movimento
  -> piu' volatilita' e autocorrelazione positiva; positiva -> il contrario. E' la stessa idea che
  Baltussen e altri (2021) provano sugli indici con un'altra misura.
- **Il condizionamento giusto**: la gamma si misura **prima** (al giorno o al minuto precedente),
  l'effetto si misura dopo.
- **La lezione sul segno**: sugli indici il segno della gamma dei dealer, in questi paper, e'
  un'ipotesi. Una misura per tipo di partecipante (X-Ray) vale piu' di una convenzione.
- **Il drift burst** di Christensen, Oomen e Reno' come rilevatore di cadute rapide, se un giorno
  servisse: banda 30/60 minuti, soglia -2,576, caduta minima dell'1%.
- **Nessuna regola operativa**: il paper non ne propone e non ne prova.
