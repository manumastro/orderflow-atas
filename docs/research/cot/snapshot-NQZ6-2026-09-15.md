# COT Nasdaq, rilevazione 2026-09-15

Fonte: `tradingster.com`, viste **Traders in Financial Futures** (`/cot/futures/fin/209742`) e
**Legacy** (`/cot/legacy-futures/209742`).
Pubblicazione: **venerdi' 18 settembre 2026**. Rilevazione: **martedi' 15 settembre a chiusura**.

Stato: **descrizione**. Nessuna regola operativa, nessun segnale.

## Come Sono Stati Presi, E Cosa Manca

La procedura documentata estrae la **serie intera** dal `dataProvider` di amCharts con Playwright
(vedi `tradingster-cross-index-2026-09-11.md`). **Qui non e' stato possibile**: il browser di
Playwright non e' installato su questa macchina (`chrome-for-testing` assente). Ho quindi preso
**solo la tabella in cima alla pagina**, cioe' l'ultimo punto della serie.

**Conseguenza, dichiarata:** i CSV in `docs/research/data/` **non sono stati aggiornati** e si
fermano al 2026-09-08 (TFF) e al 2026-09-01 (legacy). `z52` e `pct3a` qui sotto sono calcolati
sulla serie su disco **piu'** questo punto, e sono validi; l'OHLC settimanale di Tradingster per
questa settimana **non ce l'ho** e non l'ho inventato.

## La Rilevazione — Traders in Financial Futures

| Gruppo | Long | Δlong | Short | Δshort | Net | Δnet | z52 | pct3a |
|---|---|---|---|---|---|---|---|---|
| Dealer | 52.437 | -6.104 | 131.666 | +6.413 | **-79.229** | -12.517 | -0,69 | 16% |
| Asset Manager | 105.199 | -5.853 | 38.388 | +2.862 | **+66.811** | -8.715 | -0,99 | 31% |
| Leveraged Funds | 52.680 | +5.006 | 59.067 | **-20.479** | **-6.387** | **+25.485** | +0,93 | **90%** |
| Other Reportable | 11.407 | -1.277 | 7.742 | -2.007 | +3.665 | +730 | +0,20 | 43% |
| Non Reportable | 49.391 | +2.990 | 34.251 | +7.973 | **+15.140** | -4.983 | +0,91 | **87%** |

**Prova interna superata:** la somma delle Δlong e quella delle Δshort valgono entrambe **-5.238**.
Long e short totali devono muoversi insieme, e lo fanno: la tabella e' interna coerente.

## La Rilevazione — Legacy

| | Long | Δ | Short | Δ | Net | Δnet |
|---|---|---|---|---|---|---|
| Non-Commercial | 85.062 | -865 | 51.344 | **-13.688** | **+33.718** | **+12.823** |
| Commercial | 175.896 | +21.670 | 224.754 | +29.510 | -48.858 | -7.840 |
| Non-Reportable | 49.391 | +2.990 | 34.251 | +7.973 | +15.140 | -4.983 |

Open interest **325.784, +30.633**. Gli spread Non-Commercial salgono di +6.838 a 15.435.

## Il Fatto Unico Della Settimana

**Il lato corto speculativo e' stato coperto.** Leveraged Funds -20.479 short, Non-Commercial
legacy -13.688 short. Il net dei Leveraged Funds passa da -31.872 a **-6.387**: quasi piatto, e al
**90° percentile su tre anni**. Contemporaneamente l'open interest **sale** di 30.633 — non e'
liquidazione, e' rotazione: i corti escono e qualcun altro entra.

Dall'altro lato: **Asset Manager al 31° percentile** (riducono i long) e **Dealer al 16°**. Il
retail (Non Reportable) resta long al **87° percentile**.

## Dove Stava Il Prezzo Il 15 Settembre

E' il passo del live (`fabio_live_charting_1_q1.txt`, righe 1162-1183): si prende la data del
posizionamento e si torna sul grafico a vedere **dove stava il prezzo rispetto al valore di
allora**. NQZ6, cassa 13:30Z-20:00Z del 15 settembre, 270.650 lotti:

```text
15 set   O 29.420,75   H 29.453,50   L 29.207,50   C 29.269,50   delta +5.468
         POC 29.250    valore 29.215 - 29.314
         la seduta SCENDE di 151 punti dall'apertura E CHIUDE DENTRO IL PROPRIO VALORE,
         19,50 punti sopra il POC, con delta +5.468
```

**I corti sono stati coperti nella settimana in cui il prezzo ha fatto la sua area di valore piu'
bassa** — 29.250 e' il POC piu' basso di tutta la serie post-rollover — **e quella seduta ha
assorbito**: scende, ma il delta e' +5.468. Non e' il caso del 13 gennaio del transcript, dove la
distribuzione coincideva col **bordo alto** e il prezzo lo rifiutava. Qui e' il rovescio: copertura
sul **fondo**, con acquisto aggressivo dentro la discesa.

**Il prodotto operativo e' un livello, non una tesi:** **POC 29.250**, bordo basso **VAL 29.215**.
E' il posto sul profilo dove il posizionamento e' stato misurato. Da li' il prezzo ha fatto
**+1.590 punti in quattro sedute**, fino a 30.862,75 del 21 settembre.

## Il Cross Confirm Non Conferma

Righe 1187-1197 del live: il COT di un indice si incrocia con i correlati. Δnet Non-Commercial,
stessa settimana, stessa fonte legacy:

| indice | Δnet | net | verso |
|---|---|---|---|
| **NASDAQ** | **+12.823** | +33.718 | coprono corto |
| **S&P 500** | **-22.799** | -116.732 | **aggiungono corto** |
| Dow | -1.423 | +14.903 | aggiungono corto |
| Russell | +10.485 | -72.350 | coprono corto |

**Due e due, e i due grandi vanno in direzioni opposte.** La copertura sul Nasdaq **non e'
confermata dall'S&P**, che nella stessa settimana ha aggiunto 12.235 corti e tolto 10.564 long.
Un quadro COT che non si conferma incrociato vale meno di uno che si conferma, e va detto.

## Il Ritardo, Che E' La Cosa Piu' Importante

**Questi dati sono di martedi' 15 settembre.** Da allora il mercato ha fatto quattro sedute e
**+1.590 punti**, compresa la giornata di trend del 21 (+558 punti, delta +4.332). **Il COT non
ha visto niente di tutto questo.** La prossima pubblicazione e' **venerdi' 25 settembre**, con le
posizioni di **martedi' 22**, cioe' oggi.
