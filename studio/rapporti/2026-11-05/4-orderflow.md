# Fase 4 · L'Order Flow Delle Quattro Tab · 5 novembre 2026

**Stato**: parziale (manca la tab 4, le barre range: "non registrata nel bridge", `range-NQZ6.json` vuoto)
**Dati**: `in-breve.json` (sezione orderflow), `pannello.json`, `quattro-tab.txt`, `giro.txt`, `range-NQZ6.json`; raccolta delle 16:16 ora italiana (`raccolta.json`, tutte le voci "ok"). Pannello alle 16:16 (15:16 UTC); cassa di oggi in corso da 46 minuti; scalini sull'ultima candela chiusa (4/11). Errori di raccolta: nessuno; lacune sotto.

**In una riga**: NQ e' circa 1.200 punti sotto la chiusura di ieri, quasi 700 persi nella prima mezz'ora di cassa, fuori dal blocco di valore della settimana e molto sotto il valore dei tre mesi; da mezz'ora l'indicatore misura un mercato che gira su se stesso.

## In Parole Semplici

**Il punto**: oggi il valore si e' spostato in basso di colpo, in mezz'ora, lasciando un vuoto di quasi 500 punti sopra. Adesso il prezzo sta al fondo della giornata, e l'indicatore dice che il movimento si e' fermato: va avanti e indietro, lento.

**Dal piu' ampio al piu' vicino**

1. **I mesi**: il prezzo e' circa 1.600 punti sotto la zona dove si e' scambiato di piu' negli ultimi mesi.
   → E' "a sconto": lontano dal prezzo che il mercato ha trattato come giusto finora.
2. **La settimana**: dal 30/10 al 4/11 il valore e' rimasto in un blocco fra circa 28.300 e 28.900; oggi si e' staccato sotto.
   → Il valore scende (lo "shift" del metodo); fra il blocco e oggi resta una fascia quasi senza scambi.
3. **La notte**: fino alle 15:30 il prezzo era rimasto vicino al blocco, fra 28.170 e 28.670, con piu' acquisti che vendite aggressive.
   → La rottura e' della cassa: nella prima mezz'ora oltre 176.000 contratti e vendite aggressive nette.
4. **Adesso**: prezzo a pochi punti dal minimo del giorno; l'indicatore misura "choppy" e velocita' "morta".
   → Dopo la discesa le ultime 30 barre a un minuto rompono e rientrano: nessuna spinta misurata.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 27.720 | il prezzo piu' scambiato di oggi | il prezzo torna al centro della giornata |
| circa 27.300 | il minimo di oggi, che coincide col bordo basso del valore | il valore di oggi si allunga ancora in basso |
| 27.830-28.300 | il vuoto fra oggi e il blocco della settimana | si vede se il mercato lo riattraversa in fretta o si ferma al bordo |

**Quanto e' provato**: i numeri sono misurati dall'indicatore e dal collettore. L'idea che un vuoto "si riempia" e' del live di Fabio, non provata su NQ; gli scalini sono smentiti come previsione (NQ 2017-2024).

**Le parole**: **valore** = la fascia dove si e' scambiata la maggior parte del volume; **POC** = il prezzo piu' scambiato; **sconto** = sotto il valore di lungo periodo; **shift** = valore con bordo alto e basso entrambi piu' bassi di ieri; **choppy** = movimento che va e torna senza strada netta; **delta** = acquisti aggressivi meno vendite aggressive.

## I Fatti

**Tab 1, composito** (`in-breve.orderflow`; barre Daily di 24 ore per la tab, sola cassa per i due compositi delle sedute)

| composito | VAL (bordo basso) | VAH (bordo alto) | POC | zona |
|---|---|---|---|---|
| tab 1, 90 barre Daily dal 02/07, grezzo | 28.415 | 30.255 | 29.517,50 | discount |
| tab 1, aggiustato al rollover (+299,25 prima dell'11/09) | 28.995 | 30.740 | 29.817,50 | discount |
| sola cassa, 45 sedute dal 03/09 (6 prima del rollover) | 28.998 | 30.908 | 29.735 | discount |
| sola cassa, 20 sedute dal 08/10 | 28.996 | 31.002 | 29.840 | discount |

Scalini (`orderflow.scalini`, ultima candela chiusa, chart NQ Daily, giorno di borsa CME): mese (ottobre) **pausa**, massimo piu' alto ma chiusura sotto il massimo di settembre, visitato e non accettato; settimana 26-30/10 **pausa**, stesso motivo; giorno 4/11 **pausa**, minimo piu' basso (27.615,75, swing H1 delle 04:00 del 4/11) ma chiusura 28.646,75 sopra il minimo di prima: visitato, non accettato. Insieme: **misti**. Ping-pong del giornaliero dal 03/11 (2 giornate, dopo lo scalino giu' del 02/11): muro alto 28.969, muro basso 27.615,75.

**Tab 2, settimana** (`sedute-NQZ6` via `in-breve`, sola cassa 15:30-22:00 ora italiana)

| seduta | valore | POC | delta | chiude | valore rispetto a ieri |
|---|---|---|---|---|---|
| 29/10 | 29.482-29.621 | 29.560 | +751 | dentro | giu', staccato, shift giu' |
| 30/10 | 28.446-28.857 | 28.778 | +4.773 | dentro | giu', staccato, shift giu' |
| 02/11 | 28.340-28.891 | 28.695 | -7.202 | **sotto** | giu', nessuno shift |
| 03/11 | 28.296-28.387 | 28.314 | -5.843 | dentro | giu', shift giu' |
| 04/11 | 28.558-28.785 | 28.632 | +744 | dentro | su, staccato, shift su |
| 05/11 in corso | 27.338-27.826 | 27.720 | -2.538 | dentro (ultimo) | **giu', staccato, shift giu'** |

**Tab 3, seduta**

| misura | valore | fonte, ora |
|---|---|---|
| notte (00:00-15:30 ora italiana) | apre 28.663, max 28.668,50, min 28.167,75; POC 28.325, valore 28.256-28.435; vol 207.257, delta +1.217 | `oggi_notte` |
| Europa (livelli sul chart) | valore 28.272-28.390, POC 28.326, min 28.246,75 | `pannello.alLivello` |
| cassa di oggi | apre 28.203,25, max 28.228,25, min 27.303, ultimo 27.357,25; vol 259.070 (media 20 sedute 490.157), delta -2.538 | `oggi_cassa`, 16:16 |
| dalla chiusura di ieri (28.566,75) | -1.209,50 punti, -4,23% | `rendimento_dalla_chiusura_di_ieri`, 15:16 UTC |
| mezz'ore di cassa | 15:30: 28.203,25 -> 27.533,50, range 771, delta -2.541, vol 176.801 · 16:00 (aperta): range 285, delta +254 | `quattro-tab.txt` |
| struttura H1 | ultimi due swing: massimi e minimi decrescenti; ora delle 15: delta -2.448 | `giro.txt` |
| CVD (delta cumulato) dalle 15:30 | -2.526 (-1,0%) su 259.034 lotti; CVD al 18% del suo range, prezzo al 6% | pannello, 16:16 |
| **regime** | **CHOPPY**, ultime 30 barre 15:47-16:16: efficienza 0,19 (netto 346,75 su strada 1.818,25), 5 rotture rientrate, copertura no | pannello, 16:16 |
| **velocita'** (proxy, non la speed of tape) | barra in formazione **morta**, 563 lotti, 14° percentile su 60 barre; barra chiusa 16:15: 4.986 lotti, 42° percentile | pannello, 16:16 |
| big trade | 2 da 60+ lotti in 10 barre, netto +33 | pannello |
| in gioco | nessun livello vicino; sopra 27.720 POC cassa (5,0%, +359,75), sotto 27.303 VAL in sviluppo e minimo (-57,25) | pannello |
| veto | uno, legato al regime choppy (il testo contiene indicazioni operative, non riportate) | pannello |
| volatilita' realizzata (rendimenti a 5 minuti, annua) | 4 ore 49,3% · 24 ore 36,9% · 48 ore 34,2% | `in-breve.gamma.realizzata` |

**Tab 4, esecuzione**: nessun dato. Lacuna sotto.

**Le differenze fra fonti, scritte e non scelte**: (1) due fotografie nello stesso minuto 16:16: `pannello.json` (27.360,25, efficienza 0,19, copertura no, 563 lotti al 14°) e `giro.txt` (27.420,75, efficienza 0,15, copertura si', 3.721 lotti al 24°); la barra e' in formazione, il regime choppy e' lo stesso. (2) Valore di oggi: riga VALORE 27.337-27.826, livelli sul chart VAL 27.303 e VAH 27.887, sedute 27.338-27.826. (3) Ieri: livelli sul chart 28.563/28.815/POC 28.630, sedute 28.558/28.785/POC 28.632 (per le sedute chiuse vale `sedute`). (4) Notte: chart VAL 28.184, VAH 28.410, POC 28.269; `oggi_notte` 28.256/28.435/POC 28.325 (dal vivo vale il pannello).

## Cosa Dicono Per NQ

- **Composito**: tutti e quattro i compositi mettono il prezzo in discount, circa 1.600 punti sotto il VAL di sola cassa (28.998). Il live: premium e discount si leggono rispetto al picco di volume, e la posizione nella curva e' un veto, non un permesso (`i-tre-modelli-del-live-q1.md`, sezione 10, `[5 · 55:05]`, `[5 · 1:01:05]`).
- **Settimana, i profili si uniscono** (`profile-framing.md`, passo 3): 30/10, 02/11, 03/11 e 04/11 hanno valori sovrapposti e formano un blocco 28.296-28.891. Oggi il valore e' **staccato** sotto il blocco: shift giu' alla lettera (VAL e VAH piu' bassi). La seduta che ha creato la rottura e' oggi: il suo POC, 27.720, e' il secondo POC che conta (`[1 · 24:35]`), ancora in sviluppo.
- **Il vuoto**: fra il VAH di oggi (27.826) e il VAL del 03/11 (28.296), circa 470 punti. **Osservazione**: nel profilo di oggi nessuna fascia da 10 punti sopra il 2% del volume fra 27.720 e 28.200, e nel profilo della finestra (dalle 23:00 del 4/11) nessuna fascia da 100 punti sopra l'8% fra 27.700 e 28.200. La seduta piu' recente che l'ha visitato e' oggi, e l'ha attraversato in mezz'ora: e' transito, non uno scaffale (la regola di `profile-framing.md`, "Sotto il VAL c'e' uno scaffale").
- **Dove si sta nella curva** (passo 5): dentro il valore di oggi, sotto il POC, a pochi punti dal VAL che coincide col minimo. Sul bordo, con il valore che si sposta: e' la situazione che il live descrive come momentum. **Ma il regime misurato e' choppy**: nelle ultime 30 barre il prezzo ha fatto 1.818 punti di strada per 347 di netto, con 5 rotture rientrate. Le due letture si dichiarano tutte e due: la struttura della giornata e' di spostamento, il ritmo degli ultimi 30 minuti no.
- **Chi spinge**: la notte aveva delta positivo (+1.217); la rottura e' venduta in cassa (-2.541 nella prima mezz'ora). Dopo, la mezz'ora delle 16:00 ha delta +254 e il CVD sta piu' in alto nel suo range (18%) del prezzo nel suo (6%): **osservazione**, i nuovi minimi arrivano con meno vendita aggressiva netta. Non e' un segnale.
- **Scalini**: tutti in pausa, misti. Il prezzo oggi scambia sotto il muro basso del ping-pong (27.615,75), che e' il minimo della notte del 4/11. Per la regola del collettore il ping-pong e' rotto solo a chiusura di giornata CME oltre il muro. Si legge come descrizione: allineati o misti non dicono il verso (`quant/studi/scala-dei-tre-tempi`, smentito).

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 29.735 | POC del composito di cassa (45 sedute) | posizionamento: il centro del valore di lungo periodo |
| 28.998 | VAL del composito di cassa | sopra si esce dal discount |
| 28.969 | muro alto del ping-pong (max cassa 4/11) | posizionamento del giornaliero |
| 28.778 · 28.695 · 28.632 | POC 30/10, 02/11, 04/11 (scala sopra) | i POC del blocco della settimana |
| 28.296-28.891 | il blocco unito 30/10-04/11 | rientrarci vuol dire tornare nel valore della settimana |
| 28.325 · 28.314 | POC della notte e del 03/11 | i primi POC sopra il vuoto |
| 27.826-28.296 | il vuoto (VAH di oggi, VAL del 03/11) | si vede se viene riattraversato in fretta o respinto al bordo |
| 27.720 | POC di oggi, la seduta della rottura | il centro della giornata; un rientro li' e' un ritorno al valore di oggi |
| 27.615,75 | muro basso del ping-pong (min notte del 4/11) | una chiusura di giornata CME sotto rompe il ping-pong |
| 27.303 | minimo di cassa e VAL in sviluppo (pannello) | sotto, il valore di oggi si allunga ancora |
| 22:00 ora italiana | chiusura della cassa | il valore di oggi si fissa |
| 23:00 ora italiana | chiusura del giorno CME | la candela che decide lo scalino e il ping-pong |

Sotto il prezzo nessun POC di sedute recenti: la scala sotto e' vuota nei dati raccolti.

## Cosa Non Sappiamo

- **lacuna del collettore: la tab 4 (Range) non e' registrata nel bridge**; `range-NQZ6.json` e `orderflow.barre_range` sono vuoti. Niente verso, durata e delta delle barre range, niente percentili: il ritmo si legge solo dalla proxy del pannello.
- **lacuna del collettore: il composito di cassa ha 45 sedute, non le 90 del live**; l'archivio non arriva a tre mesi. Non e' dichiarato se le 6 sedute prima del rollover siano aggiustate.
- **lacuna del collettore: il delta per fascia del profilo di oggi** (`profilo_da_10` ha solo la quota): non si puo' dire chi ha difeso o attraversato ogni fascia, come fa il live.
- **lacuna del collettore: Londra non ha volume e delta suoi**, solo i livelli "Europa" sul chart.
- I livelli del chart e quelli delle sedute differiscono di 2-90 punti (sopra); il perche' (fasce, finestra della notte) non e' nei dati.
- La seduta e' in corso da 46 minuti: valore, POC e regime cambiano; la lettura vale alle 16:16.

## Per Il Quadro

- **La gamma**: regime acceleratore (GEX negativa su tutte le scadenze, fase 3). Il call wall 0DTE 27.750 sta a 30 punti dal POC di oggi 27.720; il put wall 0DTE 27.000 e il put wall entro 7 giorni 27.500 stanno sotto il minimo. L'acceleratore e lo shift giu' vanno nello stesso senso; il regime choppy degli ultimi 30 minuti no: da confrontare.
- **Confluenza** (`confluenze.json`): VAL cassa del 03/11 28.296 e call wall entro 7 giorni 28.300, dati diversi: e' il bordo alto del vuoto.
- **Il COT**: confrontare il discount di circa 1.600 punti sotto il composito con il posizionamento della fase 2 e la data del suo report.
- **La macro**: -4,2% dalla chiusura di ieri, di cui circa 670 punti nella prima mezz'ora di cassa, con volume oltre la meta' della media giornaliera: la fase 1 ha un evento che la spiega? In calendario oggi solo i sussidi delle 14:30.
