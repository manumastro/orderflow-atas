# London Mean Reversion Model

**Strategy:** mean reversion live durante London su value area dinamica  
**Market State:** balance/compression, fakeout, rientro verso POC  
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
- target oggettivo = POC della distribuzione;
- se il trade e' sbagliato deve essere sbagliato subito, con stop stretto.

Il modello quindi legge una balance in costruzione durante London, aspetta un'escursione fuori `VAH/VAL`, richiede una rejection back inside, e conferma l'entry solo con cumulative trade grande nella direzione del ritorno verso il POC.

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

3. Wait for big trade
   - long: cumulative trade Buy >= 20 contratti
   - short: cumulative trade Sell >= 20 contratti
   - il trade deve arrivare dopo la rejection, entro 1 ora
   - il prezzo del trade deve essere back inside value e prima del POC

4. Register entry
   - entry price = Lastprice del cumulative trade
   - stop = high/low della rejection +/- 2 tick
   - Target1 = POC
   - Target2 = VAH per long / VAL per short

5. Hybrid management operativo
   - se `StudyTrigger` e' `POC_RECLAIM_AFTER_LOW_REJECTION` o `POC_LOSS_AFTER_HIGH_REJECTION`, usa Target2
   - negli altri casi chiude al POC
   - quando un trade Target2 raggiunge il POC, lo stop viene protetto almeno a breakeven
   - nel backfill storico lo stop protetto diventa valido dalla barra successiva al POC, per evitare assunzioni intrabar false
   - se il mercato non arriva a Target2, l'uscita puo' essere `PROTECTED_STOP_HIT`

6. Track study parallelo
   - resta come confronto diagnostico
   - logga Target1=POC e Target2=VAH per long / VAL per short
   - logga trigger follow-through e POC reclaim/loss per confronto con versioni precedenti
```

---

## Long Setup

Condizioni:

1. `LastPreviewPoc/Vah/Val` validi.
2. Candle London fa nuovo low sotto `VAL`.
3. Close torna sopra `VAL`.
4. Distanza `Close - Low >= 10 tick`.
5. Dopo la rejection arriva un cumulative trade `Buy` con volume `>= 20`.
6. Il trade e' dentro value: `Lastprice >= VAL`.
7. Deve esserci spazio verso target: `Lastprice < POC`.

Livelli:

```text
Entry   = trade.Lastprice
Stop    = rejection.Low - 2 tick
Target1 = setup.POC
Target2 = setup.VAH solo se StudyTrigger=POC_RECLAIM_AFTER_LOW_REJECTION
```

---

## Short Setup

Condizioni:

1. `LastPreviewPoc/Vah/Val` validi.
2. Candle London fa nuovo high sopra `VAH`.
3. Close torna sotto `VAH`.
4. Distanza `High - Close >= 10 tick`.
5. Dopo la rejection arriva un cumulative trade `Sell` con volume `>= 20`.
6. Il trade e' dentro value: `Lastprice <= VAH`.
7. Deve esserci spazio verso target: `Lastprice > POC`.

Livelli:

```text
Entry   = trade.Lastprice
Stop    = rejection.High + 2 tick
Target1 = setup.POC
Target2 = setup.VAL solo se StudyTrigger=POC_LOSS_AFTER_HIGH_REJECTION
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
[MR_ENTRY]               entry confermata da big trade live/storico, include ManagementMode, FinalTarget, StudyTarget2 e StudyTrigger
[MR_MFE_UPDATE]          nuova massima escursione favorevole
[MR_TARGET1_HIT]         POC raggiunto su trade gestito verso Target2; stop protetto
[MR_EXIT]                uscita operativa a POC, Target2, stop, protected stop o London close
[MR_STUDY_TRIGGER]       follow-through o POC reclaim/loss osservato dopo rejection
[MR_STUDY_ENTRY]         entry study parallela alla entry operativa
[MR_STUDY_TARGET1_HIT]   POC raggiunto nello study
[MR_STUDY_CLOSE]         uscita study a Target2, stop o London close
[MR_MISSED_OPPORTUNITY]  setup con trigger utile ma senza entry valida nel backfill storico
[MR_EXTENDED_CUTOFF_OPPORTUNITY] entry valida solo estendendo studio fino a 16:00 London / 17:00 Italy
[MR_STUDY_PRICE_TOUCH_OPPORTUNITY] big trade scartato da LastPrice ma che attraversa la entry zone
[MR_STUDY_SETUP_BAR_AGGRESSION] big trade presente nella stessa barra della rejection, prima dell'inizio finestra corrente
[MR_STUDY_CONTINUATION_ENTRY] big trade oltre POC dopo reclaim/loss, candidato continuation verso Target2
Timeout entry           nessuna entry se il big trade arriva oltre 1 ora dalla rejection
```

Day-study dedicato per analisi manuale/agent:

```text
%APPDATA%\ATAS\Logs\FabioOrderFlow-study-2026-06-25.log

[DAY_STUDY_BAR]             ogni barra London del giorno studio con OHLC, volume, bid/ask/delta, POC/VAH/VAL preview e top price levels
[DAY_STUDY_SETUP]           ogni setup creato nel giorno studio
[DAY_STUDY_TRIGGER]         follow-through o POC reclaim/loss nel giorno studio
[DAY_STUDY_BIG_TRADE]       ogni cumulative trade >= MinAggressionVolume nella London del giorno studio, con relazione a profilo/setup
[DAY_STUDY_ACTUAL_ENTRY]    entry operative effettivamente prese nel giorno studio
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
POC_ONLY
HYBRID_TARGET2_AFTER_POC
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
