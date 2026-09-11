#!/usr/bin/env python3
"""Costruisce il profilo della sessione cash da footprint scaricati con il Data Bridge.

Aggrega il volume per prezzo di tutte le barre della finestra, individua il POC come
livello di volume massimo e ricava l'area di valore espandendo dal POC finche' non
copre la frazione dichiarata del volume totale.

E' una descrizione: non classifica il comportamento, non emette segnali e non decide
se un livello sia stato accettato o rifiutato.
"""

import json
import sys
from collections import defaultdict
from pathlib import Path

VALUE_AREA_FRACTION = 0.70


def profile(candles):
    volume_by_price = defaultdict(float)
    delta_by_price = defaultdict(float)
    for candle in candles:
        for level in candle.get("levels") or []:
            volume_by_price[level["price"]] += level["volume"]
            delta_by_price[level["price"]] += level["ask"] - level["bid"]
    return volume_by_price, delta_by_price


def value_area(volume_by_price, fraction=VALUE_AREA_FRACTION):
    """Espande dal POC verso il lato con piu' volume, come fa il profilo classico."""
    prices = sorted(volume_by_price)
    if not prices:
        return None, None, None

    total = sum(volume_by_price.values())
    poc = max(prices, key=lambda price: volume_by_price[price])
    index = prices.index(poc)

    low = high = index
    covered = volume_by_price[poc]
    target = total * fraction
    while covered < target and (low > 0 or high < len(prices) - 1):
        below = volume_by_price[prices[low - 1]] if low > 0 else -1
        above = volume_by_price[prices[high + 1]] if high < len(prices) - 1 else -1
        if above >= below:
            high += 1
            covered += above
        else:
            low -= 1
            covered += below
    return poc, prices[low], prices[high]


def main():
    print(f"{'sessione':12s}{'volume':>12s}{'delta':>9s}{'POC':>9s}{'VAL':>9s}{'VAH':>9s}"
          f"{'high':>9s}{'low':>9s}{'open':>9s}{'close':>9s}")
    previous = None
    for path in sorted(sys.argv[1:]):
        payload = json.loads(Path(path).read_text())
        candles = payload["candles"]
        if not candles:
            continue

        volume_by_price, _ = profile(candles)
        poc, val, vah = value_area(volume_by_price)
        label = Path(path).stem.replace("cash-", "2026-")
        print(f"{label:12s}{sum(volume_by_price.values()):12,.0f}"
              f"{sum(c['delta'] for c in candles):9,.0f}"
              f"{poc:9,.0f}{val:9,.0f}{vah:9,.0f}"
              f"{max(c['high'] for c in candles):9,.0f}{min(c['low'] for c in candles):9,.0f}"
              f"{candles[0]['open']:9,.0f}{candles[-1]['close']:9,.0f}")

        if previous:
            prev_poc, prev_val, prev_vah = previous
            opening = candles[0]["open"]
            where = ("sopra la value area" if opening > prev_vah
                     else "sotto la value area" if opening < prev_val
                     else "dentro la value area")
            print(f"{'':12s}apertura {opening:,.0f} {where} precedente "
                  f"({prev_val:,.0f}-{prev_vah:,.0f}, POC {prev_poc:,.0f})")
        previous = (poc, val, vah)


if __name__ == "__main__":
    main()
