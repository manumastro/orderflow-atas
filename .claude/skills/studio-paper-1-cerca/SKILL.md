---
name: studio-paper-1-cerca
description: Fase 1 dello studio dei paper - cercare e verificare. Trova i paper su una domanda (SSRN prima, poi arXiv q-fin, NBER, RePEc, pagine degli autori), ne verifica l'esistenza, sceglie quelli da leggere e scrive il file della ricerca e le righe "trovato" dell'indice.
argument-hint: "<domanda> [filone] [quanti] [file della ricerca]"
---

# Fase 1: Cercare E Verificare

I passi 1 e 2 di [`quant/metodo/1-come-si-cercano-le-idee.md`](../../../quant/metodo/1-come-si-cercano-le-idee.md):
aprilo e seguilo. Qui c'e' solo cosa consegni.

## Come Cerchi

- **Con il titolo esatto** fra virgolette se l'utente lo da'; altrimenti con le parole del finding,
  in inglese, in piu' formulazioni (almeno quattro ricerche diverse).
- **Dove**: SSRN prima di tutto `[MC · 25:42 - 25:49]`; poi arXiv q-fin, NBER, RePEc/IDEAS, le pagine
  degli autori e delle universita', dove spesso c'e' il PDF libero.
- **Prima di cercare, leggi l'indice** (`quant/paper/indice-dei-paper.md`): un paper gia' presente non
  si ricerca, si cita.

## Cosa Verifichi Per Ogni Candidato

Titolo, autori, affiliazione, anno, identificativo (SSRN `abstract_id`, DOI, arXiv id), tipo
(preprint o rivista), revisione fra pari, **dove c'e' un PDF libero** (url diretto) e se la pagina e'
dietro Cloudflare (SSRN, ResearchGate). Un candidato che non trovi alla fonte non entra.

## Come Scegli

Al massimo `quanti` paper (default 3), con questi criteri, in ordine: risponde alla domanda con un
finding misurato; dati reali e periodo lungo; revisionato o di autori con lavori revisionati sul tema;
il meccanismo (il perche') e' dichiarato; e' rifacibile su NQ con i dati di ATAS. Scrivi perche' hai
scelto e perche' hai scartato.

## Cosa Consegni

1. **Il file della ricerca**, `quant/paper/ricerche/<data>-<tema>.md`:

   ```markdown
   # Ricerca: <domanda> · <data>
   **Filone**: <filone> · **Chiesta da**: utente / studio <nome> · **Ricerche fatte**: le query, una per riga
   ## I Candidati
   | chiave | titolo | autori, anno | id | tipo | pdf libero | perche' si', perche' no | stato |
   ## La Scelta
   Le chiavi scelte, una per riga, con il motivo.
   ## Il Diario
   Le fasi successive aggiungono qui una riga datata ciascuna.
   ```

   La **chiave** e' `cognome-anno-titolo-breve`, minuscolo, con i trattini (regole in testa
   all'indice). Lo **stato** parte da `trovato`.
2. **L'indice**: nella sezione del filone (creala se manca, con una riga che dice la domanda), una
   riga `trovato` per ogni candidato scelto, con il link alla fonte.

Rispondi all'orchestratore con: il file, le chiavi scelte, quali hanno un PDF libero e quali
richiederanno il browser.
