@echo off
setlocal enabledelayedexpansion

REM Equivalente di deploy.sh per chi non ha git-bash. Deve restare allineato a deploy.sh:
REM il 20 settembre 2026 era rimasto indietro su TRE cose insieme, e nessuna dava errore -
REM stampava "deployed" e sul chart non cambiava niente.
REM   - il TFM: net10.0-windows non esiste piu', il target e' net10.0 (ATAS X non ha WPF)
REM   - la cartella: ATAS X carica da "%APPDATA%\ATAS X\Indicators", non da "%APPDATA%\ATAS\Indicators"
REM   - mancava FabioDataBridge, cioe' il bridge, i livelli e il pannello

echo ============================================================
echo  FabioOrderFlow - Build and deploy the Fabio Data Bridge
echo ============================================================
echo.

echo [1/2] Building the Release DLLs...
dotnet build FabioOrderFlow.slnx -c Release
if %ERRORLEVEL% neq 0 (
    echo BUILD FAILED
    exit /b 1
)

echo.
echo [2/2] Copying separate DLLs to ATAS Indicators...
if exist "%APPDATA%\ATAS X" (
    set "ATAS_IND=%APPDATA%\ATAS X\Indicators"
) else (
    set "ATAS_IND=%APPDATA%\ATAS\Indicators"
)
if not exist "!ATAS_IND!" mkdir "!ATAS_IND!"
REM DLL che non si distribuiscono piu': lo scheletro e, dal 10 ottobre 2026, i recorder di agosto.
for %%V in (FabioOrderFlow FabioCumulativeTradeRecorder FabioSessionLocationRecorder FabioHistoricalCumulativeContextRecorder FabioPreSessionProfileRecorder) do (
    if exist "!ATAS_IND!\%%V.dll" del /Q "!ATAS_IND!\%%V.dll"
)

call :copy_indicator "DataBridge" "FabioDataBridge"
if %ERRORLEVEL% neq 0 exit /b 1

echo.
echo FabioDataBridge.dll deployed to !ATAS_IND!.
echo ATAS X reloads it on its own.
exit /b 0

:copy_indicator
copy /Y "Indicators\%~1\bin\Release\net10.0\%~2.dll" "!ATAS_IND!\%~2.dll"
if %ERRORLEVEL% neq 0 (
    echo DEPLOY FAILED for %~2.dll
    exit /b 1
)
exit /b 0
