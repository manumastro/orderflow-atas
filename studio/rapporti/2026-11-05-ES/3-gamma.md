# Fase 3 · La Gamma · ESZ6 · 5 novembre 2026

**Stato**: parziale. Manca la scala in contratti di ES, manca la mossa attesa per scadenza, e i livelli di X-Ray in prezzi di ES non sono riempiti (vedi Cosa Non Sappiamo).
**Dati**: `dati/in-breve.json` (gamma), `dati/quattro-domande-ESZ6.json`, `dati/macro.json` (VIX). Raccolta delle 16:16 ora italiana (`raccolta.json`): 15 fonti ok, nessun errore. X-Ray: lettore vivo, ultimo giro un secondo prima, nessuna famiglia ferma da piu' di 30 minuti. Spot SPX e GEX dei market maker alle 16:14 italiane (10:14 di New York, 44 minuti di cassa). VIX di Yahoo alle 16:01, VIX ricostruito dalla catena CME alle 16:14. Nessun `gex-ESZ6.json`: per decisione del 5 novembre su ES vale solo X-Ray.

**In una riga**: secondo X-Ray i market maker delle opzioni S&P sono dalla parte che accelera, il prezzo e' circa 270 punti sotto il punto di cambio, e la giornata ha gia' superato il movimento prezzato.

## In Parole Semplici

**Il punto**: chi vende le opzioni, secondo la stima di X-Ray, oggi insegue il prezzo invece di frenarlo. Il punto dove cambierebbe comportamento e' lontano, circa 7.822. Questo dice quanto si puo' muovere il mercato, non in che verso, e sui nostri dati non e' provato.

**Dal piu' ampio al piu' vicino**

1. **La paura**: il VIX e' salito da 16,6 a 20,4 in un giorno. Le opzioni prezzano circa 97 punti di ES al giorno.
   → ES ha gia' fatto circa 165 punti dalla chiusura di ieri: la giornata e' gia' piu' larga del prezzato.
2. **Le opzioni del mese (20 novembre)**: i market maker sono quasi in pari, appena dalla parte che accelera.
   → pesano poco sul comportamento di oggi.
3. **Le opzioni di domani e di oggi**: tutte e due dalla parte che accelera; quelle di oggi, vicino al prezzo, quasi neutre.
   → l'acceleratore vicino al prezzo viene soprattutto dalla scadenza di domani.
4. **Adesso, al prezzo**: acceleratore da qui fino a circa 7.822; il punto dove accelera di piu' e' circa 7.640.
   → in teoria le coperture vanno nel verso del movimento e lo allungano, salendo o scendendo.
5. **Chi scambia oggi**: il retail ha comprato rialzo sulle opzioni, quasi tutto nell'ultima mezz'ora; i clienti professionali lo hanno venduto.
   → e' un flusso di oggi, non una posizione e non una direzione.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.822 | il punto di cambio: sopra, i market maker frenerebbero | il comportamento stimato passa da acceleratore a freno |
| circa 7.640 | dove l'acceleratore e' piu' forte, 9 punti sopra il bordo alto del valore di oggi | in teoria il movimento li' e' il piu' amplificato |
| circa 7.577-7.607 | i due strike piu' pesanti delle opzioni che scadono oggi | uno accelera (7.577), uno frena (7.607): da rileggere col prezzo |

**Quanto e' provato**: X-Ray e' una stima di un fornitore su SPX, con metodo non pubblicato. Su 42 sedute di ES l'acceleratore delle 9:35 non ha dato giornate piu' larghe, anzi un po' piu' strette. Nessun paper prova questi prezzi come supporti o resistenze.

**Le parole**: **market maker** = chi vende le opzioni e si copre sul future; **gamma** = quanto cambia la sua copertura quando il prezzo si muove; **freno / acceleratore** = market maker che comprano i ribassi e vendono i rialzi / che inseguono il prezzo; **punto di cambio (flip)** = il prezzo dove si passa dall'uno all'altro; **strike** = il prezzo scritto sull'opzione; **VIX** = la paura a 30 giorni sull'S&P 500.

## I Fatti

**Il regime, X-Ray** (OptionsDepth su SPX, 16:14 italiane). I prezzi di ES = SPX + base 27,2 (mediana ES-SPX sui minuti di cassa, `in_prezzi_di_es.base`).

| misura | valore | in prezzi di ES |
|---|---|---|
| prezzo | SPX 7.525,3 | ES 7.552,5 |
| gamma al prezzo | -179 (acceleratore) | - |
| zeri di oggi (`observed`) | SPX 7.794,9 e 7.852,3 | circa 7.822 e 7.880 |
| acceleratore piu' forte oggi | SPX 7.613, peso -236,5 | circa 7.640 |
| freno piu' forte oggi | SPX 7.820,5, peso +90 | circa 7.848 |
| zeri di domani (`outlook`) | - | 7.849,2 e 7.925,3 (dal collettore) |
| acceleratore piu' forte domani | SPX 7.585,5, peso -286,2 | circa 7.613 |
| profilo coperto | 201 strike, SPX 7.438-7.938 | da -1,2% a +5,5% dallo spot |

**La GEX dei market maker per scadenza** (unita' di OptionsDepth, conta il segno: positivo = lunghi di gamma, freno).

| scadenza (ora italiana) | call | put | netto | entro l'1% | strike piu' pesanti, SPX (netto) → ES circa |
|---|---|---|---|---|---|
| oggi, 0DTE (22:00) | -24,7 | +15,0 | -9,8 | +0,3 | 7.550 (-16,7) → 7.577; 7.475 (+15,9, put) → 7.502; 7.580 (+15,4, put) → 7.607 |
| domani (6/11, 22:00) | -10,4 | -7,5 | -17,9 | -14,8 | 7.400 (+11,3) → 7.427; 7.345 (-6,4); 7.545 (-5,8) → 7.572 |
| mese (20/11, 15:15) | +30,7 | -33,0 | -2,3 | -6,7 | 7.800 (+16,3) → 7.827; 7.475 (-9,1) → 7.502; 7.000 (-8,7) |

**Il delta scambiato per gruppo, sommato da inizio giornata** (flusso, non posizione; positivo = ha comprato rialzo).

| gruppo | 0DTE oggi | 0DTE ultimi 30 minuti | tutte le scadenze oggi |
|---|---|---|---|
| retail | +17.861 | +14.115 | +20.847 |
| clienti professionali | -3.533 | -2.725 | -7.575 |
| firm | +2.791 | +2.552 | -693 |
| broker-dealer | +664 | +600 | -4.380 |

Pressione di copertura 0DTE: -8.580, era -3.356 trenta minuti prima; il verso non e' documentato.

**Il meteo e il future**: VIX 20,39 (Yahoo, 16:01; ieri 16,61), VIX 9 giorni 20,9, VIX 3 mesi 21,12; VIX ricostruito 22,2 (catena CME, 16:14). Movimento atteso 97 punti al giorno, 70 per il resto di oggi; realizzata a 20 sedute 11,7; implicita meno realizzata +8,7; breve contro mese +0,7 (piatta); skew 9,8 (put care) (`quattro-domande-ESZ6.json`). ES: -165,75 punti (-2,15%) dalla chiusura di ieri 7.721,75; cassa 7.712,5-7.547,75; volume di cassa 660.512 alle 16:16, contro 1,05-1,92 milioni delle ultime cinque sedute intere (`in-breve.json`, orderflow).

## Cosa Dicono Per ES

1. **Il regime**. Tutte le misure di X-Ray dicono acceleratore: al prezzo, oggi, domani, il mese. Non sono segnali opposti. Ma lo 0DTE entro l'1% dal prezzo vale +0,3, quasi zero: vicino al prezzo l'acceleratore viene dalla scadenza di domani (-14,8) e dal profilo. Il prezzo e' circa 270 punti, il 3,5%, sotto il primo zero. Il regime **dice quanto, non dove** ([come-si-leggono-le-opzioni-dal-vivo](../../../quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md), punto 3), e si rilegge in giornata: lo 0DTE cambia col prezzo e col tempo, non con posizioni nuove.
2. **Le due supposizioni sul segno**. Su ES non si calcolano piu' (x-ray-in-vivo, "Come si legge", punto 2). Il confronto con X-Ray, solo come **osservazione** di un giorno: sullo 0DTE il segno stimato e' l'opposto della convenzione A (market maker corti di call e lunghi di put); sul mese coincide con A (lunghi call, corti put). La convenzione B (corti di tutto) torna solo su domani. Il segno vero non si conosce [GPP].

### X-Ray

- **Freschezza**: lettore vivo, nessuna famiglia ferma; spot SPX di 2 minuti prima della raccolta, in cassa (quindi non fermo alla chiusura).
- **Segno stimato**: market maker corti di gamma al prezzo e su tutte e tre le scadenze.
- **Zeri e picchi in prezzi di ES**: zero circa 7.822 (e 7.880); acceleratore piu' forte circa 7.640; freno piu' forte circa 7.848, sopra lo zero. Per domani lo zero sale a 7.849 e l'acceleratore piu' forte scende a circa 7.613.
- **Il flusso**: il retail compra rialzo sullo 0DTE, quasi tutto negli ultimi 30 minuti; e' coerente con market maker corti di call di oggi, ma e' un'**osservazione**, non verificata.
- **La riserva**: stima di un fornitore, su SPX, non provata; il delta accumulato puo' contenere minuti anomali (x-ray-in-vivo, "I minuti anomali").

3. **Gli strike pesanti**. Sullo 0DTE: 7.550 SPX (circa 7.577) accelera, 7.580 e 7.475 (circa 7.607 e 7.502) frenano, per le put. Lo strike 7.475 ha segni opposti fra oggi (+15,9) e il mese (-9,1): non e' un livello netto. La scadenza piu' pesante della settimana per il netto e' domani (-17,9); per le singole gambe e' il mese (+30,7 e -33,0), che pero' quasi si elidono.
4. **La mossa attesa**. Il VIX prezza 97 punti al giorno; la realizzata a 20 sedute e' 11,7, quindi l'assicurazione e' cara (+8,7). Ma oggi, in 44 minuti di cassa, ES ha gia' fatto circa 165 punti: la realizzata di oggi supera da sola il prezzato di circa 1,7 volte. Il VIX ricostruito (22,2) sta 1,8 punti sopra quello di Yahoo, preso 13 minuti prima in un mercato che scendeva: conta come si muove, non il decimale.
5. **La scala**: non si puo' dare. Le unita' di X-Ray non sono dichiarate, e la nostra GEX di ES in $ per 1% non si calcola piu'. Resta il volume del future: 1-2 milioni di contratti a seduta di cassa.

### Le Quattro Domande

- **Regime**: acceleratore al prezzo e sullo 0DTE. Per il video "i livelli tendono a rompersi": **ipotesi**. Esito: su 42 sedute di ES (08/09-04/11) il regime 0DTE delle 9:35 non ha allargato la giornata (0,55 contro 0,74 del movimento atteso, p 0,24, verso opposto; [opzioni-gamma-es](../../../quant/studi/opzioni-gamma-es/1-idea.md), ipotesi D).
- **Meteo**: VIX 20,4, fascia tesa; implicita meno realizzata +8,7, sopra +2, assicurazione cara; struttura piatta; put care. Le soglie sono convenzionali, non provate.
- **Muri sul bordo del valore**: nessuno entro 6 punti; la lista dei muri del collettore e' vuota. A occhio, il piu' vicino e' l'acceleratore piu' forte (circa 7.640), 9 punti sopra il bordo alto del valore di oggi (7.631). Ipotesi E, non provata: un caso solo dal 23/10.
- **Grilletto**: non sono le opzioni, e' l'order flow (fase 4); qui nessun segnale.

**I paper**, come ipotesi. [BB]: con gamma dei dealer alta il giorno dopo e' piu' calmo; oggi la stima e' l'opposto, ma il segno in BB e' assunto. [BDLM §4.1]: sui future S&P il momento dell'ultima mezz'ora c'e' solo nei giorni di gamma negativa (t 4,78), col segno assunto; da noi e' l'ipotesi B2, mai provata. [VAPG]: su SPX la gamma dei market maker e' di solito positiva, negativa almeno in parte di un giorno su quattro, con effetto mediano piccolo. Doshi e altri (2025, preprint): chi fornisce liquidita' sugli 0DTE abbina gli ordini piu' che coprirsi, quindi sul future arriva meno copertura di quanto la gamma faccia supporre. Buis e altri (2024): l'effetto pesa di piu' dove la liquidita' e' bassa, ma e' una simulazione.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| circa 7.880 e 7.822 | zeri di X-Ray di oggi (SPX + 27,2) | posizionamento: sopra 7.822 il regime stimato al prezzo diventa freno |
| circa 7.848 | freno piu' forte di oggi (SPX 7.820,5) | dove il freno stimato sarebbe massimo; lontano |
| circa 7.827 | strike piu' pesante del mese (SPX 7.800, +16,3, freno) | posizionamento della scadenza del 20/11 |
| circa 7.640 | acceleratore piu' forte di oggi (SPX 7.613) | dove l'amplificazione stimata e' massima; 9 punti sopra il bordo alto del valore di oggi |
| circa 7.613 | acceleratore piu' forte di domani (`outlook`) | lo stesso, per la seduta del 6/11 |
| circa 7.607 · 7.577 | 0DTE: put 7.580 (freno) · 7.550 (acceleratore) | strike di oggi vicini al prezzo: nel loro intorno il regime 0DTE puo' girare |
| circa 7.502 | strike 7.475: freno oggi, acceleratore sul mese | segni opposti: non e' un livello netto; 12 punti sopra il muro basso del ping-pong (7.490) |
| circa 7.427 | strike pesante di domani (SPX 7.400, freno) | posizionamento della scadenza del 6/11 |
| oggi 21:30-22:00 | ultima mezz'ora di cassa [BDLM] | da registrare per l'ipotesi B2 (gamma negativa alle 15:30 di New York) |
| oggi 22:00 · 6/11 22:00 | scadenza dello 0DTE · scadenza di domani | dopo le 22:00 lo 0DTE diventa la scadenza di domani |
| 20/11 15:15 | scadenza del mese di X-Ray, terzo venerdi' [BTW] | la finestra di scadenza dell'ipotesi C |

Nessun paper prova questi livelli come supporti o resistenze (1-idea, finding 6).

## Cosa Non Sappiamo

- **Il segno vero**: e' la stima di OptionsDepth su SPX, con metodo non pubblicato. Non sono le opzioni CME su ES.
- **La copertura sotto il prezzo**: il profilo di X-Ray arriva solo a SPX 7.438, l'1,2% sotto lo spot. "Nessuno zero sotto" puo' voler dire solo fuori dalla finestra.
- **La pressione di copertura 0DTE**: il verso non e' documentato; si legge solo che e' cresciuta.
- **Il delta accumulato** puo' contenere minuti anomali (partenza, salti); il JSON non li marca.
- lacuna del collettore: `in_prezzi_di_es.flip` e' vuoto, e `freno_piu_forte` e `acceleratore_piu_forte` sono null, mentre `livelli.observed` ha zeri e picchi. Qui sono convertiti con la base dichiarata (SPX + 27,2) e scritti "circa".
- lacuna del collettore: `muri_sul_profilo.muri` e' vuoto, quindi il controllo dei muri sul bordo del valore non ha girato su niente.
- lacuna del collettore: la scala della gamma di X-Ray in contratti di ES per 1% (le unita' di OptionsDepth non sono dichiarate).
- lacuna del collettore: la mossa attesa per scadenza (0DTE, domani) su ES; c'e' solo quella dal VIX.
- lacuna del collettore: il volume di cassa medio di ES gia' calcolato in `in-breve.json` (qui si citano solo le ultime cinque sedute).

## Per Il Quadro

- **Strike pesanti contro COT e valore**: circa 7.827 (mese) cade dentro la confluenza COT e order flow 7.826,75-7.827,25 (`confluenze.json`); circa 7.640 sta fra il bordo alto del valore di oggi (7.631) e la confluenza 7.649,75-7.653,5. Coincidenze o stesso prezzo per caso?
- **Muri vicini a un bordo, col regime**: nessuno entro 6 punti; il piu' vicino e' circa 7.640 (acceleratore) a 9 punti dal bordo alto di oggi, e circa 7.502 a 12 punti dal muro basso del ping-pong (7.490). Regime: acceleratore (ipotesi E, non provata).
- **Paura contro macro**: VIX da 16,6 a 20,4 e 165 punti gia' fatti contro 97 prezzati: e' coerente con lo shock che legge la macro?
- **Il flusso contro l'order flow**: retail che compra rialzo sullo 0DTE (+14.115 negli ultimi 30 minuti) mentre il future scende: lo conferma il delta del future (cassa -23.684)?
