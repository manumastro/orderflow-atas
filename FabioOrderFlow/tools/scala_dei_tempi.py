#!/usr/bin/env python3
"""La scala dei tempi: dal piu' lontano al piu' vicino, come la guarderebbe una persona.

Prima le sedute di cassa, poi dove sta il prezzo rispetto a tutto questo, poi gli eventi.
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
        if gj >= gi:  # a parita' si sale, come l'indicatore
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
    # la notte (dalle 22:00Z del giorno prima all'apertura della cassa) si misura a parte: senza, la
    # "seduta finora" mescolava notte e cassa (lacuna segnalata dalla fase 4 il 15/10)
    for nome, filtro in (("giorno", lambda c: True),
                         ("notte", lambda c: c["time"][:10] < giorno or c["time"][11:16] < cassa[0]),
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
    # La grana di POC e bordi e' 1 punto, come l'indicatore (LeZoneDelValore.cs) e come il
    # profilo di ATAS: fino al 28 settembre 2026 era 5, e il VAL di lunedi' 21 usciva 30.485
    # nel giro e 30.514 sul chart. Una trend day col volume piatto amplifica la differenza.
    grana = max(0.01, round(1 * s, 2)) if s < 1 else 1.0
    cache, mancanti = riempi(chart, strumento, oggi, cassa, grana, budget)
    oggi_m = misura(chart, oggi, cassa, grana) or {}
    prezzo = (oggi_m.get("giorno") or {}).get("C")

    print(f"\n== TAB 3 · SEDUTA — {strumento} M1 (cassa {cassa_txt}, fasce da {fmt(grana)})")
    if mancanti:
        print(f"  la storia e' in costruzione: mancano {mancanti} sedute, arrivano ai prossimi giri")

    sedute = sorted(k for k, v in cache.items() if not k.startswith("_") and v and not v.get("vuoto"))
    tutte = [(k, cache[k]) for k in sedute] + ([(oggi, oggi_m)] if oggi_m else [])

    # --- le settimane: tolte il 28 settembre 2026 --------------------------------------------
    # Il valore della settimana non e' un riferimento del live. La cornice di Fabio sono le sedute
    # di cassa dei giorni prima ("below the cash session of the previous two days",
    # fabio_4.txt 11:57) e il composito di 90 giorni (14:18). Il 28 settembre il VAL della
    # settimana, 30.650, era stato dato come "il livello che decide": non veniva dal metodo.

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
    for r in righe[-3:]:  # le tre ultime: i giorni prima li porta la tab 2
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
        ieri = [x for k, x in tutte if k != oggi and x.get("cassa")]
        if ieri:
            v = valore({float(a): b for a, b in ieri[-1]["cassa"]["hist"].items()})
            print(f"    rispetto al valore della cassa di ieri ({fmt(v[1])}-{fmt(v[2])}): {dove(prezzo, v[1], v[2])}")
        # IL PROFILE FRAMING, lo stesso conto delle zone sul chart (LeZoneDelValore.cs): se il
        # prezzo e' fuori dal valore delle ultime due sedute, si torna indietro fino alla prima
        # seduta il cui valore lo contiene; se nessuna lo contiene, la piu' recente col valore
        # oltre il prezzo, dalla parte in cui e' uscito.
        vs = [(k, valore({float(a): b for a, b in x["cassa"]["hist"].items()})) for k, x in tutte
              if k != oggi and x.get("cassa")]
        if len(vs) >= 2:
            due = vs[-2:]
            lo2 = min(v[1] for _, v in due)
            hi2 = max(v[2] for _, v in due)
            if lo2 <= prezzo <= hi2:
                print(f"    dentro il valore delle ultime due sedute ({fmt(lo2)}-{fmt(hi2)}): niente framing piu' indietro")
            else:
                sotto = prezzo < lo2
                prima = vs[:-2][::-1][:10]
                trovata = next(((k, v) for k, v in prima if v[1] <= prezzo <= v[2]), None) or next(
                    ((k, v) for k, v in prima if (v[2] < prezzo if sotto else v[1] > prezzo)), None)
                lato = "sotto" if sotto else "sopra"
                if trovata:
                    k, v = trovata
                    d = dt.date.fromisoformat(k)
                    print(f"    {lato} il valore delle ultime due sedute ({fmt(lo2)}-{fmt(hi2)}): il framing torna al "
                          f"{giorni_it[d.weekday()]} {k[8:10]}, valore {fmt(v[1])}-{fmt(v[2])} POC {fmt(v[0])}, "
                          f"{dove(prezzo, v[1], v[2])}")
                else:
                    print(f"    {lato} il valore delle ultime due sedute ({fmt(lo2)}-{fmt(hi2)}): nessuna seduta piu' indietro lo contiene")

    # --- la struttura oraria ---------------------------------------------------------------
    # Quello che una persona guarda sull'H1: i massimi e minimi di swing delle ultime tre
    # giornate (un'ora piu' alta, o piu' bassa, delle due prima e delle due dopo) e l'andamento
    # delle ultime ore. Sono i livelli di struttura, diversi da quelli di volume.
    fine = adesso[:16] + ":00Z" if len(adesso) >= 16 else adesso
    inizio_h = (dt.datetime.fromisoformat(adesso.replace("Z", "+00:00")) - dt.timedelta(hours=72))
    rh = chiedi(f"/candles?chart={chart}&from={inizio_h.strftime('%Y-%m-%dT%H:%M:%SZ')}&to={fine}", timeout=30)
    cs = (rh or {}).get("candles") or []
    ore: dict[str, dict] = {}
    for c in cs:
        k = c["time"][:13]
        o = ore.get(k)
        if o is None:
            ore[k] = dict(O=c["open"], H=c["high"], L=c["low"], C=c["close"], D=c["delta"], V=c["volume"])
        else:
            o["H"] = max(o["H"], c["high"]); o["L"] = min(o["L"], c["low"])
            o["C"] = c["close"]; o["D"] += c["delta"]; o["V"] += c["volume"]
    chiavi = sorted(ore)
    if len(chiavi) >= 5 and prezzo is not None:
        alti, bassi = [], []
        for i in range(2, len(chiavi) - 2):
            h = ore[chiavi[i]]
            vicine = [ore[chiavi[j]] for j in (i - 2, i - 1, i + 1, i + 2)]
            if all(h["H"] > x["H"] for x in vicine):
                alti.append((h["H"], chiavi[i]))
            if all(h["L"] < x["L"] for x in vicine):
                bassi.append((h["L"], chiavi[i]))
        livelli = [(p, "massimo", k) for p, k in alti] + [(p, "minimo", k) for p, k in bassi]
        sopra = sorted([x for x in livelli if x[0] > prezzo])[:4]
        sotto = sorted([x for x in livelli if x[0] <= prezzo], reverse=True)[:4]
        quando = lambda k: f"{k[8:10]}/{k[5:7]} {int(k[11:13]) + 2:02d}h"
        print("  LA STRUTTURA ORARIA (H1, ultime 72 ore)")
        for p, tipo, k in reversed(sopra):
            print(f"    sopra  {fmt(p)}  {tipo} di swing H1 delle {quando(k)}")
        for p, tipo, k in sotto:
            print(f"    sotto  {fmt(p)}  {tipo} di swing H1 delle {quando(k)}")
        ultime = chiavi[-6:]
        seq = "  ".join(f"{int(k[11:13]) + 2:02d}h {fmt(ore[k]['C'])}({ore[k]['D']:+d})" for k in ultime)
        print(f"    ultime ore (chiusura, delta): {seq}")
        # LA FORMA SI LEGGE SUGLI SWING, non sul primo e sull'ultimo estremo della finestra: il 15/10
        # alle 18:43 il confronto fra l'ora di sei ore prima e l'ultima diceva "massimi e minimi
        # crescenti" mentre i minimi delle ore 13, 14 e 15 scendevano (lacuna segnalata dal quadro).
        # Qui: gli ultimi due massimi di swing e gli ultimi due minimi di swing delle 72 ore.
        def verso(punti: list) -> str | None:
            if len(punti) < 2:
                return None
            (a, _), (b, _) = sorted(punti, key=lambda x: x[1])[-2:]
            return "crescenti" if b > a else "decrescenti" if b < a else "uguali"
        vm, vn = verso(alti), verso(bassi)
        forma = (f"massimi e minimi {vm}" if vm and vm == vn and vm != "uguali"
                 else f"massimi {vm or 'n.d.'}, minimi {vn or 'n.d.'}")
        print(f"    gli ultimi due swing H1: {forma}")

    # --- gli eventi --------------------------------------------------------------------------
    try:
        eventi = json.load(open(EVENTI, encoding="utf-8"))
    except Exception:
        eventi = []
    g = dt.date.fromisoformat(oggi)
    passati = [e for e in eventi if (g - dt.timedelta(days=1)).isoformat() <= e["data"] < oggi][-2:]
    prossimi = [e for e in eventi if oggi <= e["data"] <= (g + dt.timedelta(days=2)).isoformat()][:5]
    print("  GLI EVENTI (docs/research/calendario/eventi.json)")
    for e in passati:
        print((f"    {e['data'][8:10]}/{e['data'][5:7]} {e.get('ora', ''):>5}  {e['evento']}"
               f"{'  -> ' + e['esito'] if e.get('esito') else ''}")[:105])
    for e in prossimi:
        segno = ">>" if e["data"] == oggi else "  "
        # tagliata a 120 caratteri: il 15 ottobre 2026 gli esiti scritti dallo studio globale
        # portavano il giro oltre la soglia oltre la quale arriva all'agente solo in anteprima
        print((f"  {segno}{e['data'][8:10]}/{e['data'][5:7]} {e.get('ora', ''):>5}  {e['evento']}"
               f"  [{e.get('peso', '')}]{'  -> ' + e['esito'] if e.get('esito') else ''}")[:105])
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
