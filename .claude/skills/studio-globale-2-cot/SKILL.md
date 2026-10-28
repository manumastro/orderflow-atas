---
name: studio-globale-2-cot
description: Fase 2 dello studio globale (NQ o ES) - il COT con la procedura del live di Fabio (Legacy, Non-Commercial, ultima variazione, skew del 30%, cross-confirm sui quattro indici, la data del report riportata sul prezzo). Legge i dati CFTC gia' raccolti e scrive 2-cot.md.
argument-hint: "[cartella del rapporto]"
---

# Fase 2: Il COT

Chi e' posizionato, e **dove stava il prezzo quando si e' posizionato**. Il rapporto segue
[`../studio-globale/modello-del-rapporto.md`](../studio-globale/modello-del-rapporto.md).

## Leggi La Procedura Prima Dei Dati

Apri, non riassumere a memoria:

- [`docs/research/metodo/i-tre-modelli-del-live-q1.md`](../../../docs/research/metodo/i-tre-modelli-del-live-q1.md),
  **sezione 9**: i sei passi del live, con i minuti;
- [`docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md`](../../../docs/research/metodo/il-framing-e-il-cot-si-leggono-insieme.md):
  il passo "si torna sul prezzo";
- [`docs/research/metodo/analisi-istituzionale.md`](../../../docs/research/metodo/analisi-istituzionale.md),
  solo le conclusioni: la metrica cross-index non ha retto; Legacy e TFF non sono sinonimi.

## I Tuoi Dati

`dati/cot.json` (CFTC alla fonte, gli stessi numeri di Tradingster; raccolto da `studio/cot/cot.py`):

- per ogni indice: il netto, le variazioni, i percentili a tre anni di netto, long e short;
- le ultime quattro settimane, ciascuna con skew, z a 52 settimane, percentile della variazione e il
  rollover in due regole (`settimana_di_rollover` sull'open interest, `rollover_calendario` sul
  calendario; `regola_rollover` le spiega);
- la **TFF del Nasdaq e dell'S&P 500** (`tff_nasdaq`, `tff_sp500`), per categoria: variazioni,
  percentili a tre anni di netto, long e short, z52 e le ultime quattro;
- il cross-confirm, rispetto all'indice **principale**: quello dello strumento dello studio
  (`strumento` e `principale`: NQZ6 il NASDAQ-100 mini, ESZ6 l'S&P 500 e-mini);
- **`prezzo_alle_date`**: dove stava lo strumento dello studio rispetto al valore della sua seduta di cassa nei giorni del
  report, con il **ritardo in prezzo** fino a oggi, dalla chiusura e dal POC (`prezzo_adesso`);
- **`settimane_dei_report`**: il profilo di cassa della settimana coperta da ogni report, dal giorno
  dopo il report precedente al martedi'. Contiene estremi, volume, delta, POC, VAL e VAH della
  settimana, e la distanza del prezzo di adesso dal POC. `prima_del_rollover` segna le sedute in cui
  il contratto non era ancora il principale (volume sottostimato); `mancanti` i giorni senza dati;
- la prossima pubblicazione, in ora italiana.

Per la scala dei POC di quei giorni, `dati/sedute-<strumento>.json`.

**Non hai Bash e non ricalcoli niente**: se ti serve una misura che non c'e', e' una lacuna del
collettore e la scrivi.

## I Sei Passi, Uno Per Uno

1. **Legacy.** 2. **Solo Non-Commercial.** La tabella dei quattro indici: long, short, netto, variazioni,
   percentili.
3. **Lo skew di fondo**: long contro short assoluti.
4. **L'ultima variazione**: chi ha messo il piede sull'acceleratore. Lo skew della variazione con soglia
   0,30 (convenzione nostra, dichiarala) e lo z: lo skew satura a +-1 quando le due variazioni hanno
   segno opposto, e allora conta lo z.
5. **Il cross-confirm** sui quattro indici, com'e'.
6. **La data sul prezzo**: per le ultime quattro date, dove stava lo strumento dello studio rispetto al valore della seduta.
   **Il prodotto del passo e' un livello**: il valore del giorno in cui il posizionamento si e' mosso di
   piu', con a cosa serve arrivarci.

Poi la **TFF** dell'indice principale come complemento, con la riserva che le categorie non coincidono con la
Legacy. **Le riserve sempre**: l'eta' del dato e il prossimo report (venerdi', 21:30 italiane in ora
legale), la settimana di rollover, il COT non dice *quando*.

## Scrivi `2-cot.md`

Negli agganci **Per il quadro**: il posizionamento contro il regime della macro, e i livelli del passo 6
da confrontare con muri e zero gamma.
