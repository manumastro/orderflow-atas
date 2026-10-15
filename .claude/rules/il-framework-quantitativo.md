---
paths:
  - "quant/**"
---

# Il Framework Quantitativo

La metodologia e' `quant/metodo/la-metodologia-istituzionale.md` (come da transcript, comanda); la
mappa e le aggiunte del framework, `quant/il-metodo-quantitativo.md`; una pagina per fase in
`quant/metodo/<n>-*.md`; i dati `quant/metodo/i-dati-da-atas.md`. Le idee: `quant/studi/<idea>/`.
Un'aggiunta del framework si dichiara come aggiunta, mai come se la dicesse il transcript.

**Compilare e provare** (da `quant/framework`):

```bash
dotnet build -c Release && dotnet test Quant.Test -c Release
Quant.Cli/bin/Release/net10.0/quant.exe <comando>
```

**I cinque vincoli che hanno gia' prodotto errori, qui o nella fase 40R:**

1. **Il registro e la divisione sono veri.** `quant/registro/prove.jsonl` conta le
   varianti provate e `divisioni.json` fissa il confine dentro/fuori. Le prove sul mercato sintetico, i test e gli
   esperimenti usano `QUANT_REGISTRO`, `QUANT_RISULTATI` e `QUANT_DATI` puntati alla scratchpad.
   **La divisione di NQ e' fissata** (15 ottobre 2026, dentro fino al 31 dicembre 2024): non si
   sposta. **Il fuori campione di NQ e' dal 2025 in poi**, e ogni sguardo ai dati 2025-2026 (anche
   esplorativo, anche su `NQ/m5`) lo consuma: si guarda solo con `valida --fuori`.
2. **`valida --fuori` consuma il fuori campione.** Si lancia a regole congelate, una volta per
   variante, e solo quando l'utente lo vuole.
3. **Il look-ahead non si scrive**: una strategia legge solo dal `Contesto`. Ogni dato precalcolato
   che le si passa deve dipendere solo dal passato (vedi `Contesto.GiornoPrecedente`).
4. **Il nullo non deve sbirciare.** Il nullo del quando pesca anche la giornata: spostare
   un'operazione dentro la sua giornata le fa catturare il movimento che l'ha generata. Dopo ogni
   modifica a motore o validazione: passeggiata casuale, nessuna strategia deve passare, D1 a
   p <= 0,05 intorno al 5% dei semi. Dopo ogni modifica al simulatore di contratto:
   `quant contratto verifica` (i criteri sigillati in `quant/studi/contratto-prop/5-validazione/criteri/`).
5. **Assembly e cartelle**: le librerie sono `Quant.Comune`, `Quant.Regole`, `Quant.Strategie`,
   `Quant.Motore`, `Quant.Validazione`, `Quant.Studi`, e l'eseguibile `quant`. Mai una libreria
   che si chiami `Quant`: su Windows `quant.dll` e `Quant.dll` nella stessa cartella si
   sovrascrivono. **Il framework** sta in `quant/framework/<Progetto>/`, nella forma standard di
   .NET; **le dipendenze vanno in un verso solo** (Comune -> Regole -> Strategie -> Motore ->
   Validazione), e un pezzo che ha bisogno di un livello successivo e' nel posto sbagliato. **Il
   codice di un'idea** (una strategia, un contratto) sta nello studio, `quant/studi/<idea>/3-codice/`,
   namespace `Quant.Studi`: lo compila `Quant.Studi` e si registra in `Catalogo.cs` o
   `CatalogoContratti.cs`. I rapporti di uno studio vanno in `quant/studi/<idea>/5-validazione/risultati/`.
