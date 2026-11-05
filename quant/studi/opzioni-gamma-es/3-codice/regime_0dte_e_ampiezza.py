#!/usr/bin/env python3
"""Ipotesi D ed E della scheda opzioni-gamma-es, come DESCRIZIONE (5 novembre 2026).

D. Il regime dello 0DTE noto all'apertura (la GEX netta dei market maker sulle opzioni SPX che scadono
   oggi, stimata da OptionsDepth e letta da X-Ray, alle 09:30-09:35 di New York) e l'ampiezza del resto
   della seduta di cassa di ES, divisa per il movimento atteso.
E. I muri delle opzioni vicini al bordo del valore della cassa di ieri: tengono o si rompono secondo il
   regime al tocco. Solo dal 23 ottobre: aneddoti, non una misura.

Non e' una prova di strategia: nessun ingresso, nessun costo, niente registro. E' la fotografia di cio'
che c'e' (circa 45 sedute), con un nullo a permutazione per dire quanto una differenza cosi' si vede
anche a caso. La scheda dice perche' e con quali varianti: quant/studi/opzioni-gamma-es/1-idea.md.

    python quant/studi/opzioni-gamma-es/3-codice/regime_0dte_e_ampiezza.py            rapporto a video
    python ... --json <file>                                                         anche i numeri giorno per giorno

Fonti:
  X-Ray      quant/dati/xray/series/SPX/gex__mm_0dte/<giorno>.jsonl(.gz), letto con xray_query.colonne
  ES M1      quant/dati/ES/m1/<AAAA-MM>.bin (il continuo, fino al 23/10), poi il chart ESZ6 M1 del bridge
  VIX        la chiusura ufficiale del giorno prima, da Yahoo (^VIX), salvata accanto al rapporto
  valore     il valore di cassa di ogni giorno dal ruolo della tab M30 di ESZ6 (lo calcola l'indicatore)
Orari: la cassa 09:30-16:00 di New York, convertita in UTC per data con FabioOrderFlow/tools/sedute.py.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import math
import random
import statistics as st
import struct
import sys
import urllib.request
from pathlib import Path

REPO = Path(__file__).resolve().parents[4]
sys.path.insert(0, str(REPO / "FabioOrderFlow" / "tools"))
sys.path.insert(0, str(REPO / "quant" / "strumenti" / "xray" / "scripts"))
import sedute  # noqa: E402  gli orari nel fuso del mercato
import xray_query  # noqa: E402  le colonne di X-Ray, una per chiave, in ordine

XRAY = Path(xray_query.DEFAULT_DIR)
ARCHIVIO = REPO / "quant" / "dati" / "ES" / "m1"
QUI = Path(__file__).resolve().parents[1]
BRIDGE = "http://127.0.0.1:8787"
TICK = 0.25
UA = {"User-Agent": "Mozilla/5.0 (studio orderflow-atas)"}

# LE CONVENZIONI, DICHIARATE PRIMA DI GUARDARE (scheda, ipotesi D ed E)
ORA_REGIME = "09:35"        # il regime si legge alla colonna piu' recente fra 09:30 e 09:35 di New York
SOGLIA_VICINO_ES = 4.0      # muro "vicino" al bordo del valore: entro 4 punti di ES (la soglia delle confluenze di ES)
TOCCO_ES = 1.0              # il prezzo "tocca" il muro se arriva entro 1 punto
ROTTURA_ES = 3.0            # "rompe": una chiusura a 5 minuti oltre il muro di almeno 3 punti, entro 60 minuti dal tocco
PERMUTAZIONI = 10_000
SEME = 20261105


# ---------------------------------------------------------------- dati
def ms(t: dt.datetime) -> int:
    return int(t.timestamp() * 1000)


def barre_archivio() -> dict[int, tuple]:
    """Le barre M1 dell'archivio di ES (continuo), dall'agosto 2026: inizio ms -> (o, h, l, c)."""
    out = {}
    for f in sorted(ARCHIVIO.glob("2026-0[6-9].bin")) + sorted(ARCHIVIO.glob("2026-1[0-2].bin")):
        b = f.read_bytes()
        for off in range(24, len(b) - 43, 44):
            ini, _fine, o, h, l, c, _v, _vb, _va = struct.unpack_from("<qq7i", b, off)
            out[ini] = (o * TICK, h * TICK, l * TICK, c * TICK)
    return out


def get(percorso: str, timeout: float = 60):
    with urllib.request.urlopen(BRIDGE + percorso, timeout=timeout) as r:
        return json.load(r)


def chart_di(tf: str) -> str | None:
    try:
        for c in get("/charts", 10)["charts"]:
            if str(c["instrument"]).upper().startswith("ES") and c["timeFrame"] == tf and c.get("chartType") == "TimeFrame":
                return c["id"]
    except OSError:
        return None
    return None


def barre_bridge(da: str) -> dict[int, tuple]:
    """Le barre M1 di ESZ6 dal bridge, dal giorno `da`: completano l'archivio dopo il 23 ottobre."""
    cid = chart_di("M1")
    if not cid:
        return {}
    cs = get(f"/candles?chart={cid}&from={da}T00:00:00Z", 120).get("candles") or []
    out = {}
    for c in cs:
        t = dt.datetime.fromisoformat(c["time"].replace("Z", "+00:00"))
        out[ms(t)] = (c["open"], c["high"], c["low"], c["close"])
    return out


def valore_di_cassa() -> dict[str, dict]:
    """Il valore di cassa di ogni giorno dal ruolo della tab M30 di ESZ6 (l'indicatore lo calcola)."""
    cid = chart_di("M30")
    if not cid:
        return {}
    return {d["giorno"]: d for d in get(f"/ruolo?chart={cid}", 30).get("dati") or [] if not d.get("inCorso")}


def vix_giornaliero(cache: Path) -> dict[str, float]:
    """La chiusura ufficiale del VIX per giorno, da Yahoo; salvata in `cache` (la prova si rifa' uguale)."""
    if cache.exists():
        return json.loads(cache.read_text(encoding="utf-8"))["chiusure"]
    url = "https://query1.finance.yahoo.com/v8/finance/chart/%5EVIX?range=6mo&interval=1d"
    with urllib.request.urlopen(urllib.request.Request(url, headers=UA), timeout=30) as r:
        d = json.load(r)["chart"]["result"][0]
    out = {}
    for t, c in zip(d["timestamp"], d["indicators"]["quote"][0]["close"]):
        if c is not None:
            out[dt.datetime.fromtimestamp(t, sedute.NEW_YORK).date().isoformat()] = round(c, 2)
    cache.parent.mkdir(parents=True, exist_ok=True)
    cache.write_text(json.dumps({"fonte": url, "scaricato": dt.date.today().isoformat(), "chiusure": out}, indent=1),
                     encoding="utf-8")
    return out


def gex_0dte(giorno: str) -> list[dict]:
    """Le colonne della GEX dei market maker sullo 0DTE: istante ms, netto (call + put), netto entro l'1%,
    spot SPX, e gli strike con la call e la put piu' pesanti (i muri)."""
    base = XRAY / "series" / "SPX" / "gex__mm_0dte" / f"{giorno}.jsonl"
    out = []
    for col in xray_query.colonne(str(base)):
        S = col.get("strikes") or []
        C = [c or 0 for c in col.get("call") or []]
        P = [p or 0 for p in col.get("put") or []]
        spot = col.get("spot") or 0
        if not S or not spot:
            continue
        vicini = sum(c + p for s, c, p in zip(S, C, P) if abs(s - spot) <= spot * 0.01)
        out.append({"ts": int(col["ts"]), "netto": sum(C) + sum(P), "netto_1pct": vicini, "spot": spot,
                    "muro_call": max(zip(S, C), key=lambda x: abs(x[1]))[0],
                    "muro_put": max(zip(S, P), key=lambda x: abs(x[1]))[0]})
    return out


# ---------------------------------------------------------------- misure
def chiusure_di_cassa(barre: dict[int, tuple], giorni: list[str]) -> dict[str, float]:
    """La chiusura delle 16:00 di New York (la barra che inizia alle 15:59) per ogni giorno."""
    out = {}
    for g in giorni:
        b = barre.get(ms(sedute.utc(g, "15:59")))
        if b:
            out[g] = b[3]
    return out


def realizzata_20(chiusure: dict[str, float], giorno: str) -> float | None:
    """La deviazione standard giornaliera dei rendimenti log di cassa delle 20 sedute prima di `giorno`."""
    gg = sorted(g for g in chiusure if g < giorno)[-21:]
    if len(gg) < 21:
        return None
    r = [math.log(chiusure[b] / chiusure[a]) for a, b in zip(gg, gg[1:])]
    return st.stdev(r)


def una_seduta(g: str, barre: dict, cols: list[dict], vix: dict, chiusure: dict) -> dict | None:
    t930, t935, t1600 = (ms(sedute.utc(g, h)) for h in ("09:30", ORA_REGIME, "16:00"))
    alle = [c for c in cols if t930 <= c["ts"] <= t935]
    in_cassa = [c for c in cols if t930 <= c["ts"] < t1600]
    riga: dict = {"giorno": g, "colonne_in_cassa": len(in_cassa)}
    if not alle:
        riga["escluso"] = "nessuna colonna X-Ray fra 09:30 e 09:35"
        return riga
    c = alle[-1]
    resto = [barre[k] for k in sorted(barre) if t935 <= k < t1600]
    f = barre.get(t935 - 60_000)
    if len(resto) < 300 or not f:
        riga["escluso"] = f"barre ES di cassa incomplete ({len(resto)})"
        return riga
    F = f[3]
    prima = sorted(x for x in vix if x < g)
    if not prima:
        riga["escluso"] = "manca il VIX del giorno prima"
        return riga
    v = vix[prima[-1]]
    atteso_vix = F * v / 100 / math.sqrt(252)
    rv = realizzata_20(chiusure, g)
    massimo, minimo, fine = max(b[1] for b in resto), min(b[2] for b in resto), resto[-1][3]
    ampiezza = massimo - minimo
    quota_neg = sum(1 for x in in_cassa if x["netto"] < 0) / len(in_cassa) if in_cassa else None
    riga.update({
        "istante_regime": sedute.iso(dt.datetime.fromtimestamp(c["ts"] / 1000, dt.timezone.utc)),
        "netto_0dte": round(c["netto"], 1), "netto_0dte_1pct": round(c["netto_1pct"], 1),
        "regime": "positivo" if c["netto"] > 0 else "negativo",
        "regime_1pct": "positivo" if c["netto_1pct"] > 0 else "negativo",
        "regime_giornata_video": None if quota_neg is None else ("negativo" if quota_neg >= 0.5 else "positivo"),
        "quota_minuti_negativi": None if quota_neg is None else round(quota_neg, 2),
        "es_0935": F, "vix_ieri": v, "vix_giorno": prima[-1],
        "atteso_vix": round(atteso_vix, 2), "atteso_realizzata": round(F * rv, 2) if rv else None,
        "ampiezza": ampiezza, "rapporto": round(ampiezza / atteso_vix, 3),
        "rapporto_realizzata": round(ampiezza / (F * rv), 3) if rv else None,
        "rendimento_pb": round(1e4 * math.log(fine / F), 1),
        "spot_spx": c["spot"], "muro_call": c["muro_call"], "muro_put": c["muro_put"],
        "base_es_spx": base_della_seduta(barre, in_cassa)})
    return riga


def base_della_seduta(barre: dict, in_cassa: list[dict]) -> float | None:
    """La base ES - SPX della seduta: la mediana, minuto per minuto in cassa, della chiusura del minuto di ES
    meno lo spot di X-Ray. Una sola coppia alle 09:35 sbaglia di 10 punti nei minuti veloci (il 4/11 dava 20,4)."""
    d = [barre[c["ts"] - 60_000][3] - c["spot"] for c in in_cassa if c["ts"] - 60_000 in barre]
    return round(st.median(d), 2) if len(d) >= 30 else None


def gruppi(righe: list[dict], chiave: str, misura: str) -> dict:
    out = {}
    for reg in ("positivo", "negativo"):
        xs = [r[misura] for r in righe if r.get(chiave) == reg and r.get(misura) is not None]
        out[reg] = {"n": len(xs), "mediana": round(st.median(xs), 3) if xs else None,
                    "media": round(st.fmean(xs), 3) if xs else None}
    return out


def permutazione(righe: list[dict], chiave: str, misura: str, fn=st.median) -> dict:
    """Il nullo: le etichette del regime rimescolate a caso, la differenza (negativo - positivo) ricalcolata.
    p a due code = quota delle permutazioni con una differenza almeno grande quanto quella vera."""
    dati = [(r[chiave], r[misura]) for r in righe if r.get(chiave) and r.get(misura) is not None]
    neg = [x for k, x in dati if k == "negativo"]
    pos = [x for k, x in dati if k == "positivo"]
    if len(neg) < 3 or len(pos) < 3:
        return {"nota": "meno di tre sedute in un regime: niente nullo"}
    vera = fn(neg) - fn(pos)
    tutti = [x for _, x in dati]
    rnd = random.Random(SEME)
    estremi = 0
    for _ in range(PERMUTAZIONI):
        rnd.shuffle(tutti)
        d = fn(tutti[:len(neg)]) - fn(tutti[len(neg):])
        if abs(d) >= abs(vera) - 1e-12:
            estremi += 1
    return {"differenza_neg_meno_pos": round(vera, 3), "p_due_code": round((estremi + 1) / (PERMUTAZIONI + 1), 4),
            "permutazioni": PERMUTAZIONI, "seme": SEME}


# ---------------------------------------------------------------- E: i muri al bordo del valore
def muri_al_bordo(g: str, riga: dict, barre: dict, cols: list[dict], valore: dict) -> list[dict]:
    """I muri di 09:35 (in SPX, portati su ES con la base del momento) vicini al VAL o al VAH della cassa
    di ieri; per ognuno il primo tocco dopo le 09:35, il regime a quell'istante e l'esito."""
    ieri = sorted(x for x in valore if x < g)
    if not ieri or "escluso" in riga:
        return []
    v = valore[ieri[-1]]
    base = riga.get("base_es_spx")
    if base is None:
        return []
    t935, t1600 = ms(sedute.utc(g, ORA_REGIME)), ms(sedute.utc(g, "16:00"))
    chiavi = [k for k in sorted(barre) if t935 <= k < t1600]
    out = []
    for nome, strike in (("muro call", riga["muro_call"]), ("muro put", riga["muro_put"])):
        muro = strike + base
        for bordo, prezzo in (("VAL", v["val"]), ("VAH", v["vah"])):
            if abs(muro - prezzo) > SOGLIA_VICINO_ES:
                continue
            caso = {"giorno": g, "muro": nome, "strike_spx": strike, "muro_es": round(muro, 2), "bordo": bordo,
                    "bordo_es": prezzo, "valore_di": ieri[-1], "base": round(base, 2)}
            tocco = None
            for i, k in enumerate(chiavi):
                b = barre[k]
                if b[2] - TOCCO_ES <= muro <= b[1] + TOCCO_ES:
                    tocco = i
                    break
            if tocco is None:
                caso["esito"] = "non toccato"
                out.append(caso)
                continue
            k0 = chiavi[tocco]
            # da dove arriva il prezzo: la chiusura del minuto prima del tocco
            prima = barre.get(k0 - 60_000) or barre[k0]
            da_sopra = prima[3] > muro
            reg = [c for c in cols if c["ts"] <= k0]
            caso["tocco"] = sedute.iso(dt.datetime.fromtimestamp(k0 / 1000, dt.timezone.utc))
            caso["arriva_da"] = "sopra" if da_sopra else "sotto"
            caso["regime_al_tocco"] = None if not reg else ("positivo" if reg[-1]["netto"] > 0 else "negativo")
            # chiusure a 5 minuti nei 60 minuti dopo il tocco
            rotto = False
            for j in range(tocco + 4, min(tocco + 60, len(chiavi)), 5):
                c5 = barre[chiavi[j]][3]
                if (da_sopra and c5 <= muro - ROTTURA_ES) or (not da_sopra and c5 >= muro + ROTTURA_ES):
                    rotto = True
                    break
            caso["esito"] = "rompe" if rotto else "tiene"
            out.append(caso)
    return out


# ---------------------------------------------------------------- main
def main() -> int:
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--json", help="scrive qui le righe giorno per giorno e i riassunti")
    ap.add_argument("--vix", default=str(QUI / "5-validazione" / "risultati" / "vix-giornaliero.json"))
    a = ap.parse_args()

    giorni = sorted({p.name.split(".")[0] for p in (XRAY / "series" / "SPX" / "gex__mm_0dte").glob("*.jsonl*")})
    oggi = dt.datetime.now(sedute.NEW_YORK).date().isoformat()
    giorni = [g for g in giorni if g < oggi]          # la seduta in corso non e' finita
    barre = barre_archivio()
    ultimo = max(barre) if barre else 0
    da = dt.datetime.fromtimestamp(ultimo / 1000, dt.timezone.utc).date().isoformat() if ultimo else "2026-08-01"
    barre.update(barre_bridge(da))
    vix = vix_giornaliero(Path(a.vix))
    tutti_i_giorni = sorted({dt.datetime.fromtimestamp(k / 1000, sedute.NEW_YORK).date().isoformat() for k in barre})
    chiusure = chiusure_di_cassa(barre, tutti_i_giorni)
    valore = valore_di_cassa()

    righe, casi = [], []
    for g in giorni:
        if dt.date.fromisoformat(g).weekday() >= 5:
            continue
        cols = gex_0dte(g)
        r = una_seduta(g, barre, cols, vix, chiusure)
        if r is None:
            continue
        righe.append(r)
        if g >= "2026-10-23":
            casi += muri_al_bordo(g, r, barre, cols, valore)

    buone = [r for r in righe if "escluso" not in r]
    riass = {
        "sedute_lette": len(righe), "sedute_buone": len(buone),
        "escluse": [{"giorno": r["giorno"], "perche": r["escluso"]} for r in righe if "escluso" in r],
        "D1_regime_0935": {
            "rapporto_ampiezza_su_atteso_vix": gruppi(buone, "regime", "rapporto"),
            "nullo_mediana": permutazione(buone, "regime", "rapporto"),
            "nullo_media": permutazione(buone, "regime", "rapporto", st.fmean),
            "rapporto_su_atteso_realizzata": gruppi(buone, "regime", "rapporto_realizzata"),
            "nullo_mediana_realizzata": permutazione(buone, "regime", "rapporto_realizzata"),
            "ampiezza_punti_es": gruppi(buone, "regime", "ampiezza"),
            "rendimento_pb": gruppi(buone, "regime", "rendimento_pb"),
            "nullo_rendimento_mediana": permutazione(buone, "regime", "rendimento_pb"),
        },
        "D2_regime_0935_entro_1pct": {
            "rapporto_ampiezza_su_atteso_vix": gruppi(buone, "regime_1pct", "rapporto"),
            "nullo_mediana": permutazione(buone, "regime_1pct", "rapporto"),
            "nullo_media": permutazione(buone, "regime_1pct", "rapporto", st.fmean),
        },
        "trappola_regime_della_giornata_come_nel_video": {
            "rapporto_ampiezza_su_atteso_vix": gruppi(buone, "regime_giornata_video", "rapporto"),
            "nullo_mediana": permutazione(buone, "regime_giornata_video", "rapporto"),
            "nullo_media": permutazione(buone, "regime_giornata_video", "rapporto", st.fmean),
            "concordanza_con_0935": round(sum(1 for r in buone if r["regime"] == r["regime_giornata_video"]) / len(buone), 2)
            if buone else None,
        },
        "E_muri_al_bordo_del_valore": casi,
    }
    if a.json:
        Path(a.json).write_text(json.dumps({"riassunto": riass, "righe": righe}, ensure_ascii=False, indent=1),
                                encoding="utf-8")
    print(json.dumps(riass, ensure_ascii=False, indent=1))
    print("\nGIORNO PER GIORNO")
    for r in righe:
        if "escluso" in r:
            print(f"  {r['giorno']}  escluso: {r['escluso']}")
        else:
            print(f"  {r['giorno']}  {r['regime']:8s} netto {r['netto_0dte']:+9.1f}  (1% {r['netto_0dte_1pct']:+8.1f})  "
                  f"VIX {r['vix_ieri']:5.2f}  atteso {r['atteso_vix']:5.1f}  ampiezza {r['ampiezza']:6.2f}  "
                  f"rapporto {r['rapporto']:.2f}  rend {r['rendimento_pb']:+6.1f} pb  giornata {r['regime_giornata_video']} "
                  f"({r['quota_minuti_negativi']})")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
