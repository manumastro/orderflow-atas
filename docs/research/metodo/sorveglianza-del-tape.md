# Sorveglianza Del Tape: Dai Livelli Alla Lettura Sul Chart

Procedura standard di studio per una seduta. Chiude il cerchio fra le tre cose che finora stavano
separate: l'analisi che deriva i livelli, il chart che li disegna, e il tape che li mette alla
prova minuto per minuto.

La divisione dei ruoli e' il punto: **la sveglia dice dove guardare, l'analisi conclude,
l'annotazione riporta la conclusione sul chart.** Il giudizio non sta mai dentro un programma.

Prerequisiti: [`profile-framing.md`](profile-framing.md) per derivare i livelli,
[`livelli-sul-chart.md`](livelli-sul-chart.md) per come arrivano sul chart,
[`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md) per dove
finisce il risultato.

## Il Giro Completo

```text
1. analisi          il profilo delle sedute passate produce i livelli del giorno
2. file             i livelli si scrivono in docs/research/giornate/livelli-AAAA-MM-GG.json
3. chart            bridge.py levels --file li spinge, l'indicatore li disegna
4. sveglia          sveglia_tape.py legge lo stesso file e avvisa quando c'e' da guardare
5. lettura          l'analisi guarda, con finestre, confronti e orologio, e conclude
6. annotazione      annota.py mette la conclusione sul chart e nel diario della giornata
7. giornata         a fine seduta il diario diventa la cronaca in AAAA-MM-GG.md
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

## 4. Armare La Sveglia

```bash
python3 FabioOrderFlow/tools/sveglia_tape.py \
    --livelli docs/research/giornate/livelli-2026-09-15.json \
    --from 2026-09-15T07:30
```

`sveglia_tape.py` fa **una cosa sola: dire che e' il momento di guardare**, e mostrare i numeri
grezzi di cio' che l'ha fatto scattare. Non conclude niente.

Scatta su condizioni volutamente grossolane:

- un livello **attraversato** dalla chiusura;
- un livello **toccato** da un massimo o un minimo, con volume oltre il p95;
- una barra **fuori scala** per volume o per delta (una volta e mezza il p95).

I percentili si ricalcolano a ogni giro sulla seduta in corso: una seduta sottile e una densa non
hanno le stesse barre grosse, e una soglia fissa in una delle due e' inutile.

Valuta **solo le barre chiuse**: quella in formazione cambia volume e delta fino all'ultimo
secondo, e giudicarla produce letture che poi non reggono. Su ogni livello c'e' un silenzio di
dieci minuti dopo una sveglia, altrimenti un livello conteso ne genera una per oscillazione.

### Perche' non giudica

Una versione precedente classificava i segnali da sola — `ASSORBIMENTO`, `RIFIUTO`, `ROTTURA` — con
soglie su volume e delta della singola barra. Sembrava intelligente e non lo era: **i ruoli dei
livelli cambiano durante la seduta**, e il 15 settembre ha etichettato come assorbimento su un
supporto quello che era un rifiuto su una resistenza, perche' quel supporto era stato rotto
dodici minuti prima.

Il punto non e' che mancasse una regola. E' che una lettura ha bisogno di contesto che una barra
non contiene:

| Serve | La barra singola non ce l'ha |
|---|---|
| Delta cumulato della finestra | una barra a delta zero dentro trenta minuti a −6% dice un'altra cosa |
| Il test precedente dello stesso livello | "2.289 lotti contro i 1.561 di prima" e' la lettura piu' utile che si possa fare |
| L'accettazione dopo una rottura | 174 minuti su 198 sotto, e il 10% del volume sopra |
| La struttura | massimi decrescenti, minimi che si estendono |
| La posizione nel profilo | fascia magra o nodo, che viene dal footprint e non dai livelli |
| L'orologio | apertura di Londra, finestra IVB delle 15:30 |

Tutto questo lo fa l'analisi. La sveglia dice solo dove guardare.

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
