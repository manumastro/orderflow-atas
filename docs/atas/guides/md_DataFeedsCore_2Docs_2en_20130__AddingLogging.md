# Adding logging

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20130__AddingLogging.html

In order to use the standard mechanism of logging of the platform, the Utils.Common.dll file (which is in the program folder) and `using Utils.Common.Logging` should be added to the indicator/strategy project.

 After this, the extensions of this library (LogDebug, LogInfo, LogWarn and LogError) could be used.

 Each extension creates a log with a certain significance level (Debug, Info,Warning and Error). These logs are recorded in the application logs are displayed in the application log window.

Example of use:

using [ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md);

public class SampleTick : [Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

{

 protected override void OnCalculate(int bar, decimal value)

 {

 this.LogDebug("Debug message");

 this.LogInfo("Info message");

 this.LogWarn("Warn message");

 try

 {

 //your code

 }

 catch (Exception e)

 {

 this.LogError("Error message",e);

 }

 }

}

[ATAS.Indicators.Indicator](../api/classes/classATAS_1_1Indicators_1_1Indicator.md)

Base class for custom indicators.

Definition Indicator.cs:44

[ATAS.Indicators](../api/namespaces/namespaceATAS_1_1Indicators.md)

Definition FeatureId.cs:2
