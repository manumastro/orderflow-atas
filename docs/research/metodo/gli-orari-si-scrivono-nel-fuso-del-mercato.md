# Gli Orari Si Scrivono Nel Fuso Del Mercato

Stato: **regola**, dal 27 ottobre 2026. Vale per il codice (indicatore, bridge, strumenti, studio,
framework quantitativo), per i file delle regole dei livelli e per i documenti.

## In Parole Semplici

Ogni mercato ha il suo orologio. La borsa di New York apre alle **09:30 di New York**, sempre, estate
e inverno. Ma l'ora di New York, vista da Londra o dall'Italia o in UTC (l'ora "di riferimento"
dei computer), **si sposta di un'ora due volte l'anno**, con il cambio dell'ora legale. E i paesi non
cambiano l'ora nello stesso giorno.

Per questo **un orario di sessione non si scrive mai come un numero fisso in UTC** (per esempio
"13:30Z"). Si scrive nel fuso del suo mercato ("09:30 di New York") e il programma lo converte
**per la data di quella seduta**. Cosi' l'orario resta giusto tutto l'anno senza che nessuno debba
ricordarsi di cambiarlo.

**E' importantissimo, e tutto e' dinamico.** Un orario fisso sbagliato non da' errore: il programma
gira lo stesso e misura la cosa sbagliata — valore, POC, apertura e chiusura di un'ora prima — e il
numero sbagliato si legge esattamente come quello giusto.

## La Regola

1. **Ogni orario di sessione si scrive nel fuso del suo mercato e si converte in UTC per la data
   della seduta.** Mai un UTC fisso, mai "+2 ore" o "+6 ore" sommati a mano.
2. **Le sedute:**

   | seduta | nel suo fuso | note |
   |---|---|---|
   | cassa di New York (NQ, ES, oro) | **09:30-16:00 New York** | la "cash" |
   | pit del crude (NYMEX) | 09:00-14:30 New York | |
   | inizio del giorno di mercato CME | **17:00 Chicago = 18:00 New York** del giorno prima | la notte appartiene alla seduta dopo |
   | London | dalle **08:00 di Londra** all'apertura di New York | Londra cambia ora in un'altra data |
   | IVB | la prima mezz'ora di cassa, 09:30-10:00 New York | |

3. **Un solo punto per linguaggio definisce le sedute**, e tutto il resto lo usa: nessun orario di
   sessione ripetuto altrove.
   - **C#** (indicatore e bridge): `FabioOrderFlow/src/Observation/LeSedute.cs`.
   - **Python** (giro, strumenti, studio): `FabioOrderFlow/tools/sedute.py`.
   - **Framework quantitativo** (C#): `quant/framework/Quant.Comune/Dati/Calendario.cs`, in ora di
     New York dal primo giorno (11 ottobre 2026), con il suo test in `Quant.Test/DatiTest.cs`.
4. **Le etichette dicono l'orario vero della seduta**: "cassa 14:30Z-21:00Z" il 2 novembre, non la
   versione estiva.
5. **Nei file delle regole dei livelli** le finestre si scrivono `09:30NY`, `16:00NY`, `18:00NY`,
   `08:00LON` (anche `ET` per New York, `CT` per Chicago). `13:30Z` resta accettato, ma e' **UTC fisso** e
   sbaglia di un'ora per meta' dell'anno: in un file nuovo non si usa. Un ISO completo
   (`2026-09-13T22:00:00Z`) vale cosi' com'e'.

## Il Perche'

Fino al 27 ottobre 2026 la cassa era scritta **13:30Z-20:00Z** in tutto il codice — indicatore,
bridge, giro, collettore dello studio, COT — e il giorno di mercato cominciava alle **22:00Z**.
Erano giusti solo con l'ora legale americana.

- **New York torna all'ora solare domenica 1 novembre 2026.** Da lunedi' 2 novembre la cassa e'
  **14:30Z-21:00Z** e il giorno comincia alle 23:00Z. Con l'orario fisso ogni seduta (valore, POC,
  apertura, chiusura, le sedute del COT) si sarebbe misurata un'ora prima: la prima ora della
  "cassa" sarebbe stata premarket, e l'ultima ora vera sarebbe sparita.
- **L'Europa cambia ora in un altro giorno**, domenica 25 ottobre. Fra il 25 ottobre e il 1 novembre
  lo scarto Italia-New York e' di **5 ore** invece di 6: la cassa apre alle **14:30 italiane**, non
  alle 15:30. Chi somma una costante sbaglia proprio in quei giorni. Lo stesso vale per la London,
  che va scritta nel fuso di Londra, non in quello di New York.
- **Le sedute storiche d'inverno**: novembre-marzo e' meta' dell'anno. Un orario fisso estivo applicato
  ai dati storici sbaglia ogni seduta invernale; per questo il framework quantitativo, che lavora su
  nove anni di dati, e' in ora di New York dal primo giorno.

## Esempi

| data | cassa in New York | cassa in UTC | cassa in Italia | il giorno di mercato comincia |
|---|---|---|---|---|
| ven 23/10/2026 | 09:30-16:00 | **13:30Z-20:00Z** | 15:30-22:00 | gio 22/10 22:00Z |
| ven 30/10/2026 | 09:30-16:00 | **13:30Z-20:00Z** | **14:30-21:00** (Italia gia' in ora solare) | gio 29/10 22:00Z |
| lun 02/11/2026 | 09:30-16:00 | **14:30Z-21:00Z** | 15:30-22:00 | dom 01/11 23:00Z |
| ven 06/03/2026 | 09:30-16:00 | 14:30Z-21:00Z | 15:30-22:00 | gio 05/03 23:00Z |
| lun 09/03/2026 | 09:30-16:00 | 13:30Z-20:00Z | **14:30-21:00** (USA in legale, Italia no) | dom 08/03 22:00Z |

La London del 27/10/2026 parte alle 08:00Z (Londra in ora solare), quella del 23/10 alle 07:00Z.

Le due prove stampano la tabella e falliscono se una riga non torna:

```bash
python FabioOrderFlow/tools/sedute.py              # Python
python FabioOrderFlow/tools/sedute.py 2026-11-02   # le sedute di un giorno
```

In C# la stessa tabella (piu' London, giorno di mercato, `17:00CT`, i vecchi default ereditati) e'
stata verificata il 27 ottobre 2026 con un programma di prova che compila `LeSedute.cs` da solo:
l'indicatore non ha un progetto di test versionato, e la prova va rifatta cosi' dopo ogni modifica
a `LeSedute.cs` (un `.csproj` console con `<Compile Include=".../LeSedute.cs" />`).

## Le Cache E I Valori Salvati

- **La cache della scala dei tempi** (`~/.fabio-scala-dei-tempi-<STRUMENTO>.json`) porta `_sedute`,
  la versione della definizione. Una cache senza `_sedute` viene dal codice a ora fissa: se ne
  tengono **solo le sedute in ora legale americana**, che con 13:30Z-20:00Z e 22:00Z erano gia'
  giuste, e le altre si rimisurano (`scala_dei_tempi.migra_cache`). Ogni seduta nuova porta la sua
  `finestra`, la cassa in UTC con cui e' stata misurata.
- **Le proprieta' dell'indicatore salvate nei workspace di ATAS** possono portare i vecchi default
  (`13:30Z`, `20:00Z`, `07:00Z`; sul crude `13:00Z`, `18:30Z`). `LeSedute.Orario` li legge come la
  seduta dinamica di cui erano la versione estiva. Un altro valore scritto a mano vince, ed e'
  preso cosi' com'e'.
- **I file delle regole dei livelli di settembre** (`docs/research/giornate/regole-dei-livelli-*.json`)
  sono in Z: erano giusti per quelle date (ora legale), e restano come storia.

## Cosa Resta Fuori

- **Gli strumenti di ricerca della fase 40R** (`replay_model.py`, `probe_predictability.py`,
  `build_continuous.py`) accettano finestre da riga di comando: `09:30NY-11:30NY` si converte per
  data, un orario senza fuso resta UTC fisso come prima.
- **`livelli_vivi.py` e' ritirato** e non e' stato toccato.
- **I rapporti e i file delle giornate gia' scritti** citano gli orari del loro giorno e non si
  riscrivono.
