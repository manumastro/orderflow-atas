# London MR Performance Snapshots

Questa directory contiene solo report del `LondonMeanReversionModel` corrente generati con:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save
```

Il report usa esclusivamente i marker `MR_*` correnti, deduplica per `SetupId` e considera realizzato soltanto il PnL di `[MR_EXIT]`. Il default e' `HISTORICAL`, per evitare di sommare replay e live sovrapposti.

```bash
python FabioOrderFlow/tools/report_mr_performance.py --execution-mode LIVE
```

`--execution-mode ALL` e' solo un inventario tecnico: non usarlo come performance totale quando replay e live coprono lo stesso periodo.

Per valutare il risultato netto servono anche:

```bash
python FabioOrderFlow/tools/report_mr_performance.py --save \
  --commission-round-turn <valuta_conto> \
  --slippage-points <punti_round_turn> \
  --point-value <valuta_conto_per_punto>
```

Senza almeno 30 trade su 10 sessioni complete, oppure senza costi configurati, il report non puo' promuovere le performance come accettabili.
