---
paths:
  - "docs/research/giornate/**"
---

# Il File Della Giornata

Si carica solo quando si tocca una giornata. Procedura completa in
`docs/research/giornate/come-si-scrive-una-giornata.md`.

- **Si aggiorna durante la seduta, non a posteriori.** Una giornata scritta alla fine perde
  proprio le letture sbagliate, che sono la parte verificabile.
- **Le correzioni restano scritte**, con cosa le ha causate. Descrivere non e' validare, e un
  risultato negativo si registra con la stessa cura di uno positivo.
- **Il quadro sta fra `<!-- QUADRO -->` e `<!-- /QUADRO -->`**: e' quello che il giro d'orizzonte
  stampa alla sezione 4bis a ogni messaggio.
- **Ogni soglia si dichiara accanto al numero che produce**, mai lasciata implicita.
- **Nessun id di chart nei file**: cambia a ogni ricarica dell'indicatore.
- **In replay il diario torna indietro da solo** (`riavvolgi_giornata.py`, nell'hook del giro).
  Sposta solo i blocchi che **dichiarano in apertura** un orario avanti all'orologio; quelli che
  si limitano a *citarne* uno restano e vengono segnalati — li' il problema e' una misura presa su
  una finestra che finisce nel futuro, e si corregge rifacendo la misura.
- **Una sezione che ha perso il suo blocco datato va riscritta**: quello che resta sotto il titolo
  descrive un momento che non c'e' piu'.
