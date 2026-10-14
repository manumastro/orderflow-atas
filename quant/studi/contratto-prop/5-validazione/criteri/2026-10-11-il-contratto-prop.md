# Criteri Sigillati: Il Contratto Della Prop Firm

Scritti l'11 ottobre 2026, **prima** di scrivere il simulatore di contratto e prima di qualunque
simulazione. Il file non si modifica: il suo SHA-256 e' registrato nella scheda
`quant/1-l-idea/idee/il-valore-del-contratto-prop.md` e in `FabioOrderFlow/progress.txt`. Un
criterio sbagliato si corregge con un file nuovo, che cita questo e dice perche'.

Simboli: P probabilita' di passare una fase; T target; D drawdown; kappa livello di blocco del
pavimento rispetto al saldo iniziale; d deviazione standard del P&L di una sessione; m controlli per
sessione; SE errore standard Monte Carlo.

## C1. Il simulatore riproduce la forma chiusa di Tomas Fernandez (TF eq. 3 e 5)

- **Quantita'**: P simulata, deriva zero, T = 3.000, D = 2.000, kappa = 0, d = 850 $, nelle cinque
  combinazioni di aggiornamento del pavimento e controllo della violazione della tabella di TF §2.4
  (intraday/intraday, chiusura/intraday, chiusura/chiusura, intraday/chiusura, oraria/intraday).
- **Campione**: 200.000 tentativi per combinazione.
- **Smentito se**: in almeno una combinazione |P_sim - P_fc| / P_fc > 1,5%, dove P_fc e' la forma
  chiusa calcolata con gli stessi s_M e s_B del simulatore.

## C2. I quattro regimi del pavimento (TF §2.3)

- **Quantita'**: P simulata con monitoraggio fine (390 controlli per sessione, pavimento
  aggiornato a ogni controllo), deriva zero, T = 3.000, D = 2.000, kappa = -D, 0, +100, T - D.
- **Campione**: 200.000 tentativi per regime.
- **Smentito se**: in almeno un regime |P_sim - P_fc| / P_fc > 1,5%, con la forma chiusa corretta per
  il monitoraggio discreto.

## C3. La tabella 2 di Lim (Lim §6.1), colonna Sharpe netto 0

- **Quantita'**: P del passo 1, volatilita' giornaliera 2%, target +10%, perdita massima -10%
  statica, controllo alle chiusure giornaliere; righe: solo barriere; + regola giornaliera (-5%,
  rigida); + scadenza a 30 sessioni; entrambe; regola giornaliera senza scadenza (tetto 300 sessioni).
- **Campione**: 200.000 tentativi per riga.
- **Smentito se**: in almeno una riga |P_sim - P_Lim| > 0,006 (circa 5 SE), con P_Lim = 0,500 /
  0,453 / 0,310 / 0,298 / 0,453.

## C4. La deriva implicita (TF §5.6)

- **Quantita'**: la deriva per sessione che, nella forma chiusa del caso di studio di TF (kappa =
  +100, pavimento alla chiusura, violazione intraday, d = 850 $), da' P = 14%, 10%, 7%, 5%.
- **Smentito se**: in almeno un caso la deriva calcolata differisce da quella di TF (-127, -189,
  -249, -303 $ per sessione) di piu' di 5 $.

## C5. I dati veri di NQ: nessun criterio

L'archivio ha 50 sessioni. Con circa 4-5 sessioni per tentativo, i tentativi indipendenti sono
circa 10 (TF §8.2): l'errore standard di P e' intorno a 0,15, e nessuna affermazione su P si puo'
smentire. Le simulazioni sui dati veri sono **descrittive**: servono a vedere la forma vera delle
sessioni (code, escursione intraday) e a far girare la pipeline, non a misurare. Lo si scrive
accanto a ogni numero che ne esce.
