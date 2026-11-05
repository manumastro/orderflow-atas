# L'Esito Dei Criteri Della Scala Dei Tre Tempi

Criteri: [`2026-11-05-la-scala-dei-tre-tempi.md`](2026-11-05-la-scala-dei-tre-tempi.md), SHA-256
`900e1ce727e7b296c46554f75dcd0bf4f38c41b0b432e1e9e9eaf4307733b291`, scritti prima di guardare NQ.
Eseguiti il 5 novembre 2026, solo dentro campione (13 aprile 2017 - 31 dicembre 2024, 1.994 giornate).

| | criterio | misurato | esito |
|---|---|---|---|
| K1 | V1: D1 e D2 con p <= 0,05, almeno 30 operazioni | 106 operazioni; D1 p = 0,858, D2 p = 0,783 | **smentita** |
| K2 | V1: Sharpe deflazionato >= 0,95, N = varianti nel registro | 0,072 con N = 5 (0,174 con N = 1, alla prima esecuzione) | **smentita** |
| K3 | misti piu' stretti degli allineati: mediana piu' bassa e Welch a una coda p <= 0,05, almeno 30 giornate per gruppo | 103 allineate, 1.767 miste; mediana 151,0 contro 165,5 punti; t = +1,92, p = 0,973 | **smentita** (al contrario: le miste sono piu' larghe) |

**Nota su K2.** Il criterio dice "N = le varianti nel registro al momento della validazione". V1 e' stata
validata due volte: la prima con N = 1 (0,174); la seconda, rieseguita dopo la correzione del testo del
rapporto (`Validatore.cs`, le regole da una strategia preparata), con N = 5 (0,072). I numeri della prova
non cambiano; cambia solo N. Tutte e due sono sotto 0,95.

**La seconda lettura di K3, senza soglia**: il range diviso per la media delle 20 casse prima ha mediana
0,877 negli allineati e 0,919 nei misti, t = +0,21: tolta la volatilita' recente, nessuna differenza.

I rapporti delle cinque varianti stanno in [`../risultati/`](../risultati/); la misura di K3 in
[`../../4-backtest/misura-tempo-misto/Program.cs`](../../4-backtest/misura-tempo-misto/Program.cs).
