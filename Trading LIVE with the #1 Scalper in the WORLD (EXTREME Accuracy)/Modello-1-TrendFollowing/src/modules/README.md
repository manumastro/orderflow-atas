# Modules — Modello 1 Trend Following

Questa cartella contiene lo scheletro modulare del Modello 1.

Ogni modulo ha una cartella dedicata con un `README.md` operativo. Il codice C# del modulo verrà aggiunto nella stessa cartella quando la fase corrispondente sarà implementata.

## Ordine di Implementazione

1. `BalanceZoneTracker`
2. `ImpulseProfiler`
3. `LowVolumeNodeDetector`
4. `AggressionDetector`
5. `TradeManager`
6. `ConfirmationLayer`
7. `VisualRenderer`

## Regola

Prima di implementare o modificare un modulo:

1. leggere il `README.md` del modulo;
2. aggiornare il design se cambia una decisione;
3. implementare il codice nella stessa cartella;
4. verificare build Release;
5. aggiornare la roadmap centrale solo per decisioni globali.
