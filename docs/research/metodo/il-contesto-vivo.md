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
male proprio quando il prezzo si muove. Ci sono tre cose, in quest'ordine.

```text
15:59   NQZ6   29.294,50
VALORE     DENTRO cash  29.168,00-29.289,00  POC 29.195,00
           sopra il POC
IN GIOCO   VAH cash in sviluppo ~   29.289,00
           +5,50 sopra   arrivato da SOTTO
SFORZO     3.482 lotti al livello in 10 barre
           delta -412  venditori aggressivi
RISULTATO  toccato 4x, respinto 3x, passato 1x
SERVE A     SHORT  fade del bordo alto verso il POC (mean reverting)
           bersaglio 29.195,00 (-99,50)   invalida 29.336,25
SERVE      2 di 3
   [x] arrivato da dentro il valore
   [x] venditori al bordo (delta negativo li')   (-412)
   [ ] chiusura M1 di nuovo sotto il bordo
```

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

### 3. `SFORZO` e `RISULTATO` — la coppia con cui si legge l'assorbimento

**I numeri nudi non dicono niente.** 3.482 lotti sono tanti o pochi a seconda di cosa hanno
prodotto: il metodo legge **sempre la coppia** — quanto e' stato speso li', e se il prezzo e'
passato. Sforzo alto e risultato nullo e' assorbimento; sforzo alto e prezzo che passa e' una
rottura vera. Le due righe stanno una sopra l'altra proprio per non lasciare la sottrazione a chi
guarda.

- **`SFORZO`** e' la footprint sommata sulla fascia di due tick attorno al livello, nelle ultime
  `Lookback bars`: **non** il volume della barra, **non** quello della seduta. Il delta ha accanto
  chi e' stato aggressivo, che e' la sua definizione e non un giudizio: delta negativo vuol dire
  piu' scambiato in bid, cioe' venditori che colpiscono.
- **`RISULTATO`** e' cosa ne e' venuto fuori. Un tocco e' una barra che contiene il livello; e'
  *respinto* se chiude dallo stesso lato da cui veniva, *passato* se chiude dall'altro.

### 4. `SERVE A` e `SERVE` — lo scenario, e i prerequisiti che gli servono

**Una lista di condizioni spuntate senza uno scenario e' solo numeri.** `SERVE A` dice a cosa
servono: quale **direzione**, quale **setup**, verso quale **bersaglio**, e cosa lo **invalida**.
E' la stessa regola che vale ovunque nel repository — un livello non e' mai il fine, e' una porta,
e va detto a cosa serve attraversarla.

**Non sono condizioni armate, e la differenza e' tutta qui: non scattano, non avvisano, non fanno
niente.** Il vecchio impianto valutava condizioni e gridava, e per armarne una servivano sette
prove. Queste dicono soltanto cosa dovrebbe essere vero, e il pannello mostra quanti prerequisiti
sono gia' soddisfatti (`2 di 3`).

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
