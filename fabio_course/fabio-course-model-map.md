# Mappa Del Modello Di Fabio

## Stato Del Documento

Questa mappa sintetizza, in ordine, le tre lezioni del corso:

```text
fabio1.txt -> fondazioni e selezione del regime
fabio2.txt -> lettura discrezionale dell'asta in tempo reale
fabio3.txt -> bias quantitativo multi-sessione e integrazione ibrida
```

E' una sintesi didattica, non una strategia eseguibile e non una specifica software. Le trascrizioni restano le fonti canoniche. I vecchi live YouTube sono usati soltanto nel confronto finale.

La mappa e' completa rispetto ai tre video disponibili nel repository. Non pretende di coprire eventuali lezioni successive del bootcamp: durante i video Fabio cita argomenti avanzati e altre sessioni non presenti nelle fonti attive.

## Tesi Centrale

Fabio non utilizza un singolo setup. Utilizza un processo adattivo:

```text
CONTESTO
  -> BIAS PROBABILISTICO
  -> REGIME D'ASTA
  -> LOCATION
  -> PARTECIPAZIONE
  -> CONFERMA
  -> PLAYBOOK
  -> GESTIONE
  -> AGGIORNAMENTO DELLA NARRATIVA
```

Il modello complessivo non predice un percorso fisso. Costruisce uno scenario iniziale e lo aggiorna quando nuovi scambi mostrano accettazione, rifiuto o un cambio di dominanza.

Il cuore universale non e' un terzo setup separato da mean reversion e continuation. E' il processo che decide:

- quale dei due regimi e' attivo;
- quale playbook e' ammesso;
- su quale scala temporale vale la lettura;
- quando l'informazione nuova invalida quella precedente;
- quando non operare.

## Mappa Complessiva

```text
1. CONTESTO
   Ultime sessioni, profilo composito, sessione precedente, pre-sessione
                         |
2. BIAS
   Direzione del valore accettato, ultima accelerazione, progressione dei POC
                         |
3. REGIME
   Equilibrio <-> transizione <-> squilibrio direzionale
                         |
4. LOCATION
   POC, area di valore, HVN, LVN, VWAP, estremi e livelli di massimo sforzo
                         |
5. PARTECIPAZIONE
   Delta, grandi esecuzioni, aggressione, assorbimento, risposta passiva
                         |
6. CONFERMA
   Sforzo con risultato, sforzo senza risultato, accettazione o rifiuto
                         |
7. PLAYBOOK
   Fade | Reversal | Trend | Momentum | Nessuna operazione
                         |
8. GESTIONE
   Protezione dietro l'invalidazione, pareggio, ricarico, target di asta
                         |
9. AGGIORNAMENTO
   Nuovo dato -> nuova gerarchia dei livelli -> conferma o cambio di regime
```

## Lessico Minimo

| Termine | Significato nel corso |
|---|---|
| Asta | Processo con cui compratori e venditori cercano un prezzo accettato |
| Bias | Direzione inizialmente favorita dalle informazioni disponibili, non previsione certa |
| Regime | Stato dell'asta: equilibrio, squilibrio o passaggio tra i due |
| Location | Posizione del prezzo rispetto a valore, profilo e livelli sensibili |
| Playbook | Famiglia operativa ammessa dal regime e dal contesto |
| Valore | Zona o prezzo sul quale il mercato ha mostrato accettazione, non semplice quotazione corrente |
| POC | Point of Control: prezzo con il maggiore volume nel profilo |
| Area di valore | Fascia che contiene convenzionalmente circa il 70% del volume |
| VAH / VAL | Limite superiore / inferiore dell'area di valore |
| HVN | High Volume Node: zona ad alto volume, nella quale l'asta ha sostato e negoziato |
| LVN | Low Volume Node: zona a basso volume attraversata rapidamente |
| VWAP | Prezzo medio ponderato per il volume; nel corso rappresenta il valore dinamico |
| Delta | Differenza tra volume eseguito aggressivamente in acquisto e in vendita |
| Aggressione | Ordini eseguiti contro la liquidita' disponibile per muovere subito l'asta |
| Assorbimento | Aggressione che non ottiene uno spostamento coerente del prezzo |
| Accettazione | Permanenza e costruzione di scambi oltre un livello o in una nuova area |
| Rifiuto | Incapacita' di costruire valore oltre un livello e ritorno verso l'area precedente |
| Protezione | Livello nel quale una parte ha mostrato sforzo e ottenuto controllo; non prova l'identita' del partecipante |
| Dealing range | Intervallo nel quale l'asta sta facendo affari e costruendo valore |
| Sessione cash | Orario regolare del mercato, distinto dalla sessione completa che include gli scambi estesi |

## Video 1 - Grammatica Dell'Asta

### Cosa Introduce

Il primo video costruisce il vocabolario necessario per scegliere un modello prima di cercare un'entrata.

1. I piccoli trader non sono la causa principale dei movimenti. Fabio attribuisce il movimento a pressione degli ordini, informazione fondamentale e pressione delle opzioni.
2. Il mercato e' un'asta che alterna soltanto due regimi primari:
   - equilibrio, con comportamento rotazionale;
   - squilibrio, con ricerca direzionale di un nuovo equilibrio.
3. Prezzo e valore sono informazioni diverse. Il prezzo puo' essere sopra o sotto il valore senza averlo ancora modificato.
4. Lo stesso livello cambia funzione con il contesto:
   - il POC e' un magnete dentro un equilibrio;
   - un vecchio POC puo' diventare un livello di rifiuto dopo l'accettazione di nuovo valore.
5. Il profilo descrive la forma dell'asta:
   - HVN come area negoziata e potenziale barriera;
   - LVN come area inefficiente e potenziale acceleratore;
   - distribuzioni singole o doppie;
   - concentrazione del volume in alto, al centro o in basso.
6. La pre-sessione fornisce un business range, una forma e livelli di possibile protezione prima dell'apertura.
7. Il delta orizzontale e le grandi esecuzioni aiutano a individuare dove compratori o venditori hanno mostrato il massimo sforzo.
8. Il regime puo' cambiare durante la sessione. Il profilo dell'intera giornata non basta: Fabio introduce profili delle singole oscillazioni e delle fasi locali.

### I Quattro Playbook

Il video organizza i playbook in una matrice due per due:

| Regime | Playbook | Logica |
|---|---|---|
| Equilibrio | Fade / completamento | Operare da prezzo caro o economico verso il valore, spesso il POC |
| Equilibrio | Reversal / rifiuto | Operare il fallimento di un tentativo di uscire dal valore |
| Squilibrio | Trend | Unirsi all'asta direzionale, spesso su rotazione o ricarico |
| Squilibrio | Momentum | Unirsi all'accelerazione dopo la rottura di una protezione |

Questi non sono quattro modelli indipendenti. Fade e reversal appartengono alla famiglia mean reverting; trend e momentum appartengono alla famiglia continuation.

### Cosa Aggiunge Rispetto Ai Vecchi Live

- spiega perche' POC, HVN e LVN cambiano ruolo con il regime;
- collega profilo di pre-sessione, livelli di protezione e apertura New York;
- esplicita il passaggio intraday da equilibrio a squilibrio e ritorno;
- distingue il profilo della sessione dai profili delle singole oscillazioni;
- presenta i quattro playbook come espressioni di due regimi, non come pattern isolati.

### Cosa Non Definisce Ancora

- criterio numerico per equilibrio o squilibrio;
- ancoraggio univoco dei profili locali;
- durata minima dell'accettazione;
- soglia universale delle grandi esecuzioni;
- regola completa di entrata, invalidazione e uscita.

## Video 2 - Dominanza, Reazione E Adattamento

### Cosa Aggiunge Al Video 1

Il secondo video trasforma i concetti statici in una lettura in tempo reale.

#### Sforzo E Risultato

La grande esecuzione non e' un segnale da sola. Fabio confronta:

```text
sforzo aggressivo + spostamento coerente = partecipante ricompensato
sforzo aggressivo + nessuno spostamento = assorbimento
fallimento da un lato + risultato dall'altro = possibile cambio di dominanza
```

Il risultato viene letto tramite il comportamento successivo del prezzo, inclusa la chiusura oltre il livello. La direzione dell'ordine non basta: conta se l'asta lo premia.

#### Livelli Di Dominanza

Fabio combina:

- limite dell'area di valore;
- massimo delta orizzontale della fase osservata;
- POC o cluster vicino;
- punto che ha prodotto rottura e accelerazione;
- successivi tentativi di difesa o recupero.

Una confluenza tra area di valore e massimo sforzo viene trattata come livello sensibile. Se il livello e' attraversato con risultato e il prezzo viene accettato oltre, l'asta puo' cambiare direzione.

#### Profilo Discrezionale Per Funzione

Il profilo viene tracciato in base alla domanda:

```text
profilo A->B direzionale  -> chi domina l'oscillazione?
profilo di consolidamento -> dove sono valore, bordi e protezioni?
profilo composito locale  -> quale informazione precedente e' ancora rilevante?
```

Fabio considera la scelta dell'ancoraggio una fonte di vantaggio discrezionale. Una finestra oraria fissa e' piu' ripetibile, ma puo' omettere una parte importante del dealing range.

#### Flip Dell'Asta

Il cambio non e' una semplice rottura grafica. La sequenza insegnata e':

```text
profilo e direzione correnti
-> livello di valore/protezione
-> massimo sforzo coerente con quel livello
-> attraversamento con risultato
-> accettazione oltre il livello
-> ricerca di un nuovo equilibrio
```

Un'idea precedente perde validita' quando il mercato distrugge il livello che la sosteneva. Fabio insiste sul reagire all'informazione corrente anziche' difendere una previsione.

#### Gerarchia Temporale

La lettura e' frattale: la stessa logica puo' essere applicata a sessione, consolidamento o singola oscillazione. Tuttavia le aste possono essere disallineate:

- asta di lungo periodo ancora short;
- asta locale temporaneamente long;
- trade locale possibile ma contrario al contesto e quindi diverso per durata e rischio;
- cambio completo solo dopo invalidazione e accettazione su scala superiore.

#### Gestione Dinamica

Il video mostra, senza congelare regole universali:

- stop dietro il livello che invalida l'idea;
- passaggio a pareggio quando l'asta conferma;
- ricarico quando l'aggressione riparte;
- uscita quando la parte seguita non e' piu' ricompensata;
- target verso il prossimo punto di equilibrio o protezione.

### Il Bivio Discrezionale, Quantitativo O Ibrido

Fabio distingue tre approcci:

| Approccio | Vantaggio | Limite |
|---|---|---|
| Discrezionale | Massima adattabilita' e uso del contesto | Dipende da esperienza e ancoraggio dei profili |
| Quantitativo | Bias ripetibile e minore carico decisionale | Perde flessibilita' nei cambi di regime |
| Ibrido | Bias predefinito, timing discrezionale | Richiede un contratto chiaro tra i due livelli |

Il video termina preparando il modello oggettivo del video 3.

### Cosa Non Definisce Ancora

- come scegliere algoritmicamente punto A e punto B;
- quanto movimento costituisce risultato;
- quante barre o quanto volume costituiscono accettazione;
- priorita' formale tra asta locale e asta superiore;
- regole ripetibili di ricarico, pareggio e uscita.

## Video 3 - Bias Multi-Sessione E Modello Ibrido

### Cosa Aggiunge Ai Video 1 E 2

Il terzo video separa esplicitamente tre informazioni:

```text
DIREZIONE -> valore accettato e contesto multi-sessione
LOCATION  -> profilo, area di valore, POC, VWAP e livelli protetti
TIMING    -> order flow discrezionale, sforzo contro risultato
```

Questa separazione e' il contributo piu' importante del corso rispetto ai vecchi live.

### Bias Quantitativo

Il modello osserva fino a cinque sessioni precedenti, usando sessione completa o sessione cash in modo coerente. La tesi e':

```text
fase 1 -> stato iniziale
fase 2 -> direzione in cui il mercato accetta nuovo valore
fase 3 -> maggiore probabilita' di continuare nella direzione del valore accettato
```

Gli indizi principali sono:

- POC che progrediscono nella stessa direzione;
- area di valore accettata piu' in alto o piu' in basso;
- rifiuto dei tentativi di costruire valore dalla parte opposta;
- ultima accelerazione non ancora invalidata;
- origine del movimento ancora protetta;
- profilo composito per sapere se il mercato piu' ampio e' in equilibrio o in espansione.

Il bias e' una probabilita' iniziale, non un ordine. Un singolo breakout locale non annulla ore o giorni di valore accettato.

### Direzione, Location E Timing

Esempio short:

```text
valore accettato piu' in basso       -> direzione short
VAH / POC / vecchia protezione       -> location di prezzo caro
venditori ricompensati o buy assorbiti -> timing
```

Esempio long speculare:

```text
valore accettato piu' in alto        -> direzione long
VAL / POC / VWAP / protezione buy    -> location di prezzo economico
compratori ricompensati o sell assorbiti -> timing
```

### Valore Statico E Dinamico

Il profilo fornisce valore statico per la distribuzione osservata. La VWAP fornisce un riferimento dinamico che evolve con gli scambi. Fabio usa la loro relazione per distinguere premio e sconto nella direzione del bias.

La combinazione diventa:

```text
bias long + prezzo a sconto + protezione buy + risultato = opportunita' long
bias short + prezzo a premio + protezione sell + risultato = opportunita' short
```

### Accetta, Valida, Continua

Per la continuation intraday Fabio descrive una progressione a gradini:

```text
rottura o espansione
-> ribilanciamento del movimento
-> accettazione del nuovo livello
-> validazione da nuove esecuzioni
-> continuazione verso la stazione successiva
```

Se il mercato non accetta il nuovo livello, torna verso il punto che aveva originato il movimento o verso il valore precedente.

### Compressione E Nessuna Operazione

Dentro una compressione:

- il POC e' un magnete;
- entrambe le parti possono mostrare protezione;
- le rotture non validate producono ritorni nel range;
- il centro non offre location efficiente;
- i bordi o la rottura confermata sono i punti decisionali.

Il modello completo deve quindi produrre anche `NESSUNA OPERAZIONE`.

### Portata Del Modello

Fabio presenta la logica come applicabile a:

- profili di singola oscillazione;
- scalping;
- intraday;
- piu' sessioni;
- swing trading;
- mercati diversi dal NASDAQ.

Questa e' una dichiarazione didattica. L'universalita' empirica non e' dimostrata dalle tre lezioni e deve essere verificata separatamente per strumento, feed e orizzonte.

## Il Modello Di Fabio In Una Frase

```text
Seguire la direzione in cui il mercato sta accettando valore, operare soltanto
in una location coerente, entrare quando la partecipazione ottiene risultato,
e cambiare idea quando il livello che sosteneva la narrativa viene invalidato.
```

## Relazione Tra Bias, Regime E Playbook

| Bias superiore | Regime locale | Lettura | Playbook possibile |
|---|---|---|---|
| Long | Equilibrio | Prezzo a sconto, bordo protetto | Fade o reversal long |
| Long | Squilibrio up | Accettazione superiore e buy ricompensati | Trend o momentum long |
| Long | Squilibrio down | Pullback contrario o possibile cambio | Scalp ridotto, attesa o invalidazione del bias |
| Short | Equilibrio | Prezzo a premio, bordo protetto | Fade o reversal short |
| Short | Squilibrio down | Accettazione inferiore e sell ricompensati | Trend o momentum short |
| Short | Squilibrio up | Pullback contrario o possibile cambio | Scalp ridotto, attesa o invalidazione del bias |
| Qualsiasi | Compressione centrale | POC magnete, segnali misti | Nessuna operazione |

La tabella e' una sintesi interpretativa del corso, non una regola gia' testata.

## Confronto Con I Vecchi Live YouTube

### Cosa Contenevano Gia'

Il primo vecchio live non era limitato concettualmente a due pattern. Dichiarava gia' che:

- il mercato alterna balance e imbalance;
- il modello e' dinamico, non una strategia rigida;
- la sequenza e' stato del mercato, location e aggressione;
- il trend model usa New York, squilibrio, LVN, grandi esecuzioni e target verso il precedente equilibrio;
- il mean reverting model usa consolidamento, ritorno dentro il balance, seconda oscillazione, LVN, grandi esecuzioni e target verso il POC;
- esiste una versione piu' avanzata basata sull'identificazione discrezionale delle compressioni.

Il secondo vecchio live mostrava soprattutto l'applicazione:

- profilo iniziale di New York;
- reazione da VAL verso VAH;
- assorbimento e grandi esecuzioni;
- momentum sul breakout;
- gestione dinamica, ricarichi e uscita rapida quando l'asta fallisce.

### Cosa Mancava O Restava Compresso

| Dimensione | Vecchi live | Corso in tre video |
|---|---|---|
| Scopo | Presentazione e dimostrazione di esecuzioni | Percorso didattico progressivo |
| Regime | Due stati spiegati rapidamente | Stati, transizioni e scale annidate |
| Bias | Prevalentemente profilo corrente e scenario live | Modello quantitativo da valore accettato multi-sessione |
| Contesto | Sessione o compressione selezionata | Fino a cinque sessioni e profilo composito |
| Valore | POC, VAH/VAL e LVN come location/target | Prezzo contro valore, magnete/reiettore, statico contro dinamico |
| Profilo | Giorno, swing A->B o compressione | Profilo scelto per domanda, fase, swing, sessione e composito |
| Partecipazione | Grande ordine come trigger | Sforzo contro risultato, assorbimento e cambio di dominanza |
| Cambio di modello | Citato come adattamento | Mostrato come flip, accettazione, ribilanciamento e nuova asta |
| Metodo | Due modelli operativi | Processo discrezionale, quantitativo e ibrido |
| Nessuna operazione | Filtro balance/imbalance | Compressione, POC magnete, disallineamento e dati misti |
| Gestione | Molto visibile ma legata agli esempi | Collegata a invalidazione e aggiornamento della narrativa |

### Errore Della Vecchia Riduzione

La vecchia ricerca trasformo' materiale incompleto in due contratti stretti:

```text
London mean reversion
New York impulse continuation
```

Questi contratti catturavano esempi reali, ma perdevano il livello che li governa:

- bias multi-sessione;
- ruolo variabile del valore;
- profili locali scelti per funzione;
- gerarchia tra asta superiore e locale;
- dominanza misurata tramite sforzo e risultato;
- cambio causale di regime;
- possibilita' di restare fuori.

Il corso mostra che mean reversion e continuation sono playbook subordinati, non il modello completo.

## Separazione Epistemica

### Direttamente Osservabile

- prezzi e orari degli scambi;
- OHLC delle barre;
- volume per prezzo;
- POC, area di valore, HVN e LVN una volta definita la finestra;
- delta per prezzo;
- grandi esecuzioni secondo una soglia dichiarata;
- VWAP;
- chiusura, attraversamento o permanenza oltre un livello.

### Derivato Ma Potenzialmente Formalizzabile

- profilo bilanciato o sbilanciato;
- progressione del valore;
- massimo sforzo orizzontale;
- premio o sconto rispetto a profilo e VWAP;
- risultato ottenuto dall'aggressione;
- accettazione o rifiuto;
- livello di invalidazione;
- regime locale e regime superiore.

### Interpretazione Non Direttamente Osservabile

- identita' dell'istituzione che sta operando;
- intenzione di mantenere, coprire o chiudere una posizione;
- fatto che gli stessi partecipanti tornino a proteggere un livello;
- motivazione fondamentale di una singola esecuzione;
- affermazioni sul comportamento desiderato dai market maker.

Fabio stesso chiarisce nel video 3 che si puo' osservare interesse, ma non sapere chi stia operando o per quale motivo.

## Affermazioni Da Non Trasformare In Regole Senza Test

- percentuale retail indicata nel video 1;
- probabilita' attribuite al ritorno in balance;
- frequenza di recupero dei gap intraday;
- superiorita' stabile di una particolare soglia per i big trades;
- applicabilita' identica a tutti i mercati;
- assenza di decadimento dell'edge nell'order flow;
- risultati personali o numerosita' di esecuzioni citati durante le lezioni.

Sono affermazioni, esempi o esperienza dichiarata, non evidenza fornita dal corso.

## Parti Ancora Discrezionali

Prima di ottenere un contratto implementabile devono essere definite:

1. selezione causale dell'inizio e della fine di ogni profilo;
2. classificazione numerica di equilibrio, squilibrio e transizione;
3. misura di skew e forma della distribuzione;
4. soglia adattiva delle grandi esecuzioni;
5. associazione tra aggressione e successivo risultato;
6. durata e volume necessari per dichiarare accettazione;
7. invalidazione di un livello protetto;
8. priorita' tra bias multi-sessione e asta locale;
9. condizioni precise per i quattro playbook e per nessuna operazione;
10. regole separate di entrata, gestione e uscita.

## Riferimenti Rapidi Alle Fonti

| Video | Passaggi chiave |
|---|---|
| Video 1 | `05:04` cause del movimento e big trades; `12:14` Auction Market Theory; `15:08` balance/imbalance; `19:05` prezzo e valore; `22:14` due regimi; `27:13` POC; `31:27` HVN/LVN; `39:44` quattro playbook; `46:54` pre-sessione; `59:32` cambio di regime; `01:00:30` profili locali |
| Video 2 | `03:17` livelli di protezione; `06:24` sforzo e risultato; `09:16` natura frattale; `16:24` fallimento contro risultato; `23:40` reazione contro previsione; `27:02` ancoraggio funzionale del profilo; `36:31` massimo sforzo; `38:11` flip dell'asta; `56:27` cambio di dominanza; `01:12:56` profilo direzionale o di consolidamento; `01:19:34` approccio oggettivo e ibrido |
| Video 3 | `04:30` cinque sessioni di contesto; `07:11` modello oggettivo; `07:45` valore accettato; `13:07` direzione/location/timing; `18:45` indipendenza dalla scala; `31:25` progressione dei POC; `36:10` bias quantitativo; `38:41` VWAP; `42:44` bias multi-sessione; `01:03:36` compressione; `01:18:39` accetta/valida/continua; `01:24:06` momentum; `01:32:35` intenzione non osservabile |

## Impatto Sul Progetto

La direzione corretta non e' ricostruire subito un indicatore mean reversion o continuation. Il futuro lavoro dovra' studiare e validare separatamente:

```text
A. contesto e valore accettato multi-sessione
B. classificazione del regime per scala
C. costruzione causale dei profili rilevanti
D. livelli di dominanza e protezione
E. sforzo contro risultato
F. selezione del playbook
G. timing e gestione
```

Nessuno di questi moduli e' ancora promosso a modello operativo. La presente mappa e' la base concettuale unica per i successivi contratti di ricerca.
