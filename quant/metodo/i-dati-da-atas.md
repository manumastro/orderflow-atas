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

**Gli orari delle sedute sono di New York, convertiti per data.** Le barre sono in UTC; la cassa e'
09:30-16:00 di New York e la giornata di borsa comincia alle 18:00 di New York del giorno prima, e il
framework le converte con l'ora legale di ogni data (`Quant.Comune/Dati/Calendario.cs`, dal primo
giorno, 11 ottobre 2026): la cassa e' 13:30-20:00Z d'estate e 14:30-21:00Z d'inverno. Un orario UTC
fisso sbaglierebbe meta' delle sedute storiche. La regola per tutto il repository:
[`gli-orari-si-scrivono-nel-fuso-del-mercato.md`](../../docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md).

| simbolo | tipo | da | a | barre | origine |
|---|---|---|---|---|---|
| NQ | m1 | 2026-08-03 | 2026-10-09 | 41.142 (50 giornate) | chart M1 di NQZ6, scaricato l'11 ottobre 2026 |
| NQ | scambi, tick | 2023-01-01 | 2026-10-16 | 269,7 e 510,6 milioni (21 GB) | tape del continuo, scaricato dal 18 al 21 ottobre 2026 ([sotto](#il-tape-misurato-il-18-ottobre-2026)) |
| ES | m1 | 2017-04-20 | 2026-10-23 | 3.345.558 | chart ES continuo M1, 3500 giorni, scaricato il 25 ottobre 2026 dalla connessione Rithmic della prop firm |
| ES | scambi, tick | 2023-01-01 | 2026-10-23 | 234,8 e 993,0 milioni (29 GB) | tape del continuo, scaricato dal 25 al 26 ottobre 2026 (`ES/scambi-2023-2026.log`); tre interruzioni (un aggiornamento di Windows, due chiusure di ATAS con HTTP 504), riprese senza buchi perche' un mese si salva solo intero |

**ES, 25 ottobre 2026.** Con la connessione Rithmic della prop firm ATAS da' la stessa storia di
prima: il tape del continuo `ES` dal 2016 (provato sul 1 marzo 2016: 176 mila scambi, 2,0 milioni di
lotti), l'M1 dal 2017. **Il limite e' `MaxCandlesPerChart`** in `%APPDATA%\ATAS X\Platform.cnf`:
a 1.000.000 il chart M1 si ferma a circa due anni (672.674 barre, dal 27 novembre 2024) qualunque sia
il numero di giorni; a 5.000.000 (rimesso dall'utente) con 3500 giorni di calendario arriva al 20
aprile 2017. I giorni del chart sono di calendario. I rollover in `ES/rollover.json`: 16 dal salto,
21 dal carry, 1 misurato (ESU6 -> ESZ6 **+65,25 punti**, mediana dei 391 minuti di cassa del 10
settembre 2026 fra il chart ESZ6 e il continuo). **Senza la misura il carry dal 2022 usciva col segno
sbagliato** (-16 punti a settembre 2026), perche' l'ultimo salto noto e' del marzo 2022, con i tassi a
zero. Anche con la misura, il carry 2023-2025 e' interpolato in linea retta fra -0,5% e +3,4% annuo e
**sottostima lo scarto vero di quegli anni** (tassi al 5%): vale solo per i livelli rettificati.

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
- Ci si aspettavano mesi: **sono anni**, dal 2016 sul chart del continuo (misurato il 18 ottobre
  2026, piu' sotto).
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
- **Nei test la prima seduta dopo un rollover si esclude**: e' l'unica in cui un rendimento dalla
  chiusura del giorno prima mette insieme due contratti. Dal 15 ottobre 2026 lo fa il motore:
  `Serie.DopoRollover` segna quelle giornate, `Backtest` non ci apre posizioni (le conta in
  `EsclusiRollover`, e il rapporto lo scrive), il nullo del quando non ci pesca. La strategia le vede
  comunque, cosi' il contesto resta coerente.

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
3. **Scambi con l'aggressore**: fatto per il 2026 con `quant scambi` (18 ottobre 2026), ogni
   giornata controllata contro le barre; il 2025 e il 2023-2024 il 18-21 ottobre. Resta il
   2016-2022, su richiesta: con i tempi del 2023-2024 (circa 3,5 ore e 5 GB l'anno, meno negli anni
   con meno scambi) sono circa 20-25 ore e 25-35 GB. Il PC non deve andare in standby.
4. **Level 2 storico**: un registratore che gira durante il Market Replay, un giorno per volta.
5. **Da oggi in avanti, per sempre**: un registratore dal vivo di book, scambi con gli id degli
   ordini e MBO, a ogni seduta. E' l'unico modo, con ATAS soltanto, di avere anni di Level 2.

## Il Tape, Misurato Il 18 Ottobre 2026

- **Il tape completo c'e' per anni, non per mesi**, sul chart del **continuo** (`--chart NQ`):
  10 minuti di cassa tornano pieni dal 2016 al 2026. Sul contratto singolo, prima del suo rollover,
  non c'e' quasi niente (NQZ6: 25 scambi in 10 minuti ad agosto, zero a marzo).
- **E' lo stesso dato delle barre**: volume e delta minuto per minuto coincidono con `NQ/m1` nel
  2017, 2019, 2024 e 2026, a meno di 1 lotto al confine fra due minuti.
- **Il difetto di ATAS a mezzanotte**: nei giorni passati, ogni finestra fra l'apertura di Globex
  (22:00 o 23:00Z) e la mezzanotte UTC **che non arriva alla mezzanotte** torna vuota, senza errore.
  `quant scambi` scarica la sera in una finestra sola, 21:00-24:00Z.
- **Il 2026 e' in archivio** (`quant scambi --chart NQ --da 2026-01-01 --a 2026-10-17`, 2 ore e 25
  minuti, da 16 a 120 secondi a giornata): `NQ/scambi` 56,3 milioni di scambi aggregati (2,4 GB, 44
  byte: tempo, lato, volume, primo e ultimo prezzo, bid e ask prima e dopo, numero di tick) e
  `NQ/tick` 103,2 milioni di tick singoli (2,0 GB), nello stesso ordine. Il registro giornata per
  giornata e' `quant/dati/NQ/scambi-2026.log`.
- **Il controllo contro le barre**, 246 giornate fino al 14 ottobre: 159 esatte, 67 entro lo 0,5%,
  20 oltre. Sul totale il tape ha lo 0,03% di volume in piu' delle barre.
  - **Il tape ha dei buchi suoi**, che riprovare non chiude (il 16 giugno: 74 secondi vuoti,
    17:18:55-17:20:10Z, anche a finestre piccole). Oltre l'1% in meno: 20 gennaio -2,2%, 30 gennaio
    -1,4%, 1 giugno **-7,3%**, 12 giugno -1,2%, 24 giugno **-3,4%**, 24 settembre -1,9%, 1 marzo -1,2%
    (una domenica, 234 lotti).
  - **Le barre hanno dei buchi loro**, che il tape riempie: 20 febbraio +7,4% e 19 marzo +8,9% (i
    buchi gia' noti dell'M1), 2 aprile +5,9%, 4 febbraio +2,7%, 5 gennaio +2,4%, e altre cinque
    giornate fra +0,9% e +1,4%.
  - Il 15 e 16 ottobre non hanno il confronto: l'M1 in archivio finisce il 15 alle 06:22Z.
  - **I prezzi**, sui 279.987 minuti con barra e tape: apertura, massimo, minimo e chiusura dai
    tick coincidono con l'M1 nel 99,92-99,95% dei minuti; gli altri differiscono di un tick, sui
    minuti di confine. Dentro i file: nessun record fuori ordine, nessun lato ignoto, ogni scambio
    ha bid e ask, i tick tornano con gli scambi (conteggio, primo e ultimo prezzo); 833 book
    incrociati su 56 milioni; lo 0,4% degli acquisti esegue sotto l'ask di prima e lo 0,1% delle
    vendite sopra il bid (il book si muove nello stesso millisecondo).
  - **In uno studio**, le giornate con il tape oltre l'1% sotto le barre si segnano e si dichiarano,
    o si escludono: sono quelle elencate sopra e, per il 2025, sotto.
- **Il 2025 e' in archivio** (`--da 2025-01-01 --a 2026-01-01`, 2 ore e 25 minuti, registro
  `quant/dati/NQ/scambi-2025.log`): 67,5 milioni di scambi e 128,1 milioni di tick. Contro le barre,
  312 giornate (il 18 aprile, venerdi' santo, e' chiuso): 258 esatte, 47 entro lo 0,5%, sul totale
  +0,01%.
  - Il tape sotto le barre oltre l'1%: **10 febbraio -6,3%** e **29 luglio -4,8%**.
  - Le barre sotto il tape: **22 agosto +8,6%**, 16 luglio +2,0%, 14 ottobre +1,9%, 27 agosto
    +1,3%, 29 ottobre +0,8%.
  - I prezzi, su 352.448 minuti: massimo e minimo uguali nel 99,92%, apertura e chiusura nel 99,4%
    (nel 2026 il 99,9%). Le chiusure diverse (2.133) tornano quasi tutte spostando il confine del
    minuto **in avanti di 100-400 ms**: nel 2025 le barre e il tape hanno orari sfasati di qualche
    decimo di secondo. Pesa solo sugli scambi a ridosso del confine di un minuto, quando si
    appaiano tape e barre.
- **Il 2023 e il 2024 sono in archivio** (`--da 2023-01-01 --a 2025-01-01`, 18-21 ottobre 2026, circa 7
  ore e mezza di lavoro in due riprese, una interruzione e uno standby del PC; registro
  `quant/dati/NQ/scambi-2023-2024.log`): 74,4 milioni di scambi e 142,5 milioni di tick nel 2023,
  71,6 e 136,8 milioni nel 2024. Sono il **dentro campione** di NQ: si possono studiare.
  - Contro le barre: nel 2023 74 giornate esatte e 233 entro lo 0,5% su 310 (sul totale -0,01%); nel
    2024 166 esatte e 142 entro lo 0,5% su 313 (+0,03%). **Nessuna giornata ha il tape oltre l'1%
    sotto le barre.** Fra lo 0,5 e l'1% sotto: 12 giugno 2023, 26 ottobre 2023, 11 giugno 2024. Le barre
    sotto il tape: **28 febbraio 2024 +6,5%**, 18 aprile 2024 +2,3%, 1 dicembre 2023 +2,2%, 1 marzo 2024
    +1,3%, 3 luglio 2024 +0,6%. Chiusi e vuoti: 1 gennaio, 24 e 31 dicembre 2023, 29 marzo 2024.
  - I prezzi: massimo e minimo uguali all'M1 nel 99,98% dei minuti in entrambi gli anni; apertura e
    chiusura nel 99,97% (2023) e nel 99,80% (2024). Le chiusure diverse tornano quasi tutte spostando
    il confine di 100 ms, in avanti o indietro. Nessun record fuori ordine, nessun lato ignoto, i
    tick tornano con gli scambi; 6.369 book incrociati nel 2023 e 29 nel 2024, su oltre 70 milioni.
  - Nel 2023 molte giornate differiscono di pochi lotti (233 entro lo 0,5%): e' il confine fra due
    minuti, come negli altri anni; la causa esatta non e' misurata.
- **Il 2026 e' fuori campione**: averlo scaricato e confrontato con le barre non lo consuma; ogni
  studio sopra si', e passa solo da `valida --fuori`.

## Cosa Si Fa Con I Dati In Piu'

Le barre M1 bastano per le strategie del transcript. Tick e book servono:

- **al motore**: con i tick l'ambiguita' stop/target nella stessa barra si risolve con l'ordine
  vero degli scambi, invece che al peggio;
- **alle idee**: il flusso degli ordini (delta, aggressione, assorbimento, il book) e' il terreno
  del metodo di Fabio, e diventa ipotesi da testare.
