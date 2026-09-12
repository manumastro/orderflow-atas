#!/usr/bin/env python3
"""Misura se qualcosa, all'istante di chiusura di una barra, predice il movimento successivo.

Perche' separato dal replay: nel replay ogni misura passa attraverso l'ingresso al bordo della
value area, lo stop largo una value area e l'obiettivo 1:1. Sono tre scelte che hanno un costo
proprio, e quel costo puo' seppellire un segnale che c'e' o fabbricarne uno che non c'e'. Qui si
toglie tutto: dal prezzo di chiusura della barra si cammina sul tape e si guarda quale barriera
simmetrica viene toccata prima, +X punti o -X punti. Nessun limite, nessuno spread, nessuna
asimmetria. Se nessuna misura separa questo, nessuna regola di esecuzione la salvera'.

    ./probe_predictability.py CARTELLA --barrier 20 --window 13:30-15:30
"""

from __future__ import annotations

import argparse
import bisect
import glob
import json
import math
import os
from datetime import datetime, timedelta


def parse(raw: str) -> datetime:
    return datetime.fromisoformat(raw.replace("Z", "+00:00"))


def value_area(volume_by_price: dict[float, int], percent: float = 70.0):
    if not volume_by_price:
        return None
    prices = sorted(volume_by_price)
    total = sum(volume_by_price.values())
    poc = max(prices, key=lambda p: volume_by_price[p])
    lo = hi = prices.index(poc)
    taken = volume_by_price[poc]
    target = total * percent / 100
    while taken < target and (lo > 0 or hi < len(prices) - 1):
        up = volume_by_price[prices[hi + 1]] if hi < len(prices) - 1 else -1
        down = volume_by_price[prices[lo - 1]] if lo > 0 else -1
        if up >= down:
            hi += 1
            taken += up
        else:
            lo -= 1
            taken += down
    return poc, prices[lo], prices[hi]


def session_levels(path: str):
    volume: dict[float, int] = {}
    for bar in json.load(open(path))["candles"]:
        for level in bar.get("levels", []):
            volume[level["price"]] = volume.get(level["price"], 0) + level["volume"]
    return value_area(volume)


def barrier(tape, times, start, price, points):
    """Quale barriera simmetrica viene toccata prima. +1 sopra, -1 sotto, 0 se nessuna."""
    k = bisect.bisect_right(times, start)
    up, down = price + points, price - points
    for trade in tape[k:]:
        if trade[4] >= up:
            return 1
        if trade[4] <= down:
            return -1
    return 0


def observe(candles_path, tape_path, previous_path, window, points, big_size):
    bars = json.load(open(candles_path))["candles"]
    raw = json.load(open(tape_path))
    tape = [(parse(t[0]), t[1], t[2], t[3], t[4]) for t in raw["trades"]]
    times = [t[0] for t in tape]
    if not tape:
        return []

    previous = session_levels(previous_path) if previous_path else None

    # Linea di base della sessione per la velocita', e mediana del volume di barra.
    rates = {1: [], -1: []}
    for bar in bars:
        begin, end = parse(bar["time"]), parse(bar["lastTime"])
        seconds = max((end - begin).total_seconds(), 0.001)
        lo, hi = bisect.bisect_left(times, begin), bisect.bisect_right(times, end)
        buy = sum(t[2] for t in tape[lo:hi] if t[1] > 0)
        sell = sum(t[2] for t in tape[lo:hi] if t[1] < 0)
        rates[1].append(buy / seconds)
        rates[-1].append(sell / seconds)
    base = {k: (sorted(v)[len(v) // 2] or 1.0) for k, v in rates.items()}
    volumes = sorted(b["volume"] for b in bars)
    median_volume = volumes[len(volumes) // 2] or 1

    out = []
    developing: dict[float, int] = {}
    for i, bar in enumerate(bars):
        for level in bar.get("levels", []):
            developing[level["price"]] = developing.get(level["price"], 0) + level["volume"]
        if i == 0:
            continue
        if window and not (window[0] <= bar["lastTime"][11:16] <= window[1]):
            continue
        end = parse(bar["lastTime"])
        if end < times[0] or end > times[-1] - timedelta(seconds=1):
            continue

        begin = parse(bar["time"])
        lo, hi = bisect.bisect_left(times, begin), bisect.bisect_right(times, end)
        chunk = tape[lo:hi]
        seconds = max((end - begin).total_seconds(), 0.001)
        buy = sum(t[2] for t in chunk if t[1] > 0)
        sell = sum(t[2] for t in chunk if t[1] < 0)
        big_buy = sum(1 for t in chunk if t[1] > 0 and t[2] >= big_size)
        big_sell = sum(1 for t in chunk if t[1] < 0 and t[2] >= big_size)

        # Il profilo in corso si calcola sulle barre gia' chiuse, questa inclusa: causale.
        current = value_area(developing)
        va = vas = None
        if current:
            va = current
            vas = (bar["close"] - va[1]) / (va[2] - va[1]) if va[2] > va[1] else 0.5

        row = {
            "time": bar["lastTime"],
            "delta": bar["delta"],
            "deltaPrev": bars[i - 1]["delta"],
            "flip": 1 if bar["delta"] > 0 > bars[i - 1]["delta"] else (-1 if bar["delta"] < 0 < bars[i - 1]["delta"] else 0),
            "body": 1 if bar["close"] > bar["open"] else (-1 if bar["close"] < bar["open"] else 0),
            "volumeRatio": round(bar["volume"] / median_volume, 2),
            "bigBuy": big_buy, "bigSell": big_sell,
            "speedBuy": round((buy / seconds) / base[1], 2),
            "speedSell": round((sell / seconds) / base[-1], 2),
            "posInValue": round(vas, 2) if vas is not None else None,
            "minutes": (end - times[0]).total_seconds() / 60,
            "outcome": barrier(tape, times, end, bar["close"], points),
        }
        if previous:
            poc, val, vah = previous
            row["dPoc"] = round(bar["close"] - poc, 2)
            row["dVal"] = round(bar["close"] - val, 2)
            row["dVah"] = round(bar["close"] - vah, 2)
        out.append(row)
    return out


def report(rows, name, keyfn, buckets, base_rate):
    print(f"  {name}")
    for label, test in buckets:
        sub = [r for r in rows if r["outcome"] != 0 and test(keyfn(r))]
        if len(sub) < 20:
            print(f"    {label:>16}  {len(sub):>4} oss.   campione troppo piccolo")
            continue
        up = sum(1 for r in sub if r["outcome"] > 0)
        p = up / len(sub)
        z = (p - base_rate) / math.sqrt(0.25 / len(sub))
        print(f"    {label:>16}  {len(sub):>4} oss.  sale {100 * p:>4.1f}%   z={z:+.2f}")
    print()


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("directory", help="cartella con cAA-GG.json e tAA-GG.json")
    parser.add_argument("--barrier", type=float, default=20.0, help="ampiezza della barriera in punti")
    parser.add_argument("--window", metavar="HH:MM-HH:MM")
    parser.add_argument("--big-trade", type=int, default=20)
    parser.add_argument("--out", help="salva le osservazioni in JSON Lines")
    args = parser.parse_args()
    window = tuple(args.window.split("-")) if args.window else None

    days = []
    for candles in sorted(glob.glob(os.path.join(args.directory, "c*.json"))):
        tape = os.path.join(os.path.dirname(candles), "t" + os.path.basename(candles)[1:])
        if os.path.exists(tape) and os.path.getsize(candles) > 10_000:
            days.append((candles, tape))

    rows = []
    for k, (candles, tape) in enumerate(days):
        rows += observe(candles, tape, days[k - 1][0] if k else None, window, args.barrier, args.big_trade)

    if args.out:
        with open(args.out, "w") as fh:
            for row in rows:
                fh.write(json.dumps(row) + "\n")

    resolved = [r for r in rows if r["outcome"] != 0]
    up = sum(1 for r in resolved if r["outcome"] > 0)
    base_rate = up / len(resolved)
    print(f"{len(days)} sessioni, barriera +/-{args.barrier:g} punti")
    print(f"{len(rows)} osservazioni, {len(resolved)} risolte, sale per prima nel {100 * base_rate:.1f}%\n")

    report(resolved, "delta della barra", lambda r: r["delta"],
           [("<= -300", lambda v: v <= -300), ("-300..-100", lambda v: -300 < v <= -100),
            ("-100..+100", lambda v: -100 < v < 100), ("+100..+300", lambda v: 100 <= v < 300),
            (">= +300", lambda v: v >= 300)], base_rate)

    report(resolved, "flip di delta (condizione 01)", lambda r: r["flip"],
           [("verso l'alto", lambda v: v > 0), ("nessuno", lambda v: v == 0),
            ("verso il basso", lambda v: v < 0)], base_rate)

    report(resolved, "big trades, compratori meno venditori", lambda r: r["bigBuy"] - r["bigSell"],
           [("<= -2", lambda v: v <= -2), ("-1", lambda v: v == -1), ("0", lambda v: v == 0),
            ("+1", lambda v: v == 1), (">= +2", lambda v: v >= 2)], base_rate)

    report(resolved, "speed of tape, rapporto buy/sell", lambda r: r["speedBuy"] / (r["speedSell"] or 1e-9),
           [("< 0.7", lambda v: v < 0.7), ("0.7-1.0", lambda v: 0.7 <= v < 1.0),
            ("1.0-1.4", lambda v: 1.0 <= v < 1.4), (">= 1.4", lambda v: v >= 1.4)], base_rate)

    report(resolved, "posizione nel valore in corso", lambda r: r["posInValue"],
           [("sotto il VAL", lambda v: v is not None and v < 0),
            ("meta' bassa", lambda v: v is not None and 0 <= v < 0.5),
            ("meta' alta", lambda v: v is not None and 0.5 <= v <= 1),
            ("sopra il VAH", lambda v: v is not None and v > 1)], base_rate)

    if any("dPoc" in r for r in resolved):
        report(resolved, "distanza dal POC del giorno prima", lambda r: r.get("dPoc"),
               [("< -50", lambda v: v is not None and v < -50), ("-50..-10", lambda v: v is not None and -50 <= v < -10),
                ("-10..+10", lambda v: v is not None and -10 <= v <= 10), ("+10..+50", lambda v: v is not None and 10 < v <= 50),
                ("> +50", lambda v: v is not None and v > 50)], base_rate)

    report(resolved, "volume rispetto alla mediana", lambda r: r["volumeRatio"],
           [("< 0.6", lambda v: v < 0.6), ("0.6-1.4", lambda v: 0.6 <= v < 1.4),
            ("1.4-2.5", lambda v: 1.4 <= v < 2.5), (">= 2.5", lambda v: v >= 2.5)], base_rate)

    report(resolved, "minuti dall'apertura", lambda r: r["minutes"],
           [("0-15", lambda v: v < 15), ("15-45", lambda v: 15 <= v < 45),
            ("45-90", lambda v: 45 <= v < 90), ("> 90", lambda v: v >= 90)], base_rate)


if __name__ == "__main__":
    main()
