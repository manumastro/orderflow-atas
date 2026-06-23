# Fabio Mean Reversion — Modello 2

**Stato:** Documento iniziale / fase di studio.  
**Implementazione:** Non attiva. Il Modello 1 resta il focus operativo.

---

## 1. Scopo

Il Modello 2 studia i casi in cui il mercato tenta di uscire da una balance, fallisce l'accettazione fuori valore e rientra violentemente verso il `POC`.

Principio operativo:

```text
Se il breakout sopra/sotto la balance viene rifiutato,
allora i trader rimasti intrappolati possono alimentare il ritorno verso il POC.
```

Questo è un modello **mean reversion / fakeout**, diverso dal Modello 1 che cerca continuation solo dopo `OUT_OF_BALANCE` confermato.

---

## 2. Relazione con Modello 1

I due modelli condividono la stessa base:

- balance zone;
- volume profile;
- `POC`;
- `VAH/VAL`;
- High/Low della zona;
- acceptance vs rejection.

Differenza principale:

| Area | Modello 1 Trend Following | Modello 2 Mean Reversion |
|------|---------------------------|---------------------------|
| Trigger | Breakout accettato fuori `VAH/VAL` | Breakout/fakeout rifiutato |
| Stato cercato | `OUT_OF_BALANCE` | Rejection e ritorno in balance |
| Direzione | Continuation | Rientro verso `POC` |
| Timing | Dopo London chiusa | Anche durante balance in formazione |
| Target | POC balance precedente / continuation plan | POC della balance attuale |

Per ora il Modello 2 non deve contaminare la state machine del Modello 1.

---

## 3. Problema Originario

Il vecchio ostacolo era il tracker della balance dinamica.

Nel Modello 1 il problema è semplice:

```text
London chiusa → livelli congelati → breakout NY
```

Nel Modello 2 il problema è più complesso:

```text
Balance in formazione → profile provvisorio → sweep/fakeout → rejection → rientro verso POC
```

Per questo non basta aspettare la fine della sessione: serve una **preview intraday** di `POC/VAH/VAL`.

---

## 4. Caso Studio — 22/06/2026 NQU6

Evento osservato sul grafico:

- intorno alle `16:05–16:10` italiane il prezzo ha fatto sweep in alto;
- subito dopo è rientrato e ha iniziato una discesa forte;
- alle `17:05–17:10` il Modello 1 ha confermato breakout bearish sotto `VAL` finale.

Log rilevanti:

```text
16:05 Italy
High=30966.50
Close=30949.75
Delta=+407
Close sopra VAH preview=30942.50

16:10 Italy
High finale=30968.00
Close=30909.75
Delta=-466
Close rientra dentro Value Area preview

16:30 Italy
Close=30692.00
Delta=-704
Close sotto VAL preview=30764.50

16:50 Italy
Close=30580.50
Delta=-1008
Close sotto VAL preview=30686.75

17:10 Italy
Breakout bearish Modello 1 confermato sotto VAL finale
```

Interpretazione:

```text
Questo è un caso compatibile con Metodo 2:
sweep sopra valore → rejection → rientro in balance → accelerazione verso/sotto POC.
```

Il segnale discrezionale Metodo 2 sarebbe nato prima della conferma Modello 1.

---

## 5. Caso Studio — 23/06/2026 NQU6

### 5.1 Contesto

Evento osservato durante la London in formazione, quindi prima della chiusura ufficiale usata dal Modello 1.

Il mercato apre la London con un tentativo iniziale verso l'alto, poi perde progressivamente la parte bassa della value area provvisoria e costruisce un selloff. Successivamente, dopo un nuovo minimo, compare una rejection sul low e il prezzo torna verso il `POC preview`.

Questo caso è utile perché contiene **due setup Modello 2 opposti nella stessa sessione**:

```text
fakeout alto → early short
selloff → fakeout basso → early long
```

---

### 5.2 Timeline Operativa

#### 09:05–09:10 Italy — Apertura London e sweep alto iniziale

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

#### 09:30 Italy — Early short

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

#### 09:40–10:20 Italy — Accettazione sotto valore e selloff

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

#### 10:25 Italy — Low rejection candidate

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

#### 10:50 Italy — Early long

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

#### 11:15 Italy — Trigger conservativo su reclaim POC

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

#### 11:40–12:05 Italy — Rotazione dopo il trigger

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

### 5.3 Conclusione del Caso

Il 23/06 mostra una sequenza completa e utile per il Modello 2:

```text
1. Sweep alto iniziale
2. Early short valido
3. Selloff e accettazione sotto value
4. Sweep/fakeout basso
5. Early long
6. Reclaim POC tardivo
7. Rotazione attorno al nuovo POC preview
```

Lezione principale:

```text
Il trigger conservativo sul POC è robusto ma spesso tardivo.
Il trigger anticipato dopo rejection è più utile per entry,
ma richiede contesto per evitare falsi segnali.
```

---

### 5.4 Regole Provvisorie Derivate

#### Trigger anticipato

```text
LOW_REJECTION_CANDIDATE
→ candela successiva chiude sopra close/high della rejection
→ delta o struttura conferma follow-through
→ target iniziale POC preview
```

Speculare per short:

```text
HIGH_REJECTION_CANDIDATE
→ candela successiva chiude sotto close/low della rejection
→ delta o struttura conferma follow-through
→ target iniziale POC preview
```

#### Trigger conservativo

```text
Low rejection → reclaim POC preview = conferma long
High rejection → loss POC preview = conferma short
```

#### Gestione livelli

- `POC preview` è target/confirm dinamico, non livello fisso.
- `VAH/VAL preview` sono target secondari o invalidazioni contestuali.
- Il livello congelato resta solo quello della London chiusa per il Modello 1.

---

### 5.5 Implicazioni per lo Sviluppo

Il futuro `MeanReversionPreviewTracker` dovrà distinguere:

1. **Setup direction**: fakeout alto o fakeout basso.
2. **Trigger type**: early o conservative.
3. **Market phase**: sweep iniziale, selloff, reversal, rotazione.
4. **Target logic**: POC preview come primo target, VAH/VAL come secondo target.
5. **Invalidation**: ritorno oltre l'estremo del fakeout o accettazione fuori value.

Il modulo non deve ancora generare segnali operativi, ma deve classificare i casi storici in modo consistente.

---

## 6. Profile Preview

Per studiare il Modello 2 serve calcolare livelli provvisori durante la sessione.

Regola tecnica:

```text
Il volume profile si aggiorna ogni barra.
POC/VAH/VAL preview si calcolano ogni N barre o su eventi importanti.
POC/VAH/VAL ufficiali restano congelati solo a fine London per il Modello 1.
```

Parametri iniziali usati per debug:

- aggiornamento profile: ogni barra;
- preview `POC/VAH/VAL`: ogni 5 barre durante tutta London;
- preview forzata su nuovi massimi/minimi e rejection candidate;
- log trigger anticipato `[MR_EARLY_TRIGGER]` e conferma POC `[MR_TRIGGER]`;
- nessun disegno e nessun impatto sulla state machine del Modello 1.

Log utili:

```text
[PROFILE_PREVIEW]
[HIGH_REJECTION_CANDIDATE]
[LOW_REJECTION_CANDIDATE]
[LONDON_PRE_CLOSE]
[SESSION_EXTREMES]
```

---

## 7. Ipotesi di Detection Iniziale

### 7.1 Fakeout Alto

Condizioni candidate:

1. Prezzo fa nuovo massimo della balance in formazione.
2. Close era sopra `VAH preview` o comunque molto estesa rispetto al `POC preview`.
3. Candela successiva o stessa candela rientra sotto `VAH preview`.
4. Delta passa da positivo/assorbito a negativo.
5. Il prezzo torna verso `POC preview`.
6. Conferma forte se rompe sotto `POC preview` o `VAL preview`.

Pattern sintetico:

```text
Sweep above preview VAH
→ rejection / close back inside VA
→ return toward preview POC
→ optional break below preview VAL
```

### 7.2 Fakeout Basso

Speculare:

```text
Sweep below preview VAL
→ rejection / close back inside VA
→ return toward preview POC
→ optional break above preview VAH
```

---

## 8. Dati Necessari

Per validare il Modello 2 servono:

- `POC/VAH/VAL preview` al momento dello sweep;
- distanza del close da `POC`, `VAH`, `VAL` preview;
- delta candela (`Ask - Bid`);
- top price levels della candela;
- volume della candela rispetto al contesto;
- timestamp in ora italiana;
- massimo/minimo della sessione con bar/time.

Questi dati sono già presenti nei log di debug aggiunti al `BalanceZoneTracker`.

---

## 9. Non Obiettivi Attuali

In questa fase il Modello 2 non deve:

- generare segnali operativi automatici;
- disegnare zone proprie sul grafico;
- modificare il comportamento del Modello 1;
- usare la preview come livelli ufficiali;
- assumere che ogni rejection sia tradabile.

---

## 10. Prossimi Passi

1. Raccogliere altri esempi reali con `[PROFILE_PREVIEW]`.
2. Classificare manualmente: fakeout valido, fakeout debole, trend continuation.
3. Definire soglie minime per:
   - distanza da `POC preview`;
   - chiusura sopra/sotto `VAH/VAL preview`;
   - delta reversal;
   - volume relativo;
   - ritorno verso POC.
4. Solo dopo, valutare un modulo separato:

```text
MeanReversionPreviewTracker
```

Il modulo dovrà restare separato dalla pipeline del Modello 1.
