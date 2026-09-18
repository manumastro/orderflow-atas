# Il Framing E Il COT Si Leggono Insieme

Stato: **procedura obbligatoria di contesto.** Non produce segnali e non autorizza esecuzioni.
Produce il **quadro**: dove si sta e chi e' posizionato. Va fatta **prima** di qualunque lettura, e
il suo risultato va messo davanti agli occhi **anche quando nessuno lo chiede**.

Le due meta' esistono gia' e restano le fonti:

- [`profile-framing.md`](profile-framing.md) — la **location**: dove si e' costruito il valore, i
  POC, i bordi, i vuoti. Sei passi, dal primo live del corso.
- [`analisi-istituzionale.md`](analisi-istituzionale.md) — il **posizionamento**: chi ha il libro,
  con che ritardo, e se il livello e' estremo o il flusso e' notevole.

Questo documento dice **perche' vanno lette insieme**, **cosa dice la coppia che nessuna delle due
dice da sola**, e **come si scrive il risultato** perche' il giro d'orizzonte lo ripeta a ogni
messaggio.

---

## La Fonte: L'Accoppiata E' Di Fabio, Non Nostra

Tutto quello che c'e' qui sotto discende da un unico passo del primo live, dove Fabio spiega il COT
e poi dice **da solo** che serve accoppiato al profilo. Non e' una nostra sintesi di due documenti
separati: e' il modo in cui il corso lo insegna.

    sed -n '1130,1200p' fabio_course/fabio_q1/fabio_live_charting_1_q1.txt

> *"I only check this when a new release comes out. I'm explaining to you because this one, it's
> extremely useful when you align it with **the profile framing**."*
> — righe **1137-1143**

E subito dopo il meccanismo, per esteso:

> *"When we see that they are distributing order from the 13th of January, and **we go back to
> price**, and we see that **the 13th of January was exactly on the value area**, now this
> rejection at the top of the value area ... they start to make sense."*
> — righe **1162-1183**

> *"And the reason people lose money by using COT because **they don't understand it** is that once
> you have a framework for the indices ... you can cross confirm the S&P 500, the NASDAQ and the
> Dow Jones and the Russell."*
> — righe **1187-1197**

Il resto delle due meta' sta dove stava gia': il blueprint COT (Tradingster, vista legacy, "follow
institutions = non-commercial and asset managers") alle righe **955-1070**, il profile framing
nel suo documento.

### Il Passo Di Fabio Non E' Quello Che Avevamo Scritto

**La procedura del transcript e': si prende la data del rilascio COT, si torna sul grafico a quella
data, e si guarda dove stava il prezzo rispetto alla value area di allora.** La data del
posizionamento diventa **un posto sul profilo**. Il 13 gennaio l'istituzionale distribuiva *ed era
esattamente sulla value area*: e' la coincidenza fra le due cose a dare il senso, non nessuna delle
due.

La formulazione scritta piu' sotto — *"il lato posizionato sta sopra o sotto il cuore del
volume"* — e' una **nostra derivazione**, utile e misurabile, ma **non e' il passo del corso**.
Le due vanno tenute distinte: la prima e' tracciabile alle righe 1162-1183, la seconda no.

**Cosa ne consegue, e cosa ha prodotto sull'oro.** Il quadro diceva dove sta il prezzo **oggi**
rispetto al vertice del composito, e non **dove stava l'8 settembre** — la data dei dati COT —
rispetto alla value area di quel giorno. Fatto il conto (GCZ6, cash 13:30-20:00Z, 57.791 lotti):

```text
  8 set   O 4.444,9   H 4.454,5   L 4.401,1   C 4.407,3   delta -1.467
          POC 4.438,8   valore 4.427,1 - 4.450,6
          la seduta rifiuta il massimo e CHIUDE SOTTO IL PROPRIO VAL
  COT     nella stessa settimana i Non-Commercial aggiungono +3.836 di netto long
```

**Gli istituzionali compravano in una seduta che rompeva.** E' il rovescio dell'esempio del 13
gennaio nel transcript, dove la distribuzione coincideva col bordo alto del valore e il prezzo poi
lo rifiutava.

**Il prodotto operativo del passo e' un livello, non una tesi**: `VAL 4.427,1` — il prezzo sotto
cui, l'8 settembre, la seduta ha chiuso mentre il posizionamento veniva misurato. E' il livello che
Fabio disegna quando dice *"we go back to price"*, e va sul chart come gli altri.

**E manca il cross confirm.** Righe 1187-1197: il COT di uno strumento si conferma incrociando i
correlati. Per gli indici sono S&P, NASDAQ, Dow, Russell — la verifica e' gia' fatta in
[`../cot/tradingster-cross-index-2026-09-11.md`](../cot/tradingster-cross-index-2026-09-11.md).
**Per l'oro l'incrocio non e' stato fatto**: argento, e il dollaro.

---

## Perche' Insieme

Le due misure rispondono a domande diverse su orologi diversi, e **non vanno fuse in un
punteggio** — la regola sta in `analisi-istituzionale.md` e resta valida. Ma messe una accanto
all'altra fanno una cosa che nessuna delle due fa da sola: **dicono se chi e' posizionato sta
comodo o scomodo.**

```text
  il framing dice   DOVE si e' scambiato di piu', e dove il prezzo sta adesso rispetto a li'
  il COT dice       CHI e' lungo e chi e' corto, e se e' un estremo storico
  insieme dicono    se quel lato e' sopra o sotto il prezzo
```

Un netto speculativo lungo non dice niente. Un netto speculativo lungo **mentre il cuore del
volume sta sotto il prezzo** dice che quel lato e' in guadagno e non ha fretta. Lo stesso netto
**con il volume sopra il prezzo** dice l'opposto.

### Il caso che ha prodotto questo documento

18 settembre 2026, GCZ6 ([`../giornate/GCZ6-2026-09-18.md`](../giornate/GCZ6-2026-09-18.md)).
**Tre misure indipendenti, tre strade diverse, una sola conclusione:**

```text
  COT        short Non-Commercial 29.047 = 1 percentile su dieci anni
             (solo tre settimane peggiori in 520, tutte marzo-maggio 2020)
  composito  vertice 4.400-4.410, 102.167 lotti, delta -1.875
             e il prezzo trenta punti SOPRA
  footprint  fra le 04Z e le 06Z il prezzo sale 27 punti con delta -194
```

**Chi ha venduto l'oro nelle ultime tre settimane e' sotto**, e lo dicono il posizionamento
settimanale, il profilo di tredici sedute e la footprint di stanotte. Nessuna delle tre, da sola,
avrebbe retto: il COT e' vecchio di dieci giorni, il composito non dice quando, la footprint non
dice quanto in grande.

**Cosa la convergenza NON autorizza.** Resta contesto. Tre misure che concordano non sono un
segnale: il `verso` degli scenari resta `NESSUN PERMESSO`, la direzione della lettura dal vivo
resta quella del tape, e nessuna soglia COT e' mai stata validata su questo repository (la
[verifica cross-index](../cot/tradingster-cross-index-2026-09-11.md) non ha trovato separazione
nella risposta di prezzo). **La convergenza cambia la fiducia in una lettura, non la crea.**

---

## Cosa Guardare, In Quest'ordine

### 1. Il framing, per primo

Tutti e sei i passi di [`profile-framing.md`](profile-framing.md). Il primo passo — **quale
finestra cash** — non e' scontato fuori da NQ: sull'oro, il 16 settembre 2026, due finestre
plausibili hanno prodotto **shift opposti** (`13:30-20:00Z` → "dentro", `13:30-18:30Z` → "su").
**La finestra si dichiara accanto al numero**, e se non discende da una riga della fonte si dice
anche quello.

### 2. La scala dei POC, che e' il pezzo che si dimentica

> *"When you enter back in the value area and you have aggressive trades and speed of tape, you
> don't have a wall on the top. So at least to the POC, have a huge probability of arriving."*
> — `fabio_course/fabio_q1/fabio_live_charting_1_q1.txt`, righe 570-572

**Ogni area di valore riattraversata regala il proprio POC come magnete.** Sopra e sotto il prezzo
non c'e' "un bersaglio", c'e' una **scala**: si elencano i POC delle sedute precedenti con la loro
value area, in ordine di distanza.

**L'errore da non rifare**, fatto il 18 settembre: usare un **massimo** di seduta al posto di un
**POC**. Un massimo dice dove il prezzo si e' fermato, un POC dove si e' scambiato di piu'. Su
GCZ6 il bersaglio scritto era 4.479 (massimo del 10 settembre) invece di 4.480 (POC del 4
settembre): un punto di differenza, ma il primo non e' un magnete e il secondo si'.

### 3. I vuoti, e da dove nascono

Un vuoto vero e' **lo spazio fra due aree di valore**, non una fascia sottile del composito.
Verifica: il VAH della seduta sotto e il VAL della seduta sopra. Se combaciano con la fascia
sottile, e' un gap; se no, e' solo un prezzo che poche sedute hanno visitato — e allora vale la
regola gia' scritta in `profile-framing.md`: **si guarda il profilo della seduta piu' recente che
quel prezzo l'ha visitato.**

Esempio misurato: su GCZ6 il vuoto 4.490-4.510 e' esattamente VAH del 4 settembre (4.490) contro
VAL del 3 (4.510), con **6,6 punti fra il massimo del 4 e il minimo del 3 che la cash non ha mai
scambiato**.

### 4. Il COT, con le tre dichiarazioni

Codice, vista, variante, data del report, **ritardo quantificato in prezzo**. Poi la distinzione
che fa il lavoro:

- **flusso** = quanto si e' mosso nell'ultima pubblicazione, come percentile delle variazioni;
- **livello** = dove sta il netto nella propria distribuzione, con `z52` e percentile.

**E il campo giusto.** Il netto e' quasi sempre **il meno informativo dei tre**: su GCZ6 il netto
stava al 70° percentile — ordinario — mentre lo **short** stava al **1°**. Guardare solo il netto
avrebbe prodotto la conclusione giusta per la ragione sbagliata, che poi e' il modo di sbagliarla
la volta dopo.

### 5. La domanda finale, che e' una sola

> **Il lato che il COT dice posizionato sta sopra o sotto il cuore del volume?**

Si risponde con due numeri: il vertice del composito, e il prezzo. Nient'altro.

---

## Come Si Scrive, E Perche' Cosi'

Il quadro va nel file della giornata **dentro due marcatori**:

```text
<!-- QUADRO -->
...il quadro, al massimo 26 righe...
<!-- /QUADRO -->
```

`FabioOrderFlow/tools/giro_orizzonte.py` legge quel blocco e lo stampa alla **sezione 4bis**, a
ogni messaggio, automaticamente. **E' il meccanismo per cui il quadro viene consultato anche
quando nessuno lo chiede**, che e' l'unico modo perche' serva davvero: una procedura che si apre
solo su richiesta e' una procedura che non si apre il giorno in cui serviva.

**Se il blocco manca, il giro lo dice** e stampa il percorso di questo documento. Una lettura data
con la sezione 4bis vuota e' una lettura costruita sulle ultime barre, ed e' esattamente l'errore
che il giro d'orizzonte esiste per impedire.

**Cosa ci va dentro**, in ordine, stretto:

```text
  finestra cash usata, dichiarata
  valore di ieri: POC, VAL, VAH + shift rispetto all'altro ieri
  vertice del composito, con il suo delta, e dove sta il prezzo rispetto a li'
  la scala dei POC sopra e sotto, con le distanze
  i vuoti, con i due bordi che li generano
  COT: data, ritardo, il campo estremo (non il netto per forza), livello e flusso
  la riga di sintesi: chi e' posizionato sta sopra o sotto il volume
```

**Cosa non ci va:** direzioni, ingressi, stop, bersagli operativi. Il quadro dice **dove si sta**,
non **cosa fare**. Il "cosa fare" e' la lettura dal vivo, e ha il suo formato in `CLAUDE.md`.

---

## Quando Si Rifa

- **A ogni apertura di giornata**, prima di qualunque livello sul chart.
- **Quando esce un COT nuovo** — venerdi 21:30 italiane, dati del martedi.
- **Quando il prezzo esce dal valore** in cui stava quando il quadro e' stato scritto: da quel
  momento la riga sulla posizione nella curva e' falsa.
- **A ogni rollover**, perche' i profili storici cambiano prezzo.

**Non si rifa** a ogni lettura: e' contesto, cambia lentamente, e ricalcolarlo continuamente lo
rende rumore come qualunque altra misura fatta troppo spesso.

---

## Cosa Resta Aperto

1. **La finestra cash fuori da NQ** non discende da una riga della fonte. Su GCZ6 e' stata scelta
   sul volume (crollo alle 20Z, nessun gradino alle 18:30Z) e la scelta e' dichiarata, ma va
   decisa una volta e scritta in `profile-framing.md`.
2. **Nessuna soglia COT e' validata.** La convergenza fra framing e COT descritta qui e' stata
   osservata **una volta**. Perche' diventi una misura serve contare, su una serie, cosa e'
   successo dopo le altre occorrenze — sulla serie oro ci sono 520 settimane e il conto **non e'
   stato fatto**.
3. **La vista TFF/Disaggregated in serie** non e' estratta per l'oro: i numeri dell'8 settembre
   sono stati verificati contro la Legacy (scarto 36 contratti su 3.836) ma senza serie non si
   calcolano `z52` e percentile su quella tassonomia.
