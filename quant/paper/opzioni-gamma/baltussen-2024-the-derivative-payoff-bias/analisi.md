---
chiave: baltussen-2024-the-derivative-payoff-bias
sigla: BTW
titolo: "The Derivative Payoff Bias"
autori: [Guido Baltussen, Julian Terstegge, Paul Whelan]
versione_letta: gennaio 2024
fonte: SSRN (PDF letto dal sito dell'AFA)
identificativo: "4562800"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4562800
tipo: preprint
revisione_fra_pari: no
pagine: 73
pdf: baltussen-2024-the-derivative-payoff-bias.pdf
pdf_sha256: 841d9c2e183e6f046eab49ed040636b392cd3c025d7021811d2dd8dfed6373f7
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; i dati (Refinitiv, Bloomberg, CBOE Open-Close, OptionMetrics) sono a pagamento"
---

# Baltussen, Terstegge E Whelan 2024: L'Analisi

*The Derivative Payoff Bias*, Guido Baltussen (Erasmus University Rotterdam e Northern Trust Asset
Management), Julian Terstegge (Copenhagen Business School), Paul Whelan (Chinese University of Hong
Kong). Versione di gennaio 2024, 73 pagine (48 di paper, 24 di appendice online), SSRN 4562800.
Working paper, non revisionato. Il PDF letto viene dal sito dell'AFA
(<https://afajof.org/management/viewp.php?n=98196>), non da SSRN. Letto per intero il 14 ottobre
2026. Citazioni `[BTW §n]`, `[BTW Tab. n]`, `[BTW OA Tab. A.n]` per l'appendice online.

**Il numero in dollari dipende dalla versione.** Questa versione dice "circa 4 miliardi l'anno nelle
sole opzioni SPX" [BTW abstract, §VIII], 3,8 miliardi con il calcolo dei pagamenti [BTW Tab. VII] e
2,7 miliardi con il calcolo dei rendimenti [BTW §V]. Una versione SSRN successiva direbbe circa 2,5
miliardi: non l'abbiamo letta, non e' verificato.

**Il testo estratto e' imperfetto.** `pdftotext` mescola le colonne delle tabelle. I numeri qui sotto
sono stati rimessi nelle colonne con due controlli interni: la riga "24 ore" deve valere media x 24
/ ore della finestra, e i numeri citati nel testo devono ritrovarsi in tabella. La formula del charm
[BTW eq. 4] esce storpiata e non si riporta. Le figure non si sono potute guardare (manca
`pdftoppm`): cio' che se ne dice viene dal testo e dalle didascalie.

## Le Parole

- **Settlement (regolamento)**: il prezzo con cui un derivato in scadenza viene pagato. Qui e' in
  contanti: chi ha la call riceve max(0, prezzo di regolamento - strike), senza consegna di azioni.
- **Terzo venerdi'**: il giorno di scadenza mensile delle opzioni sull'indice S&P 500 (SPX). Nei mesi
  di marzo, giugno, settembre e dicembre scadono insieme opzioni sull'indice, future (anche ES) e
  opzioni sui future: e' il **triple witching**. Se il terzo venerdi' e' festivo, si scade il giovedi'
  prima (quattro volte nel campione) [BTW nota 2].
- **a.m. e p.m.**: le opzioni "a.m." si regolano all'apertura del terzo venerdi', le "p.m." alla
  chiusura. Le a.m. smettono di scambiarsi **il giovedi' alle 16:15 ET**, 17 ore e 15 minuti prima
  del regolamento [BTW §III.A].
- **SOQ (Special Opening Quotation)**: il prezzo di regolamento a.m. dell'S&P 500. E' la somma pesata
  del **primo scambio di ogni titolo** sulla sua borsa principale dopo le 9:30 ET. Non e' l'apertura
  dell'indice delle 9:30 (che usa le chiusure di ieri per i titoli non ancora aperti), e non e' un
  prezzo a cui si possa comprare: si pubblica 30-45 minuti dopo l'apertura [BTW §III.A, OA A.1.D].
- **Delta**: quanto cambia il valore di un'opzione per un punto dell'indice. Chi ha un portafoglio di
  opzioni "delta neutrale" compra o vende il sottostante per tenere il delta a zero.
- **Charm**: quanto cambia il delta **col solo passare del tempo**, a prezzo fermo. Vicino alla
  scadenza e' grande per le opzioni vicine allo strike: il delta di una call appena fuori dal denaro
  scivola verso 0, quello di una appena dentro verso 1. Chi vuole restare neutrale deve ricoprirsi
  anche se il prezzo non si muove [BTW §VI.A, Fig. 8].
- **3FPS (Third Friday Price Spike)**: il nome che gli autori danno al fenomeno: salita nella notte
  prima della scadenza, ritorno dopo l'apertura.

## La Domanda

I prezzi con cui si pagano i derivati sull'indice americano alla scadenza del terzo venerdi' sono
distorti? Di quanto, chi ci guadagna, e perche'?

## Il Metodo

**I dati** [BTW §II]: la SOQ giornaliera da Bloomberg (1992-2021); scambi e migliori proposte
dell'indice SPX e del future E-mini S&P 500 (ES) da Refinitiv, ricampionati a 1, 5 e 15 minuti; le
opzioni SPX a 15 minuti dal CBOE e di fine giornata da OptionMetrics; i volumi giornalieri comprati
e venduti per categoria di operatore dal dataset CBOE Open-Close (2006-2020), **cumulati** per
ottenere delle posizioni; gli indici BXM e PUT del CBOE.

**Il campione**: la SOQ da novembre 1992; il campione principale **da febbraio 2003 a dicembre
2021**, scelto con un test di rottura strutturale (supF) che la colloca al 17 gennaio 2003 [BTW
§III.C]. Esclusi i regolamenti di settembre 2001, settembre e ottobre 2008 [BTW nota 7]. **225
scadenze**, di cui 143 con la notte positiva [BTW Fig. 5]; dalle statistiche t si ricava che sono
circa 75 trimestrali e 150 mensili (conto nostro, coerente con la Tab. III).

**Le misure**: `ReturnSOQ = log(SOQ_t) - log(chiusura SPX_t-1)` [BTW eq. 1], confrontata fra giorni
di scadenza, altri giorni e altri venerdi' [BTW Tab. I]. Poi i rendimenti per finestra intorno alla
scadenza (mercoledi' chiusura -> giovedi' apertura, giovedi' apertura -> chiusura, **giovedi'
chiusura -> venerdi' apertura**, **venerdi' apertura -> chiusura**, venerdi' chiusura -> lunedi'
apertura), con tre serie di prezzi: SOQ e chiusura SPX, apertura e chiusura SPX, ES [BTW Tab. II].
Per l'ES: prezzi medi pesati per volume (VWAP) delle barre a 5 minuti, il contratto principale, e nei
giorni trimestrali il passaggio al contratto successivo alle 9:30 [BTW §IV].

**La significativita'**: il t della media dei rendimenti per evento, **un'osservazione per
scadenza**, e il t della differenza con gli altri giorni. Un evento e' una giornata, quindi e' gia'
una significativita' per giornata. In piu' un bootstrap a blocchi (1.000 ricampionamenti, blocco
ottimale di Patton, Politis e White) [BTW OA A.2.B-C].

**La strategia** [BTW §IV.D, Tab. V]: compra un ES alla chiusura del giovedi' (16:00 ET), alla
apertura del venerdi' (9:30) gira in vendita, chiude alla chiusura del venerdi'. Due versioni:
prezzi a meta' fra domanda e offerta, oppure acquisti all'offerta e vendite alla domanda. Nessuna
commissione. Lo Sharpe si annualizza sui 12 eventi l'anno, come in Lucca e Moench [BTW nota 18]; i
numeri tornano con la radice di 12.

**Il trasferimento di ricchezza** [BTW §V]: (1) i rendimenti notturni delle opzioni contro un
controfattuale con il rendimento notturno medio; (2) i pagamenti con la SOQ contro i pagamenti con la
chiusura del giovedi', moltiplicati per l'open interest [BTW eq. 2-3].

**Le spiegazioni** [BTW §VI, OA A.3]: notizie (utili, macro), vincoli di finanziamento, pinning
(il prezzo attirato dagli strike), poi la copertura dei dealer (delta e charm) e la manipolazione.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | SOQ meno chiusura del giorno prima, 2003-2021: **+18,24 bps nei giorni di scadenza** (t 4,53), +1,31 negli altri giorni, differenza 16,92 (t 3,50); negli altri venerdi' -0,97 (t -0,36). Nel 1992-2003: -3,25 (t -0,59) | Tab. I |
| 2 | la data della rottura spostata di 6 mesi avanti o indietro: la differenza resta fra 15,9 e 16,9 bps, t fra 3,3 e 3,5 | OA Tab. A.3 |
| 3 | **ES**, scadenze 2003-2021: giovedi' 16:00 -> venerdi' 9:30 **+14,30 bps** (t 3,54, deviazione 60,53); venerdi' 9:30 -> 16:00 **-12,89 bps** (t -2,40, deviazione 80,45). In tutti i giorni: +2,69 e +0,91 | Tab. II, pannello c |
| 4 | con SOQ e chiusura SPX: +18,54 (t 4,65) e -16,91 (t -2,99) | Tab. II, pannello a |
| 5 | la forma: la salita comincia verso le 2:00 ET (apertura di Londra), dura fino alle 9:30, e alle 12:00 e' tutta riassorbita | §IV, Fig. 1 |
| 6 | **asimmetria**: notte positiva in 143 scadenze su 225 (64%); torna indietro dalle 9:30 solo la notte positiva, quella negativa continua a scendere. Regressione del rendimento 9:30-12:00 su quello notturno: pendenza -0,32 (intervallo al 95% da -0,47 a -0,18), R2 12% | §IV.A, Fig. 5, OA Tab. A.4 |
| 7 | **trimestrali** (SOQ): notte +26,44 (t 4,36), giorno -37,23 (t -3,54). **Mensili**: notte +14,45 (t 2,81), giorno -6,74 (t -1,03) | Tab. III |
| 8 | 2010-2021, con le p.m. mensili in circolazione: notte +11,02 (t 2,25), giorno -17,62 (t -2,58), venerdi' chiusura -> lunedi' apertura -9,25 (t -1,23) | Tab. IV |
| 9 | **la strategia su ES**: lorda **+27,48 bps per evento** (t 3,94, deviazione 104,61, Sharpe 0,90); con domanda e offerta **+24,19** (t 3,47, Sharpe 0,80). La stessa strategia negli altri giorni: +0,07 (t 0,04) lorda, -3,21 netta. Alfa 27,55, beta -0,32 | Tab. V |
| 10 | positiva in 16 anni su 19 (non nel 2008, 2013, 2016); la versione con l'indice e la SOQ porta 1 $ a 2,22 $; bootstrap: dal 2,5% al 97,5% fra 22 e 47 bps | §IV.D, Fig. 4, OA Fig. A.2 |
| 11 | **NQ e YM** (Nasdaq 100 e Dow Jones): notte +14 e +12 bps, ritorno di misura simile entro mezzogiorno. Il Nasdaq 100 si regola dal 2004 con l'asta d'apertura (NOOP), non con la SOQ: non e' un artefatto del calcolo della SOQ. Nessun t e nessuna strategia riportati | §IV.E, Fig. 6 |
| 12 | squilibrio degli ordini su ES (volume dell'aggressore in acquisto su volume totale): notte prima della scadenza 1,88 (t 5,29) contro 0,01; venerdi' -0,89 (t -4,55) contro -0,01 | OA Tab. A.5 |
| 13 | opzioni SPX nella notte, contro il controfattuale: call al denaro **+34,4%**, put al denaro **-20,0%**. Totale 2,7 miliardi l'anno | §V, Tab. VI |
| 14 | pagamenti medi per scadenza: call 10,46 miliardi con la SOQ contro 10,23 con la chiusura del giovedi'; put 3,04 contro 3,12. Totale **3,8 miliardi l'anno** | Tab. VII |
| 15 | escluse: le sorprese degli utili (pendenza -6,5, t -1,22), le notizie macro (non significative; 37 giornate con dati d'inflazione, 90 di crescita), il pinning (distanza dallo strike uniforme, p sopra 0,20), l'open interest al denaro, i rendimenti passati | OA Tab. A.7-A.11, Fig. A.4 |
| 16 | **i dealer**: alla chiusura del giovedi' il delta netto nelle opzioni in scadenza e' circa zero (call +0,06 milioni, put -0,07). Il charm netto e' -26 miliardi: il loro delta scende di circa 50 milioni nelle 17 ore e 15 minuti, e per restare neutrali **comprano** | §VI.A, Tab. IX, Fig. 9 |
| 17 | i clienti professionali (fondi) sono lunghi di delta il giovedi', e lo aumentano nella settimana della scadenza; i clienti al dettaglio hanno la posizione opposta | §VI.B, §VII, Tab. X, Fig. 10 |
| 18 | il divario fra gli indici PUT e BXM, 1,35% l'anno: +2,00% nei giorni di scadenza, -0,63% negli altri, 0,01% negli altri venerdi' | OA Tab. A.6 |

## Cosa Abbiamo Riprodotto

Niente, per ora.

## Cosa Non Regge, O Non Si Dice

1. **Il campione e' piccolo per costruzione.** Dodici eventi l'anno: 225 in 19 anni, circa 75
   trimestrali. Per evento il segnale e' debole: media su deviazione di 0,24 per la notte su ES e
   0,26 per la strategia. Il t di 3,5-3,9 viene dagli anni, non dalla forza del singolo evento.
2. **Lo Sharpe di 1,3 non sta in nessuna tabella.** L'introduzione e le conclusioni dicono "lordo
   1,3, netto 0,8"; la Tab. V dice 0,90 lordo e 0,80 netto. Non si sa da dove venga 1,3.
3. **La versione piu' bella non e' negoziabile.** La Fig. 4 (1 $ -> 2,22 $), il bootstrap e le Tab.
   I, III e IV usano la SOQ, che non e' un prezzo a cui si esegue. La versione negoziabile e'
   quella su ES della Tab. V. Anche li': prezzi VWAP di barre a 5 minuti, non prezzi eseguiti; costi
   pari al solo spread; nessuna commissione; il giro di due contratti alle 9:30, il minuto piu'
   agitato della giornata. Con le nostre convenzioni (2,50 $ per lato e 1 tick per esecuzione, quattro
   esecuzioni per evento) il costo e' di 60 $ per contratto ES: a 5.000 punti, cioe' 250.000 $ di
   controvalore, sono 2,4 bps contro un lordo di 27 (conto nostro, con un livello d'esempio). I
   costi non uccidono l'effetto.
4. **Il ritorno non regge dappertutto.** Regge su ES (t -2,40) e nelle trimestrali (t -3,54). Non
   regge con i prezzi SPX di apertura (-2,60, t -0,43) ne' nelle scadenze mensili (-6,74, t -1,03),
   mentre il testo dice che "resta statisticamente significativo". La gamba solida e' la notte.
5. **La data d'inizio e' scelta sui dati.** Il 2003 viene da un test di rottura sugli stessi dati; prima
   del 2003 non c'e' niente. La spiegazione (la nascita degli scambi notturni) e' plausibile ma
   trovata dopo. Le scadenze escluse (2001, 2008) "non cambiano i risultati", ma non si vede.
6. **Le prove multiple non si contano.** Cinque finestre, tre serie di prezzi, tre indici,
   trimestrali e mensili, notti positive e negative, due uscite (la strategia chiude alle 16:00 [Tab.
   V], le regressioni alle 12:00 [OA Tab. A.4, A.12-A.14]), due calcoli della ricchezza. Nessuna
   correzione e nessun numero di varianti dichiarato.
7. **Le posizioni dei dealer sono ricostruite.** Sono volumi giornalieri cumulati, con un livello di
   partenza ignoto. Il "market maker" e' il residuo di "firms" e "customers", che dal 2011 contiene
   anche i clienti professionali: due gruppi che il paper confronta si sovrappongono. Il modello per
   delta e charm non e' detto. Le unita' di -26 miliardi di charm e di 50 milioni di delta non si
   confrontano con un delta netto di 0,06 milioni: non si puo' controllare. **E le posizioni non
   spiegano la grandezza del ritorno da un evento all'altro** [OA Tab. A.12-A.14]: per i dealer t fra
   0,46 e 0,86; per i clienti professionali un solo t di 2,17 su cinque specificazioni; l'interazione
   non e' significativa. Il sostegno alle due spiegazioni viene dalle medie, non dalla variazione fra
   eventi. La manipolazione, dicono gli autori, non si puo' identificare.
8. **"Causalmente riassorbiti"** [OA A.2.B] lo dice una regressione di rendimenti su rendimenti: e'
   una correlazione.
9. **Il controllo p.m. e' debole.** "Nessun effetto intorno al regolamento p.m." poggia su un numero
   solo, venerdi' chiusura -> lunedi' apertura: -9,25 bps, t -1,23 [Tab. IV]. Non e' la finestra prima
   del regolamento p.m., ed e' comunque negativo.
10. **Incoerenze del testo**: 2,12 miliardi nel §V contro 3,12 in tabella e nell'introduzione; i
    pannelli B e C della Tab. I scambiati rispetto alla didascalia; lo squilibrio degli ordini in
    "migliaia di contratti" nel testo e "frazione" in tabella, con t di 3,0 e -5,30 nel testo contro
    5,29 e -4,55 in tabella; 4, 3,8 e 2,7 miliardi a seconda della pagina.
11. **Il decadimento.** Il campione finisce a dicembre 2021. Dopo sono arrivate le opzioni 0DTE
    giornaliere, regolate p.m., e il peso delle a.m. si e' ridotto (gia' visibile nella OA Fig. A.1).
    Le intercette delle regressioni (variabili normalizzate, quindi medie) danno un ritorno fino a
    mezzogiorno di 36,24 bps nel 2006-2020 [OA Tab. A.12] e di 21,37 nel 2011-2020 [OA Tab. A.13]:
    se i campioni sono completi, il 2006-2010 vale circa 67 bps e l'effetto e' concentrato nei primi
    anni (deduzione nostra, approssimata). Gli autori dicono che lo Sharpe resta alto nel 2019-2021
    [Fig. 4b]. Qualunque dato nostro e' successivo al campione e alla diffusione del paper: e' un
    fuori campione vero.

## Cosa Ne Prende Il Framework

- **Un evento di calendario non ha look-ahead**: il terzo venerdi' si sa anni prima. La regola si
  congela prima di guardare.
- **I due nulli del paper sono quelli giusti**: le stesse finestre in tutti gli altri giorni [Tab. V,
  colonna 2] e negli altri venerdi' [Tab. I, colonna 4]. Il secondo toglie l'effetto del venerdi'.
- **Un evento, una giornata**: il t per evento e' gia' la significativita' per giornata.
- **Lo Sharpe su 12 eventi l'anno** non si confronta con quello di una strategia che opera ogni
  giorno: va scritto accanto alla convenzione di annualizzazione.
- **La forza per evento dice quanti eventi servono**: con media su deviazione d, per t = 2 servono
  (2 / d)^2 eventi, per una potenza dell'80% a 5% bilaterale (2,8 / d)^2.
- **L'asimmetria** (solo le notti positive tornano indietro) e la differenza trimestrali/mensili sono
  varianti: si dichiarano nella scheda prima di provarle.

## Cosa Si Puo' Rifare Su ES, Con ATAS

E' l'unico paper del filone costruito su dati intraday dell'ES. **La parte sul prezzo si rifa' con
le sole barre M1 e il calendario delle scadenze.** La parte sul meccanismo no: servono le posizioni
per categoria del CBOE, e gli indicatori delle opzioni di ATAS non hanno una storia usabile
([`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md)). Si prova se il prezzo fa
ancora la tenda, non perche' la fa.

### Il calendario

- **Le date**: il terzo venerdi' di ogni mese. Trimestrali (marzo, giugno, settembre, dicembre) e
  mensili si tengono separate. Se il terzo venerdi' e' festivo si scade il giovedi' prima: per esempio
  il 18 aprile 2025 (Venerdi' Santo) e il 19 giugno 2026 (Juneteenth). Le date vanno controllate sul
  calendario ufficiale del CBOE prima di usarle.
- **Le ore** sono di New York: giovedi' 16:00, venerdi' 9:30, 12:00 e 16:00. Si convertono con
  l'ora legale americana, che non coincide con quella europea per alcune settimane l'anno; il fuso
  delle barre di ATAS va controllato.
- **La notte attraversa la pausa del CME** (17:00-18:00 ET) e il cambio di giornata di borsa: va
  verificato come il motore tratta una posizione tenuta da una giornata alla successiva, o si
  misurano le gambe separate.
- **Il contratto**: nei venerdi' trimestrali l'ES in scadenza smette alle 9:30. La serie del
  contratto principale, rettificata all'indietro, e' gia' passata al contratto successivo la settimana
  prima: va bene cosi'.

### Le misure, prima della strategia

1. rendimento ES giovedi' 16:00 -> venerdi' 9:30 (la notte);
2. venerdi' 9:30 -> 12:00 e 9:30 -> 16:00 (il ritorno, con le due uscite del paper dichiarate come
   due varianti);
3. la strategia della Tab. V con le convenzioni del framework (esecuzione all'apertura della barra
   dopo la decisione, commissioni, 1 tick di slittamento).

### Il nullo

- **Le stesse ore negli altri giovedi'-venerdi'**, senza scadenza: tre o quattro al mese. Toglie
  l'effetto del venerdi'.
- **Le stesse ore in tutti gli altri giorni**, come la Tab. V.
- **Per permutazione**: si estraggono tante volte N giornate non di scadenza (N = gli eventi veri) e
  si vede dove cade la media vera nella distribuzione delle medie estratte.
- Come sempre, prima la prova sulla passeggiata casuale: non deve passare niente.

### Quanti eventi servono, e quanti ne avremo

Dalle forze per evento del paper (media su deviazione, conto nostro):

| misura | d | per t = 2 | per potenza 80% |
|---|---|---|---|
| notte ES, tutte le scadenze [Tab. II] | 0,24 | 72 eventi, 6 anni | 141 eventi, 12 anni |
| strategia ES [Tab. V] | 0,26 | 58 eventi, 5 anni | 114 eventi, 10 anni |
| notte, solo trimestrali (SOQ) [Tab. III] | 0,50 | 16 eventi, 4 anni | 31 eventi, 8 anni |

Quanta storia M1 dell'ES dia ATAS e' *[da misurare]* (`ChartHistoryDepth`,
[`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md)); ci si aspettano mesi, forse qualche anno.
**Con un anno, 12 eventi, il t atteso anche con l'effetto intatto e' 0,9.** Quindi:

- con la storia di ATAS **la prova e' descrittiva, non conferma niente**: puo' solo dire se l'effetto
  e' molto piu' grande del paper, o mostrare la forma sulle poche scadenze che ci sono;
- **le trimestrali** sono l'unico sottoinsieme dove pochi anni potrebbero bastare, e solo se l'effetto
  su ES e' come quello della SOQ (non e' detto: la Tab. III non ha una versione ES);
- **da misurare**: se ATAS da' piu' storia con barre da 30 o 60 minuti. Alla misura servono quattro
  prezzi per evento, non il minuto;
- **gli eventi crescono di 12 l'anno**: un archivio tenuto aggiornato e' l'unico modo di arrivare a
  numeri utili.

### E su NQ

Il paper trova la stessa tenda su NQ: notte +14 bps, ritorno simile entro mezzogiorno [BTW Fig. 6],
senza t e senza strategia. NQ e' il nostro strumento e sta gia' in archivio: la stessa prova si fa
anche li', con lo stesso calendario. ES e NQ sugli stessi eventi non sono due campioni indipendenti:
le scadenze sono le stesse e i due indici si muovono insieme, quindi non raddoppiano gli eventi.

### Cosa lo ucciderebbe

- nella nostra storia, la notte prima della scadenza **non diversa** dalle stesse ore negli altri
  venerdi';
- la notte positiva senza ritorno dopo le 9:30;
- un effetto che si spegne man mano che le opzioni a.m. perdono peso rispetto alle 0DTE p.m.: e' cio'
  che predicono entrambe le spiegazioni del paper, perche' tutte e due vivono del regolamento a.m.
