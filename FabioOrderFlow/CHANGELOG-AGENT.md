# CHANGELOG AGENT - FabioOrderFlow

## Reload 2026-07-11 18:56 - Shadow Path, Prima Verifica

```text
Replay M5:
- 8/8 finestre; ReceivedTrades=5.833.055; StoredTrades=236.018.
- 27 profili, 117 eventi, 468 outcome ledger completi.
- 25 acceptance totali: 22 AVAILABLE, equivalenti al campione offline, e 3 MISSING.
- 50 outcome shadow; 284 path bars; nessun path AVAILABLE con TradeCount=0.
- Entries=0; posizioni=0; ShadowOrders=0; nessun errore o marker operativo.

Correzione derivata:
- 16/25 path si fermavano a 11 barre per timestamp H12 pochi secondi oltre 60 minuti.
- Il path ora include la prima barra completata che raggiunge 60 minuti, entro tolleranza massima 5 minuti.
- Retention cumulative estesa allo stesso limite; da verificare con secondo reload.
```

## Implementazione 2026-07-11 - Shadow Path Completo e Setup Discovery Aperta

```text
Path post-entry:
- MR_SHADOW_ACCEPTANCE_BAR per ogni barra completata fino a 60 minuti.
- Include timeframe, elapsed minutes, OHLC, candle volume, cumulative flow, directional move, MFE/MAE progressivi, range state e POC touch.
- Historical cumulative retention estesa a RESOLVED + 60 minuti per evitare flow post-entry falso a zero.

Timeframe:
- Granularita' = chart ATAS; M5 circa 12 path bars, M1 circa 60.
- L'API Indicator installata non espone richiesta storica candle M1 separata da chart M5.
- Per path M1 applicare l'indicatore a chart M1; ogni marker dichiara ChartTimeFrame.

Setup discovery:
- Ogni MR_COMPRESSION_LEDGER_EVENT porta DiagnosticState.
- OUTSIDE_FIRST, OUTSIDE_ACCEPTANCE, INSIDE_BOUNDARY_INTERACTION, OPPOSITE_OUTSIDE restano tutti osservabili.
- Solo OUTSIDE_ACCEPTANCE alimenta la shadow corrente; nessun evento viene scartato dal ledger.

Vincoli invariati:
- OperationalEntry=FALSE; OrderSubmitted=FALSE; Entries=0; ShadowOrders=0.
```

## Implementazione 2026-07-11 - Acceptance Continuation Shadow Live

```text
Ipotesi promossa a diagnostica live/historical:
- ACCEPTANCE_CONTINUATION_V1.
- Trigger: seconda close consecutiva fuori dal range congelato.
- HIGH -> shadow LONG; LOW -> shadow SHORT.
- Outcome H6/H12 = close a 6/12 barre dalla shadow entry.

Marker:
- MR_SHADOW_ACCEPTANCE_ENTRY.
- MR_SHADOW_ACCEPTANCE_OUTCOME.
- OperationalEntry=FALSE; OrderSubmitted=FALSE; ShadowOrders=0.

Vincoli:
- Massimo una shadow entry per profilo.
- Nessun ordine, posizione, stop, target, PnL, MR_ENTRY o MR_EXIT.
- DirectionalMoveRanges positivo significa continuation favorevole.

Da validare al reload:
- MR_MODE ShadowModel=ACCEPTANCE_CONTINUATION_V1 e ShadowOrders=DISABLED.
- HISTORICAL_FLOW_FINISH Entries=0, ShadowAcceptanceEntries>0, ShadowAcceptanceOutcomes=2x entry quando H6/H12 disponibili.
- Nessun errore o marker operativo.
```

## Analisi 2026-07-11 - Entry Shadow Confermate da Stato

```text
Contratti offline causali:
- FAILED_BREAKOUT_REVERSION: prior OUTSIDE stesso bordo -> evento chiude INSIDE.
- ACCEPTANCE_CONTINUATION: OUTSIDE con OutsideCloseStreak>=2.
- Entry al trigger close; exit H6/H12; massimo una entry per profilo/contratto.
- EndReason non usato; nessun ordine, stop, target o PnL.

Risultati:
- Acceptance: 22 profili; train H6 +0,040/H12 -0,332; test H6 +0,147/H12 +0,870.
- Failed breakout: 8 profili; train H6 -0,162/H12 -0,426; test H6 +0,070/H12 -0,700.
- HIGH acceptance test H12: 6 profili, media +0,938, mediana +0,560; senza outlier massimo media +0,450.
- HIGH acceptance train H12 resta negativo: media -0,213, mediana -0,265.

Decisione:
- Failed breakout reversion respinta sul campione corrente.
- Acceptance continuation conservata come ipotesi shadow prospettica congelata, non validata.
- ValidatedContracts=[]; OperationalEntries=DISABLED.
```

## Analisi 2026-07-11 - Shadow Fixed-Horizon sui Dati Disponibili

```text
Contratto offline:
- Primo evento al bordo per ogni profilo AVAILABLE; massimo una shadow observation per profilo.
- Direzione verso l'interno; exit close a 6/12 barre; nessuno stop, target, ordine o PnL inventato.
- 24 profili: train cronologico 17, test 7; selectionLeakage=true per overlap con analisi precedente.

Risultato test:
- Baseline: H6 +0,104 range; H12 -0,586.
- HIGH: H6 -0,222; H12 -1,040, 5 profili.
- LOW: H6 +0,920; H12 +0,550, ma solo 2 profili.
- Volume top quartile: H6 +0,187; H12 -1,133, solo 3 profili.
- Compact: H6 +0,288; H12 -0,002, 5 profili.
- Nessun candidato raggiunge 8 profili test o stabilita' H6/H12.

Decisione:
- ValidatedCandidates=[]; nessuna promozione shadow/operativa.
- Report: ledger-snapshots/compression-shadow-2026-07-11.json.
```

## Reload 2026-07-11 18:18 - Chart Esteso, Coverage Verificata

```text
Chart/replay:
- ChartBeginItaly=2026-05-18 00:00:00; ChartEndItaly=2026-07-10 22:59:59.
- WindowCount=8; CUM_TRADES_COMPLETE=8/8; ReceivedTrades=5.833.055.
- LedgerProfiles=27, LedgerEvents=117, LedgerOutcomes=468 completi.
- Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.

Coverage:
- TradeCoverage=AVAILABLE: 24 profili, 107 eventi, 16 date profilo distinte.
- TradeCoverage=MISSING: 3 profili, 10 eventi, tutti 26/06.
- Le finestre 18/05-08/06 hanno risposte ATAS molto rade; aggiungono solo 3 profili.

Decisione:
- 16 sessioni coperte sono ancora insufficienti per validare una shadow entry.
- Etichettare i 24 profili AVAILABLE come tight/extended/unclear e continuare la raccolta fino a 20-30 sessioni coperte.
- Nessuna soglia, entry, stop, target o PnL aggiunto.
```

## Refinement 2026-07-11 - Compression Ledger JSON Only

```text
- tools/report_compression_ledger.py restituisce esclusivamente JSON su stdout.
- Con --save persiste il report summary JSON e i CSV dataset; il renderer e il file report.txt sono rimossi.
- Contratto JSON verificato sul replay corrente: report corretto, outcomesComplete=true, events=109.
- Nessuna modifica a lifecycle, coverage, classificazione o operativita' trade.
```

## Analisi 2026-07-11 - Compression Ledger, 21 Profili Flow-Covered

```text
Dataset:
- 24 profili, 109 eventi, 436 outcome completi.
- 21 profili / 99 eventi TradeCoverage=AVAILABLE; 3 profili / 10 eventi MISSING esclusi dal confronto flow.
- Report ripetibile: tools/report_compression_ledger.py --save.
- Report JSON e dataset CSV: ledger-snapshots/compression-ledger-2026-07-11_18-02-29-*.

Evidenza descrittiva:
- 97 BREACH e solo 2 TOUCH: confronto touch/breach non ancora possibile.
- HIGH e LOW hanno esiti normalizzati diversi; la media per evento e quella pesata per profilo divergono.
- Il percentile volume relativo 0,76-1,00 e' un'ipotesi diagnostica da riesaminare, non un filtro.
- Nessuna classificazione, soglia, entry, stop, target o PnL e' stata aggiunta.

Decisione:
- Raccogliere altre sessioni e aggiungere etichette manuali tight/extended/unclear.
- Valutare sempre conteggi di profili distinti e confronto fuori campione prima di un modello shadow.
```

## Implementazione 2026-07-11 - Historical Cumulative Trades Windowed

```text
Problema:
- Con chart esteso a 21 giorni, il vecchio codice tagliava esplicitamente i cumulative trade agli ultimi 7 giorni.
- Il ledger poteva quindi avere range vecchi ma flow MISSING, non utilizzabile per conclusioni order-flow.

Soluzione:
- Il chart intero viene diviso in richieste CumulativeTrades sequenziali di massimo 7 giorni.
- Viene inviata una sola richiesta ATAS alla volta; ProcessHistoricalPositions parte solo dopo CUM_TRADES_COMPLETE.
- Il modello trattiene in streaming soltanto i trade dentro le finestre READY -> RESOLVED dei profili rilevati.
- Log nuovi: CUM_TRADES_REQUEST/RESPONSE/COMPLETE e MR_HISTORICAL_TRADES_WINDOW.
- TradeCoverage continua a distinguere flow restituito da ATAS e dati storici non disponibili.

Da validare:
- CUM_TRADES_LOOKBACK WindowCount>1.
- Tutte le finestre ricevono risposta e CUM_TRADES_COMPLETE le conferma.
- StoredTrades/TradeCoverage crescono per i profili piu' vecchi se ATAS conserva quei dati.
- Ledger resta senza trade: Entries=0.
```

## Reload 2026-07-11 17:48 - Historical Windowed Requests Verificate

```text
Chart/replay:
- ChartBeginItaly=2026-06-12 00:00:00
- ChartEndItaly=2026-07-10 22:59:59
- CurrentBar=5.700
- WindowCount=5, RequestWindowDays=7
- CUM_TRADES_COMPLETE WindowsCompleted=5/5
- ReceivedTrades=4.807.895
- StoredTrades=75.654 (solo finestre READY -> RESOLVED dei profili)

Ledger:
- LedgerProfiles=24, LedgerEvents=109, LedgerOutcomes=436
- TradeCoverage=AVAILABLE: 21 profili
- TradeCoverage=MISSING: 3 profili, tutti il 26/06
  - 10:49-10:54 Italy
  - 13:04-13:14 Italy
  - 13:54-14:14 Italy
- Nessun errore/exception; Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.

Decisione:
- La precedente copertura limitata non era una retention ATAS di sette giorni: ATAS restituisce anche i trade dal 12/06.
- I tre profili MISSING restano validi solo per geometria e outcome prezzo, non per flow.
- Non classificare ne' promuovere setup: 24 profili sono ancora un campione esplorativo.
```

## Reload 2026-07-11 13:24 - Event Ledger e Coverage Confermati

```text
StudyMode=COMPRESSION_EVENT_LEDGER_NO_TRADES.
LedgerQualification=NONE.
ChartVisuals=LONDON_CONTEXT_ONLY.

Risultato:
- StoredTrades=1.314.530.
- LedgerProfiles=7; LedgerEvents=34; LedgerOutcomes=136.
- Entries=0; ClosedPositions=0; OpenPositions=0; CompletedTrades=0.
- Nessun MR_COMPRESSION_STUDY_*, MR_ENTRY, MR_EXIT o errore FabioOrderFlow.
- 02/07 TradeCoverage=MISSING e tutti i percentile prior=NA.
- Gli altri sei profili hanno percentili raw variabili, quindi confrontabili causalmente.
```

## Reload 2026-07-11 13:21 - Event Ledger funzionale, coverage fix distribuito

```text
Reload iniziale ledger:
- StudyMode=COMPRESSION_EVENT_LEDGER_NO_TRADES; LedgerQualification=NONE.
- StoredTrades=1.314.530; LedgerProfiles=7; LedgerEvents=34; LedgerOutcomes=136.
- Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.
- Nessun MR_COMPRESSION_STUDY_*, MR_ENTRY, MR_EXIT o errore FabioOrderFlow.
- TradeCoverage=AVAILABLE per 6 profili e MISSING per il 02/07, fuori dal lookback cumulative-trade.

Fix successivo distribuito:
- I percentili zero del profilo senza coverage ora sono NA, non 1,00.
- Nessun cambiamento a eventi, outcome, lifecycle o operativita'.
- Da verificare al prossimo reload: LedgerProfiles=7, LedgerEvents=34, LedgerOutcomes=136 e percentile NA sul 02/07.
```

## Implementazione 2026-07-11 - Compression Event Ledger, no drawings/no thresholds di qualifica

```text
Decisione:
- Ritirati dal flusso attivo MR_COMPRESSION_STUDY_CASE, MR_COMPRESSION_STUDY_CANDIDATE e i loro marker/box DynamicCompression.
- Il chart conserva soltanto zona London, POC/VAH/VAL grigi del BalanceZoneTracker come contesto.
- Nessun entry, stop, target, PnL o oggetto candidato viene simulato/disegnato.

Ledger:
- Dopo READY, ogni barra che tocca o supera High/Low genera MR_COMPRESSION_LEDGER_EVENT.
- Non c'e' filtro BigTradeVolume, due close, score, retest o direzione per registrare un evento.
- Il record contiene geometria normalizzata, flow raw, delta/CVD, massimi trade e percentili causali delle barre precedenti nello stesso range.
- MR_COMPRESSION_LEDGER_OUTCOME registra esito a 1/3/6/12 barre: close move, MFE/MAE, rientro e POC touch.
- Historical calcola gli outcome dal replay completo; live li aggiunge solo quando l'orizzonte diventa disponibile.

Limite esplicito:
- READY continua a usare il lifecycle dynamic-score come scoperta della finestra; il ledger non usa quelle soglie per qualificare o scartare gli eventi.

Da validare al reload:
- [MR_MODE] StudyMode=COMPRESSION_EVENT_LEDGER_NO_TRADES e LedgerQualification=NONE.
- [HISTORICAL_FLOW_FINISH] Entries=0, LedgerProfiles>0, LedgerEvents>0, LedgerOutcomes>0.
- Nessun nuovo MR_COMPRESSION_STUDY_*, MR_ENTRY o MR_EXIT.
- Zona London grigia visibile; nessun box turchese/marker candidato.
```

## Decisione 2026-07-11 - Zona London contestuale e valutazione compression

```text
Visual:
- Riattivato DrawBalanceProfileVisuals=true.
- BalanceZoneTracker disegna di nuovo zona London, POC, VAH e VAL grigi.
- Gli oggetti sono solo contesto: non influenzano DynamicCompression, candidati, trade o PnL.

Valutazione reload 10:27:
- Il detector e' causalmente corretto ma misura ancora contrazione/overlap delle barre, non soltanto dealing range visivamente stretti.
- Caso piu' compatto: 03/07 12:05-12:39, 7 barre, range 19,50, 15 high test, buy boundary 403, CVD +406.
- 08/07 e 09/07 sono profile larghi/estesi: candidati studio validi meccanicamente, non promossi a compression Fabio-style.
- 02/07 non ha copertura cumulative-trade nello storico ricevuto; resta solo contesto geometrico.
- Il caso 06/07 mostra EntryCandidate=StopCandidate: evidenza che lo studio non e' un entry model.
- Analisi completa: docs/research/compression-study-evaluation-2026-07-11.md.

Regola:
- Non introdurre ora un filtro di ampiezza, retest, target o entry.
- La prossima decisione diagnostica separata e' classificare/rifiutare deterministicamente i range estesi dopo confronto chart.
```

## Reload 2026-07-11 10:27 - Contratto Studio Confermato

```text
DLL: 4b372fcb71a3cac473fc233d8ebb11501962f9e50fa009cc36a3892a13cf59e5
[MR_MODE]: StudyMode=COMPRESSION_CASES_NO_TRADES; OperationalEntries=DISABLED.

CUM_TRADES_LOOKBACK:
- EffectiveBeginItaly=2026-07-03 22:59:59
- EndItaly=2026-07-10 22:59:59
- StoredTrades=1.314.530

Risultato replay:
- READY=7, tutti ProfileUse=STUDY_INPUT_ONLY.
- RESOLVED=7, tutti ProfileUse=STUDY_INPUT_ONLY.
- StudyCandidates=8, tutti OperationalEntry=FALSE.
- Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.
- Nessun marker MR_SETUP, MR_ENTRY, MR_EXIT, MR_BREAKEVEN o MR_REPLAY_OPEN.
- Nessun errore o eccezione FabioOrderFlow nel reload.
```

## Reload 2026-07-11 10:16 - Compression Study verificato

```text
CUM_TRADES_LOOKBACK:
- EffectiveBeginItaly=2026-07-03 22:59:59
- EndItaly=2026-07-10 22:59:59
- StoredTrades=1.314.530

Risultato:
- CompressionProfiles=7; READY=7; RESOLVED=7.
- [MR_COMPRESSION_STUDY_CASE]=7.
- StudyCandidates=8: BREAKOUT_LONG=5, BREAKOUT_SHORT=1, REVERSION_LONG=1, REVERSION_SHORT=1.
- Entries=0, ClosedPositions=0, OpenPositions=0, CompletedTrades=0.
- Nessun [MR_ENTRY], [MR_EXIT], setup legacy, errore o eccezione.

Correzione contratto:
- [MR_LOCAL_PROFILE_READY] ora dichiara ProfileUse=STUDY_INPUT_ONLY, coerente con RESOLVED e studio.

BalanceZoneTracker:
- Rimane invariato perche' funziona: [ZONE_READY] e forwarding storico/live sono presenti.
- POC/VAH/VAL, high/low, state machine e profile visual non influenzano il modello studio.
- Refactor futuro separato: riduzione/rinomina a LondonTracker, esclusivamente responsabile di identificare la sessione London.
```

## Implementazione 2026-07-11 - Fabio Compression Study, no trades

```text
Decisione:
- Ritirato il core PreviousDayProfile/PreviousLondonProfile come trigger di setup/entry.
- Le due reference restano costruite solo come log [MR_REFERENCE_READY].
- Nessun MR_ENTRY, MR_EXIT, posizione o PnL puo' essere prodotto dal modello attivo.
- DynamicCompression resta disegnato; il profile grafico grigio di BalanceZoneTracker e' disattivato.

Studio live-equivalente:
- Reversion: breach, aggressione grande assorbita, rientro e big trade opposto entro due barre.
- Breakout: due close di acceptance, big trade al bordo e CVD coerente.
- Marker: [MR_COMPRESSION_STUDY_CASE], [MR_COMPRESSION_STUDY_CANDIDATE], [MR_COMPRESSION_STUDY_PROFILE].
- Ogni candidato dichiara OperationalEntry=FALSE; non esiste PnL studio.
- Breakout target resta UNDEFINED_REQUIRES_SEPARATE_MODEL. Reversion usa Compression POC solo come target candidato.

Fonti/contratto:
- Analizzati transcription.txt e trascription_1.txt.
- Sintesi: docs/research/fabio-transcript-synthesis.md.
- I transcript distinguono Model 1 New York continuation e Model 2 London reversion; il codice studia solo Model 2 avanzato.

Da validare al reload:
- [MR_MODE] StudyMode=COMPRESSION_CASES_NO_TRADES e OperationalEntries=DISABLED.
- [HISTORICAL_FLOW_FINISH] Entries=0, CompletedTrades=0, StudyCandidates=N.
- Box turchesi DynamicCompression e marker candidati visibili; nessun box/livello grigio BalanceZoneTracker.
- Nessun nuovo [MR_ENTRY] o [MR_EXIT].
```

## Decisione 2026-07-10 - Comunicazione sempre chiarificatrice

```text
Ogni risposta/aggiornamento deve riepilogare in modo conciso, anche se gia' discusso:
- Operativo: cio' che genera o gestisce trade.
- Diagnostico: cio' che osserva soltanto e non modifica il PnL.
- Cambiato: modifica effettuata nel ciclo corrente.
- Da verificare: evidenza ancora mancante.
Non assumere che il contesto precedente sia ricordato.
```

## Correzione 2026-07-11 - Setup multipli, entry singola

```text
Decisione:
- La policy SINGLE_SETUP_AND_POSITION ha ridotto il replay reload a una sola entry ed e' superseded.
- I setup possono ora coesistere e conservano timeout/POC touch individuali.
- ProcessAggressionTrade blocca una nuova entry solo quando esiste una posizione aperta.
- Nessun setup pending viene scaduto automaticamente quando una posizione apre.
- Risultato atteso: nessuna posizione simultanea, ma nessuna soppressione preventiva dei setup.

Visual:
- Il reload del 09:30 conferma sette DynamicCompression, nessuna il 10/07.
- I marker trade sono ampliati a tre barre e cinque tick; le linee closed durano almeno tre barre.
- Da verificare dopo reload: marker entry/exit visibili e zero sovrapposizioni di posizioni.
```

## Superseded 2026-07-11 - Single lifecycle + chart visuals

```text
Operativo:
- Decisione B implementata: una sola catena setup -> posizione alla volta.
- Setup pending blocca la ricerca di altri setup; posizione aperta blocca nuovi setup.
- Nella stessa candela PreviousDayProfile ha priorita' su PreviousLondonProfile; il primo setup valido interrompe la ricerca.
- Dopo entry, ogni setup pending residuo viene scaduto come difesa.
- Nessun pyramiding/stacking di posizioni.

Visual chart:
- DynamicCompression: box turchese trasparente, POC turchese, VAH/VAL tratteggiati.
- Entry long verde, short arancio-rosso; stop cremisi; target blu.
- Exit profit verde, loss cremisi, breakeven oro.
- Funziona nel replay storico e live. Il bar mostra l'evento; il timestamp preciso resta in Italy= nel log.

Da validare dopo reload:
- [MR_MODE] SetupConcurrency=SINGLE_SETUP_AND_POSITION e ChartVisuals=MR_TRADE_AND_DYNAMIC_COMPRESSION.
- Nessuna posizione sovrapposta nel replay.
- Rettangoli/linee presenti e leggibili sul chart senza interferire con BalanceZoneTracker.
- DynamicCompression resta DIAGNOSTIC_ONLY; PreviousDayProfile e PreviousLondonProfile restano le sole reference operative.
```

## Audit storico 2026-07-11 - Overlapping setup / position

```text
Comportamento prima della correzione entry singola:
- Il core permetteva setup distinti della stessa Direction + Source + ReferenceLabel se derivavano da RejectionBar diversi.
- ExpireOverlappingSetupsOnEntry scadeva solo setup con stessa Direction e stesso RejectionBar.

Evidenza replay 2026-07-11:
- 08/07 long PreviousDayProfile: posizioni sovrapposte 09:25:09-10:07:50 e 09:34:58-10:16:39.
- 10/07 short PreviousLondonProfile: doppia entry 12:22:36 da setup 12:04:57 e 12:09:57.
- 10/07 short PreviousLondonProfile: posizioni sovrapposte 14:04:28-14:13:44 e 14:05:14-14:13:44.
- 10/07 short PreviousLondonProfile: 14:46:29 resta aperta mentre aprono 14:53:25 e 15:30:06.

Calcolo:
- Entry/Exit/PnL delle 19 posizioni replay sono aritmeticamente coerenti.
- Questo non e' un errore di PnL o del profilo locale: e' stacking di setup permesso dalla logica operativa corrente.

Decisione:
- Non correggere in questo ciclo: cambiare a una posizione per Direction+Source+ReferenceLabel e' una modifica operativa separata dal detector compressione.
- Prima definire il contratto desiderato: stacking intenzionale oppure una sola posizione alla volta per reference/direzione.
```

## Reload 2026-07-11 09:06 - Dynamic CompressionScore verificato

```text
Operativo:
- [MR_MODE] mantiene ReferenceProfiles=PreviousDayProfile|PreviousLondonProfile.
- [CUM_TRADES_LOOKBACK] 2026-07-03 22:59:59 -> 2026-07-10 22:59:59; Count=1.314.530.
- [HISTORICAL_FLOW_FINISH]: 19 entry, 19 closed, 0 open, 19 completed.
- Tratto comune al reload 15:28: 17 [MR_EXIT], PnL -419,25 invariato.
- Due exit successive spiegano il totale finale -455,25: 0,00 e -36,00.
- Nessuna regressione causata dal profilo diagnostico.

Diagnostico:
- [MR_MODE] corretto: CompressionDetection=DYNAMIC_SCORE e lifecycle SEARCHING|BUILDING|READY|RESOLVED.
- [MR_LOCAL_PROFILE_READY]=7; RESOLVED=7, contro 46/46 della versione precedente.
- Finestre 7-11 barre; range minimo 19,50, mediano 46,75, massimo 88,25.
- CompressionScore minimo 0,67, mediano 0,69, massimo 0,80.
- Risoluzioni: 6 ACCEPTANCE_ABOVE_RANGE, 1 ACCEPTANCE_BELOW_RANGE; tutte dopo ResolutionCloses=2.
- Nessun range >= 150 punti: eliminato il problema dei profili 159,50-243,75.
- [MR_PROFILE_CONTEXT]=0: nessuna entry e' avvenuta mentre uno dei 7 profili era ancora nello stato READY; non e' un errore.

Decisione:
- Lifecycle dinamico e riduzione dei falsi profili validati tecnicamente.
- Non modificare ora score/persistenza: prima confrontare visivamente i 7 range con le compressioni Fabio-style.
- ActiveCompressionProfile resta DIAGNOSTIC_ONLY; nessun filtro, target o nuova entry.
```

## Implementazione 2026-07-10 - Dynamic CompressionScore

```text
Operativo:
- PreviousDayProfile e PreviousLondonProfile invariati.
- Nessun cambio entry, stop, target, breakeven, max hold o PnL.

Diagnostico:
- Sostituiti finestra 6-18 e filtri rigidi con lifecycle SEARCHING -> BUILDING -> READY -> RESOLVED.
- SEARCHING usa distribuzione causale delle precedenti 12-24 barre London.
- BUILDING parte dal primo bar nel 50% inferiore della volatilita' e cresce per overlap/acceptance.
- CompressionScore: contraction 20%, overlap 20%, directional 15%, rotation 10%, containment 10%, boundary stability 10%, POC stability 7,5%, value concentration 7,5%.
- L'estensione richiede overlap con le ultime 3 barre, non con l'intero envelope, per evitare range che inglobano lentamente un movimento ampio.
- READY: score >= 0,65 per 2 barre consecutive, minimo 6 barre.
- RESOLVED: 2 close consecutive fuori range con tolleranza dinamica 15% della mediana precedente.
- Nessun log BUILDING per evitare rumore; marker esterni restano READY/PROFILE_CONTEXT/RESOLVED.
- Label: DynamicCompression.

Decisione:
- Il checkpoint rigido 98a1839 e' superato prima della sua validazione; non usare le soglie 3,00 / 0,85 come contratto corrente.
- ActiveCompressionProfile resta DIAGNOSTIC_ONLY.

Da verificare:
- [MR_MODE] CompressionDetection=DYNAMIC_SCORE e lifecycle completo.
- Conteggio READY/RESOLVED, distribuzione CompressionScore e componenti.
- ProfileReadyTime < entry per ogni contesto.
- Entry/PnL invariati sul tratto comune.
```

## Reload 2026-07-10 15:28 - Prima normalizzazione insufficiente

```text
Operativo:
- Tratto comune al reload precedente: 13 [MR_EXIT], PnL HISTORICAL -279,75 invariato.
- Copertura nuova estesa fino alle 15:28: 18 entry historical; 17 exit PnL + 1 [MR_REPLAY_OPEN].
- Nessun cambio a PreviousDayProfile, PreviousLondonProfile o gestione trade.

Diagnostico:
- [MR_LOCAL_PROFILE_READY]=46 e RESOLVED=46, contro 54/54 precedenti.
- [MR_PROFILE_CONTEXT]=5: 4 historical + 1 live; causalita' ReadyTime < EntryTime sempre corretta.
- Range mediano 65,13; massimo 243,75.
- Restano 5 profili >= 150 punti: 159,50 / 168,25 / 176,25 / 196,50 / 243,75.
- RangeToBaselineMedian massimo 3,30; AverageBarRangeToBaselineMedian massimo 1,25.

Conclusione:
- La soglia media 1,25 consente espansione, non impone una vera contrazione.
- Prima normalizzazione NON validata come classificatore finale.

Correzione singola:
- RangeToBaselineMedian: 4,00 -> 3,00.
- AverageBarRangeToBaselineMedian: 1,25 -> 0,85.
- Tutto resta DIAGNOSTIC_ONLY; nuovo reload richiesto.
```

## Implementazione 2026-07-10 - Compressione normalizzata per volatilita'

```text
Operativo:
- PreviousDayProfile e PreviousLondonProfile invariati.
- Nessun cambio entry, stop, target, breakeven, max hold o PnL.

Diagnostico:
- ActiveCompressionProfile richiede ora una baseline causale precedente alla finestra candidata.
- Baseline: mediana high-low delle 6-12 barre London precedenti, escluse dalla candidata.
- ProfileRange / BaselineMedianBarRange <= 4,00.
- AverageProfileBarRange / BaselineMedianBarRange <= 1,25.
- Label nuova: VolatilityAdjustedCompression.
- Nuovi campi log: BaselineMedianBarRange, RangeToBaselineMedian, AverageBarRangeToBaselineMedian.

Motivo:
- Il reload precedente aveva accettato 54 profili, inclusi range da 163,00 / 232,75 / 245,00 punti.
- Overlap e rotazione relativi alla sola finestra non bastavano a distinguere una compressione da una fase volatile ampia.

Da verificare al reload:
- [MR_MODE] deve mostrare CompressionVolatilityBaseline=PRIOR_12_BARS_MEDIAN.
- Confrontare READY/RESOLVED contro i precedenti 54/54.
- Verificare range massimo, distribuzione RangeToBaselineMedian e contesti allegati alle entry.
- Entry/PnL devono restare invariati a parita' di lookback: il profilo resta DIAGNOSTIC_ONLY.
```

## Reload 2026-07-10 12:47 - Compression lifecycle verificato

```text
Reload:
- [MR_MODE] corretto: ReferenceProfiles=PreviousDayProfile|PreviousLondonProfile.
- CompressionLifecycle=READY|RESOLVED, CompressionMinBars=6, ProfileDiagnosticsUse=DIAGNOSTIC_ONLY.
- [CUM_TRADES_LOOKBACK] 2026-07-03 12:47:06 -> 2026-07-10 12:47:06; Count=1.133.256.
- [HISTORICAL_FLOW_FINISH]: Entries=13, ClosedPositions=13, OpenPositions=0, CompletedTrades=13.
- [MR_REPLAY_OPEN]: 0.

Performance, senza mischiare replay e live:
- HISTORICAL: 13 trade / 3 sessioni, PnL -279,75, profit factor 0,29, average R -0,49, max drawdown 299,75 punti.
- LIVE dopo reload: 1 trade, PnL -9,00.
- Stato: INSUFFICIENT_SAMPLE; costi non configurati.
- Il report ora usa HISTORICAL come default. LIVE e ALL sono selezioni esplicite; ALL non e' PnL valido se replay e live si sovrappongono.

ActiveCompressionProfile:
- [MR_LOCAL_PROFILE_READY]=54; [MR_LOCAL_PROFILE_RESOLVED]=54.
- Reason: 28 ACCEPTANCE_ABOVE_RANGE, 21 ACCEPTANCE_BELOW_RANGE, 5 SESSION_END.
- [MR_PROFILE_CONTEXT]=5, tutti HISTORICAL e tutti causalmente corretti: ProfileReadyTime precede l'entry.
- 9 entry non avevano un profilo READY: corretto, la diagnostica non forza un contesto.
- Il detector resta troppo permissivo: ha ancora classificato range da 163,00 / 232,75 / 245,00 punti, gia' incompatibili con la compressione locale cercata.

Decisione:
- Lifecycle e causalita' sono validati tecnicamente.
- Qualita' della classificazione NON validata; ActiveCompressionProfile resta DIAGNOSTIC_ONLY e non puo' filtrare entry.
- Nessun cambio a PreviousDayProfile, PreviousLondonProfile, entry, stop, target, breakeven o PnL.
- Prossimo scope: aggiungere una misura robusta di contrazione rispetto alla volatilita' locale/sessione, poi ripetere il reload senza cambiare l'operativita'.
```

## Implementazione 2026-07-10 - Compression lifecycle + report canonico

```text
Reference operative confermate:
- PreviousDayProfile resta operativo.
- PreviousLondonProfile resta operativo.
- Entry, stop, target POC, breakeven e max hold non cambiano.

ActiveCompressionProfile diagnostico:
- Ritirato LatestSwingPairToSetup, che includeva spinte/reversal e dipendeva dalla barra di setup.
- Nuovo lifecycle causale su barre London completate: READY e RESOLVED.
- Detection window 6-18 barre; overlap adiacente >= 70%; almeno 2 cambi direzione close.
- DirectionalEfficiency <= 0,40; CloseSpanRatio <= 0,80; RangeToAverageBarRange <= 2,75.
- Il profilo viene congelato quando READY; una close oltre il range di 4 tick lo risolve.
- Un setup allega il profilo solo con ReadyBar < RejectionBar.
- Nuovi marker: [MR_LOCAL_PROFILE_READY] e [MR_LOCAL_PROFILE_RESOLVED].
- [MR_PROFILE_CONTEXT] resta ENTRY_ONLY e DIAGNOSTIC_ONLY.

Pulizia e performance:
- Nuovo tool unico: tools/report_mr_performance.py.
- PnL solo da [MR_EXIT]; report per giorno/source/direzione/reason/mode, R, profit factor e drawdown.
- Gate minimo: 30 trade, 10 sessioni, costi completi, PnL netto positivo, average R positiva, profit factor >= 1,25.
- Strumenti/snapshot della vecchia gestione 70/30 e DAY_STUDY spostati in archive/legacy-research; non usarli sul core corrente.

Snapshot prima del nuovo deploy, log corrente 2026-07-10:
- Il file conteneva 11 trade HISTORICAL e 3 LIVE sovrapposti temporalmente: non devono essere sommati.
- HISTORICAL: PnL -258,25; profit factor 0,31; average R -0,49; max drawdown 278,25 punti.
- Stato report: INSUFFICIENT_SAMPLE; costi non ancora configurati.
- Questi numeri descrivono il core precedente al deploy del nuovo detector diagnostico; non autorizzano filtri sulle reference.

Validazione richiesta dopo reload:
- Build Release deve restare 0 warning / 0 error.
- [MR_MODE] deve mantenere ReferenceProfiles=PreviousDayProfile|PreviousLondonProfile e mostrare CompressionLifecycle=READY|RESOLVED.
- Verificare sequenze [MR_LOCAL_PROFILE_READY] -> eventuale [MR_PROFILE_CONTEXT] -> [MR_LOCAL_PROFILE_RESOLVED].
- PnL/entry storiche devono restare invariati a parita' di lookback, perche' il detector e' solo diagnostico.
```

## Reload 2026-07-10 11:51 - ActiveCompressionProfile check

```text
Copertura replay:
- [CUM_TRADES_LOOKBACK] EffectiveBeginItaly=2026-07-03 11:51:21, EndItaly=2026-07-10 11:51:21.
- [CUM_TRADES_RESPONSE] Count=1.131.053.
- [HISTORICAL_FLOW_FINISH]: Entries=11, ClosedPositions=11, OpenPositions=0, CompletedTrades=11.
- [MR_REPLAY_OPEN]: 0.

Operativita' invariata:
- [MR_MODE] corretto: ProfileDiagnostics=ActiveCompressionProfile, ProfileDiagnosticsUse=DIAGNOSTIC_ONLY, ProfileDiagnosticsLevel=ENTRY_ONLY.
- [MR_PROFILE_CONTEXT]: 11, tutti ProfileSource=ActiveCompressionProfile e Context=ENTRY.
- Nessuna entry/exit live nel file dopo reload.
- PnL del rolling replay: -258,25 da [MR_EXIT]. Non confrontabile con la baseline +634,25: la request mobile a 7 giorni ha escluso il 01/07 e il 02/07, che contenevano +735,00.
- Il 09/07 non ha prodotto setup/entry nel log disponibile. Il 10/07 fino alle 11:51 ha un solo setup long PreviousDayProfile, poi EXPIRED_TIMEOUT senza big trade Buy; nessuna entry.

Verifica ActiveCompressionProfile:
- La diagnostica e' meccanicamente corretta: ogni entry porta ProfileBegin/End, High/Low, POC/VAH/VAL e i rapporti rispetto a entry/target.
- L'algoritmo attuale usa solo l'ultima coppia swing high/swing low e un minimo di 3 barre.
- Nei casi guida 06/07 15:35, 15:50 e 16:02 seleziona rispettivamente finestre 7/4/6 barre con range 163,00 / 191,00 / 232,75 punti, incluse nella spinta/reversal. Non sono ancora una compressione/dealing range dimostrata.
- Conclusione: ActiveCompressionProfile e' un prototipo di localizzazione, non ancora la replica del profile di compressione che Fabio disegna manualmente. Non usare POC/VAH/VAL di questa diagnostica per entry, filtro o target.

Scope invariato:
- Prossimo lavoro solo sul criterio che riconosce una vera compressione preesistente e indipendente dall'entry.
- Retest, filtri e nuove entry restano esplicitamente fuori scope.
```

## Implementazione 2026-07-08 - Focus unico ActiveCompressionProfile

```text
Decisione:
- Stop a nuovi rami: niente retest, niente filtri, niente nuove entry in questo ciclo.
- Scopo unico: verificare come Fabio disegna il profilo locale reale della compressione/dealing range.

Transcript Fabio:
- Previous day profile = versione semplice e oggettiva.
- Modalita' reale/avanzata = identificare la consolidation/compression/dealing range e plottare il volume profile su quella zona.
- Il profilo deve nascere prima dell'entry, non essere costruito a posteriori sull'entry.

Implementato:
- [MR_PROFILE_CONTEXT] ora usa solo ProfileSource=ActiveCompressionProfile.
- CurrentLondonSessionProfile e LocalRotationProfile sono parcheggiati per evitare confusione.
- ActiveCompressionProfile non e' direzionale: cerca nella London corrente l'ultima coppia swing high/swing low gia' formata e profila da quel primo estremo alla candela di setup.
- Log dedicato resta ENTRY_ONLY e DIAGNOSTIC_ONLY.
- Nessun cambio entry/exit/target/stop/PnL.

Da validare al prossimo reload:
- [MR_MODE] deve mostrare ProfileDiagnostics=ActiveCompressionProfile.
- [MR_PROFILE_CONTEXT] deve mostrare ProfileSource=ActiveCompressionProfile.
- Controllare se ProfileBegin/ProfileEnd/ProfileHigh/ProfileLow corrispondono davvero alla compressione che Fabio avrebbe disegnato sul chart.
```

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
