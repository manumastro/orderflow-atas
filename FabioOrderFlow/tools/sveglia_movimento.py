#!/usr/bin/env python3
"""Sveglia chi analizza quando il PREZZO si muove, dovunque si trovi.

`sveglia_tape.py` guarda i **livelli**: attraversamenti, presidi, tocchi con volume. E' il
sorvegliante giusto per "il prezzo e' arrivato dove ci aspettavamo qualcosa", e per quello resta
acceso. Ma ha un punto cieco strutturale, e non e' un difetto: **fra un livello e l'altro non
parla**, perche' non ha niente da dire.

Il 17 settembre 2026 il punto cieco e' costato. Fra 29.485 e 29.517 non c'era nessun livello
disegnato: per **quattordici minuti** — durante i quali uno short e' andato a bersaglio, si e'
fermato e si e' girato di venti punti — la sorveglianza e' stata muta. Non per un errore: per
costruzione. L'utente se n'e' accorto prima dell'agente, ed e' esattamente il contrario di cio'
per cui i sorveglianti esistono.

**Questo programma guarda il movimento, non la mappa.** Non sa dove sono i livelli e non gli
interessa: dice che il prezzo ha percorso `--strappo` punti da dove stava l'ultima volta che ha
parlato. Il segnale e' *geometrico*, quindi funziona anche in mezzo al nulla, anche su un livello
che nessuno aveva previsto, anche quando la mappa della giornata e' sbagliata.

**Legge la barra viva.** Aspettare la chiusura M1 significa scoprire con cinquantanove secondi di
ritardo un movimento che dura tre minuti. Qui si legge la barra in formazione a ogni giro, e lo si
dichiara nella riga: chi opera deve sapere che quel numero puo' ancora cambiare.

    STRAPPO      il prezzo ha fatto `--strappo` punti dall'ultimo estremo. Si riparte da qui
    BARRA VIVA   la barra in formazione ha gia' volume e delta da barra vera, prima di chiudere
    CALMA        `--calma` minuti senza nessuno strappo: il mercato si e' fermato, e anche
                 questo e' un fatto che cambia cosa si fa

**Perche' l'estremo mobile e non un'ancora fissa.** Con un'ancora fissa una gamba di 30 punti
produce tre avvisi nella stessa direzione e **nessuno** quando si gira. Tenendo il massimo e il
minimo da quando si e' parlato l'ultima volta, l'inversione e' il primo segnale che arriva: e'
proprio il momento in cui una posizione aperta va difesa, ed e' il momento che il 17 settembre e'
passato in silenzio due volte.

    ./sveglia_movimento.py --chart NQZ6 --strappo 10 --intervallo 2

Le soglie sono **parametri dello strumento e della sessione**, non costanti. Dieci punti su NQ a
Londra sono un movimento; su New York sono rumore, perche' il libro e' sei volte piu' spesso
(vedi `docs/research/metodo/la-sessione-di-londra.md`). Si riarma al cambio di sessione come tutti
gli altri sorveglianti.
"""
from __future__ import annotations

import argparse
import json
import subprocess
import sys
import time
from datetime import datetime, timedelta
from pathlib import Path

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
sys.path.insert(0, str(HERE))
from avviso import avvisa                                          # noqa: E402


def mostra(riga: str, muto: bool = False) -> None:
    """Stampa e, se la riga conta, la manda anche in notifica di sistema."""
    print(riga, flush=True)
    if not muto:
        avvisa(riga, titolo="NQ movimento")


def ora(b: dict, fuso: int) -> str:
    return f"{(int(b['time'][11:13]) + fuso) % 24:02d}:{b['time'][14:16]}"


def candele(args) -> list[dict]:
    """Le ultime barre, compresa quella in formazione.

    Si chiede una finestra corta: questo programma non calcola percentili, gli serve solo il
    presente. Il costo per giro deve restare basso perche' il giro e' ogni due secondi.
    """
    da = (datetime.utcnow() - timedelta(minutes=args.finestra)).strftime("%Y-%m-%dT%H:%M")
    cmd = [sys.executable, str(BRIDGE), "candles", "--from", da, "--out", args.cache]
    if args.chart:
        cmd += ["--chart", args.chart]
    subprocess.run(cmd, check=True, timeout=45,
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    return json.load(open(args.cache, encoding="utf-8"))["candles"]


def riassunto(barre: list[dict], minuti: int) -> tuple[int, int]:
    """Volume e delta delle ultime `minuti` barre CHIUSE (l'ultima e' viva e si esclude)."""
    chiuse = barre[:-1][-minuti:]
    return sum(b["volume"] for b in chiuse), sum(b["delta"] for b in chiuse)


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--chart")
    ap.add_argument("--strappo", type=float, default=10.0,
                    help="punti di escursione dall'estremo che fanno scattare l'avviso "
                         "(default 10: taglia per NQ a Londra, alzare per la cash di New York)")
    ap.add_argument("--intervallo", type=float, default=2.0,
                    help="secondi fra un giro e l'altro (default 2)")
    ap.add_argument("--finestra", type=int, default=20,
                    help="minuti di barre da chiedere al bridge a ogni giro (default 20)")
    ap.add_argument("--vol-viva", type=float, default=200.0,
                    help="lotti sulla barra IN FORMAZIONE oltre i quali si avvisa (default 200). "
                         "E' un pavimento ASSOLUTO, non un percentile: un percentile si assottiglia "
                         "insieme al libro e di notte scatta su niente")
    ap.add_argument("--delta-viva", type=float, default=60.0,
                    help="|delta| sulla barra in formazione oltre il quale si avvisa (default 60)")
    ap.add_argument("--vol-strappo", type=float, default=250.0,
                    help="lotti scambiati durante lo strappo sotto i quali lo si marca [SOTTILE] "
                         "(default 250). NON sopprime: un movimento su poco volume resta un "
                         "movimento, ma chi legge deve poterlo scartare in un colpo d'occhio "
                         "invece di imparare a ignorare gli avvisi")
    ap.add_argument("--calma", type=int, default=0,
                    help="minuti di immobilita' dopo i quali dirlo (0 = mai, default)")
    ap.add_argument("--fuso", type=int, default=2, help="ore da aggiungere all'UTC (default 2)")
    ap.add_argument("--cache", default="/tmp/fof-movimento.json")
    args = ap.parse_args()

    barre = candele(args)
    if not barre:
        mostra("NESSUNA BARRA: il bridge risponde ma non manda candele")
        return 1

    viva = barre[-1]
    alto = basso = viva["close"]
    t_alto = t_basso = time.time()
    t_ultimo = time.time()
    vol_da = viva["time"]               # da quale barra contare il volume dello strappo
    detto: dict[str, str] = {}          # chiave -> time della barra per cui si e' gia' parlato

    mostra(f"[movimento attivo] strappo {args.strappo:g} pt, giro ogni {args.intervallo:g}s, "
           f"barra viva oltre {args.vol_viva:g} lotti o |delta| {args.delta_viva:g} "
           f"- prezzo {viva['close']:.2f}")

    while True:
        time.sleep(args.intervallo)
        try:
            barre = candele(args)
        except subprocess.CalledProcessError:
            mostra("[bridge non raggiungibile: CalledProcessError]")
            time.sleep(10)
            continue
        except Exception as e:                                     # noqa: BLE001
            mostra(f"[bridge: {type(e).__name__}]")
            time.sleep(10)
            continue
        if not barre:
            continue

        viva = barre[-1]
        p = viva["close"]
        vol5, d5 = riassunto(barre, 5)
        coda = (f"vol viva {viva['volume']} delta {viva['delta']:+} | "
                f"5m: vol {vol5} delta {d5:+}")

        # --- lo strappo: escursione dall'estremo, non da un'ancora fissa -------------------
        su = p - basso
        giu = alto - p
        if su >= args.strappo or giu >= args.strappo:
            # Il volume percorso dallo strappo, non quello della barra: otto punti su cento lotti
            # e otto punti su mille sono due fatti diversi, e la riga deve dirlo.
            vol = sum(b["volume"] for b in barre if b["time"] >= vol_da)
            peso = "" if vol >= args.vol_strappo else f" [SOTTILE {vol} lotti]"
            if su >= giu:
                secondi = int(time.time() - t_basso)
                mostra(f">>> STRAPPO SU +{su:.2f} pt da {basso:.2f} in {secondi}s{peso} "
                       f"[BARRA VIVA] {ora(viva, args.fuso)} prezzo {p:.2f} | {coda}")
            else:
                secondi = int(time.time() - t_alto)
                mostra(f">>> STRAPPO GIU -{giu:.2f} pt da {alto:.2f} in {secondi}s{peso} "
                       f"[BARRA VIVA] {ora(viva, args.fuso)} prezzo {p:.2f} | {coda}")
            alto = basso = p
            t_alto = t_basso = t_ultimo = time.time()
            vol_da = viva["time"]
            continue

        if p > alto:
            alto, t_alto = p, time.time()
        if p < basso:
            basso, t_basso = p, time.time()

        # --- la barra viva e' gia' una barra vera ------------------------------------------
        if (viva["volume"] >= args.vol_viva or abs(viva["delta"]) >= args.delta_viva) \
                and detto.get("viva") != viva["time"]:
            detto["viva"] = viva["time"]
            rng = viva["high"] - viva["low"]
            pos = (viva["close"] - viva["low"]) / rng if rng else 0.0
            mostra(f"!!! BARRA VIVA {ora(viva, args.fuso)} NON CHIUSA - "
                   f"C {p:.2f} H {viva['high']:.2f} L {viva['low']:.2f} "
                   f"vol {viva['volume']} delta {viva['delta']:+} pos {pos:.2f} | 5m delta {d5:+}")
            t_ultimo = time.time()

        # --- la calma e' un fatto, non un'assenza di fatti ---------------------------------
        if args.calma and time.time() - t_ultimo >= args.calma * 60:
            mostra(f"CALMA: {args.calma} minuti dentro {alto - basso:.2f} punti "
                   f"({basso:.2f}-{alto:.2f}), prezzo {p:.2f} | {coda}")
            t_ultimo = time.time()

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
