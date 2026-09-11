# Historical Cumulative Cash Session Description - 2026-08-04

## Stato

```text
Schema sorgente:      fof-historical-cumulative-context-v5
Strumento:            NQU6@CME
Sessione:             NQ US Cash, 09:30-16:00 America/New_York
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo report descrive lo storico `CumulativeTrade` ATAS nella sola sessione cash. E' un controllo di dati, contesto e risposta temporale. Non approva soglie, setup, filtri operativi o un modello.

## Artefatti

| Artefatto | Path | Bytes | SHA-256 |
| --- | --- | --- | --- |
| cashEventsCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-cash-events.csv | 385.472.030 | 94c56937ce6baeeb5aabcd6db1f854c11a7b00336b09402150b6d51a0de3fca2 |
| anchorsCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-cash-anchors.csv | 336.292 | 25e461c54920485f5ac285284660af5d3581cfb8072518fb525ce4a6c1c5f66b |
| nonOverlapEventsCsv | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-cash-non-overlap-events.csv | 38.454 | 06f8b0d96f194b20e88669ab56d604a268588b4bc1e21a945f4b7cf7dc39dd23 |
| cashSummaryJson | FabioOrderFlow/ledger-snapshots/historical-cumulative-context-2026-08-04-v5-cash-summary.json | 17.723 | 86125cc49ce7cc6a62e84270874bace5f41a22a1bd20cab91c8f123dddb83080 |

## Copertura Cash

| Data NY | Copertura | Candle | Eventi | Volume | Buy | Sell | Non-overlap 900s |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 2026-07-28 | partial | 94 | 39.671 | 72572 | 18.697 | 20.974 | 6 |
| 2026-07-29 | full | 390 | 304.048 | 579762 | 150.787 | 153.261 | 25 |
| 2026-07-30 | full | 390 | 266.109 | 480572 | 130.451 | 135.658 | 25 |
| 2026-07-31 | full | 390 | 263.221 | 496536 | 132.586 | 130.635 | 25 |
| 2026-08-03 | full | 390 | 205.725 | 385275 | 102.456 | 103.269 | 25 |
| 2026-08-04 | partial | 297 | 185.623 | 343852 | 93.190 | 92.433 | 19 |

Le date `2026-07-28, 2026-08-04` sono parziali. Non vengono trattate come sessioni complete nel testo interpretativo.

## Metodo

Ogni timestamp sorgente e' interpretato come UTC e convertito esplicitamente in `America/New_York`. Un evento entra nel dataset solo se `09:30 <= timeNewYork < 16:00`. L'evento viene associato alla candle a 1 minuto che lo contiene. La risposta e' il close della prima candle disponibile al o dopo 60, 180, 300 o 900 secondi, meno `lastPrice` dell'evento, espresso in tick. Non viene calcolata alcuna risposta che attraversi il close cash.

Le ancore usano lo stesso calcolo, ma partono dall'open di ogni candle cash. Sono una baseline temporale: controllano il movimento normalmente disponibile nel medesimo periodo, non la causalita' dell'evento.

## Eventi E Baseline

| Popolazione | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| events 60s | 1.241.074 | 664.028 | 569.588 | 7.458 | 3.8 | -496 | 653 |
| anchors 60s | 1.945 | 1.047 | 886 | 12 | 3.31 | -463 | 641 |
| events 180s | 1.229.302 | 669.201 | 554.778 | 5.323 | 6.1 | -637 | 767 |
| anchors 180s | 1.933 | 1.065 | 852 | 16 | 6.48 | -590 | 721 |
| events 300s | 1.216.961 | 670.175 | 543.071 | 3.715 | 6.25 | -827 | 793 |
| anchors 300s | 1.921 | 1.073 | 839 | 9 | 9.29 | -802 | 729 |
| events 900s | 1.176.413 | 664.623 | 510.001 | 1.789 | 2.94 | -1511 | 1267 |
| anchors 900s | 1.861 | 1.104 | 751 | 6 | 24.5 | -1346 | 1166 |

Le righe evento sono molto sovrapposte. Le differenze tra eventi e ancore sono descrittive e non sono una stima di edge o probabilita'.

## Contesto Candle A 300 Secondi

### Direction

| Gruppo | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Buy | 604.587 | 332.759 | 270.028 | 1.800 | 6 | -827 | 789 |
| Sell | 612.374 | 337.416 | 273.043 | 1.915 | 6.5 | -825 | 793 |

### Posizione Vs POC Candle

| Gruppo | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| above-candle-poc | 569.103 | 283.799 | 283.848 | 1.456 | -16.39 | -827 | 708 |
| at-candle-poc | 34.907 | 19.012 | 15.741 | 154 | 8.51 | -753 | 709 |
| below-candle-poc | 612.951 | 367.364 | 243.482 | 2.105 | 27.15 | -733 | 793 |

### Posizione Vs Value Area Candle

| Gruppo | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| above-value-area | 169.001 | 76.524 | 91.948 | 529 | -40.66 | -827 | 691 |
| below-value-area | 190.257 | 125.717 | 63.989 | 551 | 53.34 | -673 | 780 |
| inside-value-area | 857.703 | 467.934 | 387.134 | 2.635 | 5.05 | -769 | 793 |

### Posizione Vs VWAP Candle

| Gruppo | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| above-vwap | 605.114 | 296.330 | 306.937 | 1.847 | -18.96 | -827 | 705 |
| at-vwap | 8 | 8 | 0 | 0 | 133 | 133 | 133 |
| below-vwap | 611.839 | 373.837 | 236.134 | 1.868 | 31.19 | -759 | 793 |

Queste classificazioni descrivono il contesto della candle storica ATAS. Non sono un POC di sessione ricostruito tick-by-tick e non definiscono trigger.

## Controllo Non Sovrapposto

Per ogni data cash il parser seleziona il primo evento con risposta completa a 900 secondi, poi il primo almeno 900 secondi dopo. Il risultato e':

| Gruppo | N | Positivi | Negativi | Zero | Media tick | Min | Max |
| --- | --- | --- | --- | --- | --- | --- | --- |
| selected events | 125 | 74 | 51 | 0 | 44.37 | -1044 | 870 |

La forte riduzione rispetto alla popolazione completa mostra che i milioni di eventi non sono milioni di osservazioni indipendenti.

## Limiti

- Il dataset storico contiene `CumulativeTrade` ATAS, non raw trade storico equivalente a `OnNewTrade`.
- Il report copre sei date cash, di cui due parziali. Non convalida un modello o probabilita'.
- POC, value area e VWAP sono campi di candle storica; non sostituiscono il profilo di sessione in sviluppo del recorder live.
- Il lato Buy/Sell indica il campo ATAS dell'evento, non l'intenzione, il profilo o l'orizzonte operativo del partecipante.

## Esito

Il risultato utile di questa fase e' una popolazione cash dichiarata, con risposta da candle, baseline temporale e controllo non sovrapposto. Qualunque ipotesi futura deve essere definita dopo questa descrizione, testata su giorni addizionali e mantenuta distinta da una regola eseguibile.
