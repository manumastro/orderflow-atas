# Contratto: Historical Cumulative Cash Session Description

## Stato

```text
Tipo:                 descrizione storica osservativa
Sorgente:             fof-historical-cumulative-context-v5
Strumento:            NQU6@CME
Sessione:             NQ US Cash, 09:30-16:00 America/New_York
Segnali / ordini:     nessuno
PnL:                  nessuno
```

Questo studio usa l'inventario storico `v5` del 2026-08-04. Non modifica il recorder e non promuove un modello. Il suo scopo e' capire quali descrizioni del contesto sono disponibili nello storico ATAS e se la risposta dopo i `CumulativeTrade` va distinta dal movimento temporale normale della sessione.

## Popolazione

Il feed storico registra timestamp senza offset. Per `NQU6` sono trattati come UTC, coerentemente con la verifica live del recorder di sessione. Ogni timestamp viene convertito esplicitamente in `America/New_York` prima del filtro sessione.

Un evento e' incluso solo quando il suo tempo locale e' compreso in:

```text
09:30:00 <= eventTimeNewYork < 16:00:00
```

Le candle sono incluse con lo stesso criterio sul loro `beginTime` locale. I giorni con una copertura cash parziale restano nel dataset, ma sono marcati come parziali; non vengono fatti passare per sessioni complete.

## Contesto Osservato

Ogni evento cash viene collegato alla candle che lo contiene. Il record derivato conserva:

```text
- lato e volume del CumulativeTrade;
- prezzo iniziale/finale e variazione intrinseca dell'aggregato;
- OHLC, volume, delta e VWAP della candle;
- POC e value area della candle;
- posizione del prezzo evento rispetto a POC, value area e VWAP;
- risposta da close candle a 60, 180, 300 e 900 secondi, quando disponibile nella stessa cash session.
```

La POC e la value area sono quelle della candle storica resa da ATAS, non un POC di sessione ricostruito tick-by-tick. La risposta e' una misura descrittiva in tick tra `lastPrice` dell'evento e il close della prima candle disponibile al o dopo l'orizzonte.

## Baseline E Overlap

La stessa risposta e' calcolata da ancore fisse a ogni candle cash. Il confronto eventi/ancore serve solo a controllare il regime temporale: non e' un test di significativita' e non dimostra causalita'.

Gli eventi sono fortemente sovrapposti. Per ogni giorno viene quindi costruita anche una sequenza non sovrapposta: seleziona il primo evento con risposta completa a 900 secondi e poi il primo evento almeno 900 secondi dopo. Questa sequenza e' un controllo di dipendenza, non una selezione di setup.

## Limiti

- `CumulativeTrade` storici non equivalgono al backfill raw `OnNewTrade`.
- Il dataset copre sei date cash, ma le date iniziale e finale sono parziali; non valida probabilita' o un edge.
- Le classificazioni POC/value area/VWAP sono descrittive e dipendono dalla granularita' della candle caricata.
- Nessuna soglia di volume, delta, distanza o risposta viene usata per generare segnali.
- Non viene attribuita intenzione ai partecipanti: si descrivono effort osservato e risultato di prezzo nel contesto disponibile.

## Output

Gli artefatti locali ignorati da Git sono:

```text
historical-cumulative-context-2026-08-04-v5-cash-events.csv
historical-cumulative-context-2026-08-04-v5-cash-anchors.csv
historical-cumulative-context-2026-08-04-v5-cash-non-overlap-events.csv
historical-cumulative-context-2026-08-04-v5-cash-summary.json
```

Il report canonico e' `historical-cumulative-cash-session-description-2026-08-04.md` e registra gli hash SHA-256 degli artefatti.
