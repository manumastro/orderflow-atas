# Le Gambe Di Una Seduta

Come si misura **quanto una giornata ha davvero offerto**, e perche' la misura ovvia — la piu'
grande escursione in una finestra di tempo — risponde a una domanda diversa da quella che
interessa.

Nato il 16 settembre 2026 su NQZ6, da un errore di misura che l'utente ha trovato guardando il
grafico.

---

## L'Errore Che L'Ha Prodotto

Per rispondere a *"quali sono stati i movimenti migliori oggi?"* avevo misurato, per ogni barra,
il **massimo raggiunto nei 30 minuti successivi meno l'apertura di quella barra**, e avevo
riportato il piu' grande: *"14:40Z, +97,50 punti"*.

L'utente ha risposto: **«alle 14:40 non vedo tutti quei punti in salita»**. Aveva ragione. Il
percorso reale era:

```
14:40 -> 14:45   29.438 -> 29.475    +37
14:45 -> 14:49   29.475 -> 29.451    -24
14:49 -> 14:56   laterale 29.450-29.470, sette minuti
14:57 -> 15:00   29.467 -> 29.514    +47
15:02            ricaduta a 29.485   -29
15:07 -> 15:09   29.501 -> 29.535    +34
```

Quattro spinte e due ricadute. L'aritmetica era giusta — da 29.438,25 il massimo entro trenta
minuti era 29.535,75 — ma il numero **non descriveva una salita**.

**Cosa misurava davvero:** l'inviluppo dell'opportunita', cioe' il risultato di chi indovina ogni
giro e non tiene mai una posizione attraverso un ritracciamento. E' un limite superiore teorico,
non qualcosa che qualcuno possa prendere.

**Il danno vero non e' il numero, e' la conclusione che ci avevo costruito sopra.** Avevo scritto
che le barre 14:36-14:40 erano «il Triple AAA da manuale» e che quella sequenza aveva prodotto
sessantasette punti. Falso: dopo la presunta aggressione delle 14:40 sono arrivati **dodici minuti
di laterale con quattro barre di delta negativo**. La sequenza non ha prodotto niente. Avevo
attaccato una spiegazione del corso a un movimento che la metrica aveva inventato.

---

## La Misura Giusta

Una gamba e' un movimento che **si puo' tenere**. Serve quindi un vincolo sul ritracciamento, non
solo sulla durata:

> Una gamba dura finche' il prezzo non ritraccia piu' di **R** punti dal proprio estremo. Quando
> ritraccia piu' di R, la gamba e' finita, e quello che viene dopo e' un'altra gamba.

**R si sceglie dallo strumento, non si copia.** Il criterio: due o tre barre M1 tipiche, cosi' che
il respiro normale non chiuda la gamba ma un'inversione vera si'.

```text
NQZ6    barra M1 tipica ~5 punti     ->  R = 12 punti
ESZ6    barra M1 tipica ~1,25 punti  ->  R = 3 punti
MCLV6   barra M1 tipica ~0,10 $      ->  R = 0,25 $
```

E' la stessa famiglia di costanti nascoste elencata in
[`come-si-apre-un-asset.md`](come-si-apre-un-asset.md): **R e' un parametro, non un numero.**

Si tiene poi solo cio' che supera una soglia minima di ampiezza (su NQ: 25 punti), e fra gambe
sovrapposte si tiene la piu' grande.

### Cosa e' venuto fuori il 16 settembre

Cash 13:30-16:47Z, R = 12 punti, soglia 25:

| partenza | arrivo | punti | durata | volume | delta |
|---|---|---|---|---|---|
| 13:45Z | 13:46Z | 54,25 | 2 min | 5.557 | +619 |
| 14:13Z | 14:19Z | 59,00 | 7 min | 6.975 | +463 |
| 14:55Z | 15:00Z | 54,00 | 6 min | 6.173 | +573 |
| 15:06Z | 15:09Z | 44,00 | 4 min | 5.773 | +163 |
| 15:42Z | 15:47Z | 64,75 | 6 min | 6.918 | +436 |

**Cinque gambe, tutte fra 44 e 65 punti, tutte fra 2 e 7 minuti, tutte attorno ai 6.000 lotti.**
Nessuna gamba al ribasso ha superato la soglia: la giornata ha sceso solo per ritracciamento.

Quella uniformita' e' il risultato utile della misura. Dice che su questo strumento, in questa
giornata, **l'unita' di movimento era ~55 punti in ~5 minuti con ~6.000 lotti** — che e' una
informazione operativa (quanto dura una gamba, quanto vale) che la metrica sbagliata nascondeva
dentro un singolo numero da 97,50.

---

## Il Risultato Negativo Che Ne E' Uscito

Guardando la barra immediatamente precedente a ciascuna delle cinque gambe, tutte e cinque avevano
la stessa firma: **delta negativo e chiusura nel 15% basso del range** — uno sfogo venditore che
chiude sul minimo, seguito subito dalla salita.

Cinque su cinque sembra un segnale. **Non lo e'.**

Contate **tutte** le barre della cash con quella firma: sono **19**, e solo **8** hanno prodotto
almeno 25 punti di salita nei 15 minuti successivi. **Il 42%.**

```text
19 segnali   8 salgono   5 scendono   6 niente
```

Sette degli undici fallimenti stanno dopo le 15:54Z, cioe' dopo il massimo di giornata — il che
suggerisce un filtro orario. **Ma quel filtro lo si vede solo sugli stessi dati che hanno prodotto
il pattern**, ed e' esattamente cosi' che si costruisce un edge falso.

**La lezione generale, che vale oltre questo caso:**

> Un pattern trovato leggendo **all'indietro** dai movimenti buoni ha, per costruzione, il 100%
> di successo sul campione da cui e' stato estratto. Il numero che conta e' quante volte la stessa
> firma compare **senza** essere seguita dal movimento. Finche' non si conta quello, non si e'
> trovato niente: si e' descritto il campione.

Il pattern resta **archiviato come negativo** e non va armato. La verifica sui 40 giorni caricati
nel bridge e' **lavoro aperto**, e finche' non e' fatta il 42% e' il rumore di una giornata sola,
non una stima.

---

## La Deroga: Operare Prima Del Gate

**Richiesta esplicita dell'utente, 16 settembre 2026**, ripetuta due volte: si opera anche prima
che il gate Tier 01 si sia aperto.

**Cosa dice la fonte.** Il dossier alle righe 119-124 non concede setup direzionali prima della
prima chiusura M30 fuori dall'IVB: `INSIDE RANGE -> mean reversion only, or stand aside`. Alla
riga 129: *«The IVB doesn't tell you where to enter. It tells you which way you're allowed to
enter.»* Prima del gate, quel "which way" non esiste ancora.

**Perche' la deroga e' ragionevole, dai dati.** Il 16 settembre **tre gambe su cinque sono nate
prima che il gate si aprisse** (13:45Z, 14:13Z, 14:55Z contro un gate alle 15:00Z). Chi aspetta il
permesso, quel giorno, guarda passare meta' della giornata — e la barra che ha dato il permesso
(14:59Z, 1.993 lotti, delta +395) era **l'ultima barra della terza gamba, non la prima**. Il gate
ha certificato un movimento gia' avvenuto.

Non e' un difetto del dossier: e' il costo di un filtro che richiede una chiusura a 30 minuti per
pronunciarsi. Va conosciuto in anticipo, non scoperto il giorno che si perde il movimento.

**Cosa cambia operativamente, e va scritto accanto a ogni lettura pre-gate:**

| | prima del gate | dopo il gate |
|---|---|---|
| **permesso direzionale** | **non esiste** | long o short, dal lato della rottura |
| **su cosa si giudica** | solo il flusso: delta, volume, posizione nel range, footprint | flusso **piu'** coerenza col permesso |
| **come si etichetta** | `PRE-GATE · NESSUN PERMESSO` | il verso, tracciabile alla riga della fonte |
| **rischio** | il vincolo del mean reverting resta il riferimento piu' stretto disponibile: **max 2 stop al giorno** (dossier 112-114) | quello del setup |

**Tre obblighi che la deroga non sospende:**

1. **Il `verso` di uno scenario pre-gate resta `NESSUN PERMESSO`.** Non si puo' scrivere `LONG` su
   una condizione che il dossier non autorizza: il campo `verso` dice cosa la **fonte** concede,
   non cosa si ha intenzione di fare. La deroga vive nella lettura, non nel file degli scenari.
2. **Le sei prove** di [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) valgono immutate. Un
   ingresso pre-gate ha un sosia esattamente come gli altri, e senza permesso direzionale il sosia
   e' **piu'** pericoloso, non meno: manca la cosa che altrimenti lo escluderebbe.
3. **Si dichiara che e' pre-gate**, ogni volta, accanto alla lettura. Il giorno che una serie di
   letture pre-gate va male, deve essere possibile separarle da quelle che venivano dal corso.

**Gerarchia.** Questa e' una deroga dell'utente, e sta sotto al dossier nella gerarchia delle fonti
di `CLAUDE.md`. Se una analisi futura mostra che le letture pre-gate perdono sistematicamente, e'
la deroga a cadere, non il dossier.

---

## Come Si Usa

```bash
# le gambe della cash di oggi su NQ
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 --from 2026-09-16T13:30
```

Poi si applica il vincolo R sullo strumento giusto. Il calcolo non e' in uno strumento dedicato:
finche' e' una misura che si fa a mano su una giornata, resta nell'analisi — la stessa regola per
cui la derivazione dei livelli non sta dentro l'indicatore
([`livelli-sul-chart.md`](livelli-sul-chart.md)).

**Quando si misura.** A seduta chiusa, nel bilancio della giornata. Non dal vivo: una gamba si
riconosce solo quando e' finita, e chiamarla mentre si forma e' la definizione del senno di poi
che la correzione 4.3 del 16 settembre ha registrato.

Caso applicato: [`../giornate/2026-09-16.md`](../giornate/2026-09-16.md), sezione *"Le gambe della
seduta"*.
