#!/bin/bash
# UserPromptSubmit hook: mette il contesto completo davanti all'agente a ogni messaggio.
#
# Esiste perche' l'obbligo in CLAUDE.md ("guarda tutto il contesto prima di ogni risposta")
# dipendeva dal fatto che l'agente si ricordasse di lanciare un comando. Cosi' non dipende piu'.
# Lo stdout di questo hook entra nel contesto del prompt corrente.
#
# Non blocca mai: qualunque errore esce 0 con una riga che lo dichiara.

cd /Users/sabrinastizzi/orderflow-atas || exit 0

# Se il bridge non risponde subito, non ha senso aspettare: si dice e si passa oltre.
if ! curl -s -m 2 http://127.0.0.1:8787/health >/dev/null 2>&1; then
  echo "[giro d'orizzonte] bridge non raggiungibile: ogni misura di mercato in questa risposta"
  echo "sarebbe vecchia o assente, e va dichiarato invece di presentarla come una lettura."
  exit 0
fi

OUT=$(python3 FabioOrderFlow/tools/giro_orizzonte.py --barre 8 2>&1)
if [ $? -ne 0 ]; then
  echo "[giro d'orizzonte] fallito:"
  echo "$OUT" | tail -5
  exit 0
fi

echo "=== GIRO D'ORIZZONTE (automatico, CLAUDE.md) ============================="
echo "$OUT"
echo "=== fine giro d'orizzonte ==============================================="
exit 0
