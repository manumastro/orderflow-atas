# Fase 3 · La Gamma · 2026-10-15 (sera)

**Stato**: parziale (solo NQ, per correzione dell'utente; le settimanali lontane di NQ sono quotate
poco questa sera; l'OI cambia fra le letture; vedi "Cosa Non Sappiamo")
**Dati**: `dati/gex-NQZ6.json` (calcolo di `gex.py`; future e catena alle **18:43 ora italiana**,
16:43 UTC, `future_quando`; raccolta chiusa alle 18:46 secondo `raccolta.json`; OI di ieri, 14/10),
`dati/sedute-NQZ6.json` (20 sedute di cassa, 17/09-14/10, e `oggi_finora`), `dati/macro.json`
(NQ=F alle 18:33; VXN alla chiusura del 14/10, 22:00: vecchio di quasi 21 ore), `dati/giro.txt` e il
pannello (prezzo e cassa alle 18:43), lo storico `quant/dati/NQ/gex/2026-10-15/` (sette letture,
10:05-18:43). Tutte le voci `ok`, nessuna scadenza saltata, nessuna `instabile`.
**Fuori dal rapporto**: `gex-ESZ6.json` non e' letto. Su indicazione dell'utente (tramite
l'orchestratore) la gamma si studia solo su NQ: niente regime, livelli o confronto di ES.
**Attenzione**: la tab 2 di `giro.txt` stampa ancora la lettura delle 11:51 ("412 min fa"); qui
valgono i JSON delle 18:43.

**In una riga**: con la convenzione dei fornitori la GEX di NQ resta negativa (-1.747,6). Il prezzo e'
salito a 24,5 punti dallo strike 30.000, che e' il piu' pesante in tutte e due le convenzioni. Lo zero
gamma si e' avvicinato (2,59%), e ora anche la settimana ne ha uno. La copertura vale lo 0,71% del
volume medio di cassa per 1% di movimento.

Le fonti: **[gNQ]** `gex-NQZ6.json`, **[sed]** `sedute-NQZ6.json`, **[mac]** `macro.json`,
**[giro]** `giro.txt` (pannello compreso), **[sto]** lo storico. GEX in **milioni di $ per 1%**.

Le parole. La **gamma** dice di quanto cambia il delta di un'opzione quando il future si muove. Il
**dealer** (market maker) vende le opzioni e si copre col future. La **GEX** (gamma exposure) e' la
loro gamma sommata: i dollari di future che comprano o vendono per un movimento dell'1%. Se e'
positiva, il dealer compra i ribassi e vende i rialzi (**smorza**); se e' negativa, fa il contrario
(**amplifica**). Lo **zero gamma** e' il prezzo dove la GEX totale cambia segno. I **muri** sono gli
strike con la gamma call piu' grande (call wall) o con la gamma put piu' grande (put wall). Lo **0DTE**
e' la scadenza di oggi, alle 22:00. L'**open interest** (OI) conta i contratti aperti alla fine di ieri.

## I Fatti

**Il regime, convenzione A** (fornitori, SqueezeMetrics, ATAS: dealer lunghi le call e corti le put,
GEX = call - put). Future del calcolo: **29.975,5** [gNQ, 18:43].

| NQ | GEX A | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| tutte (18 scadenze) | -1.747,6 | 30.752,1 | sotto di 776,6 (2,59%) | 30.000 (160,2) | 30.000 (572,2) |
| entro 7 giorni (fino al 21/10) | -1.558,8 | 30.656,9 | sotto di 681,4 (2,27%) | 30.000 (119,6) | 30.000 (484,7) |
| 0DTE 15/10 | -737,3 | 30.314,3 | sotto di 338,8 (1,13%) | 30.500 (15,5) | 30.000 (134,8) |

Fra parentesi la gamma del muro. Le distanze e le percentuali sono un mio calcolo dai campi del JSON.

**Per scadenza** (`gex_per_scadenza`) [gNQ]. 0DTE: A -737,3, B -1.300,5. **16/10: A -581,8, B
-1.964,6**, zero a 31.070,4, muri a 30.000. 19/10: A -126,5. 20/10: A -113,5. 30/10: A -63,2.
18/12: A -59,8, B -534,4, zero a 30.410,1. Lo 0DTE e il 16/10 fanno il **75%** della GEX A totale
(alle 11:04 erano il 68%) e il 74% della B. Nessuna scadenza ha una A positiva sopra 1 milione.

**La convenzione B** [GPP]: dealer corti di tutto. La GEX e' negativa ovunque e dice solo **dove** la
copertura pesa. Il segno vero non si conosce, in nessuna delle due convenzioni.

| NQ | GEX B | dove pesa di piu' (in ordine) |
|---|---|---|
| tutte | -4.428,4 | 30.000, 29.500, 29.800 |
| entro 7 giorni | -3.658,0 | 30.000, 29.800, 30.100 |
| 0DTE | -1.300,5 | 30.000, 29.800, 29.950 |

**Gli strike pesanti**, da `per_strike` (tutte le scadenze, entro +-6%: 28.200-31.750) [gNQ]. Il
volume e' quello di oggi fino alle 18:43, e non dice il lato.

| strike | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|
| 30.000 | 1.543 / 4.556 | 2.007 / 2.343 | 160,2 | -572,2 | -412,0 |
| 29.500 | 241 / 3.067 | 46 / 2.171 | 12,4 | -184,6 | -172,2 |
| 29.700 | 117 / 1.050 | 240 / 2.170 | 14,8 | -126,0 | -111,2 |
| 29.750 | 128 / 784 | 253 / 2.388 | 14,6 | -110,5 | -95,9 |
| 29.950 | 7 / 292 | 1.045 / 359 | 2,4 | -94,0 | -91,6 |
| 30.150 | 44 / 440 | 858 / 169 | 8,6 | -89,6 | -81,0 |
| 29.800 | 346 / 519 | 740 / 2.065 | 54,5 | -107,0 | -52,5 |
| 30.100 | 540 / 276 | 1.826 / 124 | 95,4 | -56,9 | +38,5 |
| 30.500 | 1.252 / 1.002 | 1.452 / 107 | 84,4 | -59,6 | +24,8 |
| 31.500 | 867 / 121 | 626 / 27 | 22,8 | -3,2 | +19,6 |

Sotto 30.100 la netta A e' negativa su ogni strike con OI. Da 30.100 in su alterna valori positivi
piccoli (30.100 +38,5; 30.250 +18,9; 30.200 +13,1) e negativi (30.150 -81,0; 30.300 -23,7). Il
volume di oggi va alle put sotto il prezzo (29.750 2.388; 30.000 2.343; 29.500 2.171; 29.700 2.170;
29.800 2.065) e alle call sopra (30.200 2.309; 30.300 2.134; 30.100 1.826; 30.250 1.641).

**Le scadenze della settimana** [gNQ]. OI: 16/10 16.681, 0DTE 8.894, 19/10 2.511, 20/10 1.518. La
piu' pesante e' il **16/10**, terzo venerdi' di ottobre (la mensile; il bridge la chiama "Weekly"):
prima in B, seconda in A dietro lo 0DTE. I suoi strike pesanti non cambiano dalle 11:04: 30.000 (605
call / 1.280 put), 30.500 (714 / 467), 29.500 (92 / 1.052), 30.250 (478 / 268), 29.700 (107 / 572).
Nello 0DTE l'OI piu' grande e' sulle put lontane: 29.250 (508, volume 806), 29.700 (357), 29.750
(323), 29.800 (318). La trimestrale 18/12 ha OI 51.827, con strike lontani: 28.000, 23.000, 33.500.
Sul 33.500 ci sono 1.864 call scambiate oggi.

**La mossa attesa e la volatilita' realizzata.** L'implicita ATM (al denaro) e' la volatilita' che i
prezzi delle opzioni contengono (Black-76, tempo di calendario su 365 giorni). La mossa attesa e' una
deviazione standard fino alla scadenza [gNQ]. La realizzata e' a 20 giorni, annualizzata, da chiusure
Yahoo [mac, NQ=F alle 18:33].

| NQ | 15/10 (0DTE) | 16/10 | 19/10 | 20/10 | 18/12 | realizzata 20 g | indice |
|---|---|---|---|---|---|---|---|
| vol ATM | 24,5% | 24,2% | 17,5% | 18,3% | 20,1% | 18,7% | VXN 22,7 (14/10) |
| mossa | +-141,9 | +-404,8 | +-557,8 | +-649,2 | +-2.519,4 | | |

Il rapporto fra implicita e realizzata (calcolo mio) e' 1,31 per lo 0DTE, 1,30 per il 16/10, 0,94
per il 19/10, 0,98 per il 20/10 e 1,07 per il 18/12. La mossa dello 0DTE copre circa 3 ore e 17
minuti, fino alle 22:00. Per confronto, i range delle ultime quattro mezz'ore sono stati 136, 73, 122
e 82 punti [giro, tab 2].

**La scala.** La GEX si divide per il valore di un future: 20 $ x 29.975,5 = 599.510 $. Il risultato
sono i future che il dealer comprerebbe o venderebbe per l'1%, cioe' per 299,8 punti (calcolo mio dai
totali [gNQ]).

| NQ, contratti per 1% | A tutte | B tutte | A entro 7 g | A 0DTE | B 0DTE |
|---|---|---|---|---|---|
| 18:43 | 2.915 | 7.387 | 2.600 | 1.230 | 2.169 |
| 11:04 (rapporto precedente) | 2.840 | 6.771 | 2.399 | 996 | 1.662 |

Il volume di cassa di NQ (15:30-22:00) ha una media di 408.436 contratti sulle 20 sedute, una mediana
di 403.082 e un massimo di 594.818 (01/10); il 14/10 ha fatto 540.320 [sed]. **A tutte vale 2.915
contratti su 408.436, cioe' lo 0,71% del volume medio di cassa per 1% di movimento.** Con la B e' l'1,81%;
con la A dello 0DTE lo 0,30%. Oggi la cassa ha gia' scambiato 569.186 lotti dalle 15:30 alle 18:43
[giro, pannello], e la giornata intera da ieri alle 22:00 ne ha scambiati 877 mila [giro, tab 1].
Rispetto a quei volumi, A tutte vale lo 0,51% e lo 0,33%.

**Cosa e' cambiato dalla mattina** (`precedenti/1850/3-gamma.md`, delle 11:04; la lettura delle
11:51 e' in [sto] e [giro, tab 2]):

| NQ | 11:04 | 11:51 | 18:43 |
|---|---|---|---|
| future | 29.863,5 | 29.852,5 | 29.975,5 |
| GEX A / GEX B, tutte | -1.696,4 / -4.043,9 | -1.772,7 / -4.077,1 | -1.747,6 / -4.428,4 |
| zero tutte / entro 7 g / 0DTE | 30.900,6 / nessuno / 30.470,1 | 30.887,1 / nessuno / 30.461,3 | 30.752,1 / 30.656,9 / 30.314,3 |
| GEX A 0DTE / 16/10 | -594,6 / -566,6 | -602,1 / -589,2 | -737,3 / -581,8 |
| vol ATM 0DTE / 16/10 | 32,4% / 23,7% | 33,2% / 26,7% | 24,5% / 24,2% |
| OI 0DTE / 16/10 / 18/12 | 8.607 / 15.703 / 52.002 | | 8.894 / 16.681 / 51.827 |
| OI 30.000 call / put | 1.033 / 3.690 | | 1.543 / 4.556 |

Il regime A non cambia segno. Cambiano tre cose. (1) Gli zeri scendono verso il prezzo: tutte di
149 punti, lo 0DTE di 156. La settimana ora ha uno zero dentro la griglia. (2) La GEX A dello 0DTE
cresce in valore assoluto del 24% con un OI quasi uguale. (3) L'implicita dello 0DTE scende di 7,9
punti dopo i dati delle 14:30. **L'OI "di ieri" non e' fermo**: sul 30.000 ci sono +510 call e +866
put rispetto alle 11:04.

## Cosa Dicono Per NQ

1. **Il regime, se vale la convenzione A.** La GEX A e' negativa sui tre orizzonti, e il prezzo sta
   sotto tutti e tre gli zeri. Lo zero dello 0DTE e' a 338,8 punti, circa 2,4 volte la mossa attesa
   dello 0DTE. Per il modello la copertura andrebbe nel verso del movimento fino alla chiusura
   ([`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md), "La GEX calcolata da
   noi"). **Il segno resta un'ipotesi** [GPP]. VAPG trova la gamma dei market maker su SPX di solito
   positiva; GPP trova i dealer corti quasi ovunque, se si sommano call e put.
2. **Il prezzo e' sullo strike piu' pesante.** Il 30.000 e' il muro call e put in A e il primo peso
   in B, per tutte, per la settimana e per lo 0DTE. Il prezzo e' a 24,5 punti. Vicino alla scadenza la
   gamma degli strike al denaro cresce (e' la formula di Black-76, non un dato di mercato), e questo
   spiega la GEX A dello 0DTE piu' grande con lo stesso OI. **Lettura nostra**: la descrizione della
   gamma di stasera e' piu' sensibile al prezzo che alle 11:04. Il "dove" (30.000) regge alle due
   convenzioni; il "segno" no.
3. **Il "dove" e la catena.** Sotto 30.100 l'OI e' quasi tutto put: sul 30.000 4.556 contro 1.543, sul
   29.500 3.067 contro 241. Per questo A e B mettono il peso negli stessi strike. **Lettura nostra**:
   la catena e' sbilanciata sulle put sotto il prezzo di ieri. Se siano coperture comprate dai clienti
   (dealer corti, come nella A), il dato non lo dice.
4. **Due scadenze fanno tre quarti della gamma.** Lo 0DTE scade alle 22:00 di stasera, il 16/10 alle
   22:00 di domani. **Osservazione**: se l'OI non cambia, sabato la GEX A di NQ resta circa un quarto
   di quella di stasera (le scadenze dopo il 16/10 sommano -428,5).
5. **La scala dice quanto conta.** 2.915 contratti per 1% sono lo 0,71% del volume medio di cassa, e
   lo 0,51% di quello che la cassa ha gia' fatto oggi. BPVZ vede effetti sull'autocorrelazione dei
   rendimenti brevi gia' con il dealer al 18-19% del flusso, e non ha scenari sotto il 18%.
   L'autocorrelazione dice se una variazione e' seguita da una dello stesso segno (continuazione) o
   di segno opposto (inversione). **Nostra estrapolazione, non una previsione del paper**: su NQ
   l'effetto e' piccolo. DPS: sulle opzioni brevissime chi da' liquidita' si copre soprattutto
   abbinando gli ordini. **Lettura nostra**: per lo 0DTE la GEX e' un tetto del flusso di copertura,
   non il flusso ([sintesi](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
6. **La volatilita'.** Dopo PPI e vendite al dettaglio (14:30 [mac]) l'implicita dello 0DTE scende
   da 32,4% a 24,5%. **Osservazione**: e' coerente con una varianza dell'evento che, una volta
   passata, esce dal prezzo. Il 16/10 sale invece a 24,2% e prezza 1,30 volte la realizzata. Domani
   alle 05:30 parla Warsh [mac], prima della mensile. Dal 19/10 l'implicita torna sotto la realizzata.
7. **I paper, come ipotesi.**
   - **BDLM**: sui future S&P il momento dell'ultima mezz'ora (21:30-22:00 ora italiana) c'e' solo
     nei giorni di gamma netta negativa (6,63, t 4,78, contro 0,82, t 1,03). Oggi la gamma e'
     "negativa" solo nella convenzione A. E' l'ipotesi B della scheda
     [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md), mai provata.
   - **BB e VAPG**: con gamma alta, il giorno dopo il prezzo si muove meno (BB); l'effetto mediano
     sulla volatilita' e' piccolo (VAPG: -0,08 punti contro una variazione giornaliera di 4,5).
   - **BTW** (ipotesi C della scheda): su ES, 2003-2021, la notte fra la chiusura del giovedi' e
     l'apertura del terzo venerdi' rende +14,3 punti base (t 3,54), poi la seduta -12,9 (t -2,40).
     Ora italiana: stasera 22:00, domani 15:30.
   - **GJ**: pinning, cioe' la chiusura vicino a uno strike pesante, nell'ultima mezz'ora del venerdi'
     di scadenza: 21,3% contro il 15% atteso, sui future S&P.
   - BTW e GJ sono stati misurati su ES e S&P, non su NQ, e non sono mai stati provati qui. **Nessuno
     dei nove paper usa NDX o NQ.**

## I Livelli E Le Date

**La riserva vale per ogni riga**: nessun paper letto prova lo zero gamma, i muri o gli strike pesanti
come supporti o resistenze (finding 6 della scheda; l'unico test trovato, Popovici 2026, e' un
preprint non letto). Questi prezzi dicono **dove cambia la descrizione**, non cosa fa il prezzo. Tutti
da [gNQ] delle 18:43.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 31.070,4 | zero gamma A della sola 16/10 | posizionamento: sopra, la mensile di domani passa a GEX A positiva |
| 30.752,1 | zero gamma A, tutte | posizionamento: sopra, la GEX A totale diventa positiva; il modello cambierebbe regime |
| 30.656,9 | zero gamma A, entro 7 giorni (nuovo stasera) | posizionamento: sopra, la GEX A della settimana diventa positiva |
| 30.314,3 | zero gamma A, 0DTE | posizionamento: sopra, la GEX A dello 0DTE diventa positiva; vale fino alle 22:00 |
| 30.100 | primo strike sopra il prezzo con netta A positiva (+38,5); terzo peso B della settimana | posizionamento: da qui in su A e B divergono |
| 30.000 | call e put wall di tutte, della settimana e del 16/10; put wall dello 0DTE; primo peso B ovunque | posizionamento: dove la copertura pesa di piu' in tutte e due le convenzioni; e' a 24,5 punti dal prezzo |
| 29.950-29.800 | peso B dello 0DTE (29.950, 29.800) | posizionamento per la seduta di oggi, solo per lo 0DTE |
| 29.500 | secondo peso B di tutte; 3.067 put di OI, 2.171 scambiate oggi | posizionamento: la concentrazione di put piu' grande sotto il prezzo |
| 15/10 21:30-22:00 | ultima mezz'ora della cassa [BDLM] | la finestra del finding di BDLM, come ipotesi |
| 15/10 22:00 | scadenza 0DTE (16:00 di New York, convenzione di `gex.py`) | sparisce la gamma dello 0DTE (-737,3 su -1.747,6); inizia la finestra notturna di BTW, come ipotesi |
| 16/10 05:30 | discorso di Warsh, presidente della Fed [mac] | evento prima della scadenza mensile |
| 16/10 21:30-22:00 | ultima mezz'ora del terzo venerdi' [GJ] | la finestra del pinning di GJ, come ipotesi |
| 16/10 22:00 | terzo venerdi', la scadenza piu' pesante della settimana | sparisce il 16/10 (-581,8) |
| 18/12 15:30 | trimestrale (9:30 di New York, SOQ) | la scadenza con piu' OI: 51.827 |

## Cosa Non Sappiamo

- **Il segno dei dealer** [GPP, VAPG]: A e B sono due ipotesi. Tutte le misure del segno nei paper
  sono su SPX; nessuna e' sulle opzioni CME sui future.
- **L'OI "di ieri" cambia fra le letture.** Fra le 11:04 e le 18:43: 0DTE da 8.607 a 8.894, 16/10 da
  15.703 a 16.681, 30.000 put da 3.690 a 4.556. Puo' essere un aggiornamento della borsa, o una
  catena che arriva un po' alla volta: da qui non si verifica. Una parte del -412,0 sul 30.000 viene
  da questo OI in piu'.
- **La completezza di stasera** (`quotate` su `opzioni`). Sono quotate per intero 15/10, 16/10 e
  19/10 (400/400), 20/10 (397/400) e 18/12 (337/338). Le altre sono quotate poco, e meno che alle
  11:04: 21/10 20/394, 22/10 15/394, 23/10 17/400 (era 70), 26/10 17/270, 27/10 26/270, 28/10
  38/262, 29/10 30/258, 30/10 29/394 (era 69), 06/11 36/196 (era 195), 13/11 35/194, 20/11 33/350,
  27/11 35/192 (era 157), 30/11 29/350. Dove manca il prezzo, la volatilita' viene dallo strike
  vicino. Queste scadenze pesano il 7,4% della GEX A e il 5,3% della B (calcolo mio): il regime sta
  sulle scadenze quotate per intero.
- **Il modello di `gex.py`**: lo zero tiene ferma la volatilita' di ogni strike (sticky strike) e
  cerca solo su +-4%. Il GEX Profile di ATAS usa il volume e non coincide con questo calcolo. La
  mossa attesa usa il calendario (365 giorni), la realizzata le sedute (252).
- **Il volume delle opzioni** non dice il lato, ne' se un contratto apre o chiude una posizione.
- **Lo storico** e' di un giorno solo: non si dice se il regime e' cambiato rispetto a ieri.
- lacuna del collettore: il giro (tab 2) e' raccolto alle 18:43, prima della GEX (chiusa alle
  18:46), e stampa la lettura delle 11:51.
- lacuna del collettore: un segnale di OI definitivo (data e ora dell'OI, o una rilettura di conferma).
- lacuna del collettore: la catena e' ancora tagliata a 100 strike per lato. Dove `opzioni` = 400
  (15/10, 16/10, 19/10, 20/10, 23/10), l'OI oltre il taglio non entra.
- lacuna del collettore: la griglia dello zero e' a +-4%. Il 19/10, il 20/10 e il 30/10 non hanno
  uno zero dentro.
- lacuna del collettore: la volatilita' realizzata intraday sugli orizzonti delle scadenze (barre di
  ATAS). La macro da' solo la 20 giorni da chiusure Yahoo, e il VXN di oggi non c'e' (ultimo: 14/10).
- lacuna del collettore: la finestra del "volume del giorno" delle opzioni (da che ora conta).
- lacuna del collettore: la variabile di BDLM, cioe' il rendimento dalla chiusura di cassa di ieri
  (30.185 [sed]) alle 21:30 ora italiana. A una raccolta delle 18:43 non puo' esserci.

## Per Il Quadro

1. **NQ 30.000** e' il massimo peso in A e in B, i due muri coincidono, ed e' lo strike piu' pesante
   del 16/10. Sta dentro il valore di cassa di oggi (29.792-30.060, POC 29.960) e 82 punti sotto il
   VAL di cassa di ieri (30.082-30.478, POC 30.173) [giro, sed]. Coincide con il valore dell'order
   flow?
2. **Gli zeri contro i valori del COT** (`2-cot.md`). Lo zero della settimana (30.656,9) sta 7 punti
   sopra il VAH del 29/09 (30.557-30.650). Lo zero di tutte (30.752,1) sta fra quel valore e quello
   del 22/09 (30.888-30.980). Sotto, le 29.250 put dello 0DTE (OI 508) e il 29.250 di tutte (1.143
   put) stanno sul POC del 15/09 (29.215-29.315, POC 29.250). La fase 5 dice se le coincidenze
   contano.
3. **Gamma A negativa con il regime CHOPPY** delle 18:43 (efficienza 0,10 [giro, pannello]). BPVZ,
   con gamma negativa, si aspetta continuazione; la scala (0,71% del volume medio, 0,51% di quello di
   oggi) dice che l'effetto puo' non vedersi. Il confronto lo fa la fase 5.
4. **Il calendario della gamma**: stasera alle 22:00 esce lo 0DTE, domani alle 22:00 la mensile.
   Insieme sono tre quarti della GEX A, e fra le due scadenze alle 05:30 parla Warsh. L'implicita del
   16/10 (24,2%, 1,30 volte la realizzata) e' coerente con il regime dei tassi della macro e con la
   posizione del COT?
