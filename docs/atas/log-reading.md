# ATAS Log Reading

Guida canonica per interpretare i log di `FabioOrderFlow`.

## Principi

- Il file corrente viene azzerato a ogni inizializzazione indicatore.
- `WriteItaly=` e' l'ora di scrittura; per il mercato usare `Italy=`, `London=` e `UTC=` inclusi nell'evento.
- Il modello attivo e' `COMPRESSION_EVENT_LEDGER_NO_TRADES`: non deve emettere `[MR_ENTRY]`, `[MR_EXIT]`, posizioni o PnL.
- `[MR_EXIT]` resta fonte PnL solo nei log legacy precedenti allo studio.

## File Corrente

```text
%APPDATA%/ATAS/Logs/FabioOrderFlow.log
```

## Contratto Ledger

```text
LIVE        registra eventi al bordo dopo READY e outcome quando 1/3/6/12 barre sono disponibili.
HISTORICAL  richiede finestre ATAS sequenziali di massimo 7 giorni e ricostruisce gli stessi record dopo l'ultima risposta.

Chart       solo zona London grigia del BalanceZoneTracker, come contesto.
No chart    DynamicCompression, marker candidati, entry, stop o target.
```

Marker attivi:

```text
[CUM_TRADES_LOOKBACK]              chart completo, durata finestra e numero richieste pianificate
[CUM_TRADES_REQUEST/RESPONSE]      una richiesta/risposta ATAS per finestra
[CUM_TRADES_COMPLETE]              tutte le finestre concluse prima del replay
[MR_HISTORICAL_TRADES_WINDOW]      trade ricevuti, trattenuti, fuori finestre ledger e duplicati
[MR_MODE]                         StudyMode=COMPRESSION_EVENT_LEDGER_NO_TRADES
[ZONE_READY]                      zona London grigia, POC/VAH/VAL solo contesto
[MR_REFERENCE_READY]              PreviousDay/PreviousLondon costruiti solo come log
[MR_LOCAL_PROFILE_READY]          range causalmente disponibile al ledger
[MR_LOCAL_PROFILE_RESOLVED]       fine della finestra osservata
[MR_COMPRESSION_LEDGER_PROFILE]   geometria, test, volume, CVD e coverage del range
[MR_COMPRESSION_LEDGER_EVENT]     touch/breach High o Low senza qualifica trade
[MR_COMPRESSION_LEDGER_OUTCOME]   esito a 1, 3, 6, 12 barre
[MR_SHADOW_ACCEPTANCE_ENTRY]      seconda close esterna; osservazione continuation, nessun ordine
[MR_SHADOW_ACCEPTANCE_OUTCOME]    esito direzionale dopo 6/12 barre
[MR_SHADOW_ACCEPTANCE_BAR]        ogni barra chart fino alla prima completata che raggiunge 60 minuti
[MR_SHADOW_LOW_FLOW_CONFIRMATION_ENTRY]    LOW + flow direzionale positivo nelle prime 3 barre
[MR_SHADOW_LOW_FLOW_CONFIRMATION_OUTCOME]  outcome H6/H12 dalla close di conferma
[MR_SHADOW_LOW_FLOW_CONFIRMATION_BAR]      path 60 minuti dalla close di conferma
[HISTORICAL_FLOW_FINISH]          Entries=0, LedgerProfiles=N, LedgerEvents=N, LedgerOutcomes=N
```

## Come Leggere Un Range

1. Trova `[MR_LOCAL_PROFILE_READY]`. Prima di quel punto nessun evento e' registrato per quel range.
2. Leggi `[MR_COMPRESSION_LEDGER_PROFILE]` per `RangeToBaselineMedian`, `HighTests`, `LowTests`, `BoundaryEvents`, `BuyVolume`, `SellVolume`, `ProfileCVD` e `TradeCoverage`.
3. Per ogni `[MR_COMPRESSION_LEDGER_EVENT]` confronta:

```text
Boundary / Interaction             HIGH|LOW e TOUCH|BREACH
TestOrdinal                        ripetizione causale del test sullo stesso lato
OutsideCloseStreak                quante close consecutive erano gia' fuori da quel bordo
BreachDistanceRanges              estensione oltre bordo, normalizzata per ampiezza range
CloseDistanceRanges               close rispetto al bordo, normalizzata per ampiezza range
BarRangeToBaselineMedian          volatilita' barra rispetto alla mediana precedente
TradeCount / TotalVolume          partecipazione raw della barra
BuyVolume / SellVolume / Delta    aggressione raw, senza soglia fissa
ProfileCVD                        pressione cumulata dal READY all'evento
MaxBuyVolume / MaxSellVolume      massimo singolo trade per lato nella barra
*PercentilePrior                  posizione rispetto alle barre precedenti dello stesso range
```

4. Per ogni `[MR_COMPRESSION_LEDGER_OUTCOME]` leggi:

```text
HorizonBars                       1, 3, 6 o 12 barre dopo l'evento
CloseMoveRanges                   variazione close normalizzata per range
UpMfeRanges / DownMaeRanges       escursione massima sopra/sotto l'evento
EndInsideRange                    close finale rientrata nel range
PocTouched                        POC del range attraversato nell'orizzonte
```

I percentili sono metriche di confronto causale, non condizioni. `NA` significa che non esistono ancora barre precedenti comparabili oppure che la coverage cumulative trade e' assente.

## Come Leggere La Shadow Acceptance

```text
H6 / H12               6 o 12 barre dopo la shadow entry; su chart 5 minuti circa 30/60 minuti
Direction LONG/SHORT   continuation sopra HIGH / sotto LOW
DirectionalMoveRanges positivo se il movimento segue la direzione shadow
FavorableMfeRanges     escursione massima favorevole
AdverseMfeRanges       escursione massima contraria
OperationalEntry      sempre FALSE
OrderSubmitted         sempre FALSE
```

H6/H12 sono finestre di osservazione, non target. I marker shadow non vanno sommati come PnL e non sono `[MR_ENTRY]`.

`MR_SHADOW_ACCEPTANCE_BAR` e `MR_SHADOW_LOW_FLOW_CONFIRMATION_BAR` espongono il path completo fino alla prima barra completata che raggiunge i 60 minuti, con tolleranza finale massima di 5 minuti per M5: `ChartTimeFrame`, `PathBarOrdinal`, `ElapsedMinutes`, OHLC, volume candela, flow cumulative, movimento direzionale, MFE/MAE progressivi, stato rispetto al range e POC touch. La granularita' coincide con il chart: M1 richiede un chart M1; un chart M5 non espone candele M1 separate all'indicatore.

La conferma LOW flow e' una seconda shadow distinta. `DirectionalFlowImbalance=-sum(Delta)/sum(TotalVolume)` sulle prime tre barre dopo l'acceptance; deve essere `>0`. La sua entry diagnostica e' la close della terza barra. Non modifica, cancella o qualifica retroattivamente la baseline acceptance.

## BalanceZoneTracker Corrente

`BalanceZoneTracker` identifica London e disegna la zona grigia per orientamento. `[ZONE_READY]`, POC/VAH/VAL, high/low e state machine non partecipano al ledger. Il refactor futuro lo ridurra' a `LondonTracker`, responsabile soltanto di inizio, fine e appartenenza alla sessione London.

## Verifica Reload

```text
1. [MR_MODE] contiene StudyMode=COMPRESSION_EVENT_LEDGER_NO_TRADES.
2. [HISTORICAL_FLOW_FINISH] contiene Entries=0 e contatori Ledger non nulli.
3. Nessun nuovo MR_SETUP, MR_ENTRY, MR_EXIT, MR_BREAKEVEN o MR_REPLAY_OPEN.
4. Il chart mostra la zona London grigia, ma non box turchesi o marker candidati.
5. `[CUM_TRADES_COMPLETE]` deve mostrare tutte le finestre completate prima del replay.
6. Per profili senza trade nelle risposte ricevute, `TradeCoverage=MISSING`: non inferire flow assente dal mercato. Il provider potrebbe non conservare quella finestra storica.
```

## Report

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report performance non valuta il ledger perche' non esistono trade o PnL. Usarlo solo per confronti legacy basati su `[MR_EXIT]`.

```bash
python FabioOrderFlow/tools/report_compression_ledger.py --save
```

Il report ledger restituisce solo JSON, seleziona l'ultimo replay `[HISTORICAL_FLOW_FINISH]` completo, verifica `eventi x 4 = outcome`, salva il report JSON e, con `--save`, i CSV in `FabioOrderFlow/ledger-snapshots/`. Usa flow solo con `TradeCoverage=AVAILABLE`. Gli aggregati non sono segnali. Per confrontare HIGH e LOW usare i campi normalizzati `*Reversion*`, non il segno prezzo di `CloseMoveRanges`.
