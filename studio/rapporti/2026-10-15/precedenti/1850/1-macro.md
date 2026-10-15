# Fase 1 · La Macro · 2026-10-15

**Stato**: parziale (manca il dato uscito del CPI di ieri verificato alla fonte; manca FRED)
**Dati**: `dati/macro.json` (raccolto alle 10:45 ora italiana, `raccolto_utc` 08:45:06), `dati/ore-NQZ6.json`
(barre orarie del bridge fino alle 10:46, l'ultima aperta da 46 minuti), `dati/sedute-NQZ6.json` (le
sedute di cassa), `dati/raccolta.json` (raccolta chiusa alle 10:51, nessun errore). Freschezza:
quotazioni di NQ, ES, VIX, DXY, WTI, oro di stamattina 10:30-10:35; VXN, VIX9D, VIX3M, ETF e azioni
alla chiusura di ieri 22:00-22:15; curva Treasury al 14 ottobre (1 seduta); Fear & Greed di stamattina.
Errori della raccolta: nessuno. **FRED saltato** (manca `FRED_API_KEY`). Calendario della settimana
prossima non ancora pubblicato da Forex Factory (404).

**In una riga**: tassi alti e in salita sul mese (10A al 5,19%, 95° percentile a un anno) con il
mercato che vede la Fed piu' vicina a un rialzo che a un taglio, volatilita' in risveglio ma con la
struttura ancora in contango, dollaro vicino al massimo di un anno, credito al minimo di un anno, e
una vendita concentrata su tecnologia e semiconduttori: un contesto di rischio guidato dall'AI e
dall'inflazione.

## I Fatti

### I tassi

Curva par dei Treasury (fonte: home.treasury.gov, curva par giornaliera, chiusura di New York;
in percentuale; variazioni in punti base, pb = 0,01%).

| scadenza | 14/10 | 13/10 | 15/09 (20 sedute fa) | 1 seduta | 20 sedute |
|---|---|---|---|---|---|
| 3 mesi | 4,28 | 4,26 | 4,11 | +2 pb | +17 pb |
| 2 anni | 4,82 | 4,79 | 4,67 | +3 pb | +15 pb |
| 5 anni | 5,00 | 5,00 | 4,83 | 0 | +17 pb |
| 10 anni | 5,19 | 5,21 | 5,00 | -2 pb | +19 pb |
| 30 anni | 5,52 | 5,56 | 5,36 | -4 pb | +16 pb |
| 10A-2A (pendenza) | +0,37 | +0,42 | +0,33 | -5 pb | +4 pb |
| 10A-3M (pendenza) | +0,91 | +0,95 | +0,89 | -4 pb | +2 pb |

- Il 10 anni (^TNX di Yahoo, 14/10 20:59): 5,19%, **94,8° percentile a un anno**, massimo a un anno
  5,311%. La pendenza e' la differenza fra due rendimenti: positiva vuol dire curva "normale".
- Ieri, giorno del CPI, la curva si e' **appiattita**: il 2 anni (sensibile alla Fed) e' salito, il
  10 e il 30 anni sono scesi.
- **Prossimo FOMC**: 27-28 ottobre, decisione il 28 (fonte: federalreserve.gov via `macro.json`).
  Alle 19:00 italiane: fra il 25 ottobre e il 1° novembre l'Europa e' gia' all'ora solare e gli USA no.
- **Polymarket** (gamma-api.polymarket.com, prezzo come probabilita', raccolta 10:45):

| mercato | esito | probabilita' |
|---|---|---|
| decisione di ottobre (volume 40,7 mln $) | nessun cambio | 75,5% |
| | rialzo di 25 pb | 24,5% |
| | taglio (25 o 50+) | 0,6% |
| tagli nel 2026 (volume 54,2 mln $) | zero | 96,9% |
| tasso a fine 2026 (volume 7,1 mln $) | 4,25% | 61,1% |
| | 4,5% o piu' | 19,6% |
| | 4,0% | 10,6% |
| primo taglio entro... | luglio 2027 | 51% |

- Il livello attuale dei fed funds **non c'e'** nei dati (FRED saltato). Il calendario del giro
  registra il rialzo di 25 pb del FOMC del 16 settembre.

### La volatilita'

Fonte: Yahoo Finance chart v8, giornaliero su un anno; percentile = posizione dell'ultimo valore
nella distribuzione dell'ultimo anno.

| indice | valore | ora | 1 giorno | 5 giorni | percentile 1 anno | min-max 1 anno |
|---|---|---|---|---|---|---|
| VIX (volatilita' implicita a 30 giorni dell'S&P 500) | 17,52 | 15/10 10:30 | +3,85% | +13,69% | 57,1 | 13,47-31,05 |
| VXN (la stessa misura sul Nasdaq-100) | 22,70 | 14/10 22:00 | +7,38% | +8,10% | 43,4 | 17,20-33,54 |
| VIX9D (orizzonte 9 giorni) | 14,59 | 14/10 22:15 | +21,68% | +23,85% | 41,0 | 9,17-30,64 |
| VIX3M (orizzonte 3 mesi) | 18,79 | 14/10 22:15 | +4,62% | +6,04% | 22,7 | 17,42-29,27 |

- **Struttura a termine** (alla chiusura di ieri, VIX ricavato 17,52 / 1,0385 = 16,87):
  VIX9D 14,59 < VIX 16,87 < VIX3M 18,79. E' **contango**: la volatilita' attesa vicina costa meno
  di quella lontana. Rapporto VIX9D/VIX3M = 0,78; VIX/VIX3M = 0,90. Soglia dichiarata dalla skill:
  VIX9D sopra VIX3M = struttura invertita.
- Volatilita' realizzata di NQ (deviazione standard dei rendimenti giornalieri su 20 sedute,
  annualizzata): 19,0%, contro VXN 22,7. Quella di ES: 11,1%.
- VXN meno VIX: 5,83 punti (22,70 - 16,87, chiusura di ieri).

### Dollaro, petrolio, oro, credito

| strumento | ultimo | ora | 1 giorno | 5 giorni | 20 giorni | percentile 1 anno |
|---|---|---|---|---|---|---|
| DXY (indice del dollaro) | 102,005 | 15/10 10:35 | -0,53% | -0,13% | +1,78% | 96,8 (max 102,58) |
| WTI (petrolio, future) | 94,67 | 15/10 10:35 | -0,44% | +3,48% | -7,10% | 80,2 |
| oro (future) | 4.175,3 | 15/10 10:35 | +1,64% | +0,44% | -5,10% | 25,4 |
| TLT (ETF Treasury 20+ anni) | 78,89 | 14/10 22:00 | +0,66% | +2,26% | -2,46% | 5,2 |
| HYG (ETF obbligazioni high yield) | 76,81 | 14/10 22:00 | -0,36% | -0,48% | -2,05% | **0,4 (minimo a un anno)** |

Fonte: Yahoo Finance. High yield = obbligazioni societarie sotto il grado d'investimento: il loro
prezzo misura l'appetito per il rischio di credito.

### Chi guida

| strumento | ultimo | ora | 1 giorno | 5 giorni | 20 giorni | percentile 1 anno |
|---|---|---|---|---|---|---|
| NQ=F (future Nasdaq-100, continuo) | 29.811,75 | 15/10 10:35 | -1,23% | -3,74% | +1,24% | 83,3 |
| ES=F (future S&P 500) | 7.743,5 | 15/10 10:35 | -0,34% | -0,93% | +1,35% | 90,1 |
| QQQ | 728,95 | 14/10 22:00 | -1,97% | -3,80% | +3,44% | 86,5 |
| RSP (S&P 500 equipesato: la partecipazione) | 212,09 | 14/10 22:00 | -0,57% | +0,71% | -0,09% | 74,1 |
| SMH (semiconduttori) | 564,74 | 14/10 22:00 | -3,98% | **-9,65%** | +3,52% | 68,5 |
| XLK (tecnologia) | 190,43 | 14/10 22:00 | -2,98% | -5,44% | +3,53% | 89,2 |

- QQQ meno RSP: -1,40 punti percentuali in un giorno, **-4,51 in cinque**.
- NQ=F e' 5,3% sotto il massimo a un anno (31.483,25, Yahoo continuo).
- Le otto grandi ieri (chiusura 22:00): AAPL +0,50% (al massimo a un anno), MSFT -1,80%, NVDA -3,16%,
  AMZN -1,74%, GOOGL -1,74%, META -3,27%, AVGO -3,10%, TSLA -2,15%. **Sette su otto in calo.**
  Su cinque giorni: NVDA -6,81%, AVGO -8,04%.
- **Trimestrali** (api.nasdaq.com, EPS atteso): TSM oggi prima dell'apertura (4,45 $); NFLX 20/10
  dopo la chiusura (0,82 $); META (6,39 $) e TSLA (0,21 $) 21/10 dopo; **GOOGL (2,94 $) e MSFT
  (4,71 $) il 28/10 dopo la chiusura, lo stesso giorno del FOMC**; AMZN (2,02 $, ora non data) e
  INTC (0,27 $) il 29/10.

### Il sentiment

CNN Fear & Greed (indice 0-100 da sette misure di mercato; sotto 45 "fear"): **29,8, "fear"**,
stamattina. Una settimana fa 39,8 (-10 punti), un mese fa 28,0.

### Il calendario

Fonte: Forex Factory (attesa e precedente), ora italiana; NQ da `ore-NQZ6.json` (bridge, M1
aggregato per ora; l'etichetta e' l'inizio dell'ora).

| quando | evento | attesa | precedente | uscito | NQZ6 nell'ora |
|---|---|---|---|---|---|
| 14/10 14:30 | CPI m/m · a/a | 0,6% · 3,6% | 0,4% · 3,4% | 0,6% · 3,7% (non verificato) | 30.762,75 -> 30.557 (-205,75, -0,67%); max 30.852,75, min 30.543; volume 47.627, delta +571 |
| 14/10 14:30 | CPI core m/m · a/a | 0,2% · 2,5% | 0,3% · 2,4% | 0,3% · 2,6% (non verificato) | come sopra |
| 15/10 14:30 | vendite al dettaglio m/m | 0,3% | 1,2% | dopo la raccolta | |
| 15/10 14:30 | vendite al dettaglio core m/m | 0,6% | 1,4% | | |
| 15/10 14:30 | PPI m/m · core m/m | 0,6% · 0,3% | 0,4% · 0,2% | | |
| 15/10 14:30 | Philly Fed manifatturiero | 28,2 | 37,8 | | |
| 15/10 14:30 | sussidi di disoccupazione | 199 mila | 197 mila | | |
| 16/10 05:30 | discorso del presidente della Fed Warsh | | | | |

- **Il dato uscito del CPI non e' verificato da questa fase**: quattro ricerche (BLS e stampa) hanno
  restituito solo anteprime. I valori nella colonna "uscito" vengono dall'annotazione gia' presente
  in `docs/research/calendario/eventi.json`, che cita il BLS. I titoli del 14-15/10 in `macro.json`
  confermano solo la direzione: "hot CPI print", "rate hike fears".
- La cassa di ieri (15:30-22:00, `sedute-NQZ6.json`): apertura 30.551,5, massimo 30.660,75, minimo
  30.078, chiusura 30.185, **-603,75 (-1,96%)** sulla chiusura del 13/10 (30.788,75). Volume 540.320,
  il piu' alto delle ultime venti sedute; delta +530; valore 30.082-30.478, POC 30.173.

### Le notizie e le ore di NQ

Fonte: RSS di CNBC e Google News in `macro.json` (titoli, senza testo), ore di NQZ6 dal bridge.

| ora | notizia | NQZ6 nell'ora |
|---|---|---|
| 14/10 14:30 (titoli 15:16, 22:53) | CPI caldo; "Treasury yields spike as inflation fuels Fed hike fears" | -205,75 nell'ora delle 14; -83 in quella delle 15 |
| 14/10 pomeriggio | vendita dei titoli AI (SMH -3,98%, NVDA -3,16%; CNBC 15/10 10:18 "AI stock sell-off deepens") | ora delle 17: -158,25; ora delle 19: -149,25; minimo di cassa 30.078 nell'ora delle 21 |
| 14/10 23:40 e 15/10 00:35 | Daly (Fed): "rate hikes likely needed, but not urgently"; Bloomberg: un previsore vede +50 pb a dicembre | ora delle 00: 30.154,75 -> 29.991,75 (-163), delta -1.320, il piu' negativo delle 48 ore |
| 15/10 03:49-06:32 | dollaro giu' dal massimo di 18 mesi, rendimenti in calo, "Fed hike bets ease"; petrolio giu' (06:09) | ora delle 06: 29.855,25 -> 29.957,25 (+102), l'unico rimbalzo della notte |
| 15/10 08:00-10:25 | "Nasdaq futures dive 1% as AI rout deepens on debt concerns" (Reuters 10:25); Kospi -8% (CoinDesk 06:22) | ora delle 08: -88, delta -1.243; minimo 29.754 alle 09:06; ora delle 10 piatta (-16,25) |

**Rumore** (notizia senza movimento orario coerente): 14/10 18:27 "futures rise, investors look past
Iran" (ora delle 18: -35); 14/10 22:27 "futures track Nvidia higher" (ora delle 22: -47, poi la
discesa); 15/10 02:15 "futures gain as Trump signals Iran talks" (ora delle 02: +23,75);
15/10 05:48 ricavi di Anthropic (ora delle 05: -12); 15/10 09:46 Ucraina riprende gli attacchi alle
raffinerie (WTI -0,44% sul giorno); 15/10 10:44 PIL britannico.

La notte: dalla chiusura delle 22:00 (30.139,25) alle 10:46 (29.822,5) **-316,75 (-1,05%)**; massimo
30.158,5, minimo 29.754, escursione 404,5 punti; volume 175.531, delta -2.415 (`sedute-NQZ6.json`,
`oggi_finora`).

## Cosa Dicono Per NQ

1. **I tassi pesano dalla parte corta.** Il 2 anni al 4,82% sta 54 pb sopra il 3 mesi: il mercato
   obbligazionario prezza tassi piu' alti di quelli di oggi. Polymarket da' 24,5% a un rialzo il
   28/10 e il 96,9% a zero tagli nel 2026. **Osservazione**: la caduta di NQ della notte coincide con
   le frasi di Daly sui rialzi, non con un movimento dei rendimenti lunghi, che anzi scendono.
2. **La vendita e' settoriale, non generale.** NQ=F -1,23% contro ES=F -0,34% (Yahoo, stessa
   ora, 10:35); ieri QQQ -1,97% contro RSP -0,57%; RSP +0,71% su cinque giorni contro QQQ -3,80%; SMH -9,65% su cinque giorni. Il
   motore e' il tema AI (semiconduttori, infrastrutture, il loro debito), con il CPI come innesco
   d'orario. **Osservazione**: nessun documento del metodo misura la rotazione; e' una lettura dei
   rendimenti relativi.
3. **La volatilita' si sveglia ma non ha paura.** VXN +7,38% e VIX9D +21,68% in un giorno, eppure la
   struttura resta in contango (VIX9D/VIX3M = 0,78) e il VIX e' al 57° percentile. La regola della
   skill (VIX9D sopra VIX3M = paura vicina) **non e' scattata**. La volatilita' implicita di NQ
   (22,7) sta sopra la realizzata (19,0): le opzioni prezzano piu' movimento di quello degli ultimi
   venti giorni.
4. **Il credito e' l'anello debole.** HYG al minimo a un anno (percentile 0,4) mentre TLT rimbalza
   (+2,26% su cinque giorni): si compra la duration e si vende il rischio di credito. Coerente con i
   titoli "AI rout on debt concerns". **Osservazione**, non misura: manca lo spread high yield (FRED).
5. **Il dollaro forte frena.** DXY al 96,8° percentile, appena sotto il massimo di 18 mesi; stanotte
   e' sceso dello 0,53%, e il solo rimbalzo orario della notte (ora delle 06, +102) cade nella stessa
   finestra. **Osservazione** su una sola ora.
6. **Oggi alle 14:30 escono cinque dati insieme.** Con l'inflazione gia' sopra l'attesa e un
   rialzo il 28/10 al 24,5%, PPI e vendite al dettaglio sono la prossima prova del prezzo dei tassi.
   Il 28/10 cadono nello stesso giorno il FOMC e le trimestrali di GOOGL e MSFT.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 10A 5,311% | ^TNX, massimo a un anno | posizionamento dei tassi: oltre, il 10A esce dal massimo dell'anno (il 23/09 il massimo dal 2007 coincise con NQ -1,13%, `eventi.json`) |
| VIX9D 18,79 (= VIX3M di ieri) | Yahoo, chiusura 14/10 | soglia della struttura: sopra, la curva del VIX si inverte |
| HYG 76,81 | Yahoo, minimo a un anno | posizionamento del credito: sotto, il credito fa un nuovo minimo dell'anno |
| DXY 102,58 | Yahoo, massimo a un anno | posizionamento del dollaro: sopra, il dollaro fa un nuovo massimo dell'anno |
| NQZ6 30.185 | chiusura della cassa 14/10, bridge | riferimento del gap d'apertura di oggi (alle 10:46: -362,5) |
| NQZ6 29.754 | minimo della notte (09:06), bridge | estensione notturna della seduta del CPI: per la fase 4 |
| 15/10 14:30 | vendite al dettaglio, PPI, Philly Fed, sussidi | evento alto: 5 dati alla stessa ora |
| 16/10 05:30 | discorso di Warsh (Forex Factory) | evento alto, in piena notte europea |
| 16/10 21:30 | COT, posizioni di martedi' 13 | contesto per la fase 2 |
| 20-21/10 | NFLX; META e TSLA, dopo la chiusura | trimestrali di due delle otto grandi |
| 28/10 19:00 | FOMC; poi GOOGL e MSFT dopo la chiusura | evento alto: decisione sui tassi e due grandi lo stesso giorno |

## Cosa Non Sappiamo

- **Il CPI uscito non e' verificato alla fonte**: quattro ricerche (BLS e stampa) senza il
  comunicato. I numeri in tabella vengono dall'annotazione del calendario, non da questa fase.
- **I dati di oggi delle 14:30** escono dopo la raccolta (10:51): nessun esito.
- **La Fed vista solo da Polymarket.** Non ci sono le probabilita' dai future sui fed funds (CME
  FedWatch); le regole del mercato "tasso a fine 2026" (estremo alto o medio della forbice) non
  sono nei dati.
- I tempi delle quotazioni si mescolano: VIX e NQ di stamattina, VXN, VIX9D, VIX3M ed ETF di ieri
  sera. La struttura del VIX e' confrontata tutta alla chiusura di ieri.
- La variazione a un giorno di NQ=F (Yahoo, -1,23%) non dichiara la chiusura di riferimento; dal
  bridge la notte fa -1,05% sulle 22:00 e -1,20% sulla chiusura di cassa.
- L'abbinamento notizia-ora e' una coincidenza temporale: non prova la causa.
- lacuna del collettore: FRED (fed funds effettivo, inflazione attesa, spread high yield, indice
  di stress finanziario), saltato senza `FRED_API_KEY`.
- lacuna del collettore: il dato uscito degli eventi del calendario (Forex Factory da' solo attesa e
  precedente).
- lacuna del collettore: i rendimenti di stamattina (curva e ^TNX fermi al 14/10).
- lacuna del collettore: VXN, VIX9D, VIX3M intraday di oggi.
- lacuna del collettore: l'esito delle trimestrali (TSMC di stamattina: solo l'EPS atteso).
- lacuna del collettore: probabilita' FOMC dai future sui fed funds (ZQ) o da CME FedWatch.
- lacuna del collettore: l'ampiezza del Nasdaq-100 (titoli in salita contro titoli in calo).
- lacuna del collettore: il calendario della settimana prossima (Forex Factory 404 fino a fine
  settimana).
- lacuna del collettore: il testo delle notizie (solo i titoli RSS).

## Per Il Quadro

1. **Credito al minimo, sentiment in paura** (HYG percentile 0,4; Fear & Greed 29,8, -10 in una
   settimana): coerente con il netto dei Non-Commercial su NQ della fase 2, o gli speculatori sono
   ancora lunghi?
2. **Contango con NQ -3,74% in cinque giorni** (VIX9D/VIX3M 0,78; soglia d'inversione VIX9D 18,79):
   coerente con il segno della gamma dei dealer della fase 3? Con gamma negativa una struttura in
   contango e' fragile.
3. **Cassa del 14/10 a -603,75 punti con delta +530** e volume massimo di venti sedute: la fase 4
   dica se la discesa e' stata passiva (senza aggressione in vendita) e come si e' comportata la
   notte (delta -2.415, minimo 29.754).
4. **14:30 di oggi e 28/10**: i livelli di gamma e di order flow vanno letti sapendo che alle 14:30
   escono cinque dati e che il FOMC (rialzo al 24,5%) cade con GOOGL e MSFT.
