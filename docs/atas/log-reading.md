# ATAS Log Reading

Guida canonica per interpretare i log di `FabioOrderFlow`.

## Principi

- Il file corrente viene azzerato a ogni inizializzazione indicatore.
- Il prefisso `WriteItaly` e' l'ora di scrittura del log, non necessariamente l'ora di mercato.
- Per eventi di mercato usa i campi embedded `Italy=`, `London=` e `UTC=`.
- Per analisi operativa usa l'orario italiano come riferimento principale.
- Il modello corrente e' uno studio senza trade: dopo reload non devono apparire nuovi `[MR_ENTRY]` o `[MR_EXIT]`.
- `[MR_EXIT]` e' la sola fonte PnL soltanto per i log legacy precedenti al passaggio allo studio.

## File corrente

```text
%APPDATA%/ATAS/Logs/FabioOrderFlow.log
```

## FabioCompressionStudy

Il modello attivo e' `COMPRESSION_CASES_NO_TRADES`.

```text
LIVE        osserva bar e cumulative trades real-time; nessun ordine.
HISTORICAL  ripete lo stesso studio sui dati ATAS; nessun PnL.
```

Marker attivi:

```text
[MR_MODE]                         deve mostrare StudyMode=COMPRESSION_CASES_NO_TRADES e OperationalEntries=DISABLED
[ZONE_READY]                      output profile legacy del BalanceZoneTracker; non influisce sullo studio
[MR_REFERENCE_READY]              PreviousDay/PreviousLondon costruiti esclusivamente come log
[MR_LOCAL_PROFILE_READY]          compression congelata e causalmente disponibile
[MR_LOCAL_PROFILE_RESOLVED]       fine osservazione dopo acceptance/fine sessione
[MR_COMPRESSION_STUDY_CASE]       acceptance breakout, qualificata oppure esclusa
[MR_COMPRESSION_STUDY_CANDIDATE]  candidato grafico/log; OperationalEntry=FALSE
[MR_COMPRESSION_STUDY_PROFILE]    test alto/basso, volume al bordo, CVD e conteggio candidati
[HISTORICAL_FLOW_FINISH]          deve riportare Entries=0 e StudyCandidates=N
```

## Come leggere lo studio

1. Trova `[MR_LOCAL_PROFILE_READY]`: solo da questo momento il range e' studiabile.
2. Leggi `[MR_LOCAL_PROFILE_RESOLVED]` per il tipo di esito.
3. Leggi `[MR_COMPRESSION_STUDY_PROFILE]`: `HighTests`, `LowTests`, volumi aggressivi al bordo e `ProfileCVD`.
4. Leggi ogni `[MR_COMPRESSION_STUDY_CANDIDATE]`:
   - `REVERSION_LONG/SHORT`: aggressione al bordo assorbita, rientro e big trade opposto;
   - `BREAKOUT_LONG/SHORT`: due close in acceptance, big trade e CVD coerenti.
5. `OperationalEntry=FALSE` e' obbligatorio. I marker sul chart sono candidati, non eseguiti.

## POC legacy vs Studio

La sezione seguente descrive il precedente core MR ed e' solo storica. Il nuovo studio usa il POC della compression come target candidato solo per i casi reversion; non esiste un target breakout ancora definito.

Il POC disegnato da un indicatore volume profile ATAS dipende dalla sua impostazione.

Se il profile visuale e' impostato su `Current Day`, puo' mostrare il developing POC corrente. Il modello London MR invece usa reference complete:

```text
PreviousDayProfile
PreviousLondonProfile
```

Quindi il POC visuale puo' differire da `[MR_ENTRY] TargetPOC`.

Esempio:

```text
Visual current-day POC circa 29500
[MR_ENTRY] TargetPOC=29540, Source=PreviousDayProfile, ReferenceLabel=2026-07-07
```

In caso di dubbio, il target operativo e' sempre quello loggato in `[MR_ENTRY] TargetPOC` e confermato da `[MR_REFERENCE_READY] POC`.

## BalanceZoneTracker Corrente

`BalanceZoneTracker` e' affidabile e resta invariato: identifica London e, per ora, mantiene/propaga il precedente contesto profile. `[ZONE_READY]`, POC/VAH/VAL, high/low e state machine non partecipano alla classificazione compression. Il prossimo refactor separato lo ridurra' a `LondonTracker`, responsabile soltanto di identificare inizio, fine e appartenenza alla sessione London.

## Compression Study

I marker della compression generano candidati studio, ma non segnali d'ordine e non PnL.

```text
[MR_LOCAL_PROFILE_READY]     score dinamico persistente su almeno 6 barre completate
[MR_LOCAL_PROFILE_RESOLVED]  2 close accettate fuori range o fine London
[MR_COMPRESSION_STUDY_CASE]       confronta acceptance, big trade e CVD
[MR_COMPRESSION_STUDY_CANDIDATE]  descrive un candidato non operativo
[MR_COMPRESSION_STUDY_PROFILE]    riassume l'intera finestra studiata
ProfileSource=ActiveCompressionProfile
ProfileUse=STUDY_INPUT_ONLY
```

Su `[MR_COMPRESSION_STUDY_PROFILE]` controllare che READY preceda il range studiato. Leggere `CompressionScore` e le componenti:

```text
ContractionScore / OverlapScore
DirectionalScore / RotationScore
ContainmentScore / BoundaryStabilityScore
PocStabilityScore
ValueConcentrationScore
BaselineMedianBarRange                 mediana della distribuzione precedente
RangeToBaselineMedian                 metrica, non filtro rigido
AverageBarRangeToBaselineMedian       metrica, non filtro rigido
```

Il profile diventa READY con score `>= 0,65` per 2 barre consecutive. La risoluzione richiede 2 close esterne; una singola wick non basta.

Non analizzare PnL sullo studio. `[MR_EXIT]` e' rilevante solo per dati legacy.

## Gestione Trade Legacy

Entry, stop, target e durata New York restano documentazione del core MR ritirato e non sono eseguiti dal modello studio.

Regola operativa:

```text
Entry window: London 08:00-16:00 London
Max hold:     New York regular close 16:00 New York
```

Nel periodo estivo normale questo corrisponde circa a:

```text
16:00 New York = 22:00 Italia = 20:00 UTC
```

Il codice usa `MarketTimeZones.NewYork`, quindi gestisce i cambi DST tramite timezone.

## Barre E Chart

- Le barre M5 sono stampate sull'open time della barra.
- I timestamp precisi delle entry arrivano dai cumulative trades e sono intrabar.
- Per ogni evento di mercato usare il campo `Italy=`: e' ora italiana. `WriteItaly=` indica solo quando ATAS ha scritto il log.
- Il chart posiziona candidati studio sul bar M5 che contiene il timestamp intrabar o la close di conferma.

Visual MR:

```text
DynamicCompression: box/POC/VAH/VAL turchese.
Reversion long: verde; reversion short: viola.
Breakout long: blu; breakout short: arancio.
Linea tratteggiata: target POC candidato solo per reversion.
```

## Regola pratica

Se devi capire il risultato del modello, usare il report canonico:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report performance non valuta il nuovo studio, perche' non esistono trade o PnL. Usarlo solo per confronti legacy basati su `[MR_EXIT]`.
