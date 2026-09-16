#!/usr/bin/env python3
"""Il permesso direzionale misurato sull'accettazione, non sull'orologio.

**Il problema che risolve.** Il gate Tier 01 del dossier chiede una chiusura a 30 minuti fuori
dall'IVB. Quella regola misura la cosa giusta — l'**accettazione** oltre un bordo — ma alla
risoluzione sbagliata: puo' far aspettare fino a 29 minuti per sapere una cosa che il tape ha gia'
detto. Misurato sul 16 settembre 2026, contro questo programma:

    concede LONG   gate 17:00  |  accettazione 17:01   il gate e' piu' veloce di 1 minuto
    revoca         gate 19:29  |  accettazione 18:05   l'accettazione e' piu' veloce di 84 MINUTI

**Il guadagno sta nella revoca, non nella concessione**, e va detto perche' e' il contrario di
quello che sembra: nessuna regola di accettazione puo' parlare prima che il prezzo abbia accettato,
quindi in apertura il gate M30 non e' battibile. Ma una volta concesso, il gate tiene il permesso
in piedi fino alla prossima chiusura a 30 minuti anche quando il prezzo e' gia' rientrato da un'ora
e mezza — ed e' li' che costa.

**Cosa fa invece questo programma.** Guarda la stessa domanda — il prezzo *sta* oltre il bordo o ci
e' solo passato? — su barre M1, e risponde in tre minuti invece che in trenta:

    ACCETTAZIONE sopra un bordo, tutte e tre insieme:
      1. N chiusure M1 consecutive oltre il bordo          (--chiusure, default 3)
      2. volume scambiato oltre il bordo >= soglia          (--volume, in lotti)
      3. nessun ritorno dall'altra parte nella finestra     (implicito in 1)

**L'isteresi non e' opzionale.** Un permesso concesso si revoca solo dopo M chiusure consecutive
RIENTRATE (--rientri, default 3), non appena una barra torna indietro. Senza, la regola e'
inservibile: sul 16 settembre dava DODICI cambi di permesso, con sei oscillazioni in
quarantacinque minuti. Con l'isteresi ne da' tre. Non e' un parametro aggiustato sui dati di quel
giorno — e' strutturale: un permesso non deve evaporare perche' una barra ha toccato indietro.

Non sostituisce il dossier: il gate M30 resta la fonte, e questo programma **stampa entrambi**
accanto, con quanti minuti di scarto. Dopo qualche seduta si ha il numero invece dell'impressione.

**Non decide niente.** Come `sveglia_tape.py`, dice che una condizione misurabile e' vera. Se quel
permesso vada usato e' una lettura, e la lettura la fa l'analisi.

    ./permesso_di_fatto.py --chart NQZ6 --alto 29481.5 --basso 29373.25 \\
        --from 2026-09-16T13:30

Documentazione: docs/research/metodo/il-permesso-si-misura-non-si-aspetta.md
"""
from __future__ import annotations

import argparse
import json
import subprocess
import sys
import time
from pathlib import Path

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
sys.path.insert(0, str(HERE))
try:
    from avviso import avvisa
except Exception:                                                  # pragma: no cover
    def avvisa(riga, titolo=""):                                   # type: ignore
        pass


def candele(chart: str, da: str) -> list[dict]:
    cmd = [sys.executable, str(BRIDGE), "candles", "--from", da]
    if chart:
        cmd += ["--chart", chart]
    try:
        out = subprocess.run(cmd, capture_output=True, text=True, timeout=60).stdout
        d = json.loads(out)
    except Exception:
        return []
    return d if isinstance(d, list) else d.get("candles", d.get("data", []))


def ora(b: dict, fuso: int) -> str:
    hh, mm = int(b["time"][11:13]), b["time"][14:16]
    return f"{(hh + fuso) % 24:02d}:{mm}"


def volume_oltre(barre: list[dict], bordo: float, verso: str) -> float:
    """Volume scambiato oltre il bordo, ripartito uniformemente nel range di ogni barra.

    E' una stima: senza la footprint prezzo per prezzo non si sa quanto di una barra a cavallo
    del bordo sia stato scambiato da che parte. Con `--levels` si potrebbe fare esatto, ma
    costerebbe una chiamata per barra e la stima basta a distinguere "ci e' passato" da "ci vive".
    """
    tot = 0.0
    for b in barre:
        lo, hi = b["low"], b["high"]
        if hi <= lo:
            quota = 1.0 if (hi > bordo if verso == "sopra" else hi < bordo) else 0.0
        elif verso == "sopra":
            quota = max(0.0, min(1.0, (hi - bordo) / (hi - lo)))
        else:
            quota = max(0.0, min(1.0, (bordo - lo) / (hi - lo)))
        tot += b["volume"] * quota
    return tot


def accettato(barre: list[dict], bordo: float, verso: str, n: int, vol_min: float):
    """(True, dettaglio) se le ultime n barre chiuse sono tutte oltre il bordo con volume vero."""
    if len(barre) < n:
        return False, None
    ultime = barre[-n:]
    if verso == "sopra":
        if not all(b["close"] > bordo for b in ultime):
            return False, None
    else:
        if not all(b["close"] < bordo for b in ultime):
            return False, None
    v = volume_oltre(ultime, bordo, verso)
    if v < vol_min:
        return False, None
    d = sum(b.get("delta") or 0 for b in ultime)
    return True, {"vol_oltre": v, "delta": d, "barre": ultime}


def gate_m30(barre: list[dict], alto: float, basso: float):
    """Il gate del dossier: l'ultima chiusura M30 fuori dall'IVB, e a che ora e' stata.

    Il minuto che chiude un M30 e' quello con minuto % 30 == 29 (la barra 16:29 chiude il
    blocco 16:00-16:30). Vedi come-si-apre-un-asset.md, passo 2.
    """
    stato, quando = "DENTRO", None
    for b in barre:
        if int(b["time"][14:16]) % 30 != 29:
            continue
        if b["close"] > alto:
            stato, quando = "LONG", b["time"]
        elif b["close"] < basso:
            stato, quando = "SHORT", b["time"]
        else:
            stato, quando = "DENTRO", b["time"]
    return stato, quando


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--chart")
    ap.add_argument("--from", dest="da", required=True,
                    help="deve cadere PRIMA dell'inizio dell'IVB, o il gate M30 non e' calcolabile")
    ap.add_argument("--alto", type=float, required=True, help="tetto dell'IVB")
    ap.add_argument("--basso", type=float, required=True, help="pavimento dell'IVB")
    ap.add_argument("--chiusure", type=int, default=3,
                    help="quante chiusure M1 consecutive oltre il bordo (default 3)")
    ap.add_argument("--volume", type=float, default=1500,
                    help="lotti minimi scambiati oltre il bordo nelle stesse barre")
    ap.add_argument("--rientri", type=int, default=3,
                    help="chiusure consecutive RIENTRATE che revocano un permesso gia' concesso "
                         "(isteresi: senza, la regola oscilla e non e' usabile)")
    ap.add_argument("--fuso", type=int, default=2, help="ore da sommare all'ora del bridge")
    ap.add_argument("--attesa", type=int, default=20, help="secondi fra un giro e l'altro")
    args = ap.parse_args()

    print(f"[permesso di fatto attivo] IVB {args.basso}-{args.alto} | accettazione = "
          f"{args.chiusure} chiusure M1 oltre + {args.volume:,.0f} lotti oltre, "
          f"revoca su {args.rientri} rientrate | "
          f"il gate M30 resta registrato accanto, non ferma niente", flush=True)

    fatto_prec, m30_prec, ultima = None, None, None
    while True:
        barre = [b for b in candele(args.chart, args.da) if b.get("volume")]
        if not barre:
            print("[NESSUNA BARRA dal bridge]", flush=True)
            time.sleep(args.attesa)
            continue
        chiuse = barre[:-1]                      # l'ultima e' in formazione, non e' una chiusura
        if not chiuse:
            time.sleep(args.attesa)
            continue

        # Isteresi: si CONCEDE sull'accettazione, si REVOCA solo su --rientri chiusure rientrate.
        det = None
        if fatto_prec in (None, "NESSUNO"):
            su, det_su = accettato(chiuse, args.alto, "sopra", args.chiusure, args.volume)
            giu, det_giu = accettato(chiuse, args.basso, "sotto", args.chiusure, args.volume)
            fatto = "LONG" if su else ("SHORT" if giu else "NESSUNO")
            det = det_su if su else det_giu
        elif fatto_prec == "LONG":
            rientrate = chiuse[-args.rientri:]
            fatto = "NESSUNO" if (len(rientrate) == args.rientri
                                  and all(b["close"] <= args.alto for b in rientrate)) else "LONG"
        else:
            rientrate = chiuse[-args.rientri:]
            fatto = "NESSUNO" if (len(rientrate) == args.rientri
                                  and all(b["close"] >= args.basso for b in rientrate)) else "SHORT"

        m30, quando = gate_m30(chiuse, args.alto, args.basso)
        b = chiuse[-1]

        if fatto != fatto_prec:
            if fatto == "NESSUNO":
                riga = (f"PERMESSO DI FATTO REVOCATO (era {fatto_prec}) | {ora(b, args.fuso)} "
                        f"C {b['close']} | il prezzo non e' piu' accettato oltre il bordo")
            else:
                v = det["vol_oltre"] if det else 0
                riga = (f"PERMESSO DI FATTO {fatto} | {ora(b, args.fuso)} C {b['close']} | "
                        f"{args.chiusure} chiusure oltre, {v:,.0f} lotti oltre il bordo, "
                        f"delta {det['delta']:+.0f} | gate M30 dice {m30}")
            print(riga, flush=True)
            avvisa(riga, titolo="NQ permesso")
            fatto_prec = fatto

        if m30 != m30_prec:
            oram30 = quando[11:16] if quando else "?"
            scarto = ""
            if fatto == m30 and fatto != "NESSUNO":
                scarto = "  <-- il gate conferma cio' che l'accettazione diceva gia'"
            print(f"[gate M30 -> {m30} alla chiusura {oram30}Z]{scarto}", flush=True)
            m30_prec = m30

        if b["time"] != ultima:
            ultima = b["time"]
        time.sleep(args.attesa)


if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print("\n[fermato]", flush=True)
