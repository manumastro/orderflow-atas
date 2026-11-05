# La Scala Dei Tre Tempi

**Stato**: validata dentro, **smentita** (5 novembre 2026): nessuna delle cinque varianti dichiarate batte il
nullo, e il "tempo misto" non e' piu' stretto. Il fuori campione non e' stato guardato.
**Strategia nel catalogo**: `scala-dei-tre-tempi`, versione 1

## In Breve

Un video sul bias del giorno dice: guarda se mese, settimana e giorno salgono "a scalini" (massimo piu'
alto, minimo piu' alto e **chiusura sopra il massimo di prima**). Se tutti e tre salgono, si compra
soltanto; se tutti e tre scendono, si vende soltanto; se non sono d'accordo, il mercato e' in equilibrio,
e si lavora poco e solo ai bordi.

Su NQ, dal 2017 alla fine del 2024, **non regge**. Nelle 106 giornate con i tre tempi allineati, la cassa
dopo non va nel verso della scala piu' spesso del caso, e il netto e' negativo. Le altre quattro versioni
dichiarate fanno lo stesso o peggio. E le giornate con i tempi "misti" non sono piu' strette: sono un po'
**piu' larghe**. Come contesto per leggere il grafico la scala resta una descrizione onesta di com'e'
andato il prezzo; come previsione della giornata dopo, sui nostri dati, non dice niente.

## Il Paper

**Dal video, paper non ancora letto.** La fonte e' la trascrizione di un video YouTube sul bias del giorno
dalla price action, salvata in [`docs/research/fonti-video/2026-11-05-il-bias-dalla-price-action.txt`](../../../docs/research/fonti-video/2026-11-05-il-bias-dalla-price-action.txt),
citata come `[V1 · m:ss]`. E' un trader che mostra il suo metodo su ES, non una ricerca: un esempio, una
giornata, nessun campione.

Il filone affine e' il **time-series momentum** (Moskowitz, Ooi, Pedersen, *Time Series Momentum*, JFE
2012): il rendimento passato di un future predice il suo segno futuro, su orizzonti da uno a dodici mesi.
**Non letto**: e' citato solo per dire dove cercare, e il suo orizzonte non e' quello del video (un giorno).

## Il Finding

Non c'e' un finding misurato: ci sono affermazioni.

- **La scala** (one time framing, dall'auction market theory): il prezzo sale solo quando ogni candela
  "sale tutto lo scalino": massimo piu' alto, minimo piu' alto **e chiusura sopra il massimo della
  precedente**. "Most traders only check the first two. The third one is the one that matters"
  `[V1 · 0:53 - 1:23]`. Senza la chiusura e' una pausa, e "a pause is not a reason to go short either"
  `[V1 · 1:30 - 1:47]`.
- **Tre tempi**: il mese e' la corrente, la settimana la marea, il giorno l'onda; si giudica sempre
  l'ultima candela chiusa `[V1 · 2:53 - 3:18]`.
- **L'allineamento**: "All three stepping up, clear sky. I'm a buyer only. All three stepping down ...
  seller only ... when they disagree, that's mixed weather. Small size, walls only, or I stay on the
  ground" `[V1 · 3:18 - 3:35]`.
- **L'equilibrio**: senza scala il mercato fa "ping-pong" fra due muri; si lavora ai muri, mai in
  mezzo; un muro si rompe solo se il prezzo chiude oltre e ci resta `[V1 · 2:19 - 2:53]`.

Il video stesso avverte che "these rules are never wrong ... what will be wrong sometimes is your
forecast" `[V1 · 3:37 - 3:47]`: la definizione e' precisa, la previsione no. Ed e' proprio la previsione
che qui si prova.

## Il Perche'

Quello che il video suggerisce, senza dirlo: una chiusura oltre il massimo di prima vuol dire che il
mercato ha **accettato** il prezzo nuovo, non solo lo ha toccato ("the market didn't just visit the new
price, it accepted it", `[V1 · 1:19 - 1:23]`). Se chi muove il prezzo sui tempi lunghi (i fondi, "the big
funds live" nel mensile, `[V1 · 3:01 - 3:04]`) continua a comprare, i tempi corti vanno nello stesso
verso, e quando tutti e tre concordano la spinta e' piu' probabile. E' l'idea del momentum, detta con le
candele.

## Cosa La Ucciderebbe

- **Un mercato che ritorna invece di proseguire**: se su un giorno il movimento forte viene riassorbito
  (mean reversion), la scala del giorno prima dice il verso sbagliato. Su NQ nella cassa dopo sembra
  essere cosi' (vedi Le Prove, V5), anche se non e' stato provato come ipotesi.
- **I costi e l'orizzonte**: il momentum dei paper vive su mesi; un'operazione di una cassa paga lo
  spread e il rumore di una giornata intera per un segnale lento.
- **L'arbitrarieta' delle candele**: settimana e mese hanno confini di calendario; un massimo fatto
  lunedi' o venerdi' cambia la candela, non il mercato.

## Le Regole Da Provare

Dichiarate prima di guardare NQ, con i criteri sigillati:
[`5-validazione/criteri/2026-11-05-la-scala-dei-tre-tempi.md`](5-validazione/criteri/2026-11-05-la-scala-dei-tre-tempi.md),
SHA-256 `900e1ce727e7b296c46554f75dcd0bf4f38c41b0b432e1e9e9eaf4307733b291`.

**Le candele.** Il giorno e' la giornata di borsa CME (dalle 18:00 ET del giorno prima). La settimana
(ISO) e il mese raggruppano le giornate di borsa per la loro data. Si giudicano solo candele **chiuse**:
la settimana e il mese in corso non contano. La prima candela della serie non si usa (e' parziale).
Prezzi rettificati all'indietro ai rollover.

- **ingresso**: se tutti i tempi scelti sono nello stesso verso, a mercato alle 9:30 ET, in quel verso;
  con i tempi misti, niente;
- **uscita**: alle 16:00 ET, a tempo; nessuno stop;
- **dimensione**: un contratto.

**Le cinque varianti**, e nessun'altra:

| | parametri | perche' |
|---|---|---|
| V1 | `tempi=msg memoria=candela` | la regola letterale, "I always judge the last completed candle" `[V1 · 3:14]`; **la principale** |
| V2 | `tempi=msg memoria=scalino` | come il video legge il mese: dopo due pause "the current is still up, it's just resting" `[V1 · 3:58 - 4:08]`; lo stato e' il verso dell'ultimo scalino |
| V3 | `tempi=sg memoria=candela` | senza il mese, che cambia di rado |
| V4 | `tempi=sg memoria=scalino` | |
| V5 | `tempi=g memoria=candela` | il controllo: il solo giorno |

**Una descrizione, non una strategia** (K3): con i tempi misti la cassa dovrebbe essere piu' stretta
(equilibrio). Si misura il range della cassa nei giorni misti contro quelli allineati, una giornata per
osservazione; il codice e' in [`4-backtest/misura-tempo-misto/`](4-backtest/misura-tempo-misto/Program.cs),
e non passa dal registro.

**Fuori dalla prova**: i "quattro piani di volo" con la conferma dell'order flow e la perdita massima
`[V1 · 5:16 - 6:19]` sono esecuzione: non si provano qui.

## Le Prove

**Convenzioni**: NQ M1 dall'archivio di ATAS, **dentro campione** dal 13 aprile 2017 al 31 dicembre 2024
(1.994 giornate), rettificato all'indietro; 1 contratto (20 $ a punto), commissione 2,50 $ per lato, 1 tick
di slittamento sul mercato, ambiguita' al peggio; nullo con 1.000 repliche, Monte Carlo con 10.000
simulazioni. **Varianti provate: 5**, tutte nel registro (`quant/registro/prove.jsonl`). Il fuori campione
(dal 2025) non e' stato guardato.

| data | variante | impronta | operazioni | netto | vincenti | t per giornata | D1 (quando) | D2 (verso) | D3 (deflazionato, 5) | rapporto |
|---|---|---|---:|---:|---:|---:|---:|---:|---:|---|
| 2026-11-05 | V1 msg candela | `2b672ed246` | 106 | -26.720 $ | 50,9% | -0,85 | p 0,858 | p 0,783 | 0,072 | [rapporto](5-validazione/risultati/2026-11-05-1210-2b672ed246-dentro.md) |
| 2026-11-05 | V2 msg scalino | `846d99368f` | 710 | -97.880 $ | 52,7% | -1,39 | p 0,939 | p 0,885 | 0,028 | [rapporto](5-validazione/risultati/2026-11-05-1210-846d99368f-dentro.md) |
| 2026-11-05 | V3 sg candela | `9e2862e665` | 297 | -61.375 $ | 50,8% | -1,32 | p 0,911 | p 0,898 | 0,029 | [rapporto](5-validazione/risultati/2026-11-05-1210-9e2862e665-dentro.md) |
| 2026-11-05 | V4 sg scalino | `2a681e9f8c` | 1.241 | -118.250 $ | 51,9% | -1,29 | p 0,894 | p 0,854 | 0,037 | [rapporto](5-validazione/risultati/2026-11-05-1210-2a681e9f8c-dentro.md) |
| 2026-11-05 | V5 g candela | `56bb915abe` | 851 | -157.005 $ | 49,7% | -1,97 | p 0,975 | p 0,973 | 0,006 | [rapporto](5-validazione/risultati/2026-11-05-1210-56bb915abe-dentro.md) |

**K3, il tempo misto** (dentro campione, giornate con almeno 300 minuti di cassa e 20 casse prima): 103
giornate allineate, 1.767 miste. Range di cassa mediano: **allineate 151,0 punti, miste 165,5**. Welch sul
logaritmo del range (misti meno allineati) t = +1,92, p a una coda 0,973: le miste sono piu' larghe, non
piu' strette. Relativo alla media delle 20 casse prima: 0,877 contro 0,919, t = +0,21 (nessuna differenza).

**L'esito dei criteri sigillati**: [`5-validazione/criteri/2026-11-05-la-scala-dei-tre-tempi-esito.md`](5-validazione/criteri/2026-11-05-la-scala-dei-tre-tempi-esito.md).
K1 smentita, K2 smentita, K3 smentita.

**Una cosa vista, non provata.** Tutte e cinque le varianti hanno t per giornata negativo, e il solo
giorno (V5) arriva a -1,97: dopo uno scalino del giorno, la cassa seguente e' andata un po' piu' spesso
**contro**. Non e' un finding: e' la quinta di cinque prove, e girare il verso sarebbe una variante nuova
scelta dopo aver visto i dati. Se si vuole provarla, serve una scheda sua, con il nullo e lo Sharpe
deflazionato che contano anche queste cinque.

**Il framework**, il 5 novembre 2026: il rapporto di `quant valida` scriveva le regole da una strategia non
preparata (gli orari risultavano "00:00 ET", anche per le strategie gia' nel catalogo). Corretto in
`Validatore.cs`; 56 test passano, e sulla passeggiata casuale (`SINT`, registro nella scratchpad) nessuna
delle quattro strategie del catalogo supera i cancelli.
