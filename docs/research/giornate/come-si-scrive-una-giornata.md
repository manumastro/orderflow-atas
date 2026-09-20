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
AAAA-MM-GG.md                                la cronaca, scritta durante la seduta
regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json  COME si trovano i livelli, non quali prezzi sono
annotazioni-AAAA-MM-GG.json                  il diario delle letture, con ora e misura
```

Il file delle regole **contiene regole, non livelli**, e per questo si chiama cosi': le deposita
`bridge.py rules` una volta e a risolverle e' l'indicatore, a ogni barra. Vedi
[`../metodo/i-livelli-li-calcola-l-indicatore.md`](../metodo/i-livelli-li-calcola-l-indicatore.md).
I vecchi `livelli-AAAA-MM-GG.json` e `scenari-AAAA-MM-GG.json` restano come evidenza delle fasi
chiuse e non si estendono.

Gli **scenari** si scrivono prima della seduta e dicono cosa ci aspettiamo: il motore li valuta e
quando uno scatta lo mette sul chart da solo, col nome che gli avevamo dato. A fine giornata si
guarda quali sono scattati e quali no — **uno scenario previsto e mai scattato dice qualcosa quanto
uno scattato**, e va scritto nella cronaca.

Le **annotazioni** sono il diario delle letture: e' da li' che si scrive la cronaca, invece di
ricostruirla a memoria.

Come si usano: [`../metodo/sorveglianza-del-tape.md`](../metodo/sorveglianza-del-tape.md).

### Il file delle regole

Accanto a `AAAA-MM-GG.md` sta `regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json`. Resta nel repository
perche' rileggere una giornata senza sapere quali livelli erano attivi non serve a niente — e
siccome contiene **regole**, non prezzi, rileggerlo dice anche *perche'* quei livelli erano li'.

Come si scrive: [`../metodo/i-livelli-li-calcola-l-indicatore.md`](../metodo/i-livelli-li-calcola-l-indicatore.md).

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

### In Replay Il Diario Torna Indietro Da Solo

Riavvolgendo un replay l'orologio di mercato torna indietro, **il file no**: i blocchi di un giro
precedente restano a raccontare minuti che nella sessione in corso non sono ancora successi, e si
leggono come contesto. La disciplina del replay e' non sapere come finisce, e un diario avanti la
toglie in silenzio.

`riavvolgi_giornata.py` sposta quei blocchi in fondo al file, sotto **«Il futuro di un giro
precedente»**, e gira da solo nell'hook del giro d'orizzonte. **Non cancella niente**: sono il
registro di un giro che c'e' stato davvero, e servono al confronto di fine sessione.

**Si sposta solo cio' che dichiara il proprio orario in apertura** — `**13:03Z — la mensola
cade.**`, `### 15:31 Dove eravamo`. Un blocco che si limita a *citare* un orario avanti resta
dov'e' e viene **segnalato**: in quel caso il problema non e' un pezzo di diario di troppo, e' una
misura presa su una finestra che finisce nel futuro del replay, e si corregge rifacendo la misura.

**Non torna avanti da solo.** Se il replay risupera un blocco parcheggiato, il blocco resta nel
parcheggio e il programma lo dice: rimetterlo dentro spaccerebbe il testo di un altro giro per la
cronaca di questo. Si fa a mano, con `--ripristina`.

**Una sezione svuotata va riscritta, e il programma dice quali.** Quello che resta sotto il titolo
sono le righe non datate, che descrivevano il momento portato via.

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

## Ogni Scenario Dice Anche Il Verso

Uno scenario che descrive e non conclude lascia senza risposta la domanda per cui era stato
scritto. Oltre a `quando`, `testo` e `attesa`, ogni scenario porta tre campi:

| campo | cosa contiene |
|---|---|
| `verso` | la conseguenza: `SHORT`, `LONG CONTRO GATE`, `NESSUN PERMESSO`, `FINE DELLO SHORT` |
| `implica` | cosa si cerca se scatta, e con quale vincolo |
| `altrimenti` | cosa significa il caso opposto, cioe' il ramo che non e' scattato |

Il `verso` compare sul chart fra parentesi quadre davanti al testo, cosi' si legge senza aprire
niente: `[SHORT] il VAL di lunedi respinge da sotto`.

**Il verso non e' un segnale approvato.** Nel progetto non esiste un modello attivo. E' la
conseguenza del **gate Tier 01 del dossier** — che e' una regola del corso — applicata alla
situazione della giornata, piu' il vincolo che ne deriva. Per questo esistono i due valori
scomodi: `LONG CONTRO GATE` dice che l'operazione e' possibile ma controcorrente e a rischio
stretto, e `NESSUN PERMESSO` dice che lo scenario e' un avviso e non un ingresso.
