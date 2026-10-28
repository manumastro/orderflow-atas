# Fase 3 · La Gamma · 2026-10-28

**Stato**: fatto (catena delle scadenze lontane incompleta, vedi Cosa Non Sappiamo)
**Dati**: `in-breve.json` (gamma), `gex-NQZ6.json` (`per_strike`, `per_scadenza` 0DTE, `volume_finestra`), `macro.json`
(VXN, calendario, trimestrali). Raccolta 10:18 italiane, nessun errore. GEX calcolata 10:13-10:16; future 29.940,5
alle 10:16, M1 del bridge 29.941 (-0,5): distanze da 29.941. VXN alla chiusura del 27/10 (21:15). OI di ieri (CME).

**In una riga**: con il modello dei fornitori la gamma dei dealer e' positiva e il prezzo sta circa 350 punti
sopra lo zero gamma; il peso e' tutto a 30.000, a 60 punti, e la gamma e' piccola rispetto al volume di NQ.

## In Parole Semplici

**Il punto**: chi vende le opzioni, secondo il modello piu' usato, oggi frena i movimenti di NQ invece di
spingerli. Ma la frenata e' piccola, circa lo 0,3% degli scambi di un giorno, e alle 19:00 c'e' la Fed:
un evento cosi' pesa piu' della gamma.

**Dal piu' ampio al piu' vicino**

1. **Tutte le scadenze**: la gamma totale e' positiva e il prezzo e' circa 350 punti sopra la soglia dove cambierebbe segno.
   → In teoria i dealer comprano i ribassi e vendono i rialzi: il mercato tende a smorzare.
2. **Quanto conta**: per un movimento dell'1% (circa 300 punti) dovrebbero scambiare circa 1.200 future.
   → E' lo 0,3% di un giorno normale di NQ: la frenata, se c'e', e' leggera.
3. **La settimana**: quasi tutta la gamma scade entro venerdi' 30, la scadenza piu' grossa e' quella di fine mese.
   → Dopo venerdi' la gamma che resta e' poca: il regime di oggi non dura per costruzione.
4. **Oggi**: le opzioni che scadono stasera (21:00) sono le piu' scambiate a 30.000 (call) e 29.500 (put).
   → Il mercato delle opzioni guarda una forchetta di circa 500 punti intorno al prezzo, con la Fed in mezzo.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.000 | lo strike con piu' contratti aperti, call e put; a 60 punti | e' dove in teoria la copertura pesa di piu', comunque siano posizionati i dealer |
| circa 29.700 | la soglia di segno delle sole opzioni di oggi | sotto, la parte di oggi smette di frenare (modello) |
| circa 29.580 | la soglia di segno di tutte le scadenze | sotto, il modello dice che i dealer spingono invece di frenare |

**Quando**: 19:00 decisione della Fed · 19:30 conferenza · 21:00 scadono le opzioni di oggi · dopo le 22:00 trimestrali Microsoft e Alphabet
**Quanto e' provato**: i numeri sono un nostro calcolo su un modello; chi sia davvero corto o lungo non lo sa nessuno, e nessuno studio prova questi prezzi come supporti o resistenze, ne' l'effetto su NQ.

**Le parole**: **dealer** = chi vende le opzioni e si copre con il future; **gamma** = quanto deve ricoprirsi quando il prezzo si muove;
**zero gamma** = il prezzo dove la frenata diventerebbe spinta; **strike** = il prezzo scritto nell'opzione; **contratti aperti** = le opzioni ancora in piedi.

## I Fatti

GEX = gamma x OI x 20 $ x F² x 1%, milioni di $ di future per un movimento dell'1% (`gex.py`). Convenzione A:
dealer lunghi le call, corti le put. Convenzione B [GPP]: dealer corti di tutto. Fonte: `in-breve.json`.

| orizzonte | GEX A | GEX B | zero gamma A | distanza | call wall | put wall | peso B (primi 3) |
|---|---|---|---|---|---|---|---|
| tutte | +723,7 | -2.761,7 | 29.589,7 | -351 (-1,2%) | 30.000 | 30.000 | 30.000, 30.500, 29.500 |
| entro 7 giorni | +709,7 | -2.370,3 | 29.577,8 | -363 | 30.000 | 30.000 | 30.000, 30.500, 29.500 |
| 0DTE (oggi) | +297,0 | -872,3 | 29.693,6 | -247 | 30.000 | 29.600 | 30.000, 29.600, 29.950 |

**La scala** (gia' calcolata; volume medio di cassa 20 sedute 449.621): A = 1.209 future per l'1% (299,4
punti), 0,27% del volume; B = 4.612 future, 1,03%.

| scadenza | tipo | OI | vol. ATM | mossa attesa (punti) | quotate | GEX A | zero A |
|---|---|---|---|---|---|---|---|
| 28/10 (0DTE) | settimanale | 7.601 | 31,0% | 325 | 398/480 | +297,0 | 29.693,6 |
| 29/10 | settimanale | 3.582 | 29,1% | 549 | 413/480 | +159,7 | 28.820,7 |
| 30/10 | fine mese | 18.985 | 27,9% | 683 | 458/480 | +252,0 | 29.520,2 |
| 18/12 | trimestrale | 16.754 | 21,1% | 2.371 | 78/378 | -12,5 | 30.174,1 |

Mossa attesa = una deviazione standard implicita fino alla scadenza (`gex.py`). Altre otto scadenze (3/11-4/12): OI 29-3.819,
quotate 5-44, GEX A fra -0,2 e +19,1. Saltate: 2, 5, 9-12/11. OI non stabile: 6, 13, 20, 30/11, 18/12. `oi_cambiato` nullo.

**Volatilita'**: realizzata (5 minuti, annualizzata 276 x 252) 11,4% su 4 ore, 16,4% su 24, 18,7% su 48;
VXN 24,91 (-1,49). Le implicite sono annualizzate in altro modo: il confronto e' indicativo.

**Strike pesanti** (tutte le scadenze, +-6%; volume del giorno CME, dalle 23:00 italiane del 27/10 alle 10:16):

| strike | OI call | OI put | vol. call | vol. put | GEX A netta |
|---|---|---|---|---|---|
| 30.500 | 1.495 | 215 | 68 | 1 | +96,7 |
| 30.000 | 1.907 | 2.270 | 611 | 30 | +65,7 |
| 29.000 | 133 | 1.503 | 12 | 132 | -50,2 |
| 31.500 | 2.726 | 965 | 220 | 1 | +33,7 |

**Il volume di oggi** (senza verso): put 29.500 630 (581 sullo 0DTE, OI 0DTE 194); call 30.000 611 (540 sullo 0DTE,
OI 0DTE 418); put 29.800 253, 29.200 225; call 31.500 220, 32.000 205. Sotto il prezzo quasi solo put, sopra call.

## Cosa Dicono Per NQ

- **Il regime (A)**: GEX positiva su ogni orizzonte, prezzo sopra tutti gli zeri: nel modello i dealer si coprono
  contro il movimento (`le-opzioni-in-atas.md`, La GEX calcolata da noi). Il segno vero non si conosce [GPP].
- **La convenzione B** dice solo dove pesa: 30.000, poi 30.500 e 29.500. Se i dealer fossero corti di
  tutto, la copertura seguirebbe il movimento. A 30.000 le due convenzioni coincidono sul luogo, non sul verso.
- **Lo 0DTE** porta il 41% della GEX A (297 su 724); il 30/10 e' la piu' pesante per OI e in B (-1.233). Lo zero
  0DTE sta dentro la sua mossa attesa (247 contro 325 punti). **Osservazione**: la GEX della settimana scende per costruzione.
- **La volatilita'**: implicite 28-31%, sopra il VXN (24,9) e la realizzata (11-19%). **Osservazione**: lo 0DTE
  contiene la Fed, il 29/10 Microsoft e Alphabet: le opzioni prezzano l'evento, non la quiete delle ultime ore.
- **Il volume di oggi**: call 30.000 e put 29.500 dello 0DTE superano gia' l'OI di ieri. Senza verso non si sa se
  siano coperture comprate o premi venduti; dice solo che la GEX non vede queste posizioni.
- **La scala**: 0,27% del volume (B 1,03%). In BPVZ (simulazione) la volatilita' resta nel rumore col dealer
  al 18-29% del flusso, autocorrelazione e code no gia' al 18-19%; sotto il 18% il paper tace. "Su NQ
  l'effetto e' piccolo" e' **nostra estrapolazione** (sintesi 2026-10-15). DPS: sulle brevissime i market
  maker abbinano gli ordini piu' che coprirsi, quindi la GEX e' un tetto (lettura nostra).
- **I paper, come ipotesi**: BB, gamma alta il giorno prima, rendimento assoluto e autocorrelazione intraday
  piu' bassi (indici, segno assunto) [Tab. III, VI]. BDLM, momento dell'ultima mezz'ora (20:30-21:00
  italiane) solo con gamma negativa (6,63, t 4,78, contro 0,82, t 1,03) [Tab. 7]: oggi in A e' positiva.
  VAPG: effetto mediano -0,08 punti di volatilita' contro 4,5 di variazione normale. Nessuno usa NQ: sono le
  ipotesi di `quant/studi/opzioni-gamma-es/1-idea.md`, non provate.

## I Livelli E Le Date

Nessun paper letto prova questi livelli come supporti o resistenze (l'unico test, Popovici 2026, non e' letto).

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.000 | call e put wall di tutte, OI massimo, primo in B, call 0DTE 540 | posizionamento: il peso massimo in entrambe le convenzioni |
| 30.500 | GEX A netta massima (+96,7), secondo in B, call wall del 30/10 | posizionamento sopra il prezzo |
| 29.694 | zero gamma A dello 0DTE | invalidazione del regime A per la parte di oggi |
| 29.578-29.590 | zero gamma A di 7 giorni e di tutte | invalidazione del regime A: sotto, il modello amplifica |
| 29.500 / 29.600 | terzo in B, put 0DTE 581 / put wall 0DTE | posizionamento sotto il prezzo |
| 29.000 | put OI 1.503, GEX A netta -50,2 (-941 punti) | posizionamento, lontano |
| 28/10 19:00, 19:30, 21:00 | FOMC, conferenza; scadenza 0DTE | evento sopra la gamma; esce il 41% della GEX A |
| 30/10 21:00 | scadenza di fine mese, la piu' pesante per OI | esce un altro terzo circa della GEX A |

## Cosa Non Sappiamo

- **Il segno dei dealer** sulle opzioni CME di NQ e **il passo gamma -> ordini sul future**: nessuna misura.
- **La catena lontana e' incompleta**: dal 3/11 quotate dal 3% al 21% delle opzioni, OI non stabile su cinque
  scadenze, sei saltate. La trimestrale del 18/12 (OI 16.754) pesa probabilmente piu' di -12,5.
- **L'OI e' di ieri** (le 0DTE aperte oggi non entrano nella GEX); **il volume non ha verso**, e il suo azzeramento
  alle 17:00 di Chicago e' da verificare (`volume_finestra`).
- **lacuna del collettore**: il confronto con la GEX di ieri (`quant/dati/NQ/gex/` non riassunto in `in-breve.json`).
- **lacuna del collettore**: il volume totale delle opzioni di oggi e il rapporto put/call.
- **lacuna del collettore**: l'ora della volatilita' realizzata e la convenzione di annualizzazione dell'implicita.

## Per Il Quadro

1. **30.000** (OI massimo, primo in B) a 60 punti: contro il framing di mercoledi' (29.883-30.087, POC 30.030) e il massimo della notte (29.989).
2. **Zero gamma 29.578-29.590** con la VAH di cassa del 26/10 (29.570), la VAL del report COT (29.584, 29.599) e il put wall 0DTE (29.600).
3. **29.000** (put OI 1.503) sulla VAL dei compositi di cassa a 20 e 90 sedute (28.996-28.998).
4. **Regime A positivo ma 0,27% del volume, in un giorno di Fed**: coerente con la macro (VXN in calo, implicite sopra la realizzata)?
