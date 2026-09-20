#!/usr/bin/env bash
set -euo pipefail

# Build e deploy delle estensioni ATAS. La cartella di destinazione cambia con il
# sistema E con la versione di ATAS, e le due cose si confondono facilmente:
# su Windows ATAS X carica da "%APPDATA%\ATAS X\Indicators", il vecchio ATAS 8 da
# "%APPDATA%\ATAS\Indicators". Copiare nella seconda mentre gira il primo NON da'
# errore: il deploy dice "fatto" e sul chart resta la DLL precedente.

echo "Building the FabioOrderFlow research indicators..."
dotnet build FabioOrderFlow.slnx -c Release

tfm="net10.0"
if [[ "$(uname -s)" == "Darwin" ]]; then
    atas_ind="${HOME}/Library/Application Support/ATAS/Indicators"
elif [[ -d "${APPDATA}/ATAS X" ]]; then
    atas_ind="${APPDATA}/ATAS X/Indicators"
else
    atas_ind="${APPDATA}/ATAS/Indicators"
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
# ATAS X ricarica l'indicatore da solo dopo la copia, e in replay non perde la posizione:
# verificato il 20 settembre 2026 ricaricando il motore dei livelli col replay fermo. Il vecchio
# ATAS 8 invece richiedeva il riavvio, ed e' da li' che veniva l'istruzione precedente.
echo "ATAS X reloads them on its own. On older ATAS, restart or re-add each DLL."
