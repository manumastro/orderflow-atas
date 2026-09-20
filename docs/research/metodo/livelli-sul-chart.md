# Livelli Sul Chart: Come Arrivano Da ATAS E Come Ci Tornano

Stato: **procedura**, e dal 20 settembre 2026 descrive solo meta' del giro.

> **Il deposito di prezzi gia' risolti non e' piu' il modo normale di mettere livelli sul
> chart.** Adesso si depositano **regole** e a calcolarle e' l'indicatore, a ogni barra:
> [`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md). Questo
> documento resta valido per il trasporto, il formato di un livello, dove vivono i livelli e
> come si disegnano — e per il caso in cui si voglia depositare qualcosa a mano, sapendo che
> quel POST **spegne le regole**.

## Chi Fa Cosa

Tre pezzi, e la separazione fra loro e' deliberata.

```text
ATAS  ──GET /candles, /cumulative──▶  bridge.py  ──▶  JSON grezzo
                                                          │
                                        analisi in Python, fuori da ATAS
                                        profili cash, value area, POC,
                                        composito, nodi e colli, spread del roll
                                                          │
                                                          ▼
                                                   livelli.json
                                                          │
ATAS  ◀──POST /levels──  bridge.py levels --file  ────────┘
  │
  └─▶  OnRender disegna linea ed etichetta
```

- **L'indicatore calcola i prezzi dei livelli, e nient'altro.** Fino al 20 settembre non
  calcolava niente, e la ragione era buona: le soglie non devono diventare codice sul chart
  invece che convenzioni dichiarate in un documento. La ragione regge ancora, ed e' il motivo
  per cui dentro l'indicatore e' finito **il motore** e non **le regole**: quante sedute, quale
  finestra, che cosa conta restano dichiarati in un file, fuori dal codice. Quello che e'
  passato dentro e' solo il conteggio — e ci e' passato perche' un conteggio affidato a un
  processo esterno muore, e quando muore mente.
- **`bridge.py` non calcola niente.** Il sottocomando `levels` e' solo trasporto HTTP.
- **I livelli si derivano nell'analisi**, con il metodo di
  [`profile-framing.md`](profile-framing.md), e si scrivono in un file.

Conseguenza da tenere presente: **i livelli depositati come prezzi non si aggiornano da soli.**
Restano quelli finche' qualcuno non rifa' il POST, ed e' esattamente il problema che le regole
tolgono di mezzo. `livelli_vivi.py`, che faceva questo lavoro da fuori, e' ritirato.

## Dove Vivono

In memoria nell'istanza dell'indicatore, e in copia su `~/.fabio-data-bridge-levels.json`.

Il file esiste per una ragione pratica: un riavvio di ATAS o un redeploy della DLL ricarica
l'indicatore, e senza copia su disco i livelli sparirebbero dal chart ogni volta. Chi guarda il
grafico vedrebbe semplicemente il lavoro svanire.

La chiave del file e' lo **strumento**, non l'id dell'istanza: l'id e' casuale e cambia a ogni
caricamento, lo strumento no. Un solo file tiene gli strumenti di tutti i chart, e una scrittura
tocca solo la propria voce.

```json
{"NQZ6": [{"price": 29454, "label": "MENSOLA POC 02/10/14", ...}], "NQU6": [...]}
```

Un file corrotto non impedisce il caricamento dell'indicatore, e un errore di scrittura non fa
fallire la richiesta: i livelli sono gia' applicati in memoria e disegnati. Si puo' anche
scriverlo a mano, e i livelli compaiono al caricamento successivo.

## Il Formato

```json
[
  {"price": 29454, "label": "MENSOLA POC 02/10/14", "color": "#4FC3F7", "style": "solid", "width": 3,
   "note": "tre sedute diverse hanno messo qui il POC"},
  {"price": 29306, "label": "VAL lun (retest ok)", "color": "#81C784", "style": "solid", "width": 2}
]
```

Solo `price` e' obbligatorio.

| Campo | Valori | Note |
|---|---|---|
| `price` | numero positivo | un prezzo a zero viene rifiutato: e' quasi sempre un campo mancante, non un livello |
| `label` | testo | disegnato accanto alla linea, insieme al prezzo |
| `color` | `#RRGGBB`, `#AARRGGBB`, o nome noto | un valore illeggibile ricade sul default invece di far fallire la richiesta: perdere un livello per un colore sbagliato sarebbe peggio |
| `style` | `solid`, `dash`, `dot`, `dashdot` | |
| `width` | 1-5 | |
| `note` | testo | **non** viene disegnato, torna su `GET`: serve a ricordare perche' il livello c'e' |

## I Comandi

```bash
# depositare, da file
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 --file livelli.json

# depositare al volo, prezzo[:etichetta[:colore[:stile]]]
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 \
        --set 29454:mensola:#4FC3F7:solid --set 29306:VAL-lunedi

# leggere quelli presenti, note comprese
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6

# cancellare
python3 FabioOrderFlow/tools/bridge.py levels --chart NQZ6 --clear
```

**Il POST sostituisce l'intera lista, non aggiunge.** Cosi' il chart mostra sempre l'analisi
corrente e non si accumulano livelli dimenticati di tre giorni fa. Ogni chart ha la sua lista:
senza `--chart`, con piu' chart registrati, la richiesta viene rifiutata con l'elenco dei
candidati invece di finire su un chart a caso.

## Le Proprieta' Di Disegno

Sull'istanza dell'indicatore, gruppo **Levels**:

| Proprieta' | Default | A cosa serve |
|---|---|---|
| `Show levels` | vero | spegne il disegno senza cancellare i livelli |
| `Label on the right` | vero | etichetta a destra; falso la porta a sinistra |
| `Font size` | 11 | |
| `Label margin` | 8 | distanza dal bordo, in pixel |

### Perche' Esiste `Label margin`

`ChartArea` arriva fino al bordo del pannello, **scala dei prezzi compresa**: ancorare l'etichetta
a `ChartArea.Right` la fa finire sotto i numeri dell'asse. Il disegno usa quindi come bordo destro
la X dell'**ultima barra visibile**, che sta dentro l'area dei dati per costruzione, qualunque sia
la larghezza dell'asse.

`Label margin` resta come regolazione fine, perche' la larghezza dell'asse cambia con il numero di
cifre del prezzo e con il DPI dello schermo. Se l'etichetta e' ancora troppo vicina al bordo,
alzalo; se la vuoi piu' fuori, abbassalo o porta l'etichetta a sinistra.

L'etichetta ha un fondo pieno semitrasparente: sopra un footprint denso il testo nudo e'
illeggibile.

## Sicurezza

`/levels` e' la sola superficie di scrittura del bridge. La lista vive in memoria
nell'indicatore, non entra in nessun calcolo, non produce segnali e non tocca lo stato della
piattaforma, gli ordini o le posizioni. Il listener resta legato a `127.0.0.1`.

## Esempio: I Livelli Del 15 Settembre 2026

Derivati dal composito cash di undici sedute e dal profilo della singola seduta di lunedi', in
termini NQZ6 (vedi [`profile-framing.md`](profile-framing.md)).

Le etichette portano **la misura, non solo il nome**: un livello che dice *"rotta 06:21 (-165d)"*
si legge sul chart senza tornare al documento, e soprattutto dice quanto vale la lettura. Il campo
`note`, che non viene disegnato e torna solo sul `GET`, tiene il ragionamento per esteso.

| Prezzo | Etichetta | Cosa porta |
|---|---|---|
| 29.924 | Top nodo · short LF 2-8 set | bordo alto del nodo a delta negativo; li' i Leveraged Funds hanno costruito gli short |
| 29.804 | Base zona short LF · VAL 8 set | bordo basso della zona di costruzione |
| 29.749 | VAH 11 set | contesto pre-roll |
| 29.677 | Collo · VAL 11 set | bordo alto del collo fra i due nodi |
| 29.604 | Max lun · bordo vuoto gap | massimo di lunedi', bordo del vuoto del gap di weekend |
| 29.516 | Top nodo compratori · VAH 10 set | bordo alto del nodo a delta positivo |
| 29.474 | Top nodo notte · 31% vol oggi | 14.816 lotti, il blocco piu' pesante della seduta, poi abbandonato |
| **29.454** | **MENSOLA · POC 2-10-14 set** | tre sedute distinte hanno messo qui il POC |
| 29.400 | Rotta 06:21 (-165d) · resistenza | 174 minuti su 198 sotto, 10% del volume sopra: accettazione |
| 29.375 | Base vuoto 29375-29424 (1,2% lun) | la fascia sottile e' **sopra**, non sotto |
| 29.306 | VAL lun · test 07:19 senza compratori | 1.561 lotti, delta -31: nessuno ha comprato |
| 29.275 | Scaffale 29275-29324 · 18,7% lun | 62.179 lotti, delta +1.073: zona difesa, non vuota |
| 29.199 | Blocco 29175-29224 · 14,1% lun | 46.989 lotti, delta +1.325 |
| 29.168 | Min cash lun 15:37 | il minimo che conta per il framing, che usa solo la cash |
| 29.107 | Min globex lun 11:25 | fuori dalla cash: contesto |

Quindici livelli sono vicini al massimo leggibile su un chart NQ. Oltre, le etichette si
sovrappongono e il valore informativo scende: meglio togliere il contesto lontano che stringere il
carattere.

### Cosa Rende Un'Etichetta Utile

1. **Il nome del livello**, per riconoscerlo.
2. **La misura che lo sostiene**, per sapere quanto pesa: `31% vol oggi`, `-165d`, `18,7% lun`.
3. **Lo stato**, quando e' cambiato: `rotta 06:21`, `test senza compratori`.

Quello che non ci va e' la previsione. Un'etichetta dice cosa e' successo li', non cosa succedera'.

## I Livelli Fissi E I Livelli Vivi

**Alcuni livelli descrivono un fatto chiuso, altri descrivono una misura che si muove da sola.**
Tenerli nello stesso elenco statico significa che i secondi invecchiano mentre nessuno se ne
accorge, perche' sul chart hanno lo stesso aspetto dei primi.

| | esempi | quando cambia |
|---|---|---|
| **fisso** | il minimo della notte, il bordo di un nodo, una mensola difesa quattro volte, il prezzo del COT | solo se il prezzo ci ripassa e ne cambia la **funzione**: allora si riscrive l'etichetta, non il numero |
| **vivo** | POC, VAH, VAL, massimo e minimo della finestra in sviluppo, il bordo della fascia piu' pesante | **a ogni barra** |

> **La coppia fisso/vivo e' superata.** Gli assi sono due — *misurato* contro *dichiarato*, e
> *finestra aperta* contro *finestra chiusa* — e i marcatori sul chart sono tre: `~` `=` `*`.
> Vedi [`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).

Un livello non si ricalcola a mano a ogni lettura: si **dichiara la regola** in
`docs/research/giornate/regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json`, si deposita una volta con
`bridge.py rules`, e da li' in poi la risolve l'indicatore a ogni barra.

```json
{"nome": "POC cash", "tipo": "poc", "finestra": {"da": "13:30Z"},
 "label": "POC cash {pct} · bersaglio del mean reverting dentro la seduta",
 "color": "#4FC3F7", "style": "solid", "width": 3}
```

`{prezzo}`, `{pct}`, `{lotti}` e `{delta}` vengono sostituiti con la misura corrente: l'etichetta
porta il peso del livello senza che si debba tornare al documento, come chiede la sezione *Cosa
Rende Un'Etichetta Utile*.

La separazione e' la stessa di `scenari.py`: **il programma e' il motore, le condizioni stanno in
un file che si riscrive ogni volta.** Quali finestre, quale granularita', quali nodi contano sono
convenzioni dell'analisi, e devono restare leggibili in un file, non finire dentro il codice.

```bash
# ricalcola e rideposita, cancellando prima tutto
python3 FabioOrderFlow/tools/livelli_vivi.py \
        docs/research/giornate/livelli-vivi-NQZ6-2026-09-14.json --chart NQZ6

# solo calcolo, per controllare prima di scrivere sul grafico
python3 FabioOrderFlow/tools/livelli_vivi.py ... --chart NQZ6 --prova
```

### Si Cancella Sempre Prima Di Scrivere

**Il deposito comincia da un `DELETE`, verifica che il chart sia a zero, e solo allora scrive.**
Non e' ridondante rispetto al fatto che il POST sostituisca la lista: e' la prova che si stia
parlando col chart giusto. Se la cancellazione finisce altrove il conteggio non torna a zero e il
programma si ferma li', invece di lasciare due elenchi su due chart e far scoprire l'errore
guardando il grafico.

Il 19 settembre, aprendo un replay del 14, sul chart c'erano ancora **otto livelli del 18
settembre**, fra 29.648 e 29.926: quattrocento punti sopra un mercato che girava a 29.130. Non
erano sbagliati, erano di un altro giorno — ed e' esattamente il caso in cui un residuo non si
riconosce come tale, perche' un livello vecchio e un livello nuovo si disegnano uguali.

### Due Griglie, E Vanno Dichiarate

Il POC e i bordi del valore **non si cercano sul tick**. Il volume di una notte si distribuisce su
piu' di mille prezzi da un quarto di punto, e il singolo tick piu' scambiato puo' cadere fuori dal
cuore del volume.

Il 14 settembre, sulla notte intera: POC sul tick **29.150**, POC su griglia da un punto
**29.318**. La fascia da 25 punti piu' pesante era 29.300-29.324 con il 17,9% del volume, contro
il 13,7% di 29.150-29.174. **Il POC sul tick indicava la seconda fascia**, e sul chart sarebbe
stato un POC che non era il POC.

Quindi due griglie, entrambe dichiarate accanto al numero:

- **`grana`**, default **1 punto**: su questa si cercano POC, VAH e VAL.
- **`passo`**, default **25 punti**: su questa si misurano i nodi e le percentuali delle etichette.

### Livelli Che Coincidono

Due livelli a meno di **otto punti** non si disegnano entrambi: le etichette si sovrappongono e il
chart perde due informazioni invece di guadagnarne una. Cade il secondo in ordine di dichiarazione
— l'ordine nel file e' la priorita' scelta dall'analisi — e il programma stampa quale e' caduto.

**Che due misure coincidano e' un fatto, e va nella lettura, non sul grafico.** Il POC della notte
che scende sul POC europeo dice che il cuore del volume si e' spostato: e' una frase, non due righe
sovrapposte.

**L'ordine nel file e' quindi una priorita', non un elenco.** Il 14 settembre, ordinato per prezzo,
il diradamento buttava via il **POC della cash** — cioe' il livello su cui si appoggia il mean
reverting — per tenere un bordo di nodo che gli stava a cinque punti. Si dichiarano per primi i
livelli su cui si costruisce la lettura, e per ultimi quelli di contesto.

### Una Finestra Appena Aperta Non Ha Un Profilo

`minimo_lotti` tiene un livello vivo fuori dal chart finche' la sua finestra non ha scambiato
abbastanza. Nei primi minuti della cash **tutto il volume sta in una fascia sola**: il POC dice
*"89% del volume"* perche' non c'e' nient'altro, e il VAH col VAL gli stanno addosso. Non e' una
misura prematura, e' una misura falsa — un bordo del valore che non e' bordo di niente, esattamente
l'errore del 16 settembre ma dall'altro capo della giornata.

Sulla cash NQ la soglia usata e' **15.000 lotti**, circa i primi otto minuti. Non e' una costante
del metodo: e' la scelta di quel giorno, e come tutte le soglie va dichiarata accanto al numero che
produce.

## I Livelli Si Rifanno Durante La Seduta, Non Solo All'Apertura

**Un livello derivato alle 07:45 descrive il mercato delle 07:45.** Se alle 15:00 sta ancora sul
chart senza essere stato rifatto, non e' un riferimento: e' un residuo, e chi lo guarda crede di
vedere una misura mentre vede una memoria.

Il 16 settembre e' successo in due modi diversi nella stessa giornata.

**Il primo, trovato dall'utente.** VAH 29.335 e VAL 29.250 erano i bordi del valore **notturno**,
calcolati alle 07:45. Li ho citati come "i bordi del valore" per nove ore e centomila lotti. Il
valore in sviluppo, ricalcolato alle 15:00, era VAL 29.235 / POC 29.275 / **VAH 29.372,50** — e
quel VAH coincideva con il livello su cui il prezzo stava combattendo da un'ora. Non era "sessanta
punti sopra un bordo vecchio": **era sul bordo**, e la lettura che ne discendeva era sbagliata.

**Il secondo, strutturale.** Alle 11:30 nove livelli su tredici stavano sotto 29.335, mentre tutto
il pomeriggio si e' svolto fra 29.364 e 29.439, dove ce n'erano due. E due etichette dicevano il
falso: `29.358,50 primo ostacolo sopra` era sotto il prezzo da quattro ore.

### Quando si rifanno

| | |
|---|---|
| **il valore** (VAH, VAL, POC) | **a ogni lettura che lo usa.** E' un calcolo di due secondi sul profilo in sviluppo, e il mean reverting ci si appoggia sopra: un bordo vecchio arma un fade su un prezzo che non e' piu' un bordo |
| **l'insieme dei livelli** | quando il prezzo esce dalla fascia per cui erano stati derivati, e **sempre** dopo la stampa dell'IVB, che ridefinisce la giornata |
| **le etichette** | quando cambia la **funzione** del livello: un tetto che diventa pavimento, un ostacolo che diventa supporto. Il prezzo resta, la frase no |

### Due cose che non si fanno

- **Non si ereditano i bordi da una sessione all'altra senza dirlo.** Il valore della notte e
  quello in sviluppo sono due misure diverse: se si usa uno al posto dell'altro va dichiarato
  quale, perche' il dossier (righe 139-196) dice *"fade the edges of the value area"* e **non
  specifica quale sessione** — la scelta e' del metodo, non della fonte.
- **Non si aggiorna il chart senza aggiornare gli scenari.** Il 16 settembre i livelli sono stati
  rifatti sui bordi nuovi mentre i due `fade` restavano armati su 29.335 e 29.250: il grafico
  diceva una cosa e il motore ne sorvegliava un'altra. Riscrivere uno scenario costa le sette prove,
  e finche' non sono fatte **il fade non e' armato** — e va detto, non lasciato intendere.

### Il Ridisegno Non Si Chiede, Si Fa

**Fra l'accorgersi e il ridisegnare non ci va una domanda.** Quando una lettura scopre che i bordi,
il POC o l'insieme dei livelli non corrispondono piu' ai dati, la sequenza e' questa e va eseguita
**prima** di scrivere la lettura:

1. riscrivere `livelli-AAAA-MM-GG.json`
2. spingerli con `bridge.py levels --file ... --chart ...`
3. riscrivere gli scenari che usavano quei prezzi, e passare `--controlla`
4. fermare e riavviare i due sorveglianti, che leggono i file **all'avvio**
5. solo ora la lettura, dichiarando cosa e' cambiato e perche'

Il motivo e' che la domanda non e' gratis: per tutto il tempo in cui resta senza risposta, sul
chart ci sono numeri che l'analisi ha **gia' dichiarato sbagliati**, e sono esattamente i minuti in
cui l'utente guarda il grafico. Il 16 settembre alle 16:52 il fade era armato su 29.455, che era
venticinque punti **dentro** il valore cash: un fade su un prezzo che non era bordo di niente.

**L'unica eccezione:** se il ridisegno cambia una **posizione aperta** — uno stop che si sposta, un
bersaglio che si accorcia — quello si chiede. Cambiare un livello no.

### La Finestra Di Misura Si Sceglie A Ogni Lettura

**Prima di calcolare VAH, VAL o POC si decide su quali barre**, e la scelta si dichiara accanto al
numero. Non e' un dettaglio di forma: finestre diverse misurano **popolazioni diverse**, e danno
bordi diversi sullo stesso mercato nello stesso istante.

Il 16 settembre alle 16:52 la differenza era questa:

| finestra | VAL | POC | VAH |
|---|---|---|---|
| da ieri sera (globex + cash) | 29.300 | 29.400 | **29.455** |
| **solo cash**, 92.081 lotti dalle 15:30 | 29.416,75 | 29.460 | **29.480,75** |

Venticinque punti sul bordo alto, sessanta sul POC — e il fade era armato su 29.455, cioe'
venticinque punti **dentro** il valore: un fade su un prezzo che non era bordo di niente.

**Come si sceglie.** La finestra e' quella che contiene i partecipanti che stanno facendo il prezzo
adesso, e **cambia piu' volte al giorno**:

| quando si legge | finestra |
|---|---|
| durante la cash di New York | la **cash** dall'apertura |
| in globex, prima dell'apertura | la **globex** da quando e' cominciata, piu' il framing della cash precedente come contesto |
| a cavallo di un evento che ha rotto la struttura (news, gap, spike) | **da li' in poi**, e lo si dichiara: prima e dopo sono due mercati |
| per il framing di ieri | la **cash di ieri**, che e' una misura chiusa e non si rifa' |

**La regola non e' "dopo l'apertura cash usa la cash".** Quella e' solo il caso piu' frequente. La
regola e' che la finestra si **riconsidera a ogni passaggio**, in qualunque fase della giornata: la
sera, in notturna, a mercato quasi fermo. Una finestra scelta una volta e riusata per inerzia
produce esattamente lo stesso errore di un bordo calcolato alle 07:45 e citato alle 15:00.

**E si scrive.** *"VAH 29.480,75"* non e' una misura finche' non dice **su cosa**: *"VAH 29.480,75,
cash dalle 15:30, 92.081 lotti"* lo e'. Vale nel diario, nelle etichette, nel campo `attesa` degli
scenari e nella lettura parlata.

## L'Etichetta Dice A Cosa Serve Arrivarci

Un livello non e' il fine, e' una porta: l'etichetta deve dire **cosa cambia** quando il prezzo ci
arriva, non come si chiama il livello.

    "IVB ALTO = mensola/POC 29454"                        il nome
    "TETTO IVB - sopra qui torna il permesso long"        cosa apre

Le quattro cose che un livello puo' essere — permesso, bersaglio, invalidazione, posizionamento —
sono elencate in `CLAUDE.md`, sezione *Un Livello Non E' Mai Il Fine*. La regola vale per
l'etichetta come per la frase parlata: se non si sa dire a cosa serve, il livello non va disegnato.
