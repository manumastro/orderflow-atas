# Come Si Risponde Dal Vivo

Stato: **obbligo**, riscritto il 16 ottobre 2026 (il punto, non i segnali). Riguarda l'**output**: cosa si dice, in che forma, con quali parole.
L'**input** — guardare tutto prima — sta in [`il-giro-d-orizzonte.md`](il-giro-d-orizzonte.md).

Chi legge **ha gli occhi sul grafico**, a mercato aperto, e deve capire in dieci secondi. La
lettura serve a dire quello che **non si vede guardando**.

---

## Si Fa Il Punto, Non Si Danno Segnali

**Dal 16 ottobre 2026 la lettura dal vivo fa il punto della situazione e non da' segnali.** Niente
`LONG`, `SHORT` o `NIENTE`, niente ingresso, stop, break even o bersaglio. Richiesta dell'utente:
*"vorrei che il giro normale non dica ad esempio NIENTE o altro di simile, quello e' basato su un
vecchio sistema che feci per cui l'agente mi deve dare dei segnali, invece deve solo fare il punto
della situazione utilizzando tutti gli studi fatti, considerando tutto il contesto possibile"*.
Sostituisce la richiesta del 17 settembre 2026 ("si dice la direzione"), che resta nella storia
del file.

**Il punto usa tutti gli studi, non solo il giro.** In ordine, dal piu' lontano al piu' vicino:

```text
1 IL QUADRO     l'ultimo studio globale del giorno (5-quadro.md) con la sua ora: cosa diceva
2 COSA E' CAMBIATO   da quel quadro: quali sue smentite si sono avverate, quali livelli sono caduti
3 LE FASI       macro, COT, opzioni, valore: una riga ciascuna, con l'eta' del dato
4 ADESSO        dove sta il prezzo nel valore (ieri, oggi, notte), il regime e la velocita' misurati
5 I PREZZI      i livelli che contano, ognuno con cosa cambia arrivandoci
6 COSA GUARDARE il discriminante: quale fatto osservabile cambierebbe il punto, nei due sensi
```

**Si dice sempre, perche' cambia la lettura:** se uno studio e' vecchio rispetto al prezzo (e
allora si propone di rifarlo), se il volume e' troppo sottile perche' una misura valga, se una
misura viene da una **barra non chiusa**, se una lettura precedente e' stata smentita, e **qual e'
il regime**. Le opzioni si spiegano in parole semplici: l'utente le sta imparando
(`.claude/skills/studio-globale/SKILL.md`, "Le regole che valgono per tutte le fasi").

**Le righe operative del pannello non si riportano.** L'indicatore porta ancora lo scenario
(`SERVE A`, bersaglio, pareggio), i veti e il consiglio di size del regime: il giro li toglie
(`senza_segnali` in `FabioOrderFlow/tools/giro_orizzonte.py`) e la risposta non li ricostruisce.

---

## La Forma: Breve, Con La Conclusione Prima

1. **Lo stato**, una frase. Dove siamo e cosa sta facendo il prezzo, e se e' deciso o no.
2. **La misura**, due o tre numeri. Solo quelli che sostengono la frase sopra.
3. **Il discriminante**, una riga per ramo. Quale fatto risolve, e cosa significa ciascun esito per il
   quadro (non per un'operazione).

Le sei voci del punto stanno dentro queste tre parti: il quadro e le fasi nello stato, adesso e i
prezzi nella misura, cosa guardare nel discriminante.

**Regole:**

- **Un numero entra solo se cambia la conclusione.**
- **Non si rielenca cio' che e' gia' sul chart.**
- **Un termine tecnico si spiega alla prima occorrenza della seduta**, non tutte le volte.
- **Prima la persona** (21 ottobre 2026: "vorrei che sia piu' comprensibile tutto all'umano"): lo
  stato e il discriminante si scrivono in parole comuni, senza sigle e senza codici interni, e ogni
  numero dice cosa significa per il prezzo. Le regole di scrittura sono quelle di **In parole
  semplici** in [`modello-del-rapporto.md`](../../../.claude/skills/studio-globale/modello-del-rapporto.md).
- **Se non e' cambiato niente, si scrive quella riga sola.**
- **Prima la conclusione, poi il perche'.**

**L'eccezione, e non e' negoziabile:** il file della giornata in [`../giornate/`](../giornate/) e
i documenti di metodo **restano completi**. Il diario si legge a mercato chiuso e deve contenere
tutto, comprese le misure che si riveleranno sbagliate — sono la parte verificabile.

---

## Il Modello E Il Regime Si Dichiarano

**Si dichiara quale dei tre modelli descrive il mercato, e lo decide la posizione del prezzo, non
l'umore.** E' una descrizione: dice che tipo di mercato e', non cosa fare.

```text
balance / mean reverting   il prezzo e' chiuso dentro la cash precedente  -> i bordi respingono verso il POC
momentum                   una balance viene rotta                        -> conta la speed of tape della rottura
trend following            giornata direzionale                           -> i ritracciamenti vanno nel verso del giorno
```

Leggere come momentum un mercato in balance, o come balance un prezzo fuori dal valore, e' lo
stesso errore con due nomi. Dettaglio e citazioni in
[`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

**E si dichiara il regime** come lo misura l'indicatore: direzionale (una candela copre piu' di
cinque candele del range precedente), balance, oppure choppy. Dice quanto il prezzo sta andando da
qualche parte (l'efficienza: netto su strada), non cosa fare.

---

## Un Livello Non E' Mai Il Fine, E' Sempre Una Porta

**Ogni volta che si nomina un prezzo, una zona o un livello, si dice a cosa serve arrivarci.**
Vale ovunque: letture dal vivo, file della giornata, etichette sul chart, risposte a una domanda.

Dal 16 ottobre 2026 si dice **cosa cambia del quadro** arrivandoci, non cosa permette di fare.
Un livello puo' fare **quattro cose**, e va detto quale:

| | cosa cambia arrivandoci |
|---|---|
| **cambio di modello** | il prezzo entra o esce da un valore: da balance a momentum, o il contrario |
| **tratto successivo** | apre la strada fino al prossimo livello, e allora si dice **quale** e **quanti punti** |
| **smentita** | smonta una lettura in corso o una riga del quadro: un rimbalzo smette di esserlo |
| **posizionamento** | dice chi resta intrappolato o liberato, e quindi chi dovra' agire |

**Sbagliato:** *"serve riprendere 29.372,50"* — riprenderlo per cosa?

**Giusto:** *"29.372,50 e' dove ieri si sono aperte le vendite: sopra, la strada e' aperta fino
alla mensola a 29.453,50, ottanta punti, e chi ha comprato stamattina smette di essere
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
