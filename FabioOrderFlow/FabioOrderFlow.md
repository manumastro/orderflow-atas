# FabioOrderFlow

Base neutra per un futuro indicatore ATAS derivato dallo studio completo del corso in `../fabio_course/`.

## Stato Corrente

```text
Modello attivo:             NESSUNO
Analisi runtime:            due recorder osservativi separati
Richieste cumulative:       storico chart fino a 7 giorni + eventi live
Segnali / ordini / PnL:     NESSUNO
Output grafico:             NESSUNO
```

`src/FabioOrderFlow.cs` resta uno scheletro neutro. `src/Observation/CumulativeTradeObservationRecorder.cs` registra `CumulativeTrade`, delta di volume e footprint della barra. `src/Observation/SessionLocationPriceResponseRecorder.cs` registra raw trade di una sessione dichiarata e stati `CumulativeTrade` per una futura ricostruzione offline di POC e risposta. Nessuno dei due applica filtri di dimensione, outcome, classificazioni o logica di mercato.

La cattura iniziale del 2026-08-04 ha promosso il registratore allo studio descrittivo di una sola sessione; il report canonico e' `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`. Questa decisione non promuove H1 sulla parita' con DeepCharts `Aggregate`.

## Progresso

Lo stato operativo e le decisioni canoniche sono descritti in questo documento e nei contratti/report richiamati. Il diario cronologico essenziale e' [progress.txt](progress.txt): ogni riga indica la fase conclusa e il file `.md` che ne contiene i dettagli. La sintesi dell'avvio del repository, degli esperimenti ritirati e del reset neutro e' `../docs/research/project-history-and-baseline.md`.

La descrizione della sessione e' completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Il risultato conferma soltanto la co-occorrenza riproducibile tra eventi aggregati e footprint di barra nella sessione osservata; non approva soglie, segnali o modelli.

La prossima fase e' definita e approvata in `../docs/research/session-location-and-price-response-collection-contract.md`. `Fabio Session Location Recorder` e' implementato e attende una nuova raccolta live: nessuna location di POC o risposta prezzo e' ancora stata calcolata o interpretata.

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
src/FabioOrderFlow.csproj                     configurazione build
src/deploy.bat                                build e deploy Windows
src/deploy.sh                                 build e deploy da shell
docs/atas/api/                                documentazione tecnica locale ATAS
```

Non esistono modelli direzionali, tool strategici o automazione. I contratti attivi sono `../docs/research/participation-effort-result-observation-contract.md` e `../docs/research/session-location-and-price-response-collection-contract.md`.

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

Caricarlo non oltre le `09:30 America/New_York`. Scrive raw trade e stati `CumulativeTrade` nei log ATAS con prefisso `FofSessionObservation`; non calcola POC nel chart, non mostra marker e non produce segnali. La ricostruzione di POC e percorso di 300 secondi avverra' solo dopo che una sessione completa soddisfera' il contratto `../docs/research/session-location-and-price-response-collection-contract.md`. Se il feed non espone `MarketDataArg.Time` sul clock America/New_York, la raccolta va scartata invece di adattare orari nel recorder.

### Recorder Cumulative Trade

Caricare **Fabio Cumulative Trade Recorder** sul chart del future Mini. Il recorder non disegna nulla e usa il log standard ATAS con righe JSON prefissate da `FofObservation`.

Al termine del caricamento chiede lo storico `CumulativeTrade` dell'intervallo visibile solo se questo copre al massimo sette giorni; per intervalli piu' estesi emette `historical-request-skipped` senza troncare i dati. Rimane il recorder adatto a verificare composizione, aggiornamenti e footprint del singolo evento.

Il contratto osservativo originario e' `../docs/research/participation-effort-result-observation-contract.md`; la cattura tecnica e' documentata in `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`.

La fase descrittiva e' definita da `../docs/research/cumulative-trade-footprint-description-contract.md` e completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Senza DeepCharts, nessun confronto marker-per-marker e nessuna equivalenza con `Aggregate` sono dichiarabili; il report descrive soltanto i campi ATAS gia' raccolti.
