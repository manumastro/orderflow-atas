# Studio completo — Fabio Valentino Modello 2 Mean Reversion

**Data:** 2026-06-15  
**Fonte:** transcript *Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt*

> Stato implementazione indicatore ATAS: vedi [AGENTS.md](../../AGENTS.md)

---

## 1. Tesi corretta del Modello 2

Il Modello 2 **non** è un generico indicatore di wick, CVD divergence o delta.

È un modello di **mean reversion** dentro/attorno a una **zona di balance**, basato su Auction Market Theory:

1. Il mercato è in consolidamento / balance
2. Il Volume Profile definisce dove il mercato sta transando valore
3. Il prezzo prova a uscire dal balance
4. Il breakout fallisce o viene riassorbito
5. I trader intrappolati devono chiudere
6. Il prezzo torna verso la zona dove avviene il bulk delle transazioni, soprattutto **POC**

**Frase chiave dal transcript:**

> *"Model 2 is mean reverting... market state is consolidation... we are trying to take the out of balance condition that gets back inside balance."*

Quindi il segnale **non nasce dalla candela isolata**. Nasce dal rapporto fra:

- profile / value area
- breakout o fakeout
- order flow eseguito
- assorbimento / no follow-through
- ritorno nel balance
- target POC

---

## 2. Cosa Fabio considera davvero per il Modello 2

### 2.1 Market state: consolidation / balance

Il primo filtro è lo **stato del mercato**.

Fabio distingue sempre:

- **balance**
- **out of balance**

Per il Modello 2 vuole il balance/consolidamento:

> *"The model 2 is using the market state that is the opposite... consolidation."*

Non si dovrebbe tradare mean reversion se il mercato è già in forte trend/out-of-balance. In quel caso è **Modello 1** o **no-trade**.

### 2.2 Volume Profile

Il Volume Profile è centrale anche nel Modello 2.

Serve a identificare:

- area di balance
- Value Area High
- Value Area Low
- POC
- Low Volume Node
- aree di aggressione/delta per prezzo

**Frasi chiave:**

> *"Market state is consolidation and is when the profile is protecting from breaking here and breaking here."*

> *"You go to where the bulk of the auctions taking place."*

> *"This blue line is the value area."*

> *"This is the value area high. This is the value area low. This is the POC."*

### 2.3 Target: POC / bulk of auction

Il target **non** è automaticamente l'altro lato del range.

Il target a più alta probabilità è:

- **POC**
- bulk of auction
- area dove si è transato più volume

**Frase chiave:**

> *"We don't go here... you go to where the bulk of the auction is taking place."*

Quindi per il Modello 2 l'output deve mostrare sempre:

- target POC
- distanza dal POC
- se il POC è coerente con la direzione del trade

### 2.4 Non prendere il primo breakout

Fabio lo ripete più volte:

> *"We are not trying to take the first swing because it's risky. We are getting the second swing."*

> *"Wait for the second drive. Don't take the first drive because you can get trapped in a fakeout."*

Quindi un breakout fuori VAH/VAL **non è trigger**.

È solo evento di contesto:

- primo drive
- possibile fakeout
- possibile accumulo di trapped traders

### 2.5 Trigger: big trades / aggression / bubbles

Il trigger vero è **order flow eseguito**, non previsione.

Fabio dice:

> *"I wait for big trades."*

> *"I jump in when the best ones are jumping in with big volume."*

> *"You want to filter out the noise. You want to see what actual big orders are doing."*

**Valori citati:**

| Sessione | Soglia contratti |
|----------|------------------|
| New York / NQ | ~30 |
| London | ~20 |
| Filtro alto (chiarezza) | ~40 |

Importante: Fabio guarda **market executions / bubbles**, non ordini limite:

> *"I never watch limits... with execution these are executed market."*

### 2.6 Assorbimento e no follow-through

Il concetto non è solo "delta alto".

È:

1. Una parte aggredisce
2. Ma il prezzo non prosegue
3. O recupera il livello
4. Quindi quella parte è **trapped**

**Frasi chiave:**

> *"Aggressive sellers without follow up."*

> *"They are punching a wall."*

> *"A lot of aggression and no follow up."*

> *"This got absorbed."*

Perciò l'input corretto non è solo `barDelta`, ma relazione fra:

- big sell/buy executions
- prezzo che non segue
- recupero inside value/range
- livello profile rilevante

### 2.7 Squeeze

Lo squeeze accade quando i trapped traders sono costretti a chiudere.

**Esempio long:**

1. Sellers entrano aggressivi sotto VAL / sotto range
2. Non ottengono follow-through
3. Il prezzo recupera il livello
4. Se rompe sopra il livello di protezione, gli short chiudono
5. Chiudere short = comprare → accelerazione long

**Frasi chiave:**

> *"Squeeze of the trapped sellers."*

> *"When you recover all the aggressive sellers... they are getting fake out."*

> *"They are forced to close position... closing a sell will create more momentum long."*

### 2.8 CVD

CVD **non** è trigger standalone.

Fabio lo usa come benchmark di pressione e per capire se il movimento è supportato.

**Frasi chiave:**

> *"CVD is a benchmark for pressure of volume."*

> *"If you see aggression and CVD doing this, the movement is not supported."*

Quindi CVD deve essere:

- conferma
- filtro
- warning
- strumento di trade management

Non deve generare da solo un `LONG?` o `SHORT?` se manca profile/location/trigger.

### 2.9 Timeframe

Fabio usa **5m** e **1m** nell'esempio:

| Timeframe | Uso |
|-----------|-----|
| 5m | breakout / area / profile |
| 1m | esecuzione |

Ma dice che il modello può essere usato su più timeframe.

Quindi l'indicatore non deve dipendere da timeframe fisso, ma deve distinguere:

- **profile/context window**
- **execution window**

### 2.10 Sessione / condizione

Modello 2 funziona meglio in:

- London
- summer / compression
- condizioni di range

Ma la sessione è **condizione**, non trigger.

Fabio dice anche che in New York il mean reversion può essere pericoloso se il mercato rompe con momentum.

Quindi il software deve dire:

| Contesto | Stato |
|----------|-------|
| Mercato bilanciato | `MODEL2_VALID_CONTEXT` |
| Apertura / volatilità / direzione incerta | `MODEL2_RISKY_CONTEXT` |
| Trend / out-of-balance | `MODEL2_INVALID_CONTEXT` |

---

## 3. Cosa NON deve essere un segnale operativo

Queste cose **da sole** non devono generare segnale:

- Wick lunga isolata
- CVD divergence isolata
- Delta positivo/negativo isolato
- Primo breakout fuori range
- Primo tocco di VAH/VAL
- Aggressione grande ma ancora senza recupero/fallimento
- Movimento dentro noise/range senza uscita e rientro

Devono essere classificate come:

- `CONTEXT`
- `WATCH`
- `NO_TRADE`
- `WAIT_CONFIRMATION`

---

## 4. Input corretti da usare

### 4.1 Profile inputs

| Input | Uso |
|-------|-----|
| Volume per price level | costruire profilo |
| POC | target primario |
| VAH | upper boundary balance |
| VAL | lower boundary balance |
| LVN | reaction/entry/refinement level |
| Delta per price level | identificare dominanza buyer/seller |

**Fonte ATAS:**

- `GetCandle(bar)`
- `GetAllPriceLevels()`
- `PriceVolumeInfo.Price`, `Volume`, `Ask`, `Bid`

### 4.2 Order flow inputs

| Input | Uso |
|-------|-----|
| CumulativeTrade / big trades | trigger primario |
| Direction Buy/Sell | lato aggressivo |
| Volume trade | filtro 20/30/40 contratti |
| Price del trade | livello di aggressione |
| Delta live | pressione corrente |
| CVD | benchmark pressione |

**Fonte ATAS:**

- `OnNewTrade(MarketDataArg)` per tick
- `OnCumulativeTrade(CumulativeTrade)` per big trades se disponibile
- fallback: aggregare tick per prezzo

### 4.3 Price structure inputs

| Input | Uso |
|-------|-----|
| Range high/low balance | break/fakeout |
| Local swing high/low | secondo drive / trigger |
| Close rispetto a VAH/VAL | rientro nel balance |
| High/Low corrente | stop/invalidation |
| Distance to POC | target e R:R |

---

## 5. Output corretti

### 5.1 Stati, non segnali grezzi

L'indicatore deve produrre stati chiari:

#### `NO_TRADE_CONTEXT`

- non siamo in balance
- o dati insufficienti
- o troppo rumore

#### `BALANCE_READY`

- balance identificato
- POC/VAH/VAL validi

#### `FIRST_BREAKOUT_WAIT`

- prezzo ha rotto VAH/VAL
- nessuna entry

#### `FAKEOUT_WATCH`

- breakout non ha follow-through
- si osserva rientro

#### `TRAPPED_SELLERS_LONG_WATCH`

- sellers aggressivi sotto VAL
- no follow-through
- possibile squeeze long

#### `TRAPPED_BUYERS_SHORT_WATCH`

- buyers aggressivi sopra VAH
- no follow-through
- possibile squeeze short

#### `TRIGGER_LONG`

- rientro nel balance + big buy aggression / recovery level
- target POC sopra
- stop sotto failed area / big orders

#### `TRIGGER_SHORT`

- rientro nel balance + big sell aggression / recovery level
- target POC sotto
- stop sopra failed area / big orders

#### `INVALIDATED`

- livello di invalidazione rotto con follow-through
- narrative flips

### 5.2 Output visuali

**Sul prezzo:**

- VAH line
- VAL line
- POC line
- LVN marker
- big aggression nodes
- solo tag compatti: `WATCH LONG`, `TRIGGER LONG`, `WATCH SHORT`, `TRIGGER SHORT`, `INVALID`

**Nel box:**

- STATO
- DOVE SIAMO rispetto a VAH/VAL/POC
- COSA È SUCCESSO
- COSA SERVE PER ENTRARE
- TARGET
- INVALIDA

**Nel log:**

- stesso testo del box
- snapshot tecnico: price, POC/VAH/VAL, distance to POC, big trades count/volume, delta per level, CVD, state machine state

---

## 6. Trigger reale secondo Fabio

### Long Modello 2

**Condizione iniziale:**

- mercato in balance
- profile valido
- prezzo esce sotto VAL / discount
- primo movimento ignorato

**Watch:**

- sell aggression sotto VAL
- sellers non ottengono follow-through
- prezzo recupera verso/inside value
- si vede buyer protection / buyer aggression
- short trapped

**Trigger:**

- recupero del livello chiave / break del micro swing high
- big buy trade sopra filtro
- oppure squeeze dei trapped sellers
- target POC sopra

**Stop:**

- sotto area di aggressione/protezione
- sotto low del failed breakout
- Fabio preferisce essere sbagliato subito

### Short Modello 2

Speculare:

- balance
- prezzo esce sopra VAH / premium
- primo movimento ignorato
- buy aggression senza follow-through
- prezzo rientra dentro value
- sell aggression o recovery level
- target POC sotto

---

## 7. Cosa cambiare nel nostro indicatore

### Da eliminare o declassare

| Logica attuale | Azione |
|----------------|--------|
| `FAILED_AUCTION` da wick isolata | declassare a `CONTEXT`/`WATCH` se non fuori VAH/VAL |
| `CVD_BULL_DIV` standalone | declassare a conferma, non segnale |
| `SQUEEZE` da Δsum generico | sostituire con trapped-side + recovery level |
| `ABSORPTION` da delta generico | sostituire con big trades no-follow-through a livello profile |
| `LONG?`/`SHORT?` generici | sostituire con `WATCH` e `TRIGGER` |

### Da aggiungere

**Profile engine:**

- lookback/session/manual profile
- POC/VAH/VAL
- LVN
- delta per price level

**Big trade engine:**

- `OnCumulativeTrade` se ATAS lo supporta bene
- fallback aggregazione tick
- filter 20/30/40

**Balance state machine:**

- `BALANCE_READY`
- `FIRST_BREAKOUT_WAIT`
- `FAKEOUT_WATCH`
- `TRIGGER_LONG`/`SHORT`
- `INVALIDATED`

**Risk/target:**

- target POC
- stop livello failed area
- distance/R:R stimato

---

## 8. Specifica minima nuova versione

### Parametri

| Parametro | Default | Note |
|-----------|---------|------|
| Session Profile Start | sessione corrente | profilo dalla prima barra di sessione via `IsNewSession(bar)` |
| ValueAreaPercent | 70 | VAH/VAL |
| BigTradeFilter | 20 | London default |
| BigTradeFilterNY | 30 | NY default |
| UseCumulativeTrades | true | se disponibile |
| MinBreakoutTicks | 4 | uscita minima da VAH/VAL |
| ReentryTicks | 2 | rientro nel value |
| RequireSecondDrive | true | coerente Fabio |
| LvnSensitivity | 0.35 | volume basso vs media |
| TargetMode | POC | target primario |

### Output primario

Non "BUY/SELL" continuo, ma:

```
STATO: BALANCE READY
DOVE: sotto VAL, possibile fakeout
TRIGGER: manca buyer aggression > 20 contratti
TARGET: POC 30280.50
INVALIDA: nuova rottura sotto 30260 con sellers follow-through
```

Solo quando trigger completo:

```
TRIGGER LONG: trapped sellers recovered + big buy aggression
TARGET: POC
STOP: sotto failed low / big orders
```

---

## 9. Conclusione

Il nostro indicatore attuale è utile per leggere order flow, ma **non è ancora** il vero Modello 2 di Fabio.

Il vero Modello 2 richiede questa gerarchia:

1. **Market state:** balance/consolidation
2. **Profile:** POC, VAH, VAL, LVN
3. **First breakout:** osserva, non entrare
4. **Fakeout/reentry:** prepara scenario
5. **Big trades/aggression:** trigger
6. **No follow-through/absorption:** conferma trapped side
7. **Target POC**
8. **Invalidation** se la rottura continua con follow-through

La prossima implementazione deve essere una **riscrittura parziale** basata su profile/state machine, non un semplice tuning delle soglie attuali.

---

*Specifica completa — Fabio Valentino Modello 2 Mean Reversion · orderflow-atas*