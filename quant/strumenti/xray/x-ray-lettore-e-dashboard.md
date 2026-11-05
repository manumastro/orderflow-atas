# X-Ray: Il Lettore, La Pagina E La Gestione

**30 ottobre 2026; la pagina dal 2 novembre.** I dati di Options X-Ray arrivano dalla cache cifrata
di ATAS X. Lo strumento ha tre parti, ciascuna con un solo compito:

| parte | cosa fa | dove si guarda |
|---|---|---|
| **XrayLive**, il server | legge la cache e scrive l'archivio; con `--serve` serve anche l'API e la pagina su `127.0.0.1:8790` | il log, `quant/dati/xray/lettore.log` |
| **XrayWeb**, il client | la pagina per leggere i dati, in stile QuantData (5 novembre 2026): la Lettura, le sei pagine di X-Ray (in prezzi di ES di default), la Volatilita', NQ · QQQ, le Fonti. React, TypeScript, Mantine, ECharts, compilati con Vite: disegna, non calcola | **http://127.0.0.1:8790/** |
| **XrayGui**, la gestione | controlla se lettore e server sono vivi e se le famiglie arrivano; avvia, ferma, compatta, apre la pagina | il collegamento X-Ray sul desktop |

Ci sono anche gli script, per l'agente e per il task all'accesso.

Il significato dei dati, come si leggono e come entrano nelle letture sono in
[`quant/metodo/x-ray-in-vivo.md`](../../metodo/x-ray-in-vivo.md). Il formato della cache e la
derivazione della chiave sono in `docs/ATASX_XRAY_CACHE.md` del repo `ATAS_Tool`.

## Cosa C'e' In Questa Cartella

| | |
|---|---|
| `XrayLive/Cache.cs` | dal file cifrato al chunk: chiave, AES, verifica SHA-256, gzip, MessagePack, float32 planarizzato, classificazione (plane, strikes, levels, pressure) |
| `XrayLive/Archivio.cs` | l'archivio delle serie: niente doppioni, arrotondamento, giornate chiuse in `.gz` |
| `XrayLive/Compatta.cs` | `--compatta`: le stesse regole applicate a un archivio gia' scritto, con verifica |
| `XrayLive/Program.cs` | il giro sulla cache, la pubblicazione, lo stato, gli argomenti |
| `XrayLive/Server/` | il server della pagina: `Lettura.cs` (legge l'archivio per giornata, solo le righe nuove), `Magazzino.cs` (le giornate in memoria), `Misure.cs` (tutti i calcoli), `Ponte.cs` (la cassa di New York e il prezzo di ES dal bridge), `Servitore.cs` (Kestrel, le rotte, lo stato) |
| `XrayLive/Volatilita/` | le fonti a confronto (dal 2 novembre 2026): `Formula.cs` (la formula del VIX, Black-76, il sorriso, la volatilita' da vanna/gamma), `CatenaAtas.cs` (la catena CME dal bridge), `CboePubblico.cs` (QQQ e NDX dal Cboe, la GEX), `Yahoo.cs` (VIX e VXN ufficiali), `Raccolta.cs` (i giri in background e l'archivio), `MisureVol.cs` (le rotte nuove) |
| `XrayLive.Test/` | le prove del server su un archivio sintetico, e della matematica della volatilita' su catene sintetiche |
| `scripts/prova_volatilita.py` | le fonti nuove: il server contro un calcolo Python indipendente sullo stesso file del Cboe, e la forma delle rotte |
| `XrayWeb/` | il client (2 novembre 2026; fino ad allora `XrayPagina/`, HTML e JS senza build): sorgente in `src/`, il build in `dist/` che il server serve: Vite, React 19, TypeScript, Mantine, ECharts, TanStack Query; gruppi Lettura, SPX · X-Ray (sei pagine), Volatilita', NQ · QQQ, Fonti (dieci pagine dal 5 novembre 2026). Come si compila, si prova e si estende: `.claude/skills/xray-pagina/SKILL.md` |
| `XrayGui/` | la finestra di gestione WinForms: Stato, Famiglie, Lettore |
| `scripts/xray_query.py` | per l'agente e per il collettore: `riassunto`, `status`, `coverage`, `levels`, `at`, `csv`; `riassunto_dati()` e `righe_giro()` da importare |
| `scripts/prova_server.py` | confronta il server con un calcolo indipendente in Python, sui dati veri |
| `scripts/prova_pagina.py` | prova la pagina nel browser (Playwright): errori, grafici disegnati, filtri, telefono, foto |
| `scripts/scrivi_lettura.py` | pubblica una lettura dal vivo nella pagina **Lettura** (2 novembre 2026): controlla la forma lineare e l'assenza di segnali, la salva in `quant/dati/letture/<giorno NY>/<HHMM>.json`; il server la serve in `/api/lettura` (`XrayLive/Server/Letture.cs`) |
| `scripts/leggi_pagina.py` | **la pagina letta come la vede l'utente** (2 novembre 2026): scheda per scheda il testo visibile, la scelta dei comandi e le serie e le linee dei grafici come sono disegnate (l'istanza di ECharts che `Grafico.tsx` appende al nodo come `__grafico`); `--foto DIR` una foto per scheda. E' da dove parte la lettura dell'agente: [come-si-leggono-le-opzioni-dal-vivo](../../metodo/come-si-leggono-le-opzioni-dal-vivo.md) |
| `scripts/xray_live_task.ps1` | il task all'accesso: `-Install`, `-Start`, `-Stop`, `-Status`, `-Remove` |
| `scripts/xray_live_run.ps1` | il lanciatore nascosto che il task chiama: un solo lanciatore, il lettore senza finestra, `lettore.log` |

## I Dati

In `quant/dati/xray/` (fuori da git, come gli altri dati):

```text
status.json                                   il lettore: vivo, ultimo giro, chunk, errori, archivio
latest/<famiglia>.json                        l'ultimo chunk; "last" e' la sua ultima colonna
series/SPX/<famiglia>/<giorno>.jsonl          la giornata in corso, una riga per chunk
series/SPX/<famiglia>/<giorno>.jsonl.gz       le giornate chiuse
journal.jsonl                                 un chunk per riga: quando, che famiglia, quante colonne nuove
index.tsv                                     i file della cache gia' letti (al riavvio non si rileggono)
lettore.log                                   l'output del lettore (ruotato a 5 MB)
```

**Le tre regole dell'archivio** (30 ottobre 2026). L'archivio era 3,8 GB in due mesi e cresceva di
circa 2 GB al giorno a indicatori aperti. Dopo le tre regole e' 577 MB, con le stesse informazioni:

1. **Una colonna si scrive una volta**. I chunk di X-Ray sono cumulativi: la GEX per strike arriva a
   blocchi orari che crescono minuto per minuto, quindi ogni colonna tornava fino a 60 volte
   identica. Ogni colonna ha una **chiave** e un'**impronta** del contenuto, e si scrive solo se la
   chiave e' nuova o il contenuto e' cambiato (una revisione).
   - la chiave della GEX/DEX/VEX per strike: istante, scadenza, giorni;
   - del profilo osservato: istante;
   - di previsione e giorno dopo: `asOf` e istante (ogni chunk e' una previsione nuova);
   - dei livelli: superficie, istante, fine.
2. **I numeri si arrotondano**: le greche a 6 cifre significative, i prezzi a 2 decimali. Arrivano
   in float32, che ne ha 7; scritti da double erano 20 caratteri di rumore (`-0.011099999999999888`).
3. **Le giornate chiuse si comprimono** in `.jsonl.gz`: quelle con la data di sessione prima di ieri e
   senza scritture da 2 ore, ogni 30 minuti. Un chunk tardivo di una giornata chiusa finisce in un
   `.jsonl` nuovo, che la volta dopo si accoda al `.gz` come altro membro. Chi legge legge prima il
   `.gz`, poi il `.jsonl`: `xray_query.colonne()` e `Dati.Colonne()` lo fanno gia'.

**La verifica della compattazione** del 30 ottobre:
- 653 giornate, 437.851 colonne lette, 405.449 tenute: 32.402 doppie, 28.232 revisioni;
- `--compatta` ha controllato giornata per giornata che le chiavi fossero le stesse;
- uno script Python indipendente ha confrontato chiavi e valori, entro l'arrotondamento.

## Compilare E Avviare

```bash
dotnet build quant/strumenti/xray/XrayLive -c Release
dotnet build quant/strumenti/xray/XrayGui -c Release
quant/strumenti/xray/XrayGui/bin/Release/net10.0-windows/XrayGui.exe
```

Serve Windows e .NET 10. Il lettore e la pagina, senza la finestra di gestione:

```bash
dotnet quant/strumenti/xray/XrayLive/bin/Release/net10.0/XrayLive.dll --watch --serve --out quant/dati/xray
powershell -File quant/strumenti/xray/scripts/xray_live_task.ps1 -Install   # all'accesso, con --serve
```

**Le opzioni del server**:
- `--serve` senza `--watch` accende solo il server, su un archivio scritto da un altro lettore (o per
  le prove, con `--porta 8791`);
- `--pagina DIR` e `--bridge URL` cambiano la cartella del client e il bridge (default
  `http://127.0.0.1:8787`);
- se la porta e' occupata, il programma esce con codice **5**.

**Per ricompilare il lettore** bisogna fermarlo, perche' la DLL e' bloccata:
1. `xray_live_task.ps1 -Stop` ferma task, lanciatore e lettore;
2. si ricompila;
3. `-Start` lo riavvia.

Fermarlo non perde niente: la cache e' di ATAS, e `index.tsv` dice cosa e' gia' stato letto.

**Un solo lettore per cartella**: un mutex per cartella (`Local\XrayLive-<hash>`). Una seconda
istanza esce con codice **4**. Anche il lanciatore ha il suo (`Local\XrayLiveLauncher`): il 30 ottobre
ne giravano due, e il secondo riprovava ogni minuto contro il mutex del lettore.

## La Pagina

**http://127.0.0.1:8790/**. Si accende all'accesso, con il lettore.

**In breve.** Dal 5 novembre 2026, per decisione dell'utente, la pagina e' fatta **in stile QuantData**: un
software, non un documento. In alto due barre ferme (il menu, l'ora, lo stato delle fonti; poi le pagine del
gruppo e i comandi del tempo), in fondo la barra del tempo. Ogni pagina comincia con una riga di tessere, la
risposta, e sotto ha una griglia di riquadri: il grafico grande entra nella prima schermata. I colori hanno un
ruolo solo: **verde freno, rosso acceleratore** per la gamma, **blu comprato, arancio venduto** per il flusso.
Le spiegazioni ci sono, ma non coprono i dati: si aprono con la "i" o con **Impara**.

**Lo stack** (2 novembre 2026; prima HTML e JS senza build). Si e' guardato su cosa e' fatta QuantData:
Next.js, React, Mantine, Redux e WebSocket, dai pacchetti del suo sito. Per una pagina locale servita da
Kestrel il rendering lato server non serve, quindi lo stack e':
- **Vite** con **React 19** e **TypeScript** strict;
- **Mantine** per i componenti, come QuantData;
- **ECharts** per i grafici: le mappe da decine di migliaia di celle, con import dei soli moduli usati;
- **TanStack Query** per la cache e il "Dal vivo";
- **React Router** con gli indirizzi a hash;
- i font Inter e Roboto Mono in locale: la pagina funziona anche senza rete;
- versioni esatte nel `package.json`.

Il sorgente e' in `XrayWeb/src/`. Il build (`npm run build`, in `XrayWeb/dist/`) e' quello che il server
serve, e come `node_modules/` sta fuori da git: dopo un clone si compila prima di avviare il server. Come
si estende (una pagina, una fonte, i colori, le prove), e le regole della forma e dei colori:
`.claude/skills/xray-pagina/SKILL.md`.

**Il guscio**:
- **la barra alta**: il marchio ("Opzioni dal vivo"), i cinque gruppi (il gruppo X-Ray apre un menu con le sue
  sei pagine), l'orologio di New York e d'Italia, **lo stato delle quattro fonti** (una pastiglia per fonte, con
  icona, stato ed eta'; un clic apre i flussi di quella fonte) e **Impara | Veloce**;
- **la barra del contesto**: le pagine del gruppo come linguette, la "i" della pagina, i filtri, e la **riserva
  breve** del gruppo ("stima OptionsDepth · non provato"; un clic apre quella intera);
- **la barra del tempo**, fissa in fondo, nelle pagine che usano l'istante: il cursore sulla giornata con la
  cassa 09:30-16:00 piu' chiara, ◀ ▶ di un passo (con Maiusc di 15 minuti) e "Torna al vivo";
- **nel passato** (un istante o un giorno che non e' adesso) la testata ha un bordo ambra e la chip "◷ Passato";
- **sul telefono** le pagine e i filtri stanno in due cassetti, e i cinque gruppi in una barra in basso.

**I gruppi e le pagine**, dieci (fino al 5 novembre erano undici: Livelli e Nel tempo sono diventati "Livelli
nel tempo", e `#/tempo` rimanda li'). I **Confronti** (X-Ray contro la nostra catena ES) sono stati tolti il 5
novembre 2026: su ES prevale X-Ray, ed e' l'unica GEX che si mostra (x-ray-in-vivo, "Come si legge"). **I prezzi
sono in ES di default**, come ATAS disegna X-Ray sul chart di ES; in SPX con il filtro Prezzi (`?prezzi=SPX`).

| gruppo | pagina | cosa mostra |
|---|---|---|
| Lettura | **Lettura** (`#/lettura`) | il punto della situazione che l'agente scrive su richiesta, con il prezzo di adesso accanto |
| SPX · X-Ray | **Adesso** (`#/adesso`) | le quattro domande in sei tessere (Prezzo, Regime, Flip piu' vicino, Meteo, Muri sul valore, Pressione 0DTE); il profilo "Freno e acceleratore, prezzo per prezzo"; "La scala dei prezzi" con le zone fuse e i bordi del valore; "Chi ha comprato delta oggi" |
| | **Per strike** (`#/strike`) | GEX, DEX o VEX di una scadenza, netto o call e put; le tessere dei muri (o del piu' comprato e venduto); "Strike vicini al prezzo", sopra e sotto |
| | **Heat map** (`#/heatmap`) | una riga per strike, una colonna per scadenza (o per gruppo, nel DEX), il valore nella cella e la distanza nella cella dello strike; la riga del prezzo e la netta di ogni colonna |
| | **Livelli nel tempo** (`#/livelli`) | "Il prezzo e i livelli della giornata": flip e picchi minuto per minuto sul prezzo, con lo sfondo della mappa a scelta (gamma, charm, GEX per strike, nessuno); "La scala dei livelli" |
| | **Deriva** (`#/deriva`) | la giornata in quattro pannelli allineati: "Il prezzo e il regime", "GEX netta per scadenza", "Delta accumulato per gruppo" (con i minuti anomali segnati), "Pressione 0DTE" |
| | **Previsione** (`#/previsione`) | "La mappa prevista" (resto della seduta o seduta dopo), con il flip previsto; "Livelli della prossima seduta" |
| Volatilita' | **Volatilita'** (`#/volatilita`) | il meteo: "Il meteo · Nasdaq 100" e "Il meteo · S&P 500", poi "Nel tempo", "Struttura a termine" e "Sorriso" per indice |
| NQ · QQQ | **NQ · QQQ** (`#/nq`) | la vista Exposure: le tessere (Regime NQ, Zero gamma, Zero 0DTE, Call wall, Put wall, Movimento atteso oggi), il "Profilo GEX per strike, in prezzi di NQ" nel colore del segno, "I livelli" delle tre fonti, "La GEX viva di NQ nel tempo" |
| Fonti | **Fonti** (`#/fonti`) | la diagnosi: "Lo stato di ogni fonte" per gruppo, "Il lettore di X-Ray e il server", "Le famiglie di X-Ray" |

**I filtri** (nella barra del contesto; quelli che una pagina non usa restano al loro posto, spenti, con la
ragione scritta):
- **Giorno**: le giornate dell'archivio, con ◀ ▶.
- **Istante**: il cursore sulla giornata, minuto per minuto; si muove anche con le frecce della tastiera.
  "Dal vivo" segue l'ultimo dato e aggiorna la pagina ogni 15 secondi.
- **Prezzi**: ES (default) oppure SPX; la base ES-SPX sta nella "ⓘ" accanto.

Tutte le scelte finiscono nell'indirizzo (`#/heatmap?giorno=2026-10-30&ts=...&prezzi=ES`): un link
riapre la stessa vista. Gli indirizzi vecchi senza la barra (`#heatmap?...`) e `#/tempo` si aprono ancora.

**Il prezzo di riferimento**:
- in cassa e' lo spot di SPX;
- fuori dalla cassa lo spot di X-Ray resta fermo, e il server ricostruisce SPX come **ES meno la base**;
- la base e' la mediana di ES − spot sui minuti di cassa del giorno: il 30 ottobre era +30,45, su 389
  minuti, con i singoli minuti fra 27 e 37;
- ES viene dal bridge di ATAS (il chart ESZ6 a 1 minuto); senza bridge tutto resta in SPX, e la
  pagina lo dice.

**I colori hanno un ruolo, uno solo** (5 novembre 2026), validati per il daltonismo con lo script della skill
dataviz contro il riquadro `#111723`:
- **verde `#4fae38` = freno**, cioe' gamma positiva dei market maker; **rosso `#b5443f` = acceleratore**,
  gamma negativa;
- **blu `#3987e5` = comprato**, **arancio `#d95926` = venduto**: il flusso (DEX, VEX, charm);
- **un riquadro mostra o la gamma o il flusso**, mai tutti e due; e un grafico mostra o il segno o le fonti;
- il prezzo e' l'inchiostro chiaro; **il flip e lo zero gamma sono grigi tratteggiati**; i bordi del valore
  (VAL, VAH, POC, muri del ping-pong) grigi a puntini;
- **le quattro fonti** hanno colore, icona e tratto fissi: X-Ray magenta, linea piena; **catena CME viola**
  (era blu fino al 5 novembre), linea piena; Cboe giallo, tratteggiato; Yahoo verde scuro, a puntini. Cboe e
  Yahoo sono a 6-8 di distanza per il daltonismo, quindi ogni linea ha anche il suo tratto e il nome in fondo;
- il blu notte dei comandi scelti e' solo un fondo, mai un dato; lo stato delle fonti e' sempre icona e
  parola.

La tabella intera e le regole: `.claude/skills/xray-pagina/SKILL.md`, "Le Fonti E I Colori".

### L'API

Tutte le rotte sono `GET` e rispondono in JSON. I parametri sono facoltativi: senza `giorno` vale
l'ultimo, senza `ts` l'ultimo istante. I prezzi sono in SPX salvo dove e' scritto; la base per portarli su ES
e' in `/api/base`. **Il server calcola, la pagina disegna**: ogni risposta che serve a una pagina si calcola
qui (dal 5 novembre 2026 anche le quattro domande, la scala dei livelli, i minuti anomali, il verdetto della
volatilita' e gli scalini).

| rotta | cosa restituisce |
|---|---|
| `/api/stato` | il lettore (status.json), l'eta' di ogni famiglia, il server e il bridge |
| `/api/giorni`, `/api/istanti?giorno` | le giornate; gli istanti di una giornata e la sua cassa in ms UTC |
| `/api/base?giorno` | la base ES-SPX, quanti minuti ha usato e di quale giornata |
| `/api/adesso?giorno&ts` | il punto intero: riferimento, gamma e charm al prezzo, flip, picchi, outlook, GEX e VEX per scadenza con i muri, DEX accumulato per gruppo, pressione. Dal 5 novembre: `gamma.da` (da quando il segno al prezzo e' lo stesso), `flipVicino` (con il regime sopra e sotto), `scala` (la scala dei livelli con le zone fuse e i bordi del valore, `Server/Scala.cs`), `outlook.seduta`, `pressione.verso` e `unita` (null finche' il segno di `pressure__0dte` non e' scritto in x-ray-in-vivo) |
| `/api/profilo?greca&ampiezza` | la gamma (o la charm) osservata per prezzo, con flip e picchi |
| `/api/strike?metrica&gruppo&scadenza&ampiezza` | per strike: la GEX o la VEX dei market maker, oppure il DEX accumulato di un gruppo; dal 5 novembre `muri`, `entroUnoPercento` e `flip` |
| `/api/heatmap?metrica&ambito&ampiezza` | strike per scadenze (o per gruppi), con la colonna del totale; dal 5 novembre `nette` (la netta di ogni colonna su tutti gli strike) e `livelli` |
| `/api/intervalli?greca&passo&ampiezza`, `/api/intervalli-strike?metrica&scadenza&passo` | tempo per prezzo e tempo per strike, con il prezzo e i flip |
| `/api/deriva?passo` | le serie della giornata, allineate al minuto; dal 5 novembre `adesso` (l'ultimo istante), `anomali` (i minuti anomali del delta) e `unita` |
| `/api/livelli?passo` | i flip e i picchi nel tempo, e l'outlook con la sua `seduta`; ogni traccia di `zeri` e' un flip solo, mai punti di tracce diverse uniti |
| `/api/previsione?tipo&greca&asOf` | una previsione intera, e l'elenco di tutte quelle del giorno; dal 5 novembre `zeri` (il flip previsto), `seduta`, `taglioScadenza`, `alPrezzo`, `sintesi` |
| `/api/quattro-domande?radice=ES\|NQ&giorno&ts` | **(5 novembre 2026)** le opzioni in quattro domande: il regime (con "segnali opposti" quando il prezzo e lo 0DTE non concordano), il meteo, i muri vicini ai bordi del valore, il grilletto. Tutti i prezzi in ES (o NQ). La regola sta in `Server/QuattroDomande.cs`, una copia sola: `xray_query.quattro_domande` la legge da qui |
| `/api/scalini?radice=NQ\|ES&giorno` | **(5 novembre 2026)** gli scalini di mese, settimana e giorno e il ping-pong del giornaliero, dal chart Daily del continuo con lo scarto del rollover. La regola sta in `Server/Scalini.cs`, una copia sola: la usano i bordi del valore, e `FabioOrderFlow/tools/gli_scalini.py` (il giro e lo studio) e' il client. Le chiavi sono quelle del dizionario Python di prima; senza bridge o senza il chart, 503 con l'`errore` |

### La Gestione

`XrayGui` non disegna piu' i dati. Fino al 1 novembre aveva le pagine Adesso, Per strike e Storia;
ora quelle letture stanno nella pagina. Le sue tre pagine:
- **Stato**: il lettore, il server della pagina, il bridge, l'archivio;
- **Famiglie**: l'eta' di ogni famiglia e l'indicatore da tenere aperto su ES;
- **Lettore**: avvia, ferma, avvio all'accesso, compatta, cartella, log.

In alto c'e' il pulsante **Apri la pagina**.

## Le Fonti A Confronto

**Dal 2 novembre 2026**, su richiesta dell'utente ("integriamolo nel nostro X-Ray, come farebbe QuantData,
e inseriamo cio' che possiamo su NQ e QQQ; si capisca che i dati vengono da fonti diverse, ma si possano
sovrapporre per vedere se convergono"). Il server mette accanto quattro fonti, ognuna con un id fisso:

| fonte | cos'e' | ritardo | chi la raccoglie |
|---|---|---|---|
| `xray` | X-Ray, la stima di OptionsDepth su SPX, dalla cache di ATAS | 1 minuto | il lettore |
| `catena` | la catena delle opzioni CME (ES, NQ) che ATAS riceve, con il **calcolo nostro** | 1-5 minuti | il server: VIX, VXN e sorriso ogni 5 minuti; la GEX viva e la struttura a termine ogni minuto |
| `cboe` | il file pubblico del Cboe per **QQQ** e **NDX**: bid, ask, volatilita', open interest. Fuori da ATAS: si aggiunge alla decisione 2 come contesto, mai in un backtest | 15 minuti | il server |
| `yahoo` | **VIX** e **VXN** ufficiali, via Yahoo: il riferimento con cui si confronta cio' che ricostruiamo | ~15 minuti | il server, quando la pagina li chiede |

### Cosa Si Calcola

- **VIX e VXN ricostruiti** (`Volatilita/Formula.cs`): la formula del Cboe sulle opzioni ES (VIX) e NQ (VXN),
  con i due venerdi' attorno ai 30 giorni. Il metodo e la prova con i numeri:
  [`le-opzioni-in-atas.md`](../../metodo/le-opzioni-in-atas.md), "Il VIX E Il VXN Rifatti Dalla Catena".
  Dal vivo si usa il medio bid/ask. Se il book non basta (meno di 25 strike), si usa il settlement
  dell'ultima chiusura, e la riga lo dice (`prezzi`).
- **La stessa misura su NDX e QQQ** (`vol30-ndx`, `vol30-qqq`) dal file del Cboe. Fuori dalla cassa il
  Cboe azzera bid e ask di QQQ: si usa il suo prezzo teorico, dichiarato. C'e' anche l'`iv30` che il Cboe
  calcola da se'.
- **La volatilita' di X-Ray** (`atm-xray`): dal rapporto vanna/gamma della scadenza opex
  ([`x-ray-in-vivo.md`](../../metodo/x-ray-in-vivo.md), "La Volatilita' Implicita"). E' la volatilita'
  al prezzo, piu' bassa del VIX, che pesa anche le ali.
- **La GEX di QQQ e NDX** (`CboePubblico.Gex`): convenzione A come `gex.py`, cioe' gamma × open interest
  × 100 × S² × 1%, in milioni di $ per 1%. La gamma e' di Black-Scholes con la volatilita' del Cboe, non
  quella del file, che ha 4 decimali. Zero gamma sticky strike su ±8%; scadenze fino a 70 giorni.
- **In prezzi di NQ**: QQQ × (NQ/QQQ), NDX + (NQ − NDX), con NQ al minuto dell'ultimo prezzo del
  sottostante (dal bridge). La conversione e' scritta in ogni fotografia.
- **I $ per 1% si possono confrontare fra le tre fonti del Nasdaq-100**: sono la stessa esposizione,
  presa su tre mercati diversi.

### Gli Orari E Le Sottoscrizioni

- **la catena CME**: ogni 5 minuti a CME aperto, da domenica alle 18:00 a venerdi' alle 17:00 di New York,
  con la pausa 17-18. La scadenza vicina si legge sul chart M1 del contratto, quella lontana sul Daily del
  continuo: ogni chart ha il suo tetto di 500 sottoscrizioni, e cosi' le due scadenze restano aperte fra un
  giro e l'altro. L'M30 resta a `gex.py`;
- **la GEX viva di NQ ed ES** (`Volatilita/GexViva.cs`, dal 2 novembre 2026, su richiesta: "riguardo NQ si'"):
  - lo stesso modello di `gex.py`, tenuto in memoria;
  - ogni minuto, a CME aperto, si rilegge **lo 0DTE** e si ricalcola la GEX al prezzo corrente;
  - le altre scadenze fino a 70 giorni si rileggono **a rotazione**, due al minuto. Al primo giro si leggono
    tutte: una lettura costa 7-35 secondi, e ES ne ha circa 30;
  - si legge sul chart M30, mentre l'M1 e il Daily tengono le due scadenze del VIX;
  - una riga al minuto va in `gex-<NQ|ES>.jsonl`, con totale, zero gamma e muri, di tutte le scadenze e
    dello 0DTE;
  - il limite resta quello di `gex.py` e degli Options Chain di ATAS: l'open interest e' di ieri;
  - **provata contro `gex.py` allo stesso istante** (2 novembre 2026, alle 09:02 NY): totale −561 contro
    −558, zero gamma 29.220 contro 29.215, muri 29.000/28.000 uguali, 0DTE +8 contro +9. Un giro dura circa
    75 secondi;
  - **provata e scartata**: 60 strike per lato, per tenere aperte insieme lo 0DTE e la scadenza a rotazione.
    I giri scendevano a 10 secondi, ma lo zero gamma passava a 29.906 e il put wall a 28.500, perche' si
    perdevano le put lontane. Le righe di quei dieci minuti sono state tolte dall'archivio;
  - **due muri quasi alla pari** si scambiano il ruolo: su NQ il 29.000 e il 31.000 valevano 52 e 51 M$ per 1%;
  Gli Options Chain non hanno una cache su disco da leggere, come invece X-Ray: chiedono la catena alla
  connessione, come il bridge. Fino al 2 novembre il server lanciava `gex.py` ogni 30 minuti; ora `gex.py`
  resta al giro e allo studio;
- **il Cboe**: ogni 15 minuti dalle 9:30 alle 16:45 di New York, ogni 3 ore fuori. **Fuori dalla cassa il
  file del Cboe ha i prezzi dell'ultima chiusura**: QQQ e NDX restano "vecchi" (13-17 ore la mattina) finche'
  non apre la cassa. Non e' un guasto, e' la fonte;
- **il prezzo vero di QQQ** (5 novembre 2026, su richiesta dell'utente): da Yahoo, anche nel pre-mercato e nel
  dopo-mercato, riletto al massimo ogni 30 secondi (`Yahoo.Prezzo`). Sta nella risposta di `/api/nq/gex`
  (`qqq`), con la variazione su ieri, il rapporto NQ/QQQ di adesso e, accanto, il prezzo del file del Cboe.

**Due guasti trovati il 5 novembre 2026, e come si vedono adesso**:
- il **VXN ricostruito era fermo da un giorno**: su NQ la coppia dei venerdi' attorno ai 30 giorni (4/12 e
  11/12) non dava varianza ne' dal book ne' dal settlement. Ora si provano fino a quattro coppie (prima i
  venerdi', poi le scadenze vicine di ogni tipo), e l'errore dice quali;
- la pagina Fonti **non mostrava l'errore di una fonte che non aveva mai dato un dato buono** (`Tentato >
  null` e' falso): restava "assente". Ora l'errore si vede, va anche nel log del lettore (`[catena-nq] ...`),
  e una fonte nel suo primo giro e' **"in corso"**, con l'avanzamento ("primo giro: 9 scadenze lette su 17"):
  il primo giro della GEX viva legge tutta la catena e dura minuti (ES 29 scadenze, NQ 17). Durante il primo
  giro la struttura a termine e la lettura della volatilita' usano ancora `gex.py`.

### Dove Va

In `quant/dati/volatilita/<giorno di New York>/`:

```text
serie.jsonl                 una riga per valore: {ts, id, v}; vix-catena, vxn-catena, vol30-*, iv30-*
catena-ES.json, -NQ.json    l'ultima fotografia: le due scadenze, il VIX, il sorriso
cboe-QQQ.json, -NDX.json    l'ultima fotografia: vol30, termine, sorriso, GEX per strike, conversione in NQ
cboe/HHMMZ-<SIM>.json       la storia del Cboe, una fotografia per lettura
```

La GEX della catena resta dove la mette `gex.py`: la cache `~/.fabio-gex-<strumento>.json` e la storia in
`quant/dati/<NQ|ES>/gex/<giorno>/`.

### Le Rotte

| rotta | cosa restituisce |
|---|---|
| `/api/fonti` | lo stato di ogni flusso: ultimo dato, secondi, `ok`/`vecchio`/`errore`/`assente`/`in corso`, la nota. Dal 5 novembre 2026 Yahoo e' diviso in `yahoo-vix`, `yahoo-vxn` e `yahoo-qqq`, e `sintesi` da' una riga per fonte (xray, catena, cboe, yahoo) con lo stato del flusso peggiore: e' lo stato delle fonti della barra alta |
| `/api/vol/serie?giorno` | le serie della giornata, con fonte, indice (SPX o NDX) e `misura` (`stile-vix` o `atm`), da sovrapporre |
| `/api/vol/termine` | la volatilita' al prezzo per scadenza: ES e NQ (gex.py), X-Ray (tre scadenze), NDX e QQQ (Cboe); dal 5 novembre gli `eventi` del calendario entro 70 giorni, e `sospetto` sui punti oltre il doppio della mediana della curva (dato sporco) |
| `/api/vol/in-breve` | la volatilita' letta per il prezzo, per SPX (ES) e NDX (NQ): livello e fascia, variazione da stamattina, movimento atteso oggi e in una giornata, breve contro mese, skew, **implicita meno realizzata** (il livello meno la volatilita' vera del future in 20 sedute, dal chart M30 del contratto, `Raccolta.Realizzata`; e la realizzata a 5 sedute), e le frasi (5 novembre 2026; le soglie in `MisureVol.InBreve`, convenzioni dichiarate, non provate). Dal 5 novembre: `fasciaPrezzi` e `confronto`, il verdetto (convergono, divergono, non si confronta) **solo fra le misure stile VIX** con un punto negli ultimi 30 minuti; le misure al prezzo (ATM) sono escluse e dette. Nella pagina Volatilita', i riquadri "Il meteo · Nasdaq 100" e "Il meteo · S&P 500" |
| `/api/vol/sorriso?indice=SPX\|NDX` | il sorriso in moneyness (strike/forward − 1) della scadenza vicina ai 30 giorni, per fonte |
| `/api/nq/gex?ambito=tutte\|0dte` | la GEX per strike in prezzi di NQ: catena NQ (la GEX viva), QQQ e NDX, con zero gamma, muri e conversione; `qqq`, il prezzo vero di QQQ (Yahoo); `prezzoNQ` dal bridge, non piu' dalla cache di gex.py. Dal 5 novembre: `unOraFa` (lo stato di un'ora prima), `movimentoOggi`, `finestra` (prezzo ± 2 movimenti attesi) e `scala` (i livelli delle tre fonti con le zone fuse, soglia 25 punti) |
| `/api/gex/tempo?radice=NQ\|ES&giorno` | la GEX viva minuto per minuto: prezzo del future, totale, zero gamma e muri, di tutte le scadenze e dello 0DTE (nella pagina NQ · QQQ, "La GEX viva di NQ nel tempo") |
| `/api/es/gex-confronto?giorno&ts` | (non piu' usata dalla pagina dal 5 novembre 2026) X-Ray contro la catena ES, normalizzate |

**Una lettura di prova, lunedi' 2 novembre 2026 alle 06:10 di New York**:
- **VIX**: ricostruito 18,64, ufficiale 18,95; la volatilita' al prezzo di X-Ray (opex) era 15,6;
- **VXN**: ricostruito 26,96; alla chiusura del 30/10 da NDX (Cboe) 28,05, ufficiale 28,50;
- **zero gamma del Nasdaq-100 in prezzi di NQ**:

  | fonte | zero gamma | GEX totale (M$ per 1%) |
  |---|---|---|
  | NQ, catena di ATAS | 29.054 | −337 |
  | NDX, Cboe | 29.034 | −1.650 |
  | QQQ, Cboe | 29.303 | −4.077 |

  NQ e NDX convergono; il NDX e il QQQ del Cboe sono alla chiusura di venerdi'.

## Le Prove

```bash
dotnet test quant/strumenti/xray/XrayLive.Test                     # il server e la matematica della volatilita'
python quant/strumenti/xray/scripts/prova_server.py                # il server contro Python, sui dati veri
python quant/strumenti/xray/scripts/prova_volatilita.py            # le fonti nuove contro Python, e le rotte
cd quant/strumenti/xray/XrayWeb && npm test && npm run build        # la pagina: Vitest e il build
python quant/strumenti/xray/scripts/prova_pagina.py                # la pagina in Chromium, con le foto
python quant/strumenti/xray/scripts/leggi_pagina.py                # la pagina letta come la vede l'utente (non e' una prova)
```

**Le prove aggiunte il 5 novembre 2026**, con la pagina in stile QuantData:
- in xunit, `QuattroDomandeTest.cs`: le quattro domande ("segnali opposti", le frasi di Python, i bordi vuoti con
  la nota senza bridge), la scala con le zone fuse (`ScalaTest`), gli scalini (`ScaliniTest`: scalino su e giu',
  pausa, ping-pong, scarto del rollover, giornata aperta, le chiavi di Python, settimana e mese per giorno di
  borsa con il primo mese corto scartato), i minuti anomali (`AnomaliTest`). **38 prove su 38**;
- **gli scalini, server contro Python**: prima di togliere il calcolo da `gli_scalini.py`, la sua uscita su NQ ed
  ES (il Daily del continuo, nove anni) e' stata salvata. `/api/scalini` da' lo stesso dizionario, chiave per
  chiave e valore per valore, e il giro stampa le stesse righe;
- in Vitest le prove sono divise per gruppo (`pagine/xray/xray1.test.tsx`, `xray2.test.tsx`, `lettura/`,
  `volatilita/`, `nq/`, `fonti/`), piu' `componenti/componenti.test.tsx` per il guscio, il riquadro e la scala
  dei prezzi.

**I risultati del 2 novembre 2026**, sul server vero dopo il cambio di pagina:
- **19 prove su 19** in xunit: le 8 del server su un archivio sintetico, e 11 sulla volatilita'. Sulle catene
  sintetiche di Black-76 la formula del Cboe ritrova la volatilita' piatta entro 0,3 punti, e con lo skew
  sta sopra la volatilita' al prezzo. Si provano anche il buco nella catena, l'interpolazione, l'inversione,
  la volatilita' da vanna e gamma entro 0,6 punti, il file Cboe e il segno della GEX, gli orari del CME
  nei due fusi;
- **759 confronti su 759** fra il server e Python sui dati di X-Ray;
- **34 prove su 34** sulle fonti nuove. Il server e un calcolo Python scritto a parte, sullo stesso file
  del Cboe, danno gli stessi numeri al millesimo: volatilita' a 30 giorni di NDX (28,052) e QQQ (27,218),
  GEX totale e muri. Poi le rotte nuove: campi, fonti, valori plausibili;
- **34 prove su 34** in Vitest e **88 controlli su 88** nel browser: le sette pagine di X-Ray come
  prima, la testata, i gruppi nuovi con i dati veri, l'indirizzo vecchio, la larghezza di un telefono.

In piu', la pagina e' stata guardata a mano, una pagina alla volta, con Playwright.

**I difetti trovati provando**, non leggendo il codice:
- **il campo `vex` di X-Ray e' la vanna, non la vega**: il rapporto vex/gex cambia segno al forward
  ([`x-ray-in-vivo.md`](../../metodo/x-ray-in-vivo.md), "La Volatilita' Implicita");
- **la catena di ATAS arriva a pezzi**: la prima lettura di NQ aveva 54 settlement su 226 e dava un VIX
  sbagliato; ora si rilegge finche' i prezzi smettono di crescere;
- **fuori dalla cassa il Cboe azzera bid e ask di QQQ**: niente volatilita' a 30 giorni, finche' non si e'
  usato il suo prezzo teorico;
- **il sorriso di X-Ray strike per strike e' a denti di sega**, anche sugli strike pesanti: la curva e' la
  mediana su 7 strike;
- **l'eta' del Cboe era quella dello scaricamento**: lunedi' mattina diceva "38 s" per prezzi di venerdi';
- con ECharts 6, cambiando pagina mentre si disegna una mappa grande, il disegno a pezzi continuava sul
  grafico chiuso: ora si disegna in una volta;
- **il DEX per gruppo e' un flusso, non una posizione**. Ogni colonna copre 60 secondi (`from` =
  `ts` − 60 s) e i valori oscillano attorno allo zero. Fino al 2 novembre lo si leggeva come la
  posizione del gruppo, anche in `xray_query.py`; ora si somma dall'inizio della giornata;
- un bridge lento faceva ricadere il prezzo sullo spot fermo di X-Ray: ora si tiene l'ultimo dato
  buono;
- una risposta arrivata dopo un cambio di pagina disegnava su un grafico gia' chiuso;
- un indirizzo incollato, o il tasto Indietro, cambiava la pagina ma non la giornata.

## Misurato Il 30 Ottobre 2026

| | |
|---|---|
| cache letta | 10.842 chunk, **0 errori** |
| archivio | 3.798 MB prima della compattazione, **577 MB** dopo |
| RAM del lettore | 49-99 MB (la prima versione, che riscriveva il giorno intero a ogni chunk, ne prendeva 4,3 GB) |
| giro di aggiornamento | decine di ms quando non c'e' niente di nuovo, 15 s di cadenza |
| seconda istanza | rifiutata dal mutex, codice 4 |
| il server (2 novembre) | quasi tutte le risposte sotto 0,1 s; la prima previsione di una giornata circa 3 s (30 MB compressi), poi resta in memoria |
| la memoria con la pagina | 114 MB all'avvio; circa 600 MB dopo aver sfogliato tre giornate intere, previsioni comprese. Le giornate non usate da 10 minuti si liberano, e delle previsioni se ne tengono al massimo quattro |

**I difetti trovati misurando**, non leggendo il codice:
- `±Infinity` non si serializzava in JSON (ora e' `null`);
- il registro rileggeva la colonna dei byte invece del percorso, quindi rielaborava tutto e duplicava;
- c'era una riga per colonna invece di una per chunk (4 GB);
- la GUI leggeva `journal.jsonl` bloccando chi scriveva (5 chunk persi);
- il 30 ottobre: colonne ripetute fino a 60 volte, due lanciatori insieme, `fixext` di MessagePack
  saltato della lunghezza sbagliata, gli errori di lettura segnati come letti per sempre.

## La Copia Gemella

Lo stesso codice, in una versione precedente, e' in `tools/XrayLive` e `tools/XrayGui` del repo
`ATAS_Tool`, dove si e' fatta l'analisi del binario. **La copia canonica e' questa**: qui ci sono i
dati e il task all'accesso. Quando si cambia il lettore, si cambia qui.

## Limiti

- La gestione e' Windows e WinForms. La pagina gira in qualunque browser, ma solo su questo PC: il
  server ascolta su `127.0.0.1`.
- Non parla con il plugin ne' con la rete; il prezzo di ES lo chiede al bridge di ATAS. Se il lettore
  e' fermo, pagina e gestione mostrano dati vecchi **e lo dicono** in alto, in rosso.
- La pagina mostra solo quello che X-Ray porta. Non c'e' il flusso delle opzioni scambio per scambio
  (sweep, blocchi, Net Drift sul premio), ne' azioni o dark pool: servirebbe un'altra fonte, come
  QuantData o OPRA, quindi una richiesta esplicita (decisione 2).
- La copertura dipende da quali indicatori X-Ray sono aperti su un chart ES: la cache si riempie solo
  mentre qualcuno guarda.
- La chiave dipende dal MachineGuid e dall'utente di Windows: su un altro PC o con un altro utente la
  cache di ATAS e' un'altra, e va letta li'.
