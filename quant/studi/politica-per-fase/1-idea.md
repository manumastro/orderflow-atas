# La Politica Per Fase: L'Incrocio Dei Tre Paper Sulle Prop Firm

**Stato**: scheda e criteri sigillati scritti il 14 ottobre 2026, **prima** di qualunque simulazione
della politica. Provata lo stesso giorno: **l'incrocio non regge come scritto** (K1 e K4 smentiti,
K2 e K3 passano). [Risultati](5-validazione/risultati/2026-10-14-la-politica-per-fase.md),
[esito dei criteri](5-validazione/criteri/2026-10-14-la-politica-per-fase-esito.md).
**Strategia nel catalogo**: nessuna; le politiche sono nel catalogo dei contratti (`quant contratto ... --politica`).

## Da Dove Viene: L'Incrocio

La metodologia: il paper non si copia, se ne prendono i *findings* e **si combinano gli elementi in
una strategia** `[MC · 1:06:18 - 1:06:32]`; nel PEAD tre paper diventano tre strati della stessa
regola (la reazione del prezzo, i 60 giorni, gli analisti) `[MC · 1:06:32 - 1:07:13]`. "Collegare i
punti" `[MC · 1:15:28 - 1:16:33]`.

Qui i punti sono i tre paper dello studio [`contratto-prop`](../contratto-prop/1-idea.md), e un
nostro risultato che nessuno dei tre ha:

| da | finding | come entra nell'incrocio |
|---|---|---|
| **TF** `[TF §2.4]` | la dimensione e' l'unica leva di chi non ha vantaggio, e con il pavimento aggiornato alla chiusura **operare grande alza P** | strato 1: grande in valutazione |
| **L** `[L §6.2 - 6.3]` | il valore dipende dalla dimensione piu' che dall'abilita'; le regole si misurano **una alla volta** | strato 2: la dimensione si sceglie **per fase**, e ogni strato si misura da solo |
| **Y** `[Y §3, §4.1]` | con una dimensione fissa e un **bracket giornaliero** (target e stop) alcune regole diventano **irrilevanti** | strato 3: nel finanziato, un bracket che rende irrilevanti la consistenza del 35% e il limite giornaliero |
| **noi** | su Tradeify Growth 50K **passare premia la dimensione grande, prelevare la piccola** ([studio](../contratto-prop/5-validazione/risultati/2026-10-11-tradeify-growth-50k.md#la-dimensione)) | e' la tensione che l'incrocio prova a sciogliere |

Nessuno dei tre paper cambia condotta fra valutazione e finanziato: TF e L tengono la stessa
deviazione per tutto il contratto, Y la stessa strategia.

## Il Finding Da Provare

**Una politica che cambia condotta fra le fasi** (grande in valutazione, piccola o con un bracket nel
finanziato) **vale di piu' della migliore dimensione fissa**, per un compratore senza vantaggio, su
Tradeify Growth 50K con le regole dell'11 ottobre 2026.

## Il Perche'

- In valutazione il pavimento si aggiorna solo alla chiusura: una giornata larga sale e scende
  senza trascinarlo, e la barriera effettiva si allontana `[TF §2.4]`. Il limite giornaliero morbido
  di 1.250 $ fa da stop.
- Nel finanziato il prelievo chiede 5 giornate sopra 150 $ e che la giornata migliore non superi il
  35% del profitto del periodo. Le giornate grandi lo impediscono; un target giornaliero le taglia
  `[Y §3]`.
- Il contratto e' un'opzione: la perdita e' limitata al costo dei conti `[Y §4.1]`, `[L §3]`.

## Cosa Lo Ucciderebbe

- **La politica non batte la dimensione fissa** oltre l'errore Monte Carlo: l'incrocio non aggiunge
  niente ai singoli strati.
- **Il vantaggio dipende dall'uscita esatta al limite giornaliero**: senza limite (o con uno
  slittamento oltre) sparisce. Nello studio del contratto e' gia' successo alla dimensione.
- **Il vantaggio sparisce se il conto finisce al primo prelievo**: la firma decide quando passarti al
  vivo `[TF §6.1]`.
- **I costi**: operare grande costa di piu' (i costi crescono con i contratti).
- Tutto cio' che uccide lo studio del contratto: le regole cambiano `[TF §6]`, e un vantaggio
  negativo annulla il valore `[TF §5.6]`.

## Le Regole

**Il riferimento**: sessioni browniane, nessun vantaggio, d = 850 $ per sessione (circa 2 MNQ tenuti
tutta la seduta, il trader di TF), 78 controlli per sessione. Una politica dice, per ciascuna fase:

- **scala**: quanto e' grande la posizione rispetto al riferimento (scala 2 = d 1.700 $, circa 4 MNQ);
- **stop del giorno**: quando il P&L della giornata tocca -stop, si chiude e si smette per la giornata;
- **target del giorno**: quando il P&L della giornata tocca +target, si chiude e si smette.

Convenzioni: stop e target si eseguono **esattamente al livello** (come l'uscita al limite
giornaliero nello studio del contratto); il target si guarda sui controlli, lo stop sui minimi del
passo; i costi crescono con la scala (la deriva dei costi si moltiplica per la scala).

**Varianti dichiarate prima di provarle** (12 politiche):

| | valutazione | finanziato |
|---|---|---|
| scale | 1, 2, 4 (d 850, 1.700, 3.400 $; 2, 4, 8 MNQ) | |
| F0 | | scala 1, niente bracket |
| F1 | | scala 0,5, niente bracket |
| F2 | | scala 1, bracket simmetrico ±400 $ (Y) |
| F3 | | scala 1, target 300 $ e stop 600 $ (molte giornate piccole) |

Le 12 politiche si chiamano `v<scala>-f<n>` (per esempio `v4-f2`). **I termini di confronto** sono
le tre dimensioni fisse, la stessa scala nelle due fasi: `fissa-1` (d 850 $, e' anche `v1-f0`),
`fissa-2` (1.700 $), `fissa-4` (3.400 $). In tutto 14 politiche.

**Scenari** per ciascuna politica: (a) nessun vantaggio, conto che dura fino a 1.000 sessioni;
(b) costi, deriva -6 $ a sessione al riferimento; (c) il conto finisce al primo prelievo; (d) finisce
al terzo; (e) senza limite giornaliero. 100.000 cicli, seme 1.

I criteri che lo smentirebbero, sigillati: [`5-validazione/criteri/2026-10-14-la-politica-per-fase.md`](5-validazione/criteri/2026-10-14-la-politica-per-fase.md),
sha256 `cf64729061496a6fcfe96cc604c77571c50da35ded853693efa12956a98c0d82`.

## Cosa Non Dice, Comunque Vada

E' un'ipotesi sul **contratto**, non sul mercato: nessun vantaggio per costruzione, nessun dato
vero, nessun cancello D o F. Sui dati veri non si misura finche' non c'e' la storia lunga di NQ (22
sedute complete sono circa tre tentativi indipendenti). Un risultato positivo resta **una lotteria
con valore atteso positivo** su regole che la firma cambia.
