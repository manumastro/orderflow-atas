#!/usr/bin/env python3
"""Describe non-overlapping session-location observations from an event CSV."""

from __future__ import annotations

import argparse
import csv
import hashlib
import json
from collections import Counter, defaultdict
from datetime import datetime
from decimal import Decimal
from pathlib import Path
from statistics import median

REQUIRED_COLUMNS = {
    "eventId",
    "lastTickTimeUtc",
    "responseEndUtc",
    "location",
    "returnedToFrozenPoc",
    "responseLastTicks",
    "complete",
    "tickVolumeMatchesTotal",
    "securityId",
    "connectorId",
}
VALID_LOCATIONS = {
    "above-all-pocs",
    "at-poc",
    "below-all-pocs",
    "between-tied-pocs",
}
MINIMUM_DESCRIPTIVE_COUNT = 3


def timestamp(value: str) -> datetime:
    return datetime.fromisoformat(value)


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def format_decimal(value: Decimal | None) -> str:
    return "n/a" if value is None else format(value, "f")


def read_rows(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8", newline="") as stream:
        reader = csv.DictReader(stream)
        actual_columns = set(reader.fieldnames or [])
        missing = REQUIRED_COLUMNS - actual_columns
        if missing:
            raise ValueError(f"Missing CSV columns: {', '.join(sorted(missing))}")
        rows = list(reader)

    if not rows:
        raise ValueError("The input CSV has no events.")
    return rows


def select_non_overlapping(rows: list[dict[str, str]]) -> tuple[list[dict[str, str]], dict[str, int]]:
    eligible: list[dict[str, str]] = []
    rejection_counts: Counter[str] = Counter()

    for row in rows:
        if row["complete"] != "true":
            rejection_counts["incomplete"] += 1
            continue
        if row["tickVolumeMatchesTotal"] != "true":
            rejection_counts["tick-volume-mismatch"] += 1
            continue
        if row["location"] not in VALID_LOCATIONS:
            rejection_counts["unknown-location"] += 1
            continue
        if not row["securityId"] or not row["connectorId"]:
            rejection_counts["missing-security-metadata"] += 1
            continue
        eligible.append(row)

    eligible.sort(key=lambda row: (timestamp(row["lastTickTimeUtc"]), int(row["eventId"])))
    selected: list[dict[str, str]] = []
    next_allowed: datetime | None = None
    for row in eligible:
        event_time = timestamp(row["lastTickTimeUtc"])
        if next_allowed is not None and event_time < next_allowed:
            rejection_counts["overlaps-prior-selected-response"] += 1
            continue
        selected.append(row)
        next_allowed = timestamp(row["responseEndUtc"])

    for previous, current in zip(selected, selected[1:]):
        if timestamp(current["lastTickTimeUtc"]) < timestamp(previous["responseEndUtc"]):
            raise ValueError("The selected response windows overlap.")

    return selected, dict(sorted(rejection_counts.items()))


def category_summary(rows: list[dict[str, str]]) -> dict[str, dict[str, object]]:
    grouped: dict[str, list[dict[str, str]]] = defaultdict(list)
    for row in rows:
        grouped[row["location"]].append(row)

    result: dict[str, dict[str, object]] = {}
    for location in sorted(VALID_LOCATIONS):
        subset = grouped[location]
        final_moves = [Decimal(row["responseLastTicks"]) for row in subset]
        count = len(subset)
        result[location] = {
            "count": count,
            "describable": count >= MINIMUM_DESCRIPTIVE_COUNT,
            "returnedToFrozenPoc": sum(row["returnedToFrozenPoc"] == "true" for row in subset),
            "didNotReturnToFrozenPoc": sum(row["returnedToFrozenPoc"] != "true" for row in subset),
            "medianResponseLastTicks": format_decimal(median(final_moves)) if final_moves else "n/a",
            "positiveResponseLastTicks": sum(move > 0 for move in final_moves),
            "negativeResponseLastTicks": sum(move < 0 for move in final_moves),
            "zeroResponseLastTicks": sum(move == 0 for move in final_moves),
        }
    return result


def write_selected_csv(rows: list[dict[str, str]], path: Path) -> None:
    with path.open("w", encoding="utf-8", newline="") as stream:
        if not rows:
            raise ValueError("No non-overlapping events were selected.")
        writer = csv.DictWriter(stream, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_report(
    input_csv: Path,
    selected_csv: Path,
    summary: dict[str, object],
    report_path: Path,
) -> None:
    categories = summary["categories"]
    category_lines = []
    for location, values in categories.items():
        category_lines.extend(
            [
                f"{location}:",
                f"  eventi selezionati: {values['count']}",
                f"  descrivibile: {str(values['describable']).lower()}",
                f"  ritorno POC: {values['returnedToFrozenPoc']}",
                f"  nessun ritorno POC: {values['didNotReturnToFrozenPoc']}",
                f"  mediana prezzo finale (tick): {values['medianResponseLastTicks']}",
                f"  finale positivo / negativo / zero: {values['positiveResponseLastTicks']} / {values['negativeResponseLastTicks']} / {values['zeroResponseLastTicks']}",
            ]
        )

    location_counts = {location: categories[location]["count"] for location in sorted(VALID_LOCATIONS)}
    describable_locations = [location for location, count in location_counts.items() if count >= MINIMUM_DESCRIPTIVE_COUNT]
    report_path.write_text(
        "\n".join(
            [
                "# Esplorazione: Location POC Con Eventi Non Sovrapposti",
                "",
                "## Stato",
                "",
                "```text",
                "Tipo:                 analisi esplorativa offline",
                "Sessione:             NQ US Cash 2026-08-04",
                "Schema incluso:       fof-session-observation-v2",
                "Schema escluso:       fof-session-observation-v1",
                "Modello attivo:       nessuno",
                "Segnali / ordini:     nessuno",
                "```",
                "",
                "## Metodo Fissato",
                "",
                "Il test usa solo eventi completi con volume tick coerente e metadati dello strumento. Li ordina per ultimo tick e seleziona il primo evento disponibile, poi il primo evento il cui ultimo tick non precede la fine dei 300 secondi dell'evento precedente selezionato. Le finestre future selezionate non si sovrappongono.",
                "",
                "```text",
                f"CSV input locale:          {input_csv.as_posix()}",
                f"SHA-256 CSV input:         {summary['inputCsvSha256']}",
                f"CSV selezionato locale:    {selected_csv.as_posix()}",
                f"SHA-256 CSV selezionato:   {summary['selectedCsvSha256']}",
                f"eventi input:              {summary['inputRows']}",
                f"eventi idonei:             {summary['eligibleRows']}",
                f"eventi non sovrapposti:    {summary['selectedRows']}",
                f"primo ultimo tick UTC:     {summary['firstSelectedLastTickUtc']}",
                f"ultimo ultimo tick UTC:    {summary['lastSelectedLastTickUtc']}",
                f"selezione rifiutata per sovrapposizione: {summary['rejections'].get('overlaps-prior-selected-response', 0)}",
                "```",
                "",
                "## Esito Del Test",
                "",
                f"La selezione ha lasciato `{summary['selectedRows']}` finestre non sovrapposte: `{location_counts['above-all-pocs']}` sopra tutti i POC, `{location_counts['at-poc']}` sul POC, `{location_counts['below-all-pocs']}` sotto tutti i POC e `{location_counts['between-tied-pocs']}` tra POC in parita'. Le categorie con almeno tre osservazioni sono: `{', '.join(describable_locations) if describable_locations else 'nessuna'}`. Se meno di due categorie sono descrivibili, il dataset non permette un confronto tra location. Qualunque prevalenza di segno in una categoria resta compatibile con il movimento generale della stessa apertura.",
                "",
                "## Risultati Descrittivi",
                "",
                "```text",
                *category_lines,
                "```",
                "",
                "La categoria e' `descrivibile` solo da tre eventi selezionati in su. In questo test la parola non significa statisticamente valida, predittiva o utilizzabile per un trade. Le osservazioni appartengono alla stessa apertura e possono condividere contesto e regime anche quando le finestre di risposta non si sovrappongono.",
                "",
                "## Conclusione",
                "",
                "Il test rimuove la sovrapposizione meccanica dei percorsi di cinque minuti, ma non sostituisce la replica su sessioni indipendenti. Non approva soglie, probabilita', filtro, segnale o modello. Il prossimo confronto ammesso richiede almeno cinque sessioni complete raccolte con lo stesso schema e un contesto d'asta precedente definito prima dell'analisi.",
                "",
            ]
        ),
        encoding="utf-8",
    )


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--events-csv", type=Path, required=True)
    parser.add_argument("--selected-csv", type=Path, required=True)
    parser.add_argument("--summary-json", type=Path, required=True)
    parser.add_argument("--report", type=Path, required=True)
    args = parser.parse_args()

    rows = read_rows(args.events_csv)
    selected, rejections = select_non_overlapping(rows)
    if not selected:
        raise ValueError("No event survived the fixed selection.")

    write_selected_csv(selected, args.selected_csv)
    summary: dict[str, object] = {
        "schema": "fof-session-observation-v2",
        "inputRows": len(rows),
        "eligibleRows": len(rows) - sum(value for key, value in rejections.items() if key != "overlaps-prior-selected-response"),
        "selectedRows": len(selected),
        "firstSelectedLastTickUtc": selected[0]["lastTickTimeUtc"],
        "lastSelectedLastTickUtc": selected[-1]["lastTickTimeUtc"],
        "inputCsvSha256": sha256(args.events_csv),
        "selectedCsvSha256": sha256(args.selected_csv),
        "rejections": rejections,
        "categories": category_summary(selected),
    }
    args.summary_json.write_text(json.dumps(summary, indent=2, sort_keys=True) + "\n", encoding="utf-8")
    write_report(args.events_csv, args.selected_csv, summary, args.report)


if __name__ == "__main__":
    main()
