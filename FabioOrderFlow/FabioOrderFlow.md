# FabioOrderFlow

Base neutra per un futuro indicatore ATAS derivato dallo studio completo del corso in `../fabio_course/`.

## Stato Corrente

```text
Modello attivo:             NESSUNO
Analisi runtime:            quattro recorder osservativi separati
Richieste cumulative:       storico chart limitato agli ultimi 7 giorni + eventi live
Segnali / ordini / PnL:     NESSUNO
Output grafico:             overlay osservativo pre-sessione
```

`src/FabioOrderFlow.cs` resta uno scheletro neutro e non viene distribuito. La solution `src/FabioOrderFlow.slnx` compila quattro estensioni ATAS separate, una per recorder: `FabioCumulativeTradeRecorder.dll`, `FabioSessionLocationRecorder.dll`, `FabioHistoricalCumulativeContextRecorder.dll` e `FabioPreSessionProfileRecorder.dll`. Ogni DLL contiene una sola classe `Indicator`, quindi ATAS non riceve un contenitore comune con recorder multipli. `src/Observation/CumulativeTradeObservationRecorder.cs` registra `CumulativeTrade`, delta di volume e footprint della barra. `src/Observation/SessionLocationPriceResponseRecorder.cs` registra raw trade di una sessione dichiarata e stati `CumulativeTrade` per una futura ricostruzione offline di POC e risposta. `src/Observation/HistoricalCumulativeContextRecorder.cs` registra candle/footprint storici caricati nel chart e richiede `CumulativeTrade` storici ATAS per gli ultimi sette giorni disponibili dal fondo del chart. `src/Observation/PreSessionProfileRecorder.cs` ricostruisce dai footprint il profilo della sola pre-sessione target, con business range, POC, area di valore, delta e riferimenti descrittivi all'apertura; evidenzia la sola finestra pre-market per l'intera altezza del pannello prezzi e limita tutte le linee del profilo a quella finestra. Nessuno dei recorder applica filtri di dimensione, outcome, classificazioni o logica di mercato.

La cattura iniziale del 2026-08-04 ha promosso il registratore allo studio descrittivo di una sola sessione; il report canonico e' `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`. Questa decisione non promuove H1 sulla parita' con DeepCharts `Aggregate`.

## Progresso

Lo stato operativo e le decisioni canoniche sono descritti in questo documento e nei contratti/report richiamati. Il diario cronologico essenziale e' [progress.txt](progress.txt): ogni riga indica la fase conclusa e il file `.md` che ne contiene i dettagli. La sintesi dell'avvio del repository, degli esperimenti ritirati e del reset neutro e' `../docs/research/project-history-and-baseline.md`.

La descrizione della sessione e' completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Il risultato conferma soltanto la co-occorrenza riproducibile tra eventi aggregati e footprint di barra nella sessione osservata; non approva soglie, segnali o modelli.

La raccolta del 2026-08-04 e il report sono disciplinati da `../docs/research/session-location-and-price-response-collection-contract.md`. `Fabio Session Location Recorder` ha prodotto `57.534` eventi completi con percorso di 300 secondi; il report descrittivo e' `../docs/research/session-location-and-price-response-description-2026-08-04.md`. L'esplorazione con finestre non sovrapposte ha lasciato solo nove osservazioni e non consente un confronto tra location; il risultato e' in `../docs/research/session-location-non-overlap-exploration-2026-08-04.md`. Il case study forense sulla stessa apertura, con baseline temporale neutra e timeline a cinque minuti, e' in `../docs/research/session-forensic-case-study-2026-08-04.md`. La replica fissa su cinque sessioni complete resta definita in `../docs/research/session-location-multi-session-replication-contract.md`, ma non e' necessaria per leggere il case study come descrizione locale. Lo storico cumulato da chart ATAS e' disciplinato da `../docs/research/historical-cumulative-context-collection-contract.md`; l'inventario canonico pulito e' `../docs/research/historical-cumulative-context-inventory-2026-08-04-v5.md`, mentre `../docs/research/historical-cumulative-context-inventory-2026-08-04.md` conserva l'evidenza tecnica `v4` dei record fuori finestra restituiti da ATAS. La descrizione storica della sola cash session, con baseline candle e controllo non sovrapposto, e' `../docs/research/historical-cumulative-cash-session-description-2026-08-04.md`. Nessuno di questi passaggi approva un modello.

Il case study del profilo pre-sessione NQ del 2026-08-06 e' documentato in `../docs/research/pre-session-profile-description-2026-08-06.md`. I log ATAS verificano la ricostruzione runtime di livelli e profilo; l'overlay evidenzia la sola finestra pre-market sul pannello prezzi.

## Fonte Di Ricerca

```text
../fabio_course/fabio1.txt
../fabio_course/fabio2.txt
../fabio_course/fabio3.txt
```

Le lezioni devono essere considerate insieme. Il corso comprende piu' livelli di analisi e decisione, quindi non va ridotto preventivamente a mean reversion o continuation.

## Struttura

```text
src/FabioOrderFlow.cs                         scheletro indicatore ATAS neutro non distribuito
src/FabioOrderFlow.slnx                       solution delle quattro estensioni separate
src/Indicators/CumulativeTrade/                progetto FabioCumulativeTradeRecorder.dll
src/Indicators/SessionLocation/                progetto FabioSessionLocationRecorder.dll
src/Indicators/HistoricalCumulativeContext/    progetto FabioHistoricalCumulativeContextRecorder.dll
src/Indicators/PreSessionProfile/              progetto FabioPreSessionProfileRecorder.dll
src/Observation/CumulativeTradeObservationRecorder.cs  registratore CumulativeTrade e footprint
src/Observation/SessionLocationPriceResponseRecorder.cs recorder raw trade e CumulativeTrade di sessione
src/Observation/HistoricalCumulativeContextRecorder.cs recorder storico chart/footprint e CumulativeTrade
src/Observation/PreSessionProfileRecorder.cs             recorder profilo pre-sessione target
src/deploy.bat                                build e deploy Windows delle quattro DLL
src/deploy.sh                                 build e deploy shell delle quattro DLLdocs/atas/api/                                documentazione tecnica locale ATAS
```

Non esistono modelli direzionali, tool strategici o automazione. I contratti attivi sono `../docs/research/participation-effort-result-observation-contract.md`, `../docs/research/session-location-and-price-response-collection-contract.md`, `../docs/research/historical-cumulative-context-collection-contract.md`, `../docs/research/historical-cumulative-cash-session-description-contract.md` e `../docs/research/pre-session-profile-collection-contract.md`.

## Build

```bash
cd FabioOrderFlow/src
dotnet build FabioOrderFlow.slnx -c Release
```

## Deploy

Windows:

```bat
cd FabioOrderFlow\src
deploy.bat
```

Shell:

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Il deploy rimuove l'obsoleta `FabioOrderFlow.dll` e copia quattro DLL indipendenti in `%APPDATA%/ATAS/Indicators`:

```text
FabioCumulativeTradeRecorder.dll
FabioSessionLocationRecorder.dll
FabioHistoricalCumulativeContextRecorder.dll
FabioPreSessionProfileRecorder.dll
```

Su ATAS 8, aprire `Ctrl+I`, scegliere **Add custom indicator** e selezionare la DLL del solo recorder desiderato. Per l'overlay pre-sessione selezionare `FabioPreSessionProfileRecorder.dll`.

## Raccolta Osservativa

### Recorder Di Location Sessione

Caricare **Fabio Session Location Recorder** su un chart a 1 minuto del future Mini NQ. Non richiede proprieta' da configurare: registra soltanto la sessione fissa **NQ US Cash**, dalle `09:30` alle `16:00` sul clock `America/New_York` del feed.

Caricarlo non oltre le `09:30 America/New_York`. Scrive raw trade e stati `CumulativeTrade` nei log ATAS con prefisso `FofSessionObservation`; non calcola POC nel chart, non mostra marker e non produce segnali. `MarketDataArg.Time` viene conservato come UTC e il recorder aggiunge il corrispondente tempo di sessione America/New_York, inclusa l'ora legale. La ricostruzione offline di POC e percorso di 300 secondi della raccolta del 2026-08-04 e' in `../docs/research/session-location-and-price-response-description-2026-08-04.md`; un feed che non espone UTC richiede un nuovo contratto invece di un adattamento automatico.

### Recorder Storico Cumulative Context

Caricare **Fabio Historical Cumulative Context Recorder** su un chart del future Mini NQ con il periodo storico gia' caricato. Il recorder usa l'ultima candle caricata come fine e cattura al massimo i sette giorni calendario precedenti, cosi' eventuale storico extra precaricato da ATAS non amplia la richiesta cumulativa.

Il recorder scrive righe JSON con prefisso `FofHistoricalContext` e schema `fof-historical-cumulative-context-v5`: candle/footprint storici del capture range, value area, VWAP, POC di candle, livelli footprint e risposta storica `CumulativeTrade` con ticks interni. Mantiene una sola richiesta ATAS pendente alla volta e logga solo eventi con `trade.Time` dentro la finestra richiesta; la risposta conteggia anche record restituiti e scartati. Il report canonico `../docs/research/historical-cumulative-context-inventory-2026-08-04-v5.md` documenta la cattura pulita; il precedente inventario `v4` resta evidenza del comportamento ATAS fuori finestra. Non riceve raw trade storici arbitrari e quindi non sostituisce il recorder live `fof-session-observation-v2`; serve a costruire case study storici con contesto d'asta da candle e aggregati ATAS.

Il contratto e' `../docs/research/historical-cumulative-context-collection-contract.md`.

### Recorder Profilo Pre-Sessione

Caricare **Fabio Pre-Session Profile Recorder** su un chart a 1 minuto del future Mini NQ con footprint disponibile. Il recorder analizza una sola sessione target e usa il clock `America/New_York`, interpretando i timestamp candle del feed verificato come UTC.

La finestra primaria e' `18:00` ET del giorno precedente fino a `09:30` ET del giorno analizzato. Il massimo e il minimo dei livelli footprint formano il business range. Il log contiene volume per prezzo, POC, area di valore al 70%, delta estremo, distribuzione descrittiva e un confronto London `03:00-09:30` ET. Dopo l'apertura aggiunge il riferimento della prima candle e riepiloghi fattuali a 5 e 15 minuti.

Sul chart prezzi evidenzia esclusivamente la finestra temporale `18:00-09:30 ET` per tutta l'altezza del pannello. Durante il pre-market mostra i business high/low in sviluppo. Alla prima candle `09:30` ET, se il chart ha copertura completa, sostituisce i valori provvisori con business high/low, POC, VAL, VAH e gli estremi delta grezzi sull'intero e solo intervallo pre-market; nessuna linea o fascia viene prolungata nella sessione cash. Non ricrea l'istogramma del Fixed Profile nativo, non emette segnali, alert o ordini. I campi `HasStartBoundary` e `HasEndBoundary` dichiarano se il chart conteneva entrambi i bordi della finestra; in caso contrario non disegna i livelli finali e scrive `incomplete`.

Il contratto e' `../docs/research/pre-session-profile-collection-contract.md`.

### Recorder Cumulative Trade

Caricare **Fabio Cumulative Trade Recorder** sul chart del future Mini. Il recorder non disegna nulla e usa il log standard ATAS con righe JSON prefissate da `FofObservation`.

Al termine del caricamento chiede lo storico `CumulativeTrade` dell'intervallo visibile solo se questo copre al massimo sette giorni; per intervalli piu' estesi emette `historical-request-skipped` senza troncare i dati. Rimane il recorder adatto a verificare composizione, aggiornamenti e footprint del singolo evento.

Il contratto osservativo originario e' `../docs/research/participation-effort-result-observation-contract.md`; la cattura tecnica e' documentata in `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`.

La fase descrittiva e' definita da `../docs/research/cumulative-trade-footprint-description-contract.md` e completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Senza DeepCharts, nessun confronto marker-per-marker e nessuna equivalenza con `Aggregate` sono dichiarabili; il report descrive soltanto i campi ATAS gia' raccolti.
