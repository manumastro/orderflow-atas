# LondonMeanReversionModel

Modello attivo di studio `FabioCompressionStudy`.

Nessun log del modello apre o gestisce posizioni. `MR_COMPRESSION_LEDGER_*` descrive osservazioni causali live-equivalenti e replay storico, non PnL.

## Stato Corrente

```text
Modalita':              COMPRESSION_EVENT_LEDGER_NO_TRADES
Operativita' trade:     DISABLED
Reference precedenti:   LOG_ONLY
Grafico contesto:       zona London grigia, POC/VAH/VAL dal BalanceZoneTracker
Grafico studio:         nessun box/marker DynamicCompression
Output:                 ledger + acceptance baseline + LOW flow confirmation shadow
PnL corrente:           non applicabile; nessun MR_ENTRY/MR_EXIT nuovo
```

La baseline `london-ny-close-hold` con `+634,25` e' conservata come storico del vecchio core reference mean-reversion. Non e' piu' il modello operativo.

## Tesi Fabio

Fonti: `transcription.txt` e `trascription_1.txt`.

I transcript separano due modelli, da non combinare in una singola entry:

```text
Model 1, New York trend:
imbalance -> low volume node -> aggressione -> continuation.

Model 2, London reversion:
compression/balance -> primo breakout -> rientro -> aggressione opposta -> bulk/POC.
```

Il codice studia solo la versione avanzata del Model 2: individua la compression, misura test e aggressione ai bordi, poi confronta rientro/assorbimento e acceptance breakout. Il summary esteso e' in `docs/research/fabio-transcript-synthesis.md`.

Playbook storico del core precedente:

```text
1. Esiste una value area gia' completa: previous day profile o previous completed London profile.
2. Durante London il prezzo esce da quella value area: sweep/fakeout sopra VAH o sotto VAL.
3. La candela torna dentro value.
4. Non si entra sul primo movimento cieco.
5. Si attende un cumulative big trade nella direzione di rientro.
6. Target = POC / bulk dell'asta della reference area.
7. Se il trade va a favore almeno 1R, stop a breakeven.
8. Se il target non arriva, il tempo massimo operativo e' la chiusura regular New York: 16:00 New York.
9. Se e' sbagliato, stop piccolo vicino all'estremo fallito.
```

Frasi chiave dal transcript:

```text
London sugli indici tende spesso a mean revert.
Non prendo il primo movimento; aspetto breakout, rientro nel balance e big trades.
Il target e' il POC / bulk dell'asta.
Se sono sbagliato voglio esserlo immediatamente.
Quando funziona, dopo il primo movimento favorevole porto il rischio a zero.
Filtro big trades London circa 20 contratti.
La versione semplice puo' usare il profilo del giorno precedente.
```

## Contratto Live / Historic

```text
LIVE        osserva trade/barre real-time e aggiorna gli outcome del ledger quando diventano disponibili.
HISTORICAL  riceve finestre cumulative ATAS sequenziali da massimo 7 giorni, poi ricostruisce gli stessi eventi e outcome sull'intero chart.
```

Entrambe le modalita' sono causalmente equivalenti: un profilo entra nel ledger solo dopo `READY`; ogni outcome viene scritto soltanto quando le sue 1/3/6/12 barre future sono effettivamente disponibili. Per lo storico, il replay parte soltanto dopo l'ultima risposta ATAS. Il ledger trattiene solo trade tra `READY` e `RESOLVED`; `TradeCoverage` dichiara se ATAS ha effettivamente restituito flow per quel profilo. Il ledger non apre ordini, non aggiorna posizioni e non genera PnL.

## Reference Value Areas

Il modello continua a costruire e loggare due reference complete:

```text
PreviousDayProfile       profilo completo del giorno italiano precedente
PreviousLondonProfile    profilo completo della London session precedente
```

Sono `LOG_ONLY`: non producono setup, entry, stop, target, marker trade o PnL. La scelta rispetta la richiesta di non usare piu' il profile semplice come trigger mentre viene studiata la variante avanzata sulla compression locale.

## Compression Study Input

Fabio distingue tra versione semplice e versione avanzata:

```text
Versione semplice:  usare il previous day profile / previous balance area.
Versione avanzata: identificare la compressione/dealing range che il mercato costruisce durante la sessione e plottare il profilo su quella zona.
```

La compression non e' piu' una diagnostica agganciata a setup legacy: e' l'input dello studio attivo.

```text
READY     congela range, POC, VAH e VAL prima dell'evento studiato.
RESOLVED  chiude l'osservazione dopo due close in acceptance o alla fine sessione.
LEDGER    registra ogni interazione High/Low e il suo esito, senza qualificarla come entry.
```

Non esistono setup legacy o entry reali. Ogni profilo, evento e outcome porta `OperationalEntry=FALSE`.

Il focus attuale e' una sola diagnostica locale:

```text
ProfileSource=ActiveCompressionProfile
```

`ActiveCompressionProfile` e' il range locale studiato. Non e' un entry model, non parte da un'entry e non puo' aprire posizioni.

Lifecycle causale e dinamico:

```text
SEARCHING
- Usa le ultime 12-24 barre London completate come distribuzione di volatilita' precedente.
- Avvia una candidata quando il range della nuova barra e' nel 50% inferiore della distribuzione.

BUILDING
- L'inizio resta il primo bar contratto; non esiste una finestra massima fissa.
- La candidata cresce solo se la nuova barra si sovrappone e la close resta accettata vicino al range.
- Dopo almeno 6 barre calcola CompressionScore a ogni nuova barra.

READY
- Richiede CompressionScore >= 0,65 per 2 barre consecutive.
- Congela finestra, high/low, POC, VAH e VAL; solo allora logga [MR_LOCAL_PROFILE_READY].
- Solo dopo READY il motore puo' analizzare test al bordo, assorbimento e acceptance.

RESOLVED
- Richiede 2 close consecutive oltre il range piu' una tolleranza adattiva pari al 15% della mediana precedente.
- Una wick o una singola close esterna non risolve il profilo.
```

`CompressionScore` e' normalizzato da `0` a `1`:

```text
20% VolatilityContraction
20% AdjacentOverlap sulle barre recenti
15% assenza di avanzamento direzionale
10% Rotation
10% CloseContainment
10% BoundaryStability
7,5% PocStability
7,5% ValueConcentration
```

I reload precedenti hanno dimostrato che finestre e rapporti rigidi non bastano: 54 profili iniziali e 46 dopo la prima normalizzazione, con range massimo 243,75. Il reload dinamico 2026-07-11 ha prodotto 7 profili da 7-11 barre, range 19,50-88,25 e score 0,67-0,80; tutte le risoluzioni hanno richiesto 2 close. La riduzione dei falsi profili e il lifecycle sono validati tecnicamente. Resta da confrontare visivamente ciascuna finestra con la compression Fabio-style; per questo i risultati sono candidati studio, non trade.

Le diagnostiche precedenti `CurrentLondonSessionProfile`, `LocalRotationProfile` e `LatestSwingPairToSetup` sono ritirate dal codice attivo.

Campi principali:

```text
ProfileSource / ProfileLabel
ProfileReadyBar / ProfileReadyTime
ProfilePOC / ProfileVAH / ProfileVAL
ProfileHigh / ProfileLow / ProfileRange
CompressionScore
ContractionScore / OverlapScore
DirectionalScore / RotationScore
ContainmentScore / BoundaryStabilityScore
PocStabilityScore
ValueConcentrationScore
BaselineMedianBarRange
RangeToBaselineMedian
AverageBarRangeToBaselineMedian
DirectionChanges
HighTests / LowTests / BoundaryEvents
BuyVolume / SellVolume / ProfileCVD / TradeCoverage
Boundary / Interaction / TestOrdinal / OutsideCloseStreak
BreachDistanceRanges / CloseDistanceRanges / BarRangeToBaselineMedian
TradeCount / TotalVolume / BuyVolume / SellVolume / Delta
MaxBuyVolume / MaxSellVolume e percentile causali precedenti
CloseMoveRanges / UpMfeRanges / DownMaeRanges
EndInsideRange / PocTouched
OperationalEntry=FALSE
```

Serve per confrontare ogni interazione al bordo senza PnL e senza filtro volume fisso. Un eventuale modello futuro richiede una validazione shadow separata.

## Studio Attivo

Lo studio e' attivo su live e historical, ma e' `NO_TRADES`.

### Ledger Eventi

Per ogni barra dopo `READY` fino a `RESOLVED`, il ledger registra `HIGH` e/o `LOW` quando il bordo viene toccato o superato. Non richiede big trade, due close, retest, stop, target o una direzione predeterminata.

```text
[MR_LOCAL_PROFILE_READY]          range causalmente disponibile
[MR_LOCAL_PROFILE_RESOLVED]       fine finestra osservata
[MR_COMPRESSION_LEDGER_PROFILE]   geometria, test, volume complessivo e CVD del range
[MR_COMPRESSION_LEDGER_EVENT]     touch/breach al bordo con flow raw e percentili relativi
[MR_COMPRESSION_LEDGER_OUTCOME]   close/MFE/MAE a 1, 3, 6, 12 barre, rientro e touch POC
```

Il percentile confronta l'evento solo con le barre precedenti dello stesso range: e' una metrica causale, non un filtro. Lo studio non afferma ancora che la finestra sia sempre quella che Fabio disegnerebbe; nessun retest operativo, entry o PnL viene introdotto.

### Shadow Acceptance Live

`ACCEPTANCE_CONTINUATION_V1` e' una shadow observation live/historical, non una posizione. Scatta sulla seconda close consecutiva fuori dal range congelato:

```text
HIGH + OutsideCloseStreak >= 2 -> Direction=LONG
LOW  + OutsideCloseStreak >= 2 -> Direction=SHORT
```

Emette al massimo una shadow entry per profilo:

```text
[MR_SHADOW_ACCEPTANCE_ENTRY]
OperationalEntry=FALSE
OrderSubmitted=FALSE
```

`H6` e `H12` significano outcome dopo 6 e 12 barre dalla shadow entry. Su chart 5 minuti equivalgono indicativamente a 30 e 60 minuti. Non sono target. Inoltre `[MR_SHADOW_ACCEPTANCE_BAR]` registra ogni barra completata fino alla prima barra che raggiunge i 60 minuti dall'entry:

```text
ChartTimeFrame / PathBarOrdinal / ElapsedMinutes
OHLC / CandleVolume
DirectionalMoveRanges
FavorableMfeToDateRanges / AdverseMfeToDateRanges
PriceState / PocTouchedThisBar / PocTouchedToDate
TradeCount / buy/sell volume / delta / max trade
```

La granularita' e' quella del chart ATAS. Su M5 produce normalmente 12 barre path, oppure 13 quando la dodicesima termina pochi secondi prima dei 60 minuti; su M1 circa 60. E' ammessa una tolleranza massima di 5 minuti sul timestamp finale per includere la prima barra M5 completata che raggiunge l'ora di osservazione. L'API indicatore installata non espone una richiesta candle M1 storica separata da un chart M5: per ottenere path M1 il modello deve essere applicato a un chart M1.

Checkpoint:

```text
[MR_SHADOW_ACCEPTANCE_OUTCOME]
DirectionalMoveRanges  positivo se il prezzo continua nella direzione shadow
FavorableMfeRanges     massima escursione favorevole entro l'orizzonte
AdverseMfeRanges       massima escursione contraria entro l'orizzonte
EndInsideRange         close H6/H12 dentro il range congelato
PocTouched             POC attraversato entro H6/H12
```

Nessun ordine, stop, target, posizione, PnL o marker `[MR_ENTRY]/[MR_EXIT]` viene creato. Le ipotesi sono promosse soltanto a diagnostica live prospettica.

Marker della conferma LOW flow:

```text
[MR_SHADOW_LOW_FLOW_CONFIRMATION_ENTRY]
[MR_SHADOW_LOW_FLOW_CONFIRMATION_OUTCOME]
[MR_SHADOW_LOW_FLOW_CONFIRMATION_BAR]
OperationalEntry=FALSE
OrderSubmitted=FALSE
```

La discovery setup non e' limitata all'acceptance. Ogni evento ledger resta registrato e porta `DiagnosticState`:

```text
OUTSIDE_FIRST
OUTSIDE_ACCEPTANCE
INSIDE_BOUNDARY_INTERACTION
OPPOSITE_OUTSIDE
```

Solo `OUTSIDE_ACCEPTANCE` alimenta oggi la shadow continuation; gli altri stati restano disponibili per confronti diagnostici senza generare entry sovrapposte.

Il primo studio del path completo e' documentato in `docs/research/acceptance-path-transition-analysis-2026-07-11.md`. `CONTINUOUS_ACCEPTANCE`, rejection entro/dopo 15 minuti, POC touch e reacceptance sono descrittori futuri del path, non trigger operativi.

`LOW_ACCEPTANCE_FLOW_CONFIRMATION_V1` e' promossa a seconda shadow prospettica live/historical, ancora non validata. Dopo una acceptance LOW attende esattamente tre barre completate, calcola `-sum(Delta)/sum(TotalVolume)` e registra una shadow SHORT solo se il valore e' maggiore di zero. La soglia zero rappresenta il segno del flow e non e' ottimizzata. Entry, outcome H6/H12 e path 60 minuti sono ancorati alla close della terza barra. La baseline `ACCEPTANCE_CONTINUATION_V1` resta invariata e viene registrata anche quando la conferma flow non scatta.

### Nota visuale su POC

Se sull'indicatore ATAS il volume profile e' impostato su `Current Day`, il POC visuale puo' essere diverso dal target del modello.

Esempio del reload/live 2026-07-08:

```text
POC visuale Current Day: circa 29500
Target modello:          29540
Perche':                 Source=PreviousDayProfile, ReferenceLabel=2026-07-07
```

Questo e' storico del core reference. Nel ledger il POC e' un livello osservato: `PocTouched` dice soltanto se il prezzo lo ha attraversato entro l'orizzonte analizzato.

## Entry Legacy Retired

Le regole di setup/posizione del precedente core sono ritirate dal flusso attivo. Restano sotto solo come riferimento storico; non devono essere riattivate o confrontate come risultati live.

### Short mean reversion

```text
Condizione contesto:
- London session 08:00-16:00 London.
- Reference profile completo disponibile: POC, VAH, VAL.
- Candela fa high sopra reference VAH.
- Candela chiude di nuovo sotto reference VAH.
- Rejection minima: 10 tick.

Entry:
- Dopo la candela di rejection.
- Cumulative big trade Sell >= 20 contratti.
- Prezzo entry dentro reference value, tra VAH e POC.
- Target POC ancora davanti al trade.
- Reward/Risk verso reference POC >= 1.0.

Gestione:
- Stop vicino all'estremo fallito: high - 2 tick.
- Target full position al reference POC.
- A MFE >= 1R: stop a breakeven.
- Se ancora aperto, flat massimo a New York regular close 16:00 New York.
```

### Long mean reversion

```text
Condizione contesto:
- London session 08:00-16:00 London.
- Reference profile completo disponibile: POC, VAH, VAL.
- Candela fa low sotto reference VAL.
- Candela chiude di nuovo sopra reference VAL.
- Rejection minima: 10 tick.

Entry:
- Dopo la candela di rejection.
- Cumulative big trade Buy >= 20 contratti.
- Prezzo entry dentro reference value, tra VAL e POC.
- Target POC ancora davanti al trade.
- Reward/Risk verso reference POC >= 1.0.

Gestione:
- Stop vicino all'estremo fallito: low + 2 tick.
- Target full position al reference POC.
- A MFE >= 1R: stop a breakeven.
- Se ancora aperto, flat massimo a New York regular close 16:00 New York.
```

## Parametri Legacy Retired

```text
Big trade volume:       20 contratti
Rejection minima:       10 tick
Stop offset:            2 tick dentro l'estremo fallito
Entry timeout:          1200 secondi
Min RR verso POC:       1.0
Breakeven trigger:      1.0R
Breakeven offset:       0 tick
Sessione entry:         London 08:00-16:00
Massima durata trade:   fino a New York regular close 16:00 New York
```

## Visual Chart

Gli oggetti ATAS vengono disegnati sia per replay storico sia per eventi live. Gli orari precisi restano nel log `Italy=`; il chart posiziona l'oggetto sul bar M5 che contiene l'evento.

```text
BalanceZoneTracker profile:      zona London grigia, POC/VAH/VAL solo contesto; non influenza il ledger.
DynamicCompression:             non disegnato.
Ledger events/outcomes:          non disegnati; leggibili solo nei log.
```

Il chart non promuove piu' un range o un'interazione a segnale. Il confronto avviene dai record causali del ledger.

## Log

```text
[MR_MODE]                    configurazione modello
[MR_REFERENCE_READY]         reference profile completato e disponibile
[MR_COMPRESSION_LEDGER_PROFILE]  riepilogo range, test, flow raw e copertura trade
[MR_COMPRESSION_LEDGER_EVENT]    touch/breach High/Low non qualificato
[MR_COMPRESSION_LEDGER_OUTCOME]  esito causale dell'evento a 1/3/6/12 barre
[MR_LOCAL_PROFILE_READY]     compressione locale riconosciuta; diagnostica only
[MR_LOCAL_PROFILE_RESOLVED]  compressione risolta da acceptance o fine sessione
[MR_PROFILE_CONTEXT]         marker storico legacy, non emesso nel flusso studio
[MR_SETUP_EXPIRED]           marker storico legacy, non emesso nel flusso studio
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[MR_REPLAY_AUDIT]            riepilogo replay e motivi no-entry
[MR_SETUP_NO_ENTRY]          diagnostica setup senza entry
[MR_ENTRY]                   marker legacy; non deve essere emesso dal nuovo modello
[MR_BREAKEVEN]               marker legacy; non deve essere emesso dal nuovo modello
[MR_REPLAY_OPEN]             marker legacy; non deve essere emesso dal nuovo modello
[MR_EXIT]                    marker legacy; non deve essere emesso dal nuovo modello
[MR_LIVE_HEARTBEAT]          heartbeat leggero live
[HISTORICAL_FLOW_FINISH]
```

Non esiste PnL studio. `[MR_EXIT]` resta l'unica fonte PnL per log storici legacy, ma non deve apparire dopo un reload del modello studio.

## File

```text
LondonMeanReversionModel.cs  unico file del modello pulito
```

## Build / Deploy

```bash
cd FabioOrderFlow/src && dotnet build -c Release
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$APPDATA/ATAS/Indicators/FabioOrderFlow.dll"
```

Dopo deploy: reload ATAS, attendere `[HISTORICAL_FLOW_FINISH]`, poi sommare solo `[MR_EXIT]`.
