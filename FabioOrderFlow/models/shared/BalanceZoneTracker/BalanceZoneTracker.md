# BalanceZoneTracker

Modulo shared per costruire e mantenere le reference di balance usate dai modelli.

Nel lavoro corrente serve soprattutto al `LondonMeanReversionModel`: durante London mantiene un volume profile dinamico e pubblica `LastPreviewPoc`, `LastPreviewVah`, `LastPreviewVal` per setup e target live.

---

## Responsabilita'

1. Riconoscere la sessione London con `MarketTimeZones`.
2. Aggregare i price levels delle candele nel profilo London.
3. Calcolare preview dinamico di `POC`, `VAH`, `VAL`.
4. Aggiornare la candela corrente senza duplicare volume.
5. Notificare al modello London i nuovi high/low di sessione.
6. Inoltrare cumulative trades live/storici al modello London.
7. A fine London congelare una `BalanceZone` completa per eventuali modelli post-London.

Non responsabilita':

- decidere entry/stop/target;
- filtrare big trades;
- gestire outcome;
- implementare logica specifica PostLondonImpulse.

---

## Sessioni

London:

```text
08:00-16:00 London local time
```

New York:

```text
09:30-16:00 New York local time
```

Le conversioni passano sempre da `MarketTimeZones`:

```csharp
MarketTimeZones.ToLondon(utcTime)
MarketTimeZones.ToItaly(utcTime)
MarketTimeZones.ToNewYork(utcTime)
```

---

## Ruolo Per London Mean Reversion

Durante London il tracker lavora in preview:

```text
candele London -> volume profile dinamico -> POC/VAH/VAL preview
```

Questi livelli sono esposti con:

```csharp
public decimal LastPreviewPoc { get; }
public decimal LastPreviewVah { get; }
public decimal LastPreviewVal { get; }
```

Il modello London usa questi livelli per:

- capire se un nuovo high rompe `VAH`;
- capire se un nuovo low rompe `VAL`;
- definire il target `POC` al momento del setup;
- validare che il big trade di entry sia rientrato dentro value.

---

## Volume Profile

Il profilo e' aggregato dai price levels ATAS:

```csharp
foreach (var level in candle.GetAllPriceLevels())
    profile[level.Price] += level.Volume;
```

In live ATAS puo' richiamare la stessa barra piu' volte mentre la candela si forma. Per evitare profili falsati, il tracker salva lo snapshot per bar:

```text
bar -> { price -> volume }
```

Quando lo stesso bar viene aggiornato:

1. sottrae il contributo precedente del bar;
2. legge il nuovo snapshot della candela;
3. aggiunge il nuovo contributo;
4. ricalcola il totale come somma degli snapshot attivi.

Questo mantiene `POC/VAH/VAL` vivi senza gonfiare artificialmente il volume.

---

## POC / VAH / VAL

POC:

```text
price level con volume massimo
```

Tie-break:

```text
se due livelli hanno stesso volume, scegliere il prezzo piu' basso
```

Value Area:

1. ordina i livelli per prezzo;
2. parte dal POC;
3. espande sopra/sotto scegliendo il lato con volume maggiore;
4. continua fino al 70% del volume;
5. `VAL` = livello piu' basso incluso;
6. `VAH` = livello piu' alto incluso.

La value area e' contigua.

---

## State Machine Post-London

Il tracker mantiene ancora una state machine utile ai modelli post-London:

```text
NO_ZONE
  -> BUILDING_SESSION_PROFILE
  -> BALANCE_READY
  -> BREAKOUT_PENDING
  -> OUT_OF_BALANCE
```

Per il modello London corrente, la parte importante e' la fase `BUILDING_SESSION_PROFILE`, perche' il modello lavora sulla value area preview durante London.

Per modelli futuri/post-London:

- `BALANCE_READY` significa London chiusa e livelli congelati;
- breakout post-London usa close fuori `VAH/VAL`;
- conferma breakout richiede 2 close consecutive fuori value area.

---

## Eventi Verso Il Modello London

Barre:

```text
UpdateLondonProfile()
-> OnNewSessionHigh(bar, candle, previousHigh)
-> OnNewSessionLow(bar, candle, previousLow)
```

Cumulative trades live:

```text
FabioOrderFlow.OnCumulativeTrade()
FabioOrderFlow.OnUpdateCumulativeTrade()
-> BalanceZoneTracker.OnLiveCumulativeTrade()
-> LondonMeanReversionModule.OnLiveCumulativeTrade()
```

Cumulative trades storici:

```text
FabioOrderFlow.OnCumulativeTradesResponse()
-> BalanceZoneTracker.OnHistoricalCumulativeTrades()
-> LondonMeanReversionModule.OnHistoricalCumulativeTrades()
```

---

## Visual

A fine London il tracker disegna:

- rettangolo High/Low della sessione;
- linea POC;
- linee tratteggiate VAH/VAL.

Il box visuale usa High/Low per coprire tutta la sessione. La business logic usa `VAH/VAL`.

---

## Log Principali

```text
[SESSION_START]
[SESSION_END]
[ZONE_READY]
[BREAKOUT_PENDING]
[BREAKOUT_CONFIRMED]
[OUT_OF_BALANCE]
[FALSE_BREAKOUT]
```

I log di entry/outcome London sono nel modello:

```text
[MR_SETUP_LONG]
[MR_SETUP_SHORT]
[MR_ENTRY]
[MR_MFE_UPDATE]
[MR_EXIT]
```

---

## Validazione

Il tracker e' corretto se:

1. `POC/VAH/VAL` preview cambiano durante London senza volume duplicato;
2. i nuovi high/low London arrivano al modello London;
3. i cumulative trades live e storici arrivano al modello London;
4. a fine London la balance zone viene congelata;
5. la build Release passa senza warning/errori.
