# Fase 4 · L'Order Flow Delle Quattro Tab, NQ Ed ES Insieme · 9 novembre 2026, aggiornamento pre-cassa

**Stato**: parziale (manca ancora la tab 4: la Range non e' registrata nel bridge, `barre_range` vuoto; il pannello di ES non ha livelli ne' CVD).
**Dati**: `dati/` (NQ) e `dati-ES/` (ES): `in-breve.json` (sezione `orderflow`), `pannello.json`, `giro.txt`. Pannelli di entrambi alle 14:52 ora italiana (13:52 UTC); giro di ES alle 14:52, giro di NQ alle 15:01 (14:01 UTC): **i due giri non sono allo stesso minuto**. GEX viva di NQ e X-Ray alle 14:51. Il rapporto precedente (12:28) e' in `precedenti/1452/4-orderflow.md`. Prezzi di ciascuno nei suoi punti, mai convertiti.

**In una riga**: in due ore e mezza NQ ed ES sono saliti insieme sopra la zona di venerdi' e sopra il massimo della notte; NQ ha recuperato il ritardo, ES resta piu' avanti nella struttura, e l'indicatore misura un mercato in bilancia che sta perdendo il ritmo.

## In Parole Semplici

**Il punto**: tutti e due sono ancora molto sotto la zona dei tre mesi, ma stamattina sono usciti dall'alto dalla zona di venerdi'. Alle 12:28 ES era piu' forte; adesso dalla chiusura di venerdi' sono saliti quasi uguale (circa l'1%). ES ha gia' superato il massimo di venerdi', NQ no.

**Dal piu' ampio al piu' vicino**

1. **Le settimane**: NQ ed ES restano sotto il valore dei tre mesi; la settimana scorsa ha sceso uno scalino per tutti e due.
   → Niente e' cambiato qui: siamo in basso nella curva, NQ piu' in basso di ES.
2. **I giorni**: giovedi' ha rotto verso il basso, venerdi' si e' fermato dentro giovedi'.
   → Oggi il prezzo e' sopra la zona di venerdi' su tutti e due, ma ancora dentro la zona di giovedi'.
3. **La notte**: NQ ha superato il massimo delle 03 (circa 27.320) e ha fatto un massimo nuovo verso le 14; anche ES ha fatto un massimo nuovo.
   → La guida di ES si e' ridotta: NQ ha recuperato. ES e' l'unico sopra il massimo di venerdi'.
4. **Adesso**: l'indicatore misura "bilancia" su tutti e due alle 14:52, e "zig-zag" su NQ alle 15:01; il ritmo della barra in corso e' fermo.
   → Prima della cassa il mercato si e' fermato appena sopra i massimi. La tab piu' vicina (Range) non e' leggibile.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ 27.425 · ES 7.469 | il massimo di venerdi', il muro alto del ping-pong | ES e' gia' sopra di 3 punti; se una giornata chiude oltre, il ping-pong e' rotto |
| NQ 27.361 · ES 7.537,75 | il bordo alto della zona di giovedi', il giorno della rottura | sopra, il valore esce dal blocco della rottura e c'e' una zona vuota fino a martedi' |
| NQ 27.306 · ES 7.454,50 | il bordo alto della zona di venerdi' | tornarci sotto vuol dire rientrare nella zona di ieri |

**Cambia idea se**: ES torna sotto 7.469 e NQ resta sopra 27.319: la guida passa a NQ. O tutti e due rientrano sotto il bordo alto di venerdi'.
**Quanto e' provato**: valori, delta e regime sono misurati dal collettore e dall'indicatore; chi guida e' un'osservazione; gli scalini sono smentiti come previsione su NQ 2017-2024; nessuno studio prova che una divergenza fra NQ ed ES dica il verso.

**Le parole**: **valore** = la fascia dove si e' scambiata la maggior parte del volume; **delta** = acquisti aggressivi meno vendite aggressive; **bilancia** (balance) = il prezzo va avanti e indietro senza scegliere; **zig-zag** (choppy) = molta strada, quasi nessun cammino netto; **ping-pong** = il prezzo rimbalza fra i due estremi di venerdi' dopo lo scalino; **scalino** = candela con massimo, minimo e chiusura oltre la precedente.

## I Fatti

**Cosa e' cambiato dalle 12:28** (`in-breve.json` `rendimento_dalla_chiusura_di_ieri` e `oggi_notte`, 13:52 UTC; il 12:28 dal rapporto precedente):

| | NQZ6 12:28 -> 14:52 | ESZ6 12:28 -> 14:52 |
|---|---|---|
| prezzo | 27.211,50 -> 27.345,25 (15:01: 27.313,50) | 7.451 -> 7.470,25 |
| dalla chiusura di venerdi' (BDLM) | +136 (+0,50%) -> +270,75 (+1,0%) | +51 (+0,69%) -> +70,25 (+0,949%) |
| massimo della notte | 27.319 (ore 03) -> 27.378 (swing H1 delle 13) | 7.459 -> 7.476,50 |
| notte: valore, POC | 26.928-27.184, POC 27.100 -> 26.928-27.248, POC 27.100 | 7.392,75-7.436, POC 7.428,50 -> 7.402,75-7.458,25, POC 7.446 |
| notte: delta su volume | +2.396 su 172.276 -> +4.016 su 218.038 | +1.935 su 242.645 -> +6.790 su 337.636 |
| rispetto al valore di venerdi' | dentro -> sopra il VAH di 7 (giro 15:01) | dentro -> sopra il VAH di 17,25 (giro 14:52) |
| rispetto al massimo di venerdi' (muro alto) | 27.425: sotto di 80 (15:01); mai toccato (pannello: 0 tocchi) | 7.469: sopra di 2,75 (giro 14:52) |
| regime (indicatore, 30 barre) | CHOPPY 0,09 -> BALANCE 0,17 (14:23-14:52) -> CHOPPY 0,03 (14:32-15:01) | CHOPPY 0,10 -> BALANCE 0,18 (14:23-14:52) |

**Tab 1, composito** (`in-breve.json` `orderflow`, invariato dalle 12:28): composito di cassa 90 NQ 28.998-31.080, POC 29.735 (47 sedute dal 03/09); ES 7.676-7.862, POC 7.754 (26 sedute dal 02/10). Composito della tab 1 (90 barre daily di 24 ore) grezzo NQ 28.270-30.255, POC 29.517,50; aggiustato +299,25: 28.275-30.285, POC 29.817,50. ES grezzo 7.615-7.860, POC 7.767,50; aggiustato +67,75: 7.650-7.855, POC 7.762,50. Zona: **discount** in tutte le misure, su entrambi. Posizione nel range di 20 sedute (`giro.txt`): NQ 21% (15:01), ES 27% (14:52); alle 12:28 erano 18% e 23%.

**Gli scalini** (`scalini`, invariati, ultima candela chiusa 06/11): mese pausa, settimana scalino giu', giorno pausa, insieme "misti" su entrambi. Ping-pong dal 06/11, 1 giornata: NQ 26.366,50-27.425, ES 7.322,25-7.469.

**Tab 2, la settimana** (`ultime_5_sedute`, valore di cassa · POC · delta di cassa · chiude):

| seduta | NQZ6 | ESZ6 |
|---|---|---|
| mar 03 | 28.296-28.387 · 28.314 · -5.843 · dentro, shift giu' | 7.589,75-7.609,25 · 7.597 · -3.521 · dentro, shift giu' |
| mer 04 | 28.558-28.785 · 28.632 · +744 · dentro, shift su, staccato | 7.715,50-7.741,75 · 7.729,25 · +2.387 · dentro, shift su, staccato |
| gio 05 | 26.829-27.361 · 26.900 · -4.781 · sotto, shift giu', staccato | 7.388,25-7.537,75 · 7.406 · +1.610 · sotto, shift giu', staccato |
| ven 06 | 27.113-27.306 · 27.175 · -4.837 · sotto, nessuno shift | 7.415,75-7.454,50 · 7.426 · +17.790 · sotto, nessuno shift |

**Tab 3, la seduta** (pannello 14:52; giro NQ 15:01, ES 14:52). La cassa di oggi non e' aperta (`oggi_cassa` nullo).

| | NQZ6 | ESZ6 |
|---|---|---|
| valore London (pannello) | 27.117-27.368, POC 27.325 (14:52); 27.126-27.365, POC 27.325 (15:01), prezzo sotto il POC | non nel pannello (lacuna) |
| struttura H1, ultimi due swing | massimi crescenti, minimi decrescenti (27.378 alle 13, 26.751,25 alle 08) | massimi e minimi crescenti |
| delta orario 13h · 14h | +807 · +884 | +2.554 · +2.459 |
| CVD dalla chiusura di ieri | +4.015 (+1,8%), CVD al 100% del range, prezzo al 95% (14:52); +3.820 (+1,7%), 95% e 90% (15:01) | non nel pannello (lacuna) |
| velocita' (proxy, non la speed of tape) | morta, 0° percentile, barra in formazione (14:52 e 15:01); barre chiuse 14:51 al 59°, 15:00 al 63° | morta 0° (pannello 14:52) o normale 46° (giro, stesso minuto): e' la barra in formazione; barra chiusa 14:51 al 76° |
| big trade (10 barre) | 3 da 60+ lotti, netto +213 (14:52); 0 (15:01) | 0 da 268+ lotti |
| in gioco (pannello) | 14:52 niente, porte 27.368,75 sopra e 27.319 sotto; 15:01 "Max notte 27.319", 5,50 sotto | nessun livello depositato |

**Tab 4, l'esecuzione**: Range non registrata. Nastro M1 (`giro.txt`, barre chiuse). NQ (15:01): 15 minuti +98 (+1,6%) in 27.310,25-27.368,75; 30 minuti +522 (+4,0%); 60 minuti +865 (+3,5%). ES (14:52): 15 minuti -42 (-0,3%) in 7.456,25-7.475; 30 minuti +1.789 (+6,7%); 60 minuti +2.423 (+4,8%). Volume a mezz'ora: NQ 13.436 (13:30), 11.677 (14:00), 12.783 (14:30); ES 31.150, 20.782, 24.280. Alle 12:00 erano NQ 7.949, ES 20.916.

## Cosa Dicono Per NQ (Ed ES)

**Tab 1.** Solo cassa, composito di 90 sedute `[4 · 14:14]`: entrambi in **discount**, sotto il VAL di tutte le misure, "really low in the curve" `[5 · 1:01:05]`. La salita di stamattina (circa l'1%) non cambia la zona: il VAL del composito sta quasi 1.700 punti sopra su NQ e circa 200 su ES. Gli scalini sono quelli di venerdi': si muovono solo a candela chiusa. Mai "allineati, quindi": e' smentito (`quant/studi/scala-dei-tre-tempi`).

**Tab 2.** Giovedi' resta **la seduta che ha creato la rottura**, il suo POC (NQ 26.900, ES 7.406) e' un livello `[1 · 24:35]`. Venerdi' sta dentro giovedi', e i due profili si uniscono `[1 · 24:02]`: blocco NQ 26.829-27.361, ES 7.388,25-7.537,75. **Il fatto nuovo**: il prezzo e' uscito sopra il valore di venerdi' su entrambi, ma resta dentro il blocco di giovedi'. Su NQ il massimo della notte (27.378) ha visitato il VAH di giovedi' (27.361) e il prezzo e' rientrato sotto (27.345 alle 14:52, 27.313,50 alle 15:01). Su ES il VAH di giovedi' (7.537,75) e' lontano 66 punti: ES ha piu' spazio dentro il blocco. Sopra il blocco c'e' il **vuoto** fra il VAH di giovedi' e il VAL di martedi': NQ 27.361-28.296, ES 7.537,75-7.589,75.

**Tab 3.** **Chi guida.** Alle 12:28 era ES: aveva tenuto il minimo della notte e stava al massimo, NQ no. In due ore e mezza NQ ha recuperato: e' salito oltre il massimo delle 03 (27.319) e dalla chiusura di venerdi' ha fatto +1,0% contro +0,949% di ES (stesso istante, 13:52 UTC). La condizione del rapporto delle 12:28 ("NQ sopra 27.319 mentre ES resta sotto 7.459") **non si e' verificata come scritta**: sono saliti insieme. ES resta piu' avanti nella struttura: e' l'unico sopra il massimo di venerdi' (il muro alto del ping-pong), ha massimi e minimi orari crescenti e il POC della notte salito da 7.428,50 a 7.446. NQ ha il POC della notte fermo a 27.100 (sotto il VAL di venerdi'), la struttura oraria che si allarga e il prezzo 80 punti sotto il suo massimo di venerdi'. **Osservazione**: la guida non e' piu' netta; NQ ha corso di piu' da mezzogiorno, ES tiene la struttura migliore.
Il **regime**, come lo misura l'indicatore: alle 14:52, sulle 30 barre 14:23-14:52, **BALANCE** su tutti e due (efficienza 0,17 NQ, 0,18 ES; una e due rotture rientrate). Alle 15:01 NQ e' **CHOPPY** (efficienza 0,03 sulle barre 14:32-15:01: netto 18,25 su 533 punti di strada). Per ES manca una lettura dopo le 14:52. Il CVD di NQ (il delta cumulato dalla chiusura) era al massimo con il prezzo vicino al massimo: "i compratori sono seguiti", dice il pannello alle 14:52 e alle 15:01. La barra in formazione e' "morta" su tutti e due; le barre chiuse sono a ritmo medio-alto (59°-76° percentile).
Le etichette del pannello di NQ "max cash" 27.368,75 e "min cash" 27.268 compaiono prima delle 15:30, e il giro dice che POC, VAL e VAH "cash" hanno 13.178 lotti su 15.000: l'indicatore sta gia' contando una finestra che chiama cassa. Si scrive, non si interpreta (vedi Cosa Non Sappiamo).

**Tab 4.** Non leggibile come tab. Il nastro M1 dice che il delta e' positivo a 30 e 60 minuti su entrambi, piu' forte su ES (+6,7% e +4,8% contro +4,0% e +3,5%); a 15 minuti ES e' piatto (-0,3%, 14:52) e NQ ancora positivo (+1,6%, 15:01), ma i minuti non coincidono. Il volume di NQ per mezz'ora e' salito di circa il 60% rispetto alle 12:00; quello di ES e' alto dalle 13:30. Se il ritmo accelera o si spegne sulle barre range, non si sa.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 27.425 · ES 7.469 | massimo di venerdi' = muro alto del ping-pong (`scalini`) | ES sopra di poco, NQ sotto: una chiusura giornaliera oltre rompe il ping-pong |
| NQ 27.361 · ES 7.537,75 | VAH di giovedi', bordo alto del blocco unito | NQ ci e' passato sopra solo in visita (27.378); accettazione oltre = valore fuori dal blocco della rottura |
| NQ 27.361-28.296 · ES 7.537,75-7.589,75 | vuoto fra VAH gio 05 e VAL mar 03 | zona di transito, non di sosta |
| NQ 27.378 · ES 7.476,50 | massimo della notte (`oggi_notte`) | il confronto di chi guida, dal vivo |
| NQ 27.368,75 / 27.325 | pannello: "max cash" e POC London | le due porte e il centro della sessione europea, solo NQ |
| NQ 27.319 | massimo delle 03 (pannello "max notte", 17 tocchi, 10 tiene, 7 passa) | il livello in gioco alle 15:01: sotto, NQ perde il recupero di stamattina |
| NQ 27.306 · ES 7.454,50 | VAH di venerdi' (`sedute`; pannello NQ 27.300) | sotto = rientro nella zona di ieri |
| NQ 27.248 · ES 7.458,25 | VAH della notte (`oggi_notte`; pannello NQ "VAH notte" 27.189) | bordo alto del valore costruito stanotte |
| NQ 27.175 · ES 7.426 | POC di venerdi' | ritorno al centro = mercato in bilancia sul valore di ieri |
| NQ 27.100 · ES 7.446 | POC della notte | dove la notte ha scambiato di piu': per ES e' salito, per NQ no |
| NQ 26.900 · ES 7.406 | POC di giovedi', la seduta della rottura `[1 · 24:35]` | il primo livello sulla via del ritorno |
| NQ 28.998 · ES 7.676 | VAL del composito di cassa 90 | rientro nel valore dei tre mesi: fine del discount `[1 · 1:19:30]` |
| 15:30 · 22:00 | cassa di New York 09:30-16:00 (14:30-21:00 UTC) | il profilo di oggi nasce alle 15:30; la notte non vi entra |
| domani 14:30 | CPI di ottobre (`giro.txt`, calendario) | la data la conferma la fase macro |

## Cosa Non Sappiamo

- **lacuna del collettore**: la tab 4 (Range 40) non e' registrata nel bridge, `barre_range` vuoto su entrambi: niente verso, durata e delta delle barre range.
- **lacuna del collettore**: il pannello di ES non ha livelli depositati (0 regole), ne' riga del CVD, del valore London o del livello in gioco.
- **lacuna del collettore**: i giri di NQ (15:01) ed ES (14:52) non sono allo stesso minuto: il confronto di chi guida dopo le 14:52 manca per ES.
- **lacuna del collettore**: il pannello di NQ chiama "cash" ("max cash" 27.368,75, "min cash" 27.268, POC/VAL/VAH cash a 13.178 lotti) una finestra che inizia prima delle 09:30 di New York; il file non dice da quando conta. Da chiarire nell'indicatore, non qui.
- **lacuna del collettore**: il composito di cassa 90 non e' pieno (NQ 47 sedute, di cui 6 prima del rollover senza dire se aggiustate; ES 26).
- Il pannello chiama "notte" una finestra diversa da `oggi_notte` (VAH 27.189 contro 27.248, VAL 26.923 contro 26.928): vale il pannello per il vivo, la differenza resta scritta.
- La riga del pannello sul regime contiene un'indicazione operativa (dimensione della posizione): questo rapporto non la riporta.
- Il delta di NQ ed ES non si confronta in contratti: si confrontano il segno e i percenti del collettore.

## Per Il Quadro

1. **Chi guida, aggiornato**: NQ ha recuperato (+1,0% contro +0,949% dalla chiusura, 13:52 UTC) ma solo ES e' sopra il massimo di venerdi'. Confrontare con la GEX viva di NQ (zero gamma 0DTE 27.230 alle 14:51, prezzo sopra; muri 27.500 a circa 150 punti) e con X-Ray su ES (acceleratore, il piu' forte a 7.346, lontano dal prezzo).
2. **Il VAH della notte di NQ 27.248** sta a 2 punti dallo strike 27.250 (GEX netta -20,8, `confluenze`, non lo stesso dato): un bordo di valore e uno strike vicini, da guardare in cassa.
3. **Il regime dell'indicatore** (BALANCE alle 14:52 su entrambi, CHOPPY su NQ alle 15:01) contro l'"acceleratore" delle opzioni: due misure diverse dello stesso mercato; il quadro le mette a confronto, non le somma.
4. **Il delta di ES positivo anche stanotte** (+6.790), dopo +1.610 giovedi' e +17.790 venerdi' in cassa: oggi, per la prima volta in tre sedute, il prezzo lo segue. Da confrontare con il COT del 3/11 (non commerciali di ES netti corti) e con il delta per gruppo di X-Ray.
