#!/usr/bin/env python3
"""Costruisce barre di range o di tempo dal tape, nello stesso formato di /candles.

Serve a variare la scala della barra senza ricaricare un chart in ATAS: il tape e' completo
(millisecondi, direzione classificata da ATAS) e contiene tutto cio' che serve a ricomporre
footprint, delta e value area.

Approssimazione dichiarata: un cumulative trade attraversa piu' prezzi, e qui il suo volume
viene assegnato per intero a `lastPrice`, il prezzo a cui l'ordine ha finito di eseguire. Sulle
barre reali a 40R questo riproduce delta e volume esatti e sposta la value area di un tick su
qualche barra. L'errore e' lo stesso a ogni scala, quindi non distorce il confronto fra scale.

    ./build_bars.py tape.json --range 40 --out c.json
    ./build_bars.py tape.json --minutes 5  --out c.json
"""

from __future__ import annotations

import argparse
import json
from datetime import datetime, timedelta

TICK = 0.25


def parse(raw: str) -> datetime:
    return datetime.fromisoformat(raw.replace("Z", "+00:00"))


def finish(bar: dict) -> dict:
    """Chiude la barra: value area e POC dal footprint accumulato."""
    levels = sorted(bar["_levels"].items())
    total = sum(v["volume"] for _, v in levels)
    poc_price, poc = max(levels, key=lambda kv: kv[1]["volume"])
    target = total * 0.70
    lo = hi = [p for p, _ in levels].index(poc_price)
    taken = poc["volume"]
    prices = [p for p, _ in levels]
    while taken < target and (lo > 0 or hi < len(levels) - 1):
        up = bar["_levels"][prices[hi + 1]]["volume"] if hi < len(levels) - 1 else -1
        down = bar["_levels"][prices[lo - 1]]["volume"] if lo > 0 else -1
        if up >= down:
            hi += 1
            taken += up
        else:
            lo -= 1
            taken += down
    bar["valueAreaLow"], bar["valueAreaHigh"] = prices[lo], prices[hi]
    bar["poc"] = {"price": poc_price, **poc}
    bar["levels"] = [{"price": p, **v} for p, v in levels]
    del bar["_levels"]
    bar.pop("_until", None)   # le barre di tempo portano il confine, che non va serializzato
    return bar


def build(trades, size_ticks: int | None, minutes: float | None):
    bars: list[dict] = []
    bar = None
    for time, side, volume, first, last in trades:
        price = round(last / TICK) * TICK
        if bar is not None:
            if size_ticks is not None:
                span = max(bar["high"], price) - min(bar["low"], price)
                closed = span > size_ticks * TICK
            else:
                closed = time >= bar["_until"]
            if closed:
                bars.append(finish(bar))
                bar = None
        if bar is None:
            bar = {"bar": len(bars), "time": time.isoformat().replace("+00:00", "Z"),
                   "open": price, "high": price, "low": price, "volume": 0,
                   "bid": 0, "ask": 0, "delta": 0, "_levels": {}}
            if minutes is not None:
                start = time.replace(second=0, microsecond=0)
                bar["_until"] = start + timedelta(minutes=minutes)
        bar["high"] = max(bar["high"], price)
        bar["low"] = min(bar["low"], price)
        bar["close"] = price
        bar["lastTime"] = time.isoformat().replace("+00:00", "Z")
        bar["volume"] += volume
        level = bar["_levels"].setdefault(price, {"volume": 0, "bid": 0, "ask": 0, "ticks": 0})
        level["volume"] += volume
        level["ticks"] += 1
        if side > 0:
            bar["ask"] += volume
            level["ask"] += volume
            bar["delta"] += volume
        else:
            bar["bid"] += volume
            level["bid"] += volume
            bar["delta"] -= volume
    if bar is not None and bar["_levels"]:
        bars.append(finish(bar))
    return bars


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("tape", help="JSON di /cumulative --min-volume 0 --compact")
    parser.add_argument("--range", type=int, help="ampiezza della barra in tick")
    parser.add_argument("--minutes", type=float, help="durata della barra in minuti")
    parser.add_argument("--out", required=True)
    args = parser.parse_args()
    if (args.range is None) == (args.minutes is None):
        raise SystemExit("serve --range oppure --minutes, non entrambi")

    raw = json.load(open(args.tape))
    if not raw.get("compact"):
        raise SystemExit("il tape va scaricato con --compact")
    trades = [(parse(t[0]), t[1], t[2], t[3], t[4]) for t in raw["trades"]]
    bars = build(trades, args.range, args.minutes)
    json.dump({"schema": "fof-bars-from-tape-v1", "count": len(bars),
               "source": args.tape, "range": args.range, "minutes": args.minutes,
               "candles": bars}, open(args.out, "w"))
    print(f"{len(bars)} barre -> {args.out}")


if __name__ == "__main__":
    main()
