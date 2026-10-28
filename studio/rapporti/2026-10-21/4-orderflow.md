# Fase 4 · Order Flow · 2026-10-21

**Stato**: fatto (le lacune del collettore che restano sono in "Cosa Non Sappiamo")
**Dati**: letti `in-breve.json` (sezione `orderflow` e `confluenze`), `giro.txt`, `quattro-tab.txt`, `pannello.json`, `tab-ruoli.json`, `sedute-NQZ6.json`, `ore-NQZ6.json`, `range-NQZ6.json`. Raccolta `raccolta.json`: 2026-10-21 11:13 ora italiana, 82,6 s, tutte le fonti "ok", nessun errore. Pannello alle 11:12 (`pannello.json`, 09:12Z) e alle 11:13 (`giro.txt`).
**Convenzioni**: NQZ6; cassa 13:30-20:00Z = 15:30-22:00 italiane; valore al 70% del volume attorno al POC. **POC**: il prezzo con piu' volume. **VAL/VAH**: bordo basso e alto dell'area di valore. **Delta**: lotti aggressivi in acquisto meno in vendita; **CVD**: delta cumulato.

**In una riga**: dall'8 al 16 ottobre la zona dove si scambia di piu' e' scesa quasi ogni giorno. Il 16, il 19 e il 20 ottobre si e' fermata e si sovrappone. Stanotte il prezzo e' salito sopra e stamattina ci e' tornato dentro, in un mercato che va avanti e indietro senza direzione.

## In Parole Semplici
**Il punto**: dopo una settimana di discesa, da tre giorni il mercato scambia sempre nella stessa fascia, fra circa 29.270 e 29.850. Adesso il prezzo e' nel mezzo di quella fascia e si muove a strappi, senza direzione.

**Dal piu' ampio al piu' vicino**
1. **Il composito**: sulle ultime 34 sedute il prezzo sta nella zona dei prezzi accettati, vicino al prezzo piu' scambiato (circa 29.735).
   → Non e' ne' caro ne' a sconto: e' un prezzo che il mercato ha gia' accettato di recente.
2. **Le settimane**: dall'8 al 16 ottobre la zona dei prezzi accettati e' scesa quasi ogni giorno; oggi il prezzo sta quasi 1.850 punti sotto il massimo delle ultime venti sedute.
   → Era una discesa con una direzione: ogni giorno il mercato accettava prezzi piu' bassi.
3. **I giorni**: il 16, il 19 e il 20 ottobre le zone dei prezzi accettati si sovrappongono, fra circa 29.270 e 29.850.
   → La discesa si e' fermata: i tre giorni sono una trattativa sola, e contano i bordi del blocco.
4. **La notte**: stanotte il prezzo e' salito sopra il blocco, fino a circa 30.000, e verso le 10:30 e' ricaduto dentro.
   → I prezzi accettati di notte, sopra il blocco, per ora non hanno tenuto.
5. **Adesso**: alle 11:13 il prezzo e' circa 20 punti sopra il prezzo piu' scambiato di ieri (circa 29.760). In mezz'ora ha fatto quasi 400 punti di strada per spostarsi di meno di 40.
   → Il mercato e' in equilibrio, a strappi e lento prima dell'apertura americana delle 15:30, senza ordini grandi.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 29.850 | il bordo alto del blocco dei tre giorni | se il mercato ci resta sopra, i prezzi accettati escono dal blocco verso l'alto |
| circa 29.750 | il centro: il prezzo piu' scambiato di ieri e delle ultime 34 sedute | in un mercato in equilibrio, in teoria, e' il punto a cui il prezzo torna |
| circa 29.270 | il bordo basso del blocco, il minimo del 16 ottobre | se il mercato ci resta sotto, i prezzi accettati escono dal blocco verso il basso |

**Quanto e' provato**: la fascia, i bordi e il mercato a strappi sono misurati. Che in equilibrio il prezzo torni al centro e' un modello del corso di Fabio, non provato su questi dati. Nella ricaduta delle 10:30 chi comprava subito era piu' di chi vendeva: se fossero venditori fermi ad assorbire, non lo sappiamo misurare.
**Le parole**: **zona dei prezzi accettati** (area di valore) = la fascia dove si e' scambiato il 70% dei contratti; **prezzo piu' scambiato** = il singolo prezzo con piu' contratti; **composito** = la stessa misura su molti giorni insieme; **a strappi** = il prezzo va avanti e indietro e alla fine si sposta poco; **comprare subito** = comprare al prezzo che c'e', senza aspettare.

## I Fatti

**Tab 1, Composito** (prezzo 29.763,5 nella tab alle 11:12, 29.779,5 nel giro alle 11:13)

| composito | VAL | POC | VAH | zona | fonte |
|---|---|---|---|---|---|
| tab, grezzo: barre Daily di 24 ore, 90 barre dal 17/06 | 28.285 | 29.517,5 | 30.340 | fair value | `in-breve`, `composito_della_tab_1` |
| tab, aggiustato: scarto di rollover +299,25 fino all'11/09 | 29.235 | 29.817,5 | 30.720 | fair value | idem |
| sola cassa "90": 34 sedute dal 03/09, 6 prima del rollover | 29.085 | 29.735 | 30.845 | fair value | `in-breve`, `composito_di_cassa_90` |
| sola cassa 20 sedute, dal 23/09 | 30.235 | 30.770 | 31.414 | discount | `in-breve`, `composito_di_cassa` |

Nodi della cassa a 34 sedute (fasce da 5 punti, % del volume): 29.725-29.730 0,69; 30.770-30.775 0,68; 30.630-30.635 0,59; 30.565-30.570 0,54. **Premium** = sopra il VAH, **discount** = sotto il VAL, **fair value** = dentro. Nel range delle ultime 20 sedute (29.226-31.616,5) il prezzo e' al 23%, 1.837 punti sotto il massimo (`giro.txt`).

**Tab 2, Settimana**: il valore di cassa di ogni giorno (tab M30 e `sedute-NQZ6.json` danno gli stessi numeri). **Shift** giu' = VAL **e** VAH piu' bassi del giorno prima; **staccato** = area che non tocca quella del giorno prima.

| giorno | VAL | POC | VAH | shift | staccato | chiude | volume | delta |
|---|---|---|---|---|---|---|---|---|
| 08/10 | 30.949 | 31.235 | 31.369 | giu' | no | dentro | 530.520 | +9.344 |
| 09/10 | 31.039 | 31.090 | 31.122 | nessuno | no | dentro | 332.510 | -1.110 |
| 12/10 | 30.865 | 30.890 | 30.958 | giu' | si | dentro | 308.696 | +1.248 |
| 13/10 | 30.712 | 30.775 | 30.800 | giu' | si | dentro | 359.526 | -270 |
| 14/10 | 30.082 | 30.173 | 30.478 | giu' | si | dentro | 540.320 | +530 |
| 15/10 | 29.779 | 29.970 | 30.115 | giu' | no | sopra | 544.875 | +4.909 |
| 16/10 | 29.269 | 29.397 | 29.817 | giu' | no | dentro | 632.859 | +177 |
| 19/10 | 29.719 | 29.770 | 29.854 | su | no | dentro | 438.072 | -5.158 |
| 20/10 | 29.599 | 29.757 | 29.842 | giu' | no | dentro | 491.588 | -982 |

**Tab 3, Seduta** (`in-breve`, `ultime_5_sedute` e `oggi_notte`; ore da `ore-NQZ6.json` e `giro.txt`)

| | apertura | massimo | minimo | chiusura |
|---|---|---|---|---|
| 16/10 cassa | 30.023,5 | 30.045,75 | 29.268,75 | 29.379,25 |
| 19/10 cassa | 29.778,5 | 29.983,5 | 29.653,75 | 29.721,5 |
| 20/10 cassa | 29.594 | 29.903,5 | 29.391,75 | 29.688 |
| notte 21/10, dalle 00:00 | 29.925 | 30.010,25 | 29.731,75 | 29.762,5 alle 11:12 |

- 16/10: il volume piu' alto delle 20 sedute; il VAL (29.269) coincide col minimo. La barra di 24 ore del 19/10 apre a 29.227, sotto quel minimo (tab 1 del giro).
- 19/10: delta -5.158, il piu' negativo delle 20 sedute.
- 20/10: minimo 29.391,75 nell'ora 15:00-16:00. L'ora dopo sale a 29.903,5 con delta +2.052 e 126.300 lotti, l'ora piu' scambiata delle 48.
- Dopo la chiusura del 20/10, fra le 22:00 e le 23:00, il prezzo va da 29.687,5 a 29.893,75 con delta +1.260.
- Notte (22:00Z-13:30Z): 119.246 lotti, delta +582, POC 29.888, valore 29.844-29.974. Londra (pannello, in formazione): 29.808-29.917, POC 29.894.
- La mezz'ora delle 10:30 scende da 29.885,5 a 29.745,5 (152 punti) con delta +312 e 8.722 lotti.
- Struttura oraria (giro, swing H1 delle ultime 72 ore): sugli ultimi due swing, massimi decrescenti e minimi crescenti. I primi swing sopra il prezzo sono a 29.784,75 e 29.803,5, quelli sotto a 29.736,75 e 29.701,25.
- Tape (giro, solo barre chiuse): delta +429 (+12,6%) in 15 minuti, +611 (+4,4%) in 60 minuti, range 60 minuti 29.731,75-29.902,5.

**Il pannello** (l'indicatore; 11:13 da `giro.txt`, 11:12 da `pannello.json`)

| riga | 11:13 | 11:12 |
|---|---|---|
| REGIME | **CHOPPY**, 30 barre 10:44-11:13: efficienza 0,09 (netto 36,25 su strada 393,5), 3 rotture rientrate, copertura si | CHOPPY, 10:43-11:12: efficienza 0,11, 3 rotture rientrate |
| VELOCITA' (proxy, non la speed of tape) | in formazione 97 lotti, percentile 15; chiusa 11:12: 94 lotti, percentile 7 | chiusa 11:11: 175 lotti, percentile 53 |
| big trade da 60+ lotti, 10 barre | 0 | 0 |
| CVD dalla chiusura di ieri | +1.875 su 141.975 lotti; CVD al 74% del suo range, prezzo al 28% | +1.842; 72% e 23% |
| IN GIOCO | nessun livello vicino: sopra VAL Europa 29.808 (+29,25), sotto POC cassa ieri 29.757 (-21,75) | POC cassa ieri, +5,50 |

**Efficienza**: spostamento netto diviso strada percorsa (misura nostra). **Copertura**: barre che si sovrappongono `[6 · 1:19:04]`. **Rotture rientrate**: rotture di un livello tornate indietro `[6 · 54:03]`. Alle 11:12 i veti del pannello erano 2: libro sottile e regime choppy.

**Tab 4, Esecuzione**: barre range 40 (ogni barra copre 40 tick, 10 punti). Sulle ultime 30 chiuse: 16 su e 14 giu', delta +438 (+7,5%), escursione 29.731,75-29.782,75 in 21 minuti (giro). Durata mediana delle ultime 30: 36,1 s; su 600 barre p25 20,6 s, mediana 42,5 s (`range-NQZ6.json`). Volatilita' realizzata (rendimenti a 5 minuti, annua): 16,5% in 4 ore, 20,9% in 24 ore.

## Cosa Dicono Per NQ

1. **Composito.** Il prezzo e' in fair value in ogni composito a 90, in discount su quello a 20 sedute. Sta vicino al POC di sola cassa (29.735), il nodo piu' pesante. **Osservazione**: quel nodo l'hanno costruito le sedute dal 15/10, perche' prima del 14/10 nessun minimo di cassa scende sotto 30.000. E' l'ultima cosa costruita, non una mensola vecchia (`profile-framing.md`, "Sotto il VAL c'e' uno scaffale"). Il POC aggiustato della tab dista 82,5 punti da quello di sola cassa, perche' la tab usa barre di 24 ore, contro la regola "solo cash" `[1 · 1:14:15]`.
2. **Settimana.** Dall'8 al 16/10 il valore scende: sei shift giu', tre staccati. Per il passo 5 e' il modello del momentum, rottura e continuazione. Il 19/10 da' il primo shift su dal 06/10. Il 20/10 e' shift giu' alla lettera (VAL -120, VAH -12), ma le aree di 16, 19 e 20/10 si sovrappongono. Per il passo 3 si uniscono `[1 · 24:02]`: il blocco va da 29.269 a 29.854. **La seduta della rottura** e' il 16/10, POC 29.397 `[1 · 24:35]`. Il 20/10 il prezzo l'ha ritrovato a 5,25 punti, e l'ora dopo e' salito di 276 punti: un caso solo, coerente con il passo 4 (**osservazione**).
3. **Seduta.** Il prezzo sta dentro il blocco, dentro il valore di ieri. Per il passo 5, dentro il valore e in bilancia il live descrive il modello mean reverting, dal bordo al POC. L'indicatore misura lo stesso: regime choppy, efficienza 0,09. Anche la struttura oraria e' compressa: massimi decrescenti e minimi crescenti. La notte ha costruito valore sopra il blocco, poi alle 10:30 e' rientrata sotto il proprio VAL e sotto quello di Londra. **Osservazione**: il CVD sta al 74% del suo range e il prezzo al 28%. La discesa delle 10:30 ha avuto delta +312. Sono acquisti aggressivi senza salita; se sia assorbimento non e' misurato.
4. **Esecuzione.** Rotazione: 16 barre su e 14 giu', 51 punti in 21 minuti. La durata mediana e' nella norma (fra p25 e mediana). La barra chiusa delle 11:12 e' al percentile 7: il ritmo si spegne. La volatilita' delle 4 ore e' sotto quella delle 24.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.478-30.712 | vuoto fra VAH 14/10 e VAL 13/10 | zona di transito, non di sosta (passo 6) |
| 30.173 | POC 14/10, la seduta che rompe il blocco 07-13/10 | primo POC di rottura sopra il prezzo |
| 30.010,25 / 29.974 | massimo e VAH della notte | bordo alto del valore notturno |
| 29.888-29.894 | POC notte (collettore), POC Londra (pannello) | il valore costruito stanotte |
| 29.854 / 29.842 | VAH 19/10 (bordo del blocco), VAH 20/10 | accettazione sopra = valore fuori dal blocco, verso l'alto |
| 29.844 / 29.808 | VAL notte (collettore), VAL Londra | rientro nel valore della notte |
| 29.817,5 / 29.817 | POC composito aggiustato (tab), VAH 16/10 | picco del composito della tab |
| 29.770 / 29.757 | POC 19/10, POC 20/10 | il POC dentro la bilancia |
| 29.735 (29.725-29.730) | POC composito di sola cassa, 34 sedute | picco della distribuzione: fair value |
| 29.731,75 / 29.719 | minimo notte, VAL 19/10 | bordo basso del blocco 19-20/10 |
| 29.599 | VAL 20/10, 0 tocchi da ieri sera | bordo basso di ieri |
| 29.397 | POC 16/10, seduta della rottura | ritrovato il 20/10 (minimo 29.391,75) |
| 29.269 | VAL e minimo 16/10 | bordo basso del blocco: sotto, il valore esce verso il basso (`[1 · 1:19:30]`) |
| oggi 15:30 | apertura della cassa | `oggi_cassa` ancora vuoto |
| 26-30/10 | ora legale finita in Europa (25/10), non negli USA (01/11) | la cassa va dalle 14:30 alle 21:00 italiane |

## Cosa Non Sappiamo

- lacuna del collettore: il composito "90" di sola cassa ha 34 sedute, e quello della tab usa barre di 24 ore. Nessuno dei due e' ancora la regola del live.
- lacuna del collettore: la notte ha tre misure diverse. Il pannello parte dalle 20:00Z (POC notte 29.970, valore 29.890-30.000). Il collettore parte dalle 22:00Z (POC 29.888, valore 29.844-29.974). Il profilo del giro, dalle 22:00 italiane a fasce da 25, ha il POC a 29.875-29.900. Non scelgo: per il dal vivo vale il pannello, ma il suo POC notte non torna con il profilo della stessa finestra.
- lacuna del collettore: il campo `big` e' null su ogni livello, quindi zero big trade e registro scoperto non si distinguono. Manca la derivata del delta di fascia, che direbbe se c'e' assorbimento.
- lacuna del collettore: nella struttura oraria c'e' un'ora non valida ("19/10 25h", 29.803,5). Il "18,3%" della riga IN GIOCO non e' spiegato nei dati.
- Il calendario degli eventi e' vuoto (giro: "va aggiornato a mano"). La causa del rialzo dopo la chiusura del 20/10 non e' nei miei dati.

## Per Il Quadro

1. Gamma (`confluenze`, `stesso_dato` falso, fonti indipendenti). Lo **zero gamma** e' il prezzo dove la copertura dei dealer cambia verso. Quello della scadenza di oggi (29.747) cade sul POC di sola cassa (29.735) e fra i POC di 19 e 20/10. Quello su tutte le scadenze (29.835) cade sul VAH di ieri (29.842) e sul VAL della notte (29.844).
2. Gamma: lo strike pesante 29.400 sta sul POC 16/10 (29.397), il 29.600 sul VAL 20/10 (29.599). Il **call wall** 30.000 (lo strike con piu' gamma sulle call) e' il massimo della notte; il **put wall** (lo stesso sulle put) e' 29.500, dentro il blocco.
3. COT (report del 13/10): e' preso prima dei tre shift giu' del 14, 15 e 16/10, quindi il posizionamento non vede la rottura. Le sue confluenze sono tutte `stesso_dato` vero: il COT rilegge i valori di cassa del bridge, non li conferma.
4. Volatilita' realizzata 16,5-20,9% contro l'implicita ATM di oggi, 28,1% (giro). E il rialzo dopo la chiusura del 20/10: c'e' un evento in macro?
