# orderflow-atas

Questo file tiene **solo cio' che vale in ogni sessione**. Il resto sta nei documenti, e
**una riga qui non basta mai per agire**: dice che c'e' un obbligo e dove sta scritto come si
assolve. Se stai per applicare una regola di cui hai letto solo la riga qui, apri prima il
documento. **La conversazione non e' una fonte, e la memoria nemmeno.**

## Le Tre Decisioni

Dall'11 ottobre 2026.

1. **Lo scope e' il metodo quantitativo istituzionale**, costruito come framework dalla A alla Z:
   idea, regole, codice, backtest, validazione. La fonte del metodo e' il transcript di Matteo
   Conte (`quant/metodo/fonti/`); le idee vengono dai paper. **Il corso di Fabio non comanda piu'**: e' una
   fonte di idee e un supporto, e un'osservazione del live diventa regola solo passando la pipeline.
2. **Lo strumento e' NQ, e solo NQ; i dati vengono solo da ATAS.** Altri strumenti e altre fonti
   si aggiungono su richiesta esplicita. **ES** dal 14 ottobre 2026, su richiesta, solo per lo
   studio delle opzioni (gli indicatori X-Ray di ATAS disegnano solo su ES). **Macro e COT** da fonti
   pubbliche (World Monitor, CFTC) dal 15 ottobre 2026, su richiesta, solo come **contesto** dello
   studio globale: mai in un backtest.
3. **Si arriva fino alla validazione.** Nessun ordine e nessuna esecuzione automatica: l'esecuzione
   si progetta quando il framework e' maturo, su richiesta esplicita. **La velocita' e' un
   requisito**: il framework e' in C# .NET 10, non in Python.

```text
1  quant/metodo/fonti/, quant/paper/            transcript; paper per filone       <- la fonte
2  quant/metodo/la-metodologia-istituzionale.md la metodologia come da transcript  <- comanda, cita 1 al minuto
3  quant/il-metodo-quantitativo.md              la mappa e le aggiunte dichiarate
   quant/metodo/<n>-*.md                        come il framework realizza la fase n
4  quant/studi/<idea>/                          un'idea dalla A alla Z, con le fasi numerate
```

`quant/` ha cinque aree: **metodo** (come si fa), **framework** (il codice C# riusabile), **paper**
(la biblioteca), **studi** (le idee), piu' **registro** e **dati**. Fra 2 e 3 vince 2: un'aggiunta
del framework non cambia la metodologia, la completa e lo dice.

## Gli Obblighi Del Metodo

Cinque, uno per riga. **Ciascuno si assolve aprendo il suo documento**, non fidandosi di questa riga.

| | | documento |
|---|---|---|
| **1** | **Nessuna strategia senza la sua scheda** (`quant/studi/<idea>/1-idea.md`): paper, finding, perche', cosa la ucciderebbe, varianti dichiarate prima di provarle. Il paper non si copia. | [come-si-scrive-un-idea](quant/metodo/1-come-si-scrive-un-idea.md) |
| **2** | **Il fuori campione si guarda una volta**, a regole congelate (`valida --fuori`). La divisione si fissa una volta per simbolo e non si sposta. Ogni prova passa dal registro. | [come-si-valida](quant/metodo/5-come-si-valida.md) |
| **3** | **Ogni risultato contro il modello nullo, e la significativita' per giornata**, mai per operazione. Un risultato senza nullo non e' un risultato. | [come-si-valida](quant/metodo/5-come-si-valida.md), [il-backtest](quant/metodo/4-il-backtest.md#le-statistiche) |
| **4** | **Ogni numero porta le sue convenzioni**: costi, slittamento, ambiguita', campione, varianti provate. I risultati negativi si scrivono come quelli positivi. | [il-backtest](quant/metodo/4-il-backtest.md#le-convenzioni-di-esecuzione) |
| **5** | **Il framework si valida prima delle strategie**: dopo ogni modifica al motore o alla validazione, i test e la prova sul mercato sintetico (sulla passeggiata casuale non passa niente). | [il-framework-si-valida-prima](quant/metodo/5-il-framework-si-valida-prima.md) |

## Il Metodo Agentico

Dal 15 ottobre 2026 lo studio si fa anche **con agenti, uno per fase, ognuno con la sua skill**, e un
orchestratore che li mette in fila: `/studio-globale` (macro, COT, gamma, order flow, quadro) e
`/studio-paper` (cerca, scarica, leggi, verifica, sintesi). **Un lavoro nuovo di questo tipo si
costruisce con le dodici regole** di [la-metodologia-agentica](studio/la-metodologia-agentica.md) (la
prima: il codice raccoglie e misura, l'agente legge); i sistemi, le fonti, come si lanciano e le
lezioni del primo giro in [il-sistema-di-studio](studio/il-sistema-di-studio.md).

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

**Prima di lavorare qui, leggi** [`quant/metodo/la-metodologia-istituzionale.md`](quant/metodo/la-metodologia-istituzionale.md)
(la metodologia, fase per fase), [`quant/il-metodo-quantitativo.md`](quant/il-metodo-quantitativo.md)
(come il framework la realizza, i cancelli, come si usa) e [`da-dove-si-comincia.md`](da-dove-si-comincia.md)
(la mappa). I dati: [`quant/metodo/i-dati-da-atas.md`](quant/metodo/i-dati-da-atas.md).

Le regole che valgono solo su una parte del repository si caricano da sole quando si tocca quella
parte, e stanno in [`.claude/rules/`](.claude/rules/): il framework quantitativo, i dati e il
bridge, l'indicatore e il deploy, il file della giornata, il file delle regole dei livelli, la
metodologia agentica (skill, agenti, `studio/`).
