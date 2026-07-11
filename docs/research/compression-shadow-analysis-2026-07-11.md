# Compression Shadow Analysis - 2026-07-11

## Contract

Analisi offline sullo snapshot esteso, senza modifica al modello ATAS.

```text
Universo:       soli profili TradeCoverage=AVAILABLE
Indipendenza:   massimo una osservazione per profilo
Entry shadow:   close del primo evento al bordo dopo READY
Direzione:      verso l'interno del range congelato
Exit:           close esatta dopo 6 o 12 barre
Stop/target:    non ricostruiti
Ordini/PnL:     nessuno
Costi:          non disponibili
```

Stop e target non sono simulati perche' MFE/MAE non espongono l'ordine
intrabar dei livelli. Il fixed-horizon close e' invece deterministico.

Report JSON:

```text
FabioOrderFlow/ledger-snapshots/compression-shadow-2026-07-11.json
```

## Split

```text
Profili selezionati: 24
Date distinte:       16
Train cronologico:   11 date, 17 profili
Test cronologico:     5 date,  7 profili
```

Il test non e' un holdout puro: le idee candidate erano gia' state osservate
su uno snapshot precedente sovrapposto. Il report dichiara
`selectionLeakage=true`.

## Results

Media del close normalizzato verso la reversione, in unita' di range:

```text
Candidate                            Train H6  Test H6  Train H12  Test H12
BASELINE_FIRST_EVENT_REVERSION         -0,015   +0,104      +0,312    -0,586
HIGH_BOUNDARY_REVERSION                +0,233   -0,222      +0,440    -1,040
LOW_BOUNDARY_REVERSION                 -0,294   +0,920      +0,168    +0,550
CLOSE_INSIDE_REVERSION                 +0,219       NA      +0,774        NA
RELATIVE_VOLUME_TOP_QUARTILE           +0,123   +0,187      +0,481    -1,133
ABS_DELTA_TOP_QUARTILE                 +0,488   -0,355      +1,108    -2,045
COMPACT_GEOMETRY                       -0,488   +0,288      -0,134    -0,002
EXTENDED_GEOMETRY                      +0,518   -0,355      +0,814    -2,045
```

Conteggi test rilevanti:

```text
Baseline:       7 profili
High:           5 profili
Low:            2 profili
Close inside:   0 profili
Volume alto:    3 profili
Delta alto:     2 profili
Compact:        5 profili
Extended:       2 profili
```

## Decision

Nessun candidato e' stabile tra 6 e 12 barre o sufficientemente popolato nel
blocco test. Nessuno raggiunge il minimo esplorativo interno di 8 profili test.
Il lato LOW appare positivo soltanto su due casi; non e' evidenza.

```text
ValidatedCandidates=[]
OperationalEntries=DISABLED
Shadow promotion=REJECTED_FOR_NOW
```

Con i dati disponibili e' stato eseguito tutto cio' che e' ricostruibile senza
inventare execution. Il prossimo dato utile e' una nuova sessione indipendente,
non un altro filtro sul campione corrente.
