# Fase 3 · La Gamma · 2026-10-15

**Stato**: parziale (manca il volume giornaliero di ES, quindi la scala di ES non si misura; le catene
lontane di NQ e quattro scadenze pesanti di ES sono quotate in parte; vedi "Cosa Non Sappiamo")
**Dati**: `dati/gex-NQZ6.json` e `dati/gex-ESZ6.json` (calcolo di `gex.py`; future e catena lette alle
11:04 ora italiana, 09:04 UTC, `future_quando`; chiuse alle 11:07 e 11:10 secondo `raccolta.json`;
open interest di ieri, 14/10), `dati/sedute-NQZ6.json` (20 sedute di cassa, 17/09-14/10),
`dati/macro.json` (quotazioni delle 10:35; VIX delle 10:30; VXN alla chiusura del 14/10, 22:00),
`dati/giro.txt` (prezzo delle 11:04), lo storico `quant/dati/NQ/gex/2026-10-15/` (cinque letture,
10:05-11:04). Raccolta chiusa alle 11:10, tutte le voci `ok`; ES 04/12 saltata (0 quotate su 100).
**Attenzione**: la tab 2 di `giro.txt` stampa ancora la lettura delle 10:45 ("19 min fa"); qui valgono
i JSON delle 11:04.

**In una riga**: con la convenzione dei fornitori la GEX e' negativa su NQ e su ES; NQ sta 3,5% sotto
lo zero gamma, ES 0,55%; su NQ la copertura per 1% di movimento vale lo 0,70% del volume medio di cassa.

Le fonti: **[gNQ]** `gex-NQZ6.json`, **[gES]** `gex-ESZ6.json`, **[sed]** `sedute-NQZ6.json`,
**[mac]** `macro.json`, **[giro]** `giro.txt`, **[sto]** lo storico. GEX in **milioni di $ per 1%**.

Le parole. La **gamma** dice di quanto cambia il delta di un'opzione quando il future si muove. Il
**dealer** (market maker) vende le opzioni e si copre col future. La **GEX** (gamma exposure) e' la
loro gamma sommata: i dollari di future che comprano o vendono per un movimento dell'1%. Positiva: il
dealer compra i ribassi e vende i rialzi (**smorza**); negativa: il contrario (**amplifica**). Lo
**zero gamma** e' il prezzo dove la GEX totale cambia segno; i **muri** gli strike con la gamma call
(call wall) o put (put wall) piu' grande; lo **0DTE** la scadenza di oggi; l'**open interest** (OI) i
contratti aperti alla fine di ieri.

## I Fatti

**Il regime, convenzione A** (fornitori, SqueezeMetrics, ATAS: dealer lunghi le call e corti le put,
GEX = call - put). Future del calcolo: NQ 29.863,5, ES 7.752,5 [gNQ, gES, 11:04].

| | GEX A | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| NQ tutte (18 scadenze) | -1.696,4 | 30.900,6 | sotto di 1.037,1 (3,47%) | 30.000 (107,1) | 30.000 (383,9) |
| NQ entro 7 giorni (fino al 21/10) | -1.432,6 | nessuno entro +-4% | negativa su tutta la griglia | 30.000 (95,2) | 30.000 (347,5) |
| NQ 0DTE 15/10 | -594,6 | 30.470,1 | sotto di 606,6 (2,03%) | 30.300 (12,6) | 29.700 (63,6) |
| ES tutte (31 scadenze) | -4.523,8 | 7.795,4 | sotto di 42,9 (0,55%) | 8.000 (770,6) | 7.700 (1.369,5) |
| ES entro 7 giorni | -2.869,4 | 7.788,2 | sotto di 35,7 (0,46%) | 7.775 (464,4) | 7.650 (501,4) |
| ES 0DTE 15/10 | -2.058,4 | 7.785,6 | sotto di 33,1 (0,43%) | 7.800 (181,8) | 7.700 (448,5) |

Fra parentesi la gamma del muro. Distanze e percentuali: calcolo mio dai campi del JSON.

**Per scadenza** (`gex_per_scadenza`, nuovo in questa raccolta) [gNQ, gES]:
- NQ: 0DTE A -594,6 (B -992,8); **16/10 A -566,6 (B -1.656,7)**, nessuno zero entro +-4%, muri a
  30.000; 19/10 -124,1; 20/10 -105,6; 18/12 -75,1 (B -528,3). Lo 0DTE e il 16/10 fanno il 68% della
  GEX A totale. L'unica scadenza con A positiva non trascurabile e' il 20/11 (+12,0).
- ES: 0DTE -2.058,4 (46% della A); 16/10 -658,0 (zero 7.806,4); 30/10 -616,6; 18/12 -543,6 (B
  -6.519,8, la piu' grande in B). Positive: 19/10 +42,0, 20/11 +125,8, 30/11 +27,9.

**La convenzione B** [GPP]: dealer corti di tutto. La GEX e' negativa ovunque e dice solo **dove** la
copertura pesa. Il segno vero non si conosce, in nessuna delle due.

| | GEX B | dove pesa di piu' (in ordine) |
|---|---|---|
| NQ tutte / entro 7 giorni | -4.043,9 / -3.040,5 | 30.000, 29.500, 29.700 (tutte e due) |
| NQ 0DTE | -992,8 | 29.800, 29.700, 29.750 |
| ES tutte | -21.492,5 | 7.700, 7.775, 8.000 |
| ES entro 7 giorni | -9.256,4 | 7.775, 7.650, 7.700 |
| ES 0DTE | -5.812,7 | 7.700, 7.800, 7.750 |

**Gli strike pesanti di NQ**, da `per_strike` (tutte le scadenze, ora entro +-6%: 28.100-31.650)
[gNQ]. Il volume e' quello di oggi fino alla lettura, e non dice il lato.

| strike | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|
| 30.000 | 1.033 / 3.690 | 565 / 1.108 | 107,1 | -383,9 | -276,8 |
| 29.500 | 254 / 3.217 | 13 / 879 | 13,8 | -205,7 | -191,9 |
| 29.700 | 129 / 1.096 | 16 / 797 | 14,3 | -144,7 | -130,4 |
| 29.750 | 128 / 817 | 82 / 645 | 12,9 | -113,3 | -100,4 |
| 29.800 | 427 / 722 | 96 / 352 | 48,8 | -97,1 | -48,3 |
| 30.100 | 543 / 359 | 452 / 43 | 75,0 | -41,4 | +33,6 |
| 30.500 | 1.275 / 1.135 | 836 / 57 | 68,5 | -55,4 | +13,1 |
| 31.500 | 1.852 / 990 | 69 / 25 | 48,6 | -26,9 | +21,7 |

Sotto 30.100 la netta A e' negativa su quasi ogni strike; da 30.100 in su alterna valori piccoli
positivi e negativi (+33,6 a 30.100, -54,6 a 30.150). Volume di oggi: call sopra il prezzo (30.500
836, 30.300 684, 30.200 640), put sotto (30.000 1.108, 29.500 879, 29.700 797).

**Gli strike pesanti di ES** [gES]. 7.700: OI 7.499 / 31.102, netta A -1.104,5, volume put 7.681.
7.500 -575,5; 7.600 -498,2; 7.650 -496,4. Positivi: 8.000 (OI call 29.968) +602,6; 7.900 +301,7;
8.050 +293,1; 8.100 +218,0; 8.150 +158,8. Vicino al prezzo 7.775 (+66,5) e 7.800 (-60,8, volume call
1.795) hanno molta gamma sia call sia put.

**Le scadenze della settimana.** NQ: OI 16/10 15.703, 0DTE 8.607, 19/10 2.223. La piu' pesante e' il
**16/10**, terzo venerdi' (la mensile; il bridge la chiama "Weekly"): prima in B, seconda in A per 28
milioni. Strike: 30.000 (605 call / 1.280 put), 30.500 (714 / 467), 29.500 (92 / 1.052), 30.250,
29.700. Trimestrale 18/12: OI 52.002, strike pesanti lontani (28.000, 23.000, 33.500). ES: la piu'
pesante e' lo 0DTE (OI 132.918), poi 16/10 (50.672); trimestrale 598.723 [gNQ, gES].

**La mossa attesa e la volatilita' realizzata.** L'implicita ATM (al denaro) e' la volatilita' che i
prezzi delle opzioni contengono (Black-76, tempo di calendario su 365 giorni); la mossa attesa e' una
deviazione standard fino alla scadenza [gNQ, gES]. La realizzata: 20 giorni, annualizzata, da chiusure
Yahoo [mac, 10:35].

| | 15/10 (0DTE) | 16/10 | 19/10 | 20/10 | 18/12 | realizzata 20 g | indice |
|---|---|---|---|---|---|---|---|
| NQ vol ATM | 32,4% | 23,7% | 19,0% | 19,7% | 20,3% | 19,0% | VXN 22,7 (14/10) |
| NQ mossa | +-341,8 | +-446,5 | +-628,2 | +-720,7 | +-2.536,3 | | |
| ES vol ATM | 18,8% | 18,8% | 12,8% | 10,6% | 13,9% | 11,1% | VIX 17,52 (10:30); VIX9D 14,59 (14/10) |
| ES mossa | +-51,4 | +-91,8 | +-109,5 | +-100,2 | +-450,7 | | |

Implicita / realizzata (calcolo mio): NQ 1,71 (0DTE), 1,25 (16/10), 1,00 (19/10); ES 1,69, 1,69,
1,15. La deviazione standard giornaliera dalla realizzata (252 sedute) e' circa 357 punti per NQ e 54
per ES: quasi uguale alle mosse 0DTE, che coprono pero' solo le circa 11 ore fino alle 22:00.

**NQ contro ES: la scala.** GEX diviso il valore di un future (NQ 20 $ x 29.863,5 = 597.270 $; ES
50 $ x 7.752,5 = 387.625 $): i future che il dealer comprerebbe o venderebbe per l'1% (NQ 298,6 punti,
ES 77,5). Calcolo mio dai totali [gNQ, gES].

| | A tutte | B tutte | A entro 7 g | A 0DTE | B 0DTE |
|---|---|---|---|---|---|
| NQ, contratti per 1% | 2.840 | 6.771 | 2.399 | 996 | 1.662 |
| ES, contratti per 1% | 11.670 | 55.447 | 7.403 | 5.310 | 14.996 |

Volume di cassa di NQ (15:30-22:00): media delle 20 sedute 408.436, mediana 403.082, massimo 594.818
(01/10), il 14/10 540.320 [sed]; 24 ore del 14/10: 729 mila [giro, tab 1]. **NQ, A tutte: 2.840 su
408.436, lo 0,70% del volume medio di cassa per 1% di movimento** (B 1,66%; A 0DTE 0,24%; A contro le
24 ore del 14/10 0,39%). **ES: il volume del future non c'e' in `dati/`**, il rapporto non si calcola.
In dollari la GEX A di ES e' 2,7 volte quella di NQ, la B 5,3; in contratti A 4,1 volte; l'OI dello
0DTE di ES e' 15 volte quello di NQ.

**Cosa e' cambiato dalle 10:45** (`precedenti/1110/3-gamma.md`) [sto, gNQ, gES]:

| | 10:45 | 11:04 |
|---|---|---|
| NQ future / GEX A / GEX B | 29.854 / -1.719,4 / -4.013,9 | 29.863,5 / -1.696,4 / -4.043,9 |
| NQ zero tutte / 0DTE | 30.927,7 / 30.502,7 | 30.900,6 / 30.470,1 |
| NQ vol ATM 0DTE / 16/10 | 33,7% / 27,4% | 32,4% / 23,7% |
| OI del 16/10, NQ / ES | 16.207 / 57.857 | 15.703 / 50.672 |
| ES GEX A / zero tutte / vol ATM 16/10 | -4.494,0 / 7.795,6 / 17,2% | -4.523,8 / 7.795,4 / 18,8% |

Su NQ, cinque letture dalle 10:05 alle 11:04: GEX A fra -1.668 e -1.719, zero fra 30.901 e 30.928,
GEX B da -3.716 a -4.044, in crescita a ogni lettura [sto]. Il regime non cambia; cambiano l'implicita
del 16/10 di NQ (-3,7 punti in 19 minuti) e l'OI del 16/10 (l'OI di ieri non dovrebbe muoversi).

## Cosa Dicono Per NQ

1. **Il regime, se vale la convenzione A.** GEX A negativa sui tre orizzonti; per la settimana nessun
   cambio di segno su +-4% (28.669-31.058); per tutte lo zero e' 1.037 punti sopra, circa tre volte la
   mossa attesa dello 0DTE. Per il modello la copertura andrebbe nel verso del movimento per tutta la
   giornata ([`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md), "La GEX calcolata
   da noi"). **Il segno resta un'ipotesi** [GPP]: VAPG trova la gamma dei market maker su SPX di solito
   positiva, GPP i dealer corti quasi ovunque sommando call e put.
2. **Il "dove" regge alle due convenzioni, il "segno" no.** A e B mettono il peso negli stessi strike
   (30.000, 29.500, 29.700) perche' sotto 30.100 l'OI e' quasi tutto put (30.000: 3.690 contro 1.033;
   29.500: 3.217 contro 254); sopra, call e put si equivalgono e le due divergono. **Lettura nostra**:
   catena sbilanciata sulle put sotto il prezzo di ieri. Che siano coperture comprate dai clienti
   (dealer corti, come nella A) il dato non lo dice.
3. **Due scadenze fanno la gamma della settimana.** 0DTE e 16/10 sono il 68% della GEX A; lo 0DTE
   scade alle 22:00 di oggi, il 16/10 alle 22:00 di domani. **Osservazione**: dopo il 16/10 la GEX A di
   NQ resta circa un terzo di quella di oggi, se l'OI non cambia.
4. **La scala dice quanto conta.** 2.840 contratti per 1% sono lo 0,70% del volume medio di cassa. BPVZ
   vede effetti sull'autocorrelazione dei rendimenti brevi (se una variazione e' seguita da una dello
   stesso segno, continuazione, o opposto, inversione) e sulle code della giornata gia' con il dealer
   al 18-19% del flusso, e non ha scenari sotto il 18%. **Nostra estrapolazione, non una previsione del
   paper**: su NQ l'effetto e' piccolo. DPS: sulle opzioni brevissime chi da' liquidita' si copre
   soprattutto abbinando gli ordini; **lettura nostra**, per lo 0DTE la GEX e' un tetto del flusso di
   copertura, non il flusso (`quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`).
5. **ES e' vicino al cambio di segno, NQ no.** Lo zero di ES e' 33-43 punti sopra il prezzo, dentro la
   mossa attesa dello 0DTE (+-51,4): il regime A di ES puo' cambiare segno in giornata, quello di NQ
   difficilmente. **Osservazione**: NQ si muove con ES, e la copertura sulle opzioni di ES (4 volte i
   contratti di NQ) potrebbe pesare su NQ piu' della sua catena. Nessun paper letto lo misura.
6. **La volatilita'.** Lo 0DTE prezza 1,71 volte la realizzata; dal 19/10 l'implicita e' pari alla
   realizzata. **Osservazione**: PPI e vendite al dettaglio alle 14:30 [mac] stanno dentro le 11 ore
   dello 0DTE, e una varianza concentrata in poco tempo alza l'implicita annualizzata. L'implicita
   0DTE di NQ (32,4%) e' sopra il VXN della sera prima (22,7).
7. **I paper, come ipotesi.** BDLM: sui future S&P il momento dell'ultima mezz'ora (21:30-22:00 ora
   italiana) c'e' solo nei giorni di gamma netta negativa (6,63, t 4,78, contro 0,82, t 1,03); oggi e'
   "negativo" solo nella convenzione A; e' l'ipotesi B della scheda
   [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md), mai provata. BB: con gamma
   alta il giorno dopo si muove meno; VAPG: effetto mediano sulla volatilita' piccolo (-0,08 punti
   contro una variazione giornaliera di 4,5). GJ e BTW riguardano domani, terzo venerdi' (pinning,
   cioe' chiudere vicino a uno strike pesante; il movimento dalla chiusura del giovedi' all'apertura
   del venerdi'): ipotesi della scheda, mai provate. Nessuno dei nove paper usa NDX o NQ.

## I Livelli E Le Date

**La riserva vale per ogni riga**: nessun paper letto prova lo zero gamma, i muri o gli strike pesanti
come supporti o resistenze (finding 6 della scheda; l'unico test trovato, Popovici 2026, e' un
preprint non letto). Questi prezzi dicono **dove cambia la descrizione**, non cosa fa il prezzo.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.900,6 (NQ) | zero gamma A, tutte [gNQ 11:04] | posizionamento: sopra, la GEX A totale diventa positiva; il modello cambierebbe regime |
| 30.470,1 (NQ) | zero gamma A, 0DTE [gNQ] | posizionamento: sopra, la GEX A dello 0DTE diventa positiva |
| 30.000 (NQ) | call e put wall di tutte, della settimana e del 16/10; primo peso B [gNQ] | posizionamento: dove la copertura pesa di piu' in tutte e due le convenzioni |
| 30.100 (NQ) | primo strike sopra il prezzo con netta A positiva (+33,6) [gNQ] | posizionamento: da qui in su A e B divergono |
| 29.800-29.700 (NQ) | peso B e put wall dello 0DTE [gNQ] | posizionamento per la seduta di oggi, solo per lo 0DTE |
| 29.500 (NQ) | secondo peso B; put 3.217 di OI, 879 di volume oggi [gNQ] | posizionamento: la concentrazione di put piu' grande sotto il prezzo |
| 7.795,4 / 7.785,6 (ES) | zero gamma A, tutte e 0DTE [gES] | posizionamento: il regime A di ES cambia segno; e' dentro la mossa attesa di oggi |
| 7.700 / 8.000 (ES) | put wall e call wall di tutte [gES] | posizionamento: massima gamma put e call |
| 15/10 14:30 | PPI, vendite al dettaglio, Philly Fed, sussidi [mac] | eventi dentro l'implicita dello 0DTE |
| 15/10 21:30-22:00 | ultima mezz'ora della cassa [BDLM] | la finestra del finding di BDLM, come ipotesi |
| 15/10 22:00 | scadenza 0DTE di NQ e ES (16:00 di New York, convenzione di `gex.py`) | sparisce la gamma dello 0DTE (-594,6 su -1.696,4 di NQ) |
| 16/10 05:30 | discorso di Warsh, presidente della Fed [mac] | evento prima della scadenza mensile |
| 16/10 22:00 | terzo venerdi', la scadenza piu' pesante della settimana di NQ [gNQ] | sparisce il 16/10 (-566,6); la finestra di GJ e BTW, come ipotesi |
| 18/12 15:30 | trimestrale (9:30 di New York, SOQ) | la scadenza con piu' OI: NQ 52.002, ES 598.723 |

## Cosa Non Sappiamo

- **Il segno dei dealer** [GPP, VAPG]: A e B sono due ipotesi; tutte le misure del segno nei paper
  sono su SPX, nessuna sulle opzioni CME sui future.
- **L'OI e' di ieri** e non vede gli 0DTE aperti e chiusi oggi; il volume di oggi non dice il lato ne'
  se apre o chiude. **E cambia fra le letture** (16/10: NQ 16.207 -> 15.703, ES 57.857 -> 50.672 in
  19 minuti), e la GEX B di NQ cresce a ogni lettura: forse la catena arriva un po' alla volta, non
  verificabile da qui. Nessuna scadenza e' `instabile` in questa raccolta.
- **La completezza di NQ** (`quotate` su `opzioni`): 15/10, 16/10, 19/10 400/400; 20/10 394/400; 18/12
  339/340; 06/11 195/196; 27/11 157/192. Poco quotate: 21/10 187/388, 22/10 120/388, 23/10 70/400,
  26/10 91/270, 27/10 50/270, 28/10 72/262, 29/10 41/258, 30/10 69/398, 13/11 30/194, 20/11 70/352,
  30/11 72/352. Pesano il 10% della GEX A e della B (calcolo mio): il regime di NQ sta sulle scadenze
  ben quotate.
- **La completezza di ES**: 0DTE 386/400 (46% della A). Ma il 16/10 e' quotato 99/400 con OI 50.672,
  il 30/10 132/396, il 20/11 99/316, il 18/12 280/324 (30% della B). 17/11 e 19/11 hanno una sola
  opzione quotata: la volatilita' del 19/11 (25,8%) non e' attendibile. Dove manca il prezzo, la
  volatilita' viene dallo strike vicino. 04/12 saltato.
- **Altre catene**: SPX, SPY, NDX e QQQ non ci sono; non e' confermato che le opzioni lette su NQ siano
  quelle CME sul future (`le-opzioni-in-atas.md`).
- **Il modello di `gex.py`**: lo zero tiene ferma la volatilita' di ogni strike (sticky strike) e
  cerca solo su +-4%; lo zero di NQ al 3,47% e' vicino al bordo. Il GEX Profile di ATAS usa il volume e
  non coincide. La mossa attesa usa il calendario (365 giorni), la realizzata le sedute (252).
- **Nessuno storico** prima di oggi: non si dice se il regime e' cambiato rispetto a ieri.
- lacuna del collettore: il volume giornaliero del future ES (manca un `sedute-ESZ6.json`).
- lacuna del collettore: catena ancora tagliata a 100 strike per lato (`--strikes 100`); dove
  `opzioni` = 400 (NQ 15/10, 16/10, 19/10, 20/10, 23/10; ES le prime sette) l'OI oltre non entra.
- lacuna del collettore: un segnale di OI definitivo (data e ora dell'OI, o una rilettura di conferma).
- lacuna del collettore: la griglia dello zero a +-4%; per NQ la settimana non ha zero dentro.
- lacuna del collettore: la volatilita' realizzata intraday sugli orizzonti delle scadenze (barre di
  ATAS); la macro da' solo la 20 giorni da chiusure Yahoo.
- lacuna del collettore: la finestra del "volume del giorno" delle opzioni (da che ora conta).
- lacuna del collettore: la tab 2 del giro legge la cache delle 10:45 mentre i JSON sono delle 11:04.

Risolte rispetto alle 10:45: `gex_per_scadenza`, `future_quando`, `per_strike` a +-6% (entrano 8.125
e 8.150 di ES e 31.500 di NQ).

## Per Il Quadro

1. **NQ 30.000**: massimo peso in A e in B, i due muri coincidono, strike piu' pesante del 16/10. Sta
   82 punti sotto il VAL di cassa di ieri (30.082-30.478, POC 30.173) e 132,25 sopra il prezzo delle
   11:04 (29.867,75) [sed, giro]. Da confrontare con i livelli del COT e con il valore dell'order flow.
2. **NQ 29.800-29.700** (peso B e put wall dello 0DTE) contro il valore di Londra 29.776-29.840 (POC
   29.818), il POC del composito aggiustato 29.817,50 e il minimo di oggi 29.754 [giro]. Coincide con
   il valore dell'order flow?
3. **Gamma A negativa con il regime CHOPPY** delle 11:04 (efficienza 0,15 [giro]). BPVZ, con gamma
   negativa, si aspetta continuazione; la scala su NQ (0,70% del volume) dice che l'effetto puo' non
   vedersi. Il confronto lo fa la fase 5.
4. **Catena dominata dalle put sotto il prezzo**, implicita 0DTE 32,4% contro VXN 22,7, e due scadenze
   (oggi e domani) che portano via due terzi della gamma A. Coerente con il posizionamento del COT e
   con il regime dei tassi della macro?
