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

# Gli avvisi dei monitor, letti dal log invece che dalla notifica.
#
# La sveglia e gli scenari girano in background e ogni evento passa da avviso.py, che lo
# scrive in ~/.fabio-avvisi.log. La notifica all'agente pero' arriva quando l'harness
# decide, e il 16 settembre non e' arrivata affatto mentre l'utente le vedeva a schermo.
# Leggendo il log qui, gli eventi entrano comunque nel contesto a ogni messaggio.
AVVISI="$HOME/.fabio-avvisi.log"
if [ -f "$AVVISI" ]; then
  RECENTI=$(tail -12 "$AVVISI")
  if [ -n "$RECENTI" ]; then
    echo
    echo "──────────────────────────────────────────────────────────────────────────────"
    echo "9. GLI AVVISI DEI MONITOR (ultimi 12, da ~/.fabio-avvisi.log)"
    echo "──────────────────────────────────────────────────────────────────────────────"
    echo "$RECENTI" | sed 's/^/  /'
  fi
fi

echo "=== fine giro d'orizzonte ==============================================="
exit 0
