---
chiave: youngblood-2026-inherent-value-of-retail-prop-firms
sigla: Y
titolo: "The Inherent Value of Retail Prop Firms for Fixed-Size Trading Strategies"
autori: [Zachary Youngblood]
versione_letta: 21 agosto 2026
fonte: SSRN
identificativo: "7514219"
url: https://papers.ssrn.com/sol3/papers.cfm?abstract_id=7514219
tipo: preprint
revisione_fra_pari: no
pagine: 13
pdf: youngblood-2026-inherent-value-of-retail-prop-firms.pdf
pdf_sha256: 38e8725627fc5289549747364ae4fda44e5e377482c8b036fea58cf71f106598
filone: prop-firm
ruolo: idea
stato: analizzato
letto_il: 2026-10-11
studi: [contratto-prop]
codice_degli_autori: "https://github.com/zyoungblood6/The-Inherent-Edge-of-Retail-Proprietary-Trading-Firms"
---

# Youngblood 2026: L'Analisi

*The Inherent Value of Retail Prop Firms for Fixed-Size Trading Strategies*, Zachary Youngblood,
studente della Gabelli School of Business (Fordham), 21 agosto 2026, 13 pagine, SSRN 7514219.
Preprint, non revisionato. Codice su GitHub (`zyoungblood6/The-Inherent-Edge-of-Retail-Proprietary-Trading-Firms`).
Letto per intero l'11 ottobre 2026. Citazioni `[Y §n]`.

## La Domanda

Una strategia senza vantaggio, a dimensione fissa, puo' avere un valore atteso positivo dentro le
regole di una prop firm?

## Il Metodo

- **La strategia** [Y §2.2]: 8 MNQ, ingresso alle 10:00 ET, verso a caso, target e stop simmetrici di
  100 tick (+-400 $), uscita alle 16:00; 1,22 $ di costi per contratto; esclusi i giorni con stop e
  target nella stessa barra.
- **I dati**: MNQ a 1 minuto, 8 agosto 2016 - 5 agosto 2026, 2.485 giornate (comprati da
  BacktestMarket).
- **Il contratto**: Topstep 50K come lo programma l'autore (Combine + Express Funded Account,
  abbonamento da 85 $ al mese, crediti di reset, consistenza 50% e 40%, prelievo al massimo del 50%
  del profitto) [Y §3].
- **Le simulazioni**: 10.000 percorsi che **ripescano solo il verso** sulle stesse 2.485 giornate. Lo
  stesso percorso si misura da solo e dentro il contratto: la differenza e' il contributo del
  contratto.

## I Risultati

| | risultato | dove |
|---|---|---|
| 1 | da sola la strategia non ha vantaggio: 49,98% di vincenti, -0,17 $ lordi a operazione, **-24.670 $ netti** in 10 anni | §2.4 |
| 2 | dentro Topstep: **+1.490 $ di netto medio per conto**, mediana +1.037 $, 1,19 $ di prelievi per 1 $ di costi, 60,3% dei percorsi in utile | §3.1 |
| 3 | instabile: 2016-2021 rapporto 1,06 e mediana negativa (46,45% in utile); 2021-2026 1,22 | §3.2 |
| 4 | nel primo periodo 196 uscite a tempo per percorso, nel secondo nessuna | §3.2 |
| 5 | tre meccanismi: la dimensione fissa rende irrilevanti alcune regole, la varianza naturale, il guadagno asimmetrico (perdita limitata al costo) | §4.1 |

## Cosa Abbiamo Riprodotto

La strategia e' nel framework (`--sorgente youngblood`), applicata a Tradeify Growth 50K invece che a
Topstep. **Non si puo' misurare**: abbiamo 22 sedute complete di NQ, e con circa 22 sessioni per
tentativo i tentativi indipendenti sono **uno**. Il numero che esce (P = 0,15, netto -317 $ per
ciclo) e' solo descrittivo (criterio C5). Un dato pero' si legge: con 8 MNQ la deviazione di
sessione e' circa 394 $ e i costi (1,82 $ di commissione Tradeify e 2 tick di slittamento) circa 23
$ a sessione, cioe' una deriva di -0,06 d. Contro i 9,76 $ di Y.

## Cosa Non Regge

1. **I 10.000 percorsi non sono 10.000 prove.** L'autore lo scrive [Y §2.3], poi riporta il 60,30%
   di percorsi in utile e il rapporto 1,19 senza nessun intervallo. Con 2.485 sessioni e circa
   venti sessioni per tentativo le prove indipendenti sono circa un centinaio; il margine fra 1,19
   e 1,00 va misurato contro quel numero, non contro 10.000 [TF §8.2].
2. **Il risultato dipende dal periodo**, e l'autore lo mostra: mediana negativa nel primo
   quinquennio. Il secondo e' quello con NQ piu' volatile: 25 punti si toccano sempre. **La stessa
   strategia ha cambiato dimensione relativa** (d rispetto alle regole) senza cambiare contratti:
   e' la cresta di Lim vista da un'altra parte.
3. **Target e stop scelti "con le regole di Topstep in mente"** [Y §3]: 400 $ stanno sotto il limite
   giornaliero di 1.000 $ apposta. E' una scelta fatta guardando le regole, non i dati, ma non si sa
   quante altre siano state provate.
4. **Costi**: 1,22 $ a contratto, senza slittamento. Il nostro 2,82 $ (commissione Tradeify e due
   tick) pesa piu' del doppio, e a 8 contratti la deriva conta.
5. **Il "sottoprezzo di 16 $"** viene da 85 $ x 1,19: tratta il rapporto prelievi / costi come se non
   dipendesse dal prezzo, mentre il numero di mensilita' dipende dalla durata.
6. **Le regole di Topstep le ha programmate l'autore**, come dice lui, e cambiano.

## Cosa Ne Prende Il Framework

- **Il confronto appaiato**: le stesse operazioni da sole e dentro il contratto. E' la forma giusta
  per misurare il contributo del contratto, e la stessa che manca al framework per le anomalie di
  esposizione.
- **Il netto vero e' prelievi meno costi**, non il saldo simulato [Y §2.1].
- **Un avvertimento**: un risultato positivo dentro il contratto, con la mediana negativa in un
  periodo su due, e' una lotteria con valore atteso positivo, non una rendita.
