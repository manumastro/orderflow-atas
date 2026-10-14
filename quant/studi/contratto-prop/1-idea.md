# Il Valore Del Contratto Della Prop Firm

**Stato**: regole scritte e codificate; simulatore validato contro criteri sigillati; studio di
Tradeify Growth 50K fatto (11 ottobre 2026). Nessuna misura sui dati veri: servono migliaia di sessioni.
**Strategia nel catalogo**: nessuna; il contratto e' nel catalogo dei contratti (`quant contratto`).

Le analisi dei paper: [Tomas Fernandez](../../paper/prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md),
[Lim](../../paper/prop-firm/lim-2026-price-of-a-funded-account/analisi.md), [Youngblood](../../paper/prop-firm/youngblood-2026-inherent-value-of-retail-prop-firms/analisi.md). Lo studio:
[`tradeify-growth-50k`](5-validazione/risultati/2026-10-11-tradeify-growth-50k.md).

Questa scheda e' diversa dalle altre. I tre paper non cercano un vantaggio nel mercato: misurano
**quanto vale il contratto** che una prop firm vende (la valutazione, o challenge) a chi lo compra,
e dicono che quel valore dipende dalle regole del contratto e dal modo di operare almeno quanto dal
vantaggio di chi opera. La fase 1 della metodologia vale uguale: finding, perche', cosa lo
ucciderebbe `[MC · 26:27 - 27:22]`.

## I Paper

Tutti preprint su SSRN, **senza revisione fra pari**, scritti con l'aiuto di modelli linguistici
(dichiarato da tutti e tre). I paper sono nel filone `quant/paper/prop-firm/`, uno per cartella; l'indice in
[`quant/paper/indice-dei-paper.md`](../../paper/indice-dei-paper.md).

| sigla | paper | autore, data | metodo |
|---|---|---|---|
| **TF** | *Valuing Proprietary Trading Firm Evaluation Contracts: Closed Form, Cross-Firm Dispersion and the Source of the Margin* | Martí Tomàs Fernández, agosto 2026 | forma chiusa della probabilita' di successo, 13 prodotti futures da 50K, un caso di studio con dati propri |
| **L** | *The Price of a Funded Account: An Actuarial Analysis of Proprietary Trading Firm Challenges* | Boon Chuan Lim, 25 luglio 2026 | moto browniano aritmetico, Monte Carlo da 200.000 percorsi, contratto stilizzato a due fasi da 100K (stile forex/CFD) |
| **Y** | *The Inherent Value of Retail Prop Firms for Fixed-Size Trading Strategies* | Zachary Youngblood (studente, Fordham), 21 agosto 2026 | backtest su 10 anni di MNQ a 1 minuto, verso casuale, regole Topstep 50K programmate |

Le citazioni sono per sezione: `[TF §2.3]`, `[L §6.1]`, `[Y §2.2]`.

## Il Finding

### 1. La probabilita' di passare dipende da parametri che il prezzo non mostra

**Il contratto e' un problema di primo passaggio** `[TF §2]`, `[L §4.2]`: il target e' una barriera
assorbente in alto, il drawdown una in basso. Senza vantaggio e senza attriti, passa con
probabilita' D/(T+D) (target T, drawdown D): con T = 3.000 $ e D = 2.000 $, 0,40 `[TF §2.3]`.

**Ma il drawdown dei future e' mobile**, e il punto in cui smette di salire (il *freeze level*) cambia
tutto. Senza vantaggio, con T = 3.000 e D = 2.000 `[TF §2.3]`:

| regime del pavimento | probabilita' di passare |
|---|---|
| fisso | 0,400 |
| mobile, si ferma al saldo iniziale | 0,245 |
| mobile, si ferma a +100 $ | 0,241 |
| mobile per tutta la fase | 0,223 |

**Sei parametri decidono la probabilita'** `[TF §2]`:
1. target;
2. drawdown;
3. livello a cui il pavimento si ferma;
4. ogni quanto il pavimento si aggiorna (alla chiusura o intraday);
5. ogni quanto si controlla la violazione;
6. il limite di perdita giornaliero, morbido o rigido.

Su 13 prodotti futures da 50K raccolti dalle fonti ufficiali il 10 agosto 2026, **11 pubblicizzano
lo stesso target e lo stesso drawdown (3.000 / 2.000) e la probabilita' di passare va da 0,250 a
0,407: il 62% di differenza** `[TF §4.2]`. Chi aggiorna il pavimento alla chiusura sta intorno a
0,30, chi lo aggiorna intraday intorno a 0,25.

**La scadenza e' un vantaggio nascosto per la firma** `[L §6.1]`. Contratto stilizzato con target +10%
e perdita massima -10%: senza vantaggio si passa con 0,50; aggiungendo i 30 giorni e il limite
giornaliero del 5%, 0,298. La scadenza da sola toglie 19 punti.

### 2. Conta come si dimensiona, quasi quanto il vantaggio

**L ha la cresta della volatilita'** `[L §6.2 - 6.3]`. Con scadenza, il valore del contratto si
concentra intorno al **2% di volatilita' giornaliera**:
- con volatilita' bassa si fallisce per tempo scaduto;
- con volatilita' alta si fallisce violando i limiti.

Sulla cresta il contratto "perdona" quasi un punto di Sharpe: a pareggio con il prezzo esposto
anche con Sharpe netto -0,87. La conseguenza: "il contratto non distingue un trader cauto con un
po' di vantaggio da uno aggressivo senza".

**TF, sulla dimensione della posizione** `[TF §2.4]`:
- con monitoraggio continuo e senza vantaggio la dimensione non conta, perche' la rovina di una
  martingala non dipende dalla varianza;
- conta solo attraverso la discretizzazione. La barriera effettiva e' D + 0,5826 · s (correzione di
  Broadie, Glasserman e Kou), dove s e' la deviazione standard del saldo fra due controlli;
- quadruplicare la dimensione alza la probabilita' di circa un terzo con controllo alla chiusura, e
  di circa un decimo con controllo intraday;
- **e' l'unica leva che il modello concede a chi non ha vantaggio**;
- negli abbonamenti mensili, operare piu' grande rende il prodotto piu' economico: si risolve prima
  e si pagano meno mensilita' `[TF §3.2]`.

### 3. Una strategia senza vantaggio puo' avere valore dentro il contratto

**Y, la strategia di prova** `[Y §2.2 - 2.4]`:
- 8 MNQ, ingresso alle 10:00 ET, verso casuale;
- take profit e stop simmetrici di 100 tick (±400 $), uscita alle 16:00;
- 1,22 $ di costi per contratto, su 2.485 giornate di MNQ 2016-2026.

**Da sola, senza vantaggio** `[Y §2.4]`: vince il 49,98%, lordo -0,17 $ a operazione, **-24.670 $ netti**
su 10 anni.

**Dentro le regole Topstep 50K** come le programma l'autore `[Y §3.1]`:
- **+1.490 $ di netto medio per conto**, mediana +1.037 $;
- payout medi 9.368 $ contro 7.878 $ di costi, cioe' 1,19 $ di payout per 1 $ di costi;
- il 60,3% dei percorsi in utile;
- percentili 5° e 95° del netto: -3.995 $ e +8.613 $.

**Il risultato non e' stabile nel tempo** `[Y §3.2]`:

| periodo | payout per 1 $ di costi | netto mediano | percorsi in utile |
|---|---|---|---|
| 2016-2021 | 1,06 | -214 $ | 46,45% |
| 2021-2026 | 1,22 | positivo | 56,05% |

Nel primo periodo ci sono 196 uscite a tempo per percorso, nel secondo nessuna.

### 4. Il margine della firma viene dalla selezione, non dal prezzo

**TF, sul caso di studio** `[TF §8.5]`: per un trader senza vantaggio, ottenere un conto finanziato
costa in media 338 $ e il conto vale 606 $. Il prodotto sarebbe ancora conveniente al doppio del
prezzo.

**TF, sui tassi di passaggio** `[TF §5]`:
- **il tasso di conversione di pareggio** e' PA = c_r / (c_r + F − c_e), dove c_r e' il reset, c_e
  l'ingresso e F il valore del conto. E' insieme la soglia del compratore e il punto in cui la firma
  smette di guadagnare;
- il modello prevede circa 0,30;
- i tassi che circolano nell'industria sono 5-14%, gia' sotto il pareggio;
- la differenza corrisponde a una deriva negativa del compratore medio di 127-303 $ a sessione:
  **da tre a trenta volte i costi di esecuzione plausibili**.

La conclusione: **la firma guadagna perche' il compratore medio perde molto piu' del caso, non
perche' il contratto costa troppo**.

**L, sulla popolazione** `[L §6.4]`, con una popolazione illustrativa (Sharpe lordo normale standard
e costi pari al 10% della volatilita'):
- il contratto da 500 $ vale in media 221 $, quindi il 56% di margine lordo per la firma;
- il 7,5% arriva al conto finanziato;
- senza costi varrebbe 1.098 $.

## Il Perche'

- **Il contratto e' un'opzione**: si paga un premio fisso per un guadagno condizionato, e la perdita
  vera e' limitata al costo dei conti `[Y §4.1]`, `[L §3]`. Chi tiene un'opzione vuole volatilita',
  ma le barriere la puniscono: da qui l'ottimo interno di dimensione `[L §3]`.
- **La varianza naturale fa il resto** `[Y §1.1]`: una strategia a pareggio passa periodi sopra e
  sotto la media. Il contratto incassa i periodi buoni con i payout e limita i cattivi al costo del
  conto.
- **Le regole si possono rendere irrilevanti** `[Y §3]`: con dimensione fissa e un solo trade da
  400 $ al giorno, il limite giornaliero da 1.000 $ e la regola di consistenza da 1.500 $ non
  scattano mai.

## Cosa Lo Ucciderebbe

- **Le regole cambiano, e le cambia la firma, anche sui conti gia' venduti** `[TF §6]`. E' la
  sensibilita' piu' grande del contratto, e non si copre con niente. Durante la stesura di TF la
  soglia di payout del prodotto studiato e' passata da 750 $ a 1.500 $, e la mortalita' della fase
  finanziata da 31,8% a 49,5% `[TF §6.2]`. **Nessuna configurazione favorevole sopravvive a essere
  scoperta** `[TF §6.3]`.
- **Payout negati a discrezione, regole sulle strategie proibite, minimi di attivita'**: fuori da tutti
  e tre i modelli `[TF §2.8]`, `[L §4.3]`, `[Y §4.2]`.
- **Un vantaggio negativo**: tutto regge solo se la deriva e' vicina a zero. La deriva negativa media
  implicita nei tassi pubblicati la rende negativa `[TF §5.6]`.

## Il Caso Che Ci Riguarda

L'utente opera con Tradeify. Il caso di studio di TF e' un prodotto acquistato una volta sola a
107 $, con listino 145 $ e reset 99 $ `[TF §8.5]`. Nella tabella dei prezzi di TF l'unico prodotto a prezzo
unico (non in abbonamento) con listino 145 $ e reset 99 $ e' Tradeify Growth 50K; anche Topstep ha
il reset a 99 $, ma e' in abbonamento. Il caso di studio e' quindi con ogni probabilita' **Tradeify
Growth 50K**. **E' una deduzione: il paper non lo dice.** P = 0,300 e' unico nella tabella di TF §4.2, e
il costo di 338 $ si riottiene solo con 107 $ di ingresso, 99 $ di reset e P = 0,300.

I numeri di Tradeify Growth secondo TF, al 10 agosto 2026 `[TF §4.2]`, `[TF App. A]`:
- T = 3.000 $, D = 2.000 $;
- pavimento che si ferma a +100 $, aggiornato alla chiusura, violazione controllata intraday;
- limite giornaliero morbido di 1.250 $;
- consistenza del 35% sul payout;
- probabilita' di passare senza vantaggio 0,300;
- verdetto: per un trader senza vantaggio il prodotto vale circa il doppio di quanto costa
  `[TF §8.5]`.

**Verificato l'11 ottobre 2026** sulle fonti ufficiali: [`tradeify-growth-50k.md`](2-regole/tradeify-growth-50k.md).
Da agosto sono cambiati il reset (95 $), il prezzo effettivo e la soglia del prelievo (53.000 $ di
saldo, 5 giornate sopra 150 $, consistenza del 35%). Due pagine ufficiali non concordano sul blocco
del pavimento in valutazione (non pesa: 0,302 contro 0,308).

## Le Regole Da Provare

**Ipotesi 1, la replica di Y su NQ.** Strategia senza vantaggio, verso casuale (e' il nostro modello
nullo del verso), 10:00 ET, take profit e stop simmetrici, dentro le regole di un contratto
programmato. Il confronto e' fra il netto da sola e il netto dentro il contratto. Serve la storia
lunga: Y usa 10 anni, noi abbiamo 50 giornate.

**Ipotesi 2, la cresta della dimensione su NQ.** Per una strategia data, il valore del contratto al
variare dei contratti per operazione (la dimensione fissa del framework). L e TF dicono che esiste
un ottimo interno e che dipende dal monitoraggio.

**Ipotesi 3, la forma chiusa di TF come verifica.** La formula di TF, corretta per la discretizzazione,
riproduce la simulazione entro l'1% per s/D ≤ 0,5 `[TF §2.4]`. Implementata nel framework, diventa il
test del simulatore di contratto: stessi parametri, stessa probabilita'.

Varianti dichiarate prima di provarle:
1. contratto Tradeify Growth 50K (regole da verificare alla data);
2. contratto Topstep 50K come in Y;
3. per ciascuno, dimensioni 1, 2, 4, 8 MNQ.

## Cosa Serve Al Framework

**Fatto l'11 ottobre 2026**: [`il-simulatore-di-contratto.md`](../../metodo/4-il-simulatore-di-contratto.md).
Il progetto, come scritto prima di farlo:

**Un simulatore di contratto**: prende una sequenza di risultati (le operazioni di un backtest, o di
una replica del nullo o del Monte Carlo) e applica le regole. Le regole sono:
- target;
- drawdown con livello di blocco;
- aggiornamento del pavimento alla chiusura o intraday;
- controllo della violazione;
- limite giornaliero morbido o rigido;
- consistenza (tre tipi `[TF §4.3]`);
- soglia, tetto e quota del payout;
- abbonamento o prezzo unico, reset e attivazione.

Restituisce i soldi veri: costi pagati, payout ricevuti, netto. Per farlo servono anche **il
massimo e il minimo intraday del P&L** di ogni operazione: il motore li ha gia' (MAE e MFE).

## Le Lezioni Di Metodo

Vengono da TF, e valgono per tutto il framework, non solo per questa idea:

- **I percorsi non sono osservazioni indipendenti** `[TF §8.2]`. Con S sessioni ed E[N] sessioni per
  tentativo, i tentativi indipendenti sono circa S/E[N], qualunque sia il numero di percorsi
  simulati. TF stima gli intervalli con un bootstrap a blocchi di cinque sessioni. E' la stessa
  lezione della fase 40R, e la stessa scelta del nostro ricampionamento per giornata. **Y la ignora**:
  i suoi 10.000 percorsi rimescolano solo il verso sulle stesse 2.485 giornate, e lo dice `[Y §2.3]`.
- **La coerenza della misura** `[TF §7]`. Chi ripesa gli scenari storici per riprodurre un tasso
  osservato deve riprezzare tutto cio' che vale in quegli scenari, o pesarlo con il rapporto di
  verosimiglianza; altrimenti fabbrica valore in proporzione allo spostamento. La diagnosi: si fa
  variare il fattore di calibrazione, e un effetto vero non dipende da quanto si e' calibrato.
- **Il criterio si sigilla prima di vedere i dati** `[TF App. B]`: per ogni affermazione, la
  quantita', il campione minimo e la soglia che la smentirebbe, con un hash crittografico, prima di
  collegarsi ai dati. Due criteri su quattro risultarono mal specificati: sigillare impedisce di
  spostarli, non di sbagliarli.
- **Il registro delle stime** `[TF App. C]`: la stima centrale di TF si e' mossa sette volte, e le
  quattro correzioni per errore sono andate tutte nella stessa direzione, contro la tesi. "Gli errori
  non erano distribuiti a caso."

## Le Prove

**Criteri sigillati** prima del simulatore: [`2026-10-11-il-contratto-prop.md`](5-validazione/criteri/2026-10-11-il-contratto-prop.md),
sha256 `93721e09f204bfda3c23b4b40d4213fd21cf264cff1db480b21876aa4f670877`.

| data | cosa | esito |
|---|---|---|
| 2026-10-11 | verifica del simulatore (C1-C4) | C2 e C3 passano; C1 4 su 5; C4 non passa come scritto. Diagnosi: la forma chiusa di TF non ha la frequenza di controllo del target; C4 aveva la geometria sbagliata. [Esito](5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md) |
| 2026-10-11 | ipotesi 3, la forma chiusa come verifica | riprodotti tutti i numeri stampati di TF (test); simulazione entro l'1% dove la violazione e' in tempo reale |
| 2026-10-11 | Tradeify Growth 50K, regole di oggi, nessun vantaggio, d = 850 $ | P = 0,302; netto +310 $ per ciclo ma 81,8% dei cicli in perdita; **-117 $ se il conto simulato finisce al primo prelievo**. [Studio](5-validazione/risultati/2026-10-11-tradeify-growth-50k.md) |
| 2026-10-11 | ipotesi 2, la dimensione (d da 200 a 3.500 $) | nessuna cresta al 2% (non c'e' scadenza); passare premia la dimensione grande, prelevare la piccola; il segno dipende dall'uscita esatta al limite giornaliero |
| 2026-10-11 | ipotesi 1, la replica di Y su NQ | **non misurabile**: 22 sedute complete, circa un tentativo indipendente (criterio C5) |
