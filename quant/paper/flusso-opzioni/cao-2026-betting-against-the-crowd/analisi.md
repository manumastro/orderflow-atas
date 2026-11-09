---
chiave: cao-2026-betting-against-the-crowd
sigla: CLZZ
titolo: "Betting Against the Crowd: Option Trading and Market Risk Premium"
autori: [Jie Cao, Gang Li, Xintong (Eunice) Zhan, Guofu Zhou]
versione_letta: versione accettata del gennaio 2026, con l'appendice internet, dal sito della rivista
fonte: Journal of Financial and Quantitative Analysis (in uscita)
identificativo: "10.1017/S0022109026102889"
url: https://jfqa.org/wp-content/uploads/2026/03/26295_Bet_Against_the_Crowd.pdf
tipo: rivista
revisione_fra_pari: si
pagine: 72 (articolo con tabelle e figura alle pagine 1-45 del PDF, appendice internet alle 46-72)
pdf: cao-2026-betting-against-the-crowd.pdf
pdf_sha256: f1a5055f3b7f4c0026ea5e3a9a5e164e6241106180744a200f0e9af467af55d5
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati Cboe Open-Close e Nasdaq ISE (a pagamento), CRSP, Compustat, Kenneth French, Global Financial Data"
---

# Cao, Li, Zhan, Zhou 2026: L'Analisi

*Betting Against the Crowd: Option Trading and Market Risk Premium*, Jie Cao (Hong Kong Polytechnic),
Gang Li (Chinese University of Hong Kong), Xintong Zhan (Fudan), Guofu Zhou (Washington University in
St. Louis). Versione accettata dal Journal of Financial and Quantitative Analysis, gennaio 2026, in uscita.
**Revisionato.** Letto per intero il 9 novembre 2026, articolo e appendice internet. Lo usiamo come
**metodo** (come si misura e si normalizza uno sbilancio per gruppo, e cosa succede sugli indici), non come
idea di strategia: lo diciamo in fondo.

**Come e' stato letto.** Il testo e' estratto con `pdftotext -layout`; le tabelle, che nel layout si
mescolano, sono state rilette pagina per pagina con `pdftotext -raw`, che tiene ogni riga nel suo ordine.
Le pagine del PDF **non** si sono potute vedere come immagine (`pdftoppm` manca su questa macchina): ogni
numero qui sotto viene dall'estrazione `-raw` della pagina della tabella, ricontrollata con quella
`-layout`. In verifica ogni tabella e' stata riletta anche con `pdftotext -table`, che tiene le colonne
(vedi La Verifica, in fondo). Citazioni `[CLZZ §n]`, `[CLZZ Tab. n]` (articolo), `[CLZZ Tab. An]` (appendice internet).

**In parole semplici.** Gli autori sommano, su tutti i titoli americani, quanto i clienti comprano call
per aprire posizioni rispetto a quanto ne vendono. Quando i clienti, e soprattutto quelli che mandano
ordini piccoli, comprano molte call, **il mercato nelle settimane e nei mesi dopo va peggio**: e' un
segnale al contrario (contrarian), che gli autori leggono come entusiasmo eccessivo (sentiment). Lo
stesso conto fatto **sulle opzioni di SPX e di NDX non predice quasi niente**: nessun risultato
significativo al giorno o alla settimana, e su NDX nemmeno al mese. I professionisti e i market maker non
predicono quasi niente (una riga dei market maker, al giorno e al denaro, fa eccezione: risultato 10). Tutto e' a orizzonte di giorni, settimane e mesi, 2005-2020: niente intraday, niente
0DTE.

## I Termini

- **Order imbalance (IB)**, sbilancio degli ordini: acquisti meno vendite divisi per acquisti piu'
  vendite, quindi un numero fra -1 e +1. Qui si contano **contratti**, non delta: una call molto fuori
  dal denaro pesa come una al denaro.
- **Open buy, open sell**: contratti comprati o venduti **per aprire** una posizione nuova. Il paper usa
  solo questi; le chiusure le prova a parte [§4.1].
- **CIB, PIB**: lo sbilancio delle call (C) e delle put (P) di un singolo titolo. **ACIB, APIB**: la media
  su tutti i titoli, pesata per capitalizzazione (A sta per aggregato). **ICIB, IPIB**: lo stesso sulle
  opzioni su indici (I), dal lavoro di Chordia e altri (2021).
- **I gruppi di Cboe** (*origin code*, codice d'origine): **customer** (cliente pubblico), **professional
  customer** (cliente che il broker classifica professionale), **market maker**; nel paper "market maker"
  e' la somma dei conti market maker, **firm** (conto proprietario di un membro) e **broker-dealer** [§4.1,
  nota 14]. La **regola dei 390 ordini**: e' "professional" chi manda in media piu' di 390 ordini al
  giorno in un mese; quindi "customer" **non vuol dire retail**, comprende anche professionisti che
  mandano meno ordini [§2, nota 8]. Per questo gli autori parlano di "piccoli trader (retail compreso)".
- **Taglia dell'ordine**: small sotto i 100 contratti, medium 100-199, large oltre 199 [§2, §4.1]. E' la
  taglia del singolo ordine, non del conto.
- **MKTRF** (premio per il rischio di mercato): il rendimento logaritmico del portafoglio di tutto il
  mercato americano pesato per capitalizzazione, meno il tasso senza rischio, dal sito di Kenneth French
  [§2]. **E' la variabile da predire in tutto il paper**, anche nella prova sulle opzioni di SPX e NDX.
- **Regressione predittiva**: il rendimento del periodo dopo (giorno, settimana, mese) spiegato dal
  predittore di oggi. Qui i predittori sono **standardizzati** (media zero, deviazione standard uno; il
  paper non dice su quale finestra, presumibilmente il campione intero): il coefficiente dice di quanti punti percentuali cambia il rendimento medio per una
  deviazione standard in piu' del predittore.
- **Newey-West**: una correzione degli errori standard per l'autocorrelazione e per la varianza che cambia
  nel tempo; qui con K-1 ritardi per un orizzonte di K periodi [§3.1, IA.1]. Quindi all'orizzonte di un
  periodo (D=1, W=1, M=1) i ritardi sono zero e la correzione e' solo per la varianza che cambia.
- **R quadro fuori campione** (R2 OOS) e test di **Clark e West**: quanto la previsione, stimata solo sul
  passato, batte la media storica; positivo vuol dire meglio [§5.1].
- **CER** (*certainty equivalent return*): il rendimento sicuro che un investitore media-varianza
  giudicherebbe equivalente al portafoglio; il **guadagno di CER** e' la differenza con il portafoglio che
  usa solo la media storica [§5.1, eq. 13-14].
- **Sezione trasversale e serie storica**: la prima confronta i titoli fra loro nello stesso momento (i
  titoli con CIB alto contro quelli con CIB basso); la seconda guarda un solo numero, il mercato, nel
  tempo.
- **ITI** (*informed-trading intensity*): la differenza in valore assoluto fra il rendimento assoluto del
  quintile con CIB piu' alto e quello con CIB piu' basso [§3.2, eq. 5]. Alta vuol dire che le notizie dei
  singoli titoli non si compensano.
- **Sentiment**: l'umore degli investitori, ottimismo o pessimismo non giustificato dai fondamentali.
  Indici usati per confronto: AAII (sondaggio dei piccoli investitori), SEP (emissioni di prodotti
  strutturati retail, Henderson, Pearson e Wang 2023), BW (Baker e Wurgler) e altri [§3.1].
- **PLS** (*partial least squares*): un modo di riassumere molti predittori in uno solo [§5.1].

## La Domanda

Lo sbilancio degli ordini sulle opzioni dei titoli, sommato su tutto il mercato, predice il rendimento del
mercato nel tempo? In che verso, se nella sezione trasversale lo stesso sbilancio predice i titoli **nello
stesso verso** (chi compra call e' informato)? E se predice al contrario, e' sentiment, e di chi?

## Il Metodo

**I dati** [§2]: Cboe Open-Close sulle opzioni dei titoli, 2005-2020, per gruppo, per acquisto o vendita,
per apertura o chiusura, e per taglia (la taglia solo per customer e professional). In media 2.053 titoli
con opzioni scambiate per l'ACIB settimanale e 1.718 per l'APIB [§2]. Prezzi e azioni in circolazione da
CRSP. Per la prova di robustezza, gli stessi dati dal Nasdaq ISE [IA.4, Tab. A8].

**La misura** [§2, eq. 1-3], per il titolo i nel periodo t (giorno, settimana o mese), sommando su tutti i
contratti s del titolo, tutte le scadenze e tutti i prezzi d'esercizio, **solo clienti** (customer, senza i
professional):

```text
CIB(i,t)  = ( somma OpenBuy call  - somma OpenSell call ) / ( somma OpenBuy call  + somma OpenSell call )   (1)
PIB(i,t)  = lo stesso con le put                                                                            (2)
ACIB(t)   = somma su i di w(i,t) x CIB(i,t),   w = peso per capitalizzazione del sottostante                (3)
```

Poi, per le regressioni, ACIB si standardizza (media zero, deviazione uno; la finestra non e' detta)
[§3.1]. Gli autori scelgono la forma fra
-1 e +1 perche' rende la serie stazionaria [§2].

**Le regressioni** [§3.1, eq. 4; IA.1, eq. 1 e 5]: il rendimento medio di MKTRF nei K periodi dopo, su ACIB
di oggi, da solo o con il solo APIB accanto (appendice, Tab. A3), o con altri predittori (Tab. 2, Tab.
A4): **sette** al giorno e alla settimana (APIB, ICIB, IPIB, VRP, IVS, BKM Skew, AAII), **diciassette** al
mese (in piu' PNBO, gli altri sei indici di sentiment, JZZ Skew, HLW Disp e la prima componente principale
dei 22 predittori di Goyal e Welch). Osservazioni: 4.027 giornate, 841 settimane, 191 mesi [Tab. A3]. Errori
di Newey-West. Gli orizzonti: 1, 3, 6 giorni; 1, 2, 3 settimane; 1, 2, 3 mesi [Tab. A3].

**Il fuori campione** [§5.1, eq. 8-14]: regressione ricorsiva, valutazione **2010-2020**, confronto con la
media storica, test di Clark e West. Un investitore media-varianza con avversione al rischio 3 mette nel
mercato il peso r^/(3 sigma^2), con la varianza stimata sull'anno precedente (settimanale) o sui cinque anni
(mensile), ribilanciando ogni settimana o mese. I PLS dei predittori esistenti si stimano dal 1960.

**Le scomposizioni** [§4.1, Tab. 4; IA.4, Tab. A7-A9]: per taglia, per gruppo (professional e market
maker solo dal 2009, 631 settimane), aperture contro chiusure, acquisti contro vendite (rapporto opzioni su
azioni, O/S), scadenza (sotto 15 giorni, 15-60, oltre 60) e denaro (fuori dal denaro, al denaro, dentro;
soglie 0,9 e 1,1 sul rapporto strike/prezzo; per le call la definizione scritta nelle didascalie delle Tab.
A7-A9 e' incoerente, vedi sotto). Nelle Tab. A7 e A8 **ogni colonna e' una regressione con ACIB e APIB
insieme**, non ACIB da solo.

**I regimi** [§3.2, §4.2, Tab. 3C, Tab. 5, Tab. A10]: il campione si divide in due alla **mediana sul
campione intero** di ITI, AAII, SEP e quota di societa' con trimestrali nella settimana.

**Gli indici** [§5.2, Tab. 7, Tab. A11]: lo sbilancio delle **call** su RUT, DJX, **NDX** e **SPX**, per
taglia, contro **MKTRF** della settimana dopo (Tab. 7), e senza taglia contro MKTRF a 1 e 3 giorni e a 1 e 2
mesi (Tab. A11). Le put sugli indici non sono provate come predittore principale, entrano solo come
controllo (IPIB).

**Altro** [IA.7, IA.8]: quattordici mercati esteri (Tab. A12-A13); la volatilita' del mercato (Tab. A14).

## I Risultati

Unita': i coefficienti sono **punti percentuali del rendimento medio per periodo** di MKTRF per una
deviazione standard del predittore; fra parentesi la t di Newey-West.

| | risultato | dove |
|---|---|---|
| 1 | **ACIB e' quasi sempre negativo**: media settimanale -0,14 (minimo -0,49, massimo 0,20, deviazione standard 0,13), autocorrelazione 0,72; giornaliero media -0,16, autocorrelazione 0,78. I clienti sono **venditori netti** di call e di put sui titoli | Tab. 1A, Tab. A1, §2 |
| 2 | **Chi scambia cosa** (2009-2020, quota del volume): sulle opzioni su indici (RUT, DJX, NDX, SPX insieme) customer 26,36% delle call e 27,25% delle put, market maker 71,25% e 71,06%, professional 1,55% e 1,18%, firm 0,83% e 0,50%. Fra i customer sugli indici gli ordini large (12,12% delle call, 14,43% delle put) pesano piu' dei small (11,07%, 9,90%). Sui titoli i customer small fanno il 23,70% delle call | Tab. A2, §2 |
| 3 | **Il finding, da solo**: ACIB -> MKTRF della settimana dopo -0,240 (t -2,69); del giorno dopo -0,070 (t -2,94); del mese dopo -1,024 (t -2,91). Resta a tre mesi: -0,658 (t -2,94). APIB da solo non predice mai (giorno 0,002, t 0,06; settimana -0,157, t -1,43; mese -0,255, t -0,69). **Con il solo APIB accanto**, a una settimana ACIB scende a -0,211 (t -1,67), **non significativo**; a 2 e 3 settimane -0,187 (t -1,95) e -0,201 (t -2,37); al giorno -0,075 (t -2,70); al mese -1,013 (t -2,78) | Tab. A3 |
| 4 | **Con i controlli** (7 a settimana e al giorno, 17 al mese): settimana -0,329 (t -2,37), R quadro corretto 4,77%; mese -1,358 (t -2,81), R quadro corretto 27,07% su 191 mesi; giorno -0,107 (t -3,11). E' il -0,329 dell'introduzione e del §3.1 (l'abstract non da' numeri), non quello della regressione semplice | Tab. 2, Tab. A4, §1, §3.1 |
| 5 | Nella stessa regressione con i controlli, **lo sbilancio delle put su indici (IPIB) ha segno positivo**: settimana 0,211 (t 2,47), tre giorni 0,029 (t 2,47); quello delle call su indici (ICIB) no: settimana -0,068 (t -0,78) | Tab. 2, Tab. A4 |
| 6 | **Sezione trasversale**: i titoli con CIB alto battono quelli con CIB basso, 0,176% a settimana (t 6,34). ACIB predice al contrario **tutti** e cinque i quintili (da -0,379 a -0,442, t fra -2,84 e -3,28) ma **non** la differenza fra il quinto e il primo (-0,019, t -0,56) | §3.2, Tab. 3A-B |
| 7 | ITI bassa: ACIB -0,375 (t -3,15); ITI alta: -0,133 (t -1,07). 420 settimane per regime | §3.2, Tab. 3C |
| 8 | **Per taglia** (settimana): small -0,233 (t -2,61), medium -0,148 (t -1,54), large 0,038 (t 0,44). Al giorno (con APIB accanto): small -0,086 (t -3,01), large -0,009 (t -0,39). Al mese anche il medium e' significativo, -1,125 (t -2,61); large 0,333 (t 1,51) | §4.1, Tab. 4A, Tab. A7A |
| 9 | **Per gruppo** (settimana): customer -0,240 (t -2,69, 2005-2020, 841 settimane); professional 0,086 (t 0,82) e market maker (con firm e broker-dealer) 0,046 (t 0,45), entrambi 2009-2020, 631 settimane | §4.1, Tab. 4B |
| 10 | Per gruppo e denaro (2009-2020, 3.020 giornate, 143 mesi), al giorno: ACIB dei market maker **al denaro +0,083 (t 2,60)**, dentro il denaro 0,053 (t 1,72); anche APIB dei market maker dentro il denaro, a tre giorni, 0,029 (t 2,00). Professional: ACIB mai oltre t 1,55 in valore assoluto, APIB fino a -1,67 (dentro il denaro, al mese) | Tab. A8A-B |
| 11 | **Aperture e chiusure**: solo chiusure 0,130 (t 1,13); aperture piu' chiusure -0,124 (t -1,33). Sul rapporto O/S: acquisti di call -0,138 (t -1,54), vendite di call -0,007 (t -0,08), la differenza -0,161 (t -1,82): **e' il comprare che porta il segnale**, dicono gli autori, ma a settimana nessuna delle tre e' significativa al 5%. Al giorno (Tab. A9) acquisti di call fuori dal denaro -0,080 (t -2,40) e vendite +0,063 (t 2,00): anche la vendita fuori dal denaro ha una t oltre 2, di segno opposto | §4.1, Tab. 4C-D, Tab. A9 |
| 12 | **Per scadenza** (al giorno, con APIB accanto): sotto i 15 giorni -0,041 (t -1,89), 15-60 giorni -0,068 (t -2,89), oltre 60 -0,063 (t -2,70). Al mese: -0,810 (t -2,13), -1,127 (t -3,55), -0,952 (t -3,17). **Le scadenze brevi sono le piu' deboli** | Tab. A7B |
| 13 | **Per denaro** (al giorno, con APIB accanto): al denaro -0,076 (t -2,76), dentro -0,053 (t -2,00), fuori -0,030 (t -1,04). Al mese al denaro -1,567 (t -3,29) | Tab. A7C, IA.4 |
| 14 | **Regimi** (settimana): AAII alto -0,294 (t -2,32), basso -0,175 (t -1,43); SEP alto -0,256 (t -2,00), basso -0,210 (t -1,86); molte trimestrali -0,337 (t -2,33), poche -0,170 (t -1,54). Al giorno con SEP alto -0,142 (t -4,02), basso -0,043 (t -1,02); con molte trimestrali -0,150 (t -3,43), poche -0,003 (t -0,07) | §4.2, Tab. 5, Tab. A10 |
| 15 | **Fuori campione 2010-2020**: R2 OOS 0,756% a settimana (Clark-West 1,926) e 6,114% al mese (2,835). Sharpe annuo della strategia 0,847 a settimana contro 0,541 della media storica; 1,360 al mese contro 0,725. Guadagno di CER annuo 2,978% e 10,416%. Con tutti i predittori esistenti (PLS) al mese l'R2 OOS passa da 7,864% a 8,957% aggiungendo ACIB | §5.1, Tab. 6 |
| 16 | **Le opzioni su indici, settimana dopo, per taglia** (contro MKTRF): **SPX** small -0,072 (t -0,86), medium 0,076 (t 0,82), large 0,018 (t 0,19); **NDX** small -0,016 (t -0,18), medium -0,072 (t -0,67), large -0,070 (t -0,59); RUT e DJX nessuno significativo (il piu' forte DJX small -0,135, t -1,61). **Nessuno dei dodici e' significativo**. Il paper non dice quali gruppi entrano nel CIB degli indici (l'eq. 1 e' dei soli customer, e la taglia esiste solo per customer e professional) | §5.2, Tab. 7A |
| 17 | Con ACIB nella stessa regressione, ACIB resta -0,302 (t -2,46), -0,341 (t -2,77), -0,326 (t -2,11) per small, medium, large; il coefficiente di SPX diventa positivo e non significativo (0,102, 0,183, 0,140; t da 0,98 a 1,50); quello di NDX resta nullo (0,044, t 0,49; -0,064, t -0,58; 0,039, t 0,29) | Tab. 7B |
| 18 | **Indici senza taglia, a giorni e mesi**: SPX 1 giorno 0,022 (t 0,90), 3 giorni -0,014 (t -0,92), 1 mese -0,617 (t -1,83), **2 mesi -0,583 (t -2,74)**; NDX 1 giorno -0,008 (t -0,28), 3 giorni -0,007 (t -0,44), 1 mese -0,340 (t -1,18), 2 mesi -0,464 (t -1,64); DJX 1 mese -0,795 (t -2,48), 2 mesi -0,772 (t -3,41) | Tab. A11 |
| 19 | Correlazioni: ACIB con AAII 0,37 e con SEP 0,33; ICIB con AAII 0,15 e con SEP 0,02 (solo nel testo). Ma **ACIB e ICIB sono correlati 0,60** al mese | §5.2, Tab. 1B |
| 20 | Con i dati ISE invece di Cboe, ACIB correlato 0,78 con quello Cboe; al mese al denaro -1,789 (t -3,46) | IA.4, Tab. A8C |
| 21 | **All'estero**: tutti i coefficienti di ACIB negativi su 14 mercati, con il rendimento locale e quello americano come controlli; a una settimana non significativi su Australia (t -1,55), Canada (t -1,23), Italia (t -1,72), Germania (t -1,96, al limite); a tre giorni Nuova Zelanda (t -1,73) e Spagna (t -1,51); al mese Italia (t -1,59) e Svizzera (t -1,21) | IA.7, Tab. A12-A13 |
| 22 | **Le put predicono la volatilita', non il prezzo**: APIB -> volatilita' del mercato della settimana dopo 1,455 (t 3,50), con VIX e HAR come controlli; ACIB -0,621 (t -1,49) | IA.8, Tab. A14 |

## Il Perche'

**Il meccanismo degli autori** [§1, §3.2, §4]: lo sbilancio delle opzioni ha **due componenti**.
L'informazione sui singoli titoli (chi sa qualcosa compra call su quel titolo) e il sentiment (chi e'
ottimista compra call su tutto). Nella sezione trasversale il lungo-corto fra quintili toglie il sentiment,
che tocca tutti i titoli, e resta l'informazione: segno positivo (risultato 6). Sommando su tutti i titoli,
le notizie buone e cattive si compensano e resta il sentiment: segno negativo (risultati 3 e 6). Il
sentiment passa dalle **call** perche' hanno un pagamento "a lotteria" che piace ai piccoli [§3.1]; le put
servono alla copertura e alla volatilita' (risultato 22). Gli indici non mostrano l'effetto perche' li
scambiano soprattutto istituzionali [§1, §5.2], e perche' sono cosi' visibili che gli arbitraggisti
correggono subito gli errori di prezzo del rumore [§1, nota 2].

**Le alternative che scartano**: (a) un effetto meccanico dei market maker: il loro ACIB non predice
(risultato 9) [§4.1]; (b) l'informazione privata: se fosse quella, il segno sul mercato sarebbe positivo e
l'effetto non si concentrerebbe dove la ITI e' bassa (risultato 7) [§3.2]; (c) un dato di una sola borsa:
con ISE il risultato regge (risultato 20) [IA.4].

**Cosa lo ucciderebbe**, dedotto dal perche': uno sbilancio dei large o dei professional che predice come
quello dei small; ACIB che predice anche il lungo-corto fra quintili; nessuna differenza fra i regimi di
sentiment alto e basso (gli autori non la provano formalmente: vedi sotto); un fuori campione dopo il 2020
che sparisce.

## Cosa Non Regge O Non Si Dice

1. **La prova su SPX e NDX non e' sul prezzo di SPX e NDX.** La variabile da predire e' sempre MKTRF, il
   mercato americano intero [§5.2, Tab. 7, Tab. A11]. Per NDX, che e' il piu' vicino a NQ, non c'e' una
   regressione sul rendimento dell'NDX.
2. **Il testo dice piu' della tabella sugli indici.** "Small-order CIBs ... are negatively associated with
   future stock market excess returns" [§5.2, IA.6]: in Tab. 7A nessuna t supera 1,61 in valore assoluto, e
   su SPX e NDX i small hanno t -0,86 e -0,18. La Tab. A11, citata per la stessa frase, non divide nemmeno
   per taglia.
3. **"Il miglior predittore settimanale fuori campione"** [§1]: in Tab. A5 l'IVS a una settimana ha R2 OOS
   1,183% contro lo 0,756% di ACIB (con Clark-West 1,119 contro 1,926, cioe' meno significativo), e il PLS
   dei predittori d'opzione 1,198% [Tab. 6]. L'affermazione regge solo se si guarda la significativita'.
4. **I gruppi sono confrontati su campioni diversi.** Customer 2005-2020 (841 settimane), professional e
   market maker 2009-2020 (631) [Tab. 4]. Il customer sul 2009-2020 non e' mostrato.
5. **"I market maker non predicono"** [§4.1, IA.4] non e' vero per tutte le loro righe: al giorno, al
   denaro, +0,083 con t 2,60, e APIB dentro il denaro a tre giorni 0,029 con t 2,00 [Tab. A8A]. Nostra
   lettura, non del paper: e' coerente con il market maker controparte dei clienti (segno opposto); il
   paper spiega invece il nulla dei market maker con il flusso dei professional che li "contamina" [§4.1].
   **Firm e broker-dealer non sono mai separati come predittori** [§4.1, nota 14]; la firm compare da sola
   solo nelle quote di volume della Tab. A2, il broker-dealer mai: per i due gruppi di OptionsDepth il paper
   non dice niente sul prezzo.
6. **La ITI potrebbe guardare avanti.** La didascalia della Tab. 3 la definisce dal rendimento dei quintili
   "per W=1 nel pannello A", cioe' dai rendimenti della settimana **dopo** l'ordinamento; l'eq. 5 scrive
   R(i,t) con il portafoglio del periodo t e non dice a quale settimana della regressione si attacca la ITI.
   **Riletto sul PDF (eq. 5 con `-raw`, didascalia della Tab. 3): il paper non scioglie il dubbio.** Se la
   ITI della settimana t e' fatta con i rendimenti di t+1, la divisione in regimi usa la stessa settimana che
   si predice; se e' fatta con quelli di t, no. In entrambi i casi la mediana e' del campione intero.
7. **I regimi si tagliano alla mediana del campione intero** (in-sample) e **la differenza fra i due
   regimi non e' provata**: con SEP i coefficienti sono -0,256 e -0,210 [Tab. 5B], vicini; l'affermazione
   "only when ... above the median" [§4.2] e' un confronto di t, non un test.
8. **Le regressioni giornaliere hanno un R quadro che non si spiega.** APIB da solo, con t 0,06, ha R
   quadro corretto 1,59% [Tab. A3]; RUT con t 0,15 ne ha 1,74% [Tab. A11]. Un predittore con t vicino a zero
   non da' l'1,6%: nella regressione giornaliera c'e' qualcosa che non e' dichiarato (forse ritardi del
   rendimento o altri controlli). **Confermato in verifica, con un conto nostro**: con un solo predittore
   l'R quadro e' circa t^2/N; a D=1 ACIB (t -2,94, 4.027 giornate) darebbe circa 0,21%, non l'1,88%. Alla
   settimana (small, t -2,61: 0,81% contro 0,88% della Tab. 4A) e al mese (t -2,91: 4,4% contro 4,73% della
   Tab. A3) il conto torna. Fra le regressioni giornaliere torna solo in Tab. A6 pannello B (rendimento
   equal-weighted: t -3,18, R quadro 0,25%). Le regressioni a un giorno sul rendimento pesato per
   capitalizzazione hanno quasi tutte un R quadro fra 1,5% e 2,2%, qualunque sia la t (Tab. A3, A7, A8, A9,
   A11; fa eccezione NDX in Tab. A11, 0,67%): o un controllo non dichiarato, o un errore di stampa. Il paper
   non lo spiega.
9. **Numeri che non tornano fra tabelle.** La regressione semplice di ACIB pesato per capitalizzazione
   compare due volte: al giorno -0,070 (t -2,94) in Tab. A3 e -0,075 (t -3,36) in Tab. A6; al mese -1,024 (t
   -2,91) e -0,945 (t -2,89); a tre giorni -0,045 (t -2,81) e -0,054 (t -3,58); a due mesi -0,734 (t -2,66)
   e -0,822 (t -3,28); a una e due settimane coincidono (-0,240, t -2,69; -0,216, t -3,21). APIB a una
   settimana, con i controlli, e' 0,185 (t 1,10) in Tab. 2 e 0,158 (t 1,10) in Tab. A4, che altrimenti
   ripete la colonna settimanale della Tab. 2. Manager sentiment al mese e' -0,197 (t -0,63) in Tab. 2; in
   Tab. A4 la cella stampa "(-0,20)" sopra e "-0,63" sotto, cioe' coefficiente e t scambiati (e lo stesso
   forse a due mesi, "(-0,39)" e "-1,25"). In Tab. A2 la riga "Firm" sugli indici (0,83 e 0,50) e' identica
   alla riga "Professional (Small)"; i totali tornano a 100 (99,99 sugli indici), ma la coincidenza va
   segnalata. Il testo da' customer 31% e market maker 65% del volume [§2]: la media semplice delle quattro
   colonne della Tab. A2 da' circa 30,5% e 64,1% (nostro calcolo). La stessa media semplice di call e put
   rida' esattamente i 13,27% e 9,09% di ordini large del testo, quindi la regola e' probabilmente quella, e il
   65% non torna.
10. **Il numero dell'introduzione viene dalla regressione con 7 controlli** (-0,329, settimana; l'abstract
    non da' numeri); quella semplice da' -0,240, e con il solo APIB accanto -0,211 con t -1,67 (punto 17).
    Al mese, 18 predittori su 191 osservazioni con R quadro corretto 27,07% [Tab. 2]: molto adattamento. Le
    varianti provate sono tante (tre frequenze, nove orizzonti, taglie, gruppi, scadenze, denaro, quattro
    regimi, quattro indici, 14 paesi) e **nessuna correzione per test multipli**; le t principali stanno per
    lo piu' fra 2,4 e 3,6.
11. **Un predittore molto persistente** (autocorrelazione 0,72 a settimana, 0,78 al giorno) su orizzonti
    sovrapposti: e' il caso in cui la stima del coefficiente ha la distorsione di Stambaugh, non discussa.
12. **La strategia non ha costi** e non dichiara limiti alla leva: lo Sharpe di 0,847 e' lordo [§5.1].
13. **"Customer" non e' "retail"** [nota 8], e la taglia e' quella dell'ordine, non del conto.
14. **I dati finiscono nel 2020**: prima delle scadenze giornaliere di SPX (gli 0DTE ogni giorno) e con il
    boom del retail solo all'inizio. Le scadenze sotto i 15 giorni sono gia' le piu' deboli (risultato 12).
15. **Il tempo**: la settimana non e' definita (quale giorno la chiude), ne' quando il dato Open-Close di un
    giorno e' disponibile. Con il rendimento dal giorno dopo non c'e' look-ahead evidente, ma non e' detto.
16. **Si contano contratti, non delta**: la misura non e' il DEX del riquadro.
17. **Il risultato settimanale e' meno robusto di come lo si racconta.** Con il solo APIB come controllo,
    ACIB a una settimana ha t -1,67 [Tab. A3, colonna III]. ACIB pesato in modo uguale fra i titoli, sul
    rendimento pesato per capitalizzazione, ha t -1,95 a una settimana, -1,74 a un mese e -1,85 a due mesi
    [Tab. A6A]; il testo dice che "all of our results hold" [IA.3].
18. **Piccole incoerenze di scrittura.** Le didascalie delle Tab. A7-A9 definiscono le call fuori dal
    denaro con strike/prezzo "maggiore di 0,9" e dentro "minore di 1,1", due insiemi che si sovrappongono
    (probabilmente 1,1 e 0,9 scambiati). Il testo dell'IA.4 chiama "Panel B" il denaro e "Panel C" la
    scadenza, la Tab. A7 il contrario: qui si segue la tabella. La didascalia della Tab. 3C e della Tab. 5
    parla di "sample days" per regressioni settimanali da 420 osservazioni.

## Cosa Dice Al Riquadro "Chi Ha Comprato Delta Oggi"

Il riquadro mostra il **DEX di SPX per gruppo** (customer, professional, firm, broker-dealer), 0DTE e tutte
le scadenze, cumulato dall'inizio della giornata. Dal paper:

- **Sul flusso di SPX il paper trova poco o niente.** Lo sbilancio delle call dei clienti su SPX non
  predice il mercato il giorno dopo (t 0,90), a tre giorni (t -0,92) ne' alla settimana, per nessuna taglia
  (t fra -0,86 e 0,82); al mese t -1,83; solo a due mesi -0,583 (t -2,74), al contrario [Tab. 7A, Tab.
  A11]. Su NDX niente a nessun orizzonte (t massimo in valore assoluto 1,64). Il segnale contrarian forte e'
  quello delle **opzioni sui titoli** sommate, che il riquadro non vede.
- **Il verso, quando c'e', e' al contrario e lento**: piu' i clienti piccoli comprano call, peggio va il
  mercato nelle settimane e nei mesi dopo. Non e' un segnale di giornata ne' intraday.
- **Professional, market maker, firm e broker-dealer**: nessuna previsione sul mercato alla settimana (firm
  e broker-dealer solo dentro i market maker) [Tab. 4B]; al giorno una sola eccezione, i market maker al
  denaro, di segno positivo (t 2,60) [Tab. A8A].
- **La normalizzazione**: comprato meno venduto diviso comprato piu' venduto, solo aperture, poi
  confrontato con la sua storia (standardizzato). Il livello medio dei clienti e' **negativo** (sono
  venditori netti, risultato 1): **un segno da solo non dice niente**, conta la distanza dal suo solito.
  E la serie e' lenta (autocorrelazione 0,72-0,78): cambia di regime in settimane, non in minuti.
- **Aperture e acquisti contano piu' di chiusure e vendite** (risultato 11, lettura degli autori; a
  settimana nessuna delle misure acquisto/vendita e' significativa al 5%): da verificare se il DEX di
  OptionsDepth distingue apertura e chiusura; se le somma, e' la versione "aperture piu' chiusure" che nel
  paper non predice (t -1,33).
- **Call e put dicono cose diverse**: le call il sentiment (prezzo), le put la volatilita' che verra'
  (risultato 22). Un DEX che somma call e put mescola le due.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

- **Il finding come e' scritto, no.** Serve il Cboe Open-Close di migliaia di titoli dal 2005, a pagamento,
  e una variabile da predire che non e' NQ. Non e' un dato di ATAS (CLAUDE.md, decisione 2).
- **Quanta storia servirebbe.** Nostro calcolo, non del paper: se la t crescesse con la radice del numero
  di osservazioni e l'effetto fosse quello medio, per arrivare a t = 2 servirebbero circa **465 settimane**
  (t 2,69 su 841) o circa **1.860 giornate** (t 2,94 su 4.027), cioe' sette-nove anni. Lo storico del DEX per
  gruppo di `XrayLive` parte da ottobre 2026: **nessuna prova e' possibile per anni**.
- **Cosa si puo' fare subito, come descrizione**: mostrare nel riquadro, per ogni gruppo, lo sbilancio
  normalizzato fra -1 e +1 accanto al numero assoluto, e la sua distanza dal valore tipico dei giorni
  registrati alla stessa ora (z-score), dichiarando quanti giorni ci sono dietro. Su NQ resta **contesto**
  (CLAUDE.md, X-Ray su NQ e' solo contesto), mai un ingresso, e nessun backtest.
- **Registrazione in avanti**: salvare ogni giorno, alla chiusura, lo sbilancio normalizzato per gruppo e
  per tipo (call, put), cosi' che fra qualche anno la domanda del paper si possa fare su SPX, con un
  orizzonte di settimane.

## Cosa Ne Prende Il Framework

- **La misura**: sbilancio fra -1 e +1, solo aperture, standardizzato sulla sua storia; da preferire al
  numero assoluto per confrontare giorni diversi.
- **La lezione sezione trasversale contro serie storica**: lo stesso flusso puo' predire un titolo in un
  verso e il mercato nel verso opposto. Un finding sui titoli non si porta su un indice senza provarlo.
- **Il risultato negativo sugli indici**: sul flusso delle opzioni di SPX e NDX per gruppo, questo paper
  non da' una direzione al giorno ne' alla settimana. Va scritto accanto al riquadro, perche' non si legga
  come un segnale.
- **Nessuna regola operativa**: orizzonte di settimane, dati che non abbiamo, nessun costo, nessun
  fuori campione dopo il 2020.

## La Verifica

**9 novembre 2026**, verifica avversaria sul PDF (versione del gennaio 2026 da jfqa.org, 72 pagine; sha256
ricalcolato e uguale a quello della scheda). Il DOI non e' stampato nel PDF: viene dalla scheda della
ricerca. Testo estratto con `pdftotext -layout`; **tutte le tabelle (1-7, A1-A14) rilette con
`pdftotext -table`**, che tiene le colonne, e le dubbie (Tab. 1, A3, A4, eq. 5) anche con `-raw`. Le
pagine non si sono potute vedere come immagine (manca `pdftoppm`): resta questo limite. Nessuna
istruzione trovata nel PDF.

**Circa 220 affermazioni controllate** (ogni numero della tabella dei risultati, i termini, il metodo,
i sedici punti di "Cosa Non Regge", il riquadro). **Confermate circa 200**, tutte le cifre delle righe
SPX e NDX comprese (Tab. 7A, 7B, A11: giuste). **Corrette 17, tolta 1** (il -0,329 "dell'abstract":
l'abstract non ha numeri). Le correzioni:

1. **La piu' grave**: il -0,329 settimanale non viene da "17 controlli" ma da **7** (APIB, ICIB, IPIB, VRP,
   IVS, BKM Skew, AAII); i 17 sono solo al mese. Corretti il metodo, il risultato 4 e il punto 10.
2. **Omesso**: con il solo APIB accanto, ACIB a una settimana ha t -1,67, non significativo (Tab. A3,
   colonna III); e ACIB pesato in modo uguale non regge a una settimana e al mese (Tab. A6A). Aggiunti al
   risultato 3 e come punto 17.
3. Risultato 10: "professional mai oltre t 1,56" era impreciso (ACIB fino a 1,55, APIB fino a -1,67);
   aggiunto APIB dei market maker dentro il denaro, t 2,00, e il campione 2009-2020.
4. "I market maker non predicono niente" (In parole semplici, riquadro): ridotto a "quasi niente", con
   l'eccezione al denaro della Tab. A8A.
5. Punto 5: la firm e' separata nella Tab. A2 (quote di volume), non come predittore; la spiegazione
   "controparte" e' nostra, quella del paper e' la "contaminazione" dei professional.
6. Risultato 11 e riquadro: "e' il comprare che porta il segnale" e' la lettura degli autori; a settimana
   nessuna delle tre misure e' significativa, e al giorno anche la vendita fuori dal denaro ha t 2,00.
7. Tab. A7 e A8: ogni colonna e' una regressione con ACIB e APIB insieme; detto nel metodo e nei
   risultati 8, 12, 13. Aggiunto il medium significativo al mese.
8. La standardizzazione "sul campione intero" non e' scritta nel paper: segnata come presunta.
9. Newey-West con K-1 ritardi: a un periodo i ritardi sono zero; aggiunto nei termini.
10. Punto 6 (ITI): riletto sul PDF, il paper non scioglie il dubbio; scritto cosa cambia nei due casi.
11. Punto 8 (R quadro giornalieri): confermato con il conto t^2/N, che torna alla settimana e al mese e non
    al giorno, salvo la Tab. A6B.
12. Punto 9: aggiunte le differenze a tre giorni e a due mesi fra A3 e A6, e APIB 0,185 contro 0,158 fra
    Tab. 2 e A4; il manager sentiment in A4 e' stampato con t sopra e coefficiente sotto; la media semplice
    della Tab. A2 rida' i 13,27% e 9,09% del testo, e il 65% dei market maker non torna.
13. Risultato 17: aggiunti i coefficienti di NDX nella Tab. 7B (tutti nulli).
14. Risultato 16: il paper non dice quali gruppi entrano nel CIB degli indici.
15. Risultato 21: aggiunti i paesi non significativi a tre giorni e al mese.
16. Punto 10: le t principali stanno per lo piu' fra 2,4 e 3,6, non fra 2,4 e 3,0.
17. Punto 18 nuovo: la definizione del denaro per le call e' incoerente nelle didascalie, e il testo
    dell'IA.4 scambia i pannelli B e C della Tab. A7 (qui si segue la tabella).

**Cosa resta incerto**: a quale settimana si attacca la ITI (punto 6); da dove viene l'R quadro di base
delle regressioni giornaliere (punto 8); quali gruppi entrano nel CIB di SPX e NDX; se in Tab. A4 il
manager sentiment a due mesi abbia anch'esso coefficiente e t scambiati. Nessuno di questi tocca il
risultato che conta per noi: **sulle opzioni di SPX e NDX nessuna previsione significativa al giorno o
alla settimana, e su NDX a nessun orizzonte**.
