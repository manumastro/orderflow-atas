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

## L'Etichetta Dice A Cosa Serve Arrivarci

Un livello non e' il fine, e' una porta: l'etichetta deve dire **cosa cambia** quando il prezzo ci
arriva, non come si chiama il livello.

    "IVB ALTO = mensola/POC 29454"                        il nome
    "TETTO IVB - sopra qui torna il permesso long"        cosa apre

Le quattro cose che un livello puo' essere — permesso, bersaglio, invalidazione, posizionamento —
sono elencate in `CLAUDE.md`, sezione *Un Livello Non E' Mai Il Fine*. La regola vale per
l'etichetta come per la frase parlata: se non si sa dire a cosa serve, il livello non va disegnato.
