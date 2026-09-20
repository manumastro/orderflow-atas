# Portare Il Repo Su Un'Altra Macchina

Stato: **procedura**. Scritta il 20 settembre 2026, quando il repo si e' spostato dal Mac a un PC
Windows e il primo `git pull` sul PC **non ha portato quasi niente di quello che ci si aspettava**.

Il motivo non era il push: era che **tre cose diverse viaggiano in tre modi diversi**, e si
somigliano abbastanza da farle confondere.

## Le Tre Cose, E Come Viaggia Ciascuna

| | dove sta | viaggia con `git pull`? |
|---|---|---|
| **il codice e le istruzioni** — `CLAUDE.md`, i documenti di metodo, gli script Python, il sorgente C# | nel repo | **si'** |
| **l'indicatore compilato** — la DLL che ATAS carica, cioe' i livelli sul chart e il pannello | `%APPDATA%\ATAS\Indicators` (Windows), `~/Library/Application Support/ATAS/Indicators` (macOS) | **no**: `bin/` e `obj/` sono in `.gitignore`, nel repo c'e' il **sorgente**, non il binario |
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

`deploy.sh` e `Indicators/Directory.Build.props` scelgono da soli il sistema: gli assembly ATAS
vengono da `/Applications/ATAS X.app/Contents/MonoBundle` su macOS e da
`C:\Program Files (x86)\ATAS Platform` su Windows; le DLL finiscono in
`~/Library/Application Support/ATAS/Indicators` oppure in `%APPDATA%\ATAS\Indicators`. Non c'e'
niente da cambiare a mano: se la build fallisce, il primo sospetto e' che ATAS non sia installato
in quel percorso, non che lo script sia sbagliato.

## Cosa Va Rifatto A Mano, Perche' Non Puo' Viaggiare

- **La memoria dell'agente.** Si copia la cartella `memory/` da
  `~/.claude/projects/<vecchio-percorso>/memory/` a
  `~/.claude/projects/<nuovo-percorso>/memory/`. Il nome della cartella e' derivato dal percorso
  del progetto, quindi sulla macchina nuova e' diverso: si guarda quale esiste dopo aver aperto
  l'agente una volta nel repo.
- **`.mcp.json`.** Punta all'installazione locale di `playwright-mcp`. Va reinstallato e il
  percorso riscritto: non si puo' sapere in anticipo dove finira' `npm` sulla macchina nuova.
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
3. **`avviso.py` su Windows.** Il ramo che fa la notifica di sistema con suono e' scritto ma **mai
   eseguito su Windows**: usa un balloon tip via PowerShell/WinForms al posto di `osascript`. Se
   non compare niente, non e' un blocco — `~/.fabio-avvisi.log` resta la fonte di verita' — ma va
   sistemato, perche' quella notifica e' l'unica che si vede **senza guardare**.
4. **Il bridge.** `python3 FabioOrderFlow/tools/bridge.py health --chart NQZ6`. Su Windows il
   comando potrebbe essere `python` invece di `python3`.

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
