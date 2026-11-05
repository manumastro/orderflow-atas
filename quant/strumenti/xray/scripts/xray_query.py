#!/usr/bin/env python3
"""Leggere, senza aprire ATAS, quello che X-Ray ha scaricato.

I file li scrive XrayLive (quant/strumenti/xray/x-ray-lettore-e-dashboard.md):

  <xray>/status.json                                  stato del lettore
  <xray>/journal.jsonl                                un chunk per riga (registro)
  <xray>/latest/<famiglia>.json                       l'ultimo chunk; "last" e' l'ultima colonna
  <xray>/series/<TICKER>/<famiglia>/<giorno>.jsonl    la giornata in corso, una riga per chunk
  <xray>/series/<TICKER>/<famiglia>/<giorno>.jsonl.gz le giornate chiuse (piu' membri gzip)

Una colonna compare una volta sola (le revisioni in coda): `colonne()` le rimette in ordine e
tiene, per ogni chiave, l'ultima versione.

usage:
  python xray_query.py status                     il lettore e' vivo?
  python xray_query.py riassunto [--json]         il punto per la lettura: livelli, profilo,
                                                  GEX dei market maker, delta per gruppo, pressione
  python xray_query.py coverage                   quali famiglie arrivano e quali sono ferme
  python xray_query.py levels                     i livelli chiave dell'ultimo istante
  python xray_query.py at 14:35 [--day AAAA-MM-GG] il profilo della gamma osservata a un'ora (UTC)
  python xray_query.py csv SPX observed gamma 2026-10-30

Tutti i comandi accettano `--dir DIR`. Gli istanti nei file sono epoch millisecondi UTC; qui si
stampano in UTC e in ora di New York.
"""
import datetime as dt
import glob
import gzip
import json
import os
import sys
from zoneinfo import ZoneInfo

DEFAULT_DIR = os.path.normpath(os.path.join(os.path.dirname(os.path.abspath(__file__)),
                                            "..", "..", "..", "dati", "xray"))
NY = ZoneInfo("America/New_York")

# quale indicatore di ATAS fa arrivare una famiglia (dedotto dai dati, non dal processo)
INDICATORS = {
    "levels": "Options Key Levels (X-Ray)",
    "observed": "Strike Profile (gamma/charm osservate)",
    "forecast": "Strike Profile (previsione)",
    "nextday": "Strike Profile (giorno dopo)",
    "gex": "GEX Heatmap / Strike Profile GEX",
    "dex": "Strike Profile DEX",
    "vex": "Strike Profile VEX",
    "pressure": "Hedge Pressure",
}

COHORTS = {
    "mm": "market maker",
    "firm": "firm (proprietari)",
    "broker": "broker-dealer",
    "procust": "clienti professionali",
    "nonprocust": "clienti non professionali (retail)",
}


# ---------------------------------------------------------------- lettura

def out_dir(argv):
    if "--dir" in argv:
        return argv[argv.index("--dir") + 1]
    return os.environ.get("XRAY_DIR", DEFAULT_DIR)


def flag(argv, name):
    if name in argv:
        index = argv.index(name)
        if index + 1 < len(argv):
            return argv[index + 1]
    return None


def load(path):
    with open(path, encoding="utf-8") as fh:
        return json.load(fh)


def righe(base):
    """Le righe di un giorno: prima il .gz (tutti i membri), poi il .jsonl in coda."""
    for path in (base + ".gz", base):
        if not os.path.exists(path):
            continue
        opener = gzip.open if path.endswith(".gz") else open
        with opener(path, "rt", encoding="utf-8") as fh:
            for line in fh:
                line = line.strip()
                if not line:
                    continue
                try:
                    yield json.loads(line)
                except json.JSONDecodeError:
                    continue          # una riga troncata non ferma la lettura


def chiave(testa, col):
    """La stessa chiave di Archivio.Chiave in XrayLive."""
    ts = int(col.get("ts", col.get("k", 0)) or 0)
    tipo = testa.get("type")
    if tipo == "strikes":
        return (ts, int(col.get("expiry") or 0), int(col.get("dte") or 0))
    if tipo == "plane":
        return ts if testa.get("metric") == "observed" else (int(testa.get("asOf") or 0), ts)
    if tipo == "levels":
        return (col.get("surface"), ts, int(col.get("until") or 0))
    if tipo == "seriesmap":
        return (ts, int(col.get("to") or 0))
    return ts


def colonne(base):
    """Le colonne di un giorno, una per chiave (l'ultima versione), in ordine di tempo."""
    per_chiave = {}
    for riga in righe(base):
        for col in riga.get("columns") or riga.get("rows") or []:
            col.setdefault("asOf", riga.get("asOf"))
            per_chiave[chiave(riga, col)] = col
    return sorted(per_chiave.values(), key=lambda c: (c.get("ts", 0), c.get("asOf") or 0))


def giorni(root, ticker, famiglia):
    """Le giornate disponibili di una famiglia, come percorsi base (senza .gz)."""
    paths = glob.glob(os.path.join(root, "series", ticker, famiglia, "*.jsonl*"))
    basi = {p[:-3] if p.endswith(".gz") else p for p in paths}
    return sorted(basi)


def latest_of(root, famiglia):
    path = os.path.join(root, "latest", famiglia + ".json")
    if not os.path.exists(path):
        return None
    try:
        return load(path)
    except (json.JSONDecodeError, OSError):
        return None


def ultima(data):
    if not data:
        return None
    return data.get("last") or ((data.get("columns") or [None])[-1])


def stamp(ms):
    return dt.datetime.fromtimestamp(ms / 1000, dt.UTC)


def quando(ms):
    """{'utc', 'new_york', 'eta_minuti'} di un istante in millisecondi."""
    if not ms:
        return None
    t = stamp(ms)
    return {
        "utc": t.strftime("%Y-%m-%d %H:%M"),
        "new_york": t.astimezone(NY).strftime("%Y-%m-%d %H:%M"),
        "eta_minuti": round((dt.datetime.now(dt.UTC) - t).total_seconds() / 60),
    }


def ricevute(root):
    """Quando e' arrivato l'ultimo chunk di ogni famiglia (dal registro, letto dalla coda)."""
    path = os.path.join(root, "journal.jsonl")
    out = {}
    if not os.path.exists(path):
        return out
    with open(path, "rb") as fh:
        fh.seek(0, os.SEEK_END)
        fh.seek(max(0, fh.tell() - 4_000_000))
        testo = fh.read().decode("utf-8", errors="replace")
    for line in testo.splitlines():
        try:
            r = json.loads(line)
        except json.JSONDecodeError:
            continue
        fam = r.get("family") or (r.get("metric", "") + ("__" + r["variant"].replace("/", "_") if r.get("variant") else ""))
        if r.get("seenUtc"):
            out[fam] = r["seenUtc"]
    return out


def ultime_superfici(root):
    """L'ultima superficie dei livelli per tipo (observed, outlook), in ordine di arrivo."""
    basi = giorni(root, "SPX", "levels__gamma")
    per_tipo = {}
    if basi:
        for riga in righe(basi[-1]):
            for col in riga.get("columns") or []:
                per_tipo[col.get("surface")] = col
    if not per_tipo:
        lev = latest_of(root, "levels__gamma") or {}
        for col in lev.get("columns") or []:
            per_tipo[col.get("surface")] = col
    return [per_tipo[k] for k in ("observed", "outlook") if k in per_tipo]


# ---------------------------------------------------------------- misure

def livelli_di(col):
    """Una superficie dei livelli chiave in parole: zeri (flip), picchi positivi e negativi.

    kind 0 = dove la gamma del profilo cambia segno (prezzo, per istante);
    kind 1 = picco positivo (strike per indice in `prices`, con il peso);
    kind 2 = picco negativo. Verificato il 30-10-2026 contro observed/forecast gamma."""
    prezzi = col.get("prices") or []
    out = {"superficie": col.get("surface"), "da": quando(col.get("ts")), "fino_a": quando(col.get("until")),
           "zeri": [], "picchi_positivi": [], "picchi_negativi": []}
    for tr in col.get("tracks") or []:
        kind = int(tr.get("kind") or 0)
        if kind == 0:
            valori = [p for p in tr.get("price") or [] if p is not None]
            if valori:
                out["zeri"].append(round(valori[-1], 2))
        else:
            rows, pesi = tr.get("row") or [], tr.get("weight") or []
            if not rows:
                continue
            i = rows[-1]
            voce = {"strike": prezzi[i] if 0 <= i < len(prezzi) else None,
                    "peso": round(pesi[-1], 1) if pesi else None}
            (out["picchi_positivi"] if kind == 1 else out["picchi_negativi"]).append(voce)
    out["zeri"].sort()
    out["picchi_positivi"].sort(key=lambda v: -(v["peso"] or 0))
    out["picchi_negativi"].sort(key=lambda v: v["peso"] or 0)
    return out


def profilo_di(col, spot):
    """Il profilo della gamma osservata: segno allo spot, zeri vicini, strike piu' pesanti."""
    coppie = [(s, v) for s, v in zip(col.get("prices") or [], col.get("values") or []) if v is not None]
    if not coppie:
        return None
    coppie.sort()
    zeri = [round((a[0] + b[0]) / 2, 2) for a, b in zip(coppie, coppie[1:]) if a[1] * b[1] < 0]
    vicino = min(coppie, key=lambda sv: abs(sv[0] - spot)) if spot else None
    return {
        "istante": quando(col.get("ts")),
        "strike": len(coppie),
        "da_a": [coppie[0][0], coppie[-1][0]],
        "allo_spot": {"strike": vicino[0], "gamma": round(vicino[1], 1)} if vicino else None,
        "zeri_vicini": sorted(zeri, key=lambda z: abs(z - spot))[:4] if spot else zeri[:4],
        "positivi": [{"strike": s, "gamma": round(v, 1)} for s, v in sorted(coppie, key=lambda sv: -sv[1])[:4]],
        "negativi": [{"strike": s, "gamma": round(v, 1)} for s, v in sorted(coppie, key=lambda sv: sv[1])[:4]],
    }


def per_strike(col):
    """GEX/DEX/VEX per strike: totali, netto vicino allo spot, strike piu' pesanti."""
    S = col.get("strikes") or []
    C = [c or 0 for c in col.get("call") or []]
    P = [p or 0 for p in col.get("put") or []]
    spot = col.get("spot") or 0
    netti = [(s, c + p, c, p) for s, c, p in zip(S, C, P)]
    vicini = [n for n in netti if spot and abs(n[0] - spot) <= spot * 0.01]
    return {
        "istante": quando(col.get("ts")),
        "spot": spot,
        "scadenza": quando(col.get("expiry")) if col.get("expiry") else "tutte",
        "giorni": col.get("dte"),
        "call": round(sum(C), 1),
        "put": round(sum(P), 1),
        "netto": round(sum(C) + sum(P), 1),
        "netto_entro_1pct": round(sum(n[1] for n in vicini), 1),
        "piu_pesanti": [{"strike": s, "netto": round(n, 1), "call": round(c, 1), "put": round(p, 1)}
                        for s, n, c, p in sorted(netti, key=lambda x: -abs(x[1]))[:6]],
    }


def riassunto_dati(root):
    """Tutto quello che serve alla lettura, gia' scelto. E' il blocco `xray` del collettore."""
    out = {"fonte": "OptionsDepth via ATAS X-Ray, su SPX (non ES, non NQ); stima del fornitore, metodo non pubblicato",
           "cartella": root}

    # il lettore
    st_path = os.path.join(root, "status.json")
    if os.path.exists(st_path):
        st = load(st_path)
        last = st.get("lastSweepUtc")
        eta = None
        if last:
            eta = (dt.datetime.now(dt.UTC) - dt.datetime.fromisoformat(last.replace("Z", "+00:00"))).total_seconds()
        out["lettore"] = {"vivo": eta is not None and eta < 180, "ultimo_giro_secondi_fa": round(eta) if eta is not None else None,
                          "chunk": st.get("chunks"), "errori": st.get("errors"), "ultimo_errore": st.get("lastError"),
                          "archivio_mb": st.get("archiveMB")}
    else:
        out["lettore"] = {"vivo": False, "nota": "status.json assente: il lettore non ha mai girato"}

    # la freschezza per famiglia
    rec = ricevute(root)
    now = dt.datetime.now(dt.UTC)
    out["ricevute_minuti_fa"] = {
        fam: round((now - dt.datetime.fromisoformat(s.replace("Z", "+00:00"))).total_seconds() / 60)
        for fam, s in sorted(rec.items())}

    # lo spot: l'ultimo della GEX dei market maker (fuori dalla cassa resta fermo alla chiusura)
    gex0 = ultima(latest_of(root, "gex__mm_0dte"))
    spot = (gex0 or {}).get("spot")
    out["spot_spx"] = {"valore": spot, "istante": quando((gex0 or {}).get("ts")),
                       "nota": "fuori dalla cassa (09:30-16:00 New York) e' la chiusura precedente, ferma"}

    # i livelli chiave: l'ultima superficie osservata e l'ultima prevista, dalla giornata piu'
    # recente (l'ultimo chunk da solo ne porta spesso una sola)
    out["livelli"] = [livelli_di(c) for c in ultime_superfici(root)]

    # il profilo della gamma osservata (dei market maker, stimata)
    obs = ultima(latest_of(root, "observed__gamma"))
    if obs:
        out["profilo_gamma"] = profilo_di(obs, spot)

    # la GEX dei market maker per scadenza
    out["gex_market_maker"] = {}
    for scope in ("0dte", "next_expiry", "opex"):
        col = ultima(latest_of(root, f"gex__mm_{scope}"))
        if col:
            out["gex_market_maker"][scope] = per_strike(col)

    # il delta per gruppo di partecipanti. Non e' una posizione: ogni colonna copre una finestra
    # [from, ts] di 60 secondi ed e' il delta scambiato dal gruppo in quel minuto (verificato il
    # 2 novembre 2026). Il numero che si legge e' la somma dall'inizio della giornata.
    out["dex_per_gruppo"] = {}
    for cohort, nome in COHORTS.items():
        for scope in ("0dte", "all"):
            fam = f"dex__{cohort}_{scope}"
            testa = latest_of(root, fam)
            col = ultima(testa)
            if not col:
                continue
            giorno = (testa or {}).get("session")
            cols = colonne(os.path.join(root, "series", "SPX", fam, f"{giorno}.jsonl")) if giorno else [col]
            netti = [(c["ts"], sum(x or 0 for x in c.get("call") or []) + sum(x or 0 for x in c.get("put") or []))
                     for c in cols if c["ts"] <= col["ts"]]
            out["dex_per_gruppo"].setdefault(cohort, {"chi": nome})[scope] = {
                "istante": quando(col["ts"]),
                "giornata": giorno,
                "accumulato_oggi": round(sum(v for _, v in netti), 2),
                "ultimi_30_minuti": round(sum(v for t, v in netti if t > col["ts"] - 30 * 60_000), 2),
                "ultimo_minuto": round(netti[-1][1], 3) if netti else None,
            }

    # la pressione di copertura (Hedge Pressure, 0DTE): l'ultimo minuto e la mezz'ora prima
    pr = latest_of(root, "pressure__0dte")
    if pr and pr.get("columns"):
        cols = pr["columns"]
        fine = cols[-1]
        prima = next((c for c in reversed(cols) if c["ts"] <= fine["ts"] - 30 * 60_000), cols[0])
        out["pressione_0dte"] = {"istante": quando(fine["ts"]), "delta": round(fine.get("delta") or 0, 1),
                                 "delta_30_minuti_prima": round(prima.get("delta") or 0, 1),
                                 "spot": fine.get("spot")}

    out["unita"] = ("GEX, DEX e VEX nelle unita' di OptionsDepth (non dichiarate; non confrontabili in assoluto "
                    "con la nostra GEX in $ per 1%). GEX e VEX: il segno e' quello della posizione stimata dei "
                    "market maker. DEX: e' un flusso al minuto, qui sommato dall'inizio della giornata "
                    "(positivo = il gruppo ha comprato delta)")
    return out


def righe_giro(root=DEFAULT_DIR):
    """Le tre o quattro righe che il giro d'orizzonte stampa nella tab 2, sotto la nostra GEX."""
    r = riassunto_dati(root)
    l = r["lettore"]
    if not l.get("vivo"):
        quando_ = f"ultimo giro {l['ultimo_giro_secondi_fa'] // 60} min fa" if l.get("ultimo_giro_secondi_fa") else "mai avviato"
        return [f"X-Ray: lettore FERMO ({quando_}); i numeri sotto possono essere vecchi"] + _righe_numeri(r)
    fresche = [v for v in r["ricevute_minuti_fa"].values()]
    ultimo = min(fresche) if fresche else None
    testa = f"X-Ray (OptionsDepth su SPX, stima del fornitore; per NQ solo contesto): ultimo chunk {ultimo} min fa"
    return [testa] + _righe_es() + _righe_numeri(r)


_LIVELLI_ES: dict = {}


def livelli_es(url="http://127.0.0.1:8790/api/adesso"):
    """Con una memoria di 30 secondi: il giro la chiede due volte (riga ES e quattro domande), e /api/adesso
    impiega un paio di secondi."""
    import time as _time
    m = _LIVELLI_ES.get(url)
    if m and _time.time() - m[0] < 30:
        return m[1]
    v = _livelli_es(url)
    _LIVELLI_ES[url] = (_time.time(), v)
    return v


def _livelli_es(url="http://127.0.0.1:8790/api/adesso"):
    """I livelli di X-Ray in prezzi di ES, come li disegna ATAS sul chart di ES (5 novembre 2026): il prezzo di
    riferimento (lo spot SPX in cassa, fuori ES meno la base), la gamma al prezzo, i flip, il freno e
    l'acceleratore piu' forti, tutti piu' la base ES-SPX. Dal server della pagina; None se non risponde."""
    import json as _json
    import urllib.request
    try:
        with urllib.request.urlopen(url, timeout=5) as f:
            a = _json.load(f)
    except Exception:
        return None
    b = a.get("base") or {}
    base = b.get("valore")
    rif = a.get("riferimento") or {}
    if base is None or rif.get("prezzo") is None:
        return None
    es = round(rif["prezzo"] + base, 2)
    g = a.get("gamma") or {}
    pos = a.get("picchiPositivi") or []
    neg = a.get("picchiNegativi") or []
    out = a.get("outlook") or {}
    return {
        "fonte": "X-Ray (OptionsDepth su SPX) via ATAS, piu' la base ES-SPX; prevale su ogni altra GEX di ES",
        "base": base, "base_giorno": b.get("giorno"), "base_fonte": b.get("fonte"),
        "prezzo_es": es, "spx_vivo": bool(rif.get("vivo")),
        "gamma_al_prezzo": g.get("valore"), "regime": g.get("regime"),
        "flip": sorted((round(f + base, 2) for f in a.get("flip") or []), key=lambda x: abs(x - es)),
        "freno_piu_forte": round(pos[0]["strike"] + base, 2) if pos else None,
        "acceleratore_piu_forte": round(neg[0]["strike"] + base, 2) if neg else None,
        "prossima_seduta_flip": [round(z + base, 2) for z in out.get("zeri") or []],
    }


def _righe_es():
    """La riga del giro con X-Ray in prezzi di ES."""
    l = livelli_es()
    if not l:
        return []
    def p(x):
        return f"{x:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
    es = l["prezzo_es"]
    parti = [f"ES {p(es)}" + ("" if l["spx_vivo"] else " (SPX ricostruito da ES fuori cassa)")]
    if l["gamma_al_prezzo"] is not None:
        parti.append(f"gamma al prezzo {l['gamma_al_prezzo']:+.0f} ({l['regime'] or ('freno' if l['gamma_al_prezzo'] >= 0 else 'acceleratore')})")
    if l["flip"]:
        parti.append("flip " + ", ".join(f"{p(f)} ({f - es:+.0f})" for f in l["flip"]))
    if l["freno_piu_forte"] is not None:
        parti.append(f"freno piu' forte {p(l['freno_piu_forte'])}")
    if l["acceleratore_piu_forte"] is not None:
        parti.append(f"acceleratore piu' forte {p(l['acceleratore_piu_forte'])}")
    return [f"  in prezzi di ES (base {l['base']:.2f}, {l['base_giorno'] or '?'}): " + "  ·  ".join(parti)]


def _righe_numeri(r):
    out = []
    sp = r["spot_spx"]
    pg = r.get("profilo_gamma") or {}
    parti = []
    if sp.get("valore"):
        parti.append(f"in SPX: spot {sp['valore']} ({sp['istante']['new_york'][11:]} NY)")
    if pg.get("allo_spot"):
        g = pg["allo_spot"]["gamma"]
        parti.append(f"gamma allo spot {g:+.0f} ({'freno' if g >= 0 else 'acceleratore'})")
    adesso = next((s for s in r.get("livelli") or [] if s["superficie"] == "observed"), None)
    if adesso:
        parti.append(f"zeri {', '.join(f'{z:.0f}' for z in adesso['zeri']) or '-'}")
        parti.append(f"picchi + {', '.join(str(p['strike']) for p in adesso['picchi_positivi']) or '-'}")
    if parti:
        out.append("  " + "  ·  ".join(parti))
    gm = r.get("gex_market_maker") or {}
    if gm:
        out.append("  GEX dei market maker (segno = lunghi o corti di gamma, stimato): " + "  ".join(
            f"{k} {v['netto']:+.0f}" for k, v in gm.items()))
    v = riga_volatilita()
    if v:
        out.append(v)
    return out


# ---------------------------------------------------------------- le opzioni in quattro domande
# Dal video sul bias dalle opzioni (5 novembre 2026; docs/research/fonti-video/2026-11-05-il-bias-dalle-opzioni.txt,
# [V2 · 0:20-0:47]): il regime, il meteo, i muri sul profilo, il grilletto. Come si leggono e quanto sono provate:
# docs/research/metodo/il-bias-del-giorno.md e quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md. Il codice mette
# in fila i numeri; dire cosa significano per la giornata resta alla lettura, senza segnali.

# La regola delle quattro domande sta nel server della pagina (XrayLive/Server/QuattroDomande.cs, dal 5 novembre 2026):
# una regola sola in un posto solo. Qui si legge /api/quattro-domande e si traduce la risposta nel dizionario di prima,
# con le stesse chiavi, cosi' il giro (le_quattro_tab.py), lo studio (studio/raccogli.py, quattro-domande-<strumento>.json)
# e le skill che lo leggono non cambiano. XRAY_SERVER sceglie un altro server (per le prove: http://127.0.0.1:8791).
SERVER = os.environ.get("XRAY_SERVER", "http://127.0.0.1:8790").rstrip("/")


def quattro_domande(radice, bordi=None):
    """Le quattro domande sulle opzioni per NQ (GEX viva della catena CME, convenzione A) o ES (X-Ray, in prezzi
    di ES), lette da /api/quattro-domande del server della pagina. Restituisce un dizionario: regime, meteo, muri
    vicini ai bordi, grilletto; le chiavi sono quelle di prima del 5 novembre 2026, piu' `bordi` e `nota` nei muri.
    `bordi` resta nella firma ma SI IGNORA: i bordi del valore (VAL, VAH, POC delle casse, muri del ping-pong) li
    prende il server dal bridge. Se il server non risponde: {"errore": ...}."""
    import urllib.request
    radice = radice.upper()[:2]
    try:
        with urllib.request.urlopen(f"{SERVER}/api/quattro-domande?radice={radice}", timeout=90) as f:
            q = json.load(f)
    except Exception:
        return {"radice": radice, "errore": "server senza /api/quattro-domande"}
    rg_s, mu_s = q.get("regime") or {}, q.get("muri") or {}
    val = rg_s.get("valori") or {}
    out = {"radice": radice, "fonte": q.get("fonte")}
    if radice == "ES":
        rg = {"fonte": rg_s.get("fonte"), "prezzo": q.get("prezzo"), "gamma_al_prezzo": val.get("alPrezzo"),
              "al_prezzo": rg_s.get("alPrezzo"), "zero_dte": rg_s.get("zeroDte"),
              "gex_market_maker": {k: round(v, 1) for k, v in (rg_s.get("gexPerScadenza") or {}).items() if v is not None},
              "flip_piu_vicino": rg_s.get("flipPiuVicino")}
    else:
        zero = rg_s.get("zero") or {}
        rg = {"fonte": rg_s.get("fonte"), "prezzo": q.get("prezzo"), "gex_tutte": val.get("alPrezzo"),
              "zero_tutte": zero.get("tutte"), "gex_0dte": val.get("zeroDte"), "zero_0dte": zero.get("zeroDte"),
              "al_prezzo": rg_s.get("alPrezzo"), "zero_dte": rg_s.get("zeroDte")}
    if rg_s.get("frase"):
        rg["lettura"] = rg_s["frase"]
    rg["provato"] = rg_s.get("provato")
    out["regime"] = rg
    me = q.get("meteo") or {}
    out["meteo"] = {k: me.get(k) for k in ("nomeIndice", "livello", "fascia", "movimentoOggi", "movimentoGiorno", "realizzata20",
                                           "implicitaMenoRealizzata", "implicitaTesto", "pendenza", "struttura", "skew", "skewTesto")}
    soglia = mu_s.get("sogliaPunti")
    out["muri_sul_profilo"] = {
        "soglia_punti": float(soglia) if soglia is not None else None,
        "vicini": [{"muro": v["muro"], "prezzo": v["prezzo"], "bordo": v["bordo"], "prezzo_bordo": v["prezzoBordo"],
                    "distanza": v["distanza"]} for v in mu_s.get("vicini") or []],
        "muri": [{"muro": m["muro"], "prezzo": m["prezzo"], "dal_prezzo": m.get("dalPrezzo")} for m in mu_s.get("muri") or []],
        "bordi": [(b["nome"], b["prezzo"]) for b in mu_s.get("bordi") or []],
        "nota": mu_s.get("nota"),
    }
    out["grilletto"] = q.get("grilletto")
    if q.get("errori"):
        out["errori"] = q["errori"]
    return out


def righe_quattro_domande(q):
    """Le righe del giro per le quattro domande."""
    def p(x):
        if x is None:
            return "-"
        s_ = f"{x:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
        return s_[:-3] if s_.endswith(",00") else s_
    rad = q.get("radice", "?")
    if q.get("errore"):
        return [f"LE OPZIONI IN QUATTRO DOMANDE su {rad}: {q['errore']}"]
    rg, me, mu = q.get("regime") or {}, q.get("meteo") or {}, q.get("muri_sul_profilo") or {}
    def v1(x):
        return "-" if x is None else f"{x:+.1f}".replace(".", ",")
    out = [f"LE OPZIONI IN QUATTRO DOMANDE su {rad} (dal video del 5/11; contesto, non segnali)"]
    if rad == "ES":
        gm = rg.get("gex_market_maker") or {}
        det = (f"gamma al prezzo {rg['gamma_al_prezzo']:+.0f}" if rg.get("gamma_al_prezzo") is not None else "gamma al prezzo -")
        det += "  ·  GEX market maker " + " ".join(f"{k} {v:+.0f}" for k, v in gm.items() if v is not None)
        fl = rg.get("flip_piu_vicino")
        if fl is not None and rg.get("prezzo"):
            det += f"  ·  flip piu' vicino {p(fl)} ({fl - rg['prezzo']:+.0f})"
    elif rg.get("gex_tutte") is not None and rg.get("gex_0dte") is not None:
        det = (f"GEX tutte {rg['gex_tutte']:+.0f} (zero {p(rg.get('zero_tutte'))})  ·  "
               f"0DTE {rg['gex_0dte']:+.0f} (zero {p(rg.get('zero_0dte'))})")
    else:
        det = "GEX viva assente"
    out.append(f"  1 IL REGIME   {det}")
    if rg.get("lettura"):
        out.append(f"                -> {rg['lettura']}")
    out.append(f"                ({rg.get('provato')})")
    pz = []
    if me.get("movimentoOggi") is not None:
        pz.append(f"movimento atteso oggi +-{p(me['movimentoOggi'])}")
    if me.get("implicitaMenoRealizzata") is not None:
        pz.append(f"implicita - realizzata {v1(me['implicitaMenoRealizzata'])} ({me.get('implicitaTesto')}: "
                  f"{me.get('nomeIndice')} {p(me.get('livello'))} contro {p(me.get('realizzata20'))} vera in 20 sedute)")
    if me.get("skew") is not None:
        pz.append(f"skew {v1(me['skew'])} ({me.get('skewTesto')})")
    if me.get("struttura"):
        pz.append(f"breve contro mese {v1(me.get('pendenza'))} ({me['struttura']})")
    out.append("  2 IL METEO    " + ("  ·  ".join(pz) if pz else "pagina della volatilita' non raggiungibile"))
    if mu.get("vicini"):
        out.append(f"  3 I MURI SUL PROFILO (entro {p(mu['soglia_punti'])} punti da un bordo del valore):")
        for v in mu["vicini"]:
            out.append(f"       {v['muro']} {p(v['prezzo'])}  ~  {v['bordo']} {p(v['prezzo_bordo'])}  ({v['distanza']:+.2f})")
    else:
        vicini = sorted((m for m in mu.get("muri") or [] if m.get("dal_prezzo") is not None), key=lambda m: abs(m["dal_prezzo"]))[:3]
        out.append(f"  3 I MURI SUL PROFILO   nessuno entro {p(mu.get('soglia_punti'))} punti da un bordo del valore; i piu' vicini al prezzo: "
                   + ", ".join(f"{m['muro']} {p(m['prezzo'])} ({m['dal_prezzo']:+.0f})" for m in vicini))
    out.append(f"  4 IL GRILLETTO   {q.get('grilletto')}")
    return out


def ultimi_volatilita(dati=None):
    """L'ultimo valore di ogni serie della volatilita' (quant/dati/volatilita, scritte dal server della
    pagina dal 2 novembre 2026): {id: (ts_ms, valore)}, cercando fino a 7 giorni indietro."""
    dati = dati or os.path.join(os.path.dirname(DEFAULT_DIR), "volatilita")
    out = {}
    if not os.path.isdir(dati):
        return out
    for giorno in sorted(os.listdir(dati), reverse=True)[:7]:
        path = os.path.join(dati, giorno, "serie.jsonl")
        if not os.path.exists(path):
            continue
        with open(path, encoding="utf-8-sig") as f:
            for riga in f:
                try:
                    x = json.loads(riga)
                except ValueError:
                    continue
                if x["id"] not in out or x["ts"] > out[x["id"]][0]:
                    out[x["id"]] = (x["ts"], x["v"])
    return out


def riga_volatilita():
    """Una riga per il giro: VIX e VXN ricostruiti dalla catena CME di ATAS, e la stessa misura dal Cboe."""
    u = ultimi_volatilita()
    adesso = dt.datetime.now(dt.timezone.utc).timestamp() * 1000
    parti = []
    for chiave, nome in (("vix-catena", "VIX ricostruito (ES)"), ("vxn-catena", "VXN ricostruito (NQ)"),
                         ("vol30-ndx", "VXN da NDX, Cboe"), ("vol30-qqq", "QQQ 30g, Cboe")):
        if chiave in u:
            ts, v = u[chiave]
            minuti = (adesso - ts) / 60000
            eta = f"{minuti:.0f} min fa" if minuti < 120 else f"{minuti / 60:.0f} h fa" if minuti < 2880 else f"{minuti / 1440:.0f} g fa"
            parti.append(f"{nome} {v:.2f} ({eta})")
    return ("  volatilita' a 30 giorni, formula del Cboe (non e' l'indice ufficiale): " + "  ·  ".join(parti)) if parti else None


def gex_viva(radice="NQ", dati=None, finestra_min=60):
    """La GEX viva di una radice (NQ o ES), dall'archivio che il server della pagina scrive ogni minuto
    (quant/dati/volatilita/<giorno NY>/gex-<radice>.jsonl, dal 2 novembre 2026): l'ultimo punto e come sono
    cambiati prezzo, zero gamma e GEX nell'ultima ora. Come si legge: quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md.
    Convenzione A (dealer lunghi call, corti put), milioni di $ per 1%, OI di ieri."""
    dati = dati or os.path.join(os.path.dirname(DEFAULT_DIR), "volatilita")
    if not os.path.isdir(dati):
        return None
    punti = []
    for giorno in sorted(os.listdir(dati), reverse=True)[:4]:
        path = os.path.join(dati, giorno, f"gex-{radice}.jsonl")
        if os.path.exists(path):
            with open(path, encoding="utf-8-sig") as f:
                for riga in f:
                    try:
                        punti.append(json.loads(riga))
                    except ValueError:
                        continue
            if punti:
                break
    if not punti:
        return None
    punti.sort(key=lambda x: x["ts"])
    u = punti[-1]
    prima = [x for x in punti if x["ts"] <= u["ts"] - finestra_min * 60000]
    p0 = prima[-1] if prima else punti[0]
    adesso = dt.datetime.now(dt.timezone.utc).timestamp() * 1000
    # i muri che hanno cambiato strike nella finestra: due strike quasi alla pari si scambiano il ruolo
    finestra = [x for x in punti if x["ts"] >= p0["ts"]]
    muri = {k: sorted({x[k] for x in finestra if x.get(k) is not None}) for k in ("callWall", "putWall", "callWall0", "putWall0")}

    def lato(prezzo, zero):
        return None if zero is None or prezzo is None else ("sopra" if prezzo >= zero else "sotto")

    return {
        "radice": radice, "fonte": "catena CME via ATAS, GEX viva del server della pagina (XrayLive)",
        "convenzione": "A (dealer lunghi call, corti put)", "unita": "milioni di $ per 1%", "oi": "di ieri",
        "ultimo": u, "ultimo_ny": quando(u["ts"])["new_york"], "secondi_fa": round((adesso - u["ts"]) / 1000),
        "punti_oggi": len(punti),
        "prezzo_rispetto_allo_zero": lato(u.get("future"), u.get("zero")),
        "prezzo_rispetto_allo_zero_0dte": lato(u.get("future"), u.get("zero0")),
        "finestra_minuti": round((u["ts"] - p0["ts"]) / 60000),
        "variazione": {k: (round(u[k] - p0[k], 2) if u.get(k) is not None and p0.get(k) is not None else None)
                       for k in ("future", "zero", "totale", "zero0", "totale0")},
        "muri_nella_finestra": muri,
    }


def righe_gex_viva():
    """Le righe del giro sulla GEX viva di NQ, ricalcolata ogni minuto dal server. Su ES/SPX dal 5 novembre
    2026 vale solo X-Ray (OptionsDepth), come sul chart di ATAS: la nostra catena ES non si stampa."""
    def n(x, f):  # un numero che puo' mancare (lo 0DTE tutto di un segno non ha zero gamma): "-"
        return "-" if x is None else format(x, f)
    out = []
    for radice in ("NQ",):
        g = gex_viva(radice)
        if not g:
            continue
        u, v = g["ultimo"], g["variazione"]
        eta = f"{g['secondi_fa'] // 60} min fa" if g["secondi_fa"] < 7200 else f"{g['secondi_fa'] // 3600} h fa (FERMA)"
        salti = [f"{k} fra {', '.join(f'{m:.0f}' for m in s)}" for k, s in g["muri_nella_finestra"].items() if len(s) > 1]
        out.append(
            f"  GEX viva {radice} ({g['ultimo_ny'][11:16]} NY, {eta}): tutte {n(u['totale'], '+.0f')} zero {n(u['zero'], '.0f')} "
            f"(prezzo {g['prezzo_rispetto_allo_zero']})  muri {n(u['callWall'], '.0f')}/{n(u['putWall'], '.0f')}  ·  "
            f"0DTE {n(u['totale0'], '+.0f')} zero {n(u['zero0'], '.0f')} (prezzo {g['prezzo_rispetto_allo_zero_0dte'] or 'senza zero: tutta di un segno'})")
        if g["finestra_minuti"] >= 10:
            out.append(
                f"    in {g['finestra_minuti']} min: prezzo {n(v['future'], '+.0f')}, zero {n(v['zero'], '+.0f')}, GEX {n(v['totale'], '+.0f')}, "
                f"0DTE {n(v['totale0'], '+.0f')}" + (f"  ·  muri saltati: {'; '.join(salti)}" if salti else ""))
    return out


# ---------------------------------------------------------------- comandi

def status(root):
    path = os.path.join(root, "status.json")
    if not os.path.exists(path):
        print(f"nessuno stato in {path}: il lettore non ha mai girato")
        return 1
    s = load(path)
    print(f"lettore    : pid {s['pid']}, avviato {s['startedUtc']}")
    print(f"ultimo giro: {s['lastSweepUtc']}")
    print(f"chunk      : {s['chunks']}   errori {s['errors']}   ultimo {s['lastChunk']}")
    if "columnsWritten" in s:
        print(f"colonne    : {s['columnsWritten']} scritte, {s['columnsDuplicate']} doppie saltate, "
              f"{s['columnsRevised']} revisioni")
        print(f"archivio   : {s.get('archiveMB')} MB, {s.get('archiveDays')} giornate")
    if s.get("lastError"):
        print(f"ultimo errore: {s['lastError']}")
    return 0


def riassunto(root, as_json):
    r = riassunto_dati(root)
    if as_json:
        print(json.dumps(r, ensure_ascii=False, indent=1))
        return 0
    l = r["lettore"]
    print(f"X-Ray su SPX ({r['fonte']})")
    print(f"lettore: {'vivo' if l.get('vivo') else 'FERMO'}  ultimo giro {l.get('ultimo_giro_secondi_fa')} s fa  "
          f"errori {l.get('errori')}")
    sp = r["spot_spx"]
    if sp["valore"]:
        print(f"spot SPX {sp['valore']}  ({sp['istante']['new_york']} New York)")
    for sup in r.get("livelli") or []:
        print(f"livelli {sup['superficie']:<8} zeri {sup['zeri']}  "
              f"picchi + {[(p['strike'], p['peso']) for p in sup['picchi_positivi']]}  "
              f"picchi - {[(p['strike'], p['peso']) for p in sup['picchi_negativi']]}")
    pg = r.get("profilo_gamma")
    if pg:
        print(f"gamma osservata ({pg['istante']['new_york']} NY): allo spot {pg['allo_spot']}, zeri vicini {pg['zeri_vicini']}")
    for scope, g in r["gex_market_maker"].items():
        print(f"GEX market maker {scope:<12} netto {g['netto']:>9}  (call {g['call']}, put {g['put']})  "
              f"entro 1% {g['netto_entro_1pct']}  pesanti {[(x['strike'], x['netto']) for x in g['piu_pesanti'][:3]]}")
    print("DEX accumulato oggi (flusso sommato dall'inizio della giornata; positivo = ha comprato delta):")
    for cohort, d in r["dex_per_gruppo"].items():
        parti = "  ".join(f"{s} {v['accumulato_oggi']:>10} (30 min {v['ultimi_30_minuti']:+})" for s, v in d.items() if s != "chi")
        print(f"  {d['chi']:<36} {parti}")
    p = r.get("pressione_0dte")
    if p:
        print(f"pressione 0DTE {p['delta']} (30 minuti prima {p['delta_30_minuti_prima']}) {p['istante']['new_york']} NY")
    print("\nfreschezza (minuti dall'ultimo chunk):",
          ", ".join(f"{k} {v}" for k, v in sorted(r["ricevute_minuti_fa"].items(), key=lambda kv: kv[1])[:8]), "...")
    return 0


def coverage(root):
    """Cosa e' fresco e cosa e' fermo: dice quali indicatori tenere aperti."""
    files = sorted(glob.glob(os.path.join(root, "latest", "*.json")))
    if not files:
        print("nessun chunk pubblicato")
        return 1
    now = dt.datetime.now(dt.UTC)
    rec = ricevute(root)
    print(f"{'famiglia':<24}{'ricevuto':<11}{'istante dentro':<18}{'giornate':>9}   indicatore che lo chiede")
    for path in files:
        name = os.path.basename(path)[:-5]
        col = ultima(latest_of(root, name)) or {}
        seen = rec.get(name)
        if seen:
            hours = (now - dt.datetime.fromisoformat(seen.replace("Z", "+00:00"))).total_seconds() / 3600
            age = f"{hours * 60:.0f} min" if hours < 1 else f"{hours:.1f} h" if hours < 48 else f"{hours / 24:.1f} g"
        else:
            age = "?"
        moment = stamp(col["ts"]) if col.get("ts") else None
        inside = f"{moment:%Y-%m-%d %H:%M}" + (" fut" if moment > now else "") if moment else "-"
        n = len(giorni(root, "SPX", name))
        print(f"{name:<24}{age:<11}{inside:<18}{n:>9}   {INDICATORS.get(name.split('__')[0], '?')}")
    print("\nLe famiglie ferme ripartono aprendo l'indicatore corrispondente su un chart ES;")
    print("la storia gia' scaricata resta in quant/dati/xray anche a indicatori chiusi.")
    return 0


def levels(root):
    superfici = ultime_superfici(root)
    if not superfici:
        print("nessun levels__gamma pubblicato")
        return 1
    for col in superfici:
        sup = livelli_di(col)
        print(f"{sup['superficie']}: {sup['da']['new_york'] if sup['da'] else '-'} -> "
              f"{sup['fino_a']['new_york'] if sup['fino_a'] else '-'} (New York)")
        print(f"  zeri (flip)      {sup['zeri']}")
        print(f"  picchi positivi  {sup['picchi_positivi']}")
        print(f"  picchi negativi  {sup['picchi_negativi']}")
    return 0


def profile(root, clock, day):
    basi = giorni(root, "SPX", "observed__gamma")
    if not basi:
        print("nessuna serie observed__gamma per SPX")
        return 1
    base = os.path.join(os.path.dirname(basi[-1]), day + ".jsonl") if day else basi[-1]
    cols = colonne(base)
    if not cols:
        print(f"{base}: vuoto")
        return 1
    col = cols[-1]
    if clock:
        hours, minutes = (int(x) for x in clock.split(":"))
        target = stamp(col["ts"]).replace(hour=hours, minute=minutes)
        col = min(cols, key=lambda c: abs(stamp(c["ts"]) - target))
    p = profilo_di(col, None)
    print(f"SPX gamma osservata  {stamp(col['ts']):%Y-%m-%d %H:%M} UTC  strike {p['strike']}  zeri {p['zeri_vicini']}")
    for v in p["positivi"] + p["negativi"]:
        print(f"  {v['strike']:>9.1f}  {v['gamma']:>10,.1f}")
    return 0


def csv(root, ticker, metric, variant, day):
    famiglia = f"{metric}__{variant}" if variant else metric
    base = os.path.join(root, "series", ticker, famiglia, day + ".jsonl")
    cols = colonne(base)
    if not cols:
        print(f"niente in {base}(.gz)")
        return 1
    prices = cols[0].get("prices") or []
    if not prices:
        print("colonne senza prices (per le GEX per strike usa riassunto o il JSON)")
        return 1
    print("strike," + ",".join(stamp(c["ts"]).strftime("%Y-%m-%dT%H:%M") for c in cols))
    for index, strike in enumerate(prices):
        row = []
        for col in cols:
            values = col.get("values") or []
            value = values[index] if index < len(values) else None
            row.append("" if value is None else f"{value:.6g}")
        print(f"{strike}," + ",".join(row))
    return 0


def main(argv):
    if len(argv) < 2:
        print(__doc__)
        return 2
    root = out_dir(argv)
    command = argv[1]
    if command == "status":
        return status(root)
    if command == "riassunto":
        return riassunto(root, "--json" in argv)
    if command == "latest":           # vecchio nome
        return coverage(root)
    if command == "at":
        return profile(root, flag(argv, "--at") or (argv[2] if len(argv) > 2 and not argv[2].startswith("--") else None),
                       flag(argv, "--day"))
    if command == "levels":
        return levels(root)
    if command in ("coverage", "copertura"):
        return coverage(root)
    if command == "csv":
        rest = [a for a in argv[2:] if not a.startswith("--")]
        if len(rest) < 4:
            print('csv <TICKER> <metrica> <variante> <giorno>   (variante vuota: "" )')
            return 2
        variant = "" if rest[2] == '""' else rest[2]
        return csv(root, rest[0], rest[1], variant, rest[3])
    print(__doc__)
    return 2


if __name__ == "__main__":
    try:
        sys.stdout.reconfigure(encoding="utf-8")
        sys.exit(main(sys.argv))
    except BrokenPipeError:
        # `| head` chiude il tubo: non e' un errore
        try:
            sys.stdout.close()
        finally:
            os._exit(0)
