# Fase 1 · La Macro · 28/10/2026

**Stato**: parziale (`esiti` vuoto: il dato del Conference Board di ieri viene dal web, non verificato)
**Dati**: `dati/macro.json` (raccolto 10:17 ora italiana), `dati/ore-NQZ6.json` (bridge, ultima ora aperta 10:00-10:18), `dati/raccolta.json` (nessun errore). Freschezza: curva Treasury al 27/10; FRED giornalieri al 26-27/10; VIX e future ZQ di oggi (10:02 e 09:39-10:06); VIX9D, VIX3M, VXN e 10 anni Yahoo **fermi a ieri sera** (27/10 21:15); 2YY=F fermo al 13/10.

**In una riga**: tassi alti ma in calo da un mese, inflazione calda che tiene la Fed verso un rialzo a dicembre, volatilita' media con struttura normale, dollaro e petrolio forti, credito che non si allarga; NQ fa peggio dell'S&P da venti sedute e oggi aspetta tre eventi in tre ore.

## In Parole Semplici

**Il punto**: oggi per NQ contano tre cose, in fila: la Fed alle 19:00, il suo presidente alle 19:30, i conti di Microsoft e Alphabet dopo le 21:00. Sulla decisione di oggi il mercato e' quasi sicuro (tassi fermi); la sorpresa possibile sta nelle parole su dicembre e nei conti.

**Dal piu' ampio al piu' vicino**

1. **I tassi**: il rendimento del titolo di Stato a 10 anni e' vicino al 5%, fra i piu' alti degli ultimi due anni, ma e' sceso di circa un quarto di punto in un mese.
   → Il denaro costa caro, e questo pesa sui titoli tecnologici, che vivono di utili futuri; nell'ultimo mese il peso si e' alleggerito.
2. **La Fed**: oggi tassi fermi al 95% (scommesse online) e circa 92% (future sui tassi); a dicembre le scommesse danno un rialzo al 76%.
   → La partita non e' oggi, e' su dicembre: se il presidente conferma o smentisce il rialzo, i tassi si muovono e NQ con loro.
3. **La paura**: l'indice della paura sul Nasdaq (VXN) e' a circa 25, nella meta' alta del suo anno; la paura a nove giorni e' sotto quella a tre mesi.
   → Il mercato paga una protezione normale, non da panico, anche con Fed e trimestrali in arrivo.
4. **Chi guida**: in venti sedute NQ ha perso circa 2,5%, mentre l'S&P equipesato (tutte le aziende con lo stesso peso) ha guadagnato quasi 4%. Meta ha perso il 16%, i chip l'8%.
   → I soldi si sono spostati dai grandi del tech al resto del mercato; i conti di stasera dicono se il tech si riprende la guida.
5. **La notte**: NQ e' salito di circa 80 punti, con il rialzo dei chip asiatici (SK Hynix +11%), e alle 10 sta intorno a 29.940.
   → Una notte tranquilla e in salita, in attesa degli eventi.

**Quando**: 19:00 decisione della Fed · 19:30 conferenza del presidente · dopo le 21:00 Microsoft e Alphabet · domani 13:30 PIL e inflazione PCE · domani dopo le 21:00 Amazon
**Quanto e' provato**: i numeri sono misurati alle fonti; il legame "tassi giu', tech su" e' un'osservazione generale, non misurata su NQ; nessuno studio del repository misura come NQ reagisce alla Fed o alle trimestrali.

**Le parole**: **Fed / FOMC** = la banca centrale USA e la riunione che decide i tassi; **rendimento** = l'interesse che paga un titolo di Stato; **VIX / VXN** = la volatilita' attesa (la "paura") su S&P e Nasdaq per i prossimi 30 giorni; **future sui fed funds (ZQ)** = contratti da cui si ricava il tasso che il mercato si aspetta; **equipesato** = ogni azienda conta uguale, a prescindere dalla grandezza.

## I Fatti

**Tassi** (curva par Treasury, home.treasury.gov, %; 27/10 · 26/10 · 28/09, cioe' 20 sedute fa)

- 3M 4,22 · 4,22 · 4,28; 2A 4,70 · 4,68 · 4,92; 10A 4,99 · 5,00 · 5,24; 30A 5,30 · 5,31 · 5,56.
- Pendenze: 10A-2A 0,29 · 0,32 · 0,32; 10A-3M 0,77 · 0,78 · 0,96.
- FRED: fed funds effettivo 3,88% (26/10, percentile 47,7 su due anni); 2 anni al percentile 95,0 e 10 anni al 95,6 su due anni; breakeven a 10 anni (inflazione attesa) 2,35% (27/10).
- Inflazione (FRED, settembre): CPI +0,65% m/m, 3,72% a/a; core 0,33% m/m, 2,57% a/a; PPI domanda finale +1,34% m/m, 6,35% a/a. Vendite al dettaglio +0,77% m/m. Sussidi 194 mila (settimana al 17/10). Philly Fed 20,9 (ottobre).
- Fed oggi, future ZQ (Yahoo, CBOT, 09:39-10:06): implicito prima 3,880%, dopo 3,900%, +2 pb, rialzo da 25 pb all'**8%** (metodo FedWatch, in `fomc_future.metodo`). Dicembre ZQZ26 implicito 4,05%.
- Polymarket (gamma-api, ora della raccolta): ottobre fermo 95%, +25 pb 4,6%; dicembre +25 pb **76,5%**, fermo 19,5%; zero tagli nel 2026 al 96%; tasso a fine 2026 al 4,25% per il 71,3%; "surriscaldamento" (disoccupazione <5%, inflazione >=3,5%) al 95,1%.

**Volatilita'** (Yahoo; percentile a un anno)

| indice | ultimo | ora | var 20 sedute | percentile 1 anno |
|---|---|---|---|---|
| VIX | 17,57 | 28/10 10:02 | +7,5% | 57,9 |
| VIX9D | 16,65 | 27/10 21:15 | +17,2% | 63,7 |
| VIX3M | 19,57 | 27/10 21:15 | +8,2% | 43,8 |
| VXN | 24,91 | 27/10 21:15 | +12,9% | 65,3 |

- Struttura VIX9D 16,65 < VIX 17,57 < VIX3M 19,57: **contango** (la paura vicina costa meno di quella lontana). VXN ieri -1,49 punti (26,40 -> 24,91). NQ realizzata: 18,9% annua su 20 sedute (Yahoo, giornaliera); dal bridge 11,4% (ultime 4 ore), 16,4% (24 ore), 18,7% (48 ore), rendimenti log a 5 minuti annualizzati 276 x 252.

**Dollaro, petrolio, oro, credito** (Yahoo; future alle 10:07, ETF alla chiusura del 27/10)

| | ultimo | 1 g | 20 sedute | percentile 1 anno |
|---|---|---|---|---|
| DXY | 102,37 | +0,05% | +0,91% | 98,8 (massimo 102,58) |
| WTI | 101,69 | +1,79% | +12,46% | 91,3 |
| oro | 4.065,9 | -0,21% | -2,89% | 10,3 |
| TLT | 81,63 | -0,06% | +4,35% | 12,0 |
| HYG | 77,60 | +0,52% | +0,31% | 8,8 |

- Spread high yield ICE BofA 3,30 punti (26/10, -8 pb; percentile 92 su due anni). Stress St. Louis Fed -0,63 (16/10). Bilancio Fed 6.753,6 mld $ (21/10, -2 mld).

**Chi guida** (Yahoo, variazioni %; ampiezza da api.nasdaq.com al 27/10)

| | 1 g | 5 g | 20 g |
|---|---|---|---|
| NQ (future, 10:07) | +0,35 | -0,31 | -2,55 |
| ES (future, 10:07) | +0,07 | -0,47 | +0,63 |
| RSP | +0,11 | +0,77 | +3,83 |
| SMH | +3,02 | +2,13 | -8,06 |
| META / GOOGL / MSFT | -1,64 / +1,19 / -0,24 | -10,67 / -5,79 / -0,10 | -16,05 / -5,97 / +4,40 |
| TSLA / AVGO / AAPL | +4,04 / +4,00 / +0,93 | +9,03 / +2,38 / +2,24 | +15,48 / -3,60 / +5,37 |

- Nasdaq-100 il 27/10: 58 salgono, 41 scendono; media +0,40%, mediana +0,19%, pesata per capitalizzazione +0,62%. NQ al percentile 83,7 dell'anno (massimo 1 anno 31.483,25, Yahoo continuo). Sentiment: CNN Fear & Greed 34,3 ("paura"), 30,3 una settimana fa, 32,4 un mese fa.

**Calendario** (Forex Factory, ora italiana; trimestrali da api.nasdaq.com)

| quando | evento | attesa | precedente |
|---|---|---|---|
| 27/10 15:00 | fiducia Conference Board | 81,2 | 81,9 |
| 28/10 19:00 | decisione Fed (tasso superiore) | 4,00% | 4,00% |
| 28/10 19:30 | conferenza stampa | | |
| 28/10 dopo le 21:00 | MSFT (EPS 4,71 $), GOOGL (2,94 $) | | |
| 29/10 13:30 | PIL anticipato t/t; deflatore | 3,2%; 3,2% | 1,5%; 6,2% |
| 29/10 13:30 | PCE core m/m; sussidi | 0,3%; 198 mila | 0,2%; 194 mila |
| 29/10 dopo le 21:00 | AMZN (2,01 $), INTC (0,28 $) | | |
| 30/10 13:30 | costo del lavoro ECI t/t | 0,8% | 0,9% |

- **Esito di ieri**: Conference Board non in `esiti`. Il web (pagina del Conference Board, via ricerca) riporta **91,7**, contro 84,2 a settembre: incoerente con il precedente 81,9 di Forex Factory, quindi **non verificato**. NQZ6 nell'ora delle 15:00: 29.575,5 -> 29.708,75 (+133), minimo 29.494,5, delta -255; ora delle 16:00 +133 a 29.842,25.

**Le notizie e il movimento orario di NQZ6** (bridge, ora italiana)

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 28/10 00:25 | WSJ: Dow e Treasury su, rotazione fuori dal tech | ieri QQQ +1,27% e NQZ6 in cassa da 29.432 a 29.842: il titolo non si vede nel prezzo del Nasdaq |
| 28/10 02:11 | WSJ: rendimenti francesi ai massimi di piu' anni, Treasury in leggero rialzo | ora 02 +8, delta +325: rumore |
| 28/10 04:02 | Trump: la guerra con l'Iran finira' "quando voglio"; costi a 47 mld $ | ora 04 +54,5 (29.881,75 -> 29.936,25), delta +485: nessuna reazione negativa |
| 28/10 06:51-07:56 | SK Hynix +11% su utile record; future Nasdaq su | ora 07 +37,75, ora 08 +25,25 (massimo notte 29.989,25 alle 09) |
| 28/10 08:18 | WSJ: rendimenti Treasury in rialzo prima della Fed | ora 08 +25, ora 09 -50: nessun legame chiaro |

## Cosa Dicono Per NQ

- **Il regime dei tassi**: 10 anni al percentile 95,6 su due anni, ma -25 pb in venti sedute; il 2 anni -22 pb. La curva e' positiva (10A-2A +0,29) ma si e' appiattita di 3 pb ieri. Tassi lunghi alti pesano sulle valutazioni dei titoli a lunga durata come il tech: **osservazione** generale, non misurata su NQ in questo repository.
- **La Fed di oggi e' scontata, dicembre no**: ZQ e Polymarket concordano su tassi fermi (92-95%). Polymarket dà il rialzo di dicembre al 76,5%, coerente con inflazione al 3,72% a/a, PPI al 6,35% e "surriscaldamento" al 95%. La sorpresa possibile e' nel comunicato e nella conferenza: conferma o smentita del rialzo a dicembre. Il 16/09 la Fed ha alzato di 25 pb (calendario del giro). **Osservazione**: non ci sono misure di come NQ reagisce nell'ora del FOMC (lacuna).
- **La volatilita' non prezza panico**: contango pieno; VXN/VIX circa 1,42, il Nasdaq paga piu' paura dell'S&P. VXN 24,9 sopra la realizzata a 20 sedute di NQ (18,9%): le opzioni costano piu' di quanto il prezzo si sia mosso, coerente con due eventi in arrivo. Il VIX9D (16,65) sotto il VIX e' di ieri sera: non contiene ancora la mattina di oggi.
- **Petrolio e dollaro**: WTI sopra 100 (+12,5% in venti sedute, guerra con l'Iran nelle notizie) e DXY al percentile 98,8 alimentano l'inflazione e la tesi del rialzo. L'oro al percentile 10 e TLT al 12 dicono che i beni rifugio non sono cercati.
- **Il credito**: HYG al percentile 8,8 dell'anno ma lo spread high yield e' sceso di 8 pb e lo stress St. Louis e' negativo (-0,63, sotto la media): nessun segnale di tensione acuta.
- **Chi guida**: NQ -2,55% contro RSP +3,83% in venti sedute: la partecipazione si e' allargata fuori dal tech. Ieri i chip (SMH +3%) e l'ampiezza positiva (58 su 99) hanno aiutato; META resta il peso negativo dopo i conti del 21/10. MSFT e GOOGL pesano insieme circa il 26% del Nasdaq-100 per capitalizzazione (8,7 + 8,6 + 8,5 con GOOG): i loro conti dopo le 21:00 muovono l'indice piu' di qualunque dato macro di oggi.
- **Sentiment**: Fear & Greed 34 ("paura"), in lieve risalita in una settimana; con NQ al percentile 84 dell'anno, il sentiment resta piu' cauto dei prezzi.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 28/10 19:00 | FOMC, decisione e comunicato | posizionamento: conferma o smentita del rialzo di dicembre |
| 28/10 19:30 | conferenza stampa | posizionamento: le parole su dicembre muovono il 2 anni |
| 28/10 dopo le 21:00 | MSFT e GOOGL | posizionamento: circa un quarto del Nasdaq-100 in due trimestrali, a mercato a contanti chiuso |
| 29/10 13:30 e dopo le 21:00 | PIL, deflatore, PCE core, sussidi; poi AMZN e INTC | posizionamento: l'inflazione che la Fed guarda, poi un altro peso massimo |
| 31.483,25 | massimo a un anno di NQ=F (Yahoo, continuo) | contesto: NQ e' circa 5% sotto |

## Cosa Non Sappiamo

- Il dato del Conference Board di ieri: il web dice 91,7 ma contraddice il precedente di Forex Factory (81,9). Non verificato.
- La regola "tassi su, tech giu'" e la reazione di NQ alla Fed e alle trimestrali non sono misurate in questo repository.
- lacuna del collettore: `esiti` vuoto; il Conference Board non arriva da FRED, e il collettore non ha un'altra fonte per i dati usciti. 2YY=F fermo al 13/10; VIX9D, VIX3M, VXN e ^TNX senza barre di oggi.
- lacuna del collettore: `fomc_future` converte in probabilita' solo la prossima riunione; dicembre resta un implicito (4,05%) senza probabilita'.
- lacuna del collettore: nessuna misura della reazione oraria di NQ ai FOMC passati e alle trimestrali delle grandi, ne' il movimento implicito delle opzioni di MSFT e GOOGL. Calendario della settimana prossima non pubblicato (HTTP 404); manca l'ora della lettura del Fear & Greed.

## Per Il Quadro

- Tre eventi in tre ore (19:00, 19:30, dopo le 21:00) con VXN 24,9 sopra la realizzata 18,9: coerente con quello che le opzioni (fase 3) dicono sulla scadenza di oggi e domani?
- Polymarket al 76,5% per un rialzo a dicembre e 10 anni al percentile 95,6: coerente con il posizionamento degli speculatori sui Treasury e su NQ nel COT (fase 2)?
- NQ -2,55% contro RSP +3,83% in venti sedute: si vede nell'order flow (fase 4) come vendita nei rimbalzi, o la notte in salita (+80 punti, delta orario positivo, realizzata a 4 ore all'11,4%) dice altro?
