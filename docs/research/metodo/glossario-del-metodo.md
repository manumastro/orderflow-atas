# Glossario Del Metodo — Dove Sta Scritta Ogni Definizione

Questo file **non definisce niente**. Dice, per ogni termine, **dove** sta la definizione, cosi' che
consultarla costi una riga di comando invece di una decisione.

Serve a rendere eseguibile l'obbligo di `CLAUDE.md`: **un termine del metodo non si parafrasa a
memoria.** Prima di usarlo in una risposta, in una etichetta sul chart o in un documento, si apre la
fonte e si legge il passo.

```bash
grep -n -A4 '^22:12$' fabio_course/fabio_q1/fabio_3.txt          # perche' non si opera Londra
sed -n '139,196p' docs/research/metodo/triple-aaa-dossier.md     # Mean Reverting, nel dossier
```

**Convenzione di citazione:** `[3 · 22:12]` = `fabio_course/fabio_q1/fabio_3.txt`, minuto 22:12.

---

## 1. I Termini Del Live Q1 — La Fonte Che Comanda

| termine | minuto | cosa NON dare per scontato |
|---|---|---|
| **i tre modelli** | `[1 · 18:05]`, `[1 · 49:12]`, `[4 · 7:16]` | non sono stili: **li sceglie la posizione del prezzo** rispetto al valore della cash precedente |
| **balance / mean reverting** | `[1 · 18:05]` | vale **solo** col prezzo chiuso dentro la cash precedente, e il bersaglio e' il **POC**, non il bordo opposto |
| **momentum** | `[1 · 49:12]` | la balance rotta puo' essere la sessione **o un profilo fisso da swing a swing**. Senza speed of tape non si entra |
| **trend following** | `[4 · 7:16]` | si **fade il ritracciamento nel verso del giorno**, e si entra sul **reload**, non sulla rottura |
| **Triple AAA (nel live)** | `[3 · 45:50-48:37]` | quattro pezzi: direzione, livello di delta, assorbimento, muro passivo. **Niente gate orario** |
| **assorbimento** | `[4 · 12:35]`, `[3 · 47:33]` | sforzo **senza** risultato. Non e' volume alto: e' volume alto che non ha mosso il prezzo |
| **aggressione** | `[1 · 44:02]` | ordini aggressivi **con** risultato. Se il corpo non segue, era assorbimento |
| **risultato** | `[6 · 1:09:58]`, `[2 · 2:10:31]` | lo **stoppino** dice che hanno provato e sono stati assorbiti; il **corpo** dice che sono stati accettati. Il delta e' l'input, il risultato e' l'output |
| **reload** | `[4 · 28:27]`, `[4 · 1:26:19]` | il ritorno sul livello dove i **vincitori** avevano eseguito. E' li' che si entra in trend following |
| **squeeze** | `[3 · 13:50]`, `[2 · 42:52]` | gli ordini del lato perdente **devono** chiudere. E' una conseguenza meccanica, non una figura |
| **failed auction** | `[3 · 19:22]` | tentativo di rompere un estremo che fallisce. **Un assorbimento al primo tocco non e' un failed auction** — correzione esplicita di Fabio |
| **muro di liquidita'** | `[3 · 31:34]`, `[6 · 1:01:24]` | ordini **limite passivi** impilati. Si vede dall'assorbimento ripetuto sullo stesso orizzontale |
| **speed of tape** | `[3 · 1:08:41]`, `[1 · 55:05]` | **quanto in fretta** gli ordini entrano, non quanti. Serve soprattutto a **non** entrare. **Non c'e' nel bridge**: si usa un proxy e si dichiara |
| **big trades / deep trades** | `[5 · 6:04]` | il filtro di size: **60 su NQ in cash**, 20-30 in premarket. E' la taratura di Fabio, non nostra |
| **flip dell'asta** | `[4 · 45:57]` | big trades **+** speed **+** direzione nella **stessa** candela. Due su tre non basta |
| **gap / inefficienza** | `[2 · 1:50:02]`, `[3 · 1:08:01]` | **la candela dopo decide**: ribilancia e chiude nel verso del gap = si continua; se lo mangia al contrario = inversione |
| **controlled area** | `[2 · 1:38:02-1:41:26]` | quale fra domanda e offerta controlla, finche' non si rompe. Rotta la domanda, l'offerta sopra **diventa irrilevante** |
| **no man's land** | `[3 · 17:32]` | il centro del range. Non e' un'area con poco segnale: e' un'area dove **non si opera** |
| **point of no return** | `[1 · 1:05:18]` | il livello oltre il quale non si torna indietro |
| **station** | `[1 · 1:06:00]` | il livello successivo dove il prezzo si fermera' |
| **quarters / numeri tondi** | `[3 · 1:01:20]`, `[6 · 1:04:12]` | i 100 punti su NQ e i quarti (250): dove stanno le opzioni |
| **premium / fair value / discount** | `[5 · 55:05]` | rispetto al **picco di volume** della distribuzione. Vieta i long in alto e gli short in basso |
| **livello ad alto delta** | `[5 · 1:04:56]` | ha **due mestieri in sequenza**: al primo tocco e' dove il lato vincente prende profitto (*stopping volume*, ed e' un'**area**, non un livello); se cede, quei contratti devono chiudere e diventa uno **squeeze** |
| **liquidita' sottile** | `[5 · 16:07]` | un tratto percorso in fretta su poco volume **non e' valore trasferito**: verra' ribilanciato. Non e' un posto dove mettere uno stop |
| **regime** | `[6 · 1:19:04]`, `[6 · 54:03]` | direzionale = **una candela copre piu' di cinque candele** del range precedente. Decide la **size**, prima del setup |
| **break even** | `[3 · 15:39]`, `[1 · 2:11:58]` | e' un **livello**, non una distanza: il prezzo al quale l'analisi si smonta |
| **stacking del profitto** | `[2 · 1:18:29]` | i trade successivi rischiano **il profitto della giornata**, non il capitale |
| **aggressivo / conservativo** | `[6 · 8:42]` | il conservativo aspetta la chiusura del **corpo** oltre il livello **piu'** l'aggressione del lato che si segue |
| **profile framing** | `[1 · 20:44]`, sezione 10 | **solo cash**, composito a **90 giorni**, shift = VAL e VAH entrambi piu' bassi, e **due** POC |
| **COT non-commercial** | `[1 · 26:01-43:18]`, sezione 9 | vista **Legacy**, solo non-commercial, e conta **l'ultima variazione**. L'output e' **una data che diventa un prezzo** |
| **la sessione** | `[3 · 22:12]`, `[3 · 23:00]` | **New York.** Londra e' esclusa con il motivo dichiarato |
| **un asset solo** | `[3 · 1:06:37]` | NASDAQ. *"One asset, one."* |

Le sezioni numerate citate sopra sono di
[`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

---

## 2. I Termini Del Dossier — Riferimento, Non Comando

Puntano a [`triple-aaa-dossier.md`](triple-aaa-dossier.md), trascrizione delle immagini in
[`fabio_course/ivbaaa/`](../../../fabio_course/ivbaaa/). **Nessuno di questi termini autorizza una
operazione.** Si citano per parlare del modello, non per armarlo.

| termine | righe | cosa NON dare per scontato |
|---|---|---|
| **IVB** | 90-138 | e' la prima mezz'ora della cash di New York. **Nel live non funziona da gate** |
| **Tier 01 · bias filter** | 90-138 | la chiusura M30 come permesso direzionale. **Il live non la usa**: Fabio opera anche in premarket |
| **Tier 02·A · Mean Reverting** | 139-196 | il fade dei bordi **verso l'interno**. Coincide col primo modello del live |
| **risk envelope del mean reverting** | 182-185 | size ridotta e **massimo 2 stop-loss al giorno**, non stop vicino. Il live conferma il cap a 2-3 |
| **il trigger in tre tempi** | 190-194 | stoppino, **poi** assorbimento, **poi** flip di aggressione. **Due su tre non bastano** |
| **Tier 02·B · Triple AAA** | 197-255 | nel dossier richiede il permesso dell'IVB. **Nel live no**: i quattro pezzi sono quelli di `[3 · 48:37]` |
| **i tre trigger del Tier 02·B** | 206-228 | break + high-delta bounce, break-and-retest, deep effort 40R |
| **Tier 02·C · Triple A+** | 360-400 | il ritracciamento attraversa tutto l'IVB fino al VAL. Il dossier **non ne dichiara la frequenza** |
| **Tier 03 · trigger e conferma** | 256-307 | candle framing, deep trades e block-and-reload. Sceglie **quando**, non **se** |
| **Deep Effort / 40 Range** | 222-228, 294-307 | dichiarato **solo su NQ** e **solo su grafico a 40 range** |
| **checklist di esecuzione** | 323-359 | la sequenza operativa completa del dossier |
| **cosa e' osservabile col bridge** | 439-457 | quali pezzi il Data Bridge puo' misurare davvero |

---

## 3. Termini Che Non Vengono Da Fabio

Sono misure di **questo repository**. Vanno dichiarate come tali ogni volta che entrano in una
lettura, e **non vanno confuse** con i termini del metodo.

| termine | dove sta | cos'e' |
|---|---|---|
| **accettazione** | [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) | minuti chiusi da un lato piu' quota di volume, su una finestra |
| **derivata del delta di fascia** | file delle giornate, 18 settembre 2026 | come cambia il delta cumulato di una fascia **mentre** il prezzo la attraversa. Risponde alla domanda del live *"chi sta vincendo la battaglia?"*. **4 conferme su 4**, una sola giornata |
| **magnitudine del presidio** | [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) | il delta in percentuale del volume della fascia. Misura **l'intensita' dell'attraversamento, non la sua tenuta** |
| **fascia con delta contrario** | [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) | una fascia oltre il p90 del delta assoluto e di segno opposto. Fra ingresso e bersaglio **e' il bersaglio** |
| **doppio POC** | file della giornata | due sedute che costruiscono valore nella stessa fascia |
| **permesso di fatto** | [`il-permesso-si-misura-non-si-aspetta.md`](il-permesso-si-misura-non-si-aspetta.md) | l'accettazione su barre M1 invece della chiusura M30. Nato per battere il gate del dossier: **senza gate, la misura resta, il confronto no** |
| **sosia di un setup** | [`i-pattern-di-esecuzione.md`](i-pattern-di-esecuzione.md), sezione 7 | il movimento che produce gli stessi numeri e significa il contrario. **Non e' nel live**, ed e' il motivo per cui le sette prove restano |
| **il fotogramma** | file delle giornate | un valore di fascia letto a barra **non chiusa**. Non e' una misura. Quattro occorrenze il 18 settembre |
| **proxy della speed of tape** | — | volume per barra M1 contro la distribuzione recente. **Il bridge non espone la speed of tape**, e il proxy va dichiarato |

---

## L'Errore Che Ha Prodotto Questo File

Il 16 settembre ho scritto che un long sull'accettazione sopra il massimo notturno era "mean
reverting a rischio stretto". Erano due errori in una frase:

1. **Non era mean reverting.** Andava fuori dal valore, non verso l'interno.
2. **"Rischio stretto" non e' lo stop.** E' size ridotta e cap agli stop-loss giornalieri.

Nessuno dei due sarebbe successo aprendo la fonte. Non l'avevo aperta perche' credevo di sapere, ed
e' esattamente la condizione in cui serve il controllo.
