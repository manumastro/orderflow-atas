# Come Si Scrive Una Giornata

Una analisi per giornata di mercato, un file per giorno, `AAAA-MM-GG.md`.

Serve a togliere l'analisi del giorno dalla conversazione e dai file sparsi: quando a fine
settimana si guarda indietro, la domanda non e' *cosa abbiamo detto* ma **cosa abbiamo misurato e
cosa e' successo dopo**. Un documento per giornata rende quella verifica possibile.

## Cosa Ci Va, E Cosa No

| Ci va | Non ci va |
|---|---|
| I livelli con cui si e' aperta la giornata, e da dove vengono | Il commento a caldo |
| I numeri misurati, con l'ora accanto | Le impressioni non verificate |
| Cosa e' successo su ciascun livello | La previsione presentata come esito |
| Le correzioni fatte in giornata, con cosa le ha causate | Le correzioni nascoste |
| Cosa resta aperto a fine giornata | |

La regola generale del repository vale anche qui: **descrivere non e' validare**, e i risultati
negativi si registrano con la stessa cura di quelli positivi. Una lettura sbagliata in giornata
resta scritta, insieme a cosa l'ha corretta: e' la parte piu' utile del documento.

## Struttura Di Una Giornata

```text
1. Stato            strumento, sessione, chi ha prodotto i dati
2. Il contesto      cosa si sapeva prima dell'apertura: COT, composito, framing del giorno prima
3. I livelli        la lista spinta sul chart, con la derivazione di ciascuno
4. La cronaca       cosa e' successo, con volume e delta accanto a ogni momento
5. Le correzioni    cosa si e' letto male, e cosa l'ha smentito
6. Gli strumenti    monitor, soglie, script usati
7. Cosa resta       aperto a fine giornata
```

## Da Dove Arrivano I Numeri

Tutti dal Data Bridge, con ATAS aperto sul chart NQZ6 M1. Il client e' `bridge.py`; il contratto
e' in [`../metodo/contratto-data-bridge.md`](../metodo/contratto-data-bridge.md).

```bash
python3 FabioOrderFlow/tools/bridge.py candles --from 2026-09-15 --to 2026-09-16 --levels --out giorno.json
```

Le soglie usate in giornata (volume, delta, ampiezza) vanno **scritte accanto al numero che
producono**, mai lasciate implicite. Se una soglia viene dal p95 della distribuzione del giorno, si
dice che e' il p95 e di quante barre.

## I File Di Una Giornata

```text
AAAA-MM-GG.md              la cronaca, scritta durante la seduta
livelli-AAAA-MM-GG.json    i livelli attivi, derivati dall'analisi
scenari-AAAA-MM-GG.json    cosa ci aspettiamo, come condizioni valutabili
annotazioni-AAAA-MM-GG.json  il diario delle letture, con ora e misura
```

Gli **scenari** si scrivono prima della seduta e dicono cosa ci aspettiamo: il motore li valuta e
quando uno scatta lo mette sul chart da solo, col nome che gli avevamo dato. A fine giornata si
guarda quali sono scattati e quali no — **uno scenario previsto e mai scattato dice qualcosa quanto
uno scattato**, e va scritto nella cronaca.

Le **annotazioni** sono il diario delle letture: e' da li' che si scrive la cronaca, invece di
ricostruirla a memoria.

Come si usano: [`../metodo/sorveglianza-del-tape.md`](../metodo/sorveglianza-del-tape.md).

### Il file dei livelli

Accanto a `AAAA-MM-GG.md` sta `livelli-AAAA-MM-GG.json`: i livelli attivi quel giorno, nel formato
che il chart e la sorveglianza leggono entrambi. Resta nel repository perche' rileggere una giornata
senza sapere quali livelli erano attivi non serve a niente.

Come si usa: [`../metodo/sorveglianza-del-tape.md`](../metodo/sorveglianza-del-tape.md).

## Come Si Riprende Una Giornata Gia' Iniziata

Il caso normale non e' aprire un documento nuovo: e' **rientrare a meta' sessione**, o il giorno
dopo, e dover sapere cosa e' gia' stato guardato senza rileggere la conversazione. Per questo il
file del giorno va aggiornato **durante** la seduta, non scritto alla fine.

Nell'ordine:

1. **Leggi il file di oggi**, se esiste. L'intestazione dice se e' `APERTO` o chiuso, e a che ora
   e' stato aggiornato l'ultima volta.
2. **Vai in fondo, a "Dove eravamo".** E' lo stato compatto dell'ultimo aggiornamento: prezzo,
   range in corso, cosa regge da che lato, cosa sta girando. Serve a non ripartire da zero.
3. **Leggi la sezione delle correzioni.** E' li' che stanno le letture gia' smentite: ripeterle e'
   il modo piu' facile di sprecare la giornata.
4. **Solo dopo, chiedi i dati nuovi al bridge**, dall'ora dell'ultimo aggiornamento in poi.
5. **Scrivi prima di concludere.** Ogni volta che la seduta produce un fatto — un livello rotto,
   un assorbimento, una lettura smentita — va nella cronaca, e "Dove eravamo" si riscrive.

Il file del giorno prima si legge per la stessa ragione: il framing di oggi nasce dalla value area
e dai minimi di ieri, e quelli sono nella sua sezione 1.

### Le tre parti che esistono per essere rilette

| Sezione | A cosa serve quando rientri |
|---|---|
| **1. Il contesto** | i numeri di ieri e del COT, che non cambiano in giornata |
| **4. Le correzioni** | cosa e' gia' stato provato e si e' rivelato falso |
| **Dove eravamo** | lo stato al minuto dell'ultimo aggiornamento |

## Rapporto Con Gli Altri Documenti

- [`../metodo/profile-framing.md`](../metodo/profile-framing.md) — come si costruisce il framing.
  La giornata lo **applica**, non lo ridefinisce.
- [`../metodo/triple-aaa-dossier.md`](../metodo/triple-aaa-dossier.md) — il modello. La giornata
  cita il tier, non ricopia la regola.
- [`../metodo/livelli-sul-chart.md`](../metodo/livelli-sul-chart.md) — come i livelli arrivano sul
  chart. La giornata riporta **quali** livelli, non come si spingono.
- [`../sessioni/`](../sessioni/) — le descrizioni di settimana. Una settimana riassume piu'
  giornate; una giornata non riassume niente, e' la fonte.

## Le Giornate

| Giorno | Cosa e' successo |
|---|---|
| [2026-09-15](2026-09-15.md) | Lunedi' e' il primo giorno pieno su NQZ6. Il valore lascia il nodo dei compratori alle 06:21, il VAL di lunedi' cede alle 09:55, e la lettura del "vuoto" sotto il VAL si rivela sbagliata. |
