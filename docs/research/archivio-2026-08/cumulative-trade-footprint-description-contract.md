# Contratto Descrittivo: Cumulative Trade E Footprint Di Barra

## Stato

```text
Tipo:                 descrizione osservativa di una singola sessione
Fonte primaria:       CumulativeTrade ATAS e footprint di barra registrati
Campione:             cattura live NQU6@CME del 2026-08-04
Modello attivo:       nessuno
Segnali:              nessuno
Ordini / PnL:         nessuno
Approvazione:         autorizzato dall'utente nel dialogo corrente
```

Questo contratto usa soltanto i campi gia' registrati. Non prova che `CumulativeTrade` replichi DeepCharts `Aggregate`, non definisce un big trade universale e non costruisce una regola direzionale.

## Domanda

Nella sola sessione ATAS registrata il 2026-08-04, come co-occorrono il volume finale, la composizione in tick, il lato aggressivo e la posizione rispetto al POC della barra del singolo `CumulativeTrade`?

La domanda e' descrittiva. Non chiede se un evento predice il prezzo, identifica assorbimento o autorizza un playbook.

## Dati Ammessi

La fonte canonica della cattura e':

```text
report:       atas-cumulative-trade-capture-validation-2026-08-04.md
strumento:    NQU6@CME, E-Mini Nasdaq-100
live window:  2026-08-04T10:01:33.9753093 - 2026-08-04T11:09:13.6600612
unita':       uno stato finale per EventId della cattura live
```

Lo storico di 50.688 snapshot conferma la capacita' di raccolta, ma non entra nelle statistiche della sessione live. I suoi eventi non sono mischiati agli eventi live finali.

Non sono disponibili nel dataset corrente: profilo composito, VAH, VAL, HVN, LVN, VWAP, regime d'asta annotato, massimo/minimo futuro, ritorno del prezzo, esito o fuso orario esplicito del payload. Nessuno di questi campi puo' essere inferito o sostituito con una proxy silenziosa.

## Unita' E Feature Congelate

Per ogni `EventId`, usare l'ultimo record live disponibile nella finestra della cattura. Conservare anche l'identificativo, il numero di update e i flag di disponibilita'.

| Feature | Definizione congelata |
|---|---|
| Volume finale | `TotalVolume` dell'ultimo record dell'evento. |
| Tick finali | Numero di elementi `Ticks` dell'ultimo record. |
| Volume dei tick | Somma dei `Ticks.Volume` dell'ultimo record; puo' differire dal numero dei tick. |
| Lato | `Direction` dell'ultimo record, `Buy` o `Sell`. |
| Prezzo iniziale/finale | `FirstPrice` e `LastPrice` dell'ultimo record. |
| POC della barra | Livello `Footprint.Poc`; prezzo, ask, bid, volume e delta. |
| Delta POC | `Footprint.Poc.Delta`, equivalente a `Ask - Bid` al POC. |
| Distanza finale dal POC | `(LastPrice - Footprint.Poc.Price) / Security.TickSize`, in tick con segno. |
| Delta al primo/ultimo prezzo | `Footprint.FirstPrice.Delta` e `Footprint.LastPrice.Delta` solo se il livello esiste. |
| Disponibilita' livello | Flag separato per POC, primo prezzo e ultimo prezzo. |

La distanza in tick e' una coordinata descrittiva rispetto al POC della barra, non una classificazione di premio/sconto, valore, equilibrio o squilibrio.

## Metodo Pre-Registrato

Il report deve produrre soltanto:

1. conteggi degli eventi e dei campi disponibili/mancanti;
2. per volume finale, numero di tick, volume dei tick, delta POC e distanza dal POC: minimo, mediana, p90, p99 e massimo; per serie ordinate, la mediana e' il valore centrale o la media dei due valori centrali, mentre p90 e p99 usano il nearest-rank `ceil(p * n)`;
3. gli stessi riepiloghi separati per lato `Buy` e `Sell`;
4. conteggi congiunti tra lato e segno della distanza dal POC, lasciando invariata una distanza pari a zero;
5. una lista dei primi dieci eventi per volume finale e per numero di tick, ordinata in modo decrescente e, a parita', per `EventId` crescente, con i campi grezzi congelati;
6. le limitazioni del dataset, comprese le indisponibilita' dei livelli footprint.

Non introdurre bucket di volume, finestre temporali, filtri, correlazioni, test predittivi o etichette di assorbimento dopo avere visto il campione. Qualunque ulteriore segmentazione richiede un contratto nuovo e un campione separato.

## Criteri Di Promozione

Il report descrittivo puo' essere completato solo se:

- tutti i `EventId` live finali sono ricostruibili senza mescolare lo storico;
- la somma degli incrementi per evento coincide con il volume finale registrato nel report tecnico;
- la distanza dal POC usa il tick size registrato e non arrotonda silenziosamente;
- i livelli footprint mancanti restano null e sono conteggiati;
- il report dichiara esplicitamente che il campione e' una sola sessione e non supporta generalizzazioni.

Il completamento promuove soltanto una descrizione riproducibile della cattura. Non promuove H1, una soglia, un playbook, una previsione o una modifica al runtime.

## Criteri Di Scarto O Sospensione

Sospendere il report se:

- lo stato finale di un evento non e' identificabile;
- il volume incrementale e quello finale non riconciliano;
- il POC o il tick size non sono disponibili per un evento usato nella distanza;
- il parsing richiede correzioni manuali dei dati;
- una lettura di regime, valore o risposta del prezzo viene dedotta da campi non raccolti.

In caso di sospensione, documentare il campo mancante e aprire un contratto di raccolta dati separato. Non aggiungere inferenze al recorder esistente.

## Fuori Scope

- confronto con DeepCharts e promozione H1;
- equivalenza di `CumulativeTrade` con `Aggregate`;
- classificazione automatica di assorbimento, accettazione, rifiuto o follow-through;
- profili compositi, VWAP, VAH, VAL, HVN, LVN e regime d'asta;
- soglie di volume o filtri adattivi;
- marker, output grafico, segnali, ordini, PnL, stop, target o size;
- modifiche a `FabioOrderFlow/src/FabioOrderFlow.cs` o al recorder.

## Output Canonico Previsto

```text
docs/research/cumulative-trade-footprint-description-2026-08-04.md
```

Il report deve aggiornare `FabioOrderFlow/FabioOrderFlow.md` e aggiungere una riga a `FabioOrderFlow/progress.txt` al momento della decisione.

## Riferimenti

- `fabio_course/fabio1.txt`
- `fabio_course/fabio2.txt`
- `fabio_course/fabio3.txt`
- `fabio_course/fabio-course-model-map.md`
- `docs/research/participation-effort-result-observation-contract.md`
- `docs/research/atas-cumulative-trade-capture-validation-2026-08-04.md`
