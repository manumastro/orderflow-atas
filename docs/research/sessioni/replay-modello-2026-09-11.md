# Replay Del Modello 40R Sulla Cash Del 2026-09-11

Domanda: leggendo i dati in tempo reale su barre 40 Range, dove sarebbe entrato il [modello di riferimento](../modelli/modello-40r-riferimento.md)?

Metodo: [`replay_model.py`](../../../FabioOrderFlow/tools/replay_model.py) applica il modello alla lettera, decidendo su ogni barra **solo con cio' che era noto alla sua chiusura**. Nessuna condizione usa dati successivi. Le convenzioni che rendono il modello eseguibile sono dichiarate nel documento del modello e passate esplicitamente da riga di comando.

Dati: NQU6@CME, 681 barre 40R fra le 13:30 e le 20:00 UTC, 86 trade aggregati da almeno 50 lotti.

## La Risposta

**Il modello non sarebbe entrato.** Con le convenzioni dichiarate, zero ingressi in tutta la sessione.

L'imbuto dice dove si ferma:

```text
livello      523 barre   a un livello ereditato o in sviluppo
flip          26 barre   con inversione del delta di almeno 80
shift          8 barre   con entrambi i bordi della value area spostati
conferma       0 barre   con Big Trade sul lato e tape in accelerazione
```

Il filtro che azzera tutto e' la **conferma**, non la lettura del livello. E non e' un effetto di soglia: abbassando il flip da 120 a 20 il numero di candidati sale da 3 a 15, ma la conferma resta a zero.

## Gli Otto Candidati

| Barra | UTC | Roma | Lato | Livello | Delta | VA | Big sul lato | Tape |
|---|---|---|---|---|---|---|---|---|
| 148 | 13:44:34 | 15:44 | long | VAH sviluppo 29.429 | -17 → +104 | 29.415-29.421 | 0 | 1,3x |
| 388 | 14:39:23 | 16:39 | long | VAL sviluppo 29.372 | -27 → +158 | 29.378-29.383 | **1** | 1,3x |
| 414 | 14:47:40 | 16:47 | short | VAH notte 29.355 | +53 → -177 | 29.344-29.351 | **1** | 0,9x |
| 477 | 15:03:59 | 17:03 | long | POC sviluppo 29.400 | -10 → +160 | 29.384-29.391 | **1** | 1,1x |
| 502 | 15:16:58 | 17:16 | long | VAH sviluppo 29.428 | -9 → +130 | 29.436-29.440 | 0 | 1,1x |
| 605 | 17:16:00 | 19:16 | short | VAH sviluppo 29.452 | +6 → -109 | 29.439-29.442 | 0 | 1,7x |
| 613 | 17:31:10 | 19:31 | long | VAH sviluppo 29.452 | -176 → +84 | 29.464-29.470 | 0 | 0,7x |
| 673 | 19:55:05 | 21:55 | short | POC sviluppo 29.415 | +122 → -115 | 29.399-29.403 | 0 | 0,4x |

Cinque candidati su otto non hanno **nessun** trade da 50 lotti dentro la barra. Dei tre che ce l'hanno, due falliscono l'accelerazione del tape.

## Il Candidato Piu' Vicino

La barra **388**, alle 16:39 ora italiana, e' l'unica che manca la conferma per un margine trascurabile: il volume di barra e' 1,29 volte la mediana contro una soglia di 1,30.

Abbassando la soglia del tape a 1,25 l'operazione si apre:

```text
LONG a VAL in sviluppo 29.372
  delta -27 -> +158   VA 29.378-29.383   1 Big Trade Buy da 56 lotti
  entry  29.383,00 (limit sul VAH della barra di segnale)
  SL     29.378,50
  TP     29.387,50
  rischio 4,50 punti
  esito: target raggiunto alla barra successiva, +4,50 punti
```

Allentando ancora, a 1,00, entra anche la barra 477, che va a stop per -7,50 punti. Il bilancio delle due diventa **-3,00 punti**.

Vale la pena notare la direzione dell'errore: la sessione ha avuto delta -6.196 e i due segnali che il modello avrebbe potuto prendere erano entrambi **long**. Il modello non insegue il delta di sessione, e in questa giornata era la lettura corretta: il prezzo non e' sceso.

## Il Vincolo Che Domina

Piu' importante del numero di ingressi e' la loro scala.

```text
barra 40R          10,00 punti di ampiezza
value area di barra 5,00 punti in mediana
stop risultante     4,50 punti sul candidato reale
```

Con 1 MNQ, 4,50 punti valgono **9,00 dollari lordi**. Sulle fee percentuali NDX-USD registrate in [`prop/PROP_TRADING_KNOWLEDGE.md`](../../../prop/PROP_TRADING_KNOWLEDGE.md), circa 0,014% round-trip su un notional equivalente di circa 58.800 dollari, il costo e' circa **8,20 dollari**.

Resta meno di un dollaro. Un solo tick di slippage sul limit ribalta il segno.

Questo e' il risultato piu' solido della giornata, ed e' indipendente dalla qualita' della lettura del flusso: a 40 Range su NQ, un TP 1:1 sullo stop di value area e' dello stesso ordine di grandezza del costo di transazione su una prop a fee percentuali.

## Limiti

- Una sola sessione. Nessuna delle percentuali qui sopra e' una statistica.
- Speed of Tape approssimato dal volume di barra: ATAS non lo espone in forma interrogabile.
- Il replay non modella slippage, riempimenti parziali o code sul limit.
- Quando stop e target cadono dentro la stessa barra l'ordine dei due tocchi non e' ricostruibile dal footprint: il replay marca l'esito `ambiguo` invece di sceglierne uno. Sulla sessione del 2026-09-11 non e' avvenuto.
