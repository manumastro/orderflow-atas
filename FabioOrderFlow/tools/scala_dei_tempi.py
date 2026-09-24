#!/usr/bin/env python3
"""La scala dei tempi: dal piu' lontano al piu' vicino, come la guarderebbe una persona.

Prima le settimane, poi le sedute, poi dove sta il prezzo rispetto a tutto questo, poi gli eventi.
Solo dopo il giro scende alla notte, all'Europa e al tape. Il 23 settembre il bersaglio vero del
ribasso stava due giorni indietro, e il 24 la notte era uscita sotto un valore che coincideva per
due sedute: sono cose che si vedono solo guardando prima il quadro largo.

Le sedute chiuse non cambiano piu': si misurano una volta e si tengono in una cache per strumento
(`~/.fabio-scala-dei-tempi-<STRUMENTO>.json`). L'hook ha 25 secondi, e la prima volta la cache si
riempie a rate, col tempo che c'e': il giro lo dice, invece di stampare una storia a meta' come se
fosse intera.

    python FabioOrderFlow/tools/scala_dei_tempi.py --chart NQZ6            stampa la sezione
    python FabioOrderFlow/tools/scala_dei_tempi.py --chart NQZ6 --riempi   riempie la cache, senza fretta
"""

from __future__ import annotations

import argparse
import datetime as dt
import json
import math
import os
import time
import urllib.request
from pathlib import Path

REPO = Path(__file__).resolve().parents[2]
EVENTI = REPO / "docs" / "research" / "calendario" / "eventi.json"
GIORNI_INDIETRO = 25
RANGE_CASSA_NQ = 290.0
PORTA: int | None = None


def porta() -> int:
    try:
        return int(json.load(open(Path.home() / ".fabio-data-bridge.json"))["port"])
    except Exception:
        pass
    for p in range(8787, 8797):
        try:
            urllib.request.urlopen(f"http://127.0.0.1:{p}/charts", timeout=1).read()
            return p
        except Exception:
            continue
    return 8787


def chiedi(percorso: str, timeout: float = 20) -> dict | None:
    try:
        return json.load(urllib.request.urlopen(f"http://127.0.0.1:{PORTA}{percorso}", timeout=timeout))
    except Exception:
        return None


def giorno_di_mercato(iso: str) -> str:
    """Dalle 22:00Z la notte appartiene alla seduta dopo, come la conta il CME."""
    t = dt.datetime.fromisoformat(iso.replace("Z", "+00:00"))
    return (t.date() + dt.timedelta(days=1) if t.hour >= 22 else t.date()).isoformat()


def valore(hist: dict[float, float]) -> tuple[float, float, float] | None:
    """POC e area di valore al 70%, crescendo dal POC verso il lato piu' pesante."""
    if not hist:
        return None
    ps = sorted(hist)
    tot = sum(hist.values())
    poc = max(hist, key=hist.get)
    i = j = ps.index(poc)
    s = hist[poc]
    while s / tot < 0.70 and (i > 0 or j < len(ps) - 1):
        gi = hist[ps[i - 1]] if i > 0 else -1
        gj = hist[ps[j + 1]] if j < len(ps) - 1 else -1
        if gj > gi:
            j += 1
            s += gj
        else:
            i -= 1
            s += gi
    return poc, ps[i], ps[j]


def misura(chart: str, giorno: str, cassa: tuple[str, str], grana: float) -> dict | None:
    """Una seduta intera (dalle 22:00Z del giorno prima) e la sua cassa, con gli istogrammi."""
    g = dt.date.fromisoformat(giorno)
    da = f"{(g - dt.timedelta(days=1)).isoformat()}T22:00:00Z"
    a = f"{g.isoformat()}T22:00:00Z"
    r = chiedi(f"/candles?chart={chart}&from={da}&to={a}&levels=true", timeout=60)
    cs = (r or {}).get("candles") or []
    if not cs:
        return None
    out = {}
    for nome, filtro in (("giorno", lambda c: True),
                         ("cassa", lambda c: cassa[0] <= c["time"][11:16] < cassa[1]
                          and c["time"][:10] == giorno)):
        sel = [c for c in cs if filtro(c)]
        if not sel:
            continue
        hist: dict[float, float] = {}
        for c in sel:
            for l in c.get("levels") or []:
                k = math.floor(float(l["price"]) / grana) * grana
                hist[k] = hist.get(k, 0) + l["volume"]
        out[nome] = dict(O=sel[0]["open"], H=max(c["high"] for c in sel), L=min(c["low"] for c in sel),
                         C=sel[-1]["close"], V=sum(c["volume"] for c in sel), D=sum(c["delta"] for c in sel),
                         hist={str(k): v for k, v in hist.items()})
    return out or None


def carica_cache(strumento: str) -> tuple[Path, dict]:
    f = Path.home() / f".fabio-scala-dei-tempi-{strumento}.json"
    try:
        return f, json.load(open(f, encoding="utf-8"))
    except Exception:
        return f, {}


def riempi(chart: str, strumento: str, oggi: str, cassa, grana, budget: float) -> tuple[dict, int]:
    """Riempie le sedute chiuse che mancano, dalla piu' recente, finche' c'e' tempo."""
    f, cache = carica_cache(strumento)
    if cache.get("_grana") != grana:
        cache = {"_grana": grana}
    inizio = time.time()
    mancanti = 0
    g = dt.date.fromisoformat(oggi)
    for n in range(1, GIORNI_INDIETRO + 1):
        d = (g - dt.timedelta(days=n))
        if d.weekday() >= 5:
            continue
        k = d.isoformat()
        if k in cache:
            continue
        if time.time() - inizio > budget:
            mancanti += 1
            continue
        m = misura(chart, k, cassa, grana)
        cache[k] = m if m else {"vuoto": True}
    tmp = f.with_suffix(".tmp")
    json.dump(cache, open(tmp, "w", encoding="utf-8"))
    os.replace(tmp, f)
    return cache, mancanti


def fmt(x: float) -> str:
    s = f"{x:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
    return s[:-3] if s.endswith(",00") else s


def lotti(v: float) -> str:
    return f"{v / 1e6:.1f} mln lotti".replace(".", ",") if v >= 1e6 else f"{v / 1e3:.0f} mila lotti"


def dove(prezzo: float, val: float, vah: float) -> str:
    if prezzo > vah:
        return f"SOPRA di {fmt(prezzo - vah)}"
    if prezzo < val:
        return f"SOTTO di {fmt(val - prezzo)}"
    return "DENTRO"


def stampa(chart: str, budget: float) -> None:
    global PORTA
    if PORTA is None:
        PORTA = porta()
    salute = chiedi(f"/health?chart={chart}") or {}
    strumento = salute.get("instrument") or chart
    adesso = salute.get("marketTimeUtc") or dt.datetime.utcnow().isoformat()
    oggi = giorno_di_mercato(adesso)
    scala = chiedi(f"/scala?chart={chart}") or {}
    cassa_txt = scala.get("cassa") or "13:30Z-20:00Z"
    cassa = tuple(x.strip("Z") for x in cassa_txt.split("-"))
    s = float(scala.get("punti") or 1)
    grana = max(0.01, round(5 * s, 2)) if s < 1 else 5.0
    cache, mancanti = riempi(chart, strumento, oggi, cassa, grana, budget)
    oggi_m = misura(chart, oggi, cassa, grana) or {}
    prezzo = (oggi_m.get("giorno") or {}).get("C")

    print(f"\n{'─' * 78}\n0. DAL PIU' LONTANO AL PIU' VICINO — {strumento} (cassa {cassa_txt}, fasce da {fmt(grana)})\n{'─' * 78}")
    if mancanti:
        print(f"  la storia e' in costruzione: mancano {mancanti} sedute, arrivano ai prossimi giri")

    sedute = sorted(k for k, v in cache.items() if not k.startswith("_") and v and not v.get("vuoto"))
    tutte = [(k, cache[k]) for k in sedute] + ([(oggi, oggi_m)] if oggi_m else [])

    # --- le settimane ------------------------------------------------------------------------
    settimane: dict[str, list] = {}
    for k, v in tutte:
        d = dt.date.fromisoformat(k)
        lun = (d - dt.timedelta(days=d.weekday())).isoformat()
        settimane.setdefault(lun, []).append((k, v))
    vmax = max((sum(x["giorno"]["V"] for _, x in lst if "giorno" in x) for lst in settimane.values()), default=1)
    print("  LE SETTIMANE (tutte le ore, notti comprese)")
    prec = None
    for lun in sorted(settimane)[-4:]:
        lst = [x for _, x in settimane[lun] if "giorno" in x]
        if not lst:
            continue
        hist: dict[float, float] = {}
        for x in lst:
            for kk, vv in x["giorno"]["hist"].items():
                hist[float(kk)] = hist.get(float(kk), 0) + vv
        vol = sum(x["giorno"]["V"] for x in lst)
        v = valore(hist)
        hi = max(x["giorno"]["H"] for x in lst)
        lo = min(x["giorno"]["L"] for x in lst)
        nota = ""
        if vol < 0.2 * vmax:
            nota = "   <- poco volume: quotazione, non valore"
        elif prec:
            nota = f"   valore {'SU' if v[0] > prec[0] else 'GIU'} di {fmt(abs(v[0] - prec[0]))}"
        corrente = " in corso" if lun == max(settimane) else ""
        print(f"    {lun[8:10]}/{lun[5:7]}  {fmt(lo)} - {fmt(hi)}  C {fmt(lst[-1]['giorno']['C'])}  "
              f"POC {fmt(v[0])}  valore {fmt(v[1])}-{fmt(v[2])}  {lotti(vol)}{corrente}{nota}")
        if vol >= 0.2 * vmax:
            prec = v

    # --- le sedute di cassa --------------------------------------------------------------------
    print("  LE SEDUTE DI CASSA")
    prec = None
    giorni_it = ["lun", "mar", "mer", "gio", "ven", "sab", "dom"]
    righe = []
    for k, x in tutte[-8:]:
        c = x.get("cassa")
        if not c or k == oggi:
            continue
        v = valore({float(a): b for a, b in c["hist"].items()})
        mig = ""
        if prec:
            sovrapposto = not (v[1] > prec[2] or v[2] < prec[1])
            mig = f"valore {'SU' if v[0] > prec[0] else 'GIU'}{'' if sovrapposto else ' e staccato'}"
        chiude = "sopra" if c["C"] > v[2] else "sotto" if c["C"] < v[1] else "dentro"
        d = dt.date.fromisoformat(k)
        righe.append(f"    {giorni_it[d.weekday()]} {k[8:10]}  {fmt(c['O'])} -> {fmt(c['C'])}  "
                     f"POC {fmt(v[0])}  valore {fmt(v[1])}-{fmt(v[2])}  delta {('%+d' % c['D'])}  "
                     f"chiude {chiude} il valore  {mig}")
        prec = v
    for r in righe[-6:]:
        print(r)

    # --- dove sta il prezzo ------------------------------------------------------------------
    if prezzo is not None:
        print(f"  DOVE STA IL PREZZO ADESSO ({fmt(prezzo)})")
        ultime = [x["giorno"] for _, x in tutte[-20:] if "giorno" in x]
        if ultime:
            hi = max(x["H"] for x in ultime)
            lo = min(x["L"] for x in ultime)
            pct = 100 * (prezzo - lo) / (hi - lo) if hi > lo else 50
            print(f"    nel range delle ultime {len(ultime)} sedute {fmt(lo)} - {fmt(hi)}: al {pct:.0f}%"
                  f"  (dal massimo {fmt(hi - prezzo)} punti)")
        sett = sorted(settimane)
        if len(sett) >= 2:
            lst = [x for _, x in settimane[sett[-2]] if "giorno" in x]
            hist: dict[float, float] = {}
            for x in lst:
                for kk, vv in x["giorno"]["hist"].items():
                    hist[float(kk)] = hist.get(float(kk), 0) + vv
            v = valore(hist)
            if v:
                print(f"    rispetto al valore della settimana scorsa ({fmt(v[1])}-{fmt(v[2])}): {dove(prezzo, v[1], v[2])}")
        ieri = [x for k, x in tutte if k != oggi and x.get("cassa")]
        if ieri:
            v = valore({float(a): b for a, b in ieri[-1]["cassa"]["hist"].items()})
            print(f"    rispetto al valore della cassa di ieri ({fmt(v[1])}-{fmt(v[2])}): {dove(prezzo, v[1], v[2])}")

    # --- gli eventi --------------------------------------------------------------------------
    try:
        eventi = json.load(open(EVENTI, encoding="utf-8"))
    except Exception:
        eventi = []
    g = dt.date.fromisoformat(oggi)
    passati = [e for e in eventi if (g - dt.timedelta(days=4)).isoformat() <= e["data"] < oggi]
    prossimi = [e for e in eventi if oggi <= e["data"] <= (g + dt.timedelta(days=2)).isoformat()]
    print("  GLI EVENTI (docs/research/calendario/eventi.json)")
    for e in passati:
        print(f"    {e['data'][8:10]}/{e['data'][5:7]} {e.get('ora', ''):>5}  {e['evento']}"
              f"{'  -> ' + e['esito'] if e.get('esito') else ''}")
    for e in prossimi:
        segno = ">>" if e["data"] == oggi else "  "
        print(f"  {segno}{e['data'][8:10]}/{e['data'][5:7]} {e.get('ora', ''):>5}  {e['evento']}"
              f"  [{e.get('peso', '')}]{'  -> ' + e['esito'] if e.get('esito') else ''}")
    if not passati and not prossimi:
        print("    nessun evento nel calendario: va aggiornato a mano, a inizio giornata")


if __name__ == "__main__":
    import sys
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--chart", default="NQZ6")
    ap.add_argument("--riempi", action="store_true", help="riempie la cache senza limite di tempo")
    ap.add_argument("--budget", type=float, default=9.0, help="secondi per riempire la cache in questo giro")
    arg = ap.parse_args()
    stampa(arg.chart, 3600 if arg.riempi else arg.budget)
