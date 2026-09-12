# The Prop Firm Model: Modello 40R Di Riferimento

**Fonte di verita': il dossier in [`prop/prop_firm_model/`](../../../prop/prop_firm_model/)**, *The Prop Firm Model — a method by Fabio 'Fabervaale' Valentini*, nove pagine. Dove questo documento e il dossier divergono, vale il dossier.

Stato: **modello candidato, non validato in questo repository**. Le misure qui sotto sono descrizioni di una settimana, non una verifica.

## Le Tre Condizioni

Il dossier e' esplicito: *"Do not place the limit order unless all three conditions are confirmed on the closing 40R candle."*

1. **Auction flip.** Il delta gira: i compratori prendono il controllo dai venditori per un long, l'inverso per uno short.
2. **Value area shift.** La value area della nuova candela e' piu' alta della precedente per un long, piu' bassa per uno short.
3. **Side control, in tempo reale.** Mentre la candela successiva si forma, il lato deve restare in controllo. Se il controllo gira a meta' candela, **l'ordine pendente si cancella**.

## Esecuzione

| | |
|---|---|
| Entry | limit al **VAH** della 40R chiusa per i long, al **VAL** per gli short |
| Stop | **VAL** della stessa candela, oppure il suo minimo |
| Take profit | **1:1**, piazzato automaticamente via OCO |
| Quantita' | calcolata dal software dal rischio in valuta |
| Non eseguito | non si insegue. *"It is part of the game."* |

## Contesto E Finestra

Il livello **non e' una condizione**. Il dossier lo colloca fra i due contesti in cui il modello lavora meglio:

- **mercati in tendenza**: VA impilate nella stessa direzione, asta saldamente controllata;
- **mean reversion su key level**: il delta gira l'asta e la VA si sposta contro la tendenza precedente.

La finestra invece e' una regola:

- **ON**: pre-market e **prime due ore della RTH di New York**.
- **OFF**: ore del pranzo e sessione serale. Liquidita' sottile, prezzo disordinato.

Configurazione chart: template OrderTrack in DeepCharts, 40 Range, Volume POC, Delta POC, Delta candles, Value Area con Show Line e Highlight attivi.

## Cosa Il Modello Lascia Indeterminato

Tre condizioni su tre contengono una parola che nessun dato definisce. Renderle esplicite e' la condizione per misurarle, e ogni scelta e' una convenzione.

| Condizione | Indeterminato | Convenzione nel replay |
|---|---|---|
| 1 | quanto grande dev'essere il flip | segno del delta invertito e nuovo delta di almeno 40 in valore assoluto |
| 2 | cosa significa "VA piu' alta" | **entrambi** i bordi si spostano nella direzione del lato |
| 3 | come si misura il controllo intrabarra | **non replicabile in storico**, vedi sotto |
| entry | quanto resta valido il limit | tre barre, poi si cancella |
| finestra | quali sono le "prime due ore" | 13:30-15:30 UTC, cioe' 09:30-11:30 New York |

Sono le costanti in testa a [`replay_model.py`](../../../FabioOrderFlow/tools/replay_model.py), tutte sovrascrivibili da riga di comando perche' la sensibilita' alla soglia faccia parte del risultato.

### La condizione 3 non e' replicabile in storico

Il footprint di una barra chiusa dice quanto delta ha accumulato e quali estremi ha toccato, ma non l'ordine temporale degli eventi al suo interno. Sapere se il controllo e' girato **prima** che il limit venisse eseguito richiede il flusso in formazione, non la barra finita.

Questo non e' un dettaglio: la condizione 3 cancella gli ordini pendenti proprio nelle candele che stanno girando, cioe' quelle che nel replay diventano stop immediati. Una parte dei perdenti misurati non sarebbe mai entrata.

Misurarla richiede una registrazione live del delta cumulato dentro la candela in formazione. Il bridge puo' farlo; lo storico no.

## Il Problema Di Scala

Su NQ a 40 Range, misurato sulla settimana 2026-09-04 / 09-11:

```text
barra                10,00 punti di ampiezza
value area di barra   5,00 punti in mediana
stop risultante       4,50-6,00 punti
```

Con 1 MNQ, cinque punti valgono dieci dollari. E' il vincolo dominante e non dipende da quanto bene si legga il flusso: vedi il conto in [Replay sulla settimana](../sessioni/replay-modello-settimana-2026-09-11.md).

## Applicazioni

- [Replay sulla settimana 2026-09-04 / 09-11](../sessioni/replay-modello-settimana-2026-09-11.md).

## Limiti

- Una settimana. Nessuna delle percentuali misurate e' una statistica.
- La condizione 3 non e' applicata: i risultati sono quindi un **limite inferiore** del modello.
- Il delta di barra dipende dalla classificazione bid/ask di ATAS, non ispezionabile.
- Il replay non modella slippage, riempimenti parziali o code sul limit.
- Il dossier prescrive DeepCharts; qui si misura su dati ATAS. Le due piattaforme possono differire nella costruzione della value area di barra.
