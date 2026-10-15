---
name: studio-globale
description: Lo studio globale di NQ dal piu' lontano al piu' vicino - macro, COT, gamma, order flow delle quattro tab - con un collettore che prende tutti i dati, quattro agenti che li leggono in parallelo e un quinto che scrive il quadro del giorno. Usala quando l'utente chiede lo studio globale, il quadro completo, o di preparare la giornata.
argument-hint: "[AAAA-MM-GG] [rifai:<fasi>] - oppure a parole, per esempio \"tieni macro e COT, rifai gamma e order flow\""
---

# Lo Studio Globale: L'Orchestratore

Sei l'orchestratore. **Non studi tu**: fai partire il collettore, poi gli agenti, controlli le
consegne e riferisci. Il metodo e il perche' stanno in
[`studio/il-sistema-di-studio.md`](../../../studio/il-sistema-di-studio.md); il modello del rapporto
che ogni fase rispetta in [`modello-del-rapporto.md`](modello-del-rapporto.md).

```text
1 raccolta    python studio/raccogli.py            dati/            ~6 minuti
2 analisi     4 agenti IN PARALLELO, solo lettura  1-macro.md  2-cot.md  3-gamma.md  4-orderflow.md
3 quadro      1 agente, legge i quattro rapporti   5-quadro.md + il blocco QUADRO della giornata
```

**Perche' cosi'** (15 ottobre 2026): nel primo giro gli agenti raccoglievano e ricalcolavano da soli,
uno dopo l'altro, e quattro fasi hanno preso un'ora. Ora il codice misura tutto una volta, gli
agenti leggono file e non hanno Bash, e il confronto fra le fasi lo fa solo il quadro.

## La Velocita' E' Un Requisito

Richiesta dell'utente del 15 ottobre 2026: "lo studio deve essere veloce". **Lo studio intero sta sotto
i dieci minuti**:

| passo | tempo | come |
|---|---|---|
| raccolta | meno di 1 minuto | **ogni calcolo si fa una volta** (richiesta del 15 ottobre 2026). La GEX e' una sola per giro e studio: si riusa se ha meno di 30 minuti, dichiarandone l'eta', altrimenti si calcola una volta (circa 3,5 minuti) e la usa anche il giro. Il giro e le quattro tab si fanno una volta, dopo la GEX. Macro 25 s, COT 15 s, order flow 4 s |
| analisi | 3-4 minuti | in parallelo, ognuna parte appena i suoi dati ci sono; rapporti di 120 righe al massimo, `maxTurns: 20` |
| quadro | 3-4 minuti | parte da `confluenze.json`, che il codice gia' calcola; 150 righe al massimo |

Se un passo sfora, **e' un difetto da correggere nel codice o nella skill**, non un'attesa da
spiegare: si misura dove va il tempo e si toglie lavoro agli agenti, non controlli.

## I Passi

1. **La data, la cartella e cosa rifare.** La data e' l'argomento, se c'e'; altrimenti oggi, in ora
   italiana (`python -c "import datetime,zoneinfo;print(datetime.datetime.now(zoneinfo.ZoneInfo('Europe/Rome')).date())"`).
   La cartella e' **una per giorno**, `studio/rapporti/<data>/`. **Cosa rifare lo decide l'utente, e
   glielo si chiede ogni volta** (richiesta del 15 ottobre 2026), prima di raccogliere qualunque cosa:
   - guarda quali rapporti di quel giorno esistono e l'ora della loro raccolta (`dati/raccolta.json`);
   - fai **una domanda** con AskUserQuestion, a scelta multipla, sulle fasi da rifare: macro, COT,
     gamma, order flow. Per ogni fase scrivi se c'e' gia' e di che ora. Il quadro si rifa' sempre e non
     si chiede;
   - le opzioni consigliate seguono la tabella di "L'aggiornamento in giornata". Il COT si consiglia
     solo dopo un report nuovo (venerdi', 21:30 italiane). La macro si consiglia dopo un dato del
     calendario;
   - se la richiesta indica gia' le fasi, si chiede lo stesso, con quelle come scelta consigliata;
   - **senza nessuno davanti** (`claude -p`) non si puo' chiedere: si fa cio' che dicono gli argomenti,
     altrimenti tutto.
2. **La raccolta.** In primo piano, con un timeout di 15 minuti, con le sole fonti delle fasi da rifare
   (fase 1 `macro`, 2 `cot`, 3 `gex`, 4 `orderflow`; tutte se si rifa' tutto):

   ```bash
   python studio/raccogli.py --out studio/rapporti/<data>/dati --fonti <fonti>
   ```

   Le fonti non richieste restano come sono; i file riscritti finiscono in `dati/precedenti/<HHMM>/`.

   **Non aspettare la fine della raccolta** (15 ottobre 2026: "cosi' ci mette tanto"). Le fonti arrivano
   a tempi diversi: macro, COT e order flow in meno di un minuto; la GEX di NQ in circa 3 minuti (meno di
   un secondo se il giro l'ha calcolata negli ultimi cinque minuti). **La GEX e' solo di NQ**: quella di ES
   (`--con-es`, 6-8 minuti in piu') si raccoglie solo se l'utente la chiede. Si fa cosi':
   - lancia la raccolta in background;
   - appena i file di una fase sono riscritti dopo l'avvio, sposta il suo rapporto vecchio e lancia il
     suo agente. Macro: `macro.json`. COT: `cot.json`. Order flow: `tab-ruoli.json`, `sedute-NQZ6.json`,
     `pannello.json`. Gamma: `gex-NQZ6.json`;
   - dagli agenti che partono prima della fine, di' che `raccolta.json` sara' riscritto dopo.

   Il quadro parte solo a raccolta finita e a rapporti consegnati. Leggi `dati/raccolta.json`: per ogni
   fonte stato, durata, errore. Se il bridge e' assente, le fasi
   3 e 4 partono lo stesso e scrivono `Stato: senza dati`. Dillo all'utente.
3. **Le analisi da rifare, in parallelo.** Prima sposta i rapporti che stai per rifare (e il
   `5-quadro.md`) in `studio/rapporti/<data>/precedenti/<HHMM>/`. Poi un solo messaggio con una chiamata
   all'Agent tool per fase da rifare, `subagent_type` fra `studio-macro`, `studio-cot`, `studio-gamma`,
   `studio-orderflow`, ciascuna con:

   ```text
   Fase <n> dello studio globale di NQ, <data>. Cartella: studio/rapporti/<data>/.
   I dati sono gia' raccolti in dati/ (vedi dati/raccolta.json). Segui la tua skill e il modello del
   rapporto, scrivi <n>-<fase>.md e rispondi con 5 righe: stato, tre fatti, le lacune dei dati.
   ```

4. **Il controllo.** Per ogni rapporto: esiste; ha le sezioni del modello; lo `Stato` e' dichiarato;
   i numeri hanno fonte e ora; nessuna direzione operativa (LONG, SHORT, ingresso, stop, bersaglio).
   Se manca qualcosa, rimanda **lo stesso agente** una volta (SendMessage) con cosa correggere.
5. **Il quadro.** `studio-quadro`, **sempre**, con l'elenco dei rapporti: quali sono nuovi, quali
   riusati e di che ora (da `dati/raccolta.json`), quali mancano. Il quadro dichiara l'eta' di ogni fase
   che usa.
6. **La fine: lo studio, non il resoconto del lavoro.** All'utente va **il contenuto del quadro**,
   scritto in chiaro in chat (15 ottobre 2026: un resoconto fatto di tempi, costi e percorsi non e' un
   risultato). Nell'ordine:
   - la riga del giorno;
   - le cinque righe (macro, COT, gamma, valore, adesso), ciascuna con l'eta' del suo dato;
   - cosa e' cambiato dall'ultimo quadro, se e' un aggiornamento;
   - i livelli confermati da almeno due fasi, con a cosa serve arrivarci;
   - le contraddizioni;
   - gli orari, in ora italiana;
   - cosa smentirebbe il quadro.

   Il testo si prende da `5-quadro.md`, senza aggiungere niente. Poi, in due righe: le fasi parziali e
   le lacune piu' gravi, il tempo, il percorso del quadro. **Nessun commit**: lo decide l'utente.
7. **Il sito o l'artifact, solo se l'utente lo chiede** (vedi "La pubblicazione" sotto).

## La Pubblicazione, Su Richiesta

Per difetto lo studio si da' in chat. Se l'utente chiede di vederlo **nel sito** o **in un artifact**,
si pubblica lo studio **del giorno attuale**: si aggiorna quello che c'e', o si crea se manca.

- **Il sito** (`quant/sito/sito.py`, http://127.0.0.1:8790) legge `studio/rapporti/` a ogni pagina: non
  c'e' niente da rigenerare. Se non risponde, avvialo in background (`python quant/sito/sito.py`).
  Poi dai il link `http://127.0.0.1:8790/#/giorno/<data>`. Il sito mostra il quadro, le quattro fasi
  e i quadri sostituiti.
- **L'artifact** e' uno per giorno.
  1. `python studio/pagina_del_giorno.py <data>` scrive
     `studio/rapporti/<data>/dati/studio-<data>.html`, con i cinque rapporti e i quadri sostituiti.
  2. Se `studio/rapporti/<data>/dati/artifact.txt` contiene un indirizzo, l'artifact di quel giorno
     esiste gia'. Lo si legge (`action: "read"`) e si ripubblica con `url` uguale a quell'indirizzo.
  3. Se `artifact.txt` non c'e', si pubblica il file senza `url` (icona `chart`) e si scrive
     l'indirizzo restituito in `artifact.txt`.

  Il codice impagina e non riscrive: la pagina contiene il testo dei rapporti com'e'.

## L'Aggiornamento In Giornata

Macro e COT cambiano poco in una giornata (il COT una volta a settimana); gamma e order flow cambiano
di minuto in minuto. Per questo l'utente puo' chiedere di **rifare solo una parte**, e la scelta
**dipende dalla sua richiesta**, non dall'orchestratore:

| la richiesta | fasi da rifare | `--fonti` |
|---|---|---|
| niente di particolare, "studio globale", "rifai tutto" | 1 2 3 4, poi 5 | `macro,cot,orderflow,gex` |
| "tieni macro e COT", "senza rifare macro e COT", "aggiorna gamma e order flow" | 3 4, poi 5 | `orderflow,gex` |
| "solo l'order flow", "aggiorna le tab" | 4, poi 5 | `orderflow` |
| `rifai:3,4` (o qualunque elenco di fasi) | quelle, poi 5 | le loro fonti |

Le regole dell'aggiornamento:

- **Si riusa solo cio' che esiste per quel giorno.** Se l'utente chiede di tenere macro e COT ma
  `1-macro.md` o `2-cot.md` di quel giorno non ci sono, lo dici e li fai: un quadro senza macro non e'
  un aggiornamento, e' un buco.
- **Il riuso si dichiara**: nel resoconto all'utente e nel quadro, con l'ora della raccolta di ogni fase
  riusata ("macro delle 10:45, COT al 6 ottobre").
- **Il quadro si rifa' sempre**, perche' e' l'unico che confronta le fasi: una fase nuova puo' confermare
  o smentire una vecchia.
- **Niente si cancella**: rapporti e dati sostituiti vanno in `precedenti/<HHMM>/`, cosi' si puo'
  rileggere com'era il quadro alle 10:45 e com'e' alle 15:00.

## Le Regole Che Valgono Per Tutte Le Fasi

- **Descrizione, non regola.** Nessuna fase produce un ingresso, uno stop o una direzione. Una
  lettura operativa si da' solo se l'utente la chiede, con gli obblighi della lettura dal vivo.
- **I dati esterni sono contesto.** Macro e COT vengono da fuori ATAS su richiesta dell'utente (15
  ottobre 2026): servono a descrivere, mai a un backtest. NQ 2025-2026 dal bridge e' lettura dal vivo,
  non `quant/dati`: il fuori campione del metodo quantitativo non si tocca.
- **Una misura che manca e' una lacuna del collettore**: il rapporto la dichiara, e la si aggiunge a
  `studio/raccogli.py`. Gli agenti non la rifanno a mano.
- **I file grezzi** stanno in `studio/rapporti/<data>/dati/`, fuori da git.
