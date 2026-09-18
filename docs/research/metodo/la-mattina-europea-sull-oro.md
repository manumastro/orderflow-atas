# La Mattina Europea Sull'Oro

> **Stato dal 18 settembre 2026: archiviato come misura. Lo strumento operativo e' NQ e basta.**
>
> *"Don't execute on three four asset. Get good with one asset, one can pay your bills. One asset,
> one. I choose NASDAQ"* (`fabio_q1/fabio_3.txt`, 1:06:37), piu' la decisione dell'utente del 18
> settembre 2026. La misura sulle 34 sedute resta valida come misura e non va rifatta; non entra in
> nessuna lettura, in nessuna condizione e in nessun livello.

Stato: **misura aperta, non regola.** Trentaquattro sedute, sei venerdi. Non autorizza niente, non
entra in nessuna condizione armata, e il `verso` degli scenari resta `NESSUN PERMESSO`. Esiste
perche' un indizio misurato vale piu' di un'impressione, e perche' fra venti settimane si potra'
dire se reggeva.

Nasce dalla domanda dell'utente del 18 settembre 2026: *"e' meglio per noi europei la mattina di
venerdi?"*

## Cosa Si E' Misurato, E Come

**Strumento** GCZ6 (oro intero, COMEX), **finestra** 07:00-12:00Z, cioe' **09:00-14:00 CEST** —
la mattina di chi sta in Europa. **Base** le 34 sedute complete dal 3 agosto al 17 settembre 2026,
scartando le sedute con meno di 1.200 barre M1 (domeniche di apertura e festivi).

Tre misure per ogni mattina:

```text
escursione     massimo - minimo della finestra
|C - O|        quanto della finestra e' finito in direzione netta
efficienza     |C - O| / escursione      <- la misura che conta
```

**L'efficienza direzionale e' il rapporto fra quanto il prezzo si e' spostato e quanto si e'
agitato.** Una mattina che fa 40 punti di escursione e chiude dove ha aperto ha efficienza 0: ha
offerto movimento e nessuna direzione. Una che fa 40 punti e li tiene tutti ha efficienza 1.
**E' la differenza fra un mercato tradabile e uno che si limita a muoversi.**

## Il Risultato

```text
   g   n   escursione   |C-O|   efficienza   vol medio   conferma dal pomeriggio
  lun   7      34,60     14,21      38%       23.263          1/7
  mar   7      35,36     15,07      33%       25.621          4/7
  mer   7      31,14     10,90      36%       25.309          4/7
  gio   7      39,37     23,06      56%       27.724          2/7
  ven   6      35,47     23,07      64%       25.974          4/6
  ---------------------------------------------------------------------------
  tutti 34     35,07     17,15      45%
```

**Il venerdi mattina non si muove di piu': si muove meglio.** L'escursione e' esattamente nella
media (35,47 contro 35,07) e il volume pure. Quello che cambia e' **quanta parte di
quell'escursione resta**: il 64% contro il 45% generale, e contro il 38% del lunedi che fa la
stessa escursione.

**La dispersione, perche' una media su sei osservazioni senza dispersione non e' un numero:**

```text
   g   n   media   mediana   min    max   dev.st
  lun   7     38%      25%     1%    75%     26%
  mar   7     33%      27%    10%    78%     24%
  mer   7     36%      35%    24%    58%     10%
  gio   7     56%      79%     5%    96%     35%
  ven   6     64%      75%    13%    85%     25%
```

I sei venerdi, in ordine di tempo: **55% 85% 79% 82% 13% 71%**. Cinque su sei sopra il 55%, uno
solo (4 settembre) crollato al 13%.

## La Verifica Che Conta: Il Confronto Entro La Settimana

**Le sedute della stessa settimana non sono indipendenti.** Una settimana di tendenza rende
efficienti tutte le sue mattine, e un confronto fra medie assolute puo' misurare quali settimane
sono capitate di venerdi invece che cosa fa il venerdi. E' la lezione gia' pagata da questo
repository su altre misure.

**Il modo di togliersela di mezzo e' confrontare ogni venerdi con i quattro giorni della sua
stessa settimana**, e guardare il rango.

```text
  sett   lun   mar   mer   gio   ven      rango del venerdi
   32     1%   31%   27%   24%   55%            1
   33    68%   58%   35%   20%   85%            1
   34    25%   12%   58%   79%   79%            2   (pari con giovedi)
   35    20%   12%   38%    5%   82%            1
   36    55%   78%   36%   84%   13%            5
   37    20%   10%   24%   96%   71%            2
```

```text
  rango medio osservato   2,00 su 5
  atteso per caso         3,00
  p (permutazione entro settimana, 200.000 estrazioni)   0,056
```

**Il venerdi e' stato il giorno migliore della propria settimana in quattro settimane su sei**,
secondo in una, ultimo in una.

## Come Va Letto

**E' un indizio serio e non e' una regola.** Tre ragioni, tutte da tenere insieme:

1. **Sei venerdi.** `p = 0,056` non supera nessuna soglia convenzionale, e con sei osservazioni una
   sola settimana diversa sposta il risultato. Il venerdi peggiore (13%) e' gia' dentro il campione:
   il secondo lo porterebbe sopra 0,10.
2. **Una fase sola.** Agosto-settembre 2026 e' un mercato che sale a 4.755, scende a 4.273 e
   rimbalza. Non si sa cosa faccia questa misura in un mercato laterale.
3. **Nessun meccanismo dichiarato.** Non c'e' una ragione strutturale scritta da nessuna parte per
   cui il venerdi mattina l'oro dovrebbe tenere la direzione. Senza un perche', un pattern a
   `p = 0,056` su sei osservazioni e' quello che ci si aspetta di trovare cercando.

**Quindi: non si arma niente su questa base.** Si continua a misurare e si rilegge a venti
settimane, insieme al confronto con NQ sugli stessi giorni.

## Il Contro Che La Tabella Principale Non Mostra

La mattina europea vale **poco della seduta**, e il venerdi meno che mai:

```text
   g    esc mattina   esc pomeriggio   quota mattina   quota volume mattina
  lun       34,60          54,59           39%              28%
  mar       35,36          58,33           38%              25%
  mer       31,14          90,70           26%              19%
  gio       39,37          63,37           38%              24%
  ven       35,47          98,40           26%              19%
```

**Il venerdi mattina e' pulito ma piccolo:** il 26% dell'escursione e il 19% del volume della
seduta. Il pomeriggio del venerdi fa quasi cento punti di escursione — il massimo della settimana.
Chi opera solo la mattina prende la parte piu' leggibile e lascia la piu' grossa.

## L'Evento Che Sta Dentro La Finestra, E Che Nessuna Condizione Sa Riconoscere

**ATTENZIONE ALLA FORMULA (aggiunto il 18 settembre 2026).** Quello che questo documento misura e'
**il volume del minuto 12:30Z**, non un calendario. Il repository **non ha una fonte per le
release macro**: dire "alle 12:30Z escono i dati" e' una inferenza, non un fatto verificato, e il
18 settembre l'ho presentata come fatto per un'ora prima che l'utente chiedesse *quale dato*.
La formulazione corretta e' **"il minuto 12:30Z e' statisticamente il piu' pesante della
giornata"**. Se serve sapere se oggi esce qualcosa, lo si chiede all'utente o si apre un
calendario: non lo si deduce dai lotti.

**Alle 12:30Z — 14:30 CEST — il volume esplode, ed e' il minuto piu' pesante della
giornata sull'oro.** Misurato sulle stesse 34 sedute:

```text
  12:29Z    media 172 lotti    mediana 132
  12:30Z    media 786 lotti    mediana 277    massimo 5.078
  12:31Z    media 467 lotti
```

**La media e' quattro volte piu' alta della mediana**: non e' una sessione che apre, sono cinque
sedute su trentaquattro (5.078, 3.367, 2.924, 2.903, 2.458) che fanno quasi tutto il numero. Solo
**13 sedute su 34** superano i 400 lotti in quel minuto.

**Conseguenza operativa:** 12:30Z cade **dentro** la finestra 07:00-12:00Z solo di stretta misura —
ne e' il confine. Ma una posizione aperta la mattina ci arriva sopra, e **nessuna condizione armata
sa distinguere un dato macro da una rottura**: stesso volume fuori scala, stesso delta fuori scala,
significato opposto. Finche' non esiste una regola esplicita (sospendere la sorveglianza in quella
finestra, o alzare le soglie), **il minuto delle 12:30Z si tratta a mano**.

## Il Confronto Con Il Gate Tier 01

**Non c'entra e non lo sostituisce.** Il gate Tier 01 e' un permesso che si apre su una chiusura
M30 fuori dall'IVB, e sull'oro l'IVB sta a 13:30-14:00Z (misura in
[`../giornate/GCZ6-2026-09-18.md`](../giornate/GCZ6-2026-09-18.md), passo 2). **La mattina europea
e' interamente pre-gate**, su qualunque giorno della settimana, e vale la deroga dichiarata
dell'utente descritta in
[`le-gambe-di-una-seduta.md`](le-gambe-di-una-seduta.md), sezione *"La Deroga: Operare Prima Del
Gate"*.

Questo documento dice **quando la mattina e' piu' leggibile**, non **cosa e' permesso farci**.
Sono due domande diverse e le risposte non si sommano.

## Cosa Serve Per Chiudere La Misura

1. **Venti settimane**, non sei.
2. **La stessa misura su NQZ6**, sugli stessi giorni: se il venerdi mattina e' efficiente ovunque,
   non e' una caratteristica dell'oro.
3. **Una fase di mercato diversa** dentro il campione.
4. **Un meccanismo**, o l'ammissione esplicita che non c'e'.
