# Livelli Sul Chart: Come Arrivano Da ATAS E Come Ci Tornano

Stato: **procedura**. Descrive il giro completo, dai dati grezzi alla linea disegnata.

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

- **L'indicatore non calcola niente.** Riceve una lista di prezzi con etichetta e la disegna. Se
  la derivazione finisse dentro l'indicatore, le soglie — quanti punti per blocco, quante sedute,
  cosa e' un collo — diventerebbero codice sul chart invece che convenzioni dichiarate in un
  documento, e il bridge smetterebbe di essere uno strumento di raccolta.
- **`bridge.py` non calcola niente.** Il sottocomando `levels` e' solo trasporto HTTP.
- **I livelli si derivano nell'analisi**, con il metodo di
  [`profile-framing.md`](profile-framing.md), e si scrivono in un file.

Conseguenza da tenere presente: **i livelli sono una fotografia, non sono vivi.** Restano quelli
finche' qualcuno non rifa' il POST. Vanno rifatti quando la seduta ne costruisce di nuovi.

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
  diceva una cosa e il motore ne sorvegliava un'altra. Riscrivere uno scenario costa le sei prove,
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

### Dopo L'Apertura Cash Il Valore Si Misura Sulla Cash

Il profilo che comprende la globex e quello della sola cash misurano **due popolazioni diverse**, e
dopo le 15:30 quella che fa il prezzo e' la seconda. Il 16 settembre alle 16:52 la differenza era
questa:

| | VAL | POC | VAH |
|---|---|---|---|
| finestra intera (da ieri sera) | 29.300 | 29.400 | **29.455** |
| **solo cash**, 92.081 lotti dalle 15:30 | 29.416,75 | 29.460 | **29.480,75** |

Venticinque punti sul bordo alto, sessanta sul POC. E il VAH cash coincideva col tetto dell'IVB
(29.481,50): lo stesso prezzo era insieme il gate Tier 01 e il bordo del fade — due significati
diversi che vanno tenuti distinti nelle etichette e negli scenari, non fusi in una linea sola.

## L'Etichetta Dice A Cosa Serve Arrivarci

Un livello non e' il fine, e' una porta: l'etichetta deve dire **cosa cambia** quando il prezzo ci
arriva, non come si chiama il livello.

    "IVB ALTO = mensola/POC 29454"                        il nome
    "TETTO IVB - sopra qui torna il permesso long"        cosa apre

Le quattro cose che un livello puo' essere — permesso, bersaglio, invalidazione, posizionamento —
sono elencate in `CLAUDE.md`, sezione *Un Livello Non E' Mai Il Fine*. La regola vale per
l'etichetta come per la frase parlata: se non si sa dire a cosa serve, il livello non va disegnato.
