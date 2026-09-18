# La Sessione Di Londra

> **Stato dal 18 settembre 2026: misura conservata, non piu' operativa.**
>
> I numeri su NQZ6 restano buoni e la lezione metodologica vale — aprire una sessione nuova serve a
> scoprire quali costanti erano parametri. Ma **il corso comanda e il corso dice New York**:
> *"London session is not giving me the same momentum and edge that I have on New York"*,
> *"you take a lot of stop-loss in the London because the market is really choppy"*
> (`fabio_q1/fabio_3.txt`, 22:12 e 23:00).
>
> La deroga dell'utente del 17 settembre e' anteriore alla decisione del 18 settembre di far
> comandare il corso. **Questo documento non autorizza piu' una operazione.** Sessione operativa e
> sue regole: [`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md), sezione 8.

Questo repository e' nato su New York e lo presuppone dappertutto senza dirlo: l'IVB e' i primi
trenta minuti delle 13:30Z, il gate Tier 01 e' una chiusura M30 di quella finestra, le soglie in
lotti vengono dal volume della cash americana. Questo documento dice **cosa cambia se si opera
anche Londra**, con i numeri misurati invece che con l'analogia.

Nato il 17 settembre 2026 da una richiesta esplicita dell'utente: *«considera che io voglio tradare
anche london»*.

---

## La Misura, Prima Di Tutto

NQZ6, **quattro sedute complete** nel bridge (11, 14, 15, 16 settembre 2026). Gli altri giorni
disponibili hanno la storia M1 troncata — e **la troncatura riguarda anche la finestra di New
York degli stessi giorni**, quindi non e' un buco di Londra: e' il limite dello storico. Quattro
giorni sono pochi, e ogni numero qui sotto va riletto quando ce ne saranno venti.

| finestra | ora italiana | lotti/giorno | media per barra M1 | escursione media |
|---|---|---|---|---|
| notte asiatica | 02:00-09:00 | 24.643 | 62 | 165,9 pt |
| **Londra** | **09:00-15:00** | **35.465** | **102** | **200,1 pt** |
| **New York cash** | **15:30-22:00** | **238.882** | **613** | **336,4 pt** |
| dopo la chiusura | 22:00-02:00 | 15.864 | 106 | 84,8 pt |

### Il risultato che decide come si opera Londra

**Londra fa il 60% dell'escursione di New York con il 15% del volume.**

200 punti contro 336, su 35.465 lotti contro 238.882. Non e' una sessione morta — si muove, e si
muove abbastanza da valerne la pena. Ma si muove **con sei volte meno partecipazione per minuto**,
e questo ha una conseguenza precisa, non generica:

**Il prezzo si sposta con poca convinzione dietro.** Le stesse escursioni costano un sesto degli
scambi, quindi ogni livello viene attraversato piu' facilmente e ogni rottura ha meno persone
dentro. E' esattamente la firma che si e' vista il 16 e il 17 settembre — 238 punti giu' e 247
punti su, entrambi a delta netto quasi nullo — ma a Londra e' la **condizione normale**, non
l'eccezione di una giornata.

**Conseguenza operativa:** a Londra il delta conferma meno. Un `delta >= p95` di Londra e' 54
lotti; a New York e' 170. Quando la conferma vale un terzo, la struttura — da dove arriva il
prezzo, cosa c'e' sopra e sotto, chi resta intrappolato — deve pesare di piu', non di meno.

---

## Le Soglie Riscalate

**Non si copiano da New York**, per la stessa ragione per cui non si copiano fra strumenti
([`come-si-apre-un-asset.md`](come-si-apre-un-asset.md)). Misurate sulle stesse quattro sedute:

```text
                      LONDRA        NEW YORK      rapporto
  volume M1 medio        102            613         1 : 6,0
  p25 volume M1           38            163         1 : 4,3
  p75 volume M1          130            805         1 : 6,2
  p95 volume M1          298          2.027         1 : 6,8
  p95 |delta| M1          54            170         1 : 3,1
```

**I percentili della seduta non bastano, serve anche un pavimento assoluto.** E' la regola nata il
17 settembre da un `--prova`: uno scenario che chiedeva `vol >= p75vol` sarebbe scattato alle 02:44
su **290 lotti**, perche' il p75 e' relativo e di notte si assottiglia insieme al libro. Su una
condizione che puo' scattare a Londra si scrive **`vol >= p75vol and vol >= N`**, con N preso dalla
tabella sopra:

```text
  pavimento assoluto suggerito, sessione di Londra, NQZ6
    condizione ordinaria      vol >= 130   (il p75 di Londra)
    condizione che deve pesare vol >= 300  (il p95 di Londra)
```

Un pavimento di 800-2.000 lotti, giusto per New York, a Londra **non scatta mai** e rende lo
scenario muto. Un pavimento di 130, giusto per Londra, a New York **non vincola niente** perche' il
p75 locale e' gia' sei volte tanto. Per questo si scrivono **entrambi**: il percentile si adatta
alla sessione, il numero assoluto impedisce al percentile di sciogliersi.

---

## Il Gate Tier 01 Non Esiste A Londra, E Non Si Inventa

Il dossier definisce il permesso direzionale su **una finestra sola**: i primi trenta minuti della
cash di New York, con la chiusura M30 che vota
([`glossario-del-metodo.md`](glossario-del-metodo.md), Tier 01, righe 90-138). Non c'e' nessuna
riga che parli di una finestra di apertura europea.

**Quindi, operando a Londra:**

- **il `verso` di ogni scenario resta `NESSUN PERMESSO`.** Non per prudenza: perche' non esiste una
  riga della fonte da cui farlo discendere, ed e' esattamente la regola di `CLAUDE.md` — *una
  affermazione con conseguenza operativa deve essere tracciabile a una riga della fonte*.
- **la dicitura `PRE-GATE` va nel file della giornata, non in ogni risposta.** Operare prima del
  gate e a Londra e' una deroga che l'utente ha gia' dichiarato e conosce: ripetergliela a ogni
  lettura non aggiunge niente e copre la direzione, che e' la cosa per cui la lettura esiste.
  Regola in `CLAUDE.md`, *"Una Lettura Dal Vivo Dice La Direzione"*.
- **il permesso di fatto non e' calcolabile** finche' l'IVB non esiste.
  `permesso_di_fatto.py` chiede `--alto` e `--basso`: prima delle 14:00Z quei due numeri sono
  quelli di **ieri**, e misurare l'accettazione su un bordo scaduto risponde a una domanda che
  nessuno ha fatto. Si accende dopo le 14:30Z, non prima.

### Esiste un "IVB di Londra"? Aperto, e i dati non bastano

Misurato sui primi trenta minuti dalle 07:00Z, sulle quattro sedute complete: **l'IVB di Londra e'
stato rotto tutte e quattro le volte**, e in tre casi su quattro la rottura ha tenuto fino alle
13:00Z.

**Quattro osservazioni non dimostrano niente**, e vanno registrate come tali: un range di apertura
rotto quattro volte su quattro potrebbe voler dire che non contiene, oppure che trenta minuti sono
una finestra troppo stretta per il volume di Londra, oppure niente. **Non si arma nessuno scenario
su questa base.** Il compito resta aperto: rifare la misura quando il bridge avra' venti sedute
complete, e confrontare con l'IVB di New York sugli stessi giorni.

---

## Cosa Si Usa A Londra Al Posto Del Gate

Il riferimento c'e' gia' ed e' misurabile: **il valore costruito nella notte**.

A Londra il prezzo arriva dopo dieci ore di globex, e quelle dieci ore hanno un profilo. Il
17 settembre il bilanciamento notturno era 29.368,75-29.478 con il 44% del volume in una fascia da
25 punti: **quello e' un valore ben formato**, e i suoi bordi sono livelli veri nel senso di
`CLAUDE.md` — dicono cosa si apre attraversandoli.

```text
  si misura           il profilo del globex fino a quel momento (bridge.py candles + profilo)
  si disegna          POC notturno, bordi del bilanciamento, e i livelli di ieri che restano
  si arma             i DUE bordi, uno scenario per lato (settima prova)
  non si dichiara     nessun permesso: verso NESSUN PERMESSO, lettura PRE-GATE
```

**E il mean reverting resta quello del dossier**, righe 139-196: fade dei bordi della value area
**verso l'interno**, con stoppino, assorbimento e aggressione che gira. A Londra la value area da
fadare e' quella della notte, non quella della cash di ieri — e i bordi vanno rimisurati a ogni
lettura, come sempre ([`livelli-sul-chart.md`](livelli-sul-chart.md)).

---

## I Sorveglianti, Riavviati Sulla Sessione Giusta

`sveglia_tape.py` calcola le soglie di "fuori scala" su `--storia` minuti. **Se lo si accende a
Londra con una storia che contiene solo il globex, i percentili nascono da libro sottile** e
all'apertura della cash griderebbero a ogni barra. Il 17 settembre la sveglia e' partita con
`p95 volume 296` sulle 580 barre della notte, contro un p95 di New York di 2.027: sette volte
troppo basso.

```text
  a Londra          --storia 600   copre il globex, ed e' la misura giusta PER LONDRA
  alla cash         si RIAVVIA con --storia che includa la cash di ieri
```

**Non e' un difetto dello strumento: e' la stessa soglia che significa due cose diverse in due
sessioni.** La regola e' che i sorveglianti si riarmano **al cambio di sessione**, non solo alla
scadenza del monitor.

---

## Cosa Resta Da Fare

1. **L'IVB di Londra su venti sedute**, per rispondere alla domanda lasciata aperta sopra.
2. **Le soglie su piu' di quattro giorni**: i numeri della tabella sono un ordine di grandezza
   affidabile e una cifra decimale no.
3. **Il confronto di esito**: se dopo N sedute le letture prese a Londra perdono dove quelle di
   New York tengono, cade questa estensione, non il metodo. Stessa gerarchia del permesso di
   fatto ([`il-permesso-si-misura-non-si-aspetta.md`](il-permesso-si-misura-non-si-aspetta.md)).
