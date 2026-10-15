---
chiave: buis-2024-gamma-positioning-and-market-quality
sigla: BPVZ
titolo: "Gamma positioning and market quality"
autori: [Boyd Buis, Mary Pieterse-Bloem, Willem F.C. Verschoor, Remco C.J. Zwinkels]
versione_letta: versione dell'editore (version of record), Journal of Economic Dynamics and Control 164 (2024) 104880, dal portale EUR
fonte: Journal of Economic Dynamics and Control
identificativo: "10.1016/j.jedc.2024.104880"
url: https://pure.eur.nl/ws/files/149733755/1-s2.0-S0165188924000721-main.pdf
tipo: rivista
revisione_fra_pari: si
pagine: 21 (la prima e' la copertina del portale EUR; l'articolo e' alle pagine 1-20 della rivista)
pdf: buis-2024-gamma-positioning-and-market-quality.pdf
pdf_sha256: aeb80faa4e7343213d423aa6133d06446be951dbcbf425278e0f2457a33f5eea
filone: opzioni-gamma
ruolo: idea
stato: analizzato
letto_il: 2026-10-15
verificato_il: 2026-10-15
studi: [opzioni-gamma-es]
codice_degli_autori: "nessuno; il modello e' descritto nel testo, il codice non e' pubblicato; un'analisi preliminare e' 'disponibile su richiesta' [nota 17]"
---

# Buis, Pieterse-Bloem, Verschoor, Zwinkels 2024: L'Analisi

*Gamma positioning and market quality*, Boyd Buis (NatWest Markets e VU Amsterdam), Mary
Pieterse-Bloem (Rabobank, Erasmus School of Economics), Willem Verschoor e Remco Zwinkels (VU
Amsterdam, Tinbergen Institute). Journal of Economic Dynamics and Control 164 (2024), articolo
104880; ricevuto il 23 dicembre 2022, rivisto il 2 febbraio 2024, accettato il 14 maggio 2024.
**Revisionato**, licenza CC BY. Letto per intero il 15 ottobre 2026, copertina, testo, appendice e
bibliografia. **E' una simulazione, non uno studio su dati di mercato**: nessun numero qui sotto
viene da un mercato vero.

**Come e' stato letto.** Su questo PC non c'e' `pdftoppm`, quindi il PDF non si vede come immagine.
Il testo viene da `pdftotext`. In modo `-layout` le tabelle si mescolano; **in modo `-raw` escono
riga per riga e i numeri delle tabelle 2-5, A1 e A2 sono leggibili**. Un controllo: i rapporti delle
tabelle 2 e 3 tornano dividendo i valori delle tabelle A1 e A2 per la colonna 0. Nella lettura i
simboli matematici (lettere greche e corsive) sparivano; **alla verifica si sono recuperati**: con
`pdftotext -enc UTF-8` escono come lettere matematiche Unicode scritte a coppie surrogate (due
caratteri UTF-16 per un simbolo), e decodificandole si leggono le formule e i nomi della tabella 1.
Le eq. 6-7 e la tabella 1 qui sotto sono quindi **lette, non ricostruite**. **Le figure 1-5 non si
vedono**: se ne leggono solo le didascalie e cio' che il testo ne dice. Citazioni `[BPVZ §n]`,
`[BPVZ Tab. n]`.

## I Termini

- **Dealer dinamico** (*dynamic hedger*): chi vuole tenere costante il delta del proprio portafoglio
  (di solito zero) e lo fa comprando e vendendo il sottostante. Esempi degli autori: banche,
  assicurazioni, ditte di trading ad alta frequenza, hedge fund [§2.1].
- **Delta, gamma**: il delta e' quanto cambia il valore dell'opzione quando il sottostante si muove;
  la gamma e' quanto cambia il delta. **Gamma positiva**: per restare coperto il dealer vende quando
  il prezzo sale e compra quando scende, **contro** il movimento. **Gamma negativa**: compra quando
  sale e vende quando scende, **con** il movimento [§2.1].
- **Effetto di ritorno** (*feedback effect*): gli ordini di copertura muovono il prezzo; la gamma
  netta dei dealer decide se smorzano (positiva) o amplificano (negativa) [§1, §2.1].
- **Straddle**: una call e una put con lo stesso strike e la stessa scadenza. Comprato ha gamma
  positiva, venduto negativa; al denaro parte con delta zero [§2.2.6, nota 10].
- **Contratto di gamma "evergreen"** (che si rinnova da solo): ogni giorno t il dealer compra (o
  vende) straddle al denaro che scadono in t+2 e chiude quelli del giorno prima, che scadono in
  t+1, insieme a tutta la posizione nel sottostante. Cosi' ogni seduta parte con delta zero e gamma
  piena [§2.2.5, fig. 1].
- **Modello a intelligenza zero** (*zero intelligence*): gli ordini arrivano a caso secondo regole
  fisse, senza strategia, senza memoria, senza inventario. E' la scuola di Farmer e altri (2005)
  [§1, §2.2.3].
- **Libro degli ordini** (*order book*): gli ordini limite in attesa, a ogni prezzo; **mid** e' la
  media fra il miglior bid e il miglior offer.
- **Qualita' del mercato**: per gli autori quattro cose, volatilita' bassa, liquidita' alta,
  scoperta del prezzo veloce, nessun fallimento [§3].
- **Fallimento del mercato**: arriva un ordine a mercato piu' grande di tutto il lato opposto del
  libro; prezzo, volatilita' e liquidita' smettono di essere definiti [nota 3, §4.4, eq. 26].
- **LOS** (*limit order book slope*): una misura di liquidita' che pesa i volumi del libro per la
  loro distanza dal mid, cosi' che conti di piu' il volume vicino al prezzo [§3.2, eq. 22].

## La Domanda

Che cosa fa alla qualita' del mercato del sottostante la posizione in gamma dei dealer dinamici,
lunga o corta, e quanto? Volatilita', liquidita', scoperta del prezzo e rischio di fallimento, nelle
due direzioni del segno, non solo nel caso "dealer corti di gamma" che assumono BDLM [§1].

## Il Metodo

**Il mercato** [§2.2.1-2.2.4]: un solo titolo, un solo libro degli ordini, tick 0,01, prezzo
iniziale 5, nessun dividendo e nessun tasso. Il tempo e' a strati: la **seduta** (T = 20 per
simulazione), l'**intervallo** (10 per seduta), il **sottointervallo** (4 per intervallo); in ogni
sottointervallo arriva un numero di ordini estratto da una Poisson di parametro 40 (λ, Tab. 1; il
valore e' solo nella tabella, il testo non lo ripete). Gli autori leggono il
sottointervallo come **un quarto d'ora** e l'intervallo come un'ora: 11-12 sottointervalli sono
"circa tre ore di contrattazione" [§4.3], e la nota 23 parla di "scala del quarto d'ora". Una seduta
e' quindi di circa 10 ore.

**Il valore fondamentale** [§2.2.2, eq. 3]: una passeggiata casuale gaussiana con uno shock
all'inizio di ogni intervallo, indipendente dal libro. Nella §4.1 la sua deviazione standard e'
zero (valore fondamentale fermo a 5); nella §4.2 c'e' la notizia (vedi i dubbi sul valore).

**Gli ordini a caso** [§2.2.3, §2.3]: quattro tipi, dalla combinazione di due scelte.
- Immediatezza: il 25% degli ordini sono **a mercato** (chi prende liquidita'), il 75% **limite**
  (chi la offre); i = 0,25 [§2.3, Tab. 1].
- Informazione: il 30% guarda il **valore fondamentale** (informati), il 70% guarda il **mid**
  (non informati); u = 0,3 [§2.3, Tab. 1]. La §2.2.3 scambia i due nomi (u per l'immediatezza, i
  per l'informazione); la §2.3, la §4.4 e la tab. 5 usano i per l'immediatezza.
- Gli ordini a mercato informati vanno nella direzione del valore fondamentale; quelli non informati
  a caso. Gli ordini limite si mettono a una distanza normale dal riferimento, in media 10 centesimi
  sotto (bid) o sopra (offer), deviazione standard 0,1. Volumi uniformi fra 0 e 1, non scalati.
- Gli ordini in attesa si cancellano con un decadimento continuo che cresce col quadrato della
  distanza dal mid, parametro 0,05, applicato dopo ogni arrivo (eq. 5).

**Il dealer** [§2.2.5-2.2.6]: **uno solo**, che rappresenta tutti i dealer dinamici. Compra o vende
straddle al denaro da una controparte che non tocca mai il libro. Il numero di straddle va da **-80 a
+80, a passi di 20** [§4.1]. Delta e gamma vengono da Black e Scholes applicato a un prezzo che segue
una passeggiata aritmetica (senza la correzione lognormale, nota 12), con volatilita' implicita
costante pari alla volatilita' realizzata misurata in una prova preliminare, **0,089** [§2.3]. Il
dealer e' "valutato" sul suo delta alla fine di ogni intervallo (l'ora): alla fine di ogni
sottointervallo manda un ordine limite per tutto il delta da coprire (volume -δ), **dal passivo
all'aggressivo man mano che si avvicina la misura** [eq. 6-7]. Per un acquisto: al primo quarto
d'ora uno spread sotto il miglior bid (pb - (po - pb)), al secondo sul miglior bid, al terzo a meta'
spread, all'ultimo **sul miglior offer**, cioe' attraversa lo spread e prende liquidita' (non e' un
ordine a mercato senza limite, che nel modello e' ⟨x,∞⟩). La vendita e' simmetrica [eq. 7]. La
nota 13 da' un polinomio di terzo grado che estende la regola a un numero di sottointervalli
diverso da 4.

**Le misure** [§3]: volatilita' come deviazione standard delle variazioni del mid (S.1), escursione
massimo-minimo del mid **sull'intero percorso**, non sulla seduta (S.2, eq. 16), i quantili negativi al 5% e all'1% delle variazioni del quarto d'ora (S.3,
S.4) e il quantile al 5% di quelle giornaliere (S.5); liquidita' come spread denaro-lettera (L.1),
volume scambiato (L.2), volume nel libro (L.3), LOS (L.4); scoperta del prezzo come errore quadratico
fra mid e valore fondamentale (D.2), correlazione fra lo shock di notizia e la variazione di prezzo
dell'intervallo dopo (D.3), autocorrelazione al primo ritardo delle variazioni di prezzo (D.1).
**Le sigle sono quelle delle tabelle, non del testo**: nella §3 L2 e' il volume nel libro e L3
quello scambiato, D1 l'errore quadratico, D2 la correlazione e D3 l'autocorrelazione [eq. 21-25];
nelle tabelle L.2 e' il volume scambiato, L.3 il libro, D.1 l'autocorrelazione, D.2 l'errore
quadratico, D.3 la correlazione. La quota del dealer nel flusso ("Proportion of flow", Tab. 3
pannello A) **non e' definita nel testo**: non si sa se conti ordini o volumi.

**Il disegno** [§2.2, §2.3, §4]: **100 percorsi** per scenario, ciascuno di 20 sedute, la prima
scartata perche' il libro si riempie. Gli scenari di gamma usano **gli stessi semi casuali**: sono
mercati paralleli che differiscono solo per il dealer. Si riportano medie sui 100 percorsi e il loro
errore standard; le tabelle 2 e 3 riportano le misure come **rapporto** con lo scenario senza
dealer (0 straddle). **Nessun test statistico formale.** Quattro esperimenti: senza notizie
(§4.1, Tab. 2, A1), con notizie (§4.2, Tab. 3, A2), un salto del valore fondamentale da 5 a 4
(§4.3, Tab. 4), e il fallimento del mercato al variare di cinque parametri (§4.4, Tab. 5). **Tutti i
parametri sono scelti dagli autori, non stimati dai dati** [nota 16].

## I Risultati

Nelle tabelle 2 e 3 "rapporto" vuol dire misura dello scenario divisa per quella senza dealer (1 =
uguale); fra parentesi l'errore standard sui 100 percorsi.

| | risultato | dove |
|---|---|---|
| 1 | **Volatilita', senza notizie**: deviazione standard delle variazioni (S.1) rapporto **1,789** (0,162) con -80 straddle, **0,579** (0,040) con +80. Valori assoluti 0,068, 0,038 senza dealer, 0,022. L'effetto e' **non lineare**: cresce di piu' dal lato negativo | §4.1, Tab. 2, Tab. A1 |
| 2 | **Volatilita', con notizie**: S.1 rapporto **1,243** (0,056) a -80, **0,852** (0,046) a +80; escursione massimo-minimo (S.2) 1,380 e 0,782 | §4.2, Tab. 3, Tab. A2 |
| 3 | **Con poca gamma la volatilita' resta dentro il rumore, l'autocorrelazione no**: con notizie, a -20 e +20 (il dealer fa il **19,2%** e il **18,1%** del flusso) S.1 e' 1,009 (0,049) e 0,957 (0,048), dentro un errore standard; anche a -40 (29,5% del flusso) S.1 e' 1,043 (0,050). Negli stessi scenari a ±20 pero' l'autocorrelazione D.1 e' 0,298 (0,016) e 1,409 (0,021) del valore senza dealer (in assoluto -0,075 e -0,355 contro -0,252), le code giornaliere S.5 1,106 (0,038) e 0,851 (0,027), lo spread L.1 0,754 e 0,870: scarti di molti errori standard. Senza notizie S.1 a +20 e' gia' 0,816 (0,068) | Tab. 3, pannelli A, C, D, E; Tab. A2; Tab. 2 |
| 4 | **Quanto pesa il dealer**: la sua quota del flusso e' 52,4% a -80, 40,1% a -60, 29,5% a -40, 19,2% a -20; 36,6% a +80, 33,1% a +60, 27,1% a +40, 18,1% a +20. Nessuno scenario ha una quota piu' piccola del 18% | Tab. 3, pannello A |
| 5 | **Il dealer corto si copre peggio**: delta medio assoluto alla misura 18,448 a -80 contro 10,302 a +80 (con notizie; 16,390 contro 11,203 senza), con gamma media simmetrica (-117,6 e +120,2). Per gli autori l'effetto negativo e' quindi **leggermente sottostimato** ("slightly understated") | §4.1, Tab. 2, Tab. 3 |
| 6 | **Autocorrelazione delle variazioni** (D.1, valori assoluti): senza dealer **-0,254** (senza notizie) e -0,252 (con notizie): il prezzo tende a tornare indietro. A -80 diventa **positiva**, 0,091 e 0,127: il prezzo continua nella direzione presa. A +80 scende a -0,347 e -0,345 | §4.1, Tab. A1, Tab. A2 |
| 7 | **Le code**: le variazioni giornaliere estreme (S.5, quantile al 5%) crescono con la gamma negativa, rapporto **2,596** a -80, e calano con quella positiva, **0,617** a +80. Le variazioni del quarto d'ora estreme (S.3, S.4) calano ai due estremi (0,637 a -80, 0,500 a +80) ma **crescono** a -40 e -20 (1,151 e 1,176 per S.3). Gli autori spiegano la differenza fra le scale con l'autocorrelazione: il dealer corto ammorbidisce il singolo quarto d'ora ma ne accumula molti nella stessa direzione | §4.2, Tab. 3, nota 23 |
| 8 | **Liquidita'**: con un dealer di qualunque segno lo spread scende e il volume sale. Il dealer corto stringe di piu' lo spread (rapporto 0,406 a -80, 0,652 a +80, con notizie); il dealer lungo riempie di piu' il libro lontano dal prezzo (senza notizie, volume nel libro +205% a +80 contro +137% a -80). Il dealer corto consuma piu' liquidita' (volume scambiato 1,456 contro 1,183, con notizie) | §4.1, §4.2, Tab. 2, Tab. 3 |
| 9 | **Scoperta del prezzo, con notizie**: l'errore quadratico fra prezzo e valore fondamentale (D.2) e' 7,617 volte quello senza dealer a -80 e 3,514 volte a +80; la correlazione fra notizia e prezzo (D.3) scende a 0,330 e 0,495 del valore senza dealer. Per gli autori il dealer, di qualunque segno, e' un **operatore di rumore** per la scoperta del prezzo. Con poca gamma pero' l'errore quadratico scende (0,917 a -20, 0,883 a +20): gli autori dicono "generally" | §4.2, Tab. 3 |
| 10 | **Il salto da 5 a 4 del valore fondamentale** (in sottointervalli, cioe' quarti d'ora): prima volta che il prezzo lo raggiunge, 11 a -80, 12 senza dealer, **53 a +80** (piu' di una seduta). Seconda volta, cioe' ritorno dopo lo sforamento: **57 a -80**, 14 senza dealer, 54 a +80. Tempo medio fra due attraversamenti a regime: 5,810 a -80, 4,333 senza dealer, 2,571 a +80 (non monotono: 3,095 a -40). Nessun errore standard nella tabella. Gamma negativa: arriva presto e **sfora**; gamma positiva: arriva tardi e poi **resta incollata** al valore | §4.3, Tab. 4 |
| 11 | **Fallimento, piu' ordini a mercato**: con il 35% di ordini a mercato (il riferimento e' il 25%; il pannello parte dal 30%) fallisce il 50% dei percorsi a -80, nessuno da -20 in su; con il 40%, 100% a -80, 85% senza dealer, **0% da +20 in su**; con il 45%, 100% senza dealer e 10% a +80 | §4.4, Tab. 5, pannello A |
| 12 | **Fallimento, libro meno profondo** (decadimento da 0,05 a 0,175): il dealer corto fallisce gia' a 0,075 (50% a -80); da 0 in su quasi mai (al massimo 10%) | Tab. 5, pannello C |
| 13 | **Fallimento, ordini enormi** (una frazione 0,02 degli ordini a mercato con un volume fisso v_e invece che uniforme fra 0 e 1; v_e provato da 5 a 30): a volume 20, 100% senza dealer, 0% da +60 in su; a 30, 25% a +80. Qui il dealer corto non fa peggio del mercato senza dealer: a v_e 15 e 20 fallisce l'80% e il 90% a -80 contro il 100% senza dealer | §4.4, Tab. 5, pannello E |
| 14 | **Dove la gamma positiva non protegge**: con notizie piu' volatili (deviazione standard 0,1 e 0,125) fallisce il 55-70% e il 65-100% degli scenari lunghi, contro il 65% e l'80% senza dealer. Con market maker che quotano piu' lontano (-0,3 e -0,35) gli scenari lunghi falliscono **piu'** di quello senza dealer (10-40% contro 0%; 75-85% contro 5%) | Tab. 5, pannelli B e D |

**Le conclusioni degli autori** [§5]: la gamma positiva dei dealer migliora volatilita', liquidita'
e stabilita'; quella negativa alza la volatilita', peggiora la scoperta del prezzo e rende il
mercato molto piu' fragile; la scoperta del prezzo peggiora con il dealer in ogni caso. Ne traggono
una proposta di politica: banche centrali ed emittenti potrebbero vendere straddle al denaro a
sconto, o dare opzioni non cedibili, per mettere i dealer lunghi di gamma. **Ammettono che la
dimensione vera dell'effetto "e' difficile da quantificare"** e che il modello "non e' adatto a
essere calibrato su dati reali" [§5]. Come prova empirica propongono un confronto **fra strumenti**
con gamma netta presumibilmente diversa (*cross-sectional*), non una serie storica [§5].

## Il Perche'

Il meccanismo e' meccanico, ed e' scritto nel modello [§2.1, §4.2, §4.3]:

1. **Gamma positiva**: il prezzo sale, il delta del dealer sale, il dealer vende; il prezzo scende,
   compra. Fa da **contrarian** e aggiunge ordini contro il movimento: volatilita' piu' bassa,
   autocorrelazione piu' negativa, libro piu' pieno lontano dal prezzo, che assorbe gli urti. Lo
   stesso freno rallenta il prezzo quando il valore vero cambia davvero (risultato 10).
2. **Gamma negativa**: il dealer compra dopo i rialzi e vende dopo i ribassi, fa da **momentum**.
   Il singolo quarto d'ora resta morbido perche' il dealer sta in gran parte nel libro come ordine
   limite, ma i quarti d'ora si sommano nella stessa direzione: escursioni piu' ampie, code giornaliere
   piu' grasse, sforamenti oltre il valore vero, e alla fine un ordine che svuota un lato del libro.
3. **Il dealer offre liquidita' in ogni caso**: per la regola di copertura e' passivo per buona
   parte dell'ora, quindi lo spread scende anche col dealer corto [§4.1]. Gli autori avvertono che
   e' un risultato del modello: i market maker veri, davanti a piu' volatilita', allargherebbero i
   prezzi o si ritirerebbero, e il modello non lo fa [nota 5].

**Le alternative**: il paper non mette alla prova spiegazioni alternative, perche' in un mercato
simulato il canale e' isolato per costruzione (stessi semi, solo il dealer cambia) [§1, §2.2]. Una
sola confusione viene trattata: senza notizie gli operatori informati comprano sotto 5 e vendono
sopra, e si comportano **come un dealer lungo di gamma**; per questo gli autori aggiungono le notizie
nella §4.2 [§4.2].

**Cosa lo ucciderebbe**, dedotto dal perche' (gli autori non lo dicono): su un mercato dove i
dealer coprono una **quota grande** del flusso, nessuna differenza di volatilita',
di autocorrelazione dei rendimenti brevi o di escursione dei prezzi fra i giorni di gamma
positiva e negativa. **Il paper non ha scenari con il dealer sotto il 18% del flusso** (risultato
4): a quella quota la volatilita' e' gia' dentro il rumore ma l'autocorrelazione no (risultato 3).
Su un mercato dove la quota e' molto piu' piccola il paper non prevede niente, e un nulla su NQ non
lo smentirebbe ne' lo confermerebbe.

## Cosa Abbiamo Riprodotto

Niente. Il modello si potrebbe riscrivere dal testo: le eq. 6-7 della copertura e i parametri della
tabella 1 si leggono (vedi "Come e' stato letto"). Restano da indovinare alcuni dettagli (come si
conta la quota del flusso, l'ordine esatto fra l'ordine del dealer e gli arrivi), e il codice degli
autori non c'e'.

## Cosa Non Regge, O Non Si Dice

1. **Nessun dato di mercato.** Tutto e' simulato, i parametri sono scelti a mano [nota 16] e il
   modello non si calibra sul reale [§5]. L'abstract dice che l'effetto conta "specialmente per gli
   strumenti con bassa liquidita' o basso volume": e' una lettura delle tabelle 5, non una misura.
2. **Il dealer e' enorme.** Nei suoi otto scenari fa fra il 18% e il 52% del flusso (risultato 4),
   e non c'e' uno scenario con una quota piu' piccola. Calando la quota cala l'effetto sulla
   volatilita' (S.1 dentro un errore standard a -40, -20 e +20, con notizie), ma non quello
   sull'autocorrelazione, sulle code giornaliere e sullo spread (risultato 3). Gli autori non
   definiscono come misurano la quota e non la mettono in relazione con un mercato reale.
3. **Non ci sono test statistici.** Medie ed errori standard su 100 percorsi ("standard error of
   ratios of means", note delle tab. 2 e 3), nessun test; i semi uguali fra scenari permetterebbero
   un confronto appaiato, e le note non dicono che sia stato usato (nostra lettura). Diverse frasi
   con "clearly" poggiano su **un solo percorso rappresentativo** (fig. 2 e 4, §4.1-4.2), e la
   tabella 4 non ha errori standard: il primo attraversamento 11 contro 12, che gli autori stessi
   chiamano "slightly faster", non si puo' giudicare.
4. **La tabella 5 non sembra fatta su 100 percorsi.** La nota dice 100, ma **tutti** i suoi
   valori sono multipli del 5%, come succederebbe con 20 percorsi. E' una nostra deduzione, non
   detta dagli autori: le percentuali di fallimento vanno lette come stime grezze. Ci sono anche
   righe non monotone (con il 37,5% di ordini a mercato fallisce il 20% senza dealer e lo 0% a -20).
5. **Il valore della volatilita' delle notizie nella §4.2 e' incerto.** La nota della tabella 1 dice
   0,1, il testo della §4.2 dice 0,01. Con 0,1 la tabella 5 da' il 65% di fallimenti senza dealer,
   mentre la nota 24 dice che nessuna simulazione usata per le misure e' fallita; e nei pannelli C
   e D della tabella 5 la prima riga e' il valore di riferimento (ζ 0,05, μp -0,1), come nel
   pannello B la riga 0,01. **Vale quasi certamente 0,01**, e la nota della tabella 1 e' un refuso
   (nostra deduzione).
6. **Le etichette delle code sono sbagliate o ambigue**: "Q-shock .05%" e ".01%" sono nel testo i
   quantili al 5% e all'1% delle variazioni del sottointervallo (ε*0,95,Q e ε*0,99,Q, eq. 17-18)
   [§3.1]; "H-shock" e' nel testo la variazione **giornaliera** (ε*0,95,T, eq. 19) ma l'etichetta
   suggerisce l'ora.
7. **Scoperta del prezzo, due letture opposte, fra i due esperimenti.** Nella §4.1 (senza notizie)
   la gamma positiva "migliora molto" la scoperta del prezzo: l'autocorrelazione diventa piu'
   negativa **e anche l'errore quadratico scende** (D.2 0,153 a +80, Tab. 2). Nella §4.2 (con
   notizie) l'errore quadratico sale (3,514 a +80) e la correlazione con le notizie scende, e le
   conclusioni dicono che la gamma positiva la peggiora. La differenza non e' fra misure ma fra
   esperimenti; gli autori la spiegano solo di passaggio (senza notizie gli informati fanno da
   dealer lungo, §4.2), e le conclusioni non citano il risultato della §4.1.
8. **L'abstract generalizza oltre le tabelle.** "La gamma positiva sostiene la liquidita' negli
   scenari di stress" vale per piu' ordini a mercato, libro meno profondo e ordini enormi; **non**
   vale per notizie molto volatili ne' per market maker che quotano lontano (risultato 14), dove il
   mercato senza dealer e' il piu' robusto. Il testo lo dice a meta' ("disruptive for short and long
   gamma positions, though slightly more so for short positions").
9. **Le scelte che fanno il risultato**: un dealer solo; straddle sempre al denaro, rinnovati ogni
   seduta, quindi gamma sempre massima e concentrata sul prezzo corrente (le opzioni comprate il
   giorno t scadono in t+2 e si rivendono il giorno dopo, con una seduta alla scadenza); volatilita' implicita
   costante; prezzo a passeggiata aritmetica; copertura misurata ogni ora con una regola fissa.
   Nessuna di queste e' variata. I market maker non si ritirano ne' allargano gli spread [nota 5],
   proprio il comportamento che nei crolli veri conta.
10. **Gli 0DTE non ci sono come tali**: niente scadenza dentro la seduta, niente gamma che esplode
    nelle ultime ore. Il paper non risponde alla parte 0DTE della domanda.
11. **Conflitti**: i primi due autori lavorano per NatWest Markets e Rabobank, banche con banchi di
    copertura dinamica, e la proposta finale riguarda proprio come remunerare quei banchi. C'e' la
    frase di rito ("le opinioni sono degli autori"), nessuna dichiarazione di conflitto.
12. **Il testo letto**: le figure 1-5 non sono state viste. Nel PDF non c'e' nessuna frase rivolta a
    chi lo legge con un agente.
13. **Nomi incoerenti**: i parametri u e i sono scambiati fra la §2.2.3 e la §2.3; le sigle L e D
    delle tabelle non sono quelle delle eq. 21-25 (vedi Il Metodo). Chi rifa' il modello deve
    seguire i valori della §2.3 e della tabella 1.

## Cosa Si Puo' Rifare Su NQ, Con ATAS

Lo strumento e' NQ, i dati vengono solo da ATAS (CLAUDE.md, decisione 2). Cosa abbiamo, da
[`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md): barre di NQ a 1 minuto con volume e
delta (molti anni di storia, dal continuo di ATAS); la catena delle opzioni di NQ dal Data Bridge,
con la GEX calcolata da `FabioOrderFlow/tools/gex.py` **dal 15 ottobre 2026**, senza storia
all'indietro; X-Ray non disegna su NQ.

**Le variabili che il paper prevede si misurano gia' con le sole barre:**

- volatilita' dei rendimenti a 15 minuti e a 1 ora, escursione massimo-minimo della seduta (il
  paper la misura sull'intero percorso simulato, eq. 16: la seduta e' un nostro adattamento), code
  della variazione giornaliera (risultati 1, 2, 7);
- **autocorrelazione dei rendimenti a 15 minuti dentro la seduta**: il paper prevede che diventi
  positiva con la gamma negativa e piu' negativa con quella positiva (risultato 6). E' la previsione
  piu' netta e la piu' vicina a BB e BDLM;
- il comportamento dopo uno shock (sforamento e ritorno), che su NQ si puo' guardare attorno ai dati
  macro delle 8:30 (risultato 10).

**Il condizionamento sulla gamma invece non ha storia.** La GEX di NQ esiste solo da quando la
calcoliamo, cioe' **dentro il fuori campione di NQ** (dal 2025 in poi, [`5-come-si-valida.md`](../../../metodo/5-come-si-valida.md)):
un confronto fra giorni di GEX positiva e negativa su NQ si potra' fare solo su giornate che il
metodo tiene per la validazione finale, e come va trattato **lo decide l'utente prima**, non chi
legge un paper.

**La quota del dealer, il ponte fra il modello e NQ.** Il paper dice che l'effetto conta quando il
dealer fa una parte grande del flusso. Su NQ, a una stima del 15 ottobre in
[`le-opzioni-in-atas.md`](../../../metodo/le-opzioni-in-atas.md) (nostra, non del paper), la GEX
totale chiedeva circa 2.500 future per un ribasso di 300 punti, contro 500-700 mila contratti
scambiati al giorno: **circa lo 0,4-0,5% del flusso** (il rapporto e' nostro calcolo), contro il
18-52% del modello. Le due quote non sono la stessa misura: la nostra e' la copertura per un
ribasso di 300 punti divisa per il volume di una giornata, quella del paper una "Proportion of flow"
che il paper non definisce. **Il paper non ha scenari sotto il 18%**: a quella quota l'effetto
sulla volatilita' e' gia' dentro il rumore, quello sull'autocorrelazione no (risultato 3), e cosa
resti allo 0,5% non lo dice. Che su NQ l'effetto sia piccolo e' quindi **una nostra estrapolazione,
non una previsione del paper**; resta aperto se sia l'S&P (SPX ed ES), dove le opzioni pesano di
piu', il mercato su cui guardarlo. ES per le opzioni e' gia' ammesso dal 14 ottobre 2026
(decisione 2).

## Cosa Ne Prende Il Framework

- **Il perche'** per la scheda dello studio sulla gamma: gamma positiva frena e fa tornare indietro
  il prezzo, gamma negativa lo spinge e fa sforare. E **la condizione che il perche' richiede**: una
  quota del flusso di copertura non piccola. La scheda deve stimarla prima di provare l'effetto.
- **La previsione da provare, una volta che c'e' la GEX**: l'autocorrelazione dei rendimenti a 15
  minuti e la volatilita' della seduta, divise per segno della gamma, una osservazione per giornata,
  contro il nullo.
- **Cosa non prende**: nessun numero. Le tabelle sono di un mercato simulato con parametri scelti a
  mano; non sono un finding di mercato e non vanno citate come tali.
- **Un'idea di metodo**, da dichiarare come aggiunta se mai si usa: il disegno a mercati paralleli
  con gli stessi semi (BPVZ §2.2) e' un **controllo positivo** naturale per il mercato sintetico del
  framework: un mercato dove un effetto c'e' per costruzione dice se le prove hanno la forza di
  vederlo, accanto alla passeggiata casuale dove non deve passare niente.
- **Nessuna regola operativa.**

## La Verifica

**15 ottobre 2026**, verifica avversaria sul PDF (sha256 ricontrollato, uguale alla scheda; 21
pagine). Testo da `pdftotext -raw` e `-layout`, e da `pdftotext -enc UTF-8` con le coppie
surrogate decodificate, che restituisce i simboli delle formule e della tabella 1. Ogni valore delle
tabelle 2-5, A1, A2 citato qui e' stato riletto riga per riga, e i rapporti delle tab. 2-3 ricontrollati
contro A1-A2 (per esempio S.1 con notizie 0,143/0,115 = 1,243 e 0,098/0,115 = 0,852; D.1 -0,345,
-0,252, +0,127; S.5 -0,244/-0,094 = 2,596). Figure non viste (manca un modo di renderizzare il PDF).

**Affermazioni controllate: circa 130** (ogni numero contato a parte): **confermate 112, corrette
15, tolte 3**.

**Le correzioni gravi**

1. **La soglia del flusso era sbagliata.** L'analisi diceva che l'effetto e' grande solo con il
   dealer al 30-52% del flusso, e dentro un errore standard al 18-19%. Vale solo per la volatilita'
   S.1 (con notizie), e nemmeno del tutto: a -40, con il 29,5% del flusso, S.1 e' 1,043 (0,050),
   dentro un errore standard. A ±20 invece l'autocorrelazione D.1 (0,298 e 1,409, errori 0,016 e
   0,021), le code giornaliere S.5 e lo spread si muovono di molti errori standard [Tab. 3]. Corretti
   i risultati 3 e 4, "Cosa lo ucciderebbe", il punto 2 dei limiti e la parte su NQ.
2. **"Il paper porta a prevedere su NQ un effetto piccolo"** non e' del paper: il paper non ha
   scenari sotto il 18% del flusso e non definisce la quota. Ora e' scritto come estrapolazione
   nostra; la stima 0,4-0,5% resta attribuita a `le-opzioni-in-atas.md` (riga "La misura della
   scala") e il rapporto e' dichiarato come nostro calcolo.
3. **Limite 7, la scoperta del prezzo**: la contraddizione non e' fra misure diverse ma fra i due
   esperimenti. Nella §4.1 anche l'errore quadratico migliora con la gamma positiva (D.2 0,153 a
   +80, Tab. 2).

**Le correzioni minori**: eq. 6-7 lette (all'ultimo quarto d'ora l'ordine sta sul miglior offer,
non "a mercato"; al primo uno spread oltre il miglior bid); i simboli non "spariscono", si
recuperano; tab. 1, λ = 40 ordini per sottointervallo; scadenze t+2 e t+1; S.2 e' l'escursione
dell'intero percorso, non della seduta; sigle L e D delle tabelle diverse dal testo, u e i
scambiati nella §2.2.3 (aggiunto il limite 13); "Proportion of flow" non definita; risultato 5
"leggermente" sottostimato; risultato 11 "da -20 in su" e il pannello A parte dal 30%; risultato 13
v_e e' un volume fisso, e il dealer corto non fallisce piu' del mercato senza dealer a v_e 15-20;
risultato 9 l'eccezione a ±20; risultato 10 non monotono e senza errori standard; limite 3, gli
autori scrivono "slightly faster" per 11 contro 12, e i "clearly" stanno su un solo percorso
rappresentativo; aggiunta la proposta di uno studio fra strumenti [§5].

**Tolte**: "la forma esatta delle eq. 6-7 e' ricostruita"; "su un mercato dove la quota e' piccola
il paper stesso prevede un effetto piccolo"; "molte frasi poggiano su differenze di una sola unita'".

**I dubbi del lettore**: (1) rapporti delle tab. 2-3 confermati; (2) eq. 6-7 ora lette, non
ricostruite; (3) i nomi della tab. 1 si leggono, 40 e' λ; (4) il simbolo e' t; (5) refuso della
nota della tab. 1 confermato come deduzione, con un indizio in piu'; (6) tab. 5 tutta a multipli del
5%: confermato, resta marcato come nostra deduzione; (7) etichette confermate, con i simboli delle
eq. 17-19; (8) figure ancora non viste, se ne usano solo didascalie e testo. Nessuna frase nel PDF
rivolta a un agente.
