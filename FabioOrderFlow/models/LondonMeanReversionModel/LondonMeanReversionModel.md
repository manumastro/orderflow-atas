# LondonMeanReversionModel

Modello attivo di `FabioOrderFlow`.

`MR` significa **Mean Reversion**. Tutti i log `MR_*` appartengono al modello operativo, sia live sia historical replay.

## Tesi Fabio

Fonte: `transcription.txt`.

Fabio descrive un solo playbook mean-reversion per London:

```text
1. Il mercato London e' in compressione / balance.
2. Prezzo esce dalla value area: sweep/fakeout sopra VAH o sotto VAL.
3. Il breakout fallisce e la candela torna dentro value.
4. Non si entra sul primo movimento cieco.
5. Si attende aggressione reale: cumulative big trade nella direzione di rientro.
6. Target = bulk dell'asta / POC, non l'estremo opposto della range.
7. Se il trade e' sbagliato deve essere sbagliato subito: stop piccolo vicino all'estremo fallito.
```

Frasi chiave dal transcript:

```text
London sugli indici tende spesso a mean revert.
Non prendo il primo movimento; aspetto breakout, rientro nel balance e big trades.
Il target e' il POC / bulk dell'asta.
Se sono sbagliato voglio esserlo immediatamente.
Filtro big trades London circa 20 contratti.
Usare un solo modello per non creare confusione.
```

## Contratto Live / Historic

Il modello ha solo due modalita':

```text
LIVE        dati real-time ATAS
HISTORICAL  dati passati processati con le stesse regole live
```

Il modulo contiene solo questa entry. Non contiene logiche di ricerca, debug operativo parallelo o modelli alternativi.

Regola: se una logica non puo' esistere identica live, non deve produrre risultati storici.

## Entry Operativa Unica

### Short mean reversion

```text
Condizione contesto:
- London session 08:00-16:00 London.
- Profile preview disponibile: POC, VAH, VAL.
- Nuovo high London sopra VAH.
- Candela chiude di nuovo sotto VAH.
- Rejection minima: 10 tick.

Entry:
- Dopo la candela di rejection.
- Cumulative big trade Sell >= 20 contratti.
- Prezzo entry dentro value, tra VAH e POC.
- Target POC ancora davanti al trade.
- Reward/Risk verso POC >= 1.0.

Gestione:
- Stop vicino all'estremo fallito: high - 2 tick.
- Target full position al POC.
```

### Long mean reversion

```text
Condizione contesto:
- London session 08:00-16:00 London.
- Profile preview disponibile: POC, VAH, VAL.
- Nuovo low London sotto VAL.
- Candela chiude di nuovo sopra VAL.
- Rejection minima: 10 tick.

Entry:
- Dopo la candela di rejection.
- Cumulative big trade Buy >= 20 contratti.
- Prezzo entry dentro value, tra VAL e POC.
- Target POC ancora davanti al trade.
- Reward/Risk verso POC >= 1.0.

Gestione:
- Stop vicino all'estremo fallito: low + 2 tick.
- Target full position al POC.
```

## Log

Log operativi:

```text
[MR_MODE]                    configurazione modello pulito
[MR_SETUP_LONG]              failed auction sotto VAL, rientro in value
[MR_SETUP_SHORT]             failed auction sopra VAH, rientro in value
[MR_SETUP_EXPIRED]           setup scaduto o POC gia' toccato prima dell'entry
[MR_HISTORICAL_TRADES]       cumulative trades storici ricevuti
[HISTORICAL_FLOW_PROCESS_START]
[HISTORICAL_FLOW_FINISH]
[MR_ENTRY]                   posizione creata
[MR_EXIT]                    exit finale; PnL valido
[MR_LIVE_HEARTBEAT]          heartbeat leggero live
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
