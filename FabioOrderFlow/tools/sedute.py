#!/usr/bin/env python3
"""Le sedute, in un posto solo: ogni orario di sessione nel fuso del suo mercato, convertito per data.

**La regola** (docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md): un orario di
sessione non si scrive mai in UTC fisso. Si scrive nel fuso del mercato e si converte in UTC per la
data della seduta, con l'ora legale di quel giorno.

    la cassa di New York     09:30-16:00 New York    13:30Z-20:00Z d'estate, 14:30Z-21:00Z d'inverno
    il pit del crude         09:00-14:30 New York
    il giorno CME            dalle 17:00 di Chicago = 18:00 di New York del giorno prima (22:00Z o 23:00Z)
    la London                dalle 08:00 di Londra all'apertura di New York

**Perche'.** Fino al 27 ottobre 2026 la cassa era scritta 13:30Z-20:00Z in tutto il codice, giusta
solo con l'ora legale americana. New York torna all'ora solare il 1 novembre 2026: dal 2 novembre
la cassa e' 14:30Z-21:00Z, e con l'orario fisso valore, POC, apertura, chiusura e le sedute del COT
si sarebbero misurati un'ora prima. Londra e Roma cambiano ora un'altra domenica (25 ottobre):
fra il 25/10 e il 1/11 la cassa apre alle 14:30 italiane, non alle 15:30.

La gemella in C# e' `FabioOrderFlow/src/Observation/LeSedute.cs`: le due devono dire la stessa cosa.

    python FabioOrderFlow/tools/sedute.py                 la prova: 30/10 e 2/11
    python FabioOrderFlow/tools/sedute.py 2026-11-02      le sedute di un giorno
"""
from __future__ import annotations

import datetime as dt
from zoneinfo import ZoneInfo

UTC = dt.timezone.utc
NEW_YORK = ZoneInfo("America/New_York")
LONDRA = ZoneInfo("Europe/London")
CHICAGO = ZoneInfo("America/Chicago")
ROMA = ZoneInfo("Europe/Rome")

CASSA = ("09:30", "16:00")          # New York: NQ, ES, oro
PIT_CRUDE = ("09:00", "14:30")      # New York: il pit NYMEX
INIZIO_GIORNO = "18:00"             # New York, il giorno prima (17:00 di Chicago)
APERTURA_LONDRA = "08:00"           # Londra
IVB_MINUTI = 30                     # la prima mezz'ora di cassa

_FUSI = {"NY": NEW_YORK, "ET": NEW_YORK, "LON": LONDRA, "CT": CHICAGO, "Z": UTC}


def _data(giorno) -> dt.date:
    if isinstance(giorno, dt.datetime):
        return giorno.date()
    if isinstance(giorno, dt.date):
        return giorno
    return dt.date.fromisoformat(str(giorno)[:10])


def _istante(quando) -> dt.datetime:
    if isinstance(quando, dt.datetime):
        t = quando
    else:
        t = dt.datetime.fromisoformat(str(quando).strip().replace("Z", "+00:00"))
    return t if t.tzinfo else t.replace(tzinfo=UTC)


def utc(giorno, hhmm: str, fuso=NEW_YORK) -> dt.datetime:
    """L'istante UTC di un orario `hhmm` nel `fuso`, nella data `giorno`."""
    h, m = (int(x) for x in hhmm.split(":"))
    locale = dt.datetime.combine(_data(giorno), dt.time(h, m), fuso)
    return locale.astimezone(UTC)


def momento(testo: str, giorno) -> dt.datetime:
    """Come `LeSedute.Momento` in C#: `09:30NY`, `08:00LON`, `17:00CT`, `13:30Z` (UTC fisso) o ISO."""
    t = testo.strip().upper()
    if len(t) <= 9 and ":" in t:
        for sigla in ("LON", "NY", "ET", "CT", "Z"):
            if t.endswith(sigla):
                return utc(giorno, t[: -len(sigla)], _FUSI[sigla])
        return utc(giorno, t, UTC)
    return _istante(testo).astimezone(UTC)


def iso(t: dt.datetime) -> str:
    """`2026-11-02T14:30:00Z`, la forma che il bridge accetta."""
    return t.astimezone(UTC).strftime("%Y-%m-%dT%H:%M:%SZ")


def hhmm_z(t: dt.datetime) -> str:
    return t.astimezone(UTC).strftime("%H:%M")


def e_il_crude(strumento: str | None) -> bool:
    s = (strumento or "").upper()
    return s.startswith("CL") or s.startswith("MCL")


def orari_di_cassa(strumento: str | None = None) -> tuple[str, str]:
    """Gli orari di cassa nel fuso di New York per lo strumento."""
    return PIT_CRUDE if e_il_crude(strumento) else CASSA


def cassa(giorno, strumento: str | None = None) -> tuple[dt.datetime, dt.datetime]:
    """La cassa di una data, in UTC."""
    da, a = orari_di_cassa(strumento)
    return utc(giorno, da), utc(giorno, a)


def cassa_hhmm(giorno, strumento: str | None = None) -> tuple[str, str]:
    """`("14:30", "21:00")`: la cassa di quella data in ore UTC, per confronti con `time[11:16]`."""
    da, a = cassa(giorno, strumento)
    return hhmm_z(da), hhmm_z(a)


def etichetta_cassa(giorno, strumento: str | None = None) -> str:
    """`14:30Z-21:00Z`: l'orario vero della cassa di quella data."""
    da, a = cassa_hhmm(giorno, strumento)
    return f"{da}Z-{a}Z"


def descrizione_cassa(strumento: str | None = None) -> str:
    """`09:30-16:00 New York, convertita per data`: la definizione, per le fonti dei rapporti."""
    da, a = orari_di_cassa(strumento)
    return f"{da}-{a} New York, convertita in UTC per data"


def inizio_giorno(giorno) -> dt.datetime:
    """L'inizio (UTC) del giorno di mercato `giorno`: le 18:00 di New York del giorno prima."""
    return utc(_data(giorno) - dt.timedelta(days=1), INIZIO_GIORNO)


def giorno_di_mercato(quando) -> str:
    """Il giorno di mercato di un istante: dalle 18:00 di New York la notte appartiene alla seduta dopo."""
    ny = _istante(quando).astimezone(NEW_YORK)
    return (ny.date() + dt.timedelta(days=1) if ny.hour >= 18 else ny.date()).isoformat()


def londra(giorno) -> tuple[dt.datetime, dt.datetime]:
    """La London di una data: dalle 08:00 di Londra all'apertura di New York."""
    return utc(giorno, APERTURA_LONDRA, LONDRA), cassa(giorno)[0]


def ivb(giorno, strumento: str | None = None) -> tuple[dt.datetime, dt.datetime]:
    """L'IVB: la prima mezz'ora di cassa."""
    da = cassa(giorno, strumento)[0]
    return da, da + dt.timedelta(minutes=IVB_MINUTI)


def sessione(quando) -> str:
    """Quale libro c'e' davanti: `newyork` (cassa), `londra` (dalle 08:00 di Londra), `notte`."""
    t = _istante(quando).astimezone(UTC)
    g = t.date()
    da, a = cassa(g)
    if da <= t < a:
        return "newyork"
    if londra(g)[0] <= t < da:
        return "londra"
    return "notte"


def in_cassa(quando, strumento: str | None = None) -> bool:
    t = _istante(quando).astimezone(UTC)
    da, a = cassa(t.date(), strumento)
    return da <= t < a


def in_finestra(quando, da: str, a: str, chiusa: bool = False) -> bool:
    """Vero se l'istante cade fra `da` e `a` nella SUA data. Gli estremi seguono `momento`:
    `09:30NY` si converte per data, `13:30` o `13:30Z` resta UTC fisso (le finestre degli
    strumenti di ricerca di settembre erano scritte cosi')."""
    t = _istante(quando).astimezone(UTC)
    g = t.date()
    inizio, fine = momento(da, g), momento(a, g)
    return inizio <= t <= fine if chiusa else inizio <= t < fine


def _prova() -> int:
    casi = [("2026-10-30", "13:30Z-20:00Z"), ("2026-11-02", "14:30Z-21:00Z"),
            ("2026-03-06", "14:30Z-21:00Z"), ("2026-03-09", "13:30Z-20:00Z")]
    sbagliate = 0
    for g, atteso in casi:
        vero = etichetta_cassa(g)
        sbagliate += vero != atteso
        print(f"{'ok' if vero == atteso else 'NO'}  cassa {g}  {vero}" + ("" if vero == atteso else f"  atteso {atteso}"))
    altri = [
        ("inizio giorno 2026-11-02", iso(inizio_giorno("2026-11-02")), "2026-11-01T23:00:00Z"),
        ("inizio giorno 2026-10-30", iso(inizio_giorno("2026-10-30")), "2026-10-29T22:00:00Z"),
        ("giorno di mercato 2026-11-02T22:30Z", giorno_di_mercato("2026-11-02T22:30:00Z"), "2026-11-02"),
        ("giorno di mercato 2026-11-02T23:00Z", giorno_di_mercato("2026-11-02T23:00:00Z"), "2026-11-03"),
        ("London 2026-10-27", iso(londra("2026-10-27")[0]), "2026-10-27T08:00:00Z"),
        ("17:00CT 2026-11-02", iso(momento("17:00CT", "2026-11-02")), "2026-11-02T23:00:00Z"),
        ("apertura in italiano 2026-10-30", cassa("2026-10-30")[0].astimezone(ROMA).strftime("%H:%M"), "14:30"),
        ("apertura in italiano 2026-11-02", cassa("2026-11-02")[0].astimezone(ROMA).strftime("%H:%M"), "15:30"),
        ("sessione 2026-11-02T14:00Z", sessione("2026-11-02T14:00:00Z"), "londra"),
    ]
    for cosa, vero, atteso in altri:
        sbagliate += vero != atteso
        print(f"{'ok' if vero == atteso else 'NO'}  {cosa}  {vero}" + ("" if vero == atteso else f"  atteso {atteso}"))
    print("TUTTE GIUSTE" if not sbagliate else f"{sbagliate} SBAGLIATE")
    return sbagliate


if __name__ == "__main__":
    import sys
    if len(sys.argv) > 1:
        g = sys.argv[1]
        print(f"{g}: cassa {etichetta_cassa(g)} ({descrizione_cassa()}); giorno dalle {iso(inizio_giorno(g))}; "
              f"London dalle {iso(londra(g)[0])}")
    else:
        raise SystemExit(_prova())
