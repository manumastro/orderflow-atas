# Case Study Forense: Apertura NQ 2026-08-04

## Stato

```text
Tipo:                 analisi offline su una sola apertura
Sessione:             NQ US Cash 2026-08-04
Schema incluso:       fof-session-observation-v2
Schema escluso:       fof-session-observation-v1
Modello attivo:       nessuno
Segnali / ordini:     nessuno
```

## Provenienza

- Snapshot v2: `FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2.jsonl.gz`
- SHA-256 snapshot: `e5448962f25401774c119f152c178463715471f1f46c6b678f5e0febd5424153`
- Eventi: `FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-events.csv`
- SHA-256 eventi: `8c145ce67b4061dc62667fa4a1a02a8f0290bb9452296705c696ff25829cda7c`
- Anchor temporali locali: `FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-anchors.csv`
- SHA-256 anchor: `b4f9b6009689b87260ae3e59125b02aa720dae5eabf8c8143fcb11fed9fdce98`
- Timeline locale: `FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-timeline.csv`
- SHA-256 timeline: `f0b261f80667d86c4e5befc9a90281cc31ce047dc584cecf7bcb103441773658`
- Sintesi locale: `FabioOrderFlow/ledger-snapshots/session-forensic-2026-08-04-summary.json`
- SHA-256 sintesi: `e264ad7fcb91b80bd5bba1c85beb17368bd22d7618c4c726af9f2802cee09bf6`
- Sintesi non sovrapposta locale: `FabioOrderFlow/ledger-snapshots/session-location-2026-08-04-v2-non-overlap-summary.json`
- SHA-256 non sovrapposta: `2078280a22cf429e87cf78d60fa5b58a6f57ddcacc1245286e077705a3bd0ad5`

I CSV e JSON locali non sono inclusi in Git. Gli hash sopra identificano gli artefatti usati per questo report.

## Metodo

Il report confronta tre viste della stessa apertura: eventi `CumulativeTrade` completi, anchor temporali neutri e bucket fissi di cinque minuti. La risposta viene misurata a 60, 120 e 300 secondi. Gli anchor temporali non richiedono un evento: partono dal primo raw trade osservato a o dopo un orario fisso.

Questa analisi non stima probabilita', non testa significativita' e non approva una regola. Serve a capire quanto delle risposte osservate sia spiegabile dal regime temporale della singola apertura.

## Copertura

```text
raw trade:                       101879
record v2 snapshot:              286898
primo raw America/New_York:      2026-08-04T09:30:00.000445
ultimo raw America/New_York:     2026-08-04T10:17:00.283881
eventi finali nel CSV:           61590
eventi completi e coerenti:      57534
primo evento completo:           2026-08-04T09:30:00.0004456
ultimo evento completo:          2026-08-04T10:12:00.2827746
prezzo apertura osservato:       29234.75
prezzo ultimo raw osservato:     29521.25
movimento netto osservato tick:  1146
massimo da apertura tick:        1254
minimo da apertura tick:         -106
```

La raccolta non copre la sessione cash intera: l'ultimo raw trade osservato e' alle `10:17:00` New York. Gli eventi dopo circa `10:12` non hanno un percorso futuro completo di 300 secondi e restano fuori dalle statistiche evento complete.

## Baseline Temporale

| anchor | orizzonte | n | p10 | mediana | p90 | + / - / 0 |
| --- | --- | --- | --- | --- | --- | --- |
| ogni 60s | 60s | 47 | -61 | 28 | 114 | 30 / 17 / 0 |
| ogni 60s | 120s | 46 | -95 | 45.5 | 154 | 33 / 13 / 0 |
| ogni 60s | 300s | 43 | -87 | 114 | 242 | 32 / 11 / 0 |
| ogni 300s | 60s | 10 | -149 | 54 | 166 | 7 / 3 / 0 |
| ogni 300s | 120s | 10 | -257 | 78.5 | 140 | 8 / 2 / 0 |
| ogni 300s | 300s | 9 | -139 | 89 | 592 | 8 / 1 / 0 |

Gli anchor a cinque minuti sono il controllo piu' severo contro la sovrapposizione temporale. Nella finestra osservata hanno risposta a 300 secondi positiva in `8` casi su `9`. Gli anchor a un minuto hanno risposta a 300 secondi positiva in `32` casi su `43`. Questo conferma che la baseline dell'apertura era gia' inclinata al rialzo.

## Eventi CumulativeTrade

| orizzonte | n | p10 | mediana | p90 | + / - / 0 |
| --- | --- | --- | --- | --- | --- |
| 60s | 57534 | -72 | 33 | 136 | 39474 / 17740 / 320 |
| 120s | 57534 | -80 | 58 | 193 | 40816 / 16541 / 177 |
| 300s | 57534 | -57 | 124 | 315 | 46778 / 10652 / 104 |

A 300 secondi gli eventi completi sono positivi in `46778` casi su `57534`. Questo dato non e' indipendente: gli eventi sono molto densi e molte finestre future si sovrappongono.

### Per Lato

| lato | n | mediana 60s | mediana 120s | mediana 300s | + / - / 0 a 300s |
| --- | --- | --- | --- | --- | --- |
| Buy | 28800 | 32 | 57 | 123 | 23312 / 5438 / 50 |
| Sell | 28734 | 34 | 59 | 125 | 23466 / 5214 / 54 |

Buy e Sell restano quasi sovrapposti: mediana a 300 secondi `123` per Buy e `125` per Sell. In questa apertura il lato dell'evento non separa una lettura direzionale.

### Per Location POC

| location | n | minuti da open mediana | mediana 60s | mediana 120s | mediana 300s | + / - / 0 a 300s |
| --- | --- | --- | --- | --- | --- | --- |
| above-all-pocs | 44056 | 18.0 | 26 | 47 | 100 | 34449 / 9509 / 98 |
| at-poc | 233 | 6.6 | 72 | 132 | 226 | 224 / 9 / 0 |
| below-all-pocs | 13156 | 8.9 | 61 | 107 | 199 | 12016 / 1134 / 6 |
| between-tied-pocs | 89 | 0.9 | 138 | 205 | 451 | 89 / 0 / 0 |

Le location sono intrecciate col tempo. Gli eventi sotto tutti i POC hanno mediana temporale piu' vicina all'apertura rispetto agli eventi sopra tutti i POC, quindi la differenza di risposta non puo' essere letta come proprieta' autonoma della location.

## Timeline A Cinque Minuti

| fascia NY | raw netto | baseline +300 | eventi tutti/completi | CT mediana +300 | CT + / - / 0 | location dominante | ritorni POC |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 09:30-09:35 | 592 | 592 | 10921 / 10921 | 316 | 10233 / 668 / 20 | above-all-pocs:8439 | 3951 |
| 09:35-09:40 | 42 | 43 | 8904 / 8904 | 151 | 7748 / 1138 / 18 | below-all-pocs:5920 | 6533 |
| 09:40-09:45 | 75 | 75 | 7353 / 7353 | 100 | 6228 / 1116 / 9 | above-all-pocs:5877 | 7311 |
| 09:45-09:50 | 197 | 197 | 6980 / 6980 | 175 | 6980 / 0 / 0 | above-all-pocs:6968 | 0 |
| 09:50-09:55 | 166 | 166 | 5677 / 5677 | -130 | 889 / 4781 / 7 | above-all-pocs:5677 | 3675 |
| 09:55-10:00 | -142 | -139 | 5323 / 5323 | 110 | 3531 / 1786 / 6 | below-all-pocs:2926 | 4537 |
| 10:00-10:05 | 86 | 86 | 5601 / 5601 | 169 | 5601 / 0 / 0 | above-all-pocs:5007 | 1676 |
| 10:05-10:10 | 128 | 128 | 5093 / 5093 | 42 | 4230 / 825 / 38 | above-all-pocs:5093 | 0 |
| 10:10-10:15 | 90 | 89 | 3802 / 1682 | 61 | 1338 / 338 / 6 | above-all-pocs:1682 | 0 |
| 10:15-10:20 | -76 | n/a | 1936 / 0 | n/a | 0 / 0 / 0 | n/a | 0 |

La timeline mostra perche' il confronto grezzo puo' ingannare. Alcune fasce hanno baseline e risposta evento allineate, altre no, ma restano tutte parti dello stesso movimento di apertura. La fascia `10:15-10:20` contiene raw trade osservati ma non eventi completi, perche' manca la finestra futura di 300 secondi.

## Controllo Non Sovrapposto

La selezione non sovrapposta ha lasciato `9` finestre: `7` sopra tutti i POC, `1` sul POC, `1` sotto tutti i POC e `0` tra POC in parita'.

Questo controllo riduce `57.534` eventi completi a poche finestre temporalmente distinte. E' la ragione principale per cui la singola apertura non puo' essere trasformata in probabilita' operative.

## Cosa Si Puo' Dire

1. L'apertura osservata e' stata fortemente direzionale: dal primo raw trade all'ultimo raw trade il prezzo sale di `1146` tick, con massimo a `1254` tick dall'apertura.
2. La risposta positiva dopo molti `CumulativeTrade` e' compatibile con la baseline temporale rialzista. Gli eventi non bastano a isolare una causa.
3. Il lato Buy/Sell non separa il comportamento futuro nella sessione osservata.
4. La location rispetto al POC di sessione in sviluppo descrive dove si trova il prezzo nella distribuzione corrente, ma in questa apertura e' confusa con il momento della giornata e col trend gia' in corso.
5. L'unico uso corretto di questo dataset e' come case study: timeline, anatomia degli eventi, limiti del recorder e ipotesi candidate. Non e' un set di validazione.

## Ipotesi Candidate, Non Regole

- In una apertura direzionale, `CumulativeTrade` puo' descrivere partecipazione nel movimento piu' che segnalare ritorno al POC.
- Gli eventi sotto o sul POC durante questa apertura sembrano spesso coincidere con fasi precoci o pullback dentro una spinta rialzista; questa e' una lettura di contesto, non una regola di acquisto.
- Per studiare assorbimento o rifiuto servirebbe un recorder del contesto precedente: POC/VAH/VAL della sessione passata, overnight range, apertura rispetto al valore e sviluppo dell'initial balance.

## Conclusione

Con i dati gia' registrati possiamo documentare bene una apertura e impedire letture premature. Non possiamo validare un modello. Il risultato pratico e' un frame di lavoro: prima baseline temporale e contesto d'asta, poi eventi di partecipazione; mai il contrario.
