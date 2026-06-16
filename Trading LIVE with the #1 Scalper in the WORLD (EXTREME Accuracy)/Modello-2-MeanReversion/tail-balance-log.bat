@echo off
title Tail Log Balance Zone - ATAS (origine)

echo ================================================
echo  TAIL LIVE LOG BALANCE ZONE (da ATAS)
echo ================================================
echo.
echo File origine:
echo   %APPDATA%\ATAS\Logs\FabioBalanceZone.log
echo.
echo Mostra le ultime righe e segue gli aggiornamenti in tempo reale.
echo.
echo Per uscire: premi Ctrl + C
echo ================================================
echo.

:: Crea la directory se non esiste (per sicurezza)
if not exist "%APPDATA%\ATAS\Logs" mkdir "%APPDATA%\ATAS\Logs"

:: Se il file non esiste ancora, crea uno stub vuoto
if not exist "%APPDATA%\ATAS\Logs\FabioBalanceZone.log" (
    echo. > "%APPDATA%\ATAS\Logs\FabioBalanceZone.log"
    echo [INFO] File di log creato (vuoto per ora).
)

:: Esegui il tail con PowerShell (mostra ultime 40 righe + follow)
powershell -NoExit -Command ^
    "Get-Content -Path \"$env:APPDATA\ATAS\Logs\FabioBalanceZone.log\" -Wait -Tail 40 -Encoding UTF8"