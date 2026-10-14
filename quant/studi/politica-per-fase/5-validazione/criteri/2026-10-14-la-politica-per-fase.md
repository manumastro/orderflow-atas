# Criteri Sigillati: La Politica Per Fase

Scritti il 14 ottobre 2026, **prima** di scrivere il codice delle politiche e prima di qualunque
simulazione. Il file non si modifica: il suo SHA-256 e' registrato nella scheda
`quant/studi/politica-per-fase/1-idea.md` e in `FabioOrderFlow/progress.txt`. Un criterio sbagliato
si corregge con un file nuovo, che cita questo e dice perche'.

Contratto: `tradeify-growth-50k` (regole lette l'11 ottobre 2026). Sessioni browniane, d = 850 $ di
riferimento, 78 controlli, 100.000 cicli, seme 1, orizzonte finanziato 1.000 sessioni. Le politiche
e gli scenari (a)-(e) sono quelli della scheda. N e' il netto medio per ciclo del compratore
(incassato meno pagato); SE il suo errore standard Monte Carlo; per una differenza fra due politiche,
SE_diff = radice(SE_1^2 + SE_2^2).

**La politica migliore** P* e' quella con N piu' alto nello scenario (a) fra le 12 politiche per
fase (`v*-f*`, esclusa `v1-f0`). **La fissa migliore** F*(x) e' quella con N piu' alto fra `fissa-1`,
`fissa-2`, `fissa-4` **nello scenario x**. P* si sceglie una volta, in (a), e non si sceglie di nuovo
negli altri scenari.

## K0. Il simulatore non cambia senza politica

- **Quantita'**: con la politica neutra (scala 1, niente bracket) ogni ciclo coincide con quello senza
  politica, stesso seme.
- **Smentito se**: un solo ciclo diverso (test).
- In piu', dopo la modifica: i test, `quant contratto verifica` identico a prima (13 righe su 18
  passano), la passeggiata casuale senza strategie che passano.

## K1. L'incrocio aggiunge qualcosa

- **Quantita'**: N(P*, a) - N(F*(a), a).
- **Smentito se**: <= 4 SE_diff. La soglia e' 4 e non 2 perche' P* e' il massimo di 12 e F* di 3: il
  massimo di piu' varianti e' gonfiato dal caso.

## K2. Non dipende dall'uscita esatta al limite giornaliero

- **Quantita'**: N(P*, e) - N(F*(e), e), senza limite giornaliero.
- **Smentito se**: <= 3 SE_diff.

## K3. Regge se il conto finisce al primo prelievo

- **Quantita'**: N(P*, c) - N(F*(c), c).
- **Smentito se**: <= 3 SE_diff. Si scrive anche il segno di N(P*, c): sotto zero il contratto non
  vale il prezzo nemmeno con la politica.

## K4. Regge ai costi

- **Quantita'**: N(P*, b) - N(F*(b), b), deriva -6 $ a sessione al riferimento, moltiplicata per la scala.
- **Smentito se**: <= 3 SE_diff.

## K5. Descrittivo, senza soglia

Per ogni politica e scenario: P di passare la valutazione, probabilita' di almeno un prelievo da
finanziati, mediana del netto, cicli in perdita. Lo scenario (d), il conto che finisce al terzo
prelievo, e' solo descrittivo.

**L'incrocio regge** se K1, K2, K3 e K4 non sono smentiti. Se K1 regge e un altro no, l'esito dice
quale ipotesi lo uccide.
