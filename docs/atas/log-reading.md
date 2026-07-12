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
[HISTORICAL_FLOW_FINISH]          baseline London disattivata; non deve apparire nel runtime NY-only
[AUCTION_STATE_MODE]              NEW_YORK_IMPULSE_STUDY_NO_TRADES, Sessions=NEW_YORK
[AUCTION_STATE_BAR]               baseline dual-session; disabilitato nel runtime NY-only
[AUCTION_STATE_SUMMARY]           copertura NY, lifecycle impulse e guardrail no-trade
[AUCTION_IMPULSE_READY]           profilo New York A->B congelato prima del pullback
[AUCTION_IMPULSE_PULLBACK_BAR]    location e flow di ogni barra dopo B
[AUCTION_IMPULSE_RESOLVED]        nuovo estremo, origin reentry, two-sided o session end
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

## Come Leggere L'Auction State Ledger

```text
Session=LONDON|NEW_YORK            contesti separati; una barra overlap puo' avere due record
PriorPOC/VAH/VAL                   profilo sessione prima della barra corrente
DevelopingPOC/VAH/VAL              profilo disponibile alla close corrente
PriorProfileRelation               ABOVE_VAH | BELOW_VAL | INSIDE_VALUE
Prior6/Prior12Efficiency           avanzamento netto rispetto al path precedente
Prior*LvnAbove/Below               minimo locale raw, non livello qualificato
EffortResult                       BUY/SELL_WITH_RESULT | BUY/SELL_ABSORBED | NEUTRAL
MaxCumulativeBuy/Sell              massima bolla cumulative nella barra
CumulativeTradeCoverage            AVAILABLE | MISSING
```

`ABSORBED` descrive delta e risultato prezzo della barra; da solo non e' un setup. London reversion richiede ancora sequenza breakout-rientro-risposta opposta. New York continuation richiede stato di imbalance, location LVN/pullback e aggressione con risultato.

Il lifecycle A->B parte da una transizione inside-value -> close esterna con effort/result coerente. Accumula solo le barre che estendono l'estremo senza rientrare dal boundary di origine, poi congela il profilo prima della prima barra di pullback. `ImpulseLvns` e `TouchedLvns` restano liste legacy `prezzo:percentile`, non livelli qualificati.

Con `LvnRanking=RAW_CAUSAL_V1`, `ImpulseLvnMetrics` e `TouchedLvnMetrics` aggiungono nell'ordine: prezzo, volume percentile, adjacent depth, shoulder depth, prominence, prominence rank, rank score, position in range, directional progress, distanza da POC, origine e bordo. Nessuna metrica filtra i raw LVN. I marker pullback/resolved non ripetono la geometria statica: il report la ricostruisce dal READY tramite `ImpulseId`.

```bash
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
python FabioOrderFlow/tools/analyze_auction_impulse_lvn_ranking.py --timeframe M1 --save
```

L'analyzer LVN usa soltanto il primo pullback strettamente precedente a `ResolvedBar`; esclude same-bar outcome, profili senza raw LVN e barre senza touch. Restituisce distribuzioni/AUC continue con `selectionLeakage=true`, mai soglie o segnali.

Su dxFeed storico ATAS puo' restituire `CUM_TRADES_RESPONSE Count=0`: il profilo footprint resta disponibile, ma ogni pullback avra' `CumulativeTradeCoverage=MISSING`. Non interpretarlo come aggressione zero.

```bash
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M1 --save
```

`NY_IMPULSE_LVN_CUMULATIVE_CONFIRMATION_V1` richiede una barra precedente alla risoluzione, directional `WITH_RESULT`, raw LVN attraversato e cumulative max direzionale `>=30` e maggiore dell'opposto. La soglia 30 viene dal transcript New York. Il raw LVN touch resta descrittivo: sul primo replay e' presente in ogni pullback.

## Componenti London

`BalanceZoneTracker` e `LondonMeanReversionModel` sono baseline compilate ma non inizializzate. Nel runtime NY-only non devono apparire `[ZONE_READY]`, `[MR_MODE]`, `[HISTORICAL_FLOW_FINISH]` o disegni London.

## Verifica Reload

```text
1. [AUCTION_STATE_MODE] contiene StudyMode=NEW_YORK_IMPULSE_STUDY_NO_TRADES.
2. Sessions=NEW_YORK e AuctionStateBars=DISABLED.
3. [CUM_TRADES_COMPLETE] mostra tutte le finestre prima di [AUCTION_STATE_SUMMARY].
4. LondonBars=0; READY/PULLBACK/RESOLVED sono coerenti col report impulse.
5. Nessun HISTORICAL_FLOW_FINISH, AUCTION_STATE_BAR, MR_ENTRY o MR_EXIT.
6. TradeCoverage=MISSING non equivale a flow zero.
```

## Report

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report performance non valuta il ledger perche' non esistono trade o PnL. Usarlo solo per confronti legacy basati su `[MR_EXIT]`.

```bash
python FabioOrderFlow/tools/report_auction_state_ledger.py --save
```

Il report auction-state e' JSON-only. Legge i vecchi dataset dual-session e, nel runtime corrente, valida il summary NY-only senza richiedere righe `[AUCTION_STATE_BAR]`.

```bash
python FabioOrderFlow/tools/analyze_fabio_auction_playbooks.py \
  --save FabioOrderFlow/ledger-snapshots/fabio-auction-playbooks-2026-07-11.json
```

L'analizzatore ricostruisce balance rotation e NY imbalance pullback con split cronologico. `selectionLeakage=true` resta obbligatorio finche' non arrivano date prospettiche indipendenti.

```bash
python FabioOrderFlow/tools/report_compression_ledger.py --save
```

Il report ledger restituisce solo JSON, seleziona l'ultimo replay `[HISTORICAL_FLOW_FINISH]` completo, verifica `eventi x 4 = outcome`, salva il report JSON e, con `--save`, i CSV in `FabioOrderFlow/ledger-snapshots/`. Usa flow solo con `TradeCoverage=AVAILABLE`. Gli aggregati non sono segnali. Per confrontare HIGH e LOW usare i campi normalizzati `*Reversion*`, non il segno prezzo di `CloseMoveRanges`.
