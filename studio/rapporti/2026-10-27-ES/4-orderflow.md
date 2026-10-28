# Fase 4 · Order Flow · 27 ottobre 2026 · ES

**Stato**: parziale. Mancano la tab 4 (Range: la finestra ES ne ha tre) e il composito di sola cassa a 90 sedute (ne ha 17).
**Dati**: `in-breve.json`, `pannello.json` e `sedute-ESZ6.json` raccolti alle 07:23. `quattro-tab.txt` e `giro.txt` alle 07:30. `ore-ESZ6.json` e `range-ESZ6.json` (vuoto) alle 07:23. Tutto da `raccolta.json`. Ora italiana: dal 25 ottobre Roma e' a UTC+1 e New York ancora in ora legale, quindi la cassa apre alle 14:30. La GEX di oggi e' fallita nella raccolta (`gex ESZ6`: errore, nessun chart corrisponde). Quella stampata in `giro.txt` e `quattro-tab.txt` e' di ieri alle 15:17 e qui non si usa. Fra le 07:26 e le 07:28 le schede M1 sono state messe per errore su M2 e poi rimesse su M1. Le sedute erano gia' state misurate alle 07:23.

**In una riga**: ES sta nella parte bassa del valore dei tre mesi, dentro il valore di ieri. La notte ha costruito valore piu' in alto e il prezzo ne e' sceso sotto. L'indicatore misura un mercato senza direzione (choppy alle 07:23, balance alle 07:30).

## In Parole Semplici

**Il punto**: ES non ha una direzione. E' nel prezzo "giusto" dei tre mesi, ma nella parte bassa. Dalla notte scende piano, a piccoli passi, e l'indicatore lo misura come un mercato che gira su se stesso.

**Dal piu' ampio al piu' vicino**

1. **I tre mesi**: il prezzo (circa 7.710) e' dentro la zona dove si e' scambiato di piu', circa 60 punti sotto il suo centro (circa 7.770).
   → Non e' ne' caro ne' a sconto rispetto ai tre mesi. Sta pero' piu' vicino al fondo che alla cima.
2. **La settimana**: il valore e' sceso due giorni di fila (22 e 23 ottobre). Ieri e' risalito di un gradino, senza toccare il valore dei giorni prima.
   → Il calo di giovedi' e venerdi' non e' stato ricomprato: fra circa 7.730 e circa 7.745 nessuna seduta recente ha costruito valore.
3. **Ieri**: la seduta ha chiuso esattamente sul suo prezzo piu' scambiato (circa 7.714,5).
   → Una giornata in equilibrio, che chiude al centro.
4. **La notte**: ha costruito valore fra circa 7.719 e 7.730, nella parte alta del valore di ieri. Alle 07:23 il prezzo era sceso sotto, a circa 7.709.
   → Gli ultimi due massimi e i due minimi orari sono piu' bassi dei precedenti: si scende a piccoli passi, senza strappi.
5. **Adesso**: l'indicatore dice "choppy" alle 07:23 e "balance" alle 07:30. In mezz'ora il prezzo ha fatto molta strada e quasi nessuno spostamento netto.
   → Il mercato va da un lato all'altro senza scegliere. Nessun grande ordine registrato.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 7.730 | la cima del valore di ieri e della notte | sopra, il prezzo entra in una zona senza valore recente, fino a circa 7.745 |
| circa 7.704 | il fondo del valore di ieri | sotto, si torna nel valore di giovedi' (centro a 7.690) |
| circa 7.690 | il prezzo piu' scambiato del 22 ottobre, il giorno che ha rotto verso il basso | e' il primo prezzo che il mercato tende a ritrovare tornando indietro (osservazione del live) |

**Quanto e' provato**: i valori, il regime e il delta sono misurati dall'indicatore. Le soglie sono tarate su NQ, non su ES. Che il prezzo torni al centro di un'area o al prezzo piu' scambiato della rottura e' una regola di lettura del live di Fabio: nessuno studio di questo repository la prova su ES.

**Le parole**: **valore** = la fascia di prezzi dove si e' fatto il 70% degli scambi di una seduta, con un fondo e una cima; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **cassa** = la seduta di New York, 14:30-21:00 italiane; **delta** = acquisti aggressivi meno vendite aggressive; **choppy** = va da un lato all'altro senza rompere; **balance** = chiuso dentro un'area, ogni rottura rientra.

## I Fatti

**Tab 1 · Composito** (ES continuo Daily, barre di 24 ore; `quattro-tab.txt`, 07:30)

| composito | valore | POC | zona |
|---|---|---|---|
| 90 giornate Daily, grezzo, dal 23/06 | 7.590-7.865 | 7.772,50 | fair value |
| idem, aggiustato al rollover (+67,75 prima dell'11/09) | 7.645-7.865 | 7.767,50 | fair value |
| sola cassa, 17 sedute dal 02/10 (`composito_di_cassa_90`) | 7.681,75-7.823 | 7.775 | fair value |

I nodi del composito di cassa (fasce da 1 punto, % del volume): 7.854 (0,94), 7.775 (0,90), 7.690 (0,83), 7.754 (0,81), 7.824 (0,71). Il prezzo e' al 26% del range delle ultime 18 sedute (7.648,50-7.897,50; `giro.txt`, 07:30).

**Tab 2 · Settimana** (ESZ6, valore di cassa; `in-breve.json` da `sedute-ESZ6.json`, 07:23)

| seduta | valore | POC | delta | chiude | shift | staccato |
|---|---|---|---|---|---|---|
| mar 20 | 7.743,25-7.774,50 | 7.754,50 | +3.910 | dentro | su | no |
| mer 21 | 7.765,75-7.801,50 | 7.777 | +10.841 | dentro | su | no |
| gio 22 | 7.682,50-7.707,50 | 7.690 | +14.872 | dentro | giu | si |
| ven 23 | 7.653,50-7.681,25 | 7.666 | +690 | dentro | giu | si |
| lun 26 | 7.704,50-7.728,50 | 7.714,50 | +834 | dentro | su | si |

Il 22 ottobre il prezzo e' sceso di circa 93 punti nella barra Daily, con delta Daily +10.068 e range 122 (tab 1). Ieri, fasce da 10 punti: il 49,4% del volume fra 7.710 e 7.720.

**Tab 3 · Seduta** (ESZ6 M1)

| misura | valore | fonte, ora |
|---|---|---|
| notte (23:00-07:23, in corso) | apertura 7.715, massimo 7.731, minimo 7.702,75, ultimo 7.709,75 | `in-breve.json`, 07:23 |
| valore della notte | 7.719,25-7.730, POC 7.724,75; il 66% del volume fra 7.720 e 7.730 | idem |
| delta e volume della notte | +2.942 su 110.964 lotti | idem |
| dalla chiusura di ieri (7.714,50) | -4,75 punti, -0,062% | idem, 07:23 |
| cassa di oggi | non ancora aperta (`oggi_cassa`: null) | idem |
| chiusure orarie (delta) | 03h 7.728,75 (+925), 04h 7.720,50 (+248), 05h 7.725,50 (+302), 06h 7.721,75 (-194), 07h 7.717,50 (+744), 08h 7.713,25 (+509) | `giro.txt`, 07:30 |
| swing orari | massimi 7.739,50 (26/10 17h), 7.731 (27/10 02h); minimi 7.704,75 (26/10 21h), 7.702,75; gli ultimi due: massimi e minimi decrescenti | idem |
| volatilita' realizzata | 4 ore 6,6%, 24 ore 10,6%, 48 ore 11,6% (rendimenti a 5 minuti, annua) | `ore-ESZ6.json`, 07:23 |

**Il regime, come lo misura l'indicatore** (ultime 30 barre M1)

| ora | regime | efficienza (netto su strada) | rotture rientrate | velocita' (proxy dei lotti, non speed of tape) | big trade |
|---|---|---|---|---|---|
| 07:23 (`pannello.json`) | **CHOPPY** | 0,13 (4,75 su 38) | 0 | barra in formazione morta, 150 lotti, 14° percentile su 60; chiusa delle 07:22: 200 lotti, 37° | 0 da 226+ lotti in 10 barre |
| 07:30 (`giro.txt`) | **BALANCE** | 0,17 (6 su 35,50) | 0 | barra in formazione alta, 487 lotti, 95°; chiusa delle 07:29: 339 lotti, 73° | 0 da 226+ lotti in 10 barre |

Veti e livelli: nessun livello depositato, 0 regole e 0 livelli disegnati (su ES non ci sono regole dei livelli).

**Tab 4 · Esecuzione**: non c'e'. La finestra ES non ha la tab Range (`range-ESZ6.json` vuoto, `barre_range` vuoto).

## Cosa Dicono Per ES

- **Il composito** (passo 5 di `profile-framing.md`; sezione 10 di `i-tre-modelli-del-live-q1.md`): il prezzo e' fair value in tutte e tre le versioni, ma basso nella curva. E' circa 60 punti sotto il POC e circa 28 sopra il VAL del composito di cassa. Il POC grezzo e quello aggiustato differiscono di 5 punti, quindi il rollover non cambia la lettura.
- **Il valore della settimana** (passo 2): due shift giu' staccati (22 e 23), poi uno shift su staccato (26). Il valore del 26 si sovrappone a quello del 22 per 3 punti (7.704,50-7.707,50). E' **osservazione** se basti per unirli in un blocco (passo 3). I valori del 22, 23 e 26 coprono senza buchi 7.653,50-7.728,50: un blocco basso, sotto il blocco del 20 e 21 (7.743,25-7.801,50).
- **Il vuoto** (passo 6, cercato fra il VAH di una seduta e il VAL di un'altra): fra il VAH del 26 (7.728,50) e il VAL del 20 (7.743,25) ci sono circa 15 punti. Nessuna delle ultime cinque sedute ci ha costruito valore. Sotto, fra il blocco basso e i minimi, il valore e' continuo: non c'e' vuoto.
- **I due POC che contano** (passo 4, `[1 · 24:35]`): il 22 ottobre e' la seduta che ha rotto sotto il blocco 20-21, con POC 7.690. E' anche un nodo del composito (0,83%). La rottura in su del 26 ha POC 7.714,50, cioe' circa il prezzo attuale.
- **La notte** ha costruito valore nella meta' alta del valore di ieri (VAH della notte 7.730, VAH di ieri 7.728,50). Il prezzo ne e' uscito dal basso. **Osservazione**: il delta e' positivo in quasi tutte le ore, mentre le chiusure orarie scendono da 7.728,75 a 7.713,25. Che sia assorbimento non e' misurato.
- **Il regime** e' quello dell'indicatore: CHOPPY alle 07:23, BALANCE alle 07:30, con efficienza 0,13 e 0,17. Lo stesso mercato e' a cavallo di una soglia che non e' nei dati. La lettura del live per entrambi (sezione 7) e' "dentro un'area, rotture che rientrano". Su ES la scala e' circa un quarto di quella di NQ, e le soglie dell'indicatore non sono tarate su ES.
- **Prima della cassa**: le ore di Londra non sono la sessione del metodo (sezione 8). Il regime della notte non dice quello delle 14:30.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 7.775 / 7.777 | POC del composito di cassa e POC del 21 | il centro dei tre mesi: arrivarci rimette il prezzo al fair value pieno |
| 7.754,50 | POC del 20, nodo del composito | il primo POC sopra il vuoto |
| 7.743,25 | VAL del 20, bordo alto del vuoto | arrivarci vuol dire attraversare il vuoto 7.728,50-7.743,25 |
| 7.730 / 7.728,50 | VAH della notte / VAH di ieri | il bordo alto del valore di ieri: tenerlo sopra cambia la lettura da "dentro" a "fuori sopra" |
| 7.724,75 | POC della notte | dove la notte ha accettato il prezzo |
| 7.714,50 | POC e chiusura di ieri, seduta della rottura in su | il centro di ieri: e' dove sta il prezzo |
| 7.704,50 / 7.702,75 | VAL di ieri / minimo della notte e di swing | il bordo basso: sotto, si torna nel valore del 22 |
| 7.690 | POC del 22 (la seduta della rottura in giu'), nodo del composito | il primo prezzo sulla via del ritorno (`[1 · 24:35]`) |
| 7.681,75 | VAL del composito di cassa | il limite fra fair value e discount |
| 7.666 / 7.653,50 | POC e VAL del 23 | il fondo del blocco basso |
| 27/10 15:00 | fiducia del Conference Board (medio) | 30 minuti dopo l'apertura della cassa |
| 28/10 19:00 | FOMC, conferenza 19:30 (alto) | |
| 29/10 13:30 | PIL anticipato e PCE core (alto) | prima della cassa |

## Cosa Non Sappiamo

- lacuna del collettore: la tab 4 (Range) di ES non e' nel bridge, quindi mancano il ritmo, la durata e il delta delle barre range. E' una lacuna della finestra, non un errore.
- lacuna del collettore: il composito di sola cassa ha 17 sedute (dal 2 ottobre), non 90. L'archivio di ESZ6 non lo riempie (`prima_del_rollover`: 0). Il composito a 90 giornate c'e' solo sulle barre Daily di 24 ore, che il live non usa (passo 1).
- lacuna del collettore: la soglia fra CHOPPY e BALANCE non e' nel pannello. Il cambio fra 07:23 e 07:30 non si puo' pesare.
- lacuna del collettore: la soglia dei big trade (226+ lotti nei dati, circa 230 detto dall'orchestratore) non e' tarata su ES. Quella di Fabio (60) e' di NQ.
- lacuna del collettore: `confluenze.json` e' vuoto perche' la GEX di oggi e' fallita (errore "no chart matches").
- lacuna del collettore: `giro.txt` scrive un minimo di swing "26/10 25h" (7.702,75), un'ora che non esiste. Probabilmente e' il 27/10 alle 01h, ma non e' verificato. Il POC del 26 vale 7.714,50 in `sedute-ESZ6.json` e 7.714,35 in `giro.txt` (fasce da 0,21, non da un tick di 0,25). Per le sedute chiuse vale il primo.
- La finestra del regime delle 07:30 (07:01-07:30) comprende i minuti 07:26-07:28, quando le schede sono state su M2. Il tape del giro mostra barre M1, ma un ricalcolo pulito non e' verificato. La lettura di riferimento e' quella delle 07:23.
- Le fasce del profilo da 10 punti su ES valgono circa 40 punti di NQ: sono grossolane.

## Per Il Quadro

1. **Il vuoto 7.728,50-7.743,25 e il bordo 7.730**: confrontali con il muro delle call e lo zero gamma della fase 3 (di oggi, non quelli di ieri a 7.725 e 7.720).
2. **7.690, il POC della rottura e nodo del composito**: e' vicino a un muro delle put della fase 3? Va confrontato anche con il prezzo di ES alla data del cambio COT (fase 2).
3. **Il regime "senza direzione" alle 07:23 e alle 07:30, con volatilita' realizzata del 6,6% (4 ore) e 10,6% (24 ore)**: coerente con la volatilita' implicita di oggi e con l'attesa del FOMC del 28/10?
4. **Fair value basso nella curva** (60 punti sotto il POC dei tre mesi): coerente con il posizionamento COT degli speculatori su ES?
