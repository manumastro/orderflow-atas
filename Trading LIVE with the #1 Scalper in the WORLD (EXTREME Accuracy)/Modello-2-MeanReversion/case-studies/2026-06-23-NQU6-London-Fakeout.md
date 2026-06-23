# Caso Studio — 23/06/2026 NQU6 London Fakeout

## 1. Contesto

Evento osservato durante la London in formazione, quindi prima della chiusura ufficiale usata dal Modello 1.

Il mercato apre la London con un tentativo iniziale verso l'alto, poi perde progressivamente la parte bassa della value area provvisoria e costruisce un selloff. Successivamente, dopo un nuovo minimo, compare una rejection sul low e il prezzo torna verso il `POC preview`.

Questo caso è utile perché contiene **due setup Modello 2 opposti nella stessa sessione**:

```text
fakeout alto → early short
selloff → fakeout basso → early long
```

---

## 2. Timeline Operativa

### 09:05–09:10 Italy — Apertura London e sweep alto iniziale

```text
09:05 Italy
Close=30043.75
POC preview=29994.75
VAH preview=30021.25
VAL preview=29966.75
Relation=ABOVE_PREVIEW_VAH

09:10 Italy
High=30054.25
Close=30046.00
POC preview=30041.50
VAH preview=30054.00
VAL preview=30010.50
Delta=+46
```

Lettura:

- il prezzo lavora subito sopra la parte alta del valore provvisorio;
- il nuovo massimo a `30054.25` genera `[HIGH_REJECTION_CANDIDATE]`;
- non è ancora uno short completo, ma segnala possibile sweep iniziale sopra valore.

---

### 09:30 Italy — Early short

```text
09:30 Italy
[MR_EARLY_TRIGGER] Direction=Short
CandidateBar=5039
CandidateHigh=30054.25
CandidateClose=30046.00
Close=29993.00
POC preview=30000.00
VAH preview=30041.00
VAL preview=29980.75
DistToPOC=-7.00
Target1=30000.00
Target2=29980.75
```

Lettura:

- il prezzo perde il `POC preview` dopo il candidato fakeout alto;
- l'early short non è un errore: il mercato prosegue effettivamente al ribasso;
- questa parte è più simile a **fakeout alto → ritorno verso valore basso**.

Nota importante:

```text
Il trigger short nasce presto, ma il target2 è vicino.
Per renderlo operativo serve distinguere scalp verso VAL da possibile selloff esteso.
```

---

### 09:40–10:20 Italy — Accettazione sotto valore e selloff

```text
09:40 Italy
Close=29949.75
Relation=BELOW_PREVIEW_VAL
POC preview=30000.00
VAL preview=29950.75
Delta=-94

09:50 Italy
Low=29917.00
Close=29935.25
Relation=BELOW_PREVIEW_VAL
Delta=-132

09:55 Italy
Low=29889.00
Close=29909.00
Relation=BELOW_PREVIEW_VAL
Delta=-179

10:00 Italy
Low=29812.25
Close=29848.25
POC preview=29850.50
VAL preview=29812.25
Delta=-379

10:20 Italy
Low=29780.00
Close=29797.00
POC preview=29850.50
VAL preview=29780.00
Delta=-267
```

Lettura:

- il mercato accetta sotto la value area provvisoria iniziale;
- il `POC preview` si sposta in basso da area `30000` verso `29850.50`;
- la pressione resta bearish fino a `10:20`;
- in questa fase non c'è ancora long: il selloff è ancora dominante.

---

### 10:25 Italy — Low rejection candidate

```text
10:25 Italy
[LOW_REJECTION_CANDIDATE]
Low=29776.00
Close=29799.25
High=29814.75
POC preview=29850.50
VAH preview=29974.25
VAL preview=29776.00
Delta=+28
Top level: 29800.00 V52 / D+36
```

Lettura:

- il prezzo fa nuovo minimo a `29776.00`;
- chiude sopra il minimo e torna dentro la value area provvisoria;
- il delta torna positivo dopo una sequenza di delta negativi;
- questo è il primo punto in cui si può parlare di **candidate fakeout basso**.

Non è ancora trigger conservativo, perché il prezzo è ancora sotto `POC preview` di circa `51.25` punti.

---

### 10:50 Italy — Early long

```text
10:50 Italy
[MR_EARLY_TRIGGER] Direction=Long
Trigger=LOW_REJECTION_FOLLOW_THROUGH
CandidateBar=5054
CandidateLow=29776.00
CandidateClose=29799.25
CandidateHigh=29814.75
CandidateDelta=+28
Close=29831.75
High=29844.75
POC preview=29850.50
VAH preview=29956.25
VAL preview=29776.00
DistToPOC=-18.75
StopReference=29776.00
Target1=29850.50
Target2=29956.25
Delta=+29
```

Lettura:

- questa è la prima conferma operativa anticipata del long;
- arriva prima del reclaim POC;
- il prezzo ha fatto follow-through sopra la candela di rejection;
- il target 1 naturale è il `POC preview`;
- il target 2 è la parte alta della value area provvisoria.

Questo è il trigger più interessante per il Modello 2.

---

### 11:15 Italy — Trigger conservativo su reclaim POC

```text
11:15 Italy
[MR_TRIGGER] Direction=Long
Trigger=POC_RECLAIM_AFTER_LOW_REJECTION
Close=29910.50
POC preview=29850.50
VAH preview=29934.00
VAL preview=29776.00
DistToPOC=+60.00
Target1=29934.00
Delta=-61
```

Lettura:

- il reclaim del `POC preview` conferma che il fakeout basso ha funzionato;
- però come entry primaria arriva tardi;
- al momento del trigger il prezzo è già vicino al target `VAH preview`;
- meglio usarlo come conferma/management, non come unico trigger.

---

### 11:40–12:05 Italy — Rotazione dopo il trigger

```text
11:40 Italy
Close=29863.25
POC preview=29850.50
VAH preview=29935.00
VAL preview=29796.75
DistToPOC=+12.75
Delta=-44

12:05 Italy
Close=29881.00
POC preview=29905.00
VAH preview=29924.50
VAL preview=29798.25
DistToPOC=-24.00
Delta=+6
```

Lettura:

- dopo il reclaim, il prezzo non esplode immediatamente verso l'alto;
- il `POC preview` si sposta da `29850.50` a `29905.00`;
- il prezzo torna sotto il nuovo POC preview;
- questo mostra che i livelli preview sono dinamici e vanno trattati diversamente dai livelli congelati.

---

### 15:30–15:35 Italy — Footprint entry dopo sweep profondo

Dopo un nuovo selloff intraday, il prezzo fa uno sweep molto più profondo e rientra velocemente dentro la value area preview.

```text
15:30 Italy
[LOW_REJECTION_CANDIDATE]
Low=29616.50
Close=29768.25
CandidateDelta=+167
VAL preview=29692.75
POC preview=29712.00
VAH preview=29875.00

15:31:21.830 Italy circa
SweepTime: minimo candidate già formato

15:31:40.835 Italy
[MR_AGGRESSION_CONFIRM]
EntryModel=FootprintCumulativeTradeHistorical
EntryPrice=29697.50
EntryAreaLow=29694.75
EntryAreaHigh=29697.50
Volume=25
MinVolume=20
VolumeRule=Hardcoded20
TradeDirection=Buy
SecondsAfterSweep=19.0
StopReference=29616.50
RiskPoints=81.00
Target1POC=29712.00
RewardToPOC=14.50
Target2=29875.00
RewardToTarget2=177.50

15:35 Italy
[MR_EARLY_TRIGGER] Direction=Long
[MR_TRIGGER] Direction=Long
Close=29780.75
```

Lettura:

- la candela delle `15:30` è la rejection candidate;
- la conferma footprint arriva **dentro la candela candidate**, circa 19 secondi dopo lo sweep;
- l'entry più fedele al metodo Fabio non è la close `29780.75`, ma la buy aggression footprint in area `29694.75–29697.50`; il trade `Volume=25` passa la soglia provvisoria hardcoded `20`; in live lo stesso evento deve nascere da `OnCumulativeTrade`, dopo reload da `CumulativeTrades` storici;
- il trigger di barra delle `15:35` conferma il setup, ma arriva molto più tardi e con rapporto rischio/rendimento peggiore.

Regola derivata:

```text
Entry Fabio-style = prima buy/sell aggression significativa dopo sweep e rientro in value.
Trigger di barra = conferma/validazione, non necessariamente prezzo di ingresso primario.
```

---

## 3. Conclusione del Caso

Il 23/06 mostra una sequenza completa e utile per il Modello 2:

```text
1. Sweep alto iniziale
2. Early short valido
3. Selloff e accettazione sotto value
4. Sweep/fakeout basso
5. Footprint entry su buy aggression dopo sweep
6. Early long di barra
7. Reclaim POC tardivo
8. Rotazione attorno al nuovo POC preview
```

Lezione principale:

```text
La entry più fedele a Fabio nasce dalla prima aggressione footprint dopo sweep.
Il trigger anticipato di barra è utile come conferma.
Il trigger conservativo sul POC è robusto ma spesso tardivo.
```

---

## 4. Regole Provvisorie Derivate

### Trigger anticipato

```text
LOW_REJECTION_CANDIDATE
→ sweep identificato
→ prima buy aggression significativa dopo sweep e rientro in value
→ target iniziale POC preview
```

Speculare per short:

```text
HIGH_REJECTION_CANDIDATE
→ sweep identificato
→ prima sell aggression significativa dopo sweep e rientro in value
→ target iniziale POC preview
```

### Trigger di barra e conferma conservativa

```text
Low rejection → candela successiva con follow-through = early confirmation long
High rejection → candela successiva con follow-through = early confirmation short
Low rejection → reclaim POC preview = conferma conservativa long
High rejection → loss POC preview = conferma conservativa short
```

### Gestione livelli

- `POC preview` è target/confirm dinamico, non livello fisso.
- `VAH/VAL preview` sono target secondari o invalidazioni contestuali.
- Il livello congelato resta solo quello della London chiusa per il Modello 1.

---

## 5. Implicazioni per lo Sviluppo

Il futuro `MeanReversionPreviewTracker` dovrà distinguere:

1. **Setup direction**: fakeout alto o fakeout basso.
2. **Trigger type**: early o conservative.
3. **Market phase**: sweep iniziale, selloff, reversal, rotazione.
4. **Target logic**: POC preview come primo target, VAH/VAL come secondo target.
5. **Invalidation**: ritorno oltre l'estremo del fakeout o accettazione fuori value.

Il modulo non deve ancora generare segnali operativi, ma deve classificare i casi storici in modo consistente.
