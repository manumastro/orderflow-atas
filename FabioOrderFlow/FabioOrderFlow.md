# FabioOrderFlow

Base neutra per un futuro indicatore ATAS derivato dallo studio completo del corso in `../fabio_course/`.

## Stato Corrente

```text
Modello attivo:             NESSUNO
Analisi runtime:            registratore osservativo separato
Richieste cumulative:       storico chart fino a 7 giorni + eventi live
Segnali / ordini / PnL:     NESSUNO
Output grafico:             NESSUNO
```

`src/FabioOrderFlow.cs` resta uno scheletro neutro. `src/Observation/CumulativeTradeObservationRecorder.cs` e' l'unico runtime di ricerca ammesso: registra eventi `CumulativeTrade` e il loro delta di volume, footprint della barra, tick costituenti e identificativo dello strumento/connettore nei log ATAS. Non applica filtri di dimensione, outcome, classificazioni o logica di mercato.

La cattura iniziale del 2026-08-04 ha promosso il registratore allo studio descrittivo di una sola sessione; il report canonico e' `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`. Questa decisione non promuove H1 sulla parita' con DeepCharts `Aggregate`.

## Progresso

Lo stato operativo e le decisioni canoniche sono descritti in questo documento e nei contratti/report richiamati. Il diario cronologico essenziale e' [progress.txt](progress.txt): ogni riga indica la fase conclusa e il file `.md` che ne contiene i dettagli.

La descrizione della sessione e' completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Il risultato conferma soltanto la co-occorrenza riproducibile tra eventi aggregati e footprint di barra nella sessione osservata; non approva soglie, segnali o modelli. Un eventuale passo successivo richiede un nuovo contratto per location di profilo e risposta futura del prezzo.

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
src/Observation/CumulativeTradeObservationRecorder.cs  registratore CumulativeTrade
src/FabioOrderFlow.csproj                     configurazione build
src/deploy.bat                                build e deploy Windows
src/deploy.sh                                 build e deploy da shell
docs/atas/api/                                documentazione tecnica locale ATAS
```

Non esistono modelli direzionali, tool strategici o automazione. Il contratto osservativo attivo e' `../docs/research/participation-effort-result-observation-contract.md`.

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

Caricare **Fabio Cumulative Trade Recorder** sul chart del future Mini da confrontare con DeepCharts. Il recorder non disegna nulla e usa il log standard ATAS con righe JSON prefissate da `FofObservation`.

Al termine del caricamento chiede lo storico `CumulativeTrade` dell'intervallo visibile solo se questo copre al massimo sette giorni; per intervalli piu' estesi emette un record `historical-request-skipped` senza troncare i dati. Per il confronto, registrare esternamente le impostazioni DeepCharts, inclusi `Base Dati = Aggregate Trades`, Min/Max, strumento, contratto, sessione e fuso orario.

Il contratto osservativo originario e' `../docs/research/participation-effort-result-observation-contract.md`; la cattura tecnica e' documentata in `../docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`.

La fase descrittiva e' definita da `../docs/research/cumulative-trade-footprint-description-contract.md` e completata in `../docs/research/cumulative-trade-footprint-description-2026-08-04.md`. Senza DeepCharts, nessun confronto marker-per-marker e nessuna equivalenza con `Aggregate` sono dichiarabili; il report descrive soltanto i campi ATAS gia' raccolti.
