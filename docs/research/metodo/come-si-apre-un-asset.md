# Come Si Apre Un Asset

**Obbligatorio.** Nessuna lettura, nessun livello sul chart e nessuno scenario armato su uno
strumento che non ha completato i nove passi che seguono. Non e' una raccomandazione: un asset
aperto a meta' produce numeri che sembrano una analisi e non lo sono, e il 16 settembre 2026 —
prima giornata sul crude — tre passi su nove hanno prodotto un errore ciascuno prima ancora
dell'apertura del pit.

La procedura vale **per ogni strumento nuovo**, e i primi otto passi si rifanno **da capo** a ogni
rollover del contratto.

---

## Perche' Esiste

Il metodo di questo repository e' nato sul Nasdaq. Tutto quello che e' scritto altrove presuppone
NQ senza dirlo: l'orologio della sessione, il tick, le soglie in punti, la finestra dell'IVB, il
codice COT. Aprire un secondo strumento non e' «usare gli stessi strumenti su dati diversi» —
e' **scoprire quali costanti erano in realta' parametri**.

Le tre categorie di cose che non si trasferiscono, trovate sul crude e valide in generale:

| | esempio del 16 settembre |
|---|---|
| **Gli orologi** | il pit NYMEX apre alle 15:00 CEST, non alle 15:30: l'IVB e il gate cadono mezz'ora prima. `calcola_ivb` aveva la finestra degli indici scritta nel codice |
| **Le soglie assolute** | «40 range» del TRIGGER 03 e' in tick; 0,25 di NQ e 0,01 del crude non misurano la stessa cosa. Stessa ragione per presidio e avviso della sveglia |
| **La validazione** | il p95 di delta del crude e' **94** contro **48** di NQZ6 su p95 di volume quasi identici: a parita' di barra, il doppio dello sbilanciamento. Nessuna soglia empirica si eredita, nemmeno in proporzione |

---

## I Nove Passi

### 0. Il rollover, prima di ogni altra misura

Regola gia' in `CLAUDE.md`, qui con l'aggiunta che riguarda gli strumenti nuovi:

```bash
python3 FabioOrderFlow/tools/bridge.py instrument --chart <STRUMENTO>
```

Si legge `security.expiration`. Se `rollovers` risponde *"Only continuous contracts are
supported"* — succede su ogni contratto singolo — **il controllo automatico non e' disponibile**,
e allora si confronta a mano il volume del contratto con quello del successivo. **Il risultato
"non verificabile" e' un risultato e va scritto**, non saltato.

### 1. L'anagrafica del contratto

Dallo stesso comando: tick size, tickCost, moltiplicatori, fuso. Il **tick** e' il numero da cui
discende ogni soglia dei passi 6 e 7: si scrive nel file della giornata prima di usarlo.

### 2. Gli orologi

Due, e vanno trovati entrambi **prima** di armare qualunque scenario IVB:

- la **sessione elettronica**, da `bridge.py session`;
- la **sessione regolamentata** (pit / cash), che il bridge **non** dice e va stabilita dalla
  specifica del mercato.

L'IVB e' i primi 30 minuti della **regolamentata**. Sbagliarla significa misurare mezz'ora a caso
e chiamarla gate.

```text
indici (NQ, ES)      cash NY 15:30 CEST   ->  IVB 15:30-16:00, gate 16:30
crude (CL, MCL)      pit NYMEX 15:00 CEST ->  IVB 15:00-15:30, gate 15:30
```

### 3. Il contesto istituzionale

Procedura in [`analisi-istituzionale.md`](analisi-istituzionale.md), che vale **identica** per ogni
asset. Serve il codice Tradingster dello strumento, e la rilevazione dichiara sempre vista
(legacy / TFF), variante (futures only), data del report e **ritardo alla lettura**.

```text
209742   Nasdaq-100 Mini
067651   WTI-PHYSICAL (crude)
```

**Su un derivato micro il COT copre il sottostante, non il contratto**: va detto nella
rilevazione. Esempio: [`../cot/snapshot-MCLV6-2026-09-16.md`](../cot/snapshot-MCLV6-2026-09-16.md).

**Il buco fra la data del report e oggi va sempre quantificato in prezzo**, non solo in giorni:
l'8 settembre il crude chiudeva a 93,03 e il 16 stava a 103,79, cioe' tutto il movimento recente
e' successivo all'ultima fotografia disponibile.

### 4. Il framing delle sedute precedenti

Dal bridge, con lo stesso calcolo usato su NQ: POC, value area al 70%, range e chiusura della
seduta precedente, poi della notte. Senza un file di giornata precedente **si ricostruisce dai
dati**, e si dichiara che e' ricostruito.

### 5. I livelli

Derivati dall'analisi, mai dallo strumento, e spinti con `POST /levels`. Ognuno con **a cosa serve
arrivarci** — permesso, bersaglio, invalidazione o posizionamento — come vuole `CLAUDE.md`.

### 6. Le soglie, riscalate e dichiarate

**Nessun numero si copia da NQ.** Presidio e avviso della sveglia si scelguono da due misure dello
strumento: la **distanza tipica fra livelli chiave** e l'**escursione tipica di una barra M1**.

```text
NQZ6    livelli a 15-25 punti, barra ~5 punti    ->  --presidio 4     --avviso 15
MCLV6   livelli a 0,25-0,75 $, barra ~0,10 $     ->  --presidio 0.06  --avviso 0.25
```

I percentili (p95 volume, p95 delta) li calcola la sveglia sulla propria base: si **legge la riga
di avvio** e si controlla che siano plausibili. Se dice `BASE CORTA`, si alza `--storia`.

### 7. Gli scenari

Le **sei prove** di
[`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) valgono immutate, con due aggiunte che
esistono solo per un asset nuovo:

- **Prova 3 con il nome dello strumento accanto.** Se la riga della fonte che giustifica il
  `verso` porta un marcatore di strumento, quel trigger **non e' armabile altrove**. Il
  TRIGGER 03 — *Deep Effort, 40 Range* — e' marcato **(NQ)** alla riga 222 del dossier: e' l'unico
  del dossier che lo sia, ed e' escluso dal crude.
- **Il trasferimento si dichiara.** Quando la fonte non qualifica un setup per strumento, il
  `verso` e' tracciabile e quindi lecito — ma in questo repository non e' stato misurato li'.
  Si scrive **`TRASFERIMENTO NON VERIFICATO`** nel campo `attesa`. Il primo giorno si osserva.

Poi `--controlla` e `--prova`, come sempre.

### 8. I file, col prefisso dello strumento

`--giorno` e' una **chiave**, non una data: `MCLV6-2026-09-16` produce
`scenari-MCLV6-2026-09-16.json` e `annotazioni-MCLV6-2026-09-16.json` senza che serva toccare il
codice. Lo stesso prefisso per livelli e diario.

```text
MCLV6-2026-09-16.md              la cronaca
livelli-MCLV6-2026-09-16.json    i livelli
scenari-MCLV6-2026-09-16.json    gli scenari
```

**Debito aperto**: i file di NQZ6 sono senza prefisso, per ragioni storiche. O tutti gli strumenti
lo portano, o nessuno; finche' l'asimmetria esiste va ricordata qui.

---

## La Verifica Finale

Prima di dire una sola parola sul mercato nuovo, le nove risposte devono esistere per iscritto nel
file della giornata. **Se una manca, manca l'analisi** — non e' una analisi con una lacuna, e'
un insieme di numeri che somigliano a una analisi.

| | la domanda | dove sta la risposta |
|---|---|---|
| 0 | quando scade, e il volume e' ancora qui? | sezione 1 della giornata |
| 1 | quanto vale un tick? | sezione 1 |
| 2 | a che ora apre la regolamentata? | sezione 1 |
| 3 | chi e' posizionato, con che ritardo? | sezione 2, piu' la rilevazione in `../cot/` |
| 4 | dov'era il valore ieri e stanotte? | sezione 2 |
| 5 | quali livelli, e a cosa serve arrivarci? | sezione 3 |
| 6 | con quali soglie, e perche' quelle? | sezione 6 |
| 7 | quali scenari, e cosa non e' armabile qui? | sezione 4 |
| 8 | i file hanno il prefisso? | i nomi stessi |

Primo caso applicato, con gli errori che ha prodotto:
[`../giornate/MCLV6-2026-09-16.md`](../giornate/MCLV6-2026-09-16.md).
