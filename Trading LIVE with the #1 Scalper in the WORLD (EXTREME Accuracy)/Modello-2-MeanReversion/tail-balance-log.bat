@echo off
title Tail Log Balance Zone - ATAS (origine)

echo ================================================
echo  TAIL LIVE LOG BALANCE ZONE (da ATAS)
echo ================================================
echo.
echo File origine (giornaliero):
echo   %%APPDATA%%\ATAS\Logs\FabioBalanceZone_yyyy-MM-dd.log
echo.
echo Mostra le ultime righe e segue gli aggiornamenti in tempo reale.
echo.
echo Per uscire: premi Ctrl + C nella finestra PowerShell che si apre.
echo ================================================
echo.

:: Assicura directory
if not exist "%APPDATA%\ATAS\Logs" mkdir "%APPDATA%\ATAS\Logs" >nul 2>&1

echo Avvio PowerShell per il tail...
echo.

:: Lancia una NUOVA finestra PowerShell: prende il file del giorno corrente
start "Tail Balance Log" powershell -NoExit -Command "& { $d = Get-Date -Format 'yyyy-MM-dd'; $p = \"$env:APPDATA\ATAS\Logs\FabioBalanceZone_$d.log\"; if (-not (Test-Path $p)) { New-Item -ItemType File -Path $p -Force | Out-Null }; Get-Content -LiteralPath $p -Wait -Tail 50 -Encoding UTF8 }"

echo Finestra PowerShell aperta. Chiudi questa se vuoi.
timeout /t 3 >nul
