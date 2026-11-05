# X-Ray: I Dati Di OptionsDepth, In Archivio E In Diretta

**30 ottobre 2026.** Di Options X-Ray fino al 29 ottobre si sapeva cosa mostrava, non cosa
contenesse. I valori stanno in una **cache cifrata** (`%APPDATA%\ATAS X\OptionsDataCache`). Dal 30
ottobre li decifra e li pubblica in JSON ogni 15 secondi il lettore `XrayLive`, mentre ATAS lavora,
in `quant/dati/xray/`. Dal 2 novembre lo stesso processo serve anche la pagina per leggerli,
**http://127.0.0.1:8790/**. Lo strumento (lettore e server, pagina, gestione, script) e' in
[`quant/strumenti/xray/`](../strumenti/xray/x-ray-lettore-e-dashboard.md).

## In Parole Semplici

- **Cosa sono**: la stima di OptionsDepth di **come sono messi i market maker sulle opzioni SPX**.
  Gli stessi numeri che X-Ray disegna sul chart di ES. Per ogni strike dicono se i market maker sono
  lunghi o corti di gamma, e minuto per minuto quanto delta comprano o vendono retail,
  professionisti, firm e broker.
- **Perche' contano**: la nostra GEX **suppone** il segno dei dealer (convenzione A o B), mentre
  questi dati lo **stimano** trade per trade. E' il pezzo che ci mancava
  ([`le-opzioni-in-atas.md`](le-opzioni-in-atas.md), "Come si potrebbe conoscere il segno").
- **Da dove vengono** (verificato il 2 novembre 2026 sulla [FAQ](https://optionsdepth.com/faq)):
  dai **dati del Cboe con il tipo di partecipante** di ogni trade (market maker, cliente, firm,
  broker), non dal nastro OPRA, che dice prezzo e quantita' ma non chi ha scambiato. SPX si scambia
  solo al Cboe. Per questo ci sono i gruppi `firm`, `broker`, `procust`, `nonprocust` nel DEX.
- **Cosa non sono**: non sono una misura, ma una stima di un fornitore con un metodo non pubblicato.
  Non sono NQ: valgono per SPX, quindi per ES. Su NQ sono solo contesto. Non sono provati: nessun
  livello di X-Ray e' stato verificato come supporto o resistenza.

## Cosa Arriva

Il plugin *Options X-Ray* di ATAS X scarica da OptionsDepth i dati su **SPX** e li mette in cache solo
**mentre l'indicatore e' aperto su un chart ES**: la copertura dipende da cosa si tiene aperto. Il
lettore li divide in famiglie, `<metrica>__<variante>`:

| famiglia | cosa e' | da quando |
|---|---|---|
| `levels__gamma` | **i livelli chiave**: zeri (flip) e picchi, `observed` (la seduta di oggi) e `outlook` (la prossima seduta) | 23 ottobre |
| `observed__gamma` · `observed__charm` | **il profilo per strike** della gamma (e della charm) osservata, un istante al minuto, ~200 strike | 8 ottobre |
| `forecast__*` · `nextday__*` | lo stesso profilo **previsto** fino alla chiusura e per il giorno dopo; ogni chunk e' una previsione nuova (`asOf`) | 8 ottobre |
| `gex__mm_<scadenza>` | **GEX dei market maker per strike**, call e put separate; scadenza `0dte`, `next_expiry`, `opex` | 31 agosto |
| `dex__<gruppo>_<scadenza>` | **il delta scambiato in un minuto**, per strike, da `firm`, `broker`, `procust` (clienti professionali), `nonprocust` (retail); `0dte` e `all`. **E' un flusso, non una posizione** (vedi sotto) | 31 agosto |
| `vex__mm_<scadenza>` | **vanna** dei market maker per strike (non vega: vedi "La volatilita' implicita") | 31 agosto |
| `pressure__0dte` | **Hedge Pressure**: delta da coprire sugli 0DTE, minuto per minuto | 31 agosto |
| `observed` · `forecast` · `nextday` (senza greca) | blocchi del formato vecchio, fino al 7 ottobre | - |

## Come Si Leggono

**Il segno e' quello della posizione stimata del gruppo**. Lo si e' verificato il 30 ottobre:
- le serie per strike sono **posizioni**: curve lisce che crescono durante la giornata;
- non sono flussi minuto per minuto;
- un valore GEX positivo vuol dire market maker **lunghi** di gamma su quello strike: coprendosi
  comprano sui ribassi e vendono sui rialzi, cioe' **freno**;
- un valore negativo vuol dire **acceleratore**.

Questo vale per GEX e VEX (colonne con `from` = 0).

**Il DEX per gruppo invece e' un flusso**. Lo si e' verificato il 2 novembre 2026, sul 30 ottobre:
- ogni colonna copre una finestra di 60 secondi (`from` = `ts` − 60 s, in 712 colonne su 713);
- i valori oscillano attorno allo zero, minuto per minuto;
- e' quindi il delta che il gruppo ha **scambiato in quel minuto**, non la sua posizione.

Fino a quel giorno lo si leggeva come posizione ("il retail e' corto"), anche in `xray_query.py`.
Il numero giusto e' la **somma dall'inizio della giornata**: il delta che il gruppo ha accumulato.
Positivo vuol dire che ha comprato delta, cioe' si e' caricato di rialzo. Il 30 ottobre, a fine
giornata, il retail aveva accumulato circa −22.000 sugli 0DTE e i firm circa +6.000.

Le unita' sono quelle di OptionsDepth e non sono dichiarate: si confrontano il segno e la forma, non
i numeri con la nostra GEX in $ per 1%.

**I minuti anomali del delta accumulato** (5 novembre 2026). Sommando il DEX minuto per minuto, alcuni minuti
fanno un salto di decine di volte il passo normale, e il salto sposta tutta la linea. Sull'archivio grezzo del
4 novembre (`dex__firm_all`) le cause sono tre:
- **la partenza**: la prima colonna del giorno (04:01 di New York, +14.100) non e' un minuto come gli altri;
- **la scadenza**: fra le 16:00 e le 16:02 di New York lo 0DTE scade ed esce dalla somma (16:01, −21.693);
- **il salto**: un minuto con una finestra normale di 60 secondi ma un valore fuori misura, spesso seguito dal
  suo ritorno (09:59 −14.685, 10:12 +15.430). Una colonna con la finestra diversa da 60 secondi (09:31, una
  finestra di 7 minuti, `from` diverso da `ts` − 60 s) si marca come **finestra**.

La regola, nel server (`Misure.Anomali`): Δ e' la differenza fra un minuto e il precedente, m la mediana dei |Δ|
non nulli del giorno; un minuto e' anomalo se |Δ| supera il piu' grande fra 20·m e un quarto dell'escursione della
serie. Il primo punto e' anomalo se vale piu' di 20·m. **I valori non si toccano**: il server li marca
(`/api/deriva`, `anomali`), la pagina interrompe la linea e li segna con ⚠. Una convenzione nostra, non di
OptionsDepth, e non provata.

**Il verso della Hedge Pressure** (`pressure__0dte`) **non e' documentato**: non si sa se un valore positivo
voglia dire che i market maker devono comprare o vendere. Finche' non lo si verifica, il server lascia `verso`
e `unita` a null e la pagina scrive "verso non dichiarato": si leggono la forma e il cambio, non il verso.

**I livelli chiave** (`levels__gamma`). Il significato dei campi e' stato verificato il 30 ottobre
contro i profili:

| campo | cosa e' |
|---|---|
| `kind 0` | uno **zero** della gamma, cioe' il *flip*: il prezzo dove il profilo cambia segno, istante per istante. Gli zeri 7.486, 7.650 e 7.665 coincidevano con i cambi di segno di `observed__gamma` |
| `kind 1` | un **picco positivo**: lo strike (indice in `prices`) dove il freno e' piu' forte, con il peso. 7.806, peso 382, era il massimo del profilo, 383 |
| `kind 2` | un **picco negativo**: dove l'acceleratore e' piu' forte (peso negativo) |
| `observed` | dall'inizio della sessione a adesso |
| `outlook` | la previsione per la **prossima seduta** (il 30/10, venerdi', puntava al 2/11): verificato sulle giornate dal 27 al 30 ottobre |

**Lo spot** (`spot` nelle GEX per strike) e' quello di SPX. **Fuori dalla cassa resta fermo alla
chiusura precedente**: il 30 ottobre alle 06:34 di New York segnava 7.719,28 mentre ES si muoveva.
Fra ES e SPX c'e' la base, il costo del finanziamento fino alla scadenza: ES dicembre e' sopra SPX
di qualche decina di punti. Un livello di SPX si porta su ES aggiungendo la base del momento.

**Gli orari** sono epoch in millisecondi UTC. Si scrivono nel fuso del mercato
([`gli-orari-si-scrivono-nel-fuso-del-mercato.md`](../../docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md)).

## La Volatilita' Implicita

**Cosa si e' trovato** (2 novembre 2026, prova esplorativa nella scratchpad). Il campo `vex` **non e' la
vega, e' la vanna**: la sensibilita' del delta alla volatilita'. Lo dice il rapporto `vex/gex`, strike per strike:
- e' identico per call e put allo stesso strike: la posizione si elide, resta solo il rapporto delle greche;
- **cambia segno al forward**, in tutte e tre le scadenze. Vega/gamma = S²σT, che e' sempre positivo;
  vanna/gamma = −S·d2·√T, che si annulla dove d2 = 0.

Dal rapporto si ricava allora la **volatilita' implicita per strike**, quella usata da OptionsDepth:
σ(K) ≈ S·ln(K/F)/(vex/gex), con F dove il rapporto passa per zero (d2 = 0). Le unita' tornano con
fattore 1, perche' i valori escono plausibili.

**Il confronto col VIX**: 29 sedute dal 22/9 al 30/10, alle 15:45 di New York, la volatilita' ATM
(media degli strike entro l'1% da F) della scadenza `opex`. Il VIX e il VIX9D sono le chiusure di Yahoo.

| | livelli | variazioni giornaliere |
|---|---|---|
| con il VIX | corr 0,89 | corr 0,73, segno concorde 20 su 28 |
| con il VIX9D | corr 0,90 | corr 0,70 |

Fra i due la relazione e' VIX ≈ 0,78·ATM + 6,6, con errore tipico 0,7 punti. Il 2/11 alle 05:27 di
New York la volatilita' ATM era 15,6, cioe' un VIX stimato di 18,8 contro il 18,83 di Yahoo: e' un
giorno fuori dal campione della retta.

**I limiti**:
- **il VIX vero non si rifa'**: la formula del Cboe vuole tutte le opzioni fuori dal denaro fino alle
  ali, e X-Ray ha strike solo per circa ±3% dallo spot. La varianza "stile VIX" che ne esce e' piu' bassa
  dell'ATM, perche' le ali mancano;
- la scadenza `opex` va da 35 giorni a meno di 1 e poi salta alla successiva (il 16/10), mentre il VIX
  e' sempre a 30 giorni;
- la scadenza 0DTE vicino alla chiusura e' inservibile, perche' T tende a zero;
- la retta e' stata stimata sulle stesse 29 giornate che misura: e' esplorazione, non validazione.

**A cosa serve**: e' una misura della paura sull'S&P presa da ATAS, minuto per minuto, anche fuori dalla
cassa, e con il sorriso (la volatilita' per strike). Il VIX di Yahoo resta il riferimento.

## Come Entra Nelle Letture

| dove | cosa | chi lo fa |
|---|---|---|
| **il giro d'orizzonte**, tab 2 | tre righe sotto la nostra GEX: lettore vivo o fermo, spot SPX, gamma allo spot (freno o acceleratore), zeri, picchi, GEX netta dei market maker per scadenza | `le_quattro_tab.py`, da `xray_query.righe_giro()` |
| **lo studio globale**, fase 3 | `dati/xray.json` (raccolto con la fonte `gex`) e la sezione `gamma.xray` di `in-breve.json` | `studio/raccogli.py`, da `xray_query.riassunto_dati()` |
| **la lettura dal vivo** ("vedi ora") | `python quant/strumenti/xray/scripts/xray_query.py riassunto` | l'agente, con `/xray` |
| **a occhio** | **la pagina http://127.0.0.1:8790/**: Adesso, Per strike, Heat map, Livelli nel tempo, Deriva, Previsione, in ES (o SPX), istante per istante; e, accanto alle altre fonti, la Volatilita' (VIX e VXN ricostruiti, X-Ray, Cboe, ufficiali) e NQ · QQQ. In stile QuantData dal 5 novembre 2026: tessere in cima, poi i riquadri; verde freno, rosso acceleratore | `XrayLive --serve` e `XrayWeb` ([lo strumento](../strumenti/xray/x-ray-lettore-e-dashboard.md#la-pagina)) |
| **la gestione** | lettore e server vivi, famiglie, avvio, compattazione | `XrayGui` |

**Le regole di lettura** di X-Ray da solo stanno qui sotto; **come si legge insieme alle altre fonti**
(GEX viva, VIX e VXN ricostruiti, Cboe), in cassa e nello studio, in
[`come-si-leggono-le-opzioni-dal-vivo.md`](come-si-leggono-le-opzioni-dal-vivo.md).


1. **Prima la freschezza**. Il lettore e' vivo? Quanti minuti ha l'ultimo chunk di ogni famiglia?
   Una famiglia ferma vuol dire un indicatore chiuso su ES: si dichiara, non si legge come dato di
   adesso.
2. **Su ES e SPX prevale X-Ray, ed e' l'unica GEX che si mostra** (5 novembre 2026, decisione
   dell'utente: "deve prevalere sempre quello di ATAS, ovvero di OptionsDepth, in tutto, mostriamo solo
   quello"). La nostra GEX della catena ES (`gex.py --strumento ESZ6`, la GEX viva di ES, convenzione A)
   non si stampa piu' nel giro, non si raccoglie nello studio e non ha piu' una pagina (i "Confronti" sono
   tolti). I livelli si scrivono **in prezzi di ES**, come li disegna ATAS sul chart: il livello di SPX piu'
   la base ES-SPX dichiarata (`xray_query.livelli_es`). La base della pagina e' la mediana di ES − SPX sui
   minuti di cassa della seduta prima; ATAS ne usa una sua, e fuori dalla cassa i due flip possono
   differire di qualche punto (il 5 novembre 2026: 7.761,7 contro 7.763,7). La catena ES resta solo per la
   volatilita' (VIX ricostruito, struttura a termine). **Su NQ non cambia niente**: X-Ray non esiste e la
   GEX di NQ resta la nostra, da tre fonti.
3. **Su NQ**: X-Ray e' il contesto dell'S&P 500, come la macro. Non sposta i livelli di NQ, e non si
   converte in prezzi di NQ.
4. **Ogni numero porta**: la fonte (stima di OptionsDepth su SPX), l'istante in ora di New York, e la
   riserva che non e' provato.

## Cosa Questo Cambia Per Il Metodo

- **Resta dentro la decisione 2** (dati solo da ATAS): arrivano dalla cache di ATAS, non da un
  abbonamento nostro a OptionsDepth.
- **Il segno dei dealer diventa stimato, non vero**: e' quello di un fornitore, su SPX. Per la prima
  volta si puo' **misurare quanto spesso la convenzione A ha ragione su SPX/ES**. E' il primo studio
  possibile.
- **La storia e' corta**: GEX per strike dal 31 agosto 2026, profilo dall'8 ottobre, livelli dal 23
  ottobre. Si accumula da qui in avanti, a indicatori aperti. Un mese non valida niente.
- **Nessun backtest con questi dati prima della scheda** (obbligo 1): un'idea che li usa passa da
  `quant/studi/<idea>/1-idea.md`, con il paper e cosa la ucciderebbe.

## Perche' I Valori Erano Illeggibili

Non era una licenza da aggirare: era il **formato**. Gli strati sono tre:
1. AES-CBC, con chiave `HKDF(MachineGuid + utente)`. Non e' scritta da nessuna parte, ma si ricalcola.
2. gzip.
3. MessagePack, con i numeri per strike in `float32` **planarizzato**: prima tutti i byte bassi, poi i
   secondi, e cosi' via. Chi legge i byte in ordine vede rumore.

Lo schema completo e' in `docs/ATASX_XRAY_CACHE.md` del repo `ATAS_Tool`, dove si e' fatta l'analisi.
La verifica: ogni file ha per nome lo SHA-256 del suo contenuto in chiaro, e il lettore lo ricalcola
a ogni chunk. Il 30 ottobre: 10.684 chunk letti, 0 errori.
