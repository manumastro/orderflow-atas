# Fase 3 · La Gamma · 2026-11-06

**Stato**: parziale (la GEX viva e' ferma da ieri sera, la catena e' senza la trimestrale di dicembre)
**Dati**: `in-breve.json` (gamma), `gex-NQZ6.json` via in-breve, `opzioni-vive.json`, `quattro-domande-NQZ6.json`, `xray.json` via in-breve, `macro.json` (VXN, VIX); raccolta 15:05 ora italiana, nessun errore. Freschezza: GEX di `gex.py` 15:02 (riusata dal giro), future 27.112,5 alle 15:04, M1 del bridge 27.105,75 alle 15:04; **GEX viva ferma al 5/11 18:33** (20,5 ore); VXN e VIX ricostruiti al 5/11 18:31; VXN ufficiale e Cboe alla chiusura del 5/11 (22:15); VIX ufficiale 14:49 di oggi; X-Ray 15:03, lettore vivo, spot SPX fermo alla chiusura di ieri (prima della cassa).

**In una riga**: con la supposizione solita sul segno, i dealer su NQ inseguono il prezzo a ogni livello vicino, ma la loro copertura e' piccola rispetto agli scambi del future, e oltre meta' pesa sulle opzioni che scadono stasera.

## In Parole Semplici

**Il punto**: con la supposizione usata di solito, chi vende le opzioni su NQ oggi fa da acceleratore a ogni prezzo vicino: per coprirsi compra quando sale e vende quando scende. Il peso pero' e' piccolo, e il segno vero non lo conosciamo.

**Dal piu' ampio al piu' vicino**

1. **La paura**: l'indice della paura sul Nasdaq ha chiuso ieri a 38, in salita di meta' in un giorno, dopo un ribasso di circa il 5%.
   → Le opzioni prezzano oggi un movimento di circa 550 punti fino alle 22:00; e' una misura di quanto, non di dove.
2. **I dealer**: con la supposizione solita la loro gamma e' negativa a ogni prezzo entro l'8%, quindi non c'e' un prezzo dove cambino comportamento.
   → In teoria i movimenti si allungano invece di smorzarsi; sui nostri dati di ES questo non si e' ancora visto.
3. **Quanto pesa**: per un movimento dell'1% (circa 270 punti) i dealer comprerebbero o venderebbero circa 900 future.
   → E' meno di 2 su mille dei contratti scambiati in una giornata di cassa: le opzioni non sono il motore del prezzo.
4. **Oggi**: quasi due terzi di quel peso stanno nelle opzioni che scadono alle 22:00, con molte put a 27.000.
   → Dopo la scadenza la gamma di NQ si riduce di molto.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 27.300 | lo strike con piu' call fra le opzioni di oggi | si vede se il rialzo trova vendite delle coperture o le supera |
| circa 27.100 | strike pesante, coincide con il prezzo piu' scambiato della notte | e' gia' qui: una confluenza da confrontare con l'order flow |
| circa 27.000 | lo strike con piu' put, oggi e su tutte le scadenze, molto scambiato anche oggi | si vede se il ribasso accelera oltre o se il prezzo torna su |

**Quanto e' provato**: il peso e la scala sono misurati dalla catena; il segno dei dealer e' una supposizione; nessuno studio prova che questi strike facciano da supporto o resistenza, e su 42 sedute di ES il regime "acceleratore" non ha allargato la giornata.

**Le parole**: **dealer** = chi vende le opzioni e si copre col future; **gamma** = quanto cambia la copertura quando il prezzo si muove; **freno / acceleratore** = dealer che comprano sui ribassi / che inseguono il prezzo; **strike** = il prezzo scritto sull'opzione; **opzioni di oggi (0DTE)** = quelle che scadono stasera; **indice della paura (VXN)** = la volatilita' attesa a 30 giorni sul Nasdaq.

## I Fatti

GEX di `gex.py` (catena CME di NQZ6 via ATAS, 15:02 ora italiana, OI di ieri, 20 $ a punto), in milioni di $ per un movimento dell'1%. La **GEX** e' quanto future i dealer devono scambiare per coprirsi se il prezzo si muove dell'1%. **Convenzione A**: dealer lunghi di call e corti di put (fornitori, ATAS). **Convenzione B** [GPP]: dealer corti di tutto, dice solo dove pesa.

| orizzonte | GEX A | GEX B | zero gamma (+-8%) | call wall | put wall | piu' peso in B |
|---|---|---|---|---|---|---|
| tutte le scadenze nel file (fino al 20/11) | −492 | −1.276 | nessuno | 28.000 | 27.000 | 27.000, 27.500, 27.250 |
| entro sette giorni | −466 | −1.223 | nessuno | 28.000 | 27.000 | idem |
| 0DTE (scade oggi alle 22:00 italiane) | −308 | −912 | nessuno | 27.300 | 27.000 | idem |

Strike pesanti (tutte, OI di ieri e volume del giorno CME, GEX A netta): 27.000 (put 1.144 di OI e 1.157 scambiate oggi, call 137; −78,6), 27.500 (put 822, call 220 con 610 scambiate; −37,5), 27.250 (−33,8), 26.500 (put 804; −33,7), 26.750 (−24,2), 26.800 (−18,9), 27.100 (−18,4). Tutti con piu' put che call.

| scadenza | OI | vol. ATM | mossa attesa | quotate | OI stabile | GEX A |
|---|---|---|---|---|---|---|
| ven 6/11 (0DTE) | 12.362 | 72,2% | 552 | 478/480 | si' | −308 |
| lun 9/11 | 3.427 | 39,1% | 1.007 | 480/480 | si' | −75 |
| mar 10/11 | 1.391 | 40,1% | 1.180 | 462/480 | si' | −37 |
| mer 11/11 | 1.872 | 40,0% | 1.306 | 433/480 | si' | −42 |
| gio 12/11 - gio 19/11 (6 scadenze) | 38-1.452 | 36-41% | 1.426-1.937 | 4-66 su 282-458 | 3 su 6 no | da −12 a −1 |
| ven 20/11 (mensile) | 1.909 | 34,1% | 1.830 | 35/480 | no | −9 |

Saltate: 23/11, 27/11, 30/11, 4/12, 11/12, **18/12 (la trimestrale di NQZ6)**. Nessun calcolo precedente dello stesso giorno (`oi_cambiato` nullo).

Scala (in-breve, calcolata dal collettore): A = 908 future per 1% (271 punti), 0,18% del volume medio di cassa (512.915 contratti, 20 sedute); B = 2.354 future, 0,46%. Realizzata di NQ (log a 5 minuti, annualizzata): 4 ore 28,3%, 24 ore 48,5%, 48 ore 42,0%; 20 sedute 33,8%. VXN ufficiale 38,26 (chiusura 5/11, +52,6% in un giorno); VIX 26,4 alle 14:49 (−6,3%); VIX 9 giorni 32,7 sopra VIX 3 mesi 25,6 (chiusura 5/11).

GEX viva (`opzioni-vive.json`, XrayLive, convenzione A, OI di ieri): **ultimo punto 5/11 18:33 ora italiana, 73.916 secondi prima della raccolta**, future 27.021; totale −1.300 (fino a 70 giorni, trimestrale inclusa), 0DTE del 5/11 −288,5, call wall 30.000, put wall 27.000. Non e' un dato di adesso.

## X-Ray

Contesto dell'S&P 500, non sposta i livelli di NQ. Lettore vivo (giro di 15 secondi fa alle 15:05), nessuna famiglia ferma; lo spot SPX 7.358,14 e' la chiusura di ieri, perche' la cassa apre alle 15:30. Profilo stimato dei market maker: gamma negativa allo spot (−130,9) e su tutto 7.124-7.624, **nessuno zero**, nessun picco di freno; acceleratore piu' forte a 7.611,5-7.619 e 7.184 (osservato), 7.409 e 7.261,5 (previsione per lunedi' 9/11). Per scadenza: 0DTE netto **+16,7** (freno), lunedi' −17,6, 20/11 −23,2: **segnali opposti** fra le opzioni di oggi e il resto. Delta scambiato da stamattina (un flusso, non una posizione): retail −645 sugli 0DTE, firm +591 su tutte. Riserva: stima di un fornitore con metodo non pubblicato, su SPX, unita' sue, non provata.

## Cosa Dicono Per NQ

- **Il regime** (le-opzioni-in-atas, "La GEX calcolata da noi"): in A la GEX e' negativa su tutti e tre gli orizzonti e non ha zeri entro l'8%: **acceleratore** a ogni prezzo vicino, senza un punto dove cambi. Tutte le scadenze e lo 0DTE concordano. In B e' negativa per costruzione; il segno vero non si conosce [GPP].
- **Il regime dice quanto, non dove** (il-bias-del-giorno, ipotesi D): con l'acceleratore il video dice che i livelli tendono a rompersi. **Esito**: su 42 sedute di ES con X-Ray il regime 0DTE delle 9:35 non ha allargato la giornata (0,55 contro 0,74 del movimento atteso, p 0,24). Si rilegge in giornata.
- **Lo 0DTE pesa il 63%** della GEX A del file e scade alle 22:00. E' fatto di put (27.000 la piu' pesante, scambiata oggi piu' del suo OI). In giornata la sua GEX cambia col prezzo e col tempo, non con posizioni nuove: l'OI e' di ieri. Dopo le 22:00 restano circa −185 sulle scadenze del file.
- **La scala**: 908 future per 1% sono lo 0,18% del volume medio di cassa (2.354, 0,46%, nel caso peggiore B). Come il 15 ottobre: su NQ le opzioni sul future sono piccole rispetto al future. La sintesi del 15/10 aggiunge che la GEX e' un tetto: quanto si copra davvero non lo misura nessuno.
- **La mossa attesa**: lo 0DTE prezza 72% annuo, circa 552 punti fino alle 22:00, contro 28% realizzato nelle ultime 4 ore e 48,5% nelle 24. Le scadenze da lunedi' stanno a 39-40%, il 20/11 a 34%: **la paura vicina costa piu' di quella lontana** (struttura invertita, come VIX 9 giorni sopra VIX 3 mesi ieri sera). La volatilita' dello 0DTE e' alta: puo' contenere un evento della giornata (fase 1) o il rumore vicino alla scadenza; **osservazione**.
- **I paper, come ipotesi**. [BDLM]: il momento dell'ultima mezz'ora (21:30-22:00) su NQ c'e' in generale (t 7,97); sui future S&P solo nei giorni di gamma negativa. [BB]: con gamma alta il giorno dopo e' piu' calmo; qui la gamma A e' negativa, ma su indici il segno e' assunto. Sintesi del 15/10: nessun paper trova il verso opposto, l'effetto sul livello della volatilita' e' piccolo [VAPG], quello sulla forma del percorso (continuazione) piu' grande. Nessuno e' su NQ con la nostra catena.

## Le Quattro Domande

- **Regime**: acceleratore su tutte e 0DTE, da `gex.py` delle 15:02. Il file delle quattro domande dice lo stesso, ma dalla GEX viva ferma di ieri (prezzo 27.021). Esito: ipotesi D non regge su ES (sopra).
- **Meteo**: VXN 38,26 (chiusura di ieri), fascia "paura", movimento atteso del giorno circa 650 punti. Implicita meno realizzata **+4,5** (contro 20 sedute): oltre +2, "assicurazione cara". Ma la realizzata di 24 ore (48,5%) supera il VXN: nel breve il mercato si e' mosso piu' del prezzato. Soglie convenzionali, non provate. Skew 8,1, put care (senza ora). Breve contro mese: non calcolato.
- **Muri sul bordo del valore**: il file nomina il call wall 0DTE 26.900 sul POC della cassa di ieri, ma e' **lo 0DTE scaduto del 5/11**: non vale. Con i muri di oggi (`gex.py`), nessuno entro 25 punti dai bordi della cassa di ieri (VAL 26.829, POC 26.900, VAH 27.361): 27.300 dista 61 dal VAH, 27.000 dista 100 dal POC. Ipotesi E: un caso solo, non provata.
- **Grilletto**: l'order flow, non le opzioni; nessun segnale.

## I Livelli E Le Date

Nessun paper prova questi livelli come supporti o resistenze (1-idea, finding 6). Distanze dal bridge, 27.105,75 alle 15:04.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.000 | call wall di tutte (A) | posizionamento lontano (+894): dove le call pesano di piu' |
| 27.500 | strike pesante, secondo in B, call scambiate oggi | +394: posizionamento della settimana |
| 27.300 | call wall dello 0DTE | +194: si osserva se lo 0DTE rallenta o cede, fino alle 22:00 |
| 27.250 | strike pesante (put) | +144 |
| 27.100 | strike pesante, = POC della notte | qui: confluenza gamma-order flow (in-breve, `confluenze`) |
| 27.000 | put wall di tutte e dello 0DTE, primo in B | −106: lo strike piu' pesante; si osserva accettazione o rifiuto |
| 26.500 | strike pesante (put 804) | −606: posizionamento sotto |
| nessuno zero gamma | entro +-8% (25.000-29.300 circa) | in A non c'e' un prezzo vicino dove il regime cambi |
| oggi 15:30 | apertura della cassa | X-Ray riparte con lo spot vero |
| oggi 21:30-22:00 | ultima mezz'ora | finestra di [BDLM] |
| oggi 22:00 | scadenza 0DTE | la gamma di NQ perde circa due terzi |

## Cosa Non Sappiamo

- Il segno vero dei dealer su NQ: nessuna fonte lo misura; A e' una supposizione [GPP].
- Quanto della GEX si copra davvero sul future (sintesi del 15/10: la GEX e' un tetto).
- La volatilita' dello 0DTE a 72%: evento della giornata o rumore vicino alla scadenza.
- lacuna del collettore: la GEX viva e il VXN/VIX ricostruiti sono fermi al 5/11 18:31-18:33 (20,5 ore); il server ripartito alle 15:00 non aveva ancora un punto nuovo alle 15:05.
- lacuna del collettore: `quattro-domande-NQZ6.json` usa la GEX viva senza controllarne l'eta', e prende i muri 0DTE di una scadenza gia' scaduta.
- lacuna del collettore: sei scadenze saltate, fra cui la trimestrale del 18/12; "tutte" vuol dire fino al 20/11.
- lacuna del collettore: dal 12/11 in poi poche opzioni quotate (4-66 su 282-480) e OI non stabile in 4 scadenze su 11.
- lacuna del collettore: il meteo non ha l'ora delle sue misure, e la struttura breve contro mese e' nulla.
- lacuna del collettore: nessun confronto con un calcolo precedente dello stesso giorno (`oi_cambiato` nullo).

## Per Il Quadro

- **Gli strike pesanti** 27.000, 27.100, 27.250, 27.500 e 26.500: da confrontare con il valore della notte (VAL 26.917, POC 27.100, VAH 27.191), con la London (27.086-27.184) e con i livelli del COT. 27.100 coincide gia' con il POC della notte.
- **I muri vicino al valore, con il regime**: con i muri di oggi nessuno sta entro 25 punti da un bordo della cassa di ieri; il piu' vicino e' 27.300 a 61 punti dal VAH 27.361, in regime acceleratore (ipotesi E, non provata).
- **Il regime e la paura**: acceleratore con peso dello 0,18% del volume, paura vicina sopra quella lontana. E' coerente con la macro di ieri (VXN +52% in un giorno) e con un giorno dopo un ribasso del 5%?
- **Dopo le 22:00**: la gamma che resta e' circa un terzo; il quadro di lunedi' parte da una GEX diversa.
