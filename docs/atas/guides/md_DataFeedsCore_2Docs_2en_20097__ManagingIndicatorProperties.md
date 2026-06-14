# Categories and Property Management in Indicator Settings

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20097__ManagingIndicatorProperties.html

This indicator is intended to demonstrate how you can control the collapse and expansion of categories and properties to provide the user with a flexible and convenient interface for managing objects.

Main functions:

- Category management: The `CategoryManager` property controls the collapse/expand state for the category named `Managed category`.

- Property management: The `PropertyManager` property controls the collapse/expand state of the `ExpandableProperty` property, which is represented by the `PenSettings` type.

Main properties:

- `CategoryManager` - controls whether the `Managed category` is expanded in the editor.

- `PropertyManager` - controls whether the `ExpandableProperty` is expanded in the editor.

- `ExpandableProperty` - managed property.

- `ManagedProperty1` and `ManagedProperty2` - properties belonging to the `Managed category`.

Steps to create an indicator:

- Inherit your indicator from the [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md) class and implement the [IPropertiesEditorOwner](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) interface so that your indicator can interact with the property editor:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

using OFT.Rendering.Settings;

using System.ComponentModel;

using System.ComponentModel.DataAnnotations;

using System.Drawing;

[DisplayName("Expand and collapse categories or properties")]

public class ExpandAndCollapseCategoriesOrPropertiesIndicator : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md), [IPropertiesEditorOwner](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md)

{

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.Indicators.IPropertiesEditorOwner](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md)

Definition IPropertiesEditorOwner.cs:4

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2

- Define private fields to store the state of categories and properties, and to work with the property editor object:

private [IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) _propertiesEditor;

private bool _categoryManager;

private bool _propertyManager;

[ATAS.Indicators.IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)

Definition IPropertiesEditor.cs:8

- Add public properties for grouping and display in the property editor:

[Display(Name = "Category", GroupName = "Management")]

public bool CategoryManager

{

 get => _categoryManager;

 set => SetProperty(ref _categoryManager, value, () => _propertiesEditor?.SetIsExpandedCategory(_managedCategoryCategoryName, value));

}

[Display(Name = "Property", GroupName = "Management")]

public bool PropertyManager

{

 get => _propertyManager;

 set => SetProperty(ref _propertyManager, value, () => _propertiesEditor?.SetIsExpandedProperty(nameof(ExpandableProperty), value));

}

 Use [SetProperty](../api/classes/classATAS_1_1Indicators_1_1Filters_1_1TrackedPropertyBase.md) to notify about changes and perform specific actions when a property's state changes.

- In this example, using the `CategoryManager` property, we will manage a group of properties such as `ManagedProperty1` and `ManagedProperty2`, and using the `PropertyManager` property, we will manage the `ExpandableProperty` property of the `PenSettings` type, which has several properties for configuration.

[Display(Name = "Expandable property", GroupName = "Custom class properties")]

public PenSettings ExpandableProperty { get; set; } = new();

[Display(Name = "Managed Property 1", GroupName = _managedCategoryCategoryName)]

public int ManagedProperty1 { get; set; }

[Display(Name = "Managed Property 2", GroupName = _managedCategoryCategoryName)]

public int ManagetProperty2 { get; set; }

- Implement the [IPropertiesEditorOwner](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md) property, which will store the editor object and handle its changes:

[Browsable(false)]

 [IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) IPropertiesEditorOwner.PropertiesEditor

 {

 get => _propertiesEditor;

 set

 {

 if (_propertiesEditor == value)

 return;

 _propertiesEditor = value;

 PropertiesEditorOnChanged(value);

 }

 }

- Implement the `PropertiesEditorOnChanged` method, which will update the state of the property editor when it changes:

private void PropertiesEditorOnChanged([IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? newValue)

{

 newValue?.[BeginInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a3edac77a6ee218d221f107e1ba8b308d)();

 newValue?.[SetIsExpandedCategory](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a309a0718abd12b1acd1baeec0c115e04)(_managedCategoryCategoryName, CategoryManager);

 newValue?.[SetIsExpandedProperty](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ac948316bbc13f15cbd2f0ece479c2d7f)(nameof(ExpandableProperty), PropertyManager);

 newValue?.[EndInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a6e21a08b49908185288c444b1bd12f5f)();

}

[ATAS.Indicators.IPropertiesEditor.SetIsExpandedCategory](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a309a0718abd12b1acd1baeec0c115e04)

void SetIsExpandedCategory(string? categoryName, bool isExpanded)

[ATAS.Indicators.IPropertiesEditor.BeginInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a3edac77a6ee218d221f107e1ba8b308d)

void BeginInit()

[ATAS.Indicators.IPropertiesEditor.EndInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a6e21a08b49908185288c444b1bd12f5f)

Task EndInit()

[ATAS.Indicators.IPropertiesEditor.SetIsExpandedProperty](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ac948316bbc13f15cbd2f0ece479c2d7f)

void SetIsExpandedProperty(string? propertyName, bool isExpanded)

Categories and properties are managed in this method between calls to the `BeginInit` and `EndInit` editor methods.

To manage categories, we call the `SetIsExpandedCategory` editor method and pass the name of the managed category as the first argument and the `CategoryManager` control property as the second argument. To manage a property, we call the `SetIsExpandedProperty` editor method and pass the name of the managed property as the first argument and the `PropertyManager` control property as the second argument.

This is how our indicator settings window will look when the managed group and property are collapsed:

Group and property are collapsed

 If you check the `Category` property, the `Managed category` group will expand.

The group has been expanded

 If you check the `Property` property, the `Expandable property` will expand and you will have access to its properties, such as `Color`, `Line style`, `Width`.

The property has been expanded

 Below is the complete code for this indicator:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

using OFT.Rendering.Settings;

using System.ComponentModel;

using System.ComponentModel.DataAnnotations;

using System.Drawing;

[DisplayName("Expand and collapse categories or properties")]

public class ExpandAndCollapseCategoriesOrPropertiesIndicator : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md), [IPropertiesEditorOwner](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditorOwner.md)

{

 #region Fields

 private const string _managedCategoryCategoryName = "Managed category";

 private [IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) _propertiesEditor;

 private bool _categoryManager;

 private bool _propertyManager;

 #endregion

 #region Properties

 [Display(Name = "Category", GroupName = "Management")]

 public bool CategoryManager

 {

 get => _categoryManager;

 set => SetProperty(ref _categoryManager, value, () => _propertiesEditor?.SetIsExpandedCategory(_managedCategoryCategoryName, value));

 }

 [Display(Name = "Property", GroupName = "Management")]

 public bool PropertyManager

 {

 get => _propertyManager;

 set => SetProperty(ref _propertyManager, value, () => _propertiesEditor?.SetIsExpandedProperty(nameof(ExpandableProperty), value));

 }

 [Display(Name = "Expandable property", GroupName = "Custom class properties")]

 public PenSettings ExpandableProperty { get; set; } = new();

 [Display(Name = "Managed Property 1", GroupName = _managedCategoryCategoryName)]

 public int ManagedProperty1 { get; set; }

 [Display(Name = "Managed Property 2", GroupName = _managedCategoryCategoryName)]

 public int ManagetProperty2 { get; set; }

 #endregion

 #region ctor

 public ExpandAndCollapseCategoriesOrPropertiesIndicator() : base(true)

 {

 DenyToChangePanel = true;

 DataSeries[0].IsHidden = true;

 ExpandableProperty.Color = Color.Red.Convert();

 }

 #endregion

 #region Protected methods

 protected override void OnCalculate(int bar, decimal value)

 {

 }

 #endregion

 #region Implementation of IPropertiesEditorOwner

 [Browsable(false)]

 [IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md) IPropertiesEditorOwner.PropertiesEditor

 {

 get => _propertiesEditor;

 set

 {

 if (_propertiesEditor == value)

 return;

 _propertiesEditor = value;

 PropertiesEditorOnChanged(value);

 }

 }

 private void PropertiesEditorOnChanged([IPropertiesEditor](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md)? newValue)

 {

 newValue?.[BeginInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a3edac77a6ee218d221f107e1ba8b308d)();

 newValue?.[SetIsExpandedCategory](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a309a0718abd12b1acd1baeec0c115e04)(_managedCategoryCategoryName, CategoryManager);

 newValue?.[SetIsExpandedProperty](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#ac948316bbc13f15cbd2f0ece479c2d7f)(nameof(ExpandableProperty), PropertyManager);

 newValue?.[EndInit](../api/interfaces/interfaceATAS_1_1Indicators_1_1IPropertiesEditor.md#a6e21a08b49908185288c444b1bd12f5f)();

 }

 #endregion

}
