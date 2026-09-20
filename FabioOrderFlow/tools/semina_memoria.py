#!/usr/bin/env python3
"""Installa la memoria di orientamento dell'agente a partire dai file versionati nel repo.

La memoria dell'agente vive in `~/.claude/projects/<percorso-del-progetto>/memory/`, **fuori dal
controllo di git**, con un nome di cartella derivato dal percorso: cambiando macchina non arriva,
e il 20 settembre 2026, spostando il repo dal Mac a un PC, e' sparita per intero.

La risposta non e' rinunciare alla memoria — serve, perche' e' l'unica cosa che l'agente ha
**prima** di aver letto qualcosa — ma tenerne il contenuto nel repo, dove si versiona e viaggia,
e installarlo con un comando.

    python3 FabioOrderFlow/tools/semina_memoria.py            # installa
    python3 FabioOrderFlow/tools/semina_memoria.py --prova    # dice solo cosa farebbe
    python3 FabioOrderFlow/tools/semina_memoria.py --in PERCORSO

**Cosa va nella memoria e cosa no.** Qui ci va solo l'orientamento: dove siamo, come ci siamo
arrivati, cosa e' gia' stato chiuso. Le regole, il metodo e le procedure stanno nel repo e
arrivano dall'obbligo di lettura di CLAUDE.md. Una memoria che duplica il repo prima o poi diverge
da lui, e **non si accorge di divergere**: e' successo, due voci su undici dicevano il contrario
di CLAUDE.md.
"""

from __future__ import annotations

import argparse
import shutil
import sys
from pathlib import Path

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")

SORGENTE = Path(__file__).resolve().parent.parent.parent / "docs" / "research" / "memoria-di-orientamento"
PROGETTI = Path.home() / ".claude" / "projects"


def cartella_memoria(repo: Path) -> Path:
    """Il nome della cartella e' il percorso del progetto con i separatori sostituiti da trattini.

    Non e' documentato come stabile, quindi se esiste gia' una cartella che finisce col nome del
    repo si preferisce quella: e' la prova che l'agente ci ha gia' lavorato, e indovinare la
    regola di trasformazione su un sistema diverso e' proprio il tipo di assunzione che si rompe
    cambiando macchina.
    """
    if PROGETTI.is_dir():
        esistenti = [d for d in PROGETTI.iterdir() if d.is_dir() and d.name.endswith(repo.name)]
        if len(esistenti) == 1:
            return esistenti[0] / "memory"
        if len(esistenti) > 1:
            elenco = "\n  ".join(str(d) for d in esistenti)
            raise SystemExit(
                f"piu' di una cartella di progetto finisce con '{repo.name}':\n  {elenco}\n"
                "scegline una con --in <percorso della cartella memory>")

    mangiato = str(repo).replace("/", "-").replace("\\", "-").replace(":", "-")
    return PROGETTI / mangiato / "memory"


def main() -> None:
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--in", dest="destinazione", help="la cartella memory da riempire, se la si sa gia'")
    p.add_argument("--prova", action="store_true", help="stampa cosa farebbe e non scrive niente")
    a = p.parse_args()

    repo = Path(__file__).resolve().parent.parent.parent
    if not SORGENTE.is_dir():
        raise SystemExit(f"non trovo i file di orientamento in {SORGENTE}")

    destinazione = Path(a.destinazione) if a.destinazione else cartella_memoria(repo)
    sorgenti = sorted(SORGENTE.glob("*.md"))

    print(f"repo         {repo}")
    print(f"destinazione {destinazione}")
    for f in sorgenti:
        esiste = (destinazione / f.name).exists()
        print(f"  {'sovrascrive' if esiste else 'crea       '}  {f.name}")

    if a.prova:
        print("--prova: niente scritto")
        return

    # Si sovrascrive senza chiedere: questi file sono una copia di quelli nel repo, e una copia
    # locale modificata a mano e' esattamente la divergenza che si vuole evitare. Quello che va
    # conservato si scrive nel repo.
    destinazione.mkdir(parents=True, exist_ok=True)
    for f in sorgenti:
        shutil.copy2(f, destinazione / f.name)
    print(f"installati {len(sorgenti)} file. La memoria e' attiva dalla prossima sessione.")


if __name__ == "__main__":
    main()
