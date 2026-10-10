#!/usr/bin/env python3
"""Fotografa le tab di ATAS una per una, senza clic: le seleziona l'indicatore da dentro.

Perche': l'agente deve poter vedere cosa e' disegnato su ogni tab del workspace, e da fuori non puo'
cliccarle (ATAS gira con privilegi diversi, e computer use da terminale su Windows non c'e'). Il
Fabio Data Bridge gira dentro ATAS e con `/ui?tab=N&davanti=1` seleziona la tab e porta la finestra in
primo piano; questo script aspetta che il chart si ridisegni e ritaglia lo screenshot sulla sola
finestra di ATAS. Alla fine rimette la tab che era selezionata.

Uso:
    python FabioOrderFlow/tools/foto_atas.py                 # tutte le tab, nella cartella data
    python FabioOrderFlow/tools/foto_atas.py --tab 3 --out C:/percorso
Stampa il percorso di ogni immagine. Lo schermo resta occupato da ATAS per qualche secondo a tab.
"""
from __future__ import annotations

import argparse
import ctypes
import json
import os
import sys
import tempfile
import time
import urllib.parse
import urllib.request
from pathlib import Path


def porta() -> int:
    d = json.loads((Path.home() / ".fabio-data-bridge.json").read_text(encoding="utf-8"))
    return int(d["port"])


def chiedi(percorso: str, **parametri) -> dict:
    url = f"http://127.0.0.1:{porta()}{percorso}"
    if parametri:
        url += "?" + urllib.parse.urlencode(parametri)
    with urllib.request.urlopen(url, timeout=30) as r:
        return json.loads(r.read().decode("utf-8"))


def atas_davanti() -> bool:
    """Vero se la finestra in primo piano e' di ATAS (OFT.PlatformX) e non e' ridotta a icona."""
    u = ctypes.windll.user32
    h = u.GetForegroundWindow()
    if not h or u.IsIconic(h):
        return False
    pid = ctypes.c_ulong()
    u.GetWindowThreadProcessId(h, ctypes.byref(pid))
    k = ctypes.windll.kernel32
    proc = k.OpenProcess(0x1000, False, pid.value)  # PROCESS_QUERY_LIMITED_INFORMATION
    if not proc:
        return False
    try:
        buf = ctypes.create_unicode_buffer(1024)
        n = ctypes.c_ulong(1024)
        if not k.QueryFullProcessImageNameW(proc, 0, buf, ctypes.byref(n)):
            return False
        return buf.value.lower().endswith("oft.platformx.exe")
    finally:
        k.CloseHandle(proc)


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--tab", action="append", help="numero (da 1) o inizio del titolo; ripetibile")
    ap.add_argument("--out", default=os.path.join(tempfile.gettempdir(), "foto_atas"))
    ap.add_argument("--attesa", type=float, default=3.0, help="secondi di attesa dopo il cambio di tab")
    a = ap.parse_args()

    # Pixel veri: senza questo, su uno schermo al 150% si fotografa solo l'angolo in alto a sinistra.
    try:
        ctypes.windll.shcore.SetProcessDpiAwareness(2)
    except Exception:
        ctypes.windll.user32.SetProcessDPIAware()
    from PIL import ImageGrab

    charts = chiedi("/charts")["charts"]
    if not charts:
        print("nessun chart col Fabio Data Bridge", file=sys.stderr)
        return 1
    chart = charts[0]["id"]

    prima = chiedi("/ui", chart=chart, vm="SelectedTab")["oggetti"]
    era = next((p["valore"] for p in prima.get("proprieta", []) if p["nome"] == "Header"), None)
    tutte = chiedi("/ui", chart=chart, tab="1")["tabs"] if not a.tab else None
    quali = a.tab or [str(i + 1) for i in range(len(tutte))]

    out = Path(a.out)
    out.mkdir(parents=True, exist_ok=True)
    restituisci = 0
    try:
        for q in quali:
            r = chiedi("/ui", chart=chart, tab=q, davanti="1")
            restituisci = restituisci or r.get("primaInPrimoPiano", 0)
            time.sleep(a.attesa)
            # Si fotografa solo ATAS: se in primo piano c'e' un'altra finestra (Windows puo' negare
            # il primo piano), non si salva niente, perche' lo screenshot riprenderebbe quella.
            if not atas_davanti():
                print("ATAS non e' in primo piano o e' ridotto a icona: nessuna foto salvata. "
                      "Porta ATAS davanti e rilancia.", file=sys.stderr)
                return 3
            f = r["finestra"]
            x, y = max(0, f["x"]), max(0, f["y"])
            img = ImageGrab.grab(bbox=(x, y, x + f["larghezza"], y + f["altezza"]), all_screens=True)
            nome = out / f"tab_{q}_{r['selezionata'].replace(' ', '_')}.png"
            img.save(nome)
            print(f"{r['selezionata']:<20} {nome}")
    finally:
        if era:
            chiedi("/ui", chart=chart, tab=era)
        # La finestra che l'utente aveva davanti torna davanti.
        if restituisci:
            chiedi("/ui", chart=chart, ridai=str(restituisci))
    return 0


if __name__ == "__main__":
    sys.exit(main())
