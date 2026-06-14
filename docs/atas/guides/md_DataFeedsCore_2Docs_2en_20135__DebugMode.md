# Debug mode

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20135__DebugMode.html

# Basics

A good helper in the development of indicators is the debug mode. Here we will talk about attaching to the running process of the [ATAS](../api/namespaces/namespaceATAS.md) platform using the Visual Studio debugger.

 While working in this mode, the developer can step through the code sequentially, analyze the data stored in variables, set variable monitoring to track changes, determine where exceptions occur, and so on.

 Before using this mode, you should learn more about its principles of operation [here](https://learn.microsoft.com/en-us/visualstudio/debugger/debugger-feature-tour?view=vs-2022).

To attach the process, you need to do the following steps:

- compile the file with the indicator and place it in the `APPDATA%\[ATAS](../api/namespaces/namespaceATAS.md)\Indicators` folder,

- in Visual Studio, press the key combination `CTRL` + `ALT` + `P`,

- in the window that appears with a list of processes running on the computer, enter the name of the desired process in the search field, in this case it is platform and click on the Attach button or double-click on the desired process.

Process attachment

# Breakpoints

Using such a debugger tool as a breakpoint, you can pause the execution of the process at the desired line of code. Using the `F10` and `F11` keys, you can go through the code line by line and track the path of the process, while finding out the values in the variables.

 `F11` (Step Into) - move through the lines of code, entering methods.

 `F10` (Step Over) - move through lines of code without going into methods.

If you need to continue the process - you need to click on the Continue button, in this case the process will continue to the next breakpoint. If for some time you no longer need to stop the process, you need to remove the breakpoints by clicking on them again. To exit debug mode, press the Stop button.

Debugger window

# Errors

You can find the appearance of errors in the platform log window. Errors throw [Exceptions](https://learn.microsoft.com/en-us/dotnet/api/system.exception?view=net-7.0) during code execution.

 Having found an exception in the debugger, you can find out the reason for this exception, perhaps values in some variables are invalid for calculation.

 A few tips for troubleshooting:

- if errors are repeated at every tick - when attaching to a platform process, the debugger often stops at the place where the exception occurred,

- if the error occurred one or more times - to reproduce this exception in the debugger, update the chart to cause the indicator to be recalculated.

In the screenshot below, you can see an example of throwing an exception when we try to call the [GetCandle](../api/classes/classATAS_1_1Indicators_1_1ExtendedIndicator.md#a4e22f2730354b56ffd66559a9c5f27af) method and pass a negative value to the parameters. By hovering over a variable, you can see its current value. In this case, the cursor is hovering over the `prevBar` variable, and you can see in the tooltip that it has a value of `-1`.

An example of throwing an exception
