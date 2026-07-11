# Fabio Auction Playbooks Analysis - 2026-07-11

## Scopo

Ricostruire causalmente dai due transcript due famiglie, senza ordini o PnL:

```text
BALANCE_ROTATION_V1
assorbimento fuori value -> rientro -> aggressione opposta -> POC

NY_IMBALANCE_PULLBACK_V1
expansion da value -> pullback a boundary/LVN -> aggressione con risultato -> continuation
```

Fonte:

```text
ledger-snapshots/auction-state-2026-07-11_23-14-29-bars.csv
```

Tool JSON-only:

```bash
python FabioOrderFlow/tools/analyze_fabio_auction_playbooks.py \
  --save FabioOrderFlow/ledger-snapshots/fabio-auction-playbooks-2026-07-11.json
```

## Contratto

```text
Massimo una osservazione per modello/sessione/data/direzione.
Split cronologico: train fino al 2026-06-23, test dopo.
Cumulative big trade transcript: London >=20, New York >=30 per BALANCE_ROTATION.
NY continuation: directional cumulative max maggiore dell'opposto, senza soglia ottimizzata.
Outcome: H3/H6/H12 normalizzati per Prior12Range.
OperationalEntry=FALSE; OrderSubmitted=FALSE; PnL=NONE.
selectionLeakage=true: i contratti sono stati formalizzati dopo aver visto il campione.
```

## Dataset

```text
Auction bars:       6.488
Date London:        40
Date New York:      40
Cumulative matched: 4.854.494 / 5.833.055
Osservazioni:       73

BALANCE_ROTATION_V1:       32
NY_IMBALANCE_PULLBACK_V1:  41
LONG:                       39
SHORT:                      34
```

## Risultati Principali

### NY Imbalance Pullback Long

```text
Train H6: 19 outcome, mediana +0,342 range, positivi 78,9%
Test H6:   8 outcome, mediana -0,098 range, positivi 37,5%
```

Il lato LONG non regge lo split cronologico. L'evidenza aggregata positiva era train-dominated.

### NY Imbalance Pullback Short

```text
Train H6: 7 outcome, mediana +0,123 range, positivi 71,4%
Test H6:  6 outcome, mediana +0,084 range, positivi 66,7%

Test H3:  mediana -0,023, positivi 50,0%
Test H12: mediana +0,176, positivi 50,0%
```

H6 e' la traccia piu' coerente, ma deriva da soli 13 casi complessivi. La debolezza H3/H12 non permette una promozione.

### Balance Rotation London

```text
SHORT train H6: 7 outcome, mediana +0,063, positivi 57,1%
SHORT test H6:  5 outcome, mediana +0,175, positivi 80,0%
LONG train H6:  4 outcome, mediana +0,083, positivi 75,0%
LONG test H6:   2 outcome, mediana -0,352, positivi 0,0%
```

Il lato LONG fallisce; SHORT resta troppo piccolo e degrada a H12. Il POC viene spesso toccato entro H12, ma il close finale puo' aver gia' invertito: target touch e directional hold sono fenomeni distinti.

### Balance Rotation New York

```text
LONG:  4 osservazioni
SHORT: 7 osservazioni
```

Campione insufficiente. SHORT e' negativo nel test a H3/H6/H12. Non e' corretto generalizzare il trade VAL->VAH mostrato nel singolo live.

## Interpretazione

La correzione concettuale derivata dal secondo transcript resta valida:

```text
stato d'asta -> location -> aggressione -> risultato
```

La sessione non basta a scegliere reversion o continuation. Nel live New York Fabio esegue prima una rotation da value edge e poi momentum dopo l'espansione.

Il ledger ha risolto il problema di osservabilita', ma il proxy corrente ha limiti:

```text
- M5 aggrega sequenze che Fabio legge su M1 o range chart;
- il rolling LVN a 12 barre non equivale ancora al profilo causale dell'impulso A->B;
- Prior12Efficiency non definisce da sola BALANCE/IMBALANCE;
- nessun contratto conserva ancora la struttura precisa dell'impulso e del suo retest.
```

## Decisione

```text
ValidatedModels=[]
OperationalEntries=DISABLED
ShadowPromotion=NONE
```

Non aggiungere filtri ricavati dai risultati. Prossimo ciclo isolato:

```text
1. costruire il profilo causale dell'impulso A->B durante New York;
2. congelare i suoi LVN prima del pullback;
3. applicare lo studio a chart M1 e mantenere M1/M5 separati;
4. confrontare prospetticamente LONG e SHORT, H3/H6/H12;
5. mantenere NY SHORT H6 come traccia diagnostica, non come segnale.
```
