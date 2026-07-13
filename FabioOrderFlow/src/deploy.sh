#!/usr/bin/env bash
set -euo pipefail

echo "Building neutral FabioOrderFlow shell..."
dotnet build -c Release

atas_ind="${APPDATA}/ATAS/Indicators"
mkdir -p "$atas_ind"
cp -f bin/Release/net10.0-windows/FabioOrderFlow.dll "$atas_ind/FabioOrderFlow.dll"

echo "DLL deployed to $atas_ind/FabioOrderFlow.dll"
echo "Restart ATAS or remove and re-add the indicator to load it."
