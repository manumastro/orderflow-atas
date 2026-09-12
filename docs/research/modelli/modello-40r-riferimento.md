# Modello 40R Di Riferimento

Stato: **modello candidato, non validato**. Nessuna regola qui dentro e' stata verificata su un campione. Il repository resta osservativo: questo documento serve a rendere il modello abbastanza preciso da poter essere sbagliato.

Il modello nasce dalla pratica operativa discussa attorno al 2026-09-01 ed era finora incorporato in [`prop/PROP_TRADING_KNOWLEDGE.md`](../../../prop/PROP_TRADING_KNOWLEDGE.md). E' stato separato perche' e' una cosa a se': descrive come leggere il flusso, non quale prop usare. Le regole delle prop cambiano; il modello no.

## La Sequenza

```text
livello importante
  -> delta flip
  -> VA shift
  -> Big Trades + tape confermano
  -> pullback al bordo della value area
  -> entry limit
  -> SL lato opposto
  -> TP 1:1
```

1. **Livello importante**: VAH, VAL, POC, HVN/LVN, massimo o minimo, oppure un'area gia' reattiva.
2. **Delta flip**: il controllo passa da venditori a compratori per un long, l'inverso per uno short.
3. **VA shift**: la nuova value area si sposta piu' in alto per un long, piu' in basso per uno short.
4. **Conferma**: Big Trades e Speed of Tape devono sostenere il lato. Non sono il trigger primario.
5. **Pullback**: si attende il ritorno sul bordo della value area della 40R appena chiusa. Non si insegue.
6. **Entry**: limit. Long tipicamente su VAH, short su VAL.
7. **Pending**: se il controllo cambia, l'ordine si cancella.
8. **SL**: bordo opposto della value area, oppure estremo tecnico della barra.
9. **TP**: 1:1 sul rischio lordo.
10. **Sessione**: finestre liquide. Si evitano le fasi morte se il flusso non e' leggibile.

Configurazione ATAS: 40 Range, Volume POC, Delta POC, Delta candles, Value Area lines, Big Trades, Speed of Tape, Volume Profile di contesto.

## Cosa Il Modello Lascia Indeterminato

Nella forma sopra il modello non e' eseguibile da un programma: sei punti su dieci contengono una parola che nessun dato definisce. Renderli espliciti e' la condizione per poterlo misurare, e ogni scelta e' una convenzione, non un fatto.

| Punto | Indeterminato | Convenzione adottata nel replay |
|---|---|---|
| 1 | quanto vicino e' "a un livello" | il livello cade dentro il range della barra piu' 10 punti |
| 1 | quali livelli, oltre a quelli ereditati | POC, VAH e VAL **in sviluppo** della sessione, calcolati sulle sole barre chiuse |
| 2 | quanto grande dev'essere il flip | segno del delta invertito e nuovo delta di almeno 80 in valore assoluto |
| 3 | cosa significa "VA piu' alta" | **entrambi** i bordi della value area si spostano nella direzione del lato |
| 4 | cosa conta come Big Trade | almeno un trade aggregato da 50 lotti sul lato, dentro la finestra della barra |
| 4 | cosa conta come tape veloce | volume di barra almeno 1,3 volte la mediana delle 20 barre precedenti |
| 5 | quanto resta valido il limit | tre barre, poi si cancella |
| 8 | quale dei due stop | bordo opposto della value area; l'estremo di barra e' selezionabile |

Sono le costanti in testa a [`FabioOrderFlow/tools/replay_model.py`](../../../FabioOrderFlow/tools/replay_model.py), tutte sovrascrivibili da riga di comando perche' la sensibilita' alla soglia faccia parte del risultato.

## Il Problema Di Scala, Misurato

Su NQ a 40 Range, con i dati della cash del 2026-09-11:

- una barra e' larga **10 punti** (40 tick);
- la value area di barra e' larga **5 punti** in mediana;
- quindi uno stop al bordo opposto della value area vale **4-8 punti**, e un TP 1:1 altrettanto.

Questo e' il vincolo dominante del modello, e non dipende da quanto bene si legga il flusso. Con 1 MNQ (2 dollari a punto) un target da 4,5 punti vale **9 dollari lordi**. Sulle fee percentuali registrate per NDX-USD in [`prop/PROP_TRADING_KNOWLEDGE.md`](../../../prop/PROP_TRADING_KNOWLEDGE.md), circa 0,014% round-trip su un notional equivalente di circa 58.800 dollari, il costo e' di circa **8 dollari**.

Il netto e' quindi prossimo a zero. Su una prop futures-native con commissione fissa per contratto il conto cambia, ma resta stretto. La conclusione e' che a 40R il collo di bottiglia non e' la qualita' del segnale: e' il rapporto fra ampiezza della value area di barra e costo di transazione.

## Applicazioni

- [Replay sulla cash del 2026-09-11](../sessioni/replay-modello-2026-09-11.md): zero ingressi con le convenzioni dichiarate.

## Limiti

- Una sola sessione applicata finora. Nessuna statistica.
- Speed of Tape e' approssimato dal volume di barra: ATAS non lo espone a un indicatore in forma interrogabile.
- Il delta di barra dipende dalla classificazione bid/ask di ATAS, non ispezionabile.
- Il replay non modella slippage sul limit, ne' il rifiuto parziale di un ordine.
