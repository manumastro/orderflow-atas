#!/usr/bin/env python3
"""Il COT degli indici, con la procedura del live di Fabio. Fase 2 dello studio globale.

    python studio/cot/cot.py --out <cartella>     # scrive cot.json e stampa la lettura

**La procedura** (docs/research/metodo/i-tre-modelli-del-live-q1.md, sezione 9, `[1 · 26:01 - 43:18]`):

    1  report LEGACY                                            [1 · 27:46]
    2  solo NON-COMMERCIAL                                      [1 · 34:51]
    3  long assoluto contro short assoluto: lo skew di fondo
    4  poi SOLO L'ULTIMA VARIAZIONE: chi ha il piede sull'acceleratore   [1 · 30:01]
       skew forte = 30% di differenza fra i due lati            [1 · 33:12]
    5  cross-confirm su S&P 500, NASDAQ, Dow, Russell           [1 · 32:31]
    6  la DATA del report si riporta sul prezzo: dove stava rispetto al valore   [1 · 31:53]

**La fonte.** Fabio legge Tradingster, che ripubblica i dati della CFTC. Qui si leggono alla fonte,
dall'API pubblica della CFTC (publicreporting.cftc.gov, gratuita, senza chiave; la stessa che usa
World Monitor in `scripts/seed-cot.mjs`). Verificato il 15 ottobre 2026: il 22 settembre 2026 il
Nasdaq legacy da' Non-Commercial long 88.507 e short 32.357, Δ +3.445 e -18.987, esattamente lo
snapshot Tradingster (docs/research/cot/snapshot-NQZ6-2026-09-22.md).

**Le convenzioni nostre**, dichiarate: lo skew della variazione e' Δnet / (|Δlong| + |Δshort|), con
soglia 0,30 (come `build_cot_cross_index.py`); il percentile del netto e' su tre anni (156
settimane); lo z della variazione netta su 52 settimane. Il rollover si segna in due modi, entrambi
dichiarati: `settimana_di_rollover` quando l'open interest cala di oltre il 10% nel mese di una
scadenza trimestrale; `rollover_calendario` quando il martedi' del report cade fra il lunedi' della
settimana prima della scadenza trimestrale (terzo venerdi' di marzo, giugno, settembre, dicembre) e la
scadenza stessa: e' la finestra in cui il volume passa al contratto successivo.

Il passo 6 legge il valore della seduta di cassa del martedi' del report dalla cache del giro
d'orizzonte (`~/.fabio-scala-dei-tempi-NQZ6.json`), o dal bridge se manca. Accanto c'e' il **profilo
della settimana coperta dal report** (dal mercoledi' dopo il report precedente al martedi' del report,
solo cassa): il COT fotografa il martedi', ma le posizioni si sono mosse in tutta la settimana.
Descrizione, non segnale.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import os
import statistics
import sys
import urllib.parse
import urllib.request
from pathlib import Path
from zoneinfo import ZoneInfo

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")

REPO = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(REPO / "FabioOrderFlow" / "tools"))

LEGACY = "6dca-aqww"   # Legacy, Futures Only
TFF = "gpe5-46if"      # Traders in Financial Futures, Futures Only
INDICI = {"209742": "NASDAQ-100 mini", "13874A": "S&P 500 e-mini", "124603": "Dow x5", "239742": "Russell 2000 mini"}
SETTIMANE = 160
SOGLIA_SKEW = 0.30


def socrata(dataset: str, dove: str, limite: int) -> list[dict]:
    q = urllib.parse.urlencode({"$where": dove, "$order": "report_date_as_yyyy_mm_dd DESC", "$limit": limite})
    req = urllib.request.Request(f"https://publicreporting.cftc.gov/resource/{dataset}.json?{q}",
                                 headers={"User-Agent": "orderflow-atas studio globale"})
    with urllib.request.urlopen(req, timeout=60) as r:
        return json.load(r)


def i(x) -> int:
    return int(float(x or 0))


def pct(serie: list[float], x: float) -> float:
    return round(100 * sum(1 for v in serie if v <= x) / len(serie), 1) if serie else float("nan")


def terzo_venerdi(anno: int, mese: int) -> dt.date:
    d = dt.date(anno, mese, 1)
    return d + dt.timedelta(days=(4 - d.weekday()) % 7 + 14)


def rollover_calendario(data: str) -> bool:
    """Il martedi' del report cade fra il lunedi' della settimana prima della scadenza trimestrale e la
    scadenza (terzo venerdi' di mar, giu, set, dic)."""
    g = dt.date.fromisoformat(data)
    if g.month not in (3, 6, 9, 12):
        return False
    scad = terzo_venerdi(g.year, g.month)
    return scad - dt.timedelta(days=11) <= g <= scad


def settimana(serie: list[dict], k: int) -> dict:
    """Una delle ultime settimane, con lo skew, lo z a 52 settimane e il percentile di |Δnetto| a tre anni."""
    x = serie[k]
    dn = x["dlong"] - x["dshort"]
    lordo = abs(x["dlong"]) + abs(x["dshort"])
    prec = [y["dlong"] - y["dshort"] for y in serie[k + 1:k + 53]]
    sd = statistics.pstdev(prec) if len(prec) > 5 else 0
    oi_prima = x["oi"] - x["doi"]
    mese = int(x["data"][5:7])
    return {"data": x["data"], "data_precedente": serie[k + 1]["data"] if k + 1 < len(serie) else None,
            "settimana_di_rollover": mese in (3, 6, 9, 12) and x["doi"] < -0.10 * oi_prima,
            "rollover_calendario": rollover_calendario(x["data"]),
            "netto": x["long"] - x["short"], "dlong": x["dlong"], "dshort": x["dshort"],
            "dnetto": dn, "skew": round(dn / lordo, 3) if lordo else 0.0,
            "z52": round((dn - statistics.mean(prec)) / sd, 2) if sd else None,
            "pct_abs_dnetto_3a": pct([abs(y["dlong"] - y["dshort"]) for y in serie[k:k + 156]], abs(dn)),
            "open_interest": x["oi"], "d_open_interest_pct": round(100 * x["doi"] / oi_prima, 1) if oi_prima else None}


def legacy(codice: str) -> dict:
    righe = socrata(LEGACY, f"cftc_contract_market_code='{codice}'", SETTIMANE)
    serie = []
    for r in righe:
        serie.append({"data": r["report_date_as_yyyy_mm_dd"][:10], "oi": i(r["open_interest_all"]),
                      "doi": i(r["change_in_open_interest_all"]),
                      "long": i(r["noncomm_positions_long_all"]), "short": i(r["noncomm_positions_short_all"]),
                      "dlong": i(r["change_in_noncomm_long_all"]), "dshort": i(r["change_in_noncomm_short_all"]),
                      "comm_long": i(r["comm_positions_long_all"]), "comm_short": i(r["comm_positions_short_all"])})
    u = serie[0]
    netti = [s["long"] - s["short"] for s in serie]
    dnetti = [s["dlong"] - s["dshort"] for s in serie[:53]]
    dnet = u["dlong"] - u["dshort"]
    lordo = abs(u["dlong"]) + abs(u["dshort"])
    skew = dnet / lordo if lordo else 0.0
    precedenti = dnetti[1:53]
    z = (dnet - statistics.mean(precedenti)) / statistics.pstdev(precedenti) if len(precedenti) > 5 and statistics.pstdev(precedenti) else None
    mese = int(u["data"][5:7])
    rollover = mese in (3, 6, 9, 12) and u["doi"] < -0.10 * (u["oi"] - u["doi"])
    piede = ("compratori" if dnet > 0 else "venditori") if abs(skew) >= SOGLIA_SKEW else "nessuno (skew sotto 0,30)"
    return {"mercato": righe[0]["market_and_exchange_names"], "codice": codice, "data": u["data"],
            "open_interest": u["oi"], "d_open_interest": u["doi"],
            "noncomm": {"long": u["long"], "short": u["short"], "netto": u["long"] - u["short"],
                        "dlong": u["dlong"], "dshort": u["dshort"], "dnetto": dnet,
                        "skew_fondo": round((u["long"] - u["short"]) / (u["long"] + u["short"]), 3),
                        "skew_variazione": round(skew, 3), "piede_sull_acceleratore": piede,
                        "pct_netto_3a": round(100 * sum(1 for n in netti if n <= netti[0]) / len(netti), 1),
                        "z52_dnetto": round(z, 2) if z is not None else None},
            "commercial_netto": u["comm_long"] - u["comm_short"], "settimana_di_rollover": rollover,
            "rollover_calendario": rollover_calendario(u["data"]),
            "regola_rollover": "settimana_di_rollover: OI -10% nel mese di scadenza; rollover_calendario: "
                               "report fra il lunedi' della settimana prima del terzo venerdi' trimestrale e la scadenza",
            # dal primo giro (15 ottobre 2026) l'agente se le calcolava da solo: ora le misura il codice
            "pct_long_3a": pct([s["long"] for s in serie[:156]], u["long"]),
            "pct_short_3a": pct([s["short"] for s in serie[:156]], u["short"]),
            "netto_min_max_3a": [min(netti[:156]), max(netti[:156])],
            "ultime_4": [settimana(serie, k) for k in range(4)],
            "settimane_in_serie": len(serie)}


def tff_nasdaq() -> dict:
    """La TFF del Nasdaq: per categoria netto, variazioni, percentili a tre anni di netto, long e short,
    lo z a 52 settimane della variazione netta e le ultime quattro settimane (lacune della fase 2,
    15 ottobre 2026). Le categorie non coincidono con la Legacy."""
    righe = socrata(TFF, "cftc_contract_market_code='209742'", 160)
    r = righe[0]
    out = {"data": r["report_date_as_yyyy_mm_dd"][:10]}
    for nome, (l, s, dl, ds) in {
        "dealer": ("dealer_positions_long_all", "dealer_positions_short_all", "change_in_dealer_long_all", "change_in_dealer_short_all"),
        "asset_manager": ("asset_mgr_positions_long", "asset_mgr_positions_short", "change_in_asset_mgr_long", "change_in_asset_mgr_short"),
        "leveraged_funds": ("lev_money_positions_long", "lev_money_positions_short", "change_in_lev_money_long", "change_in_lev_money_short"),
    }.items():
        serie = [{"data": x["report_date_as_yyyy_mm_dd"][:10], "long": i(x.get(l)), "short": i(x.get(s)),
                  "dlong": i(x.get(dl)), "dshort": i(x.get(ds)), "oi": 0, "doi": 0} for x in righe]
        u = serie[0]
        netti = [x["long"] - x["short"] for x in serie[:156]]
        ultime = []
        for k in range(4):
            w = settimana(serie, k)
            ultime.append({c: w[c] for c in ("data", "netto", "dlong", "dshort", "dnetto", "skew", "z52", "pct_abs_dnetto_3a")})
        out[nome] = {"long": u["long"], "short": u["short"], "netto": u["long"] - u["short"],
                     "dlong": u["dlong"], "dshort": u["dshort"], "dnetto": u["dlong"] - u["dshort"],
                     "pct_netto_3a": pct(netti, u["long"] - u["short"]),
                     "pct_long_3a": pct([x["long"] for x in serie[:156]], u["long"]),
                     "pct_short_3a": pct([x["short"] for x in serie[:156]], u["short"]),
                     "z52_dnetto": ultime[0]["z52"], "ultime_4": ultime}
    return out


def valore_del_giorno(giorno: str) -> dict | None:
    """Passo 6: la seduta di cassa di NQZ6 nel giorno del report, dalla cache del giro o dal bridge."""
    try:
        import scala_dei_tempi as sc
    except Exception:
        return None
    _, cache = sc.carica_cache("NQZ6")
    m = cache.get(giorno)
    if not m or m.get("vuoto"):
        try:
            sc.PORTA = sc.porta()
            charts = (sc.chiedi("/charts") or {}).get("charts") or []
            chart = next((c["id"] for c in charts if c["instrument"] == "NQZ6" and c["timeFrame"] == "M1"), None)
            m = sc.misura(chart, giorno, ("13:30", "20:00"), 1.0) if chart else None
        except Exception:
            m = None
    cassa = (m or {}).get("cassa")
    if not cassa:
        return None
    v = sc.valore({float(k): x for k, x in cassa["hist"].items()})
    if not v:
        return None
    poc, val, vah = v
    c = cassa["C"]
    dove = "sopra il valore" if c > vah else "sotto il valore" if c < val else "dentro il valore"
    return {"giorno": giorno, "apertura": cassa["O"], "chiusura": c, "massimo": cassa["H"], "minimo": cassa["L"],
            "poc": poc, "val": val, "vah": vah, "delta": cassa["D"], "chiude": dove,
            "fonte": "NQZ6 cassa 13:30Z-20:00Z, fasce da 1 punto (bridge)"}


def profilo_settimana(data_report: str, data_prec: str | None) -> dict:
    """Le sedute di cassa di NQZ6 coperte dal report: dal giorno dopo il report precedente al martedi'
    del report, incluso. Estremi, volume, delta e il valore della settimana (gli istogrammi di cassa
    delle sedute sommati), dalla cache del giro; i giorni feriali che mancano si dichiarano."""
    fine = dt.date.fromisoformat(data_report)
    inizio = dt.date.fromisoformat(data_prec) + dt.timedelta(days=1) if data_prec else fine - dt.timedelta(days=6)
    out = {"dal": inizio.isoformat(), "al": data_report, "sedute": [], "mancanti": [],
           "fonte": "NQZ6 cassa 13:30Z-20:00Z, fasce da 1 punto (cache di scala_dei_tempi)"}
    try:
        import scala_dei_tempi as sc
        _, cache = sc.carica_cache("NQZ6")
    except Exception as e:  # noqa: BLE001
        out["errore"] = str(e)[:160]
        return out
    hist: dict[float, float] = {}
    tot = None
    g = inizio
    while g <= fine:
        if g.weekday() < 5:
            cassa = (cache.get(g.isoformat()) or {}).get("cassa")
            if not cassa:
                out["mancanti"].append(g.isoformat())
            else:
                out["sedute"].append(g.isoformat())
                for k, v in cassa["hist"].items():
                    hist[float(k)] = hist.get(float(k), 0) + v
                if tot is None:
                    tot = {"apertura": cassa["O"], "massimo": cassa["H"], "minimo": cassa["L"], "volume": 0, "delta": 0}
                tot["chiusura"] = cassa["C"]
                tot["massimo"] = max(tot["massimo"], cassa["H"])
                tot["minimo"] = min(tot["minimo"], cassa["L"])
                tot["volume"] += cassa.get("V", 0)
                tot["delta"] += cassa.get("D", 0)
        g += dt.timedelta(days=1)
    # prima del rollover trimestrale (il giovedi' otto giorni prima del terzo venerdi') il contratto
    # principale era il precedente: il volume e il valore di NQZ6 di quei giorni sono sottostimati
    prima = []
    for x in out["sedute"]:
        d = dt.date.fromisoformat(x)
        m = d.month + (-d.month % 3)
        scad = terzo_venerdi(d.year + (m > 12), (m - 1) % 12 + 1)
        if scad < d:
            m += 3
            scad = terzo_venerdi(d.year + (m > 12), (m - 1) % 12 + 1)
        if d < scad - dt.timedelta(days=8) and (scad - d).days <= 14:
            prima.append(x)
    if prima:
        out["prima_del_rollover"] = prima
    if tot:
        out.update(tot)
        v = sc.valore(hist)
        if v:
            out["poc"], out["val"], out["vah"] = v
    return out


def prossima_pubblicazione(data_report: str) -> str:
    # il venerdi' dopo il martedi' del report, alle 15:30 di New York, scritto anche in ora italiana
    venerdi = dt.date.fromisoformat(data_report) + dt.timedelta(days=10)
    ny = dt.datetime.combine(venerdi, dt.time(15, 30), ZoneInfo("America/New_York"))
    roma = ny.astimezone(ZoneInfo("Europe/Rome"))
    return f"{venerdi.isoformat()} (venerdi', 15:30 ET = {roma:%H:%M} ora italiana)"


def raccogli() -> dict:
    """Tutto il COT in un dizionario; la usano main() e studio/raccogli.py."""
    esito = {"raccolto_utc": dt.datetime.now(dt.timezone.utc).isoformat(timespec="seconds"),
             "fonte": "publicreporting.cftc.gov (Legacy futures only 6dca-aqww; TFF gpe5-46if)",
             "indici": {}, "errori": {}}
    for codice in INDICI:
        try:
            esito["indici"][INDICI[codice]] = legacy(codice)
        except Exception as e:  # noqa: BLE001
            esito["errori"][INDICI[codice]] = str(e)[:160]
    try:
        esito["tff_nasdaq"] = tff_nasdaq()
    except Exception as e:  # noqa: BLE001
        esito["errori"]["tff"] = str(e)[:160]
    nq = esito["indici"].get("NASDAQ-100 mini")
    if nq:
        segni = {k: (v["noncomm"]["dnetto"] > 0) - (v["noncomm"]["dnetto"] < 0) for k, v in esito["indici"].items()}
        concordi = sum(1 for s in segni.values() if s == segni["NASDAQ-100 mini"])
        esito["cross_confirm"] = {"segni_dnetto": segni, "concordi_col_nasdaq": f"{concordi} su {len(segni)}"}
        esito["prezzo_alle_date"] = [x for x in (valore_del_giorno(s["data"]) for s in nq["ultime_4"]) if x]
        esito["settimane_dei_report"] = [profilo_settimana(s["data"], s["data_precedente"]) for s in nq["ultime_4"]]
        # il ritardo in prezzo: quanto si e' mosso NQZ6 dalla chiusura di cassa del giorno del report a
        # oggi (lacuna segnalata dalla fase 2, 15 ottobre 2026); l'ultimo prezzo dalla cache del giro
        try:
            import scala_dei_tempi as sc
            sc.PORTA = sc.porta()
            charts = (sc.chiedi("/charts") or {}).get("charts") or []
            m1 = next((c["id"] for c in charts if c["instrument"] == "NQZ6" and c["timeFrame"] == "M1"), None)
            ora = (sc.chiedi(f"/health?chart={m1}") or {}) if m1 else {}
            ultimo = (sc.chiedi(f"/candles?chart={m1}&fromBar={max(0, int(ora.get('bars', 1)) - 1)}") or {}).get("candles") if m1 else None
            if ultimo:
                adesso = ultimo[-1]["close"]
                esito["prezzo_adesso"] = {"NQZ6": adesso, "quando_utc": ultimo[-1]["time"]}
                for p_ in esito["prezzo_alle_date"]:
                    p_["ritardo_in_prezzo"] = {"punti": round(adesso - p_["chiusura"], 2),
                                               "percento": round(100 * (adesso / p_["chiusura"] - 1), 2),
                                               "dal_poc": round(adesso - p_["poc"], 2)}
                for w in esito["settimane_dei_report"]:
                    if "poc" in w:
                        w["prezzo_adesso_dal_poc"] = round(adesso - w["poc"], 2)
        except Exception as e:  # noqa: BLE001
            esito["errori"]["prezzo_adesso"] = str(e)[:160]
        esito["prossima_pubblicazione"] = prossima_pubblicazione(nq["data"])
    return esito


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--out", help="cartella dove scrivere cot.json")
    a = ap.parse_args()
    esito = raccogli()
    if a.out:
        os.makedirs(a.out, exist_ok=True)
        with open(os.path.join(a.out, "cot.json"), "w", encoding="utf-8") as f:
            json.dump(esito, f, ensure_ascii=False, indent=1)

    print(f"COT {esito['fonte']} — errori: {', '.join(esito['errori']) or 'nessuno'}")
    for nome, v in esito["indici"].items():
        n = v["noncomm"]
        print(f"  {nome:<18} {v['data']}  NC long {n['long']:,} short {n['short']:,} netto {n['netto']:+,} "
              f"(pct3a {n['pct_netto_3a']})  Δlong {n['dlong']:+,} Δshort {n['dshort']:+,} Δnetto {n['dnetto']:+,} "
              f"skew {n['skew_variazione']:+.2f} z52 {n['z52_dnetto']}  piede: {n['piede_sull_acceleratore']}"
              f"{'  ROLLOVER' if v['settimana_di_rollover'] else ''}")
    if "cross_confirm" in esito:
        print(f"  cross-confirm: {esito['cross_confirm']['concordi_col_nasdaq']} indici con la stessa direzione del Nasdaq")
        for p in esito["prezzo_alle_date"]:
            print(f"  {p['giorno']}: NQZ6 cassa {p['apertura']} -> {p['chiusura']}, valore {p['val']:g}-{p['vah']:g} "
                  f"POC {p['poc']:g}, {p['chiude']}, delta {p['delta']:+,}")
        print(f"  prossima pubblicazione: {esito['prossima_pubblicazione']}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
