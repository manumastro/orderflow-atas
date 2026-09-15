# Indice Delle Procedure: Come Si Fa Una Analisi

Le procedure attive, **nell'ordine in cui si usano**. Il contesto che le ha prodotte sta in
[`../percorso-del-progetto.md`](../percorso-del-progetto.md): leggilo prima, se non l'hai gia' fatto.

Qui non ci sono conclusioni di mercato. Le analisi vere stanno in [`../giornate/`](../giornate/) e
[`../sessioni/`](../sessioni/), e ogni procedura rimanda a quelle come esempio applicato.

---

## L'Ordine

### 0. Il modello, per sapere cosa si sta cercando

[**`triple-aaa-dossier.md`**](triple-aaa-dossier.md) — trascrizione integrale del dossier del
corso. Tre livelli: l'**IVB** a 30 minuti sulla cash di New York decide da che parte si puo'
operare; il **Tier 02** decide dove (mean reverting sui bordi, Triple AAA sulla rottura, A+ sul
ritracciamento profondo); il **Tier 03** decide quando.

Contiene anche, dichiarato, **cosa e' stato misurato e cosa no**. Non confondere il dossier con
l'archivio del modello 40R, che ha testato una meccanizzazione parziale di tre soli trigger.

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

### 5. La sorveglianza del tape, durante la seduta

[**`sorveglianza-del-tape.md`**](sorveglianza-del-tape.md) — la procedura standard di studio di una
seduta: i livelli in un file per giornata, spinti sul chart e letti dallo stesso file da
`watch_signals.py`, che valuta il tape sui livelli e riporta i segnali forti sul chart come linee
effimere.

Il file dei livelli e' **l'unica fonte**, letta sia da chi disegna sia da chi sorveglia. E ci sta
la regola che vale per qualunque filtro: **un filtro non provato non e' un filtro** — va fatto
passare sulle barre gia' note prima di armarlo, per vedere se ritrova cio' che avevi individuato a
mano e quanti segnali produce.

### 6. La giornata, mentre succede

[**`../giornate/come-si-scrive-una-giornata.md`**](../giornate/come-si-scrive-una-giornata.md) — un file per giornata di mercato, scritto
**durante** la seduta. Contiene la procedura per **riprendere a meta' sessione**: intestazione,
"Dove eravamo", correzioni, e solo dopo i dati nuovi.

Le letture sbagliate restano scritte con cio' che le ha smentite: e' la parte verificabile del
documento.

---

## Contratti Osservativi

Documenti che fissano *prima* cosa si registra e cosa no, cosi' che il risultato non possa essere
adattato all'ipotesi dopo averlo visto.

- [`contratto-osservativo-partecipazione.md`](contratto-osservativo-partecipazione.md)
- [`../sessioni/contratto-profilo-pre-sessione.md`](../sessioni/contratto-profilo-pre-sessione.md)

## Storia

[`storia-del-progetto.md`](storia-del-progetto.md) — cosa c'era prima della baseline attuale, e
dove sta la discontinuita'. Serve a non riusare soglie e risultati di esperimenti ritirati.
