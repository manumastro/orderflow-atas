# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-10-15 (aggiornamento delle 18:43)

**Stato**: parziale (la cassa di oggi e' in corso, fino alle 22:00; mancano il composito di sola
cassa a 90 giorni, la cassa di oggi in `sedute-NQZ6.json` e la finestra del regime: vedi Cosa Non
Sappiamo)
**Dati**: `tab-ruoli.json`, `quattro-tab.txt`, `sedute-NQZ6.json`, `ore-NQZ6.json`, `pannello.json`,
`giro.txt`; per gli agganci le date di `2-cot.md` e la riga GEX del giro. Ora dei dati: **18:43** ora
italiana (`pannello.json`, `marketTimeUtc` 16:43Z; barra in formazione). `raccolta.json` non e' ancora
riscritto (dice 11:04-11:10): l'ora viene dal pannello, non dalla raccolta. GEX nel giro alle 11:51
(412 minuti prima, OI di ieri); COT al martedi' 6 ottobre (9 giorni). Errori della raccolta: non
noti finche' `raccolta.json` non e' chiuso.
Rapporto precedente: `precedenti/1850/4-orderflow.md` (dati delle 11:04).

**In una riga**: la cassa di oggi costruisce valore sotto quello di ieri, staccato di 22 punti; dal
minimo di 29.622,50 nella prima mezz'ora il prezzo e' risalito di 439 punti con delta in acquisto e
si e' fermato 20 punti sotto il VAL di ieri; l'indicatore misura ancora un regime choppy.

Convenzioni: cassa = 15:30-22:00 ora italiana (13:30-20:00 UTC); **POC** = il prezzo con il volume
massimo; **area di valore** (VAL bordo basso, VAH bordo alto) = il 70% del volume attorno al POC;
**delta** = contratti aggrediti in acquisto meno in vendita (bridge); **CVD** = il delta cumulato;
**shift** = VAL **e** VAH piu' bassi (o piu' alti) del giorno prima (`profile-framing.md` §2);
**staccato** = l'area non tocca quella del giorno prima (convenzione del collettore). Prezzi NQZ6.

## Cosa E' Cambiato Dalle 11:04

| misura | 11:04 | 18:43 | fonte |
|---|---|---|---|
| prezzo | 29.867,75 | 29.946,25 (+78,50) | pannello |
| cassa di oggi | non aperta | valore 29.792-30.060, POC 29.960, estremi 29.622,50-30.061,75 (in corso) | `tab-ruoli.json` |
| riga VALORE | fuori Londra, sopra | **dentro NY oggi, sotto il POC** | pannello |
| regime | CHOPPY: 0,15, 6 rotture rientrate, copertura no | CHOPPY: **0,10** (49,50 su 508,25), **3** rotture rientrate, copertura **si'** | pannello |
| velocita' (proxy) | morta, 24 lotti, 0 big trade | morta, 312 lotti, percentile 0; **2 big trade, netto -158** | pannello |
| porte | 29.920 sopra / 29.840 sotto | 29.960 POC cassa sopra (+13,75) / 29.920 POC notte sotto (-26,25) | pannello |
| veti | libro sottile; choppy | **in mezzo al range delle ultime 30 barre** (43%); choppy | pannello |
| CVD | dalla chiusura di ieri -2.402 | **dall'apertura 15:30 +8.960 (+1,6%)** su 569.186 lotti | pannello |
| barra daily | range 404, delta -2.321, 180k | range 536, delta **+7.829**, 877k (ieri intero 729k) | `quattro-tab.txt` |
| minimo Europa | 29.754 | 29.711 | pannello, livelli |
| barre range, ultime 30 | 25 minuti, delta -0,2% | **9 minuti**, delta +3,3% | TAB 4 |
| big trade sui livelli | `null` | numerico (0 o 1 per livello) | pannello, `alLivello` |
| composito 90 gg, VAH | 30.550 / 30.820 agg. | 30.530 / 30.810 agg. | `tab-ruoli.json` |

## I Fatti

### Tab 1 · Composito (NQ Daily continuo, 90 barre dal 2026-06-11, grana 5; `tab-ruoli.json`)

| versione | VAL | VAH | POC | zona del prezzo |
|---|---|---|---|---|
| grezzo (continuo come lo incolla ATAS) | 28.410 | 30.530 | 29.517,50 | fair value |
| aggiustato (+299,25 prima del 2026-09-11) | 29.340 | 30.810 | 29.817,50 | fair value |
| di sola cassa, 20 sedute dal 17/09 (`sedute-NQZ6.json`) | 30.458 | 31.177 | 30.770 | non data dal collettore |

Le prime due righe sono barre di 24 ore (nota nel JSON). Il prezzo e' 128,75 punti sopra il POC
aggiustato e 511,75 sotto il VAL del composito di cassa. Il composito di cassa non include oggi:
e' identico a quello delle 11:04.

### Tab 2 · Settimana (cassa per giorno; `sedute-NQZ6.json` M1 fasce da 1, oggi da `tab-ruoli.json` M30)

| seduta | VAL-VAH | POC | chiusura | volume | delta | chiude | staccato | shift |
|---|---|---|---|---|---|---|---|---|
| 08/10 | 30.949-31.369 | 31.235 | 30.976,25 | 530.520 | +9.344 | dentro | no | giu' |
| 09/10 | 31.039-31.122 | 31.090 | 31.112,25 | 332.510 | -1.110 | dentro | no | nessuno |
| 12/10 | 30.865-30.958 | 30.890 | 30.895,75 | 308.696 | +1.248 | dentro | si' | giu' |
| 13/10 | 30.712-30.800 | 30.775 | 30.788,75 | 359.526 | -270 | dentro | si' | giu' |
| 14/10 | 30.082-30.478 | 30.173 | 30.185 | 540.320 | +530 | dentro | si' | giu' |
| **15/10, in corso** | **29.792-30.060** | **29.960** | - | - | CVD +8.960 | - | si' (tab) | GIU (tab) |

La riga di oggi e' della tab Settimana ("GIU staccato in corso", POC sul giorno prima): VAL 29.792 <
30.082 e VAH 30.060 < 30.478, e fra il VAH di oggi e il VAL di ieri restano 22 punti. Il campo
`shift` del collettore non c'e' per oggi (lacuna sotto).

**La scala dei POC di cassa** (20 sedute dal 17/09 piu' oggi), rispetto a 29.946,25:

| POC | seduta | dove |
|---|---|---|
| 29.730 / 29.737 | 18/09, 17/09 | sotto, 209-216 punti |
| **29.960** | oggi, in formazione | sopra, 13,75 |
| **30.173** | 14/10, la seduta della rottura sotto i valori dal 21/09 | sopra, 226,75 |
| 30.609 / 30.630 / 30.634 | 29/09, 28/09, 01/10 | sopra |
| 30.745-30.783 | 23/09, 24/09, 13/10, 21/09 | sopra; 30.770 e' il POC del composito di cassa |
| 30.835-30.922 | 30/09, 12/10, 25/09, 22/09 | sopra |
| 31.065-31.530 | 02/10 ... 06/10 | sopra |

Fra 30.173 e 30.609 nessun POC di cassa; fra 29.737 e 29.960 nemmeno.

### Tab 3 · Seduta (`tab-ruoli.json`, `pannello.json`, `ore-NQZ6.json`, `giro.txt`)

| finestra | VAL | POC | VAH | estremi |
|---|---|---|---|---|
| notte | 29.844 | 29.920 | 29.982 | max 30.158,50 |
| Europa | 29.777 | 29.815 | 29.843 | min 29.711 |
| **cassa di oggi** (M1 e Range concordano) | 29.792 | 29.960 | 30.060 | 29.622,50 - 30.061,75 |
| cassa di ieri | 30.082 | 30.173 (M1) / 30.187 (Range) | 30.478 | 30.078 - 30.660,75 |

- **Dove sta il prezzo** (giro, TAB 3): dentro il valore di oggi, sotto il POC; sotto il valore di
  ieri di 135,50; al **16%** del range delle ultime 20 sedute (29.622,50-31.616,50).
- **Delta orario** (`ore-NQZ6.json`, ora italiana): 13:00 +332; 14:00 (dati USA alle 14:30) **+1.223**,
  min 29.711; 15:00 **+2.416**, 191.688 lotti, min 29.622,50, chiusura 29.688; 16:00 **+4.594**,
  207.862 lotti, 29.688,25 -> 29.971,25; 17:00 +2.414, max 30.061,75, chiusura 30.013,25; 18:00
  (44 minuti) -222, chiusura 29.946.
- **Mezz'ore** (TAB 2): 17:00 delta +324, 79.332 lotti; 17:30 **+2.090**, 55.912, 29.998,50 ->
  30.013,25 (range 73); 18:00 -699, 41.967, -> 29.925,50; 18:30 +482, 19.798 (aperta).
- **Struttura oraria** (giro): swing sopra 29.978,25 (06h) e 30.591,25-30.628,50 (13/10); sotto
  29.899,50 (11h), 29.823,50, 29.754 e **29.622,50 (15h)**. Lettura del giro: "le ultime sei ore:
  massimi e minimi crescenti".
- **Profilo 22:00 di ieri -> 18:43** (giro, sez. 8, fasce da 50, notte e cassa insieme): POC
  29.800-29.850 23,0% delta +3.180; 29.750-29.800 19,4% +532; 29.950-30.000 17,2% **+2.822**;
  29.900-29.950 12,2% -637.
- **Tape M1** (giro, sez. 7, barre chiuse): 15 min 21.920 lotti, +480 (+2,2%); 30 min 41.993, +251;
  60 min 90.222, +1.152 (+1,3%), range 29.909,25-30.037,25.

**Il regime e la velocita', come li misura l'indicatore** (`pannello.json`, barra delle 18:43):

| riga | valore |
|---|---|
| REGIME | **CHOPPY**: efficienza 0,10 (netto 49,50 punti su strada 508,25), 3 rotture rientrate, copertura si' |
| VELOCITA' (proxy, non la speed of tape) | **morta**: 312 lotti, percentile 0 su 60 barre; 2 big trade da 60+ lotti in 10 barre, netto -158 |
| VALORE | dentro NY oggi 29.792-30.060, sotto il POC 29.960; sotto la cassa di ieri e di martedi' |
| in gioco | niente; porte: sopra 29.960 POC cassa (+13,75), sotto 29.920 POC notte (-26,25) |
| VETI | 2: prezzo al 43% del range delle ultime 30 barre (29.909,25-29.995,75); regime choppy |
| CVD dall'apertura 15:30 | +8.960 su 569.186 lotti (+1,6%); **CVD al 95% del suo range, prezzo al 74%** |

Efficienza = spostamento netto diviso strada percorsa. La riga operativa che il pannello stampa
sotto il regime non si riporta (descrizione, non regola).

**I livelli sul chart, dalle 15:30** (`pannello.json`, `alLivello`):

| livello | prezzo | tocchi | tiene | passa | lotti | delta | big (netto) |
|---|---|---|---|---|---|---|---|
| VAL, POC, max cassa ieri; max notte | 30.082-30.660,75 | 0 | - | - | 0 | 0 | 0 |
| VAH cassa | 30.060 | 1 | 1 | 0 | 201 | +41 | 0 |
| VAH notte | 29.982 | 28 | 10 | 18 | 2.043 | +43 | 0 |
| POC cassa | 29.960 | 31 | 15 | 16 | **4.481** | **+389** | 1 (+132) |
| POC notte | 29.920 | 17 | 8 | 9 | 2.429 | +21 | 0 |
| VAH Europa (unito a VAL notte 29.844) | 29.843 | 15 | 12 | 3 | 1.979 | +147 | 0 |
| top nodo Europa | 29.824,75 | 27 | 10 | 17 | 3.046 | +80 | 1 (+85) |
| POC Europa | 29.815 | 30 | 17 | 13 | 2.801 | +91 | 0 |
| VAL cassa (unito a mensola 29.796) | 29.792 | 24 | 11 | 13 | 3.224 | +6 | 0 |
| VAL Europa | 29.777 | 17 | 12 | 5 | 2.301 | **-275** | 1 (-71) |
| min Europa | 29.711 | 9 | 3 | 6 | 2.172 | -114 | 1 (+70) |
| min cassa | 29.622,50 | 1 | 1 | 0 | 12 | -12 | 0 |

### Tab 4 · Esecuzione (NQZ6 Range 40 tick = 10 punti; `quattro-tab.txt`)

- Ultime barre: 18:41:37 giu' (12 s, 306 lotti, -18), 18:41:49 su (19 s, 281, +13), 18:42:09 giu'
  (7 s, 167, +39); aperta dalle 18:42:16, 68 s, 831 lotti, delta -37.
- Ultime 30 chiuse: 16 su, 14 giu', delta +455 (+3,3%), fra 29.911,75 e 29.991,75 in **9 minuti**
  (alle 11:04: 30 barre in 25 minuti).

## Cosa Dicono Per NQ

1. **Il valore continua a scendere.** Se la cassa chiude cosi', dal 07/10 sei sedute su sette hanno
   valore piu' basso, le ultime quattro staccate. Per `profile-framing.md` §5 il valore che si sposta
   e' il modello di momentum. Il POC della seduta della rottura resta **30.173 (14/10)** `[1 · 24:35]`:
   oggi non e' stato toccato (0 tocchi dalle 15:30).
2. **Dentro la seduta, la direzione si e' girata e poi fermata.** Minimo 29.622,50 nella prima mezz'ora
   di cassa, poi 16:00 e 17:00 con delta +4.594 e +2.414 e 439,25 punti di salita fino a 30.061,75. Il
   massimo e' 20,25 punti sotto il VAL di ieri (30.082): il valore di ieri non e' stato riaccettato.
   Il VAL e' una strada `[1 · 1:19:30]`: stanotte e' stato passato dall'alto, oggi non e' stato
   raggiunto dal basso. Poi 115,50 punti di ritorno, fino al POC di oggi. **Osservazione**.
3. **Delta in acquisto che ottiene poco, attorno a 30.000.** Mezz'ora delle 17:30: delta +2.090 per 15
   punti di avanzamento (range 73). Fascia 29.950-30.000: delta +2.822. Il CVD della cassa e' al 95% del
   suo range, il prezzo al 74%: il delta e' salito piu' del prezzo. Il pannello non stampa una
   divergenza; **osservazione**, non misura dell'indicatore.
4. **Tre scale, tre posizioni nella curva.** Composito aggiustato di 90 giorni (24 ore): fair value,
   128,75 sopra il picco `[5 · 55:05]`. Composito di cassa di 20 sedute: 511,75 sotto il VAL. Range delle
   20 sedute: al 16%, **basso nella curva** `[5 · 1:01:05]`. La regola del live (sola cassa, 90 giorni,
   `i-tre-modelli-del-live-q1.md` §10, `[4 · 14:14]`) non e' rispettata da nessuna delle tre scale.
5. **La cassa di oggi e' tornata sul valore di cassa di meta' settembre.** Il VAL di oggi (29.792) sta
   dentro il valore del 18/09 (29.694-29.819). Il minimo di cassa 29.622,50 e' 16,75 sopra quello del
   17/09 (29.605,75). Il 21/09 la cassa aveva aperto a 30.222: il vuoto di cassa di stamattina
   (29.819-30.082) ora e' quasi tutto dentro il valore di oggi. **Osservazione**.
6. **Il regime lo dice l'indicatore: choppy.** Efficienza 0,10 su 508,25 punti di strada, 3 rotture
   rientrate, copertura si'. Il prezzo va da un lato all'altro dell'asta `[6 · 54:03]` (§7). Le barre
   range da 40 tick ora durano 7-19 s, contro 18-47 s di stamattina, ma restano in 80 punti. Il volume
   per mezz'ora cala: 79k, 56k, 42k, poi 20k (aperta). Il framing di settimana (momentum) e quello della
   seduta (rotazione) non coincidono: **il regime si dichiara, il setup no**.
7. **Chi ha difeso i bordi di sotto.** VAL Europa 29.777: tenuto 12 volte su 17, delta -275, venditori
   aggressivi che non hanno ottenuto prezzo. POC di cassa 29.960: il livello piu' scambiato (4.481
   lotti, 31 tocchi, 15 tenute e 16 passaggi). E' un punto di rotazione, non un bordo. **Osservazione**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.890-30.922 | POC di cassa 12/10, 25/09, **22/09** | mensola di tre POC; il 22/09 e' la data COT della fase 2 |
| 30.770 | POC del composito di cassa, 20 sedute | il cuore della distribuzione di cassa recente |
| 30.712 | VAL cassa 13/10 (pannello, "sotto martedi'") | bordo del valore di due sedute fa |
| 30.478-30.712 | VAH 14/10 - VAL 13/10 | vuoto fra due sedute, con lo scaffale di fine settembre dentro (28/09-01/10) |
| 30.609-30.634 | POC di cassa 29/09, 28/09, 01/10 | primo scaffale di valore sopra il buco di POC 30.173-30.609 |
| 30.458 / 30.478 | VAL composito di cassa / VAH 14/10 | rientro nella distribuzione di cassa recente |
| 30.173 | POC cassa 14/10 | il POC della seduta della rottura `[1 · 24:35]`; 0 tocchi oggi |
| 30.082 | VAL cassa 14/10 | riaccettarlo smentisce lo stacco di oggi; non raggiunto (max 30.061,75) |
| 30.060-30.082 | VAH oggi - VAL ieri | vuoto di 22 punti fra le due casse |
| 30.060 / 30.061,75 | VAH e massimo di cassa di oggi | bordo alto del valore in costruzione: uscirne riapre il vuoto sopra |
| 29.982 | VAH notte | 28 tocchi, passato 18 volte: bordo consumato |
| 29.960 | POC cassa oggi (porta sopra del pannello) | il centro del valore di oggi |
| 29.920 | POC notte (porta sotto del pannello) | il centro del valore della notte |
| 29.815-29.844 | POC Europa, VAH Europa, VAL notte; POC composito agg. 90 gg 29.817,50; VAH 18/09 29.819 | confluenza: il fair value del composito di 24 ore |
| 29.777-29.796 | VAL Europa, VAL cassa oggi, mensola europea | bordo basso di oggi: uscirne e' lasciare il valore della seduta |
| 29.730-29.737 | POC cassa 17 e 18/09 | l'ultimo valore di cassa prima del 21/09 |
| 29.622,50 / 29.605,75 | minimo di cassa oggi / minimo di cassa 17/09 | estremi bassi delle 20 sedute: sotto, nessun valore di cassa recente |
| 29.340 | VAL composito aggiustato 90 gg | sotto e' discount sulla scala lunga |
| 15/10 22:00 | chiusura della cassa e scadenza dello 0DTE (giro) | la seduta di oggi diventa un profilo chiuso |
| 16/10 05:30 / 21:30 | Warsh (Fed); nuovo COT, posizioni al 13/10 (giro) | notte di domani; posizionamento prima del 14/10 |

## Cosa Non Sappiamo

- **lacuna del collettore: la cassa di oggi non e' in `sedute-NQZ6.json`.** Mancano apertura, delta di
  cassa, "chiude", `staccato` e `shift` del collettore; ci sono solo il valore della tab (M30, M1,
  Range) e il CVD del pannello.
- **lacuna del collettore: `oggi_finora` ha cambiato contenuto.** Alle 11:04 era la notte. Ora va dalle
  00:00 alle 18:43, notte e cassa insieme (POC 29.800, VAL 29.700, VAH 29.950), con numeri tondi da
  50 benche' il file dichiari fasce da 1. La notte chiusa alle 15:30 non c'e' come profilo a se'.
- **lacuna del collettore: composito di sola cassa a 90 giorni** e la sua zona (premium, fair value,
  discount); unita' della "quota" dei nodi non dichiarata.
- **lacuna del collettore: finestra del regime.** Non si sa su quante barre l'indicatore misura
  efficienza e rotture rientrate, ne' come definisce la copertura. Per questo non si sa se la salita
  delle 16:00 (+283 punti) sta dentro la misura.
- **lacuna del collettore: velocita' sulla barra chiusa.** Il percentile 0 e' della barra M1 aperta
  (312 lotti); manca lo stesso proxy sull'ultima chiusa.
- **lacuna del collettore: profilo per fasce della cassa del 14/10 e di oggi.** Serve a dire se
  30.173-30.478 e 29.792-30.060 sono nodi o fasce sottili (la correzione di `profile-framing.md`).
- **lacuna del collettore: ritmo delle barre range.** Manca una distribuzione di riferimento delle
  durate: "accelera" resta un'osservazione.
- **lacuna del collettore: definizione della struttura oraria.** Il giro dice "massimi e minimi
  crescenti" sulle ultime sei ore, ma i minimi orari di `ore-NQZ6.json` delle 13, 14 e 15 scendono
  (29.748,50; 29.711; 29.622,50). La regola di swing del giro non e' dichiarata.
- **lacuna del collettore: `raccolta.json`** non e' ancora riscritto; l'ora dei dati viene dal pannello.
- Discrepanze dichiarate, non risolte: POC di ieri 30.173 (M1) contro 30.187 (Range); scarto al
  rollover +299,25 (tab) contro +293,75 (`profile-framing.md`, mediana al minuto).
- L'ora delle 15:00 di `ore-NQZ6.json` include la mezz'ora prima della cassa.
- L'esito dei dati delle 14:30 non e' nei miei dati (fase 1). Niente footprint per prezzo sui bordi.

## Per Il Quadro

1. **COT** (fase 2): il 22/09, data della variazione piu' grande delle ultime quattro, la cassa aveva
   valore 30.888-30.980 e POC 30.922, dentro la mensola 30.890-30.922. Il 6/10 il valore era
   31.483-31.556. Il COT di domani (posizioni al 13/10) non vedra' 14/10 e 15/10, le due sedute
   staccate.
2. **Gamma, tutte le scadenze** (giro, 11:51, OI di ieri, convenzione A): il muro a 30.000 sta nella
   fascia dove oggi il delta in acquisto ha ottenuto poco (17:30, +2.090 per 15 punti). Il massimo di
   cassa e' a 30.061,75. Lo zero gamma 30.887 sta contro la mensola dei POC 30.890-30.922.
3. **Gamma 0DTE**: il put wall 29.700 sta sopra il minimo di cassa 29.622,50, sotto il VAL di oggi
   29.792. Lo zero gamma 0DTE 30.461 coincide con il VAL del composito di cassa (30.458) e col VAH del
   14/10 (30.478). Lo 0DTE scade alle 22:00 (la GEX di ES e' ancora in raccolta).
4. **Macro** (fase 1): delta orario positivo dalle 13:00 alle 17:00, +1.223 nell'ora dei dati delle
   14:30; minimo nella prima mezz'ora di cassa, poi la salita. Il pannello va riletto dopo Warsh
   (16/10 05:30), non estrapolato.
