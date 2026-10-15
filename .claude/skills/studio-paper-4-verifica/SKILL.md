---
name: studio-paper-4-verifica
description: Fase 4 dello studio dei paper - la verifica avversaria di un'analisi. Controlla ogni numero, citazione e finding di analisi.md contro il PDF, corregge gli errori, e segna l'analisi come verificata. Un paper per agente, mai lo stesso che l'ha scritta.
argument-hint: "<percorso di analisi.md>"
---

# Fase 4: La Verifica Avversaria

Il tuo compito e' **trovare gli errori** dell'analisi, non confermarla. Parti dal presupposto che ce
ne siano: un numero letto dalla colonna sbagliata di una tabella mescolata da `pdftotext`, un segno
invertito, un finding attribuito alla sezione sbagliata, un limite che gli autori dichiarano e
l'analisi tace, un'affermazione che il paper non fa.

## Come

1. Estrai il testo del PDF nella scratchpad (`pdftotext -layout`) e tieni aperto il PDF (Read con
   `pages`) per le tabelle e le formule.
2. **Fai l'elenco di ogni affermazione verificabile** dell'analisi: ogni numero, ogni citazione
   `[SIGLA §n]`, ogni finding, ogni "gli autori dicono". Per ciascuna cerca il punto del paper.
3. Per ciascuna: **confermata** (con la pagina), **sbagliata** (cosa dice davvero il paper),
   **non trovata** (il paper non lo dice da nessuna parte).
4. Controlla anche **cosa manca**: il limite principale, il campione fuori, i costi, il numero di
   varianti. E che la scheda bibliografica sia giusta (sha256 compreso:
   `sha256sum <pdf>`).

## Cosa Consegni

- **Correggi `analisi.md`** dove sbaglia o dice cose non scritte nel paper; non riscrivere lo stile.
- In fondo all'analisi, una sezione **La Verifica** con la data, quante affermazioni hai controllato,
  quante confermate, corrette e tolte, e l'elenco delle correzioni.
- Nella scheda bibliografica, `stato: analizzato` e `verificato_il: <data>`.
- Una riga datata nel Diario del file della ricerca, e lo stato `analizzato` nell'indice.

Rispondi con: affermazioni controllate, corrette, tolte, e la piu' grave.
