---
chiave: squeezemetrics-2017-gamma-exposure
sigla: SM
titolo: "Gamma Exposure (GEX): Quantifying hedge rebalancing in SPX options"
autori: [SqueezeMetrics Research]
versione_letta: "marzo 2016, rivista dicembre 2017 (il PDF e' stato generato il 7 marzo 2020)"
fonte: SqueezeMetrics (Prior Analytics LLC), sito dell'editore
identificativo: "nessuno"
url: https://squeezemetrics.com/download/white_paper.pdf
tipo: white paper
revisione_fra_pari: no
pagine: 8
pdf: squeezemetrics-2017-gamma-exposure.pdf
pdf_sha256: 23eaf3fa1bbb765dc9b4a9077d64a784d930662bbf329a9872d4da7c9a60a8c1
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; metodo e serie dei dati sono proprietari, venduti dall'editore"
---

# SqueezeMetrics 2017: L'Analisi

*Gamma Exposure (GEX): Quantifying hedge rebalancing in SPX options*, SqueezeMetrics Research,
marzo 2016, rivisto dicembre 2017, 8 pagine (una di avvertenze, una di bibliografia, due di soli
grafici). **White paper commerciale**: nessuna revisione fra pari, nessun autore con nome; l'editore
(Prior Analytics LLC) **vende** i dati GEX che il documento promuove. Letto per intero il 14
ottobre 2026. Citazioni `[SM p. n]`, con il numero di pagina del PDF.

**Come e' stato letto.** Il testo estratto con `pdftotext` e' completo, ma le formule perdono la
lettera greca della gamma e i segni di moltiplicazione, e le pagine 3 e 6 sono solo grafici. Pagine
3, 5 e 6 rilette come immagini (PyMuPDF, nella scratchpad). I valori letti **dai grafici** sono
approssimati e lo diciamo.

## La Domanda

La quantita' di opzioni in circolazione, e non il loro prezzo, prevede la volatilita' dell'S&P 500
del giorno dopo meglio del VIX?

## Il Metodo

**Il meccanismo** [SM p. 4]. Il market maker (il *dealer*) che ha venduto o comprato un'opzione si
copre con il **delta hedging**: tiene sul sottostante una posizione opposta al delta dell'opzione.
Quando il prezzo si muove il delta cambia, e la copertura va rifatta. La **gamma** e' quanto cambia
il delta per un punto di movimento del sottostante: dice quanto il dealer deve comprare o vendere.
Se il dealer e' **lungo gamma**, ricoprendosi compra quando il prezzo scende e vende quando sale:
smorza il movimento. Se e' **corto gamma**, vende nei ribassi e compra nei rialzi: lo amplifica.

**Le quattro assunzioni** [SM p. 4-5]:

1. tutte le opzioni passano da un dealer che fa delta hedging;
2. **le call le vendono gli investitori e le comprano i dealer** (vendita di call coperte, collari);
3. **le put le comprano gli investitori e le vendono i dealer** (put di protezione);
4. il dealer si copre esattamente al delta, senza bande di tolleranza.

Gli autori ammettono che la direzione degli scambi non si osserva (la maggior parte si fa a meta'
fra denaro e lettera) e che la 2 e la 3 si giustificano anche "circolarmente", con gli effetti
stessi del GEX [SM p. 4].

**Il calcolo** [SM p. 5], verificato sull'immagine della pagina:

```text
GEX di uno strike, call  =  Gamma x OI x 100
GEX di uno strike, put   =  Gamma x OI x (-100)
GEX dell'indice          =  somma su tutti gli strike e tutte le scadenze
```

**GEX** (*gamma exposure*) e' quindi la gamma totale che si attribuisce ai dealer, in azioni. OI e'
l'*open interest*, i contratti aperti; 100 e' il moltiplicatore da contratto ad azioni. Per l'SPX la
nota a margine dice "lo esprimiamo in dollari", senza dire come.

**La prova** [SM p. 2-3, 6]: distribuzioni del rendimento giornaliero dell'SPX per quartili del
VIX e del GEX, e un grafico a punti del rendimento da chiusura a chiusura contro il GEX della
chiusura del giorno prima, "2004 - Present".

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | il VIX e' correlato 0,75 con la volatilita' realizzata dei 30 giorni dopo e **0,85 con quella dei 30 giorni prima** (cifre del CBOE, citate) | p. 2 |
| 2 | deviazione standard del rendimento SPX a 1 giorno: dopo il quartile piu' basso del VIX **0,51%**, dopo il secondo **0,66%** | p. 2, grafico p. 3 |
| 3 | dopo il quartile piu' alto del GEX **0,55%**, dopo il secondo piu' alto **0,85%**. SPX, giornaliero, 2004 - "oggi" (dicembre 2017, presumibilmente) | p. 2, grafico p. 3 |
| 4 | "con l'indice a 2000, la differenza fra 0,66% e 0,85% e' di 4 punti di range ogni giorno" | p. 2 |
| 5 | grafico a punti: con GEX sotto zero la dispersione dei rendimenti cresce ("in modo esponenziale", dicono); con GEX alto si stringe. Asse del GEX da circa -2 a circa +2,9 miliardi (letto sul grafico, unita' non dichiarata); i rendimenti estremi, fino a circa +11% e -9%, stanno tutti fra GEX -1 e 0 miliardi | p. 6 |
| 6 | GEX vicino a zero: "il mercato si muove liberamente", senza interferenza dei dealer | p. 6 |
| 7 | scegliere titoli dell'S&P 500 con GEX vicino a zero "ha prodotto rendimenti in eccesso": **nessun numero** | p. 7 |

Non c'e' altro: nessuna tabella, nessuna numerosita', nessun test.

## Cosa Abbiamo Riprodotto

Niente: non abbiamo una storia di GEX. Un controllo aritmetico (nostro): (0,85% - 0,66%) x 2000 =
3,8 punti, e il paper scrive 4. Ma confronta il **secondo quartile del VIX** con il **terzo del
GEX**: non e' lo stesso confronto (vedi sotto, punto 6).

## Cosa Non Regge, O Non Si Dice

1. **Conflitto d'interessi.** Chi scrive vende l'indicatore. Nessun autore, nessuna revisione,
   nessun dato pubblicato con il documento.
2. **Il segno dei dealer e' assunto, non misurato.** Dealer lunghi di tutte le call e corti di
   tutte le put, a ogni strike e scadenza. Golez e Jackwerth misurano, sulle SPX alla pari il giorno
   prima della scadenza (1992-2009), dealer **corti anche delle call** (-1.840 call, -12.315 put in
   media) [GJ §2]. Se sulle call alla pari il dealer e' corto, il segno di quel pezzo del GEX e'
   rovesciato. Gli stessi autori dicono che l'assunzione si giustifica in parte con gli effetti del
   GEX: e' circolare.
3. **Le unita' non tornano o non sono dette.** "La gamma di un'opzione a 50 delta e' 10" mescola
   gamma per azione e delta in centesimi. Il GEX "in dollari" per l'SPX non ha formula: per punto di
   movimento? per 1%? Il grafico ha l'asse in 1e9 senza unita'. Gli indicatori di oggi (ATAS
   compreso) lo danno in $ per 1% di movimento: e' una convenzione loro, non di questo documento.
4. **Il calcolo non e' specificato.** Con quale modello e quale volatilita' implicita si calcola la
   gamma, con quale prezzo, quali scadenze (le settimanali?), da quale fonte e a che ora l'OI. L'OI
   di una giornata si pubblica la mattina dopo: il "GEX della chiusura del giorno prima" usa dati che
   alla chiusura non c'erano ancora. Sul rendimento da chiusura a chiusura e' un **look-ahead**
   piccolo ma da controllare.
5. **Nessuna statistica.** Quattro deviazioni standard e due grafici; nessuna numerosita', nessun
   intervallo di confidenza, nessun test, nessun campione fuori. Le soglie dei quartili sono
   calcolate su tutto il periodo.
6. **I confronti sono scelti.** Per il VIX i due quartili piu' bassi, per il GEX i due piu' alti; il
   "4 punti al giorno" confronta il secondo quartile del VIX con il terzo del GEX. Il confronto che
   conta manca: **il GEX dice qualcosa in piu' del VIX, o della volatilita' realizzata di ieri?**
7. **Un'altra spiegazione c'e'**, contro "sarebbe difficile trovarne una alternativa" [SM p. 6].
   Deduzione nostra: il GEX non e' esogeno. Dipende dalla distanza del prezzo dagli strike con
   molto OI e dalla volatilita' implicita. Dopo un ribasso il prezzo si avvicina agli strike delle
   put e il GEX scende; la volatilita' dopo un ribasso e' alta per conto suo (la volatilita' si
   raggruppa nel tempo). Il grafico a punti e' compatibile con questo, **senza** che la copertura
   dei dealer muova niente. Inoltre il GEX in livello non e' normalizzato: l'OI e' cresciuto dal
   2004 al 2017, e il quartile alto puo' cadere soprattutto negli anni calmi.
8. **Le bande di copertura** (il dealer non si ricopre a ogni tick) sono ignorate per scelta
   (assunzione 4); chi si copre con i future e non con le azioni e' citato solo in una nota.
9. **Nessun costo, nessuna strategia**, e la frase sulla selezione dei titoli con GEX vicino a
   zero non ha un numero.

## Cosa Si Puo' Rifare Su ES, Con ATAS

**Cosa abbiamo.** Barre a 1 minuto di ES dal Data Bridge, con volume e delta; storia **da misurare**
(per NQ: mesi, non anni; [`i-dati-da-atas.md`](../../../metodo/i-dati-da-atas.md)). Gli indicatori
([`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md)): **Options Chain** dalla nostra
connessione Rithmic, con GEX Profile e Zero Gamma, **senza storia**; **Options X-Ray**, dati SPX di
OptionsDepth ogni minuto, con Flip, GEX heatmap, charm heatmap e hedge pressure, storia
**sconosciuta**. **Zero Gamma** (o *Flip*) e' il prezzo a cui il GEX totale cambia segno.

**Cosa serve storia che non abbiamo.** La prova di SM (GEX della sera prima contro la volatilita'
del giorno dopo, 2004-2017) vuole **anni** di GEX giornaliero. ATAS non li ha. Una serie storica di
fornitori esterni (lo stesso SqueezeMetrics, SpotGamma, OptionsDepth, CME) **esce dalla decisione 2
del progetto**: servirebbe una richiesta esplicita dell'utente. Se X-Ray avesse storia accessibile,
cambierebbe tutto: **va misurato** quanto indietro va, e se il Data Bridge legge i valori di un
altro indicatore.

**Cosa permette una registrazione in avanti.** Ogni giorno, alla chiusura e all'apertura: GEX
totale, Zero Gamma, muri (Chain su ES, X-Ray su SPX); ogni minuto, il Flip e la hedge pressure di
X-Ray. Le barre di ES gia' le abbiamo. Dopo 6 mesi ci sono circa 125 giornate: bastano solo per un
effetto grande come quello dichiarato (rapporto delle deviazioni standard 0,85 / 0,55, circa 1,5);
dopo un anno, circa 250.

**Ipotesi da scrivere in una scheda, se si apre lo studio** (non sono regole):

- **H1**: il segno del GEX alla chiusura di ieri prevede il range della seduta di oggi di ES (dalle
  barre a 1 minuto) **oltre** il range di ieri e la media dei 5 giorni prima. Il nullo e' la sola
  volatilita' passata: se il GEX non aggiunge niente a quella, il punto 7 qui sopra basta a
  spiegarlo. Un'osservazione per giornata.
- **H2**: con il prezzo **sotto** lo Zero Gamma (o il Flip di X-Ray, registrato al minuto)
  l'autocorrelazione dei rendimenti di ES a 5 minuti e' piu' alta (il movimento continua) che con il
  prezzo sopra (il movimento rientra). E' la versione intraday del meccanismo, e si misura con le
  barre di ES piu' la registrazione del Flip.
- **H3**: il GEX di Chain (opzioni su ES, dalla nostra connessione) e quello di X-Ray (opzioni SPX,
  con le posizioni divise per tipo di partecipante) danno lo stesso segno quasi sempre. Se no, quale
  dei due prevede meglio dice quale assunzione sui dealer regge: quella fissa di SM o quella
  stimata di OptionsDepth.

## Cosa Ne Prende Il Framework

- **La definizione e la convenzione dei segni** del GEX, come variabile da registrare, con le sue
  quattro assunzioni scritte accanto.
- **Il nullo giusto per un previsore di volatilita'**: la volatilita' passata. Un indicatore di
  volatilita' che non batte il range di ieri non e' un risultato, perche' la volatilita' si
  raggruppa da sola.
- **La lezione di metodo**: un grafico a punti senza numerosita' e senza test non e' una prova, e
  "difficile trovare un'altra spiegazione" e' il punto dove la si cerca.
- **Il contrasto con Golez e Jackwerth**: il segno dei dealer sulle call. Vedi
  [`../golez-2012-pinning-in-the-sp500-futures/analisi.md`](../golez-2012-pinning-in-the-sp500-futures/analisi.md).
