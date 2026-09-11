#!/usr/bin/env bash
set -euo pipefail

# Build e deploy delle estensioni ATAS. Funziona sia su ATAS X (macOS) sia su
# ATAS classico (Windows): la cartella di destinazione cambia con il sistema.

echo "Building the FabioOrderFlow research indicators..."
dotnet build FabioOrderFlow.slnx -c Release

if [[ "$(uname -s)" == "Darwin" ]]; then
    atas_ind="${HOME}/Library/Application Support/ATAS/Indicators"
    tfm="net10.0"
else
    atas_ind="${APPDATA}/ATAS/Indicators"
    tfm="net10.0"
fi

mkdir -p "$atas_ind"
rm -f "$atas_ind/FabioOrderFlow.dll"

copy_indicator() {
    local project="$1"
    local library="$2"
    cp -f "Indicators/${project}/bin/Release/${tfm}/${library}.dll" "$atas_ind/${library}.dll"
}

copy_indicator "CumulativeTrade" "FabioCumulativeTradeRecorder"
copy_indicator "SessionLocation" "FabioSessionLocationRecorder"
copy_indicator "HistoricalCumulativeContext" "FabioHistoricalCumulativeContextRecorder"
copy_indicator "PreSessionProfile" "FabioPreSessionProfileRecorder"
copy_indicator "DataBridge" "FabioDataBridge"

echo "Five separate DLLs deployed to $atas_ind."
echo "Restart ATAS or add each DLL through Add custom indicator."
