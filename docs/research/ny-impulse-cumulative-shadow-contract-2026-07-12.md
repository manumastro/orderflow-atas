# New York Impulse Cumulative Shadow - Contratto 2026-07-12

## Fine

Capire se una precisa risposta degli acquisti o delle vendite durante il pullback New York merita di diventare, in futuro, un modello simulato con costi realistici.

Questa fase non apre ordini e non calcola profitto. Registra soltanto cosa succede dopo una conferma definita in anticipo. E' abilitata esclusivamente sul grafico M1, cioe' con una candela per minuto; su M5 resta disattivata per non mescolare test diversi.

## Termini in parole comuni

- **Impulso A->B**: movimento iniziale che porta il prezzo fuori dalla value area precedente.
- **Pullback**: ritorno parziale del prezzo dopo l'impulso.
- **LVN, Low Volume Node**: prezzo del profilo A->B dove e' passato meno volume rispetto ai prezzi vicini.
- **Cumulative trade**: gruppo di eseguiti che ATAS attribuisce allo stesso compratore o venditore aggressivo.
- **Shadow**: osservazione simulata; non e' un ordine e non puo' produrre PnL.
- **MFE, massima escursione favorevole**: punto piu' lontano raggiunto dal prezzo nella direzione attesa.
- **MAE, massima escursione contraria**: punto piu' lontano raggiunto dal prezzo contro la direzione attesa.
- **Prospettico**: dato di una sessione successiva al congelamento delle regole, quindi non usato per costruirle.

## Candidato congelato

Nome tecnico:

```text
NY_IMPULSE_CUMULATIVE_CONFIRMATION_SHADOW_V1
```

La shadow nasce quando, prima che l'impulso sia gia' risolto, una barra pullback soddisfa tutte queste condizioni:

```text
1. attraversa almeno un LVN raw del profilo A->B congelato;
2. la candela e il delta si muovono nella direzione dell'impulso;
3. il cumulative trade massimo nella direzione dell'impulso e' almeno 30;
4. quel massimo e' maggiore del cumulative trade massimo opposto;
5. e' la prima conferma conservata per data New York e direzione.
```

La soglia `30` proviene dal transcript New York di Fabio. Non e' stata scelta cercando il risultato migliore nello storico.

La barra che raggiunge gia' un nuovo estremo o rientra all'origine e' esclusa: in quel momento il risultato sarebbe gia' noto.

## Punto di osservazione

Il prezzo iniziale della shadow e' la chiusura della barra di conferma. Da quel momento il modello registra ogni barra M1 completata fino alla prima che raggiunge 30 minuti.

Per ogni barra registra:

```text
- movimento della chiusura nella direzione attesa;
- massima escursione favorevole in punti;
- massima escursione contraria in punti;
- le stesse misure divise per l'ampiezza dell'impulso;
- disponibilita' e massimi cumulative trade della barra;
- risultato finale dell'impulso: continuation, origin reentry, two-sided o session end.
```

Le letture principali sono fissate a 5, 15 e 30 minuti. Non sono target o stop.

## Separazione temporale

```text
SessionDate <= 2026-07-10: HISTORICAL_REFERENCE
SessionDate >= 2026-07-13: PROSPECTIVE
```

Lo storico serve soltanto per controllare che il software ricostruisca il contratto noto. Solo `PROSPECTIVE` puo' decidere il futuro del candidato.

## Campione decisionale

Il report congela il primo prefisso cronologico che raggiunge:

```text
20 osservazioni prospettiche totali
almeno 8 LONG
almeno 8 SHORT
```

La raccolta ha una durata massima di 40 sessioni New York. Se non raggiunge il campione, il candidato viene scartato per rarita': una regola troppo rara non e' utile al fine operativo.

## Criteri di promozione

Il candidato passa alla simulazione con costi soltanto se soddisfa contemporaneamente:

```text
1. almeno 60% continuation complessive;
2. almeno 50% continuation LONG;
3. almeno 50% continuation SHORT;
4. almeno 15 punti percentuali di vantaggio rispetto a tutti gli impulsi A->B
   delle stesse sessioni;
5. almeno 18 delle 20 osservazioni disponibili a 15 minuti;
6. movimento mediano a 15 minuti positivo sia LONG sia SHORT;
7. rapporto tra mediana MFE15 e mediana MAE15 almeno 1,5.
```

Il punto 7 significa concretamente che il movimento favorevole tipico deve essere almeno una volta e mezza quello contrario tipico.

## Criterio di arresto

```text
PROMUOVI
- il primo campione completo supera tutti i criteri;
- passo successivo: simulazione execution con costi, non ordini reali.

SCARTA
- il primo campione completo fallisce anche un solo criterio;
- le regole non vengono corrette sullo stesso campione.

SCARTA PER RARITA'
- il campione minimo non arriva entro 40 sessioni New York.
```

Non esiste estensione indefinita del test. `ProminenceRank`, prominence, depth, percentile e location non qualificano questa shadow.

## Vincoli permanenti di questa fase

```text
OperationalEntry=FALSE
OrderSubmitted=FALSE
PnLComputed=FALSE
ShadowOrders=0
Validated=FALSE
```
