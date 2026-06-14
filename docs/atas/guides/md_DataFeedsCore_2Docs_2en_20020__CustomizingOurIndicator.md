# Customizing an indicator

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20020__CustomizingOurIndicator.html

# External indicator parameters

In order to be able to change one or another parameter in the indicator, it should be realized in the form of a public property:

public int Period { get; set; }

In order for the indicator to recalculate when the property changes, it is necessary to call the [RecalculateValues()](../api/classes/classATAS_1_1Indicators_1_1BaseIndicator.md#a6ee783f61e21eb5ff39be738b77dbd1e) method.

private int _size = 10;

public int Size

 {

 get { return _size; }

 set

 {

 _size = value;

 RecalculateValues();

 }

 }

The `Display` attribute, where the displayed name and category and the property sequence number are specified, could be set for every property. This attribute is located in `System.ComponentModel.DataAnnotations`.

public class SampleIndicator : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 [Display(GroupName = "GroupName", Name = "PropertyName", Order = 10)]

 public int Type { get; set; }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}

[ATAS](../api/namespaces/namespaceATAS.md)

Definition AsyncConnector.cs:7

# Parameter types

Examples of using different types of indicator properties are given in the indicator [Properties](https://github.com/AtasPlatform/Indicators/blob/Develop/Technical/SampleProperties.cs)

## Value types

Value types like `int`, `decimal`, `boolean`, `enum`, `string`, etc.

 Here are some examples with parameters of these types:

[Display(Name = "Integer", GroupName = "Examples")]

public int Integer { get; set; }

[Display(Name = "Decimal", GroupName = "Examples")]

public decimal Decimal { get; set; }

[Display(Name = "Boolean", GroupName = "Examples")]

public bool Boolean { get; set; }

[Display(Name = "Heatmap", GroupName = "Examples")]

public HeatmapTypes HeatmapType { get; set; }

[Display(Name = "Date", GroupName = "Examples")]

public [DateTime](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) [DateTime](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e) { get; set; } = new(2020, 01, 01);

[Display(Name = "Label", GroupName = "Examples")]

public string Label { get; set; } = "Some Label";

[OFT.Attributes.Editors.MaskTypes.DateTime](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7a8cf10d2341ed01492506085688270c1e)

@ DateTime

Value types properties

## Filters

Derived types from the [Filter](../api/classes/classATAS_1_1Indicators_1_1Filter.md) class. Such types allow you to implement in one property the ability to store data and control access to this data.

 These properties must be initialized immediately. Objects derived from the [Filter](../api/classes/classATAS_1_1Indicators_1_1Filter.md) class have an event [PropertyChanged](../api/classes/classATAS_1_1Indicators_1_1NotifyPropertyChangedBase.md#a0dc11be741c756c9a36c0756d2f5527a), by subscribing to which you can track changes in the values in this property.

public class SampleIndicator : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 [Display(Name = "Filter Decimal", GroupName = "Examples")]

 public Filter FilterDecimal { get; set; } = new();

 [Display(Name = "Filter Integer", GroupName = "Examples")]

 public FilterInt FilterInt { get; set; } = new();

 [Display(Name = "Filter Text", GroupName = "Examples")]

 public FilterString FilterText { get; set; } = new() { Enabled = true, [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = "Value" };

 public SampleIndicator() : base(true)

 {

 DenyToChangePanel = true;

 DataSeries[0].IsHidden = true;

 FilterDecimal.PropertyChanged += OnFilterPropertyChanged;

 FilterInt.PropertyChanged += OnFilterPropertyChanged;

 FilterText.PropertyChanged += OnFilterPropertyChanged;

 }

 private void OnFilterPropertyChanged(object? sender, PropertyChangedEventArgs e)

 {

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}

[ATAS.Indicators.DataSeriesType.Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c)

@ Value

Represents a value-based data series.

Filter properties

## Collections

In order to store collections in indicator properties, it is recommended to use [ObservableCollection](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=net-7.0). To correctly serialize such properties, you must use the [JsonPropertyAttribute](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/JsonPropertyAttribute.cs).

 You can add and remove elements to properties of a collection type.

public class SampleIndicator : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 [Display(Name = "Numbers", GroupName = "Examples")]

 [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]

 public ObservableCollection<decimal> Numbers { get; set; } = new()

 { 1.0m, 2.0m, 3.0m };

 [Display(Name = "Filters", GroupName = "Examples")]

 [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]

 public ObservableCollection<Filter> Filters { get; set; } = new();

 [Display(Name = "Colors", GroupName = "Examples")]

 [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]

 public ObservableCollection<Color> ColorsSource { get; set; } = new()

 { Color.Red, Color.Green, Color.Blue };

 public SampleIndicator() : base(true)

 {

 DenyToChangePanel = true;

 DataSeries[0].IsHidden = true;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}

Collection type properties

# Advanced parameter settings

Additional attributes can be used to give your parameters more customization. Attribute names can be written without the `Attribute` suffix. For example: `DisplayAttribute` = `Display`

 Some of the attributes are described here.

[DisplayAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute?view=net-7.0) - using this attribute, you can set the parameter's display name, group name, ordinal number, and more.

 [NumericEditorAttribute](../api/classes/classOFT_1_1Attributes_1_1Editors_1_1NumericEditorAttribute.md) - used for numeric parameter settings. In the constructor of this attribute, you can specify the minimum value, maximum value, price step, and display format.

 [ComboBoxEditorAttribute](../api/classes/classOFT_1_1Attributes_1_1Editors_1_1ComboBoxEditorAttribute.md) - a property with this attribute looks like a drop-down menu.

using [OFT.Attributes.Editors](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md);

public class Entity

{

 #region Properties

 public int Value { get; set; }

 public string Name { get; set; }

 #endregion

}

[DisplayName("Simple Indicator")]

public class SimpleInd : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.[Indicator](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da330d9b3991e7785c21cd29a452b56219)

{

 private class EntitiesSource : Collection<Entity>

 {

 #region ctor

 public EntitiesSource()

 : base(new[]

 {

 new Entity { [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 1, Name = "Entity 1" },

 new Entity { [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 2, Name = "Entity 2" },

 new Entity { [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 3, Name = "Entity 3" },

 new Entity { [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 4, Name = "Entity 4" },

 new Entity { [Value](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da689202409e48743b914713f96d93947c) = 5, Name = "Entity 5" }

 })

 {

 }

 #endregion

 }

 [Display(Name = "Selector", GroupName = "Examples")]

 [ComboBoxEditor(typeof(EntitiesSource), DisplayMember = nameof(Entity.Name), ValueMember = nameof(Entity.Value))]

 public int? Selector { get; set; }

 [Display(Name = "Decimal", GroupName = "Examples")]

 [NumericEditor(0.0, 100.0, Step = 0.5, DisplayFormat = "F2")]

 public decimal Decimal { get; set; }

 public SimpleInd() : base(true)

 {

 DenyToChangePanel = true;

 DataSeries[0].IsHidden = true;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 }

}

[ATAS.Indicators.DataSeriesType.Indicator](../api/namespaces/namespaceATAS_1_1Indicators.md#a917f1fe318fb64330165d772c7d7415da330d9b3991e7785c21cd29a452b56219)

@ Indicator

Represents a data series sourced from an indicator.

[OFT.Attributes.Editors](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md)

Definition CheckEditorAttribute.cs:1

[RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute?view=net-7.0) - used for numeric parameters and sets the minimum and maximum values.

 [DisplayFormat](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayformatattribute?view=net-7.0) - specifies how data fields are displayed and formatted.

[Display(Name = "Filter Decimal", GroupName = "Examples")]

[Range(-100, 100)]

[DisplayFormat(DataFormatString = "##0.0##")]

[RegularExpressionAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.regularexpressionattribute?view=net-7.0) - specifies that a data field value must match the specified regular expression.

 [MaskAttribute](../api/classes/classOFT_1_1Attributes_1_1Editors_1_1MaskAttribute.md) - when entering a value in property fields with this attribute, the editor checks the text against the mask and only allows data that satisfies the mask expression to be entered.

[Display(Name = "E-mail", GroupName = "Examples")]

[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]

public string Email { get; set; }

[Display(Name = "Time Span", GroupName = "Examples")]

[Mask([MaskTypes](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7).DateTimeAdvancingCaret, "HH:mm:ss")]

public TimeSpan TimeSpan { get; set; } = new(1, 0, 0);

[OFT.Attributes.Editors.MaskTypes](../api/namespaces/namespaceOFT_1_1Attributes_1_1Editors.md#aca3be0855f2aece4d1906350ef314cf7)

MaskTypes

Definition MaskTypes.cs:4

[BrowsableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.browsableattribute?view=net-7.0) - with the `false` parameter allows you to not display a property with this attribute in the settings menu.

[Browsable(false)]

public int NoBrowsable { get; set; }

# Indicator location in a separate panel

Indicator could be displayed in the chart area or in a separate panel. By default, indicator is displayed in the chart area. If you want an indicator to be displayed in a new panel by default, it is necessary to specify it in the constructor.

public MyCustomIndicator()

{

 Panel = IndicatorDataProvider.NewPanel;

}

It is also possible to prohibit a user from changing the chart location from the interface. To do it, it is necessary to put the [DenyToChangePanel](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a71700e9c11b8ce894e64a4e7dff0885c) flag in the constructor.

public MyCustomIndicator()

{

 DenyToChangePanel = true;

 Panel = IndicatorDataProvider.NewPanel;

}

# Unified style

Each applied indicator can demonstrate the results of its work on the chart by displaying lines, labels, histograms and other visual elements of certain colors. When developing an individual indicator, it is possible to choose a wide range of colors. However, to ensure a consistent style of plots with user-supplied settings, it is recommended to use the predefined colors contained in the [ColorsStore](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#ade75579aa63f898e84c8231c73ec1b90) object. In this case, the developer does not need to know what color should be set to a particular property. These are the colors that are set in the chart settings.

You can get the [ColorsStore](../api/interfaces/interfaceATAS_1_1Indicators_1_1IChart.md#ade75579aa63f898e84c8231c73ec1b90) object from the indicator's [ChartInfo](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a52e91039466de7c2218a0248e585a259) property.

 Let's give an example of an indicator that will display the delta of candles in the form of a histogram.

public class SampleIndicator : [ATAS](../api/namespaces/namespaceATAS.md).Indicators.Indicator

{

 public SampleIndicator() : base(true)

 {

 Panel = IndicatorDataProvider.NewPanel;

 DataSeries[0].IsHidden = true;

 ((ValueDataSeries)DataSeries[0]).VisualType = VisualMode.Histogram;

 }

 protected override void OnCalculate(int bar, decimal value)

 {

 var candle = GetCandle(bar);

 DataSeries[0][bar] = candle.Delta;

 if(ChartInfo is not null)

 {

 ((ValueDataSeries)DataSeries[0]).Colors[bar] = candle.Delta < 0

 ? ChartInfo.ColorsStore.DownCandleColor

 : ChartInfo.ColorsStore.UpCandleColor;

 }

 }

}

# Identification of the moment of a new session, new week and new month

In order to identify whether a new session has started on a specific bar, it is necessary to call the [IsNewSession(int bar)](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#a66714bf4c34e2e3741d6c19058e1d641) function and pass the bar number to it. Similarly there are [IsNewWeek(int bar)](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#aeace7ec9e0967c0ec47e7085652dda88) and [IsNewMonth(int bar)](../api/classes/classATAS_1_1Indicators_1_1Indicator.md#ace949df3beb307acaed6bcf521d5bcbe) functions.

# Indicator alerts

It is possible to use the following function to call an alert from an indicator

AddAlert(string soundFile, string instrument, string message, Color background, Color foreground)

soundFile - the path to the .wav file, which should be played upon occurrence of an event instrument - the name of the instrument, which will be shown in the window with alerts message - the message background - the message background foreground - the message text colour There is also a simplified mechanism of calling alerts, where the background and text colours are default ones.

AddAlert(string soundFile, string message)

# Adding information to the indicator description

You can add a description and a website link to your indicator. Both will appear in the indicator settings window on the “About” tab.

## Adding a description

To add a description, use the [DisplayAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute?view=net-7.0) and specify the text in the `Description` property:

[Display(Description = "Test Indicator with Custom Drawing.")]

## Adding a website link

To add a link to a webpage with detailed information about your indicator, use the [HelpLinkAttribute](../api/classes/classOFT_1_1Attributes_1_1HelpLinkAttribute.md):

[HelpLink("https://your-site.com/indicators/test-indicator")]

Example

using [OFT.Attributes](../api/namespaces/namespaceOFT_1_1Attributes.md);

using System.ComponentModel.DataAnnotations;

[DisplayName("Test Indicator")]

[HelpLink("https://your-site.com/indicators/test-indicator")]

[Display(Description = "Test Indicator with Custom Drawing")]

public class TestInd : Indicator

{

 // ...

}

[OFT.Attributes](../api/namespaces/namespaceOFT_1_1Attributes.md)

Definition AttributeResourceHelpers.cs:1

 After compiling the project and placing the DLL in the indicators folder, this information will automatically appear on the “About” tab in the indicator settings window.

About tab with indicator description.
