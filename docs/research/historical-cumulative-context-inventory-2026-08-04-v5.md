# Historical Cumulative Context Inventory - 2026-08-04

## Stato

```text
Schema valido:        fof-historical-cumulative-context-v5
Range id:             20260728T182648-20260804T182648
Strumento:            NQU6@CME / E-Mini Nasdaq-100
Tipo:                 inventario storico osservativo da chart ATAS
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo report documenta la prima cattura storica valida del recorder **Fabio Historical Cumulative Context Recorder**. Lo scopo e' verificare cosa ATAS restituisce da `RequestForCumulativeTrades(...)` e conservare una base riproducibile per analisi offline successive. Non approva soglie, setup, filtri operativi o un modello.

## Artefatti

| Artefatto | Path | Bytes | SHA-256 |
| --- | --- | --- | --- |
| snapshot | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5.jsonl.gz | 58.429.471 | 21e284b969f8e244ce4736b4935b09264a4a93cbd240eaa6064dbbaac7118a76 |
| candlesCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-candles.csv | 1.326.709 | db282d6c1a2725d54bc8a5504b8dcfbd13f0dc69fa3ca0895656db6eb71330c6 |
| eventsCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-events.csv | 476.426.379 | 8d6cdaa52f77b489570c66cc35b0ca3ee6027ba3326dccb0aa4d977e5f2b2162 |
| summaryJson | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-summary.json | 71.593 | 44a9ae77be6b8fb7716b4e6d35b765450070dfc6f656db9d009f7ae06d644461 |

Il log sorgente ATAS non viene versionato. Al momento della lettura misurava `15.081.627.430` byte.

## Range

```text
Capture begin:        2026-07-28T18:26:48.4423654
Capture end:          2026-08-04T18:26:48.4423654
Capture duration:     7 giorni
Captured bars:        6.901
Loaded begin:         2026-07-26T22:00:00
Loaded end:           2026-08-04T18:26:48.4423654
Loaded duration:      8.851949564414351 giorni
Loaded bars:          9.507
Request count:        1
```

Il recorder `fof-historical-cumulative-context-v5` ha correttamente limitato la cattura agli ultimi sette giorni disponibili dal fondo del chart. ATAS aveva precaricato piu' storico (`loadedDurationDays` > 7), ma il capture range resta di sette giorni.

## Conteggi

```text
Snapshot records:             1.717.006
Chart candles:                6.901
ATAS returned records:       1.999.263
ATAS logged records:         1.710.102
Historical CumulativeTrade:   1.710.102
Inside requested window:      1.710.102
Before requested window:      0
After requested window:       0
Response skipped before:      289.156
Response skipped after:       5
Tick-volume mismatches:       0
Empty tick events:            0
```

`historical-cumulative-response` viene scritto dopo la serializzazione degli eventi accettati, quindi la presenza della risposta nel log conferma che la richiesta e' terminata. ATAS ha restituito anche record fuori dalla finestra richiesta; il recorder li ha esclusi dal log e li ha conservati solo nei conteggi `skippedBeforeRequest`/`skippedAfterRequest`.

## Relazione Con La Richiesta

| Relazione | Eventi |
| --- | --- |
| inside-request | 1.710.102 |

Evento piu' antico nel dataset: `2026-07-28T18:26:49.719158`. Evento piu' recente nel dataset: `2026-08-04T18:26:48.442265`. Dentro la finestra richiesta: `2026-07-28T18:26:49.719158` -> `2026-08-04T18:26:48.442265`.

## Lato E Volume

| Direction | Eventi | Volume |
| --- | --- | --- |
| Buy | 848.573 | 1625078 |
| Sell | 861.529 | 1630203 |

| Metrica | Min | P10 | Mediana | P90 | Max | Media |
| --- | --- | --- | --- | --- | --- | --- |
| event total volume | 1.00 | 1.00 | 1.00 | 3.00 | 649.00 | 1.90 |
| event tick count | 1.00 | 1.00 | 1.00 | 3.00 | 354.00 | 1.75 |
| event price change ticks | -118.00 | 0.00 | 0.00 | 0.00 | 138.00 | 0.00 |
| candle price levels | 6.00 | 22.00 | 45.00 | 108.00 | 473.00 | 57.33 |

## Eventi Per Data

| Data | Eventi | Volume | Buy | Sell | Primo evento | Ultimo evento |
| --- | --- | --- | --- | --- | --- | --- |
| 2026-07-28 | 62.406 | 124637 | 29.888 | 32.518 | 2026-07-28T18:26:49.719158 | 2026-07-28T23:59:59.797520 |
| 2026-07-29 | 407.715 | 795467 | 202.492 | 205.223 | 2026-07-29T00:00:00.032916 | 2026-07-29T23:59:59.439805 |
| 2026-07-30 | 369.091 | 688421 | 180.472 | 188.619 | 2026-07-30T00:00:00.061276 | 2026-07-30T23:59:59.605321 |
| 2026-07-31 | 339.790 | 646212 | 171.145 | 168.645 | 2026-07-31T00:00:00.053906 | 2026-07-31T20:59:59.713068 |
| 2026-08-02 | 9.066 | 19137 | 4.546 | 4.520 | 2026-08-02T22:00:00 | 2026-08-02T23:59:59.630929 |
| 2026-08-03 | 272.491 | 515854 | 135.274 | 137.217 | 2026-08-03T00:00:00.049801 | 2026-08-03T23:59:59.669462 |
| 2026-08-04 | 249.543 | 465553 | 124.756 | 124.787 | 2026-08-04T00:00:00.009218 | 2026-08-04T18:26:48.442265 |

## Candle Per Data

| Data | Candle | Volume | Delta |
| --- | --- | --- | --- |
| 2026-07-28 | 274 | 124939 | -3209 |
| 2026-07-29 | 1.380 | 795467 | -4567 |
| 2026-07-30 | 1.380 | 688421 | -437 |
| 2026-07-31 | 1.260 | 646212 | -4066 |
| 2026-08-02 | 120 | 19137 | 247 |
| 2026-08-03 | 1.380 | 515849 | 1613 |
| 2026-08-04 | 1.107 | 465563 | 5299 |

## Limiti

- I record storici sono `CumulativeTrade` ATAS, non un backfill raw tick-by-tick equivalente a `OnNewTrade`.
- ATAS puo' ampliare la risposta rispetto al begin/end richiesto, probabilmente per sessione ETH/caricamento interno. Questo snapshot contiene solo gli eventi accettati dentro finestra; i record esclusi restano verificabili nei conteggi della risposta.
- Le righe evento sono fortemente non indipendenti nel tempo. Questo inventario non fornisce probabilita', edge o regole di esecuzione.
- Il contesto candle/footprint e' quello caricato dal chart, con granularita' di barra, non una ricostruzione tick-by-tick del POC di sessione live.

## Prossimo Uso

Il passo successivo corretto e' un report storico descrittivo che usi solo una popolazione dichiarata, per esempio `inside-request`, e confronti gli eventi con contesto candle/footprint e risposta futura da barre. Prima di qualunque ipotesi operativa servono piu' giorni e finestre non sovrapposte.
