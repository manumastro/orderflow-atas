# Fase 4 · Order Flow · 2026-10-16

**Stato**: fatto
**Dati**: `dati/tab-ruoli.json`, `pannello.json`, `sedute-NQZ6.json`, `ore-NQZ6.json`, `range-NQZ6.json` (raccolti alle 09:34 italiane), `giro.txt` e `confluenze.json` (09:36), `raccolta.json` (tutte le fonti "ok", fine alle 09:36). Prima della cassa: `oggi_cassa` e' vuoto, come deve. Errori della raccolta: nessuno.

**In una riga**: il valore di cassa e' sceso per quattro sedute, ieri la prima chiusura sopra il proprio valore; il prezzo e' sopra il valore di ieri, in discount nel composito di sola cassa, e l'indicatore misura il regime **CHOPPY** (alle 09:34 e alle 09:36).

Convenzioni: cassa = 15:30-22:00 italiane (13:30-20:00 UTC); notte = 00:00-15:30 italiane (22:00-13:30 UTC); valore al 70% dal POC (`sedute-NQZ6.json`). POC = prezzo col volume massimo; VAL e VAH = bordi basso e alto dell'area di valore. Shift = VAL **e** VAH piu' bassi (o piu' alti) del giorno prima, alla lettera del live.

## I Fatti

**Tab 1, Composito** (`tab-ruoli.json`; barre Daily di 24 ore, 90 barre dal 15/06, grana 5 punti):

| | VAL | POC | VAH | zona del prezzo 30.165,75 |
|---|---|---|---|---|
| grezzo | 28.410 | 29.517,5 | 30.540 | fair value |
| aggiustato (scarto 299,25 fino all'11/09) | 29.340 | 29.817,5 | 30.815 | fair value |
| sola cassa, 20 sedute dal 18/09 (`sedute-NQZ6.json`) | 30.458 | 30.770 | 31.209 | discount |
| sola cassa "90", **31 sedute** dal 03/09, 6 prima del rollover | 30.389 | 30.770 | 31.411 | discount |

Nodi del composito di cassa "90" (fasce da 5 punti, % del volume): 30.770-30.775 (0,79), 30.630-30.635 (0,69), 30.565-30.570 (0,62), 30.900-30.905 (0,61), 29.735-29.740 (0,55).

**Tab 2, Settimana** (`tab-ruoli.json`, M30; coincide con `sedute-NQZ6.json`):

| giorno | VAL | POC | VAH | chiude | shift | staccato | delta |
|---|---|---|---|---|---|---|---|
| 09/10 | 31.039 | 31.090 | 31.122 | dentro | nessuno | no | -1.110 |
| 12/10 | 30.865 | 30.890 | 30.958 | dentro | giu' | si' | +1.248 |
| 13/10 | 30.712 | 30.775 | 30.800 | dentro | giu' | si' | -270 |
| 14/10 | 30.082 | 30.173 | 30.478 | dentro | giu' | si' | +530 |
| 15/10 | 29.779 | 29.970 | 30.115 | **sopra** | giu' | no | +4.909 |

Il 15/10: apertura 29.746, minimo 29.622,5, massimo 30.196,75, chiusura 30.140,25, volume 544.875 (il piu' alto delle 20 sedute). Profilo da 10 punti (fasce sopra il 2%): nodo 29.950-30.030 (31% del volume), nodo 29.770-29.840 (20%), nodo 30.140-30.170 (7,6%); fra 30.030 e 30.140 nessuna fascia sopra il 2%.

**Tab 3, Seduta** (`tab-ruoli.json` M1, `pannello.json`, `oggi_notte`, `giro.txt`):

| | VAL | POC | VAH | altro |
|---|---|---|---|---|
| notte (00:00-09:34) | 30.049 (pannello: 30.045) | 30.190 | 30.207 | min 29.959,25, max 30.225; delta +336 su 77.860 |
| Londra, in formazione | 30.160 | 30.173 | 30.186 (09:36: 30.185) | prezzo dentro, sotto il POC |

- **Regime (pannello)**: **CHOPPY**. Alle 09:34, ultime 30 barre 09:05-09:34: efficienza 0,01 (netto 3,75 su strada 330), 3 rotture rientrate. Alle 09:36 (`giro.txt`), barre 09:07-09:36: efficienza 0,01 (netto 4,50 su 310,75), 2 rotture rientrate. **Veti**: 1, il regime choppy.
- **Velocita'** (proxy dei lotti, non la speed of tape): 09:34, 113 lotti, 58° percentile su 60 barre; 09:36, 94 lotti, 42° percentile; barra chiusa 09:35: 90 lotti, 36° percentile. 0 big trade da 60+ lotti in 10 barre.
- **CVD** (delta cumulato) dalla chiusura di ieri (22:00): +432 su 93.696 lotti alle 09:34, +374 su 93.918 alle 09:36; CVD al 79% del suo range, prezzo al 76%.
- **In gioco**: VAL Europa 30.160, prezzo da +5,75 (09:34) a +1,00 sopra (09:36), arrivato da sopra. Tocchi dalle 22:00 di ieri (`alLivello`, 09:34): VAL Europa 25 (tiene 10, passa 15), VAH notte 12 (tiene 10), max cash di ieri 30.196,75: 48 (tiene 34, passa 14).
- **Struttura oraria** (`giro.txt`, swing H1 delle ultime 72 ore): **gli ultimi due swing H1 hanno massimi e minimi crescenti**. Sopra: 30.196,75 (max 15/10 21h), 30.225 (max 16/10 07h), 30.591,25, 30.705,25. Sotto: 29.978,25, 29.959,25 (min 16/10 02h), 29.899,5, 29.823,5.
- **Tape** (`giro.txt`, sole barre M1 chiuse): delta 15 minuti -104 (-4,4%), 30 minuti -4, 60 minuti +151 (+2,0%); range dell'ultima ora 30.126-30.198,5.
- **Profilo della notte, delta per fascia da 25** (`giro.txt`, 22:00-09:36, 93.920 lotti): POC 30.175-30.200 (15,5%, delta +97); unica fascia sopra l'8% col delta negativo 30.075-30.100 (-601).
- **Volatilita' realizzata** (log a 5 minuti, annua 276x252): 14,0% a 4 ore, 19,9% a 24, 21,3% a 48.
- **Rendimento dalla chiusura di ieri** (09:34): +25,5 punti, +0,085%.

**Tab 4, Esecuzione** (Range 40): durata mediana 42,8 s su 600 barre (p25 19,6, p75 89,6), ultime 30: 44,5 s (`range-NQZ6.json`). Ultime 30 chiuse alle 09:36 (`giro.txt`): 15 su e 15 giu', delta +62 (+1,4%), 30.158-30.198,5 in 32 minuti.

## Cosa Dicono Per NQ

1. **Il valore scende da quattro sedute** (shift giu' 12-15/10): dal VAH 31.556 del 06/10 al 30.115 di ieri, 1.441 punti (`profile-framing.md`, passo 2).
2. **Il 14/10 e il 15/10 si uniscono** (VAH 30.115 di ieri sopra il VAL 30.082 del 14/10): blocco unico 29.779-30.478 (passo 3, `[1 · 24:02]`). Il prezzo e' dentro il blocco, sul POC del 14/10 (30.173), che coincide col POC di Londra.
3. **La seduta della rottura** al ribasso e' il 14/10: apre a 30.551,5, sotto il VAL del 13/10 (30.712), con valore staccato. Il suo POC 30.173 e' "il POC della seduta che ha creato la rottura" `[1 · 24:35]`, e il prezzo lo sta ritrovando.
4. **Ieri la prima chiusura sopra il proprio valore**, col delta piu' alto della settimana (+4.909); la notte ha costruito valore piu' in alto (POC 30.190) e gli ultimi due swing H1 salgono. **Osservazione**: la notte non entra nel profilo di cassa `[1 · 1:14:15]`, quindi non e' uno shift.
5. **Premium e discount dipendono dal composito**: la tab Daily (24 ore) dice fair value; il composito di sola cassa, la regola del live (`i-tre-modelli-del-live-q1.md` §10), dice discount, ma ha 31 sedute e non 90. La differenza si scrive, non si sceglie.
6. **Il regime e' choppy come lo misura l'indicatore**, in due letture a due minuti di distanza. Nell'ultima mezz'ora le barre range sono 15 su e 15 giu', il delta e' piatto (+62), la durata e' alla mediana, e la velocita' scende dal 58° al 42° percentile: il ritmo non accelera. La volatilita' realizzata cala (21,3% a 48 ore, 14,0% a 4).
7. **La fascia 30.030-30.140** e' sottile nel profilo di ieri, ma la notte ci ha scambiato il 3-4,5% per fascia: **non e' un vuoto** (`profile-framing.md`, "sotto il VAL c'e' uno scaffale").

## I Livelli E Le Date

| prezzo | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.712 / 30.478 | VAL 13/10 / VAH 14/10 (M30) | **vuoto** di 234 punti fra due sedute: zona di transito, si attraversa in fretta o si respinge al bordo (passo 6) |
| 30.478 | VAH del blocco 14-15/10 | bordo alto del blocco: sopra, il prezzo esce dal valore unito |
| 30.389 | VAL composito di cassa (31 sedute) | confine discount / fair value del composito di cassa |
| 30.225 / 30.207 | max / VAH notte (pannello) | bordo alto della notte e ultimo massimo di swing H1 |
| 30.196,75 | max cash di ieri, swing H1 | il livello piu' toccato dalle 22:00 (48 tocchi) |
| 30.190 | POC notte | scala dei POC sopra il prezzo; sul chart unito al VAH Europa |
| 30.173 | POC 14/10 (M30; 30.187 sulla tab Range) e POC Londra | POC della seduta della rottura: il primo livello ritrovato sulla via del ritorno |
| 30.160 | VAL Europa (pannello) | livello in gioco alle 09:34 e alle 09:36 |
| 30.115 | VAH 15/10 | bordo del valore di ieri: rientrarci = di nuovo dentro il valore di ieri |
| 30.049 | VAL notte (pannello: 30.045) | bordo basso della notte |
| 29.970 / 29.959,25 | POC 15/10 / minimo di swing H1 delle 02h | scala dei POC sotto il prezzo; perderlo rompe la sequenza di minimi crescenti |
| 29.779 | VAL 15/10 e del blocco | bordo basso del blocco; "il VAL e' una strada" `[1 · 1:19:30]` |
| 30.770 / 30.890 | POC composito di cassa e 13/10 / POC 12/10 | scala dei POC sopra il vuoto |
| 15:30 | apertura della cassa | si apre il primo profilo di cassa di oggi |
| 21:30 | COT (posizioni di martedi' 13) | il report nuovo, dopo la chiusura dello studio |

## Cosa Non Sappiamo

- Lacuna del collettore: il **delta per fascia** nel `profilo_da_10` delle sedute di cassa (c'e' solo per la notte, nel giro): non si dice chi e' stato aggressivo nei nodi di ieri.
- Lacuna del collettore: `composito_di_cassa_90` ha 31 sedute, non 90, e non dichiara se le 6 prima del rollover sono aggiustate; il nodo 29.735-29.740 puo' venire da li'.
- Lacuna del collettore: la durata in `range-NQZ6.json` include le pause del mercato, e verso e delta delle barre range stanno solo nel giro, a quattro barre.
- Il POC del 14/10 differisce fra la tab M30 (30.173) e la tab Range (30.187): 14 punti, non spiegati.
- Il VAL della notte differisce di 4 punti (30.049 collettore, 30.045 pannello): per il livello dal vivo vale il pannello.
- La velocita' e' un proxy dei lotti, non la speed of tape.

## Per Il Quadro

1. **30.100-30.115** (`confluenze.json`, `stesso_dato` falso): call wall 0DTE e strike pesante a 30.100 contro il VAH di ieri 30.115. Due fonti indipendenti sul bordo del valore di ieri.
2. **30.190-30.200**: POC della notte 30.190 contro lo zero gamma "entro 7 giorni" 30.190,7 e lo strike 30.200. In piu' (non in `confluenze.json`) lo zero gamma "tutte" 30.231 sta a 6 punti dal massimo della notte 30.225: confrontarli.
3. **Le confluenze col COT sono tutte `stesso_dato` vero** (30.549-30.557, 30.609, 30.888-30.922, 31.483-31.556): sono gli stessi numeri di cassa del bridge alle date dei report, non conferme indipendenti. Il discount nel composito e i quattro shift giu' vanno confrontati col netto Non-Commercial di `2-cot.md`.
4. **Regime choppy e volatilita' realizzata 14-21%** contro l'implicita ATM della fase 3 (25,4% sulla 0DTE, 15,2-17,0% sulle scadenze dal 19 al 21/10).
