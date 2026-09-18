# Il Percorso, Dall'Inizio A Oggi

Questo file esiste per una ragione sola: **rendere ricostruibile come si e' arrivati all'analisi
che facciamo oggi**, senza doverlo dedurre dai file o dalla conversazione.

Va letto per intero prima di analizzare il mercato. Non contiene procedure — quelle stanno in
[`metodo/`](metodo/) — contiene la catena delle decisioni e il motivo di ciascuna.

---

## 1. Da Dove Nasce Tutto: Il Corso

La fonte e' il corso di order flow di Fabio 'Fabervaale' Valentini, in
[`../../fabio_course/`](../../fabio_course/).

**Dal 18 settembre 2026 il corso comanda.** Ogni operazione, ogni livello, ogni condizione deve
poter essere ricondotta a un minuto di una lezione del **live Q1** in `fabio_course/fabio_q1/`. Le
sei lezioni disponibili sono estratte, con il minuto citato riga per riga, in tre documenti:

- [`metodo/i-tre-modelli-del-live-q1.md`](metodo/i-tre-modelli-del-live-q1.md) — i tre modelli, il
  vocabolario, il regime, la sessione, il COT, il framing;
- [`metodo/i-pattern-di-esecuzione.md`](metodo/i-pattern-di-esecuzione.md) — la libreria dei setup;
- [`metodo/la-gestione-della-posizione.md`](metodo/la-gestione-della-posizione.md) — la gestione,
  che nel live **e' l'edge**.

**Il dossier illustrato** in `fabio_course/ivbaaa/`, trascritto in
[`metodo/triple-aaa-dossier.md`](metodo/triple-aaa-dossier.md), e' stato la fonte principale fino al
18 settembre. Ora e' **riferimento**: e' di Fabio e va menzionato, ma descrive lo stesso setup dentro
un'impalcatura a tre tier con un **gate orario che nel live non esiste**.

Le tre lezioni piu' vecchie (`fabio1.txt`, `fabio2.txt`, `fabio3.txt`) e la mappa
[`fabio-course-model-map.md`](../../fabio_course/fabio-course-model-map.md) restano come contesto, e
sono anteriori al live Q1: dove lo contraddicono, hanno torto.

**Lo strumento e' NQ e la sessione e' New York**, entrambi perche' lo dice il corso
(`fabio_3.txt`, 1:06:37 e 22:12) e perche' l'utente l'ha deciso il 18 settembre 2026.

La regola che governa tutto il resto, dichiarata fin dall'inizio: **una tecnica citata in una
lezione non diventa una specifica finche' non si e' stabilito quale parte di essa e' misurabile.**
Descrivere non e' validare.

## 2. La Prima Strada, E Perche' E' Stata Chiusa

Agosto 2026: quattro **recorder** che catturano il tape secondo uno schema fissato prima. Il limite
e' emerso con l'uso — lo schema si decide prima della cattura, quindi ogni domanda nuova richiede
una cattura nuova. Materiale in [`archivio-2026-08/`](archivio-2026-08/).

Poi il **modello sistematico 40R**, implementato dal dossier e misurato su un mese di NQ. **Chiuso
il 15 settembre 2026**: 49-51% con obiettivo 1:1, indistinguibile dal modello nullo che entra senza
condizioni. Materiale e diagnosi in [`archivio-modello-40r/`](archivio-modello-40r/).

**Quella fase non va riaperta senza richiesta esplicita.** La lezione che ne resta e' metodologica e
vale ancora oggi: **le osservazioni dentro una sessione non sono indipendenti**, e un z ingenuo di
+4,1 diventa t=+1,4 quando la statistica si calcola per sessione. Ogni misura futura deve tenerne
conto.

## 3. Il Contesto Istituzionale: Il Primo Dei Due Livelli

Il live introduce il **COT** come contesto settimanale. Lo studio in [`cot/`](cot/) ha stabilito
tre cose:

- **La fonte va disambiguata.** Tradingster espone due report per lo stesso strumento.
  `Non-Commercial` esiste solo nella vista Legacy; `Asset Manager` e `Leveraged Funds` solo nella
  vista TFF. Nel live i nomi sono usati come sinonimi: non lo sono.
- **La raccolta e' riproducibile**, estraendo la serie dal grafico invece di leggerla a occhio.
- **La metrica cross-index e' stata verificata e non regge.** Risultato negativo, registrato allo
  stesso titolo di uno positivo.

Il COT resta **contesto, non trigger**. Ma non scade con la settimana: il metodo di Fabio e' datare
il cambio di posizionamento e tornare al **prezzo** di quella data, che diventa un livello. Tutto
in [`metodo/analisi-istituzionale.md`](metodo/analisi-istituzionale.md).

## 4. Il Data Bridge: Il Secondo Livello, Al Millisecondo

Settembre 2026, la svolta. `Fabio Data Bridge` e' un indicatore caricato su un chart ATAS che
espone i dati della piattaforma su `http://127.0.0.1:8787`.

Ribalta il problema dei recorder: **si chiedono le finestre e i filtri che servono**, mentre ATAS
resta aperto, invece di deciderli prima. Rende accessibile cio' che esiste solo dentro il processo
ATAS: profilo con sessione dichiarata, trade aggregati storici con filtro di volume nativo,
snapshot del book, date di rollover.

Contratto in [`metodo/contratto-data-bridge.md`](metodo/contratto-data-bridge.md). Client:
`FabioOrderFlow/tools/bridge.py`.

## 5. Cosa Si Fa Oggi

Dal **18 settembre 2026**: la **lettura discrezionale del live Q1 su NQ, sessione di New York**,
letta insieme al contesto istituzionale. Non c'e' nessun modello attivo e nessuna regola operativa
approvata.

Una giornata si costruisce cosi':

```text
regime                         ->  direzionale, balance o choppy: decide la size prima del setup
COT non-commercial + framing   ->  il quadro, e la data del cambio che diventa un prezzo
quale dei tre modelli          ->  lo sceglie la posizione del prezzo, non l'umore
i livelli                      ->  solo aggressione massima e assorbimento impilato, da ordini eseguiti
                     sul chart ->  POST /levels, l'indicatore li disegna
             durante la seduta ->  il tape misurato sui livelli, col bridge
                     in posizione -> break even sul livello, parziali, conto in R
                        alla fine -> la giornata scritta, correzioni comprese
```

Due avvertenze che valgono sempre, e che sono costate errori reali:

- **Il contratto continuo di ATAS non e' back-adjusted.** Incolla i contratti lasciando il salto
  del roll. Per il framing servono i contratti singoli o `build_continuous.py`.
- **Il controllo del rollover precede ogni altra misura.** Confrontare il volume dei due contratti
  prima di leggere qualunque flusso.

## 6. Cosa E' Stato Chiuso Il 18 Settembre 2026

Per far comandare il corso e' stato necessario ritirare tre cose, tutte conservate come misura:

- **gli strumenti diversi da NQ** — oro (GCZ6, MGCZ6), crude (MCLV6), ES. I file delle giornate
  restano: contengono errori reali e le regole che ne sono nate.
- **la sessione di Londra** — i numeri su NQZ6 reggono, ma il corso dice New York col motivo.
- **il gate Tier 01 e la deroga per operare prima del gate** — la deroga decade perche' nel live
  **il gate non c'e'**. `permesso_di_fatto.py` misura ancora l'accettazione su M1, che resta utile;
  il confronto col gate M30 non si pone piu'.

---

## Dove Andare, Adesso

| Serve | File |
|---|---|
| **Come si fa** una analisi | [`metodo/indice-delle-procedure.md`](metodo/indice-delle-procedure.md) — l'indice delle procedure, in ordine |
| **Cosa e' gia' stato analizzato** oggi o ieri | [`giornate/`](giornate/) — un file per giornata di mercato |
| Le settimane e le sedute descritte | [`sessioni/`](sessioni/) |
| I dati COT grezzi | [`cot/`](cot/) |
| La storia prima della baseline attuale | [`metodo/storia-del-progetto.md`](metodo/storia-del-progetto.md) |
| Le fasi chiuse, come evidenza | [`archivio-2026-08/`](archivio-2026-08/), [`archivio-modello-40r/`](archivio-modello-40r/) |

## Le Cinque Regole

1. **Prima di misurare, dichiarare.** Finestra, campi e criteri prima della raccolta.
2. **Distinguere livello e flusso, descrizione e regola.** Una co-occorrenza non e' una regolarita'.
3. **Registrare anche i risultati negativi**, con la stessa cura di quelli positivi.
4. **Dichiarare le convenzioni.** Soglie, frazioni e orizzonti sono scelte, non fatti: vanno scritte
   accanto al numero che producono.
5. **Una fonte canonica per decisione.** Ogni fase sostanziale aggiorna il documento pertinente e
   aggiunge una riga datata a [`../../FabioOrderFlow/progress.txt`](../../FabioOrderFlow/progress.txt).
