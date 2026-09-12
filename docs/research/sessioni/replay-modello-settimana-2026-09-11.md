# Replay Del Modello 40R Sulla Settimana 2026-09-04 / 09-11

Domanda: applicando [The Prop Firm Model](../modelli/modello-40r-riferimento.md) alla lettera su barre 40 Range, cosa sarebbe successo?

Metodo: [`replay_model.py`](../../../FabioOrderFlow/tools/replay_model.py) decide su ogni barra **solo con cio' che era noto alla sua chiusura**. Nessuna condizione usa dati successivi. Dati dal Fabio Data Bridge, NQU6@CME, cinque sessioni cash.

Stato: **descrizione di una settimana**. Non una validazione.

## Prima Versione Sbagliata, E Perche'

La prima esecuzione di questo replay non implementava il modello del dossier ma la sua sintesi in `prop/PROP_TRADING_KNOWLEDGE.md`, e divergeva in tre punti:

| | Implementato | Dossier |
|---|---|---|
| Livelli | condizione obbligatoria | contesto di mercato, non condizione |
| Big Trades e Speed of Tape | conferma obbligatoria | non sono fra le tre condizioni |
| Finestra oraria | tutta la cash | pre-market e prime due ore di RTH, poi OFF esplicito |

Il filtro sui Big Trades azzerava gli ingressi: con esso il modello sembrava non operare mai. Era un artefatto dell'implementazione. Il risultato e' registrato qui perche' la correzione e' piu' istruttiva del risultato corretto.

## La Settimana

Configurazione: flip minimo 40, stop al bordo opposto della value area, finestra 13:30-15:30 UTC, nessun filtro di livello, nessun filtro di Big Trades.

| Sessione | Operazioni | Vinte | Win rate | Punti | R | Rischio mediano |
|---|---|---|---|---|---|---|
| 2026-09-04 | 23 | 10 | 43% | -12,00 | -3 R | 5,00 |
| 2026-09-08 | 28 | 14 | 50% | -7,25 | 0 R | 5,00 |
| 2026-09-09 | 24 | 14 | 58% | +10,75 | +4 R | 5,00 |
| 2026-09-10 | 30 | 19 | 63% | +39,50 | +8 R | 5,00 |
| 2026-09-11 | 20 | 12 | 60% | +16,00 | +4 R | 6,00 |
| **totale** | **125** | **69** | **55%** | **+47,00** | **+13 R** | |

## La Finestra Oraria Regge, La Soglia Del Flip No

```text
flip   stop      finestra   op  vinte    WR       R
   1     va   13:30-15:30  246    110   45%  -26,00
   1     va   13:30-20:00  358    148   41%  -62,00
   1    bar   13:30-15:30  263    117   44%  -29,00
   1    bar   13:30-20:00  376    161   43%  -54,00
  40     va   13:30-15:30  125     69   55%  +13,00
  40     va   13:30-20:00  206    103   50%    0,00
  40    bar   13:30-15:30  139     72   52%   +5,00
  40    bar   13:30-20:00  222    110   50%   -2,00
  80     va   13:30-15:30   52     25   48%   -2,00
  80     va   13:30-20:00   97     43   44%  -11,00
  80    bar   13:30-15:30   58     29   50%    0,00
  80    bar   13:30-20:00  104     49   47%   -6,00
```

**Sei confronti su sei migliorano restringendo la finestra a 13:30-15:30.** E' l'unico risultato di questa pagina che non sia stato scelto guardando i dati: il dossier dichiara la finestra in anticipo, e la direzione tiene in ogni configurazione.

**La soglia del flip e' un'altra cosa.** Il valore migliore e' 40, ma 1 da' -26 R e 80 da' -2 R: non e' monotona, sono tre punti testati, e la soglia e' stata scelta guardando questa tabella. Non e' un parametro, e' un'ipotesi da verificare su settimane non ancora osservate.

**Lo stop non fa differenza.** Bordo della value area ed estremo di barra danno risultati indistinguibili in R. Anche una variante con stop oltre il livello, provata prima della correzione, era la peggiore delle tre: allargare il rischio non migliora il rapporto.

## Il Conto Che Decide

Nella configurazione migliore, **+13 R su 125 operazioni fanno +0,10 R per operazione**, con rischio mediano 4,50-6,00 punti.

Su 1 MNQ, cinque punti di rischio valgono dieci dollari, quindi il lordo atteso e' circa **un dollaro per operazione**.

- Commissione round-trip su futures reali: circa 1,00-1,50 dollari per contratto.
- Fee percentuali NDX-USD registrate in [`prop/PROP_TRADING_KNOWLEDGE.md`](../../../prop/PROP_TRADING_KNOWLEDGE.md): circa 0,014% round-trip, cioe' circa 8 dollari su un notional equivalente.

Il sizing del dossier, 250 dollari di rischio per operazione con cap giornaliero di 6 R su un conto da 50K, non cambia il rapporto: le commissioni scalano con la quantita' esattamente come il profitto.

**L'edge lordo misurato su questa settimana e' dello stesso ordine di grandezza del costo di transazione.** Non e' una questione di stop o di filtri: sono 125 operazioni al 55% su un obiettivo 1:1.

## Cosa Manca Alla Misura

La condizione 3 del modello, il controllo del lato monitorato mentre la candela si forma, **non e' applicata**: dal footprint di una barra chiusa non si ricostruisce se il controllo e' girato prima o dopo l'esecuzione del limit.

E' la condizione che cancella gli ordini pendenti nelle candele che stanno girando, cioe' proprio quelle che qui diventano stop immediati. I numeri di questa pagina sono quindi un **limite inferiore**: il modello completo ne toglierebbe una parte dei perdenti.

Quanta parte, non si sa. Misurarlo richiede di registrare il delta cumulato dentro la candela in formazione, cosa che il bridge puo' fare in live e non in storico.

## Limiti

- Cinque sessioni, 125 operazioni. Non e' un campione: +13 R e' compatibile con il caso.
- Le convenzioni che rendono eseguibile il modello sono scelte di chi scrive, non del dossier.
- Nessuna modellazione di slippage, riempimenti parziali o code sul limit.
- Il dossier prescrive DeepCharts; qui si misura su ATAS.
