# Historical Cumulative Context Inventory - 2026-08-04

## Stato

```text
Schema valido:        fof-historical-cumulative-context-v4
Range id:             20260728T174359-20260804T174359
Strumento:            NQU6@CME / E-Mini Nasdaq-100
Tipo:                 inventario storico osservativo da chart ATAS
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo report documenta la prima cattura storica valida del recorder **Fabio Historical Cumulative Context Recorder**. Lo scopo e' verificare cosa ATAS restituisce da `RequestForCumulativeTrades(...)` e conservare una base riproducibile per analisi offline successive. Non approva soglie, setup, filtri operativi o un modello.

## Artefatti

| Artefatto | Path | Bytes | SHA-256 |
| --- | --- | --- | --- |
| snapshot | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4.jsonl.gz | 66.994.818 | 8873ed16b70bffa0eff4f646540b4ffcc16a5eb48f9922328914a0d54cd12769 |
| candlesCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-candles.csv | 1.326.674 | 6609fd96cd90a023506a1a62727cbbef41499b4886e99631ec666667abc9b8fc |
| eventsCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-events.csv | 551.542.586 | 0f9ed7f19adee10b590110a4b6b1423257342040425e5aa3e51d9085a931de18 |
| summaryJson | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v4-summary.json | 71.756 | b2a0aa0db2e19d77ee28f1fae7d4b35ce38b6eeb2b53e19153eafa91a1e70af6 |

Il log sorgente ATAS non viene versionato. Al momento della lettura misurava `9.855.336.194` byte.

## Range

```text
Capture begin:        2026-07-28T17:43:59.3097106
Capture end:          2026-08-04T17:43:59.3097106
Capture duration:     7 giorni
Captured bars:        6.901
Loaded begin:         2026-07-26T22:00:00
Loaded end:           2026-08-04T17:43:59.3097106
Loaded duration:      8.822214232761574 giorni
Loaded bars:          9.464
Request count:        1
```

Il recorder `v4` ha correttamente limitato la cattura agli ultimi sette giorni disponibili dal fondo del chart. ATAS aveva precaricato piu' storico (`loadedDurationDays` > 7), ma il capture range resta di sette giorni.

## Conteggi

```text
Snapshot records:             1.985.996
Chart candles:                6.901
ATAS returned records:       1.979.092
ATAS logged records:         1.979.092
Historical CumulativeTrade:   1.979.092
Inside requested window:      1.704.616
Before requested window:      274.460
After requested window:       16
Response skipped before:      0
Response skipped after:       0
Tick-volume mismatches:       0
Empty tick events:            0
```

`historical-cumulative-response` viene scritto dopo la serializzazione degli eventi ricevuti, quindi la presenza della risposta nel log conferma che la richiesta e' terminata. In questa cattura ATAS ha restituito anche record fuori dalla finestra richiesta: il parser li conserva nello snapshot come evidenza e li marca nel CSV con `timeRelationToRequest`. Dallo schema `v5`, il recorder puo' scartare questi eventi prima del log e registrarli solo nei conteggi `skippedBeforeRequest`/`skippedAfterRequest`.

## Relazione Con La Richiesta

| Relazione | Eventi |
| --- | --- |
| after-request | 16 |
| before-request | 274.460 |
| inside-request | 1.704.616 |

Evento piu' antico restituito: `2026-07-27T22:00:00`. Evento piu' recente restituito: `2026-08-04T17:43:59.988895`. Dentro la finestra richiesta: `2026-07-28T17:43:59.535399` -> `2026-08-04T17:43:59.309710`.

## Lato E Volume

| Direction | Eventi | Volume |
| --- | --- | --- |
| Buy | 982.999 | 1885708 |
| Sell | 996.093 | 1887477 |

| Metrica | Min | P10 | Mediana | P90 | Max | Media |
| --- | --- | --- | --- | --- | --- | --- |
| event total volume | 1.00 | 1.00 | 1.00 | 3.00 | 649.00 | 1.91 |
| event tick count | 1.00 | 1.00 | 1.00 | 3.00 | 354.00 | 1.76 |
| event price change ticks | -118.00 | 0.00 | 0.00 | 0.00 | 138.00 | 0.00 |
| candle price levels | 6.00 | 22.00 | 45.00 | 108.00 | 473.00 | 57.38 |

## Eventi Per Data

| Data | Eventi | Volume | Buy | Sell | Primo evento | Ultimo evento |
| --- | --- | --- | --- | --- | --- | --- |
| 2026-07-27 | 5.600 | 11099 | 2.769 | 2.831 | 2026-07-27T22:00:00 | 2026-07-27T23:59:59.491485 |
| 2026-07-28 | 345.962 | 668470 | 171.786 | 174.176 | 2026-07-28T00:00:00.016749 | 2026-07-28T23:59:59.797520 |
| 2026-07-29 | 407.715 | 795467 | 202.492 | 205.223 | 2026-07-29T00:00:00.032916 | 2026-07-29T23:59:59.439805 |
| 2026-07-30 | 369.091 | 688421 | 180.472 | 188.619 | 2026-07-30T00:00:00.061276 | 2026-07-30T23:59:59.605321 |
| 2026-07-31 | 339.790 | 646212 | 171.145 | 168.645 | 2026-07-31T00:00:00.053906 | 2026-07-31T20:59:59.713068 |
| 2026-08-02 | 9.066 | 19137 | 4.546 | 4.520 | 2026-08-02T22:00:00 | 2026-08-02T23:59:59.630929 |
| 2026-08-03 | 272.491 | 515854 | 135.274 | 137.217 | 2026-08-03T00:00:00.049801 | 2026-08-03T23:59:59.669462 |
| 2026-08-04 | 229.377 | 428525 | 114.515 | 114.862 | 2026-08-04T00:00:00.009218 | 2026-08-04T17:43:59.988895 |

## Candle Per Data

| Data | Candle | Volume | Delta |
| --- | --- | --- | --- |
| 2026-07-28 | 317 | 153541 | -3281 |
| 2026-07-29 | 1.380 | 795467 | -4567 |
| 2026-07-30 | 1.380 | 688421 | -437 |
| 2026-07-31 | 1.260 | 646212 | -4066 |
| 2026-08-02 | 120 | 19137 | 247 |
| 2026-08-03 | 1.380 | 515849 | 1613 |
| 2026-08-04 | 1.064 | 428525 | 5431 |

## Limiti

- I record storici sono `CumulativeTrade` ATAS, non un backfill raw tick-by-tick equivalente a `OnNewTrade`.
- I record fuori finestra mostrano che ATAS puo' arrotondare o ampliare la risposta rispetto al begin richiesto, probabilmente per sessione ETH/caricamento interno. Qualsiasi analisi successiva deve scegliere esplicitamente se usare tutti i record restituiti o solo `inside-request`.
- Le righe evento sono fortemente non indipendenti nel tempo. Questo inventario non fornisce probabilita', edge o regole di esecuzione.
- Il contesto candle/footprint e' quello caricato dal chart, con granularita' di barra, non una ricostruzione tick-by-tick del POC di sessione live.

## Prossimo Uso

Il passo successivo corretto e' un report storico descrittivo che usi solo una popolazione dichiarata, per esempio `inside-request`, e confronti gli eventi con contesto candle/footprint e risposta futura da barre. Prima di qualunque ipotesi operativa servono piu' giorni e finestre non sovrapposte.
