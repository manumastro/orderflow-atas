#!/usr/bin/env python3
"""Stampa in un colpo solo tutto il contesto disponibile prima di una analisi.

Esiste per una ragione precisa: la regola "guarda sempre il contesto completo" non sopravvive se
costa sei comandi. Ne costa uno.

    ./giro_orizzonte.py                 # oggi
    ./giro_orizzonte.py --giorno 2026-09-15

Otto sezioni, nell'ordine in cui servono:

    1. lo stato del bridge e il contratto        6. gli scenari armati e il loro verso
    2. dove eravamo, dal file della giornata     7. il tape recente con le finestre mobili
    3. le correzioni gia' fatte oggi             8. il profilo della seduta in corso
    4. il framing del giorno precedente
    5. cosa e' gia' scattato, dal diario

Non conclude niente e non chiama il metodo: mette davanti agli occhi cio' che c'e', perche' la
conclusione si scriva guardando tutto e non le ultime dieci barre.
"""
from __future__ import annotations

import argparse
import collections
import datetime as dt
import json
import re
import subprocess
import sys
from pathlib import Path

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
GIORNATE = HERE.parent.parent / "docs" / "research" / "giornate"
COT = HERE.parent.parent / "docs" / "research" / "cot"


# Il chart da interrogare, quando ATAS ne ha registrato piu' di uno. Lo riempie main() da
# --chart: senza, il bridge rifiuta ogni richiesta con "2 charts are registered" e il giro
# d'orizzonte dichiara il bridge irraggiungibile pur essendo acceso (16 settembre, 12:27).
CHART: list[str] = []


def bridge(*args) -> dict | None:
    try:
        out = subprocess.run([sys.executable, str(BRIDGE), *args, *CHART],
                             capture_output=True, text=True, timeout=30)
        return json.loads(out.stdout) if out.returncode == 0 else None
    except Exception:
        return None


def titolo(n: int, testo: str) -> None:
    print(f"\n{'─' * 78}\n{n}. {testo}\n{'─' * 78}")


def coda_sezione(testo: str, titoli: list[str], righe: int = 40) -> str:
    """Estrae l'ultima sezione il cui titolo contiene una delle chiavi."""
    linee = testo.splitlines()
    inizio = None
    for i, r in enumerate(linee):
        if r.startswith("#") and any(k.lower() in r.lower() for k in titoli):
            inizio = i
    if inizio is None:
        return ""
    fuori = []
    for r in linee[inizio:inizio + righe]:
        if fuori and r.startswith("## "):
            break
        fuori.append(r)
    return "\n".join(fuori).rstrip()


def main() -> int:
    p = argparse.ArgumentParser(description=__doc__,
                                formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--giorno", default=dt.date.today().isoformat())
    p.add_argument("--barre", type=int, default=15, help="quante barre di tape mostrare")
    p.add_argument("--chart", help="id o strumento, se ATAS ha piu' di un chart registrato")
    a = p.parse_args()
    if a.chart:
        CHART[:] = ["--chart", a.chart]
    g = a.giorno

    # `--giorno` e' una CHIAVE, non una data: puo' portare il prefisso dello strumento
    # (`ESZ6-2026-09-16`), come vuole il passo 8 di come-si-apre-un-asset.md. Qui serve anche la
    # data nuda, per calcolare "ieri" e per il file del giorno precedente: si separano le due cose
    # invece di assumere che coincidano. Il 16 settembre l'assunzione ha fatto fallire l'hook al
    # primo strumento col prefisso.
    prefisso, _, data_nuda = g.rpartition("-20")
    data_nuda = ("20" + data_nuda) if prefisso else g
    prefisso = (prefisso + "-") if prefisso else ""

    # 1 --------------------------------------------------------------- il bridge
    titolo(1, "IL BRIDGE E IL CONTRATTO")
    h = bridge("health")
    if not h:
        print("  bridge non raggiungibile — ogni misura che segue e' vecchia o assente")
    else:
        print(f"  {h['instrument']} {h['timeFrame']}  {h['bars']:,} barre  "
              f"ora di mercato {h['marketTimeUtc'][11:16]}Z")
        r = bridge("rollovers")
        if r and r.get("rollovers"):
            print(f"  rollover noti: {r['rollovers']}")

    # 2-3 ------------------------------------------------------- la giornata di oggi
    oggi = GIORNATE / f"{g}.md"
    titolo(2, f"DOVE ERAVAMO — {oggi.name}")
    if not oggi.exists():
        print(f"  {oggi.name} non esiste: la giornata non e' stata aperta")
    else:
        t = oggi.read_text()
        m = re.search(r"^\*\*Stato\*\*.*$|^Stato:.*$", t, re.M)
        if m:
            print(f"  {m.group(0)}")
        s = coda_sezione(t, ["dove eravamo"], 30)
        print(s if s else "  nessuna sezione 'Dove eravamo'")

        titolo(3, "LE CORREZIONI GIA' FATTE OGGI")
        c = coda_sezione(t, ["correzioni"], 200)
        if not c or "Nessuna ancora" in c:
            print("  nessuna")
        else:
            for r in c.splitlines():
                if r.startswith("### "):
                    print(f"  {r[4:]}")

    # 4 ----------------------------------------------------------- il giorno prima
    # Solo le giornate DELLO STESSO strumento: senza prefisso il glob "2*.md" prendeva anche i
    # file di un altro asset, e il framing di ieri sarebbe stato quello del mercato sbagliato.
    prec = sorted(x for x in GIORNATE.glob(f"{prefisso or '2'}*.md") if x.stem < g)
    titolo(4, f"IL FRAMING DI IERI — {prec[-1].name if prec else 'assente'}")
    if prec:
        t = prec[-1].read_text()
        chiavi = ("value area", "poc", "val ", "vah ", "minimo", "massimo", "chiusura", "delta")
        visto = 0
        for r in t.splitlines():
            if r.startswith("## 2") or r.startswith("## 3"):
                break                      # solo la sezione 1, il framing
            if r.startswith("|") and any(k in r.lower() for k in chiavi):
                print(f"  {r[:110]}")
                visto += 1
            if visto >= 14:
                break
        if not visto:
            print("  nessuna riga di framing riconosciuta: aprire il file a mano")

    # 5 ------------------------------------------------------------ cosa e' scattato
    titolo(5, "COSA E' GIA' SCATTATO OGGI (dal diario, non dedotto)")
    ann = GIORNATE / f"annotazioni-{g}.json"
    if not ann.exists():
        print("  nessuna annotazione")
    else:
        v = json.load(ann.open())
        v = v if isinstance(v, list) else v.get("annotazioni", [])
        if not v:
            print("  nessuna annotazione")
        for x in v:
            stato = "superata" if x.get("superata_da") else "ATTIVA"
            print(f"  {x.get('ora')}  [{stato}]  {x.get('scenario') or x.get('tipo')}  "
                  f"{x.get('testo','')[:60]}")

    # 6 -------------------------------------------------------------- gli scenari
    titolo(6, "GLI SCENARI ARMATI")
    sc = GIORNATE / f"scenari-{g}.json"
    if not sc.exists():
        print("  nessuno scenario per oggi")
    else:
        for s in json.load(sc.open()):
            print(f"  {s.get('prezzo'):>10}  {s['sigla']:<26} [{s.get('verso','-')}]")

    # 7 ------------------------------------------------------------------- il tape
    titolo(7, "IL TAPE RECENTE")
    # la finestra e' la seduta globex in corso: dalle 22:00 CEST di ieri, cioe' 20:00Z
    ieri = (dt.date.fromisoformat(data_nuda) - dt.timedelta(days=1)).isoformat()
    d = bridge("candles", "--from", f"{ieri}T20:00")
    cs = (d or {}).get("candles") or []
    if not cs:
        print("  nessuna barra")
    else:
        # L'ULTIMA BARRA NON E' CHIUSA. Leggerla come chiusa ha gia' prodotto due letture
        # sbagliate il 16 settembre: si marca, e le finestre mobili la escludono.
        for j, c in enumerate(cs[-a.barre:]):
            rng = c["high"] - c["low"]
            pos = (c["close"] - c["low"]) / rng if rng else 0
            viva = "  <-- IN FORMAZIONE, non e' una chiusura" if c is cs[-1] else ""
            print(f"  {c['time'][11:16]}Z  H{c['high']:9.2f} L{c['low']:9.2f} C{c['close']:9.2f}"
                  f"  v{c['volume']:6,.0f} d{c.get('delta',0):+6,.0f} pos{pos:.2f}{viva}")
        cs_chiuse = cs[:-1] or cs
        for n in (15, 30, 60):
            w = cs_chiuse[-n:]
            if len(w) < n:
                continue
            tot = sum(x["volume"] for x in w)
            dd = sum(x.get("delta", 0) for x in w)
            print(f"  {n}m: vol {tot:>7,} delta {dd:>+7,} ({dd/max(tot,1)*100:+5.1f}%)  "
                  f"range {min(x['low'] for x in w):.2f}-{max(x['high'] for x in w):.2f}"
                  f"   (solo barre chiuse)")

    # 8 ------------------------------------------------------------------ il profilo
    titolo(8, "IL PROFILO DELLA FINESTRA DISPONIBILE")
    if cs:
        # L'AMPIEZZA DELLA FASCIA E' UN PARAMETRO, NON UNA COSTANTE. Era fissa a 25 punti, che
        # su NQ (range di seduta ~300 punti) da' una dozzina di fasce leggibili e su ESZ6 (range
        # ~50) ne da' DUE: il profilo smette di dire dove sta il volume. Si ricava dal range della
        # finestra puntando a ~15 fasce, e si arrotonda a un taglio "umano" (1, 2, 5, 10, 25...).
        estensione = max(c["high"] for c in cs) - min(c["low"] for c in cs)
        grezza = max(estensione / 15.0, 1e-9)
        tagli = [0.05, 0.1, 0.25, 0.5, 1, 2, 5, 10, 25, 50, 100, 250]
        passo = min((t for t in tagli if t >= grezza), default=tagli[-1])
        b = collections.Counter()
        dl = collections.Counter()
        for c in cs:
            k = ((c["high"] + c["low"]) / 2) // passo * passo
            k = round(k, 4)
            b[k] += c["volume"]
            dl[k] += c.get("delta", 0)
        tot = sum(b.values())
        print(f"  {len(cs)} barre, {cs[0]['time'][5:16].replace('T',' ')}Z -> "
              f"{cs[-1]['time'][11:16]}Z, {tot:,} lotti")
        poc = max(b, key=b.get)
        for k in sorted(b, reverse=True):
            q = b[k] / tot * 100
            if q < 1.0:
                continue
            m = " POC" if k == poc else ""
            et = f"{k:g}-{k+passo:g}"
            print(f"  {et:<15} {b[k]:8,} {q:5.1f}%  delta {dl[k]:+7,}  "
                  f"{'#' * int(q / 1.5)}{m}")
        print(f"  (fascia {passo:g}, fasce sotto l'1% omesse; POC {poc:g}-{poc+passo:g} con "
              f"{b[poc]/tot*100:.1f}% e delta {dl[poc]:+,})")

    print("\n" + "─" * 78)
    print("Questo e' il contesto. La conclusione si scrive guardando tutto, non le ultime barre.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
