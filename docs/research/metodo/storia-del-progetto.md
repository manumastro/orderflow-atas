# Storia Del Progetto E Discontinuita' Di Baseline

## Scopo

Questo documento estende il diario di `FabioOrderFlow/progress.txt` alla storia precedente alla baseline corrente. Serve a non confondere gli esperimenti precedenti con il percorso canonico iniziato dopo il reset del 2026-07-13.

Il dettaglio completo resta nella cronologia Git. Questa e' una sintesi delle decisioni e delle discontinuita' rilevanti, non una specifica di modello.

## Cronologia

### 2026-06-13 - Avvio Del Repository

Commit: `c7729a6` (`Initial commit: ricerca order flow ATAS`).

Il repository nasce come ambiente di ricerca per ATAS e order flow. Non esiste ancora una baseline del corso attuale ne' un modello canonico.

### 2026-06-14 - Documentazione ATAS E Primi Indicatori

Commit rilevanti: `aa1ad51` (documentazione ATAS e AGENTS), `55a9a8a` (due indicatori iniziali), `60584b8` (file logging), `7a59599` (build x64).

La documentazione API locale e le procedure di build vengono introdotte in questa fase. Gli indicatori e le ipotesi di allora non fanno parte del runtime corrente.

### 2026-06-15 - 2026-07-12 - Esperimenti Storici Ritirati

La cronologia contiene piu' iterazioni su trend following, mean reversion London, profili locali, compressione e shadow study. Esempi di commit-radice: `a09f8e0`, `68dec9a`, `5a251aa`, `f4f05b5`.

Questi lavori sono storici e non sono la baseline attiva. Le soglie, i risultati, i report e la logica operativa di quegli esperimenti non possono essere riutilizzati per il corso corrente senza una richiesta esplicita di confronto storico.

### 2026-07-13 - Reset Neutro

Commit: `a2e54a3` (`reset: return repository to neutral course baseline`). Tag: `course-neutral-baseline`.

Il runtime viene riportato a uno scheletro ATAS neutro. Modelli, output di ricerca e automazione precedenti vengono rimossi dal tree corrente e rimangono consultabili soltanto nella cronologia Git.

Questa e' la discontinuita' che separa gli esperimenti storici dalla ricerca canonica attuale.

### 2026-07-13 - Studio Del Corso E Mappa Canonica

Commit: `b01cbad` e `96bca09`.

Le tre lezioni in `fabio_course/fabio1.txt`, `fabio_course/fabio2.txt` e `fabio_course/fabio3.txt` vengono studiate come sistema integrato. La mappa `fabio_course/fabio-course-model-map.md` formalizza la sequenza contesto, bias, regime, location, partecipazione, sforzo-risultato, playbook, timing e gestione, senza attivare un modello.

### 2026-08-04 - Raccolta ATAS E Prima Descrizione

Commit: `876ff84`.

Vengono introdotti il contratto osservativo, il recorder `CumulativeTrade` separato, la validazione della cattura live/storico e il contratto descrittivo di una sessione. La descrizione completata non approva segnali, soglie, modelli o equivalenza con DeepCharts `Aggregate`.

## Baseline Corrente

La fonte attiva e' il corso in tre lezioni. Il runtime contiene soltanto lo scheletro neutro e il recorder osservativo separato. La progressione canonica corrente e' indicata da:

- `FabioOrderFlow/FabioOrderFlow.md` per stato e procedure;
- `FabioOrderFlow/progress.txt` per la lista cronologica essenziale;
- `docs/research/` per domande, dati, criteri e decisioni.
