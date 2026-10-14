# Il Simulatore Di Contratto

*Aggiunta del framework.* Il transcript non tratta le prop firm. Il simulatore serve all'idea dei tre
paper (lo studio [`contratto-prop`](../studi/contratto-prop/1-idea.md)): applica le
regole di un contratto di prop firm alle sessioni di un trader e restituisce **i soldi veri del
compratore**, cioe' cio' che paga e cio' che incassa, non il saldo simulato [Y §2.1].

| pezzo | dove | fase |
|---|---|---|
| la forma del contratto: `Fase`, `Prelievi`, `Prezzi`, `Contratto` | [`quant/framework/Quant.Regole/Contratti/`](../framework/Quant.Regole/Contratti/) | 2, le regole |
| i contratti veri, un file ciascuno (`TradeifyGrowth50K`, `LimClassico`), e il catalogo | nello studio, [`quant/studi/contratto-prop/3-codice/`](../studi/contratto-prop/3-codice/); `framework/Quant.Studi/CatalogoContratti.cs` | 2 |
| le regole di una firma, con le fonti e la data | nello studio, [`quant/studi/contratto-prop/2-regole/`](../studi/contratto-prop/2-regole/) | 2 |
| le sessioni: browniane, storiche, costruite dalle barre | [`quant/framework/Quant.Motore/Contratti/Sessioni.cs`](../framework/Quant.Motore/Contratti/Sessioni.cs) | 4 |
| il simulatore | [`quant/framework/Quant.Motore/Contratti/SimulatoreContratto.cs`](../framework/Quant.Motore/Contratti/SimulatoreContratto.cs) | 4 |
| la forma chiusa di TF, il riepilogo, la verifica | [`quant/framework/Quant.Validazione/Contratti/`](../framework/Quant.Validazione/Contratti/) | 5 |

## Come Si Lancia

```bash
quant contratto verifica                                   # i criteri sigillati del simulatore
quant contratto tradeify-growth-50k --d 850                # sessioni browniane, nessun vantaggio
quant contratto tradeify-growth-50k --d 850 --deriva -118  # con una deriva per sessione
quant contratto tradeify-growth-50k --sorgente seduta --contratti 2       # sessioni vere di NQ
quant contratto tradeify-growth-50k --sorgente youngblood --contratti 8   # la strategia di Youngblood
quant contratto tradeify-growth-50k --d 850 --politica v4-f1   # una politica del compratore
quant contratto tradeify-growth-50k --d 850 --politiche        # una riga per ogni politica
```

Opzioni: `--prezzo`, `--reset` (i prezzi effettivi alla data), `--cicli 100000`, `--seme`,
`--orizzonte 1000` (sessioni del conto finanziato), `--max-prelievi N` (il conto simulato finisce
dopo N prelievi, per esempio con il passaggio al vivo), `--senza-limite` (toglie il limite
giornaliero, per misurarne l'effetto), `--controlli 78` (controlli per sessione delle sessioni
browniane).

**La politica del compratore** (`Politica.cs`, dal 14 ottobre 2026): per ciascuna fase, valutazione
e finanziato, una **condotta**: la scala rispetto alle sessioni della sorgente (deviazione, deriva e
costi crescono insieme) e un bracket sul P&L del giorno, stop e target. Le politiche stanno nello
studio che le prova, [`politica-per-fase`](../studi/politica-per-fase/1-idea.md), e si registrano in
`CatalogoContratti.Politiche`. Senza politica, ogni ciclo e' identico a prima (test).

## Cosa Simula

**Un ciclo** e' cio' che fa un compratore: compra la valutazione, la tenta; se fallisce paga il
reset e ritenta; quando passa tutte le fasi ha il conto finanziato, e lo usa finche' non tocca il
pavimento o finisce l'orizzonte. Il netto del ciclo e' incassato meno pagato. I cicli sono
indipendenti, ciascuno con il suo flusso del generatore: il risultato non dipende dal numero di thread.

**Una sessione** e' una sequenza di passi. Per ogni passo la sorgente da' il P&L dall'inizio della
sessione alla fine del passo e il peggiore toccato nel passo.

| sorgente | cosa e' | per cosa |
|---|---|---|
| `browniano` | moto browniano aritmetico, deviazione `d` e deriva per sessione, `controlli` passi; fra due passi non si guarda | il modello di TF e di Lim; la verifica |
| `seduta` | NQ dalle 9:30 alle 16:00, una posizione tenuta tutta la seduta, verso a caso | l'ipotesi di nessun vantaggio di TF §8.1 |
| `youngblood` | ingresso alle 10:00, target e stop di 100 tick, uscita alle 16:00, verso a caso | la strategia di Youngblood §2.2 |

Le sorgenti storiche pescano **blocchi di 5 sessioni consecutive** e il verso a caso a ogni sessione
[TF §8.1 - 8.2]. Long e short sono due percorsi calcolati separatamente: con stop e target nella
stessa barra vince lo stop, per tutti e due.

## Le Convenzioni

Come nel motore, ogni convenzione e' una scelta, e si dichiara accanto ai numeri.

| convenzione | scelta | perche' / effetto |
|---|---|---|
| ordine dentro un passo | limite giornaliero, poi pavimento, poi target | Lim §4.2: la regola giornaliera prima delle barriere cumulate |
| target | si controlla alla fine di ogni passo | chi lo raggiunge chiude; vedi "Il settimo parametro" |
| violazione in tempo reale | sul peggiore del passo | Tradeify: "net liquidation value touches it at any moment" |
| pavimento alla chiusura | si ricalcola sul saldo di fine sessione | EOD |
| limite giornaliero morbido | **si esce esattamente al limite** e si smette per la giornata | **ottimista**: Tradeify avverte che "losses may exceed the limit". Pesa molto per d grandi: si misura con `--senza-limite` |
| prelievo | appena le condizioni sono soddisfatte, a fine sessione, il massimo consentito (tetto, e mai sotto il pavimento) | il compratore piu' rapido; Tradeify permette di lasciare 1.400 $ di margine |
| richiesta di prelievo | blocca il pavimento al livello di blocco | Tradeify: "or immediately when you request a payout" |
| consistenza | sessione migliore dall'ultimo prelievo <= quota x profitto dall'ultimo prelievo | Tradeify, "Rules: Consistency Rule" |
| costi (sorgenti storiche) | 2,82 $ per MNQ a sessione: 1,82 $ di commissione Tradeify e 2 tick di slittamento | come il motore: 1 tick per esecuzione a mercato |
| tetto ai tentativi | 1.000 | un compratore che non passa mai smette |
| stop e target del giorno (politica) | esatti al livello; lo stop sul peggiore del passo, il target sulla chiusura del passo | ottimista come il limite giornaliero; il target sulla chiusura e' prudente |

## Il Settimo Parametro

La verifica (vedi [`il-framework-si-valida-prima.md`](5-il-framework-si-valida-prima.md#il-simulatore-di-contratto))
ha trovato un parametro che TF non elenca: **quanto spesso si controlla il target**. Con pavimento e
violazione alla chiusura, la forma chiusa di TF coincide con la simulazione solo se anche il target
si controlla alla chiusura (0,3207 contro 0,3203); se il target si guarda durante la sessione, la
probabilita' di passare sale del 12,5%. Con la violazione in tempo reale, come in Tradeify, il
problema non c'e' (scarto +0,3%).

## La Precisione

- **Sulle sessioni browniane** l'errore e' quello Monte Carlo, stampato accanto a P e al netto.
- **Sulle sessioni storiche no**: l'informazione sta nelle sessioni, non nei cicli [TF §8.2]. Con S
  sessioni e N sessioni per tentativo i tentativi indipendenti sono circa S / N, e il riepilogo lo
  stampa. Con le 22 sedute complete di NQ oggi in archivio sono **da 1 a 3**: i numeri storici
  sono descrittivi (criterio C5).
