---
chiave: lim-2026-price-of-a-funded-account
sigla: L
titolo: "The Price of a Funded Account: An Actuarial Analysis of Proprietary Trading Firm Challenges"
autori: [Boon Chuan Lim]
versione_letta: 25 luglio 2026 (revised)
fonte: SSRN
identificativo: "7178078"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7178078
tipo: preprint
revisione_fra_pari: no
pagine: 14
pdf: lim-2026-price-of-a-funded-account.pdf
pdf_sha256: 2e618b37063000b4037e15433e27fe80d2c257f22dc8f6b32d2786fc9ce1fb90
filone: prop-firm
ruolo: idea
stato: analizzato
letto_il: 2026-10-11
studi: [contratto-prop]
codice_degli_autori: "disponibile dall'autore, su richiesta"
---

# Lim 2026: L'Analisi

*The Price of a Funded Account: An Actuarial Analysis of Proprietary Trading Firm Challenges*, Boon
Chuan Lim, ricercatore indipendente (Singapore), 25 luglio 2026, 14 pagine, SSRN 7178078. Preprint,
non revisionato; codice di simulazione scritto con l'aiuto di Claude (dichiarato). L'autore ha
partecipato a valutazioni di prop firm come cliente. Letto per intero l'11 ottobre 2026. Citazioni
`[L §n]`.

## La Domanda

Quanto vale una "challenge" per chi la compra, e cosa dovrebbe essere obbligata a pubblicare una
firma?

## Il Metodo

- **Il contratto**: un archetipo stilizzato, non una firma [L Tab. 1]. Conto da 100.000 $, due passi
  (+10%, poi +5%), perdita massima -10% **statica** (dal saldo iniziale), perdita giornaliera -5%
  **rigida**, scadenze 30 e 60 sessioni, controllo **alle chiusure giornaliere**. Il finanziato:
  prelievo ogni 20 sessioni di tutto il positivo all'80%, saldo che si azzera, prezzo di 500 $
  rimborsato al primo prelievo, orizzonte di 250 sessioni. E' il modello forex/CFD, non quello dei
  future.
- **Il trader**: moto browniano aritmetico giornaliero; abilita' come Sharpe annuo netto s (da -5 a
  +4), dimensione come volatilita' giornaliera (0,5% - 5%). 200.000 percorsi per configurazione.
- **La popolazione**: Sharpe lordo normale standard, costi pari al 10% della volatilita' (un freno di
  1,59 di Sharpe annuo), pesi sulle volatilita' scelti a mano [L §5]. Dichiarata "illustrativa".

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | la simmetria apparente (+10% / -10%) nasconde un vantaggio per la firma: senza abilita' si passa con 0,500 senza attriti, con 0,298 con scadenza e regola giornaliera. **La scadenza da sola toglie 19 punti** | §6.1 |
| 2 | il valore si concentra su una **cresta stretta intorno al 2% di volatilita' giornaliera**: sotto si fallisce per tempo, sopra per i limiti | §6.2 - 6.3 |
| 3 | sulla cresta il contratto "perdona" quasi un punto di Sharpe: pareggio a -0,87 di Sharpe netto | §6.3 |
| 4 | "il contratto non distingue un trader cauto con un po' di abilita' da uno aggressivo senza" | §6.2 |
| 5 | popolazione illustrativa: valore medio 221 $ contro 500 $ di prezzo, margine lordo 56%, il 7,5% arriva al finanziato | §6.4 |
| 6 | proposta: pubblicare un imbuto per coorte, il rapporto prelievi / incassi e il prelievo mediano, come per i CFD in Europa | §7 |

## Cosa Abbiamo Riprodotto

La tabella 2, colonna senza abilita' (criterio sigillato C3): **tutte e cinque le righe** entro
0,0034 (0,5003 / 0,4548 / 0,3066 / 0,2974 / 0,4548 contro 0,500 / 0,453 / 0,310 / 0,298 / 0,453). Il
contratto e' nel catalogo come `lim-classico` e `lim-senza-scadenza`.

## Cosa Non Regge, O Non Si Applica

1. **Non e' un contratto di future.** Barriere statiche, controllo a fine giornata, scadenza: i
   prodotti di future di TF hanno il pavimento mobile, la violazione in tempo reale e quasi sempre
   nessuna scadenza. **La cresta di Lim nasce dalla scadenza** (sotto il 2% si fallisce per tempo);
   senza scadenza, quella cresta non c'e'.
2. Su Tradeify Growth (nessuna scadenza) abbiamo misurato cosa succede al variare della dimensione:
   con il limite giornaliero morbido, se si esce esattamente al limite, il valore **cresce** con la
   dimensione; senza quel limite c'e' una cresta, ma per un'altra ragione (la consistenza del 35% e i
   5 giorni sopra 150 $, che puniscono le giornate grandi). Vedi
   [`quant/studi/contratto-prop/5-validazione/risultati/2026-10-11-tradeify-growth-50k.md`](../../../studi/contratto-prop/5-validazione/risultati/2026-10-11-tradeify-growth-50k.md).
3. **La popolazione e' inventata**, e l'autore lo dice: il 56% di margine dipende dai pesi sulle
   volatilita' e dal costo k (da 60 a 557 $ nella tabella 6).
4. Il monitoraggio a fine giornata rende tutti i valori **un tetto** per i prodotti controllati in
   tempo reale; l'autore lo dice.

## Cosa Ne Prende Il Framework

- **Aggiungere le regole una alla volta** e misurare cosa toglie ciascuna [L Tab. 2]: e' il metodo a
  iterazioni della fase 3 del transcript applicato al contratto. Il simulatore lo permette
  (`--senza-limite`, `--max-prelievi`, le fasi con e senza scadenza).
- **Il contratto premia la dimensione giusta, non l'abilita'**: prima di dire che una strategia "va
  bene per una prop", si misura il suo valore al variare della dimensione.
- **Il controllo C3** del simulatore.
