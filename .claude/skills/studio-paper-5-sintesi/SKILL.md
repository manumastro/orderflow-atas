---
name: studio-paper-5-sintesi
description: Fase 5 dello studio dei paper - la sintesi. Mette insieme le analisi verificate di una ricerca - finding convergenti e in conflitto, cosa e' rifacibile su NQ - aggiorna l'indice dei paper e, se chiesto, scrive o aggiorna la scheda dell'idea in quant/studi/<idea>/1-idea.md.
argument-hint: "<file della ricerca> [idea:<studio>]"
---

# Fase 5: La Sintesi

Il passo 6 di [`quant/metodo/1-come-si-cercano-le-idee.md`](../../../quant/metodo/1-come-si-cercano-le-idee.md).
Leggi tutte le analisi verificate della ricerca, per intero.

## 1. La Sintesi Nel File Della Ricerca

Aggiungi la sezione **La Sintesi**:

- **i finding**, uno per paper, con la sigla e la sezione;
- **dove convergono** e **dove si contraddicono** (stesso effetto con segni diversi, periodi diversi,
  strumenti diversi): la contraddizione si scrive com'e';
- **il perche' comune**, se c'e';
- **cosa e' rifacibile su NQ con i dati di ATAS**, e cosa richiederebbe dati che non abbiamo;
- **le idee candidate**: per ciascuna, una riga con il finding da provare e cosa la ucciderebbe.

## 2. L'Indice

`quant/paper/indice-dei-paper.md`: la sezione del filone con le righe aggiornate (sigla, link
all'analisi, autore e versione, ruolo, stato) e, sotto la tabella, la sintesi del filone in poche
righe con il link al file della ricerca.

## 3. La Scheda Dell'Idea, Solo Se Chiesta

Se l'orchestratore ti passa `idea:<studio>`: scrivi o aggiorna `quant/studi/<studio>/1-idea.md` con
il modello di [`quant/metodo/1-come-si-scrive-un-idea.md`](../../../quant/metodo/1-come-si-scrive-un-idea.md)
(aprilo): paper, finding, perche', cosa la ucciderebbe, varianti dichiarate prima di provarle. Il
paper non si copia: le regole si scrivono dopo. **Non scrivere codice e non lanciare backtest**: la
fase 1 del metodo finisce con la scheda.

Senza `idea:`, proponi nelle ultime righe della sintesi quale studio aprire, e non aprirlo.

Rispondi con: i finding in cinque righe, le idee candidate, cosa hai scritto e dove.
