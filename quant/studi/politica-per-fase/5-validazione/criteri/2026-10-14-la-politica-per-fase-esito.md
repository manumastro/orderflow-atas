# Esito: La Politica Per Fase

14 ottobre 2026. Criteri: [`2026-10-14-la-politica-per-fase.md`](2026-10-14-la-politica-per-fase.md),
sha256 `cf64729061496a6fcfe96cc604c77571c50da35ded853693efa12956a98c0d82` (verificato: invariato).
Scritto cosi' com'e', anche dove non passa.

Comandi: `quant contratto tradeify-growth-50k --d 850 --politiche` piu' `--deriva -6` (b),
`--max-prelievi 1` (c), `--max-prelievi 3` (d), `--senza-limite` (e). Le tabelle intere sono nello
[studio](../risultati/2026-10-14-la-politica-per-fase.md).

**P\*** (la politica per fase con N piu' alto in (a), esclusa `v1-f0`): **`v4-f1`**, scala 4 in
valutazione e 0,5 nel finanziato, N = 442 $ (SE 6).

| | quantita' | P\* | F\* nello scenario | differenza | SE_diff | soglia | esito |
|---|---|---|---|---|---|---|---|
| K0 | politica neutra = nessuna politica | | | | | nessun ciclo diverso | **passa** (test); verifica 13 su 18 come prima; passeggiata casuale: nessuna strategia passa |
| K1 | (a) | 442 | `fissa-4` 563 (SE 12) | **-121** | 13 | > 4 SE_diff | **SMENTITO** |
| K2 | (e) senza limite | 422 | `fissa-1` 272 (SE 8) | +150 | 10 | > 3 SE_diff | passa |
| K3 | (c) un prelievo | 57 | `fissa-1` -117 (SE 2) | +174 | 3 | > 3 SE_diff | passa; N(P\*, c) > 0 |
| K4 | (b) costi | 367 | `fissa-4` 444 (SE 11) | **-77** | 13 | > 3 SE_diff | **SMENTITO** |

**L'incrocio non regge come scritto**: K1 e K4 sono smentiti.

## La Diagnosi

Non cambia l'esito; dice cosa l'ha prodotto.

La dimensione fissa che batte la politica e' `fissa-4`, la piu' grande, e **solo** negli scenari con
l'uscita esatta al limite giornaliero (a, b). Senza quell'ipotesi (e) `fissa-4` scende da 563 a 35 $; con
un solo prelievo (c) e' -151 $, la peggiore di tutte. Nel finanziato, a scala 4, si tocca spesso il
limite di 1.250 $ (poi 2.000 $), e uscirne esattamente al limite e' un tetto, non una misura: lo
studio del contratto l'aveva gia' detto. La scheda nominava questa ipotesi fra quelle che lo
ucciderebbero; il criterio K1 pero' la lascia dentro lo scenario di riferimento. **Non e' un criterio
scritto male**: e' un risultato. Sotto l'ipotesi piu' favorevole sul limite, la dimensione grande
fissa vale di piu' in media, con il 91% dei cicli in perdita.

Un nuovo criterio, che sceglie P\* e F\* senza l'ipotesi dell'uscita esatta, andrebbe scritto e
sigillato prima di rieseguire; non e' stato fatto.
