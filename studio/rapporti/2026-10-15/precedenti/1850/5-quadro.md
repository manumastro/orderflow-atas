# Fase 5 · Il Quadro · 2026-10-15 (aggiornamento delle 11:04)

**Stato**: fatto. Si basa su quattro rapporti: tre sono parziali (1-macro, 3-gamma, 4-orderflow),
nessuno manca. Rifa il quadro delle 10:45 (`precedenti/1110/5-quadro.md`) perche' una delle sue
smentite si e' avverata: il prezzo e' uscito dal valore di Londra.
**Dati**: `1-macro.md`, `2-cot.md`, `3-gamma.md`, `4-orderflow.md`. **L'eta' di ogni fase**,
rispetto alla raccolta chiusa alle 11:10 ora italiana (`raccolta.json`, tutte le voci `ok`):
- **macro** riusata: dati delle 10:45 (25 minuti prima), quotazioni delle 10:30-10:35; curva dei
  Treasury, VXN, VIX9D e VIX3M alla chiusura del 14/10;
- **COT** riusato: report di martedi' 6/10, cioe' 9 giorni;
- **gamma** nuova: catena NQZ6 letta alle 11:04, ESZ6 chiusa alle 11:10, open interest del 14/10;
- **order flow** nuovo: dati delle 11:04, barra in formazione.

Ho controllato nei JSON: lo zero gamma, i muri e il peso B (`gex-NQZ6.json`, `quando` 09:04 UTC =
11:04), le sedute del 17/09, 18/09, 01/10 e 14/10, `oggi_finora` e `composito_di_cassa`
(`sedute-NQZ6.json`), i compositi (`tab-ruoli.json`).

**In una riga**: tassi alti, credito al minimo di un anno, speculatori del Nasdaq al massimo di
lunghezza in una fotografia di nove giorni fa, gamma negativa nella convenzione dei fornitori,
valore di cassa in discesa. Alle 11:04 il prezzo e' sopra il bilancio di Londra, dentro il valore
della notte, e il regime misurato e' CHOPPY.

## Il Quadro In Cinque Righe

1. **Macro (10:45).** Il 10 anni e' al 5,19% (14/10, 94,8° percentile a un anno). Polymarket da' il
   24,5% a un rialzo il 28/10. HYG, l'ETF del credito ad alto rendimento, e' al minimo di un anno. Fear
   & Greed e' a 29,8 (paura). La vendita si concentra su tecnologia e semiconduttori: SMH -9,65% in
   cinque giorni [1-macro].
2. **Posizionamento (6/10, 9 giorni).** I Non-Commercial (gli speculatori; vista Legacy, solo
   future) hanno il netto lungo al 100° percentile a tre anni e il campo short al percentile 0,6. Il
   flusso e' ordinario (z 0,58). Dalla chiusura del 6/10 (31.480,75) alle 11:04 (29.867,75) il prezzo
   fa -1.613 punti, -5,1% (conto mio) [2-cot, 4-orderflow].
3. **Gamma (11:04).** La GEX (gamma dei dealer, in milioni di $ per 1% di movimento) e' -1.696,4
   nella convenzione A, quella dei fornitori (dealer lunghi le call e corti le put). Lo zero gamma di
   tutte le scadenze e' 30.900,6, il 3,47% sopra; quello dello 0DTE (la scadenza di oggi) e' 30.470,1;
   i muri sono a 30.000. Il segno vero non si conosce. In scala, la copertura vale lo 0,70% del volume
   medio di cassa [3-gamma].
4. **Valore.** Cassa = 15:30-22:00 ora italiana. Dal 07/10, cinque sedute su sei hanno lo shift giu'
   (VAL e VAH piu' bassi del giorno prima); il 09/10 e' "nessuno". Le ultime tre sono staccate. Ieri
   l'area di valore era 30.082-30.478 e il POC (prezzo di massimo volume) 30.173 [4-orderflow].
5. **Adesso (11:04).** Il prezzo e' 29.867,75: fuori da Londra (29.776-29.840), sopra; dentro il
   valore della notte (29.812-29.982, POC 29.920); 50,25 sopra il vertice del composito di 90 giorni
   (29.817,50, barre di 24 ore). Il pannello misura CHOPPY (efficienza 0,15, 6 rotture rientrate) e
   velocita' morta (proxy) [4-orderflow].

## Cosa E' Cambiato Dalle 10:45

| misura | 10:45 | 11:04 | effetto sul quadro |
|---|---|---|---|
| prezzo | 29.822,50, dentro Londra, sul vertice | 29.867,75, sopra Londra, +50,25 sul vertice | scatta la smentita "esce da Londra": quadro rifatto |
| zero gamma, tutte | 30.927,7 | 30.900,6 | resta dentro la fascia 30.888-30.922 |
| zero gamma, 0DTE | 30.502,7 | 30.470,1 | **nuova conferma**, entro 15 dal VAH del 14/10 |
| composito di sola cassa | assente | 20 sedute, POC 30.770, VAL 30.458 | seconda risposta alla domanda finale |
| shift | letto a mano, "giu' da sei sedute" | misurato: 5 su 6 | **correzione** della riga 4 del blocco delle 10:45 |
| regime | CHOPPY, efficienza 0,09, 2 rotture | CHOPPY, 0,15, 6 rotture | stessa contraddizione con la gamma |
| implicita NQ 0DTE / 16/10 | 33,7% / 27,4% | 32,4% / 23,7% | piu' bassa, sopra la realizzata (19,0%) |

## I Fatti

**La domanda finale** (`il-framing-e-il-cot-si-leggono-insieme.md`, passo 5): il lato che il COT dice
posizionato sta sopra o sotto il cuore del volume? Il lato posizionato e' il netto lungo, fotografato
alla chiusura di cassa del 6/10 (31.480,75; POC 31.530). Le sottrazioni sono fatte qui.
- **Composito di 90 giorni, aggiustato, barre di 24 ore**: vertice 29.817,50. I lunghi stanno 1.663,25
  punti **sopra**; il prezzo sta 50,25 sopra.
- **Composito di sola cassa, 20 sedute dal 17/09**: vertice 30.770. I lunghi stanno 710,75 sopra
  (31.480,75); il prezzo sta 902,25 **sotto**.
- Con le due scale la risposta per i lunghi e' la stessa: **sopra**. Nessuna delle due e' la regola del
  live (sola cassa, 90 giorni): manca il composito di cassa a 90 giorni (lacuna).

**I livelli confermati da piu' fasi.** Soglia: due prezzi di fasi diverse entro **15 punti** su NQZ6
(la soglia della skill). La distanza si misura dal prezzo delle 11:04 (29.867,75). "Stesso dato"
vuol dire che due fasi leggono lo stesso numero di `sedute-NQZ6.json` per ragioni diverse: e' una
conferma di lettura, non di fonte.

| prezzo | fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 31.530 (+1.662) | COT, OF (stesso dato) | COT: POC del 6/10, data del netto al massimo di tre anni. OF: il POC di cassa piu' alto delle 20 sedute | posizionamento: il prezzo dell'ultima fotografia COT |
| 30.888-30.922 (+1.020 / +1.054) | COT, OF, gamma | COT: valore del 22/09 (VAL 30.888, POC 30.922), la variazione del netto piu' grande (z 1,96; settimana di scadenza). OF: mensola di tre POC (30.890, 30.912, 30.922). Gamma: zero A di tutte le scadenze, 30.900,6 | posizionamento: sopra, la GEX A diventa positiva e il prezzo torna sul valore della settimana del 22/09. Fonte indipendente dal profilo. Lo zero dista 10,6 dal POC del 12/10 e 21,4 da quello del 22/09 |
| 30.609 (+741) | COT, OF (stesso dato) | COT: POC del 29/09, l'unica variazione negativa (debole). OF: primo scaffale di POC (30.609-30.634) sopra il buco 30.173-30.609 | posizionamento debole: il primo valore di cassa sopra il buco di POC |
| **30.458-30.478 (+590 / +610)** | OF, gamma | OF: VAH della cassa del 14/10 (30.478) e VAL del composito di cassa a 20 sedute (30.458). Gamma: zero A dello 0DTE, 30.470,1 | posizionamento: rientrare nella distribuzione di cassa recente coincide con il cambio di segno della gamma di oggi. **Vale fino alle 22:00**, poi lo 0DTE scade. Nuovo dalle 10:45 |
| 30.173-30.185 (+305 / +317) | macro, OF (stessa seduta) | macro: chiusura di cassa del 14/10, riferimento del gap d'apertura. OF: POC della seduta della rottura | riferimento: il centro e la chiusura della seduta che ha portato il valore sotto tutti quelli dal 21/09 |
| 29.730-29.754 (-114 / -138) | macro, OF, gamma | macro e OF: minimo della notte, 29.754 (09:06; tenuto due volte per il pannello). OF: mensola dei POC del 17 e del 18/09. Gamma: strike 29.750, terzo peso B dello 0DTE (OI put 817) | posizionamento: sotto il minimo la notte non ha scambiato. La mensola e' l'ultimo valore di cassa prima del 21/09. Lo strike sta a 4 punti dal minimo e a 13 dal POC del 17/09 |

**Fuori soglia, scritti per non ripeterli.**
- 30.000 (i due muri di tutte le scadenze) sta 18 sopra il VAH della notte (29.982) e 82 sotto il VAL
  di ieri: **dentro** il vuoto fra i due valori.
- 30.100 (primo strike con netta A positiva) dista 18 dal VAL di ieri.
- 30.500 (strike, volume call 836) dista 22 dal VAH del 14/10.
- 31.500 (OI call 1.852) sta dentro il valore COT del 6/10, ma a 17 dal VAL e a 30 dal POC.
- 29.800 (primo peso B dello 0DTE) dista 17,5 dal vertice 29.817,50. Dal VAL della notte dista 12
  secondo `sedute` (29.812) e 44 secondo l'indicatore (29.844): dipende dalla fonte, non lo conto.
- Il put wall dello 0DTE (29.700) dista 30 dalla mensola.
- 29.500 dista 17,5 dal POC del composito grezzo (29.517,50).
- Il VAH COT del 15/09 (29.315) dista 25 dal VAL del composito aggiustato.
- 30.770 (cuore della cassa) e 29.920 (POC della notte) li indica solo l'order flow.

**Una correzione verificata, ancora valida.** La fase 1 scrive che il volume di cassa del 14/10
(540.320) e' il piu' alto delle 20 sedute. In `sedute-NQZ6.json` il 01/10 ha 594.818: il 14/10 e' il
**secondo**.

## Gli Agganci, Uno Per Uno

| aggancio | esito | perche' |
|---|---|---|
| macro 1: credito al minimo e paura contro il netto COT | **contraddizione** | i lunghi al massimo sono del 6/10; credito e paura sono di oggi. Lo scarto di nove giorni non si chiude prima del COT del 16/10 |
| macro 2: contango con gamma negativa | niente da confermare | la GEX A e' negativa su NQ e su ES; la "fragilita'" vale solo nella convenzione A. ES sta a 33-43 punti dal suo zero, dentro la mossa attesa (+-51,4): il suo regime A puo' cambiare segno in giornata |
| macro 3: la discesa del 14/10 e' stata passiva? | **conferma** | delta di cassa +530 su 540.320 (0,1%), nessun lato aggressivo netto. La notte no: -1.320 alle 00 e -1.243 alle 08 hanno ottenuto prezzo. Il delta della notte e' -2.320 alle 11:04 (era -2.415 alle 10:46). Dalle 10 il delta orario e' positivo (+152) |
| macro 4: le 14:30 e il 28/10 | **conferma** | l'implicita dello 0DTE (32,4%) contiene le 14:30. Regime e velocita' sono misurati alle 11:04 e vanno riletti dopo i dati |
| COT 1: netto al massimo contro la macro | **contraddizione** | come macro 1. Il libro e' lungo sull'indice che guida la vendita (QQQ -3,80% contro RSP +0,71% in cinque giorni). **Osservazione** |
| COT 2: valori COT contro muri e zero | **conferma** per 30.888-30.980; niente per gli altri | lo zero (30.900,6) cade dentro il valore del 22/09, a 12,6 dal VAL; si e' spostato di 27 punti dalle 10:45. 31.500 sta dentro il valore del 6/10 ma fuori soglia dal POC. 30.557-30.650 e 29.215-29.315: nessuno strike pesante entro 15 |
| COT 3: sopra o sotto il cuore del volume | risposto | sopra, su tutte e due le scale; vedi "La domanda finale" |
| COT 4: il delta dopo il 6/10 conferma la rottura? | **contraddizione** | il delta di cassa delle sei sedute dal 07/10 somma +15.178 (fatto qui sui numeri della fase 4), mentre il valore scende. La prima aggressione netta in vendita e' la notte (-2.320) |
| gamma 1: 30.000 contro COT e valore | niente entro soglia | dentro il vuoto fra notte e ieri; nessun valore COT vicino |
| gamma 2: 29.800-29.700 contro Londra e composito | conferma parziale | le zone si toccano su 29.776-29.800. Entro 15 c'e' solo 29.750, con il minimo e la mensola |
| gamma 3: gamma A negativa contro CHOPPY | **contraddizione** | il modello A vorrebbe la copertura nel verso del movimento (BPVZ: continuazione); il pannello misura rotazione, ora con 6 rotture rientrate. La scala (0,70%) dice che l'effetto puo' non vedersi |
| gamma 4: put dominanti e implicita alta contro COT e macro | coerente con la macro, in tensione col COT | put sotto 30.100, implicita 0DTE 32,4% contro VXN 22,7: coerente con paura e credito. Il COT e' di prima del calo; l'OI non dice chi ha comprato le put |
| OF 1: il COT successivo vede lo shift? | in parte | il COT del 16/10 copre 7-13/10: quattro sedute di shift giu' e una "nessuno". Dal 6/10 al 13/10 la cassa fa -692 punti, circa il 43% dei -1.613 fino alle 11:04 (conto mio). Non vede il 14/10 ne' la notte |
| OF 2: muri dello 0DTE contro bordi di cassa | niente per i muri; **conferma** per lo zero 0DTE | put wall 29.700 a 30-37 dalla mensola; call wall 30.300 dentro il valore di ieri. Lo zero 0DTE (30.470,1 alle 11:04) sta a 7,9 dal VAH del 14/10. La fase 4 leggeva la GEX delle 10:45 dal giro |
| OF 3: muro 30.000 e zero 30.928 | **conferma** per lo zero, niente per il muro | alle 11:04 lo zero e' 30.900,6, dentro la mensola 30.890-30.922 e 130 sopra il POC del composito di cassa |
| OF 4: pannello prima dei dati | **conferma** | come macro 4 |

## Cosa Dicono Per NQ

1. **Tre scale, tre posizioni.** Sulla settimana il valore si sposta in giu': e' il modello di
   momentum (`profile-framing.md` §5, citato dalla fase 4). Sul composito di 90 giorni (24 ore) il
   prezzo e' fair value, vicino al picco. Sulla cassa delle 20 sedute e' sotto tutta la
   distribuzione. Il quadro dichiara le tre letture, non sceglie.
2. **Il posizionamento e' vecchio quanto il calo.** Tutto il -5,1% e' dopo la fotografia. Il COT
   dice di oggi solo dove i lunghi hanno costruito: sopra il cuore del volume, su tutte e due le
   scale. **Osservazione**: un solo caso simile (l'oro, 8 settembre); nessuna soglia COT e' validata.
3. **Il valore scende senza vendite aggressive nette in cassa; la notte no.** Le due spinte orarie in
   vendita hanno ottenuto prezzo. Dalle 10 il delta orario e' positivo e il prezzo e' risalito di 114
   punti dal minimo. **Osservazione**: un'ora, in un regime misurato choppy.
4. **Un livello nuovo con fonte indipendente.** 30.458-30.478 unisce il bordo alto della seduta della
   rottura, il bordo basso del composito di cassa e lo zero dello 0DTE. Dura fino alle 22:00.
   L'altro livello con fonte indipendente (30.888-30.922) resta, a circa 1.030 punti.
5. **Il contesto e' coerente su tre fasi su quattro**: tassi, credito, paura, put dominanti, valore
   in discesa. Stona il COT, che pero' e' di prima.

## Le Contraddizioni

Scritte, non risolte.

- **COT contro macro e gamma.** Speculatori al massimo della lunghezza (6/10) contro credito al
  minimo, paura, catena di put e prezzo -5,1%. Forse lo scarto lo spiega il tempo; non si sa.
- **Gamma contro order flow.** GEX A negativa (nel modello, continuazione) contro un regime misurato
  CHOPPY con 6 rotture rientrate. Il segno della GEX e' un'ipotesi.
- **Valore contro delta.** Valore in discesa da sei sedute, con il delta di cassa sommato a +15.178.
- **Volatilita'.** La struttura del VIX e' in contango ("la volatilita' non ha paura", fase 1).
  Contro: implicita 0DTE di NQ al 32,4% e VXN +7,38% in un giorno. Gli orizzonti sono diversi.
- **Il composito.** 90 giorni a 24 ore dice fair value (+50,25 sul vertice); 20 sedute di cassa dice
  590,25 sotto il VAL. Lo scarto di rollover del composito e' +299,25, quello di `profile-framing.md`
  +293,75.
- **Lo stesso numero da due fonti** (dichiarate dalla fase 4, non risolte):
  - VAL della notte: 29.812 (`sedute`) contro 29.844 (indicatore);
  - minimo delle 20 sedute: 29.605,75 (`sedute`, 17/09, verificato) contro 29.648 (giro);
  - POC di ieri: 30.173 (M1) contro 30.187 (Range); VAH di Londra: 29.840 contro 29.850;
  - zero gamma: 30.928 nella tab 2 del giro (cache delle 10:45) contro 30.900,6 nel JSON delle 11:04.
- **L'OI di ieri cambia fra le letture**: il 16/10 di NQ passa da 16.207 a 15.703, quello di ES da
  57.857 a 50.672. L'OI di ieri non dovrebbe muoversi (fase 3).

## Gli Orari Di Oggi

| quando (ora italiana) | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 15/10 prima dell'apertura USA (ora non data) | trimestrale di TSMC (EPS atteso 4,45 $). Il giro (`eventi.json`) annota "utile record sopra le stime"; nessuna fase l'ha verificato | il tema AI e semiconduttori, motore della vendita (fase 1) |
| 15/10 14:30 | vendite al dettaglio, PPI, Philly Fed, sussidi | il prezzo dei tassi (rialzo al 24,5%); regime e velocita' del pannello, da rileggere |
| 15/10 15:30 | apertura della cassa | il primo profilo di cassa di oggi; il gap sulla chiusura 30.185 |
| 15/10 21:30-22:00 | ultima mezz'ora della cassa | la finestra di BDLM, ipotesi mai provata sui nostri dati |
| 15/10 22:00 | scadenza dello 0DTE di NQ ed ES | esce la gamma dello 0DTE (-594,6 su -1.696,4 di NQ); il livello 30.458-30.478 perde la gamma |
| ven 16/10 05:30 | discorso di Warsh, presidente della Fed | il prezzo dei tassi, di notte |
| 16/10 21:30 | COT, posizioni di martedi' 13/10 | il posizionamento dopo quattro sedute di shift giu'; il quadro si rifa |
| 16/10 22:00 | scadenza mensile, terzo venerdi' (OI NQ 15.703 alle 11:04) | dopo, la GEX A di NQ resta circa un terzo; ipotesi GJ e BTW |
| 20-21/10; 28/10 19:00 | NFLX; META e TSLA; FOMC, poi GOOGL e MSFT dopo la chiusura | il contesto delle prossime due settimane |

## Cosa Smentirebbe Il Quadro

Fatti osservabili, non previsioni.

- Il prezzo **esce dal valore della notte** (sotto 29.812, o 29.844 per l'indicatore; sopra
  29.982): la riga "adesso" diventa falsa e il quadro va rifatto (`il-framing-e-il-cot-si-leggono-insieme.md`,
  "Quando si rifa").
- La cassa **riaccetta il valore del 14/10**, cioe' torna e resta sopra il VAL 30.082: cade "valore in
  discesa, prezzo sotto ieri".
- Il minimo della notte (29.754) **cede**: cade "il minimo ha tenuto due volte".
- Dopo le 14:30 il pannello **non misura piu' CHOPPY**: cade la contraddizione con la gamma.
- **VIX9D sopra VIX3M** (18,79 alla chiusura di ieri): la struttura si inverte e cade "la
  volatilita' non ha paura".
- **ES scende quanto NQ**, o RSP cede quanto QQQ: cade "vendita settoriale".
- **ES passa sopra il suo zero A** (7.795,4): cade "gamma A negativa su NQ e su ES".
- **Il COT del 16/10**: con il netto ancora al massimo cade l'ipotesi che il calo abbia portato una
  riduzione; con un netto in forte calo cade l'opposta.
- **Una lettura GEX** con lo zero fuori dalla fascia 30.888-30.922 o il muro lontano da 30.000.

## Cosa Non Sappiamo

- Il CPI di ieri non e' verificato alla fonte; il quadro usa solo la direzione dei titoli.
- Il segno dei dealer (convenzione A o B) e chi ha comprato le put.
- Se i lunghi del 6/10 sono ancora li': il COT ha nove giorni.
- Tre dei sei livelli confermati sono conferme di lettura dello stesso dato, non di fonte.
- Se l'OI che cambia fra le letture e' una catena che arriva un po' alla volta.
- Nessuna soglia COT, nessun livello di gamma e nessuna confluenza sono validati: tutto e' descrizione.

## Le Lacune Del Collettore

Il lavoro per il codice, raccolto dalle quattro fasi e da questa. Dalle 10:45 sono state risolte:
`gex_per_scadenza`, `future_quando`, `per_strike` a +-6%, lo shift letterale, un composito di cassa
(20 sedute).

- lacuna del collettore: FRED (fed funds, inflazione attesa, spread high yield, stress), manca `FRED_API_KEY`.
- lacuna del collettore: il dato uscito degli eventi; il calendario della settimana prossima (404).
- lacuna del collettore: rendimenti, VXN, VIX9D e VIX3M intraday di oggi.
- lacuna del collettore: probabilita' FOMC dai future sui fed funds o da FedWatch; ampiezza del Nasdaq-100.
- lacuna del collettore: esito delle trimestrali; testo delle notizie (solo titoli RSS).
- lacuna del collettore: `settimana_di_rollover` solo per l'ultimo report COT, non per `ultime_4`.
- lacuna del collettore: il ritardo in prezzo del COT; il profilo della settimana coperta dal report.
- lacuna del collettore: la TFF senza variazioni di long e short, z52, percentili, ultime quattro.
- lacuna del collettore: il volume del future ES (`sedute-ESZ6.json`).
- lacuna del collettore: catena tagliata a 100 strike per lato; griglia dello zero solo a +-4%.
- lacuna del collettore: un segnale di OI definitivo (data e ora dell'OI, o una rilettura di conferma).
- lacuna del collettore: la volatilita' realizzata intraday dalle barre di ATAS; la finestra del volume delle opzioni.
- lacuna del collettore: la tab 2 del giro legge la cache GEX delle 10:45 mentre i JSON sono delle 11:04.
- lacuna del collettore: il composito di sola cassa a 90 giorni e la sua zona (premium, fair value, discount).
- lacuna del collettore: l'unita' della "quota" dei nodi; il profilo per fasce della cassa del 14/10.
- lacuna del collettore: i big trade sui livelli (`big` null); la finestra del regime; la velocita' sulla barra chiusa.
- lacuna del collettore: la distribuzione della durata delle barre range da 40 tick.
- lacuna del collettore: lo scarto di rollover con la convenzione di `profile-framing.md`.
- lacuna del collettore: una sola misura per numero (VAL notte, minimo 20 sedute, POC, VAH Londra, zero gamma).
- lacuna del collettore: la tabella delle confluenze fra fasi (prezzi entro soglia), oggi fatta a mano qui.

## Per Il Giro D'orizzonte

Il blocco QUADRO in `docs/research/giornate/NQZ6-2026-10-15.md` e' sostituito con questo quadro; il
resto del file non e' toccato. La sezione 1 del file dice ancora "dati raccolti alle 10:45": l'ora
delle 11:04 sta nel blocco. Il quadro si rifa: se il prezzo esce dal valore della notte, dopo le 14:30
se il pannello cambia regime, e venerdi' 16/10 dopo le 21:30 con il COT nuovo.
