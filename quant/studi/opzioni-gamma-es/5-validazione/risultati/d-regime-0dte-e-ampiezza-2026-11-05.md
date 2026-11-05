# Ipotesi D Ed E, Descritte Sulla Storia Che C'e' (5 Novembre 2026)

**Cos'e'**: una **descrizione**, non una prova di strategia. Nessun ingresso, nessun costo, niente
registro delle prove: il registro conta le varianti di una strategia, e qui non c'e' una strategia. Le
ipotesi e il perche' stanno nella [scheda](../../1-idea.md), "Ipotesi D" e "Ipotesi E".

Lo script: [`3-codice/regime_0dte_e_ampiezza.py`](../../3-codice/regime_0dte_e_ampiezza.py).

```bash
python quant/studi/opzioni-gamma-es/3-codice/regime_0dte_e_ampiezza.py --json <scratchpad>/d.json
```

## In Breve, Per Chi Guarda Il Prezzo

- **Il video dice**: quando lo 0DTE dei market maker e' in gamma negativa la giornata e' piu' larga,
  circa un terzo (70 punti di SPX contro 51, in 101 sedute fra aprile e settembre) [V2 · 1:39-2:00].
- **Sui nostri 42 giorni, con il regime noto alle 9:35, viene il contrario**, e comunque niente che
  il caso non faccia da solo:
  - nelle 16 sedute con lo 0DTE negativo alle 9:35, il resto della giornata si e' mosso in mediana per
    **0,55** volte il movimento atteso;
  - nelle 26 sedute positive, **0,74** volte;
  - un rimescolamento a caso delle etichette da' una differenza cosi' o piu' grande il **24%** delle
    volte.
- **La direzione non cambia col regime**: il video lo dice, e qui si vede lo stesso. Il nullo da' una
  differenza cosi' il 37% delle volte.
- **Su 42 sedute un effetto piccolo non si vede ne' si esclude, ma uno grande come quello del video e'
  poco compatibile con i nostri dati** (sotto, "I limiti"). Non e' una smentita del video: lui misura
  in un altro modo (sotto, "La trappola"), e su un altro periodo.
- **I muri al bordo del valore** (ipotesi E): dal 23 ottobre un solo caso. Il 3/11 il muro put era
  vicino al VAL del giorno prima, la gamma era negativa, e il muro ha tenuto. E' un aneddoto: non conta.

## I Dati

| | cosa | da dove |
|---|---|---|
| regime | **GEX netta dei market maker sullo 0DTE** di SPX (call + put, tutti gli strike), stimata da OptionsDepth | X-Ray, `quant/dati/xray/series/SPX/gex__mm_0dte/`, colonna piu' recente fra le 09:30 e le 09:35 di New York |
| prezzo | ES M1 | `quant/dati/ES/m1/` (continuo, fino al 23/10) poi il chart ESZ6 M1 del bridge |
| movimento atteso | **VIX ufficiale di chiusura del giorno prima** × F / √252, con F = ES alle 9:35: una deviazione standard di un giorno intero | Yahoo `^VIX`, salvato in [`vix-giornaliero.json`](vix-giornaliero.json) |
| movimento atteso, controllo | la deviazione standard dei rendimenti di cassa di ES delle 20 sedute prima × F | archivio ES |
| valore (per E) | VAL e VAH della cassa del giorno prima | ruolo della tab M30 di ESZ6 (lo calcola l'indicatore) |

**Gli orari** sono quelli della cassa di New York, 09:30-16:00, convertiti per data con
`FabioOrderFlow/tools/sedute.py`. Prima del 1 novembre sono le 13:30-20:00Z, dopo le 14:30-21:00Z.

**Le sedute**: 47 giornate con un file di X-Ray, dal 31/08 al 04/11.
- **5 escluse**, dal 31/08 al 04/09, perche' nessuna colonna di X-Ray cade fra le 09:30 e le 09:35:
  la copertura dipende da quando l'indicatore era aperto su un chart ES.
- **Restano 42 sedute.** Quella di oggi, il 5/11, non e' finita e non entra.

**Le misure**:
- **ampiezza**: il massimo meno il minimo di ES dalle 09:35 alle 16:00;
- **rapporto**: ampiezza diviso il movimento atteso;
- **rendimento**: il logaritmo di (chiusura delle 16:00 / ES delle 09:35), in punti base;
- **nullo**: 10.000 rimescolamenti delle etichette del regime (seme 20261105), p a due code sulla
  differenza fra negativo e positivo.

## I Numeri

**D1, il regime alle 9:35** (la misura principale, dichiarata prima di guardare):

| | n | rapporto mediano | rapporto medio | ampiezza mediana (punti ES) | rendimento mediano | rendimento medio |
|---|---|---|---|---|---|---|
| 0DTE positivo | 26 | 0,737 | 0,748 | 59,25 | −10,3 pb | −3,8 pb |
| 0DTE negativo | 16 | 0,553 | 0,699 | 44,75 | +9,0 pb | −2,1 pb |
| negativo − positivo | | −0,184, **p 0,24** | −0,049, p 0,63 | | +19,3 pb, **p 0,37** | |

Col movimento atteso dalla realizzata a 20 sedute, al posto del VIX, il rapporto mediano e' 1,20 nei
giorni positivi e 0,95 nei negativi (differenza −0,246, p 0,20). Il verso non cambia.

**D2, il regime della GEX entro l'1% dallo spot**: rapporto mediano 0,725 (24 sedute) contro 0,553
(18). La differenza della mediana e' −0,172 con p 0,28; quella della media −0,056 con p 0,57.

**La trappola: il regime "per gran parte della giornata", come nel video.** Una seduta e' negativa se
lo 0DTE e' negativo in almeno meta' dei minuti di cassa.

| | n | rapporto mediano | rapporto medio |
|---|---|---|---|
| giornata positiva | 31 | 0,672 | 0,686 |
| giornata negativa | 11 | 0,577 | **0,850** |
| negativo − positivo | | −0,095, p 0,70 | **+0,164, p 0,14** |

- Contato sulla giornata intera, **la media va nel verso del video**: le giornate negative sono piu'
  larghe.
- Contato alle 9:35, va nel verso opposto.
- Il regime delle 9:35 e quello della giornata coincidono nel 79% delle sedute.

Le giornate che cambiano etichetta sono proprio quelle che si sono mosse:
- **il 15/10** era positivo alle 9:35, negativo nel 91% dei minuti, rapporto 1,03;
- **il 30/10** era positivo alle 9:35, negativo nel 58% dei minuti, rapporto 1,34.

Il prezzo si e' allontanato dagli strike, e la gamma e' diventata negativa **dopo**. E' il
meccanismo della trappola, visto su due giorni: non e' una prova, ma e' la ragione per cui la
misura giusta usa solo il regime noto all'apertura.

**Le giornate piu' larghe** (rapporto sopra 1,2) non stanno da una parte sola:
- tre erano positive alle 9:35: 16/09 (1,49), 14/10 (1,28), 30/10 (1,34);
- due erano negative: 16/10 (1,49) e 02/11 (1,31).

**E, i muri al bordo del valore, dal 23/10** (convenzioni nella scheda):
- un caso solo in 9 sedute;
- il 03/11 il muro put 7.570 di SPX, cioe' 7.599,05 di ES con la base 29,05, stava a 3 punti dal
  VAL del 2/11 (7.596). Il prezzo l'ha toccato alle 09:45 di New York arrivando da sotto, con lo 0DTE
  negativo, e **ha tenuto**: nessuna chiusura a 5 minuti oltre 3 punti nell'ora dopo;
- il regime diceva "rompe", ed e' andata al contrario. Uno su uno non e' un risultato.

## Cosa Si E' Cambiato Dopo Aver Guardato, E Perche'

- **La base ES-SPX per E.** Alla prima esecuzione la base era una coppia sola alle 9:35, e il 4/11
  dava 20,4 punti invece dei soliti 29-30, perche' i minuti erano veloci. Ora e' la mediana della
  seduta, minuto per minuto. Era un errore di misura, non una variante. Ma va detto: con la base
  vecchia il caso del 3/11 stava a 4,1 punti e restava fuori dalla soglia di 4, con la nuova sta a 3,05.
- **Il nullo sulla media** per D2 e per la trappola si e' aggiunto dopo la prima esecuzione. Per D1 c'era gia'.
- Nient'altro: ora delle 9:35, soglie, movimento atteso e definizioni sono quelli scritti nello script
  prima della prima esecuzione.

## I Limiti

- **42 sedute sono poche, ma non pochissime.** La deviazione standard del rapporto e' 0,32 su una
  media di 0,73. Un allargamento di un terzo nei giorni negativi, come nel video, vorrebbe dire circa
  +0,25 di rapporto. Con 26 e 16 sedute l'errore standard della differenza fra le medie e' circa 0,10,
  e la differenza osservata e' −0,05: **tre errori standard sotto** quello che il video farebbe
  aspettare. Misurato cosi' (regime alle 9:35, ampiezza sul movimento atteso), un effetto grande come
  quello del video e' poco compatibile con i nostri dati. Un effetto piccolo, come quello mediano
  di [VAPG], non si esclude.
- **La copertura di X-Ray** dipende da quando l'indicatore era aperto su un chart ES. Una seduta senza
  colonne all'apertura sparisce, e non a caso: sparisce se quella mattina ATAS non era aperto.
- **Il segno e' una stima di OptionsDepth**, con un metodo non pubblicato; X-Ray su SPX, il prezzo su ES.
- **Il periodo e' uno solo**, da settembre a inizio novembre 2026, con il VIX fra 14 e 21. Un mercato
  piu' agitato puo' dare un'altra risposta.
- **Il movimento atteso dal VIX** e' a 30 giorni, non quello dello 0DTE: lo straddle 0DTE del video
  non c'e' nello storico. Per questo c'e' il controllo con la realizzata, e il verso e' lo stesso.

## Da Qui In Avanti

Queste sedute sono tutta la storia di X-Ray, quindi **la prova vera e' in avanti**: le sedute dal 6
novembre 2026, con le stesse regole, mai guardate prima. Lo si scrive ora, prima di vederle:
- **D1 regge** se, quando le sedute nuove con il regime alle 9:35 arrivano a **60**, il rapporto
  mediano dei giorni negativi supera quello dei positivi con p ≤ 0,05 (permutazione, due code);
- **altrimenti si scrive smentita**, con i numeri, come questa descrizione.
