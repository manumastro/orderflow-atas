#!/usr/bin/env bash
set -euo pipefail

echo "Building four separate FabioOrderFlow research indicators..."
dotnet build FabioOrderFlow.slnx -c Release

atas_ind="${APPDATA}/ATAS/Indicators"
mkdir -p "$atas_ind"
rm -f "$atas_ind/FabioOrderFlow.dll"

copy_indicator() {
    local project="$1"
    local library="$2"
    cp -f "Indicators/${project}/bin/Release/net10.0-windows/${library}.dll" "$atas_ind/${library}.dll"
}

copy_indicator "CumulativeTrade" "FabioCumulativeTradeRecorder"
copy_indicator "SessionLocation" "FabioSessionLocationRecorder"
copy_indicator "HistoricalCumulativeContext" "FabioHistoricalCumulativeContextRecorder"
copy_indicator "PreSessionProfile" "FabioPreSessionProfileRecorder"

echo "Four separate DLLs deployed to $atas_ind."
echo "Restart ATAS or add each DLL through Add custom indicator."
