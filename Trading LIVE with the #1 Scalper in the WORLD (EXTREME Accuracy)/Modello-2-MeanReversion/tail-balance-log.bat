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
echo Per uscire: premi Ctrl + C nella finestra PowerShell
echo ================================================
echo.

:: Assicura che la cartella esista
if not exist "%APPDATA%\ATAS\Logs" mkdir "%APPDATA%\ATAS\Logs" >nul 2>&1

:: Crea il file se non esiste (stub)
if not exist "%APPDATA%\ATAS\Logs\FabioBalanceZone.log" (
    type nul > "%APPDATA%\ATAS\Logs\FabioBalanceZone.log"
    echo [INFO] File di log creato.
)

echo Avvio tail in tempo reale...
echo.

:: Versione su UNA SOLA RIGA per evitare problemi di escaping con ^
powershell -NoExit -Command "& { Get-Content -LiteralPath \"$env:APPDATA\ATAS\Logs\FabioBalanceZone.log\" -Wait -Tail 50 -Encoding UTF8 }"

echo.
echo Tail terminato.
pause >nul