---
name: studio-globale-1-macro
description: Fase 1 dello studio globale di NQ - la macro. Legge i dati gia' raccolti (tassi, curva, volatilita', dollaro, materie prime, credito, calendario USD, FOMC, trimestrali, mercati di previsione, sentiment, notizie) e scrive 1-macro.md; aggiorna il calendario degli eventi del giro.
argument-hint: "[cartella del rapporto]"
---

# Fase 1: La Macro

Il contesto piu' lontano: in che mondo di tassi, volatilita' e notizie si muove NQ oggi. Il rapporto
segue [`../studio-globale/modello-del-rapporto.md`](../studio-globale/modello-del-rapporto.md).

## I Tuoi Dati

`dati/macro.json` (raccolto da `studio/raccogli.py` con `studio/macro/macro.py`), da leggere **tutto**;
`dati/ore-NQZ6.json` per cosa ha fatto NQ ora per ora nelle ultime 48 ore; `dati/raccolta.json` per
l'ora e gli errori. Le fonti e i loro limiti: [`studio/macro/le-fonti-della-macro.md`](../../../studio/macro/le-fonti-della-macro.md).

Le chiavi di `macro.json`:

| chiave | cosa c'e' |
|---|---|
| `quotazioni` | un anno giornaliero: ultimo, variazioni, percentile, volatilita' |
| `treasury` | la curva par di ieri |
| `fred.serie` | fed funds, 2 e 10 anni, breakeven, spread HY, stress, bilancio Fed; PPI, CPI, vendite al dettaglio con le variazioni mensili e annue; sussidi; Philly Fed. Ognuna con data e precedente |
| `esiti` | il **dato uscito** degli eventi della settimana, da FRED, accanto ad attesa e precedente; `uscito: null` con il perche' se FRED non l'ha ancora |
| `tempo_reale` | VIX, VIX9D, VIX3M, VXN, 10 anni, ZT=F e 2YY=F di adesso, con l'ora e se il dato e' **di oggi** |
| `fomc_future` | la probabilita' di un passo alla prossima riunione dai future sui fed funds, con il metodo |
| `ampiezza` | il Nasdaq-100: quanti salgono e quanti scendono, la media, la mediana, i dieci piu' grandi |
| `calendario`, `trimestrali`, `previsioni`, `sentiment` | come prima |
| `notizie` | titolo e **testo** (il sommario del feed, quando c'e') |

**Non hai Bash e non raccogli altri dati**, con una sola eccezione: per un evento gia' uscito che in
`esiti` ha `uscito: null`, cerca il valore con WebSearch, alla fonte primaria se possibile (BLS, Census,
Fed), al massimo due ricerche, e scrivi da dove viene.

## Leggi, Nell'Ordine

1. **I tassi**: la curva Treasury (3M, 2A, 10A, 30A), le pendenze 10A-2A e 10A-3M, la variazione su
   20 sedute; il prossimo FOMC e le probabilita' di Polymarket.
2. **La volatilita'**: VIX e VXN con il percentile a un anno; la struttura VIX9D / VIX / VIX3M
   (VIX9D sopra VIX3M = struttura invertita, la paura e' vicina).
3. **Dollaro, petrolio, oro, credito**: DXY, WTI, oro, HYG, TLT.
4. **Chi guida**: NQ contro ES contro RSP (la partecipazione), SMH, le otto grandi; le trimestrali.
5. **Il sentiment**: Fear & Greed e la variazione in una settimana.
6. **Il calendario**: gli eventi USD di oggi e domani con l'ora italiana e l'attesa; quelli di ieri con
   il dato uscito e **cosa ha fatto NQ** nell'ora (da `ore-NQZ6.json`).
7. **Le notizie**: le cinque che spiegano il movimento di ieri e della notte, con l'ora e il movimento
   orario accanto. Una notizia senza un movimento di prezzo e' rumore, e si dice.

## Aggiorna Il Calendario Del Giro

`docs/research/calendario/eventi.json` lo legge il giro d'orizzonte. Aggiungi gli eventi di peso alto e
medio di oggi e dei prossimi due giorni (`data`, `ora` italiana, `evento` in italiano breve, `peso`:
`alto`/`medio`/`basso`/`contesto`, `esito` vuoto), e scrivi l'`esito` di quelli passati con i numeri.
**Testi brevi**: evento sotto i 70 caratteri, esito sotto i 90 (il giro li taglia a 105). Non duplicare:
aggiorna.

## Scrivi `1-macro.md`

**In una riga** dice il regime: per esempio "tassi alti e in salita, volatilita' in risveglio con
struttura in contango, dollaro forte, credito debole: un contesto di rischio".
