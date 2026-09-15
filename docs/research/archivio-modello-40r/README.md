# Archivio: The Prop Firm Model, Fase Sistematica Chiusa

Questa cartella conserva il lavoro sul modello 40R. **La fase e' chiusa il 15 settembre 2026.**
Non e' materiale da riprendere: e' evidenza di cosa e' stato misurato e con quale esito.

## Perche' E' Chiusa

Tutto cio' che il dossier e il primo transcript del corso dichiarano in forma codificabile e' stato
implementato e misurato su un mese di NQ, 20 sessioni. Il risultato sta sulla moneta: **49-51% con
obiettivo 1:1**, indistinguibile dal modello nullo che entra senza nessuna condizione.

Quello che non ha funzionato, in ordine:

| Cosa | Esito |
|---|---|
| Le tre condizioni del dossier | 49% su 436 operazioni, nullo 47% |
| Filtro sul ritmo della barra | 61% su 28 operazioni nella settimana, 49/50/48% sul mese |
| Big trades come filtro di barra | nessuna separazione su quattro finestre |
| Speed of tape | piatta in ogni cella |
| Big trades come costruzione del livello | 49-51% |
| Gate sul profile framing del giorno prima | porta la frequenza nell'intervallo dichiarato da Fabio, non muove il win rate |
| Scala della barra, da 20R a M5 | il win rate sale, ma sale anche quello del modello nullo: e' attrito che cala, non informazione |
| Verso della condizione 01 | il flip di delta non condiziona la direzione in nessuno dei due versi |

## Cosa Resta Valido Da Qui

Tre cose, e sono di metodo, non di modello:

1. **Gli errori di implementazione sono documentati in ordine** in
   [`replay-modello-settimana-2026-09-11.md`](replay-modello-settimana-2026-09-11.md). La sequenza
   vale piu' del risultato: sintesi al posto della fonte, look-ahead nella risoluzione degli esiti,
   e un limit order che si eseguiva a mercato.
2. **Le osservazioni dentro una sessione non sono indipendenti.** Un z ingenuo di +4,1 diventa
   t=+1,4 quando la statistica si calcola per sessione. E' la correzione che ha smontato ogni
   risultato apparente di questa fase.
3. **Separare la predittivita' dall'esecuzione.** `probe_predictability.py` misura quale barriera
   simmetrica viene toccata per prima, senza limit, spread o asimmetria di target. E' il modo di
   sapere se un segnale esiste prima di costruirci sopra una regola.

## Strumenti Di Questa Fase

```text
FabioOrderFlow/tools/replay_model.py           replay del modello, nell'ordine del dossier
FabioOrderFlow/tools/replay_sweep.py           stesso replay su piu' sessioni, un parametro alla volta
FabioOrderFlow/tools/build_bars.py             ricostruisce barre di range o tempo dal tape
FabioOrderFlow/tools/probe_predictability.py   predittivita' separata dall'esecuzione
```

`build_bars.py` e `probe_predictability.py` restano utili fuori da questa fase: il primo perche'
permette di guardare qualunque scala senza ricaricare ATAS, il secondo perche' misura senza passare
da una regola di ingresso.

## Dove Continua Il Lavoro

Lo scope attuale e' la lettura discrezionale del primo transcript del corso. Vedi
[`../metodo/profile-framing.md`](../metodo/profile-framing.md) e
[`../metodo/analisi-istituzionale.md`](../metodo/analisi-istituzionale.md).
