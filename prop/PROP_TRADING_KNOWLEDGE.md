# Prop Trading & Order Flow — Project Knowledge

Snapshot operativo: 2026-09-01.

> Questo file riassume le decisioni e le convenzioni discusse per l'uso di ATAS con prop firm futures/crypto. Non promuove il modello a strategia validata del progetto: il repository resta osservativo finche' i dati non giustificano un modello attivo. Le regole delle prop cambiano spesso e vanno riverificate prima di acquistare o fare payout.

## 1. Stile di trading

**Low-timeframe order flow scalping: key levels, delta/VA shifts, tape confirmation, pullback entries.**

Focus: hyperscalping/intraday, barre **40 Range**, Value Area per candela, Delta/auction control, Big Trades, Speed of Tape, entry limit su pullback, SL tecnico, target base **1:1**.

## 2. Modello operativo di riferimento

La fonte di verita' del modello e' il dossier in [`prop_firm_model/`](prop_firm_model/), *The Prop Firm Model* di Fabio 'Fabervaale' Valentini. La sua trascrizione, con le convenzioni che lo rendono misurabile e le applicazioni su dati reali, e' in [`docs/research/modelli/modello-40r-riferimento.md`](../docs/research/modelli/modello-40r-riferimento.md).

La sintesi che stava qui divergeva dal dossier su tre punti e ha prodotto un replay sbagliato: aveva promosso i livelli e i Big Trades a condizioni obbligatorie, e ignorava la finestra oraria. Le tre condizioni reali sono **auction flip, value area shift e side control in tempo reale**.

E' una cosa a se' rispetto a questo documento: descrive come leggere il flusso, non quale prop usare. Le regole delle prop cambiano spesso; il modello no, e tenerli insieme faceva sembrare che una revisione dell'una implicasse una revisione dell'altro.

Quanto resta qui riguarda l'**esecuzione** di quel modello dentro i vincoli di una prop: mercato guida, sizing, fee, drawdown.

Sulla settimana 2026-09-04 / 09-11, con tutte e tre le condizioni applicate e l'esito risolto sul tape, il modello ha fatto 133 operazioni al **40%** con obiettivo 1:1, cioe' **-27 R lordi**. Il pareggio lordo richiede piu' del 50%.

A questa scala anche il costo di transazione conta, perche' il rischio mediano e' cinque punti, ma su questa settimana la perdita e' precedente alle fee. Cinque sessioni non sono un campione: la conclusione utile e' che non c'e' traccia di margine da cui partire per scegliere una prop.

## 3. Principio chiave: mercato guida vs mercato di esecuzione

Per l'order flow usare il mercato con microstruttura piu' informativa. Se la prop non esegue sullo stesso mercato, usare ATAS come **signal-generation terminal**, non assumere equivalenza tick-per-tick.

| Esecuzione prop | Mercato guida ATAS | Uso |
|---|---|---|
| NDX-USD / XYZ100 synthetic-perp | **NQ CME via Rithmic** | Delta, footprint, VA, Big Trades, tape, DOM |
| XAU-USD synthetic-perp | **GC COMEX via Rithmic** | Order flow istituzionale dell'oro |
| BTC-USD / BTC perp | **BTCUSDT Binance Futures** | Order flow crypto ad alta partecipazione |
| Prop futures nativa NQ | **NQ CME** | Analisi ed execution sullo stesso mercato |
| Prop futures nativa GC | **GC COMEX** | Analisi ed execution sullo stesso mercato |

Regola: **il segnale puo' venire dal mercato guida; entry/SL/TP numerici vanno tradotti sul prezzo effettivo dello strumento eseguito quando i due mercati non coincidono.**

## 4. Strumenti ATAS scelti

### Nasdaq
- Per NDX-USD/XYZ100 usare **NQ front-month CME via Rithmic**.
- Non serve trovare NDX cash in ATAS per l'analisi volumetrica.
- NQ offre volume centralizzato, Bid/Ask, Delta, footprint, Big Trades, Speed of Tape e DOM.

### Gold
- Per XAU-USD usare **GC front-month COMEX via Rithmic**.
- Scegliere sempre la scadenza con maggiore volume; al 2026-09-01 il riferimento discusso era **GCZ26 (Dec 2026)**.
- Il prezzo GC puo' differire da XAU spot/perp per il futures basis.

### Bitcoin
- In ATAS scegliere **BTCUSDT — BinanceFutures**, non Binance spot.
- Preferito per Delta/footprint/Big Trades/tape rispetto allo spot.

## 5. Sizing: formule canoniche

Il rischio si decide dallo **stop tecnico**, non dal notional desiderato.

```text
Quantita' = Rischio_$ / Distanza_stop
Notional_$ = Quantita' * Prezzo_entry
Margine = Notional_$ / Leva
```

La leva modifica il margine richiesto, **non** il PnL per punto/unita'.

### NDX-USD
Assunzione operativa verificata dai fill demo: circa **$1 per punto per 1 unita' NDX**.

- 1 MNQ = **$2/punto** -> ~**2 unita' NDX**.
- 1 NQ = **$20/punto** -> ~**20 unita' NDX**.
- Notional equivalente = unita' NDX x prezzo NDX.

Esempio a NDX 29,200:
- 1 MNQ ~ 2 NDX ~ **$58,400 notional**;
- 1 NQ ~ 20 NDX ~ **$584,000 notional**.

Su account 25k con leva 5x, max notional ~125k -> ~4.28 NDX a 29,200 -> ~**$4.28/punto**, circa **2.14 MNQ**.

### XAU-USD
```text
Quantita' XAU = Rischio_$ / Stop_$per_oncia
Notional = Quantita' XAU * Prezzo XAU
```

Esempio: rischio $250, stop $10 -> 25 XAU; notional = 25 x prezzo XAU.

### BTC
```text
Quantita' BTC = Rischio_$ / Stop_$per_BTC
Notional = Quantita' BTC * Prezzo BTC
```

Esempio: rischio $250, stop $200 -> 1.25 BTC.

## 6. Fee: evidenza pratica e impatto

### DojiFunded — fill demo osservati
Valori empirici da singoli trade, **non tariffario universale**:
- BTC: ~$50 fee su ~$100k notional -> ~**0.05% round-trip**;
- XAU: ~$12 su ~$100k -> ~**0.012% round-trip**;
- NDX: ~$6.47 su ~$46.23k -> ~**0.014% round-trip**.

Conclusione: su un modello lordo 1:1, le fee possono trasformare significativamente il rapporto netto; vanno incluse nel risk/reward e nel break-even.

### Breakout
Snapshot ufficiale discusso:
- trading fee ~**0.04% per lato** -> ~0.08% round-trip;
- swap/funding applicabile alle posizioni mantenute.

Per hyperscalping stretto, il costo percentuale sul notional e' una variabile primaria.

## 7. COT Gold: uso istituzionale

Per contesto istituzionale settimanale preferire **COT Disaggregated** al Legacy quando disponibile.

Interpretazione:
- **Managed Money**: proxy migliore per capitale speculativo istituzionale direzionale;
- **Producer/Merchant / Commercial**: soprattutto hedging; non usare semplicemente "Commercial short = short Gold";
- **Open Interest**: utile per distinguere nuova partecipazione da semplice covering.

Il COT definisce **bias/contesto**, non l'entry. L'entry resta sul flow GC in ATAS.

## 8. Tipi di prop: distinzione fondamentale

### Futures-native
Eseguono futures CME/CBOT/NYMEX/COMEX (spesso SIM funded, talvolta con path Live). Per il nostro stile sono preferibili quando vogliamo:

**NQ ATAS -> NQ execution** oppure **GC ATAS -> GC execution**.

### Synthetic / perpetual / tracking
NDX-USD, XAU-USD, XYZ100, ecc. possono seguire Nasdaq/Gold ma **non sono NQ/GC CME**. ATAS resta mercato guida cross-market.

### Funded != necessariamente live
Molte prop chiamano "funded" un account SIM/performance account. Distinguere sempre:
- Evaluation SIM;
- Funded/Performance SIM;
- vero **Live brokerage account**.

## 9. Prop studiate: snapshot operativo

### DojiFunded
- multi-asset/on-chain style;
- strumenti usati/discussi: **BTC-USD, NDX-USD, XAU-USD**;
- per NDX usare NQ ATAS; per XAU usare GC ATAS; per BTC usare BTCUSDT Binance Futures;
- fee reali vanno misurate per asset dai fill, perche' nei test BTC/XAU/NDX differivano molto.

### Breakout
- non e' una prop futures CME;
- XYZ100/S&P500/CL/Silver sono tracking instruments, non contratti NQ/ES/CL/SI;
- pricing esterno/crypto-native; funded basato su performance SIM con routing/hedging discrezionale;
- payout 80-90% secondo piano; regole e fee da riverificare prima dell'uso.

### Propr
- on-chain prop su Hyperliquid; account SIM/performance con A-book/B-book discrezionale;
- esempio studiato **Turbo 5K**: target $450, static DD $150 -> **Difficulty Ratio = 3.0**;
- leggere un 5K Turbo come **$150 di vero risk buffer**, non come $5,000 disponibili da perdere;
- fee Hyperliquid/funding incidono sul modello 1:1.

### Futures prop piu' allineate ad ATAS/order flow
Snapshot della ricerca 2026, da riverificare prima dell'acquisto:
- **Lucid Trading**: ATAS via Rithmic ufficiale; EOD trailing; path LucidLive; attenzione microscalping se >50% profit da trade <=5s.
- **MyFundedFutures**: ATAS ufficiale via dxFeed Prop; Builder 25K emerso come piano interessante (EOD, no DLL/consistency nell'eval al momento studiato).
- **Apex Trader Funding**: ATAS + Rithmic ufficiali; L2 disponibile; regole payout/consistency da verificare per piano.
- **TakeProfitTrader**: ATAS + Rithmic ufficiali; PRO SIM -> PRO+ live path.
- **Tradeify**: Rithmic disponibile ma attenzione a regola durata trade/payout (>10s) e disponibilita' depth; classificazione **CAUTION** per hyperscalping puro.
- **Topstep**: oggi execution centrata su TopstepX; non scelta primaria se serve ATAS per execution diretta.

Criteri preferiti per il nostro stile:
1. futures reali NQ/GC;
2. ATAS/Rithmic o feed ATAS ufficialmente supportato;
3. static o EOD drawdown;
4. basse commissioni;
5. no consistency o consistency minima;
6. nessuna restrizione incompatibile con trade brevi;
7. payout semplice;
8. path Live reale, se disponibile.

## 10. Metriche per confrontare una prop

Non confrontare i piani solo per nominal account size.

```text
True Risk Buffer = Starting Balance - Max Loss Floor
Difficulty Ratio = Profit Target / True Risk Buffer
```

Piu' basso e' il Difficulty Ratio, piu' favorevole e' il rapporto target/buffer, a parita' delle altre regole.

Verificare sempre separatamente:
- Evaluation vs Funded;
- daily loss vs max drawdown;
- static vs EOD trailing vs intraday trailing;
- balance vs equity breach;
- consistency in eval vs funded/payout;
- min trading days vs profitable days;
- payout availability vs payout eligibility;
- SIM funded vs Live funded;
- restrizioni su scalping, HFT, tick-sniping, trade <5s/<10s;
- commissioni e data fees.

## 11. Ricerca/reputazione prop su X

Per discovery usare prima conversazioni organiche di trader, poi verificare le regole sui siti ufficiali.

Separare:
- utente organico;
- affiliato/referral;
- account ufficiale della prop.

Non accettare ranking "sentiment" senza dataset verificabile. Per una ricerca auditabile richiedere almeno:
- URL del post X;
- handle;
- data;
- prop citata;
- sintesi del contenuto;
- positivo/negativo/neutro;
- affiliate/referral si'/no;
- payout proof/denial si'/no.

Solo dopo aggregare sentiment e payout trust.

## 12. Regola operativa finale

Quando la prop e' futures-native e supporta il nostro feed:

**ATAS NQ/GC -> stesso future -> execution.**

Quando la prop usa synthetic/perp:

**ATAS NQ/GC/BTC futures -> segnale microstrutturale -> traduzione su NDX/XAU/BTC della prop -> sizing dallo stop effettivo -> execution.**

Il modello di riferimento deve essere valutato **netto di fee, slippage e regole di drawdown**, non solo sul R:R lordo 1:1.
