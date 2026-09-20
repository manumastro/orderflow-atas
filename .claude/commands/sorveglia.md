---
description: Un giro di controllo sulla mappa: se i livelli statici sono scaduti, sveglia il sottoagente
---

**Un giro solo, e quasi sempre finisce in una riga.** Questo comando e' pensato per girare
**spesso** dentro un `/loop`: il costo di un giro deve restare quello di due chiamate al bridge,
non quello di una analisi.

```bash
python3 FabioOrderFlow/tools/serve_rifare.py \
        docs/research/giornate/livelli-vivi-STRUMENTO-AAAA-MM-GG.json --chart STRUMENTO
```

## Se risponde `NIENTE`

**Fermati qui.** Scrivi una riga sola — ora italiana, prezzo, `la mappa regge` — e non fare altro:
niente giro d'orizzonte, niente lettura, niente sottoagenti. Il valore di questo ciclo sta nel
costare poco quando non c'e' niente da fare, e un giro che "già che c'è" guarda anche il tape lo
trasforma in un analista che gira ogni minuto.

## Se risponde `INCERTO`

Il bridge non ha risposto. **Dillo e basta.** Non dedurre che la mappa regga: non e' stata
controllata.

## Se risponde `SERVE`

Allora, e solo allora, sveglia il sottoagente **in background**:

```
Agent(subagent_type: "livelli-statici", description: "rifai i livelli statici",
      prompt: "<le ragioni esatte stampate da serve_rifare.py>
               File delle regole: <percorso>. Chart: <strumento>.")
```

**Le ragioni si passano verbatim**, non riassunte: sono gia' scritte per essere lette da chi
arriva senza contesto, e riscriverle a memoria e' il modo in cui si perde il motivo.

**Uno per volta.** Se un `livelli-statici` e' gia' in corso da un giro precedente, non lanciarne
un altro: aspetta che finisca. Due agenti che riscrivono lo stesso file si sovrascrivono a vicenda,
e il secondo non se ne accorge.

Poi riferisci in una riga che l'hai svegliato e perche'. **Non aspettarlo**: lavora in background
e il suo esito arriva da solo.

## Perche' il filtro e l'agente sono separati

Accorgersi che la mappa e' scaduta costa due chiamate al bridge; rifarla costa leggere tape,
footprint e profilo. Se le due cose stessero insieme, ogni giro pagherebbe il prezzo della seconda
per ottenere quasi sempre la risposta della prima.

**E il filtro non giudica il mercato.** Le sue cinque ragioni — fuori fascia, attraversato, niente
in gioco, sessione, deriva — misurano la **scadenza della mappa**, non cosa il prezzo stia facendo.
Non e' una condizione armata e non torna ad esserlo: non dice mai *compra*, dice *quella riga sul
chart non descrive piu' dove siamo*.
