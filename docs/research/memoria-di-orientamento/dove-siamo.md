---
name: dove-siamo
description: "Lo stato del lavoro a ottobre 2026: lo scope e' il metodo quantitativo (framework C# in quant/), NQ, dati solo da ATAS, fino alla validazione"
metadata:
  node_type: memory
  type: project
  originSessionId: 9e6fa3fe-14f2-4ed2-af51-b266c885bbe4
  modified: 2026-10-11T12:00:00.000Z
---

**Aggiornato all'11 ottobre 2026.** Se la data e' vecchia di settimane, verifica prima di
fidarti: questo file orienta, non comanda. Le regole stanno in `CLAUDE.md` e in
`quant/il-metodo-quantitativo.md`.

## Dove Si E' Arrivati

**L'11 ottobre 2026 lo scope e' cambiato**, su decisione esplicita dell'utente: si studia e si
costruisce **il metodo quantitativo istituzionale** (idea da un paper -> regole -> codice ->
backtest -> validazione), dal transcript di Matteo Conte in `quant/metodo/fonti/`. Il corso di Fabio
diventa fonte di idee e supporto. Vedi [[la-storia-del-progetto]].

**Why:** l'utente ha un background da programmatore e vuole un framework dalla A alla Z, con la
**velocita' di esecuzione come requisito** ("python e' troppo lento"): il framework e' in C# .NET 10.

**How to apply:** prima di lavorare in `quant/` leggi `quant/il-metodo-quantitativo.md`: e' la mappa,
organizzata **per idea**, scelta dall'utente l'11 ottobre 2026 dopo aver trovato poco intuitiva
quella per fase: `metodo/` (come si fa, una pagina per fase), `framework/` (il codice C#), `paper/`
(la biblioteca, per filone), `studi/<idea>/` (un'idea dalla A alla Z, fasi numerate), `registro/`,
`dati/`. Gli otto
obblighi della lettura discrezionale valgono solo quando l'utente chiede una lettura dal vivo.

## Le Decisioni Dell'Utente

- **Solo NQ**, per ora ("poi vediamo").
- **Dati solo da ATAS**, dai tick al Level 2 completo; la domenica la connessione e' rifiutata.
- **Fino alla validazione**: l'esecuzione si pensa quando il framework e' buono.
- **Le prime idee sono tre paper sulle prop firm** (Tomas Fernandez, Lim, Youngblood): letti,
  analizzati (`quant/paper/prop-firm/<paper>/analisi.md`) e usati per studiare **Tradeify Growth 50K**, la
  firma dell'utente (lo studio `quant/studi/contratto-prop/`).

## Cosa E' Aperto

- La **profondita' storica** dei dati ATAS dipende dall'abbonamento: va misurata al primo giorno
  di connessione (`quant/metodo/i-dati-da-atas.md`, il piano). Oggi l'archivio ha 50 giornate M1
  di NQZ6, ma **solo 22 sedute complete** (dal 10 settembre): prima NQZ6 non era il contratto principale.
- **Storia lunga di NQ dal 15 ottobre 2026**: M1 dal continuo di ATAS, dal 12 aprile 2017, 2.365
  sedute complete; rollover in `NQ/rollover.json` (`quant/metodo/i-dati-da-atas.md`). La
  **divisione e' fissata**: dentro fino al 31 dicembre 2024, fuori dal 2025.
- L'hook del giro d'orizzonte e' **spento** dall'11 ottobre 2026, su richiesta dell'utente: per
  una lettura dal vivo si lancia a mano (`docs/research/metodo/il-giro-d-orizzonte.md`).
