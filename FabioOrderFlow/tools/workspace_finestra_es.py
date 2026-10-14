#!/usr/bin/env python3
"""Aggiunge al workspace ATAS X una finestra ES con le stesse quattro tab di NQ.

Perche' ES: gli indicatori Options X-Ray di ATAS usano le opzioni sull'S&P 500 (SPX) e disegnano
solo su ES, MES, SPY e US 500; su NQ non disegnano niente (quant/metodo/le-opzioni-in-atas.md).
Lo studio delle opzioni si fa quindi su ES, su richiesta dell'utente del 14 ottobre 2026.

Cosa fa, sul file del workspace e solo ad ATAS chiuso (ATAS risalva il workspace alla chiusura):

1. salva una copia accanto: "<nome> prima della finestra ES.aws";
2. clona le quattro tab della prima finestra (composito, settimana, seduta, range) cambiando il
   contratto: NQ continuo -> ES continuo, NQZ6 -> ESZ6. Stesso indicatore (Fabio Data Bridge),
   stessi timeframe; i disegni no, perche' su ATAS appartengono allo strumento;
3. solo con --con-opzioni (non usato: l'utente il 14 ottobre ha chiesto prima i paper, poi gli
   indicatori), aggiunge le tab delle opzioni su ESZ6: gli indicatori X-Ray (assembly GexHeatmap)
   e Chain (assembly ATAS.OptionsSuite), con le impostazioni di default di ATAS, senza il Data
   Bridge (due chart col ruolo seduta sullo stesso contratto confondono il selettore);
4. la finestra nuova prende il posto della seconda finestra, se c'e' (quella aperta dall'utente per
   dare ad ATAS i contratti ES), e ne tiene dimensione e posizione.

I contratti si passano per id ATAS (ContractId), letti dal workspace: --mappa 5222=4936 34387=34376.

Uso:
    python FabioOrderFlow/tools/workspace_finestra_es.py --aspetta
    python FabioOrderFlow/tools/workspace_finestra_es.py --nome "Fabio workspace" --mappa 5222=4936 34387=34376
"""
from __future__ import annotations

import argparse
import copy
import json
import shutil
import sys
import time
import uuid

from workspace_a_tab import CARTELLA, atas_aperto, disegni, indicatori, nuovo_logger, tab, tipo

# Il formato con cui ATAS salva un indicatore (letto dal GEX Profile salvato il 14 ottobre 2026).
XRAY = "GexHeatmap.{0}, GexHeatmap, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
CHAIN = "{0}.{0}, ATAS.OptionsSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"

# Le tab delle opzioni: titolo, timeframe, indicatori. I nomi sono i tipi caricati in ATAS
# (letti con `bridge.py ui --descrivi ATAS.Indicators.Indicator`, 14 ottobre 2026); fra parentesi
# il nome dell'articolo di aiuto di ATAS.
TAB_OPZIONI = [
    ("5 OPZIONI LIVELLI", "m5", [
        XRAY.format("KeyLevels"),               # Key Levels: call wall, put wall, flip, slippery, pin
        XRAY.format("ExpectedMoveBands"),       # Expected Move
        XRAY.format("OptionsPositioning"),      # Strike Profile (GEX / DEX / VEX / CEX)
        XRAY.format("OptionsSurfaceProfile"),   # Surface Profile
        XRAY.format("OptionsMarketState"),      # Market State
    ]),
    ("6 GEX HEATMAP", "m5", [XRAY.format("GexHeatmap"), XRAY.format("KeyLevels")]),
    ("7 CHARM HEATMAP", "m5", [XRAY.format("CharmHeatmap"), XRAY.format("KeyLevels")]),
    ("8 STRIKE HEATMAP", "m5", [XRAY.format("OptionsExposureHeatmap"), XRAY.format("KeyLevels")]),
    ("9 FLUSSO OPZIONI M1", "m1", [
        XRAY.format("OptionsTrades"),           # Big Trades
        XRAY.format("OptionsVolume"),           # Flow
        XRAY.format("OptionsVolumeDelta"),      # Flow Delta
        XRAY.format("HedgePressure"),           # Hedge Pressure
        XRAY.format("OptionsImpliedVolatility"),
    ]),
    ("10 CHAIN M5", "m5", [
        CHAIN.format("OptionsOiProfile"),
        CHAIN.format("OptionsKeyLevels"),
        CHAIN.format("OptionsExpectedMove"),
        CHAIN.format("OptionsGexProfile"),
    ]),
]

M5 = {"$type": "OFT.Controls.Chart.MinuteTimeFrame, OFT.Controls", "TimeFrame": "m5", "CustomTimeFrame": 1,
      "Label": "@value", "DefaultDaysToLoad": 5, "VisualName": "M5"}


def serializzato(tipo_completo: str) -> str:
    """Un indicatore con le impostazioni di default: ATAS le completa con i valori del costruttore."""
    ind = {"Type": tipo_completo, "Settings": "{}", "SourceType": "bars", "SourceSeriesId": 0,
           "DataSeries": [], "LineSeries": [], "ShowDescription": True}
    return json.dumps(ind, indent=2, ensure_ascii=False).replace("\n", "\r\n")


def pannello(tipo_completo: str) -> dict:
    return {"IsNew": True, "SerializedIndicators": [serializzato(tipo_completo)], "Height": 60,
            "Collapsed": False, "IsBackgroundTransparent": False}


def cambia_contratto(chart: dict, mappa: dict[int, int]) -> int:
    vecchio = chart["ContractId"]
    if vecchio not in mappa:
        raise SystemExit(f"il contratto {vecchio} non e' nella mappa {mappa}")
    chart["ContractId"] = mappa[vecchio]
    chart["ContractIdentifier"] = {"AtasId": mappa[vecchio]}
    return mappa[vecchio]


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--nome", default="Fabio workspace", help="il workspace da cambiare sul posto")
    ap.add_argument("--mappa", nargs="+", default=["5222=4936", "34387=34376"],
                    help="contratti NQ=ES per id ATAS: continuo e contratto della seduta")
    ap.add_argument("--aspetta", action="store_true", help="aspetta che ATAS si chiuda")
    ap.add_argument("--con-opzioni", action="store_true", help="aggiunge anche le tab degli indicatori opzioni")
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
    copia = CARTELLA / f"{a.nome} prima della finestra ES.aws"
    shutil.copy2(p, copia)
    w = json.loads(p.read_text(encoding="utf-8-sig"))
    nq = w["Windows"][0]

    tabs = []
    for t in nq["Tabs"]:
        item = t["Layout"]["Parameter"]["Item1"]
        if item.get("Type") != "chart":
            continue
        nt = copy.deepcopy(t)
        c = nt["Layout"]["Parameter"]["Item1"]["Parameter"]
        cambia_contratto(c, mappa)
        disegni(c, set())
        nuovo_logger(c)
        tabs.append(nt)

    seduta = next(t for t in tabs if t["Title"].startswith("3 "))
    for titolo, tf, tipi in (TAB_OPZIONI if a.con_opzioni else []):
        c = copy.deepcopy(seduta["Layout"]["Parameter"]["Item1"]["Parameter"])
        if tf == "m5":
            c["TimeFrame"]["Period"] = "timeFrame"
            c["TimeFrame"]["PeriodParameters"] = copy.deepcopy(M5)
            c["TimeFrame"]["CustomDaysCount"] = 5
        c["Template"]["Panels"] = [pannello(x) for x in tipi]
        nuovo_logger(c)
        tabs.append(tab(titolo, seduta, c))

    es = copy.deepcopy(nq)
    es["Tabs"] = tabs
    if len(w["Windows"]) > 1:
        for k in ("Width", "Height", "Position", "WindowState"):
            if k in w["Windows"][1]:
                es[k] = copy.deepcopy(w["Windows"][1][k])
        w["Windows"][1] = es
    else:
        w["Windows"].append(es)

    p.write_bytes(json.dumps(w, indent=2, ensure_ascii=False).replace("\n", "\r\n").encode("utf-8"))
    print(f"scritto {p}  (copia di prima: {copia.name})")
    for t in es["Tabs"]:
        c = t["Layout"]["Parameter"]["Item1"]["Parameter"]
        print(f"  {t['Title']:<22} contratto {c['ContractId']:<6} {c['TimeFrame']['PeriodParameters'].get('VisualName'):<4} "
              f"{[tipo(i) for _, i in indicatori(c)]}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
