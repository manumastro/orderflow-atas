# Come Si Scrive Un'Idea

Ogni idea ha il suo studio, `quant/studi/<nome>/`, e la sua scheda e' `1-idea.md`. La scheda viene
**prima** del codice: e' il primo passo della pipeline ([`il-metodo-quantitativo.md`](../il-metodo-quantitativo.md)).

Il paper non si copia: se ne prende il finding e il perche' `[MC · 26:27 - 27:22]`. Il perche' e'
la parte che conta, perche' dice anche **cosa ucciderebbe la strategia** `[MC · 46:23 - 47:23]`.

## Il Modello

La cartella dello studio si chiama come la strategia nel catalogo (`studi/momento-intraday/`). Sei
sezioni:

```markdown
# <Nome Leggibile>

**Stato**: idea | regole scritte | codificata | validata dentro | fuori consumato (esito)
**Strategia nel catalogo**: `<nome>`, versione N

## Il Paper
I paper da cui viene, per chiave, ciascuno con il link alla sua analisi in
`quant/paper/<filone>/<chiave>/analisi.md`. Se non e' ancora stato letto per intero, lo si dice.

## Il Finding
Cosa hanno misurato, su quale strumento, periodo e scala, con i numeri del paper.

## Il Perche'
Il meccanismo: chi fa cosa, e perche' il prezzo non si adegua subito.

## Cosa La Ucciderebbe
La condizione strutturale che, se cambiasse, farebbe sparire l'effetto.

## Le Regole Da Provare
I tre riquadri: ingresso, uscita, dimensione. Le varianti da provare, dichiarate prima di provarle.

## Le Prove
Una riga per validazione: data, impronta, esito dei cancelli, link al rapporto in 5-validazione/risultati/ dello studio.
Si scrivono anche, e soprattutto, quelle andate male.
```

## Le Regole Della Scheda

- **Il finding si cita dal paper, non dal transcript**, appena il paper e' stato letto. Finche' non
  lo e', si scrive "dal transcript, paper non ancora letto".
- **Le varianti si dichiarano prima di provarle.** Ogni variante provata finisce nel registro e
  abbassa lo Sharpe deflazionato di tutte: provarne venti a caso costa.
- **Un'idea che non passa i cancelli resta qui**, con il suo esito. Non si cancella: e' evidenza.
