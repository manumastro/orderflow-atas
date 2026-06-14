# Modello 1 — Trend Following (New York Session)

> Scalping intraday su mercati in **squilibrio** (trending)  
> Sessione: **New York 15:30 - 22:00** (ora italiana)

---

## 🎯 Quando Usarlo

| Condizione | Azione |
|------------|--------|
| Mercato **out of balance** | ✅ USA QUESTO MODELLO |
| Mercato in balance | ❌ Passa a Modello 2 (Mean Reversion) |

**Come capire se il mercato è out of balance:**
- Il prezzo ha **rotto la Value Area** del Volume Profile
- C'è un **trend chiaro** (Higher Highs / Lower Lows)
- Il Volume Profile mostra **due aree di balance separate** da una Low Volume Node

---

## 📐 Setup del Trade

### Step 1 — Identificare la Location

> "I want to buy in a low volume node — that's where price moves fast."

- Apri il **Volume Profile** dallo swing low allo swing high
- Identifica le **Low Volume Nodes** (zone con poco volume)
- Queste sono zone dove il prezzo si muove velocemente = entry ideale

### Step 2 — Il Trigger: Aggressione

> "When I see a big bubble of aggression, that's my trigger."

- Guarda il **footprint chart** o il **Time & Sales**
- Cerca **cluster di ordini aggressivi** nella direzione del trend
- **Filtro**: almeno **30 contratti** per trade su NQ

### Step 3 — Il Target: POC

> "I want the whole profit — I'm not scaling out."

- Target = **POC (Point of Control)** dell'area di balance precedente
- È dove "la maggior parte delle transazioni avviene"
- Il prezzo tende a tornare al POC dopo un movimento direzionale

### Step 4 — Lo Stop Loss

> "I put my stop right behind the aggression cluster."

- Stop **appena sopra/sotto** il cluster di aggressione
- Stop stretto = R:R alto (1:3 o superiore)

---

## 📊 Esempio Pratico

```
1. NQ in trend ribassista (out of balance)
2. Prezzo scende verso un Low Volume Node a 21.450
3. Vedi un cluster di 45 contratti in VENDITA (aggressione)
4. Il prezzo si ferma a 21.448 e inverte
5. ENTRA LONG a 21.450
6. Stop: 21.445 (sotto il cluster)
7. Target: 21.480 (POC del balance precedente)
8. R:R = 1:6 ✅
```

---

## 🔧 Indicatore ATAS: FabioTrendFollowing

### Segnali Rilevati

| Segnale | Descrizione | Output |
|---------|-------------|--------|
| **AGGRESSION** | Big trades ≥30 contratti | Freccia verde/rossa |
| **LOW VOLUME NODE** | Zone senza volume | Solo log |
| **ABSORPTION** | Big orders assorbiti senza follow-up | Freccia |
| **CVD DIVERGENCE** | Prezzo vs CVD divergenti | Solo log |

### Build + Deploy

```cmd
cd Modello-1-TrendFollowing\src
deploy.bat
```

### Parametri

| Parametro | Default | Descrizione |
|-----------|---------|-------------|
| Min Big Trade Size | 30 | Contratti minimi per "big trade" |
| Profile Lookback | 60 | Barre per il volume profile |
| Low Volume Node % | 30% | Soglia per LVN |
| Absorption Min Delta | 500 | Delta minimo per assorbimento |
| CVD Lookback | 20 | Barre per divergenza CVD |

---

## ⏰ Orari (Fuso Italiano)

| Fase | Ora | Cosa fare |
|------|-----|-----------|
| **Pre-market** | 15:00 - 15:30 | Analisi Volume Profile, identificare LVN |
| **Apertura NYSE** | 15:30 | Aspettare 15 min (troppo volatile) |
| **Miglior momento** | 15:45 - 17:30 | Massima volatilità, migliori segnali |
| **Sessione piena** | 17:30 - 21:00 | Trading attivo |
| **Chiusura** | 21:00 - 22:00 | Evitare (movimenti erratici) |

---

## ⚠️ Quando NON Tradere

- ❌ Mercato in balance → usa Modello 2
- ❌ Primi 15 minuti di apertura
- ❌ Ultimi 30 minuti prima della chiusura
- ❌ Giorni di notizie importanti (FOMC, NFP, CPI)
- ❌ Nessuna aggressione visibile (basso volume)

---

## 📝 Note di Fabio

> "When the market is out of balance, it wants to move to the next area of balance."

> "I want to buy in a low volume node — that's where price moves fast."

> "When I see a big bubble of aggression, that's my trigger."

> "I want the whole profit — I'm not scaling out."

> "I put my stop right behind the aggression cluster."

---

*Modello estratto da: "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)"*
