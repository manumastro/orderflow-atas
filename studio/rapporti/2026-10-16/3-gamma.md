# Fase 3 · La Gamma · 2026-10-16 (pomeriggio, raccolta delle 15:32)

**Stato**: parziale (solo NQ; scadenze dal 21/10 quotate a pezzi; max pain non calcolato; VXN non letto)
**Dati**: [gNQ] `in-breve.json`, sezione `gamma` (da `gex-NQZ6.json`, non aperto): calcolo dalle **15:27:54**
ora italiana (13:27 UTC), future 30.013,50 alle 15:32:32 (`future_quando`), prezzo del bridge **30.012,25**
(M1 delle 15:32:33, differenza 1,25). Raccolta `ok` alle 15:32 (`raccolta.json`), GEX in 200 secondi,
nessun errore. Scadenza saltata: 06/11. Confronto con il calcolo delle 09:31 (`precedenti/0936/3-gamma.md`).

**In una riga**: in sei ore il prezzo e' sceso di 148 punti fin sopra lo strike 30.000, e con la
convenzione dei fornitori la gamma e' ora negativa su tutti e tre gli orizzonti, 0DTE compreso (stamattina
era positivo); il prezzo sta sotto tutti gli zeri; la copertura pesa lo 0,26% del volume medio (A), l'1,76% (B).

Le parole. **Gamma**: quanto cambia il delta (la sensibilita' dell'opzione al future) se il future si
muove. Il **dealer** vende opzioni e si copre col future. **GEX**: milioni di $ di future che i dealer
scambiano per un movimento dell'1%; positiva vuol dire che comprano sui ribassi e vendono sui rialzi
(**smorza**), negativa il contrario (**amplifica**). **Zero gamma**: il prezzo dove la GEX cambia segno.
**Muri**: lo strike con la gamma call (call wall) o put (put wall) piu' grande. **0DTE**: la scadenza di
oggi, 16/10, alle 22:00 (mensile, terzo venerdi'; nel JSON `tipo` "Weekly"). **OI**: open interest.
**Max pain**: lo strike dove alla scadenza le opzioni in mano ai compratori valgono meno in totale.

## I Fatti

**Convenzione A** (fornitori e ATAS: dealer lunghi call e corti put, GEX = call - put). Distanze
(calcolo mio) dal prezzo del bridge 30.012,25 [gNQ].

| NQ | GEX A (09:31 -> 15:27) | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| tutte (17 scadenze) | -199,8 -> **-657,2** | 30.181,0 | sotto di 168,8 (0,56%) | 30.000 | 30.000 |
| entro 7 giorni (al 22/10) | -73,7 -> **-519,9** | 30.150,3 | sotto di 138,0 (0,46%) | 30.000 | 30.000 |
| 0DTE 16/10 | +71,7 -> **-278,4** | 30.096,0 | sotto di 83,7 (0,28%) | 30.000 | 30.000 |

Per scadenza (A): 19/10 -87,8; 20/10 -122,9; 23/10 -106,9; 30/10 +51,1; 20/11 +25,1; 18/12 -69,6 [gNQ].
**Convenzione B** [GPP]: dealer corti di tutto, GEX negativa ovunque; dice solo **dove** pesa. Tutte
-4.444,5; 7 giorni -3.817,4; 0DTE -3.032,3 (68,2% della B, calcolo mio). Primi pesi B su tutti e tre gli
orizzonti: **30.000, 30.100, 29.800** (stamattina 30.000, 30.250, 30.100). Il segno vero non si conosce.

**Strike pesanti** (`strike_pesanti`, +-6%, tutte le scadenze) [gNQ]. Volume del giorno, finestra non letta.

| strike | dal prezzo | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|---|
| 30.000 | -12,3 | 779 / 2.105 | 157 / 1.359 | 219,1 | -440,7 | **-221,6** |
| 30.100 | +87,8 | 620 / 259 | 342 / 286 | 170,5 | -56,7 | +113,8 |
| 29.700 | -312,3 | 151 / 966 | 1 / 534 | 18,9 | -118,3 | -99,4 |
| 29.500 | -512,3 | 127 / 1.641 | 12 / 349 | 7,0 | -99,9 | -92,9 |
| 30.500 | +487,8 | 2.084 / 691 | 481 / 26 | 115,1 | -35,5 | +79,6 |
| 29.600 | -412,3 | 132 / 1.047 | 10 / 427 | 11,5 | -89,2 | -77,7 |
| 30.200 | +187,8 | 519 / 395 | 382 / 73 | 99,7 | -50,3 | +49,4 |

La scadenza piu' pesante della settimana resta il **16/10** (OI 24.649), poi 19/10 (4.796) e 23/10 (4.271).

**Mossa attesa** (vol ATM Black-76, una deviazione standard fino alla scadenza) [gNQ] contro la
realizzata di NQ a 24 ore, 19,6% (log a 5 minuti, annualizzati 276 x 252; 4 ore 15,1%, 48 ore 19,7%).

| NQ | 16/10 | 19/10 | 20/10 | 23/10 | 30/10 | 18/12 |
|---|---|---|---|---|---|---|
| vol ATM (/ realizzata 24 ore) | 28,6% (1,46) | 14,7% (0,75) | 16,2% (0,83) | 18,4% (0,94) | 19,8% (1,01) | 19,3% (0,99) |
| mossa | +-235,0 | +-418,9 | +-526,7 | +-778,8 | +-1.174,4 | +-2.408,9 |

**La scala** (dal collettore, non rifatta) [gNQ `scala`]: 1% = 300,1 punti. A tutte = **1.095** future per
1%, lo **0,26%** del volume medio di cassa a 20 sedute (419.565); B tutte = 7.404, l'**1,76%**.
Stamattina 331 (0,08%) e 7.290 (1,74%).

**ATAS dell'utente** (letto stamattina, ora esatta e scadenza non dichiarate): max pain 30.130, call wall
30.100. Il nostro call wall 0DTE era **30.100 alle 09:31** (coincide); alle 15:27 e' 30.000. Il max pain
noi non lo calcoliamo: il 30.130 non si verifica. Il prezzo gli sta 117,8 punti sotto.

**OI rispetto alle 09:31** [gNQ `oi_cambiato`]: totale 64.372 -> 63.592 (-1,21%); 0DTE -420; 30/11 +2.172;
18/12 -1.574; 20/11 -756. Strike: 30.500 1.531 -> 2.775, 31.500 211 -> 1.767, 29.500 2.831 -> 1.768.

## Cosa Dicono Per NQ

1. **Cosa e' cambiato: soprattutto il prezzo, non le posizioni.** L'OI totale e' quasi fermo (-1,2%), il
   prezzo e' sceso di 148 punti. La GEX A dipende da dove sta il prezzo: avvicinandosi al 30.000, dove le
   put pesano quasi il triplo delle call (2.105 contro 779), lo 0DTE e' passato da +71,7 a -278,4.
   **Osservazione**: e' la gamma che si sposta col prezzo, non flusso nuovo provato.
2. **Il regime, se vale A**: negativo ovunque, prezzo sotto tutti e tre gli zeri (84-169 punti, dentro la
   mossa attesa di oggi, +-235). Se i dealer fossero davvero cosi', la copertura **amplificherebbe**. **Il
   segno e' un'ipotesi** [GPP; finding 5 di [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md)].
3. **Il segno A ora poggia sulle catene buone.** Stamattina il 75% della A totale veniva da scadenze
   quotate a pezzi; ora 16/10, 19/10 e 20/10 (quasi intere) fanno -489,1 su -657,2 (74%, calcolo mio).
   Il numero e' piu' solido di stamattina; la convenzione resta un'ipotesi.
4. **Il "dove" regge alle due convenzioni**: 30.000 e' put wall, call wall e primo peso B ovunque, a 12
   punti dal prezzo; sotto, OI quasi solo put (29.700, 29.600, 29.500); sopra, call (30.100, 30.200, 30.500).
5. **La scala**: A tre volte stamattina, ma ancora lo 0,26% del volume medio. BPVZ (simulazione) vede
   effetti con il dealer al 18-19% del flusso: **nostra estrapolazione**, l'effetto resterebbe piccolo
   anche in B. DPS: la GEX e' un tetto del flusso, non il flusso (**lettura nostra**,
   [sintesi](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
6. **La volatilita'**: lo 0DTE prezza 1,46 volte la realizzata a 24 ore (stamattina 1,28); le scadenze dal
   19/10 meno di 1. **Osservazione**: il premio e' tutto sulla scadenza di oggi.
7. **I paper, ipotesi mai provate qui** (tutti su S&P o SPX, nessuno su NQ). BDLM: momento dell'ultima
   mezz'ora solo con gamma negativa (6,63, t 4,78, contro 0,82): oggi A negativa su tutti gli orizzonti,
   ma la variabile delle 21:30 non esiste ancora. BB, VAPG: gamma alta, giorno dopo piu' calmo. BTW: su ES
   +14,3 pb fino alle 15:30 del venerdi' di scadenza, poi -12,9 pb; NQ alle 15:29 e' a -0,33% dalla
   chiusura di ieri (30.140,25 -> 30.040): opposto alla media del paper, un giorno solo. GJ: pinning
   (chiusura su uno strike pesante) nell'ultima mezz'ora, 21,3% contro 15%; il 30.000 e' a 12 punti.

## I Livelli E Le Date

**Riserva per ogni riga**: nessun paper letto prova zero gamma, muri, strike pesanti o max pain come
supporti o resistenze (finding 6 della scheda): dicono dove cambia la descrizione, non cosa fa il prezzo.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.181,0 / 30.150,3 | zero A di tutte / entro 7 giorni [gNQ 15:27] | posizionamento: sopra, la A di quell'orizzonte torna positiva |
| 30.130 | max pain di ATAS (stamattina, non nostro) | posizionamento: non verificato; noi non lo calcoliamo |
| 30.100 | strike, netta A +113,8; secondo peso B; call wall di ATAS | posizionamento: la gamma call piu' grande sopra il prezzo |
| 30.096,0 | zero A dello 0DTE [gNQ 15:27] | posizionamento: sopra, lo 0DTE torna positivo in A; vale fino alle 22:00 |
| 30.000 | put wall e call wall ovunque; primo peso B; 1.359 put di volume | posizionamento: il peso massimo, a 12 punti; candidato al pinning di GJ |
| 29.800 | terzo peso B | posizionamento: dove pesa la copertura sotto, in B |
| 29.700 / 29.500 | 966 e 1.641 put di OI | posizionamento: le concentrazioni di put sotto |
| 16/10 21:30-22:00 | ultima mezz'ora e scadenza dello 0DTE | finestra di BDLM e GJ; esce il 68% della B (-3.032,3) |

## Cosa Non Sappiamo

- **Completezza** (`quotate`/`opzioni`): intere 16/10 (476/480), 19/10 (471/480), 20/10 (480/480). A
  pezzi il resto: 21/10 85/480, 22/10 92/418, 23/10 101/428, 30/10 59/434, 18/12 103/364, novembrine
  47-59. `oi_stabile` falso su 22/10, 23/10, 26/10, 28/10, 30/10 e tutte le novembrine e il 18/12.
- **Il 02/11** ha OI 0 e vol ATM 68,4% (mossa +-4.466,6): dato anomalo, non usato.
- **Il cambio di OI dentro la giornata**: la borsa aggiorna l'OI di norma una volta al giorno, e la nota
  del collettore dice che conta solo gli strike letti. I +2.172 del 30/11 e i -1.574 del 18/12 possono
  essere catena letta diversamente, non posizioni nuove: da qui non si separa.
- **ATAS**: max pain e call wall letti stamattina, senza ora ne' scadenza; confronto a orari diversi.
- lacuna del collettore: il **max pain** (per scadenza); data e ora dell'OI della borsa;
  `volume_finestra`, `strike_per_lato` e `per_scadenza` non in `in-breve`; scadenza 06/11 saltata senza
  motivo; VXN e realizzata a 20 sedute non in `in-breve`; la variabile di BDLM alle 21:30.
- La GEX ha tre giorni di storia: nessun confronto statistico e' possibile.

## Per Il Quadro

1. **30.000**: tutti i muri e il primo peso B, a 12 punti dal prezzo, vicino al POC di cassa di ieri
   (29.970): l'order flow lo mostra difeso o attraversato?
2. **Gli zeri 30.096-30.181** stanno sopra il prezzo, nella zona VAL cassa 14/10 (30.082), VAH di ieri
   (30.115), VAH della notte (30.167) e POC del 14/10 (30.173): confluenze in `in-breve`, dati diversi.
3. **A negativa ovunque** con 0DTE a 1,46 volte la realizzata: coerente con il regime della macro? La
   scala resta piccola (0,26%).
4. I livelli del COT sono tutti sopra 30.549: lo strike pesante piu' vicino e' 30.500 (OI call 2.084).
