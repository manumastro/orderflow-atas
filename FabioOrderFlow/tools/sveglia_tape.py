#!/usr/bin/env python3
"""Sveglia chi analizza quando sul tape succede qualcosa che merita uno sguardo.

**Non giudica.** Non dice se e' assorbimento, se la rottura e' vera, se il livello ha tenuto: quelle
sono letture, e una lettura ha bisogno di contesto che una barra singola non contiene — il delta
cumulato della finestra, quanto volume aveva il test precedente dello stesso livello, se il valore
sta accettando da una parte, l'orologio della seduta. Tutto questo lo fa l'analisi.

Questo programma fa una cosa sola: **dire che e' il momento di guardare**, e mostrare i numeri
grezzi di cio' che l'ha fatto scattare. La conclusione si scrive poi con `annota.py`, che la mette
sul chart e nel diario della giornata.

E' voluto che le condizioni siano grossolane. Un filtro fine sembra intelligente e non lo e': la
versione precedente di questo strumento classificava i segnali da sola, e finiva per etichettare
come assorbimento su un supporto quello che era un rifiuto su una resistenza, perche' i ruoli dei
livelli cambiano durante la seduta e una barra non basta a saperlo.

    ./sveglia_tape.py --livelli docs/research/giornate/livelli-2026-09-15.json \\
        --from 2026-09-15T07:30
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

VICINO = 3.0        # quanto vicino al livello per considerarlo toccato


def minuti(b: dict) -> int:
    """Minuti assoluti dal timestamp.

    `bar` **non e' un identificatore**: e' un indice di posizione e riparte quando ATAS ricarica
    l'indicatore. Usarlo come chiave fa rivalutare tutta la storia dopo un reload.
    """
    t = b["time"]
    return int(t[8:10]) * 1440 + int(t[11:13]) * 60 + int(t[14:16])


def ora(b: dict, fuso: int) -> str:
    return f"{(int(b['time'][11:13]) + fuso) % 24:02d}:{b['time'][14:16]}"


def candele(args) -> list[dict]:
    cmd = [sys.executable, str(BRIDGE), "candles",
           "--from", args.begin, "--to", args.end, "--out", args.cache]
    if args.chart:
        cmd += ["--chart", args.chart]
    subprocess.run(cmd, check=True, timeout=90,
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    return json.load(open(args.cache))["candles"]


def percentili(barre):
    v = sorted(b["volume"] for b in barre)
    d = sorted(abs(b["delta"]) for b in barre)
    q = lambda a, p: a[min(int(len(a) * p), len(a) - 1)]
    return q(v, 0.95), q(d, 0.95)


def motivo(b, prev, livelli, vol95, delta95, muto, fuso):
    """Perche' guardare. Nessun verdetto: solo il fatto e i numeri."""
    cl, vol, d = b["close"], b["volume"], b["delta"]
    pc = prev["close"] if prev else cl
    m = minuti(b)

    for p, nome in livelli:
        if (cl < p <= pc) or (cl > p >= pc):
            if m - muto.get(p, -10 ** 9) >= 10:
                muto[p] = m
                return f"{nome} {p:.0f} attraversato"
        elif abs(b["low"] - p) <= VICINO or abs(b["high"] - p) <= VICINO:
            if vol >= vol95 and m - muto.get(p, -10 ** 9) >= 10:
                muto[p] = m
                return f"{nome} {p:.0f} toccato con volume"

    if vol >= vol95 * 1.5:
        return f"barra pesante ({vol} lotti)"
    if abs(d) >= delta95 * 1.5:
        return f"delta fuori scala ({d:+})"
    return None


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--livelli", required=True, help="JSON dei livelli della giornata")
    ap.add_argument("--from", dest="begin", required=True)
    ap.add_argument("--to", dest="end", default="2100-01-01T00:00")
    ap.add_argument("--chart")
    ap.add_argument("--intervallo", type=int, default=20)
    ap.add_argument("--fuso", type=int, default=2, help="ore da aggiungere all'UTC (default 2)")
    ap.add_argument("--cache", default="/tmp/fof-sveglia.json")
    args = ap.parse_args()

    livelli = sorted(((float(l["price"]), (l.get("label") or "").split(" ·")[0])
                      for l in json.load(open(args.livelli))), key=lambda x: -x[0])

    visti: set[str] = set()
    muto: dict[float, int] = {}
    frontiera = None
    prima = True

    while True:
        try:
            c = candele(args)
        except Exception as e:
            print(f"[bridge non raggiungibile: {type(e).__name__}]", flush=True)
            time.sleep(30)
            continue

        chiuse = c[:-1]                      # l'ultima barra e' ancora in formazione
        if not chiuse:
            time.sleep(args.intervallo)
            continue

        # Le soglie si ricalcolano a ogni giro sulla seduta in corso: non sono una costante
        # di mercato, e una seduta sottile e una densa non hanno le stesse barre grosse.
        vol95, delta95 = percentili(chiuse)

        if prima:
            visti = {b["time"] for b in chiuse}
            frontiera = minuti(chiuse[-1])
            u = chiuse[-1]
            print(f"[sveglia attiva] {len(livelli)} livelli, ultima barra {ora(u, args.fuso)} "
                  f"a {u['close']:.2f} - p95 volume {vol95}, p95 delta {delta95}", flush=True)
            prima = False
        else:
            for i, b in enumerate(chiuse):
                if b["time"] in visti or minuti(b) <= frontiera:
                    continue
                visti.add(b["time"])
                frontiera = minuti(b)
                perche = motivo(b, chiuse[i - 1] if i else None,
                                livelli, vol95, delta95, muto, args.fuso)
                if perche:
                    print(f"GUARDA {ora(b, args.fuso)} {b['close']:.2f} | {perche} | "
                          f"O {b['open']:.2f} H {b['high']:.2f} L {b['low']:.2f} "
                          f"vol {b['volume']} delta {b['delta']:+}", flush=True)
        time.sleep(args.intervallo)


if __name__ == "__main__":
    main()
