# ImpulseProfiler

## 1. Scopo

`ImpulseProfiler` costruisce il profilo volumetrico dell'impulso nato dopo un breakout confermato dal `BalanceZoneTracker`.

Responsabilità:

1. Ricevere `BreakoutBar` e direzione da `BalanceZoneTracker`.
2. Aggregare volume dal breakout alla barra corrente.
3. Fornire un profilo aggiornato dell'impulso.
4. Esporre metriche utili al `LowVolumeNodeDetector`.

Non responsabilità:

- non decide se il mercato è out-of-balance;
- non cerca aggression;
- non calcola entry/stop/target;
- non identifica direttamente il setup finale.

---

## 2. Input

Dal `BalanceZoneTracker`:

```text
IsOutOfBalance = true
Direction = Bullish/Bearish
BreakoutBar = bar del breakout confermato
```

Dal chart ATAS:

```text
IndicatorCandle
GetAllPriceLevels()
```

---

## 3. Output

Il modulo deve esporre:

```csharp
public bool HasActiveImpulse { get; }
public int StartBar { get; }
public int EndBar { get; }
public BreakoutDirection Direction { get; }
public IReadOnlyDictionary<decimal, decimal> Profile { get; }
public decimal High { get; }
public decimal Low { get; }
public decimal TotalVolume { get; }
```

---

## 4. Regole

### 4.1 Start

L'impulso parte dalla barra del breakout confermato.

```text
StartBar = BalanceZoneTracker.BreakoutBar
```

### 4.2 Aggiornamento

Ogni barra successiva nella direzione/out-of-balance aggiorna il profilo con i price levels della barra corrente.

### 4.3 Reset

Reset quando:

- inizia una nuova sessione rilevante;
- `BalanceZoneTracker` crea una nuova balance reference;
- il mercato rientra stabilmente nella vecchia value area e la logica futura decide di invalidare l'impulso.

La prima versione può resettare solo su nuova balance/sessione.

---

## 5. Algoritmo

```text
Se BalanceZoneTracker non è OUT_OF_BALANCE:
  non fare nulla

Se OUT_OF_BALANCE appena confermato:
  inizializza impulse profile da BreakoutBar

Per ogni barra successiva:
  aggiungi price levels al profilo
  aggiorna high/low/volume
  esponi profile al LowVolumeNodeDetector
```

---

## 6. Performance

Come per il `BalanceZoneTracker`, il profilo deve essere incrementale.

Da evitare:

```text
ogni barra → ricalcola tutto dal breakout
```

Da fare:

```text
ogni barra → aggiungi solo i livelli della barra corrente
```

---

## 7. Criteri di Validazione

Il modulo è valido se:

1. Parte solo dopo `OUT_OF_BALANCE`.
2. Usa `BreakoutBar` come origine.
3. Non usa lookback generici.
4. Aggiorna il profilo in modo incrementale.
5. Fornisce dati coerenti al `LowVolumeNodeDetector`.
