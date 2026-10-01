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
| `SETT` | dal lunedi' (domenica 22:00Z) | 5 punti | ±2 fasce | < 0,5 x il massimo entro 100 punti | 15 punti |
| `OGGI` | dalle 22:00Z | 5 punti | ±1 fascia | < 0,5 x il massimo entro 60 punti | 15 punti |

**Il chart continuo non e' aggiustato.** Fino all'11/09/2026 riporta NQU6, 289-308 punti sotto
NQZ6. L'indicatore misura lo scarto da solo, confrontando le chiusure dei giorni presenti su
entrambi i chart (all'11/09: +299,25), e lo applica ai giorni fino al rollover.

**Il 30/09 all'apertura**: `90G` sottile 30.070-30.470, `SETT` 30.355-30.550, `OGGI` 30.510-30.565.
Il 30.511 dell'utente sta sul bordo alto del vuoto a 90 giorni, dentro quello della settimana e sul
fondo di quello della notte.

## Cosa C'e' Sul Chart, Dal 1 Ottobre 2026

**Solo le zone della settimana**, come bande rosa con `LVN` a destra. La prima versione disegnava
tre strisce per scala e poi le zone incrociate `LVN x2` / `x3`: per l'utente "non si capisce
niente". L'incrocio resta calcolato e arriva all'agente nella parte del pannello che non sta sul
chart (`/panel`, campo `analisi`, righe `LVN INCROCIATI`), che il giro stampa.

I vuoti a 5 giorni di `INodiEIVuoti.cs` non si disegnano piu' quando gli LVN sono accesi.

## Lo Studio, E Quanto Vale

`python FabioOrderFlow/tools/studio_lvn_incrociati.py` (bridge acceso, chart NQZ6 M1 e NQ Daily).
Per ogni cassa dal 15/09 al 30/09 calcola le zone all'apertura e guarda il primo ingresso del
prezzo in ogni tratto dove almeno due profili sono sottili. "Respinta" se torna di 20 punti dalla
parte da cui e' arrivato; "attraversata" se esce dall'altra.

| tratti | respinti | attraversati |
|---|---|---|
| sottili in 3 profili | 1 | **9**, in 0-5 minuti |
| sottili in 2 profili | **18** | 10 |

**Il cuore di un vuoto comune ai tre profili si attraversa; i suoi bordi respingono piu' spesso.**
E' coerente con Fabio (il vuoto si ribilancia in fretta) e con il 30.511 dell'utente (un bordo).

**Perche' non e' una regola:** 13 sedute; i tratti attaccati della stessa zona non sono eventi
indipendenti; le soglie (20 punti, grane, profondita') sono state scelte una volta e non provate;
molte "respinte" sono il primo minuto della cassa, dove tutto si muove.
