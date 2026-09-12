#!/usr/bin/env python3
"""Esegue replay_model.py su piu' sessioni e aggrega, variando un parametro alla volta.

Serve a rendere visibile la sensibilita' di ogni scelta: un risultato che cambia segno fra due
valori adiacenti di una soglia non e' un risultato, e' un artefatto della soglia.

    ./replay_sweep.py /percorso/mese --sweep manage=0,20,40,80 -- --window 13:30-15:30
"""

from __future__ import annotations

import argparse
import glob
import json
import os
import subprocess
import sys

TOOL = os.path.join(os.path.dirname(os.path.abspath(__file__)), "replay_model.py")


def sessions(directory: str):
    """Coppie (candele, tape) dei giorni che hanno entrambi i file, in ordine di data."""
    out = []
    for candles in sorted(glob.glob(os.path.join(directory, "c*.json"))):
        # Solo il nome del file va tradotto: il percorso puo' contenere altre "/c".
        name = os.path.basename(candles)
        tape = os.path.join(os.path.dirname(candles), "t" + name[1:])
        if os.path.exists(tape) and os.path.getsize(candles) > 10_000:
            out.append((os.path.basename(candles)[1:-5], candles, tape))
    return out


def run(days, extra) -> dict:
    total = {"trades": 0, "wins": 0, "r": 0.0, "dollars": 0, "cancelled": 0, "unfilled": 0, "managed": 0}
    per_day = []
    for day, candles, tape in days:
        result = subprocess.run([sys.executable, TOOL, candles, "--tape", tape, "--json"] + extra,
                                capture_output=True, text=True)
        if result.returncode != 0:
            print(result.stderr.strip(), file=sys.stderr)
            continue
        summary = json.loads(result.stdout)
        per_day.append((day, summary))
        for key in total:
            total[key] += summary[key]
    return total, per_day


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("directory", help="cartella con cAA-GG.json e tAA-GG.json")
    parser.add_argument("--sweep", help="nome=valore,valore,... del parametro da variare")
    parser.add_argument("--per-day", action="store_true", help="mostra anche il dettaglio per sessione")
    # Le opzioni non riconosciute vengono passate tali e quali a replay_model.py: REMAINDER non
    # va bene perche' inghiottirebbe anche le opzioni di questo script che seguono la cartella.
    args, passthrough = parser.parse_known_args()

    extra = [a for a in passthrough if a != "--"]
    days = sessions(args.directory)
    if not days:
        raise SystemExit(f"nessuna coppia cAA-GG.json / tAA-GG.json in {args.directory}")
    print(f"{len(days)} sessioni, da {days[0][0]} a {days[-1][0]}\n")

    if args.sweep:
        name, _, raw = args.sweep.partition("=")
        values = raw.split(",")
    else:
        name, values = None, [None]

    print(f"{name or 'base':>14}{'op':>6}{'vinte':>7}{'WR':>6}{'R':>9}{'$':>10}{'cancel':>8}{'no-fill':>9}{'gestite':>9}")
    for value in values:
        options = extra + ([f"--{name}", value] if name else [])
        total, per_day = run(days, options)
        rate = 100 * total["wins"] / total["trades"] if total["trades"] else 0
        print(f"{value if value is not None else '-':>14}{total['trades']:>6}{total['wins']:>7}"
              f"{rate:>5.0f}%{total['r']:>+9.2f}{total['dollars']:>+10,}"
              f"{total['cancelled']:>8}{total['unfilled']:>9}{total['managed']:>9}")
        if args.per_day:
            for day, summary in per_day:
                wr = 100 * summary["wins"] / summary["trades"] if summary["trades"] else 0
                print(f"{'  ' + day:>14}{summary['trades']:>6}{summary['wins']:>7}{wr:>5.0f}%"
                      f"{summary['r']:>+9.2f}{summary['dollars']:>+10,}")


if __name__ == "__main__":
    main()
