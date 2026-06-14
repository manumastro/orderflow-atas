# Modello 2 — Mean Reversion (London Session)

> Scalping intraday su mercati in **equilibrio** (range-bound)  
> Sessione: **London 09:00 - 17:30** (ora italiana)  
> Migliore in: **Estate** (mancano i big players istituzionali)

---

## 🎯 Quando Usarlo

| Condizione | Azione |
|------------|--------|
| Mercato **in balance** | ✅ USA QUESTO MODELLO |
| Mercato out of balance | ❌ Passa a Modello 1 (Trend Following) |

**Come capire se il mercato è in balance:**
- Il Volume Profile mostra una **distribuzione a campana** (forma normale)
- La **Value Area** è ben definita
- Il prezzo oscilla tra **Value Area High** e **Value Area Low**
- Non ci sono rotture significative

---

## 📐 Setup del Trade

### Step 1 — Identificare il Balance

- Il Volume Profile mostra una **distribuzione a campana**
- La **Value Area** è ben definita (70% del volume)
- Il prezzo oscilla tra VAH e VAL

### Step 2 — Aspettare il Fallimento del Breakout

> "When it tries to break out and fails — that's my entry."

**Pattern chiave:**
1. Il prezzo **rompe** sopra/sotto la Value Area
2. Il breakout **fallisce** (wick lunga, chiusura dentro il range)
3. I trader che hanno inseguito il breakout sono **intrappolati**
4. Il prezzo si muove nella direzione opposta (squeeze)

### Step 3 — Il Trigger: Secondo Drive + Aggressione

> "I wait for the second drive, then look for aggression confirmation."

- Dopo il primo fallimento, aspetta un **secondo tentativo**
- Sul secondo tentativo, cerca **aggressione nella direzione opposta**
- Se vedi aggressione che **respinge** il breakout = conferma

### Step 4 — Il Target: POC

> "The POC is where the most transactions happened — price wants to go back there."

- Target = **POC del balance area**
- È il punto di equilibrio dove il prezzo tende a tornare

---

## 📊 Esempio Pratico

```
1. NQ in consolidamento (in balance) tra 21.400 e 21.500
2. POC = 21.450
3. Prezzo rompe sopra 21.500 (VAH) → fallisce → chiude a 21.490
4. Secondo tentativo → rompe di nuovo → vedi aggressione in vendita
5. ENTRA SHORT a 21.495
6. Stop: 21.510 (sopra il breakout fallito)
7. Target: 21.450 (POC)
8. R:R = 1:3 ✅
```

---

## 🔧 Indicatore ATAS: FabioMeanReversion

### Segnali Rilevati

| Segnale | Descrizione | Output |
|---------|-------------|--------|
| **FAILED AUCTION** | Breakout fallito (wick >60%) | Freccia blu/arancione |
| **SQUEEZE** | Trader intrappolati che chiudono | Freccia |
| **ABSORPTION** | Big orders assorbiti senza follow-up | Freccia |
| **CVD DIVERGENCE** | Prezzo vs CVD divergenti | Solo log |

### Build + Deploy

```cmd
cd Modello-2-MeanReversion\src
deploy.bat
```

### Parametri

| Parametro | Default | Descrizione |
|-----------|---------|-------------|
| Wick Ratio | 0.6 | Rapporto wick/corpo per failed auction |
| Min Big Trade Size | 20 | Soglia più bassa per London |
| Absorption Min Delta | 300 | Soglia più bassa per London |
| Squeeze Lookback | 5 | Barre per rilevare squeeze |
| CVD Lookback | 20 | Barre per divergenza CVD |

---

## ⏰ Orari (Fuso Italiano)

| Fase | Ora | Cosa fare |
|------|-----|-----------|
| **Pre-market** | 08:30 - 09:00 | Analisi Volume Profile, identificare balance |
| **Apertura London** | 09:00 | Aspettare 15 min |
| **Miglior momento** | 09:15 - 11:00 | Migliori segnali di mean reversion |
| **Sessione piena** | 11:00 - 16:30 | Trading attivo |
| **Chiusura** | 16:30 - 17:30 | Ridurre esposizione |

---

## ⚠️ Quando NON Tradere

- ❌ Mercato out of balance → usa Modello 1
- ❌ Primi 15 minuti di apertura
- ❌ Ultimi 30 minuti prima della chiusura
- ❌ Giorni di notizie importanti (FOMC, NFP, CPI)
- ❌ Primo breakout (potrebbe essere reale — aspetta il fallimento)

---

## 📝 Note di Fabio

> "When the market is in balance, I do one thing. When it's out of balance, I do the opposite."

> "When it tries to break out and fails — that's my entry."

> "I wait for the second drive, then look for aggression confirmation."

> "The POC is where the most transactions happened — price wants to go back there."

> "When trapped traders are forced to close, the market accelerates."

---

*Modello estratto da: "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)"*
