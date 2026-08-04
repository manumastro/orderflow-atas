# FabioOrderFlow

Base neutra per un futuro indicatore ATAS derivato dallo studio completo del corso in `../fabio_course/`.

## Stato Corrente

```text
Modello attivo:             NESSUNO
Analisi runtime:            tre recorder osservativi separati
Richieste cumulative:       storico chart fino a 7 giorni + eventi live
Segnali / ordini / PnL:     NESSUNO
Output grafico:             NESSUNO
```

`src/FabioOrderFlow.cs` resta uno scheletro neutro. `src/Observation/CumulativeTradeObservationRecorder.cs` registra `CumulativeTrade`, delta di volume e footprint della barra. `src/Observation/SessionLocationPriceResponseRecorder.cs` registra raw trade di una sessione dichiarata e stati `CumulativeTrade` per una futura ricostruzione offline di POC e risposta. `src/Observation/HistoricalCumulativeContextRecorder.cs` registra candle/footprint storici caricati nel chart e richiede `CumulativeTrade` storici ATAS sullo stesso range. Nessuno dei recorder applica filtri di dimensione, outcome, classificazioni o logica di mercato.

La cattura iniziale del 2026-08-04 ha promosso il registratore allo studio descrittivo di una sola sessione; il report canonico e' `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`. Questa decisione non promuove H1 sulla parita' con DeepCharts `Aggregate`.

## Progresso

Lo stato operativo e le decisioni canoniche sono descritti in questo documento e nei contratti/report richiamati. Il diario cronologico essenziale e' [progress.txt](progress.txt): ogni riga indica la fase conclusa e il file `.md` che ne contiene i dettagli. La sintesi dell'avvio del repository, degli esperimenti ritirati e del reset neutro e' `../docs/research/project-history-and-baseline.md`.

La descrizione della sessione e' completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Il risultato conferma soltanto la co-occorrenza riproducibile tra eventi aggregati e footprint di barra nella sessione osservata; non approva soglie, segnali o modelli.

La raccolta del 2026-08-04 e il report sono disciplinati da `../docs/research/session-location-and-price-response-collection-contract.md`. `Fabio Session Location Recorder` ha prodotto `57.534` eventi completi con percorso di 300 secondi; il report descrittivo e' `../docs/research/session-location-and-price-response-description-2026-08-04.md`. L'esplorazione con finestre non sovrapposte ha lasciato solo nove osservazioni e non consente un confronto tra location; il risultato e' in `../docs/research/session-location-non-overlap-exploration-2026-08-04.md`. Il case study forense sulla stessa apertura, con baseline temporale neutra e timeline a cinque minuti, e' in `../docs/research/session-forensic-case-study-2026-08-04.md`. La replica fissa su cinque sessioni complete resta definita in `../docs/research/session-location-multi-session-replication-contract.md`, ma non e' necessaria per leggere il case study come descrizione locale. Lo storico cumulato da chart ATAS e' disciplinato da `../docs/research/historical-cumulative-context-collection-contract.md`. Nessuno di questi passaggi approva un modello.

## Fonte Di Ricerca

```text
../fabio_course/fabio1.txt
../fabio_course/fabio2.txt
../fabio_course/fabio3.txt
```

Le lezioni devono essere considerate insieme. Il corso comprende piu' livelli di analisi e decisione, quindi non va ridotto preventivamente a mean reversion o continuation.

## Struttura

```text
src/FabioOrderFlow.cs                         scheletro indicatore ATAS neutro
src/Observation/CumulativeTradeObservationRecorder.cs  registratore CumulativeTrade e footprint
src/Observation/SessionLocationPriceResponseRecorder.cs recorder raw trade e CumulativeTrade di sessione
src/Observation/HistoricalCumulativeContextRecorder.cs recorder storico chart/footprint e CumulativeTrade
src/FabioOrderFlow.csproj                     configurazione build
src/deploy.bat                                build e deploy Windows
src/deploy.sh                                 build e deploy da shell
docs/atas/api/                                documentazione tecnica locale ATAS
```

Non esistono modelli direzionali, tool strategici o automazione. I contratti attivi sono `../docs/research/participation-effort-result-observation-contract.md`, `../docs/research/session-location-and-price-response-collection-contract.md` e `../docs/research/historical-cumulative-context-collection-contract.md`.

## Build

```bash
cd FabioOrderFlow/src
dotnet build -c Release
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

Il deploy copia la DLL in:

```text
%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll
```

Dopo il deploy, rimuovere e riaggiungere l'indicatore al chart oppure riavviare ATAS per caricare la DLL aggiornata.

## Raccolta Osservativa

### Recorder Di Location Sessione

Caricare **Fabio Session Location Recorder** su un chart a 1 minuto del future Mini NQ. Non richiede proprieta' da configurare: registra soltanto la sessione fissa **NQ US Cash**, dalle `09:30` alle `16:00` sul clock `America/New_York` del feed.

Caricarlo non oltre le `09:30 America/New_York`. Scrive raw trade e stati `CumulativeTrade` nei log ATAS con prefisso `FofSessionObservation`; non calcola POC nel chart, non mostra marker e non produce segnali. `MarketDataArg.Time` viene conservato come UTC e il recorder aggiunge il corrispondente tempo di sessione America/New_York, inclusa l'ora legale. La ricostruzione offline di POC e percorso di 300 secondi della raccolta del 2026-08-04 e' in `../docs/research/session-location-and-price-response-description-2026-08-04.md`; un feed che non espone UTC richiede un nuovo contratto invece di un adattamento automatico.

### Recorder Storico Cumulative Context

Caricare **Fabio Historical Cumulative Context Recorder** su un chart del future Mini NQ con il periodo storico gia' caricato. Il range deve essere al massimo di sette giorni, limite documentato da ATAS per `CumulativeTradesRequest`.

Il recorder scrive righe JSON con prefisso `FofHistoricalContext`: candle/footprint storici del chart, value area, VWAP, POC di candle, livelli footprint e risposta storica `CumulativeTrade` con ticks interni. Non riceve raw trade storici arbitrari e quindi non sostituisce il recorder live `fof-session-observation-v2`; serve a costruire case study storici con contesto d'asta da candle e aggregati ATAS.

Il contratto e' `../docs/research/historical-cumulative-context-collection-contract.md`.

### Recorder Cumulative Trade

Caricare **Fabio Cumulative Trade Recorder** sul chart del future Mini. Il recorder non disegna nulla e usa il log standard ATAS con righe JSON prefissate da `FofObservation`.

Al termine del caricamento chiede lo storico `CumulativeTrade` dell'intervallo visibile solo se questo copre al massimo sette giorni; per intervalli piu' estesi emette `historical-request-skipped` senza troncare i dati. Rimane il recorder adatto a verificare composizione, aggiornamenti e footprint del singolo evento.

Il contratto osservativo originario e' `../docs/research/participation-effort-result-observation-contract.md`; la cattura tecnica e' documentata in `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`.

La fase descrittiva e' definita da `../docs/research/cumulative-trade-footprint-description-contract.md` e completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Senza DeepCharts, nessun confronto marker-per-marker e nessuna equivalenza con `Aggregate` sono dichiarabili; il report descrive soltanto i campi ATAS gia' raccolti.
