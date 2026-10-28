#!/usr/bin/env python3
"""Le quattro tab di NQ nel giro d'orizzonte, dalla piu' lontana alla piu' vicina (15 ottobre 2026).

Il workspace ha quattro tab, e ognuna ha un ruolo che l'indicatore dichiara (`/ruolo`):

    1 COMPOSITO   NQ Daily    il valore di 90 sedute, e le ultime giornate intere
    2 SETTIMANA   NQZ6 M30    il valore di cassa di ogni giorno, e le ultime ore a mezz'ora
    3 SEDUTA      NQZ6 M1     le sedute di cassa, la struttura oraria, il tape (il resto del giro)
    4 ESECUZIONE  NQZ6 Range  le ultime barre da 40 tick: verso, durata, delta

Il giro guardava solo l'M1. Qui ogni tab si legge **dal suo chart**, non ricostruita dai minuti:
e' quello che l'utente ha davanti. La tab 2 porta anche la GEX (gex.py), perche' e' li' che sta
il GEX Profile di ATAS.
"""
from __future__ import annotations

import datetime as dt
import json
import os
import subprocess
import sys
import time
from pathlib import Path

import bridge as client
import gex
import ora

GIORNI = ["lun", "mar", "mer", "gio", "ven", "sab", "dom"]
QUI = Path(__file__).resolve().parent


def n(x: float | None, dec: int = 2) -> str:
    if x is None:
        return "-"
    s = f"{x:,.{dec}f}".replace(",", "X").replace(".", ",").replace("X", ".")
    return s[:-3] if dec == 2 and s.endswith(",00") else s


def k(x: float, segno: bool = True) -> str:
    """Un intero con il punto delle migliaia, come si legge in italiano."""
    return (f"{x:+,.0f}" if segno else f"{x:,.0f}").replace(",", ".")


def titolo(testo: str) -> None:
    print(f"\n== {testo}")


def giorno(iso: str) -> str:
    d = dt.date.fromisoformat(iso[:10])
    return f"{GIORNI[d.weekday()]} {d.day:02d}"


def le_tab(charts: list[dict], radice: str = "NQ") -> dict[str, dict]:
    """Il chart di ogni tab: Daily del continuo, M30, M1 e Range del contratto."""
    nq = [c for c in charts if str(c.get("instrument", "")).upper().startswith(radice)]
    trova = lambda f: next((c for c in nq if f(c)), None)  # noqa: E731
    return {"composito": trova(lambda c: c["timeFrame"] == "Daily"),
            "settimana": trova(lambda c: c["timeFrame"] == "M30"),
            "seduta": trova(lambda c: c["timeFrame"] == "M1"),
            "esecuzione": trova(lambda c: c.get("chartType") == "Range")}


def ultime(base: str, c: dict, quante: int) -> list[dict]:
    da = max(0, int(c["bars"]) - quante)
    return client.get(base, "/candles", {"chart": c["id"], "fromBar": da}, timeout=30).get("candles") or []


def tab_composito(base: str, c: dict) -> None:
    titolo(f"TAB 1 · COMPOSITO — {c['instrument']} Daily (continuo, barre di 24 ore)")
    r = client.get(base, "/ruolo", {"chart": c["id"]}, timeout=30).get("dati") or {}
    if r:
        print(f"  composito di {r.get('barre')} giornate dal {r.get('dal', '')[:10]}: valore {n(r.get('val'))}-"
              f"{n(r.get('vah'))}  POC {n(r.get('poc'))}  prezzo {n(r.get('prezzo'))} -> {r.get('zona')}")
        ag = r.get("aggiustato")
        if ag:
            print(f"  aggiustato al rollover (+{n(ag.get('scarto'))} prima del {ag.get('finoA')}): valore "
                  f"{n(ag.get('val'))}-{n(ag.get('vah'))}  POC {n(ag.get('poc'))} -> {ag.get('zona')}")
    barre = ultime(base, c, 5)
    for b in barre:
        viva = "  in corso" if b is barre[-1] else ""
        print(f"  {giorno(b['lastTime'])}  {n(b['open'])} -> {n(b['close'])}  range {b['high'] - b['low']:.0f}  "
              f"delta {k(b['delta'])}  vol {b['volume'] / 1000:.0f}k{viva}")


def tab_settimana(base: str, c: dict) -> None:
    titolo(f"TAB 2 · SETTIMANA — {c['instrument']} M30")
    dati = client.get(base, "/ruolo", {"chart": c["id"]}, timeout=30).get("dati") or []
    print("  il valore di cassa di ogni giorno, dal ruolo della tab (POC SU/GIU sul giorno prima):")
    prec = None
    for d in dati[-5:]:
        verso = ""
        if prec:
            verso = "SU" if d["poc"] > prec["poc"] else "GIU"
            if d["val"] > prec["vah"] or d["vah"] < prec["val"]:
                verso += " staccato"
        print(f"    {giorno(d['giorno'])}  {n(d['val'])}-{n(d['vah'])}  POC {n(d['poc'])}  {verso}"
              f"{'  in corso' if d.get('inCorso') else ''}")
        prec = d
    barre = ultime(base, c, 4)
    print("  le ultime due ore a mezz'ora (ora italiana; l'ultima e' aperta):")
    for b in barre:
        print(f"    {ora.hhmm(b['time'])}  {n(b['open'])} -> {n(b['close'])}  range {b['high'] - b['low']:.0f}  "
              f"delta {k(b['delta'])}  vol {k(b['volume'], False)}")


def tab_opzioni(strumento: str, prezzo: float | None) -> None:
    """La GEX dalla cache di gex.py; se e' vecchia, la ricalcola in background (due minuti e mezzo)."""
    titolo(f"TAB 2 · LE OPZIONI — GEX di {strumento} (gex.py): descrizione, non regola")
    cache = gex.cache_di(strumento)
    eta = time.time() - cache.stat().st_mtime if cache.exists() else None
    # Il lucchetto lo tiene gex.esegui; qui si guarda solo. Con GEX_SENZA_RICALCOLO (lo imposta
    # studio/raccogli.py, che la GEX la calcola da se') non si lancia niente: il 15 ottobre 2026 due
    # catene di NQ insieme sullo stesso chart hanno superato il tetto di sottoscrizioni del bridge.
    if (eta is None or eta > 30 * 60) and not os.environ.get("GEX_SENZA_RICALCOLO"):
        if not gex.in_corso(strumento):
            subprocess.Popen([sys.executable, str(QUI / "gex.py"), "--strumento", strumento],
                             stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL,
                             creationflags=getattr(subprocess, "CREATE_NO_WINDOW", 0))
        print("  la GEX si sta ricalcolando in background (circa tre minuti): "
              + ("questa e' la precedente" if cache.exists() else "al prossimo giro"))
    if cache.exists():
        gex.stampa(json.loads(cache.read_text(encoding="utf-8")), prezzo=prezzo)


def tab_esecuzione(base: str, c: dict) -> None:
    titolo(f"TAB 4 · ESECUZIONE — {c['instrument']} Range {c['timeFrame']} tick")
    barre = ultime(base, c, 31)
    if not barre:
        print("  nessuna barra")
        return
    chiuse = barre[:-1]
    for b in barre[-4:]:
        durata = (dt.datetime.fromisoformat(b["lastTime"].replace("Z", "+00:00"))
                  - dt.datetime.fromisoformat(b["time"].replace("Z", "+00:00"))).total_seconds()
        verso = "su " if b["close"] > b["open"] else "giu" if b["close"] < b["open"] else "=  "
        viva = "  <-- aperta" if b is barre[-1] else ""
        print(f"  {ora.hhmmss(b['time'])}  {verso} {n(b['open'])} -> {n(b['close'])}  {durata:4.0f}s  "
              f"vol {k(b['volume'], False):>5}  delta {k(b['delta']):>5}{viva}")
    if len(chiuse) >= 10:
        su = sum(1 for b in chiuse if b["close"] > b["open"])
        dd = sum(b["delta"] for b in chiuse)
        vv = sum(b["volume"] for b in chiuse)
        sec = (dt.datetime.fromisoformat(chiuse[-1]["lastTime"].replace("Z", "+00:00"))
               - dt.datetime.fromisoformat(chiuse[0]["time"].replace("Z", "+00:00"))).total_seconds()
        print(f"  ultime {len(chiuse)} chiuse: {su} su, {len(chiuse) - su} giu, delta {k(dd)} ({dd / max(vv, 1) * 100:+.1f}%), "
              f"{n(min(b['low'] for b in chiuse))}-{n(max(b['high'] for b in chiuse))} "
              f"in {sec / 60:.0f} minuti")


def stampa(parte: str, radice: str = "NQ") -> None:
    """parte: 'lontane' (tab 1, 2 e opzioni) o 'esecuzione' (tab 4); radice: NQ o ES."""
    try:
        base = client.discover(None)
        tab = le_tab(client.get(base, "/charts", timeout=10)["charts"], radice)
        if parte == "lontane":
            if tab["composito"]:
                tab_composito(base, tab["composito"])
            else:
                print(f"  TAB 1 (Daily di {radice}) non registrata nel bridge")
            if tab["settimana"]:
                tab_settimana(base, tab["settimana"])
                prezzo = (ultime(base, tab["settimana"], 1) or [{}])[-1].get("close")
                tab_opzioni(tab["settimana"]["instrument"], prezzo)
            else:
                print("  TAB 2 (M30) non registrata nel bridge")
        elif tab["esecuzione"]:
            tab_esecuzione(base, tab["esecuzione"])
        else:
            print("  TAB 4 (Range) non registrata nel bridge")
    except SystemExit as e:
        print(f"  le tab non si leggono: {e}")
    except Exception as e:  # il giro non deve morire per una tab
        print(f"  le tab non si leggono: {type(e).__name__}: {e}")


if __name__ == "__main__":
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    stampa("lontane")
    stampa("esecuzione")
