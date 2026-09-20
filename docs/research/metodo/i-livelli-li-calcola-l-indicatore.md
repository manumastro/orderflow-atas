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
| `fisso` | il prezzo sta nel file. **L'eccezione.** |

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

## Cosa Dice Il Pannello

```text
REGOLE     7 livelli da 15 regole, barra 15:00
           ~ si muove    = misurato, fermo    * dichiarato a mano
           non disegnato: POC cash: finestra ancora vuota
```

L'eta' si dichiara **anche adesso che il motore non puo' morire da solo**: un'eccezione dentro il
ricalcolo produrrebbe lo stesso inganno, in silenzio. Se il ricalcolo resta indietro di piu' di
una barra, la riga diventa ambra e lo scrive.

Le regole che non hanno prodotto un livello sono **contesto, non diagnostica**: *"POC cash:
finestra ancora vuota"* dice che la cash non e' aperta. Senza quella riga si nota soltanto che una
riga attesa non c'e'.

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
