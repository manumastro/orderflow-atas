---
chiave: michael-2022-option-volume-imbalance-as-a-predictor
sigla: MCH
titolo: "Option Volume Imbalance as a predictor for equity market returns"
autori: [Nikolas Michael, Mihai Cucuringu, Sam Howison]
versione_letta: arXiv 2201.09319 v1 (timbro arXiv 23 gennaio 2022; il PDF porta "January 2022" e "A PREPRINT")
fonte: arXiv (q-fin.ST); anche SSRN 4019647 (fase 1, non aperto)
identificativo: "arXiv:2201.09319v1; SSRN 4019647"
url: https://arxiv.org/pdf/2201.09319v1
tipo: preprint
revisione_fra_pari: no
pagine: 43
pdf: michael-2022-option-volume-imbalance-as-a-predictor.pdf
pdf_sha256: db09bc5cb1f187475736675bbedf261cf7e2187528c9ae94a313b9172dfbfb82
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati Nasdaq (NOM e PHLX, aggregati a 10 minuti per gruppo) e CRSP, a pagamento"
---

# Michael, Cucuringu, Howison 2022: L'Analisi

*Option Volume Imbalance as a predictor for equity market returns*, Nikolas Michael, Mihai Cucuringu,
Sam Howison (Universita' di Oxford e Alan Turing Institute). **Preprint arXiv, non revisionato**: la
versione letta e' la v1 del 23 gennaio 2022, e non abbiamo trovato una versione pubblicata in rivista.
Letto per intero il 9 novembre 2026 (43 pagine, appendici A-E comprese). Il testo e' stato estratto con
`pdftotext`; su questo PC non c'e' un programma che mostri le pagine come immagine, quindi **le figure
si sono lette solo come testo**: i numeri qui sotto sono quelli stampati dentro le figure (legende e
celle), e dove una figura andava ricostruita lo diciamo. Le formule sono state ricontrollate con
l'estrazione `-raw`, che conserva gli indici. Citazioni `[MCH §n]`, `[MCH Fig. n]`, `[MCH App. X]`.
*Nella verifica (fase 4) le 43 pagine sono state rese come immagine e le figure guardate: le correzioni sono
nel testo e l'elenco e' in fondo, "La Verifica".*

**In breve, per una persona.** Gli autori prendono, per ogni titolo e ogni giorno, i contratti di opzioni
scambiati da cinque gruppi (market maker, clienti, clienti professionali, broker, firm) e ne fanno un
numero fra -1 e +1: quanto il gruppo ha scommesso al rialzo (call comprate, put vendute) meno quanto al
ribasso, diviso il totale. Poi guardano se quel numero indovina **di quanto il titolo fara' meglio o peggio
dell'S&P 500 nella notte dopo**. Funziona per tre gruppi su cinque: i **clienti** indovinano, i **market
maker** e i **broker** sbagliano in modo sistematico (cioe' vanno letti al contrario), firm e professionisti
non dicono niente. Ma **non e' una prova sugli indici**: il paper misura un titolo contro il mercato, non la
direzione del mercato, e SPX, dai dati descritti, non c'e' (nostra deduzione: il paper non lo dice). Dentro la giornata, a 10 minuti, i risultati spariscono
appena si tiene conto di quante prove si sono fatte.

## I Termini

- **MPC** (*market participant class*, classe di partecipante): uno dei cinque gruppi che le borse Nasdaq
  segnano su ogni contratto: **Firm** (conti proprietari di un'impresa d'investimento), **Broker**
  (intermediario a commissione), **Market Maker** (chi quota sempre denaro e lettera), **Customer**
  (cliente che non e' broker o dealer: il "retail" in senso largo), **Professional Customer** (cliente non
  broker che invia piu' di 390 ordini al giorno in media in un mese) [MCH App. A].
- **Open buy, open sell, close buy, close sell**: acquisto o vendita che apre (o aumenta) una posizione,
  oppure la chiude. Per i market maker questa divisione nei dati non c'e' [MCH §3, App. A].
- **OVI** (*Option Volume Imbalance*, sbilancio del volume di opzioni): il numero fra -1 e +1 del paper,
  definito sotto. **Up** = call comprate + put vendute (scommesse al rialzo); **Down** = call vendute + put
  comprate [MCH §2.2].
- **EMR** (*excess market return*, rendimento in eccesso sul mercato): il rendimento del titolo meno quello
  dell'S&P 500 nello stesso intervallo [MCH §4]. **RR**: il rendimento grezzo.
- **CL->OP, OP->CL, CL->CL** (scritti nel paper `CL tmOP` ecc.): dalla chiusura di oggi all'apertura di
  domani (la **notte**), dall'apertura alla chiusura di domani, da chiusura a chiusura [MCH §4].
- **SR** (*Sharpe ratio* annualizzato): media del P&L giornaliero diviso la sua deviazione standard, per
  la radice di 252 [MCH eq. 3]. **PPD** (*profit per dollar*): profitto per dollaro investito, in punti base
  al giorno [MCH eq. 4, Fig. 6].
- **Q1-Q5** (gruppi per forza del segnale): ogni giorno i titoli si ordinano per |OVI|; Q1 sono tutti, Q3 il
  60% con lo sbilancio piu' forte, Q5 il 20% piu' forte [MCH §4, §7.4]. **QB1-QB5**: i cinque quinti
  separati.
- **Bonferroni**: correzione per prove multiple che divide la soglia del 5% per il numero di prove.
- **Open-t OVI**: l'OVI calcolato con tutto il volume dall'apertura fino all'ora t, aggiornato ogni 10
  minuti [MCH App. E]. E' l'analogo del "sommato dall'inizio della giornata" del riquadro X-Ray.

## La Domanda

Lo sbilancio fra il volume di opzioni "al rialzo" e quello "al ribasso", separato per classe di
partecipante, predice la direzione del sottostante? Quali gruppi portano informazione, su quali opzioni
(greche, volatilita' implicita, scadenza), su che orizzonte, e c'e' un effetto incrociato fra titoli
[MCH §1, "Summary of main contributions"]?

## Il Metodo

**I dati** [MCH §3, §4]: due serie Nasdaq, **NOTO** (Nasdaq Options Market, NOM) e **PHOTO** (Nasdaq
PHLX), dal 2 gennaio 2015 al 31 dicembre 2019. Per ogni giorno, aggiornamenti **cumulati ogni 10 minuti**
dalle 9:40 alle 16:00 di New York (39 istanti), per contratto, divisi per i cinque gruppi e per call e put;
per i gruppi diversi dai market maker anche per open/close buy/sell. In piu' un riepilogo giornaliero
(prezzi di apertura, chiusura, minimo, massimo, open interest). I sottostanti sono **soprattutto titoli e
11 ETF**; i rendimenti vengono da CRSP (NYSE), corretti per dividendi e frazionamenti. L'analisi
principale usa **PHOTO**; NOTO entra nel §7.3.

**La misura** [MCH §2.2, eq. 1, ricontrollata sull'estrazione `-raw`]: per il titolo i, il giorno d e il
gruppo m, sommando su tutti i contratti j del titolo,

```text
OVI(i,d,m) = somma_j [ X_up(i,j,d,m) - X_down(i,j,d,m) ] / somma_j [ X_up(i,j,d,m) + X_down(i,j,d,m) ]
X_up   = call comprate + put vendute         X_down = call vendute + put comprate
X = V   volume (numero di contratti)          -> la versione principale
X = T   numero di operazioni                  -> "trade-based"
X = P*V volume per il prezzo dell'opzione     -> "nominal", P = media fra apertura e chiusura del contratto
0/0 = 0  (nessun volume, OVI zero)            [nota 3]
```

La normalizzazione serve a confrontare **titoli diversi** (liquidita', capitalizzazione) sullo stesso
giorno, non uno stesso titolo a ore diverse [MCH §2.2]. Ogni operazione e' **contata due volte**, una per
parte: un cliente che compra una call da un market maker e' un "Customer buy" e un "Market Maker sell"
[MCH §2.2]. Non c'e' peso per il delta, ne' uno z-score. Il segno di OVI e' una convenzione: un gruppo puo'
risultare correlato al contrario [nota 4].

**La prova** [MCH §4, eq. 2]: un segnale per titolo e per giorno, il **segno** di OVI; si "compra" il titolo
contro SPY se OVI > 0, si "vende" se OVI < 0, con un dollaro al giorno diviso fra i titoli (schema
uniforme, il principale); P&L(d) = somma_i b(i,d) * EMR(i,d) * sign(s(i,d)). Gli autori dicono che **non e'
una strategia eseguibile** e che **non ci sono costi** [MCH §4]. La significativita' e' un test **H0: SR =
0** sulla serie dei P&L **giornalieri del portafoglio** (Bailey e Lopez de Prado, varianza dello SR di Bao
per la non normalita'); il confronto fra due SR e' il test di Ledoit e Wolf con bootstrap a blocchi
[MCH §4]. Altri schemi di peso: per |OVI|, per volume (log), per volume nominale, per volume diviso l'open
interest, per volume e volatilita' implicita [MCH §4, §6].

**La "regressione sul P&L"** [MCH §4.1, eq. 5-12]: per combinare piu' OVI, un segnale lineare s = b0 + somma
b_k OVI_k i cui pesi massimizzano il P&L cumulato (il segno sostituito da una sigmoide, penalita' L1,
ottimizzatore ADAM), stimato su finestre mobili di 500 giorni e provato sui 100 giorni dopo, cioe' **fuori
campione** a scorrimento. E' un metodo nuovo proposto dagli autori, senza soluzione chiusa ne' convessita'.

**L'intraday** [MCH App. E]: solo 2018-2019, con un universo piu' piccolo; open-t OVI a ogni t dei 39 istanti,
contro i rendimenti in eccesso a 1, 5, 10, 30, 60, 90, 180 minuti e la notte, calcolati dal punto medio del
book (LOB).

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **I gruppi piu' grandi sono market maker e clienti**; firm e professionisti hanno volumi mediani molto piccoli (ma su PHOTO i firm hanno il valore per operazione piu' alto) | §3.1, Fig. 1 |
| 2 | Correlazione (Spearman, mediana sui giorni) fra OVI dei gruppi: **clienti contro market maker circa -0,6** (da -0,53 a -0,68 sulle due serie); **clienti contro broker circa -0,6 solo su PHOTO** (da -0,55 a -0,66): su NOTO la figura da' da -0,35 a -0,40, anche se il testo scrive "-0,6 per entrambe le serie"; clienti contro firm circa -0,2 (da -0,15 a -0,21); il resto sotto 0,1 in valore assoluto, salvo market maker-broker circa 0,2 (PHOTO) e market maker-professionisti circa -0,2 (NOTO). Gli autori la leggono come effetto del doppio conteggio | §3.1, Fig. 3 |
| 3 | **Notte, rendimento in eccesso, PHOTO, schema uniforme** (SR annualizzato, Q1-Q5): **market maker da -3,55 a -4,42** (Q3 -4,42, PPD -1,9 punti base al giorno); **clienti da +2,66 a +3,46** (Q3 +3,4); **broker da -2,13 a -2,54**; professionisti da +0,98 a +1,44; firm da -0,24 a +0,24 | Fig. 6 |
| 4 | Significativita' (H0: SR = 0): **market maker e clienti p = 0** (a due decimali) in tutti i gruppi Q, broker 0,01 in Q1 e 0 negli altri; firm 0,34-0,36; professionisti 0,22-0,46. Sui quinti separati il primo (gli sbilanci piu' deboli) non dice niente: market maker QB1 p 0,97, clienti 0,08, broker 0,93 | §5, Fig. 7 |
| 5 | **Il segno conta**: l'OVI dei market maker e dei broker predice **al contrario** (SR negativo), quello dei clienti **nel verso** (SR positivo). Quando il gruppo che fa da controparte compra rialzo, il titolo fa peggio del mercato la notte dopo | Fig. 6, App. C (Fig. 26) |
| 6 | Quota di giorni "giusti" (max fra giorni positivi e negativi): market maker **sopra il 60%**, clienti e broker **fra 55% e 60%**, firm e professionisti vicini al 50% | App. C, Fig. 26 |
| 7 | **Orizzonte**: la notte (CL->OP) predice meglio di chiusura-chiusura, e la giornata dopo (OP->CL) molto meno. Market maker, rendimento in eccesso, Q3: SR -4,42 notte, -3,08 chiusura-chiusura, -1,16 giornata | §5, Fig. 8 |
| 8 | **Durata** (periodi da 1 a 10 giorni): per i market maker e' significativo anche il **secondo giorno** (p 0,00) e il cumulato **non torna indietro**; per gli altri gruppi solo il primo giorno; i clienti Q4-Q5 non tornano indietro, i broker invertono dal quinto giorno circa | §7.1, Fig. 16 |
| 9 | **Il peso per |OVI|** alza poco lo SR e, corretta per prove multiple, la differenza non e' significativa; i pesi per volume peggiorano lo SR. Il volume semplice batte il numero di operazioni (differenza significativa per i market maker) | §5, §5.1, Fig. 9, Fig. 11 |
| 10 | **Combinare i cinque gruppi** con la regressione sul P&L: domina il coefficiente dei market maker (0,3-0,75 in valore assoluto); professionisti con coefficiente negativo stabile. Fuori campione il solo segnale dei market maker fa **1,33 punti base al giorno** contro **1,25** del modello a cinque (dentro: 1,50 e 1,57): la combinazione si adatta troppo | §5.2, Fig. 12 |
| 11 | Con 45 varianti (gruppo x buy/sell/entrambi x NOTO/PHOTO/insieme): dentro 2,48, **fuori 1,68** punti base al giorno | §7.3, eq. 18, Fig. 19 |
| 12 | **Su quali opzioni**: la volatilita' implicita piu' alta (quarto quartile) e' il quartile migliore per broker, market maker e clienti (testo e Tab. 2); per i market maker il quarto batte in modo significativo tutti gli altri, per i broker tutti salvo il terzo. La Fig. 15 (quarto contro primo quartile, differenza di SR 2,88, p < 0,001) **non dice di quale gruppo e'**: le Fig. 13-14 sono dei market maker, ma i segni di significativita' della Fig. 15 seguono la descrizione dei broker (quarto contro terzo non significativo; terzo meglio solo del primo; delta migliori 1 e 3, come nella Tab. 2 per i broker) e contraddicono quella dei market maker su volatilita', gamma e delta. Le **put** piu' delle call; la gamma alta (opzioni al denaro) meglio per i market maker; **la scadenza non conta** | §6, Fig. 13-15, App. D Tab. 2 |
| 13 | **Buy contro sell**: per i clienti l'informazione sta **nelle vendite** (l'OVI dei soli acquisti e' quasi inutile, differenza di SR fra -1,40 e -2,00, p < 0,001); per i broker il contrario; le sole aperture rendono come il totale, le sole chiusure (molto meno volume) rendono poco | §7.2, Fig. 17, App. D |
| 14 | **NOTO**, secondo gli autori, e' coerente ma piu' debole (market maker "SR circa 2,5-3", §8), e su NOTO la giornata dopo tende a invertire la notte (pressione sul prezzo piuttosto che informazione). **Non verificabile**: le frasi su NOTO vengono dalla Fig. 18, la cui intestazione e' sbagliata (vedi "Cosa Non Regge", punto 4), e il "2,5-3" coincide con i valori di PHOTO chiusura-chiusura. Bonferroni su 675 prove: soglia 0,05/(5x5x3x3x3), circa 7,4 x 10^-5 | §7.3, §8, Fig. 18 |
| 15 | **Titolo per titolo** (market maker, Q3): 148 titoli rifiutano SR = 0 al 5%, **uno solo** con Bonferroni. Nessuna differenza significativa fra settori | App. D |
| 16 | **Effetto incrociato**: l'OVI dei market maker di un titolo predice la notte di altri titoli; con Bonferroni pieno da 209 (Q1) a 1.207 (Q5) archi su 1.792 titoli, e solo 2 auto-anelli in Q5 (che gli autori notano essere comunque piu' dei 0,67 attesi in una rete casuale della stessa densita'). Gli ETF hanno piu' archi in uscita della media | §7.4, Fig. 20, App. D |
| 17 | **Intraday, a 10 minuti** (2018-2019): con la correzione di Bonferroni per tutte le prove (3 gruppi Q x 10 rendimenti x 5 gruppi x 39 istanti) **nessuno SR e' significativo**, e nemmeno con Benjamini-Hochberg. Restano osservazioni qualitative: open-t OVI dei market maker con SR "moderatamente alto" verso mezzogiorno (massimo alle 13:10) per i rendimenti a 30-180 minuti, segno stabile durante la giornata; per gli altri gruppi lo SR cambia segno con t in modo quasi periodico (esempio: firm, 90 minuti, positivo prima delle 10:30, negativo dopo le 11:00) | App. E, Fig. 32-33 |
| 18 | Il volume di opzioni durante la giornata ha la **forma a U**, come il mercato a pronti | App. E, Fig. 31 |

## Il Perche'

**Degli autori** [MCH §1, §7.1, §8]: chi ha informazione (privata o da un modello) la usa con le opzioni,
per la leva; lo sbilancio fra scommesse al rialzo e al ribasso la rivela. I market maker non guadagnano
scommettendo ma sullo spread: il loro OVI e' lo **specchio** di chi scambia con loro, quindi ci si aspetta
che predica **al contrario** [MCH §1]. Che i rendimenti dei market maker non tornino indietro in 10 giorni
dice, secondo loro, che non e' pressione sul prezzo ma **almeno in parte informazione** [MCH §7.1]. I
clienti informerebbero soprattutto **vendendo** opzioni [MCH §7.2]. La volatilita' implicita alta indica
opzioni dove l'informazione e' piu' concentrata [MCH §6, senza un modello].

**Le alternative**: la pressione sul prezzo (Goncalves-Pinto e altri) e' discussa nella letteratura
[MCH §2.1] e scartata solo per i market maker, con l'argomento della mancata inversione; su NOTO gli autori
stessi leggono l'inversione della giornata dopo come pressione [MCH §7.3]. La copertura in delta dei
market maker, che per costruzione compra il sottostante quando il cliente compra call, **non e' discussa**.

**Cosa lo ucciderebbe**, dedotto dal perche': un'inversione nei giorni dopo anche per i market maker (sarebbe
pressione, non informazione); un segnale che sparisce dopo i costi (non provato); uno SR che non regge al
2020 e dopo (gli autori notano gia' un cambio di tendenza dopo il 2018 per i firm e "per altri gruppi"
[MCH §5]; per broker e clienti la giornata dopo peggiora nel 2019 [MCH App. C]).

## Cosa Abbiamo Riprodotto

Niente. Il dato del paper (Nasdaq per gruppo, per titolo) non e' il nostro.

## Cosa Non Regge, O Non Si Dice

1. **E' un preprint**, non revisionato, versione v1 del gennaio 2022. Ci sono segni di bozza: nel PDF
   sono rimasti messaggi della console di R ("## Warning: package 'DT' was built under R version 4.0.5",
   pagina 28). **Non e' un'istruzione**: lo segnaliamo come segno di cura editoriale scarsa.
2. **Testo e figure si contraddicono sui segni**. Il §5 dice "strong positive signals" per i broker, ma la
   Fig. 6 ha SR **negativi** (-2,13/-2,54), coerenti con la correlazione -0,6 fra broker e clienti; dice che
   i professionisti hanno "una tendenza al ribasso", mentre la Fig. 6 ha SR **positivi** (0,98/1,44) e
   l'App. C parla di "tendenza al rialzo". Il §5 dice "rendimento di circa il 30% in 5 anni" per i market
   maker, poche righe dopo aver scritto "fra 17% e 25%". Dove testo e figura divergono, qui vale la figura.
   *Verifica*: confermato sull'immagine della pagina 14: la curva dei broker scende (fino a circa -0,15), quella
   dei professionisti sale, e quella dei market maker finisce fra -0,18 e -0,24, cioe' 17-25%, non 30%.
3. **Le definizioni dei gruppi Q non tornano**: il §4 scrive Q2 = QB1 u QB2 u QB3 u QB4 (cioe' i **piu'
   deboli**), ma la frase prima ("il primo e' il 100%, il quinto il 20% piu' forte"), il §5 ("Q1 (tutti) o
   Q5 (il 20% piu' forte)") e il §7.4 ("Q3 = top 60%") dicono il contrario, e cosi' i numeri di titoli delle
   Fig. 6 e 10 (market maker: 886, 709, 532, 356, 313). **La didascalia della Fig. 10 non conferma**: presa
   alla lettera ("Q-k = il (20k)% piu' forte") darebbe Q1 = il 20% piu' forte, una terza definizione smentita
   dai numeri della stessa figura. Abbiamo letto Q come "il k-esimo gruppo esclude i segnali piu' deboli".
   Q5 inoltre non e' il 20%: per i market maker ha 313 titoli contro 356 di Q4, e per firm e professionisti
   Q3, Q4 e Q5 hanno lo stesso numero di titoli (217-219 e 115) e risultati quasi identici, segno di molti
   pari merito a |OVI| = 1 (nostra deduzione). Il §5 scrive anche che "i quinti bassi battono quelli alti",
   mentre la Fig. 7(b) mostra il contrario.
4. **L'intestazione della Fig. 18 e' sbagliata.** Sull'immagine della pagina 26 l'intestazione stampata e'
   davvero NOTO | PHOTO | NOTO+PHOTO in alto, e sotto ciascuno CLCL, CLOP, OPCL. Ma i valori seguono un altro
   ordine: le colonne 4-6, 13-15 e 22-24 (la terna centrale di ogni blocco di nove) riproducono **esattamente**
   PHOTO chiusura-chiusura, notte e giornata delle Fig. 6, 8 e 22-25, per tutti e cinque i gruppi e tutti i Q
   (30 serie di cinque valori, tutte uguali). Quindi il blocco esterno e' il **tipo di rendimento**, e PHOTO
   sta al centro di ogni blocco; quale delle altre due terne sia NOTO e quale l'insieme **non si puo'
   stabilire** da altre figure. Conseguenze: (a) le frasi del §7.3 e del §8 sono scritte in parte leggendo
   l'intestazione stampata (l'eccezione dei market maker NOTO con acquisti e vendite entrambi negativi; il
   "SR circa 2,5-3" dei market maker NOTO, che sono i valori di PHOTO chiusura-chiusura; l'esempio dei firm,
   dove peraltro i segni sono scambiati rispetto alla figura) e in parte si accordano solo con la lettura
   corretta (l'inversione fra notte e giornata dopo per broker-vendite e clienti-acquisti); (b) le colonne
   Buy e Sell di PHOTO non si conciliano nemmeno con la Fig. 17 (clienti, notte, Q1: Buy 3,51 contro Both
   3,20 nella Fig. 18, mentre la Fig. 17 da' i soli acquisti peggiori del totale di 1,40, p < 0,001). Dalla
   Fig. 18 prendiamo solo le colonne PHOTO "Both", confermate dalle altre figure; tutto il resto, NOTO compreso,
   resta non verificabile.
5. **Nessun costo**, e la "strategia" non e' eseguibile, lo dicono gli autori [MCH §4]. 1,3-1,9 punti base
   al giorno per dollaro sono lordi, su centinaia di titoli e con vendite allo scoperto ogni notte.
6. **Il segnale si conosce alle 16:00 e si entra alla chiusura delle 16:00**: l'ultimo aggiornamento di 10
   minuti e il prezzo di entrata sono simultanei. Gli autori lo coprono solo con una frase generica (il P&L
   "may not derive from a strategy that is executable", per "liquidity or timing constraints", §4); il punto
   preciso non e' discusso. L'App. E (open-t a mezzogiorno) attenua il
   dubbio per i market maker, ma senza significativita'.
7. **Moltissime varianti** (5 gruppi, 3 misure, 5 gruppi Q, 6 rendimenti, 6 schemi di peso, 8 caratteristiche
   delle opzioni per 4 quartili, buy/sell, 2 borse, periodi da 1 a 10 giorni, 39 istanti intraday). La
   correzione per prove multiple e' applicata solo nel §5.1 (peso per |OVI|), nel §7.3, nel §7.4, nell'App. D
   (titolo per titolo) e nell'App. E; i risultati principali (Fig. 6-7) sono test singoli. Con Bonferroni l'intraday **non regge** e, titolo per titolo, ne resta uno.
8. **Nessun modello nullo** nel nostro senso (la stessa regola su un mercato rimescolato), solo H0: SR = 0.
   La significativita' e' **per giornata** (una serie di P&L giornalieri del portafoglio), ed e' quello che
   chiede il nostro metodo; ma gli SR di 3-4 vengono da un portafoglio di centinaia di titoli, non da una
   serie sola.
9. **Il fuori campione** c'e' solo per la regressione sul P&L (finestre mobili di 500 giorni, spostate di 50
   e provate sui 100 dopo: le finestre di prova si sovrappongono, quindi la media fuori campione conta due
   volte quasi ogni giorno, Fig. 12 e 19); le Fig. 6-8 usano il segno
   di OVI senza parametri, ma le scelte (PHOTO come serie principale, Q3 come gruppo di riferimento,
   rendimento notturno in eccesso) sono fatte guardando i risultati.
10. **Non dice niente sugli indici**. Il rendimento e' **in eccesso su SPY**: per costruzione e' una prova
    **trasversale, neutrale rispetto al mercato** (quale titolo fara' meglio del mercato), non sulla
    direzione del mercato. Anche sui rendimenti grezzi i risultati restano significativi (§5, Fig. 8), e il
    modello a tre fattori di Fama e French non li cambia (nota 7), ma resta una prova su titoli singoli. Gli
    11 ETF non sono nominati. **Nostra deduzione, non del paper**: gli indici a regolamento in contanti (SPX,
    NDX) non hanno un rendimento CRSP e, dai dati descritti, non sono nell'universo; le opzioni SPX si
    scambiano al Cboe (fatto esterno al paper) e quindi non sono nei dati Nasdaq. L'unico cenno sono gli ETF con piu' archi "in uscita" nella rete
    incrociata [MCH App. D] e GLD fra i nodi centrali [MCH Fig. 30], senza numeri sul loro segnale.
11. **I gruppi non sono quelli del Cboe**. Nasdaq ha "Broker", il Cboe Open-Close ha "Broker-Dealer"; il
    "Customer" Nasdaq e' ogni cliente non professionale, non il retail per taglia d'ordine. Il paper non
    discute la corrispondenza.
12. **I market maker non hanno la divisione open/close**, e il loro OVI e' correlato a circa -0,6 con quello
    dei clienti (Fig. 3): il "segnale piu' forte" e' in buona parte lo stesso segnale dei clienti, rovesciato
    e con piu' volume (nostra lettura; gli autori lo presentano come l'immagine di tutti quelli che scambiano
    con loro, §1).
13. **Il campione finisce nel 2019**: prima del boom delle opzioni a scadenza giornaliera e del retail del
    2020-2021.

## Cosa Si Puo' Rifare Qui

Per la decisione 2 di CLAUDE.md lo strumento e' NQ con i dati di ATAS; il DEX per gruppo di OptionsDepth su
SPX entra **solo come contesto**, mai in un backtest. Quindi:

- **Il finding del paper non si rifa'**: serve il volume Nasdaq per gruppo e per titolo, che non abbiamo, e
  la prova e' trasversale fra titoli.
- **La misura si puo' prendere per il riquadro "Chi ha comprato delta oggi"**, come descrizione. Il DEX di
  OptionsDepth e' gia' firmato (delta comprato meno venduto) e per gruppo, ma e' un **netto per minuto e per
  strike**, non due gambe Up e Down separate ([`x-ray-in-vivo`](../../../metodo/x-ray-in-vivo.md), "Come Si
  Leggono"). Un indice alla OVI, fra -1 e +1, si avvicina cosi' (**nostra proposta, non del paper**):
  numeratore = DEX cumulato dall'inizio della seduta; denominatore = somma, dall'inizio della seduta, dei
  valori assoluti del DEX di ogni minuto e strike. Vale +1 se il gruppo ha solo comprato delta, -1 se ha solo
  venduto, 0 se ha girato senza direzione; sottostima il lordo, perche' acquisti e vendite nello stesso minuto
  e strike si compensano. Da dichiarare accanto al numero. Il paper non pesa per il delta: un OVI in contratti
  richiederebbe i volumi per gruppo, che X-Ray non pubblica (da verificare).
- **Il passo a 10 minuti e la somma dall'inizio** (open-t) sono la stessa forma del paper [MCH §3, App. E];
  l'aggiornamento al minuto di X-Ray si puo' campionare ogni 10 minuti per confrontarlo.
- **Cosa non si deve far credere al riquadro**: il paper non prova che un gruppo anticipi l'indice; a 10
  minuti non trova niente di significativo dopo la correzione; il gruppo "piu' informativo" (market maker)
  e' lo specchio degli altri. Una lettura "i clienti comprano, quindi sale" non ha base qui.
- **Una prova in avanti, solo se l'utente la chiede**: registrare ogni giorno l'indice alla OVI per gruppo
  alle 13:00 e alle 16:00 di New York, e metterlo contro il rendimento di NQ nella notte e nelle 1-3 ore dopo.
  Nostro calcolo, non del paper: con un'osservazione al giorno e una serie sola, la t di uno SR annualizzato e'
  circa SR x radice degli anni; uno SR di 1 (gia' molto per un indice) arriva a t = 2 in circa 4 anni. Lo
  storico del DEX c'e' solo da quando `XrayLive` lo salva (la famiglia `dex__` dal 31 agosto 2026, secondo
  x-ray-in-vivo): oggi si puo' solo descrivere.

## Cosa Ne Prende Il Framework

- **La misura normalizzata** (Up - Down)/(Up + Down) per gruppo, con 0/0 = 0, e le sue varianti dichiarate
  (volume, numero di operazioni, nominale), come modo di rendere confrontabili flussi di taglia diversa.
- **La lezione del doppio conteggio**: i gruppi non sono indipendenti; il market maker e' la controparte, e
  un gruppo puo' predire **al contrario**. Ogni lettura per gruppo deve dichiarare il segno atteso prima.
- **La lezione dell'intraday**: 39 istanti x 10 orizzonti x 5 gruppi fanno migliaia di prove; senza la
  correzione si vede una stagionalita' che con la correzione sparisce [MCH App. E].
- **Il test dello SR per giornata** (Bailey e Lopez de Prado, varianza di Bao) e il confronto di Ledoit e
  Wolf come misure accanto al nullo, non al suo posto.
- **Nessuna regola operativa**: preprint, niente costi, niente indici, intraday non significativo.

## La Verifica

**9 novembre 2026**, fase 4, su un PDF con lo sha256 della scheda (`db09bc5c...dfb82`, ricalcolato). Testo
estratto con `pdftotext -layout` pagina per pagina; **le 43 pagine rese come immagine** (con `pypdfium2`,
installato solo nella cartella temporanea della sessione) e guardate le figure 3, 6, 9, 13, 15, 17, 18 e 26.
Controllate **88 affermazioni** (scheda, termini, metodo, le 18 righe dei risultati con i loro numeri, il
perche', i 13 punti di "Cosa Non Regge", le citazioni del paper nelle sezioni finali): **75 confermate**,
**13 corrette**, **nessuna tolta**. Le formule (eq. 1-4, 17, 18), le soglie di Bonferroni (675 prove,
0,05/N^2 nella rete), i numeri delle Fig. 6, 7, 8, 12, 16, 17, 19, 20, 26 e dell'App. D ed E sono giusti.
Nel PDF nessuna istruzione rivolta al lettore; i messaggi della console di R a pagina 28 sono confermati.

I quattro dubbi del lettore:

- **Fig. 18**: la ricostruzione era giusta, e ora e' provata sull'immagine (30 serie di cinque valori
  coincidono con le Fig. 6, 8 e 22-25); resta incerto quale terna sia NOTO e quale l'insieme.
- **Segni nel §5**: la contraddizione e' vera, confermata sull'immagine della Fig. 6.
- **Q2 nel §4**: la formula del §4 e' sbagliata, ma la didascalia della Fig. 10 non era una conferma: e' una
  terza definizione, anch'essa smentita dai numeri.
- **Fig. 15**: l'attribuzione ai market maker era sbagliata; il gruppo non e' dichiarato e i segni seguono
  la descrizione dei broker.

Le correzioni:

1. Risultati, riga 2: clienti contro broker -0,6 vale solo su PHOTO; su NOTO la figura da' da -0,35 a -0,40.
2. Risultati, riga 12: la differenza 2,88 della Fig. 15 non e' "dei market maker"; aggiunto cosa il testo dice
   per ciascun gruppo.
3. Risultati, riga 13: "aprire o chiudere non cambia molto" diventa: aperture come il totale, chiusure deboli
   (App. D).
4. Risultati, riga 14: le affermazioni su NOTO (compreso il "SR 2,5-3" del §8) segnate come non
   verificabili, perche' vengono dalla Fig. 18 con l'intestazione sbagliata.
5. Risultati, riga 16: i 2 auto-anelli sono, per gli autori, piu' dei 0,67 attesi.
6. Cosa Non Regge, punto 3: la didascalia della Fig. 10 non conferma la lettura; Q5 non e' il 20% (pari merito).
7. Punto 4: riscritto con la prova sull'immagine, le frasi del §7.3 e del §8 scritte sull'intestazione
   sbagliata, e il disaccordo fra le colonne Buy e Sell e la Fig. 17.
8. Punto 6: il problema dell'ora c'e' nel paper, ma solo come "timing constraints" generici (§4).
9. Punto 7: gli schemi di peso sono 6, non 5; la correzione per prove multiple c'e' anche nel §5.1 e
    nell'App. D.
10. Punto 9: aggiunto che le finestre di prova (100 giorni, passo 50) si sovrappongono.
11. Punto 10: SPX al Cboe e indici fuori da CRSP segnati come nostra deduzione; aggiunti i rendimenti grezzi
    significativi e la nota 7 (Fama-French).
12. Punto 12: "quasi lo specchio" diceva piu' della correlazione di -0,6 (Fig. 3); ora la lettura e'
    dichiarata come nostra.
13. "In breve": "i dati non contengono SPX" e' una nostra deduzione, ora dichiarata.

Il punto 2 (segni del §5) e' fra le confermate, con l'immagine aggiunta.

**Resta incerto**: l'ordine fra NOTO e insieme nella Fig. 18, e quindi ogni numero su NOTO; il gruppo
della Fig. 15 (i broker sono la lettura piu' coerente, non una certezza); se le differenze delle Fig. 9 e 15
siano calcolate sullo SR con il segno o in valore assoluto (per market maker e broker lo SR e' negativo, e il
paper non lo dice); il disaccordo fra Fig. 17 e Fig. 18.
