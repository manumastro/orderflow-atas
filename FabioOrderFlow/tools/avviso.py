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

    # WINDOWS: nessun equivalente diretto di osascript, e due strade che si somigliano.
    #
    #   toast (WinRT)   e' quello che Windows 11 mostra davvero: compare sopra ad ATAS, suona,
    #                   e resta nel centro notifiche. Richiede un AppUserModelID gia' registrato
    #                   nel sistema - si usa quello di PowerShell, che c'e' sempre.
    #   balloon tip     la vecchia NotifyIcon. Non da' errore, ma su Windows 11 spesso NON
    #                   compare: l'icona viene creata e distrutta prima che il sistema la mostri.
    #                   Resta come ripiego per le build dove il toast non passa.
    #
    # Verificato su Windows 11 il 20 settembre 2026: entrambe le chiamate tornano senza errore,
    # quindi il codice di uscita NON basta a dire che la notifica si e' vista. Il log resta la
    # fonte di verita'.
    testo_ps = testo.replace("&", "e").replace("<", "(").replace(">", ")")
    titolo_ps = titolo.replace('"', "'").replace("&", "e").replace("<", "(").replace(">", ")")

    toast = (
        "$ErrorActionPreference='Stop';"
        r"$app='{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}\WindowsPowerShell\v1.0\powershell.exe';"
        "[Windows.UI.Notifications.ToastNotificationManager,Windows.UI.Notifications,"
        "ContentType=WindowsRuntime]|Out-Null;"
        "[Windows.Data.Xml.Dom.XmlDocument,Windows.Data.Xml.Dom,"
        "ContentType=WindowsRuntime]|Out-Null;"
        "$x=New-Object Windows.Data.Xml.Dom.XmlDocument;"
        "$x.LoadXml('<toast><visual><binding template=\"ToastGeneric\">"
        f"<text>{titolo_ps}</text><text>{testo_ps}</text>"
        "</binding></visual>"
        "<audio src=\"ms-winsoundevent:Notification.Looping.Alarm2\"/></toast>');"
        "$t=New-Object Windows.UI.Notifications.ToastNotification $x;"
        "[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier($app).Show($t)"
    )
    try:
        esito = subprocess.run(["powershell", "-NoProfile", "-Command", toast],
                               capture_output=True, timeout=15)
        if esito.returncode == 0:
            return
    except Exception:
        pass

    try:
        balloon = (
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
        subprocess.run(["powershell", "-NoProfile", "-Command", balloon],
                       capture_output=True, timeout=15)
    except Exception:
        pass
