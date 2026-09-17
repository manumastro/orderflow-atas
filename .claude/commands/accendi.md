---
description: Riaccende tutti i sorveglianti, tarati sulla sessione in corso
---

**Riaccendi la sorveglianza, adesso.** Non chiedere conferma: l'utente l'ha gia' data scrivendo
il comando.

1. **Chiedi allo strumento i comandi giusti**, invece di scriverli a memoria:

   ```bash
   python3 FabioOrderFlow/tools/comandi_sorveglianti.py --chart NQZ6
   ```

   Stampa le righe gia' tarate sulla sessione in corso, con `--from` all'ora attuale, e dice **chi
   sta gia' girando**. Le soglie non si inventano: `--strappo 8` e' un movimento a Londra e rumore
   su New York, dove il libro e' sei volte piu' spesso.

2. **Se dice che qualcosa gira gia', fermati.** Accendere sopra a un sorvegliante vivo fa arrivare
   gli avvisi doppi e non si sa piu' quale dei due dica la verita' su quale barra. Prima `/spegni`,
   poi si riaccende.

3. **Apri un `Monitor` per ciascuna delle prime tre righe** — scenari, sveglia del tape, sveglia
   del movimento — con `timeout_ms: 1800000`. **Non comandi in background**: un comando in
   background scrive su un file e non sveglia l'agente.

   I filtri `grep`/`awk` sono quelli documentati in
   [`sorveglianza-del-tape.md`](../../docs/research/metodo/sorveglianza-del-tape.md), sezione
   *"Come Si Tengono Accesi"*. Nel filtro della sveglia del tape **deve esserci `PRESIDIO`**, non
   solo `PRESIDIO FINE`: il 17 settembre quella omissione ha reso l'agente cieco esattamente
   mentre il prezzo stava sul livello.

4. **Il quarto, `permesso_di_fatto.py`, solo se lo strumento lo stampa** — cioe' dopo le 14:30Z,
   quando l'IVB di oggi e' chiusa. Prima non e' calcolabile, e inventarne i bordi produce un
   permesso che sembra misurato e non lo e'. Se va acceso, **misura i bordi dell'IVB
   13:30-14:30Z** dal bridge e sostituiscili a `--alto` e `--basso`.

5. **Aggiorna il diario.** Nel file di oggi in `docs/research/giornate/`, aggiungi **in fondo** una
   nuova sezione *"Dove eravamo"* con l'ora di riaccensione, la taratura usata e lo stato del
   mercato. **In fondo e non in cima:** `giro_orizzonte.py` prende l'**ultima** sezione che
   combacia, quindi una nuova scritta in cima non verrebbe mai letta.

6. **Riporta in tre righe**: quali sorveglianti hai acceso, con quale taratura, e cosa e'
   successo sul mercato mentre erano spenti — quello e' il buco che chi opera non ha visto.
