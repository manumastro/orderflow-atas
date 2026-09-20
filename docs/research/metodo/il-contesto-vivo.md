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

## Cosa C'e' Nel Pannello: Una Cosa Sola

**Non e' un riassunto della seduta.** Un pannello che elenca tutto costringe a cercare, e si cerca
male proprio quando il prezzo si muove. C'e' il livello **in gioco** - quello entro `In-play radius`
punti dal prezzo - e le prove che servono a giudicarlo.

```text
15:34   NQZ6   29.192,50
IN GIOCO   Max cash ~   29.210,50
           -18,00 sotto   arrivato da SOTTO
A QUEL PREZZO, ultime 10 barre
           scambiati 3.482   d -412
           4 tocchi   3 respinti   1 passati
ADESSO     barra 15:34   v 1.714   d +40   pos 0.38
```

**Perche' proprio queste righe.**

- **Il lato di arrivo** e' cio' che separa un setup dal suo sosia. Un rifiuto del bordo alto del
  valore e una rottura dello stesso bordo dall'alto hanno massimo sopra, chiusura sotto, corpo in
  basso, volume e delta negativo: **identici**. Li distingue solo da che parte arriva il prezzo, e
  senza quella riga il pannello mostrerebbe due cose opposte con gli stessi numeri.
- **`A QUEL PREZZO`** non e' il volume della barra ne' quello della seduta: e' la footprint sommata
  sulla fascia di due tick attorno al livello, nelle ultime `Lookback bars`. **Sforzo alto e
  risultato nullo e' la misura dell'assorbimento del live**, e si vede solo prezzo per prezzo.
- **`tocchi / respinti / passati`** dice se il livello ha una storia o e' al primo esame. Un tocco
  e' una barra che lo contiene; e' *respinto* se chiude dallo stesso lato da cui veniva, *passato*
  se chiude dall'altro.
- **`~`** dopo il nome dice che il livello e' **vivo**, cioe' si muove da solo. Senza marcatore un
  POC che si sposta a ogni barra e una mensola scritta stamattina si disegnano identici, e chi
  guarda non sa se sta leggendo una misura di adesso o un fatto che potrebbe essere invecchiato.

**Quando nessun livello e' in gioco il pannello lo dice**, e mostra le due porte piu' vicine:
viaggiare in mezzo al niente e' una informazione, non un vuoto.

**Sotto restano le righe depositate su `/watch`**, che sono cio' che scrive l'analisi. Vanno tenute
distinte da cio' che misura la macchina, ed e' il motivo per cui stanno in fondo.

### Le Tre Manopole

Sull'istanza dell'indicatore, gruppo **Watch**:

| | default | a cosa serve |
|---|---|---|
| `In-play radius` | 12 punti | entro quanti punti un livello e' in gioco |
| `Lookback bars` | 10 | quante barre indietro si somma la footprint al livello |
| `Offset from top` / `from right` | 56 / 110 px | dove sta il pannello nell'area dati |

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
