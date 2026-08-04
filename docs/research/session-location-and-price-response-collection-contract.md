# Contratto Di Raccolta: Location Di Sessione E Risposta Del Prezzo

## Stato

```text
Tipo:                 contratto osservativo di raccolta live
Stato:                approvato dall'utente nel dialogo corrente
Campione:             una sola sessione cash del future Mini
Modello attivo:       nessuno
Segnali:              nessuno
Ordini / PnL:         nessuno
Runtime modificato:   secondo recorder osservativo separato
```

Questo contratto segue la descrizione di volume, tick e footprint della sessione del 2026-08-04. Non converte un `CumulativeTrade` in un segnale e non prova l'equivalenza con DeepCharts `Aggregate`.

## Domanda

In una sessione cash esplicitamente dichiarata, come co-occorrono:

1. il lato, il volume finale e i tick di un `CumulativeTrade` ATAS;
2. la distanza del suo prezzo finale dal POC del profilo di sessione costruito fino a quell'evento;
3. il percorso grezzo del prezzo nei cinque minuti successivi al termine dell'evento?

Il risultato e' una descrizione di fatti. Non etichetta il percorso come assorbimento, accettazione, rifiuto, follow-through o setup.

## Perche' Serve Una Nuova Raccolta

La cattura del 2026-08-04 localizza l'evento rispetto al POC della sua barra, ma non conserva:

- un profilo di sessione calcolato progressivamente fino all'evento;
- una dichiarazione esplicita di sessione e fuso orario;
- tutti gli scambi del percorso successivo a ciascun evento.

Questi campi non possono essere ricostruiti in modo causale dal log esistente. Un backfill da barre gia' chiuse introdurrebbe volume e prezzi posteriori all'evento nel suo contesto di partenza.

## Ambiente Di Raccolta

Il recorder non espone configurazioni. Il perimetro e' fisso e viene scritto in ogni record:

```text
strumento:            future Mini NQ caricato nel chart
sessione:             NQ US Cash
clock dichiarato:     America/New_York
inizio / fine:        09:30 - 16:00
chart:                grafico a 1 minuto per il riferimento temporale
avvio recorder:       non oltre le 09:30 America/New_York
orizzonte risposta:   300 secondi dal tick finale dell'evento
```

Il perimetro e' valido solo se `MarketDataArg.Time` del feed usa il clock America/New_York. Il recorder non converte timestamp, non sceglie un fuso alternativo e non offre impostazioni modificabili.

## Unita' Di Osservazione E Campi Congelati

L'unita' primaria resta un `CumulativeTrade` finale. `OnNewTrade(MarketDataArg)` serve soltanto a costruire il profilo di sessione e il percorso del prezzo; non diventa una sorgente concorrente di eventi.

| Gruppo | Campo congelato |
|---|---|
| Evento | EventId, primo e ultimo tick, lato, volume finale, numero e volume dei tick. |
| Profilo live | Volume `Ask`, `Bid` e totale per prezzo, accumulati soltanto dagli scambi dal principio della sessione fino al tick finale dell'evento. |
| POC sessione | Prezzo con maggiore volume totale nel profilo live al tick finale dell'evento; in parita', conservare tutti i prezzi in parita' invece di sceglierne uno arbitrariamente. |
| Location | Distanza in tick tra `LastPrice` e ogni POC sessione in parita'; se esistono piu' POC, registrare il vettore e il flag di parita'. |
| Percorso futuro | Primo, massimo, minimo e ultimo prezzo degli scambi nel periodo `(ultimo tick evento, ultimo tick evento + 300 secondi]`. |
| Ritorno POC | Flag se almeno uno scambio futuro negozia un prezzo POC congelato dell'evento entro 300 secondi. |
| Completezza | Flag per evento se sessione, profilo, tick finale e finestra futura sono completi. |

La POC di sessione e' un fatto di volume aggregato fino all'evento. Non e' automaticamente valore, supporto, resistenza, protezione o location sufficiente per un playbook.

### Strategia Di Registrazione Senza Look-Ahead

Il recorder non calcola POC, location o outcome in tempo reale. Registra invece due flussi JSON separati, entrambi con configurazione di sessione e sequenza locale:

```text
raw trade:         MarketDataArg Trade dentro la sessione dichiarata
evento aggregato:  ogni stato OnCumulativeTrade / OnUpdateCumulativeTrade
```

Il report ordina i raw trade per `Time` e sequenza locale, costruisce il profilo usando solo trade con `Time <= ultimo tick dell'evento` e misura il percorso futuro con trade strettamente successivi. Questo evita che un callback ritardato o una barra gia' chiusa inserisca dati futuri nella location iniziale.

Per ogni `EventId`, lo stato finale viene scelto in modo deterministico: massimo `TotalVolume`, poi ultimo `LastTickTime`, poi maggiore `UpdateNumber`. Le parita' residue devono restare documentate nel report invece di essere risolte manualmente.

### Regole Di Raccolta

1. Caricare il recorder non oltre le 09:30 America/New_York; un avvio successivo rende incompleto il profilo e scarta gli eventi precedenti all'avvio.
2. Aggiornare il profilo live con i soli trade di sessione ricevuti fino al tempo dell'evento; non leggere barre o profile data successivi per costruire la location iniziale.
3. Congelare il profilo al tick finale dell'evento e non ricalcolarlo quando il POC cambia dopo.
4. Misurare la risposta con i raw trade ricevuti dopo il tick finale; non inferirla da barre che includono scambi precedenti all'evento.
5. Escludere dal campione valutabile gli eventi con meno di 300 secondi residui prima della fine dichiarata della sessione, ma conservarne il record come incompleto.
6. Non applicare filtri di volume, lato, distanza POC o risposta mentre si raccoglie.

## Campione E Criteri

```text
obiettivo:       almeno 2.500 eventi CumulativeTrade finali completi
copertura:       una sola sessione NQ US Cash, 09:30-16:00 America/New_York
profilo:         recorder caricato non oltre le 09:30
risposta:        finestra completa di 300 secondi per ogni evento valutabile
```

Il conteggio non include eventi incompleti. Il target e' un requisito di copertura tecnica, non una soglia di significativita' o una dimensione minima per un modello.

## Criteri Di Promozione

La raccolta puo' essere promossa al report descrittivo solo se:

- almeno 2.500 eventi finali hanno profilo sessione e finestra futura completi;
- la somma del volume dei tick coincide con il volume finale per ogni evento completo;
- ogni evento completo dichiara sessione fissa, fuso, strumento, contratto e connector;
- POC multipli, livelli mancanti ed eventi incompleti sono conservati e contati;
- la POC usata per ogni evento e' congelata prima del percorso futuro.

Il report ammesso potra' descrivere distribuzioni congiunte di evento, location e percorso. Non potra' promuovere un modello, un filtro o una previsione.

## Criteri Di Scarto O Sospensione

Sospendere se:

- il recorder viene avviato dopo le 09:30 e non puo' ricostruire il profilo completo;
- il feed non espone `MarketDataArg.Time` sul clock America/New_York;
- i trade ricevuti non permettono di delimitare il percorso dei 300 secondi;
- la POC viene calcolata usando dati posteriori all'evento;
- gli eventi completi non raggiungono la copertura dichiarata.

In caso di sospensione, documentare la causa e raccogliere una nuova sessione. Non compensare dati mancanti con barre, soglie o inferenze post-hoc.

## Fuori Scope

- VAH, VAL, HVN, LVN, VWAP, profilo composito e classificazione di regime;
- assorbimento, accettazione, rifiuto, follow-through e scelta del playbook;
- soglie di volume, dimensioni adattive o correlazioni usate come filtri;
- confronto DeepCharts e promozione H1;
- marker, grafica, segnali, ordini, PnL, stop, target o size.

## Modifica Runtime Ammessa Dopo Approvazione

Il recorder autorizzato e' `FabioOrderFlow/src/Observation/SessionLocationPriceResponseRecorder.cs`, indicatore separato denominato **Fabio Session Location Recorder**. Emette esclusivamente JSON con prefisso `FofSessionObservation` nel log ATAS:

- configurazione e primo trade osservato della sessione fissa NQ US Cash;
- raw `MarketDataArg` di tipo `Trade` nella sessione dichiarata;
- stati `CumulativeTrade` e aggiornamenti, con tick costituenti e metadati dello strumento.

Non calcola o disegna POC, non filtra volume, non emette marker e non invia ordini. Il calcolo di profilo e risposta e' ammesso solo nel report offline disciplinato da questo contratto.

## Output Canonico Previsto

```text
docs/research/session-location-and-price-response-description-YYYY-MM-DD.md
```

Alla conclusione, aggiornare `FabioOrderFlow/FabioOrderFlow.md` e aggiungere una riga datata a `FabioOrderFlow/progress.txt`.

## Riferimenti

- `fabio_course/fabio1.txt`
- `fabio_course/fabio2.txt`
- `fabio_course/fabio3.txt`
- `fabio_course/fabio-course-model-map.md`
- `docs/research/participation-effort-result-observation-contract.md`
- `docs/research/cumulative-trade-footprint-description-contract.md`
- `docs/research/cumulative-trade-footprint-description-2026-08-04.md`
- `docs/atas/guides/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.md`
