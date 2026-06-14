# ATAS — Programmazione Order Flow e Footprint

**Data ricerca:** 2026-06-10  
**Contesto:** sviluppo indicatori e strategie order flow / footprint su futures NQ/ES in C# con API ATAS.

---

## Indice

1. [ATAS per programmazione (API, coding)](#1-atas-per-programmazione-api-coding)
2. [Strategia footprint NQ/ES — percorso ATAS](#2-strategia-footprint-nqes--percorso-atas)
3. [Raccomandazioni finali](#3-raccomandazioni-finali)
4. [Risorse e link](#4-risorse-e-link)

---

## 1. ATAS per programmazione (API, coding)

### Valutazione

ATAS **non è tra i migliori** per programmazione/trading algoritmico in generale, ma **è eccellente** per indicatori e strategie legati all'**order flow** (footprint, delta, cluster, volume).

**Punteggio:** 6.5/10 in generale, **8.5/10 per order flow specifico**.

### Stack tecnico

| Feature | Dettaglio |
|---------|-----------|
| Linguaggio | **C#** (.NET 8) |
| IDE | Visual Studio / Rider |
| Indicatori custom | Classe `Indicator`, metodo `OnCalculate()` |
| Strategie auto | `ChartStrategy` con `OpenOrder()` |
| Docs | [docs.atas.net](https://docs.atas.net/) |
| Esempi | [GitHub AtasPlatform/Indicators](https://github.com/AtasPlatform/Indicators) |
| Extra | SDK Python `atas-client`, webhooks (v7.0.10+) |
| Deploy | Compili DLL → cartella `Indicators` / `Strategies` |
| Community dev | Telegram dev chat, marketplace addon |
| ATAS X | Versione cross-platform (Windows + macOS) — docs separate |

### Punto di forza unico

Accesso nativo a **dati cluster** (`GetCandle()` con bid/ask volume, delta, POC, `GetAllPriceLevels()`) — difficile da replicare su altre piattaforme.

### Limiti

- **Solo Windows** per versione classica (ATAS X in beta per macOS)
- Community dev **piccola** rispetto a NinjaTrader/Sierra Chart
- Ecosistema addon limitato
- Backtesting meno maturo di NinjaTrader/MultiCharts
- Moduli third-party richiedono piano **Plus+**
- Non è piattaforma "quant" generalista (no Python nativo robusto, no cloud backtest)

### Dove ATAS vince nel coding

| Use case | ATAS |
|----------|------|
| Footprint / delta / imbalance custom | **Prima scelta** |
| Big trades / tape analysis | **Prima scelta** |
| DOM + MBO (con Rithmic) | **Supportato** |
| Bot futures generico con optimize massivo | Meglio NinjaTrader |
| Quant research Python su storico | Meglio QuantConnect |

---

### Espansione order flow — modello dati

L'API order flow ruota attorno a due oggetti:

| Oggetto | Ruolo |
|---------|-------|
| `IndicatorCandle` | Candela con OHLCV + delta aggregato, POC, OI, timestamp |
| `PriceVolumeInfo` | Cluster footprint a un singolo livello di prezzo (bid, ask, volume) |

**Regola fondamentale:** i bar vanno da `0` a `CurrentBar - 1`. Chiamare `GetCandle(bar)` fuori range interrompe i calcoli.

#### Proprietà `IndicatorCandle` rilevanti per order flow

| Proprietà | Uso |
|-----------|-----|
| `Open`, `High`, `Low`, `Close` | Struttura prezzo standard |
| `Volume`, `Ask`, `Bid` | Volume totale e split bid/ask della barra |
| `Delta` | `Ask - Bid` aggregato sulla barra |
| `MaxDelta`, `MinDelta` | Estremi intra-barra del delta |
| `MaxOI`, `MinOI` | Open Interest min/max sulla barra |
| `LastTime` | Timestamp dell'ultimo trade sulla barra |
| `Time` | Apertura barra |

#### Metodi footprint su `IndicatorCandle`

| Metodo / proprietà | Restituisce |
|--------------------|-------------|
| `GetPriceVolumeInfo(decimal price)` | Cluster a un prezzo specifico (es. high/low) |
| `GetAllPriceLevels()` | Tutti i livelli footprint della barra |
| `MaxVolumePriceInfo` | Livello con volume massimo → **POC** |
| `MaxTickPriceInfo` | Livello con più tick |
| `MaxPositiveDeltaPriceInfo` | Livello con delta positivo massimo |
| `MaxNegativeDeltaPriceInfo` | Livello con delta negativo massimo |

#### Proprietà `PriceVolumeInfo`

| Proprietà | Significato |
|-----------|-------------|
| `Price` | Livello di prezzo |
| `Volume` | Volume totale al livello |
| `Ask` | Volume eseguito al ask (acquisti aggressivi) |
| `Bid` | Volume eseguito al bid (vendite aggressive) |
| `Ask - Bid` | Delta al livello |

```csharp
// Lettura completa footprint su barra chiusa
protected override void OnCalculate(int bar, decimal value)
{
    if (bar < 0 || bar >= CurrentBar)
        return;

    var candle = GetCandle(bar);

    // Aggregati barra
    decimal barDelta = candle.Delta;
    decimal maxDelta = candle.MaxDelta;
    decimal minDelta = candle.MinDelta;

    // POC e estremi delta
    var poc = candle.MaxVolumePriceInfo;
    var maxPosDelta = candle.MaxPositiveDeltaPriceInfo;
    var maxNegDelta = candle.MaxNegativeDeltaPriceInfo;

    // Cluster al high (utile per absorption / rejection)
    var highLevel = candle.GetPriceVolumeInfo(candle.High);
    if (highLevel != null)
    {
        decimal highVol = highLevel.Volume;
        decimal highDelta = highLevel.Ask - highLevel.Bid;
    }

    // Iterazione tutti i livelli
    foreach (var level in candle.GetAllPriceLevels())
    {
        if (level == null) continue;
        decimal levelDelta = level.Ask - level.Bid;
        // imbalance, stacked levels, value area...
    }
}
```

---

### Espansione order flow — eventi tick e big trades

Oltre ai dati cluster per barra, ATAS espone eventi real-time per analisi tape-level.

#### `OnNewTrade(MarketDataArg arg)`

Chiamato su **ogni singolo tick**. Utile per:
- Conteggio tick buy/sell intra-barra
- Filtri su size minima
- Correlazione con DOM in tempo reale

#### `OnCumulativeTrade(CumulativeTrade trade)` + `OnUpdateCumulativeTrade()`

I cumulative trades sono **big trades aggregati** — possono essere modificati mentre si formano. Pattern corretto:

```csharp
private CumulativeTrade _lastTrade;
private decimal _lastCumulativeTradeVolume;

protected override void OnCumulativeTrade(CumulativeTrade trade)
    => AddCumulativeTrade(trade);

protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)
    => AddCumulativeTrade(trade);

private void AddCumulativeTrade(CumulativeTrade trade)
{
    if (trade.Volume < MinBigTradeSize) // es. 50 per ES
        return;

    if (_lastTrade != trade)
    {
        _lastTrade = trade;
        this[CurrentBar - 1] += GetVolumeByDirection(trade.Volume, trade.Direction);
    }
    else
    {
        // Trade in aggiornamento: aggiungi solo il delta di volume
        this[CurrentBar - 1] += GetVolumeByDirection(
            trade.Volume - _lastCumulativeTradeVolume, trade.Direction);
    }
    _lastCumulativeTradeVolume = trade.Volume;
}

private decimal GetVolumeByDirection(decimal volume, TradeDirection direction)
    => volume * (direction == TradeDirection.Buy ? 1 : -1);
```

#### Storico cumulative trades

Per backfill su barre già caricate:

```csharp
private CumulativeTradesRequest _request;

protected override void OnFinishRecalculate()
{
    var startTime = GetCandle(0).Time;
    var lastTime = GetCandle(CurrentBar - 1).LastTime;
    _request = new CumulativeTradesRequest(startTime, lastTime, minVolume: 50, maxVolume: 0);
    RequestForCumulativeTrades(_request);
}

protected override void OnCumulativeTradesResponse(
    CumulativeTradesRequest request, IEnumerable<CumulativeTrade> trades)
{
    if (request != _request) return;
    // Processa storico big trades
}
```

`maxVolume: 0` disabilita il filtro superiore.

---

### Espansione order flow — DOM e MBO

#### Market Depth (`MarketDepthChanged`)

```csharp
protected override void MarketDepthChanged(MarketDataArg arg)
{
    // Snapshot completo DOM
    var snapshot = MarketDepthInfo.GetMarketDepthSnapshot();

    // Volumi cumulativi bid/ask (Dom Power)
    decimal cumAsks = MarketDepthInfo.CumulativeDomAsks;
    decimal cumBids = MarketDepthInfo.CumulativeDomBids;
}
```

**Segnali DOM codificabili:**

| Segnale | Logica |
|---------|--------|
| **Pull** | Livelli bid/ask che spariscono senza trade corrispondente |
| **Stack** | Nuovi livelli DOM che si accumulano sopra/sotto il prezzo |
| **Dom imbalance** | `CumulativeDomBids / CumulativeDomAsks` sopra soglia |
| **Spoofing hint** | Ordini grandi che appaiono e spariscono senza esecuzione (richiede MBO) |

#### MBO — Market By Order (solo Rithmic)

Accesso agli ordini singoli nel book. Unico feed supportato su ATAS.

```csharp
private IMarketByOrdersManager _manager;

protected override async void OnInitialize()
{
    _manager = await SubscribeMarketByOrderData();
    _manager.Changed += OnMboChanged;
    OnMboChanged(_manager.MarketByOrders);
}

private void OnMboChanged(IEnumerable<MarketByOrder> orders)
{
    foreach (var mbo in orders)
    {
        // mbo.Type: Snapshot | New | Change | Delete
        // mbo.Side: Bid | Ask | Trade
        // mbo.Price, mbo.Volume, mbo.ExchangeOrderId, mbo.Priority
    }
}

protected override void OnDispose()
{
    if (_manager != null)
        _manager.Changed -= OnMboChanged;
    base.OnDispose();
}
```

Con MBO + `OnNewTrade()` puoi correlare `ExchangeOrderId` e `AggressorExchangeOrderId` per identificare chi ha colpito chi.

---

### Espansione order flow — implementazione segnali

Pattern riutilizzabili per codificare i segnali footprint più comuni.

#### Imbalance / Stacked Imbalance

```csharp
private bool HasStackedImbalance(IndicatorCandle candle, decimal ratio, int minLevels, bool bullish)
{
    var levels = candle.GetAllPriceLevels()
        .Where(l => l != null)
        .OrderBy(l => l.Price)
        .ToList();

    int streak = 0;
    foreach (var level in levels)
    {
        bool isImbalance = bullish
            ? (level.Bid > 0 && level.Ask / level.Bid >= ratio)
            : (level.Ask > 0 && level.Bid / level.Ask >= ratio);

        streak = isImbalance ? streak + 1 : 0;
        if (streak >= minLevels) return true;
    }
    return false;
}
```

Soglie tipiche ES/NQ: ratio **3:1**, minimo **3 livelli** consecutivi.

#### Delta Divergence

```csharp
private bool IsBearishDeltaDivergence(int bar, int lookback = 5)
{
    var current = GetCandle(bar);
    decimal prevHigh = decimal.MinValue;
    decimal prevMaxDelta = decimal.MinValue;

    for (int i = bar - lookback; i < bar; i++)
    {
        var c = GetCandle(i);
        if (c.High > prevHigh) prevHigh = c.High;
        if (c.MaxDelta > prevMaxDelta) prevMaxDelta = c.MaxDelta;
    }

    return current.High > prevHigh && current.Delta < prevMaxDelta;
}
```

#### Absorption

```csharp
private bool IsAbsorption(IndicatorCandle candle, decimal volumeThreshold)
{
    var poc = candle.MaxVolumePriceInfo;
    if (poc == null) return false;

    // Alto volume al POC ma range stretto = absorption
    decimal range = candle.High - candle.Low;
    decimal tickSize = InstrumentInfo.TickSize;

    return poc.Volume >= volumeThreshold
        && range <= tickSize * 3
        && Math.Abs(candle.Close - candle.Open) <= tickSize;
}
```

#### POC Shift

```csharp
private int GetPocShiftDirection(int bar)
{
    if (bar < 1) return 0;
    var prev = GetCandle(bar - 1).MaxVolumePriceInfo;
    var curr = GetCandle(bar).MaxVolumePriceInfo;
    if (prev == null || curr == null) return 0;

    if (curr.Price > prev.Price) return 1;  // POC sale → bias bullish
    if (curr.Price < prev.Price) return -1; // POC scende → bias bearish
    return 0;
}
```

#### CVD intra-sessione

```csharp
private decimal _sessionCvd;

protected override void OnCalculate(int bar, decimal value)
{
    var candle = GetCandle(bar);
    _sessionCvd += candle.Delta;
    // Reset a inizio sessione con controllo su candle.Time
}
```

---

### Espansione order flow — architettura codice

#### Setup progetto

```
1. Class Library .NET 8
2. Riferimenti DLL da C:\Program Files (x86)\ATAS Platform\:
   - ATAS.Indicators.dll
   - ATAS.Strategies.dll
   - ATAS.DataFeedsCore.dll
   - Utils.Common.dll
3. Build → copia output in:
   - %APPDATA%\ATAS\Indicators  (indicatori)
   - %APPDATA%\ATAS\Strategies   (strategie)
4. In ATAS: pulsante lampeggiante per ricaricare la lista
```

Clone [AtasPlatform/Indicators](https://github.com/AtasPlatform/Indicators) come riferimento build.

#### Separare logica segnali da UI

```
OrderFlowSignals.cs     ← logica pura (imbalance, divergence, absorption)
FootprintIndicator.cs   ← visualizza segnali, log, alert
FootprintStrategy.cs    ← eredita ChartStrategy, usa OrderFlowSignals
```

`ChartStrategy` eredita da `Indicator` — stessa API footprint disponibile in entrambi.

#### Quando processare i segnali

| Contesto | Momento |
|----------|---------|
| Indicatore | `OnCalculate()` su barra chiusa (`bar < CurrentBar - 1`) |
| Strategia | `CanProcess()` → true solo su ultima barra chiusa |
| Tick events | `OnNewTrade()` / `MarketDepthChanged()` — solo per filtri real-time, non per entry |

**Anti-pattern:** aprire posizioni su ogni tick senza `CanProcess()`.

#### Logging e debug

```csharp
this.LogInfo($"Bar {bar} | Delta={candle.Delta} | POC={poc?.Price} | Imbalance={hasImbalance}");
```

Usare `Market Replay` per validare log su sessioni storiche prima del live.

#### Gestione memoria su `GetAllPriceLevels()`

Su barre footprint dense (Range 1 tick, alta volatilità NQ), iterare tutti i livelli su ogni barra può essere costoso. Ottimizzazioni:

- Processare solo barre chiuse, non l'intero storico ad ogni tick
- Cachare risultati per barra già calcolata
- Limitare analisi ai livelli tra `Low` e `High` con volume > 0

---

### Espansione order flow — matrice API completa

| Dato order flow | API | Tipo accesso |
|-----------------|-----|--------------|
| OHLCV + delta barra | `GetCandle(bar)` | Barra |
| Cluster per prezzo | `GetPriceVolumeInfo(price)` | Barra |
| Tutti i livelli | `GetAllPriceLevels()` | Barra |
| POC | `MaxVolumePriceInfo` | Barra |
| Delta max/min intra-barra | `MaxDelta`, `MinDelta` | Barra |
| Singolo tick | `OnNewTrade()` | Real-time |
| Big trade | `OnCumulativeTrade()` | Real-time |
| Storico big trades | `RequestForCumulativeTrades()` | Storico |
| DOM update | `MarketDepthChanged()` | Real-time |
| DOM snapshot | `MarketDepthInfo.GetMarketDepthSnapshot()` | Real-time |
| DOM cumulativo | `CumulativeDomBids`, `CumulativeDomAsks` | Real-time |
| Ordini singoli | `SubscribeMarketByOrderData()` | Real-time (Rithmic) |
| Open Interest | `MaxOI`, `MinOI` | Barra |
| Statistiche trading | `TradingStatisticsProvider` | Real-time / storico |

---

## 2. Strategia footprint NQ/ES — percorso ATAS

### Stack consigliato

```
ATAS Pro (futures real-time)
    + feed Rithmic (MBO opzionale)
    + Visual Studio + C#
    + strategia ChartStrategy su grafico footprint
```

### Capacità ATAS rilevanti per NQ/ES

| Requisito | API / feature ATAS |
|-----------|-------------------|
| Footprint per prezzo | `GetAllPriceLevels()` |
| Delta per candela | `candle.Delta` |
| Big trades / tape | `OnCumulativeTrade()` |
| DOM live | `MarketDepthChanged()` |
| MBO (ordini singoli) | `SubscribeMarketByOrderData()` — **solo Rithmic** |
| Strategie auto | `ChartStrategy` + `OpenOrder()` |
| Validazione | Market Replay (incluso in Pro) |

### Stack minimo e costi

| Componente | Scelta | Costo indicativo |
|------------|--------|------------------|
| Piattaforma | **ATAS Pro** | ~€40/mese annuale |
| Data feed | **Rithmic** via broker | ~$25–75/mese |
| Exchange fees CME | Non-professional | ~$40/mese |
| IDE | Visual Studio Community | €0 |
| VPS (opzionale) | Vicino a Chicago | ~$30–80/mese |

**Totale realistico:** ~$100–150/mese per sviluppare e testare su NQ/ES live.

### Architettura strategia (2 fasi)

```
Fase 1: Indicatore
  → Legge footprint / delta / DOM
  → Visualizza segnali su chart
  → Valida logica con Market Replay

Fase 2: ChartStrategy
  → Eredita stessa logica dell'indicatore
  → OpenOrder su segnale confermato
  → Risk management: SL/TP/breakeven
```

**Non passare subito al bot.** Prima indicatore con log, poi strategia.

### API ATAS — esempi chiave

#### Footprint per candela

```csharp
protected override void OnCalculate(int bar, decimal value)
{
    var candle = GetCandle(bar);

    decimal delta = candle.Delta;
    decimal ask = candle.Ask;
    decimal bid = candle.Bid;

    foreach (var level in candle.GetAllPriceLevels())
    {
        decimal price = level.Price;
        decimal levelDelta = level.Ask - level.Bid;
        // imbalance, POC, absorption...
    }

    var poc = candle.MaxVolumePriceInfo;
}
```

#### Eventi tick-by-tick

- `OnNewTrade()` — ogni tick
- `OnCumulativeTrade()` — big trades aggregati
- `MarketDepthChanged()` — DOM live
- `SubscribeMarketByOrderData()` — MBO (solo Rithmic)

#### Strategia automatica (`ChartStrategy`)

- `OpenOrder()`, `ModifyOrder()`, `CancelOrder()`
- `CurrentPosition`, `AveragePrice`
- `CanProcess()` — verifica barra chiusa prima di tradare
- `OnStopping()` — **obbligatorio**: chiudi posizioni e cancella ordini

Deploy DLL in `%APPDATA%\ATAS\Strategies`.

### Segnali footprint tipici da codificare

| Segnale | Logica programmatica |
|---------|---------------------|
| **Imbalance** | `level.Ask / level.Bid > ratio` (es. 3:1) su 3+ livelli consecutivi |
| **Delta divergence** | Prezzo fa nuovo high ma `candle.Delta` decresce |
| **Absorption** | Alto volume su un livello, prezzo non si muove |
| **POC shift** | `MaxVolumePriceInfo.Price` si sposta nella direzione del trend |
| **Stacked imbalance** | Serie di imbalance buy/sell nella stessa direzione |
| **Big trade** | `OnCumulativeTrade()` con volume > soglia (es. 50 contratti ES) |
| **DOM pull/stack** | `MarketDepthChanged()` — bid che spariscono = pressione sell |

Partire da **1–2 segnali**, non da tutti insieme.

### Percorso pratico (4 settimane)

**Settimana 1 — Setup**

1. Installa ATAS + Visual Studio + [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Clone [github.com/AtasPlatform/Indicators](https://github.com/AtasPlatform/Indicators)
3. Collega feed Rithmic su **ES** o **NQ**
4. Grafico **footprint** (Range, Volume, o 5min)

**Settimana 2 — Indicatore**

1. Crea Class Library .NET 8
2. Riferimenti: `ATAS.Indicators.dll`, `ATAS.Strategies.dll`, `Utils.Common.dll`
3. Scrivi indicatore che logga delta + imbalance su ogni barra chiusa
4. Testa con **Market Replay**

**Settimana 3 — Strategia**

1. Converti logica in `ChartStrategy`
2. Aggiungi `CanProcess()` → tradare solo a barra chiusa
3. `OpenOrder()` con stop/target in tick (ES/NQ = 0.25)
4. Implementa `OnStopping()` per cleanup

**Settimana 4 — Validazione**

1. Market Replay su 20+ sessioni
2. Log ogni trade: segnale, entry, exit, P&L
3. Solo dopo risultati stabili → account demo/live

### NQ vs ES

| | ES (E-mini S&P) | NQ (E-mini Nasdaq) |
|---|---|---|
| Tick value | $12.50 | $5.00 |
| Volatilità | Più lento, più liquido | Più veloce, più noise |
| Footprint | Ottimo per imbalance | Ottimo ma più falsi segnali |
| Per iniziare | **Consigliato** | Dopo aver validato su ES |

Iniziare su **MES/MNQ** (micro) per testare con rischio ridotto.

### Errori da evitare

1. **Tradare su ogni tick** senza filtri → overtrading
2. **Dimenticare `OnStopping()`** → posizioni fantasma
3. **Feed CQG invece di Rithmic** se serve MBO (MBO solo Rithmic su ATAS)
4. **Piano Start** per futures → dati ritardati, strategia inutile
5. **Backtest su OHLCV** → serve Market Replay tick/cluster
6. **Troppi segnali combinati** → overfitting; 2 condizioni max all'inizio

---

## 3. Raccomandazioni finali

### Per obiettivo (programmazione)

| Obiettivo | Percorso |
|-----------|----------|
| Primo indicatore footprint | `GetCandle()` + `GetAllPriceLevels()` + Market Replay |
| Strategia auto NQ/ES | Indicatore → validazione → `ChartStrategy` |
| Big trades nel codice | `OnCumulativeTrade()` con gestione `OnUpdateCumulativeTrade()` |
| DOM nel codice | `MarketDepthChanged()` + `CumulativeDomBids/Asks` |
| Ordini singoli (spoofing, queue) | `SubscribeMarketByOrderData()` + feed Rithmic |
| Codice manutenibile | Logica segnali in classe separata, condivisa tra indicator e strategy |

### Decision tree

```
Vuoi leggere footprint/delta per barra?
  → GetCandle() + GetAllPriceLevels() + MaxVolumePriceInfo

Vuoi reagire a big trades?
  → OnCumulativeTrade() + soglia volume per strumento

Vuoi leggere il DOM?
  → MarketDepthChanged() + GetMarketDepthSnapshot()

Serve MBO / ordini singoli?
  → SubscribeMarketByOrderData() + Rithmic

Pronto per il live?
  → Indicatore validato su Market Replay → ChartStrategy con OnStopping()
```

---

## 4. Risorse e link

### Documentazione e sviluppo

| Risorsa | URL |
|---------|-----|
| Docs API | https://docs.atas.net/ |
| Dati footprint (docs) | https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.html |
| Strategie (docs) | https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20100__Strategies.html |
| GitHub esempi | https://github.com/AtasPlatform/Indicators |
| Blog algoritmi | https://atas.net/blog/algorithms-for-atas/ |
| .NET 8 SDK | https://dotnet.microsoft.com/download/dotnet/8.0 |
| Telegram dev chat | https://t.me/+Afb9R7MEDqY3MDUy |

### Riferimenti API order flow

| Classe / metodo | Documentazione |
|-----------------|----------------|
| `IndicatorCandle` | https://docs.atas.net/classATAS_1_1Indicators_1_1IndicatorCandle.html |
| `PriceVolumeInfo` | https://docs.atas.net/classATAS_1_1Indicators_1_1PriceVolumeInfo.html |
| `ChartStrategy` | https://docs.atas.net/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.html |
| `CumulativeTrade` | https://docs.atas.net/classATAS_1_1Indicators_1_1CumulativeTrade.html |
| `IMarketByOrdersManager` | https://docs.atas.net/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.html |

---

*Documento aggiornato il 2026-06-14 — focalizzato su programmazione order flow, footprint e strategie NQ/ES in C#.*