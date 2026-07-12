# New York Impulse Boundary Risk - Contratto 2026-07-12

## Domanda

Dopo la conferma cumulative, il prezzo raggiunge prima il vecchio estremo dell'impulso oppure torna al confine da cui l'impulso era partito?

Questo sostituisce la decisione a durata fissa di 15 minuti. Le letture a 5, 15 e 30 minuti restano descrittive, ma non decidono piu' il candidato.

## Termini in parole comuni

- **B**: estremo raggiunto dall'impulso prima del pullback.
- **A**: confine di origine; se il prezzo torna qui, l'idea di continuation non e' piu' valida.
- **R**: unita' di rischio. `+1R` significa guadagnare quanto si era disposti a perdere; `-1R` significa perdere l'intero rischio iniziale.
- **Profit factor**: somma dei risultati positivi divisa per la somma assoluta dei risultati negativi.
- **Caso ambiguo**: obiettivo e invalidazione vengono toccati nella stessa candela M1, quindi non conosciamo quale sia avvenuto prima.

## Contratto congelato

```text
Nome:          NY_IMPULSE_CONFIRMATION_BOUNDARY_RISK_V1
Timeframe:     M1
Conferma:      NY_IMPULSE_LVN_CUMULATIVE_CONFIRMATION_V1
Prezzo start:  close della barra di conferma
Obiettivo:     estremo B congelato prima del pullback
Invalidazione: confine di origine A
```

Dopo la barra di conferma:

```text
B toccato prima di A: TARGET_B_FIRST
A toccato prima di B: ORIGIN_A_FIRST
B e A nella stessa barra: AMBIGUOUS_SAME_BAR_AS_LOSS
Nessuno entro fine sessione: SESSION_CLOSE_NO_BOUNDARY
```

Il caso ambiguo conta come perdita. Non si usa una ricostruzione favorevole dell'ordine intrabar.

## Risultato

```text
Vittoria: rewardPoints / riskPoints
Perdita:  -1R
Fine sessione: movimento direzionale / riskPoints
```

Non viene calcolato PnL monetario. Il report sottrae costi complessivi simulati di `0,5`, `1,0` e `1,5` punti. Questi scenari rappresentano insieme commissioni e differenza tra prezzo teorico e prezzo ottenibile; non affermano quale sia il costo reale del broker.

## Controllo iniziale

Il campione corrente puo' essere valutato quando contiene almeno otto casi LONG e otto SHORT.

Con costo complessivo di `1 punto`, devono essere tutti veri:

```text
risultato medio > 0 complessivo
risultato medio > 0 LONG
risultato medio > 0 SHORT
profit factor >= 1,25 complessivo
profit factor >= 1,25 LONG
profit factor >= 1,25 SHORT
casi ambigui <= 10%
nessuna geometria non valida
```

Esiti:

```text
REJECTED_HISTORICAL_FEASIBILITY
- fallisce almeno un controllo;
- nessuna estensione storica o raccolta prospettica.

EXTEND_HISTORICAL_HOLDOUT
- supera tutti i controlli iniziali;
- congelare il contratto e ampliare il chart verso il passato.
```

## Controllo storico esteso

Se il controllo iniziale passa, il successivo campione deve contenere almeno:

```text
30 osservazioni totali
10 LONG
10 SHORT
```

Le date aggiunte non possono modificare entry, obiettivo, invalidazione, costi o criteri. Lo storico esteso non e' ancora prospettico, ma evita di continuare a giudicare soltanto le stesse 40 sessioni usate nella discovery.

## Vincoli

```text
OperationalEntry=FALSE
OrderSubmitted=FALSE
PnLComputed=FALSE
Validated=FALSE
selectionLeakage=true
```
