---
paths:
  - "FabioOrderFlow/src/**"
  - "docs/atas/**"
---

# L'Indicatore: Build, Deploy, API

Procedura completa in `docs/research/build-e-deploy.md`.

```bash
cd FabioOrderFlow/src && ./deploy.sh
```

- Target `net10.0` **senza WPF**, requisito di ATAS X.
- **Su ATAS X non serve riavviare: basta il deploy**, e in replay la posizione non si perde.
  L'id del chart pero' cambia a ogni ricarica.
- **Un `git pull` non porta l'indicatore**: nel repo c'e' il sorgente C#, non la DLL. Finche' non
  si esegue `deploy.sh`, sul chart resta la versione precedente — e non e' distinguibile a occhio
  da un pull che non ha funzionato.
- **Gli orari di sessione sono dinamici**: si scrivono nel fuso del mercato (`09:30NY`, `08:00LON`,
  `18:00NY`) e si convertono per data con `Observation/LeSedute.cs`, l'unico posto che li definisce.
  Fino al 27 ottobre 2026 la cassa era 13:30Z-20:00Z fissa, e dal 2 novembre avrebbe misurato
  un'ora prima: `docs/research/metodo/gli-orari-si-scrivono-nel-fuso-del-mercato.md`.
- **Quando l'API ATAS non e' chiara, ispeziona gli assembly con reflection** invece di dedurla
  dalla documentazione: `docs/atas/` non sempre coincide con la build installata.
- **Non modificare `docs/atas/api/`** salvo necessita' tecnica concreta.

**Cosa calcola l'indicatore, e cosa no.** Calcola **il prezzo dei livelli**, dalle regole che
l'analisi deposita. Non decide **quali** regole valgono la pena, e non giudica il mercato: le
soglie e le finestre restano dichiarate in un file, fuori dal codice.
`docs/research/metodo/i-livelli-li-calcola-l-indicatore.md`
