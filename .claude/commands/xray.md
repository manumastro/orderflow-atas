---
description: I dati di Options X-Ray (livelli chiave, gamma per strike, GEX dei market maker) letti adesso
---

X-Ray non si legge a occhio: i suoi numeri stanno in `quant/dati/xray/`, pubblicati dal lettore
`XrayLive` ogni 15 secondi. **Non serve ATAS per leggerli, serve per aggiornarli**, e solo con gli
indicatori X-Ray aperti su un chart ES. Cosa sono e come si leggono:
`quant/metodo/x-ray-in-vivo.md` (leggilo prima, se non l'hai letto in questa sessione).

0. **Prima la pagina, come la vede l'utente** (2 novembre 2026): `python quant/strumenti/xray/scripts/leggi_pagina.py`
   (con `--foto <scratchpad>/foto-pagina` se la forma conta), e ogni numero si cita con la sua scheda.
   L'ordine e le trappole: `quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md`. La lettura si pubblica
   anche nella pagina, scheda Lettura: `python quant/strumenti/xray/scripts/scrivi_lettura.py <lettura.json>`.
1. **Il punto, in un comando**:

   ```bash
   python quant/strumenti/xray/scripts/xray_query.py riassunto          # in parole
   python quant/strumenti/xray/scripts/xray_query.py riassunto --json   # tutto, per un calcolo
   ```

   Dice se il lettore e' vivo, lo spot SPX, la gamma allo spot, zeri e picchi (adesso e previsti),
   la GEX netta dei market maker per scadenza, il delta accumulato oggi per gruppo (un flusso, non
   una posizione), la pressione 0DTE e quanti minuti ha ogni famiglia. Per un numero che il riassunto
   non da' (la base ES-SPX, lo SPX implicito fuori cassa, una heat map, una giornata intera) c'e'
   l'API della pagina: `curl -s http://127.0.0.1:8790/api/adesso` (le rotte in
   `quant/strumenti/xray/x-ray-lettore-e-dashboard.md`, "L'API").
2. **Se il lettore e' FERMO**: `powershell -File quant/strumenti/xray/scripts/xray_live_task.ps1 -Status`,
   e **dillo all'utente**: si accende con `-Start` (o `-Install`, poi `-Start`, se il task non c'e') solo se
   lo chiede lui, perche' X-Ray non parte da solo (6 novembre 2026). Se una famiglia e' ferma, il suo indicatore non e'
   aperto su ES: dillo all'utente, non leggerla come dato di adesso.
3. **Le altre fonti, a confronto** (dal 2 novembre 2026; ognuna dice da dove viene): VIX e VXN
   ricostruiti dalla catena CME di ATAS, la volatilita' di X-Ray, QQQ e NDX dal Cboe pubblico, VIX e VXN
   ufficiali, la GEX di NQ da tre fonti. Il riassunto ne stampa la riga; il resto e' nell'API
   (`/api/vol/serie`, `/api/vol/termine`, `/api/vol/sorriso?indice=SPX|NDX`, `/api/nq/gex`,
   `/api/es/gex-confronto`, `/api/fonti`), descritta in `x-ray-lettore-e-dashboard.md`, "Le Fonti A Confronto".
   Quando confronti le fonti, di' se **convergono o divergono**, con i numeri e l'eta' di ciascuna.
4. **Per un dettaglio**: `xray_query.py levels`, `at 14:35 --day AAAA-MM-GG` (il profilo a un'ora UTC),
   `coverage`, `csv SPX observed gamma AAAA-MM-GG`.

Nella risposta:
- **prima la persona**: cosa dicono, in parole semplici (freno o acceleratore, dove cambia, chi e'
  lungo e chi e' corto);
- poi i numeri, con l'**ora di New York** e l'**eta'** di ognuno;
- ogni volta la riserva: e' la **stima di OptionsDepth su SPX**, con un metodo non pubblicato. Su ES
  vale per lo stesso indice, a meno della base ES-SPX. Su NQ e' solo contesto. Nessun livello e'
  provato;
- **su ES prevale X-Ray ed e' l'unica GEX che si mostra** (5 novembre 2026): i livelli in prezzi di ES
  (`xray_query.livelli_es()`, o la riga "in prezzi di ES" del giro), con la base dichiarata; la nostra
  GEX di ESZ6 non si legge e non si confronta;
- niente segnali (niente LONG, SHORT, ingressi, stop): e' una lettura dal vivo.
