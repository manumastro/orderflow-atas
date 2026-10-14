# Il Vivo

**La fase 6 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#6-il-vivo)):
il vivo **e' automazione**, la macchina sorveglia i dati e apre e chiude le posizioni; il lavoro del
trader e' sorvegliare **il rendimento** e **il rischio**, e ricominciare il processo
`[MC · 18:53 - 20:31]`.

## Stato: Fuori Scope

Per decisione dell'utente (11 ottobre 2026) **si arriva fino alla validazione**: nessun ordine e
nessuna esecuzione automatica. L'esecuzione si progetta quando il framework e' maturo, su richiesta
esplicita. Questa cartella esiste per tenere il posto della fase, e non ha codice.

## Cosa C'e' Gia', Per Quando Servira'

- **I metri del vivo** li produce gia' la validazione: il drawdown p50/p95/p99 entro 10, 20, 50 e
  100 operazioni, nel rapporto di `quant valida` `[MC · 16:44 - 18:17]`. Sono la sorveglianza del
  rischio che il transcript chiede.
- **Lo stesso linguaggio di ATAS**: il framework e' in C#, e ATAS esegue strategie in C#. Una
  strategia validata qui potra' girare dentro ATAS senza essere riscritta in un altro linguaggio.
- **Il bridge con ATAS** legge gia' i dati dal vivo
  ([`quant/metodo/i-dati-da-atas.md`](i-dati-da-atas.md)); non
  manda ordini.
