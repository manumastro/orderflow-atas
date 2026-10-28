# Il Sistema Di Studio

**15 ottobre 2026.** Due studi fatti da agenti, uno per fase, ciascuno con la sua skill: lo **studio
globale** di NQ (macro, COT, gamma, order flow, quadro) e lo **studio dei paper** (cerca, scarica,
leggi, verifica, sintesi). Il metodo agentico che li regge, e che vale per ogni sistema nuovo di
questo tipo, sta in [`la-metodologia-agentica.md`](la-metodologia-agentica.md).

## Lo Studio Globale

**Cosa risponde**: in che contesto si muove NQ oggi (o ES, con `--strumento ESZ6`), dal piu' lontano
al piu' vicino. Come il giro
d'orizzonte, ma scritto, ragionato e con le fonti fuori da ATAS: il giro mostra i numeri, lo studio li
legge e li mette in fila.

```text
/studio-globale [AAAA-MM-GG] [rifai:<fasi>]     o a parole: "tieni macro e COT, rifai gamma e order flow"

1 raccolta   studio/raccogli.py [--strumento ESZ6]: macro, COT, quattro tab, sedute, ore, pannello, GEX   ~1-3 min
2 analisi    in parallelo, solo lettura:
               studio-macro      1-macro.md        tassi, volatilita', calendario, notizie
               studio-cot        2-cot.md          la procedura COT del live di Fabio
               studio-gamma      3-gamma.md        la GEX nelle due convenzioni, gli strike pesanti
               studio-orderflow  4-orderflow.md    le quattro tab con il profile framing
3 quadro     studio-quadro      5-quadro.md + il blocco QUADRO della giornata
```

- **Dove**: `studio/rapporti/<data>/`, un rapporto per fase con le stesse sezioni
  ([il modello](../.claude/skills/studio-globale/modello-del-rapporto.md)); i dati in `dati/`, fuori
  da git, con `raccolta.json` che dice fonte per fonte se c'e' e quanto ci ha messo.
- **Veloce: sotto i dieci minuti** (richiesta del 15 ottobre 2026). **Ogni calcolo si fa una volta**: la GEX e' una sola per giro e studio (si riusa sotto i 30 minuti, dichiarandone l'eta', altrimenti si calcola una volta), il giro si fa una volta dopo la GEX, le confluenze le calcola il codice e gli agenti non rifanno conti: cosi' la raccolta sta sotto il minuto. Rapporti di 120 righe, quadro di 150, `maxTurns: 20`. I tempi per passo sono nell'orchestratore, sezione "La velocita' e' un requisito".
- **Lo strumento e' un parametro, non un'eccezione** (27 ottobre 2026: "adatta il collettore per essere
  nativo in NQ e ES"). `raccogli.py --strumento NQZ6|ESZ6` (difetto NQZ6), cartella `<data>/` o
  `<data>-ES/`. Lo seguono:
  - **le scale** (`SCALE`): fasce del profilo 1 o 0,25, nodi del composito, soglia delle confluenze 15 o
    4 punti, 20 o 50 $ a punto;
  - **l'order flow**: `sedute-/ore-/range-<strumento>.json`, le quattro tab e il giro sui chart dello
    strumento, quindi anche la GEX della tab 2; il giro cerca il QUADRO solo nel file della giornata dello
    strumento (`ESZ6-<data>.md`: un file senza prefisso e' di NQ);
  - **la GEX**: `gex-<strumento>.json`, con il `moltiplicatore` scritto nel JSON;
  - **il COT** (`cot.raccogli(strumento)`): l'indice principale (NASDAQ-100 mini 209742 o S&P 500 e-mini
    13874A) per il cross-confirm, il prezzo alle date, il ritardo in prezzo e il profilo delle settimane
    dei report, cercati nella cache e nel chart M1 dello strumento; la TFF di tutti e due (`tff_nasdaq`,
    `tff_sp500`). I livelli del COT entrano nelle confluenze solo se `cot.json` e' dello stesso strumento;
  - **la macro** (`macro.raccogli(strumento)`): per ES in piu' gli undici ETF settoriali dell'S&P, KRE, IWM,
    SPY, l'ampiezza per settori (`ampiezza_sp500`: RSP meno SPY, settori che salgono) e le trimestrali
    dei pesi massimi non tech. L'S&P per titolo non c'e' fra le fonti gratuite gia' usate.
- **Il codice misura, gli agenti leggono.** Le quattro fasi d'analisi non hanno Bash: non scaricano e
  non ricalcolano. Una misura che manca e' una **lacuna del collettore**, e si aggiunge a `raccogli.py`.
- **Il confronto fra le fasi lo fa solo il quadro**: ogni fase lascia i suoi agganci (**Per il
  quadro**), e la fase 5 li verifica uno per uno. Il quadro tiene solo i livelli che almeno due fasi
  indicano entro 15 punti.
- **Il quadro arriva al giro**: la fase 5 scrive il blocco `<!-- QUADRO -->` del file della giornata
  (`docs/research/giornate/NQZ6-<data>.md`), che il giro stampa alla sezione 4bis.
- **Cosa rifare lo si chiede ogni volta.** Prima di raccogliere, l'orchestratore chiede all'utente quali
  fasi rifare. Per ognuna dice se c'e' gia' e di che ora; il quadro si rifa' sempre. Senza nessuno
  davanti (`claude -p`) vale l'argomento, altrimenti si rifa' tutto.
- **L'aggiornamento in giornata, su richiesta.** Macro e COT cambiano poco in un giorno, gamma e order
  flow di minuto in minuto. Se l'utente lo chiede ("tieni macro e COT"), si raccolgono solo le fonti
  delle fasi da rifare (`raccogli.py --fonti orderflow,gex`), si rifanno quelle fasi e sempre il
  quadro, dichiarando l'ora delle fasi riusate. Una fase si riusa solo se esiste per quel giorno.
  Niente si cancella: rapporti e dati sostituiti vanno in `precedenti/<HHMM>/`. Le regole stanno
  nell'orchestratore, sezione "L'aggiornamento in giornata".
- **Il risultato e' lo studio, in chat.** L'orchestratore chiude scrivendo il quadro in chiaro: la
  riga del giorno, le cinque righe, i livelli, le contraddizioni, gli orari e cosa lo smentirebbe.
  Tempi e percorsi vengono dopo. **Nel sito o in un artifact solo se l'utente lo chiede**, e sempre
  per il giorno attuale (aggiornando quello che c'e' o creandolo):
  - il sito locale mostra le giornate in `#/giorno/<data>`, con i quadri sostituiti;
  - l'artifact e' uno per giorno: lo genera `studio/pagina_del_giorno.py` e il suo indirizzo sta in
    `dati/artifact.txt`.

  L'orchestratore, sezione "La pubblicazione".
- **Serve ATAS aperto** per l'order flow e la GEX. Senza, quelle fasi scrivono `senza dati`.
- **Descrive, non opera**: nessun rapporto contiene direzioni, ingressi o stop.

### Le Fonti

| fase | fonte | perche' questa |
|---|---|---|
| macro | le fonti pubbliche di World Monitor, piu' Forex Factory e Nasdaq | [`macro/le-fonti-della-macro.md`](macro/le-fonti-della-macro.md) |
| COT | API pubblica della CFTC, Legacy e TFF | la fonte di Tradingster, che usa Fabio: stessi numeri, verificato sul 22 settembre 2026 |
| gamma | la catena delle opzioni CME che ATAS riceve (bridge `/opzioni`) | i valori degli indicatori di ATAS non si leggono; la GEX la calcoliamo noi, e ogni calcolo si archivia in `quant/dati/<NQ,ES>/gex/` |
| order flow | le quattro tab di ATAS (bridge) | i dati del metodo |

**Macro e COT vengono da fuori ATAS** su richiesta esplicita dell'utente del 15 ottobre 2026
(CLAUDE.md, decisione 2): sono contesto, mai dati di backtest.

## Lo Studio Dei Paper

**Cosa risponde**: cosa dice la letteratura su una domanda, letta per intero e verificata, fino alla
scheda dell'idea se la si chiede. E' la fase 1 del metodo quantitativo
([`quant/metodo/1-come-si-cercano-le-idee.md`](../quant/metodo/1-come-si-cercano-le-idee.md)) fatta
da agenti.

```text
/studio-paper <domanda o titoli> [filone:<nome>] [quanti:<n>] [idea:<studio>]

1 cerca     paper-cercatore       quant/paper/ricerche/<data>-<tema>.md, righe "trovato" nell'indice
2 scarica   paper-scaricatore     quant/paper/<filone>/<chiave>/<chiave>.pdf + sha256
3 leggi     paper-lettore         analisi.md          (un agente per paper, in parallelo)
4 verifica  paper-verificatore    analisi.md corretta (un agente per paper, mai lo stesso che l'ha scritta)
5 sintesi   paper-sintetizzatore  la sintesi, l'indice, e la scheda 1-idea.md se chiesta
```

- **Qui gli agenti hanno Bash**: scaricare, calcolare lo sha256 ed estrarre il testo del PDF sono il
  loro lavoro, e non c'e' un collettore che possa farlo prima.
- **Cloudflare lo supera una persona**: lo scaricatore si ferma, l'orchestratore chiede all'utente di
  cliccare nella finestra di Chrome, poi riprende.
- **La verifica e' avversaria**: un agente diverso controlla ogni numero dell'analisi sul PDF.

## Come Si Lancia

Da una sessione di Claude Code nel repository: `/studio-globale` o `/studio-paper <domanda>`. Senza
nessuno davanti, in un processo a parte:

```bash
claude -p "/studio-globale" --permission-mode bypassPermissions --output-format json
```

Gli agenti e le skill si caricano all'avvio della sessione: dopo averli cambiati serve una sessione
nuova. Una pianificazione (per esempio con l'Utilita' di pianificazione di Windows alle 14:00, prima
dei dati delle 14:30) si aggiunge solo su richiesta.

## Le Lezioni Del Primo Giro

**15 ottobre 2026, due prove complete con `claude -p`.**

**Lo studio dei paper ha funzionato al primo colpo**: due paper trovati, scaricati, letti in parallelo,
verificati e sintetizzati, circa 16 $ di modello. **Il verificatore ha trovato un errore grave del
lettore**: una soglia ("l'effetto c'e' solo con il dealer al 30-52% del flusso") che il paper riferisce
alla sola volatilita', e che faceva sembrare nullo l'effetto previsto su NQ. E' la regola "chi verifica
non e' chi ha scritto" che si paga da sola.

**Lo studio globale, nel primo disegno, era troppo lento**: le fasi in serie, e ogni agente raccoglieva
e ricalcolava da se'. Macro 6 minuti, COT 5, **gamma 33** (la GEX di NQ ed ES rifatta due volte e uno
script per gli strike scritto a mano), **order flow oltre 10** (1,8 MB di minuti scaricati, aree di
valore ricostruite dal 14 settembre, il sorgente C# del regime letto per rifarne il conto). Fermato
dopo un'ora con due fasi da fare. Da qui il disegno di oggi: un collettore, le analisi in parallelo
senza Bash, il confronto solo nel quadro, un tetto di turni per agente.

**Gli agenti hanno trovato difetti veri del codice**, e sono diventati codice:

| difetto | trovato da | ora |
|---|---|---|
| le notizie tagliate a 40 in totale: i feed di Fed e Treasury non entravano mai | macro | un tetto di 10 per fonte (`macro.py`) |
| percentili di long e short, z delle ultime quattro settimane, TFF a tre anni mancanti | COT | in `cot.py` |
| con un'attesa fissa di 8 secondi la catena di ES arrivava a meta' (14 scadenze su 32 perse) | gamma | `gex.py` rilegge ogni scadenza finche' l'open interest smette di crescere, e dichiara quotate, instabili e saltate |
| la GEX strike per strike non c'era | gamma | `per_strike` e `per_scadenza` in `gex.py` |
| la cache della GEX si riscriveva: nessuna storia | sintesi dei paper | ogni calcolo si archivia in `quant/dati/<NQ,ES>/gex/<giorno>/` |
| il giro lanciava un ricalcolo della GEX mentre un altro era in corso: due catene sullo stesso chart superano il tetto del bridge | il collettore | un lucchetto in `gex.esegui`, e `GEX_SENZA_RICALCOLO` quando raccoglie il collettore |
| gli esiti degli eventi, scritti dalla macro, portavano il giro a 9,8 KB, arrivato solo in anteprima | il giro | testi brevi nel calendario, e il giro sotto gli 8,5 KB |

**Aperto**: su ES, alle scadenze diverse dallo 0DTE arriva quotato circa un quarto delle opzioni (101 su
400), anche con l'attesa adattiva. Da capire se e' la connessione o il listino degli strike.

**Il secondo giro, con il disegno nuovo** (15 ottobre 2026, 10:44-11:01): **17 minuti** invece di oltre
un'ora. Raccolta 6 minuti, le quattro analisi in parallelo 6 e mezzo, il quadro 4 e mezzo. Nessuna fonte
mancata, nessuna direzione operativa nei rapporti, tutti sotto le 250 righe, e il quadro e' arrivato
da solo nella sezione 4bis del giro. Gli agenti hanno segnalato una trentina di **lacune del
collettore**, precise: quelle che costavano poco sono gia' nel codice (GEX per singola scadenza e ora
del future, `per_strike` a +-6%; ritardo in prezzo dal report COT, rollover per settimana, variazioni
TFF separate; lo shift del live alla lettera e il composito di sola cassa con i suoi nodi).

## La Coda Delle Lacune

**Chiuse la sera del 15 ottobre 2026**, su richiesta dell'utente ("fai tutto"):

| fase | cosa e' ora nel collettore |
|---|---|
| order flow | la cassa di oggi con shift e stacco, la notte separata, i profili per fasce, il composito di sola cassa a 90 sedute (si riempie con l'archivio: ATAS da' circa 30 giorni di minuti), la zona premium/fair value/discount, la durata delle barre range, la volatilita' realizzata, la variabile di BDLM, il giro raccolto dopo la GEX, la struttura oraria letta sugli swing, la tabella delle confluenze (`confluenze.json`) |
| indicatore | il regime dichiara la sua finestra, e c'e' la velocita' della barra chiusa (deploy del 15/10) |
| gamma | il future con la sua ora vera e il prezzo del bridge accanto, 120 strike per lato, lo zero cercato a +-8%, il segnale dell'OI stabile e il confronto con il calcolo precedente, la finestra del volume delle opzioni |
| macro | FRED senza chiave (CSV pubblico), il dato uscito degli eventi, VIX e rendimenti di oggi, le probabilita' FOMC dai future sui fed funds, l'ampiezza del Nasdaq-100, la descrizione delle notizie |
| COT | il rollover di calendario per ogni report, il profilo della settimana coperta da ciascun report, la TFF completa |

**Chiuse il 27 ottobre 2026** (lo studio di ES, `studio/rapporti/2026-10-27-ES/`): COT, macro, GEX e giro
seguono lo strumento (sopra); la struttura oraria del giro sommava 2 ore all'UTC e dopo il cambio dell'ora
del 25/10 scriveva "25h" e un'ora avanti: ora converte con il fuso di Roma (`scala_dei_tempi.py`); la
grana del giro su ES era 0,21 (la scala dello strumento, fuori dal tick) contro lo 0,25 del collettore,
e i due si azzeravano a vicenda la cache di ESZ6: ora e' un multiplo del tick (`grigliaMuri`).

**Risolta il 27/10**: la cassa cablata a 13:30Z-20:00Z in tutto il codice ora e' 09:30-16:00 di New York convertita per data ([la regola](../docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md)). **Ancora aperte**: il composito a 90 sedute ne ha 30 finche' l'archivio non cresce (su ES 17, dal 27/10:
la cache e' ripartita alla grana giusta); per ES niente tab Range nel workspace, e i big trade
non sono tarati su ES; l'ora di azzeramento
del volume delle opzioni va verificata fra le 22 e le 23 UTC; un OI "definitivo" del CME non esiste
nel bridge, c'e' solo il segnale di stabilita'.

**Il terzo giro, l'aggiornamento in giornata** (15 ottobre 2026, 11:04-11:20), lanciato a parole: "tieni
macro e COT di stamattina, rifai solo gamma e order flow e poi il quadro". L'orchestratore ha raccolto
solo `orderflow,gex`, archiviato dati e rapporti vecchi in `precedenti/1104/` e `precedenti/1110/`,
rifatto gamma e order flow in parallelo e il quadro, dichiarando l'ora delle fasi riusate. **16 minuti,
circa 5 $.** Il quadro nuovo ha scritto cosa era cambiato dalle 10:45, compreso un fatto che il quadro
di stamattina aveva messo fra quelli che lo avrebbero smentito, e ha corretto un proprio numero.
