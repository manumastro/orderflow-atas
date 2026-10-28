---
name: studio-globale-5-quadro
description: Fase 5 dello studio globale (NQ o ES) - il quadro. E' l'unica fase che confronta le altre - macro, COT, gamma, order flow - dal piu' lontano al piu' vicino, con i livelli confermati da piu' fasi e le contraddizioni; scrive 5-quadro.md e il blocco QUADRO nel file della giornata, che il giro d'orizzonte stampa.
argument-hint: "[cartella del rapporto]"
---

# Fase 5: Il Quadro

Non raccogli dati nuovi: **leggi i quattro rapporti** e li metti in fila. Sei l'unica fase che li
confronta: le altre hanno lavorato in parallelo, ciascuna sui suoi dati, e ti hanno lasciato i loro
agganci nella sezione **Per il quadro**. Il rapporto segue
[`../studio-globale/modello-del-rapporto.md`](../studio-globale/modello-del-rapporto.md), con le
aggiunte qui sotto.

## 1. Leggi

`1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`, per intero (quelli che ci sono: un rapporto
mancato si dichiara). E [`docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md`](../../../docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md),
sezione "Il quadro": cosa contiene e cosa no. Puoi aprire i JSON di `dati/` per controllare un numero,
non per rifare un'analisi.

## 2. Scrivi `5-quadro.md`

Oltre alle sezioni del modello:

- **In parole semplici e' la sezione piu' importante del quadro** (21 ottobre 2026): e' quella che
  l'utente legge in chat e in cima all'artifact. Segue **lo schema fisso del modello del rapporto**,
  intero: il punto; i sei gradini dal piu' ampio al piu' vicino (il mondo, le posizioni, le opzioni,
  le settimane, i giorni, adesso), ciascuno "fatto → cosa vuol dire"; i prezzi da guardare (tre o
  quattro); quando; cambia idea se; quanto e' provato; le parole. Si scrive per ultima, dopo le
  sezioni tecniche, e non aggiunge niente che non ci sia.
- **Il quadro in cinque righe**, dal piu' lontano al piu' vicino: macro, posizionamento, gamma,
  valore, adesso.
- **Gli agganci, uno per uno**: per ogni aggancio delle fasi, conferma, contraddizione o niente da dire.
- **La tabella dei livelli confermati**: un prezzo entra se lo indicano almeno due fasi entro una
  soglia dichiarata (15 punti su NQ, 4 su ES: `soglia_punti` di `confluenze.json`, salvo diversa ragione scritta). Colonne: prezzo, quali fasi,
  cosa dice ciascuna, a cosa serve arrivarci. **Si parte da `dati/in-breve.json` (le confluenze sono in fondo) e da `dati/confluenze.json`**: il collettore
  trova le vicinanze fra order flow, gamma e COT, e segna `stesso_dato` quando due fonti leggono lo
  stesso numero del bridge. Il quadro le verifica contro i rapporti e aggiunge quelle che vengono dalla
  macro o dalle letture delle fasi; le vicinanze non si ricalcolano a mano.
- **Le contraddizioni** fra le fasi, scritte e non risolte a forza.
- **Gli orari di oggi**: gli eventi con l'ora italiana e cosa potrebbero cambiare del quadro.
- **Cosa smentirebbe il quadro**: i fatti osservabili che lo renderebbero sbagliato.
- **Le lacune del collettore** segnalate dalle fasi, in un elenco: sono il lavoro per il codice.

## 3. Il Blocco QUADRO Della Giornata

Il giro d'orizzonte stampa alla sezione 4bis le prime 8 righe fra `<!-- QUADRO -->` e
`<!-- /QUADRO -->` del file `docs/research/giornate/<strumento>-<data>.md` (`NQZ6-<data>.md`, o
`ESZ6-<data>.md` nello studio di ES). Scrivi li' il quadro in **al
massimo 8 righe da 100 caratteri**, l'ultima con il rimando `studio/rapporti/<cartella>/5-quadro.md` (`<data>` o `<data>-ES`).

- Se il file della giornata non esiste, crealo con la struttura di
  [`come-si-scrive-una-giornata.md`](../../../docs/research/giornate/come-si-scrive-una-giornata.md):
  la sezione **1. Stato** (strumento, sessione, fonte: lo studio globale) e il blocco QUADRO; le
  altre sezioni con "da scrivere durante la seduta".
- Se esiste, **sostituisci solo il blocco**: il resto e' la cronaca della seduta e non si tocca.
- Nessun id di chart nel file: cambia a ogni ricarica.

**Il quadro non contiene direzioni, ingressi, stop o bersagli operativi.** Dice dove si sta.
