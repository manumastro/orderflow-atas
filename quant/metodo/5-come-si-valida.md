# Come Si Valida

**La fase 5 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#5-la-validazione)),
in due parti `[MC · 9:11 - 9:26]`: **il test di divisione**, dentro e fuori campione (5a), e **il
Monte Carlo** (5b). Il transcript non dice quando una strategia e' validata: il framework lo dice
con i cancelli, scritti prima di guardare, e lo dichiara come aggiunta.

Dove sta:

- **il codice**: il progetto [`quant/framework/Quant.Validazione/`](../framework/Quant.Validazione/):
  la divisione e il registro (`Registro.cs`), il modello nullo (`ModelloNullo.cs`), il Monte Carlo
  (`MonteCarlo.cs`), lo Sharpe deflazionato (`SharpeDeflazionato.cs`), i cancelli (`Validatore.cs`),
  il rapporto (`Rapporto.cs`), il mercato sintetico (`Sintetico.cs`), e in `Contratti/` la forma
  chiusa di Tomas Fernandez, il riepilogo e la verifica del simulatore di contratto. Il generatore
  casuale e la normale stanno nella base comune (`Quant.Comune/Calcolo/`);
- **i test**: [`quant/framework/Quant.Test/`](../framework/Quant.Test/);
- **il registro delle prove e le divisioni**: [`quant/registro/`](../registro/), **in git**, comuni a
  tutti gli studi;
- **i risultati e i criteri sigillati di uno studio**: nella sua cartella,
  `quant/studi/<idea>/5-validazione/risultati/` (i rapporti di `quant valida`) e
  `5-validazione/criteri/`;
- **la prova del framework** sul mercato sintetico, da ripetere dopo ogni modifica:
  [`5-il-framework-si-valida-prima.md`](5-il-framework-si-valida-prima.md).

## Come Si Lancia

```bash
quant divisione --simbolo NQ               # mostra il confine dentro/fuori
quant divisione --simbolo NQ --fissa       # lo fissa, UNA volta: solo su richiesta dell'utente
quant valida <strategia> [--p ...]         # dentro: nulli, Monte Carlo, Sharpe deflazionato, cancelli D
quant valida <strategia> [--p ...] --fuori # fuori: UNA volta, a regole congelate, solo su richiesta
```

Opzioni: `--repliche 1000` (del nullo), `--simulazioni 10000` (del Monte Carlo).

## 5a. Dentro E Fuori Campione

Il transcript: si lavora sulle regole **solo dentro campione** (per esempio 80%), poi le stesse
regole, **congelate**, si danno ai dati mai visti; buoni risultati dentro e fuori vogliono dire che
non c'e' stato overfitting `[MC · 9:26 - 12:01]`. Nella PEAD lo mostra nel lavoro vero: dentro si
prova lo stop, fuori si controlla se era overfitting `[MC · 1:14:21 - 1:15:18]`.

Le aggiunte del framework, perche' il transcript non dice cosa fare quando la regola si rompe:

- **la divisione si fissa una volta per simbolo e non si sposta** (`divisioni.json`). Ricalcolata a
  ogni download, porterebbe pezzi di fuori campione dentro senza che nessuno se ne accorga. I dati
  scaricati dopo vanno fuori;
- **il fuori campione si consuma**: `valida --fuori` si lancia una volta per variante, e il
  registro sa se un'altra variante della stessa strategia l'ha gia' visto (cancello F4). Guardato
  due volte, il fuori e' diventato dentro;
- **il registro delle prove** (`prove.jsonl`): ogni `prova` e ogni `valida` vi aggiungono una riga
  con strategia, versione, impronta, parametri, campione e risultato. Serve a **contare le varianti
  provate**.

**La divisione di NQ non e' ancora fissata**, apposta: l'archivio ha 50 giornate (3 agosto - 9
ottobre 2026). Si fissa quando la storia lunga e' scaricata.

## Il Modello Nullo

*Aggiunta del framework*: il transcript non confronta la strategia con un ingresso a caso. Nella
fase 40R le condizioni facevano 49% e il nullo 47%: senza confronto, il 49% sembrava un risultato.

Il nullo rifa ogni operazione vera con le stesse uscite, gli stessi contratti e gli stessi costi,
togliendo una cosa sola:

- **il quando**: giornata e minuto a caso dentro il campione e dentro la finestra della strategia,
  stesso verso, **stessa esposizione** (l'uscita a tempo si sposta con l'ingresso). Toglie il beta:
  su un campione salito per caso anche il nullo long guadagna. **La giornata si pesca a caso**:
  spostare un'operazione dentro la sua giornata le fa catturare il movimento che l'ha generata;
- **il verso**: stesso istante, verso a caso. Su una strategia che va solo long misura il beta, non
  un vantaggio: **conta solo per le strategie che scelgono il verso**.

Il p e' (1 + repliche con netto totale almeno pari a quello vero) / (1 + repliche).

## 5b. Il Monte Carlo

Il transcript `[MC · 12:11 - 18:17]`:

- **rimescolamento**: stesse operazioni, ordine diverso; stesso finale, percorso diverso. Dice
  quanto si dipendeva dall'ordine e da' una prima idea dei drawdown;
- **ricampionamento**: alcune operazioni compaiono piu' volte e altre per niente, 10.000-20.000
  simulazioni; cambia anche il finale;
- l'uscita e' una **distribuzione degli esiti**, e i **metri per il vivo**: se dopo 10 operazioni il
  drawdown supera cio' che aveva il 5% di probabilita', e' un segnale d'allarme.

Nel framework (`MonteCarlo.cs`): rimescolamento, ricampionamento delle operazioni e, *aggiunta*,
**ricampionamento per giornata a blocchi di 5**, perche' pescare operazioni una per una sottostima
le code. Il rapporto stampa finale p5/p50/p95, probabilita' di perdita, drawdown p50/p95/p99, serie
di perdite, e **i metri: il drawdown p50/p95/p99 entro 10, 20, 50 e 100 operazioni**.

Il Monte Carlo **non ha un cancello**: il transcript lo usa per leggere la forza del vantaggio e per
i metri del vivo.

## Lo Sharpe Deflazionato

*Aggiunta del framework* (Bailey e Lopez de Prado, 2014): provando cento varianti, la migliore ha uno
Sharpe alto anche se nessuna vale niente. Lo Sharpe deflazionato e' la probabilita' che lo Sharpe
vero sia sopra quello atteso dal massimo di N prove senza vantaggio, corretta per asimmetria e
curtosi. **N sono le varianti nel registro**: per questo ogni prova ci finisce.

## I Cancelli

Una validazione **supera i cancelli** se passano tutti. Superarli non approva una regola operativa:
dice che la prova non l'ha smentita. Le soglie stanno in `Validatore.cs` e qui, e si cambiano
insieme.

| | condizione | soglia | fase |
|---|---|---|---|
| D1 | dentro: batte il nullo del quando | p <= 0,05 | aggiunta |
| D2 | dentro: batte il nullo del verso (solo se la strategia sceglie il verso) | p <= 0,05 | aggiunta |
| D3 | dentro: Sharpe deflazionato per le varianti nel registro | >= 0,95 | aggiunta |
| F1 | fuori: operazioni | >= 30 | aggiunta |
| F2 | fuori: netto | > 0 | 5a, "rende ancora bene fuori" |
| F3 | fuori: Sharpe annuo | >= 50% di quello dentro | 5a, "confrontare dentro e fuori" |
| F4 | fuori: mai guardato da un'altra variante | intatto | 5a, "regole congelate" |

**Il limite noto**: D1 ha poca potenza sulle anomalie di esposizione (una strategia che *e'* una
finestra, come la notte). Li' la prova giusta e' il confronto con il complemento
(`anomalia-notte --p parte=notte` contro `--p parte=giorno`), che non e' ancora un comando:
[`il-framework-si-valida-prima.md`](5-il-framework-si-valida-prima.md).

## I Criteri Sigillati

*Aggiunta del framework*, da Tomas Fernandez (2026, App. B): per ogni affermazione da provare si
scrivono **prima di guardare i dati** la quantita', il campione minimo e la soglia che la
smentirebbe, in un file nello studio, `5-validazione/criteri/` (il primo: [`quant/studi/contratto-prop/5-validazione/criteri/`](../studi/contratto-prop/5-validazione/criteri/)); se ne calcola lo SHA-256 (`sha256sum`) e lo
si registra nella scheda dell'idea e in `FabioOrderFlow/progress.txt`. Il file non si modifica piu'.

- **L'esito si scrive cosi' com'e'**, in un file accanto, anche dove non passa.
- **Un criterio sbagliato non si corregge**: se ne scrive uno nuovo, che cita il vecchio, prima di
  rieseguirlo. Sigillare impedisce di spostare il criterio dopo aver visto i dati, non di sbagliarlo:
  nel primo uso, due criteri su quattro erano scritti male
  ([`quant/studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md`](../studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md)).
- I cancelli D e F di questo documento sono gia' scritti prima, ma non sigillati: sigillarli per ogni
  strategia e' il passo successivo.
