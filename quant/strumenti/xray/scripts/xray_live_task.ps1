<#
    Registra (o toglie) l'avvio automatico di XrayLive all'accesso dell'utente.

      powershell -File scripts/xray_live_task.ps1 -Install
      powershell -File scripts/xray_live_task.ps1 -Status
      powershell -File scripts/xray_live_task.ps1 -Remove
      powershell -File scripts/xray_live_task.ps1 -Stop     ferma lettore e lanciatore, il task resta
      powershell -File scripts/xray_live_task.ps1 -Start    riabilita e riavvia il task

    Il lettore gira senza finestra: l'output va in <OutDir>\lettore.log (rotato a 5 MB).
    Un solo task, per l'utente corrente; se il processo muore, il task lo riavvia.
#>
param(
    [switch]$Install,
    [switch]$Remove,
    [switch]$Status,
    [switch]$Start,
    [switch]$Stop,
    [string]$OutDir = "$env:USERPROFILE\orderflow-atas\quant\dati\xray",
    [int]$IntervalSeconds = 15
)

$taskName = 'XrayLive'
$repo = Split-Path -Parent $PSScriptRoot
$dll = Join-Path $repo 'XrayLive\bin\Release\net10.0\XrayLive.dll'
$launcher = Join-Path $PSScriptRoot 'xray_live_run.ps1'
$log = Join-Path $OutDir 'lettore.log'

function Get-ReaderProcess {
    Get-CimInstance Win32_Process -Filter "Name='dotnet.exe'" -ErrorAction SilentlyContinue |
        Where-Object { $_.CommandLine -like '*XrayLive.dll*' }
}

function Show-Status {
    $t = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    if (-not $t) { Write-Host "task     : NON registrato" }
    else {
        $i = Get-ScheduledTaskInfo -TaskName $taskName
        Write-Host "task     : $($t.State)  ultima esecuzione $($i.LastRunTime)  esito $($i.LastTaskResult)"
    }

    $proc = Get-ReaderProcess
    if ($proc) {
        $titles = $proc | ForEach-Object { (Get-Process -Id $_.ProcessId).MainWindowTitle } | Where-Object { $_ }
        $window = if ($titles) { "finestra '$($titles -join ', ')'" } else { 'nessuna finestra' }
        Write-Host "processo : vivo (pid $($proc.ProcessId -join ',')), $window"
    }
    else { Write-Host "processo : non in esecuzione" }

    $state = Join-Path $OutDir 'status.json'
    if (Test-Path -LiteralPath $state) {
        $s = Get-Content -LiteralPath $state -Raw | ConvertFrom-Json
        Write-Host "ultimo giro: $($s.lastSweepUtc)   chunk $($s.chunks)   errori $($s.errors)   RAM $($s.workingSetMB) MB"
        Write-Host "ultimo chunk: $($s.lastChunk)"
    }
    else { Write-Host "stato    : $state assente" }

    if (Test-Path -LiteralPath $log) {
        Write-Host "log      : $log"
        Get-Content -LiteralPath $log -Tail 3 | ForEach-Object { Write-Host "           $_" }
    }
}

function Get-LauncherProcess {
    Get-CimInstance Win32_Process -Filter "Name='powershell.exe'" -ErrorAction SilentlyContinue |
        Where-Object { $_.CommandLine -like '*xray_live_run.ps1*' }
}

if ($Stop) {
    # ferma senza togliere l'avvio all'accesso: il task, il lanciatore (che riavvierebbe il
    # lettore dopo un minuto) e il lettore
    if (Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue) {
        Stop-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    }
    Get-LauncherProcess | ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }
    Get-ReaderProcess | ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }
    Write-Host "fermato."
    Show-Status
    return
}

if ($Start) {
    $t = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    if (-not $t) {
        Write-Error "task non registrato: usa -Install"
        exit 1
    }
    if ($t.State -eq 'Disabled') { Enable-ScheduledTask -TaskName $taskName | Out-Null }
    Start-ScheduledTask -TaskName $taskName
    Start-Sleep -Seconds 5
    Write-Host "avviato."
    Show-Status
    return
}

if ($Remove) {
    Unregister-ScheduledTask -TaskName $taskName -Confirm:$false -ErrorAction SilentlyContinue
    Get-LauncherProcess | ForEach-Object { Stop-Process -Id $_.ProcessId -Force }
    Get-ReaderProcess | ForEach-Object { Stop-Process -Id $_.ProcessId -Force }
    Write-Host "rimosso."
    Show-Status
    return
}

if ($Status -or -not $Install) {
    Show-Status
    return
}

if (-not (Test-Path -LiteralPath $dll)) {
    Write-Error "manca $dll - compila prima: dotnet build quant/strumenti/xray/XrayLive -c Release"
    exit 1
}

# la finestra non serve: powershell nascosto lancia il lettore e ne raccoglie il log.
# Passa da `conhost --headless` (6 novembre 2026): su Windows 11 il terminale predefinito e'
# Windows Terminal, che ignora -WindowStyle Hidden, e all'accesso restava aperta una finestra
# di PowerShell; conhost senza testa da' al lanciatore una console che non si vede mai.
$action = New-ScheduledTaskAction -Execute 'conhost.exe' `
    -Argument "--headless powershell.exe -NoProfile -WindowStyle Hidden -ExecutionPolicy Bypass -File `"$launcher`" -OutDir `"$OutDir`" -Interval $IntervalSeconds"
$trigger = New-ScheduledTaskTrigger -AtLogOn -User $env:USERNAME
$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries `
    -StartWhenAvailable -RestartCount 999 -RestartInterval (New-TimeSpan -Minutes 1) `
    -ExecutionTimeLimit ([TimeSpan]::Zero) -MultipleInstances IgnoreNew
$principal = New-ScheduledTaskPrincipal -UserId "$env:USERDOMAIN\$env:USERNAME" -LogonType Interactive -RunLevel Limited

Register-ScheduledTask -TaskName $taskName -Action $action -Trigger $trigger -Settings $settings `
    -Principal $principal -Description 'X-Ray OptionsDepth: decifra la cache di ATAS X e la tiene leggibile' -Force | Out-Null

Start-ScheduledTask -TaskName $taskName
Start-Sleep -Seconds 8
Write-Host "installato e avviato (senza finestra): $OutDir"
Show-Status
