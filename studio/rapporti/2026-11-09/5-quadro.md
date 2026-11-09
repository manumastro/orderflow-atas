# Fase 5 · Il Quadro · lunedi' 9 novembre 2026 · NQZ6 ed ESZ6 insieme · aggiornamento pre-cassa

**Stato**: parziale (order flow senza la tab 4 Range e senza il pannello completo di ES; X-Ray con lo spot di SPX fermo a venerdi' fino alle 15:30; tutto letto prima della cassa). Aggiorna il quadro di mezzogiorno (`precedenti/1452/5-quadro.md`, raccolte delle 12:28).
**Dati**: i quattro rapporti di questo giro, tutti congiunti (`1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`); `dati/confluenze.json` (NQ, soglia 15 punti) e `dati-ES/confluenze.json` (ES, soglia 4 punti), entrambi delle 15:01 e 14:52; `dati/raccolta.json`, `dati/gex-NQZ6.json`. Raccolte: ES 14:52, NQ 14:52 tranne GEX, giro e confluenze (15:01), nessun errore. **L'eta' della GEX di NQ**: `raccolta.json` dice "gex NQZ6 ok, 15:01, 575 secondi", ma dentro `gex-NQZ6.json` il campo `quando` e' 11:54:23 UTC, cioe' **12:54 ora italiana**, con il future a 27.199,25 alle 12:55. Vale `quando`: la GEX di `gex.py` ha **circa 2 ore e 7 minuti** alle 15:01 (2 ore e 36 alla cassa). La fase 3 ha ragione; la discrepanza e' una lacuna del collettore. GEX viva di NQ e X-Ray: 14:50-14:51. COT del 3/11 (6 giorni). VIX 14:37; VXN, VIX9D, VIX3M a venerdi' 22:15. Paper: `quant/paper/ricerche/2026-11-09-opzioni-fra-indici.md`.
Prezzi di riferimento: **NQZ6 27.345,25 alle 14:52, 27.313,50 alle 15:01**; **ESZ6 7.470,25 alle 14:52** (`in-breve.json`, order flow).

**In una riga**: lo sfondo di mezzogiorno resta (sconto sotto i mesi, paura alta, posizioni opposte), ma prima della cassa tutti e due sono usciti sopra la zona di venerdi', NQ ha recuperato il passo di ES e le sue opzioni di oggi sono passate in modalita' freno; su ES no.

## In Parole Semplici

**Il punto**: rispetto a mezzogiorno sono cambiate due cose. Tutti e due i future sono saliti sopra la zona di venerdi', e su NQ le opzioni che scadono oggi ora frenano invece di accelerare. Il resto non cambia: siamo molto sotto il valore dei mesi, la paura e' alta, domani alle 14:30 c'e' l'inflazione. Vicino al prezzo un solo punto e' indicato da due studi (NQ circa 27.250), ed e' debole.

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: la Fed e' attesa alzare i tassi a dicembre; Nvidia perde il 5% per una notizia sua, eppure NQ ed ES salgono uguale, circa +0,8% su venerdi'.
   → Nel mese NQ soffre il doppio di ES (circa -12% contro -5%); stamattina gli altri grandi del Nasdaq compensano Nvidia.
2. **Le posizioni** (fotografia di martedi' 3): sul Nasdaq gli speculatori sono carichi al rialzo, sull'S&P al ribasso; le banche vendono su tutti e due.
   → Divergono: lo stesso calo pesa sui primi e premia i secondi. Il rimbalzo accorcia la distanza, il crollo di giovedi' 5 non e' ancora nel rapporto.
3. **Le opzioni**: di fondo tutti e due sono in modalita' acceleratore, lontani dal cambio; ma NQ e' salito sopra circa 27.230, dove le sue opzioni di oggi passano a freno, e su ES quelle di oggi restano quasi a zero.
   → Concordano sul fondo, si separano sull'oggi: su NQ i segnali sono opposti. Dice quanto si puo' muovere il prezzo, non dove.
4. **Le settimane**: tutti e due sono sotto la zona di scambio dei tre mesi; l'ultima settimana ha sceso uno scalino su entrambi.
   → Niente di nuovo: siamo in basso nella curva, NQ piu' in fondo di ES.
5. **I giorni**: giovedi' ha rotto verso il basso, venerdi' si e' fermato dentro giovedi'; oggi tutti e due stanno sopra la zona di venerdi', ancora dentro quella di giovedi'.
   → La pausa si e' rotta verso l'alto prima della cassa. NQ ha gia' toccato il bordo alto di giovedi' ed e' rientrato; ES ha piu' spazio.
6. **Adesso** (14:52-15:01): ES e' appena sopra il massimo di venerdi', NQ circa 80 punti sotto il suo; dalla chiusura di venerdi' salgono quasi uguale (circa +1%); l'indicatore dice "in bilancia", poi "a zig-zag" su NQ.
   → La guida non e' piu' netta: NQ ha corso di piu', ES ha la struttura migliore. Prima della cassa il mercato si e' fermato.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ circa 27.230-27.250 | dove le opzioni NQ di oggi passano da freno ad acceleratore; vicino, il bordo alto del valore di stanotte | sotto, il regime di oggi di NQ torna acceleratore e va riletto |
| NQ circa 27.306 · ES circa 7.454,50 | il bordo alto della zona di venerdi' | rientrarci sotto vuol dire che l'uscita di stamattina non ha tenuto |
| NQ 27.425 · ES 7.469 | il massimo di venerdi' (ES ci sta sopra di 3 punti) | una chiusura di giornata oltre rompe il ping-pong dopo lo scalino |
| NQ 27.361 · ES 7.537,75 | il bordo alto della zona di giovedi', il giorno della rottura | sopra si entra in una zona vuota fino al valore di martedi' 3 |

**Quando**: 15:30 apre la cassa e X-Ray riparte con il prezzo vero · 21:30-22:00 l'ultima mezz'ora · 22:00 scadono le opzioni di oggi · domani 10:00 Waller (Fed), 14:30 inflazione di ottobre · venerdi' 13/11 21:30 il nuovo rapporto sulle posizioni.
**Cambia idea se**: dopo le 15:30 X-Ray mostra su ES un punto di cambio vicino o la gamma al prezzo positiva; NQ torna sotto circa 27.230; tutti e due rientrano sotto il bordo alto di venerdi'; ES torna sotto 7.469 mentre NQ resta sopra 27.319 (la guida passa a NQ); in cassa i chip seguono Nvidia invece degli altri grandi.
**Quanto e' provato**: prezzi, volumi e posizioni sono misurati. Cosa fanno i venditori di opzioni e' un modello: su NQ il segno e' supposto, su ES lo stima un fornitore. Su 42 sedute di ES l'acceleratore del mattino non ha allargato la giornata. Che un prezzo "tenga", o che le opzioni di un indice informino l'altro, nessuno studio lo prova.

**Le parole**: **cassa** = la seduta di Wall Street, 15:30-22:00 italiane; **valore** = la fascia dove passa il 70% del volume; **a sconto** = sotto il valore dei mesi; **acceleratore / freno** = chi vende le opzioni insegue il prezzo / lo contrasta; **opzioni di oggi** = quelle che scadono stasera; **scalino** = candela con massimo, minimo e chiusura oltre la precedente; **ping-pong** = il prezzo che va fra i due estremi di venerdi'.

## Cosa E' Cambiato Da Mezzogiorno

Le condizioni "cambia idea se" e "cosa smentirebbe" del quadro delle 12:28, una per una.

| condizione delle 12:28 | esito alle 14:52-15:01 |
|---|---|
| NQ sale sopra 27.235 e ci resta: il regime 0DTE di NQ gira | **avverata**: GEX viva 0DTE da -22,0 a +58,5 (14:51), prezzo 0,44% sopra lo zero 27.230,3; l'open interest e' di ieri, l'hanno girata prezzo e tempo (fase 3) |
| uno dei due esce dalla zona di venerdi' e ci resta | **avverata su tutti e due**, per ora: NQ sopra il VAH 27.306 di 7 punti (15:01), ES di 17,25 (14:52). "Fermi nella pausa" e' falso |
| NQ supera 27.319 mentre ES resta sotto 7.459 | **non come scritta**: NQ e' salito a 27.378, ma anche ES a 7.476,50. Salgono insieme; la guida di ES si e' ridotta (fase 4) |
| X-Ray su ES con punto di cambio vicino o gamma positiva | **non ancora leggibile**: spot di SPX fermo fino alle 15:30; gamma al prezzo -163,7 → -151,2, 0DTE -8,1 → -2,0 |
| chiusura oltre i muri del ping-pong | **non verificabile prima della chiusura**: ES e' sopra 7.469 di 2,75 in giornata, NQ 80 sotto 27.425 |
| cassa con efficienza alta | da leggere dopo le 15:30 (BALANCE 0,17-0,18 alle 14:52, NQ CHOPPY 0,03 alle 15:01) |
| CPI sotto le attese; COT del 13/11 | future (domani 14:30; venerdi' 21:30) |

Altri cambiamenti: il rialzo Fed dai future passa da 82,7% a 96,4% per **un tick** del future di gennaio (fase 1); l'S&P non e' piu' "caro" nelle opzioni (implicita meno realizzata da +2,2 a +1,7) e NQ scende a -2,3 (fase 3); la ricerca sulle opzioni fra indici ora c'e'.

## Il Quadro In Cinque Righe

1. **Macro**: rialzo FOMC 9/12 al 96,4% (future ZQ, 14:39; un tick vale circa 14 punti), Polymarket 76,5%; 10 anni 4,845 (14:37); VIX 23,10 (14:37, 88 pct), VXN 34,02 (venerdi', 99 pct); NVDA -5,23%, Nasdaq-100 79 su 96 in salita, media pesata +0,99%; NQ=F ed ES=F +0,84% (14:42, Yahoo).
2. **Posizionamento**: COT 3/11 (6 giorni). Nasdaq NC netto +42.873 (90 pct a 3 anni), corti al 14,7 pct, var. netto -5.966; S&P NC -129.414 (36 pct), +4.954. TFF Dealer Nasdaq -27.015 (z -1,84), S&P -48.981 (z -1,05). Dal 3/11 NQ -3,38%, ES -1,66% (14:52).
3. **Gamma**: NQ convenzione A (segno supposto). `gex.py` 12:54: tutte -493,5 M$ per 1%, zero 28.879. Viva 14:51: tutte -425,7, zero 28.775,6 (+5,2%); 0DTE +58,5, zero 27.230,3 (freno). ES, X-Ray 14:50 su spot di venerdi': gamma al prezzo -151,2, nessun flip fra -3,9% e +2,8%, acceleratore piu' forte 7.346 di ES, 0DTE -2,0, mensile entro l'1% +13,5. Implicita meno realizzata: NQ -2,3, ES +1,7.
4. **Valore**: composito di cassa 90 NQ 28.998-31.080 (47 sedute), ES 7.676-7.862 (26): discount. Venerdi' NQ 27.113-27.306, ES 7.415,75-7.454,50; giovedi' NQ 26.829-27.361, ES 7.388,25-7.537,75. Vuoto NQ 27.361-28.296, ES 7.537,75-7.589,75.
5. **Adesso**: NQ 27.345,25 (14:52) e 27.313,50 (15:01), sopra il VAH di venerdi' di 7; ES 7.470,25, sopra il VAH di 17,25 e il massimo di venerdi' di 2,75. Notte NQ 26.751,25-27.378, POC 27.100; ES 7.363,75-7.476,50, POC 7.446. BALANCE 0,17 e 0,18 (14:23-14:52); NQ CHOPPY 0,03 (14:32-15:01). CVD di NQ +3.820 (15:01); ES non nel pannello.

## Gli Agganci, Uno Per Uno

| fase | aggancio | esito |
|---|---|---|
| 1 | NQ ha raggiunto ES con Nvidia -5% | **conferma**: fase 4, dalla chiusura di venerdi' NQ +1,0% contro ES +0,949% (13:52 UTC). **Ma** la struttura di ES resta avanti (sopra il massimo di venerdi', minimi orari crescenti) |
| 1 | Gamma sopra i massimi 27.378 / 7.476,50 | **su NQ**: il muro piu' vicino e' 27.500 (122 punti sopra), in zona dove lo 0DTE frena. **Su ES niente da dire in prezzi di ES**: gli strike 0DTE pesanti di X-Ray (7.490, 7.505 di SPX) non sono nel collettore in prezzi di ES (lacuna) |
| 1 | Folla lunga sul Nasdaq che "compra il ribasso", Fed e paura estrema | **niente da dire con misura**: il COT si ferma al 3/11 e vede i corti risalire (3,8 → 14,7 pct). Chi compra oggi lo dira' il report del 13/11 |
| 1 | Implicita molto sopra la realizzata | **contraddizione** con la fase 3 su NQ (la 2 sotto); **conferma** sulla struttura inversa: la scadenza NQ del 10/11 (43,5%) contiene il CPI |
| 2 | Posizionamento contro il regime | **niente da dire con misura** fino al 13/11 |
| 2 | Livelli del passo 6 contro muri e zero | **nessuna vicinanza entro soglia**: zero entro 7 giorni di NQ 28.343 (12:54) a 29 punti dal POC del 3/11 (28.314); su ES X-Ray non entra nelle confluenze (lacuna) |
| 2 | Forza relativa dal 3/11 (circa il doppio) | **conferma sulla scala del mese** (fase 1: -11,6% contro -5,0% a 20 sedute); **non oggi**, dove salgono pari |
| 3 | Strike pesanti di NQ contro COT e valore | **nessuno entro 15 punti** fra 26.500, 27.000, 27.500. Lo zero 0DTE 27.230 sta dentro il valore di venerdi' e coincide con il minimo dell'ora 14 di NQ (27.230, fase 1): un caso solo, e lo zero si sposta col tempo |
| 3 | Muri sul bordo del valore, col regime | **parziale**: la fase 3 non ne trova; il collettore trova lo strike 27.250 accanto al VAH della notte 27.248 (vedi muri e contraddizione 5) |
| 3 | Regimi di fondo concordi, 0DTE no | **preso**: si rilegge dopo le 15:30 |
| 3 | Volatilita' contro macro e scalino | **conferma** sulla struttura e sullo scalino giu' (fase 4, tutti e due); **contraddizione** sul livello (la 2) |
| 4 | Chi guida contro GEX viva e X-Ray | **coerente, descrittivo**: NQ e' salito nella zona dove le sue opzioni di oggi frenano; ES sta 124 punti sopra l'acceleratore piu' forte di X-Ray, lontano |
| 4 | VAH della notte 27.248 e strike 27.250 | **confermata dal collettore** (`stesso_dato` falso), ma debole: lo strike non e' fra i pesanti della fase 3 e il pannello chiama "VAH notte" 27.189 |
| 4 | BALANCE/CHOPPY contro acceleratore | **non si sommano**. Su NQ lo zig-zag sta accanto allo 0DTE in freno; su ES la bilancia accanto all'acceleratore va col risultato delle 42 sedute, non col video |
| 4 | Delta di ES positivo anche stanotte (+6.790), contro COT e X-Ray | **niente da dire con misura**: il COT si ferma al 3/11; X-Ray dice retail che compra delta su tutte le scadenze (+3.452,6) e firm che vendono (-1.060,2), ma e' delta su SPX, non chi compra ES. Il prezzo di ES, per la prima volta in tre sedute, segue il delta |

## I Livelli Confermati

Entra un prezzo indicato da almeno due fasi entro `soglia_punti`: **15 punti su NQ, 4 su ES**. Le vicinanze vengono dal collettore (15:01 NQ, 14:52 ES) e sono verificate contro i rapporti. COT e order flow leggono lo stesso bridge con costruzioni diverse: una confluenza fra loro e' meno di due fonti indipendenti. **Nessuna e' validata come supporto o resistenza.** Distanze dai prezzi delle 14:52.

| strumento | prezzo | quali fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|---|
| NQ | 27.248-27.250 | order flow, gamma | VAH della notte (`oggi_notte`); strike 27.250, GEX netta A -20,8 (`gex.py` 12:54) | posizionamento: il bordo alto del valore di stanotte, accanto allo zero 0DTE (27.230, 18 punti, fuori soglia). Circa -95 punti |
| NQ | 28.879-28.891 | gamma, order flow | zero di tutte, A, `gex.py` 12:54; VAH di cassa del 2/11 | dove il regime supposto di fondo girerebbe. **Fragile**: la GEX viva lo da' a 28.775,6, 103 punti sotto. +1.535 |
| NQ | 29.482-29.495 | COT, order flow | VAH della settimana del report 3/11; VAL 29/10, POC 26/10 | posizionamento, settimane. +2.140 |
| NQ | 30.173-30.178 | COT, order flow | VAH settimana del report 20/10; POC 14/10 | posizionamento, settimane. +2.830 |
| NQ | 30.890 | COT, order flow | POC settimana del report 13/10; POC 12/10 | la settimana del movimento piu' grande degli speculatori. +3.545 |
| ES | 7.649,75-7.653,50 | COT, order flow | VAL settimana del report 27/10; VAL 23/10 | posizionamento, settimane. +180 |
| ES | 7.690 | COT, order flow | POC settimana del report 27/10; POC 22/10 | posizionamento, settimane. +220 |
| ES | 7.733-7.734,75 | COT, order flow | VAL settimana del report 20/10; VAL 19/10 | posizionamento, settimane. +263 |
| ES | 7.750-7.754 | COT, order flow | POC settimana del report 3/11; POC dei compositi 20 e 90; VAH settimana 27/10 | la piu' fitta: centro della settimana dei report e del valore dei mesi. +280 |
| ES | 7.790,50-7.794,25 | COT, order flow | VAH settimana del report 20/10 e 3/11; POC 15/10 | posizionamento, settimane. +320 |
| ES | 7.860-7.863 | COT, order flow | VAH settimana del report 13/10; VAH del composito 90 | bordo alto del valore dei mesi. +390 |

**Vicino al prezzo, solo NQ 27.248-27.250 passa il filtro.** Su ES nessun livello entro 100 punti.
**Non contate, `stesso_dato`**: NQ 28.296 / 28.314 / 28.387 (valore del 3/11) e i valori delle sedute del 13, 20 e 27/10; ES 7.589,75-7.610,25 (3/11), 7.740,25-7.743,25, 7.754,50-7.758,50, 7.772,25-7.776,25, 7.843,50-7.856,75. Dalla macro, stesso dato del bridge dell'order flow: NQ 27.378, 27.319, 26.751,25; ES 7.476,50, 7.363,75.

## I Muri Delle Opzioni Sul Bordo Del Valore, Col Regime

Regime da `confluenze.json`: **NQ segnali opposti** (acceleratore su tutte le scadenze, freno sullo 0DTE, che pesa di piu' verso la chiusura); **ES acceleratore** al prezzo e sullo 0DTE (X-Ray su spot di venerdi').

- **NQ 27.250 accanto al VAH della notte 27.248**, con lo 0DTE in freno. E' uno strike leggero (GEX -20,8, contro -76,6 di 26.500), non un muro della fase 3.
- **ES sul muro alto del ping-pong (7.469)** in acceleratore stimato. Il ping-pong viene dal prezzo, non dalle opzioni: non e' un muro di gamma. Gli strike pesanti di X-Ray vicini non sono in prezzi di ES nel collettore.
- Fuori soglia: NQ 27.500 a 75 punti sopra il muro alto del ping-pong (27.425); ES 7.346 (acceleratore piu' forte) a 23,75 sopra il muro basso (7.322,25).
- **Ipotesi del video, non provata**: col freno il muro tiene, con l'acceleratore si rompe ([il bias del giorno](../../../docs/research/metodo/il-bias-del-giorno.md), ipotesi D ed E). Su 42 sedute di ES il verso misurato e' l'opposto (0,55 contro 0,74 del movimento atteso, p 0,24).

## Le Contraddizioni

1. **Chi guida.** Nel mese NQ scende circa il doppio (fasi 1, 2); a mezzogiorno guidava ES; ora dalla chiusura di venerdi' NQ fa +1,0% contro +0,949% (fase 4), ma solo ES e' sopra il massimo di venerdi'. Delta in proporzione piu' forte su ES (fase 1). Scale diverse, nessuna e' la "vera".
2. **Il costo della paura su NQ.** Fase 1: le opzioni costano molto piu' del movimento di stamattina (VXN 34 di venerdi' contro 21,5 di realizzata a 4 ore). Fase 3: NQ si muove piu' del prezzato (VXN ricostruito 32,1 contro 34,4 a 20 sedute, -2,3). Cambiano finestra e indice. Su ES concordano piu' o meno (in linea o caro).
3. **Il regime delle scadenze di ES.** Acceleratore al prezzo e sullo 0DTE, ma la mensile del 20/11 entro l'1% e' di segno freno (+13,5), sullo spot fermo di venerdi', quasi l'1% sotto il prezzo implicito.
4. **Due GEX di NQ.** `gex.py` 12:54: -493,5, zero 28.879; viva 14:51: -425,7, zero 28.775,6. La confluenza a 28.879 esiste solo con la prima, vecchia di due ore.
5. **Il VAH della notte di NQ.** `oggi_notte` 27.248, pannello 27.189: finestre diverse. La sola confluenza vicina al prezzo regge sulla prima. La fase 3 (domanda 3) non la vede perche' guarda i bordi delle sedute e del ping-pong.
6. **Legacy e TFF sul Nasdaq.** Speculatori in riduzione (-5.966) nella Legacy; gestori +7.756 e fondi speculativi +7.772 nella TFF. Tassonomie diverse: non si fondono.
7. **Nvidia contro l'indice.** NVDA -5,23% e NQ +1%: la fase 1 lo spiega con gli altri grandi (+2-3%), come osservazione di una mattina con un'ampiezza che mescola il pre-mercato.
8. **Il "cash" prima della cassa.** Il pannello di NQ chiama "max cash" 27.368,75 e "min cash" 27.268 una finestra che inizia prima delle 15:30. Si scrive, non si usa.

## NQ Ed ES Insieme

**Dove si confermano**: regime di fondo delle opzioni (acceleratore, cambio lontano); posizione rispetto al valore (a sconto sotto i mesi, sopra la zona di venerdi', dentro quella di giovedi'); scalini (settimana giu', ping-pong di una giornata); verso del delta orario e della notte (positivo su tutti e due, ore 13 e 14); ritmo alle 14:52 (BALANCE, 0,17 e 0,18); banche in vendita nel COT; paura alta, put care, struttura inversa; passo di oggi (circa +1% dalla chiusura).

**Dove no**: lo 0DTE (NQ freno, ES quasi zero in acceleratore); il massimo di venerdi' (ES sopra, NQ 80 sotto); la struttura oraria (ES massimi e minimi crescenti, NQ che si allarga) e il POC della notte (ES salito a 7.446, NQ fermo a 27.100); gli speculatori nel COT (Nasdaq lunghi, S&P corti); il costo della paura (NQ sotto la realizzata a 20 sedute, ES in linea); la profondita' nel mese.

**Chi guida**: nel mese NQ; stamattina nessuno in modo netto (NQ il passo, ES la struttura). Nessuno studio prova che una divergenza fra i due dica il verso.

**Il passaggio di informazione fra le opzioni dei due indici e' un'ipotesi non provata.** La ricerca del 9/11 non trova paper su gamma, muri o zeri fra indici. I due letti e verificati parlano di flusso, non di opzioni: **Hasbrouck 2003** (versione di lavoro 2000) trova la scoperta del prezzo sugli E-mini, ma stima S&P 500 e Nasdaq-100 separatamente, in secondi; **Le Coz e altri 2024** trovano che la pressione degli ordini nasce sullo strumento piu' liquido e muove i correlati nella stessa finestra, senza prevedere, e non nominano il Nasdaq-100. **Chordia e altri 2021** (flusso di opzioni di altri indici, Nasdaq-100 compreso, sull'S&P, a una settimana) e **Holowczak e altri 2014** (come si somma il flusso, su QQQ) non sono ancora scaricati: SSRN dietro Cloudflare, serve la persona. Qui regimi, segni, flussi e volatilita' stanno accanto; i prezzi non si convertono.

## Gli Orari Di Oggi

Italia e New York in ora solare: cassa 09:30-16:00 di New York = 15:30-22:00 italiane (14:30-21:00 UTC).

| ora italiana | evento | cosa puo' cambiare del quadro |
|---|---|---|
| 15:30 | apre la cassa; X-Ray riparte con lo spot vero | si rilegge il regime di ES (0DTE, mensile); nasce il valore di oggi; si vede se l'uscita sopra venerdi' tiene |
| 21:30-22:00 | ultima mezz'ora | finestra di BDLM: momento con gamma negativa sui future S&P; su NQ la condizione non e' provata |
| 22:00 | chiude la cassa, scadono le opzioni di oggi | lo zero 0DTE di NQ (27.230) sparisce; la chiusura dice se il ping-pong e' rotto |
| 10/11 10:00 e 14:30 | Waller; CPI e core di ottobre (attese 0,3% e 0,2%) | l'evento della settimana; dentro la scadenza NQ del 10/11 (43,5%) |
| 13/11 14:30 e 21:30 | PPI; COT con i dati del 10/11 | settimanale NQ piu' pesante; il COT vede il 5/11 e supera la fase 2 |
| 17/11 sera; 20/11 15:15 | conti Nvidia; mensile SPX del mattino | Nvidia pesa il 10,7% del Nasdaq-100 |

## Cosa Smentirebbe Il Quadro

- Dopo le 15:30 X-Ray su ES mostra la gamma al prezzo positiva o un punto di cambio vicino: i regimi di fondo non concordano piu'.
- NQ torna sotto 27.230 e ci resta: lo 0DTE torna acceleratore e i "segnali opposti" finiscono.
- Tutti e due rientrano sotto il VAH di venerdi' (NQ 27.306, ES 7.454,50): l'uscita dalla pausa non ha tenuto.
- ES torna sotto 7.469 mentre NQ resta sopra 27.319: la guida passa a NQ.
- Una chiusura oltre i muri del ping-pong (NQ 27.425 / 26.366,50; ES 7.469 / 7.322,25): lo scalino giu' smette di essere l'ultimo fatto.
- In cassa SMH segue Nvidia e non AVGO: cade il "solo Nvidia" della fase 1.
- Il CPI sotto le attese e il rialzo di dicembre che perde probabilita'; il COT del 13/11 con gli speculatori del Nasdaq che tolgono molto rialzo.

## Cosa Non Sappiamo

- Tutto e' letto prima della cassa: valore di oggi, X-Ray con spot vero e tab 4 mancano.
- Il segno dei dealer di NQ e' supposto; quello di ES e' stimato da OptionsDepth con metodo non pubblicato.
- Il COT ha 6 giorni e non vede il 4-6/11; nessuna soglia COT e' validata.
- Nessun livello e' provato come supporto o resistenza; l'ipotesi D e' smentita nel verso su ES, la E non e' provata.

## Le Lacune Del Collettore

1. `raccolta.json` segna "gex NQZ6 ok, 15:01, 575 s", ma `gex-NQZ6.json` ha `quando` 11:54:23 UTC (12:54): il ricalcolo non ha scritto un file nuovo, o il passo riporta l'ora sbagliata. La GEX di `gex.py` non e' della raccolta delle 15:01 (fasi 3 e 5).
2. `dati-ES/confluenze.json` non contiene i livelli di X-Ray in prezzi di ES: le vicinanze gamma-valore e gamma-COT su ES non sono misurate (fase 5).
3. La tab 4 Range 40 non e' registrata nel bridge; `barre_range` vuoto su tutti e due (fase 4).
4. Il pannello di ES non ha livelli, CVD, valore London ne' livello in gioco (fase 4).
5. I giri di NQ (15:01) ed ES (14:52) non sono allo stesso minuto (fase 4).
6. Il pannello di NQ chiama "cash" una finestra che inizia prima della cassa, senza dire da quando (fase 4).
7. Il composito di cassa 90 non e' pieno (NQ 47 sedute, 6 prima del rollover; ES 26) (fase 4).
8. La scala di ES (GEX in $ contro il volume) non c'e' dal 5/11; manca la variazione nel tempo di X-Ray; il VIX del meteo di ES non dichiara la fonte (fase 3).
9. VXN, VIX9D, VIX3M fermi a venerdi' 22:15; 2YY=F fermo al 13/10; la probabilita' FOMC non porta l'incertezza di un tick; l'ampiezza del Nasdaq-100 ha l'etichetta "Nov 6" con valori di oggi; `ore-*.json` senza venerdi'; mancano ampiezza per titolo e pesi dell'S&P (fase 1).
10. COT: manca `prima_del_rollover`; percentili a tre anni solo per l'ultimo report; manca lo z52 del livello del netto, l'ora di pubblicazione e la distanza dal composito a 20 sedute (fase 2).

## Per Il Giro

Il blocco QUADRO e' in `docs/research/giornate/NQZ6-2026-11-09.md` e `docs/research/giornate/ESZ6-2026-11-09.md`. Il quadro si rifa dopo le 15:30 se X-Ray cambia il regime di ES, se NQ torna sotto 27.230, se i due rientrano sotto venerdi', e venerdi' 13/11 dopo le 21:30.
