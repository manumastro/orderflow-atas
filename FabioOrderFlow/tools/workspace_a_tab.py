#!/usr/bin/env python3
"""Costruisce il workspace ATAS X a tab, una responsabilita' per tab, da quello che c'e' gia'.

Perche' uno script e non un file a mano: il workspace di ATAS e' un JSON di mezzo megabyte con le
impostazioni di ogni chart dentro, e il modo sicuro di ottenerne uno nuovo e' partire dai chart
che l'utente ha gia' configurato (connettore, conto, colori, footprint) e cambiare solo quello che
il ruolo richiede. Il file di partenza non si tocca: se ne scrive uno nuovo accanto, che ATAS apre
da Workspace -> Apri. Cosi' il workspace aperto in ATAS, che ATAS risalva da solo, non sovrascrive
niente, e il vecchio resta come ritorno.

Le quattro tab, dal piu' lontano al piu' vicino (CLAUDE.md, obbligo 1):

    1 COMPOSITO 90G   NQ continuo, Daily    il composito di 90 giorni, premium / fair / discount
    2 SETTIMANA M30   NQZ6, M30             una gaussiana per seduta cash, il framing
    3 SEDUTA M1       NQZ6, M1              la seduta di adesso, PD e framing, pannello, tape
    4 RANGE 40        NQZ6, range 40        lo stesso tape quando il mercato e' choppy

Il ruolo lo sceglie l'indicatore dal timeframe (IlRuoloDelChart.cs): qui si mette solo il
timeframe giusto, e si spengono LVN e nodi (9 ottobre 2026, richiesta dell'utente).

Uso:
    python FabioOrderFlow/tools/workspace_a_tab.py                  # scrive "Fabio NQ.aws"
    python FabioOrderFlow/tools/workspace_a_tab.py --da "Fabio workspace" --nome "Fabio NQ"
    python FabioOrderFlow/tools/workspace_a_tab.py --mostra "Fabio workspace"     # sola lettura
    python FabioOrderFlow/tools/workspace_a_tab.py --ritocca "Fabio workspace" --aspetta

--ritocca scrive sul file del workspace aperto, quindi si rifiuta se ATAS e' aperto: ATAS lo
risalverebbe alla chiusura e la modifica andrebbe persa. Con --aspetta resta in attesa che ATAS si
chiuda e la applica subito dopo.
"""
from __future__ import annotations

import argparse
import collections
import copy
import json
import os
import subprocess
import sys
import time
import uuid
from pathlib import Path

CARTELLA = Path(os.environ.get("APPDATA", "")) / "ATAS X" / "Workspaces2"

# Fabio: "the big trades guys are 60. The filter it's on 60" (fabio_2.txt 1:29:47).
BIG_TRADE = 60.0


def indicatori(chart: dict) -> list[tuple[dict, dict]]:
    """Le coppie (pannello, indicatore decodificato) del chart."""
    out = []
    for pannello in chart["Template"]["Panels"]:
        for s in pannello["SerializedIndicators"]:
            out.append((pannello, json.loads(s)))
    return out


def tipo(indicatore: dict) -> str:
    return indicatore["Type"].split(",")[0].split(".")[-1]


def tieni(chart: dict, quali: set[str]) -> None:
    """Lascia nel chart solo gli indicatori nominati, e i pannelli che li contengono."""
    pannelli = []
    for pannello in chart["Template"]["Panels"]:
        restano = [s for s in pannello["SerializedIndicators"] if tipo(json.loads(s)) in quali]
        if restano:
            pannello["SerializedIndicators"] = restano
            pannelli.append(pannello)
    chart["Template"]["Panels"] = pannelli


def imposta(chart: dict, quale: str, valori: dict) -> None:
    """Cambia le impostazioni di un indicatore, che ATAS salva come JSON dentro il JSON."""
    for pannello in chart["Template"]["Panels"]:
        nuove = []
        for s in pannello["SerializedIndicators"]:
            ind = json.loads(s)
            if tipo(ind) == quale:
                st = json.loads(ind["Settings"])
                for k, v in valori.items():
                    if isinstance(v, dict) and isinstance(st.get(k), dict):
                        st[k].update(v)
                    else:
                        st[k] = v
                ind["Settings"] = json.dumps(st, indent=2, ensure_ascii=False).replace("\n", "\r\n")
                s = json.dumps(ind, indent=2, ensure_ascii=False).replace("\n", "\r\n")
            nuove.append(s)
        pannello["SerializedIndicators"] = nuove


def tab(titolo: str, modello: dict, chart: dict) -> dict:
    t = copy.deepcopy(modello)
    t["Title"] = titolo
    t["Layout"]["Parameter"]["Item1"]["Parameter"] = chart
    return t


def timeframe(chart: dict, periodo: str, parametri: dict, giorni: int) -> None:
    tf = chart["TimeFrame"]
    tf["Period"] = periodo
    tf["PeriodParameters"] = parametri
    tf["CustomDaysCount"] = giorni
    tf["IsCustomPeriodSet"] = False


def nuovo_logger(chart: dict) -> None:
    if "TradingLoggerId" in chart:
        chart["TradingLoggerId"] = str(uuid.uuid4())


def senza_profilo_fisso(chart: dict) -> None:
    """Spegne il profilo fisso del template del chart: il pannello blu con VAH/VAL/vPOC e la barra
    dei lotti. Copriva meta' di ogni tab (foto del 10 ottobre 2026), e su un periodo fisso dal 24
    aprile non e' ne' il composito di 90 giorni ne' una seduta: i profili li disegna l'indicatore."""
    fp = chart.get("Template", {}).get("FixedProfile")
    if isinstance(fp, dict):
        fp["Visible"] = False


def disegni(chart: dict, tieni_tipi: set[str]) -> None:
    chart["DrawingObjectses"] = [
        d for d in chart.get("DrawingObjectses", []) if d["$type"].split(",")[0].split(".")[-1] in tieni_tipi
    ]


def atas_aperto() -> bool:
    """Vero se il processo di ATAS X (OFT.PlatformX.exe) e' in esecuzione."""
    r = subprocess.run(["tasklist", "/FI", "IMAGENAME eq OFT.PlatformX.exe", "/NH"],
                       capture_output=True, text=True, check=False)
    return "oft.platformx.exe" in r.stdout.lower()


def mostra(nome: str) -> int:
    """Cosa c'e' nel workspace salvato: per ogni tab contratto, timeframe, indicatori con le
    impostazioni che il workspace a tab cambia, profilo fisso e disegni. E' il file su disco, cioe'
    il workspace com'era all'ultimo salvataggio di ATAS, non lo schermo di adesso."""
    w = json.loads((CARTELLA / f"{nome}.aws").read_text(encoding="utf-8-sig"))
    for t in w["Windows"][0]["Tabs"]:
        item = t["Layout"]["Parameter"]["Item1"]
        if item.get("Type") != "chart":
            print(f"{t['Title']}: {item.get('Type')}")
            continue
        c = item["Parameter"]
        pp = c["TimeFrame"]["PeriodParameters"]
        fp = c.get("Template", {}).get("FixedProfile", {})
        print(f"{t['Title']}: contratto {c.get('ContractId')}  {pp.get('VisualName')}  "
              f"{c['TimeFrame'].get('CustomDaysCount')} giorni  profilo fisso "
              f"{'acceso' if isinstance(fp, dict) and fp.get('Visible') else 'spento'}")
        for _, ind in indicatori(c):
            st = json.loads(ind.get("Settings") or "{}")
            chiavi = {k: st[k] for k in ("Ruolo", "MostraLvn", "MostraNodi", "ShowWatch", "MinVol") if k in st}
            print(f"    {tipo(ind)}  {chiavi if chiavi else ''}")
        conta = collections.Counter(d["$type"].split(",")[0].split(".")[-1] for d in c.get("DrawingObjectses", []))
        print(f"    disegni nel file: {dict(conta) if conta else 'nessuno'}")
    return 0


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--da", default="Fabio workspace", help="workspace di partenza (non viene toccato)")
    ap.add_argument("--nome", default="Fabio NQ", help="nome del workspace nuovo")
    ap.add_argument("--ritocca", help="workspace gia' a tab da correggere sul posto (con ATAS chiuso): "
                                      "spegne il profilo fisso del template in ogni tab")
    ap.add_argument("--aspetta", action="store_true", help="con --ritocca: aspetta che ATAS si chiuda")
    ap.add_argument("--mostra", metavar="NOME", help="stampa cosa c'e' nel workspace salvato, senza toccarlo")
    a = ap.parse_args()

    if a.mostra:
        return mostra(a.mostra)

    if a.ritocca:
        if atas_aperto() and not a.aspetta:
            print("ATAS e' aperto: risalverebbe il workspace alla chiusura e la modifica andrebbe persa. "
                  "Chiudilo, o rilancia con --aspetta.", file=sys.stderr)
            return 2
        while atas_aperto():
            time.sleep(3)
        p = CARTELLA / f"{a.ritocca}.aws"
        w = json.loads(p.read_text(encoding="utf-8-sig"))
        for t in w["Windows"][0]["Tabs"]:
            if t["Layout"]["Parameter"]["Item1"].get("Type") == "chart":
                senza_profilo_fisso(t["Layout"]["Parameter"]["Item1"]["Parameter"])
                print(f"  {t['Title']}: profilo fisso del template spento")
        p.write_bytes(json.dumps(w, indent=2, ensure_ascii=False).replace(chr(10), chr(13) + chr(10)).encode("utf-8"))
        return 0

    sorgente = CARTELLA / f"{a.da}.aws"
    destinazione = CARTELLA / f"{a.nome}.aws"
    if destinazione.resolve() == sorgente.resolve():
        print("il workspace nuovo deve avere un nome diverso da quello di partenza", file=sys.stderr)
        return 2

    w = json.loads(sorgente.read_text(encoding="utf-8-sig"))
    finestra = w["Windows"][0]
    charts = [(t, t["Layout"]["Parameter"]["Item1"]["Parameter"]) for t in finestra["Tabs"]
              if t["Layout"].get("Type") == "dock" and t["Layout"]["Parameter"]["Item1"].get("Type") == "chart"]

    def con_bridge(c: dict) -> bool:
        return any(tipo(i) == "DataBridge" for _, i in indicatori(c))

    giornaliero = next(((t, c) for t, c in charts if c["TimeFrame"]["PeriodParameters"].get("TimeFrame") == "daily"
                        and con_bridge(c)), None)
    minuto = next(((t, c) for t, c in charts if c["TimeFrame"]["PeriodParameters"].get("TimeFrame") == "m1"
                   and con_bridge(c)), None)
    if giornaliero is None or minuto is None:
        print("servono nel workspace di partenza un chart Daily e un chart M1 col Fabio Data Bridge", file=sys.stderr)
        return 1

    spenti = {"MostraLvn": False, "MostraNodi": False}  # il ruolo resta al default, Auto
    big = {"MinVol": BIG_TRADE, "MinVolFilter": {"Value": BIG_TRADE, "Enabled": True}}

    # 1. COMPOSITO: il Daily del continuo. Via i profili di ATAS: quello fisso partiva dal 24 aprile
    #    e non era di 90 giorni; il composito lo disegna l'indicatore, e si sposta da solo.
    t1, c1 = copy.deepcopy(giornaliero)
    c1["TimeFrame"]["CustomDaysCount"] = 130  # 90 barre di composito, e qualcuna prima per vedere dove parte
    disegni(c1, set())
    tieni(c1, {"DataBridge"})
    imposta(c1, "DataBridge", spenti)
    nuovo_logger(c1)
    senza_profilo_fisso(c1)

    # 2. SETTIMANA: NQZ6 su M30, solo l'indicatore. M30 e non H1: la cassa apre alle :30.
    t2, c2 = copy.deepcopy(minuto)
    timeframe(c2, "timeFrame", {"$type": "OFT.Controls.Chart.MinuteTimeFrame, OFT.Controls", "TimeFrame": "m30",
                                "CustomTimeFrame": 1, "Label": "@value", "DefaultDaysToLoad": 15,
                                "VisualName": "M30"}, 15)
    disegni(c2, set())
    tieni(c2, {"DataBridge"})
    imposta(c2, "DataBridge", spenti)
    nuovo_logger(c2)
    senza_profilo_fisso(c2)

    # 3. SEDUTA: l'M1 com'e', senza il profilo settimanale di ATAS (era per gli LVN); i rettangoli
    #    dell'utente restano.
    t3, c3 = copy.deepcopy(minuto)
    disegni(c3, {"RectangleObj", "HorizontalLine", "HorizontalChannel", "HorizontalLineObj"})
    imposta(c3, "DataBridge", spenti)
    imposta(c3, "BigTrades", big)
    senza_profilo_fisso(c3)

    # 4. RANGE 40: "in a choppy mood, you can use the 40 range. It's still the same" (fabio_2.txt
    #    1:16:00). Footprint, big trades, delta cumulato e l'indicatore, senza DOM e senza pannello.
    t4, c4 = copy.deepcopy(minuto)
    timeframe(c4, "range", {"$type": "OFT.Controls.Chart.PeriodTimeFrame, OFT.Controls", "Period": 40,
                            "Label": "@value", "DefaultDaysToLoad": 3, "VisualName": "40"}, 3)
    disegni(c4, {"RectangleObj", "HorizontalLine", "HorizontalChannel", "HorizontalLineObj"})
    tieni(c4, {"DataBridge", "BigTrades", "CumulativeDelta"})
    imposta(c4, "DataBridge", spenti)
    imposta(c4, "BigTrades", big)
    nuovo_logger(c4)
    senza_profilo_fisso(c4)

    nuovo = copy.deepcopy(w)
    nuovo["Name"] = a.nome
    nuovo["Windows"] = [copy.deepcopy(finestra)]
    nuovo["Windows"][0]["Tabs"] = [
        tab("1 COMPOSITO 90G", t1, c1),
        tab("2 SETTIMANA M30", t2, c2),
        tab("3 SEDUTA M1", t3, c3),
        tab("4 RANGE 40", t4, c4),
    ]

    testo = json.dumps(nuovo, indent=2, ensure_ascii=False).replace("\n", "\r\n")
    destinazione.write_bytes(testo.encode("utf-8"))
    print(f"scritto {destinazione}")
    for t in nuovo["Windows"][0]["Tabs"]:
        c = t["Layout"]["Parameter"]["Item1"]["Parameter"]
        pp = c["TimeFrame"]["PeriodParameters"]
        print(f"  {t['Title']:<18} contratto {c['ContractId']:<6} {pp.get('VisualName')}  "
              f"indicatori {[tipo(i) for _, i in indicatori(c)]}  disegni {len(c.get('DrawingObjectses', []))}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
