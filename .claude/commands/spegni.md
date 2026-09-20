---
description: Spegne il contesto vivo e ogni residuo della vecchia sorveglianza, e lo verifica
---

> **Dal 20 settembre 2026 i livelli non hanno piu' un processo da spegnere.** Li calcola
> l'indicatore a ogni barra, e restano sul chart finche' l'indicatore c'e': per toglierli si
> cancellano le regole, non si uccide niente.
>
> ```bash
> python3 FabioOrderFlow/tools/bridge.py rules --chart STRUMENTO --clear
> ```
>
> Quello che segue serve ancora, perche' **un processo della fase chiusa rimasto vivo da una
> sessione precedente continua a scrivere su `/levels`, e un POST li' spegne le regole**: il
> chart smette di aggiornarsi e nessuno vede perche'.


**Spegni tutto, adesso, e dimostra che e' spento.** Nessuna conferma: l'utente l'ha gia' data
scrivendo il comando.

1. **Ferma ogni task in esecuzione.** Chiama `TaskStop` su **ognuno** dei processi in background
   ancora vivi in questa sessione — `livelli_vivi.py --ogni` in primo luogo, e qualunque residuo dei
   sorveglianti della fase chiusa (`scenari.py`, `sveglia_tape.py`, `sveglia_movimento.py`,
   `permesso_di_fatto.py`). Se non sei sicuro di quali siano attivi, guarda i `task-id` delle
   notifiche recenti: **uno dimenticato continua a scrivere sul chart**.

2. **Uccidi gli orfani.** Un task puo' morire lasciando vivo il processo python che alimentava, e
   un `livelli_vivi.py` orfano continua a riscrivere i livelli mentre l'agente crede di averlo
   spento — cioe' il chart si muove e nessuno sa chi lo muove.

   ```bash
   bash FabioOrderFlow/tools/spegni_sorveglianti.sh
   ```

3. **Verifica, non dedurre.** Controlla che non resti nessun processo:

   ```bash
   ps aux | grep -E "livelli_vivi|sveglia_|scenari\.py" | grep -v grep
   ```

   Nessuna riga = spento. Se ne resta una, **dillo** invece di dichiarare il lavoro finito.

4. **Decidi cosa lasciare sul chart, e dichiaralo.** Spegnere il processo **non cancella** livelli
   e pannello: restano l'ultima fotografia, e da quel momento **invecchiano in silenzio** — un
   livello vivo fermo si disegna esattamente come uno aggiornato.

   Se la sessione e' finita, puliscili:

   ```bash
   python3 FabioOrderFlow/tools/bridge.py levels --chart STRUMENTO --clear
   python3 FabioOrderFlow/tools/bridge.py watch  --chart STRUMENTO --clear
   ```

   Se invece restano apposta, **scrivilo nella risposta**, con l'ora dell'ultimo aggiornamento.
