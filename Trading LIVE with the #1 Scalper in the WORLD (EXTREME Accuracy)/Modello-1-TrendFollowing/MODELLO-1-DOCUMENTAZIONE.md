# Modello 1 — Trend Following Fabio

## 0. Scopo

Questo documento è la mappa centrale del Modello 1.

Non contiene tutti i dettagli implementativi: ogni modulo ha il proprio documento dentro `src/modules/<NomeModulo>/README.md`.

Regola operativa:

```text
MODELLO-1-DOCUMENTAZIONE.md = mappa, principi, roadmap globale
src/modules/*/README.md    = design operativo del singolo modulo
```

---

## 1. Principio del Modello

Il Modello 1 è un indicatore ATAS trend-following per futures ES/NQ basato sul metodo di Fabio Valentino.

Principio fondamentale:

```text
Trade solo quando il mercato è OUT OF BALANCE.
Non cercare trend-following dentro una balance/range.
```

Pipeline target:

```text
London balance reference
→ NY breakout / out-of-balance
→ impulse profile
→ low volume node
→ aggression trigger
→ entry / stop / target
→ confirmation layer
```

Il codice deve partire da base pulita e implementare direttamente questa pipeline, senza recuperare vecchi detector isolati.

---

## 2. Metodo Fabio — Sintesi Operativa

### 2.1 Balance

Una balance è una zona in cui il mercato accetta prezzo e volume.

Riferimenti:

- `POC`: prezzo con massimo volume.
- `VAH`: limite alto della value area.
- `VAL`: limite basso della value area.
- `Value Area`: area contigua attorno al POC che contiene circa il 70% del volume.

### 2.2 Out of Balance

Il mercato è out-of-balance quando rompe e accetta fuori dalla value area precedente.

Regola iniziale:

```text
Bullish out-of-balance = 2 close consecutive sopra VAH
Bearish out-of-balance = 2 close consecutive sotto VAL
```

### 2.3 Sessioni

Decisione operativa:

```text
London = balance reference
New York RTH = trading / breakout window
```

Le sessioni devono usare `TimeZoneInfo`, non offset hardcoded.

---

## 3. Struttura Directory

```text
Modello-1-TrendFollowing/
├── MODELLO-1-DOCUMENTAZIONE.md
└── src/
    ├── FabioTrendFollowing.cs
    ├── FabioTrendFollowing.csproj
    ├── deploy.bat
    └── modules/
        ├── README.md
        ├── BalanceZoneTracker/
        │   └── README.md
        ├── ImpulseProfiler/
        │   └── README.md
        ├── LowVolumeNodeDetector/
        │   └── README.md
        ├── AggressionDetector/
        │   └── README.md
        ├── TradeManager/
        │   └── README.md
        ├── ConfirmationLayer/
        │   └── README.md
        └── VisualRenderer/
            └── README.md
```

---

## 4. Moduli

### 4.1 BalanceZoneTracker

Documento: `src/modules/BalanceZoneTracker/README.md`

Scopo:

- costruire la London balance reference;
- calcolare e congelare `POC`, `VAH`, `VAL`;
- monitorare NY per breakout confermato;
- esporre stato `BALANCE_READY`, `BREAKOUT_PENDING`, `OUT_OF_BALANCE`.

Questo è il primo modulo da implementare.

### 4.2 ImpulseProfiler

Documento: `src/modules/ImpulseProfiler/README.md`

Scopo:

- partire dal breakout confermato;
- costruire il volume profile dell'impulso;
- fornire il contesto per i low volume node.

### 4.3 LowVolumeNodeDetector

Documento: `src/modules/LowVolumeNodeDetector/README.md`

Scopo:

- individuare zone di volume scarso dentro l'impulso;
- non usare lookback generici;
- restituire location candidate per il trigger.

### 4.4 AggressionDetector

Documento: `src/modules/AggressionDetector/README.md`

Scopo:

- cercare big order/aggression solo dopo out-of-balance;
- validare cambio regime small orders → big orders;
- confermare continuation.

### 4.5 TradeManager

Documento: `src/modules/TradeManager/README.md`

Scopo:

- calcolare entry, stop e target;
- target primario = POC della balance precedente;
- stimare risk/reward prima del segnale.

### 4.6 ConfirmationLayer

Documento: `src/modules/ConfirmationLayer/README.md`

Scopo:

- usare absorption e CVD solo come conferme;
- non produrre trigger autonomi;
- migliorare qualità del setup già valido.

### 4.7 VisualRenderer

Documento: `src/modules/VisualRenderer/README.md`

Scopo:

- disegnare zone, POC, breakout, entry, stop e target;
- evitare clutter;
- riutilizzare drawing objects invece di ricrearli ogni barra.

---

## 5. Roadmap Globale

### Phase 0 — Base Pulita

Stato: completata.

- `FabioTrendFollowing.cs` è un placeholder compilabile.
- La logica del vecchio prototipo è stata rimossa.
- La documentazione è riorganizzata per moduli.

### Phase 1 — BalanceZoneTracker

Obiettivo:

```text
London profile → POC/VAH/VAL congelati → NY out-of-balance
```

Implementare solo questo modulo prima di procedere.

### Phase 2 — Impulse + Low Volume Node

Obiettivo:

```text
BreakoutBar → impulse profile → low volume node dell'impulso
```

### Phase 3 — Aggression + Trade Plan

Obiettivo:

```text
Aggression in location valida → entry/stop/target
```

### Phase 4 — Confirmation + Visual Polish

Obiettivo:

```text
Absorption/CVD come conferme + rendering leggibile
```

---

## 6. Decisioni Globali

| Area | Decisione |
|------|-----------|
| Timeframe operativo | M5 raccomandato, non bloccante |
| Balance reference | London session chiusa |
| Trading window | New York RTH |
| Value Area | 70% volume |
| POC | prezzo con volume massimo |
| VAH/VAL | espansione contigua dal POC |
| Breakout | 2 close consecutive fuori VAH/VAL |
| Timezone | `TimeZoneInfo`, no offset hardcoded |
| Parametri UI | minimi finché non validato visivamente |
| Codice iniziale | base pulita, no vecchi detector isolati |

---

## 7. Build & Deploy

Build:

```bash
cd "Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Modello-1-TrendFollowing/src"
dotnet build -c Release
```

Deploy manuale:

```bash
cp bin/Release/net10.0-windows/FabioTrendFollowing.dll "$APPDATA/ATAS/Indicators/"
```

Oppure:

```bash
deploy.bat
```

---

## 8. Fonti

Transcript:

```text
Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy)/Trading LIVE with the #1 Scalper in the WORLD (EXTREME Accuracy).txt
```

Documentazione ATAS:

```text
docs/atas/api/
```

Fonti online consultate:

- TradingView Volume Profile concepts: `https://www.tradingview.com/support/solutions/43000502040-volume-profile-indicators-basic-concepts/`
- Edgeful futures sessions: `https://www.edgeful.com/blog/posts/trading-sessions-explained`
- NexusFi multi-timeframe workflow: `https://nexusfi.com/a/platforms/multi-timeframe-analysis-workflow`

---

## 9. Regola di Aggiornamento

Quando si sviluppa un modulo:

1. aggiornare prima `src/modules/<Modulo>/README.md`;
2. implementare il codice del modulo;
3. aggiornare questa roadmap solo se cambia una decisione globale;
4. fare build Release;
5. validare visivamente su ATAS quando applicabile.
