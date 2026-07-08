# CHANGELOG AGENT - FabioOrderFlow

## Decisione 2026-07-08 - Roadmap local profile / nuove entry

```text
Analisi salvata come potenziali passi successivi, non operativi.

Diagnostica:
- Manteniamo un solo marker [MR_PROFILE_CONTEXT].
- ProfileSource distingue CurrentLondonSessionProfile e LocalRotationProfile.
- Per ridurre ingombro il marker diventa ENTRY_ONLY.
- I profili restano agganciati ai setup per audit/no-entry, ma senza righe dedicate per ogni setup.

Interpretazione Fabio-style, non overfit sui soli dati:
- Location prima del trigger: il big trade serve solo dopo aver definito dove siamo rispetto alla value.
- Long con entry sotto LocalRotation VAL = fragile perche' il prezzo non ha ancora riaccettato la rotazione locale; Fabio aspetterebbe reclaim/acceptance o retest della value recuperata.
- Short con entry gia' sotto LocalRotation POC = fragile perche' non si sta piu' vendendo da premium/upper value ma nella meta' bassa della rotazione; se e' continuation va trattato come modello separato.

Nuove entry possibili:
- MR_RETEST_ENTRY: stesso setup previous reference, big trade come conferma, entry solo su retest della VAL/VAH recuperata; deve essere RetestOnly oppure Shadow, non doppia entry silenziosa.
- MR_LOCAL_PROFILE_ENTRY: possibile entry/modello separato basato su LocalRotationProfile; richiede contratto dedicato prima di live operativo.

Decisione operativa:
- Non attivare ancora filtri o nuove entry con PnL.
- Se si aggiungono subito, farlo prima come shadow/candidate live-equivalent, non come trade operativo, per proteggere la baseline london-ny-close-baseline.
```

## Implementazione 2026-07-08 - Reduce profile diagnostics clutter

```text
- [MR_PROFILE_CONTEXT] resta diagnostica unica, ma passa a ProfileDiagnosticsLevel=ENTRY_ONLY.
- Nessun cambio entry/exit/target/stop.
- Obiettivo: mantenere la diagnostica utile senza ingombrare il log con due righe per ogni setup.
```

## Reload 2026-07-08 13:15 - Unified profile diagnostics validated

```text
Reload dopo implementazione [MR_PROFILE_CONTEXT]:
- [MR_MODE] corretto: ProfileDiagnostics=CurrentLondonSessionProfile|LocalRotationProfile, ProfileDiagnosticsUse=DIAGNOSTIC_ONLY.
- [CUM_TRADES_RESPONSE] Count=1.249.803.
- [HISTORICAL_FLOW_FINISH]: Entries=20, ClosedPositions=20, OpenPositions=0, CompletedTrades=20.
- [MR_PROFILE_CONTEXT]: 342 totali = 171 CurrentLondonSessionProfile + 171 LocalRotationProfile.
- Per source/context: 151 SETUP + 20 ENTRY per ciascun ProfileSource.
- [MR_ACTIVE_PROFILE_CONTEXT]: 0, sostituito correttamente dal marker unico.
- [MR_BREAKEVEN]: 12.
- [MR_REPLAY_OPEN]: 0.
- PnL storico invariato da [MR_EXIT]: +634,25.

Caso guida 2026-07-06, entry:
- 10:00 long: LocalRotationProfile POC=29860,00 VAH=29863,75 VAL=29824,00; Target=29885,00 sopra VAH; exit -9,00.
- 15:35 short: LocalRotationProfile POC=29856,25 VAH=29895,00 VAL=29826,50; Target=29885,00 tra POC e VAH; exit +29,00.
- 15:50 short: LocalRotationProfile POC=29900,00 VAH=29936,75 VAL=29849,00; Target=29900,00 esattamente POC locale; exit -17,25.
- 16:02 short: LocalRotationProfile POC=29971,25 VAH=29985,00 VAL=29869,00; Entry=29948,25 sotto POC locale, Target=29900,00 tra VAL e POC; exit -39,50.

Conclusione:
- La diagnostica unica funziona e resta solo diagnostica.
- Sul 06/07 il LocalRotationProfile non conferma una semplice protezione "short contro VAL corrente" per il 15:50: quel trade era verso il POC locale e fallisce per whipsaw.
- Il 16:02 appare piu' fragile: entry gia' sotto POC della rotazione locale e target verso lower value, quindi possibile candidato studio per filtro futuro.
- Nessun filtro operativo va attivato ancora; servono piu' sessioni e confronto contro il 02/07 prima di trasformare la diagnostica in regola.
```

## Implementazione 2026-07-08 - Unified profile diagnostics + LocalRotationProfile

```text
Obiettivo:
- Separare meglio le diagnostiche intraday senza creare marker/log diversi e confusi.
- Aggiungere una diagnostica piu' vicina alla lettura Fabio avanzata del profilo su compressione/rotazione intraday.

Implementato:
- Nuovo marker unico [MR_PROFILE_CONTEXT].
- Ogni riga distingue la diagnostica con ProfileSource.
- ProfileSource=CurrentLondonSessionProfile: profilo broad da inizio London al setup.
- ProfileSource=LocalRotationProfile: profilo locale direzionale dalla rotazione/pivot piu' recente al setup.
- Il vecchio marker [MR_ACTIVE_PROFILE_CONTEXT] viene sostituito dal marker unico [MR_PROFILE_CONTEXT].
- Uso dichiarato: ProfileUse=DIAGNOSTIC_ONLY.
- Nessun blocco entry, nessun cambio target, nessun cambio PnL.

Validazione:
- Build Release OK, Avvisi 0, Errori 0.
- DLL deployata; da ricaricare in ATAS.
```

## Reload 2026-07-08 11:58 - Active London profile diagnostics validated

```text
Reload dopo implementazione [MR_ACTIVE_PROFILE_CONTEXT]:
- [MR_MODE] corretto: ActiveProfileDiagnostics=CurrentLondonSessionProfile_DIAGNOSTIC_ONLY.
- [CUM_TRADES_RESPONSE] Count=1.243.317.
- [HISTORICAL_FLOW_FINISH]: Entries=20, ClosedPositions=20, OpenPositions=0, CompletedTrades=20.
- [MR_ACTIVE_PROFILE_CONTEXT]: 171 totali = 151 SETUP + 20 ENTRY.
- [MR_BREAKEVEN]: 12.
- [MR_REPLAY_OPEN]: 0.
- PnL storico invariato da [MR_EXIT]: +634,25.

Caso guida 2026-07-06:
- Il profilo finale del 06/07, disponibile solo dopo la sessione, ha London VAL=29887,00 e Day VAL=29895,25.
- La diagnostica live-equivalente CurrentLondonSessionProfile al momento dei trade non mostra invece 29887/29895 come VAL.
- Entry short 15:35: ActivePOC=29860,00; ActiveVAH=29901,50; ActiveVAL=29831,00; TargetPOC=29885,00.
- Entry short 15:50: ActivePOC=29880,00; ActiveVAH=29922,25; ActiveVAL=29839,25; TargetPOC=29900,00.
- Entry short 16:02: ActivePOC=29880,00; ActiveVAH=29935,00; ActiveVAL=29833,50; TargetPOC=29900,00.

Conclusione:
- La lettura "VAL del giorno" vista a posteriori sul 06/07 usa il profilo finale e sarebbe lookahead se usata in live.
- Il CurrentLondonSessionProfile diagnostico conferma che quei target erano dentro la value corrente / vicino al POC-upper value, non sulla VAL corrente live-equivalente.
- Se si vuole una protection Fabio-style, il prossimo passo non e' un filtro statico ne' il profilo finale, ma un vero ActiveBalanceProfile/compression profile intraday da validare prima in diagnostica.
```

## Implementazione 2026-07-08 - Active London profile diagnostics

```text
Obiettivo:
- Studiare la lettura Fabio avanzata del profilo costruito durante London, senza modificare la baseline operativa.
- Caso guida: 2026-07-06, dove dopo il target short sul POC precedente il mercato ha risposto dall'area lower value della London corrente.

Implementato:
- Nuovo log [MR_ACTIVE_PROFILE_CONTEXT].
- Source diagnostica: CurrentLondonSessionProfile.
- Snapshot al momento del setup e dell'entry.
- Campi principali: ActivePOC, ActiveVAH, ActiveVAL, ActiveValueWidth, ActiveHigh/Low, CandidateTargetPOC, distanze target/entry da ActiveVAL/POC/VAH.
- Uso dichiarato: DIAGNOSTIC_ONLY.
- Nessun blocco entry, nessun cambio target, nessun cambio PnL.

Validazione:
- Build Release OK, Avvisi 0, Errori 0.
- Da ricaricare in ATAS e confrontare i nuovi log contro la baseline london-ny-close-baseline.
```

## Reload 2026-07-08 10:40 - NY close hold validated

```text
Reload dopo fix MaxHold=NEW_YORK_REGULAR_CLOSE_16:00:
- [MR_MODE] corretto: MaxHold=NEW_YORK_REGULAR_CLOSE_16:00.
- [CUM_TRADES_RESPONSE] Count=1.232.418.
- [MR_HISTORICAL_TRADES] EndItaly=2026-07-08 10:40:07.
- [HISTORICAL_FLOW_FINISH]: Entries=20, ClosedPositions=20, OpenPositions=0, CompletedTrades=20.
- [MR_BREAKEVEN]: 12.
- [MR_REPLAY_OPEN]: 0.
- ExitReason=NEW_YORK_CLOSE: 0.
- ExitReason=LONDON_CLOSE: 0.
- PnL storico da [MR_EXIT]: +634,25.

Breakdown:
- POC_TARGET_HIT: 9 trade, +913,50.
- STOP_HIT: 11 trade, -279,25.
- Per giorno entry: 2026-07-01 +100,50; 2026-07-02 +634,50; 2026-07-06 -36,75; 2026-07-08 -64,00.
- Per source: PreviousDayProfile 10 trade +314,75; PreviousLondonProfile 10 trade +319,50.
- BreakEvenActivated=True: 12 trade +913,50; False: 8 trade -279,25.

Conferme importanti:
- Il vecchio trade 2026-07-01 15:50 non va piu' overnight: ora chiude a POC_TARGET_HIT il 2026-07-01 17:31:51, +120,25.
- Il vecchio LONDON_CLOSE del 2026-07-08 non esiste piu': il trade 09:25 arriva a breakeven e chiude a 0,00 alle 10:07:50.
- Il live stop news-shock 2026-07-08 e' incluso nel replay come trade 09:34:58, STOP_HIT -64,00.
```

## Fix 2026-07-08 - Max hold fino a New York close

```text
Decisione:
- Non chiudere automaticamente a fine London.
- Entry London vicine alla chiusura possono essere corrette e devono avere tempo fino alla sessione US.
- Tempo massimo operativo: New York regular close, 16:00 New York.

Informazione sessione:
- New York regular cash close = 16:00 America/New_York.
- Nel periodo estivo normale: 16:00 NY = 22:00 Italia = 20:00 UTC.
- Il codice usa MarketTimeZones.NewYork, quindi gestisce DST tramite timezone.

Implementazione:
- Ogni posizione salva SessionCloseTimeUtc = 16:00 New York del giorno di entry.
- Historical replay aggiorna posizioni anche fuori London fino a NY close; le entry restano permesse solo in London.
- Se una posizione supera NY close viene chiusa con ExitReason=NEW_YORK_CLOSE.
- Se il replay finisce prima di NY close, la posizione viene loggata come [MR_REPLAY_OPEN] e non produce [MR_EXIT], quindi non entra nel PnL.
- OnLiveCumulativeTrade gestisce posizioni aperte anche fuori London, senza generare nuove entry fuori London.
```

## Baseline 2026-07-08 - Punto fisso operativo

```text
Baseline ufficiale: london-ny-close-hold
Code commit:        f20ec7b
Validation docs:    26b17f5
Reload validato:    2026-07-08 10:40
PnL validato:       +634,25 da [MR_EXIT]
Docs aggiornate:    FabioOrderFlow.md, LondonMeanReversionModel.md, BalanceZoneTracker.md, log-reading.md
```

Contratto baseline:

```text
MR = Mean Reversion operativo.
Reference attive = PreviousDayProfile + PreviousLondonProfile.
Target operativo = reference POC loggato in [MR_ENTRY] TargetPOC.
Gestione = full target al POC, stop, breakeven a MFE >= 1R, max hold a New York regular close 16:00 NY.
PnL valido = solo [MR_EXIT].
```

Chiarimento POC:

```text
Se il volume profile visuale ATAS e' impostato su Current Day, puo' mostrare un POC diverso dal target modello.
Esempio live 2026-07-08: visual POC circa 29500, ma [MR_ENTRY] TargetPOC=29540.
Questo e' corretto perche' il trade ha Source=PreviousDayProfile e ReferenceLabel=2026-07-07.
```

## Reload 2026-07-08 09:34 - Reference complete + breakeven validated

```text
Reload dopo implementazione PreviousDayProfile + PreviousLondonProfile + breakeven:
- [MR_MODE] corretto: ReferenceProfiles=PreviousDayProfile|PreviousLondonProfile, BreakEvenTrigger=1.00R.
- [MR_REFERENCE_READY]: 12 totali, 6 PreviousDayProfile + 6 PreviousLondonProfile.
- [HISTORICAL_FLOW_FINISH]: Entries=19, ClosedPositions=19, OpenPositions=0, CompletedTrades=19.
- PnL storico da [MR_EXIT]: +493,50.
- [MR_BREAKEVEN]: 10 attivazioni.
- No residuali DAY_STUDY/delayed/secondary/pressure/historical-intrabar/follow/second-leg/target2/scale-in.

Breakdown storico:
- POC_TARGET_HIT: 8 trade, +793,25.
- STOP_HIT: 10 trade, -332,75.
- LONDON_CLOSE: 1 trade, +33,00.
- BreakEvenActivated=True: 10 trade, +793,25.
- BreakEvenActivated=False: 9 trade, -299,75.
- Per giorno: 2026-07-01 -19,75; 2026-07-02 +517,00; 2026-07-06 -36,75; 2026-07-08 +33,00.
- Per source: PreviousDayProfile 9 trade +411,75; PreviousLondonProfile 10 trade +81,75.

Nota live:
- Dopo il replay e' apparso anche un [MR_ENTRY] LIVE alle 09:34:58, Long PreviousDayProfile, Entry=29398,00, Stop=29335,00, TargetPOC=29540,00.
- Nel log analizzato non c'era ancora [MR_EXIT] LIVE per quel SetupId.

Diagnosi:
- La correzione della reference ha aumentato le entry e recuperato una PnL storica positiva.
- Il breakeven funziona e trasforma diversi trade potenzialmente rientrati in stop in zero o target.
- Resta da valutare se il modello sta sovra-tradando sullo stesso swing, soprattutto il 2026-07-02 con 11 trade.
```

## Implementazione 2026-07-08 - Reference complete + breakeven

```text
Decisione utente:
- Implementare entrambe le reference coerenti con Fabio:
  1. PreviousDayProfile
  2. PreviousLondonProfile
- Entrambe devono essere live/replay, non study.
- Poi aggiungere gestione risk-free/breakeven dopo movimento favorevole sufficiente.

Implementato:
- LondonMeanReversionModel non usa piu' il developing POC/VAH/VAL della London corrente per generare entry.
- Il modello costruisce internamente due reference complete da barre ATAS:
  - PreviousDayProfile = giorno italiano precedente completo.
  - PreviousLondonProfile = London session precedente completa.
- Setup operativi creati solo su sweep e close back inside di una reference completa.
- Log [MR_REFERENCE_READY] per ogni reference completata.
- [MR_SETUP_LONG]/[MR_SETUP_SHORT] includono Source e ReferenceLabel.
- Entry sempre su cumulative big trade >=20 nella direzione mean-reversion.
- Target full position al reference POC.
- Break-even operativo: a MFE >= 1.0R, stop spostato a entry; log [MR_BREAKEVEN].
- Replay storico continua a usare la stessa logica live.

Nota:
- I setup duplicati tra PreviousDayProfile e PreviousLondonProfile possono coesistere come reference diverse.
- Se una entry viene confermata su una reference, gli overlap stesso bar/direzione vengono scaduti per evitare doppia posizione sullo stesso evento.
```

## Reload 2026-07-08 09:08 - No-entry audit validated

```text
Reload con audit esteso:
- [MR_MODE] corretto, modello core pulito.
- [CUM_TRADES_RESPONSE] Count=1.219.566.
- [MR_HISTORICAL_TRADES] BeginItaly=2026-07-01 00:00:00, EndItaly=2026-07-08 09:08:36.
- ActiveSetups=23.
- Entry=1, Exit=1, PnL da [MR_EXIT] = -32,75.
- Setup 30/06 marcati correttamente EXCLUDED_OUTSIDE_HISTORICAL_TRADE_COVERAGE.
- No residuali DAY_STUDY/delayed/secondary/pressure/follow/second-leg/target2/scale-in.

Audit:
- EntryRejects=WRONG_AGGRESSION_DIRECTION:22|OUTSIDE_SHORT_ENTRY_ZONE:5|RR_TO_POC_TOO_LOW:2.
- Expirations=POC_TOUCHED_BY_TRADE:17|OUTSIDE_HISTORICAL_TRADE_COVERAGE:3|TIMEOUT:2.
- 17 setup muoiono per touch POC, spesso micro-volume 1-6 contratti e senza big trade same-direction prima del touch.
- L'unico entry e' 2026-07-07 16:30 Long, stop hit -32,75, MFE=34,75.

Diagnosi:
- Il modello usa il developing profile della stessa London session come POC/VAH/VAL.
- Questo rende spesso il POC troppo vicino al fakeout: target minuscolo, touch immediato e RR basso.
- Dal transcript Fabio parla di previous balance area / bulk dell'asta e nella versione semplice usa il profilo del giorno precedente.
- Prossima correzione coerente: usare una reference value area precedente/completa, non il micro developing POC della sessione corrente.
```

## Fix 2026-07-08 - Replay audit per setup

```text
Reload dopo fix replay state:
- Setup attivi: 23.
- Entry: 1.
- Exit: 1.
- PnL da [MR_EXIT]: -32,75.
- Entry unica: 2026-07-07 16:30 Long, stop hit.
- Reject globali: WRONG_AGGRESSION_DIRECTION=22, OUTSIDE_SHORT_ENTRY_ZONE=5, RR_TO_POC_TOO_LOW=2.
- Scadenze: soprattutto POC_TOUCHED_BY_TRADE.

Problema interpretativo emerso:
- La regola attuale invalida il setup appena il prezzo tocca il POC esatto, anche con micro trade da 1-6 contratti.
- Questo e' probabilmente troppo meccanico rispetto a Fabio: il target e' l'area/bulk dell'asta, non necessariamente un singolo tick POC.
- Inoltre i setup prima del primo cumulative trade disponibile non devono contaminare il replay.

Fix audit:
- ExpireSetups viene eseguito su ogni trade storico, non solo sui big trade.
- Setup fuori dalla copertura cumulative trades vengono marcati EXCLUDED_OUTSIDE_HISTORICAL_TRADE_COVERAGE.
- [MR_SETUP_NO_ENTRY] ora include primo big trade same-direction, primo big trade opposite-direction e primo POC touch.
```

## Fix 2026-07-07 - Historical replay setup state

```text
Analisi dopo reload core pulito:
- I setup venivano creati durante la scansione barre storiche.
- Alcuni setup venivano poi marcati Expired/PocTouched da barre future prima che arrivasse il replay dei cumulative trades.
- Quando OnCumulativeTradesResponse processava i trade storici, trovava quindi setup gia' scaduti.
- In live invece i cumulative trades arrivano temporalmente prima delle barre future.

Fix:
- ProcessHistoricalPositions ora resetta AggressionConfirmed/Expired/PocTouched prima del replay.
- Il replay storico scorre poi i cumulative trades in ordine temporale.
- Aggiunta invalidazione POC da trade durante replay: POC_TOUCHED_BEFORE_ENTRY_BY_TRADE.
- Aggiunto audit pulito: [MR_REPLAY_AUDIT] e [MR_SETUP_NO_ENTRY] per capire perche' un setup non entra.
```

## Reload 2026-07-07 19:19 - Core Clean Validated

```text
Reload dopo reset Fabio core:
- [MR_MODE] presente: Model=FabioLondonMeanReversionCore, Modes=LIVE|HISTORICAL, BigTradeVolume=20, Target=POC_FULL_EXIT.
- [CUM_TRADES_RESPONSE] Count=1.346.471.
- [MR_HISTORICAL_TRADES] Count=1.346.471, BeginItaly=2026-06-30 00:00:00, EndItaly=2026-07-07 19:19:34.
- [HISTORICAL_FLOW_FINISH] Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.
- Setup trovati: 10 long, 15 short; 21 scaduti per POC toccato prima di entry.
- Nessun log residuo di DAY_STUDY, delayed reclaim, secondary rejection, pressure gate, historical intrabar, follow-through, second-leg, target2, scale-in.
- PnL da [MR_EXIT]: 0,00 perche' non ci sono entry valide nel core pulito.
- Rimossi dal filesystem i vecchi file FabioOrderFlow-days, ormai obsoleti e confondenti.
```

## Baseline 2026-07-07 - Fabio London Mean Reversion Core Clean

Decisione operativa richiesta: ripulire completamente `LondonMeanReversionModel` e lasciare una sola tipologia di entry, coerente con il transcript Fabio.

Contratto attuale:

```text
MR = Mean Reversion
LIVE = dati real-time ATAS
HISTORICAL = dati passati processati con le stesse regole live
PnL valido = solo [MR_EXIT]
```

Entry unica:

```text
London value area attiva
sweep/fakeout fuori VAH/VAL
close back inside value
cumulative big trade >= 20 contratti nella direzione di rientro
entry dentro value prima del POC
target full position al POC
stop piccolo vicino all'estremo fallito
```

Pulizia effettuata:

```text
- LondonMeanReversionModel ridotto a un solo file: LondonMeanReversionModel.cs.
- Rimossi partial file e moduli ausiliari precedenti.
- Rimossi log e percorsi di ricerca/debug paralleli.
- Rimossi modelli alternativi e filtri aggiunti sui pochi giorni disponibili.
- Rimasta solo gestione full exit al POC oppure stop.
- Rimasto solo replay storico delle stesse regole live.
```

Log attuali:

```text
[MR_MODE]
[MR_SETUP_LONG]
[MR_SETUP_SHORT]
[MR_SETUP_EXPIRED]
[MR_HISTORICAL_TRADES]
[HISTORICAL_FLOW_PROCESS_START]
[HISTORICAL_FLOW_FINISH]
[MR_ENTRY]
[MR_EXIT]
[MR_LIVE_HEARTBEAT]
```

Build:

```text
cd FabioOrderFlow/src && dotnet build -c Release
Avvisi: 0
Errori: 0
```

Deploy:

```text
%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll
```

Rollback utili:

```text
checkpoint-london-before-core-only     stato prima della semplificazione core-only
london-core-clean                      core-only prima del refactor totale
london-pressure-secondary-live         stato con filtri/secondary poi rimosso
```
