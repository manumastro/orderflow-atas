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

## 5. Profile Preview

Per studiare il Modello 2 serve calcolare livelli provvisori durante la sessione.

Regola tecnica:

```text
Il volume profile si aggiorna ogni barra.
POC/VAH/VAL preview si calcolano ogni N barre o su eventi importanti.
POC/VAH/VAL ufficiali restano congelati solo a fine London per il Modello 1.
```

Parametri iniziali usati per debug:

- aggiornamento profile: ogni barra;
- preview `POC/VAH/VAL`: ogni 5 barre da London 14:00 in poi;
- preview forzata su nuovi massimi/minimi e rejection candidate;
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

## 6. Ipotesi di Detection Iniziale

### 6.1 Fakeout Alto

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

### 6.2 Fakeout Basso

Speculare:

```text
Sweep below preview VAL
→ rejection / close back inside VA
→ return toward preview POC
→ optional break above preview VAH
```

---

## 7. Dati Necessari

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

## 8. Non Obiettivi Attuali

In questa fase il Modello 2 non deve:

- generare segnali operativi automatici;
- disegnare zone proprie sul grafico;
- modificare il comportamento del Modello 1;
- usare la preview come livelli ufficiali;
- assumere che ogni rejection sia tradabile.

---

## 9. Prossimi Passi

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
