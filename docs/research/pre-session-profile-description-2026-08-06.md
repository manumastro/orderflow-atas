# Descrizione Pre-Sessione NQ - 2026-08-06

## Stato

```text
Tipo:                 case study descrittivo di una sola apertura
Strumento:            NQU6@CME / future Mini Nasdaq-100
Fonte candle:         fof-historical-cumulative-context-v5
Timeframe:            1 minuto con footprint
Modello attivo:       nessuno
Segnali / ordini:     nessuno
```

Questo documento applica in modo ripetibile il tratto `46:54-56:43` della prima lezione alla sessione del 6 agosto. Descrive il profilo e il comportamento successivo; non trasforma quanto osservato in una regola long o short.

## Fonte E Limiti

| Artefatto | Path | SHA-256 |
| --- | --- | --- |
| sintesi locale | `FabioOrderFlow/ledger-snapshots/pre-session-profile-2026-08-06-summary.json` | `f35a06190e249a2b1337ba85d5a9f7c97ce01c0b169d0071ca4bb306ad58ab43` |
| log ATAS letto | `%APPDATA%/ATAS/Logs/app_20260806.log` | `8db83b276822ee7dd356c5b149cee1b965ff0ec2caa5703f2030d11f1329abaa` |

Al momento della lettura il log ATAS misurava `37.276.195` byte. La sintesi locale e' ignorata da Git, come gli altri artefatti di ricerca; va conservata insieme a questo report fino a quando non e' sostituita.

La sorgente contiene candle storiche con footprint, non raw trade tick-by-tick. Le candle sono state lette come UTC, convenzione gia' verificata sul feed NQU6, e convertite in `America/New_York`. L'area di valore e' ricostruita a partire dai livelli footprint, con una regola deterministica al 70%; puo' differire da una scelta di pareggio interna al Fixed Profile di ATAS. I livelli vanno quindi confrontati visivamente in ATAS, non usati come equivalenza di piattaforma gia' dimostrata.

## Cosa Dice La Lezione

Fabio prima marca un massimo e un minimo chiari formati "during the night" e chiama l'intervallo tra essi **business range** (`47:18-47:49`). Poi disegna il profilo della pre-sessione (`47:59`). Dice che si puo' semplificare usando London (`48:07`), ma nel video 2 (`01:15:15-01:18:02`) avverte che selezionare solo un tratto temporale puo' togliere parti rilevanti del dealing range.

Quindi in questo case study:

- il business range e' il massimo/minimo della finestra primaria;
- POC, VAH e VAL appartengono al volume profile della stessa finestra;
- il profilo London e' un confronto secondario, non il sostituto del profilo overnight;
- il delta massimo positivo/negativo e' un fatto grezzo per prezzo, non una "protezione" gia' confermata o un segnale.

## Orari Da Disegnare

La convenzione congelata nel contratto e':

| Elemento | New York | Italia il 6 agosto | UTC |
| --- | --- | --- | --- |
| inizio pre-sessione primaria | 5 agosto, 18:00 ET | 6 agosto, 00:00 CEST | 5 agosto, 22:00 |
| fine pre-sessione primaria | 6 agosto, 09:30 ET esclusa | 6 agosto, 15:30 CEST esclusa | 6 agosto, 13:30 |
| confronto London | 03:00-09:30 ET | 09:00-15:30 CEST | 07:00-13:30 |
| apertura cash New York | 09:30 ET | 15:30 CEST | 13:30 |

Il chart aveva tutte le `930` candle da un minuto fra `18:00` ET e `09:29` ET: entrambe le frontiere sono presenti. Nel fuso italiano del 6 agosto il primo bar e' quello delle `00:00`; l'ultimo incluso e' `15:29`. La candle che inizia alle `15:30` e' gia' cash session e non deve entrare nel Fixed Profile della pre-sessione.

## Livelli Pre-Sessione Primaria

Finestra: `2026-08-05 18:00 ET` inclusa fino a `2026-08-06 09:30 ET` esclusa.

| Livello | Prezzo | Evidenza |
| --- | ---:| --- |
| business high | `29.679,50` | 18:47 ET, 00:47 CEST |
| business low | `29.326,25` | 09:25 ET, 15:25 CEST |
| midpoint del range | `29.502,875` | ampiezza `1.413` tick |
| POC | `29.500,00` | volume footprint `430` |
| VAL | `29.437,00` | valore ricostruito al 70% |
| VAH | `29.598,50` | valore ricostruito al 70% |
| massimo delta positivo grezzo | `29.475,75` | ask `181`, bid `80`, delta `+101` |
| massimo delta negativo grezzo | `29.350,00` | ask `46`, bid `201`, delta `-155` |

Volume footprint totale: `131.030`; bid `64.843`; ask `66.187`; delta netto `+1.344`. Il POC non e' in parita'.

Il business range e' quindi l'area compresa fra `29.326,25` e `29.679,50`. Non coincide con la value area: la value area e' il sottoinsieme `29.437,00-29.598,50`, mentre il POC e' `29.500,00`.

### Perche' Il Prezzo Puo' Andare Sotto Un Livello Disegnato A Mezzanotte

Alle `00:00` italiane non esiste ancora il business low finale: esiste solo il minimo **provvisorio** raggiunto fino a quel momento. Se il prezzo scende alle 02:00, alle 09:00 o alle 15:25, quel nuovo minimo resta parte della stessa pre-sessione e sostituisce il bordo basso provvisorio. Non si chiude, non si riapre e non si sposta l'inizio del Fixed Profile.

Il 6 agosto il business high si e' formato presto, alle `00:47`, mentre il business low finale si e' formato alle `15:25`, cinque minuti prima dell'apertura. Quindi una discesa precedente alle `15:30` non era una rottura del business low finale: stava ancora costruendo il range. La prima rottura valutabile del range gia' congelato avviene solo dalla candle delle `15:30` in avanti, che infatti non entra nel profilo pre-sessione.

Per lo studio manuale: durante l'overnight aggiorna una sola linea high e una sola linea low ogni volta che compare un nuovo estremo; alle `15:30` smetti di aggiornarle e le estendi a destra. L'overlay del recorder ora automatizza esattamente questa sequenza.

## Confronto London

Finestra: `2026-08-06 03:00-09:30 ET`, cioe' `09:00-15:30 CEST`, con `390` candle complete.

| Livello | Prezzo |
| --- | ---:|
| London high | `29.563,00` |
| London low | `29.326,25` |
| London POC | `29.400,00` |
| London VAL | `29.387,75` |
| London VAH | `29.516,25` |

Qui London cattura il minimo vicino all'apertura, ma perde il massimo overnight `29.679,50` delle 18:47 ET. E' esattamente il tipo di differenza per cui il profilo London va studiato accanto al range completo, non scambiato automaticamente per lo stesso contesto.

## Come Disegnarlo In ATAS

Sul chart NQ a un minuto con Volume Profile / Fixed Profile:

1. Imposta il riferimento orario in New York oppure usa le conversioni italiane della tabella.
2. Traccia una linea orizzontale a `29.679,50` e una a `29.326,25`; estendile verso destra. Sono i due bordi del business range.
3. Disegna un Fixed Profile dall'inizio della candle `18:00 ET` del 5 agosto fino alla fine della candle `09:29 ET` del 6 agosto. Sul chart italiano e' da `00:00` fino a `15:29:59` del 6 agosto.
4. Dal profilo segnala POC `29.500,00`, VAL `29.437,00` e VAH `29.598,50`. Sono livelli diversi dai due bordi del range e vanno mantenuti distinti graficamente.
5. Solo per confronto, aggiungi un secondo Fixed Profile `03:00-09:30 ET` (`09:00-15:30 CEST`) con un colore diverso. Non sostituire il primo con questo secondo profilo.

Per le prossime sessioni, il nuovo recorder congela la stessa convenzione: `18:00 ET` del giorno precedente fino a `09:30 ET` esclusa. Il cambio d'ora e' calcolato sul fuso, quindi non occorre fissare sempre a mano l'equivalente italiano. L'overlay mostra il range in sviluppo prima dell'apertura e i livelli finali da `09:30 ET` in poi; per la forma completa del volume profile resta utile il Fixed Profile nativo di ATAS.

## Apertura Delle 15:30 Italiane

La prima candle cash apre a `29.335,00`, cioe' `35` tick sopra il business low ma `408` tick sotto VAL e `660` tick sotto POC.

| Finestra cash | OHLC | Volume / delta | Relazione con il profilo |
| --- | --- | --- | --- |
| primo minuto, 15:30 | O `29.335,00`; H `29.373,75`; L `29.321,50`; C `29.328,50` | `4.460` / `-38` | minimo di `19` tick sotto il business low, chiusura di nuovo dentro il range |
| primi 5 minuti | O `29.335,00`; H `29.373,75`; L `29.241,25`; C `29.340,00` | `19.490` / `-382` | rottura del business low, chiusura nel range ma ancora sotto VAL |
| primi 15 minuti | O `29.335,00`; H `29.474,25`; L `29.241,25`; C `29.458,75` | `49.061` / `+1.471` | chiusura nel business range e dentro la value area |

I fatti osservati non autorizzano a dire che il profilo dava un segnale short o long. Alle 15:30 il prezzo ha prima oltrepassato il business low; il solo attraversamento non ha prodotto accettazione stabile sotto il range nel campione di cinque minuti. Entro quindici minuti il prezzo era rientrato anche nella value area. Questa e' una descrizione a posteriori, non una regola di reversal.

Il punto di studio corretto per il prossimo giorno e' quindi: quando il prezzo raggiunge un bordo, un livello valore o un estremo delta, osservare se il mercato resta fuori o viene riassorbito. La lezione richiede poi lettura dell'interazione e conferma; il Fixed Profile da solo non definisce entry, stop o direzione.

## Seguito

Il recorder **Fabio Pre-Session Profile Recorder** implementa la stessa finestra e scrive `FofPreSession` nel log ATAS. Il suo schema e' documentato in `docs/research/pre-session-profile-collection-contract.md`. La prima raccolta runtime va verificata dopo il caricamento dell'indicatore sul chart a un minuto; solo allora il case study storico e l'output live possono essere confrontati campo per campo.
