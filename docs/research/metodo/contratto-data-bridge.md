# Fabio Data Bridge: Contratto Osservativo

Schema: `fof-data-bridge-v1`. Stato: strumento di raccolta. Non contiene modello, soglie o segnali.

## Perche' Esiste

I recorder precedenti decidono in anticipo cosa registrare e scrivono nel log di ATAS; l'analisi arriva dopo, su quello che era stato deciso prima. Il bridge inverte il rapporto: ATAS resta aperto e l'analisi chiede i dati quando le servono, con le finestre e i filtri che servono in quel momento.

Serve perche' diverse informazioni utili esistono solo dentro il processo ATAS: il profilo fisso con sessione dichiarata, i trade aggregati storici con filtro di volume nativo, le date di rollover calcolate sul volume, gli snapshot storici del book. Nessuna di queste e' ricavabile dall'esterno.

## Cosa Non Fa

Non calcola indicatori, non classifica partecipanti, non applica soglie e non invia ordini. Legge, converte in JSON, restituisce. Ogni risposta riporta lo strumento e il timeframe del chart su cui il bridge e' caricato, perche' quel contesto fa parte del dato.

Le eccezioni sono due, e sono la stessa cosa a due stadi. `/rules` riceve **come si trova** un livello — un POC, un bordo del valore, una mensola, su una finestra dichiarata — e l'indicatore lo ricalcola a ogni barra: vedi [`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md). `/levels`: un elenco di prezzi con etichetta che l'analisi deposita e che l'indicatore disegna. Sono dati dell'indicatore, non entrano in nessun calcolo e non producono segnali; servono a non dover ridisegnare a mano su ATAS i livelli che l'analisi ha gia' individuato.

## Sicurezza

Il listener e' legato a `127.0.0.1` e non e' raggiungibile dalla rete. Espone dati di mercato in sola lettura, con una sola superficie di scrittura: la lista dei livelli dell'istanza, che vive in memoria nell'indicatore. Nessun endpoint modifica lo stato della piattaforma, e nessuno tocca ordini o posizioni.

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
| `/levels` | `chart` | `GET` restituisce i livelli del chart, `POST`/`PUT` li sostituisce, `DELETE` li cancella. **Un POST qui spegne le regole**: il controllo passa a mano |
| `/rules` | `chart` | `GET` restituisce le regole e cosa non hanno prodotto, `POST`/`PUT` le sostituisce e ricalcola subito, `DELETE` le cancella insieme ai livelli che producevano |
| `/panel` | `chart` | `GET`, sola lettura: **il pannello gia' composto**, riga per riga, come sta sul chart. Ogni riga ha `text`, `color` e `peso` (`veto`, `attenzione`, `a favore`, `contro`, `forte`, `normale`) |
| `/muri` | `chart` | `GET`, sola lettura: **i muri che l'indicatore trova da solo**, senza regole depositate. Ogni muro esce con le quattro prove superate — sforzo, pareggio, ritorni, asimmetria — e quando non ce n'e' nessuno, `motivo` dice quale prova e' fallita |
| `/regime` | `chart` | `GET`, sola lettura: il **regime**, il **proxy** della velocita' e i **big trades**, come li misura l'indicatore a ogni barra. Ogni numero esce con la soglia che lo classifica accanto — una classificazione senza la sua regola non si contesta |

### Livelli

Un livello e' `{"price": 29454, "label": "mensola", "color": "#4FC3F7", "style": "solid", "width": 3, "note": "..."}`. Solo `price` e' obbligatorio. `style` vale `solid`, `dash`, `dot` o `dashdot`; `color` accetta `#RRGGBB`, `#AARRGGBB` o un nome noto, e un valore illeggibile ricade sul colore di default invece di far fallire la richiesta — perdere un livello per un colore sbagliato sarebbe peggio. `note` non viene disegnato e torna su `GET`: serve a ricordare perche' il livello c'e'.

Il `POST` **sostituisce** l'intera lista, non aggiunge: cosi' lo stato del chart e' sempre quello dell'ultima analisi e non si accumulano livelli dimenticati. Ogni chart ha la sua lista.

Il disegno si controlla dalle proprieta' dell'istanza: `Show levels`, `Label on the right`, `Font size`. La linea attraversa l'area del chart e l'etichetta ha un fondo pieno, perche' sopra un footprint denso il testo nudo e' illeggibile.

```bash
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 --set 29454:mensola:#4FC3F7:solid --set 29306:VAL-lunedi
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 --file livelli.json
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 --clear

# il modo normale: si depositano le REGOLE, e a calcolarle e' l'indicatore
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6 --file regole-dei-livelli-NQZ6-2026-09-14.json
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6 --clear
```

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
- **Il tape completo e' disponibile.** `GetCumulativeTradesSessionLimit(Filter)` vale `0`, cioe' nessun limite: con `minVolume 0` l'endpoint `/cumulative` restituisce ogni trade aggregato, con timestamp al millisecondo e direzione gia' classificata da ATAS. Due ore di NQ sono circa 114.000 record. Verificato ricostruendo delta e volume di barra dal tape: coincidono con quelli di ATAS a meno dell'assegnazione dei trade al millisecondo di confine. Il client lo scarica con `--min-volume 0 --window-minutes 10 --compact`.
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

## I Vincoli Che Hanno Gia' Prodotto Errori

Quattro, e ciascuno e' costato una misura sbagliata almeno una volta.

- **`bar` e' un indice di posizione, non un identificatore**: riparte quando ATAS ricarica
  l'indicatore, che succede a ogni deploy. Per riconoscere una barra si usa **`time`**.
- **Il contratto continuo di ATAS non e' back-adjusted**: si usano i contratti singoli, o
  `FabioOrderFlow/tools/build_continuous.py`. **Il controllo del rollover precede ogni altra
  misura**: il 14 settembre NQZ6 e' diventato front month da 8.600 a 173.480 lotti in un giorno,
  e i prezzi delle sedute precedenti su quel contratto non sono valore trasferito.
- **La speed of tape non esiste nel bridge.** Il proxy e' il volume per barra M1 contro la
  distribuzione recente, e **va dichiarato come proxy ogni volta che si usa**.
- **L'id del chart cambia a ogni ricarica dell'indicatore**, quindi non si cabla da nessuna parte:
  lo si chiede a `/charts` o `/health`.

### Il Delta Esiste Anche Prezzo Per Prezzo

`bridge.py candles --levels` restituisce la **footprint** di ogni barra — `ask` meno `bid` a ogni
prezzo scambiato, piu' `maxPositiveDelta` e il POC di quella barra.

**E' lo strumento con cui si misura l'assorbimento del live**: sforzo alto, risultato nullo.
Procedura in [`la-footprint-e-il-delta-per-prezzo.md`](la-footprint-e-il-delta-per-prezzo.md).

### I Big Trades Sono Il Filtro Di Volume Nativo

Non e' una nostra soglia, e' la taratura dichiarata da Fabio: **60 su NQ in cash**, 20-30 in
premarket.

```bash
python3 FabioOrderFlow/tools/bridge.py cumulative --chart NQZ6 \
        --from 2026-09-14T12:20:00Z --to 2026-09-14T13:00:00Z --min-volume 60
```

Il sottocomando e' `cumulative --min-volume`, non `trades`: `bridge.py trades` non esiste, ed e'
stato citato per errore in `CLAUDE.md` fino al 20 settembre 2026.

## `/regime`: Tre Avvertenze Che Contano Piu' Dei Numeri

- **`velocita` non e' la speed of tape, ed e' per questo che il campo si chiama `proxy`.** La
  speed of tape e' il *ritmo* di immissione degli ordini `[3 · 1:08:41]`; qui c'e' il volume della
  barra contro la distribuzione delle ultime sessanta. Mille lotti in dieci secondi e mille in
  sessanta hanno lo stesso volume e velocita' opposte. Una lettura che lo usa **lo dichiara**,
  come si dichiara la finestra accanto a una misura.
- **`bigTrades.copreTuttaLaFinestra` va letto prima del conteggio.** Il registro del tape si
  riempie da quando l'indicatore si carica, e all'avvio si semina con novanta minuti di storico.
  Se copre solo da meta' finestra, `quanti` non e' zero: e' **non lo so**, e i due casi non si
  confondono.
- **`regime.efficienza` e' una misura di questo repository, non del corso.** Netto diviso strada
  percorsa. Il live la fa a occhio; qui serviva un numero, e il numero esce con le sue soglie.

## `/panel`: Perche' Il Pannello Esce Gia' Composto

**Il livello in gioco, il lato di arrivo, lo sforzo al livello e i veti vivevano solo sullo
schermo.** Erano calcolati dentro l'indicatore e soltanto disegnati: per parlarne, l'agente doveva
rifare quei conti dalle candele grezze — cioe' **duplicare la logica**. Una copia diverge, e il
giorno che diverge l'agente dice un numero mentre l'utente ne ha un altro davanti. E' il peggiore
degli esiti, perche' nessuno dei due si accorge dell'altro.

**Per questo escono le righe gia' composte e non i dati per ricomporle.** Non c'e' nessuna seconda
formattazione che possa scostarsi dalla prima: quello che esce di qui **e'** quello che sta sul
chart, carattere per carattere.

**`peso` si dichiara, non si deduce dal colore.** Sul pannello il rosso vuol dire due cose — "veto"
e "direzione SHORT" — e a schermo si distinguono dal contesto. Nel JSON no: la prima lettura di
`/panel` ha marcato `SERVE A SHORT` come un veto. Adesso il peso `veto` lo porta solo chi lo
dichiara; il colore resta come ripiego e produce al massimo `contro`.

**La barra in formazione e' inclusa** (`barraInFormazione: true`), come sul chart: i suoi numeri
cambiano fra una lettura e la successiva senza che il mercato abbia fatto niente.
