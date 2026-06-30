# BalanceZoneTracker

Modulo shared che costruisce le reference di volume profile usate dai modelli. Per il modello attivo London MR pubblica POC/VAH/VAL preview durante London e inoltra eventi/trade al modello.

## Responsabilita'

```text
1. riconoscere sessione London tramite MarketTimeZones
2. aggregare volume per price level dai candle price levels ATAS
3. calcolare POC, VAH, VAL dinamici durante London
4. evitare duplicazione volume quando ATAS aggiorna la stessa candela
5. notificare al LondonMeanReversionModel nuovi high/low London
6. inoltrare cumulative trades live/storici al modello
7. congelare/disegnare la balance zone a fine London
```

Non decide entry, stop, target, PnL o filtri di big trade. Quelle regole stanno nel modello specifico.

## Sessioni

```text
London:   08:00-16:00 London local time
New York: 09:30-16:00 New York local time
```

Le conversioni passano sempre da:

```csharp
MarketTimeZones.ToLondon(utcTime)
MarketTimeZones.ToItaly(utcTime)
MarketTimeZones.ToNewYork(utcTime)
```

## Profilo London

Durante London il tracker mantiene un profilo in costruzione:

```text
candele London -> price levels ATAS -> volume per prezzo -> POC/VAH/VAL preview
```

Valori pubblici usati dal modello:

```csharp
LastPreviewPoc
LastPreviewVah
LastPreviewVal
```

## Calcolo Livelli

```text
POC = price level con volume massimo
Tie-break POC = prezzo piu' basso se volumi uguali
Value Area = area contigua intorno al POC fino al 70% del volume
VAL = prezzo piu' basso incluso nella value area
VAH = prezzo piu' alto incluso nella value area
```

Espansione value area: a ogni passo include il lato adiacente con volume maggiore; in caso di parita' preferisce il lato basso.

## Anti-Duplicazione Volume

ATAS puo' richiamare piu' volte la stessa candela mentre si aggiorna. Il tracker mantiene snapshot per bar:

```text
bar -> { price -> volume }
```

Quando il bar viene aggiornato:

```text
1. sottrae il contributo precedente del bar
2. legge il nuovo snapshot dei price levels
3. aggiunge il nuovo contributo
4. ricalcola il totale dagli snapshot attivi
```

Questo mantiene il profilo live coerente e impedisce POC/VAH/VAL falsati da volume duplicato.

## Contratto Con LondonMeanReversionModel

Eventi barre:

```text
UpdateLondonProfile()
-> OnNewSessionHigh(bar, candle, previousHigh)
-> OnNewSessionLow(bar, candle, previousLow)
-> OnBarUpdate(bar, currentBar, candle)
```

Cumulative trades:

```text
FabioOrderFlow.OnCumulativeTrade / OnUpdateCumulativeTrade
-> BalanceZoneTracker.OnLiveCumulativeTrade
-> LondonMeanReversionModel.OnLiveCumulativeTrade

FabioOrderFlow.OnCumulativeTradesResponse
-> BalanceZoneTracker.OnHistoricalCumulativeTrades
-> LondonMeanReversionModel.OnHistoricalCumulativeTrades
```

## Stato Post-London

Il tracker mantiene anche una state machine utile a modelli futuri/post-London:

```text
NoZone -> BuildingSessionProfile -> BalanceReady -> BreakoutPending -> OutOfBalance
```

Per London MR la fase critica e' `BuildingSessionProfile`, perche' il modello lavora sulla value area preview mentre London e' ancora aperta.

## Visual

A fine London il tracker disegna:

```text
box sessione High/Low
linea POC
linea VAH
linea VAL
```

Il box usa High/Low per coprire tutta la sessione. La business logic usa VAH/VAL/POC.

## Validazione

Il tracker e' sano se:

```text
POC/VAH/VAL preview cambiano durante London senza volume duplicato
nuovi high/low London arrivano al modello
trade live/storici arrivano al modello
[ZONE_READY] appare a fine London completa
build Release passa senza errori/warning
```
