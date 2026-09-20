# Indice Delle Procedure: Come Si Fa Una Analisi

Le procedure attive, **nell'ordine in cui si usano**. Il contesto che le ha prodotte sta in
[`../percorso-del-progetto.md`](../percorso-del-progetto.md).

Qui non ci sono conclusioni di mercato. Le analisi vere stanno in [`../giornate/`](../giornate/) e
[`../sessioni/`](../sessioni/), e ogni procedura rimanda a quelle come esempio applicato.

**Lo strumento e' NQ e la sessione e' New York.** Decisione del 18 settembre 2026, e discende dal
corso: vedi il passo 0.

---

## 0pre. Prima Di Ogni Risposta, E Poi Come Si Risponde

Due procedure che non si saltano mai, e che nelle istruzioni compaiono solo come una riga.

[**`il-giro-d-orizzonte.md`**](il-giro-d-orizzonte.md) — **l'input**: cosa si guarda prima di
rispondere, per ogni risposta e in qualunque fase di mercato, piu' **il quadro** (profile framing
e COT insieme) che si fa anche senza che lo si chieda. Arriva da solo con un hook, e va letto.

[**`come-si-risponde-dal-vivo.md`**](come-si-risponde-dal-vivo.md) — **l'output**: la direzione e
i suoi numeri (`LONG` / `SHORT` / `NIENTE`, ingresso, stop, **break even**, bersaglio,
invalidazione), la forma in tre parti, l'obbligo di dichiarare modello e regime, l'obbligo di dire
**a cosa serve** ogni livello che si nomina, e la verifica di ogni termine alla fonte.

Il primo riguarda l'input e il secondo l'output: **si guarda tutto, si scrive poco, e si sceglie
quel poco proprio perche' si e' visto tutto.**

---

## 0. Il Metodo Che Comanda

[**`i-tre-modelli-del-live-q1.md`**](i-tre-modelli-del-live-q1.md) — **la fonte operativa**,
estratta dalle sei lezioni del live Q1, con il minuto citato riga per riga. Contiene la sequenza che
Fabio esegue ogni volta, i **tre modelli** (balance/mean reverting, momentum, trend following) e la
regola che sceglie quale sta girando, il vocabolario, come si marca un livello dai soli ordini
eseguiti, **il regime**, la sessione, la procedura COT esatta e le costanti del framing.

**Prima di dire che un setup esiste si dichiara quale dei tre modelli sta girando**, e lo decide la
posizione del prezzo rispetto al valore della cash precedente, non l'umore.

### 0bis. I setup

[**`i-pattern-di-esecuzione.md`**](i-pattern-di-esecuzione.md) — la libreria che Fabio chiama
*neural pattern recognition*, letta pattern per pattern, piu' i due setup che nomina fuori dalla
libreria, la distinzione **aggressivo / conservativo**, dove va lo stop (dietro una struttura, mai a
distanza fissa), dove va il bersaglio (un livello, mai un multiplo di R scelto prima) e gli **otto
casi in cui un setup non si prende**.

### 0ter. La gestione, che nel live e' l'edge

[**`la-gestione-della-posizione.md`**](la-gestione-della-posizione.md) — il break even si mette su
un **livello**, non dopo N punti: e' il prezzo al quale l'analisi si smonta. Piu' i parziali, il
trailing dietro l'ultimo big trade, il **conto in R** con il tetto sulle esecuzioni, la size che
discende dallo stop, il budget di rischio e cosa cambia in una giornata choppy.

### 0quater. Verificare un termine prima di usarlo

[**`glossario-del-metodo.md`**](glossario-del-metodo.md) — per ogni parola del metodo, **a quale
minuto della lezione** (o a quali righe del dossier) tornare. Non definisce niente: rende economico
l'obbligo di `CLAUDE.md` di non parafrasare a memoria.

### 0quinquies. Il dossier, come riferimento

[**`triple-aaa-dossier.md`**](triple-aaa-dossier.md) — trascrizione integrale del dossier illustrato:
IVB, i tre tier, le tre tecniche di esecuzione, la checklist. **E' di Fabio e va menzionato, ma non
comanda**: descrive lo stesso setup dentro un'impalcatura con un gate orario che nel live non
esiste. Nessuna condizione armata discende da qui.

---

## L'Ordine Di Una Seduta

### 1. Il contesto istituzionale, prima di aprire

[**`analisi-istituzionale.md`**](analisi-istituzionale.md) — COT e Data Bridge letti insieme: i due
livelli e i due orologi, la trappola delle due viste Tradingster, la distinzione fra flusso e
livello, i cinque osservabili del bridge.

**La procedura esatta del live** — Legacy, solo non-commercial, solo l'ultima variazione, la
matrice sui quattro indici, e poi **la data che diventa un prezzo** — sta alla sezione 9 di
[`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

**Il controllo del rollover e' il passo zero di tutto**, prima di qualunque misura di flusso.

Esempi applicati: [`../cot/snapshot-2026-09-08.md`](../cot/snapshot-2026-09-08.md),
[`../cot/tradingster-cross-index-2026-09-11.md`](../cot/tradingster-cross-index-2026-09-11.md) (esito
negativo).

### 2. Il framing, per trovare i livelli

[**`profile-framing.md`**](profile-framing.md) — le sei cose da guardare nell'ordine del live: solo
cash, dove si costruisce il valore, i profili sovrapposti da unire, value area piu' POC, posizione
nella curva, i vuoti.

Le costanti dichiarate nel live — **composito a 90 giorni**, shift = VAL e VAH entrambi piu' bassi,
**due POC** (della seduta e della seduta che ha creato la rottura), premium/fair value/discount —
stanno alla sezione 10 di [`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

Ci sta anche la regola di lettura piu' importante emersa finora: **un blocco sottile nel composito
dice solo che poche sedute ci sono passate.**

### 2bis. Il quadro: le due sopra si leggono insieme

[**`il-framing-e-il-cot-si-leggono-insieme.md`**](il-framing-e-il-cot-si-leggono-insieme.md) —
**obbligatorio, e automatico.** Il risultato si scrive nel file della giornata fra `<!-- QUADRO -->`
e `<!-- /QUADRO -->`, e `giro_orizzonte.py` lo stampa alla **sezione 4bis a ogni messaggio**.

### 3. I dati, dal bridge

[**`contratto-data-bridge.md`**](contratto-data-bridge.md) — endpoint, parametri, limiti, schema.
Client `FabioOrderFlow/tools/bridge.py`.

Una regola che e' costata un errore: **`bar` e' un indice di posizione, non un identificatore.**

### 4. I livelli sul chart

[**`livelli-sul-chart.md`**](livelli-sul-chart.md) — il giro completo dai dati grezzi alla linea
disegnata. **L'indicatore disegna, `bridge.py` trasporta, la derivazione resta nell'analisi.**

**Quali livelli si marcano** lo dice il live, ed e' una lista di due: la **massima aggressione** e
il **massimo assorbimento impilato**, entrambi da ordini eseguiti. *"It's not necessary to mark
intermediate level that are useless for us."*

E l'obbligo che il 16 settembre e' costato una lettura sbagliata: **i livelli si rifanno durante la
seduta**, e **il ridisegno non si chiede, si fa**.

### 4bis. Il delta prezzo per prezzo

[**`la-footprint-e-il-delta-per-prezzo.md`**](la-footprint-e-il-delta-per-prezzo.md) — `candles
--levels` restituisce la footprint di ogni barra. **E' lo strumento con cui si misura
l'assorbimento del live**: sforzo alto, risultato nullo. Separa una **base**, un **passaggio** e un
**assorbimento**, che il delta di barra confonde.

### 5. Il contesto vivo, durante la seduta

[**`il-contesto-vivo.md`**](il-contesto-vivo.md) — **l'impianto attivo dal 20 settembre 2026.**
Nessun processo esterno: **i livelli li calcola l'indicatore** a ogni barra, dalle regole che
l'analisi deposita una volta, e il **pannello** si ridisegna a ogni tick. Nessuno dei due giudica,
avvisa o arma niente. La lettura si chiede.

[**`i-livelli-li-calcola-l-indicatore.md`**](i-livelli-li-calcola-l-indicatore.md) — **il
documento che dice chi calcola cosa**: i due assi del vocabolario (misurato / dichiarato, finestra
aperta / chiusa), i tre marcatori `~` `=` `*`, i tipi di regola compresi `mensola`, `tetto`,
`aggressione` e `assorbimento`, le difese, e cosa resta all'agente — che e' **quali** livelli
contano e **a cosa serve arrivarci**, non il loro prezzo.

Quello che segue descrive la **fase chiusa** della sorveglianza a condizioni armate, conservata
come evidenza.

### 5ter. I livelli statici, PROBLEMA DISSOLTO

[**`i-livelli-statici-la-strada-del-sottoagente.md`**](i-livelli-statici-la-strada-del-sottoagente.md)
— chi rifa' i livelli fissi non e' piu' una domanda aperta: quasi tutti erano gia' regole, e i
pochi che non lo erano sono diventati tipi calcolati. Il documento resta come evidenza della strada
del sottoagente, chiusa il giorno stesso in cui era stata aperta.

### 5bis. La sorveglianza del tape, FASE CHIUSA

[**`sorveglianza-del-tape.md`**](sorveglianza-del-tape.md) — la procedura standard di studio di una
seduta, i tre sorveglianti, e **le sette prove che una condizione deve passare prima di essere
armata**. Le prove restano obbligatorie: misurano il **sosia**, che la libreria dei pattern non
misura.

**Il terzo sorvegliante non guarda i livelli.** `sveglia_movimento.py` copre il punto cieco degli
altri due, che fra un livello e l'altro non parlano.

### 6. La giornata, mentre succede

[**`../giornate/come-si-scrive-una-giornata.md`**](../giornate/come-si-scrive-una-giornata.md) — un
file per giornata, scritto **durante** la seduta. Contiene la procedura per riprendere a meta'
sessione. Le letture sbagliate restano scritte con cio' che le ha smentite.

**Ci va anche il conto in R della giornata e il regime dichiarato**: sono gli input della gestione.

### 6bis. Le gambe, a seduta chiusa

[**`le-gambe-di-una-seduta.md`**](le-gambe-di-una-seduta.md) — quanto una giornata ha **davvero**
offerto. Una gamba dura finche' il prezzo non ritraccia piu' di **R** punti dal proprio estremo, e R
e' un parametro dello strumento (12 su NQ).

Ci sta la lezione che vale oltre il caso: **un pattern trovato leggendo all'indietro dai movimenti
buoni ha per costruzione il 100% di successo.**

---

## Documenti Conservati, Non Piu' Operativi

Restano come misure e come evidenza. **Non autorizzano niente.**

- [`la-sessione-di-londra.md`](la-sessione-di-londra.md) — i numeri su NQZ6 reggono; la sessione
  operativa e' New York, perche' lo dice il corso.
- [`la-mattina-europea-sull-oro.md`](la-mattina-europea-sull-oro.md) — lo strumento e' NQ.
- [`il-permesso-si-misura-non-si-aspetta.md`](il-permesso-si-misura-non-si-aspetta.md) — misurava
  l'accettazione contro il gate M30 del dossier. **Nel live il gate non c'e'**, quindi la domanda
  a cui rispondeva non si pone piu'. La misura dell'accettazione su barre M1 resta utile e
  riusabile; il confronto col gate no.

---

## Contratti Osservativi

Documenti che fissano *prima* cosa si registra e cosa no, cosi' che il risultato non possa essere
adattato all'ipotesi dopo averlo visto.

- [`contratto-osservativo-partecipazione.md`](contratto-osservativo-partecipazione.md)
- [`../sessioni/contratto-profilo-pre-sessione.md`](../sessioni/contratto-profilo-pre-sessione.md)

## Storia

[`storia-del-progetto.md`](storia-del-progetto.md) — cosa c'era prima della baseline attuale, e dove
sta la discontinuita'. Serve a non riusare soglie e risultati di esperimenti ritirati.

[`come-si-apre-un-asset.md`](come-si-apre-un-asset.md) — i nove passi per aprire uno strumento
nuovo. **Con NQ solo, serve a un caso solo: il rollover.** Il resto resta come evidenza di cosa
costa aprire un asset a meta', documentata in
[`../giornate/MCLV6-2026-09-16.md`](../giornate/MCLV6-2026-09-16.md).
