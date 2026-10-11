using Quant.Dati;
using Quant.Motore;

namespace Quant.Strategie;

/// <summary>
/// L'anomalia della notte: comprare alla chiusura della cassa e vendere all'apertura. Quarto
/// esempio del transcript (Cliff, Cooper, Gulen, "Return Differences between Trading and
/// Non-trading Hours: Like Night and Day", 2008): quasi tutto il rendimento degli indici arriva
/// a mercato chiuso. Con parte=giorno si misura il complemento, dalla 9:30 alle 16:00, per il
/// confronto che il paper fa.
///
/// L'uscita della notte e' alle 9:30 della data di calendario dopo (il lunedi', se si entra di
/// venerdi'); in un festivo esce alla prima barra disponibile dopo quell'ora.
/// </summary>
public sealed class AnomaliaNotte : Strategia
{
    public override string Nome => "anomalia-notte";
    public override string? Idea => "quant/idee/anomalia-notte.md";

    private int _entra, _esce;
    private bool _notte;
    private bool _fatto;

    public override bool ChiudeAFineGiornata => !_notte;

    public override string Regole => _notte
        ? $"Ingresso: long a mercato alle {Calendario.Orario(_entra)} ET. Uscita: alle {Calendario.Orario(_esce)} ET della giornata dopo. " +
          "Nessuno stop. Dimensione: dai parametri."
        : $"Il complemento: long alle {Calendario.Orario(_esce)} ET, uscita alle {Calendario.Orario(_entra)} ET dello stesso giorno. Nessuno stop.";

    public override void Prepara(Parametri p)
    {
        _entra = p.Orario("entra", "16:00");
        _esce = p.Orario("esce", "09:30");
        _notte = p.Testo("parte", "notte") switch
        {
            "notte" => true,
            "giorno" => false,
            var x => throw new ArgumentException($"parte={x}: attesa notte o giorno"),
        };
    }

    public override void NuovaGiornata(Contesto c) => _fatto = false;

    public override void SuBarra(Contesto c)
    {
        if (_fatto || !c.Piatto || c.HaOrdine) return;
        var m = c.MinutoEt;
        // si decide alla chiusura della barra che finisce all'ora d'ingresso: l'ordine si esegue all'ora esatta
        var ora = _notte ? _entra : _esce;
        if (m < ora - 1 || m >= ora + 30) return;
        long uscita;
        if (_notte)
        {
            var domani = c.Giorno.AddDays(1);
            while (domani.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) domani = domani.AddDays(1);
            uscita = c.Utc(domani, _esce);
        }
        else uscita = c.OggiAlle(_entra);
        c.Invia(new Ordine { Lato = Lato.Long, UscitaAlle = uscita, Motivo = _notte ? "chiusura della cassa" : "apertura della cassa" });
        _fatto = true;
    }
}
