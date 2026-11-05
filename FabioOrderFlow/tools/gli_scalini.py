#!/usr/bin/env python3
"""Gli scalini di mese, settimana e giorno, e il ping-pong (5 novembre 2026): il client.

Viene dal video sul bias dalla price action (docs/research/fonti-video/2026-11-05-il-bias-dalla-price-action.txt,
[V1 · 0:49-3:35]); come si legge: docs/research/metodo/il-bias-del-giorno.md. E' CONTESTO, non una
regola: che gli scalini allineati dicano qualcosa sulla cassa dopo e' stato provato in quant/studi/scala-dei-tre-tempi/
(5 novembre 2026, NQ 2017-2024, dentro campione): smentito, cinque varianti su cinque.

LA REGOLA NON STA QUI. Dal 5 novembre 2026 la calcola il server della pagina X-Ray, in un posto solo
(quant/strumenti/xray/XrayLive/Server/Scalini.cs, rotta /api/scalini?radice=NQ|ES): la stessa regola serve
anche i bordi del valore della pagina e delle quattro domande, e due copie divergerebbero. Questo file la
chiede al server e la mette in righe per il giro (tab 1); lo studio (studio/raccogli.py) scrive la risposta
in scalini-<strumento>.json cosi' com'e', con le chiavi di prima. XRAY_SERVER sceglie un altro server.

Le definizioni (il testo che comanda e' in Scalini.cs), sull'ULTIMA CANDELA CHIUSA contro quella prima:

    scalino su    massimo piu' alto, minimo piu' alto E chiusura sopra il massimo della candela prima
    scalino giu'  massimo piu' basso, minimo piu' basso E chiusura sotto il minimo della candela prima
    pausa         tutto il resto (anche un massimo piu' alto con la chiusura sotto il vecchio massimo)

Mese e settimana dalle barre giornaliere del chart Daily (il continuo di ATAS) per giorno di borsa CME
(dalle 17:00 di Chicago del giorno prima alle 16:00 di Chicago), con lo scarto del rollover del ruolo
della tab 1; il primo mese del chart, se ha meno di 15 giornate, si scarta. Il ping-pong: dopo l'ultimo
scalino del giornaliero, il massimo e il minimo delle giornate chiuse da allora; un muro e' rotto quando
una giornata chiude oltre, non quando lo tocca.

    python FabioOrderFlow/tools/gli_scalini.py              NQ (il Daily del continuo)
    python FabioOrderFlow/tools/gli_scalini.py --radice ES
"""
from __future__ import annotations

import argparse
import json
import os
import sys
import urllib.error
import urllib.request

SERVER = os.environ.get("XRAY_SERVER", "http://127.0.0.1:8790").rstrip("/")
SCALE = (("mese", "la corrente"), ("settimana", "la marea"), ("giorno", "l'onda"))


def dal_server(radice: str = "NQ", giorno: str | None = None, timeout: float = 10) -> dict:
    """La misura degli scalini di una radice (NQ, ES) da /api/scalini: scale, insieme, ping_pong,
    realizzata_20_giornate, ultima_chiusa, chart, barre. Con `giorno` (AAAA-MM-GG) passato: le giornate chiuse
    prima della sua cassa. Se il server non risponde o il bridge non ha il chart, solleva RuntimeError con il
    perche' (il giro lo stampa come "gli scalini non si leggono")."""
    radice = radice.upper()[:2]
    if radice not in ("NQ", "ES"):
        raise RuntimeError(f"il server misura gli scalini solo di NQ ed ES, non di {radice}")
    url = f"{SERVER}/api/scalini?radice={radice}" + (f"&giorno={giorno}" if giorno else "")
    try:
        with urllib.request.urlopen(url, timeout=timeout) as f:
            return json.load(f)
    except urllib.error.HTTPError as e:
        try:
            errore = json.load(e).get("errore")
        except Exception:
            errore = None
        raise RuntimeError(errore or f"il server su {SERVER} risponde {e.code} a /api/scalini") from None
    except (urllib.error.URLError, TimeoutError, OSError) as e:
        raise RuntimeError(f"server della pagina X-Ray non raggiungibile su {SERVER} ({getattr(e, 'reason', e)}): "
                           "gli scalini si calcolano li'") from None


def _n(x: float) -> str:
    s = f"{x:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
    return s[:-3] if s.endswith(",00") else s


def righe(m: dict) -> list[str]:
    """Le righe per il giro (tab 1): una per scala, il verdetto, il ping-pong."""
    out = []
    pezzi = []
    for nome, ruolo in SCALE:
        x = m["scale"].get(nome) or {}
        if not x.get("stato"):
            pezzi.append(f"{nome} -")
            continue
        pezzi.append(f"{nome} {x['stato']}")
    out.append(f"GLI SCALINI (ultima candela chiusa): {'  ·  '.join(pezzi)}  ->  {m.get('insieme')}")
    for nome, ruolo in SCALE:
        x = m["scale"].get(nome) or {}
        if not x.get("stato"):
            continue
        c, p = x["candela"], x["prima"]
        quando = c["al"] if nome == "giorno" else f"{c['dal']}..{c['al']}"
        ult = x.get("ultimo_scalino")
        da = f"; l'ultimo scalino: {ult['stato']} al {ult['al']}" if ult and x["stato"] == "pausa" else ""
        perche = f" ({x['perche'][0]})" if x.get("perche") else ""
        out.append(f"  {nome:9} {ruolo:11} {quando}: max {_n(c['high'])} min {_n(c['low'])} chiude {_n(c['close'])} "
                   f"contro max {_n(p['high'])} min {_n(p['low'])}{perche}{da}")
    pp = m.get("ping_pong") or {}
    if "muro_alto" in pp:
        out.append(f"  ping-pong dal {pp['dal']} ({pp['giornate']} giornate dopo uno {pp['dopo_lo_scalino']}): muro alto "
                   f"{_n(pp['muro_alto'])}, muro basso {_n(pp['muro_basso'])}; rotto solo se una giornata chiude oltre")
    elif pp.get("nota"):
        out.append(f"  ping-pong: {pp['nota']}")
    ag = m.get("aggiustato_al_rollover")
    if ag:
        out.append(f"  prezzi prima del {ag['prima_del']} aggiustati al rollover (+{_n(ag['scarto'])}); il ping-pong e' in prezzi di oggi")
    out.append("  (contesto dal video del 5/11. Provato su NQ 2017-2024 e smentito: gli scalini allineati non dicono il verso "
               "della cassa dopo, e i giorni misti non sono piu' stretti; quant/studi/scala-dei-tre-tempi. "
               "Si leggono come descrizione della struttura: docs/research/metodo/il-bias-del-giorno.md)")
    return out


if __name__ == "__main__":
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--radice", default="NQ")
    a = ap.parse_args()
    try:
        m = dal_server(a.radice)
    except RuntimeError as e:
        raise SystemExit(f"gli scalini di {a.radice} non si leggono: {e}")
    for r in righe(m):
        print(r)
