#!/bin/bash
# UserPromptSubmit hook: mette il contesto completo davanti all'agente a ogni messaggio.
#
# Esiste perche' l'obbligo in CLAUDE.md ("guarda tutto il contesto prima di ogni risposta")
# dipendeva dal fatto che l'agente si ricordasse di lanciare un comando. Cosi' non dipende piu'.
# Lo stdout di questo hook entra nel contesto del prompt corrente.
#
# Non blocca mai: qualunque errore esce 0 con una riga che lo dichiara.

cd /Users/sabrinastizzi/orderflow-atas || exit 0

# Il chart da interrogare. ATAS puo' averne registrati piu' di uno (il 16 settembre si e'
# aggiunto MCLV6) e in quel caso il bridge rifiuta ogni richiesta che non dica quale.
# Al rollover del contratto si cambia qui.
CHART="GCZ6"

# La chiave dei file della giornata. Non e' una data: e' il prefisso dello strumento piu' la
# data, come vuole il passo 8 di come-si-apre-un-asset.md. Su NQZ6 e' senza prefisso, per ragioni
# storiche - debito aperto. Cambiando strumento si cambiano ENTRAMBE le righe.
GIORNO="GCZ6-$(date +%Y-%m-%d)"

# Se il bridge non risponde subito, non ha senso aspettare: si dice e si passa oltre.
if ! curl -s -m 2 "http://127.0.0.1:8787/health?chart=$CHART" >/dev/null 2>&1; then
  echo "[giro d'orizzonte] bridge non raggiungibile: ogni misura di mercato in questa risposta"
  echo "sarebbe vecchia o assente, e va dichiarato invece di presentarla come una lettura."
  exit 0
fi

OUT=$(python3 FabioOrderFlow/tools/giro_orizzonte.py --barre 8 --chart "$CHART" --giorno "$GIORNO" 2>&1)
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
# Un `tail -12` fisso non basta: durante una fase concitata dodici righe sono otto minuti, e
# tutto quello che e' successo prima dell'ultimo messaggio sparisce senza che nessuno se ne
# accorga. Il 16 settembre l'utente ha fatto notare che non tutte le notifiche svegliano
# l'agente — e' vero, l'harness le consegna quando decide e alcune arrivano troncate — quindi
# questa sezione deve coprire il buco per intero, non fare un campione.
#
# Si tiene un segnalibro con il numero di righe gia' mostrate: a ogni messaggio si stampa
# TUTTO quello che e' arrivato da allora. Cosi' il log, non la notifica, e' la fonte di verita'.
AVVISI="$HOME/.fabio-avvisi.log"
SEGNALIBRO="$HOME/.fabio-avvisi.letto"
if [ -f "$AVVISI" ]; then
  TOTALE=$(wc -l < "$AVVISI" | tr -d ' ')
  LETTE=0
  [ -f "$SEGNALIBRO" ] && LETTE=$(cat "$SEGNALIBRO" 2>/dev/null | tr -d ' ')
  case "$LETTE" in (''|*[!0-9]*) LETTE=0 ;; esac
  # Se il log e' stato ruotato o svuotato il segnalibro e' piu' avanti della fine: si riparte.
  [ "$LETTE" -gt "$TOTALE" ] && LETTE=0

  NUOVE=$((TOTALE - LETTE))
  if [ "$NUOVE" -gt 0 ]; then
    # Alla prima esecuzione della sessione non si riversano ore di log: si mostra la coda.
    [ "$LETTE" -eq 0 ] && [ "$NUOVE" -gt 15 ] && NUOVE=15
    echo
    echo "──────────────────────────────────────────────────────────────────────────────"
    echo "9. GLI AVVISI DEI MONITOR — $NUOVE nuovi dall'ultimo messaggio (~/.fabio-avvisi.log)"
    echo "──────────────────────────────────────────────────────────────────────────────"
    tail -n "$NUOVE" "$AVVISI" | sed 's/^/  /'
  else
    echo
    echo "──────────────────────────────────────────────────────────────────────────────"
    echo "9. GLI AVVISI DEI MONITOR — nessun avviso nuovo dall'ultimo messaggio"
    echo "──────────────────────────────────────────────────────────────────────────────"
    echo "  (silenzio vero: i sorveglianti hanno scritto zero righe. Se invece sono spenti,"
    echo "   lo dice la sezione 1 insieme allo stato del bridge.)"
  fi
  echo "$TOTALE" > "$SEGNALIBRO"
fi

echo "=== fine giro d'orizzonte ==============================================="
exit 0
