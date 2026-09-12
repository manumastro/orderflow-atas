#!/usr/bin/env python3
"""Riesegue il modello operativo di riferimento su barre gia' registrate.

Il modello e' descritto in `docs/research/modelli/modello-40r-riferimento.md`. Questo script
non lo valida e non lo migliora: lo applica alla lettera a dati storici, in modo **causale**,
cioe' decidendo su ogni barra solo con cio' che era noto alla sua chiusura. Serve a rispondere
alla domanda "dove sarebbe entrato il modello" senza che la risposta sia influenzata dal sapere
come e' finita la giornata.

Ogni soglia che il modello lascia qualitativa e' qui una costante dichiarata in testa al file.
Cambiarla cambia i risultati: e' una scelta, non un fatto.

    ./replay_model.py candles.json --big big.json \
        --level 29500:massimo-notturno --level 29355:VAH-notte --level 29222:VAH-giovedi
"""

from __future__ import annotations

import argparse
import json
from datetime import datetime, timezone

# --- convenzioni dichiarate --------------------------------------------------

# Distanza entro cui una barra e' considerata "a un livello importante".
LEVEL_TOLERANCE = 10.0

# Un delta flip conta solo se il nuovo controllo e' misurabile: sotto questa soglia
# l'inversione di segno e' rumore di barra.
MIN_FLIP_DELTA = 150

# Accelerazione del tape: volume di barra rispetto alla mediana delle barre recenti.
TAPE_WINDOW = 20
TAPE_FACTOR = 1.3

# Big Trades che devono sostenere il lato, dentro la barra di segnale.
MIN_BIG_TRADES = 1

# Quante barre resta valido il limit sul bordo della value area prima di essere cancellato.
PULLBACK_BARS = 3

# Dove mettere lo stop: il modello ammette il bordo opposto della value area oppure l'estremo
# tecnico della barra. Su 40R le due scelte differiscono di circa il doppio.
STOP_MODE = "va"


def parse(raw: str) -> datetime:
    return datetime.fromisoformat(raw.replace("Z", "+00:00"))


def load_big(path: str | None) -> list[dict]:
    if not path:
        return []
    trades = json.load(open(path)).get("trades", [])
    for trade in trades:
        trade["_t"] = parse(trade["time"])
    return trades


def big_in_bar(trades: list[dict], bar: dict, side: str) -> list[dict]:
    """Big Trades eseguiti dentro la finestra temporale della barra, sul lato indicato."""
    begin, end = parse(bar["time"]), parse(bar["lastTime"])
    wanted = "Buy" if side == "long" else "Sell"
    return [t for t in trades if begin <= t["_t"] <= end and t["direction"] == wanted]


def developing_levels(volume_by_price: dict[float, int], fraction: float = 0.70):
    """POC e bordi della value area della sessione **fino alla barra precedente**.

    Il modello parla di livelli importanti includendo il POC e i bordi della value area: su un
    chart intraday quelli che il trader guarda sono anche quelli in sviluppo, non solo quelli
    ereditati dalle sessioni passate. Il calcolo usa solo barre gia' chiuse, quindi resta causale.
    """
    if not volume_by_price:
        return []

    total = sum(volume_by_price.values())
    prices = sorted(volume_by_price)
    poc = max(prices, key=lambda price: volume_by_price[price])

    low = high = prices.index(poc)
    inside = volume_by_price[poc]
    while inside < fraction * total and (low > 0 or high < len(prices) - 1):
        below = volume_by_price[prices[low - 1]] if low > 0 else -1
        above = volume_by_price[prices[high + 1]] if high < len(prices) - 1 else -1
        if above >= below:
            high += 1
            inside += volume_by_price[prices[high]]
        else:
            low -= 1
            inside += volume_by_price[prices[low]]

    return [(poc, "POC-sviluppo"), (prices[high], "VAH-sviluppo"), (prices[low], "VAL-sviluppo")]


def near_level(bar: dict, levels: list[tuple[float, str]]) -> str | None:
    for price, name in levels:
        if bar["low"] - LEVEL_TOLERANCE <= price <= bar["high"] + LEVEL_TOLERANCE:
            return f"{name} {price:,.0f}"
    return None


def signal(bars: list[dict], i: int, trades: list[dict], levels: list[tuple[float, str]]):
    """Valuta le condizioni 1-4 del modello alla chiusura della barra i. Nessun dato futuro."""
    if i < TAPE_WINDOW:
        return None

    bar, previous = bars[i], bars[i - 1]

    level = near_level(bar, levels)
    if level is None:
        return None

    # 2. delta flip: il controllo passa da un lato all'altro
    if previous["delta"] >= 0 or bar["delta"] < MIN_FLIP_DELTA:
        long_flip = False
    else:
        long_flip = True
    if previous["delta"] <= 0 or bar["delta"] > -MIN_FLIP_DELTA:
        short_flip = False
    else:
        short_flip = True
    if not (long_flip or short_flip):
        return None

    side = "long" if long_flip else "short"

    # 3. VA shift: la nuova value area si sposta nella direzione del lato
    if side == "long":
        shifted = bar["valueAreaHigh"] > previous["valueAreaHigh"] and bar["valueAreaLow"] > previous["valueAreaLow"]
    else:
        shifted = bar["valueAreaHigh"] < previous["valueAreaHigh"] and bar["valueAreaLow"] < previous["valueAreaLow"]
    if not shifted:
        return None

    # 4. conferma: Big Trades sul lato e tape in accelerazione
    recent = sorted(b["volume"] for b in bars[i - TAPE_WINDOW:i])
    median = recent[len(recent) // 2]
    tape = bar["volume"] >= TAPE_FACTOR * median
    big = big_in_bar(trades, bar, side)
    if len(big) < MIN_BIG_TRADES or not tape:
        return None

    return {
        "side": side,
        "level": level,
        "bar": i,
        "time": bar["lastTime"],
        "delta": bar["delta"],
        "previousDelta": previous["delta"],
        "vah": bar["valueAreaHigh"],
        "val": bar["valueAreaLow"],
        "big": len(big),
        "bigVolume": sum(t["volume"] for t in big),
        "tapeRatio": bar["volume"] / median if median else 0,
        "high": bar["high"],
        "low": bar["low"],
    }


def resolve(bars: list[dict], sig: dict):
    """Esegue l'ordine limit sul bordo della value area e ne segue l'esito, barra per barra."""
    long = sig["side"] == "long"
    entry = sig["vah"] if long else sig["val"]
    if STOP_MODE == "bar":
        stop = sig["low"] if long else sig["high"]
    else:
        stop = sig["val"] if long else sig["vah"]
    risk = abs(entry - stop)
    if risk == 0:
        return {"outcome": "va-degenere"}
    target = entry + risk if long else entry - risk

    filled_at = None
    for j in range(sig["bar"] + 1, min(sig["bar"] + 1 + PULLBACK_BARS, len(bars))):
        bar = bars[j]
        if (long and bar["low"] <= entry) or (not long and bar["high"] >= entry):
            filled_at = j
            break

    if filled_at is None:
        return {"outcome": "non-eseguito", "entry": entry, "stop": stop, "target": target, "risk": risk}

    for j in range(filled_at, len(bars)):
        bar = bars[j]
        hit_stop = bar["low"] <= stop if long else bar["high"] >= stop
        hit_target = bar["high"] >= target if long else bar["low"] <= target
        if hit_stop and hit_target:
            # Dentro una singola barra l'ordine dei due tocchi non e' ricostruibile dal footprint.
            return {"outcome": "ambiguo", "entry": entry, "stop": stop, "target": target,
                    "risk": risk, "filledBar": filled_at, "exitBar": j, "exitTime": bar["lastTime"]}
        if hit_stop:
            return {"outcome": "stop", "entry": entry, "stop": stop, "target": target, "risk": risk,
                    "filledBar": filled_at, "exitBar": j, "exitTime": bar["lastTime"], "result": -risk}
        if hit_target:
            return {"outcome": "target", "entry": entry, "stop": stop, "target": target, "risk": risk,
                    "filledBar": filled_at, "exitBar": j, "exitTime": bar["lastTime"], "result": risk}

    last = bars[-1]["close"]
    return {"outcome": "aperto-a-fine-sessione", "entry": entry, "stop": stop, "target": target,
            "risk": risk, "filledBar": filled_at, "result": (last - entry) if long else (entry - last)}


def main() -> None:
    global MIN_FLIP_DELTA, LEVEL_TOLERANCE, TAPE_FACTOR, PULLBACK_BARS, STOP_MODE

    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("candles", help="JSON di /candles con --levels")
    parser.add_argument("--big", help="JSON di /cumulative con il filtro di volume")
    parser.add_argument("--level", action="append", default=[], metavar="PREZZO:NOME")
    parser.add_argument("--flip", type=int, default=MIN_FLIP_DELTA, help="delta minimo del flip")
    parser.add_argument("--tolerance", type=float, default=LEVEL_TOLERANCE, help="distanza da un livello")
    parser.add_argument("--tape", type=float, default=TAPE_FACTOR, help="fattore di accelerazione del tape")
    parser.add_argument("--pullback", type=int, default=PULLBACK_BARS, help="barre di validita' del limit")
    parser.add_argument("--stop", choices=["va", "bar"], default="va",
                        help="'va' usa il bordo opposto della value area, 'bar' l'estremo della barra")
    parser.add_argument("--developing", action="store_true",
                        help="aggiunge POC e bordi della value area in sviluppo della sessione")
    parser.add_argument("--funnel", action="store_true", help="mostra quante barre superano ogni condizione")
    args = parser.parse_args()

    MIN_FLIP_DELTA, LEVEL_TOLERANCE = args.flip, args.tolerance
    TAPE_FACTOR, PULLBACK_BARS, STOP_MODE = args.tape, args.pullback, args.stop

    levels = []
    for raw in args.level:
        price, _, name = raw.partition(":")
        levels.append((float(price), name or "livello"))

    bars = json.load(open(args.candles))["candles"]
    trades = load_big(args.big)

    # Livelli in sviluppo precalcolati barra per barra, ciascuno con le sole barre precedenti.
    developing: list[list[tuple[float, str]]] = []
    if args.developing:
        running: dict[float, int] = {}
        for bar in bars:
            developing.append(developing_levels(running))
            for level in bar.get("levels", []):
                running[level["price"]] = running.get(level["price"], 0) + level["volume"]
    else:
        developing = [[] for _ in bars]

    print(f"barre {len(bars)}  big trades {len(trades)}  livelli {len(levels)}")
    print(f"soglie: tolleranza {LEVEL_TOLERANCE:g}, flip {MIN_FLIP_DELTA}, "
          f"tape {TAPE_FACTOR:g}x mediana({TAPE_WINDOW}), big >= {MIN_BIG_TRADES}, pullback {PULLBACK_BARS} barre\n")

    if args.funnel:
        counts = dict(livello=0, flip=0, shift=0, conferma=0)
        for i in range(TAPE_WINDOW, len(bars)):
            bar, previous = bars[i], bars[i - 1]
            if near_level(bar, levels + developing[i]) is None:
                continue
            counts["livello"] += 1
            long_flip = previous["delta"] < 0 <= bar["delta"] and bar["delta"] >= MIN_FLIP_DELTA
            short_flip = previous["delta"] > 0 >= bar["delta"] and bar["delta"] <= -MIN_FLIP_DELTA
            if not (long_flip or short_flip):
                continue
            counts["flip"] += 1
            side = "long" if long_flip else "short"
            shifted = (bar["valueAreaHigh"] > previous["valueAreaHigh"] and bar["valueAreaLow"] > previous["valueAreaLow"]) if side == "long" \
                else (bar["valueAreaHigh"] < previous["valueAreaHigh"] and bar["valueAreaLow"] < previous["valueAreaLow"])
            if not shifted:
                continue
            counts["shift"] += 1
            recent = sorted(b["volume"] for b in bars[i - TAPE_WINDOW:i])
            median = recent[len(recent) // 2]
            if len(big_in_bar(trades, bar, side)) >= MIN_BIG_TRADES and bar["volume"] >= TAPE_FACTOR * median:
                counts["conferma"] += 1
        print("imbuto: " + "  ".join(f"{k} {v}" for k, v in counts.items()) + "\n")

    total = 0.0
    taken = 0
    i = 0
    while i < len(bars):
        sig = signal(bars, i, trades, levels + developing[i])
        if sig is None:
            i += 1
            continue

        out = resolve(bars, sig)
        rome = parse(sig["time"]).astimezone(timezone.utc)
        print(f"barra {sig['bar']}  {sig['time'][11:19]} UTC ({rome.hour + 2:02d}:{rome.minute:02d} Roma)  "
              f"{sig['side'].upper()}  a {sig['level']}")
        print(f"   delta {sig['previousDelta']:+,} -> {sig['delta']:+,}   VA {sig['val']:,.0f}-{sig['vah']:,.0f}   "
              f"big {sig['big']} ({sig['bigVolume']:,} lotti)   tape {sig['tapeRatio']:.1f}x")
        if out["outcome"] == "non-eseguito":
            print(f"   limit {out['entry']:,.2f} non toccato in {PULLBACK_BARS} barre -> cancellato\n")
        elif out["outcome"] in ("ambiguo", "va-degenere"):
            print(f"   esito {out['outcome']}\n")
        else:
            print(f"   entry {out['entry']:,.2f}  SL {out['stop']:,.2f}  TP {out['target']:,.2f}  "
                  f"rischio {out['risk']:,.2f} punti")
            print(f"   esito {out['outcome']} alla barra {out.get('exitBar', '-')} "
                  f"{out.get('exitTime', '')[11:19]}  risultato {out.get('result', 0):+,.2f} punti\n")
            total += out.get("result", 0.0)
            taken += 1

        # Il modello non tiene due posizioni: si riprende dopo l'uscita.
        i = max(out.get("exitBar", sig["bar"]), sig["bar"]) + 1

    print(f"operazioni eseguite {taken}   somma {total:+,.2f} punti")


if __name__ == "__main__":
    main()
