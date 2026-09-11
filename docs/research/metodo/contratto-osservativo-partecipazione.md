# Contratto Di Ricerca: Partecipazione E Sforzo-Risultato

## Stato

```text
Tipo:                 contratto di ricerca osservativo
Modello attivo:       nessuno
Segnali:              nessuno
Ordini / PnL:         nessuno
Runtime modificato:   solo registratore osservativo separato
Approvazione:         autorizzato dall'utente nel dialogo corrente
```

Questo documento e' la fonte canonica per la verifica della corrispondenza tra il Big Trades del corso e le API ATAS. Non e' una specifica di indicatore, un playbook operativo o una definizione di entrata.

## Domanda

Sul future Mini usato nel corso, `CumulativeTrade` di ATAS rappresenta un proxy sufficientemente fedele di `Big Trades` con `Base Dati = Aggregate Trades` per osservare la partecipazione aggressiva insieme al footprint e al successivo risultato del prezzo?

La domanda non e': "un big trade prevede il prezzo?". Nel corso l'evento e' letto soltanto insieme a contesto d'asta, location, sforzo e risultato.

## Evidenza Di Partenza

| Fonte | Fatto stabilito |
|---|---|
| Video 1, `05:33` | Fabio cita 60 contratti NQ Mini come esempio contestuale di stampa importante. |
| Video 1, `01:09:13` | Il filtro visuale viene alzato quando il grafico e' affollato. |
| Video 2 | Le stampe vanno lette sul future Mini, non sul Micro. |
| Video 3 | Lo strumento mostrato usa Level 1; la nuova versione MBO/Level 2 e' distinta. |
| Workspace mostrato da Fabio | `Base Dati = Aggregate Trades`. |
| DeepCharts | `Aggregate` e' un algoritmo proprietario basato su velocita' e clustering del tape. |
| API ATAS | `CumulativeTrade` aggrega piu' print/esecuzioni e puo' essere aggiornato dopo la prima notifica. |

La soglia `60`, il filtro `100` e le altre dimensioni mostrate nei video non sono parametri iniziali del test. Vanno registrati come elementi del workspace e mantenuti separati da qualsiasi soglia ATAS futura.

## Ipotesi Da Testare

| Id | Ipotesi | Stato |
|---|---|---|
| H1 | Gli eventi `CumulativeTrade` consentono di osservare eventi aggressivi aggregati della stessa natura funzionale dei marker Big Trades `Aggregate`. | Da testare |
| H2 | Il footprint per prezzo localizza lo sforzo che il solo evento aggregato non localizza completamente. | Supportata dalla struttura delle API; da osservare nel confronto |
| H3 | La risposta successiva del prezzo distingue osservazioni di follow-through, assorbimento, accettazione e rifiuto solo nel contesto dell'asta. | Da definire e annotare; non implementare |

Nessuna ipotesi attribuisce un evento a una specifica istituzione, account o intenzione certa.

## Unita' Di Osservazione

L'unita' primaria e' un **evento aggressivo aggregato ATAS**. Non e' un segnale.

```text
evento CumulativeTrade
+ location nel footprint e nel profilo
+ sequenza successiva di prezzo
+ annotazione del contesto d'asta
= osservazione di partecipazione e sforzo-risultato
```

Ogni evento deve conservare almeno:

| Gruppo | Campi |
|---|---|
| Identita' di raccolta | strumento Mini, contratto, data, sessione, feed, fuso orario, chart/range usato, identificatore sequenziale locale |
| Evento ATAS | `Time`, `Direction`, `Volume`, `FirstPrice`, `Lastprice`, numero di `Ticks` |
| Composizione | volume, prezzo, orario e direzione dei tick costituenti quando disponibili |
| Footprint | barra che contiene l'evento, `PriceVolumeInfo.Price`, `Ask`, `Bid`, `Volume`, `Ask - Bid`, POC della barra |
| Risposta prezzo | prezzi e barre successivi, massimo/minimo successivi e ritorno o permanenza rispetto al livello dell'evento |
| Confronto DeepCharts | timestamp, direzione, volume visualizzato, posizione, filtro Min/Max, screenshot e presenza/assenza marker |
| Contesto annotato | regime osservato, location rispetto a valore/profilo, nota testuale separata dai fatti |

Il record non deve contenere istruzioni di entrata, uscita, stop, target, size o PnL.

## Sorgenti ATAS E Responsabilita'

| Scopo | API | Regola |
|---|---|---|
| Evento primario | `OnCumulativeTrade(CumulativeTrade)` | Registrare il primo stato dell'evento. |
| Aggiornamento | `OnUpdateCumulativeTrade(CumulativeTrade)` | Registrare l'aggiornamento e contabilizzare soltanto `Volume` incrementale per lo stesso oggetto. |
| Storico | `RequestForCumulativeTrades(CumulativeTradesRequest)` e `OnCumulativeTradesResponse` | Richiedere intervalli non superiori a sette giorni; registrare l'intervallo richiesto e la risposta. |
| Audit tick | `CumulativeTrade.Ticks` e `OnNewTrade(MarketDataArg)` | Usare per spiegare la composizione, non per sostituire l'evento primario. |
| Footprint | `GetCandle(bar).GetAllPriceLevels()` | Calcolare il delta di livello come `Ask - Bid`; non dedurre identita' del partecipante. |
| MBO | `AggressorExchangeOrderId` e MBO | Escluso dal test: il workspace del corso usa `Aggregate Trades` Level 1. |

Non e' consentito trasformare automaticamente le aggregazioni ATAS nella logica proprietaria `Aggregate` di DeepCharts. La relazione da verificare e' un proxy osservativo, non una replica dichiarata dell'algoritmo.

## Protocollo Di Raccolta

### 1. Parita' Della Fonte

Per ogni confronto usare simultaneamente:

```text
stesso future Mini
stesso contratto e venue
stesso feed, se disponibile
stessa sessione e fuso orario
stesso intervallo di chart
stesso filtro Min/Max del workspace DeepCharts annotato
```

Il Micro, un contratto diverso, un replay da feed diverso o orari non allineati invalidano il confronto e devono essere indicati come tali, non normalizzati dopo il fatto.

### 2. Cattura Di Riferimento

Per una finestra di mercato annotata, salvare il pannello Big Trades di DeepCharts con `Base Dati = Aggregate Trades` e le sue impostazioni visibili. In ATAS raccogliere tutti i `CumulativeTrade` senza applicare un nuovo filtro discrezionale nella prima passata.

`OnNewTrade` puo' essere raccolto in parallelo solo per audit. Non va filtrato per produrre marker concorrenti.

### 3. Confronto Evento Per Evento

Confrontare gli eventi per sequenza temporale, direzione, volume, prezzo iniziale/finale e cluster dei tick. Classificare ogni marker DeepCharts come:

```text
corrispondenza chiara
corrispondenza plausibile ma non univoca
nessuna corrispondenza ATAS osservata
solo evento ATAS senza marker DeepCharts
non confrontabile per differenza di fonte o sincronizzazione
```

Non usare questi esiti per modificare soglie, tempi di aggregazione o regole nello stesso campione.

### 4. Sforzo E Risultato

Dopo avere bloccato il confronto tecnico, annotare separatamente:

```text
fatto:       direzione, volume, prezzo e delta footprint
risultato:   estensione, stallo, ritorno, permanenza oltre il livello
lettura:     assorbimento, accettazione, rifiuto o follow-through candidato
```

Le quattro parole dell'ultima riga restano etichette di ricerca finche' non saranno definite su un campione di sviluppo e verificate su un campione separato. Non sono regole implementabili in questo contratto.

## Campione Pre-Registrato

Il campione iniziale misura la qualita' della raccolta ATAS e descrive una sessione; non misura una capacita' predittiva. L'unita' da contare e' un evento unico con `source = live-new`; le righe `live-update` sono audit dello stesso evento e non aumentano il conteggio.

### Studio Iniziale Di Una Sessione

```text
obiettivo:       2.500 eventi live-new unici
copertura:       una singola sessione cash del future Mini
storico:         almeno una risposta storica ATAS su un chart di massimo 7 giorni
metadati:        strumento, contratto, exchange, connector e fuso orario presenti o esplicitamente assenti
risultato:       validazione del registratore e descrizione della sola sessione osservata
```

Il flusso raccolto prima di questa registrazione e' un **pilot tecnico**: serve a verificare il deploy, il log e l'assenza di duplicazioni, ma non concorre alla decisione dello studio iniziale. I risultati del campione pre-registrato saranno mantenuti separati da qualunque successiva scelta di soglia o classificazione.

### Verifica Multi-Sessione Successiva

Una replica su almeno cinque sessioni cash distinte resta facoltativa e successiva. Serve soltanto per verificare se i risultati della prima sessione si ripetono; non e' richiesta per iniziare l'analisi descrittiva.

Senza DeepCharts, lo studio di una sessione puo' promuovere o scartare la qualita' del registratore ATAS, ma non puo' dimostrare l'identita' dell'algoritmo proprietario `Aggregate`.

## Criteri Di Promozione E Scarto

### Promozione Tecnica ATAS

Il registratore puo' essere promosso allo studio descrittivo di una sessione **solo se**:

1. raccoglie almeno 2.500 eventi `live-new` nella sessione dichiarata;
2. la raccolta live e quella storica consegnano tutti i campi dell'evento richiesti dal contratto;
3. gli aggiornamenti non duplicano volume nei record finali;
4. strumento, contratto, exchange, connector e fuso orario sono presenti o esplicitamente assenti;
5. i record mancanti, i prezzi footprint non disponibili e le risposte storiche non ricevute restano nel report.

Questo promuove il registratore ATAS, non H1. Senza una fonte DeepCharts corrispondente, `CumulativeTrade` resta un proxy funzionale dichiarato ma non una replica verificata di `Aggregate`.

### Promozione Di H1

H1 puo' essere valutata soltanto se in futuro viene resa disponibile una fonte DeepCharts o equivalente che esponga marker `Aggregate` allineabili a strumento, sessione, fuso e impostazioni. La promozione richiede che gli scostamenti siano quantificati e che le mancate corrispondenze siano conservate, non selezionate.

### Scarto O Sospensione Di H1

L'ipotesi va sospesa se:

- gli eventi ATAS non sono disponibili o non sono aggiornabili in modo coerente;
- la fonte Mini non e' allineabile al riferimento;
- gli scostamenti non sono spiegabili e rendono la sequenza o la dimensione dell'evento non confrontabile;
- la ricostruzione richiede soglie o una finestra temporale ricavate dopo aver visto gli esempi valutati.

In caso di sospensione, non sostituire silenziosamente `CumulativeTrade` con `OnNewTrade`: va aperto un nuovo contratto per un aggregatore esplicito basato sui tick.

## Fuori Scope

- segnale direzionale;
- scelta del playbook;
- definizione di assorbimento automatica;
- filtri adattivi pressione/ATR non spiegati nel corso disponibile;
- MBO, identificazione di account o liquidita' passiva;
- ordini, PnL, stop, target, size o automazione;
- modifica di `FabioOrderFlow/src/FabioOrderFlow.cs`.

## Prossimo Artefatto Ammesso

Dopo l'approvazione di questo contratto, il solo artefatto tecnico ammesso e' un **registratore osservativo** separato: nessun output grafico interpretativo e nessuna logica di trading. Il registratore e' stato aggiunto in `FabioOrderFlow/src/Observation/CumulativeTradeObservationRecorder.cs`; produce record JSON nel log ATAS con prefisso `FofObservation`, include l'identificativo del connettore quando disponibile e contabilizza gli aggiornamenti di `OnUpdateCumulativeTrade` per differenza di volume.

## Riferimenti

- `fabio_course/fabio1.txt`
- `fabio_course/fabio2.txt`
- `fabio_course/fabio3.txt`
- `fabio_course/fabio-course-model-map.md`
- `docs/atas/guides/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1CumulativeTrade.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md`
- `docs/atas/api/classes/classATAS_1_1Indicators_1_1MarketDataArg.md`
- `https://helpdesk.deepcharts.com/portal/en/kb/articles/different-types-of-input-data-for-indicators`
