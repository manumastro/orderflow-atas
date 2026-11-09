# Fase 3 · La Gamma Di NQ Ed ES, Lette Insieme · 2026-11-09

**Stato**: fatto, con tre limiti. La catena di NQ e' incompleta sulle scadenze lontane. Lo spot di SPX in X-Ray e' fermo a venerdi'. Per ES manca la scala.
**Dati**: `dati/in-breve.json` e `dati-ES/in-breve.json` (sezione `gamma`), `dati/macro.json` (VIX, VXN, calendario). Raccolta delle 12:28, ora italiana (`raccolta.json`, nessun errore).
- GEX di `gex.py` su NQZ6: calcolata alle 12:22 e riusata dal giro (6 minuti prima della raccolta). Future 27.190 alle 12:23; M1 del bridge 27.188,5 (differenza 1,5). Le distanze si misurano dal bridge.
- GEX viva di NQ (`opzioni-vive.json`): 12:27, 55 secondi prima. La finestra della variazione e' di soli 17 minuti (7 punti oggi).
- X-Ray (OptionsDepth su SPX): 12:26, lettore vivo (ultimo giro 15 secondi fa, 0 errori), nessuna famiglia ferma. **Lo spot di SPX e' fermo alla chiusura di venerdi'** (7.376,06), perche' la cassa apre alle 15:30. La base ES-SPX (23,5) e' quella del 6/11.
- VIX e VXN ricostruiti dalla catena CME: 12:26. VIX ufficiale: 12:13 di oggi. **VXN ufficiale fermo a venerdi' 22:15**. Cboe QQQ e NDX fermi alla chiusura di venerdi'.
- La ricerca di oggi sul passaggio fra indici (`quant/paper/ricerche/2026-11-09-opzioni-fra-indici.md`) **non c'e' ancora** nel repository: non ha una scelta da citare.

**In una riga**: le stime sui due indici concordano, e dicono che chi vende le opzioni accelera i movimenti invece di frenarli. Si separano su un punto: per le opzioni che scadono oggi NQ e' a 50 punti dal cambio di regime, ES no. E la paura costa il giusto su NQ e cara su ES.

## In Parole Semplici

**Il punto**: su NQ e su ES le stime dicono la stessa cosa. Chi vende le opzioni, per coprirsi, tende a seguire il prezzo invece di frenarlo. Questo parla di quanto si puo' muovere la giornata, non di dove va. Sono stime, e nessuna e' provata come regola.

**Dal piu' ampio al piu' vicino**

1. **La paura**: le opzioni prezzano circa il 2% di movimento al giorno sul Nasdaq e circa l'1,5% sull'S&P. Il Nasdaq e' vicino al massimo di un anno.
   → Il mercato si aspetta giornate larghe, soprattutto sul Nasdaq. Domani alle 14:30 c'e' l'inflazione americana.
2. **Il costo della paura**: su NQ le opzioni costano quanto il movimento vero delle ultime settimane; su ES costano di piu'.
   → Sull'S&P l'assicurazione e' cara; sul Nasdaq no, perche' il Nasdaq si e' gia' mosso tanto.
3. **Tutte le scadenze**: NQ sta circa il 6% sotto il prezzo dove, in teoria, i dealer passerebbero da acceleratore a freno. Su ES X-Ray non trova nessun punto di cambio in un intervallo di circa il 3% sopra e sotto.
   → I due indici concordano: acceleratore, e lontano dal cambiare.
4. **Le opzioni che scadono oggi**: su NQ il loro punto di cambio e' circa 50 punti sopra il prezzo (0,2%). Su ES sono negative, e senza un punto di cambio vicino.
   → Su NQ basta un piccolo rialzo per girare il regime di oggi; su ES no. E' l'unico punto dove i due raccontano cose diverse.
5. **Quanto pesa**: su NQ la copertura supposta per un movimento dell'1% vale fra lo 0,2% e lo 0,6% del volume di un giorno.
   → E' poco rispetto a quanto si scambia: l'acceleratore c'e' sulla carta, ma e' piccolo.

**I prezzi da guardare** (ciascuno nel suo strumento, nessuna conversione fra NQ ed ES)

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 27.235 (NQ) | il punto di cambio delle opzioni NQ che scadono oggi | sopra, in teoria, le opzioni di oggi farebbero da freno: il regime di oggi va riletto |
| 27.000 e 27.500 (NQ) | gli strike con piu' opzioni vicino al prezzo (i muri) | li' la copertura pesa di piu'; si confrontano con il valore |
| circa 7.343 (ES) | dove X-Ray stima l'acceleratore piu' forte | e' 21 punti sopra il minimo del ping-pong (7.322): una zona dove la copertura spingerebbe di piu' |

**Quando**: 15:30 apre la cassa di New York e X-Ray riparte con lo spot vero · 22:00 scadono le opzioni di oggi · domani 14:30 l'inflazione (CPI).
**Cambia idea se**: NQ sale sopra circa 27.235 e ci resta; oppure, alla riapertura, X-Ray mostra la gamma al prezzo positiva o un punto di cambio vicino a ES.
**Quanto e' provato**: il segno dei dealer su NQ e' supposto, su ES e' stimato da un fornitore. Su 42 sedute di ES l'acceleratore delle 9:35 di New York non ha allargato la giornata. Che le opzioni di un indice informino su quelle dell'altro non e' provato.

**Le parole**: **dealer** = chi vende le opzioni e si copre sul future; **gamma** = quanto cambia la copertura quando il prezzo si muove (positiva: freno, i dealer comprano sui ribassi e vendono sui rialzi; negativa: acceleratore, inseguono il prezzo); **punto di cambio** (zero gamma, o flip) = il prezzo dove si passa dall'uno all'altro; **muro** = strike con molta gamma; **0DTE** = opzioni che scadono oggi; **X-Ray** = la stima di OptionsDepth delle posizioni dei market maker su SPX.

## I Fatti

**Il regime.** NQ: catena CME via ATAS, **convenzione A** (si suppone dealer lunghi di call e corti di put), milioni di $ per 1%. ES: X-Ray, unita' di OptionsDepth (conta il segno), livelli in prezzi di ES con la base del 6/11.

| | NQ, `gex.py` 12:22 | NQ, GEX viva 12:27 | ES, X-Ray 12:26 |
|---|---|---|---|
| prezzo | 27.188,5 (bridge) | 27.186,75 | 7.447,5 (SPX fermo 7.376,06) |
| tutte le scadenze | −451,6; zero 28.862 (+6,2%) | −366,4; zero 28.982 (+6,6%) | gamma al prezzo −163,7; nessun flip fra 7.155 e 7.655 di SPX (−3,6%, +3,1%) |
| entro 7 giorni | −226,1; zeri 27.746 (+2,1%) e 28.257 | non c'e' | prossima scadenza (10/11): −8,0; entro l'1% −3,4 |
| 0DTE | −20,0; zero 27.235 (+0,17%) | −22,0; zero 27.236 (+0,18%) | −8,1; entro l'1% −0,4 |
| scadenza mensile | 18/12: −172,1 | non c'e' | 20/11: −23,0; **entro l'1% +13,8** |
| regime al prezzo | acceleratore | acceleratore | acceleratore |

**La convenzione B** [GPP] (dealer corti di tutto, fornitori e ATAS non la usano): negativa ovunque, per costruzione. Tutte −1.704,5; entro 7 giorni −1.148,1; 0DTE −602,2. La copertura peserebbe di piu' a 27.500, 26.500 e 27.000. **Il segno vero dei dealer di NQ non si conosce.**

**X-Ray** (stima di un fornitore, su SPX, metodo non pubblicato, non provata). Fresco: lettore vivo, nessuna famiglia ferma; ma lo spot di SPX e' quello di venerdi' fino alle 15:30. I market maker sono stimati corti di gamma su tutto il profilo letto: anche gli strike "positivi" valgono circa −31. Il picco dell'acceleratore e' 7.320 di SPX, cioe' **7.343,5 di ES** (−1,4% dal prezzo). Non c'e' nessun freno, ne' oggi (`observed`) ne' nella previsione per domani dalle 15:30 (`outlook`, picco 7.342,5 di SPX, nessuno zero). Sullo 0DTE lo strike piu' pesante e' 7.400 di SPX, con il segno del freno (+4,8); seguono 7.270 (−4,7) e 7.455 (−3,8). Sulla mensile del 20/11: 7.500 (−26,2) e 7.700 (+15,3).

**I flussi.** X-Ray: delta accumulato dalle 10:00 italiane (04:00 di New York), positivo = il gruppo ha comprato delta. Il DEX (delta scambiato) e' un flusso, non una posizione; unita' di OptionsDepth.

| gruppo | 0DTE oggi (ultimi 30 minuti) | tutte le scadenze oggi (ultimi 30 minuti) |
|---|---|---|
| retail | −331,8 (−203,6) | +1.847,5 (+563,5) |
| clienti professionali | +104,3 (+98,2) | −317,6 (+48,2) |
| firm | +210,8 (+77,9) | −245,9 (−196,1) |
| broker-dealer | −12,2 (−9,7) | +36,0 (0,0) |

- Pressione 0DTE di X-Ray: +29,0 contro −8,2 trenta minuti prima. Il verso non e' documentato: si legge il cambio, non il segno.
- GEX viva di NQ negli ultimi 17 minuti: future −39 punti; 0DTE da circa −4,5 a −22 (−17,5); tutte −18,7; zero +21,8.
- I muri sono stabili nella finestra: 27.500 per call e put di tutte; per lo 0DTE, 27.500 call e 27.000 put.

**La volatilita'** (implicita = il movimento che le opzioni prezzano; realizzata = il movimento vero).

| | NQ | ES |
|---|---|---|
| indice a 30 giorni ricostruito, 12:26 | VXN 32,7 | VIX 22,9 |
| ufficiale | VXN 34,02 (venerdi', 99° percentile a un anno) | VIX 23,56 (12:13, 89° percentile) |
| movimento atteso di un giorno | 561 punti (circa 2,1%) | 111 punti (circa 1,5%) |
| realizzata 20 sedute | 34,4 | 21,4 |
| implicita meno realizzata | −1,7, in linea | **+2,2, assicurazione cara** |
| skew | 8,2: put care | 9,6: put care |
| breve contro mese | inversa (+11,6) | non c'e' nel dato |

- Il rapporto VXN/VIX ricostruiti e' circa 1,43: il Nasdaq e' prezzato il 43% piu' mosso dell'S&P.
- Cboe di venerdi', stile VIX: QQQ 32,9 e NDX 33,4. Entro un punto dal VXN ricostruito, ma con giorni diversi.

**NQ, mossa attesa e completezza per scadenza** (`gex.py`). La realizzata di NQ (rendimenti log a 5 minuti, annualizzati con 276 × 252) e' 36,6% nelle ultime 4 ore e 30,3% nelle ultime 24.

| scadenza | volatilita' al prezzo | mossa attesa | open interest | quotate | nota |
|---|---|---|---|---|---|
| 9/11 (0DTE) | 48,9% | 441 | 7.379 | 468/480 | completa |
| 10/11 | 43,6% | 735 | 1.989 | 470/480 | contiene il CPI |
| 13/11 | 37,2% | 1.111 | 8.329 | 476/480 | la piu' pesante della settimana; contiene il PPI |
| 20/11 | 33,9% | 1.628 | 5.559 | **82/480** | mensile, quotata al 17% |
| 30/11 | 34,1% | 2.243 | 5.000 | **56/418** | fine mese, quotata al 13% |
| 18/12 | 27,8% | 2.473 | 43.848 | 256/426 | la piu' pesante; **open interest non stabile** |

- Le altre settimanali sono quotate dal 14% al 33%. La 27/11 ha l'open interest non stabile, la 24/11 e' saltata. Nessun confronto con un calcolo precedente di oggi (`oi_cambiato` nullo).
- **Gli strike pesanti di NQ** (open interest, GEX netta A): 26.500 (put 2.287, −76,9), 27.000 (put 1.044, con 1.078 put scambiate oggi; −45,6), 27.500 (call 569, put 1.806; −37,2), 26.000 (−31,9), 26.250 (−29,4). Contano le put.

**La scala** (NQ, gia' calcolata nel collettore). 1% = 271,9 punti. In convenzione A: 831 future per 1%, lo 0,16% del volume medio di cassa delle 20 sedute (524.251). In B: 3.134 future, lo 0,6%. Per ES non c'e' (lacuna, sotto).

## Cosa Dicono Per NQ (E Per ES, Letti Insieme)

- **I regimi concordano su tutte le scadenze.** NQ e' sotto il suo zero del 6,2-6,6% (convenzione A, supposta); il segno su ES e' stimato da X-Ray, che non ha nessun flip entro circa il 3%. In tutti e due i casi il punto di cambio e' lontano: il regime di fondo non gira in una seduta normale. Per il prezzo, **in teoria**, la copertura segue il movimento invece di assorbirlo [BB, BDLM, VAPG: nessun paper trova il verso opposto]. Quanto e' provato: il verso del meccanismo si', su S&P e su azioni; l'effetto sul livello della volatilita' e' piccolo (VAPG: mediano −0,08 punti al giorno). Su NQ nessuno studio.
- **Lo 0DTE e' il punto dove divergono.** Su NQ lo zero delle opzioni di oggi e' a 27.235, appena sopra il prezzo (+0,17%) e dentro il valore di venerdi' (27.113-27.306). Su ES lo 0DTE e' negativo e non ha uno zero vicino; la mensile del 20/11, entro l'1% dallo spot fermo, e' perfino positiva (+13,8). Quindi su ES si leggono **segnali in parte opposti fra le scadenze**: acceleratore al prezzo e sullo 0DTE, freno vicino al prezzo sulla mensile. Su NQ, invece, il regime di oggi e' fragile: un rialzo di 50 punti lo girerebbe **nel modello**.
- **La GEX dello 0DTE di NQ si e' fatta piu' negativa mentre il prezzo scendeva** (da −4,5 a −22 in 17 minuti, 39 punti). L'open interest e' quello di ieri: a cambiarla sono il prezzo e il tempo, non posizioni nuove. Lo zero di tutte che sale mentre il prezzo scende allarga la zona dell'acceleratore ([come-si-leggono-le-opzioni-dal-vivo](../../../quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md), passo 7).
- **I flussi di X-Ray non hanno un verso unico.** Sullo 0DTE il retail ha venduto delta e firm e professionisti l'hanno comprato; su tutte le scadenze e' il contrario. Il cambio piu' netto degli ultimi 30 minuti e' il retail: venditore sullo 0DTE, compratore sul resto. Non e' lo stesso oggetto della variazione della GEX viva di NQ: la prima e' un flusso scambiato su SPX, la seconda un effetto del prezzo su posizioni ferme. **Non si puo' dire che vadano nello stesso verso.** Per farlo servirebbe la variazione di X-Ray nel tempo, che manca (lacuna).
- **La volatilita' separa i due indici.** Su ES l'assicurazione e' cara (+2,2): le opzioni prezzano piu' di quanto l'S&P si sia mosso. Su NQ e' in linea (−1,7): il Nasdaq si e' gia' mosso quanto prezzato, e la paura e' al massimo di un anno. Su NQ la struttura e' inversa: lo 0DTE al 48,9% e la 10/11 al 43,6% sopra il mese. **Osservazione**: si paga per la vicinanza del CPI di domani alle 14:30, come vuole il passo 2 di [come-si-leggono](../../../quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md). Non e' una misura dell'evento.
- **Quanto pesa la gamma.** Su NQ la copertura supposta vale lo 0,16-0,6% del volume di un giorno. Nella simulazione di BPVZ l'effetto e' grande solo con il dealer intorno al 18% del flusso o piu'. DPS suggerisce che sulle scadenze brevissime la copertura conta meno dell'abbinamento degli ordini. **Lettura nostra**, dalla sintesi del 15/10: su NQ l'acceleratore e' un'indicazione debole.
- **I paper, come ipotesi.** BDLM: sui future S&P il momento dell'ultima mezz'ora (21:30-22:00 italiane oggi) c'e' solo con gamma negativa (6,63, t 4,78). Su NQ il momento c'e' senza condizioni (t 7,97); la condizione di gamma non e' provata. Ipotesi D del video ("con l'acceleratore i livelli tendono a rompersi"): **non confermata**. Su 42 sedute di ES l'acceleratore delle 9:35 ha dato giornate un po' piu' strette (0,55 contro 0,74 del movimento atteso, p 0,24).
- **Il passaggio di informazione fra le opzioni dei due indici non e' provato.** Nessuno dei nove paper letti lo studia. La ricerca di oggi non ha ancora una scelta. Qui il confronto e' descrittivo: regimi, segni, flussi e volatilita' messi accanto, i livelli mai convertiti.

## Le Quattro Domande

- **1 Il regime**: acceleratore su NQ (tutte e 0DTE, GEX viva) e su ES (al prezzo e 0DTE, X-Ray). Su ES la mensile vicino al prezzo e' di segno opposto. Dice quanto, non dove. Esito della prova: ipotesi D non confermata su 42 sedute; si rilegge in giornata.
- **2 Il meteo**: NQ teso (VXN 32,7), implicita meno realizzata −1,7, in linea, put care, struttura inversa. ES teso (VIX 23,6), +2,2, assicurazione cara, put care. Le soglie ±2 sono convenzionali, non provate.
- **3 I muri sul bordo del valore**: nessuno su NQ (soglia 25 punti) e nessuno su ES (soglia 6). Il piu' vicino su NQ e' lo zero 0DTE, 27.236: dentro il valore di venerdi', fra il POC (27.175) e il VAH (27.306), ma uno zero non e' un muro. Su ES l'acceleratore piu' forte, 7.343,5, e' 21 punti sopra il muro basso del ping-pong (7.322,25). Ipotesi E: un caso solo, non provata.
- **4 Il grilletto**: e' l'order flow (fase 4), non le opzioni. Qui nessun segnale.

## I Livelli E Le Date

Nessun paper prova questi livelli come supporti o resistenze. Sono i prezzi dove, nel modello, la copertura cambia o pesa di piu'.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.862-28.982 (NQ) | zero gamma di tutte, A (`gex.py` 12:22 e viva 12:27) | dove il regime supposto di fondo girerebbe; lontano, si nomina come zona |
| 27.746 e 28.257 (NQ) | zeri entro 7 giorni, A | il regime della settimana cambierebbe |
| 27.500 (NQ) | call e put wall di tutte, call wall 0DTE; peso B piu' alto | posizionamento: la copertura pesa di piu' |
| 27.235 (NQ) | zero 0DTE, A | il regime di oggi girerebbe nel modello: da rileggere |
| 27.000 (NQ) | put wall 0DTE; 1.078 put scambiate oggi | posizionamento sullo 0DTE |
| 26.500 (NQ) | lo strike piu' pesante (put 2.287) | posizionamento, settimana |
| 7.343,5 (ES) | acceleratore piu' forte di X-Ray (7.320 di SPX + base 23,5) | dove, nella stima, la copertura spingerebbe di piu' |
| oggi 15:30 | apertura della cassa di New York | X-Ray riparte con lo spot vero: si rilegge il regime di ES |
| oggi 21:30-22:00 | ultima mezz'ora; 22:00 scade lo 0DTE | la finestra di BDLM; lo 0DTE pesa di piu' |
| 10/11 14:30 | CPI (peso alto) | dentro la scadenza 10/11 (vol 43,6%) |
| 13/11 14:30 | PPI; scadenza settimanale piu' pesante di NQ | |
| 20/11 15:15 | scadenza mensile SPX del mattino (09:15 di New York) | la mensile di X-Ray |

## Cosa Non Sappiamo

- **Il segno dei dealer**: su NQ e' supposto (A), su ES e' stimato da OptionsDepth con un metodo non pubblicato. GPP lo trova corto quasi ovunque su SPX 1996-2001; VAPG di solito lungo, 2020-2023.
- **La catena di NQ e' incompleta lontano**: la mensile 20/11 e' quotata al 17%, la 30/11 al 13%; la 18/12, la piu' pesante, al 60% e con l'open interest instabile; la 24/11 manca. La GEX di tutte e il suo zero valgono meno di quelli dello 0DTE e della settimana, che sono completi.
- **`gex.py` e la GEX viva differiscono** a 5 minuti di distanza: −451,6 contro −366,4, zero 28.862 contro 28.982. Il metodo le dava coincidenti il 2/11. Non si sa se conti l'istante o la copertura delle scadenze.
- **X-Ray fuori cassa**: la GEX per scadenza e "entro l'1%" sono calcolate sullo spot fermo di venerdi' (7.376); il prezzo vero implicito e' piu' alto di circa 48 punti di SPX. La base ES-SPX e' del 6/11.
- **Il verso della pressione 0DTE** di X-Ray non e' documentato.
- **Il passaggio di informazione fra le opzioni di SPX e di NDX**: nessuna prova, nessun paper letto. La ricerca di oggi non e' ancora nel repository.
- lacuna del collettore: **la scala di ES**. X-Ray e' in unita' di OptionsDepth, e dal 5/11 non c'e' una GEX in $ di ES. Non si sa quanto pesi la gamma su ES rispetto al suo volume.
- lacuna del collettore: **la variazione nel tempo di X-Ray** (GEX dei market maker 0DTE e gamma al prezzo nell'ultima ora). Senza, non si confronta con la variazione della GEX viva di NQ.
- lacuna del collettore: **la struttura a termine di ES** (breve contro mese: nulla nel meteo di ES).
- lacuna del collettore: **il VXN ufficiale di oggi**. Yahoo e' fermo a venerdi' 22:15, il VIX ufficiale invece e' di oggi.
- lacuna del collettore: **la finestra della GEX viva** e' di 17 minuti con 7 punti, non un'ora.

## Per Il Quadro

- **Strike pesanti di NQ (26.500, 27.000, 27.500) contro i livelli del COT e il valore**: 27.500 e' 75 punti sopra il muro alto del ping-pong (27.425); 26.500 e' 133 punti sopra il muro basso (26.366,5). Lo zero 0DTE (27.236) cade dentro il valore di venerdi' (27.113-27.306).
- **I muri vicini al bordo del valore, con il regime**: nessuno entro le soglie, su tutti e due. Il piu' vicino su ES e' l'acceleratore piu' forte (7.343,5), 21 punti sopra il muro basso del ping-pong (7.322,25), in regime di acceleratore (ipotesi E, non provata).
- **I regimi concordano (acceleratore), la volatilita' no**: l'assicurazione e' cara su ES e in linea su NQ, con il VXN al 99° percentile a un anno. Coerente con la macro (CPI domani 14:30) e con lo scalino giu' della settimana su tutti e due?
- **Lo 0DTE di NQ a 50 punti dal cambio** contro quello di ES senza zero vicino: va riletto dopo le 15:30, con lo spot vero di X-Ray, prima di dire che i due indici concordano anche oggi.
