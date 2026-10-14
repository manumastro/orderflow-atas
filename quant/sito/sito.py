#!/usr/bin/env python3
"""Il sito locale del metodo quantitativo: ogni studio con le sue fasi, i paper, il metodo.

Legge quant/ a ogni richiesta, quindi mostra sempre lo stato del repository: non c'e' niente da
rigenerare. Solo lettura, solo su 127.0.0.1, solo file dentro il repository.

    python quant/sito/sito.py              # http://127.0.0.1:8790
    python quant/sito/sito.py --porta 8800

La pagina (pagina.html) disegna; questo file da' i dati:
    /api/indice          studi con le fasi e i sigilli, ricerche (filoni di paper), metodo, paper
    /api/testo?path=...  il testo di un file del repository
    /raw?path=...        un file cosi' com'e' (i PDF dei paper, che non stanno in git)
"""
from __future__ import annotations

import argparse
import hashlib
import json
import mimetypes
import re
import sys
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
from urllib.parse import parse_qs, urlparse

QUI = Path(__file__).resolve().parent
QUANT = QUI.parent
REPO = QUANT.parent

# Le fasi della metodologia come stanno in uno studio: quant/studi/<idea>/<cartella>.
FASI = [
    (1, "L'idea", "1-idea.md", "metodo/1-come-si-scrive-un-idea.md"),
    (2, "Le regole", "2-regole", "metodo/2-come-si-scrivono-le-regole.md"),
    (3, "Il codice", "3-codice", "metodo/3-come-si-scrive-una-strategia.md"),
    (4, "Il backtest", "4-backtest", "metodo/4-il-backtest.md"),
    (5, "La validazione", "5-validazione", "metodo/5-come-si-valida.md"),
]
STATI_PAPER = ["da cercare", "trovato", "scaricato", "letto", "analizzato"]


def rel(p: Path) -> str:
    return p.relative_to(REPO).as_posix()


def leggi(p: Path) -> str:
    try:
        return p.read_text(encoding="utf-8")
    except (OSError, UnicodeDecodeError):
        return ""


def titolo_md(testo: str, riserva: str) -> str:
    m = re.search(r"^# (.+)$", testo, re.M)
    return m.group(1).strip() if m else riserva


def paragrafo(testo: str, etichetta: str) -> str:
    """Il paragrafo che comincia con **Etichetta**: fino alla riga vuota."""
    m = re.search(rf"^\*\*{etichetta}\*\*:?(.*?)(?:\n\s*\n|\n\*\*|\Z)", testo, re.M | re.S)
    return " ".join(m.group(1).split()) if m else ""


def stati_dalla_mappa() -> dict[str, dict[str, str]]:
    """La tabella "Gli studi" della mappa: nome -> cosa, stato."""
    out = {}
    for riga in leggi(QUANT / "il-metodo-quantitativo.md").splitlines():
        m = re.match(r"\|\s*\[`([^`]+)`\]\(studi/[^)]*\)\s*\|(.*)\|(.*)\|\s*$", riga)
        if m:
            out[m.group(1)] = {"cosa": m.group(2).strip(), "stato": m.group(3).strip()}
    return out


def sigilli(studio: Path, scheda: str) -> list[dict]:
    """I criteri sigillati dello studio: lo sha256 scritto nella scheda contro quello del file oggi."""
    out = []
    cartella = studio / "5-validazione" / "criteri"
    if not cartella.is_dir():
        return out
    for f in sorted(cartella.glob("*.md")):
        if f.stem.endswith("-esito"):
            continue
        attuale = hashlib.sha256(f.read_bytes()).hexdigest()
        # lo sha256 registrato nella scheda, subito dopo una citazione del file
        atteso = None
        for i in (m.end() for m in re.finditer(re.escape(f.name), scheda)):
            h = re.search(r"\b([0-9a-f]{64})\b", scheda[i:i + 400])
            if h:
                atteso = h.group(1)
                break
        esito = f.with_name(f.stem + "-esito.md")
        out.append({"file": rel(f), "atteso": atteso, "attuale": attuale,
                    "intatto": atteso == attuale if atteso else None,
                    "esito": rel(esito) if esito.exists() else None})
    return out


def studi() -> list[dict]:
    mappa = stati_dalla_mappa()
    out = []
    for d in sorted((QUANT / "studi").iterdir()):
        if not d.is_dir():
            continue
        scheda = leggi(d / "1-idea.md")
        fasi = []
        for n, nome, cartella, metodo in FASI:
            p = d / cartella
            if p.is_file():
                files = [rel(p)]
            elif p.is_dir():
                files = [rel(f) for f in sorted(p.rglob("*")) if f.is_file() and f.name != ".gitkeep"]
            else:
                files = []
            fasi.append({"n": n, "nome": nome, "files": files, "metodo": f"quant/{metodo}"})
        out.append({
            "nome": d.name,
            "titolo": titolo_md(scheda, d.name),
            "stato": paragrafo(scheda, "Stato"),
            "mappa": mappa.get(d.name, {}),
            "fasi": fasi,
            "sigilli": sigilli(d, scheda),
        })
    return out


def ricerche() -> list[dict]:
    """I filoni dell'indice dei paper (le sezioni ##), con quanti paper e in che stato."""
    testo = leggi(QUANT / "paper" / "indice-dei-paper.md")
    sezioni = re.split(r"^## ", testo, flags=re.M)[1:]
    out = []
    for s in sezioni:
        titolo, _, corpo = s.partition("\n")
        if titolo.strip() in ("Come Sono Organizzati",):
            continue
        righe = [r for r in corpo.splitlines() if r.startswith("|") and not re.match(r"^\|[\s|:-]+\|$", r)]
        intestazioni = [r for r in righe if re.search(r"\|\s*(sigla|paper|filone|Fabio dice)\s*\|", r)]
        dati = [r for r in righe if r not in intestazioni]
        # le righe "Fabio dice" sono la sintesi, non paper
        sintesi = 0
        blocchi = re.split(r"^### ", corpo, flags=re.M)
        for b in blocchi:
            if b.startswith("Cosa Dicono"):
                sintesi = len([r for r in b.splitlines() if r.startswith("|")]) - 2
        stati = {k: 0 for k in STATI_PAPER}
        for r in dati:
            for k in reversed(STATI_PAPER):
                if re.search(rf"\|\s*{k}\s*\|\s*$", r):
                    stati[k] += 1
                    break
        n = len(dati) - max(sintesi, 0)
        if sum(stati.values()) == 0 and re.search(r"stato: trovato", corpo):
            stati["trovato"] = n
        studio = re.findall(r"studi/([a-z0-9-]+)/", corpo)
        if n <= 0:
            continue
        out.append({"titolo": titolo.strip(), "paper": n, "stati": stati,
                    "studi": sorted(set(studio)), "ancora": titolo.strip()})
    return out


def documenti(cartella: Path) -> list[dict]:
    return [{"path": rel(f), "titolo": titolo_md(leggi(f), f.stem)} for f in sorted(cartella.glob("*.md"))]


def analisi() -> list[dict]:
    return [{"path": rel(f), "titolo": titolo_md(leggi(f), f.parent.name), "filone": f.parent.parent.name,
             "pdf": next((rel(p) for p in f.parent.glob("*.pdf")), None)}
            for f in sorted((QUANT / "paper").rglob("analisi.md"))]


def indice() -> dict:
    mappa = leggi(QUANT / "il-metodo-quantitativo.md")
    return {
        "repo": REPO.name,
        "mappa": "quant/il-metodo-quantitativo.md",
        "mappa_titolo": titolo_md(mappa, "Il metodo quantitativo"),
        "studi": studi(),
        "ricerche": ricerche(),
        "metodo": documenti(QUANT / "metodo"),
        "analisi": analisi(),
        "indice_paper": "quant/paper/indice-dei-paper.md",
    }


def dentro(path: str) -> Path | None:
    """Il file chiesto, solo se sta nel repository."""
    p = (REPO / path).resolve()
    try:
        p.relative_to(REPO)
    except ValueError:
        return None
    return p if p.is_file() else None


class Gestore(BaseHTTPRequestHandler):
    def invia(self, codice: int, corpo: bytes, tipo: str) -> None:
        self.send_response(codice)
        self.send_header("Content-Type", tipo)
        self.send_header("Content-Length", str(len(corpo)))
        self.send_header("Cache-Control", "no-store")
        self.end_headers()
        self.wfile.write(corpo)

    def do_GET(self) -> None:  # noqa: N802 (nome imposto da http.server)
        u = urlparse(self.path)
        q = parse_qs(u.query)
        if u.path in ("/", "/index.html"):
            self.invia(200, (QUI / "pagina.html").read_bytes(), "text/html; charset=utf-8")
        elif u.path == "/api/indice":
            self.invia(200, json.dumps(indice(), ensure_ascii=False).encode("utf-8"), "application/json; charset=utf-8")
        elif u.path in ("/api/testo", "/raw"):
            p = dentro(q.get("path", [""])[0])
            if p is None:
                self.invia(404, b"file non trovato nel repository", "text/plain; charset=utf-8")
            elif u.path == "/api/testo":
                self.invia(200, leggi(p).encode("utf-8"), "text/plain; charset=utf-8")
            else:
                self.invia(200, p.read_bytes(), mimetypes.guess_type(p.name)[0] or "application/octet-stream")
        else:
            self.invia(404, b"non trovato", "text/plain; charset=utf-8")

    def log_message(self, fmt: str, *args) -> None:
        pass


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--porta", type=int, default=8790)
    a = ap.parse_args()
    server = ThreadingHTTPServer(("127.0.0.1", a.porta), Gestore)
    print(f"il metodo quantitativo: http://127.0.0.1:{a.porta}  (Ctrl+C per fermare)", flush=True)
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        pass
    return 0


if __name__ == "__main__":
    sys.exit(main())
