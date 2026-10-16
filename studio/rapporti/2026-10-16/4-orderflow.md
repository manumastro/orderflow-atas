# Fase 4 · Order Flow · 2026-10-16 (aggiornamento del pomeriggio)

**Stato**: parziale (la cassa di oggi c'e' solo nel giro delle 15:32, a due minuti dall'apertura; `oggi_cassa` e' vuoto perche' le sedute sono raccolte alle 15:29)
**Dati**: `dati/in-breve.json` (sezione `orderflow`, 15:32), `giro.txt` (15:32), `raccolta.json` (tutte le fonti "ok", fine alle 15:32). Pannello, sedute, ore e range raccolti alle 15:29, prima della cassa; COT al report del 06/10 (10 giorni), il nuovo esce alle 21:30. Errori della raccolta: nessuno. Rapporto precedente: `precedenti/0936/4-orderflow.md`.

**In una riga**: il prezzo e' sceso da 30.165,75 (09:34) a circa 30.015 ed e' rientrato nel valore della cassa di ieri; la cassa di oggi e' appena nata, con volumi alti e delta positivo, e l'indicatore misura il regime **CHOPPY** (15:32).

Convenzioni: cassa = 15:30-22:00 italiane (13:30-20:00 UTC); notte = 00:00-15:30 italiane (22:00-13:30 UTC); valore = area col 70% del volume attorno al POC (prezzo col volume massimo); VAL e VAH = bordi basso e alto. Shift = VAL **e** VAH piu' bassi (o piu' alti) del giorno prima (`profile-framing.md`, passo 2). CVD = delta cumulato (lotti all'ask meno lotti al bid).

## I Fatti

**Tab 1, Composito** (`in-breve.json`; prezzo 30.040 alle 15:29):

| | VAL | POC | VAH | zona |
|---|---|---|---|---|
| Daily 24 ore, 90 barre dal 15/06, grezzo | 28.410 | 29.517,5 | 30.540 | fair value |
| aggiustato (scarto 299,25 fino all'11/09) | 29.340 | 29.817,5 | 30.815 | fair value |
| sola cassa, 20 sedute dal 18/09 | 30.458 | 30.770 | 31.209 | discount |
| sola cassa "90", **31 sedute** dal 03/09 (6 prima del rollover) | 30.389 | 30.770 | 31.411 | discount |

Identico alle 09:36: i compositi non si muovono in giornata.

**Tab 2, Settimana** (`ultime_5_sedute`; invariata da stamattina):

| giorno | VAL | POC | VAH | chiude | shift | staccato | delta |
|---|---|---|---|---|---|---|---|
| 12/10 | 30.865 | 30.890 | 30.958 | dentro | giu' | si' | +1.248 |
| 13/10 | 30.712 | 30.775 | 30.800 | dentro | giu' | si' | -270 |
| 14/10 | 30.082 | 30.173 | 30.478 | dentro | giu' | si' | +530 |
| 15/10 | 29.779 | 29.970 | 30.115 | **sopra** | giu' | no | +4.909 |

**Tab 3, Seduta**:

| | VAL | POC | VAH | altro | fonte |
|---|---|---|---|---|---|
| notte chiusa (00:00-15:30) | 30.030 | 30.060 | 30.167 | min 29.959,25, max 30.225; vol 142.019, delta +483 | `oggi_notte` |
| Londra (15:29) | 30.040 | 30.076 | 30.136 | prezzo sul VAL, sotto il POC | pannello |
| **cassa di oggi, 15:32** | 29.977 | 29.989 | 30.013 | **2 minuti di dati** | pannello, `giro.txt` |
| cassa di oggi, tab M30 | 29.983 | 29.989 | 30.017 | in corso | `giro.txt` |

- **Regime (indicatore, 15:32)**: **CHOPPY**. Finestra: ultime 30 barre M1, 15:03-15:32; efficienza 0,09 (netto 48 su strada 526,25), 3 rotture rientrate. **Veti**: 2, il regime choppy e il delta di seduta positivo (+964) contrario allo scenario del pannello. Lo scenario non si riporta.
- **Velocita'** (proxy dei lotti, non la speed of tape): barra 15:32 in formazione 4.211 lotti, 97° percentile su 60 barre; barra chiusa 15:31 7.081 lotti, 98°. 4 big trade da 60+ lotti in 10 barre, netto -238.
- **CVD**: dalla chiusura di ieri +579 su 157.855 lotti (15:29), CVD all'89% del suo range e prezzo al 30%. Dall'apertura di cassa +964 (+4,6%) su 21.128 lotti (15:32), CVD al 100% del range, prezzo al 64%. L'indicatore scrive "assorbimento?" in tutte e due.
- **In gioco** (15:32): VAH della cassa in sviluppo ~30.013, prezzo +2,50 sopra, arrivato da sotto.
- **Tape** (`giro.txt`, barre M1): 15:30 vol 9.836 delta +288; 15:31 vol 7.081 delta +253, minimo 29.962,50. Solo barre chiuse: delta 15 minuti +338 (+1,2%), 30 minuti +33, 60 minuti -136 (-0,3%).
- **Mezz'ore** (`giro.txt`, M30): 14:30 delta -111, 15:00 -469, 15:30 (aperta) +916 su 20.902.
- **Struttura oraria** (swing H1 delle ultime 72 ore): il collettore dice "ultimi due swing: massimi e minimi crescenti". Sopra il prezzo: 30.126 (min 16/10 08h), 30.196,75, 30.225. Sotto: 29.978,25, 29.959,25 (min 16/10 02h), 29.899,5, 29.823,5.
- **Profilo 22:00-15:32** (`giro.txt`, fasce da 25, 181.277 lotti): POC 30.100-30.125 (14,6%, delta +507); fasce col delta negativo 30.025-30.100 (-497, -220, -513); 30.000-30.025 +436.
- **Rendimento dalla chiusura di ieri** (30.140,25, 15:29): -100,25 punti, -0,333%.
- **Volatilita' realizzata** (log a 5 minuti, annua 276x252): 15,1% a 4 ore, 19,6% a 24, 19,7% a 48.

**Tab 4, Esecuzione** (Range 40; `range-NQZ6.json` alle 15:29, `giro.txt` alle 15:32): durata mediana 53,6 s su 600 barre (p10 16,1, p25 30,1, p75 97,0); **mediana delle ultime 30: 24,9 s**, sotto il p25. Ultime 30 chiuse alle 15:32: 16 su, 14 giu', delta +1.311 (+8,5%), 29.962,50-30.024,75 in 2 minuti.

## Cosa E' Cambiato Dalle 09:36

| | 09:34-09:36 | 15:29-15:32 |
|---|---|---|
| prezzo | 30.165,75 | 30.040 (15:29), 30.015,5 (15:32) |
| rispetto al valore di ieri | sopra il VAH 30.115 | **dentro**, sopra il POC 29.970 |
| POC della notte | 30.190 | 30.060 (notte chiusa) |
| rendimento da ieri | +25,5 (+0,085%) | -100,25 (-0,333%) |
| velocita' | 42°-58° percentile | 97°-98° percentile |
| barre range, mediana ultime 30 | 44,5 s | 24,9 s |
| regime | CHOPPY, efficienza 0,01 | CHOPPY, efficienza 0,09 |

## Cosa Dicono Per NQ

1. **Il valore di cassa scende da quattro sedute**; ieri la prima chiusura sopra il proprio valore. Il prezzo e' tornato dentro il valore di ieri: la prima chiusura sopra non ha avuto continuazione nella notte (`profile-framing.md`, passi 2 e 5).
2. **Il blocco 14-15/10 resta unito** (VAH 30.115 sopra il VAL 30.082 del 14/10): 29.779-30.478 (passo 3). Dentro il valore, in bilancia, il live descrive il modello mean reverting, dal bordo al POC (passo 5); il regime misurato e' choppy, coerente con una bilancia. **Osservazione**, non un permesso.
3. **La notte ha spostato il suo POC in basso** (30.190 alle 09:34, 30.060 a fine notte): il valore della notte si e' costruito dove ieri c'era il bordo alto di cassa. La notte non entra nel profilo di cassa (passo 1): non e' uno shift.
4. **Il POC della seduta della rottura** (14/10, 30.173) era il livello ritrovato stamattina; ora il prezzo e' 160 punti sotto, e il POC di ieri (29.970) e' il piu' vicino sotto.
5. **La cassa e' appena nata**: due minuti, 21.128 lotti, valore 29.977-30.013 largo 36 punti. Un valore di cassa di due minuti non e' ancora una distribuzione: VAL, POC e VAH cambiano da soli. Lo shift di oggi non si puo' ancora dire.
6. **Il ritmo e' cambiato**: velocita' al 97°-98° percentile, barre range a meta' della mediana, delta d'apertura positivo con CVD al massimo del suo range e prezzo al 64%. Il minimo di 15:31 (29.962,50) sta 3,25 punti sopra il minimo della notte e swing H1 (29.959,25). **Osservazione**: l'etichetta "assorbimento?" e' dell'indicatore, non una misura provata.
7. **La struttura oraria e' ambigua**: il collettore dice minimi crescenti, ma il prezzo e' sotto il minimo di swing H1 delle 08h (30.126). Il verdetto e' sui due swing chiusi; il collettore non dice se 30.126 e' stato rotto in chiusura H1 (vedi lacune).
8. **La fascia 29.980-30.030 e' sottile nel profilo della notte** (fasce sotto il 2%), ma ieri in cassa il nodo 29.950-30.030 aveva il 31% del volume: **non e' un vuoto**, e' l'ultima cosa costruita (`profile-framing.md`, "sotto il VAL c'e' uno scaffale").

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.712 / 30.478 | VAL 13/10 / VAH 14/10 | **vuoto** di 234 punti fra due sedute: zona di transito (passo 6) |
| 30.389 | VAL composito di cassa (31 sedute) | confine discount / fair value del composito di cassa |
| 30.225 / 30.167 | max / VAH notte | bordi alti della notte |
| 30.173 | POC 14/10, seduta della rottura | posizionamento: il POC ritrovato stamattina e poi lasciato |
| 30.126 | min swing H1 16/10 08h | permesso di leggere i minimi crescenti: sotto, la sequenza e' in dubbio |
| 30.115 / 30.082 | VAH 15/10 / VAL 14/10 | bordo alto del valore di ieri e unione dei due giorni |
| 30.076 / 30.060 | POC Londra / POC notte | scala dei POC sopra il prezzo |
| 30.040 / 30.030 | VAL Londra / VAL notte | bordi bassi di Londra e della notte, appena sopra |
| ~30.013 / 29.989 / ~29.977 | VAH, POC, VAL cassa in sviluppo (15:32) | il valore di oggi, appena nato: cambia da solo |
| 29.970 | POC 15/10 | POC del valore di ieri, il piu' vicino sotto |
| 29.959,25 | min notte e swing H1 02h | perderlo rompe la sequenza dei minimi H1 |
| 29.779 | VAL 15/10 e del blocco | bordo basso del blocco; "il VAL e' una strada" `[1 · 1:19:30]` |
| 30.770 | POC compositi di cassa | scala dei POC sopra il vuoto |
| 21:30 | COT (posizioni di martedi' 13) | il report nuovo, quadro da rifare |
| 22:00 | chiusura cassa e scadenza 0DTE | il valore di oggi diventa una seduta chiusa |

## Cosa Non Sappiamo

- **Lacuna del collettore**: `oggi_cassa` e' vuoto (`sedute-NQZ6.json` raccolto alle 15:29, prima dell'apertura); il valore di cassa di oggi viene solo dal pannello e dalla tab M30 del giro, e differisce di 4-6 punti fra i due (VAL 29.977 contro 29.983, VAH 30.013 contro 30.017).
- **Lacuna del collettore**: il pannello in `in-breve.json` e' quello delle 15:29 senza regime; regime, velocita' e veti esistono solo nel giro delle 15:32.
- **Lacuna del collettore**: la struttura H1 non dice se il minimo di swing 30.126 e' rotto in chiusura oraria.
- **Lacuna del collettore**: delta per fascia del profilo di cassa di ieri; `composito_di_cassa_90` con 31 sedute e non 90, senza dire se le 6 pre-rollover sono aggiustate.
- La durata delle barre range include le pause; la velocita' e' un proxy dei lotti.

## Per Il Quadro

1. **30.000**: call wall e put wall di tutte le scadenze e strike piu' pesante della gamma (fase 3), contro il POC della cassa in sviluppo 29.989 e il prezzo ~30.015. Non e' in `confluenze.json` perche' il valore di cassa non era raccolto: confrontarlo.
2. **30.082-30.115** (`stesso_dato` falso): VAL 14/10 e VAH 15/10 contro lo zero gamma 0DTE 30.096 e lo strike 30.100. **30.167-30.181**: VAH notte e POC 14/10 contro lo zero gamma "tutte" 30.181. Il prezzo e' sotto tutti e tre gli zeri gamma.
3. **COT**: le confluenze 30.549-31.556 sono `stesso_dato` vero (numeri di cassa del bridge alle date dei report), non conferme indipendenti. Il discount del composito di cassa e i quattro shift giu' vanno confrontati col netto Non-Commercial del 06/10, e col report delle 21:30.
4. **Volatilita'**: realizzata 15,1-19,7% e ritmo d'apertura al 97° percentile contro l'implicita ATM 0DTE 28,6% (mossa attesa +-235).
