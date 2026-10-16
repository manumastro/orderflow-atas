# Fase 3 · La Gamma · 2026-10-15 (sera, raccolta delle 19:52)

**Stato**: parziale (solo NQ; GEX delle 19:30 riusata; settimanali lontane quotate a pezzi)
**Dati**: [gNQ] `gex-NQZ6.json`, calcolo dalle **19:30** ora italiana (17:30 UTC) alle 19:33:58
(`future_quando`), riusato dalla raccolta delle 19:52 (`raccolta.json`): vecchio di circa 18 minuti.
[sed] `sedute-NQZ6.json` (20 sedute 17/09-14/10, cassa di oggi alle 19:52); [mac] `macro.json` (18:43).
Tutto `ok`, nessuna scadenza saltata, due `instabile` (27/11, 30/11). ES non letto: studio solo NQ.

**In una riga**: con la convenzione dei fornitori la GEX di NQ resta negativa (-1.811,7 milioni di $ per
1%), il prezzo sta sotto i tre zeri gamma, piu' vicini (tutte a 1,60%); il 30.000 resta lo strike piu'
pesante in tutte e due le convenzioni; la copertura vale lo 0,74% del volume medio di cassa per 1%.

Le parole. **Gamma**: quanto cambia il delta di un'opzione se il future si muove. Il **dealer** vende
opzioni e si copre col future. **GEX**: i $ di future che i dealer scambiano per un movimento dell'1%
(qui in milioni); positiva **smorza**, negativa **amplifica**. **Zero gamma**: dove la GEX totale cambia
segno. **Muri**: lo strike con la gamma call o put piu' grande. **0DTE**: la scadenza di oggi, alle 22:00.

## I Fatti

**Convenzione A** (fornitori, ATAS: dealer lunghi call e corti put, GEX = call - put). Distanze (calcolo
mio) dal prezzo del bridge **30.018,75** (M1 delle 19:33:59; future del calcolo 30.021,0) [gNQ].

| NQ | GEX A | zero gamma | prezzo rispetto allo zero | call wall | put wall |
|---|---|---|---|---|---|
| tutte (18 scadenze) | -1.811,7 | 30.498,8 | sotto di 480,1 (1,60%) | 30.000 (154,9) | 30.000 (595,3) |
| entro 7 giorni (al 21/10) | -1.508,2 | 30.440,8 | sotto di 422,0 (1,41%) | 30.000 (113,7) | 30.000 (498,1) |
| 0DTE 15/10 | -713,4 | 30.277,8 | sotto di 259,0 (0,86%) | 30.300 (14,0) | 30.000 (168,1) |

Per scadenza [gNQ]: 16/10 A -528,5, B -2.016,3, zero 30.573,1, muri 30.000; 19/10 A -124,2; 30/10 A
-115,9; 18/12 A -54,9. 0DTE e 16/10 fanno il 68,5% della A (alle 18:43 il 75%) e il 66,4% della B.
**Convenzione B** [GPP]: dealer corti di tutto, GEX negativa ovunque; dice solo **dove** pesa. Il segno
vero dei dealer non si conosce, in nessuna delle due, e le misure dei paper sono su SPX, non sul CME.
Tutte -4.992,2 (30.000, 29.500, 30.100); 7 giorni -3.770,2 (30.000, 30.100, 30.250); 0DTE -1.300,0
(30.000, 30.150, 29.950).

**Strike pesanti** (`per_strike`, +-6%) [gNQ]. OI (open interest): contratti aperti a fine di ieri;
volume del giorno CME dalle 00:00 italiane, senza lato.

| strike | OI call / put | volume call / put | GEX call | GEX put | netta A |
|---|---|---|---|---|---|
| 30.000 | 1.546 / 4.723 | 2.236 / 2.514 | 154,9 | -595,3 | -440,4 |
| 29.500 | 252 / 3.589 | 105 / 2.552 | 12,0 | -197,8 | -185,8 |
| 30.100 | 583 / 334 | 1.997 / 259 | 107,2 | -73,9 | +33,3 |
| 30.500 | 1.370 / 1.299 | 1.497 / 143 | 84,2 | -71,9 | +12,3 |

La scadenza piu' pesante della settimana e' il **16/10** (terzo venerdi', OI 18.581): 30.000 (605 call /
1.280 put), 30.500 (714 / 467), 29.500 (92 / 1.052). Nello 0DTE l'OI maggiore e' su put 29.250 (508).

**Mossa attesa** (vol ATM Black-76, 365 giorni, una deviazione standard) [gNQ] contro la realizzata a 20
sedute, 18,7% (Yahoo, NQ=F alle 18:33) [mac]; VXN 22,7 alla chiusura del 14/10.

| NQ | 15/10 | 16/10 | 19/10 | 20/10 | 30/10 | 18/12 |
|---|---|---|---|---|---|---|
| vol ATM (/ realizzata) | 22,4% (1,20) | 24,35% (1,30) | 17,2% (0,92) | 19,9% (1,06) | 20,8% (1,11) | 20,0% (1,07) |
| mossa | +-113,4 | +-401,7 | +-548,3 | +-706,3 | +-1.270,5 | +-2.511,9 |

**La scala** (calcolo mio): un future vale 20 $ x 30.021 = 600.420 $; 1% sono 300,2 punti. A tutte =
**3.017** contratti per 1%; B tutte 8.314; A 0DTE 1.188. Volume di cassa delle 20 sedute: media 408.436,
massimo 594.818 [sed]. **A tutte e' lo 0,74% della media**, B il 2,04%, A 0DTE lo 0,29%. Oggi la cassa
ha 645.337 contratti alle 19:52, gia' sopra il massimo, e 952.915 con la notte [sed]: lo 0,47% e lo 0,32%.

**Cosa e' cambiato dalle 18:43** (`precedenti/1952/3-gamma.md`). Future 29.975,5 -> 30.021,0. GEX A
-1.747,6 -> -1.811,7, B -4.428,4 -> -4.992,2. Zeri: tutte 30.752,1 -> 30.498,8 (-253), 7 giorni 30.656,9
-> 30.440,8, 0DTE 30.314,3 -> 30.277,8, 16/10 31.070,4 -> 30.573,1 (-497). OI 0DTE 8.894 -> 9.981, 16/10
16.681 -> 18.581. Il segno A non cambia, ma **il calcolo non e' lo stesso** (120 strike per lato invece
di 100, riletture dell'OI, zero cercato su +-8% invece di +-4%: [`le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md),
"La GEX calcolata da noi"): quanto venga dal mercato e quanto dal metodo, da qui non si separa.

## Cosa Dicono Per NQ

1. **Il regime, se vale A**: negativo sui tre orizzonti, prezzo sotto i tre zeri; lo zero dello 0DTE e'
   a 2,3 mosse attese. Per il modello la copertura va nel verso del movimento. **Il segno e'
   un'ipotesi** [GPP; finding 5 di [`opzioni-gamma-es`](../../../quant/studi/opzioni-gamma-es/1-idea.md)].
2. **Il "dove" regge alle due convenzioni, il segno no.** Il 30.000 e' muro e primo peso B ovunque, a
   18,75 punti dal prezzo; sotto 30.100 l'OI e' quasi tutto put, quindi A e B pesano gli stessi strike.
3. **La scala.** BPVZ (simulazione) vede effetti sull'autocorrelazione dei rendimenti brevi con il dealer
   al 18-19% del flusso, non sotto: **nostra estrapolazione**, a 0,74% l'effetto su NQ e' piccolo. DPS:
   la copertura sulle brevissime e' secondaria; la GEX e' un tetto del flusso (**lettura nostra**,
   [sintesi](../../../quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md)).
4. **La volatilita'**: lo 0DTE scende ancora (24,5% -> 22,4%); il 16/10 resta 1,30 volte la realizzata,
   con Warsh alle 05:30 di domani [mac]. **Osservazione**: dopo il 16/10, a OI fermo, la A resta -569,9.
5. **I paper, ipotesi mai provate qui.** BDLM: momento dell'ultima mezz'ora sui future S&P solo con gamma
   negativa (6,63, t 4,78, contro 0,82); oggi lo e' solo in A. BB, VAPG: gamma alta, giorno dopo piu'
   calmo, effetto piccolo. BTW: notte prima del terzo venerdi' +14,3 pb su ES. GJ: pinning (chiusura
   su uno strike pesante) nell'ultima mezz'ora del venerdi', 21,3% contro 15%. Nessuno usa NDX o NQ.

## I Livelli E Le Date

**Riserva per ogni riga**: nessun paper letto prova zero gamma, muri o strike pesanti come supporti o
resistenze (finding 6 della scheda): dicono dove cambia la descrizione, non cosa fa il prezzo. [gNQ, 19:30]

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.573,1 / 30.498,8 / 30.440,8 | zero A di 16/10 / tutte / entro 7 giorni | posizionamento: sopra, la A di quell'orizzonte cambia segno |
| 30.277,8 | zero A, 0DTE | posizionamento: sopra, lo 0DTE diventa positivo; vale fino alle 22:00 |
| 30.100 | primo strike pesante con netta A positiva | posizionamento: da qui in su A e B divergono |
| 30.000 | muri di tutte, settimana, 16/10; primo peso B | posizionamento: il peso massimo in tutte e due le convenzioni |
| 29.500 | secondo peso B; 3.589 put di OI | posizionamento: la concentrazione di put piu' grande sotto |
| 15/10 21:30-22:00 | ultima mezz'ora; alle 22:00 scade lo 0DTE | finestra di BDLM; esce -713,4 di A; poi la notte di BTW |
| 16/10 21:30-22:00 | ultima mezz'ora e scadenza del terzo venerdi' | finestra di GJ; esce -528,5 di A |

## Cosa Non Sappiamo

- **Completezza** (`quotate`/`opzioni`): intere 15/10, 16/10, 19/10 (478/480), 18/12 (339/362), 20/10
  400/480. A pezzi le settimanali dal 21/10 (21/10 132/422, 30/10 126/432, 29/10 45/258): pesano il
  **15,6% della A** e il 14,7% della B (calcolo mio; alle 18:43 il 7,4%, su meno scadenze).
- **OI**: 27/11 e 30/11 crescevano ancora (`oi_stabile` falso), meno dell'1% della B; contro le 19:16
  l'OI totale e' +1,34%. **Prezzo**: alle 19:52 la cassa e' a 30.073,75 [sed], 55 punti sopra il calcolo.
- lacuna del collettore: data e ora dell'OI della borsa; l'OI oltre i 120 strike per lato (15/10-20/10);
  nessuno zero su +-8% per 20/10, 27/10, 29/10; azzeramento del volume opzioni alle 17:00 di Chicago non
  verificato; realizzata intraday e VXN di oggi assenti; la variabile di BDLM (rendimento dalla chiusura
  di ieri, 30.185, alle 21:30) non c'e' alle 19:52.
- Non controllato: `per_scadenza` oltre il 20/10, il giro e il pannello. La GEX ha un giorno di storia.

## Per Il Quadro

1. **NQ 30.000**: dentro il valore di cassa di oggi (29.800-30.101 [sed]), 82 sotto il VAL di ieri. E l'order flow?
2. **Gli zeri** di tutte (30.498,8) e della settimana (30.440,8), intorno al VAH di ieri (30.478): e i COT?
3. **Gamma A negativa e scala piccola**, cassa gia' sopra il massimo di 20 sedute: e l'order flow? Il
   16/10 a 1,30 volte la realizzata: coerente con il regime dei tassi della macro?
