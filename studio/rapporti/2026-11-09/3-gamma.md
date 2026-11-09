# Fase 3 · La Gamma Di NQ Ed ES, Lette Insieme · 2026-11-09, Prima Della Cassa

**Stato**: fatto, con quattro limiti. La GEX di `gex.py` e' delle 12:54, due ore prima della raccolta. Lo spot di SPX in X-Ray e' ancora fermo a venerdi'. La catena di NQ e' incompleta sulle scadenze lontane. Per ES manca la scala.
**Dati**: `dati/in-breve.json` (raccolta NQ 15:01) e `dati-ES/in-breve.json` (raccolta ES 14:52), sezione `gamma`. Ore italiane; i file sono in UTC.
- GEX di `gex.py` su NQZ6: 12:54, future 27.199,25 alle 12:55, M1 del bridge uguale (differenza 0). **Vecchia di due ore**: il prezzo da allora e' salito di circa 150 punti.
- GEX viva di NQ (`opzioni-vive.json`): 14:51, 13 secondi prima; finestra della variazione di 60 minuti (124 punti oggi). La lacuna delle 12:28 (17 minuti) e' chiusa.
- X-Ray (OptionsDepth su SPX): 14:50, lettore vivo (3 secondi, 0 errori), nessuna famiglia ferma. **Spot SPX fermo alla chiusura di venerdi'** (7.376,06) fino alle 15:30. Base ES-SPX 23,5, del 6/11.
- VIX e VXN ricostruiti dalla catena CME: 14:47-14:48. Cboe QQQ e NDX: fermi a venerdi' sera.
- Rapporto precedente: `precedenti/1452/3-gamma.md` (raccolta delle 12:28).

**In una riga**: su tutte le scadenze i due indici restano in acceleratore e lontani dal cambio; ma NQ e' salito sopra il punto di cambio delle opzioni di oggi, che ora frenano, mentre su ES quelle di oggi restano (debolmente) in acceleratore.

## In Parole Semplici

**Il punto**: dalle 12:28 e' successo quello che il rapporto di mezzogiorno indicava come "cambia idea". NQ e' salito sopra il prezzo dove le opzioni di oggi passano da acceleratore a freno. Ora su NQ i segnali sono opposti: il fondo accelera, la giornata di oggi frena. Su ES no. Questo parla di quanto si puo' muovere il prezzo, non di dove va.

**Dal piu' ampio al piu' vicino**

1. **La paura**: le opzioni prezzano circa il 2% al giorno sul Nasdaq e circa l'1,5% sull'S&P. Il Nasdaq e' prezzato circa il 40% piu' mosso.
   → Giornate attese larghe; domani alle 14:30 c'e' l'inflazione americana.
2. **Il costo della paura**: sul Nasdaq ora le opzioni costano meno del movimento vero delle ultime settimane; sull'S&P costano il giusto.
   → Sul Nasdaq il mercato si muove piu' di quanto l'assicurazione prezza. A mezzogiorno l'S&P era "caro": non lo e' piu'.
3. **Tutte le scadenze**: NQ resta circa il 5% sotto il suo punto di cambio; su ES X-Ray non trova un punto di cambio in circa il 3-4% sopra e sotto.
   → Concordano: acceleratore di fondo, lontano dal girare.
4. **Le opzioni che scadono oggi**: su NQ il prezzo e' circa 120 punti sopra il loro punto di cambio, e frenano. Su ES restano negative, ma quasi a zero.
   → E' il punto dove i due indici si separano. Su NQ "segnali opposti": il regime non e' netto.
5. **Quanto pesa**: su NQ la copertura supposta per un movimento dell'1% vale fra lo 0,2% e lo 0,7% del volume di un giorno.
   → Poco: freno e acceleratore ci sono sulla carta, ma sono piccoli.

**I prezzi da guardare** (ciascuno nel suo strumento, nessuna conversione fra NQ ed ES)

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 27.230 (NQ) | il punto di cambio delle opzioni NQ di oggi | sotto, le opzioni di oggi tornerebbero acceleratore: il regime di oggi va riletto |
| 27.500 (NQ) | lo strike con piu' opzioni vicino al prezzo (muro di call e di put) | li' la copertura pesa di piu' |
| circa 7.346 (ES) | dove X-Ray stima l'acceleratore piu' forte | e' la zona dove, nella stima, la copertura spingerebbe di piu' |

**Quando**: 15:30 apre la cassa e X-Ray riparte con lo spot vero · 21:30-22:00 l'ultima mezz'ora; 22:00 scadono le opzioni di oggi · domani 14:30 l'inflazione (CPI).
**Cambia idea se**: NQ torna sotto circa 27.230; oppure, dopo le 15:30, X-Ray mostra un punto di cambio vicino al prezzo di ES o la gamma al prezzo positiva.
**Quanto e' provato**: il segno dei dealer su NQ e' supposto, su ES e' stimato da un fornitore. Su 42 sedute di ES l'acceleratore del mattino non ha allargato la giornata. Che le opzioni di un indice aiutino a leggere l'altro non lo prova nessun paper: i due trovati dicono che il future S&P guida per pochi secondi, e solo come flusso.

**Le parole**: **dealer** = chi vende le opzioni e si copre sul future; **gamma** = quanto cambia la copertura quando il prezzo si muove (positiva: freno, i dealer comprano sui ribassi e vendono sui rialzi; negativa: acceleratore, inseguono il prezzo); **punto di cambio** (zero gamma, o flip) = dove si passa dall'uno all'altro; **muro** = strike con molta gamma; **opzioni di oggi** (0DTE) = che scadono oggi; **X-Ray** = la stima di OptionsDepth delle posizioni dei market maker su SPX.

## I Fatti

**Il regime.** NQ: catena CME via ATAS, **convenzione A** (si suppone dealer lunghi di call e corti di put), milioni di $ per 1%. ES: X-Ray, unita' di OptionsDepth (conta il segno), livelli in prezzi di ES con la base 23,5.

| | NQ, `gex.py` 12:54 | NQ, GEX viva 14:51 | ES, X-Ray 14:50 |
|---|---|---|---|
| prezzo | 27.199,25 (bridge) | 27.350,5 | 7.468,5 (SPX fermo 7.376,06) |
| tutte le scadenze | −493,5; zero 28.879 (+6,2%) | −425,7; zero 28.775,6 (+5,2%) | gamma al prezzo −151,2; nessun flip fra 7.155 e 7.655 di SPX (circa −3,9%, +2,8%) |
| entro 7 giorni | −222,2; zeri 27.710,6 e 28.343 | non c'e' | prossima scadenza (10/11): −10,3; entro l'1% −3,5 |
| 0DTE | −15,1; zero 27.232 (+0,12%) | **+58,5**; zero 27.230,3 (**−0,44%**) | −2,0; entro l'1% −0,7 |
| mensile | 18/12: −171,4 | non c'e' | 20/11: −25,0; **entro l'1% +13,5** |
| regime al prezzo | acceleratore | tutte acceleratore, **0DTE freno** | acceleratore, 0DTE acceleratore |

**Cosa e' cambiato dalle 12:28** (rapporto in `precedenti/1452/`).

| | alle 12:27-12:28 | alle 14:50-14:51 |
|---|---|---|
| NQ, GEX viva 0DTE | −22,0, prezzo 0,18% sotto lo zero | +58,5, prezzo 0,44% sopra lo zero: **il regime di oggi e' girato** |
| NQ, GEX viva tutte | −366,4, zero 28.982 (+6,6%) | −425,7, zero 28.775,6 (+5,2%) |
| NQ, ultima ora | finestra di 17 minuti: future −39, 0DTE −17,5 | future +14,5, zero −16,1, tutte +4,6, zero 0DTE −5,5, 0DTE +12,1 |
| ES, gamma al prezzo | −163,7 | −151,2 (meno negativa, sempre acceleratore) |
| ES, 0DTE market maker | −8,1 | −2,0 (quasi a zero, sullo spot fermo) |
| ES, acceleratore piu' forte | 7.343,5 | 7.346 |
| pressione 0DTE X-Ray | +29,0 (−8,2 trenta minuti prima) | +518,9 (+367,3 trenta minuti prima) |
| VXN / VIX ricostruiti | 32,7 / 22,9 | 32,1 / 22,6 |
| implicita meno realizzata | NQ −1,7; ES +2,2 | NQ **−2,3**; ES +1,7 |

**La convenzione B** [GPP] (dealer corti di tutto; fornitori e ATAS non la usano), `gex.py` 12:54: negativa ovunque per costruzione. Tutte −1.851,3; entro 7 giorni −1.164,3; 0DTE −609,7. La copertura peserebbe di piu' a 27.500, 26.500 e 27.000 (0DTE: 27.500, 27.000, 27.400). **Il segno vero dei dealer di NQ non si conosce.**

**X-Ray** (stima di un fornitore, su SPX, metodo non pubblicato, non provata). Fresco: lettore vivo, nessuna famiglia ferma; lo spot e' quello di venerdi' fino alle 15:30. Il profilo (gamma per livello di prezzo, 7.155-7.655 di SPX) e' negativo ovunque: anche i "positivi" valgono circa −44. L'acceleratore piu' forte e' 7.322,5 di SPX (−198,5), cioe' **7.346 di ES**, 122,5 punti sotto il prezzo. Nessun freno, ne' oggi (`observed`) ne' nella previsione per domani dalle 15:30 (`outlook`: picco 7.345 di SPX, nessuno zero). Sullo 0DTE gli strike piu' pesanti sono saliti: 7.490 di SPX (+11,6, segno del freno, call), 7.505 (−8,5), 7.550 (+5,1); a mezzogiorno erano 7.400, 7.270 e 7.455. Mensile del 20/11: 7.500 (−28,3), 7.700 (+17,0).

**I flussi di X-Ray** (DEX = delta scambiato, un flusso, non una posizione; accumulato dalle 10:00 italiane; positivo = il gruppo ha comprato delta; unita' di OptionsDepth).

| gruppo | 0DTE oggi (ultimi 30 minuti) | tutte le scadenze oggi (ultimi 30 minuti) |
|---|---|---|
| retail | −1.232,5 (−434,8) | +3.452,6 (+737,1) |
| clienti professionali | +334,8 (+177,4) | −46,7 (+156,7) |
| firm | +389,7 (+96,3) | −1.060,2 (−524,9) |
| broker-dealer | −16,5 (+4,0) | −62,6 (−36,9) |

**La volatilita'** (implicita = il movimento prezzato; realizzata = quello vero). Quattro domande delle due raccolte.

| | NQ | ES |
|---|---|---|
| indice a 30 giorni | VXN 32,1 (catena, 14:48) | VIX 23,1 (meteo); 22,6 dalla catena alle 14:47 |
| movimento atteso: oggi / un giorno | 375 / 552 punti | 68 / 109 punti |
| realizzata 20 sedute | 34,4 | 21,4 |
| implicita meno realizzata | **−2,3: si muove piu' del prezzato** | +1,7: in linea |
| skew | 8,3: put care | 9,3: put care |
| breve contro mese | inversa (+10,1) | inversa (+6,3) |

- Rapporto VXN/VIX ricostruiti: circa 1,42. Ma dalla chiusura di venerdi' NQ ha fatto +1,0% e ES +0,95% (fase 4): oggi si sono mossi quasi uguali.
- Realizzata di NQ (`gex.py`, rendimenti log a 5 minuti, 276 × 252): 21,5% nelle ultime 4 ore (36,6% a mezzogiorno), 29,5% nelle 24.

**NQ, mossa attesa e completezza** (`gex.py` 12:54).

| scadenza | volatilita' al prezzo | mossa attesa | open interest | quotate | nota |
|---|---|---|---|---|---|
| 9/11 (0DTE) | 49,6% | 435 | 7.509 | 480/480 | completa |
| 10/11 | 43,5% | 727 | 1.989 | 470/480 | contiene il CPI |
| 13/11 | 40,4% | 1.203 | 8.233 | 474/480 | la piu' pesante della settimana; PPI |
| 20/11 | 34,1% | 1.635 | 11.475 | **162/480** | mensile, **open interest non stabile** |
| 30/11 | 34,0% | 2.238 | 5.003 | **59/418** | fine mese, quotata al 14% |
| 18/12 | 28,0% | 2.491 | 48.068 | 272/426 | la piu' pesante; ora stabile |

- **L'open interest totale e' +14,8% rispetto al calcolo delle 12:22**, quasi tutto sulla 20/11 (+5.916) e sulla 18/12 (+4.220). Il collettore avverte: conta solo gli strike letti. **Lettura nostra**: sono strike letti in piu' (la 20/11 passa da 82 a 162 quotate), non posizioni nuove. La 24/11 e' saltata.
- **Strike pesanti** (open interest, GEX netta A): 26.500 (put 2.321, −76,6), 27.000 (put 1.134, con 1.193 put scambiate oggi; −49,5), 27.500 (call 569, put 1.834; −38,4), 26.000 (−32,3), 26.250 (−30,5), 28.500 (call 679, put 1.825; −26,7). Contano le put.

**La scala** (NQ, calcolata nel collettore). 1% = 272 punti. Convenzione A: 907 future per 1%, lo 0,17% del volume medio di cassa delle 20 sedute (524.251). B: 3.403 future, lo 0,65%.

## Cosa Dicono Per NQ (E Per ES, Letti Insieme)

- **Il fondo concorda.** NQ sotto il suo zero del 5,2-6,2% (A, supposta); ES senza flip entro circa il 3-4% (X-Ray). Il regime di fondo non gira in una seduta normale. **In teoria** la copertura segue il movimento invece di assorbirlo [BB, BDLM, VAPG]. Il verso del meccanismo e' provato su S&P e azioni; l'effetto sulla volatilita' e' piccolo (VAPG: mediano −0,08 punti al giorno). Su NQ nessuno studio.
- **Lo 0DTE ora divide i due.** Su NQ la GEX dello 0DTE e' passata da −22 a +58,5 perche' il prezzo e' salito sopra lo zero (27.230). L'open interest e' quello di ieri: l'hanno girata prezzo e tempo, non posizioni nuove. Su NQ quindi **segnali opposti**: acceleratore su tutte, freno sullo 0DTE, che pesa di piu' verso la chiusura. Su ES lo 0DTE e' quasi a zero (−2,0) e calcolato sullo spot fermo: non e' un acceleratore deciso. La mensile di ES vicino al prezzo resta di segno freno (+13,5).
- **I flussi vanno nello stesso verso di mezzogiorno, piu' forti.** Sullo 0DTE di SPX il retail vende delta, firm e professionisti lo comprano; su tutte le scadenze il retail compra e le firm vendono. La pressione 0DTE e' salita molto (+29 → +519); il suo verso non e' documentato. **Non e' lo stesso oggetto** della variazione della GEX viva di NQ: la prima e' un flusso scambiato su SPX, la seconda un effetto del prezzo su posizioni ferme. Che vadano "nello stesso verso" non si puo' dire senza la variazione di X-Ray nel tempo (lacuna).
- **La volatilita' si e' spostata.** Su NQ l'implicita meno realizzata e' scesa sotto −2: il Nasdaq si muove piu' di quanto le opzioni prezzano (soglia convenzionale, non provata). Su ES l'assicurazione non e' piu' cara (+1,7). La struttura e' inversa su tutti e due: le scadenze brevi costano piu' del mese. **Osservazione**: coerente con il CPI di domani dentro la 10/11 (43,5%). Non e' una misura dell'evento.
- **Quanto pesa.** Su NQ lo 0,17-0,65% del volume di un giorno. In BPVZ l'effetto e' grande solo con il dealer oltre circa il 18% del flusso; DPS suggerisce che sulle scadenze brevissime conta meno l'abbinamento degli ordini. **Lettura nostra**: indicazione debole, in tutti e due i versi.
- **I paper, come ipotesi.** BDLM: sui future S&P il momento dell'ultima mezz'ora (21:30-22:00) c'e' solo con gamma negativa (t 4,78); su NQ c'e' senza condizioni (t 7,97), la condizione di gamma non e' provata. Ipotesi D ("con l'acceleratore i livelli tendono a rompersi"): non confermata su 42 sedute di ES (0,55 contro 0,74 del movimento atteso, p 0,24).
- **Il passaggio di informazione fra i due indici** (ricerca del 9/11, due paper letti e verificati). **Hasbrouck 2003** (versione di lavoro 2000): la scoperta del prezzo sta sugli E-mini, ma S&P 500 e Nasdaq-100 sono stimati **separatamente** (quota d'informazione ES 0,854-0,882, NQ 0,836-0,853, 64 giornate del 2000); la guida si misura **in secondi** (con 10 secondi di ritardo sull'S&P il future regolare passa davanti); l'unico numero incrociato e' una frase, QQQ verso SPY al massimo circa il 10%. **Le Coz e altri 2024**: la pressione degli ordini nasce sullo strumento piu' liquido e muove i correlati, ma il flusso **spiega il prezzo della stessa finestra, non prevede**; il Nasdaq-100 non e' nominato. **Nessuno dei due misura gamma, muri o zeri fra indici.** Chordia e altri 2021 (flusso di opzioni di altri indici sull'S&P, settimanale) non e' ancora letto. Qui il confronto resta **descrittivo**: regimi, segni, flussi, volatilita', mai livelli convertiti.

## Le Quattro Domande

- **1 Il regime**: NQ segnali opposti (tutte acceleratore, 0DTE freno, GEX viva 14:51); ES acceleratore al prezzo e sullo 0DTE (X-Ray, spot fermo). Dice quanto, non dove. Esito: ipotesi D non confermata su 42 sedute; si rilegge dopo le 15:30.
- **2 Il meteo**: NQ teso (VXN 32,1), implicita meno realizzata −2,3, si muove piu' del prezzato, put care, struttura inversa. ES teso (VIX 23,1), +1,7, in linea, put care, inversa. Soglie ±2 convenzionali, non provate.
- **3 I muri sul bordo del valore**: nessuno su NQ (soglia 25 punti) ne' su ES (soglia 6). Su NQ i muri a 27.500 sono 75 punti sopra il muro alto del ping-pong (27.425). Su ES l'acceleratore piu' forte (7.346) e' 23,75 punti sopra il muro basso del ping-pong (7.322,25). Ipotesi E non provata.
- **4 Il grilletto**: e' l'order flow (fase 4), non le opzioni. Qui nessun segnale.

## I Livelli E Le Date

Nessun paper prova questi livelli come supporti o resistenze. Sono i prezzi dove, nel modello, la copertura cambia o pesa di piu'.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.776-28.879 (NQ) | zero di tutte, A (viva 14:51 e `gex.py` 12:54) | il regime di fondo girerebbe; lontano, si nomina come zona |
| 27.710,6 e 28.343 (NQ) | zeri entro 7 giorni, A (12:54) | il regime della settimana cambierebbe |
| 27.500 (NQ) | call e put wall di tutte e dello 0DTE; peso B piu' alto | posizionamento: la copertura pesa di piu' |
| 27.230 (NQ) | zero 0DTE, A (viva e `gex.py` coincidono) | sotto, il regime di oggi tornerebbe acceleratore: da rileggere |
| 27.000 (NQ) | put wall 0DTE nell'ultima ora (alternato con 27.500); 1.193 put scambiate | posizionamento sullo 0DTE: due muri quasi alla pari |
| 26.500 (NQ) | lo strike piu' pesante (put 2.321) | posizionamento, settimana |
| 7.346 (ES) | acceleratore piu' forte di X-Ray (7.322,5 di SPX + 23,5) | dove, nella stima, la copertura spingerebbe di piu' |
| oggi 15:30 | apertura della cassa di New York | X-Ray riparte con lo spot vero: si rilegge ES |
| oggi 21:30-22:00 | ultima mezz'ora; 22:00 scade lo 0DTE | la finestra di BDLM; lo 0DTE pesa di piu' |
| 10/11 14:30 | CPI | dentro la scadenza 10/11 di NQ (43,5%) |
| 13/11 14:30 | PPI; la settimanale piu' pesante di NQ | |
| 20/11 15:15 | mensile SPX del mattino (09:15 di New York) | la mensile di X-Ray |

## Cosa Non Sappiamo

- **Il segno dei dealer**: su NQ supposto (A), su ES stimato da OptionsDepth con metodo non pubblicato. GPP lo trova corto quasi ovunque (SPX 1996-2001); VAPG di solito lungo (2020-2023).
- **La catena di NQ lontana e' incompleta**: 20/11 quotata al 34% con open interest instabile, 30/11 al 14%, 18/12 al 64%; la 24/11 manca. La GEX di tutte e il suo zero valgono meno di quelli dello 0DTE e della settimana, completi.
- **`gex.py` e GEX viva differiscono** (−493,5 contro −425,7; zero 28.879 contro 28.776): due ore e circa 150 punti di distanza, e un open interest letto diverso.
- **X-Ray fuori cassa**: GEX per scadenza ed "entro l'1%" sono sullo spot di venerdi' (7.376); il prezzo implicito da ES e' circa 7.445, quasi l'1% piu' su. La base e' del 6/11.
- **Il verso della pressione 0DTE** di X-Ray non e' documentato.
- **Il passaggio di informazione fra opzioni di SPX e di NDX**: nessun paper lo misura; Chordia e Holowczak non letti (SSRN dietro Cloudflare).
- lacuna del collettore: **la GEX di `gex.py` non e' ricalcolata** per la raccolta delle 15:01 (e' delle 12:54).
- lacuna del collettore: **la scala di ES**: X-Ray in unita' di OptionsDepth, nessuna GEX in $ di ES dal 5/11.
- lacuna del collettore: **la variazione nel tempo di X-Ray** (gamma al prezzo e GEX 0DTE nell'ultima ora): senza, non si confronta con la variazione della GEX viva di NQ.
- lacuna del collettore: **il VIX del meteo di ES** (23,08) non dichiara la fonte accanto; quello della catena e' 22,6.

## Per Il Quadro

- **Strike pesanti di NQ (26.500, 27.000, 27.500) contro COT e valore**: 27.500 e' 75 punti sopra il muro alto del ping-pong (27.425); lo zero 0DTE (27.230) e' dentro il valore di venerdi' (27.113-27.306), fra POC (27.175) e VAH, e il prezzo (27.350) ora sta sopra quel valore.
- **I muri vicini al bordo del valore, con il regime**: nessuno entro le soglie. ES pero' e' gia' sul muro alto del ping-pong (7.469, prezzo 7.468,5) in regime di acceleratore stimato (ipotesi E: si guarda l'accettazione oltre, non provata). NQ e' 75 punti sotto il suo muro alto, con lo 0DTE in freno.
- **I regimi di fondo concordano, lo 0DTE no**: va riletto dopo le 15:30 con lo spot vero di X-Ray, prima di dire che i due indici concordano anche oggi.
- **La volatilita'**: NQ si muove piu' del prezzato (−2,3), ES in linea; struttura inversa su tutti e due. Coerente con la macro (CPI domani 14:30) e con lo scalino giu' della settimana su tutti e due?
