# Modello 2 — Mean Reversion: Fase 1 — Zona di Balance

> **Scope di questo documento:** individuare **solo** la zona di balance.  
> Trigger, fakeout, aggressione e state machine completa verranno documentati nelle fasi successive.

**Data:** 2026-06-15  
**Fonte:** transcript *Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)*

---

## Contesto minimo

Il Modello 2 di Fabio Valentino è mean reversion su mercati **in balance** (consolidamento).

| Stato mercato | Modello |
|---------------|---------|
| **In balance** / consolidamento | ✅ Modello 2 |
| **Out of balance** / trend | ❌ Modello 1 (Trend Following) |

> *"Model 2 is mean reverting... market state is consolidation... we are trying to take the out of balance condition that gets back inside balance."*

Senza una zona di balance valida **non esiste Modello 2**. Tutto il resto (breakout, fakeout, trigger) dipende da questa identificazione.

---

## Cosa è la zona di balance

La zona di balance è l'area dove il mercato **transa valore in modo efficiente**: il prezzo oscilla, il Volume Profile è **protettivo** ai confini, e la maggior parte del volume si concentra attorno al POC.

### Componenti obbligatori

| Livello | Significato | Ruolo nel Modello 2 |
|---------|-------------|---------------------|
| **POC** | Point of Control — prezzo con più volume | Centro gravitazionale; target futuro del trade |
| **VAH** | Value Area High — limite superiore (~70% volume) | Confine superiore del balance |
| **VAL** | Value Area Low — limite inferiore (~70% volume) | Confine inferiore del balance |
| **Value Area** | Fascia VAH–VAL | Area dove avviene il bulk delle transazioni |

> *"This blue line is the value area. This is the value area high. This is the value area low. This is the POC."*

> *"Market state is consolidation and is when the profile is protecting from breaking here and breaking here."*

### Lettura visiva (come Fabio la riconosce)

1. **Candele compresse** — il prezzo non espande in modo direzionale.
2. **Profile a forma di campana** — volume concentrato al centro, meno volume ai bordi.
3. **Confini che tengono** — tentativi di uscire da VAH/VAL vengono respinti o riassorbiti.
4. **Nessun momentum sostenuto** — non si passa da "small orders balance" ad aggressione continua in una direzione.

```
        VAH ─────────────────────────  ← confine superiore
              ╲    volume    ╱
               ╲   ████████  ╱
                ╲  ████████ ╱
        POC ───── ████████████ ─────  ← bulk of auction
                ╱  ████████ ╲
               ╱   ████████  ╲
              ╱    volume    ╲
        VAL ─────────────────────────  ← confine inferiore
```

---

## Due modi per individuare il balance

Fabio distingue un approccio semplice e uno avanzato. Per l'indicatore ATAS partiamo dal **session profile** (approccio operativo su timeframe intraday).

### Metodo A — Daily Profile (semplice)

> *"You can make it stupid simple putting just daily profile."*  
> *"I use the profile of the previous day."*

| Pro | Contro |
|-----|--------|
| Facile da tracciare anche su TradingView | Meno preciso su compressioni intraday |
| Nessuna interpretazione soggettiva del range | Non cattura balance che si forma durante la sessione |

**Uso:** contesto macro. Utile per capire dove si trova il prezzo rispetto al giorno precedente, non come unico input operativo.

### Metodo B — Compression Profile (avanzato, target implementazione)

> *"You just take an orderflow platform and you see the compressed candles and you just plot the profile on there."*

| Pro | Contro |
|-----|--------|
| Identifica il balance **reale** del momento | Richiede scegliere correttamente inizio/fine range |
| Allineato all'esecuzione intraday | Non è immediato — la compressione si vede dopo che si forma |

**Uso:** metodo principale per l'indicatore. Si traccia il profile sull'area di compressione visibile sul chart.

### Scelta per ATAS: Session Profile

Per la prima implementazione usiamo il **profile di sessione corrente**:

- **Inizio:** prima barra della sessione (`IsNewSession(bar) == true`)
- **Fine:** barra corrente (aggiornamento rolling)
- **Calcolo:** aggregare volume per price level su tutte le barre del range

Questo è coerente con l'approccio "compression profile" senza richiedere selezione manuale del range.

---

## Criteri: balance valido vs non valido

### ✅ Balance valido (`BALANCE_READY`)

Tutte le condizioni devono essere soddisfatte:

| # | Criterio | Dettaglio |
|---|----------|-----------|
| 1 | Profile costruibile | Almeno N barre nella sessione con volume > 0 |
| 2 | POC/VAH/VAL calcolabili | Value area al 70% (parametro configurabile) |
| 3 | Prezzo dentro o vicino alla value area | Close tra VAL e VAH, oppure entro tolleranza configurabile |
| 4 | Assenza di trend direzionale | Nessuna sequenza HH/HL o LH/LL sostenuta fuori dal range |
| 5 | Profile protettivo | VAH e VAL non rotti con follow-through sostenuto |

**Stato output:** `BALANCE_READY`

### ⚠️ Contesto rischioso (`BALANCE_UNCERTAIN`)

| Situazione | Perché |
|------------|--------|
| Compressione appena iniziata | *"You don't identify it immediately"* — troppo presto per tradare |
| Profile stretto / poco volume | Value area instabile, livelli poco affidabili |
| News / gap da ribilanciare | Volatilità artificiale, market maker assenti |
| Sessione NY con breakout momentum | Mean reversion pericoloso se il mercato rompe con forza |

**Stato output:** `BALANCE_UNCERTAIN` — osservare, non operare.

### ❌ Non è balance (`OUT_OF_BALANCE`)

| Situazione | Perché |
|------------|--------|
| Prezzo fuori VAH/VAL con momentum | Mercato in espansione, non consolidamento |
| Due aree di balance separate da LVN | Transizione verso nuovo equilibrio → Modello 1 |
| Breakout con aggressione continua | *"You are going from small orders balance to someone buying aggressively and continuation"* |

**Stato output:** `OUT_OF_BALANCE` — passare a Modello 1 o restare flat.

---

## Algoritmo — Profile Engine (Fase 1)

### Input ATAS

| Input | API | Uso |
|-------|-----|-----|
| Volume per livello | `GetCandle(bar).GetAllPriceLevels()` | Costruire istogramma volume |
| Prezzo livello | `PriceVolumeInfo.Price` | Chiave dizionario |
| Volume livello | `PriceVolumeInfo.Volume` | Accumulo |
| Delta per livello | `PriceVolumeInfo.Ask`, `.Bid` | Uso futuro (non in Fase 1) |
| Inizio sessione | `IsNewSession(bar)` | Trovare `sessionStartBar` |

### Parametri

| Parametro | Default | Note |
|-----------|---------|------|
| `ValueAreaPercent` | 70 | Percentuale volume per VAH/VAL |
| `MinProfileBars` | 10 | Barre minime prima di considerare il profile valido |
| `InsideValueToleranceTicks` | 0 | Tolleranza per considerare il prezzo "dentro" value |

### Calcolo POC / VAH / VAL

```
1. Scansiona barre da sessionStartBar a bar corrente
2. Per ogni barra, aggrega volume per price level → Dictionary<price, volume>
3. POC = prezzo con volume massimo
4. Ordina livelli per distanza dal POC, espandi simmetricamente fino a ValueAreaPercent del volume totale
5. VAH = prezzo più alto nella value area espansa
6. VAL = prezzo più basso nella value area espansa
```

### Validazione balance

```
if profileBars < MinProfileBars → NO_PROFILE
else if POC/VAH/VAL non calcolabili → NO_PROFILE
else if prezzo fuori VAH/VAL con momentum → OUT_OF_BALANCE
else if prezzo dentro [VAL, VAH] → BALANCE_READY
else → BALANCE_UNCERTAIN
```

---

## Output Fase 1

### Sul chart

| Elemento | Descrizione |
|----------|-------------|
| Linea verticale `SESSION START` | Inizio profile |
| Istogramma volume | Nel range sessione, sovrapposto alle candele |
| Value area evidenziata | Fascia VAH–VAL colorata |
| Linee orizzontali | POC (principale), VAH, VAL — limitate al range sessione |
| Etichetta | `Session Profile · N bars` |

### Nel box / log

```
STATO: BALANCE_READY
POC:   21280.50
VAH:   21295.00
VAL:   21265.00
DOVE:  inside value (close 21278.25)
NOTE:  profile valido · 47 barre sessione
```

Stati possibili in Fase 1:

| Stato | Significato |
|-------|-------------|
| `NO_PROFILE` | Dati insufficienti, sessione appena iniziata |
| `BALANCE_READY` | Zona di balance identificata e valida |
| `BALANCE_UNCERTAIN` | Profile presente ma contesto non ancora affidabile |
| `OUT_OF_BALANCE` | Mercato in espansione — Modello 2 non applicabile |

### Cosa NON produce la Fase 1

- ❌ Segnali LONG / SHORT
- ❌ Trigger da wick, CVD, delta isolato
- ❌ Rilevamento fakeout o trapped traders
- ❌ Target e stop operativi

---

## Sessione e timing

| Aspetto | Fabio | Implementazione Fase 1 |
|---------|-------|------------------------|
| Sessione preferita | London | Filtro opzionale, non bloccante in Fase 1 |
| Stagione | Estate / compression days | Nota contestuale |
| Timeframe contesto | 5m (breakout/area) | Profile indipendente dal TF chart |
| Timeframe esecuzione | 1m | Fase futura |

> *"You're looking at potentially London... in your consolidation periods such as normally your summer months."*

La sessione è **condizione**, non trigger. In Fase 1 il profile si costruisce comunque; il filtro sessione può limitare solo la visualizzazione dello stato operativo.

---

## Checklist operativa — Fase 1

Prima di passare alle fasi successive (fakeout, trigger), verificare:

- [ ] Il profile copre un'area di **compressione visibile** (candele strette, non trend)
- [ ] POC, VAH, VAL sono **stabili** (non saltano ogni barra)
- [ ] Il prezzo **rispetta** i confini VAH/VAL o li testa senza follow-through
- [ ] Non c'è **momentum direzionale** fuori dal range
- [ ] Il volume nel profile è **sufficiente** (non 3 barre in un range di 2 tick)

---

## Frasi chiave (solo balance)

> *"We can only have two market state. We can have a balanced market... and we can have an imbalance market."*

> *"The model 2 is using the market state that is the opposite... consolidation."*

> *"Market state is consolidation and is when the profile is protecting from breaking here and breaking here."*

> *"You go to where the bulk of the auctions taking place."*

> *"Now the tricky part is correctly identifying the consolidation."*

> *"You can make it stupid simple putting just daily profile... or identifying the most interesting compression area."*

---

## Prossime fasi (non in questo documento)

| Fase | Contenuto |
|------|-----------|
| **Fase 2** | Breakout fuori VAH/VAL — osservazione, no entry |
| **Fase 3** | Fakeout / rientro inside value |
| **Fase 4** | Aggressione (big trades) + assorbimento |
| **Fase 5** | Trigger LONG/SHORT, target POC, invalidazione |

---

*Specifica Fase 1 — Fabio Valentino Modello 2 Mean Reversion · orderflow-atas*