#!/usr/bin/env python3
"""Ricalcola una condizione dichiarata e la mostra sul pannello del chart, in continuo.

Nasce dalla stessa esigenza di `livelli_vivi.py`, applicata a un numero invece che a un
prezzo: seguire un ritorno o una rottura chiede di guardare due contatori — quante chiusure
sopra un livello, quanti big trade lo hanno difeso — e aspettare che qualcuno li legga a voce
è la stessa perdita di tempo che aspettare un livello ridisegnato a mano.

La condizione si dichiara in un file, non nel programma: stessa separazione di scenari.py e di
livelli_vivi.py. Ogni voce ha un `tipo`:

    chiusure_sopra / chiusure_sotto   quante delle ultime N barre CHIUSE hanno il close
                                      sopra/sotto un prezzo
    big_trade_sopra / big_trade_sotto quanti big trade (volume >= soglia) sono avvenuti
                                      sopra/sotto un prezzo, da un istante in poi
    volume_vs_mediana                l'ultima barra chiusa contro la mediana delle ultime N

Ogni voce diventa una riga sul pannello: `{nome}: {valore}` — verde se la soglia dichiarata è
raggiunta, altrimenti il colore neutro. Il pannello non decide niente: dice il numero.

    ./condizione_viva.py condizioni.json --chart NQZ6 --ogni 5
"""

from __future__ import annotations

import argparse
import datetime as dt
import json
import statistics
import sys
import time
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
import bridge  # noqa: E402

VERDE = "#66BB6A"
NEUTRO = "#D0D0D0"
ROSSO = "#EF5350"


def momento(raw: str, giorno: dt.date) -> dt.datetime:
    testo = raw.strip()
    if len(testo) <= 6 and ":" in testo:
        ora = dt.time.fromisoformat(testo.rstrip("Z"))
        return dt.datetime.combine(giorno, ora, tzinfo=dt.timezone.utc)
    return bridge.parse_time(testo)


def barre_chiuse(base: str, chart: str, adesso: dt.datetime, minuti: int) -> list[dict]:
    da = adesso - dt.timedelta(minutes=minuti + 2)
    dati = bridge.get(base, "/candles", {"chart": chart, "from": bridge.iso(da), "to": bridge.iso(adesso)})
    barre = [b for b in dati["candles"] if b["volume"] > 0]
    # l'ultima e' quasi certamente in formazione: la si esclude sempre, come nel giro d'orizzonte
    if barre and bridge.parse_time(barre[-1]["time"]) >= adesso - dt.timedelta(minutes=1):
        barre = barre[:-1]
    return barre


def valuta(voce: dict, base: str, chart: str, adesso: dt.datetime) -> tuple[str, bool]:
    tipo = voce["tipo"]
    livello = float(voce.get("livello", 0))

    if tipo in ("chiusure_sopra", "chiusure_sotto"):
        n = int(voce.get("barre", 3))
        chiuse = barre_chiuse(base, chart, adesso, n + 5)[-n:]
        if tipo == "chiusure_sopra":
            k = sum(1 for b in chiuse if b["close"] > livello)
        else:
            k = sum(1 for b in chiuse if b["close"] < livello)
        return f"{k}/{len(chiuse)}", k == len(chiuse) and len(chiuse) > 0

    if tipo in ("big_trade_sopra", "big_trade_sotto"):
        soglia = float(voce.get("soglia", 60))
        da = momento(voce["da"], adesso.date())
        if da >= adesso:
            return "in attesa", False
        dati = bridge.get(base, "/cumulative", {
            "chart": chart, "from": bridge.iso(da), "to": bridge.iso(adesso), "minVolume": int(soglia),
        })
        trades = dati.get("trades", [])
        if tipo == "big_trade_sopra":
            rilevanti = [t for t in trades if t["lastPrice"] > livello]
        else:
            rilevanti = [t for t in trades if t["lastPrice"] < livello]
        n = int(voce.get("almeno", 1))
        return f"{len(rilevanti)}", len(rilevanti) >= n

    if tipo == "volume_vs_mediana":
        n = int(voce.get("barre", 20))
        chiuse = barre_chiuse(base, chart, adesso, n + 5)[-n:]
        if len(chiuse) < 2:
            return "n/d", False
        ultima = chiuse[-1]["volume"]
        mediana = statistics.median(b["volume"] for b in chiuse[:-1])
        rapporto = ultima / mediana if mediana else 0
        soglia = float(voce.get("soglia", 1.0))
        return f"{ultima:,.0f} vs med {mediana:,.0f} ({rapporto:.1f}x)".replace(",", "."), rapporto >= soglia

    raise SystemExit(f"tipo sconosciuto: {tipo}")


def riga(voce: dict, valore: str, soddisfatta: bool) -> dict:
    colore = VERDE if soddisfatta else voce.get("color", NEUTRO)
    return {"text": f"{voce['nome']}: {valore}", "color": colore}


def giro(base: str, chart: str, definizioni: list[dict]) -> list[dict]:
    salute = bridge.get(base, "/health", {"chart": chart})
    adesso = bridge.parse_time(salute["marketTimeUtc"])
    intestazione = {"text": f"{salute['instrument']}  {salute['marketTimeUtc'][11:16]}Z", "color": "#90A4AE"}
    righe = [intestazione]
    for voce in definizioni:
        try:
            valore, ok = valuta(voce, base, chart, adesso)
        except Exception as errore:
            valore, ok = f"errore: {errore}", False
        righe.append(riga(voce, valore, ok))
    return righe


def main() -> None:
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("definizione", help="JSON con l'elenco delle condizioni")
    p.add_argument("--chart", required=True)
    p.add_argument("--base", help="base URL del bridge; normalmente si scopre da sola")
    p.add_argument("--ogni", type=int, default=5, metavar="SECONDI", help="intervallo di ricalcolo (default 5)")
    p.add_argument("--prova", action="store_true", help="un giro solo, stampa e non deposita")
    args = p.parse_args()

    base = bridge.discover(args.base)

    def un_giro() -> list[dict]:
        # si rilegge a ogni giro: correggere una soglia non deve richiedere un riavvio
        definizioni = json.loads(Path(args.definizione).read_text())
        righe = giro(base, args.chart, definizioni)
        for r in righe[1:]:
            print(f"  {r['text']}")
        return righe

    if args.prova:
        un_giro()
        print("--prova: niente depositato")
        return

    precedente: list[str] = []
    while True:
        try:
            righe = un_giro()
            testi = [r["text"] for r in righe]
            if testi != precedente:
                bridge.send(base, "/watch", "POST", {"chart": args.chart}, righe)
                precedente = testi
        except (Exception, SystemExit) as errore:
            # SystemExit: bridge.py lo usa per il bridge irraggiungibile (es. ATAS in riavvio
            # per un redeploy) - non deve spegnere il ciclo, che deve continuare a riprovare.
            print(f"  ! giro saltato: {errore}")
        sys.stdout.flush()
        time.sleep(args.ogni)


if __name__ == "__main__":
    main()
