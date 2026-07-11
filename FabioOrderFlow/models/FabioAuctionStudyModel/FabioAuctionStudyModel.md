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

## Report

```bash
python FabioOrderFlow/tools/report_auction_state_ledger.py --save
```

Il comando restituisce esclusivamente JSON su stdout e salva summary JSON e CSV barre. Gli aggregati sono descrittivi, senza soglie, segnali o PnL.

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

## Da Verificare

```text
- stabilita' live/historical degli update cumulative su nuove barre;
- profilo causale dell'impulso A->B, distinto dal rolling profile 12 barre;
- granularita' M1 su chart M1, mantenuta separata da M5;
- nuove date prospettiche senza modificare i contratti post-hoc.
```
