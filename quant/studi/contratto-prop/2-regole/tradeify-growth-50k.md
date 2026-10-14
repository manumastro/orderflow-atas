# Tradeify Growth 50K: Le Regole Del Contratto

**Lette l'11 ottobre 2026**, solo dalle fonti ufficiali di Tradeify (centro assistenza e sito), come fa
Tomas Fernandez [TF §4.1]: niente siti di recensioni, niente aggregatori. Le regole cambiano, e le
cambia la firma anche sui conti gia' venduti [TF §6]: **ogni numero qui vale alla data**, e prima di
riusarlo si rilegge la fonte.

Nel codice: `TradeifyGrowth50K.Regole` in [`quant/studi/contratto-prop/3-codice/TradeifyGrowth50K.cs`](../3-codice/TradeifyGrowth50K.cs),
con i nomi `tradeify-growth-50k` e `tradeify-growth-50k-blocco` (vedi "Due Pagine Che Non Concordano").

Come sono state lette: con il browser di Playwright, perche' il centro assistenza risponde 403 alla
lettura automatica, come SSRN ([`quant/metodo/1-come-si-cercano-le-idee.md`](../../../metodo/1-come-si-cercano-le-idee.md)).

## Le Fonti

| pagina | indirizzo |
|---|---|
| GROWTH Evaluation Accounts | https://help.tradeify.co/en/articles/10495915-growth-evaluation-accounts |
| Rules: Trailing Max Drawdowns | https://help.tradeify.co/en/articles/10495897-rules-trailing-max-drawdowns |
| Essential Trading Rules Overview | https://help.tradeify.co/en/articles/12268167-essential-trading-rules-overview |
| Rules: Daily Loss Limit | https://help.tradeify.co/en/articles/10468321-rules-daily-loss-limit |
| Rules: Consistency Rule | https://help.tradeify.co/en/articles/10468320-rules-consistency-rule |
| Advanced and Growth Account Payout Policies | https://help.tradeify.co/en/articles/11083796-advanced-and-growth-account-payout-policies |
| Tradeify Pricing Reference | https://help.tradeify.co/en/articles/14369021-tradeify-pricing-reference |
| sito, sezione "Choose Your Account" | https://tradeify.co/ |

## La Valutazione

| parametro (TF §2) | valore | citazione |
|---|---|---|
| target T | 3.000 $ | tabella "GROWTH evaluation rules": "$50k · $3,000 · $1,250 · $2,000 · 4 Contracts (40 Micros)" |
| drawdown D | 2.000 $, mobile | idem |
| pavimento | **alla chiusura** (EOD) | "The GROWTH evaluation uses an End of Day Trailing Max Drawdown, which is based on the account balance at the end of the trading day." |
| violazione | **in tempo reale** | "The level is still enforced in real time: if your net liquidation value touches it at any moment, the account fails immediately" |
| blocco kappa | **non concordano** due pagine | vedi sotto |
| limite giornaliero | 1.250 $, **morbido** | "If you hit it, trading stops for the day but your account is not failed." |
| consistenza | nessuna in valutazione | "GROWTH evaluation does NOT have a consistency requirement." |
| scadenza | nessuna | "Is there a time limit to complete GROWTH evaluation? A: No." |
| giorni minimi | 1 | "You can pass in as few as 1 trading day" |
| dimensione massima | 4 mini o 40 micro | tabella |

## Il Conto Finanziato

| parametro | valore | citazione |
|---|---|---|
| drawdown | 2.000 $, alla chiusura, in tempo reale | "All Funded Accounts (Select, Growth, Lightning) - End-of-Day Trailing Drawdown" |
| blocco | a +100 $, quando il saldo di fine giornata arriva a 52.100 $ **o alla prima richiesta di prelievo** | "your drawdown locks in place once you profit beyond the drawdown amount by $100, or immediately when you request a payout - whichever comes first" |
| limite giornaliero | 1.250 $, morbido; sale a 2.000 $ dalla sessione dopo quella in cui il saldo arriva a 53.000 $ | "When your account reaches 6% profit ... your DLL increases to match your drawdown amount. This larger DLL takes effect the NEXT trading session" |
| saldo per chiedere un prelievo | **53.000 $** (+3.000 $) | tabella "Account Balance Needed for Growth" |
| giorni | 5 sessioni con profitto oltre 150 $, dall'ultimo prelievo | "Minimum Trading Days: 5 or more of the Trading Days must show a profit greater than ... $150 in profit for 50k accounts" |
| consistenza | 35%: la sessione migliore non oltre il 35% del profitto dall'ultimo prelievo | "Growth Sim Funded Accounts must follow a 35% Consistency Rule"; "After an approved payout, the consistency % will reset" |
| tetto per richiesta | 1.500 / 2.000 / 2.500 / 3.000 $ (dal quarto in poi) | tabella "Maximum Payout Per Request" |
| minimo per richiesta | 500 $ | "$500 for $50k accounts" |
| quota al trader | 90% | "Traders receive 90% of the payout amount requested." |
| conti finanziati insieme | al massimo 5 | "You cannot have more than 5 sim funded accounts active at any time." |
| passaggio al vivo | a discrezione, dopo un prelievo | "Tradeify reserves the right to move you to a Live Funded Account at any time after a successful payout." |

## I Prezzi

| voce | valore | fonte |
|---|---|---|
| valutazione Growth 50K, listino | 145 $, una volta sola | Pricing Reference e sito |
| con il codice GEMS (30%, fino al 15 ottobre 2026) | 102 $ | sito: "Save $44 with code GEMS" |
| reset | **95 $** | Pricing Reference |
| attivazione | nessuna | "There are no activation fees for any current Tradeify account type." |
| commissione MNQ | 1,82 $ andata e ritorno | Pricing Reference, "Trading Commission Fees" |

## Cosa E' Cambiato Da Agosto

TF ha letto le regole il 10 agosto 2026 [TF App. A]. Due mesi dopo:

| | TF, 10 agosto | oggi, 11 ottobre |
|---|---|---|
| prezzo effettivo | 107 $ | 145 $ di listino, 102 $ con un codice a scadenza |
| reset | 99 $ | 95 $ |
| soglia del prelievo | da 750 a 1.500 $ durante la stesura [TF §6.2] | saldo di 53.000 $, cioe' **3.000 $** di profitto, piu' 5 giorni oltre 150 $ e la consistenza |
| blocco in valutazione | +100 $ | due pagine dicono due cose |

**La soglia del prelievo e' la regola che pesa di piu' sul valore del conto finanziato** [TF §6.1], e
in due mesi e' cambiata almeno due volte.

## Due Pagine Che Non Concordano

Come in TF §4.6, due pagine ufficiali pubblicano regole diverse per lo stesso prodotto, alla stessa
data:

- "Rules: Trailing Max Drawdowns": "Q: Does drawdown lock on Evaluation accounts? A: No. Drawdown
  only locks on Sim Funded accounts."
- "Essential Trading Rules Overview", sotto "All Evaluation Accounts": "Example: A Growth account
  with a $2,000 trailing drawdown locks its floor at $50,100 once its end-of-day balance reaches
  $52,100."

Nel codice sono due contratti: `tradeify-growth-50k` (nessun blocco in valutazione) e
`tradeify-growth-50k-blocco`. **La differenza non conta**: P = 0,3021 contro 0,3080 per d = 850 $
(vedi il rapporto). Il pavimento si ferma a +100 $ solo dopo che il saldo e' arrivato a 2.100 $, a
900 $ dal target.

## I Numeri Che Tradeify Pubblica

Sul sito, in fondo alla pagina, l'11 ottobre 2026:

> "Trader Performance Statistics (August 2025 – July 2026, calculated as of 9 August 2026): Across
> Tradeify's Growth, Advanced and Select evaluation products, (a) 17.2% of evaluation accounts
> initiated were successfully completed and afforded the opportunity to advance to the Funded Level;
> (b) 40.3% of individual participants who initiated one or more evaluations advanced to the Funded
> Level in at least one of their evaluations - participants may purchase multiple evaluations and
> resets, and initiated a median of 3 each over the period; (c) 28.5% of individual participants at
> the Funded Level received a payout; and (d) 3.0% of individual participants trading in a Funded
> Account were called up to a Live Funded Account."

E' il dato che TF dice mancare all'industria [TF §5.1]: un tasso di passaggio pubblicato dalla firma,
con il denominatore dichiarato (le valutazioni iniziate). Due cautele: **somma tre prodotti** (Growth,
Advanced, Select), non solo Growth; e (c) e (d) sono **per persona**, non per conto.

## Cosa Resta Fuori Dal Modello

Non sono barriere sul P&L cumulato, e restano fuori come in TF §2.8. Ciascuna puo' solo togliere
valore al compratore:

- **microscalping**: "Over 50% of your trades AND profits must come from trades held longer than 10
  seconds", solo nel finanziato, per poter prelevare;
- **attivita' minima**: almeno un'operazione a settimana;
- **chiusura** entro le 16:45 ET, niente posizioni la notte;
- **copertura** vietata, anche fra conti diversi; regole sulle notizie;
- **approvazione** del prelievo secondo il Funded Trader Agreement: discrezione della firma;
- **il passaggio al vivo** dopo un prelievo, che chiude il conto simulato.
