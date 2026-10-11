# L'Anomalia Della Notte

**Stato**: codificata, non validata su dati veri
**Strategia nel catalogo**: `anomalia-notte`, versione 1

## Il Paper

Cliff, Cooper, Gulen, *Return Differences between Trading and Non-trading Hours: Like Night and
Day*, 2008 `[MC · 1:22:41 - 1:22:50]`. **Paper non ancora letto**.

## Il Finding

Su piu' indici e azioni, circa il **90% del rendimento** viene dal possesso a mercato chiuso:
la seduta regolare quasi piatta, il guadagno nel gap della notte `[MC · 1:22:50 - 1:23:43]`.
Secondo il transcript la stessa prova su NQ dal 2015 al giugno 2026 da' ancora circa 90% di notte e
10% di giorno `[MC · 1:23:55 - 1:24:38]`: **da rifare qui**, e' la prova piu' semplice del catalogo.

## Il Perche'

Due teorie, nessuna definitiva `[MC · 1:25:25 - 1:25:37]`:

1. **premio per il rischio della notte**: chi tiene la posizione a mercato chiuso, con meno
   liquidita', va pagato. Molti trovano il 90% troppo per spiegarlo cosi' `[MC · 1:25:44 - 1:26:28]`;
2. **liquidita' della notte**: le notizie arrivano quando il mercato e' sottile, la reazione e'
   eccessiva, e la seduta regolare la riporta verso il valore `[MC · 1:26:28 - 1:27:40]`.

## Cosa La Ucciderebbe

Una notte liquida quanto il giorno: se il grosso degli operatori potesse agire di notte, sparirebbe
sia la reazione eccessiva sia il premio per la poca liquidita'.

## Le Regole Da Provare

`[MC · 1:23:55 - 1:24:08]`:

- **ingresso**: long a mercato alle 16:00 ET;
- **uscita**: alle 9:30 ET della giornata dopo, da venerdi' a lunedi';
- **dimensione**: un contratto.

Con `parte=giorno` la stessa strategia misura il complemento, dalle 9:30 alle 16:00: il confronto
del paper.

**Attenzione al cancello D1.** Questa e' un'anomalia di esposizione: il nullo del quando, a parita'
di esposizione, ha poca potenza qui (sul mercato sintetico con una deriva piantata di notte D1 non
passa quasi mai). La prova giusta e' il confronto notte contro giorno. Vedi
[`../il-metodo-quantitativo.md`](../il-metodo-quantitativo.md).

Idea derivata `[MC · 1:28:06 - 1:28:31]`: un opening range breakout notturno.

## Le Prove

Nessuna su NQ: servono anni, l'archivio ha 50 giornate.
