# Glossario Del Metodo — Dove Sta Scritta Ogni Definizione

Questo file **non definisce niente**. Dice, per ogni termine del metodo, **dove** sta la
definizione, cosi' che consultarla costi una riga di comando invece di una decisione.

Serve a rendere eseguibile l'obbligo di `CLAUDE.md`: **un termine del metodo non si parafrasa a
memoria.** Prima di usarlo in una risposta, in un `verso`, in una etichetta sul chart o in un
documento, si apre la fonte e si legge il passo.

    sed -n '139,196p' docs/research/metodo/triple-aaa-dossier.md     # Mean Reverting

## La Tabella

Le righe puntano a [`triple-aaa-dossier.md`](triple-aaa-dossier.md), che e' la trascrizione
integrale delle immagini in [`fabio_course/ivbaaa/`](../../../fabio_course/ivbaaa/) — la fonte
primaria. Quando dossier e memoria divergono, vince il dossier; quando dossier e immagine
divergono, vince l'immagine.

| termine | righe | cosa NON dare per scontato |
|---|---|---|
| **IVB** | 90-138 | e' la prima mezz'ora della cash di New York, e da' **permesso direzionale**, non un bersaglio |
| **Tier 01 · bias filter** | 90-138 | la chiusura M30 decide: sopra = solo long, sotto = solo short, **dentro = solo mean reverting**. Vale sulla cash di **New York** e su nessun'altra finestra: a Londra il gate non esiste, vedi [`la-sessione-di-londra.md`](la-sessione-di-londra.md) |
| **Tier 02·A · Mean Reverting** | 139-196 | e' **solo** il fade dei bordi della value area **verso l'interno**: short al VAH, long al VAL. Una rottura che va **fuori** dal valore non e' mean reverting |
| **risk envelope del mean reverting** | 182-185 | "rischio stretto" significa **size ridotta e massimo 2 stop-loss al giorno**, non stop vicino. Al secondo stop la sessione e' finita |
| **il trigger in tre tempi** | 190-194 | stoppino, **poi** assorbimento, **poi** flip di aggressione. **Due su tre non bastano** |
| **Tier 02·B · Triple AAA** | 197-255 | e' trend following, quindi **richiede il permesso dell'IVB**: prima delle 16:00 non e' disponibile |
| **i tre trigger del Tier 02·B** | 206-228 | break + high-delta bounce, break-and-retest, deep effort 40R |
| **Tier 02·C · Triple A+** | 360-400 | il ritracciamento attraversa tutto l'IVB fino al VAL. E' il piu' raro, e il dossier **non ne dichiara la frequenza** |
| **Tier 03 · trigger e conferma** | 256-307 | candle framing, deep trades e block-and-reload, deep effort. Sceglie **quando**, non **se** |
| **Deep Effort / 40 Range** | 222-228, 294-307 | dichiarato **solo su NQ** e **solo su grafico a 40 range** |
| **checklist di esecuzione** | 323-359 | la sequenza operativa completa, da rileggere prima di dichiarare che un setup e' valido |
| **cosa e' osservabile col bridge** | 439-457 | quali pezzi del dossier il Data Bridge puo' misurare davvero, e quali no |

## Termini Che Non Vengono Dal Dossier

Vanno definiti dove si usano, e **non vanno confusi** con i termini del metodo.

| termine | dove sta | cos'e' |
|---|---|---|
| **accettazione** | [`sorveglianza-del-tape.md`](sorveglianza-del-tape.md) | minuti chiusi da un lato piu' quota di volume, su una finestra. Nostra, non del dossier |
| **doppio POC** | file della giornata | due sedute che costruiscono valore nella stessa fascia. Nostra |
| **verso** di uno scenario | [`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md) | la conseguenza operativa. **Deve essere derivabile da una riga del dossier**, e se non lo e' il verso corretto e' `NESSUN PERMESSO` |

## L'Errore Che Ha Prodotto Questo File

Il 16 settembre ho scritto che un long sull'accettazione sopra il massimo notturno era
"mean reverting a rischio stretto". Erano due errori in una frase:

1. **Non era mean reverting.** Andava fuori dal valore, non verso l'interno: era trend following,
   che alle 07:45 non aveva nessun permesso.
2. **"Rischio stretto" non e' lo stop.** E' size ridotta e cap a 2 stop-loss giornalieri.

Nessuno dei due sarebbe successo aprendo le righe 139-196. Non li avevo aperti perche' credevo di
sapere, ed e' esattamente la condizione in cui serve il controllo.

## permesso di fatto

**Non e' un termine del corso.** E' una misura di questo repository: l'accettazione oltre un bordo
dell'IVB calcolata su barre M1 invece che sulla chiusura M30 del gate Tier 01. Definizione, soglie
e isteresi in
[`il-permesso-si-misura-non-si-aspetta.md`](il-permesso-si-misura-non-si-aspetta.md).

Sta **sotto** al dossier nella gerarchia delle fonti. Una lettura che lo usa **lo dichiara**, e non
va mai scritto come se fosse il permesso del corso: *"permesso di fatto LONG dalle 20:06, gate M30
ancora DENTRO"*, non *"permesso LONG"*.
