# Fase 4 · Order Flow Delle Quattro Tab · ESZ6 · 28/10/2026

**Stato**: parziale (manca la tab Esecuzione: niente barre range; mancano giro, struttura oraria, confluenze, in-breve)
**Dati**: `dati/tab-ruoli.json`, `sedute-ESZ6.json`, `ore-ESZ6.json`, `pannello.json` (pannello alle 10:17 italiane, 09:17Z). `range-ESZ6.json` vuoto; `in-breve.json`, `quattro-tab.txt`, `giro.txt`, `confluenze.json`, `raccolta.json` assenti al momento della lettura; `log.txt` vuoto. Ora della raccolta non dichiarata (manca `raccolta.json`): uso l'ora del pannello.

**In una riga**: lo S&P 500 future sta nel mezzo del suo valore di tre mesi, dentro il valore di ieri, e la notte e' stata piccola e tranquilla; l'indicatore misura un mercato in equilibrio.

## In Parole Semplici

**Il punto**: il prezzo, circa 7.768, sta esattamente dove il mercato ha scambiato di piu' negli ultimi tre mesi. Ieri il valore e' salito per il secondo giorno di fila; stanotte il prezzo e' rimasto dentro il valore di ieri, e l'indicatore misura un mercato che oscilla senza direzione.

**Dal piu' ampio al piu' vicino**

1. **I tre mesi**: il prezzo e' sul prezzo piu' scambiato di 90 giornate (circa 7.767).
   → Ne' caro ne' a sconto: e' il "prezzo giusto" del periodo.
2. **Le settimane**: il valore e' sceso il 22 e 23 ottobre, poi e' risalito il 26 e il 27, ogni volta staccandosi dal giorno prima.
   → Il mercato e' tornato nella zona alta 7.735-7.800, dove era a meta' ottobre.
3. **Ieri**: chiusura a 7.758, sul prezzo piu' scambiato del giorno, con i compratori aggressivi in netta prevalenza.
   → La salita di ieri e' stata spinta da chi compra, non solo lasciata salire.
4. **La notte**: un movimento di soli 18 punti (7.752-7.770), tutto dentro il valore di ieri, con volumi bassi.
   → Nessuno ha cambiato idea stanotte; il test vero arriva con l'apertura di New York.
5. **Adesso**: l'indicatore dice "equilibrio" (il prezzo va avanti e indietro), velocita' normale, nessun ordine grande.
   → Il mercato si sta muovendo poco e senza fretta.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.772 | il bordo alto del valore di ieri, toccato anche stanotte | sopra, il prezzo esce dal valore di ieri verso l'alto |
| circa 7.758 | il prezzo piu' scambiato ieri e stanotte | e' il centro dell'equilibrio di adesso |
| circa 7.729-7.741 | un tratto dove ieri si e' scambiato poco, fra il valore di lunedi' e quello di ieri | sotto il valore di ieri, una zona che il prezzo di solito attraversa in fretta |

**Quando**: 09:00 apertura di Londra (gia' in corso) · 14:30 apertura della cassa di New York (09:30 locali) · 21:00 chiusura.
**Quanto e' provato**: i numeri sono misurati da ATAS; leggere "equilibrio" e "zona di transito" e' la procedura del corso di Fabio, non una regola validata su ES.

**Le parole**: **valore** = la fascia di prezzi dove si e' fatto il 70% degli scambi della seduta; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **staccato** = il valore di oggi non tocca quello di ieri; **compratori aggressivi** (delta) = chi compra pagando subito, meno chi vende subito.

## I Fatti

Orari in ora italiana (UTC+1 dal 25 ottobre; New York UTC-4 fino al 1° novembre: la cassa e' 13:30Z-20:00Z, 14:30-21:00 italiane). Ultimo prezzo 7.768,00 alle 10:17 (`pannello.json`).

**Tab 1, Composito** (`tab-ruoli.json`, barre Daily di 24 ore, 90 barre dal 24/06, grana 5 punti)

| | VAL | POC | VAH | zona |
|---|---|---|---|---|
| grezzo | 7.600 | 7.767,5 | 7.865 | fair value |
| aggiustato (scarto rollover 67,75 fino all'11/09) | 7.650 | 7.762,5 | 7.860 | fair value |
| composito di sola cassa (`sedute-ESZ6.json`, **18 sedute** dal 02/10) | 7.682 | 7.755 | 7.816,5 | fair value |

Nodi del composito di cassa (fasce da 1 punto, % del volume): 7.754 (0,95), 7.768 (0,91), 7.854 (0,89), 7.778 (0,80), 7.690 (0,79).

**Tab 2, Settimana** (`sedute-ESZ6.json`, sola cassa, M1, valore al 70%; coincide con la tab M30 tranne il VAH del 14/10: 7.795,0 contro 7.795,75)

| giorno | VAL | POC | VAH | volume | delta | shift | staccato |
|---|---|---|---|---|---|---|---|
| 14/10 | 7.745 | 7.768,75 | 7.795 | 1.571.480 | +19.632 | giu' | si |
| 15/10 | 7.758 | 7.791,75 | 7.801 | 1.413.195 | +5.721 | su | no |
| 16/10 | 7.676,25 | 7.700 | 7.758,5 | 1.953.614 | -15.860 | giu' | no |
| 19/10 | 7.734,75 | 7.743 | 7.752 | 1.325.460 | +16.830 | nessuno | no |
| 20/10 | 7.743,25 | 7.754,5 | 7.774,5 | 1.272.280 | +3.910 | su | no |
| 21/10 | 7.765,75 | 7.777 | 7.801,5 | 1.288.179 | +10.841 | su | no |
| 22/10 | 7.682,5 | 7.690 | 7.707,5 | 1.708.340 | +14.872 | giu' | si |
| 23/10 | 7.653,5 | 7.666 | 7.681,25 | 1.332.718 | +690 | giu' | si |
| 26/10 | 7.704,5 | 7.714,5 | 7.728,5 | 1.273.698 | +834 | su | si |
| 27/10 | 7.740,75 | 7.758 | 7.772,25 | 1.128.317 | +18.413 | su | si |

Tutte le 18 sedute chiudono **dentro** il proprio valore. Il 27/10 ha il volume piu' basso dal 13/10 e il secondo delta piu' alto delle 18.

**Tab 3, Seduta**

- 27/10, profilo da 10 punti: 7.750-7.760 36,4%, 7.760-7.770 25,9%, **7.740-7.750 6,5%**, 7.730-7.740 14,2%, 7.720-7.730 9,3%.
- Ore del 27/10 (`ore-ESZ6.json`): 16:00-17:00 da 7.748,75 a 7.768 con delta +10.336; 20:00-21:00 delta +8.281.
- Notte (dalle 23:00 italiane, `oggi_notte`): 7.752,25-7.770,25, POC 7.759, VAL 7.758, VAH 7.765, volume 125.002, delta +196.
- London in formazione (tab Seduta): VAL 7.763,25, POC 7.766,75, VAH 7.767,25. Finestra diversa dalla notte: i due VAL non sono lo stesso numero.
- Dalla chiusura di ieri (7.758,25): +9,75 punti, +0,126%, alle 10:17.
- Volatilita' realizzata (rendimenti log a 5 minuti, annua, 276 x 252): 6,6% su 4 ore, 9,6% su 24, 10,9% su 48.
- **Pannello, 10:17**: REGIME **BALANCE** sulle ultime 30 barre 09:48-10:17, efficienza 0,16 (netto 4,50 su strada 28,25), 2 rotture rientrate, copertura si; modello indicato: mean reverting dai bordi verso il POC. VELOCITA' **normale**, 277 lotti, 39° percentile su 60 barre (**proxy**, non la speed of tape); barra chiusa 10:16: 284 lotti, 44° percentile. 0 big trade da 222+ lotti in 10 barre. Nessun livello depositato.

**Tab 4, Esecuzione**: `esecuzione: null` in `tab-ruoli.json`, `range-ESZ6.json` vuoto. Nessuna misura.

## Cosa Dicono Per NQ

- **Composito** (`i-tre-modelli-del-live-q1.md` sez. 10, premium e discount rispetto al picco `[5 · 55:05]`): il prezzo e' sul POC grezzo e a 13 punti dal POC di cassa: fair value, nessun veto di posizione nella curva. **Ma** la tab e' su barre di 24 ore e il composito di cassa ha 18 sedute, non 90 (`profile-framing.md` passo 1).
- **Settimana** (passo 2, shift alla lettera): due shift giu' staccati (22-23/10), poi due shift su staccati (26-27/10). Il valore e' rientrato nel blocco alto. **Lettura dell'agente** (passo 3, i profili sovrapposti si uniscono): 14, 15, 19, 20, 21 e 27/10 hanno valori sovrapposti, blocco circa 7.735-7.801; 22, 23 e 26/10 fanno il blocco basso circa 7.653-7.728; il 16/10 (7.676-7.758) li tocca entrambi.
- **POC della rottura** (passo 4, `[1 · 24:35]`): la risalita parte il 26/10 (POC 7.714,5); la seduta che ha riportato il valore nel blocco alto e' il 27/10, POC 7.758, che coincide con il POC della notte (7.759).
- **Il vuoto** (passo 6): fra il VAH del 26/10 (7.728,5) e il VAL del 27/10 (7.740,75); il profilo del 27/10 conferma poco volume a 7.740-7.750 (6,5%). Per la regola dello scaffale (`profile-framing.md`), la seduta che l'ha visitato e' ieri: e' sottile anche li', quindi resta un vuoto, non uno scaffale.
- **Seduta**: ieri delta +18.413 su volume basso, con la spinta concentrata nell'ora dopo l'apertura e in quella di chiusura: compratori aggressivi che hanno ottenuto lo spostamento (**osservazione**). La notte e' dentro il valore di ieri, delta quasi nullo, volatilita' in calo da 10,9% a 6,6%: compressione.
- **Il regime lo dichiara l'indicatore**: BALANCE alle 10:17, su 30 minuti di notte/Londra; dentro il valore, in bilancia: il modello del live e' il mean reverting (passo 5). Vale per quella finestra, non per la cassa che non e' aperta.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.801-7.801,5 | VAH del 15 e del 21/10, bordo alto del blocco | invalidazione della lettura "equilibrio nel blocco alto": sopra, valore oltre il blocco |
| 7.777 / 7.791,75 | POC del 21/10 e del 15/10 | i primi POC sopra il prezzo nella scala |
| 7.772,25-7.772,75 | VAH e massimo di cassa del 27/10; massimo notte 7.770,25 | uscita dal valore di ieri verso l'alto (shift su se il VAL di oggi resta sopra 7.740,75) |
| 7.767,5 / 7.762,5 | POC del composito, grezzo / aggiustato | posizionamento: il centro dei tre mesi |
| 7.758-7.759 | POC del 27/10 e della notte | il centro dell'equilibrio di adesso; POC della seduta di rottura |
| 7.755 / 7.754,5 | POC del composito di cassa; POC del 20/10 (data del COT) | aggancio con il COT |
| 7.740,75 | VAL del 27/10 | bordo basso del valore di ieri: sotto, si entra nel vuoto |
| 7.728,5-7.740,75 | vuoto fra VAH 26/10 e VAL 27/10 | zona di transito, non di sosta |
| 7.714,5 | POC del 26/10, prima seduta della risalita | primo POC sotto il vuoto |
| 7.682 | VAL del composito di cassa | bordo del discount a 18 sedute |
| 28/10 14:30 / 21:00 | apertura e chiusura cassa (09:30-16:00 New York) | la prima misura della seduta vera |
| 30/10 20:30 | prossimo COT (da `2-cot.md`) | dati al 27/10 |

## Cosa Non Sappiamo

- **lacuna del collettore**: tab Esecuzione assente (`esecuzione: null`) e `range-ESZ6.json` vuoto: niente barre range, niente ritmo, verso o durata.
- **lacuna del collettore**: `in-breve.json`, `giro.txt`, `quattro-tab.txt`, `confluenze.json` e `raccolta.json` non presenti: niente struttura oraria sugli swing, niente tape del giro, niente ora di raccolta ne' errori dichiarati.
- **lacuna del collettore**: `composito_di_cassa_90` ha 18 sedute (dal 02/10, `prima_del_rollover: 0`): l'archivio di ES non lo riempie fino a 90.
- **lacuna del collettore**: il CVD (delta cumulato) non c'e' come serie; ci sono solo i delta orari e di seduta.
- Il pannello non stampa una riga di veti: non si distingue "nessun veto" da "veti non misurati".
- La soglia dei big trade (222 lotti) e' quella del pannello per ES; il corso tara 60 su NQ: non confrontabili.
- Il regime e' misurato su 30 barre di notte/Londra, prima della cassa: non dice niente della seduta.
- Lettura dei blocchi uniti e del vuoto: e' la procedura del live, nessuno studio la valida su ES.

## Per Il Quadro

1. **7.754,5-7.759**: POC del 20/10 (data del COT), del composito di cassa, del 27/10 e della notte stanno entro 5 punti; il COT lo cita gia' (`2-cot.md`). Sono letture diverse dello stesso valore, non conferme indipendenti: va confrontato con lo zero gamma e i muri di ES della fase 3.
2. **7.772 e 7.801**: VAH di ieri e bordo alto del blocco: ci sono muri della gamma vicino?
3. **7.728,5-7.740,75**: il vuoto sotto il valore di ieri: coincide con un muro o un salto di gamma?
4. **Compressione**: volatilita' realizzata 6,6% a 4 ore e regime BALANCE: coerente con un regime di gamma positiva, se la fase 3 lo misura? Il POC del 13/10 (7.851,25, citato dal COT) e' 83 punti sopra, fuori dal blocco.
