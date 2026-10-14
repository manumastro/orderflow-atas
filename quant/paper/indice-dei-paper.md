# Indice Dei Paper

I paper da cui vengono le idee (fase 1), e quelli da cui il framework prende un metodo. La fonte
del metodo, il transcript, sta in [`quant/metodo/fonti/`](../metodo/fonti/). Come si cercano, si scaricano e si
archiviano: [`quant/metodo/1-come-si-cercano-le-idee.md`](../metodo/1-come-si-cercano-le-idee.md).

## Come Sono Organizzati

```text
paper/
  indice-dei-paper.md                 questo file: tutti i paper, per filone, con lo stato
  <filone>/                           un filone di ricerca: prop-firm, momento-intraday, ...
    <chiave>/                         un paper: autore-anno-titolo-breve
      analisi.md                      la scheda bibliografica (in testa) e la nostra lettura
      <chiave>.pdf                    il PDF letto, fuori da git; il suo sha256 e' nella scheda
```

- **Il filone** raggruppa i paper che rispondono alla stessa domanda. Un paper ha un filone
  principale; se ne tocca altri, la scheda li elenca.
- **La chiave** e' il nome stabile del paper: cognome del primo autore, anno, titolo breve, in
  minuscolo e con i trattini. E' anche il nome della cartella e del PDF, e si cita cosi' nelle schede
  delle idee.
- **Il ruolo**: `idea` se il paper porta un finding da provare (fase 1), `metodo` se porta una
  tecnica che il framework usa (per esempio lo Sharpe deflazionato nella fase 5).
- **Lo stato**: trovato -> scaricato -> letto (per intero) -> analizzato. Un paper solo trovato sta
  in questo indice e non ha ancora una cartella.
- **Lo sha256 del PDF** dice quale versione e' stata letta: su SSRN i paper vengono rivisti, e
  un'analisi vale per la versione che ha letto.
- **Paper e studi sono separati**: uno studio puo' venire da piu' paper (i tre sulle prop firm fanno
  lo studio `contratto-prop`) e un paper puo' servire a piu' studi. La scheda dello studio elenca i
  paper; la scheda del paper elenca gli studi.

## prop-firm

Il valore del contratto di una prop firm per chi lo compra. Tre paper indicati dall'utente l'11
ottobre 2026, tutti preprint su SSRN, non revisionati. Studio: [`contratto-prop`](../studi/contratto-prop/1-idea.md).

| sigla | paper | autore, versione | ruolo | stato |
|---|---|---|---|---|
| TF | [*Valuing Proprietary Trading Firm Evaluation Contracts*](prop-firm/tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts/analisi.md) | Marti Tomas Fernandez, agosto 2026 | idea, e metodo (forma chiusa, sigillo dei criteri) | analizzato |
| L | [*The Price of a Funded Account*](prop-firm/lim-2026-price-of-a-funded-account/analisi.md) | Boon Chuan Lim, 25 luglio 2026 | idea | analizzato |
| Y | [*The Inherent Value of Retail Prop Firms for Fixed-Size Trading Strategies*](prop-firm/youngblood-2026-inherent-value-of-retail-prop-firms/analisi.md) | Zachary Youngblood, 21 agosto 2026 | idea | analizzato |
| | *Phantom Generosity: Contract Design and Value in the Market for Proprietary-trading Evaluations* ([SSRN 7184138](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7184138)) | Boon Chuan Lim | idea | trovato |
| | *Prop-Firm Challenges: A Barrier Model of Pass Rates and Expected Value* ([SSRN 7445798](https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7445798)) | Ruben Villahermosa | idea | trovato |

Lo studio che li usa: [`tradeify-growth-50k`](../studi/contratto-prop/5-validazione/risultati/2026-10-11-tradeify-growth-50k.md).

## Gli Esempi Del Transcript

Citati nel transcript, **non ancora cercati**: i titoli sono quelli pronunciati nel video e vanno
verificati. Il filone e' quello che avranno quando si scaricano.

| filone | paper | ruolo | stato |
|---|---|---|---|
| momento-intraday | *Market Intraday Momentum* (2018) | idea | da cercare |
| vwap | *VWAP: The Holy Grail for Day Trading Systems* | idea | da cercare |
| notte | *The Return Difference Between Trading and Non-Trading Hours: Like Night and Day* (2008) | idea | da cercare |
| pead | i tre paper sulla PEAD (1968, 1989, 2006) | idea, fuori scope (azioni) | da cercare |
| dimensione | *Volatility Managed Portfolios*; *The Impact of Volatility Targeting* | metodo (fase 2) | da cercare |

## Metodo

Paper da cui il framework prende una tecnica, citati nel codice ma **non ancora scaricati**:
Bailey e Lopez de Prado (2014), lo Sharpe deflazionato (fase 5); Broadie, Glasserman e Kou (1997),
la correzione per il monitoraggio discreto (fase 5, via TF).
