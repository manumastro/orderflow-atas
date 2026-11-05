<#
    Lancia il lettore di X-Ray (e il server della pagina) senza finestra e ne raccoglie il log.
    Lo usa il task all'accesso (xray_live_task.ps1 -Install); a mano serve solo per il log.

      powershell -File quant/strumenti/xray/scripts/xray_live_run.ps1 -OutDir <cartella>
#>
param(
    [string]$OutDir = "$env:USERPROFILE\orderflow-atas\quant\dati\xray",
    [int]$Interval = 15
)

$repo = Split-Path -Parent $PSScriptRoot
$dll = Join-Path $repo 'XrayLive\bin\Release\net10.0\XrayLive.dll'
$log = Join-Path $OutDir 'lettore.log'

if (-not (Test-Path -LiteralPath $dll)) {
    Write-Error "manca $dll - compila prima: dotnet build quant/strumenti/xray/XrayLive -c Release"
    exit 1
}

New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

# un solo lanciatore: il 30 ottobre 2026 ne giravano due (il task e uno avviato a mano), e il
# secondo riprovava ogni minuto contro il mutex del lettore
$created = $false
$launcherLock = New-Object System.Threading.Mutex($true, 'Local\XrayLiveLauncher', [ref]$created)
if (-not $created) {
    Write-Host "un altro lanciatore di XrayLive e' gia' attivo: esco"
    exit 0
}

while ($true) {
    if ((Test-Path -LiteralPath $log) -and (Get-Item -LiteralPath $log).Length -gt 5MB) {
        Move-Item -LiteralPath $log -Destination ($log + '.1') -Force
    }

    # il lettore gira senza finestra; il log in UTF-8 (il `*>>` di PowerShell 5.1 scriverebbe UTF-16,
    # illeggibile agli altri strumenti)
    # --serve: anche il server della pagina (http://127.0.0.1:8790/), dentro lo stesso processo
    & dotnet $dll --watch --serve --interval $Interval --out $OutDir *>&1 |
        ForEach-Object { $_.ToString() } |
        Out-File -LiteralPath $log -Encoding utf8 -Append

    # esce se un'altra istanza tiene il mutex o se si e' fermato: riprova fra un minuto
    Start-Sleep -Seconds 60
}
