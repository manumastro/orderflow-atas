# The Triple AAA Framework: Trascrizione Integrale Del Dossier

> **Stato dal 18 settembre 2026: riferimento, non fonte che comanda.**
>
> Il dossier e' di Fabio e resta citabile. Descrive lo stesso setup del live Q1 dentro una
> impalcatura a tre tier con un **gate orario** — la chiusura M30 dell'IVB del Tier 01 — che **nel
> live non esiste**: Fabio opera anche in premarket e sull'apertura, cambiando la taratura dei big
> trades, e cio' che aspetta e' la stabilita' del prezzo, non un permesso dell'orologio.
>
> La fonte che comanda e' [`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md). Quando
> dossier e live divergono, vince il live; quando dossier e immagine divergono, vince l'immagine.
> Nessuna condizione armata discende piu' da questo file.

Fonte: `fabio_course/ivbaaa/` — sette pagine, `1.webp` … `7.webp`.
Documento originale: **Strategy Dossier v3.0**, *The Triple AAA Setup*, a cura di Fabio
'Fabervaale' Valentini, dal programma *Blood Sweat & Scalps*. Accompagna il primo live del corso
(`fabio_course/fabio_q1/`).

Stato: **fonte primaria trascritta**. Questo documento riporta cio' che il dossier dice, non cio'
che e' stato verificato. La distinzione vale per intero: descrivere non e' validare. Cosa il
repository ha effettivamente misurato, e con quale esito, sta in fondo alla pagina, nella sezione
[Cosa Il Repository Ha Gia' Misurato](#cosa-il-repository-ha-gia-misurato).

Il testo del dossier e' in inglese. La trascrizione lo riporta **alla lettera**, refusi compresi
(`Absorbtion` nella pagina di copertina e' scritto cosi' nell'originale). Il commento intorno e'
in italiano ed e' chiaramente separato dalla citazione.

## Mappa Delle Pagine

| File | Sezione del dossier | Pagina |
|---|---|---|
| [`1.webp`](../../../fabio_course/ivbaaa/1.webp) | Copertina, principio operativo, mappa del sistema | 01 / 06 |
| [`2.webp`](../../../fabio_course/ivbaaa/2.webp) | Tier 01 — Bias filter: l'IVB | 02 / 06 |
| [`3.webp`](../../../fabio_course/ivbaaa/3.webp) | Tier 02·A — Mean reverting: fade dei bordi | 03 / 06 |
| [`4.webp`](../../../fabio_course/ivbaaa/4.webp) | Tier 02·B — Trend following: il Triple AAA | 04 / 06 |
| [`5.webp`](../../../fabio_course/ivbaaa/5.webp) | Tier 03 — Trigger e conferma: le tre tecniche | 05 / 06 |
| [`6.webp`](../../../fabio_course/ivbaaa/6.webp) | Libreria dei pattern e checklist giornaliera | 06 / 06 |
| [`7.webp`](../../../fabio_course/ivbaaa/7.webp) | Tier 02·C — **Triple A+**, il setup a R:R piu' alto | inserto, senza numero |

La settima pagina non e' numerata nella sequenza 01-06: e' un inserto marcato `02 · C`, quindi
appartiene al Tier 02 come terzo modello accanto a Mean Reverting (02·A) e Triple AAA (02·B).

---

## 01 / 06 — Copertina, Principio Operativo, Mappa Del Sistema

> **STRATEGY DOSSIER · V3.0**  ·  ORDER FLOW  ·  AUCTION MARKET THEORY
>
> # The Triple *AAA* Framework
>
> *A three-tier order-flow execution system for auction markets.*
>
> A method by **Fabio 'Fabervaale' Valentini**
> FROM THE BLOOD SWEAT & SCALPS PROGRAM
>
> **OPERATING PRINCIPLE**
> *Auction*
> *Absorbtion* [sic]
> *Aggression*
>
> **01 · Bias.**
> The 30-minute IVB sets directional permission. Before it prints, prefer mean reverting.
>
> **02 · Location & Model.**
> Am I trend following - IVB Break and Retest / Deep Retrace?
> Am I fading the edges - Mean Reverting at VAH / VAL?
>
> **03 · Trigger & Execution.**
> Candle Framing
> Deep Trades
> 40 Range Deep Effort (NQ)

### Mappa del sistema, come appare nel diagramma

```text
                    IVB · 30-minute bias filter
          Sets directional permission - prefer MR before it prints
                              |
              +---------------+---------------+
              |                               |
      Mean reverting                 Trend following · Triple AAA
      Fade at VAH · VAL              IVB Break and Retest
      LOW RISK · 2 SL MAX/DAY        or Deep Retrace to VAL / Extreme Delta
              |                      IVB DIRECTION ONLY
              |                               |
              +---------------+---------------+
                              |
        +---------------------+---------------------+
        |                     |                     |
  Candle framing         Deep trades           Deep effort (NQ)
  Delta % confirmation   Absorb - retrace -    40-range deep-effort
  · 40 range             continue              entry
```

Il diagramma dice una cosa che il testo da' per scontata: i tre trigger del Tier 03 **non sono
alternativi ai modelli**, stanno sotto a entrambi i rami. Il modello sceglie dove; il trigger
sceglie quando. Vale sia per il mean reverting sia per il trend following.

---

## 02 / 06 — Tier 01, Il Bias Filter

> `01 · BIAS FILTER`
>
> # The 30-minute *IVB* is the day's gate.
>
> *It decides the single question that every trade must answer before it's allowed to exist:*
> **which direction am I trading today?**
>
> ## What the IVB is
>
> The Initial Volume Balance is the range printed during the first 30 minutes of the session -
> the opening auction's acceptance zone. It's the market's first vote on fair value.
>
> When price **closes outside** that range on the 30-minute close, the market has cast a
> directional vote. That vote is your permission.
>
> **RULE**
> Trend-following setups (Triple AAA) **only fire in the direction of the IVB break.** A break
> higher means longs are permitted; a break lower means shorts. Period.
>
> **EXCEPTION**
> Mean reversion is the only setup permitted against the IVB - and it pays for that privilege
> with tighter risk: **max 2 stop-losses per day.** After the second stop, the session is over.

### La tabella del bias

| | |
|---|---|
| RANGE WINDOW | First 30 min of NY/RTH |
| SIGNAL | 30-min close outside the range |
| ABOVE HIGH | **Longs only** for trend setups |
| BELOW LOW | **Shorts only** for trend setups |
| INSIDE RANGE | Mean reversion only, or stand aside |

Lo schema a fianco e' etichettato `IVB BREAK · SCHEMATIC`, con `IVB HIGH` e `IVB LOW` tratteggiati,
una barra che rompe al rialzo e la didascalia: *"Price closes above IVB high - day's bias = long."*
Sotto, la barra dei permessi: `LONGS PERMITTED` solo dopo la rottura.

> *The IVB doesn't tell you **where** to enter. It tells you **which way** you're allowed to enter.
> The rest of the system fills in the rest.*

**Nota di lettura.** La finestra e' dichiarata: *first 30 min of NY/RTH*. Il gate e' quindi
ancorato all'apertura di New York (13:30-14:00 UTC, 15:30-16:00 ora italiana con l'ora legale),
non a Londra e non all'apertura di Globex. Prima che quella candela chiuda, il dossier non
concede setup direzionali: solo mean reverting, o stare fermi.

---

## 03 / 06 — Tier 02·A, Mean Reverting

> `02·A · MEAN REVERTING`
>
> # Fade the edges of the *value area*.
>
> *Where the auction is rejecting, delta tells you who's winning - and who's about to be run over.*

### FADE AT VAH · SHORT

> **Absorbed big buy - big sell aggression**
>
> Price pushes up into the Value Area High. Aggressive buyers hit the offer - and **get absorbed**.
> No price follow-through. The next print is a big *sell* that drives the tape back into the
> profile.
>
> `LONG WICK TOP`   `BUY DELTA ABSORBED`   `SELL AGGRESSION`

### FADE AT VAL · LONG

> **Absorbed big sell - big buy aggression**
>
> Price probes the Value Area Low. Aggressive sellers lean in - and **get absorbed**. The low
> holds. The next print is a big *buy* that lifts the tape back toward value.
>
> `LONG WICK BOTTOM`   `SELL DELTA ABSORBED`   `BUY AGGRESSION`

### La tabella operativa

| | |
|---|---|
| WHERE | VAH (short) · VAL (long) |
| TRIGGER | Delta outlier visible on footprint |
| CONFIRM | Wick + absorption + aggression-flip candle |
| STOP | Just beyond the absorbed wick |
| TARGET 1 | POC (point of control) / profile midline |
| TARGET 2 | Opposite VA edge |
| DAILY CAP | **Hard stop after 2 SL** |

> **WHY IT WORKS**
> At VA edges the market is asking *"do we accept new value here?"* Absorption answers *"no"* -
> you are punching a wall here - and the next aggressive print prices that rejection.

> **RISK ENVELOPE**
> Mean reversion is the only setup that runs against IVB bias. The cost of that privilege is
> **lower size and a 2-SL-max daily cap.** Two stops = session over.

L'immagine a fianco, `DELTA PRINT AT VAL`, mostra un profilo con tre colori: arancio `VALUE AREA`,
giallo `OUTSIDE VA`, e una linea verde orizzontale lunga etichettata `BUY DELTA AT VAL` — la
stessa figura che la libreria dei pattern chiamera' *VAL delta print*.

**Nota di lettura.** La sequenza e' in tre tempi e va letta in ordine: **stoppino** (il prezzo e'
andato oltre il bordo e non e' rimasto), **assorbimento** (l'aggressione che ha spinto non ha
prodotto prezzo), **flip di aggressione** (la stampa successiva e' grossa e dal lato opposto).
Due su tre non bastano: il dossier chiede la candela che ribalta l'aggressione come conferma.

---

## 04 / 06 — Tier 02·B, Trend Following, Il Triple AAA

> `02·B · TREND FOLLOWING · TRIPLE AAA`
>
> # Press the *IVB break*, but only with order-flow confirmation.
>
> *The break is permission - not the entry. Three distinct triggers convert that permission into
> a trade.*

### TRIGGER 01 — Break + high-delta bounce

> Price breaks IVB in direction. On the first retrace or pullback, price bounces off a
> **high-delta cluster** visible on the volume profile. If big trades print, market participants
> are defending the area.
>
> `IVB BREAK`   `DELTA BOUNCE`

### TRIGGER 02 — Break-and-retest of IVB

> Price breaks IVB, then returns to the IVB level as support (long) or resistance (short).
> Absorption holds the line, and the subsequent aggression resumes the direction. Clean
> mechanical entry.
>
> `IVB RETEST`   `HOLDS AS S/R`

### TRIGGER 03 — Deep Effort (NQ) - 40 Range

> IVB breaks - sellers stack exhausting - price falls to a Deep Effort zone. Use zone as entry,
> read candle framing to confirm. Stop just beyond the Deep Effort zone. **40 Range only**
>
> `DEEP EFFORT ZONE`   `40 RANGE ONLY`

### The IVB Break trade, in sequence

> **01 · Alignment.** Price is moving in the direction of the IVB break. No exceptions for trend
> setups.
>
> **02 · Absorption.** On the retrace or retest, the opposite side hits and fails - a deep-delta
> print on the wrong side gets absorbed without breaking structure.
>
> **03 · Aggression.** The in-direction side takes over with a visibly larger delta print - the
> block that signals the reload.
>
> Three confirmations stack: Confluences = higher probability. **The Triple AAA Setup.**

> **STOP LOGIC**
> Stop sits **just below the deep-effort print** (longs) or just above it (shorts). If the level
> that held absorption gets re-broken, the thesis is invalidated - get out.

Lo schema `IVB BREAK-AND-RETEST · SCHEMATIC` marca tre momenti sulla linea `IVB HIGH`: `break`,
`retest + absorb` (con una stampa da **92** sulla candela del retest) e `continuation`.

**Nota di lettura.** Qui si scioglie il nome. Le tre A non sono tre setup: sono i tre elementi
che devono impilarsi **nello stesso trade** — *Alignment*, *Absorption*, *Aggression* — sopra il
principio operativo dichiarato in copertina (*Auction, Absorbtion, Aggression*). Manca una delle
tre e il trade non esiste, non e' un trade piu' debole.

---

## 05 / 06 — Tier 03, Trigger E Conferma

> `03 · TRIGGER & CONFIRMATION`
>
> # Three techniques to *time* the pull.
>
> *Setup tells you **where**; execution tells you **when**. All three read the same signal -
> absorption on one side, aggression on the other.*

### TECHNIQUE 01 — Candle framing

> Watch the delta% on the forming candle and read the candle profile. When the auction flips,
> prepare to enter. **Works best on 40 Range.**
>
> *The flip isn't a moment - it's a decay, then a reversal. Watching the percentage gives you two
> candles of early warning.*

Il grafico `DELTA % · PROGRESSIVE FLIP` mostra la sequenza esatta, barra per barra:

```text
-88   -65   -40   -18   +12   +45   +72   +85
 \_____ decadimento _____/\_____ inversione _____/
```

### TECHNIQUE 02 — Deep trades & block-and-reload

> A break of a key level, a retrace, and a reload. The tape shows *seller* at the break, then
> absorbed *buyers* on the retrace, then *sellers* resume - sell absorbed in the **wick**, buy
> aggression in the **body**.
>
> **BLOCK-AND-RELOAD · BULLISH**
> Big *sell* absorbed in the wick at the bottom of a candle, followed by aggressive *buy* filling
> the body. Same mechanic, one candle.

Lo schema `THE SIGNATURE · ABSORB - RETRACE - CONTINUE` numera i tre momenti sulla `KEY LEVEL`:
① *break* (stampa **88**), ② *absorb* (stampa **81**), ③ *reload* (stampa **107**), poi
`continuation`.

### TECHNIQUE 03 — Deep Effort (NQ) - 40 Range

> IVB breaks - the market retraces - price falls to a **Deep Effort zone**. Use that zone as your
> entry, stop just beyond. Mirror for shorts. **40 Range only.**
>
> `IVB BREAK · ALIGNED`   `DEEP EFFORT LOCATED`   `ENTRY ZONE = PRINT`   `STOP BELOW / ABOVE PRINT`

**Nota di lettura.** *Block-and-reload* e' lo stesso meccanismo del Triple AAA compresso dentro
una candela sola: l'assorbimento sta nello stoppino, l'aggressione riempie il corpo. E' questo il
motivo per cui il dossier insiste sulla barra a 40 range: serve una barra abbastanza grande da
contenere entrambe le fasi e abbastanza piccola da poterle distinguere.

---

## 06 / 06 — Libreria Dei Pattern E Checklist

> `REF · VISUAL PATTERN LIBRARY`
>
> # Know the tells *before* the chart shows them.

| Stampa | Nome | Descrizione integrale |
|---|---|---|
| **95** | BEARISH · ABSORB TOP — *Buy absorbed at wick top* | Long upper wick, buyers spent size without gain. Expect sell continuation. |
| **107** | BULLISH · ABSORB BOT — *Sell absorbed at wick bottom* | Long lower wick, offer held. Expect buy continuation. |
| **81 · 88** | FLIP — *Absorb & aggression* | Buy at the high absorbed, then purple sell aggression - short entry. |
| **%** | FRAMING — *Delta % decay at VAL* | Delta gets more positive, value area changes, flip. |
| ▪ | DEEP EFFORT — *Stacked exhaust - zone* | Purple sellers stack and fail; green deep-effort zone supports. |
| ◆ | TRIPLE AAA FUEL — *VAL delta print* | Long green line = deep buy at VAL. The ignition for Triple AAA. |

### `OPS · DAILY EXECUTION CHECKLIST`

> **PRE-SESSION · BIAS**
> - [ ] Read COT report and do daily profile framing
> - [ ] Mark ETH VAH, VAL, POC, previous day high, low
> - [ ] any "red envelope" news expected?
>
> **WAIT FOR TIER 1**
> - [ ] 30-min candle closes outside IVB? Bias locked.
> - [ ] Inside? Mean Reverting only, tighter risk.
>
> **TIER 2 · SETUP CHECK**
> - [ ] Mean Reverting: at VAH / VAL with visible delta outlier?
> - [ ] AAA: IVB broken + retest / absorb / aggression?
>
> **TIER 3 · EXECUTION**
> - [ ] Candle framing: delta % and VA flipping?
> - [ ] Deep trade signature: absorb - wall punch - go?
> - [ ] Stop placed just beyond trigger
>
> **DAY-LEVEL GUARDRAILS**
> - [ ] Mean Reverting: max 2 stop-losses. Second SL = done.

> *Every entry reads the same tell - **absorption** on one side, **aggression** on the other. The
> rest is just risk management.*
>
> `DOSSIER V3.0 · FABIO 'FABERVAALE' VALENTINI · BLOOD SWEAT & SCALPS · END · 06/06`

**Nota di lettura.** La prima riga della checklist collega questo dossier al resto del repository:
*"Read COT report and do daily profile framing"* e' il lavoro descritto in
[`analisi-istituzionale.md`](analisi-istituzionale.md) e [`profile-framing.md`](profile-framing.md).
La seconda riga — marcare VAH, VAL, POC della sessione estesa piu' massimo e minimo del giorno
prima — e' esattamente cio' che la procedura in [`livelli-sul-chart.md`](livelli-sul-chart.md)
manda al chart.

---

## Inserto — Tier 02·C, Il Triple A+

> `02 · C · LOCATION & MODEL · TRIPLE A+`   —   `HIGHEST R:R`
>
> # A+ — The conviction break, the deep retrace, the launch.
>
> *IVB breaks with size. Price extends. Then it comes **all the way back** to VAL — where the deep
> effort lives — and shoots up harder than it came down.*

### ① CONVICTION — The break has size

> IVB breaks with visibly higher delta and volume than the preceding candles. Not a wobble — a
> commitment. This is what makes the A+ different from any other break.

### ② DEEP RETRACE — All the way back to VAL

> Price pulls back not just to IVB — it goes all the way through, into the opposite side of value.
> The move appears dead; late longs are being trapped.

### ③ LAUNCH — Big absorb, bigger push

> At VAL, deep effort absorbs the sell. The very next candle is aggressive buying — size, delta,
> body. Target is the prior break high. Often, well beyond.

> **WHY A+ IS THE BEST R:R OF THE SYSTEM**
> The stop is tiny — **just below the VAL absorb wick**. The target is the prior swing high, often
> beyond. The whole journey down gave the market time to shake out weak hands, and the whole
> journey back up is fuelled by those same traders chasing. Mirror everything for shorts
> (conviction break down → retrace to VAH → reject → drop).

Lo schema `A+ · THE FULL CYCLE · LONG` numera quattro momenti fra `IVB HIGH` e `VAL`:
① `CONVICTION BREAK`, ② `DEEP RETRACE`, ③ `VAL ABSORB` — con una stampa da **142** sullo stoppino
e una da **98** sul corpo della candela successiva, `entry` subito dopo, `stop just below VAL` —
④ `LAUNCH`, fino a `NEW HIGH · T2`.

**Nota di lettura.** L'A+ e' il caso in cui il ritracciamento e' cosi' profondo da attraversare
tutto il valore: non si ferma all'IVB come il Trigger 02, arriva al bordo opposto. Per questo lo
stop e' minuscolo rispetto al bersaglio. E' anche il piu' raro dei tre, e il dossier non ne
dichiara la frequenza.

---

## Cosa Aggiunge Il Dossier Rispetto A Quello Che Gia' Avevamo

Il primo live insegna la **location**: profile framing, dove si costruisce il valore, i vuoti, i
bordi. Il dossier aggiunge i tre pezzi che il live lascia impliciti:

1. **Il gate direzionale.** L'IVB a 30 minuti su NY/RTH decide se la giornata concede long, short
   o niente. Prima che quella candela chiuda, il dossier ammette solo mean reverting. Nel live
   questa regola non e' enunciata in forma cosi' netta.
2. **La separazione fra permesso, luogo e momento.** Tier 01 dice da che parte, Tier 02 dice dove,
   Tier 03 dice quando. Sono tre filtri in serie, non tre segnali da sommare.
3. **I limiti di rischio dichiarati.** Massimo due stop al giorno sul mean reverting, e la seduta
   finisce. E' l'unico numero duro di tutto il documento.

E ne chiarisce uno che avevamo letto male: **il mean reverting non e' il setup di default**, e' la
deroga. Costa taglia piu' piccola e cappello sulle perdite proprio perche' va contro il bias.

## Cosa Il Repository Ha Gia' Misurato

Va detto con precisione, perche' i nomi si somigliano.

La fase sistematica chiusa il 15 settembre 2026, archiviata in
[`archivio-modello-40r/`](../archivio-modello-40r/), ha misurato una **meccanizzazione parziale**
di alcuni elementi di questo dossier — la barra a 40 range, il flip di delta della *candle
framing*, i big trades, la velocita' del tape — su un mese di NQ. Esito: **49-51% con obiettivo
1:1**, indistinguibile dal modello nullo. In particolare il flip di delta (la condizione 01) non
porta informazione direzionale in nessuno dei due segni, verificato con test appaiati dentro
sessione a due scale e tre ampiezze di barriera.

Questo **non** e' una misura del Triple AAA. Quello che e' stato testato e' un sottoinsieme
eseguito meccanicamente, senza il gate dell'IVB, senza il requisito di location e senza la
discrezionalita' che il dossier da' per acquisita. Il risultato dice che *quei tre trigger da
soli, senza il resto, non producono un edge* — che e' esattamente cio' che il dossier stesso
afferma quando scrive **"the break is permission - not the entry"**.

Resta non misurato, e va detto, tutto il resto: l'IVB come filtro, i due rami Tier 02, l'A+.
Documentarli non li valida.

## Cosa E' Osservabile Col Data Bridge

Tradotto negli osservabili che ATAS espone davvero, tramite
[`contratto-data-bridge.md`](contratto-data-bridge.md):

| Elemento del dossier | Osservabile | Come |
|---|---|---|
| IVB (range dei primi 30 minuti) | si' | `candles` sulla finestra 13:30-14:00 UTC, massimo e minimo |
| Chiusura fuori dall'IVB | si' | chiusura della barra M30, o M1 aggregate |
| VAH / VAL / POC | si' | `profile` con sessione dichiarata, o footprint da `candles --levels` |
| Delta % sulla candela in formazione | si' | `delta` e `volume` per barra; la percentuale e' delta/volume |
| Assorbimento allo stoppino | parziale | il footprint da' bid/ask per prezzo; "assorbimento" resta una lettura, non un campo |
| Big trades / deep effort print | si' | `cumulative --min-volume`, con la soglia dichiarata accanto al risultato |
| Zona di deep effort | no | e' una zona disegnata a mano dalla sequenza delle stampe |
| "Red envelope news" | no | fonte esterna, non nel bridge |

Le prime sei righe sono misurabili e ripetibili. Le ultime due restano discrezionali, e il
dossier non pretende il contrario.

## Convenzioni Dichiarate

Dove il dossier lascia un numero aperto, questo repository fissa una convenzione e la dichiara
qui, invece di lasciarla nella conversazione:

- **Finestra IVB**: 13:30-14:00 UTC, la prima mezz'ora della cash di New York. Il dossier dice
  *"first 30 min of NY/RTH"*.
- **Sessione cash**: 13:30-20:00 UTC, come gia' fissato in
  [`profile-framing.md`](profile-framing.md).
- **"Visibly larger delta print"**: il dossier non da' una soglia. Qualsiasi soglia usata in una
  misura va scritta accanto al risultato, mai assunta.
- **"40 Range"**: barre a range di 40 punti su NQ. Non e' una scelta neutra: l'archivio ha
  mostrato che allargare la barra alza il win rate solo perche' l'attrito fisso di esecuzione si
  riduce rispetto a R, e il modello nullo sale nello stesso modo.
