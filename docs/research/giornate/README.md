# Giornate

Una analisi per giornata di mercato, un file per giorno, `AAAA-MM-GG.md`.

Serve a togliere l'analisi del giorno dalla conversazione e dai file sparsi: quando a fine
settimana si guarda indietro, la domanda non e' *cosa abbiamo detto* ma **cosa abbiamo misurato e
cosa e' successo dopo**. Un documento per giornata rende quella verifica possibile.

## Cosa Ci Va, E Cosa No

| Ci va | Non ci va |
|---|---|
| I livelli con cui si e' aperta la giornata, e da dove vengono | Il commento a caldo |
| I numeri misurati, con l'ora accanto | Le impressioni non verificate |
| Cosa e' successo su ciascun livello | La previsione presentata come esito |
| Le correzioni fatte in giornata, con cosa le ha causate | Le correzioni nascoste |
| Cosa resta aperto a fine giornata | |

La regola generale del repository vale anche qui: **descrivere non e' validare**, e i risultati
negativi si registrano con la stessa cura di quelli positivi. Una lettura sbagliata in giornata
resta scritta, insieme a cosa l'ha corretta: e' la parte piu' utile del documento.

## Struttura Di Una Giornata

```text
1. Stato            strumento, sessione, chi ha prodotto i dati
2. Il contesto      cosa si sapeva prima dell'apertura: COT, composito, framing del giorno prima
3. I livelli        la lista spinta sul chart, con la derivazione di ciascuno
4. La cronaca       cosa e' successo, con volume e delta accanto a ogni momento
5. Le correzioni    cosa si e' letto male, e cosa l'ha smentito
6. Gli strumenti    monitor, soglie, script usati
7. Cosa resta       aperto a fine giornata
```

## Da Dove Arrivano I Numeri

Tutti dal Data Bridge, con ATAS aperto sul chart NQZ6 M1. Il client e' `bridge.py`; il contratto
e' in [`../metodo/contratto-data-bridge.md`](../metodo/contratto-data-bridge.md).

```bash
python3 FabioOrderFlow/tools/bridge.py candles --from 2026-09-15 --to 2026-09-16 --levels --out giorno.json
```

Le soglie usate in giornata (volume, delta, ampiezza) vanno **scritte accanto al numero che
producono**, mai lasciate implicite. Se una soglia viene dal p95 della distribuzione del giorno, si
dice che e' il p95 e di quante barre.

## Rapporto Con Gli Altri Documenti

- [`../metodo/profile-framing.md`](../metodo/profile-framing.md) — come si costruisce il framing.
  La giornata lo **applica**, non lo ridefinisce.
- [`../metodo/triple-aaa-dossier.md`](../metodo/triple-aaa-dossier.md) — il modello. La giornata
  cita il tier, non ricopia la regola.
- [`../metodo/livelli-sul-chart.md`](../metodo/livelli-sul-chart.md) — come i livelli arrivano sul
  chart. La giornata riporta **quali** livelli, non come si spingono.
- [`../sessioni/`](../sessioni/) — le descrizioni di settimana. Una settimana riassume piu'
  giornate; una giornata non riassume niente, e' la fonte.

## Le Giornate

| Giorno | Cosa e' successo |
|---|---|
| [2026-09-15](2026-09-15.md) | Lunedi' e' il primo giorno pieno su NQZ6. Il valore lascia il nodo dei compratori alle 06:21, il VAL di lunedi' cede alle 09:55, e la lettura del "vuoto" sotto il VAL si rivela sbagliata. |
