# Il Contesto Vivo: Il Chart Si Aggiorna Da Solo, L'Agente Costruisce Il Quadro

Stato: **procedura attiva dal 20 settembre 2026.** Sostituisce la sorveglianza a condizioni armate
descritta in [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md), che e' una **fase chiusa**.

## La Decisione, E Perche'

Richiesta esplicita dell'utente, 20 settembre 2026:

> *"non voglio l'agente che fa le operazioni ma che abbia e fornisca e aggiorna tutto il contesto
> necessario, questo e' lo scope e questo e' fondamentale"*

Il vecchio impianto chiedeva all'agente di **prevedere**: si scrivevano prima gli scenari, si
armavano le condizioni, e tre programmi gridavano quando scattavano. Aveva due difetti che non si
correggono stringendo le soglie.

- **Una condizione armata e' una previsione travestita da misura.** Per armarla servivano sette
  prove, si riscrivevano a ogni cambio di livello, e quando il mercato faceva qualcosa che non era
  previsto il sistema taceva — perche' non c'era una condizione per quello.
- **Il silenzio era ambiguo.** Fra un livello e l'altro i sorveglianti non parlavano, e un processo
  morto produce esattamente lo stesso silenzio di un mercato fermo. Il 17 settembre quel silenzio e'
  durato quattordici minuti con una posizione aperta dentro.

Il nuovo impianto non prevede niente. **Dice sempre tutto**, e chi guarda decide quando guardare.

```text
        vecchio                                  nuovo
  scenari armati -> grida quando scatta    contesto sempre a schermo, sempre fresco
  l'agente prevede                         l'agente misura e tiene aggiornato
  il silenzio e' ambiguo                   il pannello dichiara la propria eta'
```

## Chi Fa Cosa

| | chi lo fa | quando |
|---|---|---|
| **il prezzo di ogni livello** — POC, bordi, estremi, nodi, mensole, aggressione, assorbimento | **l'indicatore**, in C# | a **ogni barra** |
| **pannello** — il livello in gioco e le prove per giudicarlo | **l'indicatore**, in C# | a **ogni tick** |
| **quali regole**, su quale finestra, e a cosa serve arrivarci | **l'agente**, scrivendo il file delle regole | quando la struttura cambia |
| **la lettura** — cosa significa | l'agente, su richiesta | quando si chiede |

**La separazione non e' negoziabile: il programma misura, l'agente interpreta.** Un programma che
classifica da solo finisce per chiamare assorbimento su un supporto quello che e' un rifiuto su una
resistenza, perche' il ruolo di un livello cambia durante la seduta e una barra non basta a saperlo.
E' gia' successo, ed e' il motivo per cui i sorveglianti erano deliberatamente grossolani.

## Non C'e' Niente Da Accendere

Ne' i livelli ne' il pannello hanno un processo: sono entrambi l'indicatore, e ci sono finche'
l'indicatore e' sul chart. Le regole si depositano **una volta**, e da li' in poi il ricalcolo
e' dell'indicatore, a ogni barra.

```bash
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6 --file         docs/research/giornate/regole-dei-livelli-NQZ6-AAAA-MM-GG.json
```

Il resto di questa sezione descrive il processo esterno che faceva questo lavoro fino al
20 settembre 2026, ed e' conservato come evidenza. Uno
solo per chart. Si riavvia quando cambia il file delle regole? **No**: il file si rilegge a ogni
giro, apposta, perche' correggere una definizione non deve richiedere un riavvio.

**Il pannello sta nell'indicatore, non in un processo Python, e la ragione e' la cadenza.** Un
demone che rideposita ogni pochi secondi mostra la barra in formazione com'era due secondi fa - ed
e' proprio la barra in formazione quella che si guarda quando si decide. L'indicatore ha la
footprint sotto mano a ogni tick, e non dipendendo da nessun processo acceso **non puo' restare
fermo a mentire**.

## Cosa C'e' Nel Pannello: Il Valore, Il Livello, Cosa Serve

**Non e' un riassunto della seduta.** Un pannello che elenca tutto costringe a cercare, e si cerca
male proprio quando il prezzo si muove. Si legge dall'alto: **dove si sta** (il valore), **cosa si
sta testando** (il livello in gioco), **cosa e' passato li'**, **quanto vale in scala** (le
finestre larghe) e **a cosa serve** (lo scenario).

```text
15:59   NQZ6   29.294,50
REGIME     CHOPPY
           efficienza 0,12 (netto 74,25 su strada 609,00), 5 rotture rientrate, copertura si
           size ridotta, si rubano 1.000-2.000, due stop e si chiude
VELOCITA'  morta — 718 lotti, 29° percentile su 60 barre  (PROXY, non la speed of tape)
           6 big trade da 60+ lotti in 10 barre, netto -6
VALORE     DENTRO cash  29.168,00-29.289,00  POC 29.195,00
           sopra il POC
IN GIOCO   VAH cash in sviluppo ~   29.289,00
           +5,50 sopra   arrivato da SOTTO
AL LIVELLO 29.289,00 +/-0,50, ultime 10 barre
  sforzo   3.482 lotti scambiati a questo prezzo
  delta    -412 (-11,8%)  venditori aggressivi
  esito    toccato 4x, respinto 3x, passato 1x
  big      4 ordini da 60+ lotti, 314 lotti, netto -182
SU TUTTE LE BARRE, non solo al livello:
  30 barre +1.745 (+4,5%)  su 38.913 lotti
  da 13:30Z +2.300 (+4,9%)  su 46.730 lotti
SERVE A     SHORT  fade del bordo alto verso il POC (mean reverting)
           bersaglio 29.195,00 (-99,50)   invalida 29.336,25
           pareggio 29.318,00 (+23,50) — li' il lato opposto torna a vincere
SERVE      2 di 3
   [x] arrivato da dentro il valore
   [x] venditori al bordo (delta negativo li')   (-412)
   [ ] chiusura M1 di nuovo sotto il bordo
VETI       1, e ne basta uno
   ! regime choppy: size ridotta, niente bersagli lontani, due stop e si chiude
BARRA APERTA 15:59, cambia ancora
  787 lotti   delta +77 (+9,8%)   chiude nel 95% alto del suo range
```

**Ogni blocco dichiara in testata su cosa e' misurato**, e non e' pignoleria: fino al 20 settembre
2026 il pannello scriveva `SFORZO 3.482 lotti` e sotto `delta -412`, senza dire ne' a quale prezzo
ne' su quale finestra. Chi guardava leggeva quel delta come il delta della seduta, mentre erano i
soli lotti scambiati dentro una fascia di **due tick**, in **dieci barre**. Due numeri con lo
stesso nome e significati diversi e' il modo piu' rapido di leggere il chart al contrario.

### 0. `REGIME` e `VELOCITA'` — quello che nel live viene prima di tutto

**Sta in cima perche' decide la size, non il setup.** A chi gli chiede da dove cominciare Fabio
riduce tutta la sequenza a due cose: *"I would start from sensitive level of the market like we
are doing together, and regime"* `[5 · 1:18:06]`. I livelli li calcolava gia' il motore delle
regole; il regime, fino al 20 settembre 2026, era **una parola scritta a mano nel file della
giornata** — e da li' restava ferma finche' qualcuno non la riscriveva. Un regime fermo e' lo
stesso difetto del livello vivo fermo, spostato da un prezzo a una parola.

**Tre misure, e ciascuna risponde a una riga del live.** Sono misure di questo repository: il live
riconosce il regime a occhio, qui serve un numero, e allora ogni soglia esce accanto al numero che
produce.

| | cos'e' | riga |
|---|---|---|
| **copertura** | una candela copre piu' di **cinque** candele del range precedente | *"when you are directional, one candle cover more than five candle of the previous range"* `[6 · 1:19:04]` |
| **efficienza** | spostamento **netto** diviso la strada percorsa. **Misura nostra** | nel live la distinzione fra "va da qualche parte" e "balla" e' a occhio |
| **rientri** | rotture degli estremi recenti che rientrano entro tre barre | *"they go from one side of the auction to the other side"* `[6 · 54:03]` |

**Le tre non si sommano, si ordinano, e choppy ha la precedenza.** Una candela che copre cinque
candele dentro una finestra che torna al punto di partenza non e' una giornata direzionale: e' uno
strappo dentro il chop, ed e' proprio il caso in cui Fabio si aspetta il chop *dopo* l'esplosione —
*"usually what you see is profit release and then you start like this for hours and hours"* `[4 ·
1:05]`. Choppy vince anche perche' e' il regime dove l'errore costa di piu': una tossica presa per
direzionale fa aprire con size piena su rotture che rientrano, che e' il modo documentato di
restituire una giornata alle commissioni `[4 · 1:22:54]`. Il contrario fa perdere un treno, e un
treno perso non toglie soldi dal conto.

**La velocita' e' un PROXY, e la riga lo dice ogni volta.** La speed of tape e' *"how fast order
are being inputs"* `[3 · 1:08:41]`: il **ritmo** con cui gli ordini entrano. Il bridge non ha quel
dato. Quello che c'e' e' il volume della barra e quanto sta in alto nella distribuzione delle
ultime sessanta — un altro numero: mille lotti in dieci secondi e mille in sessanta danno lo stesso
volume e velocita' opposte. Per questo il proxy entra **nei veti e non nei prerequisiti**: un veto
su un proxy resta onesto, un permesso su un proxy no.

**I big trades sono la taratura di Fabio**, 60 lotti su NQ in cash `[5 · 6:04]`, e arrivano dal
tape vivo. **Zero e "non lo so" sono due cose diverse** e la riga le separa: il registro dichiara
da quando copre, perche' comincia a riempirsi quando l'indicatore si carica. Il 20 settembre,
subito dopo una ricarica, avrebbe scritto *"nessun ordine da 60+ lotti"* su una finestra che ne
conteneva **tre**. Adesso il registro si semina all'avvio con una richiesta storica di novanta
minuti, e quando la finestra chiesta comincia prima della copertura la risposta non e' un numero.

Lo stesso blocco esce dal bridge, perche' una misura che vive solo sul pannello obbliga l'agente a
rifarla a mano a ogni messaggio:

```bash
python3 FabioOrderFlow/tools/bridge.py regime --chart NQZ6
```

### 0bis. Cosa Il Pannello NON Dice Piu', E Dove E' Finito

**Il 20 settembre 2026 sono usciti dal pannello il conteggio delle regole, la legenda dei
marcatori e l'elenco di cio' che non e' stato disegnato.** Decisione dell'utente, e la ragione
regge: sul chart erano ingombro, e **nessuna delle tre serve a decidere** mentre il prezzo si
muove. Un pannello che elenca tutto costringe a cercare, e si cerca male proprio in quel momento.

**Non sono spariti: si sono spostati dove servono davvero.** Vivono su `/rules`, li stampa
`bridge.py rules`, e il giro d'orizzonte li mette alla **sezione 6ter** — cioe' in mano
all'agente, non davanti agli occhi di chi opera. Toglierli dallo schermo e' una scelta di
leggibilita'; perderli sarebbe stato un'altra cosa.

**Una riga di quel blocco e' rimasta, e si vede solo quando c'e' da vederla:**

```text
LIVELLI FERMI DA 4 BARRE — ultimo ricalcolo 15:55
```

In condizioni normali non stampa niente. **Non si toglie per fare spazio**, perche' un pannello
fermo e' indistinguibile da uno aggiornato, ed e' esattamente il guasto del livello vivo che
questo impianto e' nato per chiudere. Il motore adesso non puo' morire da solo, ma un'eccezione
dentro il ricalcolo produrrebbe lo stesso inganno in silenzio.

I tre marcatori restano sulle etichette dei livelli — `~` si muove, `=` misurato e fermo, `*`
dichiarato a mano — e la legenda sta in
[`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).

### 1. `VALORE` — dentro o fuori, e di quale

E' la prima riga dopo il prezzo perche' **e' la domanda che sceglie il modello**: dentro il valore
si fa mean reverting sui bordi verso il POC, fuori quel permesso non c'e'. Prendere un mean
reverting fuori dal valore e un momentum dentro la balance sono lo stesso errore con due nomi.

**Quando ci sono due aree vince quella dichiarata chiave, non la piu' vicina.** Durante la cash sul
chart ci sono almeno due valori — quello della notte e quello in sviluppo — e il piu' vicino non e'
il piu' importante.

### 2. `IN GIOCO` — il livello che il prezzo sta testando

Quello entro `In-play radius` punti. **A parita' di distanza vince quello dichiarato chiave**: un
bordo di contesto non deve rubare il posto al livello su cui si decide. Se il livello in gioco non
e' chiave, l'intestazione e' minuscola (`in gioco`).

- **Il lato di arrivo** separa un setup dal suo sosia. Un rifiuto del bordo alto del valore e una
  rottura dello stesso bordo dall'alto hanno massimo sopra, chiusura sotto, corpo in basso, volume
  e delta negativo: **identici**. Li distingue solo da che parte arriva il prezzo.
- **`LI'`** non e' il volume della barra ne' quello della seduta: e' la footprint sommata sulla
  fascia di due tick attorno al livello, nelle ultime `Lookback bars`. **Sforzo alto e risultato
  nullo e' la misura dell'assorbimento**, e si vede solo prezzo per prezzo.
- **`tocchi, respinti`** dice se il livello ha una storia o e' al primo esame.

**Quando nessun livello e' in gioco il pannello lo dice**, e mostra le due porte piu' vicine.

### 3. `AL LIVELLO` — sforzo e risultato, la coppia con cui si legge l'assorbimento

**I numeri nudi non dicono niente.** 3.482 lotti sono tanti o pochi a seconda di cosa hanno
prodotto: il metodo legge **sempre la coppia** — quanto e' stato speso li', e se il prezzo e'
passato. Sforzo alto e risultato nullo e' assorbimento; sforzo alto e prezzo che passa e' una
rottura vera. Le righe stanno una sopra l'altra proprio per non lasciare la sottrazione a chi
guarda.

La testata dice **il prezzo, la larghezza della fascia e quante barre**: `AL LIVELLO 29.289,00
+/-0,50, ultime 10 barre`. Tutto cio' che sta indentato sotto e' misurato **li' dentro** e in
nessun altro posto.

- **`sforzo`** e' la footprint sommata sulla fascia di due tick attorno al livello: **non** il
  volume della barra, **non** quello della seduta.
- **`delta`** e' la stessa somma, ask meno bid. Accanto c'e' chi e' stato aggressivo, che e' la
  sua definizione e non un giudizio: delta negativo vuol dire piu' scambiato in bid, cioe'
  venditori che colpiscono.
- **`esito`** e' cosa ne e' venuto fuori. Un tocco e' una barra che contiene il livello; e'
  *respinto* se chiude dallo stesso lato da cui veniva, *passato* se chiude dall'altro.

### 3bis. `SU TUTTE LE BARRE` — il delta in una finestra larga, che e' la scala

**Un delta non si giudica da solo, e questo e' il motivo per cui il blocco esiste.** `-412` al
livello e' una divergenza se la seduta sta a `+2.300`, ed e' la stessa direzione di tutti se la
seduta sta a `-4.000`. Senza il secondo numero il primo si legge come si vuole.

E' la misura che il **16 settembre 2026** ha smentito un permesso LONG tenuto in piedi per
ottantaquattro minuti da un gate orario: il prezzo era rientrato dentro l'IVB e il delta cumulato
in quello stesso intervallo era **negativo** →
[`il-permesso-si-misura-non-si-aspetta.md`](il-permesso-si-misura-non-si-aspetta.md).

**Sono barre intere, non la fascia al livello, e la testata lo dice apposta.** Sono due
popolazioni diverse: un delta di fascia si legge **contro** il delta di seduta, non si somma con
lui. Le due finestre sono `Lookback largo` (30 barre) e **dall'apertura della cash**
(`Apertura cash (UTC)`, `13:30Z` su NQ); il giorno lo prende dall'ultima barra e non
dall'orologio di casa, perche' in replay sono due date diverse.

**Il delta si stampa sempre anche come quota del volume** — `+1.745 (+4,5%)` — perche' la
percentuale dice se e' tanto senza dover sapere a memoria quanto scambia NQ: su cento lotti,
quattro e mezzo sono aggressione netta. Sotto i cento lotti la percentuale non si stampa: su un
campione minuscolo e' rumore travestito da misura.

### 3ter. `big` — la meta' mancante dello sforzo

**Mille lotti in ordini da due non sono un muro; mille lotti in sei ordini da centosessanta lo
sono.** Il volume da solo non distingue i due casi, e il live guarda sempre il secondo: *"look how
many absorption contract you have here on this horizontal level: 70, 75, 141, 33. This means that
there is a liquidity wall here. There is no other way"* `[6 · 1:01:24]`.

La riga conta gli ordini sopra la soglia **dentro la stessa fascia e la stessa finestra** dello
sforzo, cosi' i due numeri si leggono insieme senza doverli riconciliare.

### 4. `SERVE A` e `SERVE` — lo scenario, e i prerequisiti che gli servono

**Una lista di condizioni spuntate senza uno scenario e' solo numeri.** `SERVE A` dice a cosa
servono: quale **direzione**, quale **setup**, verso quale **bersaglio**, e cosa lo **invalida**.
E' la stessa regola che vale ovunque nel repository — un livello non e' mai il fine, e' una porta,
e va detto a cosa serve attraversarla.

**Il `pareggio` e' la terza riga, e non e' un di piu'.** Nel live Q1 la gestione **e' l'edge**, e
il break even ne e' la regola singola piu' importante: si mette **su un livello**, non dopo N
punti, ed e' il prezzo al quale l'analisi si smonta. *"Why I put the break even point at zero is
point at 65? This is where the buyers got completely absorbed. So it's a level where you could
expect to see sellers getting back in"* `[1 · 2:11:58]`. Uno stop senza il suo break even e' meta'
istruzione, e infatti `CLAUDE.md` obbliga a dichiararlo a ogni risposta operativa — mentre fino al
20 settembre il chart non aveva nemmeno il campo.

Si dichiara con `pareggio_livello` **per nome**, come bersaglio e invalidazione. Due differenze:
**non ha lo stacco minimo** dell'invalidazione, perche' il break even di Fabio e' deliberatamente
vicino — *"now you understand why my break even point was so close"* `[3 · 15:39]` — e se viene
toccato non si perde niente, si restituisce il tentativo. E quando manca **il pannello lo dice**
invece di tacere, perche' il campo vuoto e' esattamente il difetto.

**Non sono condizioni armate, e la differenza e' tutta qui: non scattano, non avvisano, non fanno
niente.** Il vecchio impianto valutava condizioni e gridava, e per armarne una servivano sette
prove. Queste dicono soltanto cosa dovrebbe essere vero, e il pannello mostra quanti prerequisiti
sono gia' soddisfatti (`2 di 3`).

### 5. `VETI` — e ne basta uno

**Non sono il complemento dei prerequisiti, e la differenza e' aritmetica.** I prerequisiti si
contano e servono **tutti**: `2 di 3` vuol dire che manca qualcosa. I veti no: **ne basta uno** e
il setup non si prende, quante che siano le spunte verdi sopra. Nel live sono una lista dichiarata
a parte `[4 · 17:37]`, `[2 · 44:17]`, `[2 · 1:56:36]`, `[6 · 44:25]`, e il veto che li riassume e'
*"we cannot force setups. Only when it's there"* `[4 · 43:22]`.

Il pannello ne valuta **quattro**, quelli misurabili con cio' che il bridge ha:

```text
mezzo range     il prezzo sta nel terzo centrale della finestra del regime e non c'e'
                nessun livello in gioco: niente strada pulita        [4 · 17:37]
libro sottile   nessun big trade sopra soglia e velocita' sotto il 30° percentile
                "no support from the aggressive order participants"  [2 · 44:17]
regime choppy   non vieta di operare: vieta la size piena e il terzo tentativo
controtrend     lo scenario dice SHORT ma il delta di seduta e' lungo, o viceversa
                "the auction is still long"                          [2 · 1:56:36]
modello sbagliato   uno scenario di mean reverting col prezzo FUORI dal valore
```

**I due che mancano non si simulano.** "Troppo vicini al muro" `[2 · 1:36:58]` e la posizione nella
curva del composito `[6 · 1:13:50]` chiedono un dato che questo chart non tiene. **Un veto
inventato e' peggio di un veto mancante**, perche' fa saltare setup buoni con l'aria di una misura:
restano dichiarati qui come mancanti, e li valuta l'analisi.

Il veto del mezzo range si vede **soprattutto quando non c'e' niente in gioco**, ed e' il caso in
cui il pannello prima usciva subito.

Si dichiarano nel file delle regole, accanto al livello:

```json
"scenario": {
  "direzione": "SHORT",
  "nome": "fade del bordo alto verso il POC (mean reverting)",
  "bersaglio_livello": "POC cash",
  "invalida_livello": "max cash"
},
"condizioni": [
  {"cosa": "arrivo",   "verso": "SOTTO", "testo": "arrivato da dentro il valore"},
  {"cosa": "delta",    "almeno": -150,   "testo": "venditori al bordo"},
  {"cosa": "chiusura", "verso": "sotto", "testo": "chiusura M1 di nuovo sotto il bordo"}
]
```

**Bersaglio e invalidazione si dichiarano per NOME di livello, mai come numero.** Il POC della cash
si sposta a ogni barra: un bersaglio scritto 29.195 sarebbe giusto per dieci minuti e poi sbagliato
in silenzio, che e' lo stesso difetto dei livelli non ridisegnati. Li risolve il motore dentro l'indicatore a
ogni giro; se un nome non esiste, **toglie il campo e lo dice**, invece di inventare un prezzo.

**Lo scenario va scritto dopo aver riconosciuto il modello**, non prima: dentro il valore si fa
mean reverting sui bordi verso il POC, una balance rotta e' momentum e senza speed of tape non si
prende. Il nome del modello sta nella riga, cosi' chi legge sa da dove discende.

| `cosa` | com'e' verificata |
|---|---|
| `arrivo` | il lato da cui il prezzo e' arrivato coincide con `verso` |
| `delta` | il delta **al livello** supera `almeno` (negativo: deve stare sotto) |
| `volume` | i lotti **al livello** superano `almeno` |
| `chiusura` | il prezzo sta oltre `prezzo` nel verso indicato; senza `prezzo`, oltre il livello |

**`testo` sono le parole dell'analisi e vengono mostrate com'e': la macchina non le interpreta.**
Un tipo `cosa` sconosciuto resta non soddisfatto e scrive *condizione sconosciuta*, invece di
passare per vero.

**Il volume come condizione e' un proxy della speed of tape, e va scritto nel testo.** Il bridge non
espone la speed of tape, e una condizione che la sottintende senza dirlo la fa passare per una
misura.

## Sul Chart: La Banda Del Valore, E Cosa Conta

**Due righe orizzontali dicono dove sono i bordi; non dicono che in mezzo c'e' un dentro.** La
fascia fra VAL e VAH viene dipinta dietro le candele (`Show value band`, opacita' regolabile), e la
domanda *sono dentro o fuori?* diventa una cosa che si vede invece di una che si calcola.

**Cio' che conta per la strategia si disegna pieno, il contesto piu' spento.** Un livello e' chiave
se lo dichiara il file delle regole con `"chiave": true`; in mancanza, lo spessore vale come
dichiarazione. Senza questa distinzione dodici righe hanno tutte lo stesso peso visivo, e quella su
cui si decide non si trova a colpo d'occhio — che e' l'unico momento in cui serve.

**Tre marcatori dopo il nome**, perche' le cose sono tre: `~` misurato su finestra ancora aperta,
`=` misurato su finestra chiusa, `*` dichiarato a mano dall'analisi. Senza, un POC che si sposta a
ogni barra, un massimo della notte ormai definitivo e una mensola scritta stamattina si disegnano
identici, e chi guarda non sa quale delle tre sta leggendo. Dettaglio in
[`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).

### Le Manopole

Sull'istanza dell'indicatore:

| | gruppo | default | a cosa serve |
|---|---|---|---|
| `In-play radius` | Watch | 12 punti | entro quanti punti un livello e' in gioco |
| `Lookback bars` | Watch | 10 | quante barre indietro si somma la footprint al livello |
| `Offset from top` / `from right` | Watch | 56 / 110 px | dove sta il pannello nell'area dati |
| `Show value band` | Levels | acceso | dipinge la fascia del valore |
| `Value band opacity` | Levels | 18 su 255 | quanto e' marcata |

## I Livelli Statici: Problema Aperto

I livelli vivi si aggiornano programmaticamente. **I fissi no, e non devono**: sono affermazioni
dell'analisi — *questa mensola e' stata difesa sei volte* — e un programma non puo' scriverle senza
classificare.

**Chi si accorge che sono scaduti, e chi li rifa', e' una domanda ancora senza risposta.** Il
tentativo del 20 settembre — un filtro che svegliava un sottoagente in background — e' stato chiuso
lo stesso giorno. Cosa e' stato costruito, cosa si e' visto, e le tre direzioni da cui ripartire
stanno in
[`i-livelli-statici-la-strada-del-sottoagente.md`](i-livelli-statici-la-strada-del-sottoagente.md).

**Quello che resta utilizzabile** e' il filtro, che non sveglia piu' niente e si lancia a mano
quando si vuole sapere se la mappa regge:

```bash
python3 FabioOrderFlow/tools/serve_rifare.py \
        docs/research/giornate/regole-dei-livelli-NQZ6-AAAA-MM-GG.json --chart NQZ6
```

Risponde `NIENTE` o `SERVE` con le ragioni — fuori fascia, attraversato, niente in gioco, sessione,
deriva. **Non giudica il mercato**: dice *quella riga sul chart non descrive piu' dove siamo*.

## Il Guasto Che C'Era, E Come Si E' Chiuso

**Quando il processo dei livelli moriva, le righe restavano sul chart identiche a prima.** Il
20 settembre e' morto due volte: la mattina insieme alla sessione, con undici livelli fermi mentre
il replay andava avanti, e la sera semplicemente non girava, con otto righe che sembravano di
adesso.

Era peggio di un livello fisso scaduto: **il `~` prometteva che quel livello si muoveva.** Un POC
fermo che si dichiara vivo inganna piu' di uno che non dichiara niente.

```text
il pannello   vive nell'indicatore, dichiara la propria eta'   -> non puo' mentire
i livelli     vivevano in un processo esterno, nessuna difesa  -> mentivano in silenzio
```

**La difesa non e' stata accorgersene: e' stata togliere di mezzo la cosa che moriva.** Il motore
e' passato dentro l'indicatore la sera del 20 settembre. Chart aperto, livelli di adesso; chart
chiuso, niente da ingannare — lo stato intermedio non esiste piu'. L'eta' il pannello continua a
dichiararla, perche' un'eccezione dentro il ricalcolo produrrebbe lo stesso inganno in silenzio.
Procedura e difese in
[`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).

## Cosa Non Fa, E Va Detto

- **Non avvisa.** Niente notifiche, niente suoni. Se serve guardare, si guarda il pannello.
- **Non arma condizioni.** Non esiste piu' un file di scenari da valutare.
- **Non dice la direzione.** Quella e' una lettura, si chiede, e segue il formato di `CLAUDE.md`.
- **Non tocca ordini o posizioni.** `/levels` e `/watch` restano le sole superfici di scrittura del
  bridge, e non entrano in nessun calcolo della piattaforma.

## La Fase Chiusa

`scenari.py`, `sveglia_tape.py`, `sveglia_movimento.py`, `comandi_sorveglianti.py` e i comandi
`/accendi` e `/spegni` restano nel repo come **evidenza di una fase conclusa**, insieme a
[`sorveglianza-del-tape.md`](sorveglianza-del-tape.md), che descrive le sette prove di una
condizione. Non si estendono e non si riaprono senza richiesta esplicita.

Cio' che di quella fase **e' sopravvissuto** sta nel pannello: le finestre mobili a 15/30/60 minuti,
il confronto col percentile della distribuzione recente, l'escursione dagli estremi, la distanza dai
livelli che contano. Erano le misure giuste; era il fatto di trasformarle in condizioni armate che
non funzionava.
