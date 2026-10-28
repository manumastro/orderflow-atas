# Fase 5 · Il Quadro · 27 ottobre 2026 (ES, contratto ESZ6)

**Stato**: fatto, su quattro rapporti parziali (vedi Le Lacune); ES per richiesta dell'utente del 27/10.
**Dati**: `1-macro.md` (dati delle 07:23), `2-cot.md` (report CFTC del martedi' 20/10, 7 giorni),
`3-gamma.md` (GEX di ESZ6 ricalcolata alle 07:37; la prima, delle 07:23, era fallita), `4-orderflow.md`
(sedute alle 07:23, quattro tab e giro alle 07:30). Controllati in `dati/`: `confluenze.json` (07:37,
soglia 4 punti), `in-breve.json` (gamma: future 7.711,5 alle 07:37, bridge M1, differenza 0),
`raccolta.json` (chiusa alle 07:37, nessun errore nell'ultima versione), `ore-ESZ6.json`, `giro.txt`.
**Orari**: ora italiana, solare dal 25/10; New York in ora legale fino all'1/11: cassa 14:30-21:00.

**In una riga**: ES fermo dentro il valore di ieri, basso nei tre mesi; opzioni che in teoria
amplificano, mercato che si muove poco; speculatori corti con pochi acquisti; Fed domani alle 19:00.

## In Parole Semplici

**Il punto**: ES e' in attesa. Sta circa 2% sotto il massimo dell'anno, dentro il prezzo "giusto" dei
tre mesi ma nella parte bassa, e da ieri gira fra circa 7.700 e 7.730 senza scegliere. Il mondo
intorno e' teso e la settimana e' piena di appuntamenti: oggi i numeri dicono "calma prima della Fed".

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: tassi al 5%, petrolio sopra 100, credito debole, paura; l'S&P regge grazie al resto
   del mercato mentre la tecnologia scende.
   → l'indice e' piu' solido del Nasdaq, ma il contesto non lo conferma.
2. **Le posizioni**: gli speculatori sono corti sull'S&P 500, con acquisti fra i piu' bassi in tre anni;
   l'ultima settimana non hanno spinto da nessuna parte.
   → pochi sono pronti per una salita; chi ha comprato il 13 ottobre sta circa 140 punti sotto.
3. **Le opzioni**: secondo il modello dei fornitori chi vende opzioni oggi amplifica i movimenti; la
   soglia di cambio e' circa 30 punti sopra, a 7.744.
   → la forza e' piccola (meno dell'1% degli scambi) e per le opzioni di oggi e' quasi zero.
4. **Le settimane**: il prezzo e' circa 60 punti sotto il centro dei tre mesi (circa 7.775).
   → non e' ne' caro ne' a sconto, ma e' piu' vicino al fondo che alla cima.
5. **I giorni**: giovedi' e venerdi' il valore e' sceso; ieri e' risalito di poco, senza tornare sopra.
   → fra circa 7.730 e 7.745 c'e' un tratto dove nessuna seduta recente ha scambiato molto.
6. **Adesso**: alle 07:30 il prezzo e' circa 7.710, al centro di ieri; l'indicatore dice "senza
   direzione".
   → la notte scende a piccoli passi, ma si muove meno della meta' di quanto le opzioni mettono in conto.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.740-7.744 | la soglia di cambio delle opzioni e la cima del tratto vuoto; il massimo di ieri | in teoria le opzioni passano da "amplificano" a "frenano", e il prezzo esce sopra il valore recente |
| circa 7.760 | il prezzo con piu' call aperte, e un bordo di valore della settimana scorsa | il prezzo torna nel valore del 20-21 ottobre |
| circa 7.700-7.703 | il prezzo con piu' put aperte e il minimo della notte | sotto, il prezzo esce dal valore di ieri e torna in quello del 22 ottobre |
| circa 7.650 | molte put aperte, il fondo del valore del 23 e il minimo di ieri notte | il fondo del tratto basso delle ultime tre sedute |

**Quando**: 14:30 apre la cassa · 15:00 fiducia dei consumatori · 21:00 scadono le opzioni di oggi ·
domani 19:00 Fed, 19:30 conferenza, sera Microsoft e Alphabet · giovedi' 13:30 PIL e inflazione ·
venerdi' 20:30 nuovo COT, 21:00 scadenza di fine mese
**Cambia idea se**: il prezzo passa e resta sopra 7.744, o sotto 7.700; la cassa apre fuori da
7.704-7.729; il prossimo calcolo delle opzioni sposta soglia o muri di decine di punti.
**Quanto e' provato**: prezzi, valori, posizioni e volatilita' sono misurati. Il regime delle opzioni e'
un modello con un segno che nessuno conosce, e nessuno studio prova che questi prezzi fermino ES.

**Le parole**: **valore** = la fascia dove si e' fatto il 70% degli scambi; **COT** = il rapporto
settimanale della CFTC su chi tiene i future; **speculatori** = fondi che cercano un guadagno sul
prezzo; **call/put** = opzioni che guadagnano se il prezzo sale/scende; **muro** = il prezzo con piu'
opzioni aperte di un tipo; **soglia di cambio (zero gamma)** = dove, nel modello, chi vende le opzioni
smette di amplificare e comincia a frenare.

## Il Quadro In Cinque Righe

| | fatto (fonte, ora) |
|---|---|
| macro | 10A 5,00% (Tesoro, 26/10); VIX 18,4 contro realizzata 9,6 (20 sedute); RSP +3,6%, NQ -4,1% in 20 sedute; HYG pct 6; FOMC 28/10 fermi 93,8% |
| posizionamento | COT 20/10 (7 gg), S&P e-mini Non-Commercial netto -142.671 (pct 29), long pct 7,1; variazione pct 3,2; long del 13/10 su POC 7.851,25 |
| gamma | 07:37, convenzione A (segno ipotetico): tutte -3.278 M$, zero 7.743,9; 0DTE -34, zero 7.711,9; muri 7.760 / 7.700; 0,68% del volume |
| valore | composito di cassa 17 sedute 7.681,75-7.823, POC 7.775: fair value basso; blocco 22-26/10 7.653,5-7.728,5; vuoto 7.728,5-7.743,25 |
| adesso | 07:23 ultimo 7.709,75, dentro ieri (7.704,5-7.728,5, POC 7.714,5); notte 7.719,25-7.730 lasciata dal basso; CHOPPY 0,13 |

## Gli Agganci, Uno Per Uno

**Dalla macro**
1. *Salita larga, COT coerente?* **Conferma parziale.** Sull'S&P e-mini gli speculatori non comprano
   (long pct 7,1, variazione pct 3,2): la salita larga non passa dai loro future. L'unico flusso forte
   e' sul Russell 2000 (z52 +2,50 e +1,67), che e' coerente con una salita larga. **Osservazione.**
2. *VIX contro realizzata, contro la gamma.* **Conferma.** Implicita della settimana 16-17% (gamma)
   contro realizzata 10,6-11,6% a 24-48 ore; il VIX contro la realizzata a 20 sedute dice lo stesso. La
   realizzata e' lo stesso dato in macro, gamma e order flow (`ore-ESZ6.json`): una misura, non tre.
3. *Credito debole contro ES vicino al massimo.* **Niente da dire** dalle altre fasi (Contraddizioni, 2).
4. *Recinto 7.651,75-7.739,5.* **Conferma.** Il massimo (cassa del 26/10, 16:00) sta 1,1 punti sotto lo
   zero gamma della settimana e dentro il vuoto dell'order flow; il minimo (notte del 26/10, ora 02)
   sta fra lo strike 7.650 e il VAL del 23 (7.653,5).

**Dal COT**
1. *Corti con pochi acquisti, coerente con la macro?* **Conferma** con il sentiment (paura 27,1) e il
   credito; **contraddizione** con il prezzo a circa 2% dal massimo. Vedi Le Contraddizioni, 2.
2. *7.851,25, 7.754,5, 7.879,75 contro le opzioni.* **Niente entro 4 punti.** 7.754,5 e' a 4,5 dal peso B
   7.750 e a 5,5 dalla call wall; 7.879,75 e' a 4,35 dallo zero gamma del 30/10 (7.884,1): fuori soglia.
3. *Il cuore del volume sopra il prezzo.* **Conferma**, ma con lo stesso dato: il composito di cassa
   (POC 7.775) e' lo stesso file nelle due fasi. Il lato che ha comprato il 13/10 sta sopra.
4. *Il flusso sul Russell.* **Niente da dire**: nessuna fase legge i piccoli (IWM e' una lacuna).

**Dalla gamma**
1. *7.700.* **Conferma**: put wall dei tre orizzonti e POC di cassa del 16/10 (`confluenze.json`,
   `stesso_dato` falso); minimo della notte 7.702,75 a 2,75 punti. Il VAL di ieri, 7.704,5, e' a 4,5.
2. *Zero gamma, 7.760, 7.800 contro il profilo.* **Confermati tutti e tre** (tabella sotto).
3. *I livelli del COT sopra le opzioni.* **Conferma**: nessun livello COT dentro la soglia.
4. *GEX negativa e implicita alta prima della Fed.* Conferma sulla volatilita', contraddizione sul regime (1).

**Dall'order flow**
1. *Il vuoto 7.728,5-7.743,25 contro i muri.* **Conferma**: la cima del vuoto coincide con lo zero
   gamma (7.740,6-7.743,9), la call wall 7.760 e' il primo valore sopra. La lacuna "confluenze vuoto"
   del rapporto 4 e' superata dal ricalcolo delle 07:37.
2. *7.690 contro i muri delle put.* **Niente**: put wall a 10 punti, data COT a 64. Una sola fase.
3. *Senza direzione, realizzata bassa, FOMC.* **Conferma** con macro e gamma sulla calma; la GEX dello
   0DTE quasi nulla (-34) e il suo zero sul prezzo dicono anche loro "nessuna spinta oggi".
4. *Fair value basso contro il COT.* **Conferma descrittiva**: gli speculatori sono corti e il prezzo
   sta sotto il cuore del volume; il dato aggregato non dice che siano gli stessi operatori.

## I Livelli Confermati

Soglia **4 punti** su ES (15 su NQ, scalata: la barra M1 di ES si muove circa un quarto di quella di
NQ; dichiarata dall'orchestratore). Vicinanze da `confluenze.json` (07:37), piu' quelle della macro.
Nessun livello e' validato come supporto o resistenza.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 7.800-7.801,5 | gamma, order flow | strike con OI 7.168 call e 11.308 put, GEX A -358, secondo peso B; VAH di cassa del 15 e del 21/10 | posizionamento: la cima del blocco 20-21/10 |
| 7.758-7.760 | gamma, order flow | call wall dei tre orizzonti, GEX A +540; VAL del 15/10, VAH del 16/10 | posizionamento: primo valore sopra lo zero gamma |
| 7.739,5-7.743,9 | gamma, order flow, macro | zero gamma (7.740,6 settimana, 7.743,9 tutte); POC del 19/10, VAL del 20/10 (cima del vuoto); massimo di cassa del 26/10 | il regime teorico cambia; uscire sopra il valore recente |
| 7.711,9-7.714,5 | gamma, order flow | zero gamma dello 0DTE (vale fino alle 21:00); POC e chiusura del 26/10 | e' dove sta il prezzo: il centro di ieri |
| 7.700-7.702,75 | gamma, order flow (e macro) | put wall dei tre orizzonti, OI put 20.801, GEX A -1.664; POC del 16/10; minimo della notte | sotto, il prezzo lascia il valore di ieri |
| 7.650-7.653,5 | gamma, order flow, macro | strike pesante, GEX A -246; VAL del 23/10; minimo della notte del 26/10 (7.651,75) | il fondo del blocco basso 22-26/10 |

**Non contati**: 7.754,5 (COT e order flow leggono lo stesso POC del 20/10: un dato), 7.775 (stesso
composito), 7.851,25 contro il nodo del composito 7.854 (stesse sedute, 2,75 punti), 7.879,75 (4,35
dallo zero gamma del 30/10), 7.690 (una fase).

## Le Contraddizioni

1. **Il modello delle opzioni dice "amplifica", il mercato si muove poco.** GEX A negativa su tutti gli
   orizzonti e prezzo sotto lo zero gamma; ma realizzata 6,6% a 4 ore, CHOPPY/BALANCE, efficienza
   0,13-0,17. Due letture possibili, nessuna provata: la forza e' piccola (0,68% del volume) e lo 0DTE
   e' neutro; oppure il segno dei dealer non e' quello della convenzione A. Resta aperta.
2. **Il prezzo vicino al massimo, tutto il resto no.** ES a 2% dal massimo dell'anno e nel fair value;
   credito al minimo dell'anno (HYG pct 6), paura (27,1), speculatori con long al pct 7,1. La macro
   lo spiega con la rotazione (osservazione di un mese); nessuna fase la misura sui settori.
3. **La notte: delta positivo, chiusure orarie in calo** (7.728,75 → 7.713,25); assorbimento non misurato.
4. **Il regime dell'indicatore**: CHOPPY alle 07:23, BALANCE alle 07:30, con schede M1 passate su M2
   fra le 07:26 e le 07:28. La soglia fra i due non e' nei dati: vale la lettura delle 07:23.
5. **Gamma, ieri contro oggi**: GEX A da -530 a -3.278, zero da 7.720,5 a 7.743,9; per la fase 3 e'
   soprattutto la catena letta meglio. Fuori strumento: speculatori Nasdaq al pct 96,9, NQ -4,1%.

## Gli Orari Di Oggi

| ora italiana | evento | cosa puo' cambiare del quadro |
|---|---|---|
| 14:30 | apertura della cassa | il regime della notte non vale per la cassa; apertura dentro o fuori 7.704,5-7.728,5 |
| 15:00 | fiducia Conference Board (att. 81,2) | primo dato della settimana, peso medio |
| 21:00 | scadenza 0DTE e chiusura della cassa | lo zero 7.711,9 esce dal calcolo; l'OI di stasera rifa' soglia e muri |
| 28/10 19:00, 19:30 | FOMC e conferenza; sera MSFT e GOOGL | la volatilita' implicita e quindi la GEX; il giudizio su dicembre |
| 29/10 13:30 | PIL, PCE core, sussidi; sera AMZN | prima della cassa |
| 30/10 20:30, 21:00 | nuovo COT (dati del 27/10); scadenza di fine mese (GEX A -2.370) | sostituisce il posizionamento; dopo la scadenza il quadro gamma cambia |

## Cosa Smentirebbe Il Quadro

- Prezzo accettato sopra 7.743,25-7.744 (valore nel vuoto): "dentro ieri" e "sotto lo zero" cadono.
- Prezzo accettato sotto 7.700-7.702,75: si torna nel valore del 22/10 (POC 7.690).
- Realizzata che sale verso l'implicita (mossa dello 0DTE 43,4 punti) prima della Fed: "calma" e' falso.
- La GEX con l'OI di stasera che sposta zero o muri di decine di punti; il COT di venerdi' con long in forte aumento.

## Le Lacune Del Collettore

- Macro: i settori dell'S&P 500 (XLF, XLI, XLE, KRE, IWM); ampiezza dell'S&P, non del Nasdaq-100;
  trimestrali non tecnologiche; dicembre dai future ZQ, non solo Polymarket.
- COT: `prezzo_alle_date`, `prezzo_adesso`, `settimane_dei_report` solo per NQZ6, quindi i livelli COT
  di ES non entrano in `confluenze.json`; manca la seduta ESZ6 del 29/09; manca la TFF dell'S&P 500
  (13874A); cross-confirm calcolato rispetto al Nasdaq.
- Gamma: `gex.py` cerca il chart con un id fisso e fallisce dopo un ricaricamento; il moltiplicatore
  (50 $) non e' scritto nel JSON e la docstring dice 20 $; `in-breve.json` non riporta i muri per
  scadenza; `volume_finestra` scritto in ora legale.
- Ordine della raccolta: `giro.txt` e `quattro-tab.txt` (07:30) stampano la GEX di ieri, perche' il giro
  e' partito prima del ricalcolo delle 07:37 (la regola: il giro una volta, dopo la GEX).
- Order flow: niente tab Range su ES; composito di cassa a 17 sedute, non 90; soglia CHOPPY/BALANCE
  non nel pannello; big trade non tarati su ES. Macro: `raccolta.json` non c'era alle 07:23.
- `giro.txt` scrive le ore un'ora avanti dopo il 25/10 (massimo di ieri "17h" contro le 16:00 di
  `ore-ESZ6.json`; "26/10 25h"; "08h" alle 07:30); POC del 26/10 7.714,35 contro 7.714,50.
