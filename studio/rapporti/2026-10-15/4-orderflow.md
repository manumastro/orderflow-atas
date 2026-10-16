# Fase 4 · L'Order Flow Delle Quattro Tab · 2026-10-15 (aggiornamento delle 19:52)

**Stato**: parziale (cassa di oggi in corso fino alle 22:00; il composito di sola cassa ha 30 sedute su 90)
**Dati**: `tab-ruoli`, `quattro-tab`, `sedute-NQZ6`, `ore-NQZ6`, `range-NQZ6`, `pannello`, `giro`, `confluenze`. Raccolta
**19:52** ora italiana (`raccolta.json`; pannello 17:52Z, barra in formazione); GEX NQZ6 19:30 (OI di ieri); COT al 6/10
(9 giorni). Errori: nessuno. La sez. 4bis del giro ripete il quadro delle 18:43: non usata. Precedente: `precedenti/1952/`.

**In una riga**: la cassa di oggi ha allargato il valore fino a toccare quello di ieri (19 punti, non piu' staccato); il
prezzo sta sopra il POC di oggi e 8,25 sotto il VAL di ieri, CVD e prezzo sul massimo; l'indicatore misura **balance**.

Convenzioni: cassa 15:30-22:00 italiane; **POC** = prezzo col volume massimo; **valore** = 70% del volume attorno al POC
(VAL basso, VAH alto); **delta** = aggressori in acquisto meno in vendita, **CVD** il suo cumulato; **shift** = VAL **e** VAH
piu' bassi del giorno prima (`profile-framing.md` §2); **staccato** = aree che non si toccano. Prezzi NQZ6.

## Cosa E' Cambiato Dalle 18:43

| misura | 18:43 | 19:52 | fonte |
|---|---|---|---|
| prezzo | 29.946,25 | **30.073,75** (+127,50) | pannello |
| valore di cassa oggi | 29.792-30.060, POC 29.960, staccato di 22 | **29.800-30.101**, POC 29.959; **sovrapposto di 19** | `oggi_cassa` |
| regime | CHOPPY, efficienza 0,10 | **BALANCE**, 0,17 sulle 30 barre 19:23-19:52 | pannello |
| CVD dalle 15:30 | +8.960; CVD 95%, prezzo 74% | **+11.815** (+1,8%); CVD 100%, prezzo 94% | pannello |
| in gioco / veti / 30 barre range | niente / 2 veti / 9 minuti | **VAL cash ieri 30.082**, da sotto / nessuna riga VETI / **19 minuti** | pannello, TAB 4 |

## I Fatti

**Tab 1 · Composito** (`tab-ruoli`, `sedute-NQZ6`): il prezzo e' 382,25 sotto il VAL di cassa, 256,25 sopra il POC di 24 ore.

| versione | VAL | VAH | POC | zona |
|---|---|---|---|---|
| 24 ore, aggiustato (+299,25 prima del 11/09; grezzo 28.410-30.530) | 29.340 | 30.805 | 29.817,50 | fair value |
| sola cassa, 20 sedute dal 17/09 / "90", **30 sedute** dal 03/09 (6 prima del rollover) | 30.458 / 30.456 | 31.177 / 31.309 | 30.770 | **discount** |

**Tab 2 · Settimana** (`sedute-NQZ6`, M1 fasce da 1; tutte chiudono dentro il valore). Dal 12/10 ogni seduta ha shift giu' e valore staccato; oggi no:

| seduta | VAL-VAH | POC | delta | staccato | shift |
|---|---|---|---|---|---|
| 14/10 | 30.082-30.478 | 30.173 | +530 | si' | giu' |
| **15/10, in corso** | **29.800-30.101** | **29.959** | **+11.815** su 645.337 | **no** | giu' |

Profili da 10 punti (solo fasce sopra il 2% del volume): **14/10** nodi 30.140-30.250 e 30.470-30.510,
nulla fra 30.250 e 30.470; **oggi** nodi 29.950-30.030 (29.990-30.000 al 5,5%) e 29.760-29.840, nulla fra
29.840 e 29.910 e sopra 30.030. **Scala dei POC di cassa**: sotto 29.959 (oggi, -114,75) e 29.730-29.737
(18 e 17/09); sopra **30.173** (14/10, seduta della rottura, +99,25), 30.609-30.634, 30.745-30.783,
30.835-30.922, 31.065-31.530. Nessun POC fra 30.173 e 30.609.

**Tab 3 · Seduta** (`sedute-NQZ6`, `ore-NQZ6`, `giro`, `pannello`; ora italiana)

- **Notte 00:00-15:30** (collettore): VAL 29.753, POC 29.815, VAH 29.898, 30.158,50-29.711, delta -1.162.
  La notte del pannello (finestra non scritta) ha POC 29.920, VAH 29.982, VAL 29.844; Europa POC 29.815.
- **Ore**: 15:00 +2.416 (min 29.622,50); 16:00 +4.594; 17:00 +2.414; 18:00 +535; 19:00 (53 min) **+2.106**,
  max 30.102,25. Mezz'ora 19:30 (aperta) **+1.572**, 30.000 -> 30.074,75. Volume per mezz'ora dalle 18:00:
  41.967, 38.520, 28.040, 29.400. Dalla chiusura di ieri (30.185): -111,25 (-0,369%); range 20 sedute: al 23%.
- **Volatilita' realizzata** (rendimenti a 5 minuti, annua): 4 ore 32,9%, 24 ore 22,7%, 48 ore 20,8%. Tape M1
  chiuso: 60 min +2.836 (+4,4%). Struttura oraria (giro): "ultimi due swing H1 massimi e minimi decrescenti"
  (29.978,25 e 29.899,50; 29.823,50 e 29.754); il prezzo e' sopra entrambi i massimi.

**Regime e velocita', come li misura l'indicatore** (`pannello.json`, 19:52):

| riga | valore |
|---|---|
| REGIME | **BALANCE**; **ultime 30 barre, 19:23-19:52**: efficienza 0,17 (netto 71,75 su strada 422,50), 1 rottura rientrata, copertura si' |
| VELOCITA' (proxy, non speed of tape) | aperta: **morta**, 497 lotti, 2° percentile su 60; **chiusa 19:51: 1.283 lotti, 76° percentile**; 2 big trade 60+, netto +174 |
| VALORE / IN GIOCO | dentro NY oggi, sopra il POC / **VAL cash ieri 30.082**, 8,25 sotto, **arrivato da sotto** |

Efficienza = spostamento netto / strada; la riga operativa sotto il regime non si riporta. CVD: "CVD e prezzo
insieme sul massimo: i compratori sono seguiti". **Livelli dalle 15:30** (`alLivello`): VAL ieri 30.082: 3 tocchi,
tiene 1, passa 2, 325 lotti, delta +101. POC 29.959: 39 tocchi, 5.310 lotti (il piu' scambiato), delta -142.
VAL 29.800: tiene 17 su 30. POC 30.173: 0 tocchi.

**Tab 4 · Esecuzione** (Range 40 tick = 10 punti; `quattro-tab`, `range-NQZ6`): ultime barre 39, 24, 51 s. Ultime 30: 16 su,
14 giu', delta +1.430 (+5,6%), 30.017,25-30.102,25; durata mediana **33,0 s**, fra il 75° (24,8) e il 90° (40,0) di 600 barre.

## Cosa Dicono Per NQ

1. **Il valore scende ancora, ma non si stacca piu'.** Shift giu' per la quinta seduta (§2), pero' il VAH
   di oggi entra di 19 punti in quello di ieri. Per §3 i profili sovrapposti si uniscono: blocco 14-15/10
   da 29.800 a 30.478. Sovrapposizione piccola, cassa aperta: **osservazione**, da rileggere alle 22:00.
2. **Il VAL di ieri e' la strada in gioco** `[1 · 1:19:30]`: raggiunto dal basso, passato 2 volte su 3, ora
   8,25 sotto. Oltre: il POC della rottura 30.173 `[1 · 24:35]`, poi 30.250-30.470 senza fasce sopra il 2%
   nel profilo del 14/10, zona di transito (§6).
3. **Il delta ora ottiene prezzo.** Alle 18:43 CVD 95% e prezzo 74%; ora 100% e 94%; la mezz'ora delle
   19:30 fa +1.572 per 74,75 punti. Lo stampa il pannello: non e' una ricostruzione.
4. **Regime balance, misurato su 30 minuti**: prezzo chiuso nel valore, rotture che rientrano (`i-tre-modelli-del-live-q1.md`
   §7); barre range piu' lente (33 s). Tre scale (§10, `[5 · 55:05]`): cassa discount, 24 ore fair value, 20 sedute al
   23%, **basso nella curva** `[5 · 1:01:05]`. La settimana resta di momentum (§5): **il regime si dichiara, il setup no**.

## I Livelli E Le Date

| prezzo o data | da dove viene | a cosa serve arrivarci |
|---|---|---|
| 30.456-30.770 | VAL e POC del composito di cassa | rientrare dal discount nella distribuzione di cassa |
| 30.478 | VAH 14/10, bordo alto del blocco unito | uscire dal blocco 14-15/10 dall'alto |
| 30.250-30.470 | fasce sotto il 2% del 14/10 | zona di transito dentro il valore di ieri |
| 30.173 | POC 14/10, seduta della rottura | il primo punto del ritorno; 0 tocchi oggi |
| 30.082-30.102 | VAL ieri; VAH e massimo di oggi | la strada in gioco: accettare o rifiutare il valore di ieri |
| 29.950-30.030 | nodo alto di oggi, POC 29.959 | il centro del valore in costruzione |
| 29.840-29.910 | fasce sotto il 2% di oggi | collo fra i due nodi della cassa |
| 29.777-29.815 | VAL Europa, VAL cassa 29.800, POC notte | bordo basso di oggi: sotto si lascia il valore della seduta |
| 29.622,50 | minimo di cassa (15:30-16:00) | estremo delle 20 sedute: sotto, nessun valore di cassa recente |
| 15/10 22:00; 16/10 05:30, 21:30 | chiusura cassa e 0DTE; Warsh (Fed); COT al 13/10 (giro) | il profilo di oggi si chiude (§1, §3 da rileggere); il COT non vedra' il 14 e il 15/10 |

## Cosa Non Sappiamo

- **lacuna del collettore: composito di cassa a 90 giorni** con 30 sedute, senza dire se le 6 prima del rollover
  sono aggiustate; **finestra della notte del pannello** (POC 29.920 contro 29.815 del collettore).
- **lacuna del collettore: definizioni di "copertura" e "rottura rientrata"**, unita' delle 30 barre; **veti**
  (riga assente: zero o omessi?); **big trade per livello** tornati `null`; **regola di swing del giro**;
  **profili da 10 sotto il 2%** (il collo si vede, la profondita' no).
- Discrepanze: POC 14/10 30.173 (M1) / 30.187 (Range); rollover +299,25 (tab) / +293,75 (`profile-framing.md`);
  prezzo 30.073,75 (pannello) / 30.074,75 (giro, un istante dopo). Durate range con le pause; niente footprint.

## Per Il Quadro

1. **Gamma** (19:30, conv. A): call e put wall 30.000 nel nodo alto di oggi; zero gamma 0DTE 30.278 nella zona
   sotto il 2% del 14/10; zero gamma di tutte 30.499 accanto ai VAL di cassa 28/09 e 21/09 (`confluenze.json`).
2. **Volatilita'**: realizzata 4 ore 32,9% contro implicita ATM 0DTE 22,4%: coerente con la GEX negativa?
3. **COT** (report 22/09, 29/09, 06/10): valori di cassa tutti sopra il prezzo; `stesso_dato` in `confluenze.json`.
4. **Macro**: delta di cassa positivo in ogni ora dalle 15:00; da rileggere dopo Warsh (16/10 05:30).
