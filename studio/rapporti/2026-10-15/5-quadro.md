# Fase 5 · Il Quadro · 2026-10-15 (sera, dati delle 19:52)

**Stato**: fatto, su quattro rapporti: due nuovi e parziali (3-gamma, 4-orderflow), due riusati (1-macro
delle 18:43, 2-cot di stamattina). Nessuno manca. Rifa il quadro delle 18:43 (`precedenti/1952/5-quadro.md`).
**Dati**: raccolta delle **19:52** ora italiana (`raccolta.json`, tutte `ok`). **Macro**: `macro.json` delle 18:43,
senza gli esiti delle 14:30 (FRED saltato; PPI solo da un titolo CNBC), VXN al 14/10. **COT**: report del 6/10,
**9 giorni**. **Gamma**, solo NQ: GEX di NQZ6 delle **19:30-19:34** (17:30 UTC), OI (contratti aperti) di fine 14/10,
calcolo cambiato dalle 18:43 (120 strike per lato, zero a +-8%). **Order flow**: bridge alle 19:52 (17:52 UTC).
Controllati: `confluenze.json`, strike 29.700-29.950 (`gex-NQZ6.json`), `pannello.json`, `ore-NQZ6.json`. Il blocco
QUADRO della giornata e' sostituito; il resto del file no (la sua sezione 1 cita ancora i dati delle 10:45).

**In una riga**: macro e COT sono quelli delle 18:43; la gamma A resta negativa con gli zeri piu' vicini al
prezzo; la cassa ha raggiunto dal basso il valore di ieri e il pannello misura balance, delta e prezzo sul massimo.

## Il Quadro In Cinque Righe

1. **Macro (18:43, non riletta).** 10 anni 5,087% (-10 pb, pb = 0,01%) dopo un PPI caldo (titolo CNBC);
   Polymarket 27,5% per un rialzo il 28/10; VIX 16,94 in contango; QQQ meno RSP (S&P equipesato) -1,76 pp [1-macro].
2. **Posizionamento (6/10).** Non-Commercial (speculatori; Legacy, solo future) del Nasdaq: netto al 100° percentile
   a tre anni, short a 0,6, flusso ordinario (z 0,58). Dal 6/10 (31.480,75) alle 19:52: **-1.407 punti, -4,5%** (conto mio).
3. **Gamma (19:30).** GEX (milioni di $ di future che i dealer scambiano per 1% di movimento) -1.811,7 in convenzione A
   (fornitori), segno vero ignoto. Zeri gamma (dove la GEX cambia segno): tutte 30.498,8, 7 giorni 30.440,8, 0DTE
   (scade alle 22:00) 30.277,8. Muri (gamma call e put massima) a 30.000. Scala: 0,74% del volume medio di cassa.
4. **Valore.** Cassa (15:30-22:00) in corso: valore (70% del volume) 29.800-30.101, POC (prezzo di massimo volume)
   29.959. Quinto shift giu' (VAL e VAH sotto ieri), ma **non staccata**: 19 punti dentro ieri, 30.082-30.478 [4-orderflow].
5. **Adesso (19:52).** 30.073,75: sopra il POC, 8,25 sotto il VAL di ieri, arrivato dal basso (3 tocchi, passa 2). CVD
   (delta cumulato) +11.815 al 100% del range, prezzo al 94%. **BALANCE**, efficienza (netto su strada) 0,17 [pannello].

## Cosa E' Cambiato Dalle 18:43

| misura | 18:43 | 19:52 (gamma 19:30) |
|---|---|---|
| prezzo / valore di cassa | 29.946,25 / 29.792-30.060, staccato di 22 | 30.073,75 (max 30.102,25) / 29.800-30.101, sovrapposto di 19 |
| regime / CVD | CHOPPY 0,10 / +8.960 (CVD 95%, prezzo 74%) | BALANCE 0,17 / +11.815 (100%, 94%) |
| GEX A / zeri tutte, 7 giorni, 0DTE, 16/10 | -1.747,6 / 30.752,1, 30.656,9, 30.314,3, 31.070,4 | -1.811,7 / 30.498,8, 30.440,8, 30.277,8, 30.573,1: fino a -497 in 50 minuti, calcolo cambiato |
| implicita ATM 0DTE / ritardo del COT | 24,5% / -1.534,5 (-4,9%) | 22,4% / -1.407 (-4,5%) |
| future del calcolo GEX contro bridge | 29.975,5 contro 29.946,25 | 30.021,0 contro 30.018,75: lo scarto si chiude |
| livelli a piu' fasi | 30.650-30.661 (tre fasi), 30.745-30.783, 31.065-31.070 | **cadono**; nuovi 30.498,8-30.513, 30.082-30.102, 29.750-29.760 |

| smentita delle 18:43 | esito alle 19:52 |
|---|---|
| il prezzo esce dal valore di cassa (sotto 29.792 o sopra 30.060) | **avverata** sopra (30.102,25); il valore si e' allargato con lui |
| la cassa riaccetta il valore del 14/10 (torna e resta sopra 30.082) | **a meta'**: lo stacco non c'e' piu'; "resta sopra" no, 8,25 sotto |
| il pannello non misura piu' CHOPPY | **avverata nella lettera** (BALANCE); balance e' ancora rotazione, la contraddizione con la gamma resta |
| zero della settimana a piu' di 15 da 30.650-30.661, o muro lontano da 30.000 | **avverata per lo zero** (30.440,8, 209 sotto); il muro resta a 30.000 |
| cede 29.622,5; COT del 16/10 | non avverata; non ancora uscito |
| 10 anni sopra 5,19%; RSP cede quanto QQQ; VIX9D sopra VIX3M | non valutabili: macro non riraccolta |

## I Livelli Confermati

Soglia **15 punti** su NQZ6 fra fasi diverse. Base `confluenze.json`; *quadro* = aggiunta dalle letture delle fasi.
**Stesso dato** = lo stesso numero del bridge letto da due fasi: conferma di lettura, non di fonte. Distanze dal
pannello delle 19:52. Nessun livello e' validato come supporto o resistenza.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.483-31.556 (+1.409) | COT, OF (stesso dato) | valore del 6/10, netto al massimo; POC 31.530 | posizionamento: il prezzo dell'ultima foto COT |
| 30.888-30.980 (+814) | COT, OF (stesso dato) | valore del 22/09 (z 1,96, settimana di scadenza); POC 12/10 e 25/09 | posizionamento: il prezzo del cambio piu' grande |
| 30.549-30.657 (+475) | COT, OF (stesso dato) | valore del 29/09, unica variazione negativa; VAL 01/10 e 23/09 | posizionamento, debole |
| **30.498,8-30.513 (+425)** | gamma, OF | zero A di tutte; VAL di cassa 28/09 (30.506) e 21/09 (30.513) | posizionamento: sopra, la GEX A totale cambia segno nei valori di fine settembre. **Nuovo**, fonte indipendente |
| 30.173-30.185 (+99) | macro, OF (stessa seduta) | chiusura di cassa del 14/10; POC del 14/10, 0 tocchi oggi | riferimento: -111,25 (-0,369%) dalla chiusura; variabile di BDLM alle 21:30 |
| **30.082-30.102 (+8)** | OF, gamma (*quadro*) | VAL di ieri, VAH di oggi 30.101, massimo 30.102,25; 30.100, primo strike con netta A positiva | invalidazione della descrizione: accettare o no il valore di ieri. **Nuovo** |
| 29.990-30.000 (-74) | gamma, OF (*quadro*) | muri di tutte, settimana, 16/10, primo peso B; fascia da 10 punti al 5,5% del volume di oggi | posizionamento: il peso massimo nelle due convenzioni, nel nodo alto di oggi |
| 29.950-29.959 (-115) | gamma, OF | netta A -94,1, terzo peso B dello 0DTE; POC di cassa (39 tocchi, 5.310 lotti) | posizionamento: il centro di oggi; la parte 0DTE scade alle 22:00 |
| 29.777-29.815 (-274) | macro, OF (stesso bridge) | POC dalle 00:00 29.800 (18:43); VAL di cassa 29.800, VAL e POC Europa | posizionamento: bordo basso di oggi |
| 29.750-29.760 (-319) | gamma, OF | netta A -71,3; VAL della notte 29.753 (collettore), VAH 17/09 29.760 | posizionamento, debole: il pannello ha un'altra notte. **Nuovo** |
| 29.694-29.711 (-377) | gamma, OF, macro | netta A -93,3; VAL 18/09 29.694, minimo Europa 29.711; VAL dalle 00:00 29.700 | posizionamento: l'ultimo valore di cassa prima del 21/09 |
| 29.622,5 (-451) | macro, OF (stesso dato) | minimo di oggi, ora delle 15 | posizionamento: sotto, nessun valore di cassa in 20 sedute |
| 29.250 (-824) | COT, gamma (*quadro*) | POC del 15/09; put 29.250, OI maggiore dello 0DTE (508) | debole: la parte 0DTE scade alle 22:00 |

**Fuori soglia.** Zero 7 giorni 30.440,8 (15,2 dal VAL del composito 30.456); zero 16/10 30.573,1 (16,1 da 30.557).

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| macro 1: tecnologia, COT contro rotazione | **contraddizione** | lunghi al massimo sull'indice che resta indietro; nessun dato nuovo |
| macro 2: VIX in calo e contango, e la gamma? | **contraddizione** | implicita 0DTE 22,4% in calo, realizzata a 4 ore 32,9% [`ore-NQZ6`]; VXN fermo a ieri |
| macro 3: aggressione in acquisto? il valore regge? | **conferma** | delta positivo ogni ora dalle 15; 29.622,5 intatto. Apertura o chiusura non si distingue |
| macro 4: Warsh alle 05:30 | **conferma** | implicita del 16/10 24,35%, 1,30 volte la realizzata a 20 sedute |
| COT 1: netto al massimo contro la macro | **contraddizione** | come macro 1; S&P e Russell con i venditori sull'acceleratore |
| COT 2: valori COT contro muri e zeri | **cade** la conferma delle 18:43 | zero 7 giorni a 209 dal VAH del 29/09; zero 16/10 a 16,1 dal VAL; resta 29.250, debole |
| COT 3: sopra o sotto il cuore del volume | **sopra**, invariato | 1.663,25 sopra il POC a 24 ore (29.817,50), 710,75 sopra quello di cassa (30.770) |
| COT 4: il delta dopo il 6/10 conferma la rottura? | **contraddizione** | quinta seduta di shift giu' con CVD di cassa +11.815 oggi |
| gamma 1: 30.000 e l'order flow | **conferma** | dentro il valore di oggi, sulla fascia 29.990-30.000 (5,5% del volume) |
| gamma 2: zeri vicino al VAH di ieri, e i COT? | niente dal COT; **conferma** dall'OF | 30.478 fuori soglia (20,8 e 37,2); lo zero di tutte cade sui VAL del 28/09 e 21/09 |
| gamma 3: A negativa e scala piccola, e l'OF? | **contraddizione**, attenuata | BALANCE contro "amplifica"; la copertura e' lo 0,47% del volume di oggi. **Osservazione** |
| OF 1: muri nel nodo alto, zeri nel transito e sui VAL | **conferma** | verificato in `confluenze.json` e `gex-NQZ6.json` |
| OF 2: realizzata 32,9% e GEX negativa | coerenza apparente | le 4 ore contengono la salita 15:30-17:00; ipotesi mai provata su NQ |
| OF 3: valori COT sopra il prezzo | **conferma** | nove confluenze COT-OF, tutte `stesso_dato`; il 15/09 e' sotto |
| OF 4: delta orario, rileggere dopo Warsh | **conferma** | ora delle 18: -222 in 44 minuti (macro), +535 intera (OF): finestre diverse |

## Cosa Dicono Per NQ

1. **Tre scale, tre posizioni** (`profile-framing.md` §10): cassa a 20 sedute discount, 24 ore fair value, settimana di momentum.
2. **Il posizionamento e' vecchio quanto il calo**: lunghi sopra il cuore del volume su tutte e due le scale (**osservazione**).
3. **La gamma e' piu' vicina ma meno stabile**: il "dove" (30.000) regge alle due convenzioni, il segno no.
4. **Il delta ora ottiene prezzo** e la cassa tocca il valore di ieri, prima volta dal 12/10: si rilegge alle 22:00.

## Le Contraddizioni

- **COT contro macro**: speculatori al massimo sull'indice che la rotazione vende (foto del 6/10).
- **Gamma contro order flow**: A negativa (il dealer amplifica) contro BALANCE; la scala dice effetto piccolo.
- **Valore contro delta**: quinta seduta di shift giu' con delta di cassa in acquisto.
- **Volatilita'**: implicita 0DTE e VIX in calo contro realizzata a 4 ore 32,9% (a 24 ore 22,7%).
- **Inflazione contro tassi lunghi** (18:43): PPI caldo, rialzo piu' probabile, 10 anni -10 pb.
- **Gli zeri gamma**: -253 (tutte) e -497 (16/10) in 50 minuti, con un calcolo cambiato in mezzo.
- **Il giro**: "massimi e minimi decrescenti" degli ultimi due swing H1, con il prezzo sopra i due massimi.
- **Dalle 11:04**: POC 14/10 30.173 (M1) contro 30.187 (Range); rollover +299,25 contro +293,75; POC della
  notte 29.815 (collettore, 00:00-15:30) contro 29.920 (pannello, finestra non scritta).

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 15/10 21:30-22:00 | ultima mezz'ora della cassa | finestra di BDLM (momento con gamma negativa, S&P), mai provata su NQ |
| 15/10 22:00 | chiusura della cassa; scade lo 0DTE | profilo chiuso: sovrapposizione e shift si rimisurano; esce il 39% della A (-713,4) |
| 15/10 22:00 - 16/10 15:30 | notte prima del terzo venerdi' | finestra di BTW (misurata su ES, mai su NQ) |
| 16/10 05:30 | discorso di Warsh, presidente della Fed | tassi e pannello: il quadro si rilegge |
| 16/10 21:30 | COT, posizioni di martedi' 13/10 | il quadro si rifa; il 14 e il 15/10 non ci sono |
| 16/10 21:30-22:00 | ultima mezz'ora e scadenza mensile (-528,5 di A) | finestra di GJ (pinning); dopo, a OI fermo, la A resta -569,9 |

## Cosa Smentirebbe Il Quadro

- La cassa **resta sopra 30.082-30.102** (cade "sotto ieri") o **chiude con il VAH sotto 30.082** (torna staccata).
- **Cede 29.800** (VAL di oggi) o **29.622,5**; il pannello **non misura piu' BALANCE**; CVD e prezzo si separano.
- **Una lettura GEX** con lo zero di tutte a piu' di 15 da 30.498,8-30.513, il muro lontano da 30.000, o la A positiva.
- **Macro riletta**: 10 anni sopra 5,19% con NQ in calo, RSP cede quanto QQQ, VIX9D sopra VIX3M. **COT del 16/10**:
  netto ancora al massimo, o in forte calo.

## Cosa Non Sappiamo

Il segno dei dealer; se l'OI che cambia fra le riletture e' una catena che arriva; se i lunghi del 6/10 ci sono
ancora; se il recupero e' apertura di acquisti o chiusura di vendite; gli esiti delle 14:30. Sei livelli su tredici
sono conferme di lettura. Il quadro si rifa dopo le 22:00, dopo Warsh e dopo il COT del 16/10.

## Le Lacune Del Collettore

Risolte dalle 18:43: `confluenze.json`, `oggi_cassa`, `raccolta.json` riscritto, catena a 120 strike, zero a +-8%.
- lacuna del collettore: macro assente dalla raccolta delle 19:52; FRED (`FRED_API_KEY`); dato uscito degli eventi;
  calendario della settimana prossima (404); 2A, 30A, VXN e VIX3M intraday; FedWatch; ampiezza; trimestrali; notizie.
- lacuna del collettore: COT, `settimana_di_rollover` solo per l'ultimo report; ritardo in prezzo; profilo della
  settimana coperta; TFF senza variazioni separate, z52, percentili, ultime quattro.
- lacuna del collettore: data e ora dell'OI; OI oltre 120 strike (15/10-20/10); zeri per 20/10, 27/10, 29/10;
  settimanali dal 21/10 a pezzi (15,6% della A); volume opzioni alle 17:00 di Chicago; variabile di BDLM.
- lacuna del collettore: `confluenze.json` non dichiara i 101 livelli ne' il criterio di "strike pesante":
  30.100 (a 1 punto dal VAH 30.101), zeri per orizzonte, compositi e fasce dei profili non ci sono.
- lacuna del collettore: composito di cassa a 90 giorni (30 sedute, 6 prima del rollover); notte del pannello;
  "copertura", "rottura rientrata", veti; big trade per livello di nuovo `null`; swing del giro; una misura per numero.
- Non e' una lacuna: la realizzata intraday che la fase 3 cerca sta in `ore-NQZ6.json` (`volatilita_realizzata`).
