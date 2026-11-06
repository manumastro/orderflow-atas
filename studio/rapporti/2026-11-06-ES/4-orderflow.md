# Fase 4 · L'Order Flow Delle Quattro Tab · ESZ6 · 6 novembre 2026

**Stato**: parziale (manca la tab 4, le barre range: `range-ESZ6.json` vuoto, "TAB 4 non registrata nel bridge"; la cassa di oggi apre alle 15:30, dopo la raccolta)
**Dati**: `in-breve.json` (orderflow), `pannello.json`, `quattro-tab.txt`, `giro.txt`, `sedute-ESZ6.json`, `ore-ESZ6.json`, `range-ESZ6.json`. Raccolta del 6/11 alle 15:05 italiane (`raccolta.json`), ogni fonte "ok", nessun errore. Pannello alle 15:05 (14:05 UTC); sedute di cassa chiuse fino a giovedi' 5/11; notte fino alle 15:05; `oggi_cassa` vuota (attesa).

**In una riga**: dopo il crollo di giovedi' il prezzo sta fermo sul bordo basso del valore di ieri, molto sotto il valore del mese, e l'indicatore misura un mercato senza direzione.

## In Parole Semplici

**Il punto**: ieri il prezzo e' sceso di oltre 300 punti e ha chiuso vicino al minimo. Stanotte si e' fermato a girare in una fascia stretta, proprio sul bordo basso della zona scambiata ieri. Adesso l'indicatore dice "mercato che va avanti e indietro": nessuno spinge davvero.

**Dal piu' ampio al piu' vicino**

1. **Le settimane**: il prezzo (circa 7.390) sta circa 250 punti sotto la zona dove si e' scambiato di piu' nell'ultimo mese (7.640-7.810).
   → E' "a sconto": lontano dal prezzo che il mercato ha considerato giusto per settimane.
2. **I giorni**: giovedi' la zona piu' scambiata e' scesa tutta, staccata da quella di mercoledi', e la giornata ha chiuso sotto di essa.
   → Il valore si e' spostato in basso, non si e' solo allargato. Mese e settimana restano invece "in pausa".
3. **La notte**: dalle 00:00 quasi tutto si e' scambiato fra 7.380 e 7.408, sopra un minimo a 7.322 toccato subito e non piu' rivisto.
   → La notte ha costruito un piccolo equilibrio sul bordo basso di ieri: quel bordo e' ancora conteso.
4. **Adesso (15:05)**: in 30 minuti il prezzo ha percorso circa 123 punti per spostarsi di poco piu' di 1; tre rotture sono rientrate. I dati sul lavoro delle 14:30 hanno mosso circa 27 punti in mezz'ora.
   → Dopo l'esplosione di ieri il mercato "ricarica": come lo misura l'indicatore, e' choppy.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.406 | il prezzo piu' scambiato di ieri, il giorno del crollo | il prezzo torna dentro la zona di ieri: il crollo comincia a essere "digerito" |
| circa 7.388 | il bordo basso della zona di ieri, dove sta il prezzo ora | se la cassa scambia stabilmente sotto, il valore continua a scendere |
| 7.538-7.590 | un tratto che ieri e' stato attraversato in fretta | in teoria si riattraversa veloce, perche' li' c'e' poco scambiato |
| circa 7.640 | il bordo basso della zona del mese | li' finisce lo "sconto" |

**Quando**: 15:30 apre la cassa di New York · 16:00 fiducia dell'Universita' del Michigan
**Cambia idea se**: la cassa accetta prezzi sotto 7.380 (il valore scende ancora), o li accetta sopra 7.408 (si torna in equilibrio dentro la zona di ieri); il regime passa da choppy a direzionale.
**Quanto e' provato**: le misure sono dell'indicatore e del collettore. Che il bordo "conti" e che il centro di ieri richiami il prezzo e' il modello del corso, non provato su ES. Gli "scalini" sono smentiti come previsione su NQ: qui sono solo descrizione.

**Le parole**: **valore** = la fascia dove passa il 70% degli scambi della giornata; **POC** = il prezzo piu' scambiato; **VAL/VAH** = bordo basso/alto del valore; **sconto (discount)** = sotto il valore del mese; **choppy** = avanti e indietro senza direzione; **scalino** = candela che supera gli estremi della precedente e chiude oltre.

## I Fatti

**Tab 1, composito** (`in-breve.json`, `quattro-tab.txt`; solo cassa 09:30-16:00 New York = 15:30-22:00 italiane, salvo dove detto)

| misura | valore (VAL-VAH) | POC | zona del prezzo |
|---|---|---|---|
| composito di cassa, 20 sedute dal 9/10 | 7.640,25-7.810,75 | 7.754 | discount |
| composito di cassa "90", **solo 25 sedute** dal 2/10, nessuna prima del rollover | 7.676-7.854,5 | 7.754 | discount |
| tab 1, 90 barre Daily di 24 ore, grezzo | 7.620-7.860 | 7.767,5 | discount |
| tab 1 aggiustata al rollover (+67,75 prima dell'11/09) | 7.650-7.855 | 7.762,5 | discount |

Prezzo nel range delle ultime 20 sedute (7.322,25-7.887): al 12% (`giro.txt`, 15:05). Rendimento dalla chiusura di cassa di ieri (7.381,25): +10,25 punti, +0,139% (14:05 UTC = 15:05).

**Gli scalini** (`orderflow.scalini`, ultima candela chiusa, barre Daily per giorno CME): mese di ottobre **pausa** (massimo 7.897,5 e minimo 7.558,25 visitati, non accettati: chiude 7.614,5 dentro il range di settembre 7.575-7.848,5; ultimo scalino su al 31/08); settimana 26-30/10 **pausa** (massimo 7.819,5 visitato, non accettato; ultimo scalino giu' al 23/10); giorno 5/11 **scalino giu'** (7.346 sotto 7.490, chiude 7.353). Insieme: **misti**. Ping-pong: nessuno, l'ultimo giorno e' uno scalino.

**Tab 2, la settimana** (`sedute-ESZ6.json`, cassa M1)

| seduta | valore | POC | chiude | valore vs ieri | staccato | shift |
|---|---|---|---|---|---|---|
| ven 30/10 | 7.597,75-7.669,5 | 7.610 | dentro | giu' | si | giu' |
| lun 2/11 | 7.596-7.674 | 7.657 | sotto | su | no | nessuno |
| mar 3/11 | 7.589,75-7.609,25 | 7.597 | dentro | giu' | no | giu' |
| mer 4/11 | 7.715,5-7.741,75 | 7.729,25 | dentro | su | si | su |
| gio 5/11 | 7.388,25-7.537,75 | 7.406 | sotto | giu' | si | giu' |

Giovedi' 5: apertura 7.707,5, minimo 7.377,25, chiusura 7.381,25, volume 2.886.204, delta di cassa +1.610. La barra Daily di 24 ore dello stesso giorno ha delta -11.468 (`quattro-tab.txt`): finestre diverse, non si confrontano. Profilo di ieri (fasce da 10 sopra il 2%): 7.400-7.410 al 9,7%; da 7.540 a 7.620 fra 2,1% e 2,6% per fascia, e 7.540-7.550 e 7.560-7.570 sotto il 2%.

**Tab 3, la seduta** (notte dalle 18:00 di New York = 00:00 italiane, fino alle 15:05; `sedute-ESZ6.json`)

| notte | apertura | massimo | minimo | ultimo | volume | delta | POC | valore |
|---|---|---|---|---|---|---|---|---|
| 6/11 | 7.359 | 7.427,25 | 7.322,25 | 7.391,5 | 438.940 | -4.126 | 7.401 | 7.380-7.408,25 |

Profilo della notte: 7.380-7.410 contiene il 71,8% (20,9 + 28,4 + 22,5). Struttura oraria H1 (`giro.txt`, ore italiane): minimi di swing 7.322,25 (00h) e 7.351,25 (08h), massimi 7.417 (03h) e 7.427,25 (10h): massimi e minimi crescenti sugli ultimi due swing. Mezz'ore (`quattro-tab.txt`): 13:30 delta -1.577; 14:00 +294; **14:30 (occupati non agricoli) range 27, volume 36.382, delta -848**; 15:00 (aperta) -292. Tape: delta 15 minuti -518 (-3,4%), 30 minuti -1.082 (-3,4%), 60 minuti -355 (-0,5%), solo barre chiuse.

**Il regime come lo misura l'indicatore** (`pannello.json`, 15:05; orari del pannello italiani)

- **REGIME CHOPPY**: ultime 30 barre 14:36-15:05, efficienza 0,01 (netto 1,25 su strada 123,25), 3 rotture rientrate, copertura si.
- **Velocita'** (proxy, non la speed of tape): barra in formazione "morta", 14 lotti, 0° percentile su 60 barre; **barra chiusa 15:04: 933 lotti, 51° percentile**. 0 big trade da 268+ lotti in 10 barre.
- **Livelli e veti**: "nessun livello depositato"; 0 livelli da 0 regole; il file della giornata `ESZ6-2026-11-06.md` non esiste.
- Il giro, raccolto nello stesso minuto, legge efficienza 0,03 (3,50 su 128,50) e velocita' "normale", 1.014 lotti, 59°: e' la barra in formazione che cresce. Vale il pannello; la misura stabile e' la barra chiusa.

Volatilita' realizzata (`ore-ESZ6.json`, rendimenti log a 5 minuti, annua): 4 ore 15,2% · 24 ore 31,6% · 48 ore 26,0%.

**Tab 4, l'esecuzione**: nessun dato (barre range non registrate nel bridge).

## Cosa Dicono Per ES

- **Composito**: il prezzo e' in discount su tutte e quattro le misure, circa 250 punti sotto il VAL di cassa a 20 sedute. Il live: *"we are really low in the curve"* veta l'inseguire in basso `[5 · 1:01:05]`. Qui si dice solo dove si sta nella curva: molto in basso.
- **Settimana**: 30/10, 2/11 e 3/11 hanno valori sovrapposti e si uniscono in un blocco 7.589,75-7.674 `[1 · 24:02]`. Mercoledi' e giovedi' sono staccati, uno sopra e uno sotto: due rotture in due giorni, di verso opposto. Giovedi' e' uno **shift giu'** alla lettera del live (VAL **e** VAH piu' bassi) `[1 · 22:13]`.
- **Il POC della seduta che ha creato la rottura** e' 7.406 (giovedi'), e coincide quasi con il POC della notte (7.401): il live lo indica come il primo punto ritrovato sulla via del ritorno `[1 · 24:35]`. **Osservazione**: la notte ci ha gia' costruito sopra il suo valore.
- **Seduta**: il prezzo sta sul VAL di ieri (7.388,25), dentro il valore della notte. *"Il VAL e' una strada"* `[1 · 1:19:30]`: accettazione sotto vuol dire che il valore continua a sud, rientro vuol dire bilancia dentro il valore di ieri. Sotto 7.388 non c'e' valore di cassa nelle ultime 25 sedute: c'e' solo la notte (7.351,25, 7.322,25).
- **Regime**: choppy, come lo misura l'indicatore. E' coerente con il live: *"dopo una esplosione direzionale ci si aspetta il chop"* `[4 · 1:05]` (**osservazione**: realizzata 4 ore 15,2% contro 31,6% a 24 ore). Il regime si dichiara; il setup no.
- **Il vuoto**: fra il VAH di giovedi' (7.537,75) e il VAL del blocco 30/10-3/11 (7.589,75), 52 punti; e fra il VAH del blocco (7.674) e il VAL di mercoledi' (7.715,5), 41 punti. Il primo l'ha visitato ieri, ma con fasce sotto il 2,6%: transito, non mensola (`profile-framing.md`, la regola dello scaffale).
- **Scalini**: misti. Mese e settimana sono in pausa, con estremi visitati e non accettati; il giorno e' uno scalino giu'. Descrizione, non previsione: smentito su NQ 2017-2024 (`quant/studi/scala-dei-tre-tempi`).

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.754 (7.762,5 aggiustato) | POC composito di cassa 20 e 25 sedute; tab 1 | il centro del valore del mese: il fair value |
| 7.715,5-7.741,75, POC 7.729,25 | valore di mer 4/11, staccato, seduta della rottura su | il bordo sopra il secondo vuoto |
| 7.674-7.715,5 | vuoto fra VAH del blocco e VAL di mer 4/11 | transito |
| 7.640,25 (7.650 aggiustato, 7.676 a 25 sedute) | VAL dei compositi | uscita dal discount verso il fair value |
| 7.589,75-7.674, POC 7.597 / 7.610 / 7.657 | blocco unito 30/10-3/11 | la scala dei POC sopra il vuoto |
| 7.537,75-7.589,75 | vuoto fra VAH di gio 5/11 e VAL del blocco | transito; ieri attraversato con poco volume |
| 7.537,75 | VAH di gio 5/11 | il bordo alto del valore di ieri |
| 7.427,25 | massimo della notte, swing H1 delle 10h | il bordo alto della struttura di stanotte |
| 7.406 / 7.401 / 7.408,25 | POC di ieri (rottura giu'); POC e VAH della notte | rientro nel valore di ieri e nel suo centro |
| 7.388,25 / 7.380 | VAL di ieri; VAL della notte | la strada: sotto, valore che continua a scendere |
| 7.377,25 | minimo della cassa di ieri | sotto, prezzi che la cassa non ha mai scambiato |
| 7.351,25 / 7.322,25 | minimi di swing H1 (08h, 00h); minimo della notte | la fine della struttura di stanotte |
| 6/11 15:30 · 16:00 | apertura della cassa · fiducia del Michigan | la prima prova del VAL con la cassa |

## Cosa Non Sappiamo

- **lacuna del collettore: le barre range** (tab 4): `range-ESZ6.json` vuoto, verso, durata, delta e ritmo non misurati.
- **lacuna del collettore: il composito di cassa a 90 sedute** ne ha 25: l'archivio non arriva a tre mesi. Il composito tab 1 a 90 giorni usa barre di 24 ore, non la sola cassa.
- **lacuna del collettore: Londra** non ha un profilo separato dalla notte.
- **lacuna del collettore: `oggi_notte` senza il campo `cassa`** (l'orario vero di apertura): l'ho preso dalla regola delle 18:00 di New York.
- **lacuna del collettore: l'esito degli occupati delle 14:30** non e' nel calendario del giro (manca il dato uscito).
- La cassa di oggi non c'e': ogni lettura del VAL va rifatta dopo le 15:30. Le due letture del pannello nello stesso minuto differiscono per la barra in formazione.

## Per Il Quadro

1. **Gamma contro regime**: X-Ray dice acceleratore al prezzo (gamma -131, fase 3), l'indicatore dice choppy: due misure che oggi non si accordano, da confrontare con il movimento atteso di ±38 punti.
2. **Confluenza 7.637-7.640**: VAL del composito di cassa a 20 sedute e acceleratore piu' forte di X-Ray (`stesso_dato` falso), circa 250 punti sopra il prezzo.
3. **COT**: le sue date cadono tutte sopra 7.640 (VAL della settimana del report 27/10 a 7.649,75; POC a 7.690; POC del 20/10 a 7.754,5 sul POC del composito). Nessun livello del COT entro 15 punti dal prezzo.
4. **Macro**: gli occupati delle 14:30 hanno mosso 27 punti in mezz'ora; il crollo di giovedi' 5/11 e' senza causa dichiarata nel calendario.
