---
chiave: pan-2006-information-in-option-volume-for-future-stock-prices
sigla: PP
titolo: "The Information in Option Volume for Future Stock Prices"
autori: [Jun Pan, Allen M. Poteshman]
versione_letta: "NBER Working Paper 10925, novembre 2004 (titolo: The Information of Option Volume for Future Stock Prices); non la versione pubblicata"
fonte: "Review of Financial Studies 19(3), 871-908 (2006); NBER w10925"
identificativo: "10.3386/w10925"
url: https://nber.org/system/files/working_papers/w10925/w10925.pdf
tipo: rivista
revisione_fra_pari: si
pagine: 37
pdf: pan-2006-information-in-option-volume-for-future-stock-prices.pdf
pdf_sha256: a58fe3a07cd58aed20aa63381ef225204f27a7bd49f7769ca1c3521bf554e5a0
filone: flusso-opzioni
ruolo: idea
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati CBOE per tipo di volume e classe d'investitore (non pubblici, forniti dal CBOE), Berkeley Option Database, CRSP, TAQ, PIN dal sito di Soeren Hvidkjaer"
---

# Pan, Poteshman 2006: L'Analisi

*The Information in Option Volume for Future Stock Prices*, Jun Pan (MIT Sloan, NBER) e Allen M.
Poteshman (Illinois Urbana-Champaign). Pubblicato nella Review of Financial Studies 19(3), 2006.
**Revisionato** nella versione pubblicata; **letta invece la versione NBER** del novembre 2004 (w10925,
37 pagine, numerate da 2 a 36), che il frontespizio intitola *The Information **of** Option Volume*. Le
differenze con la versione pubblicata non le abbiamo viste. Letto per intero il 9 novembre 2026, dal
frontespizio alla bibliografia.

Il testo estratto con `pdftotext -layout` mescola le righe delle tabelle 1, 2, 5, 6 e 9 e perde tutte le
lettere greche delle formule. Le tabelle le abbiamo rilette con `pdftotext -table` e `-raw`, che le
danno riga per riga in modo coerente (sul PC non c'e' `pdftoppm`, quindi il PDF non si e' potuto
guardare come immagine). Citazioni `[PP §n]`, `[PP Tab. n]`, `[PP nota n]`.

**Per il riquadro "Chi ha comprato delta oggi" in due righe**: sui titoli il volume d'acquisto in
apertura dei clienti porta informazione sul prezzo dei giorni dopo, molto significativa; **sugli indici
nessun gruppo porta informazione** sul giorno dopo, e dove il flusso predice qualcosa (OEX, NDX) lo fa
**al contrario**. Su SPX, l'indice di X-Ray, nessun gruppo predice niente. I conti proprietari (firm) non
predicono niente nemmeno sui titoli.

## I Termini

- **Volume per tipo** [§3.1]: il CBOE divide il volume giornaliero di ogni opzione in quattro tipi.
  **Open-buy**: chi compra per aprire una posizione nuova. **Open-sell**: chi vende per aprirne una nuova
  (scrive l'opzione). **Close-buy**: chi ricompra per chiudere una posizione corta. **Close-sell**: chi
  rivende per chiudere una posizione lunga. Non sono dati pubblici: li ha il CBOE.
- **Classi d'investitore** [§3.1, nota 15]: la Options Clearing Corporation (OCC, la cassa di
  compensazione) assegna a ogni scambio un **codice d'origine**: cliente pubblico, conto proprietario
  dell'intermediario (*firm proprietary*), market maker. Il volume dei market maker **non c'e'** nei dati.
  Un analista del CBOE ha diviso i clienti pubblici in clienti dei **broker a sconto** (*discount*, come
  E-Trade), dei **broker a servizio completo** (*full service*, come Merrill Lynch, dove stanno anche gli
  hedge fund) e **altri**. Quattro classi: prop, discount, full service, other.
- **Put-call ratio open-buy** [§2.2.1, eq. 2]: per un titolo e un giorno, X = P / (P + C), con P e C i
  contratti put e call comprati in apertura da chi non e' market maker. Sta fra 0 (solo call) e 1 (solo
  put). Chi ha una notizia buona compra call e lo abbassa; una cattiva, compra put e lo alza. Se c'e'
  informazione, il coefficiente sul rendimento futuro e' **negativo**.
- **Coefficiente in punti base** [nota 19]: i rendimenti sono in punti base (centesimi di punto
  percentuale), quindi il coefficiente si legge come il cambio del rendimento del giorno dopo quando il
  volume open-buy passa da tutto call a tutto put.
- **Rendimento aggiustato per il rischio** [§2.2.1]: il rendimento del titolo meno la parte spiegata da
  quattro fattori (mercato, dimensione, valore, momento): resta la parte propria del titolo.
- **Lee-Ready** [§3.4]: la regola per indovinare se uno scambio l'ha iniziato chi compra o chi vende
  dal prezzo rispetto al denaro-lettera; e' il segnale **pubblico**, che non distingue apertura e
  chiusura.
- **Fama-MacBeth** [nota 19]: una regressione per ogni giorno sui titoli di quel giorno, poi la media dei
  coefficienti; l'errore standard viene dalla dispersione delle stime giornaliere, quindi la
  significativita' e' **per giornata**.
- **PIN** [§2.2.3]: probabilita' che uno scambio sul titolo sia di chi e' informato (Easley e altri).
- **Equilibrio di pooling / separante** [§2.1]: nel modello di Easley, O'Hara e Srinivas (1998) chi e'
  informato scambia anche le opzioni (pooling) o solo il titolo (separante).
- **Contrarian**: un flusso che anticipa il prezzo nel verso opposto a quello in cui scommette.

## La Domanda

Il volume delle opzioni contiene informazione sul prezzo futuro del sottostante, cioe' chi e' informato
scambia anche sulle opzioni [§1, §2.2.1]? Se si', l'informazione e' privata o e' un'inefficienza fra i
due mercati [§2.2.2]; cresce dove ci sono piu' informati e con la leva [§2.2.3, §2.2.4]; **quale classe
d'investitore** la porta [eq. 6]; e c'e' anche sugli **indici** [§4.7]?

## Il Metodo

**I dati** [§3.1, §3.2, Tab. 1]: tutte le opzioni quotate al CBOE, **gennaio 1990 - dicembre 2001**,
volume giornaliero diviso in 16 categorie (4 tipi x 4 classi). NDX solo dal 7 febbraio 1994. Unito con
CRSP (rendimenti dei titoli). Ogni giorno si tengono i titoli con almeno **50 contratti** di open-buy:
da 91 titoli in media nel 1990 a 359 nel 2001, **242 in media** [§3.2, §4.1]. Provate anche soglie di 20
e 100 contratti, "robusto" [nota 18].

**La regressione principale** [§2.2.1, eq. 1-2]:

```text
R(i, t+tau) = a + b X(i,t) + e(i, t+tau)        tau = 1, 2, ...
X(i,t)      = P(i,t) / (P(i,t) + C(i,t))         open-buy, non market maker
nullo: b = 0 per ogni tau (equilibrio separante)
```

Le lettere greche spariscono nel testo estratto; le abbiamo chiamate a e b. La struttura (rendimento di
tau giorni dopo su X del giorno t) si legge senza ambiguita'.

**Le varianti** [§2.2.2 - §2.2.4, eq. 3-7]: aggiunta del put-call ratio pubblico Lee-Ready (eq. 3-4,
1990-1996, perche' il Berkeley Option Database finisce nel 1996 [§3.4]); interazioni con ln(size) e PIN
(eq. 5); **un put-call ratio per classe** (eq. 6, almeno 10 contratti di open-buy della classe [§4.4]);
un put-call ratio per fascia di moneyness e di scadenza (eq. 7, almeno 20 contratti [§4.5]); gli altri
tre tipi di volume [§4.6]. Controlli: liquidita' (turnover e spread, 1993-2001 per i dati TAQ) e
rendimento della settimana prima [§4.2, Tab. 4]; rendimenti grezzi; titoli lontani dalle trimestrali
[§4.2].

**Gli indici** [§4.7, Tab. 9]: per SPX, OEX (S&P 100) e NDX, **regressioni in serie storica** del
rendimento dell'indice del **giorno dopo** sul put-call ratio open-buy di **una classe alla volta**
(univariate), errori robusti a eteroschedasticita' e autocorrelazione. Il numero di giornate non e'
riportato.

**Errori standard** [nota 19]: Fama-MacBeth per la sezione trasversale; per i rendimenti a una
settimana, Newey-West con 5 ritardi in piu'. Nessun campione fuori, nessun costo.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Il test principale**: rendimento aggiustato del giorno dopo sul put-call ratio open-buy, coefficiente **-53 punti base, t -32,92**. Quintili: il piu' basso (put-call ratio vicino a 0) +15,7 pb il giorno dopo, il piu' alto (circa 0,8) -26,6 pb; il portafoglio lungo-corto **42 pb al giorno, t 28,55, Sharpe 0,52** (l'unita' dello Sharpe non e' detta; t / Sharpe = 55, cioe' la radice di circa 3.000 sedute, quante ce ne sono nel 1990-2001: e' coerente con uno Sharpe **giornaliero, non annualizzato**, nostro calcolo) | §4.1 |
| 2 | **L'orizzonte**: i coefficienti calano in modo che gli autori dicono esponenziale; i primi cinque giorni sommano oltre l'1%; dopo **tre settimane** sono vicini a zero; **nessuna inversione** | §4.1, Fig. 1 |
| 3 | Non e' la chiusura delle opzioni dopo il mercato (alle 16:10 fino al 23 giugno 1997, poi alle 16:02): prima del 1997 -46 pb (t -22,31), dopo -60 pb (t -20,86); il paper divide "pre- and post-1997", non al 23 giugno, e il -46 (t -22,31) e' lo stesso numero del 1990-1996 della §4.3. Anno per anno, dal 1990 al 2001, risultati "extremely consistent" (i numeri non sono riportati) | §4.2, nota 23 |
| 4 | Liquidita' e inversione di breve: nel 1993-2001 il coefficiente va da -59,31 (t -32,82) senza controlli a -51,23 (t -30,21) con turnover, spread e rendimento dei cinque giorni prima. Rendimenti grezzi: -50 pb (t -28,17), lungo-corto 38,4 pb (t 23,9). Senza i titoli vicini alle trimestrali: "estremamente simile" | §4.2, Tab. 4 |
| 5 | **Pubblico contro privato** (1990-1996): da soli, open-buy -46 pb (t -22,31), Lee-Ready -30 pb (t -13,51); **insieme**, open-buy -44 (t -16,27), Lee-Ready -5 (t -1,68, non significativo). Il segnale pubblico muore prima e **si inverte** in parte ("a certain degree of reversal"): per gli autori non e' chiaro che sia informazione, ed e' "consistent with price pressure" | §4.3, Fig. 2 |
| 6 | Il segnale pubblico spiega solo una parte di quello privato: R quadro medi dal 13% (close-buy) al 45% (open-buy); per classe, 15% prop, 19% discount, 42% full service, 25% other | §3.4, Tab. 3 |
| 7 | **Dove ci sono piu' informati**: con i soli titoli con PIN (111 in media) il coefficiente e' -34,60 (t -22,20); l'interazione con PIN -189,3 (t -5,05); con size e PIN insieme, PIN -112,4 (t -2,14). Dal PIN piu' basso al piu' alto del campione (circa 0,05 - 0,28) la predicibilita' cresce di circa 43 pb. A una settimana (rendimenti da +1 a +5 giorni), con PIN nel modello, gli autori dicono che il put-call ratio da solo "vanishes"; il numero e' 38,10 con t 2,59, cioe' positivo e oltre la soglia di 2 (segno opposto) | §4.4, Tab. 5 |
| 8 | **Chi, sui titoli** (una classe alla volta): full service **-44,26 (t -37,00)**, 336 titoli in media; discount -34,82 (t -20,02), 175 titoli; other -28,94 (t -17,51), 141 titoli; **prop -1,52 (t -0,75)**, 53 titoli. Tutte insieme (27 titoli in media): full service -30,39 (t -3,53), other -24,47 (t -4,35), discount -12,96 (t -1,71), prop 4,72 (t 0,99) | §4.4, Tab. 6 |
| 9 | **I prop non predicono niente**. Gli autori: o non sono informati, o usano le opzioni quotate soprattutto per coprirsi; i dati dicono solo di chi e' informativo il volume open-buy, non chi e' informato. Indizio di copertura: comprano meno call sui titoli che hanno perso nella settimana e meno put su quelli che hanno guadagnato | §4.4, §3.3, Tab. 2 pannello C |
| 10 | Le classi si somigliano poco: correlazione del put-call ratio dei prop con discount 2%, con full service 8%, con other 8%; fra i clienti, full service con discount 24%, con other 23% | §3.3 |
| 11 | **La leva**: oltre il 10% fuori dal denaro -44,67 (t -29,57); 3-10% fuori -21,15; al denaro -11,74; 3-10% dentro -2,71 (t -1,85); oltre il 10% dentro +7,95 (t 3,52). Per scadenza: sotto i 30 giorni -34,83 (t -31,20), oltre i 179 giorni -6,91 (t -3,63) | §4.5, Tab. 7 |
| 12 | **Gli altri tipi di volume**: open-buy -52,6 (t -32,90); open-sell +20,0 (t 12,40, segno giusto per chi scrive put sulla notizia buona, ma piu' debole); close-buy -0,9 (t -0,46, niente); close-sell +27,4 (t 14,60). **L'open-buy e' il piu' informativo** | §4.6, Tab. 8 |
| 13 | **Gli indici, il risultato per noi** (giorno dopo, una classe alla volta). **SPX**: prop -8,5 [-1,13], discount 10,2 [1,08], full service -1,5 [-0,14], other 1,8 [0,24]: **niente**. **OEX**: prop 7,3 [0,90], **discount 43,7 [3,12], full service 64,5 [3,60]**, other -5,6 [-0,46]. **NDX**: prop -3,2 [-0,26], **discount 46,5 [3,11]**, full service 12,1 [0,69], **other 36,0 [3,09]**. Nessun coefficiente negativo significativo: **nessuna informazione**. I quattro significativi sono **positivi**: piu' put comprate in apertura, indice **piu' alto** il giorno dopo, cioe' **contrarian** | §4.7, Tab. 9 |
| 14 | Il testo commenta come contrarian **solo l'NDX** (discount e other); i due coefficienti positivi e significativi dell'**OEX** sono nella tabella ma il testo non li nomina | §4.7, Tab. 9 |
| 15 | **Chi scambia gli indici** (media dei giorni): su SPX i prop fanno il 23,51% delle put e il 34,29% delle call comprate in apertura, i discount il 4,22% e il 4,19%, i full service il 58,24% e il 48,16%. Su NDX prop 22,68% e 33,25%, discount 5,90% e 9,76%. Su OEX i discount pesano di piu': 12,32% e 14,04%, prop 6,04% e 11,01%. Volume open-buy medio al giorno: SPX 17.398 put e 10.254 call, OEX 25.545 e 19.112, NDX 1.757 e 1.119. Sui titoli i prop sono sotto il 10% **in media** (nota 28); nella Tab. 1 l'open-buy put dei prop sui titoli grandi arriva al 14,45% | §3.1, Tab. 1, nota 28 |
| 16 | Sugli indici, a differenza dei titoli, le put si comprano in apertura piu' di quanto si vendano, e il volume delle call e' simile o inferiore a quello delle put | §3.1, Tab. 1 |

**Lettura del risultato 13 nelle unita' del paper**: per i clienti discount di NDX, passare da un giorno
in cui comprano in apertura solo call a uno in cui comprano solo put si associa a +46,5 punti base
sull'NDX del giorno dopo. Il paper non dice quanto varia il put-call ratio degli indici da un giorno
all'altro, quindi non si puo' tradurre in "una deviazione standard vale tanti punti base".

## Il Perche'

**Sui titoli** [§2.1, §4.1, §4.3, §5]: chi ha un'informazione privata sul titolo la porta sulle opzioni,
perche' danno leva (Black 1975; Easley, O'Hara, Srinivas 1998). I prezzi incorporano subito
l'informazione pubblica del flusso, ma solo piano piano quella privata: per questo il segnale open-buy,
che non e' pubblico, predice per settimane senza tornare indietro, e quello pubblico (Lee-Ready) predice
un giorno o due e poi si inverte. Le conferme del meccanismo: piu' forte dove ci sono piu' informati
(PIN), piu' forte con piu' leva (fuori dal denaro, scadenze brevi), nessuna inversione.

**Le alternative scartate**: un'inefficienza fra i due mercati (il segnale pubblico non predice quando
c'e' quello privato, risultato 5); la pressione della copertura dei market maker (ci sarebbe
un'inversione, e i market maker si coprono in giornata; il volume delle opzioni e' meno di 10 pb di
quello dei titoli [nota 20]); la differenza d'orario di chiusura (risultato 3); liquidita' e inversione di
breve (risultato 4); la dimensione al posto del PIN (risultato 7); la liquidita' al posto della leva
(le tre fasce piu' fuori dal denaro hanno volume simile ma informativita' diversa [§4.5]).

**Sugli indici** [§4.7, §5]: chi e' informato ha informazioni sul singolo titolo, non sul mercato, e gli
indici si usano soprattutto per coprirsi. Il segno contrarian su NDX corrisponde all'uso di Wall Street
del put-call ratio degli indici come segnale al contrario (troppe put = mercato troppo pessimista), e gli
autori dicono che trova "qualche" sostegno. Lo stesso segno contrarian, ma **piccolo**, compare sui titoli
nella parte del segnale pubblico che resta dopo aver tolto l'open-buy [§4.3]: gli autori avvertono di
non darne letture forti.

**Cosa lo ucciderebbe, sui titoli** (dedotto dal perche'): un'inversione nei giorni dopo; una
predicibilita' che non cresce con il PIN e con la leva; un segnale pubblico che predice quanto quello
privato. Per noi conta un'altra conseguenza: **se il flusso per classe diventa pubblico mentre si forma**,
la teoria che gli autori usano dice che il prezzo lo incorpora subito, e la predicibilita' dovrebbe
sparire (nostra deduzione da §2.1 e §4.3).

## Cosa Non Regge O Non Si Dice

1. **Versione letta**: la NBER del 2004, non la pubblicata del 2006. Tabelle e numeri potrebbero essere
   cambiati con la revisione; il verificatore e la sintesi devono saperlo.
2. **L'OEX non e' commentato**. Nella tabella 9 discount (43,7, t 3,12) e full service (64,5, t 3,60)
   sono contrarian e significativi su OEX come su NDX, ma il testo parla solo di NDX. La riga della
   tabella l'abbiamo letta con `pdftotext -table` e `-raw`, coerenti fra loro; la versione pubblicata non
   l'abbiamo vista. **La ricerca dice "su SPX e OEX nessun gruppo": per l'OEX non e' esatto**.
3. **Dodici prove sugli indici senza correzione** (3 indici x 4 classi), univariate, una classe alla
   volta, mai insieme come sui titoli. Quattro hanno t sopra 3. Il numero di giornate non e' riportato.
4. **La serie del put-call ratio non e' trattata**: il livello entra cosi' com'e', senza togliere trend o
   stagionalita' su 12 anni (o 8 per NDX) in cui il mercato delle opzioni e' cresciuto molto [§3.2]. Il
   paper non dice se un trend del rapporto e un mercato in salita (1990-2001) possano dare da soli un
   coefficiente positivo.
5. **L'orario degli indici**: le opzioni su SPX, OEX e NDX chiudevano alle 16:15, quindici minuti dopo
   l'indice [nota 22]. Sui titoli gli autori provano che la differenza d'orario non conta (risultato 3);
   **sugli indici non lo provano**. Il volume del giorno t contiene scambi fatti dopo la chiusura
   dell'indice.
6. **Il segnale non era osservabile**: i dati per tipo e classe sono del CBOE, non pubblici [§3.4]. Il
   lungo-corto di 42 pb al giorno non e' una strategia: non ha costi, e chi lo faceva non vedeva il dato.
   Lo dicono loro stessi, nel senso che lo usano per escludere l'inefficienza.
7. **Lo Sharpe di 0,52 non ha unita'**: e' calcolato su rendimenti giornalieri e il paper non dice se e'
   annualizzato. Il rapporto con la t (28,55 / 0,52 = 55, la radice di circa 3.000 sedute) dice che e'
   giornaliero: annualizzato sarebbe circa 8 (nostro calcolo, non del paper).
8. **Il campione e' vecchio** (1990-2001): prima del retail sulle app, delle scadenze settimanali e
   giornaliere (0DTE), e della categoria dei **clienti professionali** e dei **broker-dealer** che oggi
   esistono nei codici d'origine del Cboe e in OptionsDepth. Le classi del paper sono divise per **tipo
   di broker** (sconto, servizio completo), non per tipo di cliente.
9. **Significativita' per giornata, ma nessun nullo nel nostro senso**: Fama-MacBeth e serie storica con
   errori robusti sono per giornata, come chiede il metodo; il confronto e' con zero, non con la stessa
   regola su un mercato rimescolato.
10. **Le classi sono "volume informativo", non "persone informate"** [§4.4]: gli autori lo dicono per i
    prop. Il flusso di una classe e' una media di chi si copre e di chi scommette.
11. **Piccole incoerenze**: il test principale ha t -32,92 nel testo [§4.1] e t -32,90 nella tabella 8 (il
    coefficiente, -53 contro -52,6, e' solo arrotondato); la didascalia della tabella 9 dice "1990-2001" anche
    per l'NDX, che la tabella 1 da' dal 7 febbraio 1994; il testo cita "Panel B of Table 2" per la scomposizione per classe
    che e' invece nella **tabella 3** [§3.4]; il 70% del volume ai full service e' detto del volume totale
    [§3.4] e del volume open-buy [§4.4].
12. **Conflitti**: nessuno dichiarato. I dati vengono dal CBOE [§3.1]; il frontespizio ringrazia tre persone
    (Levin, Smith, Thaler) per l'aiuto con i dati, senza dire per quale ente.

Nessuna frase del PDF chiede di fare qualcosa: niente istruzioni nel contenuto scaricato.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

**Su NQ, niente di diretto.** La prova del paper vuole il volume delle opzioni per tipo (apertura o
chiusura) e per classe d'investitore. Quello lo da' solo il Cboe; la catena CME di ATAS su NQ non dice
chi ha scambiato (vedi [`x-ray-in-vivo.md`](../../../metodo/x-ray-in-vivo.md), "Da dove vengono"). Il
risultato del paper che tocca NQ e' quello sull'**NDX** (risultato 13): **contesto**, non una regola.

**Sul riquadro "Chi ha comprato delta oggi"** (DEX per gruppo di OptionsDepth su SPX, che su NQ e' solo
contesto, CLAUDE.md decisione 2):

- **Non e' la stessa misura.** Il paper usa il **conteggio dei contratti** comprati **in apertura**,
  diviso fra put e call, **al giorno**, e l'orizzonte e' il giorno dopo. Il DEX e' il **delta scambiato al
  minuto**, netto fra acquisti e vendite, e non sappiamo se OptionsDepth separa apertura e chiusura. Il
  paper mostra che mescolare i tipi diluisce l'informazione (close-buy non dice niente, open-sell e
  close-sell meno dell'open-buy, risultato 12) e che il segnale che non distingue apertura e chiusura
  (Lee-Ready) predice meno e si inverte (risultato 5).
- **Le classi non corrispondono.** `firm` di OptionsDepth corrisponde ai prop del paper; `nonprocust`
  (retail) e `procust` sono **clienti** divisi per tipo di cliente, mentre il paper li divide per tipo
  di broker; `broker` (broker-dealer) nel paper non c'e'. La corrispondenza e' nostra, non del paper.
- **Cosa il riquadro non deve far credere** (dal risultato 13): su **SPX** nessuna classe ha predetto
  l'indice del giorno dopo nel 1990-2001; dove un flusso di clienti ha predetto qualcosa (OEX, NDX), lo
  ha fatto **al contrario**; i **prop**, cioe' `firm`, non predicono niente ne' sui titoli ne' sugli indici,
  e il paper li legge come copertura. Il riquadro puo' **descrivere** chi ha comprato delta, non dire chi
  ha ragione.
- **La normalizzazione**: il paper usa un rapporto fra 0 e 1, che secondo gli autori controlla le
  differenze di volume fra titoli e nel tempo [nota 10]. Per il riquadro l'equivalente sarebbe una quota
  (per esempio il delta comprato su quello scambiato del gruppo), o il confronto con lo storico alla
  stessa ora: e' un'idea nostra, non del paper.
- **Quanto serve per provarlo** (nostro calcolo, approssimato): l'effetto contrarian piu' forte su NDX ha
  t 3,11 su circa 8 anni, cioe' circa 2.000 sedute (dal 7 febbraio 1994 alla fine del 2001, contate da
  noi). Se la t crescesse con la radice delle sedute, per arrivare a t = 2 con un effetto della stessa
  forza servirebbero circa **800 sedute**, oltre tre anni. Il DEX per gruppo salvato da `XrayLive` copre
  pochi mesi (dal 31 agosto 2026, secondo `x-ray-in-vivo.md`, e solo quando l'indicatore e' aperto): non
  prova niente, ne' in un verso ne' nell'altro. E nessun backtest (CLAUDE.md, decisione 2: X-Ray su NQ e'
  contesto).

## Cosa Ne Prende Il Framework

- **Per il riquadro X-Ray**: una frase di contesto provata, con la fonte. "Su SPX, nel 1990-2001, il
  flusso d'acquisto in apertura di nessun gruppo ha anticipato l'indice del giorno dopo; su OEX e NDX
  quello di alcune classi di clienti (OEX: discount e full service; NDX: discount e other) l'ha anticipato al
  contrario; i conti proprietari non hanno mai anticipato niente"
  `[PP §4.7, Tab. 9; §4.4, Tab. 6]`.
- **La misura di riferimento**: il put-call ratio dell'open-buy, P / (P + C), giornaliero, per classe. Se
  un giorno OptionsDepth o il Cboe danno apertura e chiusura, e' la misura con cui confrontarsi.
- **Le prove di meccanismo da riusare** in qualunque studio sul flusso: segnale privato contro pubblico
  nella stessa regressione, inversione nei giorni dopo, crescita con la leva (scadenza e moneyness).
- **Nessuna regola operativa**: sugli indici il paper non trova informazione, e sui titoli il segnale non
  era osservabile ne' scambiabile.

## La Verifica

**9 novembre 2026**, verifica avversaria sul PDF (NBER w10925, novembre 2004, sha256 ricalcolato e uguale
a quello della scheda). Testo estratto con `pdftotext -layout`; le tabelle 1, 2, 4, 5, 6 e 9 rilette pagina
per pagina con `-table` e `-raw` (pagine del PDF 12, 16, 23, 28, 29, 34), perche' `-layout` le mescola: nella
tabella 1 sposta di una riga le etichette del pannello A (le percentuali di "Discount" sono in realta' dei
full service) e nella tabella 4 mette tutti i controlli nella prima riga. Il PDF non si e' potuto guardare
come immagine (niente `pdftoppm`). Nessuna frase del PDF chiede di fare qualcosa.

**Controllate circa 230 affermazioni**: i 16 risultati con circa 170 numeri, piu' circa 60 fra scheda,
termini, metodo, perche', limiti e parte su NQ, con le loro citazioni `§`, `Tab.` e `nota`. **Confermate
tutte le cifre**; **9 correzioni**, nessuna affermazione tolta.

- **La riga OEX e le righe SPX e NDX della tabella 9 sono giuste**: `-table` e `-raw` danno ognuna con la
  sua etichetta (con `-raw` l'etichetta sta fra il coefficiente e la t della stessa riga), e la riga NDX e'
  quella che il testo commenta (discount e other positivi e significativi). Le righe di indice della
  tabella 1 (volumi e percentuali di SPX, OEX, NDX) sono giuste anche loro.
- I coefficienti rinominati (a, b al posto delle lettere greche) non cambiano niente: la struttura delle
  equazioni 1-7 e' quella del paper.

Le correzioni:

1. Scheda: le pagine sono 37, **numerate da 2 a 36**, non "36 numerate".
2. Risultato 1 e limite 7: lo Sharpe di 0,52, dal rapporto con la t, e' **giornaliero** (circa 8
   annualizzato); calcolo nostro, il paper non lo dice.
3. Risultato 3: la divisione e' "pre- and post-1997", non al 23 giugno 1997; e "ogni anno da' lo stesso
   risultato" era piu' di quanto il paper dica ("extremely consistent", numeri non riportati).
4. Risultato 5: "e' pressione sul prezzo, non informazione" era troppo forte; gli autori dicono "a certain
   degree of reversal" e "consistent with price pressure".
5. Risultato 7: a una settimana il paper dice che il put-call ratio "vanishes", ma il coefficiente e'
   38,10 con t 2,59: lo si dice.
6. Risultato 15: i prop sotto il 10% sui titoli e' una **media** (nota 28); nella tabella 1 arrivano al
   14,45%.
7. Limite 11: fra -53 e -52,6 il coefficiente e' solo arrotondato; cambia la t (-32,92 contro -32,90).
   Aggiunta l'incoerenza della didascalia della tabella 9 ("1990-2001" anche per l'NDX, che parte dal 1994).
   "Panel B of Table 2" per la tabella 3 e il 70% detto di due volumi diversi: confermati.
8. Limite 12: il frontespizio ringrazia tre persone per i dati, ma non dice che sono del CBOE.
9. "Cosa ne prende il framework": "su OEX e NDX quello dei clienti" diventa le classi precise (OEX discount
   e full service, NDX discount e other); le altre classi di clienti non sono significative.

**Cosa resta incerto**: la versione pubblicata (Review of Financial Studies 2006) non l'abbiamo vista,
e quindi nemmeno se la tabella 9 e i numeri sono cambiati con la revisione; volume, pagine e DOI della
rivista nella scheda non stanno nel PDF letto. La correzione della "risposta breve" della ricerca per l'OEX
resta da fare nella sintesi.
