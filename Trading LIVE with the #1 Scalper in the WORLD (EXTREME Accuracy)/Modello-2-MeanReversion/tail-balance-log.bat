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
echo Per uscire: premi Ctrl + C nella finestra PowerShell che si apre.
echo ================================================
echo.

:: Assicura directory e file
if not exist "%APPDATA%\ATAS\Logs" mkdir "%APPDATA%\ATAS\Logs" >nul 2>&1
if not exist "%APPDATA%\ATAS\Logs\FabioBalanceZone.log" (
    type nul > "%APPDATA%\ATAS\Logs\FabioBalanceZone.log"
)

echo Avvio PowerShell per il tail...
echo.

:: Lancia una NUOVA finestra PowerShell pulita con il tail
:: Usiamo start per aprire una finestra separata (piu' stabile)
start "Tail Balance Log" powershell -NoExit -Command ^
    "& { Get-Content -LiteralPath \"$env:APPDATA\ATAS\Logs\FabioBalanceZone.log\" -Wait -Tail 50 -Encoding UTF8 }"

echo Finestra PowerShell aperta. Chiudi questa se vuoi.
timeout /t 3 >nul
