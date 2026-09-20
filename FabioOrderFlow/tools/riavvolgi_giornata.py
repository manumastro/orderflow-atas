#!/usr/bin/env python3
"""Riporta il diario della giornata indietro insieme al replay.

**Il problema, e non e' teorico.** In replay l'orologio di mercato torna indietro quando si
riavvolge; il file della giornata no. Cosi' il diario di un giro precedente resta li' a
raccontare minuti che nella sessione in corso non sono ancora successi, e chi legge — persona o
agente — li legge come contesto. La disciplina del replay e' **non sapere come finisce**: un
diario avanti la toglie in silenzio, perche' un blocco datato 13:31Z si legge esattamente come
uno datato 12:31Z.

Il 20 settembre e' successo due volte nella stessa ora. `giro_orizzonte.py` ha imparato a **non
stampare** la sezione avanti, ma nascondere non basta: il file resta sul disco e chiunque lo
apra lo legge lo stesso. Infatti e' stato riaperto e letto.

**Cosa fa.** Sposta i blocchi datati nel futuro del replay in fondo al file, sotto
`## Il futuro di un giro precedente`. **Non cancella niente**: quei blocchi sono il registro di
un giro che c'e' stato davvero, e servono al confronto di fine sessione.

**Cosa conta come blocco datato, ed e' una regola stretta apposta.** Il blocco deve *dichiarare
il proprio orario in apertura*:

    **13:03Z — la mensola cade.** 409 lotti, ...      <- datato: si sposta
    **13:12-13:29Z — il rimbalzo.** Settanta punti    <- range: vale l'estremo alto
    ### 15:31 Dove eravamo                            <- datato: si sposta
    LA NOTTE — finestra 22:00Z -> 13:11Z, 74.072      <- CITA un orario avanti: resta

L'ultimo caso e' il motivo della regola stretta. Il quadro del 14 settembre e' misurato su una
finestra che finisce alle 13:11Z: spostarlo perche' contiene "13:11" svuoterebbe la sezione 4bis
del giro d'orizzonte, e una lettura con la 4bis vuota e' una lettura costruita sulle ultime
barre. Quei blocchi restano dove sono e vengono **segnalati**, perche' una misura presa su una
finestra che finisce nel futuro del replay e' un problema vero — ma e' un problema di finestra,
non di diario, e si corregge rifacendo la misura.

**Non torna avanti da solo.** Se il replay risupera un blocco parcheggiato, il blocco resta nel
parcheggio e il programma lo dice. Rimetterlo dentro significherebbe spacciare il testo di un
altro giro per la cronaca di questo. Si fa a mano, con `--ripristina`, quando lo si vuole.

    python3 FabioOrderFlow/tools/riavvolgi_giornata.py
    python3 FabioOrderFlow/tools/riavvolgi_giornata.py --prova
    python3 FabioOrderFlow/tools/riavvolgi_giornata.py --file docs/.../NQZ6-2026-09-14-replay.md
"""
from __future__ import annotations

import argparse
import datetime as dt
import json
import re
import subprocess
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
import ora  # noqa: E402

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

HERE = Path(__file__).resolve().parent
BRIDGE = HERE / "bridge.py"
GIORNATE = HERE.parent.parent / "docs" / "research" / "giornate"

TITOLO_PARCHEGGIO = "## Il futuro di un giro precedente"
#: Quanto avanti si guarda prima di concludere che un orario nudo appartiene al giorno prima.
#: Una seduta copre quasi ventiquattro ore: oltre questo scarto, "avanti" e' quasi sempre
#: "ieri sera" letto male, e una segnalazione falsa costa piu' di una mancata.
ORIZZONTE = dt.timedelta(hours=6)

APRE = "<!-- PARCHEGGIO -->"
CHIUDE = "<!-- /PARCHEGGIO -->"

#: Un orario in apertura di blocco: `13:03Z`, `15:31`, `13:12-13:29Z`. Il prefisso ammette la
#: punteggiatura con cui il diario apre un blocco datato — grassetto, trattino, cancelletti.
APERTURA = re.compile(
    r"^[\s>#*\-|]*"                      # la punteggiatura di apertura, qualunque sia
    r"(\d{1,2}:\d{2})"                   # l'orario, o il primo di un range
    r"(?:\s*[-–]\s*(\d{1,2}:\d{2}))?"    # l'estremo alto del range, se c'e'
    r"(Z?)"                              # Z: l'orario e' UTC, altrimenti e' italiano
)


def chiedi(*args) -> dict | None:
    try:
        out = subprocess.run([sys.executable, str(BRIDGE), *args],
                             capture_output=True, text=True, timeout=30)
        return json.loads(out.stdout) if out.returncode == 0 else None
    except Exception:
        return None


def ora_del_blocco(prima_riga: str, adesso_utc: str) -> dt.datetime | None:
    """Il momento che il blocco dichiara in apertura, sul giorno di mercato. None se non ne ha.

    Di un range vale **l'estremo alto**: un blocco che copre 12:36-13:02 descrive anche i due
    minuti dopo l'orologio, e quei due minuti non sono ancora successi.
    """
    m = APERTURA.match(prima_riga)
    if not m:
        return None
    adesso = ora.italiana(adesso_utc)
    candidati = [x for x in (m.group(1), m.group(2)) if x]
    momenti = []
    for grezzo in candidati:
        hh, mm = (int(x) for x in grezzo.split(":"))
        if not (0 <= hh <= 23 and 0 <= mm <= 59):
            return None
        if m.group(3):  # Z: l'orario e' UTC e va portato sul fuso di lettura
            quando = ora.italiana(adesso.astimezone(dt.timezone.utc).replace(
                hour=hh, minute=mm, second=0, microsecond=0))
        else:
            quando = adesso.replace(hour=hh, minute=mm, second=0, microsecond=0)
        momenti.append(quando)
    return max(momenti) if momenti else None


def cita_futuro(testo: str, adesso_utc: str) -> str | None:
    """L'orario piu' avanti *citato* nel corpo del blocco, se supera l'orologio.

    Non e' un motivo per spostare niente: e' un motivo per guardare. Una misura presa su una
    finestra che finisce nel futuro del replay non e' un pezzo di diario di troppo, e' un
    numero sbagliato, e si corregge rifacendo la misura — non spostando il paragrafo.

    **Si guarda solo poche ore avanti, ed e' necessario.** Un orario nudo non porta il giorno:
    `22:00Z` in un file del 14 settembre e' l'apertura di domenica sera, cioe' quindici ore
    *indietro*, ma appoggiato sul giorno di mercato diventa nove ore *avanti*. Senza questo
    limite ogni finestra notturna verrebbe segnalata come futuro, e dodici segnalazioni false
    seppelliscono l'unica vera.
    """
    adesso = ora.italiana(adesso_utc)
    peggio = None
    coppie = [(x, True) for x in re.findall(r"\b(\d{1,2}:\d{2})Z", testo)]
    coppie += [(x, False) for x in re.findall(r"\b(\d{1,2}:\d{2})(?![Z\d])", testo)]
    for grezzo, in_utc in coppie:
        hh, mm = (int(x) for x in grezzo.split(":"))
        if not (0 <= hh <= 23 and 0 <= mm <= 59):
            continue
        if in_utc:
            quando = ora.italiana(adesso.astimezone(dt.timezone.utc).replace(hour=hh, minute=mm))
        else:
            quando = adesso.replace(hour=hh, minute=mm)
        avanti = quando - adesso
        if dt.timedelta(0) < avanti < ORIZZONTE and (peggio is None or quando > peggio):
            peggio = quando
    return peggio.strftime("%H:%M") if peggio else None


def spezza(testo: str) -> list[str]:
    """Il file in blocchi separati da riga vuota, senza mai spezzare un blocco di codice.

    Una ``` a meta' di un blocco spostato lascerebbe il file con una cornice aperta e tutto
    quello che segue dentro un riquadro: il danno non e' il testo spostato, e' il resto.
    """
    blocchi: list[str] = []
    corrente: list[str] = []
    dentro_codice = False
    for riga in testo.splitlines():
        if riga.lstrip().startswith("```"):
            dentro_codice = not dentro_codice
        if not riga.strip() and not dentro_codice:
            if corrente:
                blocchi.append("\n".join(corrente))
                corrente = []
            continue
        corrente.append(riga)
    if corrente:
        blocchi.append("\n".join(corrente))
    return blocchi


def sezione_di(blocco: str, corrente: str) -> str:
    """Il titolo `##` sotto cui sta il blocco, per non perdere la provenienza."""
    for riga in blocco.splitlines():
        if riga.startswith("## "):
            return riga[3:].strip()
    return corrente


def dividi_parcheggio(testo: str) -> tuple[str, str]:
    """(il documento, il contenuto gia' parcheggiato). Il parcheggio non si riscandaglia."""
    i = testo.find(TITOLO_PARCHEGGIO)
    if i < 0:
        return testo, ""
    return testo[:i].rstrip() + "\n", testo[i:]


def main() -> int:
    p = argparse.ArgumentParser(description=__doc__,
                                formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--file", help="il file della giornata; senza, si deduce dal bridge")
    p.add_argument("--chart", help="id o strumento, se ATAS ne ha piu' di uno")
    p.add_argument("--prova", action="store_true", help="dice cosa sposterebbe, senza toccare")
    p.add_argument("--ripristina", action="store_true",
                   help="rimette nel documento i blocchi parcheggiati che il replay ha risuperato")
    p.add_argument("--tolleranza", type=int, default=0, metavar="MINUTI",
                   help="minuti di scarto tollerati prima di considerare un blocco avanti "
                        "(default 0: un blocco datato all'ora esatta dell'orologio non e' avanti)")
    a = p.parse_args()

    salute = chiedi("health", *(["--chart", a.chart] if a.chart else []))
    if not salute:
        print("[riavvolgi] bridge non raggiungibile: non si sa a che minuto sia il replay,")
        print("            e senza quello non si puo' dire cosa sia futuro. Niente toccato.")
        return 0
    adesso_utc = salute["marketTimeUtc"]
    data_mercato = adesso_utc[:10]

    if a.file:
        percorso = Path(a.file)
    else:
        strumento = salute["instrument"]
        candidati = [f"{strumento}-{data_mercato}-replay", f"{strumento}-{data_mercato}",
                     data_mercato]
        trovato = next((k for k in candidati if (GIORNATE / f"{k}.md").exists()), None)
        if not trovato:
            print(f"[riavvolgi] nessun file di giornata per {strumento} {data_mercato}: "
                  "niente da riavvolgere")
            return 0
        percorso = GIORNATE / f"{trovato}.md"

    if not percorso.exists():
        print(f"[riavvolgi] {percorso} non esiste")
        return 1

    testo = percorso.read_text(encoding="utf-8")
    documento, parcheggio = dividi_parcheggio(testo)
    limite = ora.italiana(adesso_utc) + dt.timedelta(minutes=a.tolleranza)

    if a.ripristina:
        return ripristina(percorso, documento, parcheggio, adesso_utc, limite, a.prova)

    tenuti: list[str] = []
    spostati: list[tuple[str, str]] = []   # (sezione di provenienza, blocco)
    segnalati: list[tuple[str, str]] = []  # (orario citato, prima riga)
    sezione = "(prima di ogni titolo)"
    for blocco in spezza(documento):
        sezione = sezione_di(blocco, sezione)
        quando = ora_del_blocco(blocco.splitlines()[0], adesso_utc)
        if quando and quando > limite:
            spostati.append((sezione, blocco))
            continue
        tenuti.append(blocco)
        citato = cita_futuro(blocco, adesso_utc)
        if citato:
            segnalati.append((citato, blocco.splitlines()[0][:72]))

    orologio = f"{ora.hhmm(adesso_utc)} ({adesso_utc[11:16]}Z)"
    if not spostati:
        print(f"[riavvolgi] {percorso.name}: nessun blocco avanti all'orologio del replay "
              f"({orologio}).")
    else:
        print(f"[riavvolgi] {percorso.name}: il replay e' a {orologio} e il diario e' avanti.")
        for sez, blocco in spostati:
            print(f"  parcheggio  [{sez}]  {blocco.splitlines()[0][:66]}")

    if segnalati:
        print("  ATTENZIONE: questi blocchi restano, ma CITANO un orario avanti all'orologio.")
        print("  Non sono diario di troppo: sono misure prese su una finestra che finisce nel")
        print("  futuro del replay, e si correggono rifacendo la misura, non spostando il testo.")
        for citato, riga in segnalati:
            print(f"    {citato}  {riga}")

    if parcheggio:
        risuperati = sum(1 for b in spezza(parcheggio)
                         if (q := ora_del_blocco(b.splitlines()[0], adesso_utc)) and q <= limite)
        if risuperati:
            print(f"  nel parcheggio ci sono {risuperati} blocchi che il replay ha ormai "
                  "risuperato.")
            print("  Non li rimetto dentro da solo: sono il registro di un altro giro, non la")
            print("  cronaca di questo. Con --ripristina, se li vuoi.")

    if not spostati:
        return 0
    if a.prova:
        print("  --prova: niente scritto")
        return 0

    quando_locale = dt.datetime.now().strftime("%Y-%m-%d %H:%M")
    # `tenuti` E' GIA' IL DOCUMENTO, blocco per blocco. Rimetterci dentro anche `documento`
    # intero lo scriveva due volte: il file usciva con due copie di tutte le sezioni, e la
    # seconda conteneva ancora i blocchi appena spostati. Visto al primo giro vero.
    nuovo: list[str] = []
    if not parcheggio:
        nuovo += [
            TITOLO_PARCHEGGIO, "", APRE, "",
            "**Questi blocchi raccontano minuti che nella sessione in corso non sono ancora**",
            "**successi.** Il replay e' stato riavvolto e il diario e' rimasto avanti: li ha",
            f"spostati qui `riavvolgi_giornata.py`. Non sono cancellati — sono il registro di un",
            "giro che c'e' stato davvero, e servono al confronto di fine sessione. Non tornano",
            "su da soli: rimetterli dentro spaccerebbe il testo di un altro giro per la cronaca",
            "di questo.", "",
        ]
    else:
        nuovo.append(parcheggio.replace(CHIUDE, "").rstrip())
        nuovo.append("")
    nuovo += [f"### Parcheggiati il {quando_locale} locali, replay a {orologio}", ""]
    for sez, blocco in spostati:
        nuovo += [f"*da «{sez}»*", "", blocco, ""]
    nuovo += [CHIUDE, ""]

    percorso.write_text("\n".join(tenuti_a_testo(tenuti, nuovo)), encoding="utf-8")
    print(f"  spostati {len(spostati)} blocchi in «Il futuro di un giro precedente»")
    # UNA SEZIONE SVUOTATA E' PEGGIO DI UNA SEZIONE AVANTI. Quello che resta sotto il titolo
    # sono le righe non datate — «i livelli sul chart sono dodici» — che descrivevano il
    # momento portato via, e adesso descrivono un momento che non c'e' piu'. Il programma non
    # puo' riscriverle: non sa cos'e' vero adesso. Puo' pero' dire quali guardare.
    ferite = sorted({sez for sez, _ in spostati})
    print(f"  da riscrivere, hanno perso il loro momento: {', '.join(f'«{x}»' for x in ferite)}")
    return 0


def tenuti_a_testo(tenuti: list[str], coda: list[str]) -> list[str]:
    """Il documento ricomposto: un blocco, una riga vuota, poi il parcheggio in fondo."""
    righe: list[str] = []
    for blocco in tenuti:
        righe.append(blocco)
        righe.append("")
    return righe + coda


def ripristina(percorso: Path, documento: str, parcheggio: str, adesso_utc: str,
               limite: dt.datetime, prova: bool) -> int:
    """Rimette nel documento i blocchi che il replay ha risuperato. Solo su richiesta.

    Si accodano alla cronaca nell'ordine in cui erano, con la loro provenienza: non si
    reinseriscono al punto esatto da cui erano usciti, perche' nel frattempo il giro in corso
    puo' avere scritto altro li' in mezzo, e un innesto silenzioso e' peggio di una coda
    dichiarata.
    """
    if not parcheggio:
        print("[riavvolgi] niente parcheggio in questo file")
        return 0
    torna, restano = [], []
    for blocco in spezza(parcheggio):
        if blocco.startswith(("##", "###", APRE, CHIUDE)) or blocco.startswith("*da «"):
            restano.append(blocco)
            continue
        quando = ora_del_blocco(blocco.splitlines()[0], adesso_utc)
        (torna if quando and quando <= limite else restano).append(blocco)
    if not torna:
        print("[riavvolgi] nessun blocco parcheggiato e' ancora nel passato del replay")
        return 0
    for blocco in torna:
        print(f"  ripristino  {blocco.splitlines()[0][:66]}")
    if prova:
        print("  --prova: niente scritto")
        return 0
    corpo = documento.rstrip() + "\n\n"
    corpo += "### Rientrati dal parcheggio, " + dt.datetime.now().strftime("%Y-%m-%d %H:%M") + "\n\n"
    corpo += "\n\n".join(torna) + "\n\n"
    corpo += "\n\n".join(restano).rstrip() + "\n"
    percorso.write_text(corpo, encoding="utf-8")
    print(f"  rientrati {len(torna)} blocchi")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
