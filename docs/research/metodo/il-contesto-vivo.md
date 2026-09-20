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
| **livelli vivi** — POC, VAH, VAL, estremi, bordi dei nodi | `livelli_vivi.py --ogni`, da solo | ogni 30 secondi |
| **pannello** — il livello in gioco e le prove per giudicarlo | **l'indicatore**, in C# | a **ogni tick** |
| **livelli statici** — mensole, nodi, prezzi del COT, rotture | **l'agente**, riscrivendo il file delle regole | quando la struttura cambia |
| **la lettura** — cosa significa | l'agente, su richiesta | quando si chiede |

**La separazione non e' negoziabile: il programma misura, l'agente interpreta.** Un programma che
classifica da solo finisce per chiamare assorbimento su un supporto quello che e' un rifiuto su una
resistenza, perche' il ruolo di un livello cambia durante la seduta e una barra non basta a saperlo.
E' gia' successo, ed e' il motivo per cui i sorveglianti erano deliberatamente grossolani.

## I Due Comandi, E Uno Solo Va Acceso

I **livelli vivi** hanno un processo, il **pannello** no: il pannello e' l'indicatore, e c'e'
finche' l'indicatore e' sul chart.

```bash
python3 FabioOrderFlow/tools/livelli_vivi.py \
        docs/research/giornate/livelli-vivi-NQZ6-AAAA-MM-GG.json --chart NQZ6 --ogni 30

# per vedere cosa uscirebbe, senza depositare
python3 FabioOrderFlow/tools/livelli_vivi.py ... --chart NQZ6 --prova
```

Gira **in background**, non come `Monitor`: non deve interrompere niente, deve solo esserci. Uno
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
15:49   NQZ6   29.313,00
VALORE     FUORI cash, sopra  29.170,00-29.239,00  POC 29.195,00
IN GIOCO   POC ASIA 43.5% ~   29.318,00
           -5,00 sotto   arrivato da SOTTO
LI'        3.482 lotti   d -412   4 tocchi, 3 respinti   (10 barre)
SERVE
   [x] arrivato da sotto: e' un bersaglio, non un muro
   [x] volume che sostiene l'arrivo   (3.482)
   [ ] delta positivo li' (nessun venditore in attesa)   (-412)
ADESSO     v 298   d +40   pos 0.68
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

### 3. `SERVE` — le condizioni, scritte dall'analisi e spuntate dalla macchina

**Non sono condizioni armate, e la differenza e' tutta qui: non scattano, non avvisano, non fanno
niente.** Il vecchio impianto valutava condizioni e gridava, e per armarne una servivano sette
prove. Queste dicono soltanto *cosa dovrebbe essere vero perche' questo livello diventi operabile*,
e il pannello mostra quali prerequisiti sono gia' soddisfatti.

Si dichiarano nel file delle regole, accanto al livello:

```json
"condizioni": [
  {"cosa": "arrivo",   "verso": "SOTTO", "testo": "arrivato da dentro il valore"},
  {"cosa": "delta",    "almeno": -150,   "testo": "venditori al bordo"},
  {"cosa": "volume",   "almeno": 2000,   "testo": "volume che sostiene l'arrivo"},
  {"cosa": "chiusura", "verso": "sotto", "testo": "chiusura M1 di nuovo sotto il bordo"}
]
```

| `cosa` | com'e' verificata |
|---|---|
| `arrivo` | il lato da cui il prezzo e' arrivato coincide con `verso` |
| `delta` | il delta **al livello** supera `almeno` (negativo: deve stare sotto) |
| `volume` | i lotti **al livello** superano `almeno` |
| `chiusura` | il prezzo sta oltre `prezzo` nel verso indicato; senza `prezzo`, oltre il livello |

**`testo` sono le parole dell'analisi e vengono mostrate com'e': la macchina non le interpreta.**
Un tipo `cosa` che non conosce lo lascia non soddisfatto e scrive *condizione sconosciuta*, invece
di far finta che sia vera.

## Sul Chart: La Banda Del Valore, E Cosa Conta

**Due righe orizzontali dicono dove sono i bordi; non dicono che in mezzo c'e' un dentro.** La
fascia fra VAL e VAH viene dipinta dietro le candele (`Show value band`, opacita' regolabile), e la
domanda *sono dentro o fuori?* diventa una cosa che si vede invece di una che si calcola.

**Cio' che conta per la strategia si disegna pieno, il contesto piu' spento.** Un livello e' chiave
se lo dichiara il file delle regole con `"chiave": true`; in mancanza, lo spessore vale come
dichiarazione. Senza questa distinzione dodici righe hanno tutte lo stesso peso visivo, e quella su
cui si decide non si trova a colpo d'occhio — che e' l'unico momento in cui serve.

**Un `~` dopo il nome dice che il livello e' vivo**, cioe' si muove da solo. Senza marcatore un POC
che si sposta a ogni barra e una mensola scritta stamattina si disegnano identici, e chi guarda non
sa se sta leggendo una misura di adesso o un fatto che potrebbe essere invecchiato.

### Le Manopole

Sull'istanza dell'indicatore:

| | gruppo | default | a cosa serve |
|---|---|---|---|
| `In-play radius` | Watch | 12 punti | entro quanti punti un livello e' in gioco |
| `Lookback bars` | Watch | 10 | quante barre indietro si somma la footprint al livello |
| `Offset from top` / `from right` | Watch | 56 / 110 px | dove sta il pannello nell'area dati |
| `Show value band` | Levels | acceso | dipinge la fascia del valore |
| `Value band opacity` | Levels | 18 su 255 | quanto e' marcata |

## I Livelli Statici: L'Agente Si Sveglia Da Solo

I livelli vivi si aggiornano programmaticamente. **I fissi no, e non devono**: sono affermazioni
dell'analisi — *questa mensola e' stata difesa sei volte*, *qui i Leveraged Funds hanno costruito
gli short* — e un programma non puo' scriverle senza classificare, che e' esattamente cio' che non
deve fare.

L'agente li rivede **a intervalli, di propria iniziativa**, e interviene solo se serve. Il segnale
che qualcosa va rifatto e' quello che il pannello gia' mostra:

1. il prezzo e' **uscito dalla fascia** per cui i fissi erano stati derivati;
2. un livello fisso ha cambiato **funzione** — un tetto diventato pavimento: il prezzo resta,
   l'etichetta no;
3. e' passata una **stampa che ridefinisce la giornata** (apertura cash, IVB, una news).

Quando non cambia niente, non si scrive niente. Un giro che non produce modifiche e' il caso
normale, non un fallimento.

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
