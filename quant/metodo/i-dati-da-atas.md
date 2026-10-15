# I Dati Da ATAS

ATAS X e' **l'unica fonte di dati** del framework. Questo documento dice cosa ATAS puo' dare, fin
dove, cosa e' gia' collegato e cosa manca. La ricognizione e' dell'11 ottobre 2026 (una domenica:
il connettore Rithmic rifiutava il login, quindi **nessun limite e' ancora stato misurato dal
vivo**). Fonti: le assembly di ATAS X in `C:\ATASX` decompilate, la documentazione locale in
`docs/atas/`, i log e la cache di ATAS, il sito di ATAS. Le voci segnate *[da misurare]* dipendono
dall'abbonamento e si misurano con una chiamata.

## Come Arrivano

Il framework non parla con ATAS direttamente: legge dal **Fabio Data Bridge**, l'indicatore che
gira dentro ATAS ed espone i dati su `http://127.0.0.1:8787`
([`contratto-data-bridge.md`](../../docs/research/metodo/contratto-data-bridge.md)). `PonteAtas.cs`
e' il client; `quant scarica` scrive in archivio.

L'archivio e' `quant/dati/<SIMBOLO>/<tipo>/<AAAA-MM>.bin`: un file per mese, intestazione di 24
byte e poi i record come stanno in memoria (`Barra` 44 byte, `Scambio` 20 byte). Leggere un anno di
M1 e' una copia, non un parsing. Riscaricare un periodo corregge, non duplica. I prezzi sono interi
in tick. `quant/dati/` non va in git.

## Cosa C'e' In Archivio

| simbolo | tipo | da | a | barre | origine |
|---|---|---|---|---|---|
| NQ | m1 | 2026-08-03 | 2026-10-09 | 41.142 (50 giornate) | chart M1 di NQZ6, scaricato l'11 ottobre 2026 |

Un solo contratto (NQZ6): nessun rollover nel periodo.

**Attenzione: 28 giornate su 50 non sono il mercato vero.** Misurato l'11 ottobre 2026: fino al 9
settembre NQZ6 non era il contratto principale (lo era NQU6, che scadeva a settembre). Poco
scambiato, ha minuti senza scambi e quindi senza barra: dal 3 agosto al 9 settembre le giornate
hanno da 197 a 722 barre invece di 1.380, e la seduta da 73 a 322 barre invece di 390. **Le sedute
complete sono 22, dal 10 settembre al 9 ottobre 2026.** Per la storia lunga si scarica il contratto
principale di ogni periodo, e si cuce con i rollover (il piano, punto 2). Le barre mancanti di un
contratto poco scambiato non sono un errore del bridge: sono minuti senza scambi.

## Cosa ATAS Puo' Dare

### Le barre e il footprint

- Ogni candela di un chart porta il **footprint completo**: volume, bid, ask e tick per prezzo.
  Il bridge lo espone con `/candles?levels=1`. Oggi `quant scarica` prende solo OHLC e volumi.
- **Quanto indietro va un chart** lo decide la voce di tariffario `ChartHistoryDepth`
  *[da misurare]*. La cache locale ha candele giornaliere di NQ da dicembre 2025.
- ATAS X ha un'API che **i documenti locali non citano**: `CandlesDataProvider` (`ICandlesDataProvider`):
  - `GetCandlesAsync(CandleSeriesRequest)`, con `TimeFrame`, `Instrument`, `DaysToLoad` e `TradingSessionId`;
  - `LoadMoreHistoryAsync(int days)`.

  **`DaysToLoad` e' limitato a 1-30.** Permetterebbe di chiedere barre di qualunque contratto da
  codice, senza aprire un chart. **Non e' ancora nel bridge.**

### Gli scambi, uno per uno

- `RequestForCumulativeTrades(CumulativeTradesRequest)` restituisce trade aggregati con:
  - `Time`;
  - **`Direction`, il lato dell'aggressore gia' classificato da ATAS**;
  - `Volume`, primo e ultimo prezzo;
  - bid e ask prima e dopo il trade;
  - la lista dei singoli tick (`Ticks`).

  Il bridge lo espone con `/cumulative` (`ticks=1` per i singoli tick).
- I limiti sono quattro:
  - una richiesta pendente alla volta;
  - **al massimo 7 giorni per richiesta**;
  - quanto indietro si va (`CustomModeMaxDepth` in modalita' `Filter`) lo decide il tariffario,
    *[da misurare]* con `/limits`, e la finestra viene tagliata **in silenzio**;
  - **in caso di errore del server ATAS restituisce una lista vuota**: "niente scambi" e "errore"
    non si distinguono.
- Ci si aspettano **mesi, non anni** *[da misurare]*.
- Non c'e' un accesso storico ai tick grezzi per gli indicatori: esiste dentro ATAS, ma lo usa
  solo il Market Replay.

### Il book (Level 2) e gli ordini (MBO)

- **Il book dal vivo**: `MarketDepthChanged`, `MarketDepthsChanged` e `GetMarketDepthSnapshot()`
  (solo il book corrente). **Gli ordini dal vivo** (MBO): `SubscribeMarketByOrderData()` e
  `OnMarketByOrdersChanged`, piu' cache a finestra mobile.
- **Istantanee storiche del book**: `GetMarketDepthSnapshotsAsync` restituisce lo stato del book a
  fine di ogni periodo, cio' che usa la heatmap DOM Levels. Per i future USA serve la voce
  `UsFuturesMarketDepthSnapshots` oppure **una connessione dxFeed**; altrimenti torna vuota. Con
  Rithmic probabilmente non funziona *[da misurare]*: il bridge la espone come `/depth`.
- **Il book storico completo** esiste sui server di ATAS, ma lo usa solo il Market Replay.
- **L'MBO storico non esiste**: si puo' solo registrare dal vivo.

### Il Market Replay

- Rigioca **scambi e book completo** dai server di ATAS, e gli indicatori girano anche in replay:
  **un indicatore registratore puo' raccogliere il Level 2 di un giorno passato**.
- I limiti dipendono dall'abbonamento *[da misurare]*. Le pagine di ATAS, forse precedenti ad
  ATAS X, dicono:
  - circa 3 mesi di storia con il piano a pagamento;
  - "Ticks + DOM: 1 giorno" per sessione di replay.
- Un giorno di book NQ in cache pesa 270-440 MB, piu' 6-9 MB di scambi. Il formato e' proprietario.

### Esportazione

Nessuna esportazione in blocco di tick o book: **il bridge e' la via d'uscita**.

## Misurato Il 14 Ottobre 2026

- **Le barre per codice** (`/serie`, sopra `CandlesDataProvider.SubscribeCandles`) arrivano a
  **circa 30 giorni di borsa** e non oltre: `LoadMoreHistoryAsync` non aggiunge niente, e nemmeno
  chiedere 400 giorni in una volta. Vale a 1 e a 60 minuti, su ES continuo (dal 2 settembre). La
  storia piu' lunga passa solo dai giorni che **il chart** carica (l'M1 ne ha 90): quanto in la'
  arriva un chart e' la prossima misura, cambiando i giorni del chart.
- **Le opzioni** (`/opzioni`, sopra `OptionsDataProvider`): la connessione Rithmic da' le opzioni CME
  su ESZ6, 33 serie (giornaliere, fine mese, mensili), con open interest, volume, ultimo,
  settlement e, quando la sottoscrizione e' calda, bid e ask. **Nessuna storia**: si registra con
  `quant opzioni --chart <ESZ6> --simbolo ES` in `quant/dati/ES/opzioni/<giorno>/`, il dato grezzo.
- **La storia lunga c'e', dal chart del continuo** (15 ottobre 2026). Alzando `MaxCandlesPerChart`
  nelle impostazioni di ATAS, un chart **NQ continuo a 1 minuto** carica **3.343.038 barre, dal 12
  aprile 2017**: in archivio `NQ/m1`, 140 MB, **2.365 sedute di cassa complete** (circa 250 l'anno).
  Delle 86 incomplete, quasi tutte sono festivi e chiusure anticipate (210 barre: chiusura alle 13:00);
  cinque hanno qualche buco nei dati (2019-08-05, 2020-07-21, 2020-12-24, 2026-02-20, 2026-03-19).
  Prima, con un chart a 5 minuti, erano arrivati 17 mesi (`NQ/m5`, dal 25 maggio 2025).
- **Il continuo di ATAS** passa al contratto principale alle date di `/rollovers` (a volume, 40 dal
  2017), **senza rettificare il prezzo**. Un contratto singolo invece, prima del suo rollover, e'
  poco scambiato: l'M1 di NQZ6 dal 4 marzo aveva un terzo delle barre attese. Per questo l'M1 di
  NQZ6 e ESZ6 scaricato prima e' stato tagliato al rollover dell'11 settembre 2026 (`quant taglia`).
- **I rollover**: `quant rollover --chart <continuo> --simbolo NQ --misurato NQU6=294.5` prende le
  date da ATAS e scrive `NQ/rollover.json`, con lo scarto di ogni cambio e il metodo
  (`ScartiDiRollover.cs`, con il suo test):
  - **salto** (15): se fra l'ultima barra del contratto vecchio e la prima del nuovo c'e' solo l'ora
    di pausa di Globex (i rollover di giovedi' fino al 2019, alcuni lunedi' dal 2024), il salto e'
    lo scarto. Il tasso implicito torna con i tassi meno i dividendi: 0,5-1,8% nel 2017-2019, 0,1% a
    fine 2021, 4-5,5% nel 2024-2025;
  - **carry** (22): se in mezzo c'e' il fine settimana il salto e' inutilizzabile (giugno 2020 -118
    punti, giugno 2026 +513, settembre 2026 -10,75). Lo scarto e' prezzo x tasso x giorni fino al
    rollover dopo, con il tasso interpolato linearmente fra quelli dei rollover noti;
  - **misurato** (1): NQU6 -> NQZ6, 11 settembre 2026, +294,5 punti di mediana sui minuti in cui i
    due contratti stampano insieme (il carry avrebbe dato +294,25).
- **L'errore dei carry**: nel 2020-2023 i tassi si sono mossi in fretta e l'interpolazione e'
  lineare. Nel 2020-2021 lo scarto stimato e' probabilmente sopra il vero di 20-25 punti per
  rollover, nel 2022-2023 sotto fino a circa 60. **Pesa solo sui livelli rettificati del passato**
  (le distanze fra prezzi di contratti diversi); i rendimenti dentro una giornata non ne dipendono.
  Per toglierlo, si misura ogni rollover sui minuti appaiati, se ATAS carica i contratti scaduti.
- **Nei test intraday la prima seduta dopo un rollover si esclude**: e' l'unica in cui un rendimento
  dalla chiusura del giorno prima mette insieme due contratti. **Da fare nel motore.**

## Il Piano

1. **Misurare i diritti, appena la connessione torna** (lunedi'), con quattro misure:
   - `/limits` sul chart NQ;
   - `/depth` su un'ora di cassa recente;
   - un `/cumulative` di un giorno di 60 giorni fa;
   - la finestra che il dialogo del Replay lascia scegliere.

   Da questi numeri dipende tutto il resto.
2. **Barre lunghe**: un endpoint `/serie` nel bridge sopra `CandlesDataProvider`, a blocchi di
   30 giorni, contratto per contratto, con i rollover da `/rollovers`. Poi `quant scarica` lo usa. **Sempre il contratto
   principale** del periodo: un contratto prima del suo rollover e' poco scambiato e ha buchi.
3. **Scambi con l'aggressore**: andare indietro un giorno alla volta con `/cumulative`, finche'
   torna vuoto in un giorno feriale, ed e' la profondita' vera. Ogni giorno si controlla contro le
   barre: volume e delta devono coincidere. Prima di raccogliere in grande, il bridge deve
   distinguere "vuoto" da "errore".
4. **Level 2 storico**: un registratore che gira durante il Market Replay, un giorno per volta.
5. **Da oggi in avanti, per sempre**: un registratore dal vivo di book, scambi con gli id degli
   ordini e MBO, a ogni seduta. E' l'unico modo, con ATAS soltanto, di avere anni di Level 2.

## Cosa Si Fa Con I Dati In Piu'

Le barre M1 bastano per le strategie del transcript. Tick e book servono:

- **al motore**: con i tick l'ambiguita' stop/target nella stessa barra si risolve con l'ordine
  vero degli scambi, invece che al peggio;
- **alle idee**: il flusso degli ordini (delta, aggressione, assorbimento, il book) e' il terreno
  del metodo di Fabio, e diventa ipotesi da testare.
