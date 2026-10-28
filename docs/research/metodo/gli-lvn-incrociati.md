# Gli LVN Incrociati

Stato: **descrizione**. Nessuna regola operativa; nessuna soglia qui sotto e' validata.

## Da Dove Viene

Il 30 settembre 2026 l'utente ha tracciato a mano, sul profilo della settimana, una zona a **30.511**
"un po' vuota di volume e gia' rispettata". Il prezzo ci ha fatto il minimo (30.512,25) e poi e'
salito di 400 punti. La richiesta: incrociare piu' profili per trovare queste zone da soli.

La fonte nel live e' il vuoto del profilo, `fabio_1.txt` 46:38-47:08: *"if we manage to break this
level, we have a gap. And this gap is the first level that the market will aggressively want to
rebalance ... market maker don't like inefficient transaction"*. Il composito e' quello del
template, *"the composite is the last three months, 90 days"* (`fabio_4.txt` 14:18). L'incrocio
fra profili e' dell'utente, non del live. Vedi anche [`profile-framing.md`](profile-framing.md),
sezione 6.

## Come Si Calcola

Codice: `FabioOrderFlow/src/Observation/GliLvnIncrociati.cs`. Tre profili, ricalcolati a ogni
barra chiusa:

| scala | finestra | grana | lisciatura | sottile se | lunghezza minima |
|---|---|---|---|---|---|
| `90G` | 90 giorni del chart NQ Daily continuo, aggiustato al rollover | 10 punti | ±3 fasce | < 0,6 x il massimo entro 300 punti | 30 punti |
| `SETT` | dal lunedi' (domenica 18:00 New York) | 5 punti | ±2 fasce | < 0,5 x il massimo entro 100 punti | 15 punti |
| `OGGI` | dalle 18:00 New York del giorno prima | 5 punti | ±1 fascia | < 0,5 x il massimo entro 60 punti | 15 punti |

**Il chart continuo non e' aggiustato.** Fino all'11/09/2026 riporta NQU6, 289-308 punti sotto
NQZ6. L'indicatore misura lo scarto da solo, confrontando le chiusure dei giorni presenti su
entrambi i chart (all'11/09: +299,25), e lo applica ai giorni fino al rollover.

**Il 30/09 all'apertura**: `90G` sottile 30.070-30.470, `SETT` 30.355-30.550, `OGGI` 30.510-30.565.
Il 30.511 dell'utente sta sul bordo alto del vuoto a 90 giorni, dentro quello della settimana e sul
fondo di quello della notte.

## Spenti Dal 9 Ottobre 2026

*"I lvn per ora li voglio togliere, voglio studiarli meglio dopo."* `Show crossed LVN` vale `false`
di default e nel workspace a tab ([`il-workspace-a-tab.md`](il-workspace-a-tab.md)): niente
rettangoli, e niente righe `LVN INCROCIATI` nel pannello per l'agente. Il calcolo e lo studio
restano; quello che segue descrive com'erano accesi.

## Cosa C'e' Sul Chart, Dal 1 Ottobre 2026

**Solo le zone della settimana**, come rettangoli dal primo minuto della settimana al bordo destro: **verdi sotto il prezzo, viola sopra** (se il prezzo e' dentro, conta la meta'). I colori sono quelli esatti dei rettangoli che l'utente disegna a mano, letti dal chart con `/disegni` il 1 ottobre 2026: verde 0,128,0, viola 83,55,117 col fondo 106,78,140. La prima versione disegnava
tre strisce per scala e poi le zone incrociate `LVN x2` / `x3`: per l'utente "non si capisce
niente". L'incrocio resta calcolato e arriva all'agente nella parte del pannello che non sta sul
chart (`/panel`, campo `analisi`, righe `LVN INCROCIATI`), che il giro stampa.

I vuoti a 5 giorni di `INodiEIVuoti.cs` non si disegnano piu' quando gli LVN sono accesi.

## Lo Studio, E Quanto Vale

`python FabioOrderFlow/tools/studio_lvn_incrociati.py` (bridge acceso, chart NQZ6 M1 e NQ Daily).
Per ogni cassa dal 15/09 al 30/09 calcola le zone all'apertura e guarda il primo ingresso del
prezzo in ogni tratto dove almeno due profili sono sottili. "Respinta" se torna di 20 punti dalla
parte da cui e' arrivato; "attraversata" se esce dall'altra.

Rifatto il 1 ottobre 2026 dopo l'audit: il lunedi' la settimana e' lo stesso profilo del giorno,
quindi ora si usa la settimana prima (e cosi' fa l'indicatore fino alla prima cassa chiusa).

| tratti | respinti | attraversati | nessun esito |
|---|---|---|---|
| sottili in 3 profili | 0 | **8**, in 0-4 minuti | |
| sottili in 2 profili | **19** | 11 | |
| zone del solo composito 90G, intere | 4 | 3 | 1 |

**Il composito da solo, caso per caso.** Il vuoto 30.070-30.480 (aggiustato al rollover):
attraversato dal basso il 21/09 in 51 minuti (la salita da 30.222 a 30.784); il 28/09 il prezzo ci
entra dall'alto alle 14:42Z, scende fino a 30.357 - 120 punti dentro, non sul bordo - e torna su; il
29/09 e il 30/09 i minimi si fermano a 30.504,5 e 30.512, sopra il bordo, senza entrarci: sono i
tocchi che l'utente ha visto come "rispettati". Lo studio conta solo gli ingressi, e quei due non lo
sono.

**Il cuore di un vuoto comune ai tre profili si attraversa; i suoi bordi respingono piu' spesso.**
E' coerente con Fabio (il vuoto si ribilancia in fretta) e con il 30.511 dell'utente (un bordo).

**Perche' non e' una regola:** 13 sedute; i tratti attaccati della stessa zona non sono eventi
indipendenti; le soglie (20 punti, grane, profondita') sono state scelte una volta e non provate;
molte "respinte" sono il primo minuto della cassa, dove tutto si muove.

## Leggere I Disegni Dell'Utente

`/disegni?chart=<id>` restituisce tutto cio' che e' disegnato sul chart, per reflection:
`ChartInfo.DrawingObjectsListInfo` espone solo linee e canali orizzontali, ma il suo campo privato
`DrawingObjectsCollection` ha `GetAllObjects(Instrument)`, che restituisce anche rettangoli
(`RectangleObj`: `TrueFirstPrice`, `TrueSecondPrice`, `_firstTime`, `_secondTime`, `Color`,
`AreaColor`) e profili fissi (`CustomHistogramm`, con `ProfilePeriod`). Il 1 ottobre 2026 c'erano un
rettangolo viola 30.686-30.698,75, due verdi 30.499-30.511,75 e 30.443,75-30.454,50, una linea
verde a 30.684 e il profilo `CurrentWeek`. E' un'API interna di ATAS: puo' cambiare con un
aggiornamento senza avviso.
