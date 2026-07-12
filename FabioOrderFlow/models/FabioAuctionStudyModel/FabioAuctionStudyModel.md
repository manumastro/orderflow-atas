# FabioAuctionStudyModel

Studio causale New York A->B derivato congiuntamente da `transcription.txt` e `trascription_1.txt`.

## Stato

```text
Modalita':          NEW_YORK_IMPULSE_STUDY_NO_TRADES
Sessione:           New York 09:30-16:00 New York
Direzioni:          simmetriche LONG e SHORT
Ordini / PnL:       DISABLED
Auction-state bars: DISABLED
Output:             READY + PULLBACK + RESOLVED + cumulative shadow + path
```

Il modulo genera una shadow diagnostica: osserva il prezzo dalla chiusura della conferma, ma non invia ordini e non calcola PnL. Non esistono posizioni, stop o target. London reversion e il ledger dual-session restano baseline storiche e non sono nel percorso runtime.

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
[AUCTION_IMPULSE_SHADOW_ENTRY]       inizio osservazione, nessun ordine
[AUCTION_IMPULSE_SHADOW_PATH]        ogni barra M1 successiva fino a 30 minuti
[AUCTION_IMPULSE_SHADOW_RESOLVED]    risultato del lifecycle A->B
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

`READY` non e' una entry. `PULLBACK_BAR` registra OHLC, cioe' apertura, massimo, minimo e chiusura, oltre a location, LVN toccati, effort/result e coverage cumulative. I prezzi OHLC permettono di sapere quale confine e' stato toccato per primo senza creare ordini nel runtime.

### Raw Causal LVN Ranking V1

Ogni minimo locale raw resta nel profilo. Al `READY`, prima del pullback, riceve:

```text
VolumePercentile
AdjacentDepth / ShoulderDepth
Prominence / ProminenceRank / ProminenceRankScore
PositionInRange / DirectionalProgress
DistanceToPocRanges / DistanceToOriginRanges / DistanceToEdgeRanges
```

`ProminenceRank=1` indica il livello piu' prominente nello stesso profilo, non un livello qualificato. Plateau e livelli con depth zero restano registrati. Contratto completo: `docs/research/auction-impulse-lvn-ranking-contract-2026-07-12.md`.

## Cumulative Confirmation Shadow V1

La shadow e' abilitata soltanto su `ChartTimeFrame=M1`, cioe' un grafico con candele da un minuto. Su M5 il profilo A->B resta disponibile ma `ShadowEnabled=FALSE`, per evitare di mescolare granularita' diverse.

Conserva massimo una conferma per data New York e direzione. Richiede LVN attraversato, candela e delta coerenti con l'impulso, cumulative trade direzionale almeno `30` e maggiore dell'opposto. La barra che ha gia' risolto l'impulso e' esclusa.

Dal prezzo di chiusura della conferma segue ogni barra M1 fino a 30 minuti. `MFE` significa massima distanza favorevole raggiunta dal prezzo; `MAE` significa massima distanza contraria. Le date dal `2026-07-13` sono prospettiche, cioe' non usate per costruire la regola.

Il primo campione con 20 osservazioni, almeno 8 per direzione, decide la promozione o lo scarto. Scadenza massima: 40 sessioni New York. Contratto completo: `docs/research/ny-impulse-cumulative-shadow-contract-2026-07-12.md`.

## Boundary Risk V1

La decisione a durata fissa di 15 minuti e' superata. Il candidato `NY_IMPULSE_CONFIRMATION_BOUNDARY_RISK_V1` parte dalla stessa conferma e verifica se viene toccato prima l'estremo `B` oppure il confine di origine `A`. Se entrambi sono toccati nella stessa candela M1, il caso conta conservativamente come perdita.

Il report sottrae costi simulati di `0,5`, `1,0` e `1,5` punti e restituisce risultati in `R`, unita' di rischio, non PnL. Contratto: `docs/research/ny-impulse-boundary-risk-contract-2026-07-12.md`.

## Report

```bash
python FabioOrderFlow/tools/report_auction_impulse_ledger.py --save
python FabioOrderFlow/tools/report_auction_impulse_shadow.py --save
python FabioOrderFlow/tools/analyze_auction_impulse_boundary_risk.py --timeframe M1 --save
```

Il comando restituisce esclusivamente JSON su stdout e salva profili A->B, barre pullback e risoluzioni. Con `RAW_CAUSAL_V1` salva anche `lvns.csv` e `touched-lvns.csv`, una riga strutturata per livello/occorrenza. `report_auction_state_ledger.py` resta compatibile con snapshot dual-session precedenti e valida il summary NY-only, ma il runtime corrente non emette barre auction-state. Gli aggregati sono descrittivi, senza segnali o PnL.

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

## Analisi Boundary Risk V1 2026-07-12

Il modello simmetrico confirmation-close -> estremo B / origine A e' respinto. Con costo simulato di 1 punto, LONG produce `+0,681R` medio e profit factor `4,17`; SHORT produce circa `0R` e profit factor `1,00`, sotto il minimo `1,25`. Un caso ambiguo stessa-barra e' contato come perdita.

La media complessiva positiva non supera il requisito dichiarato di sostenibilita' in entrambe le direzioni. Nessuna raccolta prospettica simmetrica. LONG-only resta ipotesi post-hoc da formalizzare separatamente e testare su date non incluse nel campione corrente.

## Reload Cumulative Confirmation Shadow V1 2026-07-12

```text
Shadow observations / dates:      19 / 14
LONG / SHORT:                      11 / 8
Continuation / reentry / 2-sided: 13 / 5 / 1
Path bars:                         554
Path disponibili H15 / H30:       18 / 18
Prospective observations:          0
```

Il controllo software e' superato: una sola osservazione per data/direzione, progressione del percorso coerente, zero ordini ed errori. `H15` significa lettura 15 minuti dopo la conferma. Il movimento mediano H15 e' `-6,375` punti: LONG `-9,625`, SHORT `-4,125`. La mediana della massima escursione favorevole e' `57,5` punti contro `49,375` contrari, rapporto `1,165`.

In parole comuni, lo storico riconosce spesso una continuation (`13/19`) ma la chiusura scelta come inizio shadow non mostra ancora un vantaggio temporale utilizzabile. Lo storico non puo' decidere il test perche' ha contribuito alla costruzione del candidato; lo stato resta `COLLECTING` con zero osservazioni prospettiche.

## Reload Raw Causal LVN Ranking V1 2026-07-12

```text
Raw LVN / profili con LVN:       23.030 / 383
Touch occurrences:               31.676
Rank/bounds/subset errors:        0 / 0 / 0
First-pullback causal sample:     211
Continuation / reentry:          129 / 79
```

`BestRankScore` e `BestProminence` hanno AUC continuation-vs-reentry circa `0,49-0,50`: non discriminano. `MinimumDirectionalProgress` e distanza dall'origine arrivano a circa `0,63` su tutti gli impulsi, ma scendono a `0,54-0,56` usando solo la prima osservazione per data/direzione. Nessuna soglia, shadow o combinazione flow viene promossa. Analisi: `docs/research/auction-impulse-lvn-ranking-analysis-2026-07-12.md`.

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
- reload storico: circa 19 shadow di riferimento e zero prospettiche sul chart fermo al 2026-07-10;
- path progressivo e conteggi summary coerenti, senza ordini o PnL;
- nuove date dal 2026-07-13 senza modificare il contratto shadow;
- conclusione automatica al campione minimo oppure scarto per rarita' dopo 40 sessioni.
```
