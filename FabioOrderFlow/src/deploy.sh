#!/usr/bin/env bash
set -euo pipefail

# Build e deploy delle estensioni ATAS. La cartella di destinazione cambia con il
# sistema E con la versione di ATAS, e le due cose si confondono facilmente:
# su Windows ATAS X carica da "%APPDATA%\ATAS X\Indicators", il vecchio ATAS 8 da
# "%APPDATA%\ATAS\Indicators". Copiare nella seconda mentre gira il primo NON da'
# errore: il deploy dice "fatto" e sul chart resta la DLL precedente.

echo "Building the Fabio Data Bridge..."
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
# DLL che non si distribuiscono piu': lo scheletro iniziale e, dal 10 ottobre 2026, i quattro
# recorder della fase di agosto. Se ATAS le tiene aperte non si cancellano: si riprova al deploy dopo.
for vecchia in FabioOrderFlow FabioCumulativeTradeRecorder FabioSessionLocationRecorder FabioHistoricalCumulativeContextRecorder FabioPreSessionProfileRecorder; do
    rm -f "$atas_ind/$vecchia.dll" 2>/dev/null || echo "  $vecchia.dll e' aperta da ATAS: si toglie al prossimo deploy"
done

copy_indicator() {
    local project="$1"
    local library="$2"
    cp -f "Indicators/${project}/bin/Release/${tfm}/${library}.dll" "$atas_ind/${library}.dll"
}

copy_indicator "DataBridge" "FabioDataBridge"

echo "FabioDataBridge.dll deployed to $atas_ind."
# ATAS X ricarica l'indicatore da solo dopo la copia, e in replay non perde la posizione:
# verificato il 20 settembre 2026 ricaricando il motore dei livelli col replay fermo. Il vecchio
# ATAS 8 invece richiedeva il riavvio, ed e' da li' che veniva l'istruzione precedente.
echo "ATAS X reloads it on its own. On older ATAS, restart or re-add the DLL."
