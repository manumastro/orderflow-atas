# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-11-06

**Stato**: parziale (manca la tab 4: le barre range non sono registrate nel bridge; la cassa di oggi apre alle 15:30, dopo la raccolta)
**Dati**: `dati/in-breve.json` (sezione `orderflow`), `pannello.json`, `quattro-tab.txt`, `giro.txt`, `raccolta.json`. Raccolta delle 15:05 ora italiana (14:05 UTC), ogni fonte "ok". Pannello delle 15:05; GEX di NQZ6 delle 15:02 (OI di ieri); GEX viva di NQ ferma da 20 ore. `range-NQZ6.json` vuoto, `oggi_cassa` vuoto.

**In una riga**: dopo il crollo di giovedi' il prezzo e' rientrato nel valore di giovedi', molto sotto il valore dei tre mesi, e l'indicatore misura un mercato che va avanti e indietro senza strada.

## In Parole Semplici

**Il punto**: NQ sta molto sotto la zona dove ha scambiato negli ultimi tre mesi. Stanotte ha recuperato circa 370 punti ed e' tornato nella zona costruita giovedi'. Adesso oscilla senza direzione, e i compratori aggressivi non dominano.

**Dal piu' ampio al piu' vicino**

1. **I tre mesi**: la zona piu' scambiata sta fra circa 29.000 e 31.000; il prezzo e' a circa 27.100.
   → Siamo quasi 1.900 punti sotto: prezzi "in saldo", che il mercato recente non aveva mai accettato.
2. **La settimana**: da venerdi' 30 a mercoledi' 4 il valore e' rimasto fra circa 28.300 e 28.900; giovedi' e' crollato e ha costruito a 26.800-27.400.
   → Fra le due zone c'e' un vuoto di quasi 1.000 punti dove nessuna seduta ha costruito valore.
3. **La notte**: da 26.366 a 27.251, con il prezzo piu' scambiato a 27.100.
   → Il prezzo e' tornato nella parte alta della zona di giovedi'.
4. **Adesso (15:05)**: in 30 minuti il prezzo ha percorso circa 816 punti per spostarsi di 30. Nelle ultime due ore hanno spinto di piu' i venditori, ma il prezzo non e' sceso.
   → E' un mercato in equilibrio di breve, non una corsa. La cassa delle 15:30 deve ancora parlare.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 27.100 | il prezzo piu' scambiato di stanotte e di Londra | e' il centro di adesso: restarci vuol dire equilibrio |
| circa 27.361 | il bordo alto della zona di giovedi' | oltre, il prezzo entra nel vuoto che sale fino a circa 28.300 |
| circa 26.900 | il prezzo piu' scambiato di giovedi', il giorno della rottura | tornarci vuol dire che il recupero della notte si e' spento |
| circa 26.829 | il bordo basso della zona di giovedi' | sotto, il prezzo esce di nuovo dalla zona, verso il minimo di stanotte 26.366 |

**Quando**: 15:30 apre la cassa di New York · 16:00 fiducia del Michigan (i dati sul lavoro delle 14:30 sono gia' usciti).
**Quanto e' provato**: livelli e regime sono misure dell'indicatore e del collettore. Che il prezzo torni al centro dentro una zona, o corra nel vuoto, e' la lettura del live di Fabio: nessuno studio la prova su NQ. La tab delle barre range manca.

**Le parole**: **valore** = la fascia dove si e' scambiato il 70% del volume; **POC** = il prezzo piu' scambiato; **delta** = compratori aggressivi meno venditori aggressivi; **choppy** = avanti e indietro senza strada; **discount** = sotto il valore del periodo; **vuoto** = fascia fra due valori dove non si e' costruito niente.

## I Fatti

**Tab 1 · Composito** (`in-breve.json` orderflow e `quattro-tab.txt`, alle 15:05; solo cassa salvo dove detto)

| composito | valore | POC | campione | zona |
|---|---|---|---|---|
| cassa 90 (regola del live) | 28.998-31.017 | 29.735 | 46 sedute dal 03/09, 6 prima del rollover | discount |
| cassa 20 | 28.788-31.180 | 29.840 | 20 sedute dal 09/10 | discount |
| tab 1, barre di 24 ore, grezzo | 28.330-30.255 | 29.517,50 | 90 barre dal 05/07 | discount |
| tab 1, aggiustato (+299,25 prima dell'11/09) | 28.995-30.780 | 29.817,50 | idem | discount |

Gli scalini (`orderflow.scalini`, ultima candela chiusa, barre CME di 24 ore): **mese** di ottobre in pausa (massimo 31.616,50 sopra i 31.094,75 di settembre, chiusura 28.543 sotto: visitato, non accettato; ultimo scalino su al 30/09). **Settimana** 26-30/10 in pausa (massimo 30.178 sopra 30.113, chiusura sotto; ultimo scalino giu' al 23/10). **Giorno** 05/11 scalino giu' (massimo 28.668,50, minimo 26.570, chiusura 26.604 sotto il minimo di prima, 27.615,75). Insieme: **misti**. Ping-pong: nessuno, l'ultimo giorno e' uno scalino.

**Tab 2 · Settimana** (`ultime_5_sedute`, cassa 09:30-16:00 New York = 15:30-22:00 italiane, M1)

| seduta | valore | POC | delta | volume | chiude | shift | staccato |
|---|---|---|---|---|---|---|---|
| ven 30/10 | 28.446-28.857 | 28.778 | +4.773 | 754.747 | dentro | giu' | si' |
| lun 02/11 | 28.340-28.891 | 28.695 | -7.202 | 537.136 | sotto | nessuno | no |
| mar 03/11 | 28.296-28.387 | 28.314 | -5.843 | 416.025 | dentro | giu' | no |
| mer 04/11 | 28.558-28.785 | 28.632 | +744 | 587.882 | dentro | su | si' |
| gio 05/11 | 26.829-27.361 | 26.900 | -4.781 | 985.683 | sotto (26.746,25) | giu' | si' |

Giovedi' ha aperto a 28.203,25 e toccato 26.718; il volume e' circa 1,9 volte la media di cassa di 20 sedute (512.915, `in-breve.json` gamma). Il calendario del giro segna il crollo fra le 15:30 e le 16:17, causa non dichiarata.

**Tab 3 · Seduta** (alle 15:05; ultimo prezzo 27.119,50 nel pannello, 27.079,50 nel giro un istante dopo)

- **Notte** (`oggi_notte`, dalle 18:00 di New York = 00:00 italiane): apertura 26.640,25, massimo 27.251,50, minimo 26.366,50, volume 251.385, delta +1.923, POC 27.100, valore 26.917-27.191. Le fasce da 10 sopra il 2% formano due nodi, 27.080-27.180 e 26.930-27.010; fra 27.010 e 27.080 ogni fascia sta sotto il 2%.
- **Dalla chiusura di ieri**: +372,75 punti (+1,39%). CVD (delta cumulato) -1.267 (-0,4%) su 299.631 lotti; CVD al 65% del suo range, prezzo all'85% (pannello).
- **Il valore nel pannello**: dentro London 27.086-27.184, sopra il suo POC 27.100; dentro il valore di cassa di ieri; sotto quello di mercoledi'.
- **Regime: CHOPPY** (pannello): ultime 30 barre 14:36-15:05, efficienza 0,04 (netto 30,25 su strada 815,75), 1 rottura rientrata, copertura si'. Il giro, un istante dopo: 0,01 (9,75 su 856).
- **Velocita'** (proxy, non la speed of tape): barra chiusa delle 15:04, 1.253 lotti, 90° percentile su 60 barre. La barra in formazione passa da 17 lotti (0°) a 1.084 (86°) nello stesso minuto: non si legge. Big trade da 60+ lotti: 0 in 10 barre.
- **Veti**: 2. Libro sottile (nessun big trade, velocita' al 0° percentile della barra appena aperta) e regime choppy.
- **In gioco**: il bordo alto della zona pesante Europa, 27.124,75 (con il VAH notte del pannello, 27.132); il prezzo 5,25 sotto, arrivato da sotto.
- **Struttura oraria** (H1, giro): gli ultimi due swing fanno massimi e minimi crescenti; massimo di swing 27.251,50 (10:00), minimi 26.805 (08:00) e 26.366,50 (00:00). Delta orario: 10h +545, 11h -99, 12h +121, 13h -83, 14h -1.065, 15h -96 (aperta).
- **Volatilita' realizzata** (rendimenti a 5 minuti, annua): 4 ore 28,3%, 24 ore 48,5%, 48 ore 42,0%.

**Tab 4 · Esecuzione**: non registrata nel bridge (`range-NQZ6.json` vuoto). Al suo posto, solo come descrizione, il tape M1 del giro (barre chiuse): delta 15 minuti -1,0%, 30 minuti -1,4%, 60 minuti -2,3%; range dei 60 minuti 27.043-27.191,25.

## Cosa Dicono Per NQ

- **Composito.** Il prezzo e' circa 1.900 punti sotto il VAL dei 90 giorni di cassa: discount, in tutte e quattro le versioni. Nel live la posizione nella curva e' il filtro di contesto prima di tutto (`i-tre-modelli-del-live-q1.md` sezione 10, [5 · 55:05], [5 · 1:01:05]).
- **Settimana.** Venerdi'-mercoledi' hanno valori sovrapposti: per il live si uniscono in un blocco, 28.296-28.891 ([1 · 24:02]). Giovedi' ne esce con shift giu' (VAL e VAH piu' bassi) e staccato. La seduta della rottura e' giovedi': il suo POC, **26.900**, e' il secondo POC che conta ([1 · 24:35]).
- **Il vuoto** sta fra il VAH di giovedi' (27.361) e il VAL di martedi' (28.296). Giovedi' l'ha attraversato nei primi 47 minuti (**osservazione**). Che sia il primo livello da riequilibrare e' il modello del live (`profile-framing.md`, passo 6), non una prova.
- **Seduta.** Il prezzo sta dentro il valore di giovedi', sopra il suo POC, e dentro il valore di Londra, vicino al suo POC. E' il caso "dentro il valore, mercato in bilancia" del passo 5. Il regime choppy dell'indicatore descrive la stessa cosa sulle ultime 30 barre.
- **Chi spinge.** Il prezzo e' all'85% del suo range dalla chiusura, il CVD al 65% ed e' negativo. Nelle ultime due ore il delta e' negativo (14h -1.065) e il prezzo e' rimasto fra 27.043 e 27.191 (**osservazione**: vendite aggressive senza discesa). La notte ha delta +1.923, il CVD dalla chiusura -1.267: le finestre sono diverse (00:00 contro 22:00 italiane). La differenza si scrive, non si sceglie.
- **Scalini.** Misti: mese e settimana con il massimo visitato e non accettato, giorno scalino giu'. Sono una descrizione: come previsione sono smentiti su NQ 2017-2024 (`quant/studi/scala-dei-tre-tempi`).

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 29.735 / 29.840 | POC dei compositi di cassa 90 / 20 | il centro dei tre mesi: misura quanto e' profondo il discount |
| 28.998 | VAL composito di cassa 90 | rientrarci chiude il discount dei tre mesi |
| 28.632 · 28.695 · 28.778 | POC di mer 04, lun 02, ven 30 | la scala dei POC sopra: il cuore del blocco della settimana |
| 28.296 | VAL mar 03, bordo basso del blocco ven-mer | il tetto del vuoto: arrivarci vuol dire vuoto attraversato |
| 27.361-28.296 | vuoto fra VAH gio e VAL mar | zona di transito, non di sosta, per il live |
| 27.361 | VAH gio (sedute; 27.412 nel pannello) | uscire dal valore di giovedi' verso l'alto, nel vuoto |
| 27.251,50 | massimo della notte, swing H1 delle 10:00 | l'estremo che la notte ha toccato e non tenuto |
| 27.184-27.191 | VAH London (pannello) e VAH notte (sedute) | il bordo alto della bilancia di breve |
| 27.100 | POC della notte e di Londra | il centro della bilancia di adesso |
| 26.900 | POC gio, seduta della rottura | il primo punto della via del ritorno: tornarci spegne il recupero della notte |
| 26.829 | VAL gio (sedute; 26.825 nel pannello) | sotto, uscita dal valore di giovedi' verso il basso |
| 26.366,50 | minimo della notte, swing H1 delle 00:00 | l'estremo inferiore della notte |
| 06/11 15:30 · 16:00 | apertura della cassa · fiducia del Michigan | la prima seduta di cassa dopo lo scalino; il dato delle 14:30 e' gia' uscito |

## Cosa Non Sappiamo

- `lacuna del collettore: tab 4, barre range non registrate nel bridge; range-NQZ6.json e orderflow.barre_range vuoti.`
- `lacuna del collettore: il pannello e le sedute danno numeri diversi per gli stessi bordi, senza dichiarare la finestra.` VAH cassa di ieri 27.412 (livelli del pannello) contro 27.361 (sedute e riga VALORE del pannello); massimo di cassa di ieri 28.375,25 contro 28.228,25; VAL di ieri 26.825 contro 26.829. Notte: POC 26.950, VAL 26.830, VAH 27.132 (pannello) contro 27.100, 26.917, 27.191 (sedute).
- **Da verificare nell'indicatore**: alle 15:05, prima dell'apertura, il pannello disegna gia' una "cash in sviluppo" (VAL 27.104, POC 27.159, VAH 27.185). Il suo minimo e massimo (27.043 e 27.191,25) coincidono con il range dei 60 minuti del tape. **Osservazione**: e' compatibile con una cassa che parte alle 14:30 italiane (13:30 UTC, l'orario estivo) invece che alle 15:30. Se e' cosi', viola la regola degli orari dinamici; non l'ho ricostruito.
- `lacuna del collettore: il composito di cassa 90 ha 46 sedute su 90, 6 prima del rollover, e il file non dice se sono aggiustate.`
- La velocita' e il veto del libro sottile si misurano sulla barra in formazione, che cambia nello stesso minuto: vale solo la barra chiusa.
- La GEX viva di NQ e' ferma da 20 ore; il risultato dei dati delle 14:30 non e' nei miei dati (sta nella fase 1).

## Per Il Quadro

1. **27.100**: POC della notte e di Londra, e strike pesante della GEX di NQZ6 (netta -18,4, OI di ieri). Due fonti diverse (`stesso_dato` false): e' una confluenza vera da confrontare.
2. **27.000 e 26.900**: il put wall della GEX (15:02) e il call wall 0DTE della GEX viva (ferma da 20 ore) stanno sul POC della seduta della rottura (26.900) e vicino al VAL di giovedi' (26.829). La GEX e' tutta negativa ("acceleratore"), l'order flow misura choppy: la fase 5 dica se convivono.
3. **COT**: le confluenze 29.584-29.880 sono valore e POC di cassa delle settimane dei report del 20/10 e del 27/10 (stesso dato del bridge), attorno ai POC dei compositi 29.735-29.840. Il valore delle settimane dei report sta tutto circa 2.600 punti sopra il prezzo.
4. **Macro**: discount profondo, scalini misti, +1,39% dalla chiusura di ieri e delta negativo nell'ora dei dati delle 14:30: coerenti con il risultato dei dati sul lavoro?
