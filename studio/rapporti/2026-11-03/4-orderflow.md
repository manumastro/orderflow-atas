# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-11-03

**Stato**: parziale (manca la tab 4, Esecuzione: le barre range non sono registrate nel bridge; mancano `in-breve.json`, `giro.txt`, `confluenze.json` e `raccolta.json`, non ancora scritti dalla raccolta in corso)
**Dati**: `pannello.json` (16:46 ora italiana, 15:46Z), `sedute-NQZ6.json` (ultimo prezzo 16:46:56), `tab-ruoli.json`, `quattro-tab.txt` (GEX delle 16:45), `ore-NQZ6.json` (ultima barra aperta, 47 minuti), `range-NQZ6.json` (vuoto), `cot.json` solo per la data del report. Ora della raccolta: non nota, `raccolta.json` assente. Cassa di oggi 15:30-22:00 ora italiana (09:30-16:00 New York, 14:30-21:00Z: New York e' in ora solare dal 1 novembre).

**In una riga**: dopo quattro sedute di valore in discesa, la cassa di oggi costruisce un valore stretto sul bordo basso del blocco di venerdi' e lunedi'; i venditori aggressivi prevalgono ma il prezzo sale, e l'indicatore misura un mercato choppy e lento.

## In Parole Semplici

**Il punto**: NQ e' sceso di circa 1.500 punti in quattro sedute e oggi si e' fermato. Il prezzo sta proprio sul bordo basso della zona dove venerdi' e lunedi' si e' scambiato di piu'. Per ora il mercato va avanti e indietro, senza scegliere.

**Dal piu' ampio al piu' vicino**

1. **I tre mesi**: il prezzo sta sotto tutta la zona dove si e' scambiato di piu' da settembre (che finisce in basso a circa 29.000).
   → E' "a sconto": in basso nella curva dei prezzi recenti.
2. **La settimana**: da mercoledi' 28/10 il valore di ogni giorno e' sceso, due volte con un salto netto.
   → Chi vende ha spostato il prezzo, e il mercato ha accettato prezzi piu' bassi.
3. **Ieri**: lunedi' ha chiuso a circa 28.140, sotto la propria zona di valore, con la vendita aggressiva piu' forte delle ultime 20 sedute.
   → La rottura verso il basso e' arrivata in chiusura.
4. **La notte**: ha scambiato fra circa 28.040 e 28.430, centrata a 28.250, con compratori e venditori in pari.
   → Il ribasso di ieri non e' continuato.
5. **Adesso (16:46)**: la cassa costruisce valore fra 28.285 e 28.358; il prezzo, 28.341, sta sul bordo basso di ieri (28.340). Le vendite aggressive prevalgono ma il prezzo sale.
   → Qualcuno forse assorbe le vendite; l'indicatore dice mercato choppy e ritmo lento.

**I prezzi da guardare**

| prezzo | cos'e' | se ci arriva |
|---|---|---|
| circa 28.340 | il bordo basso della zona di venerdi'-lunedi' | sopra e con scambi, il prezzo rientra nella zona; sotto, ne resta fuori |
| circa 28.700-28.780 | i prezzi piu' scambiati di lunedi' e venerdi' | e' il centro della zona da cui si e' scesi |
| circa 28.040-28.115 | il minimo e il bordo basso della notte | sotto, il prezzo esce da tutto cio' che e' stato costruito |

**Quanto e' provato**: valori, prezzi e regime sono misurati dal bridge e dall'indicatore. "Qualcuno assorbe" e' un'ipotesi dell'indicatore, con il punto di domanda. Che dal bordo si torni al centro e' il modello del corso, non provato su NQ.

**Le parole**: **valore** = la fascia dove passa il 70% del volume della seduta; **sconto** = prezzo sotto il valore dei mesi; **choppy** = prezzo che va su e giu' senza andare da nessuna parte; **vendita aggressiva** = chi vende subito al prezzo di chi compra (delta negativo).

## I Fatti

**Tab 1 · Composito** (POC = prezzo piu' scambiato; VAL e VAH = bordi basso e alto del valore al 70%)

| composito | VAL | VAH | POC | zona | fonte |
|---|---|---|---|---|---|
| 90 giornate Daily, 24 ore, grezzo, dal 30/06 | 28.520 | 30.275 | 29.517,50 | discount | `tab-ruoli.json` |
| lo stesso aggiustato al rollover (+299,25 prima del 11/09) | 29.015 | 30.640 | 29.837,50 | discount | `tab-ruoli.json` |
| sola cassa, 20 sedute dal 06/10 | 28.996 | 30.935 | 29.840 | discount | `sedute-NQZ6.json` |
| sola cassa "90", in realta' 43 sedute dal 03/09 | 28.998 | 30.834 | 29.735 | discount | `sedute-NQZ6.json` |

Prezzo 28.341,50 alle 16:46: circa 655 punti sotto il VAL del composito di sola cassa.

**Tab 2 · Settimana** (sola cassa, `sedute-NQZ6.json`; shift alla lettera del live: VAL **e** VAH piu' bassi)

| seduta | valore | POC | chiude | shift | staccato | volume | delta |
|---|---|---|---|---|---|---|---|
| lun 26/10 | 29.349-29.570 | 29.495 | dentro | su | si' | 458.549 | -4.001 |
| mar 27/10 | 29.637-29.880 | 29.805 | dentro | su | si' | 400.375 | -565 |
| mer 28/10 | 29.805-29.904 | 29.844 | sopra | su | no | 435.142 | -170 |
| gio 29/10 | 29.482-29.621 | 29.560 | dentro | giu' | si' | 526.843 | +751 |
| ven 30/10 | 28.446-28.857 | 28.778 | dentro | giu' | si' | **754.747** | +4.773 |
| lun 02/11 | 28.340-28.891 | 28.695 | **sotto** (28.137,75) | nessuno (allargato) | no | 537.136 | **-7.202** |
| mar 03/11, in corso | 28.285-28.358 | 28.313 | dentro | giu' | no | 149.411 | -2.821 |

Venerdi' ha il volume piu' alto e lunedi' il delta piu' negativo delle 20 sedute. Profilo di lunedi' a fasce da 10 punti (solo quelle sopra il 2% del volume): il nodo sta a 28.650-28.740 (picco 4,7% a 28.690-28.700); fra 28.330 e 28.560 nessuna fascia arriva al 2%.

**Tab 3 · Seduta**

| finestra | apertura | massimo | minimo | ultimo | POC | valore | volume | delta | fonte |
|---|---|---|---|---|---|---|---|---|---|
| notte, 00:00-15:30 italiane | 28.201 | 28.426,50 | 28.038,25 | 28.266,50 | 28.250 | 28.116-28.304 | 198.678 | -220 | `sedute` |
| cassa, dalle 15:30, in corso | 28.266 | 28.389 | 28.215,75 | 28.341,50 | 28.313 | 28.285-28.358 | 149.411 | -2.821 | `sedute` |

- Il massimo della notte cade nella barra oraria delle 14:00 italiane (da 28.228 a 28.426,50 e ritorno a 28.317), il minimo in quella delle 08:00 (`ore-NQZ6.json`).
- Delta per mezz'ora in cassa: 15:30 -1.349, 16:00 -951, 16:30 -371 (aperta), mentre il prezzo va da 28.266 a 28.361 (`quattro-tab.txt`).
- **CVD** (delta cumulato dall'apertura delle 15:30): -2.821 su 149.411 lotti (-1,9%), al 0% del suo range con il prezzo al 73% del suo (pannello, 16:46).
- **Regime CHOPPY**, finestra ultime 30 barre 16:17-16:46: efficienza 0,00 (netto 0,75 punti su strada 816,50), 4 rotture rientrate, copertura si' (pannello).
- **Velocita'** (proxy, non la speed of tape): barra in formazione 1.199 lotti, 19 percentile su 60 barre, "morta"; barra chiusa delle 16:45 1.623 lotti, 56 percentile. 2 big trade da 60+ lotti in 10 barre, netto -168 (pannello).
- **In gioco**: il VAH di cassa in sviluppo, 28.353, 11,50 sotto, arrivato da sotto. **Veti**: 1, il regime choppy (pannello).
- Ai livelli dalle 15:30: POC di cassa 28.313, 37 tocchi (16 tiene, 21 passa), delta -163; VAH 28.353, 17 tocchi (11 tiene); mensola europea 28.326, 34 tocchi, delta -87 (pannello).
- Volatilita' realizzata, rendimenti a 5 minuti annualizzati: 4 ore 33,7%, 24 ore 23,5%, 48 ore 20,2% (`ore-NQZ6.json`). Dalla chiusura di ieri: +203,75 punti, +0,72% (15:46Z).

**Tab 4 · Esecuzione**: non disponibile (`tab-ruoli.json`: `esecuzione` nullo; `range-NQZ6.json` vuoto).

Il pannello stampa anche righe operative (verso, riferimenti di prezzo, size): per regola non si riportano.

## Cosa Dicono Per NQ

1. **Dove si sta nella curva** (passo 5 di `profile-framing.md`; premium e discount, sezione 10 di `i-tre-modelli-del-live-q1.md`, `[5 · 55:05]`): in discount su tutti e quattro i compositi. Il live usa la posizione nella curva come veto: "we are really low in the curve" `[5 · 1:01:05]`. Regola del corso, non provata qui.
2. **I profili si uniscono** (passo 3, `[1 · 24:02]`): venerdi' e lunedi' si sovrappongono; il blocco e' 28.340-28.891, con due POC, 28.778 e 28.695. Il prezzo, 28.341,50, sta sul VAL del blocco: 1,50 punti dentro.
3. **Il valore si sposta giu'** (passo 2, `[1 · 22:13]`): shift giu' su 29/10, 30/10 e oggi in corso; lunedi' si e' allargato. Ma il valore di oggi tocca quello di lunedi' (28.340-28.358): non e' staccato. Sul bordo, con il valore che scende, il passo 5 indica il modello momentum; dentro, mean reverting. Oggi i due casi coincidono sul bordo, e la misura non sceglie. "Il VAL e' una strada" `[1 · 1:19:30]`.
4. **Il POC della seduta di rottura** (`[1 · 24:35]`): due candidati, dichiarati. Venerdi' 30/10, la seduta staccata che ha lasciato il valore di fine ottobre (POC 28.778); lunedi', che ha chiuso sotto il proprio valore (POC 28.695).
5. **Il vuoto**, fra il VAH di una seduta e il VAL di un'altra: fra 28.891 (VAH di lunedi') e 29.482 (VAL di giovedi'). Nessuna cassa della settimana ha scambiato fra 28.891 e 29.405 (minimo di giovedi'): la discesa e' avvenuta di notte. **Ma non e' vuoto nel mese**: 22 e 23/10 hanno costruito valore a 29.054-29.344 (POC 29.137 e 29.155). La regola di `profile-framing.md` ("uno scaffale, non un vuoto"): e' vuoto della settimana, mensola del mese.
6. **Fascia sottile, non vuoto**: nel profilo di lunedi', 28.330-28.560 sta sotto il 2% per fascia; la parte sopra 28.446 e' pero' dentro il valore di venerdi'. La fascia davvero poco costruita e' 28.340-28.446. Osservazione.
7. **Chi spinge adesso**: delta negativo in ogni mezz'ora della cassa con il prezzo che sale, CVD al minimo con il prezzo al 73%. L'indicatore lo segnala come possibile assorbimento, con il punto di domanda. **Osservazione**, non misura di assorbimento.
8. **Il regime**: choppy con efficienza nulla e ritmo lento. Per il live, nel choppy si legge su 40 range `[2 · 1:16:00]`: e' proprio la tab che manca. La volatilita' delle ultime 4 ore (33,7%) e' sopra quella a 48 ore (20,2%): la mattina, con il picco delle 14:00, e' stata piu' mossa del pomeriggio misurato dall'indicatore.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 29.735 / 29.840 | POC del composito di sola cassa (43 e 20 sedute) | il fair value dei mesi: misura quanto si e' lontani |
| 29.482 / 29.405 | VAL e minimo di cassa di giovedi' 29/10 | il bordo alto del vuoto della settimana |
| 29.054-29.344 | valore di 22-23/10, POC 29.137 / 29.155 | la mensola del mese dentro il vuoto della settimana |
| 28.891 | VAH di lunedi' e del blocco, massimo di cassa di lunedi' | il bordo alto del blocco; sopra comincia il vuoto |
| 28.778 / 28.695 | POC di venerdi' e di lunedi' (sedute di rottura) | il centro del blocco: dove il live dice che si torna dal bordo |
| 28.446 | VAL di venerdi' | dove finisce la fascia sottile di lunedi' |
| 28.340 | VAL di lunedi' e del blocco | il bordo in gioco: dentro o fuori dal blocco |
| 28.285-28.358, POC 28.313 | valore di cassa di oggi, in corso | la bilancia di oggi; il VAH 28.353 e' il livello in gioco del pannello |
| 28.250 / 28.116-28.304 | POC e valore della notte | il valore della notte, sotto quello di oggi |
| 28.215,75 / 28.101,75 / 28.038,25 | minimo di cassa di oggi, di lunedi', della notte | sotto, il prezzo esce da tutto il costruito delle 20 sedute |
| 22:00 ora italiana | chiusura della cassa | la seduta di oggi entra nella settimana; si rilegge lo shift |

## Cosa Non Sappiamo

- lacuna del collettore: la tab 4, Esecuzione (barre range), non e' registrata nel bridge e `range-NQZ6.json` e' vuoto; mancano verso, durata e delta delle ultime barre range e il metro veloce o lenta.
- lacuna del collettore: `in-breve.json`, `giro.txt`, `confluenze.json` e `raccolta.json` non c'erano alla lettura; mancano la struttura oraria sugli ultimi due swing, il tape e le confluenze con il campo che dice se due fonti leggono lo stesso numero.
- lacuna del collettore: il pannello chiama "max cash" 28.426,50, che e' il massimo della notte (barra delle 14:00 italiane, prima delle 15:30); `sedute` da' 28.389 per la cassa. Una delle due finestre e' sbagliata.
- lacuna del collettore: il composito "90" ha 43 sedute, e non dice se le 6 prima del rollover sono aggiustate.
- lacuna del collettore: i big trade non dichiarano da quando copre il registro del tape.
- Il VAL di oggi vale 28.278 (pannello, livelli), 28.284 (pannello, valore) e 28.285 (`sedute`): 7 punti, valore in corso. Vale il pannello per il vivo.
- La velocita' e' un proxy sul volume per barra, non la speed of tape. La causa del picco delle 14:00 non e' in questi dati: la dice la macro.

## Per Il Quadro

- **POC di lunedi' 28.695 a 13 punti dallo zero gamma di NQ 28.708** (GEX delle 16:45): due fonti diverse o lo stesso livello? `confluenze.json` manca, il confronto resta da fare.
- **Il muro put 28.500** (tutte le scadenze e 0DTE) sta dentro il blocco, fra il VAL di venerdi' 28.446 e i POC; il muro call 0DTE 28.400 sta fra il massimo di cassa 28.389 e quello della notte 28.426,50.
- **COT al martedi' 27/10**: quel giorno il valore di cassa era 29.637-29.880 (POC 29.805). Da allora il prezzo e' sceso di circa 1.460 punti: il report precede tutta la discesa.
- **Regime choppy e ritmo lento con GEX negativa** (in teoria i coperturisti accelerano i movimenti): il contrasto va spiegato. La volatilita' realizzata a 4 ore (33,7%) e' vicina all'implicita 0DTE (34,1%).
