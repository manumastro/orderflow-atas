---
name: la-storia-del-progetto
description: Le fasi di orderflow-atas da luglio a settembre 2026, e perche' ciascuna e' finita
metadata:
  type: project
---

Serve a capire **perche' il repository ha la forma che ha**: molte cartelle sono resti di fasi
chiuse, e senza la storia si rischia di riaprirle. Il dettaglio riga per riga sta in
`FabioOrderFlow/progress.txt`; le strade chiuse e il perche' in
`docs/research/percorso-del-progetto.md`.

**Luglio 2026 — la baseline.** Il repository viene resettato a una baseline neutra del corso. Si
mappa il corso di Fabio e si distingue Big Trades Aggregate da Deep Trades.

**Agosto 2026 — la fase osservativa.** Si costruiscono i recorder ATAS e un contratto osservativo,
si raccolgono decine di migliaia di eventi aggregati. Quei recorder sono oggi archiviati in
`docs/research/archivio-2026-08/` e **non vanno estesi**.

**Inizio settembre — la misura, e il suo esito.** Si testa il modello sistematico (40R e i suoi
trigger) e si legge il COT con serie decennale. Il risultato e' **negativo**: i trigger stanno al
49-51%, la scala della barra migliora il win rate ma migliora anche quello del modello nullo, e
la lezione di metodo che costa piu' di una volta e' che **le osservazioni dentro una sessione non
sono indipendenti** — un z ingenuo di +4,1 diventa t=+1,4 clusterizzando per giornata.

**15 settembre — la svolta.** Lo scope viene ridefinito sulla **lettura discrezionale** del primo
live del corso. La fase sistematica sul modello 40R e' **chiusa e archiviata**, e non si riapre
senza richiesta esplicita. Si trascrive il dossier Triple AAA dalle sette immagini.

**16-18 settembre — si opera dal vivo, e si sbaglia.** Tre giorni di sedute vere su NQ, poi su ES,
crude e oro. Da qui nascono quasi tutte le regole di `CLAUDE.md`: i tre sorveglianti, le sette
prove prima di armare una condizione, il giro d'orizzonte a ogni messaggio, la gestione che non si
da' al buio, un movimento che non e' il massimo di una finestra. Ogni regola ha un errore dietro,
e i casi sono in `docs/research/come-lavora-l-utente.md`.

**18 settembre — le due decisioni.** Arrivano le trascrizioni del live Q1 e cambiano la gerarchia:
**il corso comanda**, il dossier diventa riferimento, **lo strumento e' NQ e solo NQ**. Cadono
insieme la deroga su Londra e quella sul gate orario, che nel live non esiste.

**19-20 settembre — gli strumenti e il trasloco.** I livelli sul chart diventano regole che si
ricalcolano da sole, nasce il pannello delle condizioni, e il lavoro comincia a spostarsi su un PC
Windows perche' l'esecuzione lo impone. Nello stesso passaggio si scopre che **la memoria
dell'agente non viaggia con git e due voci erano gia' divergite dal repo**. Vedi
[[il-contesto-sta-nel-repo]] e [[dove-siamo]].
