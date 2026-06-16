@echo off
echo ================================================
echo  FabioMeanReversion — Deploy to ATAS
echo  Model 2: Mean Reversion (London Session)
echo ================================================
echo.

echo [0/2] Cleaning old balance logs...
if exist "%APPDATA%\ATAS\Logs\FabioBalanceZone*.log" (
    del /Q "%APPDATA%\ATAS\Logs\FabioBalanceZone*.log" >nul 2>&1
    echo       Old logs deleted.
) else (
    echo       No old logs found.
)
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
copy /Y "bin\Release\net10.0-windows\FabioMeanReversion.dll" "%ATAS_IND%\"

echo.
echo Done! DLL: %ATAS_IND%\FabioMeanReversion.dll
echo.
pause
