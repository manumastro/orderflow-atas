# FabioOrderFlow: Estensioni ATAS

Questa cartella contiene il codice che mette a disposizione i dati di ATAS per lo studio. Non contiene modelli, soglie o segnali. Il metodo che questo codice serve e' descritto nel [README del repository](../README.md).

## Due Modi Di Ottenere I Dati

Il progetto e' passato per due approcci successivi. Entrambi funzionano, ma servono a cose diverse.

**Il bridge, percorso attuale.** `Fabio Data Bridge` espone i dati della piattaforma su un endpoint HTTP locale. L'analisi chiede le finestre e i filtri che le servono nel momento in cui le servono, mentre ATAS resta aperto. E' il percorso da usare per qualunque studio nuovo. Contratto: [`docs/research/metodo/contratto-data-bridge.md`](../docs/research/metodo/contratto-data-bridge.md).

**I recorder, fase di agosto 2026.** Quattro indicatori che scrivono righe JSON nel log di ATAS, con lo schema deciso prima della cattura. Restano utili per cio' che il bridge non copre: il flusso live continuo e l'overlay disegnato sul chart. I loro contratti e i report prodotti sono in [`docs/research/archivio-2026-08/`](../docs/research/archivio-2026-08/).

## Le Cinque Estensioni

`src/FabioOrderFlow.slnx` compila cinque DLL indipendenti, una classe `Indicator` ciascuna. `src/FabioOrderFlow.cs` resta uno scheletro neutro e non viene distribuito.

| DLL | Sorgente | Cosa fa |
|---|---|---|
| `FabioDataBridge` | `src/Observation/DataBridge.cs` | Endpoint HTTP locale interrogabile a runtime |
| `FabioPreSessionProfileRecorder` | `src/Observation/PreSessionProfileRecorder.cs` | Profilo della pre-sessione, con overlay di sole linee sul pannello prezzi |
| `FabioSessionLocationRecorder` | `src/Observation/SessionLocationPriceResponseRecorder.cs` | Raw trade live di una sessione dichiarata, per ricostruzione offline |
| `FabioHistoricalCumulativeContextRecorder` | `src/Observation/HistoricalCumulativeContextRecorder.cs` | Candle storiche e `CumulativeTrade` degli ultimi sette giorni |
| `FabioCumulativeTradeRecorder` | `src/Observation/CumulativeTradeObservationRecorder.cs` | `CumulativeTrade`, delta di volume e footprint di barra |

Nessuna delle cinque applica filtri di dimensione, classificazioni di partecipante o logica di mercato.

## Build E Deploy

La solution compila sia su ATAS X (macOS) sia su ATAS classico (Windows): `Indicators/Directory.Build.props` risolve gli assembly dentro il bundle dell'applicazione su macOS e da `Program Files` su Windows. Il target e' `net10.0` senza WPF, requisito di ATAS X.

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Il deploy copia le cinque DLL in `~/Library/Application Support/ATAS/Indicators` su macOS, in `%APPDATA%/ATAS/Indicators` su Windows, e rimuove l'obsoleta `FabioOrderFlow.dll`. Dopo il deploy occorre riavviare ATAS perche' carichi le versioni nuove.

## Usare Il Bridge

Caricare **Fabio Data Bridge** su ogni chart da interrogare: quanti si vuole, senza configurare porte. Le istanze condividono un solo listener, che sceglie la prima porta libera fra 8787 e 8796 e la annuncia in `~/.fabio-data-bridge.json`; il client la trova da solo. Con piu' chart registrati ogni richiesta vuole `--chart`, altrimenti viene rifiutata con l'elenco dei candidati.

```bash
python3 FabioOrderFlow/tools/bridge.py charts
python3 FabioOrderFlow/tools/bridge.py health
python3 FabioOrderFlow/tools/bridge.py instrument --chart NQZ6
python3 FabioOrderFlow/tools/bridge.py limits
python3 FabioOrderFlow/tools/bridge.py candles --from 2026-09-08T13:30:00Z --to 2026-09-08T20:00:00Z --levels --out cash.json
python3 FabioOrderFlow/tools/bridge.py cumulative --from 2026-09-04 --to 2026-09-11 --min-volume 100 --out big.json
```

## Strumenti

| Script | Cosa fa |
|---|---|
| `tools/bridge.py` | Client del bridge; spezza le finestre oltre il limite di ATAS |
| `tools/describe_cash_profile.py` | Costruisce POC e area di valore della cash dal footprint scaricato |
| `tools/replay_model.py` | Riesegue il modello 40R su barre registrate, in modo causale, con le soglie dichiarate |
| `tools/build_cot_cross_index.py` | Unisce le serie COT estratte da Tradingster e calcola la metrica cross-index |
| `tools/archivio-2026-08/` | Parser dei log prodotti dai recorder nella fase di agosto |

## Vincoli ATAS Verificati

Scoperti sul campo e rispettati dal codice:

- Una sola richiesta `CumulativeTrades` pendente alla volta.
- Un solo `HttpListener` per porta: piu' istanze dell'indicatore devono condividerne uno.
- Profondita' massima **sette giorni** per richiesta, per ogni `CumulativeTradesMode`; interrogabile su `/limits`.
- `CumulativeTradesMode.Filter` non ha limite di sessione: con `minVolume 0` si ottiene il tape completo, che va pero' spezzato per minuti perche' una sola ora supera `MaxItems`.
- ATAS puo' restituire record fuori dalla finestra richiesta: vanno contati e scartati.
- `FixedProfileRequest` su ATAS X espone solo `(period)` e `(period, tradingSession)`: nessun `baseTime`.
- `CumulativeTradesRequest.Mode` e' di sola lettura, va passato al costruttore.
- Le date di rollover richiedono un chart a **contratto continuo**; su un contratto singolo ATAS risponde `Only continuous contracts are supported`.
- Su ATAS X sono vietati gli editor WPF custom. Nessuna delle cinque estensioni ne usa.

## Diario

Il diario cronologico e' [`progress.txt`](progress.txt): una riga per fase conclusa, con il documento che ne contiene i dettagli.
