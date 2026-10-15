# Fase 5 · Il Quadro · 2026-10-15

**Stato**: fatto, su quattro rapporti di cui tre parziali (1-macro, 3-gamma, 4-orderflow); nessun
rapporto mancato.
**Dati**: `1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`. Raccolta alle 10:45 ora italiana,
chiusa alle 10:51 (`raccolta.json`), nessuna fonte mancata. Ho controllato nei JSON: lo zero gamma e
i muri (`gex-NQZ6.json`, 08:45 UTC = 10:45), i percentili del COT (`cot.json`, report del martedi'
6 ottobre, 9 giorni), le sedute del 17/09, 18/09, 01/10 e 14/10 (`sedute-NQZ6.json`). Freschezza:
COT a 9 giorni, open interest delle opzioni a ieri, rendimenti e VXN alla chiusura di ieri, prezzo
NQZ6 delle 10:45 (non e' cassa: la cassa apre alle 15:30).

**In una riga**: tassi alti e credito al minimo di un anno, speculatori del Nasdaq ai massimi di
lunghezza a una fotografia di nove giorni fa, gamma negativa nella convenzione dei fornitori, valore
di cassa in discesa da sei sedute; alle 10:45 il prezzo ruota sul cuore del composito di 90 giorni.

## Il Quadro In Cinque Righe

1. **Macro.** 10 anni al 5,19% (14/10, 94,8° percentile a un anno); Polymarket da' 24,5% a un rialzo
   il 28/10; HYG (ETF del credito ad alto rendimento) al minimo di un anno; Fear & Greed 29,8 (paura);
   vendita concentrata su tecnologia e semiconduttori (SMH -9,65% in cinque giorni) [1-macro].
2. **Posizionamento.** Al 6/10 i Non-Commercial (speculatori, vista Legacy futures only) hanno il
   netto lungo al 100° percentile a tre anni e il campo short al percentile 0,6; flusso ordinario
   (z 0,58). Il prezzo e' sceso di circa il 5,3% dopo la fotografia [2-cot].
3. **Gamma.** Nella convenzione A (dealer lunghi le call e corti le put, quella dei fornitori) la
   GEX (gamma dei dealer, in milioni di $ per 1% di movimento) e' -1.719,4 su NQ; NQ sta 3,6% sotto
   lo zero gamma 30.927,7. Il segno vero non si conosce [3-gamma].
4. **Valore.** Cassa (15:30-22:00 ora italiana) con valore in discesa da sei sedute e staccato da
   tre; ieri 30.082-30.478, POC (prezzo di massimo volume) 30.173 [4-orderflow].
5. **Adesso (10:45).** Prezzo 29.822,50, 259,50 sotto il valore di ieri, 5 punti sopra il POC del
   composito aggiustato di 90 giorni (29.817,50, barre di 24 ore); il pannello misura CHOPPY,
   efficienza 0,09 [4-orderflow].

## I Fatti

**La domanda finale** (`il-framing-e-il-cot-si-leggono-insieme.md`, passo 5): il lato che il COT dice
posizionato sta sopra o sotto il cuore del volume? Due numeri: vertice del composito aggiustato
29.817,50 (`tab-ruoli.json`), prezzo 29.822,50 (10:45). Il prezzo sta **sul** vertice. Il lato
posizionato, il netto lungo, e' stato fotografato a 31.480,75 (chiusura di cassa del 6/10): 1.663,25
punti **sopra** il vertice (sottrazione fatta qui). Con il composito grezzo (POC 29.517,50) la
risposta non cambia verso. Riserva: composito di barre di 24 ore, non di sola cassa.

**I livelli confermati da piu' fasi.** Soglia: due prezzi di fasi diverse entro **15 punti** su NQZ6
(la soglia della skill). Distanza dal prezzo delle 10:45 (29.822,50). "Stesso dato" vuol dire che le
due fasi leggono lo stesso numero di `sedute-NQZ6.json` per ragioni diverse: la conferma e' di
lettura, non di fonte.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.530 (+1.707,5) | COT, order flow (stesso dato) | COT: POC del 6/10, data del netto al massimo di tre anni. OF: il POC di cassa piu' alto della scala delle 20 sedute | posizionamento: il prezzo dell'ultima fotografia COT. E' fuori dalla griglia della GEX (+-4%): la gamma non dice niente |
| 30.922-30.927,7 (+1.100) | COT, order flow, gamma | COT: POC del 22/09, la variazione del netto piu' grande (z 1,96; settimana di scadenza). OF: mensola di tre POC 30.890-30.922. Gamma: zero gamma A di tutte le scadenze 30.927,7 | posizionamento: li' la descrizione del regime cambierebbe (GEX A positiva sopra) e il prezzo tornerebbe sul valore della settimana del 22/09. L'unico livello con una fonte indipendente dal profilo; lo zero sta vicino al bordo della griglia |
| 30.609 (+786,5) | COT, order flow (stesso dato) | COT: POC del 29/09, unica variazione negativa (debole). OF: primo scaffale di POC 30.609-30.634 sopra il buco 30.173-30.609 | posizionamento debole: il primo valore di cassa sopra il buco di POC |
| 30.173-30.185 (+350,5) | macro, order flow (stessa seduta) | macro: chiusura di cassa del 14/10, riferimento del gap d'apertura (-362,5 alle 10:46). OF: POC della seduta della rottura | riferimento: il centro e la chiusura della seduta che ha portato il valore sotto tutti quelli dal 21/09 |
| 29.737-29.754 (-68,5 / -85,5) | macro, order flow, gamma | macro e OF: minimo della notte 29.754 (09:06). OF: mensola dei POC del 17 e 18/09 (29.730-29.737). Gamma: strike 29.750, terzo peso B dello 0DTE (OI put 817) | posizionamento: sotto il minimo la notte non ha scambiato; la mensola e' l'ultimo valore di cassa prima del rialzo del 21/09. Lo strike sta a 4 punti dal minimo e a 13 dal POC del 17/09 |

**Fuori soglia, scritti per non ripeterli.** 30.000 (muro unico di tutte le scadenze) sta 18 punti
sopra il VAH della notte 29.982 e 82 sotto il VAL di ieri 30.082: e' **dentro** il vuoto fra i due
valori, non su un bordo. 30.502,7 (zero 0DTE) dista 24,7 dal VAH del 14/10 (30.478). 29.800 (primo
peso B dello 0DTE) dista 17,5 dal POC del composito; le due zone (0DTE 29.700-29.800, Londra
29.781-29.838) si sovrappongono solo su 29.781-29.800. Il put wall 0DTE 29.700 dista 30-37 dalla
mensola 29.730-29.737. Il valore COT del 15/09 (29.215-29.315) dista 25 dal VAL del composito 29.340.

**Una correzione verificata.** La fase 1 scrive che il volume di cassa del 14/10 (540.320) e' il piu'
alto delle 20 sedute. In `sedute-NQZ6.json` il 01/10 ha 594.818: il 14/10 e' il **secondo**, come
scrivono le fasi 3 e 4.

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| 1-macro 1: credito al minimo e paura contro il netto COT | **contraddizione** | al 6/10 gli speculatori erano al massimo della lunghezza; credito e paura sono di oggi. Lo scarto di nove giorni non si chiude prima del COT del 16/10 |
| 1-macro 2: contango con gamma negativa | niente da confermare | GEX A negativa su NQ e su ES; la "fragilita'" vale solo se vale la convenzione A, e il segno non si conosce [3-gamma] |
| 1-macro 3: discesa del 14/10 passiva? | **conferma** | delta di cassa +530 su 540.320 (0,1%): nessun lato aggressivo netto (osservazione della fase 4). La notte invece ha due spinte in vendita (-1.320 alle 00, -1.243 alle 08) che hanno ottenuto prezzo, e il CVD segue il prezzo. Convenzioni: -603,75 sulla chiusura del 13/10 (macro), -366,5 dall'apertura (OF) |
| 1-macro 4: 14:30 e 28/10 | **conferma** | la gamma: l'implicita 0DTE (33,7%) contiene le 14:30. L'order flow: regime e velocita' sono misurati alle 10:45 e vanno riletti dopo i dati |
| 2-cot 1: netto al massimo contro il regime macro | **contraddizione** | vedi macro 1. In piu': il libro e' lungo sull'indice che guida la vendita (QQQ -3,80% contro RSP +0,71% in cinque giorni), mentre S&P e Russell hanno i venditori sull'acceleratore. **Osservazione** |
| 2-cot 2: valori COT contro muri e zero gamma | **conferma** per 30.922 (zero 30.927,7, a 5,7 punti); niente per gli altri tre | 31.530 e' fuori dalla griglia della GEX; 30.609 dista 106 dallo zero 0DTE; 29.250 non ha strike pesanti entro 15 |
| 2-cot 3: sopra o sotto il cuore del volume | risposto | i lunghi fotografati sopra (31.480,75), il prezzo sul vertice (29.817,50). Vedi "La domanda finale" |
| 2-cot 4: il delta dopo il 6/10 conferma la rottura? | **contraddizione** | le sei sedute dal 07/10 al 14/10 hanno delta di cassa +5.436, +9.344, -1.110, +1.248, -270, +530: somma +15.178 (fatta qui sui numeri della fase 4), con il valore in discesa. La prima aggressione netta in vendita e' la notte di oggi (-2.415) |
| 3-gamma 1: 30.000 contro COT e valore | niente entro soglia | 30.000 sta dentro il vuoto fra il valore della notte e quello di ieri; nessun valore COT vicino |
| 3-gamma 2: 29.800-29.700 contro Londra e composito | conferma parziale | le zone si toccano su 29.781-29.800; entro 15 punti c'e' solo 29.750 con il minimo della notte (tabella) |
| 3-gamma 3: gamma A negativa contro CHOPPY | **contraddizione** | il modello A vorrebbe copertura nel verso del movimento; il pannello misura rotazione (efficienza 0,09). La scala (0,7% del volume medio di cassa per 1%) dice che l'effetto puo' non vedersi |
| 3-gamma 4: put dominanti, implicita alta, paura | coerente con la macro, in tensione col COT | le put coprono sotto 30.100; il COT e' di prima del calo. L'OI non dice chi le ha comprate |
| 4-orderflow 1: il COT successivo riflette lo shift? | in parte | il COT del 16/10 copre 7-13/10: cinque delle sei sedute di valore in discesa, non il 14/10 ne' la notte |
| 4-orderflow 2: muri 0DTE contro bordi di cassa | niente | put wall 29.700 a 30-37 dalla mensola; call wall 30.300 dentro il valore di ieri, 127 sopra il POC e 178 sotto il VAH |
| 4-orderflow 3: muro 30.000 e zero 30.921 | conferma per lo zero, niente per il muro | lo zero (30.921 alle 10:36, 30.927,7 alle 10:45) sta sulla mensola 30.890-30.922; il muro sta dentro il vuoto 29.982-30.082 |
| 4-orderflow 4: pannello prima dei dati | **conferma** | come macro 4 |

## Cosa Dicono Per NQ

1. **Due scale, due letture, e non si fondono.** La settimana descrive momentum: valore giu' da sei
   sedute (`profile-framing.md` §5, come lo cita la fase 4). La seduta descrive bilancia: prezzo sul
   vertice del composito, dentro Londra, regime CHOPPY. Il quadro dichiara le due cose, non sceglie.
2. **Il posizionamento e' vecchio quanto il calo.** Tutto il -5,3% e' dopo la fotografia del 6/10.
   La sola cosa che il COT dice di oggi e' dove i lunghi hanno costruito: sopra il cuore del volume.
   Come per l'oro dell'8 settembre (`il-framing-e-il-cot-si-leggono-insieme.md`), il netto e' salito
   in una seduta che usciva dal valore dal basso. **Osservazione**: un caso solo, nessuna soglia
   COT e' validata.
3. **Il valore e' sceso senza vendite aggressive nette in cassa, la notte no.** Sei sedute di shift
   con delta di cassa sommato positivo; poi una notte con delta -2.415 e due spinte orarie che hanno
   ottenuto prezzo. **Osservazione**: nessun documento del metodo dice come pesare il delta su piu'
   sedute.
4. **L'unico livello con fonti indipendenti e' lontano.** 30.922-30.927,7 unisce il profilo del 22/09
   e un modello di opzioni, a 1.100 punti dal prezzo. Vicino al prezzo, la sola coincidenza fra
   fonti diverse e' 29.750 con il minimo della notte e la mensola del 17-18/09.
5. **Il contesto e' coerente su tre fasi su quattro**: tassi, credito, paura, put dominanti, valore
   in discesa. Lo stona il COT, che pero' e' di prima.

## Le Contraddizioni

Scritte, non risolte.

- **COT contro macro e gamma.** Speculatori al massimo della lunghezza (6/10) contro credito al
  minimo, paura, catena di put e prezzo -5,3%. Il tempo spiega forse lo scarto; non si sa.
- **Gamma contro order flow.** GEX A negativa (continuazione nel modello, BPVZ) contro regime
  misurato CHOPPY e velocita' morta. Il segno della GEX e' un'ipotesi; il regime e' misurato alle 10:45.
- **Valore contro delta.** Valore in discesa da sei sedute con delta di cassa sommato positivo.
- **Volatilita'.** Struttura del VIX in contango ("la volatilita' non ha paura", fase 1) contro
  implicita 0DTE di NQ al 33,7% e VXN +7,38% in un giorno. Contengono orizzonti diversi (le 14:30).
- **Fonti dello stesso numero** (dichiarate dalla fase 4, non risolte): VAL della notte 29.808
  (`sedute-NQZ6.json`) contro 29.844 (indicatore); minimo delle 20 sedute 29.605,75 (`sedute`, 17/09,
  verificato) contro 29.648 (giro); POC di ieri 30.173 (M1) contro 30.187 (Range). Zero gamma 30.921
  (giro, 10:36, future 29.823,75) contro 30.927,7 (`gex-NQZ6.json`, 10:45, future 29.854).
- **Scarto di rollover**: +299,25 nel composito contro +293,75 di `profile-framing.md`.

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| giovedi' 15/10, prima dell'apertura USA (ora non data) | trimestrale di TSM (EPS atteso 4,45 $) | il tema AI e semiconduttori, motore della vendita (fase 1) |
| 15/10 14:30 | vendite al dettaglio, PPI, Philly Fed, sussidi | il prezzo dei tassi (rialzo al 24,5%); il regime e la velocita' del pannello, da rileggere |
| 15/10 15:30 | apertura della cassa | il primo profilo di cassa di oggi; il gap sulla chiusura 30.185 |
| 15/10 21:30-22:00 | ultima mezz'ora di cassa | la finestra di BDLM, ipotesi mai provata sui nostri dati |
| 15/10 22:00 | scadenza 0DTE di NQ ed ES | esce la gamma dello 0DTE (-584,8 su -1.719,4 di NQ) |
| venerdi' 16/10 05:30 | discorso di Warsh, presidente della Fed | il prezzo dei tassi, di notte |
| 16/10 21:30 | COT, posizioni di martedi' 13/10 | il posizionamento dopo cinque sedute di calo; il quadro si rifa |
| 16/10 22:00 | scadenza mensile (terzo venerdi', OI NQ 16.207) | la gamma della settimana; ipotesi GJ e BTW |
| 20-21/10, 28/10 19:00 | NFLX; META e TSLA; FOMC con GOOGL e MSFT dopo la chiusura | contesto delle prossime due settimane |

## Cosa Smentirebbe Il Quadro

Fatti osservabili, non previsioni.

- La cassa **riaccetta il valore del 14/10** (prezzo che torna e resta sopra il VAL 30.082): la riga
  "valore in discesa, prezzo sotto ieri" diventa falsa (fase 4).
- Il prezzo **esce dalla bilancia di Londra** (29.781-29.838) e dal vertice 29.817,50: la riga "fair
  value del composito" diventa falsa, e il quadro va rifatto (`il-framing-e-il-cot-si-leggono-insieme.md`,
  "Quando si rifa").
- Il pannello, dopo le 14:30, **non misura piu' CHOPPY**: cade la contraddizione con la gamma.
- **VIX9D sopra VIX3M** (18,79 alla chiusura di ieri): la struttura si inverte e la lettura "la
  volatilita' non ha paura" cade.
- **ES che scende quanto NQ**, o RSP che cede quanto QQQ: cade la lettura "vendita settoriale".
- **Il COT del 16/10** con il netto ancora al massimo smentisce l'ipotesi che il calo sia stato
  accompagnato da una riduzione; un netto in calo forte smentisce l'opposta.
- **La lettura GEX di domani** con lo zero spostato oltre la griglia o il muro lontano da 30.000.

## Cosa Non Sappiamo

- Il CPI di ieri non e' verificato alla fonte; il quadro ne usa solo la direzione dei titoli.
- Il segno dei dealer (convenzione A o B) e chi ha comprato le put.
- Se i lunghi del 6/10 sono ancora li': il COT ha nove giorni.
- Tre dei cinque livelli confermati sono conferme di lettura dello stesso dato, non di fonte.
- Nessuna soglia COT, nessun livello di gamma e nessuna confluenza sono validati: tutto e' descrizione.

## Le Lacune Del Collettore

Il lavoro per il codice, raccolto dalle quattro fasi e da questa.

- lacuna del collettore: FRED (fed funds, inflazione attesa, spread high yield, stress), senza `FRED_API_KEY`.
- lacuna del collettore: il dato uscito degli eventi del calendario; il calendario della settimana prossima (404).
- lacuna del collettore: rendimenti, VXN, VIX9D e VIX3M intraday di oggi.
- lacuna del collettore: probabilita' FOMC da future sui fed funds o FedWatch; ampiezza del Nasdaq-100.
- lacuna del collettore: esito delle trimestrali; testo delle notizie (solo titoli RSS).
- lacuna del collettore: `settimana_di_rollover` solo per l'ultimo report COT, non per `ultime_4`.
- lacuna del collettore: il ritardo in prezzo del COT e il profilo della settimana coperta dal report.
- lacuna del collettore: la TFF senza variazioni di long e short, z52, percentili e ultime quattro.
- lacuna del collettore: il volume del future ES (`sedute-ESZ6.json`); la GEX per singola scadenza.
- lacuna del collettore: catena tagliata a 100 strike per lato; `per_strike` e griglia solo +-4%.
- lacuna del collettore: l'ora del prezzo sottostante della GEX; la finestra del volume delle opzioni.
- lacuna del collettore: la volatilita' realizzata intraday dalle barre di ATAS.
- lacuna del collettore: il composito di sola cassa e il suo profilo per fasce; il profilo per fasce del 14/10.
- lacuna del collettore: lo shift letterale (VAL e VAH); il rango del volume nelle 20 sedute.
- lacuna del collettore: i big trade sui livelli (`big` null); la finestra del regime; la velocita' sulla barra chiusa.
- lacuna del collettore: la distribuzione della durata delle barre range; le soglie premium, fair value, discount.
- lacuna del collettore: lo scarto di rollover del composito con la convenzione di `profile-framing.md`.
- lacuna del collettore: una sola misura per numero (VAL notte, minimo 20 sedute, POC, zero gamma divergono).
- lacuna del collettore: la tabella delle confluenze fra fasi (prezzi entro soglia), oggi fatta a mano qui.

## Per Il Giro D'orizzonte

Il blocco QUADRO e' in `docs/research/giornate/NQZ6-2026-10-15.md`, file creato da questa fase con la
sezione 1 e il blocco; il resto e' da scrivere durante la seduta. Si rifa dopo le 14:30 se il prezzo
esce dal valore di Londra, e venerdi' 16/10 dopo le 21:30 con il COT nuovo.
