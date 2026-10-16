# Fase 5 · Il Quadro · 2026-10-15 (aggiornamento della sera, dati delle 18:43)

**Stato**: fatto, su quattro rapporti: tre nuovi e parziali (1-macro, 3-gamma, 4-orderflow), uno
riusato (2-cot). Nessuno manca. Rifa il quadro delle 11:04 (`precedenti/1850/5-quadro.md`) perche'
tre delle sue smentite si sono avverate (vedi sotto).
**Dati**: `1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`; raccolta chiusa alle **18:51**
ora italiana (`raccolta.json`, tutte le voci `ok`; le fasi 1 e 4 l'hanno letta prima che fosse
riscritta). **L'eta' di ogni fase**, rispetto alle 18:51:
- **macro** nuova: dati delle 18:43, quotazioni 18:28-18:43; curva Treasury al 14/10; VXN alla
  chiusura del 14/10 (22:00, quasi 21 ore); VIX3M forse non aggiornato; i dati delle 14:30 non sono
  verificati (del PPI c'e' solo un titolo CNBC);
- **COT** riusato: testo scritto sui dati delle 10:45, report di martedi' 6/10, **9 giorni**. I numeri
  di `cot.json` sono stati riletti alle 18:43: stesso report (`data` 2026-10-06). Il "prezzo adesso"
  del rapporto (29.822,5) e' delle 10:51;
- **gamma** nuova, solo NQ: catena NQZ6 alle 18:43, chiusa alle 18:46, OI (contratti aperti) del 14/10.
  **ES non entra nel quadro**, per richiesta dell'utente: `gex-ESZ6.json` non e' letto;
- **order flow** nuovo: dati delle 18:43, cassa (15:30-22:00 ora italiana) in corso da 3 ore e 13 minuti.

Ho controllato nei JSON: zeri, muri, peso B e strike 29.250-30.900 (`gex-NQZ6.json`, `quando`
16:43:09 UTC); sedute dal 07/10, 14/10, 17-18/09, 22/09, 29/09, `composito_di_cassa`, `oggi_finora`
(`sedute-NQZ6.json`); valore di oggi e compositi (`tab-ruoli.json`); regime e CVD (`pannello.json`);
prezzo e tab 2 (`giro.txt`).

**In una riga**: tassi lunghi in calo dopo un PPI caldo, Fed vista un po' piu' vicina a un rialzo,
paura concentrata sulla tecnologia; speculatori del Nasdaq al massimo di lunghezza in una foto di
nove giorni fa; gamma A negativa con il prezzo a 54 punti dallo strike piu' pesante; cassa di oggi
staccata sotto quella di ieri, con delta in acquisto e regime misurato CHOPPY.

## Il Quadro In Cinque Righe

1. **Macro (18:43).** Il 10 anni scende a 5,087% (-10 pb, pb = 0,01%) nonostante un PPI caldo (6,3%
   annuo, solo titolo CNBC). Polymarket da' il 27,5% a un rialzo il 28/10 (era 24,5%). VIX 16,94 in
   contango. HYG, l'ETF del credito ad alto rendimento, al percentile 1,6 a un anno. Fear & Greed 29,3.
   QQQ meno RSP (l'S&P equipesato) -1,76 punti percentuali oggi; SMH -9,27% in cinque giorni [1-macro].
2. **Posizionamento (6/10, 9 giorni).** Non-Commercial (gli speculatori; Legacy, solo future): netto
   lungo al 100° percentile a tre anni, campo short (posizioni corte) al percentile 0,6; flusso
   ordinario (z 0,58). Dalla chiusura del 6/10 (31.480,75) alle 18:43 (29.946,25): **-1.534,5 punti,
   -4,9%** (conto mio) [2-cot, 4-orderflow].
3. **Gamma (18:43, solo NQ).** GEX (gamma dei dealer, milioni di $ per 1%) -1.747,6 nella convenzione
   A, quella dei fornitori; il segno vero non si conosce. Zeri (dove la GEX cambia segno): tutte
   30.752,1, entro 7 giorni 30.656,9, 0DTE (scade alle 22:00) 30.314,3. Muri a 30.000. 0DTE e 16/10
   fanno il 75% della GEX A. Scala: 0,71% del volume medio di cassa per 1% [3-gamma].
4. **Valore.** La cassa di oggi, in corso, ha valore 29.792-30.060, POC (prezzo di massimo volume)
   29.960: **staccata** 22 punti sotto quella di ieri (30.082-30.478, POC 30.173). Se chiude cosi',
   dal 07/10 sei sedute su sette hanno lo shift giu', le ultime quattro staccate [4-orderflow].
5. **Adesso (18:43).** 29.946,25: dentro il valore di cassa, sotto il POC. Minimo 29.622,5 nell'ora
   delle 15, poi +439,25 fino a 30.061,75, 20,25 sotto il VAL di ieri. CVD di cassa (delta cumulato)
   +8.960. Pannello: CHOPPY (efficienza 0,10, 3 rotture rientrate), velocita' morta (proxy) [4-orderflow].

## Cosa E' Cambiato Dalle 11:04

| misura | 11:04 | 18:43 | effetto sul quadro |
|---|---|---|---|
| prezzo | 29.867,75, nel valore della notte | 29.946,25, nel valore di cassa; estremi 29.622,5-30.061,75 | riga "adesso" riscritta |
| valore di cassa di oggi | non aperto | 29.792-30.060, POC 29.960, staccato giu' | riga "valore" estesa a oggi |
| 10 anni / rialzo il 28/10 | 5,19% (14/10) / 24,5% | 5,087% / 27,5% | inflazione e tassi lunghi si separano |
| VIX / implicita 0DTE NQ | 17,52 / 32,4% | 16,94 / 24,5% | la volatilita' si calma dopo i dati |
| zero gamma, tutte | 30.900,6 | 30.752,1 | **esce** dalla fascia 30.888-30.922 |
| zero gamma, 7 giorni | nessuno | 30.656,9 | **nuovo** livello confermato (30.650-30.661) |
| zero gamma, 0DTE | 30.470,1 | 30.314,3 | **cade** il livello 30.458-30.478 |
| regime | CHOPPY 0,15, 6 rotture | CHOPPY 0,10, 3 rotture, copertura si' | la contraddizione con la gamma resta |
| delta | notte -2.320 | cassa +8.960 (+1,6% di 569.186 lotti) | la contraddizione valore-delta si allarga |
| ritardo del COT | -1.613 (-5,1%) | -1.534,5 (-4,9%) | stesso report, prezzo piu' alto |

**Le smentite del quadro delle 11:04.**

| smentita scritta alle 11:04 | esito alle 18:43 |
|---|---|
| il prezzo esce dal valore della notte | **avverata**: sotto fino a 29.622,5, sopra fino a 30.061,75 |
| la cassa riaccetta il valore del 14/10 (sopra 30.082) | non avverata: massimo 30.061,75, 0 tocchi su 30.082 |
| cede il minimo della notte 29.754 | **avverata**: 29.711 nell'ora delle 14, 29.622,5 in quella delle 15 |
| dopo le 14:30 il pannello non misura piu' CHOPPY | non avverata: CHOPPY 0,10 |
| VIX9D sopra VIX3M | non avverata: 14,87 contro 18,79 (VIX3M forse fermo a ieri) |
| RSP cede quanto QQQ | non avverata: RSP +0,98%, QQQ -0,78% |
| ES sopra il suo zero A | non valutata: ES fuori dal quadro per richiesta dell'utente |
| COT del 16/10 | non ancora uscito |
| zero gamma fuori da 30.888-30.922, o muro lontano da 30.000 | **avverata per lo zero** (30.752,1); il muro resta a 30.000 |

## I Fatti

**La domanda finale** (`il-framing-e-il-cot-si-leggono-insieme.md`, passo 5): il lato posizionato
(il netto lungo, chiusura del 6/10 a 31.480,75) sta sopra o sotto il cuore del volume? I compositi
non sono cambiati dalle 11:04; le sottrazioni sono mie.
- **Composito di 90 giorni aggiustato, barre di 24 ore**: vertice 29.817,50. I lunghi stanno 1.663,25
  **sopra**; il prezzo 128,75 sopra (fair value).
- **Composito di sola cassa, 20 sedute dal 17/09**: vertice 30.770. I lunghi stanno 710,75 **sopra**;
  il prezzo 823,75 sotto, 511,75 sotto il VAL 30.458.
- Risposta invariata: **sopra**, su tutte e due le scale. Nessuna e' la regola del live (sola cassa,
  90 giorni): lacuna.

**I livelli confermati da piu' fasi.** Soglia: due prezzi di fasi diverse entro **15 punti** su NQZ6
(soglia della skill). Distanze dal prezzo del bridge delle 18:43 (29.946,25). "Stesso dato" = due
fasi leggono lo stesso numero del bridge per ragioni diverse: conferma di lettura, non di fonte. La
gamma e' l'unica fonte indipendente dal profilo.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.530 (+1.584) | COT, OF (stesso dato) | COT: POC del 6/10, netto al massimo. OF: POC di cassa piu' alto delle 20 sedute | posizionamento: il prezzo dell'ultima foto COT |
| 31.065-31.070 (+1.119) | gamma, OF | gamma: zero A della sola 16/10 (31.070,4). OF: POC del 02/10 (31.065), un gradino della scala | posizionamento, **debole**: per l'OF non e' una riga dei livelli; lo zero vale fino a domani alle 22:00 |
| 30.888-30.922 (+942) | COT, OF (stesso dato) | COT: valore del 22/09, la variazione piu' grande (z 1,96; settimana di scadenza). OF: mensola di tre POC | posizionamento: **ha perso la gamma** (zero ora 136 sotto il VAL 30.888) |
| 30.745-30.783 (+799) | gamma, OF | gamma: zero A di tutte, 30.752,1. OF: POC del 23/09, 24/09, 13/10, 21/09; il POC del composito di cassa (30.770) e' a 17,9 dallo zero | posizionamento: sopra, la GEX A totale diventa positiva, dentro il cuore della cassa recente. **Nuovo** |
| **30.650-30.661 (+704)** | COT, gamma, OF | COT: VAH del 29/09. Gamma: zero A entro 7 giorni (30.656,9). OF: massimo di cassa del 14/10 (30.660,75, 0 tocchi oggi) | posizionamento: sopra, la GEX A della settimana diventa positiva sul massimo della seduta della rottura. **Nuovo**, fonte indipendente |
| 30.609-30.634 (+663) | COT, OF (stesso dato) | COT: POC del 29/09, unica variazione negativa. OF: primo scaffale di POC sopra il buco 30.173-30.609 | posizionamento debole |
| 30.173-30.185 (+227) | macro, OF (stessa seduta) | macro: chiusura di cassa del 14/10, riferimento del gap. OF: POC della seduta della rottura, 0 tocchi oggi | riferimento: centro e chiusura della seduta che ha portato il valore sotto |
| 30.060-30.082 (+114) | macro, OF (stesso dato) | macro: massimo del rimbalzo 30.061,75. OF: VAH di oggi 30.060, VAL di ieri 30.082, il vuoto di 22 punti | invalidazione della descrizione: sopra 30.082, lo stacco di oggi non c'e' piu'. **Nuovo** |
| **29.950-29.960 (+4)** | gamma, OF, macro | gamma: 29.950, terzo peso B dello 0DTE. OF: POC di cassa di oggi, 31 tocchi, 4.481 lotti, punto di rotazione. Macro: VAH dalle 00:00 | posizionamento: il centro di oggi. La gamma **vale fino alle 22:00**. **Nuovo** |
| **29.792-29.815 (-154 / -131)** | gamma, OF, macro | gamma: 29.800, terzo peso B di tutte, secondo dello 0DTE. OF: VAL di cassa 29.792, POC Europa 29.815 (il vertice 29.817,50 a 17,5 dallo strike). Macro: POC dalle 00:00 | posizionamento: il bordo basso di oggi; uscirne e' lasciare il valore della seduta. **Nuovo** |
| 29.700-29.737 (-246 / -209) | macro, OF, gamma (debole) | macro: VAL dalle 00:00 29.700. OF: minimo Europa 29.711, POC di cassa 17-18/09. Gamma: strike 29.700 e 29.750 (netta A -111,2 e -95,9), solo nella tabella degli strike | posizionamento: l'ultimo valore di cassa prima del 21/09. Sostituisce 29.730-29.754: il 29.754 ha ceduto |
| 29.622,5 (-324) | macro, OF (stesso dato) | minimo di oggi, ora delle 15; OF: 16,75 sopra il minimo del 17/09 | posizionamento: sotto, nessun valore di cassa nelle 20 sedute |
| 29.215-29.315 (-696) | COT, gamma | COT: valore del 15/09 (POC 29.250), var. +12.823, z 1,15. Gamma: strike 29.250, 1.143 put di OI (508 nello 0DTE) | posizionamento: l'unico valore COT sotto il prezzo. **Nuovo**, fonte indipendente |

**Fuori soglia, scritti per non ripeterli.**
- **30.000** (i muri, il primo peso B ovunque) sta **dentro** il valore di cassa di oggi, sul bordo
  della fascia 29.950-30.000 (delta +2.822), ma a 18 dal VAH della notte e a 40 dal POC di cassa:
  conferma d'area, non di livello.
- 30.314,3 (zero 0DTE): 141 sopra 30.173, 164 sotto 30.478. 30.100 (prima netta A positiva): 18
  dal VAL del 14/10. 31.500 (strike): 17 dal VAL del 6/10. 29.500: 17,5 dal composito grezzo.
- 30.458-30.478 resta solo all'order flow. 30.770 (cuore della cassa) entra solo come vicino dello zero.

**Lo stesso numero da due fonti.** Il future del calcolo GEX e' **29.975,5** alle 18:43:09; il bridge
da' 29.946,25-29.946,50 (pannello, giro, `oggi_finora`). Le barre range da 10 punti fra le 18:42:16 e
le 18:43:24 (fase 4, tab 4) escludono che sia lo stesso istante. Con il prezzo del bridge lo strike
30.000 sta a **53,75** punti, non a 24,5 come scrive la fase 3; lo zero di tutte a 805,85 (2,69%).

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| macro 1: paura sulla tecnologia, netto COT contro rotazione | **contraddizione** | i lunghi al massimo sono sull'indice che resta indietro (QQQ-RSP -4,33 pp in cinque giorni). La foto e' del 6/10: lo scarto non si chiude prima del COT di domani |
| macro 2: VIX in calo e contango, e la gamma? | conferma parziale | anche l'implicita dello 0DTE di NQ scende (32,4% -> 24,5%) dopo le 14:30. La "fragilita'" vale solo nella convenzione A; VXN fermo a ieri |
| macro 3: il recupero e' aggressione in acquisto? il valore regge? | **conferma** per l'aggressione; niente sul resto | delta positivo dalle 13 alle 17, CVD +8.960. Il delta conta chi aggredisce, non se apre o chiude: chiusura di vendite non si distingue. Valore dalle 00:00 (29.700-29.950) e di cassa (29.792-30.060) contengono il prezzo |
| macro 4: Warsh alle 05:30, rialzo al 27,5% | **conferma** | l'implicita del 16/10 (24,2%, 1,30 volte la realizzata) contiene l'evento fra le due scadenze; la fase 4 chiede di rileggere il pannello dopo Warsh |
| COT 1: netto al massimo contro la macro | **contraddizione** | come macro 1; S&P e Russell hanno i venditori sull'acceleratore, RSP sale. **Osservazione** |
| COT 2: valori COT contro muri e zeri | **conferma** per 30.557-30.650 e 29.215-29.315; **cade** per 30.888-30.980 | zero 7 giorni a 6,9 dal VAH del 29/09; strike 29.250 sul POC del 15/09; lo zero di tutte e' sceso a 136 sotto il VAL del 22/09 |
| COT 3: sopra o sotto il cuore del volume | risposto | sopra, su tutte e due le scale; invariato |
| COT 4: il delta dopo il 6/10 conferma la rottura? | **contraddizione** | delta di cassa dal 07/10 al 14/10 +15.178 (somma mia da `sedute`), oggi +8.960 in corso: il valore scende con il delta in acquisto |
| gamma 1: 30.000 dentro il valore dell'OF? | conferma d'area | dentro 29.792-30.060; nessun bordo entro 15 |
| gamma 2: zeri contro valori COT | conferma per due coincidenze | 30.656,9 / 30.650 e 29.250 / 29.250. Sono coincidenze entro soglia: nessuna e' validata |
| gamma 3: gamma A negativa contro CHOPPY | **contraddizione** | il modello A vorrebbe continuazione (BPVZ); il pannello misura rotazione. In mezzo, una salita di 439 punti fra le 15 e le 17: se stia nella finestra del regime non si sa (lacuna) |
| gamma 4: implicita del 16/10 contro tassi e COT | coerente con la macro; niente dal COT | evento alto prima della mensile; il COT e' di prima e non parla di volatilita' |
| OF 1: COT 22/09 nella mensola; il COT di domani non vede 14-15/10 | **conferma** | stesso dato; la fase 2 dice lo stesso ("il 14 non ci sara'") |
| OF 2: muro 30.000 e zero 30.887 sulla mensola | conferma d'area per il muro; **smentito** per lo zero | la fase 4 leggeva la tab 2 del giro delle 11:51; alle 18:43 lo zero e' 30.752,1 |
| OF 3: put wall 0DTE 29.700, zero 0DTE 30.461 sul VAH del 14/10 | **smentito** | stesso motivo: alle 18:43 il put wall dello 0DTE e' 30.000 e lo zero 30.314,3 |
| OF 4: delta orario positivo, pannello dopo Warsh | **conferma** | la fase 1 legge le stesse barre orarie (`ore-NQZ6.json`) |

## Cosa Dicono Per NQ

1. **Tre scale, tre posizioni, invariate.** Settimana: valore che scende, modello di momentum
   (`profile-framing.md` §5, fase 4). 90 giorni a 24 ore: fair value. 20 sedute di cassa: sotto tutta
   la distribuzione. Il quadro dichiara le tre letture, non sceglie.
2. **Il posizionamento e' vecchio quanto il calo.** I lunghi stanno sopra il cuore del volume su tutte
   e due le scale. **Osservazione**: un solo caso simile (oro, 8/09); nessuna soglia COT e' validata.
3. **La gamma si e' avvicinata al prezzo.** Dalle 11:04 gli zeri scendono di 148-156 punti e la
   settimana ne ha uno, sul massimo della seduta della rottura e sul VAH COT del 29/09. Sotto il
   prezzo, la gamma conferma i bordi di oggi (29.950, 29.800) solo fino alle 22:00.
4. **Oggi il delta in acquisto ha ottenuto prezzo, poi poco.** Dal minimo +439 punti con delta
   positivo; alle 17:30 +2.090 per 15 punti; il CVD al 95% del suo range con il prezzo al 74%. Il
   valore resta staccato sotto ieri. **Osservazione** della fase 4.
5. **Il contesto macro e' di rotazione, non di fuga dal rischio**: tassi lunghi giu', VIX giu', RSP
   su, credito al minimo. Stona il COT, che e' di prima.

## Le Contraddizioni

Scritte, non risolte.

- **COT contro macro.** Speculatori al massimo della lunghezza (6/10) sull'indice che la rotazione
  vende. Forse lo spiega il tempo; il COT di domani copre solo 7-13/10.
- **Gamma contro order flow.** GEX A negativa (nel modello, continuazione) contro CHOPPY con 3
  rotture rientrate, in una giornata con una salita di 439 punti in due ore.
- **Valore contro delta.** Valore piu' basso in sei sedute su sette dal 07/10, con delta di cassa
  +15.178 dal 07/10 a ieri e +8.960 oggi. Nelle ore delle 14 e delle 15 delta +1.223 e +2.416 con prezzo -26,5 e -102.
- **Inflazione contro tassi lunghi.** PPI caldo e rialzo piu' probabile, 10 anni -10 pb. Senza il 2
  anni di oggi non si vede quale parte della curva abbia seguito la Fed.
- **Il prezzo del future**: 29.975,5 (GEX) contro 29.946,25 (bridge) nello stesso minuto.
- **La tab 2 del giro contro i JSON**: alle 18:43 il giro stampa la GEX delle 11:51 (zero 30.887,
  0DTE 30.461, put wall 29.700); i JSON dicono 30.752,1, 30.314,3, 30.000. Due agganci della fase 4
  nascono da li'.
- **La struttura oraria**: il giro dice "massimi e minimi crescenti", ma i minimi delle ore 13, 14 e
  15 scendono (29.748,50; 29.711; 29.622,50).
- **Non risolte dalle 11:04**: POC di ieri 30.173 (M1) contro 30.187 (Range); scarto di rollover
  +299,25 contro +293,75; l'OI "di ieri" che cambia (put 30.000: 3.690 alle 11:04, 4.556 alle 18:43).

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 15/10 21:30-22:00 | ultima mezz'ora della cassa | finestra di BDLM, ipotesi mai provata sui nostri dati |
| 15/10 22:00 | chiusura della cassa; scadenza dello 0DTE | la cassa diventa un profilo chiuso (shift e stacco misurati); esce il 42% della GEX A (-737,3 su -1.747,6); 29.950-29.960 perde la gamma |
| 15/10 22:00 - 16/10 15:30 | notte prima del terzo venerdi' | finestra di BTW (misurata su ES, mai su NQ) |
| ven 16/10 05:30 | discorso di Warsh, presidente della Fed | tassi e pannello: il quadro va riletto dopo |
| 16/10 21:30 | COT, posizioni di martedi' 13/10 | il quadro si rifa: il libro dopo quattro sedute di shift giu' e una "nessuno" |
| 16/10 21:30-22:00, 22:00 | ultima mezz'ora e scadenza mensile (-581,8 di GEX A) | finestra di GJ (pinning); dopo, la GEX A resta circa un quarto |
| 20-21/10; 28/10 19:00 | NFLX; META e TSLA; FOMC, poi GOOGL e MSFT | il contesto delle prossime due settimane |

## Cosa Smentirebbe Il Quadro

Fatti osservabili, non previsioni.

- Il prezzo **esce dal valore di cassa di oggi** (sotto 29.792 o sopra 30.060): la riga "adesso"
  diventa falsa.
- La cassa **riaccetta il valore del 14/10** (torna e resta sopra 30.082): cade lo stacco di oggi.
- **Cede il minimo 29.622,5**: cade "il minimo ha tenuto"; sotto non c'e' valore di cassa recente.
- Il pannello **non misura piu' CHOPPY**: cade la contraddizione con la gamma.
- **Il 10 anni torna sopra 5,19%** con NQ in calo: cade "i tassi lunghi fanno da rifugio".
- **RSP cede quanto QQQ**: cade "vendita settoriale".
- **VIX9D sopra VIX3M**: cade "la volatilita' si e' calmata".
- **Una lettura GEX** con lo zero della settimana lontano piu' di 15 da 30.650-30.661, o il muro
  lontano da 30.000.
- **Il COT del 16/10**: un netto ancora al massimo smentisce che il calo abbia portato una
  riduzione; un netto in forte calo smentisce l'opposto.

## Cosa Non Sappiamo

- I dati delle 14:30: del PPI solo un titolo; vendite al dettaglio, Philly Fed e sussidi mancano.
- Il segno dei dealer (A o B) e chi ha comprato le put; se l'OI che cambia e' una catena che arriva.
- Se i lunghi del 6/10 sono ancora li': nove giorni.
- Se il recupero di oggi e' apertura di acquisti o chiusura di vendite.
- Sei dei tredici livelli confermati sono conferme di lettura dello stesso dato.
- Nessuna soglia COT, nessun livello di gamma e nessuna confluenza sono validati: tutto e' descrizione.

## Le Lacune Del Collettore

Il lavoro per il codice. Dalle 11:04 sono risolte: i big trade sui livelli (ora numerici) e la
chiusura di `raccolta.json` (18:51), arrivata pero' dopo che due fasi l'avevano letta.

- lacuna del collettore: FRED (fed funds, inflazione attesa, spread high yield, stress), manca `FRED_API_KEY`.
- lacuna del collettore: il dato uscito degli eventi del giorno; il calendario della settimana prossima (404).
- lacuna del collettore: rendimenti intraday per scadenza (2A, 30A); VXN e VIX3M intraday affidabili.
- lacuna del collettore: probabilita' FOMC da future sui fed funds o FedWatch; ampiezza del Nasdaq-100.
- lacuna del collettore: esito delle trimestrali; testo delle notizie (solo titoli RSS).
- lacuna del collettore: `settimana_di_rollover` solo per l'ultimo report COT, non per `ultime_4`.
- lacuna del collettore: il ritardo in prezzo del COT; il profilo della settimana coperta dal report.
- lacuna del collettore: la TFF senza variazioni di long e short, z52, percentili, ultime quattro.
- lacuna del collettore: catena tagliata a 100 strike per lato; griglia dello zero solo a +-4%.
- lacuna del collettore: un segnale di OI definitivo (data e ora dell'OI, o una rilettura di conferma).
- lacuna del collettore: volatilita' realizzata intraday dalle barre di ATAS; finestra del volume delle opzioni.
- lacuna del collettore: la variabile di BDLM (rendimento dalla chiusura di ieri alle 21:30).
- lacuna del collettore: il giro e' raccolto prima della GEX e la tab 2 stampa la lettura delle 11:51.
- lacuna del collettore: la fonte e l'istante del `future` di `gex-NQZ6.json` (29.975,5 contro 29.946,25 del bridge).
- lacuna del collettore: la cassa di oggi non e' in `sedute-NQZ6.json` (apertura, delta, chiude, staccato, shift).
- lacuna del collettore: `oggi_finora` ha cambiato contenuto (notte e cassa insieme, numeri tondi da 50).
- lacuna del collettore: il composito di sola cassa a 90 giorni e la sua zona; l'unita' della "quota" dei nodi.
- lacuna del collettore: la finestra del regime; la velocita' sulla barra chiusa; la durata di riferimento delle barre range.
- lacuna del collettore: il profilo per fasce della cassa del 14/10 e di oggi.
- lacuna del collettore: la regola di swing della struttura oraria del giro.
- lacuna del collettore: lo scarto di rollover con la convenzione di `profile-framing.md`; una sola misura per numero.
- lacuna del collettore: la tabella delle confluenze fra fasi (prezzi entro soglia), oggi fatta a mano qui.

## Per Il Giro D'orizzonte

Il blocco QUADRO in `docs/research/giornate/NQZ6-2026-10-15.md` e' sostituito con questo quadro; il
resto del file non e' toccato (la sezione 1 dice ancora "dati raccolti alle 10:45": l'ora delle 18:43
sta nel blocco). Il quadro si rifa: dopo le 22:00 se la cassa chiude con valore diverso da quello
scritto qui, dopo Warsh, e venerdi' 16/10 dopo le 21:30 con il COT nuovo.
