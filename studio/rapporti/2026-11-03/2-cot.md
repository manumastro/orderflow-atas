# Fase 2 · Il COT · 2026-11-03

**Stato**: fatto
**Dati**: `dati/cot.json` (CFTC, Legacy futures only e TFF, raccolto alle 16:47 ora italiana),
`dati/sedute-NQZ6.json` (sedute di cassa), `dati/raccolta.json` (raccolta chiusa alle 16:47, nessun
errore). Report in uso: dati di **martedi' 27 ottobre**, 7 giorni fa. Prezzo di adesso: NQZ6 28.357
alle 16:47 ora italiana, a cassa aperta (fonte `prezzo_adesso` di `cot.json`). Errori: nessuno.

**In una riga**: gli speculatori sul Nasdaq restano lunghi quasi come mai in tre anni, l'ultima
settimana hanno alleggerito poco, i gestori di fondi molto, e tutto il calo di circa 1.450 punti e'
venuto dopo la fotografia.

## In Parole Semplici

**Il punto**: chi specula sul Nasdaq e' ancora molto sbilanciato al rialzo, ma nell'ultima settimana
misurata i grandi gestori hanno alleggerito come raramente fanno. Lo hanno fatto con il prezzo
intorno a 29.800; oggi il prezzo e' circa 1.450 punti piu' sotto, e il rapporto non vede quel calo.

**Dal piu' ampio al piu' vicino**

1. **Le posizioni di fondo**: gli speculatori hanno piu' del doppio di contratti al rialzo che al ribasso, e i ribassisti sono ai minimi di tre anni.
   → Sono lunghi come quasi mai negli ultimi tre anni: il lato affollato e' quello che compra.
2. **L'ultima settimana, speculatori**: hanno tolto poco, circa 1.900 contratti di netto.
   → Una variazione piccola e normale; sugli altri tre indici americani invece hanno aggiunto.
3. **L'ultima settimana, gestori di fondi**: hanno tagliato circa 12.400 contratti di netto.
   → E' uno dei tagli piu' grandi degli ultimi tre anni; e' l'unico movimento fuori dall'ordinario.
4. **Dove stava il prezzo**: il 27 ottobre il valore della seduta era fra circa 29.640 e 29.880.
   → I gestori hanno alleggerito mentre il prezzo risaliva, non mentre scendeva.
5. **Adesso**: 28.357, sotto il prezzo di tutte e quattro le ultime date del rapporto.
   → Chi si e' caricato al rialzo in queste settimane, oggi, e' in perdita sulla carta.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 29.640-29.880 (centro 29.805) | il valore del 27 ottobre, il giorno del taglio grande dei gestori | il prezzo torna dove i gestori hanno alleggerito: e' un ritorno sul loro prezzo |
| circa 30.710-30.800 (centro 30.775) | il valore del 13 ottobre, il taglio piu' grande degli speculatori nelle quattro settimane | lontano, circa 2.400 punti: riferimento per le settimane, non per oggi |

**Quando**: venerdi' 6 novembre, 21:30 ora italiana, il prossimo rapporto (dati di oggi, martedi' 3).
**Quanto e' provato**: i numeri sono misurati (CFTC). Usare il rapporto per prevedere la direzione non
ha retto nelle nostre verifiche; trasformare la data in un prezzo e' la procedura del corso, vista
funzionare una volta, mai contata su una serie.

**Le parole**: **COT** = il rapporto settimanale della CFTC su chi tiene i contratti; **speculatori** =
i "Non-Commercial", chi opera per guadagnare sul prezzo; **gestori di fondi** = gli "Asset Manager"
di un'altra classificazione, la TFF; **netto** = contratti al rialzo meno contratti al ribasso;
**valore della seduta** = la fascia dove si e' scambiato il 70% del volume; **centro** = il POC, il
prezzo piu' scambiato.

## I Fatti

**Passi 1-2: Legacy, solo Non-Commercial, futures only, report del 27/10** (`cot.json`, `indici`).
Percentili a tre anni (156 settimane); variazioni sul 20/10.

| indice | long | short | netto | var. long | var. short | var. netto | pct netto | pct long | pct short |
|---|---|---|---|---|---|---|---|---|---|
| NASDAQ-100 mini | 84.567 | 35.728 | 48.839 | -1.532 | +355 | -1.887 | 95,0 | 64,7 | 3,8 |
| S&P 500 e-mini | 215.924 | 350.292 | -134.368 | +39 | -8.264 | +8.303 | 34,4 | 7,7 | 51,9 |
| Dow x5 | 19.827 | 12.083 | 7.744 | -785 | -1.804 | +1.019 | 60,6 | 53,8 | 37,2 |
| Russell 2000 mini | 112.363 | 148.442 | -36.079 | -3.431 | -9.261 | +5.830 | 36,2 | 99,4 | 94,9 |

**Passo 3, skew di fondo** = (long - short) / (long + short): NASDAQ +0,406; netto 48.839 contro un
massimo a tre anni di 58.123 (6 ottobre). Commercial netto -62.092. Open interest 268.508 (-1,5%).

**Passo 4, ultima variazione** (NASDAQ): skew della variazione = (var. long - var. short) / (|var.
long| + |var. short|) = **-1,0**, saturo perche' le due variazioni hanno segno opposto; **z a 52
settimane -0,17**; |var. netto| al 21,2 percentile delle variazioni a tre anni. Soglia dello skew
0,30: convenzione nostra, dalla frase del live "delta del 30% fra i due lati" `[1 · 33:12]`.

**Passo 5, cross-confirm** (`cross_confirm`): segni della var. netto NASDAQ -1, S&P +1, Dow +1,
Russell +1; concordi col principale **1 su 4**. Skew della variazione: S&P +1,0 (saturo), Dow +0,394,
Russell +0,459. z a 52 settimane: S&P 0,23, Dow 0,18, Russell 0,46; nessuno oltre 1.

**Passo 6, le quattro date sul prezzo** (`prezzo_alle_date`, `settimane_dei_report`; cassa 09:30-16:00
New York: 15:30-22:00 ora italiana il 6, 13 e 20 ottobre, 14:30-21:00 il 27 ottobre, fasce da 1 punto).

| report | var. netto NC | z52 | pct var. | seduta: VAL-POC-VAH | chiude | settimana: VAL-POC-VAH | adesso dal POC del giorno |
|---|---|---|---|---|---|---|---|
| 27/10 | -1.887 | -0,17 | 21,2 | 29.637-29.805-29.880 | dentro, 29.813 | 29.026-29.500-29.679 | -1.448 |
| 20/10 | +2.203 | +0,20 | 23,1 | 29.599-29.757-29.842 | dentro, 29.688 | 29.584-29.770-30.178 | -1.400 |
| 13/10 | **-9.600** | **-0,86** | **71,2** | 30.712-30.775-30.800 | dentro, 30.789 | 30.846-30.890-31.315 | **-2.418** |
| 06/10 | +6.876 | +0,58 | 59,0 | 31.483-31.530-31.556 | sotto, 31.481 | 30.529-30.824-31.213 | -3.173 |

Nessuna delle quattro settimane e' di rollover, in nessuna delle due regole (`regola_rollover`);
nessuna seduta mancante.

**TFF del Nasdaq, complemento** (`tff_nasdaq`, report 27/10):

| categoria | netto | var. long | var. short | var. netto | z52 | pct var. 3a | pct netto | pct short |
|---|---|---|---|---|---|---|---|---|
| Asset Manager | 65.381 | -3.793 | +8.616 | **-12.409** | **-2,06** | **97,4** | 24,4 | 89,1 |
| Leveraged Funds | 2.301 | -578 | -1.929 | +1.351 | 0,11 | 7,7 | 94,9 | 0,6 |
| Dealer | -85.514 | +2.893 | -5.715 | +8.608 | 0,58 | 53,8 | 10,3 | 85,9 |

TFF S&P 500 (`tff_sp500`): Asset Manager +22.072 (z 0,81), Leveraged Funds -29.359 (z -0,62).

## Cosa Dicono Per NQ

- **Livello**: il netto speculativo al 95° percentile, ma il campo estremo e' lo **short al 3,8°**:
  il netto e' alto perche' i corti sono quasi spariti, non per un eccesso di lunghi (64,7°).
  Il campo giusto e' quello estremo, non il netto per forza (`il-framing-e-il-cot-si-leggono-insieme.md`, passo 4).
- **Flusso**: sulla Legacy l'ultima variazione e' ordinaria; lo skew saturo a -1 non conta, conta lo
  z (-0,17). "Piede sull'acceleratore" `[1 · 30:01]`: venditori, ma con il piede appena appoggiato.
- **Cross-confirm** `[1 · 32:31]`: il Nasdaq e' l'unico dei quattro in riduzione. Nessun indice ha
  z oltre 1. La metrica cross-index **non ha retto** alla verifica (`analisi-istituzionale.md`): si
  scrive com'e', non si usa come conferma.
- **Il passo 6** `[1 · 31:53]`, sulla Legacy: il movimento piu' grande delle quattro date e' il **13
  ottobre** (-9.600, z -0,86). Quel giorno la seduta chiude dentro il proprio valore, vicino al bordo
  alto (30.789 contro VAH 30.800), ma tutta **sotto il valore della settimana** (VAL settimanale
  30.846, massimo del giorno 30.814,75). Gli speculatori riducevano mentre il prezzo usciva dal basso
  dal valore della settimana. Il livello e' **30.712-30.800, POC 30.775**.
- **La TFF dice un'altra cosa, e non e' un sinonimo**: gli Asset Manager il 27/10 tagliano -12.409
  (z -2,06, piu' del 97% delle variazioni a tre anni), quasi tutto aprendo corti (+8.616). Quel giorno
  il prezzo chiude a 29.813, **sopra il valore della propria settimana** (VAH settimanale 29.679).
  **Osservazione**: e' la forma dell'esempio del live, distribuzione con il prezzo in alto nel valore;
  dopo, il prezzo ha fatto il massimo del 28/10 (30.009) ed e' sceso a 28.102 (minimo del 2/11).
  Vista una volta qui; nessuna categoria CFTC si lega a un contratto del tape.
- **Comodo o scomodo**: tutte e quattro le date stanno sopra il prezzo di adesso, da 1.400 a 3.173
  punti. I lunghi costruiti in ottobre sono sotto; i Leveraged Funds hanno quasi zero corti (0,6°).
- **Il ritardo in prezzo**: dal 27/10 il prezzo ha fatto -1.456 punti (-4,88%). Il calo del 30/10 e del
  2/11 non e' nel rapporto. Il COT non dice **quando**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.712-30.800, POC 30.775 | valore del 13/10, Legacy, il movimento NC piu' grande delle quattro date | posizionamento: ritorno sul prezzo dove gli speculatori hanno ridotto; oggi a +2.418 punti |
| 29.637-29.880, POC 29.805 | valore del 27/10, TFF, taglio Asset Manager z -2,06 | posizionamento: ritorno sul prezzo dove i gestori hanno aperto corti; oggi a +1.448 punti |
| 29.026-29.679, POC 29.500 | valore della settimana 21-27/10 | il valore della settimana dell'ultimo report; il prezzo ne e' uscito dal basso |
| ven 6/11, 21:30 ora italiana | prossimo COT (dati di martedi' 3/11) | dira' se il taglio dei gestori e' continuato durante il calo |

## Cosa Non Sappiamo

- Il dato ha 7 giorni e tutto il calo di circa 1.450 punti e' successivo: la fotografia e' vecchia in
  prezzo piu' che in giorni.
- **Nessuna soglia COT e' validata**; la "data che diventa livello" e' osservata, non contata.
- Legacy e TFF sono tassonomie diverse sugli stessi contratti: la Legacy dice "movimento piu' grande
  il 13/10", la TFF "il 27/10". Non si fondono.
- Percentili delle variazioni a tre anni (156 settimane), non sul decennio (521) come nelle
  rilevazioni storiche di `analisi-istituzionale.md`: i due numeri non si confrontano.
- Futures only: la variante futures + options darebbe numeri diversi.
- **lacuna del collettore**: l'ora effettiva di pubblicazione del report in uso (venerdi' 30/10) non
  e' in `cot.json`, solo la prossima.
- **lacuna del collettore**: la TFF non riporta Other Reportable e Non Reportable, e manca lo skew di
  fondo per categoria TFF.

## Per Il Quadro

1. Speculatori lunghi al 95° percentile con i corti al 3,8°, gestori in taglio forte (z -2,06): e'
   coerente con il regime della macro, o il calo del 30/10-2/11 sta scaricando proprio quel lato?
2. I livelli del passo 6, **29.805** (27/10) e **30.775** (13/10), da confrontare con i muri delle
   opzioni e con lo zero gamma: coincidono con un muro, o stanno in terreno libero?
3. Il prezzo di adesso, 28.357, e' sotto tutte le date del rapporto: confrontare con la scala dei POC
   dell'order flow (2/11 POC 28.695, 30/10 POC 28.778) per dire quanto spazio c'e' prima del primo
   prezzo di posizionamento.
4. Il cross-confirm (Nasdaq 1 su 4) contro la correlazione NQ-ES della fase gamma: il Nasdaq si muove
   da solo anche nelle opzioni?
