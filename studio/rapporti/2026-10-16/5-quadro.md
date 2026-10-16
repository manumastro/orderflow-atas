# Fase 5 · Il Quadro · 2026-10-16 (pomeriggio, dati delle 15:27-15:32)

**Stato**: fatto, su quattro rapporti: tre nuovi e parziali (1-macro 15:29, 3-gamma 15:27, 4-orderflow 15:32),
uno **riusato** (2-cot, scritto il 15/10 alle 10:45). Nessuno manca. Secondo quadro di oggi; il primo (09:36) e'
in `precedenti/0936/5-quadro.md`.
**Dati**: raccolta chiusa alle **15:32** ora italiana (`raccolta.json`, tutte `ok`, 202,7 s). **Macro**: `macro.json`
delle 15:29. **COT**: report di martedi' **6/10**, pubblicato il 9/10, **10 giorni**; `cot.json` raccolto alle 09:34
porta lo stesso report, quindi `2-cot.md` del 15/10 vale ancora, tranne il suo "prezzo adesso" (29.822,5 del 15/10),
qui sostituito. Il COT nuovo esce **stasera alle 21:30**. **Gamma**, solo NQ: GEX delle **15:27** (13:27 UTC).
**Order flow**: pannello e sedute 15:29, giro 15:32 (13:29-13:32 UTC); la cassa (15:30-22:00) ha **due minuti**.
Controllati: `in-breve.json`, `confluenze.json` (soglia 15 punti, 98 livelli letti), `raccolta.json`.
Prezzo di riferimento: **30.012,25** (bridge, M1 delle 15:32:33; future della GEX 30.013,50).

**In una riga**: tassi un poco su e rialzo di ottobre un poco piu' probabile; COT vecchio di 10 giorni con i lunghi
al massimo sopra il prezzo; gamma A negativa ovunque col prezzo sotto tutti gli zeri e a 12 punti dal 30.000; il
prezzo e' rientrato nel valore di ieri e la cassa e' appena nata, in un regime che il pannello misura CHOPPY.

## Il Quadro In Cinque Righe

1. **Macro (15:29).** Rialzo di 25 pb il 28/10 al **15,5%** (future fed funds ZQ; Polymarket 13,5%; stamattina
   12,2%), 10 anni 5,121% (+3,2 pb; pb = 0,01%), VIX 15,81 in calo (ultima struttura misurata: contango ieri sera),
   dollaro 101,79 fermo, HYG al minimo dell'anno (dato del 15/10); NQ -117 dalle 10 senza notizia di peso [1-macro].
2. **Posizionamento (6/10, 10 giorni, riusato).** Non-Commercial (speculatori; Legacy, solo future) del Nasdaq: netto
   al 100° percentile a tre anni, short a 0,6, flusso ordinario (z 0,58). Dalla chiusura del 6/10 (31.480,75) alle
   15:32 (30.012,25): **-1.468,5 punti, -4,7%** (sottrazione mia sui due numeri) [2-cot, bridge].
3. **Gamma (15:27).** GEX = milioni di $ di future che i dealer (chi vende le opzioni) scambiano per coprirsi a ogni
   1% di movimento. Convenzione A (quella dei fornitori; il segno vero non si conosce): tutte -657,2, 7 giorni
   -519,9, 0DTE (scade alle 22:00) **-278,4**; zeri 30.181,0 / 30.150,3 / 30.096,0, tutti **sopra** il prezzo.
   **In parole semplici**: sotto lo zero i dealer, per restare coperti, venderebbero quando il prezzo scende e
   comprerebbero quando sale, cioe' **allargherebbero i movimenti invece di frenarli**. Il 30.000 e' il punto dove
   pesano piu' opzioni (muro delle call e delle put insieme), 12 punti sotto il prezzo. Ma la copertura vale lo
   **0,26%** del volume medio di cassa: un effetto piccolo, e tutto dipende da un segno ipotetico [3-gamma].
4. **Valore.** Ieri cassa 29.779-30.115, POC 29.970, quarto shift giu', chiusa sopra a 30.140,25. Il blocco 14-15/10
   resta unito, 29.779-30.478. Notte chiusa 30.030-30.167, POC 30.060 (stamattina 30.190). Composito di sola cassa
   (31 sedute, POC 30.770): discount; composito a 24 ore: fair value [4-orderflow].
5. **Adesso (15:32).** 30.012,25, **dentro il valore di ieri**, sopra il suo POC; cassa di due minuti 29.977-30.013,
   POC 29.989; minimo 15:31 29.962,50; CVD (delta cumulato) dall'apertura +964 su 21.128 lotti; velocita' al 97°-98°
   percentile; **CHOPPY**, efficienza (netto su strada) 0,09 [4-orderflow, giro].

## Cosa E' Cambiato Dal Quadro Delle 09:36

| misura | 09:31-09:36 | 15:27-15:32 |
|---|---|---|
| prezzo / rispetto al valore di ieri | 30.165,75 / sopra il VAH 30.115 | 30.012,25 / **dentro** |
| GEX A tutte / 0DTE | -199,8 / +71,7 | **-657,2 / -278,4** |
| zeri A tutte / 7 giorni / 0DTE | 30.230,6 / 30.190,7 / 30.135,0 | 30.181,0 / 30.150,3 / 30.096,0 |
| call wall 0DTE / scala A | 30.100 / 0,08% del volume | 30.000 / 0,26% |
| rialzo 28/10 (ZQ) / 10 anni / VIX | 12,2% / 5,089% / 16,02 | 15,5% / 5,121% / 15,81 |
| POC notte / regime / velocita' | 30.190 / CHOPPY 0,01 / 42°-58° pct | 30.060 / CHOPPY 0,09 / 97°-98° pct |
| ritardo COT | -4,2% | -4,7% |

OI quasi fermo (-1,21%): la gamma e' cambiata col prezzo, non con posizioni nuove provate [3-gamma].

**Le smentite delle 09:36.**
- "La cassa costruisce valore **sotto 30.115** (torna dentro ieri)": **avverata per il prezzo**, rientrato
  nel valore di ieri gia' prima delle 15:30; il valore di cassa ha due minuti (29.977-30.013) e non e' ancora una
  distribuzione. "Sopra 30.478": non avverata.
- "Una GEX nuova con gli zeri fuori da 30.135-30.231 di piu' di 15 punti": **avverata per lo 0DTE** (30.096,0, 39
  sotto); gli altri due zeri sono dentro. "Put wall lontano da 30.000": non avverata.
- "Cede 29.959,25": **non avverata** (minimo 29.962,50 alle 15:31, 3,25 sopra). "Non piu' CHOPPY": non avverata.
- La riga 30.100-115 era indicata come "invalidazione della descrizione: rientrare nel valore di ieri": e' successo.
- Macro: 10 anni sotto 5,19% (non avverata); VIX9D/VIX3M e HYG non misurati oggi (non verificabili). COT: alle 21:30.

## I Livelli Confermati

Soglia **15 punti** su NQZ6 fra fasi diverse. Base `confluenze.json`; *quadro* = aggiunta dalle letture delle fasi.
**Stesso dato** = lo stesso numero del bridge letto da due fasi: conferma di lettura, non di fonte. Distanze da
30.012,25 (15:32). Nessun livello e' validato come supporto o resistenza (finding 6 della scheda opzioni-gamma-es).

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.483-31.556 (+1.471) | COT, OF (stesso dato) | valore del 6/10, netto al massimo; POC 31.530 | posizionamento: il prezzo dell'ultima foto COT |
| 30.888-30.980 (+876) | COT, OF (stesso dato) | valore del 22/09 (z 1,96, settimana di scadenza); POC 12/10 | posizionamento: il prezzo del cambio piu' grande |
| 30.549-30.657 (+537) | COT, OF (stesso dato) | valore del 29/09, unica variazione negativa | posizionamento, debole |
| **30.500-30.513 (+488)** | gamma, OF | strike 30.500, netta A +79,6, OI call 2.084 (OI salito 1.531 -> 2.775); VAL 28/09 e 21/09 | posizionamento: la gamma call piu' grande lontana. **Nuovo** |
| 30.197-30.200 (+185) | gamma, OF (*quadro*) | strike 30.200, netta A +49,4; massimo di swing H1 30.196,75 | posizionamento |
| **30.167-30.181 (+155)** | gamma, OF | zero A di tutte 30.181,0; VAH notte 30.167, POC 14/10 30.173 | posizionamento: sopra, la GEX A totale torna positiva |
| **30.100-30.126 (+88)** | gamma, OF | strike 30.100 netta A +113,8, secondo peso B; VAH 15/10 30.115; min swing H1 30.126 | bordo alto del valore di ieri: sopra, si esce di nuovo da ieri |
| **30.082-30.096 (+70)** | gamma, OF | zero A 0DTE 30.096,0; VAL 14/10 30.082 | posizionamento: sopra, lo 0DTE torna positivo in A; vale fino alle 22:00 |
| **29.989-30.013 (0)** | gamma, OF (*quadro*) | 30.000 call wall, put wall, primo peso B; cassa di 2 minuti VAL-VAH, POC 29.989 | il peso massimo dove nasce il valore di oggi; candidato al pinning (GJ, mai provato su NQ) |
| 29.959-29.970 (-42) | macro, OF (stesso bridge) | minimo notte 29.959,25; POC 15/10 29.970 | sotto, cade la sequenza di minimi H1 crescenti |
| 29.694-29.700 (-312) | gamma, OF | strike 29.700, netta A -99,4 (OI put 966); VAL cassa 18/09 | posizionamento, debole |

**Sciolti dalle 09:36**: 30.225-30.231 (zero di tutte sceso a 30.181) e 30.135-30.140 (zero 0DTE sceso a 30.096).
**Fuori soglia**: zero 7 giorni 30.150,3 a 16,7 da 30.167; peso B 29.800 a 21 da 29.779; strike 30.500 a 22 da
30.478 e a 49 da 30.549; zero di tutte 30.181 a 15,75 da 30.196,75.

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| macro 1: tassi su e COT, tono di NQ | niente per il COT; **coerente** per NQ | il COT e' del 6/10; tassi su e NQ giu' nelle stesse ore, abbinamento non causa |
| macro 2: VIX calmo e gamma | **contraddizione** | VIX in calo, ma A negativa ovunque (amplifica); il premio sta solo sullo 0DTE (1,46 volte la realizzata) |
| macro 3: chi spinge la deriva | **conferma a meta'** | ore 11-15 con delta -223/+239; fasce 30.025-30.100 della notte a delta negativo; CVD da ieri +579 |
| macro 4: rotazione e credito | niente da dire | nessuna fase legge ampiezza o credito |
| COT 1: netto al massimo contro la macro | **contraddizione** | lunghi al massimo sull'indice dei semiconduttori deboli; foto prima del calo |
| COT 2: valori COT contro muri e zeri | **niente** | strike 30.500 a 49 da 30.549; zeri tutti sotto 30.182 |
| COT 3: sopra o sotto il cuore del volume | **sopra** | 31.483-31.556 contro POC cassa 30.770 e POC 24 ore 29.517,5 (aggiustato 29.817,5) |
| COT 4: il delta dopo il 6/10 | **contraddizione** | valore giu' da quattro sedute con delta per lo piu' positivo (+1.248, -270, +530, +4.909) |
| gamma 1: 30.000 difeso o attraversato | **conferma per la zona** | la cassa nasce li' (POC 29.989); attraversato fino a 29.962,50 e ripreso: due minuti, troppo presto |
| gamma 2: zeri e livelli dell'OF | **conferma** | 30.082-30.096 e 30.167-30.181 in `confluenze.json`, dati diversi |
| gamma 3: A negativa e macro | **contraddizione** | come macro 2; scala 0,26% |
| gamma 4: COT e strike 30.500 | **niente** per il COT | 30.500 conferma invece con VAL 28/09 e 21/09 |
| OF 1: 30.000 e POC in sviluppo | **conferma** | aggiunto dal quadro; valore di due minuti |
| OF 2: 30.082-30.115 e 30.167-30.181 | **conferma** | come gamma 2 |
| OF 3: COT tutto stesso dato | **conferma** di lettura | sette righe COT-OF, tutte `stesso_dato` vero; discount contro netto: vedi COT 4 |
| OF 4: realizzata contro implicita | **coerente** con la gamma | realizzata 15,1-19,7%, 0DTE 28,6%, dal 19/10 sotto 1 |

## Cosa Dicono Per NQ

1. **Il prezzo e' tornato nel valore di ieri**: la prima chiusura sopra il proprio valore non ha avuto continuazione
   (`profile-framing.md`, passi 2 e 5). Dentro il blocco 14-15/10 il quadro e' una bilancia; CHOPPY coerente.
2. **La gamma si e' spostata col prezzo**: tre zeri sopra in 84-169 punti, dentro la mossa attesa di oggi (+-235).
3. **Il 30.000 e' il centro di oggi**: muri, primo peso B e il valore di cassa appena nato (**osservazione**).
4. **Il posizionamento resta vecchio quanto il calo**: lunghi sopra il cuore del volume su due scale.

## Le Contraddizioni

- **COT contro macro**: speculatori al massimo sull'indice che la rotazione vende (foto del 6/10, 10 giorni).
- **Valore contro delta**: shift giu' con delta di cassa per lo piu' positivo; oggi CVD all'89% del range, prezzo al 30%.
- **Gamma contro regime**: A negativa (amplifica) e mercato CHOPPY con VIX in calo. Il segno resta un'ipotesi.
- **Struttura H1**: il collettore dice minimi crescenti, il prezzo e' sotto il minimo di swing 30.126.
- **Composito**: discount sulla sola cassa (31 sedute), fair value sulle 24 ore. Non si sceglie.
- **Due misure per un numero**: cassa di oggi VAL 29.977 / 29.983, VAH 30.013 / 30.017 (pannello / M30); rialzo
  28/10 15,5% (ZQ) / 13,5% (Polymarket); ampiezza con etichetta "Oct 15" e numeri cambiati da stamattina.

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 15:30-16:30 | prima ora di cassa | il valore di oggi diventa una distribuzione: dentro, sopra o sotto ieri |
| 21:30 | COT, posizioni di martedi' 13/10 | la riga 2 si rifa: libro ridotto o no dopo il calo; 14 e 15/10 esclusi |
| 21:30-22:00 | ultima mezz'ora; BDLM e GJ (pinning), su S&P | descrittivo; la variabile di BDLM non esiste ancora |
| 22:00 | chiusura; scade lo 0DTE (OI 24.649, A -278,4, 68,2% della B) | a OI fermo la A totale va a -378,8 (sottrazione mia) |
| 20-21/10 · 28/10 19:00 | NFLX, META, TSLA · FOMC, GOOGL, MSFT | fuori dalla giornata |

## Cosa Smentirebbe Il Quadro

- La cassa **costruisce valore sopra 30.115** (esce di nuovo da ieri) o **sotto 29.779** (esce dal blocco).
- **Cede 29.959,25** (minimi H1 crescenti); il pannello **non misura piu' CHOPPY**.
- **Una GEX nuova** con put wall o call wall lontani da 30.000, o la A totale positiva.
- **COT delle 21:30**: netto ancora al massimo dopo il -4,7%, o short in forte aumento.
- **Macro**: 10 anni sopra 5,19%; VIX9D sopra VIX3M; HYG sotto 76,81.

## Cosa Non Sappiamo

Il segno dei dealer; se i lunghi del 6/10 ci sono ancora; che forma prendera' una cassa di due minuti; se l'OI
cambiato in giornata e' posizione nuova o catena letta diversamente. Cinque livelli su undici sono conferme di
lettura (stesso dato). Il quadro si rifa dopo il COT delle 21:30 e dopo la chiusura delle 22:00.

## Le Lacune Del Collettore

- lacuna del collettore: macro, VIX9D, VIX3M e VXN di oggi (ultimo dato 15/10); `2YY=F` fermo al 13/10; curva
  Treasury di oggi; calendario della settimana prossima; testo di Google News; ampiezza con data incerta.
- lacuna del collettore (orchestrazione): la fase 1 ha letto un `raccolta.json` non ancora riscritto.
- lacuna del collettore: COT, `settimana_di_rollover` solo per l'ultimo report; ritardo in prezzo; profilo della
  settimana coperta; TFF senza variazioni separate, z52, percentili, ultime quattro; nessun campo di eta'/riuso.
- lacuna del collettore: gamma, max pain; data e ora dell'OI; `volume_finestra`, `strike_per_lato`, `per_scadenza`
  non in `in-breve`; 06/11 saltata senza motivo; 02/11 con OI 0; VXN e realizzata a 20 sedute; variabile di BDLM.
- lacuna del collettore: order flow, `oggi_cassa` vuoto (sedute raccolte alle 15:29); regime e veti solo nel giro;
  rottura in chiusura H1 di 30.126 non detta; delta per fascia di cassa; composito "90" di 31 sedute.
- lacuna del collettore: `confluenze.json` non legge cassa in sviluppo, swing H1, estremi della notte ne' macro:
  29.989-30.013 e 30.197-30.200 li ha aggiunti il quadro; i 98 livelli non sono elencati.
