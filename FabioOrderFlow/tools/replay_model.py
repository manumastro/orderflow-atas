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

# Quanto oltre il livello mettere lo stop, quando il rischio si misura dal livello.
LEVEL_BUFFER = 3.0

# Finestra oraria UTC in cui il modello e' dichiarato attivo: pre-market e prime due ore di RTH.
# Fuori da qui il dossier dice esplicitamente di non operare.
SESSION_WINDOW: tuple[str, str] | None = None

# Condizione 3: quanto delta contrario, dentro la candela che si forma, conta come perdita del
# controllo. A zero qualunque oscillazione iniziale cancellerebbe l'ordine, quindi serve una soglia.
CONTROL_FLIP_DELTA = 30


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


def near_level(bar: dict, levels: list[tuple[float, str]]):
    """Restituisce (etichetta, prezzo) del primo livello toccato, oppure None."""
    for price, name in levels:
        if bar["low"] - LEVEL_TOLERANCE <= price <= bar["high"] + LEVEL_TOLERANCE:
            return f"{name} {price:,.0f}", price
    return None


def signal(bars: list[dict], i: int, trades: list[dict], levels: list[tuple[float, str]]):
    """Valuta le condizioni 1-4 del modello alla chiusura della barra i. Nessun dato futuro."""
    if i < TAPE_WINDOW:
        return None

    if SESSION_WINDOW and not (SESSION_WINDOW[0] <= bars[i]["lastTime"][11:16] <= SESSION_WINDOW[1]):
        return None

    bar, previous = bars[i], bars[i - 1]

    # Il dossier non richiede un livello: i livelli sono contesto di mercato, non condizione.
    # Restano un filtro attivabile, ma senza livelli dichiarati la condizione non si applica.
    if levels:
        found = near_level(bar, levels)
        if found is None:
            return None
        level, level_price = found
    else:
        level, level_price = "nessun livello richiesto", bar["close"]

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

    # Conferma opzionale. Il dossier elenca tre condizioni, e Big Trades e Speed of Tape non
    # sono fra queste: restano strumenti di lettura. Si applicano solo se richiesti.
    recent = sorted(b["volume"] for b in bars[i - TAPE_WINDOW:i])
    median = recent[len(recent) // 2]
    if TAPE_FACTOR > 0 and bar["volume"] < TAPE_FACTOR * median:
        return None
    big = big_in_bar(trades, bar, side) if trades else []
    if trades and len(big) < MIN_BIG_TRADES:
        return None

    return {
        "side": side,
        "level": level,
        "levelPrice": level_price,
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


def resolve_with_tape(bars: list[dict], sig: dict, tape, times):
    """Esegue il setup sul tape, trade per trade, applicando anche la condizione 3.

    Il tape porta il tempo al millisecondo e la direzione gia' classificata, quindi dentro la
    candela in formazione si sa **l'ordine** degli eventi: se il controllo gira prima che il
    limit venga toccato, l'ordine si cancella come prescrive il dossier. La stessa informazione
    elimina l'ambiguita' fra stop e target colpiti nella stessa barra.
    """
    import bisect

    long = sig["side"] == "long"
    entry = sig["vah"] if long else sig["val"]
    stop = (sig["low"] if long else sig["high"]) if STOP_MODE == "bar" else (sig["val"] if long else sig["vah"])
    risk = abs(entry - stop)
    if risk == 0:
        return {"outcome": "va-degenere"}
    target = entry + risk if long else entry - risk

    filled = False
    for j in range(sig["bar"] + 1, min(sig["bar"] + 1 + PULLBACK_BARS, len(bars))):
        begin, end = parse(bars[j]["time"]), parse(bars[j]["lastTime"])
        lo, hi = bisect.bisect_left(times, begin), bisect.bisect_right(times, end)
        forming = 0  # delta cumulato della candela in formazione, azzerato a ogni barra
        for trade in tape[lo:hi]:
            price = trade[4]
            if (long and price <= entry) or (not long and price >= entry):
                filled = True
                fill_bar, fill_time = j, trade[0]
                break
            forming += trade[1] * trade[2]
            if (long and forming <= -CONTROL_FLIP_DELTA) or (not long and forming >= CONTROL_FLIP_DELTA):
                return {"outcome": "annullato-condizione-3", "entry": entry, "stop": stop,
                        "target": target, "risk": risk, "exitBar": j}
        if filled:
            break

    if not filled:
        return {"outcome": "non-eseguito", "entry": entry, "stop": stop, "target": target, "risk": risk}

    # Dal fill in poi l'esito si legge sul tape: il primo dei due prezzi toccato vince, senza ambiguita'.
    start = bisect.bisect_left(times, fill_time)
    for trade in tape[start:]:
        price = trade[4]
        if (long and price <= stop) or (not long and price >= stop):
            return {"outcome": "stop", "entry": entry, "stop": stop, "target": target, "risk": risk,
                    "filledBar": fill_bar, "exitBar": fill_bar, "exitTime": trade[0].isoformat(),
                    "result": -risk}
        if (long and price >= target) or (not long and price <= target):
            return {"outcome": "target", "entry": entry, "stop": stop, "target": target, "risk": risk,
                    "filledBar": fill_bar, "exitBar": fill_bar, "exitTime": trade[0].isoformat(),
                    "result": risk}

    last = tape[-1][4]
    return {"outcome": "aperto-a-fine-tape", "entry": entry, "stop": stop, "target": target,
            "risk": risk, "filledBar": fill_bar, "exitBar": fill_bar,
            "result": (last - entry) if long else (entry - last)}


def resolve(bars: list[dict], sig: dict):
    """Esegue l'ordine limit sul bordo della value area e ne segue l'esito, barra per barra."""
    long = sig["side"] == "long"
    entry = sig["vah"] if long else sig["val"]
    if STOP_MODE == "bar":
        stop = sig["low"] if long else sig["high"]
    elif STOP_MODE == "level":
        # Il rischio viene dal livello che il setup difende, non dall'ampiezza della barra:
        # uno stop dentro il range della barra stessa viene tolto dal rumore intrabarra.
        edge = min(sig["levelPrice"], sig["low"]) if long else max(sig["levelPrice"], sig["high"])
        stop = edge - LEVEL_BUFFER if long else edge + LEVEL_BUFFER
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
    global MIN_FLIP_DELTA, LEVEL_TOLERANCE, TAPE_FACTOR, PULLBACK_BARS, STOP_MODE, LEVEL_BUFFER
    global SESSION_WINDOW, CONTROL_FLIP_DELTA

    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("candles", help="JSON di /candles con --levels")
    parser.add_argument("--big", help="JSON di /cumulative con il filtro di volume")
    parser.add_argument("--level", action="append", default=[], metavar="PREZZO:NOME")
    parser.add_argument("--levels-from", action="append", default=[], metavar="CANDELE.JSON",
                        help="ricava POC, VAH, VAL, massimo e minimo da una sessione precedente")
    parser.add_argument("--flip", type=int, default=MIN_FLIP_DELTA, help="delta minimo del flip")
    parser.add_argument("--tolerance", type=float, default=LEVEL_TOLERANCE, help="distanza da un livello")
    parser.add_argument("--tape-factor", type=float, default=TAPE_FACTOR,
                        help="accelerazione del volume di barra richiesta; 0 disattiva il filtro")
    parser.add_argument("--pullback", type=int, default=PULLBACK_BARS, help="barre di validita' del limit")
    parser.add_argument("--stop", choices=["va", "bar", "level"], default="va",
                        help="'va' bordo opposto della value area, 'bar' estremo della barra, "
                             "'level' oltre il livello difeso")
    parser.add_argument("--buffer", type=float, default=LEVEL_BUFFER, help="punti oltre il livello")
    parser.add_argument("--window", metavar="HH:MM-HH:MM", help="finestra oraria UTC in cui operare")
    parser.add_argument("--tape", metavar="TAPE.JSON",
                        help="tape completo da /cumulative --min-volume 0 --compact: abilita la "
                             "condizione 3 e l'esito senza ambiguita'")
    parser.add_argument("--control", type=int, default=CONTROL_FLIP_DELTA,
                        help="delta contrario che conta come perdita del controllo")
    parser.add_argument("--developing", action="store_true",
                        help="aggiunge POC e bordi della value area in sviluppo della sessione")
    parser.add_argument("--funnel", action="store_true", help="mostra quante barre superano ogni condizione")
    args = parser.parse_args()

    MIN_FLIP_DELTA, LEVEL_TOLERANCE = args.flip, args.tolerance
    TAPE_FACTOR, PULLBACK_BARS, STOP_MODE = args.tape_factor, args.pullback, args.stop
    LEVEL_BUFFER = args.buffer
    SESSION_WINDOW = tuple(args.window.split("-")) if args.window else None
    CONTROL_FLIP_DELTA = args.control

    levels = []
    for path in args.levels_from:
        previous = json.load(open(path))["candles"]
        volume: dict[float, int] = {}
        for bar in previous:
            for level in bar.get("levels", []):
                volume[level["price"]] = volume.get(level["price"], 0) + level["volume"]
        tag = path.split("/")[-1].replace(".json", "")
        levels += [(price, f"{name}-{tag}") for price, name in developing_levels(volume)]
        levels += [(max(b["high"] for b in previous), f"max-{tag}"),
                   (min(b["low"] for b in previous), f"min-{tag}")]

    for raw in args.level:
        price, _, name = raw.partition(":")
        levels.append((float(price), name or "livello"))

    bars = json.load(open(args.candles))["candles"]
    trades = load_big(args.big)

    tape = times = None
    if args.tape:
        raw = json.load(open(args.tape))
        if not raw.get("compact"):
            raise SystemExit("il tape va scaricato con --compact")
        tape = [(parse(t[0]), t[1], t[2], t[3], t[4]) for t in raw["trades"]]
        times = [t[0] for t in tape]

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
    r_total = 0.0
    wins = 0
    risks: list[float] = []
    i = 0
    while i < len(bars):
        sig = signal(bars, i, trades, levels + developing[i])
        if sig is None:
            i += 1
            continue

        out = resolve_with_tape(bars, sig, tape, times) if tape else resolve(bars, sig)
        rome = parse(sig["time"]).astimezone(timezone.utc)
        print(f"barra {sig['bar']}  {sig['time'][11:19]} UTC ({rome.hour + 2:02d}:{rome.minute:02d} Roma)  "
              f"{sig['side'].upper()}  a {sig['level']}")
        print(f"   delta {sig['previousDelta']:+,} -> {sig['delta']:+,}   VA {sig['val']:,.0f}-{sig['vah']:,.0f}   "
              f"big {sig['big']} ({sig['bigVolume']:,} lotti)   tape {sig['tapeRatio']:.1f}x")
        if out["outcome"] == "annullato-condizione-3":
            print(f"   controllo girato prima del fill -> ordine cancellato\n")
        elif out["outcome"] == "non-eseguito":
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
            # In R il confronto fra varianti di stop e' leale: i punti premiano solo il rischio piu' largo.
            r_total += out.get("result", 0.0) / out["risk"]
            wins += 1 if out.get("result", 0.0) > 0 else 0
            risks.append(out["risk"])

        # Il modello non tiene due posizioni: si riprende dopo l'uscita.
        i = max(out.get("exitBar", sig["bar"]), sig["bar"]) + 1

    rate = f"{100 * wins / taken:.0f}%" if taken else "-"
    median_risk = sorted(risks)[len(risks) // 2] if risks else 0
    print(f"operazioni {taken}   vinte {wins} ({rate})   somma {total:+,.2f} punti   "
          f"{r_total:+.2f} R   rischio mediano {median_risk:,.2f} punti")


if __name__ == "__main__":
    main()
