#!/usr/bin/env python3
"""Porta un avviso davanti agli occhi di chi opera, senza passare dall'analisi.

Chi guarda il grafico non deve aspettare che l'agente scriva: quando il tape fa qualcosa, lo deve
vedere **subito**. Questo modulo fa arrivare la riga in tre posti, e ognuno serve a un momento
diverso:

    notifica di sistema   compare sopra ad ATAS, con suono. E' l'unica che si vede senza guardare
    un file di log       `tail -f ~/.fabio-avvisi.log` in un terminale a fianco: la cronologia
    lo stdout            resta come prima, ed e' cio' che legge l'agente

Non decide niente e non filtra: filtra chi lo chiama, perche' e' li' che si sa cosa e' importante.
"""
from __future__ import annotations

import datetime as dt
import subprocess
import sys
from pathlib import Path

LOG = Path.home() / ".fabio-avvisi.log"
MACOS = sys.platform == "darwin"
WINDOWS = sys.platform == "win32"


def avvisa(riga: str, titolo: str = "tape", suono: str = "Submarine") -> None:
    """Notifica di sistema + riga nel log. Non solleva mai: un avviso che rompe il monitor
    sarebbe peggio di un avviso mancato."""
    ora = dt.datetime.now().strftime("%H:%M:%S")
    try:
        with LOG.open("a") as f:
            f.write(f"{ora}  {riga}\n")
    except Exception:
        pass

    if not (MACOS or WINDOWS):
        return

    # Il testo va troncato: una notifica lunga viene tagliata dal sistema nel punto sbagliato.
    testo = riga if len(riga) <= 180 else riga[:177] + "..."
    testo = testo.replace('"', "'").replace("\\", "/")

    if MACOS:
        try:
            subprocess.run(
                ["osascript", "-e",
                 f'display notification "{testo}" with title "{titolo}" sound name "{suono}"'],
                capture_output=True, timeout=5)
        except Exception:
            pass
        return

    # WINDOWS: nessun equivalente diretto di osascript. Un balloon tip via WinForms non chiede
    # nessun modulo extra (System.Windows.Forms e' nel .NET di sistema), ma NON E' STATO
    # VERIFICATO su una macchina Windows vera - il repo si sposta la' a settembre 2026 e questo
    # e' il primo tentativo. Se non compare niente, il log resta comunque la fonte di verita'.
    try:
        script = (
            "Add-Type -AssemblyName System.Windows.Forms;"
            "Add-Type -AssemblyName System.Drawing;"
            "$n = New-Object System.Windows.Forms.NotifyIcon;"
            "$n.Icon = [System.Drawing.SystemIcons]::Information;"
            "$n.Visible = $true;"
            f'$n.ShowBalloonTip(6000, "{titolo}", "{testo}", '
            "[System.Windows.Forms.ToolTipIcon]::Info);"
            "Start-Sleep -Seconds 6;"
            "$n.Dispose()"
        )
        subprocess.run(["powershell", "-NoProfile", "-Command", script],
                       capture_output=True, timeout=10)
    except Exception:
        pass
