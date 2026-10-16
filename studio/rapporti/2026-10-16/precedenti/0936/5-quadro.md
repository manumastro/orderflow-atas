# Fase 5 · Il Quadro · 2026-10-16 (mattina, dati delle 09:31-09:36)

**Stato**: fatto, su quattro rapporti: tre nuovi (1-macro parziale, 3-gamma parziale, 4-orderflow), uno
**riusato** (2-cot, scritto il 15/10 sui dati delle 10:45). Nessuno manca. Primo quadro di oggi.
**Dati**: raccolta chiusa alle **09:36** ora italiana (`raccolta.json`, tutte `ok`, 102 s). **Macro**: `macro.json`
delle 09:35; la fase 1 e' partita prima che `raccolta.json` esistesse. **COT**: report di martedi' **6/10**,
pubblicato il 9/10, **10 giorni**; `cot.json` di oggi (09:34) porta lo stesso report (controllato: `data` 2026-10-06),
quindi `2-cot.md` del 15/10 vale ancora, tranne il suo "prezzo adesso" (29.822,5 delle 10:51 del 15/10), qui
sostituito. Il nuovo COT esce stasera alle 21:30. **Gamma**, solo NQ: GEX di NQZ6 delle **09:31** (07:31 UTC), OI
(contratti aperti) di fine 15/10. **Order flow**: bridge alle 09:34-09:36 (07:34-07:36 UTC). Controllati:
`in-breve.json`, `confluenze.json` (soglia 15 punti, 98 livelli letti), `cot.json`, `raccolta.json`.

**In una riga**: macro con dati caldi ma Fed percepita meno aggressiva; COT vecchio di 10 giorni con i lunghi al
massimo sopra il prezzo; gamma A quasi neutra con tre zeri a ridosso del prezzo; il prezzo e' sopra il valore di
ieri, dentro il blocco 14-15/10, in un regime che il pannello misura CHOPPY.

## Il Quadro In Cinque Righe

1. **Macro (09:35).** CPI e PPI di settembre sopra l'attesa; dopo Warsh il rialzo del 28/10 scende al 12,5%
   (Polymarket; future fed funds 12,2%); 10 anni 5,10% (-9 pb, pb = 0,01%) al 15/10; VIX 16,02 in contango
   (VIX9D < VIX < VIX3M); HYG al percentile 2,4; Nasdaq-100 mediana +1,19%, pesata -0,09% [1-macro].
2. **Posizionamento (6/10, 10 giorni).** Non-Commercial (speculatori; Legacy, solo future) del Nasdaq: netto al
   100° percentile a tre anni, short a 0,6, flusso ordinario (z 0,58). Dalla chiusura del 6/10 (31.480,75) alle
   09:34 (30.165,75): **-1.315 punti, -4,2%** (sottrazione mia sui due numeri) [2-cot, pannello].
3. **Gamma (09:31).** GEX (milioni di $ di future che i dealer scambiano per 1% di movimento), convenzione A
   (fornitori; segno vero ignoto): tutte -199,8, 7 giorni -73,7, 0DTE (scade alle 22:00) **+71,7**. Zeri (dove la
   GEX cambia segno) 30.230,6 / 30.190,7 / 30.135,0. Put wall e primo peso B a 30.000. Scala 0,08% del volume [3-gamma].
4. **Valore.** Ieri cassa (15:30-22:00) 29.779-30.115, POC 29.970: quarto shift giu' (VAL e VAH sotto l'altro ieri),
   non staccato, chiuso **sopra** il proprio valore a 30.140,25 con delta +4.909. Notte 30.049-30.207, POC 30.190.
   Composito di sola cassa (31 sedute, POC 30.770): discount; composito a 24 ore: fair value [4-orderflow].
5. **Adesso (09:34-09:36).** 30.165,75, dentro il valore di Londra 30.160-30.186, sotto il suo POC 30.173; CVD
   (delta cumulato dalle 22:00) +432 su 93.696 lotti; ultimi due swing H1 con massimi e minimi crescenti;
   **CHOPPY**, efficienza (netto su strada) 0,01 in due letture [pannello, giro].

## Cosa E' Cambiato Dal Quadro Di Ieri (19:52)

| misura | 15/10 19:52 | 16/10 09:31-09:36 |
|---|---|---|
| prezzo / valore di cassa del 15/10 | 30.073,75 / 29.800-30.101 in corso | 30.165,75 / chiuso 29.779-30.115, chiusura 30.140,25 sopra |
| regime | BALANCE 0,17 | CHOPPY 0,01 |
| GEX A tutte / zeri tutte, 7 giorni | -1.811,7 / 30.498,8, 30.440,8 | -199,8 / 30.230,6, 30.190,7 (0DTE del 15/10 scaduto) |
| rialzo 28/10 (Polymarket) / ritardo COT | 27,5% / -4,5% | 12,5% / -4,2% |

Le smentite di ieri: "la cassa resta sopra 30.082-30.102" **avverata** alla chiusura (30.140,25); "zero di tutte a
piu' di 15 da 30.498,8" **avverata** (30.230,6); "cede 29.622,5" **non avverata** (minimo notte 29.959,25).

## I Livelli Confermati

Soglia **15 punti** su NQZ6 fra fasi diverse. Base `confluenze.json`; *quadro* = aggiunta dalle letture delle fasi.
**Stesso dato** = lo stesso numero del bridge letto da due fasi: conferma di lettura, non di fonte. Distanze dal
pannello delle 09:34 (30.165,75). Nessun livello e' validato come supporto o resistenza.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.483-31.556 (+1.317) | COT, OF (stesso dato) | valore del 6/10, netto al massimo; POC 31.530 | posizionamento: il prezzo dell'ultima foto COT |
| 30.888-30.980 (+722) | COT, OF (stesso dato) | valore del 22/09 (z 1,96, settimana di scadenza); POC 12/10 30.890 | posizionamento: il prezzo del cambio piu' grande |
| 30.549-30.657 (+383) | COT, OF (stesso dato) | valore del 29/09, unica variazione negativa; VAL 01/10 e 23/09 | posizionamento, debole |
| **30.225-30.231 (+59)** | gamma, OF (*quadro*) | zero A di tutte 30.230,6; massimo notte 30.225, ultimo swing H1 | posizionamento: sopra, la GEX A totale cambia segno fuori dalla notte. **Nuovo** |
| **30.190-30.200 (+24)** | gamma, OF | zero A 7 giorni 30.190,7, strike 30.200 (netta A +57,8, 280 call di volume); POC notte | posizionamento: il centro della notte coincide con un cambio di segno |
| **30.135-30.140 (-26)** | gamma, macro, OF (*quadro*) | zero A dello 0DTE 30.135,0; chiusura di cassa del 15/10 30.140,25 | riferimento: sotto, lo 0DTE e' negativo in A e il prezzo e' sotto la chiusura; vale fino alle 22:00 |
| **30.100-30.115 (-51)** | gamma, OF | call wall 0DTE, strike netta A +95,8, terzo peso B; VAH del 15/10 | invalidazione della descrizione: rientrare nel valore di ieri |
| 29.990-30.000 (-166) | gamma, OF (*quadro*) | put wall ovunque, primo peso B; fascia da 10 punti piu' pesante di ieri (5,0%) | posizionamento: il peso massimo nelle due convenzioni, nel nodo di ieri |
| 29.959-29.970 (-196) | macro, OF (stesso bridge) | minimo notte 29.959,25 (ora 02); POC 15/10 29.970 | posizionamento: sotto, cade la sequenza di minimi H1 crescenti |
| 29.694-29.700 (-466) | gamma, OF | strike 29.700, netta A -57,7; VAL di cassa 18/09 | posizionamento, debole |
| 29.622,5 (-543) | macro, OF (stesso dato) | minimo del 15/10 | posizionamento: sotto, nessun valore di cassa in 20 sedute |

**Fuori soglia.** Call wall 30.250 a 25 da 30.225; strike 30.500 a 22 da 30.478 (VAH 14/10) e a 49 da 30.549;
zero 0DTE a 20 da 30.115; POC 15/10 29.970 a 30 da 30.000.

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| macro 1: rotazione contro il COT | **contraddizione** | lunghi al massimo sull'indice che la rotazione vende; foto del 6/10, prima del calo |
| macro 2: VIX in contango e la gamma | **conferma**, debole | A quasi neutra, scala 0,08%, realizzata 4 ore 14,0%; il premio sta solo sullo 0DTE (25,4%) |
| macro 3: chi ha comprato il recupero | **conferma** a meta' | delta di cassa 15/10 +4.909, notte +336; ora 21 -2.067. Apertura o chiusura non si distingue |
| macro 4: tasso giu', credito no | niente da dire | nessuna fase legge il credito; il COT e' anteriore al CDS di CoreWeave |
| COT 1: netto al massimo contro la macro | **contraddizione** | come macro 1; S&P e Russell con i venditori sull'acceleratore |
| COT 2: valori COT contro muri e zeri | **niente** | il piu' vicino: strike 30.500 a 49 da 30.549; zeri tutti sotto 30.231 |
| COT 3: sopra o sotto il cuore del volume | **sopra** | 31.483-31.556 contro POC cassa 30.770 (+713) e POC 24 ore 29.517,5 (aggiustato 29.817,5) |
| COT 4: il delta dopo il 6/10 conferma? | **contraddizione** | quattro shift giu' con delta di cassa -1.110, +1.248, -270, +530, +4.909 |
| gamma 1: 30.000 e l'order flow | **conferma** per la zona | fascia 29.990-30.000 la piu' pesante di ieri; POC 29.970 e VAL notte 30.049 oltre soglia |
| gamma 2: zeri dentro il valore della notte | **conferma** | 30.190,7 sul POC 30.190 (`confluenze.json`); 30.135 e 30.231 dentro 29.959,25-30.225 |
| gamma 3: 30.100 e VAH 30.115 | **conferma** | due fonti indipendenti; nella notte la fascia 30.075-30.100 ha delta -601 [giro] |
| gamma 4: A neutra, 0DTE 1,28 volte la realizzata | **coerente** con la macro | VIX basso e contango; il premio dello 0DTE e' la scadenza mensile. **Osservazione** |
| OF 1: 30.100-30.115 | **conferma** | come gamma 3 |
| OF 2: zero di tutte e massimo notte | **conferma** | 30.230,6 contro 30.225: 5,6 punti; non in `confluenze.json` |
| OF 3: confluenze COT tutte stesso dato | **conferma** | sette righe COT-OF, tutte `stesso_dato` vero; il discount contro il netto: vedi COT 4 |
| OF 4: CHOPPY, realizzata e implicita | **coerente** | realizzata 14,0-21,3%, implicita dal 19/10 15,2-18,2%; solo lo 0DTE sopra |

## Cosa Dicono Per NQ

1. **Tre scale, tre posizioni** (`profile-framing.md` §10): composito di cassa discount, 24 ore fair value,
   settimana con valore in discesa ma ieri chiusa sopra il proprio valore.
2. **Il posizionamento e' vecchio quanto il calo**: lunghi sopra il cuore del volume su due scale (**osservazione**).
3. **La gamma si e' avvicinata al prezzo**: tre zeri in 96 punti attorno a lui, dentro un terzo della mossa attesa
   di oggi (+-288,9); il "dove" (30.000, 30.100) regge alle due convenzioni, il segno no.
4. **Il prezzo sta nella notte, sopra ieri**, sul POC della seduta della rottura (30.173, 14/10), senza ritmo.

## Le Contraddizioni

- **COT contro macro**: speculatori al massimo sull'indice che la rotazione vende (foto del 6/10, 10 giorni).
- **Valore contro delta**: quattro sedute di shift giu' con delta di cassa per lo piu' positivo.
- **Composito**: discount sulla sola cassa (31 sedute, non 90), fair value sulle 24 ore. Non si sceglie.
- **Inflazione contro tassi**: CPI e PPI caldi, 10 anni -9 pb e rialzo del 28/10 meno probabile.
- **Gamma A**: totale negativa, 0DTE positiva; il 75% della A totale viene da catene quotate a pezzi.
- **Volatilita'**: 0DTE 25,4% contro VIX 16,02 e realizzata a 4 ore 14,0%.
- **Due misure per un numero**: POC 14/10 30.173 (M30) contro 30.187 (Range); VAL notte 30.049 contro 30.045.

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 15:30 | apertura della cassa; fine della finestra notturna di BTW (ES, mai provata su NQ) | primo profilo di cassa: sopra o dentro il valore di ieri |
| 21:30 | COT, posizioni di martedi' 13/10 | la riga 2 si rifa: il libro ridotto o no dopo il calo; 14 e 15/10 esclusi |
| 21:30-22:00 | ultima mezz'ora; finestre di BDLM e GJ (pinning), su S&P | descrittivo; la variabile di BDLM non esiste alle 09:36 |
| 22:00 | chiusura; scade lo 0DTE (OI 25.069, A +71,7, 67,6% della B) | a OI fermo la A totale va a -271,5 (sottrazione mia) |
| 20-21/10 · 28/10 19:00 | NFLX, META, TSLA · FOMC e GOOGL, MSFT | fuori dalla giornata; nessun altro evento USD oggi |

## Cosa Smentirebbe Il Quadro

- La cassa **costruisce valore sotto 30.115** (torna dentro ieri) o **sopra 30.478** (esce dal blocco 14-15/10).
- **Cede 29.959,25** (minimi H1 crescenti) o 29.622,5; il pannello **non misura piu' CHOPPY**.
- **Una GEX nuova** con gli zeri fuori da 30.135-30.231 di piu' di 15 punti, o il put wall lontano da 30.000.
- **COT delle 21:30**: netto ancora al massimo dopo il -4,2%, o short in forte aumento.
- **Macro**: 10 anni sopra 5,19%; VIX9D sopra VIX3M; HYG sotto 76,81 (nuovo minimo).

## Cosa Non Sappiamo

Il segno dei dealer; se i lunghi del 6/10 ci sono ancora; se il recupero del 15/10 e' apertura di acquisti o
chiusura di vendite; perche' lo 0DTE del terzo venerdi' risulta "Weekly". Quattro livelli su undici sono conferme
di lettura (stesso dato). Il quadro si rifa alle 15:30, dopo il COT delle 21:30 e se il prezzo esce dal valore.

## Le Lacune Del Collettore

- lacuna del collettore: macro, FRED `DFF` e `STLFSI4` in timeout; VIX9D, VIX3M, VXN, ^TNX solo alla chiusura del
  15/10; `2YY=F` fermo al 13/10; calendario della settimana prossima (404); testo di Google News.
- lacuna del collettore (orchestrazione): la fase 1 e' partita prima di `raccolta.json`.
- lacuna del collettore: COT, `settimana_di_rollover` solo per l'ultimo report; ritardo in prezzo; profilo della
  settimana coperta; TFF senza variazioni separate, z52, percentili, ultime quattro; nessun campo di eta'/riuso.
- lacuna del collettore: gamma, data e ora dell'OI; `strike_per_lato`, `volume_finestra`, `per_scadenza` non in
  `in-breve`; 02/11 e 06/11 saltate senza motivo; nessuno zero per il 29/10; catene dal 21/10 a pezzi (18/12
  90/364); VXN e realizzata a 20 sedute non in `in-breve`; variabile di BDLM.
- lacuna del collettore: order flow, delta per fascia nei profili di cassa; composito "90" di 31 sedute con
  aggiustamento non dichiarato; durata delle barre range con le pause; POC 14/10 diverso fra M30 e Range.
- lacuna del collettore: `confluenze.json` non legge estremi della notte, chiusura di ieri, fasce dei profili ne'
  la macro: 30.225-30.231, 30.135-30.140 e 29.990-30.000 li ha aggiunti il quadro; i 98 livelli non sono elencati.
