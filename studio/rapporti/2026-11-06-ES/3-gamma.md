# Fase 3 · La Gamma · 6 novembre 2026 (ESZ6)

**Stato**: parziale (manca la scala in contratti di ES: le unita' di X-Ray non sono dichiarate; X-Ray letto prima della cassa, con lo spot SPX fermo).
**Dati**: `in-breve.json` (gamma), `xray.json`, `quattro-domande-ESZ6.json`, `macro.json` (solo il VIX), `raccolta.json`; raccolta delle 15:05 ora italiana (09:05 di New York), **prima della cassa** (15:30). **X-Ray**: lettore vivo, ultimo giro 15 secondi prima, tutte le 22 famiglie ricevute da 0-1 minuti, ultimo istante 15:03; **il server e' ripartito alle 15:00 dopo uno stop da ieri sera** (nota dell'orchestratore), quindi al momento della raccolta aveva circa 5 minuti di lettura viva. Lo spot SPX (7.358,14) e' la chiusura di ieri, fermo. **GEX viva e VIX ricostruito dalla catena**: fermi al 5/11 alle 18:33 (20,5 ore), non letti; su ES non valgono comunque. **VIX ufficiale** (Yahoo): 26,40 alle 14:49. Errori della raccolta: nessuno.

**In una riga**: prima della cassa, la stima di X-Ray mette i market maker dalla parte che accelera su tutte le scadenze e dalla parte che frena sulle opzioni di oggi: segnali opposti, con la paura alta e l'assicurazione cara.

## In Parole Semplici

**Il punto**: chi vende le opzioni sull'S&P 500, secondo la stima di X-Ray, oggi tende ad amplificare i movimenti se si guardano tutte le scadenze, e a smorzarli se si guardano solo le opzioni che scadono stasera. Le due cose si contraddicono: il quadro non e' netto, e va riletto dopo l'apertura delle 15:30.

**Dal piu' ampio al piu' vicino**

1. **La paura**: il VIX e' a circa 26,5, piu' alto del 96% dell'ultimo anno, ma sotto i 28,2 di ieri.
   → Le opzioni prezzano circa 124 punti di ES in una giornata, piu' di quanto il mercato si e' mosso in media nell'ultimo mese.
2. **Tutte le scadenze**: la gamma stimata e' negativa a ogni prezzo fra circa 7.150 e 7.650 di ES; nessun punto di cambio dentro quella fascia.
   → In teoria i market maker inseguono il prezzo: amplificano. Dice quanto, non dove.
3. **Le opzioni di oggi**: lo stesso conto fatto solo sulla scadenza di stasera e' positivo.
   → Su quelle i market maker frenerebbero, e il loro peso cresce verso la chiusura delle 22:00.
4. **Adesso**: gli strike con piu' copertura vicino al prezzo stanno a circa 7.381 e 7.401 di ES, a 10 punti dal prezzo.
   → Sono i prezzi dove la copertura pesa di piu', vicini al valore di ieri; nessuno studio li prova come pavimenti o tetti.

**I prezzi da guardare**

| prezzo (ES) | cos'e' | se ci arriva |
|---|---|---|
| circa 7.637 | il punto dove l'acceleratore e' piu' forte (stima su SPX) | e' anche il bordo basso del valore delle ultime 20 sedute: una coincidenza da confrontare |
| circa 7.526 | lo strike piu' pesante della scadenza mensile del 20/11 (acceleratore) | vicino al bordo alto del valore di ieri (7.538) |
| circa 7.401 | strike della scadenza di lunedi', acceleratore, vicino al prezzo piu' scambiato di ieri (7.406) | la copertura cambia di piu' li' intorno |
| circa 7.381 | lo strike delle put di oggi con piu' freno, sotto il bordo basso del valore di ieri (7.388) | e' dove il freno di oggi pesa di piu', se il segno stimato e' giusto |

**Quando**: 15:30 apertura della cassa, lo spot di X-Ray riparte e il regime si rilegge · 16:00 fiducia Michigan · 22:00 scadono le opzioni di oggi.
**Cambia idea se**: a cassa aperta compare un punto di cambio vicino al prezzo, o lo 0DTE diventa negativo (allora tutte le scadenze direbbero acceleratore).
**Quanto e' provato**: il segno e' la stima di un fornitore, su SPX, con un metodo non pubblicato; i paper misurano un effetto della gamma sulla volatilita' piccolo in media; sui nostri dati il regime del mattino non ha allargato la giornata (42 sedute); nessun livello e' provato.

**Le parole**: **market maker** = chi vende le opzioni e si copre sul future; **gamma** = quanto deve cambiare la copertura quando il prezzo si muove; **freno / acceleratore** = copertura contro il movimento (lo smorza) o nel suo verso (lo allunga); **0DTE** = opzioni che scadono oggi; **strike** = il prezzo di esercizio di un'opzione; **punto di cambio** (flip) = il prezzo dove la gamma cambia segno; **implicita / realizzata** = il movimento che le opzioni prezzano / quello vero.

## I Fatti

**X-Ray** (OptionsDepth su SPX via ATAS, istante 15:03 ora italiana; unita' di OptionsDepth, conta il segno). In prezzi di ES con la base ES-SPX del 5/11, 25,72 (mediana dei minuti di cassa); alla chiusura di ieri la differenza era circa 23: i prezzi di ES hanno qualche punto di incertezza. Le conversioni dei singoli strike (SPX + 25,72) sono mie: il collettore da' in prezzi di ES solo il prezzo, la gamma al prezzo e l'acceleratore piu' forte.

| misura | valore | in prezzi di ES |
|---|---|---|
| gamma al prezzo | −130,6 (acceleratore) | prezzo ES 7.390,25 |
| profilo gamma, 201 strike SPX 7.124-7.624 | tutti negativi, da −130,4 (SPX 7.371-7.379) a −199,9 (SPX 7.606-7.614) | nessuno zero fra circa 7.150 e 7.650 |
| picchi negativi `observed` (dalle 10:00) | SPX 7.619 (−203,5), 7.611,5 (−199,9), 7.184 (−154,1) | 7.644,7 · **7.637,2** · 7.209,7 |
| picchi negativi `outlook` (lunedi' 9/11) | SPX 7.409 (−245,0), 7.261,5 (−186,2), 7.604 (−124,8) | 7.434,7 · 7.287,2 · 7.629,7 |
| freno piu' forte, flip oggi e lunedi' | nessuno | - |

| GEX dei market maker | call | put | netto | entro 1% | strike piu' pesanti SPX (netto) -> ES |
|---|---|---|---|---|---|
| 0DTE (scade oggi 22:00) | −9,6 | +26,3 | **+16,7** | +11,6 | 7.455 (+8,9) -> 7.480,7; 7.355 (+7,8, put +8,0) -> 7.380,7; 7.300 (+7,0) -> 7.325,7 |
| prossima (lun 9/11, 22:00) | −10,5 | −7,1 | **−17,6** | −12,3 | 7.425 (−6,2) -> 7.450,7; 7.375 (−5,9) -> 7.400,7; 7.350 (+5,8) -> 7.375,7 |
| opex (ven 20/11, 15:15) | +44,6 | −67,8 | **−23,2** | +6,7 | 7.500 (−23,1, put −29,7) -> 7.525,7; 7.700 (+13,0) -> 7.725,7; 7.000 (−12,1) -> 7.025,7 |

**Delta scambiato per gruppo** (flusso, non posizione; somma dalle 10:00 ora italiana, primo minuto del giorno compreso). Pressione di copertura 0DTE 726,4, 30 minuti prima 754,8 (verso non documentato).

| gruppo | 0DTE oggi | 0DTE ultimi 30 min | tutte oggi | tutte ultimi 30 min |
|---|---|---|---|---|
| retail | −644,6 | −263,5 | −685,6 | −924,3 |
| clienti professionali | −199,2 | −4,4 | −263,2 | −29,6 |
| firm | +106,6 | +280,8 | +590,8 | +671,9 |
| broker | +12,1 | +17,3 | +131,9 | +38,2 |

**Il meteo** (`quattro-domande-ESZ6.json`): VIX 26,59 ("tesa"; Yahoo 26,40 alle 14:49, ieri 28,18, 96° percentile a un anno); movimento atteso di un giorno 124 punti di ES; fino alla chiusura 38 punti (dalla volatilita' dello 0DTE della catena CME, eta' non dichiarata); realizzata a 20 sedute 21,4; **implicita meno realizzata +5,2**; breve meno mese +1,5 (piatta); skew 10,4 (put care). Ieri la cassa di ES ha fatto 335 punti (7.712,5-7.377,25), circa 2,7 movimenti attesi. **Il volume del future** (sedute): cassa del 5/11 2.886.204 contratti; la notte di oggi 438.940 fino alle 15:05.

## Cosa Dicono Per ES

**Il regime** (punto 1, su X-Ray al posto della convenzione A, decisione del 5/11, `x-ray-in-vivo.md` "Come si leggono", punto 2). Su tutte le scadenze la gamma stimata e' negativa a ogni strike del profilo, e le scadenze di lunedi' e del 20/11 sono nette negative: **acceleratore**. Lo 0DTE e' netto positivo (+16,7, quasi tutto dalle put): **freno**. Il collettore scrive **segnali opposti**, e lo confermo. Lo 0DTE pesa di piu' man mano che si avvicina la scadenza delle 22:00 (`come-si-leggono-le-opzioni-dal-vivo.md`, punto 4); ma la sua GEX cambia col prezzo, non con posizioni nuove. Il regime **dice quanto, non dove**.

**Il segno** (punto 2). Su ES non si calcolano piu' le convenzioni A e B. Il segno e' la stima di OptionsDepth, dai dati del Cboe con il tipo di partecipante: non e' una misura. Che i dealer possano essere corti di gamma quasi ovunque e' anche cio' che GPP ricava su SPX 1996-2001; VAPG trova invece la gamma dei market maker di solito positiva (2020-2023). **Il segno vero non si conosce.**

**X-Ray, prima la freschezza** (punto 3). Lettore vivo, nessuna famiglia ferma. Ma: il server e' ripartito alle 15:00; lo spot SPX e' fermo alla chiusura di ieri, quindi la "gamma al prezzo" e il netto per scadenza sono calcolati a 7.358 di SPX, non al prezzo di adesso; il profilo copre solo circa −3%/+4% dallo spot, e un punto di cambio fuori dalla fascia non si vedrebbe. La lettura vera e' quella dopo le 15:30, con lo spot vivo.

**Gli strike pesanti** (punto 4). Vicino al prezzo: SPX 7.355 sullo 0DTE (put, freno; circa 7.381 di ES) e SPX 7.375 sulla scadenza di lunedi' (acceleratore; circa 7.401). La scadenza piu' pesante e' l'opex del 20/11 (netto −23,2), con SPX 7.500 (circa 7.526) come strike dominante, da put. Il punto piu' acceleratore del profilo e' lontano, circa 7.637, 247 punti sopra.

**La mossa attesa** (punto 5). Il VIX prezza 124 punti al giorno contro una realizzata di 21,4: **assicurazione cara** (sopra +2, soglia convenzionale non provata). Pero' ieri la giornata e' stata quasi tre volte il movimento atteso: la realizzata a 20 sedute non ha ancora assorbito i giorni recenti, e "cara" va letto con questo.

**La scala** (punto 6). **Non calcolabile**: le unita' di X-Ray non sono dichiarate e non si convertono in contratti di ES (lacuna sotto). Il termine di paragone resta il volume: quasi 2,9 milioni di contratti nella cassa di ieri. La sintesi del 15/10 ricorda che sulla catena di NQ la copertura stimata era lo 0,4-0,5% del flusso (estrapolazione nostra).

**Il flusso per gruppo** (osservazione). Da stamattina il retail ha venduto delta, i firm l'hanno comprato, anche negli ultimi 30 minuti. E' un flusso, non chi e' lungo; il primo minuto del giorno e' un salto noto che sposta la somma (`x-ray-in-vivo.md`, "I minuti anomali"); e la pausa del server rende incerta la copertura dei minuti.

**I paper** (punto 8; ipotesi, non regole; sintesi del 15/10 in `quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`).
- **BB**: con gamma alta il giorno dopo e' piu' calmo; sugli indici il segno e' assunto.
- **BDLM** [Tab. 7]: sui future S&P il momento dell'ultima mezz'ora c'e' solo con gamma netta negativa (6,63, t 4,78, contro 0,82, t 1,03). Oggi il segno e' negativo su tutte le scadenze e positivo sullo 0DTE: quale conti per BDLM non si sa.
- **VAPG**: l'effetto mediano della gamma sulla volatilita' del giorno e' piccolo (−0,08 punti).
- **DPS**: sulle opzioni brevissime chi da' liquidita' si copre poco in delta e abbina molto gli ordini; la parte 0DTE di una GEX puo' pesare meno di quanto conta.
- **BPVZ** (simulazione): la gamma negativa rende positiva l'autocorrelazione dei rendimenti brevi, cioe' i movimenti tendono a continuare.

## Le Quattro Domande

- **Il regime**: acceleratore al prezzo e su tutte le scadenze, freno sullo 0DTE: **segnali opposti**. La frase del video "con l'acceleratore i livelli tendono a rompersi" e' **un'ipotesi**: su 42 sedute di ES il regime 0DTE delle 9:35 non ha allargato la giornata (0,55 contro 0,74 del movimento atteso, p 0,24, verso opposto; ipotesi D, prova in avanti dal 6/11, oggi compreso).
- **Il meteo**: paura "tesa", implicita meno realizzata +5,2 (assicurazione cara), struttura piatta, put care.
- **I muri sul bordo del valore**: nessun muro entro 6 punti dai bordi della cassa di ieri (il collettore usa come muro l'acceleratore piu' forte, 247 punti sopra). **Osservazione mia**: lo strike delle put 0DTE con piu' freno, circa 7.381, sta 7,5 punti sotto il bordo basso del valore di ieri (7.388,25): fuori dalla soglia di 6 del collettore e da quella di 4 dell'ipotesi E, e misurato alle 9:03 di New York, non alle 9:35. Ipotesi E non provata (un caso dal 23/10).
- **Il grilletto**: e' l'order flow, non le opzioni.

## I Livelli E Le Date

| prezzo ES o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| circa 7.645 e 7.637 | picchi acceleratore `observed` di X-Ray (SPX 7.619, 7.611,5) | dove, in teoria, la copertura amplificherebbe di piu'; confluenza con il bordo basso del valore a 20 sedute (7.640,25), fonti diverse |
| circa 7.526 | strike piu' pesante dell'opex del 20/11 (SPX 7.500, netto −23,1) | posizionamento della scadenza mensile; vicino al bordo alto del valore di ieri (7.537,75) |
| circa 7.481 | strike 0DTE piu' pesante (SPX 7.455, freno) | dove il freno di oggi pesa di piu' sopra il prezzo |
| circa 7.435 | picco acceleratore `outlook` per lunedi' (SPX 7.409) | il livello che X-Ray prevede per la prossima seduta |
| circa 7.401 | strike della scadenza di lunedi' (SPX 7.375, acceleratore) | vicino al prezzo piu' scambiato di ieri (7.406) |
| circa 7.381 | strike put 0DTE (SPX 7.355, freno) | sotto il bordo basso del valore di ieri; posizionamento dello 0DTE |
| 6/11 15:30 | apertura della cassa | lo spot di X-Ray riparte: si rilegge il regime |
| 6/11 16:00 | fiducia Michigan | evento di peso medio |
| 6/11 22:00 · 9/11 22:00 · 20/11 15:15 | scadenze 0DTE, prossima, opex | lo 0DTE pesa di piu' verso le 22:00 |

**Nessun paper prova questi livelli come supporti o resistenze** (scheda `opzioni-gamma-es`, finding 6): sono il posizionamento stimato, non pavimenti o tetti.

## Cosa Non Sappiamo

- **Il segno vero** dei market maker: X-Ray e' una stima con metodo non pubblicato, su SPX; nessun paper usa OptionsDepth.
- **Lo stato a cassa aperta**: tutto e' calcolato con lo spot SPX di ieri e prima delle 15:30.
- **Oltre la fascia del profilo** (SPX 7.124-7.624) non si vede se ci sia un punto di cambio.
- **La base ES-SPX** di oggi: si usa quella di ieri; incertezza di qualche punto.
- lacuna del collettore: la scala per ES (GEX di X-Ray in contratti di ES per 1%, o in $ per 1%) contro il volume del future.
- lacuna del collettore: gli strike pesanti per scadenza e i picchi `outlook` in prezzi di ES (convertiti qui a mano con la base).
- lacuna del collettore: la copertura in minuti di ogni famiglia di X-Ray dopo la pausa del server (se ATAS ha scritto la cache durante lo stop); il delta accumulato puo' essere incompleto.
- lacuna del collettore: l'eta' e la fonte del VIX (26,59) e della volatilita' 0DTE delle quattro domande; il VIX di Yahoo nello stesso istante era 26,40.
- lacuna del collettore: la GEX viva e il VIX ricostruito sono fermi da 20,5 ore; il collettore li ha raccolti senza segnarli come vecchi.

## Per Il Quadro

- **Strike pesanti contro COT e valore**: circa 7.381 (put 0DTE, freno) e 7.401 (lunedi', acceleratore) stanno attorno al bordo basso (7.388,25) e al prezzo piu' scambiato (7.406) della cassa di ieri; circa 7.526 (opex) vicino al bordo alto (7.537,75). Coincidono con i livelli del COT o della notte?
- **Muri vicini a un bordo del valore, con il regime**: nessuno entro la soglia; il piu' vicino e' lo strike put 0DTE a 7,5 punti sotto il bordo basso, in regime a segnali opposti (ipotesi E, non provata).
- **La confluenza lontana**: l'acceleratore piu' forte (7.637) e il bordo basso del valore a 20 sedute (7.640,25), fonti diverse, 247 punti sopra il prezzo.
- **Il meteo con la macro**: VIX al 96° percentile ma in calo da ieri, assicurazione cara dopo una giornata di 2,7 movimenti attesi: coerente con il quadro macro della fase 1?
