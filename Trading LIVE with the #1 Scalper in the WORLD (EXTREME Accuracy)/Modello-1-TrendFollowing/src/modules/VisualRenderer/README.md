# VisualRenderer

## 1. Scopo

`VisualRenderer` centralizza la parte grafica dell'indicatore.

Responsabilità:

1. Disegnare balance zone.
2. Disegnare POC.
3. Evidenziare breakout/out-of-balance.
4. Disegnare low volume node.
5. Disegnare entry, stop e target.
6. Evitare clutter e oggetti duplicati.

---

## 2. Regola Performance

Non ricreare gli oggetti grafici ogni barra.

Corretto:

```text
crea una volta
aggiorna proprietà / SecondBar
cambia colore solo su transizione
```

Sbagliato:

```text
Clear() + recreate ogni barra
```

---

## 3. Layer Visuali

Ordine consigliato:

1. Balance zone `VAH/VAL` — rettangolo trasparente.
2. POC — linea arancione.
3. Breakout state — colore zona o marker.
4. Low volume node — box sottile o highlight.
5. Entry/stop/target — linee operative.
6. Confirmation warnings — marker discreti.

---

## 4. Colori Iniziali

```text
Balance ready: grigio trasparente
Out-of-balance bullish: blu trasparente
Out-of-balance bearish: rosso trasparente
POC: arancione
Entry: verde/rosso direzionale
Stop: rosso
Target: verde
```

---

## 5. Criteri di Validazione

Il modulo è valido se:

1. Il grafico resta leggibile.
2. Le zone non si duplicano ogni barra.
3. I livelli restano allineati allo stato dei moduli.
4. Gli oggetti storici sono limitati.
5. Può essere spento con un flag visuale senza alterare la logica.
