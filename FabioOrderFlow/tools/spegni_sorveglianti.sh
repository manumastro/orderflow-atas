#!/usr/bin/env bash
# Spegne ogni sorvegliante del tape rimasto vivo su questa macchina, e lo dimostra.
#
# I sorveglianti si accendono come `Monitor`, e `TaskStop` li chiude: questo script NON li
# sostituisce, e' la rete sotto. Un monitor puo' morire lasciando vivo il processo python che
# alimentava — succede quando il pipe a valle si chiude per primo — e un `sveglia_tape.py`
# orfano continua a interrogare il bridge ogni tre secondi e a far suonare notifiche di sistema
# senza che nessuno le legga. Peggio: al riarmo successivo ce ne sono due, e gli avvisi arrivano
# doppi.
#
# Per questo lo script stampa cosa ha trovato PRIMA di ucciderlo e ricontrolla DOPO: "spento"
# senza la verifica e' un'affermazione, non un fatto. Se dopo il controllo resta qualcosa, esce
# con 1 e lo dice, invece di dichiarare il successo.
#
#     ./spegni_sorveglianti.sh          # spegne e verifica
#     ./spegni_sorveglianti.sh --lista  # dice solo cosa sta girando, non tocca niente
set -uo pipefail

SORVEGLIANTI='sveglia_tape\.py|sveglia_movimento\.py|scenari\.py|permesso_di_fatto\.py'

vivi() {
    # -e perche' possono essere partiti da una shell diversa.
    #
    # Il filtro `python` NON e' cosmetico, e la prima versione di questo script non ce l'aveva:
    # la riga di comando della shell che lancia lo script contiene il nome dello strumento, quindi
    # il pattern trovava **la shell chiamante** e la uccideva. Al primo test `/spegni` si e'
    # suicidato, exit 144. Si uccide solo un processo che sta davvero ESEGUENDO python, e si
    # escludono per pid la shell corrente e quella che l'ha invocata.
    ps -eo pid,command \
        | grep -Ei 'python' \
        | grep -E "$SORVEGLIANTI" \
        | grep -v grep \
        | grep -v spegni_sorveglianti \
        | awk -v me="$$" -v pa="$PPID" '$1 != me && $1 != pa'
}

trovati=$(vivi)

if [ -z "$trovati" ]; then
    echo "nessun sorvegliante in esecuzione"
    exit 0
fi

echo "trovati:"
echo "$trovati" | sed 's/^/  /'

if [ "${1:-}" = "--lista" ]; then
    exit 0
fi

echo "$trovati" | awk '{print $1}' | while read -r pid; do
    kill "$pid" 2>/dev/null
done

sleep 1

# Chi non se n'e' andato col TERM se ne va col KILL: un sorvegliante bloccato su una richiesta
# al bridge puo' ignorare il primo segnale finche' il timeout non scade.
residui=$(vivi)
if [ -n "$residui" ]; then
    echo "$residui" | awk '{print $1}' | while read -r pid; do
        kill -9 "$pid" 2>/dev/null
    done
    sleep 1
fi

residui=$(vivi)
if [ -n "$residui" ]; then
    echo "ANCORA VIVI dopo kill -9:"
    echo "$residui" | sed 's/^/  /'
    exit 1
fi

echo "spenti tutti, verificato"
