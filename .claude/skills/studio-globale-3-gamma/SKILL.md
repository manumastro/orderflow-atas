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
- [`quant/metodo/x-ray-in-vivo.md`](../../../quant/metodo/x-ray-in-vivo.md): i dati di X-Ray (OptionsDepth
  su SPX), cosa sono, come si leggono e cosa non sono;
- [`quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md`](../../../quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md):
  l'ordine in cui si leggono insieme volatilita', GEX viva, X-Ray e Cboe, e le trappole gia' viste;
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
strumento dello studio** (`<strumento>` = NQZ6; per ESZ6 dal 5 novembre 2026 il file non c'e', vedi "Leggi", punto 3; il moltiplicatore usato, 20 $ o 50 $ a punto,
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

**X-Ray** (dal 30 ottobre 2026): `in-breve.json` -> `gamma.xray`, e per intero `dati/xray.json`. E' la
stima di OptionsDepth **su SPX**, letta dalla cache di ATAS: il lettore (`lettore.vivo`), lo spot SPX
(fermo fuori dalla cassa), i `livelli` (zeri e picchi, `observed` oggi e `outlook` per la
prossima seduta), il `profilo_gamma` (gamma allo spot, zeri vicini), la `gex_market_maker` per scadenza
(netto: positivo = market maker lunghi di gamma, freno), il `dex_per_gruppo` (retail, clienti pro,
firm, broker), la `pressione_0dte`, e `famiglie_ferme_da_piu_di_30_minuti`. **Il DEX non e' una
posizione** (verificato il 2 novembre 2026): e' il delta che il gruppo scambia minuto per minuto, e
`accumulato_oggi` e' la sua somma dall'inizio della giornata (positivo = il gruppo ha comprato delta,
si e' caricato di rialzo), con `ultimi_30_minuti` accanto. Si legge come flusso, non come "chi e' lungo".
Le unita' sono di OptionsDepth: conta il segno, non il numero.

**La GEX viva e la volatilita' ricostruita** (dal 2 novembre 2026): `in-breve.json` -> `gamma.vive`, per
intero `dati/opzioni-vive.json`. `gex_viva.NQ` e `gex_viva.ES`: l'ultimo minuto (GEX totale e 0DTE, zero
gamma, muri, convenzione A, OI di ieri), `variazione` nell'ultima ora (prezzo, zero, GEX) e
`muri_nella_finestra` (piu' di uno strike = due muri quasi alla pari che si scambiano il ruolo: si nominano
tutti e due). `volatilita`: l'ultimo VIX e VXN ricostruiti dalla catena CME e, se freschi, quelli del Cboe.
La GEX di `gex.py` resta la base del rapporto; la viva dice com'e' adesso. Una GEX dello 0DTE che cambia in
giornata la cambiano prezzo e tempo, non posizioni nuove: l'OI e' fermo.

**Le opzioni in quattro domande** (dal 5 novembre 2026): `in-breve.json` -> `gamma.quattro_domande`, per intero
`dati/quattro-domande-<strumento>.json`. Contiene:
- il regime: `al_prezzo` su tutte le scadenze, `zero_dte`, `lettura`, e `provato` con l'esito della prova;
- il meteo: movimento atteso, **implicita meno realizzata**, skew, breve contro mese;
- i muri vicini ai bordi del valore (`muri_sul_profilo.vicini`, entro 25 punti su NQ e 6 su ES);
- il grilletto.

Su ES vengono da X-Ray, su NQ dalla GEX viva. Cosa sono e quanto sono provate:
[il bias del giorno](../../../docs/research/metodo/il-bias-del-giorno.md).

**Non hai Bash e non ricalcoli niente.** Se una catena e' incompleta (poche opzioni quotate, scadenze
saltate), lo dichiari come limite del dato: non si rifa' il calcolo.

## Leggi, Nell'Ordine

1. **Il regime** (convenzione A, quella dei fornitori e di ATAS): la GEX totale, il prezzo sopra o sotto
   lo zero gamma e quanto lontano; tutte le scadenze, entro sette giorni, lo 0DTE.
2. **La convenzione B** [GPP]: dove pesa di piu' la copertura se i dealer fossero corti di tutto. Il
   segno vero non si conosce: scrivilo ogni volta.
3. **Il segno stimato da X-Ray** (se `gamma.xray` c'e' e il lettore e' vivo). Prima la freschezza: una
   famiglia ferma e' un indicatore chiuso su ES, e si dichiara. **Nello studio di ES vale solo X-Ray**
   (5 novembre 2026, decisione dell'utente: prevale OptionsDepth, come sul chart di ATAS): non c'e'
   `gex-ESZ6.json` e non c'e' la convenzione A di ES. I livelli sono in `gamma.xray.in_prezzi_di_es`
   (flip, freno e acceleratore piu' forti, gamma al prezzo, con la base ES-SPX dichiarata), e i punti 1, 2,
   4 e 6 qui sotto si fanno su X-Ray (GEX dei market maker per scadenza al posto della GEX A). **Per NQ**
   valgono la GEX della catena CME e la GEX viva; X-Ray non sposta i livelli di NQ e non si converte in
   prezzi di NQ.
3bis. **NQ ed ES letti insieme** (dal 9 novembre 2026 lo studio e' uno: l'utente, "le informazioni
   optionsflow possono aiutarsi a vicenda"). L'asse del rapporto e' il confronto: i due regimi (freno o
   acceleratore, distanza in percentuale dallo zero gamma) concordano? lo 0DTE di qui e di la'? il flusso
   di X-Ray (DEX per gruppo, pressione 0DTE) e la variazione della GEX viva di NQ vanno nello stesso
   verso? VIX contro VXN, implicita meno realizzata di qui e di la'. Si confrontano regimi, segni, flussi,
   volatilita' e tempi, mai livelli convertiti. Quanto e' provato il passaggio di informazione fra i due:
   la ricerca `quant/paper/ricerche/2026-11-09-opzioni-fra-indici.md` e la sua sintesi.
4. **Gli strike pesanti** da `per_strike` e `per_scadenza`; la scadenza piu' pesante della settimana.
5. **La mossa attesa** per scadenza contro la volatilita' realizzata della macro.
6. **La scala**: converti la GEX in future dello strumento da comprare o vendere per l'1% e confrontali con il volume
   giornaliero del future (`sedute-<strumento>.json`): e' la misura di quanto conta la gamma.
7. **Le quattro domande** (`gamma.quattro_domande`).
   - Il regime **dice quanto, non dove**. Se tutte le scadenze e lo 0DTE dicono cose opposte, si scrive
     "segnali opposti".
   - La frase del video ("con l'acceleratore i livelli tendono a rompersi") si scrive **come ipotesi, con
     l'esito accanto**: su 42 sedute di ES il regime delle 9:35 non ha allargato la giornata (ipotesi D).
   - Implicita meno realizzata: sopra +2 l'assicurazione e' cara; sotto −2 il mercato si muove piu' del
     prezzato.
   - I muri vicini a un bordo del valore vanno negli agganci del quadro, con il regime accanto (ipotesi E,
     non provata).
8. **I paper**, come ipotesi e non come regola (BDLM, BB, e i finding della sintesi piu' recente).

## Scrivi `3-gamma.md`

Una sezione **X-Ray** breve (5-8 righe) dopo il regime: freschezza, segno stimato dei market maker,
zeri e picchi di SPX (nello studio di ES in prezzi di ES, senza confronto con la convenzione A); la riserva ogni volta (stima di un
fornitore, su SPX, non provata). Nei **Livelli**: zero gamma, muri, strike pesanti, ciascuno con a cosa serve arrivarci e la riserva che
nessun paper prova questi livelli come supporti o resistenze. Una sezione **Le quattro domande** (4-6 righe):
regime, meteo con implicita meno realizzata, muri sul bordo del valore, ciascuno con l'esito della prova
accanto. Negli agganci **Per il quadro**:
- gli strike pesanti da confrontare con i livelli del COT e con il valore dell'order flow;
- i muri vicini a un bordo del valore, con il regime.
