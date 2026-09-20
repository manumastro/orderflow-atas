# Come Lavora L'Utente, E Cosa Si Aspetta Da Una Misura

Stato: **contesto**. Non e' metodo di mercato: e' come va condotto il lavoro, e da dove vengono
alcune regole di `CLAUDE.md` che altrimenti sembrano arbitrarie.

Questo documento sta nel repo e non nella memoria dell'agente **apposta**: la memoria vive in
`~/.claude/projects/<percorso>/memory/`, non viaggia con `git` e cambia nome a ogni macchina. Il
20 settembre 2026, spostando il repo dal Mac a un PC, e' sparita tutta — e con lei il motivo di
meta' delle regole. Vedi
[`portare-il-repo-su-un-altra-macchina.md`](portare-il-repo-su-un-altra-macchina.md).

## Come Lavora

Opera sul **Nasdaq** con **ATAS X**, in italiano, studiando il corso di order flow di Fabio
Valentini. Fa **profile framing giornaliero** e tiene aperti in parallelo i chart dei contratti
singoli per il tape.

**Non accetta un numero senza sapere come e' stato ottenuto.** Piu' volte ha smontato una
conclusione con una domanda breve, e aveva ragione entrambe le volte:

- *"non si potrebbero avere dei dati sufficienti dallo storico?"* — quando avevo dichiarato
  impossibile una misura che era invece fattibile.
- *"secondo me o non stiamo leggendo correttamente il modello o lo stiamo applicando male"* —
  quando avevo implementato una mia sintesi al posto della fonte.

**Conseguenza operativa:** dichiarare le convenzioni **accanto** al risultato, distinguere cio'
che e' misurato da cio' che e' assunto, e portare i risultati negativi senza addolcirli. Una
risposta che nasconde il metodo con cui e' stata prodotta viene smontata, e giustamente.

## Le Regole Nate Da Un Errore

Le regole stanno in `CLAUDE.md`. Qui c'e' **il caso che le ha prodotte**, perche' una regola di
cui si e' perso il motivo si erode.

### La gestione non si da' al buio

**L'utente non entra in tutti i trade che la lettura propone, e non lo dice a meno che non glielo
si chieda.** Il 18 settembre 2026 e' successo **due volte nella stessa seduta**: ho dato gestione
(*"sei a +3,5 punti"*, *"porta lo stop a pari"*) su un long e poi su uno short, e entrambe le volte
la risposta e' stata *"non sono nel trade"*.

Il danno non e' l'inutilita': e' che **una gestione data al buio riscrive la seduta nel diario**,
che e' l'unica cosa verificabile che il progetto produce. Quel giorno nel file della giornata
c'era l'esito di uno short mai esistito, e ho dovuto correggerlo.

Quindi: la lettura dal vivo dice DIREZIONE / INGRESSO / STOP / BREAK EVEN / BERSAGLIO / INVALIDA e
**si ferma li'**. La gestione si da' **solo dopo** che l'utente ha detto di essere dentro, oppure
si chiede *"sei entrato?"* in una riga. Nel diario l'operazione si scrive come **lettura data**,
non come trade fatto, finche' non e' confermata.

### Un movimento non e' il massimo di una finestra

Il 16 settembre 2026 avevo misurato *"il massimo dei 30 minuti successivi meno l'apertura"* e
riportato **+97,50 punti**. La risposta e' stata **«alle 14:40 non vedo tutti quei punti in
salita»**, ed era giusta: il percorso vero era +37, −24, sette minuti di laterale, +47, −29, +34.

Quella metrica misura l'**inviluppo dell'opportunita'** — chi indovina ogni giro e non tiene mai
attraverso un ritracciamento. Il danno non era il numero ma la conclusione: ci avevo costruito
sopra un *"Triple AAA da manuale"*, smentito dai dodici minuti di delta negativo che lo seguivano.

Procedura corretta (vincolo di ritracciamento, parametro R per strumento) in
[`metodo/le-gambe-di-una-seduta.md`](metodo/le-gambe-di-una-seduta.md), insieme alla regola che ne
discende: **un pattern trovato leggendo all'indietro dai movimenti buoni ha per costruzione il
100% di successo**, e il numero che conta e' quante volte la stessa firma compare *senza* il
movimento. Quel giorno: cinque gambe su cinque con la stessa firma, ma **19 occorrenze totali e
42% di riuscita**.

## Il Setup Delle Macchine

Il repo, ATAS, il bridge e la sessione con l'agente girano su **un PC Windows** dal 20 settembre
2026. Prima stava tutto su un Mac; il trasloco e' stato deciso perche' l'esecuzione degli ordini
passa da **Tradeify**, che usa Rithmic ma richiede **R|Trader Pro**, che non ha una build macOS.

Il dettaglio della connessione, cosa non viaggia con `git` e la sequenza su una macchina nuova
stanno in [`portare-il-repo-su-un-altra-macchina.md`](portare-il-repo-su-un-altra-macchina.md).
