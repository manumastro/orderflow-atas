#!/usr/bin/env python3
"""Scrive una lettura sul chart, e la registra nel diario della giornata.

E' il modo in cui una conclusione dell'analisi diventa visibile: non una soglia che scatta, ma
quello che si e' concluso guardando il tape, con la misura che lo sostiene.

Ogni annotazione finisce in due posti:

    docs/research/giornate/annotazioni-AAAA-MM-GG.json   il diario, con l'ora e la misura
    il chart, come linea punteggiata prefissata da `!`   quello che vedi mentre operi

Il diario non e' un sottoprodotto: e' il materiale con cui a fine seduta si scrive la giornata,
e l'unico modo per rileggere una lettura sapendo *quando* e' stata fatta e su quali numeri.

    ./annota.py --prezzo 29306 --tipo rottura \
        --testo "rottura vera, 30m a -6,2%" \
        --misura "2.335 lotti, delta -127, minimi che si estendono"

    ./annota.py --elenco          # cosa c'e' sul chart adesso
    ./annota.py --togli 2         # rimuove la terza annotazione
    ./annota.py --pulisci         # torna ai soli livelli strutturali
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import subprocess
import sys
from pathlib import Path

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
GIORNATE = HERE.parent.parent / "docs" / "research" / "giornate"

PREFISSO = "! "

# Il tipo sceglie il colore e basta: non cambia il significato, che sta nel testo.
TIPI = {
    "rottura":      "#FF4444",
    "reclaim":      "#44DD66",
    "assorbimento": "#00D0FF",
    "rifiuto":      "#FFA033",
    "attesa":       "#BDBDBD",
    "nota":         "#FFD54F",
}


def percorsi(giorno: str) -> tuple[Path, Path]:
    return GIORNATE / f"livelli-{giorno}.json", GIORNATE / f"annotazioni-{giorno}.json"


def leggi(p: Path) -> list:
    return json.loads(p.read_text()) if p.exists() else []


def spingi(livelli: list, chart: str | None) -> None:
    """Il chart e' sempre strutturali + annotazioni, ricostruito da capo.

    `POST /levels` sostituisce l'intera lista, quindi non si aggiunge una riga sola. Ripartire
    ogni volta dai file garantisce che un'annotazione non possa cancellare un livello.
    """
    cmd = [sys.executable, str(BRIDGE), "levels", "--file", "-"]
    if chart:
        cmd += ["--chart", chart]
    subprocess.run(cmd, input=json.dumps(livelli), text=True, check=True,
                   stdout=subprocess.DEVNULL)


def voce_chart(a: dict) -> dict:
    return {
        "price": a["prezzo"],
        "label": f"{PREFISSO}{a['ora']} {a['testo']}",
        "color": TIPI.get(a.get("tipo", "nota"), "#FFFFFF"),
        "style": "dot",
        "width": 1,
        "note": a.get("misura") or "",
    }


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--giorno", default=dt.date.today().isoformat(), help="default: oggi")
    ap.add_argument("--prezzo", type=float, help="il prezzo a cui riferire la lettura")
    ap.add_argument("--testo", help="la lettura, breve: e' cio' che si legge sul chart")
    ap.add_argument("--misura", help="i numeri che la sostengono; non disegnati, tornano sul GET")
    ap.add_argument("--tipo", default="nota", choices=sorted(TIPI), help="solo il colore")
    ap.add_argument("--ora", help="HH:MM; default l'ora attuale")
    ap.add_argument("--elenco", action="store_true", help="mostra le annotazioni di oggi")
    ap.add_argument("--togli", type=int, metavar="N", help="rimuove l'annotazione N (vedi --elenco)")
    ap.add_argument("--pulisci", action="store_true", help="rimuove tutte le annotazioni")
    ap.add_argument("--max", type=int, default=6, help="quante tenerne sul chart (default 6)")
    ap.add_argument("--chart", help="id o strumento, se ne e' registrato piu' di uno")
    args = ap.parse_args()

    p_liv, p_ann = percorsi(args.giorno)
    if not p_liv.exists():
        sys.exit(f"manca {p_liv}: i livelli della giornata sono la base, vanno derivati prima")
    strutturali = [l for l in leggi(p_liv) if not (l.get("label") or "").startswith(PREFISSO)]
    annotazioni = leggi(p_ann)

    if args.elenco:
        for i, a in enumerate(annotazioni):
            print(f"{i:>2}  {a['ora']}  {a['prezzo']:>9.2f}  [{a.get('tipo','nota')}]  {a['testo']}")
            if a.get("misura"):
                print(f"      {a['misura']}")
        print(f"-- {len(annotazioni)} annotazioni, le ultime {args.max} sul chart")
        return

    if args.pulisci:
        annotazioni = []
    elif args.togli is not None:
        if not 0 <= args.togli < len(annotazioni):
            sys.exit(f"indice fuori intervallo: ce ne sono {len(annotazioni)}")
        rimossa = annotazioni.pop(args.togli)
        print(f"tolta: {rimossa['ora']} {rimossa['testo']}")
    else:
        if args.prezzo is None or not args.testo:
            sys.exit("servono --prezzo e --testo, oppure --elenco / --togli / --pulisci")
        annotazioni.append({
            "ora": args.ora or dt.datetime.now().strftime("%H:%M"),
            "prezzo": args.prezzo,
            "tipo": args.tipo,
            "testo": args.testo,
            "misura": args.misura or "",
        })

    p_ann.write_text(json.dumps(annotazioni, ensure_ascii=False, indent=2) + "\n")
    spingi(strutturali + [voce_chart(a) for a in annotazioni[-args.max:]], args.chart)
    print(f"chart: {len(strutturali)} livelli + {min(len(annotazioni), args.max)} annotazioni "
          f"({len(annotazioni)} nel diario di {args.giorno})")


if __name__ == "__main__":
    main()
