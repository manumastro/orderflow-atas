# Analisi Istituzionale: COT E Data Bridge

Stato: **metodo descrittivo**. Non produce segnali, non autorizza esecuzioni, non definisce soglie
operative. Documenta come si costruisce, con le fonti disponibili in questo repository, una lettura
di chi sta posizionandosi sul Nasdaq e di cosa sta ottenendo.

Questo documento e' la fonte canonica del metodo. Le singole rilevazioni stanno in
[`../cot/`](../cot/) e [`../sessioni/`](../sessioni/); il contratto tecnico del bridge sta in
[`contratto-data-bridge.md`](contratto-data-bridge.md).

## Cosa Significa "Istituzionale" Qui

Due cose diverse, che vanno tenute separate perche' vivono su orologi diversi e rispondono a
domande diverse.

| | COT | Data Bridge |
|---|---|---|
| domanda | **chi e' posizionato, e come sta cambiando** | **chi sta agendo adesso, e cosa ottiene** |
| unita' | contratti netti per categoria di trader | volume, delta e prezzo per barra e per livello |
| frequenza | settimanale | millisecondo |
| ritardo | tre giorni strutturali | nessuno |
| copertura | dieci anni | le barre caricate nel chart |
| attribuzione | categorie dichiarate alla CFTC | nessuna: si osserva l'aggressivita', non l'identita' |

Il COT dice chi ha il libro; il bridge dice chi sta pagando lo spread. Nessuno dei due dice cosa
fara' il prezzo, e **non vanno fusi in un unico punteggio**: il primo e' un contesto che cambia una
volta a settimana con tre giorni di ritardo, il secondo e' una misura che cambia ogni secondo.
Sommarli produrrebbe un numero senza unita' di misura.

---

# Livello 1: Il COT

## La Trappola Che Viene Prima Di Tutto

Tradingster pubblica **due report diversi per lo stesso strumento**, su pagine diverse:

```text
legacy-futures/209742    ->  Non-Commercial, Commercial, Non-Reportable
fin/209742               ->  Dealer, Asset Manager, Leveraged Funds, Other Reportable, Non Reportable
```

Nel live del corso i due vocabolari vengono usati come sinonimi — *"non-commercial is asset
manager"* — e non lo sono. Sono tassonomie distinte applicate agli stessi open interest. Ogni
rilevazione di questo repository dichiara quale vista usa, e confronti fra le due non sono
ammessi senza dirlo.

Esiste inoltre la variante **futures only** e la variante **futures + options**, con numeri
diversi. Qui si usa futures only; va dichiarato ogni volta.

## Come Si Estrae

Le pagine Tradingster costruiscono i grafici con amCharts e caricano nel browser, dentro
`AmCharts.charts[...].dataProvider`, **l'intera serie settimanale del decennio** — netto, long,
short per categoria, piu' l'OHLC settimanale. Il numero in cima alla pagina e' solo l'ultimo punto
di una serie gia' presente.

La raccolta usa il server MCP Playwright: si apre la pagina, si legge il `dataProvider`, si salva
in JSON. Non e' scraping dell'HTML e non dipende dal layout.

```text
https://tradingster.com/cot/legacy-futures/13874+    S&P 500 Consolidated
https://tradingster.com/cot/legacy-futures/209742    Nasdaq-100 Mini
https://tradingster.com/cot/legacy-futures/124603    DJIA x $5
https://tradingster.com/cot/legacy-futures/239742    Russell 2000 E-Mini
https://tradingster.com/cot/futures/fin/209742       Nasdaq-100 Mini, vista TFF
```

```bash
python3 FabioOrderFlow/tools/build_cot_cross_index.py <cartella-json> \
        docs/research/data/cot-legacy-indices-weekly.csv
```

Serie disponibili nel repository: [`cot-tff-nasdaq-weekly.csv`](../data/cot-tff-nasdaq-weekly.csv)
(522 settimane dal 2016-09-13) e
[`cot-legacy-indices-weekly.csv`](../data/cot-legacy-indices-weekly.csv) (2.032 righe su quattro
indici).

## Le Convenzioni Dichiarate

Sono scelte di questo repository, non fatti. Cambiarle cambia i numeri.

```text
net    = long - short, per ciascun gruppo
z52    = z-score del net sulle 52 settimane precedenti, deviazione di popolazione
pct3a  = percentile del net dentro le ultime 156 settimane
il percentile sulle variazioni confronta |delta net| con le 521 variazioni disponibili
```

## Livello Contro Flusso: La Distinzione Che Fa Il Lavoro

E' la regola 2 del [percorso del progetto](../percorso-del-progetto.md) e la sola lettura COT che questo repository
considera affidabile.

**Il flusso** e' quanto si e' mosso nell'ultima pubblicazione. **Il livello** e' dove si trova il
netto rispetto alla propria distribuzione storica. Una variazione grande non implica uno stato
estremo, e la confusione fra i due e' il modo piu' comune di sbagliare a leggere il COT.

Esempio misurato, rilevazione dell'8 settembre 2026: i Leveraged Funds cambiano netto di
**-17.780**, variazione piu' grande dell'**89%** delle 521 settimane del decennio. Il livello
raggiunto e' netto -31.872, **z52 -0,12, 59° percentile**: assolutamente ordinario. Flusso
notevole, livello no.

Nello stesso rilievo va letto **come** il netto si muove: sul Nasdaq legacy, al report del primo
settembre, il netto migliora di 15.851 contratti quasi interamente per **chiusura di short**
(-15.049), non per apertura di long (+802). Il netto da solo non distingue i due casi. E' anche la
ragione tecnica per cui l'indicatore ATAS `COT Net positions` non basta e la fonte esterna resta
necessaria.

## La Metrica Cross-Index E Il Suo Esito

Il live enuncia una regola di conferma incrociata fra indici americani: se la maggioranza mostra
nell'ultima pubblicazione una variazione forte nella stessa direzione, il contesto e' coerente.
Resa numerica in due modi, entrambi dichiarati:

```text
Criterio A, skew:   skew = (dlong - dshort) / (|dlong| + |dshort|)
                    soglia |skew| >= 0,30; consenso 3 su 4, nessuno contrario

Criterio B, z:      z = delta netto / dev.st. del delta netto sulle 52 settimane precedenti
                    soglia |z| >= 1,0; consenso 3 su 4, nessuno contrario
```

| Criterio | Verdetto | Settimane | NQ a 4 settimane, media | Quota positive |
|---|---|---|---|---|
| A | long | 58 | +1,13% | 62,1% |
| A | short | 57 | +1,23% | 66,7% |
| A | nessuno | 404 | +1,66% | 68,8% |
| B | long | 3 | +2,96% | 100,0% |
| B | short | 12 | -2,73% | 41,7% |
| B | nessuno | 404 | +1,60% | 66,0% |

**Il criterio A non separa nulla**: consenso long e consenso short hanno la stessa risposta
successiva, e nessuno dei due batte le settimane senza consenso. **Il criterio B separa nella
direzione attesa su 12 e 3 occorrenze**, che con finestre sovrapposte e un decennio a trend
rialzista non sono una verifica.

Conseguenza registrata: **il COT resta contesto, non trigger**, e non c'e' motivo di costruirci un
indicatore ATAS. Dettaglio in
[`tradingster-cross-index-2026-09-11.md`](../cot/tradingster-cross-index-2026-09-11.md).

## Il COT Fra Un Report E L'Altro: La Data Diventa Un Livello

Il limite del ritardo di tre giorni vale se si usa il COT come **segnale temporale**. Nel live
Fabio non lo usa cosi', e la differenza e' sostanziale. Lui conferma di leggerlo solo alla
pubblicazione:

> *"No, no, no. I don't check this every day. I only check this when a new release comes out."*

Ma poi fa un'altra cosa, ed e' quella che gli da' valore anche nei giorni successivi:

> *"When we see that they are distributing order from the 13th of January, and we go back to
> price, and we see that the 13th of January was exactly on the value area, now this rejection at
> the top of the value area [...] they start to make sense, yes or no? These three days of
> distribution start to make a lot of sense."*

E lo ripete su tre casi storici, sempre con lo stesso movimento: individua **quando** il
posizionamento e' cambiato, torna sul grafico a **quella data**, e legge **a che prezzo** e'
avvenuto.

> *"They start the sell exposure really strong from the level 12 of November. From that moment
> they never recovered."*

La conseguenza e' che il COT non scade con la settimana. **La rilevazione data un evento; il
prezzo di quella data diventa un livello**, e il livello resta finche' il posizionamento non
cambia di nuovo. Un dato di martedi' pubblicato venerdi' e' inutile per decidere cosa fare
lunedi' mattina, ma dice a che prezzo qualcuno si e' caricato — e quello serve per settimane.

E' una lettura diversa dalla metrica cross-index, ed e' la ragione per cui il risultato negativo
di quella non chiude il discorso sul COT: la cross-index testava il COT come predittore
direzionale a quattro settimane, non come marcatore di livelli.

### Applicato Al Presente

La rilevazione TFF dell'8 settembre 2026 dice che i Leveraged Funds hanno chiuso 7.687 long e
aperto 10.093 short, per un netto di -17.780 — variazione piu' grande dell'89% delle 521 settimane
del decennio. La settimana coperta va da mercoledi' 2 a martedi' 8 settembre.

A che prezzo. Dai profili cash di quelle sedute, riportati in termini NQZ6:

| Data | POC | VAL | VAH | Delta cash |
|---|---|---|---|---|
| 09-02 | 29.451 | 29.418 | 29.496 | +2.759 |
| 09-03 | 29.819 | 29.618 | 29.875 | +2.851 |
| 09-04 | 29.811 | 29.772 | 29.881 | **-15.967** |
| 09-08 | 29.901 | 29.804 | 29.922 | -8.477 |

Il posizionamento short e' stato costruito mentre il prezzo saliva verso **29.800-29.900**. E
quella e' esattamente la fascia del **nodo superiore del composito, 29.675-29.924, con delta
negativo in tutto il cuore** (vedi [`profile-framing.md`](profile-framing.md)).

Due misure indipendenti — il posizionamento dichiarato alla CFTC e il delta aggressivo sul tape —
indicano la stessa fascia di prezzo come il posto dove i venditori si sono caricati. Il prezzo l'ha
poi lasciata e non e' tornato.

Questo e' il modo corretto di usare il COT fra un report e l'altro: **non "c'e' bias short quindi
vendo", ma "29.675-29.924 e' una zona dove qualcuno ha costruito, e un ritorno li' e' un ritorno
sul loro prezzo"**. Il livello sopravvive alla settimana; il bias no.

Una sola occorrenza. Va registrata come coerenza fra due fonti, non come conferma: nessuna delle
due dimostra l'altra, e nessun dato disponibile lega un contratto del tape a una categoria CFTC.

## Limiti Della Fonte

- Rilevazione di martedi', pubblicazione il venerdi' successivo: **tre giorni di ritardo
  strutturale**, prima ancora di qualunque considerazione sull'utilita'.
- Le OHLC settimanali di Tradingster non coincidono con NQU6 su ATAS: servono solo per ordine di
  grandezza.
- `Spreads` della riga Non-Commercial non e' esposizione direzionale e non entra in nessun calcolo.
- Il conteggio dei contratti non e' esposizione economica comparabile fra indici: S&P $50,
  Nasdaq mini $20, Dow $5.
- Per un uso formale la fonte da citare resta il rilascio CFTC, non la visualizzazione Tradingster.

---

# Livello 2: Il Data Bridge

## Cosa Rende Possibile

Il bridge espone via HTTP locale i dati che **esistono solo dentro il processo ATAS**: profilo
fisso con sessione dichiarata, trade aggregati storici con filtro di volume nativo, snapshot
storici del book, date di rollover calcolate sul volume. Contratto completo e vincoli noti in
[`contratto-data-bridge.md`](contratto-data-bridge.md).

Per l'analisi istituzionale contano cinque osservabili.

### 1. Il Profilo Di Sessione

Volume per prezzo su una finestra dichiarata, con POC e area di valore. Da qui si ricava **dove il
mercato ha accettato di scambiare**, che e' la domanda di location.

Convenzione del repository: cash **13:30-20:00 UTC**, notte 20:00-13:30 UTC. La finestra va
dichiarata perche' cambia il profilo.

### 2. Il Delta Di Sessione Contro Il Movimento Di Prezzo

Il delta e' volume aggressivo netto: quanto si e' pagato lo spread da un lato. Confrontato con il
movimento netto di prezzo, misura **sforzo e risultato** invece di intuirli.

### 3. `maxDelta` E `minDelta`

Delta massimo e minimo raggiunti **durante** la barra. Sono la misura diretta dello sforzo che non
ottiene risultato, e non sono ricavabili dal solo delta di chiusura. Non esistono fuori da ATAS.

### 4. Il Tape Completo

`GetCumulativeTradesSessionLimit(Filter)` vale `0`, cioe' nessun limite: con `minVolume 0`
l'endpoint `/cumulative` restituisce **ogni** trade aggregato, con timestamp al millisecondo e
direzione gia' classificata da ATAS. Due ore di NQ sono circa 114.000 record.

Verificato ricostruendo delta e volume di barra dal tape: coincidono con quelli di ATAS a meno
dell'assegnazione dei trade al millisecondo di confine.

Con lo stesso filtro nativo si isola la **partecipazione di taglia**: trade aggregati sopra una
soglia di volume. Va detto con chiarezza cosa sono e cosa non sono — nella settimana del 4-11
settembre, 80 trade da 100 lotti o piu' per circa 13.000 contratti dentro un volume settimanale di
2.162.401. Descrivono chi si muove in blocchi visibili, **non "le istituzioni"**, e non spiegano da
soli il movimento.

### 5. Il Profilo Composito Su Piu' Sessioni

Aggregando il footprint di piu' finestre in blocchi di prezzo si vedono **nodi** (dove il volume si
accumula) e **colli** (dove sparisce). E' la struttura su cui il corso ragiona, qui misurata.

## Il Controllo Che Viene Prima Di Ogni Altra Cosa: Il Rollover

**Un contratto vicino alla scadenza smette di essere il mercato prima di smettere di esistere.**
Se la liquidita' e' migrata, i profili e i delta descrivono un contratto in liquidazione, non il
Nasdaq. Questo controllo va fatto **prima** di qualunque lettura, non dopo.

Due modi, e servono entrambi:

**Il modo dichiarativo**, `/rollovers`, richiede un chart a contratto continuo: su un contratto
singolo ATAS risponde `Only continuous contracts are supported`. Attenzione all'insidia:
`VolumeBasedCurrentEnd` che **coincide con la data di scadenza** significa che il roll basato sul
volume *non e' ancora stato rilevato*, non che avvenga alla scadenza.

**Il modo misurato**, e quello che decide: caricare entrambi i contratti e confrontare il volume.

```bash
python3 FabioOrderFlow/tools/bridge.py candles --chart NQU6 --from 2026-09-08 --to 2026-09-16 --out u6.json
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 --from 2026-09-08 --to 2026-09-16 --out z6.json
```

Misura del roll U6 -> Z6, settembre 2026:

| Data | NQU6 24h | NQZ6 24h | quota Z6 | NQU6 cash | NQZ6 cash |
|---|---|---|---|---|---|
| 2026-09-08 | 504.846 | 4.241 | 0,8% | 353.985 | 2.782 |
| 2026-09-09 | 486.361 | 4.452 | 0,9% | 342.343 | 3.084 |
| 2026-09-10 | 544.240 | 9.135 | 1,7% | 385.727 | 6.008 |
| 2026-09-11 | 494.395 | 33.116 | 6,3% | 348.466 | 25.262 |
| 2026-09-13 | 17.747 | 8.600 | 32,6% | - | - |
| **2026-09-14** | 296.404 | **338.855** | **53,3%** | 204.225 | **249.978** |
| 2026-09-15 | 10.899 | 32.290 | 74,8% | - | - |

Il roll attraversa il 50% **lunedi' 14 settembre**, in una sola seduta: venerdi' 11 il dicembre
pesava il 6,3%, lunedi' il 53,3%. Da lunedi' 14 ogni misura di flusso va letta su **NQZ6**.

Questo conferma e data l'avvertenza aperta in
[`settimana-2026-09-14-09-18.md`](../sessioni/settimana-2026-09-14-09-18.md), che era stata scritta
su NQU6 e va quindi proseguita su NQZ6.

---

# La Griglia Di Lettura: Sforzo E Risultato

E' il ponte fra i due livelli, ed e' l'unica lettura che questo repository ha misurato su piu'
occorrenze. La domanda: **chi e' aggressivo, e ottiene quello che sta pagando?**

Tre forme, tutte osservate sul NQ fra il 9 e il 14 settembre 2026:

| Seduta | Delta cash | Movimento netto | Forma |
|---|---|---|---|
| 2026-09-09 | **+3.089** | chiusura identica all'apertura, POC da 29.610 a 29.443 | compratori aggressivi **senza** risultato |
| 2026-09-11 | **-6.196** | -36 punti, valore ricostruito 255 punti sopra il POC del giorno prima e difeso | venditori aggressivi **senza** risultato |
| 2026-09-14 | **+5.895** | +253 punti, sei ore su sette di delta positivo | aggressivita' **con** risultato |

Il terzo caso e' quello che rende leggibili i primi due: senza un riferimento allineato,
"sforzo senza risultato" non ha termine di paragone misurato.

## Il Livello Che Non Cede

Una figura ricorrente nelle tre sedute: **un minimo fatto nella prima ora e mai piu' riavvicinato**,
con delta netto positivo per tutta la sessione. Il 10 settembre il minimo 29.058 e' della prima ora
e quasi 390.000 contratti scambiano sopra di esso; il 14 settembre il minimo 28.879 e' della prima
ora e non torna piu'. Un minimo toccato una volta sola e poi difeso per sette ore non e' un minimo
in cedimento: e' un minimo assorbito.

Il seguito e' la parte piu' istruttiva: nella notte fra il 10 e l'11, il prezzo torna a
**29.040**, diciotto punti **sotto** il minimo della cash precedente, non ci resta, e nella stessa
ora il delta gira a +409, poi +950. Retest, penetrazione di pochi punti, nessuna accettazione,
delta che gira: **quello e' il momento dell'inversione**, e non richiedeva il COT.

Dettaglio completo in [`seduta-2026-09-11.md`](../sessioni/seduta-2026-09-11.md).

## Nodi E Colli

Sul composito del 10-11 settembre, 914.586 contratti in blocchi da 25 punti:

```text
nodo inferiore  29.100-29.250   costruito giovedi',  delta positivo nel cuore  (+1.321, +863)
collo sottile   29.275-29.349   36.238 contratti su 75 punti, contro ~90.000 per blocco nei nodi
nodo superiore  29.375-29.474   costruito venerdi',  delta negativo nel cuore  (-2.611, -1.062)
```

I due nodi hanno delta **di segno opposto rispetto al loro esito**: e' la stessa asimmetria letta
due volte. Il collo e' la zona attraversata in un solo blocco da 35.023 contratti alle 12:00 UTC.

La settimana successiva mostra la variante vuota: il gap del weekend lascia 2.534 contratti su 50
punti fra 29.300 e 29.349, e **il massimo di lunedi', 29.303, si ferma sul bordo inferiore del
vuoto**. Una sola occorrenza, con il volume gia' in migrazione: registrata, non usata.

---

# La Procedura Settimanale

```text
0.  ROLLOVER      confrontare il volume dei due contratti vicini.
                  Se la quota del contratto lontano supera il 50%, cambiare chart prima di
                  qualunque altra misura. Nessuna lettura di flusso e' valida su un contratto
                  in liquidazione.

1.  COT           leggere la pubblicazione del venerdi', riferita al martedi'.
                  Dichiarare la vista (TFF o Legacy) e la variante (futures only).
                  Separare flusso e livello: delta dell'ultima pubblicazione contro z52 e
                  percentile a tre anni. Registrare anche "nessun contesto".

2.  PROFILI       costruire i profili cash e notte delle sedute della settimana:
                  volume, delta, POC, VAL, VAH, high, low, open, close.

3.  SFORZO        per ogni seduta, delta di sessione contro movimento netto di prezzo.
                  Classificare nelle tre forme. maxDelta/minDelta dove serve il dettaglio
                  intrabarra.

4.  COMPOSITO     aggregare le finestre in blocchi di prezzo, individuare nodi e colli,
                  annotare il segno del delta dentro ciascun nodo.

5.  TAGLIA        trade aggregati sopra soglia, con il peso dichiarato rispetto al volume
                  totale. Non chiamarli "istituzioni".

6.  POSIZIONE     dove sta il prezzo adesso rispetto a: valore della notte in formazione,
                  valore dell'ultima cash, POC, e bordi dei nodi del composito.
```

Comandi corrispondenti:

```bash
python3 FabioOrderFlow/tools/bridge.py charts
python3 FabioOrderFlow/tools/bridge.py candles --chart NQZ6 --from 2026-09-14 --to 2026-09-19 --levels --out c.json
python3 FabioOrderFlow/tools/bridge.py cumulative --chart NQZ6 --from 2026-09-15T13:30:00Z --to 2026-09-15T15:30:00Z \
        --min-volume 0 --window-minutes 10 --compact --out t.json
python3 FabioOrderFlow/tools/bridge.py cumulative --chart NQZ6 --from 2026-09-14 --to 2026-09-19 --min-volume 100 --out big.json
python3 FabioOrderFlow/tools/bridge.py profile --chart NQZ6 --period LastDay --out profile.json
python3 FabioOrderFlow/tools/bridge.py rollovers --chart NQ --from 2026-06-01 --to 2026-12-31
```

---

# Cosa Questa Analisi Non Fa

- **Non attribuisce identita'.** Il tape dice che qualcuno e' stato aggressivo per un certo volume
  a un certo prezzo. Non dice chi, e nessun dato disponibile lo direbbe. Le categorie COT sono
  autodichiarazioni alla CFTC su un altro orizzonte, non i partecipanti del tape.
- **Non produce un segnale direzionale.** Nessuna delle letture qui descritte e' stata validata
  come predittiva, e quella che e' stata testata formalmente — la metrica cross-index — non ha
  separato la risposta di prezzo.
- **Non fonde i due livelli in un punteggio.** Sono unita' di misura diverse su orologi diversi.
- **Non sopravvive a una sola occorrenza.** Ogni co-occorrenza osservata in una seduta e'
  registrata come fatto, non come regolarita'.

# Risultati Registrati

Positivi e negativi hanno lo stesso titolo a stare qui.

| Esito | Cosa |
|---|---|
| verificato | Il tape restituito con `minVolume 0` e' completo: delta e volume di barra ricostruiti coincidono con ATAS |
| verificato | Il roll U6 -> Z6 attraversa il 50% il 14 settembre 2026, misurato sul volume dei due contratti |
| verificato | `VolumeBasedCurrentEnd` uguale alla scadenza significa roll non ancora rilevato, non roll alla scadenza |
| verificato | Tradingster carica dieci anni di serie nel `dataProvider`: la raccolta e' riproducibile senza lettura a occhio |
| **negativo** | La metrica cross-index nella forma letterale **non separa** la risposta di prezzo a quattro settimane |
| **negativo** | Nella forma selettiva separa, ma su 12 e 3 occorrenze: non e' una verifica |
| distinzione | Flusso e livello sono cose diverse: -17.780 di variazione, 89° percentile, su un livello al 59° percentile |
| correzione | `Non-Commercial` (Legacy) e `Asset Manager` (TFF) non sono sinonimi, contrariamente a quanto dice il live |

# Riferimenti

- [`contratto-data-bridge.md`](contratto-data-bridge.md) — endpoint, vincoli, sicurezza
- [`../cot/snapshot-2026-09-08.md`](../cot/snapshot-2026-09-08.md) — rilevazione TFF piu' recente
- [`../cot/tradingster-cross-index-2026-09-11.md`](../cot/tradingster-cross-index-2026-09-11.md) — verifica della metrica
- [`../sessioni/seduta-2026-09-11.md`](../sessioni/seduta-2026-09-11.md) — dove e' avvenuta l'inversione
- [`../sessioni/settimana-2026-09-04-09-11.md`](../sessioni/settimana-2026-09-04-09-11.md) — sforzo senza risultato
- [`../sessioni/settimana-2026-09-14-09-18.md`](../sessioni/settimana-2026-09-14-09-18.md) — gap del weekend e rollover
- [`contratto-osservativo-partecipazione.md`](contratto-osservativo-partecipazione.md) — cosa e' un evento aggressivo
