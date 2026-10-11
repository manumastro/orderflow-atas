using Quant.Dati;
using Quant.Motore;

namespace Quant.Strategie;

/// <summary>
/// Momento intraday: la rottura del range della prima mezz'ora. Primo esempio del transcript
/// (Gao, Han, Li, Zhou, "Market Intraday Momentum", 2018): lo squilibrio accumulato nella notte
/// si rivela nei primi trenta minuti e il mercato non lo digerisce subito.
/// </summary>
public sealed class MomentoIntraday : Strategia
{
    public override string Nome => "momento-intraday";
    public override string? Idea => "quant/idee/momento-intraday.md";

    private int _inizio, _minuti, _uscita;
    private double _rr;
    private bool _soloLong;

    private int _massimo, _minimo, _barreRange;
    private bool _fatto;

    public override string Regole =>
        $"Ingresso: chiusura M1 sopra il massimo del range {Calendario.Orario(_inizio)}-{Calendario.Orario(_inizio + _minuti)} ET, long a mercato " +
        $"alla barra dopo{(_soloLong ? "; solo long" : "; sotto il minimo, short")}. Una operazione al giorno. " +
        $"Uscita: stop all'altro estremo del range, target {_rr:0.##} volte il rischio, a tempo alle {Calendario.Orario(_uscita)} ET. " +
        "Dimensione: dai parametri (in sviluppo un contratto).";

    public override (int Da, int A) FinestraIngresso => (_inizio + _minuti, _uscita);

    public override void Prepara(Parametri p)
    {
        _inizio = p.Orario("inizio", "09:30");
        _minuti = p.Intero("minuti", 30);
        _rr = p.Reale("rr", 1.0);
        _soloLong = p.Booleano("soloLong", true);
        _uscita = p.Orario("uscita", "15:30");
    }

    public override void NuovaGiornata(Contesto c)
    {
        _massimo = int.MinValue;
        _minimo = int.MaxValue;
        _barreRange = 0;
        _fatto = false;
    }

    public override void SuBarra(Contesto c)
    {
        var m = c.MinutoEt;
        var b = c.Barra;
        if (m >= _inizio && m < _inizio + _minuti)
        {
            _massimo = Math.Max(_massimo, b.Massimo);
            _minimo = Math.Min(_minimo, b.Minimo);
            _barreRange++;
            return;
        }
        // l'ultimo minuto prima dell'uscita non apre: l'ordine si eseguirebbe gia' all'ora di uscita
        if (_fatto || _barreRange == 0 || m < _inizio + _minuti || m >= _uscita - 1 || !c.Piatto || c.HaOrdine) return;

        if (b.Chiusura > _massimo)
        {
            c.Invia(new Ordine { Lato = Lato.Long, Stop = _minimo, TargetR = _rr, UscitaAlle = c.OggiAlle(_uscita), Motivo = "rottura del massimo" });
            _fatto = true;
        }
        else if (!_soloLong && b.Chiusura < _minimo)
        {
            c.Invia(new Ordine { Lato = Lato.Short, Stop = _massimo, TargetR = _rr, UscitaAlle = c.OggiAlle(_uscita), Motivo = "rottura del minimo" });
            _fatto = true;
        }
    }
}
