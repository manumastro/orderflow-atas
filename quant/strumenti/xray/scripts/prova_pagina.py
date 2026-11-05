#!/usr/bin/env python3
"""La prova della pagina X-Ray nel browser (Playwright, Chromium senza finestra).

Apre ogni pagina come la apre l'utente (la pagina XrayWeb: Vite, React, Mantine, ECharts) e
controlla:
- nessun errore in console e nessun avviso rosso della pagina (i 404 delle rotte nuove che il
  server non ha ancora non contano: la pagina li mostra come "fonte non ancora disponibile");
- ogni grafico ha disegnato qualcosa (pixel non vuoti nel canvas);
- la heat map ha righe e la riga del prezzo;
- i filtri funzionano: un giorno passato, l'istante indietro con la freccia, i prezzi in ES
  (spostati della base), le scelte di ogni pagina (GEX/DEX/VEX, scadenze, viste);
- i gruppi (Lettura, Volatilita', NQ · QQQ, Fonti): con la rotta assente il riquadro
  "non ancora disponibile", con la rotta presente i grafici disegnati;
- gli indirizzi vecchi (#adesso?giorno=..., #/tempo) portano alla pagina nuova;
- lo zoom dei grafici resta quando le opzioni cambiano (D.3), e la modalita' passato si vede;
- i colori della mappa: freno verde, acceleratore rosso (5 novembre 2026);
- la larghezza di un telefono (390 px) senza scorrimento orizzontale;
e salva una foto per pagina, da guardare.

    python quant/strumenti/xray/scripts/prova_pagina.py                          # server su 8790
    python quant/strumenti/xray/scripts/prova_pagina.py --url http://127.0.0.1:4173 --foto DIR

Con --url si prova la pagina servita da `npm run preview` o `npm run dev` (XrayWeb), che manda
/api al server vero; le domande dirette all'API vanno sempre a --api (8790).
Esce con 1 se una prova fallisce. Le foto vanno in una cartella temporanea (non nel repo).
"""
from __future__ import annotations

import argparse
import json
import os
import re
import sys
import tempfile
import time
import urllib.error
import urllib.request

from playwright.sync_api import sync_playwright

PAGINE = ["adesso", "strike", "heatmap", "livelli", "deriva", "previsione"]

# quanti grafici si aspetta ogni pagina (canvas di ECharts con qualcosa disegnato)
GRAFICI = {"adesso": 1, "strike": 1, "heatmap": 0, "livelli": 1, "deriva": 4, "previsione": 1}

# i gruppi nuovi: la pagina, le rotte che usa, quanti grafici quando le rotte rispondono
NUOVE = {
    "volatilita": (["vol/serie", "vol/termine", "vol/sorriso?indice=SPX"], 6),
    "nq": (["nq/gex"], 1),
    "fonti": (["fonti"], 0),
    "lettura": (["lettura"], 0),
}
ROTTE_NUOVE = ("/api/fonti", "/api/vol/", "/api/nq/", "/api/lettura")

DISEGNATI = """
() => [...document.querySelectorAll('.grafico')].map((g) => {
  const c = g.querySelector('canvas');
  if (!c || !c.width) return 0;
  const x = c.getContext('2d');
  const d = x.getImageData(0, 0, c.width, c.height).data;
  let n = 0;
  for (let i = 3; i < d.length; i += 16) if (d[i] > 0) n++;
  return n;
})
"""


# i colori della mappa: quanti pixel sono freno (verde #4fae38) e quanti acceleratore (rosso #b5443f)
COLORI_MAPPA = """
() => {
  const c = document.querySelector('.grafico canvas');
  if (!c || !c.width) return [0, 0];
  const d = c.getContext('2d').getImageData(0, 0, c.width, c.height).data;
  let freno = 0, acc = 0;
  for (let i = 0; i < d.length; i += 4 * 7) {
    const r = d[i], g = d[i + 1], b = d[i + 2];
    if (g > r + 40 && g > b + 20) freno++;
    else if (r > g + 40 && r > b + 20) acc++;
  }
  return [freno, acc];
}
"""

# i compiti lunghi del thread principale, dall'apertura della pagina
LUNGHI = """
async (attesa) => {
  const lunghi = [];
  new PerformanceObserver((l) => { for (const e of l.getEntries()) lunghi.push(Math.round(e.duration)); })
    .observe({ type: 'longtask', buffered: true });
  await new Promise((r) => setTimeout(r, attesa));
  return lunghi;
}
"""

# la prima mappa deve essere disegnata entro questo tempo dall'apertura, e nessun compito del thread
# principale deve durare piu' di questo mentre la pagina si aggiorna dal vivo
MAPPA_ENTRO_S = 8.0
COMPITO_MASSIMO_MS = 1000


class Prova:
    def __init__(self) -> None:
        self.fallite: list[str] = []
        self.passate = 0

    def ok(self, condizione: bool, cosa: str) -> None:
        if condizione:
            self.passate += 1
            print(f"  ok    {cosa}")
        else:
            self.fallite.append(cosa)
            print(f"  NO    {cosa}")


def api(base: str, percorso: str) -> dict:
    with urllib.request.urlopen(f"{base}/api/{percorso}", timeout=30) as r:
        return json.load(r)


def esiste(base: str, percorso: str) -> bool:
    try:
        with urllib.request.urlopen(f"{base}/api/{percorso}", timeout=30):
            return True
    except urllib.error.HTTPError as e:
        if e.code == 404:
            return False
        raise


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    ap.add_argument("--url", default="http://127.0.0.1:8790", help="dove si apre la pagina")
    ap.add_argument("--api", default="http://127.0.0.1:8790", help="il server XrayLive, per i confronti")
    ap.add_argument("--foto", default=os.path.join(tempfile.gettempdir(), "xray-pagina-foto"))
    ap.add_argument("--larghezza", type=int, default=1600)
    a = ap.parse_args()
    os.makedirs(a.foto, exist_ok=True)
    p = Prova()

    giorni = api(a.api, "giorni")["giorni"]
    passato = next((g for g in giorni[1:] if api(a.api, f"istanti?giorno={g}")["istanti"]), None)
    # nel giorno passato si guarda un istante di cassa (le 12:00 di New York): spot vivo, 0DTE vivi
    mezzogiorno = None
    if passato:
        ist = api(a.api, f"istanti?giorno={passato}")
        meta = (ist["cassa"]["apertura"] + ist["cassa"]["chiusura"]) // 2
        mezzogiorno = max((t for t in ist["istanti"] if t <= meta), default=None)
    base = api(a.api, f"base?giorno={giorni[0]}").get("valore")
    print(f"pagina {a.url}, server {a.api}: {len(giorni)} giornate, ultima {giorni[0]}, prova anche {passato}; base {base}")

    with sync_playwright() as pw:
        browser = pw.chromium.launch()
        pagina = browser.new_page(viewport={"width": a.larghezza, "height": 1000}, device_scale_factor=1)
        errori: list[str] = []

        def console(m) -> None:  # noqa: ANN001
            if m.type != "error":
                return
            dove = (m.location or {}).get("url", "")
            # il 404 di una rotta nuova e' atteso finche' il server non la ha: la pagina lo mostra
            if "404" in m.text and any(r in dove for r in ROTTE_NUOVE):
                return
            errori.append(f"console {m.type}: {m.text} @ {dove}")

        pagina.on("console", console)
        pagina.on("pageerror", lambda e: errori.append(f"pageerror: {e}"))

        def pronta() -> None:
            # si aspetta che la pagina abbia finito di chiedere (la prima previsione di un giorno chiede secondi)
            pagina.wait_for_timeout(300)
            pagina.wait_for_selector("#pagina[data-carica='no']", timeout=90_000)
            pagina.wait_for_timeout(500)

        def apri(frammento: str) -> None:
            pagina.goto(f"{a.url}/#/{frammento}")
            pronta()

        def scegli(testo: str) -> None:
            """Un comando a segmenti (Mantine SegmentedControl): si clicca l'etichetta."""
            pagina.locator(".segmenti label").filter(has_text=re.compile(f"^{re.escape(testo)}$")).first.click()

        def avvisi() -> int:
            return pagina.locator(".avviso").count()

        def controlla(nome: str, foto: str, attesi: int | None = None) -> None:
            pronta()
            n = avvisi()
            p.ok(n == 0, f"{nome}: nessun avviso" + (f" ({pagina.locator('.avviso').first.inner_text()})" if n else ""))
            pixel = pagina.evaluate(DISEGNATI)
            attesi = GRAFICI.get(nome.split(" ")[0], 0) if attesi is None else attesi
            pieni = sum(1 for x in pixel if x > 200)
            p.ok(pieni >= attesi, f"{nome}: {pieni}/{len(pixel)} grafici disegnati (attesi almeno {attesi})")
            pagina.screenshot(path=os.path.join(a.foto, foto), full_page=True)

        print("\n== la testata")
        apri("adesso")
        gruppi = pagina.locator("nav.gruppi a").all_inner_texts()
        p.ok(gruppi == ["Lettura", "SPX · X-Ray", "Volatilità", "NQ · QQQ", "Fonti"], f"cinque gruppi, la Lettura per prima: {gruppi}")
        voci = pagina.locator("nav.voci a").all_inner_texts()
        p.ok(voci == ["Adesso", "Per strike", "Heat map", "Livelli nel tempo", "Deriva", "Previsione"], f"le sei pagine di X-Ray: {voci}")

        for g, etichetta in ((giorni[0], "ultimo giorno"), (passato, f"giorno passato {passato}")):
            if g is None:
                continue
            print(f"\n== {etichetta}")
            for nome in PAGINE:
                errori.clear()
                apri(f"{nome}?giorno={g}&ts={mezzogiorno}" if g != giorni[0] else nome)
                controlla(nome, f"{g}-{nome}.png")
                p.ok(not errori, f"{nome}: nessun errore in console" + (f": {errori[:2]}" if errori else ""))

        print("\n== gli altri gruppi")
        for nome, (rotte, grafici) in NUOVE.items():
            errori.clear()
            presenti = [r for r in rotte if esiste(a.api, r)]
            apri(nome)
            if presenti:
                controlla(nome, f"nuova-{nome}.png", grafici if len(presenti) == len(rotte) else 0)
            else:
                pronta()
                n = pagina.locator(".non-disponibile").count()
                p.ok(n >= 1, f"{nome}: rotte assenti ({', '.join(rotte)}), {n} riquadri 'non ancora disponibile'")
                p.ok(avvisi() == 0, f"{nome}: nessun avviso rosso per una rotta assente")
                pagina.screenshot(path=os.path.join(a.foto, f"nuova-{nome}.png"), full_page=True)
            p.ok(not errori, f"{nome}: nessun errore in console" + (f": {errori[:2]}" if errori else ""))
        apri("fonti")
        p.ok(pagina.locator(".fonte-gruppo").count() == 4, "fonti: una riga di testa per ciascuna delle quattro fonti")
        p.ok(pagina.get_by_text("gex__mm_0dte", exact=True).count() == 1, "fonti: le famiglie di X-Ray da /api/stato")

        print("\n== i filtri")
        errori.clear()
        apri(f"adesso?giorno={passato}&ts={mezzogiorno}")
        ora_prima = pagina.locator("#istante-testo").inner_text()
        pagina.locator("body").press("ArrowLeft")
        pronta()
        p.ok(pagina.locator("#istante-testo").inner_text() != ora_prima, "freccia a sinistra: l'istante torna indietro di un minuto")
        p.ok("ts=" in pagina.url and f"ts={mezzogiorno}" not in pagina.url, "l'istante scelto finisce nell'indirizzo")

        # i prezzi: senza parametro sono in ES (5 novembre 2026); SPX finisce nell'indirizzo, ES lo toglie
        num = lambda s: float(s.replace(".", "").replace(",", "."))  # noqa: E731
        es = pagina.locator(".prezzi input[value=ES]")
        if es.count() and es.is_enabled():
            base_g = api(a.api, f"base?giorno={passato}").get("valore")
            # il confronto si fa sullo stesso istante della pagina, chiesto all'API
            ts = pagina.url.split("ts=")[1].split("&")[0]
            spx = api(a.api, f"adesso?giorno={passato}&ts={ts}")["riferimento"]["prezzo"]
            in_es = pagina.locator(".tessera .valore").first.inner_text()
            p.ok("prezzi=" not in pagina.url, "ES e' il default: nessun prezzi= nell'indirizzo")
            p.ok(abs(num(in_es) - (spx + base_g)) < 0.011, f"prezzi in ES: {in_es} = SPX {spx} + base {base_g}")
            pagina.locator(".prezzi label", has_text="SPX").click()
            pronta()
            in_spx = pagina.locator(".tessera .valore").first.inner_text()
            p.ok(abs(num(in_spx) - spx) < 0.011, f"prezzi in SPX: {in_spx} = SPX {spx}")
            p.ok("prezzi=SPX" in pagina.url, "SPX finisce nell'indirizzo")
            pagina.locator(".prezzi label", has_text="ES").click()
            pronta()
            p.ok("prezzi=" not in pagina.url, "tornando a ES il parametro esce dall'indirizzo")
        else:
            p.ok(False, "il pulsante ES e' spento: manca la base (bridge?)")

        # la modalita' passato: il corpo lo dice e la chip si vede
        p.ok(pagina.evaluate("document.body.dataset.tempo") == "passato", "passato: body[data-tempo=passato]")
        p.ok(pagina.locator(".chip-passato").first.is_visible(), "passato: la chip 'Passato' si vede")

        pagina.goto(f"{a.url}/#adesso?giorno={passato}")
        pronta()
        p.ok(f"#/adesso?giorno={passato}" in pagina.url, f"l'indirizzo vecchio #adesso?giorno= diventa #/adesso: {pagina.url.split('#')[1]}")
        p.ok(pagina.locator("#giorno").input_value() == passato, "l'indirizzo vecchio riapre la stessa giornata")
        pagina.goto(f"{a.url}/#/tempo?giorno={passato}")
        pronta()
        p.ok(f"#/livelli?giorno={passato}" in pagina.url, f"#/tempo rimanda a #/livelli: {pagina.url.split('#')[1]}")

        apri("strike")
        for testo in ("DEX", "VEX", "GEX"):
            scegli(testo)
            controlla(f"strike {testo}", f"strike-{testo}.png")
        scegli("Call e put")
        controlla("strike call e put", "strike-callput.png")

        apri("heatmap")
        scegli("GEX")
        pronta()
        righe = pagina.locator(".heat tbody tr").count()
        p.ok(righe > 5, f"heat map: {righe} righe")
        p.ok(pagina.locator(".heat tr.prezzo").count() == 1, "heat map: una riga del prezzo")
        p.ok(pagina.locator(".heat td.nodo").count() >= 1, "heat map: almeno un nodo contornato")
        scegli("DEX")
        pronta()
        p.ok(pagina.locator(".heat thead th").count() == 6, "heat map DEX: strike + 4 gruppi + totale")
        pagina.screenshot(path=os.path.join(a.foto, "heatmap-dex.png"), full_page=True)
        scegli("GEX")
        pronta()

        apri("livelli")
        scegli("Gamma osservata")
        controlla("livelli Gamma osservata", "livelli-gamma.png")
        freno, acc = pagina.evaluate(COLORI_MAPPA)
        p.ok(freno + acc > 500, f"livelli: la mappa e' nei colori del segno (freno {freno}, acceleratore {acc} pixel)")
        # lo zoom resta quando le opzioni cambiano (D.3): prima ogni giro lo azzerava
        pagina.evaluate("() => document.querySelector('.grafico').__grafico.dispatchAction({ type: 'dataZoom', start: 40, end: 80 })")
        for testo in ("Charm", "GEX per strike"):
            scegli(testo)
            controlla(f"livelli {testo}", f"livelli-{testo.replace(' ', '-')}.png")
        zoom = pagina.evaluate("() => { const z = document.querySelector('.grafico').__grafico.getOption().dataZoom[0]; return [z.start, z.end]; }")
        p.ok(abs(zoom[0] - 40) < 1 and abs(zoom[1] - 80) < 1, f"livelli: lo zoom resta cambiando lo sfondo ({zoom})")
        scegli("Nessuno")
        controlla("livelli Nessuno", "livelli-nessuno.png")

        apri("previsione")
        freno, acc = pagina.evaluate(COLORI_MAPPA)
        p.ok(freno + acc > 500, f"previsione: la mappa e' nei colori del segno (freno {freno}, acceleratore {acc} pixel)")
        scegli("Prossima seduta")
        controlla("previsione prossima seduta", "previsione-prossima.png")
        scegli("Resto della seduta")
        pronta()

        p.ok(not errori, "filtri: nessun errore in console" + (f": {errori[:3]}" if errori else ""))

        # la larghezza di un telefono: niente scorrimento orizzontale della pagina
        pagina.set_viewport_size({"width": 390, "height": 900})
        for nome in ("lettura", *PAGINE, "volatilita", "nq", "fonti"):
            apri(nome)
            largo = pagina.evaluate("document.documentElement.scrollWidth")
            p.ok(largo <= 392, f"telefono, {nome}: larghezza della pagina {largo}px (schermo 390)")
            pagina.screenshot(path=os.path.join(a.foto, f"telefono-{nome}.png"), full_page=True)
        browser.close()

    print(f"\n{p.passate} prove passate, {len(p.fallite)} fallite. Foto in {a.foto}")
    for f in p.fallite:
        print(f"  FALLITA: {f}")
    return 1 if p.fallite else 0


if __name__ == "__main__":
    sys.exit(main())
