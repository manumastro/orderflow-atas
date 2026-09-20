---
paths:
  - "FabioOrderFlow/tools/**"
  - "docs/research/giornate/**"
---

# Dati: Il Data Bridge

Contratto ed endpoint in `docs/research/metodo/contratto-data-bridge.md`, client
`FabioOrderFlow/tools/bridge.py`.

**I quattro vincoli che hanno gia' prodotto errori:**

- **`bar` e' un indice di posizione, non un identificatore**: riparte a ogni ricarica
  dell'indicatore. Per riconoscere una barra si usa **`time`**.
- **Il contratto continuo di ATAS non e' back-adjusted.** Si usano i contratti singoli o
  `build_continuous.py`, e **il controllo del rollover precede ogni altra misura**.
- **La speed of tape non esiste nel bridge.** Il proxy e' il volume per barra M1 contro la
  distribuzione recente, e **va dichiarato come proxy ogni volta che si usa**.
- **L'id del chart cambia a ogni ricarica**: si chiede a `/charts` o `/health`, non si cabla.

**Il delta esiste anche prezzo per prezzo.** `bridge.py candles --levels` da' la footprint di ogni
barra: e' lo strumento con cui si misura l'assorbimento del live — sforzo alto, risultato nullo.

**I big trades sono il filtro di volume nativo**, taratura di Fabio: **60 su NQ in cash**, 20-30 in
premarket. Il sottocomando e' `cumulative --min-volume`; **`bridge.py trades` non esiste**.
