# Da Dove Si Comincia: orderflow-atas

Dall'11 ottobre 2026: **il metodo quantitativo istituzionale**, costruito come framework dalla A
alla Z, su NQ e con i dati di ATAS. Prima di allora, lo studio del corso di order flow di Fabio
'Fabervaale' Valentini, che resta come fonte di idee e come supporto alla lettura dal vivo.

Il repository non contiene un sistema di trading. Contiene un **metodo**: un'idea presa da un paper
diventa regole, codice, backtest e validazione, e si registra tanto cio' che regge quanto cio' che
non regge. Nessuna regola operativa e' stata approvata.

## Le Tre Decisioni Che Governano Tutto

**Lo scope e' il metodo quantitativo.** Idea, regole, codice, backtest, validazione, dal transcript
di Matteo Conte in [`quant/fonti/`](quant/fonti/). Il corso di Fabio non comanda piu': e' una fonte
di idee.

**Lo strumento e' NQ, e solo NQ; i dati vengono solo da ATAS.**

**Si arriva fino alla validazione.** Niente ordini, niente esecuzione automatica, finche' il
framework non e' maturo e l'utente non lo chiede. La velocita' e' un requisito: il framework e' in
C# .NET 10.

## Da Dove Si Comincia

| Serve | File |
|---|---|
| **La metodologia istituzionale**, esattamente come da transcript, fase per fase | [`quant/la-metodologia-istituzionale.md`](quant/la-metodologia-istituzionale.md) |
| **Il framework**: come realizza ogni fase, le aggiunte, i cancelli, come si usa | [`quant/il-metodo-quantitativo.md`](quant/il-metodo-quantitativo.md) |
| **I dati da ATAS**: cosa da', fin dove, cosa e' collegato, il piano | [`quant/i-dati-da-atas.md`](quant/i-dati-da-atas.md) |
| **Le idee**: una scheda per strategia, e come si scrive | [`quant/idee/come-si-scrive-un-idea.md`](quant/idee/come-si-scrive-un-idea.md) |
| **I rapporti** delle validazioni, e il registro delle prove | [`quant/risultati/`](quant/risultati/), [`quant/registro/`](quant/registro/) |
| *La lettura discrezionale, supporto:* **gli obblighi** quando si chiede una lettura dal vivo | [`docs/research/metodo/gli-obblighi-della-lettura-dal-vivo.md`](docs/research/metodo/gli-obblighi-della-lettura-dal-vivo.md) |
| **Il metodo che comanda**: i tre modelli, il vocabolario, il regime, la sessione, il COT, il framing | [`docs/research/metodo/i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md) |
| **I setup**: la libreria dei pattern, ingresso, stop, bersaglio | [`docs/research/metodo/i-pattern-di-esecuzione.md`](docs/research/metodo/i-pattern-di-esecuzione.md) |
| **La gestione**: break even, parziali, conto in R, size. Nel live e' l'edge | [`docs/research/metodo/la-gestione-della-posizione.md`](docs/research/metodo/la-gestione-della-posizione.md) |
| **Come si e' arrivati qui**: le strade chiuse, il COT, il Data Bridge | [`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) |
| **Come si fa** una analisi, procedura per procedura | [`docs/research/metodo/indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) |
| **Cosa si guarda prima di rispondere**: il giro d'orizzonte e il quadro | [`docs/research/metodo/il-giro-d-orizzonte.md`](docs/research/metodo/il-giro-d-orizzonte.md) |
| **Come si risponde dal vivo**: direzione, forma, livelli come porte, termini | [`docs/research/metodo/come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md) |
| **Come l'agente legge e comanda ATAS**: dati, interfaccia, file del workspace, foto delle tab | [`docs/research/metodo/come-l-agente-vede-e-comanda-atas.md`](docs/research/metodo/come-l-agente-vede-e-comanda-atas.md) |
| **Il workspace ATAS**: quattro tab, cosa mostra ciascuna e perche' | [`docs/research/metodo/il-workspace-a-tab.md`](docs/research/metodo/il-workspace-a-tab.md) |
| **Chi calcola i livelli** e con quale vocabolario | [`docs/research/metodo/i-livelli-li-calcola-l-indicatore.md`](docs/research/metodo/i-livelli-li-calcola-l-indicatore.md) |
| **Build e deploy**, e cosa un `git pull` non porta | [`docs/research/build-e-deploy.md`](docs/research/build-e-deploy.md) |
| **Cosa e' gia' stato analizzato**, giorno per giorno | [`docs/research/giornate/`](docs/research/giornate/) |
| Le regole operative per lavorare nel repository | [`CLAUDE.md`](CLAUDE.md) |
| Le regole che valgono solo su una parte del repo, e si caricano da sole quando la si tocca | [`.claude/rules/`](.claude/rules/) |
| Quali fonti esistono e quale batte quale | [`fabio_course/mappa-delle-fonti.md`](fabio_course/mappa-delle-fonti.md) |

## Le Fasi Chiuse

La fase sistematica sul **modello 40R** e' stata misurata su un mese di NQ e **chiusa**: 49-51% con
obiettivo 1:1, indistinguibile dal modello nullo che entra senza nessuna condizione. Conservata come
evidenza in [`docs/research/archivio-modello-40r/`](docs/research/archivio-modello-40r/), non va
riaperta senza richiesta esplicita. Le sue lezioni di metodo (il nullo, la statistica per giornata,
il look-ahead, i limiti eseguiti al tocco) sono dentro il framework quantitativo.

I **recorder di agosto 2026** stanno in
[`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/).

Gli **strumenti diversi da NQ** — oro (GCZ6, MGCZ6), crude (MCLV6), ES — restano in
`docs/research/giornate/` come evidenza di giornate reali e di errori reali. Non si estendono.

## Struttura

```text
quant/                     il metodo quantitativo        <- lo scope
quant/fonti/               transcript e paper
quant/idee/                una scheda per strategia
quant/src/                 il framework in C#: dati, motore, statistiche, validazione, strategie
quant/registro/            le prove e la divisione dentro/fuori campione, in git
quant/risultati/           i rapporti delle validazioni
quant/dati/                l'archivio dei dati di ATAS, fuori da git
fabio_course/fabio_q1/     le trascrizioni del live Q1  <- fonte di idee, comanda la lettura dal vivo
fabio_course/ivbaaa/       il dossier illustrato        <- riferimento
docs/research/metodo/      le procedure attive
docs/research/giornate/    una analisi per giornata di mercato
docs/research/sessioni/    profili di sessione e descrizioni settimanali
docs/research/cot/         contesto istituzionale: COT e Tradingster
docs/research/archivio-*/  le fasi chiuse, conservate come evidenza
docs/atas/                 documentazione tecnica dell'API ATAS
FabioOrderFlow/            le estensioni ATAS e gli strumenti di analisi
prop/                      esecuzione: mercato guida, sizing, fee, regole delle prop
```

## Avvio Rapido

```bash
# il framework quantitativo
cd quant/src && dotnet build -c Release && dotnet test Quant.Test -c Release
Quant.Cli/bin/Release/net10.0/quant.exe aiuto

# build e deploy delle estensioni ATAS
cd FabioOrderFlow/src && ./deploy.sh

# caricare Fabio Data Bridge su un chart, poi
python3 FabioOrderFlow/tools/bridge.py health
```
