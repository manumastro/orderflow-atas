# orderflow-atas - Agent Guide

## Stato

Il repository e' una base neutra per studiare il corso in `fabio_course/` e, solo in seguito, progettare un nuovo indicatore ATAS.

Non esiste un modello attivo. Non assumere che il futuro modello sia mean reversion, continuation o una combinazione predeterminata di tecniche.

## Fonte Attiva

Prima di formulare ipotesi o modificare il runtime, leggere integralmente:

```text
fabio_course/fabio1.txt
fabio_course/fabio2.txt
fabio_course/fabio3.txt
```

I vecchi transcript YouTube, i modelli precedenti e i relativi risultati non fanno parte della baseline corrente.

## Regole Di Lavoro

1. Studiare il corso come un sistema completo: contesto, regime d'asta, prezzo e valore, profilo, volume, partecipanti, timing, esecuzione e gestione.
2. Non trasformare una singola tecnica o frase del corso in un modello prima di aver definito come si collega agli altri elementi.
3. Separare sempre osservazione del mercato, interpretazione, ipotesi testabile e regola implementabile.
4. Dichiarare prima di ogni test domanda, dati necessari, criterio di promozione e criterio di scarto.
5. Evitare soglie ricavate dallo stesso campione usato per giudicarle; dichiarare esplicitamente ogni selezione post-hoc.
6. Non introdurre ordini reali, PnL o automazione operativa senza una richiesta esplicita e una validazione separata.
7. Mantenere il runtime neutro finche' non esiste un contratto di modello documentato e approvato.
8. Scrivere documentazione comprensibile sia a una persona sia a un agente: spiegare i termini tecnici alla prima occorrenza.
9. Conservare una sola fonte canonica per ogni decisione; rimuovere output intermedi non piu' utili.
10. Non modificare la documentazione API ATAS in `docs/atas/api/` salvo necessita' tecnica specifica.

## Struttura

```text
fabio_course/                 fonte didattica attiva
FabioOrderFlow/src/           scheletro indicatore ATAS
FabioOrderFlow/FabioOrderFlow.md  stato e procedure del progetto
docs/atas/api/                riferimento tecnico locale ATAS
```

## Build E Deploy

```bash
cd FabioOrderFlow/src
dotnet build -c Release
```

DLL:

```text
FabioOrderFlow/src/bin/Release/net10.0-windows/FabioOrderFlow.dll
%APPDATA%/ATAS/Indicators/FabioOrderFlow.dll
```
