---
description: Mette sul chart le regole dei livelli, che da li' in poi l'indicatore ricalcola da solo
---

**Deposita le regole, adesso, senza chiedere conferma:** l'utente l'ha gia' data scrivendo il
comando.

> **Non c'e' piu' niente da "accendere", e il nome del comando e' rimasto per abitudine.** Dal
> 20 settembre 2026 **i livelli li calcola l'indicatore**, a ogni barra, dalle regole che si
> depositano una volta: nessun processo, nessun `--ogni`, niente in background. Procedura e
> vocabolario in
> [`i-livelli-li-calcola-l-indicatore.md`](../../docs/research/metodo/i-livelli-li-calcola-l-indicatore.md).
>
> **Non far ripartire** `livelli_vivi.py`, `scenari.py`, `sveglia_tape.py` o
> `sveglia_movimento.py`: sono tutti fasi chiuse. `livelli_vivi.py` in particolare deposita su
> `/levels`, e **un POST li' spegne le regole**.

Nell'ordine:

1. **Verifica che il bridge risponda** e prendi lo strumento del chart:

   ```bash
   python3 FabioOrderFlow/tools/bridge.py health
   ```

   Se non risponde, fermati e dillo: un contesto senza dati non e' un contesto.

2. **Scegli il file delle regole**, che e' quello di **oggi** per lo strumento in uso:

   ```text
   docs/research/giornate/regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json
   ```

   Se per oggi non esiste, **non riusare quello di ieri**: le finestre e i prezzi dichiarati sono
   di un altro giorno e si disegnano identici a quelli di adesso. Dillo, e scrivine uno.

3. **Deposita.** Il POST sostituisce tutto e ricalcola subito; da li' in poi ricalcola
   l'indicatore, a ogni barra.

   ```bash
   python3 FabioOrderFlow/tools/bridge.py rules --chart STRUMENTO --file docs/research/giornate/regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json
   ```

4. **Leggi cosa NON e' uscito.** Il comando stampa le regole che non hanno prodotto un livello:
   finestra ancora vuota, troppo pochi lotti, due livelli che coincidono. **Non e' diagnostica, e'
   contesto**: "POC cash: finestra ancora vuota" dice che la cash non e' aperta. Senza leggerlo, si
   nota soltanto che una riga attesa non c'e'.

5. **Verifica che siano davvero sul chart**, invece di fidarti della risposta:

   ```bash
   python3 FabioOrderFlow/tools/bridge.py levels --chart STRUMENTO
   ```

   **Il pannello non si accende e non si verifica cosi': e' l'indicatore.** Se manca, manca
   l'indicatore sul chart, oppure `Show watch panel` e' spento nelle sue impostazioni.

6. **Riferisci in una riga**: quanti livelli ci sono, quante regole non hanno prodotto niente e
   perche', e l'ora di mercato **in italiano**.
