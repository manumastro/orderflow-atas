# ATAS — Ricerca Order Flow e Footprint

**Data ricerca:** 2026-06-10  
**Contesto:** valutazione di ATAS per trading order flow / footprint su futures NQ/ES e sviluppo strategie volumetriche in C#.

---

## Indice

1. [ATAS — panoramica e licenze](#1-atas--panoramica-e-licenze)
2. [Breve confronto con altre piattaforme](#2-breve-confronto-con-altre-piattaforme)
3. [ATAS per programmazione (API, coding)](#3-atas-per-programmazione-api-coding)
4. [Strategia footprint NQ/ES — percorso ATAS](#4-strategia-footprint-nqes--percorso-atas)
5. [Raccomandazioni finali](#5-raccomandazioni-finali)
6. [Risorse e link](#6-risorse-e-link)

---

## 1. ATAS — panoramica e licenze

**ATAS** (Advanced Time And Sales) è una piattaforma specializzata in order flow, footprint, volume profile e analisi del tape. Sviluppata da ATAS LLC (origine russofona/Lettonia). Sito ufficiale anche in [cinese](https://atas.net/cn/) e [russo](https://atas.net/ru/).

### Punti di forza

- **Footprint nativo** — 400+ varianti di visualizzazione cluster, bid/ask per livello di prezzo
- **Big Trades** e Smart Tape per lettura del flusso istituzionale
- **Market Replay** tick-level per validare strategie come in live
- **Reload dati** dal server — meno tick persi rispetto ad altre piattaforme (vantaggio citato da trader pro)
- **Crypto + futures + azioni** in un'unica piattaforma
- **API C#** con accesso diretto a dati cluster (delta, POC, imbalance)
- **Heatmap** (beta) per crypto; heatmap futures locale su piani avanzati
- **Smart DOM** e DOM Trader integrati

### Piani ufficiali (2026)

| Piano | Prezzo indicativo | Note |
|-------|-------------------|------|
| **Start** | €0 — gratis per sempre | Crypto real-time; futures con **15 min di ritardo** |
| **Plus** | ~€20–25/mese | Analisi base, Range/Renko/Reversal |
| **Pro** | ~€40–70/mese | **Futures real-time**, più indicatori e connessioni |
| **Ultra** | ~€50–90/mese | Tutto sbloccato, connessioni illimitate |
| **Lifetime Ultra** | ~€1.999 una tantum | Licenza a vita |

Fonte: [atas.net/pricing](https://atas.net/pricing/)

### Percorsi economici legali

- **Trial 14 giorni** sui piani a pagamento
- **Rimborso 14 giorni** sul primo acquisto
- **Promo stagionali** (Black Friday, Capodanno) con sconti fino a €200–500
- **Prop firm** [The Trading Pit](https://www.thetradingpit.com/atas) — licenza ATAS inclusa con challenge

### Cosa include il piano Start (gratis)

- Dati crypto real-time
- 1 connessione exchange
- 2 asset simultanei
- 3 indicatori per grafico
- Indicatori base: Order Flow, Cumulative Delta, Market Profile, ecc.
- BigTrades history (auto-filter)
- Heatmap (Beta)
- Market Replay

### Cosa NON include Start (serve Plus/Pro/Ultra)

- Big Trades avanzato, CVD Pro
- Più di 3 indicatori / 2 asset
- Frame Renko/Range avanzati
- Heatmap storica estesa
- **Futures CME/MOEX in real-time** (solo ritardo 15 min)

**Per NQ/ES serio serve minimo Pro** (~€40/mese annuale).

### Partnership Bitget (ufficiale)

Partnership reale dal 2022 per trading crypto via API. Il piano Start gratuito con dati crypto real-time è già disponibile senza depositi o codici speciali. Link ufficiale: [partner.bitget.com/bg/Atas](https://partner.bitget.com/bg/Atas). Guida connessione: [help ATAS Bitget](https://help.atas.net/en/support/solutions/articles/72000606743-how-to-connect-to-bitget).

---

## 2. Breve confronto con altre piattaforme

ATAS è la scelta di riferimento per **order flow puro**. Le alternative hanno punti di forza diversi:

| Criterio | ATAS | Sierra Chart | NinjaTrader |
|----------|------|--------------|-------------|
| **Order flow / footprint** | Eccellente (core) | Eccellente | Medio (add-on $59/mese) |
| **Qualità dati tick** | Molto buona | Top (feed Denali) | Critiche da pro trader |
| **API footprint** | C# .NET 8, ricca | C++ (ACSIL) | C#, meno profonda |
| **Esecuzione futures** | Buona | Eccellente | Eccellente (SuperDOM) |
| **Facilità d'uso** | Media | Bassa | Media-alta |
| **Crypto** | Forte | Limitato | Limitato |
| **Costo software** | ~€40–90/mese (Pro/Ultra) | ~$46/mese + exchange | Gratis + $59 OF+ o $1.499 lifetime |
| **Backtest order flow** | Market Replay | Buono con replay | Debole su tick-level |

**In sintesi:** per footprint, delta e Big Trades → **ATAS prima scelta**. Sierra Chart compete su feed dati e prezzo. NinjaTrader vince su broker+esecuzione, non su analisi volumetrica.

Fonte comparativa: [United Daytraders](https://united-daytraders.com/blog/best-order-flow-trading-platforms), [Trader Dale](https://www.trader-dale.com/top-order-flow-platforms-data-explained-ninjatrader-sierra-chart-atas-quantower-tradingview/).

---

## 3. ATAS per programmazione (API, coding)

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

## 4. Strategia footprint NQ/ES — percorso ATAS

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

## 5. Raccomandazioni finali

### Per obiettivo

| Obiettivo | Consiglio ATAS |
|-----------|----------------|
| Provare order flow su crypto | Piano **Start** gratis |
| Footprint serio su NQ/ES | **Pro** + Rithmic (~$100–150/mese totale) |
| Sviluppare strategia custom | Indicatore → Market Replay → `ChartStrategy` |
| Massime funzioni | **Ultra** o lifetime |
| Risparmiare sulla licenza | Trial 14 gg, promo ufficiali, prop firm |

### Decision tree

```
Solo crypto, costo zero?
  → ATAS Start gratis

Futures NQ/ES con footprint?
  → ATAS Pro + Rithmic + C# + Market Replay
  → Iniziare su MES, poi ES/NQ

Vuoi programmare indicatori order flow?
  → ATAS (API cluster nativa in C#)

Serve MBO / ordini singoli?
  → ATAS + Rithmic (unica combinazione supportata)
```

---

## 6. Risorse e link

### ATAS — documentazione e sviluppo

| Risorsa | URL |
|---------|-----|
| Sito principale | https://atas.net/ |
| Pricing | https://atas.net/pricing/ |
| Download | https://atas.net/atas-download/ |
| Docs API | https://docs.atas.net/ |
| GitHub esempi | https://github.com/AtasPlatform/Indicators |
| Blog algoritmi | https://atas.net/blog/algorithms-for-atas/ |
| Strategie (docs) | https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20100__Strategies.html |
| Dati footprint (docs) | https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.html |
| Partnership Bitget | https://partner.bitget.com/bg/Atas |
| Help Bitget | https://help.atas.net/en/support/solutions/articles/72000606743-how-to-connect-to-bitget |
| The Trading Pit (licenza inclusa) | https://www.thetradingpit.com/atas |

### Confronti esterni (riferimento)

| Risorsa | URL |
|---------|-----|
| United Daytraders — best platforms | https://united-daytraders.com/blog/best-order-flow-trading-platforms |
| Trader Dale — platforms comparison | https://www.trader-dale.com/top-order-flow-platforms-data-explained-ninjatrader-sierra-chart-atas-quantower-tradingview/ |

---

*Documento aggiornato il 2026-06-10 — focalizzato su ATAS per order flow, footprint e strategie NQ/ES.*