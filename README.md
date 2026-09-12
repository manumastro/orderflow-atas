# orderflow-atas

Studio del corso di order flow di Fabio, con verifica sui dati reali di ATAS.

Il repository non contiene un sistema di trading. Contiene un **metodo**: partire da cio' che il corso insegna, isolare quali affermazioni sono osservabili, misurarle, e registrare tanto cio' che regge quanto cio' che non regge. Nessuna regola operativa e' stata approvata.

## Il Percorso, Dall'Inizio

### 1. Le trascrizioni

Il punto di partenza sono le lezioni in [`fabio_course/`](fabio_course/): tre lezioni piu' un live di charting. Da queste nasce [`fabio-course-model-map.md`](fabio_course/fabio-course-model-map.md), che collega i concetti in ordine didattico e, per ciascuno, distingue quattro cose diverse:

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

### 4. Il bridge: chiedere i dati invece di catturarli

La svolta metodologica di settembre 2026. `Fabio Data Bridge` e' un indicatore che espone i dati della piattaforma su un endpoint HTTP locale, cosi' che l'analisi possa chiedere le finestre e i filtri che le servono mentre ATAS resta aperto.

Rende accessibili cose che esistono solo dentro il processo ATAS: il profilo fisso con sessione dichiarata, i trade aggregati storici con filtro di volume nativo, gli snapshot storici del book, le date di rollover. Contratto in [`docs/research/metodo/contratto-data-bridge.md`](docs/research/metodo/contratto-data-bridge.md).

### 5. La prima analisi servita dal bridge

[`docs/research/sessioni/settimana-2026-09-04-09-11.md`](docs/research/sessioni/settimana-2026-09-04-09-11.md) descrive una settimana di NQ costruendo i profili cash dal footprint reale. Mostra una migrazione ordinata del valore verso il basso su tre sedute e, soprattutto, una sessione in cui i compratori sono aggressivi (delta +3.089) e il valore scende comunque: la configurazione che il corso chiama sforzo senza risultato, qui misurata invece che intuita.

## Struttura

```text
fabio_course/              trascrizioni delle lezioni e mappa del corso
docs/research/metodo/      contratti osservativi e storia del progetto
docs/research/modelli/     modelli candidati e loro verifica su dati reali
docs/research/cot/         contesto istituzionale: COT e Tradingster
docs/research/sessioni/    profili di sessione e descrizioni settimanali
docs/research/archivio-2026-08/  la fase dei recorder, conservata come evidenza
docs/atas/                 documentazione tecnica dell'API ATAS
FabioOrderFlow/            le cinque estensioni ATAS e gli strumenti di analisi
prop/                      esecuzione: mercato guida, sizing, fee, regole delle prop
```

### 6. Il primo modello reso misurabile

[The Prop Firm Model](docs/research/modelli/modello-40r-riferimento.md) e' l'unico modello operativo del repository, ed e' esplicitamente **non validato**. La fonte di verita' e' il dossier in [`prop/prop_firm_model/`](prop/prop_firm_model/); il documento nel repository lo riporta e ne esplicita le convenzioni.

Il lavoro utile e' stato renderlo falsificabile. La prima esecuzione del [replay](docs/research/sessioni/replay-modello-settimana-2026-09-11.md) implementava non il dossier ma la sua sintesi, e aggiungeva due condizioni inesistenti: un filtro sui livelli e uno sui Big Trades. Il secondo azzerava gli ingressi, facendo sembrare che il modello non operasse mai. Era un artefatto dell'implementazione, ed e' registrato accanto al risultato corretto.

Sulla settimana, applicato come il dossier lo descrive, il modello fa 125 operazioni al 55% con obiettivo 1:1, cioe' **+0,10 R per operazione**. La finestra oraria che il dossier dichiara in anticipo regge in sei confronti su sei; la soglia sul delta flip, scelta guardando i dati, no. E il lordo atteso per operazione e' dello stesso ordine di grandezza delle commissioni.

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
