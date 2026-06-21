# LowVolumeNodeDetector

## 1. Scopo

`LowVolumeNodeDetector` identifica zone di volume scarso dentro il profilo dell'impulso prodotto da `ImpulseProfiler`.

Nel Modello 1 il low volume node è una location, non un trigger autonomo.

---

## 2. Input

Da `ImpulseProfiler`:

```text
Active impulse profile
Direction
StartBar / EndBar
High / Low
TotalVolume
```

---

## 3. Output

```csharp
public bool HasCandidateNode { get; }
public decimal? NodeLow { get; }
public decimal? NodeHigh { get; }
public decimal? NodeMid { get; }
```

---

## 4. Regole

1. Cercare low volume node solo dentro l'impulso attivo.
2. Non usare lookback generici.
3. Il nodo deve essere vicino alla zona in cui si cerca aggression.
4. Il nodo non genera trade da solo.

---

## 5. Algoritmo Iniziale

```text
1. Prendi il profilo dell'impulso.
2. Calcola volume medio per price level.
3. Marca livelli con volume < 30% della media.
4. Raggruppa livelli adiacenti in zone.
5. Seleziona la zona più rilevante rispetto al prezzo corrente e alla direzione.
```

---

## 6. Criteri di Validazione

Il modulo è valido se:

1. I low volume node appartengono all'impulso.
2. Non appaiono prima del breakout.
3. Non cambiano casualmente per finestre mobili.
4. Forniscono location chiare all'`AggressionDetector`.
