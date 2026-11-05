# Fase 4 · L'Order Flow Delle Quattro Tab · ESZ6 · 5 novembre 2026

**Stato**: parziale (manca la tab 4: le barre range non sono registrate nel bridge)
**Dati**: `dati/in-breve.json` (sezione `orderflow`), `pannello.json`, `quattro-tab.txt`, `giro.txt`, `range-ESZ6.json` (vuoto); raccolta delle 16:16 ora italiana (`raccolta.json`, nessun errore). La cassa di New York e' aperta dalle 15:30: i numeri di oggi coprono 46 minuti e sono **in corso**.

**In una riga**: la cassa di oggi ha lasciato in mezz'ora il valore di ieri, ha attraversato senza attrito la fascia vuota sotto, ed e' scesa sotto ogni prezzo di cassa dal primo ottobre; negli ultimi 30 minuti l'indicatore misura una bilancia.

## In Parole Semplici

**Il punto**: il prezzo e' sceso di circa 165 punti da ieri sera (-2,1%) ed e' nella parte "a sconto" degli ultimi due mesi. Dopo la discesa veloce dell'apertura, da mezz'ora gira in un recinto: l'indicatore lo chiama bilancia, non spinta.

**Dal piu' ampio al piu' vicino**

1. **I mesi**: ottobre ha toccato un massimo e un minimo oltre quelli di settembre, ma ha chiuso dentro.
   → Gli estremi sono stati visitati e non accettati; il giornaliero rimbalza da 3 giornate fra circa 7.490 e 7.746.
2. **Le ultime settimane**: il prezzo e' sotto la zona dove si e' scambiato di piu' da ottobre.
   → E' "a sconto" rispetto a quel valore, oltre cento punti sotto il suo bordo basso.
3. **I giorni**: mercoledi' aveva costruito il valore in alto e staccato; oggi lo ha lasciato e il valore e' sceso.
   → La fascia fra 7.674 e 7.715 e' stata attraversata in fretta: li' nessuno si e' fermato a scambiare.
4. **La notte**: calma, fra 7.691 e 7.739, con i venditori un po' piu' aggressivi.
   → La cassa e' partita dal fondo del valore notturno, sotto il valore di ieri.
5. **Adesso** (16:16): il prezzo e' al fondo del valore di oggi, che coincide con il minimo di oggi.
   → Bilancia negli ultimi 30 minuti, ritmo basso nella barra che si sta formando.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.593 | il prezzo piu' scambiato di oggi | e' il centro della bilancia di adesso |
| circa 7.674-7.715 | la fascia vuota fra il valore di lunedi' e quello di mercoledi' | una zona di transito, non di sosta |
| circa 7.729 | il prezzo piu' scambiato di mercoledi', la seduta del salto in su | il primo livello sopra il vuoto |
| circa 7.490 | il muro basso del rimbalzo giornaliero | se una giornata chiude sotto, il rimbalzo e' rotto |

**Quanto e' provato**: i livelli e il regime sono misure dell'indicatore; che il vuoto si attraversi in fretta e' una regola del live di Fabio, non provata su ES; gli "scalini" sono stati provati su NQ 2017-2024 e non prevedono il verso.

**Le parole**: **valore** = la fascia dove si e' scambiato il 70% del volume; **bordo basso/alto** (VAL/VAH) = i suoi due estremi; **POC** = il prezzo piu' scambiato; **delta** = acquisti aggressivi meno vendite aggressive; **bilancia** = il prezzo va su e giu' e ogni rottura rientra; **vuoto** = fascia con poco volume.

## I Fatti

Prezzo: 7.554 nel pannello (16:16, `pannello.json`, 15:16Z); 7.556 nelle sedute (`in-breve`, 15:16Z). Dalla chiusura di ieri (7.721,75): -165,75 punti, -2,147%.

**Tab 1, composito** (`in-breve.orderflow`)

| composito | sedute | VAL | POC | VAH | zona |
|---|---|---|---|---|---|
| cassa "90" (regola del live) | 24, dal 02/10 | 7.677,75 | 7.754 | 7.836,25 | discount |
| cassa 20 | 20, dal 08/10 | 7.667,75 | 7.754 | 7.818,75 | discount |
| Daily 24 ore, grezzo | 90 barre dal 02/07 | 7.630 | 7.767,50 | 7.855 | discount |
| Daily 24 ore, aggiustato (+67,75 prima dell'11/09) | 90 barre | 7.650 | 7.762,50 | 7.845 | discount |

Scalini (ultima candela chiusa, `in-breve.orderflow.scalini`): mese **pausa** (ottobre: massimo 7.897,50 e minimo 7.558,25 oltre settembre, chiusura 7.614,50 dentro; visitati, non accettati), settimana **pausa** (26-30/10: massimo 7.819,50 visitato, non accettato), giorno **pausa** (04/11: minimo 7.490 visitato, non accettato, candela che ingloba). Insieme: **misti**. Ping-pong dal 02/11, 3 giornate dopo lo scalino giu' del 30/10: muro alto 7.746,25, muro basso 7.490.

**Tab 2, settimana** (valore di cassa, M1, `in-breve` e `quattro-tab.txt`)

| giorno | VAL-VAH | POC | chiude | valore | shift | delta |
|---|---|---|---|---|---|---|
| ven 30/10 | 7.597,75-7.669,50 | 7.610 | dentro | giu', staccato | giu' | +27.672 |
| lun 02/11 | 7.596-7.674 | 7.657 | sotto | su | nessuno | +3.502 |
| mar 03/11 | 7.589,75-7.609,25 | 7.597 | dentro | giu' | giu' | -3.521 |
| mer 04/11 | 7.715,50-7.741,75 | 7.729,25 | dentro | su, staccato | su | +2.387 |
| gio 05/11 (in corso) | 7.547,75-7.631 | 7.593,25 | ultimo dentro | giu', staccato | giu' | -23.684 |

Il VAH di oggi e' 7.631 in `in-breve`, 7.630 in `quattro-tab.txt`, 7.631,75 nelle quattro domande: tre istanti della stessa barra in formazione.

**Tab 3, seduta**

| tratto | ora italiana | apertura | max | min | ultimo | POC | VAL-VAH | delta / volume |
|---|---|---|---|---|---|---|---|---|
| notte | 00:00-15:30 | 7.737,25 | 7.738,75 | 7.691 | 7.707,50 | 7.711,50 | 7.706,25-7.728,50 | -8.602 / 321.950 |
| cassa | 15:30-16:16 | 7.707,50 | 7.712,50 | 7.547,75 | 7.556 | 7.593,25 | 7.547,75-7.631 | -23.684 / 660.512 |

- Ore (`giro.txt`, ora italiana): 11h-14h chiusure fra 7.724,50 e 7.736, delta fra -1.023 e +540; 15h 7.593,75 delta -17.408; 16h 7.566 delta -8.321. Mezz'ora delle 15:30: range 132 punti, volume 432.744, delta -14.716.
- Profilo della cassa a fasce da 10 punti: 7.650-7.710 fra il 2,1% e il 4,7% per fascia; 7.570-7.620 fra il 9,3% e l'11,5%.
- CVD (delta cumulato, M1 chiuse, `giro.txt`): 15 minuti -9.724 (-4,8%), 30 minuti -14.600 (-3,4%), 60 minuti -24.057 (-3,6%).
- Struttura oraria H1: ultimi due swing con massimi e minimi decrescenti; minimi di swing sotto il prezzo a 7.555,25 (03/11 13h), 7.546 (03/11 08h), 7.490 (04/11 04h).

**Il regime e la velocita'** (`pannello.json`, marketTime 15:16Z = 16:16 italiane, barra in formazione)

| misura | valore |
|---|---|
| regime | **BALANCE**, ultime 30 barre 15:47-16:16: efficienza 0,22 (netto 65,25 su strada 291,75), 2 rotture rientrate, copertura no |
| velocita' barra in formazione | **morta**: 2.260 lotti, 19° percentile su 60 barre (proxy, non la speed of tape) |
| velocita' barra chiusa 16:15 | 14.126 lotti, 61° percentile su 60 barre |
| big trade | 0 da 255+ lotti in 10 barre |
| livelli e veti | nessun livello depositato; nessun veto stampato |

La copia del pannello in `giro.txt`, presa pochi secondi dopo, dice: efficienza 0,18 (54 su 303,75), copertura si', velocita' **normale** (13.434 lotti, 49° percentile), 1 big trade netto -412. Il regime resta BALANCE in entrambe; vale `pannello.json`. La riga operativa del pannello non si riporta.

**Tab 4, esecuzione**: nessun dato (`range-ESZ6.json` vuoto, "TAB 4 (Range) non registrata nel bridge"). Il solo surrogato e' il tape M1 (`giro.txt`): 16:12-16:15 fra 7.547,75 e 7.559, delta fra -801 e +402; 16:16 in formazione a 7.566,25, delta +862.

## Cosa Dicono Per ES

1. **Composito**: il prezzo e' sotto il VAL di tutti e quattro i compositi: discount. Nel live la posizione nella curva veta il gesto contro di essa ("low in the curve", `i-tre-modelli-del-live-q1.md` §10 `[5 · 1:01:05]`); qui si registra la sola posizione.
2. **Accettato e visitato**: gli estremi di mese, settimana e giorno sono tutti visitati e non accettati. Il giornaliero e' in ping-pong da 3 giornate fra 7.490 e 7.746,25; oggi e' dentro. **Osservazione**, smentita come previsione su NQ (`quant/studi/scala-dei-tre-tempi`).
3. **Settimana, i blocchi** (`profile-framing.md` passo 3): 30/10, 02/11 e 03/11 si sovrappongono e fanno un blocco 7.589,75-7.674 con tre POC (7.597, 7.610, 7.657). Mercoledi' e' un'area staccata sopra (7.715,50-7.741,75), la seduta che ha creato la rottura in su: il suo POC 7.729,25 resta un livello (passo 4, `[1 · 24:35]`). Oggi il valore e' tornato nel blocco ed e' sceso sotto il suo VAL: shift giu' alla lettera (VAL e VAH piu' bassi).
4. **Il vuoto** (passo 6): fra il VAH di lunedi' (7.674) e il VAL di mercoledi' (7.715,50). Non e' una fascia sottile del composito: la seduta piu' recente che l'ha visitata, oggi, ci ha lasciato fra il 2,1% e il 4,7% per fascia da 10 punti, contro il 9-11% di 7.570-7.620. Attraversato in apertura, come il live descrive (zona di transito).
5. **Seduta**: la notte ha costruito il valore sopra, sovrapposto a mercoledi'. La cassa e' partita al VAL della notte (7.706,25), sotto il VAL di ieri, ed e' scesa: e' il caso del "VAL come strada" del live (§10 `[1 · 1:19:30]`), descritto, non generalizzato. Il delta e' negativo in ogni finestra.
6. **Il regime, come lo misura l'indicatore**: BALANCE sugli ultimi 30 minuti, dopo una mezz'ora da 132 punti. Il live descrive lo stesso passaggio: dopo un'esplosione direzionale ci si aspetta il chop, non la continuazione (§7 `[4 · 1:05]`). **Osservazione**: la finestra di 30 barre contiene la parte finale della discesa.
7. **Dove si sta nella curva** (passo 5): ultimo prezzo dentro il valore di oggi, sul suo bordo basso, che coincide con il minimo della cassa. Sotto non c'e' valore di cassa in nessuna seduta letta ("nessuna seduta piu' indietro lo contiene", `giro.txt`).

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.547,75 | VAL e minimo della cassa di oggi (in corso) | bordo della bilancia attuale; sotto, nessun valore di cassa nelle sedute lette |
| 7.546 / 7.555,25 | minimi di swing H1 del 03/11 (24 ore) | gli ultimi minimi della struttura oraria |
| 7.490 | muro basso del ping-pong (minimo del 04/11, notte) | chiusura di giornata sotto = ping-pong rotto (invalidazione della struttura) |
| 7.593,25 | POC di oggi, in corso | centro della bilancia misurata dal pannello |
| 7.597 / 7.610 | POC del 03/11 e del 30/10, dentro il blocco | scala dei POC sopra il prezzo |
| 7.631 | VAH di oggi, in corso | bordo alto della bilancia di oggi |
| 7.657 / 7.674 | POC e VAH del 02/11, tetto del blocco | bordo alto del blocco 30/10-03/11; sopra comincia il vuoto |
| 7.674-7.715,50 | vuoto fra VAH 02/11 e VAL 04/11 | zona di transito, non di sosta |
| 7.677,75 | VAL del composito di cassa | rientro dal discount al fair value |
| 7.711,50 | POC della notte (non cassa) | riferimento della notte, fuori dal framing |
| 7.729,25 | POC del 04/11, seduta della rottura in su | il primo livello che il mercato ritrova sopra il vuoto |
| 7.746,25 | muro alto del ping-pong | chiusura oltre = ping-pong rotto |
| 7.754 | POC dei compositi di cassa 20 e "90" | il centro del valore da ottobre |
| 22:00 di oggi | chiusura della cassa (16:00 New York) | i numeri di oggi diventano una seduta chiusa |

## Cosa Non Sappiamo

- **lacuna del collettore: le barre range** (`range-ESZ6.json` vuoto, tab 4 non registrata nel bridge): senza durata e verso delle barre range non si dice se il ritmo accelera o si spegne, se non dal proxy di velocita' del pannello.
- **lacuna del collettore: il composito "90" ha 24 sedute** (dal 02/10, nessuna prima del rollover): non e' ancora il composito di tre mesi del live.
- **lacuna del collettore: Londra non e' separata** (08:00-15:30 italiane): si legge solo dalle barre orarie.
- **lacuna del collettore: il pannello non stampa i veti**; e il file della giornata `ESZ6-2026-11-05.md` non esiste, quindi nessun livello disegnato.
- Due copie del pannello nello stesso minuto danno velocita' opposte (morta contro normale): la barra in formazione cambia da sola.
- Il ping-pong e gli scalini usano barre Daily di 24 ore, non la sola cassa; il minimo 7.490 e' notturno.

## Per Il Quadro

1. **Gamma**: il pannello misura BALANCE, X-Ray (fase 3) dice gamma da acceleratore al prezzo. Bilancia sugli ultimi 30 minuti contro un regime opzioni che allarga: da confrontare, con gli strike X-Ray in prezzi di ES dati dalla fase 3, contro 7.547,75, 7.593,25, 7.631 e il vuoto 7.674-7.715,50.
2. **COT**: 7.649,75 (VAL della settimana del report 27/10) cade 3,75 punti dal VAL di cassa del 23/10 (7.653,50), dentro il blocco 30/10-03/11. 7.690 (POC della settimana del report 27/10 e POC di cassa del 22/10) cade nel vuoto. 7.740-7.743 e' una confluenza `stesso_dato`: lo stesso numero del bridge, non due conferme.
3. **Composito**: il POC 7.754 dei compositi coincide con la confluenza 7.750-7.754 (VAH della settimana del report 27/10): il centro del valore di ottobre per entrambe le fonti.
4. **Macro**: -2,1% dalla chiusura di ieri con il delta negativo in ogni finestra e i sussidi alle 14:30 italiane (mezz'ora delle 14:30: range 12 punti). La discesa e' partita con l'apertura della cassa, non con il dato: da confrontare con le notizie della fase 1.
