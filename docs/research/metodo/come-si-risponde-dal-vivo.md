# Come Si Risponde Dal Vivo

Stato: **obbligo**. Riguarda l'**output**: cosa si dice, in che forma, con quali parole.
L'**input** — guardare tutto prima — sta in [`il-giro-d-orizzonte.md`](il-giro-d-orizzonte.md).

Chi legge **ha gli occhi sul grafico**, a mercato aperto, e deve capire in dieci secondi. La
lettura serve a dire quello che **non si vede guardando**.

---

## Si Dice La Direzione, Non La Si Lascia Intendere

**Quando l'utente chiede cosa fare, si risponde con una direzione e i suoi numeri.** Non con una
descrizione da cui dedurla, non con un elenco di rami equiprobabili, non con un cappello di
cautele. Richiesta esplicita dell'utente, 17 settembre 2026.

```text
DIREZIONE      LONG, SHORT oppure NIENTE. Una parola, per prima.
INGRESSO       a che prezzo, e su quale evento (chiusura sopra X, ritorno su Y)
STOP           un prezzo, dietro una STRUTTURA di ordini eseguiti - mai un numero tondo
BREAK EVEN     il LIVELLO al quale l'analisi si smonta, e quale struttura lo giustifica
BERSAGLIO      un prezzo e i punti che dista
INVALIDA       il prezzo che smonta la lettura
```

**Il campo BREAK EVEN non e' decorativo**: nel live e' l'edge. Uno stop senza il suo punto di
break even e' meta' istruzione. Sta in
[`la-gestione-della-posizione.md`](la-gestione-della-posizione.md).

**`NIENTE` e' una direzione e si dice senza scusarsi.** "Non c'e' un setup, questi sono i due
prezzi che lo farebbero nascere" e' una risposta completa. Cio' che non e' una risposta e'
descrivere il tape e lasciare che sia chi legge a concludere.

**Le cautele di provenienza non entrano nella risposta dal vivo.** Restano nei file, che e'
tracciabilita'. **Quello che invece si dice sempre, perche' cambia cosa si fa:** se il volume e'
troppo sottile perche' il segnale valga, se una misura viene da una **barra non chiusa**, se una
lettura precedente e' stata smentita, e **qual e' il regime**.

**Non si da' gestione su una posizione senza sapere che l'utente e' dentro.**

---

## La Forma: Tre Parti, In Quest'Ordine, E Nient'Altro

1. **Lo stato**, una frase. Cosa sta facendo il prezzo, e se e' deciso o no.
2. **La misura**, due o tre numeri. Solo quelli che sostengono la frase sopra.
3. **Il discriminante**, una riga per ramo. Quale livello risolve, e cosa significa ciascun esito.

**Regole:**

- **Un numero entra solo se cambia la conclusione.**
- **Niente tabella sotto le quattro righe.**
- **Non si rielenca cio' che e' gia' sul chart.**
- **Un termine tecnico si spiega alla prima occorrenza della seduta**, non tutte le volte.
- **Se non e' cambiato niente, si scrive quella riga sola.**
- **Prima la conclusione, poi il perche'.**

**L'eccezione, e non e' negoziabile:** il file della giornata in [`../giornate/`](../giornate/) e
i documenti di metodo **restano completi**. Il diario si legge a mercato chiuso e deve contenere
tutto, comprese le misure che si riveleranno sbagliate — sono la parte verificabile.

---

## Prima Di Dichiarare Che Un Setup Esiste

**Si dichiara quale dei tre modelli sta girando, e lo decide la posizione del prezzo, non
l'umore.**

```text
balance / mean reverting   il prezzo e' chiuso dentro la cash precedente  -> fade dei bordi verso il POC
momentum                   una balance viene rotta                        -> con speed of tape, o non si entra
trend following            giornata direzionale                           -> si fade il ritracciamento nel verso del giorno
```

Prendere un setup momentum dentro la balance, o un mean reverting fuori dal valore, e' lo stesso
errore con due nomi. Dettaglio e citazioni in
[`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

**E si dichiara il regime**, perche' decide la size prima di decidere il setup: direzionale (una
candela copre piu' di cinque candele del range precedente), balance, oppure choppy. Su una
giornata choppy si riduce la size, si prendono profitti piccoli sui livelli, si accettano i break
even e si chiude al secondo stop.

---

## Un Livello Non E' Mai Il Fine, E' Sempre Una Porta

**Ogni volta che si nomina un prezzo, una zona o un livello, si dice a cosa serve arrivarci.**
Vale ovunque: letture dal vivo, file della giornata, etichette sul chart, risposte a una domanda.

Un livello puo' servire a **quattro cose**, e va detto quale:

| | cosa cambia arrivandoci |
|---|---|
| **permesso** | cambia cosa e' lecito fare: i bordi del valore per il mean reverting, la rottura della balance per il momentum |
| **bersaglio** | apre il tratto successivo — e allora si dice **quale** e **quanti punti** |
| **invalidazione** | smonta una lettura in corso: un rimbalzo smette di esserlo |
| **posizionamento** | dice chi resta intrappolato o liberato, e quindi chi dovra' agire |

**Sbagliato:** *"serve riprendere 29.372,50"* — riprenderlo per cosa?

**Giusto:** *"serve riprendere 29.372,50, dove ieri si e' aperto lo short: sopra, il primo
bersaglio e' la mensola a 29.453,50, ottanta punti, e chi ha comprato stamattina smette di essere
sott'acqua."*

**Corollario:** se non si sa dire a cosa serve un livello, quel livello non va nominato — e
probabilmente non andava nemmeno disegnato sul chart.

**E si marcano solo due tipi di livello**, come nel live: la **massima aggressione** e il
**massimo assorbimento impilato**, entrambi da ordini eseguiti. *"It's not necessary to mark
intermediate level that are useless for us."* Come si calcolano:
[`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).

---

## Prima Di Usare Un Termine Del Metodo

**Un termine del metodo non si parafrasa a memoria. Si apre la fonte e si legge il passo.**

Vale per: assorbimento, aggressione, risultato, reload, squeeze, failed auction, muro di
liquidita', speed of tape, big trades, flip dell'asta, gap, point of no return, station,
premium/discount — e per ogni termine che viene dal dossier: IVB, Tier 01/02/03, mean reverting,
Triple AAA, Triple A+, deep effort, 40 range, candle framing, block-and-reload, risk envelope.

[`glossario-del-metodo.md`](glossario-del-metodo.md) dice, per ciascuno, **a quale minuto di quale
lezione** (o a quali righe del dossier) tornare, cosi' la verifica costa un `sed -n` o un `grep`.

L'obbligo scatta **ogni volta**, non solo a inizio sessione, e in particolare quando la risposta
sembra ovvia: e' li' che si salta il controllo. Il 16 settembre l'agente ha definito "mean
reverting a rischio stretto" un setup che non era mean reverting, con un vincolo di rischio che
non era quello scritto.

**Una affermazione con conseguenza operativa deve essere tracciabile a un minuto del live.** Un
permesso, una regola di rischio, un bersaglio: se non si riesce a dire da quale minuto discende,
la frase va riscritta come osservazione.

---

## Prima Di Armare Una Condizione

**Una condizione non descrive un setup: lo separa dal suo sosia.** Ogni setup ne ha uno — un
movimento che produce gli stessi numeri e significa il contrario. Un rifiuto del bordo alto del
valore e una rottura dello stesso bordo dall'alto hanno massimo sopra, chiusura sotto, corpo in
basso, volume e delta negativo: identici. Li separa **solo** da che parte arriva il prezzo.

Le sette prove obbligatorie — lato di arrivo, sosia dichiarato, verso tracciabile alla fonte,
`--controlla`, `--prova` sulla storia, tempi del setup dichiarati, livello coperto nei due sensi —
stanno in [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md), sezione *"Le Prove Che Una
Condizione Deve Passare Prima Di Essere Armata"*. **Nessuna e' facoltativa, e valgono a ogni
riscrittura.**

**Una condizione che usa la derivata di una fascia deve nominare la fascia in cui il prezzo SARA'
quando la condizione viene valutata**, non quella che sta lasciando: il delta di una fascia si
congela appena il prezzo ne esce.

Le difese automatiche verificano che una condizione sia **eseguibile**. Non possono accorgersi
che sia **sbagliata**: quello lo fanno solo le sette prove, a mano.
