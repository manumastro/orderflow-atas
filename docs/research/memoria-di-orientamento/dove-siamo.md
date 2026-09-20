---
name: dove-siamo
description: Lo stato del lavoro a settembre 2026: cosa e' attivo, su cosa si opera, cosa e' aperto
metadata:
  type: project
---

**Aggiornato al 20 settembre 2026.** Se la data e' vecchia di settimane, verifica prima di
fidarti: questo file orienta, non comanda. Le regole stanno in `CLAUDE.md`.

## Dove Si E' Arrivati

Il progetto e' nella fase di **lettura discrezionale dal vivo**, non di ricerca sistematica. Si
legge il mercato seduta per seduta col metodo del live Q1 di Fabio, si scrive un file di giornata
**durante** la seduta, e si misura a posteriori cosa era giusto. La fase di backtest sistematico
e' chiusa da metà settembre. Vedi [[la-storia-del-progetto]].

## Cosa E' Fermo E Non Si Rimette In Discussione

- **Lo strumento e' NQ, e solo NQ** (deciso il 18 settembre). Oro, crude ed ES sono stati provati
  fra il 16 e il 18 settembre: quei file restano come evidenza e non si estendono.
- **Il corso di Fabio comanda.** Il dossier Triple AAA e' citabile ma non comanda — descrive lo
  stesso setup dentro un'impalcatura con un gate orario che nel live non esiste.
- **La sessione e' New York**, cash 13:30-20:00Z.

## Cosa E' Vivo Adesso

- **Le trascrizioni del corso sono 6 su circa 20.** L'utente sta studiando le altre e le
  incollera' in `fabio_course/fabio_q1/`. I documenti di metodo sono scritti perche' una lezione
  nuova possa **contraddire una riga e correggerla sul posto**: ogni riga cita il minuto da cui
  viene.
- **Si sta facendo pratica in replay**, non solo dal vivo. L'ultimo e' il replay del 14 settembre
  su NQZ6, con la disciplina di non leggere il diario della giornata vera prima di aver dato la
  lettura.
- **Il chart si aggiorna da solo**: `livelli_vivi.py` ridisegna POC, bordi del valore ed estremi a
  ogni giro, e `condizione_viva.py` scrive sul pannello in alto a destra i contatori di una
  condizione dichiarata, cosi' l'utente li vede senza aspettare una risposta scritta.
- **Il lavoro si sta spostando su un PC Windows**, perche' l'esecuzione passa da Tradeify che
  richiede R|Trader Pro, che non esiste su macOS. Il trasloco e' cominciato il 20 settembre e non
  e' ancora verificato del tutto: la notifica di sistema di `avviso.py` su Windows e' scritta ma
  mai eseguita.

## Cosa Resta Aperto

- Il calendario macro non ha una fonte nel repository: si dice *"il minuto 12:30Z e' il piu'
  pesante"*, mai il nome di un evento.
- Non esiste un modello attivo e nessuna regola operativa e' approvata. **Descrivere non e'
  validare**, e vale anche per il metodo del live.
