# Il Metodo Quantitativo: Il Framework

Dall'11 ottobre 2026 lo scope del progetto e' **la metodologia istituzionale**, costruita come
framework dalla A alla Z. Due documenti, con due ruoli diversi:

- [`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md): **la metodologia esattamente
  come da transcript**, fase per fase, ogni affermazione con il suo minuto `[MC · mm:ss]`. Non
  contiene aggiunte. **Comanda.**
- questo documento: **come il framework realizza ogni fase**, e cosa aggiunge, dichiarandolo come
  aggiunta, con il motivo. Dove il framework e la metodologia divergono, lo si scrive qui.

Il codice sta in `quant/src/`, i dati in `quant/dati/` (fuori da git), le idee in `quant/idee/`, i
rapporti in `quant/risultati/`, il registro delle prove in `quant/registro/`.

Le decisioni che delimitano il lavoro, prese dall'utente:

- **solo NQ**, per ora;
- **dati solo da ATAS** ([`i-dati-da-atas.md`](i-dati-da-atas.md));
- **si arriva fino alla validazione**: le fasi 6 e 7 della metodologia (il vivo, il portafoglio)
  sono fuori scope finche' il framework non e' maturo;
- **la velocita' e' un requisito**, per questo il framework e' in **C# .NET 10**, lo stesso
  linguaggio di ATAS: una strategia validata qui potra' un giorno girare dentro ATAS senza essere
  riscritta.

## Fase Per Fase

| | fase della metodologia | nel framework | aggiunta del framework |
|---|---|---|---|
| 0 | [la differenza](la-metodologia-istituzionale.md#0-la-differenza-fra-retail-e-istituzionale): strategie validate, non operazioni | tutto il framework | — |
| 1 | [l'idea](la-metodologia-istituzionale.md#1-lidea): dal paper, il finding e il perche', non si copia | una scheda per idea in `quant/idee/` ([modello](idee/come-si-scrive-un-idea.md)); le fonti in `quant/fonti/` | la scheda scrive anche **cosa ucciderebbe la strategia** (dal perche' strutturale `[MC · 46:23]`) e **le varianti, dichiarate prima di provarle** |
| 2 | [le regole](la-metodologia-istituzionale.md#2-le-regole): ingresso, uscita (stop, target, tempo), dimensione | `Ordine` (ingresso, `Stop`, `Target`/`TargetR`, `UscitaAlle`), `Dimensione` (`fissa`, `rischio`, `volatilita`) | — |
| 3 | [il codice](la-metodologia-istituzionale.md#3-il-codice): a iterazioni, ogni pezzo verificato | una classe `Strategia` in `Quant/Strategie/`, registrata in `Catalogo.cs`; il codice lo scrive l'agente | **la strategia vede solo il passato** (il `Contesto`): il look-ahead non si puo' scrivere |
| 4 | [il backtest](la-metodologia-istituzionale.md#4-il-backtest): rigiocare la storia, netto, % vincente, media, drawdown | `Backtest.Esegui`, `Metriche`, `quant prova` | **convenzioni di esecuzione dichiarate**; **statistiche per giornata** |
| 5a | [dentro e fuori campione](la-metodologia-istituzionale.md#5a-il-test-di-divisione-dentro-e-fuori-campione), 80/20, regole congelate | `quant divisione`, `quant valida --fuori` | la divisione **si fissa una volta**; il fuori campione **si consuma**; **il modello nullo**; **lo Sharpe deflazionato**; **i cancelli con soglie** |
| 5b | [il Monte Carlo](la-metodologia-istituzionale.md#5b-il-monte-carlo): rimescolamento, ricampionamento, distribuzione, metri per il vivo | `MonteCarlo.Rimescola`, `Ricampiona`; i metri nel rapporto | **il ricampionamento per giornata**, a blocchi |
| 6 | [il vivo](la-metodologia-istituzionale.md#6-il-vivo): automazione, sorveglianza | **fuori scope** | — |
| 7 | [il ciclo e il portafoglio](la-metodologia-istituzionale.md#7-il-ciclo-e-il-portafoglio): strategie poco correlate, revisione mensile | **fuori scope**, salvo il registro che tiene la storia delle prove | — |

**Gli esempi del transcript** sono nel catalogo come strategie di prova, ciascuno con la sua scheda:
[`momento-intraday`](idee/momento-intraday.md) (esempio 1), [`vwap-tendenza`](idee/vwap-tendenza.md)
(esempio 2), [`anomalia-notte`](idee/anomalia-notte.md) (esempio 4). **La PEAD (esempio 3) non c'e'**:
lavora su azioni, utili e consenso degli analisti, e lo scope e' NQ.

## Le Aggiunte, E Perche'

Il transcript non tratta alcune cose che in questo repository sono gia' costate care, nella fase
40R ([`fase-sistematica-40r-chiusa.md`](../docs/research/archivio-modello-40r/fase-sistematica-40r-chiusa.md)).
L'elenco di cio' che il transcript non dice sta in fondo a
[`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#cosa-il-transcript-non-dice).

| aggiunta | perche' |
|---|---|
| **la strategia vede solo il passato** | il look-ahead e' stato il primo errore della fase 40R |
| **convenzioni di esecuzione conservative**, stampate accanto al numero | nella fase 40R un limite si eseguiva al tocco, cioe' a mercato |
| **statistiche per giornata**: il t si calcola sul netto giornaliero | le operazioni della stessa giornata non sono indipendenti: uno z ingenuo di +4,1 e' diventato t = +1,4 |
| **il modello nullo** | nella fase 40R le condizioni facevano 49% e il nullo 47%: senza confronto il 49% sembrava un risultato |
| **la divisione si fissa una volta** e non si sposta | ricalcolata a ogni download, porterebbe pezzi di fuori campione dentro senza che nessuno se ne accorga |
| **il fuori campione si consuma**, e il registro sa se un'altra variante l'ha gia' visto | il fuori campione guardato due volte e' diventato dentro |
| **il registro delle prove** (`quant/registro/prove.jsonl`, in git) | per contare le varianti provate |
| **lo Sharpe deflazionato** (Bailey e Lopez de Prado, 2014) | provando cento varianti, la migliore ha uno Sharpe alto anche se nessuna vale niente |
| **il ricampionamento per giornata**, a blocchi di 5 | pescare operazioni una per una sottostima le code |
| **i cancelli, con soglie** | il transcript dice "buoni risultati dentro e fuori": serve un criterio scritto prima di guardare |

## Le Convenzioni Di Esecuzione

| convenzione | valore di default | perche' |
|---|---|---|
| ordine a mercato | si esegue all'apertura della barra dopo la decisione | la decisione si prende a barra chiusa |
| commissione | 2,50 $ per contratto per lato | commissioni e tasse CME, ordine di grandezza retail/prop |
| slittamento | 1 tick su ogni esecuzione a mercato o a stop, in ingresso e in uscita | NQ ha spread di 1 tick |
| limite | si esegue solo se il prezzo lo **attraversa** di un tick | toccarlo non garantisce l'eseguito |
| stop e target nella stessa barra | **vince lo stop** | senza tick non si sa chi e' venuto prima |
| target nella barra dell'ingresso | **non conta** | idem |
| eseguito oltre lo stop (gap) | l'ingresso non si fa, e si conta fra i saltati | il rischio non e' misurabile |
| rollover | la serie si rettifica all'indietro (Panama) | una posizione a cavallo del cambio non deve vedere un salto finto |

**La dimensione** realizza la fase 2 della metodologia: `fissa` (un contratto, lo sviluppo), `rischio`
(stessa somma a rischio sullo stop: il volatility targeting del transcript, dove la distanza dello
stop segue il range), `volatilita` (stessa somma divisa l'escursione media delle ultime N giornate).

## Il Modello Nullo

Rifa ogni operazione vera con le stesse uscite, gli stessi contratti e gli stessi costi, togliendo
una cosa sola:

- **il quando**: giornata e minuto a caso dentro il campione e dentro la finestra della strategia,
  stesso verso, **stessa esposizione** (l'uscita a tempo si sposta con l'ingresso). E' il nullo che
  toglie il beta: su un campione salito per caso, anche il nullo long guadagna;
- **il verso**: stesso istante, verso a caso. Su una strategia che va solo long misura il beta del
  campione, non un vantaggio: **e' un cancello solo per le strategie che scelgono il verso**.

## I Cancelli

Una validazione **supera i cancelli** se passano tutti. Superarli non approva una regola operativa:
dice che la prova non l'ha smentita. Le soglie stanno in `Validatore.cs` e qui, e si cambiano insieme.

| | condizione | soglia | fase |
|---|---|---|---|
| D1 | dentro: batte il nullo del quando | p <= 0,05 | aggiunta |
| D2 | dentro: batte il nullo del verso (solo se la strategia sceglie il verso) | p <= 0,05 | aggiunta |
| D3 | dentro: Sharpe deflazionato per le varianti nel registro | >= 0,95 | aggiunta |
| F1 | fuori: operazioni | >= 30 | aggiunta |
| F2 | fuori: netto | > 0 | 5a, "rende ancora bene fuori" |
| F3 | fuori: Sharpe annuo | >= 50% di quello dentro | 5a, "confrontare dentro e fuori" |
| F4 | fuori: mai guardato da un'altra variante | intatto | 5a, "regole congelate" |

Il Monte Carlo non ha un cancello: il transcript lo usa per leggere la forza del vantaggio e per i
metri del vivo, e il rapporto li stampa (finale p5/p50/p95, probabilita' di perdita, drawdown
p50/p95/p99, serie di perdite, drawdown entro 10, 20, 50 e 100 operazioni).

## Come Si Usa

```bash
cd quant/src
dotnet build -c Release                  # una volta, e dopo ogni modifica
dotnet test Quant.Test -c Release        # i test del motore e della validazione
Q=Quant.Cli/bin/Release/net10.0/quant.exe

$Q chart                                 # i chart aperti in ATAS
$Q scarica --chart <id> --simbolo NQ     # le barre del chart in archivio
$Q dati                                  # cosa c'e' in archivio
$Q divisione --simbolo NQ                # mostra il confine dentro/fuori; --fissa lo fissa, una volta
$Q strategie                             # il catalogo, con regole e parametri
$Q prova momento-intraday --p rr=2       # fase 4: backtest dentro campione, nel registro
$Q valida momento-intraday               # fase 5 dentro: nulli, Monte Carlo, Sharpe deflazionato
$Q valida momento-intraday --fuori       # fase 5a: una volta, a regole congelate
```

Opzioni comuni: `--simbolo NQ|SINT`, `--tipo m1`, `--p chiave=valore` (ripetibile),
`--commissione 2.5`, `--slittamento 1`, `--repliche 1000`, `--simulazioni 10000`. Un parametro
sconosciuto e' un errore: quasi sempre e' un refuso.

**La divisione di NQ non e' ancora fissata**, apposta: oggi l'archivio ha 50 giornate (3 agosto - 9
ottobre 2026, dal chart M1 di NQZ6). Si fissa quando la storia lunga e' scaricata.

**Il codice di una strategia si scrive come dice la fase 3**: prima l'ingresso, si verifica; poi
l'uscita, si verifica; poi la dimensione.

## Il Framework Si Valida Prima Delle Strategie

Su un mercato finto, dove la verita' e' nota (`quant sintetico`), il framework deve fare due cose:
non trovare niente dove non c'e' niente, e trovare cio' che si e' piantato. Misurato l'11 ottobre
2026, 1.500 giornate sintetiche per prova, 400 repliche del nullo:

| prova | esito |
|---|---|
| **passeggiata casuale**, 3 strategie x 20 semi | **nessuna supera i cancelli**; D1 a p <= 0,05 in 3 casi su 60, il 5% atteso: il nullo e' tarato |
| **deriva piantata di notte**, +15 punti a notte | `anomalia-notte` passa D3, F2, F3 e i cancelli fuori; **D1 no** |
| deriva di notte +15 / +30 / +50 punti, 5 semi ciascuna | D1 a p <= 0,05 in 0, 0 e 1 caso su 5 |

**Il limite che ne esce: D1 ha poca potenza sulle anomalie di esposizione.** Quando la strategia
*e'* una finestra di esposizione (la notte), le finestre casuali con la stessa esposizione coprono
gran parte della stessa notte, e la differenza da misurare si riduce. Per quelle ipotesi la prova
giusta e' il confronto con il complemento, che e' anche quello del paper dell'esempio 4:
`anomalia-notte --p parte=notte` contro `--p parte=giorno`. Un confronto appaiato fra due varianti
non e' ancora un comando.

Due difetti del nullo trovati cosi', e corretti prima di questa versione:

1. **il nullo del quando dentro la stessa giornata sbirciava il futuro.** Il verso di un'operazione
   VWAP dipende dal percorso della giornata; spostarla prima nella stessa giornata le faceva
   catturare il movimento che l'aveva generata. Sulla passeggiata casuale quel nullo guadagnava
   +447.000 $. Ora la giornata si pesca a caso;
2. **il nullo del verso premiava il beta**: su un campione salito per caso, una strategia solo long
   batteva la moneta. Ora conta solo per le strategie che scelgono il verso.

**Ogni modifica al motore o alla validazione ripete questa prova** prima di toccare dati veri.

## La Velocita'

Misurata l'11 ottobre 2026 su questo PC, Release:

| operazione | tempo |
|---|---|
| generare 2,07 milioni di barre M1 sintetiche (1.500 giornate) | 2,5 s |
| scaricare da ATAS 41.142 barre M1 | 2,9 s |
| `valida --fuori` completa su 2,07 milioni di barre: 2 backtest, 4 nulli da 1.000 repliche, 3 Monte Carlo da 10.000 | 0,9 - 2,6 s |
| 23 test unitari | 0,4 s |

## Cosa Manca

- **La storia lunga di NQ.** Quanta ne da' ATAS lo dice il tariffario, e si misura appena la
  connessione torna: [`i-dati-da-atas.md`](i-dati-da-atas.md).
- **Tick e book nel motore.** Il motore lavora su barre; con i tick l'ambiguita' stop/target si
  risolve esattamente invece che al peggio.
- **Il confronto appaiato fra varianti**, per le anomalie di esposizione.
- **Le idee dai paper sulle prop firm**, che l'utente mandera'.
- **Le fasi 6 e 7**: il vivo e il portafoglio, fuori scope finche' il framework non e' maturo.
