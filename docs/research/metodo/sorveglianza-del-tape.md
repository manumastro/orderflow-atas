# Sorveglianza Del Tape: Dai Livelli Alla Lettura Sul Chart

Procedura standard di studio per una seduta. Chiude il cerchio fra le tre cose che finora stavano
separate: l'analisi che deriva i livelli, il chart che li disegna, e il tape che li mette alla
prova minuto per minuto.

La divisione dei ruoli e' il punto: **gli scenari li scrive l'analisi prima della seduta, il
motore li valuta, l'annotazione riporta la lettura sul chart.** Il giudizio non e' mai codice
fisso: e' un file che si riscrive ogni volta guardando il contesto di quel giorno.

Prerequisiti: [`profile-framing.md`](profile-framing.md) per derivare i livelli,
[`livelli-sul-chart.md`](livelli-sul-chart.md) per come arrivano sul chart,
[`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md) per dove
finisce il risultato.

## Il Giro Completo

```text
1. analisi          il profilo delle sedute passate produce i livelli del giorno
2. file             i livelli si scrivono in docs/research/giornate/livelli-AAAA-MM-GG.json
3. chart            bridge.py levels --file li spinge, l'indicatore li disegna
4. scenari          l'analisi scrive in scenari-AAAA-MM-GG.json cosa si aspetta, come condizioni
5. motore           scenari.py le valuta a ogni barra chiusa e, quando una scatta, annota da solo
6. sveglia          sveglia_tape.py resta acceso in parallelo per cio' che non avevamo previsto
7. lettura          l'analisi guarda con calma e, se conclude qualcosa di nuovo, annota a mano
8. giornata         a fine seduta il diario diventa la cronaca in AAAA-MM-GG.md
```

Il punto 2 e' la chiave: **il file dei livelli e' l'unica fonte**, letta sia da chi disegna sia da
chi sorveglia. Senza, le due cose divergono e il monitor guarda livelli diversi da quelli che vedi.

## 1-2. I Livelli, In Un File Per Giornata

Un array JSON in `docs/research/giornate/livelli-AAAA-MM-GG.json`, un oggetto per livello:

```json
{
  "price": 29306,
  "label": "VAL lun · test 07:19 senza compratori",
  "color": "#81C784",
  "style": "solid",
  "width": 2,
  "note": "1.561 lotti con delta -31: nessuno ha comprato."
}
```

L'etichetta porta **la misura, non solo il nome**; la `note` non viene disegnata e tiene il
ragionamento per esteso. Le regole complete stanno in
[`livelli-sul-chart.md`](livelli-sul-chart.md).

Il file resta nel repository: e' il documento di cosa si guardava quel giorno, e serve a rileggere
la giornata sapendo quali livelli erano attivi.

## 3. Spingerli Sul Chart

```bash
python3 FabioOrderFlow/tools/bridge.py levels --file docs/research/giornate/livelli-2026-09-15.json
```

`POST /levels` **sostituisce l'intera lista**. E' voluto: il file e' la fonte, il chart ne e' il
riflesso.

## 4. Scrivere Gli Scenari Della Seduta

**E' il passo che conta.** Le condizioni non stanno nel programma: stanno in
`docs/research/giornate/scenari-AAAA-MM-GG.json`, e **le scrive l'analisi ogni volta**, guardando il
contesto di quel giorno — il posizionamento istituzionale, il profilo delle sedute passate, la
struttura della mattina, cosa ci si aspetta che succeda.

Il programma e' solo il motore. Cosi' il segnale che compare a schermo **non e' generico**: e' gia'
lo scenario che avevamo previsto, col nome che gli avevamo dato e con l'attesa scritta accanto.

```json
{
  "nome": "B1 - cede il bordo basso del range",
  "quando": "chiude_sotto(29236) and vol >= p95vol and delta <= -p95delta",
  "prezzo": 29236,
  "tipo": "rottura",
  "testo": "B1: perso il bordo basso della sega 29236-29285",
  "attesa": "prossimo appoggio 29224, poi il blocco 29175-29224 dove lunedi c'erano 18.540 lotti."
}
```

- **`quando`** e' una espressione valutata su un contesto ricco. Nessun accesso a moduli o
  builtin: solo le variabili documentate.
- **`testo`** compare sul chart appena la condizione scatta.
- **`attesa`** e' cosa ci aspettiamo dopo. Finisce nella misura dell'annotazione, quindi nel
  diario, e a fine giornata si puo' verificare se e' successo.
- **`una_volta`**, vero di default: uno scenario scatta una volta sola.

### Il contesto disponibile

`./scenari.py --variabili` lo elenca. Le famiglie sono sei, e coprono proprio cio' che una barra
singola non sa:

| Famiglia | Esempi |
|---|---|
| La barra | `o h l c vol delta pos ora` |
| Soglie della seduta | `p95vol p75vol p95delta`, ricalcolate a ogni giro |
| Finestre mobili | `d30 v30 dpct30` — il delta in percentuale del volume su 15, 30, 60 minuti |
| Struttura | `max3 min10 massimi_calanti(n) minimi_crescenti(n)` |
| Livelli e loro memoria | `chiude_sotto(p) tocca(p,d) test_vol(p) test_prec_vol(p)` |
| Accettazione e orologio | `minuti_sotto(p,n) vol_sopra_pct(p,n) dopo("15:30") ivb_alto` |

`test_prec_vol(p)` merita una riga a parte: confronta il passaggio in corso su un livello con quello
precedente sullo stesso livello. E' la lettura piu' utile che si possa fare — *"2.289 lotti contro i
1.561 di prima"* — e finora si poteva fare solo a mano.

### Provarli prima di armarli

```bash
python3 FabioOrderFlow/tools/scenari.py --giorno 2026-09-15 --from 2026-09-15T06:00 --prova
```

`--prova` li valuta su tutta la storia senza scrivere niente. Serve a due cose: vedere che le
espressioni non contengano errori, e capire quanto sono selettive. Uno scenario che scatta dieci
volte in una mattina non e' uno scenario.

### Armarli

```bash
python3 FabioOrderFlow/tools/scenari.py --giorno 2026-09-15 --from 2026-09-15T07:00
```

Quando una condizione scatta, il motore **annota da solo**: il testo va sul chart e nel diario,
insieme ai numeri della barra e all'attesa. Non aspetta la lettura ragionata — quella arriva dopo,
ma intanto sullo schermo c'e' scritto cosa e' successo.

## 4bis. La Sveglia, Per Cio' Che Non Avevamo Previsto

```bash
python3 FabioOrderFlow/tools/sveglia_tape.py --livelli docs/research/giornate/livelli-2026-09-15.json \
    --from 2026-09-15T07:30
```

Gli scenari coprono quello che ci aspettiamo. `sveglia_tape.py` gira in parallelo e copre il resto:
dice **soltanto** che e' il momento di guardare, con i numeri grezzi, su condizioni volutamente
grossolane — un livello attraversato, un livello toccato con volume oltre il p95, una barra fuori
scala. Non conclude niente, e non scrive sul chart.

I due si completano: se una giornata scatta solo sulla sveglia e mai sugli scenari, vuol dire che
gli scenari erano scritti male, ed e' un'informazione che vale la pena registrare nella giornata.

## 5. Annotare La Lettura

Quando l'analisi ha concluso qualcosa, `annota.py` la mette **sul chart e nel diario**:

```bash
python3 FabioOrderFlow/tools/annota.py --prezzo 29275 --tipo rifiuto \
    --testo "29275 ora resistenza: spinta piu' pesante dell'ora, respinta" \
    --misura "384 lotti e delta +78, max 29274,75, chiude 29255,50"
```

- **`--testo`** e' cio' che si legge sul chart: la conclusione, breve.
- **`--misura`** sono i numeri che la sostengono. Non viene disegnata, torna sul `GET` e finisce nel
  diario. E' la regola 4 del [percorso](../percorso-del-progetto.md) applicata alla lettera: la
  soglia, o il numero, sta accanto al risultato che produce.
- **`--tipo`** sceglie solo il colore: rottura rosso, reclaim verde, assorbimento azzurro, rifiuto
  arancio, attesa grigio, nota giallo. Non cambia il significato, che sta nel testo.

Sul chart l'annotazione e' una linea **punteggiata con `!` davanti**, distinguibile a colpo d'occhio
dai livelli strutturali che sono `solid` o `dash`. Ne restano le ultime sei; `--elenco`, `--togli N`
e `--pulisci` gestiscono il resto.

### Il diario non e' un sottoprodotto

Ogni annotazione finisce in `docs/research/giornate/annotazioni-AAAA-MM-GG.json` con l'ora, il
prezzo e la misura. **E' il materiale con cui a fine seduta si scrive la giornata**, ed e' l'unico
modo per rileggere una lettura sapendo quando e' stata fatta e su quali numeri — invece di
ricostruirla a posteriori, quando si ricordano solo quelle che avevano ragione.

## Cosa Questi Strumenti Non Fanno

- **Non derivano livelli.** Li leggono da un file prodotto dall'analisi.
- **Non concludono.** La sveglia mostra fatti, l'annotazione registra una lettura umana.
- **Non toccano ordini ne' posizioni.** L'unica scrittura e' `POST /levels`, che disegna e basta.
- **Non sono segnali operativi.**

E' la stessa divisione dei ruoli dei livelli: **l'indicatore disegna, il client trasporta, la
derivazione resta nell'analisi.**

## Un Vincolo Tecnico Che E' Costato Un Errore

Il campo `bar` restituito dal bridge **e' un indice di posizione, non un identificatore**: riparte
quando ATAS ricarica l'indicatore. Una prima versione di questo strumento ci teneva l'elenco delle
barre gia' valutate, e il 15 settembre alle 10:29, quando il chart e' passato da `04336da9` a
`1e003f54`, ha rivalutato tutta la mattina emettendo quattro segnali falsi.

Per riconoscere una barra si usa `time`, e non si valuta mai una barra piu' vecchia dell'ultima
gia' vista.
