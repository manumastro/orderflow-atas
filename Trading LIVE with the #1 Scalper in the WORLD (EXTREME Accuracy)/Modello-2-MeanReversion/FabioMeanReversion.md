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

---

## 7. Log ATAS

La guida canonica per interpretare i log è `../docs/atas/log-reading.md`.

Per i dettagli specifici del Modello 2, rimanda a quella guida e poi usa qui solo la logica del fakeout / mean reversion.

---

## 8. Ipotesi di Detection Iniziale

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
- outcome post-entry: MFE, MAE, target hit, invalidation;
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

---

## 11. Outcome Tracking e Entry Models

### 11.1 Exit Management

I trade mean reversion vengono chiusi automaticamente quando:
- **Target2 raggiunto** → exit con `ExitReason=TARGET2_HIT`
- **Stop colpito** → exit con `ExitReason=STOP_HIT`

La classe `MeanReversionOutcome` traccia:
- `PositionClosed` - se la posizione è stata chiusa
- `FinalPnL` - profitto/perdita finale in punti
- `ExitReason` - motivo della chiusura
- `ExitBar`, `ExitTime` - barra e timestamp di uscita

Log generato: `[MR_POSITION_CLOSED]` con PnL finale e statistiche complete.

### 11.2 Entry Models

Il sistema supporta due approcci di entry:

#### M5 + Aggression Confirm (Storico e Live)

**Trigger basati su struttura candela M5:**
- `[MR_EARLY_TRIGGER]` - Entry anticipata (candela dopo rejection)
- `[MR_TRIGGER]` - Entry conservativa (reclaim POC)

**Conferma footprint:**
- `[MR_AGGRESSION_CONFIRM]` con `EntryModel=FootprintCumulativeTradeHistorical`
- Dopo trigger M5, cerca big trades (≥20 contratti) nei cumulative trades
- Valida aggression dentro l'entry area della candela trigger
- Log: `SecondsAfterSweep`, `Volume`, `TradeDirection`, `EntryPrice`
- **Funziona sia su storico che su live**

#### Footprint-First (Solo Live)

**Parametro ATAS:** `EnableLiveFootprintFirst` (default: `false`)

Sistema opzionale per entry real-time con latenza ridotta. Disabilitato di default.

**Pipeline real-time:**
1. **Sweep Detection** - `[FOOTPRINT_HIGH_SWEEP]` / `[FOOTPRINT_LOW_SWEEP]`
   - Big trade (≥20) rompe sopra VAH o sotto VAL
   - Salva livelli VAH/VAL/POC al momento dello sweep

2. **Rejection Detection** - `[FOOTPRINT_REJECTION]`
   - Big trade direzione opposta dopo sweep
   - Log: `SecondsAfterSweep`, latenza media ~83s

3. **Entry Detection** - `[FOOTPRINT_ENTRY]`
   - Big buy (long) se `price >= sweep.VAL`
   - Big sell (short) se `price <= sweep.VAH`
   - `EntryModel=FootprintFirst`
   - Log: `SweepToEntrySeconds`, `RejectionToEntrySeconds`

**Gestione stato:**
- Reset sweep senza rejection ad ogni nuova barra (`OnBarUpdate`)
- Timeout 300s (5 min) per sweep con rejection se entry non arriva
- Uno sweep attivo per direzione (long/short) per volta

**Limitazione:** Footprint-first funziona solo su **live/replay real-time** perché richiede processing tick-by-tick. Su dati storici batch, usa M5 + Aggression Confirm.

**Abilitazione:** Spunta `EnableLiveFootprintFirst` nelle proprietà dell'indicatore ATAS. Default: disabilitato per mantenere comportamento identico a storico.

**Implementazione tecnica:**
- Classe `LiveSweepCandidate` con VAH/VAL/POC salvati
- Processing in `OnLiveCumulativeTrade` per detection real-time
- Metodi helper: `IsHighSweepTrade`, `IsLowSweepTrade`, `IsRejectionTrade`, `IsAggressionEntryTrade`

### 11.3 Statistiche Entry (24 Giugno 2026, Dati Storici)

**M5 + Aggression Confirm:**
- 15 entry totali (7 short, 6+ long)
- 14 posizioni chiuse con exit management
- Latenze sweep→entry: min 0s, max 290s, media 83s
- Win rate: da validare con analisi outcome

**Footprint-First:**
- Non applicabile su dati storici (richiede live)
- Da testare su live trading per confrontare con M5

### 11.4 Log Monitoring

```bash
# Exit management (storico e live)
grep "MR_POSITION_CLOSED" FabioTrendFollowing.log

# Trigger M5 (storico e live)
grep "MR_TRIGGER\|MR_EARLY_TRIGGER" FabioTrendFollowing.log

# Footprint aggression M5 (storico e live)
grep "MR_AGGRESSION_CONFIRM" FabioTrendFollowing.log

# Footprint-first (solo live)
grep "FOOTPRINT_" FabioTrendFollowing.log
```

### 11.5 Codice

**File:** `BalanceZoneTracker.cs`

**Classi principali:**
- `MeanReversionOutcome` (linea ~77) - Tracking completo outcome con exit management
- `LiveSweepCandidate` (linea ~103) - Sweep con livelli VAH/VAL/POC salvati

**Metodi chiave:**
- `OnHistoricalCumulativeTrades` (linea ~215) - Solo M5 + aggression confirm
- `OnLiveCumulativeTrade` (linea ~325) - M5 + aggression + footprint-first
- `OnBarUpdate` (linea ~389) - Reset sweep per nuova barra
- `EvaluateLongOutcome`, `EvaluateShortOutcome` (linea ~1145) - Exit management
- `#region Footprint-First Trigger Detection` (linea ~1550+) - 9 metodi helper
