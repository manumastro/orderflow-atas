# Fase 5 · Il Quadro · lunedi' 9 novembre 2026 · NQZ6 ed ESZ6 insieme

**Stato**: parziale (order flow senza la tab 4 Range e senza il pannello completo di ES; X-Ray con lo spot di SPX fermo a venerdi' fino alle 15:30; la ricerca sulle opzioni fra indici non c'e' ancora; tutto letto prima della cassa).
**Dati**: i quattro rapporti di oggi (`1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`, tutti congiunti); `dati/confluenze.json` (NQ, soglia 15 punti) e `dati-ES/confluenze.json` (ES, soglia 4 punti), `dati-ES/in-breve.json` (X-Ray in prezzi di ES), `dati/gex-NQZ6.json`. Raccolte delle 12:28 (NQ) e 12:29 (ES) ora italiana, nessun errore (`raccolta.json`). Freschezza: COT di martedi' 3/11 (6 giorni), uscito venerdi' 6/11 alle 21:30; GEX di NQ 12:22 (`gex.py`) e 12:27 (viva); X-Ray 12:26 con lo spot di SPX della chiusura di venerdi' (7.376,06) e la base ES-SPX del 6/11 (23,5); VIX 12:13, VXN e VIX9D/VIX3M a venerdi' 22:15. Prezzi di riferimento alle 12:28: **NQZ6 27.211,50**, **ESZ6 7.451,00** (`in-breve.json`, order flow). `quant/paper/ricerche/2026-11-09-opzioni-fra-indici.md` non esiste ancora.

**In una riga**: tutti e due i future stanno a sconto sotto il valore dei mesi, fermi dentro la zona di venerdi', con le opzioni stimate in modalita' acceleratore e la paura alta; li separano le posizioni (speculatori lunghi sul Nasdaq, corti sull'S&P), il delta di venerdi' e la notte, dove ES ha tenuto e NQ no.

## In Parole Semplici

**Il punto**: NQ ed ES sono nello stesso posto della curva: molto sotto la zona dove si e' scambiato negli ultimi mesi, e fermi nella zona di venerdi', in attesa dell'inflazione di domani alle 14:30. Il mondo pesa di piu' su NQ (i chip), ma stanotte e' ES quello che ha tenuto meglio. Vicino al prezzo nessun livello e' indicato da due studi insieme.

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: la Fed e' attesa alzare i tassi a dicembre (circa 8 su 10), la paura e' alta su tutti e due, quasi al massimo dell'anno sul Nasdaq; in un mese NQ ha perso circa il 12%, ES circa il 5%.
   → Lo sfondo e' lo stesso, ma NQ soffre il doppio: lo guidano i chip, mentre dentro l'S&P salgono i settori lontani dalla tecnologia.
2. **Le posizioni** (fotografia di martedi' 3): sul Nasdaq gli speculatori sono carichi al rialzo, sull'S&P al ribasso; le banche vendono su tutti e due.
   → Qui i due divergono: lo stesso calo fa male a chi e' carico sul Nasdaq e premia chi e' carico sull'S&P. Il crollo di giovedi' non e' ancora nel rapporto.
3. **Le opzioni**: su tutti e due chi vende le opzioni sarebbe, nelle stime, nella modalita' che segue il prezzo invece di frenarlo, e lontano dal cambiarla.
   → Concordano: in teoria giornate piu' larghe, non una direzione. Divergono sulle opzioni di oggi: su NQ basta salire di circa 25 punti (dal prezzo delle 12:28) per girarle, su ES no.
4. **Le settimane**: tutti e due sono sotto la zona di scambio di tre mesi; la settimana scorsa ha sceso uno scalino, su entrambi.
   → Siamo in basso nella curva, NQ piu' in fondo: solo NQ ha chiuso ottobre sotto il minimo di settembre.
5. **I giorni**: giovedi' ha rotto verso il basso, venerdi' si e' fermato dentro giovedi'. Su ES chi comprava aggressivo ha spinto molto senza ottenere il prezzo; su NQ hanno spinto i venditori.
   → Una pausa dopo la rottura, su tutti e due; il segno della spinta li separa.
6. **Adesso** (12:28): tutti e due dentro la zona di venerdi', sopra il suo centro; alle 08 NQ ha fatto un minimo nuovo, ES no; l'indicatore dice "a zig-zag" su entrambi.
   → Stanotte ES guida e NQ segue: e' un'osservazione di una notte, non una regola.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ circa 27.306 · ES circa 7.454,50 | il bordo alto della zona di venerdi' (ES ci e' gia' a 3,5 punti) | accettare sopra vuol dire uscire dalla pausa verso l'alto |
| NQ circa 27.235 | dove, nel modello, le opzioni di NQ che scadono oggi passano da acceleratore a freno | su NQ il regime di oggi va riletto; su ES non c'e' un punto cosi' |
| NQ circa 27.113 · ES circa 7.415,75 | il bordo basso della zona di venerdi' | sotto, si torna fuori dalla pausa verso il basso |
| NQ circa 26.900 · ES circa 7.406 | il prezzo piu' scambiato di giovedi', il giorno della rottura | il primo punto che il mercato ritrova scendendo |

**Quando**: oggi 15:30 apre la cassa di New York (X-Ray riparte con il prezzo vero) · 21:30-22:00 l'ultima mezz'ora · 22:00 chiude la cassa e scadono le opzioni di oggi · domani 10:00 Waller (Fed) · domani 14:30 l'inflazione di ottobre (CPI) · venerdi' 13/11 21:30 il nuovo rapporto sulle posizioni.
**Cambia idea se**: dopo le 15:30 X-Ray mostra su ES un punto di cambio vicino o la gamma al prezzo positiva; NQ supera il suo massimo della notte (27.319) mentre ES resta sotto il suo (7.459); uno dei due esce dalla zona di venerdi' e ci resta; venerdi' gli speculatori del Nasdaq risultano aver tolto molto rialzo.
**Quanto e' provato**: prezzi, volumi, posizioni e probabilita' della Fed sono misurati; il comportamento di chi vende le opzioni e' una stima (su NQ supposto, su ES di un fornitore); su 42 sedute di ES la modalita' acceleratore non ha allargato la giornata; che un prezzo "tenga", o che le opzioni di un indice informino su quelle dell'altro, nessuno studio lo prova.

**Le parole**: **cassa** = la seduta di Wall Street, 15:30-22:00 italiane; **valore** = la fascia dove passa il 70% del volume; **a sconto** = sotto il valore dei mesi; **scalino** = candela con massimo, minimo e chiusura oltre la precedente; **speculatori** = chi nel rapporto CFTC opera sul prezzo; **acceleratore / freno** = chi vende opzioni insegue il prezzo / lo contrasta; **delta** = acquisti aggressivi meno vendite aggressive; **a zig-zag** (choppy) = molta strada, poco cammino.

## Il Quadro In Cinque Righe

1. **Macro**: Fed +25 punti base il 9/12 all'82,7% (future ZQ 12:17); 2 anni 4,51, 10 anni 4,86, -0,36 in 20 sedute (Tesoro 6/11); VXN 34,02 (99 pct, venerdi'), VIX 23,56 (89 pct, 12:13); VIX9D > VIX > VIX3M venerdi'; spread high yield 3,48 (95 pct); 20 sedute NQ=F -11,87%, ES=F -5,15%, SMH -23,1%, RSP +6,97 su SPY (Yahoo).
2. **Posizionamento**: COT 3/11 (6 giorni). Nasdaq NC netto +42.873 (90 pct a 3 anni), corti 3,8 → 14,7 pct, var. netto -5.966; S&P NC netto -129.414 (36 pct), +4.954. TFF Dealer: Nasdaq -27.015 (z -1,84, corti al 100 pct), S&P -48.981 (z -1,05). Dal 3/11 NQ -3,88%, ES -1,93%; tutti i prezzi del COT stanno sopra.
3. **Gamma**: NQ convenzione A (segno supposto), `gex.py` 12:22: tutte -451,6 M$ per 1%, zero 28.862 (+6,2%); 0DTE -20,0, zero 27.235 (+0,17%); muri 27.000 e 27.500; scala 0,16% del volume medio. ES X-Ray 12:26 (spot di venerdi'): gamma al prezzo -163,7, nessun flip fra -3,6% e +3,1%, acceleratore piu' forte 7.343,5 di ES. Implicita meno realizzata: NQ -1,7, ES +2,2.
4. **Valore**: composito di cassa 90, NQ 28.998-31.080 (47 sedute), ES 7.676-7.862 (26 sedute): discount su tutti e due. Venerdi' NQ 27.113-27.306 POC 27.175, ES 7.415,75-7.454,50 POC 7.426, dentro giovedi' (NQ 26.829-27.361 POC 26.900; ES 7.388,25-7.537,75 POC 7.406). Vuoto NQ 27.361-28.296, ES 7.537,75-7.589,75.
5. **Adesso** (12:28): NQ 27.211,50, ES 7.451,00, dentro il valore di venerdi' e sopra il POC. Notte NQ 26.751,25-27.319, POC 27.100; ES 7.363,75-7.459, POC 7.428,50. CHOPPY su tutti e due (efficienza 0,09 e 0,10 su 30 barre 11:59-12:28); CVD di NQ +2.395 dalla chiusura (ES non nel pannello).

## Gli Agganci, Uno Per Uno

| fase | aggancio | esito |
|---|---|---|
| 1 | Rialzo Fed all'82,7% e paura estrema contro speculatori Nasdaq lunghi al 90 pct | **conferma, come racconto**: la fase 2 vede la folla lunga in riduzione e i corti risalire dal 3,8 al 14,7 pct. Ma il report si ferma al 3/11 e non vede il crollo del 5/11 |
| 1 | VXN al 99 pct e inversione contro la gamma | **conferma su NQ**: la catena ha lo 0DTE al 48,9% e il 10/11 al 43,6%, sopra il mese; acceleratore su ogni orizzonte. **Niente da dire su ES**: la struttura a termine di ES non c'e' (lacuna). Vedi contraddizione 3 sulla volatilita' |
| 1 | Rotazione dell'S&P (RSP +7 su SPY in 20 g): spiega un order flow di ES diverso? | **coerente, non misurato**: ES tiene stanotte e ha delta di cassa positivo da due sedute. Ma venerdi' RSP stava sotto SPY (-0,69) proprio mentre il delta di ES era +17.790: la rotazione a 20 giorni non spiega la seduta |
| 1 | Minimi della notte NQ 26.751 / ES 7.364 e del 6/11 NQ 26.366,5 contro i livelli delle fasi 3 e 4 | **niente conferma indipendente**: la fase 4 li usa, ma sono gli stessi numeri del bridge. La gamma non ha livelli entro soglia: 26.500 e' a 133 punti da 26.366,5; 7.343,5 e' a 20 punti da 7.363,75 |
| 2 | Posizionamento contro il regime: il calo scarica il lato affollato del Nasdaq? | **niente da dire con misura**: lo dira' il report del 13/11. Oggi si sa solo che i lunghi crescevano ancora (+4.332) mentre i corti crescevano di piu' |
| 2 | Livelli del passo 6 contro muri e zero gamma (NQ 28.314, 30.775; ES 7.597, 7.851) | **nessuna vicinanza**: lo zero entro 7 giorni di NQ (28.257) e' a 39 punti dal VAL del 3/11 (28.296), fuori soglia; su ES X-Ray non entra nelle confluenze (lacuna) |
| 2 | Forza relativa: dal 3/11 NQ -3,88%, ES -1,93% | **conferma** con la macro (circa il doppio in un mese) e con l'order flow (ottobre accettato sotto settembre solo su NQ). **Ma** dalla chiusura di venerdi' ES fa +0,69% e NQ +0,50%: oggi il rapporto e' rovesciato |
| 2 | Il rapporto vale fino al 13/11 21:30 | preso: e' negli orari |
| 3 | Strike pesanti di NQ contro COT e valore | **nessuna confluenza entro 15 punti**: 27.500 sta 75 sopra il muro alto (27.425), 27.000 sta 113 sotto il VAL di venerdi'. Lo zero 0DTE 27.235 cade dentro il valore di venerdi', ma e' uno zero, non un muro |
| 3 | Muri sul bordo del valore, col regime | **conferma**: `confluenze.json` di NQ non ha livelli della gamma vicino ai bordi del valore recente; su ES non e' misurato (lacuna) |
| 3 | Regimi concordi, volatilita' no: coerente con la macro? | **conferma con riserva**: la paura concentrata sui prossimi giorni (macro) e la struttura inversa di NQ (gamma) dicono la stessa cosa; lo "scalino giu'" e' su tutti e due (fase 4). La misura del costo della paura pero' cambia con la finestra (contraddizione 3) |
| 3 | Lo 0DTE di NQ a 50 punti dal cambio, ES senza zero vicino | **niente da dire ora**: va riletto dopo le 15:30, con lo spot vero di X-Ray |
| 4 | Divergenza della notte contro X-Ray e GEX di NQ | **un fatto in piu'**: alle 08 NQ ha attraversato il put wall 0DTE 27.000 scendendo fino a 26.751,25, e alle 09 lo ha riattraversato salendo (ore 08-09 di `ore-NQZ6.json`, in `1-macro.md`). **Osservazione** di un caso, di notte, con l'open interest di ieri. ES e' rimasto 20 punti sopra il suo acceleratore piu' forte (7.343,5) |
| 4 | Delta di cassa di ES positivo in due cadute: copertura su SPX o posizionamento? | **niente da dire con misura**: i flussi di X-Ray sono solo di oggi e misti; il COT si ferma al 3/11. Resta una contraddizione aperta (la 1) |
| 4 | Discount profondo contro il COT | **conferma**: tutti i prezzi del COT stanno sopra, su tutti e due. Su ES il POC della settimana del report (7.753,75) coincide col POC dei compositi (7.754), con `stesso_dato` falso |
| 4 | CHOPPY contro acceleratore | **non si sommano**: misurano cose diverse (efficienza di 30 barre contro copertura stimata). Su 42 sedute di ES l'acceleratore ha dato giornate un po' piu' strette, non piu' larghe: lo zig-zag non contraddice cio' che si e' misurato, contraddice solo la lettura del video |

## I Livelli Confermati

Entra un prezzo indicato da almeno due fasi entro `soglia_punti` di `confluenze.json`: **15 punti su NQ, 4 su ES**. Le vicinanze vengono dal collettore; il quadro le ha verificate contro i rapporti. Tutte le fonti qui sotto sono profili di volume del bridge o la catena delle opzioni: COT e order flow leggono lo stesso bridge con costruzioni diverse (seduta contro settimana del report), quindi una confluenza fra loro e' meno di due fonti indipendenti. **Nessuna e' validata come supporto o resistenza.**

| strumento | prezzo | quali fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|---|
| NQ | 28.857-28.862 | gamma, order flow | zero gamma di tutte, A, `gex.py` 12:22; VAH di cassa del 30/10 | posizionamento: dove il regime supposto di fondo girerebbe, sul bordo di un valore vecchio. Fragile: la GEX viva da' lo zero a 28.982, 120 punti piu' su, e la catena e' incompleta lontano. Oggi +1.650 punti |
| NQ | 29.482-29.495 | COT, order flow | VAH della settimana del report 3/11; VAL 29/10, POC 26/10 | posizionamento, settimane: il bordo alto della settimana dei corti nuovi. +2.280 |
| NQ | 30.173-30.178 | COT, order flow | VAH settimana del report 20/10; POC 14/10 | posizionamento, settimane. +2.960 |
| NQ | 30.890 | COT, order flow | POC settimana del report 13/10; POC 12/10 | posizionamento: la settimana del movimento piu' grande degli speculatori. +3.680 |
| ES | 7.649,75-7.653,50 | COT, order flow | VAL settimana del report 27/10; VAL 23/10 | posizionamento, settimane. +200 |
| ES | 7.690 | COT, order flow | POC settimana del report 27/10; POC 22/10 | posizionamento, settimane. +240 |
| ES | 7.733-7.734,75 | COT, order flow | VAL settimana del report 20/10; VAL 19/10 | posizionamento, settimane. +280 |
| ES | 7.750-7.754 | COT, order flow | POC settimana del report 3/11 (7.753,75), VAH settimana 27/10; POC dei compositi di cassa 20 e 90 (7.754), POC 29/10 | la piu' fitta: dove la settimana dei report e il valore dei mesi hanno il centro. Fine del discount passa di qui. +300 |
| ES | 7.790,50-7.794,25 | COT, order flow | VAH settimana del report 20/10 e 3/11; POC 15/10 | posizionamento, settimane. +340 |
| ES | 7.860-7.863 | COT, order flow | VAH settimana del report 13/10; VAH del composito 90 (7.862), VAH 12/10 | bordo alto del valore dei mesi. +410 |

**Vicino al prezzo nessun livello passa il filtro.** Tutti i livelli di oggi entro 300 punti su NQ ed entro 100 su ES sono indicati da una fase sola (order flow) o sono lo stesso numero letto da due fasi.

**Non contate, `stesso_dato`** (due fasi, lo stesso numero del bridge; non e' una conferma): NQ 28.296 / 28.314 / 28.387 (valore del 3/11), 29.584-29.599, 29.637, 29.757-29.770, 29.805-29.817, 29.840-29.854, 29.880-29.883, 30.712 / 30.775 / 30.800 (valore del 13/10); ES 7.589,75, 7.596-7.597,75, 7.609,25-7.610,25, 7.740,25-7.743,25, 7.754,50-7.758,50, 7.772,25-7.776,25, 7.843,50-7.856,75 (valore del 13/10). Dalla macro, stesso dato del bridge dell'order flow: NQ 26.366,5 e 26.751,25, ES 7.363,75.

## I Muri Delle Opzioni Sul Bordo Del Valore, Col Regime

Regime da `confluenze.json`: **acceleratore al prezzo e sullo 0DTE, su NQ e su ES**.

- **Nessun muro entro soglia da un bordo del valore o da un muro del ping-pong**, su tutti e due (fase 3, domanda 3; confermato da `confluenze.json` di NQ, che non ha vicinanze gamma-valore vicino al prezzo).
- I piu' vicini: NQ 27.500 (call e put wall) a 75 punti sopra il muro alto del ping-pong (27.425); NQ 27.000 (put wall 0DTE) a 113 sotto il VAL di venerdi'; ES 7.343,5 (acceleratore piu' forte di X-Ray) a 21 punti sopra il muro basso (7.322,25).
- **Ipotesi del video, non provata**: col freno un muro tiene, con l'acceleratore si rompe ([il bias del giorno](../../../docs/research/metodo/il-bias-del-giorno.md), ipotesi D ed E). Su 42 sedute di ES il verso misurato e' stato l'opposto (0,55 contro 0,74 del movimento atteso, p 0,24). Il passaggio di NQ attraverso 27.000 alle 08 e' un caso solo, di notte.

## Le Contraddizioni

1. **Il delta di ES contro il prezzo di ES.** Giovedi' +1.610 e venerdi' +17.790 di delta di cassa con chiusure sotto il valore; NQ negativo tutti e due i giorni. Il COT dice speculatori S&P corti e banche che vendono (al 3/11); X-Ray dice flussi misti oggi. Chi compri aggressivo su ES senza ottenere il prezzo, nessuna fase lo misura.
2. **Chi guida.** In un mese NQ si muove circa il doppio di ES (macro, COT); stanotte ES ha tenuto il minimo e fatto il massimo, NQ ha rotto il suo (order flow); dall'apertura di domenica salgono quasi uguale (+1,2% e +1,1%, macro). Sono scale diverse: nessuna e' la "vera".
3. **Il costo della paura su NQ.** La macro trova l'implicita sopra la realizzata su tutti e due (VXN 34,02 contro 30,3 su 24 ore); la gamma trova NQ in linea (VXN ricostruito 32,7 contro 34,4 a 20 sedute, -1,7) ed ES caro (+2,2). Cambiano la finestra della realizzata e l'indice (ufficiale di venerdi' o ricostruito di oggi). Su ES le due fasi concordano.
4. **Il regime delle scadenze di ES.** Acceleratore al prezzo e sullo 0DTE, ma la mensile del 20/11 entro l'1% e' positiva (+13,8): e' calcolata sullo spot di venerdi', circa 48 punti di SPX sotto il prezzo implicito.
5. **Due GEX di NQ a 5 minuti.** `gex.py` -451,6, zero 28.862; GEX viva -366,4, zero 28.982. La confluenza a 28.857-28.862 esiste solo con la prima.
6. **Legacy e TFF sul Nasdaq.** Speculatori in riduzione (-5.966) nella Legacy, gestori +7.756 e fondi speculativi +7.772 nella TFF, nello stesso report. Tassonomie diverse: non si fondono.
7. **Nvidia.** -5% prima della cassa per l'indagine sul suo capo (CNBC 12:26), mentre NQ sale dalla chiusura di venerdi'. La macro lo registra; nessuna fase lo spiega.

## NQ Ed ES Insieme

**Dove si confermano**: stesso regime delle opzioni su tutte le scadenze (acceleratore, punto di cambio lontano); stessa posizione rispetto al valore (discount sotto i compositi, dentro venerdi', sopra il POC di venerdi'); stessa forma degli scalini (settimana giu', giorno in pausa, ping-pong di una giornata); stesso ritmo (CHOPPY, efficienza 0,09 e 0,10); stesso verso delle banche nel COT (Dealer in vendita, il movimento piu' grande delle quattro date); paura alta e put care su entrambi.

**Dove no**: le posizioni degli speculatori (Nasdaq netti lunghi al 90 pct, S&P netti corti; segni del flusso opposti per la seconda settimana); il delta di cassa (ES positivo, NQ negativo, due sedute); la notte (ES tiene il minimo, NQ lo rompe); lo 0DTE (NQ a 25 punti dal cambio, ES senza zero); il costo della paura (caro su ES, in linea o caro su NQ secondo la finestra); la profondita' (ottobre accettato sotto settembre solo su NQ); la macro sotto (chip contro rotazione difensiva).

**Chi guida**: nel mese NQ, che scende circa il doppio; stanotte ES (osservazione di una notte, fase 4). Nessuno studio prova che una divergenza fra i due dica il verso.

**Il passaggio di informazione fra le opzioni dei due indici e' un'ipotesi non provata.** La ricerca di oggi (`quant/paper/ricerche/2026-11-09-opzioni-fra-indici.md`) non c'e' ancora nel repository, e nessuno dei nove paper letti dalla fase 3 lo studia. Qui regimi, segni, flussi e volatilita' stanno accanto; i prezzi non si convertono.

## Gli Orari Di Oggi

Italia e New York in ora solare: cassa 09:30-16:00 di New York = 15:30-22:00 italiane (14:30-21:00 UTC).

| ora italiana | evento | cosa puo' cambiare del quadro |
|---|---|---|
| 15:30 | apre la cassa; X-Ray riparte con lo spot vero di SPX | si rilegge il regime di ES, la mensile positiva, lo 0DTE; nasce il valore di oggi, la notte non vi entra |
| 21:30-22:00 | ultima mezz'ora | la finestra di BDLM (momento con gamma negativa sui future S&P; su NQ la condizione non e' provata) |
| 22:00 | chiude la cassa, scadono le opzioni di oggi | lo zero 0DTE di NQ (27.235) smette di esistere; le chiusure dicono se il ping-pong regge |
| 23:00-00:00 | pausa del CME (16:00-17:00 di Chicago) | nuovo giorno CME |
| 10/11 10:00 e 14:30 | Waller; CPI e core di ottobre (attese 0,3% e 0,2%) | l'evento della settimana per tutti e due; dentro la scadenza NQ del 10/11 (volatilita' 43,6%) |
| 13/11 14:30 e 21:30 | PPI; nuovo COT con i dati del 10/11 | la scadenza settimanale piu' pesante di NQ; il COT vede il crollo del 5/11 e supera la fase 2 |
| 17/11 sera, 20/11 15:15 | conti Nvidia; scadenza mensile SPX del mattino | Nvidia pesa il 10,7% del Nasdaq-100 |

## Cosa Smentirebbe Il Quadro

- Dopo le 15:30 X-Ray su ES mostra la gamma al prezzo positiva o un punto di cambio vicino: i regimi non concordano piu'.
- NQ sale sopra 27.235 e ci resta: nel modello il regime 0DTE di NQ gira.
- NQ supera 27.319 mentre ES resta sotto 7.459: la guida passa a NQ.
- Uno dei due esce dalla zona di venerdi' (NQ 27.113-27.306, ES 7.415,75-7.454,50) e ci resta: "fermi nella pausa" diventa falso.
- Una chiusura oltre i muri del ping-pong (NQ 27.425 / 26.366,5; ES 7.469 / 7.322,25): lo scalino giu' smette di essere l'ultimo fatto.
- La cassa ha efficienza alta su tutti e due: il "a zig-zag" valeva solo per la notte.
- Il CPI di domani sotto le attese e il rialzo di dicembre che perde probabilita'.
- Il COT del 13/11 con gli speculatori del Nasdaq che tolgono molto rialzo, o le banche che ricomprano.

## Cosa Non Sappiamo

- Tutto e' letto prima della cassa: valore di oggi, X-Ray con spot vero e tab 4 mancano.
- Il segno dei dealer di NQ e' supposto; quello di ES e' stimato da OptionsDepth con metodo non pubblicato.
- Il COT ha 6 giorni e non vede il 4-6/11; nessuna soglia COT e' validata.
- Nessun livello di questo quadro e' provato come supporto o resistenza; l'ipotesi D e' smentita nel verso su ES, l'ipotesi E ha un caso solo.

## Le Lacune Del Collettore

1. `dati-ES/confluenze.json` non contiene i livelli di X-Ray in prezzi di ES (nessuna fonte gamma): le vicinanze gamma-valore e gamma-COT su ES non sono misurate (fase 5).
2. La tab 4 Range 40 non e' registrata nel bridge; `range-NQZ6.json` e `range-ESZ6.json` vuoti (fase 4).
3. Il pannello di ES non ha livelli depositati, CVD, valore ne' veti (fase 4).
4. Il composito di cassa 90 non e' pieno (NQ 47 sedute, ES 26); per NQ 6 sedute sono prima del rollover senza dire se aggiustate; manca la distanza dal VAL del composito in percento (fase 4).
5. La scala di ES (GEX in $ di ES contro il volume) non c'e' dal 5/11 (fase 3).
6. La variazione nel tempo di X-Ray (GEX 0DTE e gamma al prezzo nell'ultima ora) non c'e' (fase 3).
7. La struttura a termine di ES (breve contro mese) non c'e' nel meteo di ES (fase 3).
8. Il VXN ufficiale di oggi non c'e' (Yahoo fermo a venerdi' 22:15); VIX9D, VIX3M e 10 anni pure; 2YY=F fermo al 13/10 (fasi 1 e 3).
9. La finestra della GEX viva e' di 17 minuti con 7 punti, non un'ora (fase 3).
10. `ore-*.json` copre 48 ore di calendario: il lunedi' mancano le ore di venerdi' (Michigan, cassa) (fase 1).
11. `esiti` vuoto il lunedi'; l'ampiezza del Nasdaq-100 da api.nasdaq.com mescola probabilmente il pre-mercato di oggi; mancano il peso delle grandi nell'S&P e l'ampiezza per titolo dell'S&P (fase 1).
12. COT: manca `prima_del_rollover` in `settimane_dei_report`; i percentili a tre anni ci sono solo per l'ultimo report; manca lo z52 del livello del netto; manca l'ora di pubblicazione del report (fase 2).

## Per Il Giro

Il blocco QUADRO e' in `docs/research/giornate/NQZ6-2026-11-09.md` e `docs/research/giornate/ESZ6-2026-11-09.md`. Il quadro si rifa alle 15:30 se X-Ray cambia il regime di ES, quando il prezzo esce dalla zona di venerdi', e venerdi' 13/11 dopo le 21:30.
