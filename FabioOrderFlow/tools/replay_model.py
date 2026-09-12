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
from datetime import datetime, timedelta
from statistics import median

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

# Un limit order si piazza **lontano** dal prezzo: per un long al VAH, il VAH dev'essere sotto la
# chiusura della barra di segnale. Se sta sopra, l'ordine si esegue al primo trade e non e' piu'
# un ingresso su pullback ma un ingresso a mercato, con lo stop gia' vicinissimo. Il dossier lo
# implica due volte: "wait for the pullback" e "if you do not get filled, do not chase".
REQUIRE_PULLBACK = True

# ============================================================ pagina 07: session timing
# ON: pre-market e prime due ore della RTH di New York. OFF: pranzo e sera.
SESSION_WINDOW: tuple[str, str] | None = None

STOP_MODE = "va"            # va = VAL della candela, bar = minimo della candela

# ============================================================ ipotesi sotto test
# Nessuna di queste viene dal dossier in forma operativa: sono tentativi di rendere misurabili
# le parti che il dossier lascia alla discrezione, piu' un controllo sul modello stesso.

# Controllo non valido, tenuto per non ripeterlo. Invertire il lato cambia anche la geometria:
# uno short al VAL si riempie al primo trade, perche' il prezzo a fine barra sta gia' sopra, e
# lo stop al VAH e' subito sopra il fill. Il 5% che ne esce misura quell'artefatto, non il segnale.
# L'informazione che questo test avrebbe dato e' comunque gia' nel win rate di base: un 39%
# significa che dal prezzo di ingresso il mercato raggiunge lo stop prima del target nel 61% dei casi.
INVERT = False

# Lettura per assorbimento delle due condizioni: si testano separatamente perche' potrebbero
# avere il verso sbagliato una senza l'altra.
REVERSE_FLIP = False
REVERSE_SHIFT = False

# Controllo valido: stessa costruzione di entry, stop e target, ma **senza le tre condizioni**.
# Il lato viene dal corpo della barra. Se il modello con le condizioni non batte questo, le
# condizioni non aggiungono informazione.
NULL_MODEL = False

# Contesto "trending markets": stacked VA shifts in one direction (pagina 04).
TREND_BARS = 0

# Contesto "key level mean reversion": il segnale sta a un livello ereditato e va contro la
# direzione delle barre precedenti.
MEAN_REVERSION = False

# Pagina 07: "not too fast, not too slow". Durata della barra 40R in secondi.
PACE_RANGE: tuple[float, float] | None = None

# Pattern 04 della library: assorbimento. Delta di barra opposto al corpo della barra.
ABSORPTION = False

# Tolleranza in punti con cui una barra 40R si considera "sul livello" del profile framing
# del giorno prima. Non viene dal dossier: viene dal primo transcript del corso, dove ogni
# esecuzione parte da un livello marcato prima dell'apertura.
LEVEL_TOLERANCE = 10.0

# --- dal primo transcript del corso, non dal dossier ---------------------------------------
# Il dossier chiude sempre a 1:1 con un OCO. Nella live Fabio non lo fa mai: porta lo stop a
# pareggio appena la posizione respira ("why leaving floating profit on the market?") e tiene
# aperto molto oltre 1:1. A fine sessione dichiara "we didn't take a single stop loss for the
# day, only breakeven and take profit". Queste due variabili servono a misurare quella
# differenza, che cambia il gioco: a 1:1 un 48% di vittorie non puo' guadagnare, con lo stop a
# pareggio le perdite diventano zeri e bastano pochi target per stare sopra.
BREAKEVEN_R = 0.0           # frazione di R favorevole dopo cui lo stop va all'ingresso
TARGET_R = 1.0              # obiettivo in multipli di R

# ============================================================ big trades e speed of tape
# Nemmeno questi vengono dal dossier. Vengono dalla prima live del corso, dove sono il filtro
# che decide se un livello merita un'esecuzione:
#
#   "you not only have an amazing cluster of aggressive holder on this horizontal level,
#    but you are also supported by speed"
#   "look how much is low the activity. If you try to take this breakout, you will incur a lot
#    of losses. [...] It's keeping you out of useless moment in the market."
#
# I big trades sono i singoli cumulative trade sopra una certa size: Fabio cita 60, 84, 124
# contratti su NASDAQ. Sul tape di una sessione il 99esimo percentile sta intorno a 10 e il
# 99,9esimo intorno a 34, quindi una soglia di 20 seleziona lo 0,3% superiore delle stampe.
BIG_TRADE_SIZE = 20         # volume minimo perche' una stampa sia un big trade
BIG_TRADE_COUNT = 0         # quanti ne servono dal lato del segnale; 0 disattiva il filtro
BIG_TRADE_DOMINANCE = False # ne servono piu' dal lato del segnale che dal lato opposto

# La speed of tape e' volume aggressivo per secondo, separato per lato. La soglia assoluta non
# e' trasferibile fra sessioni diverse, quindi si misura in multipli della mediana della sessione
# stessa: "1,5" vuol dire che in quella finestra il lato del segnale sta andando una volta e
# mezza piu' veloce di quanto vada di solito quel giorno.
SPEED_MULTIPLE = 0.0        # 0 disattiva il filtro
SPEED_DOMINANCE = False     # il lato del segnale deve anche essere piu' veloce di quello opposto

# Finestra su cui si leggono entrambi. None = la barra 40R stessa, che e' cio' che si vede
# quando la candela chiude.
FLOW_WINDOW: float | None = None

# Nella live i big trades non sono un filtro sulla candela: sono cio' che **costruisce il
# livello**. Fabio marca il prezzo dove si sono accumulati e poi lo tratta per il resto della
# sessione ("this is the level that created the most important breakout, so I mark this";
# "if you want to see a reload really fast [...] we can mark this last movement with profile
# and see what's the most important price level"). Questi due parametri implementano quella
# lettura: si tengono i prezzi con piu' volume di big trades accumulato **prima** della barra
# di segnale, e la barra deve arrivarci sopra.
BIG_LEVELS = 0              # quanti prezzi tenere; 0 disattiva
BIG_LEVEL_AGE = 60.0        # secondi minimi perche' un livello sia gia' formato


class Flow:
    """Letture di tape per una sessione, con la linea di base della sessione stessa."""

    def __init__(self, tape, times, bars):
        self.tape, self.times = tape, times
        rates: dict[int, list[float]] = {1: [], -1: []}
        for bar in bars:
            begin, end = parse(bar["time"]), parse(bar["lastTime"])
            seconds = max((end - begin).total_seconds(), 0.001)
            buy, sell = self.volume(begin, end)
            rates[1].append(buy / seconds)
            rates[-1].append(sell / seconds)
        # Una mediana nulla renderebbe il filtro sempre vero: in quel caso non c'e' linea di base.
        self.baseline = {k: (median(v) if v and median(v) > 0 else None) for k, v in rates.items()}

    def window(self, begin, end):
        lo = bisect.bisect_left(self.times, begin)
        hi = bisect.bisect_right(self.times, end)
        return self.tape[lo:hi]

    def volume(self, begin, end) -> tuple[int, int]:
        buy = sell = 0
        for trade in self.window(begin, end):
            if trade[1] > 0:
                buy += trade[2]
            else:
                sell += trade[2]
        return buy, sell

    def rate(self, begin, end) -> dict[int, float]:
        seconds = max((end - begin).total_seconds(), 0.001)
        buy, sell = self.volume(begin, end)
        return {1: buy / seconds, -1: sell / seconds}

    def big_levels(self, before, size, keep, tick=0.25) -> list[float]:
        """I prezzi su cui si e' accumulato piu' volume di big trades prima di `before`.

        Causale per costruzione: la ricerca binaria taglia il tape all'istante richiesto, quindi
        un livello puo' essere usato solo dopo essersi formato.
        """
        hi = bisect.bisect_left(self.times, before)
        volume: dict[float, int] = {}
        for trade in self.tape[:hi]:
            if trade[2] >= size:
                price = round(trade[4] / tick) * tick
                volume[price] = volume.get(price, 0) + trade[2]
        return [price for price, _ in sorted(volume.items(), key=lambda kv: -kv[1])[:keep]]

    def big(self, begin, end, size) -> dict[int, int]:
        counted = {1: 0, -1: 0}
        for trade in self.window(begin, end):
            if trade[2] >= size:
                counted[1 if trade[1] > 0 else -1] += 1
        return counted


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
    """Il delta gira: i compratori prendono il controllo dai venditori, o viceversa.

    REVERSE_FLIP legge lo stesso evento come assorbimento invece che come continuazione. Il
    dossier dice continuazione; la misura diretta sulle barriere simmetriche dice che il segno
    potrebbe essere l'altro, ed e' una cosa che si testa invece di discuterla.

    L'inversione avviene **qui**, non a valle: cosi' condizione 02, il lato del limit, il
    controllo dell'ordine pendente e la geometria di stop e target seguono tutti il lato
    definitivo. La vecchia opzione --invert girava il lato dopo, e produceva un ordine che si
    eseguiva al primo trade: misurava quell'artefatto, non l'ipotesi.
    """
    if previous["delta"] < 0 and bar["delta"] >= MIN_FLIP_DELTA:
        return "short" if REVERSE_FLIP else "long"
    if previous["delta"] > 0 and bar["delta"] <= -MIN_FLIP_DELTA:
        return "long" if REVERSE_FLIP else "short"
    return None


def condition_02_value_area_shift(side: str, previous_va, current_va) -> bool:
    """La value area della nuova candela e' posizionata piu' in alto, o piu' in basso.

    Con REVERSE_SHIFT il valore deve essersi spostato **contro** il lato: e' la lettura coerente
    con l'assorbimento, dove si compra dopo che il valore e' sceso invece che dopo che e' salito.
    """
    if REVERSE_SHIFT:
        side = "short" if side == "long" else "long"
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

def signal(bars, i, vas, levels, flow):
    if i < 1:
        return None
    if SESSION_WINDOW and not (SESSION_WINDOW[0] <= bars[i]["lastTime"][11:16] <= SESSION_WINDOW[1]):
        return None

    previous, bar = bars[i - 1], bars[i]

    if PACE_RANGE is not None:
        seconds = (parse(bar["lastTime"]) - parse(bar["time"])).total_seconds()
        if not (PACE_RANGE[0] <= seconds <= PACE_RANGE[1]):
            return None

    if NULL_MODEL:
        side = "long" if bar["close"] > bar["open"] else "short"
    else:
        side = condition_01_auction_flip(previous, bar)
        if side is None:
            return None
        if not condition_02_value_area_shift(side, vas[i - 1], vas[i]):
            return None

    if ABSORPTION:
        # Il corpo della barra va in una direzione e il delta nell'altra: chi e' aggressivo
        # non ottiene il prezzo. E' la lettura dei pattern 03 e 04 della library.
        body = bar["close"] - bar["open"]
        if not ((body > 0 and bar["delta"] < 0) or (body < 0 and bar["delta"] > 0)):
            return None

    if TREND_BARS:
        # Stacked VA shifts: le barre precedenti devono aver spostato il valore nello stesso verso.
        if i < TREND_BARS + 1:
            return None
        for k in range(i - TREND_BARS, i):
            if not condition_02_value_area_shift(side, vas[k - 1], vas[k]):
                return None

    if MEAN_REVERSION:
        # Il contrario: il valore stava andando dall'altra parte e il segnale lo inverte.
        if i < 3:
            return None
        opposite = "short" if side == "long" else "long"
        if not all(condition_02_value_area_shift(opposite, vas[k - 1], vas[k]) for k in range(i - 2, i)):
            return None

    # Il livello non e' una condizione del dossier: e' contesto. Resta un filtro attivabile.
    if levels:
        near = next((f"{name} {price:,.0f}" for price, name in levels
                     if bar["low"] - LEVEL_TOLERANCE <= price <= bar["high"] + LEVEL_TOLERANCE), None)
        if near is None:
            return None
    else:
        near = "nessun filtro di livello"

    # Big trades e speed of tape: la conferma che al livello c'e' davvero qualcuno.
    measured = {}
    if flow is not None:
        end = parse(bar["lastTime"])
        begin = parse(bar["time"]) if FLOW_WINDOW is None else end - timedelta(seconds=FLOW_WINDOW)
        want = 1 if side == "long" else -1
        counted = flow.big(begin, end, BIG_TRADE_SIZE)
        rate = flow.rate(begin, end)
        base = flow.baseline[want]
        measured = {
            "bigPro": counted[want], "bigContro": counted[-want],
            "speedPro": round(rate[want] / base, 2) if base else None,
            "speedContro": round(rate[-want] / flow.baseline[-want], 2) if flow.baseline[-want] else None,
        }
        if BIG_TRADE_COUNT and counted[want] < BIG_TRADE_COUNT:
            return None
        if BIG_TRADE_DOMINANCE and counted[want] <= counted[-want]:
            return None
        if SPEED_MULTIPLE and (base is None or rate[want] < SPEED_MULTIPLE * base):
            return None
        if SPEED_DOMINANCE and rate[want] <= rate[-want]:
            return None

        if BIG_LEVELS:
            formed = begin - timedelta(seconds=BIG_LEVEL_AGE)
            cluster = flow.big_levels(formed, BIG_TRADE_SIZE, BIG_LEVELS)
            touched = next((price for price in cluster
                            if bar["low"] - LEVEL_TOLERANCE <= price <= bar["high"] + LEVEL_TOLERANCE), None)
            if touched is None:
                return None
            measured["bigLevel"] = touched

    val, vah = vas[i]
    if REQUIRE_PULLBACK:
        entry = vah if side == "long" else val
        if (side == "long" and entry >= bar["close"]) or (side == "short" and entry <= bar["close"]):
            return None

    if INVERT:
        side = "short" if side == "long" else "long"
    return {
        "bar": i, "side": side, "level": near, "time": bar["lastTime"],
        "delta": bar["delta"], "previousDelta": previous["delta"],
        "vah": vah, "val": val, "high": bar["high"], "low": bar["low"], **measured,
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
    target = entry + risk * TARGET_R if long else entry - risk * TARGET_R

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
    moved = False
    trigger = entry + risk * BREAKEVEN_R if long else entry - risk * BREAKEVEN_R
    for trade in tape[start:]:
        price = trade[4]
        if BREAKEVEN_R and not moved and ((long and price >= trigger) or (not long and price <= trigger)):
            # Lo stop va all'ingresso: da qui in poi la peggiore uscita possibile e' zero.
            stop, moved = entry, True
        if (long and price <= stop) or (not long and price >= stop):
            outcome = "pareggio" if moved else "stop"
            return done(outcome, 0.0 if moved else -risk, entry, stop, target, risk, quantity, trade[0])
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
    global INVERT, TREND_BARS, MEAN_REVERSION, PACE_RANGE, ABSORPTION, NULL_MODEL
    global REVERSE_FLIP, REVERSE_SHIFT
    global REQUIRE_PULLBACK, LEVEL_TOLERANCE, BREAKEVEN_R, TARGET_R
    global BIG_TRADE_SIZE, BIG_TRADE_COUNT, BIG_TRADE_DOMINANCE
    global SPEED_MULTIPLE, SPEED_DOMINANCE, FLOW_WINDOW, BIG_LEVELS, BIG_LEVEL_AGE

    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("candles", help="JSON di /candles con --levels")
    parser.add_argument("--tape", required=True, help="JSON di /cumulative --min-volume 0 --compact")
    parser.add_argument("--level", action="append", default=[], metavar="PREZZO:NOME")
    parser.add_argument("--levels-from", action="append", default=[], metavar="CANDELE.JSON")
    parser.add_argument("--level-tolerance", type=float, default=LEVEL_TOLERANCE,
                        help="punti entro cui la barra si considera sul livello")

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

    tests = parser.add_argument_group("ipotesi sotto test - non vengono dal dossier")
    tests.add_argument("--invert", action="store_true", help="controllo non valido, vedi il commento nel file")
    parser.add_argument("--no-pullback-check", action="store_true",
                        help="accetta anche i setup in cui il limit si eseguirebbe subito")
    tests.add_argument("--reverse-flip", action="store_true",
                       help="condizione 01 letta come assorbimento: delta che gira in giu' -> long")
    tests.add_argument("--reverse-shift", action="store_true",
                       help="condizione 02 letta come assorbimento: il valore si sposta contro il lato")
    tests.add_argument("--null", action="store_true", help="stessa esecuzione senza le tre condizioni")
    tests.add_argument("--trend", type=int, default=0, metavar="N", help="richiede N VA shift consecutivi nello stesso verso")
    tests.add_argument("--mean-reversion", action="store_true", help="richiede che il valore stesse andando dall'altra parte")
    tests.add_argument("--pace", metavar="MIN,MAX", help="durata della barra 40R in secondi")
    tests.add_argument("--absorption", action="store_true", help="richiede delta opposto al corpo della barra")
    tests.add_argument("--breakeven", type=float, default=0.0, metavar="R",
                       help="porta lo stop all'ingresso dopo N R favorevoli; 0 disattiva")
    tests.add_argument("--target", type=float, default=1.0, metavar="R",
                       help="obiettivo in multipli di R; il dossier dice 1")

    flow_args = parser.add_argument_group("big trades e speed of tape - dal transcript, non dal dossier")
    flow_args.add_argument("--big-trade", type=int, default=BIG_TRADE_SIZE, metavar="SIZE",
                           help="volume minimo di una stampa perche' sia un big trade")
    flow_args.add_argument("--big-trades", type=int, default=0, metavar="N",
                           help="big trades richiesti dal lato del segnale; 0 disattiva")
    flow_args.add_argument("--big-dominance", action="store_true",
                           help="ne servono piu' dal lato del segnale che dal lato opposto")
    flow_args.add_argument("--speed", type=float, default=0.0, metavar="MULT",
                           help="velocita' minima del lato del segnale, in multipli della mediana della sessione")
    flow_args.add_argument("--speed-dominance", action="store_true",
                           help="il lato del segnale deve essere piu' veloce di quello opposto")
    flow_args.add_argument("--flow-window", type=float, default=None, metavar="SEC",
                           help="finestra di lettura; omessa usa la barra 40R stessa")
    flow_args.add_argument("--big-levels", type=int, default=0, metavar="N",
                           help="la barra deve toccare uno degli N prezzi con piu' big trades accumulati")
    flow_args.add_argument("--big-level-age", type=float, default=BIG_LEVEL_AGE, metavar="SEC",
                           help="eta' minima di un livello da big trades")

    parser.add_argument("--quiet", action="store_true", help="solo la riga di riepilogo")
    parser.add_argument("--json", action="store_true", help="riepilogo in JSON, per aggregare piu' sessioni")
    parser.add_argument("--trades-json", action="store_true",
                        help="una riga JSON per operazione con le misure di flusso e l'esito")
    args = parser.parse_args()
    if args.json or args.trades_json:
        args.quiet = True

    VALUE_AREA_PERCENT = args.va_percent
    RISK_DOLLARS, POINT_VALUE, DAILY_CAP_R = args.risk, args.point_value, args.daily_cap
    MIN_FLIP_DELTA, VA_SHIFT_RULE = args.flip, args.va_shift
    PENDING_CONTROL_DELTA, ACTIVE_CONTROL_DELTA = args.pending_control, args.manage
    PULLBACK_BARS, STOP_MODE = args.pullback, args.stop
    SESSION_WINDOW = tuple(args.window.split("-")) if args.window else None
    INVERT, TREND_BARS, MEAN_REVERSION = args.invert, args.trend, args.mean_reversion
    NULL_MODEL = args.null
    REVERSE_FLIP, REVERSE_SHIFT = args.reverse_flip, args.reverse_shift
    REQUIRE_PULLBACK = not args.no_pullback_check
    LEVEL_TOLERANCE = args.level_tolerance
    BREAKEVEN_R, TARGET_R = args.breakeven, args.target
    BIG_TRADE_SIZE, BIG_TRADE_COUNT = args.big_trade, args.big_trades
    BIG_TRADE_DOMINANCE = args.big_dominance
    SPEED_MULTIPLE, SPEED_DOMINANCE = args.speed, args.speed_dominance
    FLOW_WINDOW = args.flow_window
    BIG_LEVELS, BIG_LEVEL_AGE = args.big_levels, args.big_level_age
    ABSORPTION = args.absorption
    PACE_RANGE = tuple(float(x) for x in args.pace.split(",")) if args.pace else None

    bars = json.load(open(args.candles))["candles"]
    raw = json.load(open(args.tape))
    if not raw.get("compact"):
        raise SystemExit("il tape va scaricato con --compact")
    tape = [(parse(t[0]), t[1], t[2], t[3], t[4]) for t in raw["trades"]]
    times = [t[0] for t in tape]

    vas = [bar_value_area(bar, VALUE_AREA_PERCENT) for bar in bars]
    flow = Flow(tape, times, bars)

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
        sig = signal(bars, i, vas, levels, flow)
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
            if args.trades_json:
                print(json.dumps({"time": sig["time"], "side": sig["side"],
                                  "bigPro": sig.get("bigPro"), "bigContro": sig.get("bigContro"),
                                  "speedPro": sig.get("speedPro"), "speedContro": sig.get("speedContro"),
                                  "outcome": out["outcome"], "r": round(out["r"], 2)}))
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

    median_risk = sorted(risks)[len(risks) // 2] if risks else 0
    summary = {"trades": taken, "wins": wins, "r": round(r_total, 2), "dollars": round(dollars),
               "medianRisk": median_risk, "cancelled": cancelled, "unfilled": unfilled, "managed": managed}
    if args.trades_json:
        return
    if args.json:
        print(json.dumps(summary))
        return
    rate = f"{100 * wins / taken:.0f}%" if taken else "-"
    print(f"operazioni {taken}   vinte {wins} ({rate})   {r_total:+.2f} R   {dollars:+,.0f} $   "
          f"rischio mediano {median_risk:,.2f} pt   cancellati {cancelled}   non eseguiti {unfilled}   "
          f"uscite per controllo {managed}")


if __name__ == "__main__":
    main()
