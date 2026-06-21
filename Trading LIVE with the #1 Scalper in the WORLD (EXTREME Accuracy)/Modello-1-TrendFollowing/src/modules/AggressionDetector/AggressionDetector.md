# AggressionDetector

## 1. Scopo

`AggressionDetector` cerca il trigger operativo del modello: aggression cluster nella direzione dell'out-of-balance e dentro/attorno a un low volume node valido.

L'aggression non è un segnale autonomo. È valida solo dopo:

```text
BalanceZoneTracker = OUT_OF_BALANCE
ImpulseProfiler = active impulse
LowVolumeNodeDetector = valid location
```

---

## 2. Input

Da `BalanceZoneTracker`:

```text
Direction = Bullish/Bearish
IsOutOfBalance = true
```

Da `LowVolumeNodeDetector`:

```text
NodeLow / NodeHigh / NodeMid
```

Da ATAS:

```text
IndicatorCandle.GetAllPriceLevels()
PriceVolumeInfo.Ask
PriceVolumeInfo.Bid
PriceVolumeInfo.Volume
```

---

## 3. Output

```csharp
public bool HasAggressionTrigger { get; }
public decimal? TriggerPrice { get; }
public decimal? AggressionVolume { get; }
public BreakoutDirection? Direction { get; }
public int? TriggerBar { get; }
```

---

## 4. Regole

1. Bullish setup: cercare buy aggression.
2. Bearish setup: cercare sell aggression.
3. Il trigger deve trovarsi in location valida, non ovunque.
4. Richiedere cambio regime small orders → big orders quando implementato.
5. Richiedere continuation come filtro successivo, non nella primissima versione se rallenta la validazione.

---

## 5. Soglie Iniziali

Decisione iniziale per NY:

```text
Big trade / aggression threshold = 30 contratti
```

La soglia resta costante interna finché non validata visivamente.

---

## 6. Algoritmo Iniziale

```text
Se non siamo OUT_OF_BALANCE:
  skip

Se non esiste low volume node valido:
  skip

Per la barra corrente:
  leggi price levels
  calcola net aggression Ask-Bid per livello
  filtra livelli dentro/attorno al low volume node
  se aggression supera soglia nella direzione del breakout:
    genera trigger candidate
```

---

## 7. Criteri di Validazione

Il modulo è valido se:

1. Non produce trigger in balance.
2. Non produce trigger fuori location.
3. Rispetta la direzione del breakout.
4. Logga prezzo, volume e direzione del trigger.
5. Fornisce dati sufficienti al `TradeManager`.
