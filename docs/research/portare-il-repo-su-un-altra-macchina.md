# Portare Il Repo Su Un'Altra Macchina

Stato: **procedura**. Scritta il 20 settembre 2026, quando il repo si e' spostato dal Mac a un PC
Windows e il primo `git pull` sul PC **non ha portato quasi niente di quello che ci si aspettava**.

Il motivo non era il push: era che **tre cose diverse viaggiano in tre modi diversi**, e si
somigliano abbastanza da farle confondere.

## Le Tre Cose, E Come Viaggia Ciascuna

| | dove sta | viaggia con `git pull`? |
|---|---|---|
| **il codice e le istruzioni** — `CLAUDE.md`, i documenti di metodo, gli script Python, il sorgente C# | nel repo | **si'** |
| **l'indicatore compilato** — la DLL che ATAS carica, cioe' i livelli sul chart e il pannello | `%APPDATA%\ATAS X\Indicators` (Windows, vedi sotto), `~/Library/Application Support/ATAS/Indicators` (macOS) | **no**: `bin/` e `obj/` sono in `.gitignore`, nel repo c'e' il **sorgente**, non il binario |
| **la memoria dell'agente** — `MEMORY.md` e i file in `memory/` | `~/.claude/projects/<percorso-del-progetto>/memory/`, **fuori dal repo** | **no**, e il nome della cartella cambia con la macchina perche' deriva dal percorso |

**Il sintomo che ha fatto scoprire la cosa:** dopo il pull sul PC non c'era il pannello in alto a
destra. Non poteva esserci. Il pannello e' disegnato dall'indicatore, e l'indicatore e' una DLL che
si compila — un `git pull` porta il `.cs`, non il `.dll`, e ATAS continua a caricare quello vecchio
(o nessuno).

## La Sequenza Su Una Macchina Nuova

```bash
git clone git@github.com:manumastro/orderflow-atas.git
cd orderflow-atas/FabioOrderFlow/src
./deploy.sh          # su Windows: da git-bash, serve il .NET SDK per net10.0
```

Poi **si riavvia ATAS**, altrimenti resta caricata la DLL precedente.

`deploy.sh` e `Indicators/Directory.Build.props` scelgono da soli il sistema. Non c'e' niente da
cambiare a mano, ma **su Windows i percorsi non sono quelli che sembrano**, ed e' costato una
build fallita e un deploy finito nel posto sbagliato il 20 settembre 2026.

## Su Windows Esistono Due ATAS, E Si Chiamano Quasi Uguale

| | ATAS X — quello che si usa | ATAS 8 — il vecchio |
|---|---|---|
| gli assembly | `C:\ATASX` | `C:\Program Files (x86)\ATAS Platform` |
| gli indicatori | `%APPDATA%\ATAS X\Indicators` | `%APPDATA%\ATAS\Indicators` |
| il processo | `AtasLauncherX.exe` | `Atas.exe` |

**Non sono intercambiabili, e sbagliare non da' un errore chiaro.**

- **Compilare contro ATAS 8** fa morire la build con un `CS1705` sul `System.Drawing.Common` che
  `OFT.Rendering` si aspetta, piu' due `CS1061` su `IInstrumentInfo.TimeZoneOffset` e
  `Security.ExpirationMoment`: sono API che ATAS 8 non ha. Sembra un errore del codice, non del
  percorso.
- **Copiare in `%APPDATA%\ATAS\Indicators`** mentre gira ATAS X non da' **nessun** errore: il
  deploy stampa "deployed", ATAS X non guarda quella cartella, e sul chart resta la DLL
  precedente. E' lo stesso sintomo di un `git pull` che non e' arrivato.

Entrambi gli script adesso provano ATAS X per primo. Il percorso degli assembly si puo' forzare con
`dotnet build -p:AtasAssemblyDir=...` se un giorno l'installazione si sposta.

Su macOS gli assembly vengono da `/Applications/ATAS X.app/Contents/MonoBundle` e le DLL finiscono
in `~/Library/Application Support/ATAS/Indicators`.

## Cosa Va Rifatto A Mano, Perche' Non Puo' Viaggiare

- **La memoria dell'agente.** Non si copia a mano: il contenuto e' versionato in
  `docs/research/memoria-di-orientamento/` e si installa con un comando, che trova da solo la
  cartella giusta sulla macchina in uso.

  ```bash
  python3 FabioOrderFlow/tools/semina_memoria.py
  ```

  Va lanciato **dopo** aver aperto l'agente una volta nel repo, cosi' la cartella di progetto
  esiste gia' e non va indovinata.
- **`.mcp.json`.** Puntava all'installazione locale di `playwright-mcp` del Mac. Adesso non
  contiene piu' un percorso: lancia `npx.cmd -y @playwright/mcp@latest`, che si risolve da solo
  ovunque sia `npm`. Resta da fare una volta per macchina `npx playwright install chromium`, che
  scarica il browser (~115 MB) fuori dal repo.

  **Perche' `npx.cmd` e non `npx`:** il client MCP lancia il comando **senza shell**, e su Windows
  un `npx` nudo non e' un eseguibile — si ottiene `WinError 2`, che il client riporta come
  `CONNECTION_CLOSED`, cioe' esattamente come un server che parte e muore. E' Windows-only: su
  macOS tornerebbe `npx`.
- **I file di stato locali**, tutti nella home e tutti rigenerabili: `~/.fabio-data-bridge.json`
  (la porta, la scrive il bridge da solo), `~/.fabio-data-bridge-levels.json` e
  `~/.fabio-data-bridge-watch.json` (livelli e pannello dell'ultima sessione),
  `~/.fabio-avvisi.log` e `~/.fabio-avvisi.letto`. **Non si portano**: sono residui, non fonti, e
  ripartire puliti e' preferibile — un livello di un'altra macchina e' un livello di un altro
  giorno, con lo stesso identico problema.

## Cosa Verificare Per Primo Sul PC

1. **Che il pull sia davvero arrivato.** `git log --oneline -3` deve mostrare gli stessi commit di
   `origin/main`, e `git status` il branch giusto. Se `CLAUDE.md` non ha le sezioni nuove, il
   problema e' li' e non altrove.
2. **Che l'indicatore sia quello nuovo.** Sul chart: le righe dei livelli devono essere **corte**
   (si fermano vicino al bordo destro) e l'etichetta deve mostrare solo il nome, col testo intero
   al passaggio del mouse. Se le righe attraversano tutto il chart, ATAS ha ancora la DLL vecchia.
3. **`avviso.py` su Windows.** Eseguito per la prima volta il 20 settembre 2026. Il balloon tip
   via WinForms **torna senza errore ma su Windows 11 spesso non si vede**: l'icona viene creata e
   distrutta prima che il sistema la mostri. Adesso si prova prima un **toast WinRT**, che compare
   sopra ad ATAS, suona e resta nel centro notifiche; il balloon resta come ripiego.

   **Attenzione a cosa prova cosa:** entrambe le strade escono con codice 0 anche quando non si
   vede niente, quindi **il codice di uscita non e' una verifica**. L'unica verifica e' guardare.
   `~/.fabio-avvisi.log` resta la fonte di verita'.
4. **Il bridge.** `python3 FabioOrderFlow/tools/bridge.py health --chart NQZ6`. Su Windows il
   comando potrebbe essere `python` invece di `python3`.
5. **Che il giro d'orizzonte esca intero**, tutte e nove le sezioni. Su Windows la console e'
   `cp1252` e il primo carattere di cornice faceva morire `giro_orizzonte.py` con
   `UnicodeEncodeError`: all'agente arrivava un traceback invece del contesto — **e una risposta
   esce lo stesso, costruita a memoria, senza che si veda**. Difeso in due punti (il programma
   riconfigura il proprio stdout, l'hook esporta `PYTHONUTF8=1`), ma e' il tipo di guasto che non
   grida: si controlla guardando che le sezioni ci siano davvero.

## Perche' Il Repo Si E' Spostato

Non per preferenza. L'esecuzione degli ordini passa da **Tradeify**, che usa Rithmic ma richiede
**R|Trader Pro** come punto di ingresso (System Name "Tradeify", poi *Allow Plugins* li' e
*Connect via RTrader Pro* in ATAS). **R|Trader Pro non ha una build macOS** — esiste una versione
web e una app mobile, nessuna delle due si aggancia via plugin. Il market data su Mac funzionava
benissimo con una connessione Rithmic diretta: e' l'esecuzione a non essere possibile, e da li' la
scelta di portare tutto su una macchina sola invece di tenerne due.

**Vincolo da ricordare se un giorno si aggiunge una seconda connessione Rithmic sullo stesso
ATAS:** un solo account per istanza puo' passare per RTrader Pro; gli altri devono restare
diretti, e un account non deve essere gia' collegato altrove mentre si prova ad agganciarlo.
