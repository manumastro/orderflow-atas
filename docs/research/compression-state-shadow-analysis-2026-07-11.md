# Compression State Shadow Analysis - 2026-07-11

## Purpose

Il primo evento al bordo non distingue rifiuto e acceptance. Questa analisi
genera entry shadow causali da due stati gia' presenti nel ledger, senza usare
`EndReason` futuro.

```text
FAILED_BREAKOUT_REVERSION
prior OUTSIDE sul bordo -> evento successivo sullo stesso bordo chiude INSIDE
-> entry al close verso range/POC

ACCEPTANCE_CONTINUATION
close OUTSIDE con OutsideCloseStreak >= 2
-> entry al close nella direzione esterna
```

Regole comuni:

```text
massimo una entry per profilo e contratto
exit close a 6 e 12 barre
nessuno stop/target ricostruito
nessun ordine o PnL
split cronologico 11 date train / 5 date test
selectionLeakage=true
```

Report completo con ogni entry:

```text
FabioOrderFlow/ledger-snapshots/compression-state-shadow-2026-07-11.json
```

## Results

```text
Contract                     Profiles  Train H6  Test H6  Train H12  Test H12
ACCEPTANCE_CONTINUATION          22       +0,040   +0,147     -0,332    +0,870
FAILED_BREAKOUT_REVERSION         8       -0,162   +0,070     -0,426    -0,700
FIRST_CONFIRMED_STATE            22       -0,183   +0,147     -0,333    +0,870
```

`FAILED_BREAKOUT_REVERSION` non mostra stabilita' e rimane negativo a 12 barre
sia train sia test.

Il test continuation contiene 7 profili:

```text
HIGH acceptance: 6
LOW acceptance:  1
```

Per HIGH acceptance nel test:

```text
H6:  media +0,300; mediana -0,010
H12: media +0,938; mediana +0,560
H12 senza outlier massimo +3,38: media +0,450
```

Le entry HIGH H12 test sono:

```text
2026-07-02  +3,38
2026-07-03  +0,91
2026-07-03  -0,35
2026-07-06  +0,21
2026-07-08  -0,17
2026-07-08  +1,65
```

Nel train, HIGH acceptance H12 ha media `-0,213`, mediana `-0,265` e positive
rate `33,3%`. La divergenza train/test puo' indicare regime recente o rumore;
non dimostra un edge generale.

## Decision

```text
Failed breakout reversion: rejected on current sample.
Acceptance continuation:   retain as frozen prospective shadow hypothesis.
ValidatedContracts:         []
OperationalEntries:         DISABLED
```

La prossima verifica utile non e' ottimizzare altre soglie sugli stessi dati.
E' registrare prospetticamente la stessa acceptance continuation senza
cambiarne il contratto, distinguendo HIGH e LOW e confrontando H6/H12 sulle
nuove sessioni.
