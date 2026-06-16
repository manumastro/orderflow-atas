# Analisi: come rilevare correttamente le zone di balance/consolidamento

**Data:** 2026-06-16 *(ultimo aggiornamento nella stessa giornata)*  
**Contesto:** FabioMeanReversion / Modello 2 — mean reversion in balance (dal transcript di Fabio Valentino)  
**Stato attuale:** il detector iterativo `impulso + range ratio` produce zone frammentate, aggregate o troppo piccole; siamo orientati a sostituirlo con un approccio volume-profile puro.

---

## 1. Cosa dice davvero il transcript

Le frasi chiave da non tradurre male:

- *“Il Modello 2 è mean reverting… consolidamento… condizione out of balance che rientra dentro il balance.”*
- *“Vedi le candele compresse e ci tracci sopra il profile.”*
- *“Identificare l’area di compressione più interessante… dove il mercato non ha transato più alto né basso.”*
- *“L’era di consolidamento è ancora la stessa.”*
- *“Puoi renderlo stupidamente semplice mettendo solo il daily profile.”*

Il metodo di Fabio non è: “trova un’impulse bar con ratio X e controlla che la zona sia ≤ 0.8Y”.  
Il metodo è: **vedi un’area dove il prezzo ha smesso di estendere e ci tracci sopra il profile**.  
Se il profile mostra un’area di valore concentrata, quella è balance. Se rompe con follow-through, l’era finisce.

---

## 2. Perché l’approccio `impulso + range ratio` è fragile

Dai log dell’indicatore attuale emergono due problemi:

| Problema | Cosa succede nei log | Perché è sbagliato |
|---|---|---|
| **Micro-impulsi** | Barre di 30-40 punti vengono classificate come “leg” espansivo | Il confronto è fatto sulla media locale troppo bassa; ogni piccolo movimento interno al consolidamento resetta l’ancoraggio e spezza la zona |
| **Range ratio troppo sensibile** | `compRange / legRange` fa chiudere o scartare zone che a occhio sono balance | La qualità della zona dipende da quanto era grande l’impulso precedente, non dalla struttura della zona stessa |

Risultato: zone valide vengono divise in tanti rettangolini, oppure zone aggregate coprono periodi troppo lunghi.

---

## 3. Approcci usati da indicatori consolidamento esistenti

Ricerca effettuata su indicatori/scripts pubblici.

### 3.1 Impulso + range ratio (Trendoscope Consolidation Range Tracker)
- Trova un’“impulse wave” (tipicamente con zigzag).
- Calcola un range di consolidamento come ratio dell’impulso.
- Conferma la zona quando il prezzo è rimasto nel range per un numero sufficiente di barre.
- Breakout quando il prezzo esce dal range.

**Pro:** diretto.  
**Contro:** è esattamente dove siamo bloccati noi — sensibile alla qualità dell’impulso e ai micro-reset.

### 3.2 Support/resistance + touch points + ATR (TRN Trading)
- Identifica boundaries orizzontali.
- Richiede almeno N contatti (default 4) con i bordi per validare un range.
- Usa ATR come buffer per distinguere touch da breakout.

**Pro:** strutturale e visivamente intuitivo.  
**Contro:** richiede più parametri (touch, ATR, accuracy) e non è esattamente quello che descrive Fabio.

### 3.3 ATR / squeeze / ADX / multi-layer score (Rangezone ATR, Chop Detector)
- Comprime volatilità: range corto rispetto ad ATR.
- Filtri trend: ADX basso, pendenza EMA piatta, regressione lineare piatta.
- Oscillazione interna, drift, efficienza del percorso.
- Score di confluenza.

**Pro:** robusto e adattivo.  
**Contro:** troppo complesso per l’obiettivo “stupid simple” del transcript; rischia di diventare una macchina nera.

### 3.4 Volume Profile / Value Area (Zeiierman, Market Profile classico)
- Costruisce il profilo di volume sull’area compressa.
- Verifica che la maggior parte del volume sia concentrata in una Value Area.
- I bordi della Value Area tengono finché non c’è breakout con follow-through.

**Pro:** è letteralmente il metodo operativo di Fabio: *“vedi le candele compresse e ci tracci sopra il profile.”*  
**Contro:** richiede di calcolare il profilo a ogni barra (costo CPU) se fatto naive.

---

## 4. Proposta raccomandata: volume profile interno

Restano invariati i vincoli dell’utente:
- niente linee POC/VAH/VAL disegnate;
- solo rettangoli semi-trasparenti per le zone passate;
- niente parametri esposti;
- full-history scan.

La differenza è **interna**: invece di usare `rangeRatio`, usiamo il profilo di volume per decidere se l’area è balance.

### 4.1 Algoritmo proposto

```text
1. Individua un’area candidata dopo un movimento direzionale.
   (l’ancoraggio può restare quello attuale, ma meno sensibile).

2. Per ogni barra della candidata, accumula i livelli di prezzo/volume
   tramite GetCandle(bar).GetAllPriceLevels().

3. Calcola:
   - POC  = prezzo con volume massimo;
   - VAH/VAL = estremi della Value Area che contiene ~70% del volume.

4. La zona è valida se:
   - la Value Area è nettamente più stretta del range totale della candidata
     (es. 70% del volume nel 30-50% del range),
   - il volume non è tutto schiacciato sui bordi.

5. Il rettangolo (zone) copre High/Low dell’area candidata, NON VAH/VAL.

6. La zona si chiude quando il prezzo rompe i bordi dell’area con una
   barra espansiva (breakout con follow-through).
```

### 4.2 Vantaggi attesi

- **Fedele al transcript**: usa il profile come fa Fabio, senza linee.
- **Più robusto**: non dipende dalla dimensione dell’impulso precedente.
- **Strutturale su tutti i TF**: la Value Area è un concetto di prezzo, non di numero di barre.
- **Riduce fusioni**: se il profilo non è concentrato, non viene disegnata nessuna zona, evitando mega-rettangoli.

### 4.3 Alternative più semplici (se il profilo fosse troppo pesante)

| Alternativa | Idea | Trade-off |
|---|---|---|
| **ATR band containment** | Prezzo dentro banda di ATR per X minuti | Più leggero, meno fedele al transcript |
| **Touch points + ATR** | Range con almeno N contatti sui bordi | Più strutturale, ma richiede più parametri interni |

---

## 5. Individuazione dell’area candidata tramite volume?

**Sì, e anzi è preferibile.**  
Se il profile serve a *validare* una zona, lo stesso profile può anche dirci *dove inizia* il consolidamento.

### 5.1 Come si manifesta la transizione da leg a balance

| Fase del mercato | Segnale di volume | Perché è rilevante |
|---|---|---|
| **Fine del leg** | Volume climax o delta-divergenza (assorbimento) | L’iniziativa direzionale si esaurisce |
| **Inizio consolidamento** | Volume che si concentra in un nodo ristretto | Il prezzo trova un’area di accettazione |
| **Durante la zona** | Volume distribuito attorno alla POC, VA stabile | L’era di balance è ancora viva |
| **Breakout** | Espansione del range con volume crescente | Il balance rompe |

Non serve guardare solo le chiusure: guardiamo **come il volume è distribuito nello spazio dei prezzi**.

### 5.2 Fine di un leg: effort vs result

- Alla fine di un movimento direzionale si osserva spesso un picco di volume con scarsa progressione del prezzo: i partecipanti aggressivi (delta) non riescono più a spingere oltre.
- In ATAS possiamo usare `Candle.Delta` oppure sommare `Bid`/`Ask` dai `PriceLevelInfo` della barra.
- Quando il delta tiene da una parte ma il prezzo non segue, è il classico *absorption*: probabile fine del leg e inizio zona.

### 5.3 Inizio della candidata con profilo mobile

Invece di cercare un impulso mediante massimi/minimi, usiamo una **finestra mobile di N barre** e calcoliamo:

1. `totalRange = Massimo(High) - Minimo(Low)` della finestra.
2. `vaRange = VAH - VAL` della Value Area al 70%.
3. `concentration = vaRange / totalRange` (piccolo = volume concentrato).
4. `pocPosition = (POC - minLow) / totalRange` (0.35-0.65 = POC centrale).
5. `deltaSlope` = pendenza del cumulative delta nella finestra (≈0 = indecisione).

La candidata inizia quando passiamo da:

- finestra precedente `concentration` alto / `vaRange` ampio / `deltaSlope` marcato (trend),  
- a finestra corrente `concentration` basso, `pocPosition` centrale, `deltaSlope` ≈ 0 (balance).

In pratica: rileviamo un **cambio di regime del volume** da distribuzione allungata a distribuzione compatta.

### 5.4 Come evitare falsi candidati

- **Volume minimo relativo**: in fasi di mercato morto anche un range stretto sembra balance. Richiediamo volume nella finestra almeno pari alla media dell’ultimo periodo.
- **Non confermare subito**: la candidata deve persistere per alcune barre con profile stabile, altrimenti è solo un singolo battito di mercato.
- **Evitare zone in mezzo al trend**: se `vaRange` resta grande e il POC si sposta in direzione del trend, non è balance, è continuazione.

### 5.5 Algoritmo candidato integrato

```text
Per ogni barra i:
  W = finestra mobile [i - N, i]
  Profilo(W) -> volume per livello di prezzo
  Calcola POC, VAH, VAL, totalVolume, cumulativeDelta, totalRange

  Se finestra precedente era in trend:
    - deltaSlope marcato
    - POC in movimento
    - vaRange grande rispetto al range

  E finestra corrente è balance:
    - vaRange <= 0.45 * totalRange
    - POC compresa tra il 35% e il 65% del range
    - deltaSlope vicino a 0
    - totalVolume >= media volume locale

  Allora avvia una nuova candidata al bordo W.start.

  Conferma/estendi la candidata finché il profile resta compatto.
  Chiudi quando il prezzo rompe High/Low della candidata con espansione + volume.
```

Questo approccio elimina la necessità di un `FindLastImpulse`: l’inizio della zona è definito dal **cambio di regime del volume**, esattamente come fa un trader che legge il profile.

---

## 6. Conclusione

L’approccio `impulso + range ratio` è troppo fragile per catturare le vere zone di balance visibili a occhio.  
La direzione corretta è **usare il Volume Profile internamente**, sia per **individuare** l’area candidata sia per **validarla**.

I rettangoli restano gli stessi (semi-trasparenti, niente POC/VAH/VAL disegnate), ma sotto il cofano il detector diventa un osservatore di **distribuzione del volume nello spazio-prezzo**: passaggio da trend a nodo compatto = start; rottura dei bordi con espansione = stop.

**Prossimo passo consigliato:** implementare il detector volume-profile puro, rimuovendo `FindLastImpulse` e le logiche di ratio sull’impulso.
