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

## Update 2026-07-07 18:45

```text
Chiarito contratto live/storico/studio prima di fase 1/2:
- Aggiunti startup log [MR_LOG_CONTRACT] e [MR_REPLAY_CONTRACT].
- I log MR_* di setup/entry/exit ora espongono ExecutionMode, LogicPath, StudyOnly, SetupSource e LiveParity.
- DAY_STUDY_ACTUAL_ENTRY/EXIT marcano MirrorsOperational=True per distinguere copia giornaliera da study-only puro.
- Contratto documentato: MR_* = operativo/live o historical replay; DAY_STUDY_* = ricerca/debug, non deve tradare; PnL valido solo da [MR_EXIT].
- Nessuna modifica a parametri, entry, exit o gestione posizione.
- Build Release: 0 warning / 0 errori.
```

## Update 2026-07-07 18:20

```text
Reload post-cleanup validato e refactor partial completato:
- Reload ATAS dopo cleanup: [MR_OPERATIONAL_MODE] mostra CoreMeanReversionOnly=True e AllowedTriggers=POC_RECLAIM_AFTER_LOW_REJECTION|POC_LOSS_AFTER_HIGH_REJECTION|DELAYED_RECLAIM.
- [HISTORICAL_FLOW_FINISH] presente: StoredTrades=1.316.095, ClosedPositions=5, OpenPositions=0.
- PnL da [MR_EXIT]: 5 exit, totale +60,44.
  - NONE delayed reclaim: 2 trade, +65,81.
  - POC_LOSS_AFTER_HIGH_REJECTION: 2 trade, -17,77.
  - POC_RECLAIM_AFTER_LOW_REJECTION: 1 trade, +12,40.
- Nessun residuo log/code di FollowThrough/SecondLeg/FOLLOW_THROUGH/SECOND_LEG/MR_STUDY_CONTINUATION_ENTRY nei file modello/documentazione core.
- Refactor meccanico senza cambio comportamento: LondonMeanReversionModule trasformato in partial e separato in file per responsabilita' (Config, Types, Session, Setups, Entry, DelayedReclaim, Positions, Historical, Live, Logging, Diagnostics, Study, StudyOutcomes, ScaleStudy, Risk, Triggers).
- Build Release post-refactor completata con 0 warning / 0 errori e DLL deployata in %APPDATA%/ATAS/Indicators/FabioOrderFlow.dll.
```

## Update 2026-07-07 18:10

```text
Cleanup core-only post semplificazione:
- Rimosso dal codice LondonMeanReversionModel il layer follow-through / seconda gamba / auction continuation rimasto parcheggiato dopo core-only.
- Rimossi flag e contesti non piu' operativi: EnableOperationalFollowThrough*, EnableFollowThroughStudyLogs, FollowThroughSweepContext, PendingSecondLegAuction.
- Rimossi trigger/setup/log non piu' parte del modello: LOW/HIGH_REJECTION_FOLLOW_THROUGH, FOLLOW_THROUGH_RECLAIM_CONTINUATION, FOLLOW_THROUGH_SECOND_LEG_AUCTION, MR_STUDY_CONTINUATION_ENTRY e study post-target correlati.
- IsOperationalSetupEnabled ora ammette solo POC_RECLAIM_AFTER_LOW_REJECTION e POC_LOSS_AFTER_HIGH_REJECTION; delayed reclaim resta sul path dedicato.
- Documentazione modello aggiornata: il recupero dei profitti 30/06 andra' fatto in un modello separato, non reinserendo continuation nel core mean reversion.
- Build Release completata con 0 warning / 0 errori.
```

## Update 2026-07-07 17:20

```text
Checkpoint e semplificazione modello:
- Creato checkpoint git d876bf3 e tag checkpoint-london-before-core-only per tornare allo stato multi-day debug prima della semplificazione.
- Modalita' operativa portata a CoreMeanReversionOnly=true.
- Operativo ON solo per POC_RECLAIM_AFTER_LOW_REJECTION, POC_LOSS_AFTER_HIGH_REJECTION e delayed reclaim esplicito.
- Operativo OFF per LOW/HIGH_REJECTION_FOLLOW_THROUGH, FOLLOW_THROUGH_RECLAIM_CONTINUATION e FOLLOW_THROUGH_SECOND_LEG_AUCTION.
- Bloccate entry normali con StudyTrigger=NONE; resta consentito il delayed reclaim tramite path dedicato.
- Obiettivo: un solo modello coerente London mean reversion; eventuali modelli London alternativi andranno riattivati come moduli separati/study con contratto esplicito.
```

## Update 2026-07-07 16:55

```text
Debug storico esteso a multi-day chart:
- HistoricalStudyDebugDays impostato a 2026-06-29, 2026-06-30, 2026-07-01, 2026-07-02, 2026-07-03, 2026-07-06, 2026-07-07.
- Obiettivo: produrre day log profondi per i 7 giorni caricati sul chart, saltando solo weekend/non sessioni.
- Nota vincolo ATAS: l'ultimo reload disponibile prima del cambio mostrava CUM_TRADES_LOOKBACK EffectiveBeginItaly=2026-06-30 16:42:52, quindi 2026-06-29 e gran parte del 2026-06-30 potrebbero non avere cumulative trades storici completi anche se il chart parte dal 29/06.
- Nessuna logica entry/exit cambiata: solo ampliamento del perimetro debug/study.
```

## Update 2026-07-07 11:05

```text
Nuovo focus debug storico:
- HistoricalStudyDebugDays spostato a 2026-07-03, 2026-07-06, 2026-07-07.
- Obiettivo: studiare in profondita' il 03/07, il 06/07 e la sessione live di oggi, invece del vecchio cluster 01/02/03.
- Nessuna logica entry/exit cambiata in questo passaggio: e' solo rifocalizzazione del debug profondo/day logs.

Osservazione iniziale su oggi 2026-07-07:
- Il long base delle 10:04:14 (POC_RECLAIM_AFTER_LOW_REJECTION, entry 29646,25) ha preso il POC alle 10:04:17 e poi e' uscito a breakeven del runner alle 10:07:19 con [MR_EXIT] PROTECTED_STOP_HIT +2,63.
- Con la logica corrente questo e' atteso: il setup base mean reversion fa partial 70% al POC e protegge il runner a breakeven; non ha ancora una continuation dedicata oltre POC.
- Il possibile follow-through long successivo e' stato poi scartato alle 10:14:09 con [MR_FOLLOW_THROUGH_CONTINUATION_WEAK_ACCEPTANCE_EXPIRED], perche' il trade oltre POC aveva ProgressPct=0,05 sotto MinProgressPct=0,12.
- Da verificare con i nuovi day log se oggi serva una continuation long meno stretta o una gestione post-POC diversa.
```

## Update 2026-07-05 20:55

```text
Nuovo study mirato ultimi 3 giorni:
- HistoricalStudyDebugDays impostato a 2026-07-01, 2026-07-02, 2026-07-03.
- Obiettivo: produrre day log profondi solo per gli ultimi 3 giorni evento, con focus sul 2026-07-03.
- Study follow-through post-target resta spento; si riattivano solo i day log/debug storici per quei tre giorni.
```

## Update 2026-07-05 20:35

```text
Cleanup debug:
- HistoricalStudyDebugDays riportato a [] per reload/live puliti.
- Marker file FabioOrderFlow-enable-historical-study-debug.flag verificato assente in %APPDATA%/ATAS/Logs.
- Con questa configurazione [HISTORICAL_STUDY_DEBUG] deve risultare Enabled=False, DailyLogs=False, DebugDays=OFF al prossimo reload.
```

## Update 2026-07-02 19:10

```text
Reload validation:
- Reload storico completato: 1.392.921 trade processati, 16 posizioni chiuse, 0 aperte.
- Weak follow-through 2026-07-02 10:00:10 ora scade con [MR_FOLLOW_THROUGH_CONTINUATION_WEAK_ACCEPTANCE_EXPIRED] e non produce piu' la loss successiva.
- Delayed reclaim 2026-07-02 16:08 viene catturato nello storico: short 30198,75, exit protetta 30198,75, PnL +34,13.
- Trovato bug di pulizia daily log: DailyHistoricalLog scriveva anche giorni non inclusi in HistoricalStudyDebugDays quando chiamato direttamente da log operativi storici.
- Fix: DailyHistoricalLog ora applica ShouldDebugHistoricalDay(eventUtc), quindi con HistoricalStudyDebugDays=[2026-07-02] i file daily debug vengono prodotti solo per oggi.
```

## Update 2026-07-02 19:00

```text
Live hardening e debug oggi:
- Salvati log reference in FabioOrderFlow-live-reference-logs-20260702-190018/ prima del reload successivo.
- Review live-safe: altri usi di _lastHistoricalTrades letti risultano study-only o storici; il punto live-capable corretto era delayed reclaim pressure.
- Heartbeat live reso piu' chiaro: quando ci sono setup/candidati attivi aggiunge SetupDiagnostics e DelayedDiagnostics con motivo sintetico di mancata entry.
- Heartbeat ora viene scritto dopo ProcessAggressionTrade, cosi' diagnostica e contatori includono anche il trade corrente gia' processato.
- Debug profondo limitato a 2026-07-02 via HistoricalStudyDebugDays=[2026-07-02].
```

## Update 2026-07-02 17:20

```text
Live delayed reclaim pressure fix:
- Log live osservati senza reload della patch successiva: 9875 cumulative trade live validi fino alle 16:59:39 Italy, OpenPositions=0, PendingSecondLegAuction=0.
- Un setup live delayed short e' nato alle 16:00:00 (POC=30150, VAH=30263, VAL=30021,25), ma non ha aperto nonostante molti sell in zona.
- Causa trovata: GetDelayedReclaimPressureBeforeTime usava _lastHistoricalTrades quando lo storico era caricato; in live post-reload questo escludeva i trade live dal calcolo pressione delayed.
- Fix: pressione delayed calcolata su _processedAggressionTrades, cioe' feed causale storico/live gia' processato.
```

## Update 2026-07-02 15:45

```text
Follow-up reload post filtro acceptance:
- Il filtro 12% ha pulito lo stato live: ActiveSetups da 64 a 0/2, PendingSecondLegAuction=0.
- Lo storico e' passato a 18 exit, PnL +441,19; trigger FollowThroughReclaimContinuationShort ora +38,87.
- Il trade perso 2026-07-02 non entra piu' alle 10:00, ma il setup restava vivo e rientrava alle 10:16:57, ancora stop -33,50.
- Nuovo intervento live: se una trade coerente entra in continuation zone ma non raggiunge l'acceptance minima, il setup follow-through viene marcato Expired subito.
- Nuovo log: [MR_FOLLOW_THROUGH_CONTINUATION_WEAK_ACCEPTANCE_EXPIRED].
```

## Update 2026-07-02 15:25

```text
Intervento live su prima gamba follow-through:
- Problema osservato: FOLLOW_THROUGH_RECLAIM_CONTINUATION_SHORT e' quasi flat nello storico disponibile (+0,99 su 11 trade) e il trade perso 2026-07-02 10:00 era una entry short troppo vicina al POC.
- Aggiunto filtro dinamico di acceptance: per FollowThroughReclaimContinuation l'entry deve aver percorso almeno il 12% della distanza POC->VAH/VAL oltre il POC verso il target.
- Esempio trade filtrato: 2026-07-02 short entry 29880, POC 29884,50, VAL 29836,25; progress 4,50/48,25 = 9,3%, sotto soglia 12%.
- Seconda gamba FollowThroughSecondLegAuction invariata.
- Pulizia stato live: setup scaduti marcati Expired e pending second-leg oltre 5 minuti rimossi, cosi' LIVE_FLOW_HEARTBEAT non mostra piu' pending storici sporchi.
```

## Update 2026-07-02 13:55

```text
Live monitoring:
- Aggiunto [LIVE_FLOW_HEARTBEAT] per distinguere live assente da live presente senza setup.
- Emissione: primo cumulative trade live valido, poi ogni 25 trade validi o almeno ogni 60 secondi.
- Il heartbeat contiene EntryModel=FootprintCumulativeTradeLive, AcceptedTrades, trade time, direzione, prezzo, volume, active setups, posizioni aperte, pending second-leg.
- Routing: scritto sia nel log generale sia in FabioOrderFlow-live.log.
- Storico reload 2026-07-02: una entry short FollowThroughReclaimContinuation alle 10:00:10, exit STOP_HIT alle 10:05:09, PnL=-33,50.
```

## Update 2026-07-02 12:10

```text
Promozione live e cleanup:
- Promossa nel core solo la seconda gamba immediata NEW_AUCTION_ACCEPTED, non le forme alternative pullback-hold.
- Nuovo setup operativo: FollowThroughSecondLegAuction.
- Log operativi: [MR_SECOND_LEG_AUCTION_ARMED], [MR_FOLLOW_THROUGH_SECOND_LEG_AUCTION], [MR_SECOND_LEG_AUCTION_CONFIRMED], normali [MR_ENTRY]/[MR_EXIT].
- Trigger: FOLLOW_THROUGH_SECOND_LEG_AUCTION_LONG/SHORT.
- Entry causale: prima gamba FollowThroughReclaimContinuation chiude a TARGET2_HIT su VAH/VAL; pullback-hold su barra chiusa; entro 5 minuti trade coerente oltre decision area; volume forte relativo al decision window (rank >= 0,90 e >= 2x mediana window).
- Gestione live: no target futuro/final POC; stop dietro decision area cappato su distanza old POC/decision area; exit su stop o London close.
- Study follow-through post-target spenti di default con EnableFollowThroughStudyLogs=false.
- HistoricalStudyDebugDays ora vuoto: reload pulito senza dump multi-day, salvo marker file esterno.
- Fix post-reload: daily debug log non e' piu' forzato a true; viene acceso solo quando e' acceso historical study debug. ResetDailyHistoricalDebugLogs elimina comunque i vecchi day log per evitare grep sporchi.
- Progress reload separato dallo study: nuovo tag sempre attivo [HISTORICAL_RELOAD_PROGRESS] con Phase=Start/Bars/Trades/Finish, quindi `grep PROGRESS` resta utile anche con debug spento.
```

## Update 2026-07-02 11:45

```text
Studio aggiunto:
- [FOLLOWTHROUGH_ALTERNATIVE_SECOND_LEG_STUDY] per OpportunityReason=ALTERNATIVE_SECOND_LEG_FORM.
- Simula tre ingressi osservazionali: PullbackEntry, PocMigrationEntry, BestSameTradeEntry.
- Per ogni ingresso misura ReachedFinalPOC, PnlToFinalPOC, LondonClosePnL, MFE, MAE e BestAlternativeMode.
- Obiettivo: capire se le 5 forme alternative pullback-hold/POC migration sono realmente tradeabili, invece di limitarci alla re-entry immediata.
- Non modifica live/core.

Reload verificato 11:47:
- Alternative study prodotto 5 righe: 2026-06-26=1, 2026-07-01=4.
- BestAlternativeMode=PULLBACK_HOLD in 5/5 casi.
- PullbackEntry: 5 casi, PnlToFinalPOC totale=+38,00.
- PocMigrationEntry: 5 casi, PnlToFinalPOC totale=-351,50; conferma troppo tardiva.
- BestSameTradeEntry: 2 casi, PnlToFinalPOC totale=-5,00; meno affidabile del pullback-hold.
- Dettaglio: 2026-06-26 short pullback=-33,00; 2026-07-01 short pullback +10,00/+41,00/+10,00/+10,00.
- Lettura: le forme alternative esistono, ma sono molto meno forti del caso immediato 2026-06-30 long. Se diventano live, la forma piu' promettente e' pullback-hold verso final/developing POC, non POC migration tardiva.
```

## Update 2026-07-02 11:25

```text
Studio aggiunto:
- [FOLLOWTHROUGH_POST_TARGET_OPPORTUNITY_STUDY] per ogni follow-through che raggiunge la decision area VAH/VAL.
- Obiettivo: spiegare le 11 non-candidate su 13 casi post-target e capire se la seconda gamba e' rara o solo cercata troppo stretta.
- Classifica opportunity mode: IMMEDIATE_BIG_TRADE, PULLBACK_HOLD_AND_POC_MIGRATION, POC_MIGRATION_CONFIRMATION, BREAKOUT_ACCEPTANCE_NO_REENTRY, PRESSURE_EXTENSION_NO_STRUCTURE, MFE_ONLY_NO_CONFIRMATION, NO_SECOND_LEG.
- Misura first close beyond decision, pullback-hold, POC migration, best same-direction trade entro 15m, pressione same/opposite e reason di mancata candidatura.
- Non modifica live/core.

Reload verificato 11:35:
- Opportunity study prodotto 13 righe, come target decision study.
- Modalita': IMMEDIATE_BIG_TRADE=2, PULLBACK_HOLD_AND_POC_MIGRATION=7, POC_MIGRATION_CONFIRMATION=1, BREAKOUT_ACCEPTANCE_NO_REENTRY=2, NO_SECOND_LEG=1.
- Reason: ALTERNATIVE_SECOND_LEG_FORM=5, EXISTING_REENTRY_DETECTED=2, SAME_PRESSURE_NOT_DOMINANT=3, NO_POC_MIGRATION=2, NO_CLOSE_BEYOND_DECISION_AREA=1.
- Lettura: la seconda gamba non e' solo rara; la detection immediata era stretta. Ci sono 5 casi alternativi con pullback-hold/POC migration e pressione same dominante che meritano uno study auction dedicato.
```

## Update 2026-07-02 11:10

```text
Configurazione study:
- HistoricalStudyDebugDays esteso a 6 giorni disponibili nel lookback corrente: 2026-06-25, 2026-06-26, 2026-06-29, 2026-06-30, 2026-07-01, 2026-07-02.
- Obiettivo: produrre day study profondi anche sugli altri giorni per confrontare continuation/auction read oltre al caso 30/06.

Reload verificato 11:09:
- DebugDays corretti su tutti e 6 i giorni; day log pesanti prodotti per 25/26/29/30/01/02.
- [HISTORICAL_FLOW_FINISH] ClosedPositions=26, StoredTrades=1380942, PnL reale modello corrente=+283,84.
- FollowThrough reale: Long +54,50 su 3 exit; Short +0,99 su 11 exit.
- FOLLOWTHROUGH_TARGET_DECISION_STUDY: 13 righe; ReentryCandidate=2; MFE>=50 in 12/13; MFE>=100 in 10/13; avgMFE=228,33; avgMAE=205,79.
- FOLLOWTHROUGH_SECOND_LEG_AUCTION_STUDY: 2 righe, entrambe 2026-06-30; NEW_AUCTION_ACCEPTED=1, OPPOSITE_AUCTION_ACCEPTED=1.
- Nessun altro giorno genera ReentryCandidate con la logica attuale; molti casi hanno MFE post-target ma non nuova auction/re-entry causale.
```

## Update 2026-07-02 10:55

```text
Studio dinamico aggiunto:
- [FOLLOWTHROUGH_SECOND_LEG_AUCTION_STUDY] affianca il filtro statico con metriche relative di auction acceptance.
- Non usa soglie di ingresso live; misura rank volume re-entry, rapporto con mediana decision window, pressione same/opposite, tenuta old decision area, pullback vs impulso, volume costruito oltre decision area, velocita'/distanza migrazione POC.
- Produce AuctionScore continuo e AuctionRead qualitativo: NEW_AUCTION_ACCEPTED, WEAK_BREAK_NO_ACCEPTANCE, OPPOSITE_ABSORPTION, NO_POC_MIGRATION, OPPOSITE_AUCTION_ACCEPTED, MIXED_AUCTION.
- Obiettivo: capire come Fabio distinguerebbe nuova auction accettata da breakout debole senza fissare soglie statiche premature.

Reload verificato 10:55:
- Auction study prodotto 2 righe, entrambe 2026-06-30.
- Short 14:25:33: AuctionScore=0,45, AuctionRead=OPPOSITE_AUCTION_ACCEPTED, ReentryVolume=10, DayRank=0,32, PostSameShare=0,49, PullbackVsImpulse=1,11, HoldScore=0,42, WickHoldScore=0,39, BarsToPocMigration=12, TargetPnL=-298,50.
- Long 15:35:01: AuctionScore=0,89, AuctionRead=NEW_AUCTION_ACCEPTED, ReentryVolume=49, DayRank=0,96, DecisionRank=0,97, ReentryVsDecisionMedian=4,08, PostSameShare=0,59, PullbackVsImpulse=0,34, HoldScore=1,00, WickHoldScore=1,00, BarsToPocMigration=0, TargetPnL=+178,75.
- Lettura: la separazione dinamica conferma il pattern Fabio-style senza dipendere da una singola soglia statica.
```

## Update 2026-07-02 10:35

```text
Filtro study aggiunto:
- [FOLLOWTHROUGH_SECOND_LEG_FILTER_STUDY] applica la regola candidata alla seconda gamba, senza modificarla in live.
- Criteri: ReentryVolume >= 30; old decision area tiene senza close/wick back inside; MAE prime 3 barre <= 0,75R; developing POC migra nella direzione; target strutturale FINAL_POC raggiunto e profittevole.
- Output: PassesStructureFilter e FilterFailures.
- Obiettivo: confermare che il long 2026-06-30 15:35 passa e il falso short 14:25 viene scartato prima di promuovere la regola nel core.

Reload verificato 10:26:
- ClosedPositions=27, StoredTrades=1379750, PnL reale modello corrente=+314,84.
- Filter study prodotto 2 righe.
- Short 2026-06-30 14:25:33: PassesStructureFilter=False; failures=WEAK_REENTRY_VOLUME|OLD_DECISION_FAILED|INITIAL_MAE_TOO_HIGH|STRUCTURE_TARGET_NOT_PROFITABLE; StructureTargetPnL=-298,50.
- Long 2026-06-30 15:35:01: PassesStructureFilter=True; ReentryVolume=49; OldDecisionHolds=True; InitialMae=21,00 <= MaxInitialMae=30,84; POC migrated at 15:39:59; StructureTarget FINAL_POC=30350,00 reached at 15:59:59; StructureTargetPnL=+178,75.
- Lettura: la regola candidata distingue correttamente il falso positivo dallo scenario Fabio-style valido nel dataset corrente.
```

## Update 2026-07-02 10:05

```text
Studio aggiunto:
- [FOLLOWTHROUGH_SECOND_LEG_STRUCTURE_STUDY] per ogni ReentryCandidate=True dello study continuation.
- Misura seconda gamba come movimento verso nuova struttura/developing POC, non solo come estensione 1R.
- Campi: MFE/MAE dopo re-entry, tenuta vecchia decision area, primo POC migrato, FinalPOC/VAH/VAL entro London session, touch e PnL potenziale verso FinalPOC/VAH/VAL, LondonClosePnL.
- Non modifica live/core; serve a studiare casi come 2026-06-30 long 15:35 verso POC sviluppato area 30350.

Reload verificato 10:14:
- ClosedPositions=27, StoredTrades=1379640, PnL reale modello corrente=+314,84.
- FollowThrough reale: Long +54,50 su 3 exit; Short +31,99 su 12 exit.
- Second-leg structure study prodotto 2 righe, entrambe 2026-06-30.
- Short falso 14:25:33: ReentryPrice=30051,50, Volume=10, MFE=54,50, MAE=391,00, FinalPOC=30350,00, PnlToFinalPOC=-298,50, LondonClosePnL=-253,75.
- Long buono 15:35:01: ReentryPrice=30171,25, Volume=49, MFE=271,25, MAE=21,00, FinalPOC=30350,00, ReachedFinalPOC=True at 15:59:59, PnlToFinalPOC=+178,75, FinalVAH=30440,25, PnlToFinalVAH=+269,00, LondonClosePnL=+134,00.
- Lettura: il long conferma la tesi del target strutturale/developing POC; il filtro deve eliminare re-entry deboli con volume basso e MAE/ritorni dentro decision area elevati.
```

## Update 2026-07-02 09:20

```text
Studio storico multi-day:
- HistoricalStudyDebugDay singolo sostituito da HistoricalStudyDebugDays.
- Giorni attivi per debug profondo: 2026-06-29, 2026-06-30, 2026-07-01.
- ShouldDebugHistoricalDay ora filtra tramite HashSet<DateOnly>.
- Lista vuota mantiene compatibilita' con marker file: se il marker abilita lo study e la lista e' vuota, il debug vale per tutti i giorni.
- Obiettivo: studiare continuation e second-leg Fabio-style su piu' giornate prima di riorganizzare regole/documentazione.

Reload verificato 09:31:
- [HISTORICAL_FLOW_FINISH] ClosedPositions=10, OpenPositions=0, StoredTrades=547246.
- DebugDays=2026-06-29|2026-06-30|2026-07-01 configurati.
- CUM_TRADES_LOOKBACK BeginItaly=2026-06-30, quindi 29/06 non disponibile nel dataset.
- Day log pesanti prodotti: 2026-06-30 (1.4MB, 1873 DAY_STUDY lines), 2026-07-01 (1.3MB, 1862 DAY_STUDY lines).
- Logica multi-day funziona correttamente: processa tutti i giorni richiesti presenti nel lookback.
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
