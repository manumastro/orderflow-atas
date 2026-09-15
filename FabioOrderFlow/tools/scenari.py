#!/usr/bin/env python3
"""Valuta gli scenari scritti per la seduta e, quando uno scatta, lo mette subito sul chart.

La differenza con un monitor generico e' tutta qui: **le condizioni non sono nel programma, sono
nel file della giornata**, e le scrive l'analisi ogni volta, guardando il contesto di quel giorno —
il posizionamento istituzionale, il profilo delle sedute passate, la struttura della mattina, gli
scenari che ci si aspetta. Il programma e' solo il motore che le valuta.

Quindi il segnale che compare a schermo **non e' generico**: e' gia' lo scenario che avevamo
previsto, con il nome che gli avevamo dato. La lettura ragionata viene dopo, con `annota.py`, ma
intanto sul chart c'e' scritto cosa e' successo.

Il file degli scenari sta in `docs/research/giornate/scenari-AAAA-MM-GG.json`:

    [
      {
        "nome": "B - il VAL cede",
        "quando": "chiude_sotto(29306) and vol >= p95vol and delta <= -p95delta and dpct30 <= -3",
        "prezzo": 29306,
        "tipo": "rottura",
        "testo": "scenario B: VAL perso con aggressione",
        "attesa": "primo gradino 29275, poi 29199"
      }
    ]

`quando` e' una espressione Python valutata su un contesto ricco, elencato da `--variabili`.
Nessun accesso a moduli o builtin: solo le variabili e le funzioni documentate.

    ./scenari.py --giorno 2026-09-15 --from 2026-09-15T07:30
    ./scenari.py --variabili          # cosa si puo' usare in `quando`
    ./scenari.py --giorno 2026-09-15 --prova    # valuta sulla storia, senza scrivere
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import subprocess
import sys
from pathlib import Path
import time

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
ANNOTA = HERE / "annota.py"
GIORNATE = HERE.parent.parent / "docs" / "research" / "giornate"

VARIABILI = """
La barra chiusa in esame
  o h l c            open, high, low, close
  vol delta          volume e delta della barra
  pos                dove chiude nel proprio range: 1 sul massimo, 0 sul minimo
  ora                "HH:MM" ora locale
  minuto             minuti assoluti, per confronti temporali

Soglie della seduta, ricalcolate a ogni giro
  p95vol p75vol      percentili del volume per barra
  p95delta           percentile del |delta| per barra

Finestre mobili (15, 30 e 60 minuti fino a questa barra)
  v15 v30 v60        volume cumulato
  d15 d30 d60        delta cumulato
  dpct15 dpct30 dpct60   delta in percentuale del volume: e' la misura che distingue
                         una rottura vera da uno sconfinamento

Struttura
  max3 min3          massimo e minimo delle ultime 3 barre
  max10 min10        delle ultime 10
  massimi_calanti(n) True se gli ultimi n massimi sono decrescenti
  minimi_crescenti(n)  lo specchio

Livelli, rispetto alla barra
  chiude_sotto(p)    la chiusura passa sotto p, quella precedente no
  chiude_sopra(p)    lo specchio
  tocca(p, d=8)      il minimo o il massimo arrivano entro d punti da p
  dist(p)            distanza con segno fra chiusura e p

Memoria dei test di un livello  --  il confronto piu' utile che si possa fare
  test_vol(p)        volume del passaggio in corso su p
  test_delta(p)      delta del passaggio in corso
  test_prec_vol(p)   volume del passaggio precedente sullo stesso livello, 0 se e' il primo
  test_prec_delta(p) delta del passaggio precedente

Accettazione dopo una rottura
  minuti_sotto(p, n=60)   quanti degli ultimi n minuti hanno chiuso sotto p
  vol_sopra_pct(p, n=60)  percentuale del volume scambiato sopra p negli ultimi n minuti

Orologio (ora locale del chart)
  dopo("15:30")      True da quell'ora in poi
  prima("16:00")     True fino a quell'ora
  fra("15:30","16:00")
  ivb_alto ivb_basso il range dei primi 30 minuti della cash NY, None finche' non e' chiuso
"""


# ------------------------------------------------------------------ dati

def candele(args) -> list[dict]:
    cmd = [sys.executable, str(BRIDGE), "candles",
           "--from", args.begin, "--to", args.end, "--out", args.cache]
    if args.chart:
        cmd += ["--chart", args.chart]
    subprocess.run(cmd, check=True, timeout=90,
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    return json.load(open(args.cache))["candles"]


def minuti(b) -> int:
    t = b["time"]
    return int(t[8:10]) * 1440 + int(t[11:13]) * 60 + int(t[14:16])


def hhmm(b, fuso) -> str:
    return f"{(int(b['time'][11:13]) + fuso) % 24:02d}:{b['time'][14:16]}"


# ------------------------------------------------------------------ contesto

def costruisci_contesto(storia, i, fuso, ivb):
    """Tutte le variabili disponibili a `quando`, calcolate sulla barra i di `storia`."""
    b = storia[i]
    prec = storia[i - 1] if i else b
    fino = storia[:i + 1]

    def fin(n):
        w = fino[-n:]
        v = sum(x["volume"] for x in w)
        d = sum(x["delta"] for x in w)
        return v, d, (d / v * 100 if v else 0.0)

    v15, d15, p15 = fin(15)
    v30, d30, p30 = fin(30)
    v60, d60, p60 = fin(60)

    vv = sorted(x["volume"] for x in fino)
    dd = sorted(abs(x["delta"]) for x in fino)
    q = lambda a, p: a[min(int(len(a) * p), len(a) - 1)] if a else 0

    rng = max(b["high"] - b["low"], 0.25)

    def passaggi(p, tol=8.0):
        """Spezza la storia nei passaggi distinti sul livello p, separati da 10 minuti di assenza."""
        blocchi, cur, ultimo = [], [], None
        for x in fino:
            if x["low"] - tol <= p <= x["high"] + tol:
                if ultimo is not None and minuti(x) - ultimo > 10:
                    blocchi.append(cur); cur = []
                cur.append(x); ultimo = minuti(x)
        if cur:
            blocchi.append(cur)
        return blocchi

    def _tot(bl, campo):
        return sum(x[campo] for x in bl) if bl else 0

    def massimi_calanti(n=3):
        w = fino[-n:]
        return len(w) == n and all(w[k]["high"] < w[k - 1]["high"] for k in range(1, n))

    def minimi_crescenti(n=3):
        w = fino[-n:]
        return len(w) == n and all(w[k]["low"] > w[k - 1]["low"] for k in range(1, n))

    def minuti_sotto(p, n=60):
        return sum(1 for x in fino[-n:] if x["close"] < p)

    def vol_sopra_pct(p, n=60):
        w = fino[-n:]
        tot = sum(x["volume"] for x in w)
        return (sum(x["volume"] for x in w if x["close"] > p) / tot * 100) if tot else 0.0

    adesso = hhmm(b, fuso)
    return {
        "o": b["open"], "h": b["high"], "l": b["low"], "c": b["close"],
        "vol": b["volume"], "delta": b["delta"], "pos": (b["close"] - b["low"]) / rng,
        "ora": adesso, "minuto": minuti(b),
        "p95vol": q(vv, .95), "p75vol": q(vv, .75), "p95delta": q(dd, .95),
        "v15": v15, "v30": v30, "v60": v60,
        "d15": d15, "d30": d30, "d60": d60,
        "dpct15": p15, "dpct30": p30, "dpct60": p60,
        "max3": max(x["high"] for x in fino[-3:]), "min3": min(x["low"] for x in fino[-3:]),
        "max10": max(x["high"] for x in fino[-10:]), "min10": min(x["low"] for x in fino[-10:]),
        "massimi_calanti": massimi_calanti, "minimi_crescenti": minimi_crescenti,
        "chiude_sotto": lambda p: b["close"] < p <= prec["close"],
        "chiude_sopra": lambda p: b["close"] > p >= prec["close"],
        "tocca": lambda p, d=8.0: b["low"] - d <= p <= b["high"] + d,
        "dist": lambda p: b["close"] - p,
        "test_vol": lambda p: _tot(passaggi(p)[-1] if passaggi(p) else [], "volume"),
        "test_delta": lambda p: _tot(passaggi(p)[-1] if passaggi(p) else [], "delta"),
        "test_prec_vol": lambda p: _tot(passaggi(p)[-2] if len(passaggi(p)) > 1 else [], "volume"),
        "test_prec_delta": lambda p: _tot(passaggi(p)[-2] if len(passaggi(p)) > 1 else [], "delta"),
        "minuti_sotto": minuti_sotto, "vol_sopra_pct": vol_sopra_pct,
        "dopo": lambda t: adesso >= t, "prima": lambda t: adesso <= t,
        "fra": lambda a, z: a <= adesso <= z,
        "ivb_alto": ivb.get("alto"), "ivb_basso": ivb.get("basso"),
    }


def calcola_ivb(storia, fuso, inizio="15:30", fine="16:00"):
    w = [x for x in storia if inizio <= hhmm(x, fuso) < fine]
    if not w or hhmm(storia[-1], fuso) < fine:
        return {}
    return {"alto": max(x["high"] for x in w), "basso": min(x["low"] for x in w)}


# ------------------------------------------------------------------ motore

def chiave(s) -> tuple[str, str]:
    """Nome piu' condizione. Cosi' correggere un `quando` riarma lo scenario, che e' quello che
    serve mentre si aggiusta un file durante la seduta; rinominarlo pure."""
    return (s["nome"], s["quando"])


def valuta(scenari, ctx, scattati):
    for s in scenari:
        nome = s["nome"]
        if chiave(s) in scattati and s.get("una_volta", True):
            continue
        try:
            if eval(s["quando"], {"__builtins__": {}}, ctx):   # noqa: S307 - espressione dell'analisi
                yield s
        except Exception as e:
            yield {"nome": nome, "quando": s["quando"], "errore": f"{type(e).__name__}: {e}"}


def ricomponi(giorno, chart):
    """Rimanda sul chart livelli + scenari in attesa + annotazioni, senza aggiungere niente."""
    cmd = [sys.executable, str(ANNOTA), "--giorno", giorno, "--ricomponi"]
    if chart:
        cmd += ["--chart", chart]
    try:
        subprocess.run(cmd, check=True, timeout=30, stdout=subprocess.DEVNULL)
    except Exception as e:
        print(f"[attese non spinte sul chart: {type(e).__name__}]", flush=True)


def gia_scattati(giorno: str) -> set[tuple[str, str]]:
    """Le chiavi degli scenari che hanno gia' annotato oggi, lette dal diario."""
    f = GIORNATE / f"annotazioni-{giorno}.json"
    if not f.exists():
        return set()
    try:
        return {(a["scenario"], a.get("condizione", ""))
                for a in json.loads(f.read_text()) if a.get("scenario")}
    except Exception:
        return set()


def annota(s, ctx, giorno, chart):
    cmd = [sys.executable, str(ANNOTA), "--giorno", giorno,
           "--ora", ctx["ora"], "--prezzo", str(s.get("prezzo", ctx["c"])),
           "--tipo", s.get("tipo", "nota"), "--testo", s.get("testo", s["nome"]),
           "--scenario", s["nome"], "--condizione", s["quando"]]
    if s.get("tema"):
        cmd += ["--tema", s["tema"]]
    misura = s.get("attesa", "")
    fatto = (f"{ctx['ora']} a {ctx['c']:.2f}: vol {ctx['vol']}, delta {ctx['delta']:+}, "
             f"30m {ctx['dpct30']:+.1f}%")
    cmd += ["--misura", f"{fatto}. {misura}".strip()]
    if chart:
        cmd += ["--chart", chart]
    try:
        subprocess.run(cmd, check=True, timeout=30, stdout=subprocess.DEVNULL)
    except Exception as e:
        print(f"[annotazione non spinta: {type(e).__name__}]", flush=True)


# ------------------------------------------------------------------ main

def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--variabili", action="store_true", help="elenca cosa si puo' usare in `quando`")
    ap.add_argument("--giorno", default=dt.date.today().isoformat())
    ap.add_argument("--from", dest="begin", help="inizio della finestra chiesta al bridge")
    ap.add_argument("--to", dest="end", default="2100-01-01T00:00")
    ap.add_argument("--chart")
    ap.add_argument("--prova", action="store_true", help="valuta su tutta la storia senza scrivere niente")
    ap.add_argument("--intervallo", type=int, default=20)
    ap.add_argument("--fuso", type=int, default=2)
    ap.add_argument("--cache", default="/tmp/fof-scenari.json")
    args = ap.parse_args()

    if args.variabili:
        print(VARIABILI)
        return
    if not args.begin:
        sys.exit("serve --from")

    f = GIORNATE / f"scenari-{args.giorno}.json"
    if not f.exists():
        sys.exit(f"manca {f}: gli scenari li scrive l'analisi, non il programma")

    def ricarica(vecchi):
        """Rilegge il file se e' cambiato e racconta cosa e' cambiato.

        Durante una seduta gli scenari si riscrivono spesso: aspettarsi un riavvio del monitor
        a ogni correzione e' il modo migliore per non correggerli. Un file JSON rotto a meta'
        salvataggio non deve fermare la sorveglianza: si tiene la versione precedente.
        """
        try:
            nuovi = json.loads(f.read_text())
        except Exception as e:
            print(f"[scenari illeggibili, tengo i precedenti: {type(e).__name__}]", flush=True)
            return vecchi
        if vecchi is None:
            return nuovi
        pv, pn = {s["nome"] for s in vecchi}, {s["nome"] for s in nuovi}
        vv = {s["nome"]: s["quando"] for s in vecchi}
        cambi = ([f"+{n}" for n in pn - pv] + [f"-{n}" for n in pv - pn]
                 + [f"~{s['nome']}" for s in nuovi if s["nome"] in pv and s["quando"] != vv[s["nome"]]])
        if cambi:
            print(f"[scenari ricaricati] {len(nuovi)} attivi: " + ", ".join(cambi), flush=True)
            ricomponi(args.giorno, args.chart)
        return nuovi

    scenari = ricarica(None)
    visto_mtime = f.stat().st_mtime
    # Gli scenari gia' scattati si rileggono dal diario, non dalla memoria: il motore viene
    # riavviato spesso — cambiare il codice o gli scenari durante una seduta e' la norma — e una
    # condizione ancora vera al riavvio riscriverebbe un doppione sul chart. La chiave e' nome
    # piu' condizione, cosi' correggere un `quando` continua a riarmare lo scenario apposta.
    scattati: set[tuple[str, str]] = gia_scattati(args.giorno)
    if scattati:
        print(f"[dal diario] {len(scattati)} scenari gia' scattati oggi, non si ripetono", flush=True)
    frontiera = None

    while True:
        if not args.prova:
            m = f.stat().st_mtime if f.exists() else visto_mtime
            if m != visto_mtime:
                visto_mtime = m
                scenari = ricarica(scenari)
        try:
            c = candele(args)
        except Exception as e:
            print(f"[bridge non raggiungibile: {type(e).__name__}]", flush=True)
            time.sleep(30)
            continue

        chiuse = c[:-1]                      # l'ultima barra e' ancora in formazione
        if not chiuse:
            time.sleep(args.intervallo)
            continue
        ivb = calcola_ivb(chiuse, args.fuso)

        if args.prova:
            for i in range(len(chiuse)):
                ctx = costruisci_contesto(chiuse, i, args.fuso, ivb)
                for s in valuta(scenari, ctx, scattati):
                    if "errore" in s:
                        print(f"  ERRORE  {s['nome']}: {s['errore']}"); scattati.add(chiave(s)); continue
                    scattati.add(chiave(s))
                    print(f"SCATTA  {s['nome']}  |  {ctx['ora']} {ctx['c']:.2f} "
                          f"vol {ctx['vol']} delta {ctx['delta']:+} 30m {ctx['dpct30']:+.1f}%")
            print(f"-- {len(scattati)} scenari su {len(scenari)}, {len(chiuse)} barre")
            return

        if frontiera is None:
            frontiera = minuti(chiuse[-1])
            ricomponi(args.giorno, args.chart)
            print(f"[scenari attivi] {len(scenari)}: " + " | ".join(s["nome"] for s in scenari)
                  + f" -- ultima barra {hhmm(chiuse[-1], args.fuso)} a {chiuse[-1]['close']:.2f}",
                  flush=True)
        else:
            for i in range(len(chiuse)):
                if minuti(chiuse[i]) <= frontiera:
                    continue
                frontiera = minuti(chiuse[i])
                ctx = costruisci_contesto(chiuse, i, args.fuso, ivb)
                for s in valuta(scenari, ctx, scattati):
                    if "errore" in s:
                        print(f"ERRORE nello scenario {s['nome']}: {s['errore']}", flush=True)
                        scattati.add(chiave(s))
                        continue
                    scattati.add(chiave(s))
                    print(f"SCATTA {s['nome']} | {ctx['ora']} {ctx['c']:.2f} "
                          f"vol {ctx['vol']} delta {ctx['delta']:+} 30m {ctx['dpct30']:+.1f}% "
                          f"| atteso: {s.get('attesa', '-')}", flush=True)
                    annota(s, ctx, args.giorno, args.chart)
        time.sleep(args.intervallo)


if __name__ == "__main__":
    main()
