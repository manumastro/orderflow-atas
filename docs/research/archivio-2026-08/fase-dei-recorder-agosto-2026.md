# Archivio: La Fase Dei Recorder, Agosto 2026

Questo materiale documenta il primo approccio del progetto alla raccolta dati: quattro indicatori ATAS che scrivono righe JSON nel log della piattaforma, con lo schema deciso prima della cattura.

E' conservato perche' contiene evidenza reale e le decisioni che ne sono derivate, non perche' sia il percorso da seguire oggi. Per gli studi nuovi si usa il [Data Bridge](../metodo/contratto-data-bridge.md), che permette di chiedere i dati a runtime invece di catturarli secondo uno schema fissato in anticipo.

## Cosa Contiene

| Documento | Cosa stabilisce |
|---|---|
| `cumulative-trade-footprint-description-contract.md` | Contratto: volume, tick e footprint di barra |
| `cumulative-trade-footprint-description-2026-08-04.md` | Descrizione di una sessione live |
| `atas-cumulative-trade-capture-validation-2026-08-04.md` | Validazione tecnica della cattura |
| `session-location-and-price-response-collection-contract.md` | Contratto: POC di sessione e risposta prezzo a 300 secondi |
| `session-location-and-price-response-description-2026-08-04.md` | 57.534 eventi completi, zero mismatch tick-volume |
| `session-forensic-case-study-contract-2026-08-04.md` | Contratto del case study sull'apertura |
| `session-forensic-case-study-2026-08-04.md` | Risposte a 60, 120 e 300 secondi su una singola apertura |
| `historical-cumulative-context-collection-contract.md` | Contratto: candle storiche e `CumulativeTrade` a sette giorni |
| `historical-cumulative-context-inventory-2026-08-04-v5.md` | Inventario canonico: 1.999.263 record restituiti, 1.710.102 dentro finestra |
| `historical-cumulative-cash-session-description-contract.md` | Contratto della descrizione storica della sola cash |
| `historical-cumulative-cash-session-description-2026-08-04.md` | 1.264.397 `CumulativeTrade` cash su sei date |

## Cosa Ne Resta Di Valido

I vincoli ATAS scoperti qui sul campo valgono ancora e sono rispettati dal bridge:

- una sola richiesta `CumulativeTrades` pendente alla volta;
- profondita' massima di sette giorni per richiesta;
- ATAS puo' restituire record fuori dalla finestra richiesta, che vanno contati e scartati.

I parser dei log di questa fase sono in `FabioOrderFlow/tools/archivio-2026-08/`.
