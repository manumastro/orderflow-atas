# Fabio Mean Reversion (Modello 2) — Approccio Discrezionale

**Stato:** Non implementato programmaticamente. Mantenuto solo come riferimento per trading discrezionale.

## Contesto

Approccio mean reversion di Fabio basato su:
- **Zone di balance** (compression) dove il prezzo lavora in un dealing range ristretto
- **Breakout** della zona (vero vs fakeout)
- **Trapped traders** dalla parte sbagliata del breakout
- **Trigger volumetrico** (big orders/ball) per confermare il re-entry verso il POC

## Principi Chiave (da transcript)

1. **Sessioni London e New York** — le uniche con liquidità sufficiente per balance validi
2. **Profile volumetrico** — POC (Point of Control) e Value Area (VAH/VAL) per identificare dealing range
3. **Acceptance vs rejection** — il prezzo che torna ripetutamente nella zona indica balance
4. **Big orders** — 20+ contratti London, 30+ NY, confermano trapped traders e trigger l'inversione
5. **Target POC, stop oltre breakout** — mean reversion classico verso il centro della zona

## Perché non automatizzato

Le zone di balance richiedono:
- Validazione qualitativa (contesto market structure, sessione, momentum precedente)
- Interpretazione dei big orders live (non stimabile su storico)
- Discernimento breakout/fakeout che dipende da fattori non codificabili

L'approccio resta prezioso per **trading discrezionale**, ma non si presta a un indicatore ATAS retroattivo.

## Focus implementativo

**Modello 1 (Trend Following)** — implementato programmaticamente, retroattivo, basato su pattern rilevabili.

---

**File sorgente:** `src/FabioMeanReversion.cs` (placeholder vuoto)  
**Transcript originale:** `../Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/`
