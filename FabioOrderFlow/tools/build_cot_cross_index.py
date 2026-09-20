"""Costruisce la vista cross-index COT Legacy dai dati estratti da Tradingster.

Ingresso: i file JSON prodotti navigando le pagine `tradingster.com/cot/legacy-futures/<codice>`
ed estraendo i dataProvider amCharts (serie netta, Long e Short per categoria).

Uscita: un CSV settimanale unificato e un riepilogo descrittivo della metrica
cross-index descritta nel live Q1. Lo script non emette segnali operativi: calcola
soltanto grandezze osservabili e le confronta con la variazione di prezzo successiva.
"""

import csv
import json
import statistics
import sys
from pathlib import Path

# codice contratto CFTC -> etichetta leggibile
MARKETS = {
    "13874": "SP500",
    "209742": "NASDAQ100_MINI",
    "124603": "DOW_x5",
    "239742": "RUSSELL2000_MINI",
}

# soglia dichiarata: |variazione netta| / (|variazione long| + |variazione short|).
# Operazionalizza la frase del live "1000 long contro 5000 short e' uno skew forte".
# E' una convenzione di questo repository, non una regola validata.
SKEW_THRESHOLD = 0.30

# numero minimo di indici concordi perche' la settimana sia dichiarata "coerente"
CONSENSUS_MIN = 3

# criterio alternativo, basato sulla grandezza: la variazione netta della settimana
# viene normalizzata sulla deviazione standard delle 52 settimane precedenti.
# Serve perche' il rapporto di skew satura a +-1 quando le variazioni long e short
# hanno segno opposto, e quindi da solo non distingue una settimana normale da una estrema.
Z_WINDOW = 52
Z_THRESHOLD = 1.0


def load(raw_dir):
    series = {}
    for code, label in MARKETS.items():
        path = Path(raw_dir) / f"cot-{code}.json"
        if not path.exists():
            print(f"manca {path}", file=sys.stderr)
            continue
        payload = json.loads(path.read_text(encoding="utf-8"))
        rows = {}
        for bucket, key in (("net", "net"), ("long", "long"), ("short", "short")):
            for row in payload[bucket]:
                date = row["date"][:10]
                rows.setdefault(date, {})[key] = row
        series[label] = {
            "market": payload["market"],
            "asof": " ".join(payload["asof"].split()),
            "rows": rows,
        }
    return series


def weekly_table(series):
    """Una riga per mercato e data, con livelli, variazioni e skew dichiarato."""
    out = []
    for label, data in series.items():
        dates = sorted(data["rows"])
        prev = None
        for date in dates:
            row = data["rows"][date]
            if not {"net", "long", "short"} <= set(row):
                prev = None
                continue
            nc_long = row["long"]["NonCommercial"]
            nc_short = row["short"]["NonCommercial"]
            record = {
                "date": date,
                "market": label,
                "nc_long": nc_long,
                "nc_short": nc_short,
                "nc_net": row["net"]["NonCommercial"],
                "close": row["net"].get("close"),
            }
            if prev is not None:
                d_long = nc_long - prev["nc_long"]
                d_short = nc_short - prev["nc_short"]
                d_net = d_long - d_short
                gross = abs(d_long) + abs(d_short)
                record.update(
                    d_long=d_long,
                    d_short=d_short,
                    d_net=d_net,
                    skew_ratio=round(d_net / gross, 4) if gross else 0.0,
                )
            out.append(record)
            prev = record
    out.sort(key=lambda r: (r["date"], r["market"]))
    return out


def consensus(table):
    """Per ogni data, conta gli indici con skew forte nella stessa direzione."""
    by_date = {}
    for row in table:
        if "skew_ratio" in row:
            by_date.setdefault(row["date"], []).append(row)
    result = []
    for date in sorted(by_date):
        rows = by_date[date]
        strong_long = [r for r in rows if r["skew_ratio"] >= SKEW_THRESHOLD]
        strong_short = [r for r in rows if r["skew_ratio"] <= -SKEW_THRESHOLD]
        verdict = "nessuno"
        if len(strong_long) >= CONSENSUS_MIN and not strong_short:
            verdict = "long"
        elif len(strong_short) >= CONSENSUS_MIN and not strong_long:
            verdict = "short"
        result.append(
            {
                "date": date,
                "markets": len(rows),
                "strong_long": len(strong_long),
                "strong_short": len(strong_short),
                "verdict": verdict,
            }
        )
    return result


def consensus_zscore(table):
    """Come `consensus`, ma con soglia sulla grandezza della variazione netta."""
    by_market = {}
    for row in table:
        if "d_net" in row:
            by_market.setdefault(row["market"], []).append(row)

    scores = {}
    for market, rows in by_market.items():
        rows.sort(key=lambda r: r["date"])
        for i, row in enumerate(rows):
            if i < Z_WINDOW:
                continue
            window = [r["d_net"] for r in rows[i - Z_WINDOW:i]]
            sd = statistics.pstdev(window)
            if sd:
                scores.setdefault(row["date"], {})[market] = row["d_net"] / sd

    result = []
    for date in sorted(scores):
        values = scores[date]
        if len(values) < len(MARKETS):
            continue
        up = sum(1 for v in values.values() if v >= Z_THRESHOLD)
        down = sum(1 for v in values.values() if v <= -Z_THRESHOLD)
        verdict = "nessuno"
        if up >= CONSENSUS_MIN and down == 0:
            verdict = "long"
        elif down >= CONSENSUS_MIN and up == 0:
            verdict = "short"
        result.append({"date": date, "scores": values, "verdict": verdict})
    return result


def forward_response(cons, table, weeks=4):
    """Variazione percentuale del Nasdaq nelle `weeks` settimane successive, per verdetto.

    E' una descrizione condizionale, non un test statistico: i campioni sono piccoli
    e le settimane si sovrappongono.
    """
    closes = {r["date"]: r["close"] for r in table
              if r["market"] == "NASDAQ100_MINI" and r.get("close")}
    dates = sorted(closes)
    index = {d: i for i, d in enumerate(dates)}
    buckets = {}
    for row in cons:
        i = index.get(row["date"])
        if i is None or i + weeks >= len(dates):
            continue
        start, end = closes[dates[i]], closes[dates[i + weeks]]
        buckets.setdefault(row["verdict"], []).append((end - start) / start)
    return buckets


def report(name, cons, table):
    print(f"\n--- {name} ---")
    total = len(cons)
    for verdict in ("long", "short", "nessuno"):
        n = sum(1 for r in cons if r["verdict"] == verdict)
        print(f"  {verdict:8s} {n:4d} settimane ({n / total:5.1%})")
    print("  variazione NQ a 4 settimane per verdetto:")
    for verdict, values in forward_response(cons, table).items():
        values.sort()
        mean = sum(values) / len(values)
        positive = sum(1 for v in values if v > 0) / len(values)
        print(f"    {verdict:8s} n={len(values):4d}  media={mean:+.2%}  "
              f"mediana={values[len(values) // 2]:+.2%}  positive={positive:.1%}")


def main():
    raw_dir = sys.argv[1]
    out_csv = Path(sys.argv[2])
    series = load(raw_dir)
    table = weekly_table(series)

    fields = ["date", "market", "nc_long", "nc_short", "nc_net",
              "d_long", "d_short", "d_net", "skew_ratio", "close"]
    with out_csv.open("w", newline="", encoding="utf-8") as fh:
        writer = csv.DictWriter(fh, fieldnames=fields, extrasaction="ignore")
        writer.writeheader()
        writer.writerows(table)

    print(f"mercati: {len(series)}  righe settimanali: {len(table)}  csv: {out_csv}")
    for label, data in series.items():
        print(f"  {label}: {data['asof']}")

    report(f"criterio A: rapporto di skew >= {SKEW_THRESHOLD}, consenso {CONSENSUS_MIN}/4",
           consensus(table), table)
    report(f"criterio B: z-score >= {Z_THRESHOLD} su {Z_WINDOW} settimane, consenso {CONSENSUS_MIN}/4",
           consensus_zscore(table), table)

    print("\nultime 6 settimane, criterio B:")
    for row in consensus_zscore(table)[-6:]:
        scores = "  ".join(f"{m}={v:+.2f}" for m, v in sorted(row["scores"].items()))
        print(f"  {row['date']}  {scores}  -> {row['verdict']}")


if __name__ == "__main__":
    main()
