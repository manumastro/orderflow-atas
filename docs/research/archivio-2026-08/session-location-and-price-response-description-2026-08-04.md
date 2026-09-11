# Descrizione Di Sessione: Location POC E Risposta Prezzo

## Stato

```text
Tipo:                 report descrittivo offline
Sessione:             NQ US Cash 2026-08-04
Schema incluso:       fof-session-observation-v2
Schema escluso:       fof-session-observation-v1
Modello attivo:       nessuno
Segnali / ordini:     nessuno
```

## Provenienza

- Snapshot locale: `FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2.jsonl.gz`
- SHA-256 snapshot: `e5448962f25401774c119f152c178463715471f1f46c6b678f5e0febd5424153`
- Tabella evento: `FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-events.csv`
- SHA-256 tabella: `8c145ce67b4061dc62667fa4a1a02a8f0290bb9452296705c696ff25829cda7c`
- Log ATAS sorgente: `C:\Users\emanu\AppData\Roaming\ATAS\Logs\app_20260804.log`
- Byte del log al primo accesso: `489223069`
- Record v2 nello snapshot: `286898`
- Righe ATAS malformate escluse: `0`

Il log ATAS completo non e' incluso in Git. Lo snapshot compresso contiene soltanto i JSON v2 usati dal report; i file v1 restano esclusi.

## Copertura E Integrita'

```text
session id:                 20260804-NQ US Cash
primo raw UTC:              2026-08-04T13:30:00.000445
primo raw America/New_York: 2026-08-04T09:30:00.000445
ultimo raw America/New_York:2026-08-04T10:17:00.283881
raw trade:                  101879
eventi finali unici:        61590
eventi completi:            57534
eventi incompleti:          4056
mismatch volume tick:       0
profilo avviato in orario:  True
```

Un evento e' completo soltanto quando la somma dei suoi tick coincide con il volume finale, il profilo parte entro le 09:30 e sono osservabili raw trade fino a 300 secondi dopo il tick finale. Gli eventi nell'ultima finestra di cinque minuti restano nel CSV ma non nelle statistiche sotto.

## Descrizione Congiunta

```text
location:                   {"above-all-pocs": 44056, "at-poc": 233, "below-all-pocs": 13156, "between-tied-pocs": 89}
ritorno a POC congelato:    {"false": 29851, "true": 27683}
POC con parita':            6880
distanza assoluta POC tick: {"count": 57534, "median": "120", "p10": "15", "p90": "195"}
escursione up tick:         {"count": 57534, "median": "177", "p10": "51", "p90": "368"}
escursione down tick:       {"count": 57534, "median": "-60", "p10": "-212", "p90": "-11"}
prezzo finale futuro tick:  {"count": 57534, "median": "124", "p10": "-57", "p90": "315"}
```

## Per Lato Dell'Evento

```json
{
  "Buy": {
    "count": 28800,
    "medianMinimumAbsolutePocDistanceTicks": "121",
    "medianResponseDownTicks": "-61",
    "medianResponseUpTicks": "176",
    "returnedToFrozenPocCount": 13783
  },
  "Sell": {
    "count": 28734,
    "medianMinimumAbsolutePocDistanceTicks": "119",
    "medianResponseDownTicks": "-59",
    "medianResponseUpTicks": "178",
    "returnedToFrozenPocCount": 13900
  }
}
```

Queste sono distribuzioni descrittive della singola raccolta. Non definiscono assorbimento, accettazione, rifiuto, probabilita', filtro o modello; non sono base per segnali o ordini.

## Metodo Riproducibile

1. Il parser filtra esclusivamente `fof-session-observation-v2`.
2. Per ogni EventId seleziona lo stato finale per volume totale massimo, poi ultimo tick, poi update maggiore.
3. Ordina i raw trade UTC per tempo e sequenza; aggiorna il profilo solo fino all'ultimo tick dell'evento.
4. Conserva tutti i POC a parita' e calcola il vettore delle distanze in tick.
5. Misura il primo, massimo, minimo e ultimo raw trade nei 300 secondi strettamente successivi al tick finale.
