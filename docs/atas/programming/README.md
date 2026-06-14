# ATAS — Guida Programmazione (Order Flow / Footprint)

Percorso curato per sviluppare indicatori e strategie order flow in C# su ATAS.
Fonte: [docs.atas.net](https://docs.atas.net/) · [ricerca-order-flow-completa.md](../../../ricerca-order-flow-completa.md)

---

## Setup rapido

| Passo | Risorsa |
|-------|---------|
| Installare Visual Studio, creare class library .NET 8 | [Development of a user indicator](../../guides/md_DataFeedsCore_2Docs_2en_20010__BasicIndicator.md) |
| Collegare `ATAS.Indicators.dll` + `PresentationCore.dll` | Stessa guida § "Installing the library" |
| Esempi pronti su GitHub | [AtasPlatform/Indicators](https://github.com/AtasPlatform/Indicators) |

## Guide fondamentali

| Argomento | Link |
|-----------|------|
| Parametri esterni, filtri, collezioni | [Customizing an indicator](../../guides/md_DataFeedsCore_2Docs_2en_20020__CustomizingOurIndicator.md) |
| Candele, footprint, tick, DOM, MBO | [Receiving and processing data](../../guides/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.md) |
| Eventi di trading (OnNewTrade, CumulativeTrade) | [Working with trading events](../../guides/md_DataFeedsCore_2Docs_2en_20030__IndicatorEvents.md) |
| Indicatori compositi (multi-serie) | [Creating composite indicators](../../guides/md_DataFeedsCore_2Docs_2en_20040___01CompositeIndicators.md) |
| DataSeries (Value, Range, Paintbars, Candle, PriceSelection, Object) | [Dataseries](../../guides/md_DataFeedsCore_2Docs_2en_20050__Dataseries.md) |
| Disegno custom (RenderContext, coordinate) | [Drawing](../../guides/md_DataFeedsCore_2Docs_2en_20070__Graphics.md) |
| Oggetti grafici (linee, trend, rettangoli, testo) | [Graphic shapes](../../guides/md_DataFeedsCore_2Docs_2en_20060__EmbeddedGraphicShapes.md) |
| Keyboard + mouse events | [Keyboard & mouse](../../guides/md_DataFeedsCore_2Docs_2en_20080__KeyboardMouse.md) |
| Strategie ChartStrategy + ordini | [Strategies](../../guides/md_DataFeedsCore_2Docs_2en_20100__Strategies.md) |
| Gestione ordini (sync/async) | [Managing orders](../../guides/md_DataFeedsCore_2Docs_2en_20110__ManagingOrders.md) |
| Esempi indicatori (volume, price, tabelle, watermark) | [Indicator examples](../../guides/md_DataFeedsCore_2Docs_2en_20115__IndicatorExamples.md) |
| Esempi strategie (SMA strategy) | [Strategy examples](../../guides/md_DataFeedsCore_2Docs_2en_20120__StrategyExamples.md) |
| Logging + Debug mode | [Logging](../../guides/md_DataFeedsCore_2Docs_2en_20130__AddingLogging.md) · [Debug](../../guides/md_DataFeedsCore_2Docs_2en_20135__DebugMode.md) |
| Distribuzione indicatori/strategie | [Distribution](../../guides/md_DataFeedsCore_2Docs_2en_20140__IndicatorsStrategiesDistribution.md) |
| Accesso a indicatori da altri indicatori | [Access to indicators](../../guides/md_DataFeedsCore_2Docs_2en_20150__AccessToIndicators.md) |
| ATAS X (cross-platform Win+macOS) | [ATAS X indicator](../../guides/md_DataFeedsCore_2Docs_2en_20200__ATASX__Indicator.md) |
| Heatmap indicators | [Heatmap guide](../../guides/md_Indicators_2Heatmap_2README.md) |

## API Reference — classi chiave per order flow

### Core indicatori

| Classe | Ruolo |
|--------|-------|
| [Indicator](../../api/classes/classATAS_1_1Indicators_1_1Indicator.md) | Base class per tutti gli indicatori |
| [BaseIndicator](../../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md) | `OnCalculate()`, `GetCandle()`, `CurrentBar` |
| [ExtendedIndicator](../../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md) | `Draw()`, `SubscribeToTimer()`, `ApplyDefaultColors()` |
| [ChartObject](../../api/classes/classATAS_1_1Indicators_1_1ChartObject.md) | Eventi mouse/keyboard sul grafico |

### Dati order flow / footprint

| Classe | Ruolo |
|--------|-------|
| [IndicatorCandle](../../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) | Candela con OHLCV, Delta, POC, OI, `GetPriceVolumeInfo()`, `GetAllPriceLevels()` |
| [PriceVolumeInfo](../../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) | Cluster footprint per livello (Price, Ask, Bid, Volume) |
| [CumulativeTrade](../../api/classes/classATAS_1_1Indicators_1_1CumulativeTrade.md) | Big trades aggregati (volume, direction) |
| [CumulativeTradesRequest](../../api/classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) | Richiesta storico cumulative trades |
| [MarketDataArg](../../api/classes/classATAS_1_1Indicators_1_1MarketDataArg.md) | Tick-by-tick market data |
| [MarketDepthSnapshot](../../api/classes/classATAS_1_1Indicators_1_1MarketDepthSnapshot.md) | Snapshot DOM completo |
| [MarketDepthInfoProvider](../../api/classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md) | CumulativeDomAsks/Bids |
| [InstrumentInfo](../../api/classes/classATAS_1_1Indicators_1_1InstrumentInfo.md) | Info strumento (tick size, margini) |
| [ValueArea](../../api/classes/classATAS_1_1Indicators_1_1ValueArea.md) | Value Area (VAH, VAL, POC) |

### DataSeries

| Classe | Ruolo |
|--------|-------|
| [ValueDataSeries](../../api/classes/classATAS_1_1Indicators_1_1ValueDataSeries.md) | Serie numerica (linee, istogrammi) |
| [CandleDataSeries](../../api/classes/classATAS_1_1Indicators_1_1CandleDataSeries.md) | Serie di candele custom |
| [PaintbarsDataSeries](../../api/classes/classATAS_1_1Indicators_1_1PaintbarsDataSeries.md) | Colorazione barre |
| [PriceSelectionDataSeries](../../api/classes/classATAS_1_1Indicators_1_1PriceSelectionDataSeries.md) | Selezione prezzo sul grafico |
| [ObjectDataSeries](../../api/classes/classATAS_1_1Indicators_1_1ObjectDataSeries.md) | Oggetti generici |
| [RangeDataSeries](../../api/classes/classATAS_1_1Indicators_1_1RangeDataSeries.md) | Range (high/low) |
| [IndicatorSeries](../../api/classes/classATAS_1_1Indicators_1_1IndicatorSeries.md) | Serie da altri indicatori |

### Strategie

| Classe | Ruolo |
|--------|-------|
| [ChartStrategy](../../api/classes/classATAS_1_1Strategies_1_1Chart_1_1ChartStrategy.md) | Strategia sul grafico con `OpenOrder()` |
| [Strategy](../../api/classes/classATAS_1_1Strategies_1_1Strategy.md) | Base class strategie |
| [ATMStrategy](../../api/classes/classATAS_1_1Strategies_1_1ATM_1_1ATMStrategy.md) | ATM (Advanced Trade Management) |

### Disegno

| Classe | Ruolo |
|--------|-------|
| [DrawingText](../../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingText.md) | Testo sul grafico |
| [HorizontalLine](../../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1HorizontalLine.md) | Linea orizzontale |
| [TrendLine](../../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1TrendLine.md) | Trend line |
| [DrawingRectangle](../../api/classes/classATAS_1_1Indicators_1_1Drawing_1_1DrawingRectangle.md) | Rettangolo |

---

## Punti chiave da `ricerca-order-flow-completa.md`

- **Bar index:** `0` → `CurrentBar - 1`. Mai chiamare `GetCandle(bar)` fuori range.
- **Footprint pattern:** `GetCandle(bar)` → `GetAllPriceLevels()` → per ogni livello `level.Ask - level.Bid` = delta.
- **Big trades:** pattern `OnCumulativeTrade` + `OnUpdateCumulativeTrade` con delta volume.
- **DOM signals:** `MarketDepthInfo.CumulativeDomAsks` / `CumulativeDomBids`.
- **Deploy:** compili DLL → cartella `Indicators` o `Strategies` di ATAS.
