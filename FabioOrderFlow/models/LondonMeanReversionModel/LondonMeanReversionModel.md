# London Mean Reversion Model

Modello attivo di `FabioOrderFlow`. `MR` significa `Mean Reversion`: tutti i tag `MR_*` sono log operativi del modello mean-reversion, live o replay storico della stessa logica. Implementa una versione live-first del modello mean reverting descritto da Fabio Valentino per London sugli indici: mercato in consolidamento, falsa escursione fuori value area, rientro in balance e conferma tramite cumulative big trades.

## Sintesi Strategica

Fabio distingue due modelli:

```text
Trend following: mercato out of balance, tipicamente New York, target verso nuova/precedente area di balance.
Mean reversion: mercato in balance/compressione, tipicamente London, target verso bulk of auction/POC.
```

Questo file documenta solo il secondo.

Riferimenti transcript principali:

```text
17:01-19:56   London: fakeout, mean reversion, target POC, big trades, break-even
42:56-44:45   modello mean reverting: non primo swing, target bulk of auction, stop stretto
46:11-48:28   target probabile, consolidamento, delta per price level
1:08:43-1:14:31 esempio London: non accettazione sotto value, big trades, secondo drive
2:21:07-2:23:38 parziali, break-even, riuso profitto per nuovi trade
```

Tesi del modello:

- durante London sugli indici e' frequente vedere `out of balance -> back inside balance`;
- il primo spike fuori value area non va anticipato;
- dopo la falsa rottura serve una conferma di rientro e partecipazione dei big players;
- POC/bulk of auction e' il target a probabilita' piu' alta;
- VAH/VAL opposta e' estensione, non il target primario;
- se il trade e' sbagliato deve fallire presto, con stop piccolo;
- dopo POC il rischio va tolto o ridotto drasticamente.

## Fonte E Implementazione

```text
Transcript Fabio: transcription.txt
Codice:          LondonMeanReversionModel*.cs partial, separati per responsabilita'
Livelli:         BalanceZoneTracker.LastPreviewPoc/Vah/Val
Entry trigger:   ATAS CumulativeTrade live/storici
```

Il codice non e' una trascrizione discrezionale completa del trading live di Fabio. E' un modello meccanico che codifica una parte verificabile del playbook:

```text
profilo London dinamico -> sweep/reclaim -> big trade coerente -> POC partial -> runner protetto
```

Mappa file principali:

```text
LondonMeanReversionModel.cs                 orchestratore/stato/entrypoint ATAS
LondonMeanReversionModel.Config.cs          parametri core
LondonMeanReversionModel.Types.cs           tipi interni e record study
LondonMeanReversionModel.Session.cs         London session, time helper, bar lookup
LondonMeanReversionModel.Setups.cs          sweep/rejection/setup creation
LondonMeanReversionModel.Entry.cs           entry validation, RR, duplicate guard, CreatePosition
LondonMeanReversionModel.DelayedReclaim.cs  delayed reclaim causale
LondonMeanReversionModel.Positions.cs       gestione posizioni, POC partial, exit, PnL
LondonMeanReversionModel.Historical.cs      historical replay/reload flow
LondonMeanReversionModel.Live.cs            live heartbeat e diagnostiche live
LondonMeanReversionModel.Study*.cs          study/debug storico e outcome simulati
LondonMeanReversionModel.Logging.cs         log storico/day log
LondonMeanReversionModel.Diagnostics.cs     diagnostiche setup/trade/bar
```

Mappa flusso principale:

```text
OnNewSessionHigh/Low                 crea setup sweep + rejection
OnBarUpdate                          aggiorna snapshot, delayed reclaim, trigger study, posizioni
OnLiveCumulativeTrade                filtra/deduplica cumulative trades live
OnHistoricalCumulativeTrades         prepara backfill e study storico
ProcessHistoricalPositions           processa snapshot, trade storici, exit e day logs
ProcessAggressionTrade               ordine decisionale: delayed reclaim, scale-in, base entry
TryProcessDelayedReclaimEntry        delayed reclaim operativo causale
CreatePosition                       crea posizione o scarta duplicato base
ProtectStopAfterTarget1              POC partial + protezione runner
ClosePosition                        MR_EXIT con PnL blended
```

## Sessione E Timeframe

```text
Sessione profilo:        London 08:00-16:00
Nuove entry operative:   London 08:00-16:00
Gestione posizioni:      fino a London 16:00
Timeframe chart:         M5 consigliato
Trigger order flow:      CumulativeTrade ATAS
```

Le conversioni passano da `MarketTimeZones`; in estate London 08:00-16:00 corrisponde a Italy 09:00-17:00.

## Livelli Di Mercato

`BalanceZoneTracker` costruisce durante London un volume profile dinamico sui price levels ATAS.

```text
POC = prezzo con volume massimo
VAH = limite alto della value area 70%
VAL = limite basso della value area 70%
```

Il modello usa i livelli preview del momento, non solo i livelli congelati a fine sessione. Questo e' necessario per lavorare live durante London.

## Setup Base: Value Re-Entry

Long:

```text
1. price fa low sotto VAL
2. la candela chiude di nuovo sopra VAL
3. distanza close-low >= 10 tick
4. arriva cumulative Buy >= 10 contratti
5. prezzo del trade tra VAL e POC
6. trade entro 20 minuti dalla rejection
7. Reward/Risk verso Target2 >= 1.0
```

Short:

```text
1. price fa high sopra VAH
2. la candela chiude di nuovo sotto VAH
3. distanza high-close >= 10 tick
4. arriva cumulative Sell >= 10 contratti
5. prezzo del trade tra VAH e POC
6. trade entro 20 minuti dalla rejection
7. Reward/Risk verso Target2 >= 1.0
```

Il setup nasce da nuovi high/low London notificati dal tracker. In storico, se abilitato, il modello crea anche setup `HistoricalIntrabar` ricostruiti dai cumulative trades dentro la candela per avvicinare il timing live.

## Delayed Reclaim

Il delayed reclaim copre i casi in cui il mercato esce dalla value, poi rientra in modo piu' tardivo. Serve a non perdere reclaim buoni, ma deve restare causale e non usare lookahead.

Creazione candidato:

```text
Long:  close precedente sotto VAL, close corrente sopra VAL
Short: close precedente sopra VAH, close corrente sotto VAH
Stop:  estremo degli ultimi 6 bar +/- 2 tick
```

Entry delayed valida:

```text
trade entro 20 minuti dal reclaim
volume >= 10 * 5 = 50
prezzo tra edge e POC, inclusivo
same-direction volume > opposite-direction volume
max same bubble >= max opposite bubble
risk operativo <= 120 punti
RR verso Target2 >= 1.0
AcceptanceMode valido
```

`AcceptanceMode` ammessi:

```text
CONFIRMED_ACCEPTANCE          almeno 2 barre gia' chiuse accettate inside value
IMMEDIATE_DOMINANT_PRESSURE  entro 120s e maxSame >= 2 * maxOpposite
EARLY_ACCEPTED_PRESSURE      almeno 1 barra accettata, maxSame dominante, same/opposite >= 1.50
```

Non usare direttamente come filtro operativo diagnostiche future/study tipo `PostNetVolume15m` o `NextBarHolds`.

## Stop, Target E Gestione

Alla entry:

```text
Stop tecnico long  = rejection low - 2 tick
Stop tecnico short = rejection high + 2 tick
Stop operativo     = stop tecnico cappato a 0.5 * width(VAH-VAL) se troppo lontano
Target1            = POC
Target2 long       = VAH
Target2 short      = VAL
```

Gestione corrente:

```text
al POC: chiusura simulata 70%
runner: 30% verso Target2
stop runner dopo POC: breakeven entry
exit: TARGET2_HIT, PROTECTED_STOP_HIT, STOP_HIT, LONDON_CLOSE
```

Nota importante: Fabio nel transcript non definisce una percentuale fissa `70/30` per questo setup. Dice che nel mean reverting model il bulk of auction/POC e' il target a probabilita' piu' alta, mentre puntare direttamente all'estremo opposto della value area e' meno probabile. Il runner fisso e' quindi una scelta di studio/gestione corrente, non la versione piu' pura del metodo Fabio.

Ipotesi Fabio-style da validare:

```text
1. uscita completa o quasi completa al POC/bulk of auction;
2. rischio tolto quando il mercato conferma con nuovo breakout/accettazione, non meccanicamente sempre al primo touch;
3. estensione oltre POC solo con nuova informazione order-flow: big trades, CVD/delta coerente, assorbimento opposto fallito;
4. eventuale seconda operazione finanziata dal profitto, non runner passivo lasciato sempre aperto;
5. se il setup perde tempo o torna in compressione, prendere profitto e non restituire il movimento.
```

## Scale-In

Scale-in operativo corrente:

```text
ManagementMode = VALUE_REENTRY_TARGET2_SCALE_IN_EXPAND25
max add-on = 2
solo dopo base arrivata a POC/risk-free
entry add-on deve rispettare value re-entry, RR_T2 >= 1.0 e freshness
prezzo deve aver espanso almeno 25% del tratto POC -> Target2 dopo risk-free
```

Gli scale-in sono esclusi dal blocco duplicati base.

## Blocco Duplicati Base

Una nuova base entry non scale-in viene scartata se esiste gia' una posizione base aperta:

```text
stessa direzione
stesso giorno Italy
Target1/POC entro 4 punti
Target2/value edge entro 8 punti
```

Log:

```text
[MR_ENTRY_SKIPPED] Reason=DUPLICATE_BASE_POSITION
```

## Parametri Correnti

```text
MinAggressionVolume = 10
AggressionTimeoutSeconds = 3600              study/missed window
OperationalEntryTimeoutSeconds = 1200        entry operativa
RejectionThresholdTicks = 10
StopOffsetTicks = 2
LateCutoff = 16:00 London
MinRewardRiskToTarget2 = 1.0
DynamicStopMaxValueAreaRiskPct = 0.50
PocPartialExitPct = 0.70
RunnerExitPct = 0.30
ScaleInMinExpansionAfterRiskFreePct = 0.25
ScaleInMinRewardToTarget1Points = 4
MaxScaleInsPerSetup = 2
DelayedReclaimNarrativeMinBubbleMultiplier = 5
DelayedReclaimMinAcceptedBars = 2
DelayedReclaimImmediateMaxSeconds = 120
DelayedReclaimDominantBubbleMultiplier = 2
DelayedReclaimEarlyPressureVolumeRatio = 1.50
DelayedReclaimMaxOperationalRiskPoints = 120
DuplicateBasePositionPocTolerancePoints = 4
DuplicateBasePositionValueEdgeTolerancePoints = 8
EnableHistoricalIntrabarFromCumulativeTrades = false       disabilitato: era replay storico senza identico path live
OperationalCoreMeanReversionOnly = true
HistoricalStudyDebugDays = [2026-06-29, 2026-06-30, 2026-07-01, 2026-07-02, 2026-07-03, 2026-07-06, 2026-07-07]  day log/study profondo limitato ai 7 giorni chart sotto review
Daily historical logs                     attivi solo quando e' attivo historical study debug
HistoricalStudyDebugMarker = %APPDATA%/ATAS/Logs/FabioOrderFlow-enable-historical-study-debug.flag
DailyHistoricalDebugLogs = day log operativo minimo sempre attivo
```

## Live E Storico

Live:

```text
OnCumulativeTrade / OnUpdateCumulativeTrade
-> dedupe per Time + Direction + FirstPrice
-> valuta solo nuovi massimi di volume per la stessa chiave
-> EntryModel=FootprintCumulativeTradeLive
```

Storico:

```text
OnFinishRecalculate richiede cumulative trades ultimi 7 giorni effettivi
OnHistoricalCumulativeTrades salva tutti i trade e filtra volume >= 10
ProcessStoredHistoricalTrades valuta entry con lo stesso core di live: ProcessAggressionTrade
UpdateHistoricalPositionsWithTrade aggiorna posizioni con trade successivi
posizioni ancora aperte vengono chiuse a London close del giorno entry
```

Reload normale = operativo veloce: niente dump `[DAY_STUDY_*]` massivi.
Debug profondo = valorizzare nel codice la costante interna `HistoricalStudyDebugDays` con una o piu' date `yyyy-MM-dd`; il dump pesante viene limitato a quei giorni e deve essere deciso prima dell'inizializzazione del modulo.
In alternativa resta disponibile il marker `%APPDATA%/ATAS/Logs/FabioOrderFlow-enable-historical-study-debug.flag`; con lista giorni vuota il marker abilita lo study su tutti i giorni.

Se un giorno non ha entry/exit, prima ipotesi da verificare: `CUM_TRADES_LOOKBACK` non include la sessione London di quel giorno.

## Modalita' Operativa Corrente: Core Mean Reversion Only

Il modello operativo live/storico deve essere uno solo e coerente con la mean reversion London:

```text
ON  POC_RECLAIM_AFTER_LOW_REJECTION
ON  POC_LOSS_AFTER_HIGH_REJECTION
ON  DelayedReclaim esplicito e confermato
OFF entry normale con StudyTrigger=NONE
```

Il codice operativo non contiene piu' famiglie follow-through/seconda-gamba/auction. Quelle idee sono parcheggiate fuori da questo modello: se verranno riprese, dovranno nascere come modello separato con contratto, log e validazione propri.

## Log Operativi

Tag operativi reali del modello:

```text
[MR_LOG_CONTRACT]                contratto log: MR_* operativo/replay, DAY_STUDY_* studio
[MR_REPLAY_CONTRACT]             mapping live/replay/study dei SetupSource
[MR_OPERATIONAL_MODE]            stato core-only e trigger ammessi
[MR_SETUP_LONG]                  setup long da sweep sotto VAL + close back inside
[MR_SETUP_SHORT]                 setup short da sweep sopra VAH + close back inside
[LIVE_FLOW_HEARTBEAT]            heartbeat leggero: primo trade live valido, poi ogni 25 trade validi o almeno ogni 60s; include diagnostica setup/delayed quando attivi
[MR_HISTORICAL_TRADES]           cumulative trades storici filtrati
[MR_ENTRY]                       posizione creata
[MR_DELAYED_RECLAIM_SETUP]       candidato delayed reclaim
[MR_DELAYED_RECLAIM_ENTRY]       delayed realmente creata, include AcceptanceMode; pressione calcolata sui trade processati causalmente storico/live
[MR_ENTRY_SKIPPED]               entry scartata
[MR_TARGET1_HIT]                 POC raggiunto, stop runner protetto
[MR_PARTIAL_EXIT]                70% simulato chiuso al POC
[MR_MFE_UPDATE]                  nuovo massimo favorevole
[MR_EXIT]                        exit finale; PnL blended se POC raggiunto
[MR_MISSED_OPPORTUNITY]          setup non entrato con motivo, solo debug profondo
[MR_POC_TRIGGER]                 POC reclaim/loss trigger operativo/replay
```

Contratto live/storico/studio:

```text
MR_*                  log operativo. ExecutionMode=LIVE e' live; ExecutionMode=HISTORICAL_REPLAY e' replay storico della stessa logica operativa.
DAY_STUDY_*           log di studio/debug. Non genera entry operative.
PnL valido            solo [MR_EXIT].
Study outcome         puo' usare valutazioni ex-post e serve solo per ricerca.
StudyOnly=False       presente sui log MR_* operativi di setup/entry/exit.
MirrorsOperational    sui DAY_STUDY_ACTUAL_* indica copia giornaliera di un evento MR_* reale/replay.
```

Mapping `SetupSource`:

```text
BarClose                LIVE_SAME_BAR_UPDATE_PATH
DelayedReclaimAccepted  LIVE_SAME_DELAYED_RECLAIM_PATH
HistoricalIntrabar      HISTORICAL_ONLY_DISABLED_BY_LIVE_PARITY
PreviewRejectionStudy   STUDY_ONLY_NOT_TRADED
DelayedReclaimStudy     STUDY_ONLY_NOT_TRADED
```

Regola per fase 1/2: una nuova idea puo' essere studiata con `DAY_STUDY_*`, ma se deve influire sul risultato deve diventare subito `MR_*` con logica causale live/replay identica. In sviluppo, `ExecutionMode=HISTORICAL_REPLAY` e' il nostro ambiente live simulato.

Nota legacy: alcuni campi mantengono il nome `StudyTrigger` per compatibilita' parser/log; il campo equivalente chiaro e' `OperationalTrigger`.

Tag reload/studio principali:

```text
[CUM_TRADES_LOOKBACK]                  range storico richiesto ad ATAS
[CUM_TRADES_REQUEST]                   request ATAS inviata
[CUM_TRADES_RESPONSE]                  risposta ATAS ricevuta
[HISTORICAL_FLOW_TRADES_READY]         trade ricevuti e filtrati
[HISTORICAL_FLOW_PROCESS_START]        inizio processing storico
[HISTORICAL_RELOAD_PROGRESS]           avanzamento Start/Bars/Trades/Finish sempre attivo, visibile con grep PROGRESS
[HISTORICAL_STUDY_PROGRESS]            tag legacy non piu' usato nel reload pulito
[HISTORICAL_FLOW_FINISH]               processo storico completato
[DAY_DEBUG_START] / [DAY_DEBUG_FINISH] day log operativo minimo
[DAY_STUDY_ACTUAL_ENTRY]               copia study di entry reale, solo debug profondo
[DAY_STUDY_ACTUAL_EXIT]                copia study di exit reale, solo debug profondo
[DAY_STUDY_POC_MANAGEMENT]             confronto POC all-out / 70-30 / full runner protetto, solo debug profondo
[DAY_STUDY_BIG_TRADE]                  big trades London diagnostici, solo debug profondo e filtrabili per giorno
[DAY_STUDY_SETUP_BLOCKED]              barra setup-ready ma bloccata dal gating attuale
```

## Modelli Parcheggiati Fuori Dal Core

La semplificazione core-only ha rimosso dal codice operativo e dagli study interni le famiglie precedenti di follow-through, seconda gamba e auction continuation.

Motivo:

```text
- il 2026-06-30 quelle logiche hanno catturato profitti importanti;
- dopo il 2026-06-30 hanno generato loss e comportamento misto;
- mescolare mean reversion e continuation nello stesso modello rende il codice e la validazione ambigui.
```

Checkpoint utili:

```text
checkpoint-london-before-core-only  stato pre-semplificazione/study multi-day
london-core-only                    baseline operativa core-only
```

Per recuperare il tema 30/06 non riabilitare codice dentro `LondonMeanReversionModel`: creare un modello separato, per esempio `LondonAuctionContinuationModel`, con trigger, gestione, log e parser PnL dedicati.

## PnL E Parser

Per risultati storici usare solo `[MR_EXIT]`. `DAY_STUDY_ACTUAL_EXIT` duplica informazione study e non va sommato.

```bash
perl -ne 'next unless /\[MR_EXIT\]/; if(/, PnL=([-]?[0-9]+,[0-9]+), RunnerPnL=/){$p=$1;$d="?"; if($ARGV=~/day-(\d{4}-\d{2}-\d{2})\.log/){$d=$1} $p=~s/,/./; $s{$d}+=$p; $n{$d}++} END{for $d (sort keys %s){printf "%s exits=%d pnl=%.2f\n",$d,$n{$d},$s{$d}}}' "$APPDATA/ATAS/Logs/FabioOrderFlow-days"/FabioOrderFlow-day-2026-06-*.log
```

Snapshot persistente e confronto gestione POC:

```bash
python FabioOrderFlow/tools/snapshot_performance.py
python FabioOrderFlow/tools/study_poc_management.py --archive-logs
```

`snapshot_performance.py` confronta `current70_30`, `poc100` e `fullRunnerBE` usando solo `[MR_EXIT]`. `study_poc_management.py` richiede debug profondo attivo per avere day log completi (`DAY_STUDY_BIG_TRADE`, `DAY_STUDY_BAR`, `DAY_STUDY_POC_MANAGEMENT`) e testare regole causali post-POC: uscire 100% al POC salvo conferma runner entro una finestra definita. Se `HistoricalStudyDebugDays` contiene date nel codice, il dataset study profondo viene prodotto solo per quei giorni. Lista vuota + marker file significa debug su tutti i giorni, quindi per reload puliti rimuovere il marker.

## Cosa Non E' Ancora Codificato

Dal transcript restano aspetti discrezionali non pienamente meccanizzati:

```text
lettura completa di delta per price level come supply/demand reale
CVD come conferma/gestione avanzata
low-volume node su swing custom disegnato manualmente
news/macro/regime day filter
distinzione fine tra compressione buona e chop da evitare
parziali/live order execution reale invece di simulazione loggata
```

Questi punti possono guidare study futuri, ma non vanno descritti come regole operative gia' implementate.

## Checklist Per Agenti

1. Leggi `CHANGELOG-AGENT.md` prima di interpretare performance.
2. Controlla `CUM_TRADES_LOOKBACK` prima di dire che un giorno e' valido o mancante.
3. Dopo modifiche C#: build Release e deploy DLL.
4. Dopo reload: attendi `[HISTORICAL_FLOW_FINISH]`.
5. Per PnL: somma solo `[MR_EXIT]` nei day log.
6. Per gestione POC: usa snapshot e studio POC, archiviando i log del reload.
7. Verifica delayed reclaim con `AcceptanceMode` e `MR_ENTRY_SKIPPED`.
8. Aggiorna il changelog con decisione, reload e numeri essenziali.
