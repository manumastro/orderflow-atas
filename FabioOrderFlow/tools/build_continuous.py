#!/usr/bin/env python3
"""Costruisce una serie continua **back-adjusted** da piu' contratti, footprint incluso.

Il contratto continuo di ATAS incolla i contratti senza correggere il salto di prezzo: la barra
daily del 2026-09-13 e' gia' NQZ6 mentre quella del 09-10 e' ancora NQU6, con circa 294 punti di
differenza. Su un chart daily usato per il profile framing questo fa vedere uno spostamento di
valore che non e' avvenuto.

Qui il salto si toglie. Lo spread si misura sui minuti in cui **entrambi** i contratti stampano
davvero, quindi non e' una stima su due prezzi di chiusura ma una mediana su migliaia di
osservazioni appaiate. Tutte le barre dei contratti piu' vecchi vengono spostate di quello spread,
cosi' la serie finisce nei prezzi del contratto corrente: i livelli letti sul grafico sono prezzi
operabili oggi, non prezzi storici.

Il footprint viene spostato con le barre, quindi anche POC e value area storici risultano
confrontabili.

    ./build_continuous.py c-NQU6.json c-NQZ6.json --out continuo.json
    ./build_continuous.py c-NQU6.json c-NQZ6.json --cash 13:30-20:00 --out daily.json
"""

from __future__ import annotations

import argparse
import json
import statistics
from collections import defaultdict


def load(path: str):
    data = json.load(open(path))
    return data.get("instrument", path), data["candles"]


def daily_volume(bars):
    out = defaultdict(int)
    for b in bars:
        out[b["time"][:10]] += b["volume"]
    return out


def roll_date(old, new) -> str | None:
    """Prima giornata in cui il contratto nuovo supera il vecchio per volume."""
    vo, vn = daily_volume(old), daily_volume(new)
    for day in sorted(set(vo) | set(vn)):
        o, n = vo.get(day, 0), vn.get(day, 0)
        if o + n > 0 and n > o:
            return day
    return None


def spread(old, new, until: str, min_volume: int = 5) -> float:
    """Mediana di (nuovo - vecchio) sui minuti in cui entrambi hanno scambiato davvero.

    Il filtro sul volume serve a scartare i minuti in cui il contratto lontano ha una sola stampa
    ferma: userebbe un prezzo vecchio e allargherebbe la dispersione senza aggiungere informazione.
    """
    o = {b["time"]: b for b in old if b["time"][:10] <= until}
    n = {b["time"]: b for b in new if b["time"][:10] <= until}
    diffs = [n[t]["close"] - o[t]["close"] for t in (o.keys() & n.keys())
             if o[t]["volume"] >= min_volume and n[t]["volume"] >= min_volume]
    if len(diffs) < 100:
        raise SystemExit(f"solo {len(diffs)} minuti appaiati: spread non misurabile in modo affidabile")
    return statistics.median(diffs), len(diffs), statistics.stdev(diffs)


def shift(bar: dict, offset: float) -> dict:
    out = dict(bar)
    for k in ("open", "high", "low", "close", "vwap", "valueAreaHigh", "valueAreaLow"):
        if isinstance(out.get(k), (int, float)):
            out[k] = out[k] + offset
    for k in ("poc", "maxTick", "maxBid", "maxAsk", "maxPositiveDelta", "maxNegativeDelta"):
        if isinstance(out.get(k), dict) and "price" in out[k]:
            out[k] = {**out[k], "price": out[k]["price"] + offset}
    if out.get("levels"):
        out["levels"] = [{**L, "price": L["price"] + offset} for L in out["levels"]]
    return out


def value_area(volume_by_price: dict[float, int], percent: float = 70.0):
    prices = sorted(volume_by_price)
    total = sum(volume_by_price.values())
    poc = max(prices, key=lambda p: volume_by_price[p])
    lo = hi = prices.index(poc)
    taken, target = volume_by_price[poc], total * percent / 100
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


def to_sessions(bars, window):
    begin, end = window
    days = defaultdict(lambda: {"levels": {}, "bars": []})
    for b in bars:
        if not (begin <= b["time"][11:16] < end):
            continue
        d = days[b["time"][:10]]
        d["bars"].append(b)
        for L in b.get("levels", []):
            d["levels"][L["price"]] = d["levels"].get(L["price"], 0) + L["volume"]
    out = []
    for day in sorted(days):
        e = days[day]
        if not e["bars"]:
            continue
        poc = val = vah = None
        if e["levels"]:
            poc, val, vah = value_area(e["levels"])
        out.append({
            "date": day, "contract": e["bars"][-1].get("_contract"),
            "volume": sum(b["volume"] for b in e["bars"]),
            "delta": sum(b["delta"] for b in e["bars"]),
            "open": e["bars"][0]["open"], "close": e["bars"][-1]["close"],
            "high": max(b["high"] for b in e["bars"]), "low": min(b["low"] for b in e["bars"]),
            "poc": poc, "valueAreaLow": val, "valueAreaHigh": vah,
        })
    return out


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("candles", nargs="+", help="JSON di /candles per contratto, dal piu' vecchio al piu' recente")
    parser.add_argument("--cash", metavar="HH:MM-HH:MM", help="aggrega in sedute su questa finestra UTC")
    parser.add_argument("--out", required=True)
    args = parser.parse_args()

    series = [load(p) for p in args.candles]
    if len(series) < 2:
        raise SystemExit("servono almeno due contratti")

    # Si parte dal piu' recente e si va a ritroso: ogni contratto piu' vecchio accumula lo spread
    # di tutti i roll successivi, cosi' la serie finisce nei prezzi di oggi.
    offsets = [0.0]
    rolls = []
    for k in range(len(series) - 1, 0, -1):
        (old_name, old), (new_name, new) = series[k - 1], series[k]
        day = roll_date(old, new)
        if day is None:
            raise SystemExit(f"nessun roll rilevato fra {old_name} e {new_name}")
        med, n, sd = spread(old, new, day)
        rolls.append({"from": old_name, "to": new_name, "date": day,
                      "spread": round(med, 2), "pairedMinutes": n, "stdev": round(sd, 2)})
        offsets.insert(0, offsets[0] + med)

    merged = []
    for (name, bars), off, k in zip(series, offsets, range(len(series))):
        stop = rolls[len(rolls) - k - 1]["date"] if k < len(series) - 1 else None
        for b in bars:
            if stop is not None and b["time"][:10] >= stop:
                continue
            if k > 0 and b["time"][:10] < rolls[len(rolls) - k]["date"]:
                continue
            out = shift(b, off) if off else dict(b)
            out["_contract"] = name
            out["_offset"] = round(off, 2)
            merged.append(out)
    merged.sort(key=lambda b: b["time"])

    payload = {"schema": "fof-continuous-backadjusted-v1", "rolls": list(reversed(rolls)),
               "count": len(merged)}
    if args.cash:
        payload["window"] = args.cash
        payload["sessions"] = to_sessions(merged, tuple(args.cash.split("-")))
    else:
        payload["candles"] = merged
    json.dump(payload, open(args.out, "w"))

    for r in payload["rolls"]:
        print(f"roll {r['from']} -> {r['to']} il {r['date']}: spread {r['spread']:+.2f} punti "
              f"(mediana su {r['pairedMinutes']:,} minuti appaiati, dev.st. {r['stdev']:.2f})")
    print(f"{payload.get('count')} barre -> {args.out}"
          + (f", {len(payload['sessions'])} sedute" if args.cash else ""))


if __name__ == "__main__":
    main()
