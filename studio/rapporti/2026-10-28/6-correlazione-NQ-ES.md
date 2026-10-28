# Correlazione NQ-ES · 28/10/2026

**Stato**: fatto, confronto di un giorno, descrittivo. Non è una fase fissa dello studio globale.
**Dati**:
- ATAS dal bridge, contratti NQZ6 ed ESZ6 (non il continuo), raccolti alle 10:43 italiane:
  - M30 dal 14/09 al 28/10: 1.459 coppie di barre, 33 giorni di mercato;
  - M1 dal 13/10 al 28/10: 15.867 coppie di minuti consecutivi.
- I due quadri del giorno: `5-quadro.md` di NQ e `../2026-10-28-ES/5-quadro.md` di ES.
- La cassa è 09:30-16:00 di New York, convertita per data da `sedute.py`. Il giorno di mercato comincia alle 18:00 di New York.
- Script di misura, nella scratchpad di sessione: `corr.py`.
- I dati 2026 dal bridge sono lettura dal vivo, non `quant/dati`: il fuori campione del framework non è toccato.

## In Parole Semplici

**Il punto**: NQ ed ES si muovono quasi sempre insieme e nello stesso minuto, ma NQ si muove una volta e
mezza tanto. Oggi però le opzioni e le posizioni li mettono in due situazioni opposte, e stanotte NQ ha
superato il massimo di ieri mentre ES no.

**Dal più ampio al più vicino**

1. **Quanto vanno insieme**:
   - correlazione 0,89 sulle mezz'ore da metà settembre, 0,92 nelle ore di New York;
   - su 416 mezz'ore di cassa solo 2 sono andate in versi opposti in modo netto.

   → Sono quasi lo stesso mercato. Una divergenza vera è rara, e per questo si nota.
2. **Quanto pesa NQ**: per ogni 1% di ES, NQ fa in media 1,42%. Anche le opzioni lo prezzano: oggi la mossa attesa è ±1,09% su NQ e ±0,72% su ES, un rapporto di 1,5. → NQ è la versione amplificata di ES.
3. **Chi guida da un mese**: il rapporto NQ/ES, cioè quante volte NQ vale ES, è sceso da 4,00 (5/10) a 3,78 (22/10), ed è risalito a 3,84 ieri e a 3,85 stamattina. → Il tech è andato peggio del resto dell'S&P fino al 22 e da tre giorni recupera.
4. **Chi anticipa**: al minuto, nessuno dei due. La correlazione fra un minuto di NQ e il minuto dopo di ES è circa zero, in tutti e due i versi. → Non c'è un "NQ si muove prima" misurabile al minuto.
5. **Le opzioni, opposte**:
   - su NQ il prezzo sta sopra lo zero gamma, quindi nel modello frenano;
   - su ES sta sotto lo zero gamma di tutte le scadenze, quindi nel modello spingono.

   → Se il modello avesse ragione, oggi ES sarebbe quello più libero di allungare, NQ quello più frenato. È un'ipotesi.
6. **Le posizioni, opposte** (COT del 20/10): gli speculatori sono lunghi su NQ quasi al massimo di tre anni, e su ES hanno pochissimi acquisti (7° percentile). → Il rischio di vendite forzate pesa più su NQ.
7. **Stanotte**:
   - NQ ha fatto +0,37% dalla chiusura di ieri ed è salito fino a 29.989, sopra il massimo di cassa di ieri (29.881);
   - ES ha fatto +0,11% e si è fermato a 7.770,25, sotto il suo massimo di ieri (7.772,75);
   - i due massimi sono arrivati nello stesso minuto, alle 09:01-09:02 italiane.

   → NQ è andato oltre, ES no. Il salto della notte lo hanno spinto i chip asiatici, che pesano su NQ e poco su ES.

**I prezzi tradotti da uno all'altro**

Il calcolo parte dalla distanza in percentuale dal prezzo delle 10:43 (NQ 29.923, ES 7.767) e la moltiplica per 1,42 (o la divide, nel verso opposto). Vale per piccoli movimenti e va ricalcolato col prezzo.

| livello di uno | cos'è | sull'altro | cosa c'è lì sull'altro |
|---|---|---|---|
| NQ 30.000 | muro delle opzioni di NQ | ES ~7.781 | bordo alto del valore di ieri e zero gamma di ES (7.772-7.784) |
| ES 7.800 | muro call di ES | NQ ~30.105 | la confluenza di NQ 30.100-30.115 |
| ES 7.745 | fine della frenata delle opzioni di stasera su ES | NQ ~29.804 | POC della cassa di ieri di NQ (29.805) |
| NQ 29.590 | zero gamma di NQ | ES ~7.706 | put wall di ES (7.700) |

→ Quattro livelli di uno cadono su un livello dell'altro. Quando entrambi i mercati sono sul "loro" prezzo nello stesso momento, è una coincidenza da guardare. Non è provato che conti.

**Quando**: 14:30 apre New York · 19:00 la Fed · 19:30 la conferenza · 21:00 scadono le opzioni di oggi · dopo le 21:00 Microsoft e Alphabet (pesano molto più su NQ che su ES).

**Cambia idea se**:
- una mezz'ora di cassa va in versi opposti in modo netto (oggi è successo 2 volte su 416);
- il rapporto NQ/ES torna sotto 3,80;
- ES supera 7.772,75, il suo massimo di ieri, e chiude la divergenza della notte.

**Quanto è provato**: correlazioni, beta e rapporto sono misurati, ma solo su sei settimane. Che una divergenza fra i due anticipi qualcosa non è provato qui. Sarebbe un'idea da scheda, `quant/studi`, con la divisione dentro/fuori campione di ES ancora da fissare.

**Le parole**:
- **correlazione** = quanto due mercati si muovono insieme, da -1 a 1. 0,9 vuol dire quasi sempre insieme;
- **beta** = di quanto si muove uno quando l'altro fa 1%;
- **rapporto NQ/ES** = quante volte NQ vale ES: se sale, il tech va meglio;
- **divergenza** = uno fa un nuovo massimo o minimo e l'altro no.

## Le Misure

| misura | valore | campione |
|---|---|---|
| correlazione dei rendimenti M30 | 0,89 | 14/09-28/10, 1.459 barre |
| solo cassa / solo notte | 0,92 / 0,87 | 416 barre di cassa |
| beta NQ su ES (M30) | 1,42 | idem |
| rapporto delle deviazioni standard in cassa | 1,46 | idem |
| correlazione M1, stesso minuto | 0,888 | 13/10-28/10, 15.867 minuti |
| NQ al minuto t, ES a t+1 / t+2 / t+3 | 0,013 / -0,013 / 0,024 | idem |
| ES al minuto t, NQ a t+1 / t+2 / t+3 | -0,004 / -0,016 / 0,028 | idem |
| correlazione del delta M1 in cassa | 0,53 | idem |
| mezz'ore di cassa in versi opposti (NQ oltre 0,05%, ES oltre 0,03%) | 2 su 416 | 14/09-28/10 |
| correlazione M30 per giorno | mediana 0,91, minimo 0,56 | 33 giorni |

**Le giornate in cui NQ si è staccato di più**:
- 22/10: NQ -2,51%, ES -0,93%;
- 16/10: NQ -2,40%, ES -1,14%;
- 14/10: NQ -2,17%, ES -1,19%;
- 23/10: NQ +0,13%, ES -0,52%, l'unico giorno con versi opposti;
- 27/10: NQ +1,05%, ES +0,50%.

I giorni sono "di mercato", dalle 18:00 di New York. Nelle discese forti NQ amplifica più di 1,42 volte: è un'osservazione su tre giorni, non una misura.

**Il rapporto NQ/ES a fine cassa**: 3,83 (14/09) → 4,00 (5/10, massimo) → 3,78 (22/10, minimo) → 3,84 (27/10) → 3,853 alle 10:43 di oggi.

**Stanotte**, dalle 18:00 di New York alle 10:43 italiane:

| | NQZ6 | ESZ6 |
|---|---|---|
| chiusura della cassa di ieri | 29.813,25 | 7.758,25 |
| massimo della cassa di ieri | 29.881,25 | 7.772,75 |
| massimo della notte | 29.989,25 (09:02 italiane) | 7.770,25 (09:01) |
| minimo della notte | 29.783,75 (23:02) | 7.752,25 (03:26) |
| variazione dalla chiusura di ieri | +0,37% | +0,11% |
| delta della notte | +1.569 su 92 mila lotti | +676 su 133 mila lotti |

**Le opzioni a confronto** (convenzione A, cioè dealer lunghi di call e corti di put, in milioni di $ per 1%):

| | NQ (10:13) | ES (10:23, catena parziale) |
|---|---|---|
| GEX di tutte le scadenze | +724, prezzo sopra lo zero gamma 29.590 | -1.570, prezzo sotto lo zero gamma 7.784 |
| GEX di oggi (0DTE) | +297, zero gamma 29.694 | +1.310, zero gamma 7.745 |
| muro | 30.000 | 7.700 (put), 7.800-7.815 (call) |
| mossa attesa di oggi | ±325 (1,09%) | ±56 (0,72%) |

## Convenzioni E Limiti

- **Il campione**: sei settimane per M30, due per M1, una sola fase di mercato, con il calo del tech a metà ottobre. Non dice come si comportano in un altro regime.
- **I rendimenti**: logaritmici, sulla chiusura delle barre. Ogni coppia è sullo stesso orario di apertura. Al minuto sono contati solo i minuti consecutivi.
- **Le soglie**: le "mezz'ore in versi opposti" usano soglie scelte qui (NQ 0,05%, ES 0,03%) e dichiarate. Nessuna variante è stata provata.
- **La traduzione dei livelli**: usa il beta medio. Il beta di un giorno cambia (vedi le discese forti), quindi la traduzione è un'approssimazione di pochi punti.
- **Le opzioni**: il segno della GEX di ES dipende dalla scadenza del 30/10, letta mentre l'open interest cresceva ancora (`../2026-10-28-ES/3-gamma.md`). Il segno vero dei dealer non si conosce, su nessuno dei due.
- **Nessun segnale.** La divergenza della notte è descritta, non usata.
