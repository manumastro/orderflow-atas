# Il Metodo Di Traderyush

Stato: **fonte di idee, non regola.** Scritto il 27 ottobre 2026 su richiesta dell'utente. Niente di
questo documento e' una regola operativa approvata: e' un'idea che, per diventare regola, deve
passare la pipeline del metodo quantitativo (`quant/metodo/`), con la sua scheda, il modello nullo e
il fuori campione guardato una volta. L'indicatore che ne nasce **descrive**, non da' segnali.

Fonte: [`fabio_course/other_traders/traderyush.txt`](../../../fabio_course/other_traders/traderyush.txt),
un episodio di Chart Fanatics con un trader di prop firm che opera NQ ed ES con l'order flow,
tradotto in italiano, circa 98 KB, senza minutaggio.

**Convenzione di citazione.** Il file e' una riga sola. Le citazioni `riga N` si riferiscono alla
versione piegata a 200 caratteri, che si rifa' identica con:

```bash
fold -s -w 200 fabio_course/other_traders/traderyush.txt > yush.txt   # 500 righe
```

Fabio si cita come nel resto del repository, `[lezione · minuto]`, da
[`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md).

---

## In Parole Semplici

**Cosa fa questo trader.** Prima dell'apertura di New York si segna dei prezzi. Un prezzo diventa
una "zona" quando **almeno due** di quattro cose ci cadono sopra insieme: un livello che il mercato
ha gia' fatto (massimo e minimo di ieri, massimo e minimo della notte, i primi 30 minuti), un punto
del profilo di volume di oggi (i bordi del valore, o un buco dove si e' scambiato poco), un ordine
grosso (75 lotti o piu' su NQ), un accumulo di delta in un blocco di 30 minuti. Poi **aspetta** che
il prezzo torni li', e guarda cosa succede: se chi spinge si fa assorbire, o resta intrappolato,
entra contro di lui. Ha due soli modelli: uno per le giornate in equilibrio (vende il bordo alto
del valore, compra il basso, mai al centro) e uno per le giornate in tendenza (aspetta il
ritracciamento dentro un buco di volume e rientra nel verso della giornata). Opera solo le prime
una o due ore, due o tre operazioni al giorno, su NQ a 2 minuti.

**Cosa prendiamo.** Quasi tutto il suo impianto e' gia' quello di Fabio, detto in un altro modo, e
questo e' gia' un'informazione: due trader indipendenti che arrivano alle stesse cose
(assorbimento, trappole, non operare al centro, aspettare la mezz'ora, size ridotta sul controtrend).
Le cose **nuove** per noi sono quattro:

1. **la confluenza contata**: una zona vale per quante fonti diverse ci cadono sopra, e lui ne vuole
   almeno due. E' una regola semplice e misurabile, quindi verificabile;
2. **il delta che riparte ogni 30 minuti**, invece che dall'apertura: dice dove si e' combattuto
   *in quel blocco*, non nella giornata;
3. **la migrazione del valore durante la giornata**: guardare dove si sposta il valore *mentre* la
   seduta si forma, non solo da un giorno all'altro;
4. **i livelli che Fabio non usa**: chiusura di ieri, massimo e minimo della notte, opening range
   di 30 minuti.

Per ora un indicatore nuovo, **TraderYush**, disegna **solo i livelli** che il trader si segna
(ieri, il valore di ieri, la notte, l'apertura e i primi 30 minuti), ciascuno con una retta e
un'etichetta `NOME (SIGLA): PREZZO`. Le altre tre cose (confluenza contata, delta a blocchi,
migrazione del valore) restano descritte qui come sviluppi possibili (sezione 6.5).

**Cosa non prendiamo.** La parte di marketing (sponsor, pagamenti, "4.000 like"), la regola di
coerenza della prop firm come criterio di uscita, l'idea che una zona "4 su 4" sia migliore (e'
una sua impressione, non una misura), e i numeri delle operazioni mostrate: sono esempi scelti,
senza nullo e senza conteggio di quelle andate male. **E dove contraddice Fabio, comanda Fabio**
(la struttura delle candele, l'M1, i livelli di apertura e chiusura): lo si dice punto per punto
sotto.

---

## 1. I Quattro Strumenti

Il trader li chiama "quattro criteri" e li usa per **costruire** supporto e resistenza, non per
entrare (righe 18-20).

### 1.1 I livelli generati dal mercato

> *"uso il massimo del giorno precedente, il minimo del giorno precedente [...] e questo e' solo RTH
> perche' opero solo nella sessione di New York [...] il massimo overnight, il minimo overnight,
> che va dalle 21:00 EST alle 9:29 EST [...] l'orb high e l'orb bottom"* (righe 21-23)

- **RTH** = 09:30-16:00 ET (riga 28).
- La **notte** dalle 21:00 ET: lo dice cosi', ma il globex apre alle 18:00 ET. Nell'indicatore e'
  un'impostazione, col default del transcript. **Da decidere** (vedi in fondo).
- **ORB 30**: il transcript dice *"orb 302"* (riga 23) e *"l'orbita bassa, 30 secondi o basso
  valore"* (riga 473), cioe' con ogni probabilita' l'opening range dei primi 30 minuti, storpiato
  dalla trascrizione. Concetto preso *"dai giorni dei pit"* (riga 23).
- La **chiusura del giorno prima** compare negli esempi come livello dove *"c'e' un grande flusso
  di ordini"* (righe 282-284).
- **Da soli non significano nulla**: *"Devo vedere la reazione ed e' li' che avviene la conferma in
  tempo reale"* (righe 25-26).

**Contro Fabio.** Fabio *"I don't use opening and closing price. I use only the cash session"*
`[1 · 1:14:15]`, e la notte *"si osserva a parte, ma non entra nel profilo"*
([`profile-framing.md`](profile-framing.md)). Il trader usa invece chiusura, notte e ORB come
livelli. Non e' una contraddizione sul profilo (anche lui usa solo la RTH, riga 120-121), lo e' sui
livelli: **ce n'e' tre che Fabio non marca**. Sono una variante da dichiarare, non un'aggiunta
silenziosa.

### 1.2 Il profilo di volume in sviluppo, solo RTH

- Il profilo **della giornata, in sviluppo**: *"cambia continuamente, ma puoi posizionarti"*
  (righe 30-32). Solo RTH, non dall'apertura del globex (righe 120-121).
- **Area del valore** = 70% del volume: *"sia gli acquirenti che i venditori sono d'accordo sul
  prezzo [...] valore equo"* (righe 33-35).
- **Nodi a basso volume (LVN)**: *"una delle due parti diventa aggressiva. Questo crea delle sacche di
  volume tra le aree di valore"* (righe 35-39). *"Quando ci sono giornate di trend, si vedono molti
  nodi a basso volume"* (righe 39-40).
- **Il tipo di giornata si legge da qui**: valore stabile = equilibrio, si giocano i bordi; molti
  LVN = tendenza, si cercano i ritracciamenti verso il bordo o verso l'LVN (righe 40-44).
- **Una coda non e' un LVN.** All'estremo del profilo la mancanza di volume e' una *coda*: *"Questi
  non sono nodi significativi a basso volume. Questo e' quello che chiamerei una coda di vendita"*
  (righe 269-271).
- **La migrazione del valore, durante la giornata**: *"nel corso della giornata, quest'area di
  valore si spostera' [...] non credo che molte persone lo facciano ed e' un grande vantaggio se lo
  osservi ogni giorno"* (righe 126-128); *"questo e' valore in via di sviluppo [...] Perche' questo
  cambia sempre. Ed e' [...] una parte importante del mio vantaggio"* (righe 400-401).
- Prima dell'apertura, il profilo **della notte** (righe 303-304, il caso del payroll; righe
  443-444, la preparazione dal vivo).

**Con Fabio.** Il valore come accordo, la gaussiana per seduta e il suo spostamento sono il profile
framing `[1 · 22:13 - 24:08]`; la liquidita' sottile che si ribilancia `[5 · 16:07]` e' il LVN.
**Nuovo**: Fabio guarda lo spostamento **fra sedute**; il trader lo guarda **dentro la seduta**, a
ogni blocco. Il workspace oggi lo fa solo fra sedute (tab 2, `valore SU / GIU`).

### 1.3 I big trades

- Filtro **75 lotti su NQ, 200 su ES** (righe 52-53); *"quando il book e' molto sottile [...] devo
  regolare questi valori"*: 50 ad aprile, poi 100 (righe 53-56).
- **Ordini aggregati a 100 millisecondi**, che si vedono solo dal vivo e non in replay (righe
  191-193).
- Serve una piattaforma con dati tick: *"TradingView non ha i dati tick e non si ottengono valori
  reali"* (righe 47-48).
- **Il big trade non dice l'intento**: *"Questo e' un acquisto. Non conosco l'intento di questo
  ordine. Potrebbe essere un venditore che esce"* (righe 273-275). Lo dice il prezzo dopo.

**Con Fabio.** Stessa idea, soglia diversa: Fabio 60 su NY, 20-30 in premarket `[5 · 6:04]`.
Il workspace salvato ha `BigTrades MinVol 50` sulla seduta M1, non 60 come dice
[`il-workspace-a-tab.md`](il-workspace-a-tab.md): una discrepanza da guardare, non toccata qui.

### 1.4 Il profilo del delta, che riparte ogni 30 minuti

- Delta = scambi sull'ask meno scambi sul bid, prezzo per prezzo (righe 56-60).
- **Riparte ogni 30 minuti**: *"quando uso il profilo delta, si resetta ogni 30 minuti [...] mi da'
  un'idea di cosa sta succedendo in quell'intervallo"* (righe 253-254). Sul DOM invece guarda il
  delta dell'intera sessione (righe 254-255).
- **Cosa ci cerca: assorbimento e intrappolati.** *"Si vede molto delta negativo a un livello di
  prezzo. Il prezzo non si muove. [...] c'e' un acquirente passivo che consuma tutti quegli ordini
  sul bid"* (righe 61-64). Un esempio numerico: delta positivi accumulati vicino ai massimi, *"ma il
  prezzo non va da nessuna parte. [...] C'e' un venditore passivo"* (righe 255-257).
- Ha **sostituito footprint e bookmap**: *"Ora non uso nemmeno piu' niente di tutto cio' perche'
  questo riepilogo sul delta mi dice [...] se c'e' un acquirente o un venditore passivo"* (righe
  360-362).

**Con Fabio.** L'assorbimento e' identico: sforzo senza risultato `[4 · 12:35]`, il livello ad alto
delta `[5 · 1:04:56]`. **Nuovo**: la finestra di 30 minuti. Fabio misura il delta su un profilo
fisso da A a B (la gamba), noi nel bridge sul tratto di seduta.

### 1.5 La confluenza: almeno due su quattro

> *"non ho bisogno che siano soddisfatti tutti e quattro i criteri. Ne ho bisogno di almeno due per
> creare una zona di supporto e resistenza. [...] Se il prezzo non torna piu', indovinate un po'?
> Non faccio trading."* (righe 67-68)

> *"Tutti e quattro. Questa e' una configurazione A+."* (riga 218)

> *"uso solo questi quattro criteri per ottenere questi livelli. Non mi credono."* (righe 369-370)

E le zone **nascono anche dopo**: *"non c'e' niente che dica che queste aree non si formeranno piu'
in alto o piu' in basso"* (righe 69-70). L'esempio del 15 gennaio: minimo dell'opening range, big
trades e acquirenti intrappolati, *"due criteri [...] Non e' una configurazione A+, ma vedrai perche'
lo diventa"* (righe 366-367).

**Con Fabio.** Le quattro strade per qualificare un'area `[2 · 1:32:32 - 1:38:02]` sono la stessa
famiglia di idee (framing, big trades, delta profile, origine del movimento). **Nuovo**: la regola
del conteggio, *almeno due*. E' la parte piu' utile del transcript per noi, perche' e' l'unica
**misurabile senza interpretazione**.

---

## 2. I Due Modelli

### 2.1 Modello uno: equilibrio (range)

- **Quando**: il prezzo e' bloccato in un intervallo (righe 80-81). *"Mi impedisce di operare al
  centro"* (righe 81-82).
- **Dove**: ai bordi del valore. *"Non mi interessa la parte centrale"* (riga 86).
- **Cosa aspetta**: assorbimento, o acquirenti aggressivi *"ma non c'e' molto seguito"* (righe
  86-87); oppure la **falsa rottura**: *"guarda sopra e fallisce"* (righe 133-134, 418).
- **Bersagli**: TP1 il punto medio, TP2 l'altro bordo del valore (righe 94-97).
- **Non compra le rotture, le vende**: *"Non mi piace fare trading sui breakout [...] Voglio vendere
  alle persone che diventano aggressive"* (righe 95-97).

**Con Fabio.** E' il modello mean reverting `[1 · 18:05]`. **Differenza sul bersaglio**: Fabio dentro
la balance punta al POC `[1 · 1:22:45]` e all'altro bordo solo col framing allineato `[1 · 19:05]`;
il trader punta al centro e poi all'altro bordo, senza la condizione del framing.

### 2.2 Modello due: tendenza

- **Quando**: un valore si forma, rompe, lascia un LVN e si forma un nuovo cluster piu' in alto:
  *"gli acquirenti hanno chiaramente vinto qui e stanno iniziando ad accettare prezzi piu' alti"*
  (righe 123-125).
- **Dove**: il ritracciamento **dentro l'LVN** (righe 130-131), non la bandiera delle candele (righe
  129-130).
- **Cosa aspetta**: *"grandi scambi, molti venditori che rimangono intrappolati, molti ordini che
  raggiungono il prezzo di acquisto e poi il prezzo non scende"* (righe 131-132).
- **Bersagli**: i massimi, *"perche' di solito molti trader di breakout cercheranno di entrare li'.
  Glielo vendero'"* (righe 145-147).
- **Uscita**: *"Non voglio vedere formarsi un massimo inferiore [...] E se quel massimo inferiore si
  formasse con grandi scambi, sarei completamente fuori"* (righe 155-156).
- **Il doppio carburante**: in tendenza forte i ritracciamenti sono corti perche' il lato opposto
  viene messo sotto pressione: *"si ottiene il doppio del carburante per salire"* (righe 139-142).

**Con Fabio.** E' il trend following con il *reload* `[4 · 28:27]` e il *fade del ritracciamento*
`[4 · 7:16]`. **Contraddizione**: il trader esce sul massimo inferiore a 2 minuti; Fabio *"the
structure can be really fake"* `[4 · 46:19]` e mette il break even su un livello, non sulla
struttura. Comanda Fabio.

**Nota**: Fabio ha tre modelli (balance, momentum, trend), il trader due, e **non opera il
momentum**: *"non facevo trading sui breakout quando ho iniziato, e' cosi' che ho perso piu' soldi"*
(righe 388-389). Fabio il momentum lo opera, con la speed of tape `[1 · 55:05]`.

---

## 3. L'Esecuzione E La Gestione

| | il trader | Fabio |
|---|---|---|
| **timeframe** | NQ a **2 minuti**, ES a 3 (righe 105-106); l'M1 *"stavo solo guardando il rumore"* (righe 351-353) | **M1 primario** `[1 · 1:24:04]`, range 40 quando e' choppy |
| **struttura delle candele** | si', per massimi e minimi crescenti e per gli stop (righe 106, 353-354) | no `[4 · 46:19]` |
| **velocita' del nastro** | sul DOM, per **non** entrare contro il momento: *"e' come mettersi davanti a un treno in corsa"* (righe 249-252); *"non andare mai contro lo slancio"* (righe 394-396) | stessa funzione: *"it's keeping you out"* `[1 · 57:47]` |
| **ingresso aggressivo** | size molto piu' piccola e stop stretto (righe 87-89) | size dimezzata sul mean reverting `[5 · 1:25:41]` |
| **aggiungere** | *"Dovresti mediare al rialzo, incrementando una posizione vincente"* dopo la conferma (righe 89-93) | non risulta nei documenti del live |
| **break even** | subito dopo il TP1 in giornata di chop (righe 97-100); *"non lasciare mai che un'operazione in posizione vincente vada in rosso"* (riga 100) | sul **livello**, non dopo N punti `[1 · 2:11:58]` |
| **parziali in tendenza** | taglia 1-2 contratti su 5, poi pareggio (righe 152-153) | parziali sui livelli orizzontali (`la-gestione-della-posizione.md`) |
| **R** | ingresso aggressivo 3-4 R, conferma 2 R (riga 149) | 1:2-1:3 sul momentum |
| **orari** | prime 1-2 ore; i primi 30 minuti fuori (righe 157-158, 466-467, 476-477) | aspetta 30 minuti la stabilita' `[3 · 22:42]` |
| **numero di operazioni** | due o tre (righe 286, 421) | non dichiarato come tetto |
| **notizie** | CPI e payroll muovono la notte: li' guarda i big trades anche in premarket (righe 301-302) | opera anche in premarket con soglia piu' bassa `[5 · 3:34]` |
| **NQ** | *"ama intrappolare"* (righe 380, 388) | lo squeeze degli intrappolati `[3 · 13:50]` |

**Supporto che diventa resistenza, e perche'.** *"L'assorbimento intrappola acquirenti e venditori.
Questo e' cio' che crea questi livelli di supporto e resistenza"* (righe 321-322); nell'esempio
del payroll, il livello dell'assorbimento in premarket diventa la resistenza della RTH (righe
327-338). E' il livello ad alto delta di Fabio, con i suoi due mestieri `[5 · 1:05:20]`.

**La mentalita'.** Accettare il rischio a inizio giornata (righe 210-211, 416-417); se l'area non
viene testata, non si opera (righe 68, 478); niente impulso: *"Ci devono essere dei criteri"* (riga
396); lo stato mentale e il sonno (righe 384-386). Coincide con le tre regole mentali di Fabio
`[2 · 11:47 - 14:24]`.

**La preparazione dal vivo** (righe 438-453): profilo settimanale e profilo della notte, i livelli
del timeframe piu' ampio (il VAL della RTH dentro un gap, riga 442-443; il VAH di ieri, riga 452),
due scenari scritti prima dell'apertura (rottura e ritest, oppure *"guarda sotto e recupera"*,
righe 447-449). E' lo stesso ordine del nostro studio globale: dal lontano al vicino.

---

## 4. Tutto Cio' Che Ne Prendiamo, In Un Elenco

Ognuno con la sua riga, e con cosa e' rispetto a Fabio: **=** ripete, **+** aggiunge, **≠**
contraddice.

| | cosa | righe | Fabio |
|---|---|---|---|
| 1 | una zona richiede **almeno due criteri indipendenti** sullo stesso prezzo | 67-68, 364 | + |
| 2 | **delta per prezzo a blocchi di 30 minuti** | 253-254 | + |
| 3 | **migrazione del valore dentro la seduta** come lettura del lato forte | 126-128, 400-402 | + |
| 4 | livelli del mercato: PDH, PDL, PDC, ON high/low, ORB 30 | 21-23, 282-284 | ≠ (Fabio non usa apertura e chiusura) |
| 5 | **LVN interno contro coda**: due cose diverse | 35-39, 269-271 | + (precisa la liquidita' sottile `[5 · 16:07]`) |
| 6 | il tipo di giornata dal profilo in sviluppo: valore stabile o LVN multipli | 39-44 | = (il regime) |
| 7 | **guarda sopra e fallisce**: rottura di un estremo con delta a favore, e rientro | 133-134, 260-266, 418 | = (failed auction `[3 · 19:22]`) |
| 8 | assorbimento: molto delta a un prezzo, prezzo fermo | 61-64, 255-257, 306-307 | = |
| 9 | gli estremi della notte sono dove molti restano intrappolati | 265-267 | + (per noi sono solo livelli di regola) |
| 10 | il big trade non dice l'intento: lo dice il prezzo dopo | 273-275 | = (delta accoppiato al risultato `[2 · 2:10:31]`) |
| 11 | non entrare quando il prezzo arriva sul livello in velocita' | 250-252, 370-371, 394-396 | = |
| 12 | soglia dei big trades che si **abbassa col libro sottile** | 53-56 | = (Fabio la abbassa in premarket) |
| 13 | size piu' piccola sull'ingresso aggressivo; aggiungere solo a conferma | 87-93 | + (aggiungere) |
| 14 | break even dopo il primo bersaglio nel chop | 97-100 | ≠ (Fabio: sul livello) |
| 15 | il supporto nato da un assorbimento diventa resistenza quando si rompe | 321-338 | = |
| 16 | prime 1-2 ore, primi 30 minuti fuori, due o tre operazioni | 157-158, 286, 466-467 | = (in parte) |
| 17 | due scenari scritti prima dell'apertura, dal timeframe ampio | 438-453 | = (lo studio globale) |
| 18 | il timeframe si sceglie per togliere rumore, non per moda | 351-357 | ≠ (Fabio M1) |

**Cosa non si prende, e perche'.**

- **"4 su 4 e' A+"** come giudizio di qualita': nessuna misura lo sostiene nel transcript. Diventa
  un'**ipotesi** (le zone con piu' criteri reagiscono di piu'?) da provare contro un nullo.
- **I numeri delle operazioni** (rischi 15, guadagni 60; 1,7 R; 6.000 dollari su cinque conti):
  esempi scelti a posteriori, senza le operazioni perse. Regola 3 del metodo: un risultato senza
  nullo non e' un risultato.
- **La regola di coerenza della prop firm** come motivo per uscire (righe 240-243): e' un vincolo del
  contratto, non del mercato.
- **"Succede ogni volta"** (righe 347, 387): e' la frase che il metodo esiste per verificare.
- Sponsor, pagamenti, polemica con ICT (righe 71-80, 173-190, 348).

---

## 5. Cosa Abbiamo Gia', E Cosa Fa ATAS Da Solo

Prima di scrivere codice, cosa del metodo esiste gia' nel workspace o fra gli indicatori nativi di
ATAS X (verificato con reflection su `C:\ATASX`, 27 ottobre 2026).

| pezzo del metodo | gia' coperto da | impostazioni |
|---|---|---|
| PDH, PDL, PDC | nativo **DailyLines** (`ATAS.Indicators.Technical`) | `Period = PreviousDay`, `CustomSession = true`, `StartTime`/`EndTime` = la RTH; `HighPen`, `LowPen`, `ClosePen` accese. **Il fuso delle ore della sessione personalizzata va verificato sul chart** |
| ORB 30 | nativo **InitialBalance** | `CustomSessionStart = true`, `StartDate` = apertura RTH, `PeriodMode = Minutes`, `Period = 30`, `ShowOpenRange = true` |
| VAH, POC, VAL della seduta | **Fabio Data Bridge** sul ruolo seduta (zone `NY oggi`, `PD CASH`); nativo **DynamicLevels** (`Type = Volume`, `PeriodFrame = Daily`) ma sulle 24 ore, non sulla RTH | |
| big trades a 75 | nativo **BigTrades** (`ATAS.Indicators.Other`), gia' sulla tab M1 a 50 | `MinVol = 75`, `CalculationMode = CumulativeTrades` (l'ordine aggressivo intero, l'equivalente piu' vicino dei 100 ms del trader) |
| delta per prezzo ogni 30 minuti, da guardare | il **footprint del chart M30** (tab 2) in modalita' delta: ogni candela M30 *e'* il profilo del delta del suo blocco | modalita' del cluster del chart: Delta, o Volume + Delta |
| delta della sessione | nativo **ClusterStatistic** (`ShowSessionDelta`), o il DOM | |
| massimi e minimi a 2-3 minuti | nativo **SwingHighLow** | `Period` 2-3 |
| velocita' del nastro | nativo **SpeedOfTape**, gia' sulla tab M1 | |
| assorbimento su un prezzo | i **muri** del Data Bridge (quattro prove) | |
| LVN | `GliLvnIncrociati` e `INodiEIVuoti` del Data Bridge, **spenti** dal 9 ottobre | |

**Cosa manca.** L'indicatore TraderYush oggi copre solo il punto 4 e i livelli di ieri e dei
primi 30 minuti in un posto solo, con le etichette esplicite; il resto e' sviluppo possibile (6.5):

1. **il conteggio della confluenza**: nessuno strumento dice *quante fonti diverse* cadono sullo
   stesso prezzo;
2. **i picchi del delta di ogni blocco di 30 minuti come livelli**, marcati "senza risultato" quando
   il blocco chiude dalla parte opposta allo sforzo;
3. **la migrazione del valore della RTH dentro la seduta**, a gradini;
4. **massimo e minimo della notte**, che il Data Bridge disegna solo se una regola li chiede;
5. **gli LVN interni del profilo di oggi**, distinti dalle code;
6. **le rotture rientrate** sui livelli del mercato, con il delta della barra.

---

## 6. L'Indicatore TraderYush

**Nome in ATAS**: `TraderYush`. **File**:
[`FabioOrderFlow/src/Observation/TraderYush.cs`](../../../FabioOrderFlow/src/Observation/TraderYush.cs),
classe `TraderYush`, compilato nella stessa DLL del Data Bridge (`FabioDataBridge.csproj`), quindi
lo porta lo stesso `deploy.sh`. E' una classe a se', **non** un ruolo del Data Bridge: non apre il
listener HTTP e non tocca regole, muri o pannello dell'M1.

**Storia.** Il 27 ottobre 2026 e' nato come indicatore delle zone di confluenza (conteggio dei
criteri, big trades, delta a blocchi, migrazione del valore, rotture rientrate, pannello). Lo stesso
giorno l'utente ha chiesto: *"vorrei che semplicemente avesse il nome di quel trader e che
semplicemente per ora disegnasse solo i livelli, con delle rette e dei nomi espliciti tipo
NOME(SIGLA): PREZZO"*. Quella prima versione e' stata tolta; cosa faceva resta in 6.5 come
sviluppo possibile.

### 6.1 Cosa disegna

Una **retta orizzontale** per livello e un'**etichetta** a destra, sopra la retta, nel formato
`NOME (SIGLA): PREZZO`, col prezzo sul tick dello strumento e in formato italiano, per esempio
`Chiusura di ieri (PDC): 7.712,50`. Le sigle sono quelle standard inglesi; per i primi 30 minuti si
usa **ORH / ORL** (opening range high/low), perche' il trader lo chiama "orb" (riga 23), non IB.

| famiglia | etichetta | sigla | fonte nel transcript | la retta parte da |
|---|---|---|---|---|
| ieri (azzurro) | Massimo di ieri | PDH | righe 21-22: *"il massimo del giorno precedente [...] solo RTH"* | inizio della notte |
| | Minimo di ieri | PDL | righe 21-22 | inizio della notte |
| | Chiusura di ieri | PDC | righe 282-284: *"proprio alla chiusura del giorno precedente"* | inizio della notte |
| valore di ieri (rosa, tratteggiato) | Massimo del valore di ieri | PDVAH | riga 452: *"il massimo dell'area di valore del giorno precedente"* | inizio della notte |
| | Minimo del valore di ieri | PDVAL | righe 197-198: i bordi del valore come rette rosa tratteggiate | inizio della notte |
| notte (viola) | Massimo della notte | ONH | righe 22-23: *"dalle 21:00 EST alle 9:29 EST"* | prima barra della notte |
| | Minimo della notte | ONL | righe 22-23; riga 199, *"la linea viola tratteggiata [...] e' il minimo della notte"* | prima barra della notte |
| cassa di oggi (ambra) | Apertura della cassa | RTHO | riga 260: *"formando valore sopra quel prezzo di apertura"* | prima barra della cassa |
| | Massimo dei primi 30 minuti | ORH | righe 23, 473: *"orb high"*, *"orbita bassa, 30"* | fine dei primi 30 minuti |
| | Minimo dei primi 30 minuti | ORL | righe 23, 366: *"un minimo di apertura range"* | fine dei primi 30 minuti |

- **Ieri** e' la cassa precedente (09:30-16:00 ET), cercata fino a 6 giorni indietro per i ponti.
- **Il valore di ieri** e' il 70% del volume della cassa di ieri attorno al POC, prezzo per prezzo
  dal footprint. **Il POC non si disegna**: nel transcript non compare come livello.
- **La notte** si aggiorna barra per barra fino alle 9:29; la retta sta al valore di adesso anche
  sul tratto di notte gia' passato.
- **ORH e ORL compaiono solo a range chiuso**: un range in formazione e' un estremo qualunque.
- Si calcola sulle **barre chiuse**, una volta per barra nuova, come il Data Bridge: l'apertura
  compare alla chiusura della prima barra della cassa.
- Solo i livelli **della sessione corrente**: dalle 21:00 ET si lavora gia' per la cassa di
  domani. I giorni passati non si ridisegnano.

### 6.2 Gli orari

La cassa **09:30-16:00** e la notte **dalle 21:00** si leggono **nel fuso di New York** e si
convertono in UTC **per data** con la classe comune delle sedute,
[`LeSedute.cs`](../../../FabioOrderFlow/src/Observation/LeSedute.cs) (`Orario`, `Momento`,
`GiornoDiMercato`): 09:30 ET e' 13:30Z con l'ora legale e 14:30Z dal 2 novembre 2026. Le
impostazioni usano la sua sintassi: `21:00NY`, `09:30NY`, `16:00NY`. **Un orario senza suffisso e'
UTC**, non New York. La regola e il perche' stanno nel commento di `LeSedute.cs`, che rimanda a `gli-orari-si-scrivono-nel-fuso-del-mercato.md` (in scrittura il 27 ottobre).

### 6.3 Le impostazioni

| gruppo | impostazione | default | da dove |
|---|---|---|---|
| 1. Orari New York | Notte da | 21:00NY | righe 22-23. **Da decidere**: il globex apre alle 18:00 |
| | Cassa da / a | 09:30NY / 16:00NY | riga 28 |
| | Primi minuti (ORH, ORL) | 30 | righe 23 e 473, trascrizione incerta |
| 2-5. una per famiglia | acceso/spento, colore, spessore | ieri azzurro 2; valore di ieri rosa 1 tratteggiato; notte viola 2; cassa di oggi ambra 2, con apertura e primi minuti accendibili a parte | rosa e viola sono i colori dei grafici del trader (righe 197-199) |
| 6. Disegno | Font, distanza dalla scala dei prezzi | 9, 10 px | |

Le etichette vicine non si sovrappongono: quella di sotto scende.

### 6.4 Cosa NON fa

- **Non dice LONG o SHORT**, non avvisa, non scatta. I livelli *"di per se', non significano
  nulla"* (righe 25-26): la conferma si legge sul footprint, su BigTrades e su SpeedOfTape.
- **Non e' una regola approvata**: e' la parte "livelli del mercato" di un'idea che, per diventare
  regola, deve passare la pipeline del metodo quantitativo.
- **Non distingue NQ da ES**: i livelli sono gli stessi, cambia solo il tick dell'etichetta.

### 6.5 Rimandato: sviluppi possibili

Tolti il 27 ottobre su richiesta, descritti qui per non perderli. Ciascuno si rifa' solo su
richiesta esplicita.

1. **La confluenza contata** (1.5): i candidati di quattro famiglie (livelli, profilo, big trades,
   delta) raggruppati per prezzo con una tolleranza (era 4 punti, nostra); zona se ci cadono almeno
   2 famiglie diverse (riga 67), nata quando l'ultima famiglia diventa nota (niente look-ahead), con
   etichetta `n/4`, che e' un conteggio, non un giudizio.
2. **Il profilo della cassa in sviluppo**: VAH, POC, VAL di oggi e gli LVN interni distinti dalle
   code (righe 35-39, 269-271; soglie nostre: fascia 5 punti, vuoto sotto 0,25 x mediana).
3. **I big trades** (1.3) dalla notte in fasce di un punto, con il registro del tape che dice da
   quando copre (75 lotti; semina storica di 26 ore).
4. **I picchi del delta a blocchi di 30 minuti** (1.4), marcati "senza risultato" se il blocco
   chiude dalla parte opposta allo sforzo (soglia nostra 300).
5. **La migrazione del valore** dentro la seduta, a gradini di 30 minuti, con `valore SU / GIU /
   fermo` (1.2).
6. **Le rotture rientrate** sui livelli (*"guarda sopra e fallisce"*, righe 133-134, 418), con il
   delta della barra (soglia nostra 200).
7. **Il pannello** in alto a sinistra con le finestre, la copertura del tape e le zone piu' vicine.

### 6.6 Come si mette sulla tab M3

1. `deploy.sh` (lo fa l'orchestratore: ricarica i chart).
2. Sulla tab M3 si aggiunge **TraderYush**.
3. **Il Data Bridge sulla M3**: se lo si aggiunge anche li', il ruolo automatico di un M3 e'
   **seduta** (da M1 a M10, `IlRuoloDelChart.cs`), che rifarebbe tape, muri e regole e potrebbe
   contendere all'M1 il selettore per strumento. Va forzato `Role = Esecuzione`, o lasciato fuori.

---

## 7. Cosa Resta Da Decidere

1. **La notte**: 21:00 ET come dice il trader, o 18:00 ET (apertura del globex)?
2. **Il timeframe**: il trader usa **2 minuti su NQ** e 3 su ES (righe 105-106). La tab e' M3.
3. **Le sigle dei primi 30 minuti**: ORH/ORL (scelte, come lo chiama il trader) o IBH/IBL.
4. **Il valore di ieri**: acceso di default (riga 452), o solo i livelli delle righe 21-23; e se
   aggiungere il POC, che il transcript non cita come livello.
5. **Quali sviluppi di 6.5 rifare**, e in che ordine: la confluenza contata e' l'unica misurabile
   senza interpretazione.
6. **Se farne un'idea della pipeline**: una scheda in `quant/studi/<idea>/1-idea.md` con
   l'ipotesi *"il prezzo reagisce sui livelli del trader (e sulle zone con 2+ criteri) piu' che su
   prezzi casuali alla stessa distanza"*, il nullo e le varianti (finestra della notte, livelli
   che Fabio non usa si/no).
7. **Il Data Bridge sulla M3**: niente, o forzato a Esecuzione.

---

## Documenti Collegati

- [`i-tre-modelli-del-live-q1.md`](i-tre-modelli-del-live-q1.md) — la fonte che comanda.
- [`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md) — il motore delle
  regole, i muri, le difese che questo indicatore ricopia (barra chiusa, copertura del tape).
- [`il-workspace-a-tab.md`](il-workspace-a-tab.md) — le tab e i ruoli.
- [`gli-lvn-incrociati.md`](gli-lvn-incrociati.md) — lo studio degli LVN, spento.
- [`../build-e-deploy.md`](../build-e-deploy.md) — come arriva su ATAS.

## Le Scelte Dell'Indicatore, Dal Transcript (27 Ottobre 2026)

Rilette sul transcript intero (righe del file piegato a 200 caratteri), su richiesta dell'utente.

| domanda | risposta | dove |
|---|---|---|
| da quando conta la notte | **21:00 di New York fino alle 9:29**, come dice lui; il parametro resta per le 18:00 del globex. Alla riga 200 chiama il range della notte "quello impostato con il Globex": la parola non basta a cambiare l'orario detto in chiaro | righe 22-23, 200 |
| i primi minuti | **ORH / ORL, 30 minuti**: lui dice "orb high e orb bottom" e "orb 30"; nella diretta "i primi 30 minuti di solito resto fuori" e "consolidarci nei primi 30 minuti" | righe 23, 373, 467-468, 473 |
| il valore di ieri | **acceso**: il VAH di ieri e' un pivot del suo piano pre-mercato | riga 452 |
| il valore di oggi | **aggiunto e acceso** (VAH, VAL, rosa tratteggiato): e' il centro del suo metodo, il profilo RTH in formazione che si sposta durante la seduta, e il modello uno opera ai suoi bordi | righe 30-33, 84-86, 120-128, 197, 399-402 |
| il POC | **no**: non lo nomina mai; il "punto medio" che usa come primo obiettivo e' il centro del range o del valore, non il POC | righe 95, 280, 284 |
| la notte tratteggiata | **si'**: "la linea viola tratteggiata" | riga 199 |
| il timeframe | **2 minuti su NQ, 3 minuti su ES**: la tab M3 e' quella di ES; su NQ servirebbe una M2. L'M1 l'ha abbandonato per il rumore | righe 105-106, 351-353 |
| i big trades (non sono livelli, non nell'indicatore) | 75 lotti su NQ, 200 su ES, 50-100 con liquidita' scarsa; ordini aggregati a 100 ms | righe 52-56, 191-192 |
| il Data Bridge sulla M3 | **fuori**: TraderYush calcola da se'; un secondo Data Bridge su un M3 prenderebbe il ruolo di seduta e rifarebbe il lavoro dell'M1 (scelta nostra, non del transcript) | — |

Non disegnati, ma suoi: il profilo RTH settimanale e i suoi nodi a basso volume (righe 441-445), il delta
per prezzo che riparte ogni 30 minuti (righe 253-254), i livelli nati da un assorbimento (righe 321-338).
