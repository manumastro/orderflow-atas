# Il Giro D'Orizzonte: Cosa Si Guarda Prima Di Rispondere

Stato: **obbligo**, non procedura facoltativa. Vale **per ogni risposta**, non a inizio sessione.

Questo documento tiene il *perche'* e il *dettaglio*; l'obbligo in una riga sta in
[`../../../CLAUDE.md`](../../../CLAUDE.md).

---

## L'Obbligo

**Prima di qualunque analisi, anche la piu' piccola, si guarda tutto il contesto disponibile.**
Non le ultime dieci barre, non solo il livello di cui si sta parlando: **tutto** — lo stato del
bridge, dove eravamo, le correzioni gia' fatte oggi, il framing di ieri, cosa e' gia' scattato,
gli scenari armati, il tape con le finestre mobili, il profilo della seduta.

**Non dipende dalla fase di mercato.** In globex, a mercato quasi fermo, a cash chiusa e nel
weekend si riconsidera tutto esattamente come durante la cash. Una fase tranquilla non e' un
contesto piu' piccolo, e' lo stesso contesto con meno barre nuove — ed e' quando si e' piu'
tentati di rispondere a memoria.

**Una domanda breve non autorizza un contesto breve.** Le domande piu' corte — *"ha senso che
scenda?"*, *"cosa ne pensi?"* — sono quelle a cui si risponde piu' facilmente guardando lo schermo
invece dei dati, ed e' li' che si sbaglia.

**Questo non contraddice la brevita' della lettura dal vivo.** Il giro riguarda l'**input**: si
guarda tutto. La lettura riguarda l'**output**: si scrive poco, e si sceglie quel poco proprio
perche' si e' visto tutto. Vedi
[`come-si-risponde-dal-vivo.md`](come-si-risponde-dal-vivo.md).

---

## La Scala: Dal Piu' Lontano Al Piu' Vicino

**Dal 24 settembre 2026 il giro comincia dalla sezione 0**, e l'ordine non e' estetico: e' l'ordine
in cui una persona legge un mercato. Prima **le settimane** (dove si e' costruito il valore, e se
si sposta), poi **le sedute** una per una (dove chiude ciascuna rispetto al proprio valore, e dove
va il valore rispetto al giorno prima), poi **dove sta il prezzo adesso** rispetto a tutto questo,
poi **gli eventi** che hanno mosso quei giorni e quelli in arrivo. Solo dopo si scende alla notte,
all'Europa e al tape.

Fino al 23 il giro partiva da ieri. Quel giorno il bersaglio vero del ribasso stava due giorni
indietro, e il 24 la notte era uscita sotto un valore che coincideva per due sedute: cose che si
vedono solo guardando prima il quadro largo.

| blocco | cosa dice | da dove |
|---|---|---|
| le settimane | range, chiusura, POC e valore di ciascuna; se il valore sale o scende; le settimane con poco volume sono **quotazione, non valore** (il rollover) | bridge, tutte le ore |
| le sedute di cassa | apertura -> chiusura, POC, valore, delta; **chiude sopra/dentro/sotto il proprio valore**; valore **SU/GIU** rispetto al giorno prima, e **staccato** se le due aree non si toccano | bridge, finestra di cassa dello strumento |
| dove sta il prezzo | posizione nel range di venti sedute, distanza dal massimo, rispetto al valore della settimana scorsa e della cassa di ieri | bridge |
| la struttura oraria | i massimi e minimi di **swing H1** delle ultime 72 ore (un'ora piu' alta, o piu' bassa, delle due prima e delle due dopo), i quattro piu' vicini sopra e sotto; chiusura e delta delle ultime sei ore, e se fanno massimi e minimi crescenti o decrescenti | bridge |
| gli eventi | gli ultimi due giorni con l'**esito** sul mercato, e i prossimi due con ora e peso | `docs/research/calendario/eventi.json` |

Lo calcola `FabioOrderFlow/tools/scala_dei_tempi.py`. Le sedute chiuse si misurano una volta sola e
restano in una cache per strumento (`~/.fabio-scala-dei-tempi-<STRUMENTO>.json`); la prima volta
la cache si riempie a rate e il giro lo dice.

**Il calendario lo tiene l'agente**, e fa parte dell'apertura della giornata: si aggiungono gli
eventi dei prossimi giorni con ora italiana e peso (`alto`, `medio`, `basso`, `contesto`), e a
giornata chiusa si scrive l'**esito** di quelli passati — cosa ha fatto il mercato, con i numeri.
Un evento senza esito e' una previsione; con l'esito diventa memoria di come il mercato reagisce.

**Il giro deve stare sotto i 10 KB**, oltre i quali arriva all'agente tagliato e salvato a parte:
il 24 settembre e' successo, e per un messaggio l'agente ha visto solo l'inizio. Per questo la
vecchia sezione 4 (le tabelle del file di ieri) e' stata tolta — la sezione 0 misura le stesse
sedute dal bridge — e le regole con la finestra ancora vuota si contano invece di elencarle.

**E la risposta segue lo stesso ordine.** Si ragiona dal piu' lontano al piu' vicino anche quando si
scrive poco: una lettura dal vivo che parte dalle ultime barre senza sapere dove sta la settimana e'
la lettura che il giro esiste per impedire.

## Non Costa Niente: Arriva Da Solo

Un hook `UserPromptSubmit` in [`../../../.claude/settings.json`](../../../.claude/settings.json)
esegue [`../../../.claude/hooks/giro-orizzonte.sh`](../../../.claude/hooks/giro-orizzonte.sh) a
ogni messaggio e ne mette lo stdout nel contesto del prompt.

**Il giro automatico si legge, non si ignora.** Se il blocco manca — hook disattivato, sessione
diversa — si lancia a mano:

```bash
python3 FabioOrderFlow/tools/giro_orizzonte.py
```

Se dice che il bridge non risponde, **lo si dichiara nella risposta**: una lettura senza dati va
detta tale, non presentata come una lettura.

---

## Tre Cose Che Il Giro Impedisce, Tutte Gia' Successe

- **Dedurre invece di leggere.** Il 16 settembre l'agente ha detto che uno scenario non era
  scattato: era scattato otto minuti prima, e stava scritto nel diario. La sezione 5 lo mette
  davanti agli occhi.
- **Rispondere sulle ultime barre.** Una lettura costruita su dieci minuti ignora che il POC della
  seduta sta 100 punti sotto e che il delta a 60 minuti dice il contrario di quello a 15.
- **Ripetere un errore gia' corretto oggi.** La sezione 3 elenca le correzioni della giornata.

---

## Il Quadro: Framing E COT, Sempre, Anche Senza Che Lo Si Chieda

**Prima di qualunque lettura si fa il quadro**: dove si e' costruito il valore (profile framing) e
chi e' posizionato (COT). Le due meta' si leggono **insieme**, perche' e' la coppia a dire la cosa
che nessuna delle due dice da sola — **se il lato posizionato sta sopra o sotto il cuore del
volume**.

Procedura in
[`il-framing-e-il-cot-si-leggono-insieme.md`](il-framing-e-il-cot-si-leggono-insieme.md); le fonti
restano [`profile-framing.md`](profile-framing.md) e
[`analisi-istituzionale.md`](analisi-istituzionale.md), e la procedura COT esatta del live sta
alla **sezione 9** di [`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

**Non e' su richiesta, ed e' questo il punto.** Il quadro si scrive nel file della giornata fra
`<!-- QUADRO -->` e `<!-- /QUADRO -->`, e `giro_orizzonte.py` lo stampa alla **sezione 4bis** a
ogni messaggio. Se il blocco manca, il giro lo dichiara: **una lettura data con la 4bis vuota e'
una lettura costruita sulle ultime barre**.

### Tre Cose Che Il Quadro Contiene E Che Si Dimenticano

- **La scala dei POC**, non "il bersaglio". Ogni area di valore riattraversata regala il proprio
  POC come magnete. **Un massimo di seduta non e' un POC.** E il live ne vuole due: quello della
  singola seduta e **quello della seduta che ha creato la rottura**.
- **Il vuoto e da dove nasce**: un vuoto vero e' lo spazio fra il VAH di una seduta e il VAL di
  un'altra, non una fascia sottile del composito.
- **Quale campo del COT e' l'estremo.** Nel live si guarda **solo non-commercial, vista Legacy**,
  e **solo l'ultima variazione** decide chi ha il piede sull'acceleratore. Poi si prende la
  **data** e si torna sul **prezzo** di quella data: e' li' che il COT diventa un livello.

**Il quadro non contiene direzioni, ingressi, stop o bersagli operativi.** Dice dove si sta, non
cosa fare.

---

## Prima Di Chiedere Dati Al Bridge

Oltre al giro:

1. [`../giornate/`](../giornate/) — il file di **oggi**, se esiste. L'intestazione dice se e'
   `APERTO`; la sezione finale **"Dove eravamo"** da' lo stato all'ultimo aggiornamento; la
   sezione **"Le correzioni"** dice cosa e' gia' stato smentito oggi.
2. Il file del **giorno precedente**, per il framing: value area, POC e minimi di ieri stanno
   nella sua sezione 1.

Poi si chiedono al bridge solo i dati **successivi all'ultimo aggiornamento**, non tutta la
seduta.

Il file del giorno si aggiorna **durante** la seduta, non a posteriori: una giornata scritta alla
fine perde proprio le letture sbagliate, che sono la parte verificabile. Formato e regole in
[`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md).

**In replay il diario torna indietro da solo.** `riavvolgi_giornata.py` sposta in fondo al file i
blocchi datati dopo l'orologio del replay, e gira nello stesso hook del giro. Cosa sposta e cosa
invece si limita a segnalare sta in
[`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md).
