#!/usr/bin/env python3
"""Tutti i dati dello studio globale, in un colpo, prima che parta qualunque agente.

    python studio/raccogli.py --out studio/rapporti/<data>/dati

**Perche' esiste** (15 ottobre 2026). Nel primo giro ogni agente raccoglieva i suoi dati e li
ricalcolava: l'agente della gamma ha rifatto la GEX due volte e si e' scritto uno script per gli
strike, quello dell'order flow ha scaricato 1,8 MB di minuti e ricostruito aree di valore che
l'indicatore gia' misura. Un'ora per quattro fasi su cinque. La regola della metodologia agentica e'
"il codice misura, l'agente legge": qui il codice misura tutto, una volta, e gli agenti leggono file.

Due fili in parallelo, perche' non si disturbano:

    rete     macro (World Monitor e altre fonti pubbliche)  ->  macro.json
             COT (CFTC)                                     ->  cot.json
    bridge   le quattro tab, il giro, il pannello, le sedute, le ore  ->  vedi sotto
             la GEX di NQZ6 (ES solo con --con-es: lo studio e' di NQ, richiesta del 15 ottobre 2026)

Alla fine `raccolta.json` dice, fonte per fonte, se c'e', quanto ci ha messo e l'errore.
"""
from __future__ import annotations

import argparse
import contextlib
import datetime as dt
import io
import json
import math
import os
import subprocess
import sys
import threading
import time
import traceback
from concurrent.futures import ThreadPoolExecutor
from pathlib import Path
from zoneinfo import ZoneInfo

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")

REPO = Path(__file__).resolve().parents[1]
STRUMENTI = REPO / "FabioOrderFlow" / "tools"
for p in (STRUMENTI, REPO / "studio" / "macro", REPO / "studio" / "cot"):
    sys.path.insert(0, str(p))

import bridge as client  # noqa: E402
import cot  # noqa: E402
import gex  # noqa: E402
import le_quattro_tab  # noqa: E402
import macro  # noqa: E402
import scala_dei_tempi as scala  # noqa: E402

ROMA = ZoneInfo("Europe/Rome")
STAMPA = threading.Lock()


def scrivi(cartella: Path, nome: str, dati) -> None:
    testo = dati if isinstance(dati, str) else json.dumps(dati, ensure_ascii=False, indent=1)
    (cartella / nome).write_text(testo, encoding="utf-8")


def passo(manifesto: dict, nome: str, file: str, fn):
    """Esegue una fonte, ne registra esito e durata; un errore non ferma le altre."""
    inizio = time.time()
    try:
        out = fn()
        manifesto[nome] = {"stato": "ok", "file": file, "secondi": round(time.time() - inizio, 1),
                           "raccolto": dt.datetime.now(ROMA).strftime("%H:%M")}
        return out
    except (Exception, SystemExit) as e:  # noqa: BLE001 - il client del bridge esce con SystemExit
        manifesto[nome] = {"stato": "errore", "file": file, "secondi": round(time.time() - inizio, 1),
                           "errore": f"{type(e).__name__}: {e}"[:300]}
        with STAMPA:
            print(f"  ! {nome}: {type(e).__name__}: {e}", file=sys.stderr)
            traceback.print_exc(limit=2, file=sys.stderr)
        return None
    finally:
        with STAMPA:
            print(f"  {nome:<16} {manifesto[nome]['stato']:<7} {manifesto[nome]['secondi']:>6.1f} s")


# --- le misure dell'order flow --------------------------------------------------------------------

def sedute(chart_m1: str, quante: int = 20) -> dict:
    """Le sedute di cassa di NQZ6: apertura, chiusura, estremi, volume, delta, POC e valore, dove chiude
    rispetto al proprio valore, e se il valore sale, scende o si stacca dal giorno prima. Le stesse
    misure del giro (scala_dei_tempi.py), su piu' giorni: la scala dei POC si legge da qui."""
    scala.PORTA = scala.porta()
    salute = scala.chiedi(f"/health?chart={chart_m1}") or {}
    oggi = scala.giorno_di_mercato(salute.get("marketTimeUtc") or dt.datetime.utcnow().isoformat())
    cache, mancanti = scala.riempi(chart_m1, "NQZ6", oggi, ("13:30", "20:00"), 1.0, budget=240)
    righe, prec = [], None
    for k in sorted(x for x in cache if not x.startswith("_")):
        c = (cache[k] or {}).get("cassa")
        if not c or k >= oggi:
            continue
        v = scala.valore({float(a): b for a, b in c["hist"].items()})
        if not v:
            continue
        poc, val, vah = v
        r = {"giorno": k, "apertura": c["O"], "massimo": c["H"], "minimo": c["L"], "chiusura": c["C"],
             "volume": c["V"], "delta": c["D"], "poc": poc, "val": val, "vah": vah,
             "chiude": "sopra" if c["C"] > vah else "sotto" if c["C"] < val else "dentro"}
        if prec:
            r["valore_rispetto_a_ieri"] = "SU" if poc > prec["poc"] else "GIU" if poc < prec["poc"] else "="
            r["staccato"] = val > prec["vah"] or vah < prec["val"]
            # lo shift del live, alla lettera: VAL piu' basso E VAH piu' basso (o entrambi piu' alti)
            # [1 · 22:13 - 24:08]; il POC da solo non basta (lacuna segnalata dalla fase 4)
            r["shift"] = ("giu" if val < prec["val"] and vah < prec["vah"] else
                          "su" if val > prec["val"] and vah > prec["vah"] else "nessuno")
        r["_hist"] = c["hist"]
        righe.append(r)
        prec = r
    # IL COMPOSITO DI SOLA CASSA A 90 SEDUTE, la regola del live (lacuna della fase 4). ATAS da' circa 30
    # giorni di minuti (contratto-data-bridge.md, /serie): il composito lungo si costruisce con l'archivio
    # delle sedute di scala_dei_tempi, che non si svuota e cresce di una seduta al giorno. Finche' non ne ha
    # 90, `sedute` dice quante ne contiene. Prima del rollover dell'11/09 NQZ6 non era il contratto
    # principale: quelle sedute pesano poco (volume basso) e si contano in `prima_del_rollover`.
    composito_lungo = composito_di(righe[-90:])
    if composito_lungo:
        composito_lungo["prima_del_rollover"] = sum(1 for r in righe[-90:] if r["giorno"] < "2026-09-11")
    righe = righe[-quante:]
    # il profilo per fasce della seduta piu' recente (lacuna della fase 4): non solo POC e bordi
    if righe:
        righe[-1]["profilo_da_10"] = fasce_del_profilo(righe[-1]["_hist"])
    # il composito di sola cassa delle ultime `quante` sedute: la scala intermedia fra la settimana e i 90
    composito = composito_di(righe)
    for r in righe:
        r.pop("_hist", None)
    # OGGI, diviso: la notte (dalle 22:00Z di ieri alle 13:30Z) e la cassa in corso, ciascuna con il suo
    # valore. Fino al 15/10 `oggi_finora` era il giorno intero, notte e cassa insieme (lacuna della fase 4)
    oggi_m = scala.misura(chart_m1, oggi, ("13:30", "20:00"), 1.0) or {}
    oggi_out: dict = {}
    for parte in ("notte", "cassa"):
        x = oggi_m.get(parte)
        if not x:
            continue
        v = scala.valore({float(a): b for a, b in x["hist"].items()})
        oggi_out[parte] = {"apertura": x["O"], "massimo": x["H"], "minimo": x["L"], "ultimo": x["C"],
                           "volume": x["V"], "delta": x["D"]} | ({"poc": v[0], "val": v[1], "vah": v[2]} if v else {})
        oggi_out[parte]["profilo_da_10"] = fasce_del_profilo(x["hist"])
    cassa_oggi = oggi_out.get("cassa")
    if cassa_oggi and righe and "poc" in cassa_oggi:
        # la cassa di oggi in fila con le sedute chiuse: stesse misure, ma IN CORSO fino alle 22:00 italiane
        ieri = righe[-1]
        cassa_oggi["in_corso"] = (salute.get("marketTimeUtc") or "")[11:16] < "20:00"
        cassa_oggi["valore_rispetto_a_ieri"] = ("SU" if cassa_oggi["poc"] > ieri["poc"] else
                                                "GIU" if cassa_oggi["poc"] < ieri["poc"] else "=")
        cassa_oggi["staccato"] = cassa_oggi["val"] > ieri["vah"] or cassa_oggi["vah"] < ieri["val"]
        cassa_oggi["shift"] = ("giu" if cassa_oggi["val"] < ieri["val"] and cassa_oggi["vah"] < ieri["vah"] else
                               "su" if cassa_oggi["val"] > ieri["val"] and cassa_oggi["vah"] > ieri["vah"] else "nessuno")
        cassa_oggi["dove_sta_l_ultimo"] = ("sopra" if cassa_oggi["ultimo"] > cassa_oggi["vah"] else
                                           "sotto" if cassa_oggi["ultimo"] < cassa_oggi["val"] else "dentro")
    ultimo = (oggi_m.get("giorno") or {}).get("C")
    # la variabile di BDLM (Baltussen e altri): il rendimento dalla chiusura di cassa di ieri. Si scrive
    # sempre; quella del paper e' alle 21:30 italiane (19:30Z in ora legale)
    rendimento = None
    if righe and ultimo:
        rendimento = {"chiusura_di_ieri": righe[-1]["chiusura"], "ultimo": ultimo,
                      "punti": round(ultimo - righe[-1]["chiusura"], 2),
                      "percento": round(100 * (ultimo / righe[-1]["chiusura"] - 1), 3),
                      "quando_utc": salute.get("marketTimeUtc")}
    # la zona nel composito di cassa: premium sopra il VAH, discount sotto il VAL, fair value dentro
    # (profile-framing.md). La soglia e' il bordo del valore, non una distanza scelta da noi
    zona = {}
    for nome, c in (("20_sedute", composito), ("90_sedute", composito_lungo)):
        if c and ultimo:
            zona[nome] = ("premium" if ultimo > c["vah"] else "discount" if ultimo < c["val"] else "fair value")
    return {"strumento": "NQZ6", "cassa": "13:30Z-20:00Z", "fasce": 1.0, "oggi": oggi,
            "sedute_non_in_cache": mancanti, "sedute": righe, "composito_di_cassa": composito, "composito_di_cassa_90": composito_lungo,
            "zona_nel_composito_di_cassa": zona, "oggi_notte": oggi_out.get("notte"), "oggi_cassa": cassa_oggi,
            "rendimento_dalla_chiusura_di_ieri": rendimento,
            "fonte": "bridge, M1 con footprint; valore al 70% dal POC (scala_dei_tempi.valore)"}


def composito_di(righe: list[dict]) -> dict | None:
    """Il composito di sola cassa di un gruppo di sedute: POC, valore e i cinque nodi piu' pesanti a fasce
    da 5 punti, distanti almeno 50 punti fra loro (dove stanno i diversi cuori di volume, non cinque fasce
    attaccate al POC)."""
    comp: dict[float, float] = {}
    for r in righe:
        for k, v in (r.get("_hist") or {}).items():
            comp[float(k)] = comp.get(float(k), 0) + v
    if not comp:
        return None
    cv = scala.valore(comp)
    fasce: dict[float, float] = {}
    for k, v in comp.items():
        fasce[k // 5 * 5] = fasce.get(k // 5 * 5, 0) + v
    tot = sum(comp.values())
    nodi: list[float] = []
    for k in sorted(fasce, key=fasce.get, reverse=True):
        if all(abs(k - n) >= 50 for n in nodi):
            nodi.append(k)
        if len(nodi) == 5:
            break
    return {"sedute": len(righe), "dal": righe[0]["giorno"], "poc": cv[0], "val": cv[1], "vah": cv[2],
            "nodi_da_5_punti": [{"da": k, "a": k + 5, "quota_percento_del_volume": round(100 * fasce[k] / tot, 2)}
                                for k in nodi]}


def fasce_del_profilo(hist: dict, larghezza: float = 10, soglia: float = 2.0) -> list[dict]:
    """Il profilo a fasce di `larghezza` punti, solo le fasce con almeno `soglia`% del volume, dall'alto."""
    fasce: dict[float, float] = {}
    for k, v in hist.items():
        f = float(k) // larghezza * larghezza
        fasce[f] = fasce.get(f, 0) + v
    tot = sum(fasce.values()) or 1
    return [{"da": k, "a": k + larghezza, "quota": round(100 * v / tot, 1)}
            for k, v in sorted(fasce.items(), reverse=True) if 100 * v / tot >= soglia]


def ore(chart_m1: str, base: str) -> dict:
    """Le ultime 48 ore di NQZ6 a barre orarie (dall'M1), con orario italiano: la scala intermedia fra
    la tab M30 e il tape, e la reazione agli eventi di ieri senza scaricare i minuti."""
    da = (dt.datetime.now(dt.timezone.utc) - dt.timedelta(hours=48)).strftime("%Y-%m-%dT%H:00:00Z")
    cs = client.get(base, "/candles", {"chart": chart_m1, "from": da}, timeout=120).get("candles") or []
    gruppi: dict[str, list] = {}
    for c in cs:
        gruppi.setdefault(c["time"][:13], []).append(c)
    out = []
    for h in sorted(gruppi):
        g = gruppi[h]
        t = dt.datetime.fromisoformat(h + ":00:00+00:00").astimezone(ROMA)
        out.append({"ora_italiana": t.strftime("%d/%m %H:00"), "apertura": g[0]["open"], "massimo": max(x["high"] for x in g),
                    "minimo": min(x["low"] for x in g), "chiusura": g[-1]["close"], "volume": sum(x["volume"] for x in g),
                    "delta": sum(x.get("delta", 0) for x in g), "minuti": len(g)})
    return {"strumento": "NQZ6", "barre_orarie": out, "volatilita_realizzata": realizzata(cs),
            "fonte": "bridge, M1 aggregato per ora (l'ultima e' aperta)"}


def realizzata(cs: list[dict]) -> dict:
    """La volatilita' realizzata annua dai rendimenti a 5 minuti delle barre M1 (lacuna della fase 3: la
    mossa attesa delle opzioni contro la realizzata sullo stesso orizzonte). Convenzione: 276 barre da 5
    minuti per giorno di 23 ore, 252 giorni; la barra aperta si esclude."""
    chiuse = cs[:-1]
    out: dict = {"convenzione": "rendimenti log a 5 minuti, annualizzati con 276 x 252"}
    for nome, quante_ore in (("ultime_4_ore", 4), ("ultime_24_ore", 24), ("ultime_48_ore", 48)):
        chius = [c["close"] for c in chiuse[-quante_ore * 60:][4::5]]
        r = [math.log(b / a) for a, b in zip(chius, chius[1:]) if a > 0 and b > 0]
        if len(r) >= 10:
            out[nome] = round(100 * math.sqrt(sum(x * x for x in r) / len(r) * 276 * 252), 1)
    return out


def barre_range(base: str, chart: dict | None, quante: int = 600) -> dict:
    """La durata delle barre range: una distribuzione di riferimento (lacuna della fase 4), cosi' "veloce"
    e "lenta" nella tab 4 hanno un metro. Ultime `quante` barre."""
    if not chart:
        return {}
    da = (dt.datetime.now(dt.timezone.utc) - dt.timedelta(days=3)).strftime("%Y-%m-%dT%H:%M:%SZ")
    cs = client.get(base, "/candles", {"chart": chart["id"], "from": da}, timeout=120).get("candles") or []
    t = [dt.datetime.fromisoformat(c["time"].replace("Z", "+00:00")) for c in cs]
    durate = [(b - a).total_seconds() for a, b in zip(t, t[1:])][-quante:]
    if not durate:
        return {}
    s_ = sorted(durate)
    q = lambda p: s_[min(len(s_) - 1, int(p / 100 * len(s_)))]
    ultime = sorted(durate[-30:])
    return {"chart": f"{chart['instrument']} Range {chart['timeFrame']}", "barre": len(s_),
            "durata_secondi": {"p10": q(10), "p25": q(25), "mediana": q(50), "p75": q(75), "p90": q(90)},
            "ultime_30_mediana": ultime[len(ultime) // 2],
            "nota": "durata = tempo fra l'inizio di una barra e della successiva; include le pause del mercato"}


def quattro() -> str:
    buf = io.StringIO()
    with contextlib.redirect_stdout(buf):
        le_quattro_tab.stampa("lontane")
        le_quattro_tab.stampa("esecuzione")
    return buf.getvalue()


def giro() -> str:
    env = os.environ | {"PYTHONUTF8": "1", "PYTHONIOENCODING": "utf-8", "GEX_SENZA_RICALCOLO": "1"}
    return subprocess.run([sys.executable, str(STRUMENTI / "giro_orizzonte.py"), "--barre", "5"], cwd=REPO,
                          capture_output=True, text=True, encoding="utf-8", timeout=180, env=env).stdout


def confluenze(cartella: Path, soglia: float = 15.0) -> dict:
    """I prezzi che due fonti diverse indicano entro `soglia` punti (lacuna del quadro: la tabella la
    faceva a mano). Fonti: order flow (sedute di cassa, oggi, composito), gamma (zeri, muri, strike
    pesanti), COT (il valore della cassa nei giorni del report). Il codice trova le vicinanze; dire a
    cosa serve arrivarci resta al quadro. `stesso_dato` segna due fonti che leggono lo stesso numero
    del bridge (il COT usa le sedute dell'order flow): e' una conferma di lettura, non di fonte."""
    def leggi(nome: str) -> dict:
        try:
            return json.loads((cartella / nome).read_text(encoding="utf-8"))
        except (OSError, ValueError):
            return {}
    livelli: list[dict] = []
    sed = leggi("sedute-NQZ6.json")
    for r in sed.get("sedute") or []:
        for k in ("poc", "val", "vah"):
            livelli.append({"prezzo": r[k], "fonte": "orderflow", "cosa": f"{k.upper()} cassa {r['giorno']}",
                            "dato": f"{r['giorno']}:{k}"})
    for parte in ("oggi_cassa", "oggi_notte"):
        x = sed.get(parte) or {}
        for k in ("poc", "val", "vah"):
            if k in x:
                livelli.append({"prezzo": x[k], "fonte": "orderflow", "cosa": f"{k.upper()} {parte.replace('_', ' ')}",
                                "dato": f"{parte}:{k}"})
    for chiave, nome in (("composito_di_cassa", "composito di cassa 20"), ("composito_di_cassa_90", "composito di cassa 90")):
        comp = sed.get(chiave) or {}
        for k in ("poc", "val", "vah"):
            if k in comp:
                livelli.append({"prezzo": comp[k], "fonte": "orderflow", "cosa": f"{k.upper()} {nome} ({comp['sedute']} sedute)",
                                "dato": f"{chiave}:{k}"})
    gx = leggi("gex-NQZ6.json")
    for parte, nome in (("tutte", "tutte"), ("settimana", "entro 7 giorni"), ("prima", "0DTE")):
        x = gx.get(parte) or {}
        if x.get("zero"):
            livelli.append({"prezzo": round(x["zero"], 1), "fonte": "gamma", "cosa": f"zero gamma A, {nome}",
                            "dato": f"gex:{parte}:zero"})
        for w in ("call_wall", "put_wall"):
            if x.get(w):
                livelli.append({"prezzo": x[w], "fonte": "gamma", "cosa": f"{w.replace('_', ' ')}, {nome}",
                                "dato": f"gex:{parte}:{w}"})
    pesanti = sorted(gx.get("per_strike") or [], key=lambda r: abs(r.get("gex_netta") or 0), reverse=True)[:8]
    for r in pesanti:
        livelli.append({"prezzo": r["k"], "fonte": "gamma", "cosa": f"strike pesante, GEX netta A {r['gex_netta']}",
                        "dato": f"gex:strike:{r['k']}"})
    for r in (leggi("cot.json").get("prezzo_alle_date") or []):
        for k in ("poc", "val", "vah"):
            if r.get(k):
                livelli.append({"prezzo": r[k], "fonte": "cot", "cosa": f"{k.upper()} cassa del report {r['giorno']}",
                                "dato": f"{r['giorno']}:{k}"})
    livelli.sort(key=lambda x: x["prezzo"])
    # gruppi: livelli consecutivi a non piu' di `soglia` dal primo del gruppo
    gruppi, cur = [], []
    for l in livelli:
        if cur and l["prezzo"] - cur[0]["prezzo"] > soglia:
            gruppi.append(cur)
            cur = []
        cur.append(l)
    if cur:
        gruppi.append(cur)
    out = []
    for g in gruppi:
        fonti = sorted({x["fonte"] for x in g})
        if len(fonti) < 2:
            continue
        dati = [x["dato"] for x in g]
        stesso = all(x["fonte"] != "gamma" for x in g) and len(set(dati)) < len(dati)
        out.append({"da": g[0]["prezzo"], "a": g[-1]["prezzo"], "fonti": fonti, "stesso_dato": stesso,
                    "livelli": [{k: x[k] for k in ("prezzo", "fonte", "cosa")} for x in g]})
    return {"soglia_punti": soglia, "livelli_letti": len(livelli), "confluenze": out,
            "nota": "solo vicinanze fra fonti diverse; nessuna e' validata come supporto o resistenza"}


def in_breve(cartella: Path) -> dict:
    """UN FILE PICCOLO PER FASE, da leggere per primo (15 ottobre 2026: la gamma e l'order flow leggevano i
    JSON grandi a pezzi e sforavano il tempo). Sono gli stessi numeri dei file grandi, scelti e messi in
    fila, piu' le poche misure che prima l'agente calcolava a mano (la scala della GEX): ogni conto si fa
    qui, una volta. I file grandi restano per un dato che qui manca."""
    def leggi(nome: str) -> dict:
        try:
            return json.loads((cartella / nome).read_text(encoding="utf-8"))
        except (OSError, ValueError):
            return {}
    gx, sed, pan = leggi("gex-NQZ6.json"), leggi("sedute-NQZ6.json"), leggi("pannello.json")
    out: dict = {}
    if gx:
        tot = gx.get("tutte") or {}
        vol_medio = None
        righe = sed.get("sedute") or []
        if righe:
            vol_medio = round(sum(r["volume"] for r in righe) / len(righe))
        f = gx.get("future") or 0
        # la scala: future NQ da comprare o vendere per un movimento dell'1%, contro il volume medio di cassa
        # (GEX in milioni di $ per 1%; un future NQ vale 20 $ a punto, quindi f*20 $ di nozionale)
        scala = {}
        for conv in ("tot_a", "tot_b"):
            if tot.get(conv) and f:
                n = abs(tot[conv]) * 1e6 / (f * 20)
                scala[conv.replace("tot_", "convenzione_")] = {
                    "future_per_1pct": round(n), "punti_di_un_1pct": round(f / 100, 1),
                    "quota_del_volume_medio_di_cassa_pct": round(100 * n / vol_medio, 2) if vol_medio else None}
        pesanti = sorted(gx.get("per_strike") or [], key=lambda r: abs(r.get("gex_netta") or 0), reverse=True)[:10]
        out["gamma"] = {
            "quando": gx.get("quando"), "future": f, "future_quando": gx.get("future_quando"),
            "prezzo_bridge": gx.get("prezzo_bridge"),
            "orizzonti": {k: {x: (gx.get(k) or {}).get(x) for x in ("tot_a", "tot_b", "zero", "zeri", "call_wall",
                                                                      "put_wall", "peso_b", "giorno")}
                          for k in ("tutte", "settimana", "prima")},
            "strike_pesanti": pesanti,
            "scadenze": [{x: s_.get(x) for x in ("giorno", "tipo", "oi", "vol_atm", "mossa_attesa", "quotate",
                                                 "opzioni", "oi_stabile")} | (gx.get("gex_per_scadenza") or {}).get(s_["giorno"], {})
                         for s_ in gx.get("scadenze") or []],
            "scadenze_saltate": [x.get("giorno") for x in gx.get("scadenze_saltate") or []],
            "oi_cambiato": gx.get("oi_cambiato_rispetto_al_calcolo_precedente"),
            "volume_medio_di_cassa_20_sedute": vol_medio, "scala": scala,
            "realizzata": (leggi("ore-NQZ6.json") or {}).get("volatilita_realizzata")}
    if sed or pan:
        out["orderflow"] = {
            "pannello": [r.get("text") for r in pan.get("lines") or []],
            "oggi_notte": sed.get("oggi_notte"), "oggi_cassa": sed.get("oggi_cassa"),
            "ultime_5_sedute": [{k: v for k, v in r.items() if k != "profilo_da_10"} for r in (sed.get("sedute") or [])[-5:]],
            "profilo_di_ieri": ((sed.get("sedute") or [{}])[-1]).get("profilo_da_10"),
            "composito_di_cassa": sed.get("composito_di_cassa"), "composito_di_cassa_90": sed.get("composito_di_cassa_90"),
            "zona": sed.get("zona_nel_composito_di_cassa"), "rendimento_dalla_chiusura_di_ieri": sed.get("rendimento_dalla_chiusura_di_ieri"),
            "composito_della_tab_1": (leggi("tab-ruoli.json").get("composito") or {}).get("dati"),
            "barre_range": leggi("range-NQZ6.json")}
    out["confluenze"] = (leggi("confluenze.json") or {}).get("confluenze")
    return out


def orderflow(cartella: Path, manifesto: dict, giro_dopo: bool = False) -> None:
    """Le misure dell'order flow. Con `giro_dopo` il giro e le quattro tab NON si fanno qui: li fa main()
    una volta sola, a GEX finita (ogni calcolo si fa una volta)."""
    base = client.discover(None)
    tab = le_quattro_tab.le_tab(client.get(base, "/charts", timeout=10)["charts"])
    m1 = (tab.get("seduta") or {}).get("id")

    def ruoli():
        return {nome: (client.get(base, "/ruolo", {"chart": c["id"]}, timeout=30) if c else None)
                for nome, c in tab.items()}
    scrivi(cartella, "tab-ruoli.json", passo(manifesto, "tab-ruoli", "tab-ruoli.json", ruoli) or {})

    if not giro_dopo:
        scrivi(cartella, "quattro-tab.txt", passo(manifesto, "quattro-tab", "quattro-tab.txt", quattro) or "")
        scrivi(cartella, "giro.txt", passo(manifesto, "giro", "giro.txt", giro) or "")

    scrivi(cartella, "pannello.json", passo(manifesto, "pannello", "pannello.json",
                                            lambda: client.get(base, "/panel", {"chart": m1}, timeout=30)) or {})
    scrivi(cartella, "sedute-NQZ6.json", passo(manifesto, "sedute", "sedute-NQZ6.json", lambda: sedute(m1)) or {})
    scrivi(cartella, "ore-NQZ6.json", passo(manifesto, "ore", "ore-NQZ6.json", lambda: ore(m1, base)) or {})
    scrivi(cartella, "range-NQZ6.json", passo(manifesto, "range", "range-NQZ6.json",
                                              lambda: barre_range(base, tab.get("esecuzione"))) or {})


def gex_fresca(strumento: str, eta_massima: float = 30 * 60) -> dict:
    """La GEX di uno strumento, CALCOLATA UNA VOLTA per tutti (richiesta del 15 ottobre 2026): la cache
    se ha meno di trenta minuti, la stessa soglia con cui il giro d'orizzonte la ricalcola; altrimenti un
    calcolo nuovo (circa tre minuti e mezzo), che poi usa anche il giro. L'eta' sta in `future_quando`."""
    try:
        r = json.loads(gex.cache_di(strumento).read_text(encoding="utf-8"))
        if time.time() - dt.datetime.fromisoformat(r.get("future_quando") or r["quando"]).timestamp() < eta_massima:
            return r
    except (OSError, ValueError, KeyError):
        pass
    return gex.esegui(strumento)


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--out", required=True, help="cartella dei dati, di solito studio/rapporti/<data>/dati")
    ap.add_argument("--fonti", default="macro,cot,orderflow,gex",
                    help="quali fonti raccogliere, fra macro, cot, orderflow, gex (default tutte). Le altre "
                         "restano come sono nella cartella: e' l'aggiornamento in giornata, su richiesta")
    ap.add_argument("--senza-gex", action="store_true", help="salta la GEX (per una prova veloce)")
    ap.add_argument("--con-es", action="store_true",
                    help="calcola anche la GEX di ESZ6 (circa 6-8 minuti in piu'); per difetto solo NQZ6")
    a = ap.parse_args()
    fonti = {f.strip() for f in a.fonti.split(",") if f.strip()} - ({"gex"} if a.senza_gex else set())
    strumenti_gex = ("NQZ6", "ESZ6") if a.con_es else ("NQZ6",)
    ignote = fonti - {"macro", "cot", "orderflow", "gex"}
    if ignote:
        raise SystemExit(f"fonti sconosciute: {', '.join(sorted(ignote))}")
    # la GEX la calcola questo collettore: il giro e le tab non devono lanciarne un'altra in background
    os.environ["GEX_SENZA_RICALCOLO"] = "1"
    cartella = Path(a.out)
    cartella.mkdir(parents=True, exist_ok=True)
    inizio = time.time()
    # L'AGGIORNAMENTO: le fonti non richieste restano; i file di quelle richieste si archiviano prima di
    # essere riscritti, cosi' un rapporto gia' scritto resta legato ai dati che ha letto.
    vecchio = cartella / "raccolta.json"
    manifesto: dict = json.loads(vecchio.read_text(encoding="utf-8")) if vecchio.exists() else {}
    file_di = {"macro": ["macro.json"], "cot": ["cot.json"], "gex": [f"gex-{x}.json" for x in strumenti_gex],
               "orderflow": ["tab-ruoli.json", "quattro-tab.txt", "giro.txt", "pannello.json", "sedute-NQZ6.json", "ore-NQZ6.json"]}
    da_archiviare = [f for fonte in fonti for f in file_di[fonte] if (cartella / f).exists()]
    if da_archiviare:
        archivio = cartella / "precedenti" / dt.datetime.now(ROMA).strftime("%H%M")
        archivio.mkdir(parents=True, exist_ok=True)
        for f in da_archiviare:
            (cartella / f).replace(archivio / f)
        print(f"  archiviati in {archivio.relative_to(cartella)}: {', '.join(da_archiviare)}")
    for fonte in {"macro", "cot", "orderflow", "gex"} - fonti:
        if any((cartella / f).exists() for f in file_di[fonte]):
            print(f"  {fonte}: tenuto quello gia' raccolto")
    print(f"RACCOLTA in {cartella}: {', '.join(sorted(fonti))}")

    try:
        bridge_vivo = not ({"orderflow", "gex"} & fonti) or bool(client.discover(None))
    except SystemExit:
        bridge_vivo = False
        manifesto["bridge"] = {"stato": "assente", "errore": "il bridge non risponde: ATAS e' aperto?"}
        print("  ! bridge non raggiungibile: niente order flow e niente GEX")

    with ThreadPoolExecutor(max_workers=5) as pool:
        lavori = []
        if "macro" in fonti:
            lavori.append(pool.submit(lambda: scrivi(cartella, "macro.json", passo(manifesto, "macro", "macro.json", macro.raccogli) or {})))
        if "cot" in fonti:
            lavori.append(pool.submit(lambda: scrivi(cartella, "cot.json", passo(manifesto, "cot", "cot.json", cot.raccogli) or {})))
        if bridge_vivo:
            if "orderflow" in fonti:
                lavori.append(pool.submit(orderflow, cartella, manifesto, "gex" in fonti))
            if "gex" in fonti:
                # NQ ed ES stanno su chart diversi, cioe' su istanze diverse del bridge con il loro tetto
                # di sottoscrizioni: se si chiede anche ES, si calcolano insieme
                for s in strumenti_gex:
                    lavori.append(pool.submit(
                        lambda s=s: scrivi(cartella, f"gex-{s}.json",
                                           passo(manifesto, f"gex {s}", f"gex-{s}.json", lambda: gex_fresca(s)) or {})))
        for l in lavori:
            l.result()

    # IL GIRO DOPO LA GEX, UNA VOLTA: raccolto in parallelo, la sua tab 2 stampava la GEX precedente (il
    # 15/10 alle 18:43 quella delle 11:51). Ora, se si raccoglie anche la GEX, il giro si fa solo qui.
    if "gex" in fonti and "orderflow" in fonti and bridge_vivo:
        scrivi(cartella, "quattro-tab.txt", passo(manifesto, "quattro-tab", "quattro-tab.txt", quattro) or "")
        scrivi(cartella, "giro.txt", passo(manifesto, "giro", "giro.txt", giro) or "")
    scrivi(cartella, "confluenze.json", passo(manifesto, "confluenze", "confluenze.json",
                                              lambda: confluenze(cartella)) or {})
    scrivi(cartella, "in-breve.json", passo(manifesto, "in-breve", "in-breve.json", lambda: in_breve(cartella)) or {})

    manifesto["_totale_secondi"] = round(time.time() - inizio, 1)
    manifesto["_raccolto"] = dt.datetime.now(ROMA).strftime("%Y-%m-%d %H:%M ora italiana")
    scrivi(cartella, "raccolta.json", manifesto)
    errori = [k for k, v in manifesto.items() if isinstance(v, dict) and v.get("stato") != "ok"]
    print(f"FINE in {manifesto['_totale_secondi']:.0f} s; fonti mancanti: {', '.join(errori) or 'nessuna'}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
