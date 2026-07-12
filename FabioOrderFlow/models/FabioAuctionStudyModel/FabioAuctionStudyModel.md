# FabioAuctionStudyModel

Studio causale New York A->B derivato congiuntamente da `transcription.txt` e `trascription_1.txt`.

## Stato

```text
Modalita':          NEW_YORK_IMPULSE_STUDY_NO_TRADES
Sessione:           New York 09:30-16:00 New York
Direzioni:          simmetriche LONG e SHORT
Ordini / PnL:       DISABLED
Auction-state bars: DISABLED
Output:             READY + PULLBACK_BAR + RESOLVED + summary
```

Il modulo non genera setup, shadow entry, ordini, posizioni, stop, target o PnL. London reversion e il ledger dual-session restano baseline storiche e non sono nel percorso runtime.

## Tesi Dei Transcript

```text
London reversion:
BALANCE -> primo drive fuori -> aggressione senza risultato -> rientro -> aggressione opposta -> POC/bulk.

New York continuation:
IMBALANCE -> profilo impulso -> LVN/pullback -> aggressione con risultato -> continuation verso nuova balance.
```

Le due famiglie restano concettualmente separate. Il runtime corrente studia soltanto New York continuation; London non viene calcolata.

## Record

Marker:

```text
[AUCTION_STATE_MODE]
[AUCTION_STATE_BAR]             DISABLED nel runtime NY-only
[AUCTION_STATE_SUMMARY]
[AUCTION_IMPULSE_READY]
[AUCTION_IMPULSE_PULLBACK_BAR]
[AUCTION_IMPULSE_RESOLVED]
```

Per velocizzare il replay non viene piu' emessa una riga per ogni barra. In memoria ogni barra NY conserva soltanto i dati necessari:

```text
SessionDate / bar / begin-end UTC / high-low-close
footprint bid-ask per classificare effort/result
prior NY POC/VAH/VAL, sempre precedente alla barra corrente
contributo volume-per-prezzo
cumulative trade count e max buy/sell
```

Non vengono piu' calcolati developing profile, rolling 6/12, overlap o LVN sessione/rolling. Gli LVN raw del profilo impulso congelato restano completi e senza soglia di qualificazione.

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
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
```

Il comando restituisce esclusivamente JSON su stdout e salva profili A->B, barre pullback e risoluzioni. `report_auction_state_ledger.py` resta compatibile con snapshot dual-session precedenti e valida il summary NY-only, ma il runtime corrente non emette barre auction-state. Gli aggregati sono descrittivi, senza segnali o PnL.

## Baseline Dual-Session Verificata 2026-07-11

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

## Reload Runtime NY-Only Verificato 2026-07-12

```text
Range / timeframe:               2026-05-18 -> 2026-07-10 / M1
Sessions / LondonBars:           NEW_YORK / 0
NewYorkBars:                     13.303
Cumulative ricevuti / matched:  5.833.055 / 4.170.108
Impulse READY / PULLBACK:       396 / 1.132
Impulse RESOLVED:               396
Coverage AVAILABLE / MISSING:   1.095 / 37
MR / auction-state bar / error: 0 / 0 / 0
```

I CSV profiles, pullbacks e resolutions sono identici byte-per-byte alla baseline dual-session. Il tempo totale mode-to-summary scende da `180,528s` a `49,630s` (`-72,5%`); il log da `65.627.353` a `4.075.440` byte (`-93,8%`). Il contratto diagnostico e i risultati `6/11` historical e `7/8` formalization restano invariati.

## Estensione M1 Rithmic a 40 Date 2026-07-12

```text
Range:                           2026-05-18 -> 2026-07-10
Date per sessione:              40 / 40
Cumulative ricevuti / matched:  5.833.055 / 4.854.494
Finestre complete:              8 / 8
Impulse READY / RESOLVED:       396 / 396
Pullback AVAILABLE / MISSING:   1.095 / 37
OperationalEntries / Orders:    0 / 0
```

Applicazione invariata del contratto a `SessionDate < 2026-07-02`:

```text
Profile continuation baseline:  108/307 = 35,2%
Primary confirmations:          11 su 9 date
Clean continuation:             6/11 = 54,5%
LONG / SHORT:                   4/6 / 2/5
```

Il risultato e' superiore al baseline descrittivo, ma molto inferiore al `7/8` del campione di formalizzazione. `Validated=FALSE`, `PromotedToShadow=FALSE`, storico precedente non prospettico.

## Reload M1 Rithmic e Conferma Cumulative 2026-07-12

```text
Date per sessione:               7 / 7
Cumulative ricevuti / matched:   1.645.204 / 1.370.383
Impulse READY / RESOLVED:        89 / 89
LONG / SHORT:                    47 / 42
Pullback AVAILABLE:              253 / 253
Continuation / Origin reentry:   35 / 39
Two-sided:                       15
OperationalEntries / Orders:     0 / 0
```

Contratto offline congelato:

```text
pullback prima della risoluzione
-> raw LVN attraversato
-> WITH_RESULT nella direzione impulso
-> max cumulative direzionale >=30 e > opposto
```

M1 produce 10 conferme su 5 date, 8 continuation e 2 reentry. Limitando alla prima per data/direzione: 8 osservazioni, 7 continuation e 1 reentry. M5 separato produce 5 continuation su 8 primarie. `Validated=FALSE`, `PromotedToShadow=FALSE`, `selectionLeakage=true`.

Il raw LVN touch e' presente in tutte le barre pullback e non e' discriminante. I prossimi dati devono aggiungere rilevanza/rank causale mantenendo tutti i minimi raw.

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
- nuove date prospettiche senza modificare il contratto A->B;
- ranking causale degli LVN raw senza soglia di selezione post-hoc;
- equivalenza live/historical dei nuovi campi LVN prima di usarli negli analyzer.
```
