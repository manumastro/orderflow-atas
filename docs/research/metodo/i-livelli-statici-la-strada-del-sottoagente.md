# I Livelli Statici: La Strada Del Sottoagente, E Perche' E' Chiusa

Stato: **strada chiusa il 20 settembre 2026**, il giorno stesso in cui era stata aperta.
Non si riapre senza richiesta esplicita.

> **IL PROBLEMA CHE QUESTO DOCUMENTO DESCRIVE NON E' PIU' APERTO.** La sera dello stesso giorno
> si e' visto che era mal posto: i livelli "fissi" erano fissi quasi solo per abitudine. Su
> quindici regole del 14 settembre, tredici erano gia' calcolate; le due restanti sono diventate
> tipi calcolati — `mensola`, e la mensola trovata dalla macchina era **piu' precisa** di quella
> scritta a mano. Il motore e' passato dentro l'indicatore, e con lui e' sparito anche il guasto
> del processo che muore. Vedi
> [`i-livelli-li-calcola-l-indicatore.md`](i-livelli-li-calcola-l-indicatore.md).
>
> Quello che segue resta come **evidenza della strada del sottoagente**, che resta chiusa.

> *"voglio togliere la questione del sottoagente che rifà i modelli, dobbiamo trovare un metodo
> migliore, l'importante è che documenti tutto"* — l'utente, 20 settembre 2026

## Il Problema Che Restava, E Resta

I livelli **vivi** si aggiornano da soli: POC, VAH, VAL, estremi sono definizioni meccaniche, e
`livelli_vivi.py` le risolve a ogni giro. I livelli **fissi** no, e non devono: sono affermazioni
dell'analisi — *questa mensola e' stata difesa sei volte*, *qui i Leveraged Funds hanno costruito
gli short* — e un programma non puo' scriverle senza classificare.

Ma qualcuno deve **accorgersi che sono scaduti**. Il prezzo esce dalla fascia per cui erano stati
derivati, un tetto diventa pavimento, la cash apre e ridefinisce la giornata: da quel momento le
righe sul chart descrivono un posto dove non siamo piu', e **si disegnano esattamente come quelle
giuste**.

Questo problema e' ancora aperto. Quello che segue e' un tentativo di risolverlo, e cosa si e'
imparato.

## Cosa Era Stato Costruito

Due pezzi separati, perche' costano in modo diverso:

```text
accorgersene    serve_rifare.py     due chiamate al bridge   -> puo' girare ogni due minuti
rifarli         sottoagente         tape, footprint, profilo -> e' il lavoro di un agente
```

- **`serve_rifare.py`** rispondeva `NIENTE` quasi sempre, e `SERVE` con le ragioni quando la mappa
  era scaduta: fuori fascia, attraversato, niente in gioco, sessione, deriva.
- **`.claude/agents/livelli-statici.md`** era un sottoagente svegliato in background solo su
  `SERVE`, con le ragioni passate verbatim.
- **`/sorveglia`** teneva insieme i due, e `/loop 2m /sorveglia` faceva girare il ciclo.

**Il filtro e' sopravvissuto** — vedi sotto. Il sottoagente e il comando no.

## Cosa Si E' Visto, Nell'Unica Prova

Va detto con precisione, perche' e' poco e non va gonfiato:

1. **Il filtro ha funzionato e aveva ragione.** Col prezzo a 29.344 e i fissi derivati per la fascia
   29.117-29.300, ha risposto `SERVE  FUORI FASCIA`. La diagnosi era corretta.
2. **Il sottoagente e' stato lanciato in background e fermato prima di produrre qualcosa.** Il file
   delle regole e' rimasto identico: nessun lavoro parziale, niente da ripulire.
3. **Il processo in background e' morto con la sessione.** Questa e' l'osservazione che pesa, ed e'
   strutturale, non un incidente: un agente in background non sopravvive alla sessione che lo ha
   lanciato. Una manutenzione che conta puo' quindi **non avvenire, in silenzio** — che e'
   esattamente il difetto per cui era stata chiusa la sorveglianza a condizioni armate.

**La decisione di chiudere e' dell'utente**, e non discende da un guasto misurato: il sottoagente
non ha avuto il tempo di fallire o riuscire. Quello che si puo' registrare e' il punto 3, piu' una
ragione di metodo che era gia' visibile:

**un sottoagente in background scrive sul chart senza che nessuno guardi.** Il resto dell'impianto
e' costruito perche' la macchina misuri e l'agente interpreti **davanti a chi opera**; un pezzo che
riscrive la mappa mentre l'attenzione e' altrove reintroduce la cosa che si voleva togliere — un
automatismo che decide.

## Cosa Resta In Piedi

**`serve_rifare.py` resta, e non sveglia piu' niente.** E' un comando che si lancia quando si vuole
sapere se la mappa regge, e risponde in una riga:

```bash
python3 FabioOrderFlow/tools/serve_rifare.py \
        docs/research/giornate/livelli-vivi-NQZ6-AAAA-MM-GG.json --chart NQZ6
```

Le sue cinque ragioni restano il **vocabolario della scadenza di una mappa**, e valgono qualunque
sia il metodo che le raccogliera':

| | cosa ha visto |
|---|---|
| `FUORI FASCIA` | il prezzo e' uscito dall'intervallo per cui i fissi erano stati derivati |
| `ATTRAVERSATO` | un fisso ha cambiato lato: il tetto e' diventato pavimento, e l'etichetta dice ancora la funzione vecchia |
| `NIENTE IN GIOCO` | nessun livello entro il raggio: si viaggia fuori dalla mappa |
| `SESSIONE` | e' passata l'apertura della cash, che ridefinisce la giornata |
| `DERIVA` | il prezzo si e' spostato di piu' della soglia da quando la mappa fu scritta |

**Il riferimento e' il prezzo di quando il file e' stato scritto**, non quello del giro precedente:
confrontando col giro prima, una deriva lenta non si vede mai — ed e' proprio il caso in cui la
mappa scade senza che nessuno se ne accorga. Lo stato sta in `~/.fabio-livelli-statici.json` ed e'
rigenerabile.

**Il filtro non giudica il mercato e non e' una condizione armata**: dice *quella riga sul chart non
descrive piu' dove siamo*, mai *compra*.

## Il Guasto Gemello, Che E' Ancora Li'

Mentre questa strada si chiudeva ne e' emerso un altro, sugli stessi livelli ma dall'altra parte.

**Quando il processo dei livelli vivi muore, le righe restano sul chart identiche a prima.** Il
20 settembre e' morto insieme alla sessione, e sul chart sono rimasti undici livelli fermi mentre
il replay andava avanti.

E' peggio di un livello fisso scaduto, per un motivo preciso: **il `~` promette che quel livello si
muove**. Un POC fermo che dice di essere vivo inganna di piu' di uno che non dichiara niente.

```text
il pannello   vive nell'indicatore, dichiara la propria eta', scrive CONTESTO FERMO   -> non puo' mentire
i livelli     vivono in un processo esterno, e non hanno nessuna difesa               -> mentono in silenzio
```

**La difesa va messa dove non muore**, cioe' nell'indicatore: se un livello vivo non viene
ridepositato entro una soglia, va marcato o spento. Non e' stato fatto.

## Tre Direzioni Per Il Metodo Migliore

Nessuna e' stata scelta. Si scrivono perche' la prossima volta si parta da qui e non da zero.

1. **Spostare la difesa nell'indicatore.** I livelli portano gia' `role`, `area` e `key`: aggiungere
   il momento del deposito e farli invecchiare a vista risolve il guasto gemello, e non dipende da
   nessun processo acceso. **Questa e' indipendente dalle altre due e serve comunque.**
2. **Rendere programmatici anche alcuni fissi.** Un nodo di volume, un muro di assorbimento e una
   mensola difesa N volte sono riconoscibili dalla footprint con regole dichiarate in un file, come
   gia' si fa per i vivi. Non tutti i fissi — un prezzo del COT resta una affermazione — ma la parte
   che oggi costa un agente forse non ne ha bisogno.
3. **Rifarli in primo piano, quando si guarda comunque.** Se la mappa si rifa' mentre l'utente e'
   davanti allo schermo, non serve nessun automatismo: serve che il filtro lo dica in modo
   impossibile da ignorare — per esempio nel pannello, che e' gia' sotto gli occhi.

La terza e' la piu' vicina allo scope dichiarato il 20 settembre: **l'agente costruisce il contesto
davanti a chi opera**, non mentre nessuno guarda.
