# Come Si Leggono Le Opzioni Dal Vivo

**2 novembre 2026.** Da oggi le opzioni arrivano da quattro fonti, quasi tutte al minuto: la GEX viva di
NQ e di ES, il VIX e il VXN ricostruiti dalla catena CME, la stima di X-Ray su SPX, e il Cboe pubblico su
QQQ e NDX. Questo documento dice **in che ordine si leggono, cosa significa ogni numero per il prezzo e
dove si sbaglia**, durante la cassa e nello studio. Cosa sono le fonti e come si calcolano sta altrove:
[`le-opzioni-in-atas.md`](le-opzioni-in-atas.md) (la catena, la GEX, il VIX rifatto),
[`x-ray-in-vivo.md`](x-ray-in-vivo.md) (X-Ray), [lo strumento](../strumenti/xray/x-ray-lettore-e-dashboard.md)
(server, rotte, pagina).

Vale insieme agli [obblighi della lettura dal vivo](../../docs/research/metodo/gli-obblighi-della-lettura-dal-vivo.md):
**si fa il punto, non si danno segnali**. Le opzioni sono una delle fasi del punto (la riga "LE FASI" di
[come si risponde dal vivo](../../docs/research/metodo/come-si-risponde-dal-vivo.md)), non il punto intero.

## In Parole Semplici

- **Cosa si guarda**: quanto ha paura il mercato (la volatilita'), e come si coprono i dealer, cioe' chi
  vende le opzioni (la gamma). Una gamma positiva fa da **freno**: i dealer comprano quando il prezzo
  scende e vendono quando sale. Una gamma negativa fa da **acceleratore**: inseguono il prezzo.
- **Su NQ il segno dei dealer non si conosce**: lo si suppone (convenzione A: dealer lunghi di call e
  corti di put). Su ES c'e' in piu' X-Ray, che lo stima trade per trade, ma su SPX.
- **Durante la giornata le posizioni non si aggiornano**: l'open interest (quante opzioni sono aperte) e'
  quello di ieri sera. Se la GEX cambia in giornata, la cambiano **il prezzo e il tempo**, non posizioni
  nuove.
- **Niente di questo e' provato su NQ.** I paper sulla gamma sono su S&P 500 e su singoli titoli, e nessun
  livello (zero gamma, muri) e' stato provato come supporto o resistenza. E' contesto.
- **Il regime dice quanto, non dove** (5 novembre 2026, dal video sul bias dalle opzioni): freno o
  acceleratore parlano di quanto si allarga la giornata e di se un livello tiene, mai della direzione. E anche
  questo, sui nostri dati, non regge ancora: su 42 sedute di ES il regime delle 9:35 non ha allargato la
  giornata ([il bias del giorno](../../docs/research/metodo/il-bias-del-giorno.md)).

## Le Fonti, Con La Loro Eta'

| fonte | cosa da' | ritmo | dove | vale per |
|---|---|---|---|---|
| **GEX viva** (catena CME via ATAS) | GEX totale e 0DTE, zero gamma, muri, strike per strike, in convenzione A | 0DTE ogni minuto; le altre scadenze, fino a 70 giorni, una al minuto a rotazione | giro (tab 2), pagina "NQ · QQQ", `quant/dati/volatilita/<giorno>/gex-<NQ|ES>.jsonl`, `/api/nq/gex`, `/api/gex/tempo` | NQ (su ES non si mostra: vale X-Ray) |
| **gex.py** (stessa catena, stesso modello) | in piu': convenzione B, volatilita' ATM e mossa attesa per scadenza, GEX per scadenza | a richiesta, circa 50 secondi; il giro lo rilancia se ha piu' di 30 minuti | giro (tab 2), `dati/gex-<strumento>.json` dello studio | NQ ed ES |
| **VIX e VXN ricostruiti** (catena CME) | la paura a 30 giorni su S&P e Nasdaq, con la formula del Cboe | ogni 5 minuti a CME aperto | giro (riga volatilita'), pagina "Volatilita'", `serie.jsonl` | S&P (ES), Nasdaq (NQ) |
| **struttura a termine** (catena CME) | la volatilita' al prezzo di ogni scadenza | ogni ora; lo 0DTE ogni minuto | pagina "Volatilita'", `/api/vol/termine` | NQ ed ES |
| **X-Ray** (OptionsDepth, cache di ATAS) | il segno stimato dei market maker su SPX, zeri, picchi, delta scambiato per gruppo | ogni 15 secondi, **solo con l'indicatore aperto su un chart ES** | giro, pagina "SPX · X-Ray", `/xray` | SPX, quindi ES; su NQ solo contesto |
| **Cboe pubblico** (QQQ, NDX) | la catena di QQQ e NDX, con la GEX e la volatilita' a 30 giorni | ogni 15 minuti in cassa, con 15 minuti di ritardo; fuori cassa resta l'ultima chiusura | pagina "NQ · QQQ" e "Volatilita'" | contesto di NQ |
| **VIX e VXN ufficiali** (Yahoo) | il riferimento | quando la pagina li chiede | pagina "Volatilita'", `macro.json` | il metro degli altri |

**Prima regola: l'eta'.** La pagina "Fonti" e `/api/fonti` dicono l'eta' di ognuna. Una fonte ferma si
dichiara e non si legge come dato di adesso. Fuori dalla cassa il Cboe e lo spot di X-Ray restano alla
chiusura precedente: nel fine settimana il Cboe ha **tre giorni**.

## La Lettura Parte Dalla Pagina

Richiesta dell'utente del 2 novembre 2026: *"facciamo in modo che la lettura dell'agente sia in base a
visivamente cio' che vede l'utente"*. L'utente guarda **http://127.0.0.1:8790/**; l'agente legge **la stessa
pagina**, non un calcolo suo:

```bash
python quant/strumenti/xray/scripts/leggi_pagina.py              # Fonti, Volatilita', NQ · QQQ, Adesso, Livelli
python quant/strumenti/xray/scripts/leggi_pagina.py --foto <scratchpad>/foto-pagina   # e una foto per scheda
```

Lo script apre la pagina in un Chromium senza finestra e, scheda per scheda, stampa il testo visibile
(tessere, tabelle, note), la scelta attiva dei comandi e, per ogni grafico, **le serie e le linee come sono
disegnate** (dall'istanza di ECharts, non ricalcolate): l'ultimo punto e quello di un'ora prima, gli strike
piu' pesanti, le linee del prezzo, dello zero gamma e dei muri. Circa 10 secondi.

Le regole:

1. **Ogni numero delle opzioni si cita con la scheda dove l'utente lo vede**: *"NQ · QQQ > La GEX viva di NQ
   nel tempo: lo zero gamma e' sceso da 29.447 a 29.012 in un'ora"*. Cosi' l'utente ritrova il numero e la
   forma con un'occhiata.
2. **Se la forma conta, si guarda la foto** (`--foto`, poi Read sull'immagine): un salto, un incrocio, due
   linee che si toccano non si vedono dai numeri. Le foto stanno nella scratchpad e si cancellano.
3. **Il giro resta per l'order flow** (il pannello e' gia' "esattamente com'e' sul chart") e come riassunto
   delle opzioni a ogni messaggio. Giro e pagina leggono lo stesso server: se differiscono, e' l'istante (il
   giro legge l'archivio di un minuto prima) o un difetto, e lo si dice.
4. **Cio' che la pagina non mostra** si dice da dove viene: la mossa attesa e la convenzione B (solo nel
   giro, da `gex.py`), la GEX viva di ES nel tempo (solo nel giro e nell'archivio).

## Dove Si Legge Nella Pagina

La pagina e' in stile QuantData dal 5 novembre 2026 (x-ray-lettore-e-dashboard, "La Pagina"): ogni pagina ha
in cima una riga di **tessere** (la risposta) e sotto i **riquadri**. Un riquadro si cita con il suo titolo,
che non cambia con i comandi; una tessera con la sua etichetta. I valori stanno nella legenda del riquadro.

| passo della lettura | pagina > riquadro (o tessera) |
|---|---|
| l'eta' delle fonti | le pastiglie delle fonti nella barra in alto; **Fonti** > Lo stato di ogni fonte; per X-Ray, Le famiglie di X-Ray |
| 1 la paura a 30 giorni | **Volatilita'** > Il meteo · Nasdaq 100 (e · S&P 500): livello e fascia, movimento atteso oggi, implicita − realizzata, breve − mese, skew, e il verdetto fra le misure stile VIX (convergono se entro 1 punto); > Nel tempo · Nasdaq 100 (e · S&P 500) |
| 2 la paura vicina contro quella lontana | **Volatilita'** > Struttura a termine · NDX (e · SPX): i giorni alla scadenza in orizzontale, in scala logaritmica, con gli eventi del calendario |
| la paura sulle ali (le put lontane costano piu' delle call) | **Volatilita'** > Sorriso · NDX (e · SPX) |
| 3 il regime della gamma di NQ | **NQ · QQQ** > tessere Regime NQ e Zero gamma; > I livelli (zero gamma e muri delle tre fonti, con la distanza dal prezzo); > Profilo GEX per strike, in prezzi di NQ, comando "Tutte le scadenze" |
| 4 lo 0DTE | **NQ · QQQ** > tessera Zero 0DTE; > Profilo GEX per strike, comando "Solo oggi (0DTE)"; > La GEX viva di NQ nel tempo, la linea a puntini |
| 5 i muri e gli strike pesanti | **NQ · QQQ** > tessere Call wall e Put wall; > Profilo GEX per strike: le barre piu' lunghe, nel profilo scelto (o "Tutti affiancati"); per ES, **SPX · X-Ray** > Adesso > tessera Muri sul valore |
| 6 ES, solo X-Ray (in prezzi di ES, di default) | **SPX · X-Ray** > Adesso: tessere Regime (al prezzo, 0DTE, tutte; "Segnali opposti" se non concordano) e Flip piu' vicino; > Freno e acceleratore, prezzo per prezzo; > La scala dei prezzi (con il regime per scadenza sotto) |
| 7 gli zeri nel tempo | **NQ · QQQ** > La GEX viva di NQ nel tempo; per SPX, **SPX · X-Ray** > Livelli nel tempo > Il prezzo e i livelli della giornata |
| 8 QQQ e NDX | **NQ · QQQ** > Profilo GEX per strike: il profilo scelto (comando "Profilo": NQ, QQQ, NDX, Tutti affiancati), e nella legenda il prezzo vero di QQQ (Yahoo, anche nel pre-mercato) e NQ/QQQ; > I livelli |
| chi ha comprato o venduto delta oggi (SPX) | **SPX · X-Ray** > Adesso > Chi ha comprato delta oggi (0DTE e tutte le scadenze, per gruppo); minuto per minuto, Deriva > Delta accumulato per gruppo |
| i livelli di X-Ray per domani | **SPX · X-Ray** > Previsione > Livelli della prossima seduta; e le righe "prossima seduta" di Adesso > La scala dei prezzi |

## L'Ordine Della Lettura

Dal piu' ampio al piu' vicino, come il resto del punto. Ogni passo dice cosa guardare, cosa significa per il
prezzo e dove si sbaglia.

**Le quattro domande** (5 novembre 2026). Il giro, per NQ e per ES, e lo studio (`quattro-domande-<strumento>.json`)
mettono questi passi in quattro domande, dal video sul bias dalle opzioni. Cosa sono e quanto sono provate:
[il bias del giorno](../../docs/research/metodo/il-bias-del-giorno.md).

| domanda | passi qui sotto |
|---|---|
| **1 il regime**: i market maker fanno da freno o da acceleratore? | 3, 4, 6, 7 |
| **2 il meteo**: quanto si aspetta che si muova, e quanto costa l'assicurazione? | 1 (con implicita meno realizzata), 2 |
| **3 i muri sul profilo**: un muro delle opzioni sta vicino a un bordo del valore? | 5 |
| **4 il grilletto**: cosa fa l'order flow al livello | non sono le opzioni: le tab 3 e 4, e senza segnali |

**1. La paura a 30 giorni.** VXN ricostruito (NQ) e VIX ricostruito (ES), accanto agli ufficiali.
- *Per il prezzo*: piu' e' alta, piu' ampi ci si aspettano i movimenti. Il VXN diviso per la radice di 252 da'
  la mossa attesa di un giorno: con il VXN a 27, circa l'1,7%, cioe' circa 490 punti di NQ a 28.700.
- *Dove si sbaglia*: il ricostruito non e' l'indice ufficiale. Sui settlement del 30/10 il VIX rifatto era
  19,29 (ufficiale 19,42) e il VXN 27,86 (ufficiale 28,50). Conta **come si muove**, non il decimale.
- *Implicita meno realizzata* (dal 5 novembre 2026): il VIX o il VXN meno la volatilita' vera del future nelle
  ultime 20 sedute. Se e' sopra +2 punti l'assicurazione e' **cara**: le opzioni prezzano piu' movimento di
  quello che c'e' stato, ed e' la situazione normale nei periodi calmi. Se e' sotto −2 il mercato **si muove
  piu' di quanto le opzioni prezzano**, ed e' tipico delle fasi agitate. Soglie convenzionali, non provate.
- *Il verdetto della pagina* (Volatilita' > Il meteo): dal 5 novembre 2026 lo calcola il server, e **solo fra le
  misure "stile VIX"** (ricostruito, ufficiale, VXN da NDX, QQQ 30 giorni) con un punto negli ultimi 30 minuti:
  convergono se stanno entro un punto. L'IV30 del Cboe e l'ATM di X-Ray sono la volatilita' al prezzo, senza
  le ali, e stanno 3-4 punti sotto il VIX e il VXN: sono escluse, e la pagina lo dice. Fino al 4 novembre
  entravano nel confronto, e il "divergono" del 2/11 veniva da loro; quel giorno le misure stile VIX
  convergevano entro un punto.

**2. La paura vicina contro quella lontana** (la struttura a termine).
- *Per il prezzo*: se le scadenze dei prossimi giorni costano piu' di quella a 30 giorni, il mercato **paga
  per un evento vicino** (un dato, una trimestrale, un voto). Si cerca quale scadenza si gonfia e quale
  evento ci cade dentro, con il calendario di `docs/research/calendario/eventi.json`.
- *Dove si sbaglia*: una scadenza con la volatilita' assurda (il 2/11 la 16/11 di NQ a 100%) e' un dato
  sporco della catena, non paura: si scarta e si dichiara.

**3. Il regime della gamma di NQ** (GEX viva, tutte le scadenze).
- *Cosa guardare*: il segno della GEX totale, e il prezzo sopra o sotto lo zero gamma, e di quanto.
- *Per il prezzo*: sotto lo zero, in convenzione A, i dealer inseguono il prezzo (acceleratore); sopra,
  lo frenano. Lo zero e' il prezzo dove il comportamento cambierebbe.
- *Dove si sbaglia*: e' **una supposizione sul segno**. In convenzione B (dealer corti di tutto) la gamma e'
  negativa ovunque. Si scrive ogni volta.
- *Quanto, non dove* (5 novembre 2026): il regime dice quanto si allarga la giornata e se un livello tende a
  tenere o a rompersi, non la direzione. Non e' un bias del mattino: **si rilegge durante il giorno**, perche'
  lo 0DTE cambia col prezzo e col tempo. Sui nostri dati l'allargamento non c'e' ancora: su 42 sedute di ES
  il regime 0DTE delle 9:35 ha dato giornate un po' **piu' strette** (0,55 contro 0,74 del movimento atteso,
  p 0,24). E' l'ipotesi D di [`opzioni-gamma-es`](../studi/opzioni-gamma-es/1-idea.md), che si prova in avanti.
- *Se tutte le scadenze e lo 0DTE dicono cose opposte*, si scrive "segnali opposti": il regime non e' netto.

**4. Lo 0DTE** (le opzioni che scadono oggi alle 16:00 di New York).
- *Cosa guardare*: la GEX dello 0DTE, il suo zero, i suoi muri; la sua volatilita' e la mossa attesa fino
  alla chiusura (mossa = prezzo × volatilita' × radice del tempo rimasto in anni).
- *Per il prezzo*: lo 0DTE pesa sempre di piu' man mano che si avvicina la scadenza, perche' la gamma di
  un'opzione vicina al prezzo cresce quando il tempo finisce. Nell'ultima ora puo' dominare.
- *Dove si sbaglia*: **la GEX dello 0DTE cambia col prezzo, non con le posizioni**. Il 2/11, con lo stesso
  open interest, era +8 con NQ a 28.695 e +150 con NQ a 28.809: il prezzo era passato sopra gli strike
  delle call. Una GEX che sale in giornata non vuol dire "i dealer si sono comprati gamma".

**5. I muri e gli strike pesanti.**
- *Cosa guardare*: call wall e put wall (gli strike con piu' gamma di call e di put), e i due o tre strike
  piu' pesanti vicino al prezzo.
- *Per il prezzo*: sono i prezzi dove la copertura dei dealer pesa di piu'. Si confrontano con i livelli
  del valore e del COT: uno strike pesante che coincide con un bordo di valore e' una confluenza, da dire.
- *Dove si sbaglia*: **due strike quasi alla pari si scambiano il ruolo** da un minuto all'altro. Il 2/11
  il put wall di NQ e' passato da 28.000 a 29.000 e ritorno, quello di ES da 7.500 a 7.700; il call wall di
  NQ oscillava fra 29.000 e 31.000 (52 e 51 milioni di $). Un muro che salta non e' una notizia: il giro
  stampa "muri saltati" quando succede, e allora si nominano tutti e due.
- *Il muro sul bordo del valore, col regime* (5 novembre 2026, dal video sul bias dalle opzioni): un muro put
  vicino al bordo basso del valore, col freno, e' dove si guarda l'assorbimento e il ritorno al POC; con
  l'acceleratore puo' rompersi. Un muro call vicino al bordo alto, col freno, e' dove si guarda l'esaurimento;
  con l'acceleratore puo' essere sfondato verso l'alto. Il giro e lo studio stampano i muri entro 25 punti (NQ)
  o 6 (ES) da un bordo. **Non provato**: un caso solo dal 23/10 (ipotesi E di `opzioni-gamma-es`).

**6. Su ES vale solo X-Ray** (5 novembre 2026: prevale OptionsDepth, come sul chart di ATAS;
[x-ray-in-vivo](x-ray-in-vivo.md), "Come si legge", punto 2). Gamma al prezzo, flip, freno e acceleratore
piu' forti, GEX dei market maker per scadenza, **in prezzi di ES**. La nostra catena ES non si confronta
piu' e non si mostra.
- *Per il prezzo*: sopra il flip i market maker frenano, sotto accelerano; il freno e l'acceleratore piu'
  forti sono gli strike dove l'effetto e' piu' grande.
- *Dove si sbaglia*: X-Ray e' su SPX, e fuori dalla cassa il suo spot resta alla chiusura precedente. Un
  livello di SPX si porta su ES aggiungendo la base ES-SPX del momento (la pagina la dichiara). Le unita' di
  X-Ray non sono le nostre: conta il segno.

**7. Nel tempo: come si muovono gli zeri rispetto al prezzo** (pagina "NQ · QQQ", "La GEX viva di NQ nel
tempo"; il giro stampa la variazione dell'ultima ora).
- *Per il prezzo*: uno zero gamma che sale mentre il prezzo scende allarga la zona dell'acceleratore; uno
  zero che il prezzo attraversa cambia il regime supposto. E' la cosa che una fotografia sola non mostra.
- *Dove si sbaglia*: con l'open interest fermo, lo zero si sposta perche' la gamma di ogni strike cambia
  col prezzo e col tempo. E' una conseguenza del modello, non un movimento dei dealer.
- *E lo zero puo' saltare*: il 2/11 alle 10:11 di New York lo zero di NQ e' sceso da 29.523 a 29.095 in un
  minuto, mentre la GEX totale cambiava appena (da −555 a −486) e il prezzo di 32 punti. Quando la curva
  della GEX attraversa lo zero quasi piatta, basta poco per spostare il punto d'incrocio di centinaia di
  punti. Uno zero che salta cosi' si nomina come **zona**, non come prezzo, e il salto si dice. Nella pagina
  si vede come un gradino nella linea grigia tratteggiata (lo zero gamma; dal 5 novembre 2026 il flip e lo
  zero sono grigi tratteggiati in tutta la pagina).

**8. QQQ e NDX dal Cboe**, solo in cassa e solo se freschi: un secondo sguardo sullo stesso indice, con un
mercato diverso (le opzioni su ETF e sull'indice, non sul future). Se il loro zero e i loro muri, portati
sui prezzi di NQ, cadono lontano da quelli della catena CME, lo si dice: vuol dire che le due catene
raccontano posizioni diverse.

## I Momenti Della Seduta

Gli orari sono di New York, convertiti per data ([la regola](../../docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md)).

| quando | cosa cambia nella lettura |
|---|---|
| **prima della cassa** (fino alle 09:30) | open interest di ieri, catena CME viva, Cboe e spot di X-Ray fermi alla chiusura. Si leggono i passi 1-5; X-Ray solo per i livelli `outlook`, che erano la previsione per oggi |
| **l'apertura** (09:30-10:00) | X-Ray riparte con lo spot vero; il Cboe arriva con 15 minuti di ritardo. Il passo 6 diventa possibile |
| **un dato o un evento** (08:30, 10:00, 14:00 per la Fed) | la volatilita' 0DTE scende dopo il dato se non sorprende: si guarda la struttura a termine prima e dopo |
| **metà seduta** | il passo 7: dove si sono mossi gli zeri rispetto al prezzo dall'apertura |
| **ultime due ore** (14:00-16:00) | lo 0DTE pesa sempre di piu' e la sua volatilita' perde senso vicino alla scadenza (il tempo va a zero). I muri dello 0DTE sono quelli da nominare |
| **dopo le 16:00** | lo 0DTE e' scaduto: lo 0DTE diventa la scadenza di domani. X-Ray passa a `outlook`. La GEX di stasera vale con l'open interest di ieri fino al nuovo, domattina |

## Come Si Scrive: Una Lettura Lineare

Richiesta dell'utente del 2 novembre 2026: *"la lettura deve essere piu' chiara e lineare"*. La lettura che
l'aveva provocata aveva sei blocchi, elenchi dentro elenchi e una quarantina di numeri. Da allora la lettura
dal vivo ha **sempre la stessa forma**: sei righe, nello stesso ordine, una cosa per riga.

```text
IN BREVE      una frase: dove sta il prezzo e cosa sta succedendo
1 COSA E' CAMBIATO   dall'ultima lettura, una frase (se niente, "niente": e la lettura finisce qui)
2 IL PREZZO   il modello (balance, momentum, trend) e il regime, dove sta nel valore       (chart)
3 LA PAURA    sale o scende, e per quale evento si paga                                   (Volatilita')
4 I DEALER    freno o acceleratore su NQ, e quanto e' lontano il punto dove cambia         (NQ · QQQ)
5 ES          lo stesso su ES, solo da X-Ray, in prezzi di ES                              (SPX · X-Ray)
I PREZZI      una tabella: prezzo | cos'e' | se ci arriva          (tre o quattro righe, dall'alto in basso)
DA GUARDARE   una frase per ramo: cosa risolverebbe, nei due sensi
```

Le regole:

- **Una riga, un'idea, al massimo due numeri.** Un numero entra solo se cambia la conclusione; il resto sta
  nella pagina, e la scheda in parentesi dice dove.
- **Niente elenchi dentro elenchi**, niente blocchi per fonte. Le fonti si nominano solo quando non sono
  d'accordo: "le tre fonti concordano" basta.
- **Le parole si spiegano una volta, in fondo**, in una riga ("Le parole: zero gamma = ..."), e solo quelle
  usate.
- **I prezzi in tabella, in ordine di prezzo**, ognuno con cosa cambia arrivandoci (obbligo 6). Livelli di
  fonti diverse vicini fra loro diventano una riga sola, una zona.
- **I dubbi in una riga sola** in fondo (dati sporchi, fonti vecchie, cio' che non e' provato), non sparsi.
- **Gli studi del giorno** (quadro, fasi e loro eta') stanno nella riga IN BREVE se cambiano la lettura,
  altrimenti nella riga dei dubbi.

Esempio (2 novembre 2026, 10:59 di New York):

> **In breve**: NQ e' tornato giu' dentro il valore di oggi dopo il secondo rifiuto sopra venerdi'; i dealer
> restano dalla parte che accelera, su NQ e di nuovo anche su ES.
> 1. **Cosa e' cambiato**: il rifiuto a 28.891 e' stato seguito, da 28.891 a 28.710; ES e' tornato acceleratore.
> 2. **Il prezzo**: balance, dentro il valore di oggi (28.667-28.788); choppy (chart).
> 3. ...

## La Pagina Lettura

Richiesta dell'utente del 2 novembre 2026: *"possiamo fare una pagina nel nostro x ray con la lettura che
posso far partire quando te lo dico io e aggiorneresti il contenuto"*. La lettura lineare non resta solo in
chat: sta anche nella pagina **http://127.0.0.1:8790/#/lettura**, il primo gruppo in alto.

- **Quando si scrive**: quando l'utente chiede una lettura ("vedi ora", "aggiorna la lettura", "rifai
  l'analisi"). Mai da sola: la pagina mostra quanti minuti ha la lettura, e oltre 20 la dichiara vecchia.
- **Come si scrive**: l'agente mette la lettura in un JSON nella scratchpad, con la forma qui sopra (lo
  scheletro: `scrivi_lettura.py --esempio`), e la pubblica:

  ```bash
  python quant/strumenti/xray/scripts/scrivi_lettura.py <scratchpad>/lettura.json
  ```

  Lo script la rifiuta se manca una riga, se le righe non sono nell'ordine (cosa e' cambiato, il prezzo, la
  paura, i dealer, ES), se un prezzo non dice cosa cambia arrivandoci, se una riga supera 320 caratteri, o se
  contiene una parola da segnale (LONG, SHORT, ingresso, stop, bersaglio). La salva in
  `quant/dati/letture/<giorno di New York>/<HHMM>.json`; il server la serve (`/api/lettura`) e la pagina la
  mostra da sola entro 15 secondi.
- **Cosa mostra la pagina**: in cima le ore delle letture del giorno (si possono rileggere le precedenti),
  poi in breve, le cinque righe, la tabella dei prezzi, da guardare, dubbi e parole. Ogni riga ha il
  **rimando al riquadro** dove si vede il dato (`dove`: pagina e titolo del riquadro, o `chart` per il chart
  di ATAS): un clic apre la pagina, scorre fino al riquadro e lo evidenzia. Accanto, il **prezzo di adesso**
  dello strumento della lettura (dal server, `prezzoAdesso`), al suo posto nella tabella dei prezzi.
- **I prezzi** (dal 5 novembre 2026): ogni prezzo puo' avere `da` e `a` numerici (una zona); se mancano, lo
  script li ricava dal testo ("28.960-29.030" e' la zona 28.960-29.030, "28.812" un prezzo solo). Ogni riga
  puo' avere un'`etichetta` breve (al massimo 24 caratteri, con gli stessi controlli sulle parole da
  segnale): diventa una tessera in cima alla pagina.
- **In chat** la stessa lettura si scrive com'e', con il link alla pagina.
- **La forma si puo' cambiare**: e' una prima versione (`versione: 1`). Se l'utente la vuole diversa, si
  cambiano insieme questo documento, `scrivi_lettura.py` (le voci e i controlli) e
  `XrayWeb/src/pagine/lettura/Lettura.tsx`.

## Come Si Scrive Nello Studio

**Nello studio** (fase 3), l'agente legge `dati/opzioni-vive.json` (la GEX viva di NQ ed ES con la
variazione dell'ultima ora, e l'ultimo VIX e VXN ricostruiti), raccolto dalla fonte `gex` di
`studio/raccogli.py`, e lo riassume in `in-breve.json` -> `gamma.vive`. La GEX di `gex.py` resta la base del
rapporto, perche' ha la convenzione B, la mossa attesa e le scadenze; la viva dice com'e' adesso e come si e'
mossa.

## Quanto E' Provato

- **Misurato**: la GEX viva coincide con `gex.py` allo stesso istante (2/11: totale −561 contro −558, zero
  29.220 contro 29.215, stessi muri); il VIX e il VXN ricostruiti stanno entro un punto dagli ufficiali.
- **Supposto**: il segno dei dealer (convenzione A) su NQ. Su ES, stimato da un fornitore con un metodo non
  pubblicato.
- **Non provato**: che lo zero gamma, i muri o lo 0DTE muovano NQ. Ogni idea che li usa passa dalla scheda
  (`quant/studi/<idea>/1-idea.md`) e dal metodo; la storia della GEX viva comincia il 2 novembre 2026.
