# Acceptance Path Transition Analysis - 2026-07-11

## Scopo

Classificare il percorso completo dopo `ACCEPTANCE_CONTINUATION_V1` senza creare ordini, stop, target o PnL.

Fonte:

```text
ledger-snapshots/compression-ledger-2026-07-11_19-03-07-*
22 acceptance TradeCoverage=AVAILABLE
15 date distinte
ChartTimeFrame=M5
```

Tool ripetibile, JSON-only:

```bash
python FabioOrderFlow/tools/analyze_acceptance_path_transitions.py \
  --save FabioOrderFlow/ledger-snapshots/acceptance-path-transitions-2026-07-11.json
```

## Contratto

Una sola osservazione per profilo. La prima close interna classifica il path:

```text
CONTINUOUS_ACCEPTANCE   nessuna close path dentro il range nei 60 minuti
EARLY_REJECTION         prima close dentro entro 15 minuti
DELAYED_REJECTION       prima close dentro dopo 15 minuti
```

Tag sovrapposti, non campioni indipendenti:

```text
REJECTION_TO_POC
REACCEPTANCE_AFTER_REENTRY
OPPOSITE_BREAKOUT
```

Il directional flow delle prime 1/2/3 barre e' calcolato causalmente:

```text
signed cumulative delta nella direzione shadow / total volume
HIGH: delta positivo e' direzionale
LOW:  delta negativo e' direzionale
```

Lo split usa 10 date train e 5 date test. `selectionLeakage=true`: le classi sono state formulate dopo aver osservato lo stesso campione.

## Risultati Per Transizione

```text
CONTINUOUS_ACCEPTANCE
Profili=5 / date=5
H6 mediana=+1,350; positivi=100%
H12 mediana=+1,650; positivi=80%
POC touch=0%

EARLY_REJECTION
Profili=8 / date=6
H6 mediana=-0,400; positivi=25%
H12 mediana=-0,070; positivi=50%
POC touch=87,5%
Reacceptance successiva=100%

DELAYED_REJECTION
Profili=9 / date=8
H6 mediana=-0,300; positivi=22,2%
H12 mediana=-0,290; positivi=11,1%
POC touch=77,8%
Reacceptance successiva=55,6%
```

`CONTINUOUS_ACCEPTANCE` descrive bene i continuation winner, ma e' conoscibile solo alla fine della finestra. Non e' un trigger causale all'entry. Il semplice hold fuori range nelle prime tre barre era gia' risultato non discriminante.

`DELAYED_REJECTION` e' il descrittore piu' debole per continuation. Un rientro tardivo non equivale a un retest favorevole nel campione corrente.

## Asimmetria HIGH / LOW

```text
HIGH EARLY_REJECTION:   5 profili; H12 mediana +0,210; positivi 60%
LOW EARLY_REJECTION:    3 profili; H12 mediana -1,370; positivi 33,3%

HIGH DELAYED_REJECTION: 4 profili; H12 mediana -0,195; positivi 25%
LOW DELAYED_REJECTION:  5 profili; H12 mediana -0,290; positivi 0%
```

Il rientro dopo acceptance LOW e' piu' incompatibile con continuation rispetto a HIGH. I conteggi sono troppo piccoli per promuovere un contratto.

## POC E Opposite Breakout

```text
REJECTION_TO_POC: 14 profili / 11 date
H12 mediana=-0,290; positivi=28,6%

OPPOSITE_BREAKOUT: 5 profili / 5 date
H12 mediana=-1,420; positivi=20%
```

Il touch POC e l'uscita opposta descrivono deterioramento della continuation. Sono outcome path, non filtri disponibili sulla barra di acceptance.

## Flow Prime Tre Barre

```text
DIRECTIONAL_FLOW_POSITIVE
Profili=12; H6 mediana +0,025; H12 mediana +0,020; H12 positivi 50%

DIRECTIONAL_FLOW_NON_POSITIVE
Profili=10; H6 mediana -0,290; H12 mediana -0,290; H12 positivi 30%
```

La separazione non e' stabile nello split:

```text
Train H12 positivi: positivo 37,5%; non positivo 14,3%
Test  H12 positivi: positivo 75,0%; non positivo 66,7%
```

Per boundary:

```text
HIGH: flow positivo 50% H12 positivi; non positivo 50%
LOW:  flow positivo 50% H12 positivi; non positivo 0%
```

Il flow a tre barre non conferma una regola generale. `LOW + directional flow positivo` resta una traccia prospettica su 6 contro 4 profili, non una soglia validata.

## Decisione

```text
ValidatedTransitions=[]
ValidatedFlowConfirmations=[]
OperationalEntry=FALSE
OrderSubmitted=FALSE
PnL=NONE
```

Non modificare `ACCEPTANCE_CONTINUATION_V1`. Raccogliere nuove sessioni separando HIGH/LOW e registrare prospetticamente:

```text
LOW_ACCEPTANCE + first3BarsDirectionalFlow > 0
LOW_ACCEPTANCE + first3BarsDirectionalFlow <= 0
```

La soglia zero e' solo il segno del flow, non una soglia ottimizzata. Prima di qualsiasi promozione servono almeno 8 profili test indipendenti per gruppo e stabilita' H6/H12.
