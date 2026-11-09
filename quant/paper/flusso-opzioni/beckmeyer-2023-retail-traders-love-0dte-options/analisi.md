---
chiave: beckmeyer-2023-retail-traders-love-0dte-options
sigla: BBG
titolo: "Retail Traders Love 0DTE Options... But Should They?"
autori: [Heiner Beckmeyer, Nicole Branger, Leander Gayda]
versione_letta: "SSRN 4404704, versione del 27 luglio 2024 (prima versione 30 marzo 2023)"
fonte: SSRN (working paper; presentato a convegni, fra cui la German Finance Association 2023 e la FMA European 2024)
identificativo: "10.2139/ssrn.4404704"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=4404704
tipo: preprint
revisione_fra_pari: no
pagine: 50 (frontespizio, testo e tabelle alle pagine 1-39 numerate, bibliografia 40-43, appendici A-D 44-49)
pdf: beckmeyer-2023-retail-traders-love-0dte-options.pdf
pdf_sha256: e97a8685aceef757121377a5e635b8c71a8638a37fb5d748c793c707f9695c50
filone: flusso-opzioni
ruolo: metodo
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati Cboe a livello di transazione con le quote dei market maker e il profilo giornaliero Open/Close di Cboe (a pagamento), commenti di Reddit r/wallstreetbets"
---

# Beckmeyer, Branger, Gayda 2023: L'Analisi

*Retail Traders Love 0DTE Options... But Should They?*, Heiner Beckmeyer, Nicole Branger, Leander Gayda
(Universita' di Munster). Working paper su SSRN, prima versione 30 marzo 2023, **letta la versione del 27
luglio 2024**. **Non revisionato** (il frontespizio non indica una rivista). Letto per intero il 9 novembre
2026. Il testo estratto con `pdftotext -layout` mescola le colonne delle tabelle 2, 3, 4, 7, 8 e D2-D4 e
perde i segni meno nel testo: le tabelle sono rilette con `pdftotext -table`, e le pagine 13-14, 21-22, 27,
35-36 sono state rese come immagine (`pypdfium2` nella scratchpad) per controllare segni e formule.
Citazioni `[BBG §n]`, `[BBG Tab. n]`, `[BBG Fig. n]`; le pagine sono quelle stampate.

**In parole semplici.** Il paper non chiede se il retail sugli 0DTE di SPX anticipa l'indice: chiede se ci
guadagna. La risposta e' no: fra febbraio 2021 e settembre 2023 il retail che riescono a riconoscere ha perso
oltre 125 milioni di dollari, per circa tre quarti in costi di transazione. Il loro "retail" e' una fetta
piccola e precisa (2-6% del volume), riconosciuta da un codice d'asta, non il gruppo "clienti" del Cboe che
OptionsDepth chiama retail. Per il nostro riquadro dice due cose: l'etichetta "retail" va presa con cautela,
e nessuna prova del paper mostra che il verso delle scommesse retail anticipi il prezzo.

## I Termini

- **0DTE** (*zero days to expiration*): opzioni che scadono il giorno stesso in cui si scambiano. Su SPX
  esistono ogni giorno della settimana dal **16 maggio 2022**, quando il Cboe ha aggiunto le scadenze del
  martedi' e del giovedi' [BBG §1, §3, §4.1].
- **AIM** (*Automated Price Improvement Mechanism*): l'asta del Cboe in cui un ordine piccolo puo' essere
  eseguito a un prezzo migliore della quota. Attivata su SPX su proposta SR-CBOE-2020-051 [BBG §1, §3.1].
- **SLAN** e **MLAT**: i codici OPRA delle operazioni passate dall'asta, *Single Leg Auction Non ISO* (una
  sola opzione) e *Multi Leg Auction* (piu' opzioni nello stesso ordine). **E' cosi' che il paper riconosce
  il retail**, dal 22 febbraio 2021 [BBG §3.1].
- **Internalizzazione, wholesaler**: un intermediario (il *wholesaler*) compra il flusso d'ordini dei
  broker al dettaglio (*payment for order flow*) e lo esegue contro il proprio inventario. Il paper vede
  solo la parte internalizzata che passa dall'asta [BBG §1, §3.1].
- **Single-leg, multi-leg**: un ordine su una sola opzione, o su piu' opzioni insieme (una strategia
  complessa). **Put spread, call spread** (verticali): compro un'opzione e ne vendo un'altra dello stesso
  tipo a uno strike diverso; scommessa sul verso con perdita massima limitata. **Iron condor, butterfly,
  straddle, strangle**: scommesse sulla volatilita' (che l'indice si muova poco o molto) [BBG §3.1, App. C].
- **Debit, credit**: un ordine *debit* paga un premio all'ingresso (per una singola opzione vuol dire
  comprarla); uno *credit* lo incassa (vendere) [BBG §4.2].
- **Regola della quota** (*quote rule*): un'operazione sopra il prezzo medio fra denaro e lettera (*mid*) e'
  un acquisto, sotto e' una vendita; al mid resta indeterminata [BBG §2].
- **Spread quotato e spread effettivo**: QS = (lettera - denaro) / mid; ES = 2 x |prezzo - mid| / mid. Lo
  spread effettivo e' quanto si paga davvero rispetto al mid [BBG eq. 2].
- **Open/Close**: il dato giornaliero del Cboe che divide il volume per tipo di partecipante (fra cui i
  market maker) [BBG §3]; il paper ne usa solo il partecipante e il verso, la divisione fra apertura e
  chiusura non la nomina. Che sia lo stesso tipo di dato da cui viene il DEX per gruppo di X-Ray e' nostro,
  non del paper.
- **Sbilancio d'ordini dei market maker**: contratti comprati meno venduti dai market maker, diviso il
  volume totale del giorno [BBG eq. 1]. Negativo vuol dire che i market maker vendono, cioe' gli utenti finali
  comprano.
- **Premio per il rischio di varianza** (*variance risk premium*): in media le opzioni costano piu' di
  quanto pagano, perche' chi le compra paga un'assicurazione; chi le vende lo incassa [BBG §4.1].
- **Margine** (Regulation T, manuale dei margini del Cboe): il capitale che il broker blocca per una
  posizione. Il **rendimento corretto per il margine** e' il profitto netto diviso il margine [BBG eq. 4].
- **Varianza realizzata, asimmetria realizzata, range**: misure della giornata dell'S&P 500 dai rendimenti a
  un minuto (la varianza in logaritmo; l'asimmetria come Amaya e altri 2015), e massimo meno minimo [BBG
  Tab. 2].
- **Effetti fissi** (del giorno, del giorno della settimana): una costante per ogni giorno, che toglie cio'
  che e' comune a tutte le operazioni dello stesso giorno. **Newey-West**: errori standard che tengono conto
  dell'autocorrelazione. **Winsorizzare all'1%**: tagliare i valori oltre il 1° e il 99° percentile.
- **Delta, gamma, vega**: la sensibilita' dell'opzione al sottostante, quanto cambia il delta, la
  sensibilita' alla volatilita'. **|Delta|** e' la misura di quanto l'opzione e' dentro o fuori dal denaro
  (0,5 circa al denaro) [BBG Tab. 1].

## La Domanda

Il retail ha guadagnato dagli 0DTE sull'S&P 500? Quanto ci scambia, come (singole opzioni o strategie,
acquisti o vendite, quali strike, a che ora), cosa spinge la sua domanda e cosa spiega profitti e perdite,
in totale e operazione per operazione [BBG §1]. **Non chiede se il flusso retail anticipi l'indice**: nessuna
regressione del paper ha come variabile il rendimento futuro dell'S&P 500.

## Il Metodo

**I dati** [BBG §2]: transazioni sulle opzioni S&P 500 fornite dal Cboe, che secondo gli autori coprono
tutto il volume perche' quelle opzioni si scambiano solo sui mercati del Cboe; gennaio 2005 - settembre 2023,
ma quasi tutte le analisi su **febbraio 2021 - settembre 2023**. Ogni operazione e' abbinata all'ultima quota
dei market maker; il verso con la regola della quota (Muravyev 2016). Filtri di Bryzgalova e altri (2023):
via le cancellate, le taglie non positive, gli spread negativi e i prezzi fuori da [denaro - spread, lettera
+ spread]; operazioni con lo stesso istante e lo stesso identificativo trattate come una. Il paper **non
distingue** SPX e SPXW, ne' dice se c'e' XSP.

**Il retail** [BBG §1, §3.1]: le operazioni con codice SLAN o MLAT, cioe' ordini fino a **10 contratti**
(per le strategie: la gamba piu' piccola fino a 10) internalizzati e passati dall'asta AIM, dal 22 febbraio
2021. E' **solo una parte** del retail: quella che il wholesaler non manda direttamente al Cboe. Una prova
alternativa usa la taglia 1 come segno di retail [BBG App. B]. Le strategie multi-leg sono riconosciute con
l'algoritmo di Li (2020) fra le operazioni MLAT dello stesso millisecondo: il 97,99% delle operazioni (98,24%
del volume) cade in una delle 24 strategie predefinite [BBG §1, App. C]. Il verso di una strategia viene dal
confronto fra mid sintetico e prezzo netto (Li e altri 2023) [BBG nota 12].

**I market maker** [BBG §3, eq. 1, Fig. 2]: dal profilo giornaliero Open/Close del Cboe, sbilancio
giornaliero diviso il volume totale, media mobile di 5 giorni, febbraio 2021 - febbraio 2023.

**Il profitto** [BBG §4.1, eq. 3]: per ogni operazione retail, verso (+1 acquisto, -1 vendita) x taglia x
100 x (valore a scadenza - prezzo d'ingresso). Il valore a scadenza e' il payoff al regolamento dell'S&P 500;
le posizioni si considerano **tenute fino a scadenza**, salvo un'operazione opposta osservata. **Lordo**: il
prezzo d'ingresso e' il mid; **netto**: il prezzo pagato. Esclusi commissioni del broker, oneri regolamentari
e di compensazione. Il rendimento corretto per il margine divide il netto per il margine del manuale Cboe
[BBG eq. 4].

**Le regressioni**:

- quota giornaliera del retail sul volume 0DTE contro varianza, asimmetria e range dello stesso giorno e il
  giorno del FOMC, Newey-West [BBG Tab. 2];
- per operazione, una variabile "e' un acquisto" (la didascalia dice che vale 1; il testo legge i
  coefficienti in punti percentuali, e le loro dimensioni lo confermano: in pratica 0/100) contro |Delta|, ore a scadenza, volatilita'
  implicita, taglia, multi-leg; effetti fissi del giorno, errori raggruppati per giorno [BBG Tab. 3];
- profitto netto aggregato del giorno contro le stesse misure dell'indice, il volume retail e il FOMC,
  Newey-West [BBG Tab. 7];
- per operazione, rendimento netto corretto per il margine (in %, winsorizzato all'1%) contro |Delta|, ore a
  scadenza, volatilita' implicita, taglia, spread, delta firmato, gamma, vega, multi-leg, debit; effetti
  fissi del giorno, errori raggruppati per giorno [BBG Tab. 8; per gruppi di strategie Tab. D2-D4].

**Nessun campione fuori**, nessuna strategia, nessun modello nullo: e' uno studio descrittivo.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Il mercato 0DTE**: dal 2018 quasi tutta la crescita del volume S&P 500 e' nelle scadenze fino a 4 giorni, e soprattutto negli 0DTE; il volume oltre i 4 giorni e' un po' sceso. Le scadenze fino a una settimana passano dal 20-30% del volume (2018) a oltre il 60%, e di questo oltre due terzi sono 0DTE: **oltre il 40% dei contratti scade in giornata** | §1, §3, Fig. 1 |
| 2 | **I market maker sono corti di 0DTE**: lo sbilancio 0DTE dei market maker e' sempre negativo, fra **-0,5% e -1%** del volume fino a maggio 2022, poi stabilmente **sotto -1%**. Sulle altre scadenze era negativo prima e tende al positivo dopo: dal maggio 2022 gli utenti finali **comprano 0DTE e vendono le altre scadenze** | §3, eq. 1, Fig. 2 (segni controllati sul PDF, p. 14) |
| 3 | **Quanto pesa il retail**: sul totale delle opzioni S&P 500 circa il **2%** del volume nel 2021, circa il **4%** alla fine; sugli 0DTE circa il **4%** nel 2021 e **oltre il 6%** nel 2022; sulle altre scadenze intorno al 2%. Gli 0DTE passano da circa il 30% a circa **tre quarti** del volume retail. In volume retail (tutte le scadenze): circa mezzo milione di contratti al mese nella prima meta' del 2021, oltre 1,5 milioni al mese dopo maggio 2022 | §1, §3.1, Fig. 3 |
| 4 | **Singole e strategie**: la quota single-leg degli 0DTE retail sale da meno del 20% (2021) a oltre il 60%; le strategie sono la maggioranza all'inizio e circa un terzo alla fine. Fra le strategie, put e call spread sono circa il 70% del volume (36,87% e 33,67%), le scommesse sulla volatilita' il resto; straddle e strangle rari (0,65% e 0,69%) | §1, §3.1, Fig. 4, Fig. 5, Tab. C1 |
| 5 | **Il profilo delle operazioni** (0DTE, retail contro non retail): call e put circa alla pari (49,0% e 51,0% del volume retail); taglia 1 nel 62,5% delle operazioni retail, oltre 10 contratti nello 0,1% (non retail: 44,4% del volume oltre 10); acquisti 40,1% e vendite 41,2% delle operazioni retail (volume 39,7% e 42,4%), 18,8% al mid; non retail 37,3% e 44,9%; il 61,7% delle operazioni retail con **3-6,5 ore a scadenza** (con la scadenza alle 16:00 di New York che il paper indica, cioe' fra le 9:30 e le 13:00), il 12,7% nell'ultima ora; fra \|Delta\| 0,125 e 0,625 (al denaro o poco fuori) la tabella da' il **65,2%** delle operazioni retail (63,4% del volume) contro il 55,1% (operazioni) e il 48,2% (volume) dei non retail: il testo scrive "circa il 75%" e "65%", che la tabella non da' (vedi le incoerenze) | §3.1, Tab. 1 |
| 6 | **L'asta funziona**: spread effettivo retail 5,7% sulle call e 4,8% sulle put, contro 10,4% e 7,9% dei non retail; anche nell'ultima ora lo spread effettivo retail e' circa la meta' | §3.1, Tab. 1 |
| 7 | **Cosa muove la quota retail** (costante 5,106%): +0,505 punti per una deviazione standard di varianza realizzata (t 7,5, R quadro 17,29%); range +0,204 (t 2,9, R quadro 2,67%; il testo scrive 0,24); asimmetria niente (0,033, t 0,7); nei giorni del FOMC la quota scende di 0,895 (t -4,0) e 0,848 (t -3,4, con il giorno della settimana) | §3.1, Tab. 2 |
| 8 | **Chi compra cosa** (probabilita' che l'operazione sia un acquisto, in punti): volatilita' implicita +4,328 / +4,343 / +4,643 per deviazione standard (t circa 18,7): il retail compra le opzioni piu' care; ore a scadenza circa -1 (t da -14 a -16); taglia -1,026 (t -34,90), -0,390 con il multi-leg; **multi-leg -18,130 (t -81,96)**: le strategie sono molto piu' spesso vendute. \|Delta\| +1,339 / +1,563 / +1,520, ma **-0,132 (t -2,15) quando entra il multi-leg** | §3.1, Tab. 3 |
| 9 | **Il retail perde**: perdita netta cumulata **oltre 125 milioni di dollari** a settembre 2023, lorda circa **-30 milioni**; perdite single-leg oltre 100 milioni. Media giornaliera netta **-241.000 dollari** sull'intero campione, **-350.000** dal 16 maggio 2022; lorda -57.000 e -106.000. Il lordo e' piatto da novembre 2022, il netto continua a scendere | §1, §4.1, Fig. 6, Tab. 4 A-B |
| 10 | **I costi pesano piu' della scelta**: "oltre 90 milioni" dei 125 sono costi di transazione, "circa il 70%" delle perdite giornaliere [§1]. Nostro conto dalla Tab. 4: (2,41 - 0,57) / 2,41 = 76% sull'intero campione, (3,50 - 1,06) / 3,50 = 70% dal 16 maggio 2022 | §1, Tab. 4 |
| 11 | **Debit e credit**: sull'intero campione -364.000 dollari al giorno sugli ordini debit e +122.000 sui credit (netti); dal 16 maggio 2022 -805.000 e +455.000. Le vendite guadagnano anche dopo i costi; gli autori lo leggono come **premio per il rischio di varianza** incassato da una parte del retail | §4.1, Tab. 4 A-B |
| 12 | **Mese, giorno, ora**: il netto mensile oscilla fra +286.000 al giorno (gennaio 2022) e -680.000 (settembre 2022); negativo ogni giorno della settimana (martedi' -429.000); **negativo in ogni ora d'ingresso** dalle 9:30 alle 15:00 (da -50.000 alle 10:00 a -22.000 alle 12:00), lordo anch'esso negativo in ogni ora | §4.1, Tab. 4 C-E |
| 13 | **Per strategia**: perdita media per operazione put -16,3 dollari, call -20,7; mediana di put e call spread **+85 dollari**; rendimento corretto per il margine: put media -5,6%, call -9,4%, mediane -0,7% e -0,8%, almeno un quarto delle operazioni perde il 100%; put e call spread mediana +3,0% e +3,3%. Le strategie a rischio definito vanno meglio di straddle, strangle e vendite scoperte | §4.1, Tab. 5, Tab. 6 |
| 14 | **Cosa spiega il profitto del giorno** (aggregato, netto): il range **+0,305 (t 4,5)**, R quadro 9,50%; la varianza realizzata niente (-0,037, t -1,0); l'asimmetria niente; il **volume retail -0,126 (t -3,3)**, -0,197 (t -4,3) con gli altri; il FOMC niente. Il testo traduce: +30.000 dollari per una deviazione standard di range, fra -13.000 e -20.000 per il volume; ma la costante (-0,241) e' la perdita media di 241.000 dollari, quindi l'unita' della tabella e' il **milione** e gli effetti sono circa **+305.000** e fra **-126.000 e -197.000** dollari (nostra deduzione, vedi le incoerenze) | §4.2, Tab. 7 |
| 15 | **Cosa spiega il profitto dell'operazione** (rendimento corretto per il margine): \|Delta\| +0,095 (t 35,76); ore a scadenza -0,023 (t -1,97); **volatilita' implicita -0,163 (t -9,54)**, che il testo legge "-16,3 punti base" (probabilmente -16,3 punti percentuali, punto 7 dei limiti); taglia +0,009 (t 9,13) e spread +0,004 (t 3,09), "convinzione"; **delta firmato 0,000 (t 0,02)**; gamma -0,030 (t -3,17); vega -0,052 (t -2,82); multi-leg +0,079 (t 7,98), debit **-0,287 (t -7,91)**, multi-leg x debit +0,160 (t 8,64) | §4.2, Tab. 8 |
| 16 | **Per gruppi di strategie**: volatilita' implicita e debit sempre negativi (single-leg IV -0,196, t -8,21, debit -0,280, t -7,54; spread IV -0,058, debit -0,154; volatilita' IV -0,027, debit -0,127). Ma lo spread denaro-lettera, positivo nella Tab. 8, e' **negativo** negli spread (-0,047, t -4,82) e nelle strategie di volatilita' (-0,039, t -9,78) e non significativo sulle singole (-0,003, t -1,21); anche la taglia regge solo sulle singole (+0,002, t 2,39; negli spread -0,000, t -0,38, nella volatilita' -0,004, t -1,73); il delta firmato sulle singole 0,000 (t 0,01) | App. D, Tab. D2-D4 |
| 17 | **Il retail per taglia 1** (identificazione alternativa): secondo l'App. B il 74,2% delle operazioni SLAN/MLAT ha taglia 1 (la Tab. 1, che l'App. B cita, dice 62,5%), ma oltre meta' del volume ha taglia maggiore (Tab. 1: 73,1%); la perdita cumulata netta ha lo stesso andamento | App. B, Fig. B2 |

## Il Perche'

**Degli autori** [BBG §1, §4.1, §5]:

- Il retail ama gli 0DTE per il **profilo da lotteria** (payoff molto asimmetrico, prezzo nominale basso) e
  per la promessa di guadagni rapidi (Reddit, siti dedicati). Ma la quota retail **non** sale con
  l'asimmetria realizzata, solo con la volatilita': contro una parte della letteratura sulle preferenze per
  la lotteria (Boyer e Vorkink 2014) [BBG §3.1, Tab. 2].
- Perde per **i costi** (lo spread pagato, che l'asta dimezza ma non toglie) e per **la scelta**: compra le
  opzioni piu' care (Tab. 3) e le opzioni piu' care rendono meno (Tab. 8).
- Chi **vende** (credit) e chi usa **strategie** a rischio definito va meglio: incassa il premio per il
  rischio di varianza; gli autori lo collegano all'idea che il retail piu' informato preferisca strategie
  complesse (Hu e altri 2023, sul mercato coreano).
- I **market maker** assorbono la domanda netta d'acquisto degli 0DTE e restano corti; gestiscono il rischio
  facilitando le vendite (spread effettivi piu' bassi sulle vendite) [BBG §3.1, Tab. 1]. Per gli effetti
  della loro copertura sull'indice rimandano a Dim e altri (2024) e Brogaard e altri (2024) [BBG nota 7].

**Alternative scartate**: le preferenze per l'asimmetria (Tab. 2); un interesse specifico per gli annunci
FOMC (la quota retail e' piu' bassa quel giorno, Tab. 2, e il profitto non diverso, Tab. 7).

**Cosa lo ucciderebbe** (dedotto da noi): perdite lorde non diverse da zero con un campione piu' lungo (il
lordo e' gia' piatto da novembre 2022, risultato 9), cioe' una perdita solo di costi; oppure che la parte di
retail non internalizzata, che il paper non vede, si comporti in modo diverso.

## Cosa Non Regge O Non Si Dice

1. **Non e' la nostra domanda.** Il paper non prova mai se il flusso retail anticipa l'S&P 500, a nessun
   orizzonte. Quello che si avvicina e' indiretto, e la lettura e' **nostra**: (a) il profitto lordo al mid
   fino al regolamento e' negativo (-57.000 dollari al giorno, Tab. 4 A), quindi le posizioni retail, prese
   insieme, non hanno anticipato il prezzo di regolamento abbastanza da pagare nemmeno il premio delle
   opzioni; ma il paper non da' una prova di significativita' su questa media lorda (la t -6,3 della Tab. 7
   e' della media **netta**), e il lordo e' stato positivo per mesi (fine 2021 - maggio 2022, Fig. 6).
   (b) Nella Tab. 8 (col. 4) il **delta firmato** dell'operazione non ha relazione con il rendimento
   (0,000, t 0,02; sulle singole 0,000, t 0,01, Tab. D2). **Questo pero' non e' una prova sul verso del
   retail** (correzione della verifica): con gli effetti fissi del giorno, la posizione netta del retail in
   quel giorno, cioe' proprio cio' che ci interesserebbe, finisce nella costante del giorno; resta il
   confronto fra operazioni dello stesso giorno, il cui coefficiente e' in buona parte la media, fra i giorni,
   del movimento dell'indice (positivo nei giorni di rialzo, negativo in quelli di ribasso), piu' la scelta
   del momento d'ingresso dentro la giornata. Un coefficiente nullo dice poco. Ne' (a) ne' (b) separano il
   verso dalla volatilita'.
2. **Il "retail" del paper non e' il "retail" di OptionsDepth.** Qui e' l'operazione d'asta SLAN/MLAT fino a
   10 contratti, internalizzata: il 2-4% del volume S&P 500 e circa il 6% degli 0DTE (risultato 3). Il gruppo
   "clienti" del Cboe, che OptionsDepth chiama retail, e' un'altra cosa e comprende anche gli istituzionali
   (ADEFOV §3.4, CLZZ nota 8). Il paper **non dice** che quota del volume dei clienti Cboe sia retail d'asta.
3. **Una sola parte del retail.** Il wholesaler puo' internalizzare solo una parte del flusso e mandare il
   resto al Cboe; quella parte il paper non la vede, e non stima quanto sia [BBG §1, §3.1]. A sostegno
   dell'identificazione il paper porta un aneddoto (una conference call di Interactive Brokers), una prova
   per taglia 1 che mescola altri partecipanti [BBG App. B], e il fatto che Bryzgalova e altri (2023) trovano
   il codice SLAN migliore delle regole sulla taglia, ma per le operazioni a una gamba nel mercato piu' ampio
   (opzioni su azioni ed ETF) [BBG §1, §3.1]: per gli 0DTE di SPX e per le strategie MLAT nessuna convalida
   esterna.
4. **Il profitto ipotizza la scadenza.** Ogni posizione e' valutata al regolamento "salvo un'operazione
   opposta osservata" [BBG §4.1], ma il paper non spiega come abbini apertura e chiusura di operazioni
   anonime. Se molte posizioni si chiudono prima (lo dice per gli 0DTE in generale, §1), i profitti misurati
   non sono quelli realizzati. Nella formula manca lo zero del massimo (max(S - K) invece di max(S - K, 0)):
   un refuso, il testo e' chiaro.
5. **I costi sono parziali**: niente commissioni ne' oneri; un conto "a spanne" con le commissioni di
   Interactive Brokers aggiungerebbe oltre 600.000 dollari solo a settembre 2022 [BBG §4.1]. Le perdite sono
   quindi una stima per difetto, come dicono.
6. **Incoerenze interne** (da guardare in verifica):
   - **Unita' della Tab. 7**: la costante -0,241 corrisponde alla perdita media di 241.000 dollari solo se
     l'unita' e' il milione; ma il testo traduce il range 0,305 in "+30.000 dollari" e il volume in "-13.000 /
     -20.000", cioe' usa l'unita' di 100.000. Una delle due letture e' sbagliata di un fattore 10, e la tabella
     non dichiara l'unita'. **Si decide** (nostra deduzione, in verifica): con i regressori standardizzati la
     costante e' la media del giorno, -0,241 come i -2,41 (in 100.000) della Tab. 4 A; e con un solo regressore
     l'R quadro da' la deviazione standard del profitto giornaliero, 0,305 / radice(0,095) = circa 1,0 unita'
     (col. 3), 0,126 / radice(0,0147) = circa 1,0 (col. 4). In unita' da 100.000 sarebbe 100.000 dollari, meno
     della dispersione delle sole medie mensili della Tab. 4 C (circa 226.000): impossibile. L'unita' e' il
     milione, e **gli effetti in dollari del testo sono dieci volte troppo piccoli** (range circa +305.000,
     volume fra -126.000 e -197.000 al giorno, anche nell'introduzione, p. 7).
   - **Ore a scadenza nella Tab. 8**: il coefficiente e' negativo (-0,023), cioe' piu' ore a scadenza, rendimento
     piu' basso; il testo dice "trades with a shorter time to maturity tend to perform worse" [§4.2]. Testo e
     tabella si contraddicono. Negativo anche sulle singole (Tab. D2, da -0,035 a -0,037, t circa -2,4);
     positivo negli spread (Tab. D3, da +0,010 a +0,022, t da 3,1 a 7,1); nelle strategie di volatilita' non
     significativo (Tab. D4, da +0,006, t 1,57, a -0,000).
   - **"North of $500,000"**: dopo la Tab. 4 il testo dice che il retail perde "oltre 500.000 dollari" nel
     giorno medio, contro -241.000 (Tab. 4 A) e -350.000 (Tab. 4 B).
   - **Acquisti o vendite?** La Tab. 1 classifica piu' vendite che acquisti fra le operazioni retail 0DTE
     (40,1% contro 41,2%; volume 39,7% contro 42,4%), mentre il §4.1 dice che il retail "preferisce gli ordini
     debit" e che gli utenti finali sono "prevalentemente lunghi". Le due cose possono convivere (debit e
     acquisto non coincidono per le strategie; il 18,8% e' al mid), ma il paper non lo chiarisce.
   - **"Holding period"**: l'introduzione dice che il retail preferisce "un periodo di detenzione fra 3 e 6,5
     ore" [§1], ma la Tab. 1 misura le **ore a scadenza al momento dell'operazione**, non la durata della
     posizione.
   - **\|Delta\| e acquisti**: "piu' dentro il denaro, piu' acquisti" (Tab. 3 col. 1-3) cambia segno quando
     entra il multi-leg (col. 4, -0,132, t -2,15); il testo riporta solo la prima lettura.
   - **"Convinzione"**: lo spread denaro-lettera positivo nella Tab. 8 e' negativo o nullo in tutti e tre i
     gruppi dell'App. D; il §1 dice che i gruppi "confermano" i risultati.
   - **Al denaro o poco fuori** (aggiunta in verifica): il testo dice che le operazioni con \|Delta\| fra
     0,125 e 0,625 sono "circa il 75%" di quelle retail e del loro volume, contro "65% e 48%" dei non retail
     [§3.1]; la Tab. 1 da' 40,2 + 25,0 = **65,2%** delle operazioni retail (63,4% del volume) e 34,9 + 20,2 =
     **55,1%** delle operazioni non retail (31,2 + 17,0 = 48,2% del volume). Torna solo il 48%.
   - **Taglia 1** (aggiunta in verifica): l'App. B dice che il 74,2% delle operazioni SLAN/MLAT ha taglia 1 e
     rimanda alla Tab. 1, che ne da' il 62,5% (il testo del §3.1 dice "quasi due terzi", coerente con la
     tabella).
   - Minori: \|Delta\| fra "0,325" e 0,625 nel testo, 0,375 nella tabella [§3.1, Tab. 1]; range 0,24 nel
     testo e 0,204 nella Tab. 2 [§3.1]; la didascalia della Tab. 2 dice che ci sono gli effetti fissi del giorno
     della settimana, la tabella li mette solo nella col. 5; la Fig. 1 parte dal
     gennaio 2004 nella didascalia e dal 2005 nel testo; Fig. 3 da febbraio 2021 nella didascalia e da gennaio
     nel testo; la Fig. 5 mostra 10 strategie e il testo dice 11.
7. **Unita' della Tab. 8**: la didascalia dice rendimento "in %", e il testo legge -0,163 come -16,3 punti
   base: fra loro sono coerenti (0,163% = 16,3 punti base). Con rendimenti fra -100% e +182% (Tab. 6), pero',
   un effetto di 0,16 punti percentuali per deviazione standard sarebbe minuscolo; se invece il rendimento
   fosse in decimali, l'effetto sarebbe -16,3 punti percentuali. Il testo non lo decide; **un indizio** (nostra
   deduzione, in verifica): aggiungere la sola volatilita' implicita alza l'R quadro di 0,40 punti (dall'8,89%
   al 9,29%, col. 1 e 2), il che con un coefficiente di 0,163 vuol dire una deviazione standard del rendimento
   di circa 2-2,5 unita'. In percento sarebbe il 2-2,5%, incompatibile con le code della Tab. 6; in decimali il
   200-250%, compatibile. E' quindi **probabile** che le Tab. 8 e D2-D4 siano in decimali e che il testo
   sottostimi di cento volte gli effetti (volatilita' implicita circa -16 punti percentuali, debit circa -29,
   multi-leg circa +8). Non e' una lettura del paper: va presa come dubbio forte, non come numero.
8. **Regressioni contemporanee**: la quota retail (Tab. 2) e il profitto del giorno (Tab. 7) sono spiegati
   con varianza e range **dello stesso giorno**: descrivono, non prevedono. Che il profitto salga con il range
   e' anche meccanico per chi e' lungo di opzioni (nostra osservazione).
9. **Statistica**: molte specifiche, nessuna correzione per prove multiple; t molto alte nelle regressioni
   per operazione (milioni di operazioni, errori raggruppati per giorno), ma R quadro corretto fra lo 0,39% e
   l'1,96% nella Tab. 3 e fra l'1,09% e l'11,85% nelle Tab. 8 e D2-D4; e quelli della Tab. 8 sono in gran parte
   degli effetti fissi del giorno (la col. 4, con i soli delta, gamma e vega e coefficienti piccoli, ha gia' l'8,60%; nostra lettura). Nessun
   modello nullo nel nostro senso, nessun campione fuori; il campione e' di circa due anni e mezzo, con un
   regime (scadenze giornaliere) che inizia a meta'.
10. **Il campione finisce a settembre 2023**: prima degli 0DTE su altre scadenze e dei cambiamenti di
    mercato successivi. **Non revisionato**; nessun conflitto dichiarato; nessun codice.
11. **Istruzioni nel PDF**: nessuna trovata.

## Cosa Dice Al Riquadro "Chi Ha Comprato Delta Oggi"

Solo descrizione, nessun segnale.

- **L'etichetta "retail" va qualificata.** Il retail riconoscibile sugli 0DTE di SPX e' il 4-6% del volume
  (risultato 3); il gruppo che OptionsDepth chiama retail (clienti non professionali del Cboe) e' piu'
  largo e comprende gli istituzionali. Il riquadro dovrebbe dire "clienti non professionali (Cboe)", non
  "retail", o spiegarlo accanto.
- **Il segno tipico non e' zero.** Gli utenti finali, nel complesso, **comprano** 0DTE e i market maker li
  vendono, per oltre l'1% del volume al giorno fra maggio 2022 e febbraio 2023 (dove finisce la Fig. 2);
  sulle altre scadenze, dopo maggio 2022, il contrario (risultato 2). Un acquisto netto di 0DTE da parte dei clienti e' quindi la normalita', non una
  notizia: conferma che il flusso va letto contro la sua storia (come dice la sintesi della ricerca).
- **Il flusso retail e' fatto in gran parte di strategie** (fino all'80% nel 2021, circa un terzo nel 2023),
  e le strategie sono vendute molto piu' spesso (risultato 8): un DEX netto mescola scommesse sul verso
  (spread) e sulla volatilita' (condor, butterfly), che hanno delta piccolo.
- **Quando**: il 61,7% delle operazioni retail 0DTE ha fra 3 e 6,5 ore a scadenza, cioe' avviene fra le 9:30
  e le 13:00 di New York (risultato 5). Ma sono 3,5 ore su 6,5: per ora fa circa il 17,6% delle operazioni,
  contro il 12,4% fra le 13:00 e le 15:00 e il 12,7% nell'ultima ora. La mattina e' **un po' piu' densa**, non
  dominante; e si contano operazioni, non delta.
- **Chi ha ragione**: niente nel paper dice che il verso del retail anticipi l'indice; l'unico indizio
  indiretto e' il lordo negativo, senza prova di significativita' (il delta firmato della Tab. 8, con gli
  effetti fissi del giorno, non misura il verso: punto 1 dei limiti). Va nel verso "nessuna informazione sul
  verso", debolmente. Converge con PP, CLZZ, MCH e con la nostra misura sulle 35 sedute.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

- **Il retail sulle opzioni di NQ, no.** ATAS non dice chi scambia, la catena CME non ha un codice d'asta
  come SLAN/MLAT, e le opzioni su NQ non sono SPX. Su NQ il DEX per gruppo di X-Ray e' solo contesto
  (CLAUDE.md, decisione 2).
- **La taglia come segno di retail** (App. B) sui future NQ con i dati di ATAS e' tecnicamente possibile, ma
  il paper stesso mostra che la taglia 1 mescola altri partecipanti, e sui future non c'e' un fondamento
  equivalente. Non lo proponiamo.
- **Sull'archivio di X-Ray (SPX)**, descrittivo: il segno medio del DEX 0DTE dei clienti non professionali,
  per vedere se ripete il "compratori netti di 0DTE" del risultato 2; e la forma oraria del flusso, per
  vedere se si accumula la mattina come il risultato 5. Poche settimane di dati, nessun backtest.
- **Dati che non abbiamo**: le transazioni Cboe con i codici OPRA, le quote dei market maker, il profilo
  Open/Close storico.

## Cosa Ne Prende Il Framework

- **Nessun finding da provare**: il paper misura il profitto del retail, non un effetto sul prezzo.
- **Una regola di lettura**: "retail" in un dato di fornitore va sempre definito (codice d'origine, codice
  d'asta, taglia), perche' le tre definizioni danno gruppi di dimensioni diverse.
- **Un metodo**: lordo al mid contro netto al prezzo pagato, per separare scelta e costi. (Il delta firmato
  in una regressione con gli effetti fissi del giorno **non** e' una prova di informazione sul verso: la
  posizione netta del giorno finisce nell'effetto fisso. Tolto in verifica.)
- **Una cautela sulle unita'**: le tabelle del paper non dichiarano sempre l'unita', e due volte (Tab. 7, e
  probabilmente Tab. 8) il testo le traduce con un fattore sbagliato; un numero preso da qui si ricontrolla
  con la costante e con l'R quadro.
- **Nessuna regola operativa.**

## La Verifica

**9 novembre 2026**, fase 4, verifica avversaria sul PDF (sha256 confermato, 50 pagine). Testo estratto con
`pdftotext -layout`; Fig. 2 (p. 14), Tab. 1, 2, 3, 4, 7, 8, D2, D3, D4 e l'eq. 3 rese come immagine con
`pypdfium2` e rilette cella per cella. **Circa 215 affermazioni controllate** (numeri, citazioni, finding):
**199 confermate**, **15 corrette o precisate**, **1 tolta**. Nessuna istruzione nel PDF.

Confermati, fra gli altri: la definizione del retail (SLAN e MLAT, asta AIM, fino a 10 contratti e la gamba
piu' piccola fino a 10, dal 22 febbraio 2021, pp. 15-16); le quote (2-4% del volume S&P 500, oltre il 6%
degli 0DTE nel 2022, circa tre quarti del volume retail negli 0DTE, p. 17); lo sbilancio dei market maker
(fra -0,5% e -1%, poi sotto -1%, segni letti sull'immagine di p. 14); le perdite (oltre 125 milioni netti,
circa 30 lordi, oltre 90 di costi, -241.000 e -57.000 al giorno, -350.000 e -106.000 dal 16 maggio 2022,
debit e credit, mesi, giorni e ore della Tab. 4); il 61,7% con 3-6,5 ore a scadenza (Tab. 1); il delta firmato
0,000 (t 0,02) nella **col. 4** della Tab. 8, con i soli gamma e vega; tutte le cifre delle Tab. 2, 3, 5, 6,
7, 8, C1, D2-D4 riportate. Confermate anche le incoerenze che il lettore aveva segnalato: "north of
$500,000" contro -241.000 e -350.000 (p. 30); ore a scadenza negative nella Tab. 8 contro il testo (p. 35);
piu' vendite che acquisti nella Tab. 1 (40,1% e 41,2%); "holding period"; \|Delta\| -0,132 nella col. 4 della
Tab. 3; Fig. 1, 3, 5; il refuso max(S - K) nell'eq. 3.

Le correzioni:

1. **Unita' della Tab. 7, deciso**: la costante (-0,241) e la deviazione standard implicita nell'R quadro
   (circa 1 unita') danno il milione; gli effetti in dollari del testo (+30.000, -13.000 / -20.000) sono dieci
   volte troppo piccoli (risultato 14, limiti punto 6). Deduzione nostra, non del paper.
2. **Il delta firmato della Tab. 8 non e' una prova sul verso**: con gli effetti fissi del giorno la posizione
   netta del giorno sparisce nella costante. Corretti il punto 1 dei limiti e "chi ha ragione"; **tolta** la
   voce di metodo in "Cosa ne prende il framework".
3. **Al denaro o poco fuori**: il "circa 75%" retail e il "65%" non retail del testo non sono nella Tab. 1
   (65,2% e 55,1% delle operazioni); il risultato 5 riportava i numeri del testo.
4. **Taglia 1**: 74,2% nell'App. B contro 62,5% nella Tab. 1 che cita (risultato 17, incoerenze).
5. **Ore a scadenza per gruppi**: nelle strategie di volatilita' (Tab. D4) il segno non e' positivo ma non
   significativo (t 1,57 al massimo); sulle singole (Tab. D2) e' negativo.
6. **Unita' della Tab. 8**: didascalia e testo sono coerenti fra loro; l'aumento dell'R quadro con la sola
   volatilita' implicita fa pensare a rendimenti in decimali, quindi effetti cento volte piu' grandi di come li
   legge il testo. Segnato come dubbio forte, non come numero.
7. **R quadro**: non "fra l'1% e il 10%", ma 0,39-1,96% (Tab. 3) e 1,09-11,85% (Tab. 8, D2-D4), in gran parte
   effetti fissi del giorno.
8. **La variabile della Tab. 3**: la didascalia dice che vale 1, non 0/100; il testo e le dimensioni dei
   coefficienti la leggono in punti percentuali.
9. **Sbilancio nel riquadro**: "oltre l'1%", non "circa l'1%", e solo fino a febbraio 2023.
10. **La mattina**: 3-6,5 ore a scadenza vuol dire 9:30-13:00, cioe' 3,5 ore su 6,5; per ora la mattina e'
    poco piu' densa (17,6% contro 12-13%), non "soprattutto".
11. **Il lordo**: nessuna prova di significativita' sulla media lorda di -57.000 dollari; positivo per mesi.
12. **L'identificazione**: aggiunto il sostegno di Bryzgalova e altri (2023), valido per le operazioni a una
    gamba del mercato piu' ampio, non per gli 0DTE di SPX ne' per le MLAT.
13. **Open/Close**: la divisione per apertura e chiusura non e' nel paper; il legame con il DEX di X-Ray e'
    nostro.
14. **Minori**: range 0,24 nel testo e 0,204 nella Tab. 2; effetti fissi del giorno della settimana solo nella
    col. 5 della Tab. 2, contro la didascalia; volume retail mensile su tutte le scadenze.
15. **App. D**: la taglia, positiva nella Tab. 8, regge solo sulle singole (D2), non negli spread ne' nella
    volatilita'.

Non verificabile dal PDF: il DOI e l'identificativo SSRN (il frontespizio non li porta; vengono dalla pagina
SSRN della fase 1).
