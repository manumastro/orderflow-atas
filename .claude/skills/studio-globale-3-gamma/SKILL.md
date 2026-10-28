---
name: studio-globale-3-gamma
description: Fase 3 dello studio globale (NQ o ES) - la gamma dei dealer. Legge la GEX gia' calcolata dello strumento dello studio (NQZ6 o ESZ6) (Black-76, due convenzioni di segno, zero gamma, muri, strike per strike, mossa attesa) e scrive 3-gamma.md con i paper del filone opzioni-gamma.
argument-hint: "[cartella del rapporto]"
---

# Fase 3: La Gamma

Come si coprono i dealer, e quindi se il mercato tende a **smorzare** o ad **amplificare** i movimenti.
Il rapporto segue [`../studio-globale/modello-del-rapporto.md`](../studio-globale/modello-del-rapporto.md).

## Leggi Prima

- [`quant/metodo/le-opzioni-in-atas.md`](../../../quant/metodo/le-opzioni-in-atas.md), sezione "La GEX
  calcolata da noi": il modello e i suoi limiti;
- [`quant/studi/opzioni-gamma-es/1-idea.md`](../../../quant/studi/opzioni-gamma-es/1-idea.md): i finding
  dei paper e cosa li ucciderebbe; e la sintesi piu' recente in `quant/paper/ricerche/` sul filone
  opzioni-gamma, se c'e'.

## Per Chi Legge

L'utente sta imparando le opzioni (16 ottobre 2026). Nella sezione **In parole semplici**, che apre il
rapporto (modello del rapporto, 21 ottobre 2026), si spiega cosa fanno i dealer e cosa significa per
il prezzo prima di dare qualunque numero. In **Cosa dicono per NQ** ogni termine si spiega la prima volta, e ogni numero dice cosa significa per il prezzo e quanto e'
provato. La regola intera e' nell'orchestratore, "Le regole che valgono per tutte le fasi".

## I Tuoi Dati

**Leggi per primo `dati/in-breve.json`, la sezione `gamma`**: e' un file solo, piccolo, con i numeri che ti servono gia' scelti e la **scala gia' calcolata** (future per 1% contro il volume medio di cassa): non rifarla. Apri i file grandi qui sotto solo per un dato che li' manca, e mai a pezzi: lo studio ha un tempo (15 ottobre 2026: leggendo i JSON grandi a pezzi questa fase ha sforato).

`dati/gex-<strumento>.json` (calcolato da `gex.py` dentro `studio/raccogli.py`). **La GEX e' quella dello
strumento dello studio** (`<strumento>` = NQZ6 o ESZ6; il moltiplicatore usato, 20 $ o 50 $ a punto,
sta in `moltiplicatore`): quella dell'altro si raccoglie solo se l'utente la chiede (`--con-es`), e solo
allora se ne parla. Il file contiene:
`tutte`, `settimana`, `prima` (lo 0DTE) con GEX nelle due convenzioni, zero gamma e muri;
`per_strike` (open interest, volume del giorno e GEX di call e put entro +-6% dal future);
`gex_per_scadenza` (GEX, zero gamma e muri di ogni scadenza);
**il future**:
- `future` con `future_quando` (l'ora dell'ultima lettura, non quella d'inizio `quando`) e `future_fonte`;
- `prezzo_bridge`: l'M1 subito dopo, con la `differenza`. Le distanze si misurano dal prezzo del bridge.
`per_scadenza` (gli strike pesanti di ogni scadenza, con la volatilita').
`scadenze` con volatilita' ATM, mossa attesa e **completezza**:
- `quotate` su `opzioni`, `instabile`, `strike_per_lato`;
- il segnale dell'OI: `oi_prima_lettura`, `oi_ultima_lettura`, `letture`, `oi_stabile`;
- il `future` di ogni scadenza.
Poi `scadenze_saltate`, e `oi_cambiato_rispetto_al_calcolo_precedente` (OI totale e cinque strike
pesanti contro l'ultimo calcolo dello stesso giorno). `volume_finestra` dice che finestra copre il volume
delle opzioni. Gli zeri si cercano su +-8%; 120 strike per lato (`limite_strike`). Per la
volatilita' realizzata e il VXN, `dati/macro.json`; per il volume del future, `dati/sedute-<strumento>.json`.
Lo storico dei giorni prima, se serve un confronto: `quant/dati/NQ/gex/<giorno>/`.

**Non hai Bash e non ricalcoli niente.** Se una catena e' incompleta (poche opzioni quotate, scadenze
saltate), lo dichiari come limite del dato: non si rifa' il calcolo.

## Leggi, Nell'Ordine

1. **Il regime** (convenzione A, quella dei fornitori e di ATAS): la GEX totale, il prezzo sopra o sotto
   lo zero gamma e quanto lontano; tutte le scadenze, entro sette giorni, lo 0DTE.
2. **La convenzione B** [GPP]: dove pesa di piu' la copertura se i dealer fossero corti di tutto. Il
   segno vero non si conosce: scrivilo ogni volta.
3. **Gli strike pesanti** da `per_strike` e `per_scadenza`; la scadenza piu' pesante della settimana.
4. **La mossa attesa** per scadenza contro la volatilita' realizzata della macro.
5. **La scala**: converti la GEX in future dello strumento da comprare o vendere per l'1% e confrontali con il volume
   giornaliero del future (`sedute-<strumento>.json`): e' la misura di quanto conta la gamma.
6. **I paper**, come ipotesi e non come regola (BDLM, BB, e i finding della sintesi piu' recente).

## Scrivi `3-gamma.md`

Nei **Livelli**: zero gamma, muri, strike pesanti, ciascuno con a cosa serve arrivarci e la riserva che
nessun paper prova questi livelli come supporti o resistenze. Negli agganci **Per il quadro**: gli
strike pesanti da confrontare con i livelli del COT e con il valore dell'order flow.
