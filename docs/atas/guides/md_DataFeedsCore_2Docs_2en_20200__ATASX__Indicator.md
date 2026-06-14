# Developing indicators for ATAS X

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20200__ATASX__Indicator.html

# Overview

ATAS X is a cross-platform version of the ATAS trading platform that runs natively on both Windows and macOS. Unlike the classic ATAS, which is built on WPF (Windows Presentation Foundation) and runs only on Windows, ATAS X uses cross-platform technologies, which imposes certain additional requirements on custom indicator assemblies.

# Automatic type conversion

To make the transition to ATAS X as seamless as possible, we have implemented automatic conversion of Windows-specific types to their cross-platform equivalents. This means that most indicators originally developed for the classic ATAS will work on ATAS X without any code modifications.

For example, types from the `System.Windows.Media` namespace (such as `Color`, `SolidColorBrush`, etc.) are automatically mapped to cross-platform counterparts at runtime. You do not need to change your existing code or add conditional compilation — the platform handles this transparently.

# Limitation: custom WPF editors

Despite the automatic conversion of most types, there is one significant limitation — custom WPF editors.

If your indicator assembly contains custom property editors built using WPF controls (e.g., custom `UserControl` elements used in the indicator settings window), these cannot be automatically converted to cross-platform equivalents. WPF is a Windows-only UI framework, and there is no way to automatically translate arbitrary WPF visual components into a cross-platform representation.

If your assembly includes custom WPF editors, it will not work on ATAS X automatically.

# Troubleshooting

If your indicator does not contain custom WPF editors but still does not work on ATAS X, please contact our technical support. Send us the DLL file or the source code of your indicator, and we will help resolve the issue.
