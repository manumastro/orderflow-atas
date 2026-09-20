#!/usr/bin/env python3
"""L'orario con cui si parla a chi opera: quello italiano.

**La convenzione, e vale ovunque si scriva un orario che una persona legge.**

    dentro   UTC. Il bridge parla UTC, le finestre nei file di configurazione sono in Z,
             i confronti fra barre si fanno in UTC. Non si tocca niente di tutto questo.
    fuori    ora italiana. Il tape, il profilo, gli avvisi, il pannello, le letture dal vivo.

Il motivo e' che chi guarda il grafico ha un orologio solo, ed e' il suo. Un tape stampato in
Z chiede una sottrazione a ogni riga, e la sottrazione si sbaglia proprio quando si ha fretta.

**Perche' si converte invece di sommare due ore.** Lo scarto fra Roma e UTC non e' costante:
e' +2 in ora legale e +1 in inverno, e le date di cambio dell'Europa non coincidono con quelle
degli Stati Uniti. Nelle due settimane in cui i due calendari non si sovrappongono, l'apertura
della cash di New York cade a un'ora italiana diversa dal solito. Sommare una costante sbaglia
proprio li', cioe' nei giorni in cui l'orario e' l'unica cosa che e' cambiata.

**In replay si converte l'ora di mercato, non l'orologio del muro.** L'ora che conta e' quella
della barra: un replay del 14 settembre alle 13:19Z si legge 15:19, qualunque giorno sia oggi.
"""
from __future__ import annotations

import datetime as dt

try:
    from zoneinfo import ZoneInfo

    ROMA = ZoneInfo("Europe/Rome")
except Exception:  # pragma: no cover - senza tzdata si resta su UTC, dichiarandolo
    ROMA = dt.timezone.utc


#: Vero quando il fuso italiano non e' disponibile e gli orari restano in UTC. Chi stampa un
#: orario lo controlla e lo dichiara, invece di far passare un UTC per un'ora italiana.
SENZA_FUSO = ROMA is dt.timezone.utc


def _parse(quando: str | dt.datetime) -> dt.datetime:
    if isinstance(quando, dt.datetime):
        t = quando
    else:
        testo = quando.strip()
        if testo.endswith("Z"):
            testo = testo[:-1] + "+00:00"
        t = dt.datetime.fromisoformat(testo)
    if t.tzinfo is None:
        t = t.replace(tzinfo=dt.timezone.utc)
    return t


def italiana(quando: str | dt.datetime) -> dt.datetime:
    """Il momento, spostato sul fuso italiano. L'istante non cambia, cambia come si legge."""
    return _parse(quando).astimezone(ROMA)


def hhmm(quando: str | dt.datetime) -> str:
    """`15:19` - l'orario italiano di una barra, per il tape e le etichette."""
    return italiana(quando).strftime("%H:%M")


def hhmmss(quando: str | dt.datetime) -> str:
    """`15:19:04`, per gli avvisi e i log, dove il secondo distingue due righe vicine."""
    return italiana(quando).strftime("%H:%M:%S")


def data_e_ora(quando: str | dt.datetime) -> str:
    """`14-09 15:19`, quando serve anche il giorno: profili a cavallo della mezzanotte."""
    return italiana(quando).strftime("%d-%m %H:%M")


def completa(quando: str | dt.datetime) -> str:
    """`2026-09-14 15:19` - l'intestazione di un file o di una sezione."""
    return italiana(quando).strftime("%Y-%m-%d %H:%M")


def con_utc(quando: str | dt.datetime) -> str:
    """`15:19 (13:19Z)`.

    Si usa **solo** dove l'orario italiano dovra' essere riportato in una finestra di
    configurazione, che resta in Z: cosi' chi legge non deve rifare la conversione a mano
    per scrivere un `da`/`a`. Altrove la doppia forma e' rumore.
    """
    t = _parse(quando)
    return f"{t.astimezone(ROMA).strftime('%H:%M')} ({t.astimezone(dt.timezone.utc).strftime('%H:%M')}Z)"


def sigla() -> str:
    """`CEST` o `CET`, da mettere una volta in testa a un blocco di orari."""
    if SENZA_FUSO:
        return "UTC"
    return dt.datetime.now(ROMA).strftime("%Z") or "Roma"


def sigla_di(quando: str | dt.datetime) -> str:
    """Come `sigla()`, ma per il momento indicato: in replay conta quello, non adesso."""
    if SENZA_FUSO:
        return "UTC"
    return italiana(quando).strftime("%Z") or "Roma"
