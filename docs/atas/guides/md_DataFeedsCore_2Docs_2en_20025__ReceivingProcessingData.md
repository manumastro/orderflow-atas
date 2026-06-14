# Receiving and processing data

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20025__ReceivingProcessingData.html

# Obtaining and using candle data

To receive and use these candles, you need to get an object of the [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) class. To do this, we can use the [GetCandle(int bar)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4e22f2730354b56ffd66559a9c5f27af) method by specifying the bar number.

 It must be remembered that bar is an integer and the numbering of bars starts from 0 and ends with `CurrentBar - 1`, where [CurrentBar](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a2dad625c8cb62d89a65a46b824638b68) is the total number of loaded bars.

 We can get the bar value from the parameters in the overridden [OnCalculate](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a65c50fc1fec7d6c490aa23a1c3eeebd3) method.

 If you specify a bar number less than 0 or greater than `CurrentBar - 1` when calling the [GetCandle(bar)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4e22f2730354b56ffd66559a9c5f27af) method, this will lead to an error and the calculations will be interrupted.

The [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) object contains data that can be obtained from its properties or by calling methods of this object.

 Some properties worth clarifying:

- [LastTime](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a15b797ae49bbf7647514cb36cf66eadb) - time of the last trade on this bar,

- [MaxDelta](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a5496a694ff63727a56de57cca5180664) is the largest delta value that was on this bar,

- [MinDelta](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ab774715f9b730387684841fc6e2e01cd) - the lowest delta value that was on this bar,

- [MaxOI](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#aff03ffc123adbaa07128352297a99c0c) - the largest OI value that was on this bar,

- [MinOI](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6ad1ad8638bb6a2c62a37d45a647ce5c) - the lowest OI value that was on this bar

public class SimpleInd : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 var open = candle.Open;

 var close = candle.Close;

 var high = candle.High;

 var low = candle.Low;

 var volume = candle.Volume;

 var ask = candle.Ask;

 var bid = candle.Bid;

 var delta = candle.Delta;

 // TODO: your code

 }

}

[ATAS](../api/namespaces/namespaceATAS.md)

Definition AsyncConnector.cs:7

# Footprint data

One important data type is bar data at a certain price (footprint data), which is stored in the [PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) object.

 [PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) stores such data as: price, the number of volumes traded at the bid price, ask price, etc.

Here are some ways to get [PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects from an [IndicatorCandle](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md) object:

- [GetPriceVolumeInfo (decimal price)](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ae6a64cd0a12f9ff193102b3fc8a6d3d3) - cluster data at a certain price,

- [GetAllPriceLevels ()](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3b16639dcd407b7c88866fb0d7626179) - cluster data for all prices, in this case we will get a collection of [PriceVolumeInfo](../api/classes/classATAS_1_1Indicators_1_1PriceVolumeInfo.md) objects,

- [MaxVolumePriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6a02e9c11fd7379fa28bac67ebee4c8e) - cluster data at the price of the maximum volume,

- [MaxTickPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a6fe87b7dae24861c3b842149bf0304ad) - cluster data for the price with the most ticks,

- [MaxPositiveDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#a3c4ef67bb640c8570f16c798a90ed6e2) - cluster data on the price with maximum delta,

- [MaxNegativeDeltaPriceInfo](../api/classes/classATAS_1_1Indicators_1_1IndicatorCandle.md#ab322ca90826ea2437379e6101e48496e) - cluster data for the price with minimal delta

public class SimpleInd : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 var high = candle.High;

 var highData = candle.GetPriceVolumeInfo(high);

 if (highData != null)

 {

 var highVolume = highData.Volume;

 // TODO: your code

 }

 var maxVolumeData = candle.MaxVolumePriceInfo;

 if (maxVolumeData != null)

 {

 var maxVolume = maxVolumeData.Volume;

 // TODO: your code

 }

 var allPriceLevels = candle.GetAllPriceLevels();

 foreach ( var level in allPriceLevels )

 {

 if (level == null)

 continue;

 var price = level.Price;

 var volume = level.Volume;

 var delta = level.Ask - level.Bid;

 // TODO: your code

 }

 }

}

# Getting online ticks and aggregated trades

To get the online tick data, it is necessary to redefine the [OnNewTrade(MarketDataArg arg)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a486ed770252ac81006f6bba11ae9c140) method:

public class SampleTick : Indicator

{

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnNewTrade(MarketDataArg arg)

 {

 }

}

API also allows getting aggregated trades. For this, it is necessary to redefine the [OnCumulativeTrade(CumulativeTrade arg)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#ae7a7437439fea1973c7431bd45945f60) method.

 This method will be automatically called when a new cumulative trade appears. Such trades may be subsequently modified. In order to keep track of these changes, you need to override the [OnUpdateCumulativeTrade(CumulativeTrade trade)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#af6133d0303a699a7d8d52c4c6fa89d6e) method.

 Example of realization of the indicator, which outputs the delta of cumulative trades of the volume of more than 3 lots:

public class SampleCumulativeTrades : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 private CumulativeTrade _lastTrade;

 private decimal _lastCumulativeTradeVolume;

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnCumulativeTrade(CumulativeTrade trade)

 {

 AddCumulativeTrade(trade);

 }

 protected override void OnUpdateCumulativeTrade(CumulativeTrade trade)

 {

 AddCumulativeTrade(trade);

 }

 private void AddCumulativeTrade(CumulativeTrade trade)

 {

 if (trade.Volume < 3)

 return;

 if (_lastTrade != trade)

 {

 _lastTrade = trade;

 this[CurrentBar - 1] += GetVolumeByDirection(trade.Volume, trade.Direction);

 }

 else

 {

 this[CurrentBar - 1] += GetVolumeByDirection(trade.Volume - _lastCumulativeTradeVolume, trade.Direction);

 }

 _lastCumulativeTradeVolume = trade.Volume;

 }

 private decimal GetVolumeByDirection(decimal volume, TradeDirection direction)

 {

 return volume * (direction == TradeDirection.Buy ? 1 : -1);

 }

}

To get historical cumulative trades, you need to call the [RequestForCumulativeTrades](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#add114c0bc61b7caa1ea3920d40ee0b62) method, pass the [CumulativeTradesRequest](../api/classes/classATAS_1_1Indicators_1_1CumulativeTradesRequest.md) request to the parameters, and override the [OnCumulativeTradesResponse](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a979770a8f1280d7a41b26a03ea92da4b) method. In the request, you need to specify the beginning and end of the time period for which you want to get cumulative trades, you can also set filters for the minimum and maximum volume of trades. If you specify 0 for the minimum and maximum volumes, then filters by volume will not be applied.

 An example of an indicator that receives data from cumulative trades from the first to the last bar:

public class SampleCumulativeTrades : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 private List<CumulativeTrade> _trades;

 private CumulativeTradesRequest _request;

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnFinishRecalculate()

 {

 var startTime = GetCandle(0).Time;

 var lastTime = GetCandle(CurrentBar - 1).LastTime;

 _request = new CumulativeTradesRequest(startTime, lastTime, 0, 0);

 RequestForCumulativeTrades(_request);

 }

 protected override void OnCumulativeTradesResponse(CumulativeTradesRequest request, IEnumerable<CumulativeTrade> cumulativeTrades)

 {

 if (request != _request)

 return;

 _trades = cumulativeTrades.ToList();

 }

}

# Working with Market Depth

API allows getting the data about updates of the MarketDepth. For this, the [MarketDepthChanged(MarketDataArg arg)](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a532e9f8787219b852bd10f90fd682a36) method should be redefined:

public class SampleMD : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void MarketDepthChanged(MarketDataArg arg)

 {

 }

}

It is also possible to get the total volume of all Bids and total volume of all Asks with the help of the [MarketDepthInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a06e73f90f3cb6842174f775b2c613705) object properties: [CumulativeDomAsks](../api/classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a612f8bb390a79ec0bb01da9963871228) and [CumulativeDomBids](../api/classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#ae1da9888dc3f9bbfada9187facabf56d). Example of the indicator which reflects the history of the total values of the Asks and Bids volumes

public class DomPower : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 private ValueDataSeries _asks;

 private ValueDataSeries _bids = new ValueDataSeries("Bids");

 private int _lastCalculatedBar = 0;

 privte bool _first = true;

 public DomPower() : base(true)

 {

 Panel = IndicatorDataProvider.NewPanel;

 _asks = (ValueDataSeries)DataSeries[0];

 _asks.Name = "Asks";

 _bids.Color = Colors.Green;

 DataSeries.Add(_bids);

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void MarketDepthChanged(MarketDataArg arg)

 {

 if (_first)

 {

 _first = false;

 _lastCalculatedBar = CurrentBar - 1;

 }

 var lastCandle = CurrentBar - 1;

 var cumAsks = MarketDepthInfo.CumulativeDomAsks;

 var cumBids = MarketDepthInfo.CumulativeDomBids;

 for (int i = _lastCalculatedBar; i <= lastCandle; i++)

 {

 _asks[i] = -cumAsks;

 _bids[i] = cumBids;

 }

 _lastCalculatedBar = lastCandle;

 }

}

In some cases it might be necessary to get a snapshot of the MaketDepth data. For this, the [MarketDepthInfo.GetMarketDepthSnapshot()](../api/classes/classATAS_1_1Indicators_1_1MarketDepthInfoProvider.md#a76a61066eaf4c4eafdc31c38b37452fd) function could be used. The function returns a list of all levels in the DOM.

# MBO (Market By Orders)

To date, this data is provided only by the Rhithmic provider.

 To get and analyze MBO (Market By Orders) data, you need to get an instance of an object that implements the [IMarketByOrdersManager](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) interface using the [SubscribeMarketByOrderData](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a1a25f6922ea82bc35ef188fb54083ace) method. This object contains the [MarketByOrders](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#aa16e5582e02892df31b84fc27363a81f) property, which is a collection of objects of type [MarketByOrder](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md). To monitor changes in this collection in real time, you must subscribe to the [Changed](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#ad7f415ab2ad44afe1443f9afd00cb954) event of the [IMarketByOrdersManager](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md) object.

[MarketByOrder](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md#aa16e5582e02892df31b84fc27363a81f) objects have the following properties:

- [Security](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#ae63739f9b55f568eb01c317b48e300ad) - security associated with the market by order entry.

- [Time](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a492b23803210abaff252c6015f70f850) - date and time of the market by order entry.

- [Type](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#aaa322eb2c52aa8be0e7cdc58ae6059e7) - type of market by order update:
[Snapshot](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617ad4e2713d1b1725a1592f9268589f990d) - Indicates that the MBO data is from a cache and not from the real-time data stream.

- [New](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617a03c2e7e41ffc181a4e84080b4710e81e) - Indicates that the MBO data represents a new order added to the order book of this instrument.

- [Change](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617af4ec5f57bd4d31b803312d873be40da9) - Indicates that the MBO data represents a change to an existing order.

- [Delete](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1a710cc738e98a7cc605cb391ff86617af2a6c498fb90ee345d997f888fce3b18) - Indicates that the MBO data represents a deletion of an existing order.

- [Side](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#ab1706f37dd78eecb0143a726927f1dcc) - Specifies the type of market data:
[Bid](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4ae36ba1e187ae2b3ebcfd0a4c68367caf)

- [Ask](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4aa0b271a9d8aa8e7473922164d6a1c03c)

- [Trade](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md#a1aa3babe61bbdf13a9271fd9e95a44f4a5f390d80b20daad8f5d2f483fb0ae9d8)

- [Priority](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a80766904fccdf9c041ada9e5e16be5fc) - Priority of this order in the exchange's matching engine queue.

- [ExchangeOrderId](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a889d5ef746b506ef536c21e6f89f9d04) - Exchange order id of this order.

- [Price](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a76c45d926f6a2b9a7177a92125903ecd) - Price associated with this order.

- [Volume](../api/classes/classATAS_1_1DataFeedsCore_1_1MarketByOrder.md#a141c57af356e8b960de5a7924f0e3025) - Volume associated with this order.

Below is an example of an indicator using [IMarketByOrdersManager](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1IMarketByOrdersManager.md).

public class SampleMBODataIndicator : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

 {

 private IMarketByOrdersManager _manager;

 protected override async void OnInitialize()

 {

 _manager = await SubscribeMarketByOrderData();

 _manager.Changed += Manager_Changed;

 Manager_Changed(_manager.MarketByOrders);

 }

 private void Manager_Changed(System.Collections.Generic.IEnumerable<DataFeedsCore.MarketByOrder> marketByOrders)

 {

 this.LogInfo("new MBO data");

 foreach (var marketByOrder in marketByOrders)

 {

 this.LogInfo("{0}", marketByOrder);

 }

 }

 #region Overrides of ExtendedIndicator

 protected override void OnNewTrade(MarketDataArg trade)

 {

 this.LogInfo($"ExchangeOrderId: {trade.ExchangeOrderId}, AggressorExchangeOrderId: {trade.AggressorExchangeOrderId}");

 }

 #endregion

 protected override void OnDispose()

 {

 base.OnDispose();

 if (_manager != null)

 {

 _manager.Changed -= Manager_Changed;

 }

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 }

# Statistical data

You can get statistical data using the indicator property [TradingStatisticsProvider](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a8f17faea0cd2927486eb77fb218b0eda). Statistical data is stored in an object that implements [ITradingStatistics](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md), which you can retrieve through:

- The [TradingStatisticsProvider.Realtime](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#ababb7b8655b614919db56107efbb89f7) property.

- The asynchronous method TradingStatisticsProvider.LoadAsync(DateTime from, DateTime to, ICollection? accounts = null, ICollection? securities = null).

To get the history of statistics, you need to call the LoadAsync asynchronous method, passing the `start time` and `end time` of the desired period as parameters. You can also pass in the parameters for which accounts and instruments you need data. Statistics data obtained using this method displays statistics for a certain period and is not updated in real time.

 An example of an indicator that logs equity data for the period from the beginning of the chart to the current bar:

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.DataFeedsCore.Statistics](../api/namespaces/namespaceATAS_1_1DataFeedsCore_1_1Statistics.md);

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

using Utils.Common.Collections;

using Utils.Common.Logging;

public class SampleStatistics : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private [ITradingStatistics](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) _tradingStatistics;

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnFinishRecalculate()

 {

 if (_tradingStatistics is null)

 return;

 foreach (var equity in _tradingStatistics.[Equity](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a2737aff6dc45dddf35a4b752e91bab10))

 {

 this.LogInfo($"Time: {equity.Time} | equity: {equity.Equity} | total equity: {equity.TotalEquity}");

 }

 }

 protected override async void OnInitialize()

 {

 var start = GetCandle(0).Time;

 var end = GetCandle(CurrentBar - 1).LastTime;

 _tradingStatistics = await TradingStatisticsProvider.LoadAsync(start, end);

 }

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.DataFeedsCore.Statistics.ITradingStatistics](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md)

Definition ITradingStatistics.cs:10

[ATAS.DataFeedsCore.Statistics.ITradingStatistics.Equity](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md#a2737aff6dc45dddf35a4b752e91bab10)

IMutableEnumerable< EquityValue > Equity

Definition ITradingStatistics.cs:19

[ATAS.DataFeedsCore.Statistics](../api/namespaces/namespaceATAS_1_1DataFeedsCore_1_1Statistics.md)

Definition ITradingStatistics.cs:1

[ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md)

Definition AsyncConnector.cs:7

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

The [ITradingStatistics](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) object contains the following types of statistics:

- Equity the collection of [EquityValue](../api/namespaces/namespaceATAS_1_1DataFeedsCore_1_1Statistics.md#a38271fe8706f1b9675f22ad2d2cf845f)

- HistoryMyTrades the collection of [HistoryMyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md)

- Orders the collection of [Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

- MyTrades the collection of [MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md)

- Statistics the collection of [IStatisticsParameterGroup](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md)

If you get the [ITradingStatistics](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatistics.md) object using [TradingStatisticsProvider.Realtime](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1ITradingStatisticsProvider.md#ababb7b8655b614919db56107efbb89f7), then you can track changes in these properties in real time by subscribing to their events:

- Added

- Changed

- Removed

- Cleared

An example of an indicator that outputs these data to the log when new statistical data appears:

using [ATAS.DataFeedsCore](../api/namespaces/namespaceATAS_1_1DataFeedsCore.md);

using [ATAS.DataFeedsCore.Statistics](../api/namespaces/namespaceATAS_1_1DataFeedsCore_1_1Statistics.md);

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

using Utils.Common.Collections;

using Utils.Common.Logging;

public class SampleStatistics : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 private IMutableEnumerable<HistoryMyTrade> _historyMyTrades;

 private IMutableEnumerable<EquityValue> _equity;

 private IMutableEnumerable<Order> _orders;

 private IMutableEnumerable<MyTrade> _myTrades;

 private IMutableEnumerable<IStatisticsParameterGroup> _statistics;

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 protected override void OnInitialize()

 {

 var tradingStatistics = TradingStatisticsProvider.Realtime;

 _historyMyTrades = tradingStatistics.HistoryMyTrades;

 _historyMyTrades.Added += HistoryMyTrades_Added;

 _equity = tradingStatistics.Equity;

 _equity.Added += Equity_Added;

 _orders = tradingStatistics.Orders;

 _orders.Added += Orders_Added; ;

 _myTrades = tradingStatistics.MyTrades;

 _myTrades.Added += MyTrades_Added;

 _statistics = tradingStatistics.Statistics;

 _statistics.Added += Statistics_Added;

 }

 private void Statistics_Added([IStatisticsParameterGroup](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md) spGroup)

 {

 this.LogInfo($"New {spGroup.Name} data.");

 }

 private void MyTrades_Added([MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md) trade)

 {

 this.LogInfo($"New MyTrade {trade.AccountID} added.");

 }

 private void Orders_Added([Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md) order)

 {

 this.LogInfo($"New Order {order.AccountID} added.");

 }

 private void Equity_Added(EquityValue equity)

 {

 this.LogInfo($"Time: {equity.Time} | equity: {equity.Equity} | total equity: {equity.TotalEquity}");

 }

 private void HistoryMyTrades_Added([HistoryMyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md) trade)

 {

 this.LogInfo($"New HistoryMyTrade {trade.AccountID} added.");

 }

 protected override void OnDispose()

 {

 base.OnDispose();

 if (_historyMyTrades != null)

 _historyMyTrades.Added -= HistoryMyTrades_Added;

 if (_equity != null)

 _equity.Added -= Equity_Added;

 if (_orders != null)

 _orders.Added -= Orders_Added;

 if (_myTrades != null)

 _myTrades.Added -= MyTrades_Added;

 if (_statistics != null)

 _statistics.Added -= Statistics_Added;

 }

}

[ATAS.DataFeedsCore.HistoryMyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1HistoryMyTrade.md)

Represents a historical trade record.

Definition HistoryMyTrade.cs:42

[ATAS.DataFeedsCore.MyTrade](../api/classes/classATAS_1_1DataFeedsCore_1_1MyTrade.md)

Represents a trade entity in the system.

Definition MyTrade.cs:19

[ATAS.DataFeedsCore.Order](../api/classes/classATAS_1_1DataFeedsCore_1_1Order.md)

Represents an order for trading on a financial exchange.

Definition Order.cs:13

[ATAS.DataFeedsCore.Statistics.IStatisticsParameterGroup](../api/interfaces/interfaceATAS_1_1DataFeedsCore_1_1Statistics_1_1IStatisticsParameterGroup.md)

Definition StatisticsParameterGroup.cs:9
