# Fase 2 · Il COT · 2026-10-15

**Stato**: fatto (con le lacune del collettore elencate in fondo)
**Dati**: `dati/cot.json` (raccolto 10:45 ora italiana), `dati/sedute-NQZ6.json`, `dati/raccolta.json`
(raccolta chiusa alle 10:51 ora italiana, tutte le voci "ok"). COT al **martedi' 6 ottobre 2026**,
pubblicato venerdi' 9 ottobre: **9 giorni**. Errori della raccolta: nessuno.

**In una riga**: i Non-Commercial del Nasdaq hanno il netto lungo piu' alto di tre anni e gli short
piu' bassi; l'ultima variazione e' ordinaria, la piu' grande delle ultime quattro e' del 22 settembre,
settimana di scadenza; da martedi' 6 ottobre il prezzo ha perso circa il 5%, tutto dopo la fotografia.

## I Fatti

Fonte: CFTC, API pubblica, **Legacy, futures only** (la vista di Tradingster che Fabio apre
`[1 · 27:46]`), solo **Non-Commercial** (gli speculatori: fondi, asset manager, leveraged money
`[1 · 34:51]`). Report del 6 ottobre 2026. "Netto" = long meno short.

**Passi 1-2. La tabella dei quattro indici**

| indice | long | short | netto | var. long | var. short | var. netto | pct netto 3a | pct long 3a | pct short 3a |
|---|---|---|---|---|---|---|---|---|---|
| Nasdaq-100 mini | 86.745 | 28.622 | +58.123 | +5.315 | -1.561 | +6.876 | **100,0** | 73,1 | **0,6** |
| S&P 500 e-mini | 212.009 | 366.079 | -154.070 | +2.422 | +13.993 | -11.571 | 25,0 | 5,1 | 61,5 |
| Dow x5 | 23.625 | 14.134 | +9.491 | +1.135 | +1.488 | -353 | 66,9 | 72,4 | 60,3 |
| Russell 2000 mini | 87.450 | 175.295 | -87.845 | -1.451 | +8.954 | -10.405 | **1,9** | 91,7 | **100,0** |

"pct 3a" = percentile dentro le ultime tre anni di settimane (il valore di oggi contro la propria
storia: 100 = il piu' alto). Il netto del Nasdaq coincide con il massimo a tre anni (intervallo
-39.302 / +58.123). Commercial netto Nasdaq: -68.446. Open interest (contratti aperti) Nasdaq
283.586, +4,8% nella settimana.

**Passo 3. Lo skew di fondo** = (long - short) / (long + short), quanto il libro pende da un lato.

| Nasdaq | S&P 500 | Dow | Russell |
|---|---|---|---|
| +0,504 | -0,267 | +0,251 | -0,334 |

**Passo 4. L'ultima variazione e le quattro settimane, Nasdaq**

Skew della variazione = var. netto / (|var. long| + |var. short|), soglia **0,30** (convenzione nostra,
da `build_cot_cross_index.py`; Fabio dice "delta del 30%" `[1 · 33:12]`). z52 = z della **variazione**
del netto contro le 52 settimane precedenti (non lo z del netto: e' un'altra convenzione da quella di
`analisi-istituzionale.md`). pct |var.| = percentile della variazione assoluta su tre anni.

| report (martedi') | netto | var. long | var. short | var. netto | skew | z52 | pct abs var. | var. OI |
|---|---|---|---|---|---|---|---|---|
| 6 ott | 58.123 | +5.315 | -1.561 | +6.876 | +1,00 | 0,58 | 59,0 | +4,8% |
| 29 set | 51.247 | -7.077 | -2.174 | -4.903 | -0,53 | -0,47 | 44,9 | -5,5% |
| **22 set** | 56.150 | +3.445 | -18.987 | **+22.432** | +1,00 | **1,96** | **98,1** | **-12,1%** |
| 15 set | 33.718 | -865 | -13.688 | +12.823 | +0,88 | 1,15 | 85,3 | +10,4% |

Ultima variazione degli altri indici: S&P skew -0,705, z -0,31 (venditori); Russell skew -1,00,
z -0,91 (venditori); Dow skew -0,135, z -0,22 (nessuno, sotto 0,30).

**Passo 5. Il cross-confirm** (segno della var. netto): Nasdaq +, S&P -, Dow -, Russell -.
Concordi col Nasdaq: **1 su 4** (il Nasdaq stesso).

**Passo 6. La data sul prezzo.** NQZ6, cassa 13:30Z-20:00Z = **15:30-22:00 ora italiana**, fasce da 1
punto, valore al 70% dal POC (fonte: `cot.json` -> `prezzo_alle_date`, dal bridge).

| martedi' | apertura | chiusura | POC | VAL | VAH | delta | chiude |
|---|---|---|---|---|---|---|---|
| 6 ott | 31.513,25 | 31.480,75 | 31.530 | 31.483 | 31.556 | -3.557 | sotto il valore |
| 29 set | 30.708,75 | 30.617,5 | 30.609 | 30.557 | 30.650 | +1.378 | dentro |
| 22 set | 30.764,25 | 31.029,25 | 30.922 | 30.888 | 30.980 | +2.427 | sopra il valore |
| 15 set | 29.420,75 | 29.251,75 | 29.250 | 29.215 | 29.315 | +5.358 | dentro |

POC = prezzo con piu' volume nella seduta; VAL e VAH = bordi basso e alto dell'area di valore
(il 70% del volume attorno al POC); delta = acquisti aggressivi meno vendite aggressive.

**Il prezzo adesso.** `sedute-NQZ6.json` -> `oggi_finora`: ultimo 29.822,5, minimo 29.754, alla
raccolta delle 10:51 ora italiana. **Non e' cassa** (la cassa apre alle 15:30 ora italiana).

**TFF Nasdaq, complemento** (Traders in Financial Futures, stessa data, categorie diverse):

| categoria | long | short | netto | var. netto | pct netto 3a |
|---|---|---|---|---|---|
| Dealer | 48.831 | 121.393 | -72.562 | -14.320 | 25,6 |
| Asset Manager | 111.727 | 37.929 | +73.798 | +4.054 | 47,4 |
| Leveraged Funds | 51.349 | 70.561 | -19.212 | +5.511 | 74,4 |

## Cosa Dicono Per NQ

**Livello estremo, flusso ordinario.** E' la distinzione che fa il lavoro (`analisi-istituzionale.md`,
"Livello contro flusso"). Il **livello**: netto Non-Commercial al 100° percentile a tre anni, e il
campo piu' estremo e' lo **short**, al percentile 0,6: gli speculatori del Nasdaq non sono mai stati
cosi' poco corti in tre anni. Il **flusso** dell'ultimo report: +6.876, z 0,58, percentile 59:
nella norma.

**Chi ha il piede sull'acceleratore** `[1 · 30:01]`: nell'ultima settimana i compratori, con long
aggiunti e short chiusi insieme. Quando i due lati vanno in versi opposti Fabio lo chiama "ancora
meglio" `[1 · 33:28 - 33:50]`, e lo skew satura a +1,00: per questo conta lo z, che dice "ordinario".

**Come e' salito il netto.** Nelle quattro settimane il grosso viene dalla **chiusura di short**:
-13.688 il 15 settembre, -18.987 il 22, contro long quasi fermi (-865, +3.445). E' lo stesso
schema registrato al report del primo settembre (`analisi-istituzionale.md`): il netto da solo non
distingue chiusura di short da apertura di long.

**Il 22 settembre ha una riserva pesante.** E' la variazione piu' grande delle quattro (z 1,96,
percentile 98,1), ma la settimana coperta (mercoledi' 16 - martedi' 22) contiene la scadenza di
settembre, venerdi' 18. L'open interest cala del 12,1% sul Nasdaq, del 22,7% sull'S&P, del 16,4%
sul Russell, del 12,9% sul Dow. Con la regola del collettore (calo oltre il 10% in un mese di
scadenza) sarebbe **settimana di rollover**: parte degli short "chiusi" possono essere contratti
scaduti e non riaperti, non una scelta direzionale. **Osservazione**, non verificata.

**Il cross-confirm non c'e'.** Il Nasdaq e' solo: S&P e Russell hanno i venditori sull'acceleratore,
il Dow e' neutro. Con le due soglie dichiarate (skew 0,30 o z 1,0, consenso 3 su 4) nessun consenso.
I libri divergono anche nel livello: Nasdaq al massimo del netto e al minimo degli short, Russell
al minimo del netto (1,9) e al massimo degli short (100). La metrica cross-index **non ha retto**
alla verifica (`analisi-istituzionale.md`, criterio A senza separazione): il consenso, o la sua
assenza, resta contesto, non conferma.

**La data sul prezzo** `[1 · 31:53]`. Fabio prende la data del cambio e guarda dove stava il prezzo
rispetto al valore di quel giorno.
- **6 ottobre** (ultimo report, netto al massimo di tre anni): la seduta apre in alto, fa il
  massimo di tutte le sedute del file (31.616,5), poi chiude **2,25 punti sotto il proprio VAL** con
  delta -3.557. Gli speculatori risultano piu' lunghi in una seduta che rifiutava il massimo e
  usciva dal valore dal basso. E' la stessa forma del caso dell'oro dell'8 settembre
  (`il-framing-e-il-cot-si-leggono-insieme.md`): posizionamento che sale in una seduta che rompe.
  Dal 7 ottobre il valore scende a ogni seduta; il 6 ottobre e' il massimo del file.
- **22 settembre** (la variazione piu' grande): seduta di salita, chiusura **sopra** il VAH con
  delta +2.427, dentro una settimana in cui la cassa passa da 29.916,5 (chiusura del 18) a 31.029,25
  (chiusura del 22). La chiusura di short coincide con la salita: **osservazione**.
- **15 settembre** (seconda variazione, z 1,15): valore 29.215-29.315, **sotto** il prezzo di stamattina.

**Il ritardo in prezzo** (`analisi-istituzionale.md`, "si quantifica in prezzo"): il 6 ottobre la
cassa chiude a 31.480,75; stamattina, fuori cassa, 29.822,5. Circa **-1.658 punti, -5,3%**
(sottrazione fatta qui sui due numeri dei file). **Tutto il calo recente e' successivo all'ultima
fotografia**: il COT di venerdi' 16 ottobre coprira' 7-13 ottobre, e il 14 non ci sara'.

**La domanda del quadro** (`il-framing-e-il-cot-si-leggono-insieme.md`, "La domanda finale"): il lato
posizionato, il netto lungo, sta sopra o sotto il cuore del volume? Da questa fase si sa solo questo:
i valori del 22 settembre, del 29 settembre e del 6 ottobre stanno **sopra** il prezzo di stamattina,
quello del 15 settembre **sotto**. Il vertice del composito non e' nei miei dati: la risposta la da'
la fase 5.

**TFF.** Le categorie non coincidono con la Legacy (Non-Commercial esiste solo in Legacy, Asset
Manager e Leveraged Funds solo in TFF; `analisi-istituzionale.md`, "La trappola"). L'estremo della
Legacy **non compare** in TFF: Asset Manager al 47° percentile, Leveraged Funds al 74°. I Dealer
(le banche dall'altro lato) vanno piu' corti di 14.320 nella settimana. Nessuna conclusione dalla
somma delle due viste.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| **30.888 - 30.980, POC 30.922** | valore NQZ6 del 22 set, cassa; la variazione del netto piu' grande delle quattro (+22.432, z 1,96) | **posizionamento**: e' il prezzo del martedi' in cui gli short si sono chiusi di piu'; un ritorno li' e' un ritorno sul prezzo di quel cambio. Riserva: settimana di scadenza |
| **31.483 - 31.556, POC 31.530** | valore NQZ6 del 6 ott, cassa; netto al massimo di tre anni | **posizionamento**: il prezzo dell'ultima fotografia; la seduta ha chiuso sotto il VAL 31.483 mentre il netto saliva |
| 30.557 - 30.650, POC 30.609 | valore NQZ6 del 29 set, cassa; unica variazione negativa (-4.903, z -0,47) | posizionamento, debole: flusso ordinario |
| 29.215 - 29.315, POC 29.250 | valore NQZ6 del 15 set, cassa; var. +12.823, z 1,15 | **posizionamento**: l'unico dei quattro valori sotto il prezzo di stamattina |
| **ven 16 ott, 21:30 ora italiana** | prossimo COT CFTC (15:30 di New York), dati di martedi' 13 ott | la prima fotografia dopo il calo dal 6 ottobre |

## Cosa Non Sappiamo

- **Il COT non dice quando** ne' a che prezzo: il martedi' e' un'approssimazione di una
  settimana intera (mercoledi'-martedi'). Nella settimana del 22 settembre la chiusura di cassa sale
  di circa 1.100 punti dal 18 al 22; il valore del martedi' ne e' un punto solo.
- **Eta'**: 9 giorni e circa il 5% di prezzo. Il report di venerdi' coprira' 7-13 ottobre.
- **Scadenza di settembre** dentro la settimana del 22: il calo degli short puo' essere meccanico.
- **Nessuna soglia COT e' validata** in questo repository; la convergenza COT-profilo e' stata
  osservata una volta (oro). La metrica cross-index non ha separato. Tutto qui e' contesto.
- Le convenzioni: futures only (non futures + options); skew 0,30 nostra; z52 sulla variazione;
  percentile del netto sulle 160 settimane della serie, long e short sulle ultime 156.
- Il conteggio dei contratti non e' esposizione confrontabile fra indici ($20 Nasdaq mini, $50 S&P,
  $5 Dow).
- lacuna del collettore: `settimana_di_rollover` e' calcolata solo per l'ultimo report, non per
  `ultime_4`. La settimana del 22 settembre (OI -12,1% a settembre) non e' segnata.
- lacuna del collettore: `cot.json` non porta il prezzo attuale ne' il **ritardo in prezzo**
  (chiusura del martedi' contro adesso); qui e' stato preso da `oggi_finora`, che non e' cassa e
  non dichiara la sua finestra.
- lacuna del collettore: `prezzo_alle_date` da' solo il martedi'; manca il profilo **della settimana
  coperta dal report** (mercoledi'-martedi'), che e' il periodo in cui la variazione si e' formata.
- lacuna del collettore: la TFF del Nasdaq non ha var. long e var. short separate, ne' z52, ne'
  percentile della variazione, ne' le ultime quattro settimane: flusso e livello non si leggono
  come nella Legacy.
- lacuna del collettore: nessun vertice del composito in `dati/` per questa fase; la domanda "sopra
  o sotto il cuore del volume" resta aperta per la fase 5.

## Per Il Quadro

1. **Netto Non-Commercial Nasdaq al 100° percentile a tre anni, short al percentile 0,6, mentre S&P e
   Russell vanno dall'altra parte**: e' coerente con il regime della macro? Un libro speculativo
   al massimo di lunghezza su un solo indice, con il prezzo -5,3% dopo la fotografia.
2. **I valori del passo 6 contro muri e zero gamma**: 30.888-30.980 (22 set), 31.483-31.556 (6 ott),
   30.557-30.650 (29 set), 29.215-29.315 (15 set). Coincide qualcuno con un muro di call o di put, o
   con lo zero gamma di NQZ6?
3. **Sopra o sotto il cuore del volume**: tre dei quattro valori stanno sopra il prezzo di stamattina
   (29.822,5, fuori cassa). Va confrontato con il vertice del composito a 90 giorni della fase 4.
4. **Il 6 ottobre e' il massimo del file e chiude sotto il proprio VAL** mentre il netto sale: la
   fase dell'order flow dice se il delta delle sedute successive conferma o no quella rottura.
   Venerdi' 16 ottobre, 21:30 ora italiana, il COT nuovo dira' se il libro e' stato ridotto.
