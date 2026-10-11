# Il Momento Intraday

**Stato**: codificata, non validata su dati veri (manca la storia lunga di NQ)
**Strategia nel catalogo**: `momento-intraday`, versione 1

## Il Paper

Gao, Han, Li, Zhou, *Market Intraday Momentum*, Journal of Financial Economics, 2018; anche su
SSRN. Citato nel transcript `[MC · 34:03 - 34:25]`. **Paper non ancora letto per intero**: il finding
qui sotto e' quello riportato nel transcript.

## Il Finding

Il rendimento dalla chiusura del giorno prima alle 10:00 (il gap della notte piu' la prima mezz'ora,
9:30-10:00) predice il rendimento dell'ultima mezz'ora della seduta regolare `[MC · 35:06 - 35:56]`.

## Il Perche'

A mercato chiuso escono notizie, utili, posizionamenti globali. Gli ordini che ne nascono si
scontrano nella prima mezz'ora e **rivelano lo squilibrio** fra compratori e venditori. Il mercato
non lo digerisce tutto in trenta minuti e lo porta nel resto della giornata `[MC · 36:08 - 37:11]`.

## Cosa La Ucciderebbe

Molti operatori, fondi e fondi pensione, possono operare solo dopo le 9:30 ET. Se potessero farlo
prima, lo squilibrio non si concentrerebbe nella prima mezz'ora e l'effetto sparirebbe
`[MC · 46:23 - 47:23]`.

## Le Regole Da Provare

Dal transcript `[MC · 38:13 - 40:31]`:

- **ingresso**: long se una barra chiude sopra il massimo del range 9:30-10:00 ET;
- **uscita**: stop al minimo del range, target a 1 o 2 volte il rischio, chiusura alle 15:30 ET se
  non si tocca nessuno dei due;
- **dimensione**: un contratto in sviluppo, poi volatilita' obiettivo `[MC · 40:39 - 42:51]`.

Indicazioni del transcript da verificare, non da assumere:
- **solo long** funziona meglio: rotto il livello al ribasso, i compratori tornano e assorbono lo
  squilibrio `[MC · 43:16 - 43:55]`, e la cosa reggeva anche nel 2022 `[MC · 49:04 - 49:09]`;
- 30 minuti e' la finestra ottimale, e rapporto 1:1 o 1:2 l'ottimo `[MC · 48:32 - 49:19]`.

Varianti dichiarate, nell'ordine:

1. `rr=1`, `soloLong=si` (default);
2. `rr=2`;
3. `soloLong=no`, per misurare il lato short che il transcript sconsiglia;
4. `dimensione=volatilita`, solo sulla variante migliore.

Idea derivata nel transcript `[MC · 1:28:06 - 1:28:31]`: un opening range breakout **di notte**, se
li' stanno i movimenti direzionali. Non ancora codificata.

## Le Prove

Nessuna su NQ. Sul mercato sintetico (`SINT`) la strategia **non** supera i cancelli su una
passeggiata casuale, come deve: vedi la taratura in
[`../il-metodo-quantitativo.md`](../il-metodo-quantitativo.md).
