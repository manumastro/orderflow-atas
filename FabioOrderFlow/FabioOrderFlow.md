# FabioOrderFlow: Estensioni ATAS

Questa cartella contiene il codice che mette a disposizione i dati di ATAS per lo studio. Non contiene modelli, soglie o segnali. Il metodo che questo codice serve e' descritto nel [percorso del progetto](../docs/research/percorso-del-progetto.md).

## Due Modi Di Ottenere I Dati

Il progetto e' passato per due approcci successivi. Entrambi funzionano, ma servono a cose diverse.

**Il bridge, percorso attuale.** `Fabio Data Bridge` espone i dati della piattaforma su un endpoint HTTP locale. L'analisi chiede le finestre e i filtri che le servono nel momento in cui le servono, mentre ATAS resta aperto. E' il percorso da usare per qualunque studio nuovo. Contratto: [`docs/research/metodo/contratto-data-bridge.md`](../docs/research/metodo/contratto-data-bridge.md).

**I recorder, fase di agosto 2026.** Quattro indicatori che scrivevano righe JSON nel log di ATAS, con lo schema deciso prima della cattura. Contratti e report sono in [`docs/research/archivio-2026-08/`](../docs/research/archivio-2026-08/). **Il sorgente dei quattro recorder e' stato tolto il 10 ottobre 2026**: non stavano su nessun chart, e ogni deploy li ricopiava in ATAS. Si recupera dalla storia di git, per esempio `git show ad6f5ec:FabioOrderFlow/src/Observation/PreSessionProfileRecorder.cs`; i percorsi `src/...` citati nei documenti di questa cartella sono quelli di allora.

## L'Estensione

`src/FabioOrderFlow.slnx` compila una DLL sola, **`FabioDataBridge`**: una `partial class DataBridge` divisa per responsabilita' in `src/Observation/*.cs`, elencati in `src/Indicators/DataBridge/FabioDataBridge.csproj`. Endpoint HTTP locale; calcola e disegna livelli, zone del valore, pannello, regime, composito e sedute secondo il ruolo del chart; legge l'interfaccia di ATAS. Non applica logica di mercato propria: le soglie e le regole stanno nei documenti e nei file che l'analisi deposita.

## Build E Deploy

Il target e' `net10.0` senza WPF, requisito di ATAS X. `Indicators/Directory.Build.props` cerca gli
assembly di ATAS in `C:\ATASX` (ATAS X su Windows), poi nel bundle di macOS, poi nel vecchio ATAS 8;
si forza con `-p:AtasAssemblyDir=...`.

```bash
cd FabioOrderFlow/src
./deploy.sh
```

Il deploy copia la DLL in `%APPDATA%\ATAS X\Indicators` e rimuove quelle che non si distribuiscono
piu' (lo scheletro iniziale e i recorder di agosto). **ATAS X le ricarica da solo**, senza riavvio; l'id di ogni chart pero'
cambia. Procedura completa in [`docs/research/build-e-deploy.md`](../docs/research/build-e-deploy.md).

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

**Il bridge e ATAS**

| Script | Cosa fa |
|---|---|
| `tools/bridge.py` | Client del bridge, un sottocomando per endpoint; spezza le finestre oltre il limite di ATAS |
| `tools/foto_atas.py` | Fotografa le tab di ATAS una per una, solo la finestra di ATAS |
| `tools/workspace_a_tab.py` | Costruisce il workspace a tab; `--mostra` lo legge, `--ritocca` lo corregge ad ATAS chiuso |

Come si usano insieme: [`come-l-agente-vede-e-comanda-atas.md`](../docs/research/metodo/come-l-agente-vede-e-comanda-atas.md).

**Il contesto e la giornata**

| Script | Cosa fa |
|---|---|
| `tools/giro_orizzonte.py` | Tutto il contesto prima di una lettura dal vivo; si lancia a mano (l'hook e' spento dall'11 ottobre 2026) |
| `tools/scala_dei_tempi.py` | Le sedute dal piu' lontano al piu' vicino, per il giro |
| `tools/ora.py` | L'orario italiano con cui si parla a chi opera |
| `tools/annota.py` | Scrive una lettura sul chart e la registra nel diario della giornata |
| `tools/riavvolgi_giornata.py` | Riporta il diario della giornata indietro insieme al replay |
| `tools/semina_memoria.py` | Installa la memoria di orientamento dell'agente dai file del repo |
| `tools/serve_rifare.py` | Dice se i livelli statici vanno rifatti |
| `tools/permesso_di_fatto.py` | Il permesso direzionale misurato sull'accettazione |
| `tools/livelli_vivi.py` | Ricalcola i livelli che si muovono e li rideposita sul chart |

**La sorveglianza** (fase chiusa: non si riaccende senza richiesta esplicita)

| Script | Cosa fa |
|---|---|
| `tools/sveglia_tape.py`, `tools/sveglia_movimento.py` | Svegliano l'analisi quando il tape o il prezzo fanno qualcosa |
| `tools/scenari.py`, `tools/condizione_viva.py` | Valutano scenari e condizioni dichiarati e li mettono sul chart |
| `tools/avviso.py` | Porta un avviso davanti a chi opera |
| `tools/comandi_sorveglianti.py`, `tools/spegni_sorveglianti.sh` | Accendono e spengono i sorveglianti |

**Gli studi**

| Script | Cosa fa |
|---|---|
| `tools/describe_cash_profile.py` | POC e area di valore della cash dal footprint scaricato |
| `tools/build_continuous.py` | Serie continua back-adjusted da piu' contratti, footprint incluso |
| `tools/build_bars.py` | Barre di range o di tempo ricostruite dal tape |
| `tools/build_cot_cross_index.py` | La vista cross-index COT dai dati di Tradingster |
| `tools/studio_lvn_incrociati.py` | Lo studio dietro agli LVN incrociati |
| `tools/replay_model.py`, `tools/replay_sweep.py`, `tools/probe_predictability.py` | Il replay del modello 40R e la predittivita' (fase sistematica chiusa, [`archivio-modello-40r/`](../docs/research/archivio-modello-40r/)) |
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
- Su ATAS X sono vietati gli editor WPF custom. Il bridge non ne usa.
- I clic e i tasti simulati da fuori non arrivano ad ATAS (UIPI): l'interfaccia si legge e si comanda
  da dentro, con `/ui` e `/disegni`.

## Diario

Il diario cronologico e' [`progress.txt`](progress.txt): una riga per fase conclusa, con il documento che ne contiene i dettagli.
