# Esito Dei Criteri Sigillati: Il Contratto Della Prop Firm

I criteri sono in [`2026-10-11-il-contratto-prop.md`](2026-10-11-il-contratto-prop.md), sha256
`93721e09f204bfda3c23b4b40d4213fd21cf264cff1db480b21876aa4f670877`, scritti prima del simulatore.
Eseguiti l'11 ottobre 2026 con `quant contratto verifica` (seme 20261011, 200.000 tentativi per
riga, 21,6 s). **L'esito si riporta cosi' com'e', anche dove non passa.**

## L'Esito

| | caso | atteso | misurato | scarto | esito |
|---|---|---|---|---|---|
| C1 | pavimento intraday, violazione intraday | 0,2544 | 0,2560 | +0,6% | passa |
| C1 | pavimento alla chiusura, violazione intraday | 0,3019 | 0,3029 | +0,3% | passa |
| C1 | **pavimento alla chiusura, violazione alla chiusura** | 0,3203 | 0,3618 | **+13,0%** | **non passa** |
| C1 | pavimento intraday, violazione alla chiusura | 0,3203 | 0,3206 | +0,1% | passa |
| C1 | pavimento orario, violazione intraday | 0,2696 | 0,2705 | +0,3% | passa |
| C2 | fisso | 0,4030 | 0,4009 | -0,5% | passa |
| C2 | bloccato a 0 | 0,2493 | 0,2498 | +0,2% | passa |
| C2 | bloccato a +100 | 0,2454 | 0,2460 | +0,2% | passa |
| C2 | sale sempre | 0,2273 | 0,2284 | +0,5% | passa |
| C3 | solo barriere | 0,500 | 0,5003 | +0,0003 | passa |
| C3 | + regola giornaliera | 0,453 | 0,4548 | +0,0018 | passa |
| C3 | + scadenza 30 sessioni | 0,310 | 0,3066 | -0,0034 | passa |
| C3 | entrambe | 0,298 | 0,2974 | -0,0006 | passa |
| C3 | regola giornaliera, senza scadenza | 0,453 | 0,4548 | +0,0018 | passa |
| C4 | **P = 14%** | -127 $ | -155,9 $ | **-28,9 $** | **non passa** |
| C4 | **P = 10%** | -189 $ | -213,6 $ | **-24,6 $** | **non passa** |
| C4 | **P = 7%** | -249 $ | -270,5 $ | **-21,5 $** | **non passa** |
| C4 | **P = 5%** | -303 $ | -321,2 $ | **-18,2 $** | **non passa** |

**C2 e C3 passano. C1 non passa in una combinazione su cinque. C4 non passa.**

## La Diagnosi

Fatta dopo aver visto l'esito, quindi **non e' una prova**: e' la ricerca della causa. Le prove
diagnostiche stanno nella scratchpad della sessione e si rifanno con le stesse chiamate.

**C1, pavimento e violazione alla chiusura.** Il simulatore controlla il target a ogni passo (77 per
sessione), il pavimento e la violazione solo alla chiusura. Rifacendo la stessa combinazione con
**anche il target alla chiusura** (un passo per sessione) la simulazione da' 0,3207 contro 0,3203,
e la simulazione di TF dava 0,3193. Lo scarto nasce da un parametro che TF non elenca fra i sei:
**quanto spesso si controlla il target**. Con la violazione in tempo reale non conta (le altre
quattro righe passano); con la violazione alla chiusura conta fino al 12-17%. Due controlli in piu',
nella stessa diagnosi: pavimento fisso controllato alla chiusura con target alla chiusura, 0,4150
contro la forma chiusa 0,4541 (-8,6%, perche' la forma chiusa corregge solo la barriera bassa);
pavimento che sale sempre, alla chiusura, target intraday: +17,5%.

Conclusione: **il simulatore non e' smentito; il criterio era incompleto, e la forma chiusa di TF ha
un limite di validita' che il paper non dichiara**: vale quando il target si controlla con la stessa
frequenza della violazione, o quando la violazione e' in tempo reale. Per Tradeify (violazione in
tempo reale) vale: +0,3%.

**C4, la deriva implicita.** La forma chiusa riproduce la tabella di TF §5.6 **esattamente**
(-127,3 / -189,0 / -249,4 / -302,9 $) con un'altra geometria: pavimento bloccato a 0, aggiornato e
controllato intraday (P senza deriva 0,2543), non quella del caso di studio (kappa = +100,
pavimento alla chiusura, P 0,2998) che il criterio aveva scritto. TF non dice quale geometria usa
in quella tabella. Conclusione: **il criterio era mal specificato da chi l'ha scritto**; la formula
e il codice riproducono il numero del paper.

## La Lezione

E' la stessa di TF App. B: "sigillare impedisce di spostare il criterio, non di sbagliarlo". Due
criteri su quattro erano scritti male, uno per un parametro mancante, uno per una geometria
indovinata. Nessuno dei due si corregge in questo file: se servira' ripeterli, si scrive un file nuovo
con i criteri corretti, prima di rieseguirli.
