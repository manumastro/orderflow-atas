# Fase 3 · La Gamma · 28/10/2026 · ES (ESZ6)

**Stato**: parziale (catena incompleta: 13 scadenze su 21 con l'open interest ancora in crescita, fra cui le due piu' pesanti dopo lo 0DTE; 10 scadenze saltate; niente dati SPX per confrontare l'X-Ray)
**Dati**: `dati/in-breve.json` (sezione `gamma`), `dati/gex-ESZ6.json` (letto per `per_strike`, `per_scadenza`, completezza), `dati/raccolta.json`. GEX calcolata dalle 10:17 alle 10:23 ora italiana (09:17-09:23 UTC), future 7.766,25 alle 10:23 (bridge `/opzioni` e M1, differenza 0). Volume medio e volatilita' realizzata da `in-breve.json`; VIX e VXN da `1-macro.md`. Errori della raccolta: nessuno. Primo calcolo del giorno: nessun confronto con un calcolo precedente.

**In una riga**: con la convenzione dei fornitori il totale e' negativo e il prezzo sta poco sotto lo zero gamma, ma il segno dipende quasi tutto da una scadenza (venerdi' 30) letta a meta'; le opzioni di oggi da sole sono positive, e la copertura stimata e' piccola rispetto al volume di ES.

## In Parole Semplici

**Il punto**: chi vende le opzioni su ES deve coprirsi comprando o vendendo future. Oggi il conto dice "piccola spinta ad amplificare i movimenti" sul totale e "piccola spinta a frenarli" sulle opzioni che scadono stasera. Il segno del totale e' fragile, e in ogni caso la copertura vale meno dell'1% del volume di un giorno.

**Dal piu' ampio al piu' vicino**

1. **Tutte le scadenze**: il totale, con l'ipotesi dei fornitori, e' negativo; il prezzo (circa 7.766) sta circa 17 punti sotto il prezzo dove cambierebbe segno (circa 7.784).
   → In teoria i dealer accompagnano i movimenti; ma il segno lo decide la scadenza di venerdi' 30, letta solo in parte.
2. **Questa settimana**: stesso segno, molto piu' debole; il cambio di segno e' a circa 7.772, a 6 punti.
   → Il prezzo e' quasi sulla linea: il regime "di settimana" non e' definito.
3. **Le opzioni che scadono stasera alle 21:00**: totale positivo, cambio di segno a circa 7.745, 21 punti sotto.
   → In teoria per queste opzioni i dealer frenano i movimenti finche' il prezzo sta sopra 7.745.
4. **Quanto conta**: per un movimento dell'1% (circa 78 punti) la copertura stimata e' di circa 4.000 contratti, lo 0,3% del volume medio di un giorno di cassa.
   → E' una spinta piccola; anche nel caso peggiore (tutti corti di opzioni) resta circa il 3%.
5. **La mossa attesa di oggi**: le opzioni di stasera prezzano circa 56 punti in piu' o in meno, il doppio di quanto ES si e' mosso nelle ultime 24 ore.
   → Il prezzo delle opzioni contiene la Fed delle 19:00.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.700 | lo strike con piu' opzioni put in assoluto (oggi, venerdi', tutte) | la zona dove la copertura pesa di piu'; coincide con un vecchio prezzo piu' scambiato |
| circa 7.772-7.784 | dove la gamma cambia segno (settimana, tutte) | sopra, il totale diventa positivo nel modello: il regime cambia nome |
| circa 7.800-7.815 | gli strike con piu' call (7.815 quasi tutto di stasera) | la seconda zona di peso; vicino al massimo del valore delle ultime settimane |

**Quando**: 19:00 decisione della Fed · 19:30 conferenza · 20:30-21:00 l'ultima mezz'ora di cassa · 21:00 scadono le opzioni di oggi · venerdi' 30 alle 21:00 la scadenza di fine mese
**Cambia idea se**: il prossimo calcolo, con la scadenza del 30 letta per intero, cambia il segno del totale; o il prezzo esce dalla fascia 7.745-7.784, dove il segno del modello cambia.
**Quanto e' provato**: le quantita' di opzioni sono misurate (open interest di ieri, volume di stanotte); chi e' lungo e chi e' corto e' un'ipotesi; che la gamma cambi la volatilita' lo dicono alcuni paper sull'S&P, con effetti piccoli; nessun paper prova i "muri" come supporti o resistenze.

**Le parole**: **dealer** = chi vende le opzioni e si copre sul future; **gamma** = quanto cambia la copertura quando il prezzo si muove; **GEX** = la gamma di tutte le opzioni in dollari per un movimento dell'1%; **zero gamma** = il prezzo dove il totale cambia segno; **strike** = il prezzo scritto nel contratto di opzione; **open interest** = contratti aperti a fine di ieri; **0DTE** = opzioni che scadono oggi; **muro** = lo strike con piu' gamma di call (sopra) o di put (sotto).

## I Fatti

Fonte: `gex-ESZ6.json`, 10:17-10:23 ora italiana, opzioni CME sul future ESZ6, 50 $ a punto. GEX in miliardi di $ per un movimento dell'1%. **Convenzione A** (fornitori, ATAS, SqueezeMetrics): dealer lunghi le call, corti le put. **Convenzione B** [GPP]: dealer corti di tutto. Zeri cercati su +-8%, 120 strike per lato.

| orizzonte | GEX A | GEX B | zero gamma A | prezzo - zero | muro call | muro put | piu' peso in B |
|---|---|---|---|---|---|---|---|
| tutte | -1,57 | -16,34 | 7.783,7 | -17,4 punti | 7.800 | 7.700 | 7.700, 7.800, 7.815 |
| entro 7 giorni | -0,50 | -11,98 | 7.772,3 | -6,0 | 7.800 | 7.700 | 7.700, 7.800, 7.815 |
| 0DTE (28/10) | +1,31 | -6,00 | 7.745,1 | +21,2 | 7.815 | 7.700 | 7.815, 7.700, 7.800 |

Le scadenze che pesano (A; `gex_per_scadenza`, `scadenze`): 30/10 fine mese -1,81 (open interest 116.399, **instabile**: da 15.524 a 116.399 in 7 letture, ancora in crescita); 28/10 +1,31 (64.726, stabile); 20/11 -0,30 (100.220, instabile); 18/12 trimestrale -0,30 (141.873, instabile: da 56.237). **La sola 30/10 vale piu' del totale**: e' lei a rendere negativo "tutte" e "entro 7 giorni".

Completezza: 21 scadenze lette, 10 saltate (5/11, 18/11, 24/11, 25/11, 1/12, 2/12, 3/12, 4/12, 10/12, 11/12: 0-22 opzioni quotate). Quotate su lette: 0DTE 152/458, 30/10 98/420 (105 strike per lato), 29/10 79/324 (81 per lato), 18/12 35/366. OI stabile solo in 8 scadenze su 21.

Strike pesanti, open interest (OI) e volume di oggi (call/put; `per_strike`, +-6%; volume dalle 23:00 italiane del 27/10, il giorno CME, alle 10:2x; **il volume non ha verso**: non si sa se e' comprato o venduto):

| strike | OI call | OI put | vol call | vol put | GEX A netta (mln $) | di quale scadenza |
|---|---|---|---|---|---|---|
| 7.700 | 6.407 | 31.229 | 205 | 377 | -1.986 | put 16.001 del 30/10, 6.515 di oggi |
| 7.800 | 10.355 | 14.626 | 643 | 61 | -457 | 30/10 call 6.005, put 11.033; 20/11 |
| 7.815 | 4.953 | 20 | 449 | 0 | +742 | tutte di oggi |
| 7.850 | 8.543 | 2.151 | 647 | 0 | +460 | n.d. (non fra i primi cinque) |
| 7.835 | 2.381 | 27 | 279 | 1 | +222 | 1.885 di oggi |
| 7.400 | 4.454 | 21.333 | 7 | 430 | -294 | volume put quasi tutto del 18/12 (417) |
| 8.200 | 18.455 | 427 | 124 | 0 | +204 | n.d. |

Il volume piu' alto per strike: **7.875 call 1.087** (OI 1.700), 7.850 call 647, 7.800 call 643, 7.650 put 549, 7.830 call 547, 7.755 put 523, 7.780 call 516, 7.740 put 464. Le put trattate stanno fra 7.620 e 7.760, le call fra 7.765 e 7.875: il volume e' sui due lati fuori dal denaro, piu' alto sulle call lontane.

Mossa attesa (Black-76, `scadenze`) contro la realizzata (`in-breve`, log a 5 minuti, 276 x 252):

| scadenza | vol. implicita ATM | mossa attesa | | realizzata | valore |
|---|---|---|---|---|---|
| 28/10 | 20,5% | +-55,7 punti | | ultime 4 ore | 6,6% |
| 30/10 | 16,9% | +-107,5 | | ultime 24 ore | 9,6% |
| 20/11 | 14,2% | +-280,5 | | ultime 48 ore | 10,9% |
| 18/12 | 18,1% | +-527,0 | | VIX / VXN (macro) | 17,6 / 24,9 |

La scala (`in-breve.gamma.scala`): 1% = 77,7 punti. A: 4.036 future per 1%, 0,33% del volume medio di cassa (1.235.431 contratti, 20 sedute). B: 42.089 future, 3,41%.

## Cosa Dicono Per ES

- **Il regime A e' negativo, ma non e' un dato solido.** Il segno del totale viene dalla 30/10, letta mentre l'open interest arrivava ancora (`oi_stabile` falso dopo 7 letture). Un OI che si muove cosi' e' una catena arrivata a pezzi, non posizioni nuove ([le-opzioni-in-atas](../../../quant/metodo/le-opzioni-in-atas.md#la-gex-calcolata-da-noi)). Il segno puo' cambiare con la stessa scadenza letta per intero.
- **Due regimi sovrapposti.** Per la teoria lo 0DTE (positivo sopra 7.745) frena fino alle 21:00; le scadenze di venerdi' e oltre spingono nel verso opposto. Il prezzo sta fra i due zeri (7.745 e 7.784): **osservazione**, nessun paper misura due orizzonti in conflitto.
- **La convenzione B** mette il peso a 7.700, 7.800, 7.815: se i dealer fossero corti di tutto [GPP], li' la copertura amplificherebbe di piu'. **Il segno vero non si conosce**: GPP (SPX 1996-2001) li trova corti quasi ovunque, VAPG (SPX 2020-2023) di solito lunghi; nessuna misura esiste sulle opzioni CME sul future.
- **7.800 e' un "muro call" con piu' put che call**: la GEX A netta e' negativa. Il nome "muro" viene solo dalla gamma delle call.
- **La scala dice che conta poco.** 0,33% (A) o 3,4% (B) del volume medio: BPVZ vede effetti sull'autocorrelazione solo con il dealer al 18% del flusso o piu', e sotto non ha scenari; DPS dice che sulle scadenze brevissime i market maker si coprono piu' abbinando ordini che sul future. "Effetto piccolo su ES" e' **nostra estrapolazione** ([sintesi del 15/10](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
- **I paper, come ipotesi.** BB: gamma alta il giorno prima, giornata meno ampia (segno assunto). BDLM: sui future S&P l'ultima mezz'ora (20:30-21:00 italiane) segue il resto della giornata solo nei giorni di gamma negativa; oggi il segno dipende dall'orizzonte e c'e' la Fed un'ora e mezza prima. VAPG: l'effetto mediano sulla volatilita' e' piccolo. GJ: il future S&P chiude vicino a uno strike piu' spesso dell'atteso nelle scadenze seriali del venerdi' (21,3% contro 15%). **Nessuno e' una regola** ([scheda opzioni-gamma-es](../../../quant/studi/opzioni-gamma-es/1-idea.md)).
- **La mossa attesa e' il doppio della realizzata** (20,5% contro 9,6%): la Fed e' nel prezzo. **Osservazione**: non dice in che verso.
- **X-Ray non e' questa GEX.** L'X-Ray di ATAS su ES usa le opzioni SPX (OptionsDepth, per tipo di partecipante); noi usiamo le opzioni CME sul future ES. Strumenti diversi, livelli in punti dell'indice contro punti del future (c'e' la base fra i due): i numeri non si confrontano uno a uno.

## I Livelli E Le Date

Nessun paper prova questi livelli come supporti o resistenze (Popovici 2026, l'unico test trovato, non e' letto).

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.700 | muro put di tutti gli orizzonti; strike piu' pesante (OI put 31.229) | posizionamento: dove la copertura e' piu' grande in entrambe le convenzioni |
| 7.745,1 | zero gamma A dello 0DTE | sotto, anche lo 0DTE diventa negativo nel modello: tutti gli orizzonti stesso segno |
| 7.772,3 / 7.783,7 | zero gamma A, settimana / tutte | sopra, il modello A diventa positivo: il regime cambia nome (fragile, vedi 30/10) |
| 7.800 / 7.815 | muro call tutte e settimana / muro call 0DTE | posizionamento: la seconda zona di peso; 7.815 vale solo fino alle 21:00 |
| 7.850 / 7.875 | OI call 8.543 / il volume piu' alto di stanotte (1.087 call) | posizionamento: dove si sono concentrate le call di oggi |
| 28/10 19:00-19:30 | Fed e conferenza (`1-macro.md`) | l'evento che la vol. implicita dello 0DTE sta prezzando |
| 28/10 21:00 | scadenza dello 0DTE (16:00 di New York) | dopo, la gamma positiva di oggi sparisce e resta il segno delle altre |
| 30/10 21:00 | scadenza di fine mese, la piu' pesante in A | il segno del totale dipende da lei |

## Cosa Non Sappiamo

- Il segno vero dei dealer [GPP, VAPG]; e se la gamma diventa davvero ordini sul future [DPS].
- L'open interest e' di ieri e non vede gli 0DTE aperti e chiusi oggi.
- Il verso del volume: comprato o venduto non si sa, quindi il volume non dice chi e' lungo.
- L'azzeramento del volume alle 17:00 di Chicago non e' ancora verificato (`volume_finestra`).
- **lacuna del collettore**: la 30/10, la 20/11 e la 18/12 sono lette con l'OI ancora in crescita dopo le riletture; serve una rilettura fino a stabilita' sulle scadenze pesanti, o un segnale che dica quanto della GEX viene da scadenze instabili.
- **lacuna del collettore**: le 10 scadenze saltate non hanno una stima del loro peso.
- **lacuna del collettore**: il volume per strike non e' diviso per scadenza (per 7.875 e 7.850 non si sa quale scadenza ha scambiato).
- **lacuna del collettore**: nessun dato SPX (i livelli dell'X-Ray stanno in campi offuscati) e nessuna base ES-SPX: il confronto con l'X-Ray non si puo' fare.
- **lacuna del collettore**: nessuno storico della GEX di ES nei giorni prima in `dati/`: non si sa se il regime e' nuovo o di sempre.

## Per Il Quadro

- **7.700**: muro put e strike piu' pesante, gia' prezzo piu' scambiato della cassa del 16/10 (`confluenze.json`): da confrontare con i livelli del COT e con il valore delle settimane.
- **7.745-7.784**, la fascia fra gli zeri: contiene il valore della notte (7.758-7.765) e il prezzo piu' scambiato delle 18 sedute (7.755); 7.772 coincide con il massimo del valore di ieri. Il regime del modello cambia dentro il valore dell'order flow.
- **7.800-7.816**: muri call e massimo del valore del composito (7.816,5): la stessa zona da due misure diverse.
- **Il segno**: A negativo ma fragile, scala 0,3% del volume; con la Fed alle 19:00 e la vol. implicita al doppio della realizzata, la macro pesa piu' della gamma? Da confrontare con la fase 1.
