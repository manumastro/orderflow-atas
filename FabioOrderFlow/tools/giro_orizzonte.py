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

# Su Windows lo stdout di una console non e' UTF-8 (cp1252 di default), e il primo carattere di
# cornice — U+2500 — fa morire il programma con UnicodeEncodeError. Il danno non e' il crash: e'
# che l'hook consegna all'agente un traceback al posto del contesto, e **una risposta esce lo
# stesso**, costruita a memoria, senza che nessuno veda che il giro non e' mai arrivato.
#
# Si difende qui e non solo nell'hook perche' CLAUDE.md dice di lanciare questo comando a mano
# quando il blocco manca: un programma che funziona solo se chiamato dal suo hook non e' difeso.
import ora  # gli orari si stampano in italiano, vedi ora.py

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

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


def ora_piu_avanti(testo: str, adesso_utc: str) -> str | None:
    """L'orario piu' avanti citato nel testo, se supera l'ora di mercato. Altrimenti None.

    Serve a non stampare, in replay, un pezzo di diario scritto in un giro precedente che era
    arrivato piu' avanti: quel testo racconta minuti che nella sessione in corso non sono ancora
    successi. Si cercano sia gli orari in Z (`13:31Z`) sia quelli italiani (`15:31`), perche' il
    diario contiene entrambi, e si confronta il piu' avanti di tutti - conservativo apposta: un
    dubbio costa una sezione non mostrata, uno sbaglio costa una lettura contaminata.
    """
    adesso = ora.italiana(adesso_utc)
    peggio = None
    for grezzo, in_utc in [(m, True) for m in re.findall(r"\b(\d{1,2}:\d{2})Z", testo)] + \
                          [(m, False) for m in re.findall(r"\b(\d{1,2}:\d{2})(?![Z\d])", testo)]:
        try:
            hh, mm = (int(x) for x in grezzo.split(":"))
        except ValueError:
            continue
        if not (0 <= hh <= 23 and 0 <= mm <= 59):
            continue
        quando = adesso.replace(hour=hh, minute=mm)
        if in_utc:
            quando = ora.italiana(adesso.astimezone(dt.timezone.utc)
                                  .replace(hour=hh, minute=mm))
        if quando > adesso and (peggio is None or quando > peggio):
            peggio = quando
    return peggio.strftime("%H:%M") if peggio else None


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
    p.add_argument("--giorno", help="chiave del file della giornata; senza, si deduce dal bridge")
    p.add_argument("--barre", type=int, default=15, help="quante barre di tape mostrare")
    p.add_argument("--chart", help="id o strumento; senza, si chiede al bridge quale e' registrato")
    a = p.parse_args()
    if a.chart:
        CHART[:] = ["--chart", a.chart]

    # Lo strumento e la data NON si cablano. Cablati, sbagliano in silenzio: il 19 settembre
    # l'hook portava `CHART="GCZ6"` mentre ATAS era su NQZ6, quindi ogni richiesta veniva
    # rifiutata e il giro dichiarava il bridge irraggiungibile pur essendo acceso — e stampava
    # il quadro dell'oro del giorno prima come se fosse quello di oggi. Un contesto sbagliato e'
    # peggio di nessun contesto, perche' si legge come se fosse giusto.
    #
    # E la data e' quella **di mercato**, non quella locale: in replay le due divergono di giorni.
    if not CHART:
        c = bridge("charts") or {}
        registrati = c.get("charts") or []
        if len(registrati) == 1:
            CHART[:] = ["--chart", registrati[0]["instrument"]]
        elif len(registrati) > 1:
            print(f"  {len(registrati)} chart registrati: serve --chart fra "
                  f"{', '.join(x['instrument'] for x in registrati)}")

    h = bridge("health")
    g = a.giorno
    if not g:
        if h:
            strumento, data_mercato = h["instrument"], h["marketTimeUtc"][:10]
            # In replay la giornata e' un file a parte, perche' la seduta e' gia' stata operata
            # dal vivo e il suo diario non va sovrascritto: si cerca prima quello.
            # NQZ6 ha inoltre file storici senza prefisso, debito aperto: si prova anche cosi'.
            candidati = [f"{strumento}-{data_mercato}", data_mercato]
            if data_mercato != dt.date.today().isoformat():
                candidati.insert(0, f"{strumento}-{data_mercato}-replay")
            g = next((k for k in candidati if (GIORNATE / f"{k}.md").exists()), candidati[0])
        else:
            g = dt.date.today().isoformat()

    # `--giorno` e' una CHIAVE, non una data: puo' portare il prefisso dello strumento
    # (`ESZ6-2026-09-16`), come vuole il passo 8 di come-si-apre-un-asset.md. Qui serve anche la
    # data nuda, per calcolare "ieri" e per il file del giorno precedente: si separano le due cose
    # invece di assumere che coincidano. Il 16 settembre l'assunzione ha fatto fallire l'hook al
    # primo strumento col prefisso.
    # La data si estrae, non si ricava tagliando la stringa: la chiave puo' avere un prefisso
    # davanti (`ESZ6-`) e anche un suffisso dietro (`-replay`), e un rpartition se li portava
    # appresso — `dt.date.fromisoformat("2026-09-14-replay")` fa fallire tutto il giro.
    m_data = re.search(r"(\d{4}-\d{2}-\d{2})", g)
    data_nuda = m_data.group(1) if m_data else dt.date.today().isoformat()
    prefisso = g[:m_data.start()] if m_data and m_data.start() else ""

    # 1 --------------------------------------------------------------- il bridge
    titolo(1, "IL BRIDGE E IL CONTRATTO")
    if not h:
        print("  bridge non raggiungibile — ogni misura che segue e' vecchia o assente")
    else:
        oggi_mercato = h["marketTimeUtc"][:10]
        avviso = ""
        if oggi_mercato != dt.date.today().isoformat():
            avviso = f"   <-- REPLAY: la data locale e' {dt.date.today().isoformat()}"
        print(f"  {h['instrument']} {h['timeFrame']}  {h['bars']:,} barre  "
              f"ora di mercato {ora.completa(h['marketTimeUtc'])} "
              f"{ora.sigla_di(h['marketTimeUtc'])}{avviso}")
        print(f"  TUTTI GLI ORARI DI QUESTO BLOCCO SONO ITALIANI. La cash di New York apre alle "
              f"{ora.hhmm(h['marketTimeUtc'][:10] + 'T13:30:00Z')}.")
        print(f"  file della giornata usato: {g}.md")
        r = bridge("rollovers")
        if r and r.get("rollovers"):
            print(f"  rollover noti: {r['rollovers']}")

    # 2-3 ------------------------------------------------------- la giornata di oggi
    oggi = GIORNATE / f"{g}.md"
    titolo(2, f"DOVE ERAVAMO — {oggi.name}")
    if not oggi.exists():
        print(f"  {oggi.name} non esiste: la giornata non e' stata aperta")
    else:
        t = oggi.read_text(encoding="utf-8")
        m = re.search(r"^\*\*Stato\*\*.*$|^Stato:.*$", t, re.M)
        if m:
            print(f"  {m.group(0)}")
        s = coda_sezione(t, ["dove eravamo"], 30)
        if not s:
            print("  nessuna sezione 'Dove eravamo'")
        else:
            avanti = ora_piu_avanti(s, h["marketTimeUtc"]) if h else None
            if avanti:
                # IL DIARIO PUO' ESSERE PIU' AVANTI DEL REPLAY, e allora racconta il futuro.
                # E' successo il 20 settembre: il replay era alle 13:11Z e la sezione era datata
                # 13:31Z, scritta in un giro precedente dello stesso replay. Stamparla contamina
                # la lettura con venti minuti che non sono ancora successi, e la disciplina del
                # replay e' non sapere come finisce. Si dichiara e non si stampa.
                print(f"  SEZIONE NON MOSTRATA: e' datata {avanti} e il mercato e' a "
                      f"{ora.hhmm(h['marketTimeUtc'])}. In replay il diario di un giro precedente")
                print("  racconta il futuro: leggerlo qui contaminerebbe la lettura.")
            else:
                print(s)

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
    # "ieri" e' il file precedente esistente, non necessariamente il giorno prima: dopo un weekend
    # o una pausa puo' essere di tre giorni fa, e il nome stampato lo dice.
    titolo(4, f"IL FRAMING DELLA SEDUTA PRECEDENTE (sezione 1 del file) — "
              f"{prec[-1].name if prec else 'assente'}")
    if prec:
        t = prec[-1].read_text(encoding="utf-8")
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

    # 4bis ---------------------------------------------- il quadro: framing + COT
    # Sta qui, automatico, e non a richiesta. La location (dove si e' costruito il valore, dove
    # sono i POC, dove sono i vuoti) e il posizionamento (chi e' lungo, con che ritardo) sono
    # CONTESTO: vanno messi davanti agli occhi prima che la risposta cominci, non cercati quando
    # qualcuno li chiede. Procedura in metodo/il-framing-e-il-cot-si-leggono-insieme.md.
    titolo("4bis", "IL QUADRO — PROFILE FRAMING E COT")
    q, fonte = "", ""
    for f in [oggi] + ([prec[-1]] if prec else []):
        if f and f.exists():
            m = re.search(r"<!-- QUADRO -->(.*?)<!-- /QUADRO -->", f.read_text(encoding="utf-8"), re.S)
            if m and m.group(1).strip():
                q, fonte = m.group(1).strip(), f.name
                break
    if q:
        if fonte != oggi.name:
            print(f"  ATTENZIONE: nessun quadro per oggi, questo viene da {fonte} ed e' vecchio")
        for r in q.splitlines()[:26]:
            print(f"  {r}")
    else:
        print("  NESSUN QUADRO. Il framing e il COT non sono stati fatti, o non sono stati")
        print("  scritti fra <!-- QUADRO --> e <!-- /QUADRO --> nel file della giornata.")
        print("  Procedura: docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md")
        print("  Una lettura data senza il quadro e' una lettura sulle ultime barre.")

    # 5 ------------------------------------------------------------ cosa e' scattato
    titolo(5, "COSA E' GIA' SCATTATO OGGI (dal diario, non dedotto)")
    ann = GIORNATE / f"annotazioni-{g}.json"
    if not ann.exists():
        print("  nessuna annotazione")
    else:
        v = json.load(ann.open(encoding="utf-8"))
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
        for s in json.load(sc.open(encoding="utf-8")):
            print(f"  {s.get('prezzo'):>10}  {s['sigla']:<26} [{s.get('verso','-')}]")

    # 7 ------------------------------------------------------------------- il tape
    titolo(7, "IL TAPE RECENTE")
    # la finestra e' la seduta globex in corso: dalle 22:00 CEST di ieri, cioe' 20:00Z.
    # Il giorno e' quello DI MERCATO: in replay la data locale e' un'altra, e usarla chiedeva
    # barre che nel replay non esistono ancora.
    base = h["marketTimeUtc"][:10] if h else data_nuda
    ieri = (dt.date.fromisoformat(base) - dt.timedelta(days=1)).isoformat()
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
            print(f"  {ora.hhmm(c['time'])}  H{c['high']:9.2f} L{c['low']:9.2f} C{c['close']:9.2f}"
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
        print(f"  {len(cs)} barre, {ora.data_e_ora(cs[0]['time'])} -> "
              f"{ora.hhmm(cs[-1]['time'])}, {tot:,} lotti")
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
