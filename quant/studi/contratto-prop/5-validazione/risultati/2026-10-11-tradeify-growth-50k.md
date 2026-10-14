# Tradeify Growth 50K: Lo Studio, Fase Per Fase

**11 ottobre 2026.** Il contratto Tradeify Growth 50K passato per la pipeline della metodologia,
dall'idea alla validazione. **Non e' una raccomandazione**: dice quanto vale il contratto sotto
ipotesi dichiarate, e quanto quelle ipotesi pesano.

Tutti i numeri: regole lette l'11 ottobre 2026; sessioni browniane con 78 controlli per sessione
(uno ogni 5 minuti di seduta); 100.000 cicli per riga; seme 1; conto finanziato fino al pavimento o a
1.000 sessioni; si esce dal limite giornaliero morbido esattamente al limite. Ogni riga si rifa con
il comando indicato.

## 1. L'Idea

I tre paper ([analisi TF](../../../../paper/prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md),
[analisi Lim](../../../../paper/prop-firm/lim-2026-price-of-a-funded-account/analisi.md),
[analisi Youngblood](../../../../paper/prop-firm/youngblood-2026-inherent-value-of-retail-prop-firms/analisi.md)) e la scheda
([`1-idea.md`](../../1-idea.md)). Il finding:
la valutazione e' un'opzione; per chi non ha vantaggio puo' valere piu' del prezzo [TF §5.7], perche'
la perdita vera e' limitata al costo e il guadagno no [Y §4.1]; il valore dipende da regole che non
stanno accanto al prezzo [TF §4.2] e dalla dimensione [L §6.3]. Cosa lo ucciderebbe: le regole le
cambia la firma [TF §6], e un vantaggio negativo lo annulla [TF §5.6].

## 2. Le Regole

Lette dalle fonti ufficiali, con le citazioni: [`tradeify-growth-50k.md`](../../2-regole/tradeify-growth-50k.md).
In breve:
- **valutazione**: target 3.000 $; drawdown 2.000 $ mobile, aggiornato alla chiusura e controllato
  in tempo reale; limite giornaliero morbido di 1.250 $; nessuna consistenza, nessuna scadenza;
- **conto finanziato**: lo stesso drawdown, bloccato a +100 $; prelievo con saldo di 53.000 $, 5
  giornate sopra 150 $ e consistenza del 35%; tetti di 1.500, 2.000, 2.500, poi 3.000 $; 90% al
  trader;
- **prezzi**: 145 $ (102 $ con un codice fino al 15 ottobre), reset 95 $.

Rispetto a TF (10 agosto): reset da 99 a 95 $, prezzo effettivo da 107 a 145 / 102 $, e soprattutto
**la soglia del prelievo**, ora 3.000 $ di profitto piu' giorni e consistenza.

## 3. Il Codice

Il contratto come dati (fase 2), il simulatore e le sessioni (fase 4), la forma chiusa di TF e la
verifica (fase 5): [`il-simulatore-di-contratto.md`](../../../../metodo/4-il-simulatore-di-contratto.md).
24 test nuovi, 47 in tutto, verdi.

## 4. Il Backtest: Cosa Vale Il Contratto

### Il caso di riferimento

Il trader di TF: nessun vantaggio, d = 850 $ di deviazione per sessione (circa 2 MNQ tenuti tutta
la seduta, sulle sedute di NQ di settembre e ottobre 2026).
`quant contratto tradeify-growth-50k --d 850`

| quantita' | valore |
|---|---|
| probabilita' di passare la valutazione | **0,302** (forma chiusa 0,300) |
| sessioni per tentativo | 6,9 |
| costo per ottenere un conto finanziato | 364 $ |
| probabilita' di almeno un prelievo, una volta finanziati | **18,3%** |
| valore di un conto finanziato (prelievi al 90%) | 675 $ |
| **netto medio per ciclo** | **+310 $** (errore 8 $) |
| netto: p5 / mediana / p95 | -810 / **-240** / +2.910 $ |
| cicli in perdita | **81,8%** |
| tasso di pareggio PA | 0,152 |

Il valore atteso e' positivo, ma **quattro cicli su cinque perdono**: la mediana e' -240 $ (due
tentativi e nessun prelievo). E' la forma di una lotteria con valore atteso positivo, come in Y.

### Cosa lo muove

| variante, d = 850 $ | netto medio per ciclo | in perdita | comando |
|---|---|---|---|
| riferimento | +310 $ | 81,8% | |
| prezzo 102 $ (codice GEMS) | +353 $ | 81,8% | `--prezzo 102` |
| blocco del pavimento anche in valutazione | +316 $ | 81,8% | `tradeify-growth-50k-blocco` |
| **il conto simulato finisce dopo 1 prelievo** | **-117 $** | 81,9% | `--max-prelievi 1` |
| finisce dopo 3 prelievi | +93 $ | 81,8% | `--max-prelievi 3` |
| costi di esecuzione (deriva -6 $ a sessione) | +217 $ | 82,6% | `--deriva -6` |
| piccolo vantaggio (+40 $ a sessione, 0,05 d) | +1.699 $ | 76,2% | `--deriva 40` |
| la deriva implicita nel 17,2% pubblicato (-118 $) | -490 $ | 93,6% | `--deriva -118` |

**La variabile che pesa di piu' e' quanto dura il conto simulato.** Tradeify "si riserva il diritto
di spostarti su un conto dal vivo in qualunque momento dopo un prelievo". Se succede al primo
prelievo, il contratto non vale il prezzo nemmeno per chi non ha vantaggio (PA sale a 0,48, P e'
0,30). E' la sensibilita' che TF chiama "senza analogo" [TF §6.1]: la decide la firma.

### La dimensione

`quant contratto tradeify-growth-50k --d <d>`, e con `--senza-limite`:

| d ($ per sessione) | P valutazione | almeno un prelievo | netto, uscita esatta al limite | netto, senza limite giornaliero |
|---|---|---|---|---|
| 200 | 0,243 | 24,9% | +235 $ | |
| 400 | 0,260 | 24,8% | +301 $ | |
| 600 | 0,279 | 22,0% | +331 $ | |
| 850 | 0,302 | 18,3% | +310 $ | +272 $ |
| 1.250 | 0,336 | 14,5% | +302 $ | |
| 1.750 | 0,369 | 12,0% | +347 $ | +131 $ |
| 2.500 | 0,403 | 10,1% | +453 $ | |
| 3.500 | 0,430 | 9,0% | +588 $ | +31 $ |

Due forze opposte. **Operare grande aiuta a passare**: il pavimento si aggiorna solo alla chiusura,
e piu' la giornata e' larga piu' quel ritardo favorisce il compratore [TF §2.4]. **Operare grande
rende difficile prelevare**: la consistenza del 35% e i 5 giorni sopra 150 $ puniscono le giornate
grandi. Il risultato netto dipende da un'ipotesi: se si esce **esattamente** a -1.250 $ quando si
tocca il limite giornaliero, la dimensione grande vince; senza quel taglio (o con uno slittamento
oltre il limite, che Tradeify dice possibile) c'e' una cresta e conviene operare piccoli. **Il
numero affidabile e' la colonna senza limite**; quella con il limite e' un tetto.

Una differenza con TF: secondo TF il limite morbido riduce P [TF §2.5]; qui con l'uscita esatta al
limite la alza (0,302 contro 0,295 senza limite, a d = 850 $). Dipende da quell'ipotesi.

### Il tasso pubblicato

Tradeify pubblica il **17,2%** delle valutazioni iniziate completate (agosto 2025 - luglio 2026,
Growth, Advanced e Select insieme). Il modello senza vantaggio da' 0,30. Nella forma chiusa, a d =
850 $, il 17,2% corrisponde a una deriva di **-118 $ per sessione** (-0,14 d): circa venti volte i
costi di esecuzione di due MNQ (6 $). E' la tesi di TF, con un dato nuovo e di fonte primaria: **la
firma guadagna sulla distanza fra il compratore medio e il compratore senza vantaggio**.

Un conto pero' non torna, e va scritto. Il 17,2% sta **sopra** il pareggio PA = 0,152, calcolato con
il valore del conto di un trader senza vantaggio. Se chi arriva al finanziato si comportasse come un
trader senza vantaggio, a quel tasso la firma incasserebbe 602 $ per conto finanziato
(145 + (1/0,172 - 1) x 95) e ne consegnerebbe 675 $: perderebbe circa 70 $ a conto. Se guadagna, e'
per una di due cose che il modello non vede: anche nel finanziato il compratore medio fa peggio del
caso (TF §5.5 sostiene il contrario, con dati di un altro periodo), oppure **il conto simulato finisce
prima** (passaggio al vivo, Elite dopo tre prelievi). Con il conto che finisce al primo prelievo, PA
sale a 0,48 e la firma guadagna a qualunque tasso realistico. Il dato (c) di Tradeify, 28,5% dei
finanziati con almeno un prelievo, e' per persona e non per conto: non si confronta con il 18,3%
simulato per conto.

### I dati veri di NQ: solo descrittivi

L'archivio ha 22 sedute complete (10 settembre - 9 ottobre 2026; prima, NQZ6 non era il contratto
principale e le barre hanno buchi). Con circa 7 sessioni per tentativo i tentativi indipendenti sono
**tre**: l'errore vero di P e' circa 0,25 (criterio C5). Per completezza:

| sorgente | P | netto per ciclo | tentativi indipendenti |
|---|---|---|---|
| seduta 9:30-16:00, 2 MNQ, verso a caso | 0,26 | +103 $ | circa 3 |
| Youngblood, 8 MNQ, verso a caso | 0,15 | -317 $ | circa 1 |

**Non dicono niente** sul contratto. Servono a provare che la pipeline gira sui dati veri, e a
mostrare la forma delle sedute vere: deviazione di una seduta 395 $ per MNQ, escursione avversa media
300-340 $ per MNQ.

## 5. La Validazione

- **Il simulatore e' validato prima** contro criteri sigillati con un hash: C2 e C3 passano, C1
  passa 4 su 5, C4 non passa come scritto. La diagnosi trova un parametro che la forma chiusa di TF
  non ha (quanto spesso si guarda il target) e un criterio scritto male da noi:
  [`quant/studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md`](../criteri/2026-10-11-il-contratto-prop-esito.md).
  **Per Tradeify** (violazione in tempo reale) la forma chiusa regge: 0,300 contro 0,302 simulato.
- **Nessun cancello D o F**: non c'e' una strategia con un vantaggio da validare, e nessun fuori
  campione e' stato guardato. Il trader e' senza vantaggio per costruzione.
- **La precisione**: sul browniano, l'errore Monte Carlo e' stampato accanto a ogni numero; sui dati
  veri l'informazione e' troppo poca per misurare.

## Cosa Se Ne Conclude

1. **Per un trader senza vantaggio, con le regole di oggi e il conto simulato che dura, il contratto
   vale piu' del prezzo** (+310 $ per ciclo), come dice TF. Ma **quattro cicli su cinque perdono**.
2. **Il segno dipende da cio' che il compratore non controlla**: quanto dura il conto simulato (con
   un solo prelievo e' negativo), e regole che la firma ha cambiato due volte in due mesi.
3. **Il segno dipende anche dal vantaggio**, molto: +40 $ a sessione portano il netto a +1.700 $,
   -118 $ (la media implicita del mercato) a -490 $. Il contratto **non sostituisce un vantaggio**:
   lo amplifica, in tutte e due le direzioni.
4. **La dimensione conta, ma in modo diverso da Lim**: senza scadenza non c'e' la cresta del 2%;
   c'e' una tensione fra passare (meglio grande) e prelevare (meglio piccolo).

**Cosa manca per una misura vera**: la storia lunga di NQ (servono migliaia di sessioni, non 22), e
la strategia con cui l'utente opera davvero al posto del verso a caso. E' il prossimo uso naturale:
la stessa pipeline con una strategia del catalogo, quando avra' passato i suoi cancelli.
