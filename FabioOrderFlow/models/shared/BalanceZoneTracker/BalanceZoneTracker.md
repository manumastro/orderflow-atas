# BalanceZoneTracker

Baseline shared London. Dal runtime NY-only 2026-07-12 resta compilato ma non viene costruito dall'orchestrator: non calcola profili, non inoltra eventi e non disegna la zona London.

## Stato Corrente

```text
Runtime: DISABLED

Ancora implementato per riproducibilita' delle baseline:
- aggregazione volume per price level
- preview POC / VAH / VAL
- high / low London e state machine post-London
- [ZONE_READY] e zona/profile London grigi, visibili solo come contesto
```

Il tracker non decide entry, stop, target, PnL o filtri di big trade.

## Destinazione Del Refactor

Il refactor non e' incluso nel ciclo compression study. Quando sara' eseguito, il modulo sara' ridotto e rinominato `LondonTracker` con una responsabilita' esclusiva:

```text
Dato un timestamp UTC, identificare appartenenza, inizio e fine della sessione London.
```

Quella versione non dovra' calcolare profile, POC/VAH/VAL, high/low, volume per prezzo, state machine, grafica o forwarding trade. L'orchestrator inoltrera' direttamente al modello i dati necessari. Fino ad allora il codice presente resta invariato perche' funziona correttamente e il suo output non puo' influenzare candidati, trade o PnL.

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

## Profile Legacy Non Consumati

Durante London il tracker mantiene un profilo in costruzione:

```text
candele London -> price levels ATAS -> volume per prezzo -> POC/VAH/VAL preview
```

Valori pubblici ancora esposti dal tracker:

```csharp
LastPreviewPoc
LastPreviewVah
LastPreviewVal
```

Il modello studio non usa questi developing levels per generare entry: non esistono entry operative. I valori restano disponibili solo per log e per modelli futuri separati.

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

FabioOrderFlow.OnCumulativeTradesResponse, una finestra ATAS alla volta
-> BalanceZoneTracker.AppendHistoricalCumulativeTrades
-> LondonMeanReversionModel.AppendHistoricalCumulativeTrades
-> dopo CUM_TRADES_COMPLETE: LondonMeanReversionModel.ProcessHistoricalPositions
```

## Stato Post-London Legacy

Il tracker mantiene anche una state machine utile a modelli futuri/post-London:

```text
NoZone -> BuildingSessionProfile -> BalanceReady -> BreakoutPending -> OutOfBalance
```

Per FabioCompressionStudy il tracker serve oggi alla sessione e al forwarding corrente. La logica studio non dipende dalla value area preview della London corrente ne' dagli altri output profile legacy.

## Visual

Il tracker disegna la zona London grigia, POC, VAH e VAL (`DrawBalanceProfileVisuals=true`) come contesto della sessione. Non e' un trigger e non influenza `FabioCompressionStudy`. `DynamicCompression` turchese e i candidati colorati restano disegnati dal modello studio e hanno semantica distinta.

## Validazione

Stato verificato al reload 2026-07-11:

```text
[ZONE_READY] continua ad apparire per le sessioni complete.
Il tracker inoltra barre/trade e lo studio storico completa senza errori.
POC/VAH/VAL e state machine non sono consumati da FabioCompressionStudy.
```

Il futuro `LondonTracker` sara' sano se riconosce correttamente inizio, fine e appartenenza alla sessione London, inclusi i cambi DST.
