# FabioOrderFlow - Trade Report Tool (PowerShell)
# Extracts and formats MR trades from ATAS logs with UUID grouping

param(
    [string]$Date = (Get-Date -Format "yyyy-MM-dd"),
    [string]$LogFile = "$env:APPDATA\ATAS\Logs\FabioOrderFlow.log",
    [switch]$GroupByUuid,
    [switch]$OnlyCompleted
)

Write-Host "===============================================================================" -ForegroundColor Cyan
Write-Host "FabioOrderFlow - Trade Report" -ForegroundColor Cyan
Write-Host "===============================================================================" -ForegroundColor Cyan
Write-Host "Log file: $LogFile"
Write-Host "Date filter: $Date"
Write-Host ""

if (-not (Test-Path $LogFile)) {
    Write-Host "ERROR: Log file not found: $LogFile" -ForegroundColor Red
    exit 1
}

# Parse log line into object
function Parse-LogLine {
    param([string]$Line)
    
    $obj = @{}
    
    # Extract timestamp - try both formats:
    # 1. New format (historical): Italy=2026-06-25 15:30:00 (in body)
    # 2. Old format (live): [Italy=2026-06-25 15:30:00] (prefix)
    if ($Line -match '\[Italy=([^\]]+)\]') {
        $obj.Timestamp = $matches[1]
    }
    elseif ($Line -match 'Italy=([0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}(?:\.[0-9]{3})?)') {
        $obj.Timestamp = $matches[1]
    }
    
    # Extract tag
    if ($Line -match '\[([A-Z_]+)\]') {
        $obj.Tag = $matches[1]
    }
    
    # Extract all key=value pairs
    $pairs = [regex]::Matches($Line, '(\w+)=((?:[^,\s]+|"[^"]*"))')
    foreach ($pair in $pairs) {
        $key = $pair.Groups[1].Value
        $value = $pair.Groups[2].Value.Trim('"')
        $obj[$key] = $value
    }
    
    return $obj
}

# Read and filter log lines
$triggers = @()
$aggressions = @()
$mfeUpdates = @()
$targetHits = @()
$positionClosed = @()

Get-Content $LogFile | Where-Object { $_ -match "Italy=$Date" } | ForEach-Object {
    $line = $_
    
    if ($line -match '\[MR_TRIGGER\]|\[MR_EARLY_TRIGGER\]') {
        $triggers += Parse-LogLine $line
    }
    elseif ($line -match '\[MR_AGGRESSION_CONFIRM\]') {
        $aggressions += Parse-LogLine $line
    }
    elseif ($line -match '\[MR_MFE_UPDATE\]') {
        $mfeUpdates += Parse-LogLine $line
    }
    elseif ($line -match '\[MR_TARGET_HIT\]') {
        $targetHits += Parse-LogLine $line
    }
    elseif ($line -match '\[MR_POSITION_CLOSED\]') {
        $positionClosed += Parse-LogLine $line
    }
}

Write-Host "===============================================================================" -ForegroundColor Yellow
Write-Host "SUMMARY" -ForegroundColor Yellow
Write-Host "===============================================================================" -ForegroundColor Yellow
Write-Host "Triggers: $($triggers.Count)"
Write-Host "Aggression Confirmations: $($aggressions.Count)"
Write-Host "Position Closed: $($positionClosed.Count)"
Write-Host ""

# Group by UUID if requested
if ($GroupByUuid) {
    Write-Host "===============================================================================" -ForegroundColor Green
    Write-Host "TRADES GROUPED BY UUID" -ForegroundColor Green
    Write-Host "===============================================================================" -ForegroundColor Green
    Write-Host ""
    
    # Get all unique UUIDs
    $allUuids = @()
    $allUuids += $triggers | Where-Object { $_.Uuid } | ForEach-Object { $_.Uuid }
    $allUuids += $aggressions | Where-Object { $_.TriggerUuid } | ForEach-Object { $_.TriggerUuid }
    $allUuids = $allUuids | Select-Object -Unique
    
    foreach ($uuid in $allUuids) {
        $trigger = $triggers | Where-Object { $_.Uuid -eq $uuid } | Select-Object -First 1
        $aggression = $aggressions | Where-Object { $_.TriggerUuid -eq $uuid } | Select-Object -First 1
        $mfe = $mfeUpdates | Where-Object { $_.TriggerUuid -eq $uuid } | Select-Object -Last 1
        $targets = $targetHits | Where-Object { $_.TriggerUuid -eq $uuid }
        $closed = $positionClosed | Where-Object { $_.TriggerUuid -eq $uuid } | Select-Object -First 1
        
        if ($OnlyCompleted -and -not $closed) {
            continue
        }
        
        Write-Host "UUID: $uuid" -ForegroundColor Cyan
        Write-Host ("=" * 80)
        
        if ($trigger) {
            Write-Host "  TRIGGER:" -ForegroundColor Yellow
            Write-Host "    Time: $($trigger.Timestamp)"
            Write-Host "    Direction: $($trigger.Direction)"
            Write-Host "    Type: $($trigger.Trigger)"
            Write-Host "    Bar: $($trigger.Bar) (Candidate: $($trigger.CandidateBar))"
            Write-Host ""
        }
        
        if ($aggression) {
            Write-Host "  ENTRY:" -ForegroundColor Green
            Write-Host "    Time: $($aggression.Timestamp)"
            Write-Host "    Price: $($aggression.EntryPrice)"
            Write-Host "    Volume: $($aggression.Volume)"
            Write-Host "    Stop: $($aggression.StopReference)"
            Write-Host "    Target1: $($aggression.Target1POC)"
            Write-Host "    Target2: $($aggression.Target2)"
            Write-Host "    Risk: $($aggression.RiskPoints) pts"
            Write-Host "    Delay: $($aggression.SecondsAfterSweep)s"
            Write-Host ""
        }
        
        if ($targets) {
            Write-Host "  TARGETS:" -ForegroundColor Magenta
            foreach ($target in $targets) {
                Write-Host "    $($target.Target) hit @ $($target.Timestamp) - Price: $($target.TargetPrice) (+$($target.RewardPoints) pts)"
            }
            Write-Host ""
        }
        
        if ($mfe) {
            Write-Host "  PEAK PERFORMANCE:" -ForegroundColor Blue
            Write-Host "    MFE: $($mfe.MFE) pts (Max: $($mfe.MaxFavorablePrice))"
            Write-Host "    MAE: $($mfe.MAE) pts (Max: $($mfe.MaxAdversePrice))"
            Write-Host ""
        }
        
        if ($closed) {
            $color = if ([decimal]$closed.FinalPnL -gt 0) { "Green" } else { "Red" }
            Write-Host "  CLOSED:" -ForegroundColor $color
            Write-Host "    Time: $($closed.Timestamp)"
            Write-Host "    Reason: $($closed.ExitReason)"
            Write-Host "    PnL: $($closed.FinalPnL) pts"
            Write-Host "    Target1: $($closed.Target1Hit)"
            Write-Host "    Target2: $($closed.Target2Hit)"
        }
        else {
            Write-Host "  STATUS: OPEN" -ForegroundColor Yellow
        }
        
        Write-Host ""
    }
}
else {
    # Simple list view
    Write-Host "===============================================================================" -ForegroundColor Green
    Write-Host "TRIGGERS ($($triggers.Count))" -ForegroundColor Green
    Write-Host "===============================================================================" -ForegroundColor Green
    $triggers | ForEach-Object {
        Write-Host "$($_.Timestamp) | UUID: $($_.Uuid) | $($_.Direction) $($_.Trigger) | Bar: $($_.Bar)"
    }
    Write-Host ""
    
    Write-Host "===============================================================================" -ForegroundColor Green
    Write-Host "AGGRESSION CONFIRMATIONS ($($aggressions.Count))" -ForegroundColor Green
    Write-Host "===============================================================================" -ForegroundColor Green
    $aggressions | ForEach-Object {
        Write-Host "$($_.Timestamp) | UUID: $($_.TriggerUuid) | $($_.Direction) @ $($_.EntryPrice) | Vol: $($_.Volume)"
    }
    Write-Host ""
    
    Write-Host "===============================================================================" -ForegroundColor Green
    Write-Host "POSITION CLOSED ($($positionClosed.Count))" -ForegroundColor Green
    Write-Host "===============================================================================" -ForegroundColor Green
    $positionClosed | ForEach-Object {
        $color = if ([decimal]$_.FinalPnL -gt 0) { "Green" } else { "Red" }
        Write-Host "$($_.Timestamp) | UUID: $($_.TriggerUuid) | $($_.Direction) | PnL: $($_.FinalPnL) pts | $($_.ExitReason)" -ForegroundColor $color
    }
}

Write-Host ""
Write-Host "===============================================================================" -ForegroundColor Cyan
Write-Host "End of Report" -ForegroundColor Cyan
Write-Host "===============================================================================" -ForegroundColor Cyan
