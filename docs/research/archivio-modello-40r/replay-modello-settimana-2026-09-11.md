# Replay Del Modello 40R Sulla Settimana 2026-09-04 / 09-11

Domanda: applicando [The Prop Firm Model](modello-40r-riferimento.md) alla lettera su barre 40 Range, cosa sarebbe successo?

Metodo: [`replay_model.py`](../../../FabioOrderFlow/tools/replay_model.py) decide su ogni barra **solo con cio' che era noto alla sua chiusura**. Dati dal Fabio Data Bridge, NQU6@CME, cinque sessioni cash, finestra 13:30-15:30 UTC.

Stato: **descrizione di una settimana**. Non una validazione.

## Risultato

Tutte e tre le condizioni del dossier applicate, esito risolto sul tape trade per trade:

| Sessione | Operazioni | Vinte | Win rate | R |
|---|---|---|---|---|
| 2026-09-04 | 30 | 10 | 33% | -10 |
| 2026-09-08 | 28 | 9 | 32% | -10 |
| 2026-09-09 | 22 | 8 | 36% | -6 |
| 2026-09-10 | 32 | 14 | 44% | -4 |
| 2026-09-11 | 21 | 12 | 57% | +3 |
| **totale** | **133** | **53** | **40%** | **-27 R** |

Con un obiettivo 1:1 il pareggio lordo richiede piu' del 50%. Quaranta per cento su 133 operazioni e' una perdita, non un margine sottile.

## Due Errori Corretti, In Ordine

Questa pagina ha avuto tre versioni. Le prime due erano sbagliate e la sequenza degli errori conta piu' del risultato.

### Primo errore: il modello implementato non era il modello

La prima esecuzione veniva dalla sintesi in `prop/PROP_TRADING_KNOWLEDGE.md`, non dal dossier, e divergeva su tre punti:

| | Implementato | Dossier |
|---|---|---|
| Livelli | condizione obbligatoria | contesto di mercato, non condizione |
| Big Trades e Speed of Tape | conferma obbligatoria | non sono fra le tre condizioni |
| Finestra oraria | tutta la cash | pre-market e prime due ore di RTH |

Il filtro sui Big Trades azzerava gli ingressi: il modello sembrava non operare mai. Era un artefatto dell'implementazione.

### Secondo errore: la risoluzione a barre guardava avanti

Corretto il primo errore, il modello sembrava dare **+13 R al 55% su 125 operazioni**. Quel numero era falso.

L'esito veniva deciso confrontando lo stop e il target con il minimo e il massimo della barra di esecuzione. Ma il massimo di quella barra puo' essere stato stampato **prima** che il limit venisse eseguito: un target contato come raggiunto con un prezzo anteriore all'ingresso. Look-ahead.

Il tape completo, che porta l'ordine temporale, lo elimina. L'effetto isolato, a condizione 3 disattivata:

```text
        risoluzione a barre        risoluzione sul tape
09-04   23 op  43%    -3 R         32 op  31%  -12 R
09-08   28 op  50%     0 R         30 op  30%  -12 R
09-09   24 op  58%    +4 R         26 op  42%   -4 R
09-11   20 op  60%    +4 R         23 op  61%   +5 R
```

Tre giorni su quattro perdono fra i 12 e i 16 punti percentuali di win rate. Il margine apparente era interamente l'artefatto.

## La Condizione 3 E' Quasi Neutra

La terza condizione del dossier, il controllo del lato monitorato mentre la candela si forma, e' applicabile in storico: il tape dice a che millisecondo ogni trade e' avvenuto e da che lato, quindi si sa se il controllo e' girato **prima** che il limit venisse toccato.

| Soglia di flip del controllo | Operazioni | Win rate | R |
|---|---|---|---|
| disattivata | 147 | 41% | -25 |
| 15 | 123 | 40% | -25 |
| 30 | 133 | 40% | -27 |
| 60 | 144 | 41% | -26 |

Cancella dal 2% al 16% degli ordini pendenti e **non sposta il win rate**. Toglie vincenti e perdenti nella stessa proporzione.

Era l'ipotesi che avevo formulato per spiegare la differenza fra i miei numeri e quelli del dossier. E' falsa, almeno su questa settimana e con questa definizione di controllo.

## Il Tape Completo, E Perche' Era Disponibile

`GetCumulativeTradesSessionLimit(Filter)` vale **0**, cioe' nessun limite. Con `minVolume 0` il bridge restituisce ogni trade aggregato con timestamp al millisecondo e direzione gia' classificata da ATAS.

Verifica di completezza, ricostruendo le barre dal tape:

```text
 barra  delta ATAS  delta tape  vol ATAS  vol tape
 28940         -27         -27       335       335
 28948         -33         -33       443       443
 28949         -47         -47       487       487
 28950           3           3       397       397
```

Due ore di tape sono circa 114.000 record, 10 MB, sette secondi di download. Il client li scarica con `--min-volume 0 --window-minutes 10 --compact`.

## Limiti

- Cinque sessioni, 133 operazioni. Non e' un campione sufficiente per una conclusione sul modello: e' sufficiente per dire che **su questa settimana non c'e' traccia di margine**.
- Le convenzioni che rendono eseguibile il modello, a partire dalla soglia sul delta flip, sono scelte di chi scrive e non del dossier.
- **Il modello di esecuzione e' ottimista**: si assume il riempimento al primo prezzo stampato al livello del limit, senza coda ne' riempimenti parziali. Un modello realistico peggiorerebbe il risultato, non lo migliorerebbe.
- Il dossier prescrive DeepCharts; qui si misura su ATAS, e la costruzione della value area di barra puo' differire.
- Il delta di barra e la direzione dei trade dipendono dalla classificazione bid/ask di ATAS, non ispezionabile.
