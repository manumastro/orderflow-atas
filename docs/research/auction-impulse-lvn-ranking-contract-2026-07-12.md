# Auction Impulse LVN Ranking Contract - 2026-07-12

## Scopo

Aggiungere rilevanza causale a tutti gli LVN raw del profilo New York A->B senza scegliere una soglia e senza eliminare livelli.

```text
Model:              NEW_YORK_A_TO_B_CAUSAL
LVN ranking:        RAW_CAUSAL_V1
Availability:       READY, prima della prima barra pullback
Selection filters:  NONE
Operational entry:  FALSE
Orders / PnL:       NONE
```

## Minimo Raw

Un livello interno e' raw LVN quando il suo volume e' minore o uguale al volume dei due livelli adiacenti. I plateau restano rappresentati integralmente. Il primo e l'ultimo livello del profilo non sono minimi locali.

Nessun percentile, depth, prominence, rank o location scarta un livello.

## Metriche

Ogni record `ImpulseLvnMetrics` usa questo ordine:

```text
Price
VolumePercentile
AdjacentDepth
ShoulderDepth
Prominence
ProminenceRank
ProminenceRankScore
PositionInRange
DirectionalProgress
DistanceToPocRanges
DistanceToOriginRanges
DistanceToEdgeRanges
```

Definizioni:

```text
VolumePercentile
= quota dei livelli del profilo con volume <= volume LVN.

AdjacentDepth
= (min(volume adiacente sinistro, destro) - volume LVN)
  / min(volume adiacente sinistro, destro), limitato inferiormente a zero.

ShoulderDepth
= (min(picco massimo a sinistra, picco massimo a destra) - volume LVN)
  / min(picco massimo a sinistra, picco massimo a destra), >=0.

Prominence
= differenza di volume usata da ShoulderDepth / volume POC del profilo.

ProminenceRank
= 1 per prominenza maggiore; tie-break ShoulderDepth discendente,
  VolumePercentile ascendente, Price ascendente.

ProminenceRankScore
= 1 per rank migliore, 0 per rank peggiore; distribuzione lineare nel profilo.

PositionInRange
= (Price - ImpulseLow) / (ImpulseHigh - ImpulseLow).

DirectionalProgress LONG
= (Price - OriginBoundary) / (ImpulseHigh - OriginBoundary).

DirectionalProgress SHORT
= (OriginBoundary - Price) / (OriginBoundary - ImpulseLow).

DistanceToPocRanges / DistanceToOriginRanges
= distanza assoluta divisa per ImpulseHigh - ImpulseLow.

DistanceToEdgeRanges
= distanza dal bordo impulso piu' vicino divisa per il range impulso.
```

Le metriche possono essere zero e `DirectionalProgress` puo' essere negativo quando il livello e' dietro il boundary di origine. Sono descrittori, non condizioni.

## Marker Normalizzati

```text
AUCTION_IMPULSE_READY
- contiene geometria, tutti gli ImpulseLvns e tutti gli ImpulseLvnMetrics.

AUCTION_IMPULSE_PULLBACK_BAR
- contiene solo identita', dati pullback, TouchedLvns e TouchedLvnMetrics.

AUCTION_IMPULSE_RESOLVED
- contiene solo identita' e outcome.
```

Il report unisce pullback e resolution al READY tramite `ImpulseId`. Questo evita di ripetere l'intero profilo su ogni marker.

## Export

```bash
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
```

Oltre ai CSV esistenti salva:

```text
auction-impulse-*-lvns.csv          una riga per ogni raw LVN ranked
auction-impulse-*-touched-lvns.csv  una riga per ogni LVN attraversato da un pullback
```

`stdout` resta esclusivamente JSON. Il report dichiara `allRawLvnsRetained=true` e `selectionThresholds=[]`.

## Validazione Richiesta

```text
1. LvnRanking=RAW_CAUSAL_V1 nel mode marker.
2. Conteggi READY/PULLBACK/RESOLVED invariati sul chart M1 40 date.
3. Ogni profilo mantiene lo stesso ImpulseLvns legacy.
4. ProminenceRank e' una permutazione 1..N per ogni ImpulseId.
5. TouchedLvnMetrics e' sottoinsieme esatto degli LVN dello stesso impulso.
6. Nessun marker operativo, ordine o PnL.
7. Confronto outcome solo dopo il reload; nessuna soglia scelta sul campione.
```
