@echo off
echo ================================================
echo  FabioOrderFlow — Deploy to ATAS
echo  London Mean Reversion + Post-London Impulse
echo ================================================
echo.

echo [1/2] Building Release...
dotnet build -c Release
if %ERRORLEVEL% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo [2/2] Copying DLL to ATAS Indicators...
set "ATAS_IND=%APPDATA%\ATAS\Indicators"
if not exist "%ATAS_IND%" mkdir "%ATAS_IND%"
copy /Y "bin\Release\net10.0-windows\FabioOrderFlow.dll" "%ATAS_IND%\"

echo.
echo Done! DLL: %ATAS_IND%\FabioOrderFlow.dll
echo Timestamp:
dir "%ATAS_IND%\FabioOrderFlow.dll" | findstr "FabioOrderFlow.dll"
echo.
echo IMPORTANT: Remove indicator from chart, restart ATAS, then re-add indicator.
echo.
pause
