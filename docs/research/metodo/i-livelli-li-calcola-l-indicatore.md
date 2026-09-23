# I Livelli Li Calcola L'Indicatore

Stato: **procedura**, e sostituisce il motore esterno. Decisa e messa in opera il **20 settembre
2026**.

> *"non voglio far fare ad un agente ma se possibile ad uno script... e togliere la
> responsabilita' all'agente, se non per quali livelli sono importanti in base alla strategia e a
> tutto il contesto"*

---

## Il Problema Che Chiude

Fino al 20 settembre i livelli li calcolava `livelli_vivi.py`: un processo esterno che ogni trenta
secondi rileggeva le regole, ricalcolava i prezzi e li ridepositava sul chart. Funzionava
**finche' girava**.

Quando moriva — ATAS riavviato, terminale chiuso, un'eccezione — le righe restavano sul chart
**identiche a prima**, con la tilde in coda che continuava a promettere che si muovevano. Un
livello vivo fermo e' peggio di uno scaduto: dichiara una freschezza che non ha, e chi guarda non
ha modo di accorgersene. Era il guasto aperto lasciato dal 20 settembre mattina, e la sera stessa
si e' ripresentato: il processo non girava, e sul chart c'erano otto righe che sembravano di
adesso.

**La difesa non e' accorgersene: e' togliere di mezzo la cosa che puo' morire.** Il motore adesso
e' **dentro l'indicatore**. Se il chart e' aperto, i livelli sono di adesso; se il chart e'
chiuso, non c'e' niente da ingannare. Lo stato intermedio che faceva danno non esiste piu'.

---

## Il Vocabolario: Due Assi, Non Uno

Fino a oggi si diceva *livelli vivi* e *livelli statici*, e quella coppia metteva insieme cose
diverse. Su quindici regole del 14 settembre, **tredici erano gia' calcolate da una regola**: "max
notte", "POC Asia", "min notte" si chiamavano statici solo perche' non si muovevano piu'.

Gli assi sono due, e vanno tenuti separati.

| | **misurato** | **dichiarato** |
|---|---|---|
| **finestra aperta** | `~` si muove a ogni barra | — |
| **finestra chiusa** | `=` fermo, ma nato da una misura | `*` un'affermazione dell'analisi |

- **`~` si muove.** POC e bordi del valore della cash in sviluppo, estremi di sessione. Il
  prossimo minuto puo' spostarli.
- **`=` fermo, ma misurato.** Il POC della notte asiatica quando l'Asia e' chiusa. Non si muove
  piu' e non puo' essere sbagliato: e' il risultato di un conteggio su una finestra finita.
- **`*` dichiarato.** Un prezzo scritto dall'analisi perche' significa qualcosa che nessun
  conteggio cattura. **E' l'eccezione e deve giustificarsi**, perche' e' l'unico dei tre che puo'
  invecchiare senza che niente lo dica.

**Una finestra con una fine dichiarata non e' per forza chiusa.** Una finestra `07:00Z-13:30Z`
alle 13:00Z **non e' ancora finita**: il suo POC si sposta a ogni barra fino alle 13:30. Marcarla
`=` perche' "ha una fine" direbbe fermo di un livello che si muove — la stessa bugia che tutto
questo esiste per togliere. Il marcatore guarda **l'ultima barra chiusa**, non il calendario.
Trovato al primo giro vero, 20 settembre.

---

## Chi Fa Cosa, E Adesso E' Una Riga Sola

```text
l'analisi    QUALI regole, su quale finestra, e a cosa serve arrivare a quel livello
l'indicatore TUTTO il resto: trova i prezzi, li ricalcola a ogni barra, li disegna
```

Non c'e' piu' una colonna "livelli statici — li rifa' l'agente". Il prezzo di un POC, di un bordo
del valore, di un massimo, di una mensola e' una **misura**, e una misura non ha bisogno di
qualcuno che la rifaccia a mano. Resta all'agente la sola cosa che non e' una misura: **quali
livelli contano, dato il metodo e il contesto**, e **a cosa serve arrivarci** — che e' la regola
*un livello non e' mai il fine, e' sempre una porta*.

---

## I Tipi Di Regola

| tipo | cosa trova |
|---|---|
| `poc` | il prezzo piu' scambiato della finestra |
| `vah` / `val` | i bordi del valore, il 70% del volume attorno al POC |
| `massimo` / `minimo` | gli estremi della finestra |
| `nodo_base` / `nodo_top` | i bordi della fascia piu' pesante |
| `mensola` | il prezzo che ha fatto da **minimo** di barra piu' volte |
| `tetto` | il prezzo che ha fatto da **massimo** di barra piu' volte |
| `aggressione` | il prezzo col delta piu' grande in valore assoluto |
| `assorbimento` | molto scambiato, delta quasi nullo: sforzo alto, risultato nullo |
| `muro_sotto` / `muro_sopra` | il prezzo che ha **respinto** da sotto o da sopra, con quattro prove |
| `fisso` | il prezzo sta nel file. **L'eccezione.** |

### I Muri Non Si Depositano: Li Trova L'Indicatore

**Dal 22 settembre 2026 i muri non sono piu' una regola da scrivere nel file.** Il calcolo gira
dentro l'indicatore a ogni barra **sul tratto di seduta in corso** — dopo le 13:30Z solo la
cassa, prima solo cio' che viene prima, dalla chiusura precedente — e disegna le righe da solo: verde sotto, rosso sopra.

**Il tratto, non le ultime N barre.** La prima versione usava una finestra scorrevole di 360
barre e si e' rotta il giorno stesso: **mezz'ora dopo l'apertura i muri erano ancora quelli della
mattina europea, 165 punti sotto il prezzo** — cioe' inutili proprio nel momento in cui servivano.
Una finestra scorrevole trascina dentro un regime che non c'e' piu'. Il tratto no: alle 13:30Z
riparte da zero, e i muri che compaiono sono quelli che la cassa ha costruito. Sotto
`Min lots in window` (15.000) non si disegna niente e si dice *"ancora presto"*, perche' nei primi
minuti tutto il volume sta su pochi prezzi e il piu' scambiato lo e' solo perche' non c'e' altro.

**La riga parte dalla barra di adesso e va a destra**, non per tutto l'asse. Il muro e' misurato
su una finestra che **finisce adesso**: tirarlo indietro sulla seduta intera lo farebbe passare
sopra ore in cui quel prezzo non aveva ancora respinto niente, e **una riga disegnata dove la
misura non vale non si distingue a occhio da una vera**.

Sulla riga c'e' **solo il nome e il prezzo**; i numeri che l'hanno fatta nascere compaiono
**passando sopra col mouse**, come per i livelli delle regole. Tre muri con la spiegazione intera
scritta addosso coprirebbero le candele esattamente dove il prezzo sta lavorando.

```text
sulla riga    MURO SOTTO 30.770,00
sul mouse     MURO SOTTO 30.770,00 · 8 ritorni contro 3 (asimmetria 2,7)
              · 1.436 lotti, 5,5x il mediano · delta pari (0,07)
              · qui il ribasso ha trovato un compratore fermo
```

**Perche' non e' una regola.** Un livello di regola e' una scelta dell'analisi — *guardo il POC
della notte* — e vive quanto la finestra che gli si dichiara. Un muro non e' una scelta: e' un
fatto che o c'e' o non c'e', e **cambia ogni pochi minuti**. Depositarlo vorrebbe dire scrivere a
mano una finestra e aspettare che qualcuno la riscriva, cioe' il difetto del livello fermo che
tutto questo lavoro esiste per togliere.

Si legge anche da fuori, gia' composto, su **`/muri`**. Le impostazioni stanno nel gruppo
**Muri** dell'istanza: le quattro soglie sono lì, una per prova.

I tipi `muro_sotto` e `muro_sopra` **restano** fra le regole, e servono a una cosa sola che il
calcolo automatico non sa fare: cercare un muro dentro una **finestra dichiarata** — la notte, la
mattina europea — invece che nelle ultime N barre.

### Le Quattro Prove, E Perche' Non Basta `assorbimento`

`assorbimento` ordina i candidati e **restituisce sempre il primo**: marca qualcosa anche quando
non c'e' niente da marcare. Un muro inventato fa tenere una posizione contro un prezzo che non
difende nessuno, ed e' peggio di nessun muro.

`muro_sotto` e `muro_sopra` sono lo stesso conto con **una prova che puo' fallire** — quattro, in
realta', e ognuna esce col suo numero accanto:

| | prova | default | cosa esclude |
|---|---|---|---|
| 1 | **sforzo**: volume li' / volume del prezzo mediano | `sforzo_minimo` 3 | il traffico normale |
| 2 | **pareggio**: `abs(delta) / volume` | `pareggio_massimo` 0,15 | dove qualcuno ha vinto: e' aggressione |
| 3 | **tenuta**: barre che hanno girato su quel prezzo | `respinte_minime` 3 | il prezzo di passaggio |
| 4 | **asimmetria**: ritorni dal lato giusto / dal lato opposto | `asimmetria_minima` 2 | **il POC** |

**La quarta e' quella che tiene in piedi tutto**, e senza di lei il muro esce sul POC. Provata dal
vivo il 22 settembre 2026, finestra della notte: il prezzo piu' scambiato era il POC **30.880**, e
passava le prime tre prove — sforzo 3,0x, pareggio 0,13, 12 ritorni. **Asimmetria 1,5**: girava
dodici volte da sotto e otto da sopra. Un prezzo che respinge in tutte e due le direzioni **non e'
un muro, e' un centro**. Stessa mattina, a 30.767: dodici minimi e due massimi, asimmetria 6,0.

Quando nessun prezzo passa, la regola **non disegna niente e dice perche'**, col numero mancante:

```text
muro notte sotto: nessun muro: il piu' scambiato e' 30.880,00, sforzo 3,0x (ne servono 3,0),
pareggio 0,13 (max 0,15), 12 ritorni (ne servono 3), asimmetria 1,5 (ne serve 2,0)
```

**Un muro si vede cosi' sul grafico, senza indicatore:** tanto volume su una riga del footprint,
delta quasi nullo su quella riga — e le barre che la toccano ci **girano sopra** invece di
attraversarla, sempre dallo stesso lato.

`aggressione` e `assorbimento` non sono due tipi qualunque: sono **i due soli livelli che il live
dice di marcare**, entrambi da ordini eseguiti. *"It's not necessary to mark intermediate level
that are useless for us."*

### Mensola E Tetto Si Contano Separati, E Il Perche' Vale La Pena

Al primo tentativo la mensola contava insieme i massimi e i minimi di ogni barra, e ha risposto
**29.172**: il centro del range 29.142-29.195, non un appoggio. Contati insieme, gli estremi
trovano il prezzo piu' visitato, che dentro un range e' il suo mezzo. **Un appoggio e' fatto di
minimi ripetuti, un tetto di massimi ripetuti, e la loro somma non e' niente.**

Separati e su griglia da un punto, la stessa finestra da' **29.147 con quattro minimi**. A mano
era stata scritta **29.142**: quello era un singolo minimo piu' profondo, non l'appoggio. La
macchina e' stata piu' precisa della lettura a mano — che e' il motivo per cui questo lavoro non
deve restare all'agente.

### La Griglia

`grana` (default 1 punto) e' dove si cercano POC, bordi e appoggi. **Sul tick nudo non si
cercano**: il volume di una notte si spalma su millecinquecento prezzi da un quarto di punto, e il
tick piu' scambiato puo' cadere dove non c'e' il cuore del volume. Il 14 settembre il POC sul tick
dava 29.150 mentre la fascia piu' pesante della notte era 29.300-29.324, col 18,4% contro il
13,7%: due misure diverse, e quella sul tick avrebbe messo sul chart un POC che non era il POC.

`passo` (default 25 punti) e' la griglia dei **nodi**, cioe' delle fasce di cui l'etichetta
dichiara la percentuale.

Per una mensola su NQ, `grana` 1 o 2. A 4 punti i minimi si fondono e la regola risponde 29.164,
che non e' niente.

---

## La Finestra

Si dichiara **per regola**, perche' finestre diverse misurano popolazioni diverse: il POC della
cash e quello della notte sono due numeri, non due stime dello stesso numero.

```json
"finestra": {"da": "13:30Z"}                                    dalle 13:30Z a adesso
"finestra": {"da": "2026-09-13T22:00:00Z", "a": "07:00Z"}       una finestra chiusa
```

Un orario nudo si appoggia al **giorno di mercato** — in replay quello del replay, non
l'orologio del muro. Se la fine cade prima dell'inizio, la finestra **attraversa la mezzanotte**
e l'inizio si sposta al giorno prima: e' il caso normale della notte, `22:00Z -> 07:00Z`.

```json
"finestra": {"da": "13:30Z", "a": "20:00Z", "seduta": -1}      la cassa della seduta precedente
```

**`seduta: -1` e' la seduta precedente che ha barre in quella finestra**, quindi il lunedi' e'
venerdi' e non una domenica vuota. Serve una fine dichiarata. Nasce il 23 settembre 2026: il VAH
della cassa del giorno prima era il livello piu' importante della mattina — testato e tenuto alle
04:16 — e il chart non poteva disegnarlo. Le regole di cassa guardano solo la cassa di oggi, e una
data assoluta invecchia in una notte, come era gia' successo due volte con la finestra d'Asia.

**Due livelli piu' vicini dello stacco minimo non si perdono piu': si uniscono.** Resta la riga del
primo dichiarato, e la sua etichetta porta anche l'altro nome col suo prezzo — *"MIN EUROPA + VAL
notte 31.015,00"*. Prima il secondo spariva, e quella stessa mattina era sparito proprio il VAL
notte, il bordo su cui si stava lavorando.

---

## L'Etichetta Dice Il Peso

`{prezzo}`, `{pct}`, `{lotti}`, `{delta}`, `{tocchi}` vengono sostituiti con la misura corrente,
cosi' il chart dice **quanto pesa** il livello senza tornare al documento.

```json
"label": "POC EUROPA {pct} · prezzo piu' scambiato della gamba che ha portato qui"
```

Il marcatore entra **in coda al nome**, prima del ` · `, non in fondo alla riga: in fondo
finirebbe dopo la frase che dice a cosa serve il livello, dove nessuno lo cerca.

---

## Lo Scenario: Tre Nomi, Non Tre Prezzi

Uno scenario dichiara a cosa serve arrivare al livello, e lo fa **per nome**, mai scrivendo un
prezzo: i prezzi cambiano a ogni barra, i nomi no.

```json
"scenario": {
  "direzione": "LONG",
  "nome": "fade del bordo basso verso il POC (mean reverting)",
  "bersaglio_livello": "POC cash",
  "invalida_livello": "VAL Europa",
  "pareggio_livello": "mensola europea"
}
```

**`pareggio_livello` e' il break even, ed e' il campo che mancava fino al 20 settembre 2026.** Nel
live Q1 la gestione **e' l'edge** e questa ne e' la regola singola piu' importante: il break even
si mette **su un livello** — il prezzo al quale l'analisi si smonta, cioe' dove il lato opposto
tornerebbe a vincere — e **non dopo N punti**.

> *"Why I put the break even point at zero is point at 65? This is where the buyers got completely
> absorbed. So it's a level where you could expect to see sellers getting back in."* `[1 · 2:11:58]`

Due differenze dagli altri due campi, e sono deliberate:

- **non ha lo stacco minimo** che difende l'invalidazione. Il break even di Fabio e' vicino
  apposta — *"now you understand why my break even point was so close"* `[3 · 15:39]` — e se viene
  toccato non si perde niente: *"break even it's a free attempt, so you don't risk anything"*
  `[3 · 14:42]`. Applicargli la difesa dello stop lo cancellerebbe proprio quando e' fatto bene.
- **quando manca, il pannello lo scrive** (`pareggio NON DICHIARATO`) invece di tacere. Uno stop
  senza il suo break even e' meta' istruzione, e il campo vuoto e' esattamente il difetto.

---

## Come Si Deposita

```bash
python3 FabioOrderFlow/tools/bridge.py rules --chart NQZ6 \
        --file docs/research/giornate/regole-dei-livelli-NQZ6-2026-09-14.json
```

Il deposito **sostituisce tutto** e ricalcola subito. Poi ricalcola da solo **a ogni barra
nuova**, e le regole sopravvivono a un redeploy: stanno in `~/.fabio-data-bridge-rules.json`, con
una voce per strumento.

Non c'e' niente da tenere acceso, niente `--ogni`, niente processo in background.

**Il deposito cancella sempre tutto prima di scrivere, e non e' facoltativo.** Un livello che
l'analisi non ha appena riconfermato non deve restare sul grafico: un residuo di un altro giorno
si disegna esattamente come una misura di adesso, e chi guarda non puo' distinguerli. Il
19 settembre, aprendo un replay del 14, sul chart c'erano ancora otto livelli del 18 —
quattrocento punti sopra il mercato. Con le regole questo e' automatico, perche' ogni ricalcolo
riscrive l'elenco intero; **resta a carico di chi scrive il file non lasciarci dentro una regola
di ieri**, e in particolare un `fisso`, che e' l'unico tipo che non si accorge del giorno.

**Depositare livelli a mano su `/levels` spegne le regole, ed e' esplicito apposta.** Le due cose
scrivono nello stesso posto: se restassero accese entrambe, alla prima barra nuova il motore
ricalcolerebbe e i livelli appena depositati sparirebbero senza che nessuno abbia sbagliato
niente. Meglio dire che il controllo e' passato a mano, che vederlo tornare indietro da solo.

---

## Le Difese, E Nessuna E' Decorativa

- **La barra in formazione non entra mai in una misura.** Cambierebbe da sola fra un tick e il
  successivo, e il chart mostrerebbe un livello che balla senza che il mercato abbia fatto niente.
- **`minimo_lotti`.** Nei primi minuti di cash tutto il volume sta in una fascia sola, e il POC
  direbbe *"89% del volume"* perche' non c'e' altro. Non e' una misura prematura, e' **una misura
  falsa**: sotto la soglia il livello non si disegna e il pannello dice perche'.
- **Lo sfoltimento.** Due etichette a meno di otto punti su NQ si sovrappongono e diventano
  illeggibili entrambe: il chart perde due informazioni invece di guadagnarne una. Vince **il
  primo dichiarato nel file**, perche' l'ordine nel file e' la priorita' scelta dall'analisi — e
  questo va usato: il 20 settembre la mensola, dichiarata prima, cancellava il POC europeo che
  stava tre punti sopra. Cosa e' caduto viene **detto**, non nascosto.
- **Una invalidazione troppo vicina non e' una invalidazione.** Sotto i dieci punti dal livello,
  lo stop sta dentro il rumore e non dietro una struttura: il campo si toglie e si dichiara,
  perche' quel numero sul pannello e' peggio di un campo vuoto — sembra una misura.
- **Bersagli e invalidazioni si dichiarano per NOME di livello**, mai come prezzo: un bersaglio
  scritto come numero invecchia, il POC della cash si sposta a ogni barra. Li risolve il motore a
  ogni giro, e se un nome non si risolve il campo si toglie invece di inventare un prezzo.

---

## Cosa Dice Il Pannello, E Cosa Dice Il Giro

**Il pannello non elenca piu' le regole.** Fino al 20 settembre 2026 stampava quante ne erano
state risolte, la legenda dei marcatori e l'elenco di quelle che non avevano prodotto una riga.
Tolto su richiesta dell'utente: sul chart era ingombro e non serviva a decidere.

Quell'informazione **non e' andata persa**, e' cambiata di posto. Sta su `/rules`, la stampa
`bridge.py rules`, e il giro d'orizzonte la mette alla sezione **6ter**:

```text
6ter. I LIVELLI SUL CHART, E COSA NON C'E'
  11 livelli disegnati da 15 regole, ricalcolati alla barra 15:20Z
  5 regole non hanno prodotto una riga, e non sono un guasto:
    - VAH cash: invalidazione a 4 punti, sotto i 10 richiesti
    - max cash 29.379,00: a 4,00 punti da «VAH cash», che e' dichiarato prima (stacco minimo 8)
```

**Sono contesto, non diagnostica**, e la riga lo dice. *"POC cash: finestra ancora vuota"* vuol
dire che la cash non e' aperta. E il caso piu' frequente — due livelli a meno di otto punti — e'
proprio il fatto che conta: **due letture indipendenti indicano lo stesso posto**. Il motivo
nomina chi ha vinto, non solo il prezzo: scritto *"coincide con 29.172,00"*, come era fino al
20 settembre, obbligava a cercarsi a mano quale livello fosse quel prezzo.

**Sul pannello e' rimasta una riga sola, e appare solo se serve:**

```text
LIVELLI FERMI DA 4 BARRE — ultimo ricalcolo 15:55
```

Non si toglie per fare spazio. Un pannello fermo e' indistinguibile da uno aggiornato — e' il
guasto del livello vivo — e il motore, pur non potendo morire da solo, produrrebbe lo stesso
inganno se un'eccezione entrasse nel ricalcolo.

---

## Cosa Non Fa, E Non Deve Fare

Non giudica il mercato, non avvisa, non scatta. **Non sono le condizioni armate della fase
chiusa**: le condizioni le scrive l'analisi nel file, la macchina le spunta soltanto. Il motivo
per cui quella strada e' chiusa sta in [`il-contesto-vivo.md`](il-contesto-vivo.md) — una
condizione armata e' una previsione travestita da misura, e taceva proprio quando il mercato
faceva qualcosa di non previsto.

---

## Cosa E' Stato Ritirato

- `livelli_vivi.py` — il motore esterno. Resta nel repository come evidenza e **non si lancia
  piu'**: se lo si lancia, deposita su `/levels` e quindi **spegne le regole**.
- Il file `livelli-vivi-STRUMENTO-AAAA-MM-GG.json` si chiama ora
  **`regole-dei-livelli-STRUMENTO-AAAA-MM-GG.json`**, perche' contiene regole e non livelli, e il
  nome sbagliato e' la prima cosa che fa divergere un documento.
- La colonna *"livelli statici — li rifa' l'agente su richiesta"* della divisione del lavoro.
  Il problema *"chi rifa' i livelli statici"*, aperto il 20 settembre mattina in
  [`i-livelli-statici-la-strada-del-sottoagente.md`](i-livelli-statici-la-strada-del-sottoagente.md),
  **non si risolve: si dissolve.** Quasi tutti quei livelli erano gia' regole, e i pochi che non
  lo erano sono diventati `mensola`, `tetto`, `aggressione`, `assorbimento`.
- `serve_rifare.py` resta utilizzabile ma ha molto meno da dire: misurava la scadenza di una mappa
  che adesso si rifa' da sola a ogni barra.

---

## Dove Sta Il Codice

| | |
|---|---|
| il motore | [`FabioOrderFlow/src/Observation/RegoleDeiLivelli.cs`](../../../FabioOrderFlow/src/Observation/RegoleDeiLivelli.cs) |
| il disegno e il pannello | [`FabioOrderFlow/src/Observation/DataBridge.cs`](../../../FabioOrderFlow/src/Observation/DataBridge.cs) |
| il trasporto | `bridge.py rules` |
| il contratto HTTP | [`contratto-data-bridge.md`](contratto-data-bridge.md) |

**Dopo un `deploy.sh` ATAS X ricarica l'indicatore da solo**: non serve riavviarlo, e la
posizione del replay non si perde. (Il vecchio ATAS 8 invece lo richiedeva.)

---

## Su Ogni Livello, A Sinistra: Cosa Ci E' Successo

**Dal 23 settembre 2026 sforzo, delta, esito e big trades non stanno piu' nel pannello, e non
riguardano piu' un livello solo.** Ogni livello porta a sinistra, sulla sua riga, un'etichetta:

```text
[▮▮▮▮▯▯] 2.128 lotti  Δ +168   124 tocchi · tiene 67 · passa 57   big —
```

- **la barretta** e' la quota di acquisti aggressivi (verde) contro vendite aggressive (rosso) a quel
  prezzo: si legge prima dei numeri;
- **lotti e Δ** sono quelli scambiati nella fascia di due tick attorno al livello;
- **tocchi, tiene, passa**: quante barre ci sono arrivate, quante sono tornate dal lato da cui
  venivano, quante l'hanno attraversato;
- **big**: gli ordini da 60+ lotti a quel prezzo. **Se il registro del tape non copre il tratto, non
  si scrive niente**, perche' *"big —"* direbbe zero e zero non e' *"non lo so"*.

**La finestra e' il tratto di seduta in corso**, la stessa dei muri: dopo l'apertura la cassa, prima
tutto cio' che viene dalla chiusura precedente. Il primo giro usava le ultime dieci barre del
pannello, e su nove livelli sette dicevano *"0 lotti, 0 tocchi"*: non erano muti perche' non
contavano, erano muti perche' la finestra era sbagliata.

Il pannello tiene il livello in gioco (nome, distanza, lato di arrivo) e le condizioni dello
scenario, che restano misurate sulle ultime dieci barre. Le stesse misure si leggono dal bridge nel
campo `alLivello` di `/panel`.

---

## Su Un Altro Strumento: La Scala

**Le soglie in punti sono tarate sul NQ**: stacco fra livelli 8, raggio del livello in gioco 12,
stacco dell'invalidazione 10, distanza fra muri 8, big trade 60 lotti. Sull'oro la cassa fa 48 punti
di range contro i 290 del NQ: con le soglie del NQ due livelli d'oro a cinque punti — lontani —
diventavano lo stesso livello, e 60 lotti d'oro sono un evento raro.

Dal 23 settembre 2026 l'indicatore **misura la scala dello strumento** (`LaScalaDelloStrumento.cs`)
e ci moltiplica le soglie:

| | NQ | oro (GCZ6) | crude (CLZ6) |
|---|---|---|
| scala dei punti = range mediano di cassa / 290 | **1**, fissa | 0,166 | 0,007 |
| scala dei lotti = volume mediano di cassa / 346.567 | **1**, fissa | 0,150 | 0,079 |
| stacco fra livelli | 8 | 1,3 | 0,06 |
| raggio del livello in gioco | 12 | 2,0 | 0,09 |
| stacco dell'invalidazione | 10 | 1,7 | 0,07 |
| big trade | 60 lotti | 9 lotti | 5 lotti |

**La cassa cambia con lo strumento.** Su NQ e oro e' 13:30Z-20:00Z; sul crude e' il pit NYMEX,
**13:00Z-18:30Z**. Vale per la scala, per il tratto dei muri e delle etichette e per il delta
"da apertura" del pannello; le regole del crude la dichiarano nelle loro finestre. Un `13:30Z`
scritto nelle proprieta' di un chart di crude si tratta come il template ereditato, non come
una scelta, e si usa 13:00Z.

Sul NQ la scala e' 1 **fissa**: e' lo strumento del metodo e le sue soglie non devono muoversi da
sole. Sugli altri si misura sulle ultime sei casse in memoria (13:30Z-20:00Z); i due riferimenti
sono le mediane del NQ sulle sedute 15-22 settembre. Si forza dalle proprieta' dell'istanza,
gruppo **Instrument**, e si legge su **`/scala`**.

**Nel file delle regole grana, passo, tolleranza e `minimo_lotti` si scrivono a mano** per lo
strumento: sono campi della regola, non soglie dell'indicatore. Per l'oro:
`regole-dei-livelli-GCZ6-2026-09-23.json`, grana 0,5, passo 5, `minimo_lotti` 2.500.

**E' uno strumento di studio.** Lo strumento operativo resta il NQ (CLAUDE.md, seconda decisione).
Il file dell'oro non ha scenari.
