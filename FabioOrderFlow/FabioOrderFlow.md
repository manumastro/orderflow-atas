# FabioOrderFlow

Base neutra per un futuro indicatore ATAS derivato dallo studio completo del corso in `../fabio_course/`.

## Stato Corrente

```text
Modello attivo:             NESSUNO
Analisi runtime:            NESSUNA
Richieste cumulative:       NESSUNA
Segnali / ordini / PnL:     NESSUNO
Output grafico:             NESSUNO
```

L'indicatore compila e puo' essere caricato in ATAS, ma `OnCalculate` non applica logica di mercato. Questa neutralita' e' intenzionale: il prossimo modello deve nascere dallo studio del corso, non dai modelli rimossi.

## Fonte Di Ricerca

```text
../fabio_course/fabio1.txt
../fabio_course/fabio2.txt
../fabio_course/fabio3.txt
```

Le lezioni devono essere considerate insieme. Il corso comprende piu' livelli di analisi e decisione, quindi non va ridotto preventivamente a mean reversion o continuation.

## Struttura

```text
src/FabioOrderFlow.cs       scheletro indicatore ATAS
src/FabioOrderFlow.csproj   configurazione build
src/deploy.bat              build e deploy Windows
src/deploy.sh               build e deploy da shell
docs/atas/api/              documentazione tecnica locale ATAS
```

Non esistono directory `models/`, tool strategici, snapshot di ricerca o contratti attivi.

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

Dopo il deploy, rimuovere e riaggiungere l'indicatore al chart oppure riavviare ATAS per caricare la DLL neutra.

## Prossima Fase

La prossima fase e' documentale, non implementativa:

1. costruire una mappa completa dei concetti del corso;
2. distinguere dati osservabili, interpretazioni discrezionali e regole misurabili;
3. identificare quali informazioni ATAS puo' fornire causalmente;
4. soltanto dopo proporre candidati di modello separati e verificabili.
