# orderflow-atas

Studio del corso di order flow di Fabio, con verifica sui dati reali di ATAS.

Il repository non contiene un sistema di trading. Contiene un **metodo**: partire da cio' che il corso insegna, isolare quali affermazioni sono osservabili, misurarle, e registrare tanto cio' che regge quanto cio' che non regge. Nessuna regola operativa e' stata approvata.

## Il Percorso, Dall'Inizio

### 1. Le trascrizioni

Il punto di partenza sono le lezioni in [`fabio_course/`](fabio_course/): tre lezioni piu' un live di charting, accompagnato dal dossier del modello in [`fabio_course/ivbaaa/`](fabio_course/ivbaaa/) — sette pagine trascritte per intero in [`triple-aaa-dossier.md`](docs/research/metodo/triple-aaa-dossier.md). Da queste nasce [`fabio-course-model-map.md`](fabio_course/fabio-course-model-map.md), che collega i concetti in ordine didattico e, per ciascuno, distingue quattro cose diverse:

```text
concetti insegnati
informazioni direttamente osservabili
passaggi discrezionali
limiti dei dati disponibili in ATAS
```

Questa distinzione e' la regola che governa tutto il resto. Una tecnica citata in una lezione non diventa una specifica software finche' non si e' stabilito quale parte di essa e' misurabile.

### 2. Dal concetto al dato osservabile

Il modello del corso poggia su una sequenza: **location**, **sforzo**, **risultato**, **accettazione o rifiuto**. Sono concetti, non campi di un feed. Il lavoro di agosto 2026 e' consistito nel tradurli in quantita' che ATAS espone davvero, e nel verificare che la traduzione fosse fedele.

Ne sono usciti quattro recorder e una serie di contratti osservativi: documenti che fissano *prima* cosa si registra e cosa no, cosi' che il risultato non possa essere adattato all'ipotesi dopo averlo visto. Quel materiale e' in [`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/).

Il limite di quell'approccio e' emerso con l'uso: il recorder decide lo schema prima della cattura, quindi ogni nuova domanda richiede una nuova cattura.

### 3. Il contesto istituzionale: COT e Tradingster

Il live introduce il Commitment of Traders come contesto settimanale, letto a mano su `tradingster.com`. Lo studio in [`docs/research/cot/`](docs/research/cot/) ha fatto tre cose:

- **Chiarito la fonte.** Tradingster espone due report diversi per lo stesso strumento. `Non-Commercial` esiste solo nella vista Legacy; `Asset Manager` e `Leveraged Funds` solo nella vista TFF. Nel live i due nomi vengono usati come sinonimi: non lo sono.
- **Reso la raccolta riproducibile.** Le pagine caricano dieci anni di serie settimanali dentro il grafico. Estratte via browser, diventano un CSV invece di una lettura a occhio.
- **Verificato la metrica cross-index.** Resa numerica in due modi diversi: nella forma piu' letterale **non separa** la risposta di prezzo delle quattro settimane successive; nella forma piu' selettiva la separa, ma su 12 e 3 occorrenze, troppo poche per concludere.

Il COT resta quindi contesto, non trigger, e non c'e' motivo di costruirci un indicatore.

Il metodo completo, che tiene insieme il posizionamento settimanale del COT e la partecipazione osservata sul tape, e' in [`docs/research/metodo/analisi-istituzionale.md`](docs/research/metodo/analisi-istituzionale.md): cosa chiede ciascuna delle due fonti, perche' non vanno fuse in un punteggio unico, la procedura settimanale e il controllo del rollover che precede ogni altra misura.

### 4. Il bridge: chiedere i dati invece di catturarli

La svolta metodologica di settembre 2026. `Fabio Data Bridge` e' un indicatore che espone i dati della piattaforma su un endpoint HTTP locale, cosi' che l'analisi possa chiedere le finestre e i filtri che le servono mentre ATAS resta aperto.

Rende accessibili cose che esistono solo dentro il processo ATAS: il profilo fisso con sessione dichiarata, i trade aggregati storici con filtro di volume nativo, gli snapshot storici del book, le date di rollover. Contratto in [`docs/research/metodo/contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md).

### 5. La prima analisi servita dal bridge

[`docs/research/sessioni/settimana-2026-09-04-09-11.md`](docs/research/sessioni/settimana-2026-09-04-09-11.md) descrive una settimana di NQ costruendo i profili cash dal footprint reale. Mostra una migrazione ordinata del valore verso il basso su tre sedute e, soprattutto, una sessione in cui i compratori sono aggressivi (delta +3.089) e il valore scende comunque: la configurazione che il corso chiama sforzo senza risultato, qui misurata invece che intuita.

## Struttura

```text
fabio_course/              trascrizioni delle lezioni e mappa del corso
docs/research/metodo/      metodo attivo: profile framing, analisi istituzionale, contratti
docs/research/cot/         contesto istituzionale: COT e Tradingster
docs/research/sessioni/    profili di sessione e descrizioni settimanali
docs/research/giornate/    una analisi per giornata di mercato, AAAA-MM-GG.md
docs/research/archivio-2026-08/     la fase dei recorder, conservata come evidenza
docs/research/archivio-modello-40r/ la fase sistematica, misurata e chiusa
docs/atas/                 documentazione tecnica dell'API ATAS
FabioOrderFlow/            le cinque estensioni ATAS e gli strumenti di analisi
prop/                      esecuzione: mercato guida, sizing, fee, regole delle prop
```

### 6. Il modello sistematico, misurato e chiuso

The Prop Firm Model e' stato implementato dal dossier, misurato su un mese di NQ e **chiuso il 15 settembre 2026**. Con obiettivo 1:1 sta al **49-51%**, indistinguibile dal modello nullo che entra senza nessuna condizione. Big trades, speed of tape, scala della barra, filtro sul ritmo, gate di location e inversione del verso: nessuno sposta il risultato.

Il materiale e' conservato in [`docs/research/archivio-modello-40r/`](docs/research/archivio-modello-40r/) come evidenza. Vale la pena leggerlo per la sequenza degli errori — sintesi al posto della fonte, look-ahead nella risoluzione degli esiti, un limit order che si eseguiva a mercato — e per la correzione che ha smontato ogni risultato apparente: **le osservazioni dentro una sessione non sono indipendenti**, e un z ingenuo di +4,1 diventa t=+1,4 quando la statistica si calcola per sessione.

### 7. Lo scope attuale: la lettura discrezionale

Dal 15 settembre 2026 il lavoro e' sulla lettura discrezionale descritta nel primo live del corso, letta insieme al contesto istituzionale.

- [`docs/research/metodo/profile-framing.md`](docs/research/metodo/profile-framing.md) — le sei cose da guardare, nell'ordine in cui il live le mette, piu' il quadro corrente e l'avvertenza sul chart continuous che non e' back-adjusted.
- [`docs/research/metodo/analisi-istituzionale.md`](docs/research/metodo/analisi-istituzionale.md) — COT e Data Bridge letti insieme, incluso il modo in cui il COT resta valido fra un report e l'altro: la data di un cambio di posizionamento diventa un livello di prezzo, e il livello sopravvive alla settimana.
- [`docs/research/metodo/livelli-sul-chart.md`](docs/research/metodo/livelli-sul-chart.md) — il giro completo dai dati grezzi alla linea disegnata: l'indicatore non calcola, il client trasporta, la derivazione resta nell'analisi.
- [`docs/research/giornate/`](docs/research/giornate/) — una analisi per giornata di mercato: i livelli di apertura e da dove vengono, la cronaca con volume e delta accanto a ogni momento, e le letture sbagliate con cio' che le ha smentite.
- [`docs/research/metodo/triple-aaa-dossier.md`](docs/research/metodo/triple-aaa-dossier.md) — la trascrizione integrale del dossier che accompagna il live: il gate dell'IVB a 30 minuti, i tre modelli del Tier 02, le tre tecniche di esecuzione, la checklist. E' la fonte del modello, non la sua validazione: cosa e' stato misurato, e cosa no, sta dichiarato in fondo al documento.

## Come Si Lavora Qui

1. **Prima di misurare, dichiarare.** Un contratto osservativo fissa la finestra, i campi e i criteri prima della raccolta.
2. **Distinguere livello e flusso, descrizione e regola.** Una co-occorrenza osservata in una sessione non e' una regolarita'.
3. **Registrare anche i risultati negativi.** La verifica della metrica cross-index vale quanto una che avesse funzionato, e sta nel repository allo stesso titolo.
4. **Dichiarare le convenzioni.** Soglie, frazioni di area di valore e orizzonti sono scelte, non fatti: vanno scritte accanto al risultato.
5. **Una fonte canonica per decisione.** Ogni fase sostanziale aggiorna il documento pertinente e aggiunge una riga datata a [`FabioOrderFlow/progress.txt`](FabioOrderFlow/progress.txt).

## Avvio Rapido

```bash
# build e deploy delle estensioni ATAS
cd FabioOrderFlow/src && ./deploy.sh

# caricare Fabio Data Bridge su un chart, poi
python3 FabioOrderFlow/tools/bridge.py health
```
