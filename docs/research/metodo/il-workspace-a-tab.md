# Il Workspace A Tab: Una Responsabilita' Per Tab

Dal 9 ottobre 2026 il workspace ATAS X e' **`Fabio NQ`**: quattro tab, ognuna con una cosa sola da
dire, in ordine dal piu' lontano al piu' vicino. L'indicatore e' lo stesso dappertutto (**Fabio Data
Bridge**) e **sceglie da solo cosa disegnare dal timeframe del chart**.

Stato: **strumento di lettura**. Non contiene regole operative.

## Perche'

Fino all'8 ottobre c'erano due chart, e l'M1 portava tutto: le zone del valore, gli LVN incrociati,
i nodi di cinque giorni, il profilo settimanale di ATAS, il pannello. La richiesta dell'utente:
*"voglio dividere un po' tutto, con piu' tab con una responsabilita'... un workspace pulito diviso
per bene"*, con l'indicatore *"lo stesso, dinamico"*. E: *"i lvn per ora li voglio togliere,
voglio studiarli meglio dopo"*.

## Le Quattro Tab

| tab | chart | responsabilita' | fonte nel live |
|---|---|---|---|
| **1 COMPOSITO 90G** | NQ continuo, Daily | il profilo degli ultimi 90 giorni; **premium / fair value / discount** | `[4 · 14:18]`, `[5 · 55:05]` |
| **2 SETTIMANA M30** | NQZ6, M30 | **una gaussiana per ogni seduta cash**, lo spostamento del valore, quali sedute fanno il framing | `[1 · 22:13 - 24:35]`, `[1 · 1:14:20]` |
| **3 SEDUTA M1** | NQZ6, M1 | la seduta di adesso, PD e il giorno prima a destra, il pannello, footprint, big trades, speed of tape, DOM | `[1 · 1:24:04]`, `[4 · 11:57]` |
| **4 RANGE 40** | NQZ6, range 40 | lo stesso tape quando il mercato e' choppy: zone, big trades, delta cumulato. Niente pannello | `[2 · 1:16:00]`, `[2 · 1:29:52]` |

**Perche' M30 e non H1 per la settimana.** La cassa apre alle 13:30Z: una barra H1 delle 13:00Z
mette mezz'ora di premarket dentro la seduta. Le barre M30 cadono esattamente sull'apertura e sulla
chiusura, e la gaussiana e' quella della sola cassa.

**I big trades sono a 60** sull'M1 e sul range: *"the big trades guys are 60. The filter it's on
60"* `[2 · 1:29:47]`. Prima erano a 50.

## Il Ruolo, Dal Timeframe

`IlRuoloDelChart.cs`. L'impostazione **Role** vale `Auto`; gli altri valori forzano il ruolo.

```text
Daily e oltre           COMPOSITO    solo il composito
da M15 a H4             SETTIMANA    solo le gaussiane delle sedute
da M1 a M10             SEDUTA       zone, gaussiana di oggi, pannello, legenda; misura tape,
                                     muri, regole
range, tick, secondi    ESECUZIONE   zone e gaussiana di oggi, senza pannello e senza tape
```

**Solo la seduta misura.** Tape, muri, nodi e regole si calcolano su un chart solo, l'M1: tre
semine del tape e tre motori delle regole sullo stesso contratto sarebbero lavoro doppio, e numeri
che possono divergere.

**Chi chiede `NQZ6` al bridge riceve l'M1.** Con tre chart sullo stesso contratto il selettore per
strumento ne troverebbe tre: vince quello col ruolo seduta. Gli altri si raggiungono con l'id.

## Cosa Disegna Ogni Ruolo

**Composito.** Il profilo degli ultimi 90 giorni a destra, fasce da 5 punti, il blocco al 70% piu'
chiaro, il POC in oro; VAH, POC e VAL come righe dalla novantesima barra, che e' segnata con
`inizio 90G`. In alto a sinistra, due versioni:

- **grezzo, come le candele**: il continuo di ATAS non e' aggiustato al rollover, e il profilo
  disegnato deve combaciare con le candele del chart;
- **aggiustato al rollover**, per il contratto che si opera: i giorni fino all'ultimo rollover
  spostati dello scarto misurato contro l'M1 (metodo Panama, lo stesso degli LVN). Se l'M1 non c'e',
  il riquadro lo dice. **Quando le due versioni non concordano, vale l'aggiustata.**

Le barre Daily contengono **24 ore**: il composito comprende il globex. Si dichiara nel riquadro.
Il profilo fisso di ATAS che stava sul Daily (dal 24 aprile, cinque mesi e mezzo) e' stato tolto.

**Settimana.** Per ogni seduta cash, dalla piu' recente fino a `Sessions` (10): la gaussiana sulle
barre della seduta, fasce da 5 punti, il valore piu' marcato; VAH e VAL tratteggiati e il POC
pieno; sopra il massimo, giorno, valore, POC e **lo spostamento rispetto alla seduta prima**
(`valore SU` / `GIU`, `e staccato` se i due valori non si toccano — la stessa regola del giro).
I colori sono quelli delle scritte dell'M1: **PD**, il giorno prima, la seduta di **framing** quando
il prezzo e' uscito dal valore delle ultime due, **oggi in corso**; le altre grigie. La seduta in
corso si ridisegna ogni dieci secondi, barra aperta compresa.

**Seduta.** Quello che c'era il 1 ottobre (scritte PD / GIORNO / framing a destra, VAH POC VAL di
oggi come lineette a sinistra, pannello, legenda), **piu' la gaussiana della seduta in corso** sul
bordo sinistro, sotto le lineette: *"they are low in the curve"* `[1 · 1:24:28]` si legge sulla
forma. **Meno** gli LVN e i nodi di cinque giorni, spenti.

## Gli LVN E I Nodi Sono Spenti, Non Cancellati

`Show crossed LVN` e `Show nodes and gaps` valgono `false` di default e nel workspace. Il calcolo
resta nel codice, e lo studio in [`gli-lvn-incrociati.md`](gli-lvn-incrociati.md). Si riaccendono
dalle impostazioni dell'indicatore, quando l'utente avra' deciso come leggerli.

## Come Si Apre, E Come Si Rifa'

Il workspace si genera con uno script, partendo da quello che c'e' gia' (connettore, conto, colori,
footprint dell'utente) e cambiando solo cio' che il ruolo richiede:

```bash
python FabioOrderFlow/tools/workspace_a_tab.py          # da "Fabio workspace" a "Fabio NQ"
```

**Il workspace di partenza non si tocca.** ATAS tiene in memoria quello aperto e lo risalva da solo,
quindi scriverci sopra mentre e' aperto non serve: se ne scrive uno nuovo accanto, in
`%APPDATA%\ATAS X\Workspaces2\`, e si apre da ATAS (Workspace, apri, `Fabio NQ`). Il vecchio resta
come ritorno.

## `/ruolo`

`/ruolo?chart=<id|strumento>` restituisce il ruolo del chart e cio' che disegna, in numeri: il
composito grezzo e aggiustato, le sedute della settimana, o le zone della seduta. Serve all'agente
a leggere quei numeri invece di ricalcolarli, e a controllare che il chart mostri quello che crede.

Il 9 ottobre 2026, 17:50 italiane: composito di 90 barre dal 08/06, grezzo 28.395-30.330 POC
29.517,5; aggiustato (+299,25 fino all'11/09) **29.005-30.665, POC 29.817,5**. NQZ6 a 31.084:
**premium** in tutte e due.

## Cosa Non E' Stato Fatto, Dichiarato

- **Il delta cumulato di ATAS riparte dalla sessione di default**, non dall'apertura della cassa
  come il CVD del pannello. Le opzioni della sessione personalizzata non sono state toccate.
- **L'apertura della cassa e' un orario UTC fisso, 13:30Z.** Dal 1 novembre 2026, con l'ora solare
  americana, la cassa apre alle 14:30Z: zone, sedute e giro vanno spostati prima di allora, o
  tutti i profili di cassa sbaglieranno di un'ora.
