#!/bin/bash
set -e

echo "================================================"
echo " FabioOrderFlow — Deploy to ATAS"
echo " London Mean Reversion live-first"
echo "================================================"
echo ""

echo "[1/2] Building Release..."
dotnet build -c Release

echo ""
echo "[2/2] Copying DLL to ATAS Indicators..."
ATAS_IND="$APPDATA/ATAS/Indicators"
mkdir -p "$ATAS_IND"
cp -v bin/Release/net10.0-windows/FabioOrderFlow.dll "$ATAS_IND/"

echo ""
echo "Done! DLL: $ATAS_IND/FabioOrderFlow.dll"
ls -lh "$ATAS_IND/FabioOrderFlow.dll"
echo ""
echo "IMPORTANT: Remove indicator from chart, restart ATAS, then re-add indicator."
echo ""
read -p "Press Enter to continue..."
