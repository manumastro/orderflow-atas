# Fabio Data Bridge: Contratto Osservativo

Schema: `fof-data-bridge-v1`. Stato: strumento di raccolta. Non contiene modello, soglie o segnali.

## Perche' Esiste

I recorder precedenti decidono in anticipo cosa registrare e scrivono nel log di ATAS; l'analisi arriva dopo, su quello che era stato deciso prima. Il bridge inverte il rapporto: ATAS resta aperto e l'analisi chiede i dati quando le servono, con le finestre e i filtri che servono in quel momento.

Serve perche' diverse informazioni utili esistono solo dentro il processo ATAS: il profilo fisso con sessione dichiarata, i trade aggregati storici con filtro di volume nativo, le date di rollover calcolate sul volume, gli snapshot storici del book. Nessuna di queste e' ricavabile dall'esterno.

## Cosa Non Fa

Non calcola indicatori, non classifica partecipanti, non applica soglie, non disegna sul chart e non invia ordini. Legge, converte in JSON, restituisce. Ogni risposta riporta lo strumento e il timeframe del chart su cui il bridge e' caricato, perche' quel contesto fa parte del dato.

## Sicurezza

Il listener e' legato a `127.0.0.1` e non e' raggiungibile dalla rete. Espone dati di mercato in sola lettura: non c'e' nessun endpoint che modifichi lo stato della piattaforma o che tocchi ordini e posizioni.

## Endpoint

| Endpoint | Parametri | Cosa restituisce |
|---|---|---|
| `/health` | `chart` | schema, strumento, timeframe, barre caricate, id del chart, porta, elenco endpoint |
| `/charts` | | elenco dei chart registrati: id, strumento, exchange, timeframe, tipo, barre |
| `/instrument` | | strumento, exchange, tick size, fuso; del contratto: scadenza, lot size, tick cost, moltiplicatori, open interest |
| `/limits` | | limiti ATAS per `CumulativeTradesMode` (profondita' massima, limite di sessione) ed enum disponibili |
| `/session` | `at` | finestra di sessione della piattaforma per un istante |
| `/rollovers` | `from`, `to`, `type` | date di rollover del contratto; `type` fra `ExpirationDate`, `VolumeBasedCurrentEnd`, `VolumeBasedNextStart` |
| `/profile` | `period`, `session`, `levels` | profilo fisso nativo: POC, value area, footprint completo, versione scalata e originale |
| `/candles` | `from`, `to`, `fromBar`, `toBar`, `levels` | candele del chart con volume, tick, bid/ask, delta, `maxDelta`/`minDelta`, VWAP, POC, value area, open interest e footprint opzionale |
| `/cumulative` | `from`, `to`, `minVolume`, `maxVolume`, `mode`, `ticks` | trade aggregati storici con il filtro di volume nativo di ATAS |
| `/depth` | `from`, `to`, `periodSeconds` | snapshot storici del book |

`maxDelta` e `minDelta` sono il delta massimo e minimo raggiunti **durante** la barra: sono la misura diretta dello sforzo che non ottiene risultato, non ricavabile dal solo delta di chiusura.

## Piu' Chart, Un Solo Listener

L'indicatore puo' essere caricato su un numero qualsiasi di chart. Le istanze **condividono un unico listener di processo**: un `HttpListener` per istanza fallirebbe alla seconda con `AddressAlreadyInUse`, e costringerebbe ad assegnare porte a mano.

- La prima istanza avvia il listener sulla **prima porta libera** fra 8787 e 8796 e scrive l'indirizzo in `~/.fabio-data-bridge.json`.
- Ogni istanza riceve un `id` breve, visibile su `/charts`.
- Ogni richiesta accetta `chart=<id|strumento>`. Con un solo chart registrato il parametro e' superfluo.
- La risoluzione del selettore prova **id esatto, poi strumento esatto, poi prefisso dello strumento**. L'ordine e' necessario: il contratto continuo `NQ` e' prefisso di `NQU6` e senza la corrispondenza esatta sarebbe irraggiungibile.
- Con piu' chart registrati, una richiesta **senza** `chart` viene rifiutata con `400` e l'elenco dei candidati, invece di essere servita da un chart arbitrario: lo strumento fa parte del dato e sceglierlo per conto del chiamante produrrebbe risposte silenziosamente sbagliate.
- L'ultima istanza rimossa ferma il listener e cancella il file di discovery.

Il client risolve l'indirizzo da solo: legge il file di discovery, verifica che risponda davvero un bridge, e in mancanza sonda l'intervallo di porte. La verifica interroga `/charts` e non `/health`, perche' `/health` appartiene a un chart e con piu' chart registrati risponde `400`: usarlo in fase di scoperta farebbe scartare un bridge funzionante. `--base` resta disponibile per forzare l'indirizzo.

## Vincoli Noti, Rispettati Dal Bridge

- **Una sola richiesta `CumulativeTrades` pendente alla volta.** Vincolo scoperto con la versione v3 del recorder storico. Il bridge serializza le richieste HTTP concorrenti con un semaforo invece di lasciarle fallire.
- **Record fuori finestra.** ATAS puo' restituire trade fuori dall'intervallo richiesto, come documentato dalla v4/v5 del recorder storico. Il bridge li conta in `outsideWindow` e li esclude da `trades`.
- **Profondita' massima per richiesta.** Interrogabile su `/limits`; il client `bridge.py` spezza le finestre piu' ampie in blocchi consecutivi.
- **`/rollovers` richiede un chart a contratto continuo.** Su un contratto singolo ATAS risponde `Only continuous contracts are supported`. Con un chart `NQ` caricato l'endpoint funziona, e `VolumeBasedCurrentEnd` che coincide con la data di scadenza significa che il roll basato sul volume **non e' ancora stato rilevato**, non che avvenga alla scadenza.
- **Nessun `baseTime` sul profilo fisso.** La build ATAS X espone solo `FixedProfileRequest(period)` e `(period, tradingSession)`: il profilo e' sempre relativo al market time corrente. L'overload con `baseTime` documentato per ATAS classico non esiste qui.
- **Timeout.** Una richiesta di trade aggregati che non riceve risposta entro cinque minuti restituisce `504` invece di restare appesa.
- **`MaxItems`.** Ogni risposta e' limitata; il campo `truncated` dice se il taglio e' avvenuto.

## Uso

Caricare **Fabio Data Bridge** su ogni chart da interrogare. Non c'e' nessuna porta da configurare. Abilitazione e limite di elementi restano proprieta' dell'istanza. Il chart determina strumento, timeframe e candele disponibili.

```bash
python3 FabioOrderFlow/tools/bridge.py charts
python3 FabioOrderFlow/tools/bridge.py health
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 --from 2026-09-11 --to 2026-09-12 --levels
python3 FabioOrderFlow/tools/bridge.py rollovers --from 2026-06-01 --to 2026-12-31
python3 FabioOrderFlow/tools/bridge.py profile --period LastDay --out profile.json
python3 FabioOrderFlow/tools/bridge.py cumulative --from 2026-09-04 --to 2026-09-11 --min-volume 50 --out big.json
```
