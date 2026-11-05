#!/usr/bin/env python3
"""Pubblica una lettura dal vivo nella pagina X-Ray, scheda "Lettura" (2 novembre 2026).

Richiesta dell'utente: "possiamo fare una pagina nel nostro x ray con la lettura che posso far partire quando
te lo dico io e aggiorneresti il contenuto". L'agente scrive la lettura in un file JSON (nella scratchpad),
questo script la controlla e la mette in quant/dati/letture/<giorno NY>/<HHMM>.json; il server la serve
(/api/lettura) e la pagina la mostra da sola entro 15 secondi.

La forma e' quella di quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md, "Come si scrive: una lettura
lineare". Lo script rifiuta una lettura che non la rispetta, o che da' un segnale (LONG, SHORT, ingresso,
stop, bersaglio: si fa il punto, non si danno segnali).

    python quant/strumenti/xray/scripts/scrivi_lettura.py <lettura.json>
    python quant/strumenti/xray/scripts/scrivi_lettura.py --esempio      # stampa lo scheletro da riempire

Il file d'ingresso:

    {
      "strumento": "NQZ6", "prezzo": 28745,
      "studi": "macro e COT delle 10:58 italiane; gamma, order flow e quadro non fatti",
      "inBreve": "una frase",
      "righe": [
        {"voce": "Cosa è cambiato", "testo": "...", "dove": null},
        {"voce": "Il prezzo", "testo": "...", "dove": {"pagina": "chart", "scheda": "pannello"}},
        {"voce": "La paura", "testo": "...", "dove": {"pagina": "volatilita", "scheda": "Il meteo · Nasdaq 100"},
         "etichetta": "Paura: VXN 27, stabile"},
        {"voce": "I dealer", "testo": "...", "dove": {"pagina": "nq", "scheda": "I livelli"}, "etichetta": "NQ: acceleratore"},
        {"voce": "ES", "testo": "...", "dove": {"pagina": "adesso", "scheda": "Regime"}, "etichetta": "ES: acceleratore"}
      ],
      "prezzi": [{"prezzo": "28.960-29.030", "cosa": "...", "seCiArriva": "...", "da": 28960, "a": 29030}],
      "daGuardare": ["...", "..."],
      "dubbi": "una riga",
      "parole": [{"parola": "zero gamma", "significa": "..."}]
    }

Dal 5 novembre 2026 la pagina Lettura mette i prezzi in scala, con il prezzo di adesso al suo posto:
- ogni prezzo porta `da` e `a` numerici (una zona da < a, un prezzo da = a). Se mancano, lo script li ricava
  dalla stringa all'italiana: "28.960-29.030" -> 28960 e 29030, "28.812" -> da = a = 28812;
- ogni riga puo' avere un'`etichetta` di al massimo 24 caratteri: diventa una tessera in cima alla pagina.
Le letture scritte prima restano leggibili: senza `da`/`a` la pagina mostra i prezzi come tabella di testo.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import os
import re
import sys
from zoneinfo import ZoneInfo

NY = ZoneInfo("America/New_York")
ROMA = ZoneInfo("Europe/Rome")
DIR = os.path.normpath(os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "..", "dati", "letture"))

VOCI = ["Cosa è cambiato", "Il prezzo", "La paura", "I dealer", "ES"]
# `tempo` resta: la pagina lo rimanda a livelli (Livelli nel tempo)
PAGINE = {"chart", "lettura", "fonti", "volatilita", "nq", "adesso", "strike", "heatmap", "tempo", "deriva",
          "livelli", "previsione"}
ETICHETTA_MASSIMA = 24
# un numero all'italiana: 28.960 o 28.960,25 o 7031,5
NUMERO = re.compile(r"\d{1,3}(?:\.\d{3})+(?:,\d+)?|\d+(?:,\d+)?")
# si fa il punto, non si danno segnali (come-si-risponde-dal-vivo.md, 16 ottobre 2026)
SEGNALI = re.compile(r"\b(LONG|SHORT|NIENTE|ingresso|entrare|entra|stop loss|stop|bersaglio|target|take profit)\b")

# l'esempio usa i nomi dei riquadri della pagina di oggi (come-si-leggono-le-opzioni-dal-vivo.md)
DOVE_ESEMPIO = {
    "Il prezzo": {"pagina": "chart", "scheda": "pannello"},
    "La paura": {"pagina": "volatilita", "scheda": "Il meteo · Nasdaq 100"},
    "I dealer": {"pagina": "nq", "scheda": "I livelli"},
    "ES": {"pagina": "adesso", "scheda": "Regime"},
}
ESEMPIO = {
    "strumento": "NQZ6", "prezzo": 0, "studi": "",
    "inBreve": "",
    "righe": [{"voce": v, "testo": "", "dove": DOVE_ESEMPIO.get(v), "etichetta": ""} for v in VOCI],
    "prezzi": [{"prezzo": "", "cosa": "", "seCiArriva": "", "da": None, "a": None}],
    "daGuardare": [""], "dubbi": "", "parole": [{"parola": "", "significa": ""}],
}


def numero(testo: str) -> float:
    """Un numero all'italiana: "28.960" -> 28960, "7.031,5" -> 7031.5."""
    return float(testo.replace(".", "").replace(",", "."))


def da_a(prezzo: str) -> tuple[float, float] | None:
    """Gli estremi di un prezzo scritto: "28.960-29.030" -> (28960, 29030); "28.812" -> (28812, 28812)."""
    trovati = [numero(m) for m in NUMERO.findall(prezzo or "")]
    if not trovati:
        return None
    return min(trovati[:2]), max(trovati[:2])


def completa(x: dict) -> None:
    """I prezzi numerici: quelli dati si tengono (in ordine), quelli mancanti si ricavano dalla stringa."""
    for p in x.get("prezzi") or []:
        da, a = p.get("da"), p.get("a")
        if isinstance(da, (int, float)) and isinstance(a, (int, float)):
            p["da"], p["a"] = float(min(da, a)), float(max(da, a))
        elif isinstance(da, (int, float)) or isinstance(a, (int, float)):
            v = float(da if isinstance(da, (int, float)) else a)
            p["da"] = p["a"] = v
        else:
            r = da_a(p.get("prezzo") or "")
            if r:
                p["da"], p["a"] = r
    for r in x.get("righe") or []:
        if r.get("etichetta") is not None and not str(r.get("etichetta")).strip():
            r.pop("etichetta")


def controlla(x: dict) -> list[str]:
    e = []
    for campo in ("inBreve", "righe", "prezzi", "daGuardare", "dubbi", "parole"):
        if not x.get(campo):
            e.append(f"manca {campo}")
    righe = x.get("righe") or []
    voci = [r.get("voce") for r in righe]
    if voci != VOCI:
        e.append(f"le righe devono essere, in ordine: {VOCI} (sono {voci})")
    for r in righe:
        if not (r.get("testo") or "").strip():
            e.append(f"riga {r.get('voce')}: testo vuoto")
        d = r.get("dove")
        if d is not None and (d.get("pagina") not in PAGINE or not d.get("scheda")):
            e.append(f"riga {r.get('voce')}: 'dove' vuole una pagina fra {sorted(PAGINE)} e una scheda")
    for r in righe:
        et = r.get("etichetta")
        if et is not None and (not isinstance(et, str) or len(et) > ETICHETTA_MASSIMA):
            e.append(f"riga {r.get('voce')}: l'etichetta e' una parola breve, al massimo {ETICHETTA_MASSIMA} caratteri ({et!r})")
    for p in x.get("prezzi") or []:
        if not all((p.get(k) or "").strip() for k in ("prezzo", "cosa", "seCiArriva")):
            e.append(f"prezzo {p.get('prezzo')!r}: servono prezzo, cosa e seCiArriva (obbligo 6)")
        if not (isinstance(p.get("da"), (int, float)) and isinstance(p.get("a"), (int, float))):
            e.append(f"prezzo {p.get('prezzo')!r}: servono da e a numerici, o un prezzo scritto con i numeri")
    if len(x.get("prezzi") or []) > 6:
        e.append("al massimo sei prezzi: gli altri stanno nella pagina")
    # una riga, un'idea: una frase lunga e' un elenco travestito
    for r in righe:
        if len(r.get("testo") or "") > 320:
            e.append(f"riga {r.get('voce')}: piu' di 320 caratteri, va accorciata")
    testo = json.dumps(x, ensure_ascii=False)
    s = SEGNALI.search(testo)
    if s:
        e.append(f"parola da segnale {s.group(0)!r}: si fa il punto, non si danno segnali")
    return e


def main() -> int:
    sys.stdout.reconfigure(encoding="utf-8")
    a = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    a.add_argument("file", nargs="?")
    a.add_argument("--esempio", action="store_true")
    a.add_argument("--dir", default=DIR)
    x = a.parse_args()
    if x.esempio:
        print(json.dumps(ESEMPIO, ensure_ascii=False, indent=1))
        return 0
    if not x.file:
        a.error("serve il file della lettura")
    with open(x.file, encoding="utf-8-sig") as f:
        lettura = json.load(f)
    completa(lettura)
    errori = controlla(lettura)
    if errori:
        print("LETTURA RIFIUTATA:")
        for e in errori:
            print(f"  - {e}")
        return 1
    adesso = dt.datetime.now(dt.timezone.utc)
    lettura["versione"] = 1
    lettura["quando"] = int(adesso.timestamp() * 1000)
    lettura["oraNY"] = adesso.astimezone(NY).strftime("%H:%M")
    lettura["oraItalia"] = adesso.astimezone(ROMA).strftime("%H:%M")
    giorno = adesso.astimezone(NY).strftime("%Y-%m-%d")
    cartella = os.path.join(x.dir, giorno)
    os.makedirs(cartella, exist_ok=True)
    path = os.path.join(cartella, adesso.astimezone(NY).strftime("%H%M") + ".json")
    with open(path, "w", encoding="utf-8", newline="\n") as f:
        json.dump(lettura, f, ensure_ascii=False, indent=1)
    print(f"lettura pubblicata: {path}")
    print("si vede in http://127.0.0.1:8790/#/lettura entro 15 secondi")
    return 0


if __name__ == "__main__":
    sys.exit(main())
