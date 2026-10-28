#!/usr/bin/env python3
"""Clona una finestra del workspace ATAS X su un altro strumento, tab per tab.

Perche': il 26 ottobre 2026 l'utente ha sistemato a mano la finestra ES (tre tab, con gli
indicatori delle opzioni) e ha chiesto la stessa finestra, uguale, anche per NQ. Il workspace
salvato aveva la sola finestra ES: questo script la copia cambiando i contratti.

Cosa fa, sul file del workspace e solo ad ATAS chiuso (ATAS risalva il workspace alla chiusura):

1. salva una copia accanto: "<nome> prima della finestra <etichetta>.aws";
2. copia la finestra --da con tutte le sue tab, gli indicatori e le loro impostazioni, i
   timeframe e i giorni caricati, cambiando solo il contratto con --mappa (id ATAS, ContractId);
   i disegni no, perche' su ATAS appartengono allo strumento;
3. mette la finestra nuova in posizione --in (0 = la prima), con dimensione e posizione prese da
   --geometria, un workspace che la finestra di quello strumento l'aveva gia'.

Uso (la finestra NQ dalla finestra ES, prima del 26 ottobre 2026):
    python FabioOrderFlow/tools/workspace_clona_finestra.py --aspetta
"""
from __future__ import annotations

import argparse
import copy
import json
import shutil
import sys
import time

from workspace_a_tab import CARTELLA, atas_aperto, disegni, indicatori, nuovo_logger, tipo
from workspace_finestra_es import cambia_contratto


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--nome", default="Fabio workspace", help="il workspace da cambiare sul posto")
    ap.add_argument("--da", type=int, default=0, help="la finestra da copiare")
    ap.add_argument("--in", dest="pos", type=int, default=0, help="dove mettere la finestra nuova")
    ap.add_argument("--mappa", nargs="+", default=["4936=5222", "34376=34387"],
                    help="contratti vecchio=nuovo per id ATAS: ES continuo -> NQ, ESZ6 -> NQZ6")
    ap.add_argument("--etichetta", default="NQ", help="il nome della copia di prima")
    ap.add_argument("--geometria", default="Fabio workspace prima della finestra ES",
                    help="il workspace da cui prendere dimensione e posizione della finestra nuova")
    ap.add_argument("--aspetta", action="store_true", help="aspetta che ATAS si chiuda")
    a = ap.parse_args()
    mappa = {int(x.split("=")[0]): int(x.split("=")[1]) for x in a.mappa}

    if atas_aperto() and not a.aspetta:
        print("ATAS e' aperto: risalverebbe il workspace alla chiusura e la modifica andrebbe persa. "
              "Chiudilo, o rilancia con --aspetta.", file=sys.stderr)
        return 2
    if atas_aperto():
        print("aspetto che ATAS si chiuda...", flush=True)
    while atas_aperto():
        time.sleep(3)

    p = CARTELLA / f"{a.nome}.aws"
    copia = CARTELLA / f"{a.nome} prima della finestra {a.etichetta}.aws"
    shutil.copy2(p, copia)
    w = json.loads(p.read_text(encoding="utf-8-sig"))

    nuova = copy.deepcopy(w["Windows"][a.da])
    for t in nuova["Tabs"]:
        item = t["Layout"]["Parameter"]["Item1"]
        if item.get("Type") != "chart":
            continue
        c = item["Parameter"]
        cambia_contratto(c, mappa)
        disegni(c, set())
        nuovo_logger(c)

    g = CARTELLA / f"{a.geometria}.aws"
    if g.exists():
        vecchia = json.loads(g.read_text(encoding="utf-8-sig"))["Windows"][0]
        for k in ("Width", "Height", "Position", "WindowState"):
            if k in vecchia:
                nuova[k] = copy.deepcopy(vecchia[k])
    w["Windows"].insert(a.pos, nuova)

    p.write_bytes(json.dumps(w, indent=2, ensure_ascii=False).replace("\n", "\r\n").encode("utf-8"))
    print(f"scritto {p}  (copia di prima: {copia.name})")
    for i, win in enumerate(w["Windows"]):
        print(f" finestra {i}")
        for t in win["Tabs"]:
            item = t["Layout"]["Parameter"]["Item1"]
            if item.get("Type") != "chart":
                continue
            c = item["Parameter"]
            print(f"  {t['Title']:<22} contratto {c['ContractId']:<6} "
                  f"{c['TimeFrame']['PeriodParameters'].get('VisualName'):<6} {[tipo(x) for _, x in indicatori(c)]}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
