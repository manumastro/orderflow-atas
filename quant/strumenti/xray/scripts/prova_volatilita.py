#!/usr/bin/env python3
"""La prova delle fonti nuove del server (2 novembre 2026): il calcolo C# contro un calcolo Python
scritto a parte, sugli stessi dati, e la forma delle risposte delle rotte nuove.

1. Cboe: si scarica lo stesso file pubblico che ha letto il server (stesso `timestamp`) e si ricalcolano
   qui la volatilita' a 30 giorni (formula del Cboe), la GEX totale e i muri; devono tornare con
   l'ultima fotografia del server (quant/dati/volatilita/<giorno>/cboe-<SIM>.json).
2. Le rotte /api/fonti, /api/vol/serie, /api/vol/termine, /api/vol/sorriso, /api/nq/gex,
   /api/es/gex-confronto: i campi del contratto, le fonti dichiarate, numeri plausibili.

    python quant/strumenti/xray/scripts/prova_volatilita.py [--url http://127.0.0.1:8790]

Esce con 1 se una prova fallisce. Se il file del Cboe e' cambiato dopo l'ultima raccolta del server, la
prova 1 lo dice e si salta (il server lo rilegge ogni 15 minuti in cassa).
"""
from __future__ import annotations

import argparse
import datetime as dt
import glob
import json
import math
import os
import re
import sys
import urllib.request
from zoneinfo import ZoneInfo

NY = ZoneInfo("America/New_York")
R = 0.0388
RADICE = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "..", ".."))
DATI = os.path.join(RADICE, "quant", "dati", "volatilita")
FONTI = {"xray", "catena", "cboe", "yahoo"}

fallite: list[str] = []
passate = 0


def ok(cond: bool, cosa: str) -> None:
    global passate
    if cond:
        passate += 1
        print(f"  ok    {cosa}")
    else:
        fallite.append(cosa)
        print(f"  NO    {cosa}")


def scarica(url: str) -> dict:
    req = urllib.request.Request(url, headers={"User-Agent": "Mozilla/5.0"})
    with urllib.request.urlopen(req, timeout=60) as r:
        return json.load(r)


# ------------------------------------------------------------ il calcolo indipendente

def catena(dati: dict) -> dict[str, dict]:
    """Le scadenze del file Cboe: {giorno: {scade (datetime UTC), opz: [(k, call, bid, ask, theo, oi, iv)]}}."""
    out: dict[str, dict] = {}
    for o in dati["options"]:
        m = re.match(r"^([A-Z]+)(\d{6})([CP])(\d{8})$", o["option"])
        if not m:
            continue
        g = dt.datetime.strptime(m.group(2), "%y%m%d")
        ora = dt.time(9, 30) if m.group(1) == "NDX" else dt.time(16, 0)
        scade = dt.datetime.combine(g.date(), ora, NY).astimezone(dt.timezone.utc)
        chiave = g.strftime("%Y-%m-%d") + ("am" if m.group(1) == "NDX" else "")
        x = out.setdefault(chiave, {"scade": scade, "opz": []})
        x["opz"].append((int(m.group(4)) / 1000, m.group(3) == "C", o.get("bid"), o.get("ask"), o.get("theo"),
                         o.get("open_interest") or 0, o.get("iv")))
    return out


def prezzo(q, modo: str):
    _, _, b, a, theo, _, _ = q
    if modo == "medio":
        return (a + b) / 2 if b is not None and a is not None and b > 0 and a >= b else None
    return theo if theo and theo > 0 else None


def varianza(opz, t: float, modo: str):
    per_k: dict[float, dict] = {}
    for q in opz:
        per_k.setdefault(q[0], {})["C" if q[1] else "P"] = prezzo(q, modo)
    ks = sorted(k for k, v in per_k.items() if (v.get("C") or 0) > 0 and (v.get("P") or 0) > 0)
    if len(ks) < 3:
        return None
    kf = min(ks, key=lambda k: abs(per_k[k]["C"] - per_k[k]["P"]))
    f = kf + math.exp(R * t) * (per_k[kf]["C"] - per_k[kf]["P"])
    sotto = [k for k in ks if k <= f]
    if not sotto:
        return None
    k0 = sotto[-1]
    scelte = []

    def lato(strike, tipo):
        nulli = 0
        for k in strike:
            q = per_k[k].get(tipo)
            if q is None:
                continue
            if q <= 0.05:
                nulli += 1
                if nulli >= 2:
                    break
                continue
            nulli = 0
            scelte.append((k, q))

    lato(sorted((k for k in per_k if k < k0), reverse=True), "P")
    lato(sorted(k for k in per_k if k > k0), "C")
    scelte.append((k0, (per_k[k0]["C"] + per_k[k0]["P"]) / 2))
    scelte.sort()
    if len(scelte) < 5:
        return None
    s = 0.0
    for i, (k, q) in enumerate(scelte):
        dk = (scelte[1][0] - scelte[0][0] if i == 0 else scelte[i][0] - scelte[i - 1][0] if i == len(scelte) - 1
              else (scelte[i + 1][0] - scelte[i - 1][0]) / 2)
        s += dk / k / k * math.exp(R * t) * q
    return {"var": 2 / t * s - (f / k0 - 1) ** 2 / t, "t": t, "f": f, "n": len(scelte)}


def vol30(cat: dict, adesso: dt.datetime):
    bersaglio = adesso + dt.timedelta(days=30)
    valide = sorted((v["scade"], g) for g, v in cat.items() if (v["scade"] - adesso).days >= 7
                    or (v["scade"] - adesso).total_seconds() >= 7 * 86400)
    prima = [x for x in valide if x[0] <= bersaglio]
    dopo = [x for x in valide if x[0] > bersaglio]
    if not prima or not dopo:
        return None
    (s1, g1), (s2, g2) = prima[-1], dopo[0]
    for modo in ("medio", "teorico"):
        t1 = (s1 - adesso).total_seconds() / 86400 / 365
        t2 = (s2 - adesso).total_seconds() / 86400 / 365
        v1, v2 = varianza(cat[g1]["opz"], t1, modo), varianza(cat[g2]["opz"], t2, modo)
        if v1 and v2 and v1["n"] >= 25 and v2["n"] >= 25:
            n = 30 / 365
            w = (t1 * v1["var"] * (t2 - n) / (t2 - t1) + t2 * v2["var"] * (n - t1) / (t2 - t1)) / n
            return math.sqrt(w) * 100, modo
    return None


def ncdf_pdf(x: float) -> float:
    return math.exp(-x * x / 2) / math.sqrt(2 * math.pi)


def gex(cat: dict, spot: float, adesso: dt.datetime):
    per_k: dict[float, list[float]] = {}
    for v in cat.values():
        t = (v["scade"] - adesso).total_seconds() / 86400 / 365
        if t <= 0 or t * 365 > 70:
            continue
        for k, call, _, _, _, oi, iv in v["opz"]:
            if oi <= 0 or not iv or iv <= 0:
                continue
            sv = iv * math.sqrt(t)
            d1 = (math.log(spot / k) + sv * sv / 2) / sv
            g = ncdf_pdf(d1) / (spot * sv) * 100 * spot * spot * 0.01 / 1e6 * oi
            x = per_k.setdefault(k, [0.0, 0.0])
            x[0 if call else 1] += g
    tot = sum(c - p for c, p in per_k.values())
    cw = max(per_k, key=lambda k: per_k[k][0])
    pw = max(per_k, key=lambda k: per_k[k][1])
    return tot, cw, pw


# ------------------------------------------------------------ le prove

def prova_cboe(simbolo: str) -> None:
    file = f"_{simbolo}" if simbolo == "NDX" else simbolo
    foto_path = sorted(glob.glob(os.path.join(DATI, "*", f"cboe-{simbolo}.json")))
    if not foto_path:
        ok(False, f"{simbolo}: nessuna fotografia del server in {DATI}")
        return
    foto = json.load(open(foto_path[-1], encoding="utf-8"))
    d = scarica(f"https://cdn.cboe.com/api/global/delayed_quotes/options/{file}.json")
    quando = dt.datetime.strptime(d["timestamp"], "%Y-%m-%d %H:%M:%S").replace(tzinfo=dt.timezone.utc)
    if int(quando.timestamp() * 1000) != foto["quando"]:
        print(f"  --    {simbolo}: il file del Cboe ({quando:%H:%M}Z) e' piu' nuovo della fotografia del server: salto")
        return
    dati = d["data"]
    # gli istanti come nel server: 15 minuti prima del file, o l'ultimo scambio fuori dalla cassa
    ultimo = dt.datetime.fromisoformat(dati["last_trade_time"]).replace(tzinfo=NY).astimezone(dt.timezone.utc)
    t_dati = quando - dt.timedelta(minutes=15)
    ny = t_dati.astimezone(NY)
    if not (ny.weekday() < 5 and dt.time(9, 30) <= ny.time() < dt.time(16, 0)):
        t_dati = ultimo
    cat = catena(dati)
    v = vol30(cat, t_dati)
    s = foto.get("vol30") or {}
    if v is None or s.get("valore") is None:
        ok(v is None and s.get("valore") is None, f"{simbolo}: vol a 30 giorni assente in tutti e due ({v}, {s.get('valore')})")
    else:
        ok(abs(v[0] - s["valore"]) < 0.01, f"{simbolo}: vol a 30 giorni Python {v[0]:.3f} ({v[1]}) = server {s['valore']:.3f} ({s.get('prezzi')})")
    tot, cw, pw = gex(cat, dati["current_price"], t_dati)
    g = foto["gex"]
    ok(abs(tot - g["totale"]) <= max(0.5, abs(tot) * 1e-4), f"{simbolo}: GEX totale Python {tot:.2f} = server {g['totale']:.2f}")
    ok(cw == g["callWall"] and pw == g["putWall"], f"{simbolo}: muri Python {cw}/{pw} = server {g['callWall']}/{g['putWall']}")


def prova_rotte(url: str) -> None:
    api = lambda q: scarica(f"{url}/api/{q}")  # noqa: E731
    f = api("fonti")
    ok(all(x["fonte"] in FONTI and x["stato"] in {"ok", "vecchio", "errore", "assente"} for x in f["fonti"]),
       f"fonti: {len(f['fonti'])} fonti, tutte con fonte e stato validi")
    ids = {x["id"] for x in f["fonti"]}
    ok({"xray", "catena-es", "catena-nq", "gex-nq", "gex-es", "cboe-qqq", "cboe-ndx", "yahoo"} <= ids, "fonti: ci sono tutte")

    s = api("vol/serie")
    ok(all(x["fonte"] in FONTI and x["indice"] in {"SPX", "NDX"} for x in s["serie"]), "vol/serie: fonte e indice dichiarati")
    for x in s["serie"]:
        bad = [p for p in x["punti"] if not (3 < p[1] < 150)]
        ok(not bad, f"vol/serie {x['id']}: {len(x['punti'])} punti, tutti fra 3 e 150" + (f" (fuori: {bad[:2]})" if bad else ""))
    t = api("vol/termine")
    for c in t["curve"]:
        bad = [p for p in c["punti"] if not (p["giorni"] > 0 and 3 < p["vol"] < 150)]
        ok(c["fonte"] in FONTI and not bad, f"vol/termine {c['id']}: {len(c['punti'])} scadenze plausibili")
    for indice in ("SPX", "NDX"):
        so = api(f"vol/sorriso?indice={indice}")
        for c in so["curve"]:
            ms = [p["moneyness"] for p in c["punti"]]
            ok(c["fonte"] in FONTI and ms == sorted(ms) and all(abs(m) <= 12.5 for m in ms),
               f"vol/sorriso {indice} {c['id']}: {len(ms)} punti in ordine, entro +-12%")
    n = api("nq/gex")
    for x in n["fonti"]:
        prezzi = [p["prezzo"] for p in x["strike"] if p["prezzo"] is not None]
        vicino = n["prezzoNQ"] is None or not prezzi or min(prezzi) < n["prezzoNQ"] < max(prezzi)
        ok(x["fonte"] in FONTI and vicino, f"nq/gex {x['id']}: {len(prezzi)} strike in prezzi di NQ attorno a {n['prezzoNQ']}")
        if x["id"] != "gex-nq":
            ok(x["conversione"] is not None, f"nq/gex {x['id']}: conversione dichiarata ({x['conversione'] and x['conversione']['valore']})")
    e = api("es/gex-confronto")
    ok(len(e["curve"]) == 2 and all(max((abs(p["normalizzata"]) for p in c["punti"]), default=1) <= 1.0001 for c in e["curve"]),
       f"es/gex-confronto: due curve normalizzate, accordo {e['accordo']['segnoUguale']} su {e['accordo']['strike']} strike")


def prova_gex_viva(url: str) -> None:
    """La GEX viva del server (catena letta a rotazione, ricalcolata ogni minuto) contro l'ultimo gex.py: stesso
    modello, letture diverse (strike per lato, ora delle quotazioni), quindi entro una tolleranza."""
    cache = os.path.expanduser("~/.fabio-gex-NQZ6.json")
    n = scarica(f"{url}/api/nq/gex")
    viva = next((f for f in n["fonti"] if f["id"] == "gex-nq" and "viva" in f["nome"]), None)
    if viva is None or not os.path.exists(cache):
        print("  --    GEX viva o cache di gex.py assente: salto")
        return
    g = json.load(open(cache, encoding="utf-8"))
    eta = (dt.datetime.now(dt.timezone.utc) - dt.datetime.fromisoformat(g["quando"])).total_seconds() / 60
    if eta > 45:
        print(f"  --    la cache di gex.py ha {eta:.0f} minuti: salto (lanciala con gex.py --strumento NQZ6)")
        return
    t = g["tutte"]
    ok(abs(viva["zeroGamma"] - t["zero"]) <= 0.004 * t["zero"],
       f"GEX viva NQ: zero gamma {viva['zeroGamma']:.0f} contro gex.py {t['zero']:.0f} (entro lo 0,4%)")
    # i muri: due strike quasi alla pari si scambiano il ruolo (il 2/11/2026 su NQ 29.000 e 31.000 valevano 52 e 51
    # M$ per 1%): una differenza si scrive, non fa fallire la prova
    if viva["callWall"] == t["call_wall"] and viva["putWall"] == t["put_wall"]:
        ok(True, f"GEX viva NQ: muri {viva['callWall']}/{viva['putWall']} uguali a gex.py")
    else:
        print(f"  --    GEX viva NQ: muri {viva['callWall']}/{viva['putWall']} contro gex.py {t['call_wall']}/{t['put_wall']} "
              f"(gex.py: call wall a {t['cw']:.0f} M$ per 1%; strike quasi alla pari si scambiano)")
    ok(abs(viva["totale"] - t["tot_a"]) <= max(25, 0.15 * abs(t["tot_a"])),
       f"GEX viva NQ: totale {viva['totale']:.0f} contro gex.py {t['tot_a']:.0f} (entro il 15%)")


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--url", default="http://127.0.0.1:8790")
    a = ap.parse_args()
    print("== Cboe: il server contro il calcolo Python")
    for s in ("NDX", "QQQ"):
        prova_cboe(s)
    print("\n== la GEX viva contro gex.py")
    prova_gex_viva(a.url)
    print("\n== le rotte nuove")
    prova_rotte(a.url)
    print(f"\n{passate} prove passate, {len(fallite)} fallite")
    for f in fallite:
        print(f"  FALLITA: {f}")
    return 1 if fallite else 0


if __name__ == "__main__":
    sys.exit(main())
