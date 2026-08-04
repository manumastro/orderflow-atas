# Descrizione Cumulative Trade E Footprint - 2026-08-04

## Decisione

```text
Esito:                 descrizione riproducibile completata
Campione:              una sola sessione ATAS
Modello attivo:        nessuno
Soglie approvate:      nessuna
Segnali / ordini / PnL: nessuno
H1 Aggregate:          sospesa; nessun riferimento DeepCharts disponibile
```

Il report completa il contratto `cumulative-trade-footprint-description-contract.md`. Descrive campi ATAS gia' raccolti; non attribuisce significato direzionale agli eventi e non definisce assorbimento, accettazione, rifiuto o follow-through.

## Domanda E Perimetro

Domanda: nella sessione live NQU6@CME del 2026-08-04, come co-occorrono volume finale, tick costituenti, lato aggressivo e posizione del prezzo finale rispetto al POC della barra?

Fonte live congelata:

```text
Log ATAS:          %APPDATA%/ATAS/Logs/app_20260804.log
Finestra log:      2026-08-04 12:02:24 fino a prima di 2026-08-04 13:12:34
Finestra eventi:   2026-08-04T10:01:33.9753093 - 2026-08-04T11:09:13.6600612
Strumento:         NQU6@CME, E-Mini Nasdaq-100
Eventi finali:     5,050
Record live:       12,353
```

Il timestamp nell'header del log ATAS e il timestamp dell'evento sono due clock distinti. Il payload non dichiara un fuso orario esplicito; il fuso e' quindi assente e non inferito.

Lo storico ATAS di 50.688 snapshot validato nel report tecnico non e' incluso nei risultati qui sotto. Questo documento descrive esclusivamente gli stati finali della cattura live.

## Metodo Congelato

Per ogni `EventId` e' stato mantenuto l'ultimo record `live-new` o `live-update` della finestra. I percentili p90 e p99 usano il nearest-rank `ceil(p * n)`; la mediana e' il valore centrale o la media dei due valori centrali. Le liste dei primi dieci sono ordinate in modo decrescente sulla metrica e, a parita', per `EventId` crescente.

```text
Volume finale = TotalVolume dell'ultimo record
Tick finali = numero di Ticks dell'ultimo record
Volume tick = somma di Ticks.Volume dell'ultimo record
Distanza POC = (LastPrice - Footprint.Poc.Price) / Security.TickSize
```

La distanza POC e' espressa in tick con segno. `Negativa`, `zero` e `positiva` indicano rispettivamente che il prezzo finale e' sotto, al POC o sopra il POC della stessa barra. Non indicano premio/sconto, valore, regime o previsione.

## Integrita' Del Campione

```text
Eventi finali ricostruiti:                 5,050
Volume finale totale:                      9,849
Somma volume incrementale:                 9,849
Eventi con mismatch volume tick/finale:        0
Eventi con POC o tick size mancanti:            0
Distanze POC non integrali in tick:             0
POC footprint disponibile:             5,050 / 5,050
Livello footprint primo prezzo:        5,045 / 5,050
Livello footprint ultimo prezzo:       4,746 / 5,050
```

I cinque livelli del primo prezzo e i 304 livelli dell'ultimo prezzo non disponibili restano `null`; non vengono trasformati in zero o esclusi dal campione.

## Riepilogo Complessivo

| Metrica | n | Min | Mediana | p90 | p99 | Max |
|---|---:|---:|---:|---:|---:|---:|
| Volume finale | 5,050 | 1 | 1 | 3 | 14 | 108 |
| Tick finali | 5,050 | 1 | 1 | 3 | 10 | 48 |
| Volume tick | 5,050 | 1 | 1 | 3 | 14 | 108 |
| Delta POC | 5,050 | -34 | 1 | 16 | 69 | 94 |
| Distanza finale dal POC, tick | 5,050 | -55 | 0 | 20 | 50 | 66 |

Il volume tick ricompone esattamente il volume finale per ogni evento. La concentrazione della distribuzione su volume e tick pari a uno e' un fatto di questa sessione; non e' un filtro proposto.

## Riepilogo Per Lato

| Lato | Eventi | Volume mediana / p99 / max | Tick mediana / p99 / max | Delta POC mediana / p99 / max | Distanza POC mediana / p99 / max |
|---|---:|---|---|---|---|
| Buy | 2,584 | 1 / 16 / 108 | 1 / 10 / 48 | 2 / 69 / 94 | 1 / 53 / 66 |
| Sell | 2,466 | 1 / 12 / 37 | 1 / 10 / 36 | 0 / 69 / 94 | -2 / 45 / 61 |

Le differenze tra i due lati sono descrittive e non sono state sottoposte a un test predittivo o causale.

## Lato E Posizione Finale Rispetto Al POC

| Lato | Sotto POC | Al POC | Sopra POC |
|---|---:|---:|---:|
| Buy | 889 | 320 | 1,375 |
| Sell | 1,369 | 273 | 824 |

Questa tabella conta solo la co-occorrenza nello stato finale dell'evento e della barra che lo contiene. Non misura il percorso successivo del prezzo.

## Dieci Eventi Per Volume Finale

| EventId | Time | Lato | Volume | Tick | First -> Last | POC delta | Distanza POC |
|---:|---|---|---:|---:|---|---:|---:|
| 838 | 10:17:44.2211033 | Buy | 108 | 13 | 29094.25 -> 29096.00 | 94 | 7 |
| 2442 | 10:44:26.8452050 | Buy | 49 | 48 | 29099.00 -> 29107.25 | 7 | 41 |
| 728 | 10:16:45.0256351 | Buy | 46 | 12 | 29094.25 -> 29094.25 | 42 | 0 |
| 727 | 10:16:45.0249308 | Sell | 37 | 36 | 29098.25 -> 29094.25 | -7 | -6 |
| 2373 | 10:43:16.9685957 | Buy | 36 | 36 | 29088.25 -> 29093.25 | 0 | 28 |
| 3926 | 10:53:23.9871941 | Buy | 35 | 12 | 29125.50 -> 29125.50 | 33 | 0 |
| 2440 | 10:44:26.8390644 | Buy | 33 | 28 | 29094.50 -> 29097.00 | 5 | 4 |
| 4417 | 11:00:55.9770490 | Sell | 29 | 1 | 29131.25 -> 29131.25 | -33 | 0 |
| 3654 | 10:49:55.7190640 | Buy | 27 | 27 | 29141.00 -> 29144.00 | -4 | 53 |
| 2549 | 10:44:30.3572637 | Buy | 26 | 24 | 29115.25 -> 29120.50 | 11 | 25 |

## Dieci Eventi Per Numero Di Tick

| EventId | Time | Lato | Volume | Tick | First -> Last | POC delta | Distanza POC |
|---:|---|---|---:|---:|---|---:|---:|
| 2442 | 10:44:26.8452050 | Buy | 49 | 48 | 29099.00 -> 29107.25 | 7 | 41 |
| 727 | 10:16:45.0249308 | Sell | 37 | 36 | 29098.25 -> 29094.25 | -7 | -6 |
| 2373 | 10:43:16.9685957 | Buy | 36 | 36 | 29088.25 -> 29093.25 | 0 | 28 |
| 2440 | 10:44:26.8390644 | Buy | 33 | 28 | 29094.50 -> 29097.00 | 5 | 4 |
| 3654 | 10:49:55.7190640 | Buy | 27 | 27 | 29141.00 -> 29144.00 | -4 | 53 |
| 333 | 10:09:45.6051454 | Sell | 25 | 25 | 29092.50 -> 29090.50 | -3 | -22 |
| 1894 | 10:33:51.7869532 | Buy | 25 | 25 | 29098.00 -> 29102.50 | -5 | 3 |
| 2073 | 10:36:49.7978425 | Sell | 25 | 25 | 29082.25 -> 29080.25 | -16 | -20 |
| 2809 | 10:45:06.5663170 | Sell | 25 | 25 | 29120.25 -> 29116.00 | 13 | -15 |
| 3669 | 10:49:56.2888896 | Sell | 26 | 25 | 29138.25 -> 29135.00 | 5 | -29 |

Le tabelle preservano i campi grezzi richiesti dal contratto. Non classificano i dieci eventi come rilevanti, assorbiti o operabili.

## Limiti E Decisione

Il dataset non contiene location rispetto a profilo composito, VWAP, VAH/VAL, HVN/LVN o regime d'asta; non contiene neppure il risultato del prezzo dopo l'evento. Per questi motivi non puo' rispondere a domande su contesto, valore, assorbimento, accettazione, rifiuto, follow-through o playbook.

La descrizione e' riproducibile e completa i criteri del contratto. Il suo solo esito e' confermare che il dataset live consente di misurare in modo non ambiguo la co-occorrenza tra evento aggregato e footprint di barra nella sessione osservata. Nessuna soglia, segnale, modello o modifica runtime e' approvata.

## Prossima Domanda Ammessa

Un eventuale passo successivo deve aprire un nuovo contratto per raccogliere location di profilo e risposta futura del prezzo. Deve dichiarare prima della raccolta come definire sessione, valore, livello di riferimento e orizzonte di osservazione; non puo' derivarli dalla tabella corrente.
