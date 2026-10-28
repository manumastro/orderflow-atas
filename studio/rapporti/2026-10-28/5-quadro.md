# Fase 5 · Il Quadro · 28/10/2026

**Stato**: parziale. Quattro rapporti su quattro; il COT e' riusato dal 27/10 (report CFTC del 20/10,
8 giorni) e i suoi confronti di prezzo sono fatti con il prezzo del 27/10 alle 12:18 (29.579,75):
qui sono aggiornati al prezzo di oggi. Manca la tab 4 (barre range) dell'order flow.
**Dati**: i quattro rapporti; `dati/confluenze.json` e `dati/in-breve.json` (raccolta 10:13-10:18 ora
italiana, 09:13-09:18 UTC, nessun errore in `raccolta.json`). Prezzo di riferimento NQZ6 29.939,75
alle 10:17 (pannello) e 29.941 alle 10:16 (M1 del bridge). Soglia delle vicinanze: 15 punti
(`soglia_punti` di `confluenze.json`).

**In una riga**: tassi alti ma in calo, Fed ferma oggi e attesa in rialzo a dicembre; speculatori
molto sbilanciati al rialzo; gamma positiva nel modello ma piccola; il prezzo e' tornato nel valore
del 21/10, sotto il muro di 30.000, in un mercato a zig-zag che aspetta Fed e trimestrali.

## In Parole Semplici

**Il punto**: NQ stamattina e' fermo poco sotto 30.000, il prezzo dove si concentrano le opzioni, dopo
aver recuperato in due giorni quasi tutto il crollo del 22. Il mercato e' a zig-zag e aspetta: la Fed
alle 19:00 e i conti di Microsoft e Alphabet dopo le 21:00 pesano piu' di qualunque livello.

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: tassi a 10 anni vicino al 5%, ma scesi in un mese; la Fed oggi e' data ferma, a dicembre in rialzo.
   → La sorpresa possibile non e' la decisione, sono le parole su dicembre e i conti di stasera.
2. **Le posizioni**: gli speculatori sul Nasdaq sono sbilanciati al rialzo quasi come mai in tre anni (dato del 20/10).
   → Molto peso da una parte; il prezzo e' sopra il loro ultimo report ma oltre 800 punti sotto il 13/10.
3. **Le opzioni**: nel modello piu' usato chi vende opzioni frena i movimenti, e il peso e' tutto a 30.000.
   → La frenata e' piccola (0,3% del volume di un giorno) e quasi tutta scade entro venerdi'.
4. **Le settimane**: il prezzo e' nella parte alta della zona "giusta" degli ultimi due mesi, sopra il prezzo piu' scambiato.
   → Ne' caro ne' a sconto: si sta nel mezzo, verso l'alto.
5. **I giorni**: giovedi' 22 il valore e' crollato, lunedi' e martedi' e' risalito; il prezzo e' tornato dove si scambiava mercoledi' 21.
   → Il terreno del crollo e' quasi ripreso, ma con piu' venditori aggressivi che compratori.
6. **Adesso**: alle 10:17 il prezzo e' circa 29.940 e l'indicatore misura un mercato a zig-zag, lento.
   → Nessuna direzione scelta; con l'apertura di New York e la Fed il regime si rimisura.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.000 | il muro delle opzioni, appena sopra il massimo della notte e della mattina | il prezzo e' dove in teoria la copertura pesa di piu' |
| circa 29.700 | dove si ferma la frenata delle opzioni di oggi, in un tratto poco scambiato ieri | sotto, la parte di oggi smette di frenare (modello) |
| circa 29.570-29.600 | soglia della frenata di tutte le opzioni, bordo dei valori del 20 e del 26, inizio di un vuoto | sotto, il modello dice spinta invece di frenata, e il prezzo entra in un tratto senza scambi |
| circa 30.775 | dove gli speculatori hanno alleggerito di piu' (13/10) | lontano, quasi 850 punti: si torna dove hanno venduto |

**Quando**: 14:30 apre New York · 19:00 decisione della Fed · 19:30 conferenza · 21:00 scadono le opzioni di oggi e chiude New York · dopo le 21:00 Microsoft e Alphabet · domani 13:30 PIL e inflazione PCE · venerdi' 20:30 nuovo COT
**Cambia idea se**: il prezzo esce dal valore del 21 (sopra 30.087 o sotto 29.880) e ci resta; scende sotto 29.580, dove il modello delle opzioni cambia segno; la Fed alza i tassi oggi (dato all'8%); il regime passa da zig-zag a direzionale con l'apertura; il COT di venerdi' mostra uno scarico grande.
**Quanto e' provato**: prezzi, volumi e posizioni sono misurati; il segno delle opzioni e' un modello (nessuno sa davvero chi e' corto); nessuno studio del repository prova questi prezzi come appoggi, ne' misura come NQ reagisce alla Fed o alle trimestrali.

**Le parole**: **valore** = la fascia dove passa il 70% del volume di una seduta; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **muro** = lo strike con piu' opzioni aperte; **gamma / frenata** = quanto chi vende opzioni deve ricoprirsi, contro il movimento (frena) o a favore (spinge); **zero gamma** = il prezzo dove la frenata diventerebbe spinta; **COT** = il rapporto settimanale della CFTC su chi tiene i future; **zig-zag** (choppy) = rotture che rientrano.

## Il Quadro In Cinque Righe

1. **Macro** (10:17): 10 anni 4,99% (27/10, percentile 95,6 su due anni, -25 pb in 20 sedute); Fed ferma oggi 92-95%, dicembre +25 pb 76,5% (Polymarket); VXN 24,91 (27/10) sopra la realizzata 18,9%; NQ -2,55% contro RSP +3,83% in 20 sedute.
2. **Posizionamento** (CFTC, 20/10, 8 giorni): Non-Commercial Nasdaq netto 50.726, percentile 96,9; corti al 3,2. Prezzo di oggi sopra il valore del 20/10 (29.599-29.842), 835 punti sotto il POC del 13/10 (30.775).
3. **Gamma** (10:13-10:16, convenzione A, segno ipotetico): tutte +723,7 M$, zero 29.589,7 (-351); 0DTE +297,0, zero 29.693,6; muro 30.000; 1.209 future per l'1%, 0,27% del volume di cassa.
4. **Valore** (cassa 14:30-21:00 italiane): shift su il 26 e il 27 dopo il crollo del 22; prezzo dentro il valore del 21/10 (29.883-30.087, POC 30.030), sopra il POC dei compositi di cassa (29.735-29.780).
5. **Adesso** (pannello 10:17): 29.939,75, dentro il valore di Londra 29.933-29.989; regime CHOPPY, efficienza 0,08; notte +125 punti con delta +1.546 (1,8% del volume).

## Gli Agganci, Uno Per Uno

**Dalla macro**

- *VXN 24,9 sopra la realizzata, tre eventi in tre ore; coerente con le opzioni?* **Conferma**: le implicite delle scadenze vicine (28/10 31,0%, 29/10 29,1%, 30/10 27,9%) stanno sopra il VXN e sopra la realizzata (11,4% a 4 ore). Le opzioni prezzano l'evento, il prezzo della mattina no.
- *Polymarket 76,5% e 10 anni alto: coerente con il COT?* **Contraddizione parziale**: tassi alti e Fed restrittiva, eppure gli speculatori restano lunghi al 96,9 percentile e nella settimana al 20/10 hanno aggiunto (+2.203) con il prezzo in calo di quasi 1.400 punti. Sui Treasury **niente da dire**: il COT dei Treasury non e' raccolto.
- *NQ peggio di RSP: si vede nell'order flow come vendita nei rimbalzi?* **Niente di conclusivo**: le ultime tre casse hanno delta negativo (-1.812, -4.001, -565) mentre il valore saliva; la notte sale con delta piccolo positivo. Venditori aggressivi senza discesa del valore: e' un'osservazione, non una vendita nei rimbalzi misurata.

**Dal COT**

- *Netto al 96,9: regime della macro o posizionamento indietro?* Vedi la contraddizione sopra; la rotazione della macro (tech dietro all'indice equipesato) e lo sbilanciamento al rialzo sul Nasdaq non si conciliano nei dati di oggi.
- *30.775 e 30.890 contro muri e zero gamma*: **niente da dire** dalla gamma (nessuno strike pesante entro 15 punti; il piu' vicino e' 30.500). Vicini all'order flow (VAH composito 39 sedute 30.779; POC cassa 12/10 30.890), in parte stesso dato.
- *29.599-29.757 e VAL settimanale 29.584 contro gamma e order flow*: **conferma** su 29.570-29.600 (sotto). 29.757-29.770 **non e' una conferma**: e' lo stesso numero del bridge letto da due fasi (`stesso_dato`).
- *Russell con ricoperture forti (z +1,67): coerente con la rotazione?* **Conferma debole**: la macro vede la rotazione fuori dal tech (RSP +3,83%); il COT la vede nel Russell. Date diverse (20/10 contro 20 sedute al 27/10).

**Dalla gamma**

- *30.000 contro la cornice del 21 e il massimo della notte*: **conferma** con il massimo della notte (29.989,25) e il VAH di Londra (29.989, pannello 10:17), 11 punti. Il POC del 21 (30.030) e' a 30 punti, fuori soglia: dentro la stessa cornice, non vicino.
- *Zero gamma 29.578-29.590 con VAH del 26, VAL del COT, put wall 0DTE*: **conferma**, tre fasi entro 30 punti; vedi la tabella.
- *29.000 sulla VAL dei compositi di cassa*: **conferma** (28.996-29.000), a circa 940 punti.
- *Regime A positivo ma 0,27%, giorno di Fed: coerente con la macro?* **Conferma** sul fatto che l'evento pesa piu' della gamma: implicite sopra il VXN, contango della volatilita', nessun panico.

**Dall'order flow**

- *30.000 dentro la cornice del 21*: **conferma**, come sopra.
- *29.570-29.600, tre fonti*: **conferma**; le fonti indipendenti sono due (modello della gamma e profili di volume del bridge): il COT legge lo stesso profilo dell'order flow.
- *29.757-29.770 stesso dato*: **escluso** dalla tabella.
- *Choppy alle 10:17 contro gamma positiva*: **coerenti nella descrizione** (rotture che rientrano, mercato smorzato), ma la gamma e' 0,27% del volume: niente lega le due cose. Il regime e' misurato sulla mattina di Londra, non sulla cassa.

## I Livelli Confermati

Almeno due fasi entro 15 punti (`confluenze.json`, piu' le aggiunte dalle letture, marcate). Nessun livello e' validato come appoggio o respinta.

| prezzo | quali fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 30.773-30.785 | COT, order flow (in parte stesso dato) | COT: POC del 13/10, alleggerimento piu' grande; OF: VAH composito 39 sedute 30.779 | posizionamento: dove gli speculatori hanno venduto, e il bordo alto del valore del periodo |
| 30.173-30.178 | order flow, COT | OF: POC cassa 14/10; COT: VAH della settimana del report 20/10 | posizionamento: primo POC sopra la cornice del 21 |
| 30.100-30.115 | gamma, order flow | gamma: strike 30.100, GEX A +34,9; OF: VAH cassa 15/10 | posizionamento sopra la cornice, poco sopra il suo bordo (30.087) |
| 29.989-30.000 | gamma, order flow, macro (aggiunta del quadro) | gamma: muro call e put di tutte, OI 4.177, call 0DTE 540; OF: VAH Londra 10:17 e massimo notte 29.989,25; macro: stesso massimo notte | posizionamento: il peso massimo delle opzioni sul bordo alto della mattina |
| 29.694-29.700 | gamma, order flow (aggiunta del quadro) | gamma: zero 0DTE 29.693,6, strike 29.700 GEX A +48,0; OF: tratto sottile di ieri 29.670-29.720 | invalidazione del regime A per le opzioni di oggi, in un tratto poco scambiato |
| 29.570-29.600 | gamma, order flow, COT | gamma: zero tutte 29.589,7 e 7 giorni 29.577,8, put wall 0DTE 29.600; OF: VAH 26/10 29.570, VAL 20/10 29.599, inizio vuoto 29.570-29.637; COT: VAL settimana 29.584, VAL 20/10 | invalidazione del regime A di tutte le scadenze, sopra un vuoto di volume |
| 28.996-29.000 | gamma, order flow | gamma: put OI 1.503, GEX A -50,2; OF: VAL compositi di cassa 20 e 39 sedute | posizionamento: il bordo dello sconto, lontano |

Esclusi: 29.757-29.770 e 29.842-29.854 (stesso dato COT e order flow); 30.712, 30.800, 31.483-31.556
(stesso dato); 30.549-30.557, 30.824-30.890, 31.310-31.315 (vicinanze fra profili del bridge, oltre 850
punti, nessuna lettura delle fasi). Il massimo a un anno di Yahoo (31.483,25) coincide col VAL del 06/10:
contesto, a oltre 1.500 punti.

## Le Contraddizioni

1. **Speculatori lunghi contro rotazione e tassi.** COT al 96,9 percentile, corti al 3,2; macro: NQ -2,55% contro RSP +3,83%, Fed verso il rialzo. Non si risolve: il COT e' di 8 giorni fa e non dice quando.
2. **Il prezzo rispetto al COT e' cambiato dal rapporto.** Il 2-cot.md (prezzo 29.579,75 del 27/10) dice "sotto il valore di tutte e quattro le date". Con 29.940 oggi il prezzo sta sopra il valore del 20/10 e sopra il POC della settimana (29.770); resta sotto il VAH della settimana (30.178) e sotto i POC del 29/09 (30.609, -669), del 13/10 (30.775, -835) e del 06/10 (31.530, -1.590).
3. **Tassi in calo, tech in calo.** 10 anni -25 pb in un mese e NQ -2,55%: la relazione "tassi giu', tech su" (osservazione generale della macro) non si vede in queste 20 sedute.
4. **Valore su con delta giu'.** Shift su il 26 e il 27 con delta di cassa negativi; il 22, la seduta del crollo, con delta +2.805. Nessun documento del metodo ne trae una regola.
5. **Opzioni agitate, prezzo calmo.** Implicite 28-31% contro realizzata 11,4% a 4 ore e regime choppy: non e' contraddizione di dati, e' il prezzo dell'evento; si dice perche' le due letture vanno in direzioni opposte.
6. **Orario delle trimestrali.** La macro scrive "dopo le 21:00", la gamma "dopo le 22:00". Oggi l'Italia e' in ora solare e New York no: la chiusura di New York (16:00) e' alle 21:00 italiane. Vale "dopo le 21:00".

## Gli Orari Di Oggi

| ora italiana | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 14:30 | apre la cassa di New York (09:30) | il regime e il valore si rimisurano; il valore di Londra smette di essere il riferimento |
| 19:00 | decisione Fed e comunicato (14:00 New York) | un rialzo oggi (8% nei future) o un cambio sul dicembre muovono i tassi e la volatilita' |
| 19:30 | conferenza stampa | le parole su dicembre; muove il 2 anni |
| 21:00 | chiude la cassa; scadono le opzioni di oggi | esce il 41% della GEX A; lo zero 29.694 smette di esistere |
| dopo le 21:00 | Microsoft e Alphabet | circa un quarto del Nasdaq-100, a cassa chiusa |
| 29/10 13:30 | PIL, deflatore, PCE core, sussidi | l'inflazione che la Fed guarda |
| 30/10 20:30 | nuovo COT (dati del 27/10); 21:00 scadenza di fine mese | nuova data sul prezzo; esce un altro terzo della GEX A |

## Cosa Smentirebbe Il Quadro

- Il prezzo esce dal valore del 21/10 (sopra 30.087 o sotto 29.880) e ci resta: la riga "Valore" e' falsa e il quadro va rifatto.
- Il prezzo scende sotto 29.570-29.590 e attraversa il vuoto lentamente, con volume: il vuoto non era un vuoto.
- Il prezzo passa 30.000 senza alcuna sosta e la volatilita' realizzata sale: la frenata del modello A non si vede, o il segno dei dealer e' l'altro.
- La Fed alza oggi, o il regime passa a trend prima delle 19:00: la riga "Adesso" e la lettura macro sono superate.
- Il COT del 30/10 mostra un taglio grande dei lunghi: la riga "Posizionamento" cambia.

## Cosa Non Sappiamo

- Il segno vero dei dealer sulle opzioni CME di NQ; la reazione misurata di NQ a FOMC e trimestrali.
- Il COT ha 8 giorni; il Conference Board di ieri non e' verificato (91,7 dal web contro precedente 81,9).
- Il valore di Londra cambia fra 10:17 e 10:18 (29.933-29.989 contro 29.912-29.969): la vicinanza 29.989-30.000 dipende dal minuto.

## Le Lacune Del Collettore

- Barre range della tab 4 non registrate nel bridge (`range-NQZ6.json` vuoto).
- Composito di cassa a 90 sedute con solo 39; due definizioni della notte (dalle 18:00 New York in `oggi_notte`, dalle 16:00 nel pannello); finestra di Londra non dichiarata.
- I livelli del pannello (Londra, notte) e il massimo della notte non entrano in `confluenze.json`.
- Macro: `esiti` vuoto; 2YY=F fermo al 13/10; VIX9D, VIX3M, VXN, ^TNX senza barre di oggi; probabilita' solo per la prossima riunione; nessuna reazione storica di NQ a FOMC e trimestrali; movimento implicito delle opzioni di MSFT e GOOGL; ora del Fear & Greed.
- Gamma: confronto con la GEX di ieri; volume totale e rapporto put/call delle opzioni di oggi; ora della realizzata e annualizzazione dell'implicita.
- COT: percentili assoluti di lunghi e corti per le `ultime_4`; profilo del venerdi' di pubblicazione; COT dei Treasury; i confronti di prezzo non si riaggiornano quando il rapporto si riusa.
