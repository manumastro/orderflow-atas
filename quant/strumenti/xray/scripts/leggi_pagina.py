#!/usr/bin/env python3
"""La pagina X-Ray letta come la vede l'utente (2 novembre 2026).

Richiesta dell'utente: "facciamo in modo che la lettura dell'agente sia in base a visivamente cio' che vede
l'utente". Questo script apre http://127.0.0.1:8790/ in Chromium senza finestra, pagina per pagina, e per
ogni scheda stampa cio' che c'e' sullo schermo:
- il titolo della scheda, cosi' la lettura puo' dire "NQ · QQQ > La GEX viva di NQ nel tempo";
- il testo visibile (tessere, tabelle, note), senza le spiegazioni "In parole semplici";
- la scelta attiva dei comandi della scheda (per esempio "Tutte le scadenze" o "Solo oggi (0DTE)");
- per ogni grafico, le serie e le linee **come sono disegnate** (l'istanza di ECharts, appesa al nodo da
  Grafico.tsx): l'ultimo punto e quello di un'ora prima per le serie nel tempo, gli strike piu' pesanti per
  i profili, le linee di riferimento (prezzo, zero gamma, muri) con la loro etichetta.

Non ricalcola niente: legge la pagina. Se un numero qui e nella pagina differiscono, il difetto e' qui.
L'ordine in cui si legge e cosa significa: quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md.

    python quant/strumenti/xray/scripts/leggi_pagina.py                       # le pagine della lettura
    python quant/strumenti/xray/scripts/leggi_pagina.py --pagine nq,volatilita
    python quant/strumenti/xray/scripts/leggi_pagina.py --pagine tutte         # le dieci pagine
    python quant/strumenti/xray/scripts/leggi_pagina.py --foto DIR            # e una foto per scheda
    python quant/strumenti/xray/scripts/leggi_pagina.py --json

Le foto vanno nella scratchpad, mai nel repo: si guardano con Read quando la forma conta (un profilo, un
incrocio nel tempo) e si cancellano.
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import os
import sys
from zoneinfo import ZoneInfo

from playwright.sync_api import sync_playwright

NY = ZoneInfo("America/New_York")

# le pagine della lettura dal vivo, nell'ordine di come-si-leggono-le-opzioni-dal-vivo.md
LETTURA = ["fonti", "volatilita", "nq", "adesso", "livelli"]
# le dieci pagine (5 novembre 2026); `tempo` resta come nome e la pagina lo rimanda a livelli
TUTTE = ["lettura", "adesso", "strike", "heatmap", "livelli", "deriva", "previsione", "volatilita", "nq", "fonti"]
RIMANDI = {"tempo": "livelli"}
TITOLI = {"lettura": "Lettura", "fonti": "Fonti", "volatilita": "Volatilita'", "nq": "NQ · QQQ",
          "adesso": "SPX · X-Ray > Adesso", "livelli": "SPX · X-Ray > Livelli nel tempo", "strike": "SPX · X-Ray > Per strike",
          "deriva": "SPX · X-Ray > Deriva", "previsione": "SPX · X-Ray > Previsione",
          "heatmap": "SPX · X-Ray > Heat map"}

# i riquadri che ogni pagina deve avere (i nomi stabili, progetto C.13): se ne manca uno, la lettura lo dice
# ed esce con 1, perche' una lettura che cita un riquadro sparito sbaglia senza accorgersene
RIQUADRI = {
    "lettura": ["La lettura", "I prezzi", "Da guardare", "Dubbi e parole"],
    "adesso": ["Freno e acceleratore, prezzo per prezzo", "La scala dei prezzi", "Chi ha comprato delta oggi"],
    "strike": ["Esposizione per strike", "Strike vicini al prezzo"],
    "heatmap": ["Esposizione per strike e per scadenza"],
    "livelli": ["Il prezzo e i livelli della giornata", "La scala dei livelli"],
    "deriva": ["Il prezzo e il regime", "GEX netta per scadenza", "Delta accumulato per gruppo", "Pressione 0DTE"],
    "previsione": ["La mappa prevista", "Livelli della prossima seduta"],
    "volatilita": ["Il meteo · Nasdaq 100", "Il meteo · S&P 500"],
    "nq": ["Profilo GEX per strike, in prezzi di NQ", "I livelli", "La GEX viva di NQ nel tempo"],
    "fonti": ["Lo stato di ogni fonte", "Il lettore di X-Ray e il server", "Le famiglie di X-Ray"],
}

# Nel browser: per ogni scheda il testo visibile e le opzioni dei grafici ridotte a cio' che si legge.
ESTRAI = r"""
() => {
  const pulito = (s) => (s || '').replace(/ /g, ' ').replace(/[ \t]+/g, ' ').replace(/\n\s*\n+/g, '\n').trim();
  const num = (v) => (typeof v === 'number' && isFinite(v) ? v : null);
  const punto = (d) => {
    const v = d && typeof d === 'object' && !Array.isArray(d) ? d.value : d;
    return Array.isArray(v) ? v : typeof v === 'number' ? [v] : null;
  };
  const grafico = (g) => {
    const ist = g.__grafico;
    const out = { etichetta: g.getAttribute('aria-label'), serie: [], linee: [] };
    if (!ist) return out;
    const o = ist.getOption();
    const assiX = o.xAxis || [], assiY = o.yAxis || [];
    for (const s of o.series || []) {
      for (const m of (s.markLine && s.markLine.data) || []) {
        const v = num(m.yAxis) ?? num(m.xAxis);
        const nome = m.name || (m.label && typeof m.label.formatter === 'string' ? m.label.formatter : '') || '';
        if (v != null && nome) out.linee.push({ nome, valore: v, asse: m.yAxis != null ? 'y' : 'x' });
      }
      const dati = (s.data || []).map(punto).filter((p) => p && p.length);
      if (!dati.length) continue;
      const ax = assiX[s.xAxisIndex || 0] || {}, ay = assiY[s.yAxisIndex || 0] || {};
      const cat = Array.isArray(ax.data) ? ax.data : Array.isArray(ay.data) ? ay.data : null;
      const r = { nome: s.name || '', tipo: s.type, punti: dati.length };
      if (s.type === 'custom' && dati[0].length >= 2) {
        // le barre per prezzo di viz/grafici.ts: [valore, prezzo, i]
        const pesanti = [...dati].sort((a, b) => Math.abs(b[0]) - Math.abs(a[0])).slice(0, 6);
        r.pesanti = pesanti.map((p) => ({ prezzo: p[1], valore: p[0] }));
      } else if (dati[0].length >= 2 && dati[0][0] > 1e12) {
        // una serie nel tempo: l'ultimo punto e quello di un'ora prima
        const validi = dati.filter((p) => num(p[1]) != null);
        if (validi.length) {
          const u = validi[validi.length - 1];
          const ora = validi.filter((p) => p[0] <= u[0] - 3600e3).pop();
          const ys = validi.map((p) => p[1]);
          r.ultimo = { ts: u[0], v: u[1] };
          if (ora) r.un_ora_prima = { ts: ora[0], v: ora[1] };
          r.min = Math.min(...ys); r.max = Math.max(...ys);
        }
      } else if (cat && dati[0].length === 1) {
        r.valori = dati.slice(0, 40).map((p, i) => [cat[i], p[0]]);
      } else if (dati[0].length >= 2) {
        r.valori = dati.length <= 40 ? dati.map((p) => [p[0], p[1]]) : null;
        if (dati.length > 40) { r.primo = dati[0].slice(0, 2); r.ultimo_punto = dati[dati.length - 1].slice(0, 2); }
      }
      out.serie.push(r);
    }
    return out;
  };
  const testata = pulito((document.querySelector('header') || {}).innerText);
  const schede = [...document.querySelectorAll('#pagina section.scheda')].map((s) => {
    // innerText rispetta l'impaginazione solo su nodi vivi: si nasconde cio' che non si legge, si legge, si rimette
    const via = [...s.querySelectorAll('.spiega, .sotto, .scheda-breve, .scheda-comandi, .grafico-scatola, .scheda-titolo')];
    const prima = via.map((x) => x.style.display);
    via.forEach((x) => { x.style.display = 'none'; });
    const testo = pulito(s.innerText);
    via.forEach((x, i) => { x.style.display = prima[i]; });
    const scelte = [...s.querySelectorAll('.scheda-comandi input:checked')].map((i) => {
      const l = s.querySelector(`label[for="${i.id}"]`);
      return pulito(l ? l.innerText : i.value);
    });
    return {
      titolo: s.getAttribute('aria-label'),
      tessera: s.classList.contains('tessera'),
      scelte,
      testo,
      grafici: [...s.querySelectorAll('.grafico')].map(grafico),
    };
  });
  return { testata, schede };
}
"""


def ora_ny(ms: float) -> str:
    return dt.datetime.fromtimestamp(ms / 1000, NY).strftime("%H:%M")


def n(v) -> str:
    if v is None:
        return "-"
    if isinstance(v, (int, float)):
        return f"{v:,.2f}".replace(",", "X").replace(".", ",").replace("X", ".").rstrip("0").rstrip(",")
    return str(v)


def stampa(pagina: str, d: dict) -> None:
    print(f"\n== {TITOLI.get(pagina, pagina)}   (http://127.0.0.1:8790/#/{pagina})")
    for s in d["schede"]:
        testa = f"-- {s['titolo']}" + (f"   [scelta: {', '.join(s['scelte'])}]" if s["scelte"] else "")
        print(testa)
        if s["testo"]:
            for riga in s["testo"].splitlines()[:30]:
                print(f"   {riga}")
        for g in s["grafici"]:
            if not g["serie"] and not g["linee"]:
                continue
            print(f"   grafico: {g['etichetta']}")
            if g["linee"]:
                # l'etichetta sul grafico porta gia' il numero quasi sempre ("zero gamma 29.012"): lo si ripete solo se manca
                print("     linee: " + "  ·  ".join(
                    l["nome"] if any(c.isdigit() for c in l["nome"])
                    else f"{l['nome']} {n(l['valore']) if l['asse'] == 'y' or l['valore'] < 1e12 else ora_ny(l['valore'])}"
                    for l in g["linee"]))
            for r in g["serie"]:
                if "ultimo" in r:
                    u, h = r["ultimo"], r.get("un_ora_prima")
                    print(f"     {r['nome']}: {n(u['v'])} alle {ora_ny(u['ts'])} NY"
                          + (f" (un'ora prima {n(h['v'])})" if h else "") + f"  min {n(r['min'])} max {n(r['max'])}")
                elif "pesanti" in r:
                    print(f"     {r['nome']}: piu' pesanti " + ", ".join(f"{n(p['prezzo'])} ({n(p['valore'])})" for p in r["pesanti"]))
                elif r.get("valori"):
                    print(f"     {r['nome']}: " + ", ".join(f"{n(a)}={n(b)}" for a, b in r["valori"][:20]))
                elif "primo" in r:
                    print(f"     {r['nome']}: {r['punti']} punti, da {n(r['primo'][0])}={n(r['primo'][1])} a "
                          f"{n(r['ultimo_punto'][0])}={n(r['ultimo_punto'][1])}")


def main() -> int:
    sys.stdout.reconfigure(encoding="utf-8")
    a = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    a.add_argument("--url", default="http://127.0.0.1:8790")
    a.add_argument("--pagine", default=",".join(LETTURA))
    a.add_argument("--foto", help="cartella per una foto per scheda (nella scratchpad)")
    a.add_argument("--json", action="store_true")
    a.add_argument("--attesa", type=int, default=60, help="secondi massimi per pagina")
    x = a.parse_args()
    pagine = TUTTE if x.pagine.strip() == "tutte" else [RIMANDI.get(p.strip(), p.strip()) for p in x.pagine.split(",") if p.strip()]
    tutto = {}
    problemi: list[str] = []
    with sync_playwright() as pw:
        b = pw.chromium.launch()
        ctx = b.new_context(viewport={"width": 1600, "height": 1000}, color_scheme="dark")
        pg = ctx.new_page()
        for p in pagine:
            pg.goto(f"{x.url}/#/{p}")
            try:
                pg.wait_for_selector("#pagina[data-carica='no']", timeout=x.attesa * 1000)
            except Exception:
                problemi.append(f"{p}: la pagina non ha finito di caricare in {x.attesa} s")
            pg.wait_for_timeout(800)
            d = pg.evaluate(ESTRAI)
            tutto[p] = d
            titoli = [s["titolo"] for s in d["schede"]]
            problemi += [f"{p}: manca il riquadro \"{t}\"" for t in RIQUADRI.get(p, []) if t not in titoli]
            problemi += [f"{p}: il riquadro \"{s['titolo']}\" e' vuoto" for s in d["schede"]
                         if not s["testo"] and not any(g["serie"] or g["linee"] for g in s["grafici"])]
            if x.foto:
                os.makedirs(x.foto, exist_ok=True)
                for i, s in enumerate(pg.query_selector_all("#pagina section.scheda:not(.tessera)")):
                    titolo = (s.get_attribute("aria-label") or f"scheda{i}")
                    nome = "".join(c if c.isalnum() else "-" for c in titolo.lower())[:50]
                    s.screenshot(path=os.path.join(x.foto, f"{p}-{i:02d}-{nome}.png"))
        b.close()
    for pr in problemi:
        print(f"! {pr}", file=sys.stderr)
    if x.json:
        print(json.dumps(tutto, ensure_ascii=False, indent=1))
        return 1 if problemi else 0
    adesso = dt.datetime.now(NY).strftime("%Y-%m-%d %H:%M")
    print(f"LA PAGINA X-RAY COME LA VEDE L'UTENTE — {adesso} New York ({x.url})")
    for p, d in tutto.items():
        stampa(p, d)
    return 1 if problemi else 0


if __name__ == "__main__":
    sys.exit(main())
