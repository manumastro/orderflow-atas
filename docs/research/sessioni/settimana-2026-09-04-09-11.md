# Settimana NQ 2026-09-04 / 2026-09-11: Descrizione Della Sessione Cash

Strumento: NQU6@CME, scadenza 2026-09-18. Fonte: Fabio Data Bridge, chart M1, footprint per barra.
Finestra cash: 09:30-16:00 America/New_York. Stato: descrizione. Nessun segnale, nessuna regola approvata.

## Profili Di Sessione

Volume per prezzo aggregato da tutte le barre della cash, POC come livello di volume massimo, area di valore al 70% espansa dal POC.

| Sessione | Volume | Delta | POC | VAL | VAH | High | Low | Open | Close |
|---|---|---|---|---|---|---|---|---|---|
| 2026-09-01 | 395.195 | +6.317 | 29.125 | 29.082 | 29.212 | 29.318 | 29.002 | | 29.124 |
| 2026-09-04 | 346.423 | -15.963 | 29.520 | 29.482 | 29.590 | 29.692 | 29.468 | 29.588 | 29.560 |
| 2026-09-07 | 24.852 | +30 | 29.608 | 29.594 | 29.630 | 29.645 | 29.564 | 29.615 | 29.604 |
| 2026-09-08 | 356.350 | -8.308 | 29.610 | 29.514 | 29.631 | 29.686 | 29.425 | 29.644 | 29.538 |
| 2026-09-09 | 344.709 | +3.089 | 29.443 | 29.427 | 29.519 | 29.594 | 29.358 | 29.455 | 29.455 |
| 2026-09-10 | 388.337 | +1.205 | 29.160 | 29.106 | 29.222 | 29.275 | 29.058 | 29.090 | 29.143 |

Il 7 settembre e' il Labor Day: 24.852 contratti contro una media di circa 360.000, quindi il suo profilo non e' comparabile con gli altri e non va usato come riferimento.

## Migrazione Del Valore

Il fatto strutturale della settimana e' una discesa ordinata del valore su tre sessioni consecutive:

```text
POC   29.610  (08 set)  ->  29.443  (09 set)  ->  29.160  (10 set)
```

Ogni sessione si e' aperta **sotto l'area di valore della precedente**:

- 09 set: apertura 29.455, sotto la value area 29.514-29.631
- 10 set: apertura 29.090, sotto la value area 29.427-29.519

Non c'e' stata nessuna sessione che abbia riportato il valore verso l'alto. Nella terminologia del corso e' migrazione del valore verso il basso, non un singolo movimento direzionale.

## Sforzo E Risultato

Le tre sessioni piene raccontano tre cose diverse, e il delta da solo non basta a distinguerle:

- **08 settembre**: delta -8.308 con chiusura a 29.538 contro apertura 29.644. Venditori aggressivi e ricompensati.
- **09 settembre**: delta **+3.089** con chiusura identica all'apertura (29.455) e POC sceso da 29.610 a 29.443. Compratori aggressivi che non ottengono risultato: il valore scende comunque.
- **10 settembre**: delta +1.205, apertura 29.090 sul minimo dell'area, chiusura 29.143. Volume piu' alto della settimana, 388.337, sul profilo piu' basso.

Il 9 settembre e' il caso piu' interessante: e' la configurazione che il corso descrive come sforzo senza risultato, osservata qui in modo misurabile invece che a occhio.

## Partecipazione Di Taglia

Trade aggregati con volume maggiore o uguale a 100 lotti, filtro nativo ATAS:

| Data NY | Numero | Volume | Buy | Sell | Netto |
|---|---|---|---|---|---|
| 2026-09-07 | 2 | 272 | 0 | 272 | -272 |
| 2026-09-08 | 24 | 3.948 | 1.071 | 2.877 | -1.806 |
| 2026-09-09 | 25 | 4.197 | 2.338 | 1.859 | +479 |
| 2026-09-10 | 28 | 4.591 | 1.806 | 2.785 | -979 |
| 2026-09-11 | 1 | 101 | 0 | 101 | -101 |

Il trade singolo piu' grande della settimana e' una vendita di 382 lotti il 9 settembre alle 20:03:32 UTC, da 29.443 a 29.428,75.

Va detto con chiarezza che questi numeri sono piccoli: 80 trade da 100 lotti o piu', per circa 13.000 contratti, dentro un volume settimanale di 2.162.401. Descrivono chi si muove in blocchi visibili, non "le istituzioni", e non spiegano da soli il movimento.

## Oggi, 11 Settembre, In Formazione

Alle 09:42 UTC, con la cash ancora chiusa:

```text
volume dalle 18:00 ET di ieri   80.492   circa il 20% di una sessione cash piena
POC in formazione               29.108
area di valore in formazione    29.041 - 29.227
prezzo                          29.314   sopra l'area in formazione
```

Il POC provvisorio di oggi, 29.108, coincide di fatto con il POC della cash del 1 settembre, 29.125: diciassette punti di differenza. In sette sedute il mercato ha percorso un giro completo ed e' tornato sul livello dove si trovava alla data di rilevazione dell'ultimo report COT.

Due avvertenze che limitano questa lettura:

1. Il profilo di oggi e' costruito su un quinto del volume di una sessione e la cash non e' ancora aperta. Il POC si spostera'. Il confronto con il 1 settembre e' un'osservazione di posizione, non un profilo comparabile.
2. Il prezzo attuale, 29.314, e' **sopra** l'area di valore in formazione e sopra il suo estremo superiore. Il livello del 1 settembre e' alle spalle del prezzo, non dove il mercato sta scambiando adesso.

## Limiti

- NQU6 scade il 18 settembre: la settimana osservata e' dentro il periodo di rollover verso NQZ6, quindi il volume non e' confrontabile con settimane lontane dalla scadenza. ATAS calcola le date di rollover solo su chart a contratto continuo, quindi il peso effettivo del roll non e' stato misurato qui.
- L'area di valore e' calcolata al 70% espandendo dal POC verso il lato con piu' volume. E' la convenzione classica, ma resta una convenzione: soglie diverse spostano VAL e VAH.
- Il 4 settembre e' parziale all'inizio della serie caricata nel chart e il 7 settembre e' festivo.
- Nessuna delle osservazioni sopra e' stata verificata fuori campione. Descrivono una settimana, non una regolarita'.
