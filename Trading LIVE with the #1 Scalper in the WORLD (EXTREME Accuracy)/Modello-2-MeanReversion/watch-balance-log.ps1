# watch-balance-log.ps1
# Real-time mirror of the ATAS balance zone log into this project folder.
#
# === COME ESEGUIRLO (il modo PIU' SEMPLICE) ===
# 1. Vai nella cartella Modello-2-MeanReversion
# 2. Fai doppio clic su "avvia-watcher.bat"
#    (si aprirà PowerShell automaticamente con i permessi giusti)
#
# In alternativa (se preferisci PowerShell manuale):
# - Tasto destro in uno spazio vuoto della cartella → "Apri finestra PowerShell qui"
# - Digita:   .\watch-balance-log.ps1
#
# Se ti dice che non può eseguire script, esegui prima:
#   Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
#
# Per fermare: premi Ctrl+C dentro la finestra PowerShell
#              (oppure chiudi semplicemente la finestra)
#
# Il log in tempo reale sarà visibile in:
#   logs\FabioBalanceZone.log   (dentro questa cartella)
#
# Puoi tenerlo aperto con Notepad++ / VS Code mentre fai trading.
# ============================================

$sourceDir  = "$env:APPDATA\ATAS\Logs"
$sourceFile = Join-Path $sourceDir "FabioBalanceZone.log"

$projectRoot = $PSScriptRoot
$logsDir     = Join-Path $projectRoot "logs"
$localFile   = Join-Path $logsDir "FabioBalanceZone.log"

# Ensure local logs directory exists
if (!(Test-Path $logsDir)) {
    New-Item -ItemType Directory -Path $logsDir -Force | Out-Null
    Write-Host "Created logs directory: $logsDir"
}

# Initial copy if source exists
if (Test-Path $sourceFile) {
    Copy-Item -Path $sourceFile -Destination $localFile -Force
    Write-Host "Initial copy done -> $localFile"
} else {
    Write-Host "Source log not found yet: $sourceFile"
    Write-Host "It will be created when the indicator first writes a log line."
}

# Create the watcher
$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $sourceDir
$watcher.Filter = "FabioBalanceZone.log"
$watcher.NotifyFilter = [System.IO.NotifyFilters]::LastWrite -bor [System.IO.NotifyFilters]::FileName
$watcher.IncludeSubdirectories = $false
$watcher.EnableRaisingEvents = $true

$action = {
    $path = $Event.SourceEventArgs.FullPath
    $changeType = $Event.SourceEventArgs.ChangeType
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')] Log changed ($changeType) -> copying..."
    try {
        Copy-Item -Path $path -Destination $localFile -Force
        Write-Host "  Copied to $localFile"
    } catch {
        Write-Host "  Copy error: $_" -ForegroundColor Red
    }
}

# Register events
Register-ObjectEvent $watcher "Changed" -Action $action | Out-Null
Register-ObjectEvent $watcher "Created" -Action $action | Out-Null
Register-ObjectEvent $watcher "Renamed" -Action $action | Out-Null

Write-Host ""
Write-Host "Watching $sourceFile"
Write-Host "Local mirror: $localFile"
Write-Host "Press Ctrl+C to stop."
Write-Host ""

# Keep the script alive
try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
} finally {
    # Cleanup
    $watcher.EnableRaisingEvents = $false
    $watcher.Dispose()
    Get-EventSubscriber | Unregister-Event
    Write-Host "Watcher stopped."
}