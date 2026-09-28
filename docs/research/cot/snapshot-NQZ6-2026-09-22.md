# COT Nasdaq, rilevazione 2026-09-22

Fonte: `tradingster.com`, viste **Traders in Financial Futures** (`/cot/futures/fin/209742`) e
**Legacy** (`/cot/legacy-futures/209742`); per il cross confirm la legacy dell'E-mini S&P 500
(`/cot/legacy-futures/13874A`).
Pubblicazione: **venerdi' 25 settembre 2026**. Rilevazione: **martedi' 22 settembre a chiusura**.
Scaricato lunedi' 28 settembre, con WebFetch sulla tabella in cima alla pagina.

Stato: **descrizione**. Nessuna regola operativa, nessun segnale.

## Come Sono Stati Presi, E Cosa Manca

Come per il 15 settembre, **solo l'ultimo punto** della serie, senza Playwright. I CSV in
`docs/research/data/` **non sono aggiornati** e si fermano al 2026-09-08. Qui `z52` e `pct3a` sono
calcolati sulla serie su disco **piu'** il punto del 15 e questo del 22. L'OHLC settimanale di
Tradingster **non c'e'**.

**Settimana di scadenza.** Il 18 settembre e' scaduto il contratto di settembre: l'open interest
del Nasdaq scende di **39.463**, quello dell'S&P di **555.866**. Le variazioni di questa settimana
contengono il rollover, e vanno lette con quella riserva.

## La Rilevazione — Traders in Financial Futures

| Gruppo | Long | Δlong | Short | Δshort | Net | Δnet | z52 | pct3a |
|---|---|---|---|---|---|---|---|---|
| Dealer | 57.512 | +5.075 | 115.916 | -15.750 | **-58.404** | +20.825 | 0,00 | 49% |
| Asset Manager | 106.240 | +1.041 | 34.020 | -4.368 | **+72.220** | +5.409 | -0,31 | 44% |
| Leveraged Funds | 54.033 | +1.353 | 84.716 | **+25.649** | **-30.683** | **-24.296** | -0,07 | 61% |
| Other Reportable | 15.590 | +4.183 | 8.336 | +594 | +7.254 | +3.589 | | |
| Non Reportable | 40.610 | -8.781 | 30.997 | -3.254 | +9.613 | -5.527 | +0,05 | 54% |

**Prova interna superata:** la somma delle Δlong e quella delle Δshort valgono entrambe **+2.871**.
I valori di partenza coincidono con lo snapshot del 15: per esempio 59.067 + 25.649 = 84.716.

## La Rilevazione — Legacy

| | Long | Δ | Short | Δ | Net | Δnet |
|---|---|---|---|---|---|---|
| Non-Commercial | 88.507 | +3.445 | 32.357 | **-18.987** | **+56.150** | **+22.432** |
| Commercial | 152.580 | -23.316 | 218.343 | -6.411 | -65.763 | -16.905 |
| Non-Reportable | 40.610 | -8.781 | 30.997 | -3.254 | +9.613 | -5.527 |

Open interest **286.321, -39.463**. Gli spread Non-Commercial scendono di -10.811 a 4.624.

## Il Fatto Unico Della Settimana

**I Leveraged Funds hanno riaperto i corti che avevano coperto.** Il 15 settembre avevano
ricomprato 20.479 short; il 22 ne aggiungono **25.649**. Il net torna da -6.387 a **-30.683**, cioe'
al valore di due settimane prima (-31.872). Il 90° percentile del 15 e' rientrato al 61°: **nessun
gruppo e' a un estremo**, tutti fra il 44° e il 61° percentile su tre anni.

**Le due viste non dicono la stessa cosa.** Nella legacy i Non-Commercial *riducono* gli short di
18.987, mentre nella TFF i Leveraged Funds li *aumentano* di 25.649. Le categorie non coincidono (i
Non-Commercial comprendono anche Asset Manager e altri), e con il rollover in mezzo la differenza
non si puo' attribuire. Resta una contraddizione dichiarata, non una lettura.

## Dove Stava Il Prezzo Il 22 Settembre

NQZ6, cassa 13:30Z-20:00Z del 22 settembre, dal giro d'orizzonte:

```text
22 set   30.764,25 -> 31.029,25   POC 30.920   valore 30.890-30.980   delta +2.427
         chiude SOPRA il proprio valore, valore SU e staccato dal giorno prima
```

**I corti sono stati aperti sul massimo della settimana**, nella seduta che chiudeva sopra il suo
valore. E' il caso rovescio del 15, dove i corti si coprivano sull'area di valore piu' bassa. Da
allora il prezzo e' sceso: il 28 settembre, alle 11 italiane, NQ sta a 30.600, **sotto** tutto il
valore della settimana (30.650-30.995).

**Il prodotto operativo e' un livello, non una tesi:** il valore del 22, **30.890-30.980, POC
30.920**. E' l'area dove i Leveraged Funds si sono posizionati short.

## Il Cross Confirm Non Conferma

Δnet Non-Commercial legacy, stessa settimana: NASDAQ **+22.432**, **S&P 500 -32.767** (long
-22.247, short +10.520). Come il 15, i due grandi vanno in direzioni opposte, e l'S&P ha dentro
un rollover da mezzo milione di contratti.
