# La Footprint E Il Delta Per Prezzo

## Cosa C'E' E Dove

**Il bridge restituisce il delta prezzo per prezzo, dentro ogni singola barra.** Non e' una cosa da
dedurre né da guardare a schermo: e' un campo del payload.

```bash
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 \
    --from 2026-09-16T13:45 --to 2026-09-16T13:46 --levels --out /tmp/fp.json
```

Con `--levels` ogni candela porta un array `levels`, un elemento per **ogni prezzo scambiato**
dentro quella barra:

```json
{"price": 29430.0, "volume": 59, "bid": 5, "ask": 54, "between": 0, "ticks": 42}
```

- **`ask`** = contratti scambiati al prezzo lettera, cioe' **comprati in aggressione**;
- **`bid`** = contratti scambiati al denaro, cioe' **venduti in aggressione**;
- **il delta di quel prezzo e' `ask - bid`**. Il campo `delta` della barra e' la loro somma.

La barra porta anche `maxPositiveDelta` e `maxNegativeDelta` gia' calcolati — il prezzo con il
delta piu' positivo e quello con il piu' negativo — e `poc`, il prezzo piu' scambiato **dentro
quella barra**, che non e' il POC della seduta.

Lo stesso array esiste su `/profile`, ma li' e' aggregato su tutto il periodo: serve a un'altra
domanda. **Per barra si usa `candles --levels`.**

## A Cosa Risponde, Che Il Delta Di Barra Non Sa Dire

Il delta di una barra e' un numero solo: dice **quanto** netto, non **dove**. Due barre con lo
stesso `delta +554` possono essere una spinta concentrata su tre prezzi o un acquisto spalmato su
trenta, e significano cose diverse.

La footprint separa tre cose che a livello di barra si assomigliano:

| | cosa si vede nella footprint |
|---|---|
| **base dei compratori** | il picco di delta positivo **coincide** con il POC della barra: hanno colpito l'ask *e* scambiato di piu' allo stesso prezzo |
| **passaggio** | delta positivo sparso, nessun prezzo domina: il prezzo e' transitato, nessuno ha costruito |
| **assorbimento** | volume alto a un prezzo con delta **vicino a zero**: qualcuno ha preso tutto quello che arrivava, da entrambi i lati |

**La regola che ne esce:** un livello serve a qualcosa quando **picco di delta e POC della barra
cadono insieme**. Se cadono su prezzi diversi, il delta e' un fatto isolato e non una base.

## L'Incidente Del 16 Settembre

L'utente ha chiesto se la candela delle 15:45 avesse punti a delta molto positivo — livelli da cui
un buy sarebbe ripartito. **Ho risposto che il dato non c'era**: che il bridge desse un delta per
barra e non prezzo per prezzo, e che la footprint fosse solo sullo schermo di ATAS.

Era falso, e **stava scritto**: `contratto-data-bridge.md` documenta `levels` su `/candles` come
"footprint opzionale" da quando il bridge esiste. Ho dichiarato assente una capacita' documentata
invece di aprire il contratto e controllare.

Ha insistito — *"pero' dovresti averlo il delta prezzo per prezzo"* — e il dato e' uscito al primo
tentativo:

```text
15:45   O 29.405  H 29.435  L 29.401,50  C 29.433,50   vol 2.854   delta +554
        29.430,00   59 lotti   delta +49    <- picco
        29.426,00  101 lotti   delta +37    <- POC della barra
        29.425,00   53 lotti   delta +39
        29.428,50   36 lotti   delta +24
```

Picco e POC a quattro punti di distanza, circa 150 lotti con delta +149 in cinque punti: una base
vera, e un livello che andava disegnato.

**La lezione non e' sulla footprint, e' sul controllo.** Prima di dire *"quel dato non ce l'ho"* si
apre `contratto-data-bridge.md` e si guarda l'elenco degli endpoint. Dichiarare assente un dato
disponibile e' peggio di non averlo: chiude la domanda invece di rispondere.

## Il Contesto Non Si Salta

Un picco di delta dentro una barra **non basta a fare un livello**. Va confrontato con il profilo
della seduta, e il 16 settembre i due dicevano cose opposte:

```text
29.430 nella barra delle 15:45     delta  +149 su ~150 lotti
29.425-29.450 in tutta la giornata delta  -238 su 19.504 lotti
```

I compratori di quel minuto erano **in minoranza nel loro stesso quartiere**. Resta un livello da
testare, non un supporto dimostrato, e l'etichetta sul chart deve dirlo.

## Correlati

- [`contratto-data-bridge.md`](contratto-data-bridge.md) — gli endpoint e i campi, fonte di verita'
  su cosa il bridge sa dare.
- [`livelli-sul-chart.md`](livelli-sul-chart.md) — come un livello derivato dalla footprint finisce
  sul grafico, e l'obbligo di rifarlo durante la seduta.
