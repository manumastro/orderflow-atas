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

Il caso completo è documentato qui:

`case-studies/2026-06-23-NQU6-London-Fakeout.md`

Sintesi osservativa:

- la London apre con uno sweep/fakeout alto iniziale;
- il primo `[MR_EARLY_TRIGGER]` short arriva presto e non è un errore: il mercato prosegue effettivamente al ribasso;
- il selloff crea poi un nuovo minimo significativo a `29776.00`;
- da quel low nasce un `[LOW_REJECTION_CANDIDATE]` con delta tornato positivo;
- il `[MR_AGGRESSION_CONFIRM]` storico da footprint/cumulative trades individua l'entry più fedele a Fabio;
- il `[MR_EARLY_TRIGGER]` long arriva prima del reclaim del `POC preview`, quindi è più utile della conferma conservativa di barra;
- il `[MR_TRIGGER]` su reclaim POC è più robusto, ma nel caso osservato arriva tardi e va considerato più come conferma/management.

Importanza per il Modello 2:

```text
La stessa London può contenere due setup opposti:
fakeout alto → early short
selloff → fakeout basso → early long
```

Questo conferma che il futuro `MeanReversionPreviewTracker` non dovrà limitarsi a trigger isolati, ma dovrà classificare contesto, fase di mercato, direzione del fakeout e tipo di conferma.

---

## 6. Profile Preview

Per studiare il Modello 2 serve calcolare livelli provvisori durante la sessione.

Regola tecnica:

```text
Il volume profile si aggiorna ogni barra.
POC/VAH/VAL preview si calcolano live/intrabar durante London.
POC/VAH/VAL ufficiali restano congelati solo a fine London per il Modello 1.
```

Parametri diagnostici correnti:

- aggiornamento profile: ogni barra;
- preview `POC/VAH/VAL`: live/intrabar durante tutta London;
- log trigger anticipato `[MR_EARLY_TRIGGER]` e conferma POC `[MR_TRIGGER]`;
- conferma footprint live tramite `OnCumulativeTrade` / `OnUpdateCumulativeTrade`;
- fallback storico/reload tramite `CumulativeTrades` sugli ultimi 7 giorni per `[MR_AGGRESSION_CONFIRM]`;
- nessun disegno e nessun impatto sulla state machine del Modello 1.

File di log:

```text
FabioTrendFollowing_YYYY-MM-DD.log
```

Contiene tutto in un unico file, ma i log tecnici pesanti sono disattivati di default. Restano attivi trigger, profile preview, rejection candidate e cumulative trades utili allo studio.

Regola di analisi:

```text
Per entry footprint: cercare [MR_AGGRESSION_CONFIRM].
Per conferma di barra: cercare [MR_EARLY_TRIGGER] e [MR_TRIGGER].
Per contesto: seguire NEW_SESSION_LOW/HIGH -> LOW/HIGH_REJECTION_CANDIDATE -> PROFILE_PREVIEW.
```

I log `[MR_EARLY_TRIGGER]` e `[MR_TRIGGER]` includono `BarMode`:

```text
BarMode=HISTORICAL_CLOSED
BarMode=LIVE_OR_LAST_BAR
```

`[MR_AGGRESSION_CONFIRM]` aggiunge invece la lettura footprint live o storica:

```text
EntryModel=FootprintCumulativeTradeLive
EntryModel=FootprintCumulativeTradeHistorical
EntryPrice=...
EntryAreaLow=...
EntryAreaHigh=...
SweepTimeUtc=...
SecondsAfterSweep=...
StopReference=...
RiskPoints=...
Target1POC=...
RewardToPOC=...
```

Questa distinzione è fondamentale: la barra definisce contesto/candidate, mentre `[MR_AGGRESSION_CONFIRM]` prova a stimare l'entry più fedele al metodo Fabio sui big trades dopo lo sweep.

---

## 7. Ipotesi di Detection Iniziale

### 7.1 Fakeout Alto

Condizioni candidate:

1. Prezzo fa nuovo massimo della balance in formazione.
2. Close era sopra `VAH preview` o comunque molto estesa rispetto al `POC preview`.
3. Candela successiva o stessa candela rientra sotto `VAH preview`.
4. Footprint mostra sell aggression significativa dopo lo sweep.
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
→ buy aggression significativa dopo sweep
→ return toward preview POC
→ optional break above preview VAH
```

### 7.3 Barra vs Footprint

```text
Barra = struttura del fakeout: nuovo high/low, rejection, close position, relazione con VAH/VAL preview.
Footprint = timing di ingresso: big trades/aggression dopo lo sweep.
```

Lo studio della barra non va eliminato per il fakeout: serve a definire che il mercato ha effettivamente fatto sweep e rifiuto. Il footprint sostituisce invece la barra come prezzo di entry primaria, perché Fabio parla di entrare quando si vede la mano dei big market participants.

### 7.4 Soglia volume aggressione

Riferimento verificato nel transcript:

```text
30 contracts on NASDAQ on the one minute
```

Fabio parla anche di `big trades`, `bubble`, `big volume` e `larger orders`, ma nel transcript disponibile non c'è una regola completa e verificata per sessione.

Valore corrente nel codice:

```text
MinAggressionTradeVolume = 20
VolumeRule = Hardcoded20
```

Regola provvisoria:

- usare soglia unica `20` per evitare overfitting;
- trattare `30 NASDAQ M1` come riferimento da testare, non come regola attiva;
- miglioramento futuro: confrontare `20`, `30` e percentile/volume relativo per strumento, sessione e volatilità.

---

## 8. Dati Necessari

Per validare il Modello 2 servono:

- `POC/VAH/VAL preview` al momento dello sweep;
- distanza del close da `POC`, `VAH`, `VAL` preview;
- delta candela (`Ask - Bid`);
- cumulative trades live/storici dopo lo sweep (`FirstPrice`, `LastPrice`, `Volume`, `Direction`, `Time`);
- tempo tra sweep e aggressione (`SecondsAfterSweep`);
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
