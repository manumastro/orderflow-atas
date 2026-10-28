#!/usr/bin/env python3
"""La pagina dello studio globale di un giorno: i cinque rapporti in un solo file HTML.

    python studio/pagina_del_giorno.py [AAAA-MM-GG]     # senza data: oggi, ora italiana

Scrive `studio/rapporti/<data>/dati/studio-<data>.html` (fuori da git, come tutto `dati/`): una pagina
autosufficiente, con i rapporti dentro, che si pubblica come artifact **solo quando l'utente lo
chiede**. Un artifact per giorno: l'indirizzo sta in `dati/artifact.txt`, e un aggiornamento in
giornata ripubblica sullo stesso indirizzo. Il sito locale (`quant/sito/sito.py`) i rapporti li legge da
solo e non ha bisogno di questo file.

Il codice impagina, non scrive: il testo e' quello dei rapporti, com'e'.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import re
import sys
from pathlib import Path
from zoneinfo import ZoneInfo

QUI = Path(__file__).resolve().parent
RAPPORTI = QUI / "rapporti"
FASI = [("quadro", "Il quadro", "5-quadro.md"), ("macro", "Macro", "1-macro.md"), ("cot", "COT", "2-cot.md"),
        ("gamma", "Gamma", "3-gamma.md"), ("orderflow", "Order flow", "4-orderflow.md")]
MESI = ["gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre",
        "ottobre", "novembre", "dicembre"]


def leggi(p: Path) -> str:
    try:
        return p.read_text(encoding="utf-8")
    except OSError:
        return ""


def campo(testo: str, etichetta: str) -> str:
    m = re.search(rf"^\*\*{etichetta}\*\*:?(.*?)(?:\n\s*\n|\n\*\*|\Z)", testo, re.M | re.S)
    return " ".join(m.group(1).split()) if m else ""


def dividi(testo: str) -> tuple[str, str]:
    """La parte per la persona (## In Parole Semplici) e il resto, il rapporto tecnico.

    La pagina mette la prima in cima e il resto sotto, chiuso (21 ottobre 2026: prima la persona)."""
    m = re.search(r"^## In Parole Semplici\s*\n(.*?)(?=^## |\Z)", testo, re.M | re.S)
    if not m:
        return "", testo
    return m.group(1).strip(), (testo[:m.start()] + testo[m.end():]).strip()


def pagina(data: str) -> Path:
    cartella = RAPPORTI / data
    if not cartella.is_dir():
        raise SystemExit(f"nessuno studio per il {data}: manca {cartella}")
    fasi = []
    for chiave, nome, file in FASI:
        t = leggi(cartella / file)
        semplice, tecnico = dividi(t)
        fasi.append({"chiave": chiave, "nome": nome, "file": file, "testo": t,
                     "semplice": semplice, "tecnico": tecnico,
                     "titolo": (re.search(r"^# (.+)$", t, re.M) or [None, nome])[1],
                     "stato": campo(t, "Stato").split("(")[0].split(".")[0].strip()})
    precedenti = []
    for d in sorted((cartella / "precedenti").glob("[0-9]" * 4), reverse=True):
        t = leggi(d / "5-quadro.md")
        if t:
            precedenti.append({"ora": f"{d.name[:2]}:{d.name[2:]}", "testo": t})
    raccolta = {}
    try:
        raccolta = json.loads(leggi(cartella / "dati" / "raccolta.json") or "{}")
    except ValueError:
        pass
    quadro = fasi[0]["testo"]
    g = dt.date.fromisoformat(data)
    dati = {
        "data": data,
        "giorno": f"{g.day} {MESI[g.month - 1]} {g.year}",
        "riga": campo(quadro, "In una riga"),
        "titolo_quadro": fasi[0]["titolo"],
        "raccolto": raccolta.get("_raccolto", ""),
        "generata": dt.datetime.now(ZoneInfo("Europe/Rome")).strftime("%H:%M"),
        "fasi": fasi,
        "precedenti": precedenti,
    }
    html = MODELLO.replace("__TITOLO__", f"Studio NQ, {g.day} {MESI[g.month - 1]}").replace(
        "__DATI__", json.dumps(dati, ensure_ascii=False).replace("</", "<\\/"))
    out = cartella / "dati" / f"studio-{data}.html"
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(html, encoding="utf-8")
    return out


MODELLO = r"""<title>__TITOLO__</title>
<meta name="claude:viewport-fit-sides" content="cover">
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=IBM+Plex+Mono:wght@400;500&family=IBM+Plex+Sans+Condensed:wght@500;600&family=IBM+Plex+Sans:ital,wght@0,400;0,600;1,400&display=swap">
<style>
/* una colonna di lettura; testata con la riga del giorno, schede delle fasi fisse in alto */
:root {
  --bg: #f3f4f7; --carta: #ffffff; --fg: #1a2030; --muto: #5b6476; --linea: #d8dce4;
  --accento: #2b55c4; --accento-tenue: #e4eafa; --attenzione: #9a5b00; --attenzione-tenue: #fbefdc;
  --display: "IBM Plex Sans Condensed", "Arial Narrow", system-ui, sans-serif;
  --testo: "IBM Plex Sans", system-ui, -apple-system, "Segoe UI", sans-serif;
  --mono: "IBM Plex Mono", ui-monospace, Consolas, monospace;
}
@media (prefers-color-scheme: dark) { :root:not([data-theme="light"]) {
  --bg: #11141b; --carta: #181c25; --fg: #e3e6ee; --muto: #9aa3b5; --linea: #2b313e;
  --accento: #86a5ff; --accento-tenue: #1f2a45; --attenzione: #f0b45a; --attenzione-tenue: #3a2a12; color-scheme: dark } }
:root[data-theme="dark"] {
  --bg: #11141b; --carta: #181c25; --fg: #e3e6ee; --muto: #9aa3b5; --linea: #2b313e;
  --accento: #86a5ff; --accento-tenue: #1f2a45; --attenzione: #f0b45a; --attenzione-tenue: #3a2a12; color-scheme: dark }
body { background: var(--bg); color: var(--fg); font: 15px/1.6 var(--testo);
  padding-left: max(16px, env(safe-area-inset-left, 0px)); padding-right: max(16px, env(safe-area-inset-right, 0px)); padding-block: 0 48px; }
.colonna { max-width: 860px; margin: 0 auto; display: flex; flex-direction: column; gap: 20px; }
header { padding-block: 32px 4px; display: flex; flex-direction: column; gap: 10px; }
.occhiello { font: 500 12px/1 var(--mono); letter-spacing: .08em; text-transform: uppercase; color: var(--muto); }
h1.giorno { font: 600 clamp(28px, 5vw, 40px)/1.1 var(--display); margin: 0; text-wrap: balance; }
.riga { font-size: 17px; line-height: 1.55; max-width: 68ch; margin: 0; }
.meta { display: flex; flex-wrap: wrap; gap: 6px 14px; font: 12px/1.4 var(--mono); color: var(--muto); }
nav.fasi { position: sticky; top: env(safe-area-inset-top, 0px); z-index: 2; background: var(--bg);
  display: flex; gap: 4px; overflow-x: auto; border-bottom: 1px solid var(--linea); padding-block: 8px 0; }
nav.fasi button { font: 600 14px/1 var(--display); letter-spacing: .02em; background: none; color: var(--muto);
  border: 0; border-bottom: 2px solid transparent; padding: 10px 12px 12px; cursor: pointer; white-space: nowrap;
  display: flex; flex-direction: column; align-items: flex-start; gap: 4px; }
nav.fasi button small { font: 400 11px/1 var(--mono); letter-spacing: 0; }
nav.fasi button[aria-selected="true"] { color: var(--fg); border-bottom-color: var(--accento); }
nav.fasi button:focus-visible, select:focus-visible { outline: 2px solid var(--accento); outline-offset: 2px; }
.versione { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; font-size: 13px; color: var(--muto); }
select { font: 13px var(--testo); color: var(--fg); background: var(--carta); border: 1px solid var(--linea); border-radius: 4px; padding: 4px 6px; }
.avviso { background: var(--attenzione-tenue); color: var(--attenzione); border-radius: 4px; padding: 8px 12px; font-size: 13px; }
article { background: var(--carta); border: 1px solid var(--linea); border-radius: 6px; padding: 8px 28px 28px; min-width: 0; }
article h1 { font: 600 24px/1.2 var(--display); margin: 20px 0 8px; text-wrap: balance; }
article h2 { font: 600 19px/1.25 var(--display); margin: 32px 0 8px; padding-top: 12px; border-top: 1px solid var(--linea); text-wrap: balance; }
article h3 { font: 600 16px/1.3 var(--display); margin: 22px 0 6px; }
article p, article li { max-width: 72ch; }
article ul, article ol { padding-left: 22px; display: flex; flex-direction: column; gap: 4px; }
article code { font: 13px var(--mono); background: var(--accento-tenue); padding: 1px 4px; border-radius: 3px; overflow-wrap: anywhere; }
article strong { font-weight: 600; }
article a { color: var(--accento); }
.tabella { overflow-x: auto; margin: 12px 0; }
table { border-collapse: collapse; font-size: 13.5px; line-height: 1.45; font-variant-numeric: tabular-nums; }
th, td { text-align: left; vertical-align: top; padding: 7px 10px; border-bottom: 1px solid var(--linea); }
th { font: 600 12px/1.3 var(--mono); letter-spacing: .04em; text-transform: uppercase; color: var(--muto); white-space: nowrap; }
td:first-child { white-space: nowrap; font-weight: 600; }
.vuota { color: var(--muto); padding: 24px 0; }
.semplice { background: var(--accento-tenue); border-radius: 6px; padding: 4px 20px 16px; margin: 16px 0 8px; font-size: 16px; line-height: 1.65; }
.semplice > h2:first-child { border-top: 0; padding-top: 0; margin-top: 14px; }
.semplice ol { gap: 10px; }
.semplice table { background: var(--carta); }
details.tecnico { margin-top: 20px; border-top: 1px solid var(--linea); }
details.tecnico > summary { cursor: pointer; padding: 14px 0 6px; font: 600 15px/1.3 var(--display); color: var(--muto); }
details.tecnico > summary:focus-visible { outline: 2px solid var(--accento); outline-offset: 2px; }
@media (max-width: 560px) { article { padding: 4px 16px 20px; } }
</style>

<div class="colonna">
  <header>
    <div class="occhiello">NQZ6 · studio globale · descrizione, non regola</div>
    <h1 class="giorno" id="giorno"></h1>
    <p class="riga" id="riga"></p>
    <div class="meta" id="meta"></div>
  </header>
  <nav class="fasi" id="fasi" role="tablist" aria-label="Le fasi dello studio"></nav>
  <div class="versione" id="versione" hidden>
    <label for="ora">Versione del quadro</label><select id="ora"></select>
  </div>
  <div class="avviso" id="avviso" hidden></div>
  <article id="rapporto"></article>
</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/marked/12.0.2/marked.min.js"></script>
<script>
const D = __DATI__;
const $ = s => document.querySelector(s);
const esc = s => String(s ?? "").replace(/[&<>"]/g, c => ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;" }[c]));
const inline = s => window.marked ? marked.parseInline(s || "") : esc(s);

$("#giorno").textContent = D.giorno;
$("#riga").innerHTML = inline(D.riga);
$("#meta").innerHTML = [D.titolo_quadro, D.raccolto && "raccolta " + D.raccolto, "pagina delle " + D.generata]
  .filter(Boolean).map(x => `<span>${inline(x)}</span>`).join("");

function rendi(md) {
  const div = document.createElement("div");
  div.innerHTML = window.marked ? marked.parse(md) : `<pre>${esc(md)}</pre>`;
  div.querySelectorAll("table").forEach(t => { const w = document.createElement("div"); w.className = "tabella"; t.replaceWith(w); w.appendChild(t); });
  // i link relativi portano a file del repository, che qui non ci sono: restano testo
  div.querySelectorAll("a[href]").forEach(a => { if (!/^https?:/i.test(a.getAttribute("href"))) a.replaceWith(...a.childNodes); else { a.target = "_blank"; a.rel = "noopener"; } });
  return div;
}

function mostra(chiave, ora) {
  const f = D.fasi.find(x => x.chiave === chiave) || D.fasi[0];
  document.querySelectorAll("nav.fasi button").forEach(b => b.setAttribute("aria-selected", b.dataset.k === f.chiave));
  const vecchio = f.chiave === "quadro" && ora ? D.precedenti.find(p => p.ora === ora) : null;
  $("#versione").hidden = !(f.chiave === "quadro" && D.precedenti.length);
  $("#avviso").hidden = !vecchio;
  if (vecchio) $("#avviso").textContent = `Questo e' il quadro sostituito alle ${vecchio.ora}: e' qui per confronto, il quadro valido e' l'ultimo.`;
  const testo = vecchio ? vecchio.testo : f.testo;
  if (!testo) {
    $("#rapporto").replaceChildren(Object.assign(document.createElement("p"), { className: "vuota", textContent: `${f.file} non c'e' per questo giorno.` }));
  } else if (!vecchio && f.semplice) {
    // prima la persona: la parte in parole semplici in cima, il rapporto tecnico sotto, chiuso
    // ogni "→ cosa vuol dire" va a capo sotto il suo fatto (lo schema a gradini, 21 ottobre 2026)
    const box = rendi("## In parole semplici\n\n" + f.semplice.replace(/\n(\s*)→/g, "  \n$1→")); box.className = "semplice";
    const det = document.createElement("details"); det.className = "tecnico";
    det.append(Object.assign(document.createElement("summary"), { textContent: "Il rapporto tecnico: numeri, fonti, convenzioni e lacune" }), rendi(f.tecnico));
    $("#rapporto").replaceChildren(box, det);
  } else {
    $("#rapporto").replaceChildren(rendi(testo));
  }
  try { history.replaceState(null, "", "#" + f.chiave); } catch (e) {}
}

$("#fasi").innerHTML = D.fasi.map(f => `<button type="button" role="tab" data-k="${f.chiave}">${esc(f.nome)}<small>${esc(f.stato || (f.testo ? "" : "manca"))}</small></button>`).join("");
document.querySelectorAll("nav.fasi button").forEach(b => b.addEventListener("click", () => { $("#ora").value = ""; mostra(b.dataset.k); window.scrollTo(0, 0); }));
$("#ora").innerHTML = `<option value="">l'ultimo</option>` + D.precedenti.map(p => `<option value="${p.ora}">sostituito alle ${p.ora}</option>`).join("");
$("#ora").addEventListener("change", e => mostra("quadro", e.target.value));
mostra(location.hash.slice(1) || "quadro");
</script>
"""


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("data", nargs="?", default=dt.datetime.now(ZoneInfo("Europe/Rome")).date().isoformat())
    print(pagina(ap.parse_args().data))
    return 0


if __name__ == "__main__":
    sys.exit(main())
