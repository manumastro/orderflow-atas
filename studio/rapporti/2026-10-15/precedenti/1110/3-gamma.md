# Fase 3 · La Gamma · 2026-10-15

**Stato**: parziale (manca il volume giornaliero di ES, quindi la scala di ES non si misura; la catena
delle scadenze lontane e' incompleta; vedi "Cosa Non Sappiamo")
**Dati**: `dati/gex-NQZ6.json` e `dati/gex-ESZ6.json` (calcolo di `gex.py` alle 10:45 ora italiana,
08:45 UTC; open interest di ieri, 14/10), `dati/sedute-NQZ6.json` (20 sedute di cassa, 17/09-14/10),
`dati/macro.json` (quotazioni delle 10:35; VXN alla chiusura del 14/10, 22:00), `dati/giro.txt` (prezzo
delle 10:45), lo storico `quant/dati/NQ/gex/2026-10-15/` (quattro letture, 10:05-10:45). Raccolta
chiusa alle 10:51 (`raccolta.json`), tutte le voci `ok`. Una scadenza di ES saltata (04/12, 0 opzioni
quotate su 100).

**In una riga**: con la convenzione dei fornitori la GEX e' negativa su NQ e su ES; NQ sta 3,6% sotto
lo zero gamma, ES solo 0,6%; su NQ la copertura vale circa lo 0,7% del volume di cassa per 1% di
movimento.

Le fonti: **[gNQ]** `gex-NQZ6.json`, **[gES]** `gex-ESZ6.json`, **[sed]** `sedute-NQZ6.json`,
**[mac]** `macro.json`, **[giro]** `giro.txt`. La GEX e' in **milioni di $ per 1% di movimento** del future.

Le parole. La **gamma** dice di quanto cambia il delta di un'opzione quando il future si muove. Il
**dealer** (market maker) e' chi vende le opzioni e copre il rischio col future. La **GEX** (gamma
exposure) e' la loro gamma sommata: i dollari di future che comprano o vendono per un movimento
dell'1%. Positiva: il dealer compra i ribassi e vende i rialzi, cioe' **smorza**. Negativa: fa il
contrario, cioe' **amplifica**. Lo **zero gamma** e' il prezzo dove la GEX totale cambia segno. I
**muri** sono gli strike con la gamma call (call wall) o put (put wall) piu' grande. Lo **0DTE** e'
la scadenza di oggi. L'**open interest** (OI) conta i contratti aperti a fine giornata di ieri.

## I Fatti

**Il regime, convenzione A.** Nella convenzione A, quella dei fornitori, di SqueezeMetrics e di ATAS,
i dealer sono lunghi le call e corti le put; GEX = call - put. Il future del calcolo e' 29.854 per
NQ e 7.752 per ES [gNQ, gES, 10:45].

| | GEX A | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| NQ tutte (18 scadenze) | -1.719,4 | 30.927,7 | sotto di 1.073,7 (3,60%) | 30.000 (98,7) | 30.000 (362,7) |
| NQ entro 7 giorni | -1.453,9 | nessuno entro +-4% | negativa su tutta la griglia | 30.000 (86,9) | 30.000 (326,3) |
| NQ 0DTE 15/10 | -584,8 | 30.502,7 | sotto di 648,7 (2,17%) | 30.300 (12,6) | 29.700 (62,2) |
| ES tutte (31 scadenze) | -4.494,0 | 7.795,6 | sotto di 43,6 (0,56%) | 8.000 (771,2) | 7.700 (1.374,3) |
| ES entro 7 giorni | -2.830,9 | 7.788,1 | sotto di 36,1 (0,47%) | 7.775 (434,1) | 7.650 (515,9) |
| ES 0DTE 15/10 | -2.011,4 | 7.785,8 | sotto di 33,8 (0,44%) | 7.800 (178,9) | 7.700 (446,7) |

Fra parentesi la gamma call o put del muro. Stabilita' su NQ, quattro letture dalle 10:05 alle
10:45: GEX A da -1.668 a -1.719, zero gamma da 30.921 a 30.928, GEX B da -3.716 a -4.014 [storico].

**La convenzione B** [GPP]. Qui i dealer sono corti di tutto: la GEX e' negativa ovunque, e dice solo
**dove** la copertura pesa. Il segno vero non si conosce, in nessuna delle due.

| | GEX B | dove pesa di piu' (in ordine) |
|---|---|---|
| NQ tutte | -4.013,9 | 30.000, 29.500, 29.700 |
| NQ entro 7 giorni | -3.027,0 | 30.000, 29.500, 29.700 |
| NQ 0DTE | -979,5 | 29.800, 29.700, 29.750 |
| ES tutte | -21.609,0 | 7.700, 8.000, 7.775 |
| ES entro 7 giorni | -9.366,4 | 7.775, 7.650, 7.700 |
| ES 0DTE | -5.896,1 | 7.700, 7.800, 7.750 |

**Gli strike pesanti di NQ**, da `per_strike` (somma di tutte le scadenze, entro +-4%) [gNQ]. Il
volume e' quello di oggi fino alle 10:45, e non dice il lato.

| strike | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|
| 30.000 | 1.033 / 3.690 | 547 / 1.108 | 98,7 | -362,7 | -264,0 |
| 29.500 | 254 / 3.217 | 13 / 824 | 14,2 | -211,0 | -196,8 |
| 29.700 | 129 / 1.096 | 16 / 790 | 15,4 | -149,0 | -133,6 |
| 29.750 | 128 / 817 | 82 / 591 | 13,3 | -113,1 | -99,8 |
| 29.000 | 270 / 2.144 | 14 / 1.095 | 8,1 | -81,7 | -73,6 |
| 29.800 | 427 / 722 | 93 / 349 | 51,2 | -96,4 | -45,2 |
| 30.100 | 543 / 359 | 450 / 25 | 70,0 | -39,6 | +30,4 |
| 30.500 | 1.255 / 1.020 | 828 / 55 | 67,9 | -50,3 | +17,6 |
| 31.000 | 983 / 530 | 30 / 123 | 28,5 | -15,2 | +13,3 |

Sotto 30.100 la netta A e' negativa su quasi ogni strike; da 30.100 in su e' vicina a zero o positiva.

**Gli strike pesanti di ES** [gES]. 7.700: OI 7.499 / 31.102, netta A -1.107,1. 7.500: -576,3.
7.650: -510,3. 7.600: -502,6. Positivi: 8.000 (OI call 29.968) +603,6; 7.900 +306,5; 8.050 +295,2.
Vicino al prezzo, 7.775 (+63,7) e 7.800 (-59,7) hanno molta gamma sia call sia put.

**Le scadenze della settimana, per open interest.**
- NQ: 16/10 16.207, 0DTE 8.635, 19/10 2.192, 21/10 1.597, 20/10 1.480. La piu' pesante e' il
  **16/10**, il terzo venerdi' di ottobre (la scadenza mensile; il bridge la chiama "Weekly"). I suoi
  strike: 30.000 (605 call / 1.280 put), 30.500 (714 / 467), 29.500 (92 / 1.052), 30.250, 29.700.
  La trimestrale del 18/12 ne ha 52.002 [gNQ].
- ES: 0DTE 133.429, 16/10 57.857, 23/10 31.095. Strike del 16/10: 8.100 call 6.885, 7.250 put 6.818,
  7.400 put 6.166, 7.650, 7.775. Trimestrale 598.723 [gES].

**La mossa attesa e la volatilita' realizzata.** La volatilita' implicita ATM (al denaro) e' quella
che i prezzi delle opzioni contengono, calcolata con Black-76 su tempo di calendario (365 giorni). La
mossa attesa e' a una deviazione standard fino alla scadenza [gNQ, gES]. La realizzata viene dalla
macro: 20 giorni, annualizzata, da chiusure Yahoo [mac, 10:35].

| | 15/10 (0DTE) | 16/10 | 19/10 | 20/10 | 18/12 | realizzata 20 g | indice |
|---|---|---|---|---|---|---|---|
| NQ vol ATM | 33,7% | 27,4% | 19,5% | 19,9% | 20,3% | 19,0% | VXN 22,7 (14/10) |
| NQ mossa | +-360,3 | +-518,9 | +-642,6 | +-726,0 | +-2.546,4 | | |
| ES vol ATM | 19,5% | 17,2% | 11,4% | 11,1% | 13,8% | 11,1% | VIX 17,52; VIX9D 14,59 |
| ES mossa | +-54,2 | +-84,5 | +-97,8 | +-105,5 | +-450,1 | | |

Rapporto implicita / realizzata: NQ 1,77 (0DTE), 1,44 (16/10), 1,03 (19/10); ES 1,76, 1,55, 1,03.
La deviazione standard giornaliera dalla realizzata (calcolo mio, su 252 sedute) e' circa 357 punti
per NQ e 54 per ES: quasi uguale alle mosse 0DTE, che pero' coprono solo le circa 11 ore che mancano
alle 22:00.

**NQ contro ES: la scala.** Si divide la GEX per il valore di un future: NQ 20 $ x 29.854 = 597.080 $;
ES 50 $ x 7.752 = 387.600 $. Il risultato e' il numero di future che il dealer comprerebbe o
venderebbe per un movimento dell'1% (NQ 298,5 punti, ES 77,5). Calcolo mio, dai totali di [gNQ] e [gES].

| | A tutte | B tutte | A entro 7 g | A 0DTE | B 0DTE |
|---|---|---|---|---|---|
| NQ, contratti per 1% | 2.880 | 6.723 | 2.435 | 979 | 1.641 |
| ES, contratti per 1% | 11.594 | 55.751 | 7.304 | 5.189 | 15.212 |

Il volume di cassa di NQ (15:30-22:00): media delle 20 sedute 408.436 contratti, mediana 403.082,
massimo 594.818 (01/10), il 14/10 540.320 [sed]. Il volume di 24 ore del 14/10 e' 729 mila [giro].
**NQ, A tutte: 2.880 su 408.436, cioe' lo 0,71% del volume medio di cassa per 1% di movimento**
(calcolo mio). E' lo 0,40% del volume di 24 ore del 14/10; con la convenzione B e' l'1,65%.
**ES: il volume del future non c'e' in `dati/`**, quindi il rapporto non si calcola. In dollari la GEX
A di ES e' 2,6 volte quella di NQ, la B 5,4 volte; l'OI dello 0DTE di ES e' 15 volte quello di NQ.

## Cosa Dicono Per NQ

1. **Il regime, se vale la convenzione A.** La GEX A e' negativa sui tre orizzonti. Per le scadenze
   della settimana non cambia segno su tutta la banda di +-4% (28.660-31.050). Per tutte le scadenze
   lo zero e' a 30.928, 1.074 punti sopra: circa tre volte la mossa attesa dello 0DTE. Per il modello
   la copertura andrebbe quindi nel verso del movimento per tutta la giornata
   ([`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md), "La GEX calcolata da noi").
   **Il segno resta un'ipotesi** [GPP]. VAPG trova la gamma dei market maker su SPX di solito
   positiva; GPP trova i dealer corti quasi ovunque, sommando call e put.
2. **Il "dove" regge alle due convenzioni, il "segno" no.** A e B mettono il peso negli stessi
   strike: 30.000, poi 29.500 e 29.700. Il motivo e' che sotto 30.100 l'open interest e' quasi tutto
   in put (30.000: 3.690 put contro 1.033 call; 29.500: 3.217 contro 254). Le due convenzioni
   divergono solo da 30.100 in su, dove call e put si equivalgono: li' A da' una gamma netta
   positiva piccola, B una negativa. **Lettura nostra**: la catena di NQ e' sbilanciata sulle put
   sotto il prezzo di ieri. Che quelle put siano coperture comprate dai clienti (dealer corti, come
   nella A) non lo dice il dato.
3. **La scala dice quanto conta.** Una GEX A di 2.880 contratti per 1% e' lo 0,7% del volume medio di
   cassa. BPVZ vede effetti sull'autocorrelazione dei rendimenti brevi e sulle code della giornata
   con un dealer al 18-29% del flusso, e non ha scenari sotto il 18%. L'autocorrelazione dice se una
   variazione e' seguita da una dello stesso segno (continuazione) o di segno opposto (inversione).
   **Nostra estrapolazione, non una previsione del paper**: su NQ l'effetto e' piccolo. DPS aggiunge
   che sulle scadenze brevissime chi da' liquidita' si copre soprattutto abbinando gli ordini.
   **Lettura nostra**: per la parte 0DTE la GEX e' un tetto del flusso di copertura, non il flusso
   (sintesi del 15/10, `quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`).
4. **ES e' vicino al cambio di segno, NQ no.** Lo zero gamma di ES e' 34-44 punti sopra il prezzo,
   dentro la mossa attesa dello 0DTE (+-54): il regime A di ES puo' cambiare segno in giornata,
   quello di NQ, con lo zero a 1.074 punti, difficilmente. Su ES la gamma in contratti e' 4 volte
   quella di NQ. **Osservazione**: NQ si muove con ES, e la copertura sulle opzioni di ES potrebbe
   pesare su NQ piu' della sua catena. Nessun paper letto lo misura.
5. **La volatilita'.** Lo 0DTE e il 16/10 prezzano piu' movimento di quello realizzato nelle ultime 20
   sedute (1,77 e 1,44 volte); dal 19/10 l'implicita e' pari alla realizzata. **Osservazione**: alle
   14:30 escono PPI e vendite al dettaglio [mac], e le 11 ore dello 0DTE le contengono. Una varianza
   concentrata in un tempo breve alza l'implicita annualizzata. L'implicita dello 0DTE di NQ (33,7%)
   e' sopra il VXN della sera prima (22,7).
6. **I paper, come ipotesi.**
   - BDLM: sui future S&P il momento dell'ultima mezz'ora c'e' solo nei giorni di gamma netta
     negativa (6,63, t 4,78, contro 0,82, t 1,03), 15:30-16:00 di New York, cioe' 21:30-22:00 ora
     italiana. Oggi e' un giorno "negativo" solo nella convenzione A. E' l'ipotesi B della scheda
     [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md), mai provata.
   - BB: con gamma alta il giorno dopo si muove meno; effetto piccolo sul livello della
     volatilita' (VAPG: mediano -0,08 punti contro una variazione giornaliera di 4,5).
   - GJ e BTW riguardano domani, terzo venerdi': il pinning (la tendenza a chiudere vicino a uno
     strike pesante) sulle scadenze seriali dei future S&P, e il movimento dalla chiusura del giovedi'
     all'apertura del venerdi'. Ipotesi della scheda, mai provate sui nostri dati; il pinning non e'
     provabile col nostro campione.
   - Nessuno dei nove paper usa NDX o NQ.

## I Livelli E Le Date

**La riserva vale per ogni riga**: nessun paper letto prova lo zero gamma, i muri o gli strike pesanti
come supporti o resistenze (finding 6 della scheda; l'unico test trovato, Popovici 2026, e' un
preprint non letto). Questi prezzi dicono **dove cambia la descrizione**, non cosa fa il prezzo.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.927,7 (NQ) | zero gamma A, tutte le scadenze [gNQ 10:45] | posizionamento: sopra, la GEX A totale passa positiva; il modello cambierebbe regime |
| 30.502,7 (NQ) | zero gamma A, 0DTE [gNQ] | posizionamento: sopra, la GEX A dello 0DTE diventa positiva |
| 30.000 (NQ) | call wall e put wall di tutte le scadenze e della settimana; massimo peso B; strike piu' pesante del 16/10 [gNQ] | posizionamento: lo strike dove la copertura pesa di piu' in tutte e due le convenzioni |
| 29.500 / 29.700 (NQ) | secondo e terzo peso B; put pesanti, volume put di oggi 824 / 790 [gNQ] | posizionamento: concentrazione delle put sotto il prezzo |
| 29.800-29.700 (NQ) | peso B e put wall dello 0DTE [gNQ] | posizionamento per la seduta di oggi, solo per lo 0DTE |
| 30.100 (NQ) | primo strike con netta A positiva (+30,4) [gNQ] | posizionamento: da qui in su le due convenzioni divergono |
| 7.795,6 / 7.785,8 (ES) | zero gamma A, tutte e 0DTE [gES] | posizionamento: il regime A di ES cambia segno; e' dentro la mossa attesa di oggi |
| 7.700 / 8.000 (ES) | put wall e call wall di tutte [gES] | posizionamento: massima gamma put e call |
| 15/10 14:30 | PPI, vendite al dettaglio, Philly Fed, sussidi [mac] | eventi che l'implicita dello 0DTE contiene |
| 15/10 21:30-22:00 | ultima mezz'ora della cassa [BDLM] | la finestra del finding di BDLM, come ipotesi |
| 15/10 22:00 | scadenza 0DTE di NQ e ES (16:00 di New York, convenzione di `gex.py`) | dopo, sparisce la gamma dello 0DTE (-584,8 su -1.719,4 di NQ) |
| 16/10 05:30 | discorso di Warsh, presidente della Fed [mac] | evento prima della scadenza mensile |
| 16/10 22:00 | scadenza del terzo venerdi', la piu' pesante della settimana di NQ (16.207 di OI) [gNQ] | la finestra dei finding di GJ e BTW, come ipotesi |
| 30/10 21:00 | scadenza di fine mese (16:00 di New York; l'Italia e' gia' all'ora solare) [gNQ] | prossima scadenza con OI rilevante su ES (100.360) |
| 18/12 15:30 | trimestrale (9:30 di New York, SOQ) [gNQ, gES] | la scadenza con piu' OI: NQ 52.002, ES 598.723 |

## Cosa Non Sappiamo

- **Il segno dei dealer** [GPP, VAPG]: la convenzione A e' un'ipotesi, la B un'altra. Tutte le misure
  del segno nei paper sono su SPX, nessuna sulle opzioni CME sui future.
- **L'open interest e' di ieri**: non vede le posizioni 0DTE aperte e chiuse oggi. Il volume di oggi
  (per esempio 828 call a 30.500, 1.108 put a 30.000) non dice il lato, ne' se apre o chiude.
- **Altre catene**: le opzioni su SPX, SPY, NDX e QQQ non sono nella catena. Non e' confermato che le
  opzioni lette su NQ siano quelle CME sul future (`le-opzioni-in-atas.md`).
- **Il modello di `gex.py`**: lo zero gamma tiene ferma la volatilita' di ogni strike (sticky strike)
  e cerca solo su +-4%. Lo zero di NQ di tutte le scadenze, al 3,6%, sta vicino al bordo della
  griglia; per la settimana non c'e' zero nella griglia. Il GEX Profile di ATAS usa il volume e non
  coincide con questo calcolo.
- **Il tempo**: la mossa attesa usa il calendario (365 giorni, notte compresa), la realizzata le
  sedute (252). Il confronto dipende da questa scelta.
- **La completezza della catena di NQ** (`quotate` su `opzioni`): 15/10 400/400, 16/10 400/400,
  19/10 396/400, 20/10 398/400, 18/12 339/340. Le lontane sono poco quotate: 21/10 169/384, 22/10
  107/384, 23/10 72/400, 30/10 69/400, 13/11 30/194. Tre sono `instabile` (22/10, 06/11, 27/11):
  l'OI cresceva ancora dopo sei riletture. Dove manca il prezzo, la volatilita' viene dallo strike vicino.
- **La completezza della catena di ES**: 0DTE 385/400, ma il 16/10 solo 101/400 (25%) con 57.857 di
  OI. 17/11 e 19/11 hanno una sola opzione quotata: la volatilita' del 19/11 (25,8%) e la sua mossa
  non sono attendibili. Il 04/12 e' saltato.
- **La GEX B di NQ cresce fra le letture** (da -3.716 a -4.014 in 40 minuti) piu' della A. Forse e'
  la catena che arriva un po' alla volta: non verificabile da qui.
- **Nessuno storico**: la GEX di NQ si archivia solo da oggi (quattro letture). Non si dice se il
  regime e' cambiato rispetto a ieri.
- lacuna del collettore: il volume giornaliero del future ES (manca un `sedute-ESZ6.json`), quindi la
  scala di ES contro il suo volume non si misura.
- lacuna del collettore: la GEX A e B per singola scadenza. Il JSON da' solo tutte, entro 7 giorni e
  0DTE, quindi la scadenza piu' pesante si giudica dall'open interest, non dalla gamma.
- lacuna del collettore: la catena e' tagliata a 100 strike per lato (`--strikes 100`, 400 opzioni).
  Dove `opzioni` = 400 (NQ 15/10, 16/10, 19/10, 20/10, 23/10, 30/10; su ES le prime sette e altre)
  l'OI oltre quegli strike non entra.
- lacuna del collettore: l'ora del prezzo del sottostante usato nel calcolo. Per NQ e' 29.854, mentre
  l'ultimo prezzo a 1 minuto delle 10:45 e' 29.822,50 [giro]: 31,5 punti, poco rispetto alla distanza
  dallo zero.
- lacuna del collettore: la volatilita' realizzata sullo stesso orizzonte delle scadenze (intraday,
  dalle barre di ATAS); la macro da' solo la 20 giorni da chiusure Yahoo.
- lacuna del collettore: la finestra del "volume del giorno" delle opzioni (da che ora conta).
- lacuna del collettore: `per_strike` copre solo +-4% dal future. Su ES restano fuori le call 0DTE a
  8.125 e 8.150, con 10.751 e 9.510 di OI (gamma piccola a quella distanza, ma non misurata).

## Per Il Quadro

1. **NQ 30.000**: massimo peso in A e in B, i due muri coincidono, ed e' lo strike piu' pesante della
   scadenza del 16/10. Sta 82 punti sotto il VAL di cassa di ieri (30.082-30.478, POC 30.173)
   [giro]. Da confrontare con i livelli del COT e con il valore dell'order flow (fase 4).
2. **NQ 29.800-29.700** (peso B dello 0DTE) contro il valore di Londra 29.781-29.838 (POC 29.818) e
   il POC del composito aggiustato 29.817,50 [giro]. Coincide con il valore dell'order flow?
3. **Gamma A negativa con il regime CHOPPY della notte** (efficienza 0,09 [giro]). BPVZ, con gamma
   negativa, si aspetta continuazione; la scala su NQ (0,7% del volume) dice che l'effetto puo' non
   vedersi. Il confronto lo fa la fase 5.
4. **Catena dominata dalle put sotto il prezzo**, implicita dello 0DTE a 33,7% contro il VXN a 22,7,
   e paura (CNN Fear & Greed 29,8 [mac]). E' coerente con il posizionamento del COT e con il regime
   dei tassi della macro?
