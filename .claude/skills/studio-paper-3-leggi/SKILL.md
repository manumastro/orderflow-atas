---
name: studio-paper-3-leggi
description: Fase 3 dello studio dei paper - leggere un paper per intero e scriverne analisi.md (la scheda bibliografica e la lettura - domanda, metodo, risultati con la sezione, il perche', i limiti, cosa si puo' rifare su NQ). Un paper per agente.
argument-hint: "<percorso del PDF>"
---

# Fase 3: Leggere Un Paper Per Intero

I passi 4 e 5 di [`quant/metodo/1-come-si-cercano-le-idee.md`](../../../quant/metodo/1-come-si-cercano-le-idee.md).
Un esempio di analisi finita da imitare nella forma:
[`quant/paper/opzioni-gamma/baltussen-2021-hedging-demand-and-market-intraday-momentum/analisi.md`](../../../quant/paper/opzioni-gamma/baltussen-2021-hedging-demand-and-market-intraday-momentum/analisi.md).

## 1. Estrai E Leggi Tutto

```bash
pdftotext -layout <pdf> <scratchpad>/<chiave>.txt
```

Il testo va nella scratchpad, non nel repository. **Leggilo tutto**, appendici comprese: un paper di
40 pagine si legge in piu' blocchi, e nessun blocco si salta. Le formule escono storpiate e le tabelle
a piu' colonne si mescolano: **ogni numero e ogni formula che entra nell'analisi si ricontrolla sul
PDF** (lo strumento Read legge il PDF a pagine, con `pages`).

## 2. Cosa Estrai

- **la domanda** e **il metodo** (dati, strumenti, periodo, frequenza, campione fuori, test);
- **il finding**, con i numeri, la tabella o la sezione da cui viene (`[SIGLA §3.2]`, `[SIGLA Tab. 4]`);
- **il perche'**: il meccanismo che propongono gli autori, e le alternative che scartano;
- **cosa lo ucciderebbe**: se lo dicono loro, o cosa si deduce dal perche';
- **i limiti dichiarati e quelli non dichiarati**: dati simulati o reali, costi, look-ahead, numero di
  varianti provate, sopravvivenza, campione fuori;
- **cosa si puo' rifare qui**, su NQ con i dati di ATAS (CLAUDE.md, decisione 2), e cosa no.

## 3. Scrivi `analisi.md`

Nella cartella del paper. In testa, fra due righe `---`, la scheda bibliografica con i campi
dell'esempio: chiave, sigla, titolo, autori, versione_letta, fonte, identificativo, url, tipo,
revisione_fra_pari, pagine, pdf, pdf_sha256, filone, ruolo (`idea` o `metodo`), stato: `letto`,
letto_il, studi, codice_degli_autori. Poi la lettura: **I Termini** (ogni termine tecnico spiegato),
**La Domanda**, **Il Metodo**, **I Risultati** (con la fonte di ogni numero), **Il Perche'**,
**Cosa Non Regge O Non Si Dice**, **Cosa Ne Prende Il Framework**.

Italiano, apostrofi ASCII. Un numero che non hai letto nel paper non si scrive. Aggiungi una riga
datata nel Diario del file della ricerca.

Rispondi con: il percorso dell'analisi, il finding in due righe, i dubbi di lettura (tabelle
ricostruite, formule incerte) che il verificatore deve guardare.
