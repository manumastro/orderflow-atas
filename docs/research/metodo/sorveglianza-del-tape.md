# Sorveglianza Del Tape: Dai Livelli All'Alert Sul Chart

Procedura standard di studio per una seduta. Chiude il cerchio fra le tre cose che finora stavano
separate: l'analisi che deriva i livelli, il chart che li disegna, e il tape che li mette alla
prova minuto per minuto.

Prerequisiti: [`profile-framing.md`](profile-framing.md) per derivare i livelli,
[`livelli-sul-chart.md`](livelli-sul-chart.md) per come arrivano sul chart,
[`../giornate/come-si-scrive-una-giornata.md`](../giornate/come-si-scrive-una-giornata.md) per dove
finisce il risultato.

## Il Giro Completo

```text
1. analisi          il profilo delle sedute passate produce i livelli del giorno
2. file             i livelli si scrivono in docs/research/giornate/livelli-AAAA-MM-GG.json
3. chart            bridge.py levels --file li spinge, l'indicatore li disegna
4. sorveglianza     watch_signals.py legge lo stesso file e valuta il tape sui livelli
5. alert            un segnale forte torna sul chart come livello effimero
6. giornata         quello che e' successo entra nel file del giorno
```

Il punto 2 e' la chiave: **il file dei livelli e' l'unica fonte**, letta sia da chi disegna sia da
chi sorveglia. Senza, le due cose divergono e il monitor guarda livelli diversi da quelli che vedi.

## 1-2. I Livelli, In Un File Per Giornata

Un array JSON in `docs/research/giornate/livelli-AAAA-MM-GG.json`, un oggetto per livello:

```json
{
  "price": 29306,
  "label": "VAL lun · test 07:19 senza compratori",
  "color": "#81C784",
  "style": "solid",
  "width": 2,
  "note": "1.561 lotti con delta -31: nessuno ha comprato."
}
```

L'etichetta porta **la misura, non solo il nome**; la `note` non viene disegnata e tiene il
ragionamento per esteso. Le regole complete stanno in
[`livelli-sul-chart.md`](livelli-sul-chart.md).

Il file resta nel repository: e' il documento di cosa si guardava quel giorno, e serve a rileggere
la giornata sapendo quali livelli erano attivi.

## 3. Spingerli Sul Chart

```bash
python3 FabioOrderFlow/tools/bridge.py levels --file docs/research/giornate/livelli-2026-09-15.json
```

`POST /levels` **sostituisce l'intera lista**. E' voluto: il file e' la fonte, il chart ne e' il
riflesso.

## 4. Armare La Sorveglianza

```bash
python3 FabioOrderFlow/tools/watch_signals.py \
    --levels docs/research/giornate/livelli-2026-09-15.json \
    --from 2026-09-15T07:30 \
    --push-alerts
```

Interroga il bridge ogni 20 secondi e valuta **solo le barre chiuse**: quella in formazione cambia
volume e delta fino all'ultimo secondo, e giudicarla produce letture che poi non reggono.

Emette un solo segnale per barra, in ordine di priorita':

| Segnale | Quando | Sul chart |
|---|---|---|
| `ROTTURA` | chiude sotto un livello, volume alto, delta venditore | rosso |
| `RECLAIM` | chiude sopra un livello, volume alto, delta compratore | verde |
| `ASSORBIMENTO` | minimo sul livello, delta non negativo, chiusura nella meta' alta | azzurro |
| `RIFIUTO` | lo specchio, sul massimo | arancio |
| `DELTA GROSSO` | stampa isolata oltre la soglia, ovunque sia | viola |
| `attraversato` | livello passato senza i requisiti sopra | solo a video |

**I ruoli non sono fissi.** Rottura e reclaim si cercano su ogni livello in entrambi i versi,
perche' un supporto rotto diventa resistenza nel giro di una barra, ed e' proprio li' che serve il
segnale.

## 5. Gli Alert Che Tornano Sul Chart

Con `--push-alerts` ogni segnale forte diventa una linea effimera: **punteggiata, spessore 1,
etichetta prefissata da `!`**, colorata per tipo. Si distingue a colpo d'occhio dai livelli
strutturali, che sono `solid` o `dash`.

Due scelte che vale la pena conoscere:

- **La lista si ricostruisce sempre da capo** come strutturali + ultimi N alert, rileggendo il
  file. Siccome `POST /levels` sostituisce tutto, aggiungere una riga sola non e' possibile; e
  ripartire dal file garantisce che un alert non possa cancellare un livello dell'analisi.
- **Gli alert sono a scorrimento**, `--max-alerts 5` di default: il sesto fa cadere il primo.
  Senza il limite, dopo un'ora il chart e' illeggibile.

## Le Soglie Vanno Dichiarate

I default vengono dai percentili delle barre M1 di NQ del 15 settembre 2026. **Non sono una
costante del mercato**: in una seduta piu' sottile lasciano passare tutto, in una piu' densa non
scattano mai.

| Soglia | Default | Cos'e' |
|---|---|---|
| `--vol-alto` | 221 | p95 del volume per barra M1 |
| `--vol-medio` | 138 | p75: sotto, un attraversamento non si segnala |
| `--delta-forte` | 64 | p95 del \|delta\| per barra M1 |
| `--delta-enorme` | 100 | stampa isolata |
| `--vela-muta` | 15 | minuti di silenzio sullo stesso livello dopo un attraversamento |

`--calibra` le ricalcola sui percentili della seduta in corso. **Qualunque soglia usata va scritta
accanto al risultato che produce**, nel file della giornata: e' la regola 4 del
[percorso](../percorso-del-progetto.md), e qui si applica alla lettera.

Gli ultimi due filtri non sono cosmetici. Senza, un livello conteso genera una riga per ogni
oscillazione: nella prova sulle 243 barre del 15 settembre i segnali passavano da **10 a 29**, e
venti dei ventinove erano lo stesso livello attraversato avanti e indietro.

## Verificare Prima Di Armare

**Un filtro non provato non e' un filtro.** Prima di lasciarlo girare, fallo passare sulle barre
gia' note della giornata e guarda due cose: che prenda i momenti che avevi individuato a mano, e
quanti segnali produce in totale.

Nella prova del 15 settembre: 10 segnali su 243 barre, con dentro la rottura delle 06:21, quella
delle 09:55 e l'assorbimento delle 10:01. Se un filtro non ritrova cio' che hai gia' visto, e'
sbagliato; se ne produce trenta in quattro ore, e' rumore.

## Cosa Questo Strumento Non Fa

- **Non deriva livelli.** Li legge da un file prodotto dall'analisi.
- **Non decide soglie.** Hanno un default dichiarato e si passano da riga di comando.
- **Non tocca ordini ne' posizioni.** L'unica scrittura e' `POST /levels`, che disegna e basta.
- **Non e' un segnale operativo.** Dice che una condizione misurabile e' scattata su un livello, non
  che si debba fare qualcosa.

E' la stessa divisione dei ruoli dei livelli: **l'indicatore disegna, il client trasporta, la
derivazione resta nell'analisi.** Le soglie devono restare convenzioni dichiarate in un documento,
non numeri sepolti dentro una DLL.

## Un Vincolo Tecnico Che E' Costato Un Errore

Il campo `bar` restituito dal bridge **e' un indice di posizione, non un identificatore**: riparte
quando ATAS ricarica l'indicatore. Una prima versione di questo strumento ci teneva l'elenco delle
barre gia' valutate, e il 15 settembre alle 10:29, quando il chart e' passato da `04336da9` a
`1e003f54`, ha rivalutato tutta la mattina emettendo quattro segnali falsi.

Per riconoscere una barra si usa `time`, e non si valuta mai una barra piu' vecchia dell'ultima
gia' vista.
