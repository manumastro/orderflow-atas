# Run this script as Administrator (right-click -> Run as administrator)
# It creates a symbolic link inside this folder pointing to the real-time ATAS log.
# After creation you can open "FabioBalanceZone.log" directly from here and it will stay in sync.

$target = "$env:APPDATA\ATAS\Logs\FabioBalanceZone.log"
$link = "$PSScriptRoot\FabioBalanceZone.log"

Write-Host "Target log : $target"
Write-Host "Link       : $link"

if (Test-Path $link) {
    Remove-Item $link -Force
    Write-Host "Removed existing link."
}

try {
    $null = New-Item -ItemType SymbolicLink -Path $link -Target $target -Force
    Write-Host "SUCCESS: Symbolic link created." -ForegroundColor Green
    Write-Host "You can now tail or open 'FabioBalanceZone.log' from this folder for real-time logs."
} catch {
    Write-Host "FAILED to create symlink: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Solutions:" -ForegroundColor Yellow
    Write-Host "1. Run this script as Administrator (recommended for one-time setup)."
    Write-Host "2. Enable Windows Developer Mode (Settings > Update & Security > For developers)."
    Write-Host "3. Then re-run the script without elevation."
}