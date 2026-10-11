# La VWAP Come Tendenza

**Stato**: codificata, non validata su dati veri
**Strategia nel catalogo**: `vwap-tendenza`, versione 1

## Il Paper

Un paper di industria, *Volume Weighted Average Price (VWAP): The Holy Grail for Day Trading
Systems* `[MC · 50:33 - 50:49]`, di Zarattini e Aziz (2023). **Paper non ancora letto**: autori e
anno vanno verificati sul paper, il finding qui sotto e' quello riportato nel transcript.

## Il Finding

Su QQQ a un minuto: long quando la barra chiude sopra la VWAP, short quando chiude sotto, uscita
quando il prezzo torna sulla VWAP. Circa **671% in cinque anni** `[MC · 52:02 - 54:10]`. Il numero
del titolo non interessa: interessa il perche' `[MC · 54:10 - 54:33]`.

## Il Perche'

Quasi ogni trader istituzionale manda ogni giorno almeno un ordine eseguito da un **algoritmo
VWAP**, che cerca un prezzo medio vicino alla VWAP partecipando al volume. Quando il prezzo passa da
un lato all'altro della VWAP, tanti algoritmi dalla stessa parte **amplificano il movimento**
`[MC · 54:43 - 55:48]`.

## Cosa La Ucciderebbe

Che gli algoritmi di esecuzione smettessero di inseguire la VWAP, per esempio passando in massa a
TWAP o a esecuzioni a implementation shortfall. La logica vale su ogni strumento dove le istituzioni
usano algoritmi VWAP `[MC · 56:47 - 56:58]`.

## Le Regole Da Provare

- **ingresso**: long a chiusura M1 sopra la VWAP, short sotto `[MC · 57:04 - 57:33]`, sul **future
  NQ** invece che su QQQ `[MC · 56:12 - 56:27]`;
- **uscita**: chiusura dall'altra parte della VWAP, dove si gira; chiusura alle 16:00 ET;
- **VWAP dalla apertura** delle 9:30 ET, come suggerito `[MC · 57:42 - 58:11]`; quanto aspettare
  dopo l'apertura e' un parametro da studiare (`attesa`) `[MC · 58:11 - 58:42]`;
- **dimensione**: un contratto in sviluppo.

Con barre M1 la VWAP si stima dal prezzo tipico (H+L+C)/3 per il volume; con il footprint sara'
esatta.

Varianti dichiarate:

1. default (`attesa=0`, entrambi i versi);
2. `attesa=30`;
3. `soloLong=si`.

## Le Prove

Nessuna su NQ. **Il costo e' il problema da guardare per primo**: sulla serie sintetica la
strategia fa circa 19 operazioni a seduta, e a 15 $ di costi per operazione (commissione e due tick
di slittamento) il costo da solo e' circa 290 $ a giornata per contratto.
