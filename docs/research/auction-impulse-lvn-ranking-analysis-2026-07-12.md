# Auction Impulse LVN Ranking Analysis - 2026-07-12

## Scopo

Verificare `RAW_CAUSAL_V1` sul runtime M1 New York-only senza scegliere soglie, entry o PnL.

## Reload

```text
Range:                           2026-05-18 -> 2026-07-10
ChartTimeFrame:                  M1
StudyMode:                       NEW_YORK_IMPULSE_STUDY_NO_TRADES
LvnRanking:                      RAW_CAUSAL_V1
NewYorkBars / LondonBars:        13.303 / 0
Cumulative ricevuti / matched:   5.833.055 / 4.170.108
READY / PULLBACK / RESOLVED:     396 / 1.132 / 396
LONG / SHORT:                    220 / 176
Coverage AVAILABLE / MISSING:    1.095 / 37
Operativita' / errori:           0 / 0
```

Il replay mode-to-summary richiede `32,638s`. Il log ranked misura `6.259.755` byte: piu' del runtime NY-only senza metriche, ma circa `90,5%` meno della baseline dual-session/compression.

## Integrita' LVN

```text
Raw LVN ranked:                  23.030
Profili con almeno un raw LVN:  383 / 396
Touch occurrences:              31.676
LVN per profilo con LVN:
- min / mediana / media / max:   1 / 41 / 60,13 / 533

Rank permutation errors:        0
Metric bound errors:            0
Touch subset errors:            0
```

Ogni `ProminenceRank` e' una permutazione `1..N` all'interno dell'impulso. Ogni touched LVN coincide esattamente, prezzo e metriche, con un raw LVN dello stesso `ImpulseId`.

## Campione Causale

Si usa soltanto il primo pullback con `FirstPullbackBar < ResolvedBar`.

```text
Profili con first pullback:           392
Resolved sulla prima pullback bar:    154 esclusi
Senza raw LVN:                          3 esclusi
Senza LVN touch al primo pullback:     24 esclusi
Osservazioni causali:                 211

Continuation:                         129
Origin reentry:                        79
Two-sided:                              3
```

La barra che realizza gia' continuation/reentry non viene trattata come predittiva.

## Risultato Continuo

AUC descrittiva: probabilita' che una continuation abbia metrica maggiore di un origin reentry; tie `0,5`. Non e' un modello classificatore.

```text
Metrica                              Tutti impulsi   Prima per data/direzione
BestRankScore                            0,490                 0,520
BestProminence                           0,499                 0,484
BestAdjacentDepth                        0,480                 0,451
BestShoulderDepth                        0,540                 0,482
MinimumVolumePercentile                  0,509                 0,516
MinimumDirectionalProgress              0,627                 0,540
MaximumDirectionalProgress              0,616                 0,552
MinimumDistanceToPocRanges               0,533                 0,606
MinimumDistanceToOriginRanges            0,629                 0,560
TouchedLvnCount                          0,508                 0,386
```

Prominence, rank e volume percentile sono sostanzialmente casuali rispetto all'outcome. Location/directional progress mostrano separazione moderata su tutti gli impulsi, ma scendono a `0,54-0,56` quando il peso intraday viene ridotto. `MinimumDistanceToPocRanges=0,606` nel campione primario e' una singola lettura post-hoc, non una conferma.

Per direzione, `MinimumDirectionalProgress` e' `0,602` LONG e `0,654` SHORT; `MinimumDistanceToOriginRanges` e' `0,615` LONG e `0,647` SHORT. La direzione non risolve il limite di dipendenza e selection leakage.

## Decisione

```text
Validated=FALSE
PromotedToShadow=FALSE
OperationalEntry=FALSE
OrdersSubmitted=FALSE
PnL=NONE
selectionLeakage=true
```

Non usare `ProminenceRank`, prominence o percentile come filtro. Mantenere le metriche nel ledger per date future. La sola traccia diagnostica e' la profondita'/location del primo pullback, ma non giustifica una soglia o la combinazione con cumulative aggression sul campione corrente.
