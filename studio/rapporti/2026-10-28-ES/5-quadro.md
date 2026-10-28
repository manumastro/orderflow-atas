# Fase 5 · Il Quadro · 28/10/2026 · ES (ESZ6)

**Stato**: parziale. Quattro rapporti su quattro, ma il COT e' riusato dal 27/10 (report CFTC del 20/10,
8 giorni) con i confronti di prezzo fatti a 7.709,75 (27/10 07:23): qui sono riportati al prezzo di oggi.
La gamma e' su una catena parziale (scadenza del 30/10 con l'open interest ancora in arrivo); manca la
tab 4 dell'order flow (barre range).
**Dati**: i quattro rapporti; `dati/confluenze.json` e `dati/in-breve.json` (raccolta finita alle 10:23
ora italiana, 09:23 UTC; tutte le voci "ok" in `raccolta.json`). Prezzo di riferimento ESZ6 7.768,00
alle 10:17 (pannello) e 7.766,25 alle 10:23 (bridge M1 e `/opzioni`, differenza 0). Soglia delle
vicinanze: **4 punti** (`soglia_punti`). Per le differenze con NQ: `studio/rapporti/2026-10-28/5-quadro.md`.

**In una riga**: tassi alti ma in calo, Fed ferma oggi e attesa in rialzo a dicembre; speculatori corti
sull'S&P con pochissimi acquisti; gamma di segno incerto e piccola; il prezzo sta al centro del valore
di tre mesi e dentro il valore di ieri, fermo in attesa di Fed e trimestrali.

## In Parole Semplici

**Il punto**: l'S&P future e' fermo intorno a 7.766, nel mezzo di tutto: del valore di tre mesi, del
valore di ieri e della fascia dove le opzioni cambiano comportamento. Oggi i prezzi contano meno degli
orari: la Fed alle 19:00 e i conti di Microsoft e Alphabet dopo le 21:00.

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: tassi a 10 anni vicino al 5% ma scesi in un mese; Fed ferma oggi, a dicembre le scommesse danno un rialzo al 76%.
   → Dentro l'S&P i soldi sono andati su energia e difensivi, non fuori dal mercato: l'indice tiene.
2. **Le posizioni**: gli speculatori sono scommessi al ribasso in misura normale, e hanno pochissime scommesse al rialzo (dato del 20/10).
   → Pochi sono posizionati per una salita; chi ha comprato il 13/10 sta circa 85 punti sotto.
3. **Le opzioni**: sul totale, nel modello piu' usato, chi vende opzioni amplifica un po' i movimenti; su quelle che scadono stasera li frena.
   → Il segno dipende da una scadenza letta a meta', e la spinta vale meno dell'1% del volume di un giorno.
4. **Le settimane**: il prezzo e' sul prezzo piu' scambiato degli ultimi tre mesi.
   → Ne' caro ne' a sconto: e' il "prezzo giusto" del periodo.
5. **I giorni**: il valore e' sceso il 22 e il 23, poi e' risalito il 26 e il 27; ieri chiusura con compratori aggressivi in netta prevalenza.
   → Il mercato e' tornato nella zona alta di meta' ottobre.
6. **Adesso**: alle 10:17 circa 7.768; notte di soli 18 punti dentro il valore di ieri; l'indicatore misura equilibrio.
   → Nessuno ha cambiato idea stanotte; le opzioni di oggi prezzano circa 56 punti, il doppio di quanto ES si e' mosso.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.800-7.815 | le opzioni call piu' pesanti e il bordo alto del valore delle ultime settimane | il prezzo esce dalla zona alta di meta' ottobre, dove la copertura pesa di piu' sopra |
| circa 7.772-7.784 | il bordo alto del valore di ieri e la linea dove le opzioni cambiano segno | sopra, nel modello, le opzioni passano da "spinta" a "frenata" |
| circa 7.745 | dove smettono di frenare le opzioni di stasera, appena sopra un tratto poco scambiato | sotto, tutte le opzioni nel modello spingono, e sotto 7.741 il prezzo entra in un vuoto |
| circa 7.700 | lo strike con piu' opzioni put, gia' prezzo piu' scambiato del 16/10 | il posto dove la copertura e' piu' grande in ogni ipotesi |

**Quando**: 14:30 apre New York · 19:00 decisione Fed · 19:30 conferenza · 21:00 chiude New York e scadono le opzioni di oggi · dopo le 21:00 Microsoft e Alphabet · domani 13:30 PIL e inflazione PCE · venerdi' 20:30 nuovo COT
**Cambia idea se**: il prezzo esce dal valore di ieri (sopra 7.773 o sotto 7.741) e ci resta; il prossimo calcolo delle opzioni, con la scadenza di venerdi' completa, cambia il segno; la Fed alza oggi (8% nei future); il COT di venerdi' mostra acquisti o vendite grandi.
**Quanto e' provato**: prezzi, volumi e posizioni sono misurati; chi e' lungo e chi e' corto di opzioni e' un'ipotesi; nessuno studio del repository prova questi prezzi come appoggi, ne' misura come ES reagisce alla Fed o alle trimestrali.

**Le parole**: **valore** = la fascia dove passa il 70% del volume di una seduta; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **speculatori** = i fondi che cercano un guadagno sul prezzo (COT "Non-Commercial"); **COT** = il rapporto settimanale della CFTC su chi tiene i future; **call / put** = opzioni che guadagnano se il prezzo sale / scende; **zero gamma** = il prezzo dove, nel modello, chi vende opzioni passa da frenare a spingere; **vuoto** = tratto dove si e' scambiato poco, che il prezzo di solito attraversa in fretta.

## Il Quadro In Cinque Righe

1. **Macro** (10:18): 10 anni 4,99% (27/10, percentile 95,6 su due anni, -25 pb in 20 sedute); Fed ferma oggi 92-95%, dicembre +25 pb 76,5% (Polymarket); VIX 17,57 (10:02) contro realizzata ES 9,8% (20 sedute); RSP +3,83% contro SPY +0,71% in 20 sedute; XLC al minimo dell'anno.
2. **Posizionamento** (CFTC, 20/10, 8 giorni): S&P 500 e-mini Non-Commercial netto -142.671 (percentile 29,4), long al 7,1; variazione dell'ultima settimana al 3° percentile. Prezzo di oggi ~12 punti sopra il POC del 20/10 (7.754,5), dentro il suo valore; ~85 sotto il POC del 13/10 (7.851,25).
3. **Gamma** (10:17-10:23, convenzione A, segno ipotetico, catena parziale): tutte -1.567 M$, zero 7.783,7 (-17); 7 giorni -501, zero 7.772,3; 0DTE +1.307, zero 7.745,1; muri 7.800 e 7.815 call, 7.700 put; 4.036 future per l'1%, 0,33% del volume medio di cassa.
4. **Valore** (cassa 14:30-21:00 italiane, 18 sedute): shift su staccati il 26 e il 27 dopo due giu' (22-23); ieri 7.740,75-7.772,25, POC 7.758, delta +18.413; composito di cassa POC 7.755, valore 7.682-7.816,5: fair value.
5. **Adesso** (pannello 10:17): 7.768,00, dentro il valore di ieri; notte 7.752,25-7.770,25, POC 7.759, delta +196; regime BALANCE, efficienza 0,16; velocita' normale (proxy di volume, non speed of tape); nessun big trade.

## Gli Agganci, Uno Per Uno

**Dalla macro**

- *VIX 17,6 contro realizzata 9,8%, tre eventi in tre ore: coerente con la gamma?* **Conferma**: la volatilita' implicita dello 0DTE e' 20,5% (mossa attesa +-55,7 punti) contro realizzata 9,6% a 24 ore. Il "mercato compresso dalle opzioni" **non si conferma**: lo 0DTE e' positivo, il totale negativo, e la scala e' 0,33% del volume.
- *Rotazione difensiva e RSP +3,1 su SPY: coerente con il COT?* **Conferma debole** sui piccoli: il COT vede acquisti forti sul Russell (z +2,50 e +1,67), la macro IWM e KRE positivi. Date diverse (20/10 contro 20 sedute al 27/10). Sui Treasury **niente da dire**: COT dei Treasury non raccolto.
- *Chi paga la tesi dei tassi alti, i settori o l'indice?* I settori: comunicazioni al minimo dell'anno, tecnologia -2,6%; l'indice +0,63% e al percentile 90,9. Il COT aggiunge che gli speculatori non sono posizionati per la salita (long al 7,1). **Osservazione**, non una causa.
- *Notte ferma: attesa o accumulo?* **Attesa**: l'order flow misura 18 punti di notte, delta +196 su 125.002 contratti (0,16%), BALANCE, realizzata in calo da 10,9% a 6,6%.

**Dal COT**

- *Corti con long al 7,1: coerente con la macro?* **Coerente con il sentiment** (Fear & Greed 34, "paura"), **in contrasto con il prezzo** (percentile 91 dell'anno): vedi le contraddizioni. L'ipotesi dell'arbitraggio future-titoli resta non verificabile (manca la TFF dell'S&P).
- *7.851,25 contro muri e zero gamma*: **conferma** con lo strike 7.850 (open interest call 8.543, GEX A +460 M$). *7.754,5*: nessun livello di gamma entro 4 punti (lo zero dello 0DTE e' a 9,4). *7.879,75*: le call 7.875 (volume 1.087) sono a 4,75 punti, fuori soglia, e sono volume senza verso, non posizione.
- *Il cuore del volume (POC 7.775) sopra il prezzo*: **superato dal dato di oggi**. Con 17 sedute il POC era 7.775; con 18 sedute (`in-breve.json`) e' 7.755. Il prezzo (7.766) sta fra i due.
- *Il Russell*: gamma e order flow non leggono i piccoli: **niente da dire**.

**Dalla gamma**

- *7.700 contro COT e valore*: **conferma** con il POC del 16/10 (7.700). Nessun livello del COT vicino.
- *La fascia 7.745-7.784 fra gli zeri*: **conferma descrittiva**: contiene il valore della notte (7.758-7.765), il POC di ieri (7.758), il POC del composito di cassa (7.755). Il regime del modello cambia nome dentro il valore di oggi.
- *7.800-7.816*: **conferma**, muri call e VAH del 15/10, del 21/10 e del composito di cassa.
- *La macro pesa piu' della gamma?* **Conferma**: implicita al doppio della realizzata, Fed e trimestrali in tre ore, scala della copertura 0,33% (A) o 3,4% (B).

**Dall'order flow**

- *7.754,5-7.759*: **escluso** dalla tabella. COT e order flow leggono lo stesso file (`sedute-ESZ6.json`): e' un dato solo, e la gamma non ha niente entro 4 punti.
- *7.772 e 7.801 contro i muri*: **conferma**: zero dei 7 giorni a 7.772,3, muro call a 7.800.
- *Il vuoto 7.728,5-7.740,75*: **niente** di pesante dalla gamma; lo zero dello 0DTE (7.745,1) e' a 4,35 punti dal bordo alto, appena fuori soglia.
- *Compressione e gamma positiva?* **Contraddizione parziale**: positiva solo lo 0DTE, negativo il totale; in ogni caso la scala e' troppo piccola per legare le due cose.

## I Livelli Confermati

Almeno due fasi entro 4 punti (`confluenze.json`, `stesso_dato` falso ovunque, verificato contro i rapporti). Nessun livello e' validato come appoggio o respinta.

| prezzo | quali fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 7.847,5-7.851,25 | COT, gamma, order flow | COT: POC del 13/10, la settimana degli acquisti; gamma: strike 7.850, call OI 8.543; OF: VAH 05/10, VAL 12/10 | posizionamento: dove tornano in pari i long del 13/10, con un peso di call |
| 7.815-7.816,5 | gamma, order flow | gamma: muro call 0DTE, GEX A +742 M$ (solo fino alle 21:00); OF: VAH del composito di cassa | posizionamento: il bordo alto del valore di 18 sedute |
| 7.800-7.801,5 | gamma, order flow | gamma: muro call di tutte e dei 7 giorni (ma piu' put che call, GEX A netta -457); OF: VAH 15/10 e 21/10 | invalidazione della lettura "equilibrio nel blocco alto" |
| 7.783,25-7.783,7 | gamma, order flow (debole) | gamma: zero di tutte; OF: VAL 08/10, vicinanza del collettore non letta dalla fase 4 | sopra, il modello A diventa positivo su tutte le scadenze (fragile) |
| 7.770,25-7.775 | gamma, order flow, COT, macro | gamma: zero 7 giorni 7.772,3; OF: VAH e massimo di ieri 7.772,25-7.772,75; COT: VAH 20/10 7.774,5; macro e OF: massimo notte 7.770,25 | uscita dal valore di ieri verso l'alto; regime "di settimana" che cambia segno |
| 7.743,25-7.745,1 | gamma, order flow, COT | gamma: zero 0DTE; OF: VAL 14/10; COT: VAL 20/10 | sotto, tutte le scadenze negative nel modello; subito sotto il VAL di ieri (7.740,75) e il vuoto |
| 7.700 | gamma, order flow | gamma: muro put di ogni orizzonte, OI put 31.229, GEX A -1.986 M$; OF: POC cassa 16/10 | posizionamento: la copertura piu' grande in entrambe le convenzioni |

Le righe con COT e order flow insieme contano la gamma come fonte indipendente; COT e order flow leggono lo stesso file di sedute. Esclusi: 7.754,5-7.759 (stesso dato), 7.728,5-7.740,75 (solo order flow), 7.682 (solo order flow), 7.874-7.880 (massimo a un anno di Yahoo, POC 6/10, zero della 30/10 7.873,9: entro 6 punti, fuori soglia, e lo zero della 30/10 nessuna fase lo legge).

## Le Contraddizioni

1. **Speculatori corti, prezzo alto.** Long al 7,1 percentile e netto corto, mentre ES sta al percentile 91 dell'anno a 108 punti dal massimo e ieri ha chiuso con delta +18.413. Non si risolve: il COT ha 8 giorni, e il corto strutturale potrebbe essere arbitraggio (non verificabile senza la TFF dell'S&P).
2. **Il prezzo rispetto al COT e' cambiato dal rapporto.** `2-cot.md` (a 7.709,75) dice "45 punti sotto il POC del 20/10" e "141,5 sotto il 13/10". A 7.766 il prezzo e' sopra il POC del 20/10, dentro il suo valore, e circa 85 sotto il 13/10.
3. **Due regimi di gamma insieme.** 0DTE positivo sopra 7.745, totale negativo sotto 7.784: il prezzo e' fra i due. In piu' il segno del totale viene dalla 30/10, letta con l'open interest in crescita da 15.524 a 116.399. Ieri alle 07:37 (`ESZ6-2026-10-27.md`) il totale era -3.278 M$ con zero 7.743,9: le due catene sono entrambe parziali, il confronto non e' una misura.
4. **Il "muro call" 7.800 ha piu' put che call** (OI 14.626 contro 10.355): si chiama muro per la gamma delle call, la GEX netta e' negativa.
5. **"Prezzo giusto" di tre mesi su barre di 24 ore.** Il POC 7.767,5 della tab 1 e' su 90 barre Daily; il composito di sola cassa ha 18 sedute (POC 7.755). Le due letture dicono "fair value" entrambe, ma non sono lo stesso numero.
6. **ES contro NQ, oggi** (`studio/rapporti/2026-10-28/5-quadro.md`): su NQ speculatori lunghi al 96,9 percentile, su ES long al 7,1; gamma A positiva su NQ (+724 M$, 351 punti sopra lo zero), negativa su ES (17 sotto); NQ -2,55% in 20 sedute, ES +0,63%; notte di NQ +125 punti, di ES 18; NQ CHOPPY, ES BALANCE. Stessa giornata di eventi, posizionamenti opposti.

## Gli Orari Di Oggi

| ora italiana | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 14:30 | apre la cassa di New York (09:30) | regime e valore si rimisurano sulla seduta vera; il BALANCE della mattina smette di valere |
| 19:00 | decisione Fed e comunicato (14:00 New York) | un rialzo oggi (8% nei future) o un cambio sul dicembre muovono tassi e volatilita' |
| 19:30 | conferenza stampa | le parole su dicembre |
| 20:30-21:00 | ultima mezz'ora di cassa | BDLM (paper, future S&P): segue il resto della giornata solo con gamma negativa; oggi il segno dipende dall'orizzonte |
| 21:00 | chiude la cassa; scadono le opzioni di oggi | la gamma positiva dello 0DTE sparisce, lo zero 7.745 e il muro 7.815 smettono di esistere |
| dopo le 21:00 | Microsoft e Alphabet | due pesi massimi a cassa chiusa, con le comunicazioni al minimo dell'anno; peso nell'S&P non nei dati |
| 29/10 13:30 · prima delle 14:30 | PIL, PCE core, sussidi · LLY, MA, MRK, CAT, LIN | l'inflazione che la Fed guarda; settori non tech |
| 30/10 20:30 · 21:00 | nuovo COT (dati del 27/10) · scadenza di fine mese | nuova data sul prezzo; esce la scadenza che decide il segno del totale |

## Cosa Smentirebbe Il Quadro

- Il prezzo esce dal valore di ieri (sopra 7.772,75 o sotto 7.740,75) e ci resta: la riga "Valore" e' falsa.
- Il prezzo scende sotto 7.740 e attraversa 7.729-7.741 lentamente, con volume: il vuoto non era un vuoto.
- Il prossimo calcolo della gamma, con la 30/10 stabile, cambia il segno del totale: la riga "Gamma" cambia.
- Il regime passa a trend prima delle 19:00, o la realizzata sale verso l'implicita senza un evento: la lettura "attesa" e' superata.
- La Fed alza oggi; il COT del 30/10 mostra un aumento grande dei long o uno scarico dei corti.

## Cosa Non Sappiamo

- Il segno vero dei dealer sulle opzioni CME di ES (GPP contro VAPG); se la gamma diventa ordini sul future (DPS).
- La reazione misurata di ES a FOMC e trimestrali; il peso di MSFT e GOOGL nell'S&P.
- Il COT ha 8 giorni; il Conference Board di ieri non e' verificato (91,7 dal web contro 81,9 precedente).
- Il volume delle opzioni non ha verso: le call 7.875 (1.087) non dicono chi ha comprato.

## Le Lacune Del Collettore

- Orchestrazione: macro e order flow hanno letto prima della fine della raccolta (10:23): `raccolta.json`, `in-breve.json` e `confluenze.json` risultavano assenti, ma ci sono. Le fasi vanno lanciate dopo la raccolta.
- Order flow: barre range assenti (`range-ESZ6.json` vuoto, `esecuzione: null`); composito di cassa a 90 sedute con solo 18; CVD non come serie; i livelli del pannello (notte, Londra) non entrano in `confluenze.json`.
- COT: per ES `prezzo_alle_date`, `prezzo_adesso`, `settimane_dei_report` e cross-confirm sono calcolati su NQZ6; 29/09 di ESZ6 assente; TFF dell'S&P 500 assente; i livelli del COT non sono fonte in `confluenze.json`; i confronti non si riaggiornano quando il rapporto si riusa.
- Gamma: rilettura fino a stabilita' delle scadenze pesanti (30/10, 20/11, 18/12); peso delle 10 scadenze saltate; volume per strike non diviso per scadenza; storico della GEX di ES; dati SPX e base ES-SPX per l'X-Ray.
- Macro: `esiti` vuoto; VIX9D, VIX3M, VXN, ^TNX senza barre di oggi; 2YY=F fermo al 13/10; probabilita' solo per la prossima riunione; pesi dei titoli nell'S&P; reazione storica a FOMC; movimento implicito di MSFT e GOOGL; ora del Fear & Greed.
