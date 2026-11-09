---
chiave: holowczak-2014-aggregating-information-in-option-transactions
sigla: HHW
titolo: "Consolidating Information in Option Transactions (working paper); pubblicato come: Aggregating Information in Option Transactions"
autori: [Richard Holowczak, Jianfeng Hu, Liuren Wu]
versione_letta: working paper SSRN 1740046, "First draft: January 13, 2011", 28 pagine; NON la versione pubblicata nel Journal of Derivatives 21(3), 2014, 9-23, che non e' stata letta
fonte: SSRN (working paper); pubblicato poi nel Journal of Derivatives (dati della rivista dalla ricerca, fase 1, non dal PDF)
identificativo: "SSRN 1740046; doi della versione pubblicata 10.3905/jod.2014.21.3.009"
url: https://ssrn.com/abstract=1740046
tipo: preprint (working paper; la versione pubblicata e' su rivista)
revisione_fra_pari: "no per la versione letta; si' per la versione pubblicata, non letta"
pagine: 28
pdf: holowczak-2014-aggregating-information-in-option-transactions.pdf
pdf_sha256: 343b8e738893d66d36df4b4902e7fb3eadba819afd30b498f25f8e6496b6ebc2
filone: opzioni-gamma
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati OPRA (ogni quota e ogni scambio di opzioni USA), TAQ della NYSE e volatilita' implicita di Bloomberg, a pagamento"
---

# Holowczak, Hu, Wu 2011 (Working Paper): L'Analisi

*Consolidating Information in Option Transactions*, Richard Holowczak, Jianfeng Hu, Liuren Wu (Zicklin
School of Business, Baruch College). **Letto il working paper**: SSRN 1740046, "First draft: January 13,
2011", 28 pagine, dalla prima all'ultima (riferimenti, quattro tabelle e quattro figure comprese). **Non e'
la versione pubblicata** (Journal of Derivatives 21(3), 2014, pagine 9-23, con il titolo *Aggregating
Information in Option Transactions*), che non abbiamo: **titolo, campione, misure e numeri possono essere
cambiati con la revisione**, e nessun numero qui sotto va attribuito alla versione pubblicata. E' una prima
stesura, **non revisionata**, e lo si vede: ci sono incongruenze fra testo e tabelle (elencate sotto).

Come e' stato letto: `pdftotext -layout` per il testo, `-raw` e `-table` per le tabelle (le quattro tabelle
escono pulite in `-raw`, tre sono ruotate nella pagina), `-raw` per le formule 9 e 10 (il segno di radice
quadrata non esce come carattere ne' in `-layout` ne' in `-raw`; vedi sotto). Le **figure 1-4 non sono leggibili**: sono grafici senza testo e su questo PC manca
`pdftoppm` per vederle come immagine; di loro si riportano solo didascalie e cio' che ne dice il testo.
**Nessuna istruzione** trovata nel PDF. Citazioni `[HHW §n]`, `[HHW Tab. n]`, `[HHW eq. n]`.

## In Parole Semplici

Su un indice si scambiano centinaia di opzioni diverse (prezzi d'esercizio e scadenze). Per sapere se chi
le scambia "spinge" il prezzo su o giu', bisogna sommarle in un numero solo, e il paper prova **come**
sommarle: 72 modi diversi, su QQQQ (l'ETF del Nasdaq-100, oggi QQQ), un minuto alla volta, nel 2006.
Risultato: **nello stesso minuto** la pressione d'acquisto sulle opzioni va con il prezzo, e parecchio
(correlazione fino a 0,41); **sul minuto dopo** quasi niente (correlazione intorno a 0,01, cioe' un
centesimo di "spiegazione" per cento). Il modo di sommare che va meglio nello stesso minuto e' pesare ogni
scambio per il **delta** e per il **logaritmo della taglia**; per il minuto dopo, la **taglia intera**.
Per noi: il flusso delle opzioni **descrive** il minuto, non lo anticipa.

## I Termini

- **Flusso degli ordini** (*order flow*) e **pressione d'acquisto** (*buy pressure*, BP): scambi iniziati
  da chi compra meno scambi iniziati da chi vende, in una finestra di tempo [§2].
- **Chi ha iniziato lo scambio**: OPRA non lo dice; gli autori lo stimano con l'algoritmo di **Lee e
  Ready**: sopra il prezzo medio fra denaro e lettera e' un acquisto, sotto una vendita, a meta' decide
  l'ultimo prezzo diverso [§3.2].
- **PBP** (*price buy pressure*): la pressione netta sul **prezzo**: comprare call e vendere put spinge su,
  il contrario giu' [§2.1, eq. 1].
- **VBP** (*volatility buy pressure*): la pressione netta sulla **volatilita'**: comprare call o put e'
  comprare volatilita' [eq. 2].
- **Delta**: di quanto si muove l'opzione se il sottostante sale di un dollaro; positivo per le call,
  negativo per le put. **Vega**: di quanto si muove se la volatilita' sale; positivo per call e put,
  massimo vicino al denaro.
- **n(d1)**: la densita' della normale standard calcolata in d1, il numero della formula di Black e Scholes
  [eq. 9]. E' massima per le opzioni **al denaro** (prezzo d'esercizio vicino al prezzo) e piccola per
  quelle lontane: e' un peso "quanto e' vicina al denaro".
- **ITM, ATM, OTM**: dentro, al, fuori dal denaro. Nel paper: al denaro se 0,375 <= |delta| <= 0,625,
  fuori se |delta| < 0,375 [Tab. 1].
- **Rendimento in eccesso** (*excess return*): il rendimento di un minuto ripulito dal "rimbalzo" fra denaro
  e lettera con un modello MA(1) stimato giorno per giorno; e' il residuo di quel modello [§3.4].
- **Correlazione contemporanea** e **di previsione**: fra la pressione e il rendimento dello **stesso**
  minuto, o fra la pressione di un minuto e il rendimento del **minuto dopo** [§4.1].

## La Domanda

Due domande di metodo [§1]: (1) come si somma il flusso di tante opzioni diverse sullo stesso sottostante
(prezzi d'esercizio e scadenze) per estrarne l'informazione su **prezzo** e **volatilita'** del sottostante;
(2) come si sommano gli scambi di **taglia** diversa (numero di scambi, volume, logaritmo del volume). Piu'
una terza: se conviene filtrare via le opzioni poco scambiate [§2.3].

## Il Metodo

**I dati** [§3.1, Tab. 1]: OPRA (tutti gli scambi e le quote di opzioni USA) sulle opzioni di **QQQQ**,
231 giornate dal 1 febbraio al 29 dicembre **2006**; il sottostante dal TAQ (quote NBBO, il migliore denaro
e lettera nazionale). Esclusi: errori, opzioni che scadono entro 10 giorni di calendario, scambi fuori orario,
i primi 15 minuti e gli ultimi 5 (la "rotazione d'apertura"). Restano gli scambi fra le **9:45:00 e le
15:54:59**: 1.107.061 scambi (da 1.572.865), 4.792 al giorno; taglia media 70 contratti, **mediana 5**
[§3.1, Tab. 1].

**Il campione descritto** [Tab. 1]: 515.359 scambi su call e 591.702 su put; volume medio giornaliero
311.246 contratti; scambi al denaro 36,69%, fuori dal denaro 41,26% (sulle put 42,88%), a scadenza entro 60
giorni 79,91%; scambi iniziati da chi compra 52,12% (call 49,38%, put 54,51%); non classificati 0,86%.

**Il segno** [§3.2]: Lee e Ready con le quote **dello stesso istante e della stessa borsa** (t-0), non quelle
di cinque secondi prima e non la NBBO, perche' con t-0 piu' scambi cadono esattamente sul denaro o sulla
lettera. Gli scambi non classificati si scartano.

**Le misure** [§2]: tre dimensioni.

1. **Fra prezzi d'esercizio e scadenze, cinque modi** [§2.1]:

```text
one pair         PBP = Call BP - Put BP  (solo la coppia call/put piu' scambiata del giorno)   (1)
                 VBP = Call BP + Put BP                                                        (2)
equal weighting  PBP = somma su K,T di (Call BP - Put BP)                                      (3)
                 VBP = somma su K,T di (Call BP + Put BP)                                      (4)
delta per share  PBP = somma sugli scambi i di delta_i                                         (5)
vega per share   VBP = somma sugli scambi i di vega_i                                          (6)
delta per dollar PBP = somma sugli scambi i di delta_i / p_i     (p_i = prezzo dell'opzione)   (7)
vega per dollar  VBP = somma sugli scambi i di vega_i / p_i                                    (8)
HHW              PBP = somma su K,T di (Call BP - Put BP) / M  x n(d1)                         (9)
                 VBP = somma su K,T di (Call BP + Put BP) / radice(M) x n(d1)                  (10)

d1 = [ ln(F/K) + sigma^2 T / 2 ] / (sigma radice(T))      M = max{1, T x 12}  (scadenza in mesi, almeno 1)
```

   Le formule 9 e 10 sono ricontrollate in `-raw` sul PDF: la (9) divide per **M** ("We divide the weight by
   maturity"), la (10) per **radice di M** [§2.1]. Il segno di radice si perde nell'estrazione (in `-raw` resta
   solo uno spazio sopra la M della (10), non della (9)); la radice si ricava dalla frase "the denominator is
   [radice] M instead of M", che senza la radice non avrebbe senso. Le formule non sono state viste come
   immagine (manca `pdftoppm`). Le (5)-(8) non scrivono il segno dello scambio (+1 acquisto, -1 vendita): dal testo si
   capisce che c'e', ma la formula non lo mostra.

   **Il perche' dei pesi HHW** [§2.1]: stesso peso, di segno opposto, a call e put dello stesso K e T (come
   la parita' put-call: call meno put e' il sottostante, cosi' si toglie la volatilita'); n(d1) da' piu'
   peso alle opzioni al denaro, **perche' sono le piu' liquide**; 1/M punisce le scadenze lunghe, meno
   liquide. Nella VBP il vega cresce come radice di T e compensa il valore dell'opzione, per questo si
   divide per radice di M. **Attenzione**: la PBP HHW **non e' pesata per il delta**. Gli autori scrivono
   che la misura "tiene conto dell'esposizione delta" [§2.1, p. 8], ma nella formula il delta entra solo come
   segno opposto fra call e put. Il peso n(d1) e'
   uguale per call e put allo stesso K e misura la vicinanza al denaro; la misura pesata per il delta e'
   la *delta per share* (5).

2. **Fra taglie, quattro modi** [§2.2]: numero di scambi (ogni scambio conta 1), volume (conta la taglia),
   **log(volume)** (conta il logaritmo della taglia; uno scambio da 1 contratto vale 0), log(volume+1). La
   ragione del logaritmo: chi sa qualcosa spezza gli ordini; due scambi da N valgono 2 log N, piu' di uno
   da 2N, se N >= 3 [§2.2].

3. **Filtro**: con e senza le opzioni sotto i **50 scambi al giorno** [§2.3].

Cinque modi per quattro taglie per due pressioni fanno 40 misure; con il filtro (che non si applica a *one
pair*) **72** [§2, §2.3].

**Le variabili da spiegare** [§3.4, Tab. 2]: il rendimento di un minuto di QQQQ dalla NBBO (media -0,0063
punti base, deviazione 4,5374), ripulito con un MA(1) **stimato per ogni giornata**: media -0,0022 pb,
deviazione 4,5157 pb; la volatilita' come deviazione standard dei rendimenti **secondo per secondo** dentro
il minuto, annualizzata: media 3,03% (deviazione 1,90%). I greci con Black e Scholes, **tasso e dividendo
zero**, e la **volatilita' implicita a tre mesi di Bloomberg** [§3.4].

**La frequenza** [§3.3]: finestre di **un minuto** non sovrapposte. Provate anche da 5 secondi a mezz'ora
(§3.3) o a 15 minuti (§4.1 e figure): "non cambiano la conclusione".

**Il test** [§4, Tab. 3-4]: **correlazioni semplici**, una per misura, con le stelle al 1%, 5%, 10%. Nessuna
regressione, nessun controllo, nessun campione fuori, nessuna strategia, nessun costo. Come siano calcolati
i livelli di significativita' **non e' detto**.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Nello stesso minuto tutte le PBP vanno con il rendimento**, tutte significative all'1% (36 su 36). Il contrario di Easley, O'Hara e Srinivas (1998), che trovavano un segno negativo a 5 minuti | §4.1, Tab. 3A |
| 2 | **La piu' alta**: *delta per share*, log(volume), senza filtro, **0,4112**; poi *equal weighting* 0,3962 e HHW 0,3883 (stessa riga); con il filtro 0,3887, 0,3746, 0,3748. *Delta per dollar* 0,3358, *one pair* 0,2208 | §4.1, Tab. 3A |
| 3 | **La taglia conta piu' dei pesi**: in ogni colonna log(volume) e' il migliore, log(volume+1) secondo. Con il volume pieno le correlazioni crollano: *delta per share* 0,1052, HHW 0,0933, *equal* 0,0808. Il numero di scambi sta in mezzo (0,2855, 0,1923, 0,1869) | §4.1, Tab. 3A |
| 4 | **Il filtro dei 50 scambi** abbassa la correlazione contemporanea in tutte le colonne con il filtro (per esempio 0,4112 -> 0,3887) | §4.1, Tab. 3A |
| 5 | La correlazione contemporanea **cresce con la lunghezza della finestra** (da 5 secondi a 15 minuti), e sotto log(volume) i modi di pesare quasi si equivalgono, tranne *one pair*. Le didascalie dicono che le figure 1-4 usano **solo i dati filtrati** | §4.1, Fig. 1 (solo dal testo e dalla didascalia: la figura non e' leggibile) |
| 6 | **Sul minuto dopo quasi niente.** Le correlazioni con il rendimento del minuto dopo vanno da -0,0035 a **0,0110**. Il numero di scambi non predice (nessuna significativa). Il **volume** e' il migliore: HHW 0,0110 (1%), *one pair* 0,0107 (1%), *delta per share* 0,0103 (1%), *equal* 0,0084 (5%). Con log(volume): *one pair* 0,0089 (1%), HHW 0,0061 (10%) e 0,0069 con il filtro (5%); *delta per share* 0,0027, non significativa | §4.1, Tab. 3B |
| 7 | **Le VBP e la volatilita' dello stesso minuto**: correlazioni piccolissime, al massimo **0,0205** (*equal* e HHW, log(volume), con il filtro). Qui il filtro **alza** la correlazione (*vega per share* log(volume) da 0,0049 a 0,0198). *Vega per dollar* log(volume) senza filtro e' **negativa e significativa** (-0,0097, 1%) | §4.2, Tab. 4A |
| 8 | **Le VBP e la volatilita' del minuto dopo**: la piu' alta HHW log(volume) senza filtro, **0,0203** (1%); poi *vega per share* con il filtro 0,0200 e HHW con il filtro 0,0199. *Vega per dollar* mai significativa | §4.2, Tab. 4B |
| 9 | Le conclusioni degli autori: le pressioni sono informative su rendimento e volatilita'; log(volume) e' il modo migliore fra le taglie, **tranne che per prevedere il rendimento**, dove vince il volume; *per share*, *equal* e HHW meglio di *one pair* e *per dollar*; il filtro conta meno per la PBP che per la VBP. "Quale metodo scegliere dipende dalla domanda empirica" | §5 |

**Quanto pesano questi numeri** (calcolo nostro, non del paper): una correlazione di 0,41 vuol dire che la
pressione dello stesso minuto "spiega" circa il 17% della varianza del rendimento di quel minuto; una di 0,011
spiega circa **lo 0,01%** di quella del minuto dopo. Le stelle si spiegano con il numero di osservazioni:
370 minuti al giorno (9:45-15:55) per 231 giornate fanno circa **85.470 minuti**; con osservazioni
indipendenti l'errore di una correlazione e' circa 1/radice(85.470) = 0,0034, e le soglie 1%, 5%, 10%
cadono a 0,0088, 0,0067, 0,0056. **Tutte le stelle delle tabelle 3 e 4 coincidono con queste soglie**: la
significativita' e' quasi certamente **per minuto, come se i minuti fossero indipendenti**, non per giornata.

## Il Perche'

Il meccanismo degli autori [§1, §2]: chi ha un'informazione privata la porta sulle opzioni per la **leva**
(Black 1975) e per **nascondersi** fra tanti contratti (Easley, O'Hara, Srinivas 1998), ma paga costi alti;
quindi sceglie i contratti mettendo insieme **esposizione** (delta o vega), **leva** (esposizione per dollaro)
e **liquidita'** (spread e impatto), e la liquidita' sta sulle opzioni vicine al denaro e a breve scadenza.
Spezza gli ordini per non farsi vedere (Anand e Chakravarty 2003), da cui il logaritmo della taglia. Su un
ETF l'informazione e' soprattutto **di liquidita'** (chi deve comprare o vendere molto nei prossimi
secondi o ore), non sui fondamentali; per questo scelgono finestre brevi [§3.1, §3.3].

**Le alternative scartate** [§1, §4.1]: *one pair* ignora gli altri contratti (variabile omessa); *equal
weighting* presume che chi sa scelga a caso; il numero di scambi sopravvaluta i piccoli, il volume i grandi
(spesso pre-negoziati); *per dollar* sopravvaluta gli scambi da pochi centesimi delle opzioni molto fuori
dal denaro e ignora la liquidita'.

**L'alternativa che non discutono**: che la correlazione dello **stesso minuto** venga dal prezzo che
muove le opzioni, e non dalle opzioni che muovono il prezzo. Se il sottostante sale, gli scambi sulle call
eseguiti a quote non ancora aggiornate cadono sopra la media e Lee e Ready li segna come acquisti; e chi
copre o insegue il movimento compra call nello stesso minuto. Una correlazione contemporanea non dice chi
guida. Il segno solo debole sul minuto dopo e' compatibile con entrambe le letture.

**Cosa lo ucciderebbe** (dedotto, gli autori non lo dicono): una correlazione contemporanea che resta
uguale anche quando gli scambi si segnano con quote vecchie di qualche secondo (sarebbe meccanica); una
correlazione di previsione che sparisce con errori robusti all'autocorrelazione o per giornata; un fuori
campione (un altro anno, un altro sottostante) dove l'ordine dei metodi cambia.

## Cosa Non Regge O Non Si Dice

1. **"Predice" e' detto piu' forte di quanto le tabelle mostrino.** L'abstract dice che la pressione
   delta "predice positivamente" il rendimento [abstract]; la tabella 3B da' correlazioni fra -0,0035 e 0,0110 (quelle con le stelle fra 0,0058 e 0,0110),
   e la *delta per share* con log(volume), la misura che vince nello stesso minuto, **non e' significativa**
   sul minuto dopo (0,0027). Il finding robusto e' **contemporaneo**.
2. **La significativita' e' per minuto e da indipendenza** (calcolo nostro, sopra): nessuna correzione per
   autocorrelazione, eteroschedasticita' o raggruppamento per giornata. Con 231 giornate stimate una per una,
   l'errore resterebbe circa lo stesso se i minuti fossero davvero indipendenti, e crescerebbe se dentro la
   giornata o fra le giornate c'e' dipendenza: il paper non lo controlla, quindi le stelle non bastano
   (calcolo nostro). **Il nostro metodo chiede la
   significativita' per giornata** (CLAUDE.md, obbligo 3): qui non c'e'.
3. **Nessun modello nullo, nessun campione fuori, nessun costo, nessuna strategia.** E' un confronto di
   correlazioni nel campione.
4. **Molte varianti, nessuna correzione**: 72 misure, due variabili da spiegare, due orizzonti, e piu'
   frequenze (5 secondi - 30 minuti) e filtri alternativi "provati" ma non riportati [§2.3, §3.3]. La
   scelta del migliore e' fatta sulle stesse tabelle.
5. **Un anno, un sottostante, il 2006**: niente 0DTE, scadenze settimanali non citate, un mercato delle
   opzioni diverso da oggi. Esclusi gli ultimi 10 giorni di scadenza [§3.1], cioe' proprio la parte che
   oggi domina i volumi sugli indici.
6. **Il rimbalzo fra denaro e lettera e' tolto con un piccolo look-ahead** (dedotto): il paper dice solo che il
   MA(1) e' stimato "for each trading day" [§3.4], cioe' un coefficiente per giornata, calcolato su tutti i
   minuti di quella giornata. Il residuo di un minuto dipende dai minuti passati e da quel coefficiente, quindi
   i minuti successivi entrano solo attraverso il coefficiente. Per una correlazione descrittiva va bene; dal
   vivo il coefficiente va stimato sui giorni precedenti.
7. **I greci con una sola volatilita'** (implicita a tre mesi di Bloomberg, tasso e dividendo zero) per
   tutti i prezzi d'esercizio e le scadenze [§3.4], mentre la (5) parla di delta "implicito nel prezzo" di
   ogni scambio [§2.1]: le due frasi non si conciliano. Opzioni americane su un ETF valutate con Black e
   Scholes europeo, senza dirlo.
8. **La volatilita' misurata e' strana**: media annualizzata 3,03% [Tab. 2], molto sotto la volatilita'
   annua tipica di un indice azionario. Viene da rendimenti secondo per secondo della NBBO, in gran parte
   zero; gli autori non lo spiegano (scrivono solo "only 3.03%"). Il testo dell'introduzione dice invece che la
   VBP predice la volatilita' "misurata dalle volatilita' implicite" [§1, p. 3], che non e' la misura usata
   nelle tabelle; in §3.4 dicono che con l'implicita (e con massimo meno minimo) i risultati non cambiano,
   senza mostrarlo.
9. **Incongruenze della prima stesura** (da guardare prima di citare un numero):
   - scambi iniziati da chi compra: 51,81% nel testo [§3.2], 52,12% nella tabella 1;
   - la didascalia della tabella 1 dice "fra le 9:30:00 o dopo le 4:00:00", il testo 9:45:00-15:54:59 [§3.1];
   - "il numero di scambi ha coefficienti da due a tre volte quelli del volume, tranne *one pair*" [§4.1]:
     falso per *delta per dollar* (0,0746 contro 0,0619), e appena sotto il doppio con il filtro per *equal*
     (0,1515 contro 0,0807) e HHW (0,1768 contro 0,0899) [Tab. 3A];
   - "HHW sempre al primo posto" nella previsione [§4.1]: nella riga log(volume) *one pair* (0,0089) batte
     HHW (0,0061 e 0,0069) [Tab. 3B];
   - "log(volume) batte le altre taglie" anche per la VBP contemporanea [§4.2]: falso per *vega per share*
     senza filtro (0,0049 contro 0,0094 del numero di scambi) e per *vega per dollar* (-0,0097) [Tab. 4A];
   - "HHW senza filtro batte le altre VBP senza filtro" [§4.2]: falso con il numero di scambi (HHW 0,0074,
     *vega per share* 0,0094) [Tab. 4A];
   - "il filtro migliora molto la correlazione contemporanea della VBP" [§4.2]: vero con log(volume) e
     log(volume+1); con numero di scambi e volume resta quasi uguale o scende (*equal* 0,0044 -> 0,0040, HHW
     volume 0,0101 -> 0,0098) [Tab. 4A];
   - "il filtro migliora leggermente la previsione della PBP" [§4.1]: vero con numero di scambi, log(volume)
     e log(volume+1); con il volume, proprio la taglia che prevede meglio, e' falso per *delta per share*
     (0,0103 -> 0,0099), *delta per dollar* (0,0040 -> 0,0033) e HHW (0,0110 -> 0,0100) [Tab. 3B];
   - le tabelle 2 e 4 intitolano "delta per share / delta per dollar" anche le colonne della VBP, che sono
     vega; il pannello B della tabella 4 si chiama "Forecasting t+1 returns" ma e' la volatilita'; le
     didascalie delle figure 3 e 4 parlano di "price buy pressures"; le figure dicono "50 contratti" al
     giorno, il testo "50 scambi"; nella tabella 2 una deviazione (24,65) e' senza parentesi.
10. **Nessuna informazione su chi scambia**: OPRA non dice se e' un cliente, un market maker o una firm;
    tutto il flusso e' sommato, e il segno e' stimato. Non dice niente sui gruppi.
11. **Il working paper non e' la versione pubblicata**: la versione del Journal of Derivatives (2014) puo'
    avere un campione, misure, test e numeri diversi. Questa analisi vale per la stesura del 13 gennaio 2011.

## Cosa Vale Per Le Due Ricerche

**[Opzioni fra indici](../../ricerche/2026-11-09-opzioni-fra-indici.md)** (il flusso delle opzioni su QQQ
pesato per delta e vega, intraday). Due correzioni a quanto scritto in fase 1 dai riassunti:

- "entrambe le misure predicono rendimento e volatilita' **nello stesso intervallo e nei successivi**": nello
  stesso minuto si', forte (fino a 0,41); nei successivi solo il minuto dopo, e con correlazioni di circa
  0,01 per il prezzo e 0,02 per la volatilita', significative solo come minuti indipendenti [Tab. 3B, 4B];
- "pesato per il delta": la misura proposta (HHW) pesa per **n(d1)/M**, cioe' per vicinanza al denaro e
  scadenza breve, non per il delta; la pesata per il delta e' la *delta per share*, che vince nello stesso
  minuto e **non** sul minuto dopo con log(volume).

Resta vero: un solo sottostante (QQQQ), **nessun confronto fra indici**. Il paper non dice niente su QQQ
contro SPX o NQ contro ES; dice solo **come si costruisce** il flusso di un indice prima di confrontarlo.

**[Flusso delle opzioni per gruppo](../../ricerche/2026-11-09-flusso-delle-opzioni-per-gruppo.md)** (come si
somma e si normalizza il flusso per una lettura dal vivo):

- **come si somma**: con il segno di chi inizia, pesando per l'esposizione (delta per il prezzo, vega per la
  volatilita'), con call e put di segno opposto nella PBP e uguale nella VBP [§2.1]; la taglia conta piu'
  del peso fra contratti [Tab. 3A, Fig. 1];
- **il DEX di X-Ray e' la variante "delta x volume"** (delta per contratto per numero di contratti): nel
  paper e' la combinazione con la correlazione contemporanea **piu' bassa** fra le *delta per share* senza
  filtro (0,1052 contro 0,4112 con log(volume); la *delta per dollar* con il volume e' ancora piu' bassa, 0,0619) e, insieme agli altri pesati per il volume, la **migliore** sul
  minuto dopo (0,0103) [Tab. 3]. Il log della taglia richiede gli scambi uno per uno, che il DEX per minuto
  non da';
- **come si normalizza**: il paper **non normalizza** (niente divisione per il volume, niente z-score, niente
  confronto alla stessa ora); riporta media e deviazione di ogni misura [Tab. 2] e verifica che siano
  stazionarie (Dickey-Fuller, p < 0,001) [§3.4]. Per la normalizzazione la fonte resta un'altra;
- **conferma la lettura gia' scritta** in [x-ray-in-vivo](../../../metodo/x-ray-in-vivo.md): il flusso
  va con il prezzo nello stesso tempo e non lo anticipa in modo utile. Il paper non separa i gruppi, quindi
  non dice nulla su chi abbia ragione.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

- **Il segno lo da' gia' ATAS** sul future: il tape di NQ ha il lato dell'aggressore, non serve Lee e Ready.
  Per le opzioni, invece, non abbiamo scambi singoli con il segno: la catena CME di ATAS da' open interest e
  volume, e X-Ray da' il **DEX per minuto e per strike** di SPX, gia' sommato (non per scambio). **Da
  verificare** se la catena CME di ATAS porti gli scambi delle opzioni di NQ con il lato.
- **Rifacibile**: la correlazione fra il DEX di un minuto (X-Ray, SPX) e il rendimento di ES o NQ nello
  stesso minuto e nel minuto dopo, **con una stima per giornata** e il nullo per permutazione delle giornate;
  con il DEX per strike si puo' anche ripesare per n(d1)/M (calcolando d1 per strike), cioe' la misura HHW
  senza il log della taglia.
- **Il vincolo**: X-Ray c'e' solo da ottobre 2026, e **su NQ i dati dal 2025 sono fuori campione**
  (divisione fissata il 15 ottobre 2026): un test su NQ lo consumerebbe. Come lettura descrittiva (contesto)
  va bene; come prova, solo su ES dopo averne fissato la divisione, o con la storia che arrivera'.
- **Non rifacibile**: il confronto fra le 72 misure, che vuole gli scambi uno per uno con taglia e prezzo
  (OPRA); la VBP contro la volatilita' secondo per secondo del sottostante.

## Cosa Ne Prende Il Framework

- **Il metodo, non un'idea**: il flusso delle opzioni si costruisce con il segno, il peso dell'esposizione
  e una scelta dichiarata sulla taglia; chi lo usa dice quale delle varianti usa.
- **La distinzione contemporaneo / previsione**: un flusso che va con il prezzo nello stesso minuto **non e'
  un segnale**. Ogni lettura del delta scambiato dal vivo lo dice.
- **Un esempio di quello che il nostro metodo evita**: significativita' per minuto come se i minuti fossero
  indipendenti, 72 varianti senza correzione, nessun nullo e nessun fuori campione.
- **Nessuna regola operativa**: il paper non ne propone e i suoi numeri non la reggerebbero.

## La Verifica

**9 novembre 2026**, verifica avversaria (fase 4) sul PDF del working paper (sha256 confermato
`343b8e738893d66d36df4b4902e7fb3eadba819afd30b498f25f8e6496b6ebc2`, 28 pagine). Testo con `pdftotext -layout` e
`-raw`; le tabelle 3 e 4 rilette anche con `-table`, che conferma l'ordine di `-raw` per la colonna *one pair*
dei pannelli B (in `-layout` quella colonna e' sfalsata di una riga). Figure e formule non viste come immagine
(manca `pdftoppm`). Nessuna istruzione nel PDF.

**Circa 140 affermazioni controllate** (numeri, citazioni, finding): **circa 129 confermate**, **7 corrette**,
**nessuna tolta**, **4 aggiunte**. Confermati tutti i numeri delle tabelle 1-4 riportati (fra cui 0,4112,
0,1052, il minuto dopo fra -0,0035 e 0,0110, 0,0205 e 0,0203), le formule 1-10 (con la riserva sulla radice
della 10), le incongruenze gia' elencate al punto 9, e il calcolo delle stelle: 370 minuti per 231 giornate =
85.470, errore 0,00342, soglie 0,0088, 0,0067, 0,0056; **le stelle e le assenze di stelle delle tabelle 3 e 4
coincidono con queste soglie** (ricontrollate valore per valore, compreso il caso al limite 0,0056 senza
stella nella 4B).

Le correzioni:

1. punto 1 di "Cosa Non Regge": la tabella 3B va da -0,0035 a 0,0110, non "fra 0,003 e 0,011";
2. punto 2: "per giornata l'errore sarebbe ben piu' grande" non segue dall'indipendenza (con minuti
   indipendenti resterebbe circa lo stesso); riscritto come dipendenza non controllata;
3. punto 6: il paper dice solo "for each trading day"; il MA(1) toglie il rimbalzo fra denaro e lettera, non
   gli effetti di "dentro la giornata", e il look-ahead passa solo dal coefficiente della giornata;
4. "il DEX e' la piu' bassa fra le pesate per il delta": vero solo fra le *delta per share* (la *delta per
   dollar* con il volume e' 0,0619);
5. formule 9-10: la radice non si legge nemmeno in `-raw`, si ricava dalla frase del testo;
6. HHW: aggiunto che gli autori dicono che la misura tiene conto dell'esposizione delta;
7. punto 8: gli autori notano il 3,03% ("only") senza spiegarlo, e dichiarano la robustezza con la
   volatilita' implicita senza mostrarla.

Le aggiunte: le figure usano solo i dati filtrati (didascalie); due incongruenze in piu' del §4.2 (HHW non
batte le altre con il numero di scambi; il filtro non migliora la VBP con numero di scambi e volume); il
rapporto "da due a tre volte" e' appena sotto 2 anche per *equal* e HHW con il filtro.

**Resta incerto**: la radice della formula 10 (letta dal testo, non dall'immagine); le figure 1-4; come
siano calcolate le stelle (coincidono con l'ipotesi di minuti indipendenti, ma il paper non lo scrive); se
la versione pubblicata del 2014 abbia gli stessi numeri.
