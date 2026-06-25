# ATAS Log Reading

Questa è la guida canonica per interpretare i log di `FabioTrendFollowing`.

## 1. Principi generali

- Il file log è unico e viene svuotato a ogni inizializzazione dell'indicatore.
- Il prefisso temporale all'inizio di riga è esplicitato come `Italy=yyyy-MM-dd HH:mm:ss.fff`; è la data/ora di scrittura del log in formato italiano, non il tempo di mercato.
- I timestamp utili per il mercato sono quelli embedded nei campi `UTC=`, `London=` e `Italy=`.
- Per l'analisi operativa usa sempre l'orario italiano come riferimento principale.
- Usa `UTC` solo come supporto quando devi riallineare il timestamp.

## 2. File log

```text
%APPDATA%/ATAS/Logs/FabioTrendFollowing.log
```

Questo è il file corrente dell'indicatore.

## 3. Come leggere le barre

- Le barre M5 sono stampate sull'`open time` della barra.
- Esempio: `Italy=16:55` indica la candela che chiude alle `17:00`.
- Quando vedi `LONDON_PRE_CLOSE`, stai leggendo l'ultima parte della sessione London, non la chiusura effettiva.

## 4. Marker principali

### Contesto / value area

- `[PROFILE_PREVIEW]` — contesto di `POC`, `VAH`, `VAL` e value area.
- `[NEW_SESSION_LOW]` / `[NEW_SESSION_HIGH]` — nuovi estremi di sessione.
- `[HIGH_REJECTION_CANDIDATE]` / `[LOW_REJECTION_CANDIDATE]` — sweep o rejection candidate.

### Trigger detection

- `[MR_EARLY_TRIGGER]` — trigger anticipato (POC reclaim/loss prima di M5).
- `[MR_TRIGGER]` — trigger M5 confermato (POC reclaim/loss o follow-through).

### Aggression confirmation

**Historical Mode** (sempre attivo su backtest/replay):
- `[MR_AGGRESSION_CONFIRM]` — Conferma entry con timestamp intrabar preciso (millisecondi).
  - `EntryModel=FootprintCumulativeTradeHistorical`
  - Entry individuata tramite batch processing su CumulativeTrades.
  - Timestamp esempio: `London=2026-06-25 09:04:47.686` (dentro barra 09:00-09:05).
  - `SecondsAfterSweep` indica ritardo preciso dal sweep (es: 105.0 secondi).

**Live Mode** (solo durante trading live, se `EnableLiveFootprintFirst=true`):
- `[FOOTPRINT_HIGH_SWEEP]` — Rilevamento sweep high real-time.
- `[FOOTPRINT_LOW_SWEEP]` — Rilevamento sweep low real-time.
- `[FOOTPRINT_REJECTION]` — Rilevamento rejection real-time.
- `[FOOTPRINT_ENTRY]` — Entry confermata real-time (prima chiusura bar).
- Questi tag appaiono SOLO durante live trading tick-by-tick, non su dati storici.

**Differenze chiave:**
- **Historical:** Batch post-processing dopo chiusura bar trigger, ma entry timestamp è comunque intrabar preciso.
- **Live:** Detection real-time tick-by-tick prima della chiusura bar, per alert anticipati.
- **Criteri:** Identici in entrambi i modi (volume >= minVolume, direction, value area).
- **Precisione:** Entrambi individuano entry intrabar con millisecondi, non solo chiusura bar.

### Outcome

- `[MR_MFE_UPDATE]` — aggiornamento movimento favorevole durante posizione attiva.
- `[MR_TARGET_HIT]` — target raggiunto (POC o Target2).
- `[MR_POSITION_CLOSED]` — posizione chiusa (TARGET_HIT o STOP_HIT).
- `[MR_INVALIDATED]` — (codice morto, mai generato).

## 5. Regola di lettura

1. Filtra prima per fascia oraria italiana.
2. Interpreta la barra come contesto o candidate.
3. Interpreta il footprint come timing di entry.
4. Considera sempre la timezone dichiarata dal log quando confronti eventi e prezzi.

## 6. Note per i modelli

### Modello 1 — Trend Following

- Il log serve a leggere balance, breakout, acceptance e continuation.
- La documentazione di riferimento del modello è `../Modello-1-TrendFollowing/MODELLO-1-DOCUMENTAZIONE.md`.

### Modello 2 — Mean Reversion

- Il log serve a leggere fakeout, rejection e ritorno verso `POC`.
- La documentazione di riferimento del modello è `../Modello-2-MeanReversion/FabioMeanReversion.md`.

## 7. Regola pratica

Se devi capire cosa è successo sul mercato, parti sempre da qui, poi passa alla documentazione del modello specifico.
