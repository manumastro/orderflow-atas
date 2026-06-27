# London Mean Reversion Model

**Strategy:** mean reversion live durante London su value area dinamica  
**Market State:** balance/compression, fakeout, POC reclaim/loss, rientro verso lato opposto della value area  
**Session:** London 08:00-15:30 London time per nuove entry, gestione fino a 16:00 London time  
**Timeframe operativo:** chart M5 consigliato; entry da cumulative big trades live/storici  
**Implementation:** live-first con backfill storico sul chart

---

## Core Concept

Dal transcript Fabio Valentino:

- durante London sugli indici il mercato tende spesso a fare `out of balance -> back inside balance`;
- non si prende il primo movimento fuori dalla value area;
- dopo il fakeout si aspetta il rientro dentro la balance;
- l'entry non e' solo price action: serve la mano dei big players, cioe' big trades/bubbles;
- POC e' il punto di accettazione/protezione; il target operativo migliore e' il lato opposto della value area quando c'e' reclaim/loss del POC;
- se il trade e' sbagliato deve essere sbagliato subito, con stop stretto.

Il modello quindi legge una balance in costruzione durante London, aspetta un'escursione fuori `VAH/VAL`, richiede una rejection back inside, poi accetta entry solo se il prezzo conferma `POC_RECLAIM`/`POC_LOSS` e arriva un cumulative trade nella value area tra edge e POC con spazio sufficiente verso Target2.

---

## Trading Flow

```text
1. Build London profile
   - aggrega i price levels delle candele London
   - calcola preview dinamico di POC, VAH, VAL
   - aggiorna lo snapshot della candela corrente senza duplicare volume

2. Detect fakeout/rejection
   - short: high rompe VAH, close torna sotto VAH
   - long: low rompe VAL, close torna sopra VAL
   - rejection minima: 10 tick dal punto estremo alla close

3. Confirm POC acceptance
   - long: serve `POC_RECLAIM_AFTER_LOW_REJECTION`
   - short: serve `POC_LOSS_AFTER_HIGH_REJECTION`
   - follow-through senza reclaim/loss non basta per l'entry operativa

4. Wait for value re-entry big trade
   - long: cumulative trade Buy >= 10 contratti tra `VAL` e `POC`
   - short: cumulative trade Sell >= 10 contratti tra `VAH` e `POC`
   - il trade deve arrivare dopo la rejection, entro 1 ora
   - `RewardToTarget2 / Risk >= 1.0`

5. Register entry
   - entry price = Lastprice del cumulative trade
   - stop = high/low della rejection +/- 2 tick
   - Target1 = POC solo per protezione stop
   - Target2 = VAH per long / VAL per short

6. Management operativo
   - ManagementMode = `VALUE_REENTRY_TARGET2`
   - quando il trade raggiunge il POC, lo stop viene protetto almeno a breakeven
   - nel backfill storico lo stop protetto diventa valido dalla barra successiva al POC, per evitare assunzioni intrabar false
   - exit operative: `TARGET2_HIT`, `PROTECTED_STOP_HIT`, `STOP_HIT`, `LONDON_CLOSE`

7. Study leggero
   - continuation oltre POC resta solo log study, non entry operativa
   - il file historical study aiuta a confrontare candidati su tutto lo storico caricato
```

---

## Long Setup

Condizioni:

1. `LastPreviewPoc/Vah/Val` validi.
2. Candle London fa nuovo low sotto `VAL`.
3. Close torna sopra `VAL`.
4. Distanza `Close - Low >= 10 tick`.
5. Dopo la rejection deve arrivare `POC_RECLAIM_AFTER_LOW_REJECTION`.
6. Dopo il reclaim arriva un cumulative trade `Buy` con volume `>= 10`.
7. Il trade e' dentro value tra `VAL` e `POC`.
8. `RewardToTarget2 / Risk >= 1.0`.

Livelli:

```text
Entry   = trade.Lastprice
Stop    = rejection.Low - 2 tick
Target1 = setup.POC per protezione stop
Target2 = setup.VAH operativo
```

---

## Short Setup

Condizioni:

1. `LastPreviewPoc/Vah/Val` validi.
2. Candle London fa nuovo high sopra `VAH`.
3. Close torna sotto `VAH`.
4. Distanza `High - Close >= 10 tick`.
5. Dopo la rejection deve arrivare `POC_LOSS_AFTER_HIGH_REJECTION`.
6. Dopo il POC loss arriva un cumulative trade `Sell` con volume `>= 10`.
7. Il trade e' dentro value tra `VAH` e `POC`.
8. `RewardToTarget2 / Risk >= 1.0`.

Livelli:

```text
Entry   = trade.Lastprice
Stop    = rejection.High + 2 tick
Target1 = setup.POC per protezione stop
Target2 = setup.VAL operativo
```

---

## Orari Operativi

Il codice usa timezone London:

```text
Profilo/setup London: 08:00-16:00 London time
Nuove entry: 08:00-15:30 London time
Gestione posizioni: fino a 16:00 London time
```

Con l'ora legale europea, in Italia corrisponde a:

```text
Profilo/setup London: 09:00-17:00 Italy time
Nuove entry: 09:00-16:30 Italy time
Gestione posizioni: fino a 17:00 Italy time
```

Quando cambia l'ora legale, la conversione resta gestita da `MarketTimeZones`.

---

## Live vs Storico

Il sistema deve funzionare prima in live.

Live:

- ATAS chiama `OnCumulativeTrade(CumulativeTrade trade)` quando nasce un big trade aggregato.
- ATAS chiama `OnUpdateCumulativeTrade(CumulativeTrade trade)` quando quel trade viene aggiornato.
- `FabioOrderFlow` inoltra entrambi a `BalanceZoneTracker.OnLiveCumulativeTrade()`.
- Il tracker inoltra a `LondonMeanReversionModule.OnLiveCumulativeTrade()`.
- Il modulo deduplica gli update dello stesso cumulative trade con una chiave stabile basata su tempo, direzione e `FirstPrice`.
- Il trade viene valutato quando il volume aggregato supera `MinAggressionVolume`; non sommiamo gli update, perche' il big trade e' un trigger e non un indicatore delta cumulativo.

Storico:

- dopo il ricalcolo, `OnFinishRecalculate()` richiede i cumulative trades degli ultimi 7 giorni del chart, limite imposto da ATAS per una singola request;
- `OnCumulativeTradesResponse()` inoltra i trade storici al modulo;
- i trade storici passano dalla stessa logica di entry del live;
- le posizioni storiche vengono processate dalla barra dell'entry in avanti, non da prima dell'entry.

Questo significa che il chart mostra come sarebbe andata la logica live sui dati caricati, senza avere un motore storico separato.

---

## Configuration Corrente

```csharp
MinAggressionVolume = 10m
MinRewardRiskToTarget2 = 1.0m
AggressionTimeoutSeconds = 3600
RejectionThresholdTicks = 10
StopOffsetTicks = 2
LateCutoffHour = 15
LateCutoffMinute = 30
```

Le soglie in tick usano `InstrumentInfo.TickSize`, quindi stop e rejection sono coerenti con lo strumento.

---

## Log Tags

```text
[MR_SETUP_LONG]          low sweep + rejection sopra VAL
[MR_SETUP_SHORT]         high sweep + rejection sotto VAH
[MR_HISTORICAL_TRADES]   backfill cumulative trades ricevuto
[MR_ENTRY]               entry value re-entry confermata da big trade live/storico, include ManagementMode, FinalTarget, StudyTrigger e RR
[MR_MFE_UPDATE]          nuova massima escursione favorevole
[MR_TARGET1_HIT]         POC raggiunto; stop protetto
[MR_EXIT]                uscita operativa a Target2, stop, protected stop o London close
[MR_STUDY_TRIGGER]       follow-through o POC reclaim/loss osservato dopo rejection
[MR_MISSED_OPPORTUNITY]  setup senza entry valida, con motivo esplicito
[MR_STUDY_CONTINUATION_ENTRY] big trade oltre POC dopo reclaim/loss, solo studio continuation
Timeout entry           nessuna entry se il big trade arriva oltre 1 ora dalla rejection
```

Day-study dedicato per analisi manuale/agent:

```text
%APPDATA%\ATAS\Logs\FabioOrderFlow-study-historical.log

[DAY_STUDY_BAR]             ogni barra London dello storico caricato con OHLC, volume, bid/ask/delta, POC/VAH/VAL preview e top price levels
[DAY_STUDY_SETUP]           ogni setup creato nello storico caricato
[DAY_STUDY_TRIGGER]         follow-through o POC reclaim/loss nello storico caricato
[DAY_STUDY_BIG_TRADE]       ogni cumulative trade >= MinAggressionVolume nella London storica, con relazione a profilo/setup
[DAY_STUDY_ACTUAL_ENTRY]    entry operative effettivamente prese
[DAY_STUDY_SETUP_SUMMARY]   riepilogo setup e numero candidati alternativi
[DAY_STUDY_CANDIDATE_ENTRY] candidate entry alternative con risk/reward, MFE/MAE, outcome POC e Target2
```

`EntryModel` distingue:

```text
FootprintCumulativeTradeLive
FootprintCumulativeTradeHistorical
```

`StudyTrigger` distingue:

```text
NONE
LOW_REJECTION_FOLLOW_THROUGH
HIGH_REJECTION_FOLLOW_THROUGH
POC_RECLAIM_AFTER_LOW_REJECTION
POC_LOSS_AFTER_HIGH_REJECTION
```

`ManagementMode` distingue:

```text
VALUE_REENTRY_TARGET2
```

---

## When Not To Trade

- Prima parte di London senza contesto sufficiente.
- Dopo 15:30 London per nuove entry.
- Strong trend con breakout e nessun rientro in value.
- Compressione strettissima con fakeout ripetuti e target POC troppo vicino.
- Quando non arrivano big trades nella direzione del ritorno verso POC.

---

## Current Files

```text
FabioOrderFlow/models/LondonMeanReversionModel/
├── LondonMeanReversionModel.cs
└── LondonMeanReversionModel.md
```

Non mantenere copie `.old` del modello nella stessa directory: il progetto include `../models/**/*.cs` e la directory deve avere una sola implementazione attiva.

---

## References

- `transcription.txt`, Fabio Valentino @ Chart Fanatics
- `ricerca-order-flow-completa.md`, sezione cumulative trades live/storici ATAS
- `docs/atas/guides/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.md`
