# Da Dove Si Comincia: orderflow-atas

Studio del corso di order flow di Fabio 'Fabervaale' Valentini, con verifica sui dati reali di ATAS.

Il repository non contiene un sistema di trading. Contiene un **metodo**: partire da cio' che il
corso insegna, isolare quali affermazioni sono osservabili, misurarle, e registrare tanto cio' che
regge quanto cio' che non regge. Nessuna regola operativa e' stata approvata.

## Le Tre Decisioni Che Governano Tutto

**Il corso comanda.** Ogni operazione, ogni livello, ogni condizione deve poter essere ricondotta a
un minuto di una lezione del live Q1 in
[`fabio_course/fabio_q1/`](fabio_course/fabio_q1/). Il modello **IVB / Triple AAA** del dossier
illustrato resta citabile — e' di Fabio e va menzionato — ma **non comanda**.

**Lo strumento e' NQ, e solo NQ.** Con la conseguenza che la sessione operativa e' **New York**,
perche' e' quello che il corso dice e il corso comanda.

**L'agente costruisce il contesto, non opera** (20 settembre 2026). Non prende decisioni
operative e non arma condizioni: tiene aggiornato tutto il contesto necessario perche' la
decisione la prenda una persona.

## Da Dove Si Comincia

| Serve | File |
|---|---|
| **Il metodo che comanda**: i tre modelli, il vocabolario, il regime, la sessione, il COT, il framing | [`docs/research/metodo/i-tre-modelli-del-live-q1.md`](docs/research/metodo/i-tre-modelli-del-live-q1.md) |
| **I setup**: la libreria dei pattern, ingresso, stop, bersaglio | [`docs/research/metodo/i-pattern-di-esecuzione.md`](docs/research/metodo/i-pattern-di-esecuzione.md) |
| **La gestione**: break even, parziali, conto in R, size. Nel live e' l'edge | [`docs/research/metodo/la-gestione-della-posizione.md`](docs/research/metodo/la-gestione-della-posizione.md) |
| **Come si e' arrivati qui**: le strade chiuse, il COT, il Data Bridge | [`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md) |
| **Come si fa** una analisi, procedura per procedura | [`docs/research/metodo/indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) |
| **Cosa si guarda prima di rispondere**: il giro d'orizzonte e il quadro | [`docs/research/metodo/il-giro-d-orizzonte.md`](docs/research/metodo/il-giro-d-orizzonte.md) |
| **Come si risponde dal vivo**: direzione, forma, livelli come porte, termini | [`docs/research/metodo/come-si-risponde-dal-vivo.md`](docs/research/metodo/come-si-risponde-dal-vivo.md) |
| **Chi calcola i livelli** e con quale vocabolario | [`docs/research/metodo/i-livelli-li-calcola-l-indicatore.md`](docs/research/metodo/i-livelli-li-calcola-l-indicatore.md) |
| **Build e deploy**, e cosa un `git pull` non porta | [`docs/research/build-e-deploy.md`](docs/research/build-e-deploy.md) |
| **Cosa e' gia' stato analizzato**, giorno per giorno | [`docs/research/giornate/`](docs/research/giornate/) |
| Le regole operative per lavorare nel repository | [`CLAUDE.md`](CLAUDE.md) |
| Quali fonti esistono e quale batte quale | [`fabio_course/mappa-delle-fonti.md`](fabio_course/mappa-delle-fonti.md) |

## Le Fasi Chiuse

La fase sistematica sul **modello 40R** e' stata misurata su un mese di NQ e **chiusa**: 49-51% con
obiettivo 1:1, indistinguibile dal modello nullo che entra senza nessuna condizione. Conservata come
evidenza in [`docs/research/archivio-modello-40r/`](docs/research/archivio-modello-40r/), non va
riaperta senza richiesta esplicita.

I **recorder di agosto 2026** stanno in
[`docs/research/archivio-2026-08/`](docs/research/archivio-2026-08/).

Gli **strumenti diversi da NQ** — oro (GCZ6, MGCZ6), crude (MCLV6), ES — restano in
`docs/research/giornate/` come evidenza di giornate reali e di errori reali. Non si estendono.

## Struttura

```text
fabio_course/fabio_q1/     le trascrizioni del live Q1  <- la fonte che comanda
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
# build e deploy delle estensioni ATAS
cd FabioOrderFlow/src && ./deploy.sh

# caricare Fabio Data Bridge su un chart, poi
python3 FabioOrderFlow/tools/bridge.py health
```
