# AGENTS.md — orderflow-atas

Indicatori/strategie order flow C# per ATAS (futures NQ/ES).

---

## 📁 Progetto: FabioOrderFlow

**Path:** `Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/FabioOrderFlow/`  
**File:** `src/FabioOrderFlow.cs`  
**DLL:** `FabioOrderFlow.dll`

Indicatore unificato che combina strategie order flow complementari basate su balance zones e volume profile.

---

## 📚 Documentazione

### Overview
**File:** [`FabioOrderFlow/docs/README.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/README.md)  
Panoramica completa: strategie, architettura, sessioni, configurazione.

### Strategie
1. **London Mean Reversion** → [`docs/LondonMeanReversion.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/LondonMeanReversion.md)  
   Fade London fakeouts, sweep → rejection → POC
   
2. **Post-London Impulse** → [`docs/PostLondonImpulse.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/docs/PostLondonImpulse.md)  
   Follow impulse to low volume nodes (future)

### Architettura Moduli
**File:** [`FabioOrderFlow/src/modules/README.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/src/modules/README.md)  
Struttura moduli, shared/specific, dipendenze, implementazione.

### Analisi Codice
**File:** [`BalanceZoneTracker/CODE-ANALYSIS.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md)  
Breakdown dettagliato: core (800 lines) vs mean reversion (600 lines), extraction plan.

### Riferimenti
- **Sessioni:** [`CHIAREZZA-DEFINITIVA.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/CHIAREZZA-DEFINITIVA.md)
- **Transcript:** [`Trading LIVE... .txt`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy).txt)

---

## 🔧 Build & Deploy

```bash
cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/FabioOrderFlow/src"
dotnet build -c Release
./deploy.bat  # Windows - copia DLL in %APPDATA%\ATAS\Indicators\
./deploy.sh   # Linux
```

**Output:**
- DLL: `%APPDATA%\ATAS\Indicators\FabioOrderFlow.dll`
- Log: `%APPDATA%\ATAS\Logs\FabioOrderFlow.log`

---

## ⚙️ Configurazione

```csharp
// Parametri disponibili in ATAS
public bool EnableLondonMeanReversion { get; set; } = true;
public bool EnablePostLondonImpulse { get; set; } = false;
public bool EnableLiveFootprintFirst { get; set; } = true;
```

**Note:** 
- `EnableLondonMeanReversion` attualmente non condiziona logica (sempre attiva in BalanceZoneTracker)
- `EnablePostLondonImpulse` non implementato
- Codice MR è integrato in BalanceZoneTracker, delimitato da commenti per futura extraction
- Vedi [`BalanceZoneTracker/CODE-ANALYSIS.md`](Trading%20LIVE%20with%20the%20%231%20Scalper%20in%20the%20WORLD%20(EXTREME%20Accuracy)/FabioOrderFlow/src/modules/shared/BalanceZoneTracker/CODE-ANALYSIS.md)

---

## 📖 Documentazione ATAS

- **Log reading:** `docs/atas/log-reading.md`
- **API ATAS:** `docs/atas/api/`

---

## 🎯 Stato Corrente

**Sistema operativo:**
- London Mean Reversion funzionante (integrato in BalanceZoneTracker)
- Post-London Impulse documentato (da implementare)

**Prossimi step:**
- Implementare Post-London Impulse, oppure
- Estrarre London Mean Reversion da BalanceZoneTracker (opzionale)

Vedi `docs/README.md` per roadmap dettagliata.
