# Come Si Scrive Una Strategia

**La fase 3 della metodologia** ([`la-metodologia-istituzionale.md`](la-metodologia-istituzionale.md#3-il-codice)):
le regole dentro una macchina, **un pezzo alla volta**, verificando ogni pezzo prima di aggiungere
il successivo `[MC · 1:29:50 - 1:31:02]`. Il modello linguistico e' uno strumento, non uno
sviluppatore senior `[MC · 1:29:30 - 1:29:50]`: qui il codice lo scrive l'agente, e l'utente deve
poterlo leggere e capire cosa fa `[MC · 1:31:02 - 1:31:43]`.

Il framework di questa fase e' il progetto [`quant/framework/Quant.Strategie/`](../framework/Quant.Strategie/):
la classe base `Strategia`, il `Contesto` che le mostra il mercato, la `Posizione` aperta. **Le
strategie sono il codice di un'idea**, e stanno nel loro studio: `quant/studi/<nome>/3-codice/`, un
file per strategia. Le compila il progetto `Quant.Studi`, e si registrano in
`framework/Quant.Studi/Catalogo.cs`.

## La Forma Di Una Strategia

Una strategia e' una classe che eredita da `Strategia` (`Strategia.cs`):

| membro | cosa dice |
|---|---|
| `Nome` | il nome nel catalogo, uguale al file della scheda (`momento-intraday`) |
| `Versione` | si alza ogni volta che cambia la logica: per il registro e' una variante nuova |
| `Regole` | le tre caselle in parole, con i valori dei parametri: il rapporto le stampa |
| `Idea` | il percorso della scheda, `quant/studi/<nome>/1-idea.md` |
| `FinestraIngresso` | da che ora a che ora (ET) puo' entrare: il modello nullo pesca nella stessa finestra |
| `ChiudeAFineGiornata` | se il motore chiude tutto sull'ultima barra della giornata di borsa |
| `Prepara(Parametri)` | legge i parametri, una volta, prima della prova |
| `NuovaGiornata(Contesto)` | prima barra di una giornata nuova |
| `SuBarra(Contesto)` | la barra e' chiusa: si decide qui, si esegue dalla barra dopo |

Poi si registra in `framework/Quant.Studi/Catalogo.cs`, e `quant strategie` la elenca.

## Il Contesto: Solo Il Passato

*Aggiunta del framework.* La strategia non vede la serie: vede il `Contesto`, che le mostra la barra
appena chiusa e le precedenti (`Indietro(k)`, `Storia`, `Oggi`), le giornate gia' chiuse
(`GiornoPrecedente(k)`, con k >= 1), la posizione e gli ordini. **Il futuro non c'e'**:
`Indietro(-1)` e `GiornoPrecedente(0)` sono un errore. Il look-ahead e' stato il primo errore della
fase 40R, e qui non si puo' scrivere.

Ogni dato precalcolato che si passa a una strategia deve dipendere solo dal passato.

## I Parametri

Ogni numero delle regole e' un parametro con un default (`p.Intero("minuti", 30)`). `Parametri`
(nella fase 2, perche' i parametri sono parte delle regole)
registra **ogni valore letto, anche il default**: cosi' il rapporto elenca la variante per intero, e
l'**impronta** (SHA-256 di nome, versione e parametri, i primi 10 caratteri) la identifica nel
registro. Un parametro passato e mai letto e' un errore: quasi sempre e' un refuso.

## Le Iterazioni

Come nell'esempio del transcript (long alle 16:00, poi l'uscita alle 9:30, poi la dimensione)
`[MC · 1:29:50 - 1:31:02]`:

1. **solo l'ingresso**, con un'uscita banale (a fine giornata); si lancia `quant prova` sul mercato
   sintetico e si controlla che le operazioni siano dove devono essere;
2. **l'uscita**: stop, target, tempo; si controlla che i motivi d'uscita nel rapporto siano quelli
   attesi;
3. **la dimensione**: prima un contratto, poi le altre regole;
4. **un test** in `quant/framework/Quant.Test/` per ogni comportamento che conta (un caso costruito a
   mano, con il risultato calcolato a mano).

Il mercato sintetico (`quant sintetico`, fase 5) serve qui come banco di prova: lavorarci sopra non
consuma i dati veri e non entra nel registro vero.

## Il Catalogo

| strategia | scheda | esempio del transcript |
|---|---|---|
| `momento-intraday` | [momento-intraday](../studi/momento-intraday/1-idea.md) | 1 |
| `vwap-tendenza` | [vwap-tendenza](../studi/vwap-tendenza/1-idea.md) | 2 |
| `anomalia-notte` | [anomalia-notte](../studi/anomalia-notte/1-idea.md) | 4 |

La PEAD (esempio 3) non c'e': lavora su azioni, utili e consenso degli analisti, e lo scope e' NQ.
