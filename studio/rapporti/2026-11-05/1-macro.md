# Fase 1 · La Macro · 5 novembre 2026

**Stato**: parziale (manca la causa dichiarata del crollo delle 15:30, manca l'ISM servizi di ieri)
**Dati**: `macro.json`, `ore-NQZ6.json`, `raccolta.json`; raccolta delle 16:16 italiane, nessun errore. Yahoo
16:00-16:16 (di oggi); curva Treasury al 4/11; FRED al 3-4/11, sussidi al 31/10, CPI e PPI a settembre; notizie
fino alle 16:09; ultima barra di NQ aperta (17 min); `2YY=F` fermo al 13/10, non usato. ISM servizi: 2 ricerche web, nulla.

**In una riga**: tassi corti verso un rialzo della Fed e lunghi in discesa, volatilita' del Nasdaq al 94esimo
percentile e in salita veloce, dollaro forte, credito debole: rischio concentrato su chip e intelligenza artificiale.

## In Parole Semplici

**Il punto**: oggi crollano i grandi titoli tecnologici del Nasdaq, non la Borsa intera; chip e intelligenza
artificiale scendono da due settimane. Nei dati nessuna notizia dichiara la causa del crollo delle 15:30.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: la Fed e' attesa alzare i tassi il 9 dicembre (circa 8 probabilita' su 10).
   → il denaro piu' caro pesa di piu' sui titoli che valgono per gli utili futuri, come i tech.
2. **La paura**: l'indice della paura sul Nasdaq e' a 30, piu' alto che in 94 giorni su 100 dell'anno.
   → il mercato si aspetta movimenti ampi; la paura a pochi giorni e' salita piu' di quella lontana.
3. **Chi scende**: chip -7,7%, Nasdaq -4%, S&P -2%, il titolo medio dell'S&P solo -0,7%.
   → e' una vendita sui grandi nomi dell'intelligenza artificiale, non una fuga da tutte le azioni.
4. **Le notizie**: vittoria democratica alle elezioni, dubbi sui data center, due membri della Fed per rialzi.
   → sono lo sfondo; nessuna nomina il crollo delle 15:30, che coincide con l'apertura della cassa.
5. **Domani**: alle 14:30 il rapporto sul lavoro di ottobre.
   → e' il dato che puo' cambiare le attese sul rialzo di dicembre.

**Quando**: domani 14:30 occupati e disoccupazione · 16:00 fiducia dei consumatori · 9 dicembre la Fed
**Quanto e' provato**: i numeri sono misurati (fonti sotto); il legame fra rialzo dei tassi e calo dei tech e'
un modello di manuale, non provato su NQ; la causa del crollo di oggi non la sappiamo.

**Le parole**: **cassa** = la seduta di Wall Street, 15:30-22:00 italiane; **indice della paura sul Nasdaq**
(VXN) = la volatilita' attesa a 30 giorni dalle opzioni; **percentile** = quanti giorni dell'anno stavano sotto.

## I Fatti

**Tassi** (curva par, home.treasury.gov, chiusura del 4/11; oggi solo il 10 anni, Yahoo `^TNX` 16:01)

| | 3M | 2A | 10A | 30A | 10A-2A | 10A-3M |
|---|---|---|---|---|---|---|
| 4/11 | 4,21 | 4,58 | 4,90 | 5,28 | +0,32 | +0,69 |
| 6/10 (20 sedute fa) | 4,21 | 4,79 | 5,27 | 5,64 | +0,48 | +1,06 |

- 10 anni oggi 4,868% (-2,6 punti base). In 20 sedute 2A -21 pb, 10A -37 pb: curva giu' e piu' piatta.
  Fed funds effettivo 3,88% (FRED DFF, 3/11); inflazione attesa a 10 anni 2,26% (FRED, 4/11, -4 pb).
- Fed del 9/12: rialzo di 25 pb all'82,7% (future ZQ, Yahoo 16:05, metodo FedWatch in `fomc_future`);
  Polymarket rialzo 73,5%, fermo 24,5%. Inflazione (FRED, settembre): CPI +3,72% annuo, PPI +6,35%.

**Volatilita'** (Yahoo 16:00-16:01; percentile a un anno su chiusure giornaliere)

| | oggi | chiusura prima | percentile 1a |
|---|---|---|---|
| VXN | 30,32 | 25,08 | 94,4 (max 1a 33,54) |
| VIX | 20,39 | 16,61 | 83,5 |
| VIX9D / VIX3M | 20,90 / 21,05 | 15,19 / 18,80 | 85,3 / 73,0 |

- VIX9D sotto VIX3M di 0,15: contango (paura lontana sopra la vicina) quasi piatto; ieri 3,6. Realizzata
  di NQZ6 (bridge, rendimenti a 5 minuti annualizzati): 4 ore 49,3%, 24 ore 36,9%, 48 ore 34,2%.

**Dollaro, petrolio, oro, credito, sentiment** (Yahoo 16:06-16:16, 1 giorno; pct = percentile a un anno):
DXY 101,61 +0,1% (pct 90,5); WTI 90,11 +2,3% (5 giorni -11,7%); oro 4.116 +1,1% (pct 17,9); HYG 77,27 -0,3%
(pct 7,9); TLT 81,84 +0,1%. Spread high yield 3,32 punti % (FRED 4/11, -8 pb; pct a 2 anni 92,2). Stress
St. Louis -0,52 (30/10). Fear & Greed 27,2, paura (CNN; una settimana fa 30,8, un mese fa 43,1).

**Chi guida** (Yahoo 16:06-16:16; ampiezza api.nasdaq.com alle 16:15)

| | 1 giorno | 5 giorni | 20 giorni |
|---|---|---|---|
| NQ=F 27.415 | -4,03% | -7,26% | -11,48% |
| ES=F 7.569 | -1,96% | -2,39% | -3,16% |
| RSP (S&P equipesato, pct 97,6) | -0,67% | +0,81% | +4,70% |
| SMH (chip) | -7,67% | -17,10% | -23,33% |

- Le grandi: TSLA -8,9%, NVDA -6,7%, AVGO -6,5% (pct 1,2), META -4,9%, AMZN -2,5%, MSFT -2,1%, GOOGL
  -1,6%, AAPL +0,1% (massimo dell'anno); SPCX -9,2% (peso 3,9%). XLK -5,4%.
- Nasdaq-100: 38 su, 63 giu'; media -3,13%, mediana -1,88%, pesata -3,7%. NVDA riporta il 17/11 sera.

**Calendario** (Forex Factory, ora italiana; esiti FRED; NQZ6 dal bridge)

| quando | evento | attesa | uscito | NQZ6 nell'ora |
|---|---|---|---|---|
| 4/11 16:00 | ISM servizi | 54,8 | non trovato | 28.818 -> 28.629 |
| 5/11 14:30 | sussidi di disoccupazione | 197 mila | 189 mila | 28.327 -> 28.308 |
| 6/11 14:30 | occupati non agricoli; disoccupazione; salari m/m | 65 mila; 4,2%; 0,3% | | |
| 6/11 16:00 | fiducia Michigan; attese d'inflazione | 48,1; prec. 4,7% | | |

**NQZ6 ora per ora** (bridge): notte del voto 4/11 da 28.512 a 27.615,75 (ore 0-4), poi fino a 28.969
(ore 16); chiusura di cassa ieri 28.566,75. Ora 15 di oggi: 28.308,5 -> minimo 27.457,5, chiusura
27.533,5, volume 196.124 (massimo delle 48 ore), delta -2.448. Ora 16 (17 min): minimo 27.303.

**Le notizie** (ora del feed; le date di Google News non sono sempre affidabili)

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 10:47 | WSJ: vendite sull'AI continuano, Treasury su, "blue wave" | +62 |
| 12:01-14:31 | CNBC su elezioni e data center; Barchart, TradingView: Nasdaq giu' sui chip | ora 13: -115 |
| 14:55 | Reuters: Hammack (Fed) per rialzi graduali | ora 14: -19 |
| 15:29 | CNBC: Barkin (Fed) vede rialzi graduali | ora 15: -775 |

- Il crollo coincide con l'apertura della cassa e con il titolo su Barkin; nessuna notizia ne nomina la causa.
- ABC 4/11 20:23 "Fed raises rates": **non coerente** con fed funds 3,88%, future e WSJ ("Rate Hold"). Non verificato.

## Cosa Dicono Per NQ

- **Vendita concentrata**: NQ -4,0% contro RSP -0,7%; la mediana del Nasdaq-100 (-1,9%) e' meta' della
  pesata (-3,7%). Trascinano chip e AI; la rotazione dura da 20 sedute (RSP +4,7%, NQ -11,5%). **Osservazione**.
- **I tassi non spiegano da soli**: il 10 anni scende mentre NQ crolla; sale l'attesa sul tasso corto. Il
  canale "Fed piu' dura, tech giu'" e' coerente con Hammack e Barkin, ma e' un modello, non misurato su NQ.
- **La volatilita' corre**: VXN +5,2 punti, VIX9D +5,7, struttura quasi piatta ma non invertita; la
  realizzata a 4 ore (49%) supera l'implicita (30%). **Osservazione**.
- **Credito e dollaro non segnano panico generale**: spread HY in calo ieri, HYG -0,3%, DXY fermo, oro +1%.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28.566,75 | chiusura di cassa del 4/11 (bridge) | misura lo scarto di oggi da ieri |
| 27.615,75 | minimo della notte del voto (bridge) | rotto oggi; posizionamento delle 48 ore |
| 6/11 14:30 | occupati, disoccupazione, salari | muove le attese sul 9 dicembre |
| 9/12; 17/11 sera | Fed (rialzo 82,7%); trimestrale NVDA | i due appuntamenti di peso |

## Cosa Non Sappiamo

- La causa dichiarata del crollo delle 15:30 (la skill non prevede ricerche web sulle notizie).
  **lacuna del collettore: un feed d'agenzia con l'ora al minuto, e notizie dopo le 16:09**.
- **lacuna del collettore: l'ISM servizi del 4/11** (non e' in FRED) e **l'esito delle trimestrali uscite** (QCOM).
- **lacuna del collettore: le decisioni Fed passate** (per verificare titoli come ABC) e la curva di oggi
  oltre il 10 anni; `2YY=F` fermo al 13/10. SPCX non ha nome per esteso nei dati.

## Per Il Quadro

- VXN al 94esimo percentile e realizzata al 49%: coerente con la gamma (fase 3)? In gamma negativa, in teoria, il movimento si allunga.
- RSP al 97,6 percentile, NQ -11,5% in 20 sedute: le posizioni (fase 2) sono ancora lunghe sul Nasdaq?
- Ora 15: volume 196 mila e delta -2.448; cosa dice l'order flow (fase 4) sotto 27.457?
