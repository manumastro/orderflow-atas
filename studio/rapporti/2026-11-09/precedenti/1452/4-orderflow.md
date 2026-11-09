# Fase 4 · L'Order Flow Delle Quattro Tab, NQ Ed ES Insieme · 9 novembre 2026

**Stato**: parziale (manca la tab 4: la Range non e' registrata nel bridge, `range-*.json` vuoti; il pannello di ES non ha livelli ne' CVD).
**Dati**: `dati/` e `dati-ES/`: `in-breve.json` (sezione `orderflow`), `pannello.json`, `quattro-tab.txt`, `giro.txt`, due ore di `ore-*.json`. Raccolta 12:28 ora italiana (`raccolta.json`, errori: nessuno); pannelli alle 12:28 (11:28 UTC); il giro un minuto dopo. Prezzi di ciascuno nei suoi punti, mai convertiti.

**In una riga**: NQ ed ES stanno in sconto sotto il valore dei tre mesi, dentro il valore stretto di venerdi'; stanotte ES ha tenuto e NQ no, e l'indicatore misura un mercato a zig-zag su tutti e due.

## In Parole Semplici

**Il punto**: tutti e due sono molto sotto la zona dove si e' scambiato negli ultimi mesi, e fermi dentro la zona di venerdi'. Stamattina ES e' il piu' forte: ha tenuto il suo minimo della notte e sta al massimo; NQ ha fatto un minimo nuovo alle 08 e resta sotto il suo massimo. L'indicatore, su entrambi, dice "a zig-zag".

**Dal piu' ampio al piu' vicino**

1. **Le settimane**: NQ ed ES sono sotto il valore di tre mesi; la settimana scorsa ha sceso uno scalino, per tutti e due.
   → Siamo in basso nella curva. NQ e' piu' in basso di ES: ottobre ha chiuso sotto il minimo di settembre solo su NQ.
2. **I giorni**: giovedi' ha rotto verso il basso; venerdi' ha costruito valore dentro quello di giovedi' e ha chiuso sotto.
   → Una pausa dopo la rottura. Su ES i compratori aggressivi hanno spinto molto venerdi' senza ottenere il prezzo; su NQ hanno spinto i venditori.
3. **La notte**: alle 08 NQ e' sceso a un minimo nuovo, ES no; poi entrambi sono risaliti sopra la zona della notte.
   → ES guida, NQ segue. E' un'osservazione di una notte, non una regola.
4. **Adesso**: l'indicatore misura "choppy" su entrambi: molta strada, poco cammino netto, rotture che rientrano.
   → NQ si muove a ritmo normale, ES quasi fermo. La tab piu' vicina (Range) non e' leggibile oggi.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| NQ 27.306 · ES 7.454,50 | il bordo alto della zona di venerdi' | ES e' gia' li' sotto (3,5 punti); accettare sopra vuol dire uscire dalla pausa verso l'alto |
| NQ 27.425 · ES 7.469 | il massimo di venerdi', il muro alto del ping-pong | una giornata che chiude oltre rompe il ping-pong |
| NQ 26.900 · ES 7.406 | il prezzo piu' scambiato di giovedi', il giorno della rottura | il primo punto che il mercato ritrova sulla via del ritorno |

**Quando**: 15:30 apre la cassa di New York (09:30 NY) · 22:00 chiude. Il calendario degli eventi e' vuoto: la data la da' la fase macro.
**Cambia idea se**: NQ supera il suo massimo della notte (27.319) mentre ES resta sotto il suo (7.459): la guida passa a NQ. O ES torna sotto il bordo basso di venerdi' (7.415,75).
**Quanto e' provato**: valori, delta e regime sono misurati dal collettore e dall'indicatore; chi guida e' un'osservazione; gli scalini sono smentiti come previsione su NQ 2017-2024; nessuno studio prova che una divergenza fra NQ ed ES dica il verso.

**Le parole**: **valore** = la fascia dove si e' scambiata la maggior parte del volume; **prezzo piu' scambiato** (POC) = il prezzo con piu' volume; **sconto** (discount) = sotto il valore; **delta** = acquisti aggressivi meno vendite aggressive; **choppy** = a zig-zag; **ping-pong** = il prezzo rimbalza fra due muri dopo uno scalino; **scalino** = candela con massimo, minimo e chiusura oltre la precedente.

## I Fatti

**Tab 1, composito.** Fonte `in-breve.json` `orderflow`; prezzo alle 12:28: NQ 27.211,50, ES 7.451,00.

| | NQZ6 | ESZ6 |
|---|---|---|
| composito di cassa 90 (sedute presenti) | 28.998-31.080, POC 29.735 (47, dal 03/09) | 7.676-7.862, POC 7.754 (26, dal 02/10) |
| composito di cassa 20 (dal 12/10) | 28.686-31.048, POC 29.840 | 7.631-7.810,75, POC 7.754 |
| composito della tab 1, 90 barre daily di 24 ore, grezzo | 28.270-30.255, POC 29.517,50 | 7.615-7.860, POC 7.767,50 |
| idem aggiustato al rollover (prima dell'11/09) | +299,25: 28.275-30.285, POC 29.817,50 | +67,75: 7.650-7.855, POC 7.762,50 |
| zona (tutti e quattro) | discount | discount |
| posizione nel range delle ultime 20 sedute (`giro.txt`) | 18% | 23% |

**Gli scalini** (ultima candela chiusa; convenzione in `scalini`: massimo, minimo **e** chiusura oltre la candela prima; daily di 24 ore):

| scala | NQZ6 | ESZ6 |
|---|---|---|
| mese (ottobre su settembre) | pausa: massimo piu' alto ma chiude sotto (visitato, non accettato), ingloba; chiusura 28.543 sotto il minimo di settembre 29.040,50 | pausa: massimo piu' alto e minimo piu' basso, entrambi non accettati; chiusura 7.614,50 dentro settembre |
| settimana 02-06/11 | scalino giu' (28.969-26.366,50, chiude 27.107,50) | scalino giu' (7.746,25-7.322,25, chiude 7.404) |
| giorno 06/11 | pausa: minimo piu' basso, chiude sopra il minimo di giovedi' | pausa: idem |
| insieme · ping-pong | misti · dal 06/11, 1 giornata: 26.366,50-27.425 | misti · dal 06/11, 1 giornata: 7.322,25-7.469 |

**Tab 2, la settimana** (valore di cassa per seduta, `quattro-tab.txt`, `ultime_5_sedute`):

| seduta | NQ valore · POC · delta cassa · chiude | ES valore · POC · delta cassa · chiude |
|---|---|---|
| lun 02 | 28.340-28.891 · 28.695 · -7.202 · sotto | 7.596-7.674 · 7.657 · +3.502 · sotto |
| mar 03 | 28.296-28.387 · 28.314 · -5.843 · dentro, shift giu' | 7.589,75-7.609,25 · 7.597 · -3.521 · dentro, shift giu' |
| mer 04 | 28.558-28.785 · 28.632 · +744 · dentro, shift su, staccato | 7.715,50-7.741,75 · 7.729,25 · +2.387 · dentro, shift su, staccato |
| gio 05 | 26.829-27.361 · 26.900 · -4.781 · sotto, shift giu', staccato | 7.388,25-7.537,75 · 7.406 · +1.610 · sotto, shift giu', staccato |
| ven 06 | 27.113-27.306 · 27.175 · -4.837 · sotto, nessuno shift | 7.415,75-7.454,50 · 7.426 · +17.790 · sotto, nessuno shift |

**Tab 3, la seduta.** Notte (dalle 00:00 alle 12:28 ora italiana, `oggi_notte`); cassa di oggi non ancora aperta (`oggi_cassa` nullo).

| | NQZ6 | ESZ6 |
|---|---|---|
| notte: apertura, massimo, minimo | 26.878,50 · 27.319 (ora delle 03) · 26.751,25 (ora delle 08) | 7.371 · 7.459 (ultimi 30 minuti) · 7.363,75 (ora delle 00) |
| notte: valore, POC, delta su volume | 26.928-27.184, POC 27.100 · +2.396 su 172.276 | 7.392,75-7.436, POC 7.428,50 · +1.935 su 242.645 |
| ora delle 08 (`ore-*.json`) | 27.058,75 -> 26.817, minimo 26.751,25, delta -141 | 7.422,50 -> 7.391,25, minimo 7.385,75, delta +1.701 |
| struttura H1, ultimi due swing (`giro.txt`) | massimi crescenti, minimi decrescenti | massimi e minimi crescenti |
| dalla chiusura di venerdi' (BDLM) | +136 punti, +0,50% | +51 punti, +0,69% |
| volatilita' realizzata 4h · 24h · 20 giornate | 36,6 · 30,3 · 34,4 | 18,1 · 15,6 · 21,4 |
| regime (pannello, 30 barre 11:59-12:28) | CHOPPY: efficienza 0,09 (netto 64 su strada 732), 4 rotture rientrate | CHOPPY: efficienza 0,10 (netto 11,50 su strada 114,75), 1 rottura rientrata |
| velocita' (proxy, non la speed of tape) | normale, 41° percentile su 60 barre; barra chiusa 12:27 al 73° | morta, 7° percentile; barra chiusa 12:27 al 68° |
| CVD dalla chiusura di ieri | +2.395 (+1,4%), CVD al 95% del suo range, prezzo all'81% | non nel pannello (lacuna) |
| veti | 1: regime choppy | non nel pannello |

**Tab 4, l'esecuzione**: la Range 40 non e' registrata nel bridge (`quattro-tab.txt`, `barre_range` vuoto). Al suo posto, il nastro M1 di `giro.txt` (barre chiuse): 30 minuti NQ +231 (+2,9%), ES +1.024 (+4,8%); 15 minuti NQ -31 (-0,6%), ES +415 (+3,3%). Mezz'ora delle 12:00: NQ volume 7.949 contro 3.845-4.860 prima; ES 20.916 contro 6.930-7.660.

## Cosa Dicono Per NQ (Ed ES)

**Tab 1.** Solo cassa, composito di 90 sedute (`i-tre-modelli-del-live-q1.md`, sezione 10, `[4 · 14:14]`): NQ ed ES sono entrambi in **discount**, sotto il VAL (il bordo basso del valore) di tutte e quattro le misure. Il live dice che li' si e' "really low in the curve" `[5 · 1:01:05]`. Il VAL del composito e' stato rotto dal basso: "il VAL e' una strada" `[1 · 1:19:30]`. Fin qui NQ ed ES **concordano**. Divergono nella profondita': NQ sta al 18% del range di 20 sedute, ES al 23%; e solo NQ ha chiuso ottobre sotto il minimo di settembre, cioe' ha **accettato** prezzi piu' bassi su scala mensile, mentre ES li ha solo **visitati**. Gli scalini sono identici nella forma: settimana giu', giorno in pausa (il minimo di venerdi' visitato, non accettato), ping-pong da una giornata fra i muri di venerdi'. Mai "allineati, quindi": e' smentito (`quant/studi/scala-dei-tre-tempi`).

**Tab 2.** Giovedi' 05 e' **la seduta che ha creato la rottura** (staccata, shift giu' su entrambi): il suo POC, NQ 26.900 ed ES 7.406, resta un livello `[1 · 24:35]`. Venerdi' ha il valore dentro quello di giovedi' (VAL piu' alto, VAH piu' basso: nessuno shift), quindi i due profili si **uniscono** `[1 · 24:02]`: blocco NQ 26.829-27.361, blocco ES 7.388,25-7.537,75. Entrambi venerdi' chiudono **sotto** il proprio valore: concordano. Il delta di cassa **diverge**: NQ -4.837, ES +17.790. Anche giovedi', mentre ES perdeva 326 punti, il suo delta di cassa era +1.610, quello di NQ -4.781. **Osservazione**: su ES chi comprava aggressivo non ha ottenuto il prezzo, due giorni di fila; su NQ i venditori aggressivi si'. La scala dei POC sopra il prezzo comincia lontano: NQ 28.314 (mar), 28.632 (mer), 28.695 (lun); ES 7.597 (mar), 7.657 (lun), 7.729,25 (mer). Il **vuoto** sta fra il VAH di giovedi' e il VAL di martedi': NQ 27.361-28.296, ES 7.537,75-7.589,75. Su ES e' piu' corto in rapporto al blocco, perche' giovedi' ES ha costruito valore piu' in alto dentro la caduta.

**Tab 3.** Il prezzo e' **dentro** il valore di venerdi' e **sopra** il suo POC su entrambi, e sopra il valore della notte. Pero' ES sta 3,5 punti sotto il suo VAH di venerdi', ha fatto il massimo della notte negli ultimi 30 minuti e ha il POC della notte (7.428,50) dentro la parte alta del valore di venerdi'. NQ sta 95 punti sotto il suo VAH, 108 sotto il massimo della notte delle 03, e il POC della notte (27.100) e' sotto il VAL di venerdi'. Alle 08 la divergenza e' netta: NQ scende di oltre 300 punti e rompe il minimo delle 00 (26.816), con delta -141; ES scende di circa 37, tiene il minimo della notte e ha delta +1.701. Lo dice anche la struttura oraria: NQ si allarga (massimi su, minimi giu'), ES sale (massimi e minimi su). **Osservazione**: oggi ES guida e NQ segue. Il **regime**, come lo misura l'indicatore sulle 30 barre 11:59-12:28, e' **CHOPPY** su tutti e due (efficienza 0,09 e 0,10: un decimo della strada diventa cammino). Concordano. NQ ha 4 rotture rientrate, ES una. Il CVD di NQ (il delta cumulato) e' al 95% del suo range con il prezzo all'81%: il delta corre davanti al prezzo. La volatilita' realizzata sale nelle ultime 4 ore su entrambi (NQ 36,6 contro 30,3; ES 18,1 contro 15,6). Il giro, un minuto dopo il pannello, legge NQ a efficienza 0,07 e velocita' "alta" all'80° percentile: e' la barra in formazione, che cambia da sola.

**Tab 4.** Non leggibile come tab. Il nastro M1 dice solo che nell'ultima mezz'ora il delta e' positivo su entrambi, piu' forte su ES (+4,8% contro +2,9%), e che a 15 minuti NQ e' gia' piatto (-0,6%) mentre ES resta positivo. Il volume della mezz'ora delle 12:00 si e' quasi triplicato su ES. Se il ritmo accelera o si spegne sulle barre range, oggi non si sa.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| NQ 27.425 · ES 7.469 | massimo di venerdi' = muro alto del ping-pong (`scalini`) | una chiusura giornaliera oltre rompe il ping-pong: lo scalino giu' della settimana smette di essere l'ultimo fatto |
| NQ 27.361 · ES 7.537,75 | VAH di giovedi', bordo alto del blocco unito | accettazione oltre = il valore esce dal blocco della rottura |
| NQ 27.361-28.296 · ES 7.537,75-7.589,75 | vuoto fra VAH gio 05 e VAL mar 03 | zona di transito, non di sosta |
| NQ 27.319 · ES 7.459 | massimo della notte | il confronto di chi guida: ES ci e' sopra o vicino, NQ 108 sotto |
| NQ 27.306 · ES 7.454,50 | VAH di venerdi' (`sedute`; pannello NQ: 27.300) | sopra = fuori dal valore di ieri verso l'alto |
| NQ 27.175 · ES 7.426 | POC di venerdi' (pannello NQ: 27.160) | ritorno al centro = mercato in bilancia |
| NQ 27.113 · ES 7.415,75 | VAL di venerdi' (pannello NQ: 27.114) | sotto = di nuovo fuori dal valore di ieri, verso il basso |
| NQ 26.900 · ES 7.406 | POC di giovedi', la seduta della rottura | il primo livello sulla via del ritorno `[1 · 24:35]` |
| NQ 26.751,25 · ES 7.363,75 | minimo della notte (NQ alle 08, ES alle 00) | rotto su NQ stanotte, non su ES: la divergenza |
| NQ 26.366,50 · ES 7.322,25 | minimo di venerdi' (24 ore) = muro basso del ping-pong | chiusura sotto = rottura del ping-pong in basso |
| NQ 28.314 · ES 7.597 | POC di martedi' 03, primo POC sopra; la data del report COT | stesso dato nelle confluenze (`stesso_dato`): non e' una conferma |
| NQ 28.998 · ES 7.676 | VAL del composito di cassa 90 | rientro nel valore dei tre mesi: fine del discount |
| 15:30 · 22:00 | cassa di New York 09:30-16:00 (14:30-21:00 UTC) | il profilo di oggi nasce alle 15:30; la notte non vi entra |

## Cosa Non Sappiamo

- **lacuna del collettore**: la tab 4 (Range 40) non e' registrata nel bridge, e `range-NQZ6.json` e `range-ESZ6.json` sono vuoti: niente verso, durata e delta delle barre range, niente metro di "veloce" o "lenta".
- **lacuna del collettore**: il pannello di ES non ha livelli depositati (0 regole) ne' riga del CVD, valore o veti: per ES il CVD dalla chiusura e il livello in gioco mancano.
- **lacuna del collettore**: il composito di cassa 90 non e' pieno (NQ 47 sedute, ES 26); per NQ 6 sedute sono prima del rollover e il file non dice se sono aggiustate.
- **lacuna del collettore**: la distanza dal VAL del composito non e' data in percento, quindi "NQ piu' in profondita' di ES" poggia sul 18% contro 23% del range di 20 sedute, non su una misura di sconto.
- Il composito della tab 1 e gli scalini usano barre di 24 ore, che il framing del live esclude (solo cassa): per questo il composito di cassa e' scritto per primo.
- Il delta di NQ ed ES non si confronta in contratti (volumi diversi): si confrontano il segno e i percenti dati dal collettore.
- Il pannello di NQ da' livelli un po' diversi da `sedute` (POC cassa ieri 27.160 contro 27.175, VAH 27.300 contro 27.306, VAH notte 27.189 contro 27.184): vale il pannello dal vivo, `sedute` per la seduta chiusa; la differenza resta scritta.
- Calendario degli eventi vuoto (`giro.txt`): non va aggiornato qui.

## Per Il Quadro

1. **Divergenza NQ-ES stanotte** (ES tiene il minimo e fa il massimo, NQ rompe il minimo alle 08): confrontare con X-Ray in prezzi di ES (acceleratore, il piu' forte a 7.343,50, sotto il minimo della notte 7.363,75) e con la GEX di NQ (put wall 0DTE 27.000, zero gamma 0DTE 27.236,41, appena sopra il prezzo).
2. **Il delta di cassa di ES positivo in due cadute** (+1.610 giovedi', +17.790 venerdi', chiusure sotto il valore): coerente con chi copre opzioni su SPX, o con il posizionamento del COT su ES?
3. **Il discount profondo** (NQ sotto 28.998, ES sotto 7.676; ottobre accettato sotto settembre solo su NQ): confrontare con il COT del report del 3/11, la cui settimana ha POC 28.314 su NQ e 7.753,75 su ES (confluenze).
4. **Il regime CHOPPY dell'indicatore su entrambi** contro l'"acceleratore" delle opzioni: due misure diverse dello stesso pomeriggio; il quadro le mette a confronto, non le somma.
