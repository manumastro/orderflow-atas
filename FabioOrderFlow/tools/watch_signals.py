#!/usr/bin/env python3
"""Sorveglia il tape sui livelli del giorno e, quando scatta un segnale, lo riporta sul chart.

Cosa fa, e cosa non fa. Legge i livelli **gia' derivati dall'analisi** da un file JSON, chiede al
bridge le barre M1 e valuta solo quelle chiuse. Non deriva livelli, non decide soglie da solo e non
tocca ordini: e' la stessa divisione dei ruoli dei livelli sul chart, dove l'indicatore disegna, il
client trasporta e la derivazione resta nell'analisi.

Con `--push-alerts` ogni segnale forte torna sul chart come livello effimero con etichetta
prefissata da `ALERT_PREFIX`. I livelli strutturali del file non vengono mai toccati: a ogni push
la lista viene ricostruita come strutturali + ultimi N alert.

Le soglie hanno un default, ma **vanno dichiarate accanto al risultato che producono**. Quelle di
fabbrica vengono dai percentili delle barre M1 di NQ del 15 settembre 2026 e non sono una costante
del mercato: con `--calibra` si ricalcolano sulla seduta in corso.

    python3 watch_signals.py --levels docs/research/giornate/livelli-2026-09-15.json \
        --from 2026-09-15T07:30 --push-alerts

Procedura completa in docs/research/metodo/sorveglianza-del-tape.md.
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

ALERT_PREFIX = "! "          # come si riconosce un alert fra i livelli del chart
NEAR = 8.0                   # quanto vicino al livello deve stare il minimo o il massimo
BODY_FRAC = 0.5              # la chiusura deve stare in questa meta' del range della barra

# Colore per tipo di segnale. Gli alert sono sempre `dot` e spessore 1, cosi' restano
# distinguibili a colpo d'occhio dai livelli strutturali, che sono solid o dash.
COLORI = {
    "ROTTURA": "#FF4444",
    "RECLAIM": "#44DD66",
    "ASSORBIMENTO": "#00D0FF",
    "RIFIUTO": "#FFA033",
    "DELTA GROSSO": "#E066FF",
}


# ----------------------------------------------------------------- dati

def chiedi_candele(args) -> list[dict]:
    cmd = [sys.executable, str(BRIDGE), "candles",
           "--from", args.begin, "--to", args.end, "--out", str(args.cache)]
    if args.chart:
        cmd += ["--chart", args.chart]
    subprocess.run(cmd, check=True, timeout=90,
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    return json.load(open(args.cache))["candles"]


def minuti(b: dict) -> int:
    """Minuti assoluti dal timestamp.

    Serve perche' `bar` **non e' un identificatore**: e' un indice di posizione, e riparte quando
    ATAS ricarica l'indicatore. Usarlo come chiave fa rivalutare tutta la storia dopo un reload.
    """
    t = b["time"]
    return int(t[8:10]) * 1440 + int(t[11:13]) * 60 + int(t[14:16])


def ora(b: dict, fuso: int) -> str:
    h = (int(b["time"][11:13]) + fuso) % 24
    return f"{h:02d}:{b['time'][14:16]}"


# ----------------------------------------------------------------- segnali

def vicino(prezzo: float, livelli, sotto: float, sopra: float):
    """Il livello piu' vicino al prezzo, purche' dentro la finestra."""
    cand = [(abs(prezzo - p), p, n) for p, n in livelli if p - sotto <= prezzo <= p + sopra]
    return min(cand)[1:] if cand else None


def valuta(b, prev, livelli, soglie, ultimo_avviso, fuso):
    """Un solo segnale per barra, in ordine di priorita'. Restituisce (tipo, prezzo, riga)."""
    hi, lo, cl, vol, d = b["high"], b["low"], b["close"], b["volume"], b["delta"]
    rng = max(hi - lo, 0.25)
    pos = (cl - lo) / rng                       # 1 = chiude sul massimo, 0 sul minimo
    pc = prev["close"] if prev else cl
    tag = f"{ora(b, fuso)} {cl:.2f} vol={vol} delta={d:+}"

    # 1. attraversamento con aggressione, in entrambi i versi. I ruoli non sono fissi: un
    #    supporto rotto diventa resistenza nel giro di una barra.
    if vol >= soglie["vol_alto"]:
        for p, nome in livelli:
            if cl < p <= pc and d <= -soglie["delta_forte"]:
                return "ROTTURA", p, f"ROTTURA {nome} {p:.0f} | {tag} | chiude sotto con aggressione venditrice"
            if cl > p >= pc and d >= soglie["delta_forte"]:
                return "RECLAIM", p, f"RECLAIM {nome} {p:.0f} | {tag} | chiude sopra con aggressione compratrice"

        # 2. assorbimento: minimo sul livello, delta non negativo, chiusura risalita
        hit = vicino(lo, livelli, sotto=2 * NEAR, sopra=NEAR)
        if hit and d >= 0 and pos >= BODY_FRAC:
            p, nome = hit
            return "ASSORBIMENTO", p, f"ASSORBIMENTO {nome} {p:.0f} | {tag} | min {lo:.2f}, chiude al {pos * 100:.0f}%"

        # 3. rifiuto: lo specchio, sul massimo
        hit = vicino(hi, livelli, sotto=NEAR, sopra=2 * NEAR)
        if hit and d <= 0 and pos <= 1 - BODY_FRAC:
            p, nome = hit
            return "RIFIUTO", p, f"RIFIUTO {nome} {p:.0f} | {tag} | max {hi:.2f}, chiude al {pos * 100:.0f}%"

    # 4. stampa isolata molto grossa, ovunque sia
    if abs(d) >= soglie["delta_enorme"]:
        verso = "compratrice" if d > 0 else "venditrice"
        return "DELTA GROSSO", cl, f"DELTA GROSSO {d:+} | {tag} | aggressione {verso}"

    # 5. livello attraversato senza i requisiti sopra: informativo, non finisce sul chart.
    #    Senza il filtro di volume e il silenzio temporaneo, un livello conteso produce una
    #    riga per ogni oscillazione.
    if vol >= soglie["vol_medio"]:
        for p, nome in livelli:
            if (cl < p <= pc) or (cl > p >= pc):
                if minuti(b) - ultimo_avviso.get(p, -10 ** 9) < soglie["vela_muta"]:
                    return None
                ultimo_avviso[p] = minuti(b)
                verso = "sotto" if cl < p else "sopra"
                return "attraversato", p, f"attraversato {nome} {p:.0f} | {tag} | passa {verso} senza conferma"
    return None


# ----------------------------------------------------------------- chart

def push_alert(args, tipo: str, prezzo: float, etichetta: str, strutturali, alert_attivi):
    """Ricostruisce la lista del chart come strutturali + ultimi N alert.

    `POST /levels` sostituisce l'intera lista, quindi non si puo' aggiungere una riga sola. Si
    riparte sempre dai livelli strutturali letti dal file, che restano la fonte: cosi' un alert
    non puo' cancellare un livello dell'analisi nemmeno se qualcosa va storto.
    """
    alert_attivi.append({
        "price": prezzo,
        "label": f"{ALERT_PREFIX}{etichetta}",
        "color": COLORI.get(tipo, "#FFFFFF"),
        "style": "dot",
        "width": 1,
        "note": f"alert automatico: {tipo}",
    })
    del alert_attivi[:-args.max_alerts]
    corpo = strutturali + alert_attivi
    cmd = [sys.executable, str(BRIDGE), "levels", "--file", "-"]
    if args.chart:
        cmd += ["--chart", args.chart]
    try:
        subprocess.run(cmd, input=json.dumps(corpo), text=True, check=True, timeout=30,
                       stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    except Exception as e:                        # il chart e' un di piu': non deve fermare la sorveglianza
        print(f"[alert non spinto sul chart: {type(e).__name__}]", flush=True)


def calibra(candele, soglie):
    """Ricalcola le soglie sui percentili della seduta in corso."""
    if len(candele) < 60:
        return soglie
    v = sorted(b["volume"] for b in candele)
    d = sorted(abs(b["delta"]) for b in candele)
    q = lambda a, p: a[int(len(a) * p)]
    return {**soglie,
            "vol_alto": q(v, 0.95), "vol_medio": q(v, 0.75),
            "delta_forte": q(d, 0.95), "delta_enorme": max(q(d, 0.95) * 2, 100)}


# ----------------------------------------------------------------- main

def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--levels", required=True, help="JSON dei livelli derivati dall'analisi")
    ap.add_argument("--from", dest="begin", required=True, help="inizio della finestra chiesta al bridge")
    ap.add_argument("--to", dest="end", default="2100-01-01T00:00")
    ap.add_argument("--chart", help="id o strumento del chart, se ne e' registrato piu' di uno")
    ap.add_argument("--push-alerts", action="store_true", help="riporta i segnali sul chart come livelli effimeri")
    ap.add_argument("--max-alerts", type=int, default=5, help="quanti alert tenere sul chart (default 5)")
    ap.add_argument("--intervallo", type=int, default=20, help="secondi fra due interrogazioni (default 20)")
    ap.add_argument("--calibra", action="store_true", help="ricalcola le soglie sui percentili della seduta")
    ap.add_argument("--vol-alto", type=int, default=221, help="p95 del volume per barra (default 221)")
    ap.add_argument("--vol-medio", type=int, default=138, help="p75 del volume per barra (default 138)")
    ap.add_argument("--delta-forte", type=int, default=64, help="p95 del |delta| per barra (default 64)")
    ap.add_argument("--delta-enorme", type=int, default=100, help="stampa isolata (default 100)")
    ap.add_argument("--vela-muta", type=int, default=15, help="minuti di silenzio sullo stesso livello (default 15)")
    ap.add_argument("--fuso", type=int, default=2, help="ore da aggiungere all'UTC per stampare l'ora (default 2)")
    ap.add_argument("--cache", default="/tmp/fof-watch.json")
    args = ap.parse_args()

    grezzi = json.load(open(args.levels))
    livelli = [(float(l["price"]), l.get("label") or f"{l['price']:.0f}") for l in grezzi]
    livelli.sort(key=lambda x: -x[0])
    strutturali = [l for l in grezzi if not (l.get("label") or "").startswith(ALERT_PREFIX)]

    soglie = {"vol_alto": args.vol_alto, "vol_medio": args.vol_medio,
              "delta_forte": args.delta_forte, "delta_enorme": args.delta_enorme,
              "vela_muta": args.vela_muta}

    visti, ultimo_avviso, alert_attivi = set(), {}, []
    frontiera, prima = None, True

    while True:
        try:
            c = chiedi_candele(args)
        except Exception as e:
            print(f"[bridge non raggiungibile: {type(e).__name__}]", flush=True)
            time.sleep(30)
            continue

        chiuse = c[:-1]                      # l'ultima barra e' ancora in formazione
        if not chiuse:
            time.sleep(args.intervallo)
            continue

        if prima:
            if args.calibra:
                soglie = calibra(chiuse, soglie)
            visti = {b["time"] for b in chiuse}
            frontiera = minuti(chiuse[-1])
            u = chiuse[-1]
            print(f"[attivo] {len(livelli)} livelli, ultima barra {ora(u, args.fuso)} a {u['close']:.2f} - "
                  f"vol>={soglie['vol_alto']} delta>={soglie['delta_forte']}"
                  f"{' - alert sul chart' if args.push_alerts else ''}", flush=True)
            prima = False
        else:
            for i, b in enumerate(chiuse):
                if b["time"] in visti or minuti(b) <= frontiera:
                    continue
                visti.add(b["time"])
                frontiera = minuti(b)
                esito = valuta(b, chiuse[i - 1] if i else None, livelli, soglie, ultimo_avviso, args.fuso)
                if not esito:
                    continue
                tipo, prezzo, riga = esito
                print(riga, flush=True)
                if args.push_alerts and tipo in COLORI:
                    push_alert(args, tipo, prezzo, f"{tipo} {ora(b, args.fuso)}", strutturali, alert_attivi)
        time.sleep(args.intervallo)


if __name__ == "__main__":
    main()
