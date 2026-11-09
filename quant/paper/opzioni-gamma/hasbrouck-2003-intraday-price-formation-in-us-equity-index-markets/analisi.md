---
chiave: hasbrouck-2003-intraday-price-formation-in-us-equity-index-markets
sigla: HAS
titolo: "Intraday Price Formation in U.S. Equity Index Markets"
autori: [Joel Hasbrouck]
versione_letta: "versione di lavoro NYU Stern (FIN-00-046), bozza del 16 novembre 2000, 'Preliminary draft, not for attribution'; NON la versione pubblicata"
fonte: "Journal of Finance 58(6), 2375-2400 (2003), per la versione pubblicata"
identificativo: "10.1046/j.1540-6261.2003.00609.x"
url: https://archive.nyu.edu/jspui/bitstream/2451/27374/2/FIN-00-046.pdf
tipo: rivista (letta la versione di lavoro che la precede)
revisione_fra_pari: "si' per la pubblicata; la versione letta e' anteriore alla revisione"
pagine: 59
pdf: hasbrouck-2003-intraday-price-formation-in-us-equity-index-markets.pdf
pdf_sha256: 68f7d314210c1e6215064015fc38f5a24e300c9ed05188661511a41dedcaad3a
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "promessi programmi e dati sul sito dell'autore (www.stern.nyu.edu/~jhasbrou), non cercati; dati TAQ (ETF) e CME (future); che siano a pagamento e' un fatto esterno al paper"
---

# Hasbrouck 2003 (Versione Del 2000): L'Analisi

*Intraday Price Formation in US Equity Index Markets*, Joel Hasbrouck (NYU Stern). **Letta la
versione di lavoro del 16 novembre 2000** (bozza preliminare, "not for attribution"), scaricata dal
repository della NYU; la versione pubblicata (Journal of Finance 58(6), 2003, 26 pagine) **non e'
stata letta**: numeri, campione e conclusioni della pubblicata possono essere diversi, e prima di
citare il paper come "Journal of Finance 2003" va guardata quella. Letto per intero il 9 novembre 2026
(59 pagine, appendice e tabelle comprese). Le tabelle con `pdftotext -layout` escono mescolate; sono
state ricostruite con `pdftotext -raw`, che le da' riga per riga. Le formule escono senza lettere
greche: dove una formula e' ricostruita lo diciamo. Citazioni `[HAS §n]`, `[HAS Tab. n]`.

**Per la domanda della ricerca** (l'informazione di un indice aiuta a leggere l'altro? chi guida fra
S&P 500 e Nasdaq-100?) la risposta del paper e' quasi nulla: **stima i due indici separatamente**,
ognuno con i suoi tre strumenti, e l'unico numero incrociato e' **una frase senza tabella**: in un VAR
congiunto di SPY e QQQ, il contributo di QQQ alla scoperta del prezzo di SPY e' "al massimo circa il
10%" [HAS §7]. Il paper serve per **il metodo**: come si misura chi guida, giorno per giorno, a un
secondo, e come si controlla che il risultato non dipenda dagli orologi.

## I Termini

- **Scoperta del prezzo** (*price discovery*, o *price leadership*): quale mercato incorpora per primo
  l'informazione nuova, quando lo stesso valore (un indice) si scambia in piu' posti.
- **E-mini**: il future piccolo sull'indice (ES sull'S&P 500, NQ sul Nasdaq-100), che nel 2000 si
  scambiava solo sul book elettronico Globex; **il future regolare** (SP, ND, MD) si scambiava **alle grida** (*pit*, il recinto del CME) in orario di cassa; SP e ND erano cinque volte piu' grandi del loro E-mini (MD non
  aveva E-mini) [HAS §1, §2.b, Tab. 2].
- **ETF** (*exchange-traded fund*): il fondo quotato che replica l'indice: SPY (S&P 500), QQQ
  (Nasdaq-100), MDY (S&P 400), piu' nove ETF di settore dell'S&P 500 (XLB, XLE, XLF, XLI, XLK, XLP, XLU,
  XLV, XLY) [HAS §1, Tab. 1].
- **Base** (*basis*): future meno pronto, `F - S`; qui il pronto e' il prezzo dell'ETF riportato in
  punti d'indice [HAS §3.c, eq. 1-3].
- **Innovazione**: la parte di una variazione di prezzo che il passato non prevedeva, cioe'
  l'informazione nuova di quel secondo.
- **Risposta all'impulso cumulata**: di quanto si muovono in totale i prezzi, nei minuti dopo, a un
  colpo unitario in uno solo dei mercati [HAS §4.a, eq. 5, Fig. 5].
- **Cointegrazione**: due prezzi che singolarmente vagano a caso ma la cui differenza (qui la base)
  torna verso un valore fisso. E' il caso di "stesso titolo in mercati diversi" [HAS §4.c]; che non sia il caso di due indici
  diversi come S&P 500 e Nasdaq-100 e' una deduzione nostra: il paper non lo dice, ma per SPY e QQQ usa
  un VAR e non un VECM [HAS §7].
- **VECM** (*vector error correction model*): una regressione delle variazioni di tutti i prezzi sui
  loro ritardi, piu' un termine che li richiama verso il legame di cointegrazione [HAS §4.d, eq. 11].
  **VAR**: la stessa cosa senza quel termine, per prezzi che non sono legati.
- **Quota d'informazione** (*information share*, Hasbrouck 1995): la parte della varianza del
  "prezzo efficiente" comune (il movimento che resta) che nasce dalle innovazioni di un mercato. Se le
  innovazioni dei mercati sono correlate nello stesso secondo, la quota non e' unica: dipende da chi si
  mette per primo nell'ordinamento, e si danno **il minimo e il massimo** su tutti gli ordinamenti
  [HAS §4.b].
- **Prezzo medio** (*quote midpoint*): la media fra miglior denaro e miglior lettera. Il paper lo usa
  per gli ETF perche' si aggiorna prima dell'ultimo scambio; per gli E-mini, nel 2000, lo storico
  aveva solo gli scambi [HAS §5.c].
- **Ritardi polinomiali** (*polynomial distributed lags*): invece di stimare un coefficiente per ogni
  secondo di ritardo, i coefficienti si vincolano a una curva a tratti quadratica, per ridurne il
  numero [HAS §5.e, §5.g].

## La Domanda

Con l'arrivo degli ETF, degli E-mini elettronici e degli ETF di settore, **dove si forma il prezzo
degli indici azionari USA**? Il future alle grida, che gli studi precedenti trovavano in testa, ha
ceduto il posto al future elettronico o all'ETF? E gli ETF di settore, la tecnologia in particolare,
guidano l'indice largo? [HAS §1]

## Il Metodo

**I dati** [HAS §3, §5.c, Tab. 1-2]: dal 1 marzo al 31 maggio 2000, **64 giornate** di borsa, in
orario di cassa (quando tutti i mercati sono aperti insieme). ETF: dati TAQ, **prezzo medio** del
miglior denaro e lettera. Future regolari: l'ultimo prezzo battuto alle grida. E-mini: **l'ultimo
scambio** (lo storico Globex non aveva denaro e lettera). Ogni prezzo e' "l'ultimo noto" alla fine
dell'intervallo, anche se vecchio; l'appendice mostra che il prezzo "ultimo noto" resta stazionario in covarianza se
il processo degli scambi e' stazionario e il prezzo latente e' stazionario in covarianza condizionata
al processo degli scambi: due condizioni, non una [HAS §5.d, §9].

**La frequenza**: intervalli di **1 secondo** per S&P 500 e Nasdaq-100, con ritardi fino a 5 minuti;
5 secondi e ritardi fino a 10 minuti per l'S&P 400 [HAS §5.f, §6.a, §6.c]. Ritardi polinomiali a
tratti: punti di rottura 5, 10, 30, 60, 90, 120, 150, 180, 300 nelle stime a un secondo; 6, 12, 30,
60, 90, 120 in quelle a cinque [HAS §5.g, nota 11].

**Il modello** [HAS §5.g, eq. 12], ricostruito dal testo senza lettere greche:

```text
dp_t = B1 dp_t-1 + ... + BM dp_t-M + gamma (alpha' p_t-1 - alpha' pmedio_giorno) + u_t      (12)
```

`p` sono i tre prezzi (ETF, future regolare, E-mini), `alpha` il legame di cointegrazione (le
differenze fra i prezzi), `pmedio_giorno` **la media del giorno**: la base e' stazionaria dentro la
giornata ma scende di giorno in giorno verso la scadenza, quindi **il modello si stima giorno per
giorno**, e il risultato del campione e' la media delle 64 stime giornaliere, con l'errore standard
calcolato su quelle [HAS §3.c, §5.a, §5.b]. Niente intercetta, minimi quadrati ordinari.

**La quota d'informazione** [HAS §4.b, eq. 7-10]: dal modello si ricava la matrice A dell'impatto di
lungo periodo (eq. 6); la varianza della revisione del prezzo efficiente si scompone fra le
innovazioni dei mercati. Con innovazioni non correlate, `Var(w_1) = a11^2 sigma1^2 + a12^2 sigma2^2`
(eq. 9) e la quota del mercato j e' il suo termine diviso la varianza totale (eq. 10: **il numeratore
nel testo estratto e' illeggibile**, lo ricostruiamo dalla eq. 9). Con innovazioni correlate si
calcolano minimo e massimo sulle rotazioni [HAS §4.b].

**Le prove di robustezza**: il prezzo dell'E-mini ritardato **apposta** di 0, 5, 10 e 15 secondi, a
risoluzione di 5 secondi, per vedere quanto il risultato dipende dagli orologi di tre sistemi diversi
[HAS §6.e, Tab. 7].

**Gli ETF di settore** [HAS §7]: SPY e i nove ETF di settore, legati non dalla base ma dalla
replica: `p_SPY = somma a_k p_k` (regressione sui prezzi di chiusura, R quadro "essenzialmente 1"),
VECM a 5 secondi con ritardi fino a 5 minuti; la tabella riporta le quote **circa massime** (il
mercato j messo per primo) [HAS Tab. 8, Tab. 9].

**Niente fuori campione**, niente test formale di "A guida B" con un nullo: si riportano medie,
mediane, errori standard sulle giornate.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **S&P 500** (SPY, SP, ES; 1 secondo; 64 giornate): quota d'informazione media di **ES 0,854-0,882** (mediana 0,866-0,893; errore standard 0,012 e 0,011), SP 0,081-0,107, SPY 0,037-0,040. Correlazione delle innovazioni piccola (SP-ES 0,052), quindi minimo e massimo vicini. Coefficienti del prezzo efficiente: SPY 0,167, SP 0,199, ES 0,612 | §6.a, Tab. 4 |
| 2 | **Nasdaq-100** (QQQ, ND, NQ; 1 secondo): **NQ 0,836-0,853** (mediana 0,858-0,875; errore standard 0,012 e 0,011), QQQ 0,085-0,089, ND 0,061-0,076. Correlazione ND-NQ 0,031. Coefficienti: QQQ 0,305, ND 0,192, NQ 0,677 | §6.b, Tab. 5 |
| 3 | **S&P 400** (MDY, MD; 5 secondi; niente E-mini): **l'ETF guida**, MDY 0,616-0,629 (mediana 0,661-0,680), MD 0,371-0,384; deviazione standard fra giornate 0,26, molto piu' dispersa | §6.c, Tab. 6 |
| 4 | **Risposte all'impulso**: un colpo su SPY, SP, QQQ o ND rientra in fretta; un colpo su ES o NQ **resta** e trascina gli altri; tutto converge in circa 15 minuti | §6.a, §6.b, Fig. 5 |
| 5 | **Gli orologi** (5 secondi, quote medie minimo-massimo). ES con ritardo 0: 0,730-0,874; 5 s: 0,452-0,752; 10 s: 0,214-0,469; **15 s: 0,114-0,225**, e la quota passa al future regolare (SP 0,631-0,770). NQ con ritardo 0: 0,769-0,839; 5 s: 0,632-0,779; 10 s: 0,450-0,638; 15 s: 0,298-0,460 (ND 0,392-0,553). L'autore scrive che sull'S&P la quota di ES e' "sostanziale" anche a 10 secondi e viene superata solo a 15; **la tabella dice di piu'**: a 10 secondi SP (0,449-0,684) e' gia' sopra ES (0,214-0,469). Su NQ il primato regge a 10 secondi, a 15 ND (0,392-0,553) supera NQ. Il riassunto dell'autore ("non molto sensibile a errori di 5 o 10 secondi") vale per NQ, sull'S&P solo fino a 5 | §6.e, Tab. 7 |
| 6 | **Settori**: di SPY, 0,801 viene da SPY stesso; il settore che pesa di piu' e' la tecnologia, **XLK 0,097**; gli altri fra 0,014 e 0,021. Di XLK, 0,658 viene da SPY e 0,244 da XLK. XLK pesa 0,402 di SPY. XLI ha la correlazione giornaliera piu' alta con SPY (0,902, contro 0,811 di XLK) ma porta circa l'1,9%: **la correlazione non e' la scoperta del prezzo** | §7, Tab. 8, 9, 10 |
| 7 | **L'unico numero incrociato fra indici**: "un VAR congiunto delle variazioni di prezzo di SPY e QQQ" da' a QQQ un contributo alla scoperta del prezzo di SPY "al massimo anch'esso circa il 10%". Nessuna tabella, nessun errore standard, nessun numero per la direzione opposta (SPY verso QQQ) | §7 |
| 8 | **I costi non spiegano il primato**: costo relativo di andata (mezzo spread + 10 dollari di commissione) SP 0,0001, ES 0,0002, SPY 0,0020; ND 0,0001, NQ 0,0002, QQQ 0,0013. Gli E-mini non sono i piu' economici sui costi visibili | §3.b, Tab. 3, §6.d |
| 9 | **Volumi del 2000**: ES 73.779 contratti al giorno contro SP 98.324; NQ 30.991 contro ND 24.096. Open interest medio di fine mese: SP 384.476, ES 32.331, ND 36.189, NQ 18.247. Nel valore in dollari il future regolare domina, e nonostante questo l'E-mini guida | Tab. 2, §6.d |

**La conclusione dell'autore** [HAS §8]: per S&P 500 e Nasdaq-100 la scoperta del prezzo resta nei
future, e dentro i future nell'E-mini elettronico; gli ETF contribuiscono poco anche se in modo
statisticamente significativo; per l'S&P 400 i risultati sono "meno chiari" ma suggeriscono che guidi l'ETF; gli ETF di settore
contribuiscono poco all'indice largo, il che "suggerisce" che l'informazione non si produca a livello
di settore.

## Il Perche'

**Gli E-mini guidano** [HAS §6.d]: sono su un book elettronico accessibile in tempo reale da tutto il
mondo [HAS §1]; l'autore cita la tesi (Domowitz e Steil) che i sistemi elettronici costino meno e
finiranno per sostituire le grida, ma ammette che **i costi visibili non lo spiegano** (risultato 8):
**congettura** che altri costi (impatto sul prezzo, il "costo umano" di inserire l'ordine) favoriscano
l'elettronico. Non e' una prova, e lo dice.

**L'ETF guida l'S&P 400** [HAS §6.d]: congettura dell'autore: l'ETF costa molto meno dei titoli poco
liquidi del paniere, attira gli scambi di liquidita', e questi sostengono gli scambi informati.
"Le ragioni non sono chiare."

**I settori contano poco** [HAS §7, §2.c]: con i modelli a paniere (Subrahmanyam, Gorton e Pennacchi)
l'informazione privata si diversifica nel paniere largo, che attira gli scambi non informati; i
settori non sono benchmark, hanno pochi scambi di liquidita' e quindi poca informazione. Per la
tecnologia l'autore propone **veicoli migliori**: chi ha informazione tecnologica usa il Nasdaq-100.
Ma anche QQQ contribuisce al massimo il 10% a SPY (risultato 7). E' una spiegazione proposta, non
provata.

**Le alternative di metodo scartate**:

- misurare la guida con i soli coefficienti di correzione (la velocita' di aggiustamento in un passo):
  non dicono niente su piu' passi [HAS §4.e];
- il prezzo comune come media pesata dei prezzi (Ding e altri): citato, non usato [HAS §4.e];
- il VECM su tutto il campione con la costante (la prassi di allora): la base a dente di sega
  converge alla media del campione per effetto del tempo, non dell'arbitraggio; per questo la stima
  giornaliera [HAS §5.a];
- lo sfoltimento del campione (tenere solo gli istanti in cui tutti scambiano): costringe il mercato
  piu' veloce al ritmo del piu' lento e butta informazione [HAS §5.d];
- "la scoperta del prezzo e' solo correlazione di lungo periodo": smentito da XLI, molto correlato e
  quasi senza quota [HAS §7, Tab. 10].

**Cosa lo ucciderebbe**, dichiarato o dedotto:

- **un errore negli orologi** a favore dell'E-mini ribalterebbe il risultato sull'S&P 500: secondo
  l'autore a 15 secondi, secondo la sua tabella gia' a 10 (risultato 5); l'autore ammette che errori
  grandi peserebbero [HAS §6.e, Tab. 7];
- il primato vale **solo con tutti i mercati aperti insieme**: non dice cosa succederebbe se uno
  mancasse [HAS §6.d, §2.b];
- il paper misura **sottoinsiemi** dell'informazione: per gli E-mini solo gli scambi, non il book.
  L'autore presume che col book il vantaggio crescerebbe; non lo prova [HAS §6.d].

## Cosa Abbiamo Riprodotto

Niente, ancora.

## Cosa Non Regge, O Non Si Dice

1. **Non risponde alla domanda "S&P o Nasdaq, chi guida".** La ricerca lo aveva segnato "da verificare
   in lettura": **i due indici sono stimati separatamente**. Il solo numero incrociato (QQQ verso SPY,
   "al massimo circa il 10%") e' una frase senza tabella, senza errore standard, con un VAR e non un
   VECM (il paper non dice perche'; la mancata cointegrazione e' una deduzione nostra), e **senza la direzione opposta** [HAS §7]. Non si puo'
   citare come "l'S&P guida il Nasdaq": il paper non lo misura.
2. **64 giornate, un solo trimestre**: marzo-maggio 2000. Nessun sottocampione, nessun fuori campione.
   (Fatto esterno al paper, da tenere presente: e' il trimestre del massimo e del primo crollo del
   Nasdaq del 2000.)
3. **Prezzi di tipo diverso**: prezzo medio per gli ETF, ultimo scambio per gli E-mini, ultimo prezzo
   alle grida per i future regolari [HAS §5.c]. L'ultimo scambio rimbalza fra denaro e lettera, il
   prezzo medio no: il confronto fra mercati mescola due misure. L'autore non lo discute.
4. **La media del giorno guarda avanti.** Il termine di correzione usa la media dei prezzi di tutta la
   giornata [HAS §5.a]. Per descrivere va bene; **come segnale in tempo reale no**: alle 10:00 la media
   del giorno non si conosce.
5. **La guida si misura in secondi.** Con un ritardo di 10 secondi la quota di ES scende sotto quella
   del future regolare, con 15 e' ribaltata (risultato 5): la differenza fra chi guida e chi segue sta dentro pochi secondi. Deduzione nostra:
   su barre di un minuto la "guida" diventa movimento contemporaneo, e **non e' un segnale per chi
   legge il grafico a M1**.
6. **Nessun test di "A guida B" contro un nullo**: medie ed errori standard delle quote giornaliere,
   senza un confronto con quote ottenute da dati in cui per costruzione nessuno guida. L'errore
   standard assume giornate indipendenti [HAS §5.b].
7. **Le quote di settore sono circa massimi**, non valori puntuali, perche' le correlazioni dei
   residui le lasciano indeterminate (il mercato j messo per primo) [HAS Tab. 9]; sommate per riga
   superano 1 (riga di SPY: 1,040). L'introduzione dice che i settori pesano "circa il 10%" su SPY
   [HAS §1], la tabella da' 0,801 a SPY: il resto e' 0,199, e la somma dei massimi dei nove settori e'
   0,239; il testo del §7 dice "circa 80%" a SPY e "quasi 10%" a XLK. Il "10%" dell'introduzione
   sembra riferito a XLK (lettura nostra). Incoerenza di scrittura, non segnalata dall'autore.
7b. **I settori e QQQ sono misurati contro SPY, non contro ES**: per il §7 l'autore usa l'ETF come
   prezzo dell'indice (stessa borsa dei settori, stessi dividendi, orologi meno in dubbio), pur avendo
   appena mostrato che sull'S&P 500 guida l'E-mini [HAS §7]. Il "10%" di QQQ e quello di XLK sono
   quote dentro un sistema di ETF, che nel §6 portano meno del 5% della scoperta (SPY 0,037-0,040):
   non dicono quanto il Nasdaq o la tecnologia pesino sul mercato che guida.
8. **Il mercato non e' piu' quello.** Fatti esterni al paper, da verificare se servono: le grida dei
   future azionari del CME non ci sono piu', gli scambi ad alta frequenza, le opzioni a scadenza
   giornaliera e i book elettronici hanno cambiato la microstruttura. **Le quote di allora non si
   trasferiscono a oggi**; il metodo si'.
9. **Versione di lavoro, non pubblicata.** "Preliminary draft, not for attribution": numeri e
   conclusioni della versione del Journal of Finance (2003) vanno controllati li'.
10. **Non riguarda le opzioni**, anche se sta nel filone opzioni-gamma: e' il pezzo "trasmissione fra
    future" della catena che la ricerca vuole costruire.
11. **Conflitti**: nessuno dichiarato; un ringraziamento a Richard Hogan di Spear Leeds and Kellog
    (cosi' nel PDF; il ruolo non e' detto).

## Cosa Si Puo' Rifare Su NQ, Con ATAS

**Il paper non misura ES contro NQ; il suo metodo si'.** Con i dati di ATAS (CLAUDE.md, decisione 2)
si puo' misurare chi guida fra ES e NQ, e lo si fa cosi'.

**I dati che ci sono** ([`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md)): il **tape** del
continuo, scambio per scambio, di **NQ e di ES dal 1 gennaio 2023** al 16 e al 23 ottobre 2026, con
l'orario al millisecondo, il lato dell'aggressore e **denaro e lettera prima e dopo ogni scambio**;
piu' le barre M1 (NQ e ES dal 2017). Entrambi vengono **dallo stesso programma** (ATAS) e, per quanto
dice il documento dei dati, ES dalla connessione Rithmic della prop firm (per il tape di NQ la
connessione non e' scritta: da verificare): un solo orologio, se confermato, che toglie il problema principale del paper (tre sistemi, tre
orologi, risultato 5). Da verificare prima: che i due tape abbiano lo stesso riferimento di tempo
anche fra loro (il documento dei dati segnala uno sfasamento di 100-400 ms fra barre e tape del 2025,
non fra ES e NQ).

**Il fuori campione.** Per NQ il fuori campione e' dal 2025 in poi e **ogni sguardo lo consuma**
(CLAUDE.md, obbligo 2; `quant/registro/divisioni.json`). La misura esplorativa si fa **solo sul
2023-2024** (circa 500 sedute). Per ES la divisione **non e' ancora fissata**: va fissata prima di
guardare, con la procedura di [`come-si-valida`](../../../metodo/5-come-si-valida.md).

**Come si misura, passo per passo** (disegno nostro, sul modello del paper):

1. **Il prezzo**: il prezzo medio fra denaro e lettera di ES e di NQ, l'ultimo noto alla fine di ogni
   secondo, come fa il paper per gli ETF [HAS §5.c, §5.d]; in rendimenti logaritmici, perche' i due
   indici hanno livelli diversi.
2. **La seduta**: la cassa 09:30-16:00 di New York, convertita in UTC per data (`LeSedute.cs`,
   `sedute.py`); a parte, la notte, dove la liquidita' e' diversa.
3. **Il modello**: ES e NQ **non sono lo stesso titolo** e non sono cointegrati con `[1, -1]`, quindi
   **non** il VECM della tabella 4 ma un **VAR** sui rendimenti a 1 secondo, come il VAR di SPY e QQQ
   del paper [HAS §7], con ritardi fino a 5 minuti e ritardi polinomiali (o i soli primi 30-60
   secondi). **Una stima per giornata**, poi media ed errore standard sulle giornate [HAS §5.b]: e'
   anche la nostra regola, la significativita' per giornata (CLAUDE.md, obbligo 3).
4. **Le misure**, nei due versi: (a) la quota della varianza di lungo periodo di NQ che nasce dalle
   innovazioni di ES, e viceversa, con **minimo e massimo sui due ordinamenti**; (b) la risposta
   cumulata di NQ a un colpo su ES (e viceversa) a 1, 5, 30 secondi e 5 minuti, come la figura 5; (c)
   come controllo semplice, la correlazione incrociata dei rendimenti a ritardi da -30 a +30 secondi
   (le "regressioni anticipo-ritardo" che il paper giudica un test di causalita' senza misura di
   importanza [HAS §2.a]).
5. **Gli orologi**: rifare la stima con ES ritardato apposta di 0, 1, 2, 5 secondi, e poi NQ, come la
   tabella 7: se la guida cambia di segno con un secondo di ritardo, non c'e' una guida da leggere.
6. **Il nullo**: la stessa procedura su una coppia sintetica di passeggiate casuali con innovazioni
   correlate e **nessun ritardo** (le quote devono uscire simmetriche), e su ES di un giorno con NQ di un
   altro giorno (le quote incrociate devono sparire). Senza il nullo il numero non e' un risultato
   (CLAUDE.md, obbligo 3).
7. **Le condizioni**, solo dopo la misura di base e dichiarate prima: giornate di notizie macro contro
   giornate calme, giornate con NQ che si muove piu' di ES contro il contrario, segno della gamma dei
   dealer quando ci sara' la storia registrata.

**Cosa aspettarsi, e cosa no.** Il paper non da' un numero per ES contro NQ; quello piu' vicino (QQQ
verso SPY, al massimo il 10%, nel 2000) dice soltanto che allora l'ETF tecnologico non guidava
l'indice largo. **Una guida misurata in secondi non e' un segnale su M1** (limite 5): per lo studio
globale servirebbe come **lettura** (quale dei due muove il comune nelle ore di cassa, e se cambia nei
giorni di notizie), non come regola.

**Cosa non si puo' rifare**: le grida (non ci sono piu'); gli ETF SPY e QQQ (non vengono da ATAS; il
prezzo di QQQ da Yahoo e' solo contesto, CLAUDE.md decisione 2); i settori dell'S&P 500.

## Cosa Ne Prende Il Framework

- **Il metodo per dire chi guida**: quota d'informazione con minimo e massimo, risposte all'impulso
  cumulate, a un secondo, **stimate giorno per giorno** e mediate sulle giornate con l'errore standard
  delle giornate.
- **La prova degli orologi**: ritardare apposta un mercato e guardare quanto resiste la guida.
- **Il prezzo medio invece dell'ultimo scambio**, e "l'ultimo noto" alla fine dell'intervallo, con la
  giustificazione dell'appendice (processo degli scambi stazionario e prezzo latente stazionario in
  covarianza condizionata) [HAS §9].
- **La correlazione non e' la guida** (XLI contro XLK): un mercato molto correlato puo' non portare
  informazione.
- **Il ruolo e' `metodo`**: nessun finding su ES contro NQ da trasformare in idea. La misura descritta
  sopra e' una lettura da costruire, non una strategia; nessuna regola operativa ne discende.

## La Verifica

**9 novembre 2026**, verifica avversaria contro il PDF (59 pagine, sha256 confermato:
`68f7d314...caad3a`). Testo estratto con `pdftotext -layout` e `-raw`; le pagine non si sono potute
vedere come immagine (manca `pdftoppm`), quindi **le eq. 10 e 12 restano ricostruite e non
confermate**: nel testo grezzo dell'eq. 12 si leggono i ritardi, un simbolo prima di `p_t-1` e uno
prima di `p day`, compatibili con la ricostruzione; dell'eq. 10 si leggono solo i pedici `ij` e
`Var(w_it)` al denominatore, e la ricostruzione a parole ("il suo termine diviso la varianza totale")
e' coerente con l'eq. 9, che invece si legge (`a11^2 sigma1^2 + a12^2 sigma2^2`). Le pagine citate
sotto sono quelle stampate nel PDF ("Page n"; la pagina del PDF e' n+1).

**Controllate circa 140 affermazioni**: 83 numeri delle tabelle 2-10 e del testo, tutti confermati
(Tab. 2 p. 44, Tab. 3 p. 45, Tab. 4 p. 46, Tab. 5 p. 47, Tab. 6 p. 48, Tab. 7 p. 49, Tab. 8 p. 50,
Tab. 9 p. 51, Tab. 10 p. 52; il "circa il 10%" di QQQ a p. 34, quello dell'introduzione a p. 4), e
circa 60 fra citazioni, finding e "l'autore dice". **Corrette 11, in 14 punti del testo; tolte 0; aggiunta 1** (un limite che
mancava). Nessuna istruzione trovata nel PDF.

Le correzioni:

1. **Risultato 5, orologi (la piu' grave)**: l'analisi diceva che il primato degli E-mini "regge a
   errori di 5-10 secondi"; e' il riassunto dell'autore (p. 32), ma **la Tab. 7 lo smentisce per
   l'S&P**: a 10 secondi SP 0,449-0,684 contro ES 0,214-0,469. Regge a 10 solo su NQ. Corretti di
   conseguenza anche "Cosa lo ucciderebbe" e il limite 5 ("quasi sparisce" a 15 secondi: e' ribaltato,
   e gia' superato a 10).
2. **Limite 7, i settori**: "circa 0,20 se si somma, con i massimi" mescolava due conti: 1 - 0,801 =
   0,199, la somma dei massimi dei settori e' 0,239. E le quote sono massimi per le correlazioni dei
   residui (Tab. 9, p. 51), non "perche' a 5 secondi le correlazioni crescono", che il §6.e dice della
   Tab. 7. Il dubbio del lettore sul "circa il 10%" dell'introduzione resta: e' un'incoerenza del paper.
3. **Aggiunto il limite 7b**: i settori e QQQ sono misurati contro SPY, non contro l'E-mini che guida
   (§7, p. 32).
4. **Il metodo e il framework, l'appendice**: le condizioni sono due (scambi stazionari e prezzo
   latente stazionario in covarianza condizionata, eq. 21, p. 37), non "basta la frequenza stazionaria".
5. **I termini, E-mini**: "SP, ND, MD cinque volte piu' grandi": MD non ha E-mini; il rapporto vale per
   SP e ND (p. 3, Tab. 2).
6. **I termini, cointegrazione**: "non e' il caso di S&P 500 e Nasdaq-100 [HAS §4.c]": il §4.c non lo
   dice; ora e' segnata come deduzione nostra. Lo stesso nel limite 1 ("i due indici non sono
   cointegrati").
7. **La conclusione dell'autore**: per l'S&P 400 il paper dice "meno chiari", e per i settori
   "suggerisce" (p. 35); l'analisi li dava come affermazioni piene.
8. **Risultato 9**: l'open interest e' la media di quello di fine mese (Tab. 2).
9. **Limite 11, conflitti**: il ringraziato e' Richard Hogan di Spear Leeds and Kellog; che sia "un
   operatore" il paper non lo dice.
10. **Scheda**: "dati a pagamento" non e' nel paper, segnato come fatto esterno.
11. **Cosa si puo' rifare**: "stessa connessione (Rithmic)" per NQ e ES: `i-dati-da-atas.md` la scrive
   solo per ES; ora e' da verificare.

Non controllabili sul PDF, e lasciati come scritti: rivista, volume, pagine e doi della versione
pubblicata, e l'url (vengono dal file della ricerca); i fatti esterni gia' segnati come tali (marzo-maggio
2000 e il Nasdaq, le grida chiuse).
