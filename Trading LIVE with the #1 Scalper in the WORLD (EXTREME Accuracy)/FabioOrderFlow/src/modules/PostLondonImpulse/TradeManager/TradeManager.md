# TradeManager

## 1. Scopo

`TradeManager` trasforma un trigger valido in un trade plan completo.

Non invia ordini. Calcola e disegna livelli operativi.

---

## 2. Input

Da `AggressionDetector`:

```text
TriggerPrice
TriggerBar
Direction
AggressionVolume
```

Da `BalanceZoneTracker`:

```text
BalancePocTarget
VAH
VAL
```

Da strumento ATAS:

```text
InstrumentInfo.TickSize
```

---

## 3. Output

```csharp
public bool HasActivePlan { get; }
public decimal Entry { get; }
public decimal Stop { get; }
public decimal Target { get; }
public decimal RiskTicks { get; }
public decimal RewardTicks { get; }
public decimal RiskReward { get; }
```

---

## 4. Regole

1. Entry: area del trigger aggression.
2. Stop: 2-3 tick oltre il cluster aggression.
3. Target primario: POC della balance precedente.
4. Il piano è valido solo se risk/reward è sensato.
5. Nessun ordine automatico nella prima versione.

---

## 5. Calcolo Direzionale

Bullish:

```text
Entry  = trigger price / area
Stop   = sotto cluster aggression
Target = POC balance precedente o livello definito dal contesto
```

Bearish:

```text
Entry  = trigger price / area
Stop   = sopra cluster aggression
Target = POC balance precedente o livello definito dal contesto
```

---

## 6. Criteri di Validazione

Il modulo è valido se:

1. Ogni trigger valido produce entry, stop e target.
2. Stop e target sono nella direzione corretta.
3. Il POC della balance precedente è usato come target/reference.
4. Risk/reward è loggato.
5. I livelli sono passati a `VisualRenderer`.
