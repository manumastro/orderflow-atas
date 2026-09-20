---
paths:
  - "docs/research/giornate/regole-dei-livelli-*.json"
  - "FabioOrderFlow/src/Observation/RegoleDeiLivelli.cs"
---

# Scrivere Il File Delle Regole

Tipi, finestre, etichette e difese in
`docs/research/metodo/i-livelli-li-calcola-l-indicatore.md`.

```bash
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6 --file <questo file>
```

- **Il file contiene regole, non prezzi.** `fisso` e' l'eccezione e deve giustificarsi: e'
  l'unico tipo che non si accorge del giorno, e quindi l'unico che puo' invecchiare in silenzio.
- **Ogni `label` dice a cosa serve arrivare a quel livello** — permesso, bersaglio, invalidazione
  o posizionamento. Un livello di cui non si sa dire a cosa serve non va messo nel file.
- **L'ordine nel file e' la priorita':** quando due livelli cadono a meno di otto punti, vince il
  primo dichiarato. Una mensola messa prima del POC lo cancella dal chart.
- **`minimo_lotti` non e' prudenza:** senza, nei primi minuti di cash il POC dichiara "89% del
  volume" perche' non c'e' altro, e non e' una misura prematura, e' una misura falsa.
- **Bersagli, invalidazioni e pareggi si dichiarano per NOME di livello**, mai come prezzo.
- **`pareggio_livello` va messo su ogni scenario**, ed e' il prezzo dove il lato opposto torna a
  vincere — non una distanza, non "dopo 1R". Nel live la gestione e' l'edge e questa ne e' la
  regola centrale; **non ha lo stacco minimo** dell'invalidazione, perche' il break even e' vicino
  apposta. Quando manca, il pannello scrive `pareggio NON DICHIARATO`: e' un promemoria, non un
  guasto.
- **Leggi sempre cosa il deposito dice di non aver disegnato** (`skipped`, e la sezione 6ter del
  giro): e' contesto, non diagnostica. **Il pannello non lo mostra piu'** — era ingombro sul
  chart — quindi se non lo leggi da li' non lo vede nessuno. Il caso piu' frequente e' due livelli
  a meno di otto punti, e li' il fatto che conta e' proprio quello: due letture indipendenti
  indicano lo stesso posto.
- **Non lanciare `livelli_vivi.py`**: e' ritirato, deposita su `/levels`, e un POST li' spegne le
  regole.
