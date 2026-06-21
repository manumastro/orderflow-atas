# ConfirmationLayer

## 1. Scopo

`ConfirmationLayer` contiene segnali secondari che migliorano la qualità del setup.

Non deve produrre trade autonomi.

Conferme previste:

1. Absorption multi-barra.
2. CVD divergence / confirmation.
3. Eventuale failure di continuation.

---

## 2. Regola Principale

```text
ConfirmationLayer può solo confermare o filtrare un setup già valido.
Non può creare un setup da zero.
```

Setup già valido significa:

```text
OUT_OF_BALANCE
+ impulse profile
+ low volume node
+ aggression trigger
+ trade plan
```

---

## 3. Absorption

Absorption target:

```text
pressione aggressiva ripetuta
+ prezzo che non riesce a passare
+ reazione/conferma opposta
```

Non basta:

```text
delta forte + close che tiene
```

Serve pattern multi-barra.

---

## 4. CVD

Il CVD serve come conferma della qualità del movimento.

Esempi:

- prezzo continua ma CVD non conferma → warning;
- prezzo rifiuta livello e CVD diverge → conferma possibile;
- CVD da solo non basta mai.

---

## 5. Output

```csharp
public bool IsConfirmed { get; }
public bool HasWarning { get; }
public string Reason { get; }
```

---

## 6. Criteri di Validazione

Il modulo è valido se:

1. Non produce segnali senza setup primario.
2. Non blocca il BalanceZoneTracker.
3. Logga conferme e warning in modo leggibile.
4. Può essere disattivato senza rompere la pipeline principale.
