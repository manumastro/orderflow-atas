# London MR Performance Snapshots

Questa directory contiene solo report del `LondonMeanReversionModel` corrente generati con:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report usa esclusivamente i marker `MR_*` correnti, deduplica per `SetupId` e considera realizzato soltanto il PnL di `[MR_EXIT]`.

Per valutare il risultato netto servono anche:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save \
  --commission-round-turn <valuta_conto> \
  --slippage-points <punti_round_turn> \
  --point-value <valuta_conto_per_punto>
```

Senza almeno 30 trade su 10 sessioni complete, oppure senza costi configurati, il report non puo' promuovere le performance come accettabili.
