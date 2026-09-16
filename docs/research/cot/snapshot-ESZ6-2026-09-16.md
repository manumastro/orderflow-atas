# COT TFF E-mini S&P 500, rilevazione 2026-09-08

Fonte: `tradingster.com/cot/futures/fin/13874A`, vista **Traders in Financial Futures**, variante
**futures only**.
Pubblicazione: venerdi' 11 settembre 2026. Rilevazione: **martedi' 8 settembre** a chiusura.
**Ritardo alla lettura: otto giorni** — mercoledi' 16 settembre.

Stato: **descrizione**. Nessuna regola operativa, nessun segnale.

## Cosa Manca, Dichiarato

A differenza del Nasdaq, **non esiste in questo repository una serie storica settimanale dell'ES**.
Quindi `z52` e `pct3a` **non sono calcolabili**: si legge la fotografia, non il livello dentro la
sua distribuzione. E' la distinzione fra **flusso e livello** della regola 2 del percorso: qui c'e'
solo il flusso, e il flusso da solo non dice se lo stato e' estremo.

Il **buco di prezzo** fra rilevazione e lettura non e' quantificato: il bridge ha in memoria due
sole sedute di ESZ6, quindi la chiusura dell'8 settembre non e' disponibile. Va recuperata prima di
usare questa rilevazione come contesto.

## La Rilevazione

`net = long - short`.

| Gruppo | Long | Δlong | Short | Δshort | Net | Δnet |
|---|---|---|---|---|---|---|
| Dealer/Intermediary | 213.184 | +11.225 | 898.526 | -24.180 | **-685.342** | +35.405 |
| Asset Manager | 1.153.305 | -3.488 | 240.944 | +18.331 | **+912.361** | -21.819 |
| Leveraged Funds | 155.517 | -9.794 | 496.621 | +13.746 | **-341.104** | -23.540 |
| Other Reportable | 56.464 | -683 | 68.432 | +5.362 | **-11.968** | -6.045 |
| Non Reportable | 263.462 | +6.763 | 137.409 | -9.236 | **+126.053** | +15.999 |

## Due Osservazioni, Senza Conclusione

### La struttura e' quella tipica dell'indice largo

Asset Manager massicciamente long (+912.361) contro Dealer massicciamente short (-685.342): e' la
fotografia strutturale dell'S&P, dove i gestori istituzionali sono lunghi per mandato e i dealer
stanno dall'altra parte per fornire il servizio. **Non e' una opinione direzionale di nessuno dei
due**, ed e' un errore leggerla come tale.

Per confronto, sul Nasdaq l'8 settembre gli Asset Manager erano +75.526 e i Dealer -66.712: stessa
forma, scala dodici volte piu' piccola. **Le due serie non sono confrontabili in valore assoluto.**

### Nella settimana tutti e tre i grandi gruppi si sono mossi nella stessa direzione

Asset Manager -21.819, Leveraged Funds -23.540, Other Reportable -6.045: **riduzione di esposizione
long su tutti i reportable**, contro un Dealer che copre (+35.405) e un Non Reportable che compra
(+15.999). I piccoli hanno preso il rischio che i grandi hanno lasciato.

Questo e' l'unico elemento della rilevazione che descriva un movimento e non una struttura. **Senza
la serie storica non si puo' dire se sia grande o ordinario**, e finche' non lo si sa non se ne
ricava niente.
