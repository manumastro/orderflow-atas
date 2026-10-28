# Fase 4 · Order Flow · 2026-10-28

**Stato**: parziale (manca la tab 4, le barre range: non registrata nel bridge, `range-NQZ6.json` vuoto)
**Dati**: `in-breve.json` (orderflow), `pannello.json`, `quattro-tab.txt`, `giro.txt`, `raccolta.json`; raccolta 2026-10-28 10:17-10:18 ora italiana, tutte le fonti "ok". Pannello alle 10:17 (09:17Z); il giro lo ristampa alle 10:18. La cassa di oggi non e' aperta (apre alle 14:30 ora italiana): `oggi_cassa` e' vuota per orario, non per errore.

**In una riga**: il valore di cassa e' salito due sedute di fila, il prezzo e' tornato dentro il valore di mercoledi' 21 e sta nella meta' alta del composito, e l'indicatore misura un mercato choppy con il ritmo spento.

## In Parole Semplici

**Il punto**: dopo il crollo di giovedi' 22, il mercato ha ricostruito il valore verso l'alto per due giorni. Stamattina sta fermo dentro la zona di scambio di mercoledi' 21, e l'indicatore dice che si muove a zig-zag, senza direzione.

**Dal piu' ampio al piu' vicino**

1. **Gli ultimi mesi**: il prezzo sta circa 200 punti sopra il prezzo piu' scambiato del periodo, ma dentro la zona dove si e' scambiato il 70% del volume.
   → Non e' ne' caro ne' a sconto: e' prezzo "giusto", nella parte alta.
2. **Le settimane**: giovedi' 22 il valore e' crollato di quasi 800 punti; lunedi' e martedi' e' risalito, ogni volta tutto sopra il giorno prima.
   → Il terreno perso e' stato quasi ripreso: il prezzo e' tornato nella zona di mercoledi' 21.
3. **I giorni**: le ultime cinque sedute hanno chiuso tutte dentro la propria zona di valore; da venerdi' gli aggressori netti in cassa sono stati i venditori, eppure il valore e' salito.
   → E' un'osservazione: il prezzo e' salito senza che i compratori aggressivi dominassero.
4. **La notte**: circa 125 punti sopra la chiusura di ieri, con un saldo di compratori aggressivi piccolo (meno del 2% del volume).
   → La salita della notte e' avvenuta con poca spinta netta.
5. **Adesso**: l'indicatore misura il regime "choppy": in 30 minuti il prezzo ha fatto 214 punti di strada per spostarsi di 16. Le barre sono lente.
   → Il mercato va da un lato all'altro senza scegliere; e' la lettura dell'indicatore, non una stima nostra.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 30.030 | il prezzo piu' scambiato di mercoledi' 21, la seduta che fa da cornice | il prezzo ritrova il centro del valore di prima del crollo |
| circa 29.880 | il bordo alto della zona di valore di ieri | il prezzo esce dalla zona di mercoledi' e rientra in quella di ieri |
| circa 29.580-29.640 | un tratto dove ieri si e' scambiato pochissimo | si attraversa in fretta: e' un vuoto, non un appoggio |

**Quando**: 14:30 apre la cassa di New York · 19:00 decisione della Fed, 19:30 conferenza · dopo le 21:00 trimestrali di Microsoft e Google
**Quanto e' provato**: i numeri sono misurati dall'indicatore e dal collettore; la lettura "dentro il valore = mercato in bilancia" viene dal corso di Fabio, ed e' un modello, non provato su NQ. Le barre range (il ritmo fine) mancano.

**Le parole**: **zona di valore** = la fascia di prezzi dove si e' scambiato il 70% del volume di una seduta; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **choppy** = movimento a zig-zag, rotture che rientrano; **delta** = contratti comprati aggressivamente meno venduti aggressivamente; **cassa** = la seduta di New York, 14:30-21:00 ora italiana.

## I Fatti

**Tab 1, composito** (POC = point of control, il prezzo piu' scambiato; VAL e VAH = bordo basso e alto dell'area di valore, il 70% del volume):

| profilo | sedute | VAL | VAH | POC | zona | fonte |
|---|---|---|---|---|---|---|
| cassa, regola del live (90 sedute) | **39**, dal 2026-09-03 (6 prima del rollover) | 28.998 | 30.779 | 29.735 | fair value | `composito_di_cassa_90` |
| cassa, 20 sedute | 20, dal 2026-09-30 | 28.996 | 30.850 | 29.780 | fair value | `composito_di_cassa` |
| tab 1, Daily 24 ore, grezzo | 90 barre dal 2026-06-24 | 28.580 | 30.295 | 29.517,5 | fair value | `composito_della_tab_1` |
| tab 1, aggiustato (+299,25 prima del 2026-09-11) | 90 barre | 29.070 | 30.575 | 29.817,5 | fair value | idem |

Prezzo 29.939,75 alle 10:17 (pannello). Rendimento dalla chiusura di ieri (29.813,25): +126,5 punti, +0,424% alle 10:17.

**Tab 2, settimana** (valore di cassa 09:30-16:00 New York = 14:30-21:00 ora italiana; shift alla lettera del live: VAL **e** VAH dalla stessa parte; fonte `ultime_5_sedute`):

| seduta | VAL | VAH | POC | chiude | valore | staccato | shift | delta | volume |
|---|---|---|---|---|---|---|---|---|---|
| mer 21 | 29.883 | 30.087 | 30.030 | dentro | SU | si | su | -1.994 | 454.472 |
| gio 22 | 29.054 | 29.344 | 29.137 | dentro | GIU | si | giu | +2.805 | 600.209 |
| ven 23 | 29.092 | 29.252 | 29.155 | dentro | SU | no | nessuno | -1.812 | 452.786 |
| lun 26 | 29.349 | 29.570 | 29.495 | dentro | SU | si | su | -4.001 | 458.549 |
| mar 27 | 29.637 | 29.880 | 29.805 | dentro | SU | si | su | -565 | 400.375 |

**Tab 3, seduta e notte**:

- Notte (dalle 18:00 New York = 23:00 ora italiana del 27, `oggi_notte`): apertura 29.814,25, massimo 29.989,25, minimo 29.783,75; volume 85.976, delta +1.546; POC 29.923, VAL 29.859, VAH 29.983.
- Pannello 10:17: prezzo DENTRO il valore di Londra 29.933-29.989, POC 29.974, sotto il POC; SOPRA il valore di cassa di ieri e di lunedi'; DENTRO il valore del 21/10 (framing) 29.883-30.087, POC 30.030.
- CVD (delta cumulato) dalla chiusura di ieri: +835 su 102.711 lotti (+0,8%); CVD al 79% del suo range, prezzo al 76% (pannello 10:17).
- Struttura oraria (giro, H1, ora italiana): gli ultimi due swing fanno massimi e minimi crescenti; swing alto 29.963,50 (28/10 04h), swing alti sotto a 29.881,25 e 29.875,50, swing basso 29.755,75 (27/10 19h). Mezz'ore: 08:30 delta +269, 09:00 -211, 09:30 -161, 10:00 +47.
- **Regime (pannello 10:17): CHOPPY**, ultime 30 barre 09:48-10:17, efficienza 0,08 (netto 16,25 su strada 214,00), 3 rotture rientrate. Alle 10:18 (giro): efficienza 0,05 (11,00 su 203,50), 09:49-10:18.
- **Velocita'** (proxy del volume per barra, non la speed of tape): barra in formazione 115 lotti, 19° percentile su 60 barre; barra chiusa 10:16: 288 lotti, 92° percentile. Alle 10:18: barra chiusa 10:17 140 lotti, 37° percentile. Un big trade da 60+ lotti in 10 barre, netto +61.
- Livello in gioco: VAL Europa 29.936, prezzo +3,75 sopra, arrivato da sopra. Veti: 1, legato al regime choppy.
- Volatilita' realizzata (rendimenti a 5 minuti, annua): 11,4% a 4 ore, 16,4% a 24 ore, 18,7% a 48 ore.

**Tab 4, esecuzione**: nessun dato. "TAB 4 (Range) non registrata nel bridge"; `range-NQZ6.json` = `{}`.

## Cosa Dicono Per NQ

- **Il valore si e' spostato su due volte di fila** (lun 26, mar 27: VAL e VAH piu' alti, staccati), dopo lo shift giu' del 22 (`profile-framing.md`, passo 2). Il blocco 22-23 si unisce (aree sovrapposte, passo 3): 29.054-29.344.
- **Il framing torna al 21/10**: il prezzo e' sopra il valore delle ultime due sedute e dentro quello del 21 (pannello e giro). Il VAH di ieri (29.880) e il VAL del 21 (29.883) distano 3 punti: fra le due aree non c'e' un vuoto.
- **Dove si sta nella curva** (passo 5; `i-tre-modelli-del-live-q1.md` §10): dentro il valore del 21 e dentro quello di Londra; nei compositi di cassa, sopra il POC e dentro il valore. Il live associa "dentro il valore" al modello in bilancia. **Il regime lo misura l'indicatore: choppy**, la terza riga della tabella del §7 (rotture che rientrano, velocita' morta). Non lo ricostruisco.
- **Due POC** (§10): il POC della seduta che incornicia (30.030, il 21) e il POC della seduta che ha creato la rottura. La rottura in su dal blocco 22-23 l'ha fatta lunedi' 26: POC 29.495. La rottura in giu' del 22 ha POC 29.137.
- **Osservazione**: le ultime tre cassa hanno delta negativo (-1.812, -4.001, -565) con il valore che sale; il 22, la seduta del crollo, ha delta positivo (+2.805). Il prezzo si e' mosso contro il saldo degli aggressori. Nessun documento del metodo letto qui ne trae una regola.
- **Osservazione**: la notte e' salita di circa 125 punti con delta +1,8% del volume; il CVD segue il prezzo (79% contro 76% del range). Nessuna divergenza misurata.
- **Il vuoto** (passo 6, fra il VAH di una seduta e il VAL di un'altra): VAH del 26 29.570, VAL del 27 29.637, 67 punti. Il profilo di ieri, la seduta piu' recente che ci e' passata, non ha fasce da 10 punti sopra il 2% fra 29.580 e 29.620, ne' fra 29.670 e 29.720: e' sottile anche nell'ultima seduta, quindi non e' un composito che inganna (la correzione di `profile-framing.md`).

## I Livelli E Le Date

Scala dei POC e bordi, dall'alto (fonte fra parentesi):

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.173 | POC cassa 14/10 (confluenze) | primo POC sopra la cornice del 21: il prezzo e' uscito dal valore del 21 |
| 30.087 | VAH cassa 21/10 (sedute) | bordo alto della cornice: sopra, il prezzo e' fuori dal valore del 21 |
| 30.030 | POC cassa 21/10 | centro del valore della cornice |
| 29.989 / 29.974 / 29.933 | VAH, POC, VAL di Londra (pannello 10:17) | la bilancia di stamattina: uscirne e' la prima rottura del giorno |
| 29.923 | POC della notte (sedute, dalle 23:00) | il prezzo piu' scambiato della notte |
| 29.883 / 29.880 | VAL 21/10 e VAH 27/10, 3 punti | sotto, il prezzo lascia la cornice e rientra nel valore di ieri |
| 29.805 | POC cassa 27/10 | centro del valore di ieri |
| 29.780 / 29.735 | POC compositi di cassa a 20 e 39 sedute | il picco di volume del periodo: la soglia fra parte alta e parte bassa della curva |
| 29.637 | VAL cassa 27/10 | bordo basso di ieri: sotto, il vuoto |
| 29.570-29.637 | vuoto fra VAH 26 e VAL 27 | zona di transito, non di sosta |
| 29.495 | POC cassa 26/10, la seduta della rottura in su | il primo punto che il live si aspetta ritrovato sulla via del ritorno |
| 29.344 / 29.054 | bordi del blocco 22-23 | il valore dopo il crollo: rientrarci annulla i due shift su |
| 28.996-28.998 | VAL compositi di cassa | il bordo del discount |
| 28/10 14:30 / 19:00-19:30 / dopo 21:00 | apertura cassa; FOMC e conferenza; MSFT e GOOGL (giro, `eventi.json`) | il regime di stamattina e' misurato su Londra: con la cassa e la Fed si rimisura |

## Cosa Non Sappiamo

- **Lacuna del collettore: le barre range** (tab 4): verso, durata e delta delle ultime barre e i percentili delle durate mancano; il ritmo si legge solo dal proxy del pannello.
- **Lacuna del collettore: il composito di cassa a 90 sedute ne ha 39**: l'archivio non arriva a 90, la regola del live non e' ancora rispettata. Il composito della tab 1 ha 90 barre, ma di 24 ore (cassa e notte insieme, che il §10 esclude).
- **Lacuna del collettore: due notti diverse**. `oggi_notte` parte alle 18:00 New York (POC 29.923, VAL 29.859); i livelli del pannello partono dalle 20:00Z (16:00 New York, la chiusura di cassa): POC notte 29.820, VAL notte 29.808. Per i livelli dal vivo vale il pannello; scrivo entrambe.
- **Lacuna del collettore: il valore di Londra cambia in un minuto**: 29.933-29.989 POC 29.974 alle 10:17 (pannello), 29.912-29.969 POC 29.941 alle 10:18 (giro). La finestra di Londra non e' dichiarata nei dati.
- Il delta per fascia di prezzo della cassa (per leggere chi ha agito nel vuoto) non e' nel riassunto; il profilo di ieri da' solo le quote sopra il 2%.

## Per Il Quadro

- **30.000**: il call wall della gamma (tutte le scadenze e 0DTE) cade fra il VAL di Londra e il POC del 21 (30.030): la cornice del 21 contiene il muro delle opzioni. Coerente?
- **29.570-29.600**: VAH del 26 e bordo del vuoto, zero gamma 29.578-29.590, put wall 0DTE 29.600, VAL della settimana del COT 29.584 (report 2026-10-20) e VAL cassa del 20/10 29.599: tre fonti, dati diversi.
- **29.757-29.770**: POC cassa del 19 e 20/10 e POC della settimana del report COT 2026-10-20, ma `stesso_dato`: e' lo stesso numero del bridge, non una conferma.
- **Regime choppy alle 10:17, Fed alle 19:00**: il regime e' misurato su Londra; la gamma lunga (convenzione A) descrive in teoria un mercato smorzato. Il quadro confronti le due letture.
