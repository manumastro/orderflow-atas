# Contratto: Historical Cumulative Context Recorder

## Stato

```text
Tipo:                 raccolta osservativa storica da chart ATAS
Schema log:           fof-historical-cumulative-context-v2
Prefisso log:         FofHistoricalContext
Modello attivo:       nessuno
Segnali / ordini:     nessuno
Output grafico:       nessuno
```

Questo contratto permette di usare dati storici gia' caricabili in ATAS senza dover registrare una nuova sessione live. Il recorder resta neutro: salva contesto e `CumulativeTrade` storici, ma non calcola segnali, non filtra setup e non invia ordini.

## Domanda

Possiamo costruire un archivio storico utile al metodo del corso, collegando partecipazione aggregata, profilo, value area, POC di candle e risposta successiva, anche senza raw trade live tick-by-tick?

## API ATAS Usata

ATAS espone lo storico dei `CumulativeTrade` tramite:

```text
RequestForCumulativeTrades(CumulativeTradesRequest)
OnCumulativeTradesResponse(CumulativeTradesRequest, IEnumerable<CumulativeTrade>)
```

`CumulativeTradesRequest(beginTime, endTime, 0, 0)` richiede tutti gli aggregati storici del range indicato senza filtro di volume. Il limite documentato e' sette giorni per richiesta. Se il chart contiene un range piu' lungo, il recorder lo divide in finestre consecutive da massimo sette giorni e invia una richiesta ATAS per ogni finestra.

Il contesto storico viene preso dalle candle caricate nel chart tramite `GetCandle(bar)`. Per ogni candle vengono salvati OHLC, volume, bid, ask, delta, ticks, VWAP, value area, POC di candle e tutti i livelli footprint disponibili con ask, bid, volume, delta, ticks, between e time.

Il recorder usa schema `fof-historical-cumulative-context-v2`. Lo schema `v1` e' stato usato solo nel primo smoke test e poteva emettere `historical-context-skipped` quando il chart superava sette giorni.

## Cosa Non Fa

Il recorder non richiede raw trade storici arbitrari. L'API documentata espone `OnNewTrade` per dati live e `GetTradesCache(period)` come cache recente, non come backfill tick-by-tick su un giorno passato. Quindi questo schema non e' equivalente a `fof-session-observation-v2`: non ricostruisce il POC di sessione al tick esatto dell'evento partendo da tutti i raw trade storici.

Il confronto corretto e':

```text
fof-session-observation-v2:
  raw trade live + CumulativeTrade live + POC in sviluppo tick-by-tick + risposta raw a 300s

fof-historical-cumulative-context-v2:
  candle/footprint storici caricati + CumulativeTrade storici + risposta futura ricostruibile da candle/barre
```

Le finestre cumulative hanno un confine temporale condiviso per evitare buchi tra due richieste ATAS. Un parser offline deve quindi deduplicare eventuali `CumulativeTrade` identici emessi esattamente sul confine tra due finestre.

## Uso Operativo

1. Aprire in ATAS un chart del future Mini NQ con il periodo storico desiderato gia' caricato.
2. Preferire un range compatto: il recorder puo' dividerlo in richieste da massimo sette giorni, ma il log cresce con ogni candle e ogni `CumulativeTrade` storico.
3. Caricare **Fabio Historical Cumulative Context Recorder**.
4. Attendere il ricalcolo e la risposta storica ATAS.
5. Leggere i log `FofHistoricalContext` dal log applicativo ATAS.

Il recorder usa il range effettivamente caricato nel chart: `GetCandle(0).Time` come inizio e `GetCandle(CurrentBar - 1).LastTime` come fine. Se il range non contiene barre, registra uno skip esplicito. Se il range supera sette giorni, non tronca i dati: emette piu' `historical-cumulative-requested`, uno per finestra ATAS valida.

## Record Emessi

```text
historical-context-started        range, strumento e numero di finestre richiesta
chart-candle                      candle storica con footprint levels
historical-cumulative-requested   richiesta CumulativeTradesRequest inviata, con requestSequence/requestCount
historical-cumulative-trade       CumulativeTrade storico con ticks interni e finestra richiesta
historical-cumulative-response    conteggio finale della risposta per finestra
historical-context-skipped        motivo di esclusione del range vuoto
```

## Regole Di Interpretazione

- Un giorno storico puo' diventare un case study, non una validazione.
- La lettura deve partire dal contesto: profilo, value area, VWAP, POC, fase di asta e posizione del prezzo.
- I `CumulativeTrade` descrivono partecipazione/effort; il risultato va letto solo rispetto al contesto e alla risposta successiva.
- Non dichiarare intenzione dei partecipanti. Si puo' dire che un effort e' stato premiato, assorbito o non seguito dal prezzo, non perche' qualcuno ha comprato/venduto.
- Nessuna soglia di volume, distanza, delta o risposta viene promossa da questo contratto.

## Output Previsto

I log ATAS completi restano locali. Una fase successiva potra' produrre snapshot filtrati e report offline sotto `FabioOrderFlow/ledger-snapshots/` e `docs/research/`, con hash SHA-256 come per gli altri studi.
