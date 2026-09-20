#!/bin/bash
# UserPromptSubmit hook: mette il contesto completo davanti all'agente a ogni messaggio.
#
# Esiste perche' l'obbligo in CLAUDE.md ("guarda tutto il contesto prima di ogni risposta")
# dipendeva dal fatto che l'agente si ricordasse di lanciare un comando. Cosi' non dipende piu'.
# Lo stdout di questo hook entra nel contesto del prompt corrente.
#
# Non blocca mai: qualunque errore esce 0 con una riga che lo dichiara.

# Non un percorso cablato: questo repo cambia macchina (Mac -> PC, settembre 2026), e un
# percorso assoluto sopravvive a un utente ma non a un sistema operativo diverso. La variabile
# la mette l'harness prima di lanciare l'hook - vedi la nota sul percorso in settings.json.
cd "$CLAUDE_PROJECT_DIR" || exit 0

# NIENTE STRUMENTO E NIENTE DATA CABLATI QUI.
#
# Fino al 19 settembre questo file portava CHART="GCZ6" e GIORNO="GCZ6-$(date ...)". Quando ATAS
# e' passato a NQZ6 il curl di controllo ha cominciato a interrogare un chart inesistente, il
# giro ha dichiarato il bridge irraggiungibile pur essendo acceso, e ha stampato il quadro
# dell'oro del giorno prima come se fosse quello di oggi. Un contesto sbagliato si legge come se
# fosse giusto: e' peggio di nessun contesto.
#
# Ora lo strumento lo dice il bridge (/charts) e la data la dice l'ora di mercato (/health),
# non l'orologio di sistema — cosi' funziona anche in replay. Se ATAS ha piu' di un chart
# registrato, giro_orizzonte.py lo dichiara e chiede quale.

# La porta la annuncia il bridge nel file di discovery; 8787 e' solo il ripiego.
# Su Windows la console e' cp1252 e il primo carattere di cornice fa morire Python con
# UnicodeEncodeError: all'agente arriva un traceback invece del contesto, e la risposta esce lo
# stesso costruita a memoria. giro_orizzonte.py si difende anche da solo, ma questo copre ogni
# altro script python lanciato da qui.
export PYTHONUTF8=1
export PYTHONIOENCODING=utf-8

PORTA=$(python3 -c "import json,os;print(json.load(open(os.path.expanduser('~/.fabio-data-bridge.json')))['port'])" 2>/dev/null || echo 8787)

# Se il bridge non risponde subito, non ha senso aspettare: si dice e si passa oltre.
# Si interroga /charts, che e' servito dall'hub e resta valido con qualunque numero di chart:
# /health con piu' chart registrati risponde 400, e verrebbe scambiato per un bridge spento.
if ! curl -s -m 2 "http://127.0.0.1:$PORTA/charts" >/dev/null 2>&1; then
  echo "[giro d'orizzonte] bridge non raggiungibile sulla porta $PORTA: ogni misura di mercato"
  echo "in questa risposta sarebbe vecchia o assente, e va dichiarato invece di presentarla"
  echo "come una lettura."
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
    echo "9. GLI AVVISI DEI MONITOR — $NUOVE nuovi (~/.fabio-avvisi.log; ORARI LOCALI, non UTC)"
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
