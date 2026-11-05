# Fase 5 · Il Quadro · 03/11/2026

**Stato**: parziale. Quattro rapporti su quattro, tutti nuovi, nessuno riusato. La macro e' parziale (JOLTS
e Apple da fonti web, non dal collettore); l'order flow e' parziale (manca la tab 4, barre range).
**Dati**: i quattro rapporti; `dati/confluenze.json`, `dati/in-breve.json`, `dati/raccolta.json` (raccolta
16:46-16:47 ora italiana, 15:46-15:47 UTC, nessun errore). Prezzo di riferimento NQZ6 28.341,50 alle 16:46
(pannello), 28.348,75 alle 16:47 (M1 del bridge). Yahoo `NQ=F` 28.298,75 (16:36-16:47) e' piu' basso di circa
45 punti: per i prezzi vale il bridge. Soglia delle vicinanze: 15 punti (`soglia_punti` di `confluenze.json`).
Cassa 09:30-16:00 New York, oggi 15:30-22:00 ora italiana (14:30-21:00 UTC; Italia e USA in ora solare).

**In una riga**: Nasdaq -10% in un mese per i giganti della tecnologia; speculatori ancora sbilanciati al
rialzo in un dato di prima del calo; gamma negativa nel modello ma piccola; prezzo sul bordo basso della zona
di venerdi'-lunedi', in un mercato a zig-zag che aspetta elezioni e AMD.

## In Parole Semplici

**Il punto**: NQ ha perso circa il 10% in un mese, per i giganti della tecnologia e non per i tassi. Oggi si e'
fermato proprio sul bordo basso della zona costruita venerdi' e lunedi', e va su e giu' senza scegliere.
Stanotte arrivano i risultati delle elezioni e i conti di AMD, e le opzioni di domani li pagano cari.

**Dal piu' ampio al piu' vicino**

1. **Il mondo**: in un mese il Nasdaq perde il 10%, l'azione media dell'S&P guadagna quasi il 4%; la Fed e' data in rialzo a dicembre quasi 9 volte su 10.
   → Escono soldi dai giganti della tecnologia, non dalla Borsa intera; e la paura sul Nasdaq e' piu' alta che nel 95% dei giorni dell'anno.
2. **Le posizioni**: gli speculatori restano lunghi quasi come mai in tre anni; i grandi gestori pero' hanno alleggerito come raramente fanno, con il prezzo verso 29.800.
   → La fotografia e' del 27 ottobre, prima di un calo di circa 1.450 punti: non dice cosa hanno fatto dopo.
3. **Le opzioni**: nel modello piu' usato chi vende opzioni oggi accelera i movimenti, fino a circa 28.700; le opzioni di domani costano molto piu' di quelle di dicembre.
   → E' una supposizione, e la copertura vale meno dell'1% degli scambi del future; il caro di domani e' la notte elettorale.
4. **Le settimane**: il prezzo sta sotto tutta la zona scambiata da settembre, e da mercoledi' 28 la zona di ogni giorno e' scesa.
   → E' "a sconto", e il mercato ha accettato prezzi piu' bassi per quattro sedute.
5. **I giorni**: venerdi' e lunedi' hanno costruito una zona sola, circa 28.340-28.890; lunedi' ha chiuso sotto, con la vendita aggressiva piu' forte di 20 sedute.
   → La notte si e' fermata: la rottura non e' continuata, ma il prezzo non e' rientrato.
6. **Adesso** (16:46): circa 28.340, sul bordo basso della zona; mercato lento a zig-zag, piu' vendite aggressive mentre il prezzo sale un poco.
   → Nessuna scelta fatta: dentro o fuori dalla zona si decide su questo bordo.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.040 | il minimo della notte | sotto, il prezzo esce da tutto cio' che si e' scambiato in 20 sedute |
| circa 28.340 | il bordo basso della zona di venerdi'-lunedi', con molte opzioni aperte | sopra e con scambi rientra nella zona; sotto ne resta fuori |
| circa 28.700 | il prezzo piu' scambiato di lunedi'; nel modello, dove le opzioni passano da accelerare a frenare | il centro della zona da cui si e' scesi; sopra, il modello cambia segno |
| circa 29.330 | il prezzo con piu' opzioni al rialzo, sul bordo alto della zona del 22 ottobre | lontano, circa 1.000 punti: oltre il movimento atteso di oggi e di domani |

**Quando**: 21:30 ultima mezz'ora · 22:00 chiude New York e scadono le opzioni di oggi · dopo le 22:00 AMD · notte 3-4 elezioni · mer 16:00 ISM servizi · ven 14:30 lavoro, 21:30 nuovo COT
**Cambia idea se**: il prezzo scende sotto circa 28.040 e ci resta; sale sopra circa 28.700 e ci resta; lo zig-zag diventa una direzione in cassa; dopo le elezioni la paura sulle opzioni di domani non scende.
**Quanto e' provato**: prezzi, volumi, posizioni e paura sono misurati; il comportamento di chi vende opzioni e' un modello con un segno supposto; nessuno studio del repository prova che questi prezzi fermino o attirino NQ, ne' come NQ reagisce a elezioni o trimestrali.

**Le parole**: **zona** (valore) = la fascia dove passa il 70% del volume di una o piu' sedute; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **sconto** = prezzo sotto la zona dei mesi; **vendita aggressiva** = chi vende subito al prezzo di chi compra; **zig-zag** (choppy) = rotture che rientrano; **speculatori / gestori** = due categorie del rapporto CFTC (COT) su chi tiene i future; **opzione al rialzo** (call) = il diritto di comprare a un prezzo fissato.

## Il Quadro In Cinque Righe

1. **Macro** (Yahoo 16:31-16:47, Treasury al 2/11): 10 anni 4,95% (+5,8 pb oggi, -37 pb in 20 sedute); FOMC 9/12 +25 pb all'89,6% (future, 16:30-16:35; ieri 82,7%); VXN 30,30 (percentile 94,8), VIX9D 21,53 sopra VIX3M 21,27; NQ -10,1% contro RSP +3,8% in 20 sedute; Fear & Greed 20,1.
2. **Posizionamento** (CFTC, 27/10, 7 giorni): Non-Commercial Nasdaq netto 48.839, percentile 95,0; corti al 3,8; variazione -1.887 (z52 -0,17). TFF Asset Manager -12.409 (z52 -2,06). Prezzo sotto il valore di tutte e quattro le date, -1.448 dal POC del 27/10 (29.805).
3. **Gamma** (`gex.py` 16:45-16:47, convenzione A, segno supposto): tutte -614,6 M$, zero 28.708 (+360); 0DTE -270,0 (44%), zero 28.565; put wall 28.500; call wall 29.330 (viva 30.000); 1.084 future per l'1%, 0,23% del volume di cassa. Volatilita' ATM 4/11 44,2% contro 18/12 25,7%.
4. **Valore** (cassa 15:30-22:00): shift giu' il 29/10, il 30/10 e oggi in corso; blocco venerdi'-lunedi' 28.340-28.891, POC 28.778 e 28.695; lunedi' chiude sotto (28.137,75) con delta -7.202; sconto su tutti i compositi (VAL di cassa 28.996-28.998).
5. **Adesso** (pannello 16:46): 28.341,50, 1,50 sopra il VAL del blocco; cassa 28.285-28.358, POC 28.313; CHOPPY, efficienza 0,00; CVD -2.821 (-1,9%) al minimo con il prezzo al 73% del range; realizzata 4 ore 33,7%.

## Gli Agganci, Uno Per Uno

**Dalla macro**

- *VXN al 95° e VIX9D sopra VIX3M: coerente con le opzioni per la notte elettorale?* **Conferma**: la curva di NQ e' invertita (4/11 44,2%, 6/11 39,4%, 18/12 25,7%), mossa attesa del 4/11 724 punti. Quale evento pesi (elezioni o AMD) non si separa, lo dicono tutte e due le fasi.
- *RSP +3,8% contro NQ -10,1%: il COT segue la rotazione?* **Contraddizione parziale**: la Legacy no (speculatori al 95°, corti al 3,8°); la TFF si' (Asset Manager -12.409, z -2,06, aprendo corti). Vedi Le Contraddizioni, 1.
- *Fed all'89,6% e 10 anni +6 pb: pesa sull'order flow della cassa?* **Niente di conclusivo**: delta negativo in ogni mezz'ora (-1.349, -951, -371) con il prezzo in salita (+0,72%); l'ora del JOLTS (16:00) ha delta -1.475 e prezzo +56. Tassi e delta non si separano con questi dati.
- *Le date di rischio*: **conferma** con la gamma (la scadenza del 4/11 e' la piu' cara) e con l'order flow (la seduta entra nella settimana alle 22:00).

**Dal COT**

- *Speculatori lunghi e gestori in taglio: regime macro o scarico?* Come sopra. Il COT del 3/11 (esce venerdi' 21:30) e' il primo che vede il calo.
- *29.805 e 30.775 contro muri e zero gamma*: **29.805 coincide con lo zero della sola scadenza 18/12** (29.805, `3-gamma.md`), aggiunta del quadro; quella scadenza ha 189 opzioni quotate su 394, quindi il numero e' fragile. **30.775 sta in terreno libero**: il call wall piu' alto (30.000) e' 775 punti sotto.
- *Spazio prima del primo prezzo di posizionamento*: dal prezzo ai POC dell'order flow 28.695 (+354) e 28.778 (+437); al primo prezzo del COT, POC della settimana 21-27/10 29.500, circa +1.160. In mezzo c'e' il vuoto della settimana 28.891-29.482, con la mensola del mese 29.054-29.344.
- *Cross-confirm Nasdaq 1 su 4 contro la correlazione NQ-ES*: **niente da dire** sulla correlazione, che non c'e' in `dati/` (lacuna). Indizio di contesto: NQ -10,1% contro ES -3,55% in 20 sedute, VXN/VIX 1,47; il segno supposto delle opzioni e' lo stesso su NQ e su SPX (X-Ray).

**Dalla gamma**

- *Zero gamma 28.708 contro POC di lunedi' 28.695*: **conferma**, fonti diverse (`stesso_dato` falso). Il COT non ha livelli vicini.
- *Strike 28.250, 28.300, 28.350 contro POC notte 28.250, VAL di oggi 28.285, VAL di lunedi' 28.340*: **conferma**, con una riserva: vicino al prezzo gli strike stanno ogni 50 punti, quindi ogni prezzo e' entro 25 punti da uno strike, e una soglia di 15 ne prende per caso la maggior parte. Il VAL di oggi e' in corso (28.278-28.285 secondo la fonte).
- *Call wall 29.330 contro VAH del 22/10 29.344*: **conferma**; i livelli del COT partono da 29.584, sopra.
- *Curva invertita contro il calendario*: **conferma**. *Acceleratore supposto contro CVD negativo e choppy*: **contraddizione**, vedi 2.

**Dall'order flow**

- *POC 28.695 a 13 punti dallo zero 28.708: due fonti o lo stesso livello?* `confluenze.json` c'e' (16:47): fonti diverse, **conferma**.
- *Put wall 28.500 dentro il blocco; call wall 0DTE 28.400 fra i massimi*: 28.500 dista 54 punti dal VAL di venerdi' 28.446, **niente da dire** come vicinanza; sta nella fascia sottile di lunedi'. 28.400 e il massimo di cassa 28.389: 11 punti, **conferma debole** (aggiunta del quadro): il massimo e' in corso e il call wall 0DTE alterna 28.200 e 28.400.
- *Il COT precede la discesa*: **conferma**, la stessa cosa nei due rapporti (-1.448 e circa -1.460).
- *Choppy e ritmo lento con gamma negativa*: **contraddizione**, vedi 2.

## I Livelli Confermati

Almeno due fasi entro 15 punti (`confluenze.json`, piu' le aggiunte marcate). Nessun livello e' validato
come appoggio o respinta.

| prezzo | quali fasi | cosa dice ciascuna | a cosa serve arrivarci |
|---|---|---|---|
| 29.805 | COT, gamma (aggiunta del quadro) | COT: POC del 27/10, taglio Asset Manager z -2,06 (= POC cassa 27/10 dell'order flow, stesso dato); gamma: zero della scadenza 18/12 | posizionamento: dove i gestori hanno alleggerito; +1.460, lontano |
| 29.330-29.344 | gamma, order flow | gamma: call wall di tutte e di 7 giorni, GEX A +29,3; OF: VAH cassa 22/10, bordo alto della mensola del mese | posizionamento: il peso maggiore delle call, sopra il vuoto della settimana |
| 28.695-28.708 | gamma, order flow | gamma: zero di tutte 28.708 (viva 28.710); OF: POC di lunedi', centro del blocco | invalidazione del regime supposto (sopra, freno in teoria) e centro del blocco |
| 28.389-28.400 | gamma, order flow (aggiunta del quadro, debole) | gamma: call wall 0DTE 28.400 (alterna 28.200); OF: massimo di cassa in corso 28.389 | posizionamento dello 0DTE fino alle 22:00 |
| 28.340-28.350 | gamma, order flow | gamma: strike 28.350, GEX A -26,1; OF: VAL di lunedi' e del blocco | il bordo in gioco: dentro o fuori dal blocco |
| 28.285-28.313 | gamma, order flow | gamma: strike 28.300, GEX A -32,4; OF: VAL di cassa in corso 28.285, POC di cassa 28.313 (aggiunta) | la bilancia di oggi, in movimento |
| 28.250 | gamma, order flow | gamma: strike 28.250, GEX A -44,3; OF: POC della notte | il centro della notte, sotto il valore di oggi |

Esclusi: 29.584-29.599, 29.637, 29.757-29.770, 29.840-29.854, 29.880-29.883, 30.712, 30.775, 30.800 e
31.483-31.556 (stesso dato, COT e order flow leggono lo stesso profilo del bridge); 30.173-30.178,
30.824-30.834 e 30.890 (vicinanze fra profili del bridge di finestre diverse, a oltre 1.800 punti, nessuna
lettura delle fasi). 28.038 e 28.891 della macro sono gli stessi numeri del bridge dell'order flow (stesso
dato). Lo zero 0DTE 28.565 tocca il bordo della fascia sottile di lunedi' (28.560), ma l'order flow stesso
dice che la fascia davvero poco costruita e' 28.340-28.446: non entra.

## Le Contraddizioni

1. **Due tassonomie, due storie.** Legacy: speculatori lunghi al 95°, corti al 3,8°, taglio ordinario (z -0,17); il taglio piu' grande e' il 13/10. TFF: gestori -12.409 il 27/10 (z -2,06). La macro vede una rotazione fuori dal tech. Non si fondono e non si risolvono: il dato ha 7 giorni e precede tutto il calo.
2. **Acceleratore supposto, mercato a zig-zag.** Gamma A negativa su ogni orizzonte, ma l'indicatore misura efficienza 0,00 e 4 rotture rientrate (16:17-16:46). La realizzata a 4 ore (33,7%) e' alta per il picco delle 14:00, non per la cassa. La scala (0,23% del volume) dice che niente lega le due letture.
3. **Vendite aggressive, prezzo su.** Delta negativo in ogni mezz'ora, CVD al minimo, prezzo al 73% del range. L'order flow lo marca come possibile assorbimento, con il punto di domanda; la macro dice che il rimbalzo e' di pochi titoli (mediana +0,26% contro media +0,76%). Due osservazioni, nessuna misura.
4. **Tassi giu', Nasdaq giu'.** 10 anni -37 pb in 20 sedute e NQ -10%; oggi 10 anni +6 pb e NQ +0,7%. La macro lo legge come pressione settoriale: e' un'osservazione.
5. **Momentum o ritorno?** Sul bordo, con il valore che scende, il metodo indica il modello momentum; dentro il blocco, il ritorno al POC. Il prezzo e' 1,50 punti dentro: la misura non sceglie.
6. **Numeri che non coincidono**: call wall 29.330 (`gex.py`) contro 30.000 (viva), quasi alla pari; "max cash" del pannello 28.426,50 (e' il massimo della notte) contro 28.389 di `sedute`.

## Gli Orari Di Oggi

| ora italiana | evento | cosa potrebbe cambiare del quadro |
|---|---|---|
| 21:30-22:00 | ultima mezz'ora della cassa | la finestra di BDLM (momento con gamma negativa, sui future S&P): da registrare, non da usare |
| 22:00 | chiude la cassa; scadono le opzioni di oggi | esce il 44% della GEX A, lo zero 28.565 smette di esistere; la seduta entra nella settimana e si rilegge lo shift |
| dopo le 22:00 | AMD (EPS atteso 1,68 $) | i chip, il motore del rimbalzo di oggi |
| notte 3-4 | risultati delle elezioni, future aperti | la volatilita' del 4/11 si scarica o no; orari dei seggi non nei dati |
| 4/11 16:00 · 22:00 | ISM servizi (att. 54,8) · scadenza del 4/11, poi Qualcomm | la crescita; la scadenza piu' cara della curva |
| 5/11 14:30 | sussidi (att. 197 mila) | lavoro |
| 6/11 14:30 · 16:00 · 21:30 | lavoro (att. 65 mila) · Michigan · nuovo COT (dati di oggi) | la verifica della Fed; il primo COT che vede il calo |

## Cosa Smentirebbe Il Quadro

- Il prezzo scende sotto 28.038 e ci resta: esce da tutto il costruito delle 20 sedute, le righe Valore e Adesso sono da rifare.
- Il prezzo torna sopra 28.700 e ci resta: rientra al centro del blocco e il regime gamma A cambia segno.
- La cassa passa da zig-zag a direzionale: la riga Adesso e' superata.
- Dopo le elezioni la volatilita' del 4/11 non scende, o sale: la lettura "si paga un evento vicino" e' sbagliata.
- Il 10 anni e NQ si muovono forte in versi opposti: la pressione non era settoriale.
- Il COT del 6/11 mostra i gestori di nuovo in acquisto durante il calo: il taglio del 27/10 era isolato.

## Cosa Non Sappiamo

- Il segno vero dei dealer sulle opzioni CME di NQ; quanta gamma diventa ordini; come NQ reagisce a elezioni e trimestrali.
- Perche' NQ e' sceso di circa 790 punti ieri in cassa: nessuna notizia nei feed.
- JOLTS (7,52 mln) da CNBC, non verificato al BLS; Apple da ricerca web.
- Yahoo `NQ=F` e bridge differiscono di circa 45 punti nella stessa finestra: le variazioni della macro sono di Yahoo.

## Le Lacune Del Collettore

- Order flow: barre range della tab 4 non registrate nel bridge (`range-NQZ6.json` vuoto).
- Order flow: la fase ha letto prima che la raccolta chiudesse (`in-breve.json`, `giro.txt`, `confluenze.json`, `raccolta.json` assenti alla sua lettura, scritti alle 16:47): l'orchestrazione deve aspettare la fine del collettore.
- Order flow: "max cash" del pannello sulla finestra sbagliata; composito "90" con 43 sedute e aggiustamento al rollover non dichiarato; tre valori del VAL di oggi (28.278, 28.284, 28.285); big trade senza la copertura del registro del tape.
- Gamma: scadenze del 17, 18, 20 e 30/11 saltate; 18/12 con 189 quotate su 394; OI del 12 e 13/11 non stabile e confronto fra calcoli vuoto; call wall diverso fra `gex.py` e viva senza il perche'; nessun confronto con la GEX di ieri; azzeramento del volume alle 17:00 di Chicago non verificato.
- Confluenze: `confluenze.json` non contiene gli zeri per scadenza, i massimi e minimi di seduta in corso, i muri 0DTE; non segnala la griglia degli strike (50 punti) che rende le vicinanze vicino al prezzo quasi automatiche.
- Correlazione NQ-ES assente da `dati/`.
- Macro: `esiti` vuoto senza il perche'; esiti delle trimestrali passate; nessun mercato di previsione sulle elezioni ne' orari dei seggi; `2YY=F` fermo al 13/10; calendario della settimana prossima 404; nessun feed sull'Iran.
- COT: ora di pubblicazione del report in uso; TFF senza Other Reportable e Non Reportable; skew di fondo per categoria TFF.
