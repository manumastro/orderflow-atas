# Fase 3 · La Gamma · 27 ottobre 2026 (ES)

**Stato**: fatto, con una catena parziale (18 scadenze lette, 13 saltate; vedi Cosa Non Sappiamo).
**Dati**: `dati/in-breve.json` (sezione `gamma`), `dati/confluenze.json`, `dati/raccolta.json`. La GEX di ESZ6 e' del ricalcolo delle 07:37: il primo calcolo, alle 07:23, era fallito con un errore del chart, `no chart matches`. Il future e' 7.711,5 alle 07:37 (bridge M1, differenza 0). L'open interest e' quello della chiusura del 26/10. Il confronto con ieri viene da `quant/dati/ES/gex/2026-10-26/1417Z-ESZ6.json`, delle 15:17. Lo studio di oggi e' su ES per richiesta dell'utente.

**In una riga**: chi vende le opzioni su ES, secondo il modello dei fornitori, si trova in una posizione che amplifica i movimenti. Il prezzo sta circa 30 punti sotto la soglia che separa i due regimi, e il peso si concentra su 7.700.

## In Parole Semplici

**Il punto**: in teoria oggi chi vende le opzioni su ES e si copre spinge il prezzo nella direzione in cui gia' va, invece di frenarlo. Il prezzo e' circa 30 punti sotto la soglia di cambio, a 7.744. La forza in gioco e' piccola rispetto agli scambi del future, e il segno vero nessuno lo conosce.

**Dal piu' ampio al piu' vicino**

1. **La scadenza di dicembre**: e' il gruppo di opzioni con piu' contratti aperti, molte put fra 6.400 e 7.500, lontane dal prezzo.
   → oggi pesa poco sulla copertura: sono opzioni lontane dal prezzo.
2. **La settimana**: il peso e' sulla scadenza di fine mese (venerdi' 30/10), con una montagna di put a 7.700.
   → 7.700 e' il prezzo dove la copertura cambierebbe di piu', in un verso o nell'altro.
3. **Ieri contro oggi**: la soglia di cambio e' salita da circa 7.720 a circa 7.744. Il muro delle call e' passato da 7.725 a 7.760.
   → buona parte del cambio viene da una catena letta meglio oggi, non da posizioni nuove.
4. **Oggi**: per le opzioni che scadono stasera la soglia e' quasi sul prezzo (7.712). La forza complessiva e' circa 8.500 future per un movimento dell'1%.
   → sono meno dell'1% dei contratti scambiati in una seduta: un effetto possibile, non dominante.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.744 | la soglia di cambio: sopra, in teoria, chi vende opzioni frena i movimenti | il regime teorico passa da "amplifica" a "smorza" |
| 7.700 | il prezzo con piu' put aperte: il muro delle put | e' il punto dove la copertura teorica cambia di piu' |
| 7.760 | il prezzo con piu' call aperte vicino al prezzo: il muro delle call | sopra la soglia: zona dove, in teoria, il movimento si frena |

**Quando**: oggi alle 21:00 scadono le opzioni del giorno · domani alle 19:00 la Fed · venerdi' 30/10 alle 21:00 la scadenza di fine mese, la piu' pesante della settimana.
**Cambia idea se**: il prezzo passa e resta sopra 7.744. Oppure il prossimo calcolo, con l'open interest di stasera, sposta la soglia o i muri di decine di punti.
**Quanto e' provato**: la forza e la soglia sono calcoli di un modello sui dati di ATAS. Che la gamma negativa renda i movimenti piu' ampi lo dicono i paper sulle opzioni dell'indice S&P, non su queste opzioni sul future. Che 7.700 o 7.760 fermino il prezzo non lo prova nessuno studio.

**Le parole**: **opzione** = il diritto di comprare (call) o vendere (put) a un prezzo fissato, lo strike; **dealer** = chi vende le opzioni e si copre comprando o vendendo future; **gamma** = quanto deve cambiare la sua copertura quando il prezzo si muove; **GEX** = quella gamma sommata su tutte le opzioni, in dollari di future per l'1%; **soglia di cambio (zero gamma)** = il prezzo dove la GEX cambia segno; **muro** = lo strike con piu' gamma di call o di put; **open interest** = i contratti aperti a fine di ieri; **0DTE** = le opzioni che scadono oggi.

## I Fatti

GEX in milioni di $ di future per un movimento dell'1% (77,1 punti), con il moltiplicatore ES di 50 $. Le due convenzioni: **A**, quella dei fornitori e di ATAS, assume i dealer lunghi le call e corti le put (GEX = call - put). **B** [GPP] li assume corti di tutto: dice solo dove pesa. Fonte: `in-breve.json` > `gamma.orizzonti`, calcolo delle 07:37.

| orizzonte | GEX A | GEX B | zero gamma A | distanza dal prezzo | call wall | put wall | dove pesa B |
|---|---|---|---|---|---|---|---|
| tutte | -3.278 | -15.230 | 7.743,9 | +32,4 | 7.760 | 7.700 | 7.700, 7.800, 7.750 |
| entro 7 giorni | -2.803 | -12.236 | 7.740,6 | +29,1 | 7.760 | 7.700 | 7.700, 7.800, 7.750 |
| 0DTE (27/10) | -34 | -5.746 | 7.711,9 | +0,4 | 7.760 | 7.700 | 7.760, 7.750, 7.700 |

**Ieri contro oggi** (convenzione A, tutte le scadenze):

| | 26/10 alle 15:17 | 27/10 alle 07:37 |
|---|---|---|
| future | 7.712,5 | 7.711,5 |
| GEX A | -530 | -3.278 |
| zero gamma | 7.720,5 | 7.743,9 |
| call wall / put wall | 7.725 / 7.700 | 7.760 / 7.700 |
| open interest del 30/10 | 54.900 (non stabile, 84 opzioni quotate su 480) | 119.895 (stabile, 129 su 480) |
| open interest del 18/12 | 129.475 (non stabile) | 187.838 (non stabile) |
| scadenze 27/10 e 29/10 | saltate (0 quotate) | lette |

**Gli strike pesanti** (`gamma.strike_pesanti`; OI = open interest, GEX netta A):

| strike | OI call | OI put | GEX netta A | nota |
|---|---|---|---|---|
| 7.700 | 4.166 | 20.801 | -1.664 | put wall di tutte le scadenze, della settimana e dello 0DTE |
| 7.760 | 3.930 | 243 | +540 | call wall di tutte le scadenze, della settimana e dello 0DTE |
| 7.800 | 7.168 | 11.308 | -358 | secondo peso B |
| 7.615 / 7.650 | 51 / 225 | 4.976 / 3.348 | -313 / -246 | put sotto il prezzo |
| 7.525 / 7.500 / 7.300 | poche | 9.880 / 15.706 / 30.450 | -272 / -217 / -268 | put lontane, molto open interest |

**Le scadenze principali** (`gamma.scadenze`): la mossa attesa e' quella implicita nel prezzo delle opzioni, fino alla scadenza.

| scadenza | OI | volatilita' implicita al prezzo | mossa attesa | GEX A | zero gamma | quotate |
|---|---|---|---|---|---|---|
| 27/10 (0DTE) | 96.463 | 14,4% | 43,4 punti | -34 | 7.711,9 | 288/480, stabile |
| 28/10 | 16.791 | 16,2% | 81,5 | -259 | 7.756,9 | 102/480, stabile |
| 30/10 fine mese | 119.895 | 17,0% | 129,1 | -2.370 | 7.884,1 | 129/480, stabile |
| 20/11 | 58.935 | 17,6% | 351,4 | +77 | 7.668,5 | 28/434, non stabile |
| 18/12 trimestrale | 187.838 | 17,9% | 523,3 | -433 | 8.007,6 | 56/368, non stabile |

**Volatilita' realizzata** (`gamma.realizzata`: rendimenti log a 5 minuti, annualizzati con 276 x 252): 6,6% sulle ultime 4 ore, 10,6% sulle 24 ore, 11,6% sulle 48 ore. VIX 18,4 contro una realizzata a 20 sedute di 9,6 (`1-macro.md`).

**La scala** (`gamma.scala`, sul volume medio di cassa di 20 sedute, 1.241.732 contratti): con la convenzione A, 8.501 future per l'1%, cioe' lo 0,68% del volume. Con la B, 39.498 future, il 3,18%. Ieri, con la A: circa 1.400 future (calcolo nostro, -530 / (7.712,5 x 50 $)).

## Cosa Dicono Per ES

- **Il regime (convenzione A).** La GEX e' negativa su tutti e tre gli orizzonti, e il prezzo sta circa 30 punti sotto lo zero gamma di tutte le scadenze e della settimana. In teoria i dealer, corti di gamma, comprano dopo i rialzi e vendono dopo i ribassi, e allargano il movimento ([`1-idea.md`](../../../quant/studi/opzioni-gamma-es/1-idea.md), Il Finding 2). Per lo 0DTE lo zero gamma e' sul prezzo (7.711,9) e la GEX e' quasi nulla (-34): le opzioni di oggi non spingono in nessun verso. La GEX negativa sta soprattutto sulla scadenza del 30/10 (-2.370).
- **La convenzione B** [GPP]. Il peso massimo della copertura sta a 7.700, 7.800 e 7.750. Per lo 0DTE sta a 7.760, 7.750 e 7.700. Il segno vero dei dealer non si conosce: GPP, sulle SPX, li trova corti quasi ovunque; VAPG li trova di solito lunghi (sintesi del 15/10, Dove Si Contraddicono 1). Questi sono dati di SPX: sulle opzioni CME sul future ES nessuna misura del segno ([sintesi](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md), Cosa Resta Senza Risposta).
- **Il confronto con ieri e' falsato dalla catena.** La GEX A e' passata da -530 a -3.278 e lo zero gamma e' salito di circa 23 punti. Ieri pero' mancavano le scadenze del 27/10 e del 29/10, e l'open interest del 30/10 era ancora in arrivo (da 0 a 54.900 in sette letture, non stabile). Oggi il 30/10 ha 119.895 contratti, stabili. **Osservazione**: buona parte del salto e' una catena letta meglio, non posizioni nuove ([`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md), "Il segnale dell'open interest"). Lo spostamento del muro delle call (da 7.725 a 7.760) segue la scadenza di ieri, che pesava su 7.715 e 7.725 ed e' finita.
- **La mossa attesa contro la realizzata.** La volatilita' implicita degli 0DTE (14,4%) e della settimana (16-17%) sta 1,4-1,6 volte sopra la realizzata a 24-48 ore (10,6-11,6%). Le opzioni mettono in conto piu' movimento di quello che c'e' stato. E' coerente con la Fed di domani, ma il legame e' una nostra lettura, non un dato.
- **La scala.** Con la A servono circa 8.500 future per l'1%, lo 0,68% di una seduta di cassa. E' poco piu' di NQ (0,4-0,5%, [`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md)), ed e' molto sotto il 18-52% del flusso a cui BPVZ simula un effetto. BPVZ non ha scenari sotto il 18%: che l'effetto sia piccolo e' una **nostra estrapolazione**. Con la B, che e' un tetto, il 3,2%. DPS suggerisce che sulle scadenze brevissime la copertura arrivi meno del calcolo: la GEX e' un tetto, ed e' una lettura nostra (sintesi, Dove Convergono 3).
- **I paper, come ipotesi.** Su ES sono piu' pertinenti che su NQ. BDLM trova il momento dell'ultima mezz'ora sui future S&P solo nei giorni di gamma negativa: 6,63 (t 4,78) contro 0,82 (t 1,03) [BDLM Tab. 7]. BB trova, con gamma alta il giorno prima, giornate meno ampie [BB]. GJ studia proprio le opzioni sul future S&P, sulle scadenze del venerdi'. **Ma le opzioni lette qui sono quelle CME sul future ES, non le SPX** su cui stanno BDLM, BB, VAPG e GPP: il segno e il peso dei dealer possono essere diversi. Nessuno di questi finding e' una regola: l'ipotesi B della scheda non ha prove ([`1-idea.md`](../../../quant/studi/opzioni-gamma-es/1-idea.md), Le Prove).

## I Livelli E Le Date

Riserva valida per ogni riga: **nessun paper prova questi livelli come supporti o resistenze** ([`1-idea.md`](../../../quant/studi/opzioni-gamma-es/1-idea.md), finding 6). Servono a descrivere, non a decidere.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.743,9 (7.740,6 per la settimana) | zero gamma A, tutte le scadenze | posizionamento: sopra, il regime teorico diventa "smorza" |
| 7.760 | call wall di tutti e tre gli orizzonti, OI call 3.930 | posizionamento: lo strike di call con piu' gamma, oltre lo zero gamma |
| 7.711,9 | zero gamma dello 0DTE | descrive l'equilibrio delle opzioni di oggi; vale fino alle 21:00 |
| 7.700 | put wall di tutti e tre gli orizzonti, OI put 20.801, GEX netta -1.664 | posizionamento: lo strike dove la copertura teorica cambia di piu' |
| 7.800 | secondo peso B, OI 7.168 call e 11.308 put | posizionamento sopra il prezzo |
| 7.650 / 7.615 | put pesanti sotto il prezzo | posizionamento sotto il prezzo |
| 27/10 alle 21:00 | scadenza 0DTE (16:00 di New York; gli USA sono ancora in ora legale) | dopo, lo 0DTE esce dal calcolo |
| 28/10 alle 19:00 | Fed (`1-macro.md`) | evento che puo' spostare la volatilita' implicita e quindi la GEX |
| 30/10 alle 21:00 | scadenza di fine mese, GEX A -2.370 | la scadenza piu' pesante della settimana; dopo, il quadro cambia |

## Cosa Non Sappiamo

- **Il segno dei dealer** su queste opzioni: la convenzione A e' un'ipotesi dei fornitori [GPP, SM]. Con la B il regime non esiste, esiste solo dove pesa.
- **La catena e' incompleta.** Su 31 scadenze, 13 sono saltate (5, 9-13, 16-18 e 25 novembre; 2, 4 e 11 dicembre). Le scadenze vicine hanno 102-288 opzioni quotate su 480. Novembre e dicembre hanno l'open interest **non stabile**: dicembre, 187.838, e' ancora in crescita. La settimana regge, le scadenze lontane meno.
- **L'open interest e' di ieri sera**: non vede gli 0DTE aperti e chiusi oggi.
- **Il confronto con ieri mescola posizioni e completezza**: `oi_cambiato` e' nullo, perche' questo e' il primo calcolo di oggi.
- **Opzioni sul future, non SPX**: i finding dei paper sono su SPX o sui future S&P, non su questa catena.
- Lacuna del collettore: il primo calcolo e' fallito per un chart ricaricato (`no chart matches 'e1a0c7bc'`, 402 secondi). `gex.py` cerca il chart con un id fisso e non lo ritrova dopo che ATAS ricarica i chart.
- Lacuna del collettore: la docstring di `gex.py` (riga 18) dice 20 $ a punto, cioe' NQ. Il JSON non scrive il moltiplicatore usato: per ES e' 50 $ (riga 55, conferma dell'orchestratore). Andrebbe scritto nel JSON.
- Lacuna del collettore: `in-breve.json` non riporta i muri e gli strike di ogni scadenza (`per_scadenza`). Quanta parte di 7.700 e 7.760 stia sullo 0DTE o sul 30/10 non e' scritta: la si deduce dai muri di ogni scadenza.
- Lacuna del collettore: `volume_finestra` dice "00:00 italiane in ora legale". Fino al 1/11 le 17:00 di Chicago sono le 23:00 italiane: la finestra va riscritta con l'ora solare.

## Per Il Quadro

1. **7.700**, put wall con GEX netta -1.664, coincide con il POC di cassa del 16/10 (`confluenze.json`, `stesso_dato` falso). Il minimo della notte e' 7.702,75: confrontalo con il valore dell'order flow.
2. **Lo zero gamma 7.740-7.744** coincide con il POC del 19/10 e con il VAL del 20/10. **7.760** coincide con il VAL del 15/10 e con il VAH del 16/10. **7.800** coincide con i VAH del 15 e del 21/10. Sono gli agganci con il profilo.
3. **I livelli del COT** (`2-cot.md`: 7.851,25, 7.754,5, 7.879,75): 7.754,5 sta fra lo zero gamma e la call wall. Gli altri due sono sopra tutti i livelli delle opzioni.
4. **La GEX negativa e la volatilita' implicita 1,5 volte la realizzata, prima della Fed**: e' coerente con il "mercato calmo che mette in conto movimento" della macro (VIX 18,4 contro 9,6)? La scala resta lo 0,68% del volume.
