#!/usr/bin/env python3
"""Dice se i livelli STATICI vanno rifatti. Non li rifa', e non decide niente d'altro.

**A cosa serve.** I livelli vivi si aggiornano da soli; i fissi no, e non devono — sono
affermazioni dell'analisi (*questa mensola e' stata difesa sei volte*) e un programma non puo'
scriverle senza classificare. Ma qualcuno deve accorgersi che sono **scaduti**, e accorgersene
costa: rileggere tape, profilo e contesto a ogni giro sarebbe il lavoro di un agente, ogni minuto,
per niente.

Questo programma risponde a quella domanda e basta. Costa due chiamate al bridge e si lancia a
mano, quando si vuole sapere se la mappa regge.

    NIENTE   il quadro regge
    SERVE    il perche', in chiaro, cosi' chi rifa' i livelli sa da dove partire

**Non sveglia niente e non lancia niente.** Nasceva come filtro davanti a un sottoagente che
rifaceva i fissi in background: quella strada e' stata chiusa il 20 settembre 2026, il giorno
stesso in cui era stata aperta, e il motivo sta in
`docs/research/metodo/i-livelli-statici-la-strada-del-sottoagente.md`. Chi rifa' i livelli statici
resta un problema aperto; per ora e' l'agente, su richiesta, in primo piano.

**Le cinque ragioni, e nessuna e' un giudizio sul mercato.** Sono tutte misure di *scadenza della
mappa*, non di cosa il prezzo stia facendo:

    fuori fascia    il prezzo e' uscito dall'intervallo per cui i fissi erano stati derivati
    attraversato    un livello fisso ha cambiato lato: il tetto e' diventato pavimento
    niente in gioco nessun livello entro il raggio: si viaggia fuori dalla mappa
    sessione        e' passata l'apertura della cash, che ridefinisce la giornata
    deriva          il prezzo si e' spostato di piu' della soglia da quando la mappa fu scritta

**Il riferimento e' il prezzo di quando il file e' stato scritto**, non quello dell'ultimo giro:
altrimenti una deriva lenta non viene mai vista, perche' ogni giro la confronta con quello prima.
Lo stato sta in `~/.fabio-livelli-statici.json` ed e' rigenerabile: perderlo costa un falso NIENTE
al primo giro, non un errore.

    python3 serve_rifare.py docs/research/giornate/livelli-vivi-NQZ6-AAAA-MM-GG.json --chart NQZ6
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import sys
from pathlib import Path

HERE = Path(__file__).resolve().parent
sys.path.insert(0, str(HERE))

import bridge                                                        # noqa: E402
import ora                                                           # noqa: E402

STATO = Path.home() / ".fabio-livelli-statici.json"


def leggi_stato() -> dict:
    try:
        return json.loads(STATO.read_text(encoding="utf-8"))
    except Exception:
        return {}


def scrivi_stato(stato: dict) -> None:
    try:
        STATO.write_text(json.dumps(stato, indent=2), encoding="utf-8")
    except Exception:
        # Perdere lo stato costa un falso NIENTE al giro dopo, non un errore: non vale la pena
        # far fallire il controllo per un file che si rigenera da solo.
        pass


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("definizione", help="JSON con l'elenco dei livelli, fissi e vivi")
    ap.add_argument("--chart", required=True)
    ap.add_argument("--base")
    ap.add_argument("--margine", type=float, default=25.0,
                    help="punti oltre i fissi estremi prima di dire 'fuori fascia' (default 25)")
    ap.add_argument("--raggio", type=float, default=40.0,
                    help="entro quanti punti deve esserci almeno un livello (default 40)")
    ap.add_argument("--deriva", type=float, default=80.0,
                    help="punti di spostamento dalla scrittura oltre i quali la mappa e' vecchia "
                         "(default 80: su NQ e' circa mezza gamba di cash)")
    ap.add_argument("--json", action="store_true", help="risposta in JSON, per chi la legge da codice")
    args = ap.parse_args()

    base = bridge.discover(args.base)
    salute = bridge.get(base, "/health", {"chart": args.chart})
    adesso = bridge.parse_time(salute["marketTimeUtc"])

    percorso = Path(args.definizione)
    definizioni = json.loads(percorso.read_text(encoding="utf-8"))
    mtime = percorso.stat().st_mtime

    da = (adesso - dt.timedelta(minutes=3)).strftime("%Y-%m-%dT%H:%M")
    barre = (bridge.get(base, "/candles", {"chart": args.chart, "from": da}) or {}).get("candles") or []
    if not barre:
        return esito(args, ["il bridge non manda barre: il controllo non e' stato fatto"], adesso,
                     serve=False, incerto=True)
    prezzo = float(barre[-1]["close"])

    # --- il riferimento: prezzo e momento in cui la mappa e' stata scritta --------------------
    stato = leggi_stato()
    chiave = str(percorso.resolve())
    voce = stato.get(chiave)
    if not voce or voce.get("mtime") != mtime:
        voce = {"mtime": mtime, "prezzo": prezzo, "quando": salute["marketTimeUtc"]}
        stato[chiave] = voce
        scrivi_stato(stato)
    riferimento = float(voce["prezzo"])
    scritto = bridge.parse_time(voce["quando"])

    fissi = [float(d["price"]) for d in definizioni if d.get("tipo") == "fisso" and d.get("price")]
    tutti = list(fissi)
    livelli_bridge = bridge.get(base, "/levels", {"chart": args.chart}) or {}
    tutti += [float(l["price"]) for l in (livelli_bridge.get("levels") or [])]

    ragioni: list[str] = []

    if fissi:
        basso, alto = min(fissi) - args.margine, max(fissi) + args.margine
        if not (basso <= prezzo <= alto):
            ragioni.append(
                f"FUORI FASCIA: il prezzo {prezzo:,.2f} e' fuori da {basso:,.2f}-{alto:,.2f}, "
                f"l'intervallo per cui i livelli fissi erano stati derivati")

        for f in fissi:
            if (riferimento - f) * (prezzo - f) < 0:
                ragioni.append(
                    f"ATTRAVERSATO: il fisso {f:,.2f} ha cambiato lato da quando la mappa e' "
                    f"stata scritta ({riferimento:,.2f} -> {prezzo:,.2f}): l'etichetta dice "
                    f"ancora la funzione vecchia")
    else:
        ragioni.append("NESSUN LIVELLO FISSO dichiarato: la mappa e' fatta di soli livelli vivi")

    if tutti and min(abs(prezzo - t) for t in tutti) > args.raggio:
        vicino = min(tutti, key=lambda t: abs(prezzo - t))
        ragioni.append(
            f"NIENTE IN GIOCO: il livello piu' vicino e' {vicino:,.2f}, a "
            f"{abs(prezzo - vicino):,.2f} punti. Si viaggia fuori dalla mappa")

    apertura = adesso.replace(hour=13, minute=30, second=0, microsecond=0)
    if scritto < apertura <= adesso:
        ragioni.append(
            f"SESSIONE: la cash ha aperto ({ora.hhmm(apertura)}) dopo che la mappa e' stata "
            f"scritta ({ora.hhmm(scritto)}). L'apertura ridefinisce la giornata")

    if abs(prezzo - riferimento) > args.deriva:
        ragioni.append(
            f"DERIVA: {abs(prezzo - riferimento):,.2f} punti da quando la mappa e' stata scritta "
            f"({riferimento:,.2f} -> {prezzo:,.2f}), oltre i {args.deriva:g} di soglia")

    return esito(args, ragioni, adesso, serve=bool(ragioni), incerto=False, prezzo=prezzo)


def esito(args, ragioni: list[str], adesso, serve: bool, incerto: bool, prezzo: float = 0.0) -> int:
    if args.json:
        print(json.dumps({"serve": serve, "incerto": incerto, "prezzo": prezzo,
                          "ora": ora.hhmm(adesso), "ragioni": ragioni}, ensure_ascii=False))
    elif incerto:
        print(f"INCERTO  {ora.hhmm(adesso)}  " + "; ".join(ragioni))
    elif serve:
        print(f"SERVE  {ora.hhmm(adesso)}  prezzo {prezzo:,.2f}")
        for r in ragioni:
            print(f"  - {r}")
    else:
        print(f"NIENTE  {ora.hhmm(adesso)}  prezzo {prezzo:,.2f}  la mappa regge")
    # Il codice di uscita e' sempre 0: SERVE non e' un errore, e far fallire il comando
    # costringerebbe chi lo chiama a distinguere un guasto da una risposta.
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
