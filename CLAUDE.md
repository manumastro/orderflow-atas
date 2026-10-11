# orderflow-atas

Questo file tiene **solo cio' che vale in ogni sessione**. Il resto sta nei documenti, e
**una riga qui non basta mai per agire**: dice che c'e' un obbligo e dove sta scritto come si
assolve. Se stai per applicare una regola di cui hai letto solo la riga qui, apri prima il
documento. **La conversazione non e' una fonte, e la memoria nemmeno.**

## Le Tre Decisioni

Dall'11 ottobre 2026.

1. **Lo scope e' il metodo quantitativo istituzionale**, costruito come framework dalla A alla Z:
   idea, regole, codice, backtest, validazione. La fonte del metodo e' il transcript di Matteo
   Conte (`quant/fonti/`); le idee vengono dai paper. **Il corso di Fabio non comanda piu'**: e' una
   fonte di idee e un supporto, e un'osservazione del live diventa regola solo passando la pipeline.
2. **Lo strumento e' NQ, e solo NQ; i dati vengono solo da ATAS.** Altri strumenti e altre fonti
   si aggiungono su richiesta esplicita.
3. **Si arriva fino alla validazione.** Nessun ordine e nessuna esecuzione automatica: l'esecuzione
   si progetta quando il framework e' maturo, su richiesta esplicita. **La velocita' e' un
   requisito**: il framework e' in C# .NET 10, non in Python.

```text
1  quant/fonti/*                         transcript e paper                 <- la fonte
2  quant/la-metodologia-istituzionale.md la metodologia come da transcript  <- comanda, cita 1 al minuto
3  quant/il-metodo-quantitativo.md       come il framework la realizza, e le aggiunte dichiarate
4  quant/idee/*.md                       una scheda per strategia
```

Fra 2 e 3 vince 2: un'aggiunta del framework non cambia la metodologia, la completa e lo dice.

## Gli Obblighi Del Metodo

Cinque, uno per riga. **Ciascuno si assolve aprendo il suo documento**, non fidandosi di questa riga.

| | | documento |
|---|---|---|
| **1** | **Nessuna strategia senza la sua scheda**: paper, finding, perche', cosa la ucciderebbe, varianti dichiarate prima di provarle. Il paper non si copia. | [come-si-scrive-un-idea](quant/idee/come-si-scrive-un-idea.md) |
| **2** | **Il fuori campione si guarda una volta**, a regole congelate (`valida --fuori`). La divisione si fissa una volta per simbolo e non si sposta. Ogni prova passa dal registro. | [il-metodo-quantitativo](quant/il-metodo-quantitativo.md) |
| **3** | **Ogni risultato contro il modello nullo, e la significativita' per giornata**, mai per operazione. Un risultato senza nullo non e' un risultato. | [il-metodo-quantitativo](quant/il-metodo-quantitativo.md) |
| **4** | **Ogni numero porta le sue convenzioni**: costi, slittamento, ambiguita', campione, varianti provate. I risultati negativi si scrivono come quelli positivi. | [il-metodo-quantitativo](quant/il-metodo-quantitativo.md) |
| **5** | **Il framework si valida prima delle strategie**: dopo ogni modifica al motore o alla validazione, i test e la prova sul mercato sintetico (sulla passeggiata casuale non passa niente). | [il-metodo-quantitativo](quant/il-metodo-quantitativo.md) |

**Quando l'utente chiede una lettura del mercato dal vivo**, valgono gli otto obblighi della
lettura discrezionale, tutti: [gli-obblighi-della-lettura-dal-vivo](docs/research/metodo/gli-obblighi-della-lettura-dal-vivo.md).

## Regole Di Lavoro

- **Nessuna regola operativa e' approvata.** Superare i cancelli dice che la prova non ha smentito
  la strategia, non che la strategia va usata.
- **Le fasi chiuse restano chiuse**: il modello 40R come modello (le sue lezioni di metodo sono
  dentro il framework), la sorveglianza a condizioni armate, la strada del sottoagente. Non si
  riaprono senza richiesta esplicita.
- **Quando una regola cambia, si cambia nel documento che la tiene**, e qui resta al massimo la
  riga che ci rimanda: due copie della stessa regola divergono senza che nessuno se ne accorga.
- **Per ogni fase sostanziale** aggiorna il documento canonico e aggiungi una riga datata a
  `FabioOrderFlow/progress.txt`.
- **Registra i risultati negativi** come quelli positivi, e dichiara soglie, convenzioni e
  orizzonti accanto al numero che producono.
- **Scrivi per una persona e per un agente**: termine tecnico spiegato alla prima occorrenza,
  frasi brevi, nessuna decisione importante lasciata solo nella conversazione.
- **Ogni file di documentazione ha un nome descrittivo.** Niente `README.md`; l'ingresso e'
  `da-dove-si-comincia.md`. L'unica cartella esclusa e' `docs/atas/`.
- **Non accumulare output intermedi**: i file grezzi vanno nella scratchpad di sessione, e le prove
  sul mercato sintetico usano `QUANT_REGISTRO`, `QUANT_RISULTATI` e `QUANT_DATI` in una cartella
  temporanea, mai il registro vero.

## Dove Sta Il Resto

**Prima di lavorare qui, leggi** [`quant/la-metodologia-istituzionale.md`](quant/la-metodologia-istituzionale.md)
(la metodologia, fase per fase), [`quant/il-metodo-quantitativo.md`](quant/il-metodo-quantitativo.md)
(come il framework la realizza, i cancelli, come si usa) e [`da-dove-si-comincia.md`](da-dove-si-comincia.md)
(la mappa). I dati: [`quant/i-dati-da-atas.md`](quant/i-dati-da-atas.md).

Le regole che valgono solo su una parte del repository si caricano da sole quando si tocca quella
parte, e stanno in [`.claude/rules/`](.claude/rules/): il framework quantitativo, i dati e il
bridge, l'indicatore e il deploy, il file della giornata, il file delle regole dei livelli.
