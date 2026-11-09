---
chiave: wang-2021-index-options-trading-and-sentiment
sigla: NSW
titolo: "Index Options Trading and Sentiment"
autori: [Sophie Xiaoyan Ni, Aristotelis Stouraitis, Li Wang]
versione_letta: working paper SSRN dell'8 dicembre 2021 (data sul frontespizio del PDF)
fonte: SSRN
identificativo: "10.2139/ssrn.3981994"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=3981994
tipo: preprint
revisione_fra_pari: no
pagine: 38 (testo alle pagine 1-22 del PDF, bibliografia 23-24, figure 25-26, tabelle 27-38)
pdf: wang-2021-index-options-trading-and-sentiment.pdf
pdf_sha256: 74e51c85f0881efa8809f9052518ca3db5d9678c2d8aaa97934c0b20ba003d39
filone: flusso-opzioni
ruolo: idea
stato: analizzato
letto_il: 2026-11-09
verificato_il: 2026-11-09
studi: []
codice_degli_autori: "nessuno; dati Cboe Open-Close (a pagamento), OptionMetrics, CRSP, Shiller, Kenneth French, sito di Hao Zhou (VRP)"
---

# Ni, Stouraitis, Wang 2021: L'Analisi

*Index Options Trading and Sentiment*, Sophie Xiaoyan Ni (Hong Kong Baptist University), Aristotelis
Stouraitis (Hong Kong Baptist University), Li Wang (Case Western Reserve, Weatherhead). Working paper SSRN
3981994, versione dell'8 dicembre 2021. **Non revisionato**: il PDF non nomina nessuna rivista. Sul
frontespizio l'ordine degli autori e' Ni, Stouraitis, Wang; la chiave della cartella usa Wang, come la
ricerca, e la sigla usa l'ordine del PDF. **La chiave non segue la regola dell'indice** ("cognome del primo
autore"): sarebbe `ni-2021-...`. Segnalato nella verifica, la cartella non e' stata rinominata. Ni e' coautrice di Chen, Joslin e Ni (2019), che il paper usa
come controllo.

Letto per intero il 9 novembre 2026. Il testo estratto con `pdftotext` e' pulito, ma **le tabelle 6, 7 e 9
escono vuote** e le altre mescolano le colonne: tutte le tabelle e le due figure sono state **guardate
come immagine** (pagine rese con `pypdfium2` nella cartella temporanea). Le pagine del testo hanno il
numero stampato uguale a quello del PDF meno uno. La numerazione delle sezioni del paper e' disordinata
(3.2, poi 3.2.1, 3.3.2, 3.3.3, e **due sezioni 3.4**): qui si cita come e' stampata, e le due 3.4 si
distinguono con "lungo periodo" e "portafogli". Citazioni `[NSW §n]`, `[NSW Tab. n]`, `[NSW Fig. n]`.
Nessuna istruzione trovata nel PDF.

## In Parole Semplici

Gli autori guardano cosa fanno i **clienti pubblici** del Cboe (chi non e' un conto proprietario di una
banca o di un intermediario) con le opzioni sull'S&P 500 **in the money** e a scadenza lunga, mese per mese
dal 1991 al 2018. Quando nel mese hanno comprato piu' call di quelle vendute (e meno put), **i tre mesi dopo
l'indice va peggio**; quando hanno fatto il contrario, va meglio. E' un segnale **al contrario e lento**: un
mese, fino a tre. Lo leggono come **sentiment** (ottimismo o pessimismo non giustificato), non come
informazione. Per il riquadro "Chi ha comprato delta oggi" dice poco di diretto: e' su mesi, su una fetta
piccola del flusso (solo in the money, solo scadenze lunghe), e i "clienti pubblici" sono soprattutto
**istituzioni**, non il retail. Dice pero' una cosa utile: nello **stesso mese** quei clienti comprano call
(piu' del solito: in media ne vendono) quando il mercato scende (Tab. 9), cioe' vanno **contro il prezzo**, come il gruppo che X-Ray chiama retail
dentro la seduta.

## I Termini

- **SPX**: le opzioni sull'indice S&P 500 quotate al Cboe, che ne ha l'esclusiva [NSW §2].
- **In the money (ITM), at the money (ATM), out of the money (OTM)**: un'opzione e' in the money se,
  esercitata ora, varrebbe qualcosa (una call con lo strike sotto il prezzo, una put con lo strike sopra).
  Il paper le separa con il rapporto strike/prezzo K/S: call ITM sotto 0,985, put ITM sopra 1,015; ATM fra
  0,985 e 1,015; il resto OTM [NSW §2, Tab. 1].
- **Clienti pubblici (public customers, codice "C")** e **conti proprietari (firm proprietary, "F")**: le
  due classi di chi scambia nel dato Cboe Open-Close come le usa il paper. I market maker non ci sono: la
  loro posizione si deduce come lato opposto [NSW §2]. Il paper **non separa** i clienti professionali e non
  parla mai di retail.
- **Open-buy, open-sell**: contratti comprati, o venduti, per **aprire** una posizione nuova.
- **PNBC, PNBP**: *public net buy* di call e di put, cioe' gli acquisti meno le vendite in apertura dei
  clienti pubblici in un mese, in contratti (eq. 1.2-1.3). Le versioni **normalizzate** (PNBCN, PNBPN) sono
  divise per il volume [NSW §3.1, Tab. 1, Tab. 3].
- **DOI** (*directional order imbalance*, sbilancio direzionale): PNBC - PNBP sulle sole opzioni ITM, call da
  1 a 12 mesi e put da 2 a 12 mesi [NSW §3.2, Tab. 3]. Comprare una call ITM o vendere una put ITM e' quasi
  come comprare l'indice; il DOI e' quindi un **acquisto netto sintetico dell'indice** da parte del pubblico.
- **PNBPO** (anche PNBOP o PNBDO nel paper): acquisti netti dei clienti pubblici di put molto fuori dal
  denaro, la misura di Chen, Joslin e Ni (2019).
- **Sentiment**: l'umore degli investitori non giustificato dai fondamentali. Gli indici usati come
  confronto: **BW** (Baker e Wurgler 2006), **HJTZ** (Huang, Jiang, Tu, Zhou 2014), **ICS** (fiducia dei
  consumatori dell'Universita' del Michigan).
- **VRP** (*variance risk premium*): la differenza fra la varianza attesa implicita nelle opzioni e quella
  realizzata; da Bollerslev, Tauchen e Zhou un buon predittore del mercato [NSW §3.3.3].
- **Newey-West con 3 ritardi**: errori standard che tengono conto della correlazione fra mesi vicini, utile
  quando i rendimenti a tre mesi si sovrappongono.
- **R quadro fuori campione**: di solito, quanto la previsione batte la media storica su un periodo non
  usato per stimarla (definizione abituale della letteratura, **non scritta nel paper**). Il paper lo segna
  con una "O" e dice solo "half-period of evaluation" [NSW Tab. 3].
- **Limiti all'arbitraggio**: le ragioni (capitale, rischio) per cui chi potrebbe correggere un prezzo
  sbagliato non lo fa subito; qui misurati con le **recessioni** NBER [NSW §3.4, lungo periodo].
- **Durbin-Watson**: un test dell'autocorrelazione di una serie; vicino a 2 vuol dire poca autocorrelazione.

## La Domanda

Perche' si scambiano le opzioni SPX **in the money**, che quasi nessuno studia, e se il loro sbilancio
dice qualcosa del mercato futuro [NSW §1]. L'ipotesi: fare trading direzionale con le SPX costa (i
premi sono cari, e il future fa lo stesso a meno), quindi chi lo fa ha una **componente irrazionale**, e lo
sbilancio dovrebbe predire il mercato **al contrario** [NSW §1, §3.1]. Le ITM servono perche' sono le meno
sensibili al VIX: chi scambia volatilita' o assicurazione contro i crolli usa le OTM [NSW §1].

## Il Metodo

**I dati** [NSW §2]:

- Cboe Open-Close, **tutte le SPX** dal 2 gennaio 1991 al 31 dicembre 2018, per serie, per classe (C o F) e
  per verso (open-buy, open-sell, close-buy, close-sell). "We will use the open trades for all analyses".
- OptionMetrics per volatilita' implicita, delta e prezzi (il prezzo e' il **medio fra denaro e lettera**,
  non quello scambiato, nota 5); CRSP per l'indice; Shiller per dividendi e utili; Kenneth French per i
  rendimenti di mercato e dei portafogli; il VRP dal sito di Hao Zhou (nota 6).
- Non tabulato: i clienti pubblici fanno **piu' di due terzi** del volume delle SPX [NSW §2]. Per la taglia
  degli scambi, "it is believed" che la maggior parte sia **istituzionale** [NSW §1, §5].

**La frequenza e il campione**: **mensile**. Le regressioni del §4 dichiarano **336 osservazioni**
(28 anni per 12 mesi) [NSW Tab. 9]. La significativita' e' quindi **per mese**, non per scambio ne' per
giorno.

**Passo 1, quali opzioni non dipendono dal VIX** [NSW §3.1, eq. 1.1, Fig. 1]: per ogni gruppo di
moneyness e scadenza, il rendimento giornaliero dell'opzione regredito sul rendimento del VIX, con il
rendimento dell'S&P 500, il theta e K/S come controlli.

```text
R_i,d = a + b * R_VIX,d + c * R_Mkt,d + d * theta_i,d-1 + e * K_i,d-1 / S_d-1 + eps_i,d     (1.1)
```

Il coefficiente b e' vicino a zero per le call ITM a ogni scadenza; per le put ITM e' piu' piccolo che per
ATM e OTM ma **non e' mai zero**: dalla figura, a occhio, circa 1,25 a scadenza zero, 0,65 a due mesi, 0,3 a
cinque e 0,2 a dodici, con bande di confidenza strette, cioe' positivo a ogni scadenza; le put ATM a dodici
mesi sono intorno a 0,3 [NSW Fig. 1]. Anche le call **ATM e OTM** oltre i due mesi stanno vicino a zero (fra
circa -0,1 e +0,1). Il testo dice le put ITM "similar" alle call "except for ... fewer than two months" [NSW
§3.1]: la figura non lo mostra (vedi "Cosa Non Regge", punto 7). Da qui la scelta: **call ITM da 1 a 12 mesi,
put ITM da 2 a 12 mesi**.

**Passo 2, la misura** [NSW §3.1, eq. 1.2-1.3; §3.2]:

```text
PNBC_t = BuyCall_t - SellCall_t      (solo aperture dei clienti pubblici, nel mese t)   (1.2)
PNBP_t = BuyPut_t  - SellPut_t                                                          (1.3)
DOI_t  = PNBC_t - PNBP_t             (ITM; normalizzato o no, vedi i dubbi)
```

**Passo 3, le regressioni predittive** [NSW §3.2.1, §3.3.3, eq. 2.1, Tab. 3, Tab. 5]: il rendimento in
eccesso del mercato (Fama-French) del mese dopo, del secondo, del terzo e dei tre mesi insieme, sul DOI di
fine mese. Ipotesi nulla beta = 0, alternativa **beta < 0** (una coda, "contrarian pattern"). Errori
Newey-West a 3 ritardi. Controlli: PNBPO, rapporto prezzo/utili, VRP, BW, HJTZ, ICS. Il testo promette anche
VIX, EPU, NVIX, avversione al rischio e incertezza di Bekaert e altri: **nella Tab. 5 non ci sono** (vedi
"Cosa Non Regge").

**Il fuori campione** [NSW §3.2.1, Tab. 3]: un R quadro "with a half-period of evaluation". Come si stima
(finestra che cresce, fissa, da quale mese) **non e' scritto**. Nella bibliografia c'e' Rapach, Strauss e Zhou
(2010), ma il testo non lo cita.

**Passo 4, le prove accessorie**: altri attivi (fondi hedge, high yield, materie prime, carry, Treasury a
10 anni, swap) [NSW §3.3.2, Tab. 4]; l'interazione con le recessioni NBER a 1, 3 e 6 mesi [NSW §3.4 lungo
periodo, eq. 2.3, Tab. 6]; i portafogli di Kenneth French per settore, taglia, book-to-market, momento,
redditivita', investimento [NSW §3.4 portafogli, eq. 3.1, Tab. 7]; il canale dei flussi di cassa o del tasso
di sconto [NSW §3.5, eq. 5.1, Tab. 8]; cosa muove il DOI [NSW §4, Tab. 9].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | **Medie mensili 1991-2018**: PNBC -7.250 contratti (mediana -4.230, deviazione standard 15.270): il pubblico **vende** call ITM. PNBP +6.560 (mediana 4.800, deviazione 10.330): **compra** put ITM. Normalizzati: PNBCN -0,13 (deviazione 0,25), PNBPN -0,37 (deviazione 0,37), in "%". Durbin-Watson 1,53 (PNBC) e 1,63 (PNBP): poca autocorrelazione mensile; delle versioni normalizzate, quelle del finding, 1,25 e 1,36 (il testo non le commenta); di PNBOP 0,42 | Tab. 1, §2 |
| 2 | **Moneyness e scadenze, rendimento dei tre mesi dopo, una variabile alla volta (normalizzate)**. Call ITM, tutte le scadenze: **-12,31 (t -3,57)**; put ITM: **+10,22 (t 2,08)**. Per scadenza, call ITM: mese corrente +5,93 (t 1,21), un mese **-16,88 (t -3,04)**, due mesi -11,80 (t -1,84), da due mesi a un anno -6,18 (t -1,18). Put ITM: mese corrente -2,42 (t -0,30), un mese 1,61 (t 0,17), due mesi **21,58 (t 2,20)**, da due mesi a un anno **12,95 (t 3,95)** | Tab. 2 |
| 3 | **Le put OTM vanno nel verso opposto**: piu' put OTM comprate, mercato **piu' basso** dopo: tutte le scadenze -1,79 (t -3,11), da due mesi a un anno -3,80 (t -2,81). ATM: niente, salvo le put da due mesi a un anno, 12,86 (t 2,57). Tutte le moneyness insieme, tutte le scadenze: solo le put, -1,17 (t -2,15); per scadenza, le put a un mese -1,56 (t -2,01) | Tab. 2 |
| 4 | **Il finding, DOI normalizzato, 1991-2018** (Newey-West 3 ritardi): mese dopo **-2,02 (t -3,03)**, R quadro 3,06%, fuori campione 4,14%; secondo mese -1,96 (t -3,02); terzo mese -1,80 (t -2,90); **tre mesi insieme -5,74 (t -3,85)**, R quadro 8,37%, fuori campione 12,83%. Non normalizzato: mese dopo -4,50 (t -4,08), R quadro 4,80%; tre mesi -12,10 (t -5,08), R quadro 11,47%, fuori campione 18,67% | Tab. 3A |
| 5 | **Le due gambe separate** (normalizzate): PNBC mese dopo -2,95 (t -3,06), tre mesi -7,97 (t -3,99); PNBP mese dopo +1,75 (t 1,79), secondo mese +2,06 (t 2,15), terzo +1,56 (t 1,76), tre mesi +5,30 (t 2,56). La gamba delle call e' la piu' forte | Tab. 3A |
| 6 | **Le due meta' del campione**, DOI normalizzato, tre mesi: 1991-2005 -5,86 (t -3,31), R quadro 10,73%, fuori campione 7,26%; 2006-2018 -8,31 (t -2,71), R quadro 8,99%, fuori campione 20,89%. Le put da sole, normalizzate, nella seconda meta': 7,07 (t 1,66), non significative; nella prima meta' fuori campione -2,11% | Tab. 3B, 3C |
| 7 | **Con i controlli** (DOI di versione non dichiarata): mese dopo -1,50 (t -2,73) con PNBPO, PE, VRP; -1,20 (t -2,27) aggiungendo BW, HJTZ, ICS. Tre mesi: -3,30 (t -2,81) e -2,40 (t -2,44). Le put OTM di Chen, Joslin e Ni restano negative (-1,39, t -2,09 al mese; -4,15, t -2,44 a tre mesi); il VRP positivo (0,05, t 4,68). BW e ICS non significativi; HJTZ -0,89 (t -1,84) al mese, -2,09 (t -1,98) a tre mesi. R quadro 0,11, 0,13, 0,19, 0,26 | Tab. 5, §3.3.3 |
| 8 | **Altri attivi**: il DOI normalizzato predice al contrario i fondi hedge (mese -0,73, t -2,67; tre mesi -2,42, t -3,43) e l'high yield (mese -1,10, t -2,29; tre mesi -2,85, t -2,35); niente su materie prime, carry, Treasury a 10 anni, swap (t fra -0,86 e 0,93) | Tab. 4, §3.3.2 |
| 9 | **Recessioni**: DOI a un mese -1,22 (t -2,49), a tre mesi -2,50 (t -2,33), a sei mesi -3,80 (stampato con t **+2,38**). Interazione DOI x recessione: -6,93 (t -2,16) a un mese, -13,16 (t -2,13) a tre, +7,69 (t 0,66) a sei. Nelle recessioni l'effetto e' diverse volte piu' grande fino a tre mesi | Tab. 6, §3.4 lungo periodo |
| 10 | **Portafogli, mese dopo, una variabile**: tecnologia -2,80 (t -3,53), R quadro 3,70%; durevoli -2,32 (t -3,42); "altro" -2,10 (t -3,83), R quadro 3,91%; telecomunicazioni -1,89 (t -3,31). Taglia: tutti e dieci significativi, dal piu' piccolo -1,31 (t -2,11) al piu' grande -1,77 (t -3,73), senza un ordine. Book-to-market: crescita -1,76 (t -3,18), valore -2,30 (t -3,29). Momento: perdenti -2,30 (t -2,45), vincenti -2,31 (t -3,44). Sulle stesse colonne BW e HJTZ sono quasi sempre piu' deboli | Tab. 7 |
| 11 | **Il canale**: il DOI predice il rapporto dividendo/prezzo del mese dopo (0,73, t 2,71: DOI alto, prezzi piu' bassi) e la crescita dei dividendi (-1,86, t -3,04) e degli utili (-0,10, t -2,03). BW e HJTZ predicono la crescita ma non il dividendo/prezzo (t 1,24 e 1,47) | Tab. 8, §3.5 |
| 12 | **Cosa muove il DOI, nello stesso mese** (336 mesi): le call ITM comprate nette **scendono quando il mercato del mese sale**: rendimento del mese -2,00 (t -2,45); il mese prima -0,75 (t -0,97). Le put ITM vendute nette (-PNBP) pure: -2,23 (t -2,49). Cioe' il pubblico compra piu' call e vende piu' put ITM, rispetto alla sua media (che e' vendere call e comprare put, Tab. 1), **nei mesi in cui il mercato scende**. La tabella non dice se PNBC e PNBP siano normalizzati ne' come sono calcolati gli errori. Con il sentiment: BW 0,07 (t 1,15), HJTZ 0,23 (t 1,37) per le call; per le put HJTZ 0,03 (t 0,54) | Tab. 9 |

**I numeri dell'effetto in parole.** Le unita' dei coefficienti non sono dichiarate (rendimenti in
percento? misura in percento o in frazione?), quindi il "quanto" si legge meglio dall'R quadro: 3,06% al mese
dopo vuol dire una correlazione di circa -0,17 fra il DOI di un mese e il rendimento del mese dopo (nostro
conto, radice di 0,0306). E' un effetto da predittore mensile del mercato, non da segnale di giornata.

## Il Perche'

**Quello degli autori** [NSW §1, §3.1, §3.4, §3.5, §5]:

- Fare trading **direzionale** con le SPX costa: i premi delle SPX sono cari (Bollen e Whaley; Garleanu,
  Pedersen, Poteshman), e il future fa lo stesso a meno. Chi lo fa comunque ha una parte **irrazionale**.
- Le ITM sono le meno sensibili al VIX, quindi il loro sbilancio misura la **direzione** e non la paura dei
  crolli o la volatilita' [NSW Fig. 1].
- La Fig. 2 mostra le call ITM nette che si muovono con il **livello** dell'indice e le put contro: gli autori
  lo leggono come **inseguire la tendenza** (*trend-chasing*) [NSW §3.1].
- Il sentiment e' persistente, e i **limiti all'arbitraggio** ritardano la correzione: per questo l'effetto
  dura mesi ed e' piu' forte nelle recessioni [NSW §3.4 lungo periodo, Tab. 6].
- Poiche' i clienti delle SPX sono per lo piu' istituzionali, il risultato direbbe che anche **istituzioni
  meno sofisticate** sono soggette a sentiment [NSW §1, §5]; i fondi comuni e i fondi pensione sono nominati come
  esempio solo nella conclusione [NSW §5].

**Le alternative che discutono.** Con Chen, Joslin e Ni (2019) il risultato e' "in stark contrast": li'
le put OTM comprate dal pubblico predicono un mercato **piu' basso**, che si legge come vincoli degli
intermediari o come investitori che sanno [NSW §1]; e il paper stesso lo ritrova (risultato 3 e 7). Il DOI
non predice gli altri attivi come fa PNBPO, quindi **non e' un fattore di rischio comune** ne' macro [NSW
§3.3.2]. Non e' la volatilita' a breve: nella Tab. 5 il VIX non c'e', ma il testo dice che non la spiega
[NSW §3.3.3].

**Cosa lo ucciderebbe**, dedotto dal perche':

- che il DOI fosse solo il **rendimento del mercato dello stesso mese** visto allo specchio: la Tab. 9 dice
  che il DOI va contro il mercato del mese, e il rendimento del mese **non e' fra i controlli** della Tab. 5
  (se il mercato del mese t predice quello del mese t+1, una parte della "previsione" sarebbe del prezzo, non
  del sentiment);
- che l'effetto non ci fosse fuori dalle recessioni (la Tab. 6 dice che c'e', piu' piccolo);
- che sparisse dopo il 2018, con le scadenze settimanali e giornaliere che hanno cambiato chi usa le SPX
  (il paper si ferma al 2018);
- che le ITM a scadenza lunga fossero comprate per altre ragioni (coperture con le put ITM, strategie a piu'
  gambe, rinnovi a scadenza): il paper non le separa.

## Cosa Non Regge O Non Si Dice

1. **Il testo e la Tab. 5 non coincidono.** Il §3.3.3 descrive quattro modelli: (1) con il solo VIX, (2) con
   PNBPO, PE e VRP, (3) con i sentiment, (4) con le variabili d'incertezza macro (EPU, NVIX, avversione al
   rischio, BEX). La Tab. 5 ha invece due modelli al mese e due a tre mesi, **senza VIX e senza nessuna
   variabile d'incertezza**. Il testo dice che la t del DOI "decreases from -3.70 to -2.10" e l'introduzione
   "range from -1.86 to -3.70": **nessuno di questi numeri e' nella Tab. 5** (le t sono -2,73, -2,27, -2,81,
   -2,44). Il testo dice il VRP "significantly negative": nella tabella e' **positivo** (0,05, t 4,68). Il
   modello (4) "significativo al 10%" ha t -2,44. La tabella stampata e il testo vengono da due versioni
   diverse dell'analisi; si citano i numeri della tabella.
2. **"Fino a nove mesi"** [NSW §5]: nessuna tabella prova i nove mesi. La Tab. 6 arriva a sei, e li' il
   coefficiente -3,80 ha la t stampata **positiva** (2,38): un errore di stampa da qualche parte.
3. **Le stelle non seguono le t.** -2,41*** con t -2,41 e 2,06*** con t 2,15 (Tab. 3A), 21,58*** con t 2,20
   (Tab. 2), -3,32 con t -1,80 senza stelle (Tab. 3A): anche con l'alternativa a una coda del §3.3.3, t 2,20
   non e' all'1%. Nella Tab. 6 il DOI ha le stelle (-1,22**, t -2,49) e l'interazione con la recessione no
   (-6,93, t -2,16; -13,16, t -2,13), e -3,80** ha la t stampata positiva. La Tab. 8 dichiara stelle "two-tailed"
   ma non ne stampa nessuna. Si leggono le t.
4. **Il fuori campione non e' descritto**, e i suoi R quadro sono spesso **piu' alti** di quelli nel
   campione (DOI tre mesi 2006-2018: 8,99% dentro, 20,89% fuori; put normalizzate 2006-2018: 3,19% dentro,
   27,86% fuori). Con un predittore stimato su meta' dati e giudicato sull'altra meta' succede di rado; senza
   sapere come e' calcolato non si puo' usare come prova. In due celle (PNBC non normalizzato, Tab. 3B e 3C) il
   secondo R quadro **non ha la "O"**.
5. **Quale DOI.** "We will use the open trades for all analyses" [§2] ed eq. 1.2-1.3 "only open
   transactions"; ma due righe dopo "our main analysis combines ... public open and close buy volume minus open
   and close sell volume" [§2]. La normalizzazione e' detta in tre modi: "total trading volume, including both
   buys and sells" [§3.1], "sum of public open buy and sell" [Tab. 3], "total open volume of the option
   contracts in this category" [Tab. 1]. Nelle Tab. 5-9 non si dice se il DOI sia normalizzato; la Tab. 6 lo
   chiama "the monthly public net buying call volume", cioe' solo le call.
6. **La moneyness e' definita in due modi**: con il prezzo di chiusura dell'indice [§2, eq. 1.1] e con il
   **forward** [Tab. 2, Fig. 2]. La didascalia della Fig. 2 ha il verso sbagliato (call ITM con lo strike
   "1,5% higher" del forward). La banda fissa dell'1,5% non cambia con la scadenza: a dodici mesi una call
   all'1,6% sotto il prezzo e' appena in the money (osservazione nostra).
7. **Le scadenze sono scelte guardando i risultati.** La Tab. 2 prova 40 coefficienti (4 gruppi di
   moneyness per 5 di scadenza, call e put) sul rendimento dei tre mesi dopo, la stessa variabile del finding,
   e la finestra scelta (call 1-12 mesi, put 2-12) e' quella dove le ITM risultano significative. La
   giustificazione dichiarata e' la Fig. 1 (sensibilita' al VIX), ma la scelta coincide con la Tab. 2. E nella
   Tab. 2 le call ITM da due mesi a un anno **non** sono significative (-6,18, t -1,18), contro il testo che le
   dice "stable" fra un mese e un anno [§3.1]. Nessuna correzione per prove multiple. E la Fig. 1 regge la
   scelta meno di quanto dica il testo (lettura a occhio): oltre i due mesi anche le call ATM e OTM hanno b
   vicino a zero, quindi per le call il vantaggio delle ITM e' solo sotto i due mesi; le put ITM hanno b
   positivo a ogni scadenza (circa 0,65 a due mesi, 0,2 a dodici), non "simile" alle call oltre i due mesi.
8. **Il rendimento dello stesso mese non e' un controllo.** La Tab. 9 mostra che il DOI va contro il mercato
   del mese (t -2,45 per le call, -2,49 per le put), e questo **contraddice la lettura "trend-chasing"** che
   gli autori danno della Fig. 2: la figura confronta il flusso con il **livello** di un indice che sale per 28
   anni, la tabella con il **rendimento**, e la tabella dice il contrario. Il paper non commenta il segno.
9. **Le osservazioni sono mesi e i rendimenti si sovrappongono.** A tre mesi le finestre si accavallano di
   due mesi; Newey-West a 3 ritardi basta appena. Newey-West e' dichiarato solo nelle Tab. 3, 4 e 5: nelle
   Tab. 6 (a nessun orizzonte, e a sei mesi le finestre si accavallano di cinque), 7, 8 e 9 il metodo degli
   errori non e' scritto.
   La significativita' e' per mese, come si deve per un predittore mensile, ma 336 mesi sono pochi per molte
   specifiche.
10. **Le recessioni sono poche.** Il paper non dice quanti mesi la variabile vale uno; nel 1991-2018 le
    recessioni NBER sono poche e corte, e un'interazione stimata su pochi mesi e' fragile. La definizione,
    "at least half of the prediction period" (§3.4 e Tab. 6), e l'esempio a sei mesi, "more than two out of
    six", coincidono (tre su sei e' la meta'); a un mese la variabile vale uno se quel mese e' in recessione.
11. **Errori di tabella.** La Tab. 1 da' per PNBP una media positiva (+6,56 migliaia) e per PNBPN una
    negativa (-0,37): possibile solo con pesi molto diversi fra i mesi, e il paper non lo spiega. La Fig. 2
    mostra PNBP oltre 100.000 contratti in un mese, la Tab. 1 un massimo di 52.970. I pannelli E e F della
    Tab. 7 hanno le etichette di C e D ("Growth/Value", "Loser/Winner") per redditivita' e investimento. La
    Tab. 7 e' "univariate" nella didascalia e "controlling for Carhart four-factor" nel testo [§1, §3.4
    portafogli]. La Tab. 9 chiama "independent variables" le variabili dipendenti. Il campione della Fig. 1 e'
    "1991 to 2018" nella figura e "1996 to 2018" nel testo (OptionMetrics). Gli R quadro sono in percento
    nelle Tab. 3, 6, 7 e 8; nelle Tab. 5 (0,11-0,26) e 9 (0,01-0,30) sembrano in frazione, ma non e' dichiarato.
    Altre etichette sbagliate: il §3.1 chiama "Table 3" la tabella delle moneyness, che e' la Tab. 2; il testo
    dopo l'eq. 1.2 dice "In equation (1.1)"; la Tab. 8 chiama il DOI "PNBAC"; la didascalia della Tab. 6 dice
    "over one and three months" e la tabella arriva a sei; la didascalia della Tab. 3 chiama "DOI" il PNBC.
12. **Contraddizioni interne del testo.** L'introduzione dice che sul settore delle telecomunicazioni non si
    trova l'effetto, ma la Tab. 7 lo da' significativo (-1,89, t -3,31). Il §4 dice le put ITM vendute "positively
    correlated with the HJTZ" "to some extent": t 0,54.
13. **I gruppi.** Solo due classi, C e F; nessuna separazione dei clienti professionali e nessuna taglia.
    "Clienti pubblici" qui vuol dire **istituzioni in maggioranza**, per ammissione degli autori.
14. **Costi, strategia, nullo.** Nessuna strategia, nessun costo, nessun confronto con un modello nullo nel
    nostro senso (lo stesso predittore su dati rimescolati). Il confronto e' con zero.
15. **Il campione finisce nel 2018**: prima delle scadenze giornaliere quotidiane (0DTE) e della crescita del
    retail sulle opzioni. Il paper non dice niente sulle scadenze brevi, salvo che le ITM in scadenza nel mese
    stesso non predicono (Tab. 2, t 1,21 e -0,30).
16. **Non revisionato**, e con le incoerenze sopra: da trattare come un working paper in bozza.

## Il Confronto Con Gli Altri Paper Della Ricerca

- **Con CLZZ** (Cao e altri): sulle opzioni **SPX** CLZZ non trova niente al giorno e alla settimana, e al
  contrario solo a **due mesi** (-0,583, t -2,74, dalla sintesi della ricerca). NSW trova al contrario da uno
  a tre mesi, ma su una fetta (ITM, scadenze lunghe). I due vanno nello stesso verso: **al contrario e
  lento**. Le misure sono diverse (CLZZ tutte le moneyness, NSW solo ITM).
- **Con PP** (Pan e Poteshman): su SPX nessuna classe predice il giorno dopo; NSW non guarda il giorno.
  Coerenti.
- **Con CKMS** (Chordia e altri): le opzioni SPX non predicono a settimane (0,02, errore 0,30); NSW non prova le
  settimane. Sulle **put** il verso si contraddice: in CKMS piu' put comprate dai piccoli, poi indice **piu'
  alto** (al contrario, indici ISE); in NSW piu' put **OTM** comprate, poi indice **piu' basso** (nel verso,
  Tab. 2 e Tab. 5, come Chen, Joslin e Ni), e piu' put **ITM** comprate, poi indice **piu' alto** (al
  contrario). Il verso cambia con la moneyness, e nessuno dei due paper separa i gruppi nello stesso modo.
- **Con la nostra misura sull'archivio di X-Ray**: dentro la seduta il flusso dei clienti non professionali va
  **contro** il prezzo in 33 sedute su 35. NSW trova la stessa cosa **su mesi** (Tab. 9): i clienti pubblici
  comprano piu' call ITM del solito nei mesi in cui il mercato scende. E' un comportamento contemporaneo, non una previsione.

## Cosa Si Puo' Rifare Su NQ, E Cosa No

- **Il DOI su NQ, no.** Serve il flusso per classe e per apertura/chiusura sulle opzioni del Nasdaq-100
  (Cboe Open-Close di NDX, o dell'ISE): non lo abbiamo, e la catena CME di ATAS non dice chi ha scambiato
  (CLAUDE.md, decisione 2).
- **Il DOI su SPX con X-Ray, no come prova.** Il DEX per gruppo di OptionsDepth e' di SPX, e su NQ e' solo
  contesto. E l'orizzonte del paper e' il **mese**: per arrivare a una t intorno a 2 servirebbero, a parita'
  di effetto, circa 150 mesi (nostro conto: t 3,03 su 336 mesi, la t cresce con la radice del numero di
  mesi), cioe' oltre dodici anni d'archivio. L'archivio di X-Ray ha 35 sedute, meno di due mesi.
- **Con le sole barre di ATAS**: la parte del "cosa lo ucciderebbe" che non richiede opzioni, cioe' se il
  rendimento mensile di NQ predice quello del mese dopo, si potrebbe misurare con le barre dal 2017, ma sono
  circa 90 mesi dentro il campione (aprile 2017 - dicembre 2024, nostro conto) e la domanda non e' quella del
  paper. Non e' un'idea da aprire.
- **Da verificare in OptionsDepth**: se il DEX per gruppo si possa dividere per **moneyness** (ITM, ATM, OTM)
  e per **call e put**. E' la divisione su cui NSW trova segni opposti.

## Per Il Riquadro "Chi Ha Comprato Delta Oggi"

Solo descrizione, nessun segnale. Cosa dice NSW, e cosa no.

- **Non sostiene una lettura "al contrario" del riquadro.** Il segnale di NSW e' **mensile**, solo **ITM**,
  solo **scadenze da 1-2 a 12 mesi**, solo **aperture**, e predice da uno a tre mesi. Il riquadro somma il
  delta di un giorno, tutte le moneyness, 0DTE e tutte le scadenze, aperture e chiusure. Le ITM che scadono
  nel mese stesso, le piu' vicine agli 0DTE, in NSW non predicono niente (Tab. 2, t 1,21 e -0,30). Il "clienti
  contrarian" di NSW non si trasferisce al minuto.
- **Conferma l'etichetta**: i clienti pubblici delle SPX sono in maggioranza istituzioni, per gli autori [§1,
  §5], come per ADEFOV e CLZZ. Il gruppo che X-Ray chiama "Retail" e' una parte dei clienti del Cboe, non il
  retail.
- **Conferma che il flusso va contro il prezzo nello stesso periodo** (Tab. 9, su mesi), come la nostra
  misura dentro la seduta: e' una ragione in piu' per mostrare la **sorpresa oltre il prezzo** accanto al
  flusso grezzo. E mostra il rischio che il riquadro gia' evita: una previsione "al contrario" puo' essere in
  parte il prezzo visto allo specchio, e NSW non lo controlla.
- **Aggiunge: la moneyness cambia il segno.** Le put OTM comprate vanno con un mercato piu' basso dopo, le put
  ITM comprate con un mercato piu' alto (Tab. 2). Il DEX pesa per il delta, e le ITM hanno delta grande: in un
  DEX netto il flusso ITM pesa piu' di quello OTM a parita' di contratti (nostra deduzione). Un netto che somma
  tutto mescola cose che in NSW vanno in versi opposti. Se OptionsDepth desse la moneyness, una descrizione
  separata sarebbe piu' fedele ai paper (**da verificare**).
- **Il segno tipico non e' zero**: in media il pubblico vende call ITM e compra put ITM (Tab. 1), cioe' e'
  corto di delta sulle ITM. Come per gli altri paper, il flusso si legge **contro la sua storia**, non contro
  zero: il riquadro lo fa gia' con il percentile.

## Cosa Ne Prende Il Framework

- **Nessuna idea da aprire su NQ**: il dato per gruppo non c'e', e l'orizzonte mensile chiede anni.
- **Un caso di scuola per il metodo**: una tabella che non coincide con il testo, un fuori campione non
  descritto con R quadro piu' alti di quelli nel campione, le scadenze scelte sulla stessa tabella che poi si
  usa come prova, le stelle che non seguono le t. Sono i controlli che la fase 4 e la nostra validazione fanno
  sempre: si cita la tabella, mai il testo che la riassume.
- **Il controllo che manca**: quando un flusso "predice al contrario", va messo nella stessa regressione il
  **rendimento del periodo** in cui il flusso si e' formato. Il paper non lo fa; il riquadro, con la sorpresa
  oltre il prezzo stimata solo sulle sedute passate, si'.

## I Dubbi Per La Verifica

- **Tab. 3** ricostruita dall'immagine della pagina 29: l'ordine delle colonne (PNBC, PNBP, DOI; normalizzati
  a sinistra, non normalizzati a destra) e delle righe (mese 1, 2, 3, tre mesi) va ricontrollato cella per
  cella, compresi gli R quadro con e senza "O".
- **Tab. 2** dall'immagine della pagina 28: l'attribuzione delle scadenze (T=0, T=1, T=2, 2 mesi < T < 1 anno).
- **Tab. 5**: la colonna di ogni controllo (PE, VRP, BW, HJTZ, ICS) e le righe vuote dei modelli (1) e (3).
- **Tab. 6**: il segno della t a sei mesi (2,38 stampato positivo accanto a -3,80).
- **Tab. 9**: il verso della lettura del rendimento del mese (Panel A sulle call nette comprate, Panel B sulle
  put nette **vendute**, -PNBP); la mia lettura e' "il pubblico compra call e vende put ITM nei mesi in cui il
  mercato scende".
- **Fig. 1**: i valori di b per le put ITM (circa 1,2 e 0,2) sono letti a occhio dalla figura.
- **Le unita'** di PNBCN e PNBPN ("%" con deviazione standard 0,25 e 0,37) e dei coefficienti non sono
  dichiarate; la correlazione -0,17 e' un nostro conto dall'R quadro 3,06%.
- **I conti nostri**: circa 150 mesi per t 2 e circa 90 mesi di NQ dentro il campione.

## La Verifica

**9 novembre 2026**, fase 4, verifica avversaria fatta da un agente diverso da quello che ha scritto l'analisi.
Fonte: il PDF (sha256 `74e51c85...0ba003d39`, ricalcolato e uguale alla scheda), testo con `pdftotext -layout`,
**tutte le tabelle (1-9) e le due figure rese come immagine** con `pypdfium2` (installato in un ambiente della
cartella temporanea, non nel Python di sistema) e lette cella per cella; la Fig. 1 anche ingrandita.

**Controllate circa 230 affermazioni**: circa 170 numeri delle Tab. 1-9 (tutte le celle citate, coefficienti, t,
R quadro con e senza "O", stelle), le citazioni dal testo (§1, §2, §3.1, §3.2.1, §3.3.2, §3.3.3, le due §3.4,
§3.5, §4, §5) e la scheda bibliografica. **Confermate** tutte le celle delle Tab. 2, 3 (A, B, C), 4, 5, 6, 7, 8 e
9; **corrette 11** affermazioni; **tolte 0**.

Confermato in particolare: Tab. 3 (DOI normalizzato -2,02, t -3,03, R quadro 3,06%; tre mesi -5,74, t -3,85; le
due meta' -5,86, t -3,31 e -8,31, t -2,71; le due celle senza "O"); Tab. 5 (t del DOI -2,73, -2,27, -2,81,
-2,44; niente VIX ne' incertezza macro; VRP positivo, t 4,68); il testo con "-3.70 to -2.10", "-1.86 to -3.70",
VRP "significantly negative", "up to nine months" (solo §5, nessuna tabella); la t +2,38 accanto a -3,80** nella
Tab. 6; le stelle che non seguono le t nelle Tab. 2 e 3; il verso della Tab. 9 (Panel B e' su -PNBP, "net sales
of puts": il coefficiente negativo sul rendimento del mese vuol dire piu' put vendute nei mesi in cui il mercato
scende, e la lettura dell'analisi e' giusta); i dati (Cboe Open-Close dal 2 gennaio 1991 al 31 dicembre 2018,
due classi C e F, 336 osservazioni, Newey-West a 3 ritardi nelle Tab. 3, 4, 5); l'ordine degli autori sul
frontespizio (Ni, Stouraitis, Wang) e la sigla NSW; nessuna menzione di retail ne' di clienti professionali.
I **conti nostri** sono rifatti e restano scritti come nostri: radice di 0,0306 = 0,175 (correlazione circa
-0,17); 336 x (2 / 3,03)^2 = 146 mesi (circa 150); aprile 2017 - dicembre 2024 = 93 mesi (circa 90).

Le correzioni:

1. **Fig. 1, put ITM** (la piu' grave): l'analisi diceva b "non zero sotto i due mesi", come se oltre fosse
   zero. Dalla figura b e' positivo a ogni scadenza (circa 0,65 a due mesi, 0,2 a dodici, bande strette), e oltre
   i due mesi anche le call ATM e OTM sono vicine a zero: la figura regge la scelta delle scadenze meno di quanto
   dica il testo. Corretti il Passo 1 e il punto 7 di "Cosa Non Regge" (lettura a occhio, dichiarata).
2. **La chiave** `wang-2021-...` non segue la regola dell'indice (cognome del primo autore, che e' Ni): segnalato
   in testa, cartella non rinominata. Nell'indice l'ordine degli autori era "Wang, Ni, Stouraitis": corretto in
   Ni, Stouraitis, Wang.
3. **Recessioni**: le due formulazioni ("at least half", "more than two out of six") coincidono; l'analisi le
   presentava come diverse. Corretto il punto 10.
4. **Errori standard**: Newey-West e' dichiarato solo nelle Tab. 3, 4, 5; nella Tab. 6 manca a ogni orizzonte, non
   solo a sei mesi, e manca nelle Tab. 7, 8, 9. Corretto il punto 9.
5. **Stelle**: aggiunte la Tab. 6 (interazione senza stelle con t -2,16 e -2,13) e la Tab. 8 (stelle dichiarate,
   nessuna stampata). Punto 3.
6. **Etichette sbagliate del paper** non elencate: "Table 3" per la Tab. 2 (§3.1), "equation (1.1)" per la 1.2,
   "PNBAC" (Tab. 8), "one and three months" (Tab. 6), "DOI" per il PNBC (didascalia Tab. 3); unita' degli R
   quadro delle Tab. 5 e 9 non dichiarate (l'analisi dava la Tab. 5 "in frazione" come certo). Punto 11.
7. **Tab. 9 e "In Parole Semplici"**: "il pubblico compra call" diventa "piu' call del solito", perche' in media
   il pubblico vende call ITM (Tab. 1); aggiunto che la Tab. 9 non dichiara normalizzazione ne' errori.
8. **R quadro fuori campione**: la definizione ("batte la media storica") e' quella abituale, non e' nel paper;
   ora e' detto.
9. **Fondi comuni e pensione**: nominati solo nel §5, non nel §1.
10. **Tab. 1**: aggiunti i Durbin-Watson delle misure normalizzate (1,25 e 1,36), che sono quelle del finding.
11. **Tab. 2**: aggiunta la put "tutte le moneyness" a un mese, -1,56 (t -2,01), anch'essa significativa.
Annotato, non corretto: il confronto con CLZZ e CKMS usa numeri di altri paper, non verificabili su questo PDF;
il frontespizio e' la pagina 1 del PDF e porta anch'esso il numero 1, quindi la regola "stampato = PDF meno
uno" vale dalla pagina 2.

**Resta incerto**: i valori della Fig. 1 sono letti a occhio; le unita' dei coefficienti e di PNBCN e PNBPN
("%"); quale DOI (normalizzato o no, solo aperture o aperture e chiusure) entri nelle Tab. 5-9; come e'
calcolato il fuori campione. Nessuna istruzione trovata nel PDF.
