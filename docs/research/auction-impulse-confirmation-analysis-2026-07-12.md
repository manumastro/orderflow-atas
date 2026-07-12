# Auction Impulse Confirmation Analysis - 2026-07-12

## Scopo

Completare in modo diagnostico i punti 5 e 6 del ciclo A->B:

```text
5. osservare cumulative aggression dopo il pullback e prima della risoluzione;
6. confrontare M1 e M5 come dataset separati.
```

Nessun ordine, shadow live o PnL.

## Contratto Congelato

```text
NY_IMPULSE_LVN_CUMULATIVE_CONFIRMATION_V1

impulse profile A->B READY
-> barra pullback precedente alla barra RESOLVED
-> attraversa almeno un LVN raw del profilo impulso
-> EffortResult nella direzione A->B e WITH_RESULT
-> cumulative max direzionale >=30
-> cumulative max direzionale > cumulative max opposto
-> outcome futuro = EndReason del lifecycle
```

La soglia `30` deriva dal transcript New York di Fabio, non da ottimizzazione sul dataset. La barra di conferma deve avere indice strettamente minore della barra di risoluzione; in questo modo non usa una continuation gia' realizzata.

Per ridurre dipendenza intraday, il campione primario conserva solo la prima conferma per data/direzione.

Tool JSON-only:

```bash
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M1 --save
python FabioOrderFlow/tools/analyze_auction_impulse_confirmations.py --timeframe M5 --save
```

## Reload M1 Rithmic

```text
Chart range:                       2026-07-02 -> 2026-07-10
Chart bars:                        9.419
Auction bars:                      5.909
London / New York:                3.359 / 2.550
Date per sessione:                7 / 7
Cumulative ricevuti / matched:    1.645.204 / 1.370.383
Impulse READY / RESOLVED:         89 / 89
LONG / SHORT:                     47 / 42
Pullback bars AVAILABLE:          253 / 253
Entries / ShadowOrders / errori:  0 / 0 / 0
```

Risoluzioni M1:

```text
CONTINUATION_NEW_EXTREME: 35
ORIGIN_REENTRY:            39
TWO_SIDED_RANGE:           15
```

## Conferma M1

Tutte le conferme dello stesso impulso, massimo una per impulso:

```text
10 osservazioni su 5 date
LONG / SHORT:              7 / 3
Continuation / reentry:    8 / 2
Clean continuation rate:   80,0%
```

Campione primario, massimo una per data/direzione:

```text
8 osservazioni su 5 date
LONG / SHORT:              5 / 3
Continuation / reentry:    7 / 1
Clean continuation rate:   87,5%
LONG:                       5/5 continuation
SHORT:                      2/3 continuation
```

## Confronto M5 Separato

Sul benchmark M5 delle stesse date:

```text
Impulse READY:             37
Conferme:                   10
Conferme primarie:          8
Continuation / reentry:     5 / 3
Clean continuation rate:   62,5%
LONG:                       2/3 continuation
SHORT:                      3/5 continuation
```

M1 mostra una separazione descrittiva migliore, ma non e' un test indipendente: le date coincidono e gli impulsi dei due timeframe non hanno corrispondenza uno-a-uno.

## Estensione M1 a 40 Date

Il contratto e' stato rieseguito senza modifiche sul chart esteso `2026-05-18 -> 2026-07-10`:

```text
Chart bars:                       40.232
Auction records:                  27.651
London / New York:                14.348 / 13.303
Date per sessione:                40 / 40
Cumulative ricevuti / matched:    5.833.055 / 4.854.494
Impulse READY / RESOLVED:         396 / 396
Pullback AVAILABLE / MISSING:     1.095 / 37
Entries / ShadowOrders / errori:  0 / 0 / 0
```

I `37` pullback senza coverage sono concentrati su `2026-05-18` e `2026-06-26`; sono esclusi dalla conferma e non interpretati come flow zero.

Separazione congelata:

```text
Historical holdout:       SessionDate < 2026-07-02
Formalization sample:     SessionDate >= 2026-07-02
```

Historical holdout:

```text
Profile baseline:                 307 profili, 108 continuation = 35,2%
Conferme:                         13 su 9 date, 7 continuation = 53,8%
Conferme primarie:                11 su 9 date, 6 continuation = 54,5%
Primarie LONG:                    4/6 continuation
Primarie SHORT:                   2/5 continuation
Altri outcome primari:            4 origin reentry, 1 two-sided
```

Il risultato resta sopra il baseline descrittivo dei profili, ma scende nettamente rispetto a `7/8` del campione di formalizzazione. Lo storico aggiuntivo e' precedente, non prospettico, e non valida il contratto.

## Limite LVN

Tutte le barre pullback M1 e M5 attraversano almeno un LVN raw. Il profilo contiene molti minimi locali e plateau, come richiesto dal ledger senza soglia di qualificazione.

Conseguenza:

```text
TouchedLvns != NONE e' location registrata, non filtro discriminante.
```

Non scegliere un percentile minimo sul campione corrente. Il prossimo miglioramento deve registrare per ogni LVN raw metriche causali di rilevanza, ad esempio profondita' rispetto ai massimi adiacenti, distanza dal bordo e rank interno al profilo. Tutti gli LVN devono restare nel ledger.

## Decisione

```text
Validated=FALSE
PromotedToShadow=FALSE
OperationalEntry=FALSE
OrdersSubmitted=FALSE
PnL=NONE
selectionLeakage=true
```

Il contratto resta congelato per nuove date prospettiche. Lo storico aggiuntivo indebolisce il risultato iniziale: nessuna shadow live viene aperta. Servono sessioni future non usate nella formalizzazione; LONG e SHORT restano separati e nessun risultato M1 viene mescolato con M5.
