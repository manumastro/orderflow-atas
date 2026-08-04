# Esplorazione: Location POC Con Eventi Non Sovrapposti

## Stato

```text
Tipo:                 analisi esplorativa offline
Sessione:             NQ US Cash 2026-08-04
Schema incluso:       fof-session-observation-v2
Schema escluso:       fof-session-observation-v1
Modello attivo:       nessuno
Segnali / ordini:     nessuno
```

## Metodo Fissato

Il test usa solo eventi completi con volume tick coerente e metadati dello strumento. Li ordina per ultimo tick e seleziona il primo evento disponibile, poi il primo evento il cui ultimo tick non precede la fine dei 300 secondi dell'evento precedente selezionato. Le finestre future selezionate non si sovrappongono.

```text
CSV input locale:          FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-events.csv
SHA-256 CSV input:         8c145ce67b4061dc62667fa4a1a02a8f0290bb9452296705c696ff25829cda7c
CSV selezionato locale:    FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-non-overlap-events.csv
SHA-256 CSV selezionato:   15d87604f201a8ad3e08916c42472a9ed941795c3f90e52bb3b2a9aefe5fb548
eventi input:              61590
eventi idonei:             57534
eventi non sovrapposti:    9
primo ultimo tick UTC:     2026-08-04T13:30:00.0004456
ultimo ultimo tick UTC:    2026-08-04T14:10:00.0247407
selezione rifiutata per sovrapposizione: 57525
```

## Esito Del Test

La selezione ha lasciato `9` finestre non sovrapposte: `7` sopra tutti i POC, `1` sul POC, `1` sotto tutti i POC e `0` tra POC in parita'. Le categorie con almeno tre osservazioni sono: `above-all-pocs`. Se meno di due categorie sono descrivibili, il dataset non permette un confronto tra location. Qualunque prevalenza di segno in una categoria resta compatibile con il movimento generale della stessa apertura.

## Risultati Descrittivi

```text
above-all-pocs:
  eventi selezionati: 7
  descrivibile: true
  ritorno POC: 2
  nessun ritorno POC: 5
  mediana prezzo finale (tick): 89
  finale positivo / negativo / zero: 6 / 1 / 0
at-poc:
  eventi selezionati: 1
  descrivibile: false
  ritorno POC: 1
  nessun ritorno POC: 0
  mediana prezzo finale (tick): 592
  finale positivo / negativo / zero: 1 / 0 / 0
below-all-pocs:
  eventi selezionati: 1
  descrivibile: false
  ritorno POC: 1
  nessun ritorno POC: 0
  mediana prezzo finale (tick): 74
  finale positivo / negativo / zero: 1 / 0 / 0
between-tied-pocs:
  eventi selezionati: 0
  descrivibile: false
  ritorno POC: 0
  nessun ritorno POC: 0
  mediana prezzo finale (tick): n/a
  finale positivo / negativo / zero: 0 / 0 / 0
```

La categoria e' `descrivibile` solo da tre eventi selezionati in su. In questo test la parola non significa statisticamente valida, predittiva o utilizzabile per un trade. Le osservazioni appartengono alla stessa apertura e possono condividere contesto e regime anche quando le finestre di risposta non si sovrappongono.

## Conclusione

Il test rimuove la sovrapposizione meccanica dei percorsi di cinque minuti, ma non sostituisce la replica su sessioni indipendenti. Non approva soglie, probabilita', filtro, segnale o modello. Il prossimo confronto ammesso richiede almeno cinque sessioni complete raccolte con lo stesso schema e un contesto d'asta precedente definito prima dell'analisi.
