# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-10-15 (aggiornamento delle 11:04)

**Stato**: parziale (il composito di 90 giorni e' ancora di barre a 24 ore; mancano i profili per
fasce, i big trade sui livelli, la finestra del regime: vedi Cosa Non Sappiamo)
**Dati**: `tab-ruoli.json`, `quattro-tab.txt`, `sedute-NQZ6.json`, `ore-NQZ6.json`, `pannello.json`,
`giro.txt`; per gli agganci solo le date di `cot.json` e la riga GEX del giro. Order flow raccolto
alle 11:04 ora italiana (`raccolta.json`), nessun errore. Pannello alla barra delle 11:04 (09:04 UTC,
barra in formazione). GEX nel giro alle 10:45 (OI di ieri); COT al martedi' 6 ottobre (9 giorni).
Rapporto precedente: `precedenti/1110/4-orderflow.md` (dati delle 10:45-10:51).

**In una riga**: il valore di cassa scende da tre sedute, staccato, e il collettore ora lo misura
come shift giu'; la notte ha costruito valore nel vuoto fra le casse, il prezzo e' risalito di 45
punti sopra il bilancio di Londra, e l'indicatore misura ancora un regime choppy.

Convenzioni: cassa = 15:30-22:00 ora italiana (13:30-20:00 UTC); **POC** = il prezzo con il volume
massimo; **area di valore** (VAL bordo basso, VAH bordo alto) = il 70% del volume attorno al POC;
**delta** = contratti aggrediti in acquisto meno in vendita; **CVD** = il delta cumulato; **shift**
= VAL **e** VAH piu' bassi (o piu' alti) del giorno prima (`profile-framing.md` §2). Prezzi NQZ6.

## Cosa E' Cambiato Dalle 10:45

| misura | 10:45 | 11:04 | fonte |
|---|---|---|---|
| prezzo | 29.822,50 | 29.867,75 (+45,25) | pannello |
| valore della seduta | dentro Londra, sul bordo alto | **fuori Londra, sopra** | pannello, riga VALORE |
| in gioco | zona pesante Europa 29.818-29.824,75 | **niente**: porte a 29.920 e 29.840 | pannello |
| efficienza / rotture rientrate | 0,09 / 2 | 0,15 / **6** | pannello, REGIME |
| CVD dalla chiusura di ieri | -2.497, all'11% del range, prezzo al 15% | -2.402, al 14%, prezzo al **25%** | pannello |
| struttura oraria H1 | massimi e minimi decrescenti | "senza direzione" | giro, TAB 3 |
| shift letterale | letto a mano | **misurato dal collettore** (campo `shift`) | `sedute-NQZ6.json` |
| composito di sola cassa | assente | **20 sedute** con i nodi (non 90) | `sedute-NQZ6.json` |

## I Fatti

### Tab 1 · Composito (NQ Daily continuo, 90 barre dal 2026-06-11, grana 5; `tab-ruoli.json`)

| versione | VAL | VAH | POC | zona del prezzo 29.867,75 |
|---|---|---|---|---|
| grezzo (continuo come lo incolla ATAS) | 28.410 | 30.550 | 29.517,50 | fair value |
| aggiustato (+299,25 sui giorni prima del 2026-09-11) | 29.340 | 30.820 | 29.817,50 | fair value |
| **di sola cassa, 20 sedute dal 17/09** | 30.458 | 31.177 | 30.770 | zona non data dal collettore |

Le prime due righe sono barre di 24 ore (nota nel JSON). La terza e' la sola cassa, ma su 20 sedute,
non su 90. Nodi da 5 punti del composito di cassa (`nodi_da_5_punti`, "quota" senza unita'
dichiarata): 30.770-30.775 (0,94), 30.630-30.635 (0,82), 30.565-30.570 (0,74), 30.900-30.905 (0,72),
30.820-30.825 (0,63). Il prezzo e' 590,25 punti sotto il VAL di quel composito.

Barra daily di oggi, in corso (`quattro-tab.txt`): 30.154,75 -> 29.867,75, range 404, delta
-2.321, volume 180k. Ieri: range 814, volume 729k, delta +1.572.

### Tab 2 · Settimana (cassa per giorno; `sedute-NQZ6.json`, M1 fasce da 1)

| seduta | VAL-VAH | POC | chiusura | volume | delta | chiude | staccato | shift |
|---|---|---|---|---|---|---|---|---|
| 06/10 | 31.483-31.556 | 31.530 | 31.480,75 | 348.459 | -3.557 | sotto | si' | su |
| 07/10 | 31.245-31.415 | 31.353 | 31.395,75 | 361.070 | +5.436 | dentro | si' | giu' |
| 08/10 | 30.949-31.369 | 31.235 | 30.976,25 | 530.520 | +9.344 | dentro | no | giu' |
| 09/10 | 31.039-31.122 | 31.090 | 31.112,25 | 332.510 | -1.110 | dentro | no | nessuno |
| 12/10 | 30.865-30.958 | 30.890 | 30.895,75 | 308.696 | +1.248 | dentro | si' | giu' |
| 13/10 | 30.712-30.800 | 30.775 | 30.788,75 | 359.526 | -270 | dentro | si' | giu' |
| 14/10 | 30.082-30.478 | 30.173 | 30.185 | 540.320 | +530 | dentro | si' | giu' |

"Staccato" (convenzione del collettore) = l'area non tocca quella del giorno prima. 08/10 e 09/10 si
sovrappongono (09/10 sta dentro 08/10): per `profile-framing.md` §3 sono un blocco unico,
30.949-31.369. Le ultime tre sedute non si sovrappongono a nessuna vicina: nessuna fusione.

**La scala dei POC di cassa** (20 sedute dal 17/09), dal basso:

| POC | seduta | rispetto a 29.867,75 |
|---|---|---|
| 29.730 / 29.737 | 18/09, 17/09 | sotto, 131-138 punti |
| 30.173 | 14/10, la seduta della rottura | sopra, 305 punti |
| 30.609 / 30.630 / 30.634 | 29/09, 28/09, 01/10 | sopra |
| 30.745 / 30.769 / 30.775 / 30.783 | 23/09, 24/09, 13/10, 21/09 | sopra |
| 30.835 / 30.890 / 30.912 / 30.922 | 30/09, 12/10, 25/09, 22/09 | sopra |
| 31.065 / 31.090 / 31.235 / 31.310 / 31.353 / 31.530 | 02/10 ... 06/10 | sopra |

Fra 30.173 e 30.609 nessun POC di cassa.

### Tab 3 · Seduta (`sedute-NQZ6.json`, `tab-ruoli.json`, `pannello.json`, `giro.txt`)

| finestra | VAL-VAH | POC | altri numeri |
|---|---|---|---|
| notte, 00:00-11:04 (M1) | 29.812-29.982 | 29.920 | O 30.154,75 H 30.158,50 L 29.754 C 29.867,75; 180.344 lotti, delta -2.320 |
| Londra, in formazione (M1 / Range 40) | 29.776-29.840 / 29.781-29.850 | 29.818 | il bordo alto dipende dalla grana della tab |
| ieri cassa (M1 / Range 40) | 30.082-30.478 | 30.173 / 30.187 | idem |

- Il prezzo e' **sotto** il valore di ieri di 214,25 punti e sotto quello delle ultime due sedute; e'
  all'11% del range delle ultime 20 sedute, a 1.748,75 punti dal massimo (giro, TAB 3).
- Profilo della finestra 22:00 di ieri -> 11:04 (giro, sez. 8, fasce da 50): POC 29.900-29.950 con il
  20,9% e delta **-899**; 29.800-29.850 18,9% delta -477; 29.850-29.900 16,5% delta +270.
- **Delta orario** (`ore-NQZ6.json`): 00:00 -1.320, 01:00 -382, dalle 02:00 alle 06:00 positivo
  (+71, +454, +236, +60, +367), 07:00 -510, 08:00 **-1.243** (min 29.764,75), 09:00 -225 (min
  29.754), **10:00 +152** (min 29.754,50, chiusura 29.862,25), 11:00 +20 (5 minuti, aperta).
- **Struttura oraria** (H1, giro): swing sotto il prezzo 29.823,50 (05h) e 29.754 (09h); sopra
  29.978,25 (06h), poi 30.591,25-30.628,50 (13/10). Lettura del giro: "senza direzione".
- **Tape M1** (giro, sez. 7, barre chiuse): 15 minuti 3.760 lotti delta -8; 30 minuti 7.273, -45;
  60 minuti 21.484, **+400 (+1,9%)**, range 29.754,50-29.877,25.

**Il regime e la velocita', come li misura l'indicatore** (`pannello.json`, barra delle 11:04):

| riga | valore |
|---|---|
| REGIME | **CHOPPY**: efficienza 0,15 (netto 52,50 punti su strada 361,75), 6 rotture rientrate, copertura no |
| VELOCITA' (proxy, non la speed of tape) | **morta**: 24 lotti, percentile 0 su 60 barre; 0 big trade da 60+ lotti in 10 barre |
| VALORE | fuori Londra, sopra 29.776-29.840 (POC 29.818); sotto la cassa di ieri e di martedi' |
| in gioco | niente; porte: sopra 29.920 POC notte (+52,25), sotto 29.840 VAH Europa con VAL notte 29.844 (-27,75) |
| VETI | 2: libro sottile (nessun big trade, velocita' al percentile 0); regime choppy |
| CVD dalla chiusura di ieri | -2.402 su 199.604 lotti (-1,2%); CVD al 14% del suo range, prezzo al 25% |

Efficienza = spostamento netto diviso strada percorsa (misura del repository). La riga operativa
che il pannello aggiunge sotto il regime non si riporta qui (descrizione, non regola).

**I livelli della notte sul chart** (`pannello.json`, `alLivello`, conteggi dalle 22:00 di ieri):

| livello | prezzo | tocchi | tiene | passa | lotti | delta |
|---|---|---|---|---|---|---|
| VAL cash ieri | 30.082 | 3 | 0 | 3 | 64 | -30 |
| max notte | 30.158,50 | 6 | 5 | 1 | 467 | +77 |
| VAH notte | 29.982 | 19 | 12 | 7 | 473 | +111 |
| POC notte | 29.920 | 68 | 39 | 29 | 1.643 | +39 |
| VAH Europa | 29.840 | 54 | 28 | 26 | 1.304 | +78 |
| top nodo Europa | 29.824,75 | 37 | 21 | 16 | 723 | +11 |
| VAL Europa | 29.776 | 23 | 15 | 8 | 862 | -54 |
| min Europa | 29.754 | 2 | 2 | 0 | 19 | -19 |

### Tab 4 · Esecuzione (NQZ6 Range 40 tick = 10 punti; `quattro-tab.txt`)

- Ultime barre: 11:02:27 su (18 s, 90 lotti, delta +46), 11:02:45 su (19 s, 131, +27), 11:03:05
  giu' (18 s, 156, +32); aperta dalle 11:03:24, 47 s, 147 lotti, delta -11.
- Ultime 30 chiuse: 18 su, 12 giu', delta -12 (-0,2%), fra 29.806,50 e 29.877,25 in 25 minuti
  (alle 10:45: 30 barre in 20 minuti, delta -59).
- Mezz'ore (TAB 2): 09:30 5.917 lotti, 10:00 9.816, 10:30 7.689, 11:00 1.180 (aperta).

## Cosa Dicono Per NQ

1. **Il valore scende, e ora lo misura il collettore.** Dal 07/10 cinque sedute su sei hanno shift
   giu' (09/10 "nessuno"), le ultime tre staccate. Per `profile-framing.md` §5 valore che si sposta =
   il modello di momentum, non il ritorno al POC. La seduta della rottura sotto tutti i valori di
   cassa dal 21/09 e' **il 14/10** (area larga 396 punti, secondo volume delle 20 sedute): il suo
   POC 30.173 e' il livello che il live tiene anche dopo la rottura `[1 · 24:35]`.
2. **Tre scale, tre posizioni nella curva.** Sul composito aggiustato di 90 giorni (24 ore) il prezzo
   e' 50,25 sopra il POC 29.817,50: fair value, vicino al picco `[5 · 55:05]`. Sul composito di sola
   cassa delle 20 sedute e' 590 punti sotto il VAL: sotto tutta la distribuzione recente. Sulle 20
   sedute e' all'11% del range: **basso nella curva** `[5 · 1:01:05]`. La regola del live e' sola
   cassa e 90 giorni (`i-tre-modelli-del-live-q1.md` §10): nessuna delle tre scale la rispetta per
   intero, e la lettura va presa con questa riserva.
3. **La notte ha usato il VAL di ieri come strada** `[1 · 1:19:30]`: 30.082 tre tocchi, tre passaggi,
   mai tenuto. Le due spinte in giu' (00:00 -1.320, 08:00 -1.243) hanno ottenuto prezzo; le ore con
   delta positivo (02:00-06:00) no. **Osservazione**.
4. **Il minimo della notte ha tenuto due volte.** 29.754 alle 09h e 29.754,50 alle 10h; il pannello
   conta 2 tocchi, 2 tenute. L'ora delle 10:00 ha chiuso a 29.862,25 con delta +152, 108,25 punti
   sopra il minimo, e il prezzo e' uscito dal bilancio di Londra dall'alto. E' la prima ora con
   delta positivo dopo tre negative. **Osservazione**: un'ora, in un regime che l'indicatore misura
   choppy.
5. **Il vuoto di cassa e' riempito di notte.** Fra il VAH del 18/09 (29.819) e il VAL del 14/10
   (30.082) nessuna seduta di cassa delle 20 ha valore (il 21/09 ha aperto a 30.222). La notte ci ha
   messo il suo valore, 29.812-29.982 con POC 29.920 (1.643 lotti, 68 tocchi), e il prezzo e' dentro.
   Per la correzione di `profile-framing.md` (scaffale, non vuoto) e' un vuoto di cassa con uno
   scaffale di notte; la notte non entra nel profilo `[1 · 1:14:15]`. **Osservazione**.
6. **Il regime lo dice l'indicatore: choppy, e piu' di prima.** Efficienza da 0,09 a 0,15, ma le
   rotture rientrate da 2 a **6**: il prezzo va da un lato all'altro dell'asta `[6 · 54:03]`
   (`i-tre-modelli-del-live-q1.md` §7). Le ultime 30 barre range coprono 71 punti con delta -0,2%. Il
   framing di settimana (momentum) e quello della seduta (rotazione) non coincidono: il regime si
   dichiara, il setup no.
7. **Il ritmo resta spento.** Velocita' al percentile 0 (sulla barra aperta da pochi secondi, 24
   lotti), nessun big trade da 60 lotti in 10 barre; le barre range da 40 tick durano 18-47 s.
8. **CVD e prezzo si scostano di poco.** Alle 10:45 erano insieme sul minimo (11% e 15%); alle 11:04
   il prezzo e' al 25% del suo range, il CVD al 14%. Il prezzo e' salito piu' del CVD; il pannello
   stavolta non stampa una lettura. Il cuore del profilo della notte (29.900-29.950) ha delta -899.
   **Osservazione**, non una divergenza misurata dall'indicatore.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.890-30.922 | POC di cassa 12/10, 25/09, 22/09 | mensola di tre POC: valore della settimana scorsa riaccettato |
| 30.770 (30.745-30.783) | POC del composito di cassa 20 sedute; POC 23/09, 24/09, 13/10, 21/09 | il cuore delle 20 sedute |
| 30.609-30.634 | POC di cassa 29/09, 28/09, 01/10 | primo scaffale di valore sopra il buco di POC 30.173-30.609 |
| 30.458 | VAL del composito di cassa 20 sedute | rientrare nella distribuzione di cassa recente |
| 30.478 | VAH cassa 14/10 | bordo alto della seduta della rottura |
| 30.173 | POC cassa 14/10 | il POC della seduta della rottura `[1 · 24:35]`: primo riferimento sulla via del ritorno |
| 30.082 | VAL cassa 14/10 | passato tre volte su tre stanotte; riaccettarlo smentisce la rottura sotto il valore di ieri |
| 29.982-30.082 | VAH notte - VAL ieri | vuoto fra il valore della notte e quello di ieri |
| 29.920 | POC notte | porta sopra del pannello: il centro del valore costruito di notte |
| 29.840-29.844 | VAH Europa, VAL notte | porta sotto del pannello: rientro nel bilancio di Londra |
| 29.817,50-29.824,75 | POC composito agg. 90 gg, POC Londra 29.818, VAH cassa 18/09 29.819, top nodo Europa | confluenza: il fair value del composito di 24 ore |
| 29.776 / 29.754 | VAL Europa / minimo notte (2 tenute) | bordo basso di Londra; sotto, la notte non ha scambiato |
| 29.730-29.737 | POC cassa 17 e 18/09 | mensola di due POC: l'ultimo valore di cassa prima del 21/09 |
| 29.340 | VAL composito aggiustato 90 gg | sotto e' discount sulla scala lunga |
| 15/10 14:30 | vendite al dettaglio, PPI, Philly Fed, sussidi (giro, eventi) | un'ora prima della cassa delle 15:30 |
| 15/10 22:00 | scadenza dello 0DTE (giro, 4bis) | fine della seduta |
| 16/10 05:30 / 21:30 | Warsh (Fed); nuovo COT (giro, 4bis) | notte di domani; posizionamento al 13/10 |

## Cosa Non Sappiamo

- **lacuna del collettore: composito di sola cassa a 90 giorni.** Ora c'e' quello di 20 sedute; la
  tab 1 resta di barre Daily a 24 ore. Nessuna delle due e' la regola del live `[4 · 14:14]`.
- **lacuna del collettore: zona del composito di cassa.** Premium, fair value o discount non sono
  dati per le 20 sedute, ne' la soglia che separa le zone per i 90 giorni.
- **lacuna del collettore: unita' della "quota" dei nodi** (`nodi_da_5_punti`): non e' dichiarata.
- **lacuna del collettore: profilo per fasce della cassa del 14/10.** Serve a dire se 30.173-30.478 e'
  un nodo o una fascia sottile (la correzione di `profile-framing.md`).
- **lacuna del collettore: big trade sui livelli.** `big` e' `null` su tutti gli 11 livelli: zero e
  "non lo so" non si distinguono.
- **lacuna del collettore: finestra del regime.** Il pannello non dice su quante barre misura
  efficienza e rotture rientrate, ne' come definisce la copertura.
- **lacuna del collettore: velocita' sulla barra chiusa.** Il percentile 0 e' della barra aperta
  (24 lotti); manca lo stesso proxy sull'ultima chiusa.
- **lacuna del collettore: ritmo delle barre range.** Manca una distribuzione di riferimento della
  durata delle barre da 40 tick: "accelera o si spegne" resta un'osservazione.
- **Discrepanze fra fonti** (dichiarate, non risolte): VAL notte 29.812 (`sedute-NQZ6.json`) contro
  29.844 (indicatore, giro 6ter); minimo delle 20 sedute 29.648 (giro) contro 29.605,75 del 17/09
  (`sedute-NQZ6.json`); POC di ieri 30.173 (M1) contro 30.187 (Range); VAH Londra 29.840 (M1) contro
  29.850 (Range).
- Lo scarto al rollover +299,25 non e' la misura di `profile-framing.md` (mediana al minuto, +293,75):
  convenzione diversa, non dichiarata nel JSON.
- Il "delta" e' quello del bridge (aggressore ask meno bid); niente footprint per prezzo sui bordi.
- Il file della giornata `NQZ6-2026-10-15.md` non ha "Dove eravamo" ne' annotazioni (giro, sez. 2 e 5).

## Per Il Quadro

1. **COT al 6 ottobre**: quella settimana la cassa faceva il massimo delle 20 sedute (valore
   31.483-31.556, POC 31.530). Da allora sei sedute, cinque con shift giu'. Il prossimo COT (16/10
   21:30, posizioni al 13/10) puo' vedere solo una parte di questo spostamento.
2. **Gamma, 0DTE** (giro, 10:45): put wall 29.700 contro la mensola dei POC 29.730-29.737 e il minimo
   della notte 29.754; call wall 30.300 dentro il valore di ieri (30.082-30.478). Coincidono con i
   bordi di cassa? (il file GEX delle 11:07 e' della fase 3).
3. **Gamma, tutte le scadenze**: muro a 30.000 dentro il vuoto 29.982-30.082 fra notte e ieri; zero
   gamma 30.928 contro la mensola dei POC 30.890-30.922 e sopra il POC del composito di cassa 30.770.
4. **Macro**: regime choppy e velocita' morta sono misurati alle 11:04, prima dei dati delle 14:30.
   Dopo i dati il pannello va riletto, non estrapolato.
