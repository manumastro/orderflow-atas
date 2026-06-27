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

Il modello quindi legge una balance in costruzione durante London, aspetta un'escursione fuori `VAH/VAL`, richiede una rejection back inside, poi accetta una entry base quando arriva un cumulative trade nella value area tra edge e POC con spazio sufficiente verso Target2. `POC_RECLAIM`/`POC_LOSS` non e' prerequisito rigido per aprire la base: e' conferma di accettazione, gestione/risk-free, e filtro per continuation/scale-in.

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

3. Wait for value re-entry big trade
   - long: cumulative trade Buy >= 10 contratti tra `VAL` e `POC`
   - short: cumulative trade Sell >= 10 contratti tra `VAH` e `POC`
   - il trade deve arrivare dopo la rejection, entro 20 minuti operativi
   - lo study resta a 1 ora per analisi dei setup tardivi
   - non serve aspettare POC reclaim/loss: il POC diventa conferma/gestione, non prerequisito rigido
   - `RewardToTarget2 / Risk >= 1.0`, con risk operativo dinamico

4. POC acceptance / management
   - long: `POC_RECLAIM_AFTER_LOW_REJECTION` conferma forza e abilita studio continuation
   - short: `POC_LOSS_AFTER_HIGH_REJECTION` conferma forza e abilita studio continuation
   - quando il prezzo arriva al POC, lo stop viene protetto

5. Register entry
   - entry price = Lastprice del cumulative trade
   - stop originale = high/low della rejection +/- 2 tick
   - stop operativo = stop originale cappato a massimo `0.5 * (VAH - VAL)` di rischio quando lo stop tecnico e' troppo lontano
   - Target1 = POC solo per protezione stop
   - Target2 = VAH per long / VAL per short

6. Management operativo
   - ManagementMode base = `VALUE_REENTRY_TARGET2`
   - quando il trade raggiunge il POC, lo stop viene protetto almeno a breakeven
   - nel backfill storico lo stop protetto diventa valido dalla barra successiva al POC, per evitare assunzioni intrabar false
   - exit operative: `TARGET2_HIT`, `PROTECTED_STOP_HIT`, `STOP_HIT`, `LONDON_CLOSE`

7. Scale-in operativo Fabio-style
   - ManagementMode add-on = `VALUE_REENTRY_TARGET2_SCALE_IN_EXPAND25`
   - massimo 1 add-on per setup
   - solo dopo che la base ha raggiunto POC/risk-free
   - add-on deve rispettare la stessa entry value-reentry e `RR_T2 >= 1.0`
   - dopo risk-free, il prezzo deve aver espanso almeno il 25% del tratto `POC -> Target2`

8. Study leggero
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
5. Dopo la rejection arriva un cumulative trade `Buy` con volume `>= 10`.
6. Il trade e' dentro value tra `VAL` e `POC`.
7. `RewardToTarget2 / Risk >= 1.0`.
8. `POC_RECLAIM_AFTER_LOW_REJECTION` resta conferma/gestione e abilita study continuation/scale-in, ma non blocca la base.

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
5. Dopo la rejection arriva un cumulative trade `Sell` con volume `>= 10`.
6. Il trade e' dentro value tra `VAH` e `POC`.
7. `RewardToTarget2 / Risk >= 1.0`.
8. `POC_LOSS_AFTER_HIGH_REJECTION` resta conferma/gestione e abilita study continuation/scale-in, ma non blocca la base.

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
DynamicStopMaxValueAreaRiskPct = 0.50m
AggressionTimeoutSeconds = 3600      // study / setup window
OperationalEntryTimeoutSeconds = 1200 // entry operative base
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
[DAY_STUDY_CANDIDATE_ENTRY] candidate entry alternative con risk/reward, MFE/MAE, outcome POC e Target2; include `TriggerAtEntry` per distinguere informazione live da trigger finale post-entry
[DAY_STUDY_DYNAMIC_STOP_CANDIDATE] studio non operativo degli stop alternativi per value-reentry: RR, outcome protetto, bucket temporale e riduzione rischio; include `TriggerAtEntry`
[DAY_STUDY_SCALE_IN_SUMMARY] simulazione scale-in Fabio-style per setup, solo dopo POC/risk-free della prima entry; ora usa stop/RR dinamici operativi e timeout base 20 minuti
[DAY_STUDY_SCALE_IN_CANDIDATE] add-on candidate successivi alla prima entry risk-free, con RR/RMultiple dinamici
[DAY_STUDY_SCALE_PLAN] piani setup-level: base + max add-on, filtri volume/tempo/espansione post-POC, TotalPnL/TotalR/worst leg usando logica operativa corrente
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
VALUE_REENTRY_TARGET2_SCALE_IN_EXPAND25
```

---

## When Not To Trade

- Prima parte di London senza contesto sufficiente.
- Dopo 15:30 London per nuove entry.
- Strong trend con breakout e nessun rientro in value.
- Compressione strettissima con fakeout ripetuti e target POC troppo vicino.
- Quando non arrivano big trades nella direzione del ritorno verso POC.

---

## Current State

Ultimo reload analizzato dopo `c0aa526 Promote dynamic RR stop cap`:

```text
MR_ENTRY: 13
MR_EXIT: 13
MR_TARGET1_HIT: 9
MR_MISSED_OPPORTUNITY: 8
DAY_STUDY_ACTUAL_ENTRY: 13
DAY_STUDY_DYNAMIC_STOP_CANDIDATE: 3131
```

Risultato operativo su storico caricato:

```text
Totale entry: 13
Base: 11
Scale-in EXPAND25: 2
PnL: +397.74 punti
Net R: +6.18R
Exit: 4 TARGET2_HIT, 5 PROTECTED_STOP_HIT, 4 STOP_HIT
```

Entry operative ultimo reload:

```text
2026-06-22 09:35:30 Short Base  30773.00 TARGET2_HIT        +13.75  CAP_VALUE_WIDTH_50
2026-06-22 10:25:57 Long  Base  30712.25 PROTECTED_STOP_HIT +15.25  ORIGINAL_REJECTION
2026-06-22 10:44:31 Long  Scale 30727.75 PROTECTED_STOP_HIT +0.00   CAP_VALUE_WIDTH_50
2026-06-22 15:35:00 Short Base  30780.75 STOP_HIT           -55.88  CAP_VALUE_WIDTH_50
2026-06-22 15:45:00 Short Base  30788.50 STOP_HIT           -67.63  CAP_VALUE_WIDTH_50
2026-06-22 16:15:00 Short Base  30906.75 TARGET2_HIT        +131.75 ORIGINAL_REJECTION
2026-06-23 09:35:26 Long  Base  29996.00 STOP_HIT           -24.75  CAP_VALUE_WIDTH_50
2026-06-23 10:30:11 Long  Base  29809.75 PROTECTED_STOP_HIT +40.25  ORIGINAL_REJECTION
2026-06-23 15:35:00 Long  Base  29753.75 PROTECTED_STOP_HIT +45.75  CAP_VALUE_WIDTH_50
2026-06-23 15:45:00 Long  Scale 29749.75 TARGET2_HIT        +185.25 CAP_VALUE_WIDTH_50
2026-06-24 15:36:42 Long  Base  29773.50 PROTECTED_STOP_HIT +26.00  CAP_VALUE_WIDTH_50
2026-06-24 16:25:10 Long  Base  29675.00 TARGET2_HIT        +125.00 CAP_VALUE_WIDTH_50
2026-06-26 14:55:33 Long  Base  29334.75 STOP_HIT           -37.00  ORIGINAL_REJECTION
```

Reload notes:

```text
- 2026-06-24 16:25 long is now captured by dynamic RR: OriginalRisk 150.50 -> OperationalRisk 84.50, RR_T2 1.48, TARGET2_HIT +125.00.
- 2026-06-24 10:37 short is filtered as stale by OperationalEntryTimeoutSeconds=1200.
- Remaining weak entries are from NONE, HIGH_REJECTION_FOLLOW_THROUGH and LOW_REJECTION_FOLLOW_THROUGH. Next study now logs `TriggerAtEntry` so the cleanup can be causal: decide whether a weak trigger was actually known at entry time, not only after the setup completed.
```

Current known issues / next studies:

```text
1. NONE and LOW_REJECTION_FOLLOW_THROUGH are weak for operative entries:
   - LOW_REJECTION_FOLLOW_THROUGH study was strongly negative.
   - NONE should probably remain study-only.

2. Some follow-through entries without POC reclaim/loss restore trade frequency but add noise.
   Next filter candidate: allow base pre-POC only with stronger evidence, not simply any follow-through.

3. 2026-06-24 10:37 short was formally valid under the 1-hour window but qualitatively stale:
   - about 27 minutes after rejection
   - small volume
   - setup family failed repeatedly
   Operational response: base entries now require `OperationalEntryTimeoutSeconds = 1200`, so this family should be filtered.

4. 2026-06-24 16:25 long looked visually interesting but RR_T2 was below 1.0 because stop was the full rejection low:
   - Reward points were large.
   - Risk was larger because technical stop was very far.
   Operational response: RR now uses dynamic stop cap `0.5 * value width`; this makes the case eligible without lowering the static RR floor.

5. Dynamic stop study candidates are logged in `[DAY_STUDY_DYNAMIC_STOP_CANDIDATE]`:
   - `ORIGINAL_REJECTION`: stop tecnico della rejection
   - `VALUE_EDGE_2T`: `VAL - 2 tick` per long, `VAH + 2 tick` per short
   - `RECENT_SWING_AFTER_REJECTION_2T`: swing dopo la rejection fino all'entry, con buffer 2 tick
   - `POC_TRIGGER_BAR_2T`: low/high della barra di POC reclaim/loss, solo se gia' confermata prima del candidato
   - `CAP_VALUE_WIDTH_100`: stop cap a massimo 1 value-area width di rischio
   - `CAP_VALUE_WIDTH_50`: stop cap a massimo 0.5 value-area width di rischio
   - ogni piano logga `Risk`, `RR_T2`, `RiskReductionPct`, `RejectionAgeBucket`, outcome protetto e `RMultiple`
```

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
