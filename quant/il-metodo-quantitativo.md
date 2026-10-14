# Il Metodo Quantitativo: Il Framework

Dall'11 ottobre 2026 lo scope del progetto e' **la metodologia istituzionale**, costruita come
framework dalla A alla Z. Questo documento e' **la mappa**: dove sta cosa, le aggiunte del framework
con il loro perche', come si compila e cosa manca. La metodologia esattamente come da transcript,
fase per fase, sta in [`la-metodologia-istituzionale.md`](metodo/la-metodologia-istituzionale.md),
e **comanda**.

Le decisioni che delimitano il lavoro, prese dall'utente:

- **solo NQ**, per ora;
- **dati solo da ATAS** ([`i-dati-da-atas.md`](metodo/i-dati-da-atas.md));
- **si arriva fino alla validazione**: le fasi 6 e 7 sono fuori scope finche' il framework non e'
  maturo;
- **la velocita' e' un requisito**, per questo il framework e' in **C# .NET 10**, lo stesso
  linguaggio di ATAS.

## Dove Sta Cosa

`quant/` ha cinque aree, e ciascuna risponde a una domanda:

```text
quant/
├── il-metodo-quantitativo.md     questa mappa
├── metodo/      COME si fa      la metodologia, il transcript, una pagina per fase
├── framework/   CON COSA        il codice C# riusabile: un progetto per livello
├── paper/       DA DOVE         la biblioteca: un filone per domanda, una cartella per paper
├── studi/       COSA            le idee, una cartella ciascuna, dalla A alla Z
├── registro/                    le prove fatte e la divisione dentro/fuori (in git)
└── dati/                        l'archivio delle barre di ATAS (fuori da git)
```

**Uno studio segue la pipeline dentro la sua cartella**, con le fasi numerate come nella
metodologia. Si crea solo cio' che serve:

```text
studi/<idea>/
├── 1-idea.md            la scheda: paper, finding, perche', cosa la ucciderebbe, varianti
├── 2-regole/            le regole scritte, se non bastano i riquadri della scheda (per esempio
│                        le regole di una prop firm, con le fonti)
├── 3-codice/            il codice dell'idea: la strategia, il contratto
├── 4-backtest/          i rapporti delle prove, se servono a parte
└── 5-validazione/
    ├── criteri/         i criteri sigillati con un hash, e il loro esito
    └── risultati/       i rapporti di `quant valida` e gli studi scritti
```

**Il metodo**, una pagina per fase, con lo stesso numero:

| | fase | come si fa |
|---|---|---|
| 0 | [la differenza](metodo/la-metodologia-istituzionale.md#0-la-differenza-fra-retail-e-istituzionale) | [cosa vuol dire validata](metodo/0-cosa-vuol-dire-validata.md) |
| 1 | [l'idea](metodo/la-metodologia-istituzionale.md#1-lidea) | [come si cercano le idee](metodo/1-come-si-cercano-le-idee.md), [come si scrive un'idea](metodo/1-come-si-scrive-un-idea.md) |
| 2 | [le regole](metodo/la-metodologia-istituzionale.md#2-le-regole) | [come si scrivono le regole](metodo/2-come-si-scrivono-le-regole.md) |
| 3 | [il codice](metodo/la-metodologia-istituzionale.md#3-il-codice) | [come si scrive una strategia](metodo/3-come-si-scrive-una-strategia.md) |
| 4 | [il backtest](metodo/la-metodologia-istituzionale.md#4-il-backtest) | [il backtest](metodo/4-il-backtest.md), [il simulatore di contratto](metodo/4-il-simulatore-di-contratto.md) |
| 5 | [la validazione](metodo/la-metodologia-istituzionale.md#5-la-validazione) | [come si valida](metodo/5-come-si-valida.md), [il framework si valida prima](metodo/5-il-framework-si-valida-prima.md) |
| 6 | [il vivo](metodo/la-metodologia-istituzionale.md#6-il-vivo) | [il vivo](metodo/6-il-vivo.md): fuori scope |
| 7 | [il ciclo e il portafoglio](metodo/la-metodologia-istituzionale.md#7-il-ciclo-e-il-portafoglio) | [il ciclo e il portafoglio](metodo/7-il-ciclo-e-il-portafoglio.md): fuori scope |
| | i dati | [i dati da ATAS](metodo/i-dati-da-atas.md) |
| | le opzioni | [le opzioni in ATAS](metodo/le-opzioni-in-atas.md): gli indicatori, cosa dice Fabio, perche' non c'e' ancora un backtest |

**Gli studi** oggi:

| studio | cosa | stato |
|---|---|---|
| [`contratto-prop`](studi/contratto-prop/1-idea.md) | il valore del contratto di una prop firm (tre paper); Tradeify Growth 50K | studiato fino alla validazione del simulatore |
| [`politica-per-fase`](studi/politica-per-fase/1-idea.md) | l'incrocio dei tre paper: grande in valutazione, piccolo o con un bracket nel finanziato | criteri sigillati: K1 e K4 smentiti, K2 e K3 reggono; l'incrocio non regge come scritto |
| [`momento-intraday`](studi/momento-intraday/1-idea.md) | esempio 1 del transcript | codificato, in attesa della storia lunga |
| [`vwap-tendenza`](studi/vwap-tendenza/1-idea.md) | esempio 2 del transcript | codificato, in attesa della storia lunga |
| [`anomalia-notte`](studi/anomalia-notte/1-idea.md) | esempio 4 del transcript | codificato, in attesa della storia lunga |

La PEAD (esempio 3) non c'e': lavora su azioni, utili e consenso degli analisti, e lo scope e' NQ.

**I paper**: [`paper/indice-dei-paper.md`](paper/indice-dei-paper.md), per filone. Paper e studi sono
separati perche' non si corrispondono uno a uno: tre paper fanno lo studio `contratto-prop`, e un paper
puo' servire a piu' studi.

## Il Codice

**Il framework** e' in `framework/`, nella forma standard di .NET: ogni progetto e' una cartella con
i suoi sorgenti. I progetti seguono le fasi della pipeline, e **le dipendenze vanno in un verso
solo**: un progetto usa quelli prima, mai quelli dopo, e lo fa rispettare il compilatore.

```text
progetto            cosa contiene                                   namespace                       dipende da
Quant.Comune        i dati di mercato, il calcolo                   Quant.Dati, Quant.Calcolo       niente
Quant.Regole        Ordine, Dimensione, Parametri, Contratto  (2)   Quant.Regole, .Contratti        Comune
Quant.Strategie     Strategia, Contesto, Posizione            (3)   Quant.Strategie                 Regole
Quant.Motore        Backtest, Metriche, simulatore contratto  (4)   Quant.Motore, .Contratti        Strategie
Quant.Validazione   nulli, Monte Carlo, cancelli, rapporto    (5)   Quant.Validazione, .Contratti   Motore
Quant.Studi         il codice degli studi e i cataloghi             Quant.Studi                     Strategie
quant (Quant.Cli)   la riga di comando                              -                               Validazione, Studi
Quant.Test          i test                                          Quant.Test                      Validazione, Studi
```

- **Se un pezzo ha bisogno di una fase successiva, e' nel posto sbagliato.** Cosi' si e' deciso dove
  stanno i pezzi di confine: `Parametri` nelle regole (i parametri sono parte delle regole),
  `Posizione` nelle strategie (e' cio' che la strategia legge), i dati e il generatore casuale nella
  base comune (li usano tutti).
- **Il codice delle idee sta negli studi**, `studi/<idea>/3-codice/`, un file per strategia o per
  contratto. Lo compila il progetto `Quant.Studi`, che lo raccoglie da li' e lo registra nei due
  cataloghi (`Catalogo` per le strategie, `CatalogoContratti` per i contratti). Il framework non sa
  niente delle idee.
- Ogni progetto dichiara una volta sola, nel suo `.csproj`, i namespace che usa: i sorgenti non hanno
  `using Quant...`. `InternalsVisibleTo` apre gli interni solo a chi ne ha bisogno (il `Contesto` al
  motore) e ai test.

## Le Aggiunte, E Perche'

Il transcript non tratta alcune cose che in questo repository sono gia' costate care, nella fase
40R ([`fase-sistematica-40r-chiusa.md`](../docs/research/archivio-modello-40r/fase-sistematica-40r-chiusa.md)).
L'elenco di cio' che il transcript non dice sta in fondo a
[`la-metodologia-istituzionale.md`](metodo/la-metodologia-istituzionale.md#cosa-il-transcript-non-dice).
Ogni aggiunta e' spiegata nel documento della sua fase.

| aggiunta | perche' | fase |
|---|---|---|
| **la scheda scrive cosa ucciderebbe la strategia e le varianti, prima di provarle** | il perche' strutturale `[MC · 46:23]` dice anche quando smette di funzionare; le varianti contano | [1](metodo/1-come-si-scrive-un-idea.md) |
| **la strategia vede solo il passato** | il look-ahead e' stato il primo errore della fase 40R | [3](metodo/3-come-si-scrive-una-strategia.md#il-contesto-solo-il-passato) |
| **convenzioni di esecuzione conservative**, stampate accanto al numero | nella fase 40R un limite si eseguiva al tocco, cioe' a mercato | [4](metodo/4-il-backtest.md#le-convenzioni-di-esecuzione) |
| **statistiche per giornata**: il t si calcola sul netto giornaliero | le operazioni della stessa giornata non sono indipendenti: uno z ingenuo di +4,1 e' diventato t = +1,4 | [4](metodo/4-il-backtest.md#le-statistiche) |
| **il modello nullo** | nella fase 40R le condizioni facevano 49% e il nullo 47%: senza confronto il 49% sembrava un risultato | [5](metodo/5-come-si-valida.md#il-modello-nullo) |
| **la divisione si fissa una volta** e non si sposta | ricalcolata a ogni download, porterebbe pezzi di fuori campione dentro | [5](metodo/5-come-si-valida.md#5a-dentro-e-fuori-campione) |
| **il fuori campione si consuma**, e il registro sa se un'altra variante l'ha gia' visto | il fuori campione guardato due volte e' diventato dentro | [5](metodo/5-come-si-valida.md#5a-dentro-e-fuori-campione) |
| **il registro delle prove** | per contare le varianti provate | [5](metodo/5-come-si-valida.md#5a-dentro-e-fuori-campione) |
| **lo Sharpe deflazionato** (Bailey e Lopez de Prado, 2014) | provando cento varianti, la migliore ha uno Sharpe alto anche se nessuna vale niente | [5](metodo/5-come-si-valida.md#lo-sharpe-deflazionato) |
| **il ricampionamento per giornata**, a blocchi di 5 | pescare operazioni una per una sottostima le code | [5](metodo/5-come-si-valida.md#5b-il-monte-carlo) |
| **i cancelli, con soglie** | il transcript dice "buoni risultati dentro e fuori": serve un criterio scritto prima di guardare | [5](metodo/5-come-si-valida.md#i-cancelli) |
| **il framework si valida prima delle strategie**, sul mercato sintetico | un nullo che sbirciava guadagnava +447.000 $ su una passeggiata casuale | [5](metodo/5-il-framework-si-valida-prima.md) |
| **i criteri sigillati** con un hash prima di guardare i dati (Tomas Fernandez, 2026) | un criterio scritto dopo aver visto il risultato non e' una prova; nel primo uso due criteri su quattro erano scritti male, e si e' visto | [5](metodo/5-come-si-valida.md#i-criteri-sigillati) |
| **il simulatore di contratto** della prop firm | l'idea dei tre paper sulle prop firm; i soldi veri del compratore, non il saldo simulato | [4](metodo/4-il-simulatore-di-contratto.md) |

## Come Si Compila E Si Usa

```bash
cd quant/framework
dotnet build -c Release                  # una volta, e dopo ogni modifica
dotnet test Quant.Test -c Release        # i test di tutte le fasi
Q=Quant.Cli/bin/Release/net10.0/quant.exe

$Q chart                                 # i chart aperti in ATAS
$Q scarica --chart <id> --simbolo NQ     # fase 4: le barre del chart in archivio
$Q dati                                  # cosa c'e' in archivio
$Q strategie                             # fase 3: il catalogo, con regole e parametri
$Q prova momento-intraday --p rr=2       # fase 4: backtest dentro campione, nel registro
$Q divisione --simbolo NQ                # fase 5a: il confine dentro/fuori; --fissa, una volta
$Q valida momento-intraday               # fase 5 dentro: nulli, Monte Carlo, Sharpe deflazionato
$Q valida momento-intraday --fuori       # fase 5a: una volta, a regole congelate
$Q contratto verifica                    # il simulatore di contratto contro i criteri sigillati
$Q contratto tradeify-growth-50k --d 850 # il valore di un contratto di prop firm
```

Opzioni comuni: `--simbolo NQ|SINT`, `--tipo m1`, `--p chiave=valore` (ripetibile),
`--commissione 2.5`, `--slittamento 1`, `--repliche 1000`, `--simulazioni 10000`. Un parametro
sconosciuto e' un errore: quasi sempre e' un refuso.

## La Velocita'

Misurata l'11 ottobre 2026 su questo PC, Release:

| operazione | tempo |
|---|---|
| generare 2,07 milioni di barre M1 sintetiche (1.500 giornate) | 2,5 s |
| scaricare da ATAS 41.142 barre M1 | 2,9 s |
| `valida --fuori` completa su 2,07 milioni di barre: 2 backtest, 4 nulli da 1.000 repliche, 3 Monte Carlo da 10.000 | 0,9 - 2,6 s |
| 47 test unitari | 0,4 s |
| `contratto verifica`: 18 righe da 200.000 tentativi | 21,6 s |
| un contratto, 100.000 cicli browniani da 78 controlli per sessione | 1,8 s |

## Cosa Manca

- **La storia lunga di NQ.** Quanta ne da' ATAS lo dice il tariffario, e si misura appena la
  connessione torna: [`i-dati-da-atas.md`](metodo/i-dati-da-atas.md).
- **Tick e book nel motore.** Il motore lavora su barre; con i tick l'ambiguita' stop/target si
  risolve esattamente invece che al peggio.
- **Il confronto appaiato fra varianti**, per le anomalie di esposizione.
- **Il contratto applicato alle operazioni di una strategia del catalogo**: oggi il simulatore
  di contratto prende sessioni browniane o storiche con il verso a caso; il passo dopo e' dargli le
  operazioni di una prova (`Backtest`), quando una strategia avra' passato i suoi cancelli.
- **Il sigillo dei cancelli** per ogni strategia: oggi le soglie sono scritte prima, ma non
  sigillate; i criteri del simulatore di contratto lo sono.
- **Le fasi 6 e 7**: il vivo e il portafoglio, fuori scope finche' il framework non e' maturo.
