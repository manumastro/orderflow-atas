# Il Backtest

**La fase 4 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#4-il-backtest)):
prendere le regole e **rigiocare la storia**, con il codice e non a mano. Ne escono la curva dei
profitti e le statistiche: netto, percentuale vincente, media per operazione, drawdown
`[MC · 7:19 - 9:11]`. Non e' l'ultimo passo: poi si valida.

Dove sta:

- **il codice**: il progetto [`quant/framework/Quant.Motore/`](../framework/Quant.Motore/): il motore
  (`Backtest.cs`), le operazioni e le convenzioni (`Tipi.cs`), le statistiche (`Metriche.cs`), il
  simulatore di contratto (`Contratti/`, vedi [`4-il-simulatore-di-contratto.md`](4-il-simulatore-di-contratto.md));
- **i test**: [`quant/framework/Quant.Test/`](../framework/Quant.Test/), ciascuno con un caso
  costruito a mano;
- **i dati**: nella base comune, perche' li usano tutti: [`i-dati-da-atas.md`](i-dati-da-atas.md);
- **i risultati di uno studio**: nella sua cartella, `quant/studi/<idea>/4-backtest/` o
  `5-validazione/risultati/`.

## Come Si Lancia

```bash
quant prova <strategia> [--p chiave=valore ...]   # backtest dentro campione, annotato nel registro
```

`prova` lavora **solo dentro campione** e annota la variante nel registro: ogni backtest e' una
variante provata, e conta nello Sharpe deflazionato della fase 5. Per questo vuole la divisione
fissata; prima, si prova sul mercato sintetico (`--simbolo SINT`).

## Il Motore

A ogni barra chiusa il motore chiama la strategia (`SuBarra`), poi esegue nella barra dopo cio' che
la strategia ha deciso. Ogni operazione chiusa porta: verso, contratti, prezzi e istanti, motivo
d'uscita (stop, target, tempo, fine giornata, strategia, fine dati), giornata di borsa, MAE e MFE
(massima escursione avversa e favorevole), tick, lordo, costi, netto e, se c'era lo stop, il
risultato in multipli del rischio (R).

## Le Convenzioni Di Esecuzione

*Aggiunta del framework*: il transcript non tratta costi, slittamento e ordine degli eventi dentro
una barra. Sono stampate accanto a ogni numero che producono, e stanno in `Esecuzione` (`Tipi.cs`).

| convenzione | valore di default | perche' |
|---|---|---|
| ordine a mercato | si esegue all'apertura della barra dopo la decisione | la decisione si prende a barra chiusa |
| commissione | 2,50 $ per contratto per lato | commissioni e tasse CME, ordine di grandezza retail/prop |
| slittamento | 1 tick su ogni esecuzione a mercato o a stop, in ingresso e in uscita | NQ ha spread di 1 tick |
| limite | si esegue solo se il prezzo lo **attraversa** di un tick | toccarlo non garantisce l'eseguito |
| stop e target nella stessa barra | **vince lo stop** | senza tick non si sa chi e' venuto prima |
| target nella barra dell'ingresso | **non conta** | idem |
| eseguito oltre lo stop (gap) | l'ingresso non si fa, e si conta fra i saltati | il rischio non e' misurabile |
| uscita a tempo | all'apertura della prima barra all'ora d'uscita o dopo | come un ordine a mercato |
| rollover | la serie si rettifica all'indietro (Panama) | una posizione a cavallo del cambio non deve vedere un salto finto |

Si cambiano con `--commissione` e `--slittamento`; un cambio fa una variante nuova.

## Le Statistiche

`Metriche.cs`. **Due famiglie**, e la distinzione e' la lezione piu' cara della fase 40R:

- **per operazione, descrivono**: operazioni, % vincenti, media vincente e perdente, payoff, profit
  factor, aspettativa, tick medi per contratto, R medio, serie massima di perdite;
- **per giornata, misurano**: netto giornaliero, Sharpe e Sortino annui, drawdown massimo e sua
  durata in giornate, asimmetria e curtosi, e il **t del netto medio giornaliero**, una
  osservazione per giornata. Le operazioni della stessa giornata non sono indipendenti: il t per
  operazione (`TIngenuo`) si stampa per confronto e non si usa. Nella fase 40R uno z ingenuo di
  +4,1 e' diventato t = +1,4.

Le quattro statistiche del transcript (netto, % vincente, media, drawdown) ci sono tutte.
