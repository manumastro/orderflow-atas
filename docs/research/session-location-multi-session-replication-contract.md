# Replica Multi-Sessione: Location POC E Risposta Prezzo

## Stato

```text
Tipo:                 raccolta osservativa replicata
Schema richiesto:     fof-session-observation-v2
Sessioni richieste:   cinque NQ US Cash indipendenti
Modello attivo:       nessuno
Segnali / ordini:     nessuno
Runtime aggiuntivo:   nessuno
```

## Domanda

Le descrizioni di location rispetto al POC di sessione congelato e del percorso a 300 secondi restano comparabili tra cinque sessioni cash complete, invece di riflettere soltanto l'apertura del 2026-08-04?

## Raccolta Fissata

Per ciascuna delle prossime cinque sessioni idonee:

1. caricare **Fabio Session Location Recorder** prima delle 09:30 America/New_York su Mini NQ;
2. lasciare la stessa istanza attiva dalle 09:30 alle 16:00 America/New_York;
3. usare il feed che espone `MarketDataArg.Time` in UTC, senza cambiare schema, sessione, orizzonte o codice;
4. dopo la chiusura, creare snapshot v2, CSV evento, JSON e report con `report_session_location_response.py`;
5. conservare localmente gli artefatti con hash nel report, come definito in `FabioOrderFlow/ledger-snapshots/README.md`.

Una sessione resta esclusa se il primo raw trade non e' entro un secondo dalle 09:30 New York, se manca la finestra finale di 300 secondi prima delle 16:00, se il volume tick non coincide con il volume dell'evento, se mancano metadati dello strumento o se il log mescola uno schema diverso da v2.

## Confronto Ammesso Dopo La Raccolta

Il confronto verrà definito prima dell'aggregazione. Dovra' usare la stessa selezione non sovrapposta gia' definita in `session-location-non-overlap-exploration-contract.md` e mostrare ogni giornata separatamente prima di qualsiasi aggregato.

Criterio di promozione al confronto descrittivo: almeno cinque sessioni idonee, ciascuna con almeno tre finestre non sovrapposte in almeno due categorie di location. Criterio di scarto: una o piu' sessioni non soddisfano la raccolta o il confronto mostra copertura insufficiente; in quel caso si raccolgono nuove sessioni invece di modificare soglie o categorie.

Anche il confronto superato resta descrittivo. Per formulare una prima ipotesi che colleghi la location al regime servira' un contratto separato per il contesto precedente: valore della sessione passata, overnight range e posizione dell'apertura. Non verranno introdotti segnali, ordini o PnL.

## Output Previsto

```text
docs/research/session-location-multi-session-description-YYYY-MM-DD.md
```
