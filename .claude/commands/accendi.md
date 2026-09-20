---
description: Accende il contesto vivo sul chart: livelli dinamici e pannello, sempre aggiornati
---

**Accendi il contesto vivo, adesso, senza chiedere conferma:** l'utente l'ha gia' data scrivendo
il comando.

> **Cosa NON fa piu' questo comando.** Fino al 19 settembre 2026 accendeva i tre sorveglianti a
> condizioni armate. Quella e' una **fase chiusa**: vedi
> [`il-contesto-vivo.md`](../../docs/research/metodo/il-contesto-vivo.md). Non far ripartire
> `scenari.py`, `sveglia_tape.py` o `sveglia_movimento.py`.

Nell'ordine:

1. **Verifica che il bridge risponda** e prendi lo strumento del chart:

   ```bash
   python3 FabioOrderFlow/tools/bridge.py health
   ```

   Se non risponde, fermati e dillo: un contesto senza dati non e' un contesto.

2. **Controlla che non ne giri gia' uno.** Due processi sullo stesso chart si sovrascrivono il
   pannello a vicenda e il risultato lampeggia. Se ce n'e' uno vivo in questa sessione, usalo; se
   e' orfano, spegnilo con `/spegni` prima di riaccendere.

3. **Scegli il file delle regole**, che e' quello di **oggi** per lo strumento in uso:

   ```text
   docs/research/giornate/livelli-vivi-STRUMENTO-AAAA-MM-GG.json
   ```

   Se per oggi non esiste, **non riusare quello di ieri**: i livelli fissi che contiene sono di un
   altro giorno e si disegnano identici a quelli di adesso. Dillo, e scrivine uno.

4. **Avvia il processo in background** — non come `Monitor`: non deve interrompere niente, deve
   solo esserci.

   ```bash
   python3 FabioOrderFlow/tools/livelli_vivi.py \
           docs/research/giornate/livelli-vivi-STRUMENTO-AAAA-MM-GG.json --chart STRUMENTO --ogni 30
   ```

5. **Verifica che sia davvero sul chart**, invece di fidarti dell'avvio:

   ```bash
   python3 FabioOrderFlow/tools/bridge.py watch --chart STRUMENTO
   python3 FabioOrderFlow/tools/bridge.py levels --chart STRUMENTO
   ```

   I livelli devono avere un conteggio. Se e' zero, il processo e' partito e non sta scrivendo:
   e' il guasto che non grida, perche' sul chart resta quello di prima.

   **Il pannello non si accende e non si verifica cosi': e' l'indicatore.** Se manca, manca
   l'indicatore sul chart, oppure `Show watch panel` e' spento nelle sue impostazioni.

6. **Riferisci in una riga**: quanti livelli ci sono e l'ora di mercato **in italiano**.
