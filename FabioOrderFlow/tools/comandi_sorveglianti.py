#!/usr/bin/env python3
"""Stampa i comandi esatti per accendere i sorveglianti, gia' tarati sulla sessione in corso.

**Non accende niente.** I sorveglianti si accendono come `Monitor`, e un `Monitor` lo apre
l'agente: questo programma prepara le righe, con i numeri giusti e l'ora giusta dentro.

Serve perche' **le soglie dei sorveglianti sono parametri della sessione, non costanti**, e
sbagliarle e' silenzioso in tutti e due i sensi. Otto punti di `--strappo` sono un movimento a
Londra e rumore su New York; centosessanta lotti di `--vol-viva` sono una barra grossa a Londra e
un quarto di barra normale in cash. Un sorvegliante tarato sulla sessione sbagliata non da'
errore: **parla troppo o non parla**, e in entrambi i casi si smette di ascoltarlo.

Le tarature vengono dalla misura in `docs/research/metodo/la-sessione-di-londra.md`: su NQZ6
Londra fa il **60% dell'escursione** di New York con il **15% del volume** — 102 lotti per barra
M1 contro 613, p95 volume 298 contro 2.027, p95 delta 54 contro 170.

**`--from` e' sempre ADESSO**, mai l'orario di apertura. Riarmare con un `--from` di tre ore fa
significa rimasticare mezza mattina gia' vista: gli avvisi arrivano tutti insieme, per fatti
finiti, e coprono quello vero. La sola cosa che guarda indietro e' `--storia`, che dice su quante
barre calcolare le soglie — quello **deve** guardare indietro, o lo strumento riparte cieco.

    ./comandi_sorveglianti.py                    # sessione dedotta dall'ora
    ./comandi_sorveglianti.py --sessione newyork # forzata
"""
from __future__ import annotations

import argparse
import subprocess
import sys
from datetime import datetime, timedelta, timezone
from pathlib import Path

HERE = Path(__file__).resolve().parent
RADICE = HERE.parent.parent
GIORNATE = RADICE / "docs/research/giornate"

# Le tre tarature. I numeri non sono opinioni: vengono dai percentili misurati per sessione, e
# ognuno ha accanto da dove viene, perche' il prossimo che li tocca sappia cosa sta cambiando.
TARATURE = {
    "notte": dict(
        strappo=6, vol_viva=100, delta_viva=30, vol_strappo=150,
        nota="globex: libro ancora piu' sottile di Londra, soglie scese di un altro terzo",
    ),
    "londra": dict(
        strappo=8, vol_viva=160, delta_viva=45, vol_strappo=250,
        nota="Londra: 102 lotti/barra M1, p95 vol 298, p95 delta 54",
    ),
    "newyork": dict(
        strappo=14, vol_viva=1000, delta_viva=140, vol_strappo=1500,
        nota="cash New York: 613 lotti/barra M1, p95 vol 2.027, p95 delta 170",
    ),
}


def sessione_da_ora(u: datetime) -> str:
    """Quale libro abbiamo davanti, in UTC.

    I confini sono grossolani di proposito: nella mezz'ora attorno al cambio le due tarature si
    somigliano abbastanza, e un confine finto preciso farebbe credere che il passaggio sia netto.
    """
    m = u.hour * 60 + u.minute
    if 13 * 60 + 30 <= m < 20 * 60:
        return "newyork"
    if 6 * 60 <= m < 13 * 60 + 30:
        return "londra"
    return "notte"


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--chart", default="NQZ6")
    ap.add_argument("--sessione", choices=sorted(TARATURE), help="forza la taratura")
    ap.add_argument("--giorno", help="AAAA-MM-GG (default: oggi in UTC)")
    args = ap.parse_args()

    u = datetime.now(timezone.utc)
    giorno = args.giorno or u.strftime("%Y-%m-%d")
    sess = args.sessione or sessione_da_ora(u)
    t = TARATURE[sess]

    # Un minuto indietro, non l'istante esatto: partendo dal minuto in corso la prima barra e'
    # sempre monca e il primo giro non ha niente da confrontare.
    adesso = (u - timedelta(minutes=1)).strftime("%Y-%m-%dT%H:%M")

    livelli = GIORNATE / f"livelli-{giorno}.json"
    scenari = GIORNATE / f"scenari-{giorno}.json"

    print(f"# sessione: {sess.upper()}  ({t['nota']})")
    print(f"# ora UTC {u.strftime('%H:%M')}  ->  --from {adesso}")
    print()

    if not livelli.exists():
        print(f"# ATTENZIONE: {livelli.name} non esiste. I livelli della giornata vanno scritti")
        print("#             PRIMA di accendere la sveglia, non dopo.")
    if not scenari.exists():
        print(f"# ATTENZIONE: {scenari.name} non esiste. Niente scenari da valutare.")
    if not (livelli.exists() and scenari.exists()):
        print()

    print("## 1. scenari — e' successo quello che avevamo previsto?")
    print(f"python3 -u FabioOrderFlow/tools/scenari.py --giorno {giorno} --chart {args.chart} \\")
    print(f"    --from {adesso} --intervallo 5")
    print()

    print("## 2. sveglia del tape — e' successo qualcosa su un livello che conta?")
    print("python3 -u FabioOrderFlow/tools/sveglia_tape.py \\")
    print(f"    --livelli docs/research/giornate/livelli-{giorno}.json \\")
    print(f"    --from {adesso} --storia 600 --presidio 5 --avviso 25 --vicino 3 \\")
    print(f"    --isteresi 4 --intervallo 3 --chart {args.chart}")
    print()

    print("## 3. sveglia del movimento — si e' mosso, dovunque fosse?")
    print(f"python3 -u FabioOrderFlow/tools/sveglia_movimento.py --chart {args.chart} \\")
    print(f"    --strappo {t['strappo']} --intervallo 2 --vol-viva {t['vol_viva']} \\")
    print(f"    --delta-viva {t['delta_viva']} --vol-strappo {t['vol_strappo']} --calma 20")
    print()

    print("## 4. permesso di fatto — l'accettazione oltre i bordi dell'IVB")
    if sess == "newyork" and (u.hour * 60 + u.minute) >= 14 * 60 + 30:
        print(f"# IVB 13:30-14:30Z chiusa: misura i bordi e sostituisci --alto/--basso.")
        print(f"python3 -u FabioOrderFlow/tools/permesso_di_fatto.py --chart {args.chart} \\")
        print(f"    --from {adesso} --alto <TETTO_IVB> --basso <PAVIMENTO_IVB> --attesa 20")
    else:
        print("# NON si accende: l'IVB di oggi non e' ancora chiusa (serve dopo le 14:30Z).")
        print("# Senza i suoi bordi il permesso di fatto non e' calcolabile, e inventarli")
        print("# produce un permesso che sembra misurato e non lo e'.")
    print()

    vivi = subprocess.run(["bash", str(HERE / "spegni_sorveglianti.sh"), "--lista"],
                          capture_output=True, text=True)
    print("## chi sta gia' girando")
    print(vivi.stdout.strip() or "(nessuno)")
    if "trovati" in vivi.stdout:
        print()
        print("# NON accendere sopra a questi: gli avvisi arriverebbero doppi e non si saprebbe")
        print("# piu' quale dei due dica la verita'. Prima /spegni, poi si riaccende.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
