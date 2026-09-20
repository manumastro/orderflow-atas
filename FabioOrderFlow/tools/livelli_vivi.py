#!/usr/bin/env python3
"""Ricalcola i livelli che si muovono e li rideposita sul chart, cancellando i vecchi.

Un livello disegnato alle 15:00 descrive il mercato delle 15:00. Alcuni livelli restano veri
per tutta la seduta — un minimo della notte, il bordo di un nodo che nessuno ha piu' toccato —
altri no: il POC in sviluppo, i bordi del valore, il minimo e il massimo di sessione si spostano
a ogni barra. Finche' quelli vecchi restano sul chart non sono un riferimento, sono un residuo,
e chi guarda crede di vedere una misura mentre vede una memoria.

Qui la distinzione e' dichiarata in un file, non dedotta. Ogni livello ha un `tipo`:

    fisso        il prezzo sta nel file e non cambia mai
    poc          il prezzo piu' scambiato della finestra
    vah / val    i bordi del valore (70% del volume) della finestra
    massimo      il massimo della finestra
    minimo       il minimo della finestra
    nodo_top     il bordo alto della fascia piu' pesante della finestra
    nodo_base    il bordo basso della stessa fascia

La finestra si dichiara per livello, perche' finestre diverse misurano popolazioni diverse:
il POC della cash e quello della notte sono due numeri, non due stime dello stesso numero.

    "finestra": {"da": "13:30Z"}                 dalle 13:30Z a adesso
    "finestra": {"da": "2026-09-13T22:00:00Z", "a": "2026-09-14T13:30:00Z"}

L'etichetta e' un modello: `{prezzo}`, `{pct}`, `{lotti}`, `{delta}` vengono sostituiti con la
misura corrente, cosi' il chart dice quanto pesa il livello senza tornare al documento.

    ./livelli_vivi.py docs/research/giornate/livelli-vivi-NQZ6-2026-09-14.json --chart NQZ6

Il deposito **cancella sempre** la lista precedente prima di scrivere: e' la stessa ragione per
cui il POST sostituisce invece di aggiungere, portata fino in fondo. Un livello che l'analisi
non ha appena riconfermato non deve restare sul grafico.
"""

from __future__ import annotations

import argparse
import datetime as dt
import json
import sys
import time
from collections import defaultdict
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
import bridge
import ora  # noqa: E402

TIPI_VIVI = {"poc", "vah", "val", "massimo", "minimo", "nodo_top", "nodo_base"}


def momento(raw: str, giorno: dt.date) -> dt.datetime:
    """Accetta un ISO completo oppure la sola ora ('13:30Z'), che si riferisce a `giorno`."""
    testo = raw.strip()
    if len(testo) <= 6 and ":" in testo:
        ora = dt.time.fromisoformat(testo.rstrip("Z"))
        return dt.datetime.combine(giorno, ora, tzinfo=dt.timezone.utc)
    return bridge.parse_time(testo)


def barre(base: str, chart: str, da: dt.datetime, a: dt.datetime) -> list[dict]:
    dati = bridge.get(base, "/candles", {
        "chart": chart, "from": bridge.iso(da), "to": bridge.iso(a), "levels": "true",
    })
    return [b for b in dati["candles"] if b["volume"] > 0 and b.get("levels")]


def misura(sel: list[dict], passo: float, grana: float) -> dict:
    """Profilo della finestra: volume per prezzo, POC, bordi del valore, fascia piu' pesante.

    Due griglie, e vanno tenute distinte perche' rispondono a due domande diverse.

    `grana` (default 1 punto) e' quella su cui si cercano POC e bordi del valore. Sul tick nudo
    non si cercano: il volume di una notte si spalma su millecinquecento prezzi da un quarto di
    punto, e il singolo tick piu' scambiato puo' finire in una zona che non e' affatto il cuore
    del volume. Il 14 settembre il POC sul tick dava 29.150 mentre la fascia piu' pesante della
    notte era 29.300-29.324, con il 18,4% contro il 13,7%: due misure diverse, e quella sul tick
    avrebbe messo sul chart un POC che non era il POC.

    `passo` (default 25 punti) e' la griglia dei nodi, cioe' delle fasce di cui si dichiara la
    percentuale nelle etichette.

    La barra in formazione va gia' esclusa dal chiamante: una misura presa su una barra aperta
    cambia da sola fra una lettura e la successiva.
    """
    prezzi: dict[float, float] = defaultdict(float)
    delta: dict[float, float] = defaultdict(float)
    for b in sel:
        for l in b["levels"]:
            p = (l["price"] // grana) * grana
            prezzi[p] += l["volume"]
            delta[p] += l["ask"] - l["bid"]

    poc = max(prezzi, key=prezzi.get)
    totale = sum(prezzi.values())
    ordinati = sorted(prezzi)
    i = ordinati.index(poc)
    basso = alto = i
    dentro = prezzi[poc]
    while dentro < 0.7 * totale and (basso > 0 or alto < len(ordinati) - 1):
        giu = prezzi[ordinati[basso - 1]] if basso > 0 else -1
        su = prezzi[ordinati[alto + 1]] if alto < len(ordinati) - 1 else -1
        if su >= giu:
            alto += 1
            dentro += su
        else:
            basso -= 1
            dentro += giu

    fasce: dict[float, float] = defaultdict(float)
    fasce_delta: dict[float, float] = defaultdict(float)
    for p, v in prezzi.items():
        f = (p // passo) * passo
        fasce[f] += v
        fasce_delta[f] += delta[p]
    nodo = max(fasce, key=fasce.get)

    return {
        "poc": poc, "val": ordinati[basso], "vah": ordinati[alto],
        "massimo": max(b["high"] for b in sel), "minimo": min(b["low"] for b in sel),
        "nodo_base": nodo, "nodo_top": nodo + passo - 0.25,
        "totale": totale, "prezzi": prezzi, "delta": delta,
        "fasce": fasce, "fasce_delta": fasce_delta, "nodo": nodo, "passo": passo, "grana": grana,
    }


def peso(m: dict, prezzo: float, tipo: str) -> tuple[float, float, float]:
    """Lotti, percentuale del volume della finestra e delta attribuibili al livello.

    Per un livello di nodo si misura la fascia intera; per gli altri la sola fascia in cui
    cade il prezzo, perche' e' quella che un'etichetta puo' onestamente rivendicare.
    """
    f = (prezzo // m["passo"]) * m["passo"]
    lotti = m["fasce"].get(f, 0.0)
    d = m["fasce_delta"].get(f, 0.0)
    return lotti, 100 * lotti / m["totale"] if m["totale"] else 0.0, d


def numero(x: float) -> str:
    return f"{x:,.0f}".replace(",", ".")


def risolvi(definizione: dict, base: str, chart: str, adesso: dt.datetime, cache: dict) -> dict | None:
    tipo = definizione.get("tipo", "fisso")
    modello = definizione.get("label", "")

    if tipo == "fisso":
        prezzo = float(definizione["price"])
        misure = {"prezzo": prezzo, "pct": "", "lotti": "", "delta": ""}
    else:
        if tipo not in TIPI_VIVI:
            raise SystemExit(f"tipo sconosciuto: {tipo}")
        fin = definizione["finestra"]
        chiave = (fin["da"], fin.get("a", ""), definizione.get("passo", 25), definizione.get("grana", 1))
        if chiave not in cache:
            da = momento(fin["da"], adesso.date())
            a = momento(fin["a"], adesso.date()) if fin.get("a") else adesso
            sel = barre(base, chart, da, a)
            # la barra in formazione e' l'ultima solo se la finestra arriva fino ad adesso
            if not fin.get("a") and sel and bridge.parse_time(sel[-1]["time"]) >= adesso - dt.timedelta(minutes=1):
                sel = sel[:-1]
            if not sel:
                cache[chiave] = None
            else:
                cache[chiave] = misura(sel, float(definizione.get("passo", 25)), float(definizione.get("grana", 1)))
        m = cache[chiave]
        if m is None:
            print(f"  - {definizione.get('nome', tipo)}: finestra ancora vuota, livello non disegnato")
            return None
        # Una finestra appena aperta non ha ancora un profilo: nei primi minuti di cash tutto il
        # volume sta in una fascia sola, e il POC dice "89% del volume" perche' non c'e' altro.
        # Un numero del genere sul chart non e' una misura prematura, e' una misura falsa.
        minimo = float(definizione.get("minimo_lotti", 0))
        if m["totale"] < minimo:
            print(f"  - {definizione.get('nome', tipo)}: {m['totale']:,.0f} lotti su {minimo:,.0f} richiesti, ancora presto")
            return None
        prezzo = float(m[tipo])
        lotti, pct, d = peso(m, prezzo, tipo)
        misure = {"prezzo": prezzo, "pct": f"{pct:.1f}%", "lotti": numero(lotti), "delta": f"{d:+,.0f}".replace(",", ".")}

    # UN LIVELLO VIVO SI DEVE RICONOSCERE SUL CHART. Un POC che si sposta a ogni barra e una
    # mensola scritta stamattina si disegnano identici, e chi guarda non puo' sapere quale delle
    # due sta leggendo: il primo e' una misura di adesso, il secondo e' un fatto che potrebbe
    # essere invecchiato. La tilde in coda al nome e' il marcatore, e il pannello la spiega.
    etichetta = modello.format(**misure)
    if tipo != "fisso":
        nome, sep, resto = etichetta.partition(" · ")
        etichetta = f"{nome} ~{sep}{resto}"

    larghezza = definizione.get("width", 1)

    # IL LIVELLO PORTA IL PROPRIO RUOLO, non solo il prezzo. Serve all'indicatore per due cose
    # che da una lista di prezzi non si possono dedurre:
    #   - disegnare la BANDA della value area, che richiede di sapere quale VAL e quale VAH
    #     appartengono alla stessa area (e i bordi di due aree diverse si somigliano);
    #   - distinguere cio' che conta per la strategia da cio' che e' contesto.
    # `area` si ricava dal nome della regola, che e' gia' scritto cosi': "VAL Europa", "POC cash".
    area = ""
    if tipo in ("poc", "vah", "val"):
        area = definizione.get("nome", "")
        for prefisso in ("VAL", "VAH", "POC"):
            area = area.replace(prefisso, "")
        area = area.strip()

    livello = {
        "price": round(prezzo * 4) / 4,
        "label": etichetta,
        "color": definizione.get("color", "#7A8FA6"),
        "style": definizione.get("style", "solid"),
        "width": larghezza,
        "role": tipo,
        "area": area,
        # Chiave = conta per la strategia. Se non e' dichiarato, lo spessore sul chart e' gia' il
        # modo in cui l'analisi dice cosa conta: e' la convenzione che usava la vecchia sveglia.
        "key": bool(definizione.get("chiave", larghezza >= 2)),
    }
    if definizione.get("note"):
        livello["note"] = definizione["note"]
    # Le condizioni le SCRIVE L'ANALISI e le SPUNTA la macchina. Non sono condizioni armate: non
    # scattano, non avvisano, non fanno niente. Dicono cosa dovrebbe essere vero perche' il livello
    # diventi operabile, e il pannello mostra quali lo sono gia'.
    if definizione.get("condizioni"):
        livello["conditions"] = definizione["condizioni"]
    return livello


def sfoltisci(livelli: list[dict], stacco: float) -> list[dict]:
    """Toglie i livelli troppo vicini per essere letti, tenendo il primo dichiarato.

    Due etichette a cinque punti di distanza su un chart NQ si sovrappongono e diventano
    illeggibili entrambe, quindi il grafico perde due informazioni invece di guadagnarne una.
    Quando due misure coincidono e' comunque un fatto — il POC della notte caduto sul POC
    europeo dice che il cuore del volume si e' spostato — ma e' un fatto che va nella lettura,
    non due righe sovrapposte sul grafico.

    Si tiene il primo in ordine di dichiarazione, perche' l'ordine nel file e' la priorita'
    scelta dall'analisi, e si stampa cosa e' caduto: un livello che sparisce senza dirlo e'
    peggio di uno di troppo.
    """
    tenuti: list[dict] = []
    for l in livelli:
        vicino = next((t for t in tenuti if abs(t["price"] - l["price"]) < stacco), None)
        if vicino:
            print(f"  - {l['label'][:48]}... a {l['price']:,.2f}: coincide con {vicino['price']:,.2f}, non disegnato")
            continue
        tenuti.append(l)
    return sorted(tenuti, key=lambda x: -x["price"])


def main() -> None:
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("definizione", help="JSON con l'elenco dei livelli, fissi e vivi")
    p.add_argument("--chart", required=True, help="id o strumento del chart")
    p.add_argument("--base", help="base URL del bridge; normalmente si scopre da sola")
    p.add_argument("--prova", action="store_true", help="calcola e stampa, senza depositare")
    p.add_argument("--ogni", type=int, metavar="SECONDI",
                   help="ricalcola e rideposita ogni N secondi, per girare come Monitor")
    p.add_argument("--minimo-stacco", type=float, default=8.0,
                   help="punti sotto i quali due livelli si sovrappongono e il secondo cade (default 8)")
    p.add_argument("--out", help="scrive i livelli risolti anche su file")
    args = p.parse_args()

    base = bridge.discover(args.base)
    if args.ogni:
        # Si gira in continuo solo per i livelli vivi: il prezzo si muove e POC, bordi ed estremi
        # con lui. Ogni giro stampa l'ora di mercato, cosi' chi rilegge sa a che minuto guardava
        # il chart quello che c'era sopra.
        precedenti: list[float] = []
        while True:
            try:
                nuovi = giro(base, args)
                prezzi = [l["price"] for l in nuovi]
                if prezzi != precedenti:
                    mossi = len(set(prezzi) - set(precedenti))
                    quali = "primo deposito" if not precedenti else f"{mossi} mossi"
                    print(f"  -> {len(prezzi)} livelli, {quali}")
                    precedenti = prezzi
            except (Exception, SystemExit) as errore:  # SystemExit: bridge.py lo usa per il bridge
                                            # irraggiungibile (es. ATAS in riavvio) - non deve
                                            # spegnere il ciclo, che deve continuare a riprovare
                print(f"  ! giro saltato: {errore}")
            sys.stdout.flush()
            time.sleep(args.ogni)
    giro(base, args)


def giro(base: str, args) -> list[dict]:
    salute = bridge.get(base, "/health", {"chart": args.chart})
    adesso = bridge.parse_time(salute["marketTimeUtc"])
    print(f"chart {salute['chart']} {salute['instrument']} {salute['timeFrame']} — "
          f"ora di mercato {ora.completa(adesso)} {ora.sigla_di(adesso)} (ora italiana)")

    # si rilegge a ogni giro: correggere una definizione non deve richiedere un riavvio
    definizioni = json.loads(Path(args.definizione).read_text(encoding="utf-8"))
    cache: dict = {}
    livelli = []
    for d in definizioni:
        risolto = risolvi(d, base, args.chart, adesso, cache)
        if risolto:
            livelli.append(risolto)

    livelli = sfoltisci(livelli, args.minimo_stacco)
    for l in livelli:
        print(f"  {l['price']:>10,.2f}  {l['label']}")

    if args.out:
        Path(args.out).write_text(json.dumps(livelli, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")
        print(f"scritto {args.out}")

    if args.prova:
        print("--prova: niente depositato")
        return livelli

    # Si cancella sempre prima di scrivere. Il POST sostituisce gia' la lista, ma la cancellazione
    # esplicita e' anche la verifica che si stia parlando col chart giusto: se il clear finisce
    # altrove, il conteggio a zero non torna e ce ne si accorge qui invece che sul grafico.
    bridge.send(base, "/levels", "DELETE", {"chart": args.chart})
    prima = bridge.get(base, "/levels", {"chart": args.chart})
    if prima.get("count"):
        raise SystemExit(f"la cancellazione non ha svuotato il chart: restano {prima['count']} livelli")
    bridge.send(base, "/levels", "POST", {"chart": args.chart}, livelli)
    dopo = bridge.get(base, "/levels", {"chart": args.chart})
    print(f"depositati {dopo['count']} livelli su {salute['instrument']}")
    return livelli


if __name__ == "__main__":
    main()
