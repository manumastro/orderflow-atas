---
chiave: vasquez-2025-0dte-index-options-and-market-volatility
sigla: VAPG
titolo: "0DTE Index Options and Market Volatility: How Large is Their Impact?"
autori: [Diego Amaya, Pedro A. Garcia-Ares, Neil D. Pearson, Aurelio Vasquez]
versione_letta: "25 gennaio 2025"
fonte: Cboe (sezione research publications)
identificativo: "nessuno; file gammasqueezes.pdf"
url: https://cdn.cboe.com/resources/education/research_publications/gammasqueezes.pdf
tipo: working paper
revisione_fra_pari: no
pagine: 48
pdf: vasquez-2025-0dte-index-options-and-market-volatility.pdf
pdf_sha256: ace91d549da7c1743beba3e9fb6eb7164a787da1f2a1b8fd3ea64b79b0b46ccb
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-14
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; i dati (trade Cboe con la capacita' di chi compra e chi vende, quote Algoseek, OptionMetrics) sono proprietari"
---

# Vasquez, Amaya, Pearson E Garcia-Ares 2025: L'Analisi

*0DTE Index Options and Market Volatility: How Large is Their Impact?*, versione del 25 gennaio 2025,
48 pagine, ospitato da Cboe. Working paper, non revisionato. Nel PDF gli autori sono in ordine
alfabetico (Amaya, Garcia-Ares, Pearson, Vasquez); la chiave e la sigla seguono l'indice dei paper.
**Cboe ha fornito i dati** dei trade e quota i prodotti studiati: non e' un conflitto dichiarato, ma
va tenuto presente accanto alla conclusione ("l'effetto non e' grande"). Letto per intero il 14
ottobre 2026. Citazioni `[VAPG §n]`.

Le formule (equazioni 1-6, 10, 12-27) escono storpiate da `pdftotext`: le lettere greche spariscono.
**Nella tabella 3 i nomi dei coefficienti sono persi**; quali righe siano quali lo deduciamo dal testo,
e lo diciamo. Le tabelle 1-6 sono state rilette con `pdftotext -raw`. I valori delle figure non si
leggono dal testo.

## I Termini

- **0DTE** (*zero days to expiration*): opzioni che scadono il giorno stesso in cui si negoziano.
  Sull'S&P 500 sono le SPXW; le scadenze del martedi' e del giovedi' esistono dal 26 aprile e dal 19
  maggio 2022, e da allora c'e' una scadenza ogni giorno.
- **Market maker** (OMM, *options market maker*): chi quota le opzioni e prende l'altro lato dei
  clienti. Nei dati Cboe e' una **capacita'** dichiarata su ogni trade, accanto a cliente, cliente
  professionale, firm, broker-dealer.
- **Gamma**: quanto cambia il delta (la sensibilita' al sottostante) quando il sottostante si muove.
  Gli 0DTE al denaro hanno gamma molto alta.
- **Gamma dei market maker** (*dealer gamma exposure*): la gamma della posizione aggregata dei market
  maker, sommata su tutte le serie. **Positiva**: per restare coperti vendono quando il prezzo sale e
  comprano quando scende, e smorzano il movimento. **Negativa**: fanno il contrario e lo amplificano.
- **Open interest**: i contratti aperti a fine giornata. Qui non si usa: le posizioni si ricostruiscono
  dai trade.
- **Volatilita' realizzata**: la radice della somma dei quadrati dei rendimenti a 1 minuto,
  annualizzata (x 252 sul giorno).
- **GARCH**: un modello in cui la varianza di oggi dipende dal rendimento al quadrato e dalla
  varianza di ieri (qui: del minuto prima).

## La Domanda

Quanto puo' alzare la volatilita' dell'S&P 500 la copertura dei market maker, ora che gli 0DTE sono
un terzo del volume? Non la media: **il massimo**.

## Il Metodo

**Le posizioni** [VAPG §2.1-2.2]. Tutti i trade in SPX e SPXW da gennaio 2020 a giugno 2023,
**442.589.991 record**, ciascuno con la capacita' di chi compra e di chi vende. Alla nascita di ogni
serie la posizione dei market maker e' zero; ogni loro acquisto la alza, ogni vendita la abbassa.
Cumulando i trade si ha la **posizione netta dei market maker per serie, minuto per minuto**, senza
supporre nessun segno. Le serie nate prima del 2 gennaio 2020 hanno un errore pari alla posizione di
quel giorno: per limitarlo l'analisi parte dal luglio 2020.

**La gamma** [VAPG §2.3]. Ogni minuto, per ogni serie con posizione diversa da zero: volatilita'
implicita dal punto medio fra denaro e lettera (quote Algoseek), con Black-Scholes-Merton; il prezzo
a pronti dal future **ES** piu' scambiato, riportato alla data; dividendo implicito del giorno prima
e tassi da OptionMetrics. Gamma = posizione x 100 x gamma della serie, sommata su tutte le serie.
Dalle 9:30 alle 16:00; la gamma delle 9:30 include i trade della notte.

**I modelli** [VAPG §3]. Rendimenti a 1 minuto del punto medio di ES, in punti base.

- **GARCH a componenti** (Engle-Sokalska 2012 + MIDAS di Engle-Ghysels-Sohn 2013): varianza = componente
  giornaliera (la varianza realizzata del giorno) x profilo diurno (stimato su tutto il campione) x
  componente intraday GARCH(1,1), con dentro **la gamma dei market maker dei 5 minuti precedenti**,
  pesata con uno schema "Beta". Una varianza iniziale stimata per ogni giorno. Stimato **mese per
  mese** (36 mesi), poi si fa la media dei coefficienti; errore standard = deviazione dei 36 valori
  diviso 6.
- **Modello lineare**: rendimento al quadrato su 5 rendimenti al quadrato ritardati, da 1 a 5 gamma
  ritardate, effetti fissi del giorno e dell'ora. Stesso schema mensile.

**L'effetto** [VAPG §4]. Si recuperano le innovazioni del modello, si rimettono nel modello con i
coefficienti della gamma **posti a zero**, e si confronta la volatilita' realizzata vera con quella
"senza gamma", sul giorno e su finestre di 30 minuti dalle 9:30. Poi si confronta l'effetto massimo
con la distribuzione delle variazioni di volatilita' dovute a tutto [VAPG §6].

## I Risultati

SPX e SPXW per le posizioni, **ES a 1 minuto** per i rendimenti, luglio 2020 - giugno 2023, ore
regolari.

| | risultato | dove |
|---|---|---|
| 1 | Gli **0DTE fanno il 34,771%** del volume SPX+SPXW del giorno medio. Cliente con market maker: **68,717%** del volume; market maker fra loro 8,959%; clienti fra loro 2,184%; solo il 2,866% non coinvolge ne' un cliente ne' un market maker | §2.1, Tab. 1 |
| 2 | Sugli 0DTE i clienti comprano il 17,150% del volume e vendono il 15,988%: **sono compratori e venditori in misura quasi uguale**. Il commercio fra clienti pesa piu' che sulle scadenze lunghe | Tab. 1A |
| 3 | Gamma dei market maker (unita' "miliardi", vedi sotto): media delle medie giornaliere **341,2**, deviazione 319,6. **Di solito positiva**. Il 25 percentile dei minimi giornalieri e' -94,9: **negativa almeno per un momento in almeno un giorno su quattro**; l'1 percentile dei massimi e' -55,1: negativa tutto il giorno in almeno l'1% dei giorni. Minimo -5.426, massimo 16.743,9 | §2.4, Tab. 2A |
| 4 | Prima e dopo le scadenze di martedi' e giovedi': media 408,8 (lug 2020 - apr 2022) contro **234,2** (mag 2022 - giu 2023). Dopo, la mediana dei minimi giornalieri e' **-45,0**: gamma negativa almeno per un momento **in almeno un giorno su due**. La media giornaliera e' raramente negativa prima del 2022, spesso dopo | §2.4, Tab. 2B-C, Fig. 1 |
| 5 | GARCH: il coefficiente della gamma e' **negativo**, circa -0,48 / -0,50 (errore 0,08) a seconda della specifica. La statistica del rapporto di verosimiglianza, **in media sui 36 mesi**, vale 8,94 (1 ritardo) e 9,09 (5 ritardi), contro un valore critico al 5% di 5,99. I ritardi oltre il primo aggiungono 0,15: quasi niente | §3.2, Tab. 3 |
| 6 | Lineare: con un ritardo il coefficiente e' -19,53 (errore 3,76). Con due o piu' ritardi **nessun coefficiente singolo e' significativo** (t sotto 1,5); le F congiunte valgono 16,3, 12,0, 9,0, 7,6, 6,7 | §3.3, Tab. 4 |
| 7 | Effetto sulla volatilita' del giorno (GARCH): media **-0,19 punti**, mediana -0,08, 90 percentile +0,72, 99 percentile +1,56, **massimo +3,32**. Su 30 minuti: media -0,17, 99 percentile +2,01, **massimo +6,42**, minimo -10,63. La gamma **alza** la volatilita' nel 10% dei giorni e delle mezz'ore | §4.1, Tab. 5A |
| 8 | Effetto con il modello lineare: media giornaliera **-2,14 punti**, massimo +3,18; su 30 minuti media -2,32, massimo +7,00 | §5.2 (sic), Tab. 5B |
| 9 | Il termine di paragone: volatilita' realizzata giornaliera media 12,7%. Le sue variazioni da un giorno all'altro hanno deviazione **4,5 punti** e superano i 3 punti in circa il **20% dei giorni**. Sulle mezz'ore: deviazione 4,6, 1 e 99 percentile -11,6 e +14,2, massimo +63,4 | §6, Tab. 6 |
| 10 | La conclusione degli autori: **l'effetto massimo della gamma non e' grande** rispetto a come la volatilita' si muove comunque | §1, §6 |
| 11 | Appendice: per gli 0DTE i termini di *speed* (come cambia la gamma col prezzo) e di *charm* (come cambia il delta col tempo) sono grandi ma **quasi si annullano**; la somma resta proporzionale alla gamma (circa il 6,2% del termine di gamma al picco, a 6,75 ore dalla scadenza). Quindi il ribilanciamento della copertura e' proporzionale alla gamma | App. A.1-A.2, Fig. A1 |

## Cosa Non Regge, O Non Si Dice

1. **I dati sono proprietari.** I trade con la capacita' vengono da Cboe; le quote da Algoseek;
   dividendi e tassi da OptionMetrics. Non si puo' rifare.
2. **Solo SPX e SPXW, solo i market maker registrati.** Le opzioni SPY, quelle sul future ES al CME e
   l'OTC non ci sono, e anche la loro copertura passa per ES. I firm e i broker-dealer possono fare da
   fornitori di liquidita' ma non entrano nella gamma. **La gamma totale dei dealer non e' misurata**.
3. **L'errore della posizione iniziale non e' quantificato.** Le serie SPX nate prima del 2020
   (scadenze da settembre 2020 a dicembre 2022) partono da zero sbagliato; gli autori dicono che
   l'errore e' piccolo perche' l'open interest lontano e' piccolo, ma non lo misurano.
4. **Le unita' della gamma non tornano.** Il testo divide per 10^9 nella tabella 2 e per 10^12 nella
   figura 1 e nei modelli; posizione x 100 x gamma di Black-Scholes, come la definiscono, non da' valori
   di quell'ordine senza un fattore (prezzo, prezzo al quadrato) che non e' scritto. Non sappiamo
   rifare la scala.
5. **Si suppone che i market maker si coprano tutti, sul delta, subito, e in ES.** Non e' osservato.
   La derivazione dell'appendice tiene ferma la volatilita': i cambi di volatilita' implicita (la
   *vanna*) non entrano.
6. **Le statistiche sono deboli e fatte per mese.** La media di 36 statistiche di verosimiglianza non
   ha la distribuzione chi quadro a 2 gradi con cui la confrontano; quanti dei 36 mesi siano
   significativi non e' riportato. Nel modello lineare le F si confrontano con i valori critici del chi
   quadro. **Nessuna significativita' per giornata, nessun modello nullo nel senso del nostro metodo**:
   il "senza gamma" e' una simulazione che dipende dal modello. Il massimo e' un solo valore estremo,
   senza intervallo di confidenza.
7. **E' una descrizione dentro il campione, non una previsione.** La componente giornaliera della
   varianza e' la varianza realizzata **dello stesso giorno**, il profilo diurno e' stimato su tutti i
   tre anni, la varianza iniziale e' un parametro per giorno. Va bene per misurare, non per prevedere:
   nessuna prova fuori campione.
8. **Un legame meccanico non discusso** (deduzione nostra). La gamma di un'opzione al denaro e' piu'
   alta quando la volatilita' implicita e' bassa e quando manca poco alla scadenza. Volatilita'
   implicita bassa va con volatilita' realizzata bassa: parte della relazione negativa fra gamma e
   varianza puo' venire da qui, e non dalla copertura. Le componenti giornaliera e oraria ne assorbono
   una parte, non i movimenti dentro l'ora. C'e' anche il verso opposto: i clienti comprano e vendono
   in risposta alla volatilita', e la posizione dei market maker la segue.
9. **Incoerenze del testo.** La didascalia della tabella 1 cita 17,5%, 5,00% e 24,65%; la tabella e il
   testo dicono 14,952%, 1,659% e 17,150%. La didascalia della tabella 2 chiama 319,6 la media delle
   deviazioni giornaliere; tabella e testo dicono che e' la deviazione delle medie. La sezione "5.2"
   sta dentro la 4 e ci sono due sezioni 6. La tabella 5 rimanda all'equazione 6, l'effetto e'
   l'equazione 9. Il testo dice che le medie del modello lineare sono "di grandezza simile" a quelle
   della GARCH: sono **-2,14 contro -0,19**, undici volte tanto. Il testo dice che il peso dei ritardi
   cala con il ritardo; il peso del quinto (0,194) e' piu' alto del secondo, terzo e quarto (0,145,
   0,108, 0,113).
10. **Il paragone finale e' fra cose diverse**: un effetto sul livello della volatilita' contro la
    distribuzione delle sue variazioni da un giorno all'altro.
11. **Costi e look-ahead di trading**: non c'e' nessuna strategia, quindi nessun costo. Il paper
    misura la volatilita', **non la direzione**: non dice niente su dove va il prezzo.
12. **Il periodo** e' di tre anni, con un cambio di regime dentro (maggio 2022). Solo ore regolari.

## Cosa Si Puo' Rifare Su ES, Con ATAS

Quello che abbiamo: **barre a 1 minuto di ES** con volume e delta, dal Data Bridge; gli indicatori
**Options Chain** su Rithmic (OI Profile, Key Levels, Expected Move, GEX Profile con Zero Gamma)
**senza storia**, che stimano la GEX dall'open interest con un segno supposto; gli indicatori
**Options X-Ray**, dati OptionsDepth sulle SPX ogni minuto, con le posizioni divise per gruppo di
partecipanti, market maker compresi, con una storia che non conosciamo (vedi
[`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md)).

**Subito, con la storia di ES che abbiamo:**

- la volatilita' realizzata giornaliera e a 30 minuti, il profilo diurno, e la tabella 6 (come si
  muove la volatilita' per tutte le cause). E' il termine di paragone di qualunque effetto della gamma.
  Attenzione: il paper usa il **punto medio** fra denaro e lettera; le nostre barre usano i prezzi
  scambiati, che a 1 minuto rimbalzano fra denaro e lettera e gonfiano la varianza. Si confronta con
  le barre a 5 minuti.

**Non si puo' rifare indietro**: la gamma dei market maker minuto per minuto non l'abbiamo nel
passato. La Chain non ha storia; di X-Ray non sappiamo quanta ne abbia.

**Registrando da oggi in avanti** (se il Data Bridge riesce a leggere gli indicatori, da verificare):

- ogni minuto, la gamma dei market maker di X-Ray, la GEX e lo Zero Gamma della Chain, la volatilita'
  implicita;
- la prova: la volatilita' realizzata della mezz'ora successiva e' piu' alta quando la gamma dei market
  maker e' negativa? Con la **significativita' per giornata** (ricampionamento a blocchi di giorni) e un
  **nullo** che rimescola la gamma fra giorni diversi tenendo il profilo dell'ora;
- **due controlli che il paper non fa**: la volatilita' implicita (il legame meccanico del punto 8) e
  la gamma calcolata a volatilita' fissa, come fanno Garleanu, Pedersen e Poteshman;
- **lo stesso test con la GEX della Chain**, che ha il segno supposto: se X-Ray da' l'effetto e la
  Chain no, la convenzione costa; se nessuno dei due lo da', l'idea muore su ES.

**Cosa non va dimenticato:**

- **la Chain legge l'open interest, che si aggiorna una volta al giorno.** Gli 0DTE aperti e chiusi
  nella giornata non entrano mai nell'open interest. Il paper ricostruisce le posizioni trade per trade
  proprio per questo: una GEX da open interest **non vede la gamma degli 0DTE di oggi**, che e' quella
  di cui parla il paper;
- l'effetto e' piccolo (mediana -0,08 punti sul giorno) e il paper ha usato tre anni. **Un anno di
  registrazione potrebbe non bastare** per vederlo: va detto prima di cominciare, non dopo;
- il paper misura la volatilita', non la direzione. Una regola di trading ("in gamma negativa si segue
  la rottura, in positiva si sfuma") **e' un'idea nuova**, con la sua scheda e il suo nullo; il paper
  non la prova.

## Cosa Ne Prende Il Framework

- **La posizione dei dealer si ricostruisce dai trade, non dall'open interest**, quando si puo'; e
  conta la gamma di oggi, compresi gli 0DTE aperti oggi.
- **Il segno della gamma dei market maker cambia**: positiva di solito, negativa almeno per un momento
  in un giorno su quattro (uno su due dopo maggio 2022). Uno studio che la usa come filtro deve avere
  abbastanza giorni negativi nel campione.
- **L'effetto si misura contro come la volatilita' si muove comunque** (tabella 6): e' una forma di
  confronto con il caso, da fare con il nostro nullo e per giornata, non per mese.
- **Il massimo non e' una statistica**: un effetto si riporta con la sua distribuzione e un intervallo.
