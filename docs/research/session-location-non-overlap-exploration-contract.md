# Test Esplorativo: Eventi Non Sovrapposti E Location POC

## Stato

```text
Tipo:                 analisi esplorativa offline
Dataset iniziale:     NQ US Cash, 2026-08-04, fof-session-observation-v2
Modello attivo:       nessuno
Segnali / ordini:     nessuno
Runtime:              invariato
```

## Domanda

Nella raccolta disponibile, la descrizione di location rispetto al POC di sessione congelato resta diversa quando si osserva una sequenza di eventi con finestre future di 300 secondi non sovrapposte?

La domanda non chiede se un evento predice il mercato. Serve solo a separare la ripetizione dello stesso movimento di apertura da osservazioni temporalmente distinte.

## Campione E Metodo Fissati Prima Del Calcolo

Il parser accetta solo le righe `fof-session-observation-v2` che nel report precedente sono complete e hanno `tickVolumeMatchesTotal=true`.

1. Ordina gli eventi per `lastTickTimeUtc`, poi `eventId`.
2. Seleziona il primo evento idoneo.
3. Seleziona ogni evento successivo solo se il suo ultimo tick e' uguale o successivo alla fine della finestra di risposta dell'evento precedente (`ultimo tick + 300 secondi`).
4. Mantieni senza modificarle le quattro categorie gia' definite: `above-all-pocs`, `at-poc`, `below-all-pocs`, `between-tied-pocs`.
5. Riporta per ogni categoria conteggio, ritorno al POC congelato e movimento finale dopo 300 secondi. Nessuna soglia di distanza dal POC, volume o direzione viene aggiunta.

La selezione e' deterministica e riduce la sovrapposizione delle finestre, ma non rende indipendenti le osservazioni rispetto al regime della stessa giornata.

## Dati Necessari

```text
input canonico:  session-location-2026-08-04-v2-events.csv locale
schema:          fof-session-observation-v2
orizzonte:       300 secondi
unita':          stato finale di un CumulativeTrade
```

Il CSV locale e' identificato dall'hash nel report `session-location-and-price-response-description-2026-08-04.md` e non viene incluso in Git.

## Criteri

Il test e' eseguibile se esiste almeno un evento selezionato. Una categoria e' soltanto descrivibile se contiene almeno tre eventi selezionati; altrimenti il risultato e' `non valutabile` per quella categoria.

Il test viene scartato se compaiono schema diverso da v2, mismatch di volume, finestre sovrapposte o metadati mancanti. Anche un esito descrivibile non promuove un modello: per considerare un'ipotesi servono almeno cinque sessioni indipendenti raccolte con lo stesso contratto e un criterio di confronto definito prima della loro analisi.

## Output Canonico

```text
docs/research/session-location-non-overlap-exploration-2026-08-04.md
```

Il parser e' `FabioOrderFlow/tools/analyze_session_location_non_overlap.py`. Il CSV selezionato e il JSON di sintesi restano artefatti locali in `FabioOrderFlow/ledger-snapshots/`.

## Limiti

- una sola sessione e una sola fase di apertura;
- contesto d'asta precedente non ancora raccolto dal recorder;
- POC di sessione in sviluppo, non valore della sessione precedente;
- nessuna stima di probabilita', significativita' o regola operativa.
