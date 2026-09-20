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
