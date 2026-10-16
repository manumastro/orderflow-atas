# Fase 3 · La Gamma · 2026-10-16 (mattina, raccolta delle 09:36)

**Stato**: parziale (solo NQ; settimanali lontane e 18/12 quotate a pezzi; VXN non letto)
**Dati**: [gNQ] `in-breve.json`, sezione `gamma` (da `gex-NQZ6.json`): calcolo dalle **09:31:57** ora
italiana (07:31 UTC), future 30.164,75 alle 09:36:25 (`future_quando`), prezzo del bridge **30.160,50**
(M1 delle 09:36:25, differenza 4,25). Raccolta `ok` alle 09:36 (`raccolta.json`), nessun errore. Scadenze
saltate: 02/11 e 06/11. Primo calcolo del giorno (`oi_cambiato` nullo). ES non letto: studio solo NQ.

**In una riga**: con la convenzione dei fornitori la GEX totale di NQ resta negativa ma e' scesa a
-199,8 milioni di $ per 1% (ieri sera -1.811,7); lo 0DTE e' positivo e il prezzo gli sta sopra lo zero;
il 30.000 resta il peso massimo; la copertura vale lo 0,08% del volume medio di cassa (A), l'1,74% (B).

Le parole. **Gamma**: quanto cambia il delta di un'opzione se il future si muove. Il **dealer** vende
opzioni e si copre col future. **GEX**: i $ di future che i dealer scambiano per un movimento dell'1%
(qui milioni); positiva **smorza**, negativa **amplifica**. **Zero gamma**: dove la GEX totale cambia
segno. **Muri**: lo strike con la gamma call o put piu' grande. **0DTE**: la scadenza di oggi, 16/10,
alle 22:00 (terzo venerdi'; nel JSON il `tipo` e' "Weekly"). **OI**: open interest, contratti aperti.

## I Fatti

**Convenzione A** (fornitori e ATAS: dealer lunghi call e corti put, GEX = call - put). Distanze
(calcolo mio) dal prezzo del bridge 30.160,50 [gNQ].

| NQ | GEX A | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| tutte (16 scadenze) | -199,8 | 30.230,6 | sotto di 70,1 (0,23%) | 30.250 | 30.000 |
| entro 7 giorni (al 22/10) | -73,7 | 30.190,7 | sotto di 30,2 (0,10%) | 30.000 | 30.000 |
| 0DTE 16/10 | **+71,7** | 30.135,0 | sopra di 25,5 (0,08%) | 30.100 | 30.000 |

Per scadenza (A): 19/10 -26,0; 20/10 -95,7; 23/10 -106,1; 30/10 +5,7; 20/11 +17,1; 18/12 -20,4 [gNQ].
**Convenzione B** [GPP]: dealer corti di tutto, GEX negativa ovunque; dice solo **dove** pesa. Tutte
-4.398,1; 7 giorni -3.761,6; 0DTE -2.972,6 (67,6% della B, calcolo mio). Primi pesi B su tutti e tre
gli orizzonti: **30.000, 30.250, 30.100**. Il segno vero dei dealer non si conosce, in nessuna delle due.

**Strike pesanti** (`strike_pesanti`, +-6%, tutte le scadenze) [gNQ]. Volume del giorno senza lato,
finestra non letta (`volume_finestra` non e' in `in-breve`).

| strike | dal prezzo | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|---|
| 30.000 | -160,5 | 755 / 1.819 | 112 / 373 | 150,4 | -309,8 | -159,4 |
| 30.100 | -60,5 | 621 / 265 | 162 / 101 | 146,1 | -50,3 | +95,8 |
| 29.500 | -660,5 | 250 / 2.581 | 5 / 200 | 8,2 | -101,0 | -92,8 |
| 29.600 | -560,5 | 184 / 1.397 | 4 / 90 | 10,6 | -84,1 | -73,5 |
| 30.250 | +89,5 | 948 / 509 | 126 / 10 | 153,8 | -87,5 | +66,3 |
| 30.200 | +39,5 | 519 / 358 | 280 / 50 | 111,3 | -53,5 | +57,8 |
| 30.500 | +339,5 | 960 / 571 | 288 / 17 | 117,2 | -69,2 | +48,0 |

La scadenza piu' pesante della settimana e' il **16/10** (OI 25.069), poi 19/10 (4.878) e 23/10 (4.798).

**Mossa attesa** (vol ATM Black-76, una deviazione standard fino alla scadenza) [gNQ] contro la
realizzata di NQ a 24 ore, 19,9% (rendimenti log a 5 minuti, annualizzati 276 x 252; 4 ore 14,0%,
48 ore 21,3%) [gNQ `realizzata`].

| NQ | 16/10 | 19/10 | 20/10 | 23/10 | 30/10 | 18/12 |
|---|---|---|---|---|---|---|
| vol ATM (/ realizzata 24 ore) | 25,4% (1,28) | 15,2% (0,76) | 16,3% (0,82) | 18,2% (0,92) | 18,7% (0,94) | 19,6% (0,99) |
| mossa | +-288,9 | +-450,6 | +-548,3 | +-789,3 | +-1.126,8 | +-2.464,9 |

**La scala** (dal collettore, non rifatta) [gNQ `scala`]: 1% = 301,6 punti. A tutte = **331** future per
1%, lo **0,08%** del volume medio di cassa a 20 sedute (419.565); B tutte = 7.290, l'**1,74%**. La cassa
di oggi non e' aperta (`oggi_cassa` nullo); la notte ha 77.860 contratti [orderflow in `in-breve`].

**Cosa e' cambiato da ieri sera** (calcolo delle 19:30 del 15/10, stesso metodo: 120 strike, zero su
+-8%; `../2026-10-15/3-gamma.md`). Prezzo 30.018,75 -> 30.160,50. GEX A tutte -1.811,7 -> -199,8, B
-4.992,2 -> -4.398,1. E' scaduto lo 0DTE del 15/10 (A -713,4). Il 16/10: A -528,5 -> **+71,7**, B
-2.016,3 -> -2.972,6, zero 30.573,1 -> 30.135,0, OI 18.581 -> 25.069 (+35%, l'OI nuovo e' quello di fine
15/10). Zeri: tutte 30.498,8 -> 30.230,6, 7 giorni 30.440,8 -> 30.190,7. Il 30.000 resta primo peso B e
put wall; il call wall di tutte passa da 30.000 a 30.250. **Ma la completezza non e' la stessa**: il
18/12 ieri 339/362 quotate, oggi 90/364 (vedi sotto).

## Cosa Dicono Per NQ

1. **Il regime, se vale A**: misto. Lo 0DTE e' positivo e il prezzo gli sta 25,5 punti sopra lo zero;
   tutte e settimana sono negative, con gli zeri 30-70 punti sopra. I tre zeri stanno fra 30.135 e
   30.231, dentro un decimo della mossa attesa di oggi: il segno A cambia con movimenti piccoli.
   **Il segno e' un'ipotesi** [GPP; finding 5 di [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md)].
2. **Il segno della A totale poggia sulle catene peggiori.** Le scadenze dal 21/10 in poi, quotate a
   pezzi, fanno -149,8 dei -199,8 della A (75%, calcolo mio), ma solo il 15,6% della B. **Osservazione**:
   la B e' la lettura piu' robusta del "dove"; il segno A totale oggi no.
3. **Il "dove" regge alle due convenzioni.** 30.000 e' put wall e primo peso B ovunque; sotto il prezzo
   l'OI e' soprattutto put (30.000, 29.500, 29.600), sopra soprattutto call (30.200, 30.250, 30.500).
4. **La scala**: A allo 0,08% del volume medio, dieci volte meno di ieri sera (0,74%). BPVZ
   (simulazione) vede effetti con il dealer al 18-19% del flusso: **nostra estrapolazione**, su NQ oggi
   l'effetto della copertura sarebbe piccolo anche in B. DPS: la GEX e' un tetto del flusso, non il
   flusso (**lettura nostra**, [sintesi](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
5. **La volatilita'**: lo 0DTE prezza 1,28 volte la realizzata a 24 ore, le scadenze dal 19/10 meno di
   1. **Osservazione**: il premio sta solo sulla scadenza di oggi; la realizzata a 4 ore (14,0%) e'
   gia' piu' bassa di quella a 24.
6. **I paper, ipotesi mai provate qui** (tutti su S&P o SPX, nessuno su NDX o NQ). BDLM: momento
   dell'ultima mezz'ora solo con gamma negativa (6,63, t 4,78, contro 0,82); oggi 0DTE positivo in A,
   totale negativo. BB, VAPG: gamma alta, giorno dopo piu' calmo, effetto piccolo. BTW: su ES dalle 22:00
   del giovedi' alle 15:30 del venerdi' di scadenza +14,3 pb, poi fino alla chiusura -12,9 pb; NQ dalla
   chiusura di ieri (30.140,25) e' a +25,5 punti (+0,085%) alle 09:34. GJ: pinning (chiusura su uno
   strike pesante) nell'ultima mezz'ora del venerdi' di scadenza, 21,3% contro 15%.

## I Livelli E Le Date

**Riserva per ogni riga**: nessun paper letto prova zero gamma, muri o strike pesanti come supporti o
resistenze (finding 6 della scheda): dicono dove cambia la descrizione, non cosa fa il prezzo. [gNQ, 09:31]

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.250 | call wall di tutte; secondo peso B | posizionamento: la gamma call piu' grande sopra |
| 30.230,6 / 30.190,7 | zero A di tutte / entro 7 giorni | posizionamento: sopra, la A di quell'orizzonte diventa positiva |
| 30.200 | strike pesante, netta A +57,8; 280 call di volume | posizionamento: il volume call piu' alto vicino |
| 30.135,0 | zero A dello 0DTE | posizionamento: sotto, lo 0DTE diventa negativo in A; vale fino alle 22:00 |
| 30.100 | call wall 0DTE; terzo peso B | posizionamento: primo strike sotto il prezzo con netta A positiva |
| 30.000 | put wall ovunque; primo peso B | posizionamento: il peso massimo in tutte e due le convenzioni |
| 29.500 / 29.600 | 2.581 e 1.397 put di OI | posizionamento: la concentrazione di put piu' grande sotto |
| 16/10 15:30 | apertura della cassa; fine della finestra notturna di BTW | descrittivo: da qui BTW misura l'inversione su ES |
| 16/10 21:30-22:00 | ultima mezz'ora e scadenza dello 0DTE | finestra di BDLM e di GJ; esce lo 0DTE (A +71,7, B -2.972,6) |

## Cosa Non Sappiamo

- **Completezza** (`quotate`/`opzioni`): intere 16/10 (480/480), 19/10 (470/480), 20/10 (451/480). A
  pezzi tutto il resto: 21/10 78/480, 22/10 99/418, 23/10 122/428, 30/10 56/426, **18/12 90/364**
  (ieri 339/362), le novembrine 35-52. OI ancora in crescita (`oi_stabile` falso) su 22/10, 23/10, 26/10
  e da 28/10 in poi: il -106,1 del 23/10 non e' un numero fermo.
- **Il tipo dello 0DTE**: il JSON scrive "Weekly", ma e' il terzo venerdi'; se cambia l'ora del
  regolamento, cambia la finestra dei paper. Non verificato.
- **Il confronto con ieri** mescola mercato (scadenza del 15/10, OI nuovo, prezzo +142) e catena
  (18/12 quotata a un quarto): da qui non si separano.
- lacuna del collettore: data e ora dell'OI della borsa; `strike_per_lato`, `volume_finestra` e
  `per_scadenza` non sono in `in-breve` (non aperto `gex-NQZ6.json` per il tempo); scadenze 02/11 e
  06/11 saltate senza motivo; nessuno zero su +-8% per il 29/10; VXN e realizzata a 20 sedute non in
  `in-breve` (sono in `macro.json`, non letto); la variabile di BDLM alle 21:30 non esiste alle 09:36.
- La GEX ha due giorni di storia: nessun confronto statistico e' possibile.

## Per Il Quadro

1. **30.000**: put wall e primo peso B, ed e' il bordo basso del POC di ieri (29.970) e del VAL della
   notte (30.049): e l'order flow?
2. **Gli zeri 30.135-30.231** sono dentro il valore della notte (30.049-30.207, POC 30.190): lo zero
   della settimana (30.190,7) coincide con il POC della notte (confluenza in `confluenze.json`).
3. **30.100**, call wall 0DTE, sta a 15 punti dal VAH di cassa di ieri (30.115): stessa zona, due dati.
4. **Gamma A quasi neutra e scala piccola** (0,08%), 0DTE a 1,28 volte la realizzata: coerente con il
   regime della macro? I livelli del COT sono tutti sopra 30.549: nessuno strike pesante li tocca.
