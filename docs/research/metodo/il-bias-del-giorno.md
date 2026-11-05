# Il Bias Del Giorno: Cosa Prendiamo Dai Due Video

Stato: **contesto della lettura dal vivo e dello studio globale, non una regola** (5 novembre 2026). Non
produce segnali. Le due idee che si potevano provare sono state provate lo stesso giorno, e **nessuna delle
due ha retto** sui nostri dati.

## In Parole Semplici

Il 5 novembre l'utente ha portato due video su come trovare "il bias del giorno", cioè da che parte guardare
prima dell'apertura.

- **Il primo usa la price action.** Mese, settimana e giorno salgono "a scalini"? Se sono tutti d'accordo il
  video compra o vende soltanto; se non lo sono, lavora poco e solo ai bordi.
- **Il secondo usa le opzioni**, con un cruscotto simile a quello di OptionsDepth (la fonte del nostro X-Ray).
  Fa quattro controlli in fila:
  1. **il regime**: i market maker smorzano i movimenti o li amplificano?
  2. **il meteo**: quanto si aspetta che si muova il mercato?
  3. **i muri**: dove stanno i muri delle opzioni rispetto al profilo del volume?
  4. **il grilletto**: l'order flow.

**Cosa ne abbiamo preso**: le definizioni e l'ordine dei controlli, perché aiutano a leggere. Il giro e lo
studio li stampano ogni volta. **Cosa no**: le promesse. Su NQ dal 2017 al 2024 gli scalini allineati non
dicono da che parte va la cassa dopo. Sulle 42 sedute di ES con X-Ray, il regime delle opzioni all'apertura
non ha allargato la giornata, anzi. **Non prendiamo nemmeno** i "piani di volo" con ingresso, conferma e
perdita massima: sono segnali, e la lettura dal vivo non ne dà
([come-si-risponde-dal-vivo](come-si-risponde-dal-vivo.md)).

## Le Fonti

Le trascrizioni, incollate dall'utente; autore e link non dati:

| sigla | video | file |
|---|---|---|
| **V1** | il bias dalla price action, su ES, dal mensile al 5 minuti | [`2026-11-05-il-bias-dalla-price-action.txt`](../fonti-video/2026-11-05-il-bias-dalla-price-action.txt) |
| **V2** | il bias dalle opzioni, su SPX ed ES | [`2026-11-05-il-bias-dalle-opzioni.txt`](../fonti-video/2026-11-05-il-bias-dalle-opzioni.txt) |

Si citano `[V1 · m:ss]` e `[V2 · m:ss]`. Sono **fonti di idee**, come il corso di Fabio: una cosa che dicono
diventa regola solo passando la pipeline (`quant/metodo/`), mai perché è convincente. Ogni video mostra
una giornata sola, scelta da chi lo fa.

---

## 1. Gli Scalini (V1)

**La definizione** `[V1 · 0:49-1:47]`. Una candela fa uno **scalino su** se ha tre cose insieme:
- un massimo più alto della candela prima;
- un minimo più alto;
- **una chiusura sopra il massimo della candela prima**.

Senza la chiusura il prezzo ha **visitato** il nuovo livello ma non l'ha **accettato**: è una **pausa**.
Lo **scalino giù** è lo stesso, rovesciato. Tutto il resto è pausa, anche una candela dentro la precedente.
Si giudica sempre **l'ultima candela chiusa**, perché quella aperta cambia ancora `[V1 · 3:14]`.

**I tre tempi** `[V1 · 2:53-3:35]`: il mese (la corrente), la settimana (la marea), il giorno (l'onda).
Sono **allineati** quando tutti e tre fanno uno scalino nello stesso verso, **misti** altrimenti.

**Il ping-pong** `[V1 · 2:17-2:53]`. Dopo l'ultimo scalino del giornaliero il mercato è in equilibrio fra
due muri: il massimo e il minimo delle giornate da allora. Un muro è rotto **solo se una giornata chiude
oltre e ci resta**: un tocco fuori non basta. È l'accettazione di Fabio, e il balance dei tre modelli
([i-tre-modelli-del-live-q1](i-tre-modelli-del-live-q1.md)).

**Cosa dice la prova** (5 novembre 2026, [`quant/studi/scala-dei-tre-tempi/`](../../../quant/studi/scala-dei-tre-tempi/1-idea.md)):
- **smentita**, sul campione NQ 2017-2024, dentro campione, con 5 varianti dichiarate e criteri sigillati;
- con i tre tempi allineati (106 giornate), la cassa dopo non va nel verso della scala più spesso del caso:
  nullo del quando p 0,858, Sharpe deflazionato 0,072;
- le giornate "miste" non sono più strette: mediana 165,5 punti contro 151,0, cioè un po' più larghe;
- il fuori campione non è stato guardato.

**Come si usa allora.** Come **descrizione** della struttura, non come previsione:
- dove il prezzo è stato accettato e dove no;
- da quanto il giornaliero è in equilibrio, e fra quali muri.

I muri del ping-pong entrano fra i livelli che si confrontano con le opzioni (sotto, al punto 3) e fra le
confluenze dello studio. **Non si scrive mai** "tempi allineati, si compra soltanto".

**Convenzioni del codice**. Dal 5 novembre 2026 la regola sta in un posto solo, il server della pagina X-Ray
(`quant/strumenti/xray/XrayLive/Server/Scalini.cs`, rotta `/api/scalini?radice=NQ|ES`), perché serve anche ai
bordi del valore della pagina e delle quattro domande; `FabioOrderFlow/tools/gli_scalini.py` la chiede al
server e la stampa nel giro. Senza server il giro scrive "gli scalini non si leggono".
- le candele di mese e settimana si costruiscono dalle barre del chart Daily del continuo di ATAS, per giorno
  di borsa CME (dalle 17:00 di Chicago del giorno prima alle 16:00 del giorno);
- i prezzi prima del rollover si aggiustano con lo scarto che dà il ruolo della tab 1, perché il continuo
  non è aggiustato;
- il primo mese del chart, se ha meno di 15 giornate, si scarta.

---

## 2. Le Opzioni In Quattro Domande (V2)

L'utente sta imparando le opzioni: qui ogni termine è spiegato, e per ognuno si dice cosa abbiamo noi. Come si
leggono insieme agli altri numeri delle opzioni: [come-si-leggono-le-opzioni-dal-vivo](../../../quant/metodo/come-si-leggono-le-opzioni-dal-vivo.md).

### 1. Il regime: strada asciutta o ghiaccio

**Cosa vuol dire** `[V2 · 0:52-1:20]`. I market maker vendono opzioni e poi si coprono sul future. Se sono
**lunghi di gamma**, per coprirsi comprano quando il prezzo scende e vendono quando sale: fanno da
**ammortizzatori**, e noi lo chiamiamo **freno**. Se sono **corti di gamma** fanno il contrario e amplificano
la spinta: è l'**acceleratore**. Il punto dove si passa dall'uno all'altro è il **flip** (o zero gamma).

**Cosa dice il video** `[V2 · 1:39-2:16]`:
- su 101 sedute, con gamma 0DTE negativa la giornata è più larga di un terzo (70 punti contro 51);
- la direzione non cambia: **il regime dice quanto, non dove**;
- il regime non è un bias del mattino: **si rilegge durante il giorno** `[V2 · 2:42-3:04]`.

**Cosa abbiamo**:
- **su ES**: X-Ray, cioè la gamma osservata al prezzo, il flip e la GEX dei market maker per 0DTE, prossima
  scadenza e opex, tutto in prezzi di ES;
- **su NQ**: la GEX viva della nostra catena CME, in convenzione A (supposta), su tutte le scadenze e sullo 0DTE.

**Cosa dice la prova** (5 novembre 2026, [`quant/studi/opzioni-gamma-es/`](../../../quant/studi/opzioni-gamma-es/1-idea.md), ipotesi D):
- **il video misura male**. Classifica il giorno col regime "per gran parte della giornata", ma la GEX dello
  0DTE cambia col prezzo: un giorno che si muove molto la rende negativa da solo, e l'effetto si gonfia;
- **la misura giusta** usa il regime delle 9:35 di New York e divide l'ampiezza del resto della cassa per il
  movimento atteso;
- **su 42 sedute di ES** (dall'8/9 al 4/11) i giorni negativi stanno a 0,55 del movimento atteso e quelli
  positivi a 0,74, p 0,24: **verso opposto al video**, e un allargamento di un terzo è poco compatibile con
  questi dati;
- **sono poche sedute**, tutte in un periodo solo. La prova vera si fa in avanti: il criterio è già scritto,
  60 sedute nuove dal 6 novembre e p ≤ 0,05.

**Come si legge allora.**
- Si dice il regime e da cosa viene.
- Si dice la frase del video ("con l'acceleratore i livelli tendono a rompersi") **come ipotesi**, con
  accanto l'esito della prova.
- Se tutte le scadenze e lo 0DTE dicono cose opposte, si scrive **"segnali opposti"**: il regime non è netto.

### 2. Il meteo: quanto si muoverà

**Cosa vuol dire** `[V2 · 3:15-4:10]`:
- il **movimento atteso**: quanto le opzioni che scadono oggi prezzano da qui alla chiusura;
- **implicita meno realizzata**: quanto costano le opzioni rispetto al movimento che il mercato ha fatto
  davvero. Se le opzioni costano di più, l'assicurazione è **cara**: il mercato si aspetta più movimento di
  quello che c'è stato;
- lo **skew**: quanto costa la protezione dal ribasso (le put) rispetto al biglietto della lotteria al rialzo
  (le call). Che le put costino di più è normale; conta quando **salta** all'improvviso.

**Cosa abbiamo**:
- movimento atteso, skew e struttura a termine c'erano già, nella pagina Volatilità (dal 5 novembre 2026 "Il meteo · Nasdaq 100" e "Il meteo · S&P 500");
- **dal 5 novembre** c'è anche **implicita meno realizzata**: il VIX o il VXN meno la volatilità vera del
  future nelle ultime 20 sedute, chiusura su chiusura, dal chart M30 del contratto. Sopra +2 punti è
  "assicurazione cara", sotto −2 "si muove più del prezzato". Le soglie sono una convenzione, non provate.

**Il vento e la vela** `[V2 · 4:10-4:51]`: la **charm** e la **vanna**.
- La charm dice come cambia la copertura **col passare del tempo**, anche se il prezzo sta fermo: crea una
  deriva, più forte nel pomeriggio sugli 0DTE.
- La vanna dice come cambia la copertura **quando cambia la volatilità**: se il VIX scende, i market maker
  possono trovarsi a comprare.
- X-Ray le ha tutte e due (`observed__charm`, `vex__mm_*`), ma **non le leggiamo ancora**. Per il video
  stesso sono contesto per i giorni di evento, non uno strumento di tempo.

**Il flusso dei premi** `[V2 · 4:51-5:16]`: i premi comprati e venduti dai market maker. **Non c'è** nei
nostri dati. Abbiamo il delta scambiato per gruppo (firm, broker, clienti, retail), che è un'altra cosa.

### 3. I muri sul profilo: i buttafuori alle porte

**Cosa vuol dire** `[V2 · 5:16-8:38]`. Il profilo del volume è la stanza: il POC è il divano, VAL e VAH sono
le pareti. I muri delle opzioni sono **buttafuori alle porte**. Quello che conta è **un muro vicino a un
bordo del valore**. La regola del video, in quattro casi:

| | col freno (gamma positiva) | con l'acceleratore (gamma negativa) |
|---|---|---|
| **muro put vicino al bordo basso** | si cerca l'assorbimento e il ritorno al POC | può rompersi |
| **muro call vicino al bordo alto** | si cerca l'esaurimento e il ritorno al POC | può essere sfondato verso l'alto |

**Cosa abbiamo.** Il giro e lo studio confrontano i muri con i bordi del valore:
- **i muri** su ES sono freno e acceleratore più forti e flip di X-Ray; su NQ sono call wall, put wall,
  muri e zero gamma dello 0DTE;
- **i bordi** sono VAL, VAH e POC della cassa di ieri e di oggi e i muri del ping-pong;
- **vicino** vuol dire entro 25 punti su NQ ed entro 6 su ES, una convenzione.

Un livello di SPX si porta su ES con la base ES-SPX: la regola è in [x-ray-in-vivo](../../../quant/metodo/x-ray-in-vivo.md).

**Cosa dice la prova** (ipotesi E): i livelli di X-Ray ci sono solo dal 23 ottobre, e c'è **un caso solo**
(il 3/11: muro put a 3 punti dal VAL, gamma negativa, e il muro ha tenuto). Si registra e basta.

### 4. Il grilletto: l'order flow

`[V2 · 8:32-8:38]`: "il livello dice dove, il regime cosa è probabile, l'order flow quando". Il grilletto è
l'order flow delle tab 3 e 4, con l'assorbimento misurato dalla footprint
([la-footprint-e-il-delta-per-prezzo](la-footprint-e-il-delta-per-prezzo.md)). **Nella lettura dal vivo non
si tira**: si descrive cosa succede al livello, senza ingressi.

---

## Dove Sta Nel Codice

| cosa | dove | chi lo legge |
|---|---|---|
| gli scalini e il ping-pong | la regola in `XrayLive/Server/Scalini.cs` (`/api/scalini`, una copia sola); `FabioOrderFlow/tools/gli_scalini.py` ne e' il client: giro, tab 1; studio, `scalini-<strumento>.json` | la fase 4 e il quadro |
| le opzioni in quattro domande | `quant/strumenti/xray/scripts/xray_query.py` (`quattro_domande`); giro, tab 2, per NQ ed ES; studio, `quattro-domande-<strumento>.json` | la fase 3 e il quadro |
| implicita meno realizzata | `XrayLive` (`Raccolta.Realizzata`, `MisureVol.InBreve`); pagina Volatilità, "Il meteo" | la lettura, la fase 3 |
| i muri del ping-pong fra le confluenze, e il regime accanto | `studio/raccogli.py`, `confluenze.json` | il quadro |

## Cosa Non Prendiamo, E Perché

- **I piani di volo** (quattro ipotesi, due per lato, ognuna con la conferma e la perdita massima decisa
  prima) `[V1 · 3:49-6:19]`: sono ingressi. La lettura dal vivo non dà segnali, e la fase delle condizioni
  armate è chiusa (CLAUDE.md, "Le fasi chiuse restano chiuse").
- **"Queste regole non sbagliano mai"** `[V1 · 3:37-3:47]`: vuol dire solo che la definizione è precisa. Quanto
  prevede è un'altra cosa, e sui nostri dati non prevede.
- **"Allineati: si compra soltanto"** `[V1 · 3:18-3:28]`: smentito su NQ 2017-2024.
- **I numeri del video sulle opzioni** (101 sedute, 70 contro 51 punti): una fonte non verificabile, con una
  misura che si gonfia da sola. I nostri, su 42 sedute, vanno nel verso opposto.
