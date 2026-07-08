# LondonMeanReversionModel

Modello attivo di `FabioOrderFlow`.

`MR` significa **Mean Reversion**. Tutti i log `MR_*` appartengono al modello operativo, sia live sia historical replay.

## Baseline corrente

```text
Baseline:        2026-07-08 reference + breakeven + NY close hold
Code commit:     f20ec7b
Validation docs: 26b17f5
Tag stabile:     london-ny-close-hold
Reload:          2026-07-08 10:40
```

Risultato storico validato dal reload:

```text
Reference disponibili: PreviousDayProfile + PreviousLondonProfile
MR_REFERENCE_READY:    12
MR_ENTRY historical:   20
MR_EXIT historical:    20
MR_BREAKEVEN:          12
MR_REPLAY_OPEN:        0
NEW_YORK_CLOSE:        0
LONDON_CLOSE:          0
PnL da MR_EXIT:        +634,25
Open historical:       0
```

Questa baseline e' il punto fisso di partenza. Modifiche future devono confrontarsi con questo stato e avere un nuovo checkpoint/tag.

## Tesi Fabio

Fonte: `transcription.txt`.

Playbook unico:

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

Il modello ha solo due modalita':

```text
LIVE        dati real-time ATAS
HISTORICAL  dati passati processati con le stesse regole live
```

Il replay storico usa gli stessi metodi della live. Non esistono entry di ricerca/debug parallele.

## Reference Value Areas

Il modello usa due reference profile completati, entrambi validi live e replay:

```text
PreviousDayProfile       profilo completo del giorno italiano precedente
PreviousLondonProfile    profilo completo della London session precedente
```

Regola importante:

```text
Non si usa il developing POC della London/current day corrente per generare entry.
```

Motivo: il developing POC della sessione corrente era troppo vicino al fakeout e produceva target minuscoli, touch immediati e poche entry.

## Profile Diagnostics

Fabio distingue tra versione semplice e versione avanzata:

```text
Versione semplice:  usare il previous day profile / previous balance area.
Versione avanzata: identificare la compressione/dealing range che il mercato costruisce durante la sessione e plottare il profilo su quella zona.
```

Per non sporcare la baseline, il modello usa una diagnostica unica e poco invasiva:

```text
Marker:       [MR_PROFILE_CONTEXT]
Uso:          DIAGNOSTIC_ONLY
Livello log:  ENTRY_ONLY
Entry/target: invariati, sempre PreviousDayProfile/PreviousLondonProfile completati
```

I profili vengono comunque agganciati anche al setup per audit interno; il marker dedicato viene emesso solo sull'entry per evitare ingombro nel log live/replay.

Ogni riga espone `ProfileSource`, cosi' le diagnostiche restano separate senza usare marker diversi:

```text
CurrentLondonSessionProfile  profilo broad da inizio London fino al setup
LocalRotationProfile         profilo locale dalla rotazione/pivot piu' recente fino al setup
```

`CurrentLondonSessionProfile` serve a vedere la value area ampia della London corrente. `LocalRotationProfile` e' direzionale: per short parte dall'ultimo swing low utile; per long parte dall'ultimo swing high utile. Questo approssima la lettura Fabio del profilo sulla compressione/rotazione intraday, senza usare il profilo finale del giorno.

Campi principali:

```text
ProfileSource
ProfilePOC
ProfileVAH
ProfileVAL
ProfileValueWidth
ProfileHigh / ProfileLow
CandidateTargetPOC
TargetVsProfileVAL / TargetVsProfilePOC / TargetVsProfileVAH
EntryVsProfileVAL / EntryVsProfilePOC / EntryVsProfileVAH
ProfileUse=DIAGNOSTIC_ONLY
```

Serve per studiare casi come il 2026-07-06. Questa diagnostica non blocca trade e non cambia PnL; eventuali filtri futuri dovranno essere validati contro la baseline `london-ny-close-baseline`.

## Potenziali Passi Successivi

Questa sezione e' solo roadmap. Nessuna voce e' operativa finche' non appare nei log come entry reale e non viene validata con reload.

### Fabio-style local profile filter

Principio non derivato solo dai dati raccolti, ma dalla logica di Fabio:

```text
Location prima del trigger.
Non basta vedere un big trade: devo sapere se sto entrando da una zona di edge o se sto inseguendo dentro value.
```

Lettura candidata:

```text
Long con entry sotto LocalRotation VAL = fragile.
```

Motivo Fabio-style:

```text
Se per un long il prezzo e' ancora sotto la VAL del profilo locale, il mercato non ha ancora riaccettato la rotazione locale.
Il buy puo' essere solo un tentativo anticipato; Fabio aspetterebbe reclaim/accettazione e aggressione, oppure un retest della value recuperata.
```

Lettura simmetrica:

```text
Short con entry gia' sotto LocalRotation POC = fragile.
```

Motivo Fabio-style:

```text
Per uno short mean-reversion la location migliore e' premium / upper value / fuori VAH che rientra.
Se entro short sotto il POC locale, sto vendendo nella meta' bassa della rotazione, non da un edge. Quello assomiglia piu' a continuation/acceptance lower, quindi richiede un modello separato.
```

Queste non sono ancora regole operative. Devono essere validate su piu' sessioni, soprattutto contro il 2026-07-02, per evitare overfitting.

### MR_RETEST_ENTRY

Possibile nuova tipologia esplicita, separata dall'entry attuale:

```text
1. Setup failed auction su PreviousDayProfile/PreviousLondonProfile.
2. Big trade nella direzione mean-reversion conferma il setup.
3. Non entro subito sul big trade.
4. Aspetto retest della reference edge recuperata: VAL per long, VAH per short.
5. Entro solo se il retest tiene e compare nuova risposta/aggressione coerente.
6. Stop vicino all'estremo fallito; target reference POC.
```

Non deve coesistere in modo silenzioso con l'entry immediata. Modalita' possibili:

```text
AggressionOnly              baseline attuale
RetestOnly                  futura modalita' operativa alternativa
AggressionWithRetestShadow  baseline operativa + log candidate retest senza PnL
```

### MR_LOCAL_PROFILE_ENTRY

Possibile modello/entry separata dove il profilo locale diventa sorgente di setup o di filtro primario.

Da definire prima di renderla live:

```text
EntryProfileSource=LocalRotationProfile
TargetProfileSource=PreviousDayProfile/PreviousLondonProfile oppure LocalRotationProfile
Regole di acceptance/retest
Regole anti-doppia entry rispetto a MR_AGGRESSION_ENTRY
```

### Nota visuale su POC

Se sull'indicatore ATAS il volume profile e' impostato su `Current Day`, il POC visuale puo' essere diverso dal target del modello.

Esempio del reload/live 2026-07-08:

```text
POC visuale Current Day: circa 29500
Target modello:          29540
Perche':                 Source=PreviousDayProfile, ReferenceLabel=2026-07-07
```

Questo e' corretto: Fabio nella versione semplice parla di `last day profile / previous balance area`. Il target operativo va letto sempre da:

```text
[MR_REFERENCE_READY] Source=... ReferenceLabel=... POC=...
[MR_ENTRY] TargetPOC=...
```

## Entry Operativa

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

## Parametri

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

## Log

```text
[MR_MODE]                    configurazione modello
[MR_REFERENCE_READY]         reference profile completato e disponibile
[MR_SETUP_LONG]              failed auction sotto reference VAL, rientro in value
[MR_SETUP_SHORT]             failed auction sopra reference VAH, rientro in value
[MR_PROFILE_CONTEXT]         diagnostica profili intraday; ProfileSource separa CurrentLondonSessionProfile e LocalRotationProfile
[MR_SETUP_EXPIRED]           setup scaduto o POC gia' toccato prima dell'entry
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[MR_REPLAY_AUDIT]            riepilogo replay e motivi no-entry
[MR_SETUP_NO_ENTRY]          diagnostica setup senza entry
[MR_ENTRY]                   posizione creata
[MR_BREAKEVEN]               stop portato a breakeven dopo MFE >= 1R
[MR_REPLAY_OPEN]             posizione storica ancora aperta a fine dati replay; PnL non contato
[MR_EXIT]                    exit finale; PnL valido
[MR_LIVE_HEARTBEAT]          heartbeat leggero live
[HISTORICAL_FLOW_FINISH]
```

PnL valido: sommare solo `[MR_EXIT]`.

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
