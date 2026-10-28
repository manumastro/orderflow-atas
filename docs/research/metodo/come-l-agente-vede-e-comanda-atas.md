# Come L'Agente Vede E Comanda ATAS X

Dal 10 ottobre 2026 l'agente puo' **leggere tutto quello che ATAS sa** e **vedere quello che
l'utente ha davanti**, tab per tab, senza toccare mouse e tastiera. Questo documento e' la mappa:
le quattro strade, cosa passa da ciascuna, cosa scrive e cosa no, e le trappole gia' pagate.

Stato: **strumento**. Non contiene regole operative. Gli endpoint uno per uno stanno nel
[contratto del bridge](contratto-data-bridge.md); le tab e i ruoli in
[`il-workspace-a-tab.md`](il-workspace-a-tab.md).

## Perche'

*"E' importante che tu possa controllarlo e leggerne tutti i dati"* (l'utente, 10 ottobre 2026).
Una lettura dal vivo vale quanto i numeri su cui poggia, e i numeri devono essere **quelli che
l'utente ha sul chart**, non una ricostruzione: due calcoli della stessa cosa divergono senza che
nessuno se ne accorga ([`contratto-data-bridge.md`](contratto-data-bridge.md), `/panel`).

## Le Quattro Strade

| strada | cosa da' | con cosa | quando ATAS e' |
|---|---|---|---|
| **1 i dati** | candele, footprint, tape, book, profili, rollover; e cio' che l'indicatore calcola e disegna: livelli, pannello, regime, muri, ruolo del chart | il **Fabio Data Bridge**, HTTP su `127.0.0.1`; client `bridge.py` | aperto |
| **2 l'interfaccia** | le tab, la tab selezionata, i view model, i tipi caricati, i disegni di ogni chart; seleziona una tab, cancella disegni | lo stesso bridge, `/ui` e `/disegni`: l'indicatore gira **dentro** il processo di ATAS | aperto |
| **3 la configurazione** | per ogni tab: contratto, timeframe, giorni caricati, indicatori con le impostazioni, profilo fisso, disegni | il file del workspace, `workspace_a_tab.py --mostra` | aperto per leggere, **chiuso per scrivere** |
| **4 lo sguardo** | la foto di ogni tab, come la vede l'utente | `foto_atas.py`, che usa `/ui` e ritaglia sulla finestra di ATAS | aperto |

La strada 1 e la 2 richiedono che il **Fabio Data Bridge** sia caricato su almeno un chart. Nel
workspace a tab e' su tutti e quattro.

## Voglio Sapere... Uso...

Gli id dei chart **cambiano a ogni deploy e a ogni riapertura**: si leggono ogni volta con
`bridge.py charts`. Chi passa `--chart NQZ6` riceve l'M1, il chart col ruolo seduta.

| voglio sapere | comando |
|---|---|
| quali chart ci sono, con id, timeframe e barre | `bridge.py charts` |
| cosa espone il bridge | `bridge.py health --chart <id>` (campo `endpoints`) |
| il contratto: tick, scadenza, valore del punto | `bridge.py instrument --chart <id>` |
| le candele, con delta, VWAP, POC e value area di ogni barra | `bridge.py candles --chart <id> --from ... --to ...` |
| la footprint, prezzo per prezzo | `bridge.py candles ... --levels` |
| il tape, o solo i big trades | `bridge.py cumulative --chart <id> --from ... --to ... --min-volume 60` |
| il book storico | `bridge.py depth --chart <id> --from ... --to ...` |
| il profilo nativo di ATAS (giorno, settimana, contratto) | `bridge.py profile --chart <id> --period LastDay` |
| le date di rollover | `bridge.py rollovers --chart <id del continuo NQ> --from ... --to ...` |
| il pannello, riga per riga come sul chart | `bridge.py panel --chart NQZ6` |
| regime, proxy della velocita', big trades | `bridge.py regime --chart NQZ6` |
| i livelli disegnati e le regole che li producono | `bridge.py rules --chart NQZ6` |
| i muri che l'indicatore trova da solo | `bridge.py muri --chart NQZ6` |
| nodi e vuoti di cinque giorni (spenti sul chart, calcolati) | `bridge.py nodi --chart NQZ6` |
| la scala dello strumento e le soglie in punti | `bridge.py scala --chart NQZ6` |
| **cosa disegna una tab**: composito 90G, sedute della settimana, zone | `bridge.py ruolo --chart <id>` |
| i disegni di un chart (rettangoli, profili, linee) | `bridge.py disegni --chart <id>` |
| quale tab e' aperta adesso | `bridge.py ui --chart <id> --vm SelectedTab` (proprieta' `Header`) |
| come e' configurata ogni tab nel workspace salvato | `workspace_a_tab.py --mostra "Fabio workspace"` |
| cosa vede l'utente | `foto_atas.py` (tutte le tab) o `foto_atas.py --tab 3` |

Il giro d'orizzonte (`CLAUDE.md`, obbligo 1) usa gia' la strada 1 e arriva da solo a ogni
messaggio: questa tabella serve per tutto il resto.

## Cosa Scrive, E Cosa Chiede Prima

Quasi tutto e' lettura. Le eccezioni, dalla piu' innocua alla piu' delicata:

| scrive | cosa cambia | si ripristina |
|---|---|---|
| `levels`, `rules`, `watch` | i livelli e le righe che **l'indicatore** disegna su quel chart | si', ridepositando |
| `ui --tab N` | la tab selezionata in ATAS | si': `foto_atas.py` rimette quella di prima |
| `ui --tab N --davanti`, `ui --ridai` | la finestra in primo piano sullo schermo dell'utente | si': `foto_atas.py` rida' il primo piano a chi l'aveva |
| `disegni --togli TIPO` | **cancella i disegni di quel tipo** dal chart | **no**: un disegno cancellato va ridisegnato a mano |
| `workspace_a_tab.py --ritocca` | il file del workspace | si', dal backup accanto, ad ATAS chiuso |
| `workspace_finestra_es.py --aspetta` | aggiunge al workspace la finestra ES, con le quattro tab di NQ | si', da `Fabio workspace prima della finestra ES.aws`, ad ATAS chiuso |
| `workspace_clona_finestra.py --aspetta` | copia una finestra su un altro strumento, tab e indicatori uguali (default: la finestra ES diventa anche NQ) | si', da `Fabio workspace prima della finestra NQ.aws`, ad ATAS chiuso |

**Come si cambia il workspace** (usato il 9, il 10 e il 14 ottobre, e il 25 ottobre 2026):

1. si lancia lo script con `--aspetta`: resta in attesa che ATAS si chiuda;
2. **l'utente chiude ATAS**: da fuori non si chiude, perche' `CloseMainWindow` lo lascia fermo su una
   conferma a schermo, e lo schermo non si fotografa ([le foto](#le-foto-solo-atas-e-lo-si-dice));
3. alla chiusura lo script salva la copia accanto, poi scrive il file;
4. si controlla con `workspace_a_tab.py --mostra "Fabio workspace"` (solo la prima finestra), e
   l'utente riapre ATAS: l'accesso al conto e' suo.

**Prima di `disegni --togli` su un tipo che l'utente ha disegnato a mano** (`RectangleObj`, linee)
**si chiede**. Il 10 ottobre e' stato usato solo su `CustomHistogramm`, i profili di ATAS che
coprivano meta' di ogni tab, e solo dopo averli mostrati in foto.

**Cambiare tab o portare ATAS davanti si dice all'utente prima**: per qualche secondo il suo
schermo non e' suo.

## Le Foto: Solo ATAS, E Lo Si Dice

**Regola di riservatezza, nata da un errore.** Il 9 ottobre 2026 una cattura dello schermo intero
in un ciclo ha ripreso quello che l'utente stava facendo fuori da ATAS; il primo giro di
`foto_atas.py`, con ATAS ridotto a icona, ha fotografato un'altra finestra. Le immagini sono state
cancellate. Da allora:

- **si fotografa solo la finestra di ATAS**, ritagliata sulle coordinate che ATAS stesso
  restituisce da `/ui`, mai lo schermo intero;
- **se in primo piano non c'e' ATAS, non si salva niente**: `foto_atas.py` controlla il processo
  della finestra in primo piano (`OFT.PlatformX.exe`) e che non sia a icona, ed esce con codice 3;
- **si avvisa l'utente prima**, e le foto vanno nella scratchpad di sessione, non nel repo;
- **niente catture in ciclo** o in background.

`foto_atas.py --attesa` regola i secondi fra il cambio di tab e la foto (default 3): il chart deve
avere il tempo di ridisegnarsi. Lo schermo e' a 2560x1600 con scala 150%: lo script si dichiara
DPI-aware, o fotograferebbe solo l'angolo in alto a sinistra.

## Perche' Da Dentro, E Non Da Fuori

Tre strade da fuori sono state provate il 9 ottobre 2026, e nessuna funziona:

- **clic e tasti simulati** (`mouse_event`, `SendInput`): non arrivano. ATAS gira con privilegi
  diversi dal terminale dell'agente, e Windows (UIPI) scarta l'input simulato verso un processo
  piu' privilegiato;
- **computer use**: nel Claude Code da terminale esiste solo su macOS. Claude Desktop su Windows
  ce l'ha, ma le piattaforme di trading ricevono il livello "solo vista";
- **portare ATAS davanti da fuori**: Windows nega il primo piano a chi non ha dato l'ultimo input.

**L'indicatore invece gira nel processo di ATAS**, sullo stesso albero di controlli Avalonia:
puo' leggere le tab e selezionarne una sul thread dell'interfaccia. Per il primo piano usa la
tecnica documentata dei limiti di `SetForegroundWindow`: un ALT premuto e rilasciato dal processo
stesso, poi `BringWindowToTop` e `SetForegroundWindow`. **Il primo tentativo a volte fallisce**:
`foto_atas.py` lo verifica e non salva.

Tutto per reflection, senza riferimenti di compilazione ad Avalonia: la versione e' quella che
ATAS carica, e cambia con gli aggiornamenti (`LInterfacciaDiAtas.cs`).

## Come Si Esplora Quello Che Non Si Conosce

La documentazione di ATAS non copre l'interfaccia ne' molti oggetti interni, e quella che c'e' non
sempre coincide con la build installata (`.claude/rules/indicatore-e-deploy.md`). Si ispeziona:

```bash
python FabioOrderFlow/tools/bridge.py ui --chart <id> --filtro Tab        # controlli che contengono "Tab"
python FabioOrderFlow/tools/bridge.py ui --chart <id> --vm ""             # il view model principale
python FabioOrderFlow/tools/bridge.py ui --chart <id> --vm Tabs.2         # la terza tab
python FabioOrderFlow/tools/bridge.py ui --chart <id> --descrivi MainTabViewModel
python FabioOrderFlow/tools/bridge.py disegni --chart <id>                # tutto cio' che espone l'oggetto dei disegni
```

Quello che si e' trovato, e che il codice usa:

| oggetto | cosa espone |
|---|---|
| `MainWindow.DataContext` | `MainViewModel`: `Tabs`, `SelectedTab`, `ScreenshotMaker` |
| una tab | `MainTabViewModel`: `Header` (il titolo), `ActivateCommand` (la seleziona), `Layout` |
| `ChartInfo.DrawingObjectsListInfo` | due campi privati: la `DrawingObjectsCollection` e lo `Instrument`; `GetAllObjects(Instrument)` da' tutti i disegni, `Delete(DrawingObject)` ne cancella uno, sul thread dell'interfaccia |
| la finestra | `Position` in pixel veri, `Bounds` in pixel logici da moltiplicare per `RenderScaling` |

`ScreenshotMaker` non e' stato esplorato: e' lo strumento di ATAS per le foto, e potrebbe un
giorno sostituire il ritaglio dello schermo.

## Il Workspace Su Disco

`%APPDATA%\ATAS X\Workspaces2\<nome>.aws`. Il workspace in uso e' **`Fabio workspace`**: le
quattro tab sono state applicate direttamente li', ad ATAS chiuso. Il backup di prima e'
`Fabio workspace prima delle tab.aws`.

E' un JSON con fine riga CRLF, e alcune parti sono **JSON dentro una stringa JSON**:

```text
Windows[0].Tabs[]                          una per tab; Title e' il nome della tab
  .Layout.Parameter.Item1.Parameter        il chart
     ContractId                            5222 = NQ continuo, 34387 = NQZ6
     TimeFrame.PeriodParameters            VisualName, TimeFrame ("daily", "m1", "m30") o Period (range)
     TimeFrame.CustomDaysCount             giorni caricati
     Template.FixedProfile.Visible         il profilo fisso del template
     Template.Panels[].SerializedIndicators[]   una STRINGA JSON per indicatore,
                                                e dentro, Settings: un'altra STRINGA JSON
     DrawingObjectses[]                    i disegni, con il tipo in "$type"
```

**ATAS tiene il workspace in memoria e lo risalva**: un file scritto con ATAS aperto viene
sovrascritto alla chiusura. Per questo `workspace_a_tab.py --ritocca` si rifiuta se ATAS e'
aperto, e con `--aspetta` resta in attesa che si chiuda. **Leggere si puo' sempre**, sapendo che
il file e' l'ultimo salvataggio e non lo schermo di adesso.

**I disegni non stanno solo nel file**: appartengono allo strumento, e ogni chart ne tiene una
copia. Toglierli dal file non basta; si tolgono con `disegni --togli` su ogni chart
([`il-workspace-a-tab.md`](il-workspace-a-tab.md)).

## Le Trappole Di Lettura

- **Il POC che ATAS scrive su una candela Daily e' delle 24 ore**, notte compresa; il POC di una
  seduta nella tab SETTIMANA e' della **sola cassa**. Il 10 ottobre l'utente ha letto sul Daily che
  l'8 ottobre aveva toccato il POC del 1 ottobre, e sulla settimana no: avevano ragione tutte e
  due. Il 1 ottobre: **POC 24 ore 30.820, POC di cassa 30.634**; minimo dell'8, **30.792**. Per il
  metodo conta la cassa `[1 · 1:14:20]`: l'8 si e' fermato sul VAH di cassa del 1 ottobre (30.780),
  non sul suo POC. Il composito di 90 giorni e' anch'esso su barre di 24 ore, e lo dichiara.
- **La candela Daily di ATAS parte alle 22:00Z del giorno prima** (misurato in ora legale: sono le
  18:00 di New York, quindi d'inverno ci si aspetta 23:00Z, da verificare il 2 novembre 2026): `time`
  2026-09-30T22:00Z e' la giornata del 1 ottobre.
- **Il continuo `NQ` non e' aggiustato al rollover**: prima dell'11 settembre 2026 i suoi prezzi
  stanno 299,25 punti sotto NQZ6. Dopo, coincidono. `ruolo` sul Daily da' le due versioni.
- **L'id del chart e l'indice `bar` cambiano a ogni ricarica**: si chiedono, non si ricordano.

## Cosa Non Si Puo' Ancora Fare, Dichiarato

- **Aprire menu e finestre di dialogo di ATAS**, per esempio le impostazioni di un indicatore. Le
  impostazioni si cambiano nel file, ad ATAS chiuso.
- **Salvare il workspace da dentro.** Il comando esiste da qualche parte nel view model, ma non e'
  stato cercato.
- **Leggere gli indicatori di ATAS** (BigTrades, delta cumulato, speed of tape) **dal vivo**: se ne
  leggono solo le impostazioni nel file. I loro numeri si ricavano dal tape (`cumulative`), e la
  speed of tape resta un proxy.

## Come Si Controlla Che Funzioni

```bash
python FabioOrderFlow/tools/bridge.py charts                                  # 4 chart
python FabioOrderFlow/tools/bridge.py health --chart <id>                     # 21 endpoint
python FabioOrderFlow/tools/bridge.py ui --chart <id> --vm SelectedTab        # la tab aperta
python FabioOrderFlow/tools/workspace_a_tab.py --mostra "Fabio workspace"     # 4 tab, profilo fisso spento
```

Il 10 ottobre 2026, dopo il deploy: 4 chart, 21 endpoint su `/health`, sull'M1 solo i 3
`RectangleObj` dell'utente, nel file le 4 tab con LVN e nodi spenti e big trades a 60.
