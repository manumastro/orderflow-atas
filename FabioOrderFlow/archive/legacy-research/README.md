# Legacy Research Archive

Questa directory conserva strumenti e snapshot della strategia precedente al core `LondonMeanReversionModel` corrente.

I file qui presenti usano marker ritirati come `MR_TRIGGER`, `MR_POSITION_CLOSED`, `DAY_STUDY_*`, target parziali e gestione 70/30. Non devono essere usati per validare il modello operativo attuale.

Per il modello corrente usare:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il PnL corrente viene sempre calcolato esclusivamente da `[MR_EXIT]`.
