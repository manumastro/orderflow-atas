#!/usr/bin/env python3
"""La prova del server della pagina X-Ray contro un calcolo indipendente, sui dati veri.

Il server (XrayLive/Server, C#) e questo script (Python, con xray_query.colonne) leggono lo
stesso archivio con due codici diversi: i numeri devono coincidere. Per ogni giornata scelta, a
tre istanti (apertura, meta' cassa, fine) controlla:
- la GEX netta dei market maker per scadenza e i muri;
- il DEX di ogni gruppo sommato dall'inizio della giornata (e' un flusso al minuto);
- il profilo della gamma osservata e il flip all'istante;
- la heat map: l'ultima colonna e' la somma delle altre;
- la deriva: l'ultimo punto del DEX cumulato e' quello di "adesso";
- la base ES-SPX: la mediana di ES - spot sui minuti di cassa, dal bridge (se risponde).

    python quant/strumenti/xray/scripts/prova_server.py                      # server su 8790, ultime 3 giornate
    python quant/strumenti/xray/scripts/prova_server.py --url http://127.0.0.1:8791 --giorni 2026-10-30
"""
from __future__ import annotations

import argparse
import json
import os
import statistics
import sys
import urllib.request
from datetime import datetime, timezone

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import xray_query as q  # noqa: E402

ROOT = os.path.normpath(os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "..", "dati", "xray"))
GRUPPI = ["firm", "broker", "procust", "nonprocust"]
SCADENZE = ["0dte", "next_expiry", "opex"]


class Prova:
    def __init__(self) -> None:
        self.fallite: list[str] = []
        self.passate = 0

    def vicino(self, a, b, cosa: str, tolleranza: float = 0.011) -> None:
        ok = (a is None and b is None) or (a is not None and b is not None and abs(a - b) <= tolleranza * max(1.0, abs(b) * 0.001))
        if ok:
            self.passate += 1
        else:
            self.fallite.append(f"{cosa}: server {a} contro {b}")
            print(f"  NO  {cosa}: server {a} contro calcolo {b}")


def get(url: str, percorso: str) -> dict:
    with urllib.request.urlopen(f"{url}/api/{percorso}", timeout=60) as r:
        return json.load(r)


def colonne(famiglia: str, giorno: str) -> list[dict]:
    return sorted(q.colonne(os.path.join(ROOT, "series", "SPX", famiglia, giorno + ".jsonl")), key=lambda c: c["ts"])


def ultima(cs: list[dict], ts: int) -> dict | None:
    prima = [c for c in cs if c["ts"] <= ts]
    return prima[-1] if prima else None


def netto(c: dict) -> float:
    return sum(x or 0 for x in c["call"]) + sum(x or 0 for x in c["put"])


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--url", default="http://127.0.0.1:8790")
    ap.add_argument("--giorni", nargs="*")
    a = ap.parse_args()
    p = Prova()

    giorni = a.giorni or get(a.url, "giorni")["giorni"][:3]
    for g in giorni:
        ist = get(a.url, f"istanti?giorno={g}")
        tempi = ist["istanti"]
        if not tempi:
            continue
        cassa = ist["cassa"]
        scelti = sorted({t for t in (
            max((t for t in tempi if t <= cassa["apertura"] + 600_000), default=tempi[0]),
            max((t for t in tempi if t <= (cassa["apertura"] + cassa["chiusura"]) // 2), default=tempi[0]),
            tempi[-1])})
        print(f"\n== {g}: {len(tempi)} istanti, provo {[datetime.fromtimestamp(t / 1000, timezone.utc).strftime('%H:%MZ') for t in scelti]}")
        gex = {s: colonne(f"gex__mm_{s}", g) for s in SCADENZE}
        dex = {(gr, sc): colonne(f"dex__{gr}_{sc}", g) for gr in GRUPPI for sc in ("0dte", "all")}
        oss = colonne("observed__gamma", g)

        for ts in scelti:
            ad = get(a.url, f"adesso?giorno={g}&ts={ts}")
            rif = ad["riferimento"]["prezzo"]
            for x in ad["gex"]:
                c = ultima(gex[x["scadenza"]], ts)
                if not x["presente"]:
                    p.vicino(None if c is None else 0, None, f"{g} {ts} gex {x['scadenza']} assente")
                    continue
                p.vicino(x["netto"], round(netto(c), 3), f"{g} {ts} GEX netta {x['scadenza']}")
                vicini = [(s, (cc or 0) + (pp or 0)) for s, cc, pp in zip(c["strikes"], c["call"], c["put"]) if abs(s - rif) <= rif * 0.05]
                pos = max((v for v in vicini if v[1] > 0), key=lambda v: v[1], default=None)
                if pos and x["muroFreno"]:
                    p.vicino(x["muroFreno"]["strike"], pos[0], f"{g} {ts} muro freno {x['scadenza']}")
            for d in ad["dex"]:
                for sc, campo in (("0dte", "zeroDte"), ("all", "tutte")):
                    cs = [c for c in dex[(d["gruppo"], sc)] if c["ts"] <= ts]
                    atteso = round(sum(netto(c) for c in cs), 2) if cs else None
                    p.vicino(d[campo], atteso, f"{g} {ts} DEX cumulato {d['gruppo']} {sc}", tolleranza=0.05)

            pr = get(a.url, f"profilo?giorno={g}&ts={ts}&greca=gamma&ampiezza=20")
            c = ultima(oss, ts)
            if pr.get("presente") and c:
                mappa = dict(zip(c["prices"], c["values"]))
                diff = max(abs(v - round(mappa[pz], 2)) for pz, v in zip(pr["prezzi"], pr["valori"]) if v is not None)
                p.vicino(diff, 0.0, f"{g} {ts} profilo osservato, differenza massima")

            hm = get(a.url, f"heatmap?giorno={g}&ts={ts}&metrica=gex&ampiezza=3")
            for riga in hm["valori"]:
                parti = [v for v in riga[:-1] if v is not None]
                if parti:
                    p.vicino(riga[-1], round(sum(parti), 4), f"{g} {ts} heat map, totale di riga", tolleranza=0.002)

        dr = get(a.url, f"deriva?giorno={g}&passo=1")
        ultimo = get(a.url, f"adesso?giorno={g}&ts={dr['tempi'][-1]}")
        for d in ultimo["dex"]:
            serie = [v for v in dr["dexZeroDte"][d["gruppo"]] if v is not None]
            if serie:
                p.vicino(serie[-1], d["zeroDte"], f"{g} deriva = adesso, DEX 0DTE {d['gruppo']}", tolleranza=0.05)

        base = get(a.url, f"base?giorno={g}")
        if base.get("valore") is not None and base.get("giorno") == g:
            try:
                charts = json.load(urllib.request.urlopen("http://127.0.0.1:8787/charts", timeout=5))["charts"]
                es = next(c["id"] for c in charts if c["instrument"].startswith("ES") and len(c["instrument"]) > 2 and c["timeFrame"] == "M1")
                da = datetime.fromtimestamp((cassa["apertura"] - 60_000) / 1000, timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
                fino = datetime.fromtimestamp(cassa["chiusura"] / 1000, timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
                cand = json.load(urllib.request.urlopen(f"http://127.0.0.1:8787/candles?chart={es}&from={da}&to={fino}", timeout=30))["candles"]
                chiusure = {int(datetime.fromisoformat(k["time"].replace("Z", "+00:00")).timestamp() * 1000) + 60_000: k["close"] for k in cand}
                diff = [chiusure[c["ts"]] - c["spot"] for c in gex["0dte"]
                        if cassa["apertura"] <= c["ts"] < cassa["chiusura"] and c["ts"] in chiusure]
                p.vicino(base["valore"], round(statistics.median(diff), 2), f"{g} base ES-SPX ({len(diff)} minuti)")
            except (OSError, StopIteration, KeyError, ValueError) as e:
                print(f"  (base non verificata: bridge non disponibile, {e})")

    print(f"\n{p.passate} confronti uguali, {len(p.fallite)} diversi")
    return 1 if p.fallite else 0


if __name__ == "__main__":
    sys.exit(main())
