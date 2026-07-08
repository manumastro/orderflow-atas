# LondonMeanReversionModel

Modello attivo di `FabioOrderFlow`.

`MR` significa **Mean Reversion**. Tutti i log `MR_*` appartengono al modello operativo, sia live sia historical replay.

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
8. Se e' sbagliato, stop piccolo vicino all'estremo fallito.
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
Non si usa il developing POC della London corrente per generare entry.
```

Motivo: il developing POC della sessione corrente era troppo vicino al fakeout e produceva target minuscoli, touch immediati e poche entry.

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
Sessione operativa:     London 08:00-16:00
```

## Log

```text
[MR_MODE]                    configurazione modello
[MR_REFERENCE_READY]         reference profile completato e disponibile
[MR_SETUP_LONG]              failed auction sotto reference VAL, rientro in value
[MR_SETUP_SHORT]             failed auction sopra reference VAH, rientro in value
[MR_SETUP_EXPIRED]           setup scaduto o POC gia' toccato prima dell'entry
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[MR_REPLAY_AUDIT]            riepilogo replay e motivi no-entry
[MR_SETUP_NO_ENTRY]          diagnostica setup senza entry
[MR_ENTRY]                   posizione creata
[MR_BREAKEVEN]               stop portato a breakeven dopo MFE >= 1R
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
