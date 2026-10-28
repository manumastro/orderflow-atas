#!/usr/bin/env python3
"""La gamma dei dealer sulle opzioni del future, calcolata da noi dalla catena che ATAS riceve.

Gli indicatori opzioni di ATAS tengono i loro numeri in campi offuscati: non si leggono. La catena
si', dal bridge (`/opzioni`): strike, tipo, open interest, bid, ask, ultimo. Da li' questo script
calcola la GEX con un modello dichiarato, e la salva in una cache che il giro d'orizzonte stampa.

    python gex.py                    # NQZ6, tutte le scadenze entro 70 giorni, salva la cache
    python gex.py --giorni 7         # solo la settimana
    python gex.py --stampa           # stampa la cache senza interrogare il bridge

**Il modello**, una riga per scelta (quant/metodo/le-opzioni-in-atas.md):

- prezzo dell'opzione: il medio fra bid e ask; senza un book valido l'ultimo; senza, lo strike salta;
- volatilita' implicita: Black-76 (opzioni su future), tasso zero, dalla sola opzione **fuori dal
  denaro** di ogni strike (la call sopra il future, la put sotto), e vale per call e put;
- scadenza: le 16:00 di New York; le trimestrali (`Regular`) alle 9:30, come la SOQ del CME;
- **GEX di uno strike** = gamma x open interest x moltiplicatore (20 $ NQ, 50 $ ES) x F^2 x 1%: i dollari che il dealer compra o
  vende sul future per un movimento dell'1%;
- **convenzione A** (dei fornitori, SqueezeMetrics, e la piu' diffusa): i dealer sono **lunghi** le
  call e **corti** le put. GEX = call - put. Positiva: il dealer smorza. Negativa: amplifica.
- **convenzione B** [GPP]: i dealer sono **corti** di tutto, la GEX e' negativa ovunque e dice solo
  **dove** la copertura pesa di piu'.
- **zero gamma**: il prezzo del future al quale la GEX totale A cambia segno, ricalcolando la gamma
  di ogni strike con la sua volatilita' ferma (sticky strike) su una griglia di +-8% a passi dello 0,1%;
- **il future**: la chiusura dell'ultima barra del chart M30 dello strumento quando il bridge legge la
  scadenza (`underlyingPrice` di `/opzioni`, cioe' l'ultimo prezzo scambiato). Le scadenze si leggono
  una dopo l'altra in circa tre minuti, quindi ogni scadenza ha il suo future e la sua ora; il
  `future` del riassunto e' quello dell'ultima lettura, con la sua ora in `future_quando`;
- **strike**: +-4 mosse attese della scadenza, fra 25 e 120 per lato (dal 27 ottobre 2026; prima sempre 120)
  per scadenza, nel tetto di 500 sottoscrizioni per richiesta del bridge
  (`LeOpzioni.cs`): 120 x 2 lati x 2 tipi = 480; un giorno con piu' serie ne prende meno.

L'open interest e' quello di **ieri**, e non vede le posizioni aperte e chiuse in giornata (gli
0DTE): un limite del dato, non del calcolo. Questi numeri sono una **descrizione**: nessun paper
prova i muri o lo zero gamma come livelli (quant/studi/opzioni-gamma-es/1-idea.md).
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import math
import os
import sys
import time
from pathlib import Path
from zoneinfo import ZoneInfo

import bridge as client

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")

NY = ZoneInfo("America/New_York")
MOLTIPLICATORE = {"NQ": 20.0, "MNQ": 2.0, "ES": 50.0, "MES": 5.0}


def cache_di(strumento: str) -> Path:
    return Path.home() / f".fabio-gex-{strumento}.json"


def phi(x: float) -> float:
    return math.exp(-0.5 * x * x) / math.sqrt(2 * math.pi)


def ncdf(x: float) -> float:
    return 0.5 * (1 + math.erf(x / math.sqrt(2)))


def black76(f: float, k: float, t: float, s: float, call: bool) -> float:
    if s <= 0 or t <= 0:
        return max(0.0, (f - k) if call else (k - f))
    d1 = (math.log(f / k) + 0.5 * s * s * t) / (s * math.sqrt(t))
    d2 = d1 - s * math.sqrt(t)
    return f * ncdf(d1) - k * ncdf(d2) if call else k * ncdf(-d2) - f * ncdf(-d1)


def implicita(prezzo: float, f: float, k: float, t: float, call: bool) -> float | None:
    """La volatilita' che rende il prezzo, per bisezione; None se il prezzo e' fuori dai limiti."""
    if prezzo <= max(0.0, (f - k) if call else (k - f)) + 1e-9:
        return None
    lo, hi = 0.01, 3.0
    if black76(f, k, t, hi, call) < prezzo:
        return None
    for _ in range(60):
        m = (lo + hi) / 2
        if black76(f, k, t, m, call) < prezzo:
            lo = m
        else:
            hi = m
    return (lo + hi) / 2


def gamma(f: float, k: float, t: float, s: float) -> float:
    d1 = (math.log(f / k) + 0.5 * s * s * t) / (s * math.sqrt(t))
    return phi(d1) / (f * s * math.sqrt(t))


def scadenza_utc(giorno: str, tipo: str) -> dt.datetime:
    d = dt.date.fromisoformat(giorno[:10])
    ora = dt.time(9, 30) if tipo == "Regular" else dt.time(16, 0)
    return dt.datetime.combine(d, ora, NY).astimezone(dt.timezone.utc)


def prezzo_di(o: dict) -> float | None:
    b, a = o.get("bid"), o.get("ask")
    if b is not None and a is not None and a >= b > 0:
        return (a + b) / 2
    return o.get("last")


def serie_della_catena(base: str, chart: str, giorni: int) -> list[dict]:
    s = client.get(base, "/opzioni", {"chart": chart}, timeout=60)["series"]
    oggi = dt.datetime.now(dt.timezone.utc)
    return [x for x in s if scadenza_utc(x["expiration"], x["type"]) > oggi
            and (scadenza_utc(x["expiration"], x["type"]) - oggi).days <= giorni]


def leggi_stabile(base: str, chart: str, giorno: str, strikes: int, attesa: int, giri: int = 6) -> dict:
    """La catena di una scadenza, riletta finche' l'open interest totale smette di crescere.

    Le quotazioni arrivano dalla connessione un po' alla volta. Il 15 ottobre 2026, con un'attesa fissa
    di 8 secondi, la catena di ES e' arrivata a meta' (14 scadenze su 32 perse, lo 0DTE con 55 mila
    contratti invece di 110 mila). Le sottoscrizioni restano aperte nel bridge, quindi rileggere costa
    poco: si rilegge finche' l'OI cresce meno dello 0,5%, al massimo `giri` volte.

    LA VELOCITA' (27 ottobre 2026: la GEX di ES in 441 secondi). Il bridge aspettava che ogni strike
    fosse quotato, e quelli lontani non lo sono mai: ogni richiesta arrivava al tempo massimo. Ora il
    bridge smette anche dopo `quiet` millisecondi senza quotazioni nuove, e una scadenza che alla prima
    lettura non ha nessuna quotazione non si rilegge."""
    oi = lambda d: sum((o.get("openInterest") or 0) for o in d.get("options", []))  # noqa: E731
    d = client.get(base, "/opzioni", {"chart": chart, "expiration": giorno, "strikes": strikes, "wait": attesa,
                                      "quiet": 1000}, timeout=180)
    primo, letture = oi(d), 1
    for _ in range(giri if d.get("quoted") else 0):
        prima = oi(d)
        nuovo = client.get(base, "/opzioni", {"chart": chart, "expiration": giorno, "strikes": strikes, "wait": 2,
                                              "quiet": 500}, timeout=60)
        letture += 1
        if oi(nuovo) <= prima * 1.005:
            d = nuovo if oi(nuovo) >= prima else d
            break
        d = nuovo
    else:
        d["instabile"] = True
    # il segnale dell'OI (lacuna della fase 3, 15 ottobre 2026): quanto e' cambiato fra la prima e
    # l'ultima lettura. Stabile = l'ultima rilettura ha aggiunto meno dello 0,5%.
    d["_oi"] = {"oi_prima_lettura": primo, "oi_ultima_lettura": oi(d), "letture": letture,
                "oi_stabile": not d.get("instabile", False)}
    return d


TETTO_BRIDGE = 500   # sottoscrizioni per richiesta, TettoSottoscrizioni in LeOpzioni.cs


def calcola(base: str, chart: str, giorni: int, strikes: int, attesa: int) -> dict:
    righe, scadenze, saltate, f, f_quando, f_inizio = [], [], [], None, None, None
    adesso = dt.datetime.now(dt.timezone.utc)
    serie = serie_della_catena(base, chart, giorni)
    fatti: set[str] = set()
    # LA FINESTRA DEGLI STRIKE (27 ottobre 2026): +-4 mosse attese della scadenza, non sempre `strikes`
    # per lato. Fuori da li' la gamma e' quasi zero e le opzioni quasi mai quotate: chiederle costava solo
    # tempo. La prima scadenza si legge intera, e da li' vengono il passo degli strike e la volatilita'.
    passo_k, vol_nota = None, None
    for s in serie:
        giorno = s["expiration"][:10]
        if giorno in fatti:
            # il bridge legge tutte le serie di un giorno in una richiesta: un giorno si legge una volta
            continue
        fatti.add(giorno)
        n_serie = sum(1 for x in serie if x["expiration"][:10] == giorno)
        per_lato = max(1, min(strikes, TETTO_BRIDGE // (4 * n_serie)))
        if passo_k and vol_nota and f:
            t_s = max((scadenza_utc(s["expiration"], s["type"]) - adesso).total_seconds(), 600) / (365 * 86400)
            per_lato = min(per_lato, max(25, math.ceil(4 * vol_nota * math.sqrt(t_s) * f / passo_k)))
        # le riletture costano tempo: fino a sei per le scadenze che pesano (entro sette giorni e le
        # trimestrali), due per le settimanali lontane, quotate a pezzi e quasi senza OI. Il 15 ottobre
        # 2026, con sei riletture per tutte, il calcolo di NQ e' passato da tre minuti a cinque e mezzo
        # per scadenze che pesano il 7% della GEX A.
        vicina = (scadenza_utc(s["expiration"], s["type"]) - adesso).days <= 7 or s["type"] == "Regular"
        d = leggi_stabile(base, chart, giorno, per_lato, attesa, giri=6 if vicina else 2)
        f = float(d["underlyingPrice"])
        f_quando = d.get("at")
        f_inizio = f_inizio or f
        t = max((scadenza_utc(s["expiration"], s["type"]) - adesso).total_seconds(), 600) / (365 * 86400)
        per_strike: dict[float, dict] = {}
        for o in d["options"]:
            per_strike.setdefault(float(o["strike"]), {})[o["type"]] = o
        # la volatilita' dello strike dall'opzione fuori dal denaro; i buchi dai vicini
        vol = {}
        for k, coppia in per_strike.items():
            fuori = coppia.get("Call") if k > f else coppia.get("Put")
            p = prezzo_di(fuori) if fuori else None
            v = implicita(p, f, k, t, k > f) if p else None
            if v:
                vol[k] = v
        noti = sorted(vol)
        vicini = sorted(k for k in per_strike if abs(k - f) <= 0.02 * f)
        if len(vicini) > 2:
            # il passo piu' fine visto finora: le scadenze lontane hanno strike ogni 25 o 50 punti, e
            # prenderne il passo stringeva la finestra della trimestrale (il 27/10: 25 strike per lato)
            p_ = sorted(b - a for a, b in zip(vicini, vicini[1:]))[(len(vicini) - 1) // 2]
            passo_k = min(passo_k or p_, p_)
        if noti:
            vol_nota = max(vol_nota or 0, vol[min(noti, key=lambda x: abs(x - f))])
        if not noti:
            # una scadenza senza nessun prezzo valido non si perde in silenzio: si dichiara
            saltate.append({"giorno": giorno, "tipo": s["type"], "quotate": d.get("quoted"),
                            "opzioni": d.get("count"), "strike_per_lato": per_lato})
            continue
        n_gex = 0
        for k, coppia in per_strike.items():
            v = vol.get(k) or vol[min(noti, key=lambda x: abs(x - k))]
            oi_c = (coppia.get("Call") or {}).get("openInterest") or 0
            oi_p = (coppia.get("Put") or {}).get("openInterest") or 0
            vol_c = (coppia.get("Call") or {}).get("volume") or 0
            vol_p = (coppia.get("Put") or {}).get("volume") or 0
            if oi_c or oi_p or vol_c or vol_p:
                n_gex += 1
                righe.append({"k": k, "t": t, "v": v, "c": oi_c, "p": oi_p, "vc": vol_c, "vp": vol_p,
                              "scad": giorno})
        scadenze.append({"giorno": giorno, "tipo": s["type"], "serie": n_serie, "strike": n_gex,
                         "strike_per_lato": per_lato,
                         "quotate": d.get("quoted"), "opzioni": d.get("count"), "instabile": d.get("instabile", False),
                         "future": f, "future_quando": f_quando, **d.get("_oi", {}),
                         "oi": sum(r["c"] + r["p"] for r in righe if r["scad"] == giorno),
                         "vol_atm": round(vol[min(noti, key=lambda x: abs(x - f))], 4),
                         # la mossa attesa a una deviazione standard fino alla scadenza: F x vol x radice di T
                         "mossa_attesa": round(f * vol[min(noti, key=lambda x: abs(x - f))] * math.sqrt(t), 1)})
    return {"righe": righe, "scadenze": scadenze, "saltate": saltate, "future": f, "quando": adesso.isoformat(),
            "future_quando": f_quando, "future_inizio": f_inizio, "strike_per_lato": strikes}


def profilo(righe: list[dict], f: float, molt: float) -> dict:
    """GEX per strike nelle due convenzioni, totali, muri, zero gamma."""
    unita = molt * f * f * 0.01 / 1e6       # milioni di $ per 1% di movimento
    per_k: dict[float, list[float]] = {}
    for r in righe:
        g = gamma(f, r["k"], r["t"], r["v"]) * unita
        x = per_k.setdefault(r["k"], [0.0, 0.0])
        x[0] += g * r["c"]
        x[1] += g * r["p"]
    tot_a = sum(c - p for c, p in per_k.values())
    tot_b = -sum(c + p for c, p in per_k.values())

    def totale_a(s: float) -> float:
        u = molt * s * s * 0.01 / 1e6
        return sum(gamma(s, r["k"], r["t"], r["v"]) * u * (r["c"] - r["p"]) for r in righe)

    passo = 5.0 if f > 10000 else 1.0
    # +-8% (era +-4%: alle 11:04 del 15 ottobre 2026 la settimana non aveva uno zero dentro la griglia)
    griglia = sorted({round((f * (1 + i / 1000)) / passo) * passo for i in range(-80, 81)})
    valori = [(s, totale_a(s)) for s in griglia]
    zeri = []
    for (s1, g1), (s2, g2) in zip(valori, valori[1:]):
        if g1 == 0 or (g1 < 0) != (g2 < 0):
            zeri.append(s1 + (s2 - s1) * (0 - g1) / (g2 - g1) if g2 != g1 else s1)
    zero = min(zeri, key=lambda z: abs(z - f)) if zeri else None
    call_wall = max(per_k, key=lambda k: per_k[k][0]) if per_k else None
    put_wall = max(per_k, key=lambda k: per_k[k][1]) if per_k else None
    peso_b = sorted(per_k, key=lambda k: -(per_k[k][0] + per_k[k][1]))[:3]
    return {"tot_a": tot_a, "tot_b": tot_b, "zero": zero, "zeri": zeri,
            "call_wall": call_wall, "cw": per_k.get(call_wall, [0, 0])[0],
            "put_wall": put_wall, "pw": per_k.get(put_wall, [0, 0])[1], "peso_b": peso_b}


def riassunto(dati: dict, strumento: str) -> dict:
    molt = next((m for radice, m in sorted(MOLTIPLICATORE.items(), key=lambda x: -len(x[0]))
                 if strumento.startswith(radice)), 20.0)
    f, righe = dati["future"], dati["righe"]
    primo = min((r["scad"] for r in righe), default=None)
    entro7 = [r for r in righe if r["t"] <= 7 / 365]
    return {"strumento": strumento, "quando": dati["quando"], "future": f, "scadenze": dati["scadenze"],
            # i dollari a punto del future con cui si e' calcolata la GEX (20 NQ, 50 ES): scritti nel JSON
            # perche' chi legge non lo ricavi dalla docstring (lacuna della fase 3 di ES, 27 ottobre 2026)
            "moltiplicatore": molt,
            "scadenze_saltate": dati.get("saltate", []),
            "tutte": profilo(righe, f, molt),
            "prima": profilo([r for r in righe if r["scad"] == primo], f, molt) | {"giorno": primo},
            "settimana": profilo(entro7, f, molt) if entro7 else None,
            "per_strike": per_strike(righe, f, molt), "per_scadenza": per_scadenza(righe),
            # GEX, zero gamma e muri di ogni scadenza (lacuna segnalata dalla fase 3, 15 ottobre 2026)
            "gex_per_scadenza": {sc: {k: v for k, v in profilo([r for r in righe if r["scad"] == sc], f, molt).items()
                                      if k in ("tot_a", "tot_b", "zero", "call_wall", "put_wall")}
                                 for sc in sorted({r["scad"] for r in righe})},
            "future_quando": dati.get("future_quando"), "future_inizio": dati.get("future_inizio"),
            "future_fonte": ("bridge /opzioni, underlyingPrice: chiusura dell'ultima barra del chart M30 dello "
                             "strumento all'ora di future_quando, cioe' l'ultimo prezzo scambiato quando e' stata "
                             "letta l'ultima scadenza; ogni scadenza ha il suo in scadenze[].future"),
            "strike_per_lato": dati.get("strike_per_lato"), "limite_strike": f"{TETTO_BRIDGE} sottoscrizioni per richiesta",
            "volume_finestra": VOLUME_FINESTRA}


# Il volume delle opzioni: CurrentDayTotalVolume della quotazione di ATAS (LeOpzioni.cs), cioe' il volume
# del giorno di contrattazione del CME, che per le opzioni sui future comincia alle 17:00 di Chicago del
# giorno prima (22:00Z con Chicago in ora legale, 23:00Z in ora solare: in ora italiana le 00:00, salvo la
# settimana fra il cambio europeo e quello americano, quando sono le 23:00). Misurato il 15 ottobre 2026: alle
# 08:32-09:51 UTC i +-6% avevano gia' 19-21 mila contratti, quindi la notte e' dentro. L'ora esatta
# dell'azzeramento non e' ancora vista: una lettura fra le 22:00 e le 23:00 UTC deve dare quasi zero.
VOLUME_FINESTRA = ("giorno di contrattazione CME: dalle 17:00 di Chicago del giorno prima (00:00 italiane; 23:00 "
                   "nella settimana fra il cambio d'ora europeo e quello americano) "
                   "a ora; include la notte (misurato 15/10/2026); azzeramento alle 17:00 CT da verificare")


def per_strike(righe: list[dict], f: float, molt: float, banda: float = 0.06) -> list[dict]:
    """La tabella strike per strike entro +-6% dal future, sommata su tutte le scadenze: open interest,
    volume del giorno e GEX di call e put (milioni di $ per 1%, convenzione A: call +, put -).
    Il primo giro dello studio globale (15 ottobre 2026) la ricostruiva a mano: la misura il codice."""
    unita = molt * f * f * 0.01 / 1e6
    tab: dict[float, dict] = {}
    for r in righe:
        if abs(r["k"] / f - 1) > banda:
            continue
        g = gamma(f, r["k"], r["t"], r["v"]) * unita
        x = tab.setdefault(r["k"], {"k": r["k"], "oi_c": 0, "oi_p": 0, "vol_c": 0, "vol_p": 0, "gex_c": 0.0, "gex_p": 0.0})
        x["oi_c"] += r["c"]; x["oi_p"] += r["p"]
        x["vol_c"] += r.get("vc", 0); x["vol_p"] += r.get("vp", 0)
        x["gex_c"] += g * r["c"]; x["gex_p"] -= g * r["p"]
    out = sorted(tab.values(), key=lambda x: x["k"])
    for x in out:
        x["gex_c"], x["gex_p"] = round(x["gex_c"], 1), round(x["gex_p"], 1)
        x["gex_netta"] = round(x["gex_c"] + x["gex_p"], 1)
    return out


def per_scadenza(righe: list[dict], quanti: int = 5) -> dict:
    """Per ogni scadenza, gli strike con piu' open interest, con la volatilita' implicita."""
    out: dict[str, list] = {}
    for scad in sorted({r["scad"] for r in righe}):
        rr = sorted((r for r in righe if r["scad"] == scad), key=lambda r: -(r["c"] + r["p"]))[:quanti]
        out[scad] = [{"k": r["k"], "oi_c": r["c"], "oi_p": r["p"], "vol_c": r.get("vc", 0), "vol_p": r.get("vp", 0),
                      "iv": round(r["v"], 4)} for r in rr]
    return out


def n(x: float | None, dec: int = 0) -> str:
    if x is None:
        return "-"
    return f"{x:,.{dec}f}".replace(",", "X").replace(".", ",").replace("X", ".")


def stampa(r: dict, rientro: str = "  ", prezzo: float | None = None) -> None:
    import ora
    eta = (dt.datetime.now(dt.timezone.utc) - dt.datetime.fromisoformat(r["quando"])).total_seconds() / 60
    p = prezzo or r["future"]
    fq = f" delle {ora.hhmm(r['future_quando'])}" if r.get("future_quando") else ""
    print(f"{rientro}alle {ora.hhmm(r['quando'])} ({eta:.0f} min fa), future {n(r['future'], 2)}{fq}, {len(r['scadenze'])} scadenze, "
          f"OI di ieri, Black-76; milioni di $ per 1%")
    print(f"{rientro}convenzione A = dealer lunghi call e corti put (fornitori, ATAS):")
    for nome, x in (("tutte", r["tutte"]), ("entro 7 gg", r.get("settimana")),
                    (f"0DTE {r['prima']['giorno'][5:]}", r["prima"])):
        if not x:
            continue
        lato = "-" if x["zero"] is None else ("SOPRA" if p > x["zero"] else "SOTTO")
        print(f"{rientro}  {nome:<11} GEX A {x['tot_a']:+8,.0f}  zero gamma {n(x['zero'])} (prezzo {lato})  "
              f"call wall {n(x['call_wall'])}  put wall {n(x['put_wall'])}")
    b = r["tutte"]
    print(f"{rientro}convenzione B (dealer corti di tutto) {b['tot_b']:+,.0f}: la copertura pesa di piu' a "
          f"{', '.join(n(k) for k in b['peso_b'])}")
    sc = r["scadenze"]
    print(f"{rientro}volatilita' implicita ATM: " + "  ".join(
        f"{x['giorno'][5:]} {x['vol_atm'] * 100:.1f}%" + (f" (+-{x['mossa_attesa']:.0f})" if x.get("mossa_attesa") else "")
        for x in sc[:4]))


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--strumento", default="NQZ6")
    ap.add_argument("--giorni", type=int, default=70, help="scadenze entro questi giorni")
    ap.add_argument("--strikes", type=int, default=120, help="strike per lato, per scadenza (tetto 500 sottoscrizioni: 120 x 4 = 480)")
    ap.add_argument("--attesa", type=int, default=8, help="secondi di attesa delle quotazioni per scadenza")
    ap.add_argument("--stampa", action="store_true", help="stampa la cache senza interrogare il bridge")
    ap.add_argument("--out", help="cartella dove copiare il risultato (gex-<strumento>.json), per lo studio globale")
    a = ap.parse_args()
    cache = cache_di(a.strumento)
    if not a.stampa:
        esegui(a.strumento, a.giorni, a.strikes, a.attesa)
    if not cache.exists():
        raise SystemExit(f"nessuna cache in {cache}")
    r = json.loads(cache.read_text(encoding="utf-8"))
    if a.out:
        Path(a.out).mkdir(parents=True, exist_ok=True)
        (Path(a.out) / f"gex-{a.strumento}.json").write_text(json.dumps(r, indent=1), encoding="utf-8")
    stampa(r)
    return 0


def lucchetto(strumento: str) -> Path:
    return cache_di(strumento).with_suffix(".in-corso")


def in_corso(strumento: str, validita: float = 10 * 60) -> bool:
    """Un calcolo della stessa catena e' gia' in corso (lucchetto piu' giovane di dieci minuti)."""
    l = lucchetto(strumento)
    return l.exists() and time.time() - l.stat().st_mtime < validita


def esegui(strumento: str, giorni: int = 70, strikes: int = 120, attesa: int = 8) -> dict:
    """Calcola, salva la cache e archivia la storia, un calcolo per catena alla volta: se un altro e'
    in corso si aspetta che finisca (al massimo dieci minuti) e si restituisce il suo risultato.
    La usano main() e studio/raccogli.py."""
    if in_corso(strumento):
        while in_corso(strumento):
            time.sleep(5)
        if cache_di(strumento).exists():
            return json.loads(cache_di(strumento).read_text(encoding="utf-8"))
    lucchetto(strumento).write_text(str(os.getpid()), encoding="utf-8")
    try:
        return _esegui(strumento, giorni, strikes, attesa)
    finally:
        lucchetto(strumento).unlink(missing_ok=True)


def _esegui(strumento: str, giorni: int, strikes: int, attesa: int) -> dict:
    cache = cache_di(strumento)
    base = client.discover(None)
    charts = client.get(base, "/charts")["charts"]
    chart = next((c["id"] for c in charts if c["instrument"] == strumento and c["timeFrame"] == "M30"),
                 next((c["id"] for c in charts if c["instrument"] == strumento), None))
    if not chart:
        raise RuntimeError(f"nessun chart di {strumento} registrato")
    inizio = time.time()
    r = riassunto(calcola(base, chart, giorni, strikes, attesa), strumento)
    r["secondi"] = round(time.time() - inizio, 1)
    r["prezzo_bridge"] = prezzo_m1(base, charts, strumento, r["future"])
    radice = next((k for k in sorted(MOLTIPLICATORE, key=len, reverse=True) if strumento.startswith(k)), "NQ")
    storia = Path(__file__).resolve().parents[2] / "quant" / "dati" / radice / "gex" / r["quando"][:10]
    r["oi_cambiato_rispetto_al_calcolo_precedente"] = confronta_oi(r, storia, strumento)
    cache.write_text(json.dumps(r), encoding="utf-8")
    # LA STORIA: ogni calcolo si archivia, perche' la cache si riscrive e la GEX passata non si
    # ricostruisce (l'open interest e i prezzi delle opzioni di ieri non tornano). Lo ha chiesto la
    # sintesi dello studio dei paper del 15 ottobre 2026: senza storia l'ipotesi B non si prova mai.
    # Sta accanto alla catena delle opzioni di ES, in quant/dati (fuori da git).
    quando = dt.datetime.fromisoformat(r["quando"])
    storia.mkdir(parents=True, exist_ok=True)
    (storia / f"{quando:%H%M}Z-{strumento}.json").write_text(json.dumps(r), encoding="utf-8")
    return r


def prezzo_m1(base: str, charts: list[dict], strumento: str, future: float | None) -> dict | None:
    """L'ultimo prezzo del chart M1 dello strumento, appena finito il calcolo: il prezzo che il giro e il
    pannello mostrano, per confrontarlo con il future del calcolo. Il 15 ottobre 2026 il future (29.975,5)
    non tornava con il bridge delle 18:43 (29.946,25): era l'ultimo prezzo di tre minuti dopo, quando e'
    stata letta l'ultima scadenza, ma il JSON lo datava all'inizio del calcolo."""
    m1 = next((c["id"] for c in charts if c["instrument"] == strumento and c["timeFrame"] == "M1"), None)
    if not m1:
        return None
    try:
        da = dt.datetime.now(dt.timezone.utc) - dt.timedelta(minutes=5)
        c = client.get(base, "/candles", {"chart": m1, "from": client.iso(da)}, timeout=30)["candles"][-1]
    except Exception as e:  # noqa: BLE001 - il confronto non deve far cadere il calcolo
        return {"errore": str(e)}
    return {"prezzo": c["close"], "quando": c.get("lastTime") or c["time"], "chart": "M1",
            "differenza": round(future - c["close"], 2) if future is not None else None}


def confronta_oi(r: dict, storia: Path, strumento: str) -> dict | None:
    """L'OI di oggi contro l'ultimo calcolo archiviato di oggi: l'OI "di ieri" non dovrebbe muoversi, e
    se si muove la catena e' arrivata a pezzi o la fonte l'ha aggiornato (lacuna della fase 3)."""
    vecchi = sorted(storia.glob(f"*Z-{strumento}.json")) if storia.is_dir() else []
    if not vecchi:
        return None
    try:
        p = json.loads(vecchi[-1].read_text(encoding="utf-8"))
    except (OSError, ValueError):
        return None
    oi_p = {s["giorno"]: s.get("oi", 0) for s in p.get("scadenze", [])}
    per_scad = [{"giorno": s["giorno"], "oi": s.get("oi", 0), "prima": oi_p.get(s["giorno"]),
                 "variazione": (s.get("oi", 0) - oi_p[s["giorno"]]) if s["giorno"] in oi_p else None}
                for s in r["scadenze"]]
    vecchi_k = {x["k"]: x["oi_c"] + x["oi_p"] for x in p.get("per_strike", [])}
    pesanti = sorted(r.get("per_strike", []), key=lambda x: -(x["oi_c"] + x["oi_p"]))[:5]
    comuni = [x for x in per_scad if x["prima"] is not None]
    tot, tot_p = sum(x["oi"] for x in comuni), sum(x["prima"] for x in comuni)
    return {"rispetto_a": p.get("quando"), "oi_totale": tot, "oi_totale_prima": tot_p,
            "variazione_pct": round(100 * (tot - tot_p) / tot_p, 2) if tot_p else None,
            "nota": "solo le scadenze presenti in tutti e due i calcoli; l'OI conta solo gli strike letti",
            "per_scadenza": per_scad,
            "strike_pesanti": [{"k": x["k"], "oi": x["oi_c"] + x["oi_p"], "prima": vecchi_k.get(x["k"])} for x in pesanti]}


if __name__ == "__main__":
    raise SystemExit(main())
