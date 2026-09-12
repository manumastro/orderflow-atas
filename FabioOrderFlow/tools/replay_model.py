#!/usr/bin/env python3
"""Riesegue The Prop Firm Model su barre gia' registrate, nell'ordine del dossier.

Il dossier e' in `prop/prop_firm_model/`, trascritto in
`docs/research/modelli/dossier-prop-firm-model.md`. Questo script lo applica in modo **causale**:
ogni decisione usa solo cio' che era noto in quel momento. Serve a misurare, non a validare.

Il file e' organizzato come il dossier, una sezione per pagina, e ogni punto che il dossier lascia
aperto e' una costante dichiarata oppure un'opzione di riga di comando. Le scelte sono di chi
scrive lo script, non del dossier: cambiarle cambia i risultati.

    ./replay_model.py candles.json --tape tape.json --window 13:30-15:30 --manage 30
"""

from __future__ import annotations

import argparse
import bisect
import json
from datetime import datetime

# ============================================================ pagina 02: chart setup
# Il dossier prescrive il template OrderTrack con le linee di value area attive. La percentuale
# di value area non e' leggibile nello screenshot del pannello, ed e' il parametro che determina
# la distanza di ogni stop e di ogni target. Si ricalcola quindi dal footprint di barra.
VALUE_AREA_PERCENT = 70.0

# ============================================================ pagina 03: OCO
# Il rischio si fissa in valuta e la quantita' segue dallo stop. Il cap giornaliero di 6R e'
# l'unico limite di frequenza dichiarato in tutto il dossier.
RISK_DOLLARS = 250.0
POINT_VALUE = 20.0          # NQ full: gli importi degli esempi live sono coerenti con questo
DAILY_CAP_R = 6.0

# ============================================================ pagina 04: le tre condizioni
# 01 - auction flip. Il dossier non dice quanto grande debba essere il flip.
MIN_FLIP_DELTA = 40

# 02 - value area shift. "Positioned higher" ammette piu' letture.
VA_SHIFT_RULE = "both"      # both | val | mid

# 03 - side control mentre l'ordine e' pendente. "Who is in control" non e' definito.
PENDING_CONTROL_DELTA = 30

# "All aligned": monitor side control on the active position. Uscita prima di stop o target.
# Zero disattiva: il dossier prescrive di monitorare, non dice con quale soglia agire.
ACTIVE_CONTROL_DELTA = 0

# Quanto resta valido un ordine pendente. Il dossier non lo dice.
PULLBACK_BARS = 3

# ============================================================ pagina 07: session timing
# ON: pre-market e prime due ore della RTH di New York. OFF: pranzo e sera.
SESSION_WINDOW: tuple[str, str] | None = None

STOP_MODE = "va"            # va = VAL della candela, bar = minimo della candela


def parse(raw: str) -> datetime:
    return datetime.fromisoformat(raw.replace("Z", "+00:00"))


# ------------------------------------------------------------ pagina 02: value area

def value_area(volume_by_price: dict[float, int], percent: float):
    """POC e bordi della value area, espandendo dal POC verso il lato a volume maggiore."""
    if not volume_by_price:
        return None
    total = sum(volume_by_price.values())
    prices = sorted(volume_by_price)
    poc = max(prices, key=lambda price: volume_by_price[price])
    low = high = prices.index(poc)
    inside = volume_by_price[poc]
    while inside < percent / 100 * total and (low > 0 or high < len(prices) - 1):
        below = volume_by_price[prices[low - 1]] if low > 0 else -1
        above = volume_by_price[prices[high + 1]] if high < len(prices) - 1 else -1
        if above >= below:
            high += 1
            inside += volume_by_price[prices[high]]
        else:
            low -= 1
            inside += volume_by_price[prices[low]]
    return poc, prices[low], prices[high]


def bar_value_area(bar: dict, percent: float):
    """Value area della singola barra. Con la percentuale di ATAS si usano i campi nativi."""
    if percent is None:
        return bar["valueAreaLow"], bar["valueAreaHigh"]
    levels = {level["price"]: level["volume"] for level in bar.get("levels", [])}
    computed = value_area(levels, percent)
    if computed is None:
        return bar["valueAreaLow"], bar["valueAreaHigh"]
    _, val, vah = computed
    return val, vah


# ------------------------------------------------------------ pagina 04: le tre condizioni

def condition_01_auction_flip(previous: dict, bar: dict) -> str | None:
    """Il delta gira: i compratori prendono il controllo dai venditori, o viceversa."""
    if previous["delta"] < 0 and bar["delta"] >= MIN_FLIP_DELTA:
        return "long"
    if previous["delta"] > 0 and bar["delta"] <= -MIN_FLIP_DELTA:
        return "short"
    return None


def condition_02_value_area_shift(side: str, previous_va, current_va) -> bool:
    """La value area della nuova candela e' posizionata piu' in alto, o piu' in basso."""
    (prev_val, prev_vah), (val, vah) = previous_va, current_va
    if VA_SHIFT_RULE == "val":
        return val > prev_val if side == "long" else vah < prev_vah
    if VA_SHIFT_RULE == "mid":
        return ((val + vah) / 2 > (prev_val + prev_vah) / 2) if side == "long" \
            else ((val + vah) / 2 < (prev_val + prev_vah) / 2)
    return (vah > prev_vah and val > prev_val) if side == "long" \
        else (vah < prev_vah and val < prev_val)


def control_lost(side: str, cumulative_delta: int, threshold: int) -> bool:
    """Condizione 03 e gestione della posizione: il lato ha perso il controllo."""
    if threshold <= 0:
        return False
    return cumulative_delta <= -threshold if side == "long" else cumulative_delta >= threshold


# ------------------------------------------------------------ segnale

def signal(bars, i, vas, levels):
    if i < 1:
        return None
    if SESSION_WINDOW and not (SESSION_WINDOW[0] <= bars[i]["lastTime"][11:16] <= SESSION_WINDOW[1]):
        return None

    previous, bar = bars[i - 1], bars[i]

    side = condition_01_auction_flip(previous, bar)
    if side is None:
        return None
    if not condition_02_value_area_shift(side, vas[i - 1], vas[i]):
        return None

    # Il livello non e' una condizione del dossier: e' contesto. Resta un filtro attivabile.
    if levels:
        near = next((f"{name} {price:,.0f}" for price, name in levels
                     if bar["low"] - 10 <= price <= bar["high"] + 10), None)
        if near is None:
            return None
    else:
        near = "nessun filtro di livello"

    val, vah = vas[i]
    return {
        "bar": i, "side": side, "level": near, "time": bar["lastTime"],
        "delta": bar["delta"], "previousDelta": previous["delta"],
        "vah": vah, "val": val, "high": bar["high"], "low": bar["low"],
    }


# ------------------------------------------------------------ pagine 01 e 03: esecuzione

def execute(bars, sig, tape, times):
    """Piazza il limit, applica la condizione 03, poi gestisce la posizione aperta sul tape.

    Tutto si legge sul tape, che porta l'ordine temporale degli eventi: senza di esso un target
    verrebbe contato come raggiunto anche quando il suo prezzo e' stato stampato prima del fill.
    """
    long = sig["side"] == "long"
    entry = sig["vah"] if long else sig["val"]
    stop = (sig["low"] if long else sig["high"]) if STOP_MODE == "bar" else (sig["val"] if long else sig["vah"])
    risk = abs(entry - stop)
    if risk == 0:
        return {"outcome": "va-degenere"}
    target = entry + risk if long else entry - risk

    quantity = max(1, round(RISK_DOLLARS / (risk * POINT_VALUE)))

    # --- ordine pendente: condizione 03, il controllo va monitorato mentre la candela si forma
    fill_time = None
    for j in range(sig["bar"] + 1, min(sig["bar"] + 1 + PULLBACK_BARS, len(bars))):
        begin, end = parse(bars[j]["time"]), parse(bars[j]["lastTime"])
        lo, hi = bisect.bisect_left(times, begin), bisect.bisect_right(times, end)
        forming = 0
        for trade in tape[lo:hi]:
            price = trade[4]
            if (long and price <= entry) or (not long and price >= entry):
                fill_time = trade[0]
                break
            forming += trade[1] * trade[2]
            if control_lost(sig["side"], forming, PENDING_CONTROL_DELTA):
                return {"outcome": "cancellato-cond3", "entry": entry, "stop": stop,
                        "target": target, "risk": risk, "quantity": quantity}
        if fill_time is not None:
            break

    if fill_time is None:
        return {"outcome": "non-eseguito", "entry": entry, "stop": stop,
                "target": target, "risk": risk, "quantity": quantity}

    # --- posizione aperta: "monitor side control on the active position"
    start = bisect.bisect_left(times, fill_time)
    since_fill = 0
    for trade in tape[start:]:
        price = trade[4]
        if (long and price <= stop) or (not long and price >= stop):
            return done("stop", -risk, entry, stop, target, risk, quantity, trade[0])
        if (long and price >= target) or (not long and price <= target):
            return done("target", risk, entry, stop, target, risk, quantity, trade[0])
        since_fill += trade[1] * trade[2]
        if control_lost(sig["side"], since_fill, ACTIVE_CONTROL_DELTA):
            result = (price - entry) if long else (entry - price)
            return done("uscita-controllo", result, entry, stop, target, risk, quantity, trade[0])

    last = tape[-1][4]
    return done("fine-tape", (last - entry) if long else (entry - last),
                entry, stop, target, risk, quantity, tape[-1][0])


def done(outcome, result, entry, stop, target, risk, quantity, when):
    return {"outcome": outcome, "entry": entry, "stop": stop, "target": target, "risk": risk,
            "quantity": quantity, "result": result, "dollars": result * POINT_VALUE * quantity,
            "exitTime": when.isoformat(), "r": result / risk}


def main() -> None:
    global VALUE_AREA_PERCENT, RISK_DOLLARS, POINT_VALUE, DAILY_CAP_R
    global MIN_FLIP_DELTA, VA_SHIFT_RULE, PENDING_CONTROL_DELTA, ACTIVE_CONTROL_DELTA
    global PULLBACK_BARS, SESSION_WINDOW, STOP_MODE

    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("candles", help="JSON di /candles con --levels")
    parser.add_argument("--tape", required=True, help="JSON di /cumulative --min-volume 0 --compact")
    parser.add_argument("--level", action="append", default=[], metavar="PREZZO:NOME")
    parser.add_argument("--levels-from", action="append", default=[], metavar="CANDELE.JSON")

    page02 = parser.add_argument_group("pagina 02 - chart setup")
    page02.add_argument("--va-percent", type=float, default=None,
                        help=f"percentuale di value area di barra; omessa usa quella di ATAS")

    page03 = parser.add_argument_group("pagina 03 - OCO")
    page03.add_argument("--risk", type=float, default=RISK_DOLLARS, help="rischio per operazione in dollari")
    page03.add_argument("--point-value", type=float, default=POINT_VALUE, help="valore del punto")
    page03.add_argument("--daily-cap", type=float, default=DAILY_CAP_R, help="cap giornaliero in R; 0 disattiva")

    page04 = parser.add_argument_group("pagina 04 - le tre condizioni")
    page04.add_argument("--flip", type=int, default=MIN_FLIP_DELTA, help="condizione 01: delta minimo del flip")
    page04.add_argument("--va-shift", choices=["both", "val", "mid"], default=VA_SHIFT_RULE,
                        help="condizione 02: come si legge 'positioned higher'")
    page04.add_argument("--pending-control", type=int, default=PENDING_CONTROL_DELTA,
                        help="condizione 03: delta contrario che cancella l'ordine pendente")
    page04.add_argument("--manage", type=int, default=ACTIVE_CONTROL_DELTA,
                        help="delta contrario che chiude la posizione aperta; 0 disattiva")
    page04.add_argument("--pullback", type=int, default=PULLBACK_BARS, help="barre di validita' del limit")
    page04.add_argument("--stop", choices=["va", "bar"], default=STOP_MODE)

    page07 = parser.add_argument_group("pagina 07 - session timing")
    page07.add_argument("--window", metavar="HH:MM-HH:MM", help="finestra oraria UTC")

    parser.add_argument("--quiet", action="store_true", help="solo la riga di riepilogo")
    parser.add_argument("--json", action="store_true", help="riepilogo in JSON, per aggregare piu' sessioni")
    args = parser.parse_args()
    if args.json:
        args.quiet = True

    VALUE_AREA_PERCENT = args.va_percent
    RISK_DOLLARS, POINT_VALUE, DAILY_CAP_R = args.risk, args.point_value, args.daily_cap
    MIN_FLIP_DELTA, VA_SHIFT_RULE = args.flip, args.va_shift
    PENDING_CONTROL_DELTA, ACTIVE_CONTROL_DELTA = args.pending_control, args.manage
    PULLBACK_BARS, STOP_MODE = args.pullback, args.stop
    SESSION_WINDOW = tuple(args.window.split("-")) if args.window else None

    bars = json.load(open(args.candles))["candles"]
    raw = json.load(open(args.tape))
    if not raw.get("compact"):
        raise SystemExit("il tape va scaricato con --compact")
    tape = [(parse(t[0]), t[1], t[2], t[3], t[4]) for t in raw["trades"]]
    times = [t[0] for t in tape]

    vas = [bar_value_area(bar, VALUE_AREA_PERCENT) for bar in bars]

    levels = []
    for path in args.levels_from:
        previous = json.load(open(path))["candles"]
        volume: dict[float, int] = {}
        for bar in previous:
            for level in bar.get("levels", []):
                volume[level["price"]] = volume.get(level["price"], 0) + level["volume"]
        tag = path.split("/")[-1].replace(".json", "")
        computed = value_area(volume, VALUE_AREA_PERCENT or 70.0)
        if computed:
            poc, val, vah = computed
            levels += [(poc, f"POC-{tag}"), (vah, f"VAH-{tag}"), (val, f"VAL-{tag}")]
    for entry in args.level:
        price, _, name = entry.partition(":")
        levels.append((float(price), name or "livello"))

    taken = wins = 0
    r_total = dollars = 0.0
    cancelled = unfilled = managed = 0
    risks: list[float] = []

    i = 1
    while i < len(bars):
        sig = signal(bars, i, vas, levels)
        if sig is None:
            i += 1
            continue

        out = execute(bars, sig, tape, times)

        if out["outcome"] == "cancellato-cond3":
            cancelled += 1
        elif out["outcome"] == "non-eseguito":
            unfilled += 1
        elif out["outcome"] != "va-degenere":
            taken += 1
            wins += 1 if out["result"] > 0 else 0
            managed += 1 if out["outcome"] == "uscita-controllo" else 0
            r_total += out["r"]
            dollars += out["dollars"]
            risks.append(out["risk"])
            if not args.quiet:
                rome = parse(sig["time"]).hour + 2
                print(f"barra {sig['bar']}  {sig['time'][11:19]} UTC ({rome:02d}:{sig['time'][14:16]} Roma)  "
                      f"{sig['side'].upper()}  {sig['level']}")
                print(f"   delta {sig['previousDelta']:+,} -> {sig['delta']:+,}   "
                      f"VA {sig['val']:,.2f}-{sig['vah']:,.2f}   qty {out['quantity']}")
                print(f"   entry {out['entry']:,.2f}  SL {out['stop']:,.2f}  TP {out['target']:,.2f}  "
                      f"rischio {out['risk']:,.2f} pt")
                print(f"   {out['outcome']} {out['exitTime'][11:19]}  "
                      f"{out['r']:+.2f} R  {out['dollars']:+,.0f} $\n")

        if DAILY_CAP_R and r_total >= DAILY_CAP_R:
            break

        i = sig["bar"] + 1

    median = sorted(risks)[len(risks) // 2] if risks else 0
    summary = {"trades": taken, "wins": wins, "r": round(r_total, 2), "dollars": round(dollars),
               "medianRisk": median, "cancelled": cancelled, "unfilled": unfilled, "managed": managed}
    if args.json:
        print(json.dumps(summary))
        return
    rate = f"{100 * wins / taken:.0f}%" if taken else "-"
    print(f"operazioni {taken}   vinte {wins} ({rate})   {r_total:+.2f} R   {dollars:+,.0f} $   "
          f"rischio mediano {median:,.2f} pt   cancellati {cancelled}   non eseguiti {unfilled}   "
          f"uscite per controllo {managed}")


if __name__ == "__main__":
    main()
