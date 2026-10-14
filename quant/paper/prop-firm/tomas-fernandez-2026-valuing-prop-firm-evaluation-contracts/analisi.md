---
chiave: tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts
sigla: TF
titolo: "Valuing Proprietary Trading Firm Evaluation Contracts: Closed Form, Cross-Firm Dispersion and the Source of the Margin"
autori: [Martí Tomàs Fernández]
versione_letta: agosto 2026
fonte: SSRN
identificativo: "7260819"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7260819
tipo: preprint
revisione_fra_pari: no
pagine: 24
pdf: tomas-fernandez-2026-valuing-prop-firm-evaluation-contracts.pdf
pdf_sha256: 5e7e863f439f3a9d2e70f06208c1dd615394f2ca6cef8af9bf31bc9c45cab0d2
filone: prop-firm
ruolo: idea
stato: analizzato
letto_il: 2026-10-11
studi: [contratto-prop]
codice_degli_autori: "l'autore pubblica modello e dati delle firme, non il motore di simulazione"
---

# Tomas Fernandez 2026: L'Analisi

*Valuing Proprietary Trading Firm Evaluation Contracts: Closed Form, Cross-Firm Dispersion and the
Source of the Margin*, Marti Tomas Fernandez, agosto 2026, 24 pagine, SSRN 7260819. Preprint, non
revisionato; scritto con l'aiuto di modelli linguistici (dichiarato). **Conflitto d'interessi
dichiarato**: l'autore ha operato sul prodotto del caso di studio. Letto per intero l'11 ottobre 2026.
Citazioni `[TF §n]`.

## La Domanda

Quanto vale, per chi la compra, la valutazione di una prop firm di future? E da dove viene il
margine della firma?

## Il Metodo

Il contratto e' **una catena di problemi di primo passaggio** [TF §2.2]: ogni fase (valutazione,
conto finanziato) e' indipendente dalla strada che l'ha preceduta, e la probabilita' totale e' il
prodotto. Dentro una fase il saldo S e' un moto browniano con deriva mu e volatilita' d per sessione;
il pavimento e' `F = min(M - D, kappa)`, con M il massimo raggiunto [TF eq. 2].

**La forma chiusa** [TF eq. 3 e 5], con theta = 2 mu / d^2, L = min(D + kappa, T), R = T - D - kappa:

```text
P = exp( -theta L / (e^(theta D1) - 1) )  x  (e^(theta D2) - 1) / (e^(theta D2) - e^(-theta R))
     tratto mobile                             tratto bloccato (solo se R > 0)

senza deriva:  P = e^(-L/D1)  x  D2 / (D2 + R)

D1 = D + 0,5826 max(sM, sB)     D2 = D + 0,5826 sB     s = d / sqrt(controlli per sessione)
```

sM e sB sono la deviazione del saldo fra due aggiornamenti del pavimento e fra due controlli della
violazione: la correzione di Broadie, Glasserman e Kou per il monitoraggio discreto. **La dimensione
della posizione entra solo attraverso s** [TF §2.4]: con monitoraggio continuo e senza deriva non
conta (la rovina di una martingala non dipende dalla varianza).

Poi la valutazione [TF §3]: costo atteso per ottenere un conto `Pi = c_e + (1/P - 1) c_r + c_a`
[eq. 9], valore del conto `F` [eq. 10], **tasso di conversione di pareggio** `PA = c_r / (c_r + F - c_e)`
[§3.6], che e' insieme la soglia del compratore e il punto in cui la firma smette di guadagnare [§5.3].

I dati: le regole di 13 prodotti da 50K dalle fonti ufficiali al 10 agosto 2026 [§4, App. A]; 1.833
sessioni dell'indice a 5 minuti, con il verso a caso, per tarare e provare il modello [§8].

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | stesso T e D (3.000 / 2.000), probabilita' di passare da 0,250 a 0,407 su 11 prodotti: **62%** di differenza, da parametri che non stanno accanto al prezzo | §4.2 |
| 2 | i regimi del pavimento: fisso 0,400, bloccato a 0 0,245, a +100 0,241, sempre mobile 0,223 | §2.3 |
| 3 | pavimento alla chiusura intorno a 0,30, intraday intorno a 0,25 | §4.2 |
| 4 | il limite giornaliero morbido toglie dall'1 al 10% di P; il rigido ha una sua forma chiusa | §2.5 |
| 5 | la consistenza funziona in tre modi diversi (alza il target, condiziona il prelievo, limita il guadagno del giorno) | §4.3 |
| 6 | l'ingresso, l'unico prezzo che il mercato confronta, e' la leva meno efficace: 8 volte meno del reset | §5.4 |
| 7 | i tassi di passaggio dell'industria (5-14%) stanno sotto il pareggio; il modello predice 0,30. La differenza vale una deriva di -127 / -303 $ a sessione, **da 3 a 30 volte i costi di esecuzione** | §5.1 - 5.6 |
| 8 | il margine della firma viene **dalla selezione**, non dal prezzo: per un trader senza vantaggio il prodotto e' economico | §5.7 |
| 9 | le regole le scrive la firma e le cambia anche sui conti venduti: nessuna configurazione favorevole resta stabile; la copertura non conviene | §6 |
| 10 | caso di studio: costo 338 $, valore 606 $, g = 0,50; il trader dell'autore ha un vantaggio di -9,6% | §8.4 - 8.5 |

## Cosa Abbiamo Riprodotto

Tutto nel framework, con i test e con i criteri sigillati
([`quant/studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md`](../../../studi/contratto-prop/5-validazione/criteri/2026-10-11-il-contratto-prop-esito.md)):

| | esito |
|---|---|
| i quattro regimi (§2.3), forma chiusa | esatti (test) |
| le cinque combinazioni di monitoraggio (§2.4), forma chiusa | esatte (test) |
| le stesse, simulate | 4 su 5 entro l'1%; la quinta no, vedi sotto |
| Tradeify Growth P = 0,300 (§4.2) | esatto (test) |
| costo 338 $ e pareggio 0,166 (§8.5) | esatti (test) |
| la deriva implicita (§5.6) | esatta, ma con una geometria diversa da quella del caso di studio |

## Cosa Non Regge, O Non Si Dice

1. **Un settimo parametro.** La forma chiusa tratta il target come controllato in continuo. Con
   pavimento e violazione alla chiusura e target guardato durante la sessione, la simulazione da' il
   13% in piu' della forma chiusa; con target alla chiusura coincide. Con pavimento fisso alla
   chiusura e target alla chiusura la forma chiusa sbaglia dell'8,6% per eccesso, con s/D = 0,42,
   **dentro** l'intervallo in cui il paper dichiara un errore sotto l'1%. Il paper ha verificato
   cinque combinazioni, tutte con kappa = 0: la validita' dichiarata "funzione di s/D soltanto" e'
   piu' stretta di cosi'. Per i prodotti con violazione in tempo reale (la maggioranza, §4.2) la forma
   chiusa regge.
2. **La tabella della deriva implicita** (§5.6) usa una geometria che il testo non dichiara (pavimento
   bloccato a 0, intraday e intraday), non quella del caso di studio di cui parla.
3. **Il caso di studio non e' nominato.** Dai prezzi (listino 145 $, effettivo 107 $, reset 99 $,
   pagamento unico, quota 90%) e da P = 0,300, unico in tabella, e' con ogni probabilita' **Tradeify
   Growth 50K**. E' una deduzione nostra.
4. **Il valore del conto (606 $) usa la produttivita' misurata del trader dell'autore** (673 $), cioe'
   un trader solo, con conflitto d'interessi dichiarato. Per un trader senza vantaggio il paper da'
   G0 = 745 $, con una soglia di prelievo che nel frattempo e' cambiata due volte.
5. **I tassi dell'industria** (5-14%) vengono da aggregatori senza fonte primaria, e il paper lo dice;
   le uniche due osservazioni citabili sono un 7,35% autodichiarato e una stima da un ricorso CFTC.
   Oggi Tradeify pubblica il suo: **17,2%** delle valutazioni iniziate, agosto 2025 - luglio 2026, su
   tre prodotti insieme. Sta sopra il 5-14% del paper e sotto lo 0,30 del modello.
6. **La fine del conto simulato** (il passaggio al vivo dopo un prelievo, a discrezione della firma)
   non entra nel valore F. Nella nostra simulazione e' la variabile che pesa di piu'.

## Cosa Ne Prende Il Framework

- **La forma chiusa come verifica** del simulatore di contratto: `FormaChiusa.cs`.
- **Il tasso di pareggio PA**, calcolabile dai prezzi pubblici.
- **L'effetto del disegno** [§8.2]: l'informazione sta nelle sessioni, non nei percorsi; i tentativi
  indipendenti sono S / E[N]. E' la stessa lezione del ricampionamento per giornata.
- **La coerenza della misura** [§7]: chi ripesa gli scenari deve ripesare anche il valore, o usare il
  rapporto di verosimiglianza. La diagnosi: se l'effetto cresce con il fattore di calibrazione, e'
  un artefatto.
- **Il sigillo dei criteri** [App. B], ora nel framework: [`quant/metodo/5-come-si-valida.md`](../../../metodo/5-come-si-valida.md#i-criteri-sigillati).
- **Il registro delle stime** [App. C]: le correzioni per errore andavano tutte contro la tesi. Gli
  errori non sono distribuiti a caso.
