---
paths:
  - "quant/**"
---

# Il Framework Quantitativo

La metodologia e' `quant/la-metodologia-istituzionale.md` (come da transcript, comanda); come il
framework la realizza e cosa aggiunge, `quant/il-metodo-quantitativo.md`; i dati
`quant/i-dati-da-atas.md`. Un'aggiunta del framework si dichiara come aggiunta, mai come se la
dicesse il transcript.

**Compilare e provare** (da `quant/src`):

```bash
dotnet build -c Release && dotnet test Quant.Test -c Release
Quant.Cli/bin/Release/net10.0/quant.exe <comando>
```

**I cinque vincoli che hanno gia' prodotto errori, qui o nella fase 40R:**

1. **Il registro e la divisione sono veri.** `quant/registro/prove.jsonl` conta le varianti provate
   e `divisioni.json` fissa il confine dentro/fuori. Le prove sul mercato sintetico, i test e gli
   esperimenti usano `QUANT_REGISTRO`, `QUANT_RISULTATI` e `QUANT_DATI` puntati alla scratchpad.
   **Mai `quant divisione --fissa` su NQ senza che l'utente lo chieda**: si fissa una volta, quando
   la storia lunga e' scaricata.
2. **`valida --fuori` consuma il fuori campione.** Si lancia a regole congelate, una volta per
   variante, e solo quando l'utente lo vuole.
3. **Il look-ahead non si scrive**: una strategia legge solo dal `Contesto`. Ogni dato precalcolato
   che le si passa deve dipendere solo dal passato (vedi `Contesto.GiornoPrecedente`).
4. **Il nullo non deve sbirciare.** Il nullo del quando pesca anche la giornata: spostare
   un'operazione dentro la sua giornata le fa catturare il movimento che l'ha generata. Dopo ogni
   modifica a motore o validazione: passeggiata casuale, nessuna strategia deve passare, D1 a
   p <= 0,05 intorno al 5% dei semi.
5. **Assembly e cartelle**: la libreria si chiama `Quant.Framework` e l'eseguibile `quant`. Su
   Windows `quant.dll` e `Quant.dll` nella stessa cartella si sovrascrivono.
