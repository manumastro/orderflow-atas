@echo off
echo ================================================
echo  Avvio watcher log balance zone (Fabio)
echo ================================================
echo.
echo Doppio clic su questo file per avviare
echo il mirroring in tempo reale del log.
echo.
echo Il log aggiornato sara visibile in:
echo   logs\FabioBalanceZone.log
echo.
echo Per fermare: chiudi la finestra PowerShell
echo              oppure premi Ctrl+C.
echo.
pause

powershell -NoExit -ExecutionPolicy Bypass -Command "cd '%~dp0'; .\watch-balance-log.ps1"