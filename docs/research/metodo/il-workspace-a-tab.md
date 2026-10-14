# Il Workspace A Tab: Una Responsabilita' Per Tab

Dal 9 ottobre 2026 il workspace ATAS X **`Fabio workspace`** ha quattro tab, ognuna con una cosa
sola da dire, in ordine dal piu' lontano al piu' vicino. L'indicatore e' lo stesso dappertutto (**Fabio Data
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

## La Finestra ES

Dal 14 ottobre 2026 il workspace ha **una seconda finestra, ES**, con le stesse quattro tab nello
stesso ordine e con gli stessi indicatori: ES continuo (id ATAS 4936) per il composito, ESZ6 (34376)
per le altre tre. La chiede l'utente per lo studio delle opzioni: gli indicatori Options X-Ray di
ATAS disegnano solo su ES ([`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md)).
La finestra NQ non cambia.

Si costruisce con `FabioOrderFlow/tools/workspace_finestra_es.py --aspetta`, ad ATAS chiuso, con una
copia accanto (`Fabio workspace prima della finestra ES.aws`). Gli id dei contratti si leggono dal
workspace dopo averli aperti una volta in ATAS. Le tab degli indicatori opzioni sono nello script
(`--con-opzioni`) ma **spente**: prima i paper su opzioni e gamma exposure.

**Big trades a 60 anche su ES**: e' la taratura di Fabio per NQ; su ES, dove un contratto vale meno,
e' probabilmente bassa. Da studiare.

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

**Il POC sulle candele Daily e' delle 24 ore, quello della settimana della sola cassa.** Il 1 ottobre
2026: 30.820 contro 30.634. L'8 ottobre il minimo, 30.792, tocca il primo e non il secondo: le due
tab dicono cose diverse ed entrambe giuste. Per il metodo conta la cassa `[1 · 1:14:20]`.

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

**Com'e' andata davvero.** L'utente non ha aperto `Fabio NQ`: le quattro tab sono state applicate
direttamente a **`Fabio workspace`**, ad ATAS chiuso, con un backup accanto (`Fabio workspace prima
delle tab.aws`). E' il workspace in uso. Per controllarne il contenuto senza aprirlo:

```bash
python FabioOrderFlow/tools/workspace_a_tab.py --mostra "Fabio workspace"
```

## Vedere Le Tab Da Script

Tutto il modo in cui l'agente legge e comanda ATAS — dati, interfaccia, file, foto, e le regole di
riservatezza delle foto — sta in [`come-l-agente-vede-e-comanda-atas.md`](come-l-agente-vede-e-comanda-atas.md). Qui il riassunto.

```bash
python FabioOrderFlow/tools/foto_atas.py              # tutte le tab
python FabioOrderFlow/tools/foto_atas.py --tab 3      # solo la seduta
```

L'agente non puo' cliccare ATAS da fuori: ATAS gira con privilegi diversi, e computer use da
terminale su Windows non esiste. Lo fa l'indicatore da dentro, con `/ui` (`LInterfacciaDiAtas.cs`):
seleziona la tab sul thread dell'interfaccia, porta ATAS in primo piano e restituisce dove sta la
finestra. Lo script fotografa **solo la finestra di ATAS**, e **non salva niente** se in primo
piano c'e' un'altra finestra; alla fine rimette la tab e la finestra che c'erano prima.

`/ui` serve anche a esplorare: `?filtro=` cerca nei controlli, `?vm=` legge i view model,
`?descrivi=Tipo` elenca i membri di un tipo caricato e chi lo implementa.

## I Disegni Di ATAS Appartengono Allo Strumento

Il 10 ottobre 2026 le foto mostravano su ogni tab un pannello blu a sinistra, con VAH, VAL, vPOC e la
barra dei lotti: un profilo "settimana corrente" disegnato a mano su NQZ6, e un profilo fisso dal
24 aprile sul continuo. **Toglierli dal file del workspace non bastava**: su ATAS i disegni
appartengono allo strumento, e ATAS li rimette su ogni chart. Si tolgono da dentro:

```bash
curl "http://127.0.0.1:<porta>/disegni?chart=<id>&togli=CustomHistogramm"
```

Va fatto **su ogni chart**: ogni chart ne tiene la sua copia. Il profilo fisso del template del chart
(`Template.FixedProfile`) e' spento anche nel file, con `workspace_a_tab.py --ritocca "Fabio workspace"`
ad ATAS chiuso.

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
