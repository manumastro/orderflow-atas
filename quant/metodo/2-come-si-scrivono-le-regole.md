# Come Si Scrivono Le Regole

**La fase 2 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#2-le-regole)):
dopo l'idea, un insieme di regole **cosi' specifico da poterlo dare a una macchina**
`[MC · 4:30 - 4:54]`, con **tre caselle da riempire** `[MC · 29:35 - 30:15]` e una ragione per cui
dovrebbe fare soldi `[MC · 33:28 - 33:36]`, che sta nella scheda dell'idea (fase 1).

## Le Tre Caselle Nel Framework

Il codice di questa fase e' **il linguaggio delle regole**: cio' che una strategia puo' chiedere al
motore (`Ordine`, `Dimensione`, `Parametri`) e la forma del contratto di una prop firm
(`Contratti/Contratto.cs`). E' il progetto [`quant/framework/Quant.Regole/`](../framework/Quant.Regole/).

| casella | il transcript | nel framework |
|---|---|---|
| **ingresso** | le condizioni per aprire `[MC · 29:35 - 30:15]` | un `Ordine` (`Ordine.cs`): `Lato` (long o short), `Tipo` (mercato, stop, limite), `Prezzo` d'innesco, `Scadenza` |
| **uscita** | take profit, stop loss, e spesso il tempo `[MC · 30:15 - 30:48]` | nello stesso `Ordine`: `Stop` o `StopDistanza`; `Target`, `TargetDistanza` o `TargetR` (multipli del rischio); `UscitaAlle` (istante); in piu' la chiusura a fine giornata (`Strategia.ChiudeAFineGiornata`) e l'uscita decisa dalla strategia (`Contesto.Esci`) |
| **dimensione** | un contratto in sviluppo, volatility targeting dal vivo `[MC · 40:39 - 42:59]` | `Dimensione.cs`, scelta con il parametro `dimensione` |

**Le uscite arrivano al motore insieme all'ingresso**: una strategia non puo' aprire una posizione
senza aver gia' detto come la chiude. Il motore non fa piramidi: una posizione alla volta.

## La Dimensione

| `dimensione=` | cosa fa | parametri |
|---|---|---|
| `fissa` (default) | sempre gli stessi contratti: **lo sviluppo**, come dice il transcript | `contratti` (1) |
| `rischio` | stessa somma a rischio sullo stop: contratti = rischio / (distanza dello stop in dollari). E' il volatility targeting del transcript quando la distanza dello stop segue il range | `rischio` (1000 $), `maxContratti` (50) |
| `volatilita` | stessa somma divisa l'escursione media delle ultime N giornate chiuse: giorno largo, meno contratti | `rischio`, `giorniVolatilita` (20), `maxContratti` |

Le fonti del transcript per la dimensione sono *Volatility Managed Portfolios* e *The Impact of
Volatility Targeting* `[MC · 40:39 - 42:59]`, non ancora scaricati
([`quant/paper/indice-dei-paper.md`](../paper/indice-dei-paper.md)).

## Le Regole Della Fase

- **Semplici**: piu' regole vogliono dire piu' punti in cui la strategia si rompe
  `[MC · 39:39 - 39:49]`, `[MC · 1:21:01 - 1:21:31]`. Ogni regola in piu' si aggiunge solo dentro
  campione, e conta come variante (fase 5).
- **Scritte nella scheda prima del codice**, nei tre riquadri, con le varianti da provare dichiarate
  prima di provarle ([`quant/metodo/1-come-si-scrive-un-idea.md`](1-come-si-scrive-un-idea.md)).
- **Ogni numero e' un parametro**, con un default e un nome (`rr`, `uscita`, `minuti`): cosi' una
  variante e' un insieme di parametri, il registro la riconosce dalla sua impronta e il rapporto la
  stampa per intero. Il codice e' `Parametri.cs`, in questa fase; come la strategia li legge, nella
  fase 3.
- **Gli orari sono di New York** (ET), come nel transcript; la giornata di borsa di NQ comincia alle
  18:00 ET del giorno prima.

## Le Regole Di Un Contratto

*Aggiunta del framework.* Anche il contratto di una prop firm e' un insieme di regole da dare a una
macchina: target, drawdown, livello di blocco del pavimento, quando si aggiorna, quando si controlla
la violazione, limite giornaliero, scadenza, prelievi, prezzi. La forma e' nel framework
(`Quant.Regole/Contratti/Contratto.cs`); ogni contratto vero e' un file nello studio che lo usa
(`studi/contratto-prop/3-codice/TradeifyGrowth50K.cs`), registrato in
`Quant.Studi/CatalogoContratti.cs`; il simulatore che le applica e' nella fase 4
([`il-simulatore-di-contratto.md`](4-il-simulatore-di-contratto.md)).

Le regole di una firma vera si scrivono nello studio, in `2-regole/` (per esempio
[`quant/studi/contratto-prop/2-regole/`](../studi/contratto-prop/2-regole/)), una pagina per prodotto, con tre obblighi:

- **solo dalle fonti ufficiali** della firma (centro assistenza, contratto, pagina dei prezzi), con
  l'indirizzo e la citazione letterale di ogni numero; niente siti di recensioni [TF §4.1];
- **con la data di lettura**, perche' le regole cambiano anche sui conti gia' venduti [TF §6]: prima
  di riusarle si rileggono;
- **le pagine che non concordano si scrivono tutte e due**, e nel codice diventano due contratti.

Contratti scritti: [`tradeify-growth-50k`](../studi/contratto-prop/2-regole/tradeify-growth-50k.md).
