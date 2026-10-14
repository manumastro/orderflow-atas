# La Politica Per Fase: I Risultati

**14 ottobre 2026.** L'incrocio dei tre paper sulle prop firm ([scheda](../../1-idea.md)) su Tradeify
Growth 50K, regole lette l'11 ottobre 2026. **Non e' una raccomandazione.** E' il valore del
contratto per un compratore **senza vantaggio**, sotto ipotesi dichiarate.

Convenzioni, per tutte le righe:
- sessioni browniane, d = 850 $ di riferimento (circa 2 MNQ tutta la seduta), 78 controlli;
- 100.000 cicli, seme 1, conto finanziato fino al pavimento o a 1.000 sessioni;
- stop e target del giorno eseguiti esattamente al livello;
- la scala moltiplica deviazione, deriva e costi.

Colonne:
- **P**: passare la valutazione a ogni tentativo;
- **prelievo**: almeno un prelievo, una volta finanziati;
- **netto**: netto medio per ciclo, cioe' incassato meno pagato, in $ (SE e' il suo errore Monte Carlo);
- **perdita**: cicli in perdita.

## L'Esito Dei Criteri Sigillati

**L'incrocio non regge come scritto**: K1 e K4 sono smentiti, K2 e K3 passano
([esito](../criteri/2026-10-14-la-politica-per-fase-esito.md)). La politica migliore, scelta in
(a), e' `v4-f1`: scala 4 in valutazione, scala 0,5 nel finanziato.

| scenario | `v4-f1` | la fissa migliore in quello scenario | chi vince |
|---|---|---|---|
| (a) riferimento | 442 $ | `fissa-4` 563 $ | la fissa (K1 smentito) |
| (b) costi | 367 $ | `fissa-4` 444 $ | la fissa (K4 smentito) |
| (c) un solo prelievo | **+57 $** | `fissa-1` -117 $ | la politica (K3) |
| (e) senza limite giornaliero | 422 $ | `fissa-1` 272 $ | la politica (K2) |

## Le Tabelle

### (a) Riferimento: nessun vantaggio, il conto dura

| politica | valutazione | finanziato | P | prelievo | netto | SE | mediana | perdita |
|---|---|---|---|---|---|---|---|---|
| fissa-1 | 1 | 1 | 0,302 | 18,3% | 310 | 8 | -240 | 81,8% |
| fissa-2 | 2 | 2 | 0,366 | 12,2% | 346 | 10 | -240 | 87,9% |
| fissa-4 | 4 | 4 | 0,428 | 8,9% | **563** | 12 | -240 | 91,1% |
| v1-f1 | 1 | 0,5 | 0,302 | 24,5% | 345 | 6 | -240 | 75,7% |
| v1-f2 | 1 | 1, ±400 | 0,302 | 24,9% | 329 | 5 | -240 | 75,2% |
| v1-f3 | 1 | 1, +300 / -600 | 0,302 | 18,8% | 89 | 4 | -240 | 81,3% |
| v2-f0 | 2 | 1 | 0,366 | 18,3% | 365 | 8 | -240 | 81,8% |
| v2-f1 | 2 | 0,5 | 0,366 | 24,4% | 402 | 6 | -145 | 75,6% |
| v2-f2 | 2 | 1, ±400 | 0,366 | 24,9% | 386 | 5 | -145 | 75,1% |
| v2-f3 | 2 | 1, +300 / -600 | 0,366 | 18,7% | 145 | 4 | -240 | 81,3% |
| v4-f0 | 4 | 1 | 0,428 | 18,3% | 406 | 8 | -145 | 81,8% |
| **v4-f1** | 4 | 0,5 | 0,428 | 24,4% | **442** | 6 | -145 | 75,6% |
| v4-f2 | 4 | 1, ±400 | 0,428 | 24,9% | 423 | 5 | -145 | 75,1% |
| v4-f3 | 4 | 1, +300 / -600 | 0,428 | 18,7% | 183 | 4 | -145 | 81,3% |

`v1-f0` coincide con `fissa-1`.

### Il netto negli altri scenari

| politica | (b) costi | (c) 1 prelievo | (d) 3 prelievi | (e) senza limite |
|---|---|---|---|---|
| fissa-1 | 217 | -117 | 93 | 272 |
| fissa-2 | 240 | -145 | 40 | 135 |
| fissa-4 | **444** | -151 | 50 | 35 |
| v1-f1 | 265 | -34 | 232 | 339 |
| v1-f2 | 294 | -28 | 301 | 321 |
| v1-f3 | 62 | -111 | 81 | 80 |
| v2-f0 | 274 | -63 | 149 | 318 |
| v2-f1 | 326 | 20 | 286 | 385 |
| v2-f2 | 352 | 27 | 358 | 369 |
| v2-f3 | 121 | -57 | 136 | 127 |
| v4-f0 | 317 | -26 | 188 | 356 |
| v4-f1 | 367 | 57 | 324 | **422** |
| v4-f2 | 392 | **64** | **395** | 406 |
| v4-f3 | 161 | -19 | 174 | 163 |

SE da 1 a 12 $; P da 0,295 a 0,428 a seconda della scala in valutazione e dello scenario.

## Cosa Si Legge

I numeri sopra sono misure; questa sezione e' lettura.

1. **I due strati funzionano ciascuno da solo.** Grande in valutazione alza P da 0,30 a 0,43 (TF).
   Piccolo o con un bracket nel finanziato alza i conti con almeno un prelievo dal 18% al 25% (Y). A
   parita' di finanziato, passare da scala 1 a 4 in valutazione vale +97 $ (`v1-f1` → `v4-f1`).
2. **Il bracket asimmetrico (+300 / -600) e' il peggiore** in ogni scenario. Tante giornate piccole
   soddisfano i 5 giorni sopra 150 $, ma uno stop doppio del target, senza vantaggio, consuma il
   drawdown di 2.000 $ in poche giornate.
3. **La dimensione grande fissa vince solo se si esce esattamente al limite giornaliero**: 563 $
   con quell'ipotesi, 35 $ senza. E vince con il 91% dei cicli in perdita. E' la sensibilita' gia'
   vista nello studio del contratto, e qui decide il criterio.
4. **E' anche l'unico risultato che resta positivo se la firma chiude il conto al primo prelievo**:
   con `v4-f2` e `v4-f1` il netto resta sopra zero (+64 $, +57 $), mentre ogni dimensione fissa
   perde. E' il caso che lo studio del contratto indicava come il piu' pericoloso per il compratore.
5. **La mediana resta negativa ovunque** (-145 $ o -240 $): tre cicli su quattro perdono comunque.
   La politica sposta il valore atteso, non la forma della lotteria.

## Cosa Non Dice

- **Nessun vantaggio, nessun dato vero.** E' il contratto sotto un moto browniano. Con la deriva
  implicita nel 17,2% pubblicato da Tradeify (-118 $ a sessione), lo studio del contratto da' -490 $
  per ciclo: nessuna politica qui e' stata provata contro quella deriva.
- **Le varianti erano 14**, dichiarate prima. P\* e' il massimo di 12: e' gonfiato dal caso, e la
  soglia di K1 ne teneva conto.
- **Stop e target esatti al livello**: lo stesso tipo di ipotesi ottimista del limite giornaliero,
  ma su livelli piu' piccoli (400 $) e scelti da chi opera, non dalla firma.
- **Le regole sono quelle dell'11 ottobre** e la firma le cambia [TF §6].
- Per rieseguire con un criterio diverso, per esempio con P\* e F\* scelti in (e), serve un nuovo
  file di criteri sigillato **prima**.
