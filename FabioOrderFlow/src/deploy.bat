@echo off
setlocal

echo ============================================================
echo  FabioOrderFlow - Build and deploy separate research indicators
echo ============================================================
echo.

echo [1/2] Building four separate Release DLLs...
dotnet build FabioOrderFlow.slnx -c Release
if %ERRORLEVEL% neq 0 (
    echo BUILD FAILED
    exit /b 1
)

echo.
echo [2/2] Copying separate DLLs to ATAS Indicators...
set "ATAS_IND=%APPDATA%\ATAS\Indicators"
if not exist "%ATAS_IND%" mkdir "%ATAS_IND%"
if exist "%ATAS_IND%\FabioOrderFlow.dll" del /Q "%ATAS_IND%\FabioOrderFlow.dll"

call :copy_indicator "CumulativeTrade" "FabioCumulativeTradeRecorder"
if %ERRORLEVEL% neq 0 exit /b 1
call :copy_indicator "SessionLocation" "FabioSessionLocationRecorder"
if %ERRORLEVEL% neq 0 exit /b 1
call :copy_indicator "HistoricalCumulativeContext" "FabioHistoricalCumulativeContextRecorder"
if %ERRORLEVEL% neq 0 exit /b 1
call :copy_indicator "PreSessionProfile" "FabioPreSessionProfileRecorder"
if %ERRORLEVEL% neq 0 exit /b 1

echo.
echo Four separate DLLs deployed to %ATAS_IND%.
echo Restart ATAS or add each DLL through Add custom indicator.
exit /b 0

:copy_indicator
copy /Y "Indicators\%~1\bin\Release\net10.0-windows\%~2.dll" "%ATAS_IND%\%~2.dll"
if %ERRORLEVEL% neq 0 (
    echo DEPLOY FAILED for %~2.dll
    exit /b 1
)
exit /b 0
