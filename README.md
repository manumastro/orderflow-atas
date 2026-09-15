# orderflow-atas

Studio del corso di order flow di Fabio, con verifica sui dati reali di ATAS.

Il repository non contiene un sistema di trading. Contiene un **metodo**: partire da cio' che il
corso insegna, isolare quali affermazioni sono osservabili, misurarle, e registrare tanto cio' che
regge quanto cio' che non regge. Nessuna regola operativa e' stata approvata.

## Da Dove Si Comincia

**[`docs/research/percorso-del-progetto.md`](docs/research/percorso-del-progetto.md)** — il percorso completo, dall'inizio a
oggi: il corso, le due strade chiuse e perche', il contesto istituzionale COT, il Data Bridge, e
cosa si fa adesso. E' il documento che ricostruisce come si e' arrivati all'analisi attuale, ed e'
la prima lettura per chiunque, persona o agente.

Da li' si arriva a tutto il resto:

| Serve | File |
|---|---|
| **Come si fa** una analisi, procedura per procedura | [`docs/research/metodo/indice-delle-procedure.md`](docs/research/metodo/indice-delle-procedure.md) |
| **Cosa e' gia' stato analizzato**, giorno per giorno | [`docs/research/giornate/`](docs/research/giornate/) |
| Le regole operative per lavorare nel repository | [`CLAUDE.md`](CLAUDE.md) |

## Lo Scope Attuale

Dal **15 settembre 2026**: la lettura **discrezionale** descritta nel primo live del corso
(`fabio_course/fabio_q1/`), letta insieme al contesto istituzionale. Il modello che ne sta dietro e'
il dossier illustrato in [`fabio_course/ivbaaa/`](fabio_course/ivbaaa/), trascritto per intero in
[`triple-aaa-dossier.md`](docs/research/metodo/triple-aaa-dossier.md).

La fase sistematica sul modello 40R e' stata misurata su un mese di NQ e **chiusa**: 49-51% con
obiettivo 1:1, indistinguibile dal modello nullo che entra senza nessuna condizione. E' conservata
come evidenza in [`docs/research/archivio-modello-40r/`](docs/research/archivio-modello-40r/) e non
va riaperta senza una richiesta esplicita.

## Struttura

```text
fabio_course/              trascrizioni delle lezioni, mappa del corso, dossier del modello
docs/research/             il percorso, il metodo e tutte le analisi  <- percorso-del-progetto.md
docs/research/metodo/      le procedure attive
docs/research/giornate/    una analisi per giornata di mercato, AAAA-MM-GG.md
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
