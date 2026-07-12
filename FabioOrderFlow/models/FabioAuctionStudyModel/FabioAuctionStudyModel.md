# FabioAuctionStudyModel

Ledger diagnostico causale derivato congiuntamente da `transcription.txt` e `trascription_1.txt`.

## Stato

```text
Modalita':          DUAL_SESSION_AUCTION_STATE_LEDGER_NO_TRADES
Sessioni:           London 08:00-16:00 London
                    New York 09:30-16:00 New York
Direzioni:          simmetriche LONG e SHORT
Ordini / PnL:       DISABLED
Output:             una osservazione per barra/sessione completata
```

Il modulo non genera setup, shadow entry, ordini, posizioni, stop, target o PnL. Serve a evitare che la discovery venga ristretta prematuramente a un solo lato o a una sola sequenza.

## Tesi Dei Transcript

```text
London reversion:
BALANCE -> primo drive fuori -> aggressione senza risultato -> rientro -> aggressione opposta -> POC/bulk.

New York continuation:
IMBALANCE -> profilo impulso -> LVN/pullback -> aggressione con risultato -> continuation verso nuova balance.
```

Le due famiglie restano separate per sessione e stato. Non si usa il modello London durante New York ne' il modello trend durante una balance.

## Record

Marker:

```text
[AUCTION_STATE_MODE]
[AUCTION_STATE_BAR]
[AUCTION_STATE_SUMMARY]
[AUCTION_IMPULSE_READY]
[AUCTION_IMPULSE_PULLBACK_BAR]
[AUCTION_IMPULSE_RESOLVED]
```

Ogni `[AUCTION_STATE_BAR]` include:

```text
Session / SessionDate / SessionBarOrdinal / ChartTimeFrame
OHLC / CandleVolume / PriceChange / CloseLocation
BidVolume / AskVolume / Delta / DeltaImbalance
MaxBidAtPrice / MaxAskAtPrice
CumulativeTradeCount / cumulative buy-sell volume / cumulative delta
MaxCumulativeBuy / MaxCumulativeSell / CumulativeTradeCoverage
PriorPOC / PriorVAH / PriorVAL / posizione rispetto alla value area precedente
DevelopingPOC / DevelopingVAH / DevelopingVAL
Prior6/Prior12 range ed efficienza direzionale
RangeToPrior12Median / OverlapWithPrevious
profilo rolling 12 barre
LVN locali piu' vicini sopra/sotto nel profilo sessione e rolling
OperationalEntry=FALSE / OrderSubmitted=FALSE
```

Il profilo `Prior*` esclude sempre la barra corrente. Il profilo `Developing*` la include ed e' disponibile alla sua close. Gli LVN sono minimi locali raw: il ledger salva prezzo e percentile del volume, senza soglia di qualificazione.

## Effort Versus Result

Classificazione simmetrica e senza soglia ottimizzata:

```text
BUY_WITH_RESULT    delta > 0 e close > open
BUY_ABSORBED       delta > 0 e close <= open
SELL_WITH_RESULT   delta < 0 e close < open
SELL_ABSORBED      delta < 0 e close >= open
NEUTRAL            delta = 0
```

Queste etichette descrivono la singola barra. Un modello Fabio richiede ancora location e sequenza; una label `ABSORBED` isolata non e' un segnale.

## Cumulative Trades

Le finestre storiche ATAS vengono aggregate direttamente nelle barre London/NY. Il modulo non conserva milioni di oggetti trade. Gli update live dello stesso cumulative trade sostituiscono il valore precedente tramite chiave stabile, evitando doppio conteggio.

```text
TradeCoverage=AVAILABLE   almeno un cumulative trade restituito nella barra
TradeCoverage=MISSING     nessun trade restituito; non equivale a flow zero
```

## New York Impulse Profile A->B

Lifecycle causale, senza soglie di selezione:

```text
BUILDING
- parte quando la close precedente era nella prior value;
- la barra corrente chiude fuori value con aggressione nella stessa direzione;
- accumula le barre che estendono l'estremo senza rientrare dal boundary di origine.

READY
- la prima barra che non estende chiude l'impulso A->B;
- il profilo viene congelato prima di includere tale barra;
- registra POC/VAH/VAL e tutti gli LVN raw del profilo impulso.

RESOLVED
- ogni barra successiva e' registrata come pullback;
- termina a nuovo estremo, rientro dal boundary di origine, two-sided range o session end.
```

`READY` non e' una entry. `PULLBACK_BAR` registra location, LVN toccati, effort/result e coverage cumulative senza qualificare il record.

## Report

```bash
python FabioOrderFlow/tools/report_auction_state_ledger.py --save
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
```

I comandi restituiscono esclusivamente JSON su stdout. Il secondo salva profili A->B, barre pullback e risoluzioni. Gli aggregati sono descrittivi, senza segnali o PnL.

## Reload Verificato 2026-07-11

```text
Auction bars:                    6.488
London / New York:               3.508 / 2.980
Date per sessione:               40 / 40
Cumulative ricevuti / matched:   5.833.055 / 4.854.494
New York first bar:              09:30 local
OperationalEntries / Orders:     0 / 0
Errori:                          0
```

L'analisi iniziale e' in `docs/research/fabio-auction-playbooks-analysis-2026-07-11.md`. Ha prodotto 73 osservazioni tra balance rotation e NY pullback, ma nessun modello validato. NY SHORT H6 resta una traccia diagnostica su 13 casi; LONG fallisce lo split test.

## Reload M5 Rithmic Impulse 2026-07-12

```text
ChartTimeFrame:                   M5
Date per sessione:               7 / 7
Cumulative ricevuti / matched:   1.645.204 / 1.370.383
Impulse READY / RESOLVED:        37 / 37
LONG / SHORT:                    17 / 20
Pullback bars AVAILABLE:         96 / 96
Continuation / Origin reentry:   10 / 21
Two-sided / Session end:         5 / 1
OperationalEntries / Orders:     0 / 0
```

Il lifecycle e il report sono verificati end-to-end su M5. Tutte le 96 barre pullback attraversano almeno un LVN raw: il semplice touch non e' discriminante e resta descrittivo.

## Reload M1 dxFeed 2026-07-12

```text
Chart range:                      2026-07-03 -> 2026-07-10
Auction bars:                    5.039
London / New York:               2.879 / 2.160
Date per sessione:               6 / 6
Cumulative ricevuti / matched:   0 / 0
OperationalEntries / Orders:     0 / 0
Errori:                          0
```

Il dataset dxFeed e' valido per candle footprint, session profile e geometria A->B. Non e' valido per cumulative big-trade analysis: entrambe le richieste storiche ATAS hanno restituito `Count=0`.

## Timeframe

```text
M1 = discovery primaria per impulso A->B e microstruttura.
M5 = baseline separata per confronti precedenti.
```

`Prior6`, `Prior12`, rolling 12 e H6/H12 sono espressi in barre. Non confrontare direttamente M1 e M5 e non applicare gli analyzer M5 precedenti al CSV M1.

## Da Verificare

```text
- reload M1 con Rithmic e cumulative coverage disponibile;
- conteggi READY/PULLBACK/RESOLVED del nuovo lifecycle;
- equivalenza live/historical su nuove barre M1;
- nuove date prospettiche senza modificare il contratto A->B.
```
