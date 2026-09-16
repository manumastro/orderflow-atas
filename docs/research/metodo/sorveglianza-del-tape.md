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

- **`nome`** identifica lo scenario nei log e nel diario.
- **`sigla`** e' cio' che compare sul chart, e **deve dirsi da sola**: `accetta`, `cede il bordo
  basso`, `entra nel vuoto`. Non lettere o codici — `? A2` su una riga non significa niente per chi
  guarda il grafico mentre opera. Quando la sigla finisce appesa al livello di quel prezzo, il
  prezzo dentro la sigla e' ridondante: su 29400 basta `? riprende`.
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

### Uno Scenario Rotto Deve Gridare, Non Spegnersi

Il 16 settembre `fade del VAH` conteneva `p75delta`, un nome che il contesto non forniva. Finche'
il prezzo e' rimasto sotto il bordo, `h > 29335 and delta <= -p75delta` era falso **al primo pezzo**
e Python non ha mai valutato il secondo. Alla prima barra sopra il bordo l'espressione ha sollevato
`NameError`, e il motore — che allora trattava un errore come uno scatto — l'ha marcato consumato.

Si e' spento da solo, in silenzio, **nel minuto esatto in cui il fade si stava innescando**:
stoppino a 29.357,75, 628 lotti assorbiti, flip a -56. La sequenza c'era tutta e non l'ha vista
nessuno.

Da qui quattro difese, tutte necessarie perche' coprono momenti diversi:

| difesa | quando agisce | cosa impedisce |
|---|---|---|
| `--controlla` | prima di armare, a mano | di armare uno scenario gia' rotto |
| controllo a ogni caricamento | all'avvio e a ogni modifica del file | che una correzione fatta di corsa ne rompa uno |
| lo scenario rotto **non viene armato** | al caricamento | che muoia piu' tardi, quando serve |
| l'errore **non consuma** lo scenario | a ogni barra | che si veda una volta sola e poi mai piu' |

E quando qualcosa e' rotto lo si scrive **sul chart**, non solo nel log:

    !! SCENARIO NON ARMATO  fade del VAH: nome inesistente: p75delta

piu' una riga rossa `SCENARIO ROTTO: ...` fra le annotazioni. Una riga di log si perde fra le
altre; una riga sul grafico sta dove si guarda davvero.

**Perche' il controllo e' statico.** Valutare l'espressione su una barra non basta: `and`
corto-circuita, e un nome inesistente nel ramo destro resta invisibile finche' il sinistro e'
falso. Cioe' resta invisibile **esattamente fino al momento in cui lo scenario conta**. Il
controllo legge l'albero sintattico e confronta i nomi con quelli del contesto, che ricava
costruendo un contesto finto: cosi' l'elenco non puo' divergere da quello vero.


### Armarli

```bash
python3 FabioOrderFlow/tools/scenari.py --giorno 2026-09-15 --from 2026-09-15T07:00
```

Quando una condizione scatta, il motore **annota da solo**: il testo va sul chart e nel diario,
insieme ai numeri della barra e all'attesa. Non aspetta la lettura ragionata — quella arriva dopo,
ma intanto sullo schermo c'e' scritto cosa e' successo.

### Cambiarli Mentre Girano

Durante una seduta gli scenari si riscrivono spesso: il prezzo si sposta, un livello cambia ruolo,
uno scenario si rivela scritto male. **Il file viene riletto appena cambia**, senza riavviare
niente, e il motore dice cosa e' cambiato:

```text
[scenari ricaricati] 10 attivi: +E - ritorno sulla mensola, -D, ~B1
```

`+` aggiunto, `-` tolto, `~` condizione modificata. Due dettagli che rendono la cosa usabile:

- **Correggere un `quando` riarma lo scenario.** Uno scenario gia' scattato e' identificato da nome
  *piu'* condizione, quindi se aggiusti la soglia torna in gioco. Se invece vuoi solo cambiare il
  testo, lo scenario resta scattato.
- **Un JSON rotto a meta' salvataggio non ferma la sorveglianza**: il motore avvisa e tiene la
  versione precedente.

Aspettarsi un riavvio a ogni correzione e' il modo migliore per non correggerli.

### Cosa Si Vede Sul Chart

Due cose, e basta:

1. **I livelli**, che sono l'informazione permanente della giornata.
2. **Le annotazioni**, cioe' le letture gia' fatte e gli scenari **che sono scattati**.

Gli scenari *in attesa* — quelli scritti e non ancora avvenuti — sono **spenti per default**. Si
accendono con `--con-attesi` quando servono, ma non e' lo stato normale: una riga per ogni scenario
previsto riempie il chart di cose che non stanno succedendo, e i livelli si perdono in mezzo. A
schermo serve sapere dove sono i livelli, e vedere comparire qualcosa **quando qualcosa accade**.

### Due tavolozze che non devono competere

I colori vivi — verde, rosso, azzurro, arancio — sono **riservati alle annotazioni**, dove
significano un segnale: reclaim, rottura, assorbimento, rifiuto.

I livelli strutturali usano **tinte smorzate e fredde**: dicono di che zona del profilo fanno
parte, non cosa sta succedendo adesso. Il nodo dei venditori in bruno, quello dei compratori in
verde grigio, il collo e i vuoti in tinte neutre.

La distinzione non e' estetica. Con la stessa tavolozza per entrambi, un livello verde perche'
lunedi' i compratori ci avevano assorbito si legge come un segnale rialzista di adesso — e il 15
settembre 29.275 era verde mentre faceva da **resistenza**. Il colore di una zona descrive il
passato; il colore di un'annotazione descrive il presente.

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

### Una lettura che ne supera un'altra

Durante una seduta la stessa domanda viene letta piu' volte, e le letture si contraddicono per
costruzione. Alle 11:51 del 15 settembre: *"sale sopra il VAL, accettazione non ancora matura"*.
Alle 12:09: *"accettazione sopra il VAL"*. Se la prima resta disegnata, il chart dice due cose
opposte alla stessa altezza, e quella vecchia e' semplicemente falsa.

Le letture sullo stesso argomento si legano con `--tema`, una stringa qualunque purche' uguale:

    ./annota.py --tema "VAL 29306" --prezzo 29336 --testo "accettazione non ancora matura"
    ./annota.py --tema "VAL 29306" --prezzo 29306 --testo "accettazione sopra il VAL"

La seconda marca la prima con `superata_da`. Effetto: **sparisce dal chart, resta nel diario.**

La distinzione e' il punto. Cancellarla sarebbe comodo e sbagliato: a fine giornata resterebbero
solo le letture giuste, che non dimostrano niente perche' sono state selezionate dopo. La lettura
delle 11:51 era corretta nel merito — mancavano i minuti, non il prezzo — e sapere *quando* si e'
capito cosa e' l'unica misura di metodo che la giornata produce. `--elenco` le mostra con un punto
a margine e l'ora di chi le ha superate; `--rianima N` ne rimette una sul chart se serve.

Gli scenari portano il tema nel loro file, e quando scattano lo passano all'annotazione:

    {"nome": "accettazione sopra il VAL di lunedi", "tema": "VAL 29306", ...}
    {"nome": "rifiutato il rientro nel valore",     "tema": "VAL 29306", ...}

Cosi' due scenari che descrivono esiti opposti dello stesso livello non possono restare entrambi
sul chart: il secondo che scatta spegne il primo. Raggruppa per **argomento**, non per direzione —
`VAL 29306`, `muro 29400`, `bordo basso`, `IVB` — perche' e' l'argomento a essere unico, mentre le
letture che ne danno sono molte e successive.

### Se uno scenario e' scattato si legge, non si deduce

Le righe `SCATTA` del motore arrivano raggruppate e con ritardo: il 15 settembre uno scatto sulla
barra delle 13:19 e' comparso alle 13:27, e uno delle 13:26 alle 13:33. Il ritardo non e' negli
strumenti — il bridge risponde in 0,18 secondi e il motore gira ogni 20 — ma nella consegna.

L'errore che ne e' seguito: ho **dedotto** dai dati che uno scenario non fosse scattato, e ho
scritto sul diario che il filtro sul volume lo aveva tenuto fermo. Era scattato da otto minuti e
l'annotazione era gia' nel file.

    ./annota.py --elenco          # l'unica fonte sincrona

Il diario e' scritto dal motore nel momento in cui scatta. Le notifiche dicono **cosa** e'
successo, non **quando** lo si viene a sapere. Prima di affermare che qualcosa non e' scattato, si
guarda l'elenco.

### Il riavvio non deve riscrivere quello che c'e' gia'

Il motore si riavvia spesso: cambiare gli scenari durante una seduta e' la norma, e cambiare il
codice capita. L'elenco di cio' che e' gia' scattato **non puo' vivere solo in memoria**: una
condizione ancora vera al riavvio scatterebbe di nuovo sulla prima barra utile, e il chart
prenderebbe un doppione.

Il diario e' gia' il posto dove sta scritto cosa e' scattato, quindi e' da li' che si rilegge.
L'annotazione porta `scenario` e `condizione`, e la chiave e' la coppia: cosi' **correggere un
`quando` continua a riarmare lo scenario apposta**, che e' il comportamento voluto, mentre un
riavvio a scenari invariati non ripete niente.

    [dal diario] 1 scenari gia' scattati oggi, non si ripetono

Il 15 settembre il doppione si e' visto davvero: l'accettazione sopra il VAL, scattata alle 12:09,
e' stata riscritta alle 12:17 dal motore riavviato. Rimossa con `--togli`, e l'originale rimesso
sul chart con `--rianima` — il meccanismo del tema l'aveva spenta trattando la copia come una
lettura successiva.

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
