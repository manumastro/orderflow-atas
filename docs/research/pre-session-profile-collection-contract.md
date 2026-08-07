# Contratto Di Raccolta: Profilo Pre-Sessione NQ

## Stato

```text
Tipo:                 raccolta osservativa live/storica da candle ATAS
Stato:                approvato per implementazione
Modello attivo:       nessuno
Segnali:              nessuno
Ordini / PnL:         nessuno
Schema runtime:       fof-pre-session-profile-v1
Prefisso log:         FofPreSession
Output grafico:       overlay osservativo approvato
```

Questo contratto serve a rendere ripetibile l'esercizio mostrato nel video 1. Non trasforma il profilo in una strategia e non dichiara un segnale operativo.

## Evidenza Del Corso

Nel video 1 Fabio:

- a `46:54` apre il grafico della sessione New York e introduce l'analisi pre-sessione;
- a `47:18` definisce il business range come il massimo e il minimo chiari costruiti durante la notte;
- a `47:59` chiede di tracciare il profilo della pre-sessione;
- a `48:07` dice che, per semplificare, si puo' usare anche soltanto la sessione London;
- a `48:24` confronta la forma del profilo e la distribuzione del volume;
- a `52:14-54:35` collega bordi del valore, delta orizzontale e livelli di possibile protezione.

La fonte non stabilisce un'ora universale di inizio. Fabio lascia l'ancoraggio del profilo alla domanda e alla lettura discrezionale; nel video 2 ribadisce che si puo' usare l'intero dealing range oppure solo London. Le finestre sotto sono quindi una convenzione osservativa esplicita, non una regola attribuita a Fabio.

## Finestra Temporale Congelata

Strumento di riferimento: future Mini NQ del chart, con feed verificato UTC.

Fuso di analisi: `America/New_York`, implementato su Windows con `Eastern Standard Time` e conversione DST automatica.

Sessione primaria, denominata `NQ Overnight Pre-Session`:

```text
inizio: 18:00 America/New_York del giorno precedente
fine:   09:30 America/New_York del giorno analizzato (estremo escluso)
```

La finestra comprende l'overnight Globex disponibile dopo la pausa giornaliera; la manutenzione `17:00-18:00 ET` resta fuori. Il business range primario e' il massimo/minimo dei livelli footprint con volume osservato in questa finestra.

Confronto secondario, denominato `NQ London Pre-Session`:

```text
inizio: 03:00 America/New_York del giorno analizzato
fine:   09:30 America/New_York del giorno analizzato (estremo escluso)
```

`03:00 ET` e' una convenzione di lavoro per l'apertura London in estate; non e' un orario fissato dalla trascrizione. Il confronto viene registrato per misurare quanto la scelta London cambi range e profilo rispetto alla finestra primaria.

L'apertura cash New York e':

```text
09:30 America/New_York
```

Il suo equivalente italiano viene calcolato dal fuso, non hardcoded. Il 6 agosto 2026 corrisponde a `15:30 Europe/Rome`; nelle settimane di disallineamento tra ora legale statunitense ed europea puo' corrispondere a un'ora italiana diversa.

## Unita' E Fonte Dei Dati

L'unita' e' una candle ATAS caricata nel chart. Per ogni candle dentro la finestra il recorder congela:

- orario iniziale e ultimo scambio;
- open, high, low, close;
- volume, bid, ask, delta, VWAP e ticks della candle;
- tutti i `PriceVolumeInfo` disponibili tramite `GetAllPriceLevels()`.

Il profilo viene ricostruito sommando per prezzo i livelli footprint delle candle. Non usa `CumulativeTrade`, non applica filtri di volume e non interpreta l'identita' dei partecipanti. La raccolta richiede un chart a 1 minuto con footprint disponibile, perche' le finestre iniziano e finiscono su confini intraminuto precisi.

Il timestamp delle candle viene interpretato come UTC, coerentemente con la verifica gia' documentata per `MarketDataArg.Time` sul feed NQU6. Ogni record dichiara questa assunzione e la conversione in New York.

## Campi Del Profilo

Per ciascuna finestra vengono registrati:

```text
business range:        low, high, midpoint, ampiezza in tick
volume:                totale, bid, ask, between, delta
POC:                   tutti i prezzi in parita' di volume massimo
value area:            VAH e VAL con percentuale congelata al 70%
max delta positivo:    prezzo/i, ask, bid e delta
max delta negativo:    prezzo/i, ask, bid e delta
forma descrittiva:     volume sotto/sopra POC e nel terzo basso/centrale/alto
copertura:             numero candle, prima/ultima candle, bordi disponibili
```

L'area di valore e' calcolata in modo deterministico: si parte dal POC scelto come seme piu' basso in caso di parita' e si aggiunge, a ogni passo, il lato adiacente con volume maggiore finche' il volume incluso raggiunge almeno il 70% del totale. Le parita' di POC e di delta restano comunque tutte nel log. Questa e' la convenzione del recorder; non e' una prova che coincida con ogni impostazione del Fixed Profile di ATAS.

La forma non viene classificata come balance, accumulazione, distribuzione o squeeze. Il log conserva percentuali e conteggi grezzi; la classificazione resta una fase di studio separata, per evitare di introdurre soglie non definite dal corso.

## Riferimento All'Apertura

Quando sono disponibili candle dopo `09:30 ET`, il recorder aggiunge un riferimento puramente descrittivo:

- prima candle cash e relativo OHLCV/delta;
- distanza in tick dell'open da low, high, VAL, VAH e POC;
- posizione dell'open rispetto a range e area di valore;
- riepilogo delle prime finestre di 5 e 15 minuti;
- high, low, close, volume e delta delle finestre;
- flag fattuali per rottura dell'estremo e rientro nel range/valore.

Questi campi descrivono cosa e' successo dopo l'apertura. Non emettono `long`, `short`, entry, stop, target o alert.

## Overlay Grafico

L'overlay non sostituisce il Fixed Profile nativo di ATAS e non tenta di ricrearne l'istogramma. Visualizza invece i livelli risultanti dal profilo sul pannello prezzi. Il costruttore fissa `Panel = IndicatorDataProvider.CandlesPanel` e impedisce di spostare l'istanza in un pannello separato; non mostra marker di entrata, alert, ordini o classificazioni direzionali.

Durante la finestra primaria, da `18:00` ET fino a `09:29` ET, l'overlay evidenzia tutta e sola la fascia temporale del pre-market, per l'intera altezza del pannello prezzi. Disegna inoltre una linea per il massimo provvisorio e una per il minimo provvisorio del business range.

Un nuovo estremo non riavvia la finestra e non invalida il profilo: aggiorna il bordo del range. Questa scelta rende visibile il fatto che il low/high finale non e' noto al momento dell'apertura della pre-sessione.

Dalla prima candle `09:30` ET in poi, solo se il chart include le candle `18:00` ET e `09:29` ET e fornisce livelli footprint non vuoti, l'overlay sostituisce i valori provvisori e disegna questi livelli nell'intero ma solo intervallo pre-market:

```text
business high / business low
POC
VAL / VAH
massimo delta positivo grezzo
massimo delta negativo grezzo
```

Nessuna linea, banda prezzo o rettangolo viene prolungato a destra nell'orario cash. POC, VAL, VAH e delta non sono mostrati durante la pre-sessione come valori finali: sarebbero soggetti a look-ahead. In presenza di copertura incompleta l'indicatore scrive `incomplete` e non disegna i livelli finali.

I nomi delle serie nel pannello ATAS dichiarano `Pre`, `Business`, `Value` o `Delta`; nessuna serie e' chiamata supporto, resistenza, protection, long o short. Il confronto London resta nel log e non viene sovrapposto per default, per evitare di confondere il range completo con una selezione temporale piu' corta.

Per vedere la distribuzione per prezzo e la forma intera del profilo, usare comunque il Fixed Profile nativo sullo stesso intervallo. L'overlay e' un aiuto per tracciare e congelare i livelli, non un duplicato del tool ATAS.

## Logging E Frequenza

Il recorder usa il logger standard ATAS con righe JSON compatte e prefisso `FofPreSession`. Scrive:

1. una dichiarazione di configurazione per la sessione target;
2. una dichiarazione tecnica `render-configuration`, una sola volta per istanza, con panel, visibilita' e stato delle serie dell'overlay;
3. snapshot del profilo al massimo ogni 30 minuti durante la pre-sessione, solo per il giorno target;
4. uno snapshot finale quando il primo dato cash raggiunge `09:30 ET`;
5. un riferimento di apertura e i riepiloghi a 5/15 minuti, una sola volta ciascuno;
6. una notifica di incompletezza se il chart non contiene la finestra richiesta.

Non vengono serializzati tutti i livelli in ogni snapshot: il profilo aggregato contiene i livelli necessari, mentre il numero di righe resta limitato e non replica il logging verboso che ha prodotto file ATAS di grandi dimensioni.

## Giorno Target

Il recorder analizza una sola sessione target per istanza. Il giorno target viene derivato dall'orologio UTC del provider e convertito in New York:

- prima delle `18:00 ET`, il target e' il giorno New York corrente;
- dalle `18:00 ET`, il target diventa il prossimo giorno feriale;
- sabato e domenica vengono portati al lunedi' successivo.

Festivita' e orari speciali non vengono inferiti: se il feed non contiene dati, il record dichiara la copertura mancante.

## Criteri Di Completezza

Una raccolta e' completa per la lettura finale se:

- il chart espone footprint e ha almeno una candle nel range;
- sono presenti dati fino alla candle `09:29 ET` oppure il feed dichiara che non ci sono scambi;
- il confine temporale e' compatibile con il chart a 1 minuto;
- security, tick size, timeframe e clock sono dichiarati.

Se l'indicatore viene aggiunto dopo l'apertura e il chart non ha caricato l'overnight, non ricostruisce dati mancanti da supposizioni: scrive `incomplete` e richiede di caricare lo storico del range.

## Fuori Scope

- segnali automatici o alert;
- classificazione di regime;
- attribuzione a istituzioni, market maker o retail;
- confronto con DeepCharts `Aggregate`;
- esecuzione, stop, target, size e PnL;
- profili di giorni precedenti o backtest multi-sessione.

## Riferimenti

- `fabio_course/fabio1.txt`
- `fabio_course/fabio2.txt`
- `fabio_course/fabio3.txt`
- `fabio_course/fabio-course-model-map.md`
- `docs/atas/guides/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.md`
- `docs/atas/guides/md_DataFeedsCore_2Docs_2en_20130__AddingLogging.md`
- `FabioOrderFlow/src/Observation/HistoricalCumulativeContextRecorder.cs`
