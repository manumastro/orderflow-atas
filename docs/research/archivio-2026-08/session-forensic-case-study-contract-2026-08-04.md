# Case Study Forense: Apertura NQ 2026-08-04

## Stato

```text
Tipo:                 analisi offline su una sola apertura
Dataset:              NQ US Cash, 2026-08-04, fof-session-observation-v2
Modello attivo:       nessuno
Segnali / ordini:     nessuno
Runtime:              invariato
```

## Domanda

Con i dati gia' registrati, cosa possiamo descrivere in modo controllato senza trasformare una sola apertura in un modello?

La domanda pratica e': la risposta dopo i `CumulativeTrade` appare diversa dalla traiettoria temporale neutra della stessa apertura, oppure riflette soprattutto il regime direzionale gia' presente?

## Campione E Metodo Fissati Prima Del Calcolo

Il parser usa solo lo snapshot locale `fof-session-observation-v2` e la tabella evento gia' prodotta dal report di sessione. I record `fof-session-observation-v1` restano esclusi.

1. Gli eventi `CumulativeTrade` sono inclusi solo se `complete=true` e `tickVolumeMatchesTotal=true`.
2. La risposta evento viene misurata a `60`, `120` e `300` secondi dopo `lastTickTimeUtc`, usando il prezzo dell'ultimo raw trade osservato entro l'orizzonte.
3. La baseline temporale usa anchor fissi ogni `60` secondi e ogni `300` secondi da `09:30 America/New_York`; per ogni anchor il prezzo di partenza e' il primo raw trade osservato a o dopo l'orario fissato.
4. La timeline usa bucket fissi di cinque minuti da `09:30`, con raw path, conteggio eventi, location dominante e mediana della risposta evento a `300` secondi.
5. Nessuna soglia di volume, distanza dal POC, direzione o risultato viene aggiunta dopo il calcolo.

## Criteri

Il test e' valido se lo snapshot contiene raw trade v2, se la tabella evento contiene i campi del report di sessione, se gli eventi completi hanno volume tick coerente e se ogni risposta usata ha il proprio orizzonte interamente osservato.

Il test viene scartato se mancano raw trade, se i dati mescolano schema diverso da v2, se non e' possibile ricostruire gli anchor temporali o se la finestra osservata non parte a `09:30 America/New_York`.

Un risultato non puo' promuovere soglie, probabilita', filtri, segnali o modello. Puo' solo produrre fatti descrittivi e ipotesi candidate da verificare altrove.

## Output Canonico

```text
docs/research/session-forensic-case-study-2026-08-04.md
```

Il parser e' `FabioOrderFlow/tools/report_session_forensic_case_study.py`. I CSV/JSON derivati restano locali in `FabioOrderFlow/ledger-snapshots/` e vengono identificati nel report con SHA-256.

## Limiti

- una sola apertura osservata, non una sessione cash completa;
- forte regime direzionale nel periodo registrato;
- eventi molto densi e spesso sovrapposti;
- POC di sessione in sviluppo, non profilo della sessione precedente;
- nessun contesto overnight o valore precedente nel recorder attuale.
