# FabioOrderFlow - Agent Changelog

Registro per agenti: baseline, decisioni operative, reload verificati, performance e timeline commit. La strategia completa sta in `models/LondonMeanReversionModel/LondonMeanReversionModel.md`.

## Baseline

Punto tecnico di ripartenza: `5f7d705` (`docs: complete rewrite of London Mean Reversion Model based on Fabio transcript`, 2026-06-27).

Da quel punto il modello e' stato trasformato in implementazione live-first ATAS con cumulative trades, historical replay/study, gestione POC, delayed reclaim e blocco duplicati.

## Cosa E' Stato Fatto Sulle Performance

```text
1. Validazione PnL corretta
   - PnL storico valido solo da [MR_EXIT].
   - DAY_STUDY_ACTUAL_EXIT e' copia study e non va sommato.

2. POC Option B
   - POC trattato come target principale ad alta probabilita'.
   - 70% chiuso simulato al POC, 30% runner verso Target2.
   - Stop runner spostato a breakeven dopo POC.
   - [MR_EXIT] ora logga PnL blended, RunnerPnL, FullRunnerPnL, RealizedPocPnL.

3. Delayed reclaim causale
   - Rimossi delayed tardivi/deboli che funzionavano solo con diagnostiche future.
   - AcceptanceMode ammessi: CONFIRMED_ACCEPTANCE, IMMEDIATE_DOMINANT_PRESSURE, EARLY_ACCEPTED_PRESSURE.
   - Risk cap delayed = 120 punti.

4. Duplicate base guard
   - Evita doppie posizioni base nello stesso contesto.
   - Non blocca scale-in ufficiali.
   - CreatePosition ritorna bool; se una delayed viene skippata non viene piu' loggata come entry.

5. Storico/live-first
   - Entry storiche basate su cumulative trades, non su sole chiusure bar.
   - Intrabar historical setup abilitati per avvicinare il comportamento live.
   - Log reload con CUM_TRADES_LOOKBACK e HISTORICAL_FLOW_FINISH.
```

## Update 2026-07-01 16:55

```text
Studio aggiunto:
- [FOLLOWTHROUGH_TARGET_DECISION_STUDY] per ogni exit storica FollowThroughReclaimContinuation.
- Tratta VAH/VAL come area decisionale Fabio-style: target veloce/risk-free, poi seconda gamba solo con nuova conferma.
- Misura pressione same/opposite a 60/120/300s, acceptance oltre VAH/VAL, MFE/MAE post decision area.
- Simula runner con stop dietro decision area, runner con stop dietro POC e prima re-entry entro 5m se acceptance + big trade coerente.
- Non modifica entry, target o gestione live.

Reload verificato 19:11:
- [HISTORICAL_FLOW_FINISH] ClosedPositions=27, OpenPositions=0, StoredTrades=1432921.
- PnL reale modello corrente da [MR_EXIT]: +349,04.
- FollowThroughReclaimContinuation reale: Long +54,50 su 3 exit, Short +65,49 su 11 exit, totale +119,99.
- Study post-target: 13 righe; CurrentPnL=+191,74; runner passivo stop decision area=+5,50; runner passivo stop POC=-10,00.
- ReentryCandidate=2: short 2026-06-30 14:25:33 = -15,75; long 2026-06-30 15:35:01 = +41,13; totale +25,38.
- Lettura: non lasciare correre sempre; la continuation piu' Fabio-style sembra seconda gamba selettiva. Serve distinguere il long buono 15:35 dal falso positivo short 14:25.
```

## Update 2026-07-01 15:55

```text
Decisione operativa:
- introdotta seconda famiglia entry nel core: FollowThroughReclaimContinuation.
- Serve a catturare casi Fabio-style in cui lo sweep fuori value e il reclaim dentro value avvengono su barre successive.
- Non usa finestra breve hardcoded: mantiene un contesto sweep per direzione/giorno e crea setup al reclaim successivo.

Contratto:
- Long: sweep sotto VAL con close ancora sotto/pari VAL, poi reclaim sopra VAL.
- Short: sweep sopra VAH con close ancora sopra/pari VAH, poi reclaim sotto VAH.
- Entry operativa in continuation zone: Long POC..VAH, Short POC..VAL.
- Entry, exit e PnL passano dal core standard ProcessAggressionTrade/CreatePosition.
- Outcome leggibili da [MR_ENTRY]/[MR_EXIT] su tutti i giorni del lookback, senza debug giornaliero.
- Fix immediato: setup FollowThroughReclaimContinuation creati solo dentro London session.
- Punto aperto: sul 2026-06-30 15:30 la entry long cattura TARGET2_HIT rapido a VAH (+54,50), ma non esplora ancora continuation estesa; management/target della nuova famiglia resta da studiare.

Debug/log:
- [HISTORICAL_STUDY_PROGRESS] ora copre Start/Bars/Trades/Finish, quindi grep PROGRESS basta per seguire lo stato reload.
- [DAY_STUDY_FOLLOWTHROUGH_RECLAIM] resta study diagnostico, non fonte PnL.
```

## Update 2026-06-30 20:37

```text
Decisione operativa:
- il debug storico pesante non deve richiedere dump di tutte le giornate;
- niente input ATAS per il debug giornaliero, perche' la scelta del giorno deve esistere gia' all'inizializzazione del modulo.

Implementazione:
- HistoricalStudyDebugDay = DateOnly? costante interna nel codice; null = debug spento
- se valorizzata a yyyy-MM-dd, il dump study pesante viene filtrato da ShouldDebugHistoricalDay;
- marker file storico resta supportato come fallback.

Uso:
- reload normale: HistoricalStudyDebugDay = null, day log minimi e reload veloce;
- debug 2026-06-30: impostare in codice HistoricalStudyDebugDay = new DateOnly(2026, 6, 30), rebuild, deploy, reload.

Validazione:
- build Release completata senza errori.
```

## Update 2026-06-30 20:12

```text
Decisione operativa:
- reload normale deve essere operativo e veloce, non study/debug massivo;
- niente nuovi input ATAS per debug;
- rimossi input online/replay/tick diagnostics dalla UI indicatore;
- live e storico continuano a condividere il core entry ProcessAggressionTrade.

Implementazione:
- HistoricalStudyDebug attivabile solo con marker file:
  %APPDATA%/ATAS/Logs/FabioOrderFlow-enable-historical-study-debug.flag
- senza marker: niente dump DAY_STUDY_* massivi, niente LogStudyCumulativeTrades, niente LogStudyBar, niente missed-opportunity study;
- con marker: riattiva day log completi e study storico profondo;
- day log operativo resta disponibile per MR_ENTRY/MR_EXIT storici.

Validazione:
- build Release completata senza errori.
```

## Update 2026-06-30 19:46

```text
Decisione operativa:
- rimosso cutoff nuove entry a 15:30 London;
- nuove entry consentite per tutta la sessione London, fino a 16:00 London.

Implementazione:
- IsLondonTradeAllowed ora usa fine sessione London 16:00 invece di 15:30.
- Documentazione modello allineata su London 08:00-16:00.

Deploy:
- build Release completata senza errori;
- DLL deployata in %APPDATA%/ATAS/Indicators/FabioOrderFlow.dll;
- hash sorgente/target coincidente dopo copia.

Validazione post-deploy ancora da fare in ATAS:
- reload indicatore;
- attendere [HISTORICAL_FLOW_FINISH];
- rileggere i day log per verificare nuove entry 15:30-16:00 London.
```

## Performance Verificata

Ultimo reload completo verificato: `2026-06-30 10:32`.

```text
EffectiveBeginItaly=2026-06-23 10:28:25
EndItaly=2026-06-30 10:28:25
AllTrades=1.529.135
AggressionTrades=29.200
ProcessDurationMs=220.877
TotalFlowDurationMs=225.504
CompletedHistoricalEntries=12
CompletedDelayedReclaimEntries=0
OpenPositions=0
ClosedPositions=18
PositionRecords=18
```

Nota: 23/06 e 30/06 sono parziali in questo reload. Il fix OpenPositions e' confermato: il vecchio `OpenPositions=19` era diagnostica fuorviante.

PnL, solo `[MR_EXIT]`:

```text
2026-06-23   exits=3   +93,64  parziale, inizia 10:28
2026-06-24   exits=4  +174,50
2026-06-25   exits=3   +15,23
2026-06-26   exits=3   +30,06
2026-06-29   exits=4   +62,18
2026-06-30   exits=1    +8,40  parziale
Totale reload           +384,01
```

Confronto gestione POC su tutto il reload:

```text
current70_30=+384,01
poc100=+408,54
fullRunnerBE=+326,75
Delta poc100 vs current=+24,53
```

Confronto solo giorni completi 24/25/26/29:

```text
current70_30=+281,97
poc100=+262,77
fullRunnerBE=+326,75
Delta poc100 vs current=-19,20
```

Distribuzione exit su tutto il reload:

```text
TARGET2_HIT          exits=3   +119,78
PROTECTED_STOP_HIT   exits=11  +193,58
STOP_HIT             exits=3    -51,25
LONDON_CLOSE         exits=1   +121,90
```

Confronti noti durante l'intervento:

```text
Dopo POC Option B prima di delayed/duplicate fixes:       -3,44
Dopo delayed filter prima del duplicate guard:           +386,47  inflato da duplicato 24/06
Dopo duplicate guard, reload 21:04:                      +263,57
Reload 23:31, prima snapshot POC:                        +378,01
Reload 10:32, DLL nuova:                                 +384,01
```

Usare sempre il reload completo piu' recente, controllando `CUM_TRADES_LOOKBACK`.

## Verifiche Critiche

```text
24/06 delayed long perdenti 15:54 / 16:00 / 16:12 eliminate.
24/06 duplicate delayed 16:21:56 scartata con MR_ENTRY_SKIPPED.
24/06 fix confermato: lo skip duplicato non produce piu' anche MR_DELAYED_RECLAIM_ENTRY.
25/06 long debole 16:11 eliminata.
25/06 short 15:05 mantenuta e chiusa con POC protection.
```

Dettaglio 25/06 dopo fix:

```text
09:25 short   +0,70
14:14 long    +2,80
15:05 short  +11,73
Totale        +15,23
```

Dettaglio 29/06 ultimo reload:

```text
09:11 long    +14,05 TARGET2_HIT
09:29 short    +3,15 PROTECTED_STOP_HIT
13:32 short   +66,98 TARGET2_HIT
15:30 long    -22,00 STOP_HIT
Totale        +62,18
```

## Coerenza Con Fabio / Punto Ottimale

Coerente:

```text
POC/bulk of auction trattato come presa principale, non come target secondario.
Il primo spike fuori value non viene comprato/venduto alla cieca.
La conferma usa cumulative big trades e rientro in value.
I delayed reclaim ora sono causali: niente filtri basati su futuro.
Dopo POC il rischio viene tolto con stop a breakeven sul runner.
Le doppie base entry nello stesso contesto sono bloccate.
```

Ancora non completamente coerente/discrezionale:

```text
Il codice non legge ancora delta per price level come Fabio farebbe manualmente.
CVD e supply/demand per livello non sono ancora filtro operativo.
Non c'e' ancora filtro macro/news/regime day.
Non c'e' ancora distinzione meccanica forte tra compressione buona e chop da evitare.
Le parziali sono simulate/loggate, non esecuzione ordini reale.
OpenPositions=19 era diagnostica fuorviante: `_activePositions.Count` contava anche posizioni chiuse. Fix non deployato ancora: il log ora separa OpenPositions/ClosedPositions/PositionRecords.
```

## POC Partial Fabio-Style

Lettura transcript:

```text
18:20        il target e' dove e' stato transatto il massimo volume
19:50        quando hai ragione, dopo movimento/breakout aggiuntivo, vai break-even
42:56-43:10 per il modello trend verso balance precedente: full position, non half+runner, perche' oltre il target la probabilita' cala
44:28-44:39 nel mean reverting model non puntare all'estremo opposto: vai al bulk of auction dove la probabilita' di ritorno a balance e' alta
1:06:04      essere dentro presto e chiudere presto
2:21:24      con contratti multipli Fabio puo' togliere meta' quando vede debolezza/rifiuto ed e' risk-free
2:29:45      prende il target e riapre con profitto se arriva conferma, invece di restare esposto nell'indecisione
```

Conclusione operativa provvisoria:

```text
70/30 al POC non e' una regola Fabio letterale.
La versione piu' Fabio-style per London mean reversion e' POC come uscita primaria piena o quasi piena.
Runner/estensione verso VAH/VAL va consentito solo con nuova conferma order-flow dopo POC, non sempre.
La seconda gamba e' piu' simile a re-entry/scale-in finanziato dal profitto che a runner passivo.
```

Da studiare prima di cambiare produzione:

```text
A. POC_ALL_OUT: 100% al POC.
B. POC_CORE_RUNNER_CONFIRMED: 70-90% al POC, runner solo se dopo POC ci sono big trades/CVD coerenti e niente assorbimento contrario.
C. CURRENT_70_30_ALWAYS: stato attuale, utile come baseline ma non Fabio puro.
```

Snapshot corrente `FabioOrderFlow/performance-snapshots/performance-2026-06-30_10-33-30.txt`:

```text
exits=18
current70_30=+384,01
poc100=+408,54
fullRunnerBE=+326,75
Delta poc100 vs current=+24,53
```

Lettura: sul reload intero, l'uscita piena al POC batte il 70/30 fisso perche' molti runner tornano a breakeven dopo avere raggiunto il bulk. Sui soli giorni completi 24/25/26/29, pero', il 70/30 batte POC all-out per via del 24/06. Quindi non passare meccanicamente a 100% POC: serve un criterio post-POC migliore.

Verifica nuovo codice:

```text
[HISTORICAL_FLOW_FINISH] confermato con OpenPositions=0, ClosedPositions=18, PositionRecords=18.
[DAY_STUDY_POC_MANAGEMENT] confermato: 18 righe, una per exit.
PostPocContinuationConfirmed attuale e' troppo grezzo: solo 2/18 True e non isola ancora bene il 24/06 runner grande.
```

Studio POC runner da day log completi:

```text
Comando: python FabioOrderFlow/tools/study_poc_management.py --archive-logs
Report:  FabioOrderFlow/performance-snapshots/poc-management-study-2026-06-30_11-47-51.txt
JSON:    FabioOrderFlow/performance-snapshots/poc-management-study-2026-06-30_11-47-51.json
Log archiviati: FabioOrderFlow/performance-snapshots/poc-management-study-2026-06-30_11-47-51-logs/
```

Metodo:

```text
Base: stessi trade reali dei day log.
Se regola post-POC e' falsa: simula POC_ALL_OUT.
Se regola post-POC e' vera: tiene la gestione corrente 70/30.
Regole causali: usano solo DAY_STUDY_BIG_TRADE e DAY_STUDY_BAR dopo Target1Time, entro finestra scelta.
```

Primo risultato, da trattare come ipotesi e non produzione:

```text
Giorni completi 24/25/26/29:
current70_30=+281,97
poc100=+262,75
fullRunnerBE=+326,75
PRESSURE_EXTENSION25 60s: kept=2/14, pnl=+320,20, +38,23 vs current, +57,45 vs poc100
```

Controllo `2026-06-30 19:29`:

```text
Nessun nuovo reload storico dopo 10:32.
FabioOrderFlow-live.log aggiornato fino a 19:29 con ONLINE_CUMULATIVE_TRADE.
Nessuna MR_ENTRY/MR_EXIT/MR_TARGET1_HIT live nel live log.
Snapshot rigenerato: FabioOrderFlow/performance-snapshots/performance-2026-06-30_19-30-49.txt
Studio POC rigenerato: FabioOrderFlow/performance-snapshots/poc-management-study-2026-06-30_19-30-50.txt
Numeri invariati: current70_30=+384,01, poc100=+408,54, fullRunnerBE=+326,75.
```

Interpretazione:

```text
Il candidato migliore tiene runner solo se entro 60s dal POC ci sono pressione same-direction e almeno 25% di estensione verso Target2.
Tiene il runner grande del 24/06 e il piccolo Target2 del 29/06.
Scarta molti runner che poi tornano a breakeven.
Sample troppo piccolo: validare su piu' reload/giorni prima di promuovere in produzione.
```

## Cosa Migliorare

Priorita' alta:

```text
1. Migliorare criterio post-POC: distinguere runner valido tipo 24/06 da runner che torna a breakeven.
2. Studiare POC Fabio-style: POC_ALL_OUT vs runner confermato vs 70/30 sempre.
3. Ridurre costo reload: 1,53M trade e daily debug rendono il reload ~3-4 minuti.
4. Aggiungere filtro regime/chop per evitare trade tardivi o compressioni non Fabio-like.
5. Rendere la gestione POC/parziale realmente eseguibile live, non solo simulata nei log.
6. Consolidare snapshot performance persistenti per baseline/reload diversi.
```

Priorita' media:

```text
1. Integrare CVD/delta per price level come conferma, senza lookahead.
2. Migliorare parser/report per confrontare automaticamente reload diversi.
3. Salvare snapshot performance per baseline invece di dipendere solo dai log correnti ATAS.
4. Verificare 2026-06-22 con range che includa davvero London.
```

## Comandi Minimi

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
grep "CUM_TRADES_LOOKBACK\|HISTORICAL_FLOW_FINISH" "$APPDATA/ATAS/Logs/FabioOrderFlow.log"
python FabioOrderFlow/tools/snapshot_performance.py
python FabioOrderFlow/tools/study_poc_management.py --archive-logs
```

Parser PnL corretto:

```bash
perl -ne 'next unless /\[MR_EXIT\]/; if(/, PnL=([-]?[0-9]+,[0-9]+), RunnerPnL=/){$p=$1;$d="?"; if($ARGV=~/day-(\d{4}-\d{2}-\d{2})\.log/){$d=$1} $p=~s/,/./; $s{$d}+=$p; $n{$d}++} END{for $d (sort keys %s){printf "%s exits=%d pnl=%.2f\n",$d,$n{$d},$s{$d}}}' "$APPDATA/ATAS/Logs/FabioOrderFlow-days"/FabioOrderFlow-day-2026-06-*.log
```

## Timeline Commit Completa

Branch: `main`. HEAD verificato: `b4d7548`. Totale: 211 commit.

```text
b4d7548  2026-06-29  Document reload completion checks
0c05cfa  2026-06-29  Add historical flow completion markers
b4c31dc  2026-06-29  Keep historical delayed reclaim candidates causal
ff57b74  2026-06-29  Use narrative bubble for delayed reclaim entries
b69cfcb  2026-06-29  Evaluate delayed reclaim readiness per trade
bc1f3fa  2026-06-29  Process historical trades after reclaim candidates
c79f7d5  2026-06-29  Preserve causal historical snapshots
803f96c  2026-06-29  Wire delayed reclaim historical candidates
a4e1865  2026-06-29  Promote delayed reclaim entries
883f745  2026-06-29  Clean delayed reclaim studies
942aee8  2026-06-29  Study delayed reclaim narrative shifts
e868790  2026-06-29  Study delayed reclaim confirmations
5f1db2e  2026-06-29  Study delayed reclaim setups
6728282  2026-06-29  Study stopped setup rearm rules
0d4e9ba  2026-06-29  Study preview continuation and stopped re-alerts
9358d0c  2026-06-28  Fallback historical exits to completed bars
a01fdd2  2026-06-28  Manage exits with all cumulative trades
43d3584  2026-06-28  Close historical positions on entry day
b11e83b  2026-06-28  Study preview rejections and tighten scale-ins
3f7f702  2026-06-28  Manage historical positions with cumulative trades
61f5d01  2026-06-28  Order daily historical setup logs before trades
a1e45af  2026-06-28  Add daily historical debug logs
eb72c66  2026-06-28  Keep historical events out of online logs
e83317c  2026-06-28  Reset ATAS logs on indicator init
a427146  2026-06-28  Separate FabioOrderFlow log streams
2e43af2  2026-06-28  Checkpoint historical intrabar study path
30f6462  2026-06-27  Normalize London MR documentation
47ee63b  2026-06-27  Allow two expand25 scale-ins
44b4c4a  2026-06-27  Align scale-in study with dynamic RR
168f51d  2026-06-27  Document dynamic RR reload results
c0aa526  2026-06-27  Promote dynamic RR stop cap
55ad7e4  2026-06-27  Add dynamic stop study diagnostics
ef2def7  2026-06-27  Document current London MR state
cd72f7a  2026-06-27  Allow causal pre-POC value re-entry entries
afccd34  2026-06-27  Require entries after POC trigger time
dfcc11c  2026-06-27  Promote expand25 scale-in to operational logic
b1095ce  2026-06-27  Add expansion-filtered scale plans
4c248dd  2026-06-27  Add setup-level scale plan study
5d5716b  2026-06-27  Add Fabio-style scale-in study
8155727  2026-06-27  Promote value re-entry target2 strategy
07192e6  2026-06-27  Add day study log parser
e758452  2026-06-27  Avoid duplicate day study summaries
3206491  2026-06-27  Constrain day study outcomes to study session
a1abb61  2026-06-27  Add dedicated London day study log
8e70b8e  2026-06-27  Lower London aggression volume threshold
41aaa31  2026-06-27  Study continuation entries after POC reclaim
24b100d  2026-06-27  Limit historical cumulative study to last seven days
55954b1  2026-06-27  Batch historical cumulative trade requests
87190fe  2026-06-27  Study setup-bar cumulative trade timing
21bf204  2026-06-27  Stabilize live cumulative trade update dedupe
ba3fa71  2026-06-27  Study cumulative trade entry-zone touches
d4e5f9a  2026-06-27  Study lower London aggression volume
87df64d  2026-06-27  Study extended London entry cutoff
5f13f20  2026-06-27  Log London mean reversion missed opportunities
5f72b20  2026-06-27  Make London mean reversion live-first hybrid
53a171b  2026-06-27  fix: optimize aggression checking - filter by timeout window first
a756c40  2026-06-27  feat: add historical position processing for exit tracking
aeb4622  2026-06-27  fix: disable setup expiry during historical calculation
7423cb6  2026-06-27  feat: integrate v3 LondonMeanReversionModule into main indicator
f9a1b86  2026-06-27  feat: complete refactor of LondonMeanReversionModule (v3)
5f7d705  2026-06-27  docs: complete rewrite of London Mean Reversion Model based on Fabio transcript
ed43fdd  2026-06-27  fix: CRITICAL - correct Long stop loss sign (was inverted)
eb1d1ab  2026-06-26  fix: tighten FootprintFirst cutoff to 15:30 London (30min buffer)
316e6ff  2026-06-26  fix: add London close filter to FootprintFirst entries
e114bd1  2026-06-26  fix: correct Long stop loss PnL calculation and disable PROFILE_PREVIEW
128d848  2026-06-26  feat: block all trading after London close (16:00 London time)
16c6bcf  2026-06-26  chore: disable PRE_CLOSE logging (NY + London)
52eec4b  2026-06-26  chore: disable NY_PROFILE_PREVIEW logging to reduce log noise
5653046  2026-06-26  feat: Add diagnostic logs for session extremes and trigger evaluation
02f25ca  2026-06-26  fix: Prevent PROFILE_PREVIEW tick spam during active trades
2b0d23f  2026-06-26  fix: Update report-trades.ps1 to parse historical log format
015a91d  2026-06-26  feat: Improve log system with historical flag and smart PROFILE_PREVIEW
3aac79b  2026-06-26  feat: Add UUID tracking and trade report tool
12bba8b  2026-06-26  fix: Increase timeout to 3600 seconds (1 hour)
72e1111  2026-06-26  fix: Increase timeout to 900 seconds (15 minutes)
4e94d80  2026-06-26  fix: Add 300 seconds timeout for live aggression detection
e2a2350  2026-06-26  fix: Reset state machine on new London session in all states
ac023ab  2026-06-26  docs: Simplify documentation - focus on intrabar timestamps
36c4f0f  2026-06-25  docs: Document historical vs live aggression detection
1f854ad  2026-06-25  fix: restore all missing MR module calls from pre-refactoring
2cc8d81  2026-06-25  fix: call MR module OnBarUpdate in BalanceZoneTracker
8fa21c3  2026-06-25  refactor: final structure - core=model, no README, flat directory
a9ffa3a  2026-06-25  refactor: core files at model level, not in modules/
342e5c8  2026-06-25  refactor: restructure to model-based organization
ccd721b  2026-06-25  docs: consolidate Modello-2 - remove duplicate directory
4a05fa3  2026-06-25  docs: update root AGENTS.md with new FabioOrderFlow structure
d37be7f  2026-06-25  docs: consolidate to essential structure - AGENTS.md + module docs
58d6ba8  2026-06-25  docs: add final refactoring summary
c07a36b  2026-06-25  docs: add HOME.md master document, archive old docs
41ad072  2026-06-25  docs: add documentation index and refactoring summary
8f35415  2026-06-25  docs: complete architectural documentation
94e7364  2026-06-25  refactor: Phase 3a cleanup - remove MR code from core tracker
ab4dc33  2026-06-25  refactor: complete MR extraction - Phase 2b DONE
53c2049  2026-06-25  refactor: extract MR methods to LondonMeanReversionModule (Phase 2a - module complete)
9427364  2026-06-25  refactor: create LondonMeanReversionModule facade (Phase 1)
78837b4  2026-06-25  checkpoint: before physical MR extraction - safe rollback point
d4171a8  2026-06-25  docs: add code region delimiters for Mean Reversion sections
942f517  2026-06-25  docs: update to neutral documentation + add code analysis
57ed4c0  2026-06-25  docs: add final refactoring completion summary
5937106  2026-06-25  refactor: reorganize documentation and module structure (Phase 1+2)
23025d3  2026-06-25  docs: add refactoring summary
120a2f1  2026-06-25  refactor: reorganize directories - Modello-1-TrendFollowing -> FabioOrderFlow
5db73fd  2026-06-25  refactor: rename FabioTrendFollowing -> FabioOrderFlow + add module parameters
7aec726  2026-06-25  docs: analisi architettura e chiarezza sessioni pre-refactoring
d11b7f5  2026-06-25  feat: exit management automatico + footprint-first opzionale per live
cd18207  2026-06-24  Align logs to Italy-first timezone
7b7b10f  2026-06-24  Centralize log reading docs
66a6056  2026-06-23  Fix: conferma breakout solo su barre chiuse
21bd101  2026-06-23  Debug: traccia outcome entry footprint MR
49f9660  2026-06-23  Debug: usa soglia footprint unica a 20
779318d  2026-06-23  Debug: soglie volume Fabio per footprint MR
f405085  2026-06-23  Debug: aggiunge entry footprint live MR
e75cc0e  2026-06-23  Debug: chiarisce entry footprint MR
949fb75  2026-06-23  Debug: log miss aggressione storica MR
08f0c4f  2026-06-23  Debug: filtra aggressione storica dopo sweep
88b78b4  2026-06-23  Debug: ricostruisce aggressione MR da cumulative trades
fbf252a  2026-06-23  Debug: log bubble dominante nei trigger MR
a915b7c  2026-06-23  Debug: torna a log unico FabioTrendFollowing
76f7940  2026-06-23  Debug: ripristina profile preview live
a71d05f  2026-06-23  Debug: log last bar vista in verbose
8a3302c  2026-06-23  Debug: aggiunge BarMode ai trigger mean reversion
38cc2d0  2026-06-23  Debug: sposta log profilo e drawing nel verbose
d5526b3  2026-06-23  Debug: separa log principali e verbose
f8f21fe  2026-06-23  Docs: spostato caso studio 23 giugno in documento dedicato
2580e14  2026-06-23  Docs: caso studio completo Modello 2 del 23 giugno
921e157  2026-06-23  Docs: documentato early trigger Modello 2 del 23 giugno
87b9383  2026-06-23  Debug: early trigger mean reversion dopo rejection
8350dcf  2026-06-23  Debug: log trigger mean reversion su reclaim/loss POC preview
eeb6a1f  2026-06-23  Debug: profile preview ogni 5 barre per tutta London
c08af9e  2026-06-22  Debug: log sessioni chart ATAS e timezone strumento
7e3846c  2026-06-22  Docs: fase iniziale Modello 2 fakeout mean reversion
c5632a8  2026-06-22  Debug: profile preview intraday per studio fakeout metodo 2
7794aa2  2026-06-22  Debug: log mirati per fakeout/rejection durante London
476188f  2026-06-22  Debug: aggiunti log di stato ogni 10 barre per capire perche non si crea la zona in tempo reale
9fdeb6f  2026-06-22  Add: linee VAH e VAL tratteggiate nelle balance zones
646982c  2026-06-22  Remove: rimosse linee out-of-balance (non necessarie)
0c51dca  2026-06-22  Fix: balance zone NON si estende durante out-of-balance + visual OOB separato
d89f58d  2026-06-22  Docs: consolidata documentazione hybrid approach in BalanceZoneTracker.md
a29ff24  2026-06-22  Docs: aggiornata documentazione con hybrid approach completato
877df9b  2026-06-22  Fix: hybrid approach - box visivo usa High/Low, VAH/VAL solo per breakout
96f97d8  2026-06-22  Debug massivo: log tutte le candele, calcoli POC/VA, clear log all'avvio
a39570d  2026-06-22  Debug: log completi simbolo, date, prezzi e copertura zone
da1d621  2026-06-22  Fix: linee POC non piu infinite + log debug prezzi
c83541c  2026-06-22  Fix: allineamento zone London con sessioni complete
88155da  2026-06-21  Fix: risolve zone estese all'infinito e log spam
5a4ac64  2026-06-21  Implementa rendering visivo balance zones
ec768b0  2026-06-21  Implementa logging funzionante con callback da wrapper
cafc708  2026-06-21  Aggiorna script deploy con timestamp e reminder restart ATAS
3ba21ec  2026-06-21  Fix: inizializza High/Low balance zone dalla prima candle London
cf52666  2026-06-21  Implementa Phase 1: BalanceZoneTracker
2162016  2026-06-21  Rinomina documenti moduli con nome modulo
e9069b5  2026-06-21  Riorganizza Modello 1 in documentazione modulare
3b74751  2026-06-21  Reset Modello 1 a base pulita
4eec42f  2026-06-21  Consolida documentazione Modello 1 in un unico file
08b6217  2026-06-21  Documento centrale BalanceZoneTracker
bde1185  2026-06-21  Research completa: Design e Best Practices per BalanceZoneTracker
5df7fa5  2026-06-21  Documento centrale: README.md completo per Modello 1
68dec9a  2026-06-21  Pivot strategico: abbandono Modello 2, focus su Modello 1
65eb1e1  2026-06-16  Phase 1: volume-profile candidate detection, log only, no rectangles
23d6726  2026-06-16  Strip FabioMeanReversion to skeleton; plan volume-profile detection step by step
a200c6a  2026-06-16  Add balance-zone detection analysis (volume-profile approach) and clean up temp watcher/log scripts
22f9efa  2026-06-16  Added verbose per-candle logging in OnCalculate: for every bar logs time, OHLC, range, body, delta + enhanced BAL decision logs with time. This allows reconstructing M5 (or M1) timeline from start of day to debug why zones are insignificant or not detected on lower TF. Example user zone 2026-06-15 10:45-15:25 should now be traceable in log.
ca62f1e  2026-06-16  Made detection fully dynamic: removed all 3 params (range ratio, min bars, impulse score) from UI and logic. Now relative/dynamic inside (rangeExp >=1.5 && bodyRatio>=0.5 for impulse; min 5 bars; ratio<=0.6) - no fixed numbers as Fabio doesn't use them. Full history scan already. Deployed.
8015bce  2026-06-16  Fix IndexOutOfRange in FindLastImpulse (GetCandle(i-1) when i=0). Added OnFinishRecalculate to close and draw rectangle for the last open historical zone at end of data. Now past zones including the final one should show as rectangles.
b85744c  2026-06-16  Made lookback dynamic: removed fixed CompressionLookback usage in FindLastImpulse (now always scans from bar 0 / full data availability). Removed the param from UI since not used for limit. Updated header comments to match current impl (rectangles for all historical past zones, full history scan). Redeployed.
439df68  2026-06-16  Added explicit Reference to System.Drawing in csproj to ensure runtime loading of Drawing types for rectangles without load exceptions.
8ba358e  2026-06-16  Fixed indicator loading error by changing base class back to Indicator (ExtendedIndicator may not be auto-discovered the same way by ATAS loader). Redeployed.
5a51869  2026-06-16  Major simplification per user request: use DrawingRectangle boxes (semi-transparent cyan) for historical consolidation zones instead of paintbars.
b07b4bd  2026-06-16  Improve FindCompressionStart to prefer a more recent 'dealing range' start if the full post-impulse range is large. This should allow detecting the current local balance zone even after some expansion after the impulse (as per 'new dealing range' in transcript). Helps with the stuck old compStart + high ratio seen in logs.
e7e68d4  2026-06-16  Add impulseEnd to the 'ratio too high' log line so we can see in logs which old impulse is being used.
2db7fdf  2026-06-16  Relaxed default CompressionRangeRatio to 0.65 (was 0.45) so that in trending conditions with moderate compressions the zone can still trigger and paint. User can still tune per chart.
072cf73  2026-06-16  Fix detection: FindLastImpulse now returns the MOST RECENT qualifying impulse instead of the highest scoring (often stale from start of trend). This makes compStart recent, compRange realistic, ratio can drop below threshold during actual compressions.
82a5acc  2026-06-16  Final robust tail-balance-log.bat: entire start command on a single physical line to eliminate any batch continuation/quoting crash.
8dc5165  2026-06-16  Improve tail-balance-log.bat: use 'start' to launch a clean separate PowerShell window. More robust when double-clicked from Explorer. Should fix crashes on open.
c50c5e0  2026-06-16  Fix tail-balance-log.bat: use single-line PowerShell command to avoid quoting/continuation crash on double-click. Added more defensive creation of log dir/file.
6d589e4  2026-06-16  Add simple tail-balance-log.bat to read the original ATAS log in real-time directly from source.
73ff51e  2026-06-16  Add easy launcher avvia-watcher.bat + improved instructions inside watch-balance-log.ps1
ee6f3bb  2026-06-16  Add watch-balance-log.ps1 (real-time file watcher / copy, no admin required).
99374d7  2026-06-16  Add create-log-symlink.ps1 helper + document real-time log symlink in project.
44718f2  2026-06-16  Update header with log file location for easy debugging.
e9d4899  2026-06-16  Add file + debug logging for zone detection (FabioBalanceZone.log) + better handling of multiple zones.
f120ec0  2026-06-16  Add extensive Debug logging for balance zone detection + improve zone highlighting.
1159ebb  2026-06-16  Fix: candles disappear when indicator activated.
d8b7330  2026-06-16  Ultra-simplify: ONLY the balance zone (paintbars + marker). No profile at all, no POC/VAH/VAL lines or values, no BuildProfile.
b7dfc4f  2026-06-16  Simplify FabioMeanReversion from scratch: ONLY balance/compression zone detection + profile + visuals.
01fb3fd  2026-06-16  Fix graphical bugs in Modello-2-MeanReversion (FabioMeanReversion.cs)
061f8ef  2026-06-16  feat: Step 1 - Compressione dinamica + Volume Profile
8e5caed  2026-06-15  feat: aggiornamento completo documento Mean Reversion in italiano
1da0133  2026-06-15  refactor(modello-2): scheletro vuoto - solo log avvio, niente sessioni
8633b8d  2026-06-15  refactor(modello-2): solo log avvio indicatore, rimuovi LONDON_START
83ca421  2026-06-15  chore: fix AGENTS.md duplicate line
c446d28  2026-06-15  revert(modello-2): ripristina Step 0 - solo sessione London
c99f8ec  2026-06-15  fix(modello-2): London fallback 08-17 chart time quando template senza London
1501656  2026-06-15  fix(modello-2): rimuovi paintbar che coprivano le candele
6112f8f  2026-06-15  fix(modello-2): per-bar profile paint, startup log, deploy Step1b
07d18db  2026-06-15  feat(modello-2): Step 1b - profile POC/VAH/VAL e stati balance
7090078  2026-06-15  chore: AGENTS Step 1a status
4f4b47a  2026-06-15  feat(modello-2): Step 1a - individuazione zona balance (compressione dinamica)
f0c755f  2026-06-15  docs(modello-2): Step 1 balance spec - compression profile, dinamico, big trade 20/30
56132ca  2026-06-15  docs(modello-2): Step 1 balance fedele a Fabio, metodo e correzione spec
8cc4f10  2026-06-15  refactor(modello-2): indicatore ridotto a solo sessione London
ef18ccc  2026-06-15  chore: rimuovi file temporanei agent-tools/terminals
a09f8e0  2026-06-15  feat(modello-2): Step 0 - sessione London dinamica via ATAS API
637318d  2026-06-15  docs(modello-2): FabioMeanReversion.md conciso, pipeline 7 step
1c62356  2026-06-15  docs: AGENTS.md compatto (10 righe), rimossi README modelli
e9183a6  2026-06-15  docs: spec completa Modello 2 in md, stato implementazione in AGENTS.md
57906d1  2026-06-15  docs(modello-2): riscrivi FabioMeanReversion.md - Fase 1 zona di balance
60584b8  2026-06-14  feat: add file logging for CLI monitoring
55a9a8a  2026-06-14  feat: Fabio Valentino trading model - 2 ATAS indicators
aa1ad51  2026-06-14  docs: aggiunta documentazione ATAS completa + AGENTS.md
c7729a6  2026-06-13  Initial commit: ricerca order flow ATAS
```
