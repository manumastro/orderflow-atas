---
description: Spegne tutti i sorveglianti del tape e lo verifica
---

**Spegni tutta la sorveglianza, adesso, e dimostra che e' spenta.**

Esegui questi passi **nell'ordine**, senza chiedere conferma: l'utente l'ha gia' data scrivendo
il comando.

1. **Ferma ogni task in esecuzione.** Chiama `TaskStop` su **ognuno** dei monitor e dei comandi in
   background ancora vivi in questa sessione — scenari, sveglia del tape, sveglia del movimento,
   permesso di fatto, e qualunque altro. Se non sei sicuro di quali siano attivi, guarda i
   `task-id` delle notifiche recenti: **uno dimenticato continua a suonare**.

2. **Uccidi gli orfani.** Poi lancia:

   ```bash
   bash FabioOrderFlow/tools/spegni_sorveglianti.sh
   ```

   Un monitor puo' morire lasciando vivo il processo python che alimentava. Uno `sveglia_tape.py`
   orfano continua a interrogare il bridge e a far suonare notifiche che nessuno legge, e al
   riarmo successivo gli avvisi arrivano doppi. Lo script stampa cosa trova, lo uccide e
   **ricontrolla**: se esce con 1, qualcosa e' ancora vivo e va detto, non nascosto.

3. **Aggiorna il diario.** Nel file di oggi in `docs/research/giornate/`, riscrivi la sezione
   **"Dove eravamo"** con lo stato al momento dello spegnimento: prezzo, cosa stava facendo il
   mercato, cosa era scattato, ed esplicitamente che **i sorveglianti sono spenti e da che ora**.
   Senza questa riga, il prossimo giro d'orizzonte dira' che erano accesi e nessuno se ne
   accorgera'.

4. **Riporta in tre righe**: quali task hai fermato, cosa ha risposto lo script, e lo stato del
   mercato all'ultimo dato. **Se qualcosa non si e' spento, quella e' la prima riga**, non
   l'ultima.

**Non riaccendere niente** e non proporre di farlo. Se l'utente vuole riarmare, lo dira'.
