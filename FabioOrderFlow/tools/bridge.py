#!/usr/bin/env python3
"""Client del Fabio Data Bridge: interroga ATAS mentre e' in esecuzione.

Il bridge e' un indicatore caricato su un chart ATAS che espone i dati della
piattaforma su `http://127.0.0.1:<porta>`. Questo script li richiede e li salva,
spezzando automaticamente le finestre troppo ampie per una singola richiesta.

Esempi:

    ./bridge.py health
    ./bridge.py charts
    ./bridge.py instrument --chart NQZ6
    ./bridge.py limits
    ./bridge.py rollovers --from 2026-06-01 --to 2026-12-31
    ./bridge.py profile --period LastDay
    ./bridge.py candles --levels --out candles.json
    ./bridge.py cumulative --from 2026-09-04 --to 2026-09-11 --min-volume 50
"""

from __future__ import annotations

import argparse
import json
import os
import sys
import urllib.error
import urllib.parse
import urllib.request
from datetime import datetime, timedelta, timezone

# Il bridge sceglie da solo la prima porta libera dell'intervallo e la annuncia nel file di
# discovery. Il client legge quel file e, se manca, sonda l'intervallo: nessuna porta da passare.
DISCOVERY = os.path.expanduser("~/.fabio-data-bridge.json")
PORT_RANGE = range(8787, 8797)

# ATAS accetta una sola richiesta CumulativeTrades alla volta e limita la profondita'
# di ciascuna; il bridge rispetta il primo vincolo, questo client spezza il secondo.
DEFAULT_WINDOW_DAYS = 7


def parse_time(raw: str) -> datetime:
    """Accetta 'YYYY-MM-DD' oppure un ISO completo. L'assenza di fuso significa UTC."""
    # Python 3.9 non accetta la 'Z' finale in fromisoformat: va tradotta.
    value = datetime.fromisoformat(raw.replace("Z", "+00:00"))
    if value.tzinfo is None:
        value = value.replace(tzinfo=timezone.utc)
    return value.astimezone(timezone.utc)


def iso(value: datetime) -> str:
    return value.astimezone(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")


def probe(port: int, timeout: float = 1.5) -> bool:
    """Verifica che sulla porta risponda davvero un bridge, non un programma qualsiasi."""
    try:
        with urllib.request.urlopen(f"http://127.0.0.1:{port}/health", timeout=timeout) as response:
            return json.loads(response.read()).get("schema", "").startswith("fof-data-bridge")
    except Exception:
        return False


def discover(explicit: str | None) -> str:
    """Risolve l'indirizzo del bridge: opzione esplicita, file di discovery, poi scansione."""
    if explicit:
        return explicit.rstrip("/")

    try:
        with open(DISCOVERY) as handle:
            port = json.load(handle)["port"]
        if probe(port):
            return f"http://127.0.0.1:{port}"
    except Exception:
        pass

    for port in PORT_RANGE:
        if probe(port):
            return f"http://127.0.0.1:{port}"

    raise SystemExit(
        f"nessun bridge trovato sulle porte {PORT_RANGE.start}-{PORT_RANGE.stop - 1}.\n"
        "Verifica che ATAS sia aperto e che l'indicatore Fabio Data Bridge sia su almeno un chart."
    )


def get(base: str, path: str, params: dict | None = None, timeout: float = 300.0):
    url = f"{base}{path}"
    if params:
        clean = {k: v for k, v in params.items() if v is not None}
        url = f"{url}?{urllib.parse.urlencode(clean)}"
    try:
        with urllib.request.urlopen(url, timeout=timeout) as response:
            return json.loads(response.read())
    except urllib.error.HTTPError as error:
        body = error.read().decode("utf-8", "replace")
        try:
            parsed = json.loads(body)
            detail = parsed.get("error", body)
            # Quando piu' chart sono registrati il bridge rifiuta e li elenca: mostrarli evita
            # all'utente un secondo giro su /charts solo per scoprire i nomi.
            for chart in parsed.get("charts", []):
                detail += f"\n  {chart.get('id')}  {chart.get('instrument')}  {chart.get('timeFrame')} {chart.get('chartType')}"
        except json.JSONDecodeError:
            detail = body
        raise SystemExit(f"{path} -> HTTP {error.code}: {detail}") from None
    except urllib.error.URLError as error:
        raise SystemExit(
            f"{path} -> il bridge non risponde su {base} ({error.reason}).\n"
            "Verifica che ATAS sia aperto e che l'indicatore Fabio Data Bridge sia sul chart."
        ) from None


def fetch_cumulative(base: str, args) -> dict:  # noqa: D401
    """Richiede i trade aggregati spezzando la finestra in blocchi ammessi da ATAS."""
    begin, end = parse_time(args.begin), parse_time(args.end)
    window = timedelta(days=args.window_days)

    merged: list[dict] = []
    returned = outside = 0
    truncated = False
    chunks = 0

    cursor = begin
    while cursor < end:
        stop = min(cursor + window, end)
        chunks += 1
        print(f"  finestra {chunks}: {iso(cursor)} -> {iso(stop)}", file=sys.stderr)
        payload = get(
            base,
            "/cumulative",
            {
                "chart": args.chart,
                "from": iso(cursor),
                "to": iso(stop),
                "minVolume": args.min_volume,
                "maxVolume": args.max_volume,
                "mode": args.mode,
                "ticks": "true" if args.ticks else None,
            },
        )
        merged.extend(payload.get("trades", []))
        returned += payload.get("returned", 0)
        outside += payload.get("outsideWindow", 0)
        truncated = truncated or payload.get("truncated", False)
        cursor = stop

    return {
        "schema": "fof-data-bridge-client-v1",
        "fromUtc": iso(begin),
        "toUtc": iso(end),
        "windows": chunks,
        "returned": returned,
        "outsideWindow": outside,
        "truncated": truncated,
        "count": len(merged),
        "trades": merged,
    }


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    parser.add_argument("--base", help="URL del bridge; se omesso viene individuato da solo")
    # `--out` e' accettato sia prima sia dopo il sottocomando: argparse lo consente
    # solo dichiarandolo anche su ogni sottoparser, tramite un parent comune.
    common = argparse.ArgumentParser(add_help=False)
    common.add_argument("--out", help="scrive la risposta su file invece che a video")
    common.add_argument("--chart", help="id o strumento del chart da interrogare; vedi 'charts'")
    parser.add_argument("--out", help=argparse.SUPPRESS)
    parser.add_argument("--chart", help=argparse.SUPPRESS)
    sub = parser.add_subparsers(dest="command", required=True, parser_class=lambda **kw: argparse.ArgumentParser(**kw))

    def add(name):
        return sub.add_parser(name, parents=[common])

    for name in ("health", "charts", "instrument", "limits"):
        add(name)

    session = add("session")
    session.add_argument("--at", help="istante da interrogare, default adesso")

    rollovers = add("rollovers")
    rollovers.add_argument("--from", dest="begin", required=True)
    rollovers.add_argument("--to", dest="end", required=True)
    rollovers.add_argument(
        "--type",
        default="VolumeBasedCurrentEnd",
        choices=["ExpirationDate", "VolumeBasedCurrentEnd", "VolumeBasedNextStart"],
    )

    profile = add("profile")
    profile.add_argument(
        "--period",
        default="CurrentDay",
        choices=["CurrentDay", "LastDay", "CurrentWeek", "LastWeek", "CurrentMonth", "LastMonth", "Contract"],
    )
    profile.add_argument("--session", type=int, help="identificatore di sessione ATAS")
    profile.add_argument("--no-levels", action="store_true", help="omette il volume per prezzo")

    candles = add("candles")
    candles.add_argument("--from", dest="begin")
    candles.add_argument("--to", dest="end")
    candles.add_argument("--from-bar", type=int)
    candles.add_argument("--to-bar", type=int)
    candles.add_argument("--levels", action="store_true", help="include il footprint di ogni barra")

    cumulative = add("cumulative")
    cumulative.add_argument("--from", dest="begin", required=True)
    cumulative.add_argument("--to", dest="end", required=True)
    cumulative.add_argument("--min-volume", type=int, default=0)
    cumulative.add_argument("--max-volume", type=int, default=0)
    cumulative.add_argument("--mode", default="Filter", choices=["Strong", "Medium", "Weak", "Filter", "FilterLimited"])
    cumulative.add_argument("--ticks", action="store_true", help="include i tick di ogni trade aggregato")
    cumulative.add_argument("--window-days", type=int, default=DEFAULT_WINDOW_DAYS)

    depth = add("depth")
    depth.add_argument("--from", dest="begin", required=True)
    depth.add_argument("--to", dest="end", required=True)
    depth.add_argument("--period-seconds", type=int, default=60)

    args = parser.parse_args()
    args.base = discover(args.base)

    if args.command in ("health", "charts", "instrument", "limits"):
        payload = get(args.base, f"/{args.command}", {"chart": args.chart})
    elif args.command == "session":
        payload = get(args.base, "/session", {"chart": args.chart, "at": iso(parse_time(args.at)) if args.at else None})
    elif args.command == "rollovers":
        payload = get(
            args.base,
            "/rollovers",
            {"chart": args.chart, "from": iso(parse_time(args.begin)), "to": iso(parse_time(args.end)), "type": args.type},
        )
    elif args.command == "profile":
        payload = get(
            args.base,
            "/profile",
            {
                "chart": args.chart,
                "period": args.period,
                "session": args.session,
                "levels": "false" if args.no_levels else "true",
            },
        )
    elif args.command == "candles":
        payload = get(
            args.base,
            "/candles",
            {
                "chart": args.chart,
                "from": iso(parse_time(args.begin)) if args.begin else None,
                "to": iso(parse_time(args.end)) if args.end else None,
                "fromBar": args.from_bar,
                "toBar": args.to_bar,
                "levels": "true" if args.levels else None,
            },
        )
    elif args.command == "cumulative":
        payload = fetch_cumulative(args.base, args)
    else:
        payload = get(
            args.base,
            "/depth",
            {
                "chart": args.chart,
                "from": iso(parse_time(args.begin)),
                "to": iso(parse_time(args.end)),
                "periodSeconds": args.period_seconds,
            },
        )

    text = json.dumps(payload, indent=2)
    if args.out:
        with open(args.out, "w") as handle:
            handle.write(text)
        size = len(text)
        print(f"scritto {args.out} ({size:,} byte)")
    else:
        print(text)


if __name__ == "__main__":
    main()
