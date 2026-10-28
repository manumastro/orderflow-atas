#!/usr/bin/env python3
"""La fotografia macro per chi guarda NQ: tassi, volatilita', dollaro, materie prime, calendario,
trimestrali, mercati di previsione, sentiment, notizie. Fase 1 dello studio globale.

    python studio/macro/macro.py --out <cartella>                     # NQZ6: scrive macro.json e stampa il riassunto
    python studio/macro/macro.py --out <cartella> --strumento ESZ6    # ES: in piu' i settori dell'S&P 500

**Lo strumento** (27 ottobre 2026). Per NQ il modulo resta com'era. Per ES aggiunge, con le stesse
fonti: gli ETF settoriali dell'S&P 500 (piu' KRE, IWM e SPY) e la loro ampiezza (`ampiezza_sp500`:
RSP equipesato contro SPY pesato per capitalizzazione, quanti settori salgono); le trimestrali dei
pesi massimi non tecnologici dell'S&P 500 oltre a quelle tech. L'elenco dei titoli dell'S&P 500 non
c'e' fra le fonti gratuite gia' usate (api.nasdaq.com lo rifiuta): l'ampiezza per titolo resta del
Nasdaq-100, quella dell'S&P si legge per settori.

**Da dove viene.** Le fonti sono quelle di World Monitor (github.com/koala73/worldmonitor, AGPL-3.0):
il pannello "NQ Pulse" (`src/config/nq-context.ts`: NQ=F, QQQ, ^VXN, ^TNX, le otto grandi del
Nasdaq), gli script `scripts/seed-*.mjs` e i feed della variante finance. **Il codice e' nostro**:
di World Monitor si riusa l'elenco delle fonti (indirizzi pubblici), non il codice, perche' questo
repository e' pubblico e non e' sotto AGPL. Il catalogo, con le righe di World Monitor da cui viene
ogni fonte: `studio/macro/le-fonti-della-macro.md`.

Solo libreria standard. Ogni fonte e' indipendente: se una cade, il JSON lo dice (`errori`) e le
altre restano. **Nessuna chiave**: FRED si legge dal CSV pubblico dei grafici (`fredgraph.csv`), che
non chiede la API key (15 ottobre 2026: l'utente non ha la chiave).

Sono dati di **contesto**, non dati di backtest: la decisione 2 di CLAUDE.md (i dati vengono solo da
ATAS) vale per il metodo quantitativo; lo studio globale li usa per descrivere, mai per provare.
"""
from __future__ import annotations

import argparse
import csv
import datetime as dt
import html
import io
import json
import os
import re
import statistics
import sys
import time
import urllib.parse
import urllib.request
import xml.etree.ElementTree as ET
from concurrent.futures import ThreadPoolExecutor
from zoneinfo import ZoneInfo

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")

ROMA = ZoneInfo("Europe/Rome")
NY = ZoneInfo("America/New_York")
# lo user agent di un Chrome desktop, come World Monitor (scripts/_seed-utils.mjs:25); CNN vuole quello Mac
CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36"
MAC = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36"
UA_SCRIPT = "orderflow-atas-studio/1.0 (python-urllib)"

# NQ Pulse di World Monitor, piu' il contesto di un trader di indici USA
QUOTAZIONI = {
    "NQ=F": "future Nasdaq-100", "ES=F": "future S&P 500", "QQQ": "ETF Nasdaq-100", "RSP": "S&P 500 equipesato",
    "^VXN": "volatilita' Nasdaq-100", "^VIX": "VIX", "^VIX9D": "VIX 9 giorni", "^VIX3M": "VIX 3 mesi",
    "^TNX": "Treasury 10 anni", "DX-Y.NYB": "dollaro (DXY)", "CL=F": "petrolio WTI", "GC=F": "oro",
    "TLT": "Treasury 20+ anni (ETF)", "HYG": "high yield (ETF)", "SMH": "semiconduttori (ETF)", "XLK": "tecnologia (ETF)",
}
GRANDI = ["AAPL", "MSFT", "NVDA", "AMZN", "GOOGL", "META", "AVGO", "TSLA"]
# SOLO PER ES: i settori dell'S&P 500 (gli undici Select Sector SPDR), le banche regionali, le small cap
# e l'S&P pesato per capitalizzazione, da confrontare con RSP (equipesato)
SETTORI_SP = {
    "XLK": "tecnologia (ETF)", "XLF": "finanziari (ETF)", "XLV": "salute (ETF)", "XLY": "consumi discrezionali (ETF)",
    "XLC": "comunicazioni (ETF)", "XLI": "industriali (ETF)", "XLP": "consumi di base (ETF)", "XLE": "energia (ETF)",
    "XLU": "utility (ETF)", "XLRE": "immobiliare (ETF)", "XLB": "materiali (ETF)",
}
ALTRI_SP = {"SPY": "S&P 500 pesato per capitalizzazione (ETF)", "KRE": "banche regionali (ETF)", "IWM": "Russell 2000 (ETF)"}
# SOLO PER ES: i pesi massimi non tecnologici dell'S&P 500, per le trimestrali (le tech restano quelle sotto)
GRANDI_SP = ["BRK.B", "JPM", "LLY", "V", "MA", "XOM", "UNH", "WMT", "JNJ", "PG", "HD", "COST", "BAC", "ABBV",
             "CVX", "KO", "MRK", "WFC", "GS", "MS", "CAT", "GE", "PEP", "MCD", "LIN", "IBM", "ORCL", "CRM"]
# le date FOMC 2026 (seconda giornata = decisione), da federalreserve.gov come in World Monitor
FOMC_2026 = ["2026-01-28", "2026-03-18", "2026-04-29", "2026-06-17", "2026-07-29", "2026-09-16", "2026-10-28", "2026-12-09"]
POLYMARKET_TAG = ["fed", "inflation", "recession", "tariffs"]
# FRED dal CSV dei grafici, senza chiave: serie -> (nome, tipo). "livello" = il valore com'e';
# "indice" = un indice mensile, di cui si calcolano variazione mensile e annua in %
SERIE_FRED = {
    "DFF": ("fed funds effettivo, %", "livello"), "DGS2": ("Treasury 2 anni, %", "livello"),
    "DGS10": ("Treasury 10 anni, %", "livello"), "T10YIE": ("inflazione attesa a 10 anni (breakeven), %", "livello"),
    "BAMLH0A0HYM2": ("spread high yield ICE BofA, punti %", "livello"), "STLFSI4": ("stress finanziario St. Louis Fed", "livello"),
    "WALCL": ("bilancio della Fed, milioni $", "livello"),
    "PPIFIS": ("PPI domanda finale", "indice"), "PPICOR": ("PPI domanda finale esclusi alimentari, energia e commercio", "indice"),
    "PPIFES": ("PPI beni finiti esclusi alimentari ed energia", "indice"),
    "RSAFS": ("vendite al dettaglio", "indice"), "RSFSXMV": ("vendite al dettaglio esclusi auto e ricambi", "indice"),
    "CPIAUCSL": ("CPI", "indice"), "CPILFESL": ("CPI esclusi alimentari ed energia", "indice"),
    "ICSA": ("sussidi di disoccupazione, richieste iniziali", "livello"),
    "GACDFSA066MSFRBPHI": ("Philly Fed, attivita' manifatturiera (diffusione)", "livello"),
}
# il titolo di Forex Factory -> (serie FRED, misura, periodo atteso rispetto al giorno dell'uscita):
# "mese_prima" = il mese prima di quello dell'uscita, "stesso_mese", "settimana" = la settimana che
# finisce il sabato prima. Le corrispondenze "core" sono approssimate: le-fonti-della-macro.md
ESITI_FRED = {
    "PPI m/m": ("PPIFIS", "mm", "mese_prima"), "PPI y/y": ("PPIFIS", "aa", "mese_prima"),
    "Core PPI m/m": ("PPICOR", "mm", "mese_prima"),
    "Retail Sales m/m": ("RSAFS", "mm", "mese_prima"), "Core Retail Sales m/m": ("RSFSXMV", "mm", "mese_prima"),
    "CPI m/m": ("CPIAUCSL", "mm", "mese_prima"), "CPI y/y": ("CPIAUCSL", "aa", "mese_prima"),
    "Core CPI m/m": ("CPILFESL", "mm", "mese_prima"),
    "Philly Fed Manufacturing Index": ("GACDFSA066MSFRBPHI", "livello", "stesso_mese"),
    "Unemployment Claims": ("ICSA", "migliaia", "settimana"),
}
# i dati di oggi a barre di 5 minuti: per sapere se il numero e' di adesso o la chiusura di ieri
INTRADAY = {"^VIX": "VIX", "^VIX9D": "VIX 9 giorni", "^VIX3M": "VIX 3 mesi", "^VXN": "volatilita' Nasdaq-100",
            "^TNX": "Treasury 10 anni, %",
            "ZT=F": "future sul Treasury 2 anni (PREZZO: sale quando il rendimento scende)",
            "2YY=F": "future sul rendimento a 2 anni (Cboe, %)"}
MESI_FUTURE = "FGHJKMNQUVXZ"
RSS = {
    "CNBC": "https://www.cnbc.com/id/100003114/device/rss/rss.html",
    "Federal Reserve": "https://www.federalreserve.gov/feeds/press_all.xml",
    "GN Nasdaq futures": "https://news.google.com/rss/search?q=%22Nasdaq+futures%22+when:1d&hl=en-US&gl=US&ceid=US:en",
    "GN Treasury yields": "https://news.google.com/rss/search?q=Treasury+yields+when:1d&hl=en-US&gl=US&ceid=US:en",
    "GN Fed": "https://news.google.com/rss/search?q=Federal+Reserve+rates+when:1d&hl=en-US&gl=US&ceid=US:en",
}


def scarica(url: str, ua: str = CHROME, intestazioni: dict | None = None, timeout: float = 25, tentativi: int = 2) -> bytes:
    ultimo = None
    for i in range(tentativi):
        try:
            req = urllib.request.Request(url, headers={"User-Agent": ua, "Accept": "*/*", **(intestazioni or {})})
            with urllib.request.urlopen(req, timeout=timeout) as r:
                return r.read()
        except Exception as e:  # noqa: BLE001 - ogni fonte e' indipendente
            ultimo = e
            time.sleep(1.5 * (i + 1))
    raise RuntimeError(f"{type(ultimo).__name__}: {ultimo}")


def percentile(serie: list[float], x: float) -> float:
    return round(100 * sum(1 for v in serie if v <= x) / len(serie), 1) if serie else float("nan")


# --- le fonti -------------------------------------------------------------------------------------

def quotazione(simbolo: str) -> dict:
    url = f"https://query1.finance.yahoo.com/v8/finance/chart/{urllib.parse.quote(simbolo)}?range=1y&interval=1d"
    d = json.loads(scarica(url))["chart"]["result"][0]
    chiusure = [c for c in d["indicators"]["quote"][0]["close"] if c is not None]
    m = d["meta"]
    ultimo = m.get("regularMarketPrice") or chiusure[-1]
    ieri = m.get("chartPreviousClose") if len(chiusure) < 2 else chiusure[-2]
    var = lambda n: round(100 * (ultimo / chiusure[-1 - n] - 1), 2) if len(chiusure) > n else None  # noqa: E731
    rend = [chiusure[i] / chiusure[i - 1] - 1 for i in range(max(1, len(chiusure) - 20), len(chiusure))]
    return {"simbolo": simbolo, "nome": QUOTAZIONI.get(simbolo) or SETTORI_SP.get(simbolo) or ALTRI_SP.get(simbolo, simbolo), "ultimo": round(ultimo, 4),
            "quando": dt.datetime.fromtimestamp(m.get("regularMarketTime", 0), dt.timezone.utc).isoformat(),
            "var_1g": round(100 * (ultimo / ieri - 1), 2) if ieri else None, "var_5g": var(5), "var_20g": var(20),
            "pct_1a": percentile(chiusure, ultimo), "min_1a": round(min(chiusure), 4), "max_1a": round(max(chiusure), 4),
            "vol_20g_annua": round(statistics.pstdev(rend) * (252 ** 0.5) * 100, 1) if len(rend) > 5 else None}


def treasury() -> dict:
    anno = dt.date.today().year
    righe = []
    for a in (anno, anno - 1):
        url = (f"https://home.treasury.gov/resource-center/data-chart-center/interest-rates/daily-treasury-rates.csv/{a}/all"
               f"?type=daily_treasury_yield_curve&field_tdr_date_value={a}&page&_format=csv")
        righe += list(csv.DictReader(io.StringIO(scarica(url).decode("utf-8-sig"))))
        if len(righe) > 30:
            break
    righe.sort(key=lambda r: dt.datetime.strptime(r["Date"], "%m/%d/%Y"), reverse=True)
    f = lambda r, k: float(r[k]) if r.get(k) else None  # noqa: E731
    def punto(r):
        return {"data": dt.datetime.strptime(r["Date"], "%m/%d/%Y").date().isoformat(), "3M": f(r, "3 Mo"),
                "2A": f(r, "2 Yr"), "5A": f(r, "5 Yr"), "10A": f(r, "10 Yr"), "30A": f(r, "30 Yr")}
    oggi, ieri, mese = punto(righe[0]), punto(righe[1]), punto(righe[min(20, len(righe) - 1)])
    for p in (oggi, ieri, mese):
        p["10A-2A"] = round(p["10A"] - p["2A"], 2) if p["10A"] and p["2A"] else None
        p["10A-3M"] = round(p["10A"] - p["3M"], 2) if p["10A"] and p["3M"] else None
    return {"ultimo": oggi, "precedente": ieri, "20_sedute_fa": mese, "fonte": "home.treasury.gov, curva par giornaliera"}


def serie_fred(serie: str, anni: int = 2) -> list[tuple[str, float]]:
    """Le osservazioni di una serie FRED dal CSV pubblico dei grafici, senza chiave, dalla piu' vecchia."""
    inizio = (dt.date.today() - dt.timedelta(days=366 * anni)).isoformat()
    # LO USER AGENT CONTA (15 ottobre 2026): con quello di Chrome, e alla seconda prova anche con uno nostro
    # inventato, FRED tiene la connessione aperta fino al timeout; con quello standard di Python risponde in
    # un decimo di secondo. E' quello vero di urllib: nessun travestimento
    testo = scarica(f"https://fred.stlouisfed.org/graph/fredgraph.csv?id={serie}&cosd={inizio}", ua=f"Python-urllib/{sys.version_info[0]}.{sys.version_info[1]}",
                    timeout=8, tentativi=1).decode("utf-8-sig")
    return [(r[0], float(r[1])) for r in list(csv.reader(io.StringIO(testo)))[1:] if len(r) == 2 and r[1] not in ("", ".")]


def fred() -> dict:
    """Le serie FRED senza chiave: ultimo valore, data, precedente. Per gli indici mensili la variazione
    mensile e annua in %, calcolata dall'indice: puo' differire di 0,1 dal comunicato, per gli
    arrotondamenti."""
    def una(serie):
        nome, tipo = SERIE_FRED[serie]
        oss = serie_fred(serie)
        (d, v), (dp, vp) = oss[-1], oss[-2]
        r = {"nome": nome, "data": d, "valore": v, "data_precedente": dp, "precedente": vp}
        if tipo == "indice":
            r["mm_pct"] = round(100 * (v / vp - 1), 2)
            r["mm_pct_precedente"] = round(100 * (vp / oss[-3][1] - 1), 2) if len(oss) > 2 else None
            un_anno = next((x for dx, x in reversed(oss) if dx <= f"{int(d[:4]) - 1}{d[4:]}"), None)
            r["aa_pct"] = round(100 * (v / un_anno - 1), 2) if un_anno else None
        else:
            r["variazione"] = round(v - vp, 4)
            if len(oss) > 30:
                r["pct_2a"] = percentile([x for _, x in oss], v)
        return r
    # una richiesta alla volta: il 15 ottobre 2026 otto in parallelo hanno fatto scadere tutte le
    # richieste per qualche minuto, anche quelle in serie; una alla volta ognuna prende 0,1-0,5 s
    # Se FRED smette di rispondere (due errori di fila), le serie rimaste si saltano: aspettarle tutte
    # costava due minuti alla macro.
    out, errori, di_fila = {}, {}, 0
    for serie in SERIE_FRED:
        if di_fila >= 2:
            errori[serie] = "saltata: FRED non risponde (due errori di fila)"
            continue
        try:
            out[serie] = una(serie)
            di_fila = 0
        except Exception as e:  # noqa: BLE001
            errori[serie] = str(e)[:120]
            di_fila += 1
        time.sleep(0.2)
    return {"serie": out, "errori": errori,
            "fonte": "fred.stlouisfed.org/graph/fredgraph.csv (CSV pubblico, nessuna chiave); i giornalieri arrivano con un giorno di ritardo"}


def periodo_atteso(uscita: dt.date, regola: str) -> str:
    if regola == "mese_prima":
        return (uscita.replace(day=1) - dt.timedelta(days=1)).replace(day=1).isoformat()
    if regola == "stesso_mese":
        return uscita.replace(day=1).isoformat()
    return (uscita - dt.timedelta(days=(uscita.weekday() - 5) % 7 or 7)).isoformat()   # il sabato prima


def esiti(cal: dict, fr: dict) -> list[dict]:
    """Il dato uscito degli eventi gia' passati della settimana, preso da FRED, accanto ad attesa e
    precedente di Forex Factory. Si scrive solo se FRED ha gia' l'osservazione del periodo giusto."""
    adesso = dt.datetime.now(ROMA).strftime("%Y-%m-%d %H:%M")
    serie = fr.get("serie", {})
    out = []
    for e in cal.get("eventi_usd", []):
        if "evento" not in e or e["ora_italiana"] > adesso or e["evento"] not in ESITI_FRED:
            continue
        codice, misura, regola = ESITI_FRED[e["evento"]]
        s = serie.get(codice)
        atteso = periodo_atteso(dt.date.fromisoformat(e["ora_italiana"][:10]), regola)
        r = {"evento": e["evento"], "ora_italiana": e["ora_italiana"], "attesa": e["attesa"], "precedente_ff": e["precedente"],
             "serie": codice, "periodo_atteso": atteso, "uscito": None}
        if not s:
            r["nota"] = "serie FRED non raccolta"
        elif s["data"] != atteso:
            r["nota"] = f"FRED non ha ancora il periodo {atteso} (ultimo {s['data']})"
        else:
            r["uscito"] = {"mm": s.get("mm_pct"), "aa": s.get("aa_pct"), "livello": s["valore"],
                           "migliaia": round(s["valore"] / 1000)}[misura]
            r["misura"] = {"mm": "variazione mensile %, calcolata dall'indice", "aa": "variazione annua %, calcolata dall'indice",
                           "livello": "livello", "migliaia": "migliaia"}[misura]
        out.append(r)
    return out


def intraday(simbolo: str) -> dict:
    """Il prezzo di adesso a barre di 5 minuti, con l'ora. Se l'ultimo dato non e' di oggi (ora di New
    York) lo si dice: per alcuni indici, come il VXN, Yahoo da' solo la chiusura di ieri."""
    url = f"https://query1.finance.yahoo.com/v8/finance/chart/{urllib.parse.quote(simbolo)}?range=1d&interval=5m"
    d = json.loads(scarica(url))["chart"]["result"][0]
    m = d["meta"]
    quando = dt.datetime.fromtimestamp(m.get("regularMarketTime", 0), dt.timezone.utc)
    barre = len([c for c in (d["indicators"]["quote"][0].get("close") or []) if c is not None])
    di_oggi = quando.astimezone(NY).date() == dt.datetime.now(NY).date() and barre > 0
    ultimo, prec = m.get("regularMarketPrice"), m.get("chartPreviousClose")
    return {"simbolo": simbolo, "nome": INTRADAY.get(simbolo, m.get("shortName") or simbolo), "ultimo": ultimo,
            "chiusura_prima": prec, "var": round(ultimo - prec, 4) if ultimo is not None and prec else None,
            "ora_italiana": quando.astimezone(ROMA).strftime("%Y-%m-%d %H:%M"), "barre_5m_oggi": barre,
            "stato": "di oggi" if di_oggi else f"NON di oggi: ultimo dato del {quando.astimezone(ROMA):%d/%m %H:%M}"}


def tempo_reale() -> dict:
    out, errori = [], {}
    with ThreadPoolExecutor(max_workers=7) as pool:
        for s, fut in {s: pool.submit(intraday, s) for s in INTRADAY}.items():
            try:
                out.append(fut.result())
            except Exception as e:  # noqa: BLE001
                errori[s] = str(e)[:120]
    return {"quotazioni": out, "errori": errori, "fonte": "Yahoo chart v8, interval=5m, range=1d"}


def simbolo_zq(g: dt.date) -> str:
    return f"ZQ{MESI_FUTURE[g.month - 1]}{g.year % 100:02d}.CBT"


def fomc_future() -> dict:
    """La probabilita' di un passo alla prossima riunione dai future sui fed funds a 30 giorni (ZQ, CBOT),
    con il metodo di CME FedWatch: il tasso implicito di un mese e' 100 - prezzo, ed e' la media dei
    tassi di ogni giorno del mese; la decisione vale dal giorno dopo la riunione."""
    oggi = dt.date.today().isoformat()
    riunione = dt.date.fromisoformat(next(d for d in FOMC_2026 if d >= oggi))
    mese = riunione.replace(day=1)
    dopo = (mese + dt.timedelta(days=32)).replace(day=1)
    giorni_mese = (dopo - mese).days
    giorni_dopo = giorni_mese - riunione.day        # dal giorno dopo la riunione alla fine del mese
    giorni_prima = giorni_mese - giorni_dopo
    prezzi = {}
    for g in (mese, dopo, (dopo + dt.timedelta(days=32)).replace(day=1)):
        q = intraday(simbolo_zq(g))
        prezzi[g.strftime("%Y-%m")] = {"simbolo": q["simbolo"], "prezzo": q["ultimo"], "implicito_pct": round(100 - q["ultimo"], 4),
                                       "ora_italiana": q["ora_italiana"], "stato": q["stato"]}
    imp_mese = prezzi[mese.strftime("%Y-%m")]["implicito_pct"]
    imp_dopo = prezzi[dopo.strftime("%Y-%m")]["implicito_pct"]
    # il mese dopo la riunione non ne ha un'altra (vero per tutte le date 2026): il suo implicito e' il
    # tasso dopo la decisione, e il tasso prima si ricava dal mese della riunione
    prima = (imp_mese * giorni_mese - imp_dopo * giorni_dopo) / giorni_prima
    variazione = imp_dopo - prima
    # il fed funds effettivo (DFF, per il confronto con "prima") lo aggiunge raccogli() da fred(), per
    # non fare a FRED una richiesta in parallelo alle sue
    return {"riunione": riunione.isoformat(), "future": prezzi, "tasso_prima_implicito": round(prima, 4),
            "tasso_dopo_implicito": round(imp_dopo, 4), "fed_funds_effettivo": None,
            "variazione_attesa_pb": round(100 * variazione, 1),
            "probabilita_25pb": {"verso": "rialzo" if variazione > 0 else "taglio", "pct": round(100 * abs(variazione) / 0.25, 1)},
            "metodo": (f"CME FedWatch: implicito = 100 - prezzo; prima = (implicito del mese x {giorni_mese} - implicito del "
                       f"mese dopo x {giorni_dopo}) / {giorni_prima}; probabilita' = |dopo - prima| / 0,25 pb. Vale per un passo "
                       "da 25 pb e senza riunioni nel mese dopo"),
            "fonte": "Yahoo, future ZQ del CBOT (30 Day Federal Funds)"}


def ampiezza() -> dict:
    """L'ampiezza del Nasdaq-100: quanti titoli salgono e quanti scendono oggi, la variazione media e
    mediana, e quella pesata per capitalizzazione (un'approssimazione: i pesi veri dell'indice sono
    modificati)."""
    d = json.loads(scarica("https://api.nasdaq.com/api/quote/list-type/nasdaq100", MAC, {"Accept": "application/json"}))["data"]
    def num(x):
        x = re.sub(r"[^0-9.\-]", "", x or "")
        return float(x) if re.search(r"\d", x) else None
    titoli = [{"simbolo": r["symbol"], "var_pct": num(r.get("percentageChange")), "cap": num(r.get("marketCap")) or 0.0}
              for r in d["data"]["rows"]]
    titoli = [t for t in titoli if t["var_pct"] is not None]
    tot = sum(t["cap"] for t in titoli) or 1.0
    var = [t["var_pct"] for t in titoli]
    return {"quando": d.get("date"), "titoli": len(titoli), "salgono": sum(1 for v in var if v > 0),
            "scendono": sum(1 for v in var if v < 0), "invariati": sum(1 for v in var if v == 0),
            "var_media_pct": round(statistics.mean(var), 2), "var_mediana_pct": round(statistics.median(var), 2),
            "var_pesata_cap_pct": round(sum(t["var_pct"] * t["cap"] for t in titoli) / tot, 2),
            "i_dieci_piu_grandi": [{"simbolo": t["simbolo"], "peso_cap_pct": round(100 * t["cap"] / tot, 1), "var_pct": t["var_pct"]}
                                   for t in sorted(titoli, key=lambda t: -t["cap"])[:10]],
            "fonte": "api.nasdaq.com/api/quote/list-type/nasdaq100 (101 righe: GOOG e GOOGL contano due volte)"}


def calendario() -> dict:
    """Forex Factory (aggiunta nostra: World Monitor non ha un calendario con le attese) e le date FOMC."""
    eventi = []
    for nome in ("thisweek", "nextweek"):
        try:
            for e in json.loads(scarica(f"https://nfs.faireconomy.media/ff_calendar_{nome}.json", tentativi=1)):
                if e.get("country") != "USD" or e.get("impact") not in ("High", "Medium"):
                    continue
                quando = dt.datetime.fromisoformat(e["date"])
                eventi.append({"ora_italiana": quando.astimezone(ROMA).strftime("%Y-%m-%d %H:%M"), "evento": e["title"],
                               "peso": {"High": "alto", "Medium": "medio"}[e["impact"]],
                               "attesa": e.get("forecast") or "", "precedente": e.get("previous") or ""})
        except Exception as e:  # la settimana prossima esce solo a fine settimana
            if nome == "thisweek":
                raise
            eventi.append({"nota": f"settimana prossima non ancora pubblicata ({e})"})
    oggi = dt.date.today().isoformat()
    prossimo = next((d for d in FOMC_2026 if d >= oggi), None)
    return {"eventi_usd": eventi, "prossimo_fomc": prossimo,
            "fonte": "nfs.faireconomy.media (Forex Factory), date FOMC da federalreserve.gov"}


def ampiezza_sp500(quot: list[dict]) -> dict | None:
    """L'ampiezza dell'S&P 500 per settori (solo ES): RSP equipesato contro SPY pesato per capitalizzazione
    (positivo = il titolo medio fa meglio dei grandi) a 1, 5 e 20 giorni, e quanti degli undici settori
    salgono. Dalle quotazioni gia' raccolte: nessuna fonte nuova."""
    q = {x["simbolo"]: x for x in quot}
    rsp, spy = q.get("RSP"), q.get("SPY")
    sett = [q[s] for s in SETTORI_SP if s in q]
    if not (rsp and spy) and not sett:
        return None
    out: dict = {}
    if rsp and spy:
        out["rsp_meno_spy_pct"] = {k: (round(rsp[k] - spy[k], 2) if rsp.get(k) is not None and spy.get(k) is not None else None)
                                   for k in ("var_1g", "var_5g", "var_20g")}
    for k in ("var_1g", "var_5g", "var_20g"):
        v = [s[k] for s in sett if s.get(k) is not None]
        if v:
            out[f"settori_{k}"] = {"salgono": sum(1 for x in v if x > 0), "scendono": sum(1 for x in v if x < 0),
                                   "mediana_pct": round(statistics.median(v), 2),
                                   "migliore": max(sett, key=lambda s: s.get(k) if s.get(k) is not None else -1e9)["simbolo"],
                                   "peggiore": min(sett, key=lambda s: s.get(k) if s.get(k) is not None else 1e9)["simbolo"]}
    out["settori_letti"] = len(sett)
    out["fonte"] = "Yahoo chart (le stesse quotazioni di `quotazioni`): undici Select Sector SPDR, RSP e SPY"
    return out


def trimestrali(strumento: str = "NQZ6") -> dict:
    """Le trimestrali delle otto grandi del Nasdaq nei prossimi 14 giorni (api.nasdaq.com, gratuita); per ES
    anche quelle dei pesi massimi non tecnologici dell'S&P 500 (`GRANDI_SP`)."""
    cercati = set(GRANDI) | {"NFLX", "AMD", "ASML", "TSM", "QCOM", "INTC"}
    if strumento.upper().startswith("ES"):
        cercati |= {s.replace(".", "/") for s in GRANDI_SP} | set(GRANDI_SP)
    trovate = []
    for i in range(15):
        g = dt.date.today() + dt.timedelta(days=i)
        if g.weekday() >= 5:
            continue
        try:
            d = json.loads(scarica(f"https://api.nasdaq.com/api/calendar/earnings?date={g}", MAC,
                                   {"Accept": "application/json"}, tentativi=1))
        except Exception:
            continue
        for r in (d.get("data") or {}).get("rows") or []:
            if r.get("symbol") in cercati:
                trovate.append({"data": g.isoformat(), "simbolo": r["symbol"], "quando": r.get("time"),
                                "eps_atteso": r.get("epsForecast"),
                                "gruppo": "tech/Nasdaq" if r["symbol"] not in GRANDI_SP and r["symbol"].replace("/", ".") not in GRANDI_SP
                                          else "S&P 500 non tech"})
    return {"trimestrali": trovate, "cercati": len(cercati), "fonte": "api.nasdaq.com/api/calendar/earnings"}


def previsioni() -> dict:
    out = []
    for tag in POLYMARKET_TAG:
        url = f"https://gamma-api.polymarket.com/events?tag_slug={tag}&closed=false&active=true&order=volume&ascending=false&limit=4"
        for e in json.loads(scarica(url)):
            mercati = []
            for m in (e.get("markets") or [])[:12]:
                try:
                    prezzi = json.loads(m.get("outcomePrices") or "[]")
                    esiti = json.loads(m.get("outcomes") or "[]")
                except json.JSONDecodeError:
                    continue
                if prezzi and esiti:
                    mercati.append({"domanda": m.get("groupItemTitle") or m.get("question"),
                                    "probabilita": {k: round(float(v), 3) for k, v in zip(esiti, prezzi)}})
            out.append({"tag": tag, "titolo": e.get("title"), "volume": round(float(e.get("volume") or 0)),
                        "scadenza": (e.get("endDate") or "")[:10], "mercati": mercati})
    return {"eventi": out, "fonte": "gamma-api.polymarket.com"}


def sentiment() -> dict:
    d = json.loads(scarica("https://production.dataviz.cnn.io/index/fearandgreed/current", MAC,
                           {"Referer": "https://www.cnn.com/markets/fear-and-greed"}))
    return {"fear_greed": round(d["score"], 1), "giudizio": d["rating"], "una_settimana_fa": round(d["previous_1_week"], 1),
            "un_mese_fa": round(d["previous_1_month"], 1), "fonte": "CNN Fear & Greed"}


def notizie() -> dict:
    out, limite = [], dt.datetime.now(dt.timezone.utc) - dt.timedelta(hours=30)
    for fonte, url in RSS.items():
        try:
            radice = ET.fromstring(scarica(url, intestazioni={"Accept": "application/rss+xml, application/xml"}, tentativi=1))
        except Exception as e:
            out.append({"fonte": fonte, "errore": str(e)[:120]})
            continue
        for item in radice.iter("item"):
            titolo = (item.findtext("title") or "").strip()
            data = item.findtext("pubDate")
            try:
                quando = dt.datetime.strptime(data, "%a, %d %b %Y %H:%M:%S %Z").replace(tzinfo=dt.timezone.utc) if data else None
            except ValueError:
                try:
                    quando = dt.datetime.strptime(data, "%a, %d %b %Y %H:%M:%S %z")
                except ValueError:
                    quando = None
            if quando and quando < limite:
                continue
            # la descrizione, ripulita dall'HTML: su CNBC e' il sommario dell'articolo; su Google News e'
            # solo il titolo con la testata, e allora non si tiene
            testo = re.sub(r"\s+", " ", html.unescape(re.sub(r"<[^>]+>", " ", item.findtext("description") or ""))).strip()
            if len(testo) < 40 or testo.startswith(titolo[:40]):
                testo = ""
            out.append({"fonte": fonte, "ora_italiana": quando.astimezone(ROMA).strftime("%d/%m %H:%M") if quando else "",
                        "titolo": re.sub(r"\s+", " ", titolo)[:180], "testo": testo[:300]})
    # un tetto per fonte, non uno totale: il 15 ottobre 2026 le prime 40 erano tutte CNBC e Google
    # News "Nasdaq futures", e i feed della Fed e dei Treasury restavano fuori senza che si vedesse
    titoli, unici, per_fonte = set(), [], {}
    for n in out:
        if n.get("titolo") and n["titolo"] not in titoli and per_fonte.get(n["fonte"], 0) < 10:
            titoli.add(n["titolo"])
            per_fonte[n["fonte"]] = per_fonte.get(n["fonte"], 0) + 1
            unici.append(n)
        elif n.get("errore"):
            unici.append(n)
    return {"notizie": unici, "per_fonte": per_fonte, "fonte": ", ".join(RSS)}


FONTI = {"treasury": treasury, "fred": fred, "calendario": calendario, "trimestrali": trimestrali,
         "previsioni": previsioni, "sentiment": sentiment, "notizie": notizie, "tempo_reale": tempo_reale,
         "fomc_future": fomc_future, "ampiezza": ampiezza}


def raccogli(strumento: str = "NQZ6") -> dict:
    """Tutta la macro in un dizionario; la usano main() e studio/raccogli.py. Con ES (`strumento` ESZ6) in
    piu' i settori dell'S&P 500, la loro ampiezza e le trimestrali dei pesi massimi non tech."""
    inizio = time.time()
    es = strumento.upper().startswith("ES")
    esito = {"raccolto_utc": dt.datetime.now(dt.timezone.utc).isoformat(timespec="seconds"), "strumento": strumento,
             "errori": {}}
    simboli = list(QUOTAZIONI) + GRANDI + ([s for s in list(SETTORI_SP) + list(ALTRI_SP) if s not in QUOTAZIONI] if es else [])
    with ThreadPoolExecutor(max_workers=12) as pool:
        quot = {s: pool.submit(quotazione, s) for s in simboli}
        altre = {k: pool.submit((lambda: trimestrali(strumento)) if k == "trimestrali" else f) for k, f in FONTI.items()}
        esito["quotazioni"] = []
        for s, fut in quot.items():
            try:
                esito["quotazioni"].append(fut.result())
            except Exception as e:  # noqa: BLE001
                esito["errori"][s] = str(e)[:160]
        for k, fut in altre.items():
            try:
                esito[k] = fut.result()
            except Exception as e:  # noqa: BLE001
                esito["errori"][k] = str(e)[:160]
    if es:
        esito["ampiezza_sp500"] = ampiezza_sp500(esito["quotazioni"])
    if "calendario" in esito and "fred" in esito:
        esito["esiti"] = esiti(esito["calendario"], esito["fred"])
    dff = (esito.get("fred") or {}).get("serie", {}).get("DFF")
    if esito.get("fomc_future") and dff:
        esito["fomc_future"]["fed_funds_effettivo"] = {"data": dff["data"], "valore": dff["valore"]}
    esito["secondi"] = round(time.time() - inizio, 1)
    return esito


def riassunto(m: dict) -> None:
    print(f"MACRO raccolta {m['raccolto_utc']} — errori: {', '.join(m['errori']) or 'nessuno'}")
    for q in m.get("quotazioni", []):
        print(f"  {q['simbolo']:<9} {q['ultimo']:>11,.2f}  1g {q['var_1g']:+6.2f}%  5g {q['var_5g'] or 0:+6.2f}%  "
              f"20g {q['var_20g'] or 0:+6.2f}%  pct1a {q['pct_1a']:5.1f}  vol20 {q['vol_20g_annua']}%  {q['nome']}")
    t = m.get("treasury", {}).get("ultimo")
    if t:
        print(f"  Treasury {t['data']}: 3M {t['3M']} 2A {t['2A']} 10A {t['10A']} 30A {t['30A']}  10A-2A {t['10A-2A']}")
    s = m.get("sentiment")
    if s:
        print(f"  Fear & Greed {s['fear_greed']} ({s['giudizio']}), una settimana fa {s['una_settimana_fa']}")
    for e in m.get("esiti", []):
        print(f"  uscito {e['ora_italiana']} {e['evento']}: {e['uscito'] if e['uscito'] is not None else e.get('nota')}"
              f"  (attesa {e['attesa'] or '-'}, prec. {e['precedente_ff'] or '-'})")
    f = m.get("fomc_future")
    if f:
        p = f["probabilita_25pb"]
        print(f"  FOMC {f['riunione']} dai future ZQ: {p['verso']} di 25 pb al {p['pct']}% ({f['variazione_attesa_pb']:+} pb)")
    a = m.get("ampiezza")
    if a:
        print(f"  Nasdaq-100: {a['salgono']} su, {a['scendono']} giu', mediana {a['var_mediana_pct']:+}%, pesata {a['var_pesata_cap_pct']:+}%")
    sp = m.get("ampiezza_sp500")
    if sp:
        s1 = sp.get("settori_var_1g") or {}
        print(f"  S&P 500 per settori: {s1.get('salgono')} su, {s1.get('scendono')} giu' su {sp['settori_letti']}; "
              f"RSP-SPY {sp.get('rsp_meno_spy_pct')}")
    for q in (m.get("tempo_reale") or {}).get("quotazioni", []):
        print(f"  adesso {q['simbolo']:<7} {q['ultimo']}  {q['ora_italiana']}  {q['stato']}")
    c = m.get("calendario")
    if c:
        print(f"  prossimo FOMC {c['prossimo_fomc']}; eventi USD alto/medio: {sum(1 for e in c['eventi_usd'] if 'evento' in e)}")
    print(f"  trimestrali delle grandi: {len((m.get('trimestrali') or {}).get('trimestrali', []))};  "
          f"previsioni: {len((m.get('previsioni') or {}).get('eventi', []))};  notizie: {len((m.get('notizie') or {}).get('notizie', []))}")


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--out", help="cartella dove scrivere macro.json")
    ap.add_argument("--strumento", default="NQZ6", help="lo strumento dello studio: NQZ6 (difetto) o ESZ6")
    a = ap.parse_args()
    m = raccogli(a.strumento)
    if a.out:
        os.makedirs(a.out, exist_ok=True)
        with open(os.path.join(a.out, "macro.json"), "w", encoding="utf-8") as f:
            json.dump(m, f, ensure_ascii=False, indent=1)
    riassunto(m)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
