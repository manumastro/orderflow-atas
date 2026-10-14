# Il Framework Si Valida Prima Delle Strategie

*Aggiunta del framework.* Prima di credere a cio' che il framework dice di una strategia, lo si
prova dove la verita' e' nota: **un mercato finto** (`quant sintetico`, `Sintetico.cs`). Deve fare
due cose: **non trovare niente dove non c'e' niente**, e **trovare cio' che si e' piantato**.

**Ogni modifica al motore o alla validazione ripete questa prova**, insieme ai test, prima di
toccare dati veri. La prova usa `QUANT_REGISTRO`, `QUANT_RISULTATI` e `QUANT_DATI` puntati alla
scratchpad: mai il registro vero.

```bash
quant sintetico --giorni 1500 --seme 7                    # passeggiata casuale
quant sintetico --giorni 1500 --seme 7 --deriva-notte 15  # con una deriva piantata di notte
quant divisione --simbolo SINT --fissa
quant valida <strategia> --simbolo SINT --repliche 400 [--fuori]
```

## Il Risultato

Misurato l'11 ottobre 2026, 1.500 giornate sintetiche per prova, 400 repliche del nullo:

| prova | esito |
|---|---|
| **passeggiata casuale**, 3 strategie x 20 semi | **nessuna supera i cancelli**; D1 a p <= 0,05 in 3 casi su 60, il 5% atteso: il nullo e' tarato |
| **deriva piantata di notte**, +15 punti a notte | `anomalia-notte` passa D3, F2, F3 e i cancelli fuori; **D1 no** |
| deriva di notte +15 / +30 / +50 punti, 5 semi ciascuna | D1 a p <= 0,05 in 0, 0 e 1 caso su 5 |

**Il limite che ne esce: D1 ha poca potenza sulle anomalie di esposizione.** Quando la strategia
*e'* una finestra di esposizione (la notte), le finestre casuali con la stessa esposizione coprono
gran parte della stessa notte, e la differenza da misurare si riduce. Per quelle ipotesi la prova
giusta e' il confronto con il complemento, che e' anche quello del paper dell'esempio 4:
`anomalia-notte --p parte=notte` contro `--p parte=giorno`. Un confronto appaiato fra due varianti
non e' ancora un comando.

## I Difetti Trovati Cosi'

Due difetti del nullo, corretti prima della versione attuale:

1. **il nullo del quando dentro la stessa giornata sbirciava il futuro.** Il verso di un'operazione
   VWAP dipende dal percorso della giornata; spostarla prima nella stessa giornata le faceva
   catturare il movimento che l'aveva generata. Sulla passeggiata casuale quel nullo guadagnava
   +447.000 $. Ora la giornata si pesca a caso;
2. **il nullo del verso premiava il beta**: su un campione salito per caso, una strategia solo long
   batteva la moneta. Ora conta solo per le strategie che scelgono il verso.

## Il Simulatore Di Contratto

Lo stesso obbligo vale per il simulatore di contratto
([`il-simulatore-di-contratto.md`](4-il-simulatore-di-contratto.md)): prima di
dire qualcosa su un contratto vero, deve riprodurre cio' che e' noto. I criteri sono stati
**sigillati con un hash prima di scrivere il simulatore** ([`quant/studi/contratto-prop/5-validazione/criteri/`](../studi/contratto-prop/5-validazione/criteri/)), e si
rieseguono con `quant contratto verifica` dopo ogni modifica al simulatore.

Esito dell'11 ottobre 2026 ([`quant/studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md`](../studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md)):

| criterio | cosa | esito |
|---|---|---|
| C1 | le cinque combinazioni di monitoraggio di TF §2.4 | 4 su 5; la quinta (pavimento e violazione alla chiusura) +13%: il target controllato durante la sessione, un parametro che la forma chiusa non ha |
| C2 | i quattro regimi del pavimento di TF §2.3 | passa, scarti entro lo 0,5% |
| C3 | la tabella 2 di Lim | passa, scarti entro 0,0034 |
| C4 | la deriva implicita di TF §5.6 | non passa come scritto; la tabella si riproduce esattamente con un'altra geometria, che il criterio aveva indovinato male |

In piu', i test unitari: la forma chiusa riproduce i numeri stampati da TF (§2.3, §2.4, §4.2, §8.5)
e il simulatore rispetta le regole su sessioni costruite a mano (limite morbido e rigido, pavimento
alla chiusura e intraday, blocco, scadenza, prelievi con consistenza, prelievi di Lim).
