# Indice Delle Procedure: Come Si Fa Una Analisi

Le procedure attive, **nell'ordine in cui si usano**. Il contesto che le ha prodotte sta in
[`../percorso-del-progetto.md`](../percorso-del-progetto.md): leggilo prima, se non l'hai gia' fatto.

Qui non ci sono conclusioni di mercato. Le analisi vere stanno in [`../giornate/`](../giornate/) e
[`../sessioni/`](../sessioni/), e ogni procedura rimanda a quelle come esempio applicato.

---

## 0bis. Verificare Un Termine Prima Di Usarlo

[`glossario-del-metodo.md`](glossario-del-metodo.md) — per ogni parola del metodo, **a quali righe
della fonte tornare**. Non definisce niente: serve a rendere economico l'obbligo di `CLAUDE.md` di
non parafrasare a memoria. Si usa prima di scrivere un `verso`, una etichetta operativa, o una
frase che contiene un termine del corso.

## L'Ordine

### 0. Il modello, per sapere cosa si sta cercando

[**`triple-aaa-dossier.md`**](triple-aaa-dossier.md) — trascrizione integrale del dossier del
corso. Tre livelli: l'**IVB** a 30 minuti sulla cash di New York decide da che parte si puo'
operare; il **Tier 02** decide dove (mean reverting sui bordi, Triple AAA sulla rottura, A+ sul
ritracciamento profondo); il **Tier 03** decide quando.

Contiene anche, dichiarato, **cosa e' stato misurato e cosa no**. Non confondere il dossier con
l'archivio del modello 40R, che ha testato una meccanizzazione parziale di tre soli trigger.

### 0ter. Aprire uno strumento nuovo, o riaprirlo dopo un rollover

[**`come-si-apre-un-asset.md`**](come-si-apre-un-asset.md) — **obbligatorio, nove passi**, dal
controllo del rollover al prefisso dei file. Esiste perche' il metodo e' nato sul Nasdaq e quasi
tutto il resto presuppone NQ senza dirlo: orologi, soglie in tick, finestra IVB, codice COT. Dice
anche cosa **non** si trasferisce, e come si dichiara un trasferimento non verificato.

Esempio applicato: [`../giornate/MCLV6-2026-09-16.md`](../giornate/MCLV6-2026-09-16.md),
[`../cot/snapshot-MCLV6-2026-09-16.md`](../cot/snapshot-MCLV6-2026-09-16.md).

### 1. Il contesto istituzionale, prima di aprire

[**`analisi-istituzionale.md`**](analisi-istituzionale.md) — COT e Data Bridge letti insieme: i due
livelli e i due orologi, la trappola delle due viste Tradingster, la distinzione fra flusso e
livello, i cinque osservabili del bridge, la procedura settimanale in sette passi.

**Il controllo del rollover e' il passo zero di tutto**, prima di qualunque misura di flusso.

Esempi applicati: [`../cot/snapshot-2026-09-08.md`](../cot/snapshot-2026-09-08.md),
[`../cot/tradingster-cross-index-2026-09-11.md`](../cot/tradingster-cross-index-2026-09-11.md) (esito
negativo).

### 2. Il framing, per trovare i livelli

[**`profile-framing.md`**](profile-framing.md) — le sei cose da guardare nell'ordine del live: solo
cash, dove si costruisce il valore, i profili sovrapposti da unire, value area piu' POC, posizione
nella curva, i vuoti. Piu' il quadro corrente e l'avvertenza sul continuous non back-adjusted.

Ci sta anche la regola di lettura piu' importante emersa finora: **un blocco sottile nel composito
dice solo che poche sedute ci sono passate.** Prima di chiamarlo vuoto, guarda il profilo della
seduta piu' recente che quel prezzo l'ha visitato.

Esempi applicati: [`../sessioni/settimana-2026-09-04-09-11.md`](../sessioni/settimana-2026-09-04-09-11.md),
[`../sessioni/seduta-2026-09-11.md`](../sessioni/seduta-2026-09-11.md).

### 3. I dati, dal bridge

[**`contratto-data-bridge.md`**](contratto-data-bridge.md) — endpoint, parametri, limiti, schema.
Il client e' `FabioOrderFlow/tools/bridge.py`.

Una regola che e' costata un errore: **`bar` e' un indice di posizione, non un identificatore.**
Riparte quando ATAS ricarica l'indicatore. Per riconoscere una barra si usa `time`.

### 4. I livelli sul chart

[**`livelli-sul-chart.md`**](livelli-sul-chart.md) — il giro completo dai dati grezzi alla linea
disegnata. Divisione dei ruoli netta: **l'indicatore disegna, `bridge.py` trasporta, la derivazione
resta nell'analisi.** Non spostare quel calcolo dentro l'indicatore.

Ci sono anche le tre cose che rendono utile un'etichetta — il nome, la misura che la sostiene, lo
stato — e quella che non ci va mai: la previsione.

E l'obbligo che il 16 settembre e' costato una lettura sbagliata: **i livelli si rifanno durante la
seduta.** Il valore (VAH, VAL, POC) si ricalcola a ogni lettura che lo usa, l'insieme dei livelli
quando il prezzo esce dalla fascia per cui era stato derivato e sempre dopo la stampa dell'IVB, le
etichette quando il livello cambia funzione. Sezione *"I Livelli Si Rifanno Durante La Seduta"*.

E il corollario che chiude il giro: **il ridisegno non si chiede, si fa** — file, `POST /levels`,
scenari riscritti, sorveglianti riavviati, *poi* la lettura. Piu' la regola che dopo l'apertura
cash il valore si misura **sulla cash**, non sulla finestra che comprende la globex: il 16
settembre le due misure davano VAH 29.455 contro 29.480,75.

### 4bis. Il delta prezzo per prezzo

[**`la-footprint-e-il-delta-per-prezzo.md`**](la-footprint-e-il-delta-per-prezzo.md) — `candles
--levels` restituisce la footprint di ogni barra: `ask` meno `bid` a **ogni prezzo scambiato**,
piu' `maxPositiveDelta`, `maxNegativeDelta` e il POC di quella barra.

Serve a separare tre cose che il delta di barra confonde: una **base** (picco di delta e POC della
barra allo stesso prezzo), un **passaggio** (delta sparso) e un **assorbimento** (volume alto,
delta vicino a zero). Il 16 settembre ho dichiarato che questo dato non esisteva mentre era
documentato nel contratto del bridge: prima di dire *"non ce l'ho"* si apre l'elenco degli
endpoint.

### 5. La sorveglianza del tape, durante la seduta

[**`sorveglianza-del-tape.md`**](sorveglianza-del-tape.md) — la procedura standard di studio di una
seduta. I livelli stanno in un file per giornata, che e' **l'unica fonte**: lo legge sia chi li
spinge sul chart sia `sveglia_tape.py`, che sorveglia il tape.

La divisione e' netta e vale la pena capirla. **Gli scenari li scrive l'analisi prima della
seduta**, in `scenari-AAAA-MM-GG.json`, guardando il contesto di quel giorno: non sono codice fisso,
sono un file che si riscrive ogni volta. `scenari.py` e' solo il motore che li valuta, e quando uno
scatta annota da solo — cosi' a schermo compare gia' lo scenario previsto, col nome che gli avevamo
dato. `sveglia_tape.py` gira in parallelo per cio' che non avevamo previsto, e `annota.py` serve
alla lettura ragionata che arriva dopo.

### 6. La giornata, mentre succede

[**`../giornate/come-si-scrive-una-giornata.md`**](../giornate/come-si-scrive-una-giornata.md) — un file per giornata di mercato, scritto
**durante** la seduta. Contiene la procedura per **riprendere a meta' sessione**: intestazione,
"Dove eravamo", correzioni, e solo dopo i dati nuovi.

Le letture sbagliate restano scritte con cio' che le ha smentite: e' la parte verificabile del
documento.

### 6bis. Le gambe, a seduta chiusa

[**`le-gambe-di-una-seduta.md`**](le-gambe-di-una-seduta.md) — quanto una giornata ha **davvero**
offerto. Una gamba dura finche' il prezzo non ritraccia piu' di **R** punti dal proprio estremo, e
R e' un parametro dello strumento (12 su NQ, 3 su ES, 0,25 $ sul crude), non una costante.

Esiste perche' il 16 settembre avevo misurato *"il massimo dei 30 minuti successivi"* e chiamato
quel numero un movimento: era l'**inviluppo dell'opportunita'**, cioe' il risultato di chi indovina
ogni giro. L'utente l'ha visto dal grafico. Ci avevo gia' costruito sopra una lettura del Triple
AAA che non stava in piedi.

Ci sta anche la lezione che vale oltre il caso: **un pattern trovato leggendo all'indietro dai
movimenti buoni ha per costruzione il 100% di successo.** Il numero che conta e' quante volte la
stessa firma compare *senza* il movimento — quel giorno, 19 occorrenze e 42%.

E la **deroga dichiarata** dell'utente: si opera anche **prima del gate** Tier 01, con i tre
obblighi che la deroga non sospende.

---

## Contratti Osservativi

Documenti che fissano *prima* cosa si registra e cosa no, cosi' che il risultato non possa essere
adattato all'ipotesi dopo averlo visto.

- [`contratto-osservativo-partecipazione.md`](contratto-osservativo-partecipazione.md)
- [`../sessioni/contratto-profilo-pre-sessione.md`](../sessioni/contratto-profilo-pre-sessione.md)

## Storia

[`storia-del-progetto.md`](storia-del-progetto.md) — cosa c'era prima della baseline attuale, e
dove sta la discontinuita'. Serve a non riusare soglie e risultati di esperimenti ritirati.
