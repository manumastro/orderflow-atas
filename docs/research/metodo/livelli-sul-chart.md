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

Derivati dal composito cash di undici sedute, in termini NQZ6 (vedi
[`profile-framing.md`](profile-framing.md)):

| Prezzo | Etichetta | Perche' |
|---|---|---|
| 29.924 | nodo alto / short LF | bordo alto del nodo a delta negativo, dove i Leveraged Funds hanno costruito short nella settimana al 8 settembre |
| 29.804 | VAL 08 set | bordo basso della zona di costruzione |
| 29.749 | VAH 11 set | |
| 29.677 | VAL 11 set / collo | bordo alto del collo fra i due nodi |
| 29.604 | max lun / vuoto gap | massimo di lunedi', bordo del vuoto lasciato dal gap del weekend |
| 29.516 | VAH 10 set | |
| **29.454** | **MENSOLA POC 02/10/14** | tre sedute diverse hanno messo qui il POC |
| 29.400 | VAL 10 / bordo nodo | bordo basso del nodo inferiore a delta positivo |
| 29.306 | VAL lun (retest ok) | testato nella notte a 29.308 e tenuto |
| 29.168 | min lunedi' | |
