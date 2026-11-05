# Fase 3 · La Gamma · 5 novembre 2026

**Stato**: fatto (con la volatilita' ricostruita di NQ discordante dalle altre, vedi Cosa Non Sappiamo)
**Dati**: `in-breve.json` (gamma), `gex-NQZ6.json` (solo testa, completezza, finestra del volume), `quattro-domande-NQZ6.json`, `macro.json` (VXN). Raccolta alle 16:16 ora italiana, nessun errore (`raccolta.json`). Freschezza: GEX di `gex.py` **riusata dal giro**, calcolata alle 16:13-16:14 (circa 2 minuti alla raccolta); GEX viva alle 16:14 (96 secondi); X-Ray alle 16:14, lettore vivo, nessuna famiglia ferma; VXN ufficiale (Yahoo) alle 16:01; VXN e VIX ricostruiti alle 16:15; Cboe NDX alle 16:00 (15 minuti di ritardo); open interest di ieri sera (CME).

**In una riga**: tutte le scadenze delle opzioni su NQ hanno gamma negativa nella convenzione dei fornitori, senza un punto di svolta entro l'8%, in una giornata gia' a -4,2%; la copertura vale meno dell'1% del volume del future.

## In Parole Semplici

**Il punto**: chi vende le opzioni su NQ, se e' messo come si suppone di solito, oggi si copre nel verso del movimento invece di frenarlo, e non c'e' un prezzo vicino dove smetterebbe. Ma la sua copertura e' piccola rispetto al volume del future, e non dice da che parte va il prezzo.

**Dal piu' ampio al piu' vicino**

1. **La paura**: l'indice della paura sul Nasdaq e' salito da 25 a 30 in un giorno; le opzioni di oggi e di domani prezzano circa il 60% annuo.
   → l'assicurazione costa cara, ma oggi il prezzo si e' gia' mosso piu' del doppio di quanto la paura prevedeva per un giorno.
2. **Le opzioni di NQ, tutte insieme**: la gamma e' negativa su tutte le scadenze, e il punto dove cambierebbe segno non c'e' entro l'8%.
   → in teoria chi vende le opzioni vende quando scende e compra quando sale: un acceleratore. Il loro segno e' supposto, non misurato.
3. **Quanto pesa**: per un movimento dell'1% (circa 270 punti) la copertura sarebbe di circa 2.300 future, meno di mezzo punto percentuale del volume di una giornata.
   → l'effetto, se c'e', e' piccolo: NQ non e' guidato dalle sue opzioni.
4. **L'S&P 500**: la stima di un fornitore (X-Ray) da' anche li' i market maker corti di gamma al prezzo, con il punto di svolta circa il 3,6% piu' su.
   → due fonti diverse dicono la stessa cosa; per NQ resta contesto.
5. **Oggi, fino alle 22:00**: le opzioni che scadono oggi sono quasi tutte put, e i loro strike piu' pesanti sono scesi con il prezzo nell'ultima ora.
   → i "muri" di oggi seguono il prezzo: non sono livelli fissi.

**I prezzi da guardare** (dal prezzo di 27.313 delle 16:14)

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.000 | lo strike con piu' opzioni aperte (put), quasi 700 punti sopra | il prezzo tornerebbe dove la copertura pesa di piu'; il regime resterebbe acceleratore (nessuno zero li') |
| circa 27.500-27.600 | put pesanti della settimana, 200-300 punti sopra | 27.600 e' a 16 punti dal muro basso del ping-pong: zona da confrontare con l'order flow |
| circa 27.000 | il muro put delle opzioni di oggi, circa 300 punti sotto | il prezzo sarebbe sul punto dove la copertura di oggi pesa di piu'; nessuno studio lo prova come supporto |

**Quanto e' provato**: la gamma e' un modello sulle posizioni di ieri sera con il segno supposto; nessuno studio prova questi livelli come supporti o resistenze su NQ, e su 42 sedute di ES il regime negativo del mattino non ha allargato la giornata.

**Le parole**: **gamma** = quanto cambia la copertura di chi ha venduto opzioni quando il prezzo si muove; **dealer / market maker** = chi vende le opzioni e si copre sul future; **freno / acceleratore** = gamma positiva (si comprano i ribassi) / negativa (si vendono i ribassi); **strike** = il prezzo scritto nell'opzione; **put** = l'opzione che protegge dal ribasso; **muro** = lo strike con la gamma di call (o di put) piu' grande.

## I Fatti

**Il regime** (GEX = gamma x open interest x 20 $ x F² x 1%, milioni di $ per 1%; A = dealer lunghi call e corti put, la convenzione dei fornitori e di ATAS; B = dealer corti di tutto [GPP]; zeri cercati su +-8%). Prezzo: future 27.312,5 alle 16:14, M1 del bridge 27.312,75 (differenza -0,25); pannello alle 16:16: 27.360,25.

| orizzonte | GEX A | GEX B | zero gamma | call wall | put wall | fonte, ora |
|---|---|---|---|---|---|---|
| tutte le scadenze | -1.250 | -1.877 | nessuno | 30.000 | 28.000 | `gex.py`, 16:13 |
| entro 7 giorni | -553 | -616 | nessuno | 28.300 | 27.500 | `gex.py`, 16:13 |
| 0DTE (scade alle 22:00) | -353 | -372 | nessuno | 27.750 | 27.000 | `gex.py`, 16:13 |
| tutte, viva | -1.248,9 | - | nessuno | 30.000 | 28.000 | GEX viva, 16:14 |
| 0DTE, viva | -350 | - | nessuno | 27.750 | 27.000 | GEX viva, 16:14 |

GEX viva, ultimi 61 minuti: future -914,5 punti, GEX totale -138,6, GEX 0DTE -91,2. Muri nella finestra: call wall 28.700 e 30.000, put wall 27.500 e 28.000; 0DTE call wall da 28.700 a 27.750, put wall da 28.250 a 27.000. Convenzione B, dove pesa di piu': tutte 28.000, 27.000, 27.500; settimana 27.500, 27.000, 27.600; 0DTE 27.000, 27.400, 27.420.

**Gli strike pesanti** (`per_strike`, +-6%, OI di ieri, GEX netta A in milioni di $ per 1%; distanza dal bridge delle 16:14; volume dalle 00:00 italiane, giorno CME; anche 27.600, put 448, -29,7, e 28.500, put 1.761, -32,7)

| strike | OI call | OI put | volume put oggi | GEX netta A | distanza |
|---|---|---|---|---|---|
| 28.000 | 667 | 6.077 | 1.218 | -142,4 | +687 |
| 27.500 | 249 | 2.736 | 1.194 | -94,2 | +187 |
| 27.400 | 0 | 331 | 331 | -29,8 | +87 |
| 27.250 | 38 | 948 | 232 | -38,3 | -63 |
| 27.000 | 369 | 2.712 | 1.046 | -111,4 | -313 |
| 26.000 | 193 | 3.195 | 1.627 | -68,0 | -1.313 |

**Le scadenze e la completezza** (`gex.py`, 16:13; volatilita' al prezzo, Black-76; mossa attesa fino alla scadenza, in punti)

| scadenza | vol. ATM | mossa attesa | quotate / opzioni | OI | nota |
|---|---|---|---|---|---|
| 05/11, oggi | 61,3% | 430 | 432 / 468 | 5.025 | OI stabile |
| 06/11 | 59,1% | 941 | 220 / 468 | 4.147 | meta' quotata |
| 09-13/11 | 34-37% | 1.009-1.415 | 138-201 su 368-438 | 6.059 il 13/11 | il 12/11 a 6,0% (mossa 230): dato sporco, si scarta |
| 20/11, mensile | 33,0% | 1.841 | 171 / 452 | 10.980 | **instabile**: OI da 9.925 a 10.980 fra le letture |
| 30/11, fine mese | 29,5% | 2.114 | 164 / 394 | 9.044 | |
| 18/12, trimestrale | 21,2% | 1.991 | 400 / 404 | 69.235 | 61% dell'OI letto; zero 29.477 |

Scadenze saltate: nessuna; 11/12 con OI zero. OI contro il calcolo delle 15:37: -4,1% (118.298 a 113.422), quasi tutto il 06/11 (-4.719) e oggi (-1.713): la finestra di 120 strike per lato si e' spostata con il prezzo, non sono posizioni nuove (l'OI e' di ieri).

**La volatilita' e la scala**

| misura | valore | fonte, ora |
|---|---|---|
| VXN ufficiale | 30,32 (ieri 25,08) | Yahoo, 16:01 |
| VXN ricostruito dalla catena | **41,41** | catena CME, 16:15 |
| NDX stile VIX / al prezzo | 29,83 / 26,50 | Cboe, 16:00 (ritardo 15 min) |
| realizzata NQ (rendimenti a 5 minuti, annualizzati) | 4 ore 49,3; 24 ore 36,9; 48 ore 34,2; 20 sedute 21,7 | `in-breve`, `quattro-domande` |
| scala, convenzione A | 2.289 future per 1% (273,1 punti) = 0,47% del volume medio di cassa | volume medio 20 sedute 490.157 |
| scala, convenzione B | 3.437 future per 1% = 0,70% | idem |

## X-Ray (S&P 500, solo contesto per NQ)

Lettore vivo (ultimo giro 1 secondo prima), nessuna famiglia ferma; SPX 7.525,3 alle 16:14. Gamma stimata dei market maker allo spot -179: **acceleratore**. Zeri della seduta 7.794,9 e 7.852,3 (circa +3,6%); acceleratore piu' forte a 7.613, freno piu' forte a 7.820,5; per domani zeri 7.822 e 7.898. GEX dei market maker per scadenza: oggi -9,8 (entro l'1% +0,3, quasi piatta), domani -17,9, scadenza mensile -2,3. Delta scambiato dall'inizio della giornata (un flusso, non una posizione): il retail ha comprato delta sullo 0DTE (+17.861, di cui +14.115 negli ultimi 30 minuti), i clienti professionali ne hanno venduto (-3.533 sullo 0DTE, -7.575 su tutte). Pressione di copertura 0DTE da -3.356 a -8.580 in 30 minuti, verso non documentato. Unita' di OptionsDepth: conta il segno. **Riserva**: stima di un fornitore, su SPX, metodo non pubblicato, non provata; non sposta i livelli di NQ.

## Le Quattro Domande

- **Il regime**: acceleratore sia su tutte le scadenze sia sullo 0DTE (GEX viva, A): concordi, nessun segnale opposto. Dice quanto, non dove. "Con l'acceleratore i livelli tendono a rompersi" e' **un'ipotesi del video**: su 42 sedute di ES il regime delle 9:35 non ha allargato la giornata (0,55 contro 0,74 del movimento atteso, p 0,24; ipotesi D).
- **Il meteo**: VXN 30,3, fascia tesa; movimento atteso di un giorno 522 punti, per oggi 427. Implicita meno realizzata **+8,6** sulle 20 sedute: assicurazione cara (soglia +2, convenzionale). Ma sulle ultime 24 ore la realizzata (36,9) supera il VXN, e oggi il prezzo e' a -1.209,5 punti dalla chiusura: il mercato si muove piu' del prezzato. Skew 6,5, put care; breve contro mese -3,8, "normale" (ma la volatilita' al prezzo di oggi e domani e' quasi tripla della trimestrale: vedi Cosa Non Sappiamo).
- **I muri sul bordo del valore**: nessun muro entro 25 punti da un bordo (il piu' vicino, il put wall 0DTE 27.000, e' 337 punti sotto il VAL di oggi 27.337). Lo strike pesante 27.600 e' a 16 punti dal muro basso del ping-pong (27.615,75), ma non e' un muro. Ipotesi E: un caso solo, non provata. Il grilletto e' l'order flow (fase 4).

## Cosa Dicono Per NQ

- **Il regime** (le-opzioni-in-atas, "La GEX calcolata da noi"): con la catena tutta di put, la GEX A e' negativa a ogni orizzonte e nessuno zero cade nei +-8%; in B lo e' per costruzione. Qui le due convenzioni coincidono nel segno, ma il segno vero dei dealer resta **non conosciuto** [GPP]: se i clienti avessero venduto quelle put, il verso si rovescerebbe.
- **Il segno stimato su SPX** va nello stesso verso (X-Ray, acceleratore allo spot): un indizio, non una prova per NQ (le-opzioni-in-atas, "Come si potrebbe conoscere il segno", punto 2).
- **I muri seguono il prezzo**: in un'ora lo 0DTE ha spostato il put wall da 28.250 a 27.000 con l'OI fermo. E' l'effetto di prezzo e tempo sulla gamma (come-si-leggono, punto 4), non nuove posizioni.
- **La scala**: 0,47% del volume medio di cassa in A, 0,70% in B. La copertura che la gamma suppone e' un tetto [sintesi 2026-10-15, punto 3]; la stessa sintesi stima per NQ circa lo 0,4-0,5% del flusso (estrapolazione nostra), e a quella quota la simulazione di [BPVZ] non ha scenari. Su NQ le opzioni sul future sono piccole rispetto al future.
- **I paper, come ipotesi**: [BDLM] trova il momento dell'ultima mezz'ora sui future S&P solo nei giorni di gamma negativa dei dealer (coefficiente 6,63, t 4,78): oggi la condizione del paper c'e' (gamma A negativa, rendimento della giornata grande), ma su NQ la condizione di gamma non e' provata (ipotesi A e B di opzioni-gamma-es, non provate). [BB]: gamma alta, giorno dopo piu' calmo, con il segno assunto. [VAPG]: l'effetto mediano della gamma sulla volatilita' e' piccolo. La sintesi: nessun paper trova il verso opposto, e la variabile piu' sensibile e' il segno dell'autocorrelazione dei rendimenti brevi, non il livello della volatilita'. Nessuno di questi e' una regola.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| nessuno zero entro +-8% (circa 25.100-29.500); 29.477 lo zero della sola trimestrale, 30.000 il call wall | GEX A tutte, settimana, 0DTE | un ritorno sopra lo zero cambierebbe il regime supposto: oggi non e' a portata |
| 28.300 | call wall entro 7 giorni | coincide con il VAL del 3/11 (28.296): confluenza da confrontare |
| 28.000 | put wall tutte, strike piu' pesante (OI put 6.077), primo in B | dove la copertura pesa di piu' sull'intera catena |
| 27.750 | call wall 0DTE | 76 punti sotto il VAH di oggi (27.826): non un muro sul bordo (soglia 25) |
| 27.500-27.600 | put wall settimana, strike pesanti | 27.600 vicino al muro basso del ping-pong (27.615,75) |
| 27.400 / 27.250 | strike pesanti 0DTE e vicini | i piu' vicini al prezzo, sopra e sotto |
| 27.000 | put wall 0DTE, secondo strike piu' pesante, primo in B per lo 0DTE | dove la copertura di oggi pesa di piu' sotto il prezzo |
| oggi 22:00 | scadenza dello 0DTE (16:00 di New York) | dopo, lo 0DTE diventa il 06/11, meta' quotato; l'ultima mezz'ora (21:30-22:00) e' la finestra di [BDLM] |

## Cosa Non Sappiamo

- Il segno vero dei dealer su NQ: supposto (A) o estremo (B), mai misurato [GPP]. Nessun paper prova zero gamma, muri o strike pesanti come supporti o resistenze.
- L'OI e' di ieri sera: non vede gli 0DTE aperti e chiusi oggi, e in una giornata a -4% possono essere molti.
- **Il VXN ricostruito (41,4) non torna** con l'ufficiale (30,3) e con il Cboe NDX (29,8), che invece concordano fra loro: di solito stanno entro un punto. Puo' essere la catena arrivata a pezzi o il ritardo di Yahoo in un mercato veloce; non si sa. Il meteo usa l'ufficiale.
- Catena incompleta: il 06/11 ha meta' delle opzioni quotate, le settimanali 09-13/11 fra un terzo e meta'; il 20/11 e' instabile; il 12/11 ha una volatilita' sporca. Pesano poco sulla GEX A (la trimestrale e oggi dominano), ma i loro muri valgono meno.
- `lacuna del collettore: il verdetto fra le misure stile VIX` (convergono o no, con l'eta') non e' in `in-breve` ne' in `opzioni-vive.json`.
- `lacuna del collettore: la fonte della pendenza breve contro mese` (quali scadenze): -3,8 "normale" contrasta con la volatilita' al prezzo di oggi (61%) e della trimestrale (21%).
- `lacuna del collettore: i muri sul profilo controllano solo call e put wall`, non gli strike pesanti: 27.600 contro 27.615,75 l'ho visto a mano.
- `lacuna del collettore: la GEX di QQQ e NDX dal Cboe` (zero e muri) non e' nei dati dello studio: c'e' solo la loro volatilita'.

## Per Il Quadro

- **Gli strike pesanti contro il COT e il valore**: 28.000 (put, il piu' pesante), 27.500-27.600, 27.000; il call wall della settimana 28.300 sul VAL del 3/11 (28.296), gia' in `confluenze.json`.
- **Il muro vicino a un bordo**: nessun muro entro 25 punti; lo strike 27.600 a 16 punti dal muro basso del ping-pong, con il regime **acceleratore** (ipotesi E, un caso solo, non provata).
- **La paura contro la macro**: VXN ufficiale +5,2 punti in un giorno, realizzata delle ultime 4 ore 49%: coerente con la notizia o il dato che la macro ha trovato?
- **La scala contro l'order flow**: la copertura supposta vale lo 0,5-0,7% del volume medio; il delta della cassa (-2.538 alle 16:16) e il volume (259.070) dicono chi muove davvero il prezzo.
