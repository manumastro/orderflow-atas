# Il Permesso Si Misura, Non Si Aspetta

Il gate Tier 01 del dossier misura la cosa giusta alla risoluzione sbagliata. Questo documento
dice come misurare la **stessa** cosa — l'accettazione oltre un bordo — in tre minuti invece che
in trenta, cosa si guadagna davvero, e cosa **non** si guadagna.

Nato il 16 settembre 2026, sera della prima riunione Fed di Warsh, da una richiesta dell'utente:
*«non ci debba essere sempre questa storia dell'M30»*.

---

## Cosa Dice La Fonte, E Cosa Ne Discende

Il dossier (righe 101-124) e' esplicito su **cosa** decide il permesso:

> *When price **closes outside** that range on the 30-minute close, the market has cast a
> directional vote. That vote is your permission.*

Il concetto e' l'**accettazione**: il prezzo non ha solo toccato oltre il bordo, ci **sta**. La
chiusura a 30 minuti e' il modo in cui il corso la misura — uno strumento, non la definizione.

**Il costo di quello strumento e' la latenza.** Una chiusura M30 puo' far aspettare fino a
ventinove minuti per sapere una cosa che il tape ha gia' detto, e non c'e' niente nella fonte che
dica che ventinove minuti di attesa fanno parte del segnale.

---

## La Misura Alternativa

```text
ACCETTAZIONE oltre un bordo, tutte insieme:
  1. N chiusure M1 consecutive oltre il bordo               (default 3)
  2. volume scambiato oltre il bordo >= soglia in lotti     (default 1.500 su NQ)
  3. nessun ritorno dall'altra parte nella finestra          (implicito in 1)

REVOCA, con isteresi:
  M chiusure M1 consecutive RIENTRATE                        (default 3)
```

`FabioOrderFlow/tools/permesso_di_fatto.py`. Il volume oltre il bordo e' **stimato** ripartendo
uniformemente il volume di ogni barra nel suo range: senza la footprint prezzo per prezzo non si sa
quanto di una barra a cavallo sia stato scambiato da che parte, e la stima basta a distinguere *"ci
e' passato"* da *"ci vive"*. Se servisse esatto, la strada e' `candles --levels`
([`la-footprint-e-il-delta-per-prezzo.md`](la-footprint-e-il-delta-per-prezzo.md)).

### L'isteresi non e' un parametro, e' la regola

Misurato sul 16 settembre, **senza** isteresi la regola da' **dodici** cambi di permesso, con sei
oscillazioni in quarantacinque minuti. Inservibile. **Con** l'isteresi ne da' **tre**.

Non e' un numero aggiustato sui dati di quel giorno — e' strutturale: **un permesso non deve
evaporare perche' una barra ha toccato indietro.** Concedere e revocare sono asimmetrici per
natura, e una regola che li tratta uguali oscilla su qualunque strumento.

E' anche la ragione per cui `--chiusure` e `--rientri` sono due parametri e non uno.

---

## Cosa Si Guadagna, Misurato

16 settembre 2026, NQZ6, IVB 29.373,25-29.481,50:

| | gate M30 | accettazione | scarto |
|---|---|---|---|
| **concede LONG** | 17:00 | 17:01 | **il gate e' piu' veloce di 1 minuto** |
| **revoca** | 19:29 | 18:05 | l'accettazione e' piu' veloce di **84 minuti** |
| **riconcede LONG** | — | 20:06 | il gate non aveva ancora parlato |

**Il guadagno sta nella revoca, e va detto perche' e' il contrario di quello che sembra.**

In **apertura** il gate M30 non e' battibile: nessuna regola di accettazione puo' parlare prima che
il prezzo abbia accettato, e le tre chiusure M1 che servono arrivano piu' o meno quando arriva la
chiusura M30. Chi si aspetta di anticipare il gate sull'apertura resta deluso.

In **revoca** il gate tiene in piedi un permesso fino alla prossima chiusura a 30 minuti anche
quando il prezzo e' gia' rientrato da un'ora e mezza. Quel giorno ha lasciato scritto LONG dalle
18:05 alle 19:29 su un prezzo che stava dentro l'IVB — **e nello stesso intervallo il delta cumulato
era negativo**.

**Errore da non ripetere.** Dal vivo avevo detto all'utente che *«il gate ha parlato due volte e
sempre in ritardo»*. Falso sulla prima: la misura l'ha smentito e la frase e' stata corretta. Erano
entrambi in ritardo rispetto alla **gamba**, ma quello e' un altro problema e non lo risolve nessuna
delle due regole.

---

## Come Convivono

**Il dossier resta la fonte. Il gate M30 resta registrato. Non ferma piu' niente.**

- `permesso_di_fatto.py` stampa **entrambi** accanto, con lo scarto. Dopo qualche seduta si ha il
  numero invece dell'impressione, ed e' l'unico modo per sapere se questa misura vale.
- Negli scenari il campo `verso` resta **`NESSUN PERMESSO`** anche quando l'accettazione concede:
  quel campo dice cosa concede la **fonte** a quella condizione, non cosa si intende fare. Stessa
  regola della deroga pre-gate in
  [`le-gambe-di-una-seduta.md`](le-gambe-di-una-seduta.md).
- Una lettura che usa il permesso di fatto **lo dichiara**, come si dichiara la finestra di misura
  accanto al numero. *"Permesso di fatto LONG dalle 20:06, gate M30 ancora DENTRO"* e' una frase
  completa; *"permesso LONG"* non lo e' piu'.

**Gerarchia.** Questa misura sta **sotto** al dossier. Se dopo N sedute lo scarto non paga — se le
letture prese sul permesso di fatto perdono dove il gate M30 avrebbe tenuto fuori — cade questa,
non il dossier.

---

## Le Soglie Sono Parametri Dello Strumento

Come tutto il resto in [`come-si-apre-un-asset.md`](come-si-apre-un-asset.md):

```text
NQZ6    3 chiusure / 3 rientri / 1.500 lotti oltre il bordo
ESZ6    da ricavare: la barra M1 e' 1,25 punti contro i ~5 di NQ
MCLV6   da ricavare
```

**Il `--volume` non si copia**: e' in lotti, e mille lotti su NQ e mille sul crude non misurano la
stessa partecipazione. Si sceglie dal volume tipico di tre barre M1 dello strumento in sessione
cash.

**`--from` deve cadere prima dell'inizio dell'IVB**, o il gate M30 di confronto non e' calcolabile:
stessa trappola di `scenari.py`, documentata in
[`sorveglianza-del-tape.md`](sorveglianza-del-tape.md), sezione *"`--from` Di `scenari.py` Decide
Anche Dov'E' L'IVB"*.

---

## Come Si Accende

Terzo `Monitor`, accanto a `scenari.py` e `sveglia_tape.py`:

```bash
python3 -u FabioOrderFlow/tools/permesso_di_fatto.py \
    --chart NQZ6 --alto 29481.5 --basso 29373.25 --from 2026-09-16T13:30
```

Parla poco per costruzione: solo quando il permesso **cambia**, e quando cambia il gate M30. Una
seduta intera sta in cinque righe.

Caso applicato: [`../giornate/2026-09-16.md`](../giornate/2026-09-16.md), sezione 8.
