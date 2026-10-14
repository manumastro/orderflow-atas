namespace Quant.Regole.Contratti;

/// <summary>
/// Una fase di un contratto di prop firm: la valutazione (con un target) o il conto finanziato
/// (senza target, con i prelievi). Tutti gli importi sono in dollari, rispetto al saldo iniziale.
///
/// I sei parametri di Tomas Fernandez (TF §2) sono qui: target, drawdown, livello di blocco del
/// pavimento, quando il pavimento si aggiorna, quando si controlla la violazione, limite giornaliero.
/// Le regole che non sono una barriera sul P&L cumulato (attivita' minima, strumenti, durata delle
/// operazioni) restano fuori, come in TF §2.8.
/// </summary>
public sealed record Fase
{
    public required string Nome { get; init; }

    /// <summary>Profitto da raggiungere per passare. Null nella fase finanziata.</summary>
    public double? Target { get; init; }

    /// <summary>Distanza massima del saldo dal suo massimo (il pavimento sta a massimo - drawdown).</summary>
    public required double Drawdown { get; init; }

    /// <summary>
    /// Kappa di TF: il livello, rispetto al saldo iniziale, a cui il pavimento smette di salire.
    /// Null: sale sempre. -Drawdown: il pavimento e' fisso. 0: si ferma al saldo iniziale.
    /// </summary>
    public double? Blocco { get; init; }

    /// <summary>Vero: il pavimento si ricalcola sul saldo di fine sessione (EOD). Falso: a ogni controllo.</summary>
    public bool PavimentoAllaChiusura { get; init; } = true;

    /// <summary>Se dato, il pavimento si ricalcola ogni N passi della sessione (per esempio ogni ora), oltre che alla chiusura.</summary>
    public int? PavimentoOgniPassi { get; init; }

    /// <summary>Vero: la violazione del pavimento si guarda solo a fine sessione. Falso: a ogni passo, in tempo reale.</summary>
    public bool ViolazioneAllaChiusura { get; init; }

    /// <summary>Perdita massima di una sessione. Morbido: si smette per la giornata. Rigido: il conto e' perso.</summary>
    public double? LimiteGiornaliero { get; init; }
    public bool LimiteRigido { get; init; }

    /// <summary>Il limite giornaliero sale a questo valore dalla sessione dopo quella in cui il saldo raggiunge <see cref="SogliaLimiteAlzato"/>.</summary>
    public double? LimiteGiornalieroAlzato { get; init; }
    public double? SogliaLimiteAlzato { get; init; }

    /// <summary>Sessioni a disposizione per passare. Null: nessuna scadenza.</summary>
    public int? Scadenza { get; init; }

    /// <summary>Le regole dei prelievi, solo nella fase finanziata.</summary>
    public Prelievi? Prelievi { get; init; }
}

/// <summary>
/// Come si preleva dal conto finanziato. Due forme: a richiesta, quando le condizioni sono soddisfatte
/// (Tradeify, Topstep), oppure a data fissa ogni <see cref="Intervallo"/> sessioni, prelevando tutto
/// il positivo e azzerando il saldo (il contratto stilizzato di Lim).
/// </summary>
public sealed record Prelievi
{
    /// <summary>Profitto sopra il saldo iniziale necessario per chiedere un prelievo.</summary>
    public double SaldoMinimo { get; init; }

    /// <summary>Sessioni con profitto oltre <see cref="ProfittoGiorno"/> necessarie dall'ultimo prelievo.</summary>
    public int GiorniMinimi { get; init; }
    public double ProfittoGiorno { get; init; }

    /// <summary>La sessione migliore dall'ultimo prelievo non puo' superare questa quota del profitto del periodo.</summary>
    public double? Consistenza { get; init; }

    /// <summary>Tetto per richiesta, per numero di prelievo; l'ultimo si ripete.</summary>
    public double[] Massimi { get; init; } = [];

    public double Minimo { get; init; }

    /// <summary>La richiesta di prelievo blocca il pavimento al livello di blocco della fase.</summary>
    public bool BloccaPavimento { get; init; }

    /// <summary>Prelievo a data fissa ogni N sessioni, di tutto il saldo positivo, che poi si azzera (Lim).</summary>
    public int? Intervallo { get; init; }

    /// <summary>Quota del prelievo che va al trader (profit split).</summary>
    public double Quota { get; init; } = 0.9;
}

/// <summary>I prezzi. Si usano sempre quelli effettivi alla data, con il listino accanto (TF §3.2).</summary>
public sealed record Prezzi
{
    /// <summary>Il prezzo pagato per la prima valutazione.</summary>
    public required double Ingresso { get; init; }

    /// <summary>Il prezzo di ogni tentativo dopo un fallimento.</summary>
    public required double Reset { get; init; }

    /// <summary>Pagato una volta, al passaggio.</summary>
    public double Attivazione { get; init; }

    /// <summary>Il prezzo d'ingresso si restituisce con il primo prelievo (Lim).</summary>
    public bool RimborsoAlPrimoPrelievo { get; init; }

    public double? Listino { get; init; }
    public string Nota { get; init; } = "";
}

/// <summary>Un contratto: una o piu' fasi di valutazione in serie, poi la fase finanziata (TF eq. 1).</summary>
public sealed record Contratto
{
    public required string Nome { get; init; }
    public required Fase[] Valutazione { get; init; }
    public required Fase Finanziata { get; init; }
    public required Prezzi Prezzi { get; init; }

    /// <summary>Da dove vengono le regole e quando sono state lette: le regole cambiano (TF §6).</summary>
    public required string Fonte { get; init; }
    public required DateOnly LetteIl { get; init; }

    public Contratto ConPrezzi(double? ingresso = null, double? reset = null) =>
        this with { Prezzi = Prezzi with { Ingresso = ingresso ?? Prezzi.Ingresso, Reset = reset ?? Prezzi.Reset } };
}
