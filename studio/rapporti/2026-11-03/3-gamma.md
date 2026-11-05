# Fase 3 · La Gamma · 2026-11-03

**Stato**: fatto (catena incompleta su alcune scadenze, dichiarata in Cosa Non Sappiamo)
**Dati**: `dati/in-breve.json` (sezione gamma), `dati/gex-NQZ6.json` (solo lo strike 30.000), `dati/macro.json` (VXN e VIX), `dati/raccolta.json` (raccolta chiusa alle 16:47 ora italiana, nessun errore). GEX di `gex.py` dalle 16:45 alle 16:47; future 28.349 alle 16:47, M1 del bridge 28.348,75 (differenza 0,25). GEX viva alle 16:45 (60 secondi prima della raccolta). X-Ray alle 16:45, lettore vivo. VXN e VIX ufficiali (Yahoo) alle 16:31; ricostruiti dalla catena alle 16:46; Cboe QQQ e NDX alle 16:27 (dato con 15 minuti di ritardo). Open interest: quello di ieri sera.

**In una riga**: con il segno che si suppone di solito, i dealer di NQ stanno dalla parte che accelera i movimenti, circa 360 punti sotto il prezzo dove cambierebbero; la loro copertura pero' pesa meno dell'1% del volume del future, e la paura si concentra sulla scadenza di domani, quella della notte elettorale.

## In Parole Semplici

**Il punto**: chi vende le opzioni su NQ (i dealer), se e' messo come si suppone, oggi dovrebbe inseguire il prezzo invece di frenarlo. E' una supposizione, e anche se fosse vera la sua copertura vale meno dell'1% di quello che il future scambia in un giorno.

**Dal piu' ampio al piu' vicino**

1. **La paura**: l'indice della paura sul Nasdaq (VXN) e' a circa 30; le opzioni che scadono domani costano molto piu' di quelle di dicembre (44% contro 26%).
   → il mercato paga per un evento vicino: stanotte ci sono le elezioni di meta' mandato e la trimestrale di AMD.
2. **Tutte le opzioni**: con il segno supposto la gamma totale e' negativa, e il prezzo (circa 28.350) sta circa 360 punti sotto lo zero gamma (circa 28.700).
   → coprendosi, i dealer comprerebbero sui rialzi e venderebbero sui ribassi: un acceleratore, in teoria.
3. **Le opzioni di oggi** (scadono alle 22:00 italiane): fanno quasi meta' della gamma totale, e il loro zero e' a circa 28.565.
   → la mossa attesa fino alla chiusura e' circa 235 punti: quello zero sta dentro il normale di oggi, lo zero di tutte le scadenze no.
4. **Adesso**: nell'ultima mezz'ora prezzo e zero gamma sono saliti quasi insieme (circa 35 e 33 punti); anche la stima di un fornitore sull'S&P 500 dice acceleratore.
   → avvicinarsi non basta: lo zero, nel modello, si sposta con il prezzo.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.700 | lo zero gamma di tutte le scadenze; e' anche il prezzo piu' scambiato della cassa di ieri | sopra, in teoria, i dealer passerebbero a frenare |
| 28.500 | lo strike con piu' put aperte, sopra il prezzo; muro delle put anche per oggi | la zona dove la copertura pesa di piu' sopra il prezzo |
| circa 28.250-28.350 | strike con molte put, sui bordi del valore della notte e della cassa | la zona dove la copertura pesa di piu' attorno al prezzo |

**Quando**: 21:30 ultima mezz'ora della cassa · 22:00 scadono le opzioni di oggi · dopo le 22:00 AMD · notte fra il 3 e il 4 elezioni USA
**Cambia idea se**: il prezzo passa sopra lo zero gamma e ci resta; o una misura del segno vero dei dealer dice il contrario della supposizione.
**Quanto e' provato**: i numeri sono un modello sulle posizioni di ieri sera; il segno dei dealer e' supposto; nessuno studio prova che zero gamma o muri fermino o attirino NQ.

**Le parole**: **dealer** = chi vende le opzioni e si copre sul future; **gamma** = quanto cambia la copertura quando il prezzo si muove; **zero gamma** = il prezzo dove, nel modello, i dealer passano da accelerare a frenare; **strike** = il prezzo fissato di un'opzione; **muro** = lo strike con piu' gamma di call o di put; **put** = opzione che protegge dai ribassi.

## I Fatti

GEX (gamma exposure) = milioni di $ di future che i dealer comprerebbero o venderebbero per un movimento dell'1%, moltiplicatore 20 $ a punto, OI (open interest, le opzioni aperte) di ieri. **Convenzione A** = dealer lunghi di call e corti di put (fornitori, ATAS). **Convenzione B** = dealer corti di tutto [GPP]: dice solo dove pesa. Fonte `gex.py`, 16:45-16:47; distanze dal bridge 28.348,75.

| orizzonte | GEX A | GEX B | zero gamma A | distanza | call wall | put wall | piu' peso in B |
|---|---|---|---|---|---|---|---|
| tutte | -614,6 | -1.928,7 | 28.708 | +360 | 29.330 | 28.500 | 28.500, 28.250, 28.000 |
| entro 7 giorni | -446,5 | -1.458,0 | 28.628 | +279 | 29.330 | 28.500 | 28.500, 28.250, 28.000 |
| oggi (0DTE) | -270,0 | -842,0 | 28.565 | +216 | 28.400 | 28.500 | 28.500, 28.000, 28.250 |

**GEX viva** (XrayLive, convenzione A, 16:45): future 28.339,75; totale -587,4, zero 28.710; 0DTE -281,2, zero 28.566. Nei 32 minuti della finestra: future +35,25, zero +32,7, totale -55,9; 0DTE +14,9. Muri nella finestra: call wall 30.000; put wall 28.000 e 28.500; 0DTE call wall 28.200 e 28.400, put wall 28.000 e 28.500. **Call wall in disaccordo**: `gex.py` 29.330 (GEX call 29,3), la viva 30.000 (GEX call 28,0 in `per_strike`): due strike quasi alla pari, si nominano tutti e due.

| strike (`per_strike`) | distanza | OI call / put | volume call / put | GEX netta A |
|---|---|---|---|---|
| 30.000 | +1.651 | 1.212 / 1.074 | 201 / 24 | +3,3 (call +28,0) |
| 29.330 | +981 | 915 / 0 | 12 / 0 | +29,3 |
| 28.500 | +151 | 300 / 1.867 | 892 / 220 | -90,1 |
| 28.350 | +1 | 42 / 188 | 230 / 108 | -26,1 |
| 28.300 | -49 | 63 / 445 | 422 / 374 | -32,4 |
| 28.250 | -99 | 50 / 363 | 139 / 224 | -44,3 |
| 28.000 | -349 | 17 / 638 | 32 / 620 | -52,3 |
| 27.500 | -849 | 51 / 2.028 | 20 / 620 | -52,3 |

| scadenza | OI | volatilita' ATM | mossa attesa (punti) | quotate | GEX A | zero A |
|---|---|---|---|---|---|---|
| 3/11 (oggi) | 8.006 | 34,1% | 236 | 480/480 | -270,0 | 28.565 |
| 4/11 | 5.475 | 44,2% | 724 | 480/480 | -67,2 | 28.594 |
| 5/11 | 873 | 42,2% | 932 | 243/480 | -19,5 | 29.615 |
| 6/11 | 4.519 | 39,4% | 1.048 | 363/480 | -89,5 | nessuno |
| 27/11 | 3.598 | 27,1% | 1.974 | 214/232 | -5,7 | 28.607 |
| 18/12 (trimestrale) | 34.857 | 25,7% | 2.557 | 189/394 | -137,8 | 29.805 |

Le altre: 10/11 (205/404, -9,3), 13/11 (26/332, OI non stabile, -12,4), 4/12 (192/196, -1,9); quasi vuote 9/11 (35/404), 11/11 (3/404, nessuna GEX), 12/11 (11/386, OI non stabile), 16/11 (2/274). Saltate: 17, 18, 20 e 30 novembre. Mossa attesa = prezzo x volatilita' ATM x radice del tempo alla scadenza.

**Volatilita'**: realizzata di NQ (rendimenti log a 5 minuti, annualizzati 276 x 252) 33,7% sulle ultime 4 ore, 23,5% su 24, 20,2% su 48. VXN ufficiale 30,3 (16:31; -0,59 sul giorno, +21,6% in 5 giorni), ricostruito 29,3 (16:46). VIX ufficiale 20,63, ricostruito 20,1. Cboe (16:27): NDX 30 giorni 29,8, IV30 26,2.

**Scala** (`in-breve.json`, gia' calcolata): convenzione A 1.084 future per un 1% (283,5 punti), cioe' lo 0,23% del volume medio di cassa delle ultime 20 sedute (475.438); convenzione B 3.402 future, lo 0,72%.

## X-Ray (S&P 500, Solo Contesto)

- **Freschezza**: lettore vivo (ultimo giro 7 secondi prima), nessuna famiglia ferma da piu' di 30 minuti; spot SPX 7.571,6 alle 16:45, in cassa, quindi vivo.
- **Segno stimato dei market maker su SPX**: gamma allo spot -219 (acceleratore); GEX netta 0DTE -30,4, scadenza di domani +7,2, mensile del 20/11 -18,6. Unita' di OptionsDepth: conta il segno.
- **Zeri e picchi di SPX** (oggi): zeri 7.380, 7.443, 7.684; picco di freno 7.774, picco di acceleratore 7.581,5, a 10 punti dallo spot. Previsione per domani: zero 7.717, acceleratore 7.434.
- **Delta scambiato** (un flusso, non una posizione): il retail sugli 0DTE ha venduto delta, -3.485 da stamattina, -1.323 negli ultimi 30 minuti; la pressione di copertura 0DTE e' salita da 2.358 a 3.330 in 30 minuti.
- **Contro la convenzione A di NQ**: stesso segno, acceleratore. Ma e' l'S&P 500: non sposta i livelli di NQ e non si converte in prezzi di NQ. Stima di un fornitore, metodo non pubblicato, non provata.

## Cosa Dicono Per NQ

1. **Il regime** (convenzione A): GEX negativa su ogni orizzonte, prezzo sotto tutti e tre gli zeri. In teoria i dealer coprendosi inseguono il prezzo [BDLM §1, BB]; nella sintesi del filone nessuno dei nove paper trova il verso opposto (`quant/paper/ricerche/2026-10-15-gamma-0dte-volatilita-intraday.md`, "Dove convergono"). E' **una supposizione sul segno** (`come-si-leggono-le-opzioni-dal-vivo.md`, passo 3).
2. **La convenzione B**: la copertura pesa di piu' a 28.500, 28.250 e 28.000, tutti put. Il segno vero non si conosce: su SPX GPP trova i dealer corti quasi ovunque (1996-2001), VAPG di solito lunghi (2020-2023). Su NQ nessuno l'ha misurato.
3. **Lo 0DTE** fa il 44% della GEX A totale (-270 su -615) e scade alle 22:00. La sua GEX cambia con il prezzo e con il tempo, non con posizioni nuove: l'OI e' fermo (passo 4). Dopo le 22:00 il peso passa alla scadenza di domani, che ha la volatilita' piu' alta della curva.
4. **Gli zeri si muovono con il prezzo** (passo 7): +33 punti di zero per +35 di prezzo in mezz'ora. **Osservazione**: la distanza dallo zero resta circa 360 punti; e' una conseguenza del modello, non un movimento dei dealer.
5. **La mossa attesa contro la realizzata**: la volatilita' di oggi (34,1%) e' quella che NQ ha realizzato nelle ultime 4 ore (33,7%), molto sopra le 24-48 ore (20-24%). La curva e' invertita: 4/11 al 44,2%, dicembre al 25,7%. **Osservazione**: il mercato paga per un evento vicino; nel calendario ci sono le elezioni e AMD, ma quale dei due pesi non si separa.
6. **La scala**: 0,23% (A) e 0,72% (B) del volume medio di cassa. La copertura di NQ sta sotto l'1% del flusso; in BPVZ (simulazione) sotto il 18% la volatilita' resta nel rumore. L'estrapolazione a NQ e' **nostra** (sintesi, correzione 2), e la GEX e' un tetto: quanto si copre davvero non lo misura nessuno.
7. **I paper, come ipotesi**: BDLM trova il momento dell'ultima mezz'ora (21:30-22:00 italiane) sui future S&P solo nei giorni di gamma negativa (6,63, t 4,78) [BDLM Tab. 7]. Oggi la condizione dell'ipotesi B1 della scheda (`quant/studi/opzioni-gamma-es/1-idea.md`) sarebbe vera, ma e' una registrazione, non una regola: nessuna prova. BB e VAPG: l'effetto sul livello della volatilita' e' piccolo; la sintesi suggerisce che si veda piu' nel segno dell'autocorrelazione dei rendimenti brevi (lettura nostra).

## I Livelli E Le Date

Nessun paper prova questi livelli come supporti o resistenze: sono dove, nel modello, la copertura pesa o cambia verso.

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 29.330 e 30.000 | call wall (`gex.py` e viva), quasi alla pari | posizionamento: dove pesano le call; oltre la mossa attesa di oggi e di domani |
| zona 28.630-28.710 | zero gamma A di 7 giorni e di tutte (viva 28.710); POC della cassa di ieri 28.695 | invalidazione del regime supposto: sopra, in teoria, freno |
| 28.565 | zero gamma dello 0DTE | cambio di segno della gamma di oggi, dentro la mossa attesa (236) |
| 28.500 | put wall (tutte e 0DTE), strike piu' pesante, primo in B | posizionamento: il peso piu' grande sopra il prezzo |
| 28.200 e 28.400 | call wall dello 0DTE, alternati nella finestra | posizionamento dello 0DTE, fino alle 22:00 |
| 28.250-28.350 | strike pesanti di put | posizionamento attorno al prezzo; vedi Per il quadro |
| 28.000 e 27.500 | put pesanti (27.500: 2.028 put aperte) | posizionamento sotto; 28.000 e' anche put wall alternato della viva |
| oggi 21:30-22:00 | ultima mezz'ora della cassa; scadenza 0DTE | la finestra di BDLM; lo 0DTE pesa di piu' |
| oggi dopo le 22:00, notte 3-4 | AMD; elezioni USA | l'evento per cui si paga la scadenza del 4/11 |
| 4/11 16:00 e 22:00 | ISM servizi; scadenza del 4/11 | la volatilita' di domani si scarica |
| 18/12 15:30 | scadenza trimestrale (la piu' carica di OI) | il peso di dicembre nella GEX totale |

## Cosa Non Sappiamo

- Il segno vero dei dealer su NQ: supposto (A) o al peggio (B). L'OI e' di ieri e non vede gli 0DTE aperti e chiusi oggi.
- Quanta gamma diventa davvero ordini sul future: nessuno dei paper lo misura (sintesi, "Il perche' comune").
- Se la volatilita' del 4/11 sia per le elezioni o per AMD.
- **lacuna del collettore**: le scadenze del 17, 18, 20 (la mensile) e 30 novembre sono saltate; la GEX "tutte" non le contiene.
- **lacuna del collettore**: la trimestrale del 18/12, la scadenza con piu' OI (34.857), ha 189 opzioni quotate su 394: il suo -137,8 puo' essere sottostimato. Il 5/11 e il 6/11 hanno 243 e 363 su 480.
- **lacuna del collettore**: `oi_cambiato_rispetto_al_calcolo_precedente` e' vuoto (primo calcolo del giorno) e l'OI del 12/11 e del 13/11 non e' stabile: la stabilita' fra calcoli non e' verificata.
- **lacuna del collettore**: `gex.py` e la viva danno call wall diversi allo stesso minuto (29.330 contro 30.000); `in-breve.json` non dice se e' la parita' degli strike o la rotazione delle scadenze della viva.
- **lacuna del collettore**: `in-breve.json` non porta il confronto con la GEX del giorno prima (`quant/dati/NQ/gex/`): non si dice come si sono mossi zero e totale da ieri.
- L'azzeramento del volume delle opzioni alle 17:00 di Chicago non e' ancora verificato (`volume_finestra`).

## Per Il Quadro

- **Zero gamma A 28.708 contro il POC della cassa di ieri 28.695** (order flow): confluenza fra fonti diverse (`stesso_dato` falso). Nessun livello del COT vicino: le sue confluenze partono da 29.584.
- **Strike pesanti 28.250, 28.300, 28.350 contro il POC della notte 28.250, il VAL di oggi 28.285 e il VAL di ieri 28.340**: tre confluenze gamma-order flow attorno al prezzo.
- **Call wall 29.330 contro il VAH della cassa del 22/10 (29.344)** e i livelli del COT 29.584-29.880, tutti sopra.
- **La curva invertita** (4/11 al 44,2%, VXN +21,6% in 5 giorni) contro il calendario della macro (elezioni, AMD, ISM); e il regime acceleratore supposto, con scala sotto l'1%, contro il CVD dell'order flow (negativo con il prezzo che non fa il minimo).
