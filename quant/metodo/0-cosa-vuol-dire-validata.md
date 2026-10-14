# Cosa Vuol Dire Validata

**La fase 0 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#0-la-differenza-fra-retail-e-istituzionale))
non e' un passo della pipeline: e' la premessa. Il retail insegue operazioni, l'istituzionale
insegue **strategie validate**, applicate sistematicamente ogni volta `[MC · 3:01 - 6:26]`. Una
singola operazione non e' mai garantita; su cento, la strategia dice quante se ne vincono.

Questa cartella non ha codice: tutto il framework realizza la fase 0. Questo documento dice **cosa
vuol dire "validata" in questo repository**, perche' il transcript non lo fissa con un criterio.

## Il Transcript E Il Framework

| | il transcript | il framework |
|---|---|---|
| validata | "buoni risultati dentro e fuori campione", "dispersione stretta" del Monte Carlo `[MC · 9:26 - 12:01]`, `[MC · 12:24 - 15:38]` | **supera i cancelli** D1-D3 e F1-F4, con soglie scritte prima di guardare: [`quant/metodo/5-come-si-valida.md`](5-come-si-valida.md#i-cancelli) |
| dal vivo | solo dopo divisione e Monte Carlo `[MC · 18:17 - 18:53]` | fuori scope (fase 6) |

## Cosa Non Vuol Dire

- **Validata non vuol dire approvata.** Superare i cancelli dice che la prova non ha smentito la
  strategia, non che la strategia va usata. Nessuna regola operativa e' approvata.
- **Validata non vuol dire garantita.** Il transcript lo dice per l'operazione singola
  `[MC · 5:58 - 6:26]`; vale anche per il mese e per l'anno: i metri del Monte Carlo dicono quanto
  si puo' perdere prima che sia un segnale d'allarme.
- **Validata non vuol dire per sempre.** Le strategie smettono di funzionare `[MC · 1:32:43 - 1:36:01]`:
  il registro tiene la storia di ogni prova, e la fase 7 dice di rivederle.

## Dove Stanno Le Fasi

Ogni fase ha la sua pagina in `quant/metodo/`, con lo stesso numero (`1-...`, `2-...`), e ogni
studio in `quant/studi/` segue le fasi nella sua cartella con gli stessi numeri. La mappa e' in
[`quant/il-metodo-quantitativo.md`](../il-metodo-quantitativo.md#dove-sta-cosa).
