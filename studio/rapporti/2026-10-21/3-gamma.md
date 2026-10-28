# Fase 3 · La Gamma · 2026-10-21

**Stato**: fatto. Catena completa fino al 23 ottobre; la trimestrale di dicembre e le settimanali lontane no (sotto).
**Dati**: `dati/in-breve.json` (sezione `gamma`), `dati/gex-NQZ6.json`, `dati/macro.json` (VXN, VIX), raccolta delle 11:13 ora italiana (`raccolta.json`, tutto `ok`). GEX calcolata dalle 11:08:40 alle 11:13:24 (284 s); future 29.775,5 alle 11:13:23 (bridge `/opzioni`, chiusura M30), M1 del bridge 29.778,0 alle 11:13:24, differenza -2,5. **Le distanze sono dal 29.778,0.** VXN alla chiusura del 20 ottobre (22:15); VIX 17,47 alle 10:57. Storico per il confronto: `quant/dati/NQ/gex/2026-10-16/2021Z-NQZ6.json` (16 ottobre, 22:21). Errori: nessuno.

**In una riga**: nell'ipotesi piu' usata, la copertura di chi vende opzioni su NQ oggi tende di poco ad amplificare i movimenti invece di smorzarli; il prezzo sta proprio vicino al punto in cui questo si capovolgerebbe, e quella copertura vale meno di un millesimo del volume del future.

## In Parole Semplici

**Il punto**: nel modello piu' usato, la copertura di chi vende opzioni su NQ oggi tende di poco ad amplificare i movimenti. Ma il prezzo sta proprio sul confine, e questa copertura pesa pochissimo rispetto al future.

**Dal piu' ampio al piu' vicino**

1. **Il meccanismo**: chi vende le opzioni (i dealer) si copre comprando o vendendo il future, e quanto deve scambiare cambia quando il prezzo si muove.
   → Se si coprono contro il movimento lo smorzano, se nel suo stesso verso lo amplificano; il prezzo di confine si chiama zero gamma.
2. **Tutte le scadenze**: alle 11:13 NQ sta circa 60 punti sotto il confine di tutte le opzioni, che e' a circa 29.835.
   → In teoria siamo dal lato "amplifica", ma di poco: bastano 60 punti per passare dall'altra parte.
3. **La settimana e venerdi'**: il lato "amplifica" viene quasi tutto dalle opzioni che scadono venerdi' 23.
   → Venerdi' sera quel peso esce dal conto, e il quadro puo' cambiare.
4. **Oggi**: le opzioni che scadono stasera alle 22:00 hanno il confine piu' in basso, a circa 29.750, e il prezzo sta circa 30 punti sopra.
   → Per la sola parte di oggi il modello dice "smorza": le scadenze tirano in versi opposti.
5. **I muri**: le opzioni si concentrano a 30.000 sopra (scommesse al rialzo) e a 29.500 sotto (protezioni dal ribasso).
   → Dicono dove stanno le posizioni, non che il prezzo ci si fermi.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.000 | il prezzo con piu' scommesse al rialzo | e' dove le opzioni pesano di piu' sopra; nessuno studio dice che il prezzo ci si fermi |
| circa 29.835 | il confine di tutte le scadenze | sopra, nel modello, la copertura passerebbe a smorzare |
| circa 29.750 | il confine delle opzioni di oggi | sotto, anche la parte di oggi passerebbe ad amplificare |
| circa 29.500 | il prezzo con piu' protezioni dal ribasso | e' dove le opzioni pesano di piu' sotto; nessuno studio dice che il prezzo ci si fermi |

**Quanto e' provato**: non sappiamo se i dealer sono davvero lunghi o corti di opzioni; se fossero corti di tutto, amplificherebbero ovunque. Gli studi sul meccanismo sono sull'S&P 500, nessuno su NQ: e' un modello. E il peso e' piccolo: per un movimento dell'1% (circa 300 punti) la copertura vale circa 250 future, meno di un millesimo di quanti se ne scambiano in un giorno.

**Le parole**: **dealer** = chi vende le opzioni e si copre sul future; **zero gamma** = il prezzo dove la copertura passa da smorzare ad amplificare;
**muro** = il prezzo dove si concentrano piu' opzioni; **scommesse al rialzo / protezioni dal ribasso** = le opzioni call / put.

## I Fatti

Le parole, una volta. **GEX** (*gamma exposure*): quanti dollari di future i dealer (i market maker delle opzioni) dovrebbero comprare o vendere per coprirsi se il prezzo si muove dell'1%. **Positiva**: comprano i ribassi e vendono i rialzi, il movimento si smorza; **negativa**: il contrario, si amplifica. **Zero gamma**: il prezzo a cui la GEX totale cambia segno. **Call wall / put wall** (muri): lo strike con piu' GEX delle call / delle put. **0DTE**: opzioni che scadono oggi (alle 22:00 italiane). **OI** (*open interest*): i contratti aperti a fine di ieri. **Convenzione A** (fornitori, ATAS, SqueezeMetrics): dealer lunghi di call e corti di put. **Convenzione B** [GPP]: dealer corti di tutto. **Il segno vero non si conosce** [GPP, VAPG]: vale per ogni numero sotto.

Unita': milioni di $ per un movimento dell'1% (dedotta dalla scala: 248 future x 20 $ x 29.778 = 147,7 milioni). Fonte `in-breve.json`, `gamma.orizzonti`.

| orizzonte | GEX A | GEX B | zero gamma A | distanza dal prezzo | call wall | put wall | dove pesa B |
|---|---|---|---|---|---|---|---|
| tutte (15 scadenze) | -147,7 | -2.631,6 | 29.834,7 | +56,7 (+0,19%), prezzo sotto | 30.000 (+222) | 29.500 (-278) | 30.000, 29.500, 29.700 |
| entro 7 giorni (21-27 ott.) | -96,0 | -2.115,8 | 29.816,6 | +38,6 (+0,13%), prezzo sotto | 30.000 | 29.500 | 30.000, 29.500, 29.000 |
| 0DTE (21 ott.) | +48,8 | -992,9 | 29.747,1 | -30,9 (-0,10%), prezzo sopra | 30.000 | 29.500 | 29.500, 30.000, 29.850 |

Un solo zero per orizzonte, cercato su +-8%.

**Le scadenze che pesano** (`gamma.scadenze`): 21 ott. A +48,8, B -992,9; 22 ott. A +34,8, B -254,0, zero 29.627,7; **23 ott. (venerdi')** A **-175,8**, B -821,4, zero 30.210,3, muri 30.000 / 29.000, OI 11.095: e' la piu' pesante della settimana, e da sola rende negativo il totale. Dicembre (trimestrale) A -2,3, B -219,8, OI 18.680.

**Gli strike pesanti** (`gamma.strike_pesanti`, tutte le scadenze, +-6% dal future; GEX A netta, OI e volume del giorno):

| strike | distanza | GEX A netta | OI call / put | volume call / put |
|---|---|---|---|---|
| 29.500 | -278 | -82,0 | 87 / 825 | 13 / 525 |
| 29.000 | -778 | -76,7 | 19 / 1.693 | 1 / 118 |
| 29.100 | -678 | -50,4 | 13 / 1.125 | 0 / 56 |
| 29.900 | +122 | +41,4 | 343 / 151 | **686** / 338 |
| 30.100 | +322 | +39,1 | 478 / 128 | 302 / 4 |
| 29.350 / 29.600 / 29.400 / 29.650 | da -428 a -128 | da -37,6 a -27,8 | put dominanti | 29.600: put 402 |
| 29.850 | +72 | +26,8 | 251 / 137 | 72 / 215 |

Sotto il prezzo pesano le put, sopra le call. A 29.900 il volume call di oggi e' il doppio dell'OI di ieri.

**Volatilita'** (`gamma.scadenze`, `gamma.realizzata`, `macro.json`): implicita ATM 0DTE 28,1%, mossa attesa a scadenza 294,2 punti; 22 ott. 27,7% (520,4); 23 ott. 26,3% (642,4); dicembre 21,2% (2.515,1). Realizzata di NQ (rendimenti log a 5 minuti, x 276 x 252): 4 ore 16,5%, 24 ore 20,9%, 48 ore 19,3%. VXN 25,22 (20 ott.), VIX 17,47.

**La scala** (`gamma.scala`, volume medio di cassa 20 sedute 443.518 contratti): convenzione A **248 future** per un 1% (297,8 punti), lo **0,06%** del volume medio; convenzione B 4.419 future, l'**1,0%**.

**Il confronto con l'ultimo calcolo** (16 ottobre, 22:21, future 29.362,75): tutte A -689,2, zero 30.887,6; entro 7 giorni A -562,9. Oggi la GEX A e' circa un quinto, e lo zero e' 1.050 punti piu' in basso.

## Cosa Dicono Per NQ

1. **Il regime A e' negativo ma al confine.** Il prezzo sta sotto lo zero gamma di tutte le scadenze e della settimana, sopra quello dello 0DTE. I tre zeri stanno fra 29.747 e 29.835: una fascia di 88 punti, lo 0,3%. Uno spostamento cosi' cambia il segno del regime nel modello. Questi zeri sono calcolati con la volatilita' di ogni strike ferma (`le-opzioni-in-atas.md`, "La GEX calcolata da noi").
2. **Il negativo viene dal venerdi' 23.** Lo 0DTE e il 22 sono positivi in A; il 23 vale -175,8. **Osservazione**: per la sola giornata di oggi, la parte che scade stasera dice "smorza"; la parte che resta dopo stasera dice "amplifica". Lo 0DTE perde gamma a scadenza.
3. **In B pesano gli stessi due strike, 30.000 e 29.500.** Se i dealer fossero corti di tutto [GPP], la copertura peserebbe di piu' li', e amplificherebbe in entrambi i versi. Il segno vero non e' noto: la sintesi del 15 ottobre (`quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`, "Dove si contraddicono", 1) lo lascia aperto.
4. **L'implicita supera la realizzata.** Lo 0DTE e' quotato al 28,1%, le ultime 24 ore hanno realizzato il 20,9%, le ultime 4 ore il 16,5%. Il VXN e' a 25,2. **Osservazione**: il mercato delle opzioni prezza per oggi piu' movimento di quanto NQ ne abbia fatto. Le due annualizzazioni potrebbero non essere omogenee (sotto).
5. **La scala dice che la gamma conta poco su NQ.** 248 future per l'1% sono lo 0,06% del volume di cassa (in B l'1%). In BPVZ l'effetto e' solo simulato, e lo scenario piu' basso e' al 18% del flusso. "Su NQ l'effetto e' piccolo" e' una **nostra estrapolazione**, non un finding. DPS (indiretto) aggiunge un dubbio: sulle scadenze brevissime i market maker abbinano gli ordini piu' che coprirsi, quindi la GEX e' un tetto (lettura nostra).
6. **I paper, come ipotesi.** BDLM: sui future S&P il momento dell'ultima mezz'ora c'e' solo nei giorni di gamma netta negativa (6,63, t 4,78, contro 0,82, t 1,03; Tab. 7). Su NQ il momento c'e' senza condizione (t 7,97), ma la condizione sulla gamma non e' provata. BB: con gamma del giorno prima alta il giorno dopo si muove meno; il segno dei dealer sugli indici e' assunto. VAPG: l'effetto mediano sulla volatilita' del giorno e' piccolo (-0,08 punti). GJ (pinning, il prezzo che a scadenza resta vicino a uno strike) vale sui venerdi' seriali dell'S&P, non sulle settimanali di NQ. **Nessuno dei paper usa NDX o NQ** (`quant/studi/opzioni-gamma-es/1-idea.md`, "Il finding").

## I Livelli E Le Date

**Nessun paper prova questi livelli come supporti o resistenze** (scheda, finding 6; l'unico test trovato, Popovici 2026, non e' letto). Valgono come posizionamento.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 29.834,7 | zero gamma A, tutte le scadenze | Sopra, la GEX A totale diventerebbe positiva nel modello: dice che il regime e' cambiato, non cosa fa il prezzo |
| 29.816,6 | zero gamma A, entro 7 giorni | Lo stesso, per la settimana |
| 29.747,1 | zero gamma A, 0DTE | Sotto, anche lo 0DTE passa negativo: tutti e tre gli orizzonti in A negativo |
| 30.000 | call wall di tutti gli orizzonti, primo peso B | Posizionamento: lo strike dove le call pesano di piu'; 0,75% sopra |
| 29.500 | put wall di tutti gli orizzonti, strike piu' pesante (A -82,0) | Posizionamento: lo strike dove le put pesano di piu'; 0,93% sotto |
| 29.900, 30.100 | strike pesanti con GEX A positiva | 29.900: volume call di oggi doppio dell'OI, posizioni nuove che l'OI non vede |
| 29.000, 29.100 | strike pesanti di put, OI 1.693 e 1.125 | Posizionamento lontano (-2,6%), anche put wall del 23 ottobre |
| 21 ott., 22:00 | scadenza 0DTE (16:00 New York) | Esce la parte di GEX A positiva (+48,8) |
| 23 ott., 22:00 | scadenza settimanale del venerdi' | Esce la scadenza piu' pesante (A -175,8) |
| 18 dic. | trimestrale (SOQ alle 15:30 italiane) | La scadenza con piu' OI, letta incompleta |

## Cosa Non Sappiamo

- **Il segno dei dealer** [GPP, VAPG]: A e B sono due ipotesi, nessuna e' misurata sulle opzioni CME di NQ.
- **La catena incompleta**: 21-23 ottobre complete (455/480, 477/480, 480/480 quotate, OI stabile). La **trimestrale di dicembre** ha 110 opzioni quotate su 374, e l'OI e' salito da 8.934 a 18.680 in 7 letture senza fermarsi. E' un terzo dell'OI letto: la sua GEX (-2,3 in A, -219,8 in B) e' probabilmente sottostimata. Le settimanali dal 26 ottobre al 27 novembre e il 30 novembre sono quotate da 10 a 121 su 222-480, e quasi tutte con OI instabile. **Saltate** perche' senza nessuna quotazione: 3, 5 e 13 novembre.
- **Il confronto col 16 ottobre non e' omogeneo**: ci sono di mezzo la scadenza mensile del 16, una catena letta in modo diverso e mancano i calcoli del 19 e del 20.
- **L'OI e' di ieri**: non vede gli 0DTE aperti e chiusi oggi. Il volume copre la giornata CME dalle 00:00 italiane; quando si azzera non e' ancora verificato (`volume_finestra`).
- **La copertura arriva davvero sul future?** Non lo misura nessuno dei nove paper (sintesi, "Il perche' comune").
- **lacuna del collettore**: la base dei tempi di `vol_atm` e `mossa_attesa` (giorni di calendario o sedute) non e' nel JSON. Il confronto con la realizzata (x 276 x 252) potrebbe quindi non essere omogeneo.
- **lacuna del collettore**: `oi_cambiato_rispetto_al_calcolo_precedente` e' `null` (primo calcolo del giorno). Manca un confronto con l'ultimo calcolo del giorno di borsa prima; lo storico `quant/dati/NQ/gex/` salta il 19 e il 20 ottobre.
- **lacuna del collettore**: la scala c'e' solo sul volume medio di cassa. Mancano la stessa scala sul volume delle 24 ore e quella del solo 0DTE.
- **lacuna del collettore**: per ogni strike pesante manca l'orizzonte da cui viene (tutte le scadenze o una sola).

## Per Il Quadro

1. **Gli strike pesanti accanto all'order flow** (`confluenze.json`, `stesso_dato: false`): 29.600 con la VAL del 20 ottobre (29.599); 29.835 (zero tutte) con la VAH del 20 (29.842) e la VAL della notte (29.844); 29.900 con il POC della notte (29.888); 29.747 (zero 0DTE) con il POC composito a 90 sedute (29.735); 29.100 con la VAL composito 90 (29.085). Coincidenze di prezzo, non conferme.
2. **COT e gamma**: in `in-breve.json` nessuna confluenza unisce gamma e COT. I livelli COT che vi compaiono stanno da 30.549 in su; il livello gamma piu' alto vicino e' la call wall a 30.000. Da confrontare con l'elenco intero dei livelli della fase COT.
3. **L'implicita 0DTE al 28,1% contro la realizzata al 20,9%**: c'e' un evento oggi nel calendario della macro che la spieghi?
4. **Il peso**: 248 future per l'1%, lo 0,06% del volume. Il regime gamma e' un contesto debole accanto al delta e al volume dell'order flow; il prezzo e' a 31-57 punti dai tre zeri.
