# Il Percorso, Dall'Inizio A Oggi

Questo file esiste per una ragione sola: **rendere ricostruibile come si e' arrivati all'analisi
che facciamo oggi**, senza doverlo dedurre dai file o dalla conversazione.

Va letto per intero prima di analizzare il mercato. Non contiene procedure — quelle stanno in
[`metodo/`](metodo/) — contiene la catena delle decisioni e il motivo di ciascuna.

---

## 1. Da Dove Nasce Tutto: Il Corso

La fonte e' il corso di order flow di Fabio 'Fabervaale' Valentini, in
[`../../fabio_course/`](../../fabio_course/).

- **Il corso principale e' il live Q1** (`fabio_course/fabio_q1/`). Gli altri live si aggiungeranno.
- Le tre lezioni piu' vecchie (`fabio1.txt`, `fabio2.txt`, `fabio3.txt`) e la mappa
  [`fabio-course-model-map.md`](../../fabio_course/fabio-course-model-map.md) restano come contesto.
- Il **modello** e' nel dossier illustrato in `fabio_course/ivbaaa/`, trascritto per intero in
  [`metodo/triple-aaa-dossier.md`](metodo/triple-aaa-dossier.md).

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

Dal **15 settembre 2026** lo scope e' la **lettura discrezionale** del live Q1, letta insieme al
contesto istituzionale. Non c'e' nessun modello attivo e nessuna regola operativa approvata.

Una giornata si costruisce cosi', ed e' il percorso che i documenti di metodo descrivono:

```text
COT + posizionamento           ->  la zona dove gli istituzionali si sono mossi
profilo delle sedute passate   ->  nodi, colli, vuoti, value area, POC
                     insieme   ->  i livelli del giorno
                    sul chart  ->  POST /levels, l'indicatore li disegna
               durante la seduta -> il tape misurato sui livelli, col bridge
                        alla fine -> la giornata scritta, correzioni comprese
```

Due avvertenze che valgono sempre, e che sono costate errori reali:

- **Il contratto continuo di ATAS non e' back-adjusted.** Incolla i contratti lasciando il salto
  del roll. Per il framing servono i contratti singoli o `build_continuous.py`.
- **Il controllo del rollover precede ogni altra misura.** Confrontare il volume dei due contratti
  prima di leggere qualunque flusso.

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
