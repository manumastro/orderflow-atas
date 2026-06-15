# Modello 2 — Mean Reversion (London Session)

> Scalping intraday su mercati **in balance** (consolidamento)  
> Sessione: **London 09:00 - 17:30** (ora italiana)

---

## 🎯 Quando Usarlo

| Condizione | Azione |
|------------|--------|
| Mercato **in balance** / consolidamento | ✅ USA QUESTO MODELLO |
| Mercato out of balance / trending | ❌ Passa a Modello 1 (Trend Following) |

**Come capire se il mercato è in balance:**

- Candele **compresse**, prezzo oscilla in un range
- Il Volume Profile mostra un'area di valore **protettiva** (VAH/VAL tengono)
- Il prezzo transa attorno al **POC** (bulk of auction)
- Nessun momentum direzionale sostenuto fuori dal range

---

## 📐 Documentazione

| Documento | Contenuto |
|-----------|-----------|
| [FabioMeanReversion.md](FabioMeanReversion.md) | **Spec completa** — teoria, input, output, state machine, trigger, parametri |
| [AGENTS.md](../../AGENTS.md) | Stato implementazione codice ATAS |

---

## 🔧 Indicatore ATAS: FabioMeanReversion

### Build + Deploy

```cmd
cd Modello-2-MeanReversion\src
deploy.bat
```

> L'indicatore è in fase di riscrittura. Vedi [AGENTS.md](../../AGENTS.md) per lo stato corrente.

---

## ⏰ Orari (Fuso Italiano)

| Fase | Ora | Cosa fare |
|------|-----|-----------|
| **London open** | 09:00 | Iniziare a costruire il session profile |
| **Compressione visibile** | 09:30 - 12:00 | Verificare BALANCE_READY |
| **Miglior momento** | 10:00 - 16:00 | Mean reversion in range |
| **Overlap NY+London** | 15:30 - 17:30 | Contesto più volatile — cautela |
| **Chiusura London** | 17:30 | Fine finestra preferita |

---

## ⚠️ Quando NON Tradere

- ❌ Mercato out of balance → usa Modello 1
- ❌ Compressione non ancora identificabile (*"you don't identify it immediately"*)
- ❌ Breakout con momentum sostenuto fuori VAH/VAL
- ❌ Giorni di news / gap da ribilanciare
- ❌ Profile con volume insufficiente

---

## 📝 Frase Chiave

> *"Market state is consolidation and is when the profile is protecting from breaking here and breaking here."*

---

*Modello estratto da: "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)"*