# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-10-15

**Stato**: parziale (il composito non e' di sola cassa; mancano i profili per fascia del composito e
di ieri, i big trade sui livelli e lo shift letterale: vedi Cosa Non Sappiamo)
**Dati**: `tab-ruoli.json`, `quattro-tab.txt`, `sedute-NQZ6.json`, `ore-NQZ6.json`, `pannello.json`,
`giro.txt`; per gli agganci solo le date di `cot.json` e la riga GEX del giro. Raccolta alle 10:51
ora italiana (`raccolta.json`), nessun errore. Pannello alla barra delle 10:45 (08:45 UTC); GEX alle
10:36 (OI di ieri); COT al martedi' 6 ottobre (9 giorni). Il file della giornata
`NQZ6-2026-10-15.md` non esiste e l'ultimo quadro di framing e' del 24 settembre (`giro.txt`, sez. 2 e 4bis).

**In una riga**: tre sedute di cassa con il valore spostato in giu' e staccato; la notte e' scesa
sotto il valore di ieri e si e' fermata sul POC del composito aggiustato di 90 giorni, dove ora ruota
in un regime che l'indicatore misura come choppy.

Convenzioni usate in tutto il rapporto: cassa = 15:30-22:00 ora italiana (13:30-20:00 UTC); **POC**
= il prezzo con il volume massimo; **area di valore** (VAL il bordo basso, VAH l'alto) = il 70% del
volume attorno al POC; **delta** = contratti aggrediti in acquisto meno in vendita; **CVD** = il delta
cumulato. Prezzi NQZ6 salvo dove scritto.

## I Fatti

### Tab 1 · Composito (NQ Daily continuo, 90 barre dal 2026-06-11, grana 5; `tab-ruoli.json`)

| versione | VAL | VAH | POC | prezzo 29.822,50 |
|---|---|---|---|---|
| grezzo (continuo come incollato da ATAS) | 28.410 | 30.550 | 29.517,50 | fair value |
| aggiustato (+299,25 sui giorni prima del 2026-09-11) | 29.340 | 30.820 | 29.817,50 | fair value |

Le barre sono di 24 ore, non di sola cassa (nota del collettore nel JSON). Ultime barre daily
(`quattro-tab.txt`): mer 14 range 814, volume 729k, delta +1.572; gio 15, in corso, 30.154,75 ->
29.822,50, range 404, delta -2.416.

### Tab 2 · Settimana (NQZ6, valore di cassa per giorno; `sedute-NQZ6.json`, M1 fasce da 1)

| seduta | VAL-VAH | POC | chiusura | volume | delta | chiude | staccato |
|---|---|---|---|---|---|---|---|
| 06/10 | 31.483-31.556 | 31.530 | 31.480,75 | 348.459 | -3.557 | sotto | si' |
| 07/10 | 31.245-31.415 | 31.353 | 31.395,75 | 361.070 | +5.436 | dentro | si' |
| 08/10 | 30.949-31.369 | 31.235 | 30.976,25 | 530.520 | +9.344 | dentro | no |
| 09/10 | 31.039-31.122 | 31.090 | 31.112,25 | 332.510 | -1.110 | dentro | no |
| 12/10 | 30.865-30.958 | 30.890 | 30.895,75 | 308.696 | +1.248 | dentro | si' |
| 13/10 | 30.712-30.800 | 30.775 | 30.788,75 | 359.526 | -270 | dentro | si' |
| 14/10 | 30.082-30.478 | 30.173 | 30.185 | 540.320 | +530 | dentro | si' |

"Staccato" (convenzione del collettore) = l'area di valore non tocca quella del giorno prima.
**Lo shift letterale** (VAL **e** VAH piu' bassi del giorno prima, `profile-framing.md` §2), letto
confrontando le cifre della tabella: 07/10 giu', 08/10 giu', 09/10 dentro (VAL piu' alto, VAH piu'
basso: il valore si restringe), 12/10 giu', 13/10 giu', 14/10 giu'.

**La scala dei POC di cassa** (20 sedute dal 17/09, `sedute-NQZ6.json`), dal basso:

| POC | seduta | rispetto al prezzo |
|---|---|---|
| 29.730 / 29.737 | 18/09, 17/09 | sotto, 85-92 punti |
| 30.173 | 14/10 | sopra, 350 punti |
| 30.609 / 30.630 / 30.634 | 29/09, 28/09, 01/10 | sopra |
| 30.745 / 30.769 / 30.775 / 30.783 | 23/09, 24/09, 13/10, 21/09 | sopra |
| 30.835 | 30/09 | sopra |
| 30.890 / 30.912 / 30.922 | 12/10, 25/09, 22/09 | sopra |
| 31.065 / 31.090 | 02/10, 09/10 | sopra |
| 31.235 / 31.310 / 31.353 / 31.530 | 08/10, 05/10, 07/10, 06/10 | sopra |

Fra 30.173 e 30.609 non c'e' nessun POC di cassa delle 20 sedute.

### Tab 3 · Seduta (`sedute-NQZ6.json`, `tab-ruoli.json`, `pannello.json`, `giro.txt`)

| finestra | VAL-VAH | POC | altri numeri |
|---|---|---|---|
| notte, 00:00-10:45 | 29.808-29.982 | 29.920 | O 30.154,75 H 30.158,50 L 29.754 C 29.822,50; 175.531 lotti, delta -2.415 |
| Londra, in formazione (M1) | 29.781-29.838 | 29.818 | stessa sul Range: 29.782-29.841, POC 29.818 |
| ieri cassa (M1 / Range) | 30.082-30.478 | 30.173 / 30.187 | il POC dipende dalla grana della tab |

- Il prezzo (29.822,50 alle 10:45) e' **sotto** il valore di cassa di ieri di 259,50 punti, sotto il
  valore delle ultime due sedute, al **9%** del range delle ultime 20 sedute, a 1.794 punti dal
  massimo (giro, sez. TAB 3).
- **Struttura oraria** (H1, giro): massimi e minimi decrescenti nelle ultime sei ore; swing sopra il
  prezzo a 29.823,50 (minimo H1 delle 05), 29.978,25 (massimo H1 delle 06), 30.591,25 e 30.604,75.
- **Delta orario** della notte (`ore-NQZ6.json`): 00:00 -1.320 (da 30.158,50 a 29.961,25), 01:00
  -382, poi quattro ore positive (+71, +454, +236, +60), 06:00 +367, 07:00 -510, **08:00 -1.243**
  (minimo 29.764,75), 09:00 -225, 10:00 +77 (46 minuti).
- **CVD dalla chiusura di ieri** (pannello, 22:00 di ieri -> 10:45): -2.497 su 194.791 lotti (-1,3%);
  CVD all'11% del suo range, prezzo al 15%; lettura dell'indicatore: "CVD e prezzo insieme sul
  minimo: i venditori sono seguiti".

**Il regime e la velocita', come li misura l'indicatore** (`pannello.json`, barra delle 10:45):

| riga | valore |
|---|---|
| REGIME | **CHOPPY**: efficienza 0,09 (netto 36,75 punti su strada 395,50), 2 rotture rientrate, copertura no |
| VELOCITA' (proxy, non la speed of tape) | **morta**: 16 lotti, percentile 0 su 60 barre; 0 big trade da 60+ lotti in 10 barre |
| in gioco | zona pesante Europa, bordo alto + POC Europa, 29.818-29.824,75; prezzo 2,25 sotto, arrivato da sotto |
| VETI | 2: libro sottile (nessun big trade e velocita' al percentile 0); regime choppy |

Efficienza = spostamento netto diviso strada percorsa (misura del repository); rotture rientrate e
copertura vengono dal live `[6 · 54:03]`, `[6 · 1:19:04]` (`i-tre-modelli-del-live-q1.md` §12).

**I livelli della notte sul chart** (`pannello.json`, `alLivello`, conteggi dalle 22:00 di ieri):

| livello | prezzo | tocchi | tiene | passa | lotti | delta |
|---|---|---|---|---|---|---|
| POC cash ieri | 30.173 | 1 | 0 | 1 | 13 | +3 |
| max notte | 30.158,50 | 6 | 5 | 1 | 467 | +77 |
| VAL cash ieri | 30.082 | 3 | 0 | 3 | 64 | -30 |
| VAH notte | 29.982 | 19 | 12 | 7 | 473 | +111 |
| POC notte | 29.920 | 68 | 39 | 29 | 1.643 | +39 |
| VAH Europa | 29.838 | 54 | 31 | 23 | 1.209 | -27 |
| top nodo Europa | 29.824,75 | 36 | 21 | 15 | 645 | +11 |
| VAL Europa | 29.781 | 25 | 9 | 16 | 875 | +5 |
| min Europa | 29.754 | 2 | 2 | 0 | 19 | -19 |

### Tab 4 · Esecuzione (NQZ6 Range 40 tick = 10 punti; `quattro-tab.txt`)

- Ultime tre barre chiuse: 10:42 giu' (63 s, 132 lotti, delta +6), 10:43 su (47 s, 83, -21), 10:44
  su (57 s, 114, -8); la barra aperta delle 10:44:59 ha 21 lotti in 6 s.
- Ultime 30 chiuse: 18 su e 12 giu', delta -59 (-0,9%), tutte fra 29.790,75 e 29.841, in 20 minuti.
- Tape M1 (giro, sez. 7, solo barre chiuse): 15 minuti 4.040 lotti delta -50; 30 minuti 11.258,
  -148; 60 minuti 23.705, -279. Mezz'ore: 09:00 11.029 lotti, 09:30 5.917, 10:00 9.816, 10:30
  4.056 (aperta).

## Cosa Dicono Per NQ

1. **Il valore scende da sei sedute, e da tre si stacca.** Dal 07/10 cinque sedute su sei
   spostano VAL e VAH in giu', e le ultime tre non toccano il valore del giorno prima. Per
   `profile-framing.md` §5, valore che si sposta = il modello di momentum, non di ritorno al POC.
   La seduta che ha creato la rottura sotto tutti i valori di cassa dal 21/09 (il VAL piu' basso
   era 30.506, del 28/09) e' **il 14/10**: area larga 396 punti, secondo volume delle 20 sedute
   (540.320). Il suo POC 30.173 e' il livello che il live tiene anche dopo la rottura `[1 · 24:35]`.
2. **La notte ha usato il VAL di ieri come strada, non come muro** `[1 · 1:19:30]`: 30.082 toccato
   tre volte, tre volte passato, mai tenuto (pannello). La prima ora della notte (-1.320 di delta) e
   l'ora delle 08:00 (-1.243) sono le due spinte; dalle 02:00 alle 06:00 il delta orario e' positivo
   (da +60 a +454) e il prezzo non sale oltre 30.010,75. **Osservazione**: le spinte in giu' hanno
   ottenuto prezzo, le ore con delta positivo no.
3. **Due scale, due posizioni nella curva.** Sul composito aggiustato di 90 giorni il prezzo e' a 5
   punti dal POC 29.817,50: **fair value**, al picco della distribuzione `[5 · 55:05]`. Sulle 20
   sedute di cassa e' al 9% del range, cioe' **basso nella curva** `[5 · 1:01:05]`. Il composito qui
   e' di barre a 24 ore, contro la regola del live (`i-tre-modelli-del-live-q1.md` §10): la prima
   lettura va presa con questa riserva.
4. **Il prezzo sta su una confluenza di riferimenti**, tutti entro 7 punti: POC composito aggiustato
   29.817,50, POC di Londra 29.818, VAH di cassa del 18/09 29.819, top nodo Europa 29.824,75. Sotto,
   la mensola di due POC di cassa 29.730-29.737 (17 e 18/09) con i VAL 29.678-29.694.
5. **Fra cassa e cassa c'e' un vuoto, ma la notte l'ha gia' costruito.** Fra il VAH del 18/09
   (29.819) e il VAL del 14/10 (30.082) nessuna seduta di cassa delle 20 ha valore: la seduta del
   21/09 ha aperto a 30.222, sopra. Stanotte pero' il POC della notte (29.920, 1.643 lotti, 68
   tocchi) sta proprio li'. Per la correzione di `profile-framing.md` (scaffale, non vuoto: prima di
   chiamarlo vuoto si guarda la seduta piu' recente che lo ha visitato) e' **un vuoto di cassa
   riempito di notte**; la notte non entra nel profilo `[1 · 1:14:15]`, quindi resta da vedere come
   lo tratta la cassa. **Osservazione**.
6. **Il regime lo dice l'indicatore: choppy.** Efficienza 0,09: 395,50 punti di strada per 36,75 di
   netto, due rotture rientrate. Dentro Londra (29.781-29.838), sopra il suo POC. E' coerente con la
   rotazione delle ultime 30 barre range (18 su, 12 giu', 50 punti in 20 minuti, delta -0,9%). Il
   framing di settimana (momentum) e quello della seduta (bilancia) non coincidono: il regime si
   dichiara, il setup no.
7. **Il ritmo si spegne.** Velocita' al percentile 0, nessun big trade da 60 lotti in 10 barre, mezz'ore
   con volume in calo (11.029, 5.917, 9.816, 4.056 aperta). Due riserve: il percentile 0 e' misurato
   sulla barra aperta da pochi secondi (16 lotti), e alle 14:30 escono PPI, vendite al dettaglio,
   Philly Fed e sussidi, un'ora prima della cassa.
8. **Il CVD segue il prezzo** (entrambi in basso nel loro range, pannello): nessuna divergenza da
   assorbimento misurata. Il delta di ieri in cassa (+530 su 540.320, 0,1%) e' quasi piatto: la
   discesa di circa 366 punti di ieri non ha un lato aggressivo netto. **Osservazione**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.890-30.922 | POC di cassa 12/10, 25/09, 22/09 | mensola di tre POC; ritorno qui = il valore della settimana scorsa riaccettato |
| 30.745-30.783 | POC di cassa 23/09, 24/09, 13/10, 21/09 | mensola di quattro POC: il cuore del blocco di fine settembre |
| 30.609-30.634 | POC di cassa 29/09, 28/09, 01/10 | il primo scaffale di valore sopra il buco di POC 30.173-30.609 |
| 30.478 | VAH cassa 14/10 | bordo alto della seduta della rottura |
| 30.173 | POC cassa 14/10 | il POC della seduta che ha creato la rottura `[1 · 24:35]`; il primo riferimento sulla via del ritorno |
| 30.082 | VAL cassa 14/10 | la strada passata stanotte tre volte su tre; riaccettarlo smentisce la rottura sotto il valore di ieri |
| 29.982-30.082 | VAH notte - VAL ieri | vuoto fra il valore della notte e quello di ieri |
| 29.920 | POC notte | il centro del valore costruito di notte dentro il vuoto di cassa |
| 29.817,50-29.824,75 | POC composito agg., POC Londra, VAH cassa 18/09, top nodo Europa | la confluenza su cui sta il prezzo: fair value del composito |
| 29.781 / 29.754 | VAL Europa / minimo notte | bordo basso della bilancia di Londra; sotto, la notte non ha ancora scambiato |
| 29.730-29.737 | POC cassa 17 e 18/09 | mensola di due POC sotto il prezzo, l'ultimo valore di cassa prima del rialzo del 21/09 |
| 29.340 | VAL composito aggiustato | bordo basso dei 90 giorni: sotto e' discount |
| 15/10 14:30 | vendite al dettaglio, PPI, Philly Fed, sussidi (giro, eventi) | dati prima della cassa, che apre alle 15:30 |
| 16/10 05:30 | discorso di Warsh, presidente della Fed (giro) | evento della notte di domani |

## Cosa Non Sappiamo

- **lacuna del collettore: composito di sola cassa.** La tab 1 usa barre Daily di 24 ore; il live
  vuole solo la cassa `[4 · 14:14]`. I bordi 29.340-30.820 non sono quelli del metodo.
- **lacuna del collettore: profilo per fasce del composito.** Ci sono solo VAL, VAH e POC: nodi e
  colli dei 90 giorni (il "vuoto" del framing, §6) non si vedono.
- **lacuna del collettore: profilo per fasce della cassa del 14/10.** Serve per dire se 30.173-30.478
  e' un nodo o una fascia sottile, come nella correzione di `profile-framing.md`.
- **lacuna del collettore: shift letterale.** Il collettore marca SU/GIU sul POC e "staccato" sulla
  sovrapposizione; lo shift VAL **e** VAH l'ho letto confrontando le cifre, e andrebbe misurato.
- **lacuna del collettore: big trade sui livelli.** Il campo `big` e' `null` su tutti gli 11 livelli:
  zero e "non lo so" non si distinguono, e il pannello non dichiara da quando copre il tape.
- **lacuna del collettore: finestra del regime.** Il pannello non dice su quante barre misura
  efficienza e rotture rientrate, ne' come definisce la copertura.
- **lacuna del collettore: velocita' sulla barra chiusa.** Il percentile 0 e' calcolato sulla barra
  aperta (16 lotti in pochi secondi); manca lo stesso proxy sull'ultima barra chiusa.
- **lacuna del collettore: ritmo delle barre range.** Manca una distribuzione di riferimento della
  durata delle barre da 40 tick: "accelera o si spegne" resta un'osservazione sui volumi M1.
- **lacuna del collettore: soglie di zona.** "fair value" arriva senza la soglia che separa premium,
  fair value e discount.
- **Discrepanze fra fonti** (non risolte, dichiarate): VAL notte 29.808 in `sedute-NQZ6.json` contro
  29.844 nell'indicatore (giro, sez. 6ter); minimo delle 20 sedute 29.648 nel giro contro 29.605,75
  del 17/09 in `sedute-NQZ6.json`; POC di ieri 30.173 (M1) contro 30.187 (Range 40).
- Lo scarto al rollover di +299,25 non e' la misura di `profile-framing.md` (mediana al minuto,
  +293,75 sul roll di settembre): convenzione diversa, non dichiarata nel JSON.
- Il "delta" e' quello del bridge (aggressore ask meno bid); niente footprint per prezzo sui bordi.

## Per Il Quadro

1. **COT al 6 ottobre**: quella settimana la cassa faceva il massimo delle 20 sedute (valore
   31.483-31.556, POC 31.530, massimo 31.616,50); oggi il POC di ieri e' 30.173. Il posizionamento
   del report e' stato misurato in cima al valore, prima di sei sedute di valore in discesa: la
   fase 5 controlli se il COT successivo puo' gia' riflettere lo shift.
2. **Gamma, 0DTE**: put wall 29.700 contro la mensola dei POC 29.730-29.737; call wall 30.300 dentro
   il valore di ieri (30.082-30.478). Coincidono o no con i bordi di cassa?
3. **Gamma, tutte le scadenze**: muro unico a 30.000 contro il vuoto 29.982-30.082 fra notte e ieri;
   zero gamma 30.921 contro la mensola dei POC 30.890-30.922 (prezzo sotto entrambi).
4. **Macro**: il regime choppy e la velocita' morta sono misurati alle 10:45, prima dei dati delle
   14:30; dopo i dati il pannello va riletto, non estrapolato.
